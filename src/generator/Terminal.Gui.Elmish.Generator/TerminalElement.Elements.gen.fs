module Terminal.Gui.Elmish.Generator.TerminalElement_Elements

open System
open System.IO
open Terminal.Gui.Elmish.Generator

let terminalElementAndViewDeclaration (viewType: Type) =
  seq {
    yield $"    let terminalElement = terminalElement :?> TerminalElement"
    if viewType <> typeof<Terminal.Gui.ViewBase.View> then
      yield $"    let view = terminalElement.View :?> {ViewType.cleanTypeName viewType}{ViewType.genericTypeParamsBlock viewType}"
    else
      yield $"    let view = terminalElement.View"
  }

let pkeyPrefix (viewType: Type) =
  $"PKey.{Registry.GetUniqueTypeName viewType}{ViewType.genericTypeParamsBlock viewType}"

let subElementsPropKeys (view: ViewType.ViewMetadata) =
  seq {
    yield $"  override this.SubElements_PropKeys ="
    yield $"    ["
    for prop in view.View_Typed_Properties do
      yield $"      SubElementPropKey.from {pkeyPrefix view.ViewType}.{prop.PKey}_element"
    for prop in view.ViewsCollection_Typed_Properties do
      yield $"      SubElementPropKey.from {pkeyPrefix view.ViewType}.{prop.PKey}_elements"
    yield $"    ]"
    yield $"    |> List.append base.SubElements_PropKeys"
  }

let setPropsCode (view: ViewType.ViewMetadata) =
  if view.HasNoEventsOrProperties then
    Seq.empty
  else
    seq {
      yield $"  override _.setProps(terminalElement: IInternalTerminalElement, props: Props) ="
      yield $"    base.setProps(terminalElement, props)"
      yield $""
      yield! terminalElementAndViewDeclaration view.ViewType
      yield $""

      if view.Properties.Length > 0 then
        yield "    // Properties"
      for prop in view.Properties do
        yield $"    props"
        yield $"    |> Props.tryFind {pkeyPrefix view.ViewType}.{prop.PKey}"
        yield $"    |> Option.iter (fun v -> view.{prop.PKey} <- v)"
        yield ""

      if view.Events.Length > 0 then
        yield "    // Events"
      for event in view.Events do
        yield $"    terminalElement.trySetEventHandler({pkeyPrefix view.ViewType}.{event.PKey}, view.{event.PKey})"
        yield ""

    }

let removePropsCode (view: ViewType.ViewMetadata) =
  if view.HasNoEventsOrProperties then
    Seq.empty
  else
    seq {
      yield $"  override _.removeProps(terminalElement: IInternalTerminalElement, props: Props) ="
      if not (view.ViewType = typeof<Terminal.Gui.ViewBase.View>) then
        yield $"    base.removeProps(terminalElement, props)"
      yield $""
      yield! terminalElementAndViewDeclaration view.ViewType
      yield $""
      if view.Properties.Length > 0 then
        yield "    // Properties"
      for prop in view.Properties do
        yield $"    props"
        yield $"    |> Props.tryFind {pkeyPrefix view.ViewType}.{prop.PKey}"
        yield $"    |> Option.iter (fun _ ->"
        yield $"        view.{prop.PKey} <- Unchecked.defaultof<_>)"
        yield ""
      if view.Events.Length > 0 then
        yield "    // Events"
      for event in view.Events do
        yield $"    terminalElement.tryRemoveEventHandler {pkeyPrefix view.ViewType}.{event.PKey}"
    }

let setAsChildOfParentView (viewType: Type) =
  let exceptions =
    [ typeof<Terminal.Gui.Views.Menu> ]

  exceptions |> Seq.filter (fun t -> t = viewType) |> Seq.isEmpty

let gen () =
  seq {
    yield "namespace Terminal.Gui.Elmish"
    yield ""
    yield "open System"
    yield "open Terminal.Gui.App"
    yield "open Terminal.Gui.ViewBase"
    yield "open Terminal.Gui.Views"
    yield ""
    yield ""
    for viewType in ViewType.viewTypesOrderedByInheritance do
      let genericBlock = ViewType.genericTypeParamsWithConstraintsBlock viewType
      let genericParamsBlock = ViewType.genericTypeParamsBlock viewType
      let viewMetadata = ViewType.analyzeViewType viewType
      let setAsChildOfParentView = setAsChildOfParentView viewType

      if viewType.IsAbstract then
        yield "[<AbstractClass>]"
      yield $"type internal {ViewType.cleanTypeName viewType}TerminalElement{genericBlock}(props: Props) ="
      if viewType = typeof<Terminal.Gui.ViewBase.View> then
        yield $"  inherit TerminalElement(props)"
      else
        yield $"  inherit {(ViewType.parentViewType viewType).Name}TerminalElement(props)"
      yield ""
      yield $"  override _.name = \"{viewType.Name}\""
      yield ""
      if viewType.IsAbstract then
        yield $"  override _.newView() = failwith \"Cannot instantiate abstract view type {ViewType.cleanTypeName viewType}\""
      else
        yield $"  override _.newView() = new {ViewType.cleanTypeName viewType}{genericParamsBlock}()"
      yield ""
      yield $"  override _.setAsChildOfParentView = %b{setAsChildOfParentView}"
      yield ""
      if viewMetadata.View_Typed_Properties .Length > 0 ||
         viewMetadata.ViewsCollection_Typed_Properties.Length > 0 then
        yield! subElementsPropKeys viewMetadata
        yield ""
      yield! setPropsCode viewMetadata
      yield! removePropsCode viewMetadata
      yield ""

      match Registry.TryGetNeededIElementInterface viewType with
      | Some interfaceName ->
          yield $"  interface {interfaceName}"
          yield ""
      | None -> ()
  }
  |> String.concat Environment.NewLine
  |> File.writeAllText (Path.Combine (Environment.CurrentDirectory, "TerminalElement.Elements.gen.fs"))
