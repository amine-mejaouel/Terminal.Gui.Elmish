module Terminal.Gui.Elmish.Generator.PKey

open System

let genPKeyClassDefinition (viewType: Type) =
  seq {
    let className = getTypeNameWithoutArity viewType

    yield $"  type {className}PKeys{genericTypeParamsWithConstraintsBlock viewType}() ="

    if viewType <> typeof<Terminal.Gui.ViewBase.View> then
      let parentViewType = ViewType.parentView viewType
      let parentName = getTypeNameWithoutArity parentViewType
      yield $"    inherit {parentName}PKeys{genericTypeParamsBlock parentViewType}()"
    else
      yield "    member val children: ISimplePropKey<List<IInternalTerminalElement>> = PropKey.Create.simple \"children\""

    yield ""

    let view = ViewMetadata.create viewType

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
          yield $"    member val {prop.PKey}: IViewPropKey<{getFSharpTypeName prop.PropertyInfo.PropertyType}> = PropKey.Create.view \"{keyName}_view\""
        // TODO: isEnumerableOfViews does not seem to be used anywhere
        else if isEnumerableOfViews then
          yield $"    member val {prop.PKey}: IMultiViewPropKey<System.Collections.Generic.List<{getFSharpTypeName prop.PropertyInfo.PropertyType}>> = PropKey.Create.multiView \"{keyName}_views\""
        else
          yield $"    member val {prop.PKey}: ISimplePropKey<{getFSharpTypeName prop.PropertyInfo.PropertyType}> = PropKey.Create.simple \"{keyName}\""

        // Extra PKeys for properties that are Views or collections of Views
        if isViewProperty then
          let interfaceName = Registry.TEInterfaces.CreateTEInterface(prop.PropertyInfo.PropertyType)
          yield $"    member val {prop.PKey}_element: ISingleElementPropKey<{interfaceName}> = PropKey.Create.singleElement \"{keyName}_element\""
        else if prop.PropertyInfo.PropertyType.IsAssignableTo typeof<System.Collections.IEnumerable> then
          if isEnumerableOfViews then
            yield $"    member val {prop.PKey}_elements: IMultiElementPropKey<System.Collections.Generic.List<IInternalTerminalElement>> = PropKey.Create.multiElement \"{keyName}_elements\""

    if view.Events.Length > 0 then
      if view.Properties.Length > 0 then yield ""
      yield "    // Events"
      for event in view.Events do
        let keyName = $"{className}.{event.PKey}_event"
        let handlerType = eventHandlerType event.EventInfo
        yield $"    member val {event.PKey}: IEventPropKey<{handlerType}> = PropKey.Create.event \"{keyName}\""

    yield ""
  }

let genModuleInstances () =
  seq {
    for viewType in ViewType.viewTypesOrderedByInheritance do
      let typeName = viewType.Name
      let className = if typeName.Contains("`") then typeName.Substring(0, typeName.IndexOf("`")) else typeName
      let viewName = Registry.Views.GetUniqueTypeName viewType

      // Check if it's a generic type
      if viewType.IsGenericType then
        let genericParams = viewType.GetGenericArguments() |> Array.map (fun t -> $"'{t.Name}") |> String.concat ", "
        let genericConstraints = genericConstraints viewType
        yield $"  let {viewName}<{genericParams}{genericConstraints}> = {className}PKeys<{genericParams}>()"
      else
        yield $"  let {viewName} = {className}PKeys ()"
  }

let genInterfaceKeys (interfaceType: Type) =
  let i = ViewMetadata.create interfaceType

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
          let handlerType = eventHandlerType event.EventInfo
          yield $"    let {event.PKey}: IEventPropKey<{handlerType}> = PropKey.Create.event \"{moduleName}.{event.PKey}_event\""
    }

let opens = [
    "open System"
    "open System.Collections.Generic"
    "open System.Collections.Specialized"
    "open System.Text"
    "open System.Drawing"
    "open System.ComponentModel"
    "open System.IO"
    "open System.Globalization"
    "open Terminal.Gui.App"
    "open Terminal.Gui.Drawing"
    "open Terminal.Gui.Drivers"
    "open Terminal.Gui.FileServices"
    "open Terminal.Gui.Input"
    "open Terminal.Gui.Text"
    "open Terminal.Gui.ViewBase"
    "open Terminal.Gui.Views"
  ]

let gen () =

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
      yield! genPKeyClassDefinition viewType

    for interfaceType in interfaces do
      yield! genInterfaceKeys interfaceType
    yield ""

    yield! genModuleInstances ()
  }
  |> CodeWriter.write "PKey.gen.fs"
