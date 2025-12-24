namespace Terminal.Gui.Elmish

open System
open System.Collections.Generic
open System.Collections.Specialized
open Terminal.Gui.Elmish
open Terminal.Gui.ViewBase

type internal TreeNode = {
  TerminalElement: IInternalTerminalElement
  Parent: View option
}


[<CustomEquality; NoComparison>]
type internal SubElementPropKey<'a> =
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
type internal TerminalElement(props: Props) =

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

  // TODO: Is the view needed as param ? is the props needed as param ?
  abstract removeProps: terminalElement: IInternalTerminalElement * props: Props -> unit

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


type internal ViewTerminalElement(props: Props) =
  inherit TerminalElement(props)

  override _.newView() = new View()

  override _.name = $"View"

  override _.setProps(terminalElement: IInternalTerminalElement, props: Props) =

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

  override _.removeProps(terminalElement: IInternalTerminalElement, props: Props) =

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


// OrientationInterface - used by elements that implement Terminal.Gui.ViewBase.IOrientation
type internal OrientationInterface =
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
