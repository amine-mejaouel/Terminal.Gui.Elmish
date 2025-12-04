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

  let setEventHandlers = Collections.Generic.HashSet<string>()

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

  member this.trackEventHandler(eventName: string) =
    setEventHandlers.Add(eventName) |> ignore

  member this.removeTrackedEventHandler(eventName: string) =
    setEventHandlers.Remove(eventName) |> ignore

  member this.isEventHandlerTracked(eventName: string) =
    setEventHandlers.Contains(eventName)

  abstract setProps: element: View * props: Props -> unit

  default this.setProps(element: View, props: Props) =
    // Properties
    props
    |> Props.tryFind PKey.view.arrangement
    |> Option.iter (fun v -> element.Arrangement <- v)

    props
    |> Props.tryFind PKey.view.borderStyle
    |> Option.iter (fun v -> element.BorderStyle <- v)

    props
    |> Props.tryFind PKey.view.canFocus
    |> Option.iter (fun v -> element.CanFocus <- v)

    props
    |> Props.tryFind PKey.view.contentSizeTracksViewport
    |> Option.iter (fun v -> element.ContentSizeTracksViewport <- v)

    props
    |> Props.tryFind PKey.view.cursorVisibility
    |> Option.iter (fun v -> element.CursorVisibility <- v)

    props
    |> Props.tryFind PKey.view.data
    |> Option.iter (fun v -> element.Data <- v)

    props
    |> Props.tryFind PKey.view.enabled
    |> Option.iter (fun v -> element.Enabled <- v)

    props
    |> Props.tryFind PKey.view.frame
    |> Option.iter (fun v -> element.Frame <- v)

    props
    |> Props.tryFind PKey.view.hasFocus
    |> Option.iter (fun v -> element.HasFocus <- v)

    props
    |> Props.tryFind PKey.view.height
    |> Option.iter (fun v -> element.Height <- v)

    props
    |> Props.tryFind PKey.view.highlightStates
    |> Option.iter (fun v -> element.HighlightStates <- v)

    props
    |> Props.tryFind PKey.view.hotKey
    |> Option.iter (fun v -> element.HotKey <- v)

    props
    |> Props.tryFind PKey.view.hotKeySpecifier
    |> Option.iter (fun v -> element.HotKeySpecifier <- v)

    props
    |> Props.tryFind PKey.view.id
    |> Option.iter (fun v -> element.Id <- v)

    props
    |> Props.tryFind PKey.view.isInitialized
    |> Option.iter (fun v -> element.IsInitialized <- v)

    props
    |> Props.tryFind PKey.view.mouseHeldDown
    |> Option.iter (fun v -> element.MouseHeldDown <- v)

    props
    |> Props.tryFind PKey.view.needsDraw
    |> Option.iter (fun v -> element.NeedsDraw <- v)

    props
    |> Props.tryFind PKey.view.preserveTrailingSpaces
    |> Option.iter (fun v -> element.PreserveTrailingSpaces <- v)

    props
    |> Props.tryFind PKey.view.schemeName
    |> Option.iter (fun v -> element.SchemeName <- v)

    props
    |> Props.tryFind PKey.view.shadowStyle
    |> Option.iter (fun v -> element.ShadowStyle <- v)

    props
    |> Props.tryFind PKey.view.superViewRendersLineCanvas
    |> Option.iter (fun v -> element.SuperViewRendersLineCanvas <- v)

    props
    |> Props.tryFind PKey.view.tabStop
    |> Option.iter (fun v -> element.TabStop <- v |> Option.toNullable)

    props
    |> Props.tryFind PKey.view.text
    |> Option.iter (fun v -> element.Text <- v)

    props
    |> Props.tryFind PKey.view.textAlignment
    |> Option.iter (fun v -> element.TextAlignment <- v)

    props
    |> Props.tryFind PKey.view.textDirection
    |> Option.iter (fun v -> element.TextDirection <- v)

    props
    |> Props.tryFind PKey.view.title
    |> Option.iter (fun v -> element.Title <- v)

    props
    |> Props.tryFind PKey.view.validatePosDim
    |> Option.iter (fun v -> element.ValidatePosDim <- v)

    props
    |> Props.tryFind PKey.view.verticalTextAlignment
    |> Option.iter (fun v -> element.VerticalTextAlignment <- v)

    props
    |> Props.tryFind PKey.view.viewport
    |> Option.iter (fun v -> element.Viewport <- v)

    props
    |> Props.tryFind PKey.view.viewportSettings
    |> Option.iter (fun v -> element.ViewportSettings <- v)

    props
    |> Props.tryFind PKey.view.visible
    |> Option.iter (fun v -> element.Visible <- v)

    props
    |> Props.tryFind PKey.view.wantContinuousButtonPressed
    |> Option.iter (fun v -> element.WantContinuousButtonPressed <- v)

    props
    |> Props.tryFind PKey.view.wantMousePositionReports
    |> Option.iter (fun v -> element.WantMousePositionReports <- v)

    props
    |> Props.tryFind PKey.view.width
    |> Option.iter (fun v -> element.Width <- v)

    props
    |> Props.tryFind PKey.view.x
    |> Option.iter (fun v -> element.X <- v)

    props
    |> Props.tryFind PKey.view.y
    |> Option.iter (fun v -> element.Y <- v)
    // Events
    props
    |> Props.tryFind PKey.view.accepting
    |> Option.iter (fun v -> 
      Interop.setEventHandler <@ element.Accepting @> v element
      this.trackEventHandler("accepting"))

    props
    |> Props.tryFind PKey.view.advancingFocus
    |> Option.iter (fun v -> 
      Interop.setEventHandler <@ element.AdvancingFocus @> v element
      this.trackEventHandler("advancingFocus"))

    props
    |> Props.tryFind PKey.view.borderStyleChanged
    |> Option.iter (fun v -> 
      Interop.setEventHandler <@ element.BorderStyleChanged @> v element
      this.trackEventHandler("borderStyleChanged"))

    props
    |> Props.tryFind PKey.view.canFocusChanged
    |> Option.iter (fun v -> 
      Interop.setEventHandler <@ element.CanFocusChanged @> (fun _ -> v ()) element
      this.trackEventHandler("canFocusChanged"))

    props
    |> Props.tryFind PKey.view.clearedViewport
    |> Option.iter (fun v -> 
      Interop.setEventHandler <@ element.ClearedViewport @> v element
      this.trackEventHandler("clearedViewport"))

    props


    |> Props.tryFind PKey.view.clearingViewport


    |> Option.iter (fun v -> 


      Interop.setEventHandler <@ element.ClearingViewport @> v element


      this.trackEventHandler("clearingViewport"))

    props


    |> Props.tryFind PKey.view.commandNotBound


    |> Option.iter (fun v -> 


      Interop.setEventHandler <@ element.CommandNotBound @> v element


      this.trackEventHandler("commandNotBound"))

    props


    |> Props.tryFind PKey.view.contentSizeChanged


    |> Option.iter (fun v -> 


      Interop.setEventHandler <@ element.ContentSizeChanged @> v element


      this.trackEventHandler("contentSizeChanged"))

    props
    |> Props.tryFind PKey.view.disposing
    |> Option.iter (fun v -> Interop.setEventHandler <@ element.Disposing @> (fun _ -> v ()) element)

    props


    |> Props.tryFind PKey.view.drawComplete


    |> Option.iter (fun v -> 


      Interop.setEventHandler <@ element.DrawComplete @> v element


      this.trackEventHandler("drawComplete"))

    props


    |> Props.tryFind PKey.view.drawingContent


    |> Option.iter (fun v -> 


      Interop.setEventHandler <@ element.DrawingContent @> v element


      this.trackEventHandler("drawingContent"))

    props


    |> Props.tryFind PKey.view.drawingSubViews


    |> Option.iter (fun v -> 


      Interop.setEventHandler <@ element.DrawingSubViews @> v element


      this.trackEventHandler("drawingSubViews"))

    props


    |> Props.tryFind PKey.view.drawingText


    |> Option.iter (fun v -> 


      Interop.setEventHandler <@ element.DrawingText @> v element


      this.trackEventHandler("drawingText"))

    props
    |> Props.tryFind PKey.view.enabledChanged
    |> Option.iter (fun v -> Interop.setEventHandler <@ element.EnabledChanged @> (fun _ -> v ()) element)

    props


    |> Props.tryFind PKey.view.focusedChanged


    |> Option.iter (fun v -> 


      Interop.setEventHandler <@ element.FocusedChanged @> v element


      this.trackEventHandler("focusedChanged"))

    props


    |> Props.tryFind PKey.view.frameChanged


    |> Option.iter (fun v -> 


      Interop.setEventHandler <@ element.FrameChanged @> v element


      this.trackEventHandler("frameChanged"))

    props


    |> Props.tryFind PKey.view.gettingAttributeForRole


    |> Option.iter (fun v -> 


      Interop.setEventHandler <@ element.GettingAttributeForRole @> v element


      this.trackEventHandler("gettingAttributeForRole"))

    props


    |> Props.tryFind PKey.view.gettingScheme


    |> Option.iter (fun v -> 


      Interop.setEventHandler <@ element.GettingScheme @> v element


      this.trackEventHandler("gettingScheme"))

    props


    |> Props.tryFind PKey.view.handlingHotKey


    |> Option.iter (fun v -> 


      Interop.setEventHandler <@ element.HandlingHotKey @> v element


      this.trackEventHandler("handlingHotKey"))

    props


    |> Props.tryFind PKey.view.hasFocusChanged


    |> Option.iter (fun v -> 


      Interop.setEventHandler <@ element.HasFocusChanged @> v element


      this.trackEventHandler("hasFocusChanged"))

    props


    |> Props.tryFind PKey.view.hasFocusChanging


    |> Option.iter (fun v -> 


      Interop.setEventHandler <@ element.HasFocusChanging @> v element


      this.trackEventHandler("hasFocusChanging"))

    props


    |> Props.tryFind PKey.view.hotKeyChanged


    |> Option.iter (fun v -> 


      Interop.setEventHandler <@ element.HotKeyChanged @> v element


      this.trackEventHandler("hotKeyChanged"))

    props
    |> Props.tryFind PKey.view.initialized
    |> Option.iter (fun v -> Interop.setEventHandler <@ element.Initialized @> (fun _ -> v ()) element)

    props


    |> Props.tryFind PKey.view.keyDown


    |> Option.iter (fun v -> 


      Interop.setEventHandler <@ element.KeyDown @> v element


      this.trackEventHandler("keyDown"))

    props


    |> Props.tryFind PKey.view.keyDownNotHandled


    |> Option.iter (fun v -> 


      Interop.setEventHandler <@ element.KeyDownNotHandled @> v element


      this.trackEventHandler("keyDownNotHandled"))

    props


    |> Props.tryFind PKey.view.keyUp


    |> Option.iter (fun v -> 


      Interop.setEventHandler <@ element.KeyUp @> v element


      this.trackEventHandler("keyUp"))

    props


    |> Props.tryFind PKey.view.mouseClick


    |> Option.iter (fun v -> 


      Interop.setEventHandler <@ element.MouseClick @> v element


      this.trackEventHandler("mouseClick"))

    props


    |> Props.tryFind PKey.view.mouseEnter


    |> Option.iter (fun v -> 


      Interop.setEventHandler <@ element.MouseEnter @> v element


      this.trackEventHandler("mouseEnter"))

    props


    |> Props.tryFind PKey.view.mouseEvent


    |> Option.iter (fun v -> 


      Interop.setEventHandler <@ element.MouseEvent @> v element


      this.trackEventHandler("mouseEvent"))

    props


    |> Props.tryFind PKey.view.mouseLeave


    |> Option.iter (fun v -> 


      Interop.setEventHandler <@ element.MouseLeave @> v element


      this.trackEventHandler("mouseLeave"))

    props


    |> Props.tryFind PKey.view.mouseStateChanged


    |> Option.iter (fun v -> 


      Interop.setEventHandler <@ element.MouseStateChanged @> v element


      this.trackEventHandler("mouseStateChanged"))

    props


    |> Props.tryFind PKey.view.mouseWheel


    |> Option.iter (fun v -> 


      Interop.setEventHandler <@ element.MouseWheel @> v element


      this.trackEventHandler("mouseWheel"))

    props


    |> Props.tryFind PKey.view.removed


    |> Option.iter (fun v -> 


      Interop.setEventHandler <@ element.Removed @> v element


      this.trackEventHandler("removed"))

    props


    |> Props.tryFind PKey.view.schemeChanged


    |> Option.iter (fun v -> 


      Interop.setEventHandler <@ element.SchemeChanged @> v element


      this.trackEventHandler("schemeChanged"))

    props


    |> Props.tryFind PKey.view.schemeChanging


    |> Option.iter (fun v -> 


      Interop.setEventHandler <@ element.SchemeChanging @> v element


      this.trackEventHandler("schemeChanging"))

    props


    |> Props.tryFind PKey.view.schemeNameChanged


    |> Option.iter (fun v -> 


      Interop.setEventHandler <@ element.SchemeNameChanged @> v element


      this.trackEventHandler("schemeNameChanged"))

    props


    |> Props.tryFind PKey.view.schemeNameChanging


    |> Option.iter (fun v -> 


      Interop.setEventHandler <@ element.SchemeNameChanging @> v element


      this.trackEventHandler("schemeNameChanging"))

    props


    |> Props.tryFind PKey.view.selecting


    |> Option.iter (fun v -> 


      Interop.setEventHandler <@ element.Selecting @> v element


      this.trackEventHandler("selecting"))

    props


    |> Props.tryFind PKey.view.subViewAdded


    |> Option.iter (fun v -> 


      Interop.setEventHandler <@ element.SubViewAdded @> v element


      this.trackEventHandler("subViewAdded"))

    props


    |> Props.tryFind PKey.view.subViewLayout


    |> Option.iter (fun v -> 


      Interop.setEventHandler <@ element.SubViewLayout @> v element


      this.trackEventHandler("subViewLayout"))

    props


    |> Props.tryFind PKey.view.subViewRemoved


    |> Option.iter (fun v -> 


      Interop.setEventHandler <@ element.SubViewRemoved @> v element


      this.trackEventHandler("subViewRemoved"))

    props


    |> Props.tryFind PKey.view.subViewsLaidOut


    |> Option.iter (fun v -> 


      Interop.setEventHandler <@ element.SubViewsLaidOut @> v element


      this.trackEventHandler("subViewsLaidOut"))

    props


    |> Props.tryFind PKey.view.superViewChanged


    |> Option.iter (fun v -> 


      Interop.setEventHandler <@ element.SuperViewChanged @> v element


      this.trackEventHandler("superViewChanged"))

    props
    |> Props.tryFind PKey.view.textChanged
    |> Option.iter (fun v -> Interop.setEventHandler <@ element.TextChanged @> (fun _ -> v ()) element)

    props


    |> Props.tryFind PKey.view.titleChanged


    |> Option.iter (fun v -> 


      Interop.setEventHandler <@ element.TitleChanged @> (fun arg -> v arg.Value) element


      this.trackEventHandler("titleChanged"))

    props


    |> Props.tryFind PKey.view.titleChanging


    |> Option.iter (fun v -> 


      Interop.setEventHandler <@ element.TitleChanging @> v element


      this.trackEventHandler("titleChanging"))

    props


    |> Props.tryFind PKey.view.viewportChanged


    |> Option.iter (fun v -> 


      Interop.setEventHandler <@ element.ViewportChanged @> v element


      this.trackEventHandler("viewportChanged"))

    props
    |> Props.tryFind PKey.view.visibleChanged
    |> Option.iter (fun v -> Interop.setEventHandler <@ element.VisibleChanged @> (fun _ -> v ()) element)

    props
    |> Props.tryFind PKey.view.visibleChanging
    |> Option.iter (fun v -> Interop.setEventHandler <@ element.VisibleChanging @> (fun _ -> v ()) element)

    // Custom Props
    props
    |> Props.tryFind PKey.view.x_delayedPos
    |> Option.iter (applyPos (fun pos -> element.X <- pos))

    props
    |> Props.tryFind PKey.view.y_delayedPos
    |> Option.iter (applyPos (fun pos -> element.Y <- pos))

  abstract removeProps: element: View * props: Props -> unit

  default this.removeProps(element: View, props: Props) =
    // Properties
    props
    |> Props.tryFind PKey.view.arrangement
    |> Option.iter (fun _ -> element.Arrangement <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.view.borderStyle
    |> Option.iter (fun _ -> element.BorderStyle <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.view.canFocus
    |> Option.iter (fun _ -> element.CanFocus <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.view.contentSizeTracksViewport
    |> Option.iter (fun _ -> element.ContentSizeTracksViewport <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.view.cursorVisibility
    |> Option.iter (fun _ -> element.CursorVisibility <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.view.data
    |> Option.iter (fun _ -> element.Data <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.view.enabled
    |> Option.iter (fun _ -> element.Enabled <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.view.frame
    |> Option.iter (fun _ -> element.Frame <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.view.hasFocus
    |> Option.iter (fun _ -> element.HasFocus <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.view.height
    |> Option.iter (fun _ -> element.Height <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.view.highlightStates
    |> Option.iter (fun _ -> element.HighlightStates <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.view.hotKey
    |> Option.iter (fun _ -> element.HotKey <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.view.hotKeySpecifier
    |> Option.iter (fun _ -> element.HotKeySpecifier <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.view.id
    |> Option.iter (fun _ -> element.Id <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.view.isInitialized
    |> Option.iter (fun _ -> element.IsInitialized <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.view.mouseHeldDown
    |> Option.iter (fun _ -> element.MouseHeldDown <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.view.needsDraw
    |> Option.iter (fun _ -> element.NeedsDraw <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.view.preserveTrailingSpaces
    |> Option.iter (fun _ -> element.PreserveTrailingSpaces <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.view.schemeName
    |> Option.iter (fun _ -> element.SchemeName <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.view.shadowStyle
    |> Option.iter (fun _ -> element.ShadowStyle <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.view.superViewRendersLineCanvas
    |> Option.iter (fun _ -> element.SuperViewRendersLineCanvas <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.view.tabStop
    |> Option.iter (fun _ -> element.TabStop <- Unchecked.defaultof<_> |> Option.toNullable)

    props
    |> Props.tryFind PKey.view.text
    |> Option.iter (fun _ -> element.Text <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.view.textAlignment
    |> Option.iter (fun _ -> element.TextAlignment <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.view.textDirection
    |> Option.iter (fun _ -> element.TextDirection <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.view.title
    |> Option.iter (fun _ -> element.Title <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.view.validatePosDim
    |> Option.iter (fun _ -> element.ValidatePosDim <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.view.verticalTextAlignment
    |> Option.iter (fun _ -> element.VerticalTextAlignment <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.view.viewport
    |> Option.iter (fun _ -> element.Viewport <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.view.viewportSettings
    |> Option.iter (fun _ -> element.ViewportSettings <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.view.visible
    |> Option.iter (fun _ -> element.Visible <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.view.wantContinuousButtonPressed
    |> Option.iter (fun _ -> element.WantContinuousButtonPressed <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.view.wantMousePositionReports
    |> Option.iter (fun _ -> element.WantMousePositionReports <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.view.width
    |> Option.iter (fun _ -> element.Width <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.view.x
    |> Option.iter (fun _ -> element.X <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.view.y
    |> Option.iter (fun _ -> element.Y <- Unchecked.defaultof<_>)
    // Events
    props

    |> Props.tryFind PKey.view.accepting

    |> Option.iter (fun _ -> 

      if this.isEventHandlerTracked("accepting") then

        Interop.removeEventHandler <@ element.Accepting @> element

        this.removeTrackedEventHandler("accepting"))

    props


    |> Props.tryFind PKey.view.advancingFocus


    |> Option.iter (fun _ -> 


      if this.isEventHandlerTracked("advancingFocus") then


        Interop.removeEventHandler <@ element.AdvancingFocus @> element


        this.removeTrackedEventHandler("advancingFocus"))

    props


    |> Props.tryFind PKey.view.borderStyleChanged


    |> Option.iter (fun _ -> 


      if this.isEventHandlerTracked("borderStyleChanged") then


        Interop.removeEventHandler <@ element.BorderStyleChanged @> element


        this.removeTrackedEventHandler("borderStyleChanged"))

    props


    |> Props.tryFind PKey.view.canFocusChanged


    |> Option.iter (fun _ -> 


      if this.isEventHandlerTracked("canFocusChanged") then


        Interop.removeEventHandler <@ element.CanFocusChanged @> element


        this.removeTrackedEventHandler("canFocusChanged"))

    props


    |> Props.tryFind PKey.view.clearedViewport


    |> Option.iter (fun _ -> 


      if this.isEventHandlerTracked("clearedViewport") then


        Interop.removeEventHandler <@ element.ClearedViewport @> element


        this.removeTrackedEventHandler("clearedViewport"))

    props


    |> Props.tryFind PKey.view.clearingViewport


    |> Option.iter (fun _ -> 


      if this.isEventHandlerTracked("clearingViewport") then


        Interop.removeEventHandler <@ element.ClearingViewport @> element


        this.removeTrackedEventHandler("clearingViewport"))

    props


    |> Props.tryFind PKey.view.commandNotBound


    |> Option.iter (fun _ -> 


      if this.isEventHandlerTracked("commandNotBound") then


        Interop.removeEventHandler <@ element.CommandNotBound @> element


        this.removeTrackedEventHandler("commandNotBound"))

    props


    |> Props.tryFind PKey.view.contentSizeChanged


    |> Option.iter (fun _ -> 


      if this.isEventHandlerTracked("contentSizeChanged") then


        Interop.removeEventHandler <@ element.ContentSizeChanged @> element


        this.removeTrackedEventHandler("contentSizeChanged"))

    props


    |> Props.tryFind PKey.view.disposing


    |> Option.iter (fun _ -> 


      if this.isEventHandlerTracked("disposing") then


        Interop.removeEventHandler <@ element.Disposing @> element


        this.removeTrackedEventHandler("disposing"))

    props


    |> Props.tryFind PKey.view.drawComplete


    |> Option.iter (fun _ -> 


      if this.isEventHandlerTracked("drawComplete") then


        Interop.removeEventHandler <@ element.DrawComplete @> element


        this.removeTrackedEventHandler("drawComplete"))

    props


    |> Props.tryFind PKey.view.drawingContent


    |> Option.iter (fun _ -> 


      if this.isEventHandlerTracked("drawingContent") then


        Interop.removeEventHandler <@ element.DrawingContent @> element


        this.removeTrackedEventHandler("drawingContent"))

    props


    |> Props.tryFind PKey.view.drawingSubViews


    |> Option.iter (fun _ -> 


      if this.isEventHandlerTracked("drawingSubViews") then


        Interop.removeEventHandler <@ element.DrawingSubViews @> element


        this.removeTrackedEventHandler("drawingSubViews"))

    props


    |> Props.tryFind PKey.view.drawingText


    |> Option.iter (fun _ -> 


      if this.isEventHandlerTracked("drawingText") then


        Interop.removeEventHandler <@ element.DrawingText @> element


        this.removeTrackedEventHandler("drawingText"))

    props


    |> Props.tryFind PKey.view.enabledChanged


    |> Option.iter (fun _ -> 


      if this.isEventHandlerTracked("enabledChanged") then


        Interop.removeEventHandler <@ element.EnabledChanged @> element


        this.removeTrackedEventHandler("enabledChanged"))

    props


    |> Props.tryFind PKey.view.focusedChanged


    |> Option.iter (fun _ -> 


      if this.isEventHandlerTracked("focusedChanged") then


        Interop.removeEventHandler <@ element.FocusedChanged @> element


        this.removeTrackedEventHandler("focusedChanged"))

    props


    |> Props.tryFind PKey.view.frameChanged


    |> Option.iter (fun _ -> 


      if this.isEventHandlerTracked("frameChanged") then


        Interop.removeEventHandler <@ element.FrameChanged @> element


        this.removeTrackedEventHandler("frameChanged"))

    props


    |> Props.tryFind PKey.view.gettingAttributeForRole


    |> Option.iter (fun _ -> 


      if this.isEventHandlerTracked("gettingAttributeForRole") then


        Interop.removeEventHandler <@ element.GettingAttributeForRole @> element


        this.removeTrackedEventHandler("gettingAttributeForRole"))

    props


    |> Props.tryFind PKey.view.gettingScheme


    |> Option.iter (fun _ -> 


      if this.isEventHandlerTracked("gettingScheme") then


        Interop.removeEventHandler <@ element.GettingScheme @> element


        this.removeTrackedEventHandler("gettingScheme"))

    props


    |> Props.tryFind PKey.view.handlingHotKey


    |> Option.iter (fun _ -> 


      if this.isEventHandlerTracked("handlingHotKey") then


        Interop.removeEventHandler <@ element.HandlingHotKey @> element


        this.removeTrackedEventHandler("handlingHotKey"))

    props


    |> Props.tryFind PKey.view.hasFocusChanged


    |> Option.iter (fun _ -> 


      if this.isEventHandlerTracked("hasFocusChanged") then


        Interop.removeEventHandler <@ element.HasFocusChanged @> element


        this.removeTrackedEventHandler("hasFocusChanged"))

    props


    |> Props.tryFind PKey.view.hasFocusChanging


    |> Option.iter (fun _ -> 


      if this.isEventHandlerTracked("hasFocusChanging") then


        Interop.removeEventHandler <@ element.HasFocusChanging @> element


        this.removeTrackedEventHandler("hasFocusChanging"))

    props


    |> Props.tryFind PKey.view.hotKeyChanged


    |> Option.iter (fun _ -> 


      if this.isEventHandlerTracked("hotKeyChanged") then


        Interop.removeEventHandler <@ element.HotKeyChanged @> element


        this.removeTrackedEventHandler("hotKeyChanged"))

    props


    |> Props.tryFind PKey.view.initialized


    |> Option.iter (fun _ -> 


      if this.isEventHandlerTracked("initialized") then


        Interop.removeEventHandler <@ element.Initialized @> element


        this.removeTrackedEventHandler("initialized"))

    props


    |> Props.tryFind PKey.view.keyDown


    |> Option.iter (fun _ -> 


      if this.isEventHandlerTracked("keyDown") then


        Interop.removeEventHandler <@ element.KeyDown @> element


        this.removeTrackedEventHandler("keyDown"))

    props


    |> Props.tryFind PKey.view.keyDownNotHandled


    |> Option.iter (fun _ -> 


      if this.isEventHandlerTracked("keyDownNotHandled") then


        Interop.removeEventHandler <@ element.KeyDownNotHandled @> element


        this.removeTrackedEventHandler("keyDownNotHandled"))

    props


    |> Props.tryFind PKey.view.keyUp


    |> Option.iter (fun _ -> 


      if this.isEventHandlerTracked("keyUp") then


        Interop.removeEventHandler <@ element.KeyUp @> element


        this.removeTrackedEventHandler("keyUp"))

    props


    |> Props.tryFind PKey.view.mouseClick


    |> Option.iter (fun _ -> 


      if this.isEventHandlerTracked("mouseClick") then


        Interop.removeEventHandler <@ element.MouseClick @> element


        this.removeTrackedEventHandler("mouseClick"))

    props


    |> Props.tryFind PKey.view.mouseEnter


    |> Option.iter (fun _ -> 


      if this.isEventHandlerTracked("mouseEnter") then


        Interop.removeEventHandler <@ element.MouseEnter @> element


        this.removeTrackedEventHandler("mouseEnter"))

    props


    |> Props.tryFind PKey.view.mouseEvent


    |> Option.iter (fun _ -> 


      if this.isEventHandlerTracked("mouseEvent") then


        Interop.removeEventHandler <@ element.MouseEvent @> element


        this.removeTrackedEventHandler("mouseEvent"))

    props


    |> Props.tryFind PKey.view.mouseLeave


    |> Option.iter (fun _ -> 


      if this.isEventHandlerTracked("mouseLeave") then


        Interop.removeEventHandler <@ element.MouseLeave @> element


        this.removeTrackedEventHandler("mouseLeave"))

    props


    |> Props.tryFind PKey.view.mouseStateChanged


    |> Option.iter (fun _ -> 


      if this.isEventHandlerTracked("mouseStateChanged") then


        Interop.removeEventHandler <@ element.MouseStateChanged @> element


        this.removeTrackedEventHandler("mouseStateChanged"))

    props


    |> Props.tryFind PKey.view.mouseWheel


    |> Option.iter (fun _ -> 


      if this.isEventHandlerTracked("mouseWheel") then


        Interop.removeEventHandler <@ element.MouseWheel @> element


        this.removeTrackedEventHandler("mouseWheel"))

    props


    |> Props.tryFind PKey.view.removed


    |> Option.iter (fun _ -> 


      if this.isEventHandlerTracked("removed") then


        Interop.removeEventHandler <@ element.Removed @> element


        this.removeTrackedEventHandler("removed"))

    props


    |> Props.tryFind PKey.view.schemeChanged


    |> Option.iter (fun _ -> 


      if this.isEventHandlerTracked("schemeChanged") then


        Interop.removeEventHandler <@ element.SchemeChanged @> element


        this.removeTrackedEventHandler("schemeChanged"))

    props


    |> Props.tryFind PKey.view.schemeChanging


    |> Option.iter (fun _ -> 


      if this.isEventHandlerTracked("schemeChanging") then


        Interop.removeEventHandler <@ element.SchemeChanging @> element


        this.removeTrackedEventHandler("schemeChanging"))

    props


    |> Props.tryFind PKey.view.schemeNameChanged


    |> Option.iter (fun _ -> 


      if this.isEventHandlerTracked("schemeNameChanged") then


        Interop.removeEventHandler <@ element.SchemeNameChanged @> element


        this.removeTrackedEventHandler("schemeNameChanged"))

    props


    |> Props.tryFind PKey.view.schemeNameChanging


    |> Option.iter (fun _ -> 


      if this.isEventHandlerTracked("schemeNameChanging") then


        Interop.removeEventHandler <@ element.SchemeNameChanging @> element


        this.removeTrackedEventHandler("schemeNameChanging"))

    props


    |> Props.tryFind PKey.view.selecting


    |> Option.iter (fun _ -> 


      if this.isEventHandlerTracked("selecting") then


        Interop.removeEventHandler <@ element.Selecting @> element


        this.removeTrackedEventHandler("selecting"))

    props


    |> Props.tryFind PKey.view.subViewAdded


    |> Option.iter (fun _ -> 


      if this.isEventHandlerTracked("subViewAdded") then


        Interop.removeEventHandler <@ element.SubViewAdded @> element


        this.removeTrackedEventHandler("subViewAdded"))

    props


    |> Props.tryFind PKey.view.subViewLayout


    |> Option.iter (fun _ -> 


      if this.isEventHandlerTracked("subViewLayout") then


        Interop.removeEventHandler <@ element.SubViewLayout @> element


        this.removeTrackedEventHandler("subViewLayout"))

    props


    |> Props.tryFind PKey.view.subViewRemoved


    |> Option.iter (fun _ -> 


      if this.isEventHandlerTracked("subViewRemoved") then


        Interop.removeEventHandler <@ element.SubViewRemoved @> element


        this.removeTrackedEventHandler("subViewRemoved"))

    props


    |> Props.tryFind PKey.view.subViewsLaidOut


    |> Option.iter (fun _ -> 


      if this.isEventHandlerTracked("subViewsLaidOut") then


        Interop.removeEventHandler <@ element.SubViewsLaidOut @> element


        this.removeTrackedEventHandler("subViewsLaidOut"))

    props


    |> Props.tryFind PKey.view.superViewChanged


    |> Option.iter (fun _ -> 


      if this.isEventHandlerTracked("superViewChanged") then


        Interop.removeEventHandler <@ element.SuperViewChanged @> element


        this.removeTrackedEventHandler("superViewChanged"))

    props


    |> Props.tryFind PKey.view.textChanged


    |> Option.iter (fun _ -> 


      if this.isEventHandlerTracked("textChanged") then


        Interop.removeEventHandler <@ element.TextChanged @> element


        this.removeTrackedEventHandler("textChanged"))

    props


    |> Props.tryFind PKey.view.titleChanged


    |> Option.iter (fun _ -> 


      if this.isEventHandlerTracked("titleChanged") then


        Interop.removeEventHandler <@ element.TitleChanged @> element


        this.removeTrackedEventHandler("titleChanged"))

    props


    |> Props.tryFind PKey.view.titleChanging


    |> Option.iter (fun _ -> 


      if this.isEventHandlerTracked("titleChanging") then


        Interop.removeEventHandler <@ element.TitleChanging @> element


        this.removeTrackedEventHandler("titleChanging"))

    props


    |> Props.tryFind PKey.view.viewportChanged


    |> Option.iter (fun _ -> 


      if this.isEventHandlerTracked("viewportChanged") then


        Interop.removeEventHandler <@ element.ViewportChanged @> element


        this.removeTrackedEventHandler("viewportChanged"))

    props


    |> Props.tryFind PKey.view.visibleChanged


    |> Option.iter (fun _ -> 


      if this.isEventHandlerTracked("visibleChanged") then


        Interop.removeEventHandler <@ element.VisibleChanged @> element


        this.removeTrackedEventHandler("visibleChanged"))

    props


    |> Props.tryFind PKey.view.visibleChanging


    |> Option.iter (fun _ -> 


      if this.isEventHandlerTracked("visibleChanging") then


        Interop.removeEventHandler <@ element.VisibleChanging @> element


        this.removeTrackedEventHandler("visibleChanging"))

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
    |> Option.iter (fun _ -> element.Diagnostics <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.adornment.superViewRendersLineCanvas
    |> Option.iter (fun _ -> element.SuperViewRendersLineCanvas <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.adornment.thickness
    |> Option.iter (fun _ -> element.Thickness <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.adornment.viewport
    |> Option.iter (fun _ -> element.Viewport <- Unchecked.defaultof<_>)
    // Events
    props

    |> Props.tryFind PKey.adornment.thicknessChanged

    |> Option.iter (fun _ -> 

      if this.isEventHandlerTracked("thicknessChanged") then

        Interop.removeEventHandler <@ element.ThicknessChanged @> element

        this.removeTrackedEventHandler("thicknessChanged"))

  override _.name = $"Adornment"

  override this.setProps(element: View, props: Props) =
    base.setProps (element, props)

    let element = element :?> Adornment

    // Properties
    props
    |> Props.tryFind PKey.adornment.diagnostics
    |> Option.iter (fun v -> element.Diagnostics <- v)

    props
    |> Props.tryFind PKey.adornment.superViewRendersLineCanvas
    |> Option.iter (fun v -> element.SuperViewRendersLineCanvas <- v)

    props
    |> Props.tryFind PKey.adornment.thickness
    |> Option.iter (fun v -> element.Thickness <- v)

    props
    |> Props.tryFind PKey.adornment.viewport
    |> Option.iter (fun v -> element.Viewport <- v)
    // Events
    props
    |> Props.tryFind PKey.adornment.thicknessChanged
    |> Option.iter (fun v -> Interop.setEventHandler <@ element.ThicknessChanged @> (fun _ -> v ()) element)

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
    |> Option.iter (fun _ -> element.AlignmentModes <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.bar.orientation
    |> Option.iter (fun _ -> element.Orientation <- Unchecked.defaultof<_>)
    // Events
    props

    |> Props.tryFind PKey.bar.orientationChanged

    |> Option.iter (fun _ -> 

      if this.isEventHandlerTracked("orientationChanged") then

        Interop.removeEventHandler <@ element.OrientationChanged @> element

        this.removeTrackedEventHandler("orientationChanged"))

    props


    |> Props.tryFind PKey.bar.orientationChanging


    |> Option.iter (fun _ -> 


      if this.isEventHandlerTracked("orientationChanging") then


        Interop.removeEventHandler <@ element.OrientationChanging @> element


        this.removeTrackedEventHandler("orientationChanging"))

  override _.name = $"Bar"

  override this.setProps(element: View, props: Props) =
    base.setProps (element, props)

    let element = element :?> Bar
    // Properties
    props
    |> Props.tryFind PKey.bar.alignmentModes
    |> Option.iter (fun v -> element.AlignmentModes <- v)

    props
    |> Props.tryFind PKey.bar.orientation
    |> Option.iter (fun v -> element.Orientation <- v)
    // Events
    props

    |> Props.tryFind PKey.bar.orientationChanged

    |> Option.iter (fun v -> 

      Interop.setEventHandler <@ element.OrientationChanged @> (fun arg -> v arg.Value) element

      this.trackEventHandler("orientationChanged"))

    props


    |> Props.tryFind PKey.bar.orientationChanging


    |> Option.iter (fun v -> 


      Interop.setEventHandler <@ element.OrientationChanging @> v element


      this.trackEventHandler("orientationChanging"))


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
    |> Option.iter (fun _ -> element.LineStyle <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.border.settings
    |> Option.iter (fun _ -> element.Settings <- Unchecked.defaultof<_>)

  override _.name = $"Border"

  override this.setProps(element: View, props: Props) =
    base.setProps (element, props)

    let element = element :?> Border

    // Properties
    props
    |> Props.tryFind PKey.border.lineStyle
    |> Option.iter (fun v -> element.LineStyle <- v)

    props
    |> Props.tryFind PKey.border.settings
    |> Option.iter (fun v -> element.Settings <- v)


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
    |> Option.iter (fun _ -> element.HotKeySpecifier <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.button.isDefault
    |> Option.iter (fun _ -> element.IsDefault <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.button.noDecorations
    |> Option.iter (fun _ -> element.NoDecorations <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.button.noPadding
    |> Option.iter (fun _ -> element.NoPadding <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.button.text
    |> Option.iter (fun _ -> element.Text <- Unchecked.defaultof<_>)
    // Events
    props
    |> Props.tryFind PKey.button.wantContinuousButtonPressed
    |> Option.iter (fun _ -> element.WantContinuousButtonPressed <- Unchecked.defaultof<_>)

  override _.name = $"Button"

  override this.setProps(element: View, props: Props) =
    base.setProps (element, props)

    let element = element :?> Button

    // Properties
    props
    |> Props.tryFind PKey.button.hotKeySpecifier
    |> Option.iter (fun v -> element.HotKeySpecifier <- v)

    props
    |> Props.tryFind PKey.button.isDefault
    |> Option.iter (fun v -> element.IsDefault <- v)

    props
    |> Props.tryFind PKey.button.noDecorations
    |> Option.iter (fun v -> element.NoDecorations <- v)

    props
    |> Props.tryFind PKey.button.noPadding
    |> Option.iter (fun v -> element.NoPadding <- v)

    props
    |> Props.tryFind PKey.button.text
    |> Option.iter (fun v -> element.Text <- v)
    // Events
    props
    |> Props.tryFind PKey.button.wantContinuousButtonPressed
    |> Option.iter (fun v -> element.WantContinuousButtonPressed <- v)


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
    |> Option.iter (fun _ -> element.AllowCheckStateNone <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.checkBox.checkedState
    |> Option.iter (fun _ -> element.CheckedState <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.checkBox.hotKeySpecifier
    |> Option.iter (fun _ -> element.HotKeySpecifier <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.checkBox.radioStyle
    |> Option.iter (fun _ -> element.RadioStyle <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.checkBox.text
    |> Option.iter (fun _ -> element.Text <- Unchecked.defaultof<_>)
    // Events
    props

    |> Props.tryFind PKey.checkBox.checkedStateChanging

    |> Option.iter (fun _ -> 

      if this.isEventHandlerTracked("checkedStateChanging") then

        Interop.removeEventHandler <@ element.CheckedStateChanging @> element

        this.removeTrackedEventHandler("checkedStateChanging"))

    props


    |> Props.tryFind PKey.checkBox.checkedStateChanged


    |> Option.iter (fun _ -> 


      if this.isEventHandlerTracked("checkedStateChanged") then


        Interop.removeEventHandler <@ element.CheckedStateChanged @> element


        this.removeTrackedEventHandler("checkedStateChanged"))

  override _.name = $"CheckBox"

  override this.setProps(element: View, props: Props) =
    base.setProps (element, props)

    let element = element :?> CheckBox

    // Properties
    props
    |> Props.tryFind PKey.checkBox.allowCheckStateNone
    |> Option.iter (fun v -> element.AllowCheckStateNone <- v)

    props
    |> Props.tryFind PKey.checkBox.checkedState
    |> Option.iter (fun v -> element.CheckedState <- v)

    props
    |> Props.tryFind PKey.checkBox.hotKeySpecifier
    |> Option.iter (fun v -> element.HotKeySpecifier <- v)

    props
    |> Props.tryFind PKey.checkBox.radioStyle
    |> Option.iter (fun v -> element.RadioStyle <- v)

    props
    |> Props.tryFind PKey.checkBox.text
    |> Option.iter (fun v -> element.Text <- v)
    // Events
    props

    |> Props.tryFind PKey.checkBox.checkedStateChanging

    |> Option.iter (fun v -> 

      Interop.setEventHandler <@ element.CheckedStateChanging @> v element

      this.trackEventHandler("checkedStateChanging"))

    props


    |> Props.tryFind PKey.checkBox.checkedStateChanged


    |> Option.iter (fun v -> 


      Interop.setEventHandler <@ element.CheckedStateChanged @> v element


      this.trackEventHandler("checkedStateChanged"))


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
    |> Option.iter (fun _ -> element.SelectedColor <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.colorPicker.style
    |> Option.iter (fun _ -> element.Style <- Unchecked.defaultof<_>)
    // Events
    props

    |> Props.tryFind PKey.colorPicker.colorChanged

    |> Option.iter (fun _ -> 

      if this.isEventHandlerTracked("colorChanged") then

        Interop.removeEventHandler <@ element.ColorChanged @> element

        this.removeTrackedEventHandler("colorChanged"))

  override _.name = $"ColorPicker"

  override this.setProps(element: View, props: Props) =
    base.setProps (element, props)

    let element = element :?> ColorPicker

    // Properties
    props
    |> Props.tryFind PKey.colorPicker.selectedColor
    |> Option.iter (fun v -> element.SelectedColor <- v)

    props
    |> Props.tryFind PKey.colorPicker.style
    |> Option.iter (fun v -> element.Style <- v)
    // Events
    props

    |> Props.tryFind PKey.colorPicker.colorChanged

    |> Option.iter (fun v -> 

      Interop.setEventHandler <@ element.ColorChanged @> v element

      this.trackEventHandler("colorChanged"))


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
    |> Option.iter (fun _ -> element.BoxHeight <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.colorPicker16.boxWidth
    |> Option.iter (fun _ -> element.BoxWidth <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.colorPicker16.cursor
    |> Option.iter (fun _ -> element.Cursor <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.colorPicker16.selectedColor
    |> Option.iter (fun _ -> element.SelectedColor <- Unchecked.defaultof<_>)
    // Events
    props

    |> Props.tryFind PKey.colorPicker16.colorChanged

    |> Option.iter (fun _ -> 

      if this.isEventHandlerTracked("colorChanged") then

        Interop.removeEventHandler <@ element.ColorChanged @> element

        this.removeTrackedEventHandler("colorChanged"))

  override _.name = $"ColorPicker16"

  override this.setProps(element: View, props: Props) =
    base.setProps (element, props)

    let element = element :?> ColorPicker16

    // Properties
    props
    |> Props.tryFind PKey.colorPicker16.boxHeight
    |> Option.iter (fun v -> element.BoxHeight <- v)

    props
    |> Props.tryFind PKey.colorPicker16.boxWidth
    |> Option.iter (fun v -> element.BoxWidth <- v)

    props
    |> Props.tryFind PKey.colorPicker16.cursor
    |> Option.iter (fun v -> element.Cursor <- v)

    props
    |> Props.tryFind PKey.colorPicker16.selectedColor
    |> Option.iter (fun v -> element.SelectedColor <- v)
    // Events
    props

    |> Props.tryFind PKey.colorPicker16.colorChanged

    |> Option.iter (fun v -> 

      Interop.setEventHandler <@ element.ColorChanged @> v element

      this.trackEventHandler("colorChanged"))


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
    |> Option.iter (fun _ -> element.HideDropdownListOnClick <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.comboBox.readOnly
    |> Option.iter (fun _ -> element.ReadOnly <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.comboBox.searchText
    |> Option.iter (fun _ -> element.SearchText <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.comboBox.selectedItem
    |> Option.iter (fun _ -> element.SelectedItem <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.comboBox.source
    |> Option.iter (fun _ -> element.SetSource Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.comboBox.text
    |> Option.iter (fun _ -> element.Text <- Unchecked.defaultof<_>)
    // Events
    props

    |> Props.tryFind PKey.comboBox.collapsed

    |> Option.iter (fun _ -> 

      if this.isEventHandlerTracked("collapsed") then

        Interop.removeEventHandler <@ element.Collapsed @> element

        this.removeTrackedEventHandler("collapsed"))

    props


    |> Props.tryFind PKey.comboBox.expanded


    |> Option.iter (fun _ -> 


      if this.isEventHandlerTracked("expanded") then


        Interop.removeEventHandler <@ element.Expanded @> element


        this.removeTrackedEventHandler("expanded"))

    props


    |> Props.tryFind PKey.comboBox.openSelectedItem


    |> Option.iter (fun _ -> 


      if this.isEventHandlerTracked("openSelectedItem") then


        Interop.removeEventHandler <@ element.OpenSelectedItem @> element


        this.removeTrackedEventHandler("openSelectedItem"))

    props


    |> Props.tryFind PKey.comboBox.selectedItemChanged


    |> Option.iter (fun _ -> 


      if this.isEventHandlerTracked("selectedItemChanged") then


        Interop.removeEventHandler <@ element.SelectedItemChanged @> element


        this.removeTrackedEventHandler("selectedItemChanged"))

  override _.name = $"ComboBox"

  override this.setProps(element: View, props: Props) =
    base.setProps (element, props)

    let element = element :?> ComboBox

    // Properties
    props
    |> Props.tryFind PKey.comboBox.hideDropdownListOnClick
    |> Option.iter (fun v -> element.HideDropdownListOnClick <- v)

    props
    |> Props.tryFind PKey.comboBox.readOnly
    |> Option.iter (fun v -> element.ReadOnly <- v)

    props
    |> Props.tryFind PKey.comboBox.searchText
    |> Option.iter (fun v -> element.SearchText <- v)

    props
    |> Props.tryFind PKey.comboBox.selectedItem
    |> Option.iter (fun v -> element.SelectedItem <- v)

    props
    |> Props.tryFind PKey.comboBox.source
    |> Option.iter (fun v -> element.SetSource(ObservableCollection(v)))

    props
    |> Props.tryFind PKey.comboBox.text
    |> Option.iter (fun v -> element.Text <- v)
    // Events
    props
    |> Props.tryFind PKey.comboBox.collapsed
    |> Option.iter (fun v -> Interop.setEventHandler <@ element.Collapsed @> (fun _ -> v ()) element)

    props
    |> Props.tryFind PKey.comboBox.expanded
    |> Option.iter (fun v -> Interop.setEventHandler <@ element.Expanded @> (fun _ -> v ()) element)

    props


    |> Props.tryFind PKey.comboBox.openSelectedItem


    |> Option.iter (fun v -> 


      Interop.setEventHandler <@ element.OpenSelectedItem @> v element


      this.trackEventHandler("openSelectedItem"))

    props


    |> Props.tryFind PKey.comboBox.selectedItemChanged


    |> Option.iter (fun v -> 


      Interop.setEventHandler <@ element.SelectedItemChanged @> v element


      this.trackEventHandler("selectedItemChanged"))


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
    |> Option.iter (fun _ -> element.Culture <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.dateField.cursorPosition
    |> Option.iter (fun _ -> element.CursorPosition <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.dateField.date
    |> Option.iter (fun _ -> element.Date <- Unchecked.defaultof<_>)
    // Events
    props

    |> Props.tryFind PKey.dateField.dateChanged

    |> Option.iter (fun _ -> 

      if this.isEventHandlerTracked("dateChanged") then

        Interop.removeEventHandler <@ element.DateChanged @> element

        this.removeTrackedEventHandler("dateChanged"))

  override _.name = $"DateField"

  override this.setProps(element: View, props: Props) =
    base.setProps (element, props)

    let element = element :?> DateField

    // Properties
    props
    |> Props.tryFind PKey.dateField.culture
    |> Option.iter (fun v -> element.Culture <- v)

    props
    |> Props.tryFind PKey.dateField.cursorPosition
    |> Option.iter (fun v -> element.CursorPosition <- v)

    props
    |> Props.tryFind PKey.dateField.date
    |> Option.iter (fun v -> element.Date <- v)
    // Events
    props

    |> Props.tryFind PKey.dateField.dateChanged

    |> Option.iter (fun v -> 

      Interop.setEventHandler <@ element.DateChanged @> v element

      this.trackEventHandler("dateChanged"))


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
    |> Option.iter (fun _ -> element.Culture <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.datePicker.date
    |> Option.iter (fun _ -> element.Date <- Unchecked.defaultof<_>)

  override _.name = $"DatePicker"

  override this.setProps(element: View, props: Props) =
    base.setProps (element, props)

    let element = element :?> DatePicker

    // Properties
    props
    |> Props.tryFind PKey.datePicker.culture
    |> Option.iter (fun v -> element.Culture <- v)

    props
    |> Props.tryFind PKey.datePicker.date
    |> Option.iter (fun v -> element.Date <- v)


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
    |> Option.iter (fun _ -> element.ButtonAlignment <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.dialog.buttonAlignmentModes
    |> Option.iter (fun _ -> element.ButtonAlignmentModes <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.dialog.canceled
    |> Option.iter (fun _ -> element.Canceled <- Unchecked.defaultof<_>)

  override _.name = $"Dialog"

  override this.setProps(element: View, props: Props) =
    base.setProps (element, props)

    let element = element :?> Dialog

    // Properties
    props
    |> Props.tryFind PKey.dialog.buttonAlignment
    |> Option.iter (fun v -> element.ButtonAlignment <- v)

    props
    |> Props.tryFind PKey.dialog.buttonAlignmentModes
    |> Option.iter (fun v -> element.ButtonAlignmentModes <- v)

    props
    |> Props.tryFind PKey.dialog.canceled
    |> Option.iter (fun v -> element.Canceled <- v)


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
    |> Option.iter (fun _ -> element.AllowedTypes <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.fileDialog.allowsMultipleSelection
    |> Option.iter (fun _ -> element.AllowsMultipleSelection <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.fileDialog.fileOperationsHandler
    |> Option.iter (fun _ -> element.FileOperationsHandler <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.fileDialog.mustExist
    |> Option.iter (fun _ -> element.MustExist <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.fileDialog.openMode
    |> Option.iter (fun _ -> element.OpenMode <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.fileDialog.path
    |> Option.iter (fun _ -> element.Path <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.fileDialog.searchMatcher
    |> Option.iter (fun _ -> element.SearchMatcher <- Unchecked.defaultof<_>)
    // Events
    props

    |> Props.tryFind PKey.fileDialog.filesSelected

    |> Option.iter (fun _ -> 

      if this.isEventHandlerTracked("filesSelected") then

        Interop.removeEventHandler <@ element.FilesSelected @> element

        this.removeTrackedEventHandler("filesSelected"))

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
    |> Option.iter (fun v -> element.AllowsMultipleSelection <- v)

    props
    |> Props.tryFind PKey.fileDialog.fileOperationsHandler
    |> Option.iter (fun v -> element.FileOperationsHandler <- v)

    props
    |> Props.tryFind PKey.fileDialog.mustExist
    |> Option.iter (fun v -> element.MustExist <- v)

    props
    |> Props.tryFind PKey.fileDialog.openMode
    |> Option.iter (fun v -> element.OpenMode <- v)

    props
    |> Props.tryFind PKey.fileDialog.path
    |> Option.iter (fun v -> element.Path <- v)

    props
    |> Props.tryFind PKey.fileDialog.searchMatcher
    |> Option.iter (fun v -> element.SearchMatcher <- v)
    // Events
    props

    |> Props.tryFind PKey.fileDialog.filesSelected

    |> Option.iter (fun v -> 

      Interop.setEventHandler <@ element.FilesSelected @> v element

      this.trackEventHandler("filesSelected"))


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
    |> Option.iter (fun _ -> element.AxisX <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.graphView.axisY
    |> Option.iter (fun _ -> element.AxisY <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.graphView.cellSize
    |> Option.iter (fun _ -> element.CellSize <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.graphView.graphColor
    |> Option.iter (fun _ -> element.GraphColor <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.graphView.marginBottom
    |> Option.iter (fun _ -> element.MarginBottom <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.graphView.marginLeft
    |> Option.iter (fun _ -> element.MarginLeft <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.graphView.scrollOffset
    |> Option.iter (fun _ -> element.ScrollOffset <- Unchecked.defaultof<_>)

  override _.name = $"GraphView"

  override this.setProps(element: View, props: Props) =
    base.setProps (element, props)

    let element = element :?> GraphView

    // Properties
    props
    |> Props.tryFind PKey.graphView.axisX
    |> Option.iter (fun v -> element.AxisX <- v)

    props
    |> Props.tryFind PKey.graphView.axisY
    |> Option.iter (fun v -> element.AxisY <- v)

    props
    |> Props.tryFind PKey.graphView.cellSize
    |> Option.iter (fun v -> element.CellSize <- v)

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
    |> Option.iter (fun v -> element.ScrollOffset <- v)


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
    |> Option.iter (fun _ -> element.Address <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.hexView.addressWidth
    |> Option.iter (fun _ -> element.AddressWidth <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.hexView.allowEdits
    |> Option.iter (fun _ -> element.BytesPerLine <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.hexView.readOnly
    |> Option.iter (fun _ -> element.ReadOnly <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.hexView.source
    |> Option.iter (fun _ -> element.Source <- Unchecked.defaultof<_>)
    // Events
    props

    |> Props.tryFind PKey.hexView.edited

    |> Option.iter (fun _ -> 

      if this.isEventHandlerTracked("edited") then

        Interop.removeEventHandler <@ element.Edited @> element

        this.removeTrackedEventHandler("edited"))

    props


    |> Props.tryFind PKey.hexView.positionChanged


    |> Option.iter (fun _ -> 


      if this.isEventHandlerTracked("positionChanged") then


        Interop.removeEventHandler <@ element.PositionChanged @> element


        this.removeTrackedEventHandler("positionChanged"))

  override _.name = $"HexView"

  override this.setProps(element: View, props: Props) =
    base.setProps (element, props)

    let element = element :?> HexView

    // Properties
    props
    |> Props.tryFind PKey.hexView.address
    |> Option.iter (fun v -> element.Address <- v)

    props
    |> Props.tryFind PKey.hexView.addressWidth
    |> Option.iter (fun v -> element.AddressWidth <- v)

    props
    |> Props.tryFind PKey.hexView.allowEdits
    |> Option.iter (fun v -> element.BytesPerLine <- v)

    props
    |> Props.tryFind PKey.hexView.readOnly
    |> Option.iter (fun v -> element.ReadOnly <- v)

    props
    |> Props.tryFind PKey.hexView.source
    |> Option.iter (fun v -> element.Source <- v)
    // Events
    props

    |> Props.tryFind PKey.hexView.edited

    |> Option.iter (fun v -> 

      Interop.setEventHandler <@ element.Edited @> v element

      this.trackEventHandler("edited"))

    props


    |> Props.tryFind PKey.hexView.positionChanged


    |> Option.iter (fun v -> 


      Interop.setEventHandler <@ element.PositionChanged @> v element


      this.trackEventHandler("positionChanged"))


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
    |> Option.iter (fun _ -> element.HotKeySpecifier <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.label.text
    |> Option.iter (fun _ -> element.Text <- Unchecked.defaultof<_>)

  override _.name = $"Label"

  override this.setProps(element: View, props: Props) =
    base.setProps (element, props)

    let element = element :?> Label

    // Properties
    props
    |> Props.tryFind PKey.label.hotKeySpecifier
    |> Option.iter (fun v -> element.HotKeySpecifier <- v)

    props
    |> Props.tryFind PKey.label.text
    |> Option.iter (fun v -> element.Text <- v)


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
    |> Option.iter (fun _ -> element.Orientation <- Unchecked.defaultof<_>)
    // Events
    props

    |> Props.tryFind PKey.line.orientationChanged

    |> Option.iter (fun _ -> 

      if this.isEventHandlerTracked("orientationChanged") then

        Interop.removeEventHandler <@ element.OrientationChanged @> element

        this.removeTrackedEventHandler("orientationChanged"))

    props


    |> Props.tryFind PKey.line.orientationChanging


    |> Option.iter (fun _ -> 


      if this.isEventHandlerTracked("orientationChanging") then


        Interop.removeEventHandler <@ element.OrientationChanging @> element


        this.removeTrackedEventHandler("orientationChanging"))

  override _.name = $"Line"

  override this.setProps(element: View, props: Props) =
    base.setProps (element, props)

    let element = element :?> Line

    // Properties
    props
    |> Props.tryFind PKey.line.orientation
    |> Option.iter (fun v -> element.Orientation <- v)
    // Events
    props

    |> Props.tryFind PKey.line.orientationChanged

    |> Option.iter (fun v -> 

      Interop.setEventHandler <@ element.OrientationChanged @> (fun arg -> v arg.Value) element

      this.trackEventHandler("orientationChanged"))

    props


    |> Props.tryFind PKey.line.orientationChanging


    |> Option.iter (fun v -> 


      Interop.setEventHandler <@ element.OrientationChanging @> v element


      this.trackEventHandler("orientationChanging"))


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
    |> Option.iter (fun _ -> element.AllowsMarking <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.listView.allowsMultipleSelection
    |> Option.iter (fun _ -> element.AllowsMultipleSelection <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.listView.leftItem
    |> Option.iter (fun _ -> element.LeftItem <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.listView.selectedItem
    |> Option.iter (fun _ -> element.SelectedItem <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.listView.source
    |> Option.iter (fun _ -> element.SetSource Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.listView.topItem
    |> Option.iter (fun _ -> element.TopItem <- Unchecked.defaultof<_>)
    // Events
    props

    |> Props.tryFind PKey.listView.collectionChanged

    |> Option.iter (fun _ -> 

      if this.isEventHandlerTracked("collectionChanged") then

        Interop.removeEventHandler <@ element.CollectionChanged @> element

        this.removeTrackedEventHandler("collectionChanged"))

    props


    |> Props.tryFind PKey.listView.openSelectedItem


    |> Option.iter (fun _ -> 


      if this.isEventHandlerTracked("openSelectedItem") then


        Interop.removeEventHandler <@ element.OpenSelectedItem @> element


        this.removeTrackedEventHandler("openSelectedItem"))

    props


    |> Props.tryFind PKey.listView.rowRender


    |> Option.iter (fun _ -> 


      if this.isEventHandlerTracked("rowRender") then


        Interop.removeEventHandler <@ element.RowRender @> element


        this.removeTrackedEventHandler("rowRender"))

    props


    |> Props.tryFind PKey.listView.selectedItemChanged


    |> Option.iter (fun _ -> 


      if this.isEventHandlerTracked("selectedItemChanged") then


        Interop.removeEventHandler <@ element.SelectedItemChanged @> element


        this.removeTrackedEventHandler("selectedItemChanged"))

  override _.name = $"ListView"

  override this.setProps(element: View, props: Props) =
    base.setProps (element, props)

    let element = element :?> ListView

    // Properties
    props
    |> Props.tryFind PKey.listView.allowsMarking
    |> Option.iter (fun v -> element.AllowsMarking <- v)

    props
    |> Props.tryFind PKey.listView.allowsMultipleSelection
    |> Option.iter (fun v -> element.AllowsMultipleSelection <- v)

    props
    |> Props.tryFind PKey.listView.leftItem
    |> Option.iter (fun v -> element.LeftItem <- v)

    props
    |> Props.tryFind PKey.listView.selectedItem
    |> Option.iter (fun v -> element.SelectedItem <- v)

    props
    |> Props.tryFind PKey.listView.source
    |> Option.iter (fun v -> element.SetSource(ObservableCollection(v)))

    props
    |> Props.tryFind PKey.listView.topItem
    |> Option.iter (fun v -> element.TopItem <- v)
    // Events
    props

    |> Props.tryFind PKey.listView.collectionChanged

    |> Option.iter (fun v -> 

      Interop.setEventHandler <@ element.CollectionChanged @> v element

      this.trackEventHandler("collectionChanged"))

    props


    |> Props.tryFind PKey.listView.openSelectedItem


    |> Option.iter (fun v -> 


      Interop.setEventHandler <@ element.OpenSelectedItem @> v element


      this.trackEventHandler("openSelectedItem"))

    props


    |> Props.tryFind PKey.listView.rowRender


    |> Option.iter (fun v -> 


      Interop.setEventHandler <@ element.RowRender @> v element


      this.trackEventHandler("rowRender"))

    props


    |> Props.tryFind PKey.listView.selectedItemChanged


    |> Option.iter (fun v -> 


      Interop.setEventHandler <@ element.SelectedItemChanged @> v element


      this.trackEventHandler("selectedItemChanged"))


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
    |> Option.iter (fun _ -> element.ShadowStyle <- Unchecked.defaultof<_>)

  override _.name = $"Margin"

  override this.setProps(element: View, props: Props) =
    base.setProps (element, props)

    let element = element :?> Margin

    // Properties
    props
    |> Props.tryFind PKey.margin.shadowStyle
    |> Option.iter (fun v -> element.ShadowStyle <- v)


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
    |> Option.iter (fun _ -> element.SelectedMenuItem <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.menu.superMenuItem
    |> Option.iter (fun _ -> element.SuperMenuItem <- Unchecked.defaultof<_>)
    // Events
    props

    |> Props.tryFind PKey.menu.accepted

    |> Option.iter (fun v -> 

      Interop.setEventHandler <@ element.Accepted @> v element

      this.trackEventHandler("accepted"))

    props


    |> Props.tryFind PKey.menu.selectedMenuItemChanged


    |> Option.iter (fun v -> 


      Interop.setEventHandler <@ element.SelectedMenuItemChanged @> v element


      this.trackEventHandler("selectedMenuItemChanged"))

    ()

  override _.name = $"Menu"

  override this.setProps(element: View, props: Props) =
    base.setProps (element, props)

    let element = element :?> Menu

    // Properties
    props
    |> Props.tryFind PKey.menu.selectedMenuItem
    |> Option.iter (fun v -> element.SelectedMenuItem <- v)

    props
    |> Props.tryFind PKey.menu.superMenuItem
    |> Option.iter (fun v -> element.SuperMenuItem <- v)
    // Events
    props

    |> Props.tryFind PKey.menu.accepted

    |> Option.iter (fun v -> 

      Interop.setEventHandler <@ element.Accepted @> v element

      this.trackEventHandler("accepted"))

    props


    |> Props.tryFind PKey.menu.selectedMenuItemChanged


    |> Option.iter (fun v -> 


      Interop.setEventHandler <@ element.SelectedMenuItemChanged @> v element


      this.trackEventHandler("selectedMenuItemChanged"))

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
    |> Option.iter (fun _ -> element.Key <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.popoverMenu.mouseFlags
    |> Option.iter (fun _ -> element.MouseFlags <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.popoverMenu.root
    |> Option.iter (fun _ -> element.Root <- Unchecked.defaultof<_>)
    // Events
    props

    |> Props.tryFind PKey.popoverMenu.accepted

    |> Option.iter (fun _ -> 

      if this.isEventHandlerTracked("accepted") then

        Interop.removeEventHandler <@ element.Accepted @> element

        this.removeTrackedEventHandler("accepted"))

    props


    |> Props.tryFind PKey.popoverMenu.keyChanged


    |> Option.iter (fun _ -> 


      if this.isEventHandlerTracked("keyChanged") then


        Interop.removeEventHandler <@ element.KeyChanged @> element


        this.removeTrackedEventHandler("keyChanged"))

  override this.name = "PopoverMenu"

  override this.setProps(element: View, props: Props) =
    base.setProps (element, props)

    let element = element :?> PopoverMenu

    // Properties
    props
    |> Props.tryFind PKey.popoverMenu.key
    |> Option.iter (fun v -> element.Key <- v)

    props
    |> Props.tryFind PKey.popoverMenu.mouseFlags
    |> Option.iter (fun v -> element.MouseFlags <- v)

    props
    |> Props.tryFind PKey.popoverMenu.root
    |> Option.iter (fun v -> element.Root <- v)
    // Events
    props

    |> Props.tryFind PKey.popoverMenu.accepted

    |> Option.iter (fun v -> 

      Interop.setEventHandler <@ element.Accepted @> v element

      this.trackEventHandler("accepted"))

    props


    |> Props.tryFind PKey.popoverMenu.keyChanged


    |> Option.iter (fun v -> 


      Interop.setEventHandler <@ element.KeyChanged @> v element


      this.trackEventHandler("keyChanged"))

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
    |> Option.iter (fun _ -> element.PopoverMenu <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.menuBarItem.popoverMenuOpen
    |> Option.iter (fun _ -> element.PopoverMenuOpen <- Unchecked.defaultof<_>)
    // Events
    props

    |> Props.tryFind PKey.menuBarItem.popoverMenuOpenChanged

    |> Option.iter (fun _ -> 

      if this.isEventHandlerTracked("popoverMenuOpenChanged") then

        Interop.removeEventHandler <@ element.PopoverMenuOpenChanged @> element

        this.removeTrackedEventHandler("popoverMenuOpenChanged"))

  override this.name = "MenuBarItem"

  override this.setProps(element: View, props: Props) =
    base.setProps (element, props)

    let element = element :?> MenuBarItem

    // Properties
    props
    |> Props.tryFind PKey.menuBarItem.popoverMenu
    |> Option.iter (fun v -> element.PopoverMenu <- v)

    props
    |> Props.tryFind PKey.menuBarItem.popoverMenuOpen
    |> Option.iter (fun v -> element.PopoverMenuOpen <- v)
    // Events
    props

    |> Props.tryFind PKey.menuBarItem.popoverMenuOpenChanged

    |> Option.iter (fun v -> 

      Interop.setEventHandler <@ element.PopoverMenuOpenChanged @> (fun args -> v args.Value) element

      this.trackEventHandler("popoverMenuOpenChanged"))

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
    |> Option.iter (fun _ -> element.Key <- Unchecked.defaultof<_>)

    // NOTE: No need to handle `Menus: MenuBarItemElement list` property here,
    //       as it already registered as "children" property.
    //       And "children" properties are handled by the TreeDiff initializeTree function

    // Events
    props

    |> Props.tryFind PKey.menuBar.keyChanged

    |> Option.iter (fun _ -> 

      if this.isEventHandlerTracked("keyChanged") then

        Interop.removeEventHandler <@ element.KeyChanged @> element

        this.removeTrackedEventHandler("keyChanged"))

  override _.name = $"MenuBar"

  override this.setProps(element: View, props: Props) =
    base.setProps (element, props)

    let element = element :?> MenuBar

    // Properties
    props
    |> Props.tryFind PKey.menuBar.key
    |> Option.iter (fun v -> element.Key <- v)

    // NOTE: No need to handle `Menus: MenuBarItemElement list` property here,
    //       as it already registered as "children" property.
    //       And "children" properties are handled by the TreeDiff initializeTree function

    // Events
    props

    |> Props.tryFind PKey.menuBar.keyChanged

    |> Option.iter (fun v -> 

      Interop.setEventHandler <@ element.KeyChanged @> v element

      this.trackEventHandler("keyChanged"))

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
    |> Option.iter (fun _ -> element.Action <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.shortcut.alignmentModes
    |> Option.iter (fun _ -> element.AlignmentModes <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.shortcut.commandView
    |> Option.iter (fun _ -> element.CommandView <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.shortcut.forceFocusColors
    |> Option.iter (fun _ -> element.ForceFocusColors <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.shortcut.helpText
    |> Option.iter (fun _ -> element.HelpText <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.shortcut.text
    |> Option.iter (fun _ -> element.Text <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.shortcut.bindKeyToApplication
    |> Option.iter (fun _ -> element.BindKeyToApplication <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.shortcut.key
    |> Option.iter (fun _ -> element.Key <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.shortcut.minimumKeyTextSize
    |> Option.iter (fun _ -> element.MinimumKeyTextSize <- Unchecked.defaultof<_>)
    // Events
    props

    |> Props.tryFind PKey.shortcut.orientationChanged

    |> Option.iter (fun _ -> 

      if this.isEventHandlerTracked("orientationChanged") then

        Interop.removeEventHandler <@ element.OrientationChanged @> element

        this.removeTrackedEventHandler("orientationChanged"))

    props


    |> Props.tryFind PKey.shortcut.orientationChanging


    |> Option.iter (fun _ -> 


      if this.isEventHandlerTracked("orientationChanging") then


        Interop.removeEventHandler <@ element.OrientationChanging @> element


        this.removeTrackedEventHandler("orientationChanging"))

  override _.name = $"Shortcut"

  override this.setProps(element: View, props: Props) =
    base.setProps (element, props)

    let element = element :?> Shortcut

    // Properties
    props
    |> Props.tryFind PKey.shortcut.action
    |> Option.iter (fun v -> element.Action <- v)

    props
    |> Props.tryFind PKey.shortcut.alignmentModes
    |> Option.iter (fun v -> element.AlignmentModes <- v)

    props
    |> Props.tryFind PKey.shortcut.commandView
    |> Option.iter (fun v -> element.CommandView <- v)

    props
    |> Props.tryFind PKey.shortcut.forceFocusColors
    |> Option.iter (fun v -> element.ForceFocusColors <- v)

    props
    |> Props.tryFind PKey.shortcut.helpText
    |> Option.iter (fun v -> element.HelpText <- v)

    props
    |> Props.tryFind PKey.shortcut.text
    |> Option.iter (fun v -> element.Text <- v)

    props
    |> Props.tryFind PKey.shortcut.bindKeyToApplication
    |> Option.iter (fun v -> element.BindKeyToApplication <- v)

    props
    |> Props.tryFind PKey.shortcut.key
    |> Option.iter (fun v -> element.Key <- v)

    props
    |> Props.tryFind PKey.shortcut.minimumKeyTextSize
    |> Option.iter (fun v -> element.MinimumKeyTextSize <- v)

    // Events
    props

    |> Props.tryFind PKey.shortcut.orientationChanged

    |> Option.iter (fun v -> 

      Interop.setEventHandler <@ element.OrientationChanged @> (fun arg -> v arg.Value) element

      this.trackEventHandler("orientationChanged"))

    props


    |> Props.tryFind PKey.shortcut.orientationChanging


    |> Option.iter (fun v -> 


      Interop.setEventHandler <@ element.OrientationChanging @> v element


      this.trackEventHandler("orientationChanging"))

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

      if this.isEventHandlerTracked("accepted") then

        Interop.removeEventHandler <@ element.Accepted @> element

        this.removeTrackedEventHandler("accepted"))

  override _.name = $"MenuItem"

  override this.setProps(element: View, props: Props) =
    base.setProps (element, props)

    let element = element :?> MenuItem

    // Properties
    props
    |> Props.tryFind PKey.menuItem.command
    |> Option.iter (fun v -> element.Command <- v)

    props
    |> Props.tryFind PKey.menuItem.subMenu
    |> Option.iter (fun v -> element.SubMenu <- v)

    props
    |> Props.tryFind PKey.menuItem.targetView
    |> Option.iter (fun v -> element.TargetView <- v)
    // Events
    props

    |> Props.tryFind PKey.menuItem.accepted

    |> Option.iter (fun v -> 

      Interop.setEventHandler <@ element.Accepted @> v element

      this.trackEventHandler("accepted"))

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
    |> Option.iter (fun _ -> element.OpenMode <- Unchecked.defaultof<_>)

  override _.name = $"OpenDialog"

  override this.setProps(element: View, props: Props) =
    base.setProps (element, props)

    let element = element :?> OpenDialog

    // Properties
    props
    |> Props.tryFind PKey.openDialog.openMode
    |> Option.iter (fun v -> element.OpenMode <- v)


  override this.newView() = new OpenDialog()

// TODO: replace direct orientation usage with this type in the Element.fs
type OrientationInterface =
  static member removeProps (element: IOrientation) (props: Props) (isTracked: string -> bool) (removeTracked: string -> unit) =
    // Properties
    props
    |> Props.tryFind PKey.orientationInterface.orientation
    |> Option.iter (fun _ -> element.Orientation <- Unchecked.defaultof<_>)

    // Events
    props
    |> Props.tryFind PKey.orientationInterface.orientationChanged
    |> Option.iter (fun _ -> 
      if isTracked("orientationChanged") then
        Interop.removeEventHandler <@ element.OrientationChanged @> element
        removeTracked("orientationChanged"))

    props
    |> Props.tryFind PKey.orientationInterface.orientationChanging
    |> Option.iter (fun _ -> 
      if isTracked("orientationChanging") then
        Interop.removeEventHandler <@ element.OrientationChanging @> element
        removeTracked("orientationChanging"))

  static member setProps (element: IOrientation) (props: Props) (track: string -> unit) =
    // Properties
    props
    |> Props.tryFind PKey.orientationInterface.orientation
    |> Option.iter (fun v -> element.Orientation <- v)

    // Events
    props
    |> Props.tryFind PKey.orientationInterface.orientationChanged
    |> Option.iter (fun v -> 
      Interop.setEventHandler <@ element.OrientationChanged @> (fun arg -> v arg.Value) element
      track("orientationChanged"))

    props
    |> Props.tryFind PKey.orientationInterface.orientationChanging
    |> Option.iter (fun v -> 
      Interop.setEventHandler <@ element.OrientationChanging @> v element
      track("orientationChanging"))

// SelectorBase
type internal SelectorBaseElement(props: Props) =
  inherit TerminalElement(props)

  override this.removeProps(element: View, props: Props) =
    base.removeProps (element, props)
    let element = element :?> SelectorBase
    // Interfaces
    OrientationInterface.removeProps element props this.isEventHandlerTracked this.removeTrackedEventHandler

    // Properties
    props
    |> Props.tryFind PKey.selectorBase.assignHotKeys
    |> Option.iter (fun _ -> element.AssignHotKeys <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.selectorBase.doubleClickAccepts
    |> Option.iter (fun _ -> element.DoubleClickAccepts <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.selectorBase.horizontalSpace
    |> Option.iter (fun _ -> element.HorizontalSpace <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.selectorBase.labels
    |> Option.iter (fun _ -> element.Labels <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.selectorBase.styles
    |> Option.iter (fun _ -> element.Styles <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.selectorBase.usedHotKeys
    |> Option.iter (fun _ -> element.UsedHotKeys <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.selectorBase.value
    |> Option.iter (fun _ -> element.Value <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.selectorBase.values
    |> Option.iter (fun _ -> element.Values <- Unchecked.defaultof<_>)

    // Events
    props

    |> Props.tryFind PKey.selectorBase.valueChanged

    |> Option.iter (fun _ -> 

      if this.isEventHandlerTracked("valueChanged") then

        Interop.removeEventHandler <@ element.ValueChanged @> element

        this.removeTrackedEventHandler("valueChanged"))

  override _.name = "SelectorBase"

  override this.setProps(element: View, props: Props) =
    base.setProps (element, props)

    let element = element :?> SelectorBase

    // Interfaces
    OrientationInterface.setProps element props this.trackEventHandler

    // Properties
    props
    |> Props.tryFind PKey.selectorBase.assignHotKeys
    |> Option.iter (fun v -> element.AssignHotKeys <- v)

    props
    |> Props.tryFind PKey.selectorBase.doubleClickAccepts
    |> Option.iter (fun v -> element.DoubleClickAccepts <- v)

    props
    |> Props.tryFind PKey.selectorBase.horizontalSpace
    |> Option.iter (fun v -> element.HorizontalSpace <- v)

    props
    |> Props.tryFind PKey.selectorBase.labels
    |> Option.iter (fun v -> element.Labels <- v)

    props
    |> Props.tryFind PKey.selectorBase.styles
    |> Option.iter (fun v -> element.Styles <- v)

    props
    |> Props.tryFind PKey.selectorBase.usedHotKeys
    |> Option.iter (fun v -> element.UsedHotKeys <- v)

    props
    |> Props.tryFind PKey.selectorBase.value
    |> Option.iter (fun v -> element.Value <- v)

    props
    |> Props.tryFind PKey.selectorBase.values
    |> Option.iter (fun v -> element.Values <- v)

    // Events
    props

    |> Props.tryFind PKey.selectorBase.valueChanged

    |> Option.iter (fun v -> 

      Interop.setEventHandler <@ element.ValueChanged @> v element

      this.trackEventHandler("valueChanged"))

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
    |> Option.iter (fun _ -> element.Cursor <- Unchecked.defaultof<_>)

  override _.name = $"OptionSelector"

  override this.setProps(element: View, props: Props) =
    base.setProps (element, props)

    let element = element :?> OptionSelector

    // Properties
    props
    |> Props.tryFind PKey.optionSelector.cursor
    |> Option.iter (fun v -> element.Cursor <- v)

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
    |> Option.iter (fun _ -> element.Value <- Unchecked.defaultof<_>)

  override _.name = $"FlagSelector"

  override this.setProps(element: View, props: Props) =
    base.setProps (element, props)

    let element = element :?> FlagSelector

    // Properties
    props
    |> Props.tryFind PKey.flagSelector.value
    |> Option.iter (fun v -> element.Value <- v)

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
    |> Option.iter (fun _ -> element.BidirectionalMarquee <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.progressBar.fraction
    |> Option.iter (fun _ -> element.Fraction <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.progressBar.progressBarFormat
    |> Option.iter (fun _ -> element.ProgressBarFormat <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.progressBar.progressBarStyle
    |> Option.iter (fun _ -> element.ProgressBarStyle <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.progressBar.segmentCharacter
    |> Option.iter (fun _ -> element.SegmentCharacter <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.progressBar.text
    |> Option.iter (fun _ -> element.Text <- Unchecked.defaultof<_>)

  override _.name = $"ProgressBar"

  override this.setProps(element: View, props: Props) =
    base.setProps (element, props)

    let element = element :?> ProgressBar

    // Properties
    props
    |> Props.tryFind PKey.progressBar.bidirectionalMarquee
    |> Option.iter (fun v -> element.BidirectionalMarquee <- v)

    props
    |> Props.tryFind PKey.progressBar.fraction
    |> Option.iter (fun v -> element.Fraction <- v)

    props
    |> Props.tryFind PKey.progressBar.progressBarFormat
    |> Option.iter (fun v -> element.ProgressBarFormat <- v)

    props
    |> Props.tryFind PKey.progressBar.progressBarStyle
    |> Option.iter (fun v -> element.ProgressBarStyle <- v)

    props
    |> Props.tryFind PKey.progressBar.segmentCharacter
    |> Option.iter (fun v -> element.SegmentCharacter <- v)

    props
    |> Props.tryFind PKey.progressBar.text
    |> Option.iter (fun v -> element.Text <- v)


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
    |> Option.iter (fun _ -> element.AutoShow <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.scrollBar.increment
    |> Option.iter (fun _ -> element.Increment <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.scrollBar.orientation
    |> Option.iter (fun _ -> element.Orientation <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.scrollBar.position
    |> Option.iter (fun _ -> element.Position <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.scrollBar.scrollableContentSize
    |> Option.iter (fun _ -> element.ScrollableContentSize <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.scrollBar.visibleContentSize
    |> Option.iter (fun _ -> element.VisibleContentSize <- Unchecked.defaultof<_>)
    // Events
    props

    |> Props.tryFind PKey.scrollBar.orientationChanged

    |> Option.iter (fun _ -> 

      if this.isEventHandlerTracked("orientationChanged") then

        Interop.removeEventHandler <@ element.OrientationChanged @> element

        this.removeTrackedEventHandler("orientationChanged"))

    props


    |> Props.tryFind PKey.scrollBar.orientationChanging


    |> Option.iter (fun _ -> 


      if this.isEventHandlerTracked("orientationChanging") then


        Interop.removeEventHandler <@ element.OrientationChanging @> element


        this.removeTrackedEventHandler("orientationChanging"))

    props


    |> Props.tryFind PKey.scrollBar.scrollableContentSizeChanged


    |> Option.iter (fun _ -> 


      if this.isEventHandlerTracked("scrollableContentSizeChanged") then


        Interop.removeEventHandler <@ element.ScrollableContentSizeChanged @> element


        this.removeTrackedEventHandler("scrollableContentSizeChanged"))

    props


    |> Props.tryFind PKey.scrollBar.sliderPositionChanged


    |> Option.iter (fun _ -> 


      if this.isEventHandlerTracked("sliderPositionChanged") then


        Interop.removeEventHandler <@ element.SliderPositionChanged @> element


        this.removeTrackedEventHandler("sliderPositionChanged"))

  override _.name = $"ScrollBar"

  override this.setProps(element: View, props: Props) =
    base.setProps (element, props)

    let element = element :?> ScrollBar

    // Properties
    props
    |> Props.tryFind PKey.scrollBar.autoShow
    |> Option.iter (fun v -> element.AutoShow <- v)

    props
    |> Props.tryFind PKey.scrollBar.increment
    |> Option.iter (fun v -> element.Increment <- v)

    props
    |> Props.tryFind PKey.scrollBar.orientation
    |> Option.iter (fun v -> element.Orientation <- v)

    props
    |> Props.tryFind PKey.scrollBar.position
    |> Option.iter (fun v -> element.Position <- v)

    props
    |> Props.tryFind PKey.scrollBar.scrollableContentSize
    |> Option.iter (fun v -> element.ScrollableContentSize <- v)

    props
    |> Props.tryFind PKey.scrollBar.visibleContentSize
    |> Option.iter (fun v -> element.VisibleContentSize <- v)
    // Events
    props

    |> Props.tryFind PKey.scrollBar.orientationChanged

    |> Option.iter (fun v -> 

      Interop.setEventHandler <@ element.OrientationChanged @> (fun arg -> v arg.Value) element

      this.trackEventHandler("orientationChanged"))

    props


    |> Props.tryFind PKey.scrollBar.orientationChanging


    |> Option.iter (fun v -> 


      Interop.setEventHandler <@ element.OrientationChanging @> v element


      this.trackEventHandler("orientationChanging"))

    props


    |> Props.tryFind PKey.scrollBar.scrollableContentSizeChanged


    |> Option.iter (fun v -> 


      Interop.setEventHandler <@ element.ScrollableContentSizeChanged @> v element


      this.trackEventHandler("scrollableContentSizeChanged"))

    props


    |> Props.tryFind PKey.scrollBar.sliderPositionChanged


    |> Option.iter (fun v -> 


      Interop.setEventHandler <@ element.SliderPositionChanged @> v element


      this.trackEventHandler("sliderPositionChanged"))


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
    |> Option.iter (fun _ -> element.Orientation <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.scrollSlider.position
    |> Option.iter (fun _ -> element.Position <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.scrollSlider.size
    |> Option.iter (fun _ -> element.Size <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.scrollSlider.sliderPadding
    |> Option.iter (fun _ -> element.SliderPadding <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.scrollSlider.visibleContentSize
    |> Option.iter (fun _ -> element.VisibleContentSize <- Unchecked.defaultof<_>)
    // Events
    props

    |> Props.tryFind PKey.scrollSlider.orientationChanged

    |> Option.iter (fun _ -> 

      if this.isEventHandlerTracked("orientationChanged") then

        Interop.removeEventHandler <@ element.OrientationChanged @> element

        this.removeTrackedEventHandler("orientationChanged"))

    props


    |> Props.tryFind PKey.scrollSlider.orientationChanging


    |> Option.iter (fun _ -> 


      if this.isEventHandlerTracked("orientationChanging") then


        Interop.removeEventHandler <@ element.OrientationChanging @> element


        this.removeTrackedEventHandler("orientationChanging"))

    props


    |> Props.tryFind PKey.scrollSlider.positionChanged


    |> Option.iter (fun _ -> 


      if this.isEventHandlerTracked("positionChanged") then


        Interop.removeEventHandler <@ element.PositionChanged @> element


        this.removeTrackedEventHandler("positionChanged"))

    props


    |> Props.tryFind PKey.scrollSlider.positionChanging


    |> Option.iter (fun _ -> 


      if this.isEventHandlerTracked("positionChanging") then


        Interop.removeEventHandler <@ element.PositionChanging @> element


        this.removeTrackedEventHandler("positionChanging"))

    props


    |> Props.tryFind PKey.scrollSlider.scrolled


    |> Option.iter (fun _ -> 


      if this.isEventHandlerTracked("scrolled") then


        Interop.removeEventHandler <@ element.Scrolled @> element


        this.removeTrackedEventHandler("scrolled"))

  override _.name = $"ScrollSlider"

  override this.setProps(element: View, props: Props) =
    base.setProps (element, props)

    let element = element :?> ScrollSlider

    // Properties
    props
    |> Props.tryFind PKey.scrollSlider.orientation
    |> Option.iter (fun v -> element.Orientation <- v)

    props
    |> Props.tryFind PKey.scrollSlider.position
    |> Option.iter (fun v -> element.Position <- v)

    props
    |> Props.tryFind PKey.scrollSlider.size
    |> Option.iter (fun v -> element.Size <- v)

    props
    |> Props.tryFind PKey.scrollSlider.sliderPadding
    |> Option.iter (fun v -> element.SliderPadding <- v)

    props
    |> Props.tryFind PKey.scrollSlider.visibleContentSize
    |> Option.iter (fun v -> element.VisibleContentSize <- v)
    // Events
    props

    |> Props.tryFind PKey.scrollSlider.orientationChanged

    |> Option.iter (fun v -> 

      Interop.setEventHandler <@ element.OrientationChanged @> (fun arg -> v arg.Value) element

      this.trackEventHandler("orientationChanged"))

    props


    |> Props.tryFind PKey.scrollSlider.orientationChanging


    |> Option.iter (fun v -> 


      Interop.setEventHandler <@ element.OrientationChanging @> v element


      this.trackEventHandler("orientationChanging"))

    props


    |> Props.tryFind PKey.scrollSlider.positionChanged


    |> Option.iter (fun v -> 


      Interop.setEventHandler <@ element.PositionChanged @> v element


      this.trackEventHandler("positionChanged"))

    props


    |> Props.tryFind PKey.scrollSlider.positionChanging


    |> Option.iter (fun v -> 


      Interop.setEventHandler <@ element.PositionChanging @> v element


      this.trackEventHandler("positionChanging"))

    props


    |> Props.tryFind PKey.scrollSlider.scrolled


    |> Option.iter (fun v -> 


      Interop.setEventHandler <@ element.Scrolled @> v element


      this.trackEventHandler("scrolled"))


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

      if this.isEventHandlerTracked("optionFocused") then

        Interop.removeEventHandler <@ element.OptionFocused @> element

        this.removeTrackedEventHandler("optionFocused"))

    props


    |> Props.tryFind PKey.slider.optionsChanged


    |> Option.iter (fun _ -> 


      if this.isEventHandlerTracked("optionsChanged") then


        Interop.removeEventHandler <@ element.OptionsChanged @> element


        this.removeTrackedEventHandler("optionsChanged"))

    props


    |> Props.tryFind PKey.slider.orientationChanged


    |> Option.iter (fun _ -> 


      if this.isEventHandlerTracked("orientationChanged") then


        Interop.removeEventHandler <@ element.OrientationChanged @> element


        this.removeTrackedEventHandler("orientationChanged"))

    props


    |> Props.tryFind PKey.slider.orientationChanging


    |> Option.iter (fun _ -> 


      if this.isEventHandlerTracked("orientationChanging") then


        Interop.removeEventHandler <@ element.OrientationChanging @> element


        this.removeTrackedEventHandler("orientationChanging"))

  override _.name = $"Slider<'a>"

  override this.setProps(element: View, props: Props) =
    base.setProps (element, props)

    let element = element :?> Slider<'a>

    // Properties
    props
    |> Props.tryFind PKey.slider.allowEmpty
    |> Option.iter (fun v -> element.AllowEmpty <- v)

    props
    |> Props.tryFind PKey.slider.focusedOption
    |> Option.iter (fun v -> element.FocusedOption <- v)

    props
    |> Props.tryFind PKey.slider.legendsOrientation
    |> Option.iter (fun v -> element.LegendsOrientation <- v)

    props
    |> Props.tryFind PKey.slider.minimumInnerSpacing
    |> Option.iter (fun v -> element.MinimumInnerSpacing <- v)

    props
    |> Props.tryFind PKey.slider.options
    |> Option.iter (fun v -> element.Options <- List<_>(v))

    props
    |> Props.tryFind PKey.slider.orientation
    |> Option.iter (fun v -> element.Orientation <- v)

    props
    |> Props.tryFind PKey.slider.rangeAllowSingle
    |> Option.iter (fun v -> element.RangeAllowSingle <- v)

    props
    |> Props.tryFind PKey.slider.showEndSpacing
    |> Option.iter (fun v -> element.ShowEndSpacing <- v)

    props
    |> Props.tryFind PKey.slider.showLegends
    |> Option.iter (fun v -> element.ShowLegends <- v)

    props
    |> Props.tryFind PKey.slider.style
    |> Option.iter (fun v -> element.Style <- v)

    props
    |> Props.tryFind PKey.slider.text
    |> Option.iter (fun v -> element.Text <- v)

    props
    |> Props.tryFind PKey.slider.``type``
    |> Option.iter (fun v -> element.Type <- v)

    props
    |> Props.tryFind PKey.slider.useMinimumSize
    |> Option.iter (fun v -> element.UseMinimumSize <- v)
    // Events
    props

    |> Props.tryFind PKey.slider.optionFocused

    |> Option.iter (fun v -> 

      Interop.setEventHandler <@ element.OptionFocused @> v element

      this.trackEventHandler("optionFocused"))

    props


    |> Props.tryFind PKey.slider.optionsChanged


    |> Option.iter (fun v -> 


      Interop.setEventHandler <@ element.OptionsChanged @> v element


      this.trackEventHandler("optionsChanged"))

    props


    |> Props.tryFind PKey.slider.orientationChanged


    |> Option.iter (fun v -> 


      Interop.setEventHandler <@ element.OrientationChanged @> (fun arg -> v arg.Value) element


      this.trackEventHandler("orientationChanged"))

    props


    |> Props.tryFind PKey.slider.orientationChanging


    |> Option.iter (fun v -> 


      Interop.setEventHandler <@ element.OrientationChanging @> v element


      this.trackEventHandler("orientationChanging"))


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
    |> Option.iter (fun _ -> element.AutoSpin <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.spinnerView.sequence
    |> Option.iter (fun _ -> element.Sequence <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.spinnerView.spinBounce
    |> Option.iter (fun _ -> element.SpinBounce <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.spinnerView.spinDelay
    |> Option.iter (fun _ -> element.SpinDelay <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.spinnerView.spinReverse
    |> Option.iter (fun _ -> element.SpinReverse <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.spinnerView.style
    |> Option.iter (fun _ -> element.Style <- Unchecked.defaultof<_>)

  override _.name = $"SpinnerView"

  override this.setProps(element: View, props: Props) =
    base.setProps (element, props)

    let element = element :?> SpinnerView

    // Properties
    props
    |> Props.tryFind PKey.spinnerView.autoSpin
    |> Option.iter (fun v -> element.AutoSpin <- v)

    props
    |> Props.tryFind PKey.spinnerView.sequence
    |> Option.iter (fun v -> element.Sequence <- v |> List.toArray)

    props
    |> Props.tryFind PKey.spinnerView.spinBounce
    |> Option.iter (fun v -> element.SpinBounce <- v)

    props
    |> Props.tryFind PKey.spinnerView.spinDelay
    |> Option.iter (fun v -> element.SpinDelay <- v)

    props
    |> Props.tryFind PKey.spinnerView.spinReverse
    |> Option.iter (fun v -> element.SpinReverse <- v)

    props
    |> Props.tryFind PKey.spinnerView.style
    |> Option.iter (fun v -> element.Style <- v)


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
    |> Option.iter (fun _ -> element.DisplayText <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.tab.view
    |> Option.iter (fun _ -> element.View <- Unchecked.defaultof<_>)

  override _.name = $"Tab"

  override this.setProps(element: View, props: Props) =
    base.setProps (element, props)

    let element = element :?> Tab

    // Properties
    props
    |> Props.tryFind PKey.tab.displayText
    |> Option.iter (fun v -> element.DisplayText <- v)

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
    |> Option.iter (fun _ -> element.MaxTabTextWidth <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.tabView.selectedTab
    |> Option.iter (fun _ -> element.SelectedTab <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.tabView.style
    |> Option.iter (fun _ -> element.Style <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.tabView.tabScrollOffset
    |> Option.iter (fun _ -> element.TabScrollOffset <- Unchecked.defaultof<_>)
    // Events
    props

    |> Props.tryFind PKey.tabView.selectedTabChanged

    |> Option.iter (fun _ -> 

      if this.isEventHandlerTracked("selectedTabChanged") then

        Interop.removeEventHandler <@ element.SelectedTabChanged @> element

        this.removeTrackedEventHandler("selectedTabChanged"))

    props


    |> Props.tryFind PKey.tabView.tabClicked


    |> Option.iter (fun _ -> 


      if this.isEventHandlerTracked("tabClicked") then


        Interop.removeEventHandler <@ element.TabClicked @> element


        this.removeTrackedEventHandler("tabClicked"))

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
    |> Option.iter (fun v -> element.SelectedTab <- v)

    props
    |> Props.tryFind PKey.tabView.style
    |> Option.iter (fun v -> element.Style <- v)

    props
    |> Props.tryFind PKey.tabView.tabScrollOffset
    |> Option.iter (fun v -> element.TabScrollOffset <- v)
    // Events
    props

    |> Props.tryFind PKey.tabView.selectedTabChanged

    |> Option.iter (fun v -> 

      Interop.setEventHandler <@ element.SelectedTabChanged @> v element

      this.trackEventHandler("selectedTabChanged"))

    props


    |> Props.tryFind PKey.tabView.tabClicked


    |> Option.iter (fun v -> 


      Interop.setEventHandler <@ element.TabClicked @> v element


      this.trackEventHandler("tabClicked"))

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
    |> Option.iter (fun _ -> element.CellActivationKey <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.tableView.collectionNavigator
    |> Option.iter (fun _ -> element.CollectionNavigator <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.tableView.columnOffset
    |> Option.iter (fun _ -> element.ColumnOffset <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.tableView.fullRowSelect
    |> Option.iter (fun _ -> element.FullRowSelect <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.tableView.maxCellWidth
    |> Option.iter (fun _ -> element.MaxCellWidth <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.tableView.minCellWidth
    |> Option.iter (fun _ -> element.MinCellWidth <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.tableView.multiSelect
    |> Option.iter (fun _ -> element.MultiSelect <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.tableView.nullSymbol
    |> Option.iter (fun _ -> element.NullSymbol <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.tableView.rowOffset
    |> Option.iter (fun _ -> element.RowOffset <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.tableView.selectedColumn
    |> Option.iter (fun _ -> element.SelectedColumn <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.tableView.selectedRow
    |> Option.iter (fun _ -> element.SelectedRow <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.tableView.separatorSymbol
    |> Option.iter (fun _ -> element.SeparatorSymbol <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.tableView.style
    |> Option.iter (fun _ -> element.Style <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.tableView.table
    |> Option.iter (fun _ -> element.Table <- Unchecked.defaultof<_>)
    // Events
    props

    |> Props.tryFind PKey.tableView.cellActivated

    |> Option.iter (fun _ -> 

      if this.isEventHandlerTracked("cellActivated") then

        Interop.removeEventHandler <@ element.CellActivated @> element

        this.removeTrackedEventHandler("cellActivated"))

    props


    |> Props.tryFind PKey.tableView.cellToggled


    |> Option.iter (fun _ -> 


      if this.isEventHandlerTracked("cellToggled") then


        Interop.removeEventHandler <@ element.CellToggled @> element


        this.removeTrackedEventHandler("cellToggled"))

    props


    |> Props.tryFind PKey.tableView.selectedCellChanged


    |> Option.iter (fun _ -> 


      if this.isEventHandlerTracked("selectedCellChanged") then


        Interop.removeEventHandler <@ element.SelectedCellChanged @> element


        this.removeTrackedEventHandler("selectedCellChanged"))

  override _.name = $"TableView"

  override this.setProps(element: View, props: Props) =
    base.setProps (element, props)

    let element = element :?> TableView

    // Properties
    props
    |> Props.tryFind PKey.tableView.cellActivationKey
    |> Option.iter (fun v -> element.CellActivationKey <- v)

    props
    |> Props.tryFind PKey.tableView.collectionNavigator
    |> Option.iter (fun v -> element.CollectionNavigator <- v)

    props
    |> Props.tryFind PKey.tableView.columnOffset
    |> Option.iter (fun v -> element.ColumnOffset <- v)

    props
    |> Props.tryFind PKey.tableView.fullRowSelect
    |> Option.iter (fun v -> element.FullRowSelect <- v)

    props
    |> Props.tryFind PKey.tableView.maxCellWidth
    |> Option.iter (fun v -> element.MaxCellWidth <- v)

    props
    |> Props.tryFind PKey.tableView.minCellWidth
    |> Option.iter (fun v -> element.MinCellWidth <- v)

    props
    |> Props.tryFind PKey.tableView.multiSelect
    |> Option.iter (fun v -> element.MultiSelect <- v)

    props
    |> Props.tryFind PKey.tableView.nullSymbol
    |> Option.iter (fun v -> element.NullSymbol <- v)

    props
    |> Props.tryFind PKey.tableView.rowOffset
    |> Option.iter (fun v -> element.RowOffset <- v)

    props
    |> Props.tryFind PKey.tableView.selectedColumn
    |> Option.iter (fun v -> element.SelectedColumn <- v)

    props
    |> Props.tryFind PKey.tableView.selectedRow
    |> Option.iter (fun v -> element.SelectedRow <- v)

    props
    |> Props.tryFind PKey.tableView.separatorSymbol
    |> Option.iter (fun v -> element.SeparatorSymbol <- v)

    props
    |> Props.tryFind PKey.tableView.style
    |> Option.iter (fun v -> element.Style <- v)

    props
    |> Props.tryFind PKey.tableView.table
    |> Option.iter (fun v -> element.Table <- v)
    // Events
    props

    |> Props.tryFind PKey.tableView.cellActivated

    |> Option.iter (fun v -> 

      Interop.setEventHandler <@ element.CellActivated @> v element

      this.trackEventHandler("cellActivated"))

    props


    |> Props.tryFind PKey.tableView.cellToggled


    |> Option.iter (fun v -> 


      Interop.setEventHandler <@ element.CellToggled @> v element


      this.trackEventHandler("cellToggled"))

    props


    |> Props.tryFind PKey.tableView.selectedCellChanged


    |> Option.iter (fun v -> 


      Interop.setEventHandler <@ element.SelectedCellChanged @> v element


      this.trackEventHandler("selectedCellChanged"))


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
    |> Option.iter (fun _ -> element.Autocomplete <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.textField.cursorPosition
    |> Option.iter (fun _ -> element.CursorPosition <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.textField.readOnly
    |> Option.iter (fun _ -> element.ReadOnly <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.textField.secret
    |> Option.iter (fun _ -> element.Secret <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.textField.selectedStart
    |> Option.iter (fun _ -> element.SelectedStart <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.textField.selectWordOnlyOnDoubleClick
    |> Option.iter (fun _ -> element.SelectWordOnlyOnDoubleClick <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.textField.text
    |> Option.iter (fun _ -> element.Text <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.textField.used
    |> Option.iter (fun _ -> element.Used <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.textField.useSameRuneTypeForWords
    |> Option.iter (fun _ -> element.UseSameRuneTypeForWords <- Unchecked.defaultof<_>)
    // Events
    props

    |> Props.tryFind PKey.textField.textChanging

    |> Option.iter (fun _ -> 

      if this.isEventHandlerTracked("textChanging") then

        Interop.removeEventHandler <@ element.TextChanging @> element

        this.removeTrackedEventHandler("textChanging"))

  override _.name = $"TextField"

  override this.setProps(element: View, props: Props) =
    base.setProps (element, props)

    let element = element :?> TextField

    // Properties
    props
    |> Props.tryFind PKey.textField.autocomplete
    |> Option.iter (fun v -> element.Autocomplete <- v)

    props
    |> Props.tryFind PKey.textField.cursorPosition
    |> Option.iter (fun v -> element.CursorPosition <- v)

    props
    |> Props.tryFind PKey.textField.readOnly
    |> Option.iter (fun v -> element.ReadOnly <- v)

    props
    |> Props.tryFind PKey.textField.secret
    |> Option.iter (fun v -> element.Secret <- v)

    props
    |> Props.tryFind PKey.textField.selectedStart
    |> Option.iter (fun v -> element.SelectedStart <- v)

    props
    |> Props.tryFind PKey.textField.selectWordOnlyOnDoubleClick
    |> Option.iter (fun v -> element.SelectWordOnlyOnDoubleClick <- v)

    props
    |> Props.tryFind PKey.textField.text
    |> Option.iter (fun v -> element.Text <- v)

    props
    |> Props.tryFind PKey.textField.used
    |> Option.iter (fun v -> element.Used <- v)

    props
    |> Props.tryFind PKey.textField.useSameRuneTypeForWords
    |> Option.iter (fun v -> element.UseSameRuneTypeForWords <- v)
    // Events
    props

    |> Props.tryFind PKey.textField.textChanging

    |> Option.iter (fun v -> 

      Interop.setEventHandler <@ element.TextChanging @> v element

      this.trackEventHandler("textChanging"))


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
    |> Option.iter (fun _ -> element.Provider <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.textValidateField.text
    |> Option.iter (fun _ -> element.Text <- Unchecked.defaultof<_>)

  override _.name = $"TextValidateField"

  override this.setProps(element: View, props: Props) =
    base.setProps (element, props)

    let element = element :?> TextValidateField

    // Properties
    props
    |> Props.tryFind PKey.textValidateField.provider
    |> Option.iter (fun v -> element.Provider <- v)

    props
    |> Props.tryFind PKey.textValidateField.text
    |> Option.iter (fun v -> element.Text <- v)


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
    |> Option.iter (fun _ -> element.AllowsReturn <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.textView.allowsTab
    |> Option.iter (fun _ -> element.AllowsTab <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.textView.cursorPosition
    |> Option.iter (fun _ -> element.CursorPosition <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.textView.inheritsPreviousAttribute
    |> Option.iter (fun _ -> element.InheritsPreviousAttribute <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.textView.isDirty
    |> Option.iter (fun _ -> element.IsDirty <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.textView.isSelecting
    |> Option.iter (fun _ -> element.IsSelecting <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.textView.leftColumn
    |> Option.iter (fun _ -> element.LeftColumn <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.textView.multiline
    |> Option.iter (fun _ -> element.Multiline <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.textView.readOnly
    |> Option.iter (fun _ -> element.ReadOnly <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.textView.selectionStartColumn
    |> Option.iter (fun _ -> element.SelectionStartColumn <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.textView.selectionStartRow
    |> Option.iter (fun _ -> element.SelectionStartRow <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.textView.selectWordOnlyOnDoubleClick
    |> Option.iter (fun _ -> element.SelectWordOnlyOnDoubleClick <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.textView.tabWidth
    |> Option.iter (fun _ -> element.TabWidth <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.textView.text
    |> Option.iter (fun _ -> element.Text <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.textView.topRow
    |> Option.iter (fun _ -> element.TopRow <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.textView.used
    |> Option.iter (fun _ -> element.Used <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.textView.useSameRuneTypeForWords
    |> Option.iter (fun _ -> element.UseSameRuneTypeForWords <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.textView.wordWrap
    |> Option.iter (fun _ -> element.WordWrap <- Unchecked.defaultof<_>)
    // Events
    props

    |> Props.tryFind PKey.textView.contentsChanged

    |> Option.iter (fun _ -> 

      if this.isEventHandlerTracked("contentsChanged") then

        Interop.removeEventHandler <@ element.ContentsChanged @> element

        this.removeTrackedEventHandler("contentsChanged"))

    props


    |> Props.tryFind PKey.textView.drawNormalColor


    |> Option.iter (fun _ -> 


      if this.isEventHandlerTracked("drawNormalColor") then


        Interop.removeEventHandler <@ element.DrawNormalColor @> element


        this.removeTrackedEventHandler("drawNormalColor"))

    props


    |> Props.tryFind PKey.textView.drawReadOnlyColor


    |> Option.iter (fun _ -> 


      if this.isEventHandlerTracked("drawReadOnlyColor") then


        Interop.removeEventHandler <@ element.DrawReadOnlyColor @> element


        this.removeTrackedEventHandler("drawReadOnlyColor"))

    props


    |> Props.tryFind PKey.textView.drawSelectionColor


    |> Option.iter (fun _ -> 


      if this.isEventHandlerTracked("drawSelectionColor") then


        Interop.removeEventHandler <@ element.DrawSelectionColor @> element


        this.removeTrackedEventHandler("drawSelectionColor"))

    props


    |> Props.tryFind PKey.textView.drawUsedColor


    |> Option.iter (fun _ -> 


      if this.isEventHandlerTracked("drawUsedColor") then


        Interop.removeEventHandler <@ element.DrawUsedColor @> element


        this.removeTrackedEventHandler("drawUsedColor"))

    props


    |> Props.tryFind PKey.textView.unwrappedCursorPosition


    |> Option.iter (fun _ -> 


      if this.isEventHandlerTracked("unwrappedCursorPosition") then


        Interop.removeEventHandler <@ element.UnwrappedCursorPosition @> element


        this.removeTrackedEventHandler("unwrappedCursorPosition"))

    // Additional properties
    props

    |> Props.tryFind PKey.textView.textChanged

    |> Option.iter (fun _ -> 

      if this.isEventHandlerTracked("textChanged") then

        Interop.removeEventHandler <@ element.ContentsChanged @> element

        this.removeTrackedEventHandler("textChanged"))


  override _.name = $"TextView"

  override this.setProps(element: View, props: Props) =
    base.setProps (element, props)

    let element = element :?> TextView

    // Properties
    props
    |> Props.tryFind PKey.textView.allowsReturn
    |> Option.iter (fun v -> element.AllowsReturn <- v)

    props
    |> Props.tryFind PKey.textView.allowsTab
    |> Option.iter (fun v -> element.AllowsTab <- v)

    props
    |> Props.tryFind PKey.textView.cursorPosition
    |> Option.iter (fun v -> element.CursorPosition <- v)

    props
    |> Props.tryFind PKey.textView.inheritsPreviousAttribute
    |> Option.iter (fun v -> element.InheritsPreviousAttribute <- v)

    props
    |> Props.tryFind PKey.textView.isDirty
    |> Option.iter (fun v -> element.IsDirty <- v)

    props
    |> Props.tryFind PKey.textView.isSelecting
    |> Option.iter (fun v -> element.IsSelecting <- v)

    props
    |> Props.tryFind PKey.textView.leftColumn
    |> Option.iter (fun v -> element.LeftColumn <- v)

    props
    |> Props.tryFind PKey.textView.multiline
    |> Option.iter (fun v -> element.Multiline <- v)

    props
    |> Props.tryFind PKey.textView.readOnly
    |> Option.iter (fun v -> element.ReadOnly <- v)

    props
    |> Props.tryFind PKey.textView.selectionStartColumn
    |> Option.iter (fun v -> element.SelectionStartColumn <- v)

    props
    |> Props.tryFind PKey.textView.selectionStartRow
    |> Option.iter (fun v -> element.SelectionStartRow <- v)

    props
    |> Props.tryFind PKey.textView.selectWordOnlyOnDoubleClick
    |> Option.iter (fun v -> element.SelectWordOnlyOnDoubleClick <- v)

    props
    |> Props.tryFind PKey.textView.tabWidth
    |> Option.iter (fun v -> element.TabWidth <- v)

    props
    |> Props.tryFind PKey.textView.text
    |> Option.iter (fun v -> element.Text <- v)

    props
    |> Props.tryFind PKey.textView.topRow
    |> Option.iter (fun v -> element.TopRow <- v)

    props
    |> Props.tryFind PKey.textView.used
    |> Option.iter (fun v -> element.Used <- v)

    props
    |> Props.tryFind PKey.textView.useSameRuneTypeForWords
    |> Option.iter (fun v -> element.UseSameRuneTypeForWords <- v)

    props
    |> Props.tryFind PKey.textView.wordWrap
    |> Option.iter (fun v -> element.WordWrap <- v)
    // Events
    props

    |> Props.tryFind PKey.textView.contentsChanged

    |> Option.iter (fun v -> 

      Interop.setEventHandler <@ element.ContentsChanged @> v element

      this.trackEventHandler("contentsChanged"))

    props


    |> Props.tryFind PKey.textView.drawNormalColor


    |> Option.iter (fun v -> 


      Interop.setEventHandler <@ element.DrawNormalColor @> v element


      this.trackEventHandler("drawNormalColor"))

    props


    |> Props.tryFind PKey.textView.drawReadOnlyColor


    |> Option.iter (fun v -> 


      Interop.setEventHandler <@ element.DrawReadOnlyColor @> v element


      this.trackEventHandler("drawReadOnlyColor"))

    props


    |> Props.tryFind PKey.textView.drawSelectionColor


    |> Option.iter (fun v -> 


      Interop.setEventHandler <@ element.DrawSelectionColor @> v element


      this.trackEventHandler("drawSelectionColor"))

    props


    |> Props.tryFind PKey.textView.drawUsedColor


    |> Option.iter (fun v -> 


      Interop.setEventHandler <@ element.DrawUsedColor @> v element


      this.trackEventHandler("drawUsedColor"))

    props


    |> Props.tryFind PKey.textView.unwrappedCursorPosition


    |> Option.iter (fun v -> 


      Interop.setEventHandler <@ element.UnwrappedCursorPosition @> v element


      this.trackEventHandler("unwrappedCursorPosition"))

    // Additional properties
    props

    |> Props.tryFind PKey.textView.textChanged

    |> Option.iter (fun v -> 

      Interop.setEventHandler <@ element.ContentsChanged @> (fun _ -> v element.Text) element

      this.trackEventHandler("textChanged"))


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
    |> Option.iter (fun _ -> element.CursorPosition <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.timeField.isShortFormat
    |> Option.iter (fun _ -> element.IsShortFormat <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.timeField.time
    |> Option.iter (fun _ -> element.Time <- Unchecked.defaultof<_>)
    // Events
    props

    |> Props.tryFind PKey.timeField.timeChanged

    |> Option.iter (fun _ -> 

      if this.isEventHandlerTracked("timeChanged") then

        Interop.removeEventHandler <@ element.TimeChanged @> element

        this.removeTrackedEventHandler("timeChanged"))

  override _.name = $"TimeField"

  override this.setProps(element: View, props: Props) =
    base.setProps (element, props)

    let element = element :?> TimeField

    // Properties
    props
    |> Props.tryFind PKey.timeField.cursorPosition
    |> Option.iter (fun v -> element.CursorPosition <- v)

    props
    |> Props.tryFind PKey.timeField.isShortFormat
    |> Option.iter (fun v -> element.IsShortFormat <- v)

    props
    |> Props.tryFind PKey.timeField.time
    |> Option.iter (fun v -> element.Time <- v)
    // Events
    props

    |> Props.tryFind PKey.timeField.timeChanged

    |> Option.iter (fun v -> 

      Interop.setEventHandler <@ element.TimeChanged @> v element

      this.trackEventHandler("timeChanged"))


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
    |> Option.iter (fun _ -> element.StopRequested <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.runnable.result
    |> Option.iter (fun _ -> element.Result <- Unchecked.defaultof<_>)

    // Events
    props

    |> Props.tryFind PKey.runnable.isRunningChanging

    |> Option.iter (fun _ -> 

      if this.isEventHandlerTracked("isRunningChanging") then

        Interop.removeEventHandler <@ element.IsRunningChanging @> element

        this.removeTrackedEventHandler("isRunningChanging"))

    props


    |> Props.tryFind PKey.runnable.isRunningChanged


    |> Option.iter (fun _ -> 


      if this.isEventHandlerTracked("isRunningChanged") then


        Interop.removeEventHandler <@ element.IsRunningChanged @> element


        this.removeTrackedEventHandler("isRunningChanged"))

    props


    |> Props.tryFind PKey.runnable.isModalChanged


    |> Option.iter (fun _ -> 


      if this.isEventHandlerTracked("isModalChanged") then


        Interop.removeEventHandler <@ element.IsModalChanged @> element


        this.removeTrackedEventHandler("isModalChanged"))

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
    |> Option.iter (fun v -> element.StopRequested <- v)

    props
    |> Props.tryFind PKey.runnable.result
    |> Option.iter (fun v -> element.Result <- v)

    // Events
    props

    |> Props.tryFind PKey.runnable.isRunningChanging

    |> Option.iter (fun v -> 

      Interop.setEventHandler <@ element.IsRunningChanging @> v element

      this.trackEventHandler("isRunningChanging"))

    props


    |> Props.tryFind PKey.runnable.isRunningChanged


    |> Option.iter (fun v -> 


      Interop.setEventHandler <@ element.IsRunningChanged @> v element


      this.trackEventHandler("isRunningChanged"))

    props


    |> Props.tryFind PKey.runnable.isModalChanged


    |> Option.iter (fun v -> 


      Interop.setEventHandler <@ element.IsModalChanged @> v element


      this.trackEventHandler("isModalChanged"))


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
    |> Option.iter (fun _ -> element.CurrentStep <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.wizard.modal
    |> Option.iter (fun _ -> element.SetIsModal(Unchecked.defaultof<_>))
    // Events
    props

    |> Props.tryFind PKey.wizard.cancelled

    |> Option.iter (fun _ -> 

      if this.isEventHandlerTracked("cancelled") then

        Interop.removeEventHandler <@ element.Cancelled @> element

        this.removeTrackedEventHandler("cancelled"))

    props


    |> Props.tryFind PKey.wizard.finished


    |> Option.iter (fun _ -> 


      if this.isEventHandlerTracked("finished") then


        Interop.removeEventHandler <@ element.Finished @> element


        this.removeTrackedEventHandler("finished"))

    props


    |> Props.tryFind PKey.wizard.movingBack


    |> Option.iter (fun _ -> 


      if this.isEventHandlerTracked("movingBack") then


        Interop.removeEventHandler <@ element.MovingBack @> element


        this.removeTrackedEventHandler("movingBack"))

    props


    |> Props.tryFind PKey.wizard.movingNext


    |> Option.iter (fun _ -> 


      if this.isEventHandlerTracked("movingNext") then


        Interop.removeEventHandler <@ element.MovingNext @> element


        this.removeTrackedEventHandler("movingNext"))

    props


    |> Props.tryFind PKey.wizard.stepChanged


    |> Option.iter (fun _ -> 


      if this.isEventHandlerTracked("stepChanged") then


        Interop.removeEventHandler <@ element.StepChanged @> element


        this.removeTrackedEventHandler("stepChanged"))

    props


    |> Props.tryFind PKey.wizard.stepChanging


    |> Option.iter (fun _ -> 


      if this.isEventHandlerTracked("stepChanging") then


        Interop.removeEventHandler <@ element.StepChanging @> element


        this.removeTrackedEventHandler("stepChanging"))

  override _.name = $"Wizard"

  override this.setProps(element: View, props: Props) =
    base.setProps (element, props)

    let element = element :?> Wizard

    // Properties
    props
    |> Props.tryFind PKey.wizard.currentStep
    |> Option.iter (fun v -> element.CurrentStep <- v)

    props
    |> Props.tryFind PKey.wizard.modal
    |> Option.iter (fun v -> element.SetIsModal(v))
    // Events
    props

    |> Props.tryFind PKey.wizard.cancelled

    |> Option.iter (fun v -> 

      Interop.setEventHandler <@ element.Cancelled @> v element

      this.trackEventHandler("cancelled"))

    props


    |> Props.tryFind PKey.wizard.finished


    |> Option.iter (fun v -> 


      Interop.setEventHandler <@ element.Finished @> v element


      this.trackEventHandler("finished"))

    props


    |> Props.tryFind PKey.wizard.movingBack


    |> Option.iter (fun v -> 


      Interop.setEventHandler <@ element.MovingBack @> v element


      this.trackEventHandler("movingBack"))

    props


    |> Props.tryFind PKey.wizard.movingNext


    |> Option.iter (fun v -> 


      Interop.setEventHandler <@ element.MovingNext @> v element


      this.trackEventHandler("movingNext"))

    props


    |> Props.tryFind PKey.wizard.stepChanged


    |> Option.iter (fun v -> 


      Interop.setEventHandler <@ element.StepChanged @> v element


      this.trackEventHandler("stepChanged"))

    props


    |> Props.tryFind PKey.wizard.stepChanging


    |> Option.iter (fun v -> 


      Interop.setEventHandler <@ element.StepChanging @> v element


      this.trackEventHandler("stepChanging"))


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
    |> Option.iter (fun _ -> element.BackButtonText <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.wizardStep.helpText
    |> Option.iter (fun _ -> element.HelpText <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.wizardStep.nextButtonText
    |> Option.iter (fun _ -> element.NextButtonText <- Unchecked.defaultof<_>)

  override _.name = $"WizardStep"

  override this.setProps(element: View, props: Props) =
    base.setProps (element, props)

    let element = element :?> WizardStep

    // Properties
    props
    |> Props.tryFind PKey.wizardStep.backButtonText
    |> Option.iter (fun v -> element.BackButtonText <- v)

    props
    |> Props.tryFind PKey.wizardStep.helpText
    |> Option.iter (fun v -> element.HelpText <- v)

    props
    |> Props.tryFind PKey.wizardStep.nextButtonText
    |> Option.iter (fun v -> element.NextButtonText <- v)


  override this.newView() = new WizardStep()
