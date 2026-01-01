module Terminal.Gui.Elmish.Generator.TerminalElement_Elements

open System
open System.IO
open Terminal.Gui.Elmish.Generator
open Terminal.Gui.Elmish.Generator.PKey_gen

let terminalElementAndViewDeclaration (viewType: Type) =
  seq {
    yield $"    let terminalElement = terminalElement :?> TerminalElement"
    if viewType <> typeof<Terminal.Gui.ViewBase.View> then
      yield $"    let view = terminalElement.View :?> {CodeGen.cleanTypeName viewType}{CodeGen.genericTypeParams viewType}"
    else
      yield $"    let view = terminalElement.View"
  }

let pkeyPrefix (viewType: Type) =
  $"PKey.{(CodeGen.cleanTypeName >> String.lowerCamelCase) viewType}{CodeGen.genericTypeParams viewType}"

let setPropsCode (viewType: Type) =
  let properties = ViewType.properties viewType
  let events = ViewType.events viewType
  if properties.Length = 0 && events.Length = 0 then
    Seq.empty
  else
    seq {
      yield $"  override _.setProps(terminalElement: IInternalTerminalElement, props: Props) ="
      yield $"    base.setProps(terminalElement, props)"
      yield $""
      yield! terminalElementAndViewDeclaration viewType
      yield $""
      yield "    // Properties"
      for prop in properties do
        yield $"    props"
        yield $"    |> Props.tryFind {pkeyPrefix viewType}.{CodeGen.asPKey prop.Name}"
        if CodeGen.isNullable prop.PropertyType then
          yield $"    |> Option.iter (fun v -> view.{prop.Name} <- v |> Option.toNullable)"
        else
          yield $"    |> Option.iter (fun v -> view.{prop.Name} <- v)"
        yield ""
      yield "    // Events"
      for event in events do
        yield $"    terminalElement.trySetEventHandler({pkeyPrefix viewType}.{CodeGen.asPKey event.Name}, view.{event.Name})"
        yield ""
    }

let removePropsCode (viewType: Type) =
  let properties = ViewType.properties viewType
  let events = ViewType.events viewType
  if properties.Length = 0 && events.Length = 0 then
    Seq.empty
  else
    seq {
      yield $"  override _.removeProps(terminalElement: IInternalTerminalElement, props: Props) ="
      yield $"    base.removeProps(terminalElement, props)"
      yield $""
      yield! terminalElementAndViewDeclaration viewType
      yield $""
      let properties = ViewType.properties viewType
      if properties |> (not << Array.isEmpty) then
        yield "    // Properties"
      for prop in properties do
        yield $"    props"
        yield $"    |> Props.tryFind {pkeyPrefix viewType}.{CodeGen.asPKey prop.Name}"
        yield $"    |> Option.iter (fun _ ->"
        yield $"        view.{prop.Name} <- Unchecked.defaultof<_>)"
        yield ""
      let events = ViewType.events viewType
      if events |> (not << Array.isEmpty) then
        yield "    // Events"
      for event in events do
        yield $"    terminalElement.tryRemoveEventHandler {pkeyPrefix viewType}.{CodeGen.asPKey event.Name}"
    }

let gen () =
  seq {
    yield "namespace Terminal.Gui.Elmish"
    yield ""
    yield "open Terminal.Gui.App"
    yield "open Terminal.Gui.ViewBase"
    yield "open Terminal.Gui.Views"
    yield ""
    yield ""
    for viewType in ViewType.viewTypesOrderedByInheritance do
      let genericParams = CodeGen.genericTypeParams viewType
      if viewType.IsAbstract then
        yield "[<AbstractClass>]"
      yield $"type internal {CodeGen.cleanTypeName viewType}TerminalElement{genericParams}(props: Props) ="
      match ViewType.parentViewType viewType with
      | Some t ->
        yield $"  inherit {t.Name}TerminalElement(props)"
      | None ->
        yield $"  inherit TerminalElement(props)"
      yield ""
      yield $"  override _.name = \"{viewType.Name}\""
      yield ""
      if viewType.IsAbstract then
        yield $"  override _.newView() = failwith \"Cannot instantiate abstract view type {CodeGen.cleanTypeName viewType}\""
      else
        yield $"  override _.newView() = new {CodeGen.cleanTypeName viewType}{genericParams}()"
      yield ""
      yield! setPropsCode viewType
      yield ""
      yield! removePropsCode viewType
      yield ""
  }
  |> String.concat Environment.NewLine
  |> File.writeAllText (Path.Combine (Environment.CurrentDirectory, "TerminalElement.Elements.gen.fs"))
