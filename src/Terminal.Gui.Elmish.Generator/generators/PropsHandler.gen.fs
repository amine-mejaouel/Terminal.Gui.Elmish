module Terminal.Gui.Elmish.Generator.PropsHandler

open System
open Terminal.Gui.Elmish.Generator

let propHandlerTypeName (viewType: Type) =
  $"{getTypeNameWithoutArity viewType}PropHandler{genericTypeParamsBlock viewType}"

let terminalElementAndViewDeclaration (viewType: Type) =
  seq {
    if viewType <> typeof<Terminal.Gui.ViewBase.View> then
      yield
        $"    let view = terminalElement.View :?> {getTypeNameWithoutArity viewType}{genericTypeParamsBlock viewType}"
    else
      yield $"    let view = terminalElement.View"
  }

let subElementsPropKeys (view: ViewMetadata) =
  seq {
    yield $"  override this.SubElements_PropKeys ="
    yield $"    ["

    for prop in view.View_Typed_Properties do
      yield $"      {PKey.getAccessor view.Type}.{prop.PKey}_viewSpec.key"

    yield $"    ]"
    yield $"    |> List.append base.SubElements_PropKeys"
  }

let setPropsCode (view: ViewMetadata) =
  seq {
    yield $"  static member setProps(terminalElement: ViewBackedTerminalElement, props: Props) ="

    if view.Type <> typeof<Terminal.Gui.ViewBase.View> then
      yield $"    {propHandlerTypeName view.Type.ParentViewType}.setProps(terminalElement, props)"

    yield $""

    if not view.HasNoEventsOrProperties then
      yield! terminalElementAndViewDeclaration view.Type
      yield $""

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
      yield $"    if props |> Props.exists {PKey.getAccessor view.Type}.{event.PKey} then"
      yield $"      terminalElement.TrySetEventHandler({PKey.getAccessor view.Type}.{event.PKey}, view.{event.PKey})"

      yield ""
  }

let clearPropCode (view: ViewMetadata) =
  seq {
    yield $"  static member clearProp(terminalElement: ViewBackedTerminalElement, propertyId: PropertyId) ="

    if view.Properties.Length > 0 then
      yield! terminalElementAndViewDeclaration view.Type
      yield $""

    yield "    match propertyId.Value with"

    for prop in view.Properties do
      let propertyId =
        if prop.IsViewProperty then
          Registry.PropertyIds.ViewProperty(view.Type, prop.PKey)
        else
          Registry.PropertyIds.Property(view.Type, prop.PKey)

      let defaultValue =
        if prop.PropertyInfo.PropertyType = typeof<string> then
          "\"\""
        else if prop.PropertyInfo.PropertyType = typeof<Terminal.Gui.Input.Key> then
          "Terminal.Gui.Input.Key.Empty"
        else
          "Unchecked.defaultof<_>"

      yield $"    | {propertyId} -> view.{prop.PKey} <- {defaultValue}"

    for event in view.Events do
      let propertyId = Registry.PropertyIds.Event(view.Type, event.PKey)

      yield $"    | {propertyId} -> terminalElement.TryRemoveEventHandler({PKey.getAccessor view.Type}.{event.PKey})"

    if view.Type = typeof<Terminal.Gui.ViewBase.View> then
      yield "    | _ -> invalidOp $\"Property ID {propertyId} cannot be cleared on '{terminalElement.Name}'.\""
    else
      yield $"    | _ -> {propHandlerTypeName view.Type.ParentViewType}.clearProp(terminalElement, propertyId)"
  }

let setAsChildOfParentView (viewType: Type) =
  // Menu: set via PopoverMenu.Root property, not a regular child view
  let exactExceptions = [ typeof<Terminal.Gui.Views.Menu> ]
  // PopoverImpl and subclasses (Popover<>, PopoverMenu): floating overlays managed by Application.Popovers
  let assignableExceptions = [ typeof<Terminal.Gui.App.PopoverImpl> ]

  not (
    exactExceptions |> Seq.exists (fun t -> t = viewType)
    || assignableExceptions |> Seq.exists (fun t -> viewType.IsAssignableTo t)
  )

let opens =
  [ "open System"
    "open System.Collections.Generic"
    "open Terminal.Gui.App"
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
      let genericBlock = genericTypeParamsWithConstraintsBlock viewType
      let viewMetadata = ViewMetadata.create viewType

      yield $"type internal {getTypeNameWithoutArity viewType}PropHandler{genericBlock} ="

      // if viewType <> typeof<Terminal.Gui.ViewBase.View> then
      //   let genericBlock = genericTypeParamsBlock viewType.ParentViewType
      //   yield $"  inherit {getTypeNameWithoutArity viewType.ParentViewType}TerminalElement{genericBlock}(props)"

      // if
      //   viewMetadata.View_Typed_Properties.Length > 0
      //   || viewMetadata.ViewsCollection_Typed_Properties.Length > 0
      // then
      //   yield! subElementsPropKeys viewMetadata
      //   yield ""

      yield! setPropsCode viewMetadata
      yield! clearPropCode viewMetadata
      yield ""

  }
  |> CodeWriter.write __SOURCE_FILE__
