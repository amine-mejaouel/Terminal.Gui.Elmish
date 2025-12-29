module Terminal.Gui.Elmish.Generator.TerminalElement_Elements

open System
open System.IO

let properties (viewType: Type) =
  viewType.GetProperties(
    System.Reflection.BindingFlags.Public |||
    System.Reflection.BindingFlags.Instance |||
    System.Reflection.BindingFlags.DeclaredOnly)
  |> Array.filter (fun p -> p.CanRead && p.CanWrite)

let events (viewType: Type) =
  viewType.GetEvents(
    System.Reflection.BindingFlags.Public |||
    System.Reflection.BindingFlags.Instance |||
    System.Reflection.BindingFlags.DeclaredOnly)
  |> Array.filter (fun e -> e.AddMethod.IsPublic && e.RemoveMethod.IsPublic)

let lowerCamelCase (s: String) =
  if String.IsNullOrEmpty s then s
  else s.[0..0].ToLower() + s.[1..]

let setPropsCode (viewType: Type) =
  seq {
    yield $"  override _.setProps(terminalElement: IInternalTerminalElement, props: Props) ="
    yield $"    base.setProps(terminalElement, props)"
    yield $""
    yield $"    let terminalElement = terminalElement :?> TerminalElement"
    yield $"    let view = terminalElement.View :?> {viewType.FullName}"
    yield $""
    yield "    // Properties"
    for prop in properties viewType do
      yield $"    props"
      yield $"    |> Props.tryFind PKey.{lowerCamelCase viewType.Name}.{lowerCamelCase prop.Name}"
      yield $"    |> Option.iter (fun v -> view.{prop.Name} <- v)"
      yield ""
    yield "    // Events"
    for event in events viewType do
      yield $"    terminalElement.trySetEventHandler(PKey.{lowerCamelCase viewType.Name}.{lowerCamelCase event.Name}, view.{event.Name})"
      yield ""
  }

let removePropsCode (viewType: Type) =
  seq {
    yield $"  override _.removeProps(terminalElement: IInternalTerminalElement, props: Props) ="
    yield $"    base.removeProps(terminalElement, props)"
    yield $""
    yield $"    let terminalElement = terminalElement :?> TerminalElement"
    yield $"    let view = terminalElement.View :?> {viewType.FullName}"
    yield $""
    let properties = properties viewType
    if properties |> (not << Array.isEmpty) then
      yield "    // Properties"
    for prop in properties do
      yield $"    props"
      yield $"    |> Props.tryFind PKey.{lowerCamelCase viewType.Name}.{lowerCamelCase prop.Name}"
      yield $"    |> Option.iter (fun _ -> "
      yield $"        view.{prop.Name} <- Unchecked.defaultof<_>)"
      yield ""
    let events = events viewType
    if events |> (not << Array.isEmpty) then
      yield "    // Events"
    for event in events do
      yield $"    terminalElement.tryRemoveEventHandler PKey.{viewType.Name}.{event.Name}"
  }

let gen () =
  seq {
    yield "namespace Terminal.Gui.Elmish"
    yield ""
    yield "open System"
    yield "open System.Collections.ObjectModel"
    yield "open Terminal.Gui.ViewBase"
    yield "open Terminal.Gui.Views"
    yield ""
    yield ""
    for viewType in ViewTypes.orderedByInheritance do
      yield $"type internal {viewType.Name}TerminalElement(props: Props) ="
      match ViewTypes.parentViewType viewType with
      | Some t ->
          yield $"  inherit {t.Name}TerminalElement(props)"
      | None ->
        ()
      yield ""
      yield $"  override _.name = \"{viewType.Name}\""
      yield ""
      yield $"  override _.newView() = new {viewType.FullName}()"
      yield ""
      yield! setPropsCode viewType
      yield ""
      yield! removePropsCode viewType
      yield ""
  }
  |> String.concat Environment.NewLine
  |> File.writeAllText (Path.Combine (Environment.CurrentDirectory, "src", "TerminalElement.Elements.gen.fs"))
