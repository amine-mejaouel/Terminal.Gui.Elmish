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

  /// Resolves the generated DOM applicator spec for a view terminal element.
  /// Every generated `*TerminalElement` implements `IDomSpecProvider`.
  let specOf (te: IViewTE) : DomNodeSpec =
    match te with
    | :? IDomSpecProvider as provider -> provider.DomSpec
    | _ -> failwith $"Terminal element '{te.Name}' does not implement IDomSpecProvider."

  /// Creates a DomNode from a ViewBackedTerminalElement and its spec.
  ///
  /// Follows the same initialization ordering as ViewBackedTerminalElement.InitializeView():
  ///   1. Create View via NewView()
  ///   2. Initialize sub-element DomNodes
  ///   3. Apply positions
  ///   4. Call applyProps
  let rec createFromTE
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
          let subSpec = specOf (subElement :> IViewTE)
          let subDomNode = createFromTE teMapping subElement subSpec
          subDomNode.Origin <- DomSubElement(domNode, None, subKey)
          domNode.SubElementDomNodes.Add subDomNode

          let viewKey = PropKey.viewKeyOfSubElement subKey
          props |> Props.add (viewKey, subDomNode.View :> obj)

        | :? List<IViewTE> as elements ->
          let views =
            elements
            |> Seq.mapi (fun i e ->
              let subTe = e :?> ViewBackedTerminalElement
              let subSpec = specOf e
              let subDomNode = createFromTE teMapping subTe subSpec
              subDomNode.Origin <- DomSubElement(domNode, Some i, subKey)
              domNode.SubElementDomNodes.Add subDomNode
              subDomNode.View)
            |> Seq.toList

          let viewKey = PropKey.viewKeyOfSubElement subKey
          props |> Props.add (viewKey, views :> obj)

        | _ -> failwith "Out of range subElement type"

    // 3. Apply props. Positions are applied in a deferred pass (decision A2) once the whole
    //    tree and its TE->DomNode mapping exist — see DomPositionService.ApplyTree.
    domNode.ApplyProps(props)

    // 4. Initialize children recursively (via buildNode). This is also required for
    //    sub-element subtrees: e.g. a Menu inside a PopoverMenu must get its MenuItems.
    te.Children
    |> Seq.iteri (fun i child -> domNode.Children.Add(buildNode teMapping (DomChild(domNode, i)) child))

    domNode.TriggerViewReady()
    domNode

  /// Builds a DomChild for a TerminalElement at the given origin: creates the node, wires
  /// its view into the parent view (for view children), and recurses into the subtree.
  and buildNode
    (teMapping: Dictionary<ITerminalElement, DomNode>)
    (origin: DomOrigin)
    (te: TerminalElement)
    : DomChild =
    match te with
    | ElmishComponentTE elmishComponent ->
      let originForComponent =
        match origin with
        | DomRoot -> Origin.Root
        | DomElmishComponent parent -> Origin.ElmishComponent parent
        // DomChild/DomSubElement → Origin.Child/SubElement requires IViewTE.
        // Full support deferred (see plan-staticDom.md, decision A).
        | _ -> Origin.Root

      elmishComponent.Origin <- originForComponent
      elmishComponent.StartElmishLoop()

      match origin with
      | DomChild _
      | DomElmishComponent _ -> parentView origin |> Option.iter (fun v -> v.Add elmishComponent.View |> ignore)
      | _ -> ()

      ElmishComponentDomChild elmishComponent

    | ViewTE viewTE ->
      let vte = viewTE :?> ViewBackedTerminalElement
      let spec = specOf viewTE
      let domNode = createFromTE teMapping vte spec
      domNode.Origin <- origin

      let isChild =
        match origin with
        | DomChild _
        | DomElmishComponent _ -> true
        | _ -> false

      if isChild && domNode.SetAsChildOfParentView then
        parentView origin |> Option.iter (fun v -> v.Add domNode.View |> ignore)

      ViewDomChild domNode

  /// Recursively creates a DomNode tree from a TerminalElement tree.
  /// Returns the root DomChild and a mapping from ITerminalElement to DomNode.
  let initializeTree (origin: DomOrigin) (te: TerminalElement) : DomChild * Dictionary<ITerminalElement, DomNode> =

    let teMapping = Dictionary<ITerminalElement, DomNode>()
    let rootChild = buildNode teMapping origin te

    // Deferred position pass (decision A2): the whole tree and mapping now exist.
    let resolve (te: ITerminalElement) =
      match teMapping.TryGetValue te with
      | true, node -> Some node
      | _ -> None

    match rootChild with
    | ViewDomChild rootDom -> DomPositionService.Current.ApplyTree(rootDom, resolve)
    | ElmishComponentDomChild _ -> ()

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
    DomPositionService.Current.ExecuteCleanups domNode

    // 6. Dispose the View itself
    domNode.View.Dispose()
