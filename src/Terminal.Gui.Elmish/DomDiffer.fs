namespace Terminal.Gui.Elmish

open System.Collections.Generic
open Terminal.Gui.ViewBase

/// Phase 3 of plan-staticDom.md: a DomNode-based differ.
///
/// Mirrors the old `Differ.update` child-matching algorithm (type-name grouping +
/// positional index) but patches a persistent `DomNode` tree in place instead of
/// stealing Views between TerminalElements. Sub-elements are owned by the DomNode
/// tree, so the `Reuse` `_view` re-injection dance is unnecessary.
module internal DomDiffer =

  let private nameOf (child: DomChild) =
    match child with
    | ViewDomChild node -> node.Name
    | ElmishComponentDomChild ce -> (ce :> ITerminalElementBase).Name

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

    let prevNames = prevChildren |> List.map nameOf |> List.sort
    let newNames = newChildren |> List.map (fun t -> t.Name) |> List.sort

    if prevNames = newNames then
      // Same shape: match pairwise after sorting by name (mirrors the old Differ).
      let sortedPrev = prevChildren |> List.sortBy nameOf
      let sortedNew = newChildren |> List.sortBy (fun t -> t.Name)
      List.iter2 (fun pc nc -> updateChild teMapping parent pc nc) sortedPrev sortedNew
    else
      // Different shape: group by type name and match positionally within each group.
      let allNames =
        seq {
          yield! prevChildren |> List.map nameOf
          yield! newChildren |> List.map (fun t -> t.Name)
        }
        |> Seq.distinct
        |> Seq.toList

      for name in allNames do
        let prevs = prevChildren |> List.filter (fun c -> nameOf c = name)
        let news = newChildren |> List.filter (fun t -> t.Name = name)
        let minCount = min prevs.Length news.Length

        for i in 0 .. minCount - 1 do
          updateChild teMapping parent prevs[i] news[i]

        // Extra new children: create.
        news
        |> List.skip minCount
        |> List.iter (fun te -> createChild teMapping parent te)

        // Extra old children: dispose.
        prevs
        |> List.skip minCount
        |> List.iter (fun child -> disposeChild parent child)

  and updateChild teMapping (parent: DomNode) (child: DomChild) (te: TerminalElement) =
    match child, te with
    | ViewDomChild dom, ViewTE viewTE ->
      let v = viewTE :?> ViewBackedTerminalElement

      if dom.Name <> v.Name then
        // Type changed at this slot: dispose and re-create.
        let idx = parent.Children |> Seq.findIndex (fun c -> obj.ReferenceEquals(c, child))
        DomNode.dispose dom
        let spec = DomNode.specOf (v :> IViewTE)
        let newDom = DomNode.createFromTE teMapping v spec
        newDom.Origin <- DomChild(parent, idx)

        if newDom.SetAsChildOfParentView then
          parent.View.Add newDom.View |> ignore

        parent.Children[idx] <- ViewDomChild newDom
      else
        update teMapping dom (v :> IViewTE)
    | ElmishComponentDomChild _, ElmishComponentTE _ ->
      // Decision B1: match by name + positional index; keep the existing component instance
      // and its running loop (do NOT StartElmishLoop the new TE). State is preserved while
      // the component stays at the same slot (see plan-staticDom.md, B2 for the follow-up).
      ()
    | _ -> ()

  and createChild teMapping (parent: DomNode) (te: TerminalElement) =
    match te with
    | ViewTE viewTE ->
      let v = viewTE :?> ViewBackedTerminalElement
      let spec = DomNode.specOf (v :> IViewTE)
      let dom = DomNode.createFromTE teMapping v spec
      let idx = parent.Children.Count
      dom.Origin <- DomChild(parent, idx)

      if dom.SetAsChildOfParentView then
        parent.View.Add dom.View |> ignore

      parent.Children.Add(ViewDomChild dom)
    | ElmishComponentTE ce ->
      let idx = parent.Children.Count
      // The component's own DomOrigin resolves its root view's placement, so starting its
      // loop wires the view into the parent view; no explicit Add is needed.
      ce.DomOrigin <- DomChild(parent, idx)
      ce.StartElmishLoop()
      parent.Children.Add(ElmishComponentDomChild ce)

  and disposeChild (parent: DomNode) (child: DomChild) =
    match child with
    | ViewDomChild dom ->
      DomNode.dispose dom
      parent.Children.Remove child |> ignore
    | ElmishComponentDomChild ce ->
      (ce :> ITerminalElementBase).Dispose()
      parent.Children.Remove child |> ignore

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
  /// Nodes are matched positionally and require a stable <c>Name</c>. A child slot may change
  /// element type (its DomNode is disposed and re-created), but the root type must be unchanged;
  /// otherwise this throws.
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
