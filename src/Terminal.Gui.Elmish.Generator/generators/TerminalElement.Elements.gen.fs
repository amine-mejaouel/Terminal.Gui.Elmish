module Terminal.Gui.Elmish.Generator.TerminalElement_viewSpecs

open System
open Terminal.Gui.Elmish.Generator

let propHandlerTypeName (viewType: Type) =
  $"{getTypeNameWithoutArity viewType}PropHandler{genericTypeParamsBlock viewType}"

let subElementsPropKeys (view: ViewMetadata) =
  seq {
    yield $"  override this.SubElements_PropKeys ="
    yield $"    ["

    for prop in view.View_Typed_Properties do
      yield $"      {PKey.getAccessor view.Type}.{prop.PKey}_viewSpec.Untyped"

    yield $"    ]"
    yield $"    |> List.append base.SubElements_PropKeys"
  }

let setPropsCode (view: ViewMetadata) =
  seq {
    yield $"  override _.SetProps(terminalElement: ViewBackedTerminalElement, props: Props) ="
    yield $"    {propHandlerTypeName view.Type}.setProps(terminalElement, props)"
  }

let clearPropCode (view: ViewMetadata) =
  seq {
    yield $"  override this.ClearProp(propertyId: PropertyId) ="
    yield $"    {propHandlerTypeName view.Type}.clearProp(this, propertyId)"
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
  [ "open System.Collections.Generic"
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
      let genericParamsBlock = genericTypeParamsBlock viewType
      let viewMetadata = ViewMetadata.create viewType

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

      if
        viewMetadata.View_Typed_Properties.Length > 0
        || viewMetadata.ViewsCollection_Typed_Properties.Length > 0
      then
        yield! subElementsPropKeys viewMetadata
        yield ""

      yield! setPropsCode viewMetadata
      yield! clearPropCode viewMetadata
      yield ""

  // for i in Registry.TEInterfaces.GetAllPreviouslyCreatedInterfaces viewType do
  //   yield $"  interface {i}"
  //   yield ""
  }
  |> CodeWriter.write "TerminalElement.Elements.gen.fs"
