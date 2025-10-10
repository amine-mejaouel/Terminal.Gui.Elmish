
(*
#######################################
#            Elements.fs              #
#######################################
*)


namespace Terminal.Gui.Elmish.Elements

open System
open System.Collections.Specialized
open System.Collections.ObjectModel
open System.ComponentModel
open System.Drawing
open System.Globalization
open System.Text
open System.Linq
open System.IO
open Terminal.Gui
open Terminal.Gui.App
open Terminal.Gui.Drawing
open Terminal.Gui.Drivers
open Terminal.Gui.Elmish
open Terminal.Gui.FileServices
open Terminal.Gui.Input
open Terminal.Gui.Text
open Terminal.Gui.ViewBase
open Terminal.Gui.Views


[<AbstractClass>]
type TerminalElement (props:Props) =
    let mutable view: View = null
    let mutable p: View option = None
    let mutable addProps = Props.init()
    let c = props |> Props.tryFindWithDefault<TerminalElement list> "children" []

    member this.parent with get() = p and set v = p <- v
    // TODO: rename to view ?
    member this.element with get() = view and set v = view <- v
    // TODO: is this being used ?
    member this.additionalProps with get() = addProps and set v = addProps <- v
    member _.properties = Props.merge props addProps
    member _.children   = c

    abstract subElements: {| key: string; setParent: bool |} list
    default _.subElements = []

    abstract initialize: parent:View option -> unit
    abstract update: prevElement:View -> oldProps:Props -> unit
    abstract canUpdate: prevElement:View -> oldProps:Props -> bool
    abstract name: string

    member this.initializeTree(parent: View option) =
        this.initialize parent
        this.children |> List.iter (fun e -> e.initializeTree (Some this.element))

    static member initializeElement elementProp parent (props: Props) =
        // TODO: Move that outside to get rid of `props` param
        let element =
            props |> Props.tryFind<TerminalElement> elementProp

        match element with
        | None ->
            None
        | Some element ->
            element.initializeTree parent

            let viewProp = elementProp.Substring(0, elementProp.Length - (*".element".Length*) 8)

            Some (viewProp, element.element)

    member this.initializeSubElements parent =
        seq {
            for x in this.subElements do
                let parent =
                    match x.setParent with
                    | true -> Some parent
                    | false -> None

                match TerminalElement.initializeElement x.key parent props with
                | Some x -> yield x
                | None -> ()
        }

    member this.setProps (element: View, props: Props) =
        // Properties
        props |> Props.tryFind<View->unit> "ref" |> Option.iter (fun v -> v element)
        // Properties
        props |> Props.tryFind<ViewArrangement> "view.arrangement" |> Option.iter (fun v -> element.Arrangement <- v )
        props |> Props.tryFind<LineStyle> "view.borderStyle" |> Option.iter (fun v -> element.BorderStyle <- v )
        props |> Props.tryFind<bool> "view.canFocus" |> Option.iter (fun v -> element.CanFocus <- v )
        props |> Props.tryFind<bool> "view.contentSizeTracksViewport" |> Option.iter (fun v -> element.ContentSizeTracksViewport <- v )
        props |> Props.tryFind<CursorVisibility> "view.cursorVisibility" |> Option.iter (fun v -> element.CursorVisibility <- v )
        props |> Props.tryFind<Object> "view.data" |> Option.iter (fun v -> element.Data <- v )
        props |> Props.tryFind<bool> "view.enabled" |> Option.iter (fun v -> element.Enabled <- v )
        props |> Props.tryFind<Rectangle> "view.frame" |> Option.iter (fun v -> element.Frame <- v )
        props |> Props.tryFind<bool> "view.hasFocus" |> Option.iter (fun v -> element.HasFocus <- v )
        props |> Props.tryFind<Dim> "view.height" |> Option.iter (fun v -> element.Height <- v )
        props |> Props.tryFind<MouseState> "view.highlightStates" |> Option.iter (fun v -> element.HighlightStates <- v )
        props |> Props.tryFind<Key> "view.hotKey" |> Option.iter (fun v -> element.HotKey <- v )
        props |> Props.tryFind<Rune> "view.hotKeySpecifier" |> Option.iter (fun v -> element.HotKeySpecifier <- v )
        props |> Props.tryFind<string> "view.id" |> Option.iter (fun v -> element.Id <- v )
        props |> Props.tryFind<bool> "view.isInitialized" |> Option.iter (fun v -> element.IsInitialized <- v )
        props |> Props.tryFind<IMouseHeldDown> "view.mouseHeldDown" |> Option.iter (fun v -> element.MouseHeldDown <- v )
        props |> Props.tryFind<bool> "view.needsDraw" |> Option.iter (fun v -> element.NeedsDraw <- v )
        props |> Props.tryFind<bool> "view.preserveTrailingSpaces" |> Option.iter (fun v -> element.PreserveTrailingSpaces <- v )
        props |> Props.tryFind<string> "view.schemeName" |> Option.iter (fun v -> element.SchemeName <- v )
        props |> Props.tryFind<ShadowStyle> "view.shadowStyle" |> Option.iter (fun v -> element.ShadowStyle <- v )
        props |> Props.tryFind<bool> "view.superViewRendersLineCanvas" |> Option.iter (fun v -> element.SuperViewRendersLineCanvas <- v )
        props |> Props.tryFind<TabBehavior option> "view.tabStop" |> Option.iter (fun v -> element.TabStop <- v  |> Option.toNullable)
        props |> Props.tryFind<string> "view.text" |> Option.iter (fun v -> element.Text <- v )
        props |> Props.tryFind<Alignment> "view.textAlignment" |> Option.iter (fun v -> element.TextAlignment <- v )
        props |> Props.tryFind<TextDirection> "view.textDirection" |> Option.iter (fun v -> element.TextDirection <- v )
        props |> Props.tryFind<string> "view.title" |> Option.iter (fun v -> element.Title <- v )
        props |> Props.tryFind<bool> "view.validatePosDim" |> Option.iter (fun v -> element.ValidatePosDim <- v )
        props |> Props.tryFind<Alignment> "view.verticalTextAlignment" |> Option.iter (fun v -> element.VerticalTextAlignment <- v )
        props |> Props.tryFind<Rectangle> "view.viewport" |> Option.iter (fun v -> element.Viewport <- v )
        props |> Props.tryFind<ViewportSettingsFlags> "view.viewportSettings" |> Option.iter (fun v -> element.ViewportSettings <- v )
        props |> Props.tryFind<bool> "view.visible" |> Option.iter (fun v -> element.Visible <- v )
        props |> Props.tryFind<bool> "view.wantContinuousButtonPressed" |> Option.iter (fun v -> element.WantContinuousButtonPressed <- v )
        props |> Props.tryFind<bool> "view.wantMousePositionReports" |> Option.iter (fun v -> element.WantMousePositionReports <- v )
        props |> Props.tryFind<Dim> "view.width" |> Option.iter (fun v -> element.Width <- v )
        props |> Props.tryFind<Pos> "view.x" |> Option.iter (fun v -> element.X <- v )
        props |> Props.tryFind<Pos> "view.y" |> Option.iter (fun v -> element.Y <- v )
        // Events
        props |> Props.tryFind<HandledEventArgs->unit> "view.accepting" |> Option.iter (fun v -> Interop.setEventHandler <@ element.Accepting @> v element)
        props |> Props.tryFind<AdvanceFocusEventArgs->unit> "view.advancingFocus" |> Option.iter (fun v -> Interop.setEventHandler <@ element.AdvancingFocus @> v element)
        props |> Props.tryFind<EventArgs->unit> "view.borderStyleChanged" |> Option.iter (fun v -> Interop.setEventHandler <@ element.BorderStyleChanged @> v element)
        props |> Props.tryFind<unit->unit> "view.canFocusChanged" |> Option.iter (fun v -> Interop.setEventHandler <@ element.CanFocusChanged @> (fun _ -> v()) element)
        props |> Props.tryFind<DrawEventArgs->unit> "view.clearedViewport" |> Option.iter (fun v -> Interop.setEventHandler <@ element.ClearedViewport @> v element)
        props |> Props.tryFind<DrawEventArgs->unit> "view.clearingViewport" |> Option.iter (fun v -> Interop.setEventHandler <@ element.ClearingViewport @> v element)
        props |> Props.tryFind<CommandEventArgs->unit> "view.commandNotBound" |> Option.iter (fun v -> Interop.setEventHandler <@ element.CommandNotBound @> v element)
        props |> Props.tryFind<SizeChangedEventArgs->unit> "view.contentSizeChanged" |> Option.iter (fun v -> Interop.setEventHandler <@ element.ContentSizeChanged @> v element)
        props |> Props.tryFind<unit->unit> "view.disposing" |> Option.iter (fun v -> Interop.setEventHandler <@ element.Disposing @> (fun _ -> v()) element)
        props |> Props.tryFind<DrawEventArgs->unit> "view.drawComplete" |> Option.iter (fun v -> Interop.setEventHandler <@ element.DrawComplete @> v element)
        props |> Props.tryFind<DrawEventArgs->unit> "view.drawingContent" |> Option.iter (fun v -> Interop.setEventHandler <@ element.DrawingContent @> v element)
        props |> Props.tryFind<DrawEventArgs->unit> "view.drawingSubViews" |> Option.iter (fun v -> Interop.setEventHandler <@ element.DrawingSubViews @> v element)
        props |> Props.tryFind<DrawEventArgs->unit> "view.drawingText" |> Option.iter (fun v -> Interop.setEventHandler <@ element.DrawingText @> v element)
        props |> Props.tryFind<unit->unit> "view.enabledChanged" |> Option.iter (fun v -> Interop.setEventHandler <@ element.EnabledChanged @> (fun _ -> v()) element)
        props |> Props.tryFind<HasFocusEventArgs->unit> "view.focusedChanged" |> Option.iter (fun v -> Interop.setEventHandler <@ element.FocusedChanged @> v element)
        props |> Props.tryFind<EventArgs<Rectangle>->unit> "view.frameChanged" |> Option.iter (fun v -> Interop.setEventHandler <@ element.FrameChanged @> v element)
        props |> Props.tryFind<VisualRoleEventArgs->unit> "view.gettingAttributeForRole" |> Option.iter (fun v -> Interop.setEventHandler <@ element.GettingAttributeForRole @> v element)
        props |> Props.tryFind<ResultEventArgs<Scheme>->unit> "view.gettingScheme" |> Option.iter (fun v -> Interop.setEventHandler <@ element.GettingScheme @> v element)
        props |> Props.tryFind<CommandEventArgs->unit> "view.handlingHotKey" |> Option.iter (fun v -> Interop.setEventHandler <@ element.HandlingHotKey @> v element)
        props |> Props.tryFind<HasFocusEventArgs->unit> "view.hasFocusChanged" |> Option.iter (fun v -> Interop.setEventHandler <@ element.HasFocusChanged @> v element)
        props |> Props.tryFind<HasFocusEventArgs->unit> "view.hasFocusChanging" |> Option.iter (fun v -> Interop.setEventHandler <@ element.HasFocusChanging @> v element)
        props |> Props.tryFind<KeyChangedEventArgs->unit> "view.hotKeyChanged" |> Option.iter (fun v -> Interop.setEventHandler <@ element.HotKeyChanged @> v element)
        props |> Props.tryFind<unit->unit> "view.initialized" |> Option.iter (fun v -> Interop.setEventHandler <@ element.Initialized @> (fun _ -> v()) element)
        props |> Props.tryFind<Key->unit> "view.keyDown" |> Option.iter (fun v -> Interop.setEventHandler <@ element.KeyDown @> v element)
        props |> Props.tryFind<Key->unit> "view.keyDownNotHandled" |> Option.iter (fun v -> Interop.setEventHandler <@ element.KeyDownNotHandled @> v element)
        props |> Props.tryFind<Key->unit> "view.keyUp" |> Option.iter (fun v -> Interop.setEventHandler <@ element.KeyUp @> v element)
        props |> Props.tryFind<MouseEventArgs->unit> "view.mouseClick" |> Option.iter (fun v -> Interop.setEventHandler <@ element.MouseClick @> v element )
        props |> Props.tryFind<CancelEventArgs->unit> "view.mouseEnter" |> Option.iter (fun v -> Interop.setEventHandler <@ element.MouseEnter @> v element)
        props |> Props.tryFind<MouseEventArgs->unit> "view.mouseEvent" |> Option.iter (fun v -> Interop.setEventHandler <@ element.MouseEvent @> v element)
        props |> Props.tryFind<EventArgs->unit> "view.mouseLeave" |> Option.iter (fun v -> Interop.setEventHandler <@ element.MouseLeave @> v element)
        props |> Props.tryFind<EventArgs<MouseState>->unit> "view.mouseStateChanged" |> Option.iter (fun v -> Interop.setEventHandler <@ element.MouseStateChanged @> v element)
        props |> Props.tryFind<MouseEventArgs->unit> "view.mouseWheel" |> Option.iter (fun v -> Interop.setEventHandler <@ element.MouseWheel @> v element)
        props |> Props.tryFind<SuperViewChangedEventArgs->unit> "view.removed" |> Option.iter (fun v -> Interop.setEventHandler <@ element.Removed @> v element)
        props |> Props.tryFind<ValueChangedEventArgs<Scheme>->unit> "view.schemeChanged" |> Option.iter (fun v -> Interop.setEventHandler <@ element.SchemeChanged @> v element)
        props |> Props.tryFind<ValueChangingEventArgs<Scheme>->unit> "view.schemeChanging" |> Option.iter (fun v -> Interop.setEventHandler <@ element.SchemeChanging @> v element)
        props |> Props.tryFind<ValueChangedEventArgs<string>->unit> "view.schemeNameChanged" |> Option.iter (fun v -> Interop.setEventHandler <@ element.SchemeNameChanged @> v element)
        props |> Props.tryFind<ValueChangingEventArgs<string>->unit> "view.schemeNameChanging" |> Option.iter (fun v -> Interop.setEventHandler <@ element.SchemeNameChanging @> v element)
        props |> Props.tryFind<CommandEventArgs->unit> "view.selecting" |> Option.iter (fun v -> Interop.setEventHandler <@ element.Selecting @> v element)
        props |> Props.tryFind<SuperViewChangedEventArgs->unit> "view.subViewAdded" |> Option.iter (fun v -> Interop.setEventHandler <@ element.SubViewAdded @> v element)
        props |> Props.tryFind<LayoutEventArgs->unit> "view.subViewLayout" |> Option.iter (fun v -> Interop.setEventHandler <@ element.SubViewLayout @> v element)
        props |> Props.tryFind<SuperViewChangedEventArgs->unit> "view.subViewRemoved" |> Option.iter (fun v -> Interop.setEventHandler <@ element.SubViewRemoved @> v element)
        props |> Props.tryFind<LayoutEventArgs->unit> "view.subViewsLaidOut" |> Option.iter (fun v -> Interop.setEventHandler <@ element.SubViewsLaidOut @> v element)
        props |> Props.tryFind<SuperViewChangedEventArgs->unit> "view.superViewChanged" |> Option.iter (fun v -> Interop.setEventHandler <@ element.SuperViewChanged @> v element)
        props |> Props.tryFind<unit->unit> "view.textChanged" |> Option.iter (fun v -> Interop.setEventHandler <@ element.TextChanged @> (fun _ -> v()) element)
        props |> Props.tryFind<string->unit> "view.titleChanged" |> Option.iter (fun v -> Interop.setEventHandler <@ element.TitleChanged @> (fun arg -> v arg.Value) element)
        props |> Props.tryFind<App.CancelEventArgs<string>->unit> "view.titleChanging" |> Option.iter (fun v -> Interop.setEventHandler <@ element.TitleChanging @> v element)
        props |> Props.tryFind<DrawEventArgs->unit> "view.viewportChanged" |> Option.iter (fun v -> Interop.setEventHandler <@ element.ViewportChanged @> v element)
        props |> Props.tryFind<unit->unit> "view.visibleChanged" |> Option.iter (fun v -> Interop.setEventHandler <@ element.VisibleChanged @> (fun _ -> v()) element)
        props |> Props.tryFind<unit->unit> "view.visibleChanging" |> Option.iter (fun v -> Interop.setEventHandler <@ element.VisibleChanging @> (fun _ -> v()) element)


module ViewElement =

    let removeProps (element: View) (props: Props) =
        // Properties
        props |> Props.tryFind<View->unit> "ref" |> Option.iter (fun _ -> ())
        // Properties
        props |> Props.tryFind<ViewArrangement> "view.arrangement" |> Option.iter (fun _ -> element.Arrangement <- Unchecked.defaultof<_> )
        props |> Props.tryFind<LineStyle> "view.borderStyle" |> Option.iter (fun _ -> element.BorderStyle <- Unchecked.defaultof<_> )
        props |> Props.tryFind<bool> "view.canFocus" |> Option.iter (fun _ -> element.CanFocus <- Unchecked.defaultof<_> )
        props |> Props.tryFind<bool> "view.contentSizeTracksViewport" |> Option.iter (fun _ -> element.ContentSizeTracksViewport <- Unchecked.defaultof<_> )
        props |> Props.tryFind<CursorVisibility> "view.cursorVisibility" |> Option.iter (fun _ -> element.CursorVisibility <- Unchecked.defaultof<_> )
        props |> Props.tryFind<Object> "view.data" |> Option.iter (fun _ -> element.Data <- Unchecked.defaultof<_> )
        props |> Props.tryFind<bool> "view.enabled" |> Option.iter (fun _ -> element.Enabled <- Unchecked.defaultof<_> )
        props |> Props.tryFind<Rectangle> "view.frame" |> Option.iter (fun _ -> element.Frame <- Unchecked.defaultof<_> )
        props |> Props.tryFind<bool> "view.hasFocus" |> Option.iter (fun _ -> element.HasFocus <- Unchecked.defaultof<_> )
        props |> Props.tryFind<Dim> "view.height" |> Option.iter (fun _ -> element.Height <- Unchecked.defaultof<_> )
        props |> Props.tryFind<MouseState> "view.highlightStates" |> Option.iter (fun _ -> element.HighlightStates <- Unchecked.defaultof<_> )
        props |> Props.tryFind<Key> "view.hotKey" |> Option.iter (fun _ -> element.HotKey <- Unchecked.defaultof<_> )
        props |> Props.tryFind<Rune> "view.hotKeySpecifier" |> Option.iter (fun _ -> element.HotKeySpecifier <- Unchecked.defaultof<_> )
        props |> Props.tryFind<string> "view.id" |> Option.iter (fun _ -> element.Id <- Unchecked.defaultof<_> )
        props |> Props.tryFind<bool> "view.isInitialized" |> Option.iter (fun _ -> element.IsInitialized <- Unchecked.defaultof<_> )
        props |> Props.tryFind<IMouseHeldDown> "view.mouseHeldDown" |> Option.iter (fun _ -> element.MouseHeldDown <- Unchecked.defaultof<_> )
        props |> Props.tryFind<bool> "view.needsDraw" |> Option.iter (fun _ -> element.NeedsDraw <- Unchecked.defaultof<_> )
        props |> Props.tryFind<bool> "view.preserveTrailingSpaces" |> Option.iter (fun _ -> element.PreserveTrailingSpaces <- Unchecked.defaultof<_> )
        props |> Props.tryFind<string> "view.schemeName" |> Option.iter (fun _ -> element.SchemeName <- Unchecked.defaultof<_> )
        props |> Props.tryFind<ShadowStyle> "view.shadowStyle" |> Option.iter (fun _ -> element.ShadowStyle <- Unchecked.defaultof<_> )
        props |> Props.tryFind<bool> "view.superViewRendersLineCanvas" |> Option.iter (fun _ -> element.SuperViewRendersLineCanvas <- Unchecked.defaultof<_> )
        props |> Props.tryFind<TabBehavior option> "view.tabStop" |> Option.iter (fun _ -> element.TabStop <- Unchecked.defaultof<_>  |> Option.toNullable)
        props |> Props.tryFind<string> "view.text" |> Option.iter (fun _ -> element.Text <- Unchecked.defaultof<_> )
        props |> Props.tryFind<Alignment> "view.textAlignment" |> Option.iter (fun _ -> element.TextAlignment <- Unchecked.defaultof<_> )
        props |> Props.tryFind<TextDirection> "view.textDirection" |> Option.iter (fun _ -> element.TextDirection <- Unchecked.defaultof<_> )
        props |> Props.tryFind<string> "view.title" |> Option.iter (fun _ -> element.Title <- Unchecked.defaultof<_> )
        props |> Props.tryFind<bool> "view.validatePosDim" |> Option.iter (fun _ -> element.ValidatePosDim <- Unchecked.defaultof<_> )
        props |> Props.tryFind<Alignment> "view.verticalTextAlignment" |> Option.iter (fun _ -> element.VerticalTextAlignment <- Unchecked.defaultof<_> )
        props |> Props.tryFind<Rectangle> "view.viewport" |> Option.iter (fun _ -> element.Viewport <- Unchecked.defaultof<_> )
        props |> Props.tryFind<ViewportSettingsFlags> "view.viewportSettings" |> Option.iter (fun _ -> element.ViewportSettings <- Unchecked.defaultof<_> )
        props |> Props.tryFind<bool> "view.visible" |> Option.iter (fun _ -> element.Visible <- Unchecked.defaultof<_> )
        props |> Props.tryFind<bool> "view.wantContinuousButtonPressed" |> Option.iter (fun _ -> element.WantContinuousButtonPressed <- Unchecked.defaultof<_> )
        props |> Props.tryFind<bool> "view.wantMousePositionReports" |> Option.iter (fun _ -> element.WantMousePositionReports <- Unchecked.defaultof<_> )
        props |> Props.tryFind<Dim> "view.width" |> Option.iter (fun _ -> element.Width <- Unchecked.defaultof<_> )
        props |> Props.tryFind<Pos> "view.x" |> Option.iter (fun _ -> element.X <- Unchecked.defaultof<_> )
        props |> Props.tryFind<Pos> "view.y" |> Option.iter (fun _ -> element.Y <- Unchecked.defaultof<_> )
        // Events
        props |> Props.tryFind<HandledEventArgs->unit> "view.accepting" |> Option.iter (fun _ -> Interop.removeEventHandler <@ element.Accepting @> element)
        props |> Props.tryFind<AdvanceFocusEventArgs->unit> "view.advancingFocus" |> Option.iter (fun _ -> Interop.removeEventHandler <@ element.AdvancingFocus @> element)
        props |> Props.tryFind<EventArgs->unit> "view.borderStyleChanged" |> Option.iter (fun _ -> Interop.removeEventHandler <@ element.BorderStyleChanged @> element)
        props |> Props.tryFind<unit->unit> "view.canFocusChanged" |> Option.iter (fun _ -> Interop.removeEventHandler <@ element.CanFocusChanged @> element)
        props |> Props.tryFind<DrawEventArgs->unit> "view.clearedViewport" |> Option.iter (fun _ -> Interop.removeEventHandler <@ element.ClearedViewport @> element)
        props |> Props.tryFind<DrawEventArgs->unit> "view.clearingViewport" |> Option.iter (fun _ -> Interop.removeEventHandler <@ element.ClearingViewport @> element)
        props |> Props.tryFind<CommandEventArgs->unit> "view.commandNotBound" |> Option.iter (fun _ -> Interop.removeEventHandler <@ element.CommandNotBound @> element)
        props |> Props.tryFind<SizeChangedEventArgs->unit> "view.contentSizeChanged" |> Option.iter (fun _ -> Interop.removeEventHandler <@ element.ContentSizeChanged @> element)
        props |> Props.tryFind<unit->unit> "view.disposing" |> Option.iter (fun _ -> Interop.removeEventHandler <@ element.Disposing @> element)
        props |> Props.tryFind<DrawEventArgs->unit> "view.drawComplete" |> Option.iter (fun _ -> Interop.removeEventHandler <@ element.DrawComplete @> element)
        props |> Props.tryFind<DrawEventArgs->unit> "view.drawingContent" |> Option.iter (fun _ -> Interop.removeEventHandler <@ element.DrawingContent @> element)
        props |> Props.tryFind<DrawEventArgs->unit> "view.drawingSubViews" |> Option.iter (fun _ -> Interop.removeEventHandler <@ element.DrawingSubViews @> element)
        props |> Props.tryFind<DrawEventArgs->unit> "view.drawingText" |> Option.iter (fun _ -> Interop.removeEventHandler <@ element.DrawingText @> element)
        props |> Props.tryFind<unit->unit> "view.enabledChanged" |> Option.iter (fun _ -> Interop.removeEventHandler <@ element.EnabledChanged @> element)
        props |> Props.tryFind<HasFocusEventArgs->unit> "view.focusedChanged" |> Option.iter (fun _ -> Interop.removeEventHandler <@ element.FocusedChanged @> element)
        props |> Props.tryFind<EventArgs<Rectangle>->unit> "view.frameChanged" |> Option.iter (fun _ -> Interop.removeEventHandler <@ element.FrameChanged @> element)
        props |> Props.tryFind<VisualRoleEventArgs->unit> "view.gettingAttributeForRole" |> Option.iter (fun _ -> Interop.removeEventHandler <@ element.GettingAttributeForRole @> element)
        props |> Props.tryFind<ResultEventArgs<Scheme>->unit> "view.gettingScheme" |> Option.iter (fun _ -> Interop.removeEventHandler <@ element.GettingScheme @> element)
        props |> Props.tryFind<CommandEventArgs->unit> "view.handlingHotKey" |> Option.iter (fun _ -> Interop.removeEventHandler <@ element.HandlingHotKey @> element)
        props |> Props.tryFind<HasFocusEventArgs->unit> "view.hasFocusChanged" |> Option.iter (fun _ -> Interop.removeEventHandler <@ element.HasFocusChanged @> element)
        props |> Props.tryFind<HasFocusEventArgs->unit> "view.hasFocusChanging" |> Option.iter (fun _ -> Interop.removeEventHandler <@ element.HasFocusChanging @> element)
        props |> Props.tryFind<KeyChangedEventArgs->unit> "view.hotKeyChanged" |> Option.iter (fun _ -> Interop.removeEventHandler <@ element.HotKeyChanged @> element)
        props |> Props.tryFind<unit->unit> "view.initialized" |> Option.iter (fun _ -> Interop.removeEventHandler <@ element.Initialized @> element)
        props |> Props.tryFind<Key->unit> "view.keyDown" |> Option.iter (fun _ -> Interop.removeEventHandler <@ element.KeyDown @> element)
        props |> Props.tryFind<Key->unit> "view.keyDownNotHandled" |> Option.iter (fun _ -> Interop.removeEventHandler <@ element.KeyDownNotHandled @> element)
        props |> Props.tryFind<Key->unit> "view.keyUp" |> Option.iter (fun _ -> Interop.removeEventHandler <@ element.KeyUp @> element)
        props |> Props.tryFind<MouseEventArgs->unit> "view.mouseClick" |> Option.iter (fun _ -> Interop.removeEventHandler <@ element.MouseClick @> element)
        props |> Props.tryFind<CancelEventArgs->unit> "view.mouseEnter" |> Option.iter (fun _ -> Interop.removeEventHandler <@ element.MouseEnter @> element)
        props |> Props.tryFind<MouseEventArgs->unit> "view.mouseEvent" |> Option.iter (fun _ -> Interop.removeEventHandler <@ element.MouseEvent @> element)
        props |> Props.tryFind<EventArgs->unit> "view.mouseLeave" |> Option.iter (fun _ -> Interop.removeEventHandler <@ element.MouseLeave @> element)
        props |> Props.tryFind<EventArgs<MouseState>->unit> "view.mouseStateChanged" |> Option.iter (fun _ -> Interop.removeEventHandler <@ element.MouseStateChanged @> element)
        props |> Props.tryFind<MouseEventArgs->unit> "view.mouseWheel" |> Option.iter (fun _ -> Interop.removeEventHandler <@ element.MouseWheel @> element)
        props |> Props.tryFind<SuperViewChangedEventArgs->unit> "view.removed" |> Option.iter (fun _ -> Interop.removeEventHandler <@ element.Removed @> element)
        props |> Props.tryFind<ValueChangedEventArgs<Scheme>->unit> "view.schemeChanged" |> Option.iter (fun _ -> Interop.removeEventHandler <@ element.SchemeChanged @> element)
        props |> Props.tryFind<ValueChangingEventArgs<Scheme>->unit> "view.schemeChanging" |> Option.iter (fun _ -> Interop.removeEventHandler <@ element.SchemeChanging @> element)
        props |> Props.tryFind<ValueChangedEventArgs<string>->unit> "view.schemeNameChanged" |> Option.iter (fun _ -> Interop.removeEventHandler <@ element.SchemeNameChanged @> element)
        props |> Props.tryFind<ValueChangingEventArgs<string>->unit> "view.schemeNameChanging" |> Option.iter (fun _ -> Interop.removeEventHandler <@ element.SchemeNameChanging @> element)
        props |> Props.tryFind<CommandEventArgs->unit> "view.selecting" |> Option.iter (fun _ -> Interop.removeEventHandler <@ element.Selecting @> element)
        props |> Props.tryFind<SuperViewChangedEventArgs->unit> "view.subViewAdded" |> Option.iter (fun _ -> Interop.removeEventHandler <@ element.SubViewAdded @> element)
        props |> Props.tryFind<LayoutEventArgs->unit> "view.subViewLayout" |> Option.iter (fun _ -> Interop.removeEventHandler <@ element.SubViewLayout @> element)
        props |> Props.tryFind<SuperViewChangedEventArgs->unit> "view.subViewRemoved" |> Option.iter (fun _ -> Interop.removeEventHandler <@ element.SubViewRemoved @> element)
        props |> Props.tryFind<LayoutEventArgs->unit> "view.subViewsLaidOut" |> Option.iter (fun _ -> Interop.removeEventHandler <@ element.SubViewsLaidOut @> element)
        props |> Props.tryFind<SuperViewChangedEventArgs->unit> "view.superViewChanged" |> Option.iter (fun _ -> Interop.removeEventHandler <@ element.SuperViewChanged @> element)
        props |> Props.tryFind<unit->unit> "view.textChanged" |> Option.iter (fun _ -> Interop.removeEventHandler <@ element.TextChanged @> element)
        props |> Props.tryFind<string->unit> "view.titleChanged" |> Option.iter (fun _ -> Interop.removeEventHandler <@ element.TitleChanged @> element)
        props |> Props.tryFind<App.CancelEventArgs<string>->unit> "view.titleChanging" |> Option.iter (fun _ -> Interop.removeEventHandler <@ element.TitleChanging @> element)
        props |> Props.tryFind<DrawEventArgs->unit> "view.viewportChanged" |> Option.iter (fun _ -> Interop.removeEventHandler <@ element.ViewportChanged @> element)
        props |> Props.tryFind<unit->unit> "view.visibleChanged" |> Option.iter (fun _ -> Interop.removeEventHandler <@ element.VisibleChanged @> element)
        props |> Props.tryFind<unit->unit> "view.visibleChanging" |> Option.iter (fun _ -> Interop.removeEventHandler <@ element.VisibleChanging @> element)

    let canUpdate (view:View) (props: Props) (removedProps: Props) =
        let isPosCompatible (a:Pos) (b:Pos) =
            let nameA = a.GetType().Name
            let nameB = b.GetType().Name
            nameA = nameB ||
            (nameA = "PosAbsolute" && nameB = "PosAbsolute") ||
            (nameA <> "PosAbsolute" && nameB <> "PosAbsolute")

        let isDimCompatible (a:Dim) (b:Dim) =
            let nameA = a.GetType().Name
            let nameB = b.GetType().Name
            nameA = nameB ||
            (nameA = "DimAbsolute" && nameB = "DimAbsolute") ||
            (nameA <> "DimAbsolute" && nameB <> "DimAbsolute")


        let positionX = props |> Props.tryFind<Pos> "view.x"      |> Option.map (fun v -> isPosCompatible view.X v) |> Option.defaultValue true
        let positionY = props |> Props.tryFind<Pos> "view.y"      |> Option.map (fun v -> isPosCompatible view.Y v) |> Option.defaultValue true
        let width = props |> Props.tryFind<Dim> "view.width"      |> Option.map (fun v -> isDimCompatible view.Width v) |> Option.defaultValue true
        let height = props |> Props.tryFind<Dim> "view.height"      |> Option.map (fun v -> isDimCompatible view.Height v) |> Option.defaultValue true

        // in case width or height is removed!
        let widthNotRemoved  = removedProps |> Props.keyExists "view.width"   |> not
        let heightNotRemoved = removedProps |> Props.keyExists "view.height"  |> not

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
type AdornmentElement(props:Props) =
    inherit TerminalElement(props)

    let removeProps (element: Adornment) (props: Props) =
        // Properties
        props |> Props.tryFind<ViewDiagnosticFlags> "adornment.diagnostics" |> Option.iter (fun _ -> element.Diagnostics <- Unchecked.defaultof<_> )
        props |> Props.tryFind<bool> "adornment.superViewRendersLineCanvas" |> Option.iter (fun _ -> element.SuperViewRendersLineCanvas <- Unchecked.defaultof<_>)
        props |> Props.tryFind<Thickness> "adornment.thickness" |> Option.iter (fun _ -> element.Thickness <- Unchecked.defaultof<_>)
        props |> Props.tryFind<Rectangle> "adornment.viewport" |> Option.iter (fun _ -> element.Viewport <- Unchecked.defaultof<_>)
        // Events
        props |> Props.tryFind<unit->unit> "adornment.thicknessChanged" |> Option.iter (fun _ -> Interop.removeEventHandler <@ element.ThicknessChanged @> element)

    override _.name = $"Adornment"

    member this.setProps (element: Adornment, props: Props) =
        base.setProps(element, props)

        // Properties
        props |> Props.tryFind<ViewDiagnosticFlags> "adornment.diagnostics" |> Option.iter (fun v -> element.Diagnostics <- v )
        props |> Props.tryFind<bool> "adornment.superViewRendersLineCanvas" |> Option.iter (fun v -> element.SuperViewRendersLineCanvas <- v )
        props |> Props.tryFind<Thickness> "adornment.thickness" |> Option.iter (fun v -> element.Thickness <- v )
        props |> Props.tryFind<Rectangle> "adornment.viewport" |> Option.iter (fun v -> element.Viewport <- v )
        // Events
        props |> Props.tryFind<unit->unit> "adornment.thicknessChanged" |> Option.iter (fun v -> Interop.setEventHandler <@ element.ThicknessChanged @> (fun _ -> v()) element)

    override this.initialize parent =
        #if DEBUG
        Diagnostics.Trace.WriteLine $"{this.name} created!"
        #endif
        this.parent <- parent


        let el = new Adornment()
        parent |> Option.iter (fun p -> p.Add el |> ignore)
        this.setProps(el, props)
        props |> Props.tryFind<View->unit> "ref" |> Option.iter (fun v -> v el)
        this.element <- el



    override this.canUpdate prevElement oldProps =
        let changedProps,removedProps = Interop.filterProps oldProps props
        let canUpdateView = ViewElement.canUpdate prevElement changedProps removedProps
        let canUpdateElement =
            true

        canUpdateView && canUpdateElement

    override this.update prevElement oldProps =
        let element = prevElement :?> Adornment
        let changedProps,removedProps = Interop.filterProps oldProps props
        ViewElement.removeProps prevElement removedProps
        removeProps element removedProps
        this.setProps(element, changedProps)
        this.element <- prevElement

// Bar
type BarElement(props:Props) =
    inherit TerminalElement(props)

    let removeProps (element: Bar) (props: Props) =
        // Properties
        props |> Props.tryFind<AlignmentModes> "bar.alignmentModes" |> Option.iter (fun _ -> element.AlignmentModes <- Unchecked.defaultof<_>)
        props |> Props.tryFind<Orientation> "bar.orientation" |> Option.iter (fun _ -> element.Orientation <- Unchecked.defaultof<_>)
        // Events
        props |> Props.tryFind<Orientation->unit> "bar.orientationChanged" |> Option.iter (fun _ -> Interop.removeEventHandler <@ element.OrientationChanged @> element)
        props |> Props.tryFind<App.CancelEventArgs<Orientation>->unit> "bar.orientationChanging" |> Option.iter (fun _ -> Interop.removeEventHandler <@ element.OrientationChanging @> element)

    override _.name = $"Bar"

    member _.setProps (element: Bar, props: Props) =
        base.setProps(element, props)

        // Properties
        props |> Props.tryFind<AlignmentModes> "bar.alignmentModes" |> Option.iter (fun v -> element.AlignmentModes <- v )
        props |> Props.tryFind<Orientation> "bar.orientation" |> Option.iter (fun v -> element.Orientation <- v )
        // Events
        props |> Props.tryFind<Orientation->unit> "bar.orientationChanged" |> Option.iter (fun v -> Interop.setEventHandler <@ element.OrientationChanged @> (fun arg -> v arg.Value) element)
        props |> Props.tryFind<App.CancelEventArgs<Orientation>->unit> "bar.orientationChanging" |> Option.iter (fun v -> Interop.setEventHandler <@ element.OrientationChanging @> v element)


    override this.initialize parent =
        #if DEBUG
        Diagnostics.Trace.WriteLine $"{this.name} created!"
        #endif
        this.parent <- parent


        let el = new Bar()
        parent |> Option.iter (fun p -> p.Add el |> ignore)
        this.setProps(el, props)
        props |> Props.tryFind<View->unit> "ref" |> Option.iter (fun v -> v el)
        this.element <- el



    override this.canUpdate prevElement oldProps =
        let changedProps,removedProps = Interop.filterProps oldProps props
        let canUpdateView = ViewElement.canUpdate prevElement changedProps removedProps
        let canUpdateElement =
            true

        canUpdateView && canUpdateElement



    override this.update prevElement oldProps =
        let element = prevElement :?> Bar
        let changedProps,removedProps = Interop.filterProps oldProps props
        ViewElement.removeProps prevElement removedProps
        removeProps element removedProps
        this.setProps(element, changedProps)
        this.element <- prevElement



// Border
type BorderElement(props:Props) =
    inherit TerminalElement(props)

    let removeProps (element: Border) (props: Props) =
        // Properties
        props |> Props.tryFind<LineStyle> "border.lineStyle" |> Option.iter (fun _ -> element.LineStyle <- Unchecked.defaultof<_>)
        props |> Props.tryFind<BorderSettings> "border.settings" |> Option.iter (fun _ -> element.Settings <- Unchecked.defaultof<_>)

    override _.name = $"Border"

    member _.setProps (element: Border, props: Props) =
        base.setProps(element, props)

        // Properties
        props |> Props.tryFind<LineStyle> "border.lineStyle" |> Option.iter (fun v -> element.LineStyle <- v )
        props |> Props.tryFind<BorderSettings> "border.settings" |> Option.iter (fun v -> element.Settings <- v )


    override this.initialize parent =
        #if DEBUG
        Diagnostics.Trace.WriteLine $"{this.name} created!"
        #endif
        this.parent <- parent


        let el = new Border()
        parent |> Option.iter (fun p -> p.Add el |> ignore)
        this.setProps(el, props)
        props |> Props.tryFind<View->unit> "ref" |> Option.iter (fun v -> v el)
        this.element <- el



    override this.canUpdate prevElement oldProps =
        let changedProps,removedProps = Interop.filterProps oldProps props
        let canUpdateView = ViewElement.canUpdate prevElement changedProps removedProps
        let canUpdateElement =
            true

        canUpdateView && canUpdateElement



    override this.update prevElement oldProps =
        let element = prevElement :?> Border
        let changedProps,removedProps = Interop.filterProps oldProps props
        ViewElement.removeProps prevElement removedProps
        removeProps element removedProps
        this.setProps(element, changedProps)
        this.element <- prevElement



// Button
type ButtonElement(props:Props) =
    inherit TerminalElement(props)

    let removeProps (element: Button) (props: Props) =
        // Properties
        props |> Props.tryFind<Rune> "button.hotKeySpecifier" |> Option.iter (fun _ -> element.HotKeySpecifier <- Unchecked.defaultof<_>)
        props |> Props.tryFind<bool> "button.isDefault" |> Option.iter (fun _ -> element.IsDefault <- Unchecked.defaultof<_>)
        props |> Props.tryFind<bool> "button.noDecorations" |> Option.iter (fun _ -> element.NoDecorations <- Unchecked.defaultof<_>)
        props |> Props.tryFind<bool> "button.noPadding" |> Option.iter (fun _ -> element.NoPadding <- Unchecked.defaultof<_>)
        props |> Props.tryFind<string> "button.text" |> Option.iter (fun _ -> element.Text <- Unchecked.defaultof<_>)
        // Events
        props |> Props.tryFind<bool> "button.wantContinuousButtonPressed" |> Option.iter (fun _ -> element.WantContinuousButtonPressed <- Unchecked.defaultof<_>)

    override _.name = $"Button"

    member _.setProps (element: Button, props: Props) =
        base.setProps(element, props)

        // Properties
        props |> Props.tryFind<Rune> "button.hotKeySpecifier" |> Option.iter (fun v -> element.HotKeySpecifier <- v )
        props |> Props.tryFind<bool> "button.isDefault" |> Option.iter (fun v -> element.IsDefault <- v )
        props |> Props.tryFind<bool> "button.noDecorations" |> Option.iter (fun v -> element.NoDecorations <- v )
        props |> Props.tryFind<bool> "button.noPadding" |> Option.iter (fun v -> element.NoPadding <- v )
        props |> Props.tryFind<string> "button.text" |> Option.iter (fun v -> element.Text <- v )
        // Events
        props |> Props.tryFind<bool> "button.wantContinuousButtonPressed" |> Option.iter (fun v -> element.WantContinuousButtonPressed <- v )


    override this.initialize parent =
        #if DEBUG
        Diagnostics.Trace.WriteLine $"{this.name} created!"
        #endif
        this.parent <- parent


        let el = new Button()
        parent |> Option.iter (fun p -> p.Add el |> ignore)
        this.setProps(el, props)
        props |> Props.tryFind<View->unit> "ref" |> Option.iter (fun v -> v el)
        this.element <- el



    override this.canUpdate prevElement oldProps =
        let changedProps,removedProps = Interop.filterProps oldProps props
        let canUpdateView = ViewElement.canUpdate prevElement changedProps removedProps
        let canUpdateElement =
            true

        canUpdateView && canUpdateElement



    override this.update prevElement oldProps =
        let element = prevElement :?> Button
        let changedProps,removedProps = Interop.filterProps oldProps props
        ViewElement.removeProps prevElement removedProps
        removeProps element removedProps
        this.setProps(element, changedProps)
        this.element <- prevElement



// CheckBox
type CheckBoxElement(props:Props) =
    inherit TerminalElement(props)

    let removeProps (element: CheckBox) (props: Props) =
        // Properties
        props |> Props.tryFind<bool> "checkBox.allowCheckStateNone" |> Option.iter (fun _ -> element.AllowCheckStateNone <- Unchecked.defaultof<_>)
        props |> Props.tryFind<CheckState> "checkBox.checkedState" |> Option.iter (fun _ -> element.CheckedState <- Unchecked.defaultof<_>)
        props |> Props.tryFind<Rune> "checkBox.hotKeySpecifier" |> Option.iter (fun _ -> element.HotKeySpecifier <- Unchecked.defaultof<_>)
        props |> Props.tryFind<bool> "checkBox.radioStyle" |> Option.iter (fun _ -> element.RadioStyle <- Unchecked.defaultof<_>)
        props |> Props.tryFind<string> "checkBox.text" |> Option.iter (fun _ -> element.Text <- Unchecked.defaultof<_>)
        // Events
        props |> Props.tryFind<ResultEventArgs<CheckState>->unit> "checkBox.checkedStateChanging" |> Option.iter (fun _ -> Interop.removeEventHandler <@ element.CheckedStateChanging @> element)

    override _.name = $"CheckBox"

    member _.setProps (element: CheckBox, props: Props) =
        base.setProps(element, props)

        // Properties
        props |> Props.tryFind<bool> "checkBox.allowCheckStateNone" |> Option.iter (fun v -> element.AllowCheckStateNone <- v )
        props |> Props.tryFind<CheckState> "checkBox.checkedState" |> Option.iter (fun v -> element.CheckedState <- v )
        props |> Props.tryFind<Rune> "checkBox.hotKeySpecifier" |> Option.iter (fun v -> element.HotKeySpecifier <- v )
        props |> Props.tryFind<bool> "checkBox.radioStyle" |> Option.iter (fun v -> element.RadioStyle <- v )
        props |> Props.tryFind<string> "checkBox.text" |> Option.iter (fun v -> element.Text <- v )
        // Events
        props |> Props.tryFind<ResultEventArgs<CheckState>->unit> "checkBox.checkedStateChanging" |> Option.iter (fun v -> Interop.setEventHandler <@ element.CheckedStateChanging @> v element)


    override this.initialize parent =
        #if DEBUG
        Diagnostics.Trace.WriteLine $"{this.name} created!"
        #endif
        this.parent <- parent


        let el = new CheckBox()
        parent |> Option.iter (fun p -> p.Add el |> ignore)
        this.setProps(el, props)
        props |> Props.tryFind<View->unit> "ref" |> Option.iter (fun v -> v el)
        this.element <- el



    override this.canUpdate prevElement oldProps =
        let changedProps,removedProps = Interop.filterProps oldProps props
        let canUpdateView = ViewElement.canUpdate prevElement changedProps removedProps
        let canUpdateElement =
            true

        canUpdateView && canUpdateElement



    override this.update prevElement oldProps =
        let element = prevElement :?> CheckBox
        let changedProps,removedProps = Interop.filterProps oldProps props
        ViewElement.removeProps prevElement removedProps
        removeProps element removedProps
        this.setProps(element, changedProps)
        this.element <- prevElement



// ColorPicker
type ColorPickerElement(props:Props) =
    inherit TerminalElement(props)

    let removeProps (element: ColorPicker) (props: Props) =
        // Properties
        props |> Props.tryFind<Terminal.Gui.Drawing.Color> "colorPicker.selectedColor" |> Option.iter (fun _ -> element.SelectedColor <- Unchecked.defaultof<_>)
        props |> Props.tryFind<ColorPickerStyle> "colorPicker.style" |> Option.iter (fun _ -> element.Style <- Unchecked.defaultof<_>)
        // Events
        props |> Props.tryFind<ResultEventArgs<Terminal.Gui.Drawing.Color>->unit> "colorPicker.colorChanged" |> Option.iter (fun _ -> Interop.removeEventHandler <@ element.ColorChanged @> element)

    override _.name = $"ColorPicker"

    member _.setProps (element: ColorPicker, props: Props) =
        base.setProps(element, props)

        // Properties
        props |> Props.tryFind<Terminal.Gui.Drawing.Color> "colorPicker.selectedColor" |> Option.iter (fun v -> element.SelectedColor <- v )
        props |> Props.tryFind<ColorPickerStyle> "colorPicker.style" |> Option.iter (fun v -> element.Style <- v )
        // Events
        props |> Props.tryFind<ResultEventArgs<Terminal.Gui.Drawing.Color>->unit> "colorPicker.colorChanged" |> Option.iter (fun v -> Interop.setEventHandler <@ element.ColorChanged @> v element)


    override this.initialize parent =
        #if DEBUG
        Diagnostics.Trace.WriteLine $"{this.name} created!"
        #endif
        this.parent <- parent


        let el = new ColorPicker()
        parent |> Option.iter (fun p -> p.Add el |> ignore)
        this.setProps(el, props)
        props |> Props.tryFind<View->unit> "ref" |> Option.iter (fun v -> v el)
        this.element <- el



    override this.canUpdate prevElement oldProps =
        let changedProps,removedProps = Interop.filterProps oldProps props
        let canUpdateView = ViewElement.canUpdate prevElement changedProps removedProps
        let canUpdateElement =
            true

        canUpdateView && canUpdateElement



    override this.update prevElement oldProps =
        let element = prevElement :?> ColorPicker
        let changedProps,removedProps = Interop.filterProps oldProps props
        ViewElement.removeProps prevElement removedProps
        removeProps element removedProps
        this.setProps(element, changedProps)
        this.element <- prevElement



// ColorPicker16
type ColorPicker16Element(props:Props) =
    inherit TerminalElement(props)

    let removeProps (element: ColorPicker16) (props: Props) =
        // Properties
        props |> Props.tryFind<Int32> "colorPicker16.boxHeight" |> Option.iter (fun _ -> element.BoxHeight <- Unchecked.defaultof<_>)
        props |> Props.tryFind<Int32> "colorPicker16.boxWidth" |> Option.iter (fun _ -> element.BoxWidth <- Unchecked.defaultof<_>)
        props |> Props.tryFind<Point> "colorPicker16.cursor" |> Option.iter (fun _ -> element.Cursor <- Unchecked.defaultof<_>)
        props |> Props.tryFind<ColorName16> "colorPicker16.selectedColor" |> Option.iter (fun _ -> element.SelectedColor <- Unchecked.defaultof<_>)
        // Events
        props |> Props.tryFind<ResultEventArgs<Terminal.Gui.Drawing.Color>->unit> "colorPicker16.colorChanged" |> Option.iter (fun _ -> Interop.removeEventHandler <@ element.ColorChanged @> element)

    override _.name = $"ColorPicker16"

    member _.setProps (element: ColorPicker16, props: Props) =
        base.setProps(element, props)

        // Properties
        props |> Props.tryFind<Int32> "colorPicker16.boxHeight" |> Option.iter (fun v -> element.BoxHeight <- v )
        props |> Props.tryFind<Int32> "colorPicker16.boxWidth" |> Option.iter (fun v -> element.BoxWidth <- v )
        props |> Props.tryFind<Point> "colorPicker16.cursor" |> Option.iter (fun v -> element.Cursor <- v )
        props |> Props.tryFind<ColorName16> "colorPicker16.selectedColor" |> Option.iter (fun v -> element.SelectedColor <- v )
        // Events
        props |> Props.tryFind<ResultEventArgs<Terminal.Gui.Drawing.Color>->unit> "colorPicker16.colorChanged" |> Option.iter (fun v -> Interop.setEventHandler <@ element.ColorChanged @> v element)


    override this.initialize parent =
        #if DEBUG
        Diagnostics.Trace.WriteLine $"{this.name} created!"
        #endif
        this.parent <- parent


        let el = new ColorPicker16()
        parent |> Option.iter (fun p -> p.Add el |> ignore)
        this.setProps(el, props)
        props |> Props.tryFind<View->unit> "ref" |> Option.iter (fun v -> v el)
        this.element <- el



    override this.canUpdate prevElement oldProps =
        let changedProps,removedProps = Interop.filterProps oldProps props
        let canUpdateView = ViewElement.canUpdate prevElement changedProps removedProps
        let canUpdateElement =
            true

        canUpdateView && canUpdateElement



    override this.update prevElement oldProps =
        let element = prevElement :?> ColorPicker16
        let changedProps,removedProps = Interop.filterProps oldProps props
        ViewElement.removeProps prevElement removedProps
        removeProps element removedProps
        this.setProps(element, changedProps)
        this.element <- prevElement



// ComboBox
type ComboBoxElement(props:Props) =
    inherit TerminalElement(props)

    let removeProps (element: ComboBox) (props: Props) =
        // Properties
        props |> Props.tryFind<bool> "comboBox.hideDropdownListOnClick" |> Option.iter (fun _ -> element.HideDropdownListOnClick <- Unchecked.defaultof<_>)
        props |> Props.tryFind<bool> "comboBox.readOnly" |> Option.iter (fun _ -> element.ReadOnly <- Unchecked.defaultof<_>)
        props |> Props.tryFind<string> "comboBox.searchText" |> Option.iter (fun _ -> element.SearchText <- Unchecked.defaultof<_>)
        props |> Props.tryFind<Int32> "comboBox.selectedItem" |> Option.iter (fun _ -> element.SelectedItem <- Unchecked.defaultof<_>)
        props |> Props.tryFind<string list> "comboBox.source" |> Option.iter (fun _ -> element.SetSource Unchecked.defaultof<_>)
        props |> Props.tryFind<string> "comboBox.text" |> Option.iter (fun _ -> element.Text <- Unchecked.defaultof<_>)
        // Events
        props |> Props.tryFind<unit->unit> "comboBox.collapsed" |> Option.iter (fun _ -> Interop.removeEventHandler <@ element.Collapsed @> element)
        props |> Props.tryFind<unit->unit> "comboBox.expanded" |> Option.iter (fun _ -> Interop.removeEventHandler <@ element.Expanded @> element)
        props |> Props.tryFind<ListViewItemEventArgs->unit> "comboBox.openSelectedItem" |> Option.iter (fun _ -> Interop.removeEventHandler <@ element.OpenSelectedItem @> element)
        props |> Props.tryFind<ListViewItemEventArgs->unit> "comboBox.selectedItemChanged" |> Option.iter (fun _ -> Interop.removeEventHandler <@ element.SelectedItemChanged @> element)

    override _.name = $"ComboBox"

    member _.setProps (element: ComboBox, props: Props) =
        base.setProps(element, props)

        // Properties
        props |> Props.tryFind<bool> "comboBox.hideDropdownListOnClick" |> Option.iter (fun v -> element.HideDropdownListOnClick <- v )
        props |> Props.tryFind<bool> "comboBox.readOnly" |> Option.iter (fun v -> element.ReadOnly <- v )
        props |> Props.tryFind<string> "comboBox.searchText" |> Option.iter (fun v -> element.SearchText <- v )
        props |> Props.tryFind<Int32> "comboBox.selectedItem" |> Option.iter (fun v -> element.SelectedItem <- v )
        props |> Props.tryFind<string list> "comboBox.source" |> Option.iter (fun v -> element.SetSource (ObservableCollection(v)))
        props |> Props.tryFind<string> "comboBox.text" |> Option.iter (fun v -> element.Text <- v )
        // Events
        props |> Props.tryFind<unit->unit> "comboBox.collapsed" |> Option.iter (fun v -> Interop.setEventHandler <@ element.Collapsed @> (fun _ -> v()) element)
        props |> Props.tryFind<unit->unit> "comboBox.expanded" |> Option.iter (fun v -> Interop.setEventHandler <@ element.Expanded @> (fun _ -> v()) element)
        props |> Props.tryFind<ListViewItemEventArgs->unit> "comboBox.openSelectedItem" |> Option.iter (fun v -> Interop.setEventHandler <@ element.OpenSelectedItem @> v element)
        props |> Props.tryFind<ListViewItemEventArgs->unit> "comboBox.selectedItemChanged" |> Option.iter (fun v -> Interop.setEventHandler <@ element.SelectedItemChanged @> v element)


    override this.initialize parent =
        #if DEBUG
        Diagnostics.Trace.WriteLine $"{this.name} created!"
        #endif
        this.parent <- parent


        let el = new ComboBox()
        parent |> Option.iter (fun p -> p.Add el |> ignore)
        this.setProps(el, props)
        props |> Props.tryFind<View->unit> "ref" |> Option.iter (fun v -> v el)
        this.element <- el



    override this.canUpdate prevElement oldProps =
        let changedProps,removedProps = Interop.filterProps oldProps props
        let canUpdateView = ViewElement.canUpdate prevElement changedProps removedProps
        let canUpdateElement =
            true

        canUpdateView && canUpdateElement



    override this.update prevElement oldProps =
        let element = prevElement :?> ComboBox
        let changedProps,removedProps = Interop.filterProps oldProps props
        ViewElement.removeProps prevElement removedProps
        removeProps element removedProps
        this.setProps(element, changedProps)
        this.element <- prevElement



// DateField
type DateFieldElement(props:Props) =
    inherit TerminalElement(props)

    let removeProps (element: DateField) (props: Props) =
        // Properties
        props |> Props.tryFind<CultureInfo> "dateField.culture" |> Option.iter (fun _ -> element.Culture <- Unchecked.defaultof<_>)
        props |> Props.tryFind<Int32> "dateField.cursorPosition" |> Option.iter (fun _ -> element.CursorPosition <- Unchecked.defaultof<_>)
        props |> Props.tryFind<DateTime> "dateField.date" |> Option.iter (fun _ -> element.Date <- Unchecked.defaultof<_>)
        // Events
        props |> Props.tryFind<DateTimeEventArgs<DateTime>->unit> "dateField.dateChanged" |> Option.iter (fun _ -> Interop.removeEventHandler <@ element.DateChanged @> element)

    override _.name = $"DateField"

    member _.setProps (element: DateField, props: Props) =
        base.setProps(element, props)

        // Properties
        props |> Props.tryFind<CultureInfo> "dateField.culture" |> Option.iter (fun v -> element.Culture <- v )
        props |> Props.tryFind<Int32> "dateField.cursorPosition" |> Option.iter (fun v -> element.CursorPosition <- v )
        props |> Props.tryFind<DateTime> "dateField.date" |> Option.iter (fun v -> element.Date <- v )
        // Events
        props |> Props.tryFind<DateTimeEventArgs<DateTime>->unit> "dateField.dateChanged" |> Option.iter (fun v -> Interop.setEventHandler <@ element.DateChanged @> v element)


    override this.initialize parent =
        #if DEBUG
        Diagnostics.Trace.WriteLine $"{this.name} created!"
        #endif
        this.parent <- parent


        let el = new DateField()
        parent |> Option.iter (fun p -> p.Add el |> ignore)
        this.setProps(el, props)
        props |> Props.tryFind<View->unit> "ref" |> Option.iter (fun v -> v el)
        this.element <- el



    override this.canUpdate prevElement oldProps =
        let changedProps,removedProps = Interop.filterProps oldProps props
        let canUpdateView = ViewElement.canUpdate prevElement changedProps removedProps
        let canUpdateElement =
            true

        canUpdateView && canUpdateElement



    override this.update prevElement oldProps =
        let element = prevElement :?> DateField
        let changedProps,removedProps = Interop.filterProps oldProps props
        ViewElement.removeProps prevElement removedProps
        removeProps element removedProps
        this.setProps(element, changedProps)
        this.element <- prevElement



// DatePicker
type DatePickerElement(props:Props) =
    inherit TerminalElement(props)

    let removeProps (element: DatePicker) (props: Props) =
        // Properties
        props |> Props.tryFind<CultureInfo> "datePicker.culture" |> Option.iter (fun _ -> element.Culture <- Unchecked.defaultof<_>)
        props |> Props.tryFind<DateTime> "datePicker.date" |> Option.iter (fun _ -> element.Date <- Unchecked.defaultof<_>)

    override _.name = $"DatePicker"

    member _.setProps (element: DatePicker, props: Props) =
        base.setProps(element, props)

        // Properties
        props |> Props.tryFind<CultureInfo> "datePicker.culture" |> Option.iter (fun v -> element.Culture <- v )
        props |> Props.tryFind<DateTime> "datePicker.date" |> Option.iter (fun v -> element.Date <- v )


    override this.initialize parent =
        #if DEBUG
        Diagnostics.Trace.WriteLine $"{this.name} created!"
        #endif
        this.parent <- parent


        let el = new DatePicker()
        parent |> Option.iter (fun p -> p.Add el |> ignore)
        this.setProps(el, props)
        props |> Props.tryFind<View->unit> "ref" |> Option.iter (fun v -> v el)
        this.element <- el



    override this.canUpdate prevElement oldProps =
        let changedProps,removedProps = Interop.filterProps oldProps props
        let canUpdateView = ViewElement.canUpdate prevElement changedProps removedProps
        let canUpdateElement =
            true

        canUpdateView && canUpdateElement



    override this.update prevElement oldProps =
        let element = prevElement :?> DatePicker
        let changedProps,removedProps = Interop.filterProps oldProps props
        ViewElement.removeProps prevElement removedProps
        removeProps element removedProps
        this.setProps(element, changedProps)
        this.element <- prevElement



// Dialog
type DialogElement(props:Props) =
    inherit TerminalElement(props)

    let removeProps (element: Dialog) (props: Props) =
        // Properties
        props |> Props.tryFind<Alignment> "dialog.buttonAlignment" |> Option.iter (fun _ -> element.ButtonAlignment <- Unchecked.defaultof<_>)
        props |> Props.tryFind<AlignmentModes> "dialog.buttonAlignmentModes" |> Option.iter (fun _ -> element.ButtonAlignmentModes <- Unchecked.defaultof<_>)
        props |> Props.tryFind<bool> "dialog.canceled" |> Option.iter (fun _ -> element.Canceled <- Unchecked.defaultof<_>)

    override _.name = $"Dialog"

    member _.setProps (element: Dialog, props: Props) =
        base.setProps(element, props)

        // Properties
        props |> Props.tryFind<Alignment> "dialog.buttonAlignment" |> Option.iter (fun v -> element.ButtonAlignment <- v )
        props |> Props.tryFind<AlignmentModes> "dialog.buttonAlignmentModes" |> Option.iter (fun v -> element.ButtonAlignmentModes <- v )
        props |> Props.tryFind<bool> "dialog.canceled" |> Option.iter (fun v -> element.Canceled <- v )


    override this.initialize parent =
        #if DEBUG
        Diagnostics.Trace.WriteLine $"{this.name} created!"
        #endif
        this.parent <- parent


        let el = new Dialog()
        parent |> Option.iter (fun p -> p.Add el |> ignore)
        this.setProps(el, props)
        props |> Props.tryFind<View->unit> "ref" |> Option.iter (fun v -> v el)
        this.element <- el



    override this.canUpdate prevElement oldProps =
        let changedProps,removedProps = Interop.filterProps oldProps props
        let canUpdateView = ViewElement.canUpdate prevElement changedProps removedProps
        let canUpdateElement =
            true

        canUpdateView && canUpdateElement



    override this.update prevElement oldProps =
        let element = prevElement :?> Dialog
        let changedProps,removedProps = Interop.filterProps oldProps props
        ViewElement.removeProps prevElement removedProps
        removeProps element removedProps
        this.setProps(element, changedProps)
        this.element <- prevElement



// FileDialog
type FileDialogElement(props:Props) =
    inherit TerminalElement(props)

    let removeProps (element: FileDialog) (props: Props) =
        // Properties
        props |> Props.tryFind<IAllowedType list> "fileDialog.allowedTypes" |> Option.iter (fun _ -> element.AllowedTypes <- Unchecked.defaultof<_>)
        props |> Props.tryFind<bool> "fileDialog.allowsMultipleSelection" |> Option.iter (fun _ -> element.AllowsMultipleSelection <- Unchecked.defaultof<_>)
        props |> Props.tryFind<IFileOperations> "fileDialog.fileOperationsHandler" |> Option.iter (fun _ -> element.FileOperationsHandler <- Unchecked.defaultof<_>)
        props |> Props.tryFind<bool> "fileDialog.mustExist" |> Option.iter (fun _ -> element.MustExist <- Unchecked.defaultof<_>)
        props |> Props.tryFind<OpenMode> "fileDialog.openMode" |> Option.iter (fun _ -> element.OpenMode <- Unchecked.defaultof<_>)
        props |> Props.tryFind<string> "fileDialog.path" |> Option.iter (fun _ -> element.Path <- Unchecked.defaultof<_>)
        props |> Props.tryFind<ISearchMatcher> "fileDialog.searchMatcher" |> Option.iter (fun _ -> element.SearchMatcher <- Unchecked.defaultof<_>)
        // Events
        props |> Props.tryFind<FilesSelectedEventArgs->unit> "fileDialog.filesSelected" |> Option.iter (fun _ -> Interop.removeEventHandler <@ element.FilesSelected @> element)

    override _.name = $"FileDialog"

    member _.setProps (element: FileDialog, props: Props) =
        base.setProps(element, props)

        // Properties
        props |> Props.tryFind<IAllowedType list> "fileDialog.allowedTypes" |> Option.iter (fun v -> element.AllowedTypes <- v.ToList())
        props |> Props.tryFind<bool> "fileDialog.allowsMultipleSelection" |> Option.iter (fun v -> element.AllowsMultipleSelection <- v )
        props |> Props.tryFind<IFileOperations> "fileDialog.fileOperationsHandler" |> Option.iter (fun v -> element.FileOperationsHandler <- v )
        props |> Props.tryFind<bool> "fileDialog.mustExist" |> Option.iter (fun v -> element.MustExist <- v )
        props |> Props.tryFind<OpenMode> "fileDialog.openMode" |> Option.iter (fun v -> element.OpenMode <- v )
        props |> Props.tryFind<string> "fileDialog.path" |> Option.iter (fun v -> element.Path <- v )
        props |> Props.tryFind<ISearchMatcher> "fileDialog.searchMatcher" |> Option.iter (fun v -> element.SearchMatcher <- v )
        // Events
        props |> Props.tryFind<FilesSelectedEventArgs->unit> "fileDialog.filesSelected" |> Option.iter (fun v -> Interop.setEventHandler <@ element.FilesSelected @> v element)


    override this.initialize parent =
        #if DEBUG
        Diagnostics.Trace.WriteLine $"{this.name} created!"
        #endif
        this.parent <- parent


        let el = new FileDialog()
        parent |> Option.iter (fun p -> p.Add el |> ignore)
        this.setProps(el, props)
        props |> Props.tryFind<View->unit> "ref" |> Option.iter (fun v -> v el)
        this.element <- el



    override this.canUpdate prevElement oldProps =
        let changedProps,removedProps = Interop.filterProps oldProps props
        let canUpdateView = ViewElement.canUpdate prevElement changedProps removedProps
        let canUpdateElement =
            true

        canUpdateView && canUpdateElement



    override this.update prevElement oldProps =
        let element = prevElement :?> FileDialog
        let changedProps,removedProps = Interop.filterProps oldProps props
        ViewElement.removeProps prevElement removedProps
        removeProps element removedProps
        this.setProps(element, changedProps)
        this.element <- prevElement



// FrameView
type FrameViewElement(props:Props) =
    inherit TerminalElement(props)

    let removeProps (element: FrameView) (props: Props) =
        // No properties or events FrameView
        ()

    override _.name = $"FrameView"

    member _.setProps (element: FrameView, props: Props) =
        base.setProps(element, props)

        // No properties or events FrameView
        ()


    override this.initialize parent =
        #if DEBUG
        Diagnostics.Trace.WriteLine $"{this.name} created!"
        #endif
        this.parent <- parent


        let el = new FrameView()
        parent |> Option.iter (fun p -> p.Add el |> ignore)
        this.setProps(el, props)
        props |> Props.tryFind<View->unit> "ref" |> Option.iter (fun v -> v el)
        this.element <- el



    override this.canUpdate prevElement oldProps =
        let changedProps,removedProps = Interop.filterProps oldProps props
        let canUpdateView = ViewElement.canUpdate prevElement changedProps removedProps
        let canUpdateElement =
            true

        canUpdateView && canUpdateElement



    override this.update prevElement oldProps =
        let element = prevElement :?> FrameView
        let changedProps,removedProps = Interop.filterProps oldProps props
        ViewElement.removeProps prevElement removedProps
        removeProps element removedProps
        this.setProps(element, changedProps)
        this.element <- prevElement



// GraphView
type GraphViewElement(props:Props) =
    inherit TerminalElement(props)

    let removeProps (element: GraphView) (props: Props) =
        // Properties
        props |> Props.tryFind<HorizontalAxis> "graphView.axisX" |> Option.iter (fun _ -> element.AxisX <- Unchecked.defaultof<_>)
        props |> Props.tryFind<VerticalAxis> "graphView.axisY" |> Option.iter (fun _ -> element.AxisY <- Unchecked.defaultof<_>)
        props |> Props.tryFind<PointF> "graphView.cellSize" |> Option.iter (fun _ -> element.CellSize <- Unchecked.defaultof<_>)
        props |> Props.tryFind<Terminal.Gui.Drawing.Attribute option> "graphView.graphColor" |> Option.iter (fun _ -> element.GraphColor <- Unchecked.defaultof<_>)
        props |> Props.tryFind<int> "graphView.marginBottom" |> Option.iter (fun _ -> element.MarginBottom <- Unchecked.defaultof<_>)
        props |> Props.tryFind<int> "graphView.marginLeft" |> Option.iter (fun _ -> element.MarginLeft <- Unchecked.defaultof<_>)
        props |> Props.tryFind<PointF> "graphView.scrollOffset" |> Option.iter (fun _ -> element.ScrollOffset <- Unchecked.defaultof<_>)

    override _.name = $"GraphView"

    member _.setProps (element: GraphView, props: Props) =
        base.setProps(element, props)

        // Properties
        props |> Props.tryFind<HorizontalAxis> "graphView.axisX" |> Option.iter (fun v -> element.AxisX <- v )
        props |> Props.tryFind<VerticalAxis> "graphView.axisY" |> Option.iter (fun v -> element.AxisY <- v )
        props |> Props.tryFind<PointF> "graphView.cellSize" |> Option.iter (fun v -> element.CellSize <- v )
        props |> Props.tryFind<Terminal.Gui.Drawing.Attribute option> "graphView.graphColor" |> Option.iter (fun v -> element.GraphColor <- v  |> Option.toNullable)
        props |> Props.tryFind<int> "graphView.marginBottom" |> Option.iter (fun v -> element.MarginBottom <- (v |> uint32))
        props |> Props.tryFind<int> "graphView.marginLeft" |> Option.iter (fun v -> element.MarginLeft <- (v |> uint32))
        props |> Props.tryFind<PointF> "graphView.scrollOffset" |> Option.iter (fun v -> element.ScrollOffset <- v )


    override this.initialize parent =
        #if DEBUG
        Diagnostics.Trace.WriteLine $"{this.name} created!"
        #endif
        this.parent <- parent


        let el = new GraphView()
        parent |> Option.iter (fun p -> p.Add el |> ignore)
        this.setProps(el, props)
        props |> Props.tryFind<View->unit> "ref" |> Option.iter (fun v -> v el)
        this.element <- el



    override this.canUpdate prevElement oldProps =
        let changedProps,removedProps = Interop.filterProps oldProps props
        let canUpdateView = ViewElement.canUpdate prevElement changedProps removedProps
        let canUpdateElement =
            true

        canUpdateView && canUpdateElement



    override this.update prevElement oldProps =
        let element = prevElement :?> GraphView
        let changedProps,removedProps = Interop.filterProps oldProps props
        ViewElement.removeProps prevElement removedProps
        removeProps element removedProps
        this.setProps(element, changedProps)
        this.element <- prevElement



// HexView
type HexViewElement(props:Props) =
    inherit TerminalElement(props)

    let removeProps (element: HexView) (props: Props) =
        // Properties
        props |> Props.tryFind<int64> "hexView.address" |> Option.iter (fun _ -> element.Address <- Unchecked.defaultof<_>)
        props |> Props.tryFind<int> "hexView.addressWidth" |> Option.iter (fun _ -> element.AddressWidth <- Unchecked.defaultof<_>)
        props |> Props.tryFind<int> "hexView.allowEdits" |> Option.iter (fun _ -> element.BytesPerLine <- Unchecked.defaultof<_>)
        props |> Props.tryFind<bool> "hexView.readOnly" |> Option.iter (fun _ -> element.ReadOnly <- Unchecked.defaultof<_>)
        props |> Props.tryFind<Stream> "hexView.source" |> Option.iter (fun _ -> element.Source <- Unchecked.defaultof<_>)
        // Events
        props |> Props.tryFind<HexViewEditEventArgs->unit> "hexView.edited" |> Option.iter (fun _ -> Interop.removeEventHandler <@ element.Edited @> element)
        props |> Props.tryFind<HexViewEventArgs->unit> "hexView.positionChanged" |> Option.iter (fun _ -> Interop.removeEventHandler <@ element.PositionChanged @> element)

    override _.name = $"HexView"

    member _.setProps (element: HexView, props: Props) =
        base.setProps(element, props)

        // Properties
        props |> Props.tryFind<int64> "hexView.address" |> Option.iter (fun v -> element.Address <- v )
        props |> Props.tryFind<int> "hexView.addressWidth" |> Option.iter (fun v -> element.AddressWidth <- v )
        props |> Props.tryFind<int> "hexView.allowEdits" |> Option.iter (fun v -> element.BytesPerLine <- v )
        props |> Props.tryFind<bool> "hexView.readOnly" |> Option.iter (fun v -> element.ReadOnly <- v )
        props |> Props.tryFind<Stream> "hexView.source" |> Option.iter (fun v -> element.Source <- v )
        // Events
        props |> Props.tryFind<HexViewEditEventArgs->unit> "hexView.edited" |> Option.iter (fun v -> Interop.setEventHandler <@ element.Edited @> v element)
        props |> Props.tryFind<HexViewEventArgs->unit> "hexView.positionChanged" |> Option.iter (fun v -> Interop.setEventHandler <@ element.PositionChanged @> v element)


    override this.initialize parent =
        #if DEBUG
        Diagnostics.Trace.WriteLine $"{this.name} created!"
        #endif
        this.parent <- parent


        let el = new HexView()
        parent |> Option.iter (fun p -> p.Add el |> ignore)
        this.setProps(el, props)
        props |> Props.tryFind<View->unit> "ref" |> Option.iter (fun v -> v el)
        this.element <- el



    override this.canUpdate prevElement oldProps =
        let changedProps,removedProps = Interop.filterProps oldProps props
        let canUpdateView = ViewElement.canUpdate prevElement changedProps removedProps
        let canUpdateElement =
            true

        canUpdateView && canUpdateElement



    override this.update prevElement oldProps =
        let element = prevElement :?> HexView
        let changedProps,removedProps = Interop.filterProps oldProps props
        ViewElement.removeProps prevElement removedProps
        removeProps element removedProps
        this.setProps(element, changedProps)
        this.element <- prevElement



// Label
type LabelElement(props:Props) =
    inherit TerminalElement(props)

    let removeProps (element: Label) (props: Props) =
        // Properties
        props |> Props.tryFind<Rune> "label.hotKeySpecifier" |> Option.iter (fun _ -> element.HotKeySpecifier <- Unchecked.defaultof<_>)
        props |> Props.tryFind<string> "label.text" |> Option.iter (fun _ -> element.Text <- Unchecked.defaultof<_>)

    override _.name = $"Label"

    member _.setProps (element: Label, props: Props) =
        base.setProps(element, props)

        // Properties
        props |> Props.tryFind<Rune> "label.hotKeySpecifier" |> Option.iter (fun v -> element.HotKeySpecifier <- v )
        props |> Props.tryFind<string> "label.text" |> Option.iter (fun v -> element.Text <- v )


    override this.initialize parent =
        #if DEBUG
        Diagnostics.Trace.WriteLine $"{this.name} created!"
        #endif
        this.parent <- parent


        let el = new Label()
        parent |> Option.iter (fun p -> p.Add el |> ignore)
        this.setProps(el, props)
        props |> Props.tryFind<View->unit> "ref" |> Option.iter (fun v -> v el)
        this.element <- el



    override this.canUpdate prevElement oldProps =
        let changedProps,removedProps = Interop.filterProps oldProps props
        let canUpdateView = ViewElement.canUpdate prevElement changedProps removedProps
        let canUpdateElement =
            true

        canUpdateView && canUpdateElement



    override this.update prevElement oldProps =
        let element = prevElement :?> Label
        let changedProps,removedProps = Interop.filterProps oldProps props
        ViewElement.removeProps prevElement removedProps
        removeProps element removedProps
        this.setProps(element, changedProps)
        this.element <- prevElement



// LegendAnnotation
type LegendAnnotationElement(props:Props) =
    inherit TerminalElement(props)

    let removeProps (element: LegendAnnotation) (props: Props) =
        // No properties or events LegendAnnotation
        ()

    override _.name = $"LegendAnnotation"

    member _.setProps (element: LegendAnnotation, props: Props) =
        base.setProps(element, props)

        // No properties or events LegendAnnotation
        ()


    override this.initialize parent =
        #if DEBUG
        Diagnostics.Trace.WriteLine $"{this.name} created!"
        #endif
        this.parent <- parent


        let el = new LegendAnnotation()
        parent |> Option.iter (fun p -> p.Add el |> ignore)
        this.setProps(el, props)
        props |> Props.tryFind<View->unit> "ref" |> Option.iter (fun v -> v el)
        this.element <- el



    override this.canUpdate prevElement oldProps =
        let changedProps,removedProps = Interop.filterProps oldProps props
        let canUpdateView = ViewElement.canUpdate prevElement changedProps removedProps
        let canUpdateElement =
            true

        canUpdateView && canUpdateElement



    override this.update prevElement oldProps =
        let element = prevElement :?> LegendAnnotation
        let changedProps,removedProps = Interop.filterProps oldProps props
        ViewElement.removeProps prevElement removedProps
        removeProps element removedProps
        this.setProps(element, changedProps)
        this.element <- prevElement



// Line
type LineElement(props:Props) =
    inherit TerminalElement(props)

    let removeProps (element: Line) (props: Props) =
        // Properties
        props |> Props.tryFind<Orientation> "line.orientation" |> Option.iter (fun _ -> element.Orientation <- Unchecked.defaultof<_>)
        // Events
        props |> Props.tryFind<Orientation->unit> "line.orientationChanged" |> Option.iter (fun _ -> Interop.removeEventHandler <@ element.OrientationChanged @> element)
        props |> Props.tryFind<CancelEventArgs<Orientation>->unit> "line.orientationChanging" |> Option.iter (fun _ -> Interop.removeEventHandler <@ element.OrientationChanging @> element)

    override _.name = $"Line"

    member _.setProps (element: Line, props: Props) =
        base.setProps(element, props)

        // Properties
        props |> Props.tryFind<Orientation> "line.orientation" |> Option.iter (fun v -> element.Orientation <- v )
        // Events
        props |> Props.tryFind<Orientation->unit> "line.orientationChanged" |> Option.iter (fun v -> Interop.setEventHandler <@ element.OrientationChanged @> (fun arg -> v arg.Value) element)
        props |> Props.tryFind<CancelEventArgs<Orientation>->unit> "line.orientationChanging" |> Option.iter (fun v -> Interop.setEventHandler <@ element.OrientationChanging @> v element)


    override this.initialize parent =
        #if DEBUG
        Diagnostics.Trace.WriteLine $"{this.name} created!"
        #endif
        this.parent <- parent


        let el = new Line()
        parent |> Option.iter (fun p -> p.Add el |> ignore)
        this.setProps(el, props)
        props |> Props.tryFind<View->unit> "ref" |> Option.iter (fun v -> v el)
        this.element <- el



    override this.canUpdate prevElement oldProps =
        let changedProps,removedProps = Interop.filterProps oldProps props
        let canUpdateView = ViewElement.canUpdate prevElement changedProps removedProps
        let canUpdateElement =
            true

        canUpdateView && canUpdateElement



    override this.update prevElement oldProps =
        let element = prevElement :?> Line
        let changedProps,removedProps = Interop.filterProps oldProps props
        ViewElement.removeProps prevElement removedProps
        removeProps element removedProps
        this.setProps(element, changedProps)
        this.element <- prevElement



// LineView
type LineViewElement(props:Props) =
    inherit TerminalElement(props)

    let removeProps (element: LineView) (props: Props) =
        // Properties
        props |> Props.tryFind<Rune option> "lineView.endingAnchor" |> Option.iter (fun _ -> element.EndingAnchor <- Unchecked.defaultof<_>)
        props |> Props.tryFind<Rune> "lineView.lineRune" |> Option.iter (fun _ -> element.LineRune <- Unchecked.defaultof<_>)
        props |> Props.tryFind<Orientation> "lineView.orientation" |> Option.iter (fun _ -> element.Orientation <- Unchecked.defaultof<_>)
        props |> Props.tryFind<Rune option> "lineView.startingAnchor" |> Option.iter (fun _ -> element.StartingAnchor <- Unchecked.defaultof<_>)

    override _.name = $"LineView"

    member _.setProps (element: LineView, props: Props) =
        base.setProps(element, props)

        // Properties
        props |> Props.tryFind<Rune option> "lineView.endingAnchor" |> Option.iter (fun v -> element.EndingAnchor <- v  |> Option.toNullable)
        props |> Props.tryFind<Rune> "lineView.lineRune" |> Option.iter (fun v -> element.LineRune <- v )
        props |> Props.tryFind<Orientation> "lineView.orientation" |> Option.iter (fun v -> element.Orientation <- v )
        props |> Props.tryFind<Rune option> "lineView.startingAnchor" |> Option.iter (fun v -> element.StartingAnchor <- v  |> Option.toNullable)


    override this.initialize parent =
        #if DEBUG
        Diagnostics.Trace.WriteLine $"{this.name} created!"
        #endif
        this.parent <- parent


        let el = new LineView()
        parent |> Option.iter (fun p -> p.Add el |> ignore)
        this.setProps(el, props)
        props |> Props.tryFind<View->unit> "ref" |> Option.iter (fun v -> v el)
        this.element <- el



    override this.canUpdate prevElement oldProps =
        let changedProps,removedProps = Interop.filterProps oldProps props
        let canUpdateView = ViewElement.canUpdate prevElement changedProps removedProps
        let canUpdateElement =
            true

        canUpdateView && canUpdateElement



    override this.update prevElement oldProps =
        let element = prevElement :?> LineView
        let changedProps,removedProps = Interop.filterProps oldProps props
        ViewElement.removeProps prevElement removedProps
        removeProps element removedProps
        this.setProps(element, changedProps)
        this.element <- prevElement



// ListView
type ListViewElement(props:Props) =
    inherit TerminalElement(props)

    let removeProps (element: ListView) (props: Props) =
        // Properties
        props |> Props.tryFind<bool> "listView.allowsMarking" |> Option.iter (fun _ -> element.AllowsMarking <- Unchecked.defaultof<_>)
        props |> Props.tryFind<bool> "listView.allowsMultipleSelection" |> Option.iter (fun _ -> element.AllowsMultipleSelection <- Unchecked.defaultof<_>)
        props |> Props.tryFind<Int32> "listView.leftItem" |> Option.iter (fun _ -> element.LeftItem <- Unchecked.defaultof<_>)
        props |> Props.tryFind<Int32> "listView.selectedItem" |> Option.iter (fun _ -> element.SelectedItem <- Unchecked.defaultof<_>)
        props |> Props.tryFind<string list> "listView.source" |> Option.iter (fun _ -> element.SetSource Unchecked.defaultof<_>)
        props |> Props.tryFind<Int32> "listView.topItem" |> Option.iter (fun _ -> element.TopItem <- Unchecked.defaultof<_>)
        // Events
        props |> Props.tryFind<NotifyCollectionChangedEventArgs->unit> "listView.collectionChanged" |> Option.iter (fun _ -> Interop.removeEventHandler <@ element.CollectionChanged @> element)
        props |> Props.tryFind<ListViewItemEventArgs->unit> "listView.openSelectedItem" |> Option.iter (fun _ -> Interop.removeEventHandler <@ element.OpenSelectedItem @> element)
        props |> Props.tryFind<ListViewRowEventArgs->unit> "listView.rowRender" |> Option.iter (fun _ -> Interop.removeEventHandler <@ element.RowRender @> element)
        props |> Props.tryFind<ListViewItemEventArgs->unit> "listView.selectedItemChanged" |> Option.iter (fun _ -> Interop.removeEventHandler <@ element.SelectedItemChanged @> element)

    override _.name = $"ListView"

    member _.setProps (element: ListView, props: Props) =
        base.setProps(element, props)

        // Properties
        props |> Props.tryFind<bool> "listView.allowsMarking" |> Option.iter (fun v -> element.AllowsMarking <- v )
        props |> Props.tryFind<bool> "listView.allowsMultipleSelection" |> Option.iter (fun v -> element.AllowsMultipleSelection <- v )
        props |> Props.tryFind<Int32> "listView.leftItem" |> Option.iter (fun v -> element.LeftItem <- v )
        props |> Props.tryFind<Int32> "listView.selectedItem" |> Option.iter (fun v -> element.SelectedItem <- v )
        props |> Props.tryFind<string list> "listView.source" |> Option.iter (fun v -> element.SetSource (ObservableCollection(v)))
        props |> Props.tryFind<Int32> "listView.topItem" |> Option.iter (fun v -> element.TopItem <- v )
        // Events
        props |> Props.tryFind<NotifyCollectionChangedEventArgs->unit> "listView.collectionChanged" |> Option.iter (fun v -> Interop.setEventHandler <@ element.CollectionChanged @> v element)
        props |> Props.tryFind<ListViewItemEventArgs->unit> "listView.openSelectedItem" |> Option.iter (fun v -> Interop.setEventHandler <@ element.OpenSelectedItem @> v element)
        props |> Props.tryFind<ListViewRowEventArgs->unit> "listView.rowRender" |> Option.iter (fun v -> Interop.setEventHandler <@ element.RowRender @> v element)
        props |> Props.tryFind<ListViewItemEventArgs->unit> "listView.selectedItemChanged" |> Option.iter (fun v -> Interop.setEventHandler <@ element.SelectedItemChanged @> v element)


    override this.initialize parent =
        #if DEBUG
        Diagnostics.Trace.WriteLine $"{this.name} created!"
        #endif
        this.parent <- parent


        let el = new ListView()
        parent |> Option.iter (fun p -> p.Add el |> ignore)
        this.setProps(el, props)
        props |> Props.tryFind<View->unit> "ref" |> Option.iter (fun v -> v el)
        this.element <- el



    override this.canUpdate prevElement oldProps =
        let changedProps,removedProps = Interop.filterProps oldProps props
        let canUpdateView = ViewElement.canUpdate prevElement changedProps removedProps
        let canUpdateElement =
            true

        canUpdateView && canUpdateElement



    override this.update prevElement oldProps =
        let element = prevElement :?> ListView
        let changedProps,removedProps = Interop.filterProps oldProps props
        ViewElement.removeProps prevElement removedProps
        removeProps element removedProps
        this.setProps(element, changedProps)
        this.element <- prevElement



// Margin
type MarginElement(props:Props) =
    inherit TerminalElement(props)

    let removeProps (element: Margin) (props: Props) =
        // Properties
        props |> Props.tryFind<ShadowStyle> "margin.shadowStyle" |> Option.iter (fun _ -> element.ShadowStyle <- Unchecked.defaultof<_>)

    override _.name = $"Margin"

    member _.setProps (element: Margin, props: Props) =
        base.setProps(element, props)

        // Properties
        props |> Props.tryFind<ShadowStyle> "margin.shadowStyle" |> Option.iter (fun v -> element.ShadowStyle <- v )


    override this.initialize parent =
        #if DEBUG
        Diagnostics.Trace.WriteLine $"{this.name} created!"
        #endif
        this.parent <- parent


        let el = new Margin()
        parent |> Option.iter (fun p -> p.Add el |> ignore)
        this.setProps(el, props)
        props |> Props.tryFind<View->unit> "ref" |> Option.iter (fun v -> v el)
        this.element <- el



    override this.canUpdate prevElement oldProps =
        let changedProps,removedProps = Interop.filterProps oldProps props
        let canUpdateView = ViewElement.canUpdate prevElement changedProps removedProps
        let canUpdateElement =
            true

        canUpdateView && canUpdateElement



    override this.update prevElement oldProps =
        let element = prevElement :?> Margin
        let changedProps,removedProps = Interop.filterProps oldProps props
        ViewElement.removeProps prevElement removedProps
        removeProps element removedProps
        this.setProps(element, changedProps)
        this.element <- prevElement



// Menuv2
type Menuv2Element(props:Props) =
    inherit TerminalElement(props)

    let removeProps (element: Menuv2) (props: Props) =
        // Properties
        props |> Props.tryFind<MenuItemv2> "menuv2.selectedMenuItem" |> Option.iter (fun _ -> element.SelectedMenuItem <- Unchecked.defaultof<_>)
        props |> Props.tryFind<MenuItemv2> "menuv2.superMenuItem" |> Option.iter (fun _ -> element.SuperMenuItem <- Unchecked.defaultof<_>)
        // Events
        props |> Props.tryFind<CommandEventArgs->unit> "menuv2.accepted" |> Option.iter (fun v -> Interop.setEventHandler <@ element.Accepted @> v element)
        props |> Props.tryFind<MenuItemv2->unit> "menuv2.selectedMenuItemChanged" |> Option.iter (fun v -> Interop.setEventHandler <@ element.SelectedMenuItemChanged @> v element)
        ()

    override _.name = $"Menuv2"

    member _.setProps (element: Menuv2, props: Props) =
        base.setProps(element, props)

        // Properties
        props |> Props.tryFind<MenuItemv2> "menuv2.selectedMenuItem" |> Option.iter (fun v -> element.SelectedMenuItem <- v )
        props |> Props.tryFind<MenuItemv2> "menuv2.superMenuItem" |> Option.iter (fun v -> element.SuperMenuItem <- v )
        // Events
        props |> Props.tryFind<CommandEventArgs->unit> "menuv2.accepted" |> Option.iter (fun v -> Interop.setEventHandler <@ element.Accepted @> v element)
        props |> Props.tryFind<MenuItemv2->unit> "menuv2.selectedMenuItemChanged" |> Option.iter (fun v -> Interop.setEventHandler <@ element.SelectedMenuItemChanged @> v element)


    override this.initialize parent =
        #if DEBUG
        Diagnostics.Trace.WriteLine $"{this.name} created!"
        #endif
        this.parent <- parent


        let el = new Menuv2()
        parent |> Option.iter (fun p -> p.Add el |> ignore)
        this.setProps(el, props)
        props |> Props.tryFind<View->unit> "ref" |> Option.iter (fun v -> v el)
        this.element <- el



    override this.canUpdate prevElement oldProps =
        let changedProps,removedProps = Interop.filterProps oldProps props
        let canUpdateView = ViewElement.canUpdate prevElement changedProps removedProps
        let canUpdateElement =
            true

        canUpdateView && canUpdateElement



    override this.update prevElement oldProps =
        let element = prevElement :?> Menuv2
        let changedProps,removedProps = Interop.filterProps oldProps props
        ViewElement.removeProps prevElement removedProps
        removeProps element removedProps
        this.setProps(element, changedProps)
        this.element <- prevElement



type PopoverMenuElement(props: Props) =
    inherit TerminalElement(props)

    let removeProps (element:  PopoverMenu) (props: Props) =
        // Properties
        props |> Props.tryFind<Key> "popoverMenu.key" |> Option.iter (fun _ -> element.Key <- Unchecked.defaultof<_>)
        props |> Props.tryFind<MouseFlags> "popoverMenu.mouseFlags" |> Option.iter (fun _ -> element.MouseFlags <- Unchecked.defaultof<_>)
        props |> Props.tryFind<Menuv2> "popoverMenu.root" |> Option.iter (fun _ -> element.Root <- Unchecked.defaultof<_>)
        // Events
        props |> Props.tryFind<CommandEventArgs->unit> "popoverMenu.accepted" |> Option.iter (fun _ -> Interop.removeEventHandler <@ element.Accepted @> element)
        props |> Props.tryFind<KeyChangedEventArgs->unit> "popoverMenu.keyChanged" |> Option.iter (fun _ -> Interop.removeEventHandler <@ element.KeyChanged @> element)

    override this.name = "PopoverMenu"

    member _.setProps (element: PopoverMenu, props: Props) =
        base.setProps(element, props)

        // Properties
        props |> Props.tryFind<Key> "popoverMenu.key" |> Option.iter (fun v -> element.Key <- v )
        props |> Props.tryFind<MouseFlags> "popoverMenu.mouseFlags" |> Option.iter (fun v -> element.MouseFlags <- v )
        props |> Props.tryFind<Menuv2> "popoverMenu.root" |> Option.iter (fun v -> element.Root <- v )
        // Events
        props |> Props.tryFind<CommandEventArgs->unit> "popoverMenu.accepted" |> Option.iter (fun v -> Interop.setEventHandler <@ element.Accepted @> v element)
        props |> Props.tryFind<KeyChangedEventArgs->unit> "popoverMenu.keyChanged" |> Option.iter (fun v -> Interop.setEventHandler <@ element.KeyChanged @> v element)

    override this.subElements =
        {| key= "popoverMenu.root.element"; setParent= false |}::base.subElements

    override this.initialize(parent) =
        #if DEBUG
        Diagnostics.Trace.WriteLine $"{this.name} created!"
        #endif
        this.parent <- parent

        let el = new PopoverMenu()

        this.initializeSubElements(el)
        |> Seq.iter (fun (k, v) -> props.add k v)

        parent |> Option.iter (fun p -> p.Add el |> ignore)
        this.setProps(el, props)
        props |> Props.tryFind<View->unit> "ref" |> Option.iter (fun v -> v el)
        this.element <- el

    override this.canUpdate prevElement oldProps =
        let changedProps,removedProps = Interop.filterProps oldProps props
        let canUpdateView = ViewElement.canUpdate prevElement changedProps removedProps
        let canUpdateElement =
            true

        canUpdateView && canUpdateElement

    override this.update prevElement oldProps =
        let element = prevElement :?> PopoverMenu
        let changedProps,removedProps = Interop.filterProps oldProps props
        ViewElement.removeProps prevElement removedProps
        removeProps element removedProps
        this.setProps(element, changedProps)
        this.element <- prevElement



// MenuBarItemv2
type MenuBarItemv2Element(props:Props) =
    inherit TerminalElement(props)

    let removeProps (element:  MenuBarItemv2) (props: Props) =
        // Properties
        props |> Props.tryFind<PopoverMenu> "menuBarItemv2.popoverMenu" |> Option.iter (fun _ -> element.PopoverMenu <- Unchecked.defaultof<_> )
        props |> Props.tryFind<bool> "menuBarItemv2.popoverMenuOpen" |> Option.iter (fun _ -> element.PopoverMenuOpen <- Unchecked.defaultof<_> )
        // Events
        props |> Props.tryFind<bool->unit> "menuBarItemv2.popoverMenuOpenChanged" |> Option.iter (fun _ -> Interop.removeEventHandler <@ element.PopoverMenuOpenChanged @> element)

    override this.name = "MenuBarItemv2"

    member _.setProps (element: MenuBarItemv2, props: Props) =
        base.setProps(element, props)

        // Properties
        props |> Props.tryFind<PopoverMenu> "menuBarItemv2.popoverMenu" |> Option.iter (fun v -> element.PopoverMenu <- v )
        props |> Props.tryFind<bool> "menuBarItemv2.popoverMenuOpen" |> Option.iter (fun v -> element.PopoverMenuOpen <- v )
        // Events
        props |> Props.tryFind<bool->unit> "menuBarItemv2.popoverMenuOpenChanged" |> Option.iter (fun v -> Interop.setEventHandler <@ element.PopoverMenuOpenChanged @> (fun args -> v args.Value) element)

    override this.subElements =
        {| key="menuBarItemv2.popoverMenu.element"; setParent=true  |}::base.subElements

    override this.initialize(parent) =
        #if DEBUG
        Diagnostics.Trace.WriteLine $"{this.name} created!"
        #endif
        this.parent <- parent


        let el = new MenuBarItemv2()

        this.initializeSubElements(el)
        |> Seq.iter (fun (k, v) -> props.add k v)

        parent |> Option.iter (fun p -> p.Add el |> ignore)
        this.setProps(el, props)
        props |> Props.tryFind<View->unit> "ref" |> Option.iter (fun v -> v el)
        this.element <- el

    override this.canUpdate prevElement oldProps =
        let changedProps,removedProps = Interop.filterProps oldProps props
        let canUpdateView = ViewElement.canUpdate prevElement changedProps removedProps
        let canUpdateElement =
            true

        canUpdateView && canUpdateElement

    override this.update prevElement oldProps =
        let element = prevElement :?> MenuBarItemv2
        let changedProps,removedProps = Interop.filterProps oldProps props
        ViewElement.removeProps prevElement removedProps
        removeProps element removedProps
        this.setProps(element, changedProps)
        this.element <- prevElement

// MenuBar
type MenuBarv2Element(props:Props) =
    inherit TerminalElement(props)

    let removeProps (element: MenuBarv2) (props: Props) =
        // Properties
        props |> Props.tryFind<Key> "menuBarv2.key" |> Option.iter (fun _ -> element.Key <- Unchecked.defaultof<_>)

        // NOTE: No need to handle `Menus: MenuBarItemv2Element list` property here,
        //       as it already registered as "children" property.
        //       And "children" properties are handled by the TreeDiff initializeTree function

        // Events
        props |> Props.tryFind<KeyChangedEventArgs->unit> "menuBarv2.keyChanged" |> Option.iter (fun v -> Interop.removeEventHandler <@ element.KeyChanged @> element)

    override _.name = $"MenuBarv2"

    member _.setProps (element: MenuBarv2, props: Props) =
        base.setProps(element, props)

        // Properties
        props |> Props.tryFind<Key> "menuBarv2.key" |> Option.iter (fun v -> element.Key <- v )

        // NOTE: No need to handle `Menus: MenuBarItemv2Element list` property here,
        //       as it already registered as "children" property.
        //       And "children" properties are handled by the TreeDiff initializeTree function

        // Events
        props |> Props.tryFind<KeyChangedEventArgs->unit> "menuBarv2.keyChanged" |> Option.iter (fun v -> Interop.setEventHandler <@ element.KeyChanged @> v element)

    override this.initialize parent =
        #if DEBUG
        Diagnostics.Trace.WriteLine $"{this.name} created!"
        #endif

        this.parent <- parent
        let el = new MenuBarv2()
        parent |> Option.iter (fun p -> p.Add el |> ignore)
        this.setProps(el, props)
        props |> Props.tryFind<View->unit> "ref" |> Option.iter (fun v -> v el)
        this.element <- el



    override this.canUpdate prevElement oldProps =
        let changedProps,removedProps = Interop.filterProps oldProps props
        let canUpdateView = ViewElement.canUpdate prevElement changedProps removedProps
        let canUpdateElement =
            true

        canUpdateView && canUpdateElement



    override this.update prevElement oldProps =
        let element = prevElement :?> MenuBarv2
        let changedProps,removedProps = Interop.filterProps oldProps props
        ViewElement.removeProps prevElement removedProps
        removeProps element removedProps
        this.setProps(element, changedProps)
        this.element <- prevElement



// Shortcut
type ShortcutElement(props:Props) =
    inherit TerminalElement(props)

    let removeProps (element: Shortcut) (props: Props) =
        // Properties
        props |> Props.tryFind<Action> "shortcut.action" |> Option.iter (fun _ -> element.Action <- Unchecked.defaultof<_>)
        props |> Props.tryFind<AlignmentModes> "shortcut.alignmentModes" |> Option.iter (fun _ -> element.AlignmentModes <- Unchecked.defaultof<_>)
        props |> Props.tryFind<View> "shortcut.commandView" |> Option.iter (fun _ -> element.CommandView <- Unchecked.defaultof<_>)
        props |> Props.tryFind<bool> "shortcut.forceFocusColors" |> Option.iter (fun _ -> element.ForceFocusColors <- Unchecked.defaultof<_>)
        props |> Props.tryFind<string> "shortcut.helpText" |> Option.iter (fun _ -> element.HelpText <- Unchecked.defaultof<_>)
        props |> Props.tryFind<string> "shortcut.text" |> Option.iter (fun _ -> element.Text <- Unchecked.defaultof<_>)
        props |> Props.tryFind<bool> "shortcut.bindKeyToApplication" |> Option.iter (fun _ -> element.BindKeyToApplication <- Unchecked.defaultof<_>)
        props |> Props.tryFind<Key> "shortcut.key" |> Option.iter (fun _ -> element.Key <- Unchecked.defaultof<_>)
        props |> Props.tryFind<Int32> "shortcut.minimumKeyTextSize" |> Option.iter (fun _ -> element.MinimumKeyTextSize <- Unchecked.defaultof<_>)
        // Events
        props |> Props.tryFind<Orientation->unit> "shortcut.orientationChanged" |> Option.iter (fun _ -> Interop.removeEventHandler <@ element.OrientationChanged @> element)
        props |> Props.tryFind<CancelEventArgs<Orientation>->unit> "shortcut.orientationChanging" |> Option.iter (fun _ -> Interop.removeEventHandler <@ element.OrientationChanging @> element)

    override _.name = $"Shortcut"

    member _.setProps (element: Shortcut, props: Props) =
        base.setProps(element, props)

        // Properties
        props |> Props.tryFind<Action> "shortcut.action" |> Option.iter (fun v -> element.Action <- v )
        props |> Props.tryFind<AlignmentModes> "shortcut.alignmentModes" |> Option.iter (fun v -> element.AlignmentModes <- v )
        props |> Props.tryFind<View> "shortcut.commandView" |> Option.iter (fun v -> element.CommandView <- v )
        props |> Props.tryFind<bool> "shortcut.forceFocusColors" |> Option.iter (fun v -> element.ForceFocusColors <- v )
        props |> Props.tryFind<string> "shortcut.helpText" |> Option.iter (fun v -> element.HelpText <- v )
        props |> Props.tryFind<string> "shortcut.text" |> Option.iter (fun v -> element.Text <- v )
        props |> Props.tryFind<bool> "shortcut.bindKeyToApplication" |> Option.iter (fun v -> element.BindKeyToApplication <- v )
        props |> Props.tryFind<Key> "shortcut.key" |> Option.iter (fun v -> element.Key <- v )
        props |> Props.tryFind<Int32> "shortcut.minimumKeyTextSize" |> Option.iter (fun v -> element.MinimumKeyTextSize <- v )
        // Events
        props |> Props.tryFind<Orientation->unit> "shortcut.orientationChanged" |> Option.iter (fun v -> Interop.setEventHandler <@ element.OrientationChanged @> (fun arg -> v arg.Value) element)
        props |> Props.tryFind<CancelEventArgs<Orientation>->unit> "shortcut.orientationChanging" |> Option.iter (fun v -> Interop.setEventHandler <@ element.OrientationChanging @> v element)

    override this.subElements =
        {| key="shortcut.commandView.element"; setParent=true |}::base.subElements


    override this.initialize parent =
        #if DEBUG
        Diagnostics.Trace.WriteLine $"{this.name} created!"
        #endif
        this.parent <- parent



        let el = new Shortcut()

        this.initializeSubElements(el)
        |> Seq.iter (fun (k, v) -> props.add k v)

        parent |> Option.iter (fun p -> p.Add el |> ignore)
        this.setProps(el, props)
        props |> Props.tryFind<View->unit> "ref" |> Option.iter (fun v -> v el)
        this.element <- el



    override this.canUpdate prevElement oldProps =
        let changedProps,removedProps = Interop.filterProps oldProps props
        let canUpdateView = ViewElement.canUpdate prevElement changedProps removedProps
        let canUpdateElement =
            true

        canUpdateView && canUpdateElement



    override this.update prevElement oldProps =
        let element = prevElement :?> Shortcut
        let changedProps,removedProps = Interop.filterProps oldProps props
        ViewElement.removeProps prevElement removedProps
        removeProps element removedProps
        this.setProps(element, changedProps)
        this.element <- prevElement



type MenuItemv2Element(props:Props) =
    inherit ShortcutElement(props)

    let removeProps (element:  MenuItemv2) (props: Props) =
        // Properties
        props |> Props.tryFind<Command> "menuItemv2.command" |> Option.iter (fun _ -> Unchecked.defaultof<_>)
        props |> Props.tryFind<Menuv2> "menuItemv2.subMenu" |> Option.iter (fun _ -> Unchecked.defaultof<_>)
        props |> Props.tryFind<View> "menuItemv2.targetView" |> Option.iter (fun _ -> Unchecked.defaultof<_>)
        // Events
        props |> Props.tryFind<CommandEventArgs->unit> "menuItemv2.accepted" |> Option.iter (fun v -> Interop.removeEventHandler <@ element.Accepted @> element)

    override _.name = $"MenuItemv2"

    member this.setProps (element: MenuItemv2, props: Props) =
        base.setProps(element, props)

        // Properties
        props |> Props.tryFind<Command> "menuItemv2.command" |> Option.iter (fun v -> element.Command <- v )
        props |> Props.tryFind<Menuv2> "menuItemv2.subMenu" |> Option.iter (fun v -> element.SubMenu <- v )
        props |> Props.tryFind<View> "menuItemv2.targetView" |> Option.iter (fun v -> element.TargetView <- v )
        // Events
        props |> Props.tryFind<CommandEventArgs->unit> "menuItemv2.accepted" |> Option.iter (fun v -> Interop.setEventHandler <@ element.Accepted @> v element)

    override this.subElements =
        {| key="menuItemv2.subMenu.element" ; setParent=true |}::base.subElements


    override this.initialize parent =
        #if DEBUG
        Diagnostics.Trace.WriteLine $"{this.name} created!"
        #endif
        this.parent <- parent

        let el = new MenuItemv2()

        this.initializeSubElements(el)
        |> Seq.iter (fun (k, v) -> props.add k v)

        parent |> Option.iter (fun p -> p.Add el |> ignore)
        this.setProps(el, props)
        props |> Props.tryFind<View->unit> "ref" |> Option.iter (fun v -> v el)
        this.element <- el



    override this.canUpdate prevElement oldProps =
        let changedProps,removedProps = Interop.filterProps oldProps props
        let canUpdateView = ViewElement.canUpdate prevElement changedProps removedProps
        let canUpdateElement =
            true

        canUpdateView && canUpdateElement

    override this.update prevElement oldProps =
        let element = prevElement :?> MenuItemv2
        let changedProps,removedProps = Interop.filterProps oldProps props
        ViewElement.removeProps prevElement removedProps
        removeProps element removedProps
        this.setProps(element, changedProps)
        this.element <- prevElement



// NumericUpDown<'a>
type NumericUpDownElement<'a>(props:Props) =
    inherit TerminalElement(props)

    let removeProps (element:NumericUpDown<'a>) (props: Props) =
        // Properties
        props |> Props.tryFind<string> "numericUpDown`1.format" |> Option.iter (fun _ -> element.Format <- Unchecked.defaultof<_>)
        props |> Props.tryFind<'a> "numericUpDown`1.increment" |> Option.iter (fun _ -> element.Increment <- Unchecked.defaultof<_>)
        props |> Props.tryFind<'a> "numericUpDown`1.value" |> Option.iter (fun _ -> element.Value <- Unchecked.defaultof<_>)
        // Events
        props |> Props.tryFind<string->unit> "numericUpDown`1.formatChanged" |> Option.iter (fun _ -> Interop.removeEventHandler <@ element.FormatChanged @> element)
        props |> Props.tryFind<'a->unit> "numericUpDown`1.incrementChanged" |> Option.iter (fun _ -> Interop.removeEventHandler <@ element.IncrementChanged @> element)
        props |> Props.tryFind<'a->unit> "numericUpDown`1.valueChanged" |> Option.iter (fun _ -> Interop.removeEventHandler <@ element.ValueChanged @> element)
        props |> Props.tryFind<CancelEventArgs<'a>->unit> "numericUpDown`1.valueChanging" |> Option.iter (fun _ -> Interop.removeEventHandler <@ element.ValueChanging @> element)

    override _.name = $"NumericUpDown<'a>"

    member _.setProps (element: NumericUpDown<'a>, props: Props) =
        base.setProps(element, props)

        // Properties
        props |> Props.tryFind<string> "numericUpDown`1.format" |> Option.iter (fun v -> element.Format <- v )
        props |> Props.tryFind<'a> "numericUpDown`1.increment" |> Option.iter (fun v -> element.Increment <- v )
        props |> Props.tryFind<'a> "numericUpDown`1.value" |> Option.iter (fun v -> element.Value <- v )
        // Events
        props |> Props.tryFind<string->unit> "numericUpDown`1.formatChanged" |> Option.iter (fun v -> Interop.setEventHandler <@ element.FormatChanged @> (fun arg -> v arg.Value) element)
        props |> Props.tryFind<'a->unit> "numericUpDown`1.incrementChanged" |> Option.iter (fun v -> Interop.setEventHandler <@ element.IncrementChanged @> (fun arg -> v arg.Value) element)
        props |> Props.tryFind<'a->unit> "numericUpDown`1.valueChanged" |> Option.iter (fun v -> Interop.setEventHandler <@ element.ValueChanged @> (fun arg -> v arg.Value) element)
        props |> Props.tryFind<CancelEventArgs<'a>->unit> "numericUpDown`1.valueChanging" |> Option.iter (fun v -> Interop.setEventHandler <@ element.ValueChanging @> v element)


    override this.initialize parent =
        #if DEBUG
        Diagnostics.Trace.WriteLine $"{this.name} created!"
        #endif
        this.parent <- parent


        let el = new NumericUpDown<'a>()
        parent |> Option.iter (fun p -> p.Add el |> ignore)
        this.setProps(el, props)
        props |> Props.tryFind<View->unit> "ref" |> Option.iter (fun v -> v el)
        this.element <- el



    override this.canUpdate prevElement oldProps =
        let changedProps,removedProps = Interop.filterProps oldProps props
        let canUpdateView = ViewElement.canUpdate prevElement changedProps removedProps
        let canUpdateElement =
            true

        canUpdateView && canUpdateElement



    override this.update prevElement oldProps =
        let element = prevElement :?> NumericUpDown<'a>
        let changedProps,removedProps = Interop.filterProps oldProps props
        ViewElement.removeProps prevElement removedProps
        removeProps element removedProps
        this.setProps(element, changedProps)
        this.element <- prevElement



// NumericUpDown
type NumericUpDownElement(props:Props) =
    inherit NumericUpDownElement<int>(props)

// OpenDialog
type OpenDialogElement(props:Props) =
    inherit TerminalElement(props)

    let removeProps (element: OpenDialog) (props: Props) =
        // Properties
        props |> Props.tryFind<OpenMode> "openDialog.openMode" |> Option.iter (fun _ -> element.OpenMode <- Unchecked.defaultof<_>)

    override _.name = $"OpenDialog"

    member _.setProps (element: OpenDialog, props: Props) =
        base.setProps(element, props)

        // Properties
        props |> Props.tryFind<OpenMode> "openDialog.openMode" |> Option.iter (fun v -> element.OpenMode <- v )


    override this.initialize parent =
        #if DEBUG
        Diagnostics.Trace.WriteLine $"{this.name} created!"
        #endif
        this.parent <- parent


        let el = new OpenDialog()
        parent |> Option.iter (fun p -> p.Add el |> ignore)
        this.setProps(el, props)
        props |> Props.tryFind<View->unit> "ref" |> Option.iter (fun v -> v el)
        this.element <- el



    override this.canUpdate prevElement oldProps =
        let changedProps,removedProps = Interop.filterProps oldProps props
        let canUpdateView = ViewElement.canUpdate prevElement changedProps removedProps
        let canUpdateElement =
            true

        canUpdateView && canUpdateElement



    override this.update prevElement oldProps =
        let element = prevElement :?> OpenDialog
        let changedProps,removedProps = Interop.filterProps oldProps props
        ViewElement.removeProps prevElement removedProps
        removeProps element removedProps
        this.setProps(element, changedProps)
        this.element <- prevElement



// Padding
type PaddingElement(props:Props) =
    inherit TerminalElement(props)

    let removeProps (element: Padding) (props: Props) =
        ()

    override _.name = $"Padding"

    member _.setProps (element: Padding, props: Props) =
        base.setProps(element, props)

        ()


    override this.initialize parent =
        #if DEBUG
        Diagnostics.Trace.WriteLine $"{this.name} created!"
        #endif
        this.parent <- parent


        let el = new Padding()
        parent |> Option.iter (fun p -> p.Add el |> ignore)
        this.setProps(el, props)
        props |> Props.tryFind<View->unit> "ref" |> Option.iter (fun v -> v el)
        this.element <- el



    override this.canUpdate prevElement oldProps =
        let changedProps,removedProps = Interop.filterProps oldProps props
        let canUpdateView = ViewElement.canUpdate prevElement changedProps removedProps
        let canUpdateElement =
            true

        canUpdateView && canUpdateElement



    override this.update prevElement oldProps =
        let element = prevElement :?> Padding
        let changedProps,removedProps = Interop.filterProps oldProps props
        ViewElement.removeProps prevElement removedProps
        removeProps element removedProps
        this.setProps(element, changedProps)
        this.element <- prevElement



// ProgressBar
type ProgressBarElement(props:Props) =
    inherit TerminalElement(props)

    let removeProps (element: ProgressBar) (props: Props) =
        // Properties
        props |> Props.tryFind<bool> "progressBar.bidirectionalMarquee" |> Option.iter (fun _ -> element.BidirectionalMarquee <- Unchecked.defaultof<_>)
        props |> Props.tryFind<Single> "progressBar.fraction" |> Option.iter (fun _ -> element.Fraction <- Unchecked.defaultof<_>)
        props |> Props.tryFind<ProgressBarFormat> "progressBar.progressBarFormat" |> Option.iter (fun _ -> element.ProgressBarFormat <- Unchecked.defaultof<_>)
        props |> Props.tryFind<ProgressBarStyle> "progressBar.progressBarStyle" |> Option.iter (fun _ -> element.ProgressBarStyle <- Unchecked.defaultof<_>)
        props |> Props.tryFind<Rune> "progressBar.segmentCharacter" |> Option.iter (fun _ -> element.SegmentCharacter <- Unchecked.defaultof<_>)
        props |> Props.tryFind<string> "progressBar.text" |> Option.iter (fun _ -> element.Text <- Unchecked.defaultof<_>)

    override _.name = $"ProgressBar"

    member _.setProps (element: ProgressBar, props: Props) =
        base.setProps(element, props)

        // Properties
        props |> Props.tryFind<bool> "progressBar.bidirectionalMarquee" |> Option.iter (fun v -> element.BidirectionalMarquee <- v )
        props |> Props.tryFind<Single> "progressBar.fraction" |> Option.iter (fun v -> element.Fraction <- v )
        props |> Props.tryFind<ProgressBarFormat> "progressBar.progressBarFormat" |> Option.iter (fun v -> element.ProgressBarFormat <- v )
        props |> Props.tryFind<ProgressBarStyle> "progressBar.progressBarStyle" |> Option.iter (fun v -> element.ProgressBarStyle <- v )
        props |> Props.tryFind<Rune> "progressBar.segmentCharacter" |> Option.iter (fun v -> element.SegmentCharacter <- v )
        props |> Props.tryFind<string> "progressBar.text" |> Option.iter (fun v -> element.Text <- v )


    override this.initialize parent =
        #if DEBUG
        Diagnostics.Trace.WriteLine $"{this.name} created!"
        #endif
        this.parent <- parent


        let el = new ProgressBar()
        parent |> Option.iter (fun p -> p.Add el |> ignore)
        this.setProps(el, props)
        props |> Props.tryFind<View->unit> "ref" |> Option.iter (fun v -> v el)
        this.element <- el



    override this.canUpdate prevElement oldProps =
        let changedProps,removedProps = Interop.filterProps oldProps props
        let canUpdateView = ViewElement.canUpdate prevElement changedProps removedProps
        let canUpdateElement =
            true

        canUpdateView && canUpdateElement



    override this.update prevElement oldProps =
        let element = prevElement :?> ProgressBar
        let changedProps,removedProps = Interop.filterProps oldProps props
        ViewElement.removeProps prevElement removedProps
        removeProps element removedProps
        this.setProps(element, changedProps)
        this.element <- prevElement



// RadioGroup
type RadioGroupElement(props:Props) =
    inherit TerminalElement(props)

    let removeProps (element: RadioGroup) (props: Props) =
        // Properties
        props |> Props.tryFind<bool> "radioGroup.assignHotKeysToRadioLabels" |> Option.iter (fun _ -> element.AssignHotKeysToRadioLabels <- Unchecked.defaultof<_> )
        props |> Props.tryFind<Int32> "radioGroup.cursor" |> Option.iter (fun _ -> element.Cursor <- Unchecked.defaultof<_> )
        props |> Props.tryFind<bool> "radioGroup.doubleClickAccepts" |> Option.iter (fun _ -> element.DoubleClickAccepts <- Unchecked.defaultof<_> )
        props |> Props.tryFind<Int32> "radioGroup.horizontalSpace" |> Option.iter (fun _ -> element.HorizontalSpace <- Unchecked.defaultof<_> )
        props |> Props.tryFind<Orientation> "radioGroup.orientation" |> Option.iter (fun _ -> element.Orientation <- Unchecked.defaultof<_> )
        props |> Props.tryFind<string list> "radioGroup.radioLabels" |> Option.iter (fun _ -> element.RadioLabels <- Unchecked.defaultof<_>)
        props |> Props.tryFind<Int32> "radioGroup.selectedItem" |> Option.iter (fun _ -> element.SelectedItem <- Unchecked.defaultof<_> )
        // Events
        props |> Props.tryFind<Orientation->unit> "radioGroup.orientationChanged" |> Option.iter (fun _ -> Interop.removeEventHandler <@ element.OrientationChanged @> element)
        props |> Props.tryFind<CancelEventArgs<Orientation>->unit> "radioGroup.orientationChanging" |> Option.iter (fun _ -> Interop.removeEventHandler <@ element.OrientationChanging @> element)
        props |> Props.tryFind<SelectedItemChangedArgs->unit> "radioGroup.selectedItemChanged" |> Option.iter (fun _ -> Interop.removeEventHandler <@ element.SelectedItemChanged @> element)

    override _.name = $"RadioGroup"

    member _.setProps (element: RadioGroup, props: Props) =
        base.setProps(element, props)

        // Properties
        props |> Props.tryFind<bool> "radioGroup.assignHotKeysToRadioLabels" |> Option.iter (fun v -> element.AssignHotKeysToRadioLabels <- v )
        props |> Props.tryFind<Int32> "radioGroup.cursor" |> Option.iter (fun v -> element.Cursor <- v )
        props |> Props.tryFind<bool> "radioGroup.doubleClickAccepts" |> Option.iter (fun v -> element.DoubleClickAccepts <- v )
        props |> Props.tryFind<Int32> "radioGroup.horizontalSpace" |> Option.iter (fun v -> element.HorizontalSpace <- v )
        props |> Props.tryFind<Orientation> "radioGroup.orientation" |> Option.iter (fun v -> element.Orientation <- v )
        props |> Props.tryFind<string list> "radioGroup.radioLabels" |> Option.iter (fun v -> element.RadioLabels <- v |> List.toArray)
        props |> Props.tryFind<Int32> "radioGroup.selectedItem" |> Option.iter (fun v -> element.SelectedItem <- v )
        // Events
        props |> Props.tryFind<Orientation->unit> "radioGroup.orientationChanged" |> Option.iter (fun v -> Interop.setEventHandler <@ element.OrientationChanged @> (fun arg -> v arg.Value) element)
        props |> Props.tryFind<CancelEventArgs<Orientation>->unit> "radioGroup.orientationChanging" |> Option.iter (fun v -> Interop.setEventHandler <@ element.OrientationChanging @> v element)
        props |> Props.tryFind<SelectedItemChangedArgs->unit> "radioGroup.selectedItemChanged" |> Option.iter (fun v -> Interop.setEventHandler <@ element.SelectedItemChanged @> v element)


    override this.initialize parent =
        #if DEBUG
        Diagnostics.Trace.WriteLine $"{this.name} created!"
        #endif
        this.parent <- parent


        let el = new RadioGroup()
        parent |> Option.iter (fun p -> p.Add el |> ignore)
        this.setProps(el, props)
        props |> Props.tryFind<View->unit> "ref" |> Option.iter (fun v -> v el)
        this.element <- el



    override this.canUpdate prevElement oldProps =
        let changedProps,removedProps = Interop.filterProps oldProps props
        let canUpdateView = ViewElement.canUpdate prevElement changedProps removedProps
        let canUpdateElement =
            true

        canUpdateView && canUpdateElement



    override this.update prevElement oldProps =
        let element = prevElement :?> RadioGroup
        let changedProps,removedProps = Interop.filterProps oldProps props
        ViewElement.removeProps prevElement removedProps
        removeProps element removedProps
        this.setProps(element, changedProps)
        this.element <- prevElement



// SaveDialog
type SaveDialogElement(props:Props) =
    inherit TerminalElement(props)

    let removeProps (element: SaveDialog) (props: Props) =
        // No properties or events SaveDialog
        ()

    override _.name = $"SaveDialog"

    member _.setProps (element: SaveDialog, props: Props) =
        base.setProps(element, props)

        // No properties or events SaveDialog
        ()


    override this.initialize parent =
        #if DEBUG
        Diagnostics.Trace.WriteLine $"{this.name} created!"
        #endif
        this.parent <- parent


        let el = new SaveDialog()
        parent |> Option.iter (fun p -> p.Add el |> ignore)
        this.setProps(el, props)
        props |> Props.tryFind<View->unit> "ref" |> Option.iter (fun v -> v el)
        this.element <- el



    override this.canUpdate prevElement oldProps =
        let changedProps,removedProps = Interop.filterProps oldProps props
        let canUpdateView = ViewElement.canUpdate prevElement changedProps removedProps
        let canUpdateElement =
            true

        canUpdateView && canUpdateElement



    override this.update prevElement oldProps =
        let element = prevElement :?> SaveDialog
        let changedProps,removedProps = Interop.filterProps oldProps props
        ViewElement.removeProps prevElement removedProps
        removeProps element removedProps
        this.setProps(element, changedProps)
        this.element <- prevElement



// ScrollBar
type ScrollBarElement(props:Props) =
    inherit TerminalElement(props)

    let removeProps (element: ScrollBar) (props: Props) =
        // Properties
        props |> Props.tryFind<bool> "scrollBar.autoShow" |> Option.iter (fun _ -> element.AutoShow <- Unchecked.defaultof<_>)
        props |> Props.tryFind<Int32> "scrollBar.increment" |> Option.iter (fun _ -> element.Increment <- Unchecked.defaultof<_>)
        props |> Props.tryFind<Orientation> "scrollBar.orientation" |> Option.iter (fun _ -> element.Orientation <- Unchecked.defaultof<_>)
        props |> Props.tryFind<Int32> "scrollBar.position" |> Option.iter (fun _ -> element.Position <- Unchecked.defaultof<_>)
        props |> Props.tryFind<Int32> "scrollBar.scrollableContentSize" |> Option.iter (fun _ -> element.ScrollableContentSize <- Unchecked.defaultof<_>)
        props |> Props.tryFind<Int32> "scrollBar.visibleContentSize" |> Option.iter (fun _ -> element.VisibleContentSize <- Unchecked.defaultof<_>)
        // Events
        props |> Props.tryFind<Orientation->unit> "scrollBar.orientationChanged" |> Option.iter (fun _ -> Interop.removeEventHandler <@ element.OrientationChanged @> element)
        props |> Props.tryFind<CancelEventArgs<Orientation>->unit> "scrollBar.orientationChanging" |> Option.iter (fun _ -> Interop.removeEventHandler <@ element.OrientationChanging @> element)
        props |> Props.tryFind<EventArgs<Int32>->unit> "scrollBar.scrollableContentSizeChanged" |> Option.iter (fun _ -> Interop.removeEventHandler <@ element.ScrollableContentSizeChanged @> element)
        props |> Props.tryFind<EventArgs<Int32>->unit> "scrollBar.sliderPositionChanged" |> Option.iter (fun _ -> Interop.removeEventHandler <@ element.SliderPositionChanged @> element)

    override _.name = $"ScrollBar"

    member _.setProps (element: ScrollBar, props: Props) =
        base.setProps(element, props)

        // Properties
        props |> Props.tryFind<bool> "scrollBar.autoShow" |> Option.iter (fun v -> element.AutoShow <- v )
        props |> Props.tryFind<Int32> "scrollBar.increment" |> Option.iter (fun v -> element.Increment <- v )
        props |> Props.tryFind<Orientation> "scrollBar.orientation" |> Option.iter (fun v -> element.Orientation <- v )
        props |> Props.tryFind<Int32> "scrollBar.position" |> Option.iter (fun v -> element.Position <- v )
        props |> Props.tryFind<Int32> "scrollBar.scrollableContentSize" |> Option.iter (fun v -> element.ScrollableContentSize <- v )
        props |> Props.tryFind<Int32> "scrollBar.visibleContentSize" |> Option.iter (fun v -> element.VisibleContentSize <- v )
        // Events
        props |> Props.tryFind<Orientation->unit> "scrollBar.orientationChanged" |> Option.iter (fun v -> Interop.setEventHandler <@ element.OrientationChanged @> (fun arg -> v arg.Value) element)
        props |> Props.tryFind<CancelEventArgs<Orientation>->unit> "scrollBar.orientationChanging" |> Option.iter (fun v -> Interop.setEventHandler <@ element.OrientationChanging @> v element)
        props |> Props.tryFind<EventArgs<Int32>->unit> "scrollBar.scrollableContentSizeChanged" |> Option.iter (fun v -> Interop.setEventHandler <@ element.ScrollableContentSizeChanged @> v element)
        props |> Props.tryFind<EventArgs<Int32>->unit> "scrollBar.sliderPositionChanged" |> Option.iter (fun v -> Interop.setEventHandler <@ element.SliderPositionChanged @> v element)


    override this.initialize parent =
        #if DEBUG
        Diagnostics.Trace.WriteLine $"{this.name} created!"
        #endif
        this.parent <- parent


        let el = new ScrollBar()
        parent |> Option.iter (fun p -> p.Add el |> ignore)
        this.setProps(el, props)
        props |> Props.tryFind<View->unit> "ref" |> Option.iter (fun v -> v el)
        this.element <- el



    override this.canUpdate prevElement oldProps =
        let changedProps,removedProps = Interop.filterProps oldProps props
        let canUpdateView = ViewElement.canUpdate prevElement changedProps removedProps
        let canUpdateElement =
            true

        canUpdateView && canUpdateElement



    override this.update prevElement oldProps =
        let element = prevElement :?> ScrollBar
        let changedProps,removedProps = Interop.filterProps oldProps props
        ViewElement.removeProps prevElement removedProps
        removeProps element removedProps
        this.setProps(element, changedProps)
        this.element <- prevElement



// ScrollSlider
type ScrollSliderElement(props:Props) =
    inherit TerminalElement(props)

    let removeProps (element:  ScrollSlider) (props: Props) =
        // Properties
        props |> Props.tryFind<Orientation> "scrollSlider.orientation" |> Option.iter (fun _ -> element.Orientation <- Unchecked.defaultof<_>)
        props |> Props.tryFind<Int32> "scrollSlider.position" |> Option.iter (fun _ -> element.Position <- Unchecked.defaultof<_>)
        props |> Props.tryFind<Int32> "scrollSlider.size" |> Option.iter (fun _ -> element.Size <- Unchecked.defaultof<_>)
        props |> Props.tryFind<Int32> "scrollSlider.sliderPadding" |> Option.iter (fun _ -> element.SliderPadding <- Unchecked.defaultof<_>)
        props |> Props.tryFind<Int32> "scrollSlider.visibleContentSize" |> Option.iter (fun _ -> element.VisibleContentSize <- Unchecked.defaultof<_>)
        // Events
        props |> Props.tryFind<Orientation->unit> "scrollSlider.orientationChanged" |> Option.iter (fun _ -> Interop.removeEventHandler <@ element.OrientationChanged @> element)
        props |> Props.tryFind<CancelEventArgs<Orientation>->unit> "scrollSlider.orientationChanging" |> Option.iter (fun _ -> Interop.removeEventHandler <@ element.OrientationChanging @> element)
        props |> Props.tryFind<EventArgs<Int32>->unit> "scrollSlider.positionChanged" |> Option.iter (fun _ -> Interop.removeEventHandler <@ element.PositionChanged @> element)
        props |> Props.tryFind<CancelEventArgs<Int32>->unit> "scrollSlider.positionChanging" |> Option.iter (fun _ -> Interop.removeEventHandler <@ element.PositionChanging @> element)
        props |> Props.tryFind<EventArgs<Int32>->unit> "scrollSlider.scrolled" |> Option.iter (fun _ -> Interop.removeEventHandler <@ element.Scrolled @> element)

    override _.name = $"ScrollSlider"

    member _.setProps (element: ScrollSlider, props: Props) =
        base.setProps(element, props)

        // Properties
        props |> Props.tryFind<Orientation> "scrollSlider.orientation" |> Option.iter (fun v -> element.Orientation <- v )
        props |> Props.tryFind<Int32> "scrollSlider.position" |> Option.iter (fun v -> element.Position <- v )
        props |> Props.tryFind<Int32> "scrollSlider.size" |> Option.iter (fun v -> element.Size <- v )
        props |> Props.tryFind<Int32> "scrollSlider.sliderPadding" |> Option.iter (fun v -> element.SliderPadding <- v )
        props |> Props.tryFind<Int32> "scrollSlider.visibleContentSize" |> Option.iter (fun v -> element.VisibleContentSize <- v )
        // Events
        props |> Props.tryFind<Orientation->unit> "scrollSlider.orientationChanged" |> Option.iter (fun v -> Interop.setEventHandler <@ element.OrientationChanged @> (fun arg -> v arg.Value) element)
        props |> Props.tryFind<CancelEventArgs<Orientation>->unit> "scrollSlider.orientationChanging" |> Option.iter (fun v -> Interop.setEventHandler <@ element.OrientationChanging @> v element)
        props |> Props.tryFind<EventArgs<Int32>->unit> "scrollSlider.positionChanged" |> Option.iter (fun v -> Interop.setEventHandler <@ element.PositionChanged @> v element)
        props |> Props.tryFind<CancelEventArgs<Int32>->unit> "scrollSlider.positionChanging" |> Option.iter (fun v -> Interop.setEventHandler <@ element.PositionChanging @> v element)
        props |> Props.tryFind<EventArgs<Int32>->unit> "scrollSlider.scrolled" |> Option.iter (fun v -> Interop.setEventHandler <@ element.Scrolled @> v element)


    override this.initialize parent =
        #if DEBUG
        Diagnostics.Trace.WriteLine $"{this.name} created!"
        #endif
        this.parent <- parent


        let el = new ScrollSlider()
        parent |> Option.iter (fun p -> p.Add el |> ignore)
        this.setProps(el, props)
        props |> Props.tryFind<View->unit> "ref" |> Option.iter (fun v -> v el)
        this.element <- el



    override this.canUpdate prevElement oldProps =
        let changedProps,removedProps = Interop.filterProps oldProps props
        let canUpdateView = ViewElement.canUpdate prevElement changedProps removedProps
        let canUpdateElement =
            true

        canUpdateView && canUpdateElement



    override this.update prevElement oldProps =
        let element = prevElement :?> ScrollSlider
        let changedProps,removedProps = Interop.filterProps oldProps props
        ViewElement.removeProps prevElement removedProps
        removeProps element removedProps
        this.setProps(element, changedProps)
        this.element <- prevElement


// Slider<'a>
type SliderElement<'a>(props:Props) =
    inherit TerminalElement(props)

    let removeProps (element: Slider<'a>) (props: Props) =
        // Properties
        props |> Props.tryFind<bool> "slider`1.allowEmpty" |> Option.iter (fun _ -> element.AllowEmpty <- Unchecked.defaultof<_>)
        props |> Props.tryFind<Int32> "slider`1.focusedOption" |> Option.iter (fun _ -> element.FocusedOption <- Unchecked.defaultof<_>)
        props |> Props.tryFind<Orientation> "slider`1.legendsOrientation" |> Option.iter (fun _ -> element.LegendsOrientation <- Unchecked.defaultof<_>)
        props |> Props.tryFind<Int32> "slider`1.minimumInnerSpacing" |> Option.iter (fun _ -> element.MinimumInnerSpacing <- Unchecked.defaultof<_>)
        props |> Props.tryFind<SliderOption<'a> list> "slider`1.options" |> Option.iter (fun _ -> element.Options <- Unchecked.defaultof<_>)
        props |> Props.tryFind<Orientation> "slider`1.orientation" |> Option.iter (fun _ -> element.Orientation <- Unchecked.defaultof<_>)
        props |> Props.tryFind<bool> "slider`1.rangeAllowSingle" |> Option.iter (fun _ -> element.RangeAllowSingle <- Unchecked.defaultof<_>)
        props |> Props.tryFind<bool> "slider`1.showEndSpacing" |> Option.iter (fun _ -> element.ShowEndSpacing <- Unchecked.defaultof<_>)
        props |> Props.tryFind<bool> "slider`1.showLegends" |> Option.iter (fun _ -> element.ShowLegends <- Unchecked.defaultof<_>)
        props |> Props.tryFind<SliderStyle> "slider`1.style" |> Option.iter (fun _ -> element.Style <- Unchecked.defaultof<_>)
        props |> Props.tryFind<string> "slider`1.text" |> Option.iter (fun _ -> element.Text <- Unchecked.defaultof<_>)
        props |> Props.tryFind<SliderType> "slider`1.``type``" |> Option.iter (fun _ -> element.Type <- Unchecked.defaultof<_>)
        props |> Props.tryFind<bool> "slider`1.useMinimumSize" |> Option.iter (fun _ -> element.UseMinimumSize <- Unchecked.defaultof<_>)
        // Events
        props |> Props.tryFind<SliderEventArgs<'a>->unit> "slider`1.optionFocused" |> Option.iter (fun _ -> Interop.removeEventHandler <@ element.OptionFocused @> element)
        props |> Props.tryFind<SliderEventArgs<'a>->unit> "slider`1.optionsChanged" |> Option.iter (fun _ -> Interop.removeEventHandler <@ element.OptionsChanged @> element)
        props |> Props.tryFind<Orientation->unit> "slider`1.orientationChanged" |> Option.iter (fun _ -> Interop.removeEventHandler <@ element.OrientationChanged @> element)
        props |> Props.tryFind<CancelEventArgs<Orientation>->unit> "slider`1.orientationChanging" |> Option.iter (fun _ -> Interop.removeEventHandler <@ element.OrientationChanging @> element)

    override _.name = $"Slider<'a>"

    member _.setProps (element: Slider<'a>, props: Props) =
        base.setProps(element, props)

        // Properties
        props |> Props.tryFind<bool> "slider`1.allowEmpty" |> Option.iter (fun v -> element.AllowEmpty <- v )
        props |> Props.tryFind<Int32> "slider`1.focusedOption" |> Option.iter (fun v -> element.FocusedOption <- v )
        props |> Props.tryFind<Orientation> "slider`1.legendsOrientation" |> Option.iter (fun v -> element.LegendsOrientation <- v )
        props |> Props.tryFind<Int32> "slider`1.minimumInnerSpacing" |> Option.iter (fun v -> element.MinimumInnerSpacing <- v )
        props |> Props.tryFind<SliderOption<'a> list> "slider`1.options" |> Option.iter (fun v -> element.Options <- v.ToList())
        props |> Props.tryFind<Orientation> "slider`1.orientation" |> Option.iter (fun v -> element.Orientation <- v )
        props |> Props.tryFind<bool> "slider`1.rangeAllowSingle" |> Option.iter (fun v -> element.RangeAllowSingle <- v )
        props |> Props.tryFind<bool> "slider`1.showEndSpacing" |> Option.iter (fun v -> element.ShowEndSpacing <- v )
        props |> Props.tryFind<bool> "slider`1.showLegends" |> Option.iter (fun v -> element.ShowLegends <- v )
        props |> Props.tryFind<SliderStyle> "slider`1.style" |> Option.iter (fun v -> element.Style <- v )
        props |> Props.tryFind<string> "slider`1.text" |> Option.iter (fun v -> element.Text <- v )
        props |> Props.tryFind<SliderType> "slider`1.``type``" |> Option.iter (fun v -> element.Type <- v )
        props |> Props.tryFind<bool> "slider`1.useMinimumSize" |> Option.iter (fun v -> element.UseMinimumSize <- v )
        // Events
        props |> Props.tryFind<SliderEventArgs<'a>->unit> "slider`1.optionFocused" |> Option.iter (fun v -> Interop.setEventHandler <@ element.OptionFocused @> v element)
        props |> Props.tryFind<SliderEventArgs<'a>->unit> "slider`1.optionsChanged" |> Option.iter (fun v -> Interop.setEventHandler <@ element.OptionsChanged @> v element)
        props |> Props.tryFind<Orientation->unit> "slider`1.orientationChanged" |> Option.iter (fun v -> Interop.setEventHandler <@ element.OrientationChanged @> (fun arg -> v arg.Value) element)
        props |> Props.tryFind<CancelEventArgs<Orientation>->unit> "slider`1.orientationChanging" |> Option.iter (fun v -> Interop.setEventHandler <@ element.OrientationChanging @> v element)


    override this.initialize parent =
        #if DEBUG
        Diagnostics.Trace.WriteLine $"{this.name} created!"
        #endif
        this.parent <- parent


        let el = new Slider<'a>()
        parent |> Option.iter (fun p -> p.Add el |> ignore)
        this.setProps(el, props)
        props |> Props.tryFind<View->unit> "ref" |> Option.iter (fun v -> v el)
        this.element <- el



    override this.canUpdate prevElement oldProps =
        let changedProps,removedProps = Interop.filterProps oldProps props
        let canUpdateView = ViewElement.canUpdate prevElement changedProps removedProps
        let canUpdateElement =
            true

        canUpdateView && canUpdateElement



    override this.update prevElement oldProps =
        let element = prevElement :?> Slider<'a>
        let changedProps,removedProps = Interop.filterProps oldProps props
        ViewElement.removeProps prevElement removedProps
        removeProps element removedProps
        this.setProps(element, changedProps)
        this.element <- prevElement



// Slider
type SliderElement(props:Props) =
    inherit TerminalElement(props)

    let removeProps (element: Slider) (props: Props) =
        // No properties or events Slider
        ()

    override _.name = $"Slider"

    member _.setProps (element: Slider, props: Props) =
        base.setProps(element, props)

        // No properties or events Slider
        ()


    override this.initialize parent =
        #if DEBUG
        Diagnostics.Trace.WriteLine $"{this.name} created!"
        #endif
        this.parent <- parent


        let el = new Slider()
        parent |> Option.iter (fun p -> p.Add el |> ignore)
        this.setProps(el, props)
        props |> Props.tryFind<View->unit> "ref" |> Option.iter (fun v -> v el)
        this.element <- el



    override this.canUpdate prevElement oldProps =
        let changedProps,removedProps = Interop.filterProps oldProps props
        let canUpdateView = ViewElement.canUpdate prevElement changedProps removedProps
        let canUpdateElement =
            true

        canUpdateView && canUpdateElement



    override this.update prevElement oldProps =
        let element = prevElement :?> Slider
        let changedProps,removedProps = Interop.filterProps oldProps props
        ViewElement.removeProps prevElement removedProps
        removeProps element removedProps
        this.setProps(element, changedProps)
        this.element <- prevElement



// SpinnerView
type SpinnerViewElement(props:Props) =
    inherit TerminalElement(props)

    let removeProps (element: SpinnerView) (props: Props) =
        // Properties
        props |> Props.tryFind<bool> "spinnerView.autoSpin" |> Option.iter (fun _ -> element.AutoSpin <- Unchecked.defaultof<_>)
        props |> Props.tryFind<string list> "spinnerView.sequence" |> Option.iter (fun _ -> element.Sequence <- Unchecked.defaultof<_>)
        props |> Props.tryFind<bool> "spinnerView.spinBounce" |> Option.iter (fun _ -> element.SpinBounce <- Unchecked.defaultof<_>)
        props |> Props.tryFind<Int32> "spinnerView.spinDelay" |> Option.iter (fun _ -> element.SpinDelay <- Unchecked.defaultof<_>)
        props |> Props.tryFind<bool> "spinnerView.spinReverse" |> Option.iter (fun _ -> element.SpinReverse <- Unchecked.defaultof<_>)
        props |> Props.tryFind<SpinnerStyle> "spinnerView.style" |> Option.iter (fun _ -> element.Style <- Unchecked.defaultof<_>)

    override _.name = $"SpinnerView"

    member _.setProps (element: SpinnerView, props: Props) =
        base.setProps(element, props)

        // Properties
        props |> Props.tryFind<bool> "spinnerView.autoSpin" |> Option.iter (fun v -> element.AutoSpin <- v )
        props |> Props.tryFind<string list> "spinnerView.sequence" |> Option.iter (fun v -> element.Sequence <- v |> List.toArray)
        props |> Props.tryFind<bool> "spinnerView.spinBounce" |> Option.iter (fun v -> element.SpinBounce <- v )
        props |> Props.tryFind<Int32> "spinnerView.spinDelay" |> Option.iter (fun v -> element.SpinDelay <- v )
        props |> Props.tryFind<bool> "spinnerView.spinReverse" |> Option.iter (fun v -> element.SpinReverse <- v )
        props |> Props.tryFind<SpinnerStyle> "spinnerView.style" |> Option.iter (fun v -> element.Style <- v )


    override this.initialize parent =
        #if DEBUG
        Diagnostics.Trace.WriteLine $"{this.name} created!"
        #endif
        this.parent <- parent


        let el = new SpinnerView()
        parent |> Option.iter (fun p -> p.Add el |> ignore)
        this.setProps(el, props)
        props |> Props.tryFind<View->unit> "ref" |> Option.iter (fun v -> v el)
        this.element <- el



    override this.canUpdate prevElement oldProps =
        let changedProps,removedProps = Interop.filterProps oldProps props
        let canUpdateView = ViewElement.canUpdate prevElement changedProps removedProps
        let canUpdateElement =
            true

        canUpdateView && canUpdateElement



    override this.update prevElement oldProps =
        let element = prevElement :?> SpinnerView
        let changedProps,removedProps = Interop.filterProps oldProps props
        ViewElement.removeProps prevElement removedProps
        removeProps element removedProps
        this.setProps(element, changedProps)
        this.element <- prevElement



// StatusBar
type StatusBarElement(props:Props) =
    inherit TerminalElement(props)

    let removeProps (element: StatusBar) (props: Props) =
        // No properties or events StatusBar
        ()

    override _.name = $"StatusBar"

    member _.setProps (element: StatusBar, props: Props) =
        base.setProps(element, props)

        // No properties or events StatusBar
        ()


    override this.initialize parent =
        #if DEBUG
        Diagnostics.Trace.WriteLine $"{this.name} created!"
        #endif
        this.parent <- parent


        let el = new StatusBar()
        parent |> Option.iter (fun p -> p.Add el |> ignore)
        this.setProps(el, props)
        props |> Props.tryFind<View->unit> "ref" |> Option.iter (fun v -> v el)
        this.element <- el



    override this.canUpdate prevElement oldProps =
        let changedProps,removedProps = Interop.filterProps oldProps props
        let canUpdateView = ViewElement.canUpdate prevElement changedProps removedProps
        let canUpdateElement =
            true

        canUpdateView && canUpdateElement



    override this.update prevElement oldProps =
        let element = prevElement :?> StatusBar
        let changedProps,removedProps = Interop.filterProps oldProps props
        ViewElement.removeProps prevElement removedProps
        removeProps element removedProps
        this.setProps(element, changedProps)
        this.element <- prevElement



// Tab
type TabElement(props:Props) =
    inherit TerminalElement(props)

    let removeProps (element: Tab) (props: Props) =
        // Properties
        props |> Props.tryFind<string> "tab.displayText" |> Option.iter (fun _ -> element.DisplayText <- Unchecked.defaultof<_>)
        props |> Props.tryFind<TerminalElement> "tab.view" |> Option.iter (fun _ -> element.View <- Unchecked.defaultof<_>)

    override _.name = $"Tab"

    member _.setProps (element: Tab, props: Props) =
        base.setProps(element, props)

        // Properties
        props |> Props.tryFind<string> "tab.displayText" |> Option.iter (fun v -> element.DisplayText <- v )

        props |> Props.tryFind<TerminalElement> "tab.view"
        |> Option.iter (fun v ->
            v.initialize (Some element)
            element.View <- v.element
        )


    override this.initialize parent =
        #if DEBUG
        Diagnostics.Trace.WriteLine $"{this.name} created!"
        #endif
        this.parent <- parent


        let el = new Tab()
        parent |> Option.iter (fun p -> p.Add el |> ignore)
        this.setProps(el, props)
        props |> Props.tryFind<View->unit> "ref" |> Option.iter (fun v -> v el)
        this.element <- el



    override this.canUpdate prevElement oldProps =
        let changedProps,removedProps = Interop.filterProps oldProps props
        let canUpdateView = ViewElement.canUpdate prevElement changedProps removedProps
        let canUpdateElement =
            true

        canUpdateView && canUpdateElement



    override this.update prevElement oldProps =
        let element = prevElement :?> Tab
        let changedProps,removedProps = Interop.filterProps oldProps props
        ViewElement.removeProps prevElement removedProps
        removeProps element removedProps
        this.setProps(element, changedProps)
        this.element <- prevElement



// TabView
type TabViewElement(props:Props) =
    inherit TerminalElement(props)


    let removeProps (element: TabView) (props: Props) =
        // Properties
        props |> Props.tryFind<int> "tabView.maxTabTextWidth" |> Option.iter (fun _ -> element.MaxTabTextWidth <- Unchecked.defaultof<_>)
        props |> Props.tryFind<Tab> "tabView.selectedTab" |> Option.iter (fun _ -> element.SelectedTab <- Unchecked.defaultof<_>)
        props |> Props.tryFind<TabStyle> "tabView.style" |> Option.iter (fun _ -> element.Style <- Unchecked.defaultof<_>)
        props |> Props.tryFind<Int32> "tabView.tabScrollOffset" |> Option.iter (fun _ -> element.TabScrollOffset <- Unchecked.defaultof<_>)
        // Events
        props |> Props.tryFind<TabChangedEventArgs->unit> "tabView.selectedTabChanged" |> Option.iter (fun _ -> Interop.removeEventHandler <@ element.SelectedTabChanged @> element)
        props |> Props.tryFind<TabMouseEventArgs->unit> "tabView.tabClicked" |> Option.iter (fun _ -> Interop.removeEventHandler <@ element.TabClicked @> element)

    override _.name = $"TabView"

    member _.setProps (element: TabView, props: Props) =
        base.setProps(element, props)

        // Properties
        props |> Props.tryFind<int> "tabView.maxTabTextWidth" |> Option.iter (fun v -> element.MaxTabTextWidth <- (v |> uint32))
        props |> Props.tryFind<Tab> "tabView.selectedTab" |> Option.iter (fun v -> element.SelectedTab <- v )
        props |> Props.tryFind<TabStyle> "tabView.style" |> Option.iter (fun v -> element.Style <- v )
        props |> Props.tryFind<Int32> "tabView.tabScrollOffset" |> Option.iter (fun v -> element.TabScrollOffset <- v )
        // Events
        props |> Props.tryFind<TabChangedEventArgs->unit> "tabView.selectedTabChanged" |> Option.iter (fun v -> Interop.setEventHandler <@ element.SelectedTabChanged @> v element)
        props |> Props.tryFind<TabMouseEventArgs->unit> "tabView.tabClicked" |> Option.iter (fun v -> Interop.setEventHandler <@ element.TabClicked @> v element)

            // Additional properties
        props |> Props.tryFind<TerminalElement list> "tabView.tabs" |> Option.iter (fun v ->
            v
            |> List.iter (fun tabItems ->
                tabItems.initialize (Some element)
                element.AddTab ((tabItems.element :?> Tab), false)

                )
            )


    override this.initialize parent =
        #if DEBUG
        Diagnostics.Trace.WriteLine $"{this.name} created!"
        #endif
        this.parent <- parent


        let el = new TabView()
        parent |> Option.iter (fun p -> p.Add el |> ignore)
        this.setProps(el, props)
        props |> Props.tryFind<View->unit> "ref" |> Option.iter (fun v -> v el)
        this.element <- el



    override this.canUpdate prevElement oldProps =
        let changedProps,removedProps = Interop.filterProps oldProps props
        let canUpdateView = ViewElement.canUpdate prevElement changedProps removedProps
        let canUpdateElement =
            true

        canUpdateView && canUpdateElement



    override this.update prevElement oldProps =
        let element = prevElement :?> TabView
        let changedProps,removedProps = Interop.filterProps oldProps props
        ViewElement.removeProps prevElement removedProps
        removeProps element removedProps
        this.setProps(element, changedProps)
        this.element <- prevElement



// TableView
type TableViewElement(props:Props) =
    inherit TerminalElement(props)

    let removeProps (element: TableView) (props: Props) =
        // Properties
        props |> Props.tryFind<KeyCode> "tableView.cellActivationKey" |> Option.iter (fun _ -> element.CellActivationKey <- Unchecked.defaultof<_>)
        props |> Props.tryFind<ICollectionNavigator> "tableView.collectionNavigator" |> Option.iter (fun _ -> element.CollectionNavigator <- Unchecked.defaultof<_>)
        props |> Props.tryFind<Int32> "tableView.columnOffset" |> Option.iter (fun _ -> element.ColumnOffset <- Unchecked.defaultof<_>)
        props |> Props.tryFind<bool> "tableView.fullRowSelect" |> Option.iter (fun _ -> element.FullRowSelect <- Unchecked.defaultof<_>)
        props |> Props.tryFind<Int32> "tableView.maxCellWidth" |> Option.iter (fun _ -> element.MaxCellWidth <- Unchecked.defaultof<_>)
        props |> Props.tryFind<Int32> "tableView.minCellWidth" |> Option.iter (fun _ -> element.MinCellWidth <- Unchecked.defaultof<_>)
        props |> Props.tryFind<bool> "tableView.multiSelect" |> Option.iter (fun _ -> element.MultiSelect <- Unchecked.defaultof<_>)
        props |> Props.tryFind<string> "tableView.nullSymbol" |> Option.iter (fun _ -> element.NullSymbol <- Unchecked.defaultof<_>)
        props |> Props.tryFind<Int32> "tableView.rowOffset" |> Option.iter (fun _ -> element.RowOffset <- Unchecked.defaultof<_>)
        props |> Props.tryFind<Int32> "tableView.selectedColumn" |> Option.iter (fun _ -> element.SelectedColumn <- Unchecked.defaultof<_>)
        props |> Props.tryFind<Int32> "tableView.selectedRow" |> Option.iter (fun _ -> element.SelectedRow <- Unchecked.defaultof<_>)
        props |> Props.tryFind<Char> "tableView.separatorSymbol" |> Option.iter (fun _ -> element.SeparatorSymbol <- Unchecked.defaultof<_>)
        props |> Props.tryFind<TableStyle> "tableView.style" |> Option.iter (fun _ -> element.Style <- Unchecked.defaultof<_>)
        props |> Props.tryFind<ITableSource> "tableView.table" |> Option.iter (fun _ -> element.Table <- Unchecked.defaultof<_>)
        // Events
        props |> Props.tryFind<CellActivatedEventArgs->unit> "tableView.cellActivated" |> Option.iter (fun _ -> Interop.removeEventHandler <@ element.CellActivated @> element)
        props |> Props.tryFind<CellToggledEventArgs->unit> "tableView.cellToggled" |> Option.iter (fun _ -> Interop.removeEventHandler <@ element.CellToggled @> element)
        props |> Props.tryFind<SelectedCellChangedEventArgs->unit> "tableView.selectedCellChanged" |> Option.iter (fun _ -> Interop.removeEventHandler <@ element.SelectedCellChanged @> element)

    override _.name = $"TableView"

    member _.setProps (element: TableView, props: Props) =
        base.setProps(element, props)

        // Properties
        props |> Props.tryFind<KeyCode> "tableView.cellActivationKey" |> Option.iter (fun v -> element.CellActivationKey <- v )
        props |> Props.tryFind<ICollectionNavigator> "tableView.collectionNavigator" |> Option.iter (fun v -> element.CollectionNavigator <- v )
        props |> Props.tryFind<Int32> "tableView.columnOffset" |> Option.iter (fun v -> element.ColumnOffset <- v )
        props |> Props.tryFind<bool> "tableView.fullRowSelect" |> Option.iter (fun v -> element.FullRowSelect <- v )
        props |> Props.tryFind<Int32> "tableView.maxCellWidth" |> Option.iter (fun v -> element.MaxCellWidth <- v )
        props |> Props.tryFind<Int32> "tableView.minCellWidth" |> Option.iter (fun v -> element.MinCellWidth <- v )
        props |> Props.tryFind<bool> "tableView.multiSelect" |> Option.iter (fun v -> element.MultiSelect <- v )
        props |> Props.tryFind<string> "tableView.nullSymbol" |> Option.iter (fun v -> element.NullSymbol <- v )
        props |> Props.tryFind<Int32> "tableView.rowOffset" |> Option.iter (fun v -> element.RowOffset <- v )
        props |> Props.tryFind<Int32> "tableView.selectedColumn" |> Option.iter (fun v -> element.SelectedColumn <- v )
        props |> Props.tryFind<Int32> "tableView.selectedRow" |> Option.iter (fun v -> element.SelectedRow <- v )
        props |> Props.tryFind<Char> "tableView.separatorSymbol" |> Option.iter (fun v -> element.SeparatorSymbol <- v )
        props |> Props.tryFind<TableStyle> "tableView.style" |> Option.iter (fun v -> element.Style <- v )
        props |> Props.tryFind<ITableSource> "tableView.table" |> Option.iter (fun v -> element.Table <- v )
        // Events
        props |> Props.tryFind<CellActivatedEventArgs->unit> "tableView.cellActivated" |> Option.iter (fun v -> Interop.setEventHandler <@ element.CellActivated @> v element)
        props |> Props.tryFind<CellToggledEventArgs->unit> "tableView.cellToggled" |> Option.iter (fun v -> Interop.setEventHandler <@ element.CellToggled @> v element)
        props |> Props.tryFind<SelectedCellChangedEventArgs->unit> "tableView.selectedCellChanged" |> Option.iter (fun v -> Interop.setEventHandler <@ element.SelectedCellChanged @> v element)


    override this.initialize parent =
        #if DEBUG
        Diagnostics.Trace.WriteLine $"{this.name} created!"
        #endif
        this.parent <- parent


        let el = new TableView()
        parent |> Option.iter (fun p -> p.Add el |> ignore)
        this.setProps(el, props)
        props |> Props.tryFind<View->unit> "ref" |> Option.iter (fun v -> v el)
        this.element <- el



    override this.canUpdate prevElement oldProps =
        let changedProps,removedProps = Interop.filterProps oldProps props
        let canUpdateView = ViewElement.canUpdate prevElement changedProps removedProps
        let canUpdateElement =
            true

        canUpdateView && canUpdateElement



    override this.update prevElement oldProps =
        let element = prevElement :?> TableView
        let changedProps,removedProps = Interop.filterProps oldProps props
        ViewElement.removeProps prevElement removedProps
        removeProps element removedProps
        this.setProps(element, changedProps)
        this.element <- prevElement



// TextField
type TextFieldElement(props:Props) =
    inherit TerminalElement(props)

    let removeProps (element: TextField) (props: Props) =
        // Properties
        props |> Props.tryFind<IAutocomplete> "textField.autocomplete" |> Option.iter (fun _ -> element.Autocomplete <- Unchecked.defaultof<_>)
        props |> Props.tryFind<string> "textField.caption" |> Option.iter (fun _ -> element.Caption <- Unchecked.defaultof<_>)
        props |> Props.tryFind<Terminal.Gui.Drawing.Color> "textField.captionColor" |> Option.iter (fun _ -> element.CaptionColor <- Unchecked.defaultof<_>)
        props |> Props.tryFind<Int32> "textField.cursorPosition" |> Option.iter (fun _ -> element.CursorPosition <- Unchecked.defaultof<_>)
        props |> Props.tryFind<bool> "textField.readOnly" |> Option.iter (fun _ -> element.ReadOnly <- Unchecked.defaultof<_>)
        props |> Props.tryFind<bool> "textField.secret" |> Option.iter (fun _ -> element.Secret <- Unchecked.defaultof<_>)
        props |> Props.tryFind<Int32> "textField.selectedStart" |> Option.iter (fun _ -> element.SelectedStart <- Unchecked.defaultof<_>)
        props |> Props.tryFind<bool> "textField.selectWordOnlyOnDoubleClick" |> Option.iter (fun _ -> element.SelectWordOnlyOnDoubleClick <- Unchecked.defaultof<_>)
        props |> Props.tryFind<string> "textField.text" |> Option.iter (fun _ -> element.Text <- Unchecked.defaultof<_>)
        props |> Props.tryFind<bool> "textField.used" |> Option.iter (fun _ -> element.Used <- Unchecked.defaultof<_>)
        props |> Props.tryFind<bool> "textField.useSameRuneTypeForWords" |> Option.iter (fun _ -> element.UseSameRuneTypeForWords <- Unchecked.defaultof<_>)
        // Events
        props |> Props.tryFind<ResultEventArgs<string>->unit> "textField.textChanging" |> Option.iter (fun _ -> Interop.removeEventHandler <@ element.TextChanging @> element)

    override _.name = $"TextField"

    member _.setProps (element: TextField, props: Props) =
        base.setProps(element, props)

        // Properties
        props |> Props.tryFind<IAutocomplete> "textField.autocomplete" |> Option.iter (fun v -> element.Autocomplete <- v )
        props |> Props.tryFind<string> "textField.caption" |> Option.iter (fun v -> element.Caption <- v )
        props |> Props.tryFind<Terminal.Gui.Drawing.Color> "textField.captionColor" |> Option.iter (fun v -> element.CaptionColor <- v )
        props |> Props.tryFind<Int32> "textField.cursorPosition" |> Option.iter (fun v -> element.CursorPosition <- v )
        props |> Props.tryFind<bool> "textField.readOnly" |> Option.iter (fun v -> element.ReadOnly <- v )
        props |> Props.tryFind<bool> "textField.secret" |> Option.iter (fun v -> element.Secret <- v )
        props |> Props.tryFind<Int32> "textField.selectedStart" |> Option.iter (fun v -> element.SelectedStart <- v )
        props |> Props.tryFind<bool> "textField.selectWordOnlyOnDoubleClick" |> Option.iter (fun v -> element.SelectWordOnlyOnDoubleClick <- v )
        props |> Props.tryFind<string> "textField.text" |> Option.iter (fun v -> element.Text <- v )
        props |> Props.tryFind<bool> "textField.used" |> Option.iter (fun v -> element.Used <- v )
        props |> Props.tryFind<bool> "textField.useSameRuneTypeForWords" |> Option.iter (fun v -> element.UseSameRuneTypeForWords <- v )
        // Events
        props |> Props.tryFind<ResultEventArgs<string>->unit> "textField.textChanging" |> Option.iter (fun v -> Interop.setEventHandler <@ element.TextChanging @> v element)


    override this.initialize parent =
        #if DEBUG
        Diagnostics.Trace.WriteLine $"{this.name} created!"
        #endif
        this.parent <- parent


        let el = new TextField()
        parent |> Option.iter (fun p -> p.Add el |> ignore)
        this.setProps(el, props)
        props |> Props.tryFind<View->unit> "ref" |> Option.iter (fun v -> v el)
        this.element <- el



    override this.canUpdate prevElement oldProps =
        let changedProps,removedProps = Interop.filterProps oldProps props
        let canUpdateView = ViewElement.canUpdate prevElement changedProps removedProps
        let canUpdateElement =
            true

        canUpdateView && canUpdateElement



    override this.update prevElement oldProps =
        let element = prevElement :?> TextField
        let changedProps,removedProps = Interop.filterProps oldProps props
        ViewElement.removeProps prevElement removedProps
        removeProps element removedProps
        this.setProps(element, changedProps)
        this.element <- prevElement



// TextValidateField
type TextValidateFieldElement(props:Props) =
    inherit TerminalElement(props)

    let removeProps (element: TextValidateField) (props: Props) =
        // Properties
        props |> Props.tryFind<ITextValidateProvider> "textValidateField.provider" |> Option.iter (fun _ -> element.Provider <- Unchecked.defaultof<_>)
        props |> Props.tryFind<string> "textValidateField.text" |> Option.iter (fun _ -> element.Text <- Unchecked.defaultof<_>)

    override _.name = $"TextValidateField"

    member _.setProps (element: TextValidateField, props: Props) =
        base.setProps(element, props)

        // Properties
        props |> Props.tryFind<ITextValidateProvider> "textValidateField.provider" |> Option.iter (fun v -> element.Provider <- v )
        props |> Props.tryFind<string> "textValidateField.text" |> Option.iter (fun v -> element.Text <- v )


    override this.initialize parent =
        #if DEBUG
        Diagnostics.Trace.WriteLine $"{this.name} created!"
        #endif
        this.parent <- parent


        let el = new TextValidateField()
        parent |> Option.iter (fun p -> p.Add el |> ignore)
        this.setProps(el, props)
        props |> Props.tryFind<View->unit> "ref" |> Option.iter (fun v -> v el)
        this.element <- el



    override this.canUpdate prevElement oldProps =
        let changedProps,removedProps = Interop.filterProps oldProps props
        let canUpdateView = ViewElement.canUpdate prevElement changedProps removedProps
        let canUpdateElement =
            true

        canUpdateView && canUpdateElement



    override this.update prevElement oldProps =
        let element = prevElement :?> TextValidateField
        let changedProps,removedProps = Interop.filterProps oldProps props
        ViewElement.removeProps prevElement removedProps
        removeProps element removedProps
        this.setProps(element, changedProps)
        this.element <- prevElement



// TextView
type TextViewElement(props:Props) =
    inherit TerminalElement(props)


    let removeProps (element: TextView) (props: Props) =
        // Properties
        props |> Props.tryFind<bool> "textView.allowsReturn" |> Option.iter (fun _ -> element.AllowsReturn <- Unchecked.defaultof<_>)
        props |> Props.tryFind<bool> "textView.allowsTab" |> Option.iter (fun _ -> element.AllowsTab <- Unchecked.defaultof<_>)
        props |> Props.tryFind<Point> "textView.cursorPosition" |> Option.iter (fun _ -> element.CursorPosition <- Unchecked.defaultof<_>)
        props |> Props.tryFind<bool> "textView.inheritsPreviousAttribute" |> Option.iter (fun _ -> element.InheritsPreviousAttribute <- Unchecked.defaultof<_>)
        props |> Props.tryFind<bool> "textView.isDirty" |> Option.iter (fun _ -> element.IsDirty <- Unchecked.defaultof<_>)
        props |> Props.tryFind<bool> "textView.isSelecting" |> Option.iter (fun _ -> element.IsSelecting <- Unchecked.defaultof<_>)
        props |> Props.tryFind<Int32> "textView.leftColumn" |> Option.iter (fun _ -> element.LeftColumn <- Unchecked.defaultof<_>)
        props |> Props.tryFind<bool> "textView.multiline" |> Option.iter (fun _ -> element.Multiline <- Unchecked.defaultof<_>)
        props |> Props.tryFind<bool> "textView.readOnly" |> Option.iter (fun _ -> element.ReadOnly <- Unchecked.defaultof<_>)
        props |> Props.tryFind<Int32> "textView.selectionStartColumn" |> Option.iter (fun _ -> element.SelectionStartColumn <- Unchecked.defaultof<_>)
        props |> Props.tryFind<Int32> "textView.selectionStartRow" |> Option.iter (fun _ -> element.SelectionStartRow <- Unchecked.defaultof<_>)
        props |> Props.tryFind<bool> "textView.selectWordOnlyOnDoubleClick" |> Option.iter (fun _ -> element.SelectWordOnlyOnDoubleClick <- Unchecked.defaultof<_>)
        props |> Props.tryFind<Int32> "textView.tabWidth" |> Option.iter (fun _ -> element.TabWidth <- Unchecked.defaultof<_>)
        props |> Props.tryFind<string> "textView.text" |> Option.iter (fun _ -> element.Text <- Unchecked.defaultof<_>)
        props |> Props.tryFind<Int32> "textView.topRow" |> Option.iter (fun _ -> element.TopRow <- Unchecked.defaultof<_>)
        props |> Props.tryFind<bool> "textView.used" |> Option.iter (fun _ -> element.Used <- Unchecked.defaultof<_>)
        props |> Props.tryFind<bool> "textView.useSameRuneTypeForWords" |> Option.iter (fun _ -> element.UseSameRuneTypeForWords <- Unchecked.defaultof<_>)
        props |> Props.tryFind<bool> "textView.wordWrap" |> Option.iter (fun _ -> element.WordWrap <- Unchecked.defaultof<_>)
        // Events
        props |> Props.tryFind<ContentsChangedEventArgs->unit> "textView.contentsChanged" |> Option.iter (fun _ -> Interop.removeEventHandler <@ element.ContentsChanged @> element)
        props |> Props.tryFind<CellEventArgs->unit> "textView.drawNormalColor" |> Option.iter (fun _ -> Interop.removeEventHandler <@ element.DrawNormalColor @> element)
        props |> Props.tryFind<CellEventArgs->unit> "textView.drawReadOnlyColor" |> Option.iter (fun _ -> Interop.removeEventHandler <@ element.DrawReadOnlyColor @> element)
        props |> Props.tryFind<CellEventArgs->unit> "textView.drawSelectionColor" |> Option.iter (fun _ -> Interop.removeEventHandler <@ element.DrawSelectionColor @> element)
        props |> Props.tryFind<CellEventArgs->unit> "textView.drawUsedColor" |> Option.iter (fun _ -> Interop.removeEventHandler <@ element.DrawUsedColor @> element)
        props |> Props.tryFind<Point->unit> "textView.unwrappedCursorPosition" |> Option.iter (fun _ -> Interop.removeEventHandler <@ element.UnwrappedCursorPosition @> element)

            // Additional properties
        props |> Props.tryFind<string->unit> "textView.textChanged" |> Option.iter (fun _ -> Interop.removeEventHandler <@ element.ContentsChanged @> element)


    override _.name = $"TextView"

    member _.setProps (element: TextView, props: Props) =
        base.setProps(element, props)

        // Properties
        props |> Props.tryFind<bool> "textView.allowsReturn" |> Option.iter (fun v -> element.AllowsReturn <- v )
        props |> Props.tryFind<bool> "textView.allowsTab" |> Option.iter (fun v -> element.AllowsTab <- v )
        props |> Props.tryFind<Point> "textView.cursorPosition" |> Option.iter (fun v -> element.CursorPosition <- v )
        props |> Props.tryFind<bool> "textView.inheritsPreviousAttribute" |> Option.iter (fun v -> element.InheritsPreviousAttribute <- v )
        props |> Props.tryFind<bool> "textView.isDirty" |> Option.iter (fun v -> element.IsDirty <- v )
        props |> Props.tryFind<bool> "textView.isSelecting" |> Option.iter (fun v -> element.IsSelecting <- v )
        props |> Props.tryFind<Int32> "textView.leftColumn" |> Option.iter (fun v -> element.LeftColumn <- v )
        props |> Props.tryFind<bool> "textView.multiline" |> Option.iter (fun v -> element.Multiline <- v )
        props |> Props.tryFind<bool> "textView.readOnly" |> Option.iter (fun v -> element.ReadOnly <- v )
        props |> Props.tryFind<Int32> "textView.selectionStartColumn" |> Option.iter (fun v -> element.SelectionStartColumn <- v )
        props |> Props.tryFind<Int32> "textView.selectionStartRow" |> Option.iter (fun v -> element.SelectionStartRow <- v )
        props |> Props.tryFind<bool> "textView.selectWordOnlyOnDoubleClick" |> Option.iter (fun v -> element.SelectWordOnlyOnDoubleClick <- v )
        props |> Props.tryFind<Int32> "textView.tabWidth" |> Option.iter (fun v -> element.TabWidth <- v )
        props |> Props.tryFind<string> "textView.text" |> Option.iter (fun v -> element.Text <- v )
        props |> Props.tryFind<Int32> "textView.topRow" |> Option.iter (fun v -> element.TopRow <- v )
        props |> Props.tryFind<bool> "textView.used" |> Option.iter (fun v -> element.Used <- v )
        props |> Props.tryFind<bool> "textView.useSameRuneTypeForWords" |> Option.iter (fun v -> element.UseSameRuneTypeForWords <- v )
        props |> Props.tryFind<bool> "textView.wordWrap" |> Option.iter (fun v -> element.WordWrap <- v )
        // Events
        props |> Props.tryFind<ContentsChangedEventArgs->unit> "textView.contentsChanged" |> Option.iter (fun v -> Interop.setEventHandler <@ element.ContentsChanged @> v element)
        props |> Props.tryFind<CellEventArgs->unit> "textView.drawNormalColor" |> Option.iter (fun v -> Interop.setEventHandler <@ element.DrawNormalColor @> v element)
        props |> Props.tryFind<CellEventArgs->unit> "textView.drawReadOnlyColor" |> Option.iter (fun v -> Interop.setEventHandler <@ element.DrawReadOnlyColor @> v element)
        props |> Props.tryFind<CellEventArgs->unit> "textView.drawSelectionColor" |> Option.iter (fun v -> Interop.setEventHandler <@ element.DrawSelectionColor @> v element)
        props |> Props.tryFind<CellEventArgs->unit> "textView.drawUsedColor" |> Option.iter (fun v -> Interop.setEventHandler <@ element.DrawUsedColor @> v element)
        props |> Props.tryFind<Point->unit> "textView.unwrappedCursorPosition" |> Option.iter (fun v -> Interop.setEventHandler <@ element.UnwrappedCursorPosition @> v element)

            // Additional properties
        props |> Props.tryFind<string->unit> "textView.textChanged" |> Option.iter (fun v -> Interop.setEventHandler <@ element.ContentsChanged @> (fun _ -> v element.Text) element)


    override this.initialize parent =
        #if DEBUG
        Diagnostics.Trace.WriteLine $"{this.name} created!"
        #endif
        this.parent <- parent


        let el = new TextView()
        parent |> Option.iter (fun p -> p.Add el |> ignore)
        this.setProps(el, props)
        props |> Props.tryFind<View->unit> "ref" |> Option.iter (fun v -> v el)
        this.element <- el



    override this.canUpdate prevElement oldProps =
        let changedProps,removedProps = Interop.filterProps oldProps props
        let canUpdateView = ViewElement.canUpdate prevElement changedProps removedProps
        let canUpdateElement =
            true

        canUpdateView && canUpdateElement



    override this.update prevElement oldProps =
        let element = prevElement :?> TextView
        let changedProps,removedProps = Interop.filterProps oldProps props
        ViewElement.removeProps prevElement removedProps
        removeProps element removedProps
        this.setProps(element, changedProps)
        this.element <- prevElement



// TileView
type TileViewElement(props:Props) =
    inherit TerminalElement(props)

    let removeProps (element: TileView) (props: Props) =
        // Properties
        props |> Props.tryFind<LineStyle> "tileView.lineStyle" |> Option.iter (fun _ -> element.LineStyle <- Unchecked.defaultof<_>)
        props |> Props.tryFind<Orientation> "tileView.orientation" |> Option.iter (fun _ -> element.Orientation <- Unchecked.defaultof<_>)
        props |> Props.tryFind<KeyCode> "tileView.toggleResizable" |> Option.iter (fun _ -> element.ToggleResizable <- Unchecked.defaultof<_>)
        // Events
        props |> Props.tryFind<SplitterEventArgs->unit> "tileView.splitterMoved" |> Option.iter (fun _ -> Interop.removeEventHandler <@ element.SplitterMoved @> element)

    override _.name = $"TileView"

    member _.setProps (element: TileView, props: Props) =
        base.setProps(element, props)

        // Properties
        props |> Props.tryFind<LineStyle> "tileView.lineStyle" |> Option.iter (fun v -> element.LineStyle <- v )
        props |> Props.tryFind<Orientation> "tileView.orientation" |> Option.iter (fun v -> element.Orientation <- v )
        props |> Props.tryFind<KeyCode> "tileView.toggleResizable" |> Option.iter (fun v -> element.ToggleResizable <- v )
        // Events
        props |> Props.tryFind<SplitterEventArgs->unit> "tileView.splitterMoved" |> Option.iter (fun v -> Interop.setEventHandler <@ element.SplitterMoved @> v element)


    override this.initialize parent =
        #if DEBUG
        Diagnostics.Trace.WriteLine $"{this.name} created!"
        #endif
        this.parent <- parent


        let el = new TileView()
        parent |> Option.iter (fun p -> p.Add el |> ignore)
        this.setProps(el, props)
        props |> Props.tryFind<View->unit> "ref" |> Option.iter (fun v -> v el)
        this.element <- el



    override this.canUpdate prevElement oldProps =
        let changedProps,removedProps = Interop.filterProps oldProps props
        let canUpdateView = ViewElement.canUpdate prevElement changedProps removedProps
        let canUpdateElement =
            true

        canUpdateView && canUpdateElement



    override this.update prevElement oldProps =
        let element = prevElement :?> TileView
        let changedProps,removedProps = Interop.filterProps oldProps props
        ViewElement.removeProps prevElement removedProps
        removeProps element removedProps
        this.setProps(element, changedProps)
        this.element <- prevElement



// TimeField
type TimeFieldElement(props:Props) =
    inherit TerminalElement(props)

    let removeProps (element: TimeField) (props: Props) =
        // Properties
        props |> Props.tryFind<Int32> "timeField.cursorPosition" |> Option.iter (fun _ -> element.CursorPosition <- Unchecked.defaultof<_>)
        props |> Props.tryFind<bool> "timeField.isShortFormat" |> Option.iter (fun _ -> element.IsShortFormat <- Unchecked.defaultof<_>)
        props |> Props.tryFind<TimeSpan> "timeField.time" |> Option.iter (fun _ -> element.Time <- Unchecked.defaultof<_>)
        // Events
        props |> Props.tryFind<DateTimeEventArgs<TimeSpan>->unit> "timeField.timeChanged" |> Option.iter (fun _ -> Interop.removeEventHandler <@ element.TimeChanged @> element)

    override _.name = $"TimeField"

    member _.setProps (element: TimeField, props: Props) =
        base.setProps(element, props)

        // Properties
        props |> Props.tryFind<Int32> "timeField.cursorPosition" |> Option.iter (fun v -> element.CursorPosition <- v )
        props |> Props.tryFind<bool> "timeField.isShortFormat" |> Option.iter (fun v -> element.IsShortFormat <- v )
        props |> Props.tryFind<TimeSpan> "timeField.time" |> Option.iter (fun v -> element.Time <- v )
        // Events
        props |> Props.tryFind<DateTimeEventArgs<TimeSpan>->unit> "timeField.timeChanged" |> Option.iter (fun v -> Interop.setEventHandler <@ element.TimeChanged @> v element)


    override this.initialize parent =
        #if DEBUG
        Diagnostics.Trace.WriteLine $"{this.name} created!"
        #endif
        this.parent <- parent


        let el = new TimeField()
        parent |> Option.iter (fun p -> p.Add el |> ignore)
        this.setProps(el, props)
        props |> Props.tryFind<View->unit> "ref" |> Option.iter (fun v -> v el)
        this.element <- el



    override this.canUpdate prevElement oldProps =
        let changedProps,removedProps = Interop.filterProps oldProps props
        let canUpdateView = ViewElement.canUpdate prevElement changedProps removedProps
        let canUpdateElement =
            true

        canUpdateView && canUpdateElement



    override this.update prevElement oldProps =
        let element = prevElement :?> TimeField
        let changedProps,removedProps = Interop.filterProps oldProps props
        ViewElement.removeProps prevElement removedProps
        removeProps element removedProps
        this.setProps(element, changedProps)
        this.element <- prevElement



// Toplevel
type ToplevelElement(props:Props) =
    inherit TerminalElement(props)

    let removeProps (element: Toplevel) (props: Props) =
        // Properties
        props |> Props.tryFind<bool> "toplevel.modal" |> Option.iter (fun _ -> element.Modal <- Unchecked.defaultof<_>)
        props |> Props.tryFind<bool> "toplevel.running" |> Option.iter (fun _ -> element.Running <- Unchecked.defaultof<_>)
        // Events
        props |> Props.tryFind<ToplevelEventArgs->unit> "toplevel.activate" |> Option.iter (fun _ -> Interop.removeEventHandler <@ element.Activate @> element)
        props |> Props.tryFind<ToplevelEventArgs->unit> "toplevel.closed" |> Option.iter (fun _ -> Interop.removeEventHandler <@ element.Closed @> element)
        props |> Props.tryFind<ToplevelClosingEventArgs->unit> "toplevel.closing" |> Option.iter (fun _ -> Interop.removeEventHandler <@ element.Closing @> element)
        props |> Props.tryFind<ToplevelEventArgs->unit> "toplevel.deactivate" |> Option.iter (fun _ -> Interop.removeEventHandler <@ element.Deactivate @> element)
        props |> Props.tryFind<unit->unit> "toplevel.loaded" |> Option.iter (fun _ -> Interop.removeEventHandler <@ element.Loaded @> element)
        props |> Props.tryFind<unit->unit> "toplevel.ready" |> Option.iter (fun _ -> Interop.removeEventHandler <@ element.Ready @> element)
        props |> Props.tryFind<SizeChangedEventArgs->unit> "toplevel.sizeChanging" |> Option.iter (fun _ -> Interop.removeEventHandler <@ element.SizeChanging @> element)
        props |> Props.tryFind<unit->unit> "toplevel.unloaded" |> Option.iter (fun _ -> Interop.removeEventHandler <@ element.Unloaded @> element)

    override _.name = $"Toplevel"

    member _.setProps (element: Toplevel, props: Props) =
        base.setProps(element, props)

        // Properties
        props |> Props.tryFind<bool> "toplevel.modal" |> Option.iter (fun v -> element.Modal <- v )
        props |> Props.tryFind<bool> "toplevel.running" |> Option.iter (fun v -> element.Running <- v )
        // Events
        props |> Props.tryFind<ToplevelEventArgs->unit> "toplevel.activate" |> Option.iter (fun v -> Interop.setEventHandler <@ element.Activate @> v element)
        props |> Props.tryFind<ToplevelEventArgs->unit> "toplevel.closed" |> Option.iter (fun v -> Interop.setEventHandler <@ element.Closed @> v element)
        props |> Props.tryFind<ToplevelClosingEventArgs->unit> "toplevel.closing" |> Option.iter (fun v -> Interop.setEventHandler <@ element.Closing @> v element)
        props |> Props.tryFind<ToplevelEventArgs->unit> "toplevel.deactivate" |> Option.iter (fun v -> Interop.setEventHandler <@ element.Deactivate @> v element)
        props |> Props.tryFind<unit->unit> "toplevel.loaded" |> Option.iter (fun v -> Interop.setEventHandler <@ element.Loaded @> (fun _ -> v()) element)
        props |> Props.tryFind<unit->unit> "toplevel.ready" |> Option.iter (fun v -> Interop.setEventHandler <@ element.Ready @> (fun _ -> v()) element)
        props |> Props.tryFind<SizeChangedEventArgs->unit> "toplevel.sizeChanging" |> Option.iter (fun v -> Interop.setEventHandler <@ element.SizeChanging @> v element)
        props |> Props.tryFind<unit->unit> "toplevel.unloaded" |> Option.iter (fun v -> Interop.setEventHandler <@ element.Unloaded @> (fun _ -> v()) element)


    override this.initialize parent =
        #if DEBUG
        Diagnostics.Trace.WriteLine $"{this.name} created!"
        #endif
        this.parent <- parent


        let el = new Toplevel()
        parent |> Option.iter (fun p -> p.Add el |> ignore)
        this.setProps(el, props)
        props |> Props.tryFind<View->unit> "ref" |> Option.iter (fun v -> v el)
        this.element <- el



    override this.canUpdate prevElement oldProps =
        let changedProps,removedProps = Interop.filterProps oldProps props
        let canUpdateView = ViewElement.canUpdate prevElement changedProps removedProps
        let canUpdateElement =
            true

        canUpdateView && canUpdateElement



    override this.update prevElement oldProps =
        let element = prevElement :?> Toplevel
        let changedProps,removedProps = Interop.filterProps oldProps props
        ViewElement.removeProps prevElement removedProps
        removeProps element removedProps
        this.setProps(element, changedProps)
        this.element <- prevElement



// TreeView<'a when 'a : not struct>
type TreeViewElement<'a when 'a : not struct>(props:Props) =
    inherit TerminalElement(props)

    let removeProps (element: TreeView<'a>) (props: Props) =
        // Properties
        props |> Props.tryFind<bool> "treeView`1.allowLetterBasedNavigation" |> Option.iter (fun _ -> element.AllowLetterBasedNavigation <- Unchecked.defaultof<_>)
        props |> Props.tryFind<AspectGetterDelegate<'a>> "treeView`1.aspectGetter" |> Option.iter (fun _ -> element.AspectGetter <- Unchecked.defaultof<_>)
        props |> Props.tryFind<Func<'a,Scheme>> "treeView`1.colorGetter" |> Option.iter (fun _ -> element.ColorGetter <- Unchecked.defaultof<_>)
        props |> Props.tryFind<Int32> "treeView`1.maxDepth" |> Option.iter (fun _ -> element.MaxDepth <- Unchecked.defaultof<_>)
        props |> Props.tryFind<bool> "treeView`1.multiSelect" |> Option.iter (fun _ -> element.MultiSelect <- Unchecked.defaultof<_>)
        props |> Props.tryFind<MouseFlags option> "treeView`1.objectActivationButton" |> Option.iter (fun _ -> element.ObjectActivationButton <- Unchecked.defaultof<_>)
        props |> Props.tryFind<KeyCode> "treeView`1.objectActivationKey" |> Option.iter (fun _ -> element.ObjectActivationKey <- Unchecked.defaultof<_>)
        props |> Props.tryFind<Int32> "treeView`1.scrollOffsetHorizontal" |> Option.iter (fun _ -> element.ScrollOffsetHorizontal <- Unchecked.defaultof<_>)
        props |> Props.tryFind<Int32> "treeView`1.scrollOffsetVertical" |> Option.iter (fun _ -> element.ScrollOffsetVertical <- Unchecked.defaultof<_>)
        props |> Props.tryFind<'a> "treeView`1.selectedObject" |> Option.iter (fun _ -> element.SelectedObject <- Unchecked.defaultof<_>)
        props |> Props.tryFind<TreeStyle> "treeView`1.style" |> Option.iter (fun _ -> element.Style <- Unchecked.defaultof<_>)
        props |> Props.tryFind<ITreeBuilder<'a>> "treeView`1.treeBuilder" |> Option.iter (fun _ -> element.TreeBuilder <- Unchecked.defaultof<_>)
        // Events
        props |> Props.tryFind<DrawTreeViewLineEventArgs<'a>->unit> "treeView`1.drawLine" |> Option.iter (fun _ -> Interop.removeEventHandler <@ element.DrawLine @> element)
        props |> Props.tryFind<ObjectActivatedEventArgs<'a>->unit> "treeView`1.objectActivated" |> Option.iter (fun _ -> Interop.removeEventHandler <@ element.ObjectActivated @> element)
        props |> Props.tryFind<SelectionChangedEventArgs<'a>->unit> "treeView`1.selectionChanged" |> Option.iter (fun _ -> Interop.removeEventHandler <@ element.SelectionChanged @> element)

    override _.name = $"TreeView<'a>"

    member _.setProps (element: TreeView<'a>, props: Props) =
        base.setProps(element, props)

        // Properties
        props |> Props.tryFind<bool> "treeView`1.allowLetterBasedNavigation" |> Option.iter (fun v -> element.AllowLetterBasedNavigation <- v )
        props |> Props.tryFind<AspectGetterDelegate<'a>> "treeView`1.aspectGetter" |> Option.iter (fun v -> element.AspectGetter <- v )
        props |> Props.tryFind<Func<'a,Scheme>> "treeView`1.colorGetter" |> Option.iter (fun v -> element.ColorGetter <- v )
        props |> Props.tryFind<Int32> "treeView`1.maxDepth" |> Option.iter (fun v -> element.MaxDepth <- v )
        props |> Props.tryFind<bool> "treeView`1.multiSelect" |> Option.iter (fun v -> element.MultiSelect <- v )
        props |> Props.tryFind<MouseFlags option> "treeView`1.objectActivationButton" |> Option.iter (fun v -> element.ObjectActivationButton <- v  |> Option.toNullable)
        props |> Props.tryFind<KeyCode> "treeView`1.objectActivationKey" |> Option.iter (fun v -> element.ObjectActivationKey <- v )
        props |> Props.tryFind<Int32> "treeView`1.scrollOffsetHorizontal" |> Option.iter (fun v -> element.ScrollOffsetHorizontal <- v )
        props |> Props.tryFind<Int32> "treeView`1.scrollOffsetVertical" |> Option.iter (fun v -> element.ScrollOffsetVertical <- v )
        props |> Props.tryFind<'a> "treeView`1.selectedObject" |> Option.iter (fun v -> element.SelectedObject <- v )
        props |> Props.tryFind<TreeStyle> "treeView`1.style" |> Option.iter (fun v -> element.Style <- v )
        props |> Props.tryFind<ITreeBuilder<'a>> "treeView`1.treeBuilder" |> Option.iter (fun v -> element.TreeBuilder <- v )
        // Events
        props |> Props.tryFind<DrawTreeViewLineEventArgs<'a>->unit> "treeView`1.drawLine" |> Option.iter (fun v -> Interop.setEventHandler <@ element.DrawLine @> v element)
        props |> Props.tryFind<ObjectActivatedEventArgs<'a>->unit> "treeView`1.objectActivated" |> Option.iter (fun v -> Interop.setEventHandler <@ element.ObjectActivated @> v element)
        props |> Props.tryFind<SelectionChangedEventArgs<'a>->unit> "treeView`1.selectionChanged" |> Option.iter (fun v -> Interop.setEventHandler <@ element.SelectionChanged @> v element)


    override this.initialize parent =
        #if DEBUG
        Diagnostics.Trace.WriteLine $"{this.name} created!"
        #endif
        this.parent <- parent


        let el = new TreeView<'a>()
        parent |> Option.iter (fun p -> p.Add el |> ignore)
        this.setProps(el, props)
        props |> Props.tryFind<View->unit> "ref" |> Option.iter (fun v -> v el)
        this.element <- el



    override this.canUpdate prevElement oldProps =
        let changedProps,removedProps = Interop.filterProps oldProps props
        let canUpdateView = ViewElement.canUpdate prevElement changedProps removedProps
        let canUpdateElement =
            true

        canUpdateView && canUpdateElement



    override this.update prevElement oldProps =
        let element = prevElement :?> TreeView<'a>
        let changedProps,removedProps = Interop.filterProps oldProps props
        ViewElement.removeProps prevElement removedProps
        removeProps element removedProps
        this.setProps(element, changedProps)
        this.element <- prevElement



// TreeView
type TreeViewElement(props:Props) =
    inherit TreeViewElement<ITreeNode>(props)

// Window
type WindowElement(props:Props) =
    inherit TerminalElement(props)

    let removeProps (element: Window) (props: Props) =
        // No properties or events Window
        ()

    override _.name = $"Window"

    member _.setProps (element: Window, props: Props) =
        base.setProps(element, props)

        // No properties or events Window
        ()


    override this.initialize parent =
        #if DEBUG
        Diagnostics.Trace.WriteLine $"{this.name} created!"
        #endif
        this.parent <- parent


        let el = new Window()
        parent |> Option.iter (fun p -> p.Add el |> ignore)
        this.setProps(el, props)
        props |> Props.tryFind<View->unit> "ref" |> Option.iter (fun v -> v el)
        this.element <- el



    override this.canUpdate prevElement oldProps =
        let changedProps,removedProps = Interop.filterProps oldProps props
        let canUpdateView = ViewElement.canUpdate prevElement changedProps removedProps
        let canUpdateElement =
            true

        canUpdateView && canUpdateElement



    override this.update prevElement oldProps =
        let element = prevElement :?> Window
        let changedProps,removedProps = Interop.filterProps oldProps props
        ViewElement.removeProps prevElement removedProps
        removeProps element removedProps
        this.setProps(element, changedProps)
        this.element <- prevElement



// Wizard
type WizardElement(props:Props) =
    inherit TerminalElement(props)

    let removeProps (element: Wizard) (props: Props) =
        // Properties
        props |> Props.tryFind<WizardStep> "wizard.currentStep" |> Option.iter (fun _ -> element.CurrentStep <- Unchecked.defaultof<_>)
        props |> Props.tryFind<bool> "wizard.modal" |> Option.iter (fun _ -> element.Modal <- Unchecked.defaultof<_>)
        // Events
        props |> Props.tryFind<WizardButtonEventArgs->unit> "wizard.cancelled" |> Option.iter (fun _ -> Interop.removeEventHandler <@ element.Cancelled @> element)
        props |> Props.tryFind<WizardButtonEventArgs->unit> "wizard.finished" |> Option.iter (fun _ -> Interop.removeEventHandler <@ element.Finished @> element)
        props |> Props.tryFind<WizardButtonEventArgs->unit> "wizard.movingBack" |> Option.iter (fun _ -> Interop.removeEventHandler <@ element.MovingBack @> element)
        props |> Props.tryFind<WizardButtonEventArgs->unit> "wizard.movingNext" |> Option.iter (fun _ -> Interop.removeEventHandler <@ element.MovingNext @> element)
        props |> Props.tryFind<StepChangeEventArgs->unit> "wizard.stepChanged" |> Option.iter (fun _ -> Interop.removeEventHandler <@ element.StepChanged @> element)
        props |> Props.tryFind<StepChangeEventArgs->unit> "wizard.stepChanging" |> Option.iter (fun _ -> Interop.removeEventHandler <@ element.StepChanging @> element)

    override _.name = $"Wizard"

    member _.setProps (element: Wizard, props: Props) =
        base.setProps(element, props)

        // Properties
        props |> Props.tryFind<WizardStep> "wizard.currentStep" |> Option.iter (fun v -> element.CurrentStep <- v )
        props |> Props.tryFind<bool> "wizard.modal" |> Option.iter (fun v -> element.Modal <- v )
        // Events
        props |> Props.tryFind<WizardButtonEventArgs->unit> "wizard.cancelled" |> Option.iter (fun v -> Interop.setEventHandler <@ element.Cancelled @> v element)
        props |> Props.tryFind<WizardButtonEventArgs->unit> "wizard.finished" |> Option.iter (fun v -> Interop.setEventHandler <@ element.Finished @> v element)
        props |> Props.tryFind<WizardButtonEventArgs->unit> "wizard.movingBack" |> Option.iter (fun v -> Interop.setEventHandler <@ element.MovingBack @> v element)
        props |> Props.tryFind<WizardButtonEventArgs->unit> "wizard.movingNext" |> Option.iter (fun v -> Interop.setEventHandler <@ element.MovingNext @> v element)
        props |> Props.tryFind<StepChangeEventArgs->unit> "wizard.stepChanged" |> Option.iter (fun v -> Interop.setEventHandler <@ element.StepChanged @> v element)
        props |> Props.tryFind<StepChangeEventArgs->unit> "wizard.stepChanging" |> Option.iter (fun v -> Interop.setEventHandler <@ element.StepChanging @> v element)


    override this.initialize parent =
        #if DEBUG
        Diagnostics.Trace.WriteLine $"{this.name} created!"
        #endif
        this.parent <- parent


        let el = new Wizard()
        parent |> Option.iter (fun p -> p.Add el |> ignore)
        this.setProps(el, props)
        props |> Props.tryFind<View->unit> "ref" |> Option.iter (fun v -> v el)
        this.element <- el



    override this.canUpdate prevElement oldProps =
        let changedProps,removedProps = Interop.filterProps oldProps props
        let canUpdateView = ViewElement.canUpdate prevElement changedProps removedProps
        let canUpdateElement =
            true

        canUpdateView && canUpdateElement



    override this.update prevElement oldProps =
        let element = prevElement :?> Wizard
        let changedProps,removedProps = Interop.filterProps oldProps props
        ViewElement.removeProps prevElement removedProps
        removeProps element removedProps
        this.setProps(element, changedProps)
        this.element <- prevElement



// WizardStep
type WizardStepElement(props:Props) =
    inherit TerminalElement(props)

    let removeProps (element: WizardStep) (props: Props) =
        // Properties
        props |> Props.tryFind<string> "wizardStep.backButtonText" |> Option.iter (fun _ -> element.BackButtonText <- Unchecked.defaultof<_>)
        props |> Props.tryFind<string> "wizardStep.helpText" |> Option.iter (fun _ -> element.HelpText <- Unchecked.defaultof<_>)
        props |> Props.tryFind<string> "wizardStep.nextButtonText" |> Option.iter (fun _ -> element.NextButtonText <- Unchecked.defaultof<_>)

    override _.name = $"WizardStep"

    member _.setProps (element: WizardStep, props: Props) =
        base.setProps(element, props)

        // Properties
        props |> Props.tryFind<string> "wizardStep.backButtonText" |> Option.iter (fun v -> element.BackButtonText <- v )
        props |> Props.tryFind<string> "wizardStep.helpText" |> Option.iter (fun v -> element.HelpText <- v )
        props |> Props.tryFind<string> "wizardStep.nextButtonText" |> Option.iter (fun v -> element.NextButtonText <- v )


    override this.initialize parent =
        #if DEBUG
        Diagnostics.Trace.WriteLine $"{this.name} created!"
        #endif
        this.parent <- parent


        let el = new WizardStep()
        parent |> Option.iter (fun p -> p.Add el |> ignore)
        this.setProps(el, props)
        props |> Props.tryFind<View->unit> "ref" |> Option.iter (fun v -> v el)
        this.element <- el



    override this.canUpdate prevElement oldProps =
        let changedProps,removedProps = Interop.filterProps oldProps props
        let canUpdateView = ViewElement.canUpdate prevElement changedProps removedProps
        let canUpdateElement =
            true

        canUpdateView && canUpdateElement



    override this.update prevElement oldProps =
        let element = prevElement :?> WizardStep
        let changedProps,removedProps = Interop.filterProps oldProps props
        ViewElement.removeProps prevElement removedProps
        removeProps element removedProps
        this.setProps(element, changedProps)
        this.element <- prevElement



