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
        match curNode.TerminalElement.isElmishComponent with
        | true -> []
        | false ->
          curNode.TerminalElement.Children
          |> Seq.map (fun e -> {
            TerminalElement = e
            Parent = Some curNode.TerminalElement.View
          })
          |> List.ofSeq

      traverseTree (childNodes @ remainingNodes) traverse

  let mutable reused = false

  let mutable view = null
  let viewSetEvent = Event<View>()

  member this.View
    with get() = view
    and set value =
      if (view <> null) then
        failwith $"View has already been set."
      view <- value
      viewSetEvent.Trigger value

  member val ViewSet = viewSetEvent.Publish

  member val EventRegistry: PropsEventRegistry = PropsEventRegistry() with get, set

  member val Props: Props = props with get, set

  member this.children
    with get() : List<IInternalTerminalElement> =
      props
      |> Props.tryFind PKey.view.children
      |> Option.defaultValue (List<IInternalTerminalElement>())
    and set value =
      match props.tryFind PKey.view.children with
      | Some _ -> failwith "Children property has already been set."
      | None -> props.add(PKey.view.children, value)

  abstract SubElements_PropKeys: SubElementPropKey<IInternalTerminalElement> list
  default _.SubElements_PropKeys = []

  abstract newView: unit -> View

  abstract setAsChildOfParentView: bool
  default _.setAsChildOfParentView = true

  member this.SignalReuse() =
    PositionService.Current.SignalReuse this
    reused <- true

  member this.initialize() =
#if DEBUG
    Diagnostics.Trace.WriteLine $"{this.name} created!"
#endif

    let newView = this.newView ()

    this.initializeSubElements newView
    |> Seq.iter this.Props.addNonTyped

    this.View <- newView
    this.setProps (this, this.Props)

  abstract reuse: prev: IInternalTerminalElement -> unit

  abstract name: string

  member this.initializeTree(parent: View option) : unit =
    let traverse (node: TreeNode) =

      node.TerminalElement.initialize ()

      // Here, the "children" view are added to their parent
      if node.TerminalElement.setAsChildOfParentView then
        node.Parent
        |> Option.iter (fun p -> p.Add node.TerminalElement.View |> ignore)

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
        match this.Props |> Props.tryFindByRawKey<obj> x with

        | None -> ()

        | Some value ->
          match value with
          | :? TerminalElement as subElement ->
            subElement.initializeTree (Some parent)

            let viewKey = x.viewKey

            yield viewKey, subElement.View
          | :? List<IInternalTerminalElement> as elements ->
            elements
            |> Seq.iter (fun e -> e.initializeTree (Some parent))

            let viewKey = x.viewKey

            let views =
              elements |> Seq.map _.View |> Seq.toList

            yield viewKey, views
          | _ -> failwith "Out of range subElement type"
    }

  member this.trySetEventHandler<'TEventArgs> (k: IEventPropKey<'TEventArgs -> unit>, event: IEvent<EventHandler<'TEventArgs>,'TEventArgs>) =

    this.tryRemoveEventHandler k

    this.Props.tryFind k
    |> Option.iter (fun action -> this.EventRegistry.setEventHandler(k, event, action))

  member this.trySetEventHandler (k: IEventPropKey<EventArgs -> unit>, event: IEvent<EventHandler,EventArgs>) =

    this.tryRemoveEventHandler k

    this.Props.tryFind k
    |> Option.iter (fun action -> this.EventRegistry.setEventHandler(k, event, action))

  member this.trySetEventHandler (k: IEventPropKey<NotifyCollectionChangedEventArgs -> unit>, event: IEvent<NotifyCollectionChangedEventHandler,NotifyCollectionChangedEventArgs>) =

    this.tryRemoveEventHandler k

    this.Props.tryFind k
    |> Option.iter (fun action -> this.EventRegistry.setEventHandler(k, event, action))

  member this.tryRemoveEventHandler (k: IPropKey) =
    this.EventRegistry.removeHandler k

  abstract setProps: terminalElement: IInternalTerminalElement * props: Props -> unit

  default _.setProps(terminalElement: IInternalTerminalElement, props: Props) =

    let terminalElement = terminalElement :?> TerminalElement

    // Properties
    props
    |> Props.tryFind PKey.view.arrangement
    |> Option.iter (fun v -> terminalElement.View.Arrangement <- v)

    props
    |> Props.tryFind PKey.view.borderStyle
    |> Option.iter (fun v -> terminalElement.View.BorderStyle <- v)

    props
    |> Props.tryFind PKey.view.canFocus
    |> Option.iter (fun v -> terminalElement.View.CanFocus <- v)

    props
    |> Props.tryFind PKey.view.contentSizeTracksViewport
    |> Option.iter (fun v -> terminalElement.View.ContentSizeTracksViewport <- v)

    props
    |> Props.tryFind PKey.view.cursorVisibility
    |> Option.iter (fun v -> terminalElement.View.CursorVisibility <- v)

    props
    |> Props.tryFind PKey.view.data
    |> Option.iter (fun v -> terminalElement.View.Data <- v)

    props
    |> Props.tryFind PKey.view.enabled
    |> Option.iter (fun v -> terminalElement.View.Enabled <- v)

    props
    |> Props.tryFind PKey.view.frame
    |> Option.iter (fun v -> terminalElement.View.Frame <- v)

    props
    |> Props.tryFind PKey.view.hasFocus
    |> Option.iter (fun v -> terminalElement.View.HasFocus <- v)

    props
    |> Props.tryFind PKey.view.height
    |> Option.iter (fun v -> terminalElement.View.Height <- v)

    props
    |> Props.tryFind PKey.view.highlightStates
    |> Option.iter (fun v -> terminalElement.View.HighlightStates <- v)

    props
    |> Props.tryFind PKey.view.hotKey
    |> Option.iter (fun v -> terminalElement.View.HotKey <- v)

    props
    |> Props.tryFind PKey.view.hotKeySpecifier
    |> Option.iter (fun v -> terminalElement.View.HotKeySpecifier <- v)

    props
    |> Props.tryFind PKey.view.id
    |> Option.iter (fun v -> terminalElement.View.Id <- v)

    props
    |> Props.tryFind PKey.view.isInitialized
    |> Option.iter (fun v -> terminalElement.View.IsInitialized <- v)

    props
    |> Props.tryFind PKey.view.mouseHeldDown
    |> Option.iter (fun v -> terminalElement.View.MouseHeldDown <- v)

    props
    |> Props.tryFind PKey.view.preserveTrailingSpaces
    |> Option.iter (fun v -> terminalElement.View.PreserveTrailingSpaces <- v)

    props
    |> Props.tryFind PKey.view.schemeName
    |> Option.iter (fun v -> terminalElement.View.SchemeName <- v)

    props
    |> Props.tryFind PKey.view.shadowStyle
    |> Option.iter (fun v -> terminalElement.View.ShadowStyle <- v)

    props
    |> Props.tryFind PKey.view.superViewRendersLineCanvas
    |> Option.iter (fun v -> terminalElement.View.SuperViewRendersLineCanvas <- v)

    props
    |> Props.tryFind PKey.view.tabStop
    |> Option.iter (fun v -> terminalElement.View.TabStop <- v |> Option.toNullable)

    props
    |> Props.tryFind PKey.view.text
    |> Option.iter (fun v -> terminalElement.View.Text <- v)

    props
    |> Props.tryFind PKey.view.textAlignment
    |> Option.iter (fun v -> terminalElement.View.TextAlignment <- v)

    props
    |> Props.tryFind PKey.view.textDirection
    |> Option.iter (fun v -> terminalElement.View.TextDirection <- v)

    props
    |> Props.tryFind PKey.view.title
    |> Option.iter (fun v -> terminalElement.View.Title <- v)

    props
    |> Props.tryFind PKey.view.validatePosDim
    |> Option.iter (fun v -> terminalElement.View.ValidatePosDim <- v)

    props
    |> Props.tryFind PKey.view.verticalTextAlignment
    |> Option.iter (fun v -> terminalElement.View.VerticalTextAlignment <- v)

    props
    |> Props.tryFind PKey.view.viewport
    |> Option.iter (fun v -> terminalElement.View.Viewport <- v)

    props
    |> Props.tryFind PKey.view.viewportSettings
    |> Option.iter (fun v -> terminalElement.View.ViewportSettings <- v)

    props
    |> Props.tryFind PKey.view.visible
    |> Option.iter (fun v -> terminalElement.View.Visible <- v)

    props
    |> Props.tryFind PKey.view.wantContinuousButtonPressed
    |> Option.iter (fun v -> terminalElement.View.WantContinuousButtonPressed <- v)

    props
    |> Props.tryFind PKey.view.wantMousePositionReports
    |> Option.iter (fun v -> terminalElement.View.WantMousePositionReports <- v)

    props
    |> Props.tryFind PKey.view.width
    |> Option.iter (fun v -> terminalElement.View.Width <- v)

    props
    |> Props.tryFind PKey.view.x
    |> Option.iter (fun v -> terminalElement.View.X <- v)

    props
    |> Props.tryFind PKey.view.y
    |> Option.iter (fun v -> terminalElement.View.Y <- v)

    // Events
    terminalElement.trySetEventHandler(PKey.view.accepting, terminalElement.View.Accepting)

    terminalElement.trySetEventHandler(PKey.view.activating, terminalElement.View.Activating)

    terminalElement.trySetEventHandler(PKey.view.advancingFocus, terminalElement.View.AdvancingFocus)

    terminalElement.trySetEventHandler(PKey.view.borderStyleChanged, terminalElement.View.BorderStyleChanged)

    terminalElement.trySetEventHandler(PKey.view.canFocusChanged, terminalElement.View.CanFocusChanged)

    terminalElement.trySetEventHandler(PKey.view.clearedViewport, terminalElement.View.ClearedViewport)

    terminalElement.trySetEventHandler(PKey.view.clearingViewport, terminalElement.View.ClearingViewport)

    terminalElement.trySetEventHandler(PKey.view.commandNotBound, terminalElement.View.CommandNotBound)

    terminalElement.trySetEventHandler(PKey.view.contentSizeChanged, terminalElement.View.ContentSizeChanged)

    terminalElement.trySetEventHandler(PKey.view.disposing, terminalElement.View.Disposing)

    terminalElement.trySetEventHandler(PKey.view.drawComplete, terminalElement.View.DrawComplete)

    terminalElement.trySetEventHandler(PKey.view.drawingContent, terminalElement.View.DrawingContent)

    terminalElement.trySetEventHandler(PKey.view.drawingSubViews, terminalElement.View.DrawingSubViews)

    terminalElement.trySetEventHandler(PKey.view.drawingText, terminalElement.View.DrawingText)

    terminalElement.trySetEventHandler(PKey.view.enabledChanged, terminalElement.View.EnabledChanged)

    terminalElement.trySetEventHandler(PKey.view.focusedChanged, terminalElement.View.FocusedChanged)

    terminalElement.trySetEventHandler(PKey.view.frameChanged, terminalElement.View.FrameChanged)

    terminalElement.trySetEventHandler(PKey.view.gettingAttributeForRole, terminalElement.View.GettingAttributeForRole)

    terminalElement.trySetEventHandler(PKey.view.gettingScheme, terminalElement.View.GettingScheme)

    terminalElement.trySetEventHandler(PKey.view.handlingHotKey, terminalElement.View.HandlingHotKey)

    terminalElement.trySetEventHandler(PKey.view.hasFocusChanged, terminalElement.View.HasFocusChanged)

    terminalElement.trySetEventHandler(PKey.view.hasFocusChanging, terminalElement.View.HasFocusChanging)

    terminalElement.trySetEventHandler(PKey.view.hotKeyChanged, terminalElement.View.HotKeyChanged)

    terminalElement.trySetEventHandler(PKey.view.initialized, terminalElement.View.Initialized)

    terminalElement.trySetEventHandler(PKey.view.keyDown, terminalElement.View.KeyDown)

    terminalElement.trySetEventHandler(PKey.view.keyDownNotHandled, terminalElement.View.KeyDownNotHandled)

    terminalElement.trySetEventHandler(PKey.view.keyUp, terminalElement.View.KeyUp)

    terminalElement.trySetEventHandler(PKey.view.mouseEnter, terminalElement.View.MouseEnter)

    terminalElement.trySetEventHandler(PKey.view.mouseEvent, terminalElement.View.MouseEvent)

    terminalElement.trySetEventHandler(PKey.view.mouseLeave, terminalElement.View.MouseLeave)

    terminalElement.trySetEventHandler(PKey.view.mouseStateChanged, terminalElement.View.MouseStateChanged)

    terminalElement.trySetEventHandler(PKey.view.mouseWheel, terminalElement.View.MouseWheel)

    terminalElement.trySetEventHandler(PKey.view.removed, terminalElement.View.Removed)

    terminalElement.trySetEventHandler(PKey.view.schemeChanged, terminalElement.View.SchemeChanged)

    terminalElement.trySetEventHandler(PKey.view.schemeChanging, terminalElement.View.SchemeChanging)

    terminalElement.trySetEventHandler(PKey.view.schemeNameChanged, terminalElement.View.SchemeNameChanged)

    terminalElement.trySetEventHandler(PKey.view.schemeNameChanging, terminalElement.View.SchemeNameChanging)

    terminalElement.trySetEventHandler(PKey.view.subViewAdded, terminalElement.View.SubViewAdded)

    terminalElement.trySetEventHandler(PKey.view.subViewLayout, terminalElement.View.SubViewLayout)

    terminalElement.trySetEventHandler(PKey.view.subViewRemoved, terminalElement.View.SubViewRemoved)

    terminalElement.trySetEventHandler(PKey.view.subViewsLaidOut, terminalElement.View.SubViewsLaidOut)

    terminalElement.trySetEventHandler(PKey.view.superViewChanged, terminalElement.View.SuperViewChanged)

    terminalElement.trySetEventHandler(PKey.view.textChanged, terminalElement.View.TextChanged)

    terminalElement.trySetEventHandler(PKey.view.titleChanged, terminalElement.View.TitleChanged)

    terminalElement.trySetEventHandler(PKey.view.titleChanging, terminalElement.View.TitleChanging)

    terminalElement.trySetEventHandler(PKey.view.viewportChanged, terminalElement.View.ViewportChanged)

    terminalElement.trySetEventHandler(PKey.view.visibleChanged, terminalElement.View.VisibleChanged)

    terminalElement.trySetEventHandler(PKey.view.visibleChanging, terminalElement.View.VisibleChanging)

    // Custom Props
    props
    |> Props.tryFind PKey.view.x_delayedPos
    // TODO: too confusing here, too difficult to reason about, need to refactor
    |> Option.iter (fun tPos -> PositionService.Current.ApplyPos(terminalElement, tPos, (fun view pos -> view.X <- pos)))

    props
    |> Props.tryFind PKey.view.y_delayedPos
    |> Option.iter (fun tPos -> PositionService.Current.ApplyPos(terminalElement, tPos, (fun view pos -> view.Y <- pos)))

  // TODO: Is the view needed as param ? is the props needed as param ?
  abstract removeProps: terminalElement: IInternalTerminalElement * props: Props -> unit

  default _.removeProps(terminalElement: IInternalTerminalElement, props: Props) =

    let terminalElement = terminalElement :?> TerminalElement

    // Properties
    props
    |> Props.tryFind PKey.view.arrangement
    |> Option.iter (fun _ -> terminalElement.View.Arrangement <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.view.borderStyle
    |> Option.iter (fun _ -> terminalElement.View.BorderStyle <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.view.canFocus
    |> Option.iter (fun _ -> terminalElement.View.CanFocus <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.view.contentSizeTracksViewport
    |> Option.iter (fun _ -> terminalElement.View.ContentSizeTracksViewport <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.view.cursorVisibility
    |> Option.iter (fun _ -> terminalElement.View.CursorVisibility <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.view.data
    |> Option.iter (fun _ -> terminalElement.View.Data <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.view.enabled
    |> Option.iter (fun _ -> terminalElement.View.Enabled <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.view.frame
    |> Option.iter (fun _ -> terminalElement.View.Frame <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.view.hasFocus
    |> Option.iter (fun _ -> terminalElement.View.HasFocus <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.view.height
    |> Option.iter (fun _ -> terminalElement.View.Height <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.view.highlightStates
    |> Option.iter (fun _ -> terminalElement.View.HighlightStates <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.view.hotKey
    |> Option.iter (fun _ -> terminalElement.View.HotKey <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.view.hotKeySpecifier
    |> Option.iter (fun _ -> terminalElement.View.HotKeySpecifier <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.view.id
    |> Option.iter (fun _ -> terminalElement.View.Id <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.view.isInitialized
    |> Option.iter (fun _ -> terminalElement.View.IsInitialized <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.view.mouseHeldDown
    |> Option.iter (fun _ -> terminalElement.View.MouseHeldDown <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.view.preserveTrailingSpaces
    |> Option.iter (fun _ -> terminalElement.View.PreserveTrailingSpaces <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.view.schemeName
    |> Option.iter (fun _ -> terminalElement.View.SchemeName <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.view.shadowStyle
    |> Option.iter (fun _ -> terminalElement.View.ShadowStyle <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.view.superViewRendersLineCanvas
    |> Option.iter (fun _ -> terminalElement.View.SuperViewRendersLineCanvas <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.view.tabStop
    |> Option.iter (fun _ -> terminalElement.View.TabStop <- Unchecked.defaultof<_> |> Option.toNullable)

    props
    |> Props.tryFind PKey.view.text
    |> Option.iter (fun _ -> terminalElement.View.Text <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.view.textAlignment
    |> Option.iter (fun _ -> terminalElement.View.TextAlignment <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.view.textDirection
    |> Option.iter (fun _ -> terminalElement.View.TextDirection <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.view.title
    |> Option.iter (fun _ -> terminalElement.View.Title <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.view.validatePosDim
    |> Option.iter (fun _ -> terminalElement.View.ValidatePosDim <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.view.verticalTextAlignment
    |> Option.iter (fun _ -> terminalElement.View.VerticalTextAlignment <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.view.viewport
    |> Option.iter (fun _ -> terminalElement.View.Viewport <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.view.viewportSettings
    |> Option.iter (fun _ -> terminalElement.View.ViewportSettings <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.view.visible
    |> Option.iter (fun _ -> terminalElement.View.Visible <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.view.wantContinuousButtonPressed
    |> Option.iter (fun _ -> terminalElement.View.WantContinuousButtonPressed <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.view.wantMousePositionReports
    |> Option.iter (fun _ -> terminalElement.View.WantMousePositionReports <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.view.width
    |> Option.iter (fun _ -> terminalElement.View.Width <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.view.x
    |> Option.iter (fun _ -> terminalElement.View.X <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.view.y
    |> Option.iter (fun _ -> terminalElement.View.Y <- Unchecked.defaultof<_>)

    // Events
    terminalElement.tryRemoveEventHandler PKey.view.accepting

    terminalElement.tryRemoveEventHandler PKey.view.activating

    terminalElement.tryRemoveEventHandler PKey.view.advancingFocus

    terminalElement.tryRemoveEventHandler PKey.view.borderStyleChanged

    terminalElement.tryRemoveEventHandler PKey.view.canFocusChanged

    terminalElement.tryRemoveEventHandler PKey.view.clearedViewport

    terminalElement.tryRemoveEventHandler PKey.view.clearingViewport

    terminalElement.tryRemoveEventHandler PKey.view.commandNotBound

    terminalElement.tryRemoveEventHandler PKey.view.contentSizeChanged

    terminalElement.tryRemoveEventHandler PKey.view.disposing

    terminalElement.tryRemoveEventHandler PKey.view.drawComplete

    terminalElement.tryRemoveEventHandler PKey.view.drawingContent

    terminalElement.tryRemoveEventHandler PKey.view.drawingSubViews

    terminalElement.tryRemoveEventHandler PKey.view.drawingText

    terminalElement.tryRemoveEventHandler PKey.view.enabledChanged

    terminalElement.tryRemoveEventHandler PKey.view.focusedChanged

    terminalElement.tryRemoveEventHandler PKey.view.frameChanged

    terminalElement.tryRemoveEventHandler PKey.view.gettingAttributeForRole

    terminalElement.tryRemoveEventHandler PKey.view.gettingScheme

    terminalElement.tryRemoveEventHandler PKey.view.handlingHotKey

    terminalElement.tryRemoveEventHandler PKey.view.hasFocusChanged

    terminalElement.tryRemoveEventHandler PKey.view.hasFocusChanging

    terminalElement.tryRemoveEventHandler PKey.view.hotKeyChanged

    terminalElement.tryRemoveEventHandler PKey.view.initialized

    terminalElement.tryRemoveEventHandler PKey.view.keyDown

    terminalElement.tryRemoveEventHandler PKey.view.keyDownNotHandled

    terminalElement.tryRemoveEventHandler PKey.view.keyUp

    terminalElement.tryRemoveEventHandler PKey.view.mouseEnter

    terminalElement.tryRemoveEventHandler PKey.view.mouseEvent

    terminalElement.tryRemoveEventHandler PKey.view.mouseLeave

    terminalElement.tryRemoveEventHandler PKey.view.mouseStateChanged

    terminalElement.tryRemoveEventHandler PKey.view.mouseWheel

    terminalElement.tryRemoveEventHandler PKey.view.removed

    terminalElement.tryRemoveEventHandler PKey.view.schemeChanged

    terminalElement.tryRemoveEventHandler PKey.view.schemeChanging

    terminalElement.tryRemoveEventHandler PKey.view.schemeNameChanged

    terminalElement.tryRemoveEventHandler PKey.view.schemeNameChanging

    terminalElement.tryRemoveEventHandler PKey.view.subViewAdded

    terminalElement.tryRemoveEventHandler PKey.view.subViewLayout

    terminalElement.tryRemoveEventHandler PKey.view.subViewRemoved

    terminalElement.tryRemoveEventHandler PKey.view.subViewsLaidOut

    terminalElement.tryRemoveEventHandler PKey.view.superViewChanged

    terminalElement.tryRemoveEventHandler PKey.view.textChanged

    terminalElement.tryRemoveEventHandler PKey.view.titleChanged

    terminalElement.tryRemoveEventHandler PKey.view.titleChanging

    terminalElement.tryRemoveEventHandler PKey.view.viewportChanged

    terminalElement.tryRemoveEventHandler PKey.view.visibleChanged

    terminalElement.tryRemoveEventHandler PKey.view.visibleChanging

    // Custom Props
    props
    |> Props.tryFind PKey.view.x_delayedPos
    |> Option.iter (fun _ -> terminalElement.View.X <- Pos.Absolute(0))

    props
    |> Props.tryFind PKey.view.y_delayedPos
    |> Option.iter (fun _ -> terminalElement.View.Y <- Pos.Absolute(0))

  /// Reuses:
  /// // TODO: outdated documentation
  /// - Previous `View`, while updating its properties to match the current TerminalElement properties.
  /// - But also other Views that are sub elements of the previous `ITerminalElement` and made available in the `prevProps`.
  override this.reuse prev =

    let prev = prev :?> TerminalElement

    prev.SignalReuse()

    // TODO: it seems that comparing x_delayedPos/y_delayedPos is working well
    // TODO: this should be tested and documented to make sure that it continues to work well in the future.

    this.View <- prev.View
    this.EventRegistry <- prev.EventRegistry

    let c = TerminalElement.compare prev.Props this.Props

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
    |> Props.iter (fun kv -> this.Props.addNonTyped (kv.Key, kv.Value))

    this.removeProps (this, removedProps)
    this.setProps (this, c.changedProps)

  member this.equivalentTo(other: TerminalElement) =
    let mutable isEquivalent = true

    let mutable enumerator =
      this.Props.dict.GetEnumerator()

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
          other.Props
          |> Props.tryFindByRawKey kv.Key
          |> Option.map (fun (x: obj) -> x :?> TerminalElement)

        match curElement, otherElement with
        | curValue, Some otherValue when (curValue.equivalentTo otherValue) -> ()
        | _, _ -> isEquivalent <- false
      else
        let curElement = kv.Value

        let otherElement =
          other.Props |> Props.tryFindByRawKey kv.Key

        isEquivalent <- curElement = otherElement

    isEquivalent

  static member compare
    (prevProps: Props)
    (curProps: Props)
    : {|
        changedProps: Props
        unchangedProps: Props
        removedProps: Props
      |}
    =

    let remainingOldProps, removedProps =
      prevProps
      |> Props.partition (fun kv -> curProps |> Props.rawKeyExists kv.Key)

    let unchangedProps, changedProps =
      curProps
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

  member this.Dispose() =
    if (not reused) then

      // Remove any event subscriptions
      this.removeProps (this, this.Props)

      this.View |> Interop.removeFromParent
      // Dispose SubElements (Represented as `View` typed properties of the View, that are not children)
      for key in this.SubElements_PropKeys do
        this.Props
        |> Props.tryFind key
        |> Option.iter (fun subElement ->
          subElement.Dispose())

      for child in this.children do
        child.Dispose()

      PositionService.Current.SignalDispose(this)
      // Finally, dispose the View itself
      this.View.Dispose()

  interface IInternalTerminalElement with
    member this.initialize() = this.initialize()
    member this.initializeTree(parent) = this.initializeTree parent
    member this.reuse prevElementData = this.reuse prevElementData
    member this.View = this.View
    member this.name = this.name

    member this.setAsChildOfParentView =
      this.setAsChildOfParentView

    member this.Children = this.children

    member this.isElmishComponent = false

    member this.Props = this.Props

    member this.ViewSet = this.ViewSet

    member this.Dispose() = this.Dispose()


// OrientationInterface - used by elements that implement Terminal.Gui.ViewBase.IOrientation
type OrientationInterface =
  static member removeProps (terminalElement: TerminalElement) (view: IOrientation) (props: Props) =
    // Properties
    props
    |> Props.tryFind PKey.orientationInterface.orientation
    |> Option.iter (fun _ -> view.Orientation <- Unchecked.defaultof<_>)

    // Events
    terminalElement.tryRemoveEventHandler PKey.orientationInterface.orientationChanged

    terminalElement.tryRemoveEventHandler PKey.orientationInterface.orientationChanging

  static member setProps (terminalElement: TerminalElement) (view: IOrientation) (props: Props) =
    // Properties
    props
    |> Props.tryFind PKey.orientationInterface.orientation
    |> Option.iter (fun v -> view.Orientation <- v)

    // Events
    terminalElement.trySetEventHandler(PKey.orientationInterface.orientationChanged, view.OrientationChanged)

    terminalElement.trySetEventHandler(PKey.orientationInterface.orientationChanging, view.OrientationChanging)

// Adornment
type AdornmentElement(props: Props) =
  inherit TerminalElement(props)

  override _.removeProps(terminalElement: IInternalTerminalElement, props: Props) =

    let terminalElement = terminalElement :?> TerminalElement

    base.removeProps (terminalElement, props)
    let view = terminalElement.View :?> Adornment
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
    terminalElement.tryRemoveEventHandler PKey.adornment.thicknessChanged

  override _.name = $"Adornment"

  override _.setProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.setProps (terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> Adornment

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
    terminalElement.trySetEventHandler(PKey.adornment.thicknessChanged, view.ThicknessChanged)

  override this.newView() = new Adornment()


// Bar
type BarElement(props: Props) =
  inherit TerminalElement(props)

  override _.removeProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.removeProps (terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> Bar

    // Interfaces
    OrientationInterface.removeProps terminalElement view props

    // Properties
    props
    |> Props.tryFind PKey.bar.alignmentModes
    |> Option.iter (fun _ -> view.AlignmentModes <- Unchecked.defaultof<_>)

  override _.name = $"Bar"

  override _.setProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.setProps (terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> Bar

    // Interfaces
    OrientationInterface.setProps terminalElement view props

    // Properties
    props
    |> Props.tryFind PKey.bar.alignmentModes
    |> Option.iter (fun v -> view.AlignmentModes <- v)


  override this.newView() = new Bar()

// Border
type BorderElement(props: Props) =
  inherit TerminalElement(props)

  override _.removeProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.removeProps (terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> Border
    // Properties
    props
    |> Props.tryFind PKey.border.lineStyle
    |> Option.iter (fun _ -> view.LineStyle <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.border.settings
    |> Option.iter (fun _ -> view.Settings <- Unchecked.defaultof<_>)

  override _.name = $"Border"

  override _.setProps(elementData: IInternalTerminalElement, props: Props) =
    base.setProps (elementData, props)

    let view = elementData.View :?> Border

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

  override _.removeProps(elementData: IInternalTerminalElement, props: Props) =
    base.removeProps (elementData, props)
    let view = elementData.View :?> Button
    // Properties
    props
    |> Props.tryFind PKey.button.hotKeySpecifier
    |> Option.iter (fun _ -> elementData.View.HotKeySpecifier <- Unchecked.defaultof<_>)

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

  override _.setProps(elementData: IInternalTerminalElement, props: Props) =
    base.setProps (elementData, props)

    let view = elementData.View :?> Button

    // Properties
    props
    |> Props.tryFind PKey.button.hotKeySpecifier
    |> Option.iter (fun v -> elementData.View.HotKeySpecifier <- v)

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

  override _.removeProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.removeProps (terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> CheckBox

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
    terminalElement.tryRemoveEventHandler PKey.checkBox.checkedStateChanging

    terminalElement.tryRemoveEventHandler PKey.checkBox.checkedStateChanged

  override _.name = $"CheckBox"

  override _.setProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.setProps (terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> CheckBox

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
    terminalElement.trySetEventHandler(PKey.checkBox.checkedStateChanging, view.CheckedStateChanging)

    terminalElement.trySetEventHandler(PKey.checkBox.checkedStateChanged, view.CheckedStateChanged)


  override this.newView() = new CheckBox()

// ColorPicker
type ColorPickerElement(props: Props) =
  inherit TerminalElement(props)

  override _.removeProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.removeProps (terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> ColorPicker

    // Properties
    props
    |> Props.tryFind PKey.colorPicker.selectedColor
    |> Option.iter (fun _ -> view.SelectedColor <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.colorPicker.style
    |> Option.iter (fun _ -> view.Style <- Unchecked.defaultof<_>)
    // Events
    terminalElement.tryRemoveEventHandler PKey.colorPicker.colorChanged

  override _.name = $"ColorPicker"

  override _.setProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.setProps (terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> ColorPicker

    // Properties
    props
    |> Props.tryFind PKey.colorPicker.selectedColor
    |> Option.iter (fun v -> view.SelectedColor <- v)

    props
    |> Props.tryFind PKey.colorPicker.style
    |> Option.iter (fun v -> view.Style <- v)
    // Events
    terminalElement.trySetEventHandler(PKey.colorPicker.colorChanged, view.ColorChanged)


  override this.newView() = new ColorPicker()

// ColorPicker16
type ColorPicker16Element(props: Props) =
  inherit TerminalElement(props)

  override _.removeProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.removeProps (terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> ColorPicker16

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
    terminalElement.tryRemoveEventHandler PKey.colorPicker16.colorChanged

  override _.name = $"ColorPicker16"

  override _.setProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.setProps (terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> ColorPicker16

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
    terminalElement.trySetEventHandler(PKey.colorPicker16.colorChanged, view.ColorChanged)


  override this.newView() = new ColorPicker16()

// ComboBox
type ComboBoxElement(props: Props) =
  inherit TerminalElement(props)

  override _.removeProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.removeProps (terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> ComboBox

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
    terminalElement.tryRemoveEventHandler PKey.comboBox.collapsed

    terminalElement.tryRemoveEventHandler PKey.comboBox.expanded

    terminalElement.tryRemoveEventHandler PKey.comboBox.openSelectedItem

    terminalElement.tryRemoveEventHandler PKey.comboBox.selectedItemChanged

  override _.name = $"ComboBox"

  override _.setProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.setProps (terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> ComboBox

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
    |> Option.iter (fun v -> terminalElement.View.Text <- v)
    // Events
    terminalElement.trySetEventHandler(PKey.comboBox.collapsed, view.Collapsed)

    terminalElement.trySetEventHandler(PKey.comboBox.expanded, view.Expanded)

    terminalElement.trySetEventHandler(PKey.comboBox.openSelectedItem, view.OpenSelectedItem)

    terminalElement.trySetEventHandler(PKey.comboBox.selectedItemChanged, view.SelectedItemChanged)


  override this.newView() = new ComboBox()

// DateField
type DateFieldElement(props: Props) =
  inherit TerminalElement(props)

  override _.removeProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.removeProps (terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> DateField

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
    terminalElement.tryRemoveEventHandler PKey.dateField.dateChanged

  override _.name = $"DateField"

  override _.setProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.setProps (terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> DateField

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
    terminalElement.trySetEventHandler(PKey.dateField.dateChanged, view.DateChanged)


  override this.newView() = new DateField()

// DatePicker
type DatePickerElement(props: Props) =
  inherit TerminalElement(props)

  override _.removeProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.removeProps (terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> DatePicker

    // Properties
    props
    |> Props.tryFind PKey.datePicker.culture
    |> Option.iter (fun _ -> view.Culture <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.datePicker.date
    |> Option.iter (fun _ -> view.Date <- Unchecked.defaultof<_>)

  override _.name = $"DatePicker"

  override _.setProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.setProps (terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> DatePicker

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

  override _.removeProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.removeProps (terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> Dialog

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

  override _.setProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.setProps (terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> Dialog

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

  override _.removeProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.removeProps (terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> FileDialog

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
    terminalElement.tryRemoveEventHandler PKey.fileDialog.filesSelected

  override _.name = $"FileDialog"

  override _.setProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.setProps (terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> FileDialog

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
    terminalElement.trySetEventHandler(PKey.fileDialog.filesSelected, view.FilesSelected)


  override this.newView() = new FileDialog()

// FrameView
type FrameViewElement(props: Props) =
  inherit TerminalElement(props)

  override _.removeProps(terminalElement: IInternalTerminalElement, props: Props) = base.removeProps (terminalElement, props)
  // No properties or events FrameView

  override _.name = $"FrameView"

  override _.setProps(terminalElement: IInternalTerminalElement, props: Props) = base.setProps (terminalElement, props)
  // No properties or events FrameView


  override this.newView() = new FrameView()

// GraphView
type GraphViewElement(props: Props) =
  inherit TerminalElement(props)

  override _.removeProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.removeProps (terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> GraphView

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

  override _.setProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.setProps (terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> GraphView

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

  override _.removeProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.removeProps (terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> HexView

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
    terminalElement.tryRemoveEventHandler PKey.hexView.edited

    terminalElement.tryRemoveEventHandler PKey.hexView.positionChanged

  override _.name = $"HexView"

  override _.setProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.setProps (terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> HexView

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
    terminalElement.trySetEventHandler(PKey.hexView.edited, view.Edited)

    terminalElement.trySetEventHandler(PKey.hexView.positionChanged, view.PositionChanged)


  override this.newView() = new HexView()

// Label
type LabelElement(props: Props) =
  inherit TerminalElement(props)

  override _.removeProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.removeProps (terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> Label

    // Properties
    props
    |> Props.tryFind PKey.label.hotKeySpecifier
    |> Option.iter (fun _ -> view.HotKeySpecifier <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.label.text
    |> Option.iter (fun _ -> view.Text <- Unchecked.defaultof<_>)

  override _.name = $"Label"

  override _.setProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.setProps (terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> Label

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

  override _.removeProps(terminalElement: IInternalTerminalElement, props: Props) = base.removeProps (terminalElement, props)
  // No properties or events LegendAnnotation

  override _.name = $"LegendAnnotation"

  override _.setProps(terminalElement: IInternalTerminalElement, props: Props) = base.setProps (terminalElement, props)
  // No properties or events LegendAnnotation


  override this.newView() = new LegendAnnotation()

// Line
type LineElement(props: Props) =
  inherit TerminalElement(props)

  override _.removeProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.removeProps (terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> Line

    // Interfaces
    OrientationInterface.removeProps terminalElement view props

  override _.name = $"Line"

  override _.setProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.setProps (terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> Line

    // Interfaces
    OrientationInterface.setProps terminalElement view props


  override this.newView() = new Line()


// ListView
type ListViewElement(props: Props) =
  inherit TerminalElement(props)

  override _.removeProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.removeProps (terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> ListView

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
    terminalElement.tryRemoveEventHandler PKey.listView.collectionChanged

    terminalElement.tryRemoveEventHandler PKey.listView.openSelectedItem

    terminalElement.tryRemoveEventHandler PKey.listView.rowRender

    terminalElement.tryRemoveEventHandler PKey.listView.selectedItemChanged

  override _.name = $"ListView"

  override _.setProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.setProps (terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> ListView

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
    terminalElement.trySetEventHandler(PKey.listView.collectionChanged, view.CollectionChanged)

    terminalElement.trySetEventHandler(PKey.listView.openSelectedItem, view.OpenSelectedItem)

    terminalElement.trySetEventHandler(PKey.listView.rowRender, view.RowRender)

    terminalElement.trySetEventHandler(PKey.listView.selectedItemChanged, view.SelectedItemChanged)


  override this.newView() = new ListView()

// Margin
type MarginElement(props: Props) =
  inherit TerminalElement(props)

  override _.removeProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.removeProps (terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> Margin

    // Properties
    props
    |> Props.tryFind PKey.margin.shadowStyle
    |> Option.iter (fun _ -> view.ShadowStyle <- Unchecked.defaultof<_>)

  override _.name = $"Margin"

  override _.setProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.setProps (terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> Margin

    // Properties
    props
    |> Props.tryFind PKey.margin.shadowStyle
    |> Option.iter (fun v -> view.ShadowStyle <- v)


  override this.newView() = new Margin()

// Menu
type MenuElement(props: Props) =
  inherit TerminalElement(props)

  override _.removeProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.removeProps (terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> Menu

    // Properties
    props
    |> Props.tryFind PKey.menu.selectedMenuItem
    |> Option.iter (fun _ -> view.SelectedMenuItem <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.menu.superMenuItem
    |> Option.iter (fun _ -> view.SuperMenuItem <- Unchecked.defaultof<_>)
    // Events
    terminalElement.trySetEventHandler(PKey.menu.accepted, view.Accepted)

    terminalElement.trySetEventHandler(PKey.menu.selectedMenuItemChanged, view.SelectedMenuItemChanged)

    ()

  override _.name = $"Menu"

  override _.setProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.setProps (terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> Menu

    // Properties
    props
    |> Props.tryFind PKey.menu.selectedMenuItem
    |> Option.iter (fun v -> view.SelectedMenuItem <- v)

    props
    |> Props.tryFind PKey.menu.superMenuItem
    |> Option.iter (fun v -> view.SuperMenuItem <- v)
    // Events
    terminalElement.trySetEventHandler(PKey.menu.accepted, view.Accepted)

    terminalElement.trySetEventHandler(PKey.menu.selectedMenuItemChanged, view.SelectedMenuItemChanged)

  override this.newView() = new Menu()


  override this.setAsChildOfParentView = false

  interface IMenuElement


type PopoverMenuElement(props: Props) =
  inherit TerminalElement(props)

  override _.removeProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.removeProps (terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> PopoverMenu

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
    terminalElement.tryRemoveEventHandler PKey.popoverMenu.accepted

    terminalElement.tryRemoveEventHandler PKey.popoverMenu.keyChanged

  override this.name = "PopoverMenu"

  override _.setProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.setProps (terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> PopoverMenu

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
    terminalElement.trySetEventHandler(PKey.popoverMenu.accepted, view.Accepted)

    terminalElement.trySetEventHandler(PKey.popoverMenu.keyChanged, view.KeyChanged)

  override this.SubElements_PropKeys =
    SubElementPropKey.from PKey.popoverMenu.root_element
    :: base.SubElements_PropKeys

  override this.newView() = new PopoverMenu()


  interface IPopoverMenuElement

// MenuBarItem
type MenuBarItemElement(props: Props) =
  inherit TerminalElement(props)

  override _.removeProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.removeProps (terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> MenuBarItem

    // Properties
    props
    |> Props.tryFind PKey.menuBarItem.popoverMenu
    |> Option.iter (fun _ -> view.PopoverMenu <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.menuBarItem.popoverMenuOpen
    |> Option.iter (fun _ -> view.PopoverMenuOpen <- Unchecked.defaultof<_>)
    // Events
    terminalElement.tryRemoveEventHandler PKey.menuBarItem.popoverMenuOpenChanged

  override this.name = "MenuBarItem"

  override _.setProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.setProps (terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> MenuBarItem

    // Properties
    props
    |> Props.tryFind PKey.menuBarItem.popoverMenu
    |> Option.iter (fun v -> view.PopoverMenu <- v)

    props
    |> Props.tryFind PKey.menuBarItem.popoverMenuOpen
    |> Option.iter (fun v -> view.PopoverMenuOpen <- v)
    // Events
    terminalElement.trySetEventHandler(PKey.menuBarItem.popoverMenuOpenChanged, view.PopoverMenuOpenChanged)

  override this.SubElements_PropKeys =
    SubElementPropKey.from PKey.menuBarItem.popoverMenu_element
    :: base.SubElements_PropKeys

  override this.newView() = new MenuBarItem()


  interface IMenuBarItemElement

// MenuBar
type MenuBarElement(props: Props) =
  inherit TerminalElement(props)

  override _.removeProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.removeProps (terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> MenuBar

    // Properties
    props
    |> Props.tryFind PKey.menuBar.key
    |> Option.iter (fun _ -> view.Key <- Unchecked.defaultof<_>)

    // NOTE: No need to handle `Menus: MenuBarItemElement list` property here,
    //       as it already registered as "children" property.
    //       And "children" properties are handled by the TreeDiff initializeTree function

    // Events
    terminalElement.tryRemoveEventHandler PKey.menuBar.keyChanged

  override _.name = $"MenuBar"

  override _.setProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.setProps (terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> MenuBar

    // Properties
    props
    |> Props.tryFind PKey.menuBar.key
    |> Option.iter (fun v -> view.Key <- v)

    // NOTE: No need to handle `Menus: MenuBarItemElement list` property here,
    //       as it already registered as "children" property.
    //       And "children" properties are handled by the TreeDiff initializeTree function

    // Events
    terminalElement.trySetEventHandler(PKey.menuBar.keyChanged, view.KeyChanged)

  override this.newView() = new MenuBar()

// Shortcut
type ShortcutElement(props: Props) =
  inherit TerminalElement(props)

  override _.removeProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.removeProps (terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> Shortcut

    // Interfaces
    OrientationInterface.removeProps terminalElement view props

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

  override _.setProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.setProps (terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> Shortcut

    // Interfaces
    OrientationInterface.setProps terminalElement view props

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

  override _.removeProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.removeProps (terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> MenuItem

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
    terminalElement.tryRemoveEventHandler PKey.menuItem.accepted

  override _.name = $"MenuItem"

  override _.setProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.setProps (terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> MenuItem

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
    terminalElement.trySetEventHandler(PKey.menuItem.accepted, view.Accepted)

  override this.SubElements_PropKeys =
    SubElementPropKey.from PKey.menuItem.subMenu_element
    :: base.SubElements_PropKeys


  override this.newView() = new MenuItem()

// NumericUpDown<'a>
type NumericUpDownElement<'a>(props: Props) =
  inherit TerminalElement(props)

  override _.removeProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.removeProps (terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> NumericUpDown<'a>

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
    terminalElement.tryRemoveEventHandler PKey.numericUpDown<'a>.formatChanged

    terminalElement.tryRemoveEventHandler PKey.numericUpDown<'a>.incrementChanged

    terminalElement.tryRemoveEventHandler PKey.numericUpDown<'a>.valueChanged

    terminalElement.tryRemoveEventHandler PKey.numericUpDown<'a>.valueChanging

  override _.name = $"NumericUpDown<'a>"

  override _.setProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.setProps (terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> NumericUpDown<'a>

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
    |> Option.iter (fun v -> terminalElement.EventRegistry.setEventHandler(PKey.numericUpDown<'a>.formatChanged, view.FormatChanged, v))

    props
    |> Props.tryFind PKey.numericUpDown<'a>.incrementChanged
    |> Option.iter (fun v -> terminalElement.EventRegistry.setEventHandler(PKey.numericUpDown<'a>.incrementChanged, view.IncrementChanged, v))

    props
    |> Props.tryFind PKey.numericUpDown<'a>.valueChanged
    |> Option.iter (fun v -> terminalElement.EventRegistry.setEventHandler(PKey.numericUpDown<'a>.valueChanged, view.ValueChanged, v))

    props
    |> Props.tryFind PKey.numericUpDown<'a>.valueChanging
    |> Option.iter (fun v -> terminalElement.EventRegistry.setEventHandler(PKey.numericUpDown<'a>.valueChanging, view.ValueChanging, v))

  override this.newView() = new NumericUpDown<'a>()


  interface INumericUpDownElement


// NumericUpDown
type NumericUpDownElement(props: Props) =
  inherit NumericUpDownElement<int>(props)

// OpenDialog
type OpenDialogElement(props: Props) =
  inherit TerminalElement(props)

  override _.removeProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.removeProps (terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> OpenDialog

    // Properties
    props
    |> Props.tryFind PKey.openDialog.openMode
    |> Option.iter (fun _ -> view.OpenMode <- Unchecked.defaultof<_>)

  override _.name = $"OpenDialog"

  override _.setProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.setProps (terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> OpenDialog

    // Properties
    props
    |> Props.tryFind PKey.openDialog.openMode
    |> Option.iter (fun v -> view.OpenMode <- v)


  override this.newView() = new OpenDialog()

// SelectorBase
type internal SelectorBaseElement(props: Props) =
  inherit TerminalElement(props)

  override _.removeProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.removeProps (terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> SelectorBase

    // Interfaces
    OrientationInterface.removeProps terminalElement view props

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
    terminalElement.tryRemoveEventHandler PKey.selectorBase.valueChanged

  override _.name = "SelectorBase"

  override _.setProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.setProps (terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> SelectorBase

    // Interfaces
    OrientationInterface.setProps terminalElement view props

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
    terminalElement.trySetEventHandler(PKey.selectorBase.valueChanged, view.ValueChanged)

  override this.newView() = raise (NotImplementedException())

// OptionSelector
type OptionSelectorElement(props: Props) =
  inherit SelectorBaseElement(props)

  override _.removeProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.removeProps (terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> OptionSelector

    // Properties
    props
    |> Props.tryFind PKey.optionSelector.cursor
    |> Option.iter (fun _ -> view.Cursor <- Unchecked.defaultof<_>)

  override _.name = $"OptionSelector"

  override _.setProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.setProps (terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> OptionSelector

    // Properties
    props
    |> Props.tryFind PKey.optionSelector.cursor
    |> Option.iter (fun v -> view.Cursor <- v)

  override this.newView() = new OptionSelector()


// FlagSelector
type FlagSelectorElement(props: Props) =
  inherit SelectorBaseElement(props)

  override _.removeProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.removeProps (terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> FlagSelector

    // Properties
    props
    |> Props.tryFind PKey.flagSelector.value
    |> Option.iter (fun _ -> view.Value <- Unchecked.defaultof<_>)

  override _.name = $"FlagSelector"

  override _.setProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.setProps (terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> FlagSelector

    // Properties
    props
    |> Props.tryFind PKey.flagSelector.value
    |> Option.iter (fun v -> view.Value <- v)

  override this.newView() = new FlagSelector()

// Padding
type PaddingElement(props: Props) =
  inherit TerminalElement(props)

  override _.removeProps(terminalElement: IInternalTerminalElement, props: Props) = base.removeProps (terminalElement, props)

  override _.name = $"Padding"

  override _.setProps(terminalElement: IInternalTerminalElement, props: Props) = base.setProps (terminalElement, props)


  override this.newView() = new Padding()

// ProgressBar
type ProgressBarElement(props: Props) =
  inherit TerminalElement(props)

  override _.removeProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.removeProps (terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> ProgressBar

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

  override _.setProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.setProps (terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> ProgressBar

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

  override _.removeProps(terminalElement: IInternalTerminalElement, props: Props) = base.removeProps (terminalElement, props)
  // No properties or events SaveDialog

  override _.name = $"SaveDialog"

  override _.setProps(terminalElement: IInternalTerminalElement, props: Props) = base.setProps (terminalElement, props)
  // No properties or events SaveDialog


  override this.newView() = new SaveDialog()

// ScrollBar
type ScrollBarElement(props: Props) =
  inherit TerminalElement(props)

  override _.removeProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.removeProps (terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> ScrollBar

    // Interfaces
    OrientationInterface.removeProps terminalElement view props

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
    terminalElement.tryRemoveEventHandler PKey.scrollBar.scrollableContentSizeChanged

    terminalElement.tryRemoveEventHandler PKey.scrollBar.sliderPositionChanged

  override _.name = $"ScrollBar"

  override _.setProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.setProps (terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> ScrollBar

    // Interfaces
    OrientationInterface.setProps terminalElement view props

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
    terminalElement.trySetEventHandler(PKey.scrollBar.scrollableContentSizeChanged, view.ScrollableContentSizeChanged)

    terminalElement.trySetEventHandler(PKey.scrollBar.sliderPositionChanged, view.SliderPositionChanged)


  override this.newView() = new ScrollBar()

// ScrollSlider
type ScrollSliderElement(props: Props) =
  inherit TerminalElement(props)

  override _.removeProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.removeProps (terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> ScrollSlider

    // Interfaces
    OrientationInterface.removeProps terminalElement view props

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
    terminalElement.tryRemoveEventHandler PKey.scrollSlider.positionChanged

    terminalElement.tryRemoveEventHandler PKey.scrollSlider.positionChanging

    terminalElement.tryRemoveEventHandler PKey.scrollSlider.scrolled

  override _.name = $"ScrollSlider"

  override _.setProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.setProps (terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> ScrollSlider

    // Interfaces
    OrientationInterface.setProps terminalElement view props

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
    terminalElement.trySetEventHandler(PKey.scrollSlider.positionChanged, view.PositionChanged)

    terminalElement.trySetEventHandler(PKey.scrollSlider.positionChanging, view.PositionChanging)

    terminalElement.trySetEventHandler(PKey.scrollSlider.scrolled, view.Scrolled)


  override this.newView() = new ScrollSlider()


// Slider<'a>
type SliderElement<'a>(props: Props) =
  inherit TerminalElement(props)

  override _.removeProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.removeProps (terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> Slider<'a>

    // Interfaces
    OrientationInterface.removeProps terminalElement view props

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
    terminalElement.tryRemoveEventHandler PKey.slider.optionFocused

    terminalElement.tryRemoveEventHandler PKey.slider.optionsChanged

  override _.name = $"Slider<'a>"

  override _.setProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.setProps (terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> Slider<'a>

    // Interfaces
    OrientationInterface.setProps terminalElement view props

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
    terminalElement.trySetEventHandler(PKey.slider.optionFocused, view.OptionFocused)

    terminalElement.trySetEventHandler(PKey.slider.optionsChanged, view.OptionsChanged)


  override this.newView() = new Slider<'a>()


  interface ISliderElement

// Slider
type SliderElement(props: Props) =
  inherit TerminalElement(props)

  override _.removeProps(terminalElement: IInternalTerminalElement, props: Props) = base.removeProps (terminalElement, props)
  // No properties or events Slider

  override _.name = $"Slider"

  override _.setProps(terminalElement: IInternalTerminalElement, props: Props) = base.setProps (terminalElement, props)
  // No properties or events Slider

  override this.newView() = new Slider()

// SpinnerView
type SpinnerViewElement(props: Props) =
  inherit TerminalElement(props)

  override _.removeProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.removeProps (terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> SpinnerView

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

  override _.setProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.setProps (terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> SpinnerView

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

  override _.removeProps(terminalElement: IInternalTerminalElement, props: Props) = base.removeProps (terminalElement, props)
  // No properties or events StatusBar

  override _.name = $"StatusBar"

  override _.setProps(terminalElement: IInternalTerminalElement, props: Props) = base.setProps (terminalElement, props)
  // No properties or events StatusBar


  override this.newView() = new StatusBar()

// Tab
type TabElement(props: Props) =
  inherit TerminalElement(props)

  override _.removeProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.removeProps (terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> Tab

    // Properties
    props
    |> Props.tryFind PKey.tab.displayText
    |> Option.iter (fun _ -> view.DisplayText <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.tab.view
    |> Option.iter (fun _ -> view.View <- Unchecked.defaultof<_>)

  override _.name = $"Tab"

  override _.setProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.setProps (terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> Tab

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

  override _.removeProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.removeProps (terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> TabView

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
    terminalElement.tryRemoveEventHandler PKey.tabView.selectedTabChanged

    terminalElement.tryRemoveEventHandler PKey.tabView.tabClicked

  override _.name = $"TabView"

  override _.setProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.setProps (terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> TabView

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
    terminalElement.trySetEventHandler(PKey.tabView.selectedTabChanged, view.SelectedTabChanged)

    terminalElement.trySetEventHandler(PKey.tabView.tabClicked, view.TabClicked)

    // Additional properties
    props
    |> Props.tryFind PKey.tabView.tabs
    |> Option.iter (fun tabItems ->
      tabItems
      |> Seq.iter (fun tabItem -> view.AddTab((tabItem :?> IInternalTerminalElement).View :?> Tab, false))
    )

  override this.SubElements_PropKeys =
    SubElementPropKey.from PKey.tabView.tabs_elements
    :: base.SubElements_PropKeys

  override this.newView() = new TabView()

// TableView
type TableViewElement(props: Props) =
  inherit TerminalElement(props)

  override _.removeProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.removeProps (terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> TableView

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
    terminalElement.tryRemoveEventHandler PKey.tableView.cellActivated

    terminalElement.tryRemoveEventHandler PKey.tableView.cellToggled

    terminalElement.tryRemoveEventHandler PKey.tableView.selectedCellChanged

  override _.name = $"TableView"

  override _.setProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.setProps (terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> TableView

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
    terminalElement.trySetEventHandler(PKey.tableView.cellActivated, view.CellActivated)

    terminalElement.trySetEventHandler(PKey.tableView.cellToggled, view.CellToggled)

    terminalElement.trySetEventHandler(PKey.tableView.selectedCellChanged, view.SelectedCellChanged)


  override this.newView() = new TableView()

// TextField
type TextFieldElement(props: Props) =
  inherit TerminalElement(props)

  override _.removeProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.removeProps (terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> TextField

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
    terminalElement.tryRemoveEventHandler PKey.textField.textChanging

  override _.name = $"TextField"

  override _.setProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.setProps (terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> TextField

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
    terminalElement.trySetEventHandler(PKey.textField.textChanging, view.TextChanging)


  override this.newView() = new TextField()

// TextValidateField
type TextValidateFieldElement(props: Props) =
  inherit TerminalElement(props)

  override _.removeProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.removeProps (terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> TextValidateField

    // Properties
    props
    |> Props.tryFind PKey.textValidateField.provider
    |> Option.iter (fun _ -> view.Provider <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.textValidateField.text
    |> Option.iter (fun _ -> view.Text <- Unchecked.defaultof<_>)

  override _.name = $"TextValidateField"

  override _.setProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.setProps (terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> TextValidateField

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


  override _.removeProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.removeProps (terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> TextView

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
    terminalElement.tryRemoveEventHandler PKey.textView.contentsChanged

    terminalElement.tryRemoveEventHandler PKey.textView.drawNormalColor

    terminalElement.tryRemoveEventHandler PKey.textView.drawReadOnlyColor

    terminalElement.tryRemoveEventHandler PKey.textView.drawSelectionColor

    terminalElement.tryRemoveEventHandler PKey.textView.drawUsedColor

    terminalElement.tryRemoveEventHandler PKey.textView.unwrappedCursorPosition

    // Additional properties
    terminalElement.tryRemoveEventHandler PKey.textView.textChanged


  override _.name = $"TextView"

  override _.setProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.setProps (terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> TextView

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
    terminalElement.trySetEventHandler(PKey.textView.contentsChanged, view.ContentsChanged)

    terminalElement.trySetEventHandler(PKey.textView.drawNormalColor, view.DrawNormalColor)

    terminalElement.trySetEventHandler(PKey.textView.drawReadOnlyColor, view.DrawReadOnlyColor)

    terminalElement.trySetEventHandler(PKey.textView.drawSelectionColor, view.DrawSelectionColor)

    terminalElement.trySetEventHandler(PKey.textView.drawUsedColor, view.DrawUsedColor)

    terminalElement.trySetEventHandler(PKey.textView.unwrappedCursorPosition, view.UnwrappedCursorPosition)

    // Additional properties
    terminalElement.trySetEventHandler(PKey.textView.textChanged, view.ContentsChanged)


  override this.newView() = new TextView()

// TimeField
type TimeFieldElement(props: Props) =
  inherit TerminalElement(props)

  override _.removeProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.removeProps (terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> TimeField

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
    terminalElement.tryRemoveEventHandler PKey.timeField.timeChanged

  override _.name = $"TimeField"

  override _.setProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.setProps (terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> TimeField

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
    terminalElement.trySetEventHandler(PKey.timeField.timeChanged, view.TimeChanged)


  override this.newView() = new TimeField()

// Runnable
type RunnableElement(props: Props) =
  inherit TerminalElement(props)

  override _.removeProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.removeProps (terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> Runnable

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
    terminalElement.tryRemoveEventHandler PKey.runnable.isRunningChanging

    terminalElement.tryRemoveEventHandler PKey.runnable.isRunningChanged

    terminalElement.tryRemoveEventHandler PKey.runnable.isModalChanged

  override _.name = $"Runnable"

  override _.setProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.setProps (terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> Runnable

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
    terminalElement.trySetEventHandler(PKey.runnable.isRunningChanging, view.IsRunningChanging)

    terminalElement.trySetEventHandler(PKey.runnable.isRunningChanged, view.IsRunningChanged)

    terminalElement.trySetEventHandler(PKey.runnable.isModalChanged, view.IsModalChanged)


  override this.newView() = new Runnable()


// TreeView<'a when 'a : not struct>
type TreeViewElement<'a when 'a: not struct>(props: Props) =
  inherit TerminalElement(props)

  override _.removeProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.removeProps (terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> TreeView<'a>

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
    terminalElement.tryRemoveEventHandler PKey.treeView<'a>.drawLine

    terminalElement.tryRemoveEventHandler PKey.treeView<'a>.objectActivated

    terminalElement.tryRemoveEventHandler PKey.treeView<'a>.selectionChanged

  override _.name = $"TreeView<'a>"

  override _.setProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.setProps (terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> TreeView<'a>

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
    |> Option.iter (fun v -> terminalElement.EventRegistry.setEventHandler(PKey.treeView<'a>.drawLine, view.DrawLine, v))

    props
    |> Props.tryFind PKey.treeView<'a>.objectActivated
    |> Option.iter (fun v -> terminalElement.EventRegistry.setEventHandler(PKey.treeView<'a>.objectActivated, view.ObjectActivated, v))

    props
    |> Props.tryFind PKey.treeView<'a>.selectionChanged
    |> Option.iter (fun v -> terminalElement.EventRegistry.setEventHandler(PKey.treeView<'a>.selectionChanged, view.SelectionChanged, v))

  override this.newView() = new TreeView<'a>()


  interface ITreeViewElement

// TreeView
type TreeViewElement(props: Props) =
  inherit TreeViewElement<ITreeNode>(props)

// Window
type WindowElement(props: Props) =
  inherit TerminalElement(props)

  override _.removeProps(terminalElement: IInternalTerminalElement, props: Props) = base.removeProps (terminalElement, props)
  // No properties or events Window

  override _.name = $"Window"

  override _.setProps(terminalElement: IInternalTerminalElement, props: Props) = base.setProps (terminalElement, props)
  // No properties or events Window


  override this.newView() = new Window()

// Wizard
type WizardElement(props: Props) =
  inherit TerminalElement(props)

  override _.removeProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.removeProps (terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> Wizard

    // Properties
    props
    |> Props.tryFind PKey.wizard.currentStep
    |> Option.iter (fun _ -> view.CurrentStep <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.wizard.modal
    |> Option.iter (fun _ -> view.SetIsModal(Unchecked.defaultof<_>))
    // Events
    terminalElement.tryRemoveEventHandler PKey.wizard.cancelled

    terminalElement.tryRemoveEventHandler PKey.wizard.finished

    terminalElement.tryRemoveEventHandler PKey.wizard.movingBack

    terminalElement.tryRemoveEventHandler PKey.wizard.movingNext

    terminalElement.tryRemoveEventHandler PKey.wizard.stepChanged

    terminalElement.tryRemoveEventHandler PKey.wizard.stepChanging

  override _.name = $"Wizard"

  override _.setProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.setProps (terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> Wizard

    // Properties
    props
    |> Props.tryFind PKey.wizard.currentStep
    |> Option.iter (fun v -> view.CurrentStep <- v)

    props
    |> Props.tryFind PKey.wizard.modal
    |> Option.iter (fun v -> view.SetIsModal(v))
    // Events
    terminalElement.trySetEventHandler(PKey.wizard.cancelled, view.Cancelled)

    terminalElement.trySetEventHandler(PKey.wizard.finished, view.Finished)

    terminalElement.trySetEventHandler(PKey.wizard.movingBack, view.MovingBack)

    terminalElement.trySetEventHandler(PKey.wizard.movingNext, view.MovingNext)

    terminalElement.trySetEventHandler(PKey.wizard.stepChanged, view.StepChanged)

    terminalElement.trySetEventHandler(PKey.wizard.stepChanging, view.StepChanging)


  override this.newView() = new Wizard()

// WizardStep
type WizardStepElement(props: Props) =
  inherit TerminalElement(props)

  override _.removeProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.removeProps (terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> WizardStep

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

  override _.setProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.setProps (terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> WizardStep

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
