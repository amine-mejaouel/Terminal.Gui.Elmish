namespace Terminal.Gui.Elmish

open Terminal.Gui.ViewBase

/// Describes the position of a DomNode within the tree.
type internal DomOrigin =
  | DomRoot
  | DomElmishComponent of parent: IElmishComponentTE
  | DomChild of parent: DomNode * index: int
  | DomSubElement of parent: DomNode * index: int option * property: RawPropKey

/// A child of a DomNode — either a view-based DomNode or an Elmish component.
and internal DomChild =
  | ViewDomChild of DomNode
  | ElmishComponentDomChild of IElmishComponentTE

/// Persistent DOM node that owns a Terminal.Gui View.
/// Created from a TerminalElement description and mutated in place during diffs.
and [<AllowNullLiteral>] internal DomNode
  (
    name: string,
    view: View,
    appliedProps: Props,
    applyProps: DomNode * Props -> unit,
    removeProps: DomNode * Props -> unit,
    newView: unit -> View,
    setAsChildOfParentView: bool,
    subElementsPropKeys: RawPropKey list
  ) =

  let viewSetEvent = Event<View>()

  member _.Name = name
  member val View = view with get, set
  member val Origin: DomOrigin = DomRoot with get, set
  member val AppliedProps: Props = appliedProps with get, set
  member val EventRegistrar: EventHandlerRegistrar = EventHandlerRegistrar() with get, set
  member val Children = ResizeArray<DomChild>() with get
  member val SubElementDomNodes = ResizeArray<DomNode>() with get
  member _.SetAsChildOfParentView = setAsChildOfParentView
  member _.SubElementsPropKeys = subElementsPropKeys

  member this.ApplyProps(props) = applyProps (this, props)
  member this.RemoveProps(props) = removeProps (this, props)
  member _.NewView() = newView ()

  /// Fires the ViewReady event, analogous to ViewSet on ViewBackedTerminalElement.
  /// Called by createFromTE after the DomNode is fully initialized.
  member this.TriggerViewReady() = viewSetEvent.Trigger this.View

  [<CLIEvent>]
  member _.OnViewSet = viewSetEvent.Publish

/// Bundles the DomNode-compatible applicator functions for a TE type.
/// Generated per-element by the DomNode.Elements generator.
type internal DomNodeSpec =
  { ApplyProps: DomNode * Props -> unit
    RemoveProps: DomNode * Props -> unit
    NewView: unit -> View
    SetAsChildOfParentView: bool
    SubElementsPropKeys: RawPropKey list }

/// Implemented by each generated TerminalElement class to expose its DomNodeSpec.
type internal IDomSpecProvider =
  abstract DomSpec: DomNodeSpec
