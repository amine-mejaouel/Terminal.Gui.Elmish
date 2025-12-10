module internal Terminal.Gui.Elmish.Elements

open System
open System.Collections.Generic
open System.Collections.ObjectModel
open System.Collections.Specialized
open Terminal.Gui.Elmish
open Terminal.Gui.ViewBase
open Terminal.Gui.Views

type TreeNode = {
  TerminalElement: IInternalTerminalElement
  Parent: View option
}

[<CustomEquality; NoComparison>]
type SubElementPropKey<'a> =
  | SingleElementKey of ISingleElementPropKey<'a>
  | MultiElementKey of IMultiElementPropKey<'a>

  static member createSingleElementKey<'a>(key: string) : SubElementPropKey<'a> =
    SingleElementKey(PropKey.Create.singleElement<'a> key)

  static member createMultiElementKey<'a>(key: string) : SubElementPropKey<'a> =
    MultiElementKey(PropKey.Create.multiElement<'a> key)

  static member from(singleElementKey: ISingleElementPropKey<'a>) : SubElementPropKey<'b> =
    SubElementPropKey<'b>.createSingleElementKey (singleElementKey :> IPropKey<'a>).key

  static member from(multiElementKey: IMultiElementPropKey<'a>) : SubElementPropKey<'b> =
    SubElementPropKey<'b>.createMultiElementKey (multiElementKey :> IPropKey<'a>).key

  member this.key =
    match this with
    | SingleElementKey key -> key.key
    | MultiElementKey key -> key.key

  override this.GetHashCode() = this.key.GetHashCode()

  override this.Equals(obj) =
    match obj with
    | :? IPropKey as x -> this.key.Equals(x.key)
    | _ -> false

  member this.viewKey =
    match this with
    | SingleElementKey key -> key.viewKey
    | MultiElementKey key -> key.viewKey

  interface IPropKey<'a> with
    member this.key = this.key
    member this.isViewKey = false

    member this.isSingleElementKey =
      match this with
      | SingleElementKey _ -> true
      | MultiElementKey _ -> false

module internal ViewElement =

  let canReuseView (view: View) (props: Props) (removedProps: Props) =
    let isPosCompatible (a: Pos) (b: Pos) =
      let nameA = a.GetType().Name
      let nameB = b.GetType().Name

      nameA = nameB
      || (nameA = "PosAbsolute" && nameB = "PosAbsolute")
      || (nameA <> "PosAbsolute" && nameB <> "PosAbsolute")

    let isDimCompatible (a: Dim) (b: Dim) =
      let nameA = a.GetType().Name
      let nameB = b.GetType().Name

      nameA = nameB
      || (nameA = "DimAbsolute" && nameB = "DimAbsolute")
      || (nameA <> "DimAbsolute" && nameB <> "DimAbsolute")


    let positionX =
      props
      |> Props.tryFind PKey.view.x
      |> Option.map (fun v -> isPosCompatible view.X v)
      |> Option.defaultValue true

    let positionY =
      props
      |> Props.tryFind PKey.view.y
      |> Option.map (fun v -> isPosCompatible view.Y v)
      |> Option.defaultValue true

    let width =
      props
      |> Props.tryFind PKey.view.width
      |> Option.map (fun v -> isDimCompatible view.Width v)
      |> Option.defaultValue true

    let height =
      props
      |> Props.tryFind PKey.view.height
      |> Option.map (fun v -> isDimCompatible view.Height v)
      |> Option.defaultValue true

    // in case width or height is removed!
    let widthNotRemoved =
      removedProps
      |> Props.exists PKey.view.width
      |> not

    let heightNotRemoved =
      removedProps
      |> Props.exists PKey.view.height
      |> not

    // TODO: is this still relevant ?
    // TODO: should check if every check is still needed to reuse the view
    [
      positionX
      positionY
      width
      height
      widthNotRemoved
      heightNotRemoved
    ]
    |> List.forall id

type HandlerOwner =
  | LibraryItself of key: string
  | LibraryUser

type PropsEventRegistry() =

  let eventHandlerRepository = Dictionary<IPropKey, Dictionary<HandlerOwner, Delegate>>()
  let removeHandlerRepository: Dictionary<IPropKey, Dictionary<HandlerOwner,unit -> unit>> = Dictionary<IPropKey, Dictionary<HandlerOwner,unit -> unit>>()

  member private this.tryGetHandler<'THandler when 'THandler :> Delegate> (pkey: IPropKey, owner: HandlerOwner) =
    match eventHandlerRepository.TryGetValue(pkey) with
    | true, existingHandlers ->
      match existingHandlers.TryGetValue(owner) with
      | true, existingHandler ->
        Some (existingHandler :?> 'THandler)
      | false, _ ->
        None
    | false, _ ->
      None

  member private this.tryGetRemoveHandler (pkey: IPropKey, owner: HandlerOwner) =
    match removeHandlerRepository.TryGetValue(pkey) with
    | true, existingRemovers ->
      match existingRemovers.TryGetValue(owner) with
      | true, existingRemover ->
        Some existingRemover
      | false, _ ->
        None
    | false, _ ->
      None

  member private this.registerHandlerRemoval<'THandler when 'THandler :> Delegate> (pkey: IPropKey, owner, handler: 'THandler, removeHandler: 'THandler -> unit) =
    if not (removeHandlerRepository.ContainsKey(pkey)) then
      removeHandlerRepository[pkey] <- Dictionary<HandlerOwner, unit -> unit>()

    removeHandlerRepository[pkey][owner] <-
      fun () ->
        removeHandler handler
        eventHandlerRepository[pkey].Remove(owner) |> ignore

  member this.removeHandler (pkey: IPropKey, owner) =
    match this.tryGetRemoveHandler (pkey, owner) with
    | Some removeHandler ->
      removeHandler ()
      removeHandlerRepository[pkey].Remove(owner) |> ignore
    | None ->
      ()

  member private this.setHandler<'THandler when 'THandler :> Delegate> (pkey: IPropKey, owner: HandlerOwner, handler: 'THandler, removeFromEvent: 'THandler -> unit, addToEvent: 'THandler -> unit) =
    match this.tryGetHandler<'THandler> (pkey, owner) with
    | Some existingHandler ->
      removeFromEvent existingHandler
    | None ->
      if not (eventHandlerRepository.ContainsKey(pkey)) then
        eventHandlerRepository[pkey] <- Dictionary<HandlerOwner, Delegate>()

    eventHandlerRepository[pkey][owner] <- handler
    addToEvent handler

  member this.setEventHandler (pkey: IPropKey<'TEventArgs -> unit>, owner: HandlerOwner, event: IEvent<EventHandler<'TEventArgs>,'TEventArgs>, action: 'TEventArgs -> unit) =
    let handler: EventHandler<'TEventArgs> = EventHandler<'TEventArgs>(fun sender args -> action args)
    this.setHandler(pkey, owner, handler, event.RemoveHandler, event.AddHandler)
    this.registerHandlerRemoval(pkey, owner, handler, event.RemoveHandler)

  member this.setEventHandler (pkey: IPropKey<'TEventArgs -> unit>, owner: HandlerOwner, event: IEvent<EventHandler,EventArgs>, action: unit -> unit) =
    let handler: EventHandler = EventHandler(fun sender args -> action ())
    this.setHandler(pkey, owner, handler, event.RemoveHandler, event.AddHandler)
    this.registerHandlerRemoval(pkey, owner, handler, event.RemoveHandler)

  member this.setEventHandler (pkey: IPropKey<'TEventArgs -> unit>, owner: HandlerOwner, event: IEvent<EventHandler,EventArgs>, action: EventArgs -> unit) =
    let handler: EventHandler = EventHandler(fun sender args -> action args)
    this.setHandler(pkey, owner, handler, event.RemoveHandler, event.AddHandler)
    this.registerHandlerRemoval(pkey, owner, handler, event.RemoveHandler)

  member this.setEventHandler (pkey: IPropKey<NotifyCollectionChangedEventArgs -> unit>, owner: HandlerOwner, event: IEvent<NotifyCollectionChangedEventHandler,NotifyCollectionChangedEventArgs>, action: NotifyCollectionChangedEventArgs -> unit) =
    let handler: NotifyCollectionChangedEventHandler = NotifyCollectionChangedEventHandler(fun sender args -> action args)
    this.setHandler(pkey, owner, handler, event.RemoveHandler, event.AddHandler)
    this.registerHandlerRemoval(pkey, owner, handler, event.RemoveHandler)

type TerminalElementEventRegistry() =

  let removeHandlerRepository = List<unit -> unit>()

  member this.addEventHandler(event: IEvent<'TEventArgs>, action: 'TEventArgs -> unit) =
    let handler = Handler<'TEventArgs>(fun _ args -> action args)
    event.AddHandler(handler)

    removeHandlerRepository.Add(fun () -> event.RemoveHandler(handler))

  member this.removeAllEventHandlers() =
    removeHandlerRepository
    |> Seq.iter (fun removeHandler -> removeHandler ())
    removeHandlerRepository.Clear()

[<AbstractClass>]
type TerminalElement(props: Props) =

  let rec traverseTree (nodes: TreeNode list) (traverse: TreeNode -> unit) =

    match nodes with
    | [] -> ()
    | cur :: remainingNodes ->
      let curNode = {
        TerminalElement = cur.TerminalElement
        Parent = cur.Parent
      }

      traverse curNode

      let childNodes =
        curNode.TerminalElement.children
        |> Seq.map (fun e -> {
          TerminalElement = e
          Parent = Some curNode.TerminalElement.view
        })
        |> List.ofSeq

      traverseTree (childNodes @ remainingNodes) traverse

  let onDrawCompleteEvent = Event<View>()

  // TODO: relative position should be cleared from the props in between elmish loop iteration
  // TODO: cause, they are capturing view but view are being reused and they might not be reused for the same targeted element.
  let applyPos (eventRegistry: TerminalElementEventRegistry) (apply: Pos -> unit) targetPos =
    let addTerminalEventHandler (terminal: ITerminalElement) handler =
      eventRegistry.addEventHandler ((terminal :?> IInternalTerminalElement).onDrawComplete, handler)

    match targetPos with
    | TPos.X te ->
      addTerminalEventHandler te (fun view -> apply (Pos.X(view)))
    | TPos.Y te ->
      addTerminalEventHandler te (fun view -> apply (Pos.Y(view)))
    | TPos.Top te ->
      addTerminalEventHandler te (fun view -> apply (Pos.Top(view)))
    | TPos.Bottom te ->
      addTerminalEventHandler te (fun view -> apply (Pos.Bottom(view)))
    | TPos.Left te ->
      addTerminalEventHandler te (fun view -> apply (Pos.Left(view)))
    | TPos.Right te ->
      addTerminalEventHandler te (fun view -> apply (Pos.Right(view)))
    | TPos.Func (func, te) ->
      addTerminalEventHandler te (fun view -> apply (Pos.Func(func, view)))
    | TPos.Absolute position -> apply (Pos.Absolute(position))
    | TPos.AnchorEnd offset -> apply (Pos.AnchorEnd(offset |> Option.defaultValue 0))
    | TPos.Center -> apply (Pos.Center())
    | TPos.Percent percent -> apply (Pos.Percent(percent))
    | TPos.Align (alignment, modes, groupId) -> apply (Pos.Align(alignment, modes, groupId |> Option.defaultValue 0))

  member val propsEventRegistry = PropsEventRegistry() with get, set
  member val terminalElementEventRegistry = TerminalElementEventRegistry() with get, set

  member this.props = props
  member val parent: View option = None with get, set

  member val private _viewAlreadySetOnce: bool = false with get, set
  member val _view: View = null with get, set
  member this.getView() = this._view

  member this.setView(view: View) =
    if this._viewAlreadySetOnce then
      failwith $"{this.name}: View has already been set once and cannot be set again."
    this._viewAlreadySetOnce <- true
    this._view <- view
    this.propsEventRegistry.setEventHandler(PKey.view.drawComplete, LibraryItself "TerminalElement.View", view.DrawComplete, fun _ -> onDrawCompleteEvent.Trigger view)

  member this.detachView () =
    if this._view = null && this._viewAlreadySetOnce then
      Error $"{this.name}: View is already detached."
    elif this._view = null then
      Error $"{this.name}: View has not been set yet."
    else
      this.propsEventRegistry.removeHandler(PKey.view.drawComplete, LibraryItself "TerminalElement.View")
      this.terminalElementEventRegistry.removeAllEventHandlers()
      let view = this._view
      this._view <- null
      Ok view

  member _.children: List<IInternalTerminalElement> =
    props
    |> Props.tryFindWithDefault PKey.view.children (List<_>())

  abstract SubElements_PropKeys: SubElementPropKey<IInternalTerminalElement> list
  default _.SubElements_PropKeys = []

  abstract newView: unit -> View

  abstract setAsChildOfParentView: bool
  default _.setAsChildOfParentView = true

  member val isElmishComponent: bool = false with get, set

  member this.initialize() =
#if DEBUG
    Diagnostics.Trace.WriteLine $"{this.name} created!"
#endif

    let newView = this.newView ()

    this.initializeSubElements newView
    |> Seq.iter props.addNonTyped

    this.setView newView
    this.setProps props

  abstract canReuseView: prevView: View -> prevProps: Props -> bool
  abstract reuse: prevView: View -> prevProps: Props -> unit

  default this.canReuseView prevView prevProps =
    let c =
      this.compare prevProps

    let removedProps =
      c.removedProps
      |> Props.filter (not << _.Key.isViewKey)

    let canUpdateView =
      ViewElement.canReuseView prevView c.changedProps removedProps

    let canUpdateElement = true

    canUpdateView && canUpdateElement

  abstract name: string

  member this.initializeTree(parent: View option) : unit =
    let traverse (node: TreeNode) =
      node.TerminalElement.parent <- node.Parent

      node.TerminalElement.initialize ()

      // Here, the "children" view are added to their parent
      if node.TerminalElement.setAsChildOfParentView then
        node.Parent
        |> Option.iter (fun p -> p.Add node.TerminalElement.view |> ignore)

    traverseTree
      [
        {
          TerminalElement = this
          Parent = parent
        }
      ]
      traverse

  /// For each '*.element' prop, initialize the Tree of the element and then return the sub element: (proPKey * View)
  member this.initializeSubElements parent : (IPropKey * obj) seq =
    seq {
      for x in this.SubElements_PropKeys do
        match props |> Props.tryFindByRawKey<obj> x with

        | None -> ()

        | Some value ->
          match value with
          | :? TerminalElement as subElement ->
            subElement.initializeTree (Some parent)

            let viewKey = x.viewKey

            yield viewKey, subElement._view
          | :? List<IInternalTerminalElement> as elements ->
            elements
            |> Seq.iter (fun e -> e.initializeTree (Some parent))

            let viewKey = x.viewKey

            let views =
              elements |> Seq.map _.view |> Seq.toList

            yield viewKey, views
          | _ -> failwith "Out of range subElement type"
    }

  abstract setProps: props: Props -> unit

  default this.setProps(props: Props) =
    // Properties
    props
    |> Props.tryFind PKey.view.arrangement
    |> Option.iter (fun v -> this._view.Arrangement <- v)

    props
    |> Props.tryFind PKey.view.borderStyle
    |> Option.iter (fun v -> this._view.BorderStyle <- v)

    props
    |> Props.tryFind PKey.view.canFocus
    |> Option.iter (fun v -> this._view.CanFocus <- v)

    props
    |> Props.tryFind PKey.view.contentSizeTracksViewport
    |> Option.iter (fun v -> this._view.ContentSizeTracksViewport <- v)

    props
    |> Props.tryFind PKey.view.cursorVisibility
    |> Option.iter (fun v -> this._view.CursorVisibility <- v)

    props
    |> Props.tryFind PKey.view.data
    |> Option.iter (fun v -> this._view.Data <- v)

    props
    |> Props.tryFind PKey.view.enabled
    |> Option.iter (fun v -> this._view.Enabled <- v)

    props
    |> Props.tryFind PKey.view.frame
    |> Option.iter (fun v -> this._view.Frame <- v)

    props
    |> Props.tryFind PKey.view.hasFocus
    |> Option.iter (fun v -> this._view.HasFocus <- v)

    props
    |> Props.tryFind PKey.view.height
    |> Option.iter (fun v -> this._view.Height <- v)

    props
    |> Props.tryFind PKey.view.highlightStates
    |> Option.iter (fun v -> this._view.HighlightStates <- v)

    props
    |> Props.tryFind PKey.view.hotKey
    |> Option.iter (fun v -> this._view.HotKey <- v)

    props
    |> Props.tryFind PKey.view.hotKeySpecifier
    |> Option.iter (fun v -> this._view.HotKeySpecifier <- v)

    props
    |> Props.tryFind PKey.view.id
    |> Option.iter (fun v -> this._view.Id <- v)

    props
    |> Props.tryFind PKey.view.isInitialized
    |> Option.iter (fun v -> this._view.IsInitialized <- v)

    props
    |> Props.tryFind PKey.view.mouseHeldDown
    |> Option.iter (fun v -> this._view.MouseHeldDown <- v)

    props
    |> Props.tryFind PKey.view.needsDraw
    |> Option.iter (fun v -> this._view.NeedsDraw <- v)

    props
    |> Props.tryFind PKey.view.preserveTrailingSpaces
    |> Option.iter (fun v -> this._view.PreserveTrailingSpaces <- v)

    props
    |> Props.tryFind PKey.view.schemeName
    |> Option.iter (fun v -> this._view.SchemeName <- v)

    props
    |> Props.tryFind PKey.view.shadowStyle
    |> Option.iter (fun v -> this._view.ShadowStyle <- v)

    props
    |> Props.tryFind PKey.view.superViewRendersLineCanvas
    |> Option.iter (fun v -> this._view.SuperViewRendersLineCanvas <- v)

    props
    |> Props.tryFind PKey.view.tabStop
    |> Option.iter (fun v -> this._view.TabStop <- v |> Option.toNullable)

    props
    |> Props.tryFind PKey.view.text
    |> Option.iter (fun v -> this._view.Text <- v)

    props
    |> Props.tryFind PKey.view.textAlignment
    |> Option.iter (fun v -> this._view.TextAlignment <- v)

    props
    |> Props.tryFind PKey.view.textDirection
    |> Option.iter (fun v -> this._view.TextDirection <- v)

    props
    |> Props.tryFind PKey.view.title
    |> Option.iter (fun v -> this._view.Title <- v)

    props
    |> Props.tryFind PKey.view.validatePosDim
    |> Option.iter (fun v -> this._view.ValidatePosDim <- v)

    props
    |> Props.tryFind PKey.view.verticalTextAlignment
    |> Option.iter (fun v -> this._view.VerticalTextAlignment <- v)

    props
    |> Props.tryFind PKey.view.viewport
    |> Option.iter (fun v -> this._view.Viewport <- v)

    props
    |> Props.tryFind PKey.view.viewportSettings
    |> Option.iter (fun v -> this._view.ViewportSettings <- v)

    props
    |> Props.tryFind PKey.view.visible
    |> Option.iter (fun v -> this._view.Visible <- v)

    props
    |> Props.tryFind PKey.view.wantContinuousButtonPressed
    |> Option.iter (fun v -> this._view.WantContinuousButtonPressed <- v)

    props
    |> Props.tryFind PKey.view.wantMousePositionReports
    |> Option.iter (fun v -> this._view.WantMousePositionReports <- v)

    props
    |> Props.tryFind PKey.view.width
    |> Option.iter (fun v -> this._view.Width <- v)

    props
    |> Props.tryFind PKey.view.x
    |> Option.iter (fun v -> this._view.X <- v)

    props
    |> Props.tryFind PKey.view.y
    |> Option.iter (fun v -> this._view.Y <- v)
    // Events
    props
    |> Props.tryFind PKey.view.accepting
    |> Option.iter (fun v -> this.propsEventRegistry.setEventHandler(PKey.view.accepting, HandlerOwner.LibraryUser, this._view.Accepting, v))

    props
    |> Props.tryFind PKey.view.advancingFocus
    |> Option.iter (fun v -> this.propsEventRegistry.setEventHandler(PKey.view.advancingFocus, LibraryUser, this._view.AdvancingFocus, v))

    props
    |> Props.tryFind PKey.view.borderStyleChanged
    |> Option.iter (fun v -> this.propsEventRegistry.setEventHandler(PKey.view.borderStyleChanged, LibraryUser, this._view.BorderStyleChanged, v))

    props
    |> Props.tryFind PKey.view.canFocusChanged
    |> Option.iter (fun v -> this.propsEventRegistry.setEventHandler(PKey.view.canFocusChanged, LibraryUser, this._view.CanFocusChanged, v))

    props
    |> Props.tryFind PKey.view.clearedViewport
    |> Option.iter (fun v -> this.propsEventRegistry.setEventHandler(PKey.view.clearedViewport, LibraryUser, this._view.ClearedViewport, v))

    props
    |> Props.tryFind PKey.view.clearingViewport
    |> Option.iter (fun v -> this.propsEventRegistry.setEventHandler(PKey.view.clearingViewport, LibraryUser, this._view.ClearingViewport, v))

    props
    |> Props.tryFind PKey.view.commandNotBound
    |> Option.iter (fun v -> this.propsEventRegistry.setEventHandler(PKey.view.commandNotBound, LibraryUser, this._view.CommandNotBound, v))

    props
    |> Props.tryFind PKey.view.contentSizeChanged
    |> Option.iter (fun v -> this.propsEventRegistry.setEventHandler(PKey.view.contentSizeChanged, LibraryUser, this._view.ContentSizeChanged, v))

    props
    |> Props.tryFind PKey.view.disposing
    |> Option.iter (fun v -> this.propsEventRegistry.setEventHandler(PKey.view.disposing, LibraryUser, this._view.Disposing, v))

    props
    |> Props.tryFind PKey.view.drawComplete
    |> Option.iter (fun v -> this.propsEventRegistry.setEventHandler(PKey.view.drawComplete, LibraryUser, this._view.DrawComplete, v))

    props
    |> Props.tryFind PKey.view.drawingContent
    |> Option.iter (fun v -> this.propsEventRegistry.setEventHandler(PKey.view.drawingContent, LibraryUser, this._view.DrawingContent, v))

    props
    |> Props.tryFind PKey.view.drawingSubViews
    |> Option.iter (fun v -> this.propsEventRegistry.setEventHandler(PKey.view.drawingSubViews, LibraryUser, this._view.DrawingSubViews, v))

    props
    |> Props.tryFind PKey.view.drawingText
    |> Option.iter (fun v -> this.propsEventRegistry.setEventHandler(PKey.view.drawingText, LibraryUser, this._view.DrawingText, v))

    props
    |> Props.tryFind PKey.view.enabledChanged
    |> Option.iter (fun v -> this.propsEventRegistry.setEventHandler(PKey.view.enabledChanged, LibraryUser, this._view.EnabledChanged, v))

    props
    |> Props.tryFind PKey.view.focusedChanged
    |> Option.iter (fun v -> this.propsEventRegistry.setEventHandler(PKey.view.focusedChanged, LibraryUser, this._view.FocusedChanged, v))

    props
    |> Props.tryFind PKey.view.frameChanged
    |> Option.iter (fun v -> this.propsEventRegistry.setEventHandler(PKey.view.frameChanged, LibraryUser, this._view.FrameChanged, v))

    props
    |> Props.tryFind PKey.view.gettingAttributeForRole
    |> Option.iter (fun v -> this.propsEventRegistry.setEventHandler(PKey.view.gettingAttributeForRole, LibraryUser, this._view.GettingAttributeForRole, v))

    props
    |> Props.tryFind PKey.view.gettingScheme
    |> Option.iter (fun v -> this.propsEventRegistry.setEventHandler(PKey.view.gettingScheme, LibraryUser, this._view.GettingScheme, v))

    props
    |> Props.tryFind PKey.view.handlingHotKey
    |> Option.iter (fun v -> this.propsEventRegistry.setEventHandler(PKey.view.handlingHotKey, LibraryUser, this._view.HandlingHotKey, v))

    props
    |> Props.tryFind PKey.view.hasFocusChanged
    |> Option.iter (fun v -> this.propsEventRegistry.setEventHandler(PKey.view.hasFocusChanged, LibraryUser, this._view.HasFocusChanged, v))

    props
    |> Props.tryFind PKey.view.hasFocusChanging
    |> Option.iter (fun v -> this.propsEventRegistry.setEventHandler(PKey.view.hasFocusChanging, LibraryUser, this._view.HasFocusChanging, v))

    props
    |> Props.tryFind PKey.view.hotKeyChanged
    |> Option.iter (fun v -> this.propsEventRegistry.setEventHandler(PKey.view.hotKeyChanged, LibraryUser, this._view.HotKeyChanged, v))

    props
    |> Props.tryFind PKey.view.initialized
    |> Option.iter (fun v -> this.propsEventRegistry.setEventHandler(PKey.view.initialized, LibraryUser, this._view.Initialized, v))

    props
    |> Props.tryFind PKey.view.keyDown
    |> Option.iter (fun v -> this.propsEventRegistry.setEventHandler(PKey.view.keyDown, LibraryUser, this._view.KeyDown, v))

    props
    |> Props.tryFind PKey.view.keyDownNotHandled
    |> Option.iter (fun v -> this.propsEventRegistry.setEventHandler(PKey.view.keyDownNotHandled, LibraryUser, this._view.KeyDownNotHandled, v))

    props
    |> Props.tryFind PKey.view.keyUp
    |> Option.iter (fun v -> this.propsEventRegistry.setEventHandler(PKey.view.keyUp, LibraryUser, this._view.KeyUp, v))

    props
    |> Props.tryFind PKey.view.mouseClick
    |> Option.iter (fun v -> this.propsEventRegistry.setEventHandler(PKey.view.mouseClick, LibraryUser, this._view.MouseClick, v))

    props
    |> Props.tryFind PKey.view.mouseEnter
    |> Option.iter (fun v -> this.propsEventRegistry.setEventHandler(PKey.view.mouseEnter, LibraryUser, this._view.MouseEnter, v))

    props
    |> Props.tryFind PKey.view.mouseEvent
    |> Option.iter (fun v -> this.propsEventRegistry.setEventHandler(PKey.view.mouseEvent, LibraryUser, this._view.MouseEvent, v))

    props
    |> Props.tryFind PKey.view.mouseLeave
    |> Option.iter (fun v -> this.propsEventRegistry.setEventHandler(PKey.view.mouseLeave, LibraryUser, this._view.MouseLeave, v))

    props
    |> Props.tryFind PKey.view.mouseStateChanged
    |> Option.iter (fun v -> this.propsEventRegistry.setEventHandler(PKey.view.mouseStateChanged, LibraryUser, this._view.MouseStateChanged, v))

    props
    |> Props.tryFind PKey.view.mouseWheel
    |> Option.iter (fun v -> this.propsEventRegistry.setEventHandler(PKey.view.mouseWheel, LibraryUser, this._view.MouseWheel, v))

    props
    |> Props.tryFind PKey.view.removed
    |> Option.iter (fun v -> this.propsEventRegistry.setEventHandler(PKey.view.removed, LibraryUser, this._view.Removed, v))

    props
    |> Props.tryFind PKey.view.schemeChanged
    |> Option.iter (fun v -> this.propsEventRegistry.setEventHandler(PKey.view.schemeChanged, LibraryUser, this._view.SchemeChanged, v))

    props
    |> Props.tryFind PKey.view.schemeChanging
    |> Option.iter (fun v -> this.propsEventRegistry.setEventHandler(PKey.view.schemeChanging, LibraryUser, this._view.SchemeChanging, v))

    props
    |> Props.tryFind PKey.view.schemeNameChanged
    |> Option.iter (fun v -> this.propsEventRegistry.setEventHandler(PKey.view.schemeNameChanged, LibraryUser, this._view.SchemeNameChanged, v))

    props
    |> Props.tryFind PKey.view.schemeNameChanging
    |> Option.iter (fun v -> this.propsEventRegistry.setEventHandler(PKey.view.schemeNameChanging, LibraryUser, this._view.SchemeNameChanging, v))

    props
    |> Props.tryFind PKey.view.selecting
    |> Option.iter (fun v -> this.propsEventRegistry.setEventHandler(PKey.view.selecting, LibraryUser, this._view.Selecting, v))

    props
    |> Props.tryFind PKey.view.subViewAdded
    |> Option.iter (fun v -> this.propsEventRegistry.setEventHandler(PKey.view.subViewAdded, LibraryUser, this._view.SubViewAdded, v))

    props
    |> Props.tryFind PKey.view.subViewLayout
    |> Option.iter (fun v -> this.propsEventRegistry.setEventHandler(PKey.view.subViewLayout, LibraryUser, this._view.SubViewLayout, v))

    props
    |> Props.tryFind PKey.view.subViewRemoved
    |> Option.iter (fun v -> this.propsEventRegistry.setEventHandler(PKey.view.subViewRemoved, LibraryUser, this._view.SubViewRemoved, v))

    props
    |> Props.tryFind PKey.view.subViewsLaidOut
    |> Option.iter (fun v -> this.propsEventRegistry.setEventHandler(PKey.view.subViewsLaidOut, LibraryUser, this._view.SubViewsLaidOut, v))

    props
    |> Props.tryFind PKey.view.superViewChanged
    |> Option.iter (fun v -> this.propsEventRegistry.setEventHandler(PKey.view.superViewChanged, LibraryUser, this._view.SuperViewChanged, v))

    props
    |> Props.tryFind PKey.view.textChanged
    |> Option.iter (fun v -> this.propsEventRegistry.setEventHandler(PKey.view.textChanged, LibraryUser, this._view.TextChanged, v))

    props
    |> Props.tryFind PKey.view.titleChanged
    |> Option.iter (fun v -> this.propsEventRegistry.setEventHandler(PKey.view.titleChanged, LibraryUser, this._view.TitleChanged, v))

    props
    |> Props.tryFind PKey.view.titleChanging
    |> Option.iter (fun v -> this.propsEventRegistry.setEventHandler(PKey.view.titleChanging, LibraryUser, this._view.TitleChanging, v))

    props
    |> Props.tryFind PKey.view.viewportChanged
    |> Option.iter (fun v -> this.propsEventRegistry.setEventHandler(PKey.view.viewportChanged, LibraryUser, this._view.ViewportChanged, v))

    props
    |> Props.tryFind PKey.view.visibleChanged
    |> Option.iter (fun v -> this.propsEventRegistry.setEventHandler(PKey.view.visibleChanged, LibraryUser, this._view.VisibleChanged, v))

    props
    |> Props.tryFind PKey.view.visibleChanging
    |> Option.iter (fun v -> this.propsEventRegistry.setEventHandler(PKey.view.visibleChanging, LibraryUser, this._view.VisibleChanging, v))

    // Custom Props
    props
    |> Props.tryFind PKey.view.x_delayedPos
    |> Option.iter (applyPos this.terminalElementEventRegistry (fun pos -> this._view.X <- pos))

    props
    |> Props.tryFind PKey.view.y_delayedPos
    |> Option.iter (applyPos this.terminalElementEventRegistry (fun pos -> this._view.Y <- pos))

  // TODO: Is the view needed as param ? is the props needed as param ?
  abstract removeProps: props: Props -> unit

  default this.removeProps(props: Props) =
    // Properties
    props
    |> Props.tryFind PKey.view.arrangement
    |> Option.iter (fun _ -> this._view.Arrangement <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.view.borderStyle
    |> Option.iter (fun _ -> this._view.BorderStyle <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.view.canFocus
    |> Option.iter (fun _ -> this._view.CanFocus <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.view.contentSizeTracksViewport
    |> Option.iter (fun _ -> this._view.ContentSizeTracksViewport <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.view.cursorVisibility
    |> Option.iter (fun _ -> this._view.CursorVisibility <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.view.data
    |> Option.iter (fun _ -> this._view.Data <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.view.enabled
    |> Option.iter (fun _ -> this._view.Enabled <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.view.frame
    |> Option.iter (fun _ -> this._view.Frame <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.view.hasFocus
    |> Option.iter (fun _ -> this._view.HasFocus <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.view.height
    |> Option.iter (fun _ -> this._view.Height <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.view.highlightStates
    |> Option.iter (fun _ -> this._view.HighlightStates <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.view.hotKey
    |> Option.iter (fun _ -> this._view.HotKey <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.view.hotKeySpecifier
    |> Option.iter (fun _ -> this._view.HotKeySpecifier <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.view.id
    |> Option.iter (fun _ -> this._view.Id <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.view.isInitialized
    |> Option.iter (fun _ -> this._view.IsInitialized <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.view.mouseHeldDown
    |> Option.iter (fun _ -> this._view.MouseHeldDown <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.view.needsDraw
    |> Option.iter (fun _ -> this._view.NeedsDraw <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.view.preserveTrailingSpaces
    |> Option.iter (fun _ -> this._view.PreserveTrailingSpaces <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.view.schemeName
    |> Option.iter (fun _ -> this._view.SchemeName <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.view.shadowStyle
    |> Option.iter (fun _ -> this._view.ShadowStyle <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.view.superViewRendersLineCanvas
    |> Option.iter (fun _ -> this._view.SuperViewRendersLineCanvas <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.view.tabStop
    |> Option.iter (fun _ -> this._view.TabStop <- Unchecked.defaultof<_> |> Option.toNullable)

    props
    |> Props.tryFind PKey.view.text
    |> Option.iter (fun _ -> this._view.Text <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.view.textAlignment
    |> Option.iter (fun _ -> this._view.TextAlignment <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.view.textDirection
    |> Option.iter (fun _ -> this._view.TextDirection <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.view.title
    |> Option.iter (fun _ -> this._view.Title <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.view.validatePosDim
    |> Option.iter (fun _ -> this._view.ValidatePosDim <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.view.verticalTextAlignment
    |> Option.iter (fun _ -> this._view.VerticalTextAlignment <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.view.viewport
    |> Option.iter (fun _ -> this._view.Viewport <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.view.viewportSettings
    |> Option.iter (fun _ -> this._view.ViewportSettings <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.view.visible
    |> Option.iter (fun _ -> this._view.Visible <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.view.wantContinuousButtonPressed
    |> Option.iter (fun _ -> this._view.WantContinuousButtonPressed <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.view.wantMousePositionReports
    |> Option.iter (fun _ -> this._view.WantMousePositionReports <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.view.width
    |> Option.iter (fun _ -> this._view.Width <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.view.x
    |> Option.iter (fun _ -> this._view.X <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.view.y
    |> Option.iter (fun _ -> this._view.Y <- Unchecked.defaultof<_>)
    // Events
    props
    |> Props.tryFind PKey.view.accepting
    |> Option.iter (fun _ -> this.propsEventRegistry.removeHandler (PKey.view.accepting, LibraryUser))

    props
    |> Props.tryFind PKey.view.advancingFocus
    |> Option.iter (fun _ -> this.propsEventRegistry.removeHandler (PKey.view.advancingFocus, LibraryUser))

    props
    |> Props.tryFind PKey.view.borderStyleChanged
    |> Option.iter (fun _ -> this.propsEventRegistry.removeHandler (PKey.view.borderStyleChanged, LibraryUser))

    props
    |> Props.tryFind PKey.view.canFocusChanged
    |> Option.iter (fun _ -> this.propsEventRegistry.removeHandler (PKey.view.canFocusChanged, LibraryUser))

    props
    |> Props.tryFind PKey.view.clearedViewport
    |> Option.iter (fun _ -> this.propsEventRegistry.removeHandler (PKey.view.clearedViewport, LibraryUser))

    props
    |> Props.tryFind PKey.view.clearingViewport
    |> Option.iter (fun _ -> this.propsEventRegistry.removeHandler (PKey.view.clearingViewport, LibraryUser))

    props
    |> Props.tryFind PKey.view.commandNotBound
    |> Option.iter (fun _ -> this.propsEventRegistry.removeHandler (PKey.view.commandNotBound, LibraryUser))

    props
    |> Props.tryFind PKey.view.contentSizeChanged
    |> Option.iter (fun _ -> this.propsEventRegistry.removeHandler (PKey.view.contentSizeChanged, LibraryUser))

    props
    |> Props.tryFind PKey.view.disposing
    |> Option.iter (fun _ -> this.propsEventRegistry.removeHandler (PKey.view.disposing, LibraryUser))

    props
    |> Props.tryFind PKey.view.drawComplete
    |> Option.iter (fun _ -> this.propsEventRegistry.removeHandler (PKey.view.drawComplete, LibraryUser))

    props
    |> Props.tryFind PKey.view.drawingContent
    |> Option.iter (fun _ -> this.propsEventRegistry.removeHandler (PKey.view.drawingContent, LibraryUser))

    props
    |> Props.tryFind PKey.view.drawingSubViews
    |> Option.iter (fun _ -> this.propsEventRegistry.removeHandler (PKey.view.drawingSubViews, LibraryUser))

    props
    |> Props.tryFind PKey.view.drawingText
    |> Option.iter (fun _ -> this.propsEventRegistry.removeHandler (PKey.view.drawingText, LibraryUser))

    props
    |> Props.tryFind PKey.view.enabledChanged
    |> Option.iter (fun _ -> this.propsEventRegistry.removeHandler (PKey.view.enabledChanged, LibraryUser))

    props
    |> Props.tryFind PKey.view.focusedChanged
    |> Option.iter (fun _ -> this.propsEventRegistry.removeHandler (PKey.view.focusedChanged, LibraryUser))

    props
    |> Props.tryFind PKey.view.frameChanged
    |> Option.iter (fun _ -> this.propsEventRegistry.removeHandler (PKey.view.frameChanged, LibraryUser))

    props
    |> Props.tryFind PKey.view.gettingAttributeForRole
    |> Option.iter (fun _ -> this.propsEventRegistry.removeHandler (PKey.view.gettingAttributeForRole, LibraryUser))

    props
    |> Props.tryFind PKey.view.gettingScheme
    |> Option.iter (fun _ -> this.propsEventRegistry.removeHandler (PKey.view.gettingScheme, LibraryUser))

    props
    |> Props.tryFind PKey.view.handlingHotKey
    |> Option.iter (fun _ -> this.propsEventRegistry.removeHandler (PKey.view.handlingHotKey, LibraryUser))

    props
    |> Props.tryFind PKey.view.hasFocusChanged
    |> Option.iter (fun _ -> this.propsEventRegistry.removeHandler (PKey.view.hasFocusChanged, LibraryUser))

    props
    |> Props.tryFind PKey.view.hasFocusChanging
    |> Option.iter (fun _ -> this.propsEventRegistry.removeHandler (PKey.view.hasFocusChanging, LibraryUser))

    props
    |> Props.tryFind PKey.view.hotKeyChanged
    |> Option.iter (fun _ -> this.propsEventRegistry.removeHandler (PKey.view.hotKeyChanged, LibraryUser))

    props
    |> Props.tryFind PKey.view.initialized
    |> Option.iter (fun _ -> this.propsEventRegistry.removeHandler (PKey.view.initialized, LibraryUser))

    props
    |> Props.tryFind PKey.view.keyDown
    |> Option.iter (fun _ -> this.propsEventRegistry.removeHandler (PKey.view.keyDown, LibraryUser))

    props
    |> Props.tryFind PKey.view.keyDownNotHandled
    |> Option.iter (fun _ -> this.propsEventRegistry.removeHandler (PKey.view.keyDownNotHandled, LibraryUser))

    props
    |> Props.tryFind PKey.view.keyUp
    |> Option.iter (fun _ -> this.propsEventRegistry.removeHandler (PKey.view.keyUp, LibraryUser))

    props
    |> Props.tryFind PKey.view.mouseClick
    |> Option.iter (fun _ -> this.propsEventRegistry.removeHandler (PKey.view.mouseClick, LibraryUser))

    props
    |> Props.tryFind PKey.view.mouseEnter
    |> Option.iter (fun _ -> this.propsEventRegistry.removeHandler (PKey.view.mouseEnter, LibraryUser))

    props
    |> Props.tryFind PKey.view.mouseEvent
    |> Option.iter (fun _ -> this.propsEventRegistry.removeHandler (PKey.view.mouseEvent, LibraryUser))

    props
    |> Props.tryFind PKey.view.mouseLeave
    |> Option.iter (fun _ -> this.propsEventRegistry.removeHandler (PKey.view.mouseLeave, LibraryUser))

    props
    |> Props.tryFind PKey.view.mouseStateChanged
    |> Option.iter (fun _ -> this.propsEventRegistry.removeHandler (PKey.view.mouseStateChanged, LibraryUser))

    props
    |> Props.tryFind PKey.view.mouseWheel
    |> Option.iter (fun _ -> this.propsEventRegistry.removeHandler (PKey.view.mouseWheel, LibraryUser))

    props
    |> Props.tryFind PKey.view.removed
    |> Option.iter (fun _ -> this.propsEventRegistry.removeHandler (PKey.view.removed, LibraryUser))

    props
    |> Props.tryFind PKey.view.schemeChanged
    |> Option.iter (fun _ -> this.propsEventRegistry.removeHandler (PKey.view.schemeChanged, LibraryUser))

    props
    |> Props.tryFind PKey.view.schemeChanging
    |> Option.iter (fun _ -> this.propsEventRegistry.removeHandler (PKey.view.schemeChanging, LibraryUser))

    props
    |> Props.tryFind PKey.view.schemeNameChanged
    |> Option.iter (fun _ -> this.propsEventRegistry.removeHandler (PKey.view.schemeNameChanged, LibraryUser))

    props
    |> Props.tryFind PKey.view.schemeNameChanging
    |> Option.iter (fun _ -> this.propsEventRegistry.removeHandler (PKey.view.schemeNameChanging, LibraryUser))

    props
    |> Props.tryFind PKey.view.selecting
    |> Option.iter (fun _ -> this.propsEventRegistry.removeHandler (PKey.view.selecting, LibraryUser))

    props
    |> Props.tryFind PKey.view.subViewAdded
    |> Option.iter (fun _ -> this.propsEventRegistry.removeHandler (PKey.view.subViewAdded, LibraryUser))

    props
    |> Props.tryFind PKey.view.subViewLayout
    |> Option.iter (fun _ -> this.propsEventRegistry.removeHandler (PKey.view.subViewLayout, LibraryUser))

    props
    |> Props.tryFind PKey.view.subViewRemoved
    |> Option.iter (fun _ -> this.propsEventRegistry.removeHandler (PKey.view.subViewRemoved, LibraryUser))

    props
    |> Props.tryFind PKey.view.subViewsLaidOut
    |> Option.iter (fun _ -> this.propsEventRegistry.removeHandler (PKey.view.subViewsLaidOut, LibraryUser))

    props
    |> Props.tryFind PKey.view.superViewChanged
    |> Option.iter (fun _ -> this.propsEventRegistry.removeHandler (PKey.view.superViewChanged, LibraryUser))

    props
    |> Props.tryFind PKey.view.textChanged
    |> Option.iter (fun _ -> this.propsEventRegistry.removeHandler (PKey.view.textChanged, LibraryUser))

    props
    |> Props.tryFind PKey.view.titleChanged
    |> Option.iter (fun _ -> this.propsEventRegistry.removeHandler (PKey.view.titleChanged, LibraryUser))

    props
    |> Props.tryFind PKey.view.titleChanging
    |> Option.iter (fun _ -> this.propsEventRegistry.removeHandler (PKey.view.titleChanging, LibraryUser))

    props
    |> Props.tryFind PKey.view.viewportChanged
    |> Option.iter (fun _ -> this.propsEventRegistry.removeHandler (PKey.view.viewportChanged, LibraryUser))

    props
    |> Props.tryFind PKey.view.visibleChanged
    |> Option.iter (fun _ -> this.propsEventRegistry.removeHandler (PKey.view.visibleChanged, LibraryUser))

    props
    |> Props.tryFind PKey.view.visibleChanging
    |> Option.iter (fun _ -> this.propsEventRegistry.removeHandler (PKey.view.visibleChanging, LibraryUser))

  /// Reuses:
  /// - Previous `View`, while updating its properties to match the current TerminalElement properties.
  /// - But also other Views that are sub elements of the previous `ITerminalElement` and made available in the `prevProps`.
  override this.reuse prevView prevProps =
    let c = this.compare prevProps

    // 0 - foreach unchanged _element property, we identify the _view to reinject to `this` TerminalElement
    let view_PropKeys_ToReinject =
      c.unchangedProps
      |> Props.filterSingleElementKeys
      |> Seq.map _.viewKey
      |> Seq.toArray

    // 1 - then we get these Views missing in `this` TerminalElement.
    let view_Props_ToReinject, removedProps =
      c.removedProps
      |> Props.partition (fun kv -> view_PropKeys_ToReinject |> Array.contains kv.Key)

    // 2 - And we add them.
    view_Props_ToReinject
    |> Props.iter (fun kv -> this.props.addNonTyped (kv.Key, kv.Value))

    this.setView prevView
    this.removeProps removedProps
    this.setProps c.changedProps


  member this.equivalentTo(other: TerminalElement) =
    let mutable isEquivalent = true

    let mutable enumerator =
      this.props.dict.GetEnumerator()

    while isEquivalent && enumerator.MoveNext() do
      let kv = enumerator.Current

      if kv.Key.key = "children" then // TODO: for now children comparison is not yet implemented
        ()
      elif kv.Key.isViewKey then
        ()
      elif kv.Key.isSingleElementKey then
        let curElement =
          kv.Value :?> TerminalElement

        let otherElement =
          other.props
          |> Props.tryFindByRawKey kv.Key
          |> Option.map (fun (x: obj) -> x :?> TerminalElement)

        match curElement, otherElement with
        | curValue, Some otherValue when (curValue.equivalentTo otherValue) -> ()
        | _, _ -> isEquivalent <- false
      else
        let curElement = kv.Value

        let otherElement =
          other.props |> Props.tryFindByRawKey kv.Key

        isEquivalent <- curElement = otherElement

    isEquivalent

  member this.compare
    (prevProps: Props)
    : {|
        changedProps: Props
        unchangedProps: Props
        removedProps: Props
      |}
    =

    let remainingOldProps, removedProps =
      prevProps
      |> Props.partition (fun kv -> this.props |> Props.rawKeyExists kv.Key)

    let unchangedProps, changedProps =
      this.props
      |> Props.partition (fun kv ->
        match remainingOldProps |> Props.tryFindByRawKey kv.Key with
        | _ when kv.Key.key = "children" -> // Here we always consider the 'children' unchanged
          true
        | Some(v: obj) when kv.Key.isSingleElementKey ->
          let curElement =
            kv.Value :?> TerminalElement

          let oldElement = v :?> TerminalElement
          curElement.equivalentTo oldElement
        // TODO: comparison is not good here, it can fail for many C# types
        // TODO: Properties values should be comparable
        // TODO: should also be able to compare _element props
        | Some v' when kv.Value = v' -> true
        | _ -> false
      )

    {|
      changedProps = changedProps
      unchangedProps = unchangedProps
      removedProps = removedProps
    |}

  [<CLIEvent>]
  member this.onDrawComplete = onDrawCompleteEvent.Publish

  member this.Dispose() =
    ()
    // TODO: need to be precise about this, User event will be done on the removeProps, Internal library should be removed precisely here
    // this.eventRegistry.removeAllEventHandlers()

  interface IInternalTerminalElement with
    member this.initialize() = this.initialize()
    member this.initializeTree(parent) = this.initializeTree parent
    member this.canReuseView prevView prevProps = this.canReuseView prevView prevProps
    member this.reuse prevView prevProps = this.reuse prevView prevProps
    member this.view = this._view
    member this.props = this.props
    member this.name = this.name
    member this.children = this.children

    member this.setAsChildOfParentView =
      this.setAsChildOfParentView

    member this.onDrawComplete = this.onDrawComplete

    member this.parent = this.parent
    member this.parent with set value = this.parent <- value

    member this.Dispose() = this.Dispose()

    member this.detachView () = this.detachView()

// OrientationInterface - used by elements that implement Terminal.Gui.ViewBase.IOrientation
type OrientationInterface =
  static member removeProps (element: TerminalElement) (view: IOrientation) (props: Props) =
    // Properties
    props
    |> Props.tryFind PKey.orientationInterface.orientation
    |> Option.iter (fun _ -> view.Orientation <- Unchecked.defaultof<_>)

    // Events
    props
    |> Props.tryFind PKey.orientationInterface.orientationChanged
    |> Option.iter (fun _ -> element.propsEventRegistry.removeHandler (PKey.orientationInterface.orientationChanged, LibraryUser))

    props
    |> Props.tryFind PKey.orientationInterface.orientationChanging
    |> Option.iter (fun _ -> element.propsEventRegistry.removeHandler (PKey.orientationInterface.orientationChanging, LibraryUser))

  static member setProps (element: TerminalElement) (view: IOrientation) (props: Props) =
    // Properties
    props
    |> Props.tryFind PKey.orientationInterface.orientation
    |> Option.iter (fun v -> view.Orientation <- v)

    // Events
    props
    |> Props.tryFind PKey.orientationInterface.orientationChanged
    |> Option.iter (fun v -> element.propsEventRegistry.setEventHandler(PKey.orientationInterface.orientationChanged, LibraryUser, view.OrientationChanged, v))

    props
    |> Props.tryFind PKey.orientationInterface.orientationChanging
    |> Option.iter (fun v -> element.propsEventRegistry.setEventHandler(PKey.orientationInterface.orientationChanging, LibraryUser, view.OrientationChanging, v))

// Adornment
type AdornmentElement(props: Props) =
  inherit TerminalElement(props)

  override this.removeProps(props: Props) =
    base.removeProps props
    let view = (this :> IInternalTerminalElement).view :?> Adornment
    // Properties
    props
    |> Props.tryFind PKey.adornment.diagnostics
    |> Option.iter (fun _ -> view.Diagnostics <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.adornment.superViewRendersLineCanvas
    |> Option.iter (fun _ -> view.SuperViewRendersLineCanvas <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.adornment.thickness
    |> Option.iter (fun _ -> view.Thickness <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.adornment.viewport
    |> Option.iter (fun _ -> view.Viewport <- Unchecked.defaultof<_>)
    // Events
    props
    |> Props.tryFind PKey.adornment.thicknessChanged
    |> Option.iter (fun _ -> this.propsEventRegistry.removeHandler (PKey.adornment.thicknessChanged, LibraryUser))

  override _.name = $"Adornment"

  override this.setProps(props: Props) =
    base.setProps props

    let view = (this :> IInternalTerminalElement).view :?> Adornment

    // Properties
    props
    |> Props.tryFind PKey.adornment.diagnostics
    |> Option.iter (fun v -> view.Diagnostics <- v)

    props
    |> Props.tryFind PKey.adornment.superViewRendersLineCanvas
    |> Option.iter (fun v -> view.SuperViewRendersLineCanvas <- v)

    props
    |> Props.tryFind PKey.adornment.thickness
    |> Option.iter (fun v -> view.Thickness <- v)

    props
    |> Props.tryFind PKey.adornment.viewport
    |> Option.iter (fun v -> view.Viewport <- v)
    // Events
    props
    |> Props.tryFind PKey.adornment.thicknessChanged
    |> Option.iter (fun v -> this.propsEventRegistry.setEventHandler(PKey.adornment.thicknessChanged, LibraryUser, view.ThicknessChanged, v))

  override this.newView() = new Adornment()


// Bar
type BarElement(props: Props) =
  inherit TerminalElement(props)

  override this.removeProps(props: Props) =
    base.removeProps props
    let view = (this :> IInternalTerminalElement).view :?> Bar
    // Interfaces
    OrientationInterface.removeProps this view props

    // Properties
    props
    |> Props.tryFind PKey.bar.alignmentModes
    |> Option.iter (fun _ -> view.AlignmentModes <- Unchecked.defaultof<_>)

  override _.name = $"Bar"

  override this.setProps(props: Props) =
    base.setProps props

    let view = (this :> IInternalTerminalElement).view :?> Bar

    // Interfaces
    OrientationInterface.setProps this view props

    // Properties
    props
    |> Props.tryFind PKey.bar.alignmentModes
    |> Option.iter (fun v -> view.AlignmentModes <- v)


  override this.newView() = new Bar()

// Border
type BorderElement(props: Props) =
  inherit TerminalElement(props)

  override this.removeProps(props: Props) =
    base.removeProps props
    let view = (this :> IInternalTerminalElement).view :?> Border
    // Properties
    props
    |> Props.tryFind PKey.border.lineStyle
    |> Option.iter (fun _ -> view.LineStyle <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.border.settings
    |> Option.iter (fun _ -> view.Settings <- Unchecked.defaultof<_>)

  override _.name = $"Border"

  override this.setProps(props: Props) =
    base.setProps props

    let view = (this :> IInternalTerminalElement).view :?> Border

    // Properties
    props
    |> Props.tryFind PKey.border.lineStyle
    |> Option.iter (fun v -> view.LineStyle <- v)

    props
    |> Props.tryFind PKey.border.settings
    |> Option.iter (fun v -> view.Settings <- v)


  override this.newView() = new Border()

// Button
type ButtonElement(props: Props) =
  inherit TerminalElement(props)

  override this.removeProps(props: Props) =
    base.removeProps props
    let view = (this :> IInternalTerminalElement).view :?> Button
    // Properties
    props
    |> Props.tryFind PKey.button.hotKeySpecifier
    |> Option.iter (fun _ -> this._view.HotKeySpecifier <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.button.isDefault
    |> Option.iter (fun _ -> view.IsDefault <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.button.noDecorations
    |> Option.iter (fun _ -> view.NoDecorations <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.button.noPadding
    |> Option.iter (fun _ -> view.NoPadding <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.button.text
    |> Option.iter (fun _ -> view.Text <- Unchecked.defaultof<_>)
    // Events
    props
    |> Props.tryFind PKey.button.wantContinuousButtonPressed
    |> Option.iter (fun _ -> view.WantContinuousButtonPressed <- Unchecked.defaultof<_>)

  override _.name = $"Button"

  override this.setProps(props: Props) =
    base.setProps props

    let view = (this :> IInternalTerminalElement).view :?> Button

    // Properties
    props
    |> Props.tryFind PKey.button.hotKeySpecifier
    |> Option.iter (fun v -> this._view.HotKeySpecifier <- v)

    props
    |> Props.tryFind PKey.button.isDefault
    |> Option.iter (fun v -> view.IsDefault <- v)

    props
    |> Props.tryFind PKey.button.noDecorations
    |> Option.iter (fun v -> view.NoDecorations <- v)

    props
    |> Props.tryFind PKey.button.noPadding
    |> Option.iter (fun v -> view.NoPadding <- v)

    props
    |> Props.tryFind PKey.button.text
    |> Option.iter (fun v -> view.Text <- v)
    // Events
    props
    |> Props.tryFind PKey.button.wantContinuousButtonPressed
    |> Option.iter (fun v -> view.WantContinuousButtonPressed <- v)


  override this.newView() = new Button()

// CheckBox
type CheckBoxElement(props: Props) =
  inherit TerminalElement(props)

  override this.removeProps(props: Props) =
    base.removeProps props
    let view = (this :> IInternalTerminalElement).view :?> CheckBox
    // Properties
    props
    |> Props.tryFind PKey.checkBox.allowCheckStateNone
    |> Option.iter (fun _ -> view.AllowCheckStateNone <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.checkBox.checkedState
    |> Option.iter (fun _ -> view.CheckedState <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.checkBox.hotKeySpecifier
    |> Option.iter (fun _ -> view.HotKeySpecifier <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.checkBox.radioStyle
    |> Option.iter (fun _ -> view.RadioStyle <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.checkBox.text
    |> Option.iter (fun _ -> view.Text <- Unchecked.defaultof<_>)
    // Events
    props
    |> Props.tryFind PKey.checkBox.checkedStateChanging
    |> Option.iter (fun _ -> this.propsEventRegistry.removeHandler (PKey.checkBox.checkedStateChanging, LibraryUser))

    props
    |> Props.tryFind PKey.checkBox.checkedStateChanged
    |> Option.iter (fun _ -> this.propsEventRegistry.removeHandler (PKey.checkBox.checkedStateChanged, LibraryUser))

  override _.name = $"CheckBox"

  override this.setProps(props: Props) =
    base.setProps props

    let view = (this :> IInternalTerminalElement).view :?> CheckBox

    // Properties
    props
    |> Props.tryFind PKey.checkBox.allowCheckStateNone
    |> Option.iter (fun v -> view.AllowCheckStateNone <- v)

    props
    |> Props.tryFind PKey.checkBox.checkedState
    |> Option.iter (fun v -> view.CheckedState <- v)

    props
    |> Props.tryFind PKey.checkBox.hotKeySpecifier
    |> Option.iter (fun v -> view.HotKeySpecifier <- v)

    props
    |> Props.tryFind PKey.checkBox.radioStyle
    |> Option.iter (fun v -> view.RadioStyle <- v)

    props
    |> Props.tryFind PKey.checkBox.text
    |> Option.iter (fun v -> view.Text <- v)
    // Events
    props
    |> Props.tryFind PKey.checkBox.checkedStateChanging
    |> Option.iter (fun v -> this.propsEventRegistry.setEventHandler(PKey.checkBox.checkedStateChanging, LibraryUser, view.CheckedStateChanging, v))

    props
    |> Props.tryFind PKey.checkBox.checkedStateChanged
    |> Option.iter (fun v -> this.propsEventRegistry.setEventHandler(PKey.checkBox.checkedStateChanged, LibraryUser, view.CheckedStateChanged, v))


  override this.newView() = new CheckBox()

// ColorPicker
type ColorPickerElement(props: Props) =
  inherit TerminalElement(props)

  override this.removeProps(props: Props) =
    base.removeProps props
    let view = (this :> IInternalTerminalElement).view :?> ColorPicker
    // Properties
    props
    |> Props.tryFind PKey.colorPicker.selectedColor
    |> Option.iter (fun _ -> view.SelectedColor <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.colorPicker.style
    |> Option.iter (fun _ -> view.Style <- Unchecked.defaultof<_>)
    // Events
    props
    |> Props.tryFind PKey.colorPicker.colorChanged
    |> Option.iter (fun _ -> this.propsEventRegistry.removeHandler (PKey.colorPicker.colorChanged, LibraryUser))

  override _.name = $"ColorPicker"

  override this.setProps(props: Props) =
    base.setProps props

    let view = (this :> IInternalTerminalElement).view :?> ColorPicker

    // Properties
    props
    |> Props.tryFind PKey.colorPicker.selectedColor
    |> Option.iter (fun v -> view.SelectedColor <- v)

    props
    |> Props.tryFind PKey.colorPicker.style
    |> Option.iter (fun v -> view.Style <- v)
    // Events
    props
    |> Props.tryFind PKey.colorPicker.colorChanged
    |> Option.iter (fun v -> this.propsEventRegistry.setEventHandler(PKey.colorPicker.colorChanged, LibraryUser, view.ColorChanged, v))


  override this.newView() = new ColorPicker()

// ColorPicker16
type ColorPicker16Element(props: Props) =
  inherit TerminalElement(props)

  override this.removeProps(props: Props) =
    base.removeProps props
    let view = (this :> IInternalTerminalElement).view :?> ColorPicker16
    // Properties
    props
    |> Props.tryFind PKey.colorPicker16.boxHeight
    |> Option.iter (fun _ -> view.BoxHeight <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.colorPicker16.boxWidth
    |> Option.iter (fun _ -> view.BoxWidth <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.colorPicker16.cursor
    |> Option.iter (fun _ -> view.Cursor <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.colorPicker16.selectedColor
    |> Option.iter (fun _ -> view.SelectedColor <- Unchecked.defaultof<_>)
    // Events
    props
    |> Props.tryFind PKey.colorPicker16.colorChanged
    |> Option.iter (fun _ -> this.propsEventRegistry.removeHandler (PKey.colorPicker16.colorChanged, LibraryUser))

  override _.name = $"ColorPicker16"

  override this.setProps(props: Props) =
    base.setProps props

    let view = (this :> IInternalTerminalElement).view :?> ColorPicker16

    // Properties
    props
    |> Props.tryFind PKey.colorPicker16.boxHeight
    |> Option.iter (fun v -> view.BoxHeight <- v)

    props
    |> Props.tryFind PKey.colorPicker16.boxWidth
    |> Option.iter (fun v -> view.BoxWidth <- v)

    props
    |> Props.tryFind PKey.colorPicker16.cursor
    |> Option.iter (fun v -> view.Cursor <- v)

    props
    |> Props.tryFind PKey.colorPicker16.selectedColor
    |> Option.iter (fun v -> view.SelectedColor <- v)
    // Events
    props
    |> Props.tryFind PKey.colorPicker16.colorChanged
    |> Option.iter (fun v -> this.propsEventRegistry.setEventHandler(PKey.colorPicker16.colorChanged, LibraryUser, view.ColorChanged, v))


  override this.newView() = new ColorPicker16()

// ComboBox
type ComboBoxElement(props: Props) =
  inherit TerminalElement(props)

  override this.removeProps(props: Props) =
    base.removeProps props
    let view = (this :> IInternalTerminalElement).view :?> ComboBox
    // Properties
    props
    |> Props.tryFind PKey.comboBox.hideDropdownListOnClick
    |> Option.iter (fun _ -> view.HideDropdownListOnClick <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.comboBox.readOnly
    |> Option.iter (fun _ -> view.ReadOnly <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.comboBox.searchText
    |> Option.iter (fun _ -> view.SearchText <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.comboBox.selectedItem
    |> Option.iter (fun _ -> view.SelectedItem <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.comboBox.source
    |> Option.iter (fun _ -> view.SetSource Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.comboBox.text
    |> Option.iter (fun _ -> view.Text <- Unchecked.defaultof<_>)
    // Events
    props
    |> Props.tryFind PKey.comboBox.collapsed
    |> Option.iter (fun _ -> this.propsEventRegistry.removeHandler (PKey.comboBox.collapsed, LibraryUser))

    props
    |> Props.tryFind PKey.comboBox.expanded
    |> Option.iter (fun _ -> this.propsEventRegistry.removeHandler (PKey.comboBox.expanded, LibraryUser))

    props
    |> Props.tryFind PKey.comboBox.openSelectedItem
    |> Option.iter (fun _ -> this.propsEventRegistry.removeHandler (PKey.comboBox.openSelectedItem, LibraryUser))

    props
    |> Props.tryFind PKey.comboBox.selectedItemChanged
    |> Option.iter (fun _ -> this.propsEventRegistry.removeHandler (PKey.comboBox.selectedItemChanged, LibraryUser))

  override _.name = $"ComboBox"

  override this.setProps(props: Props) =
    base.setProps props

    let view = (this :> IInternalTerminalElement).view :?> ComboBox

    // Properties
    props
    |> Props.tryFind PKey.comboBox.hideDropdownListOnClick
    |> Option.iter (fun v -> view.HideDropdownListOnClick <- v)

    props
    |> Props.tryFind PKey.comboBox.readOnly
    |> Option.iter (fun v -> view.ReadOnly <- v)

    props
    |> Props.tryFind PKey.comboBox.searchText
    |> Option.iter (fun v -> view.SearchText <- v)

    props
    |> Props.tryFind PKey.comboBox.selectedItem
    |> Option.iter (fun v -> view.SelectedItem <- v)

    props
    |> Props.tryFind PKey.comboBox.source
    |> Option.iter (fun v -> view.SetSource(ObservableCollection(v)))

    props
    |> Props.tryFind PKey.comboBox.text
    |> Option.iter (fun v -> this._view.Text <- v)
    // Events
    props
    |> Props.tryFind PKey.comboBox.collapsed
    |> Option.iter (fun v -> this.propsEventRegistry.setEventHandler(PKey.comboBox.collapsed, LibraryUser, view.Collapsed, v))

    props
    |> Props.tryFind PKey.comboBox.expanded
    |> Option.iter (fun v -> this.propsEventRegistry.setEventHandler(PKey.comboBox.expanded, LibraryUser, view.Expanded, v))

    props
    |> Props.tryFind PKey.comboBox.openSelectedItem
    |> Option.iter (fun v -> this.propsEventRegistry.setEventHandler(PKey.comboBox.openSelectedItem, LibraryUser, view.OpenSelectedItem, v))

    props
    |> Props.tryFind PKey.comboBox.selectedItemChanged
    |> Option.iter (fun v -> this.propsEventRegistry.setEventHandler(PKey.comboBox.selectedItemChanged, LibraryUser, view.SelectedItemChanged, v))


  override this.newView() = new ComboBox()

// DateField
type DateFieldElement(props: Props) =
  inherit TerminalElement(props)

  override this.removeProps(props: Props) =
    base.removeProps props
    let view = (this :> IInternalTerminalElement).view :?> DateField
    // Properties
    props
    |> Props.tryFind PKey.dateField.culture
    |> Option.iter (fun _ -> view.Culture <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.dateField.cursorPosition
    |> Option.iter (fun _ -> view.CursorPosition <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.dateField.date
    |> Option.iter (fun _ -> view.Date <- Unchecked.defaultof<_>)
    // Events
    props
    |> Props.tryFind PKey.dateField.dateChanged
    |> Option.iter (fun _ -> this.propsEventRegistry.removeHandler (PKey.dateField.dateChanged, LibraryUser))

  override _.name = $"DateField"

  override this.setProps(props: Props) =
    base.setProps props

    let view = (this :> IInternalTerminalElement).view :?> DateField

    // Properties
    props
    |> Props.tryFind PKey.dateField.culture
    |> Option.iter (fun v -> view.Culture <- v)

    props
    |> Props.tryFind PKey.dateField.cursorPosition
    |> Option.iter (fun v -> view.CursorPosition <- v)

    props
    |> Props.tryFind PKey.dateField.date
    |> Option.iter (fun v -> view.Date <- v)
    // Events
    props
    |> Props.tryFind PKey.dateField.dateChanged
    |> Option.iter (fun v -> this.propsEventRegistry.setEventHandler(PKey.dateField.dateChanged, LibraryUser, view.DateChanged, v))


  override this.newView() = new DateField()

// DatePicker
type DatePickerElement(props: Props) =
  inherit TerminalElement(props)

  override this.removeProps(props: Props) =
    base.removeProps props
    let view = (this :> IInternalTerminalElement).view :?> DatePicker
    // Properties
    props
    |> Props.tryFind PKey.datePicker.culture
    |> Option.iter (fun _ -> view.Culture <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.datePicker.date
    |> Option.iter (fun _ -> view.Date <- Unchecked.defaultof<_>)

  override _.name = $"DatePicker"

  override this.setProps(props: Props) =
    base.setProps props

    let view = (this :> IInternalTerminalElement).view :?> DatePicker

    // Properties
    props
    |> Props.tryFind PKey.datePicker.culture
    |> Option.iter (fun v -> view.Culture <- v)

    props
    |> Props.tryFind PKey.datePicker.date
    |> Option.iter (fun v -> view.Date <- v)


  override this.newView() = new DatePicker()

// Dialog
type DialogElement(props: Props) =
  inherit TerminalElement(props)

  override this.removeProps(props: Props) =
    base.removeProps props
    let view = (this :> IInternalTerminalElement).view :?> Dialog
    // Properties
    props
    |> Props.tryFind PKey.dialog.buttonAlignment
    |> Option.iter (fun _ -> view.ButtonAlignment <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.dialog.buttonAlignmentModes
    |> Option.iter (fun _ -> view.ButtonAlignmentModes <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.dialog.canceled
    |> Option.iter (fun _ -> view.Canceled <- Unchecked.defaultof<_>)

  override _.name = $"Dialog"

  override this.setProps(props: Props) =
    base.setProps props

    let view = (this :> IInternalTerminalElement).view :?> Dialog

    // Properties
    props
    |> Props.tryFind PKey.dialog.buttonAlignment
    |> Option.iter (fun v -> view.ButtonAlignment <- v)

    props
    |> Props.tryFind PKey.dialog.buttonAlignmentModes
    |> Option.iter (fun v -> view.ButtonAlignmentModes <- v)

    props
    |> Props.tryFind PKey.dialog.canceled
    |> Option.iter (fun v -> view.Canceled <- v)


  override this.newView() = new Dialog()

// FileDialog
type FileDialogElement(props: Props) =
  inherit TerminalElement(props)

  override this.removeProps(props: Props) =
    base.removeProps props
    let view = (this :> IInternalTerminalElement).view :?> FileDialog
    // Properties
    props
    |> Props.tryFind PKey.fileDialog.allowedTypes
    |> Option.iter (fun _ -> view.AllowedTypes <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.fileDialog.allowsMultipleSelection
    |> Option.iter (fun _ -> view.AllowsMultipleSelection <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.fileDialog.fileOperationsHandler
    |> Option.iter (fun _ -> view.FileOperationsHandler <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.fileDialog.mustExist
    |> Option.iter (fun _ -> view.MustExist <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.fileDialog.openMode
    |> Option.iter (fun _ -> view.OpenMode <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.fileDialog.path
    |> Option.iter (fun _ -> view.Path <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.fileDialog.searchMatcher
    |> Option.iter (fun _ -> view.SearchMatcher <- Unchecked.defaultof<_>)
    // Events
    props
    |> Props.tryFind PKey.fileDialog.filesSelected
    |> Option.iter (fun _ -> this.propsEventRegistry.removeHandler (PKey.fileDialog.filesSelected, LibraryUser))

  override _.name = $"FileDialog"

  override this.setProps(props: Props) =
    base.setProps props

    let view = (this :> IInternalTerminalElement).view :?> FileDialog

    // Properties
    props
    |> Props.tryFind PKey.fileDialog.allowedTypes
    |> Option.iter (fun v -> view.AllowedTypes <- List<_>(v))

    props
    |> Props.tryFind PKey.fileDialog.allowsMultipleSelection
    |> Option.iter (fun v -> view.AllowsMultipleSelection <- v)

    props
    |> Props.tryFind PKey.fileDialog.fileOperationsHandler
    |> Option.iter (fun v -> view.FileOperationsHandler <- v)

    props
    |> Props.tryFind PKey.fileDialog.mustExist
    |> Option.iter (fun v -> view.MustExist <- v)

    props
    |> Props.tryFind PKey.fileDialog.openMode
    |> Option.iter (fun v -> view.OpenMode <- v)

    props
    |> Props.tryFind PKey.fileDialog.path
    |> Option.iter (fun v -> view.Path <- v)

    props
    |> Props.tryFind PKey.fileDialog.searchMatcher
    |> Option.iter (fun v -> view.SearchMatcher <- v)
    // Events
    props
    |> Props.tryFind PKey.fileDialog.filesSelected
    |> Option.iter (fun v -> this.propsEventRegistry.setEventHandler(PKey.fileDialog.filesSelected, LibraryUser, view.FilesSelected, v))


  override this.newView() = new FileDialog()

// FrameView
type FrameViewElement(props: Props) =
  inherit TerminalElement(props)

  override this.removeProps(props: Props) = base.removeProps props
  // No properties or events FrameView

  override _.name = $"FrameView"

  override this.setProps(props: Props) = base.setProps props
  // No properties or events FrameView


  override this.newView() = new FrameView()

// GraphView
type GraphViewElement(props: Props) =
  inherit TerminalElement(props)

  override this.removeProps(props: Props) =
    base.removeProps props
    let view = (this :> IInternalTerminalElement).view :?> GraphView
    // Properties
    props
    |> Props.tryFind PKey.graphView.axisX
    |> Option.iter (fun _ -> view.AxisX <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.graphView.axisY
    |> Option.iter (fun _ -> view.AxisY <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.graphView.cellSize
    |> Option.iter (fun _ -> view.CellSize <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.graphView.graphColor
    |> Option.iter (fun _ -> view.GraphColor <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.graphView.marginBottom
    |> Option.iter (fun _ -> view.MarginBottom <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.graphView.marginLeft
    |> Option.iter (fun _ -> view.MarginLeft <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.graphView.scrollOffset
    |> Option.iter (fun _ -> view.ScrollOffset <- Unchecked.defaultof<_>)

  override _.name = $"GraphView"

  override this.setProps(props: Props) =
    base.setProps props

    let view = (this :> IInternalTerminalElement).view :?> GraphView

    // Properties
    props
    |> Props.tryFind PKey.graphView.axisX
    |> Option.iter (fun v -> view.AxisX <- v)

    props
    |> Props.tryFind PKey.graphView.axisY
    |> Option.iter (fun v -> view.AxisY <- v)

    props
    |> Props.tryFind PKey.graphView.cellSize
    |> Option.iter (fun v -> view.CellSize <- v)

    props
    |> Props.tryFind PKey.graphView.graphColor
    |> Option.iter (fun v -> view.GraphColor <- v |> Option.toNullable)

    props
    |> Props.tryFind PKey.graphView.marginBottom
    |> Option.iter (fun v -> view.MarginBottom <- (v |> uint32))

    props
    |> Props.tryFind PKey.graphView.marginLeft
    |> Option.iter (fun v -> view.MarginLeft <- (v |> uint32))

    props
    |> Props.tryFind PKey.graphView.scrollOffset
    |> Option.iter (fun v -> view.ScrollOffset <- v)


  override this.newView() = new GraphView()

// HexView
type HexViewElement(props: Props) =
  inherit TerminalElement(props)

  override this.removeProps(props: Props) =
    base.removeProps props
    let view = (this :> IInternalTerminalElement).view :?> HexView
    // Properties
    props
    |> Props.tryFind PKey.hexView.address
    |> Option.iter (fun _ -> view.Address <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.hexView.addressWidth
    |> Option.iter (fun _ -> view.AddressWidth <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.hexView.allowEdits
    |> Option.iter (fun _ -> view.BytesPerLine <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.hexView.readOnly
    |> Option.iter (fun _ -> view.ReadOnly <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.hexView.source
    |> Option.iter (fun _ -> view.Source <- Unchecked.defaultof<_>)
    // Events
    props
    |> Props.tryFind PKey.hexView.edited
    |> Option.iter (fun _ -> this.propsEventRegistry.removeHandler (PKey.hexView.edited, LibraryUser))

    props
    |> Props.tryFind PKey.hexView.positionChanged
    |> Option.iter (fun _ -> this.propsEventRegistry.removeHandler (PKey.hexView.positionChanged, LibraryUser))

  override _.name = $"HexView"

  override this.setProps(props: Props) =
    base.setProps props

    let view = (this :> IInternalTerminalElement).view :?> HexView

    // Properties
    props
    |> Props.tryFind PKey.hexView.address
    |> Option.iter (fun v -> view.Address <- v)

    props
    |> Props.tryFind PKey.hexView.addressWidth
    |> Option.iter (fun v -> view.AddressWidth <- v)

    props
    |> Props.tryFind PKey.hexView.allowEdits
    |> Option.iter (fun v -> view.BytesPerLine <- v)

    props
    |> Props.tryFind PKey.hexView.readOnly
    |> Option.iter (fun v -> view.ReadOnly <- v)

    props
    |> Props.tryFind PKey.hexView.source
    |> Option.iter (fun v -> view.Source <- v)
    // Events
    props
    |> Props.tryFind PKey.hexView.edited
    |> Option.iter (fun v -> this.propsEventRegistry.setEventHandler(PKey.hexView.edited, LibraryUser, view.Edited, v))

    props
    |> Props.tryFind PKey.hexView.positionChanged
    |> Option.iter (fun v -> this.propsEventRegistry.setEventHandler(PKey.hexView.positionChanged, LibraryUser, view.PositionChanged, v))


  override this.newView() = new HexView()

// Label
type LabelElement(props: Props) =
  inherit TerminalElement(props)

  override this.removeProps(props: Props) =
    base.removeProps props
    let view = (this :> IInternalTerminalElement).view :?> Label
    // Properties
    props
    |> Props.tryFind PKey.label.hotKeySpecifier
    |> Option.iter (fun _ -> view.HotKeySpecifier <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.label.text
    |> Option.iter (fun _ -> view.Text <- Unchecked.defaultof<_>)

  override _.name = $"Label"

  override this.setProps(props: Props) =
    base.setProps props

    let view = (this :> IInternalTerminalElement).view :?> Label

    // Properties
    props
    |> Props.tryFind PKey.label.hotKeySpecifier
    |> Option.iter (fun v -> view.HotKeySpecifier <- v)

    props
    |> Props.tryFind PKey.label.text
    |> Option.iter (fun v -> view.Text <- v)


  override this.newView() = new Label()

// LegendAnnotation
type LegendAnnotationElement(props: Props) =
  inherit TerminalElement(props)

  override this.removeProps(props: Props) = base.removeProps props
  // No properties or events LegendAnnotation

  override _.name = $"LegendAnnotation"

  override this.setProps(props: Props) = base.setProps props
  // No properties or events LegendAnnotation


  override this.newView() = new LegendAnnotation()

// Line
type LineElement(props: Props) =
  inherit TerminalElement(props)

  override this.removeProps(props: Props) =
    base.removeProps props
    let view = (this :> IInternalTerminalElement).view :?> Line
    // Interfaces
    OrientationInterface.removeProps this view props

  override _.name = $"Line"

  override this.setProps(props: Props) =
    base.setProps props

    let view = (this :> IInternalTerminalElement).view :?> Line

    // Interfaces
    OrientationInterface.setProps this view props


  override this.newView() = new Line()


// ListView
type ListViewElement(props: Props) =
  inherit TerminalElement(props)

  override this.removeProps(props: Props) =
    base.removeProps props
    let view = (this :> IInternalTerminalElement).view :?> ListView
    // Properties
    props
    |> Props.tryFind PKey.listView.allowsMarking
    |> Option.iter (fun _ -> view.AllowsMarking <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.listView.allowsMultipleSelection
    |> Option.iter (fun _ -> view.AllowsMultipleSelection <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.listView.leftItem
    |> Option.iter (fun _ -> view.LeftItem <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.listView.selectedItem
    |> Option.iter (fun _ -> view.SelectedItem <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.listView.source
    |> Option.iter (fun _ -> view.SetSource Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.listView.topItem
    |> Option.iter (fun _ -> view.TopItem <- Unchecked.defaultof<_>)
    // Events
    props
    |> Props.tryFind PKey.listView.collectionChanged
    |> Option.iter (fun _ -> this.propsEventRegistry.removeHandler (PKey.listView.collectionChanged, LibraryUser))

    props
    |> Props.tryFind PKey.listView.openSelectedItem
    |> Option.iter (fun _ -> this.propsEventRegistry.removeHandler (PKey.listView.openSelectedItem, LibraryUser))

    props
    |> Props.tryFind PKey.listView.rowRender
    |> Option.iter (fun _ -> this.propsEventRegistry.removeHandler (PKey.listView.rowRender, LibraryUser))

    props
    |> Props.tryFind PKey.listView.selectedItemChanged
    |> Option.iter (fun _ -> this.propsEventRegistry.removeHandler (PKey.listView.selectedItemChanged, LibraryUser))

  override _.name = $"ListView"

  override this.setProps(props: Props) =
    base.setProps props

    let view = (this :> IInternalTerminalElement).view :?> ListView

    // Properties
    props
    |> Props.tryFind PKey.listView.allowsMarking
    |> Option.iter (fun v -> view.AllowsMarking <- v)

    props
    |> Props.tryFind PKey.listView.allowsMultipleSelection
    |> Option.iter (fun v -> view.AllowsMultipleSelection <- v)

    props
    |> Props.tryFind PKey.listView.leftItem
    |> Option.iter (fun v -> view.LeftItem <- v)

    props
    |> Props.tryFind PKey.listView.selectedItem
    |> Option.iter (fun v -> view.SelectedItem <- v)

    props
    |> Props.tryFind PKey.listView.source
    |> Option.iter (fun v -> view.SetSource(ObservableCollection(v)))

    props
    |> Props.tryFind PKey.listView.topItem
    |> Option.iter (fun v -> view.TopItem <- v)
    // Events
    props
    |> Props.tryFind PKey.listView.collectionChanged
    |> Option.iter (fun v -> this.propsEventRegistry.setEventHandler(PKey.listView.collectionChanged, LibraryUser, view.CollectionChanged, v))

    props
    |> Props.tryFind PKey.listView.openSelectedItem
    |> Option.iter (fun v -> this.propsEventRegistry.setEventHandler(PKey.listView.openSelectedItem, LibraryUser, view.OpenSelectedItem, v))

    props
    |> Props.tryFind PKey.listView.rowRender
    |> Option.iter (fun v -> this.propsEventRegistry.setEventHandler(PKey.listView.rowRender, LibraryUser, view.RowRender, v))

    props
    |> Props.tryFind PKey.listView.selectedItemChanged
    |> Option.iter (fun v -> this.propsEventRegistry.setEventHandler(PKey.listView.selectedItemChanged, LibraryUser, view.SelectedItemChanged, v))


  override this.newView() = new ListView()

// Margin
type MarginElement(props: Props) =
  inherit TerminalElement(props)

  override this.removeProps(props: Props) =
    base.removeProps props
    let view = (this :> IInternalTerminalElement).view :?> Margin
    // Properties
    props
    |> Props.tryFind PKey.margin.shadowStyle
    |> Option.iter (fun _ -> view.ShadowStyle <- Unchecked.defaultof<_>)

  override _.name = $"Margin"

  override this.setProps(props: Props) =
    base.setProps props

    let view = (this :> IInternalTerminalElement).view :?> Margin

    // Properties
    props
    |> Props.tryFind PKey.margin.shadowStyle
    |> Option.iter (fun v -> view.ShadowStyle <- v)


  override this.newView() = new Margin()

// Menu
type MenuElement(props: Props) =
  inherit TerminalElement(props)

  override this.removeProps(props: Props) =
    base.removeProps props
    let view = (this :> IInternalTerminalElement).view :?> Menu
    // Properties
    props
    |> Props.tryFind PKey.menu.selectedMenuItem
    |> Option.iter (fun _ -> view.SelectedMenuItem <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.menu.superMenuItem
    |> Option.iter (fun _ -> view.SuperMenuItem <- Unchecked.defaultof<_>)
    // Events
    props
    |> Props.tryFind PKey.menu.accepted
    |> Option.iter (fun v -> this.propsEventRegistry.setEventHandler(PKey.menu.accepted, LibraryUser, view.Accepted, v))

    props
    |> Props.tryFind PKey.menu.selectedMenuItemChanged
    |> Option.iter (fun v -> this.propsEventRegistry.setEventHandler(PKey.menu.selectedMenuItemChanged, LibraryUser, view.SelectedMenuItemChanged, v))

    ()

  override _.name = $"Menu"

  override this.setProps(props: Props) =
    base.setProps props

    let view = (this :> IInternalTerminalElement).view :?> Menu

    // Properties
    props
    |> Props.tryFind PKey.menu.selectedMenuItem
    |> Option.iter (fun v -> view.SelectedMenuItem <- v)

    props
    |> Props.tryFind PKey.menu.superMenuItem
    |> Option.iter (fun v -> view.SuperMenuItem <- v)
    // Events
    props
    |> Props.tryFind PKey.menu.accepted
    |> Option.iter (fun v -> this.propsEventRegistry.setEventHandler(PKey.menu.accepted, LibraryUser, view.Accepted, v))

    props
    |> Props.tryFind PKey.menu.selectedMenuItemChanged
    |> Option.iter (fun v -> this.propsEventRegistry.setEventHandler(PKey.menu.selectedMenuItemChanged, LibraryUser, view.SelectedMenuItemChanged, v))

  override this.newView() = new Menu()


  override this.setAsChildOfParentView = false

  interface IMenuElement


type PopoverMenuElement(props: Props) =
  inherit TerminalElement(props)

  override this.removeProps(props: Props) =
    base.removeProps props
    let view = (this :> IInternalTerminalElement).view :?> PopoverMenu
    // Properties
    props
    |> Props.tryFind PKey.popoverMenu.key
    |> Option.iter (fun _ -> view.Key <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.popoverMenu.mouseFlags
    |> Option.iter (fun _ -> view.MouseFlags <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.popoverMenu.root
    |> Option.iter (fun _ -> view.Root <- Unchecked.defaultof<_>)
    // Events
    props
    |> Props.tryFind PKey.popoverMenu.accepted
    |> Option.iter (fun _ -> this.propsEventRegistry.removeHandler (PKey.popoverMenu.accepted, LibraryUser))

    props
    |> Props.tryFind PKey.popoverMenu.keyChanged
    |> Option.iter (fun _ -> this.propsEventRegistry.removeHandler (PKey.popoverMenu.keyChanged, LibraryUser))

  override this.name = "PopoverMenu"

  override this.setProps(props: Props) =
    base.setProps props

    let view = (this :> IInternalTerminalElement).view :?> PopoverMenu

    // Properties
    props
    |> Props.tryFind PKey.popoverMenu.key
    |> Option.iter (fun v -> view.Key <- v)

    props
    |> Props.tryFind PKey.popoverMenu.mouseFlags
    |> Option.iter (fun v -> view.MouseFlags <- v)

    props
    |> Props.tryFind PKey.popoverMenu.root
    |> Option.iter (fun v -> view.Root <- v)
    // Events
    props
    |> Props.tryFind PKey.popoverMenu.accepted
    |> Option.iter (fun v -> this.propsEventRegistry.setEventHandler(PKey.popoverMenu.accepted, LibraryUser, view.Accepted, v))

    props
    |> Props.tryFind PKey.popoverMenu.keyChanged
    |> Option.iter (fun v -> this.propsEventRegistry.setEventHandler(PKey.popoverMenu.keyChanged, LibraryUser, view.KeyChanged, v))

  override this.SubElements_PropKeys =
    SubElementPropKey.from PKey.popoverMenu.root_element
    :: base.SubElements_PropKeys

  override this.newView() = new PopoverMenu()


  interface IPopoverMenuElement

// MenuBarItem
type MenuBarItemElement(props: Props) =
  inherit TerminalElement(props)

  override this.removeProps(props: Props) =
    base.removeProps props
    let view = (this :> IInternalTerminalElement).view :?> MenuBarItem
    // Properties
    props
    |> Props.tryFind PKey.menuBarItem.popoverMenu
    |> Option.iter (fun _ -> view.PopoverMenu <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.menuBarItem.popoverMenuOpen
    |> Option.iter (fun _ -> view.PopoverMenuOpen <- Unchecked.defaultof<_>)
    // Events
    props
    |> Props.tryFind PKey.menuBarItem.popoverMenuOpenChanged
    |> Option.iter (fun _ -> this.propsEventRegistry.removeHandler (PKey.menuBarItem.popoverMenuOpenChanged, LibraryUser))

  override this.name = "MenuBarItem"

  override this.setProps(props: Props) =
    base.setProps props

    let view = (this :> IInternalTerminalElement).view :?> MenuBarItem

    // Properties
    props
    |> Props.tryFind PKey.menuBarItem.popoverMenu
    |> Option.iter (fun v -> view.PopoverMenu <- v)

    props
    |> Props.tryFind PKey.menuBarItem.popoverMenuOpen
    |> Option.iter (fun v -> view.PopoverMenuOpen <- v)
    // Events
    props
    |> Props.tryFind PKey.menuBarItem.popoverMenuOpenChanged
    |> Option.iter (fun v -> this.propsEventRegistry.setEventHandler(PKey.menuBarItem.popoverMenuOpenChanged, LibraryUser, view.PopoverMenuOpenChanged, v))

  override this.SubElements_PropKeys =
    SubElementPropKey.from PKey.menuBarItem.popoverMenu_element
    :: base.SubElements_PropKeys

  override this.newView() = new MenuBarItem()


  interface IMenuBarItemElement

// MenuBar
type MenuBarElement(props: Props) =
  inherit TerminalElement(props)

  override this.removeProps(props: Props) =
    base.removeProps props
    let view = (this :> IInternalTerminalElement).view :?> MenuBar
    // Properties
    props
    |> Props.tryFind PKey.menuBar.key
    |> Option.iter (fun _ -> view.Key <- Unchecked.defaultof<_>)

    // NOTE: No need to handle `Menus: MenuBarItemElement list` property here,
    //       as it already registered as "children" property.
    //       And "children" properties are handled by the TreeDiff initializeTree function

    // Events
    props
    |> Props.tryFind PKey.menuBar.keyChanged
    |> Option.iter (fun _ -> this.propsEventRegistry.removeHandler (PKey.menuBar.keyChanged, LibraryUser))

  override _.name = $"MenuBar"

  override this.setProps(props: Props) =
    base.setProps props

    let view = (this :> IInternalTerminalElement).view :?> MenuBar

    // Properties
    props
    |> Props.tryFind PKey.menuBar.key
    |> Option.iter (fun v -> view.Key <- v)

    // NOTE: No need to handle `Menus: MenuBarItemElement list` property here,
    //       as it already registered as "children" property.
    //       And "children" properties are handled by the TreeDiff initializeTree function

    // Events
    props
    |> Props.tryFind PKey.menuBar.keyChanged
    |> Option.iter (fun v -> this.propsEventRegistry.setEventHandler(PKey.menuBar.keyChanged, LibraryUser, view.KeyChanged, v))

  override this.newView() = new MenuBar()

// Shortcut
type ShortcutElement(props: Props) =
  inherit TerminalElement(props)

  override this.removeProps(props: Props) =
    base.removeProps props
    let view = (this :> IInternalTerminalElement).view :?> Shortcut
    // Interfaces
    OrientationInterface.removeProps this view props

    // Properties
    props
    |> Props.tryFind PKey.shortcut.action
    |> Option.iter (fun _ -> view.Action <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.shortcut.alignmentModes
    |> Option.iter (fun _ -> view.AlignmentModes <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.shortcut.commandView
    |> Option.iter (fun _ -> view.CommandView <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.shortcut.forceFocusColors
    |> Option.iter (fun _ -> view.ForceFocusColors <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.shortcut.helpText
    |> Option.iter (fun _ -> view.HelpText <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.shortcut.text
    |> Option.iter (fun _ -> view.Text <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.shortcut.bindKeyToApplication
    |> Option.iter (fun _ -> view.BindKeyToApplication <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.shortcut.key
    |> Option.iter (fun _ -> view.Key <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.shortcut.minimumKeyTextSize
    |> Option.iter (fun _ -> view.MinimumKeyTextSize <- Unchecked.defaultof<_>)

  override _.name = $"Shortcut"

  override this.setProps(props: Props) =
    base.setProps props

    let view = (this :> IInternalTerminalElement).view :?> Shortcut

    // Interfaces
    OrientationInterface.setProps this view props

    // Properties
    props
    |> Props.tryFind PKey.shortcut.action
    |> Option.iter (fun v -> view.Action <- v)

    props
    |> Props.tryFind PKey.shortcut.alignmentModes
    |> Option.iter (fun v -> view.AlignmentModes <- v)

    props
    |> Props.tryFind PKey.shortcut.commandView
    |> Option.iter (fun v -> view.CommandView <- v)

    props
    |> Props.tryFind PKey.shortcut.forceFocusColors
    |> Option.iter (fun v -> view.ForceFocusColors <- v)

    props
    |> Props.tryFind PKey.shortcut.helpText
    |> Option.iter (fun v -> view.HelpText <- v)

    props
    |> Props.tryFind PKey.shortcut.text
    |> Option.iter (fun v -> view.Text <- v)

    props
    |> Props.tryFind PKey.shortcut.bindKeyToApplication
    |> Option.iter (fun v -> view.BindKeyToApplication <- v)

    props
    |> Props.tryFind PKey.shortcut.key
    |> Option.iter (fun v -> view.Key <- v)

    props
    |> Props.tryFind PKey.shortcut.minimumKeyTextSize
    |> Option.iter (fun v -> view.MinimumKeyTextSize <- v)

  override this.SubElements_PropKeys =
    SubElementPropKey.from PKey.shortcut.commandView_element
    :: base.SubElements_PropKeys

  override this.newView() = new Shortcut()

type MenuItemElement(props: Props) =
  inherit ShortcutElement(props)

  override this.removeProps(props: Props) =
    base.removeProps props
    let view = (this :> IInternalTerminalElement).view :?> MenuItem
    // Properties
    props
    |> Props.tryFind PKey.menuItem.command
    |> Option.iter (fun _ -> Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.menuItem.subMenu
    |> Option.iter (fun _ -> Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.menuItem.targetView
    |> Option.iter (fun _ -> Unchecked.defaultof<_>)
    // Events
    props
    |> Props.tryFind PKey.menuItem.accepted
    |> Option.iter (fun _ -> this.propsEventRegistry.removeHandler (PKey.menuItem.accepted, LibraryUser))

  override _.name = $"MenuItem"

  override this.setProps(props: Props) =
    base.setProps props

    let view = (this :> IInternalTerminalElement).view :?> MenuItem

    // Properties
    props
    |> Props.tryFind PKey.menuItem.command
    |> Option.iter (fun v -> view.Command <- v)

    props
    |> Props.tryFind PKey.menuItem.subMenu
    |> Option.iter (fun v -> view.SubMenu <- v)

    props
    |> Props.tryFind PKey.menuItem.targetView
    |> Option.iter (fun v -> view.TargetView <- v)
    // Events
    props
    |> Props.tryFind PKey.menuItem.accepted
    |> Option.iter (fun v -> this.propsEventRegistry.setEventHandler(PKey.menuItem.accepted, LibraryUser, view.Accepted, v))

  override this.SubElements_PropKeys =
    SubElementPropKey.from PKey.menuItem.subMenu_element
    :: base.SubElements_PropKeys


  override this.newView() = new MenuItem()

// NumericUpDown<'a>
type NumericUpDownElement<'a>(props: Props) =
  inherit TerminalElement(props)

  override this.removeProps(props: Props) =
    base.removeProps props
    let view = (this :> IInternalTerminalElement).view :?> NumericUpDown<'a>
    // Properties
    props
    |> Props.tryFind PKey.numericUpDown<'a>.format
    |> Option.iter (fun _ -> view.Format <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.numericUpDown<'a>.increment
    |> Option.iter (fun _ -> view.Increment <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.numericUpDown<'a>.value
    |> Option.iter (fun _ -> view.Value <- Unchecked.defaultof<_>)
    // Events
    props
    |> Props.tryFind PKey.numericUpDown<'a>.formatChanged
    |> Option.iter (fun _ -> this.propsEventRegistry.removeHandler (PKey.numericUpDown<'a>.formatChanged, LibraryUser))

    props
    |> Props.tryFind PKey.numericUpDown<'a>.incrementChanged
    |> Option.iter (fun _ -> this.propsEventRegistry.removeHandler (PKey.numericUpDown<'a>.incrementChanged, LibraryUser))

    props
    |> Props.tryFind PKey.numericUpDown<'a>.valueChanged
    |> Option.iter (fun _ -> this.propsEventRegistry.removeHandler (PKey.numericUpDown<'a>.valueChanged, LibraryUser))

    props
    |> Props.tryFind PKey.numericUpDown<'a>.valueChanging
    |> Option.iter (fun _ -> this.propsEventRegistry.removeHandler (PKey.numericUpDown<'a>.valueChanging, LibraryUser))

  override _.name = $"NumericUpDown<'a>"

  override this.setProps(props: Props) =
    base.setProps props

    let view = (this :> IInternalTerminalElement).view :?> NumericUpDown<'a>

    // Properties
    props
    |> Props.tryFind PKey.numericUpDown<'a>.format
    |> Option.iter (fun v -> view.Format <- v)

    props
    |> Props.tryFind PKey.numericUpDown<'a>.increment
    |> Option.iter (fun v -> view.Increment <- v)

    props
    |> Props.tryFind PKey.numericUpDown<'a>.value
    |> Option.iter (fun v -> view.Value <- v)
    // Events
    props
    |> Props.tryFind PKey.numericUpDown<'a>.formatChanged
    |> Option.iter (fun v -> this.propsEventRegistry.setEventHandler(PKey.numericUpDown<'a>.formatChanged, LibraryUser, view.FormatChanged, v))

    props
    |> Props.tryFind PKey.numericUpDown<'a>.incrementChanged
    |> Option.iter (fun v -> this.propsEventRegistry.setEventHandler(PKey.numericUpDown<'a>.incrementChanged, LibraryUser, view.IncrementChanged, v))

    props
    |> Props.tryFind PKey.numericUpDown<'a>.valueChanged
    |> Option.iter (fun v -> this.propsEventRegistry.setEventHandler(PKey.numericUpDown<'a>.valueChanged, LibraryUser, view.ValueChanged, v))

    props
    |> Props.tryFind PKey.numericUpDown<'a>.valueChanging
    |> Option.iter (fun v -> this.propsEventRegistry.setEventHandler(PKey.numericUpDown<'a>.valueChanging, LibraryUser, view.ValueChanging, v))

  override this.newView() = new NumericUpDown<'a>()


  interface INumericUpDownElement


// NumericUpDown
type NumericUpDownElement(props: Props) =
  inherit NumericUpDownElement<int>(props)

// OpenDialog
type OpenDialogElement(props: Props) =
  inherit TerminalElement(props)

  override this.removeProps(props: Props) =
    base.removeProps props
    let view = (this :> IInternalTerminalElement).view :?> OpenDialog
    // Properties
    props
    |> Props.tryFind PKey.openDialog.openMode
    |> Option.iter (fun _ -> view.OpenMode <- Unchecked.defaultof<_>)

  override _.name = $"OpenDialog"

  override this.setProps(props: Props) =
    base.setProps props

    let view = (this :> IInternalTerminalElement).view :?> OpenDialog

    // Properties
    props
    |> Props.tryFind PKey.openDialog.openMode
    |> Option.iter (fun v -> view.OpenMode <- v)


  override this.newView() = new OpenDialog()

// SelectorBase
type internal SelectorBaseElement(props: Props) =
  inherit TerminalElement(props)

  override this.removeProps(props: Props) =
    base.removeProps props
    let view = (this :> IInternalTerminalElement).view :?> SelectorBase
    // Interfaces
    OrientationInterface.removeProps this view props

    // Properties
    props
    |> Props.tryFind PKey.selectorBase.assignHotKeys
    |> Option.iter (fun _ -> view.AssignHotKeys <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.selectorBase.doubleClickAccepts
    |> Option.iter (fun _ -> view.DoubleClickAccepts <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.selectorBase.horizontalSpace
    |> Option.iter (fun _ -> view.HorizontalSpace <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.selectorBase.labels
    |> Option.iter (fun _ -> view.Labels <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.selectorBase.styles
    |> Option.iter (fun _ -> view.Styles <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.selectorBase.usedHotKeys
    |> Option.iter (fun _ -> view.UsedHotKeys <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.selectorBase.value
    |> Option.iter (fun _ -> view.Value <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.selectorBase.values
    |> Option.iter (fun _ -> view.Values <- Unchecked.defaultof<_>)
    // Events
    props
    |> Props.tryFind PKey.selectorBase.valueChanged
    |> Option.iter (fun _ -> this.propsEventRegistry.removeHandler (PKey.selectorBase.valueChanged, LibraryUser))

  override _.name = "SelectorBase"

  override this.setProps(props: Props) =
    base.setProps props

    let view = (this :> IInternalTerminalElement).view :?> SelectorBase

    // Interfaces
    OrientationInterface.setProps this view props

    // Properties
    props
    |> Props.tryFind PKey.selectorBase.assignHotKeys
    |> Option.iter (fun v -> view.AssignHotKeys <- v)

    props
    |> Props.tryFind PKey.selectorBase.doubleClickAccepts
    |> Option.iter (fun v -> view.DoubleClickAccepts <- v)

    props
    |> Props.tryFind PKey.selectorBase.horizontalSpace
    |> Option.iter (fun v -> view.HorizontalSpace <- v)

    props
    |> Props.tryFind PKey.selectorBase.labels
    |> Option.iter (fun v -> view.Labels <- v)

    props
    |> Props.tryFind PKey.selectorBase.styles
    |> Option.iter (fun v -> view.Styles <- v)

    props
    |> Props.tryFind PKey.selectorBase.usedHotKeys
    |> Option.iter (fun v -> view.UsedHotKeys <- v)

    props
    |> Props.tryFind PKey.selectorBase.value
    |> Option.iter (fun v -> view.Value <- v)

    props
    |> Props.tryFind PKey.selectorBase.values
    |> Option.iter (fun v -> view.Values <- v)
    // Events
    props
    |> Props.tryFind PKey.selectorBase.valueChanged
    |> Option.iter (fun v -> this.propsEventRegistry.setEventHandler(PKey.selectorBase.valueChanged, LibraryUser, view.ValueChanged, v))

  override this.newView() = raise (NotImplementedException())

  override this.canReuseView _ _ = raise (NotImplementedException())

// OptionSelector
type OptionSelectorElement(props: Props) =
  inherit SelectorBaseElement(props)

  override this.removeProps(props: Props) =
    base.removeProps props
    let view = (this :> IInternalTerminalElement).view :?> OptionSelector

    // Properties
    props
    |> Props.tryFind PKey.optionSelector.cursor
    |> Option.iter (fun _ -> view.Cursor <- Unchecked.defaultof<_>)

  override _.name = $"OptionSelector"

  override this.setProps(props: Props) =
    base.setProps props

    let view = (this :> IInternalTerminalElement).view :?> OptionSelector

    // Properties
    props
    |> Props.tryFind PKey.optionSelector.cursor
    |> Option.iter (fun v -> view.Cursor <- v)

  override this.newView() = new OptionSelector()


// FlagSelector
type FlagSelectorElement(props: Props) =
  inherit SelectorBaseElement(props)

  override this.removeProps(props: Props) =
    base.removeProps props
    let view = (this :> IInternalTerminalElement).view :?> FlagSelector

    // Properties
    props
    |> Props.tryFind PKey.flagSelector.value
    |> Option.iter (fun _ -> view.Value <- Unchecked.defaultof<_>)

  override _.name = $"FlagSelector"

  override this.setProps(props: Props) =
    base.setProps props

    let view = (this :> IInternalTerminalElement).view :?> FlagSelector

    // Properties
    props
    |> Props.tryFind PKey.flagSelector.value
    |> Option.iter (fun v -> view.Value <- v)

  override this.newView() = new FlagSelector()

// Padding
type PaddingElement(props: Props) =
  inherit TerminalElement(props)

  override this.removeProps(props: Props) = base.removeProps props

  override _.name = $"Padding"

  override this.setProps(props: Props) = base.setProps props


  override this.newView() = new Padding()

// ProgressBar
type ProgressBarElement(props: Props) =
  inherit TerminalElement(props)

  override this.removeProps(props: Props) =
    base.removeProps props
    let view = (this :> IInternalTerminalElement).view :?> ProgressBar
    // Properties
    props
    |> Props.tryFind PKey.progressBar.bidirectionalMarquee
    |> Option.iter (fun _ -> view.BidirectionalMarquee <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.progressBar.fraction
    |> Option.iter (fun _ -> view.Fraction <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.progressBar.progressBarFormat
    |> Option.iter (fun _ -> view.ProgressBarFormat <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.progressBar.progressBarStyle
    |> Option.iter (fun _ -> view.ProgressBarStyle <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.progressBar.segmentCharacter
    |> Option.iter (fun _ -> view.SegmentCharacter <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.progressBar.text
    |> Option.iter (fun _ -> view.Text <- Unchecked.defaultof<_>)

  override _.name = $"ProgressBar"

  override this.setProps(props: Props) =
    base.setProps props

    let view = (this :> IInternalTerminalElement).view :?> ProgressBar

    // Properties
    props
    |> Props.tryFind PKey.progressBar.bidirectionalMarquee
    |> Option.iter (fun v -> view.BidirectionalMarquee <- v)

    props
    |> Props.tryFind PKey.progressBar.fraction
    |> Option.iter (fun v -> view.Fraction <- v)

    props
    |> Props.tryFind PKey.progressBar.progressBarFormat
    |> Option.iter (fun v -> view.ProgressBarFormat <- v)

    props
    |> Props.tryFind PKey.progressBar.progressBarStyle
    |> Option.iter (fun v -> view.ProgressBarStyle <- v)

    props
    |> Props.tryFind PKey.progressBar.segmentCharacter
    |> Option.iter (fun v -> view.SegmentCharacter <- v)

    props
    |> Props.tryFind PKey.progressBar.text
    |> Option.iter (fun v -> view.Text <- v)


  override this.newView() = new ProgressBar()

// SaveDialog
type SaveDialogElement(props: Props) =
  inherit TerminalElement(props)

  override this.removeProps(props: Props) = base.removeProps props
  // No properties or events SaveDialog

  override _.name = $"SaveDialog"

  override this.setProps(props: Props) = base.setProps props
  // No properties or events SaveDialog


  override this.newView() = new SaveDialog()

// ScrollBar
type ScrollBarElement(props: Props) =
  inherit TerminalElement(props)

  override this.removeProps(props: Props) =
    base.removeProps props
    let view = (this :> IInternalTerminalElement).view :?> ScrollBar
    // Interfaces
    OrientationInterface.removeProps this view props

    // Properties
    props
    |> Props.tryFind PKey.scrollBar.autoShow
    |> Option.iter (fun _ -> view.AutoShow <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.scrollBar.increment
    |> Option.iter (fun _ -> view.Increment <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.scrollBar.position
    |> Option.iter (fun _ -> view.Position <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.scrollBar.scrollableContentSize
    |> Option.iter (fun _ -> view.ScrollableContentSize <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.scrollBar.visibleContentSize
    |> Option.iter (fun _ -> view.VisibleContentSize <- Unchecked.defaultof<_>)
    // Events
    props
    |> Props.tryFind PKey.scrollBar.scrollableContentSizeChanged
    |> Option.iter (fun _ -> this.propsEventRegistry.removeHandler (PKey.scrollBar.scrollableContentSizeChanged, LibraryUser))

    props
    |> Props.tryFind PKey.scrollBar.sliderPositionChanged
    |> Option.iter (fun _ -> this.propsEventRegistry.removeHandler (PKey.scrollBar.sliderPositionChanged, LibraryUser))

  override _.name = $"ScrollBar"

  override this.setProps(props: Props) =
    base.setProps props

    let view = (this :> IInternalTerminalElement).view :?> ScrollBar

    // Interfaces
    OrientationInterface.setProps this view props

    // Properties
    props
    |> Props.tryFind PKey.scrollBar.autoShow
    |> Option.iter (fun v -> view.AutoShow <- v)

    props
    |> Props.tryFind PKey.scrollBar.increment
    |> Option.iter (fun v -> view.Increment <- v)

    props
    |> Props.tryFind PKey.scrollBar.position
    |> Option.iter (fun v -> view.Position <- v)

    props
    |> Props.tryFind PKey.scrollBar.scrollableContentSize
    |> Option.iter (fun v -> view.ScrollableContentSize <- v)

    props
    |> Props.tryFind PKey.scrollBar.visibleContentSize
    |> Option.iter (fun v -> view.VisibleContentSize <- v)
    // Events
    props
    |> Props.tryFind PKey.scrollBar.scrollableContentSizeChanged
    |> Option.iter (fun v -> this.propsEventRegistry.setEventHandler(PKey.scrollBar.scrollableContentSizeChanged, LibraryUser, view.ScrollableContentSizeChanged, v))

    props
    |> Props.tryFind PKey.scrollBar.sliderPositionChanged
    |> Option.iter (fun v -> this.propsEventRegistry.setEventHandler(PKey.scrollBar.sliderPositionChanged, LibraryUser, view.SliderPositionChanged, v))


  override this.newView() = new ScrollBar()

// ScrollSlider
type ScrollSliderElement(props: Props) =
  inherit TerminalElement(props)

  override this.removeProps(props: Props) =
    base.removeProps props
    let view = (this :> IInternalTerminalElement).view :?> ScrollSlider
    // Interfaces
    OrientationInterface.removeProps this view props

    // Properties
    props
    |> Props.tryFind PKey.scrollSlider.position
    |> Option.iter (fun _ -> view.Position <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.scrollSlider.size
    |> Option.iter (fun _ -> view.Size <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.scrollSlider.sliderPadding
    |> Option.iter (fun _ -> view.SliderPadding <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.scrollSlider.visibleContentSize
    |> Option.iter (fun _ -> view.VisibleContentSize <- Unchecked.defaultof<_>)
    // Events
    props
    |> Props.tryFind PKey.scrollSlider.positionChanged
    |> Option.iter (fun _ -> this.propsEventRegistry.removeHandler (PKey.scrollSlider.positionChanged, LibraryUser))

    props
    |> Props.tryFind PKey.scrollSlider.positionChanging
    |> Option.iter (fun _ -> this.propsEventRegistry.removeHandler (PKey.scrollSlider.positionChanging, LibraryUser))

    props
    |> Props.tryFind PKey.scrollSlider.scrolled
    |> Option.iter (fun _ -> this.propsEventRegistry.removeHandler (PKey.scrollSlider.scrolled, LibraryUser))

  override _.name = $"ScrollSlider"

  override this.setProps(props: Props) =
    base.setProps props

    let view = (this :> IInternalTerminalElement).view :?> ScrollSlider

    // Interfaces
    OrientationInterface.setProps this view props

    // Properties
    props
    |> Props.tryFind PKey.scrollSlider.position
    |> Option.iter (fun v -> view.Position <- v)

    props
    |> Props.tryFind PKey.scrollSlider.size
    |> Option.iter (fun v -> view.Size <- v)

    props
    |> Props.tryFind PKey.scrollSlider.sliderPadding
    |> Option.iter (fun v -> view.SliderPadding <- v)

    props
    |> Props.tryFind PKey.scrollSlider.visibleContentSize
    |> Option.iter (fun v -> view.VisibleContentSize <- v)
    // Events
    props
    |> Props.tryFind PKey.scrollSlider.positionChanged
    |> Option.iter (fun v -> this.propsEventRegistry.setEventHandler(PKey.scrollSlider.positionChanged, LibraryUser, view.PositionChanged, v))

    props
    |> Props.tryFind PKey.scrollSlider.positionChanging
    |> Option.iter (fun v -> this.propsEventRegistry.setEventHandler(PKey.scrollSlider.positionChanging, LibraryUser, view.PositionChanging, v))

    props
    |> Props.tryFind PKey.scrollSlider.scrolled
    |> Option.iter (fun v -> this.propsEventRegistry.setEventHandler(PKey.scrollSlider.scrolled, LibraryUser, view.Scrolled, v))


  override this.newView() = new ScrollSlider()


// Slider<'a>
type SliderElement<'a>(props: Props) =
  inherit TerminalElement(props)

  override this.removeProps(props: Props) =
    base.removeProps props
    let view = (this :> IInternalTerminalElement).view :?> Slider<'a>
    // Interfaces
    OrientationInterface.removeProps this view props

    // Properties
    props
    |> Props.tryFind PKey.slider<'a>.allowEmpty
    |> Option.iter (fun _ -> view.AllowEmpty <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.slider<'a>.focusedOption
    |> Option.iter (fun _ -> view.FocusedOption <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.slider<'a>.legendsOrientation
    |> Option.iter (fun _ -> view.LegendsOrientation <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.slider<'a>.minimumInnerSpacing
    |> Option.iter (fun _ -> view.MinimumInnerSpacing <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.slider<'a>.options
    |> Option.iter (fun _ -> view.Options <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.slider<'a>.rangeAllowSingle
    |> Option.iter (fun _ -> view.RangeAllowSingle <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.slider<'a>.showEndSpacing
    |> Option.iter (fun _ -> view.ShowEndSpacing <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.slider<'a>.showLegends
    |> Option.iter (fun _ -> view.ShowLegends <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.slider<'a>.style
    |> Option.iter (fun _ -> view.Style <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.slider<'a>.text
    |> Option.iter (fun _ -> view.Text <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.slider<'a>.``type``
    |> Option.iter (fun _ -> view.Type <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.slider<'a>.useMinimumSize
    |> Option.iter (fun _ -> view.UseMinimumSize <- Unchecked.defaultof<_>)
    // Events
    props
    |> Props.tryFind PKey.slider.optionFocused
    |> Option.iter (fun _ -> this.propsEventRegistry.removeHandler (PKey.slider.optionFocused, LibraryUser))

    props
    |> Props.tryFind PKey.slider.optionsChanged
    |> Option.iter (fun _ -> this.propsEventRegistry.removeHandler (PKey.slider.optionsChanged, LibraryUser))

  override _.name = $"Slider<'a>"

  override this.setProps(props: Props) =
    base.setProps props

    let view = (this :> IInternalTerminalElement).view :?> Slider<'a>

    // Interfaces
    OrientationInterface.setProps this view props

    // Properties
    props
    |> Props.tryFind PKey.slider.allowEmpty
    |> Option.iter (fun v -> view.AllowEmpty <- v)

    props
    |> Props.tryFind PKey.slider.focusedOption
    |> Option.iter (fun v -> view.FocusedOption <- v)

    props
    |> Props.tryFind PKey.slider.legendsOrientation
    |> Option.iter (fun v -> view.LegendsOrientation <- v)

    props
    |> Props.tryFind PKey.slider.minimumInnerSpacing
    |> Option.iter (fun v -> view.MinimumInnerSpacing <- v)

    props
    |> Props.tryFind PKey.slider.options
    |> Option.iter (fun v -> view.Options <- List<_>(v))

    props
    |> Props.tryFind PKey.slider.rangeAllowSingle
    |> Option.iter (fun v -> view.RangeAllowSingle <- v)

    props
    |> Props.tryFind PKey.slider.showEndSpacing
    |> Option.iter (fun v -> view.ShowEndSpacing <- v)

    props
    |> Props.tryFind PKey.slider.showLegends
    |> Option.iter (fun v -> view.ShowLegends <- v)

    props
    |> Props.tryFind PKey.slider.style
    |> Option.iter (fun v -> view.Style <- v)

    props
    |> Props.tryFind PKey.slider.text
    |> Option.iter (fun v -> view.Text <- v)

    props
    |> Props.tryFind PKey.slider.``type``
    |> Option.iter (fun v -> view.Type <- v)

    props
    |> Props.tryFind PKey.slider.useMinimumSize
    |> Option.iter (fun v -> view.UseMinimumSize <- v)
    // Events
    props
    |> Props.tryFind PKey.slider.optionFocused
    |> Option.iter (fun v -> this.propsEventRegistry.setEventHandler(PKey.slider.optionFocused, LibraryUser, view.OptionFocused, v))

    props
    |> Props.tryFind PKey.slider.optionsChanged
    |> Option.iter (fun v -> this.propsEventRegistry.setEventHandler(PKey.slider.optionsChanged, LibraryUser, view.OptionsChanged, v))


  override this.newView() = new Slider<'a>()


  interface ISliderElement

// Slider
type SliderElement(props: Props) =
  inherit TerminalElement(props)

  override this.removeProps(props: Props) = base.removeProps props
  // No properties or events Slider

  override _.name = $"Slider"

  override this.setProps(props: Props) = base.setProps props
  // No properties or events Slider

  override this.newView() = new Slider()

// SpinnerView
type SpinnerViewElement(props: Props) =
  inherit TerminalElement(props)

  override this.removeProps(props: Props) =
    base.removeProps props
    let view = (this :> IInternalTerminalElement).view :?> SpinnerView
    // Properties
    props
    |> Props.tryFind PKey.spinnerView.autoSpin
    |> Option.iter (fun _ -> view.AutoSpin <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.spinnerView.sequence
    |> Option.iter (fun _ -> view.Sequence <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.spinnerView.spinBounce
    |> Option.iter (fun _ -> view.SpinBounce <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.spinnerView.spinDelay
    |> Option.iter (fun _ -> view.SpinDelay <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.spinnerView.spinReverse
    |> Option.iter (fun _ -> view.SpinReverse <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.spinnerView.style
    |> Option.iter (fun _ -> view.Style <- Unchecked.defaultof<_>)

  override _.name = $"SpinnerView"

  override this.setProps(props: Props) =
    base.setProps props

    let view = (this :> IInternalTerminalElement).view :?> SpinnerView

    // Properties
    props
    |> Props.tryFind PKey.spinnerView.autoSpin
    |> Option.iter (fun v -> view.AutoSpin <- v)

    props
    |> Props.tryFind PKey.spinnerView.sequence
    |> Option.iter (fun v -> view.Sequence <- v |> List.toArray)

    props
    |> Props.tryFind PKey.spinnerView.spinBounce
    |> Option.iter (fun v -> view.SpinBounce <- v)

    props
    |> Props.tryFind PKey.spinnerView.spinDelay
    |> Option.iter (fun v -> view.SpinDelay <- v)

    props
    |> Props.tryFind PKey.spinnerView.spinReverse
    |> Option.iter (fun v -> view.SpinReverse <- v)

    props
    |> Props.tryFind PKey.spinnerView.style
    |> Option.iter (fun v -> view.Style <- v)


  override this.newView() = new SpinnerView()

// StatusBar
type StatusBarElement(props: Props) =
  inherit TerminalElement(props)

  override this.removeProps(props: Props) = base.removeProps props
  // No properties or events StatusBar

  override _.name = $"StatusBar"

  override this.setProps(props: Props) = base.setProps props
  // No properties or events StatusBar


  override this.newView() = new StatusBar()

// Tab
type TabElement(props: Props) =
  inherit TerminalElement(props)

  override this.removeProps(props: Props) =
    base.removeProps props
    let view = (this :> IInternalTerminalElement).view :?> Tab
    // Properties
    props
    |> Props.tryFind PKey.tab.displayText
    |> Option.iter (fun _ -> view.DisplayText <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.tab.view
    |> Option.iter (fun _ -> view.View <- Unchecked.defaultof<_>)

  override _.name = $"Tab"

  override this.setProps(props: Props) =
    base.setProps props

    let view = (this :> IInternalTerminalElement).view :?> Tab

    // Properties
    props
    |> Props.tryFind PKey.tab.displayText
    |> Option.iter (fun v -> view.DisplayText <- v)

  override this.SubElements_PropKeys =
    SubElementPropKey.from PKey.tab.view_element
    :: base.SubElements_PropKeys

  override this.newView() = new Tab()

// TabView
type TabViewElement(props: Props) =
  inherit TerminalElement(props)

  override this.removeProps(props: Props) =
    base.removeProps props
    let view = (this :> IInternalTerminalElement).view :?> TabView
    // Properties
    props
    |> Props.tryFind PKey.tabView.maxTabTextWidth
    |> Option.iter (fun _ -> view.MaxTabTextWidth <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.tabView.selectedTab
    |> Option.iter (fun _ -> view.SelectedTab <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.tabView.style
    |> Option.iter (fun _ -> view.Style <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.tabView.tabScrollOffset
    |> Option.iter (fun _ -> view.TabScrollOffset <- Unchecked.defaultof<_>)
    // Events
    props
    |> Props.tryFind PKey.tabView.selectedTabChanged
    |> Option.iter (fun _ -> this.propsEventRegistry.removeHandler (PKey.tabView.selectedTabChanged, LibraryUser))

    props
    |> Props.tryFind PKey.tabView.tabClicked
    |> Option.iter (fun _ -> this.propsEventRegistry.removeHandler (PKey.tabView.tabClicked, LibraryUser))

  override _.name = $"TabView"

  override this.setProps(props: Props) =
    base.setProps props

    let view = (this :> IInternalTerminalElement).view :?> TabView

    // Properties
    props
    |> Props.tryFind PKey.tabView.maxTabTextWidth
    |> Option.iter (fun v -> view.MaxTabTextWidth <- (v |> uint32))

    props
    |> Props.tryFind PKey.tabView.selectedTab
    |> Option.iter (fun v -> view.SelectedTab <- v)

    props
    |> Props.tryFind PKey.tabView.style
    |> Option.iter (fun v -> view.Style <- v)

    props
    |> Props.tryFind PKey.tabView.tabScrollOffset
    |> Option.iter (fun v -> view.TabScrollOffset <- v)
    // Events
    props
    |> Props.tryFind PKey.tabView.selectedTabChanged
    |> Option.iter (fun v -> this.propsEventRegistry.setEventHandler(PKey.tabView.selectedTabChanged, LibraryUser, view.SelectedTabChanged, v))

    props
    |> Props.tryFind PKey.tabView.tabClicked
    |> Option.iter (fun v -> this.propsEventRegistry.setEventHandler(PKey.tabView.tabClicked, LibraryUser, view.TabClicked, v))

    // Additional properties
    props
    |> Props.tryFind PKey.tabView.tabs
    |> Option.iter (fun tabItems ->
      tabItems
      |> Seq.iter (fun tabItem -> view.AddTab((tabItem :?> IInternalTerminalElement).view :?> Tab, false))
    )

  override this.SubElements_PropKeys =
    SubElementPropKey.from PKey.tabView.tabs_elements
    :: base.SubElements_PropKeys

  override this.newView() = new TabView()

// TableView
type TableViewElement(props: Props) =
  inherit TerminalElement(props)

  override this.removeProps(props: Props) =
    base.removeProps props
    let view = (this :> IInternalTerminalElement).view :?> TableView
    // Properties
    props
    |> Props.tryFind PKey.tableView.cellActivationKey
    |> Option.iter (fun _ -> view.CellActivationKey <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.tableView.collectionNavigator
    |> Option.iter (fun _ -> view.CollectionNavigator <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.tableView.columnOffset
    |> Option.iter (fun _ -> view.ColumnOffset <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.tableView.fullRowSelect
    |> Option.iter (fun _ -> view.FullRowSelect <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.tableView.maxCellWidth
    |> Option.iter (fun _ -> view.MaxCellWidth <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.tableView.minCellWidth
    |> Option.iter (fun _ -> view.MinCellWidth <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.tableView.multiSelect
    |> Option.iter (fun _ -> view.MultiSelect <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.tableView.nullSymbol
    |> Option.iter (fun _ -> view.NullSymbol <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.tableView.rowOffset
    |> Option.iter (fun _ -> view.RowOffset <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.tableView.selectedColumn
    |> Option.iter (fun _ -> view.SelectedColumn <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.tableView.selectedRow
    |> Option.iter (fun _ -> view.SelectedRow <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.tableView.separatorSymbol
    |> Option.iter (fun _ -> view.SeparatorSymbol <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.tableView.style
    |> Option.iter (fun _ -> view.Style <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.tableView.table
    |> Option.iter (fun _ -> view.Table <- Unchecked.defaultof<_>)
    // Events
    props
    |> Props.tryFind PKey.tableView.cellActivated
    |> Option.iter (fun _ -> this.propsEventRegistry.removeHandler (PKey.tableView.cellActivated, LibraryUser))

    props
    |> Props.tryFind PKey.tableView.cellToggled
    |> Option.iter (fun _ -> this.propsEventRegistry.removeHandler (PKey.tableView.cellToggled, LibraryUser))

    props
    |> Props.tryFind PKey.tableView.selectedCellChanged
    |> Option.iter (fun _ -> this.propsEventRegistry.removeHandler (PKey.tableView.selectedCellChanged, LibraryUser))

  override _.name = $"TableView"

  override this.setProps(props: Props) =
    base.setProps props

    let view = (this :> IInternalTerminalElement).view :?> TableView

    // Properties
    props
    |> Props.tryFind PKey.tableView.cellActivationKey
    |> Option.iter (fun v -> view.CellActivationKey <- v)

    props
    |> Props.tryFind PKey.tableView.collectionNavigator
    |> Option.iter (fun v -> view.CollectionNavigator <- v)

    props
    |> Props.tryFind PKey.tableView.columnOffset
    |> Option.iter (fun v -> view.ColumnOffset <- v)

    props
    |> Props.tryFind PKey.tableView.fullRowSelect
    |> Option.iter (fun v -> view.FullRowSelect <- v)

    props
    |> Props.tryFind PKey.tableView.maxCellWidth
    |> Option.iter (fun v -> view.MaxCellWidth <- v)

    props
    |> Props.tryFind PKey.tableView.minCellWidth
    |> Option.iter (fun v -> view.MinCellWidth <- v)

    props
    |> Props.tryFind PKey.tableView.multiSelect
    |> Option.iter (fun v -> view.MultiSelect <- v)

    props
    |> Props.tryFind PKey.tableView.nullSymbol
    |> Option.iter (fun v -> view.NullSymbol <- v)

    props
    |> Props.tryFind PKey.tableView.rowOffset
    |> Option.iter (fun v -> view.RowOffset <- v)

    props
    |> Props.tryFind PKey.tableView.selectedColumn
    |> Option.iter (fun v -> view.SelectedColumn <- v)

    props
    |> Props.tryFind PKey.tableView.selectedRow
    |> Option.iter (fun v -> view.SelectedRow <- v)

    props
    |> Props.tryFind PKey.tableView.separatorSymbol
    |> Option.iter (fun v -> view.SeparatorSymbol <- v)

    props
    |> Props.tryFind PKey.tableView.style
    |> Option.iter (fun v -> view.Style <- v)

    props
    |> Props.tryFind PKey.tableView.table
    |> Option.iter (fun v -> view.Table <- v)
    // Events
    props
    |> Props.tryFind PKey.tableView.cellActivated
    |> Option.iter (fun v -> this.propsEventRegistry.setEventHandler(PKey.tableView.cellActivated, LibraryUser, view.CellActivated, v))

    props
    |> Props.tryFind PKey.tableView.cellToggled
    |> Option.iter (fun v -> this.propsEventRegistry.setEventHandler(PKey.tableView.cellToggled, LibraryUser, view.CellToggled, v))

    props
    |> Props.tryFind PKey.tableView.selectedCellChanged
    |> Option.iter (fun v -> this.propsEventRegistry.setEventHandler(PKey.tableView.selectedCellChanged, LibraryUser, view.SelectedCellChanged, v))


  override this.newView() = new TableView()

// TextField
type TextFieldElement(props: Props) =
  inherit TerminalElement(props)

  override this.removeProps(props: Props) =
    base.removeProps props
    let view = (this :> IInternalTerminalElement).view :?> TextField
    // Properties
    props
    |> Props.tryFind PKey.textField.autocomplete
    |> Option.iter (fun _ -> view.Autocomplete <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.textField.cursorPosition
    |> Option.iter (fun _ -> view.CursorPosition <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.textField.readOnly
    |> Option.iter (fun _ -> view.ReadOnly <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.textField.secret
    |> Option.iter (fun _ -> view.Secret <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.textField.selectedStart
    |> Option.iter (fun _ -> view.SelectedStart <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.textField.selectWordOnlyOnDoubleClick
    |> Option.iter (fun _ -> view.SelectWordOnlyOnDoubleClick <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.textField.text
    |> Option.iter (fun _ -> view.Text <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.textField.used
    |> Option.iter (fun _ -> view.Used <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.textField.useSameRuneTypeForWords
    |> Option.iter (fun _ -> view.UseSameRuneTypeForWords <- Unchecked.defaultof<_>)
    // Events
    props
    |> Props.tryFind PKey.textField.textChanging
    |> Option.iter (fun _ -> this.propsEventRegistry.removeHandler (PKey.textField.textChanging, LibraryUser))

  override _.name = $"TextField"

  override this.setProps(props: Props) =
    base.setProps props

    let view = (this :> IInternalTerminalElement).view :?> TextField

    // Properties
    props
    |> Props.tryFind PKey.textField.autocomplete
    |> Option.iter (fun v -> view.Autocomplete <- v)

    props
    |> Props.tryFind PKey.textField.cursorPosition
    |> Option.iter (fun v -> view.CursorPosition <- v)

    props
    |> Props.tryFind PKey.textField.readOnly
    |> Option.iter (fun v -> view.ReadOnly <- v)

    props
    |> Props.tryFind PKey.textField.secret
    |> Option.iter (fun v -> view.Secret <- v)

    props
    |> Props.tryFind PKey.textField.selectedStart
    |> Option.iter (fun v -> view.SelectedStart <- v)

    props
    |> Props.tryFind PKey.textField.selectWordOnlyOnDoubleClick
    |> Option.iter (fun v -> view.SelectWordOnlyOnDoubleClick <- v)

    props
    |> Props.tryFind PKey.textField.text
    |> Option.iter (fun v -> view.Text <- v)

    props
    |> Props.tryFind PKey.textField.used
    |> Option.iter (fun v -> view.Used <- v)

    props
    |> Props.tryFind PKey.textField.useSameRuneTypeForWords
    |> Option.iter (fun v -> view.UseSameRuneTypeForWords <- v)
    // Events
    props
    |> Props.tryFind PKey.textField.textChanging
    |> Option.iter (fun v -> this.propsEventRegistry.setEventHandler(PKey.textField.textChanging, LibraryUser, view.TextChanging, v))


  override this.newView() = new TextField()

// TextValidateField
type TextValidateFieldElement(props: Props) =
  inherit TerminalElement(props)

  override this.removeProps(props: Props) =
    base.removeProps props
    let view = (this :> IInternalTerminalElement).view :?> TextValidateField
    // Properties
    props
    |> Props.tryFind PKey.textValidateField.provider
    |> Option.iter (fun _ -> view.Provider <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.textValidateField.text
    |> Option.iter (fun _ -> view.Text <- Unchecked.defaultof<_>)

  override _.name = $"TextValidateField"

  override this.setProps(props: Props) =
    base.setProps props

    let view = (this :> IInternalTerminalElement).view :?> TextValidateField

    // Properties
    props
    |> Props.tryFind PKey.textValidateField.provider
    |> Option.iter (fun v -> view.Provider <- v)

    props
    |> Props.tryFind PKey.textValidateField.text
    |> Option.iter (fun v -> view.Text <- v)


  override this.newView() = new TextValidateField()

// TextView
type TextViewElement(props: Props) =
  inherit TerminalElement(props)


  override this.removeProps(props: Props) =
    base.removeProps props
    let view = (this :> IInternalTerminalElement).view :?> TextView
    // Properties
    props
    |> Props.tryFind PKey.textView.allowsReturn
    |> Option.iter (fun _ -> view.AllowsReturn <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.textView.allowsTab
    |> Option.iter (fun _ -> view.AllowsTab <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.textView.cursorPosition
    |> Option.iter (fun _ -> view.CursorPosition <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.textView.inheritsPreviousAttribute
    |> Option.iter (fun _ -> view.InheritsPreviousAttribute <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.textView.isDirty
    |> Option.iter (fun _ -> view.IsDirty <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.textView.isSelecting
    |> Option.iter (fun _ -> view.IsSelecting <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.textView.leftColumn
    |> Option.iter (fun _ -> view.LeftColumn <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.textView.multiline
    |> Option.iter (fun _ -> view.Multiline <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.textView.readOnly
    |> Option.iter (fun _ -> view.ReadOnly <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.textView.selectionStartColumn
    |> Option.iter (fun _ -> view.SelectionStartColumn <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.textView.selectionStartRow
    |> Option.iter (fun _ -> view.SelectionStartRow <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.textView.selectWordOnlyOnDoubleClick
    |> Option.iter (fun _ -> view.SelectWordOnlyOnDoubleClick <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.textView.tabWidth
    |> Option.iter (fun _ -> view.TabWidth <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.textView.text
    |> Option.iter (fun _ -> view.Text <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.textView.topRow
    |> Option.iter (fun _ -> view.TopRow <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.textView.used
    |> Option.iter (fun _ -> view.Used <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.textView.useSameRuneTypeForWords
    |> Option.iter (fun _ -> view.UseSameRuneTypeForWords <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.textView.wordWrap
    |> Option.iter (fun _ -> view.WordWrap <- Unchecked.defaultof<_>)
    // Events
    props
    |> Props.tryFind PKey.textView.contentsChanged
    |> Option.iter (fun _ -> this.propsEventRegistry.removeHandler (PKey.textView.contentsChanged, LibraryUser))

    props
    |> Props.tryFind PKey.textView.drawNormalColor
    |> Option.iter (fun _ -> this.propsEventRegistry.removeHandler (PKey.textView.drawNormalColor, LibraryUser))

    props
    |> Props.tryFind PKey.textView.drawReadOnlyColor
    |> Option.iter (fun _ -> this.propsEventRegistry.removeHandler (PKey.textView.drawReadOnlyColor, LibraryUser))

    props
    |> Props.tryFind PKey.textView.drawSelectionColor
    |> Option.iter (fun _ -> this.propsEventRegistry.removeHandler (PKey.textView.drawSelectionColor, LibraryUser))

    props
    |> Props.tryFind PKey.textView.drawUsedColor
    |> Option.iter (fun _ -> this.propsEventRegistry.removeHandler (PKey.textView.drawUsedColor, LibraryUser))

    props
    |> Props.tryFind PKey.textView.unwrappedCursorPosition
    |> Option.iter (fun _ -> this.propsEventRegistry.removeHandler (PKey.textView.unwrappedCursorPosition, LibraryUser))

    // Additional properties
    props
    |> Props.tryFind PKey.textView.textChanged
    |> Option.iter (fun _ -> this.propsEventRegistry.removeHandler (PKey.textView.textChanged, LibraryUser))


  override _.name = $"TextView"

  override this.setProps(props: Props) =
    base.setProps props

    let view = (this :> IInternalTerminalElement).view :?> TextView

    // Properties
    props
    |> Props.tryFind PKey.textView.allowsReturn
    |> Option.iter (fun v -> view.AllowsReturn <- v)

    props
    |> Props.tryFind PKey.textView.allowsTab
    |> Option.iter (fun v -> view.AllowsTab <- v)

    props
    |> Props.tryFind PKey.textView.cursorPosition
    |> Option.iter (fun v -> view.CursorPosition <- v)

    props
    |> Props.tryFind PKey.textView.inheritsPreviousAttribute
    |> Option.iter (fun v -> view.InheritsPreviousAttribute <- v)

    props
    |> Props.tryFind PKey.textView.isDirty
    |> Option.iter (fun v -> view.IsDirty <- v)

    props
    |> Props.tryFind PKey.textView.isSelecting
    |> Option.iter (fun v -> view.IsSelecting <- v)

    props
    |> Props.tryFind PKey.textView.leftColumn
    |> Option.iter (fun v -> view.LeftColumn <- v)

    props
    |> Props.tryFind PKey.textView.multiline
    |> Option.iter (fun v -> view.Multiline <- v)

    props
    |> Props.tryFind PKey.textView.readOnly
    |> Option.iter (fun v -> view.ReadOnly <- v)

    props
    |> Props.tryFind PKey.textView.selectionStartColumn
    |> Option.iter (fun v -> view.SelectionStartColumn <- v)

    props
    |> Props.tryFind PKey.textView.selectionStartRow
    |> Option.iter (fun v -> view.SelectionStartRow <- v)

    props
    |> Props.tryFind PKey.textView.selectWordOnlyOnDoubleClick
    |> Option.iter (fun v -> view.SelectWordOnlyOnDoubleClick <- v)

    props
    |> Props.tryFind PKey.textView.tabWidth
    |> Option.iter (fun v -> view.TabWidth <- v)

    props
    |> Props.tryFind PKey.textView.text
    |> Option.iter (fun v -> view.Text <- v)

    props
    |> Props.tryFind PKey.textView.topRow
    |> Option.iter (fun v -> view.TopRow <- v)

    props
    |> Props.tryFind PKey.textView.used
    |> Option.iter (fun v -> view.Used <- v)

    props
    |> Props.tryFind PKey.textView.useSameRuneTypeForWords
    |> Option.iter (fun v -> view.UseSameRuneTypeForWords <- v)

    props
    |> Props.tryFind PKey.textView.wordWrap
    |> Option.iter (fun v -> view.WordWrap <- v)
    // Events
    props
    |> Props.tryFind PKey.textView.contentsChanged
    |> Option.iter (fun v -> this.propsEventRegistry.setEventHandler(PKey.textView.contentsChanged, LibraryUser, view.ContentsChanged, v))

    props
    |> Props.tryFind PKey.textView.drawNormalColor
    |> Option.iter (fun v -> this.propsEventRegistry.setEventHandler(PKey.textView.drawNormalColor, LibraryUser, view.DrawNormalColor, v))

    props
    |> Props.tryFind PKey.textView.drawReadOnlyColor
    |> Option.iter (fun v -> this.propsEventRegistry.setEventHandler(PKey.textView.drawReadOnlyColor, LibraryUser, view.DrawReadOnlyColor, v))

    props
    |> Props.tryFind PKey.textView.drawSelectionColor
    |> Option.iter (fun v -> this.propsEventRegistry.setEventHandler(PKey.textView.drawSelectionColor, LibraryUser, view.DrawSelectionColor, v))

    props
    |> Props.tryFind PKey.textView.drawUsedColor
    |> Option.iter (fun v -> this.propsEventRegistry.setEventHandler(PKey.textView.drawUsedColor, LibraryUser, view.DrawUsedColor, v))

    props
    |> Props.tryFind PKey.textView.unwrappedCursorPosition
    |> Option.iter (fun v -> this.propsEventRegistry.setEventHandler(PKey.textView.unwrappedCursorPosition, LibraryUser, view.UnwrappedCursorPosition, v))

    // Additional properties
    props
    |> Props.tryFind PKey.textView.textChanged
    |> Option.iter (fun v -> this.propsEventRegistry.setEventHandler(PKey.textView.textChanged, LibraryUser, view.ContentsChanged, v))


  override this.newView() = new TextView()

// TimeField
type TimeFieldElement(props: Props) =
  inherit TerminalElement(props)

  override this.removeProps(props: Props) =
    base.removeProps props
    let view = (this :> IInternalTerminalElement).view :?> TimeField
    // Properties
    props
    |> Props.tryFind PKey.timeField.cursorPosition
    |> Option.iter (fun _ -> view.CursorPosition <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.timeField.isShortFormat
    |> Option.iter (fun _ -> view.IsShortFormat <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.timeField.time
    |> Option.iter (fun _ -> view.Time <- Unchecked.defaultof<_>)
    // Events
    props
    |> Props.tryFind PKey.timeField.timeChanged
    |> Option.iter (fun _ -> this.propsEventRegistry.removeHandler (PKey.timeField.timeChanged, LibraryUser))

  override _.name = $"TimeField"

  override this.setProps(props: Props) =
    base.setProps props

    let view = (this :> IInternalTerminalElement).view :?> TimeField

    // Properties
    props
    |> Props.tryFind PKey.timeField.cursorPosition
    |> Option.iter (fun v -> view.CursorPosition <- v)

    props
    |> Props.tryFind PKey.timeField.isShortFormat
    |> Option.iter (fun v -> view.IsShortFormat <- v)

    props
    |> Props.tryFind PKey.timeField.time
    |> Option.iter (fun v -> view.Time <- v)
    // Events
    props
    |> Props.tryFind PKey.timeField.timeChanged
    |> Option.iter (fun v -> this.propsEventRegistry.setEventHandler(PKey.timeField.timeChanged, LibraryUser, view.TimeChanged, v))


  override this.newView() = new TimeField()

// Runnable
type RunnableElement(props: Props) =
  inherit TerminalElement(props)

  override this.removeProps(props: Props) =
    base.removeProps props
    let view = (this :> IInternalTerminalElement).view :?> Runnable
    // Properties
    props
    |> Props.tryFind PKey.runnable.isModal
    |> Option.iter (fun _ -> view.SetIsModal(Unchecked.defaultof<_>))

    props
    |> Props.tryFind PKey.runnable.isRunning
    |> Option.iter (fun _ -> view.SetIsRunning(Unchecked.defaultof<_>))

    props
    |> Props.tryFind PKey.runnable.stopRequested
    |> Option.iter (fun _ -> view.StopRequested <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.runnable.result
    |> Option.iter (fun _ -> view.Result <- Unchecked.defaultof<_>)

    // Events
    props
    |> Props.tryFind PKey.runnable.isRunningChanging
    |> Option.iter (fun _ -> this.propsEventRegistry.removeHandler (PKey.runnable.isRunningChanging, LibraryUser))

    props
    |> Props.tryFind PKey.runnable.isRunningChanged
    |> Option.iter (fun _ -> this.propsEventRegistry.removeHandler (PKey.runnable.isRunningChanged, LibraryUser))

    props
    |> Props.tryFind PKey.runnable.isModalChanged
    |> Option.iter (fun _ -> this.propsEventRegistry.removeHandler (PKey.runnable.isModalChanged, LibraryUser))

  override _.name = $"Runnable"

  override this.setProps(props: Props) =
    base.setProps props

    let view = (this :> IInternalTerminalElement).view :?> Runnable

    // Properties
    props
    |> Props.tryFind PKey.runnable.isModal
    |> Option.iter (fun v -> view.SetIsModal(v))

    props
    |> Props.tryFind PKey.runnable.isRunning
    |> Option.iter (fun v -> view.SetIsRunning(v))

    props
    |> Props.tryFind PKey.runnable.stopRequested
    |> Option.iter (fun v -> view.StopRequested <- v)

    props
    |> Props.tryFind PKey.runnable.result
    |> Option.iter (fun v -> view.Result <- v)

    // Events
    props
    |> Props.tryFind PKey.runnable.isRunningChanging
    |> Option.iter (fun v -> this.propsEventRegistry.setEventHandler(PKey.runnable.isRunningChanging, LibraryUser, view.IsRunningChanging, v))

    props
    |> Props.tryFind PKey.runnable.isRunningChanged
    |> Option.iter (fun v -> this.propsEventRegistry.setEventHandler(PKey.runnable.isRunningChanged, LibraryUser, view.IsRunningChanged, v))

    props
    |> Props.tryFind PKey.runnable.isModalChanged
    |> Option.iter (fun v -> this.propsEventRegistry.setEventHandler(PKey.runnable.isModalChanged, LibraryUser, view.IsModalChanged, v))


  override this.newView() = new Runnable()


// TreeView<'a when 'a : not struct>
type TreeViewElement<'a when 'a: not struct>(props: Props) =
  inherit TerminalElement(props)

  override this.removeProps(props: Props) =
    base.removeProps props
    let view = (this :> IInternalTerminalElement).view :?> TreeView<'a>
    // Properties
    props
    |> Props.tryFind PKey.treeView<'a>.allowLetterBasedNavigation
    |> Option.iter (fun _ -> view.AllowLetterBasedNavigation <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.treeView<'a>.aspectGetter
    |> Option.iter (fun _ -> view.AspectGetter <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.treeView<'a>.colorGetter
    |> Option.iter (fun _ -> view.ColorGetter <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.treeView<'a>.maxDepth
    |> Option.iter (fun _ -> view.MaxDepth <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.treeView<'a>.multiSelect
    |> Option.iter (fun _ -> view.MultiSelect <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.treeView<'a>.objectActivationButton
    |> Option.iter (fun _ -> view.ObjectActivationButton <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.treeView<'a>.objectActivationKey
    |> Option.iter (fun _ -> view.ObjectActivationKey <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.treeView<'a>.scrollOffsetHorizontal
    |> Option.iter (fun _ -> view.ScrollOffsetHorizontal <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.treeView<'a>.scrollOffsetVertical
    |> Option.iter (fun _ -> view.ScrollOffsetVertical <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.treeView<'a>.selectedObject
    |> Option.iter (fun _ -> view.SelectedObject <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.treeView<'a>.style
    |> Option.iter (fun _ -> view.Style <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.treeView<'a>.treeBuilder
    |> Option.iter (fun _ -> view.TreeBuilder <- Unchecked.defaultof<_>)
    // Events
    props
    |> Props.tryFind PKey.treeView<'a>.drawLine
    |> Option.iter (fun _ -> this.propsEventRegistry.removeHandler (PKey.treeView<'a>.drawLine, LibraryUser))

    props
    |> Props.tryFind PKey.treeView<'a>.objectActivated
    |> Option.iter (fun _ -> this.propsEventRegistry.removeHandler (PKey.treeView<'a>.objectActivated, LibraryUser))

    props
    |> Props.tryFind PKey.treeView<'a>.selectionChanged
    |> Option.iter (fun _ -> this.propsEventRegistry.removeHandler (PKey.treeView<'a>.selectionChanged, LibraryUser))

  override _.name = $"TreeView<'a>"

  override this.setProps(props: Props) =
    base.setProps props

    let view = (this :> IInternalTerminalElement).view :?> TreeView<'a>

    // Properties
    props
    |> Props.tryFind PKey.treeView<'a>.allowLetterBasedNavigation
    |> Option.iter (fun v -> view.AllowLetterBasedNavigation <- v)

    props
    |> Props.tryFind PKey.treeView<'a>.aspectGetter
    |> Option.iter (fun v -> view.AspectGetter <- v)

    props
    |> Props.tryFind PKey.treeView<'a>.colorGetter
    |> Option.iter (fun v -> view.ColorGetter <- v)

    props
    |> Props.tryFind PKey.treeView<'a>.maxDepth
    |> Option.iter (fun v -> view.MaxDepth <- v)

    props
    |> Props.tryFind PKey.treeView<'a>.multiSelect
    |> Option.iter (fun v -> view.MultiSelect <- v)

    props
    |> Props.tryFind PKey.treeView<'a>.objectActivationButton
    |> Option.iter (fun v -> view.ObjectActivationButton <- v |> Option.toNullable)

    props
    |> Props.tryFind PKey.treeView<'a>.objectActivationKey
    |> Option.iter (fun v -> view.ObjectActivationKey <- v)

    props
    |> Props.tryFind PKey.treeView<'a>.scrollOffsetHorizontal
    |> Option.iter (fun v -> view.ScrollOffsetHorizontal <- v)

    props
    |> Props.tryFind PKey.treeView<'a>.scrollOffsetVertical
    |> Option.iter (fun v -> view.ScrollOffsetVertical <- v)

    props
    |> Props.tryFind PKey.treeView<'a>.selectedObject
    |> Option.iter (fun v -> view.SelectedObject <- v)

    props
    |> Props.tryFind PKey.treeView<'a>.style
    |> Option.iter (fun v -> view.Style <- v)

    props
    |> Props.tryFind PKey.treeView<'a>.treeBuilder
    |> Option.iter (fun v -> view.TreeBuilder <- v)
    // Events
    props
    |> Props.tryFind PKey.treeView<'a>.drawLine
    |> Option.iter (fun v -> this.propsEventRegistry.setEventHandler(PKey.treeView<'a>.drawLine, LibraryUser, view.DrawLine, v))

    props
    |> Props.tryFind PKey.treeView<'a>.objectActivated
    |> Option.iter (fun v -> this.propsEventRegistry.setEventHandler(PKey.treeView<'a>.objectActivated, LibraryUser, view.ObjectActivated, v))

    props
    |> Props.tryFind PKey.treeView<'a>.selectionChanged
    |> Option.iter (fun v -> this.propsEventRegistry.setEventHandler(PKey.treeView<'a>.selectionChanged, LibraryUser, view.SelectionChanged, v))

  override this.newView() = new TreeView<'a>()


  interface ITreeViewElement

// TreeView
type TreeViewElement(props: Props) =
  inherit TreeViewElement<ITreeNode>(props)

// Window
type WindowElement(props: Props) =
  inherit TerminalElement(props)

  override this.removeProps(props: Props) = base.removeProps props
  // No properties or events Window

  override _.name = $"Window"

  override this.setProps(props: Props) = base.setProps props
  // No properties or events Window


  override this.newView() = new Window()

// Wizard
type WizardElement(props: Props) =
  inherit TerminalElement(props)

  override this.removeProps(props: Props) =
    base.removeProps props
    let view = (this :> IInternalTerminalElement).view :?> Wizard
    // Properties
    props
    |> Props.tryFind PKey.wizard.currentStep
    |> Option.iter (fun _ -> view.CurrentStep <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.wizard.modal
    |> Option.iter (fun _ -> view.SetIsModal(Unchecked.defaultof<_>))
    // Events
    props
    |> Props.tryFind PKey.wizard.cancelled
    |> Option.iter (fun _ -> this.propsEventRegistry.removeHandler (PKey.wizard.cancelled, LibraryUser))

    props
    |> Props.tryFind PKey.wizard.finished
    |> Option.iter (fun _ -> this.propsEventRegistry.removeHandler (PKey.wizard.finished, LibraryUser))

    props
    |> Props.tryFind PKey.wizard.movingBack
    |> Option.iter (fun _ -> this.propsEventRegistry.removeHandler (PKey.wizard.movingBack, LibraryUser))

    props
    |> Props.tryFind PKey.wizard.movingNext
    |> Option.iter (fun _ -> this.propsEventRegistry.removeHandler (PKey.wizard.movingNext, LibraryUser))

    props
    |> Props.tryFind PKey.wizard.stepChanged
    |> Option.iter (fun _ -> this.propsEventRegistry.removeHandler (PKey.wizard.stepChanged, LibraryUser))

    props
    |> Props.tryFind PKey.wizard.stepChanging
    |> Option.iter (fun _ -> this.propsEventRegistry.removeHandler (PKey.wizard.stepChanging, LibraryUser))

  override _.name = $"Wizard"

  override this.setProps(props: Props) =
    base.setProps props

    let view = (this :> IInternalTerminalElement).view :?> Wizard

    // Properties
    props
    |> Props.tryFind PKey.wizard.currentStep
    |> Option.iter (fun v -> view.CurrentStep <- v)

    props
    |> Props.tryFind PKey.wizard.modal
    |> Option.iter (fun v -> view.SetIsModal(v))
    // Events
    props
    |> Props.tryFind PKey.wizard.cancelled
    |> Option.iter (fun v -> this.propsEventRegistry.setEventHandler(PKey.wizard.cancelled, LibraryUser, view.Cancelled, v))

    props
    |> Props.tryFind PKey.wizard.finished
    |> Option.iter (fun v -> this.propsEventRegistry.setEventHandler(PKey.wizard.finished, LibraryUser, view.Finished, v))

    props
    |> Props.tryFind PKey.wizard.movingBack
    |> Option.iter (fun v -> this.propsEventRegistry.setEventHandler(PKey.wizard.movingBack, LibraryUser, view.MovingBack, v))

    props
    |> Props.tryFind PKey.wizard.movingNext
    |> Option.iter (fun v -> this.propsEventRegistry.setEventHandler(PKey.wizard.movingNext, LibraryUser, view.MovingNext, v))

    props
    |> Props.tryFind PKey.wizard.stepChanged
    |> Option.iter (fun v -> this.propsEventRegistry.setEventHandler(PKey.wizard.stepChanged, LibraryUser, view.StepChanged, v))

    props
    |> Props.tryFind PKey.wizard.stepChanging
    |> Option.iter (fun v -> this.propsEventRegistry.setEventHandler(PKey.wizard.stepChanging, LibraryUser, view.StepChanging, v))


  override this.newView() = new Wizard()

// WizardStep
type WizardStepElement(props: Props) =
  inherit TerminalElement(props)

  override this.removeProps(props: Props) =
    base.removeProps props
    let view = (this :> IInternalTerminalElement).view :?> WizardStep
    // Properties
    props
    |> Props.tryFind PKey.wizardStep.backButtonText
    |> Option.iter (fun _ -> view.BackButtonText <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.wizardStep.helpText
    |> Option.iter (fun _ -> view.HelpText <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.wizardStep.nextButtonText
    |> Option.iter (fun _ -> view.NextButtonText <- Unchecked.defaultof<_>)

  override _.name = $"WizardStep"

  override this.setProps(props: Props) =
    base.setProps props

    let view = (this :> IInternalTerminalElement).view :?> WizardStep

    // Properties
    props
    |> Props.tryFind PKey.wizardStep.backButtonText
    |> Option.iter (fun v -> view.BackButtonText <- v)

    props
    |> Props.tryFind PKey.wizardStep.helpText
    |> Option.iter (fun v -> view.HelpText <- v)

    props
    |> Props.tryFind PKey.wizardStep.nextButtonText
    |> Option.iter (fun v -> view.NextButtonText <- v)


  override this.newView() = new WizardStep()
