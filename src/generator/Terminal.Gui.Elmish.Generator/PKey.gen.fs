module Terminal.Gui.Elmish.Generator.PKey_gen

open System
open System.IO

[<RequireQualifiedAccess>]
module CodeGen =

  let cleanTypeName (t: Type) =
    if t.Name.Contains("`") then t.Name.Substring(0, t.Name.IndexOf("`")) else t.Name

  let isNullable (t: Type) =
    t.IsGenericType && t.GetGenericTypeDefinition() = typedefof<Nullable<_>>

  let rec genericTypeParam (t: Type) =
    if isNullable t then
      let genericArg = t.GetGenericArguments().[0]
      $"%s{genericTypeParam genericArg} option"
    else if t.IsGenericType then
      let baseName = t.Name.Substring(0, t.Name.IndexOf('`'))
      $"{baseName}{genericTypeParams t}"
    else if t.IsGenericParameter then
      $"'{t.Name}"
    else if t.Name = "Boolean" then
      "bool"
    else if t.Name = "Int32" then
      "int"
    else if t.Name = "String" then
      "string"
    else
      t.Name

  and genericTypeParams (t: Type) =
    match String.concat ", " (t.GetGenericArguments() |> Array.map genericTypeParam) with
    | "" -> ""
    | args -> $"<{args}>"

  let genericConstraints (t: Type) =
    let constraints =
      t.GetGenericArguments()
      |> Array.choose (fun t ->
          let constraints = ResizeArray<string>()
          let attrs = t.GenericParameterAttributes

          if attrs.HasFlag(System.Reflection.GenericParameterAttributes.ReferenceTypeConstraint) then
            constraints.Add($"'{t.Name}: not struct")
          if attrs.HasFlag(System.Reflection.GenericParameterAttributes.NotNullableValueTypeConstraint) then
            constraints.Add($"'{t.Name}: struct")
          if attrs.HasFlag(System.Reflection.GenericParameterAttributes.DefaultConstructorConstraint) then
            constraints.Add($"'{t.Name}: (new: unit -> '{t.Name})")

          if constraints.Count > 0 then
            constraints
            |> String.concat " and "
            |> Some
          else
            None
      )
    if constraints.Length > 0 then
      " when " + (constraints |> String.concat " and ")
    else
      ""

  let asPKey (name: string) =
    name
    |> String.lowerCamelCase
    |> String.escapeReservedKeywords

type PKeyRegistry =
  static member val private Registry = System.Collections.Generic.Dictionary<string, string>()
  static member GetPKeySegment(viewType: Type) =
    match PKeyRegistry.Registry.TryGetValue(viewType.FullName) with
    | true, pkey -> pkey
    | _ ->
        let uniquePKey =
          let pkeyCandidate =
            viewType
            |> CodeGen.cleanTypeName
            |> String.lowerCamelCase
          let rec findUniquePKey candidate =
            if PKeyRegistry.Registry.ContainsValue candidate then
              findUniquePKey (candidate + "'")
            else
              candidate
          findUniquePKey pkeyCandidate
        PKeyRegistry.Registry.Add(viewType.FullName, uniquePKey)
        uniquePKey

let eventKeyType (event: System.Reflection.EventInfo) =
  let handlerType = event.EventHandlerType
  let genericArgs = handlerType.GetGenericArguments()
  if genericArgs.Length = 1 then
    $"IEventPropKey<{CodeGen.genericTypeParam genericArgs[0]} -> unit>"
  else if genericArgs.Length = 0 then
    let eventArgs = handlerType.GetMethod("Invoke").GetParameters().[1].ParameterType
    $"IEventPropKey<{CodeGen.genericTypeParam eventArgs} -> unit>"
  else
    raise (NotImplementedException())

let generatePKeyClass (viewType: Type) =
  seq {
    let cleanTypeName = CodeGen.cleanTypeName viewType
    // Remove generic suffix from type name for class name
    let className = String.lowerCamelCase cleanTypeName
    let parentType = ViewType.parentViewType viewType

    yield $"  // {cleanTypeName}"

    // Handle generic types properly
    if viewType.IsGenericType then
      let genericParams = viewType.GetGenericArguments() |> Array.map (fun t -> $"'{t.Name}") |> String.concat ", "
      let genericConstraints = CodeGen.genericConstraints viewType

      yield $"  type {className}PKeys<{genericParams}{genericConstraints}>() ="
    else
      yield $"  type {className}PKeys() ="

    // Handle inheritance
    let isViewBaseType = viewType.Name = "View" || viewType.FullName.Contains("Terminal.Gui.ViewBase.View")

    if not isViewBaseType then
      match parentType with
      | Some parent when parent.Name <> "View" && not (parent.FullName.Contains("Terminal.Gui.ViewBase.View")) ->
          let parentName = if parent.Name.Contains("`") then parent.Name.Substring(0, parent.Name.IndexOf("`")) else parent.Name
          let parentClassName = String.lowerCamelCase parentName
          if parent.IsGenericType then
            let genericParams = parent.GetGenericArguments() |> Array.map (fun t -> $"'{t.Name}") |> String.concat ", "
            yield $"    inherit {parentClassName}PKeys<{genericParams}>()"
          else
            yield $"    inherit {parentClassName}PKeys()"
      | _ ->
          yield $"    inherit viewPKeys()"

    // For View base type, add children property
    if isViewBaseType then
      yield $"    member val children: ISimplePropKey<System.Collections.Generic.List<IInternalTerminalElement>> = PropKey.Create.simple \"children\""
      yield ""

    let props = ViewType.properties viewType
    let evts = ViewType.events viewType

    if props.Length > 0 then
      yield "    // Properties"
      for prop in props do
        let propName = prop.Name |> CodeGen.asPKey
        let keyName = $"{className}.{String.lowerCamelCase prop.Name}"

        // Check if this is a delayed pos property
        if prop.Name = "X" || prop.Name = "Y" then
          yield $"    member val {propName}: ISimplePropKey<Pos> = PropKey.Create.simple \"{keyName}\""
          yield $"    member val {propName}_delayedPos: IDelayedPosKey = PropKey.Create.delayedPos \"{keyName}_delayedPos\""
        else
          yield $"    member val {propName}: ISimplePropKey<{CodeGen.genericTypeParam prop.PropertyType}> = PropKey.Create.simple \"{keyName}\""

    if evts.Length > 0 then
      if props.Length > 0 then yield ""
      yield "    // Events"
      for event in evts do
        let eventName = event.Name |> CodeGen.asPKey
        let keyName = $"{className}.{String.lowerCamelCase event.Name}_event"
        let eventType = eventKeyType event
        yield $"    member val {eventName}: {eventType} = PropKey.Create.event \"{keyName}\""

    yield ""
  }

let generateModuleInstances () =
  seq {
    for viewType in ViewType.viewTypesOrderedByInheritance do
      let typeName = viewType.Name
      let cleanTypeName = if typeName.Contains("`") then typeName.Substring(0, typeName.IndexOf("`")) else typeName
      let className = String.lowerCamelCase cleanTypeName
      let viewName = PKeyRegistry.GetPKeySegment viewType

      // Check if it's a generic type
      if viewType.IsGenericType then
        let genericParams = viewType.GetGenericArguments() |> Array.map (fun t -> $"'{t.Name}") |> String.concat ", "
        let genericConstraints = CodeGen.genericConstraints viewType
        yield $"  let {viewName}<{genericParams}{genericConstraints}> = {className}PKeys<{genericParams}>()"
      else
        yield $"  let {viewName} = {className}PKeys ()"
  }

let generateInterfaceKeys (interfaceType: Type) =
  let props = ViewType.properties interfaceType
  let evts = ViewType.events interfaceType

  // Skip interfaces with no properties or events
  if props.Length = 0 && evts.Length = 0 then
    Seq.empty
  else
    seq {
      let interfaceName = interfaceType.Name
      let cleanName = if interfaceName.StartsWith("I") then interfaceName.Substring(1) else interfaceName
      let moduleName = $"{String.lowerCamelCase cleanName}Interface"

      yield $"  // {interfaceName}"
      yield $"  module internal {moduleName} ="

      if props.Length > 0 then
        yield "    // Properties"
        for prop in props do
          let propName = String.lowerCamelCase prop.Name
          let propType = prop.PropertyType
          yield $"    let {propName}: ISimplePropKey<{propType}> = PropKey.Create.simple \"{moduleName}.{propName}\""

      if evts.Length > 0 then
        if props.Length > 0 then ()
        yield "    // Events"
        for event in evts do
          let eventName = String.lowerCamelCase event.Name
          let eventType = eventKeyType event
          yield $"    let {eventName}: {eventType} = PropKey.Create.event \"{moduleName}.{eventName}_event\""
    }

let gen () =
  let outputPath = Path.Combine (Environment.CurrentDirectory, "PKey.gen.fs")

  // Get all interfaces from Terminal.Gui that we need to handle
  let interfaces =
    typeof<Terminal.Gui.ViewBase.View>.Assembly.GetTypes()
    |> Array.filter (fun t ->
      t.IsInterface &&
      t.Namespace = "Terminal.Gui.ViewBase" &&
      t.Name.StartsWith("I") &&
      t.Name <> "IApplication" &&
      t.Name <> "IDesignTimeProperties")
    |> Array.sortBy (fun t -> t.Name)

  seq {
    yield "namespace Terminal.Gui.Elmish"
    yield ""
    yield "open System"
    yield "open System.Collections.Generic"
    yield "open System.Collections.Specialized"
    yield "open System.Text"
    yield "open System.Drawing"
    yield "open System.ComponentModel"
    yield "open System.IO"
    yield "open System.Globalization"
    yield "open Terminal.Gui.App"
    yield "open Terminal.Gui.Drawing"
    yield "open Terminal.Gui.Drivers"
    yield ""
    yield "open Terminal.Gui.FileServices"
    yield "open Terminal.Gui.Input"
    yield ""
    yield "open Terminal.Gui.Text"
    yield ""
    yield "open Terminal.Gui.ViewBase"
    yield ""
    yield "open Terminal.Gui.Views"
    yield ""
    yield "/// Properties key index"
    yield "[<RequireQualifiedAccess>]"
    yield "module internal PKey ="
    yield ""

    // Generate all types - ViewTypes.orderedByInheritance already includes View first
    for viewType in ViewType.viewTypesOrderedByInheritance do
      yield! generatePKeyClass viewType

    // Generate interface keys
    for interfaceType in interfaces do
      yield! generateInterfaceKeys interfaceType
    yield ""

    // Generate module instances
    yield! generateModuleInstances ()
  }
  |> String.concat Environment.NewLine
  |> File.writeAllText outputPath
