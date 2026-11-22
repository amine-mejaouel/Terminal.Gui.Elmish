namespace Terminal.Gui.Elmish

open System
open System.Collections.Generic
open Terminal.Gui.ViewBase

/// Key for indexing the position relationship between two DomNodes.
/// The order of the two nodes doesn't matter, so (A, B) and (B, A) are the same key.
type internal DomPairKey(first: DomNode, second: DomNode) =
  member this.First = first
  member this.Second = second

  member this.GetOther(node: DomNode) =
    if obj.ReferenceEquals(node, first) then second
    elif obj.ReferenceEquals(node, second) then first
    else failwith "DomNode not part of this pair key."

  override this.GetHashCode() =
    first.GetHashCode() ^^^ second.GetHashCode()

  override this.Equals(other: obj) =
    match other with
    | :? DomPairKey as key ->
      (obj.ReferenceEquals(first, key.First) && obj.ReferenceEquals(second, key.Second))
      || (obj.ReferenceEquals(first, key.Second) && obj.ReferenceEquals(second, key.First))
    | _ -> false

/// Phase 5 (decision A2): DomNode-keyed equivalent of `PositionService`.
///
/// Positions are applied in a deferred pass (`ApplyTree`) once the whole DomNode tree and
/// its per-render TE->DomNode mapping exist. Because a DomNode creates its View eagerly at
/// construction, every node already has a View by then, so the `OnViewSet` deferral the old
/// service needed is unnecessary. Only the pair registry here persists across renders.
type internal DomPositionService() =

  static member val Current = DomPositionService() with get

  /// Cleanup handlers keyed by the DomNode pair they were registered for.
  member val Cleanups = Dictionary<DomPairKey, List<unit -> unit>>()

  /// Reverse index: DomNode -> the pairs it participates in.
  member val NodePairs = Dictionary<DomNode, HashSet<DomPairKey>>()

  member private this.UpdateIndex(key: DomPairKey) =
    let add (node: DomNode) =
      match this.NodePairs.TryGetValue node with
      | true, set -> set.Add key |> ignore
      | false, _ ->
        let set = HashSet<DomPairKey>()
        set.Add key |> ignore
        this.NodePairs[node] <- set

    add key.First
    add key.Second

  member private this.RegisterCleanup(pair: DomNode * DomNode, cleanup: unit -> unit) =
    let key = DomPairKey(fst pair, snd pair)

    match this.Cleanups.TryGetValue key with
    | true, handlers -> handlers.Add cleanup
    | false, _ -> this.Cleanups[key] <- List<_>([ cleanup ])

    this.UpdateIndex key

  /// Runs and removes every cleanup registered for `target`, unlinking the pairs.
  member this.ExecuteCleanups(target: DomNode) =
    match this.NodePairs.TryGetValue target with
    | true, keys ->
      for key in keys do
        match this.Cleanups.TryGetValue key with
        | true, handlers ->
          for handler in handlers do
            handler ()

          this.Cleanups.Remove key |> ignore
          let other = key.GetOther target

          match this.NodePairs.TryGetValue other with
          | true, set ->
            set.Remove key |> ignore

            if set.Count = 0 then
              this.NodePairs.Remove other |> ignore
          | _ -> ()
        | false, _ -> ()

      this.NodePairs.Remove target |> ignore
    | false, _ -> ()

  member private this.ApplyRelativePos(cur: DomNode, rel: DomNode, apply: View -> View -> unit) =
    let handler = EventHandler<_>(fun _ _ -> apply cur.View rel.View)
    rel.View.DrawComplete.AddHandler handler
    this.RegisterCleanup((cur, rel), fun () -> rel.View.DrawComplete.RemoveHandler handler)

  /// Applies a single delayed (relative) position on `axis`, resolving the TPos's
  /// `ITerminalElement` reference to a DomNode via `resolve`.
  member this.ApplyDelayedPos
    (cur: DomNode, axis: PosAxis, targetPos: TPos, resolve: ITerminalElement -> DomNode option)
    =
    let applyPos (view: View) pos =
      match axis with
      | PosAxis.X -> view.X <- pos
      | PosAxis.Y -> view.Y <- pos

    let resetPos (view: View) =
      match axis with
      | PosAxis.X -> view.X <- view.Frame.X
      | PosAxis.Y -> view.Y <- view.Frame.Y

    let relative (rel: DomNode) (apply: View -> View -> unit) =
      this.ApplyRelativePos(cur, rel, apply)
      this.RegisterCleanup((cur, rel), fun () -> resetPos cur.View)

    match targetPos with
    | TPos.X te ->
      resolve te
      |> Option.iter (fun rel -> relative rel (fun v o -> applyPos v (Pos.X o)))
    | TPos.Y te ->
      resolve te
      |> Option.iter (fun rel -> relative rel (fun v o -> applyPos v (Pos.Y o)))
    | TPos.Top te ->
      resolve te
      |> Option.iter (fun rel -> relative rel (fun v o -> applyPos v (Pos.Top o)))
    | TPos.Bottom te ->
      resolve te
      |> Option.iter (fun rel -> relative rel (fun v o -> applyPos v (Pos.Bottom o)))
    | TPos.Left te ->
      resolve te
      |> Option.iter (fun rel -> relative rel (fun v o -> applyPos v (Pos.Left o)))
    | TPos.Right te ->
      resolve te
      |> Option.iter (fun rel -> relative rel (fun v o -> applyPos v (Pos.Right o)))

    | TPos.Func(func, te) ->
      resolve te
      |> Option.iter (fun rel -> relative rel (fun v o -> applyPos v (Pos.Func(func, o))))
    | TPos.Absolute position -> applyPos cur.View (Pos.Absolute position)
    | TPos.AnchorEnd offset -> applyPos cur.View (Pos.AnchorEnd(offset |> Option.defaultValue 0))
    | TPos.Center -> applyPos cur.View (Pos.Center())
    | TPos.Percent percent -> applyPos cur.View (Pos.Percent percent)
    | TPos.Align(alignment, modes, groupId) ->
      applyPos cur.View (Pos.Align(alignment, modes, groupId |> Option.defaultValue 0))

  /// Applies the absolute / delayed X and Y positions stored on `node`'s props.
  member this.ApplyPos(node: DomNode, resolve: ITerminalElement -> DomNode option) =
    let props = node.AppliedProps

    match props.X, props.XDelayed with
    | Some _, Some _ -> failwith "Cannot set both X and XDelayedPos on the same view."
    | Some xPos, None -> node.View.X <- xPos
    | None, Some delayedX -> this.ApplyDelayedPos(node, PosAxis.X, delayedX, resolve)
    | None, None -> node.View.X <- 0

    match props.Y, props.YDelayed with
    | Some _, Some _ -> failwith "Cannot set both Y and YDelayedPos on the same view."
    | Some yPos, None -> node.View.Y <- yPos
    | None, Some delayedY -> this.ApplyDelayedPos(node, PosAxis.Y, delayedY, resolve)
    | None, None -> node.View.Y <- 0

  /// Deferred second pass: re-applies positions for the whole DomNode tree
  /// (children and sub-elements), resolving TPos references via `resolve`.
  member this.ApplyTree(root: DomNode, resolve: ITerminalElement -> DomNode option) =
    let rec walk (node: DomNode) =
      this.ExecuteCleanups node
      this.ApplyPos(node, resolve)

      for child in node.Children do
        match child with
        | ViewDomChild childDom -> walk childDom
        | ElmishComponentDomChild _ -> ()

      for sub in node.SubElementDomNodes do
        walk sub

    walk root
