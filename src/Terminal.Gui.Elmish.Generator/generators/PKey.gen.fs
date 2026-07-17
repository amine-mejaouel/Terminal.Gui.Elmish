module Terminal.Gui.Elmish.Generator.PKey

open System
open Terminal.Gui.Elmish.Generator.TypeExtensions

let private propertyIdExpression value = $"PropertyId.Create({value})"

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
          let viewId = Registry.PropertyIds.ViewProperty(viewType, prop.PKey)
          let viewSpecId = Registry.PropertyIds.ViewSpecProperty(viewType, prop.PKey)
          let viewPropertyId = propertyIdExpression viewId
          let viewSpecPropertyId = propertyIdExpression viewSpecId

          yield
            $"    member val {prop.PKey}: PropKey<{prop.FSharpTypeName}> = PropKey.Create.view({viewPropertyId}, {viewSpecPropertyId}, \"{keyName}_view\")"

          let interfaceName =
            Registry.ViewInterfaces.CreateInterface(prop.PropertyInfo.PropertyType)

          yield
            $"    member val {prop.PKey}_viewSpec: PropKey<{interfaceName}> = PropKey.Create.subElement({viewPropertyId}, {viewSpecPropertyId}, \"{keyName}_viewSpec\")"
        else
          let propertyId = Registry.PropertyIds.Property(viewType, prop.PKey)
          let propertyId = propertyIdExpression propertyId

          yield
            $"    member val {prop.PKey}: PropKey<{prop.FSharpTypeName}> = PropKey.Create.simple({propertyId}, \"{keyName}\")"

    if view.Events.Length > 0 then
      if view.Properties.Length > 0 then
        yield ""

      yield "    // Events"

      for event in view.Events do
        let keyName = $"{className}.{event.PKey}_event"
        let handlerType = eventHandlerType event.EventInfo
        let propertyId = Registry.PropertyIds.Event(viewType, event.PKey)
        let propertyId = propertyIdExpression propertyId

        yield $"    member val {event.PKey}: PropKey<{handlerType}> = PropKey.Create.event({propertyId}, \"{keyName}\")"

    yield ""
  }

let genPKeysAccessors () =
  seq {
    for viewType in Registry.ViewTypes.orderedByInheritance do
      let viewName = Registry.ViewTypes.GetUniqueTypeName viewType

      yield
        $"  let {viewName}{genericTypeParamsWithConstraintsBlock viewType} = {getTypeNameWithoutArity viewType}PKeys{genericTypeParamsBlock viewType}()"
  }

let private eventIdentity (declaringType: Type) (event: EventMetadata) =
  $"{declaringType.FullName}.{event.EventInfo.Name}"

/// Current type & base types
let private behavioralTypes (viewType: Type) =
  let rec collect current result =
    if isNull current || current = typeof<Terminal.Gui.ViewBase.View> then
      result
    else
      collect current.BaseType (current :: result)

  collect viewType []

let genReconciledEventKeys () =
  seq {
    yield "  module ReconciledEventKeys ="

    for definition in Registry.ReconciledEventSets.All do
      let declaredEvents =
        behavioralTypes definition.ViewType
        |> List.collect (fun declaringType ->
          (ViewMetadata.create declaringType).Events
          |> Array.map (fun event -> declaringType, event)
          |> List.ofArray)

      let availableIdentities =
        declaredEvents
        |> Seq.map (fun (declaringType, event) -> eventIdentity declaringType event)
        |> Set.ofSeq

      let missingExclusions = Set.difference definition.ExcludedEvents availableIdentities

      if not missingExclusions.IsEmpty then
        let missing = String.concat ", " missingExclusions

        invalidOp $"Reconciled event set '{definition.Name}' excludes events that do not exist: {missing}."

      let includedEvents =
        declaredEvents
        |> List.filter (fun (declaringType, event) ->
          not (definition.ExcludedEvents.Contains(eventIdentity declaringType event)))

      yield $"    let {definition.Name}: PropKey array ="

      match includedEvents with
      | [] -> yield "      [||]"
      | events ->
        yield "      [|"

        for declaringType, event in events do
          let accessorName = Registry.ViewTypes.GetUniqueTypeName declaringType
          yield $"        {accessorName}.{event.PKey}.Untyped"

        yield "      |]"

      yield ""
  }

let getAccessor (viewType: Type) =
  let viewName = Registry.ViewTypes.GetUniqueTypeName viewType
  $"PKey.{viewName}{genericTypeParamsBlock viewType}"

// TODO: code generated here is not currently used anywhere
let genInterfaceGroupKeys moduleName (interfaceTypes: Type array) =

  let allProps =
    interfaceTypes
    |> Array.collect (fun t -> (ViewMetadata.create t).Properties |> Array.map (fun p -> p, t))

  let allEvents =
    interfaceTypes
    |> Array.collect (fun t -> (ViewMetadata.create t).Events |> Array.map (fun e -> e, t))

  if allProps.Length = 0 && allEvents.Length = 0 then
    Seq.empty
  else
    seq {
      yield $"  module internal {moduleName} ="

      if not (allProps.Length = 0) then
        yield "    // Properties"

        for prop, interfaceType in allProps do
          let propertyId = Registry.PropertyIds.Property(interfaceType, prop.PKey)
          let propertyId = propertyIdExpression propertyId

          yield
            $"    let {prop.PKey}{genericTypeParamsBlock interfaceType}: PropKey<{getFSharpTypeName prop.PropertyInfo.PropertyType}> = PropKey.Create.simple({propertyId}, \"{moduleName}.{prop.PKey}\")"

          yield ""

      if not (allEvents.Length = 0) then
        yield "    // Events"

        for event, interfaceType in allEvents do
          let handlerType = eventHandlerType event.EventInfo
          let propertyId = Registry.PropertyIds.Event(interfaceType, event.PKey)
          let propertyId = propertyIdExpression propertyId

          yield
            $"    let {event.PKey}{genericTypeParamsBlock interfaceType}: PropKey<{handlerType}> = PropKey.Create.event({propertyId}, \"{moduleName}.{event.PKey}_event\")"

          yield ""
    }

let opens =
  [ "open System"
    "open System.Collections.Generic"
    "open Terminal.Gui.App"
    "open Terminal.Gui.Views" ]

let gen () =
  // Get all interfaces from Terminal.Gui that we need to handle
  let interfaces =
    typeof<Terminal.Gui.ViewBase.View>.Assembly.GetTypes()
    |> Array.filter (fun t ->
      t.IsInterface
      && t.Namespace = "Terminal.Gui.ViewBase"
      && t.Name.StartsWith("I")
      && t.Name <> "IApplication"
      && t.Name <> "IDesignTimeProperties")
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

    // Group interfaces by name without arity, so that we can generate shared keys for generic/non generic interfaces
    let interfaceGroups =
      interfaces |> Array.groupBy getTypeNameWithoutArity |> Array.sortBy fst

    for nameWithoutArity, group in interfaceGroups do
      let moduleName = $"{nameWithoutArity}Interface"
      yield! genInterfaceGroupKeys moduleName group

    yield ""

    yield! genPKeysAccessors ()
    yield ""
    yield! genReconciledEventKeys ()
  }
  |> CodeWriter.write "PKey.gen.fs"
