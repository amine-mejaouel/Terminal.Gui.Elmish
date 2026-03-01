module Terminal.Gui.Elmish.Generator.PKey

open System
open Terminal.Gui.Elmish.Generator.TypeExtensions

let genPKeyClassDefinition (viewType: Type) =
  seq {
    let className = getTypeNameWithoutArity viewType

    yield $"  type {className}PKeys{genericTypeParamsWithConstraintsBlock viewType}() ="

    if viewType <> typeof<Terminal.Gui.ViewBase.View> then
      let parentViewType = viewType.ParentViewType
      let parentName = getTypeNameWithoutArity parentViewType
      yield $"    inherit {parentName}PKeys{genericTypeParamsBlock parentViewType}()"

    yield ""

    let view = ViewMetadata.create viewType

    if view.Properties.Length > 0 then
      yield "    // Properties"
      for prop in view.Properties do
        let keyName = $"{className}.{prop.PKey}"

        // Check if this is a delayed pos property
        if prop.IsViewProperty then
          yield $"    member val {prop.PKey}: TypedPropKey<{prop.FSharpTypeName}> = PropKey.Create.view \"{keyName}_view\""
          let interfaceName = Registry.TEInterfaces.CreateInterface(prop.PropertyInfo.PropertyType)
          yield $"    member val {prop.PKey}_element: TypedPropKey<{interfaceName}> = PropKey.Create.singleElement \"{keyName}_element\""
        // TODO: isEnumerableOfViews does not seem to be used anywhere
        else if prop.IsEnumerableOfViews then
          yield $"    member val {prop.PKey}: TypedPropKey<List<{prop.FSharpTypeName}>> = PropKey.Create.view \"{keyName}_views\""
          yield $"    member val {prop.PKey}_elements: TypedPropKey<System.Collections.Generic.List<IViewTerminalElement>> = PropKey.Create.multiElement \"{keyName}_elements\""
        else
          yield $"    member val {prop.PKey}: TypedPropKey<{prop.FSharpTypeName}> = PropKey.Create.simple \"{keyName}\""

    if view.Events.Length > 0 then
      if view.Properties.Length > 0 then yield ""
      yield "    // Events"
      for event in view.Events do
        let keyName = $"{className}.{event.PKey}_event"
        let handlerType = eventHandlerType event.EventInfo
        yield $"    member val {event.PKey}: TypedPropKey<{handlerType}> = PropKey.Create.event \"{keyName}\""

    yield ""
  }

let genPKeysAccessors () =
  seq {
    for viewType in Registry.ViewTypes.orderedByInheritance do
      let viewName = Registry.ViewTypes.GetUniqueTypeName viewType
      yield $"  let {viewName}{genericTypeParamsWithConstraintsBlock viewType} = {getTypeNameWithoutArity viewType}PKeys{genericTypeParamsBlock viewType}()"
  }

let getAccessor (viewType: Type) =
  let viewName = Registry.ViewTypes.GetUniqueTypeName viewType
  $"PKey.{viewName}{genericTypeParamsBlock viewType}"

// TODO: code generated here is not currently used anywhere
let genInterfaceKeys (interfaceType: Type) =
  let i = ViewMetadata.create interfaceType

  // Skip interfaces with no properties or events
  if i.HasNoEventsOrProperties then
    Seq.empty
  else
    seq {
      let moduleName = $"{(getTypeNameWithoutArity interfaceType)}Interface"

      yield $"  module internal {moduleName} ="

      if i.Properties.Length > 0 then
        yield "    // Properties"
        for prop in i.Properties do
          yield $"    let {prop.PKey}{genericTypeParamsBlock interfaceType}: TypedPropKey<{getFSharpTypeName prop.PropertyInfo.PropertyType}> = PropKey.Create.simple \"{moduleName}.{prop.PKey}\""
          yield ""

      if i.Events.Length > 0 then
        yield "    // Events"
        for event in i.Events do
          let handlerType = eventHandlerType event.EventInfo
          yield $"    let {event.PKey}{genericTypeParamsBlock interfaceType}: TypedPropKey<{handlerType}> = PropKey.Create.event \"{moduleName}.{event.PKey}_event\""
          yield ""
    }

let opens = [
    "open System"
    "open System.Collections.Generic"
    "open Terminal.Gui.App"
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

    for viewType in Registry.ViewTypes.orderedByInheritance do
      yield! genPKeyClassDefinition viewType

    for interfaceType in interfaces do
      yield! genInterfaceKeys interfaceType
    yield ""

    yield! genPKeysAccessors ()
  }
  |> CodeWriter.write "PKey.gen.fs"
