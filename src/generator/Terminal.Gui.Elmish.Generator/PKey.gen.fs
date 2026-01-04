module Terminal.Gui.Elmish.Generator.PKey

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
          let rec findUniquePKey candidate =
            if PKeyRegistry.Registry.ContainsValue candidate then
              findUniquePKey (candidate + "'")
            else
              candidate
          findUniquePKey pkeyCandidate
        PKeyRegistry.Registry.Add(viewType.FullName, uniquePKey)
        uniquePKey

let generatePKeyClass (viewType: Type) =
  seq {
    let className = ViewType.cleanTypeName viewType

    yield $"  // {className}"

    if viewType.IsGenericType then
      yield $"  type {className}PKeys{ViewType.genericTypeParamsWithConstraintsBlock viewType}() ="
    else
      yield $"  type {className}PKeys() ="

    if viewType = typeof<Terminal.Gui.ViewBase.View> then
      yield $"    member val children: ISimplePropKey<System.Collections.Generic.List<IInternalTerminalElement>> = PropKey.Create.simple \"children\""
      yield ""
    else
      let parentViewType = ViewType.parentViewType viewType
      let parentName = if parentViewType.Name.Contains("`") then parentViewType.Name.Substring(0, parentViewType.Name.IndexOf("`")) else parentViewType.Name
      if parentViewType.IsGenericType then
        let genericParams = parentViewType.GetGenericArguments() |> Array.map (fun t -> $"'{t.Name}") |> String.concat ", "
        yield $"    inherit {parentName}PKeys<{genericParams}>()"
      else
        yield $"    inherit {parentName}PKeys()"

    let view = ViewType.decompose viewType

    if view.Properties.Length > 0 then
      yield "    // Properties"
      for prop in view.Properties do
        let keyName = $"{className}.{prop.PKey}"

        // Check if this is a delayed pos property
        if prop.PKey = "X" || prop.PKey = "Y" then
          yield $"    member val {prop.PKey}: ISimplePropKey<Pos> = PropKey.Create.simple \"{keyName}\""
          yield $"    member val {prop.PKey}_delayedPos: IDelayedPosKey = PropKey.Create.delayedPos \"{keyName}_delayedPos\""
        else
          yield $"    member val {prop.PKey}: ISimplePropKey<{ViewType.genericTypeParam prop.PropertyInfo.PropertyType}> = PropKey.Create.simple \"{keyName}\""

    if view.Events.Length > 0 then
      if view.Properties.Length > 0 then yield ""
      yield "    // Events"
      for event in view.Events do
        let keyName = $"{className}.{event.PKey}_event"
        let handlerType = ViewType.eventHandlerType event.EventInfo
        yield $"    member val {event.PKey}: IEventPropKey<{handlerType}> = PropKey.Create.event \"{keyName}\""

    yield ""
  }

let generateModuleInstances () =
  seq {
    for viewType in ViewType.viewTypesOrderedByInheritance do
      let typeName = viewType.Name
      let className = if typeName.Contains("`") then typeName.Substring(0, typeName.IndexOf("`")) else typeName
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
      let moduleName = $"{cleanName}Interface"

      yield $"  // {interfaceName}"
      yield $"  module internal {moduleName} ="

      if i.Properties.Length > 0 then
        yield "    // Properties"
        for prop in i.Properties do
          yield $"    let {prop.PKey}: ISimplePropKey<{prop.PropertyInfo.PropertyType}> = PropKey.Create.simple \"{moduleName}.{prop.PKey}\""

      if i.Events.Length > 0 then
        yield "    // Events"
        for event in i.Events do
          let handlerType = ViewType.eventHandlerType event.EventInfo
          yield $"    let {event.PKey}: IEventPropKey<{handlerType}> = PropKey.Create.event \"{moduleName}.{event.PKey}_event\""
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
