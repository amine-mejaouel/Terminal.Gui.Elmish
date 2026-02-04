module Terminal.Gui.Elmish.Generator.TerminalElement_Elements

open System
open Terminal.Gui.Elmish.Generator

let terminalElementAndViewDeclaration (viewType: Type) =
  seq {
    yield "    let terminalElement = terminalElement :?> TerminalElement"
    if viewType <> typeof<Terminal.Gui.ViewBase.View> then
      yield $"    let view = terminalElement.View :?> {getTypeNameWithoutArity viewType}{genericTypeParamsBlock viewType}"
    else
      yield $"    let view = terminalElement.View"
  }

let subElementsPropKeys (view: ViewMetadata) =
  seq {
    yield $"  override this.SubElements_PropKeys ="
    yield $"    ["
    for prop in view.View_Typed_Properties do
      yield $"      SubElementPropKey.from {PKey.getAccessor view.Type}.{prop.PKey}_element"
    for prop in view.ViewsCollection_Typed_Properties do
      yield $"      SubElementPropKey.from {PKey.getAccessor view.Type}.{prop.PKey}_elements"
    yield $"    ]"
    yield $"    |> List.append base.SubElements_PropKeys"
  }

let setPropsCode (view: ViewMetadata) =
  if view.HasNoEventsOrProperties then
    Seq.empty
  else
    seq {
      yield $"  override _.SetProps(terminalElement: IInternalTerminalElement, props: Props) ="
      yield $"    base.SetProps(terminalElement, props)"
      yield $""
      yield! terminalElementAndViewDeclaration view.Type
      yield $""

      if view.Properties.Length > 0 then
        yield "    // Properties"
      for prop in view.Properties do
        yield $"    props"
        yield $"    |> Props.tryFind {PKey.getAccessor view.Type}.{prop.PKey}"
        yield $"    |> Option.iter (fun v -> view.{prop.PKey} <- v)"
        yield ""

      if view.Events.Length > 0 then
        yield "    // Events"
      for event in view.Events do
        yield $"    terminalElement.trySetEventHandler({PKey.getAccessor view.Type}.{event.PKey}, view.{event.PKey})"
        yield ""

    }

let removePropsCode (view: ViewMetadata) =
  if view.HasNoEventsOrProperties then
    Seq.empty
  else
    seq {
      yield $"  override _.RemoveProps(terminalElement: IInternalTerminalElement, props: Props) ="
      if not (view.Type = typeof<Terminal.Gui.ViewBase.View>) then
        yield $"    base.RemoveProps(terminalElement, props)"
      yield $""
      yield! terminalElementAndViewDeclaration view.Type
      yield $""
      if view.Properties.Length > 0 then
        yield "    // Properties"
      for prop in view.Properties do
        let defaultValue =
          if prop.PropertyInfo.PropertyType = typeof<string> then
            "\"\""
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
        yield $"    terminalElement.tryRemoveEventHandler {PKey.getAccessor view.Type}.{event.PKey}"
    }

let setAsChildOfParentView (viewType: Type) =
  let exceptions =
    [ typeof<Terminal.Gui.Views.Menu> ]

  exceptions |> Seq.filter (fun t -> t = viewType) |> Seq.isEmpty

let opens = [
    "open System"
    "open Terminal.Gui.App"
    "open Terminal.Gui.ViewBase"
    "open Terminal.Gui.Views"
  ]

let gen () =
  seq {
    yield "namespace Terminal.Gui.Elmish"
    yield ""
    yield! opens
    yield ""
    yield ""
    for viewType in Registry.ViewTypes.orderedByInheritance do
      let genericBlock = genericTypeParamsWithConstraintsBlock viewType
      let genericParamsBlock = genericTypeParamsBlock viewType
      let viewMetadata = ViewMetadata.create viewType

      if viewType.IsAbstract then
        yield "[<AbstractClass>]"
      yield $"type internal {getTypeNameWithoutArity viewType}TerminalElement{genericBlock}(props: Props) ="
      if viewType = typeof<Terminal.Gui.ViewBase.View> then
        yield $"  inherit TerminalElement(props)"
      else
        yield $"  inherit {viewType.ParentViewType.Name}TerminalElement(props)"
      yield ""
      yield $"  override _.Name = \"{viewType.Name}\""
      yield ""
      if viewType.IsAbstract then
        yield $"  override _.NewView() = failwith \"Cannot instantiate abstract view type {getTypeNameWithoutArity viewType}\""
      else
        yield $"  override _.NewView() = new {getTypeNameWithoutArity viewType}{genericParamsBlock}()"
      yield ""
      yield $"  override _.SetAsChildOfParentView = %b{(setAsChildOfParentView viewType)}"
      yield ""
      if viewMetadata.View_Typed_Properties.Length > 0 ||
         viewMetadata.ViewsCollection_Typed_Properties.Length > 0 then
        yield! subElementsPropKeys viewMetadata
        yield ""
      yield! setPropsCode viewMetadata
      yield! removePropsCode viewMetadata
      yield ""

      for i in Registry.TEInterfaces.GetAllPreviouslyCreatedInterfaces viewType do
        yield $"  interface {i}"
        yield ""
  }
  |> CodeWriter.write "TerminalElement.Elements.gen.fs"
