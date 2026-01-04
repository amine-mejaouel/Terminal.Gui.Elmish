module Terminal.Gui.Elmish.Generator.TerminalElement_Elements

open System
open System.IO
open Terminal.Gui.Elmish.Generator
open Terminal.Gui.Elmish.Generator.PKey_gen

let terminalElementAndViewDeclaration (viewType: Type) =
  seq {
    yield $"    let terminalElement = terminalElement :?> TerminalElement"
    if viewType <> typeof<Terminal.Gui.ViewBase.View> then
      yield $"    let view = terminalElement.View :?> {ViewType.cleanTypeName viewType}{ViewType.genericTypeParamsBlock viewType}"
    else
      yield $"    let view = terminalElement.View"
  }

let pkeyPrefix (viewType: Type) =
  $"PKey.{PKeyRegistry.GetPKeySegment viewType}{ViewType.genericTypeParamsBlock viewType}"

let setPropsCode (viewType: Type) =
  let view = ViewType.decompose viewType
  if view.HasNoEventsOrProperties then
    Seq.empty
  else
    seq {
      yield $"  override _.setProps(terminalElement: IInternalTerminalElement, props: Props) ="
      if not (viewType = typeof<Terminal.Gui.ViewBase.View>) then
        yield $"    base.setProps(terminalElement, props)"
      yield $""
      yield! terminalElementAndViewDeclaration viewType
      yield $""
      yield "    // Properties"
      for prop in view.Properties do
        yield $"    props"
        yield $"    |> Props.tryFind {pkeyPrefix viewType}.{prop.PKey}"
        yield $"    |> Option.iter (fun v -> view.{prop.PropertyInfo.Name} <- v)"
        yield ""
      yield "    // Events"
      for event in view.Events do
        yield $"    terminalElement.trySetEventHandler({pkeyPrefix viewType}.{event.PKey}, view.{event.EventInfo.Name})"
        yield ""
    }

let removePropsCode (viewType: Type) =
  let view = ViewType.decompose viewType
  if view.HasNoEventsOrProperties then
    Seq.empty
  else
    seq {
      yield $"  override _.removeProps(terminalElement: IInternalTerminalElement, props: Props) ="
      if not (viewType = typeof<Terminal.Gui.ViewBase.View>) then
        yield $"    base.removeProps(terminalElement, props)"
      yield $""
      yield! terminalElementAndViewDeclaration viewType
      yield $""
      if view.Properties.Length > 0 then
        yield "    // Properties"
      for prop in view.Properties do
        yield $"    props"
        yield $"    |> Props.tryFind {pkeyPrefix viewType}.{prop.PKey}"
        yield $"    |> Option.iter (fun _ ->"
        yield $"        view.{prop.PropertyInfo.Name} <- Unchecked.defaultof<_>)"
        yield ""
      if view.Events.Length > 0 then
        yield "    // Events"
      for event in view.Events do
        yield $"    terminalElement.tryRemoveEventHandler {pkeyPrefix viewType}.{event.PKey}"
    }

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
      if viewType.IsAbstract then
        yield "[<AbstractClass>]"
      yield $"type internal {ViewType.cleanTypeName viewType}TerminalElement{genericBlock}(props: Props) ="
      match ViewType.parentViewType viewType with
      | Some t ->
        yield $"  inherit {t.Name}TerminalElement(props)"
      | None ->
        yield $"  inherit TerminalElement(props)"
      yield ""
      yield $"  override _.name = \"{viewType.Name}\""
      yield ""
      if viewType.IsAbstract then
        yield $"  override _.newView() = failwith \"Cannot instantiate abstract view type {ViewType.cleanTypeName viewType}\""
      else
        yield $"  override _.newView() = new {ViewType.cleanTypeName viewType}{genericParamsBlock}()"
      yield ""
      yield! setPropsCode viewType
      yield ""
      yield! removePropsCode viewType
      yield ""
  }
  |> String.concat Environment.NewLine
  |> File.writeAllText (Path.Combine (Environment.CurrentDirectory, "TerminalElement.Elements.gen.fs"))
