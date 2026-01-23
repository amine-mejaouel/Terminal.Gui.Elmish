module Terminal.Gui.Elmish.Generator.TerminalElement_Elements

open System
open System.IO
open Terminal.Gui.Elmish.Generator

let terminalElementAndViewDeclaration (viewType: Type) =
  seq {
    yield $"    let terminalElement = terminalElement :?> TerminalElement"
    if viewType <> typeof<Terminal.Gui.ViewBase.View> then
      yield $"    let view = terminalElement.View :?> {getTypeNameWithoutArity viewType}{genericTypeParamsBlock viewType}"
    else
      yield $"    let view = terminalElement.View"
  }

let pkeyPrefix (viewType: Type) =
  $"PKey.{Registry.GetUniqueTypeName viewType}{genericTypeParamsBlock viewType}"

let subElementsPropKeys (view: ViewMetadata) =
  seq {
    yield $"  override this.SubElements_PropKeys ="
    yield $"    ["
    for prop in view.View_Typed_Properties do
      yield $"      SubElementPropKey.from {pkeyPrefix view.Type}.{prop.PKey}_element"
    for prop in view.ViewsCollection_Typed_Properties do
      yield $"      SubElementPropKey.from {pkeyPrefix view.Type}.{prop.PKey}_elements"
    yield $"    ]"
    yield $"    |> List.append base.SubElements_PropKeys"
  }

let setPropsCode (view: ViewMetadata) =
  if view.HasNoEventsOrProperties then
    Seq.empty
  else
    seq {
      yield $"  override _.setProps(terminalElement: IInternalTerminalElement, props: Props) ="
      yield $"    base.setProps(terminalElement, props)"
      yield $""
      yield! terminalElementAndViewDeclaration view.Type
      yield $""

      if view.Properties.Length > 0 then
        yield "    // Properties"
      for prop in view.Properties do
        yield $"    props"
        yield $"    |> Props.tryFind {pkeyPrefix view.Type}.{prop.PKey}"
        yield $"    |> Option.iter (fun v -> view.{prop.PKey} <- v)"
        yield ""

      if view.Events.Length > 0 then
        yield "    // Events"
      for event in view.Events do
        yield $"    terminalElement.trySetEventHandler({pkeyPrefix view.Type}.{event.PKey}, view.{event.PKey})"
        yield ""

    }

let removePropsCode (view: ViewMetadata) =
  if view.HasNoEventsOrProperties then
    Seq.empty
  else
    seq {
      yield $"  override _.removeProps(terminalElement: IInternalTerminalElement, props: Props) ="
      if not (view.Type = typeof<Terminal.Gui.ViewBase.View>) then
        yield $"    base.removeProps(terminalElement, props)"
      yield $""
      yield! terminalElementAndViewDeclaration view.Type
      yield $""
      if view.Properties.Length > 0 then
        yield "    // Properties"
      for prop in view.Properties do
        yield $"    props"
        yield $"    |> Props.tryFind {pkeyPrefix view.Type}.{prop.PKey}"
        yield $"    |> Option.iter (fun _ ->"
        yield $"        view.{prop.PKey} <- Unchecked.defaultof<_>)"
        yield ""
      if view.Events.Length > 0 then
        yield "    // Events"
      for event in view.Events do
        yield $"    terminalElement.tryRemoveEventHandler {pkeyPrefix view.Type}.{event.PKey}"
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
      let genericBlock = genericTypeParamsWithConstraintsBlock viewType
      let genericParamsBlock = genericTypeParamsBlock viewType
      let viewMetadata = ViewMetadata.create viewType
      let setAsChildOfParentView = setAsChildOfParentView viewType

      if viewType.IsAbstract then
        yield "[<AbstractClass>]"
      yield $"type internal {getTypeNameWithoutArity viewType}TerminalElement{genericBlock}(props: Props) ="
      if viewType = typeof<Terminal.Gui.ViewBase.View> then
        yield $"  inherit TerminalElement(props)"
      else
        yield $"  inherit {(ViewType.parentView viewType).Name}TerminalElement(props)"
      yield ""
      yield $"  override _.name = \"{viewType.Name}\""
      yield ""
      if viewType.IsAbstract then
        yield $"  override _.newView() = failwith \"Cannot instantiate abstract view type {getTypeNameWithoutArity viewType}\""
      else
        yield $"  override _.newView() = new {getTypeNameWithoutArity viewType}{genericParamsBlock}()"
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

      for i in Registry.GetNeededIElementInterfaces viewType do
        yield $"  interface {i}"
        yield ""
  }
  |> String.concat Environment.NewLine
  |> File.writeAllText (Path.Combine (Environment.CurrentDirectory, "TerminalElement.Elements.gen.fs"))
