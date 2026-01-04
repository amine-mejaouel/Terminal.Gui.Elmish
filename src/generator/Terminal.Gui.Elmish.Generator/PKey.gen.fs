module Terminal.Gui.Elmish.Generator.PKey_gen

open System
open System.IO



type PKeyRegistry =
  static member val private Registry = System.Collections.Generic.Dictionary<string, string>()
  static member GetPKeySegment(viewType: Type) =
    match PKeyRegistry.Registry.TryGetValue(viewType.FullName) with
    | true, pkey -> pkey
    | _ ->
        let uniquePKey =
          let pkeyCandidate =
            viewType
            |> ViewType.cleanTypeName
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
    $"IEventPropKey<{ViewType.genericTypeParam genericArgs[0]} -> unit>"
  else if genericArgs.Length = 0 then
    let eventArgs = handlerType.GetMethod("Invoke").GetParameters().[1].ParameterType
    $"IEventPropKey<{ViewType.genericTypeParam eventArgs} -> unit>"
  else
    raise (NotImplementedException())

let generatePKeyClass (viewType: Type) =
  seq {
    let cleanTypeName = ViewType.cleanTypeName viewType
    // Remove generic suffix from type name for class name
    let className = String.lowerCamelCase cleanTypeName
    let parentType = ViewType.parentViewType viewType

    yield $"  // {cleanTypeName}"

    // Handle generic types properly
    if viewType.IsGenericType then
      yield $"  type {className}PKeys{ViewType.genericTypeParamsWithConstraintsBlock viewType}() ="
    else
      yield $"  type {className}PKeys() ="

    // Handle inheritance
    let isViewBaseType = viewType = typeof<Terminal.Gui.ViewBase.View>

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

    let view = ViewType.decompose viewType

    if view.Properties.Length > 0 then
      yield "    // Properties"
      for prop in view.Properties do
        let keyName = $"{className}.{String.lowerCamelCase prop.PropertyInfo.Name}"

        // Check if this is a delayed pos property
        if prop.PropertyInfo.Name = "X" || prop.PropertyInfo.Name = "Y" then
          yield $"    member val {prop.PKey}: ISimplePropKey<Pos> = PropKey.Create.simple \"{keyName}\""
          yield $"    member val {prop.PKey}_delayedPos: IDelayedPosKey = PropKey.Create.delayedPos \"{keyName}_delayedPos\""
        else
          yield $"    member val {prop.PKey}: ISimplePropKey<{ViewType.genericTypeParam prop.PropertyInfo.PropertyType}> = PropKey.Create.simple \"{keyName}\""

    if view.Events.Length > 0 then
      if view.Properties.Length > 0 then yield ""
      yield "    // Events"
      for event in view.Events do
        let keyName = $"{className}.{event.PKey}_event"
        let eventType = eventKeyType event.EventInfo
        yield $"    member val {event.PKey}: {eventType} = PropKey.Create.event \"{keyName}\""

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
        let genericConstraints = ViewType.genericConstraints viewType
        yield $"  let {viewName}<{genericParams}{genericConstraints}> = {className}PKeys<{genericParams}>()"
      else
        yield $"  let {viewName} = {className}PKeys ()"
  }

let generateInterfaceKeys (interfaceType: Type) =
  let i = ViewType.decompose interfaceType

  // Skip interfaces with no properties or events
  if i.HasNoEventsOrProperties then
    Seq.empty
  else
    seq {
      let interfaceName = interfaceType.Name
      let cleanName = if interfaceName.StartsWith("I") then interfaceName.Substring(1) else interfaceName
      let moduleName = $"{String.lowerCamelCase cleanName}Interface"

      yield $"  // {interfaceName}"
      yield $"  module internal {moduleName} ="

      if i.Properties.Length > 0 then
        yield "    // Properties"
        for prop in i.Properties do
          yield $"    let {prop.PKey}: ISimplePropKey<{prop.PropertyInfo.PropertyType}> = PropKey.Create.simple \"{moduleName}.{prop.PKey}\""

      if i.Events.Length > 0 then
        yield "    // Events"
        for event in i.Events do
          let eventType = eventKeyType event.EventInfo
          yield $"    let {event.PKey}: {eventType} = PropKey.Create.event \"{moduleName}.{event.PKey}_event\""
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
