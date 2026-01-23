module Terminal.Gui.Elmish.Generator.PKey

open System
open System.IO


let generatePKeyClass (viewType: Type) =
  seq {
    let className = ViewType.typeNameWithoutArity viewType

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

    let view = ViewType.analyzeViewType viewType

    if view.Properties.Length > 0 then
      yield "    // Properties"
      for prop in view.Properties do
        let keyName = $"{className}.{prop.PKey}"

        let isViewProperty = prop.PropertyInfo.PropertyType.IsAssignableTo typeof<Terminal.Gui.ViewBase.View>
        let isEnumerableOfViews =
          let propertyType = prop.PropertyInfo.PropertyType
          if propertyType.IsGenericType && propertyType.IsAssignableTo typeof<System.Collections.IEnumerable> then
            let genericArg = propertyType.GetGenericArguments().[0]
            genericArg.IsAssignableTo typeof<Terminal.Gui.ViewBase.View>
          else
            false

        // Check if this is a delayed pos property
        if prop.PKey = "X" || prop.PKey = "Y" then
          yield $"    member val {prop.PKey}: ISimplePropKey<Pos> = PropKey.Create.simple \"{keyName}\""
          yield $"    member val {prop.PKey}_delayedPos: IDelayedPosKey = PropKey.Create.delayedPos \"{keyName}_delayedPos\""
        else if isViewProperty then
          yield $"    member val {prop.PKey}: IViewPropKey<{ViewType.genericTypeParam prop.PropertyInfo.PropertyType}> = PropKey.Create.view \"{keyName}_view\""
        // TODO: isEnumerableOfViews does not seem to be used anywhere
        else if isEnumerableOfViews then
          yield $"    member val {prop.PKey}: IMultiViewPropKey<System.Collections.Generic.List<{ViewType.genericTypeParam prop.PropertyInfo.PropertyType}>> = PropKey.Create.multiView \"{keyName}_views\""
        else
          yield $"    member val {prop.PKey}: ISimplePropKey<{ViewType.genericTypeParam prop.PropertyInfo.PropertyType}> = PropKey.Create.simple \"{keyName}\""

        // Extra PKeys for properties that are Views or collections of Views
        if isViewProperty then
          let interfaceName = Registry.SetNeededIElementInterface(prop.PropertyInfo.PropertyType)
          yield $"    member val {prop.PKey}_element: ISingleElementPropKey<{interfaceName}> = PropKey.Create.singleElement \"{keyName}_element\""
        else if prop.PropertyInfo.PropertyType.IsAssignableTo typeof<System.Collections.IEnumerable> then
          if isEnumerableOfViews then
            yield $"    member val {prop.PKey}_elements: IMultiElementPropKey<System.Collections.Generic.List<IInternalTerminalElement>> = PropKey.Create.multiElement \"{keyName}_elements\""

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
      let viewName = Registry.GetUniqueTypeName viewType

      // Check if it's a generic type
      if viewType.IsGenericType then
        let genericParams = viewType.GetGenericArguments() |> Array.map (fun t -> $"'{t.Name}") |> String.concat ", "
        let genericConstraints = ViewType.genericConstraints viewType
        yield $"  let {viewName}<{genericParams}{genericConstraints}> = {className}PKeys<{genericParams}>()"
      else
        yield $"  let {viewName} = {className}PKeys ()"
  }

let generateInterfaceKeys (interfaceType: Type) =
  let i = ViewType.analyzeViewType interfaceType

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

let opens =
  seq {
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
    yield! opens
    yield ""
    yield "[<RequireQualifiedAccess>]"
    yield "module internal PKey ="
    yield ""

    for viewType in ViewType.viewTypesOrderedByInheritance do
      yield! generatePKeyClass viewType

    for interfaceType in interfaces do
      yield! generateInterfaceKeys interfaceType
    yield ""

    yield! generateModuleInstances ()
  }
  |> String.concat Environment.NewLine
  |> File.writeAllText outputPath
