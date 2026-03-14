namespace Terminal.Gui.Elmish

open System.Collections.Generic
open Terminal.Gui.ViewBase

[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module internal DomNode =

  let parentView (origin: DomOrigin) : View option =
    match origin with
    | DomRoot -> None
    | DomElmishComponent parent -> parent.Origin |> Origin.parentView
    | DomChild(parent, _) -> Some parent.View
    | DomSubElement(parent, _, _) -> Some parent.View

  /// Creates a DomNode from a ViewBackedTerminalElement and its spec.
  ///
  /// Follows the same initialization ordering as ViewBackedTerminalElement.InitializeView():
  ///   1. Create View via NewView()
  ///   2. Initialize sub-element DomNodes
  ///   3. Apply positions
  ///   4. Call applyProps
  let rec createFromTE
    (resolveSpec: IViewTE -> DomNodeSpec)
    (teMapping: Dictionary<ITerminalElement, DomNode>)
    (te: ViewBackedTerminalElement)
    (spec: DomNodeSpec)
    : DomNode =

    // 1. Create View
    let view = spec.NewView()
    let props = te.Props

    let domNode =
      DomNode(
        te.Name,
        view,
        props,
        spec.ApplyProps,
        spec.RemoveProps,
        spec.NewView,
        spec.SetAsChildOfParentView,
        spec.SubElementsPropKeys
      )

    teMapping[te] <- domNode

    // 2. Initialize sub-element DomNodes
    for subKey in spec.SubElementsPropKeys do
      match props |> Props.tryFind (PropKeyKind.SubElement, subKey) with
      | None -> ()
      | Some value ->
        match value with
        | :? ViewBackedTerminalElement as subElement ->
          let subSpec = resolveSpec (subElement :> IViewTE)
          let subDomNode = createFromTE resolveSpec teMapping subElement subSpec
          subDomNode.Origin <- DomSubElement(domNode, None, subKey)
          domNode.SubElementDomNodes.Add subDomNode

          let viewKey = PropKey.viewKeyOfSubElement subKey
          props |> Props.add (viewKey, subDomNode.View :> obj)

        | :? List<IViewTE> as elements ->
          let views =
            elements
            |> Seq.mapi (fun i e ->
              let subTe = e :?> ViewBackedTerminalElement
              let subSpec = resolveSpec e
              let subDomNode = createFromTE resolveSpec teMapping subTe subSpec
              subDomNode.Origin <- DomSubElement(domNode, Some i, subKey)
              domNode.SubElementDomNodes.Add subDomNode
              subDomNode.View)
            |> Seq.toList

          let viewKey = PropKey.viewKeyOfSubElement subKey
          props |> Props.add (viewKey, views :> obj)

        | _ -> failwith "Out of range subElement type"

    // 3. Apply positions — simple cases handled here, TPos deferred to Phase 5
    match props.X, props.XDelayed with
    | Some _, Some _ -> failwith "Cannot set both X and XDelayedPos on the same view."
    | Some xPos, None -> view.X <- xPos
    | None, Some _ -> () // TODO: Phase 5 — resolve TPos via PositionService + teMapping
    | None, None -> view.X <- 0

    match props.Y, props.YDelayed with
    | Some _, Some _ -> failwith "Cannot set both Y and YDelayedPos on the same view."
    | Some yPos, None -> view.Y <- yPos
    | None, Some _ -> () // TODO: Phase 5 — resolve TPos via PositionService + teMapping
    | None, None -> view.Y <- 0

    // 4. Apply props
    domNode.ApplyProps(props)

    domNode.TriggerViewReady()
    domNode

  /// Recursively creates a DomNode tree from a TerminalElement tree.
  /// Returns the root DomChild and a mapping from ITerminalElement to DomNode.
  let initializeTree
    (resolveSpec: IViewTE -> DomNodeSpec)
    (origin: DomOrigin)
    (te: TerminalElement)
    : DomChild * Dictionary<ITerminalElement, DomNode> =

    let teMapping = Dictionary<ITerminalElement, DomNode>()

    let rec initNode (origin: DomOrigin) (te: TerminalElement) : DomChild =
      match te with
      | ElmishComponentTE elmishComponent ->
        let originForComponent =
          match origin with
          | DomRoot -> Origin.Root
          | DomElmishComponent parent -> Origin.ElmishComponent parent
          // DomChild/DomSubElement → Origin.Child/SubElement requires IViewTE.
          // Full support deferred to Phase 4.
          | _ -> Origin.Root

        elmishComponent.Origin <- originForComponent
        elmishComponent.StartElmishLoop()

        match origin with
        | DomChild _
        | DomElmishComponent _ -> parentView origin |> Option.iter (fun v -> v.Add elmishComponent.View |> ignore)
        | _ -> ()

        ElmishComponentDomChild elmishComponent

      | ViewTE viewTE ->
        let te = viewTE :?> ViewBackedTerminalElement
        let spec = resolveSpec viewTE
        let domNode = createFromTE resolveSpec teMapping te spec
        domNode.Origin <- origin

        let isChild =
          match origin with
          | DomChild _
          | DomElmishComponent _ -> true
          | _ -> false

        if isChild && domNode.SetAsChildOfParentView then
          parentView origin |> Option.iter (fun v -> v.Add domNode.View |> ignore)

        // Recursively initialize children
        te.Children
        |> Seq.iteri (fun i child ->
          let childOrigin = DomChild(domNode, i)
          let childDom = initNode childOrigin child
          domNode.Children.Add childDom)

        ViewDomChild domNode

    let rootChild = initNode origin te
    rootChild, teMapping

  /// Recursively disposes a DomNode: removes event handlers, detaches from parent,
  /// disposes sub-elements, disposes children, cleans up positions, and disposes the View.
  let rec dispose (domNode: DomNode) : unit =
    // 1. Remove event handlers
    domNode.RemoveProps(domNode.AppliedProps)

    // 2. Remove from parent view
    parentView domNode.Origin
    |> Option.iter (fun v -> v.Remove domNode.View |> ignore)

    // 3. Dispose sub-element DomNodes
    for subDom in domNode.SubElementDomNodes do
      dispose subDom

    // 4. Dispose children
    for child in domNode.Children do
      match child with
      | ViewDomChild childDom -> dispose childDom
      | ElmishComponentDomChild ec -> (ec :> ITerminalElementBase).Dispose()

    // 5. Execute position cleanups
    // TODO: Phase 5 — PositionService.ExecuteCleanups adapted for DomNode

    // 6. Dispose the View itself
    domNode.View.Dispose()
