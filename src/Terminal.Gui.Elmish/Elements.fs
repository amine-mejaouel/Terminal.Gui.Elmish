module internal Terminal.Gui.Elmish.Elements

open System
open System.Collections.Generic
open System.Collections.ObjectModel
open Terminal.Gui.Elmish
open Terminal.Gui.ViewBase
open Terminal.Gui.Views

type internal TreeNode = {
  TerminalElement: IInternalTerminalElement
  Parent: View option
}

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

  member this.props = props
  member val parent: View option = None with get, set
  member val view: View = null with get, set

  member _.children: List<IInternalTerminalElement> =
    props
    |> Props.tryFindWithDefault PKey.view.children (List<_>())

  abstract subElements: ElementPropKey<IInternalTerminalElement> list
  default _.subElements = []

  abstract newView: unit -> View

  abstract setAsChildOfParentView: bool
  default _.setAsChildOfParentView = true

  member this.initialize(parent) =
#if DEBUG
    Diagnostics.Trace.WriteLine $"{this.name} created!"
#endif
    this.parent <- parent

    let newView = this.newView ()

    this.initializeSubElements (newView)
    |> Seq.iter props.addNonTyped

    // Here, the "children" view are added to their parent
    if this.setAsChildOfParentView then
      parent
      |> Option.iter (fun p -> p.Add newView |> ignore)

    this.setProps (newView, props)
    this.view <- newView

  abstract canUpdate: prevElement: View -> oldProps: Props -> bool
  abstract update: prevElement: View -> oldProps: Props -> unit

  abstract layout: unit -> unit

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
      for x in this.subElements do
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
    |> Option.iter (fun v -> Interop.setEventHandler <@ element.Accepting @> v element)

    props
    |> Props.tryFind PKey.view.advancingFocus
    |> Option.iter (fun v -> Interop.setEventHandler <@ element.AdvancingFocus @> v element)

    props
    |> Props.tryFind PKey.view.borderStyleChanged
    |> Option.iter (fun v -> Interop.setEventHandler <@ element.BorderStyleChanged @> v element)

    props
    |> Props.tryFind PKey.view.canFocusChanged
    |> Option.iter (fun v -> Interop.setEventHandler <@ element.CanFocusChanged @> (fun _ -> v ()) element)

    props
    |> Props.tryFind PKey.view.clearedViewport
    |> Option.iter (fun v -> Interop.setEventHandler <@ element.ClearedViewport @> v element)

    props
    |> Props.tryFind PKey.view.clearingViewport
    |> Option.iter (fun v -> Interop.setEventHandler <@ element.ClearingViewport @> v element)

    props
    |> Props.tryFind PKey.view.commandNotBound
    |> Option.iter (fun v -> Interop.setEventHandler <@ element.CommandNotBound @> v element)

    props
    |> Props.tryFind PKey.view.contentSizeChanged
    |> Option.iter (fun v -> Interop.setEventHandler <@ element.ContentSizeChanged @> v element)

    props
    |> Props.tryFind PKey.view.disposing
    |> Option.iter (fun v -> Interop.setEventHandler <@ element.Disposing @> (fun _ -> v ()) element)

    props
    |> Props.tryFind PKey.view.drawComplete
    |> Option.iter (fun v -> Interop.setEventHandler <@ element.DrawComplete @> v element)

    props
    |> Props.tryFind PKey.view.drawingContent
    |> Option.iter (fun v -> Interop.setEventHandler <@ element.DrawingContent @> v element)

    props
    |> Props.tryFind PKey.view.drawingSubViews
    |> Option.iter (fun v -> Interop.setEventHandler <@ element.DrawingSubViews @> v element)

    props
    |> Props.tryFind PKey.view.drawingText
    |> Option.iter (fun v -> Interop.setEventHandler <@ element.DrawingText @> v element)

    props
    |> Props.tryFind PKey.view.enabledChanged
    |> Option.iter (fun v -> Interop.setEventHandler <@ element.EnabledChanged @> (fun _ -> v ()) element)

    props
    |> Props.tryFind PKey.view.focusedChanged
    |> Option.iter (fun v -> Interop.setEventHandler <@ element.FocusedChanged @> v element)

    props
    |> Props.tryFind PKey.view.frameChanged
    |> Option.iter (fun v -> Interop.setEventHandler <@ element.FrameChanged @> v element)

    props
    |> Props.tryFind PKey.view.gettingAttributeForRole
    |> Option.iter (fun v -> Interop.setEventHandler <@ element.GettingAttributeForRole @> v element)

    props
    |> Props.tryFind PKey.view.gettingScheme
    |> Option.iter (fun v -> Interop.setEventHandler <@ element.GettingScheme @> v element)

    props
    |> Props.tryFind PKey.view.handlingHotKey
    |> Option.iter (fun v -> Interop.setEventHandler <@ element.HandlingHotKey @> v element)

    props
    |> Props.tryFind PKey.view.hasFocusChanged
    |> Option.iter (fun v -> Interop.setEventHandler <@ element.HasFocusChanged @> v element)

    props
    |> Props.tryFind PKey.view.hasFocusChanging
    |> Option.iter (fun v -> Interop.setEventHandler <@ element.HasFocusChanging @> v element)

    props
    |> Props.tryFind PKey.view.hotKeyChanged
    |> Option.iter (fun v -> Interop.setEventHandler <@ element.HotKeyChanged @> v element)

    props
    |> Props.tryFind PKey.view.initialized
    |> Option.iter (fun v -> Interop.setEventHandler <@ element.Initialized @> (fun _ -> v ()) element)

    props
    |> Props.tryFind PKey.view.keyDown
    |> Option.iter (fun v -> Interop.setEventHandler <@ element.KeyDown @> v element)

    props
    |> Props.tryFind PKey.view.keyDownNotHandled
    |> Option.iter (fun v -> Interop.setEventHandler <@ element.KeyDownNotHandled @> v element)

    props
    |> Props.tryFind PKey.view.keyUp
    |> Option.iter (fun v -> Interop.setEventHandler <@ element.KeyUp @> v element)

    props
    |> Props.tryFind PKey.view.mouseClick
    |> Option.iter (fun v -> Interop.setEventHandler <@ element.MouseClick @> v element)

    props
    |> Props.tryFind PKey.view.mouseEnter
    |> Option.iter (fun v -> Interop.setEventHandler <@ element.MouseEnter @> v element)

    props
    |> Props.tryFind PKey.view.mouseEvent
    |> Option.iter (fun v -> Interop.setEventHandler <@ element.MouseEvent @> v element)

    props
    |> Props.tryFind PKey.view.mouseLeave
    |> Option.iter (fun v -> Interop.setEventHandler <@ element.MouseLeave @> v element)

    props
    |> Props.tryFind PKey.view.mouseStateChanged
    |> Option.iter (fun v -> Interop.setEventHandler <@ element.MouseStateChanged @> v element)

    props
    |> Props.tryFind PKey.view.mouseWheel
    |> Option.iter (fun v -> Interop.setEventHandler <@ element.MouseWheel @> v element)

    props
    |> Props.tryFind PKey.view.removed
    |> Option.iter (fun v -> Interop.setEventHandler <@ element.Removed @> v element)

    props
    |> Props.tryFind PKey.view.schemeChanged
    |> Option.iter (fun v -> Interop.setEventHandler <@ element.SchemeChanged @> v element)

    props
    |> Props.tryFind PKey.view.schemeChanging
    |> Option.iter (fun v -> Interop.setEventHandler <@ element.SchemeChanging @> v element)

    props
    |> Props.tryFind PKey.view.schemeNameChanged
    |> Option.iter (fun v -> Interop.setEventHandler <@ element.SchemeNameChanged @> v element)

    props
    |> Props.tryFind PKey.view.schemeNameChanging
    |> Option.iter (fun v -> Interop.setEventHandler <@ element.SchemeNameChanging @> v element)

    props
    |> Props.tryFind PKey.view.selecting
    |> Option.iter (fun v -> Interop.setEventHandler <@ element.Selecting @> v element)

    props
    |> Props.tryFind PKey.view.subViewAdded
    |> Option.iter (fun v -> Interop.setEventHandler <@ element.SubViewAdded @> v element)

    props
    |> Props.tryFind PKey.view.subViewLayout
    |> Option.iter (fun v -> Interop.setEventHandler <@ element.SubViewLayout @> v element)

    props
    |> Props.tryFind PKey.view.subViewRemoved
    |> Option.iter (fun v -> Interop.setEventHandler <@ element.SubViewRemoved @> v element)

    props
    |> Props.tryFind PKey.view.subViewsLaidOut
    |> Option.iter (fun v -> Interop.setEventHandler <@ element.SubViewsLaidOut @> v element)

    props
    |> Props.tryFind PKey.view.superViewChanged
    |> Option.iter (fun v -> Interop.setEventHandler <@ element.SuperViewChanged @> v element)

    props
    |> Props.tryFind PKey.view.textChanged
    |> Option.iter (fun v -> Interop.setEventHandler <@ element.TextChanged @> (fun _ -> v ()) element)

    props
    |> Props.tryFind PKey.view.titleChanged
    |> Option.iter (fun v -> Interop.setEventHandler <@ element.TitleChanged @> (fun arg -> v arg.Value) element)

    props
    |> Props.tryFind PKey.view.titleChanging
    |> Option.iter (fun v -> Interop.setEventHandler <@ element.TitleChanging @> v element)

    props
    |> Props.tryFind PKey.view.viewportChanged
    |> Option.iter (fun v -> Interop.setEventHandler <@ element.ViewportChanged @> v element)

    props
    |> Props.tryFind PKey.view.visibleChanged
    |> Option.iter (fun v -> Interop.setEventHandler <@ element.VisibleChanged @> (fun _ -> v ()) element)

    props
    |> Props.tryFind PKey.view.visibleChanging
    |> Option.iter (fun v -> Interop.setEventHandler <@ element.VisibleChanging @> (fun _ -> v ()) element)

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
    |> Option.iter (fun _ -> Interop.removeEventHandler <@ element.Accepting @> element)

    props
    |> Props.tryFind PKey.view.advancingFocus
    |> Option.iter (fun _ -> Interop.removeEventHandler <@ element.AdvancingFocus @> element)

    props
    |> Props.tryFind PKey.view.borderStyleChanged
    |> Option.iter (fun _ -> Interop.removeEventHandler <@ element.BorderStyleChanged @> element)

    props
    |> Props.tryFind PKey.view.canFocusChanged
    |> Option.iter (fun _ -> Interop.removeEventHandler <@ element.CanFocusChanged @> element)

    props
    |> Props.tryFind PKey.view.clearedViewport
    |> Option.iter (fun _ -> Interop.removeEventHandler <@ element.ClearedViewport @> element)

    props
    |> Props.tryFind PKey.view.clearingViewport
    |> Option.iter (fun _ -> Interop.removeEventHandler <@ element.ClearingViewport @> element)

    props
    |> Props.tryFind PKey.view.commandNotBound
    |> Option.iter (fun _ -> Interop.removeEventHandler <@ element.CommandNotBound @> element)

    props
    |> Props.tryFind PKey.view.contentSizeChanged
    |> Option.iter (fun _ -> Interop.removeEventHandler <@ element.ContentSizeChanged @> element)

    props
    |> Props.tryFind PKey.view.disposing
    |> Option.iter (fun _ -> Interop.removeEventHandler <@ element.Disposing @> element)

    props
    |> Props.tryFind PKey.view.drawComplete
    |> Option.iter (fun _ -> Interop.removeEventHandler <@ element.DrawComplete @> element)

    props
    |> Props.tryFind PKey.view.drawingContent
    |> Option.iter (fun _ -> Interop.removeEventHandler <@ element.DrawingContent @> element)

    props
    |> Props.tryFind PKey.view.drawingSubViews
    |> Option.iter (fun _ -> Interop.removeEventHandler <@ element.DrawingSubViews @> element)

    props
    |> Props.tryFind PKey.view.drawingText
    |> Option.iter (fun _ -> Interop.removeEventHandler <@ element.DrawingText @> element)

    props
    |> Props.tryFind PKey.view.enabledChanged
    |> Option.iter (fun _ -> Interop.removeEventHandler <@ element.EnabledChanged @> element)

    props
    |> Props.tryFind PKey.view.focusedChanged
    |> Option.iter (fun _ -> Interop.removeEventHandler <@ element.FocusedChanged @> element)

    props
    |> Props.tryFind PKey.view.frameChanged
    |> Option.iter (fun _ -> Interop.removeEventHandler <@ element.FrameChanged @> element)

    props
    |> Props.tryFind PKey.view.gettingAttributeForRole
    |> Option.iter (fun _ -> Interop.removeEventHandler <@ element.GettingAttributeForRole @> element)

    props
    |> Props.tryFind PKey.view.gettingScheme
    |> Option.iter (fun _ -> Interop.removeEventHandler <@ element.GettingScheme @> element)

    props
    |> Props.tryFind PKey.view.handlingHotKey
    |> Option.iter (fun _ -> Interop.removeEventHandler <@ element.HandlingHotKey @> element)

    props
    |> Props.tryFind PKey.view.hasFocusChanged
    |> Option.iter (fun _ -> Interop.removeEventHandler <@ element.HasFocusChanged @> element)

    props
    |> Props.tryFind PKey.view.hasFocusChanging
    |> Option.iter (fun _ -> Interop.removeEventHandler <@ element.HasFocusChanging @> element)

    props
    |> Props.tryFind PKey.view.hotKeyChanged
    |> Option.iter (fun _ -> Interop.removeEventHandler <@ element.HotKeyChanged @> element)

    props
    |> Props.tryFind PKey.view.initialized
    |> Option.iter (fun _ -> Interop.removeEventHandler <@ element.Initialized @> element)

    props
    |> Props.tryFind PKey.view.keyDown
    |> Option.iter (fun _ -> Interop.removeEventHandler <@ element.KeyDown @> element)

    props
    |> Props.tryFind PKey.view.keyDownNotHandled
    |> Option.iter (fun _ -> Interop.removeEventHandler <@ element.KeyDownNotHandled @> element)

    props
    |> Props.tryFind PKey.view.keyUp
    |> Option.iter (fun _ -> Interop.removeEventHandler <@ element.KeyUp @> element)

    props
    |> Props.tryFind PKey.view.mouseClick
    |> Option.iter (fun _ -> Interop.removeEventHandler <@ element.MouseClick @> element)

    props
    |> Props.tryFind PKey.view.mouseEnter
    |> Option.iter (fun _ -> Interop.removeEventHandler <@ element.MouseEnter @> element)

    props
    |> Props.tryFind PKey.view.mouseEvent
    |> Option.iter (fun _ -> Interop.removeEventHandler <@ element.MouseEvent @> element)

    props
    |> Props.tryFind PKey.view.mouseLeave
    |> Option.iter (fun _ -> Interop.removeEventHandler <@ element.MouseLeave @> element)

    props
    |> Props.tryFind PKey.view.mouseStateChanged
    |> Option.iter (fun _ -> Interop.removeEventHandler <@ element.MouseStateChanged @> element)

    props
    |> Props.tryFind PKey.view.mouseWheel
    |> Option.iter (fun _ -> Interop.removeEventHandler <@ element.MouseWheel @> element)

    props
    |> Props.tryFind PKey.view.removed
    |> Option.iter (fun _ -> Interop.removeEventHandler <@ element.Removed @> element)

    props
    |> Props.tryFind PKey.view.schemeChanged
    |> Option.iter (fun _ -> Interop.removeEventHandler <@ element.SchemeChanged @> element)

    props
    |> Props.tryFind PKey.view.schemeChanging
    |> Option.iter (fun _ -> Interop.removeEventHandler <@ element.SchemeChanging @> element)

    props
    |> Props.tryFind PKey.view.schemeNameChanged
    |> Option.iter (fun _ -> Interop.removeEventHandler <@ element.SchemeNameChanged @> element)

    props
    |> Props.tryFind PKey.view.schemeNameChanging
    |> Option.iter (fun _ -> Interop.removeEventHandler <@ element.SchemeNameChanging @> element)

    props
    |> Props.tryFind PKey.view.selecting
    |> Option.iter (fun _ -> Interop.removeEventHandler <@ element.Selecting @> element)

    props
    |> Props.tryFind PKey.view.subViewAdded
    |> Option.iter (fun _ -> Interop.removeEventHandler <@ element.SubViewAdded @> element)

    props
    |> Props.tryFind PKey.view.subViewLayout
    |> Option.iter (fun _ -> Interop.removeEventHandler <@ element.SubViewLayout @> element)

    props
    |> Props.tryFind PKey.view.subViewRemoved
    |> Option.iter (fun _ -> Interop.removeEventHandler <@ element.SubViewRemoved @> element)

    props
    |> Props.tryFind PKey.view.subViewsLaidOut
    |> Option.iter (fun _ -> Interop.removeEventHandler <@ element.SubViewsLaidOut @> element)

    props
    |> Props.tryFind PKey.view.superViewChanged
    |> Option.iter (fun _ -> Interop.removeEventHandler <@ element.SuperViewChanged @> element)

    props
    |> Props.tryFind PKey.view.textChanged
    |> Option.iter (fun _ -> Interop.removeEventHandler <@ element.TextChanged @> element)

    props
    |> Props.tryFind PKey.view.titleChanged
    |> Option.iter (fun _ -> Interop.removeEventHandler <@ element.TitleChanged @> element)

    props
    |> Props.tryFind PKey.view.titleChanging
    |> Option.iter (fun _ -> Interop.removeEventHandler <@ element.TitleChanging @> element)

    props
    |> Props.tryFind PKey.view.viewportChanged
    |> Option.iter (fun _ -> Interop.removeEventHandler <@ element.ViewportChanged @> element)

    props
    |> Props.tryFind PKey.view.visibleChanged
    |> Option.iter (fun _ -> Interop.removeEventHandler <@ element.VisibleChanged @> element)

    props
    |> Props.tryFind PKey.view.visibleChanging
    |> Option.iter (fun _ -> Interop.removeEventHandler <@ element.VisibleChanging @> element)

  /// Reuse a previous `View`, while updating its properties to match the current TerminalElement properties.
  override this.update oldView oldProps =
    let c = this.compare oldProps

    // 0 - foreach unchanged _element property, we identify the _view to reinject to `this` TerminalElement
    let viewKeysToReinject =
      c.unchangedProps
      |> Props.filterSingleElementKeys
      |> Seq.map _.viewKey
      |> Seq.toArray

    // 1 - then we get these Views missing in `this` TerminalElement.
    let viewsPropsToReinject, removedProps =
      c.removedProps
      |> Props.partition (fun kv -> viewKeysToReinject |> Array.contains (kv.Key))

    // 2 - And we add them.
    viewsPropsToReinject
    |> Props.iter (fun kv -> this.props.addNonTyped (kv.Key, kv.Value))

    this.removeProps (oldView, removedProps)
    this.setProps (oldView, c.changedProps)
    this.view <- oldView

  /// <summary>
  /// <seealso cref="DelayedPosKey"/>
  /// </summary>
  override this.layout() =

    let applyPos (apply: Pos -> unit) pos =
      match pos with
      | Some posValue ->
        // TODO: There is still some Pos.* function that needs to be implemented.
        match posValue with
        | TPos.X te -> apply (Pos.X((te :?> IInternalTerminalElement).view))
        | TPos.Y te -> apply (Pos.Y((te :?> IInternalTerminalElement).view))
        | TPos.Top te -> apply (Pos.Top((te :?> IInternalTerminalElement).view))
        | TPos.Bottom te -> apply (Pos.Bottom((te :?> IInternalTerminalElement).view))
        | TPos.Left te -> apply (Pos.Left((te :?> IInternalTerminalElement).view))
        | TPos.Right te -> apply (Pos.Right((te :?> IInternalTerminalElement).view))
        | TPos.Absolute position -> apply (Pos.Absolute(position))
        | TPos.AnchorEnd offset -> apply (Pos.AnchorEnd(offset |> Option.defaultValue 0))
        | TPos.Center -> apply (Pos.Center())
      | None -> ()

    let layout (node: TreeNode) =
      node.TerminalElement.props
      |> Props.tryFind PKey.view.x_delayedPos
      |> applyPos (fun pos -> node.TerminalElement.view.X <- pos)

      node.TerminalElement.props
      |> Props.tryFind PKey.view.y_delayedPos
      |> applyPos (fun pos -> node.TerminalElement.view.Y <- pos)

    traverseTree
      [
        {
          TerminalElement = this
          Parent = this.parent
        }
      ]
      layout

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
        | _ when kv.Key.key = "children" -> // Here we always ignore the 'children' from changed props
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

  interface IInternalTerminalElement with
    member this.initialize(parent) = this.initialize (parent)
    member this.initializeTree(parent) = this.initializeTree (parent)
    member this.canUpdate prevElement oldProps = this.canUpdate prevElement oldProps
    member this.update prevElement oldProps = this.update prevElement oldProps
    member this.layout() = this.layout ()
    member this.view = this.view
    member this.props = this.props
    member this.name = this.name
    member this.children = this.children

    member this.setAsChildOfParentView =
      this.setAsChildOfParentView


module internal ViewElement =


  let canUpdate (view: View) (props: Props) (removedProps: Props) =
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

    [
      positionX
      positionY
      width
      height
      widthNotRemoved
      heightNotRemoved
    ]
    |> List.forall id

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
    |> Option.iter (fun _ -> Interop.removeEventHandler <@ element.ThicknessChanged @> element)

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

  override this.canUpdate prevElement oldProps =
    let changedProps, removedProps =
      Props.compare oldProps props

    let removedProps =
      removedProps
      |> Props.filter (not << _.Key.isViewKey)

    let canUpdateView =
      ViewElement.canUpdate prevElement changedProps removedProps

    let canUpdateElement = true

    canUpdateView && canUpdateElement

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
    |> Option.iter (fun _ -> Interop.removeEventHandler <@ element.OrientationChanged @> element)

    props
    |> Props.tryFind PKey.bar.orientationChanging
    |> Option.iter (fun _ -> Interop.removeEventHandler <@ element.OrientationChanging @> element)

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
    |> Option.iter (fun v -> Interop.setEventHandler <@ element.OrientationChanged @> (fun arg -> v arg.Value) element)

    props
    |> Props.tryFind PKey.bar.orientationChanging
    |> Option.iter (fun v -> Interop.setEventHandler <@ element.OrientationChanging @> v element)


  override this.newView() = new Bar()

  override this.canUpdate prevElement oldProps =
    let changedProps, removedProps =
      Props.compare oldProps props

    let removedProps =
      removedProps
      |> Props.filter (not << _.Key.isViewKey)

    let canUpdateView =
      ViewElement.canUpdate prevElement changedProps removedProps

    let canUpdateElement = true

    canUpdateView && canUpdateElement


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

  override this.canUpdate prevElement oldProps =
    let changedProps, removedProps =
      Props.compare oldProps props

    let removedProps =
      removedProps
      |> Props.filter (not << _.Key.isViewKey)

    let canUpdateView =
      ViewElement.canUpdate prevElement changedProps removedProps

    let canUpdateElement = true

    canUpdateView && canUpdateElement


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

  override this.canUpdate prevElement oldProps =
    let changedProps, removedProps =
      Props.compare oldProps props

    let removedProps =
      removedProps
      |> Props.filter (not << _.Key.isViewKey)

    let canUpdateView =
      ViewElement.canUpdate prevElement changedProps removedProps

    let canUpdateElement = true

    canUpdateView && canUpdateElement


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
    |> Option.iter (fun _ -> Interop.removeEventHandler <@ element.CheckedStateChanging @> element)

    props
    |> Props.tryFind PKey.checkBox.checkedStateChanged
    |> Option.iter (fun _ -> Interop.removeEventHandler <@ element.CheckedStateChanged @> element)

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
    |> Option.iter (fun v -> Interop.setEventHandler <@ element.CheckedStateChanging @> v element)

    props
    |> Props.tryFind PKey.checkBox.checkedStateChanged
    |> Option.iter (fun v -> Interop.setEventHandler <@ element.CheckedStateChanged @> v element)


  override this.newView() = new CheckBox()

  override this.canUpdate prevElement oldProps =
    let changedProps, removedProps =
      Props.compare oldProps props

    let removedProps =
      removedProps
      |> Props.filter (not << _.Key.isViewKey)

    let canUpdateView =
      ViewElement.canUpdate prevElement changedProps removedProps

    let canUpdateElement = true

    canUpdateView && canUpdateElement


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
    |> Option.iter (fun _ -> Interop.removeEventHandler <@ element.ColorChanged @> element)

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
    |> Option.iter (fun v -> Interop.setEventHandler <@ element.ColorChanged @> v element)


  override this.newView() = new ColorPicker()

  override this.canUpdate prevElement oldProps =
    let changedProps, removedProps =
      Props.compare oldProps props

    let removedProps =
      removedProps
      |> Props.filter (not << _.Key.isViewKey)

    let canUpdateView =
      ViewElement.canUpdate prevElement changedProps removedProps

    let canUpdateElement = true

    canUpdateView && canUpdateElement


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
    |> Option.iter (fun _ -> Interop.removeEventHandler <@ element.ColorChanged @> element)

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
    |> Option.iter (fun v -> Interop.setEventHandler <@ element.ColorChanged @> v element)


  override this.newView() = new ColorPicker16()

  override this.canUpdate prevElement oldProps =
    let changedProps, removedProps =
      Props.compare oldProps props

    let removedProps =
      removedProps
      |> Props.filter (not << _.Key.isViewKey)

    let canUpdateView =
      ViewElement.canUpdate prevElement changedProps removedProps

    let canUpdateElement = true

    canUpdateView && canUpdateElement


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
    |> Option.iter (fun _ -> Interop.removeEventHandler <@ element.Collapsed @> element)

    props
    |> Props.tryFind PKey.comboBox.expanded
    |> Option.iter (fun _ -> Interop.removeEventHandler <@ element.Expanded @> element)

    props
    |> Props.tryFind PKey.comboBox.openSelectedItem
    |> Option.iter (fun _ -> Interop.removeEventHandler <@ element.OpenSelectedItem @> element)

    props
    |> Props.tryFind PKey.comboBox.selectedItemChanged
    |> Option.iter (fun _ -> Interop.removeEventHandler <@ element.SelectedItemChanged @> element)

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
    |> Option.iter (fun v -> Interop.setEventHandler <@ element.OpenSelectedItem @> v element)

    props
    |> Props.tryFind PKey.comboBox.selectedItemChanged
    |> Option.iter (fun v -> Interop.setEventHandler <@ element.SelectedItemChanged @> v element)


  override this.newView() = new ComboBox()

  override this.canUpdate prevElement oldProps =
    let changedProps, removedProps =
      Props.compare oldProps props

    let removedProps =
      removedProps
      |> Props.filter (not << _.Key.isViewKey)

    let canUpdateView =
      ViewElement.canUpdate prevElement changedProps removedProps

    let canUpdateElement = true

    canUpdateView && canUpdateElement


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
    |> Option.iter (fun _ -> Interop.removeEventHandler <@ element.DateChanged @> element)

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
    |> Option.iter (fun v -> Interop.setEventHandler <@ element.DateChanged @> v element)


  override this.newView() = new DateField()

  override this.canUpdate prevElement oldProps =
    let changedProps, removedProps =
      Props.compare oldProps props

    let removedProps =
      removedProps
      |> Props.filter (not << _.Key.isViewKey)

    let canUpdateView =
      ViewElement.canUpdate prevElement changedProps removedProps

    let canUpdateElement = true

    canUpdateView && canUpdateElement


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

  override this.canUpdate prevElement oldProps =
    let changedProps, removedProps =
      Props.compare oldProps props

    let removedProps =
      removedProps
      |> Props.filter (not << _.Key.isViewKey)

    let canUpdateView =
      ViewElement.canUpdate prevElement changedProps removedProps

    let canUpdateElement = true

    canUpdateView && canUpdateElement


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

  override this.canUpdate prevElement oldProps =
    let changedProps, removedProps =
      Props.compare oldProps props

    let removedProps =
      removedProps
      |> Props.filter (not << _.Key.isViewKey)

    let canUpdateView =
      ViewElement.canUpdate prevElement changedProps removedProps

    let canUpdateElement = true

    canUpdateView && canUpdateElement


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
    |> Option.iter (fun _ -> Interop.removeEventHandler <@ element.FilesSelected @> element)

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
    |> Option.iter (fun v -> Interop.setEventHandler <@ element.FilesSelected @> v element)


  override this.newView() = new FileDialog()

  override this.canUpdate prevElement oldProps =
    let changedProps, removedProps =
      Props.compare oldProps props

    let removedProps =
      removedProps
      |> Props.filter (not << _.Key.isViewKey)

    let canUpdateView =
      ViewElement.canUpdate prevElement changedProps removedProps

    let canUpdateElement = true

    canUpdateView && canUpdateElement


// FrameView
type FrameViewElement(props: Props) =
  inherit TerminalElement(props)

  override this.removeProps(element: View, props: Props) = base.removeProps (element, props)
  // No properties or events FrameView

  override _.name = $"FrameView"

  override this.setProps(element: View, props: Props) = base.setProps (element, props)
  // No properties or events FrameView


  override this.newView() = new FrameView()

  override this.canUpdate prevElement oldProps =
    let changedProps, removedProps =
      Props.compare oldProps props

    let removedProps =
      removedProps
      |> Props.filter (not << _.Key.isViewKey)

    let canUpdateView =
      ViewElement.canUpdate prevElement changedProps removedProps

    let canUpdateElement = true

    canUpdateView && canUpdateElement


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

  override this.canUpdate prevElement oldProps =
    let changedProps, removedProps =
      Props.compare oldProps props

    let removedProps =
      removedProps
      |> Props.filter (not << _.Key.isViewKey)

    let canUpdateView =
      ViewElement.canUpdate prevElement changedProps removedProps

    let canUpdateElement = true

    canUpdateView && canUpdateElement


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
    |> Option.iter (fun _ -> Interop.removeEventHandler <@ element.Edited @> element)

    props
    |> Props.tryFind PKey.hexView.positionChanged
    |> Option.iter (fun _ -> Interop.removeEventHandler <@ element.PositionChanged @> element)

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
    |> Option.iter (fun v -> Interop.setEventHandler <@ element.Edited @> v element)

    props
    |> Props.tryFind PKey.hexView.positionChanged
    |> Option.iter (fun v -> Interop.setEventHandler <@ element.PositionChanged @> v element)


  override this.newView() = new HexView()

  override this.canUpdate prevElement oldProps =
    let changedProps, removedProps =
      Props.compare oldProps props

    let removedProps =
      removedProps
      |> Props.filter (not << _.Key.isViewKey)

    let canUpdateView =
      ViewElement.canUpdate prevElement changedProps removedProps

    let canUpdateElement = true

    canUpdateView && canUpdateElement


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

  override this.canUpdate prevElement oldProps =
    let changedProps, removedProps =
      Props.compare oldProps props

    let removedProps =
      removedProps
      |> Props.filter (not << _.Key.isViewKey)

    let canUpdateView =
      ViewElement.canUpdate prevElement changedProps removedProps

    let canUpdateElement = true

    canUpdateView && canUpdateElement


// LegendAnnotation
type LegendAnnotationElement(props: Props) =
  inherit TerminalElement(props)

  override this.removeProps(element: View, props: Props) = base.removeProps (element, props)
  // No properties or events LegendAnnotation

  override _.name = $"LegendAnnotation"

  override this.setProps(element: View, props: Props) = base.setProps (element, props)
  // No properties or events LegendAnnotation


  override this.newView() = new LegendAnnotation()

  override this.canUpdate prevElement oldProps =
    let changedProps, removedProps =
      Props.compare oldProps props

    let removedProps =
      removedProps
      |> Props.filter (not << _.Key.isViewKey)

    let canUpdateView =
      ViewElement.canUpdate prevElement changedProps removedProps

    let canUpdateElement = true

    canUpdateView && canUpdateElement


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
    |> Option.iter (fun _ -> Interop.removeEventHandler <@ element.OrientationChanged @> element)

    props
    |> Props.tryFind PKey.line.orientationChanging
    |> Option.iter (fun _ -> Interop.removeEventHandler <@ element.OrientationChanging @> element)

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
    |> Option.iter (fun v -> Interop.setEventHandler <@ element.OrientationChanged @> (fun arg -> v arg.Value) element)

    props
    |> Props.tryFind PKey.line.orientationChanging
    |> Option.iter (fun v -> Interop.setEventHandler <@ element.OrientationChanging @> v element)


  override this.newView() = new Line()

  override this.canUpdate prevElement oldProps =
    let changedProps, removedProps =
      Props.compare oldProps props

    let removedProps =
      removedProps
      |> Props.filter (not << _.Key.isViewKey)

    let canUpdateView =
      ViewElement.canUpdate prevElement changedProps removedProps

    let canUpdateElement = true

    canUpdateView && canUpdateElement


// LineView
type LineViewElement(props: Props) =
  inherit TerminalElement(props)

  override this.removeProps(element: View, props: Props) =
    base.removeProps (element, props)
    let element = element :?> LineView
    // Properties
    props
    |> Props.tryFind PKey.lineView.endingAnchor
    |> Option.iter (fun _ -> element.EndingAnchor <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.lineView.lineRune
    |> Option.iter (fun _ -> element.LineRune <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.lineView.orientation
    |> Option.iter (fun _ -> element.Orientation <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.lineView.startingAnchor
    |> Option.iter (fun _ -> element.StartingAnchor <- Unchecked.defaultof<_>)

  override _.name = $"LineView"

  override this.setProps(element: View, props: Props) =
    base.setProps (element, props)

    let element = element :?> LineView

    // Properties
    props
    |> Props.tryFind PKey.lineView.endingAnchor
    |> Option.iter (fun v -> element.EndingAnchor <- v |> Option.toNullable)

    props
    |> Props.tryFind PKey.lineView.lineRune
    |> Option.iter (fun v -> element.LineRune <- v)

    props
    |> Props.tryFind PKey.lineView.orientation
    |> Option.iter (fun v -> element.Orientation <- v)

    props
    |> Props.tryFind PKey.lineView.startingAnchor
    |> Option.iter (fun v -> element.StartingAnchor <- v |> Option.toNullable)


  override this.newView() = new LineView()

  override this.canUpdate prevElement oldProps =
    let changedProps, removedProps =
      Props.compare oldProps props

    let removedProps =
      removedProps
      |> Props.filter (not << _.Key.isViewKey)

    let canUpdateView =
      ViewElement.canUpdate prevElement changedProps removedProps

    let canUpdateElement = true

    canUpdateView && canUpdateElement


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
    |> Option.iter (fun _ -> Interop.removeEventHandler <@ element.CollectionChanged @> element)

    props
    |> Props.tryFind PKey.listView.openSelectedItem
    |> Option.iter (fun _ -> Interop.removeEventHandler <@ element.OpenSelectedItem @> element)

    props
    |> Props.tryFind PKey.listView.rowRender
    |> Option.iter (fun _ -> Interop.removeEventHandler <@ element.RowRender @> element)

    props
    |> Props.tryFind PKey.listView.selectedItemChanged
    |> Option.iter (fun _ -> Interop.removeEventHandler <@ element.SelectedItemChanged @> element)

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
    |> Option.iter (fun v -> Interop.setEventHandler <@ element.CollectionChanged @> v element)

    props
    |> Props.tryFind PKey.listView.openSelectedItem
    |> Option.iter (fun v -> Interop.setEventHandler <@ element.OpenSelectedItem @> v element)

    props
    |> Props.tryFind PKey.listView.rowRender
    |> Option.iter (fun v -> Interop.setEventHandler <@ element.RowRender @> v element)

    props
    |> Props.tryFind PKey.listView.selectedItemChanged
    |> Option.iter (fun v -> Interop.setEventHandler <@ element.SelectedItemChanged @> v element)


  override this.newView() = new ListView()

  override this.canUpdate prevElement oldProps =
    let changedProps, removedProps =
      Props.compare oldProps props

    let removedProps =
      removedProps
      |> Props.filter (not << _.Key.isViewKey)

    let canUpdateView =
      ViewElement.canUpdate prevElement changedProps removedProps

    let canUpdateElement = true

    canUpdateView && canUpdateElement


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

  override this.canUpdate prevElement oldProps =
    let changedProps, removedProps =
      Props.compare oldProps props

    let removedProps =
      removedProps
      |> Props.filter (not << _.Key.isViewKey)

    let canUpdateView =
      ViewElement.canUpdate prevElement changedProps removedProps

    let canUpdateElement = true

    canUpdateView && canUpdateElement


// Menuv2
type Menuv2Element(props: Props) =
  inherit TerminalElement(props)

  override this.removeProps(element: View, props: Props) =
    base.removeProps (element, props)
    let element = element :?> Menuv2
    // Properties
    props
    |> Props.tryFind PKey.menuv2.selectedMenuItem
    |> Option.iter (fun _ -> element.SelectedMenuItem <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.menuv2.superMenuItem
    |> Option.iter (fun _ -> element.SuperMenuItem <- Unchecked.defaultof<_>)
    // Events
    props
    |> Props.tryFind PKey.menuv2.accepted
    |> Option.iter (fun v -> Interop.setEventHandler <@ element.Accepted @> v element)

    props
    |> Props.tryFind PKey.menuv2.selectedMenuItemChanged
    |> Option.iter (fun v -> Interop.setEventHandler <@ element.SelectedMenuItemChanged @> v element)

    ()

  override _.name = $"Menuv2"

  override this.setProps(element: View, props: Props) =
    base.setProps (element, props)

    let element = element :?> Menuv2

    // Properties
    props
    |> Props.tryFind PKey.menuv2.selectedMenuItem
    |> Option.iter (fun v -> element.SelectedMenuItem <- v)

    props
    |> Props.tryFind PKey.menuv2.superMenuItem
    |> Option.iter (fun v -> element.SuperMenuItem <- v)
    // Events
    props
    |> Props.tryFind PKey.menuv2.accepted
    |> Option.iter (fun v -> Interop.setEventHandler <@ element.Accepted @> v element)

    props
    |> Props.tryFind PKey.menuv2.selectedMenuItemChanged
    |> Option.iter (fun v -> Interop.setEventHandler <@ element.SelectedMenuItemChanged @> v element)

  override this.newView() = new Menuv2()

  override this.canUpdate prevElement oldProps =
    let changedProps, removedProps =
      Props.compare oldProps props

    let removedProps =
      removedProps
      |> Props.filter (not << _.Key.isViewKey)

    let canUpdateView =
      ViewElement.canUpdate prevElement changedProps removedProps

    let canUpdateElement = true

    canUpdateView && canUpdateElement

  override this.setAsChildOfParentView = false

  interface IMenuv2Element


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
    |> Option.iter (fun _ -> Interop.removeEventHandler <@ element.Accepted @> element)

    props
    |> Props.tryFind PKey.popoverMenu.keyChanged
    |> Option.iter (fun _ -> Interop.removeEventHandler <@ element.KeyChanged @> element)

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
    |> Option.iter (fun v -> Interop.setEventHandler <@ element.Accepted @> v element)

    props
    |> Props.tryFind PKey.popoverMenu.keyChanged
    |> Option.iter (fun v -> Interop.setEventHandler <@ element.KeyChanged @> v element)

  override this.subElements =
    ElementPropKey.from PKey.popoverMenu.root_element
    :: base.subElements

  override this.newView() = new PopoverMenu()

  override this.canUpdate prevElement oldProps =
    let changedProps, removedProps =
      Props.compare oldProps props

    let removedProps =
      removedProps
      |> Props.filter (not << _.Key.isViewKey)

    let canUpdateView =
      ViewElement.canUpdate prevElement changedProps removedProps

    let canUpdateElement = true

    canUpdateView && canUpdateElement

  interface IPopoverMenuElement

// MenuBarItemv2
type MenuBarItemv2Element(props: Props) =
  inherit TerminalElement(props)

  override this.removeProps(element: View, props: Props) =
    base.removeProps (element, props)
    let element = element :?> MenuBarItemv2
    // Properties
    props
    |> Props.tryFind PKey.menuBarItemv2.popoverMenu
    |> Option.iter (fun _ -> element.PopoverMenu <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.menuBarItemv2.popoverMenuOpen
    |> Option.iter (fun _ -> element.PopoverMenuOpen <- Unchecked.defaultof<_>)
    // Events
    props
    |> Props.tryFind PKey.menuBarItemv2.popoverMenuOpenChanged
    |> Option.iter (fun _ -> Interop.removeEventHandler <@ element.PopoverMenuOpenChanged @> element)

  override this.name = "MenuBarItemv2"

  override this.setProps(element: View, props: Props) =
    base.setProps (element, props)

    let element = element :?> MenuBarItemv2

    // Properties
    props
    |> Props.tryFind PKey.menuBarItemv2.popoverMenu
    |> Option.iter (fun v -> element.PopoverMenu <- v)

    props
    |> Props.tryFind PKey.menuBarItemv2.popoverMenuOpen
    |> Option.iter (fun v -> element.PopoverMenuOpen <- v)
    // Events
    props
    |> Props.tryFind PKey.menuBarItemv2.popoverMenuOpenChanged
    |> Option.iter (fun v -> Interop.setEventHandler <@ element.PopoverMenuOpenChanged @> (fun args -> v args.Value) element)

  override this.subElements =
    ElementPropKey.from PKey.menuBarItemv2.popoverMenu_element
    :: base.subElements

  override this.newView() = new MenuBarItemv2()

  override this.canUpdate prevElement oldProps =
    let changedProps, removedProps =
      Props.compare oldProps props

    let removedProps =
      removedProps
      |> Props.filter (not << _.Key.isViewKey)

    let canUpdateView =
      ViewElement.canUpdate prevElement changedProps removedProps

    let canUpdateElement = true

    canUpdateView && canUpdateElement

  interface IMenuBarItemv2Element

// MenuBar
type MenuBarv2Element(props: Props) =
  inherit TerminalElement(props)

  override this.removeProps(element: View, props: Props) =
    base.removeProps (element, props)
    let element = element :?> MenuBarv2
    // Properties
    props
    |> Props.tryFind PKey.menuBarv2.key
    |> Option.iter (fun _ -> element.Key <- Unchecked.defaultof<_>)

    // NOTE: No need to handle `Menus: MenuBarItemv2Element list` property here,
    //       as it already registered as "children" property.
    //       And "children" properties are handled by the TreeDiff initializeTree function

    // Events
    props
    |> Props.tryFind PKey.menuBarv2.keyChanged
    |> Option.iter (fun _ -> Interop.removeEventHandler <@ element.KeyChanged @> element)

  override _.name = $"MenuBarv2"

  override this.setProps(element: View, props: Props) =
    base.setProps (element, props)

    let element = element :?> MenuBarv2

    // Properties
    props
    |> Props.tryFind PKey.menuBarv2.key
    |> Option.iter (fun v -> element.Key <- v)

    // NOTE: No need to handle `Menus: MenuBarItemv2Element list` property here,
    //       as it already registered as "children" property.
    //       And "children" properties are handled by the TreeDiff initializeTree function

    // Events
    props
    |> Props.tryFind PKey.menuBarv2.keyChanged
    |> Option.iter (fun v -> Interop.setEventHandler <@ element.KeyChanged @> v element)

  override this.newView() = new MenuBarv2()

  override this.canUpdate prevElement oldProps =
    let changedProps, removedProps =
      Props.compare oldProps props

    let removedProps =
      removedProps
      |> Props.filter (not << _.Key.isViewKey)

    let canUpdateView =
      ViewElement.canUpdate prevElement changedProps removedProps

    let canUpdateElement = true

    canUpdateView && canUpdateElement


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
    |> Option.iter (fun _ -> Interop.removeEventHandler <@ element.OrientationChanged @> element)

    props
    |> Props.tryFind PKey.shortcut.orientationChanging
    |> Option.iter (fun _ -> Interop.removeEventHandler <@ element.OrientationChanging @> element)

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
    |> Option.iter (fun v -> Interop.setEventHandler <@ element.OrientationChanged @> (fun arg -> v arg.Value) element)

    props
    |> Props.tryFind PKey.shortcut.orientationChanging
    |> Option.iter (fun v -> Interop.setEventHandler <@ element.OrientationChanging @> v element)

  override this.subElements =
    ElementPropKey.from PKey.shortcut.commandView_element
    :: base.subElements


  override this.newView() = new Shortcut()

  override this.canUpdate prevElement oldProps =
    let changedProps, removedProps =
      Props.compare oldProps props

    let removedProps =
      removedProps
      |> Props.filter (not << _.Key.isViewKey)

    let canUpdateView =
      ViewElement.canUpdate prevElement changedProps removedProps

    let canUpdateElement = true

    canUpdateView && canUpdateElement


type MenuItemv2Element(props: Props) =
  inherit ShortcutElement(props)

  override this.removeProps(element: View, props: Props) =
    base.removeProps (element, props)
    let element = element :?> MenuItemv2
    // Properties
    props
    |> Props.tryFind PKey.menuItemv2.command
    |> Option.iter (fun _ -> Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.menuItemv2.subMenu
    |> Option.iter (fun _ -> Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.menuItemv2.targetView
    |> Option.iter (fun _ -> Unchecked.defaultof<_>)
    // Events
    props
    |> Props.tryFind PKey.menuItemv2.accepted
    |> Option.iter (fun _ -> Interop.removeEventHandler <@ element.Accepted @> element)

  override _.name = $"MenuItemv2"

  override this.setProps(element: View, props: Props) =
    base.setProps (element, props)

    let element = element :?> MenuItemv2

    // Properties
    props
    |> Props.tryFind PKey.menuItemv2.command
    |> Option.iter (fun v -> element.Command <- v)

    props
    |> Props.tryFind PKey.menuItemv2.subMenu
    |> Option.iter (fun v -> element.SubMenu <- v)

    props
    |> Props.tryFind PKey.menuItemv2.targetView
    |> Option.iter (fun v -> element.TargetView <- v)
    // Events
    props
    |> Props.tryFind PKey.menuItemv2.accepted
    |> Option.iter (fun v -> Interop.setEventHandler <@ element.Accepted @> v element)

  override this.subElements =
    ElementPropKey.from PKey.menuItemv2.subMenu_element
    :: base.subElements


  override this.newView() = new MenuItemv2()


  override this.canUpdate prevElement oldProps =
    let changedProps, removedProps =
      Props.compare oldProps props

    let removedProps =
      removedProps
      |> Props.filter (not << _.Key.isViewKey)

    let canUpdateView =
      ViewElement.canUpdate prevElement changedProps removedProps

    let canUpdateElement = true

    canUpdateView && canUpdateElement

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

  override this.canUpdate prevElement oldProps =
    let changedProps, removedProps =
      Props.compare oldProps props

    let removedProps =
      removedProps
      |> Props.filter (not << _.Key.isViewKey)

    let canUpdateView =
      ViewElement.canUpdate prevElement changedProps removedProps

    let canUpdateElement = true

    canUpdateView && canUpdateElement

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

  override this.canUpdate prevElement oldProps =
    let changedProps, removedProps =
      Props.compare oldProps props

    let removedProps =
      removedProps
      |> Props.filter (not << _.Key.isViewKey)

    let canUpdateView =
      ViewElement.canUpdate prevElement changedProps removedProps

    let canUpdateElement = true

    canUpdateView && canUpdateElement


type OrientationInterface =
  static member removeProps (element: IOrientation) (props: Props) =
    // Properties
    props
    |> Props.tryFind PKey.optionSelector.orientation
    |> Option.iter (fun _ -> element.Orientation <- Unchecked.defaultof<_>)
    // Events
    props
    |> Props.tryFind PKey.optionSelector.orientationChanged
    |> Option.iter (fun _ -> Interop.removeEventHandler <@ element.OrientationChanged @> element)

    props
    |> Props.tryFind PKey.optionSelector.orientationChanging
    |> Option.iter (fun _ -> Interop.removeEventHandler <@ element.OrientationChanging @> element)

  static member setProps (element: IOrientation) (props: Props) =
    // Properties
    props
    |> Props.tryFind PKey.optionSelector.orientation
    |> Option.iter (fun v -> element.Orientation <- v)
    // Events
    props
    |> Props.tryFind PKey.optionSelector.orientationChanged
    |> Option.iter (fun v -> Interop.setEventHandler <@ element.OrientationChanged @> (fun arg -> v arg.Value) element)

    props
    |> Props.tryFind PKey.optionSelector.orientationChanging
    |> Option.iter (fun v -> Interop.setEventHandler <@ element.OrientationChanging @> v element)


// OptionSelector
type OptionSelectorElement(props: Props) =
  inherit TerminalElement(props)

  override this.removeProps(element: View, props: Props) =
    base.removeProps (element, props)
    let element = element :?> OptionSelector
    // Interfaces
    OrientationInterface.removeProps element props

    // Properties
    props
    |> Props.tryFind PKey.optionSelector.assignHotKeysToCheckBoxes
    |> Option.iter (fun _ -> element.AssignHotKeysToCheckBoxes <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.optionSelector.options
    |> Option.iter (fun _ -> element.Options <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.optionSelector.selectedItem
    |> Option.iter (fun _ -> element.SelectedItem <- Unchecked.defaultof<_>)
    // Events
    props
    |> Props.tryFind PKey.optionSelector.selectedItemChanged
    |> Option.iter (fun _ -> Interop.removeEventHandler <@ element.SelectedItemChanged @> element)

  override _.name = $"OptionSelector"

  override this.setProps(element: View, props: Props) =
    base.setProps (element, props)

    let element = element :?> OptionSelector

    // Interfaces
    OrientationInterface.setProps element props

    // Properties
    props
    |> Props.tryFind PKey.optionSelector.assignHotKeysToCheckBoxes
    |> Option.iter (fun v -> element.AssignHotKeysToCheckBoxes <- v)

    props
    |> Props.tryFind PKey.optionSelector.options
    |> Option.iter (fun v -> element.Options <- v)

    props
    |> Props.tryFind PKey.optionSelector.selectedItem
    |> Option.iter (fun v -> element.SelectedItem <- v)
    // Events
    props
    |> Props.tryFind PKey.optionSelector.selectedItemChanged
    |> Option.iter (fun v -> Interop.setEventHandler <@ element.SelectedItemChanged @> v element)


  override this.newView() = new OptionSelector()

  override this.canUpdate prevElement oldProps =
    let changedProps, removedProps =
      Props.compare oldProps props

    let removedProps =
      removedProps
      |> Props.filter (not << _.Key.isViewKey)

    let canUpdateView =
      ViewElement.canUpdate prevElement changedProps removedProps

    let canUpdateElement = true

    canUpdateView && canUpdateElement


// Padding
type PaddingElement(props: Props) =
  inherit TerminalElement(props)

  override this.removeProps(element: View, props: Props) = base.removeProps (element, props)

  override _.name = $"Padding"

  override this.setProps(element: View, props: Props) = base.setProps (element, props)


  override this.newView() = new Padding()

  override this.canUpdate prevElement oldProps =
    let changedProps, removedProps =
      Props.compare oldProps props

    let removedProps =
      removedProps
      |> Props.filter (not << _.Key.isViewKey)

    let canUpdateView =
      ViewElement.canUpdate prevElement changedProps removedProps

    let canUpdateElement = true

    canUpdateView && canUpdateElement


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

  override this.canUpdate prevElement oldProps =
    let changedProps, removedProps =
      Props.compare oldProps props

    let removedProps =
      removedProps
      |> Props.filter (not << _.Key.isViewKey)

    let canUpdateView =
      ViewElement.canUpdate prevElement changedProps removedProps

    let canUpdateElement = true

    canUpdateView && canUpdateElement


// RadioGroup
type RadioGroupElement(props: Props) =
  inherit TerminalElement(props)

  override this.removeProps(element: View, props: Props) =
    base.removeProps (element, props)
    let element = element :?> RadioGroup
    // Properties
    props
    |> Props.tryFind PKey.radioGroup.assignHotKeysToRadioLabels
    |> Option.iter (fun _ -> element.AssignHotKeysToRadioLabels <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.radioGroup.cursor
    |> Option.iter (fun _ -> element.Cursor <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.radioGroup.doubleClickAccepts
    |> Option.iter (fun _ -> element.DoubleClickAccepts <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.radioGroup.horizontalSpace
    |> Option.iter (fun _ -> element.HorizontalSpace <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.radioGroup.orientation
    |> Option.iter (fun _ -> element.Orientation <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.radioGroup.radioLabels
    |> Option.iter (fun _ -> element.RadioLabels <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.radioGroup.selectedItem
    |> Option.iter (fun _ -> element.SelectedItem <- Unchecked.defaultof<_>)
    // Events
    props
    |> Props.tryFind PKey.radioGroup.orientationChanged
    |> Option.iter (fun _ -> Interop.removeEventHandler <@ element.OrientationChanged @> element)

    props
    |> Props.tryFind PKey.radioGroup.orientationChanging
    |> Option.iter (fun _ -> Interop.removeEventHandler <@ element.OrientationChanging @> element)

    props
    |> Props.tryFind PKey.radioGroup.selectedItemChanged
    |> Option.iter (fun _ -> Interop.removeEventHandler <@ element.SelectedItemChanged @> element)

  override _.name = $"RadioGroup"

  override this.setProps(element: View, props: Props) =
    base.setProps (element, props)

    let element = element :?> RadioGroup

    // Properties
    props
    |> Props.tryFind PKey.radioGroup.assignHotKeysToRadioLabels
    |> Option.iter (fun v -> element.AssignHotKeysToRadioLabels <- v)

    props
    |> Props.tryFind PKey.radioGroup.cursor
    |> Option.iter (fun v -> element.Cursor <- v)

    props
    |> Props.tryFind PKey.radioGroup.doubleClickAccepts
    |> Option.iter (fun v -> element.DoubleClickAccepts <- v)

    props
    |> Props.tryFind PKey.radioGroup.horizontalSpace
    |> Option.iter (fun v -> element.HorizontalSpace <- v)

    props
    |> Props.tryFind PKey.radioGroup.orientation
    |> Option.iter (fun v -> element.Orientation <- v)

    props
    |> Props.tryFind PKey.radioGroup.radioLabels
    |> Option.iter (fun v -> element.RadioLabels <- v |> List.toArray)

    props
    |> Props.tryFind PKey.radioGroup.selectedItem
    |> Option.iter (fun v -> element.SelectedItem <- v)
    // Events
    props
    |> Props.tryFind PKey.radioGroup.orientationChanged
    |> Option.iter (fun v -> Interop.setEventHandler <@ element.OrientationChanged @> (fun arg -> v arg.Value) element)

    props
    |> Props.tryFind PKey.radioGroup.orientationChanging
    |> Option.iter (fun v -> Interop.setEventHandler <@ element.OrientationChanging @> v element)

    props
    |> Props.tryFind PKey.radioGroup.selectedItemChanged
    |> Option.iter (fun v -> Interop.setEventHandler <@ element.SelectedItemChanged @> v element)


  override this.newView() = new RadioGroup()

  override this.canUpdate prevElement oldProps =
    let changedProps, removedProps =
      Props.compare oldProps props

    let removedProps =
      removedProps
      |> Props.filter (not << _.Key.isViewKey)

    let canUpdateView =
      ViewElement.canUpdate prevElement changedProps removedProps

    let canUpdateElement = true

    canUpdateView && canUpdateElement


// SaveDialog
type SaveDialogElement(props: Props) =
  inherit TerminalElement(props)

  override this.removeProps(element: View, props: Props) = base.removeProps (element, props)
  // No properties or events SaveDialog

  override _.name = $"SaveDialog"

  override this.setProps(element: View, props: Props) = base.setProps (element, props)
  // No properties or events SaveDialog


  override this.newView() = new SaveDialog()

  override this.canUpdate prevElement oldProps =
    let changedProps, removedProps =
      Props.compare oldProps props

    let removedProps =
      removedProps
      |> Props.filter (not << _.Key.isViewKey)

    let canUpdateView =
      ViewElement.canUpdate prevElement changedProps removedProps

    let canUpdateElement = true

    canUpdateView && canUpdateElement


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
    |> Option.iter (fun _ -> Interop.removeEventHandler <@ element.OrientationChanged @> element)

    props
    |> Props.tryFind PKey.scrollBar.orientationChanging
    |> Option.iter (fun _ -> Interop.removeEventHandler <@ element.OrientationChanging @> element)

    props
    |> Props.tryFind PKey.scrollBar.scrollableContentSizeChanged
    |> Option.iter (fun _ -> Interop.removeEventHandler <@ element.ScrollableContentSizeChanged @> element)

    props
    |> Props.tryFind PKey.scrollBar.sliderPositionChanged
    |> Option.iter (fun _ -> Interop.removeEventHandler <@ element.SliderPositionChanged @> element)

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
    |> Option.iter (fun v -> Interop.setEventHandler <@ element.OrientationChanged @> (fun arg -> v arg.Value) element)

    props
    |> Props.tryFind PKey.scrollBar.orientationChanging
    |> Option.iter (fun v -> Interop.setEventHandler <@ element.OrientationChanging @> v element)

    props
    |> Props.tryFind PKey.scrollBar.scrollableContentSizeChanged
    |> Option.iter (fun v -> Interop.setEventHandler <@ element.ScrollableContentSizeChanged @> v element)

    props
    |> Props.tryFind PKey.scrollBar.sliderPositionChanged
    |> Option.iter (fun v -> Interop.setEventHandler <@ element.SliderPositionChanged @> v element)


  override this.newView() = new ScrollBar()

  override this.canUpdate prevElement oldProps =
    let changedProps, removedProps =
      Props.compare oldProps props

    let removedProps =
      removedProps
      |> Props.filter (not << _.Key.isViewKey)

    let canUpdateView =
      ViewElement.canUpdate prevElement changedProps removedProps

    let canUpdateElement = true

    canUpdateView && canUpdateElement


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
    |> Option.iter (fun _ -> Interop.removeEventHandler <@ element.OrientationChanged @> element)

    props
    |> Props.tryFind PKey.scrollSlider.orientationChanging
    |> Option.iter (fun _ -> Interop.removeEventHandler <@ element.OrientationChanging @> element)

    props
    |> Props.tryFind PKey.scrollSlider.positionChanged
    |> Option.iter (fun _ -> Interop.removeEventHandler <@ element.PositionChanged @> element)

    props
    |> Props.tryFind PKey.scrollSlider.positionChanging
    |> Option.iter (fun _ -> Interop.removeEventHandler <@ element.PositionChanging @> element)

    props
    |> Props.tryFind PKey.scrollSlider.scrolled
    |> Option.iter (fun _ -> Interop.removeEventHandler <@ element.Scrolled @> element)

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
    |> Option.iter (fun v -> Interop.setEventHandler <@ element.OrientationChanged @> (fun arg -> v arg.Value) element)

    props
    |> Props.tryFind PKey.scrollSlider.orientationChanging
    |> Option.iter (fun v -> Interop.setEventHandler <@ element.OrientationChanging @> v element)

    props
    |> Props.tryFind PKey.scrollSlider.positionChanged
    |> Option.iter (fun v -> Interop.setEventHandler <@ element.PositionChanged @> v element)

    props
    |> Props.tryFind PKey.scrollSlider.positionChanging
    |> Option.iter (fun v -> Interop.setEventHandler <@ element.PositionChanging @> v element)

    props
    |> Props.tryFind PKey.scrollSlider.scrolled
    |> Option.iter (fun v -> Interop.setEventHandler <@ element.Scrolled @> v element)


  override this.newView() = new ScrollSlider()

  override this.canUpdate prevElement oldProps =
    let changedProps, removedProps =
      Props.compare oldProps props

    let removedProps =
      removedProps
      |> Props.filter (not << _.Key.isViewKey)

    let canUpdateView =
      ViewElement.canUpdate prevElement changedProps removedProps

    let canUpdateElement = true

    canUpdateView && canUpdateElement

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
    |> Option.iter (fun _ -> Interop.removeEventHandler <@ element.OptionFocused @> element)

    props
    |> Props.tryFind PKey.slider.optionsChanged
    |> Option.iter (fun _ -> Interop.removeEventHandler <@ element.OptionsChanged @> element)

    props
    |> Props.tryFind PKey.slider.orientationChanged
    |> Option.iter (fun _ -> Interop.removeEventHandler <@ element.OrientationChanged @> element)

    props
    |> Props.tryFind PKey.slider.orientationChanging
    |> Option.iter (fun _ -> Interop.removeEventHandler <@ element.OrientationChanging @> element)

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
    |> Option.iter (fun v -> Interop.setEventHandler <@ element.OptionFocused @> v element)

    props
    |> Props.tryFind PKey.slider.optionsChanged
    |> Option.iter (fun v -> Interop.setEventHandler <@ element.OptionsChanged @> v element)

    props
    |> Props.tryFind PKey.slider.orientationChanged
    |> Option.iter (fun v -> Interop.setEventHandler <@ element.OrientationChanged @> (fun arg -> v arg.Value) element)

    props
    |> Props.tryFind PKey.slider.orientationChanging
    |> Option.iter (fun v -> Interop.setEventHandler <@ element.OrientationChanging @> v element)


  override this.newView() = new Slider<'a>()

  override this.canUpdate prevElement oldProps =
    let changedProps, removedProps =
      Props.compare oldProps props

    let removedProps =
      removedProps
      |> Props.filter (not << _.Key.isViewKey)

    let canUpdateView =
      ViewElement.canUpdate prevElement changedProps removedProps

    let canUpdateElement = true

    canUpdateView && canUpdateElement

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

  override this.canUpdate prevElement oldProps =
    let changedProps, removedProps =
      Props.compare oldProps props

    let removedProps =
      removedProps
      |> Props.filter (not << _.Key.isViewKey)

    let canUpdateView =
      ViewElement.canUpdate prevElement changedProps removedProps

    let canUpdateElement = true

    canUpdateView && canUpdateElement


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

  override this.canUpdate prevElement oldProps =
    let changedProps, removedProps =
      Props.compare oldProps props

    let removedProps =
      removedProps
      |> Props.filter (not << _.Key.isViewKey)

    let canUpdateView =
      ViewElement.canUpdate prevElement changedProps removedProps

    let canUpdateElement = true

    canUpdateView && canUpdateElement


// StatusBar
type StatusBarElement(props: Props) =
  inherit TerminalElement(props)

  override this.removeProps(element: View, props: Props) = base.removeProps (element, props)
  // No properties or events StatusBar

  override _.name = $"StatusBar"

  override this.setProps(element: View, props: Props) = base.setProps (element, props)
  // No properties or events StatusBar


  override this.newView() = new StatusBar()

  override this.canUpdate prevElement oldProps =
    let changedProps, removedProps =
      Props.compare oldProps props

    let removedProps =
      removedProps
      |> Props.filter (not << _.Key.isViewKey)

    let canUpdateView =
      ViewElement.canUpdate prevElement changedProps removedProps

    let canUpdateElement = true

    canUpdateView && canUpdateElement


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

  override this.subElements =
    ElementPropKey.from PKey.tab.view_element
    :: base.subElements

  override this.newView() = new Tab()

  override this.canUpdate prevElement oldProps =
    let changedProps, removedProps =
      Props.compare oldProps props

    let removedProps =
      removedProps
      |> Props.filter (not << _.Key.isViewKey)

    let canUpdateView =
      ViewElement.canUpdate prevElement changedProps removedProps

    let canUpdateElement = true

    canUpdateView && canUpdateElement


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
    |> Option.iter (fun _ -> Interop.removeEventHandler <@ element.SelectedTabChanged @> element)

    props
    |> Props.tryFind PKey.tabView.tabClicked
    |> Option.iter (fun _ -> Interop.removeEventHandler <@ element.TabClicked @> element)

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
    |> Option.iter (fun v -> Interop.setEventHandler <@ element.SelectedTabChanged @> v element)

    props
    |> Props.tryFind PKey.tabView.tabClicked
    |> Option.iter (fun v -> Interop.setEventHandler <@ element.TabClicked @> v element)

    // Additional properties
    props
    |> Props.tryFind PKey.tabView.tabs
    |> Option.iter (fun tabItems ->
      tabItems
      |> Seq.iter (fun tabItem -> element.AddTab((tabItem :?> IInternalTerminalElement).view :?> Tab, false))
    )

  override this.subElements =
    ElementPropKey.from PKey.tabView.tabs_elements
    :: base.subElements

  override this.newView() = new TabView()

  override this.canUpdate prevElement oldProps =
    let changedProps, removedProps =
      Props.compare oldProps props

    let removedProps =
      removedProps
      |> Props.filter (not << _.Key.isViewKey)

    let canUpdateView =
      ViewElement.canUpdate prevElement changedProps removedProps

    let canUpdateElement = true

    canUpdateView && canUpdateElement


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
    |> Option.iter (fun _ -> Interop.removeEventHandler <@ element.CellActivated @> element)

    props
    |> Props.tryFind PKey.tableView.cellToggled
    |> Option.iter (fun _ -> Interop.removeEventHandler <@ element.CellToggled @> element)

    props
    |> Props.tryFind PKey.tableView.selectedCellChanged
    |> Option.iter (fun _ -> Interop.removeEventHandler <@ element.SelectedCellChanged @> element)

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
    |> Option.iter (fun v -> Interop.setEventHandler <@ element.CellActivated @> v element)

    props
    |> Props.tryFind PKey.tableView.cellToggled
    |> Option.iter (fun v -> Interop.setEventHandler <@ element.CellToggled @> v element)

    props
    |> Props.tryFind PKey.tableView.selectedCellChanged
    |> Option.iter (fun v -> Interop.setEventHandler <@ element.SelectedCellChanged @> v element)


  override this.newView() = new TableView()

  override this.canUpdate prevElement oldProps =
    let changedProps, removedProps =
      Props.compare oldProps props

    let removedProps =
      removedProps
      |> Props.filter (not << _.Key.isViewKey)

    let canUpdateView =
      ViewElement.canUpdate prevElement changedProps removedProps

    let canUpdateElement = true

    canUpdateView && canUpdateElement


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
    |> Props.tryFind PKey.textField.caption
    |> Option.iter (fun _ -> element.Caption <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.textField.captionColor
    |> Option.iter (fun _ -> element.CaptionColor <- Unchecked.defaultof<_>)

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
    |> Option.iter (fun _ -> Interop.removeEventHandler <@ element.TextChanging @> element)

  override _.name = $"TextField"

  override this.setProps(element: View, props: Props) =
    base.setProps (element, props)

    let element = element :?> TextField

    // Properties
    props
    |> Props.tryFind PKey.textField.autocomplete
    |> Option.iter (fun v -> element.Autocomplete <- v)

    props
    |> Props.tryFind PKey.textField.caption
    |> Option.iter (fun v -> element.Caption <- v)

    props
    |> Props.tryFind PKey.textField.captionColor
    |> Option.iter (fun v -> element.CaptionColor <- v)

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
    |> Option.iter (fun v -> Interop.setEventHandler <@ element.TextChanging @> v element)


  override this.newView() = new TextField()

  override this.canUpdate prevElement oldProps =
    let changedProps, removedProps =
      Props.compare oldProps props

    let removedProps =
      removedProps
      |> Props.filter (not << _.Key.isViewKey)

    let canUpdateView =
      ViewElement.canUpdate prevElement changedProps removedProps

    let canUpdateElement = true

    canUpdateView && canUpdateElement


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

  override this.canUpdate prevElement oldProps =
    let changedProps, removedProps =
      Props.compare oldProps props

    let removedProps =
      removedProps
      |> Props.filter (not << _.Key.isViewKey)

    let canUpdateView =
      ViewElement.canUpdate prevElement changedProps removedProps

    let canUpdateElement = true

    canUpdateView && canUpdateElement


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
    |> Option.iter (fun _ -> Interop.removeEventHandler <@ element.ContentsChanged @> element)

    props
    |> Props.tryFind PKey.textView.drawNormalColor
    |> Option.iter (fun _ -> Interop.removeEventHandler <@ element.DrawNormalColor @> element)

    props
    |> Props.tryFind PKey.textView.drawReadOnlyColor
    |> Option.iter (fun _ -> Interop.removeEventHandler <@ element.DrawReadOnlyColor @> element)

    props
    |> Props.tryFind PKey.textView.drawSelectionColor
    |> Option.iter (fun _ -> Interop.removeEventHandler <@ element.DrawSelectionColor @> element)

    props
    |> Props.tryFind PKey.textView.drawUsedColor
    |> Option.iter (fun _ -> Interop.removeEventHandler <@ element.DrawUsedColor @> element)

    props
    |> Props.tryFind PKey.textView.unwrappedCursorPosition
    |> Option.iter (fun _ -> Interop.removeEventHandler <@ element.UnwrappedCursorPosition @> element)

    // Additional properties
    props
    |> Props.tryFind PKey.textView.textChanged
    |> Option.iter (fun _ -> Interop.removeEventHandler <@ element.ContentsChanged @> element)


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
    |> Option.iter (fun v -> Interop.setEventHandler <@ element.ContentsChanged @> v element)

    props
    |> Props.tryFind PKey.textView.drawNormalColor
    |> Option.iter (fun v -> Interop.setEventHandler <@ element.DrawNormalColor @> v element)

    props
    |> Props.tryFind PKey.textView.drawReadOnlyColor
    |> Option.iter (fun v -> Interop.setEventHandler <@ element.DrawReadOnlyColor @> v element)

    props
    |> Props.tryFind PKey.textView.drawSelectionColor
    |> Option.iter (fun v -> Interop.setEventHandler <@ element.DrawSelectionColor @> v element)

    props
    |> Props.tryFind PKey.textView.drawUsedColor
    |> Option.iter (fun v -> Interop.setEventHandler <@ element.DrawUsedColor @> v element)

    props
    |> Props.tryFind PKey.textView.unwrappedCursorPosition
    |> Option.iter (fun v -> Interop.setEventHandler <@ element.UnwrappedCursorPosition @> v element)

    // Additional properties
    props
    |> Props.tryFind PKey.textView.textChanged
    |> Option.iter (fun v -> Interop.setEventHandler <@ element.ContentsChanged @> (fun _ -> v element.Text) element)


  override this.newView() = new TextView()

  override this.canUpdate prevElement oldProps =
    let changedProps, removedProps =
      Props.compare oldProps props

    let removedProps =
      removedProps
      |> Props.filter (not << _.Key.isViewKey)

    let canUpdateView =
      ViewElement.canUpdate prevElement changedProps removedProps

    let canUpdateElement = true

    canUpdateView && canUpdateElement


// TileView
type TileViewElement(props: Props) =
  inherit TerminalElement(props)

  override this.removeProps(element: View, props: Props) =
    base.removeProps (element, props)
    let element = element :?> TileView
    // Properties
    props
    |> Props.tryFind PKey.tileView.lineStyle
    |> Option.iter (fun _ -> element.LineStyle <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.tileView.orientation
    |> Option.iter (fun _ -> element.Orientation <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.tileView.toggleResizable
    |> Option.iter (fun _ -> element.ToggleResizable <- Unchecked.defaultof<_>)
    // Events
    props
    |> Props.tryFind PKey.tileView.splitterMoved
    |> Option.iter (fun _ -> Interop.removeEventHandler <@ element.SplitterMoved @> element)

  override _.name = $"TileView"

  override this.setProps(element: View, props: Props) =
    base.setProps (element, props)

    let element = element :?> TileView

    // Properties
    props
    |> Props.tryFind PKey.tileView.lineStyle
    |> Option.iter (fun v -> element.LineStyle <- v)

    props
    |> Props.tryFind PKey.tileView.orientation
    |> Option.iter (fun v -> element.Orientation <- v)

    props
    |> Props.tryFind PKey.tileView.toggleResizable
    |> Option.iter (fun v -> element.ToggleResizable <- v)
    // Events
    props
    |> Props.tryFind PKey.tileView.splitterMoved
    |> Option.iter (fun v -> Interop.setEventHandler <@ element.SplitterMoved @> v element)


  override this.newView() = new TileView()

  override this.canUpdate prevElement oldProps =
    let changedProps, removedProps =
      Props.compare oldProps props

    let removedProps =
      removedProps
      |> Props.filter (not << _.Key.isViewKey)

    let canUpdateView =
      ViewElement.canUpdate prevElement changedProps removedProps

    let canUpdateElement = true

    canUpdateView && canUpdateElement


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
    |> Option.iter (fun _ -> Interop.removeEventHandler <@ element.TimeChanged @> element)

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
    |> Option.iter (fun v -> Interop.setEventHandler <@ element.TimeChanged @> v element)


  override this.newView() = new TimeField()

  override this.canUpdate prevElement oldProps =
    let changedProps, removedProps =
      Props.compare oldProps props

    let removedProps =
      removedProps
      |> Props.filter (not << _.Key.isViewKey)

    let canUpdateView =
      ViewElement.canUpdate prevElement changedProps removedProps

    let canUpdateElement = true

    canUpdateView && canUpdateElement


// Toplevel
type ToplevelElement(props: Props) =
  inherit TerminalElement(props)

  override this.removeProps(element: View, props: Props) =
    base.removeProps (element, props)
    let element = element :?> Toplevel
    // Properties
    props
    |> Props.tryFind PKey.toplevel.modal
    |> Option.iter (fun _ -> element.Modal <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.toplevel.running
    |> Option.iter (fun _ -> element.Running <- Unchecked.defaultof<_>)
    // Events
    props
    |> Props.tryFind PKey.toplevel.activate
    |> Option.iter (fun _ -> Interop.removeEventHandler <@ element.Activate @> element)

    props
    |> Props.tryFind PKey.toplevel.closed
    |> Option.iter (fun _ -> Interop.removeEventHandler <@ element.Closed @> element)

    props
    |> Props.tryFind PKey.toplevel.closing
    |> Option.iter (fun _ -> Interop.removeEventHandler <@ element.Closing @> element)

    props
    |> Props.tryFind PKey.toplevel.deactivate
    |> Option.iter (fun _ -> Interop.removeEventHandler <@ element.Deactivate @> element)

    props
    |> Props.tryFind PKey.toplevel.loaded
    |> Option.iter (fun _ -> Interop.removeEventHandler <@ element.Loaded @> element)

    props
    |> Props.tryFind PKey.toplevel.ready
    |> Option.iter (fun _ -> Interop.removeEventHandler <@ element.Ready @> element)

    props
    |> Props.tryFind PKey.toplevel.sizeChanging
    |> Option.iter (fun _ -> Interop.removeEventHandler <@ element.SizeChanging @> element)

    props
    |> Props.tryFind PKey.toplevel.unloaded
    |> Option.iter (fun _ -> Interop.removeEventHandler <@ element.Unloaded @> element)

  override _.name = $"Toplevel"

  override this.setProps(element: View, props: Props) =
    base.setProps (element, props)

    let element = element :?> Toplevel

    // Properties
    props
    |> Props.tryFind PKey.toplevel.modal
    |> Option.iter (fun v -> element.Modal <- v)

    props
    |> Props.tryFind PKey.toplevel.running
    |> Option.iter (fun v -> element.Running <- v)
    // Events
    props
    |> Props.tryFind PKey.toplevel.activate
    |> Option.iter (fun v -> Interop.setEventHandler <@ element.Activate @> v element)

    props
    |> Props.tryFind PKey.toplevel.closed
    |> Option.iter (fun v -> Interop.setEventHandler <@ element.Closed @> v element)

    props
    |> Props.tryFind PKey.toplevel.closing
    |> Option.iter (fun v -> Interop.setEventHandler <@ element.Closing @> v element)

    props
    |> Props.tryFind PKey.toplevel.deactivate
    |> Option.iter (fun v -> Interop.setEventHandler <@ element.Deactivate @> v element)

    props
    |> Props.tryFind PKey.toplevel.loaded
    |> Option.iter (fun v -> Interop.setEventHandler <@ element.Loaded @> (fun _ -> v ()) element)

    props
    |> Props.tryFind PKey.toplevel.ready
    |> Option.iter (fun v -> Interop.setEventHandler <@ element.Ready @> (fun _ -> v ()) element)

    props
    |> Props.tryFind PKey.toplevel.sizeChanging
    |> Option.iter (fun v -> Interop.setEventHandler <@ element.SizeChanging @> v element)

    props
    |> Props.tryFind PKey.toplevel.unloaded
    |> Option.iter (fun v -> Interop.setEventHandler <@ element.Unloaded @> (fun _ -> v ()) element)


  override this.newView() = new Toplevel()

  override this.canUpdate prevElement oldProps =
    let changedProps, removedProps =
      Props.compare oldProps props

    let removedProps =
      removedProps
      |> Props.filter (not << _.Key.isViewKey)

    let canUpdateView =
      ViewElement.canUpdate prevElement changedProps removedProps

    let canUpdateElement = true

    canUpdateView && canUpdateElement

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

  override this.canUpdate prevElement oldProps =
    let changedProps, removedProps =
      Props.compare oldProps props

    let removedProps =
      removedProps
      |> Props.filter (not << _.Key.isViewKey)

    let canUpdateView =
      ViewElement.canUpdate prevElement changedProps removedProps

    let canUpdateElement = true

    canUpdateView && canUpdateElement

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

  override this.canUpdate prevElement oldProps =
    let changedProps, removedProps =
      Props.compare oldProps props

    let removedProps =
      removedProps
      |> Props.filter (not << _.Key.isViewKey)

    let canUpdateView =
      ViewElement.canUpdate prevElement changedProps removedProps

    let canUpdateElement = true

    canUpdateView && canUpdateElement


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
    |> Option.iter (fun _ -> element.Modal <- Unchecked.defaultof<_>)
    // Events
    props
    |> Props.tryFind PKey.wizard.cancelled
    |> Option.iter (fun _ -> Interop.removeEventHandler <@ element.Cancelled @> element)

    props
    |> Props.tryFind PKey.wizard.finished
    |> Option.iter (fun _ -> Interop.removeEventHandler <@ element.Finished @> element)

    props
    |> Props.tryFind PKey.wizard.movingBack
    |> Option.iter (fun _ -> Interop.removeEventHandler <@ element.MovingBack @> element)

    props
    |> Props.tryFind PKey.wizard.movingNext
    |> Option.iter (fun _ -> Interop.removeEventHandler <@ element.MovingNext @> element)

    props
    |> Props.tryFind PKey.wizard.stepChanged
    |> Option.iter (fun _ -> Interop.removeEventHandler <@ element.StepChanged @> element)

    props
    |> Props.tryFind PKey.wizard.stepChanging
    |> Option.iter (fun _ -> Interop.removeEventHandler <@ element.StepChanging @> element)

  override _.name = $"Wizard"

  override _.setProps(element: View, props: Props) =
    base.setProps (element, props)

    let element = element :?> Wizard

    // Properties
    props
    |> Props.tryFind PKey.wizard.currentStep
    |> Option.iter (fun v -> element.CurrentStep <- v)

    props
    |> Props.tryFind PKey.wizard.modal
    |> Option.iter (fun v -> element.Modal <- v)
    // Events
    props
    |> Props.tryFind PKey.wizard.cancelled
    |> Option.iter (fun v -> Interop.setEventHandler <@ element.Cancelled @> v element)

    props
    |> Props.tryFind PKey.wizard.finished
    |> Option.iter (fun v -> Interop.setEventHandler <@ element.Finished @> v element)

    props
    |> Props.tryFind PKey.wizard.movingBack
    |> Option.iter (fun v -> Interop.setEventHandler <@ element.MovingBack @> v element)

    props
    |> Props.tryFind PKey.wizard.movingNext
    |> Option.iter (fun v -> Interop.setEventHandler <@ element.MovingNext @> v element)

    props
    |> Props.tryFind PKey.wizard.stepChanged
    |> Option.iter (fun v -> Interop.setEventHandler <@ element.StepChanged @> v element)

    props
    |> Props.tryFind PKey.wizard.stepChanging
    |> Option.iter (fun v -> Interop.setEventHandler <@ element.StepChanging @> v element)


  override this.newView() = new Wizard()

  override this.canUpdate prevElement oldProps =
    let changedProps, removedProps =
      Props.compare oldProps props

    let removedProps =
      removedProps
      |> Props.filter (not << _.Key.isViewKey)

    let canUpdateView =
      ViewElement.canUpdate prevElement changedProps removedProps

    let canUpdateElement = true

    canUpdateView && canUpdateElement


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

  override this.canUpdate prevElement oldProps =
    let changedProps, removedProps =
      Props.compare oldProps props

    let removedProps =
      removedProps
      |> Props.filter (not << _.Key.isViewKey)

    let canUpdateView =
      ViewElement.canUpdate prevElement changedProps removedProps

    let canUpdateElement = true

    canUpdateView && canUpdateElement
