
(*
#######################################
#            Props.fs              #
#######################################
*)

namespace Terminal.Gui.Elmish

open System
open System.Text
open System.Drawing
open System.ComponentModel
open System.IO
open System.Collections.Specialized
open System.Globalization
open Terminal.Gui.App
open Terminal.Gui.Drawing
open Terminal.Gui.Drivers
open Terminal.Gui.Elmish
open Terminal.Gui
open Terminal.Gui.Elmish.Elements

open Terminal.Gui.FileServices
open Terminal.Gui.Input

open Terminal.Gui.Text

open Terminal.Gui.ViewBase

open Terminal.Gui.Views
// View

type view() =
    member val props = System.Collections.Generic.List<IProperty>()

    member inline this.children (children:TerminalElement list) = Interop.mkprop "children" children |> this.props.Add
    member inline this.ref (reference:View->unit) = Interop.mkprop "ref" reference |> this.props.Add

    // Properties
    member inline this.arrangement (value:ViewArrangement) = Interop.mkprop "view.arrangement" value |> this.props.Add
    member inline this.borderStyle (value:LineStyle) = Interop.mkprop "view.borderStyle" value |> this.props.Add
    member inline this.canFocus (value:bool) = Interop.mkprop "view.canFocus" value |> this.props.Add
    member inline this.contentSizeTracksViewport (value:bool) = Interop.mkprop "view.contentSizeTracksViewport" value |> this.props.Add
    member inline this.cursorVisibility (value:CursorVisibility) = Interop.mkprop "view.cursorVisibility" value |> this.props.Add
    member inline this.data (value:Object) = Interop.mkprop "view.data" value |> this.props.Add
    member inline this.enabled (value:bool) = Interop.mkprop "view.enabled" value |> this.props.Add
    member inline this.frame (value:Rectangle) = Interop.mkprop "view.frame" value |> this.props.Add
    member inline this.hasFocus (value:bool) = Interop.mkprop "view.hasFocus" value |> this.props.Add
    member inline this.height (value:Dim) = Interop.mkprop "view.height" value |> this.props.Add
    member inline this.highlightStates (value:MouseState) = Interop.mkprop "view.highlightStates" value |> this.props.Add
    member inline this.hotKey (value:Key) = Interop.mkprop "view.hotKey" value |> this.props.Add
    member inline this.hotKeySpecifier (value:Rune) = Interop.mkprop "view.hotKeySpecifier" value |> this.props.Add
    member inline this.id (value:string) = Interop.mkprop "view.id" value |> this.props.Add
    member inline this.isInitialized (value:bool) = Interop.mkprop "view.isInitialized" value |> this.props.Add
    member inline this.mouseHeldDown (value:IMouseHeldDown) = Interop.mkprop "view.mouseHeldDown" value |> this.props.Add
    member inline this.needsDraw (value:bool) = Interop.mkprop "view.needsDraw" value |> this.props.Add
    member inline this.preserveTrailingSpaces (value:bool) = Interop.mkprop "view.preserveTrailingSpaces" value |> this.props.Add
    member inline this.schemeName (value:string) = Interop.mkprop "view.schemeName" value |> this.props.Add
    member inline this.shadowStyle (value:ShadowStyle) = Interop.mkprop "view.shadowStyle" value |> this.props.Add
    member inline this.superViewRendersLineCanvas (value:bool) = Interop.mkprop "view.superViewRendersLineCanvas" value |> this.props.Add
    member inline this.tabStop (value:TabBehavior option) = Interop.mkprop "view.tabStop" value |> this.props.Add
    member inline this.text (value:string) = Interop.mkprop "view.text" value |> this.props.Add
    member inline this.textAlignment (value:Alignment) = Interop.mkprop "view.textAlignment" value |> this.props.Add
    member inline this.textDirection (value:TextDirection) = Interop.mkprop "view.textDirection" value |> this.props.Add
    member inline this.title (value:string) = Interop.mkprop "view.title" value |> this.props.Add
    member inline this.validatePosDim (value:bool) = Interop.mkprop "view.validatePosDim" value |> this.props.Add
    member inline this.verticalTextAlignment (value:Alignment) = Interop.mkprop "view.verticalTextAlignment" value |> this.props.Add
    member inline this.viewport (value:Rectangle) = Interop.mkprop "view.viewport" value |> this.props.Add
    member inline this.viewportSettings (value:ViewportSettingsFlags) = Interop.mkprop "view.viewportSettings" value |> this.props.Add
    member inline this.visible (value:bool) = Interop.mkprop "view.visible" value |> this.props.Add
    member inline this.wantContinuousButtonPressed (value:bool) = Interop.mkprop "view.wantContinuousButtonPressed" value |> this.props.Add
    member inline this.wantMousePositionReports (value:bool) = Interop.mkprop "view.wantMousePositionReports" value |> this.props.Add
    member inline this.width (value:Dim) = Interop.mkprop "view.width" value |> this.props.Add
    member inline this.x (value:Pos) = Interop.mkprop "view.x" value |> this.props.Add
    member inline this.y (value:Pos) = Interop.mkprop "view.y" value |> this.props.Add
    // Events
    member inline this.accepting (handler:HandledEventArgs->unit) = Interop.mkprop "view.accepting" handler |> this.props.Add
    member inline this.advancingFocus (handler:AdvanceFocusEventArgs->unit) = Interop.mkprop "view.advancingFocus" handler |> this.props.Add
    member inline this.borderStyleChanged (handler:EventArgs->unit) = Interop.mkprop "view.borderStyleChanged" handler |> this.props.Add
    member inline this.canFocusChanged (handler:unit->unit) = Interop.mkprop "view.canFocusChanged" handler |> this.props.Add
    member inline this.clearedViewport (handler:DrawEventArgs->unit) = Interop.mkprop "view.clearedViewport" handler |> this.props.Add
    member inline this.clearingViewport (handler:DrawEventArgs->unit) = Interop.mkprop "view.clearingViewport" handler |> this.props.Add
    member inline this.commandNotBound (handler:CommandEventArgs->unit) = Interop.mkprop "view.commandNotBound" handler |> this.props.Add
    member inline this.contentSizeChanged (handler:SizeChangedEventArgs->unit) = Interop.mkprop "view.contentSizeChanged" handler |> this.props.Add
    member inline this.disposing (handler:unit->unit) = Interop.mkprop "view.disposing" handler |> this.props.Add
    member inline this.drawComplete (handler:DrawEventArgs->unit) = Interop.mkprop "view.drawComplete" handler |> this.props.Add
    member inline this.drawingContent (handler:DrawEventArgs->unit) = Interop.mkprop "view.drawingContent" handler |> this.props.Add
    member inline this.drawingSubViews (handler:DrawEventArgs->unit) = Interop.mkprop "view.drawingSubViews" handler |> this.props.Add
    member inline this.drawingText (handler:DrawEventArgs->unit) = Interop.mkprop "view.drawingText" handler |> this.props.Add
    member inline this.enabledChanged (handler:unit->unit) = Interop.mkprop "view.enabledChanged" handler |> this.props.Add
    member inline this.focusedChanged (handler:HasFocusEventArgs->unit) = Interop.mkprop "view.focusedChanged" handler |> this.props.Add
    member inline this.frameChanged (handler:EventArgs<Rectangle>->unit) = Interop.mkprop "view.frameChanged" handler |> this.props.Add
    member inline this.gettingAttributeForRole (handler:VisualRoleEventArgs->unit) = Interop.mkprop "view.gettingAttributeForRole" handler |> this.props.Add
    member inline this.gettingScheme (handler:ResultEventArgs<Scheme>->unit) = Interop.mkprop "view.gettingScheme" handler |> this.props.Add
    member inline this.handlingHotKey (handler:CommandEventArgs->unit) = Interop.mkprop "view.handlingHotKey" handler |> this.props.Add
    member inline this.hasFocusChanged (handler:HasFocusEventArgs->unit) = Interop.mkprop "view.hasFocusChanged" handler |> this.props.Add
    member inline this.hasFocusChanging (handler:HasFocusEventArgs->unit) = Interop.mkprop "view.hasFocusChanging" handler |> this.props.Add
    member inline this.hotKeyChanged (handler:KeyChangedEventArgs->unit) = Interop.mkprop "view.hotKeyChanged" handler |> this.props.Add
    member inline this.initialized (handler:unit->unit) = Interop.mkprop "view.initialized" handler |> this.props.Add
    member inline this.keyDown (handler:Key->unit) = Interop.mkprop "view.keyDown" handler |> this.props.Add
    member inline this.keyDownNotHandled (handler:Key->unit) = Interop.mkprop "view.keyDownNotHandled" handler |> this.props.Add
    member inline this.keyUp (handler:Key->unit) = Interop.mkprop "view.keyUp" handler |> this.props.Add
    member inline this.mouseClick (handler:MouseEventArgs->unit) = Interop.mkprop "view.mouseClick" handler |> this.props.Add
    member inline this.mouseEnter (handler:CancelEventArgs->unit) = Interop.mkprop "view.mouseEnter" handler |> this.props.Add
    member inline this.mouseEvent (handler:MouseEventArgs->unit) = Interop.mkprop "view.mouseEvent" handler |> this.props.Add
    member inline this.mouseLeave (handler:EventArgs->unit) = Interop.mkprop "view.mouseLeave" handler |> this.props.Add
    member inline this.mouseStateChanged (handler:EventArgs->unit) = Interop.mkprop "view.mouseStateChanged" handler |> this.props.Add
    member inline this.mouseWheel (handler:MouseEventArgs->unit) = Interop.mkprop "view.mouseWheel" handler |> this.props.Add
    member inline this.removed (handler:SuperViewChangedEventArgs->unit) = Interop.mkprop "view.removed" handler |> this.props.Add
    member inline this.schemeChanged (handler:ValueChangedEventArgs<Scheme>->unit) = Interop.mkprop "view.schemeChanged" handler |> this.props.Add
    member inline this.schemeChanging (handler:ValueChangingEventArgs<Scheme>->unit) = Interop.mkprop "view.schemeChanging" handler |> this.props.Add
    member inline this.schemeNameChanged (handler:ValueChangedEventArgs<string>->unit) = Interop.mkprop "view.schemeNameChanged" handler |> this.props.Add
    member inline this.schemeNameChanging (handler:ValueChangingEventArgs<string>->unit) = Interop.mkprop "view.schemeNameChanging" handler |> this.props.Add
    member inline this.selecting (handler:CommandEventArgs->unit) = Interop.mkprop "view.selecting" handler |> this.props.Add
    member inline this.subViewAdded (handler:SuperViewChangedEventArgs->unit) = Interop.mkprop "view.subViewAdded" handler |> this.props.Add
    member inline this.subViewLayout (handler:LayoutEventArgs->unit) = Interop.mkprop "view.subViewLayout" handler |> this.props.Add
    member inline this.subViewRemoved (handler:SuperViewChangedEventArgs->unit) = Interop.mkprop "view.subViewRemoved" handler |> this.props.Add
    member inline this.subViewsLaidOut (handler:LayoutEventArgs->unit) = Interop.mkprop "view.subViewsLaidOut" handler |> this.props.Add
    member inline this.superViewChanged (handler:SuperViewChangedEventArgs->unit) = Interop.mkprop "view.superViewChanged" handler |> this.props.Add
    member inline this.textChanged (handler:unit->unit) = Interop.mkprop "view.textChanged" handler |> this.props.Add
    member inline this.titleChanged (handler:string->unit) = Interop.mkprop "view.titleChanged" handler |> this.props.Add
    member inline this.titleChanging (handler:App.CancelEventArgs<string>->unit) = Interop.mkprop "view.titleChanging" handler |> this.props.Add
    member inline this.viewportChanged (handler:DrawEventArgs->unit) = Interop.mkprop "view.viewportChanged" handler |> this.props.Add
    member inline this.visibleChanged (handler:unit->unit) = Interop.mkprop "view.visibleChanged" handler |> this.props.Add
    member inline this.visibleChanging (handler:unit->unit) = Interop.mkprop "view.visibleChanging" handler |> this.props.Add

module prop =
    module position =
        type x =
            static member inline absolute (position:int)                                        = Interop.mkprop "view.x" (Pos.Absolute(position))
            static member inline align (alignment:Alignment, ?modes:AlignmentModes, ?groupId:int)
                =
                    let modes = defaultArg modes AlignmentModes.StartToEnd ||| AlignmentModes.AddSpaceBetweenItems
                    let groupId = defaultArg groupId 0
                    Interop.mkprop "view.x" (Pos.Align(alignment, modes, groupId))
            static member inline anchorEnd                                                      = Interop.mkprop "view.x" (Pos.AnchorEnd())
            static member inline anchorEndWithOffset (offset:int)                               = Interop.mkprop "view.x" (Pos.AnchorEnd(offset))
            static member inline center                                                         = Interop.mkprop "view.x" (Pos.Center())
            static member inline func (f:View -> int)                                           = Interop.mkprop "view.x" (Pos.Func(f))
            static member inline percent (percent:int)                                          = Interop.mkprop "view.x" (Pos.Percent(percent))

        type y =
            static member inline absolute (position:int)                                        = Interop.mkprop "view.y" (Pos.Absolute(position))
            static member inline align (alignment:Alignment, ?modes:AlignmentModes, ?groupId:int)
                =
                    let modes = defaultArg modes AlignmentModes.StartToEnd ||| AlignmentModes.AddSpaceBetweenItems
                    let groupId = defaultArg groupId 0
                    Interop.mkprop "view.y" (Pos.Align(alignment, modes, groupId))
            static member inline anchorEnd                                                      = Interop.mkprop "view.y" (Pos.AnchorEnd())
            static member inline anchorEndWithOffset (offset:int)                               = Interop.mkprop "view.y" (Pos.AnchorEnd(offset))
            static member inline center                                                         = Interop.mkprop "view.y" (Pos.Center())
            static member inline func (f:View -> int)                                           = Interop.mkprop "view.y" (Pos.Func(f))
            static member inline percent (percent:int)                                          = Interop.mkprop "view.y" (Pos.Percent(percent))

    type width =
        static member inline absolute (size:int)                                                                    = Interop.mkprop "view.width" (Dim.Absolute(size))
        static member inline auto (?style:DimAutoStyle, ?minimumContentDim:Dim, ?maximumContentDim:Dim)
            =
                let style = defaultArg style DimAutoStyle.Auto
                let minimumContentDim = defaultArg minimumContentDim null
                let maximumContentDim = defaultArg maximumContentDim null
                Interop.mkprop "view.width" (Dim.Auto(style, minimumContentDim, maximumContentDim))
        static member inline fill (margin:int)                                                                      = Interop.mkprop "view.width" (Dim.Fill(margin))
        static member inline func (f:View->int)                                                                     = Interop.mkprop "view.width" (Dim.Func(f))
        static member inline percent (percent:int, mode:DimPercentMode)                                             = Interop.mkprop "view.width" (Dim.Percent(percent, mode))
        static member inline percent (percent:int)                                                                  = Interop.mkprop "view.width" (Dim.Percent(percent, DimPercentMode.ContentSize))

    type height =
        static member inline absolute (size:int)                                                                    = Interop.mkprop "view.height" (Dim.Absolute(size))
        static member inline auto (?style:DimAutoStyle, ?minimumContentDim:Dim, ?maximumContentDim:Dim)
            =
                let style = defaultArg style DimAutoStyle.Auto
                let minimumContentDim = defaultArg minimumContentDim null
                let maximumContentDim = defaultArg maximumContentDim null
                Interop.mkprop "view.height" (Dim.Auto(style, minimumContentDim, maximumContentDim))
        static member inline fill (margin:int)                                                                      = Interop.mkprop "view.height" (Dim.Fill(margin))
        static member inline func (f:View->int)                                                                     = Interop.mkprop "view.height" (Dim.Func(f))
        static member inline percent (percent:int, mode:DimPercentMode)                                             = Interop.mkprop "view.height" (Dim.Percent(percent, mode))
        static member inline percent (percent:int)                                                                  = Interop.mkprop "view.height" (Dim.Percent(percent, DimPercentMode.ContentSize))


    type alignment =
        static member inline center     =   Interop.mkprop "view.alignment" Alignment.Center
        static member inline ``end``    =   Interop.mkprop "view.alignment" Alignment.End
        static member inline start      =   Interop.mkprop "view.alignment" Alignment.Start
        static member inline fill       =   Interop.mkprop "view.alignment" Alignment.Fill

    type textDirection =
        static member inline bottomTop_leftRight = Interop.mkprop "view.textDirection" TextDirection.BottomTop_LeftRight
        static member inline bottomTop_rightLeft = Interop.mkprop "view.textDirection" TextDirection.BottomTop_RightLeft
        static member inline leftRight_bottomTop = Interop.mkprop "view.textDirection" TextDirection.LeftRight_BottomTop
        static member inline leftRight_topBottom = Interop.mkprop "view.textDirection" TextDirection.LeftRight_TopBottom
        static member inline rightLeft_bottomTop = Interop.mkprop "view.textDirection" TextDirection.RightLeft_BottomTop
        static member inline rightLeft_topBottom = Interop.mkprop "view.textDirection" TextDirection.RightLeft_TopBottom
        static member inline topBottom_leftRight = Interop.mkprop "view.textDirection" TextDirection.TopBottom_LeftRight

    type borderStyle =
        static member inline double = Interop.mkprop    "view.borderStyle" LineStyle.Double
        static member inline none = Interop.mkprop      "view.borderStyle" LineStyle.None
        static member inline rounded = Interop.mkprop   "view.borderStyle" LineStyle.Rounded
        static member inline single = Interop.mkprop    "view.borderStyle" LineStyle.Single

    type shadowStyle =
        static member inline none = Interop.mkprop          "view.shadowStyle" ShadowStyle.None
        static member inline opaque = Interop.mkprop        "view.shadowStyle" ShadowStyle.Opaque
        static member inline transparent = Interop.mkprop   "view.shadowStyle" ShadowStyle.Transparent

// Adornment
type adornment() =
    inherit view()
    // Properties
    member inline this.diagnostics (value:ViewDiagnosticFlags) = Interop.mkprop "adornment.diagnostics" value |> this.props.Add
    member inline this.superViewRendersLineCanvas (value:bool) = Interop.mkprop "adornment.superViewRendersLineCanvas" value |> this.props.Add
    member inline this.thickness (value:Thickness) = Interop.mkprop "adornment.thickness" value |> this.props.Add
    member inline this.viewport (value:Rectangle) = Interop.mkprop "adornment.viewport" value |> this.props.Add
    // Events
    member inline this.thicknessChanged (handler:unit->unit) = Interop.mkprop "adornment.thicknessChanged" handler |> this.props.Add

// Bar
type bar() =
    inherit view()
    // Properties
    member inline this.alignmentModes (value:AlignmentModes) = Interop.mkprop "bar.alignmentModes" value |> this.props.Add
    member inline this.orientation (value:Orientation) = Interop.mkprop "bar.orientation" value |> this.props.Add
    // Events
    member inline this.orientationChanged (handler:Orientation->unit) = Interop.mkprop "bar.orientationChanged" handler |> this.props.Add
    member inline this.orientationChanging (handler:App.CancelEventArgs<Orientation>->unit) = Interop.mkprop "bar.orientationChanging" handler |> this.props.Add

// Border
type border() =
    inherit adornment()
    // Properties
    member inline this.lineStyle (value:LineStyle) = Interop.mkprop "border.lineStyle" value |> this.props.Add
    member inline this.settings (value:BorderSettings) = Interop.mkprop "border.settings" value |> this.props.Add

// Button
type button() =
    inherit view()
    // Properties
    member inline this.hotKeySpecifier (value:Rune) = Interop.mkprop "button.hotKeySpecifier" value |> this.props.Add
    member inline this.isDefault (value:bool) = Interop.mkprop "button.isDefault" value |> this.props.Add
    member inline this.noDecorations (value:bool) = Interop.mkprop "button.noDecorations" value |> this.props.Add
    member inline this.noPadding (value:bool) = Interop.mkprop "button.noPadding" value |> this.props.Add
    member inline this.text (value:string) = Interop.mkprop "button.text" value |> this.props.Add
    member inline this.wantContinuousButtonPressed (value:bool) = Interop.mkprop "button.wantContinuousButtonPressed" value |> this.props.Add

// CheckBox
type checkBox() =
    inherit view()
    // Properties
    member inline this.allowCheckStateNone (value:bool) = Interop.mkprop "checkBox.allowCheckStateNone" value |> this.props.Add
    member inline this.checkedState (value:CheckState) = Interop.mkprop "checkBox.checkedState" value |> this.props.Add
    member inline this.hotKeySpecifier (value:Rune) = Interop.mkprop "checkBox.hotKeySpecifier" value |> this.props.Add
    member inline this.radioStyle (value:bool) = Interop.mkprop "checkBox.radioStyle" value |> this.props.Add
    member inline this.text (value:string) = Interop.mkprop "checkBox.text" value |> this.props.Add
    // Events
    member inline this.checkedStateChanging (handler:ResultEventArgs<CheckState>->unit) = Interop.mkprop "checkBox.checkedStateChanging" handler |> this.props.Add

    member inline this.ischecked (value:bool) = Interop.mkprop "checkBox.checkedState" (if value then CheckState.Checked else CheckState.UnChecked) |> this.props.Add


// ColorPicker
type colorPicker() =
    inherit view()
    // Properties
    member inline this.selectedColor (value:Color) = Interop.mkprop "colorPicker.selectedColor" value |> this.props.Add
    member inline this.style (value:ColorPickerStyle) = Interop.mkprop "colorPicker.style" value |> this.props.Add
    // Events
    member inline this.colorChanged (handler:ResultEventArgs<Color>->unit) = Interop.mkprop "colorPicker.colorChanged" handler |> this.props.Add

// ColorPicker16
type colorPicker16() =
    inherit view()
    // Properties
    member inline this.boxHeight (value:Int32) = Interop.mkprop "colorPicker16.boxHeight" value |> this.props.Add
    member inline this.boxWidth (value:Int32) = Interop.mkprop "colorPicker16.boxWidth" value |> this.props.Add
    member inline this.cursor (value:Point) = Interop.mkprop "colorPicker16.cursor" value |> this.props.Add
    member inline this.selectedColor (value:ColorName16) = Interop.mkprop "colorPicker16.selectedColor" value |> this.props.Add
    // Events
    member inline this.colorChanged (handler:ResultEventArgs<Color>->unit) = Interop.mkprop "colorPicker16.colorChanged" handler |> this.props.Add

// ComboBox
type comboBox() =
    inherit view()
    // Properties
    member inline this.hideDropdownListOnClick (value:bool) = Interop.mkprop "comboBox.hideDropdownListOnClick" value |> this.props.Add
    member inline this.readOnly (value:bool) = Interop.mkprop "comboBox.readOnly" value |> this.props.Add
    member inline this.searchText (value:string) = Interop.mkprop "comboBox.searchText" value |> this.props.Add
    member inline this.selectedItem (value:Int32) = Interop.mkprop "comboBox.selectedItem" value |> this.props.Add
    member inline this.source (value:string list) = Interop.mkprop "comboBox.source" value |> this.props.Add
    member inline this.text (value:string) = Interop.mkprop "comboBox.text" value |> this.props.Add
    // Events
    member inline this.collapsed (handler:unit->unit) = Interop.mkprop "comboBox.collapsed" handler |> this.props.Add
    member inline this.expanded (handler:unit->unit) = Interop.mkprop "comboBox.expanded" handler |> this.props.Add
    member inline this.openSelectedItem (handler:ListViewItemEventArgs->unit) = Interop.mkprop "comboBox.openSelectedItem" handler |> this.props.Add
    member inline this.selectedItemChanged (handler:ListViewItemEventArgs->unit) = Interop.mkprop "comboBox.selectedItemChanged" handler |> this.props.Add

// TextField
type textField() =
    inherit view()
    // Properties
    member inline this.autocomplete (value:IAutocomplete) = Interop.mkprop "textField.autocomplete" value |> this.props.Add
    member inline this.caption (value:string) = Interop.mkprop "textField.caption" value |> this.props.Add
    member inline this.captionColor (value:Terminal.Gui.Drawing.Color) = Interop.mkprop "textField.captionColor" value |> this.props.Add
    member inline this.cursorPosition (value:Int32) = Interop.mkprop "textField.cursorPosition" value |> this.props.Add
    member inline this.readOnly (value:bool) = Interop.mkprop "textField.readOnly" value |> this.props.Add
    member inline this.secret (value:bool) = Interop.mkprop "textField.secret" value |> this.props.Add
    member inline this.selectedStart (value:Int32) = Interop.mkprop "textField.selectedStart" value |> this.props.Add
    member inline this.selectWordOnlyOnDoubleClick (value:bool) = Interop.mkprop "textField.selectWordOnlyOnDoubleClick" value |> this.props.Add
    member inline this.text (value:string) = Interop.mkprop "textField.text" value |> this.props.Add
    member inline this.used (value:bool) = Interop.mkprop "textField.used" value |> this.props.Add
    member inline this.useSameRuneTypeForWords (value:bool) = Interop.mkprop "textField.useSameRuneTypeForWords" value |> this.props.Add
    // Events
    member inline this.textChanging (handler:ResultEventArgs<string>->unit) = Interop.mkprop "textField.textChanging" handler |> this.props.Add

// DateField
type dateField() =
    inherit textField()
    // Properties
    member inline this.culture (value:CultureInfo) = Interop.mkprop "dateField.culture" value |> this.props.Add
    member inline this.cursorPosition (value:Int32) = Interop.mkprop "dateField.cursorPosition" value |> this.props.Add
    member inline this.date (value:DateTime) = Interop.mkprop "dateField.date" value |> this.props.Add
    // Events
    member inline this.dateChanged (handler:DateTimeEventArgs<DateTime>->unit) = Interop.mkprop "dateField.dateChanged" handler |> this.props.Add

// DatePicker
type datePicker() =
    inherit view()
    // Properties
    member inline this.culture (value:CultureInfo) = Interop.mkprop "datePicker.culture" value |> this.props.Add
    member inline this.date (value:DateTime) = Interop.mkprop "datePicker.date" value |> this.props.Add

// Toplevel
type toplevel() =
    inherit view()
    // Properties
    member inline this.modal (value:bool) = Interop.mkprop "toplevel.modal" value |> this.props.Add
    member inline this.running (value:bool) = Interop.mkprop "toplevel.running" value |> this.props.Add
    // Events
    member inline this.activate (handler:ToplevelEventArgs->unit) = Interop.mkprop "toplevel.activate" handler |> this.props.Add
    member inline this.closed (handler:ToplevelEventArgs->unit) = Interop.mkprop "toplevel.closed" handler |> this.props.Add
    member inline this.closing (handler:ToplevelClosingEventArgs->unit) = Interop.mkprop "toplevel.closing" handler |> this.props.Add
    member inline this.deactivate (handler:ToplevelEventArgs->unit) = Interop.mkprop "toplevel.deactivate" handler |> this.props.Add
    member inline this.loaded (handler:unit->unit) = Interop.mkprop "toplevel.loaded" handler |> this.props.Add
    member inline this.ready (handler:unit->unit) = Interop.mkprop "toplevel.ready" handler |> this.props.Add
    member inline this.sizeChanging (handler:SizeChangedEventArgs->unit) = Interop.mkprop "toplevel.sizeChanging" handler |> this.props.Add
    member inline this.unloaded (handler:unit->unit) = Interop.mkprop "toplevel.unloaded" handler |> this.props.Add

// Dialog
type dialog() =
    inherit toplevel()
    // Properties
    member inline this.buttonAlignment (value:Alignment) = Interop.mkprop "dialog.buttonAlignment" value |> this.props.Add
    member inline this.buttonAlignmentModes (value:AlignmentModes) = Interop.mkprop "dialog.buttonAlignmentModes" value |> this.props.Add
    member inline this.canceled (value:bool) = Interop.mkprop "dialog.canceled" value |> this.props.Add

// FileDialog
type fileDialog() =
    inherit dialog()
    // Properties
    member inline this.allowedTypes (value:IAllowedType list) = Interop.mkprop "fileDialog.allowedTypes" value |> this.props.Add
    member inline this.allowsMultipleSelection (value:bool) = Interop.mkprop "fileDialog.allowsMultipleSelection" value |> this.props.Add
    member inline this.fileOperationsHandler (value:IFileOperations) = Interop.mkprop "fileDialog.fileOperationsHandler" value |> this.props.Add
    member inline this.mustExist (value:bool) = Interop.mkprop "fileDialog.mustExist" value |> this.props.Add
    member inline this.openMode (value:OpenMode) = Interop.mkprop "fileDialog.openMode" value |> this.props.Add
    member inline this.path (value:string) = Interop.mkprop "fileDialog.path" value |> this.props.Add
    member inline this.searchMatcher (value:ISearchMatcher) = Interop.mkprop "fileDialog.searchMatcher" value |> this.props.Add
    // Events
    member inline this.filesSelected (handler:FilesSelectedEventArgs->unit) = Interop.mkprop "fileDialog.filesSelected" handler |> this.props.Add

/// SaveDialog
type saveDialog() =
    inherit fileDialog()

// FrameView
type frameView() =
    inherit view()
// No properties or events FrameView

// GraphView
type graphView() =
    inherit view()

    // Properties
    member inline this.axisX (value:HorizontalAxis) = Interop.mkprop "graphView.axisX" value |> this.props.Add
    member inline this.axisY (value:VerticalAxis) = Interop.mkprop "graphView.axisY" value |> this.props.Add
    member inline this.cellSize (value:PointF) = Interop.mkprop "graphView.cellSize" value |> this.props.Add
    member inline this.graphColor (value:Attribute option) = Interop.mkprop "graphView.graphColor" value |> this.props.Add
    member inline this.marginBottom (value:int) = Interop.mkprop "graphView.marginBottom" value |> this.props.Add
    member inline this.marginLeft (value:int) = Interop.mkprop "graphView.marginLeft" value |> this.props.Add
    member inline this.scrollOffset (value:PointF) = Interop.mkprop "graphView.scrollOffset" value |> this.props.Add

// HexView
type hexView() =
    inherit view()

    // Properties
    member inline this.address (value:Int64) = Interop.mkprop "hexView.address" value |> this.props.Add
    member inline this.addressWidth (value:int) = Interop.mkprop "hexView.addressWidth" value |> this.props.Add
    member inline this.allowEdits (value:int) = Interop.mkprop "hexView.allowEdits" value |> this.props.Add
    member inline this.readOnly (value:bool) = Interop.mkprop "hexView.readOnly" value |> this.props.Add
    member inline this.source (value:Stream) = Interop.mkprop "hexView.source" value |> this.props.Add
    // Events
    member inline this.edited (handler:HexViewEditEventArgs->unit) = Interop.mkprop "hexView.edited" handler |> this.props.Add
    member inline this.positionChanged (handler:HexViewEventArgs->unit) = Interop.mkprop "hexView.positionChanged" handler |> this.props.Add

// Label
type label() =
    inherit view()

    // Properties
    member inline this.hotKeySpecifier (value:Rune) = Interop.mkprop "label.hotKeySpecifier" value |> this.props.Add
    member inline this.text (value:string) = Interop.mkprop "label.text" value |> this.props.Add

// LegendAnnotation
type legendAnnotation() =
    inherit view()
// No properties or events LegendAnnotation

// Line
type line() =
    inherit view()

    // Properties
    member inline this.orientation (value:Orientation) = Interop.mkprop "line.orientation" value |> this.props.Add
    // Events
    member inline this.orientationChanged (handler:Orientation->unit) = Interop.mkprop "line.orientationChanged" handler |> this.props.Add
    member inline this.orientationChanging (handler:App.CancelEventArgs<Orientation>->unit) = Interop.mkprop "line.orientationChanging" handler |> this.props.Add

// LineView
type lineView() =
    inherit view()

    // Properties
    member inline this.endingAnchor (value:Rune option) = Interop.mkprop "lineView.endingAnchor" value |> this.props.Add
    member inline this.lineRune (value:Rune) = Interop.mkprop "lineView.lineRune" value |> this.props.Add
    member inline this.orientation (value:Orientation) = Interop.mkprop "lineView.orientation" value |> this.props.Add
    member inline this.startingAnchor (value:Rune option) = Interop.mkprop "lineView.startingAnchor" value |> this.props.Add

// ListView
type listView() =
    inherit view()

    // Properties
    member inline this.allowsMarking (value:bool) = Interop.mkprop "listView.allowsMarking" value |> this.props.Add
    member inline this.allowsMultipleSelection (value:bool) = Interop.mkprop "listView.allowsMultipleSelection" value |> this.props.Add
    member inline this.leftItem (value:Int32) = Interop.mkprop "listView.leftItem" value |> this.props.Add
    member inline this.selectedItem (value:Int32) = Interop.mkprop "listView.selectedItem" value |> this.props.Add
    member inline this.source (value:string list) = Interop.mkprop "listView.source" value |> this.props.Add
    member inline this.topItem (value:Int32) = Interop.mkprop "listView.topItem" value |> this.props.Add
    // Events
    member inline this.collectionChanged (handler:NotifyCollectionChangedEventArgs->unit) = Interop.mkprop "listView.collectionChanged" handler |> this.props.Add
    member inline this.openSelectedItem (handler:ListViewItemEventArgs->unit) = Interop.mkprop "listView.openSelectedItem" handler |> this.props.Add
    member inline this.rowRender (handler:ListViewRowEventArgs->unit) = Interop.mkprop "listView.rowRender" handler |> this.props.Add
    member inline this.selectedItemChanged (handler:ListViewItemEventArgs->unit) = Interop.mkprop "listView.selectedItemChanged" handler |> this.props.Add

// Margin
type margin() =
    inherit adornment()

    // Properties
    member inline this.shadowStyle (value:ShadowStyle) = Interop.mkprop "margin.shadowStyle" value |> this.props.Add

type menuv2() =
    inherit bar()

    // Properties
    member inline this.selectedMenuItem (value: MenuItemv2 list) = Interop.mkprop "menuv2.selectedMenuItem" value |> this.props.Add
    member inline this.superMenuItem (value: MenuItemv2 list) = Interop.mkprop "menuv2.superMenuItem" value |> this.props.Add
    // Events
    member inline this.accepted (value: CommandEventArgs->unit) = Interop.mkprop "menuv2.accepted" value |> this.props.Add
    member inline this.selectedMenuItemChanged (value: MenuItemv2->unit) = Interop.mkprop "menuv2.selectedMenuItemChanged" value |> this.props.Add

// MenuBarV2
type menuBarv2() =
    inherit menuv2()

    // Properties
    member inline this.key (value:Key) = Interop.mkprop "menuBarv2.key" value |> this.props.Add
    member inline this.menus (value:MenuBarItemv2Element list) = Interop.mkprop "children" (value |> List.map (fun v -> v :> TerminalElement)) |> this.props.Add
    // Events
    member inline this.keyChanged (handler:KeyChangedEventArgs->unit) = Interop.mkprop "menuBarv2.keyChanged" handler |> this.props.Add

type shortcut() =
     inherit view()

     // Properties
     member inline this.action (value:Action) = Interop.mkprop "shortcut.action" value |> this.props.Add
     member inline this.alignmentModes (value:AlignmentModes) = Interop.mkprop "shortcut.alignmentModes" value |> this.props.Add
     member inline this.forceFocusColors (value:bool) = Interop.mkprop "shortcut.forceFocusColors" value |> this.props.Add
     member inline this.helpText (value:string) = Interop.mkprop "shortcut.helpText" value |> this.props.Add
     member inline this.text (value:string) = Interop.mkprop "shortcut.text" value |> this.props.Add
     member inline this.bindKeyToApplication (value:bool) = Interop.mkprop "shortcut.bindKeyToApplication" value |> this.props.Add
     member inline this.key (value:Key) = Interop.mkprop "shortcut.key" value |> this.props.Add
     member inline this.minimumKeyTextSize (value:Int32) = Interop.mkprop "shortcut.minimumKeyTextSize" value |> this.props.Add
     // Events
     member inline this.orientationChanged (handler:Orientation->unit) = Interop.mkprop "shortcut.orientationChanged" handler |> this.props.Add
     member inline this.orientationChanging (handler:CancelEventArgs<Orientation>->unit) = Interop.mkprop "shortcut.orientationChanging" handler |> this.props.Add

type menuItemv2() =
    inherit shortcut()
    member inline this.command (value: Command) = Interop.mkprop "menuItemv2.command" value |> this.props.Add
    member inline this.submenu(value: Menuv2Element) = Interop.mkprop "menuItemv2.subMenu.element" value |> this.props.Add
    member inline this.accepted(value: CommandEventArgs -> unit) = Interop.mkprop "menuItemv2.accepted" value |> this.props.Add

type menuBarItemv2() =
    inherit menuItemv2()
    member inline this.popoverMenu (value:PopoverMenuElement) = Interop.mkprop "menuBarItemv2.popoverMenu.element" value |> this.props.Add
    member inline this.popoverMenuOpen (value:bool) = Interop.mkprop "menuBarItemv2.popoverMenuOpen" value |> this.props.Add

type popoverMenu() =
    inherit view()

    member inline this.key (value:Key) = Interop.mkprop "popoverMenu.key" value |> this.props.Add
    member inline this.root (value: Menuv2Element) = Interop.mkprop "popoverMenu.root.element" value |> this.props.Add


// NumericUpDown`1
type numericUpDown() =
    inherit view()

    // Properties
    member inline this.format (value:string) = Interop.mkprop "numericUpDown`1.format" value |> this.props.Add
    member inline this.increment (value:'a) = Interop.mkprop "numericUpDown`1.increment" value |> this.props.Add
    member inline this.value (value:'a) = Interop.mkprop "numericUpDown`1.value" value |> this.props.Add
    // Events
    member inline this.formatChanged (handler:string->unit) = Interop.mkprop "numericUpDown`1.formatChanged" handler |> this.props.Add
    member inline this.incrementChanged (handler:'a->unit) = Interop.mkprop "numericUpDown`1.incrementChanged" handler |> this.props.Add
    member inline this.valueChanged (handler:'a->unit) = Interop.mkprop "numericUpDown`1.value |> this.props.AddChanged" handler
    member inline this.valueChanging (handler:App.CancelEventArgs<'a>->unit) = Interop.mkprop "numericUpDown`1.value |> this.props.AddChanging" handler

// NumericUpDown
// No properties or events NumericUpDown

// OpenDialog
type openDialog() =
    inherit fileDialog()
    // Properties
    member inline this.openMode (value:OpenMode) = Interop.mkprop "openDialog.openMode" value |> this.props.Add

// Padding
type padding() =
    inherit adornment()

// ProgressBar
type progressBar() =
    inherit view()

    // Properties
    member inline this.bidirectionalMarquee (value:bool) = Interop.mkprop "progressBar.bidirectionalMarquee" value |> this.props.Add
    member inline this.fraction (value:Single) = Interop.mkprop "progressBar.fraction" value |> this.props.Add
    member inline this.progressBarFormat (value:ProgressBarFormat) = Interop.mkprop "progressBar.progressBarFormat" value |> this.props.Add
    member inline this.progressBarStyle (value:ProgressBarStyle) = Interop.mkprop "progressBar.progressBarStyle" value |> this.props.Add
    member inline this.segmentCharacter (value:Rune) = Interop.mkprop "progressBar.segmentCharacter" value |> this.props.Add
    member inline this.text (value:string) = Interop.mkprop "progressBar.text" value |> this.props.Add

// RadioGroup
type radioGroup() =
    inherit view()

    // Properties
    member inline this.assignHotKeysToRadioLabels (value:bool) = Interop.mkprop "radioGroup.assignHotKeysToRadioLabels" value |> this.props.Add
    member inline this.cursor (value:Int32) = Interop.mkprop "radioGroup.cursor" value |> this.props.Add
    member inline this.doubleClickAccepts (value:bool) = Interop.mkprop "radioGroup.doubleClickAccepts" value |> this.props.Add
    member inline this.horizontalSpace (value:Int32) = Interop.mkprop "radioGroup.horizontalSpace" value |> this.props.Add
    member inline this.orientation (value:Orientation) = Interop.mkprop "radioGroup.orientation" value |> this.props.Add
    member inline this.radioLabels (value:string list) = Interop.mkprop "radioGroup.radioLabels" value |> this.props.Add
    member inline this.selectedItem (value:Int32) = Interop.mkprop "radioGroup.selectedItem" value |> this.props.Add
    // Events
    member inline this.orientationChanged (handler:Orientation->unit) = Interop.mkprop "radioGroup.orientationChanged" handler |> this.props.Add
    member inline this.orientationChanging (handler:App.CancelEventArgs<Orientation>->unit) = Interop.mkprop "radioGroup.orientationChanging" handler |> this.props.Add
    member inline this.selectedItemChanged (handler:SelectedItemChangedArgs->unit) = Interop.mkprop "radioGroup.selectedItemChanged" handler |> this.props.Add

// SaveDialog
// No properties or events SaveDialog

// ScrollBar
type scrollBar() =
    inherit view()

    // Properties
    member inline this.autoShow (value:bool) = Interop.mkprop "scrollBar.autoShow" value |> this.props.Add
    member inline this.increment (value:Int32) = Interop.mkprop "scrollBar.increment" value |> this.props.Add
    member inline this.orientation (value:Orientation) = Interop.mkprop "scrollBar.orientation" value |> this.props.Add
    member inline this.position (value:Int32) = Interop.mkprop "scrollBar.position" value |> this.props.Add
    member inline this.scrollableContentSize (value:Int32) = Interop.mkprop "scrollBar.scrollableContentSize" value |> this.props.Add
    member inline this.visibleContentSize (value:Int32) = Interop.mkprop "scrollBar.visibleContentSize" value |> this.props.Add
    // Events
    member inline this.orientationChanged (handler:Orientation->unit) = Interop.mkprop "scrollBar.orientationChanged" handler |> this.props.Add
    member inline this.orientationChanging (handler:CancelEventArgs<Orientation>->unit) = Interop.mkprop "scrollBar.orientationChanging" handler |> this.props.Add
    member inline this.scrollableContentSizeChanged (handler:EventArgs<Int32>->unit) = Interop.mkprop "scrollBar.scrollableContentSizeChanged" handler |> this.props.Add
    member inline this.sliderPositionChanged (handler:EventArgs<Int32>->unit) = Interop.mkprop "scrollBar.sliderPositionChanged" handler |> this.props.Add

// ScrollSlider
type scrollSlider() =
    inherit view()

    // Properties
    member inline this.orientation (value:Orientation) = Interop.mkprop "scrollSlider.orientation" value |> this.props.Add
    member inline this.position (value:Int32) = Interop.mkprop "scrollSlider.position" value |> this.props.Add
    member inline this.size (value:Int32) = Interop.mkprop "scrollSlider.size" value |> this.props.Add
    member inline this.sliderPadding (value:Int32) = Interop.mkprop "scrollSlider.sliderPadding" value |> this.props.Add
    member inline this.visibleContentSize (value:Int32) = Interop.mkprop "scrollSlider.visibleContentSize" value |> this.props.Add
    // Events
    member inline this.orientationChanged (handler:Orientation->unit) = Interop.mkprop "scrollSlider.orientationChanged" handler |> this.props.Add
    member inline this.orientationChanging (handler:CancelEventArgs<Orientation>->unit) = Interop.mkprop "scrollSlider.orientationChanging" handler |> this.props.Add
    member inline this.positionChanged (handler:EventArgs<Int32>->unit) = Interop.mkprop "scrollSlider.positionChanged" handler |> this.props.Add
    member inline this.positionChanging (handler:CancelEventArgs<Int32>->unit) = Interop.mkprop "scrollSlider.positionChanging" handler |> this.props.Add
    member inline this.scrolled (handler:EventArgs<Int32>->unit) = Interop.mkprop "scrollSlider.scrolled" handler |> this.props.Add

// Slider`1
type slider() =
    inherit view()

    // Properties
    member inline this.allowEmpty (value:bool) = Interop.mkprop "slider`1.allowEmpty" value |> this.props.Add
    member inline this.focusedOption (value:Int32) = Interop.mkprop "slider`1.focusedOption" value |> this.props.Add
    member inline this.legendsOrientation (value:Orientation) = Interop.mkprop "slider`1.legendsOrientation" value |> this.props.Add
    member inline this.minimumInnerSpacing (value:Int32) = Interop.mkprop "slider`1.minimumInnerSpacing" value |> this.props.Add
    member inline this.options (value:SliderOption<'a> list) = Interop.mkprop "slider`1.options" value |> this.props.Add
    member inline this.orientation (value:Orientation) = Interop.mkprop "slider`1.orientation" value |> this.props.Add
    member inline this.rangeAllowSingle (value:bool) = Interop.mkprop "slider`1.rangeAllowSingle" value |> this.props.Add
    member inline this.showEndSpacing (value:bool) = Interop.mkprop "slider`1.showEndSpacing" value |> this.props.Add
    member inline this.showLegends (value:bool) = Interop.mkprop "slider`1.showLegends" value |> this.props.Add
    member inline this.style (value:SliderStyle) = Interop.mkprop "slider`1.style" value |> this.props.Add
    member inline this.text (value:string) = Interop.mkprop "slider`1.text" value |> this.props.Add
    member inline this.``type`` (value:SliderType) = Interop.mkprop "slider`1.``type``" value |> this.props.Add
    member inline this.useMinimumSize (value:bool) = Interop.mkprop "slider`1.useMinimumSize" value |> this.props.Add
    // Events
    member inline this.optionFocused (handler:SliderEventArgs<'a>->unit) = Interop.mkprop "slider`1.optionFocused" handler |> this.props.Add
    member inline this.optionsChanged (handler:SliderEventArgs<'a>->unit) = Interop.mkprop "slider`1.optionsChanged" handler |> this.props.Add
    member inline this.orientationChanged (handler:Orientation->unit) = Interop.mkprop "slider`1.orientationChanged" handler |> this.props.Add
    member inline this.orientationChanging (handler:App.CancelEventArgs<Orientation>->unit) = Interop.mkprop "slider`1.orientationChanging" handler |> this.props.Add

// Slider
// No properties or events Slider

// SpinnerView
type spinnerView() =
    inherit view()

    // Properties
    member inline this.autoSpin (value:bool) = Interop.mkprop "spinnerView.autoSpin" value |> this.props.Add
    member inline this.sequence (value:string list) = Interop.mkprop "spinnerView.sequence" value |> this.props.Add
    member inline this.spinBounce (value:bool) = Interop.mkprop "spinnerView.spinBounce" value |> this.props.Add
    member inline this.spinDelay (value:Int32) = Interop.mkprop "spinnerView.spinDelay" value |> this.props.Add
    member inline this.spinReverse (value:bool) = Interop.mkprop "spinnerView.spinReverse" value |> this.props.Add
    member inline this.style (value:SpinnerStyle) = Interop.mkprop "spinnerView.style" value |> this.props.Add

// StatusBar
type statusBar() =
    inherit bar()
// No properties or events StatusBar

// Tab
type tab() =
    inherit view()

    // Properties
    member inline this.displayText (value:string) = Interop.mkprop "tab.displayText" value |> this.props.Add
    member inline this.view (value:TerminalElement) = Interop.mkprop "tab.view" value |> this.props.Add

// TabView
type tabView() =
    inherit view()

    // Properties
    member inline this.maxTabTextWidth (value:int) = Interop.mkprop "tabView.maxTabTextWidth" value |> this.props.Add
    member inline this.selectedTab (value:Tab) = Interop.mkprop "tabView.selectedTab" value |> this.props.Add
    member inline this.style (value:TabStyle) = Interop.mkprop "tabView.style" value |> this.props.Add
    member inline this.tabScrollOffset (value:Int32) = Interop.mkprop "tabView.tabScrollOffset" value |> this.props.Add
    // Events
    member inline this.selectedTabChanged (handler:TabChangedEventArgs->unit) = Interop.mkprop "tabView.selectedTabChanged" handler |> this.props.Add
    member inline this.tabClicked (handler:TabMouseEventArgs->unit) = Interop.mkprop "tabView.tabClicked" handler |> this.props.Add

    member inline this.tabs (value:TerminalElement list) = Interop.mkprop "tabView.tabs" value |> this.props.Add


// TableView
type tableView() =
    inherit view()

    // Properties
    member inline this.cellActivationKey (value:KeyCode) = Interop.mkprop "tableView.cellActivationKey" value |> this.props.Add
    member inline this.collectionNavigator (value:ICollectionNavigator) = Interop.mkprop "tableView.collectionNavigator" value |> this.props.Add
    member inline this.columnOffset (value:Int32) = Interop.mkprop "tableView.columnOffset" value |> this.props.Add
    member inline this.fullRowSelect (value:bool) = Interop.mkprop "tableView.fullRowSelect" value |> this.props.Add
    member inline this.maxCellWidth (value:Int32) = Interop.mkprop "tableView.maxCellWidth" value |> this.props.Add
    member inline this.minCellWidth (value:Int32) = Interop.mkprop "tableView.minCellWidth" value |> this.props.Add
    member inline this.multiSelect (value:bool) = Interop.mkprop "tableView.multiSelect" value |> this.props.Add
    member inline this.nullSymbol (value:string) = Interop.mkprop "tableView.nullSymbol" value |> this.props.Add
    member inline this.rowOffset (value:Int32) = Interop.mkprop "tableView.rowOffset" value |> this.props.Add
    member inline this.selectedColumn (value:Int32) = Interop.mkprop "tableView.selectedColumn" value |> this.props.Add
    member inline this.selectedRow (value:Int32) = Interop.mkprop "tableView.selectedRow" value |> this.props.Add
    member inline this.separatorSymbol (value:Char) = Interop.mkprop "tableView.separatorSymbol" value |> this.props.Add
    member inline this.style (value:TableStyle) = Interop.mkprop "tableView.style" value |> this.props.Add
    member inline this.table (value:ITableSource) = Interop.mkprop "tableView.table" value |> this.props.Add
    // Events
    member inline this.cellActivated (handler:CellActivatedEventArgs->unit) = Interop.mkprop "tableView.cellActivated" handler |> this.props.Add
    member inline this.cellToggled (handler:CellToggledEventArgs->unit) = Interop.mkprop "tableView.cellToggled" handler |> this.props.Add
    member inline this.selectedCellChanged (handler:SelectedCellChangedEventArgs->unit) = Interop.mkprop "tableView.selectedCellChanged" handler |> this.props.Add


// TextValidateField
type textValidateField() =
    inherit view()

    // Properties
    member inline this.provider (value:ITextValidateProvider) = Interop.mkprop "textValidateField.provider" value |> this.props.Add
    member inline this.text (value:string) = Interop.mkprop "textValidateField.text" value |> this.props.Add

// TextView
type textView() =
    inherit view()

    // Properties
    member inline this.allowsReturn (value:bool) = Interop.mkprop "textView.allowsReturn" value |> this.props.Add
    member inline this.allowsTab (value:bool) = Interop.mkprop "textView.allowsTab" value |> this.props.Add
    member inline this.cursorPosition (value:Point) = Interop.mkprop "textView.cursorPosition" value |> this.props.Add
    member inline this.inheritsPreviousAttribute (value:bool) = Interop.mkprop "textView.inheritsPreviousAttribute" value |> this.props.Add
    member inline this.isDirty (value:bool) = Interop.mkprop "textView.isDirty" value |> this.props.Add
    member inline this.isSelecting (value:bool) = Interop.mkprop "textView.isSelecting" value |> this.props.Add
    member inline this.leftColumn (value:Int32) = Interop.mkprop "textView.leftColumn" value |> this.props.Add
    member inline this.multiline (value:bool) = Interop.mkprop "textView.multiline" value |> this.props.Add
    member inline this.readOnly (value:bool) = Interop.mkprop "textView.readOnly" value |> this.props.Add
    member inline this.selectionStartColumn (value:Int32) = Interop.mkprop "textView.selectionStartColumn" value |> this.props.Add
    member inline this.selectionStartRow (value:Int32) = Interop.mkprop "textView.selectionStartRow" value |> this.props.Add
    member inline this.selectWordOnlyOnDoubleClick (value:bool) = Interop.mkprop "textView.selectWordOnlyOnDoubleClick" value |> this.props.Add
    member inline this.tabWidth (value:Int32) = Interop.mkprop "textView.tabWidth" value |> this.props.Add
    member inline this.text (value:string) = Interop.mkprop "textView.text" value |> this.props.Add
    member inline this.topRow (value:Int32) = Interop.mkprop "textView.topRow" value |> this.props.Add
    member inline this.used (value:bool) = Interop.mkprop "textView.used" value |> this.props.Add
    member inline this.useSameRuneTypeForWords (value:bool) = Interop.mkprop "textView.useSameRuneTypeForWords" value |> this.props.Add
    member inline this.wordWrap (value:bool) = Interop.mkprop "textView.wordWrap" value |> this.props.Add
    // Events
    member inline this.contentsChanged (handler:ContentsChangedEventArgs->unit) = Interop.mkprop "textView.contentsChanged" handler |> this.props.Add
    member inline this.drawNormalColor (handler:CellEventArgs->unit) = Interop.mkprop "textView.drawNormalColor" handler |> this.props.Add
    member inline this.drawReadOnlyColor (handler:CellEventArgs->unit) = Interop.mkprop "textView.drawReadOnlyColor" handler |> this.props.Add
    member inline this.drawSelectionColor (handler:CellEventArgs->unit) = Interop.mkprop "textView.drawSelectionColor" handler |> this.props.Add
    member inline this.drawUsedColor (handler:CellEventArgs->unit) = Interop.mkprop "textView.drawUsedColor" handler |> this.props.Add
    member inline this.unwrappedCursorPosition (handler:Point->unit) = Interop.mkprop "textView.unwrappedCursorPosition" handler |> this.props.Add
    // Additional properties
    member inline this.textChanged (value:string->unit) = Interop.mkprop "textView.textChanged" value |> this.props.Add

// TileView
type tileView() =
    inherit view()

    // Properties
    member inline this.lineStyle (value:LineStyle) = Interop.mkprop "tileView.lineStyle" value |> this.props.Add
    member inline this.orientation (value:Orientation) = Interop.mkprop "tileView.orientation" value |> this.props.Add
    member inline this.toggleResizable (value:KeyCode) = Interop.mkprop "tileView.toggleResizable" value |> this.props.Add
    // Events
    member inline this.splitterMoved (handler:SplitterEventArgs->unit) = Interop.mkprop "tileView.splitterMoved" handler |> this.props.Add

// TimeField
type timeField() =
    inherit textField()

    // Properties
    member inline this.cursorPosition (value:Int32) = Interop.mkprop "timeField.cursorPosition" value |> this.props.Add
    member inline this.isShortFormat (value:bool) = Interop.mkprop "timeField.isShortFormat" value |> this.props.Add
    member inline this.time (value:TimeSpan) = Interop.mkprop "timeField.time" value |> this.props.Add
    // Events
    member inline this.timeChanged (handler:DateTimeEventArgs<TimeSpan>->unit) = Interop.mkprop "timeField.timeChanged" handler |> this.props.Add

// TreeView`1
type treeView<'a when 'a : not struct>() =
    inherit view()

    // Properties
    member inline this.allowLetterBasedNavigation (value:bool) = Interop.mkprop "treeView`1.allowLetterBasedNavigation" value |> this.props.Add
    member inline this.aspectGetter<'a when 'a : not struct> (value:AspectGetterDelegate<'a>) = Interop.mkprop "treeView`1.aspectGetter" value |> this.props.Add
    member inline this.colorGetter<'a when 'a : not struct> (value:Func<'a,Scheme>) = Interop.mkprop "treeView`1.colorGetter" value |> this.props.Add
    member inline this.maxDepth (value:Int32) = Interop.mkprop "treeView`1.maxDepth" value |> this.props.Add
    member inline this.multiSelect (value:bool) = Interop.mkprop "treeView`1.multiSelect" value |> this.props.Add
    member inline this.objectActivationButton (value:MouseFlags option) = Interop.mkprop "treeView`1.objectActivationButton" value |> this.props.Add
    member inline this.objectActivationKey (value:KeyCode) = Interop.mkprop "treeView`1.objectActivationKey" value |> this.props.Add
    member inline this.scrollOffsetHorizontal (value:Int32) = Interop.mkprop "treeView`1.scrollOffsetHorizontal" value |> this.props.Add
    member inline this.scrollOffsetVertical (value:Int32) = Interop.mkprop "treeView`1.scrollOffsetVertical" value |> this.props.Add
    member inline this.selectedObject<'a when 'a : not struct> (value:'a) = Interop.mkprop "treeView`1.selectedObject" value |> this.props.Add
    member inline this.style (value:TreeStyle) = Interop.mkprop "treeView`1.style" value |> this.props.Add
    member inline this.treeBuilder<'a when 'a : not struct> (value:ITreeBuilder<'a>) = Interop.mkprop "treeView`1.treeBuilder" value |> this.props.Add
    // Events
    member inline this.drawLine<'a when 'a : not struct> (handler:DrawTreeViewLineEventArgs<'a>->unit) = Interop.mkprop "treeView`1.drawLine" handler |> this.props.Add
    member inline this.objectActivated<'a when 'a : not struct> (handler:ObjectActivatedEventArgs<'a>->unit) = Interop.mkprop "treeView`1.objectActivated" handler |> this.props.Add
    member inline this.selectionChanged<'a when 'a : not struct> (handler:SelectionChangedEventArgs<'a>->unit) = Interop.mkprop "treeView`1.selectionChanged" handler |> this.props.Add

// TreeView
// No properties or events TreeView

// Window
type window() =
    inherit toplevel()
// No properties or events Window

// Wizard
type wizard() =
    inherit dialog()

    // Properties
    member inline this.currentStep (value:WizardStep) = Interop.mkprop "wizard.currentStep" value |> this.props.Add
    member inline this.modal (value:bool) = Interop.mkprop "wizard.modal" value |> this.props.Add
    // Events
    member inline this.cancelled (handler:WizardButtonEventArgs->unit) = Interop.mkprop "wizard.cancelled" handler |> this.props.Add
    member inline this.finished (handler:WizardButtonEventArgs->unit) = Interop.mkprop "wizard.finished" handler |> this.props.Add
    member inline this.movingBack (handler:WizardButtonEventArgs->unit) = Interop.mkprop "wizard.movingBack" handler |> this.props.Add
    member inline this.movingNext (handler:WizardButtonEventArgs->unit) = Interop.mkprop "wizard.movingNext" handler |> this.props.Add
    member inline this.stepChanged (handler:StepChangeEventArgs->unit) = Interop.mkprop "wizard.stepChanged" handler |> this.props.Add
    member inline this.stepChanging (handler:StepChangeEventArgs->unit) = Interop.mkprop "wizard.stepChanging" handler |> this.props.Add

// WizardStep
type wizardStep() =
    inherit view()

    // Properties
    member inline this.backButtonText (value:string) = Interop.mkprop "wizardStep.backButtonText" value |> this.props.Add
    member inline this.helpText (value:string) = Interop.mkprop "wizardStep.helpText" value |> this.props.Add
    member inline this.nextButtonText (value:string) = Interop.mkprop "wizardStep.nextButtonText" value |> this.props.Add
