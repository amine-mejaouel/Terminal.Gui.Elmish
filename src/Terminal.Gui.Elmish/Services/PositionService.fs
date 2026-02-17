namespace Terminal.Gui.Elmish

open System
open System.Collections.Generic
open Terminal.Gui.ViewBase

type internal PositionService() =

  static member val Current = PositionService() with get

  // Key: (curElementData, relElementData) / Value: list of remove handlers
  member val RemoveHandlerRepository = Dictionary<ITerminalElementBase * ITerminalElementBase, List<unit -> unit>>()

  // Key: IElementData / Value: set of (curElementData, relElementData) to be used to lookup in RemoveHandlerRepository
  member val IndexedRemoveHandler = Dictionary<ITerminalElementBase, HashSet<ITerminalElementBase *
                                                                             ITerminalElementBase>>()

  member private this.UpdateIndex(v0, v1) =
    let indexRepo = this.IndexedRemoveHandler

    let update k =
      match indexRepo.TryGetValue(k) with
      | true, set ->
        set.Add (v0, v1) |> ignore
      | false, _ ->
        let set = HashSet<ITerminalElementBase * ITerminalElementBase>()
        set.Add (v0, v1) |> ignore
        indexRepo[k] <- set

    update v0
    update v1

  member private this.SetRemoveHandler(key: ITerminalElementBase * ITerminalElementBase, removeHandler: unit -> unit) =
    let repo = this.RemoveHandlerRepository

    match repo.TryGetValue(key) with
    | true, handlers ->
      handlers.Add removeHandler
    | false, _ ->
      repo.[key] <- List<_>([removeHandler])

    this.UpdateIndex(key)

  member private this.RemoveHandlers(key: IViewTE) =
    let indexRepo = this.IndexedRemoveHandler
    let repo = this.RemoveHandlerRepository

    match indexRepo.TryGetValue(key) with
    | true, set ->
      for key in set do
        match repo.TryGetValue(key) with
        | true, handlers ->
          for removeHandler in handlers do
            removeHandler()
          repo.Remove(key) |> ignore
        | false, _ -> ()
      indexRepo.Remove(key) |> ignore
    | false, _ -> ()

  member private this.ApplyRelativePos(cur: ITerminalElementBase, rel: ITerminalElementBase, apply: View -> View -> unit) =
    let handler = EventHandler<DrawEventArgs>(fun _ _ -> apply cur.View rel.View)
    rel.View.DrawComplete.AddHandler handler
    this.SetRemoveHandler((cur, rel), fun () -> rel.View.DrawComplete.RemoveHandler handler)

  member this.ApplyPos(curElementData: IViewTE, targetPos: TPos, apply: View -> Pos -> unit) =

    let onViewSetOnElementData (relativeTerminalElement: ITerminalElementBase) applyPos =
      let differApplyRelativePos (relativeTerminalElement: ITerminalElementBase) (applyPos: View -> View -> unit) =
        let handler = Handler<View>(fun _ _ -> this.ApplyRelativePos (curElementData, relativeTerminalElement, applyPos))
        relativeTerminalElement.OnViewSet.AddHandler handler
        this.SetRemoveHandler((curElementData, relativeTerminalElement), fun () -> relativeTerminalElement.
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
    this.RemoveHandlers terminalElement

  member this.SignalDispose(terminalElement: IViewTE) =
    this.RemoveHandlers terminalElement
