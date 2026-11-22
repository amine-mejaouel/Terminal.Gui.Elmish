namespace Terminal.Gui.Elmish

open System.Collections.Generic
open Terminal.Gui.ViewBase

/// Phase 3 of plan-staticDom.md: a DomNode-based differ.
///
/// View children are matched by type-name grouping + positional index (a direct port of the
/// old `Differ.update` child-matching algorithm). Elmish component children are matched by
/// `ComponentId` (decision B2) so a component instance and its running loop/state follow the
/// component across reorders and conditional insertion. Sub-elements are owned by the DomNode
/// tree, so the `Reuse` `_view` re-injection dance is unnecessary.
module internal DomDiffer =

  let private existingSubDoms (parent: DomNode) (subKey: RawPropKey) =
    parent.SubElementDomNodes
    |> Seq.filter (fun dom ->
      match dom.Origin with
      | DomSubElement(p, _, k) -> obj.ReferenceEquals(p, parent) && k = subKey
      | _ -> false)
    |> Seq.toList

  let private orderedSubDoms (doms: DomNode list) =
    doms
    |> List.sortBy (fun dom ->
      match dom.Origin with
      | DomSubElement(_, Some i, _) -> i
      | _ -> -1)

  let private subElementEquivalent (oldObj: obj) (newObj: obj) =
    match oldObj, newObj with
    | (:? ViewBackedTerminalElement as o), (:? ViewBackedTerminalElement as n) -> o.equivalentTo n
    | (:? List<IViewTE> as o), (:? List<IViewTE> as n) ->
      o.Count = n.Count
      && Seq.forall2
        (fun (a: IViewTE) (b: IViewTE) ->
          (a :?> ViewBackedTerminalElement).equivalentTo (b :?> ViewBackedTerminalElement))
        o
        n
    | _ -> false

  let rec update (teMapping: Dictionary<ITerminalElement, DomNode>) (currentDom: DomNode) (newTE: IViewTE) : unit =

    if currentDom.Name <> newTE.Name then
      failwith $"DomDiffer.update expects stable node names; got '{currentDom.Name}' vs '{newTE.Name}'."

    // Register the mapping so TPos resolution (DomPositionService) can find this node.
    teMapping[newTE] <- currentDom

    // 1. Sub-elements first, so the `_view` props are present for the generated applicators.
    reconcileSubElements teMapping currentDom newTE

    // 2. Props: diff everything except sub-element keys (handled above).
    //    The generated applyProps/removeProps ignore `_element` keys and handle `_view` keys.
    let prevNoSub, _ =
      currentDom.AppliedProps
      |> Props.partition (fun kv -> kv.Key.Kind <> PropKeyKind.SubElement)

    let curNoSub, _ =
      newTE.Props |> Props.partition (fun kv -> kv.Key.Kind <> PropKeyKind.SubElement)

    let diff = ViewBackedTerminalElement.compare prevNoSub curNoSub
    currentDom.RemoveProps diff.removedProps
    currentDom.ApplyProps diff.changedProps

    // 3. Children.
    reconcileChildren teMapping currentDom newTE

    // 4. Adopt the new props (with the `_view` props injected above).
    //    Positions are applied by the deferred pass in updateTree (decision A2).
    currentDom.AppliedProps <- newTE.Props

  and reconcileSubElements teMapping (parent: DomNode) (newTE: IViewTE) =
    let prev = parent.AppliedProps
    let cur = newTE.Props

    for subKey in parent.SubElementsPropKeys do
      let viewKey = PropKey.viewKeyOfSubElement subKey
      let prevVal = prev |> Props.tryFind (PropKeyKind.SubElement, subKey)
      let newVal = cur |> Props.tryFind (PropKeyKind.SubElement, subKey)

      match newVal with
      | None ->
        // Sub-element removed.
        for dom in existingSubDoms parent subKey do
          DomNode.dispose dom
          parent.SubElementDomNodes.Remove dom |> ignore

      | Some nv ->
        match prevVal with
        | Some pv when subElementEquivalent pv nv ->
          // Unchanged: keep existing sub-element DomNodes and re-inject their Views.
          let doms = orderedSubDoms (existingSubDoms parent subKey)

          match nv with
          | :? ViewBackedTerminalElement ->
            match doms with
            | [ dom ] ->
              teMapping[(nv :?> ITerminalElement)] <- dom
              cur |> Props.add (viewKey, dom.View :> obj)
            | _ -> ()
          | :? List<IViewTE> as elements ->
            let views = doms |> List.map (fun d -> d.View)

            elements
            |> Seq.iteri (fun i e ->
              if i < doms.Length then
                teMapping[e] <- doms[i])

            cur |> Props.add (viewKey, views :> obj)
          | _ -> failwith "Out of range subElement type"

        | _ ->
          // Changed or new: drop the old sub-element DomNodes and rebuild.
          for dom in existingSubDoms parent subKey do
            DomNode.dispose dom
            parent.SubElementDomNodes.Remove dom |> ignore

          match nv with
          | :? ViewBackedTerminalElement as subTE ->
            let spec = DomNode.specOf (subTE :> IViewTE)
            let dom = DomNode.createFromTE teMapping subTE spec
            dom.Origin <- DomSubElement(parent, None, subKey)
            parent.SubElementDomNodes.Add dom
            cur |> Props.add (viewKey, dom.View :> obj)

          | :? List<IViewTE> as elements ->
            let views =
              elements
              |> Seq.mapi (fun i e ->
                let subTe = e :?> ViewBackedTerminalElement
                let spec = DomNode.specOf e
                let dom = DomNode.createFromTE teMapping subTe spec
                dom.Origin <- DomSubElement(parent, Some i, subKey)
                parent.SubElementDomNodes.Add dom
                dom.View)
              |> Seq.toList

            cur |> Props.add (viewKey, views :> obj)

          | _ -> failwith "Out of range subElement type"

  and reconcileChildren teMapping (parent: DomNode) (newTE: IViewTE) =
    let prevChildren = parent.Children |> Seq.toList
    let newChildren = newTE.Children |> Seq.toList

    // --- Components (decision B2) -------------------------------------------
    // Match component instances by `ComponentId` so an instance and its running loop /
    // state survive reorders and conditional insertion, instead of being bound to a
    // positional slot. Only non-blank ids that are unique within this render are
    // trackable; anything else falls back to the positional, name-based matching used
    // for view children.
    let newComponentIds =
      newChildren
      |> List.choose (function
        | ElmishComponentTE ce -> Some ce.ComponentId
        | _ -> None)

    let uniqueNewIds =
      newComponentIds
      |> List.filter (fun id -> id <> "")
      |> List.countBy id
      |> List.choose (fun (id, count) -> if count = 1 then Some id else None)

    let prevById =
      prevChildren
      |> List.choose (function
        | ElmishComponentDomChild ce when uniqueNewIds |> List.contains ce.ComponentId -> Some(ce.ComponentId, ce)
        | _ -> None)
      |> Map.ofList

    // Pool of previous children still available for positional matching. ID-matched
    // components (and their previous children) are reserved up front so they cannot be
    // matched a second time.
    let available = ResizeArray<DomChild>(prevChildren)

    let takePrev (predicate: DomChild -> bool) =
      match available |> Seq.tryFindIndex predicate with
      | Some i ->
        let child = available[i]
        available.RemoveAt i
        Some child
      | None -> None

    let idMatched = Dictionary<string, IElmishComponentTE>()

    for id in uniqueNewIds do
      match prevById.TryFind id with
      | Some ce ->
        idMatched[id] <- ce

        let i =
          available
          |> Seq.findIndex (function
            | ElmishComponentDomChild c -> obj.ReferenceEquals(c, ce)
            | _ -> false)

        available.RemoveAt i
      | None -> ()

    // Resolve each new child to a DomChild, in the new order.
    let resolveChild index (te: TerminalElement) : DomChild =
      match te with
      | ElmishComponentTE ce when ce.ComponentId <> "" && idMatched.ContainsKey ce.ComponentId ->
        // Reuse the instance and its running loop; only its placement changed.
        let prevCe = idMatched[ce.ComponentId]
        prevCe.DomOrigin <- DomChild(parent, index)
        ElmishComponentDomChild prevCe

      | ElmishComponentTE ce ->
        // Untrackable (blank or duplicated id): keep the previous same-name instance if
        // one is free, otherwise start a new loop.
        match
          takePrev (function
            | ElmishComponentDomChild prevCe -> prevCe.ComponentId = ce.ComponentId
            | _ -> false)
        with
        | Some(ElmishComponentDomChild prevCe) ->
          prevCe.DomOrigin <- DomChild(parent, index)
          ElmishComponentDomChild prevCe
        | _ ->
          ce.DomOrigin <- DomChild(parent, index)
          ce.StartElmishLoop()
          ElmishComponentDomChild ce

      | ViewTE viewTE ->
        let v = viewTE :?> ViewBackedTerminalElement

        match
          takePrev (function
            | ViewDomChild dom -> dom.Name = v.Name
            | _ -> false)
        with
        | Some(ViewDomChild dom) ->
          update teMapping dom (v :> IViewTE)
          dom.Origin <- DomChild(parent, index)
          ViewDomChild dom
        | _ ->
          let spec = DomNode.specOf (v :> IViewTE)
          let dom = DomNode.createFromTE teMapping v spec
          dom.Origin <- DomChild(parent, index)

          if dom.SetAsChildOfParentView then
            parent.View.Add dom.View |> ignore

          ViewDomChild dom

    let resolved = newChildren |> List.mapi resolveChild

    // Dispose previous children that were not reused.
    for leftover in available do
      disposeChild leftover

    // Rebuild the child list in the new order.
    parent.Children.Clear()
    parent.Children.AddRange resolved

  and disposeChild (child: DomChild) =
    match child with
    | ViewDomChild dom -> DomNode.dispose dom
    | ElmishComponentDomChild ce -> (ce :> ITerminalElementBase).Dispose()

  /// <summary>
  /// Patches the persistent <paramref name="currentRoot" /> DomNode tree in place so that it
  /// matches the <paramref name="newRootTE" /> description.
  /// </summary>
  /// <param name="currentRoot">
  /// The persistent DomNode tree. It owns the Views and retains the previous render's props in
  /// each node's <c>AppliedProps</c>; that retained state is the "old" side of the diff, so no
  /// previous TE tree is needed.
  /// </param>
  /// <param name="newRootTE">
  /// The new description to sync the tree to. It is read-only and discarded afterwards: it is
  /// never given a View, stored, or disposed.
  /// </param>
  /// <remarks>
  /// View children are matched positionally and require a stable <c>Name</c>. Elmish component
  /// children are matched by <c>ComponentId</c>, so a component keeps its instance and running
  /// loop across reorders. A child slot may change element type (its DomNode is disposed and
  /// re-created), but the root type must be unchanged; otherwise this throws.
  /// </remarks>
  let updateTree (currentRoot: DomNode) (newRootTE: IViewTE) =
    let teMapping = Dictionary<ITerminalElement, DomNode>()
    update teMapping currentRoot newRootTE

    // Deferred position pass (decision A2): the whole tree and mapping now exist.
    let resolve (te: ITerminalElement) =
      match teMapping.TryGetValue te with
      | true, node -> Some node
      | _ -> None

    DomPositionService.Current.ApplyTree(currentRoot, resolve)
