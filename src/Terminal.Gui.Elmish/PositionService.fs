namespace Terminal.Gui.Elmish

open System
open System.Collections.Generic
open Terminal.Gui.ViewBase

type internal PositionService() =

  static member val Current = PositionService() with get

  // Key: (curElementData, relElementData) / Value: list of remove handlers
  member val RemoveHandlerRepository = Dictionary<IElementData * IElementData, List<unit -> unit>>()

  // Key: IElementData / Value: set of (curElementData, relElementData) to be used to lookup in RemoveHandlerRepository
  member val IndexedRemoveHandler = Dictionary<IElementData, HashSet<IElementData*IElementData>>()

  member private this.UpdateIndex(v0, v1) =
    let indexRepo = this.IndexedRemoveHandler

    let update k =
      match indexRepo.TryGetValue(k) with
      | true, set ->
        set.Add (v0, v1) |> ignore
      | false, _ ->
        let set = HashSet<IElementData*IElementData>()
        set.Add (v0, v1) |> ignore
        indexRepo[k] <- set

    update v0
    update v1

  member private this.SetRemoveHandler(key: IElementData * IElementData, removeHandler: unit -> unit) =
    let repo = this.RemoveHandlerRepository

    match repo.TryGetValue(key) with
    | true, handlers ->
      handlers.Add removeHandler
    | false, _ ->
      repo.[key] <- List<_>([removeHandler])

    this.UpdateIndex(key)

  member private this.RemoveHandlers(key: IElementData) =
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

  member private this.ApplyRelativePos(cur: IElementData, rel: IElementData, apply: View -> View -> unit) =
    let handler = EventHandler<DrawEventArgs>(fun _ _ -> apply cur.view rel.view)
    rel.view.DrawComplete.AddHandler handler
    this.SetRemoveHandler((cur, rel), fun () -> rel.view.DrawComplete.RemoveHandler handler)

  member this.ApplyPos(curElementData: IElementData, targetPos: TPos, apply: View -> Pos -> unit) =

    let onViewSetOnElementData (relativeElementData: IElementData) applyPos =
      let differApplyRelativePos (relativeElementData: IElementData) (applyPos: View -> View -> unit) =
        let handler = Handler<View>(fun _ _ -> this.ApplyRelativePos (curElementData, relativeElementData, applyPos))
        relativeElementData.ViewSet.AddHandler handler
        this.SetRemoveHandler((curElementData, relativeElementData), fun () -> relativeElementData.ViewSet.RemoveHandler handler)

      if (relativeElementData.view = null) then
        differApplyRelativePos relativeElementData applyPos
      else
        this.ApplyRelativePos (curElementData, relativeElementData, applyPos)

    let elementData (terminalElement: ITerminalElement) = (terminalElement :?> IInternalTerminalElement).elementData

    match targetPos with
    | TPos.X te ->
      onViewSetOnElementData (elementData te) (fun thisView otherView -> apply thisView (Pos.X(otherView)))
    | TPos.Y te ->
      onViewSetOnElementData (elementData te) (fun thisView otherView -> apply thisView (Pos.Y(otherView)))
    | TPos.Top te ->
      onViewSetOnElementData (elementData te) (fun thisView otherView -> apply thisView (Pos.Top(otherView)))
    | TPos.Bottom te ->
      onViewSetOnElementData (elementData te) (fun thisView otherView -> apply thisView (Pos.Bottom(otherView)))
    | TPos.Left te ->
      onViewSetOnElementData (elementData te) (fun thisView otherView -> apply thisView (Pos.Left(otherView)))
    | TPos.Right te ->
      onViewSetOnElementData (elementData te) (fun thisView otherView -> apply thisView (Pos.Right(otherView)))
    | TPos.Func (func, te) ->
      onViewSetOnElementData (elementData te) (fun thisView otherView -> apply thisView (Pos.Func(func, otherView)))
    | TPos.Absolute position -> apply curElementData.view (Pos.Absolute(position))
    | TPos.AnchorEnd offset -> apply curElementData.view (Pos.AnchorEnd(offset |> Option.defaultValue 0))
    | TPos.Center -> apply curElementData.view (Pos.Center())
    | TPos.Percent percent -> apply curElementData.view (Pos.Percent(percent))
    | TPos.Align (alignment, modes, groupId) -> apply curElementData.view (Pos.Align(alignment, modes, groupId |> Option.defaultValue 0))

  member this.SignalReuse(elementData: IElementData) =
    this.RemoveHandlers elementData

  member this.SignalDispose(elementData: IElementData) =
    this.RemoveHandlers elementData
