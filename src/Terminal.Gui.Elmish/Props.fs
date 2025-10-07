
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
    // Properties
    member inline _.arrangement (value:ViewArrangement) = Interop.mkprop "view.arrangement" value
    member inline _.borderStyle (value:LineStyle) = Interop.mkprop "view.borderStyle" value
    member inline _.canFocus (value:bool) = Interop.mkprop "view.canFocus" value
    member inline _.contentSizeTracksViewport (value:bool) = Interop.mkprop "view.contentSizeTracksViewport" value
    member inline _.cursorVisibility (value:CursorVisibility) = Interop.mkprop "view.cursorVisibility" value
    member inline _.data (value:Object) = Interop.mkprop "view.data" value
    member inline _.enabled (value:bool) = Interop.mkprop "view.enabled" value
    member inline _.frame (value:Rectangle) = Interop.mkprop "view.frame" value
    member inline _.hasFocus (value:bool) = Interop.mkprop "view.hasFocus" value
    member inline _.height (value:Dim) = Interop.mkprop "view.height" value
    member inline _.highlightStates (value:MouseState) = Interop.mkprop "view.highlightStates" value
    member inline _.hotKey (value:Key) = Interop.mkprop "view.hotKey" value
    member inline _.hotKeySpecifier (value:Rune) = Interop.mkprop "view.hotKeySpecifier" value
    member inline _.id (value:string) = Interop.mkprop "view.id" value
    member inline _.isInitialized (value:bool) = Interop.mkprop "view.isInitialized" value
    member inline _.mouseHeldDown (value:IMouseHeldDown) = Interop.mkprop "view.mouseHeldDown" value
    member inline _.needsDraw (value:bool) = Interop.mkprop "view.needsDraw" value
    member inline _.preserveTrailingSpaces (value:bool) = Interop.mkprop "view.preserveTrailingSpaces" value
    member inline _.schemeName (value:string) = Interop.mkprop "view.schemeName" value
    member inline _.shadowStyle (value:ShadowStyle) = Interop.mkprop "view.shadowStyle" value
    member inline _.superViewRendersLineCanvas (value:bool) = Interop.mkprop "view.superViewRendersLineCanvas" value
    member inline _.tabStop (value:TabBehavior option) = Interop.mkprop "view.tabStop" value
    member inline _.text (value:string) = Interop.mkprop "view.text" value
    member inline _.textAlignment (value:Alignment) = Interop.mkprop "view.textAlignment" value
    member inline _.textDirection (value:TextDirection) = Interop.mkprop "view.textDirection" value
    member inline _.title (value:string) = Interop.mkprop "view.title" value
    member inline _.validatePosDim (value:bool) = Interop.mkprop "view.validatePosDim" value
    member inline _.verticalTextAlignment (value:Alignment) = Interop.mkprop "view.verticalTextAlignment" value
    member inline _.viewport (value:Rectangle) = Interop.mkprop "view.viewport" value
    member inline _.viewportSettings (value:ViewportSettingsFlags) = Interop.mkprop "view.viewportSettings" value
    member inline _.visible (value:bool) = Interop.mkprop "view.visible" value
    member inline _.wantContinuousButtonPressed (value:bool) = Interop.mkprop "view.wantContinuousButtonPressed" value
    member inline _.wantMousePositionReports (value:bool) = Interop.mkprop "view.wantMousePositionReports" value
    member inline _.width (value:Dim) = Interop.mkprop "view.width" value
    member inline _.x (value:Pos) = Interop.mkprop "view.x" value
    member inline _.y (value:Pos) = Interop.mkprop "view.y" value
    // Events
    member inline _.accepting (handler:HandledEventArgs->unit) = Interop.mkprop "view.accepting" handler
    member inline _.advancingFocus (handler:AdvanceFocusEventArgs->unit) = Interop.mkprop "view.advancingFocus" handler
    member inline _.borderStyleChanged (handler:EventArgs->unit) = Interop.mkprop "view.borderStyleChanged" handler
    member inline _.canFocusChanged (handler:unit->unit) = Interop.mkprop "view.canFocusChanged" handler
    member inline _.clearedViewport (handler:DrawEventArgs->unit) = Interop.mkprop "view.clearedViewport" handler
    member inline _.clearingViewport (handler:DrawEventArgs->unit) = Interop.mkprop "view.clearingViewport" handler
    member inline _.commandNotBound (handler:CommandEventArgs->unit) = Interop.mkprop "view.commandNotBound" handler
    member inline _.contentSizeChanged (handler:SizeChangedEventArgs->unit) = Interop.mkprop "view.contentSizeChanged" handler
    member inline _.disposing (handler:unit->unit) = Interop.mkprop "view.disposing" handler
    member inline _.drawComplete (handler:DrawEventArgs->unit) = Interop.mkprop "view.drawComplete" handler
    member inline _.drawingContent (handler:DrawEventArgs->unit) = Interop.mkprop "view.drawingContent" handler
    member inline _.drawingSubViews (handler:DrawEventArgs->unit) = Interop.mkprop "view.drawingSubViews" handler
    member inline _.drawingText (handler:DrawEventArgs->unit) = Interop.mkprop "view.drawingText" handler
    member inline _.enabledChanged (handler:unit->unit) = Interop.mkprop "view.enabledChanged" handler
    member inline _.focusedChanged (handler:HasFocusEventArgs->unit) = Interop.mkprop "view.focusedChanged" handler
    member inline _.frameChanged (handler:EventArgs<Rectangle>->unit) = Interop.mkprop "view.frameChanged" handler
    member inline _.gettingAttributeForRole (handler:VisualRoleEventArgs->unit) = Interop.mkprop "view.gettingAttributeForRole" handler
    member inline _.gettingScheme (handler:ResultEventArgs<Scheme>->unit) = Interop.mkprop "view.gettingScheme" handler
    member inline _.handlingHotKey (handler:CommandEventArgs->unit) = Interop.mkprop "view.handlingHotKey" handler
    member inline _.hasFocusChanged (handler:HasFocusEventArgs->unit) = Interop.mkprop "view.hasFocusChanged" handler
    member inline _.hasFocusChanging (handler:HasFocusEventArgs->unit) = Interop.mkprop "view.hasFocusChanging" handler
    member inline _.hotKeyChanged (handler:KeyChangedEventArgs->unit) = Interop.mkprop "view.hotKeyChanged" handler
    member inline _.initialized (handler:unit->unit) = Interop.mkprop "view.initialized" handler
    member inline _.keyDown (handler:Key->unit) = Interop.mkprop "view.keyDown" handler
    member inline _.keyDownNotHandled (handler:Key->unit) = Interop.mkprop "view.keyDownNotHandled" handler
    member inline _.keyUp (handler:Key->unit) = Interop.mkprop "view.keyUp" handler
    member inline _.mouseClick (handler:MouseEventArgs->unit) = Interop.mkprop "view.mouseClick" handler
    member inline _.mouseEnter (handler:CancelEventArgs->unit) = Interop.mkprop "view.mouseEnter" handler
    member inline _.mouseEvent (handler:MouseEventArgs->unit) = Interop.mkprop "view.mouseEvent" handler
    member inline _.mouseLeave (handler:EventArgs->unit) = Interop.mkprop "view.mouseLeave" handler
    member inline _.mouseStateChanged (handler:EventArgs->unit) = Interop.mkprop "view.mouseStateChanged" handler
    member inline _.mouseWheel (handler:MouseEventArgs->unit) = Interop.mkprop "view.mouseWheel" handler
    member inline _.removed (handler:SuperViewChangedEventArgs->unit) = Interop.mkprop "view.removed" handler
    member inline _.schemeChanged (handler:ValueChangedEventArgs<Scheme>->unit) = Interop.mkprop "view.schemeChanged" handler
    member inline _.schemeChanging (handler:ValueChangingEventArgs<Scheme>->unit) = Interop.mkprop "view.schemeChanging" handler
    member inline _.schemeNameChanged (handler:ValueChangedEventArgs<string>->unit) = Interop.mkprop "view.schemeNameChanged" handler
    member inline _.schemeNameChanging (handler:ValueChangingEventArgs<string>->unit) = Interop.mkprop "view.schemeNameChanging" handler
    member inline _.selecting (handler:CommandEventArgs->unit) = Interop.mkprop "view.selecting" handler
    member inline _.subViewAdded (handler:SuperViewChangedEventArgs->unit) = Interop.mkprop "view.subViewAdded" handler
    member inline _.subViewLayout (handler:LayoutEventArgs->unit) = Interop.mkprop "view.subViewLayout" handler
    member inline _.subViewRemoved (handler:SuperViewChangedEventArgs->unit) = Interop.mkprop "view.subViewRemoved" handler
    member inline _.subViewsLaidOut (handler:LayoutEventArgs->unit) = Interop.mkprop "view.subViewsLaidOut" handler
    member inline _.superViewChanged (handler:SuperViewChangedEventArgs->unit) = Interop.mkprop "view.superViewChanged" handler
    member inline _.textChanged (handler:unit->unit) = Interop.mkprop "view.textChanged" handler
    member inline _.titleChanged (handler:string->unit) = Interop.mkprop "view.titleChanged" handler
    member inline _.titleChanging (handler:App.CancelEventArgs<string>->unit) = Interop.mkprop "view.titleChanging" handler
    member inline _.viewportChanged (handler:DrawEventArgs->unit) = Interop.mkprop "view.viewportChanged" handler
    member inline _.visibleChanged (handler:unit->unit) = Interop.mkprop "view.visibleChanged" handler
    member inline _.visibleChanging (handler:unit->unit) = Interop.mkprop "view.visibleChanging" handler

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
    member inline _.diagnostics (value:ViewDiagnosticFlags) = Interop.mkprop "adornment.diagnostics" value
    member inline _.superViewRendersLineCanvas (value:bool) = Interop.mkprop "adornment.superViewRendersLineCanvas" value
    member inline _.thickness (value:Thickness) = Interop.mkprop "adornment.thickness" value
    member inline _.viewport (value:Rectangle) = Interop.mkprop "adornment.viewport" value
    // Events
    member inline _.thicknessChanged (handler:unit->unit) = Interop.mkprop "adornment.thicknessChanged" handler

// Bar
type bar() =
    inherit view()
    // Properties
    member inline _.alignmentModes (value:AlignmentModes) = Interop.mkprop "bar.alignmentModes" value
    member inline _.orientation (value:Orientation) = Interop.mkprop "bar.orientation" value
    // Events
    member inline _.orientationChanged (handler:Orientation->unit) = Interop.mkprop "bar.orientationChanged" handler
    member inline _.orientationChanging (handler:App.CancelEventArgs<Orientation>->unit) = Interop.mkprop "bar.orientationChanging" handler

// Border
type border() =
    inherit adornment()
    // Properties
    member inline _.lineStyle (value:LineStyle) = Interop.mkprop "border.lineStyle" value
    member inline _.settings (value:BorderSettings) = Interop.mkprop "border.settings" value

// Button
type button() =
    inherit view()
    // Properties
    member inline _.hotKeySpecifier (value:Rune) = Interop.mkprop "button.hotKeySpecifier" value
    member inline _.isDefault (value:bool) = Interop.mkprop "button.isDefault" value
    member inline _.noDecorations (value:bool) = Interop.mkprop "button.noDecorations" value
    member inline _.noPadding (value:bool) = Interop.mkprop "button.noPadding" value
    member inline _.text (value:string) = Interop.mkprop "button.text" value
    member inline _.wantContinuousButtonPressed (value:bool) = Interop.mkprop "button.wantContinuousButtonPressed" value

// CheckBox
type checkBox() =
    inherit view()
    // Properties
    member inline _.allowCheckStateNone (value:bool) = Interop.mkprop "checkBox.allowCheckStateNone" value
    member inline _.checkedState (value:CheckState) = Interop.mkprop "checkBox.checkedState" value
    member inline _.hotKeySpecifier (value:Rune) = Interop.mkprop "checkBox.hotKeySpecifier" value
    member inline _.radioStyle (value:bool) = Interop.mkprop "checkBox.radioStyle" value
    member inline _.text (value:string) = Interop.mkprop "checkBox.text" value
    // Events
    member inline _.checkedStateChanging (handler:ResultEventArgs<CheckState>->unit) = Interop.mkprop "checkBox.checkedStateChanging" handler

    member inline _.ischecked (value:bool) = Interop.mkprop "checkBox.checkedState" (if value then CheckState.Checked else CheckState.UnChecked)


// ColorPicker
type colorPicker() =
    inherit view()
    // Properties
    member inline _.selectedColor (value:Color) = Interop.mkprop "colorPicker.selectedColor" value
    member inline _.style (value:ColorPickerStyle) = Interop.mkprop "colorPicker.style" value
    // Events
    member inline _.colorChanged (handler:ResultEventArgs<Color>->unit) = Interop.mkprop "colorPicker.colorChanged" handler

// ColorPicker16
type colorPicker16() =
    inherit view()
    // Properties
    member inline _.boxHeight (value:Int32) = Interop.mkprop "colorPicker16.boxHeight" value
    member inline _.boxWidth (value:Int32) = Interop.mkprop "colorPicker16.boxWidth" value
    member inline _.cursor (value:Point) = Interop.mkprop "colorPicker16.cursor" value
    member inline _.selectedColor (value:ColorName16) = Interop.mkprop "colorPicker16.selectedColor" value
    // Events
    member inline _.colorChanged (handler:ResultEventArgs<Color>->unit) = Interop.mkprop "colorPicker16.colorChanged" handler

// ComboBox
type comboBox() =
    inherit view()
    // Properties
    member inline _.hideDropdownListOnClick (value:bool) = Interop.mkprop "comboBox.hideDropdownListOnClick" value
    member inline _.readOnly (value:bool) = Interop.mkprop "comboBox.readOnly" value
    member inline _.searchText (value:string) = Interop.mkprop "comboBox.searchText" value
    member inline _.selectedItem (value:Int32) = Interop.mkprop "comboBox.selectedItem" value
    member inline _.source (value:string list) = Interop.mkprop "comboBox.source" value
    member inline _.text (value:string) = Interop.mkprop "comboBox.text" value
    // Events
    member inline _.collapsed (handler:unit->unit) = Interop.mkprop "comboBox.collapsed" handler
    member inline _.expanded (handler:unit->unit) = Interop.mkprop "comboBox.expanded" handler
    member inline _.openSelectedItem (handler:ListViewItemEventArgs->unit) = Interop.mkprop "comboBox.openSelectedItem" handler
    member inline _.selectedItemChanged (handler:ListViewItemEventArgs->unit) = Interop.mkprop "comboBox.selectedItemChanged" handler

// TextField
type textField() =
    inherit view()
    // Properties
    member inline _.autocomplete (value:IAutocomplete) = Interop.mkprop "textField.autocomplete" value
    member inline _.caption (value:string) = Interop.mkprop "textField.caption" value
    member inline _.captionColor (value:Terminal.Gui.Drawing.Color) = Interop.mkprop "textField.captionColor" value
    member inline _.cursorPosition (value:Int32) = Interop.mkprop "textField.cursorPosition" value
    member inline _.readOnly (value:bool) = Interop.mkprop "textField.readOnly" value
    member inline _.secret (value:bool) = Interop.mkprop "textField.secret" value
    member inline _.selectedStart (value:Int32) = Interop.mkprop "textField.selectedStart" value
    member inline _.selectWordOnlyOnDoubleClick (value:bool) = Interop.mkprop "textField.selectWordOnlyOnDoubleClick" value
    member inline _.text (value:string) = Interop.mkprop "textField.text" value
    member inline _.used (value:bool) = Interop.mkprop "textField.used" value
    member inline _.useSameRuneTypeForWords (value:bool) = Interop.mkprop "textField.useSameRuneTypeForWords" value
    // Events
    member inline _.textChanging (handler:ResultEventArgs<string>->unit) = Interop.mkprop "textField.textChanging" handler

// DateField
type dateField() =
    inherit textField()
    // Properties
    member inline _.culture (value:CultureInfo) = Interop.mkprop "dateField.culture" value
    member inline _.cursorPosition (value:Int32) = Interop.mkprop "dateField.cursorPosition" value
    member inline _.date (value:DateTime) = Interop.mkprop "dateField.date" value
    // Events
    member inline _.dateChanged (handler:DateTimeEventArgs<DateTime>->unit) = Interop.mkprop "dateField.dateChanged" handler

// DatePicker
type datePicker() =
    inherit view()
    // Properties
    member inline _.culture (value:CultureInfo) = Interop.mkprop "datePicker.culture" value
    member inline _.date (value:DateTime) = Interop.mkprop "datePicker.date" value

// Toplevel
type toplevel() =
    inherit view()
    // Properties
    member inline _.modal (value:bool) = Interop.mkprop "toplevel.modal" value
    member inline _.running (value:bool) = Interop.mkprop "toplevel.running" value
    // Events
    member inline _.activate (handler:ToplevelEventArgs->unit) = Interop.mkprop "toplevel.activate" handler
    member inline _.closed (handler:ToplevelEventArgs->unit) = Interop.mkprop "toplevel.closed" handler
    member inline _.closing (handler:ToplevelClosingEventArgs->unit) = Interop.mkprop "toplevel.closing" handler
    member inline _.deactivate (handler:ToplevelEventArgs->unit) = Interop.mkprop "toplevel.deactivate" handler
    member inline _.loaded (handler:unit->unit) = Interop.mkprop "toplevel.loaded" handler
    member inline _.ready (handler:unit->unit) = Interop.mkprop "toplevel.ready" handler
    member inline _.sizeChanging (handler:SizeChangedEventArgs->unit) = Interop.mkprop "toplevel.sizeChanging" handler
    member inline _.unloaded (handler:unit->unit) = Interop.mkprop "toplevel.unloaded" handler

// Dialog
type dialog() =
    inherit toplevel()
    // Properties
    member inline _.buttonAlignment (value:Alignment) = Interop.mkprop "dialog.buttonAlignment" value
    member inline _.buttonAlignmentModes (value:AlignmentModes) = Interop.mkprop "dialog.buttonAlignmentModes" value
    member inline _.canceled (value:bool) = Interop.mkprop "dialog.canceled" value

// FileDialog
type fileDialog() =
    inherit dialog()
    // Properties
    member inline _.allowedTypes (value:IAllowedType list) = Interop.mkprop "fileDialog.allowedTypes" value
    member inline _.allowsMultipleSelection (value:bool) = Interop.mkprop "fileDialog.allowsMultipleSelection" value
    member inline _.fileOperationsHandler (value:IFileOperations) = Interop.mkprop "fileDialog.fileOperationsHandler" value
    member inline _.mustExist (value:bool) = Interop.mkprop "fileDialog.mustExist" value
    member inline _.openMode (value:OpenMode) = Interop.mkprop "fileDialog.openMode" value
    member inline _.path (value:string) = Interop.mkprop "fileDialog.path" value
    member inline _.searchMatcher (value:ISearchMatcher) = Interop.mkprop "fileDialog.searchMatcher" value
    // Events
    member inline _.filesSelected (handler:FilesSelectedEventArgs->unit) = Interop.mkprop "fileDialog.filesSelected" handler

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
    member inline _.axisX (value:HorizontalAxis) = Interop.mkprop "graphView.axisX" value
    member inline _.axisY (value:VerticalAxis) = Interop.mkprop "graphView.axisY" value
    member inline _.cellSize (value:PointF) = Interop.mkprop "graphView.cellSize" value
    member inline _.graphColor (value:Attribute option) = Interop.mkprop "graphView.graphColor" value
    member inline _.marginBottom (value:int) = Interop.mkprop "graphView.marginBottom" value
    member inline _.marginLeft (value:int) = Interop.mkprop "graphView.marginLeft" value
    member inline _.scrollOffset (value:PointF) = Interop.mkprop "graphView.scrollOffset" value

// HexView
type hexView() =
    inherit view()

    // Properties
    member inline _.address (value:Int64) = Interop.mkprop "hexView.address" value
    member inline _.addressWidth (value:int) = Interop.mkprop "hexView.addressWidth" value
    member inline _.allowEdits (value:int) = Interop.mkprop "hexView.allowEdits" value
    member inline _.readOnly (value:bool) = Interop.mkprop "hexView.readOnly" value
    member inline _.source (value:Stream) = Interop.mkprop "hexView.source" value
    // Events
    member inline _.edited (handler:HexViewEditEventArgs->unit) = Interop.mkprop "hexView.edited" handler
    member inline _.positionChanged (handler:HexViewEventArgs->unit) = Interop.mkprop "hexView.positionChanged" handler

// Label
type label() =
    inherit view()

    // Properties
    member inline _.hotKeySpecifier (value:Rune) = Interop.mkprop "label.hotKeySpecifier" value
    member inline _.text (value:string) = Interop.mkprop "label.text" value

// LegendAnnotation
type legendAnnotation() =
    inherit view()
// No properties or events LegendAnnotation

// Line
type line() =
    inherit view()

    // Properties
    member inline _.orientation (value:Orientation) = Interop.mkprop "line.orientation" value
    // Events
    member inline _.orientationChanged (handler:Orientation->unit) = Interop.mkprop "line.orientationChanged" handler
    member inline _.orientationChanging (handler:App.CancelEventArgs<Orientation>->unit) = Interop.mkprop "line.orientationChanging" handler

// LineView
type lineView() =
    inherit view()

    // Properties
    member inline _.endingAnchor (value:Rune option) = Interop.mkprop "lineView.endingAnchor" value
    member inline _.lineRune (value:Rune) = Interop.mkprop "lineView.lineRune" value
    member inline _.orientation (value:Orientation) = Interop.mkprop "lineView.orientation" value
    member inline _.startingAnchor (value:Rune option) = Interop.mkprop "lineView.startingAnchor" value

// ListView
type listView() =
    inherit view()

    // Properties
    member inline _.allowsMarking (value:bool) = Interop.mkprop "listView.allowsMarking" value
    member inline _.allowsMultipleSelection (value:bool) = Interop.mkprop "listView.allowsMultipleSelection" value
    member inline _.leftItem (value:Int32) = Interop.mkprop "listView.leftItem" value
    member inline _.selectedItem (value:Int32) = Interop.mkprop "listView.selectedItem" value
    member inline _.source (value:string list) = Interop.mkprop "listView.source" value
    member inline _.topItem (value:Int32) = Interop.mkprop "listView.topItem" value
    // Events
    member inline _.collectionChanged (handler:NotifyCollectionChangedEventArgs->unit) = Interop.mkprop "listView.collectionChanged" handler
    member inline _.openSelectedItem (handler:ListViewItemEventArgs->unit) = Interop.mkprop "listView.openSelectedItem" handler
    member inline _.rowRender (handler:ListViewRowEventArgs->unit) = Interop.mkprop "listView.rowRender" handler
    member inline _.selectedItemChanged (handler:ListViewItemEventArgs->unit) = Interop.mkprop "listView.selectedItemChanged" handler

// Margin
type margin() =
    inherit adornment()

    // Properties
    member inline _.shadowStyle (value:ShadowStyle) = Interop.mkprop "margin.shadowStyle" value

type menuv2() =
    inherit bar()

    // Properties
    member inline _.selectedMenuItem (value: MenuItemv2 list) = Interop.mkprop "menuv2.selectedMenuItem" value
    member inline _.superMenuItem (value: MenuItemv2 list) = Interop.mkprop "menuv2.superMenuItem" value
    // Events
    member inline _.accepted (value: CommandEventArgs->unit) = Interop.mkprop "menuv2.accepted" value
    member inline _.selectedMenuItemChanged (value: MenuItemv2->unit) = Interop.mkprop "menuv2.selectedMenuItemChanged" value

// MenuBarV2
type menuBarv2() =
    inherit menuv2()

    // Properties
    member inline _.key (value:Key) = Interop.mkprop "menuBarv2.key" value
    member inline _.menus (value:MenuBarItemv2Element list) = Interop.mkprop "children" (value |> List.map (fun v -> v :> TerminalElement))
    // Events
    member inline _.keyChanged (handler:KeyChangedEventArgs->unit) = Interop.mkprop "menuBarv2.keyChanged" handler

type shortcut() =
     inherit view()

     // Properties
     member inline _.action (value:Action) = Interop.mkprop "shortcut.action" value
     member inline _.alignmentModes (value:AlignmentModes) = Interop.mkprop "shortcut.alignmentModes" value
     member inline _.forceFocusColors (value:bool) = Interop.mkprop "shortcut.forceFocusColors" value
     member inline _.helpText (value:string) = Interop.mkprop "shortcut.helpText" value
     member inline _.text (value:string) = Interop.mkprop "shortcut.text" value
     member inline _.bindKeyToApplication (value:bool) = Interop.mkprop "shortcut.bindKeyToApplication" value
     member inline _.key (value:Key) = Interop.mkprop "shortcut.key" value
     member inline _.minimumKeyTextSize (value:Int32) = Interop.mkprop "shortcut.minimumKeyTextSize" value
     // Events
     member inline _.orientationChanged (handler:Orientation->unit) = Interop.mkprop "shortcut.orientationChanged" handler
     member inline _.orientationChanging (handler:CancelEventArgs<Orientation>->unit) = Interop.mkprop "shortcut.orientationChanging" handler

type menuItemv2() =
    inherit shortcut()
    member inline _.command (value: Command) = Interop.mkprop "menuItemv2.command" value
    member inline _.submenu(value: Menuv2Element) = Interop.mkprop "menuItemv2.subMenu.element" value
    member inline _.accepted(value: CommandEventArgs -> unit) = Interop.mkprop "menuItemv2.accepted" value

type menuBarItemv2() =
    inherit menuItemv2()
    member inline _.popoverMenu (value:PopoverMenuElement) = Interop.mkprop "menuBarItemv2.popoverMenu.element" value
    member inline _.popoverMenuOpen (value:bool) = Interop.mkprop "menuBarItemv2.popoverMenuOpen" value

type popoverMenu() =
    inherit view()

    member inline _.key (value:Key) = Interop.mkprop "popoverMenu.key" value
    member inline _.root (value: Menuv2Element) = Interop.mkprop "popoverMenu.root.element" value


// NumericUpDown`1
type numericUpDown() =
    inherit view()

    // Properties
    member inline _.format (value:string) = Interop.mkprop "numericUpDown`1.format" value
    member inline _.increment (value:'a) = Interop.mkprop "numericUpDown`1.increment" value
    member inline _.value (value:'a) = Interop.mkprop "numericUpDown`1.value" value
    // Events
    member inline _.formatChanged (handler:string->unit) = Interop.mkprop "numericUpDown`1.formatChanged" handler
    member inline _.incrementChanged (handler:'a->unit) = Interop.mkprop "numericUpDown`1.incrementChanged" handler
    member inline _.valueChanged (handler:'a->unit) = Interop.mkprop "numericUpDown`1.valueChanged" handler
    member inline _.valueChanging (handler:App.CancelEventArgs<'a>->unit) = Interop.mkprop "numericUpDown`1.valueChanging" handler

// NumericUpDown
// No properties or events NumericUpDown

// OpenDialog
type openDialog() =
    inherit fileDialog()
    // Properties
    member inline _.openMode (value:OpenMode) = Interop.mkprop "openDialog.openMode" value

// Padding
type padding() =
    inherit adornment()

// ProgressBar
type progressBar() =
    inherit view()

    // Properties
    member inline _.bidirectionalMarquee (value:bool) = Interop.mkprop "progressBar.bidirectionalMarquee" value
    member inline _.fraction (value:Single) = Interop.mkprop "progressBar.fraction" value
    member inline _.progressBarFormat (value:ProgressBarFormat) = Interop.mkprop "progressBar.progressBarFormat" value
    member inline _.progressBarStyle (value:ProgressBarStyle) = Interop.mkprop "progressBar.progressBarStyle" value
    member inline _.segmentCharacter (value:Rune) = Interop.mkprop "progressBar.segmentCharacter" value
    member inline _.text (value:string) = Interop.mkprop "progressBar.text" value

// RadioGroup
type radioGroup() =
    inherit view()

    // Properties
    member inline _.assignHotKeysToRadioLabels (value:bool) = Interop.mkprop "radioGroup.assignHotKeysToRadioLabels" value
    member inline _.cursor (value:Int32) = Interop.mkprop "radioGroup.cursor" value
    member inline _.doubleClickAccepts (value:bool) = Interop.mkprop "radioGroup.doubleClickAccepts" value
    member inline _.horizontalSpace (value:Int32) = Interop.mkprop "radioGroup.horizontalSpace" value
    member inline _.orientation (value:Orientation) = Interop.mkprop "radioGroup.orientation" value
    member inline _.radioLabels (value:string list) = Interop.mkprop "radioGroup.radioLabels" value
    member inline _.selectedItem (value:Int32) = Interop.mkprop "radioGroup.selectedItem" value
    // Events
    member inline _.orientationChanged (handler:Orientation->unit) = Interop.mkprop "radioGroup.orientationChanged" handler
    member inline _.orientationChanging (handler:App.CancelEventArgs<Orientation>->unit) = Interop.mkprop "radioGroup.orientationChanging" handler
    member inline _.selectedItemChanged (handler:SelectedItemChangedArgs->unit) = Interop.mkprop "radioGroup.selectedItemChanged" handler

// SaveDialog
// No properties or events SaveDialog

// ScrollBar
type scrollBar() =
    inherit view()

    // Properties
    member inline _.autoShow (value:bool) = Interop.mkprop "scrollBar.autoShow" value
    member inline _.increment (value:Int32) = Interop.mkprop "scrollBar.increment" value
    member inline _.orientation (value:Orientation) = Interop.mkprop "scrollBar.orientation" value
    member inline _.position (value:Int32) = Interop.mkprop "scrollBar.position" value
    member inline _.scrollableContentSize (value:Int32) = Interop.mkprop "scrollBar.scrollableContentSize" value
    member inline _.visibleContentSize (value:Int32) = Interop.mkprop "scrollBar.visibleContentSize" value
    // Events
    member inline _.orientationChanged (handler:Orientation->unit) = Interop.mkprop "scrollBar.orientationChanged" handler
    member inline _.orientationChanging (handler:CancelEventArgs<Orientation>->unit) = Interop.mkprop "scrollBar.orientationChanging" handler
    member inline _.scrollableContentSizeChanged (handler:EventArgs<Int32>->unit) = Interop.mkprop "scrollBar.scrollableContentSizeChanged" handler
    member inline _.sliderPositionChanged (handler:EventArgs<Int32>->unit) = Interop.mkprop "scrollBar.sliderPositionChanged" handler

// ScrollSlider
type scrollSlider() =
    inherit view()

    // Properties
    member inline _.orientation (value:Orientation) = Interop.mkprop "scrollSlider.orientation" value
    member inline _.position (value:Int32) = Interop.mkprop "scrollSlider.position" value
    member inline _.size (value:Int32) = Interop.mkprop "scrollSlider.size" value
    member inline _.sliderPadding (value:Int32) = Interop.mkprop "scrollSlider.sliderPadding" value
    member inline _.visibleContentSize (value:Int32) = Interop.mkprop "scrollSlider.visibleContentSize" value
    // Events
    member inline _.orientationChanged (handler:Orientation->unit) = Interop.mkprop "scrollSlider.orientationChanged" handler
    member inline _.orientationChanging (handler:CancelEventArgs<Orientation>->unit) = Interop.mkprop "scrollSlider.orientationChanging" handler
    member inline _.positionChanged (handler:EventArgs<Int32>->unit) = Interop.mkprop "scrollSlider.positionChanged" handler
    member inline _.positionChanging (handler:CancelEventArgs<Int32>->unit) = Interop.mkprop "scrollSlider.positionChanging" handler
    member inline _.scrolled (handler:EventArgs<Int32>->unit) = Interop.mkprop "scrollSlider.scrolled" handler

// Slider`1
type slider() =
    inherit view()

    // Properties
    member inline _.allowEmpty (value:bool) = Interop.mkprop "slider`1.allowEmpty" value
    member inline _.focusedOption (value:Int32) = Interop.mkprop "slider`1.focusedOption" value
    member inline _.legendsOrientation (value:Orientation) = Interop.mkprop "slider`1.legendsOrientation" value
    member inline _.minimumInnerSpacing (value:Int32) = Interop.mkprop "slider`1.minimumInnerSpacing" value
    member inline _.options (value:SliderOption<'a> list) = Interop.mkprop "slider`1.options" value
    member inline _.orientation (value:Orientation) = Interop.mkprop "slider`1.orientation" value
    member inline _.rangeAllowSingle (value:bool) = Interop.mkprop "slider`1.rangeAllowSingle" value
    member inline _.showEndSpacing (value:bool) = Interop.mkprop "slider`1.showEndSpacing" value
    member inline _.showLegends (value:bool) = Interop.mkprop "slider`1.showLegends" value
    member inline _.style (value:SliderStyle) = Interop.mkprop "slider`1.style" value
    member inline _.text (value:string) = Interop.mkprop "slider`1.text" value
    member inline _.``type`` (value:SliderType) = Interop.mkprop "slider`1.``type``" value
    member inline _.useMinimumSize (value:bool) = Interop.mkprop "slider`1.useMinimumSize" value
    // Events
    member inline _.optionFocused (handler:SliderEventArgs<'a>->unit) = Interop.mkprop "slider`1.optionFocused" handler
    member inline _.optionsChanged (handler:SliderEventArgs<'a>->unit) = Interop.mkprop "slider`1.optionsChanged" handler
    member inline _.orientationChanged (handler:Orientation->unit) = Interop.mkprop "slider`1.orientationChanged" handler
    member inline _.orientationChanging (handler:App.CancelEventArgs<Orientation>->unit) = Interop.mkprop "slider`1.orientationChanging" handler

// Slider
// No properties or events Slider

// SpinnerView
type spinnerView() =
    inherit view()

    // Properties
    member inline _.autoSpin (value:bool) = Interop.mkprop "spinnerView.autoSpin" value
    member inline _.sequence (value:string list) = Interop.mkprop "spinnerView.sequence" value
    member inline _.spinBounce (value:bool) = Interop.mkprop "spinnerView.spinBounce" value
    member inline _.spinDelay (value:Int32) = Interop.mkprop "spinnerView.spinDelay" value
    member inline _.spinReverse (value:bool) = Interop.mkprop "spinnerView.spinReverse" value
    member inline _.style (value:SpinnerStyle) = Interop.mkprop "spinnerView.style" value

// StatusBar
type statusBar() =
    inherit bar()
// No properties or events StatusBar

// Tab
type tab() =
    inherit view()

    // Properties
    member inline _.displayText (value:string) = Interop.mkprop "tab.displayText" value
    member inline _.view (value:TerminalElement) = Interop.mkprop "tab.view" value

// TabView
type tabView() =
    inherit view()

    // Properties
    member inline _.maxTabTextWidth (value:int) = Interop.mkprop "tabView.maxTabTextWidth" value
    member inline _.selectedTab (value:Tab) = Interop.mkprop "tabView.selectedTab" value
    member inline _.style (value:TabStyle) = Interop.mkprop "tabView.style" value
    member inline _.tabScrollOffset (value:Int32) = Interop.mkprop "tabView.tabScrollOffset" value
    // Events
    member inline _.selectedTabChanged (handler:TabChangedEventArgs->unit) = Interop.mkprop "tabView.selectedTabChanged" handler
    member inline _.tabClicked (handler:TabMouseEventArgs->unit) = Interop.mkprop "tabView.tabClicked" handler

    member inline _.tabs (value:TerminalElement list) = Interop.mkprop "tabView.tabs" value


// TableView
type tableView() =
    inherit view()

    // Properties
    member inline _.cellActivationKey (value:KeyCode) = Interop.mkprop "tableView.cellActivationKey" value
    member inline _.collectionNavigator (value:ICollectionNavigator) = Interop.mkprop "tableView.collectionNavigator" value
    member inline _.columnOffset (value:Int32) = Interop.mkprop "tableView.columnOffset" value
    member inline _.fullRowSelect (value:bool) = Interop.mkprop "tableView.fullRowSelect" value
    member inline _.maxCellWidth (value:Int32) = Interop.mkprop "tableView.maxCellWidth" value
    member inline _.minCellWidth (value:Int32) = Interop.mkprop "tableView.minCellWidth" value
    member inline _.multiSelect (value:bool) = Interop.mkprop "tableView.multiSelect" value
    member inline _.nullSymbol (value:string) = Interop.mkprop "tableView.nullSymbol" value
    member inline _.rowOffset (value:Int32) = Interop.mkprop "tableView.rowOffset" value
    member inline _.selectedColumn (value:Int32) = Interop.mkprop "tableView.selectedColumn" value
    member inline _.selectedRow (value:Int32) = Interop.mkprop "tableView.selectedRow" value
    member inline _.separatorSymbol (value:Char) = Interop.mkprop "tableView.separatorSymbol" value
    member inline _.style (value:TableStyle) = Interop.mkprop "tableView.style" value
    member inline _.table (value:ITableSource) = Interop.mkprop "tableView.table" value
    // Events
    member inline _.cellActivated (handler:CellActivatedEventArgs->unit) = Interop.mkprop "tableView.cellActivated" handler
    member inline _.cellToggled (handler:CellToggledEventArgs->unit) = Interop.mkprop "tableView.cellToggled" handler
    member inline _.selectedCellChanged (handler:SelectedCellChangedEventArgs->unit) = Interop.mkprop "tableView.selectedCellChanged" handler


// TextValidateField
type textValidateField() =
    inherit view()

    // Properties
    member inline _.provider (value:ITextValidateProvider) = Interop.mkprop "textValidateField.provider" value
    member inline _.text (value:string) = Interop.mkprop "textValidateField.text" value

// TextView
type textView() =
    inherit view()

    // Properties
    member inline _.allowsReturn (value:bool) = Interop.mkprop "textView.allowsReturn" value
    member inline _.allowsTab (value:bool) = Interop.mkprop "textView.allowsTab" value
    member inline _.cursorPosition (value:Point) = Interop.mkprop "textView.cursorPosition" value
    member inline _.inheritsPreviousAttribute (value:bool) = Interop.mkprop "textView.inheritsPreviousAttribute" value
    member inline _.isDirty (value:bool) = Interop.mkprop "textView.isDirty" value
    member inline _.isSelecting (value:bool) = Interop.mkprop "textView.isSelecting" value
    member inline _.leftColumn (value:Int32) = Interop.mkprop "textView.leftColumn" value
    member inline _.multiline (value:bool) = Interop.mkprop "textView.multiline" value
    member inline _.readOnly (value:bool) = Interop.mkprop "textView.readOnly" value
    member inline _.selectionStartColumn (value:Int32) = Interop.mkprop "textView.selectionStartColumn" value
    member inline _.selectionStartRow (value:Int32) = Interop.mkprop "textView.selectionStartRow" value
    member inline _.selectWordOnlyOnDoubleClick (value:bool) = Interop.mkprop "textView.selectWordOnlyOnDoubleClick" value
    member inline _.tabWidth (value:Int32) = Interop.mkprop "textView.tabWidth" value
    member inline _.text (value:string) = Interop.mkprop "textView.text" value
    member inline _.topRow (value:Int32) = Interop.mkprop "textView.topRow" value
    member inline _.used (value:bool) = Interop.mkprop "textView.used" value
    member inline _.useSameRuneTypeForWords (value:bool) = Interop.mkprop "textView.useSameRuneTypeForWords" value
    member inline _.wordWrap (value:bool) = Interop.mkprop "textView.wordWrap" value
    // Events
    member inline _.contentsChanged (handler:ContentsChangedEventArgs->unit) = Interop.mkprop "textView.contentsChanged" handler
    member inline _.drawNormalColor (handler:CellEventArgs->unit) = Interop.mkprop "textView.drawNormalColor" handler
    member inline _.drawReadOnlyColor (handler:CellEventArgs->unit) = Interop.mkprop "textView.drawReadOnlyColor" handler
    member inline _.drawSelectionColor (handler:CellEventArgs->unit) = Interop.mkprop "textView.drawSelectionColor" handler
    member inline _.drawUsedColor (handler:CellEventArgs->unit) = Interop.mkprop "textView.drawUsedColor" handler
    member inline _.unwrappedCursorPosition (handler:Point->unit) = Interop.mkprop "textView.unwrappedCursorPosition" handler
    // Additional properties
    member inline _.textChanged (value:string->unit) = Interop.mkprop "textView.textChanged" value

// TileView
type tileView() =
    inherit view()

    // Properties
    member inline _.lineStyle (value:LineStyle) = Interop.mkprop "tileView.lineStyle" value
    member inline _.orientation (value:Orientation) = Interop.mkprop "tileView.orientation" value
    member inline _.toggleResizable (value:KeyCode) = Interop.mkprop "tileView.toggleResizable" value
    // Events
    member inline _.splitterMoved (handler:SplitterEventArgs->unit) = Interop.mkprop "tileView.splitterMoved" handler

// TimeField
type timeField() =
    inherit textField()

    // Properties
    member inline _.cursorPosition (value:Int32) = Interop.mkprop "timeField.cursorPosition" value
    member inline _.isShortFormat (value:bool) = Interop.mkprop "timeField.isShortFormat" value
    member inline _.time (value:TimeSpan) = Interop.mkprop "timeField.time" value
    // Events
    member inline _.timeChanged (handler:DateTimeEventArgs<TimeSpan>->unit) = Interop.mkprop "timeField.timeChanged" handler

// TreeView`1
type treeView<'a when 'a : not struct>() =
    inherit view()

    // Properties
    member inline _.allowLetterBasedNavigation (value:bool) = Interop.mkprop "treeView`1.allowLetterBasedNavigation" value
    member inline _.aspectGetter<'a when 'a : not struct> (value:AspectGetterDelegate<'a>) = Interop.mkprop "treeView`1.aspectGetter" value
    member inline _.colorGetter<'a when 'a : not struct> (value:Func<'a,Scheme>) = Interop.mkprop "treeView`1.colorGetter" value
    member inline _.maxDepth (value:Int32) = Interop.mkprop "treeView`1.maxDepth" value
    member inline _.multiSelect (value:bool) = Interop.mkprop "treeView`1.multiSelect" value
    member inline _.objectActivationButton (value:MouseFlags option) = Interop.mkprop "treeView`1.objectActivationButton" value
    member inline _.objectActivationKey (value:KeyCode) = Interop.mkprop "treeView`1.objectActivationKey" value
    member inline _.scrollOffsetHorizontal (value:Int32) = Interop.mkprop "treeView`1.scrollOffsetHorizontal" value
    member inline _.scrollOffsetVertical (value:Int32) = Interop.mkprop "treeView`1.scrollOffsetVertical" value
    member inline _.selectedObject<'a when 'a : not struct> (value:'a) = Interop.mkprop "treeView`1.selectedObject" value
    member inline _.style (value:TreeStyle) = Interop.mkprop "treeView`1.style" value
    member inline _.treeBuilder<'a when 'a : not struct> (value:ITreeBuilder<'a>) = Interop.mkprop "treeView`1.treeBuilder" value
    // Events
    member inline _.drawLine<'a when 'a : not struct> (handler:DrawTreeViewLineEventArgs<'a>->unit) = Interop.mkprop "treeView`1.drawLine" handler
    member inline _.objectActivated<'a when 'a : not struct> (handler:ObjectActivatedEventArgs<'a>->unit) = Interop.mkprop "treeView`1.objectActivated" handler
    member inline _.selectionChanged<'a when 'a : not struct> (handler:SelectionChangedEventArgs<'a>->unit) = Interop.mkprop "treeView`1.selectionChanged" handler

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
    member inline _.currentStep (value:WizardStep) = Interop.mkprop "wizard.currentStep" value
    member inline _.modal (value:bool) = Interop.mkprop "wizard.modal" value
    // Events
    member inline _.cancelled (handler:WizardButtonEventArgs->unit) = Interop.mkprop "wizard.cancelled" handler
    member inline _.finished (handler:WizardButtonEventArgs->unit) = Interop.mkprop "wizard.finished" handler
    member inline _.movingBack (handler:WizardButtonEventArgs->unit) = Interop.mkprop "wizard.movingBack" handler
    member inline _.movingNext (handler:WizardButtonEventArgs->unit) = Interop.mkprop "wizard.movingNext" handler
    member inline _.stepChanged (handler:StepChangeEventArgs->unit) = Interop.mkprop "wizard.stepChanged" handler
    member inline _.stepChanging (handler:StepChangeEventArgs->unit) = Interop.mkprop "wizard.stepChanging" handler

// WizardStep
type wizardStep() =
    inherit view()

    // Properties
    member inline _.backButtonText (value:string) = Interop.mkprop "wizardStep.backButtonText" value
    member inline _.helpText (value:string) = Interop.mkprop "wizardStep.helpText" value
    member inline _.nextButtonText (value:string) = Interop.mkprop "wizardStep.nextButtonText" value

[<RequireQualifiedAccess>]
type prop =
    static member inline children (children:TerminalElement list) = Interop.mkprop "children" children
    static member inline ref (reference:View->unit) = Interop.mkprop "ref" reference
    static member inline adornment = adornment()
    static member inline bar = bar()
    static member inline border = border()
    static member inline button = button()
    static member inline checkBox = checkBox()
    static member inline colorPicker = colorPicker()
    static member inline colorPicker16 = colorPicker16()
    static member inline comboBox = comboBox()
    static member inline textField = textField()
    static member inline dateField = dateField()
    static member inline datePicker = datePicker()
    static member inline toplevel = toplevel()
    static member inline dialog = dialog()
    static member inline fileDialog = fileDialog()
    static member inline saveDialog = saveDialog()
    static member inline frameView = frameView()
    static member inline graphView = graphView()
    static member inline hexView = hexView()
    static member inline label = label()
    static member inline legendAnnotation = legendAnnotation()
    static member inline line = line()
    static member inline lineView = lineView()
    static member inline listView = listView()
    static member inline margin = margin()
    static member inline menuv2 = menuv2()
    static member inline menuBarv2 = menuBarv2()
    static member inline shortcut = shortcut()
    static member inline menuItemv2 = menuItemv2()
    static member inline menuBarItemv2 = menuBarItemv2()
    static member inline popoverMenu = popoverMenu()
    static member inline numericUpDown = numericUpDown()
    static member inline openDialog = openDialog()
    static member inline padding = padding()
    static member inline progressBar = progressBar()
    static member inline radioGroup = radioGroup()
    static member inline scrollBar = scrollBar()
    static member inline scrollSlider = scrollSlider()
    static member inline slider = slider()
    static member inline spinnerView = spinnerView()
    static member inline statusBar = statusBar()
    static member inline tab = tab()
    static member inline tabView = tabView()
    static member inline tableView = tableView()
    static member inline textValidateField = textValidateField()
    static member inline textView = textView()
    static member inline tileView = tileView()
    static member inline timeField = timeField()
    static member inline treeView = treeView()
    static member inline window = window()
    static member inline wizard = wizard()
    static member inline wizardStep = wizardStep()
