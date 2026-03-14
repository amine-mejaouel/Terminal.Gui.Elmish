module Terminal.Gui.Elmish.Generator.DomNode_Elements

open System
open Terminal.Gui.Elmish.Generator

/// Normalize a type to its generic type definition if it's a constructed generic.
/// Needed because `BaseType` on generic type definitions returns constructed generics
/// (e.g., `Bar<T>`) whose `FullName` is null, breaking `GetUniqueTypeName`.
let private normalizeForLookup (t: Type) =
  if t.IsGenericType && not t.IsGenericTypeDefinition then
    t.GetGenericTypeDefinition()
  else
    t

let private domViewDeclaration (viewType: Type) =
  if viewType <> typeof<Terminal.Gui.ViewBase.View> then
    $"    let view = domNode.View :?> {getTypeNameWithoutArity viewType}{genericTypeParamsBlock viewType}"
  else
    $"    let view = domNode.View"

let private applyPropsCode (view: ViewMetadata) (viewType: Type) =
  let genericBlock = genericTypeParamsWithConstraintsBlock viewType

  seq {
    yield $"  let applyProps{genericBlock} (domNode: DomNode, props: Props) ="

    if viewType <> typeof<Terminal.Gui.ViewBase.View> then
      let parentName =
        Registry.ViewTypes.GetUniqueTypeName(normalizeForLookup viewType.ParentViewType)

      let parentGenericBlock = genericTypeParamsBlock viewType.ParentViewType
      yield $"    {parentName}Dom.applyProps{parentGenericBlock} (domNode, props)"

    if view.HasNoEventsOrProperties then
      if viewType = typeof<Terminal.Gui.ViewBase.View> then
        yield $"    ()"
    else
      yield domViewDeclaration viewType
      yield ""

      if view.Properties.Length > 0 then
        yield "    // Properties"

      for prop in view.Properties |> Seq.filter (fun p -> p.PKey <> "X" && p.PKey <> "Y") do
        yield $"    props"
        yield $"    |> Props.tryFind {PKey.getAccessor view.Type}.{prop.PKey}"
        yield $"    |> Option.iter (fun v -> view.{prop.PKey} <- v)"
        yield ""

      if view.Events.Length > 0 then
        yield "    // Events"

      for event in view.Events do
        yield
          $"    EventHandlerHelpers.TrySetEventHandler(domNode.EventRegistrar, props, {PKey.getAccessor view.Type}.{event.PKey}, view.{event.PKey})"

        yield ""

    yield ""
  }

let private removePropsCode (view: ViewMetadata) (viewType: Type) =
  let genericBlock = genericTypeParamsWithConstraintsBlock viewType

  seq {
    yield $"  let removeProps{genericBlock} (domNode: DomNode, props: Props) ="

    if viewType <> typeof<Terminal.Gui.ViewBase.View> then
      let parentName =
        Registry.ViewTypes.GetUniqueTypeName(normalizeForLookup viewType.ParentViewType)

      let parentGenericBlock = genericTypeParamsBlock viewType.ParentViewType
      yield $"    {parentName}Dom.removeProps{parentGenericBlock} (domNode, props)"

    if view.HasNoEventsOrProperties then
      if viewType = typeof<Terminal.Gui.ViewBase.View> then
        yield $"    ()"
    else
      yield domViewDeclaration viewType
      yield ""

      if view.Properties.Length > 0 then
        yield "    // Properties"

      for prop in view.Properties |> Seq.filter (fun p -> p.PKey <> "X" && p.PKey <> "Y") do
        let defaultValue =
          if prop.PropertyInfo.PropertyType = typeof<string> then
            "\"\""
          else if prop.PropertyInfo.PropertyType = typeof<Terminal.Gui.Input.Key> then
            "Terminal.Gui.Input.Key.Empty"
          else if prop.PropertyInfo.PropertyType = typeof<Terminal.Gui.ViewBase.View> then
            "new View()"
          else
            "Unchecked.defaultof<_>"

        yield $"    props"
        yield $"    |> Props.tryFind {PKey.getAccessor view.Type}.{prop.PKey}"
        yield $"    |> Option.iter (fun _ ->"
        yield $"        view.{prop.PKey} <- {defaultValue})"
        yield ""

      if view.Events.Length > 0 then
        yield "    // Events"

      for event in view.Events do
        yield
          $"    EventHandlerHelpers.TryRemoveEventHandler(domNode.EventRegistrar, {PKey.getAccessor view.Type}.{event.PKey})"

    yield ""
  }

let private newViewCode (viewType: Type) =
  let genericBlock = genericTypeParamsWithConstraintsBlock viewType
  let genericParamsBlock = genericTypeParamsBlock viewType

  seq {
    if viewType.IsAbstract then
      yield
        $"  let newView{genericBlock} () : View = failwith \"Cannot instantiate abstract view type {getTypeNameWithoutArity viewType}\""
    else
      yield $"  let newView{genericBlock} () : View = new {getTypeNameWithoutArity viewType}{genericParamsBlock}()"
  }

let private subElementsPropKeysCode (view: ViewMetadata) (viewType: Type) =
  seq {
    if viewType = typeof<Terminal.Gui.ViewBase.View> then
      yield $"  let subElementsPropKeys: RawPropKey list ="
      yield $"    ["

      for prop in view.View_Typed_Properties do
        yield $"      {PKey.getAccessor view.Type}.{prop.PKey}_element.key"

      yield $"    ]"
    else
      // subElementsPropKeys is always non-generic (keys are strings, not type-dependent).
      // Use the parent module name without type arguments.
      let parentName =
        Registry.ViewTypes.GetUniqueTypeName(normalizeForLookup viewType.ParentViewType)

      if view.View_Typed_Properties.Length > 0 then
        yield $"  let subElementsPropKeys: RawPropKey list ="
        yield $"    {parentName}Dom.subElementsPropKeys"
        yield $"    @ ["

        for prop in view.View_Typed_Properties do
          yield $"         {PKey.getAccessor view.Type}.{prop.PKey}_element.key"

        yield $"       ]"
      else
        yield $"  let subElementsPropKeys: RawPropKey list = {parentName}Dom.subElementsPropKeys"
  }

let private setAsChildOfParentView (viewType: Type) =
  let exactExceptions = [ typeof<Terminal.Gui.Views.Menu> ]
  let assignableExceptions = [ typeof<Terminal.Gui.App.PopoverImpl> ]

  not (
    exactExceptions |> Seq.exists (fun t -> t = viewType)
    || assignableExceptions |> Seq.exists (fun t -> viewType.IsAssignableTo t)
  )

let opens =
  [ "open Terminal.Gui.App"
    "open Terminal.Gui.ViewBase"
    "open Terminal.Gui.Views" ]

let gen () =
  seq {
    yield "namespace Terminal.Gui.Elmish"
    yield ""
    yield! opens
    yield ""
    yield ""

    for viewType in Registry.ViewTypes.orderedByInheritance do
      let uniqueName = Registry.ViewTypes.GetUniqueTypeName viewType
      let viewMetadata = ViewMetadata.create viewType

      yield $"module internal {uniqueName}Dom ="
      yield ""
      yield! applyPropsCode viewMetadata viewType
      yield! removePropsCode viewMetadata viewType
      yield! newViewCode viewType
      yield ""
      yield $"  let setAsChildOfParentView: bool = %b{(setAsChildOfParentView viewType)}"

      yield ""
      yield! subElementsPropKeysCode viewMetadata viewType
      yield ""
      yield ""
  }
  |> CodeWriter.write "DomNode.Elements.gen.fs"
