namespace Terminal.Gui.Elmish

open System
open System.Collections.Generic
open System.Runtime.CompilerServices
open Terminal.Gui.App
open Terminal.Gui.ViewBase

type private ReferenceComparer<'T when 'T: not struct>() =
  interface IEqualityComparer<'T> with
    member _.Equals(left, right) = obj.ReferenceEquals(left, right)
    member _.GetHashCode(value) = RuntimeHelpers.GetHashCode value

type internal MountedNode(spec: ViewSpec, element: TerminalElement) =
  member val Spec = spec with get, set
  member _.Element = element

  /// <summary>
  /// Ordered child nodes declared through <c>p.Children</c>. This collection mirrors the order of the parent's
  /// <c>SubViews</c>: keyed children are matched by key, while unkeyed children are matched by position and view type.
  /// </summary>
  member val Children = ResizeArray<MountedNode>()

  /// <summary>
  /// Child nodes stored in named view properties, such as <c>Shortcut.CommandView</c>, instead of in
  /// <c>p.Children</c>. Each dictionary entry maps the generated property ID to the node mounted for that property.
  /// Slots are reconciled independently and are not part of the parent's ordered <c>SubViews</c> collection.
  /// </summary>
  member val Slots = Dictionary<PropertyId, MountedNode>()
  member val IsDisposed = false with get, set

  member this.View = this.Element.View

  member this.ViewTE =
    match this.Element with
    | TerminalElement.ViewTE viewTe -> viewTe
    | TerminalElement.ElmishComponentTE componentTe -> componentTe.Child

[<RequireQualifiedAccess>]
module internal VirtualTree =

  let private specFromView (value: obj) = value :?> IView |> ViewSpec.from

  let private sameKind left right =
    match left, right with
    | ViewSpec.SimpleViewSpec leftView, ViewSpec.SimpleViewSpec rightView -> leftView.GetType() = rightView.GetType()
    | ViewSpec.ComponentViewSpec leftComponent, ViewSpec.ComponentViewSpec rightComponent ->
      leftComponent.ComponentType = rightComponent.ComponentType
    | _ -> false

  let private sameIdentity left right =
    sameKind left right && ViewSpec.key left = ViewSpec.key right

  let private terminalElementOf =
    function
    | ViewSpec.SimpleViewSpec view -> view.CreateViewTE() |> TerminalElement.ViewTE
    | ViewSpec.ComponentViewSpec componentSpec ->
      componentSpec.ResolveTerminalElement() |> TerminalElement.ElmishComponentTE

  let rec private captureMountedTree spec element =
    ViewSpec.bind spec element

    let mounted = MountedNode(spec, element)

    match spec, element with
    | ViewSpec.SimpleViewSpec simpleView, TerminalElement.ViewTE viewTe ->
      let childElements = viewTe.Children

      if simpleView.Props.Children.Count <> childElements.Count then
        invalidOp
          $"The initialized {simpleView.GetType().Name} hierarchy contains {childElements.Count} terminal elements, but its specification contains {simpleView.Props.Children.Count} children."

      for index = 0 to simpleView.Props.Children.Count - 1 do
        mounted.Children.Add(captureMountedTree simpleView.Props.Children[index] childElements[index])

      let backed = viewTe :?> ViewBackedTerminalElement

      for key in backed.SubElements_PropKeys do
        match simpleView.Props |> Props.tryFind key with
        | Some value ->
          let slotSpec = specFromView value
          let slotElement = terminalElementOf slotSpec
          mounted.Slots[key.Id] <- captureMountedTree slotSpec slotElement
        | None -> ()

    | ViewSpec.ComponentViewSpec _, TerminalElement.ElmishComponentTE _ -> ()
    | _ -> invalidOp "The view specification and mounted terminal element kinds do not match."

    mounted

  let private addComponentToParent (componentTe: IElmishComponentTE) =
    match componentTe.Origin with
    | Origin.Child(parent, index) ->
      let added =
        parent.View.AddAt(min index parent.View.SubViews.Count, componentTe.View)

      if isNull added then
        invalidOp $"Terminal.Gui cancelled mounting component '{componentTe.Name}' at child index {index}."
    | Origin.Root -> invalidOp "An Elmish component cannot be the root Terminal.Gui runnable."
    | Origin.SubElement _ -> invalidOp "Elmish components are not supported in view-valued property slots."
    | Origin.ElmishComponent _ -> ()

  let private mount (application: IApplication) origin spec =
    let element = terminalElementOf spec

    match element with
    | TerminalElement.ViewTE viewTe -> viewTe.InitializeTree(origin, application)
    | TerminalElement.ElmishComponentTE componentTe ->
      componentTe.Origin <- origin
      componentTe.StartElmishLoop(application)
      addComponentToParent componentTe

    captureMountedTree spec element

  let private markDisposed (root: MountedNode) =
    let stack = Stack<MountedNode>()
    stack.Push root

    while stack.Count > 0 do
      let current = stack.Pop()
      current.IsDisposed <- true

      for child in current.Children do
        stack.Push child

      for slot in current.Slots.Values do
        stack.Push slot

  let private unmount (node: MountedNode) =
    if not node.IsDisposed then
      markDisposed node
      node.Element.Dispose()
      node.Children.Clear()
      node.Slots.Clear()

  let private resolvedViewReference (view: IView) =
    try
      (TerminalElement.from view).View
    with _ ->
      null

  let private sameReferencedView left right =
    obj.ReferenceEquals(resolvedViewReference left, resolvedViewReference right)

  let private samePosition left right =
    match left, right with
    | TPos.Default, TPos.Default
    | TPos.Center, TPos.Center -> true
    | TPos.Absolute left, TPos.Absolute right
    | TPos.Percent left, TPos.Percent right -> left = right
    | TPos.AnchorEnd left, TPos.AnchorEnd right -> left = right
    | TPos.Align(leftAlignment, leftModes, leftGroup), TPos.Align(rightAlignment, rightModes, rightGroup) ->
      leftAlignment = rightAlignment
      && leftModes = rightModes
      && leftGroup = rightGroup
    | TPos.X left, TPos.X right
    | TPos.Y left, TPos.Y right
    | TPos.Top left, TPos.Top right
    | TPos.Bottom left, TPos.Bottom right
    | TPos.Left left, TPos.Left right
    | TPos.Right left, TPos.Right right -> sameReferencedView left right
    | TPos.Func(leftFunc, leftView), TPos.Func(rightFunc, rightView) ->
      obj.ReferenceEquals(leftFunc, rightFunc)
      && sameReferencedView leftView rightView
    | _ -> false

  let private validateKeys (parentName: string) (specs: ViewSpec array) =
    let keyedCount =
      specs |> Array.sumBy (fun spec -> if (ViewSpec.key spec).IsSome then 1 else 0)

    if keyedCount <> 0 && keyedCount <> specs.Length then
      invalidOp $"Children of '{parentName}' cannot mix keyed and unkeyed view specifications."

    if keyedCount = specs.Length then
      let keys = HashSet<string>(StringComparer.Ordinal)

      for spec in specs do
        let key = (ViewSpec.key spec).Value

        if not (keys.Add key) then
          invalidOp $"Children of '{parentName}' contain the duplicate key '{key}'."

    keyedCount = specs.Length && specs.Length > 0

  let private longestIncreasingSubsequencePositions (values: int array) =
    let keep = Array.create values.Length false

    if values.Length > 0 then
      let tails = Array.zeroCreate<int> values.Length
      let tailPositions = Array.zeroCreate<int> values.Length
      let predecessors = Array.create values.Length -1
      let mutable length = 0

      for index = 0 to values.Length - 1 do
        let value = values[index]

        if value >= 0 then
          let mutable low = 0
          let mutable high = length

          while low < high do
            let middle = (low + high) / 2

            if tails[middle] < value then
              low <- middle + 1
            else
              high <- middle

          if low > 0 then
            predecessors[index] <- tailPositions[low - 1]

          tails[low] <- value
          tailPositions[low] <- index

          if low = length then
            length <- length + 1

      if length > 0 then
        let mutable index = tailPositions[length - 1]

        while index >= 0 do
          keep[index] <- true
          index <- predecessors[index]

    keep

  let private isAttachedChild (node: MountedNode) =
    match node.Element with
    | TerminalElement.ViewTE viewTe -> viewTe.SetAsChildOfParentView
    | TerminalElement.ElmishComponentTE _ -> true

  let private indexOfView (parent: View) (view: View) =
    parent.SubViews
    |> Seq.tryFindIndex (fun candidate -> obj.ReferenceEquals(candidate, view))

  let private moveView (parent: View) (view: View) targetIndex =
    match indexOfView parent view with
    | None -> invalidOp $"Cannot reorder '{view}': it is not a SubView of '{parent}'."
    | Some currentIndex when currentIndex = targetIndex -> ()
    | Some currentIndex when abs (currentIndex - targetIndex) <= 4 ->
      let mutable currentIndex = currentIndex

      while currentIndex > targetIndex do
        parent.MoveSubViewTowardsStart view
        currentIndex <- currentIndex - 1

      while currentIndex < targetIndex do
        parent.MoveSubViewTowardsEnd view
        currentIndex <- currentIndex + 1
    | Some _ ->
      let removed = parent.Remove view

      if isNull removed then
        invalidOp $"Terminal.Gui cancelled moving '{view}' within '{parent}'."

      let added = parent.AddAt(min targetIndex parent.SubViews.Count, view)

      if isNull added then
        invalidOp $"Terminal.Gui cancelled reinserting '{view}' within '{parent}'."

  let private updateChildOrigins (parent: IViewTE) (children: ResizeArray<MountedNode>) =
    for index = 0 to children.Count - 1 do
      children[index].Element.Origin <- Origin.Child(parent, index)

  let private reorderChildren
    (parent: IViewTE)
    (oldChildren: MountedNode array)
    (newChildren: ResizeArray<MountedNode>)
    =
    let oldAttachedIndexes =
      Dictionary<MountedNode, int>(ReferenceComparer<MountedNode>())

    let mutable oldAttachedIndex = 0

    for child in oldChildren do
      if isAttachedChild child then
        oldAttachedIndexes[child] <- oldAttachedIndex
        oldAttachedIndex <- oldAttachedIndex + 1

    let desired = newChildren |> Seq.filter isAttachedChild |> Seq.toArray

    if desired.Length > 0 then
      let oldIndexes =
        desired
        |> Array.map (fun child ->
          match oldAttachedIndexes.TryGetValue child with
          | true, index -> index
          | false, _ -> -1)

      let keep = longestIncreasingSubsequencePositions oldIndexes

      let currentIndexes =
        desired |> Array.choose (fun child -> indexOfView parent.View child.View)

      if currentIndexes.Length <> desired.Length then
        invalidOp $"The mounted children of '{parent.Name}' diverged from its Terminal.Gui SubViews."

      let baseIndex = Array.min currentIndexes

      for index = desired.Length - 1 downto 0 do
        if oldIndexes[index] < 0 || not keep[index] then
          let targetIndex =
            if index + 1 < desired.Length then
              let nextView = desired[index + 1].View

              match indexOfView parent.View desired[index].View, indexOfView parent.View nextView with
              | Some currentIndex, Some nextIndex when currentIndex < nextIndex -> nextIndex - 1
              | Some _, Some nextIndex -> nextIndex
              | _ -> invalidOp $"Cannot find the keyed move anchor in '{parent.Name}'."
            else
              baseIndex + index

          moveView parent.View desired[index].View targetIndex

      let desiredSet =
        HashSet<View>(desired |> Array.map _.View, ReferenceComparer<View>())

      let actual = parent.View.SubViews |> Seq.filter desiredSet.Contains |> Seq.toArray

      if actual.Length <> desired.Length then
        invalidOp $"The reconciled children of '{parent.Name}' are missing from Terminal.Gui.SubViews."

      for index = 0 to desired.Length - 1 do
        if not (obj.ReferenceEquals(actual[index], desired[index].View)) then
          invalidOp $"Failed to establish the requested child order for '{parent.Name}'."

  let rec private reconcileNode (application: IApplication) (mounted: MountedNode) (nextSpec: ViewSpec) =
    if not (sameIdentity mounted.Spec nextSpec) then
      invalidArg (nameof nextSpec) "reconcileNode requires compatible node identities."

    ViewSpec.bind nextSpec mounted.Element

    match mounted.Spec, nextSpec, mounted.Element with
    | ViewSpec.SimpleViewSpec previous, ViewSpec.SimpleViewSpec next, TerminalElement.ViewTE viewTe ->
      reconcileSlots application mounted next
      reconcileChildren application mounted next

      let previousProps = previous.Props
      let nextProps = next.Props
      let backed = viewTe :?> ViewBackedTerminalElement

      let positionChanged =
        not (samePosition previousProps.X nextProps.X)
        || not (samePosition previousProps.Y nextProps.Y)

      let removedProps, changedProps = Props.diff (previousProps, nextProps)

      backed.Props <- nextProps

      if positionChanged then
        PositionService.Current.ExecuteCleanups viewTe
        PositionService.Current.ApplyPos viewTe

      removedProps |> Option.iter (fun props -> next.RemoveProps(viewTe, props))
      changedProps |> Option.iter (fun props -> next.SetProps(viewTe, props))
      mounted.Spec <- nextSpec

    | ViewSpec.ComponentViewSpec _, ViewSpec.ComponentViewSpec next, TerminalElement.ElmishComponentTE componentTe ->
      componentTe.UpdateProps next.ComponentProps
      next.BindTerminalElement componentTe
      mounted.Spec <- nextSpec

    | _ -> invalidOp "The retained node kind changed during reconciliation."

  and private reconcileSlots (application: IApplication) (mounted: MountedNode) (next: ISimpleViewSpec) =
    let viewTe =
      match mounted.Element with
      | TerminalElement.ViewTE value -> value
      | _ -> invalidOp "Only View nodes can own view-valued property slots."

    let backed = viewTe :?> ViewBackedTerminalElement

    let clearSlotProperty key (slot: MountedNode) =
      let removed = Props()
      removed |> Props.add (PropKey.viewKeyOfSubElement key, slot.View)
      next.RemoveProps(viewTe, removed)

    if mounted.Slots.Count <> 0 || next.Props.SubViewSpecCount <> 0 then
      for key in backed.SubElements_PropKeys do
        let nextSlotSpec = next.Props |> Props.tryFind key |> Option.map specFromView

        let previousSlot =
          match mounted.Slots.TryGetValue key.Id with
          | true, value -> Some value
          | false, _ -> None

        match previousSlot, nextSlotSpec with
        | Some previous, Some nextSpec when sameIdentity previous.Spec nextSpec ->
          ViewSpec.bind nextSpec previous.Element
          reconcileNode application previous nextSpec
        | Some previous, Some nextSpec ->
          clearSlotProperty key previous
          unmount previous
          mounted.Slots[key.Id] <- mount application (Origin.SubElement(viewTe, None, key.Key)) nextSpec
        | None, Some nextSpec ->
          mounted.Slots[key.Id] <- mount application (Origin.SubElement(viewTe, None, key.Key)) nextSpec
        | Some previous, None ->
          clearSlotProperty key previous
          unmount previous
          mounted.Slots.Remove key.Id |> ignore
        | None, None -> ()

        match mounted.Slots.TryGetValue key.Id with
        | true, slot -> next.Props |> Props.add (PropKey.viewKeyOfSubElement key, slot.View)
        | false, _ -> ()

  and private reconcileChildren (application: IApplication) (mounted: MountedNode) (next: ISimpleViewSpec) =
    let parent =
      match mounted.Element with
      | TerminalElement.ViewTE value -> value
      | _ -> invalidOp "Only View nodes can own normal child views."

    if mounted.Children.Count <> 0 || next.Props.Children.Count <> 0 then
      let oldChildren = mounted.Children.ToArray()
      let oldSpecs = oldChildren |> Array.map _.Spec
      let nextSpecs = next.Props.Children |> Seq.toArray
      let oldKeyed = validateKeys parent.Name oldSpecs
      let nextKeyed = validateKeys parent.Name nextSpecs
      let matches = Array.create nextSpecs.Length -1
      let reused = Array.create oldChildren.Length false

      if oldKeyed && nextKeyed then
        let byKey = Dictionary<string, int>(StringComparer.Ordinal)

        for index = 0 to oldSpecs.Length - 1 do
          byKey[(ViewSpec.key oldSpecs[index]).Value] <- index

        for index = 0 to nextSpecs.Length - 1 do
          match byKey.TryGetValue((ViewSpec.key nextSpecs[index]).Value) with
          | true, oldIndex when sameKind oldSpecs[oldIndex] nextSpecs[index] ->
            matches[index] <- oldIndex
            reused[oldIndex] <- true
          | _ -> ()
      elif not oldKeyed && not nextKeyed then
        let sharedLength = min oldChildren.Length nextSpecs.Length

        for index = 0 to sharedLength - 1 do
          if sameKind oldSpecs[index] nextSpecs[index] then
            matches[index] <- index
            reused[index] <- true

      // Bind the entire sibling set first so relative positions resolve to retained views.
      for index = 0 to nextSpecs.Length - 1 do
        if matches[index] >= 0 then
          ViewSpec.bind nextSpecs[index] oldChildren[matches[index]].Element

      let result: MountedNode option array = Array.create nextSpecs.Length None

      for index = 0 to nextSpecs.Length - 1 do
        let oldIndex = matches[index]

        if oldIndex >= 0 then
          let child = oldChildren[oldIndex]
          reconcileNode application child nextSpecs[index]
          result[index] <- Some child

      for index = 0 to oldChildren.Length - 1 do
        if not reused[index] then
          unmount oldChildren[index]

      for index = 0 to nextSpecs.Length - 1 do
        if result[index].IsNone then
          result[index] <- Some(mount application (Origin.Child(parent, index)) nextSpecs[index])

      let nextChildren = ResizeArray<MountedNode>(result |> Array.map _.Value)
      reorderChildren parent oldChildren nextChildren
      updateChildOrigins parent nextChildren

      mounted.Children.Clear()
      mounted.Children.AddRange nextChildren

  let rec private validateSpecTree spec =
    match spec with
    | ViewSpec.ComponentViewSpec _ -> ()
    | ViewSpec.SimpleViewSpec simple ->
      let children = simple.Props.Children |> Seq.toArray
      validateKeys (simple.GetType().Name) children |> ignore

      for child in children do
        validateSpecTree child

      // A property slot is an independent identity domain, but its subtree still needs
      // the same sibling-key validation before any hierarchy mutation is committed.
      for entry in Props.toEntries simple.Props do
        if entry.Key.IsSubViewSpec then
          entry.Value |> specFromView |> validateSpecTree

  type Renderer private (application: IApplication, ownsApplication: bool) =
    let syncRoot = obj ()
    let mutable current: MountedNode option = None

    new(application: IApplication) = new Renderer(application, false)
    new() = new Renderer(Application.Create(), true)

    member _.Current = current

    member _.Render(rootSpec: ISimpleViewSpec, origin: Origin) =
      lock syncRoot (fun () ->
        let nextSpec = ViewSpec.SimpleViewSpec rootSpec
        validateSpecTree nextSpec

        let next =
          match current with
          | None -> mount application origin nextSpec
          | Some mounted when sameIdentity mounted.Spec nextSpec ->
            reconcileNode application mounted nextSpec
            mounted
          | Some mounted ->
            invalidOp
              $"The root view identity cannot change while the Terminal.Gui session is running (current: {mounted.Spec.GetType().Name}, next: {nextSpec.GetType().Name})."

        current <- Some next

        match next.Element with
        | TerminalElement.ViewTE viewTe -> viewTe
        | TerminalElement.ElmishComponentTE _ ->
          invalidOp "An Elmish component cannot be used as the root Terminal.Gui runnable.")

    member _.Dispose() =
      lock syncRoot (fun () ->
        current |> Option.iter unmount
        current <- None

        if ownsApplication then
          application.Dispose())

    interface IDisposable with
      member this.Dispose() = this.Dispose()
