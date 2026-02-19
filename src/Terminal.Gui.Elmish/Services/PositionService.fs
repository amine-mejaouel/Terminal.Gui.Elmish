namespace Terminal.Gui.Elmish

open System
open System.Collections.Generic
open Terminal.Gui.ViewBase

/// Key for indexing the position relationships between two terminal elements.
/// The order of the two elements doesn't matter, so (A, B) and (B, A) should be considered the same key.
type internal TePairKey(first: ITerminalElementBase, second: ITerminalElementBase) =
  member this.First = first
  member this.Second = second
  member this.GetOtherTe(te: ITerminalElementBase) =
    if te = first then second
    elif te = second then first
    else failwith "Terminal element not part of this pair key."

  override this.GetHashCode() =
    // The order of the two elements doesn't matter for the key,
    // so we can use XOR to combine their hash codes.
    first.GetHashCode() ^^^ second.GetHashCode()

  override this.Equals(obj) =
    match obj with
    | :? TePairKey as other ->
        (this.First = other.First && this.Second = other.Second) ||
        (this.First = other.Second && this.Second = other.First)
    | _ -> false

type internal PositionService() =

  static member val Current = PositionService() with get

  /// Delegate to execute the cleanup of handlers for a pair of terminal elements.
  member val Cleanups = Dictionary<TePairKey, List<unit -> unit>>()

  /// CleanupKeys by terminal element.
  member val TerminalElementPairs = Dictionary<ITerminalElementBase, HashSet<TePairKey>>()

  member private this.UpdateIndex(tePairKey) =

    let updateIndex k =
      match this.TerminalElementPairs.TryGetValue(k) with
      | true, set ->
        set.Add tePairKey |> ignore
      | false, _ ->
        let set = HashSet<TePairKey>()
        set.Add tePairKey |> ignore
        this.TerminalElementPairs[k] <- set

    updateIndex tePairKey.First
    updateIndex tePairKey.Second

  member private this.RegisterCleanup(key: ITerminalElementBase * ITerminalElementBase, cleanup: unit -> unit) =

    let key = TePairKey(fst key, snd key)

    match this.Cleanups.TryGetValue(key) with
    | true, handlers ->
      handlers.Add cleanup
    | false, _ ->
      this.Cleanups.[key] <- List<_>([ cleanup ])

    this.UpdateIndex(key)

  member private this.ExecuteCleanups(targetTe: IViewTE) =
    match this.TerminalElementPairs.TryGetValue(targetTe) with
    | true, tePairKeys ->
      for tePairKey in tePairKeys do
        match this.Cleanups.TryGetValue(tePairKey) with
        | true, handlers ->
          for handler in handlers do
            handler()
          this.Cleanups.Remove tePairKey |> ignore

          // Also remove the tePairKey from the TerminalElementPairs for the other terminal element in the pair
          let otherTe = tePairKey.GetOtherTe targetTe
          match this.TerminalElementPairs.TryGetValue otherTe with
          | true, set ->
            set.Remove tePairKey |> ignore
            if set.Count = 0 then
              this.TerminalElementPairs.Remove otherTe |> ignore
          | _ -> ()

        | false, _ -> ()
      this.TerminalElementPairs.Remove targetTe |> ignore
    | false, _ -> ()


  member private this.ApplyRelativePos(cur: ITerminalElementBase, rel: ITerminalElementBase, apply: View -> View -> unit) =
    let handler = EventHandler<DrawEventArgs>(fun _ _ -> apply cur.View rel.View)
    rel.View.DrawComplete.AddHandler handler
    this.RegisterCleanup((cur, rel), fun () -> rel.View.DrawComplete.RemoveHandler handler)

  member this.ApplyPos(curElementData: IViewTE, targetPos: TPos, apply: View -> Pos -> unit) =

    let onViewSetOnElementData (relativeTerminalElement: ITerminalElementBase) applyPos =
      let differApplyRelativePos (relativeTerminalElement: ITerminalElementBase) (applyPos: View -> View -> unit) =
        let handler = Handler<View>(fun _ _ -> this.ApplyRelativePos (curElementData, relativeTerminalElement, applyPos))
        relativeTerminalElement.OnViewSet.AddHandler handler
        this.RegisterCleanup((curElementData, relativeTerminalElement), fun () -> relativeTerminalElement.
                                                                                    OnViewSet.RemoveHandler handler)

      if (relativeTerminalElement.View = null) then
        differApplyRelativePos relativeTerminalElement applyPos
      else
        this.ApplyRelativePos (curElementData, relativeTerminalElement, applyPos)

    match targetPos with
    | TPos.X te ->
      onViewSetOnElementData (te :?> ITerminalElementBase) (fun thisView otherView -> apply thisView (Pos.X(otherView)))
    | TPos.Y te ->
      onViewSetOnElementData (te :?> ITerminalElementBase) (fun thisView otherView -> apply thisView (Pos.Y(otherView)))
    | TPos.Top te ->
      onViewSetOnElementData (te :?> ITerminalElementBase) (fun thisView otherView -> apply thisView (Pos.Top(otherView)))
    | TPos.Bottom te ->
      onViewSetOnElementData (te :?> ITerminalElementBase) (fun thisView otherView -> apply thisView (Pos.Bottom(otherView)))
    | TPos.Left te ->
      onViewSetOnElementData (te :?> ITerminalElementBase) (fun thisView otherView -> apply thisView (Pos.Left(otherView)))
    | TPos.Right te ->
      onViewSetOnElementData (te :?> ITerminalElementBase) (fun thisView otherView -> apply thisView (Pos.Right(otherView)))
    | TPos.Func (func, te) ->
      onViewSetOnElementData (te :?> ITerminalElementBase) (fun thisView otherView -> apply thisView (Pos.Func(func, otherView)))
    | TPos.Absolute position -> apply curElementData.View (Pos.Absolute(position))
    | TPos.AnchorEnd offset -> apply curElementData.View (Pos.AnchorEnd(offset |> Option.defaultValue 0))
    | TPos.Center -> apply curElementData.View (Pos.Center())
    | TPos.Percent percent -> apply curElementData.View (Pos.Percent(percent))
    | TPos.Align (alignment, modes, groupId) -> apply curElementData.View (Pos.Align(alignment, modes, groupId |> Option.defaultValue 0))

  member this.SignalReuse(terminalElement: IViewTE) =
    this.ExecuteCleanups terminalElement

  member this.SignalDispose(terminalElement: IViewTE) =
    this.ExecuteCleanups terminalElement
