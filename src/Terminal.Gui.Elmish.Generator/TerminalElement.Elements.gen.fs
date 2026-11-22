module Terminal.Gui.Elmish.Generator.TerminalElement_Elements

open System
open Terminal.Gui.Elmish.Generator

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
      let genericBlock = genericTypeParamsWithConstraintsBlock viewType
      let genericParamsBlock = genericTypeParamsBlock viewType

      if viewType.IsAbstract then
        yield "[<AbstractClass>]"

      yield $"type internal {getTypeNameWithoutArity viewType}TerminalElement{genericBlock}(props: Props) ="

      if viewType = typeof<Terminal.Gui.ViewBase.View> then
        yield $"  inherit ViewBackedTerminalElement(props)"
      else
        let genericBlock = genericTypeParamsBlock viewType.ParentViewType
        yield $"  inherit {getTypeNameWithoutArity viewType.ParentViewType}TerminalElement{genericBlock}(props)"

      yield ""
      yield $"  override _.Name = \"{viewType.Name}\""
      yield ""

      if viewType.IsAbstract then
        yield
          $"  override _.NewView() = failwith \"Cannot instantiate abstract view type {getTypeNameWithoutArity viewType}\""
      else
        yield $"  override _.NewView() = new {getTypeNameWithoutArity viewType}{genericParamsBlock}()"

      yield ""
      yield $"  override _.SetAsChildOfParentView = %b{(setAsChildOfParentView viewType)}"
      yield ""

      for i in Registry.TEInterfaces.GetAllPreviouslyCreatedInterfaces viewType do
        yield $"  interface {i}"
        yield ""

      // IDomSpecProvider implementation referencing the companion *Dom module
      let domModuleName = $"{Registry.ViewTypes.GetUniqueTypeName viewType}Dom"

      yield $"  interface IDomSpecProvider with"
      yield $"    member _.DomSpec ="
      yield $"      {{ ApplyProps = {domModuleName}.applyProps{genericParamsBlock}"
      yield $"        RemoveProps = {domModuleName}.removeProps{genericParamsBlock}"
      yield $"        NewView = {domModuleName}.newView{genericParamsBlock}"
      yield $"        SetAsChildOfParentView = {domModuleName}.setAsChildOfParentView"
      yield $"        SubElementsPropKeys = {domModuleName}.subElementsPropKeys }}"
      yield ""
  }
  |> CodeWriter.write "TerminalElement.Elements.gen.fs"
