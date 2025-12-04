module internal Terminal.Gui.Elmish.Elements

open System
open System.Collections.Generic
open System.Collections.ObjectModel
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
    | MultiElementKey _ -> raise (InvalidOperationException())

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

[<AbstractClass>]
type TerminalElement(props: Props) =

  static let eventLog = Dictionary<string, string>()

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

  let applyPos (apply: Pos -> unit) targetPos =
    // TODO: There is still some Pos.* function that needs to be implemented.
    match targetPos with
    | TPos.X te ->
      (te :?> IInternalTerminalElement).onDrawComplete.Add(fun view -> apply (Pos.X(view)))
    | TPos.Y te ->
      (te :?> IInternalTerminalElement).onDrawComplete.Add(fun view -> apply (Pos.Y(view)))
    | TPos.Top te ->
      (te :?> IInternalTerminalElement).onDrawComplete.Add(fun view -> apply (Pos.Top(view)))
    | TPos.Bottom te ->
      (te :?> IInternalTerminalElement).onDrawComplete.Add(fun view -> apply (Pos.Bottom(view)))
    | TPos.Left te ->
      (te :?> IInternalTerminalElement).onDrawComplete.Add(fun view -> apply (Pos.Left(view)))
    | TPos.Right te ->
      (te :?> IInternalTerminalElement).onDrawComplete.Add(fun view -> apply (Pos.Right(view)))
    | TPos.Absolute position -> apply (Pos.Absolute(position))
    | TPos.AnchorEnd offset -> apply (Pos.AnchorEnd(offset |> Option.defaultValue 0))
    | TPos.Center -> apply (Pos.Center())

  member this.props = props
  member val parent: View option = None with get, set

  member val private _view: View = null with get, set
  member this.view
    with get() =
      this._view
    and set value =
      this._view <- value
      // TODO: Maybe rely on the already existent property for the event handler
      // TODO: thing is, when removing properties it's also removing the event handler added here.
      // TODO: probably events props should be handled more precisely, keeping track of the subscript of one View in its TerminalElement.
      // No risk of memory leaks here, since the event subscriber (`drawCompleteEvent` here) outlasts the event publisher.
      this._view.DrawComplete.Add(fun _ -> onDrawCompleteEvent.Trigger this._view)

  member _.children: List<IInternalTerminalElement> =
    props
    |> Props.tryFindWithDefault PKey.view.children (List<_>())

  abstract SubElements_PropKeys: SubElementPropKey<IInternalTerminalElement> list
  default _.SubElements_PropKeys = []

  abstract newView: unit -> View

  abstract setAsChildOfParentView: bool
  default _.setAsChildOfParentView = true

  member this.initialize(parent) =
#if DEBUG
    Diagnostics.Trace.WriteLine $"{this.name} created!"
#endif
    this.parent <- parent

    let newView = this.newView ()

    this.initializeSubElements newView
    |> Seq.iter props.addNonTyped

    // Here, the "children" view are added to their parent
    if this.setAsChildOfParentView then
      parent
      |> Option.iter (fun p -> p.Add newView |> ignore)

    this.setProps (newView, props)
    this.view <- newView

  abstract canReuseView: prevView: View -> prevProps: Props -> bool
  abstract reuse: prevView: View -> prevProps: Props -> unit

  default this.canReuseView prevView prevProps =
    let changedProps, removedProps =
      Props.compare prevProps props

    let removedProps =
      removedProps
      |> Props.filter (not << _.Key.isViewKey)

    let canUpdateView =
      ViewElement.canReuseView prevView changedProps removedProps

    let canUpdateElement = true

    canUpdateView && canUpdateElement

  abstract name: string

  member this.initializeTree(parent: View option) : unit =
    let traverse (node: TreeNode) =
      node.TerminalElement.initialize node.Parent

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

            yield viewKey, subElement.view
          | :? List<IInternalTerminalElement> as elements ->
            elements
            |> Seq.iter (fun e -> e.initializeTree (Some parent))

            let viewKey = x.viewKey

            let views =
              elements |> Seq.map _.view |> Seq.toList

            yield viewKey, views
          | _ -> failwith "Out of range subElement type"
    }

  static member logEvent(propertyName: string, eventType: string) =
    eventLog.[propertyName] <- eventType

  abstract setProps: element: View * props: Props -> unit

  default this.setProps(element: View, props: Props) =
    // Properties
    props
    |> Props.tryFind PKey.view.arrangement
    |> Option.iter (fun v -> 
      TerminalElement.logEvent("arrangement", "set")
      element.Arrangement <- v)

    props
    |> Props.tryFind PKey.view.borderStyle
    |> Option.iter (fun v -> 
      TerminalElement.logEvent("borderStyle", "set")
      element.BorderStyle <- v)

    props
    |> Props.tryFind PKey.view.canFocus
    |> Option.iter (fun v -> 
      TerminalElement.logEvent("canFocus", "set")
      element.CanFocus <- v)

    props
    |> Props.tryFind PKey.view.contentSizeTracksViewport
    |> Option.iter (fun v -> 
      TerminalElement.logEvent("contentSizeTracksViewport", "set")
      element.ContentSizeTracksViewport <- v)

    props
    |> Props.tryFind PKey.view.cursorVisibility
    |> Option.iter (fun v -> 
      TerminalElement.logEvent("cursorVisibility", "set")
      element.CursorVisibility <- v)

    props
    |> Props.tryFind PKey.view.data
    |> Option.iter (fun v -> 
      TerminalElement.logEvent("data", "set")
      element.Data <- v)

    props
    |> Props.tryFind PKey.view.enabled
    |> Option.iter (fun v -> 
      TerminalElement.logEvent("enabled", "set")
      element.Enabled <- v)

    props
    |> Props.tryFind PKey.view.frame
    |> Option.iter (fun v -> 
      TerminalElement.logEvent("frame", "set")
      element.Frame <- v)

    props
    |> Props.tryFind PKey.view.hasFocus
    |> Option.iter (fun v -> 
      TerminalElement.logEvent("hasFocus", "set")
      element.HasFocus <- v)

    props
    |> Props.tryFind PKey.view.height
    |> Option.iter (fun v -> 
      TerminalElement.logEvent("height", "set")
      element.Height <- v)

    props
    |> Props.tryFind PKey.view.highlightStates
    |> Option.iter (fun v -> 
      TerminalElement.logEvent("highlightStates", "set")
      element.HighlightStates <- v)

    props
    |> Props.tryFind PKey.view.hotKey
    |> Option.iter (fun v -> 
      TerminalElement.logEvent("hotKey", "set")
      element.HotKey <- v)

    props
    |> Props.tryFind PKey.view.hotKeySpecifier
    |> Option.iter (fun v -> 
      TerminalElement.logEvent("hotKeySpecifier", "set")
      element.HotKeySpecifier <- v)

    props
    |> Props.tryFind PKey.view.id
    |> Option.iter (fun v -> 
      TerminalElement.logEvent("id", "set")
      element.Id <- v)

    props
    |> Props.tryFind PKey.view.isInitialized
    |> Option.iter (fun v -> 
      TerminalElement.logEvent("isInitialized", "set")
      element.IsInitialized <- v)

    props
    |> Props.tryFind PKey.view.mouseHeldDown
    |> Option.iter (fun v -> 
      TerminalElement.logEvent("mouseHeldDown", "set")
      element.MouseHeldDown <- v)

    props
    |> Props.tryFind PKey.view.needsDraw
    |> Option.iter (fun v -> 
      TerminalElement.logEvent("needsDraw", "set")
      element.NeedsDraw <- v)

    props
    |> Props.tryFind PKey.view.preserveTrailingSpaces
    |> Option.iter (fun v -> 
      TerminalElement.logEvent("preserveTrailingSpaces", "set")
      element.PreserveTrailingSpaces <- v)

    props
    |> Props.tryFind PKey.view.schemeName
    |> Option.iter (fun v -> 
      TerminalElement.logEvent("schemeName", "set")
      element.SchemeName <- v)

    props
    |> Props.tryFind PKey.view.shadowStyle
    |> Option.iter (fun v -> 
      TerminalElement.logEvent("shadowStyle", "set")
      element.ShadowStyle <- v)

    props
    |> Props.tryFind PKey.view.superViewRendersLineCanvas
    |> Option.iter (fun v -> 
      TerminalElement.logEvent("superViewRendersLineCanvas", "set")
      element.SuperViewRendersLineCanvas <- v)

    props
    |> Props.tryFind PKey.view.tabStop
    |> Option.iter (fun v -> 
      TerminalElement.logEvent("tabStop", "set")
      element.TabStop <- v |> Option.toNullable)

    props
    |> Props.tryFind PKey.view.text
    |> Option.iter (fun v -> 
      TerminalElement.logEvent("text", "set")
      element.Text <- v)

    props
    |> Props.tryFind PKey.view.textAlignment
    |> Option.iter (fun v -> 
      TerminalElement.logEvent("textAlignment", "set")
      element.TextAlignment <- v)

    props
    |> Props.tryFind PKey.view.textDirection
    |> Option.iter (fun v -> 
      TerminalElement.logEvent("textDirection", "set")
      element.TextDirection <- v)

    props
    |> Props.tryFind PKey.view.title
    |> Option.iter (fun v -> 
      TerminalElement.logEvent("title", "set")
      element.Title <- v)

    props
    |> Props.tryFind PKey.view.validatePosDim
    |> Option.iter (fun v -> 
      TerminalElement.logEvent("validatePosDim", "set")
      element.ValidatePosDim <- v)

    props
    |> Props.tryFind PKey.view.verticalTextAlignment
    |> Option.iter (fun v -> 
      TerminalElement.logEvent("verticalTextAlignment", "set")
      element.VerticalTextAlignment <- v)

    props
    |> Props.tryFind PKey.view.viewport
    |> Option.iter (fun v -> 
      TerminalElement.logEvent("viewport", "set")
      element.Viewport <- v)

    props
    |> Props.tryFind PKey.view.viewportSettings
    |> Option.iter (fun v -> 
      TerminalElement.logEvent("viewportSettings", "set")
      element.ViewportSettings <- v)

    props
    |> Props.tryFind PKey.view.visible
    |> Option.iter (fun v -> 
      TerminalElement.logEvent("visible", "set")
      element.Visible <- v)

    props
    |> Props.tryFind PKey.view.wantContinuousButtonPressed
    |> Option.iter (fun v -> 
      TerminalElement.logEvent("wantContinuousButtonPressed", "set")
      element.WantContinuousButtonPressed <- v)

    props
    |> Props.tryFind PKey.view.wantMousePositionReports
    |> Option.iter (fun v -> 
      TerminalElement.logEvent("wantMousePositionReports", "set")
      element.WantMousePositionReports <- v)

    props
    |> Props.tryFind PKey.view.width
    |> Option.iter (fun v -> 
      TerminalElement.logEvent("width", "set")
      element.Width <- v)

    props
    |> Props.tryFind PKey.view.x
    |> Option.iter (fun v -> 
      TerminalElement.logEvent("x", "set")
      element.X <- v)

    props
    |> Props.tryFind PKey.view.y
    |> Option.iter (fun v -> 
      TerminalElement.logEvent("y", "set")
      element.Y <- v)
    // Events
    props
    |> Props.tryFind PKey.view.accepting
    |> Option.iter (fun v -> 
      TerminalElement.logEvent("accepting", "set")
      Interop.setEventHandler <@ element.Accepting @> v element)

    props
    |> Props.tryFind PKey.view.advancingFocus
    |> Option.iter (fun v -> 
      TerminalElement.logEvent("advancingFocus", "set")
      Interop.setEventHandler <@ element.AdvancingFocus @> v element)

    props
    |> Props.tryFind PKey.view.borderStyleChanged
    |> Option.iter (fun v -> 
      TerminalElement.logEvent("borderStyleChanged", "set")
      Interop.setEventHandler <@ element.BorderStyleChanged @> v element)

    props
    |> Props.tryFind PKey.view.canFocusChanged
    |> Option.iter (fun v -> 
      TerminalElement.logEvent("canFocusChanged", "set")
      Interop.setEventHandler <@ element.CanFocusChanged @> (fun _ -> v ()) element)

    props
    |> Props.tryFind PKey.view.clearedViewport
    |> Option.iter (fun v -> 
      TerminalElement.logEvent("clearedViewport", "set")
      Interop.setEventHandler <@ element.ClearedViewport @> v element)

    props
    |> Props.tryFind PKey.view.clearingViewport
    |> Option.iter (fun v -> 
      TerminalElement.logEvent("clearingViewport", "set")
      Interop.setEventHandler <@ element.ClearingViewport @> v element)

    props
    |> Props.tryFind PKey.view.commandNotBound
    |> Option.iter (fun v -> 
      TerminalElement.logEvent("commandNotBound", "set")
      Interop.setEventHandler <@ element.CommandNotBound @> v element)

    props
    |> Props.tryFind PKey.view.contentSizeChanged
    |> Option.iter (fun v -> 
      TerminalElement.logEvent("contentSizeChanged", "set")
      Interop.setEventHandler <@ element.ContentSizeChanged @> v element)

    props
    |> Props.tryFind PKey.view.disposing
    |> Option.iter (fun v -> 
      TerminalElement.logEvent("disposing", "set")
      Interop.setEventHandler <@ element.Disposing @> (fun _ -> v ()) element)

    props
    |> Props.tryFind PKey.view.drawComplete
    |> Option.iter (fun v -> 
      TerminalElement.logEvent("drawComplete", "set")
      Interop.setEventHandler <@ element.DrawComplete @> v element)

    props
    |> Props.tryFind PKey.view.drawingContent
    |> Option.iter (fun v -> 
      TerminalElement.logEvent("drawingContent", "set")
      Interop.setEventHandler <@ element.DrawingContent @> v element)

    props
    |> Props.tryFind PKey.view.drawingSubViews
    |> Option.iter (fun v -> 
      TerminalElement.logEvent("drawingSubViews", "set")
      Interop.setEventHandler <@ element.DrawingSubViews @> v element)

    props
    |> Props.tryFind PKey.view.drawingText
    |> Option.iter (fun v -> 
      TerminalElement.logEvent("drawingText", "set")
      Interop.setEventHandler <@ element.DrawingText @> v element)

    props
    |> Props.tryFind PKey.view.enabledChanged
    |> Option.iter (fun v -> 
      TerminalElement.logEvent("enabledChanged", "set")
      Interop.setEventHandler <@ element.EnabledChanged @> (fun _ -> v ()) element)

    props
    |> Props.tryFind PKey.view.focusedChanged
    |> Option.iter (fun v -> 
      TerminalElement.logEvent("focusedChanged", "set")
      Interop.setEventHandler <@ element.FocusedChanged @> v element)

    props
    |> Props.tryFind PKey.view.frameChanged
    |> Option.iter (fun v -> 
      TerminalElement.logEvent("frameChanged", "set")
      Interop.setEventHandler <@ element.FrameChanged @> v element)

    props
    |> Props.tryFind PKey.view.gettingAttributeForRole
    |> Option.iter (fun v -> 
      TerminalElement.logEvent("gettingAttributeForRole", "set")
      Interop.setEventHandler <@ element.GettingAttributeForRole @> v element)

    props
    |> Props.tryFind PKey.view.gettingScheme
    |> Option.iter (fun v -> 
      TerminalElement.logEvent("gettingScheme", "set")
      Interop.setEventHandler <@ element.GettingScheme @> v element)

    props
    |> Props.tryFind PKey.view.handlingHotKey
    |> Option.iter (fun v -> 
      TerminalElement.logEvent("handlingHotKey", "set")
      Interop.setEventHandler <@ element.HandlingHotKey @> v element)

    props
    |> Props.tryFind PKey.view.hasFocusChanged
    |> Option.iter (fun v -> 
      TerminalElement.logEvent("hasFocusChanged", "set")
      Interop.setEventHandler <@ element.HasFocusChanged @> v element)

    props
    |> Props.tryFind PKey.view.hasFocusChanging
    |> Option.iter (fun v -> 
      TerminalElement.logEvent("hasFocusChanging", "set")
      Interop.setEventHandler <@ element.HasFocusChanging @> v element)

    props
    |> Props.tryFind PKey.view.hotKeyChanged
    |> Option.iter (fun v -> 
      TerminalElement.logEvent("hotKeyChanged", "set")
      Interop.setEventHandler <@ element.HotKeyChanged @> v element)

    props
    |> Props.tryFind PKey.view.initialized
    |> Option.iter (fun v -> 
      TerminalElement.logEvent("initialized", "set")
      Interop.setEventHandler <@ element.Initialized @> (fun _ -> v ()) element)

    props
    |> Props.tryFind PKey.view.keyDown
    |> Option.iter (fun v -> 
      TerminalElement.logEvent("keyDown", "set")
      Interop.setEventHandler <@ element.KeyDown @> v element)

    props
    |> Props.tryFind PKey.view.keyDownNotHandled
    |> Option.iter (fun v -> 
      TerminalElement.logEvent("keyDownNotHandled", "set")
      Interop.setEventHandler <@ element.KeyDownNotHandled @> v element)

    props
    |> Props.tryFind PKey.view.keyUp
    |> Option.iter (fun v -> 
      TerminalElement.logEvent("keyUp", "set")
      Interop.setEventHandler <@ element.KeyUp @> v element)

    props
    |> Props.tryFind PKey.view.mouseClick
    |> Option.iter (fun v -> 
      TerminalElement.logEvent("mouseClick", "set")
      Interop.setEventHandler <@ element.MouseClick @> v element)

    props
    |> Props.tryFind PKey.view.mouseEnter
    |> Option.iter (fun v -> 
      TerminalElement.logEvent("mouseEnter", "set")
      Interop.setEventHandler <@ element.MouseEnter @> v element)

    props
    |> Props.tryFind PKey.view.mouseEvent
    |> Option.iter (fun v -> 
      TerminalElement.logEvent("mouseEvent", "set")
      Interop.setEventHandler <@ element.MouseEvent @> v element)

    props
    |> Props.tryFind PKey.view.mouseLeave
    |> Option.iter (fun v -> 
      TerminalElement.logEvent("mouseLeave", "set")
      Interop.setEventHandler <@ element.MouseLeave @> v element)

    props
    |> Props.tryFind PKey.view.mouseStateChanged
    |> Option.iter (fun v -> 
      TerminalElement.logEvent("mouseStateChanged", "set")
      Interop.setEventHandler <@ element.MouseStateChanged @> v element)

    props
    |> Props.tryFind PKey.view.mouseWheel
    |> Option.iter (fun v -> 
      TerminalElement.logEvent("mouseWheel", "set")
      Interop.setEventHandler <@ element.MouseWheel @> v element)

    props
    |> Props.tryFind PKey.view.removed
    |> Option.iter (fun v -> 
      TerminalElement.logEvent("removed", "set")
      Interop.setEventHandler <@ element.Removed @> v element)

    props
    |> Props.tryFind PKey.view.schemeChanged
    |> Option.iter (fun v -> 
      TerminalElement.logEvent("schemeChanged", "set")
      Interop.setEventHandler <@ element.SchemeChanged @> v element)

    props
    |> Props.tryFind PKey.view.schemeChanging
    |> Option.iter (fun v -> 
      TerminalElement.logEvent("schemeChanging", "set")
      Interop.setEventHandler <@ element.SchemeChanging @> v element)

    props
    |> Props.tryFind PKey.view.schemeNameChanged
    |> Option.iter (fun v -> 
      TerminalElement.logEvent("schemeNameChanged", "set")
      Interop.setEventHandler <@ element.SchemeNameChanged @> v element)

    props
    |> Props.tryFind PKey.view.schemeNameChanging
    |> Option.iter (fun v -> 
      TerminalElement.logEvent("schemeNameChanging", "set")
      Interop.setEventHandler <@ element.SchemeNameChanging @> v element)

    props
    |> Props.tryFind PKey.view.selecting
    |> Option.iter (fun v -> 
      TerminalElement.logEvent("selecting", "set")
      Interop.setEventHandler <@ element.Selecting @> v element)

    props
    |> Props.tryFind PKey.view.subViewAdded
    |> Option.iter (fun v -> 
      TerminalElement.logEvent("subViewAdded", "set")
      Interop.setEventHandler <@ element.SubViewAdded @> v element)

    props
    |> Props.tryFind PKey.view.subViewLayout
    |> Option.iter (fun v -> 
      TerminalElement.logEvent("subViewLayout", "set")
      Interop.setEventHandler <@ element.SubViewLayout @> v element)

    props
    |> Props.tryFind PKey.view.subViewRemoved
    |> Option.iter (fun v -> 
      TerminalElement.logEvent("subViewRemoved", "set")
      Interop.setEventHandler <@ element.SubViewRemoved @> v element)

    props
    |> Props.tryFind PKey.view.subViewsLaidOut
    |> Option.iter (fun v -> 
      TerminalElement.logEvent("subViewsLaidOut", "set")
      Interop.setEventHandler <@ element.SubViewsLaidOut @> v element)

    props
    |> Props.tryFind PKey.view.superViewChanged
    |> Option.iter (fun v -> 
      TerminalElement.logEvent("superViewChanged", "set")
      Interop.setEventHandler <@ element.SuperViewChanged @> v element)

    props
    |> Props.tryFind PKey.view.textChanged
    |> Option.iter (fun v -> 
      TerminalElement.logEvent("textChanged", "set")
      Interop.setEventHandler <@ element.TextChanged @> (fun _ -> v ()) element)

    props
    |> Props.tryFind PKey.view.titleChanged
    |> Option.iter (fun v -> 
      TerminalElement.logEvent("titleChanged", "set")
      Interop.setEventHandler <@ element.TitleChanged @> (fun arg -> v arg.Value) element)

    props
    |> Props.tryFind PKey.view.titleChanging
    |> Option.iter (fun v -> 
      TerminalElement.logEvent("titleChanging", "set")
      Interop.setEventHandler <@ element.TitleChanging @> v element)

    props
    |> Props.tryFind PKey.view.viewportChanged
    |> Option.iter (fun v -> 
      TerminalElement.logEvent("viewportChanged", "set")
      Interop.setEventHandler <@ element.ViewportChanged @> v element)

    props
    |> Props.tryFind PKey.view.visibleChanged
    |> Option.iter (fun v -> 
      TerminalElement.logEvent("visibleChanged", "set")
      Interop.setEventHandler <@ element.VisibleChanged @> (fun _ -> v ()) element)

    props
    |> Props.tryFind PKey.view.visibleChanging
    |> Option.iter (fun v -> 
      TerminalElement.logEvent("visibleChanging", "set")
      Interop.setEventHandler <@ element.VisibleChanging @> (fun _ -> v ()) element)

    // Custom Props
    props
    |> Props.tryFind PKey.view.x_delayedPos
    |> Option.iter (fun v ->
      TerminalElement.logEvent("x_delayedPos", "set")
      applyPos (fun pos -> element.X <- pos) v)

    props
    |> Props.tryFind PKey.view.y_delayedPos
    |> Option.iter (fun v ->
      TerminalElement.logEvent("y_delayedPos", "set")
      applyPos (fun pos -> element.Y <- pos) v)

  abstract removeProps: element: View * props: Props -> unit

  default this.removeProps(element: View, props: Props) =
    // Properties
    props

    |> Props.tryFind PKey.view.arrangement

    |> Option.iter (fun _ -> 

      TerminalElement.logEvent("arrangement", "removed")

      element.Arrangement <- Unchecked.defaultof<_>)

    props


    |> Props.tryFind PKey.view.borderStyle


    |> Option.iter (fun _ -> 


      TerminalElement.logEvent("borderStyle", "removed")


      element.BorderStyle <- Unchecked.defaultof<_>)

    props


    |> Props.tryFind PKey.view.canFocus


    |> Option.iter (fun _ -> 


      TerminalElement.logEvent("canFocus", "removed")


      element.CanFocus <- Unchecked.defaultof<_>)

    props


    |> Props.tryFind PKey.view.contentSizeTracksViewport


    |> Option.iter (fun _ -> 


      TerminalElement.logEvent("contentSizeTracksViewport", "removed")


      element.ContentSizeTracksViewport <- Unchecked.defaultof<_>)

    props


    |> Props.tryFind PKey.view.cursorVisibility


    |> Option.iter (fun _ -> 


      TerminalElement.logEvent("cursorVisibility", "removed")


      element.CursorVisibility <- Unchecked.defaultof<_>)

    props


    |> Props.tryFind PKey.view.data


    |> Option.iter (fun _ -> 


      TerminalElement.logEvent("data", "removed")


      element.Data <- Unchecked.defaultof<_>)

    props


    |> Props.tryFind PKey.view.enabled


    |> Option.iter (fun _ -> 


      TerminalElement.logEvent("enabled", "removed")


      element.Enabled <- Unchecked.defaultof<_>)

    props


    |> Props.tryFind PKey.view.frame


    |> Option.iter (fun _ -> 


      TerminalElement.logEvent("frame", "removed")


      element.Frame <- Unchecked.defaultof<_>)

    props


    |> Props.tryFind PKey.view.hasFocus


    |> Option.iter (fun _ -> 


      TerminalElement.logEvent("hasFocus", "removed")


      element.HasFocus <- Unchecked.defaultof<_>)

    props


    |> Props.tryFind PKey.view.height


    |> Option.iter (fun _ -> 


      TerminalElement.logEvent("height", "removed")


      element.Height <- Unchecked.defaultof<_>)

    props


    |> Props.tryFind PKey.view.highlightStates


    |> Option.iter (fun _ -> 


      TerminalElement.logEvent("highlightStates", "removed")


      element.HighlightStates <- Unchecked.defaultof<_>)

    props


    |> Props.tryFind PKey.view.hotKey


    |> Option.iter (fun _ -> 


      TerminalElement.logEvent("hotKey", "removed")


      element.HotKey <- Unchecked.defaultof<_>)

    props


    |> Props.tryFind PKey.view.hotKeySpecifier


    |> Option.iter (fun _ -> 


      TerminalElement.logEvent("hotKeySpecifier", "removed")


      element.HotKeySpecifier <- Unchecked.defaultof<_>)

    props


    |> Props.tryFind PKey.view.id


    |> Option.iter (fun _ -> 


      TerminalElement.logEvent("id", "removed")


      element.Id <- Unchecked.defaultof<_>)

    props


    |> Props.tryFind PKey.view.isInitialized


    |> Option.iter (fun _ -> 


      TerminalElement.logEvent("isInitialized", "removed")


      element.IsInitialized <- Unchecked.defaultof<_>)

    props


    |> Props.tryFind PKey.view.mouseHeldDown


    |> Option.iter (fun _ -> 


      TerminalElement.logEvent("mouseHeldDown", "removed")


      element.MouseHeldDown <- Unchecked.defaultof<_>)

    props


    |> Props.tryFind PKey.view.needsDraw


    |> Option.iter (fun _ -> 


      TerminalElement.logEvent("needsDraw", "removed")


      element.NeedsDraw <- Unchecked.defaultof<_>)

    props


    |> Props.tryFind PKey.view.preserveTrailingSpaces


    |> Option.iter (fun _ -> 


      TerminalElement.logEvent("preserveTrailingSpaces", "removed")


      element.PreserveTrailingSpaces <- Unchecked.defaultof<_>)

    props


    |> Props.tryFind PKey.view.schemeName


    |> Option.iter (fun _ -> 


      TerminalElement.logEvent("schemeName", "removed")


      element.SchemeName <- Unchecked.defaultof<_>)

    props


    |> Props.tryFind PKey.view.shadowStyle


    |> Option.iter (fun _ -> 


      TerminalElement.logEvent("shadowStyle", "removed")


      element.ShadowStyle <- Unchecked.defaultof<_>)

    props


    |> Props.tryFind PKey.view.superViewRendersLineCanvas


    |> Option.iter (fun _ -> 


      TerminalElement.logEvent("superViewRendersLineCanvas", "removed")


      element.SuperViewRendersLineCanvas <- Unchecked.defaultof<_>)

    props


    |> Props.tryFind PKey.view.tabStop


    |> Option.iter (fun _ -> 


      TerminalElement.logEvent("tabStop", "removed")


      element.TabStop <- Unchecked.defaultof<_> |> Option.toNullable)

    props


    |> Props.tryFind PKey.view.text


    |> Option.iter (fun _ -> 


      TerminalElement.logEvent("text", "removed")


      element.Text <- Unchecked.defaultof<_>)

    props


    |> Props.tryFind PKey.view.textAlignment


    |> Option.iter (fun _ -> 


      TerminalElement.logEvent("textAlignment", "removed")


      element.TextAlignment <- Unchecked.defaultof<_>)

    props


    |> Props.tryFind PKey.view.textDirection


    |> Option.iter (fun _ -> 


      TerminalElement.logEvent("textDirection", "removed")


      element.TextDirection <- Unchecked.defaultof<_>)

    props


    |> Props.tryFind PKey.view.title


    |> Option.iter (fun _ -> 


      TerminalElement.logEvent("title", "removed")


      element.Title <- Unchecked.defaultof<_>)

    props


    |> Props.tryFind PKey.view.validatePosDim


    |> Option.iter (fun _ -> 


      TerminalElement.logEvent("validatePosDim", "removed")


      element.ValidatePosDim <- Unchecked.defaultof<_>)

    props


    |> Props.tryFind PKey.view.verticalTextAlignment


    |> Option.iter (fun _ -> 


      TerminalElement.logEvent("verticalTextAlignment", "removed")


      element.VerticalTextAlignment <- Unchecked.defaultof<_>)

    props


    |> Props.tryFind PKey.view.viewport


    |> Option.iter (fun _ -> 


      TerminalElement.logEvent("viewport", "removed")


      element.Viewport <- Unchecked.defaultof<_>)

    props


    |> Props.tryFind PKey.view.viewportSettings


    |> Option.iter (fun _ -> 


      TerminalElement.logEvent("viewportSettings", "removed")


      element.ViewportSettings <- Unchecked.defaultof<_>)

    props


    |> Props.tryFind PKey.view.visible


    |> Option.iter (fun _ -> 


      TerminalElement.logEvent("visible", "removed")


      element.Visible <- Unchecked.defaultof<_>)

    props


    |> Props.tryFind PKey.view.wantContinuousButtonPressed


    |> Option.iter (fun _ -> 


      TerminalElement.logEvent("wantContinuousButtonPressed", "removed")


      element.WantContinuousButtonPressed <- Unchecked.defaultof<_>)

    props


    |> Props.tryFind PKey.view.wantMousePositionReports


    |> Option.iter (fun _ -> 


      TerminalElement.logEvent("wantMousePositionReports", "removed")


      element.WantMousePositionReports <- Unchecked.defaultof<_>)

    props


    |> Props.tryFind PKey.view.width


    |> Option.iter (fun _ -> 


      TerminalElement.logEvent("width", "removed")


      element.Width <- Unchecked.defaultof<_>)

    props


    |> Props.tryFind PKey.view.x


    |> Option.iter (fun _ -> 


      TerminalElement.logEvent("x", "removed")


      element.X <- Unchecked.defaultof<_>)

    props


    |> Props.tryFind PKey.view.y


    |> Option.iter (fun _ -> 


      TerminalElement.logEvent("y", "removed")


      element.Y <- Unchecked.defaultof<_>)
    // Events
    props

    |> Props.tryFind PKey.view.accepting

    |> Option.iter (fun _ -> 

      TerminalElement.logEvent("accepting", "removed")

      Interop.removeEventHandler <@ element.Accepting @> element)

    props


    |> Props.tryFind PKey.view.advancingFocus


    |> Option.iter (fun _ -> 


      TerminalElement.logEvent("advancingFocus", "removed")


      Interop.removeEventHandler <@ element.AdvancingFocus @> element)

    props


    |> Props.tryFind PKey.view.borderStyleChanged


    |> Option.iter (fun _ -> 


      TerminalElement.logEvent("borderStyleChanged", "removed")


      Interop.removeEventHandler <@ element.BorderStyleChanged @> element)

    props


    |> Props.tryFind PKey.view.canFocusChanged


    |> Option.iter (fun _ -> 


      TerminalElement.logEvent("canFocusChanged", "removed")


      Interop.removeEventHandler <@ element.CanFocusChanged @> element)

    props


    |> Props.tryFind PKey.view.clearedViewport


    |> Option.iter (fun _ -> 


      TerminalElement.logEvent("clearedViewport", "removed")


      Interop.removeEventHandler <@ element.ClearedViewport @> element)

    props


    |> Props.tryFind PKey.view.clearingViewport


    |> Option.iter (fun _ -> 


      TerminalElement.logEvent("clearingViewport", "removed")


      Interop.removeEventHandler <@ element.ClearingViewport @> element)

    props


    |> Props.tryFind PKey.view.commandNotBound


    |> Option.iter (fun _ -> 


      TerminalElement.logEvent("commandNotBound", "removed")


      Interop.removeEventHandler <@ element.CommandNotBound @> element)

    props


    |> Props.tryFind PKey.view.contentSizeChanged


    |> Option.iter (fun _ -> 


      TerminalElement.logEvent("contentSizeChanged", "removed")


      Interop.removeEventHandler <@ element.ContentSizeChanged @> element)

    props


    |> Props.tryFind PKey.view.disposing


    |> Option.iter (fun _ -> 


      TerminalElement.logEvent("disposing", "removed")


      Interop.removeEventHandler <@ element.Disposing @> element)

    props


    |> Props.tryFind PKey.view.drawComplete


    |> Option.iter (fun _ -> 


      TerminalElement.logEvent("drawComplete", "removed")


      Interop.removeEventHandler <@ element.DrawComplete @> element)

    props


    |> Props.tryFind PKey.view.drawingContent


    |> Option.iter (fun _ -> 


      TerminalElement.logEvent("drawingContent", "removed")


      Interop.removeEventHandler <@ element.DrawingContent @> element)

    props


    |> Props.tryFind PKey.view.drawingSubViews


    |> Option.iter (fun _ -> 


      TerminalElement.logEvent("drawingSubViews", "removed")


      Interop.removeEventHandler <@ element.DrawingSubViews @> element)

    props


    |> Props.tryFind PKey.view.drawingText


    |> Option.iter (fun _ -> 


      TerminalElement.logEvent("drawingText", "removed")


      Interop.removeEventHandler <@ element.DrawingText @> element)

    props


    |> Props.tryFind PKey.view.enabledChanged


    |> Option.iter (fun _ -> 


      TerminalElement.logEvent("enabledChanged", "removed")


      Interop.removeEventHandler <@ element.EnabledChanged @> element)

    props


    |> Props.tryFind PKey.view.focusedChanged


    |> Option.iter (fun _ -> 


      TerminalElement.logEvent("focusedChanged", "removed")


      Interop.removeEventHandler <@ element.FocusedChanged @> element)

    props


    |> Props.tryFind PKey.view.frameChanged


    |> Option.iter (fun _ -> 


      TerminalElement.logEvent("frameChanged", "removed")


      Interop.removeEventHandler <@ element.FrameChanged @> element)

    props


    |> Props.tryFind PKey.view.gettingAttributeForRole


    |> Option.iter (fun _ -> 


      TerminalElement.logEvent("gettingAttributeForRole", "removed")


      Interop.removeEventHandler <@ element.GettingAttributeForRole @> element)

    props


    |> Props.tryFind PKey.view.gettingScheme


    |> Option.iter (fun _ -> 


      TerminalElement.logEvent("gettingScheme", "removed")


      Interop.removeEventHandler <@ element.GettingScheme @> element)

    props


    |> Props.tryFind PKey.view.handlingHotKey


    |> Option.iter (fun _ -> 


      TerminalElement.logEvent("handlingHotKey", "removed")


      Interop.removeEventHandler <@ element.HandlingHotKey @> element)

    props


    |> Props.tryFind PKey.view.hasFocusChanged


    |> Option.iter (fun _ -> 


      TerminalElement.logEvent("hasFocusChanged", "removed")


      Interop.removeEventHandler <@ element.HasFocusChanged @> element)

    props


    |> Props.tryFind PKey.view.hasFocusChanging


    |> Option.iter (fun _ -> 


      TerminalElement.logEvent("hasFocusChanging", "removed")


      Interop.removeEventHandler <@ element.HasFocusChanging @> element)

    props


    |> Props.tryFind PKey.view.hotKeyChanged


    |> Option.iter (fun _ -> 


      TerminalElement.logEvent("hotKeyChanged", "removed")


      Interop.removeEventHandler <@ element.HotKeyChanged @> element)

    props


    |> Props.tryFind PKey.view.initialized


    |> Option.iter (fun _ -> 


      TerminalElement.logEvent("initialized", "removed")


      Interop.removeEventHandler <@ element.Initialized @> element)

    props


    |> Props.tryFind PKey.view.keyDown


    |> Option.iter (fun _ -> 


      TerminalElement.logEvent("keyDown", "removed")


      Interop.removeEventHandler <@ element.KeyDown @> element)

    props


    |> Props.tryFind PKey.view.keyDownNotHandled


    |> Option.iter (fun _ -> 


      TerminalElement.logEvent("keyDownNotHandled", "removed")


      Interop.removeEventHandler <@ element.KeyDownNotHandled @> element)

    props


    |> Props.tryFind PKey.view.keyUp


    |> Option.iter (fun _ -> 


      TerminalElement.logEvent("keyUp", "removed")


      Interop.removeEventHandler <@ element.KeyUp @> element)

    props


    |> Props.tryFind PKey.view.mouseClick


    |> Option.iter (fun _ -> 


      TerminalElement.logEvent("mouseClick", "removed")


      Interop.removeEventHandler <@ element.MouseClick @> element)

    props


    |> Props.tryFind PKey.view.mouseEnter


    |> Option.iter (fun _ -> 


      TerminalElement.logEvent("mouseEnter", "removed")


      Interop.removeEventHandler <@ element.MouseEnter @> element)

    props


    |> Props.tryFind PKey.view.mouseEvent


    |> Option.iter (fun _ -> 


      TerminalElement.logEvent("mouseEvent", "removed")


      Interop.removeEventHandler <@ element.MouseEvent @> element)

    props


    |> Props.tryFind PKey.view.mouseLeave


    |> Option.iter (fun _ -> 


      TerminalElement.logEvent("mouseLeave", "removed")


      Interop.removeEventHandler <@ element.MouseLeave @> element)

    props


    |> Props.tryFind PKey.view.mouseStateChanged


    |> Option.iter (fun _ -> 


      TerminalElement.logEvent("mouseStateChanged", "removed")


      Interop.removeEventHandler <@ element.MouseStateChanged @> element)

    props


    |> Props.tryFind PKey.view.mouseWheel


    |> Option.iter (fun _ -> 


      TerminalElement.logEvent("mouseWheel", "removed")


      Interop.removeEventHandler <@ element.MouseWheel @> element)

    props


    |> Props.tryFind PKey.view.removed


    |> Option.iter (fun _ -> 


      TerminalElement.logEvent("removed", "removed")


      Interop.removeEventHandler <@ element.Removed @> element)

    props


    |> Props.tryFind PKey.view.schemeChanged


    |> Option.iter (fun _ -> 


      TerminalElement.logEvent("schemeChanged", "removed")


      Interop.removeEventHandler <@ element.SchemeChanged @> element)

    props


    |> Props.tryFind PKey.view.schemeChanging


    |> Option.iter (fun _ -> 


      TerminalElement.logEvent("schemeChanging", "removed")


      Interop.removeEventHandler <@ element.SchemeChanging @> element)

    props


    |> Props.tryFind PKey.view.schemeNameChanged


    |> Option.iter (fun _ -> 


      TerminalElement.logEvent("schemeNameChanged", "removed")


      Interop.removeEventHandler <@ element.SchemeNameChanged @> element)

    props


    |> Props.tryFind PKey.view.schemeNameChanging


    |> Option.iter (fun _ -> 


      TerminalElement.logEvent("schemeNameChanging", "removed")


      Interop.removeEventHandler <@ element.SchemeNameChanging @> element)

    props


    |> Props.tryFind PKey.view.selecting


    |> Option.iter (fun _ -> 


      TerminalElement.logEvent("selecting", "removed")


      Interop.removeEventHandler <@ element.Selecting @> element)

    props


    |> Props.tryFind PKey.view.subViewAdded


    |> Option.iter (fun _ -> 


      TerminalElement.logEvent("subViewAdded", "removed")


      Interop.removeEventHandler <@ element.SubViewAdded @> element)

    props


    |> Props.tryFind PKey.view.subViewLayout


    |> Option.iter (fun _ -> 


      TerminalElement.logEvent("subViewLayout", "removed")


      Interop.removeEventHandler <@ element.SubViewLayout @> element)

    props


    |> Props.tryFind PKey.view.subViewRemoved


    |> Option.iter (fun _ -> 


      TerminalElement.logEvent("subViewRemoved", "removed")


      Interop.removeEventHandler <@ element.SubViewRemoved @> element)

    props


    |> Props.tryFind PKey.view.subViewsLaidOut


    |> Option.iter (fun _ -> 


      TerminalElement.logEvent("subViewsLaidOut", "removed")


      Interop.removeEventHandler <@ element.SubViewsLaidOut @> element)

    props


    |> Props.tryFind PKey.view.superViewChanged


    |> Option.iter (fun _ -> 


      TerminalElement.logEvent("superViewChanged", "removed")


      Interop.removeEventHandler <@ element.SuperViewChanged @> element)

    props


    |> Props.tryFind PKey.view.textChanged


    |> Option.iter (fun _ -> 


      TerminalElement.logEvent("textChanged", "removed")


      Interop.removeEventHandler <@ element.TextChanged @> element)

    props


    |> Props.tryFind PKey.view.titleChanged


    |> Option.iter (fun _ -> 


      TerminalElement.logEvent("titleChanged", "removed")


      Interop.removeEventHandler <@ element.TitleChanged @> element)

    props


    |> Props.tryFind PKey.view.titleChanging


    |> Option.iter (fun _ -> 


      TerminalElement.logEvent("titleChanging", "removed")


      Interop.removeEventHandler <@ element.TitleChanging @> element)

    props


    |> Props.tryFind PKey.view.viewportChanged


    |> Option.iter (fun _ -> 


      TerminalElement.logEvent("viewportChanged", "removed")


      Interop.removeEventHandler <@ element.ViewportChanged @> element)

    props


    |> Props.tryFind PKey.view.visibleChanged


    |> Option.iter (fun _ -> 


      TerminalElement.logEvent("visibleChanged", "removed")


      Interop.removeEventHandler <@ element.VisibleChanged @> element)

    props


    |> Props.tryFind PKey.view.visibleChanging


    |> Option.iter (fun _ -> 


      TerminalElement.logEvent("visibleChanging", "removed")


      Interop.removeEventHandler <@ element.VisibleChanging @> element)

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

    this.removeProps (prevView, removedProps)
    this.setProps (prevView, c.changedProps)
    this.view <- prevView


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

  interface IInternalTerminalElement with
    member this.initialize(parent) = this.initialize parent
    member this.initializeTree(parent) = this.initializeTree parent
    member this.canReuseView prevView prevProps = this.canReuseView prevView prevProps
    member this.reuse prevView prevProps = this.reuse prevView prevProps
    member this.view = this.view
    member this.props = this.props
    member this.name = this.name
    member this.children = this.children

    member this.setAsChildOfParentView =
      this.setAsChildOfParentView

    member this.onDrawComplete = this.onDrawComplete


// Adornment
type AdornmentElement(props: Props) =
  inherit TerminalElement(props)

  override this.removeProps(element: View, props: Props) =
    base.removeProps (element, props)
    let element = element :?> Adornment
    // Properties
    props

    |> Props.tryFind PKey.adornment.diagnostics

    |> Option.iter (fun _ -> 

      TerminalElement.logEvent("diagnostics", "removed")

      element.Diagnostics <- Unchecked.defaultof<_>)

    props


    |> Props.tryFind PKey.adornment.superViewRendersLineCanvas


    |> Option.iter (fun _ -> 


      TerminalElement.logEvent("superViewRendersLineCanvas", "removed")


      element.SuperViewRendersLineCanvas <- Unchecked.defaultof<_>)

    props


    |> Props.tryFind PKey.adornment.thickness


    |> Option.iter (fun _ -> 


      TerminalElement.logEvent("thickness", "removed")


      element.Thickness <- Unchecked.defaultof<_>)

    props


    |> Props.tryFind PKey.adornment.viewport


    |> Option.iter (fun _ -> 


      TerminalElement.logEvent("viewport", "removed")


      element.Viewport <- Unchecked.defaultof<_>)
    // Events
    props

    |> Props.tryFind PKey.adornment.thicknessChanged

    |> Option.iter (fun _ -> 

      TerminalElement.logEvent("thicknessChanged", "removed")

      Interop.removeEventHandler <@ element.ThicknessChanged @> element)

  override _.name = $"Adornment"

  override this.setProps(element: View, props: Props) =
    base.setProps (element, props)

    let element = element :?> Adornment

    // Properties
    props

    |> Props.tryFind PKey.adornment.diagnostics

    |> Option.iter (fun v -> 

      TerminalElement.logEvent("diagnostics", "set")

      element.Diagnostics <- v)

    props


    |> Props.tryFind PKey.adornment.superViewRendersLineCanvas


    |> Option.iter (fun v -> 


      TerminalElement.logEvent("superViewRendersLineCanvas", "set")


      element.SuperViewRendersLineCanvas <- v)

    props


    |> Props.tryFind PKey.adornment.thickness


    |> Option.iter (fun v -> 


      TerminalElement.logEvent("thickness", "set")


      element.Thickness <- v)

    props


    |> Props.tryFind PKey.adornment.viewport


    |> Option.iter (fun v -> 


      TerminalElement.logEvent("viewport", "set")


      element.Viewport <- v)
    // Events
    props

    |> Props.tryFind PKey.adornment.thicknessChanged

    |> Option.iter (fun v -> 

      TerminalElement.logEvent("thicknessChanged", "set")

      Interop.setEventHandler <@ element.ThicknessChanged @> (fun _ -> v ()) element)

  override this.newView() = new Adornment()


// Bar
type BarElement(props: Props) =
  inherit TerminalElement(props)

  override this.removeProps(element: View, props: Props) =
    base.removeProps (element, props)
    let element = element :?> Bar
    // Properties
    props

    |> Props.tryFind PKey.bar.alignmentModes

    |> Option.iter (fun _ -> 

      TerminalElement.logEvent("alignmentModes", "removed")

      element.AlignmentModes <- Unchecked.defaultof<_>)

    props


    |> Props.tryFind PKey.bar.orientation


    |> Option.iter (fun _ -> 


      TerminalElement.logEvent("orientation", "removed")


      element.Orientation <- Unchecked.defaultof<_>)
    // Events
    props

    |> Props.tryFind PKey.bar.orientationChanged

    |> Option.iter (fun _ -> 

      TerminalElement.logEvent("orientationChanged", "removed")

      Interop.removeEventHandler <@ element.OrientationChanged @> element)

    props


    |> Props.tryFind PKey.bar.orientationChanging


    |> Option.iter (fun _ -> 


      TerminalElement.logEvent("orientationChanging", "removed")


      Interop.removeEventHandler <@ element.OrientationChanging @> element)

  override _.name = $"Bar"

  override this.setProps(element: View, props: Props) =
    base.setProps (element, props)

    let element = element :?> Bar
    // Properties
    props

    |> Props.tryFind PKey.bar.alignmentModes

    |> Option.iter (fun v -> 

      TerminalElement.logEvent("alignmentModes", "set")

      element.AlignmentModes <- v)

    props


    |> Props.tryFind PKey.bar.orientation


    |> Option.iter (fun v -> 


      TerminalElement.logEvent("orientation", "set")


      element.Orientation <- v)
    // Events
    props

    |> Props.tryFind PKey.bar.orientationChanged

    |> Option.iter (fun v -> 

      TerminalElement.logEvent("orientationChanged", "set")

      Interop.setEventHandler <@ element.OrientationChanged @> (fun arg -> v arg.Value) element)

    props


    |> Props.tryFind PKey.bar.orientationChanging


    |> Option.iter (fun v -> 


      TerminalElement.logEvent("orientationChanging", "set")


      Interop.setEventHandler <@ element.OrientationChanging @> v element)


  override this.newView() = new Bar()

// Border
type BorderElement(props: Props) =
  inherit TerminalElement(props)

  override this.removeProps(element: View, props: Props) =
    base.removeProps (element, props)
    let element = element :?> Border
    // Properties
    props

    |> Props.tryFind PKey.border.lineStyle

    |> Option.iter (fun _ -> 

      TerminalElement.logEvent("lineStyle", "removed")

      element.LineStyle <- Unchecked.defaultof<_>)

    props


    |> Props.tryFind PKey.border.settings


    |> Option.iter (fun _ -> 


      TerminalElement.logEvent("settings", "removed")


      element.Settings <- Unchecked.defaultof<_>)

  override _.name = $"Border"

  override this.setProps(element: View, props: Props) =
    base.setProps (element, props)

    let element = element :?> Border

    // Properties
    props

    |> Props.tryFind PKey.border.lineStyle

    |> Option.iter (fun v -> 

      TerminalElement.logEvent("lineStyle", "set")

      element.LineStyle <- v)

    props


    |> Props.tryFind PKey.border.settings


    |> Option.iter (fun v -> 


      TerminalElement.logEvent("settings", "set")


      element.Settings <- v)


  override this.newView() = new Border()

// Button
type ButtonElement(props: Props) =
  inherit TerminalElement(props)

  override this.removeProps(element: View, props: Props) =
    base.removeProps (element, props)
    let element = element :?> Button
    // Properties
    props

    |> Props.tryFind PKey.button.hotKeySpecifier

    |> Option.iter (fun _ -> 

      TerminalElement.logEvent("hotKeySpecifier", "removed")

      element.HotKeySpecifier <- Unchecked.defaultof<_>)

    props


    |> Props.tryFind PKey.button.isDefault


    |> Option.iter (fun _ -> 


      TerminalElement.logEvent("isDefault", "removed")


      element.IsDefault <- Unchecked.defaultof<_>)

    props


    |> Props.tryFind PKey.button.noDecorations


    |> Option.iter (fun _ -> 


      TerminalElement.logEvent("noDecorations", "removed")


      element.NoDecorations <- Unchecked.defaultof<_>)

    props


    |> Props.tryFind PKey.button.noPadding


    |> Option.iter (fun _ -> 


      TerminalElement.logEvent("noPadding", "removed")


      element.NoPadding <- Unchecked.defaultof<_>)

    props


    |> Props.tryFind PKey.button.text


    |> Option.iter (fun _ -> 


      TerminalElement.logEvent("text", "removed")


      element.Text <- Unchecked.defaultof<_>)
    // Events
    props

    |> Props.tryFind PKey.button.wantContinuousButtonPressed

    |> Option.iter (fun _ -> 

      TerminalElement.logEvent("wantContinuousButtonPressed", "removed")

      element.WantContinuousButtonPressed <- Unchecked.defaultof<_>)

  override _.name = $"Button"

  override this.setProps(element: View, props: Props) =
    base.setProps (element, props)

    let element = element :?> Button

    // Properties
    props

    |> Props.tryFind PKey.button.hotKeySpecifier

    |> Option.iter (fun v -> 

      TerminalElement.logEvent("hotKeySpecifier", "set")

      element.HotKeySpecifier <- v)

    props


    |> Props.tryFind PKey.button.isDefault


    |> Option.iter (fun v -> 


      TerminalElement.logEvent("isDefault", "set")


      element.IsDefault <- v)

    props


    |> Props.tryFind PKey.button.noDecorations


    |> Option.iter (fun v -> 


      TerminalElement.logEvent("noDecorations", "set")


      element.NoDecorations <- v)

    props


    |> Props.tryFind PKey.button.noPadding


    |> Option.iter (fun v -> 


      TerminalElement.logEvent("noPadding", "set")


      element.NoPadding <- v)

    props


    |> Props.tryFind PKey.button.text


    |> Option.iter (fun v -> 


      TerminalElement.logEvent("text", "set")


      element.Text <- v)
    // Events
    props

    |> Props.tryFind PKey.button.wantContinuousButtonPressed

    |> Option.iter (fun v -> 

      TerminalElement.logEvent("wantContinuousButtonPressed", "set")

      element.WantContinuousButtonPressed <- v)


  override this.newView() = new Button()

// CheckBox
type CheckBoxElement(props: Props) =
  inherit TerminalElement(props)

  override this.removeProps(element: View, props: Props) =
    base.removeProps (element, props)
    let element = element :?> CheckBox
    // Properties
    props

    |> Props.tryFind PKey.checkBox.allowCheckStateNone

    |> Option.iter (fun _ -> 

      TerminalElement.logEvent("allowCheckStateNone", "removed")

      element.AllowCheckStateNone <- Unchecked.defaultof<_>)

    props


    |> Props.tryFind PKey.checkBox.checkedState


    |> Option.iter (fun _ -> 


      TerminalElement.logEvent("checkedState", "removed")


      element.CheckedState <- Unchecked.defaultof<_>)

    props


    |> Props.tryFind PKey.checkBox.hotKeySpecifier


    |> Option.iter (fun _ -> 


      TerminalElement.logEvent("hotKeySpecifier", "removed")


      element.HotKeySpecifier <- Unchecked.defaultof<_>)

    props


    |> Props.tryFind PKey.checkBox.radioStyle


    |> Option.iter (fun _ -> 


      TerminalElement.logEvent("radioStyle", "removed")


      element.RadioStyle <- Unchecked.defaultof<_>)

    props


    |> Props.tryFind PKey.checkBox.text


    |> Option.iter (fun _ -> 


      TerminalElement.logEvent("text", "removed")


      element.Text <- Unchecked.defaultof<_>)
    // Events
    props

    |> Props.tryFind PKey.checkBox.checkedStateChanging

    |> Option.iter (fun _ -> 

      TerminalElement.logEvent("checkedStateChanging", "removed")

      Interop.removeEventHandler <@ element.CheckedStateChanging @> element)

    props


    |> Props.tryFind PKey.checkBox.checkedStateChanged


    |> Option.iter (fun _ -> 


      TerminalElement.logEvent("checkedStateChanged", "removed")


      Interop.removeEventHandler <@ element.CheckedStateChanged @> element)

  override _.name = $"CheckBox"

  override this.setProps(element: View, props: Props) =
    base.setProps (element, props)

    let element = element :?> CheckBox

    // Properties
    props

    |> Props.tryFind PKey.checkBox.allowCheckStateNone

    |> Option.iter (fun v -> 

      TerminalElement.logEvent("allowCheckStateNone", "set")

      element.AllowCheckStateNone <- v)

    props


    |> Props.tryFind PKey.checkBox.checkedState


    |> Option.iter (fun v -> 


      TerminalElement.logEvent("checkedState", "set")


      element.CheckedState <- v)

    props


    |> Props.tryFind PKey.checkBox.hotKeySpecifier


    |> Option.iter (fun v -> 


      TerminalElement.logEvent("hotKeySpecifier", "set")


      element.HotKeySpecifier <- v)

    props


    |> Props.tryFind PKey.checkBox.radioStyle


    |> Option.iter (fun v -> 


      TerminalElement.logEvent("radioStyle", "set")


      element.RadioStyle <- v)

    props


    |> Props.tryFind PKey.checkBox.text


    |> Option.iter (fun v -> 


      TerminalElement.logEvent("text", "set")


      element.Text <- v)
    // Events
    props

    |> Props.tryFind PKey.checkBox.checkedStateChanging

    |> Option.iter (fun v -> 

      TerminalElement.logEvent("checkedStateChanging", "set")

      Interop.setEventHandler <@ element.CheckedStateChanging @> v element)

    props


    |> Props.tryFind PKey.checkBox.checkedStateChanged


    |> Option.iter (fun v -> 


      TerminalElement.logEvent("checkedStateChanged", "set")


      Interop.setEventHandler <@ element.CheckedStateChanged @> v element)


  override this.newView() = new CheckBox()

// ColorPicker
type ColorPickerElement(props: Props) =
  inherit TerminalElement(props)

  override this.removeProps(element: View, props: Props) =
    base.removeProps (element, props)
    let element = element :?> ColorPicker
    // Properties
    props

    |> Props.tryFind PKey.colorPicker.selectedColor

    |> Option.iter (fun _ -> 

      TerminalElement.logEvent("selectedColor", "removed")

      element.SelectedColor <- Unchecked.defaultof<_>)

    props


    |> Props.tryFind PKey.colorPicker.style


    |> Option.iter (fun _ -> 


      TerminalElement.logEvent("style", "removed")


      element.Style <- Unchecked.defaultof<_>)
    // Events
    props

    |> Props.tryFind PKey.colorPicker.colorChanged

    |> Option.iter (fun _ -> 

      TerminalElement.logEvent("colorChanged", "removed")

      Interop.removeEventHandler <@ element.ColorChanged @> element)

  override _.name = $"ColorPicker"

  override this.setProps(element: View, props: Props) =
    base.setProps (element, props)

    let element = element :?> ColorPicker

    // Properties
    props

    |> Props.tryFind PKey.colorPicker.selectedColor

    |> Option.iter (fun v -> 

      TerminalElement.logEvent("selectedColor", "set")

      element.SelectedColor <- v)

    props


    |> Props.tryFind PKey.colorPicker.style


    |> Option.iter (fun v -> 


      TerminalElement.logEvent("style", "set")


      element.Style <- v)
    // Events
    props

    |> Props.tryFind PKey.colorPicker.colorChanged

    |> Option.iter (fun v -> 

      TerminalElement.logEvent("colorChanged", "set")

      Interop.setEventHandler <@ element.ColorChanged @> v element)


  override this.newView() = new ColorPicker()

// ColorPicker16
type ColorPicker16Element(props: Props) =
  inherit TerminalElement(props)

  override this.removeProps(element: View, props: Props) =
    base.removeProps (element, props)
    let element = element :?> ColorPicker16
    // Properties
    props

    |> Props.tryFind PKey.colorPicker16.boxHeight

    |> Option.iter (fun _ -> 

      TerminalElement.logEvent("boxHeight", "removed")

      element.BoxHeight <- Unchecked.defaultof<_>)

    props


    |> Props.tryFind PKey.colorPicker16.boxWidth


    |> Option.iter (fun _ -> 


      TerminalElement.logEvent("boxWidth", "removed")


      element.BoxWidth <- Unchecked.defaultof<_>)

    props


    |> Props.tryFind PKey.colorPicker16.cursor


    |> Option.iter (fun _ -> 


      TerminalElement.logEvent("cursor", "removed")


      element.Cursor <- Unchecked.defaultof<_>)

    props


    |> Props.tryFind PKey.colorPicker16.selectedColor


    |> Option.iter (fun _ -> 


      TerminalElement.logEvent("selectedColor", "removed")


      element.SelectedColor <- Unchecked.defaultof<_>)
    // Events
    props

    |> Props.tryFind PKey.colorPicker16.colorChanged

    |> Option.iter (fun _ -> 

      TerminalElement.logEvent("colorChanged", "removed")

      Interop.removeEventHandler <@ element.ColorChanged @> element)

  override _.name = $"ColorPicker16"

  override this.setProps(element: View, props: Props) =
    base.setProps (element, props)

    let element = element :?> ColorPicker16

    // Properties
    props

    |> Props.tryFind PKey.colorPicker16.boxHeight

    |> Option.iter (fun v -> 

      TerminalElement.logEvent("boxHeight", "set")

      element.BoxHeight <- v)

    props


    |> Props.tryFind PKey.colorPicker16.boxWidth


    |> Option.iter (fun v -> 


      TerminalElement.logEvent("boxWidth", "set")


      element.BoxWidth <- v)

    props


    |> Props.tryFind PKey.colorPicker16.cursor


    |> Option.iter (fun v -> 


      TerminalElement.logEvent("cursor", "set")


      element.Cursor <- v)

    props


    |> Props.tryFind PKey.colorPicker16.selectedColor


    |> Option.iter (fun v -> 


      TerminalElement.logEvent("selectedColor", "set")


      element.SelectedColor <- v)
    // Events
    props

    |> Props.tryFind PKey.colorPicker16.colorChanged

    |> Option.iter (fun v -> 

      TerminalElement.logEvent("colorChanged", "set")

      Interop.setEventHandler <@ element.ColorChanged @> v element)


  override this.newView() = new ColorPicker16()

// ComboBox
type ComboBoxElement(props: Props) =
  inherit TerminalElement(props)

  override this.removeProps(element: View, props: Props) =
    base.removeProps (element, props)
    let element = element :?> ComboBox
    // Properties
    props

    |> Props.tryFind PKey.comboBox.hideDropdownListOnClick

    |> Option.iter (fun _ -> 

      TerminalElement.logEvent("hideDropdownListOnClick", "removed")

      element.HideDropdownListOnClick <- Unchecked.defaultof<_>)

    props


    |> Props.tryFind PKey.comboBox.readOnly


    |> Option.iter (fun _ -> 


      TerminalElement.logEvent("readOnly", "removed")


      element.ReadOnly <- Unchecked.defaultof<_>)

    props


    |> Props.tryFind PKey.comboBox.searchText


    |> Option.iter (fun _ -> 


      TerminalElement.logEvent("searchText", "removed")


      element.SearchText <- Unchecked.defaultof<_>)

    props


    |> Props.tryFind PKey.comboBox.selectedItem


    |> Option.iter (fun _ -> 


      TerminalElement.logEvent("selectedItem", "removed")


      element.SelectedItem <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.comboBox.source
    |> Option.iter (fun _ -> element.SetSource Unchecked.defaultof<_>)

    props


    |> Props.tryFind PKey.comboBox.text


    |> Option.iter (fun _ -> 


      TerminalElement.logEvent("text", "removed")


      element.Text <- Unchecked.defaultof<_>)
    // Events
    props

    |> Props.tryFind PKey.comboBox.collapsed

    |> Option.iter (fun _ -> 

      TerminalElement.logEvent("collapsed", "removed")

      Interop.removeEventHandler <@ element.Collapsed @> element)

    props


    |> Props.tryFind PKey.comboBox.expanded


    |> Option.iter (fun _ -> 


      TerminalElement.logEvent("expanded", "removed")


      Interop.removeEventHandler <@ element.Expanded @> element)

    props


    |> Props.tryFind PKey.comboBox.openSelectedItem


    |> Option.iter (fun _ -> 


      TerminalElement.logEvent("openSelectedItem", "removed")


      Interop.removeEventHandler <@ element.OpenSelectedItem @> element)

    props


    |> Props.tryFind PKey.comboBox.selectedItemChanged


    |> Option.iter (fun _ -> 


      TerminalElement.logEvent("selectedItemChanged", "removed")


      Interop.removeEventHandler <@ element.SelectedItemChanged @> element)

  override _.name = $"ComboBox"

  override this.setProps(element: View, props: Props) =
    base.setProps (element, props)

    let element = element :?> ComboBox

    // Properties
    props

    |> Props.tryFind PKey.comboBox.hideDropdownListOnClick

    |> Option.iter (fun v -> 

      TerminalElement.logEvent("hideDropdownListOnClick", "set")

      element.HideDropdownListOnClick <- v)

    props


    |> Props.tryFind PKey.comboBox.readOnly


    |> Option.iter (fun v -> 


      TerminalElement.logEvent("readOnly", "set")


      element.ReadOnly <- v)

    props


    |> Props.tryFind PKey.comboBox.searchText


    |> Option.iter (fun v -> 


      TerminalElement.logEvent("searchText", "set")


      element.SearchText <- v)

    props


    |> Props.tryFind PKey.comboBox.selectedItem


    |> Option.iter (fun v -> 


      TerminalElement.logEvent("selectedItem", "set")


      element.SelectedItem <- v)

    props
    |> Props.tryFind PKey.comboBox.source
    |> Option.iter (fun v -> element.SetSource(ObservableCollection(v)))

    props


    |> Props.tryFind PKey.comboBox.text


    |> Option.iter (fun v -> 


      TerminalElement.logEvent("text", "set")


      element.Text <- v)
    // Events
    props

    |> Props.tryFind PKey.comboBox.collapsed

    |> Option.iter (fun v -> 

      TerminalElement.logEvent("collapsed", "set")

      Interop.setEventHandler <@ element.Collapsed @> (fun _ -> v ()) element)

    props


    |> Props.tryFind PKey.comboBox.expanded


    |> Option.iter (fun v -> 


      TerminalElement.logEvent("expanded", "set")


      Interop.setEventHandler <@ element.Expanded @> (fun _ -> v ()) element)

    props


    |> Props.tryFind PKey.comboBox.openSelectedItem


    |> Option.iter (fun v -> 


      TerminalElement.logEvent("openSelectedItem", "set")


      Interop.setEventHandler <@ element.OpenSelectedItem @> v element)

    props


    |> Props.tryFind PKey.comboBox.selectedItemChanged


    |> Option.iter (fun v -> 


      TerminalElement.logEvent("selectedItemChanged", "set")


      Interop.setEventHandler <@ element.SelectedItemChanged @> v element)


  override this.newView() = new ComboBox()

// DateField
type DateFieldElement(props: Props) =
  inherit TerminalElement(props)

  override this.removeProps(element: View, props: Props) =
    base.removeProps (element, props)
    let element = element :?> DateField
    // Properties
    props

    |> Props.tryFind PKey.dateField.culture

    |> Option.iter (fun _ -> 

      TerminalElement.logEvent("culture", "removed")

      element.Culture <- Unchecked.defaultof<_>)

    props


    |> Props.tryFind PKey.dateField.cursorPosition


    |> Option.iter (fun _ -> 


      TerminalElement.logEvent("cursorPosition", "removed")


      element.CursorPosition <- Unchecked.defaultof<_>)

    props


    |> Props.tryFind PKey.dateField.date


    |> Option.iter (fun _ -> 


      TerminalElement.logEvent("date", "removed")


      element.Date <- Unchecked.defaultof<_>)
    // Events
    props

    |> Props.tryFind PKey.dateField.dateChanged

    |> Option.iter (fun _ -> 

      TerminalElement.logEvent("dateChanged", "removed")

      Interop.removeEventHandler <@ element.DateChanged @> element)

  override _.name = $"DateField"

  override this.setProps(element: View, props: Props) =
    base.setProps (element, props)

    let element = element :?> DateField

    // Properties
    props

    |> Props.tryFind PKey.dateField.culture

    |> Option.iter (fun v -> 

      TerminalElement.logEvent("culture", "set")

      element.Culture <- v)

    props


    |> Props.tryFind PKey.dateField.cursorPosition


    |> Option.iter (fun v -> 


      TerminalElement.logEvent("cursorPosition", "set")


      element.CursorPosition <- v)

    props


    |> Props.tryFind PKey.dateField.date


    |> Option.iter (fun v -> 


      TerminalElement.logEvent("date", "set")


      element.Date <- v)
    // Events
    props

    |> Props.tryFind PKey.dateField.dateChanged

    |> Option.iter (fun v -> 

      TerminalElement.logEvent("dateChanged", "set")

      Interop.setEventHandler <@ element.DateChanged @> v element)


  override this.newView() = new DateField()

// DatePicker
type DatePickerElement(props: Props) =
  inherit TerminalElement(props)

  override this.removeProps(element: View, props: Props) =
    base.removeProps (element, props)
    let element = element :?> DatePicker
    // Properties
    props

    |> Props.tryFind PKey.datePicker.culture

    |> Option.iter (fun _ -> 

      TerminalElement.logEvent("culture", "removed")

      element.Culture <- Unchecked.defaultof<_>)

    props


    |> Props.tryFind PKey.datePicker.date


    |> Option.iter (fun _ -> 


      TerminalElement.logEvent("date", "removed")


      element.Date <- Unchecked.defaultof<_>)

  override _.name = $"DatePicker"

  override this.setProps(element: View, props: Props) =
    base.setProps (element, props)

    let element = element :?> DatePicker

    // Properties
    props

    |> Props.tryFind PKey.datePicker.culture

    |> Option.iter (fun v -> 

      TerminalElement.logEvent("culture", "set")

      element.Culture <- v)

    props


    |> Props.tryFind PKey.datePicker.date


    |> Option.iter (fun v -> 


      TerminalElement.logEvent("date", "set")


      element.Date <- v)


  override this.newView() = new DatePicker()

// Dialog
type DialogElement(props: Props) =
  inherit TerminalElement(props)

  override this.removeProps(element: View, props: Props) =
    base.removeProps (element, props)
    let element = element :?> Dialog
    // Properties
    props

    |> Props.tryFind PKey.dialog.buttonAlignment

    |> Option.iter (fun _ -> 

      TerminalElement.logEvent("buttonAlignment", "removed")

      element.ButtonAlignment <- Unchecked.defaultof<_>)

    props


    |> Props.tryFind PKey.dialog.buttonAlignmentModes


    |> Option.iter (fun _ -> 


      TerminalElement.logEvent("buttonAlignmentModes", "removed")


      element.ButtonAlignmentModes <- Unchecked.defaultof<_>)

    props


    |> Props.tryFind PKey.dialog.canceled


    |> Option.iter (fun _ -> 


      TerminalElement.logEvent("canceled", "removed")


      element.Canceled <- Unchecked.defaultof<_>)

  override _.name = $"Dialog"

  override this.setProps(element: View, props: Props) =
    base.setProps (element, props)

    let element = element :?> Dialog

    // Properties
    props

    |> Props.tryFind PKey.dialog.buttonAlignment

    |> Option.iter (fun v -> 

      TerminalElement.logEvent("buttonAlignment", "set")

      element.ButtonAlignment <- v)

    props


    |> Props.tryFind PKey.dialog.buttonAlignmentModes


    |> Option.iter (fun v -> 


      TerminalElement.logEvent("buttonAlignmentModes", "set")


      element.ButtonAlignmentModes <- v)

    props


    |> Props.tryFind PKey.dialog.canceled


    |> Option.iter (fun v -> 


      TerminalElement.logEvent("canceled", "set")


      element.Canceled <- v)


  override this.newView() = new Dialog()

// FileDialog
type FileDialogElement(props: Props) =
  inherit TerminalElement(props)

  override this.removeProps(element: View, props: Props) =
    base.removeProps (element, props)
    let element = element :?> FileDialog
    // Properties
    props

    |> Props.tryFind PKey.fileDialog.allowedTypes

    |> Option.iter (fun _ -> 

      TerminalElement.logEvent("allowedTypes", "removed")

      element.AllowedTypes <- Unchecked.defaultof<_>)

    props


    |> Props.tryFind PKey.fileDialog.allowsMultipleSelection


    |> Option.iter (fun _ -> 


      TerminalElement.logEvent("allowsMultipleSelection", "removed")


      element.AllowsMultipleSelection <- Unchecked.defaultof<_>)

    props


    |> Props.tryFind PKey.fileDialog.fileOperationsHandler


    |> Option.iter (fun _ -> 


      TerminalElement.logEvent("fileOperationsHandler", "removed")


      element.FileOperationsHandler <- Unchecked.defaultof<_>)

    props


    |> Props.tryFind PKey.fileDialog.mustExist


    |> Option.iter (fun _ -> 


      TerminalElement.logEvent("mustExist", "removed")


      element.MustExist <- Unchecked.defaultof<_>)

    props


    |> Props.tryFind PKey.fileDialog.openMode


    |> Option.iter (fun _ -> 


      TerminalElement.logEvent("openMode", "removed")


      element.OpenMode <- Unchecked.defaultof<_>)

    props


    |> Props.tryFind PKey.fileDialog.path


    |> Option.iter (fun _ -> 


      TerminalElement.logEvent("path", "removed")


      element.Path <- Unchecked.defaultof<_>)

    props


    |> Props.tryFind PKey.fileDialog.searchMatcher


    |> Option.iter (fun _ -> 


      TerminalElement.logEvent("searchMatcher", "removed")


      element.SearchMatcher <- Unchecked.defaultof<_>)
    // Events
    props

    |> Props.tryFind PKey.fileDialog.filesSelected

    |> Option.iter (fun _ -> 

      TerminalElement.logEvent("filesSelected", "removed")

      Interop.removeEventHandler <@ element.FilesSelected @> element)

  override _.name = $"FileDialog"

  override this.setProps(element: View, props: Props) =
    base.setProps (element, props)

    let element = element :?> FileDialog

    // Properties
    props
    |> Props.tryFind PKey.fileDialog.allowedTypes
    |> Option.iter (fun v -> element.AllowedTypes <- List<_>(v))

    props


    |> Props.tryFind PKey.fileDialog.allowsMultipleSelection


    |> Option.iter (fun v -> 


      TerminalElement.logEvent("allowsMultipleSelection", "set")


      element.AllowsMultipleSelection <- v)

    props


    |> Props.tryFind PKey.fileDialog.fileOperationsHandler


    |> Option.iter (fun v -> 


      TerminalElement.logEvent("fileOperationsHandler", "set")


      element.FileOperationsHandler <- v)

    props


    |> Props.tryFind PKey.fileDialog.mustExist


    |> Option.iter (fun v -> 


      TerminalElement.logEvent("mustExist", "set")


      element.MustExist <- v)

    props


    |> Props.tryFind PKey.fileDialog.openMode


    |> Option.iter (fun v -> 


      TerminalElement.logEvent("openMode", "set")


      element.OpenMode <- v)

    props


    |> Props.tryFind PKey.fileDialog.path


    |> Option.iter (fun v -> 


      TerminalElement.logEvent("path", "set")


      element.Path <- v)

    props


    |> Props.tryFind PKey.fileDialog.searchMatcher


    |> Option.iter (fun v -> 


      TerminalElement.logEvent("searchMatcher", "set")


      element.SearchMatcher <- v)
    // Events
    props

    |> Props.tryFind PKey.fileDialog.filesSelected

    |> Option.iter (fun v -> 

      TerminalElement.logEvent("filesSelected", "set")

      Interop.setEventHandler <@ element.FilesSelected @> v element)


  override this.newView() = new FileDialog()

// FrameView
type FrameViewElement(props: Props) =
  inherit TerminalElement(props)

  override this.removeProps(element: View, props: Props) = base.removeProps (element, props)
  // No properties or events FrameView

  override _.name = $"FrameView"

  override this.setProps(element: View, props: Props) = base.setProps (element, props)
  // No properties or events FrameView


  override this.newView() = new FrameView()

// GraphView
type GraphViewElement(props: Props) =
  inherit TerminalElement(props)

  override this.removeProps(element: View, props: Props) =
    base.removeProps (element, props)
    let element = element :?> GraphView
    // Properties
    props

    |> Props.tryFind PKey.graphView.axisX

    |> Option.iter (fun _ -> 

      TerminalElement.logEvent("axisX", "removed")

      element.AxisX <- Unchecked.defaultof<_>)

    props


    |> Props.tryFind PKey.graphView.axisY


    |> Option.iter (fun _ -> 


      TerminalElement.logEvent("axisY", "removed")


      element.AxisY <- Unchecked.defaultof<_>)

    props


    |> Props.tryFind PKey.graphView.cellSize


    |> Option.iter (fun _ -> 


      TerminalElement.logEvent("cellSize", "removed")


      element.CellSize <- Unchecked.defaultof<_>)

    props


    |> Props.tryFind PKey.graphView.graphColor


    |> Option.iter (fun _ -> 


      TerminalElement.logEvent("graphColor", "removed")


      element.GraphColor <- Unchecked.defaultof<_>)

    props


    |> Props.tryFind PKey.graphView.marginBottom


    |> Option.iter (fun _ -> 


      TerminalElement.logEvent("marginBottom", "removed")


      element.MarginBottom <- Unchecked.defaultof<_>)

    props


    |> Props.tryFind PKey.graphView.marginLeft


    |> Option.iter (fun _ -> 


      TerminalElement.logEvent("marginLeft", "removed")


      element.MarginLeft <- Unchecked.defaultof<_>)

    props


    |> Props.tryFind PKey.graphView.scrollOffset


    |> Option.iter (fun _ -> 


      TerminalElement.logEvent("scrollOffset", "removed")


      element.ScrollOffset <- Unchecked.defaultof<_>)

  override _.name = $"GraphView"

  override this.setProps(element: View, props: Props) =
    base.setProps (element, props)

    let element = element :?> GraphView

    // Properties
    props

    |> Props.tryFind PKey.graphView.axisX

    |> Option.iter (fun v -> 

      TerminalElement.logEvent("axisX", "set")

      element.AxisX <- v)

    props


    |> Props.tryFind PKey.graphView.axisY


    |> Option.iter (fun v -> 


      TerminalElement.logEvent("axisY", "set")


      element.AxisY <- v)

    props


    |> Props.tryFind PKey.graphView.cellSize


    |> Option.iter (fun v -> 


      TerminalElement.logEvent("cellSize", "set")


      element.CellSize <- v)

    props
    |> Props.tryFind PKey.graphView.graphColor
    |> Option.iter (fun v -> element.GraphColor <- v |> Option.toNullable)

    props
    |> Props.tryFind PKey.graphView.marginBottom
    |> Option.iter (fun v -> element.MarginBottom <- (v |> uint32))

    props
    |> Props.tryFind PKey.graphView.marginLeft
    |> Option.iter (fun v -> element.MarginLeft <- (v |> uint32))

    props


    |> Props.tryFind PKey.graphView.scrollOffset


    |> Option.iter (fun v -> 


      TerminalElement.logEvent("scrollOffset", "set")


      element.ScrollOffset <- v)


  override this.newView() = new GraphView()

// HexView
type HexViewElement(props: Props) =
  inherit TerminalElement(props)

  override this.removeProps(element: View, props: Props) =
    base.removeProps (element, props)
    let element = element :?> HexView
    // Properties
    props

    |> Props.tryFind PKey.hexView.address

    |> Option.iter (fun _ -> 

      TerminalElement.logEvent("address", "removed")

      element.Address <- Unchecked.defaultof<_>)

    props


    |> Props.tryFind PKey.hexView.addressWidth


    |> Option.iter (fun _ -> 


      TerminalElement.logEvent("addressWidth", "removed")


      element.AddressWidth <- Unchecked.defaultof<_>)

    props


    |> Props.tryFind PKey.hexView.allowEdits


    |> Option.iter (fun _ -> 


      TerminalElement.logEvent("allowEdits", "removed")


      element.BytesPerLine <- Unchecked.defaultof<_>)

    props


    |> Props.tryFind PKey.hexView.readOnly


    |> Option.iter (fun _ -> 


      TerminalElement.logEvent("readOnly", "removed")


      element.ReadOnly <- Unchecked.defaultof<_>)

    props


    |> Props.tryFind PKey.hexView.source


    |> Option.iter (fun _ -> 


      TerminalElement.logEvent("source", "removed")


      element.Source <- Unchecked.defaultof<_>)
    // Events
    props

    |> Props.tryFind PKey.hexView.edited

    |> Option.iter (fun _ -> 

      TerminalElement.logEvent("edited", "removed")

      Interop.removeEventHandler <@ element.Edited @> element)

    props


    |> Props.tryFind PKey.hexView.positionChanged


    |> Option.iter (fun _ -> 


      TerminalElement.logEvent("positionChanged", "removed")


      Interop.removeEventHandler <@ element.PositionChanged @> element)

  override _.name = $"HexView"

  override this.setProps(element: View, props: Props) =
    base.setProps (element, props)

    let element = element :?> HexView

    // Properties
    props

    |> Props.tryFind PKey.hexView.address

    |> Option.iter (fun v -> 

      TerminalElement.logEvent("address", "set")

      element.Address <- v)

    props


    |> Props.tryFind PKey.hexView.addressWidth


    |> Option.iter (fun v -> 


      TerminalElement.logEvent("addressWidth", "set")


      element.AddressWidth <- v)

    props


    |> Props.tryFind PKey.hexView.allowEdits


    |> Option.iter (fun v -> 


      TerminalElement.logEvent("allowEdits", "set")


      element.BytesPerLine <- v)

    props


    |> Props.tryFind PKey.hexView.readOnly


    |> Option.iter (fun v -> 


      TerminalElement.logEvent("readOnly", "set")


      element.ReadOnly <- v)

    props


    |> Props.tryFind PKey.hexView.source


    |> Option.iter (fun v -> 


      TerminalElement.logEvent("source", "set")


      element.Source <- v)
    // Events
    props

    |> Props.tryFind PKey.hexView.edited

    |> Option.iter (fun v -> 

      TerminalElement.logEvent("edited", "set")

      Interop.setEventHandler <@ element.Edited @> v element)

    props


    |> Props.tryFind PKey.hexView.positionChanged


    |> Option.iter (fun v -> 


      TerminalElement.logEvent("positionChanged", "set")


      Interop.setEventHandler <@ element.PositionChanged @> v element)


  override this.newView() = new HexView()

// Label
type LabelElement(props: Props) =
  inherit TerminalElement(props)

  override this.removeProps(element: View, props: Props) =
    base.removeProps (element, props)
    let element = element :?> Label
    // Properties
    props

    |> Props.tryFind PKey.label.hotKeySpecifier

    |> Option.iter (fun _ -> 

      TerminalElement.logEvent("hotKeySpecifier", "removed")

      element.HotKeySpecifier <- Unchecked.defaultof<_>)

    props


    |> Props.tryFind PKey.label.text


    |> Option.iter (fun _ -> 


      TerminalElement.logEvent("text", "removed")


      element.Text <- Unchecked.defaultof<_>)

  override _.name = $"Label"

  override this.setProps(element: View, props: Props) =
    base.setProps (element, props)

    let element = element :?> Label

    // Properties
    props

    |> Props.tryFind PKey.label.hotKeySpecifier

    |> Option.iter (fun v -> 

      TerminalElement.logEvent("hotKeySpecifier", "set")

      element.HotKeySpecifier <- v)

    props


    |> Props.tryFind PKey.label.text


    |> Option.iter (fun v -> 


      TerminalElement.logEvent("text", "set")


      element.Text <- v)


  override this.newView() = new Label()

// LegendAnnotation
type LegendAnnotationElement(props: Props) =
  inherit TerminalElement(props)

  override this.removeProps(element: View, props: Props) = base.removeProps (element, props)
  // No properties or events LegendAnnotation

  override _.name = $"LegendAnnotation"

  override this.setProps(element: View, props: Props) = base.setProps (element, props)
  // No properties or events LegendAnnotation


  override this.newView() = new LegendAnnotation()

// Line
type LineElement(props: Props) =
  inherit TerminalElement(props)

  override this.removeProps(element: View, props: Props) =
    base.removeProps (element, props)
    let element = element :?> Line
    // Properties
    props

    |> Props.tryFind PKey.line.orientation

    |> Option.iter (fun _ -> 

      TerminalElement.logEvent("orientation", "removed")

      element.Orientation <- Unchecked.defaultof<_>)
    // Events
    props

    |> Props.tryFind PKey.line.orientationChanged

    |> Option.iter (fun _ -> 

      TerminalElement.logEvent("orientationChanged", "removed")

      Interop.removeEventHandler <@ element.OrientationChanged @> element)

    props


    |> Props.tryFind PKey.line.orientationChanging


    |> Option.iter (fun _ -> 


      TerminalElement.logEvent("orientationChanging", "removed")


      Interop.removeEventHandler <@ element.OrientationChanging @> element)

  override _.name = $"Line"

  override this.setProps(element: View, props: Props) =
    base.setProps (element, props)

    let element = element :?> Line

    // Properties
    props

    |> Props.tryFind PKey.line.orientation

    |> Option.iter (fun v -> 

      TerminalElement.logEvent("orientation", "set")

      element.Orientation <- v)
    // Events
    props

    |> Props.tryFind PKey.line.orientationChanged

    |> Option.iter (fun v -> 

      TerminalElement.logEvent("orientationChanged", "set")

      Interop.setEventHandler <@ element.OrientationChanged @> (fun arg -> v arg.Value) element)

    props


    |> Props.tryFind PKey.line.orientationChanging


    |> Option.iter (fun v -> 


      TerminalElement.logEvent("orientationChanging", "set")


      Interop.setEventHandler <@ element.OrientationChanging @> v element)


  override this.newView() = new Line()


// ListView
type ListViewElement(props: Props) =
  inherit TerminalElement(props)

  override this.removeProps(element: View, props: Props) =
    base.removeProps (element, props)
    let element = element :?> ListView
    // Properties
    props

    |> Props.tryFind PKey.listView.allowsMarking

    |> Option.iter (fun _ -> 

      TerminalElement.logEvent("allowsMarking", "removed")

      element.AllowsMarking <- Unchecked.defaultof<_>)

    props


    |> Props.tryFind PKey.listView.allowsMultipleSelection


    |> Option.iter (fun _ -> 


      TerminalElement.logEvent("allowsMultipleSelection", "removed")


      element.AllowsMultipleSelection <- Unchecked.defaultof<_>)

    props


    |> Props.tryFind PKey.listView.leftItem


    |> Option.iter (fun _ -> 


      TerminalElement.logEvent("leftItem", "removed")


      element.LeftItem <- Unchecked.defaultof<_>)

    props


    |> Props.tryFind PKey.listView.selectedItem


    |> Option.iter (fun _ -> 


      TerminalElement.logEvent("selectedItem", "removed")


      element.SelectedItem <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.listView.source
    |> Option.iter (fun _ -> element.SetSource Unchecked.defaultof<_>)

    props


    |> Props.tryFind PKey.listView.topItem


    |> Option.iter (fun _ -> 


      TerminalElement.logEvent("topItem", "removed")


      element.TopItem <- Unchecked.defaultof<_>)
    // Events
    props

    |> Props.tryFind PKey.listView.collectionChanged

    |> Option.iter (fun _ -> 

      TerminalElement.logEvent("collectionChanged", "removed")

      Interop.removeEventHandler <@ element.CollectionChanged @> element)

    props


    |> Props.tryFind PKey.listView.openSelectedItem


    |> Option.iter (fun _ -> 


      TerminalElement.logEvent("openSelectedItem", "removed")


      Interop.removeEventHandler <@ element.OpenSelectedItem @> element)

    props


    |> Props.tryFind PKey.listView.rowRender


    |> Option.iter (fun _ -> 


      TerminalElement.logEvent("rowRender", "removed")


      Interop.removeEventHandler <@ element.RowRender @> element)

    props


    |> Props.tryFind PKey.listView.selectedItemChanged


    |> Option.iter (fun _ -> 


      TerminalElement.logEvent("selectedItemChanged", "removed")


      Interop.removeEventHandler <@ element.SelectedItemChanged @> element)

  override _.name = $"ListView"

  override this.setProps(element: View, props: Props) =
    base.setProps (element, props)

    let element = element :?> ListView

    // Properties
    props

    |> Props.tryFind PKey.listView.allowsMarking

    |> Option.iter (fun v -> 

      TerminalElement.logEvent("allowsMarking", "set")

      element.AllowsMarking <- v)

    props


    |> Props.tryFind PKey.listView.allowsMultipleSelection


    |> Option.iter (fun v -> 


      TerminalElement.logEvent("allowsMultipleSelection", "set")


      element.AllowsMultipleSelection <- v)

    props


    |> Props.tryFind PKey.listView.leftItem


    |> Option.iter (fun v -> 


      TerminalElement.logEvent("leftItem", "set")


      element.LeftItem <- v)

    props


    |> Props.tryFind PKey.listView.selectedItem


    |> Option.iter (fun v -> 


      TerminalElement.logEvent("selectedItem", "set")


      element.SelectedItem <- v)

    props
    |> Props.tryFind PKey.listView.source
    |> Option.iter (fun v -> element.SetSource(ObservableCollection(v)))

    props


    |> Props.tryFind PKey.listView.topItem


    |> Option.iter (fun v -> 


      TerminalElement.logEvent("topItem", "set")


      element.TopItem <- v)
    // Events
    props

    |> Props.tryFind PKey.listView.collectionChanged

    |> Option.iter (fun v -> 

      TerminalElement.logEvent("collectionChanged", "set")

      Interop.setEventHandler <@ element.CollectionChanged @> v element)

    props


    |> Props.tryFind PKey.listView.openSelectedItem


    |> Option.iter (fun v -> 


      TerminalElement.logEvent("openSelectedItem", "set")


      Interop.setEventHandler <@ element.OpenSelectedItem @> v element)

    props


    |> Props.tryFind PKey.listView.rowRender


    |> Option.iter (fun v -> 


      TerminalElement.logEvent("rowRender", "set")


      Interop.setEventHandler <@ element.RowRender @> v element)

    props


    |> Props.tryFind PKey.listView.selectedItemChanged


    |> Option.iter (fun v -> 


      TerminalElement.logEvent("selectedItemChanged", "set")


      Interop.setEventHandler <@ element.SelectedItemChanged @> v element)


  override this.newView() = new ListView()

// Margin
type MarginElement(props: Props) =
  inherit TerminalElement(props)

  override this.removeProps(element: View, props: Props) =
    base.removeProps (element, props)
    let element = element :?> Margin
    // Properties
    props

    |> Props.tryFind PKey.margin.shadowStyle

    |> Option.iter (fun _ -> 

      TerminalElement.logEvent("shadowStyle", "removed")

      element.ShadowStyle <- Unchecked.defaultof<_>)

  override _.name = $"Margin"

  override this.setProps(element: View, props: Props) =
    base.setProps (element, props)

    let element = element :?> Margin

    // Properties
    props

    |> Props.tryFind PKey.margin.shadowStyle

    |> Option.iter (fun v -> 

      TerminalElement.logEvent("shadowStyle", "set")

      element.ShadowStyle <- v)


  override this.newView() = new Margin()

// Menu
type MenuElement(props: Props) =
  inherit TerminalElement(props)

  override this.removeProps(element: View, props: Props) =
    base.removeProps (element, props)
    let element = element :?> Menu
    // Properties
    props

    |> Props.tryFind PKey.menu.selectedMenuItem

    |> Option.iter (fun _ -> 

      TerminalElement.logEvent("selectedMenuItem", "removed")

      element.SelectedMenuItem <- Unchecked.defaultof<_>)

    props


    |> Props.tryFind PKey.menu.superMenuItem


    |> Option.iter (fun _ -> 


      TerminalElement.logEvent("superMenuItem", "removed")


      element.SuperMenuItem <- Unchecked.defaultof<_>)
    // Events
    props

    |> Props.tryFind PKey.menu.accepted

    |> Option.iter (fun v -> 

      TerminalElement.logEvent("accepted", "set")

      Interop.setEventHandler <@ element.Accepted @> v element)

    props


    |> Props.tryFind PKey.menu.selectedMenuItemChanged


    |> Option.iter (fun v -> 


      TerminalElement.logEvent("selectedMenuItemChanged", "set")


      Interop.setEventHandler <@ element.SelectedMenuItemChanged @> v element)

    ()

  override _.name = $"Menu"

  override this.setProps(element: View, props: Props) =
    base.setProps (element, props)

    let element = element :?> Menu

    // Properties
    props

    |> Props.tryFind PKey.menu.selectedMenuItem

    |> Option.iter (fun v -> 

      TerminalElement.logEvent("selectedMenuItem", "set")

      element.SelectedMenuItem <- v)

    props


    |> Props.tryFind PKey.menu.superMenuItem


    |> Option.iter (fun v -> 


      TerminalElement.logEvent("superMenuItem", "set")


      element.SuperMenuItem <- v)
    // Events
    props

    |> Props.tryFind PKey.menu.accepted

    |> Option.iter (fun v -> 

      TerminalElement.logEvent("accepted", "set")

      Interop.setEventHandler <@ element.Accepted @> v element)

    props


    |> Props.tryFind PKey.menu.selectedMenuItemChanged


    |> Option.iter (fun v -> 


      TerminalElement.logEvent("selectedMenuItemChanged", "set")


      Interop.setEventHandler <@ element.SelectedMenuItemChanged @> v element)

  override this.newView() = new Menu()


  override this.setAsChildOfParentView = false

  interface IMenuElement


type PopoverMenuElement(props: Props) =
  inherit TerminalElement(props)

  override this.removeProps(element: View, props: Props) =
    base.removeProps (element, props)
    let element = element :?> PopoverMenu
    // Properties
    props

    |> Props.tryFind PKey.popoverMenu.key

    |> Option.iter (fun _ -> 

      TerminalElement.logEvent("key", "removed")

      element.Key <- Unchecked.defaultof<_>)

    props


    |> Props.tryFind PKey.popoverMenu.mouseFlags


    |> Option.iter (fun _ -> 


      TerminalElement.logEvent("mouseFlags", "removed")


      element.MouseFlags <- Unchecked.defaultof<_>)

    props


    |> Props.tryFind PKey.popoverMenu.root


    |> Option.iter (fun _ -> 


      TerminalElement.logEvent("root", "removed")


      element.Root <- Unchecked.defaultof<_>)
    // Events
    props

    |> Props.tryFind PKey.popoverMenu.accepted

    |> Option.iter (fun _ -> 

      TerminalElement.logEvent("accepted", "removed")

      Interop.removeEventHandler <@ element.Accepted @> element)

    props


    |> Props.tryFind PKey.popoverMenu.keyChanged


    |> Option.iter (fun _ -> 


      TerminalElement.logEvent("keyChanged", "removed")


      Interop.removeEventHandler <@ element.KeyChanged @> element)

  override this.name = "PopoverMenu"

  override this.setProps(element: View, props: Props) =
    base.setProps (element, props)

    let element = element :?> PopoverMenu

    // Properties
    props

    |> Props.tryFind PKey.popoverMenu.key

    |> Option.iter (fun v -> 

      TerminalElement.logEvent("key", "set")

      element.Key <- v)

    props


    |> Props.tryFind PKey.popoverMenu.mouseFlags


    |> Option.iter (fun v -> 


      TerminalElement.logEvent("mouseFlags", "set")


      element.MouseFlags <- v)

    props


    |> Props.tryFind PKey.popoverMenu.root


    |> Option.iter (fun v -> 


      TerminalElement.logEvent("root", "set")


      element.Root <- v)
    // Events
    props

    |> Props.tryFind PKey.popoverMenu.accepted

    |> Option.iter (fun v -> 

      TerminalElement.logEvent("accepted", "set")

      Interop.setEventHandler <@ element.Accepted @> v element)

    props


    |> Props.tryFind PKey.popoverMenu.keyChanged


    |> Option.iter (fun v -> 


      TerminalElement.logEvent("keyChanged", "set")


      Interop.setEventHandler <@ element.KeyChanged @> v element)

  override this.SubElements_PropKeys =
    SubElementPropKey.from PKey.popoverMenu.root_element
    :: base.SubElements_PropKeys

  override this.newView() = new PopoverMenu()


  interface IPopoverMenuElement

// MenuBarItem
type MenuBarItemElement(props: Props) =
  inherit TerminalElement(props)

  override this.removeProps(element: View, props: Props) =
    base.removeProps (element, props)
    let element = element :?> MenuBarItem
    // Properties
    props

    |> Props.tryFind PKey.menuBarItem.popoverMenu

    |> Option.iter (fun _ -> 

      TerminalElement.logEvent("popoverMenu", "removed")

      element.PopoverMenu <- Unchecked.defaultof<_>)

    props


    |> Props.tryFind PKey.menuBarItem.popoverMenuOpen


    |> Option.iter (fun _ -> 


      TerminalElement.logEvent("popoverMenuOpen", "removed")


      element.PopoverMenuOpen <- Unchecked.defaultof<_>)
    // Events
    props

    |> Props.tryFind PKey.menuBarItem.popoverMenuOpenChanged

    |> Option.iter (fun _ -> 

      TerminalElement.logEvent("popoverMenuOpenChanged", "removed")

      Interop.removeEventHandler <@ element.PopoverMenuOpenChanged @> element)

  override this.name = "MenuBarItem"

  override this.setProps(element: View, props: Props) =
    base.setProps (element, props)

    let element = element :?> MenuBarItem

    // Properties
    props

    |> Props.tryFind PKey.menuBarItem.popoverMenu

    |> Option.iter (fun v -> 

      TerminalElement.logEvent("popoverMenu", "set")

      element.PopoverMenu <- v)

    props


    |> Props.tryFind PKey.menuBarItem.popoverMenuOpen


    |> Option.iter (fun v -> 


      TerminalElement.logEvent("popoverMenuOpen", "set")


      element.PopoverMenuOpen <- v)
    // Events
    props

    |> Props.tryFind PKey.menuBarItem.popoverMenuOpenChanged

    |> Option.iter (fun v -> 

      TerminalElement.logEvent("popoverMenuOpenChanged", "set")

      Interop.setEventHandler <@ element.PopoverMenuOpenChanged @> (fun args -> v args.Value) element)

  override this.SubElements_PropKeys =
    SubElementPropKey.from PKey.menuBarItem.popoverMenu_element
    :: base.SubElements_PropKeys

  override this.newView() = new MenuBarItem()


  interface IMenuBarItemElement

// MenuBar
type MenuBarElement(props: Props) =
  inherit TerminalElement(props)

  override this.removeProps(element: View, props: Props) =
    base.removeProps (element, props)
    let element = element :?> MenuBar
    // Properties
    props

    |> Props.tryFind PKey.menuBar.key

    |> Option.iter (fun _ -> 

      TerminalElement.logEvent("key", "removed")

      element.Key <- Unchecked.defaultof<_>)

    // NOTE: No need to handle `Menus: MenuBarItemElement list` property here,
    //       as it already registered as "children" property.
    //       And "children" properties are handled by the TreeDiff initializeTree function

    // Events
    props

    |> Props.tryFind PKey.menuBar.keyChanged

    |> Option.iter (fun _ -> 

      TerminalElement.logEvent("keyChanged", "removed")

      Interop.removeEventHandler <@ element.KeyChanged @> element)

  override _.name = $"MenuBar"

  override this.setProps(element: View, props: Props) =
    base.setProps (element, props)

    let element = element :?> MenuBar

    // Properties
    props

    |> Props.tryFind PKey.menuBar.key

    |> Option.iter (fun v -> 

      TerminalElement.logEvent("key", "set")

      element.Key <- v)

    // NOTE: No need to handle `Menus: MenuBarItemElement list` property here,
    //       as it already registered as "children" property.
    //       And "children" properties are handled by the TreeDiff initializeTree function

    // Events
    props

    |> Props.tryFind PKey.menuBar.keyChanged

    |> Option.iter (fun v -> 

      TerminalElement.logEvent("keyChanged", "set")

      Interop.setEventHandler <@ element.KeyChanged @> v element)

  override this.newView() = new MenuBar()

// Shortcut
type ShortcutElement(props: Props) =
  inherit TerminalElement(props)

  override this.removeProps(element: View, props: Props) =
    base.removeProps (element, props)
    let element = element :?> Shortcut
    // Properties
    props

    |> Props.tryFind PKey.shortcut.action

    |> Option.iter (fun _ -> 

      TerminalElement.logEvent("action", "removed")

      element.Action <- Unchecked.defaultof<_>)

    props


    |> Props.tryFind PKey.shortcut.alignmentModes


    |> Option.iter (fun _ -> 


      TerminalElement.logEvent("alignmentModes", "removed")


      element.AlignmentModes <- Unchecked.defaultof<_>)

    props


    |> Props.tryFind PKey.shortcut.commandView


    |> Option.iter (fun _ -> 


      TerminalElement.logEvent("commandView", "removed")


      element.CommandView <- Unchecked.defaultof<_>)

    props


    |> Props.tryFind PKey.shortcut.forceFocusColors


    |> Option.iter (fun _ -> 


      TerminalElement.logEvent("forceFocusColors", "removed")


      element.ForceFocusColors <- Unchecked.defaultof<_>)

    props


    |> Props.tryFind PKey.shortcut.helpText


    |> Option.iter (fun _ -> 


      TerminalElement.logEvent("helpText", "removed")


      element.HelpText <- Unchecked.defaultof<_>)

    props


    |> Props.tryFind PKey.shortcut.text


    |> Option.iter (fun _ -> 


      TerminalElement.logEvent("text", "removed")


      element.Text <- Unchecked.defaultof<_>)

    props


    |> Props.tryFind PKey.shortcut.bindKeyToApplication


    |> Option.iter (fun _ -> 


      TerminalElement.logEvent("bindKeyToApplication", "removed")


      element.BindKeyToApplication <- Unchecked.defaultof<_>)

    props


    |> Props.tryFind PKey.shortcut.key


    |> Option.iter (fun _ -> 


      TerminalElement.logEvent("key", "removed")


      element.Key <- Unchecked.defaultof<_>)

    props


    |> Props.tryFind PKey.shortcut.minimumKeyTextSize


    |> Option.iter (fun _ -> 


      TerminalElement.logEvent("minimumKeyTextSize", "removed")


      element.MinimumKeyTextSize <- Unchecked.defaultof<_>)
    // Events
    props

    |> Props.tryFind PKey.shortcut.orientationChanged

    |> Option.iter (fun _ -> 

      TerminalElement.logEvent("orientationChanged", "removed")

      Interop.removeEventHandler <@ element.OrientationChanged @> element)

    props


    |> Props.tryFind PKey.shortcut.orientationChanging


    |> Option.iter (fun _ -> 


      TerminalElement.logEvent("orientationChanging", "removed")


      Interop.removeEventHandler <@ element.OrientationChanging @> element)

  override _.name = $"Shortcut"

  override this.setProps(element: View, props: Props) =
    base.setProps (element, props)

    let element = element :?> Shortcut

    // Properties
    props

    |> Props.tryFind PKey.shortcut.action

    |> Option.iter (fun v -> 

      TerminalElement.logEvent("action", "set")

      element.Action <- v)

    props


    |> Props.tryFind PKey.shortcut.alignmentModes


    |> Option.iter (fun v -> 


      TerminalElement.logEvent("alignmentModes", "set")


      element.AlignmentModes <- v)

    props


    |> Props.tryFind PKey.shortcut.commandView


    |> Option.iter (fun v -> 


      TerminalElement.logEvent("commandView", "set")


      element.CommandView <- v)

    props


    |> Props.tryFind PKey.shortcut.forceFocusColors


    |> Option.iter (fun v -> 


      TerminalElement.logEvent("forceFocusColors", "set")


      element.ForceFocusColors <- v)

    props


    |> Props.tryFind PKey.shortcut.helpText


    |> Option.iter (fun v -> 


      TerminalElement.logEvent("helpText", "set")


      element.HelpText <- v)

    props


    |> Props.tryFind PKey.shortcut.text


    |> Option.iter (fun v -> 


      TerminalElement.logEvent("text", "set")


      element.Text <- v)

    props


    |> Props.tryFind PKey.shortcut.bindKeyToApplication


    |> Option.iter (fun v -> 


      TerminalElement.logEvent("bindKeyToApplication", "set")


      element.BindKeyToApplication <- v)

    props


    |> Props.tryFind PKey.shortcut.key


    |> Option.iter (fun v -> 


      TerminalElement.logEvent("key", "set")


      element.Key <- v)

    props


    |> Props.tryFind PKey.shortcut.minimumKeyTextSize


    |> Option.iter (fun v -> 


      TerminalElement.logEvent("minimumKeyTextSize", "set")


      element.MinimumKeyTextSize <- v)

    // Events
    props

    |> Props.tryFind PKey.shortcut.orientationChanged

    |> Option.iter (fun v -> 

      TerminalElement.logEvent("orientationChanged", "set")

      Interop.setEventHandler <@ element.OrientationChanged @> (fun arg -> v arg.Value) element)

    props


    |> Props.tryFind PKey.shortcut.orientationChanging


    |> Option.iter (fun v -> 


      TerminalElement.logEvent("orientationChanging", "set")


      Interop.setEventHandler <@ element.OrientationChanging @> v element)

  override this.SubElements_PropKeys =
    SubElementPropKey.from PKey.shortcut.commandView_element
    :: base.SubElements_PropKeys

  override this.newView() = new Shortcut()

type MenuItemElement(props: Props) =
  inherit ShortcutElement(props)

  override this.removeProps(element: View, props: Props) =
    base.removeProps (element, props)
    let element = element :?> MenuItem
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

    |> Option.iter (fun _ -> 

      TerminalElement.logEvent("accepted", "removed")

      Interop.removeEventHandler <@ element.Accepted @> element)

  override _.name = $"MenuItem"

  override this.setProps(element: View, props: Props) =
    base.setProps (element, props)

    let element = element :?> MenuItem

    // Properties
    props

    |> Props.tryFind PKey.menuItem.command

    |> Option.iter (fun v -> 

      TerminalElement.logEvent("command", "set")

      element.Command <- v)

    props


    |> Props.tryFind PKey.menuItem.subMenu


    |> Option.iter (fun v -> 


      TerminalElement.logEvent("subMenu", "set")


      element.SubMenu <- v)

    props


    |> Props.tryFind PKey.menuItem.targetView


    |> Option.iter (fun v -> 


      TerminalElement.logEvent("targetView", "set")


      element.TargetView <- v)
    // Events
    props

    |> Props.tryFind PKey.menuItem.accepted

    |> Option.iter (fun v -> 

      TerminalElement.logEvent("accepted", "set")

      Interop.setEventHandler <@ element.Accepted @> v element)

  override this.SubElements_PropKeys =
    SubElementPropKey.from PKey.menuItem.subMenu_element
    :: base.SubElements_PropKeys


  override this.newView() = new MenuItem()

// NumericUpDown<'a>
type NumericUpDownElement<'a>(props: Props) =
  inherit TerminalElement(props)

  override this.removeProps(element: View, props: Props) =
    base.removeProps (element, props)
    let element = element :?> NumericUpDown<'a>
    // Properties
    props
    |> Props.tryFind PKey.numericUpDown<'a>.format
    |> Option.iter (fun _ -> element.Format <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.numericUpDown<'a>.increment
    |> Option.iter (fun _ -> element.Increment <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.numericUpDown<'a>.value
    |> Option.iter (fun _ -> element.Value <- Unchecked.defaultof<_>)
    // Events
    props
    |> Props.tryFind PKey.numericUpDown<'a>.formatChanged
    |> Option.iter (fun _ -> Interop.removeEventHandler <@ element.FormatChanged @> element)

    props
    |> Props.tryFind PKey.numericUpDown<'a>.incrementChanged
    |> Option.iter (fun _ -> Interop.removeEventHandler <@ element.IncrementChanged @> element)

    props
    |> Props.tryFind PKey.numericUpDown<'a>.valueChanged
    |> Option.iter (fun _ -> Interop.removeEventHandler <@ element.ValueChanged @> element)

    props
    |> Props.tryFind PKey.numericUpDown<'a>.valueChanging
    |> Option.iter (fun _ -> Interop.removeEventHandler <@ element.ValueChanging @> element)

  override _.name = $"NumericUpDown<'a>"

  override this.setProps(element: View, props: Props) =
    base.setProps (element, props)

    let element = element :?> NumericUpDown<'a>

    // Properties
    props
    |> Props.tryFind PKey.numericUpDown<'a>.format
    |> Option.iter (fun v -> element.Format <- v)

    props
    |> Props.tryFind PKey.numericUpDown<'a>.increment
    |> Option.iter (fun v -> element.Increment <- v)

    props
    |> Props.tryFind PKey.numericUpDown<'a>.value
    |> Option.iter (fun v -> element.Value <- v)
    // Events
    props
    |> Props.tryFind PKey.numericUpDown<'a>.formatChanged
    |> Option.iter (fun v -> Interop.setEventHandler <@ element.FormatChanged @> (fun arg -> v arg.Value) element)

    props
    |> Props.tryFind PKey.numericUpDown<'a>.incrementChanged
    |> Option.iter (fun v -> Interop.setEventHandler <@ element.IncrementChanged @> (fun arg -> v arg.Value) element)

    props
    |> Props.tryFind PKey.numericUpDown<'a>.valueChanged
    |> Option.iter (fun v -> Interop.setEventHandler <@ element.ValueChanged @> (fun arg -> v arg.Value) element)

    props
    |> Props.tryFind PKey.numericUpDown<'a>.valueChanging
    |> Option.iter (fun v -> Interop.setEventHandler <@ element.ValueChanging @> v element)

  override this.newView() = new NumericUpDown<'a>()


  interface INumericUpDownElement


// NumericUpDown
type NumericUpDownElement(props: Props) =
  inherit NumericUpDownElement<int>(props)

// OpenDialog
type OpenDialogElement(props: Props) =
  inherit TerminalElement(props)

  override this.removeProps(element: View, props: Props) =
    base.removeProps (element, props)
    let element = element :?> OpenDialog
    // Properties
    props

    |> Props.tryFind PKey.openDialog.openMode

    |> Option.iter (fun _ -> 

      TerminalElement.logEvent("openMode", "removed")

      element.OpenMode <- Unchecked.defaultof<_>)

  override _.name = $"OpenDialog"

  override this.setProps(element: View, props: Props) =
    base.setProps (element, props)

    let element = element :?> OpenDialog

    // Properties
    props

    |> Props.tryFind PKey.openDialog.openMode

    |> Option.iter (fun v -> 

      TerminalElement.logEvent("openMode", "set")

      element.OpenMode <- v)


  override this.newView() = new OpenDialog()

// TODO: replace direct orientation usage with this type in the Element.fs
type OrientationInterface =
  static member removeProps (element: IOrientation) (props: Props) =
    // Properties
    props

    |> Props.tryFind PKey.orientationInterface.orientation

    |> Option.iter (fun _ -> 

      TerminalElement.logEvent("orientation", "removed")

      element.Orientation <- Unchecked.defaultof<_>)

    // Events
    props

    |> Props.tryFind PKey.orientationInterface.orientationChanged

    |> Option.iter (fun _ -> 

      TerminalElement.logEvent("orientationChanged", "removed")

      Interop.removeEventHandler <@ element.OrientationChanged @> element)

    props


    |> Props.tryFind PKey.orientationInterface.orientationChanging


    |> Option.iter (fun _ -> 


      TerminalElement.logEvent("orientationChanging", "removed")


      Interop.removeEventHandler <@ element.OrientationChanging @> element)

  static member setProps (element: IOrientation) (props: Props) =
    // Properties
    props

    |> Props.tryFind PKey.orientationInterface.orientation

    |> Option.iter (fun v -> 

      TerminalElement.logEvent("orientation", "set")

      element.Orientation <- v)

    // Events
    props

    |> Props.tryFind PKey.orientationInterface.orientationChanged

    |> Option.iter (fun v -> 

      TerminalElement.logEvent("orientationChanged", "set")

      Interop.setEventHandler <@ element.OrientationChanged @> (fun arg -> v arg.Value) element)

    props


    |> Props.tryFind PKey.orientationInterface.orientationChanging


    |> Option.iter (fun v -> 


      TerminalElement.logEvent("orientationChanging", "set")


      Interop.setEventHandler <@ element.OrientationChanging @> v element)

// SelectorBase
type internal SelectorBaseElement(props: Props) =
  inherit TerminalElement(props)

  override this.removeProps(element: View, props: Props) =
    base.removeProps (element, props)
    let element = element :?> SelectorBase
    // Interfaces
    OrientationInterface.removeProps element props

    // Properties
    props

    |> Props.tryFind PKey.selectorBase.assignHotKeys

    |> Option.iter (fun _ -> 

      TerminalElement.logEvent("assignHotKeys", "removed")

      element.AssignHotKeys <- Unchecked.defaultof<_>)

    props


    |> Props.tryFind PKey.selectorBase.doubleClickAccepts


    |> Option.iter (fun _ -> 


      TerminalElement.logEvent("doubleClickAccepts", "removed")


      element.DoubleClickAccepts <- Unchecked.defaultof<_>)

    props


    |> Props.tryFind PKey.selectorBase.horizontalSpace


    |> Option.iter (fun _ -> 


      TerminalElement.logEvent("horizontalSpace", "removed")


      element.HorizontalSpace <- Unchecked.defaultof<_>)

    props


    |> Props.tryFind PKey.selectorBase.labels


    |> Option.iter (fun _ -> 


      TerminalElement.logEvent("labels", "removed")


      element.Labels <- Unchecked.defaultof<_>)

    props


    |> Props.tryFind PKey.selectorBase.styles


    |> Option.iter (fun _ -> 


      TerminalElement.logEvent("styles", "removed")


      element.Styles <- Unchecked.defaultof<_>)

    props


    |> Props.tryFind PKey.selectorBase.usedHotKeys


    |> Option.iter (fun _ -> 


      TerminalElement.logEvent("usedHotKeys", "removed")


      element.UsedHotKeys <- Unchecked.defaultof<_>)

    props


    |> Props.tryFind PKey.selectorBase.value


    |> Option.iter (fun _ -> 


      TerminalElement.logEvent("value", "removed")


      element.Value <- Unchecked.defaultof<_>)

    props


    |> Props.tryFind PKey.selectorBase.values


    |> Option.iter (fun _ -> 


      TerminalElement.logEvent("values", "removed")


      element.Values <- Unchecked.defaultof<_>)

    // Events
    props

    |> Props.tryFind PKey.selectorBase.valueChanged

    |> Option.iter (fun _ -> 

      TerminalElement.logEvent("valueChanged", "removed")

      Interop.removeEventHandler <@ element.ValueChanged @> element)

  override _.name = "SelectorBase"

  override this.setProps(element: View, props: Props) =
    base.setProps (element, props)

    let element = element :?> SelectorBase

    // Interfaces
    OrientationInterface.setProps element props

    // Properties
    props

    |> Props.tryFind PKey.selectorBase.assignHotKeys

    |> Option.iter (fun v -> 

      TerminalElement.logEvent("assignHotKeys", "set")

      element.AssignHotKeys <- v)

    props


    |> Props.tryFind PKey.selectorBase.doubleClickAccepts


    |> Option.iter (fun v -> 


      TerminalElement.logEvent("doubleClickAccepts", "set")


      element.DoubleClickAccepts <- v)

    props


    |> Props.tryFind PKey.selectorBase.horizontalSpace


    |> Option.iter (fun v -> 


      TerminalElement.logEvent("horizontalSpace", "set")


      element.HorizontalSpace <- v)

    props


    |> Props.tryFind PKey.selectorBase.labels


    |> Option.iter (fun v -> 


      TerminalElement.logEvent("labels", "set")


      element.Labels <- v)

    props


    |> Props.tryFind PKey.selectorBase.styles


    |> Option.iter (fun v -> 


      TerminalElement.logEvent("styles", "set")


      element.Styles <- v)

    props


    |> Props.tryFind PKey.selectorBase.usedHotKeys


    |> Option.iter (fun v -> 


      TerminalElement.logEvent("usedHotKeys", "set")


      element.UsedHotKeys <- v)

    props


    |> Props.tryFind PKey.selectorBase.value


    |> Option.iter (fun v -> 


      TerminalElement.logEvent("value", "set")


      element.Value <- v)

    props


    |> Props.tryFind PKey.selectorBase.values


    |> Option.iter (fun v -> 


      TerminalElement.logEvent("values", "set")


      element.Values <- v)

    // Events
    props

    |> Props.tryFind PKey.selectorBase.valueChanged

    |> Option.iter (fun v -> 

      TerminalElement.logEvent("valueChanged", "set")

      Interop.setEventHandler <@ element.ValueChanged @> v element)

  override this.newView() = raise (NotImplementedException())

  override this.canReuseView _ _ = raise (NotImplementedException())

// OptionSelector
type OptionSelectorElement(props: Props) =
  inherit SelectorBaseElement(props)

  override this.removeProps(element: View, props: Props) =
    base.removeProps (element, props)
    let element = element :?> OptionSelector

    // Properties
    props

    |> Props.tryFind PKey.optionSelector.cursor

    |> Option.iter (fun _ -> 

      TerminalElement.logEvent("cursor", "removed")

      element.Cursor <- Unchecked.defaultof<_>)

  override _.name = $"OptionSelector"

  override this.setProps(element: View, props: Props) =
    base.setProps (element, props)

    let element = element :?> OptionSelector

    // Properties
    props

    |> Props.tryFind PKey.optionSelector.cursor

    |> Option.iter (fun v -> 

      TerminalElement.logEvent("cursor", "set")

      element.Cursor <- v)

  override this.newView() = new OptionSelector()


// FlagSelector
type FlagSelectorElement(props: Props) =
  inherit SelectorBaseElement(props)

  override this.removeProps(element: View, props: Props) =
    base.removeProps (element, props)
    let element = element :?> FlagSelector

    // Properties
    props

    |> Props.tryFind PKey.flagSelector.value

    |> Option.iter (fun _ -> 

      TerminalElement.logEvent("value", "removed")

      element.Value <- Unchecked.defaultof<_>)

  override _.name = $"FlagSelector"

  override this.setProps(element: View, props: Props) =
    base.setProps (element, props)

    let element = element :?> FlagSelector

    // Properties
    props

    |> Props.tryFind PKey.flagSelector.value

    |> Option.iter (fun v -> 

      TerminalElement.logEvent("value", "set")

      element.Value <- v)

  override this.newView() = new FlagSelector()

// Padding
type PaddingElement(props: Props) =
  inherit TerminalElement(props)

  override this.removeProps(element: View, props: Props) = base.removeProps (element, props)

  override _.name = $"Padding"

  override this.setProps(element: View, props: Props) = base.setProps (element, props)


  override this.newView() = new Padding()

// ProgressBar
type ProgressBarElement(props: Props) =
  inherit TerminalElement(props)

  override this.removeProps(element: View, props: Props) =
    base.removeProps (element, props)
    let element = element :?> ProgressBar
    // Properties
    props

    |> Props.tryFind PKey.progressBar.bidirectionalMarquee

    |> Option.iter (fun _ -> 

      TerminalElement.logEvent("bidirectionalMarquee", "removed")

      element.BidirectionalMarquee <- Unchecked.defaultof<_>)

    props


    |> Props.tryFind PKey.progressBar.fraction


    |> Option.iter (fun _ -> 


      TerminalElement.logEvent("fraction", "removed")


      element.Fraction <- Unchecked.defaultof<_>)

    props


    |> Props.tryFind PKey.progressBar.progressBarFormat


    |> Option.iter (fun _ -> 


      TerminalElement.logEvent("progressBarFormat", "removed")


      element.ProgressBarFormat <- Unchecked.defaultof<_>)

    props


    |> Props.tryFind PKey.progressBar.progressBarStyle


    |> Option.iter (fun _ -> 


      TerminalElement.logEvent("progressBarStyle", "removed")


      element.ProgressBarStyle <- Unchecked.defaultof<_>)

    props


    |> Props.tryFind PKey.progressBar.segmentCharacter


    |> Option.iter (fun _ -> 


      TerminalElement.logEvent("segmentCharacter", "removed")


      element.SegmentCharacter <- Unchecked.defaultof<_>)

    props


    |> Props.tryFind PKey.progressBar.text


    |> Option.iter (fun _ -> 


      TerminalElement.logEvent("text", "removed")


      element.Text <- Unchecked.defaultof<_>)

  override _.name = $"ProgressBar"

  override this.setProps(element: View, props: Props) =
    base.setProps (element, props)

    let element = element :?> ProgressBar

    // Properties
    props

    |> Props.tryFind PKey.progressBar.bidirectionalMarquee

    |> Option.iter (fun v -> 

      TerminalElement.logEvent("bidirectionalMarquee", "set")

      element.BidirectionalMarquee <- v)

    props


    |> Props.tryFind PKey.progressBar.fraction


    |> Option.iter (fun v -> 


      TerminalElement.logEvent("fraction", "set")


      element.Fraction <- v)

    props


    |> Props.tryFind PKey.progressBar.progressBarFormat


    |> Option.iter (fun v -> 


      TerminalElement.logEvent("progressBarFormat", "set")


      element.ProgressBarFormat <- v)

    props


    |> Props.tryFind PKey.progressBar.progressBarStyle


    |> Option.iter (fun v -> 


      TerminalElement.logEvent("progressBarStyle", "set")


      element.ProgressBarStyle <- v)

    props


    |> Props.tryFind PKey.progressBar.segmentCharacter


    |> Option.iter (fun v -> 


      TerminalElement.logEvent("segmentCharacter", "set")


      element.SegmentCharacter <- v)

    props


    |> Props.tryFind PKey.progressBar.text


    |> Option.iter (fun v -> 


      TerminalElement.logEvent("text", "set")


      element.Text <- v)


  override this.newView() = new ProgressBar()

// SaveDialog
type SaveDialogElement(props: Props) =
  inherit TerminalElement(props)

  override this.removeProps(element: View, props: Props) = base.removeProps (element, props)
  // No properties or events SaveDialog

  override _.name = $"SaveDialog"

  override this.setProps(element: View, props: Props) = base.setProps (element, props)
  // No properties or events SaveDialog


  override this.newView() = new SaveDialog()

// ScrollBar
type ScrollBarElement(props: Props) =
  inherit TerminalElement(props)

  override this.removeProps(element: View, props: Props) =
    base.removeProps (element, props)
    let element = element :?> ScrollBar
    // Properties
    props

    |> Props.tryFind PKey.scrollBar.autoShow

    |> Option.iter (fun _ -> 

      TerminalElement.logEvent("autoShow", "removed")

      element.AutoShow <- Unchecked.defaultof<_>)

    props


    |> Props.tryFind PKey.scrollBar.increment


    |> Option.iter (fun _ -> 


      TerminalElement.logEvent("increment", "removed")


      element.Increment <- Unchecked.defaultof<_>)

    props


    |> Props.tryFind PKey.scrollBar.orientation


    |> Option.iter (fun _ -> 


      TerminalElement.logEvent("orientation", "removed")


      element.Orientation <- Unchecked.defaultof<_>)

    props


    |> Props.tryFind PKey.scrollBar.position


    |> Option.iter (fun _ -> 


      TerminalElement.logEvent("position", "removed")


      element.Position <- Unchecked.defaultof<_>)

    props


    |> Props.tryFind PKey.scrollBar.scrollableContentSize


    |> Option.iter (fun _ -> 


      TerminalElement.logEvent("scrollableContentSize", "removed")


      element.ScrollableContentSize <- Unchecked.defaultof<_>)

    props


    |> Props.tryFind PKey.scrollBar.visibleContentSize


    |> Option.iter (fun _ -> 


      TerminalElement.logEvent("visibleContentSize", "removed")


      element.VisibleContentSize <- Unchecked.defaultof<_>)
    // Events
    props

    |> Props.tryFind PKey.scrollBar.orientationChanged

    |> Option.iter (fun _ -> 

      TerminalElement.logEvent("orientationChanged", "removed")

      Interop.removeEventHandler <@ element.OrientationChanged @> element)

    props


    |> Props.tryFind PKey.scrollBar.orientationChanging


    |> Option.iter (fun _ -> 


      TerminalElement.logEvent("orientationChanging", "removed")


      Interop.removeEventHandler <@ element.OrientationChanging @> element)

    props


    |> Props.tryFind PKey.scrollBar.scrollableContentSizeChanged


    |> Option.iter (fun _ -> 


      TerminalElement.logEvent("scrollableContentSizeChanged", "removed")


      Interop.removeEventHandler <@ element.ScrollableContentSizeChanged @> element)

    props


    |> Props.tryFind PKey.scrollBar.sliderPositionChanged


    |> Option.iter (fun _ -> 


      TerminalElement.logEvent("sliderPositionChanged", "removed")


      Interop.removeEventHandler <@ element.SliderPositionChanged @> element)

  override _.name = $"ScrollBar"

  override this.setProps(element: View, props: Props) =
    base.setProps (element, props)

    let element = element :?> ScrollBar

    // Properties
    props

    |> Props.tryFind PKey.scrollBar.autoShow

    |> Option.iter (fun v -> 

      TerminalElement.logEvent("autoShow", "set")

      element.AutoShow <- v)

    props


    |> Props.tryFind PKey.scrollBar.increment


    |> Option.iter (fun v -> 


      TerminalElement.logEvent("increment", "set")


      element.Increment <- v)

    props


    |> Props.tryFind PKey.scrollBar.orientation


    |> Option.iter (fun v -> 


      TerminalElement.logEvent("orientation", "set")


      element.Orientation <- v)

    props


    |> Props.tryFind PKey.scrollBar.position


    |> Option.iter (fun v -> 


      TerminalElement.logEvent("position", "set")


      element.Position <- v)

    props


    |> Props.tryFind PKey.scrollBar.scrollableContentSize


    |> Option.iter (fun v -> 


      TerminalElement.logEvent("scrollableContentSize", "set")


      element.ScrollableContentSize <- v)

    props


    |> Props.tryFind PKey.scrollBar.visibleContentSize


    |> Option.iter (fun v -> 


      TerminalElement.logEvent("visibleContentSize", "set")


      element.VisibleContentSize <- v)
    // Events
    props

    |> Props.tryFind PKey.scrollBar.orientationChanged

    |> Option.iter (fun v -> 

      TerminalElement.logEvent("orientationChanged", "set")

      Interop.setEventHandler <@ element.OrientationChanged @> (fun arg -> v arg.Value) element)

    props


    |> Props.tryFind PKey.scrollBar.orientationChanging


    |> Option.iter (fun v -> 


      TerminalElement.logEvent("orientationChanging", "set")


      Interop.setEventHandler <@ element.OrientationChanging @> v element)

    props


    |> Props.tryFind PKey.scrollBar.scrollableContentSizeChanged


    |> Option.iter (fun v -> 


      TerminalElement.logEvent("scrollableContentSizeChanged", "set")


      Interop.setEventHandler <@ element.ScrollableContentSizeChanged @> v element)

    props


    |> Props.tryFind PKey.scrollBar.sliderPositionChanged


    |> Option.iter (fun v -> 


      TerminalElement.logEvent("sliderPositionChanged", "set")


      Interop.setEventHandler <@ element.SliderPositionChanged @> v element)


  override this.newView() = new ScrollBar()

// ScrollSlider
type ScrollSliderElement(props: Props) =
  inherit TerminalElement(props)

  override this.removeProps(element: View, props: Props) =
    base.removeProps (element, props)
    let element = element :?> ScrollSlider
    // Properties
    props

    |> Props.tryFind PKey.scrollSlider.orientation

    |> Option.iter (fun _ -> 

      TerminalElement.logEvent("orientation", "removed")

      element.Orientation <- Unchecked.defaultof<_>)

    props


    |> Props.tryFind PKey.scrollSlider.position


    |> Option.iter (fun _ -> 


      TerminalElement.logEvent("position", "removed")


      element.Position <- Unchecked.defaultof<_>)

    props


    |> Props.tryFind PKey.scrollSlider.size


    |> Option.iter (fun _ -> 


      TerminalElement.logEvent("size", "removed")


      element.Size <- Unchecked.defaultof<_>)

    props


    |> Props.tryFind PKey.scrollSlider.sliderPadding


    |> Option.iter (fun _ -> 


      TerminalElement.logEvent("sliderPadding", "removed")


      element.SliderPadding <- Unchecked.defaultof<_>)

    props


    |> Props.tryFind PKey.scrollSlider.visibleContentSize


    |> Option.iter (fun _ -> 


      TerminalElement.logEvent("visibleContentSize", "removed")


      element.VisibleContentSize <- Unchecked.defaultof<_>)
    // Events
    props

    |> Props.tryFind PKey.scrollSlider.orientationChanged

    |> Option.iter (fun _ -> 

      TerminalElement.logEvent("orientationChanged", "removed")

      Interop.removeEventHandler <@ element.OrientationChanged @> element)

    props


    |> Props.tryFind PKey.scrollSlider.orientationChanging


    |> Option.iter (fun _ -> 


      TerminalElement.logEvent("orientationChanging", "removed")


      Interop.removeEventHandler <@ element.OrientationChanging @> element)

    props


    |> Props.tryFind PKey.scrollSlider.positionChanged


    |> Option.iter (fun _ -> 


      TerminalElement.logEvent("positionChanged", "removed")


      Interop.removeEventHandler <@ element.PositionChanged @> element)

    props


    |> Props.tryFind PKey.scrollSlider.positionChanging


    |> Option.iter (fun _ -> 


      TerminalElement.logEvent("positionChanging", "removed")


      Interop.removeEventHandler <@ element.PositionChanging @> element)

    props


    |> Props.tryFind PKey.scrollSlider.scrolled


    |> Option.iter (fun _ -> 


      TerminalElement.logEvent("scrolled", "removed")


      Interop.removeEventHandler <@ element.Scrolled @> element)

  override _.name = $"ScrollSlider"

  override this.setProps(element: View, props: Props) =
    base.setProps (element, props)

    let element = element :?> ScrollSlider

    // Properties
    props

    |> Props.tryFind PKey.scrollSlider.orientation

    |> Option.iter (fun v -> 

      TerminalElement.logEvent("orientation", "set")

      element.Orientation <- v)

    props


    |> Props.tryFind PKey.scrollSlider.position


    |> Option.iter (fun v -> 


      TerminalElement.logEvent("position", "set")


      element.Position <- v)

    props


    |> Props.tryFind PKey.scrollSlider.size


    |> Option.iter (fun v -> 


      TerminalElement.logEvent("size", "set")


      element.Size <- v)

    props


    |> Props.tryFind PKey.scrollSlider.sliderPadding


    |> Option.iter (fun v -> 


      TerminalElement.logEvent("sliderPadding", "set")


      element.SliderPadding <- v)

    props


    |> Props.tryFind PKey.scrollSlider.visibleContentSize


    |> Option.iter (fun v -> 


      TerminalElement.logEvent("visibleContentSize", "set")


      element.VisibleContentSize <- v)
    // Events
    props

    |> Props.tryFind PKey.scrollSlider.orientationChanged

    |> Option.iter (fun v -> 

      TerminalElement.logEvent("orientationChanged", "set")

      Interop.setEventHandler <@ element.OrientationChanged @> (fun arg -> v arg.Value) element)

    props


    |> Props.tryFind PKey.scrollSlider.orientationChanging


    |> Option.iter (fun v -> 


      TerminalElement.logEvent("orientationChanging", "set")


      Interop.setEventHandler <@ element.OrientationChanging @> v element)

    props


    |> Props.tryFind PKey.scrollSlider.positionChanged


    |> Option.iter (fun v -> 


      TerminalElement.logEvent("positionChanged", "set")


      Interop.setEventHandler <@ element.PositionChanged @> v element)

    props


    |> Props.tryFind PKey.scrollSlider.positionChanging


    |> Option.iter (fun v -> 


      TerminalElement.logEvent("positionChanging", "set")


      Interop.setEventHandler <@ element.PositionChanging @> v element)

    props


    |> Props.tryFind PKey.scrollSlider.scrolled


    |> Option.iter (fun v -> 


      TerminalElement.logEvent("scrolled", "set")


      Interop.setEventHandler <@ element.Scrolled @> v element)


  override this.newView() = new ScrollSlider()


// Slider<'a>
type SliderElement<'a>(props: Props) =
  inherit TerminalElement(props)

  override this.removeProps(element: View, props: Props) =
    base.removeProps (element, props)
    let element = element :?> Slider<'a>
    // Properties
    props
    |> Props.tryFind PKey.slider<'a>.allowEmpty
    |> Option.iter (fun _ -> element.AllowEmpty <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.slider<'a>.focusedOption
    |> Option.iter (fun _ -> element.FocusedOption <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.slider<'a>.legendsOrientation
    |> Option.iter (fun _ -> element.LegendsOrientation <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.slider<'a>.minimumInnerSpacing
    |> Option.iter (fun _ -> element.MinimumInnerSpacing <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.slider<'a>.options
    |> Option.iter (fun _ -> element.Options <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.slider<'a>.orientation
    |> Option.iter (fun _ -> element.Orientation <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.slider<'a>.rangeAllowSingle
    |> Option.iter (fun _ -> element.RangeAllowSingle <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.slider<'a>.showEndSpacing
    |> Option.iter (fun _ -> element.ShowEndSpacing <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.slider<'a>.showLegends
    |> Option.iter (fun _ -> element.ShowLegends <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.slider<'a>.style
    |> Option.iter (fun _ -> element.Style <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.slider<'a>.text
    |> Option.iter (fun _ -> element.Text <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.slider<'a>.``type``
    |> Option.iter (fun _ -> element.Type <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.slider<'a>.useMinimumSize
    |> Option.iter (fun _ -> element.UseMinimumSize <- Unchecked.defaultof<_>)
    // Events
    props

    |> Props.tryFind PKey.slider.optionFocused

    |> Option.iter (fun _ -> 

      TerminalElement.logEvent("optionFocused", "removed")

      Interop.removeEventHandler <@ element.OptionFocused @> element)

    props


    |> Props.tryFind PKey.slider.optionsChanged


    |> Option.iter (fun _ -> 


      TerminalElement.logEvent("optionsChanged", "removed")


      Interop.removeEventHandler <@ element.OptionsChanged @> element)

    props


    |> Props.tryFind PKey.slider.orientationChanged


    |> Option.iter (fun _ -> 


      TerminalElement.logEvent("orientationChanged", "removed")


      Interop.removeEventHandler <@ element.OrientationChanged @> element)

    props


    |> Props.tryFind PKey.slider.orientationChanging


    |> Option.iter (fun _ -> 


      TerminalElement.logEvent("orientationChanging", "removed")


      Interop.removeEventHandler <@ element.OrientationChanging @> element)

  override _.name = $"Slider<'a>"

  override this.setProps(element: View, props: Props) =
    base.setProps (element, props)

    let element = element :?> Slider<'a>

    // Properties
    props

    |> Props.tryFind PKey.slider.allowEmpty

    |> Option.iter (fun v -> 

      TerminalElement.logEvent("allowEmpty", "set")

      element.AllowEmpty <- v)

    props


    |> Props.tryFind PKey.slider.focusedOption


    |> Option.iter (fun v -> 


      TerminalElement.logEvent("focusedOption", "set")


      element.FocusedOption <- v)

    props


    |> Props.tryFind PKey.slider.legendsOrientation


    |> Option.iter (fun v -> 


      TerminalElement.logEvent("legendsOrientation", "set")


      element.LegendsOrientation <- v)

    props


    |> Props.tryFind PKey.slider.minimumInnerSpacing


    |> Option.iter (fun v -> 


      TerminalElement.logEvent("minimumInnerSpacing", "set")


      element.MinimumInnerSpacing <- v)

    props
    |> Props.tryFind PKey.slider.options
    |> Option.iter (fun v -> element.Options <- List<_>(v))

    props


    |> Props.tryFind PKey.slider.orientation


    |> Option.iter (fun v -> 


      TerminalElement.logEvent("orientation", "set")


      element.Orientation <- v)

    props


    |> Props.tryFind PKey.slider.rangeAllowSingle


    |> Option.iter (fun v -> 


      TerminalElement.logEvent("rangeAllowSingle", "set")


      element.RangeAllowSingle <- v)

    props


    |> Props.tryFind PKey.slider.showEndSpacing


    |> Option.iter (fun v -> 


      TerminalElement.logEvent("showEndSpacing", "set")


      element.ShowEndSpacing <- v)

    props


    |> Props.tryFind PKey.slider.showLegends


    |> Option.iter (fun v -> 


      TerminalElement.logEvent("showLegends", "set")


      element.ShowLegends <- v)

    props


    |> Props.tryFind PKey.slider.style


    |> Option.iter (fun v -> 


      TerminalElement.logEvent("style", "set")


      element.Style <- v)

    props


    |> Props.tryFind PKey.slider.text


    |> Option.iter (fun v -> 


      TerminalElement.logEvent("text", "set")


      element.Text <- v)

    props
    |> Props.tryFind PKey.slider.``type``
    |> Option.iter (fun v -> element.Type <- v)

    props


    |> Props.tryFind PKey.slider.useMinimumSize


    |> Option.iter (fun v -> 


      TerminalElement.logEvent("useMinimumSize", "set")


      element.UseMinimumSize <- v)
    // Events
    props

    |> Props.tryFind PKey.slider.optionFocused

    |> Option.iter (fun v -> 

      TerminalElement.logEvent("optionFocused", "set")

      Interop.setEventHandler <@ element.OptionFocused @> v element)

    props


    |> Props.tryFind PKey.slider.optionsChanged


    |> Option.iter (fun v -> 


      TerminalElement.logEvent("optionsChanged", "set")


      Interop.setEventHandler <@ element.OptionsChanged @> v element)

    props


    |> Props.tryFind PKey.slider.orientationChanged


    |> Option.iter (fun v -> 


      TerminalElement.logEvent("orientationChanged", "set")


      Interop.setEventHandler <@ element.OrientationChanged @> (fun arg -> v arg.Value) element)

    props


    |> Props.tryFind PKey.slider.orientationChanging


    |> Option.iter (fun v -> 


      TerminalElement.logEvent("orientationChanging", "set")


      Interop.setEventHandler <@ element.OrientationChanging @> v element)


  override this.newView() = new Slider<'a>()


  interface ISliderElement

// Slider
type SliderElement(props: Props) =
  inherit TerminalElement(props)

  override this.removeProps(element: View, props: Props) = base.removeProps (element, props)
  // No properties or events Slider

  override _.name = $"Slider"

  override this.setProps(element: View, props: Props) = base.setProps (element, props)
  // No properties or events Slider

  override this.newView() = new Slider()

// SpinnerView
type SpinnerViewElement(props: Props) =
  inherit TerminalElement(props)

  override this.removeProps(element: View, props: Props) =
    base.removeProps (element, props)
    let element = element :?> SpinnerView
    // Properties
    props

    |> Props.tryFind PKey.spinnerView.autoSpin

    |> Option.iter (fun _ -> 

      TerminalElement.logEvent("autoSpin", "removed")

      element.AutoSpin <- Unchecked.defaultof<_>)

    props


    |> Props.tryFind PKey.spinnerView.sequence


    |> Option.iter (fun _ -> 


      TerminalElement.logEvent("sequence", "removed")


      element.Sequence <- Unchecked.defaultof<_>)

    props


    |> Props.tryFind PKey.spinnerView.spinBounce


    |> Option.iter (fun _ -> 


      TerminalElement.logEvent("spinBounce", "removed")


      element.SpinBounce <- Unchecked.defaultof<_>)

    props


    |> Props.tryFind PKey.spinnerView.spinDelay


    |> Option.iter (fun _ -> 


      TerminalElement.logEvent("spinDelay", "removed")


      element.SpinDelay <- Unchecked.defaultof<_>)

    props


    |> Props.tryFind PKey.spinnerView.spinReverse


    |> Option.iter (fun _ -> 


      TerminalElement.logEvent("spinReverse", "removed")


      element.SpinReverse <- Unchecked.defaultof<_>)

    props


    |> Props.tryFind PKey.spinnerView.style


    |> Option.iter (fun _ -> 


      TerminalElement.logEvent("style", "removed")


      element.Style <- Unchecked.defaultof<_>)

  override _.name = $"SpinnerView"

  override this.setProps(element: View, props: Props) =
    base.setProps (element, props)

    let element = element :?> SpinnerView

    // Properties
    props

    |> Props.tryFind PKey.spinnerView.autoSpin

    |> Option.iter (fun v -> 

      TerminalElement.logEvent("autoSpin", "set")

      element.AutoSpin <- v)

    props
    |> Props.tryFind PKey.spinnerView.sequence
    |> Option.iter (fun v -> element.Sequence <- v |> List.toArray)

    props


    |> Props.tryFind PKey.spinnerView.spinBounce


    |> Option.iter (fun v -> 


      TerminalElement.logEvent("spinBounce", "set")


      element.SpinBounce <- v)

    props


    |> Props.tryFind PKey.spinnerView.spinDelay


    |> Option.iter (fun v -> 


      TerminalElement.logEvent("spinDelay", "set")


      element.SpinDelay <- v)

    props


    |> Props.tryFind PKey.spinnerView.spinReverse


    |> Option.iter (fun v -> 


      TerminalElement.logEvent("spinReverse", "set")


      element.SpinReverse <- v)

    props


    |> Props.tryFind PKey.spinnerView.style


    |> Option.iter (fun v -> 


      TerminalElement.logEvent("style", "set")


      element.Style <- v)


  override this.newView() = new SpinnerView()

// StatusBar
type StatusBarElement(props: Props) =
  inherit TerminalElement(props)

  override this.removeProps(element: View, props: Props) = base.removeProps (element, props)
  // No properties or events StatusBar

  override _.name = $"StatusBar"

  override this.setProps(element: View, props: Props) = base.setProps (element, props)
  // No properties or events StatusBar


  override this.newView() = new StatusBar()

// Tab
type TabElement(props: Props) =
  inherit TerminalElement(props)

  override this.removeProps(element: View, props: Props) =
    base.removeProps (element, props)
    let element = element :?> Tab
    // Properties
    props

    |> Props.tryFind PKey.tab.displayText

    |> Option.iter (fun _ -> 

      TerminalElement.logEvent("displayText", "removed")

      element.DisplayText <- Unchecked.defaultof<_>)

    props


    |> Props.tryFind PKey.tab.view


    |> Option.iter (fun _ -> 


      TerminalElement.logEvent("view", "removed")


      element.View <- Unchecked.defaultof<_>)

  override _.name = $"Tab"

  override this.setProps(element: View, props: Props) =
    base.setProps (element, props)

    let element = element :?> Tab

    // Properties
    props

    |> Props.tryFind PKey.tab.displayText

    |> Option.iter (fun v -> 

      TerminalElement.logEvent("displayText", "set")

      element.DisplayText <- v)

  override this.SubElements_PropKeys =
    SubElementPropKey.from PKey.tab.view_element
    :: base.SubElements_PropKeys

  override this.newView() = new Tab()

// TabView
type TabViewElement(props: Props) =
  inherit TerminalElement(props)


  override this.removeProps(element: View, props: Props) =
    base.removeProps (element, props)
    let element = element :?> TabView
    // Properties
    props

    |> Props.tryFind PKey.tabView.maxTabTextWidth

    |> Option.iter (fun _ -> 

      TerminalElement.logEvent("maxTabTextWidth", "removed")

      element.MaxTabTextWidth <- Unchecked.defaultof<_>)

    props


    |> Props.tryFind PKey.tabView.selectedTab


    |> Option.iter (fun _ -> 


      TerminalElement.logEvent("selectedTab", "removed")


      element.SelectedTab <- Unchecked.defaultof<_>)

    props


    |> Props.tryFind PKey.tabView.style


    |> Option.iter (fun _ -> 


      TerminalElement.logEvent("style", "removed")


      element.Style <- Unchecked.defaultof<_>)

    props


    |> Props.tryFind PKey.tabView.tabScrollOffset


    |> Option.iter (fun _ -> 


      TerminalElement.logEvent("tabScrollOffset", "removed")


      element.TabScrollOffset <- Unchecked.defaultof<_>)
    // Events
    props

    |> Props.tryFind PKey.tabView.selectedTabChanged

    |> Option.iter (fun _ -> 

      TerminalElement.logEvent("selectedTabChanged", "removed")

      Interop.removeEventHandler <@ element.SelectedTabChanged @> element)

    props


    |> Props.tryFind PKey.tabView.tabClicked


    |> Option.iter (fun _ -> 


      TerminalElement.logEvent("tabClicked", "removed")


      Interop.removeEventHandler <@ element.TabClicked @> element)

  override _.name = $"TabView"

  override this.setProps(element: View, props: Props) =
    base.setProps (element, props)

    let element = element :?> TabView

    // Properties
    props
    |> Props.tryFind PKey.tabView.maxTabTextWidth
    |> Option.iter (fun v -> element.MaxTabTextWidth <- (v |> uint32))

    props


    |> Props.tryFind PKey.tabView.selectedTab


    |> Option.iter (fun v -> 


      TerminalElement.logEvent("selectedTab", "set")


      element.SelectedTab <- v)

    props


    |> Props.tryFind PKey.tabView.style


    |> Option.iter (fun v -> 


      TerminalElement.logEvent("style", "set")


      element.Style <- v)

    props


    |> Props.tryFind PKey.tabView.tabScrollOffset


    |> Option.iter (fun v -> 


      TerminalElement.logEvent("tabScrollOffset", "set")


      element.TabScrollOffset <- v)
    // Events
    props

    |> Props.tryFind PKey.tabView.selectedTabChanged

    |> Option.iter (fun v -> 

      TerminalElement.logEvent("selectedTabChanged", "set")

      Interop.setEventHandler <@ element.SelectedTabChanged @> v element)

    props


    |> Props.tryFind PKey.tabView.tabClicked


    |> Option.iter (fun v -> 


      TerminalElement.logEvent("tabClicked", "set")


      Interop.setEventHandler <@ element.TabClicked @> v element)

    // Additional properties
    props
    |> Props.tryFind PKey.tabView.tabs
    |> Option.iter (fun tabItems ->
      tabItems
      |> Seq.iter (fun tabItem -> element.AddTab((tabItem :?> IInternalTerminalElement).view :?> Tab, false))
    )

  override this.SubElements_PropKeys =
    SubElementPropKey.from PKey.tabView.tabs_elements
    :: base.SubElements_PropKeys

  override this.newView() = new TabView()

// TableView
type TableViewElement(props: Props) =
  inherit TerminalElement(props)

  override this.removeProps(element: View, props: Props) =
    base.removeProps (element, props)
    let element = element :?> TableView
    // Properties
    props

    |> Props.tryFind PKey.tableView.cellActivationKey

    |> Option.iter (fun _ -> 

      TerminalElement.logEvent("cellActivationKey", "removed")

      element.CellActivationKey <- Unchecked.defaultof<_>)

    props


    |> Props.tryFind PKey.tableView.collectionNavigator


    |> Option.iter (fun _ -> 


      TerminalElement.logEvent("collectionNavigator", "removed")


      element.CollectionNavigator <- Unchecked.defaultof<_>)

    props


    |> Props.tryFind PKey.tableView.columnOffset


    |> Option.iter (fun _ -> 


      TerminalElement.logEvent("columnOffset", "removed")


      element.ColumnOffset <- Unchecked.defaultof<_>)

    props


    |> Props.tryFind PKey.tableView.fullRowSelect


    |> Option.iter (fun _ -> 


      TerminalElement.logEvent("fullRowSelect", "removed")


      element.FullRowSelect <- Unchecked.defaultof<_>)

    props


    |> Props.tryFind PKey.tableView.maxCellWidth


    |> Option.iter (fun _ -> 


      TerminalElement.logEvent("maxCellWidth", "removed")


      element.MaxCellWidth <- Unchecked.defaultof<_>)

    props


    |> Props.tryFind PKey.tableView.minCellWidth


    |> Option.iter (fun _ -> 


      TerminalElement.logEvent("minCellWidth", "removed")


      element.MinCellWidth <- Unchecked.defaultof<_>)

    props


    |> Props.tryFind PKey.tableView.multiSelect


    |> Option.iter (fun _ -> 


      TerminalElement.logEvent("multiSelect", "removed")


      element.MultiSelect <- Unchecked.defaultof<_>)

    props


    |> Props.tryFind PKey.tableView.nullSymbol


    |> Option.iter (fun _ -> 


      TerminalElement.logEvent("nullSymbol", "removed")


      element.NullSymbol <- Unchecked.defaultof<_>)

    props


    |> Props.tryFind PKey.tableView.rowOffset


    |> Option.iter (fun _ -> 


      TerminalElement.logEvent("rowOffset", "removed")


      element.RowOffset <- Unchecked.defaultof<_>)

    props


    |> Props.tryFind PKey.tableView.selectedColumn


    |> Option.iter (fun _ -> 


      TerminalElement.logEvent("selectedColumn", "removed")


      element.SelectedColumn <- Unchecked.defaultof<_>)

    props


    |> Props.tryFind PKey.tableView.selectedRow


    |> Option.iter (fun _ -> 


      TerminalElement.logEvent("selectedRow", "removed")


      element.SelectedRow <- Unchecked.defaultof<_>)

    props


    |> Props.tryFind PKey.tableView.separatorSymbol


    |> Option.iter (fun _ -> 


      TerminalElement.logEvent("separatorSymbol", "removed")


      element.SeparatorSymbol <- Unchecked.defaultof<_>)

    props


    |> Props.tryFind PKey.tableView.style


    |> Option.iter (fun _ -> 


      TerminalElement.logEvent("style", "removed")


      element.Style <- Unchecked.defaultof<_>)

    props


    |> Props.tryFind PKey.tableView.table


    |> Option.iter (fun _ -> 


      TerminalElement.logEvent("table", "removed")


      element.Table <- Unchecked.defaultof<_>)
    // Events
    props

    |> Props.tryFind PKey.tableView.cellActivated

    |> Option.iter (fun _ -> 

      TerminalElement.logEvent("cellActivated", "removed")

      Interop.removeEventHandler <@ element.CellActivated @> element)

    props


    |> Props.tryFind PKey.tableView.cellToggled


    |> Option.iter (fun _ -> 


      TerminalElement.logEvent("cellToggled", "removed")


      Interop.removeEventHandler <@ element.CellToggled @> element)

    props


    |> Props.tryFind PKey.tableView.selectedCellChanged


    |> Option.iter (fun _ -> 


      TerminalElement.logEvent("selectedCellChanged", "removed")


      Interop.removeEventHandler <@ element.SelectedCellChanged @> element)

  override _.name = $"TableView"

  override this.setProps(element: View, props: Props) =
    base.setProps (element, props)

    let element = element :?> TableView

    // Properties
    props

    |> Props.tryFind PKey.tableView.cellActivationKey

    |> Option.iter (fun v -> 

      TerminalElement.logEvent("cellActivationKey", "set")

      element.CellActivationKey <- v)

    props


    |> Props.tryFind PKey.tableView.collectionNavigator


    |> Option.iter (fun v -> 


      TerminalElement.logEvent("collectionNavigator", "set")


      element.CollectionNavigator <- v)

    props


    |> Props.tryFind PKey.tableView.columnOffset


    |> Option.iter (fun v -> 


      TerminalElement.logEvent("columnOffset", "set")


      element.ColumnOffset <- v)

    props


    |> Props.tryFind PKey.tableView.fullRowSelect


    |> Option.iter (fun v -> 


      TerminalElement.logEvent("fullRowSelect", "set")


      element.FullRowSelect <- v)

    props


    |> Props.tryFind PKey.tableView.maxCellWidth


    |> Option.iter (fun v -> 


      TerminalElement.logEvent("maxCellWidth", "set")


      element.MaxCellWidth <- v)

    props


    |> Props.tryFind PKey.tableView.minCellWidth


    |> Option.iter (fun v -> 


      TerminalElement.logEvent("minCellWidth", "set")


      element.MinCellWidth <- v)

    props


    |> Props.tryFind PKey.tableView.multiSelect


    |> Option.iter (fun v -> 


      TerminalElement.logEvent("multiSelect", "set")


      element.MultiSelect <- v)

    props


    |> Props.tryFind PKey.tableView.nullSymbol


    |> Option.iter (fun v -> 


      TerminalElement.logEvent("nullSymbol", "set")


      element.NullSymbol <- v)

    props


    |> Props.tryFind PKey.tableView.rowOffset


    |> Option.iter (fun v -> 


      TerminalElement.logEvent("rowOffset", "set")


      element.RowOffset <- v)

    props


    |> Props.tryFind PKey.tableView.selectedColumn


    |> Option.iter (fun v -> 


      TerminalElement.logEvent("selectedColumn", "set")


      element.SelectedColumn <- v)

    props


    |> Props.tryFind PKey.tableView.selectedRow


    |> Option.iter (fun v -> 


      TerminalElement.logEvent("selectedRow", "set")


      element.SelectedRow <- v)

    props


    |> Props.tryFind PKey.tableView.separatorSymbol


    |> Option.iter (fun v -> 


      TerminalElement.logEvent("separatorSymbol", "set")


      element.SeparatorSymbol <- v)

    props


    |> Props.tryFind PKey.tableView.style


    |> Option.iter (fun v -> 


      TerminalElement.logEvent("style", "set")


      element.Style <- v)

    props


    |> Props.tryFind PKey.tableView.table


    |> Option.iter (fun v -> 


      TerminalElement.logEvent("table", "set")


      element.Table <- v)
    // Events
    props

    |> Props.tryFind PKey.tableView.cellActivated

    |> Option.iter (fun v -> 

      TerminalElement.logEvent("cellActivated", "set")

      Interop.setEventHandler <@ element.CellActivated @> v element)

    props


    |> Props.tryFind PKey.tableView.cellToggled


    |> Option.iter (fun v -> 


      TerminalElement.logEvent("cellToggled", "set")


      Interop.setEventHandler <@ element.CellToggled @> v element)

    props


    |> Props.tryFind PKey.tableView.selectedCellChanged


    |> Option.iter (fun v -> 


      TerminalElement.logEvent("selectedCellChanged", "set")


      Interop.setEventHandler <@ element.SelectedCellChanged @> v element)


  override this.newView() = new TableView()

// TextField
type TextFieldElement(props: Props) =
  inherit TerminalElement(props)

  override this.removeProps(element: View, props: Props) =
    base.removeProps (element, props)
    let element = element :?> TextField
    // Properties
    props

    |> Props.tryFind PKey.textField.autocomplete

    |> Option.iter (fun _ -> 

      TerminalElement.logEvent("autocomplete", "removed")

      element.Autocomplete <- Unchecked.defaultof<_>)

    props


    |> Props.tryFind PKey.textField.cursorPosition


    |> Option.iter (fun _ -> 


      TerminalElement.logEvent("cursorPosition", "removed")


      element.CursorPosition <- Unchecked.defaultof<_>)

    props


    |> Props.tryFind PKey.textField.readOnly


    |> Option.iter (fun _ -> 


      TerminalElement.logEvent("readOnly", "removed")


      element.ReadOnly <- Unchecked.defaultof<_>)

    props


    |> Props.tryFind PKey.textField.secret


    |> Option.iter (fun _ -> 


      TerminalElement.logEvent("secret", "removed")


      element.Secret <- Unchecked.defaultof<_>)

    props


    |> Props.tryFind PKey.textField.selectedStart


    |> Option.iter (fun _ -> 


      TerminalElement.logEvent("selectedStart", "removed")


      element.SelectedStart <- Unchecked.defaultof<_>)

    props


    |> Props.tryFind PKey.textField.selectWordOnlyOnDoubleClick


    |> Option.iter (fun _ -> 


      TerminalElement.logEvent("selectWordOnlyOnDoubleClick", "removed")


      element.SelectWordOnlyOnDoubleClick <- Unchecked.defaultof<_>)

    props


    |> Props.tryFind PKey.textField.text


    |> Option.iter (fun _ -> 


      TerminalElement.logEvent("text", "removed")


      element.Text <- Unchecked.defaultof<_>)

    props


    |> Props.tryFind PKey.textField.used


    |> Option.iter (fun _ -> 


      TerminalElement.logEvent("used", "removed")


      element.Used <- Unchecked.defaultof<_>)

    props


    |> Props.tryFind PKey.textField.useSameRuneTypeForWords


    |> Option.iter (fun _ -> 


      TerminalElement.logEvent("useSameRuneTypeForWords", "removed")


      element.UseSameRuneTypeForWords <- Unchecked.defaultof<_>)
    // Events
    props

    |> Props.tryFind PKey.textField.textChanging

    |> Option.iter (fun _ -> 

      TerminalElement.logEvent("textChanging", "removed")

      Interop.removeEventHandler <@ element.TextChanging @> element)

  override _.name = $"TextField"

  override this.setProps(element: View, props: Props) =
    base.setProps (element, props)

    let element = element :?> TextField

    // Properties
    props

    |> Props.tryFind PKey.textField.autocomplete

    |> Option.iter (fun v -> 

      TerminalElement.logEvent("autocomplete", "set")

      element.Autocomplete <- v)

    props


    |> Props.tryFind PKey.textField.cursorPosition


    |> Option.iter (fun v -> 


      TerminalElement.logEvent("cursorPosition", "set")


      element.CursorPosition <- v)

    props


    |> Props.tryFind PKey.textField.readOnly


    |> Option.iter (fun v -> 


      TerminalElement.logEvent("readOnly", "set")


      element.ReadOnly <- v)

    props


    |> Props.tryFind PKey.textField.secret


    |> Option.iter (fun v -> 


      TerminalElement.logEvent("secret", "set")


      element.Secret <- v)

    props


    |> Props.tryFind PKey.textField.selectedStart


    |> Option.iter (fun v -> 


      TerminalElement.logEvent("selectedStart", "set")


      element.SelectedStart <- v)

    props


    |> Props.tryFind PKey.textField.selectWordOnlyOnDoubleClick


    |> Option.iter (fun v -> 


      TerminalElement.logEvent("selectWordOnlyOnDoubleClick", "set")


      element.SelectWordOnlyOnDoubleClick <- v)

    props


    |> Props.tryFind PKey.textField.text


    |> Option.iter (fun v -> 


      TerminalElement.logEvent("text", "set")


      element.Text <- v)

    props


    |> Props.tryFind PKey.textField.used


    |> Option.iter (fun v -> 


      TerminalElement.logEvent("used", "set")


      element.Used <- v)

    props


    |> Props.tryFind PKey.textField.useSameRuneTypeForWords


    |> Option.iter (fun v -> 


      TerminalElement.logEvent("useSameRuneTypeForWords", "set")


      element.UseSameRuneTypeForWords <- v)
    // Events
    props

    |> Props.tryFind PKey.textField.textChanging

    |> Option.iter (fun v -> 

      TerminalElement.logEvent("textChanging", "set")

      Interop.setEventHandler <@ element.TextChanging @> v element)


  override this.newView() = new TextField()

// TextValidateField
type TextValidateFieldElement(props: Props) =
  inherit TerminalElement(props)

  override this.removeProps(element: View, props: Props) =
    base.removeProps (element, props)
    let element = element :?> TextValidateField
    // Properties
    props

    |> Props.tryFind PKey.textValidateField.provider

    |> Option.iter (fun _ -> 

      TerminalElement.logEvent("provider", "removed")

      element.Provider <- Unchecked.defaultof<_>)

    props


    |> Props.tryFind PKey.textValidateField.text


    |> Option.iter (fun _ -> 


      TerminalElement.logEvent("text", "removed")


      element.Text <- Unchecked.defaultof<_>)

  override _.name = $"TextValidateField"

  override this.setProps(element: View, props: Props) =
    base.setProps (element, props)

    let element = element :?> TextValidateField

    // Properties
    props

    |> Props.tryFind PKey.textValidateField.provider

    |> Option.iter (fun v -> 

      TerminalElement.logEvent("provider", "set")

      element.Provider <- v)

    props


    |> Props.tryFind PKey.textValidateField.text


    |> Option.iter (fun v -> 


      TerminalElement.logEvent("text", "set")


      element.Text <- v)


  override this.newView() = new TextValidateField()

// TextView
type TextViewElement(props: Props) =
  inherit TerminalElement(props)


  override this.removeProps(element: View, props: Props) =
    base.removeProps (element, props)
    let element = element :?> TextView
    // Properties
    props

    |> Props.tryFind PKey.textView.allowsReturn

    |> Option.iter (fun _ -> 

      TerminalElement.logEvent("allowsReturn", "removed")

      element.AllowsReturn <- Unchecked.defaultof<_>)

    props


    |> Props.tryFind PKey.textView.allowsTab


    |> Option.iter (fun _ -> 


      TerminalElement.logEvent("allowsTab", "removed")


      element.AllowsTab <- Unchecked.defaultof<_>)

    props


    |> Props.tryFind PKey.textView.cursorPosition


    |> Option.iter (fun _ -> 


      TerminalElement.logEvent("cursorPosition", "removed")


      element.CursorPosition <- Unchecked.defaultof<_>)

    props


    |> Props.tryFind PKey.textView.inheritsPreviousAttribute


    |> Option.iter (fun _ -> 


      TerminalElement.logEvent("inheritsPreviousAttribute", "removed")


      element.InheritsPreviousAttribute <- Unchecked.defaultof<_>)

    props


    |> Props.tryFind PKey.textView.isDirty


    |> Option.iter (fun _ -> 


      TerminalElement.logEvent("isDirty", "removed")


      element.IsDirty <- Unchecked.defaultof<_>)

    props


    |> Props.tryFind PKey.textView.isSelecting


    |> Option.iter (fun _ -> 


      TerminalElement.logEvent("isSelecting", "removed")


      element.IsSelecting <- Unchecked.defaultof<_>)

    props


    |> Props.tryFind PKey.textView.leftColumn


    |> Option.iter (fun _ -> 


      TerminalElement.logEvent("leftColumn", "removed")


      element.LeftColumn <- Unchecked.defaultof<_>)

    props


    |> Props.tryFind PKey.textView.multiline


    |> Option.iter (fun _ -> 


      TerminalElement.logEvent("multiline", "removed")


      element.Multiline <- Unchecked.defaultof<_>)

    props


    |> Props.tryFind PKey.textView.readOnly


    |> Option.iter (fun _ -> 


      TerminalElement.logEvent("readOnly", "removed")


      element.ReadOnly <- Unchecked.defaultof<_>)

    props


    |> Props.tryFind PKey.textView.selectionStartColumn


    |> Option.iter (fun _ -> 


      TerminalElement.logEvent("selectionStartColumn", "removed")


      element.SelectionStartColumn <- Unchecked.defaultof<_>)

    props


    |> Props.tryFind PKey.textView.selectionStartRow


    |> Option.iter (fun _ -> 


      TerminalElement.logEvent("selectionStartRow", "removed")


      element.SelectionStartRow <- Unchecked.defaultof<_>)

    props


    |> Props.tryFind PKey.textView.selectWordOnlyOnDoubleClick


    |> Option.iter (fun _ -> 


      TerminalElement.logEvent("selectWordOnlyOnDoubleClick", "removed")


      element.SelectWordOnlyOnDoubleClick <- Unchecked.defaultof<_>)

    props


    |> Props.tryFind PKey.textView.tabWidth


    |> Option.iter (fun _ -> 


      TerminalElement.logEvent("tabWidth", "removed")


      element.TabWidth <- Unchecked.defaultof<_>)

    props


    |> Props.tryFind PKey.textView.text


    |> Option.iter (fun _ -> 


      TerminalElement.logEvent("text", "removed")


      element.Text <- Unchecked.defaultof<_>)

    props


    |> Props.tryFind PKey.textView.topRow


    |> Option.iter (fun _ -> 


      TerminalElement.logEvent("topRow", "removed")


      element.TopRow <- Unchecked.defaultof<_>)

    props


    |> Props.tryFind PKey.textView.used


    |> Option.iter (fun _ -> 


      TerminalElement.logEvent("used", "removed")


      element.Used <- Unchecked.defaultof<_>)

    props


    |> Props.tryFind PKey.textView.useSameRuneTypeForWords


    |> Option.iter (fun _ -> 


      TerminalElement.logEvent("useSameRuneTypeForWords", "removed")


      element.UseSameRuneTypeForWords <- Unchecked.defaultof<_>)

    props


    |> Props.tryFind PKey.textView.wordWrap


    |> Option.iter (fun _ -> 


      TerminalElement.logEvent("wordWrap", "removed")


      element.WordWrap <- Unchecked.defaultof<_>)
    // Events
    props

    |> Props.tryFind PKey.textView.contentsChanged

    |> Option.iter (fun _ -> 

      TerminalElement.logEvent("contentsChanged", "removed")

      Interop.removeEventHandler <@ element.ContentsChanged @> element)

    props


    |> Props.tryFind PKey.textView.drawNormalColor


    |> Option.iter (fun _ -> 


      TerminalElement.logEvent("drawNormalColor", "removed")


      Interop.removeEventHandler <@ element.DrawNormalColor @> element)

    props


    |> Props.tryFind PKey.textView.drawReadOnlyColor


    |> Option.iter (fun _ -> 


      TerminalElement.logEvent("drawReadOnlyColor", "removed")


      Interop.removeEventHandler <@ element.DrawReadOnlyColor @> element)

    props


    |> Props.tryFind PKey.textView.drawSelectionColor


    |> Option.iter (fun _ -> 


      TerminalElement.logEvent("drawSelectionColor", "removed")


      Interop.removeEventHandler <@ element.DrawSelectionColor @> element)

    props


    |> Props.tryFind PKey.textView.drawUsedColor


    |> Option.iter (fun _ -> 


      TerminalElement.logEvent("drawUsedColor", "removed")


      Interop.removeEventHandler <@ element.DrawUsedColor @> element)

    props


    |> Props.tryFind PKey.textView.unwrappedCursorPosition


    |> Option.iter (fun _ -> 


      TerminalElement.logEvent("unwrappedCursorPosition", "removed")


      Interop.removeEventHandler <@ element.UnwrappedCursorPosition @> element)

    // Additional properties
    props

    |> Props.tryFind PKey.textView.textChanged

    |> Option.iter (fun _ -> 

      TerminalElement.logEvent("textChanged", "removed")

      Interop.removeEventHandler <@ element.ContentsChanged @> element)


  override _.name = $"TextView"

  override this.setProps(element: View, props: Props) =
    base.setProps (element, props)

    let element = element :?> TextView

    // Properties
    props

    |> Props.tryFind PKey.textView.allowsReturn

    |> Option.iter (fun v -> 

      TerminalElement.logEvent("allowsReturn", "set")

      element.AllowsReturn <- v)

    props


    |> Props.tryFind PKey.textView.allowsTab


    |> Option.iter (fun v -> 


      TerminalElement.logEvent("allowsTab", "set")


      element.AllowsTab <- v)

    props


    |> Props.tryFind PKey.textView.cursorPosition


    |> Option.iter (fun v -> 


      TerminalElement.logEvent("cursorPosition", "set")


      element.CursorPosition <- v)

    props


    |> Props.tryFind PKey.textView.inheritsPreviousAttribute


    |> Option.iter (fun v -> 


      TerminalElement.logEvent("inheritsPreviousAttribute", "set")


      element.InheritsPreviousAttribute <- v)

    props


    |> Props.tryFind PKey.textView.isDirty


    |> Option.iter (fun v -> 


      TerminalElement.logEvent("isDirty", "set")


      element.IsDirty <- v)

    props


    |> Props.tryFind PKey.textView.isSelecting


    |> Option.iter (fun v -> 


      TerminalElement.logEvent("isSelecting", "set")


      element.IsSelecting <- v)

    props


    |> Props.tryFind PKey.textView.leftColumn


    |> Option.iter (fun v -> 


      TerminalElement.logEvent("leftColumn", "set")


      element.LeftColumn <- v)

    props


    |> Props.tryFind PKey.textView.multiline


    |> Option.iter (fun v -> 


      TerminalElement.logEvent("multiline", "set")


      element.Multiline <- v)

    props


    |> Props.tryFind PKey.textView.readOnly


    |> Option.iter (fun v -> 


      TerminalElement.logEvent("readOnly", "set")


      element.ReadOnly <- v)

    props


    |> Props.tryFind PKey.textView.selectionStartColumn


    |> Option.iter (fun v -> 


      TerminalElement.logEvent("selectionStartColumn", "set")


      element.SelectionStartColumn <- v)

    props


    |> Props.tryFind PKey.textView.selectionStartRow


    |> Option.iter (fun v -> 


      TerminalElement.logEvent("selectionStartRow", "set")


      element.SelectionStartRow <- v)

    props


    |> Props.tryFind PKey.textView.selectWordOnlyOnDoubleClick


    |> Option.iter (fun v -> 


      TerminalElement.logEvent("selectWordOnlyOnDoubleClick", "set")


      element.SelectWordOnlyOnDoubleClick <- v)

    props


    |> Props.tryFind PKey.textView.tabWidth


    |> Option.iter (fun v -> 


      TerminalElement.logEvent("tabWidth", "set")


      element.TabWidth <- v)

    props


    |> Props.tryFind PKey.textView.text


    |> Option.iter (fun v -> 


      TerminalElement.logEvent("text", "set")


      element.Text <- v)

    props


    |> Props.tryFind PKey.textView.topRow


    |> Option.iter (fun v -> 


      TerminalElement.logEvent("topRow", "set")


      element.TopRow <- v)

    props


    |> Props.tryFind PKey.textView.used


    |> Option.iter (fun v -> 


      TerminalElement.logEvent("used", "set")


      element.Used <- v)

    props


    |> Props.tryFind PKey.textView.useSameRuneTypeForWords


    |> Option.iter (fun v -> 


      TerminalElement.logEvent("useSameRuneTypeForWords", "set")


      element.UseSameRuneTypeForWords <- v)

    props


    |> Props.tryFind PKey.textView.wordWrap


    |> Option.iter (fun v -> 


      TerminalElement.logEvent("wordWrap", "set")


      element.WordWrap <- v)
    // Events
    props

    |> Props.tryFind PKey.textView.contentsChanged

    |> Option.iter (fun v -> 

      TerminalElement.logEvent("contentsChanged", "set")

      Interop.setEventHandler <@ element.ContentsChanged @> v element)

    props


    |> Props.tryFind PKey.textView.drawNormalColor


    |> Option.iter (fun v -> 


      TerminalElement.logEvent("drawNormalColor", "set")


      Interop.setEventHandler <@ element.DrawNormalColor @> v element)

    props


    |> Props.tryFind PKey.textView.drawReadOnlyColor


    |> Option.iter (fun v -> 


      TerminalElement.logEvent("drawReadOnlyColor", "set")


      Interop.setEventHandler <@ element.DrawReadOnlyColor @> v element)

    props


    |> Props.tryFind PKey.textView.drawSelectionColor


    |> Option.iter (fun v -> 


      TerminalElement.logEvent("drawSelectionColor", "set")


      Interop.setEventHandler <@ element.DrawSelectionColor @> v element)

    props


    |> Props.tryFind PKey.textView.drawUsedColor


    |> Option.iter (fun v -> 


      TerminalElement.logEvent("drawUsedColor", "set")


      Interop.setEventHandler <@ element.DrawUsedColor @> v element)

    props


    |> Props.tryFind PKey.textView.unwrappedCursorPosition


    |> Option.iter (fun v -> 


      TerminalElement.logEvent("unwrappedCursorPosition", "set")


      Interop.setEventHandler <@ element.UnwrappedCursorPosition @> v element)

    // Additional properties
    props

    |> Props.tryFind PKey.textView.textChanged

    |> Option.iter (fun v -> 

      TerminalElement.logEvent("textChanged", "set")

      Interop.setEventHandler <@ element.ContentsChanged @> (fun _ -> v element.Text) element)


  override this.newView() = new TextView()

// TimeField
type TimeFieldElement(props: Props) =
  inherit TerminalElement(props)

  override this.removeProps(element: View, props: Props) =
    base.removeProps (element, props)
    let element = element :?> TimeField
    // Properties
    props

    |> Props.tryFind PKey.timeField.cursorPosition

    |> Option.iter (fun _ -> 

      TerminalElement.logEvent("cursorPosition", "removed")

      element.CursorPosition <- Unchecked.defaultof<_>)

    props


    |> Props.tryFind PKey.timeField.isShortFormat


    |> Option.iter (fun _ -> 


      TerminalElement.logEvent("isShortFormat", "removed")


      element.IsShortFormat <- Unchecked.defaultof<_>)

    props


    |> Props.tryFind PKey.timeField.time


    |> Option.iter (fun _ -> 


      TerminalElement.logEvent("time", "removed")


      element.Time <- Unchecked.defaultof<_>)
    // Events
    props

    |> Props.tryFind PKey.timeField.timeChanged

    |> Option.iter (fun _ -> 

      TerminalElement.logEvent("timeChanged", "removed")

      Interop.removeEventHandler <@ element.TimeChanged @> element)

  override _.name = $"TimeField"

  override this.setProps(element: View, props: Props) =
    base.setProps (element, props)

    let element = element :?> TimeField

    // Properties
    props

    |> Props.tryFind PKey.timeField.cursorPosition

    |> Option.iter (fun v -> 

      TerminalElement.logEvent("cursorPosition", "set")

      element.CursorPosition <- v)

    props


    |> Props.tryFind PKey.timeField.isShortFormat


    |> Option.iter (fun v -> 


      TerminalElement.logEvent("isShortFormat", "set")


      element.IsShortFormat <- v)

    props


    |> Props.tryFind PKey.timeField.time


    |> Option.iter (fun v -> 


      TerminalElement.logEvent("time", "set")


      element.Time <- v)
    // Events
    props

    |> Props.tryFind PKey.timeField.timeChanged

    |> Option.iter (fun v -> 

      TerminalElement.logEvent("timeChanged", "set")

      Interop.setEventHandler <@ element.TimeChanged @> v element)


  override this.newView() = new TimeField()

// Runnable
type RunnableElement(props: Props) =
  inherit TerminalElement(props)

  override this.removeProps(element: View, props: Props) =
    base.removeProps (element, props)
    let element = element :?> Runnable
    // Properties
    props
    |> Props.tryFind PKey.runnable.isModal
    |> Option.iter (fun _ -> element.SetIsModal(Unchecked.defaultof<_>))

    props
    |> Props.tryFind PKey.runnable.isRunning
    |> Option.iter (fun _ -> element.SetIsRunning(Unchecked.defaultof<_>))

    props


    |> Props.tryFind PKey.runnable.stopRequested


    |> Option.iter (fun _ -> 


      TerminalElement.logEvent("stopRequested", "removed")


      element.StopRequested <- Unchecked.defaultof<_>)

    props


    |> Props.tryFind PKey.runnable.result


    |> Option.iter (fun _ -> 


      TerminalElement.logEvent("result", "removed")


      element.Result <- Unchecked.defaultof<_>)

    // Events
    props

    |> Props.tryFind PKey.runnable.isRunningChanging

    |> Option.iter (fun _ -> 

      TerminalElement.logEvent("isRunningChanging", "removed")

      Interop.removeEventHandler <@ element.IsRunningChanging @> element)

    props


    |> Props.tryFind PKey.runnable.isRunningChanged


    |> Option.iter (fun _ -> 


      TerminalElement.logEvent("isRunningChanged", "removed")


      Interop.removeEventHandler <@ element.IsRunningChanged @> element)

    props


    |> Props.tryFind PKey.runnable.isModalChanged


    |> Option.iter (fun _ -> 


      TerminalElement.logEvent("isModalChanged", "removed")


      Interop.removeEventHandler <@ element.IsModalChanged @> element)

  override _.name = $"Runnable"

  override this.setProps(element: View, props: Props) =
    base.setProps (element, props)

    let element = element :?> Runnable

    // Properties
    props
    |> Props.tryFind PKey.runnable.isModal
    |> Option.iter (fun v -> element.SetIsModal(v))

    props
    |> Props.tryFind PKey.runnable.isRunning
    |> Option.iter (fun v -> element.SetIsRunning(v))

    props


    |> Props.tryFind PKey.runnable.stopRequested


    |> Option.iter (fun v -> 


      TerminalElement.logEvent("stopRequested", "set")


      element.StopRequested <- v)

    props


    |> Props.tryFind PKey.runnable.result


    |> Option.iter (fun v -> 


      TerminalElement.logEvent("result", "set")


      element.Result <- v)

    // Events
    props

    |> Props.tryFind PKey.runnable.isRunningChanging

    |> Option.iter (fun v -> 

      TerminalElement.logEvent("isRunningChanging", "set")

      Interop.setEventHandler <@ element.IsRunningChanging @> v element)

    props


    |> Props.tryFind PKey.runnable.isRunningChanged


    |> Option.iter (fun v -> 


      TerminalElement.logEvent("isRunningChanged", "set")


      Interop.setEventHandler <@ element.IsRunningChanged @> v element)

    props


    |> Props.tryFind PKey.runnable.isModalChanged


    |> Option.iter (fun v -> 


      TerminalElement.logEvent("isModalChanged", "set")


      Interop.setEventHandler <@ element.IsModalChanged @> v element)


  override this.newView() = new Runnable()


// TreeView<'a when 'a : not struct>
type TreeViewElement<'a when 'a: not struct>(props: Props) =
  inherit TerminalElement(props)

  override this.removeProps(element: View, props: Props) =
    base.removeProps (element, props)
    let element = element :?> TreeView<'a>
    // Properties
    props
    |> Props.tryFind PKey.treeView<'a>.allowLetterBasedNavigation
    |> Option.iter (fun _ -> element.AllowLetterBasedNavigation <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.treeView<'a>.aspectGetter
    |> Option.iter (fun _ -> element.AspectGetter <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.treeView<'a>.colorGetter
    |> Option.iter (fun _ -> element.ColorGetter <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.treeView<'a>.maxDepth
    |> Option.iter (fun _ -> element.MaxDepth <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.treeView<'a>.multiSelect
    |> Option.iter (fun _ -> element.MultiSelect <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.treeView<'a>.objectActivationButton
    |> Option.iter (fun _ -> element.ObjectActivationButton <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.treeView<'a>.objectActivationKey
    |> Option.iter (fun _ -> element.ObjectActivationKey <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.treeView<'a>.scrollOffsetHorizontal
    |> Option.iter (fun _ -> element.ScrollOffsetHorizontal <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.treeView<'a>.scrollOffsetVertical
    |> Option.iter (fun _ -> element.ScrollOffsetVertical <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.treeView<'a>.selectedObject
    |> Option.iter (fun _ -> element.SelectedObject <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.treeView<'a>.style
    |> Option.iter (fun _ -> element.Style <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.treeView<'a>.treeBuilder
    |> Option.iter (fun _ -> element.TreeBuilder <- Unchecked.defaultof<_>)
    // Events
    props
    |> Props.tryFind PKey.treeView<'a>.drawLine
    |> Option.iter (fun _ -> Interop.removeEventHandler <@ element.DrawLine @> element)

    props
    |> Props.tryFind PKey.treeView<'a>.objectActivated
    |> Option.iter (fun _ -> Interop.removeEventHandler <@ element.ObjectActivated @> element)

    props
    |> Props.tryFind PKey.treeView<'a>.selectionChanged
    |> Option.iter (fun _ -> Interop.removeEventHandler <@ element.SelectionChanged @> element)

  override _.name = $"TreeView<'a>"

  override this.setProps(element: View, props: Props) =
    base.setProps (element, props)

    let element = element :?> TreeView<'a>

    // Properties
    props
    |> Props.tryFind PKey.treeView<'a>.allowLetterBasedNavigation
    |> Option.iter (fun v -> element.AllowLetterBasedNavigation <- v)

    props
    |> Props.tryFind PKey.treeView<'a>.aspectGetter
    |> Option.iter (fun v -> element.AspectGetter <- v)

    props
    |> Props.tryFind PKey.treeView<'a>.colorGetter
    |> Option.iter (fun v -> element.ColorGetter <- v)

    props
    |> Props.tryFind PKey.treeView<'a>.maxDepth
    |> Option.iter (fun v -> element.MaxDepth <- v)

    props
    |> Props.tryFind PKey.treeView<'a>.multiSelect
    |> Option.iter (fun v -> element.MultiSelect <- v)

    props
    |> Props.tryFind PKey.treeView<'a>.objectActivationButton
    |> Option.iter (fun v -> element.ObjectActivationButton <- v |> Option.toNullable)

    props
    |> Props.tryFind PKey.treeView<'a>.objectActivationKey
    |> Option.iter (fun v -> element.ObjectActivationKey <- v)

    props
    |> Props.tryFind PKey.treeView<'a>.scrollOffsetHorizontal
    |> Option.iter (fun v -> element.ScrollOffsetHorizontal <- v)

    props
    |> Props.tryFind PKey.treeView<'a>.scrollOffsetVertical
    |> Option.iter (fun v -> element.ScrollOffsetVertical <- v)

    props
    |> Props.tryFind PKey.treeView<'a>.selectedObject
    |> Option.iter (fun v -> element.SelectedObject <- v)

    props
    |> Props.tryFind PKey.treeView<'a>.style
    |> Option.iter (fun v -> element.Style <- v)

    props
    |> Props.tryFind PKey.treeView<'a>.treeBuilder
    |> Option.iter (fun v -> element.TreeBuilder <- v)
    // Events
    props
    |> Props.tryFind PKey.treeView<'a>.drawLine
    |> Option.iter (fun v -> Interop.setEventHandler <@ element.DrawLine @> v element)

    props
    |> Props.tryFind PKey.treeView<'a>.objectActivated
    |> Option.iter (fun v -> Interop.setEventHandler <@ element.ObjectActivated @> v element)

    props
    |> Props.tryFind PKey.treeView<'a>.selectionChanged
    |> Option.iter (fun v -> Interop.setEventHandler <@ element.SelectionChanged @> v element)

  override this.newView() = new TreeView<'a>()


  interface ITreeViewElement

// TreeView
type TreeViewElement(props: Props) =
  inherit TreeViewElement<ITreeNode>(props)

// Window
type WindowElement(props: Props) =
  inherit TerminalElement(props)

  override this.removeProps(element: View, props: Props) = base.removeProps (element, props)
  // No properties or events Window

  override _.name = $"Window"

  override this.setProps(element: View, props: Props) = base.setProps (element, props)
  // No properties or events Window


  override this.newView() = new Window()

// Wizard
type WizardElement(props: Props) =
  inherit TerminalElement(props)

  override this.removeProps(element: View, props: Props) =
    base.removeProps (element, props)
    let element = element :?> Wizard
    // Properties
    props

    |> Props.tryFind PKey.wizard.currentStep

    |> Option.iter (fun _ -> 

      TerminalElement.logEvent("currentStep", "removed")

      element.CurrentStep <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.wizard.modal
    |> Option.iter (fun _ -> element.SetIsModal(Unchecked.defaultof<_>))
    // Events
    props

    |> Props.tryFind PKey.wizard.cancelled

    |> Option.iter (fun _ -> 

      TerminalElement.logEvent("cancelled", "removed")

      Interop.removeEventHandler <@ element.Cancelled @> element)

    props


    |> Props.tryFind PKey.wizard.finished


    |> Option.iter (fun _ -> 


      TerminalElement.logEvent("finished", "removed")


      Interop.removeEventHandler <@ element.Finished @> element)

    props


    |> Props.tryFind PKey.wizard.movingBack


    |> Option.iter (fun _ -> 


      TerminalElement.logEvent("movingBack", "removed")


      Interop.removeEventHandler <@ element.MovingBack @> element)

    props


    |> Props.tryFind PKey.wizard.movingNext


    |> Option.iter (fun _ -> 


      TerminalElement.logEvent("movingNext", "removed")


      Interop.removeEventHandler <@ element.MovingNext @> element)

    props


    |> Props.tryFind PKey.wizard.stepChanged


    |> Option.iter (fun _ -> 


      TerminalElement.logEvent("stepChanged", "removed")


      Interop.removeEventHandler <@ element.StepChanged @> element)

    props


    |> Props.tryFind PKey.wizard.stepChanging


    |> Option.iter (fun _ -> 


      TerminalElement.logEvent("stepChanging", "removed")


      Interop.removeEventHandler <@ element.StepChanging @> element)

  override _.name = $"Wizard"

  override _.setProps(element: View, props: Props) =
    base.setProps (element, props)

    let element = element :?> Wizard

    // Properties
    props

    |> Props.tryFind PKey.wizard.currentStep

    |> Option.iter (fun v -> 

      TerminalElement.logEvent("currentStep", "set")

      element.CurrentStep <- v)

    props
    |> Props.tryFind PKey.wizard.modal
    |> Option.iter (fun v -> element.SetIsModal(v))
    // Events
    props

    |> Props.tryFind PKey.wizard.cancelled

    |> Option.iter (fun v -> 

      TerminalElement.logEvent("cancelled", "set")

      Interop.setEventHandler <@ element.Cancelled @> v element)

    props


    |> Props.tryFind PKey.wizard.finished


    |> Option.iter (fun v -> 


      TerminalElement.logEvent("finished", "set")


      Interop.setEventHandler <@ element.Finished @> v element)

    props


    |> Props.tryFind PKey.wizard.movingBack


    |> Option.iter (fun v -> 


      TerminalElement.logEvent("movingBack", "set")


      Interop.setEventHandler <@ element.MovingBack @> v element)

    props


    |> Props.tryFind PKey.wizard.movingNext


    |> Option.iter (fun v -> 


      TerminalElement.logEvent("movingNext", "set")


      Interop.setEventHandler <@ element.MovingNext @> v element)

    props


    |> Props.tryFind PKey.wizard.stepChanged


    |> Option.iter (fun v -> 


      TerminalElement.logEvent("stepChanged", "set")


      Interop.setEventHandler <@ element.StepChanged @> v element)

    props


    |> Props.tryFind PKey.wizard.stepChanging


    |> Option.iter (fun v -> 


      TerminalElement.logEvent("stepChanging", "set")


      Interop.setEventHandler <@ element.StepChanging @> v element)


  override this.newView() = new Wizard()

// WizardStep
type WizardStepElement(props: Props) =
  inherit TerminalElement(props)

  override this.removeProps(element: View, props: Props) =
    base.removeProps (element, props)
    let element = element :?> WizardStep
    // Properties
    props

    |> Props.tryFind PKey.wizardStep.backButtonText

    |> Option.iter (fun _ -> 

      TerminalElement.logEvent("backButtonText", "removed")

      element.BackButtonText <- Unchecked.defaultof<_>)

    props


    |> Props.tryFind PKey.wizardStep.helpText


    |> Option.iter (fun _ -> 


      TerminalElement.logEvent("helpText", "removed")


      element.HelpText <- Unchecked.defaultof<_>)

    props


    |> Props.tryFind PKey.wizardStep.nextButtonText


    |> Option.iter (fun _ -> 


      TerminalElement.logEvent("nextButtonText", "removed")


      element.NextButtonText <- Unchecked.defaultof<_>)

  override _.name = $"WizardStep"

  override this.setProps(element: View, props: Props) =
    base.setProps (element, props)

    let element = element :?> WizardStep

    // Properties
    props

    |> Props.tryFind PKey.wizardStep.backButtonText

    |> Option.iter (fun v -> 

      TerminalElement.logEvent("backButtonText", "set")

      element.BackButtonText <- v)

    props


    |> Props.tryFind PKey.wizardStep.helpText


    |> Option.iter (fun v -> 


      TerminalElement.logEvent("helpText", "set")


      element.HelpText <- v)

    props


    |> Props.tryFind PKey.wizardStep.nextButtonText


    |> Option.iter (fun v -> 


      TerminalElement.logEvent("nextButtonText", "set")


      element.NextButtonText <- v)


  override this.newView() = new WizardStep()
