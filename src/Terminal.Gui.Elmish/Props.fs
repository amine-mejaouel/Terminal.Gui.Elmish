
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
    member val props = Props.init()

    member inline this.children (children:TerminalElement list) = this.props.add "children" children
    member inline this.ref (reference:View->unit) = this.props.add "ref" reference

    // Properties
    member inline this.arrangement (value:ViewArrangement) = this.props.add "view.arrangement" value
    member inline this.borderStyle (value:LineStyle) = this.props.add "view.borderStyle" value
    member inline this.canFocus (value:bool) = this.props.add "view.canFocus" value
    member inline this.contentSizeTracksViewport (value:bool) = this.props.add "view.contentSizeTracksViewport" value
    member inline this.cursorVisibility (value:CursorVisibility) = this.props.add "view.cursorVisibility" value
    member inline this.data (value:Object) = this.props.add "view.data" value
    member inline this.enabled (value:bool) = this.props.add "view.enabled" value
    member inline this.frame (value:Rectangle) = this.props.add "view.frame" value
    member inline this.hasFocus (value:bool) = this.props.add "view.hasFocus" value
    member inline this.height (value:Dim) = this.props.add "view.height" value
    member inline this.highlightStates (value:MouseState) = this.props.add "view.highlightStates" value
    member inline this.hotKey (value:Key) = this.props.add "view.hotKey" value
    member inline this.hotKeySpecifier (value:Rune) = this.props.add "view.hotKeySpecifier" value
    member inline this.id (value:string) = this.props.add "view.id" value
    member inline this.isInitialized (value:bool) = this.props.add "view.isInitialized" value
    member inline this.mouseHeldDown (value:IMouseHeldDown) = this.props.add "view.mouseHeldDown" value
    member inline this.needsDraw (value:bool) = this.props.add "view.needsDraw" value
    member inline this.preserveTrailingSpaces (value:bool) = this.props.add "view.preserveTrailingSpaces" value
    member inline this.schemeName (value:string) = this.props.add "view.schemeName" value
    member inline this.shadowStyle (value:ShadowStyle) = this.props.add "view.shadowStyle" value
    member inline this.superViewRendersLineCanvas (value:bool) = this.props.add "view.superViewRendersLineCanvas" value
    member inline this.tabStop (value:TabBehavior option) = this.props.add "view.tabStop" value
    member inline this.text (value:string) = this.props.add "view.text" value
    member inline this.textAlignment (value:Alignment) = this.props.add "view.textAlignment" value
    member inline this.textDirection (value:TextDirection) = this.props.add "view.textDirection" value
    member inline this.title (value:string) = this.props.add "view.title" value
    member inline this.validatePosDim (value:bool) = this.props.add "view.validatePosDim" value
    member inline this.verticalTextAlignment (value:Alignment) = this.props.add "view.verticalTextAlignment" value
    member inline this.viewport (value:Rectangle) = this.props.add "view.viewport" value
    member inline this.viewportSettings (value:ViewportSettingsFlags) = this.props.add "view.viewportSettings" value
    member inline this.visible (value:bool) = this.props.add "view.visible" value
    member inline this.wantContinuousButtonPressed (value:bool) = this.props.add "view.wantContinuousButtonPressed" value
    member inline this.wantMousePositionReports (value:bool) = this.props.add "view.wantMousePositionReports" value
    member inline this.width (value:Dim) = this.props.add "view.width" value
    member inline this.x (value:Pos) = this.props.add "view.x" value
    member inline this.y (value:Pos) = this.props.add "view.y" value
    // Events
    member inline this.accepting (handler:HandledEventArgs->unit) = this.props.add "view.accepting" handler
    member inline this.advancingFocus (handler:AdvanceFocusEventArgs->unit) = this.props.add "view.advancingFocus" handler
    member inline this.borderStyleChanged (handler:EventArgs->unit) = this.props.add "view.borderStyleChanged" handler
    member inline this.canFocusChanged (handler:unit->unit) = this.props.add "view.canFocusChanged" handler
    member inline this.clearedViewport (handler:DrawEventArgs->unit) = this.props.add "view.clearedViewport" handler
    member inline this.clearingViewport (handler:DrawEventArgs->unit) = this.props.add "view.clearingViewport" handler
    member inline this.commandNotBound (handler:CommandEventArgs->unit) = this.props.add "view.commandNotBound" handler
    member inline this.contentSizeChanged (handler:SizeChangedEventArgs->unit) = this.props.add "view.contentSizeChanged" handler
    member inline this.disposing (handler:unit->unit) = this.props.add "view.disposing" handler
    member inline this.drawComplete (handler:DrawEventArgs->unit) = this.props.add "view.drawComplete" handler
    member inline this.drawingContent (handler:DrawEventArgs->unit) = this.props.add "view.drawingContent" handler
    member inline this.drawingSubViews (handler:DrawEventArgs->unit) = this.props.add "view.drawingSubViews" handler
    member inline this.drawingText (handler:DrawEventArgs->unit) = this.props.add "view.drawingText" handler
    member inline this.enabledChanged (handler:unit->unit) = this.props.add "view.enabledChanged" handler
    member inline this.focusedChanged (handler:HasFocusEventArgs->unit) = this.props.add "view.focusedChanged" handler
    member inline this.frameChanged (handler:EventArgs<Rectangle>->unit) = this.props.add "view.frameChanged" handler
    member inline this.gettingAttributeForRole (handler:VisualRoleEventArgs->unit) = this.props.add "view.gettingAttributeForRole" handler
    member inline this.gettingScheme (handler:ResultEventArgs<Scheme>->unit) = this.props.add "view.gettingScheme" handler
    member inline this.handlingHotKey (handler:CommandEventArgs->unit) = this.props.add "view.handlingHotKey" handler
    member inline this.hasFocusChanged (handler:HasFocusEventArgs->unit) = this.props.add "view.hasFocusChanged" handler
    member inline this.hasFocusChanging (handler:HasFocusEventArgs->unit) = this.props.add "view.hasFocusChanging" handler
    member inline this.hotKeyChanged (handler:KeyChangedEventArgs->unit) = this.props.add "view.hotKeyChanged" handler
    member inline this.initialized (handler:unit->unit) = this.props.add "view.initialized" handler
    member inline this.keyDown (handler:Key->unit) = this.props.add "view.keyDown" handler
    member inline this.keyDownNotHandled (handler:Key->unit) = this.props.add "view.keyDownNotHandled" handler
    member inline this.keyUp (handler:Key->unit) = this.props.add "view.keyUp" handler
    member inline this.mouseClick (handler:MouseEventArgs->unit) = this.props.add "view.mouseClick" handler
    member inline this.mouseEnter (handler:CancelEventArgs->unit) = this.props.add "view.mouseEnter" handler
    member inline this.mouseEvent (handler:MouseEventArgs->unit) = this.props.add "view.mouseEvent" handler
    member inline this.mouseLeave (handler:EventArgs->unit) = this.props.add "view.mouseLeave" handler
    member inline this.mouseStateChanged (handler:EventArgs->unit) = this.props.add "view.mouseStateChanged" handler
    member inline this.mouseWheel (handler:MouseEventArgs->unit) = this.props.add "view.mouseWheel" handler
    member inline this.removed (handler:SuperViewChangedEventArgs->unit) = this.props.add "view.removed" handler
    member inline this.schemeChanged (handler:ValueChangedEventArgs<Scheme>->unit) = this.props.add "view.schemeChanged" handler
    member inline this.schemeChanging (handler:ValueChangingEventArgs<Scheme>->unit) = this.props.add "view.schemeChanging" handler
    member inline this.schemeNameChanged (handler:ValueChangedEventArgs<string>->unit) = this.props.add "view.schemeNameChanged" handler
    member inline this.schemeNameChanging (handler:ValueChangingEventArgs<string>->unit) = this.props.add "view.schemeNameChanging" handler
    member inline this.selecting (handler:CommandEventArgs->unit) = this.props.add "view.selecting" handler
    member inline this.subViewAdded (handler:SuperViewChangedEventArgs->unit) = this.props.add "view.subViewAdded" handler
    member inline this.subViewLayout (handler:LayoutEventArgs->unit) = this.props.add "view.subViewLayout" handler
    member inline this.subViewRemoved (handler:SuperViewChangedEventArgs->unit) = this.props.add "view.subViewRemoved" handler
    member inline this.subViewsLaidOut (handler:LayoutEventArgs->unit) = this.props.add "view.subViewsLaidOut" handler
    member inline this.superViewChanged (handler:SuperViewChangedEventArgs->unit) = this.props.add "view.superViewChanged" handler
    member inline this.textChanged (handler:unit->unit) = this.props.add "view.textChanged" handler
    member inline this.titleChanged (handler:string->unit) = this.props.add "view.titleChanged" handler
    member inline this.titleChanging (handler:App.CancelEventArgs<string>->unit) = this.props.add "view.titleChanging" handler
    member inline this.viewportChanged (handler:DrawEventArgs->unit) = this.props.add "view.viewportChanged" handler
    member inline this.visibleChanged (handler:unit->unit) = this.props.add "view.visibleChanged" handler
    member inline this.visibleChanging (handler:unit->unit) = this.props.add "view.visibleChanging" handler

// module prop =
//     module position =
//         type x =
//             static member inline absolute (position:int)                                        = Interop.mkprop "view.x" (Pos.Absolute(position))
//             static member inline align (alignment:Alignment, ?modes:AlignmentModes, ?groupId:int)
//                 =
//                     let modes = defaultArg modes AlignmentModes.StartToEnd ||| AlignmentModes.AddSpaceBetweenItems
//                     let groupId = defaultArg groupId 0
//                     Interop.mkprop "view.x" (Pos.Align(alignment, modes, groupId))
//             static member inline anchorEnd                                                      = Interop.mkprop "view.x" (Pos.AnchorEnd())
//             static member inline anchorEndWithOffset (offset:int)                               = Interop.mkprop "view.x" (Pos.AnchorEnd(offset))
//             static member inline center                                                         = Interop.mkprop "view.x" (Pos.Center())
//             static member inline func (f:View -> int)                                           = Interop.mkprop "view.x" (Pos.Func(f))
//             static member inline percent (percent:int)                                          = Interop.mkprop "view.x" (Pos.Percent(percent))
//
//         type y =
//             static member inline absolute (position:int)                                        = Interop.mkprop "view.y" (Pos.Absolute(position))
//             static member inline align (alignment:Alignment, ?modes:AlignmentModes, ?groupId:int)
//                 =
//                     let modes = defaultArg modes AlignmentModes.StartToEnd ||| AlignmentModes.AddSpaceBetweenItems
//                     let groupId = defaultArg groupId 0
//                     Interop.mkprop "view.y" (Pos.Align(alignment, modes, groupId))
//             static member inline anchorEnd                                                      = Interop.mkprop "view.y" (Pos.AnchorEnd())
//             static member inline anchorEndWithOffset (offset:int)                               = Interop.mkprop "view.y" (Pos.AnchorEnd(offset))
//             static member inline center                                                         = Interop.mkprop "view.y" (Pos.Center())
//             static member inline func (f:View -> int)                                           = Interop.mkprop "view.y" (Pos.Func(f))
//             static member inline percent (percent:int)                                          = Interop.mkprop "view.y" (Pos.Percent(percent))
//
//     type width =
//         static member inline absolute (size:int)                                                                    = Interop.mkprop "view.width" (Dim.Absolute(size))
//         static member inline auto (?style:DimAutoStyle, ?minimumContentDim:Dim, ?maximumContentDim:Dim)
//             =
//                 let style = defaultArg style DimAutoStyle.Auto
//                 let minimumContentDim = defaultArg minimumContentDim null
//                 let maximumContentDim = defaultArg maximumContentDim null
//                 Interop.mkprop "view.width" (Dim.Auto(style, minimumContentDim, maximumContentDim))
//         static member inline fill (margin:int)                                                                      = Interop.mkprop "view.width" (Dim.Fill(margin))
//         static member inline func (f:View->int)                                                                     = Interop.mkprop "view.width" (Dim.Func(f))
//         static member inline percent (percent:int, mode:DimPercentMode)                                             = Interop.mkprop "view.width" (Dim.Percent(percent, mode))
//         static member inline percent (percent:int)                                                                  = Interop.mkprop "view.width" (Dim.Percent(percent, DimPercentMode.ContentSize))
//
//     type height =
//         static member inline absolute (size:int)                                                                    = Interop.mkprop "view.height" (Dim.Absolute(size))
//         static member inline auto (?style:DimAutoStyle, ?minimumContentDim:Dim, ?maximumContentDim:Dim)
//             =
//                 let style = defaultArg style DimAutoStyle.Auto
//                 let minimumContentDim = defaultArg minimumContentDim null
//                 let maximumContentDim = defaultArg maximumContentDim null
//                 Interop.mkprop "view.height" (Dim.Auto(style, minimumContentDim, maximumContentDim))
//         static member inline fill (margin:int)                                                                      = Interop.mkprop "view.height" (Dim.Fill(margin))
//         static member inline func (f:View->int)                                                                     = Interop.mkprop "view.height" (Dim.Func(f))
//         static member inline percent (percent:int, mode:DimPercentMode)                                             = Interop.mkprop "view.height" (Dim.Percent(percent, mode))
//         static member inline percent (percent:int)                                                                  = Interop.mkprop "view.height" (Dim.Percent(percent, DimPercentMode.ContentSize))
//
//
//     type alignment =
//         static member inline center     =   Interop.mkprop "view.alignment" Alignment.Center
//         static member inline ``end``    =   Interop.mkprop "view.alignment" Alignment.End
//         static member inline start      =   Interop.mkprop "view.alignment" Alignment.Start
//         static member inline fill       =   Interop.mkprop "view.alignment" Alignment.Fill
//
//     type textDirection =
//         static member inline bottomTop_leftRight = Interop.mkprop "view.textDirection" TextDirection.BottomTop_LeftRight
//         static member inline bottomTop_rightLeft = Interop.mkprop "view.textDirection" TextDirection.BottomTop_RightLeft
//         static member inline leftRight_bottomTop = Interop.mkprop "view.textDirection" TextDirection.LeftRight_BottomTop
//         static member inline leftRight_topBottom = Interop.mkprop "view.textDirection" TextDirection.LeftRight_TopBottom
//         static member inline rightLeft_bottomTop = Interop.mkprop "view.textDirection" TextDirection.RightLeft_BottomTop
//         static member inline rightLeft_topBottom = Interop.mkprop "view.textDirection" TextDirection.RightLeft_TopBottom
//         static member inline topBottom_leftRight = Interop.mkprop "view.textDirection" TextDirection.TopBottom_LeftRight
//
//     type borderStyle =
//         static member inline double = Interop.mkprop    "view.borderStyle" LineStyle.Double
//         static member inline none = Interop.mkprop      "view.borderStyle" LineStyle.None
//         static member inline rounded = Interop.mkprop   "view.borderStyle" LineStyle.Rounded
//         static member inline single = Interop.mkprop    "view.borderStyle" LineStyle.Single
//
//     type shadowStyle =
//         static member inline none = Interop.mkprop          "view.shadowStyle" ShadowStyle.None
//         static member inline opaque = Interop.mkprop        "view.shadowStyle" ShadowStyle.Opaque
//         static member inline transparent = Interop.mkprop   "view.shadowStyle" ShadowStyle.Transparent

// Adornment
type adornment() =
    inherit view()
    // Properties
    member inline this.diagnostics (value:ViewDiagnosticFlags) = this.props.add "adornment.diagnostics" value
    member inline this.superViewRendersLineCanvas (value:bool) = this.props.add "adornment.superViewRendersLineCanvas" value
    member inline this.thickness (value:Thickness) = this.props.add "adornment.thickness" value
    member inline this.viewport (value:Rectangle) = this.props.add "adornment.viewport" value
    // Events
    member inline this.thicknessChanged (handler:unit->unit) = this.props.add "adornment.thicknessChanged" handler

// Bar
type bar() =
    inherit view()
    // Properties
    member inline this.alignmentModes (value:AlignmentModes) = this.props.add "bar.alignmentModes" value
    member inline this.orientation (value:Orientation) = this.props.add "bar.orientation" value
    // Events
    member inline this.orientationChanged (handler:Orientation->unit) = this.props.add "bar.orientationChanged" handler
    member inline this.orientationChanging (handler:App.CancelEventArgs<Orientation>->unit) = this.props.add "bar.orientationChanging" handler

// Border
type border() =
    inherit adornment()
    // Properties
    member inline this.lineStyle (value:LineStyle) = this.props.add "border.lineStyle" value
    member inline this.settings (value:BorderSettings) = this.props.add "border.settings" value

// Button
type button() =
    inherit view()
    // Properties
    member inline this.hotKeySpecifier (value:Rune) = this.props.add "button.hotKeySpecifier" value
    member inline this.isDefault (value:bool) = this.props.add "button.isDefault" value
    member inline this.noDecorations (value:bool) = this.props.add "button.noDecorations" value
    member inline this.noPadding (value:bool) = this.props.add "button.noPadding" value
    member inline this.text (value:string) = this.props.add "button.text" value
    member inline this.wantContinuousButtonPressed (value:bool) = this.props.add "button.wantContinuousButtonPressed" value

// CheckBox
type checkBox() =
    inherit view()
    // Properties
    member inline this.allowCheckStateNone (value:bool) = this.props.add "checkBox.allowCheckStateNone" value
    member inline this.checkedState (value:CheckState) = this.props.add "checkBox.checkedState" value
    member inline this.hotKeySpecifier (value:Rune) = this.props.add "checkBox.hotKeySpecifier" value
    member inline this.radioStyle (value:bool) = this.props.add "checkBox.radioStyle" value
    member inline this.text (value:string) = this.props.add "checkBox.text" value
    // Events
    member inline this.checkedStateChanging (handler:ResultEventArgs<CheckState>->unit) = this.props.add "checkBox.checkedStateChanging" handler

    member inline this.ischecked (value:bool) = this.props.add "checkBox.checkedState" (if value then CheckState.Checked else CheckState.UnChecked)


// ColorPicker
type colorPicker() =
    inherit view()
    // Properties
    member inline this.selectedColor (value:Color) = this.props.add "colorPicker.selectedColor" value
    member inline this.style (value:ColorPickerStyle) = this.props.add "colorPicker.style" value
    // Events
    member inline this.colorChanged (handler:ResultEventArgs<Color>->unit) = this.props.add "colorPicker.colorChanged" handler

// ColorPicker16
type colorPicker16() =
    inherit view()
    // Properties
    member inline this.boxHeight (value:Int32) = this.props.add "colorPicker16.boxHeight" value
    member inline this.boxWidth (value:Int32) = this.props.add "colorPicker16.boxWidth" value
    member inline this.cursor (value:Point) = this.props.add "colorPicker16.cursor" value
    member inline this.selectedColor (value:ColorName16) = this.props.add "colorPicker16.selectedColor" value
    // Events
    member inline this.colorChanged (handler:ResultEventArgs<Color>->unit) = this.props.add "colorPicker16.colorChanged" handler

// ComboBox
type comboBox() =
    inherit view()
    // Properties
    member inline this.hideDropdownListOnClick (value:bool) = this.props.add "comboBox.hideDropdownListOnClick" value
    member inline this.readOnly (value:bool) = this.props.add "comboBox.readOnly" value
    member inline this.searchText (value:string) = this.props.add "comboBox.searchText" value
    member inline this.selectedItem (value:Int32) = this.props.add "comboBox.selectedItem" value
    member inline this.source (value:string list) = this.props.add "comboBox.source" value
    member inline this.text (value:string) = this.props.add "comboBox.text" value
    // Events
    member inline this.collapsed (handler:unit->unit) = this.props.add "comboBox.collapsed" handler
    member inline this.expanded (handler:unit->unit) = this.props.add "comboBox.expanded" handler
    member inline this.openSelectedItem (handler:ListViewItemEventArgs->unit) = this.props.add "comboBox.openSelectedItem" handler
    member inline this.selectedItemChanged (handler:ListViewItemEventArgs->unit) = this.props.add "comboBox.selectedItemChanged" handler

// TextField
type textField() =
    inherit view()
    // Properties
    member inline this.autocomplete (value:IAutocomplete) = this.props.add "textField.autocomplete" value
    member inline this.caption (value:string) = this.props.add "textField.caption" value
    member inline this.captionColor (value:Terminal.Gui.Drawing.Color) = this.props.add "textField.captionColor" value
    member inline this.cursorPosition (value:Int32) = this.props.add "textField.cursorPosition" value
    member inline this.readOnly (value:bool) = this.props.add "textField.readOnly" value
    member inline this.secret (value:bool) = this.props.add "textField.secret" value
    member inline this.selectedStart (value:Int32) = this.props.add "textField.selectedStart" value
    member inline this.selectWordOnlyOnDoubleClick (value:bool) = this.props.add "textField.selectWordOnlyOnDoubleClick" value
    member inline this.text (value:string) = this.props.add "textField.text" value
    member inline this.used (value:bool) = this.props.add "textField.used" value
    member inline this.useSameRuneTypeForWords (value:bool) = this.props.add "textField.useSameRuneTypeForWords" value
    // Events
    member inline this.textChanging (handler:ResultEventArgs<string>->unit) = this.props.add "textField.textChanging" handler

// DateField
type dateField() =
    inherit textField()
    // Properties
    member inline this.culture (value:CultureInfo) = this.props.add "dateField.culture" value
    member inline this.cursorPosition (value:Int32) = this.props.add "dateField.cursorPosition" value
    member inline this.date (value:DateTime) = this.props.add "dateField.date" value
    // Events
    member inline this.dateChanged (handler:DateTimeEventArgs<DateTime>->unit) = this.props.add "dateField.dateChanged" handler

// DatePicker
type datePicker() =
    inherit view()
    // Properties
    member inline this.culture (value:CultureInfo) = this.props.add "datePicker.culture" value
    member inline this.date (value:DateTime) = this.props.add "datePicker.date" value

// Toplevel
type toplevel() =
    inherit view()
    // Properties
    member inline this.modal (value:bool) = this.props.add "toplevel.modal" value
    member inline this.running (value:bool) = this.props.add "toplevel.running" value
    // Events
    member inline this.activate (handler:ToplevelEventArgs->unit) = this.props.add "toplevel.activate" handler
    member inline this.closed (handler:ToplevelEventArgs->unit) = this.props.add "toplevel.closed" handler
    member inline this.closing (handler:ToplevelClosingEventArgs->unit) = this.props.add "toplevel.closing" handler
    member inline this.deactivate (handler:ToplevelEventArgs->unit) = this.props.add "toplevel.deactivate" handler
    member inline this.loaded (handler:unit->unit) = this.props.add "toplevel.loaded" handler
    member inline this.ready (handler:unit->unit) = this.props.add "toplevel.ready" handler
    member inline this.sizeChanging (handler:SizeChangedEventArgs->unit) = this.props.add "toplevel.sizeChanging" handler
    member inline this.unloaded (handler:unit->unit) = this.props.add "toplevel.unloaded" handler

// Dialog
type dialog() =
    inherit toplevel()
    // Properties
    member inline this.buttonAlignment (value:Alignment) = this.props.add "dialog.buttonAlignment" value
    member inline this.buttonAlignmentModes (value:AlignmentModes) = this.props.add "dialog.buttonAlignmentModes" value
    member inline this.canceled (value:bool) = this.props.add "dialog.canceled" value

// FileDialog
type fileDialog() =
    inherit dialog()
    // Properties
    member inline this.allowedTypes (value:IAllowedType list) = this.props.add "fileDialog.allowedTypes" value
    member inline this.allowsMultipleSelection (value:bool) = this.props.add "fileDialog.allowsMultipleSelection" value
    member inline this.fileOperationsHandler (value:IFileOperations) = this.props.add "fileDialog.fileOperationsHandler" value
    member inline this.mustExist (value:bool) = this.props.add "fileDialog.mustExist" value
    member inline this.openMode (value:OpenMode) = this.props.add "fileDialog.openMode" value
    member inline this.path (value:string) = this.props.add "fileDialog.path" value
    member inline this.searchMatcher (value:ISearchMatcher) = this.props.add "fileDialog.searchMatcher" value
    // Events
    member inline this.filesSelected (handler:FilesSelectedEventArgs->unit) = this.props.add "fileDialog.filesSelected" handler

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
    member inline this.axisX (value:HorizontalAxis) = this.props.add "graphView.axisX" value
    member inline this.axisY (value:VerticalAxis) = this.props.add "graphView.axisY" value
    member inline this.cellSize (value:PointF) = this.props.add "graphView.cellSize" value
    member inline this.graphColor (value:Attribute option) = this.props.add "graphView.graphColor" value
    member inline this.marginBottom (value:int) = this.props.add "graphView.marginBottom" value
    member inline this.marginLeft (value:int) = this.props.add "graphView.marginLeft" value
    member inline this.scrollOffset (value:PointF) = this.props.add "graphView.scrollOffset" value

// HexView
type hexView() =
    inherit view()

    // Properties
    member inline this.address (value:Int64) = this.props.add "hexView.address" value
    member inline this.addressWidth (value:int) = this.props.add "hexView.addressWidth" value
    member inline this.allowEdits (value:int) = this.props.add "hexView.allowEdits" value
    member inline this.readOnly (value:bool) = this.props.add "hexView.readOnly" value
    member inline this.source (value:Stream) = this.props.add "hexView.source" value
    // Events
    member inline this.edited (handler:HexViewEditEventArgs->unit) = this.props.add "hexView.edited" handler
    member inline this.positionChanged (handler:HexViewEventArgs->unit) = this.props.add "hexView.positionChanged" handler

// Label
type label() =
    inherit view()

    // Properties
    member inline this.hotKeySpecifier (value:Rune) = this.props.add "label.hotKeySpecifier" value
    member inline this.text (value:string) = this.props.add "label.text" value

// LegendAnnotation
type legendAnnotation() =
    inherit view()
// No properties or events LegendAnnotation

// Line
type line() =
    inherit view()

    // Properties
    member inline this.orientation (value:Orientation) = this.props.add "line.orientation" value
    // Events
    member inline this.orientationChanged (handler:Orientation->unit) = this.props.add "line.orientationChanged" handler
    member inline this.orientationChanging (handler:App.CancelEventArgs<Orientation>->unit) = this.props.add "line.orientationChanging" handler

// LineView
type lineView() =
    inherit view()

    // Properties
    member inline this.endingAnchor (value:Rune option) = this.props.add "lineView.endingAnchor" value
    member inline this.lineRune (value:Rune) = this.props.add "lineView.lineRune" value
    member inline this.orientation (value:Orientation) = this.props.add "lineView.orientation" value
    member inline this.startingAnchor (value:Rune option) = this.props.add "lineView.startingAnchor" value

// ListView
type listView() =
    inherit view()

    // Properties
    member inline this.allowsMarking (value:bool) = this.props.add "listView.allowsMarking" value
    member inline this.allowsMultipleSelection (value:bool) = this.props.add "listView.allowsMultipleSelection" value
    member inline this.leftItem (value:Int32) = this.props.add "listView.leftItem" value
    member inline this.selectedItem (value:Int32) = this.props.add "listView.selectedItem" value
    member inline this.source (value:string list) = this.props.add "listView.source" value
    member inline this.topItem (value:Int32) = this.props.add "listView.topItem" value
    // Events
    member inline this.collectionChanged (handler:NotifyCollectionChangedEventArgs->unit) = this.props.add "listView.collectionChanged" handler
    member inline this.openSelectedItem (handler:ListViewItemEventArgs->unit) = this.props.add "listView.openSelectedItem" handler
    member inline this.rowRender (handler:ListViewRowEventArgs->unit) = this.props.add "listView.rowRender" handler
    member inline this.selectedItemChanged (handler:ListViewItemEventArgs->unit) = this.props.add "listView.selectedItemChanged" handler

// Margin
type margin() =
    inherit adornment()

    // Properties
    member inline this.shadowStyle (value:ShadowStyle) = this.props.add "margin.shadowStyle" value

type menuv2() =
    inherit bar()

    // Properties
    member inline this.selectedMenuItem (value: MenuItemv2 list) = this.props.add "menuv2.selectedMenuItem" value
    member inline this.superMenuItem (value: MenuItemv2 list) = this.props.add "menuv2.superMenuItem" value
    // Events
    member inline this.accepted (value: CommandEventArgs->unit) = this.props.add "menuv2.accepted" value
    member inline this.selectedMenuItemChanged (value: MenuItemv2->unit) = this.props.add "menuv2.selectedMenuItemChanged" value

// MenuBarV2
type menuBarv2() =
    inherit menuv2()

    // Properties
    member inline this.key (value:Key) = this.props.add "menuBarv2.key" value
    member inline this.menus (value:MenuBarItemv2Element list) = this.props.add "children" (value |> List.map (fun v -> v :> TerminalElement))
    // Events
    member inline this.keyChanged (handler:KeyChangedEventArgs->unit) = this.props.add "menuBarv2.keyChanged" handler

type shortcut() =
     inherit view()

     // Properties
     member inline this.action (value:Action) = this.props.add "shortcut.action" value
     member inline this.alignmentModes (value:AlignmentModes) = this.props.add "shortcut.alignmentModes" value
     member inline this.commandView (value:TerminalElement) = this.props.add "shortcut.commandView.element" value
     member inline this.forceFocusColors (value:bool) = this.props.add "shortcut.forceFocusColors" value
     member inline this.helpText (value:string) = this.props.add "shortcut.helpText" value
     member inline this.text (value:string) = this.props.add "shortcut.text" value
     member inline this.bindKeyToApplication (value:bool) = this.props.add "shortcut.bindKeyToApplication" value
     member inline this.key (value:Key) = this.props.add "shortcut.key" value
     member inline this.minimumKeyTextSize (value:Int32) = this.props.add "shortcut.minimumKeyTextSize" value
     // Events
     member inline this.orientationChanged (handler:Orientation->unit) = this.props.add "shortcut.orientationChanged" handler
     member inline this.orientationChanging (handler:CancelEventArgs<Orientation>->unit) = this.props.add "shortcut.orientationChanging" handler

type menuItemv2() =
    inherit shortcut()
    member inline this.command (value: Command) = this.props.add "menuItemv2.command" value
    member inline this.submenu(value: Menuv2Element) = this.props.add "menuItemv2.subMenu.element" value
    member inline this.accepted(value: CommandEventArgs -> unit) = this.props.add "menuItemv2.accepted" value

type menuBarItemv2() =
    inherit menuItemv2()
    member inline this.popoverMenu (value:PopoverMenuElement) = this.props.add "menuBarItemv2.popoverMenu.element" value
    member inline this.popoverMenuOpen (value:bool) = this.props.add "menuBarItemv2.popoverMenuOpen" value

type popoverMenu() =
    inherit view()

    member inline this.key (value:Key) = this.props.add "popoverMenu.key" value
    member inline this.root (value: Menuv2Element) = this.props.add "popoverMenu.root.element" value


// NumericUpDown`1
type numericUpDown<'a>() =
    inherit view()

    // Properties
    member inline this.format (value:string) = this.props.add "numericUpDown`1.format" value
    member inline this.increment (value:'a) = this.props.add "numericUpDown`1.increment" value
    member inline this.value (value:'a) = this.props.add "numericUpDown`1.value" value
    // Events
    member inline this.formatChanged (handler:string->unit) = this.props.add "numericUpDown`1.formatChanged" handler
    member inline this.incrementChanged (handler:'a->unit) = this.props.add "numericUpDown`1.incrementChanged" handler
    member inline this.valueChanged (handler:'a->unit) = this.props.add "numericUpDown`1.value |> this.props.AddChanged" handler
    member inline this.valueChanging (handler:App.CancelEventArgs<'a>->unit) = this.props.add "numericUpDown`1.value |> this.props.AddChanging" handler

// NumericUpDown
type numericUpDown() =
    inherit numericUpDown<int>()
// No properties or events NumericUpDown

// OpenDialog
type openDialog() =
    inherit fileDialog()
    // Properties
    member inline this.openMode (value:OpenMode) = this.props.add "openDialog.openMode" value

// Padding
type padding() =
    inherit adornment()

// ProgressBar
type progressBar() =
    inherit view()

    // Properties
    member inline this.bidirectionalMarquee (value:bool) = this.props.add "progressBar.bidirectionalMarquee" value
    member inline this.fraction (value:Single) = this.props.add "progressBar.fraction" value
    member inline this.progressBarFormat (value:ProgressBarFormat) = this.props.add "progressBar.progressBarFormat" value
    member inline this.progressBarStyle (value:ProgressBarStyle) = this.props.add "progressBar.progressBarStyle" value
    member inline this.segmentCharacter (value:Rune) = this.props.add "progressBar.segmentCharacter" value
    member inline this.text (value:string) = this.props.add "progressBar.text" value

// RadioGroup
type radioGroup() =
    inherit view()

    // Properties
    member inline this.assignHotKeysToRadioLabels (value:bool) = this.props.add "radioGroup.assignHotKeysToRadioLabels" value
    member inline this.cursor (value:Int32) = this.props.add "radioGroup.cursor" value
    member inline this.doubleClickAccepts (value:bool) = this.props.add "radioGroup.doubleClickAccepts" value
    member inline this.horizontalSpace (value:Int32) = this.props.add "radioGroup.horizontalSpace" value
    member inline this.orientation (value:Orientation) = this.props.add "radioGroup.orientation" value
    member inline this.radioLabels (value:string list) = this.props.add "radioGroup.radioLabels" value
    member inline this.selectedItem (value:Int32) = this.props.add "radioGroup.selectedItem" value
    // Events
    member inline this.orientationChanged (handler:Orientation->unit) = this.props.add "radioGroup.orientationChanged" handler
    member inline this.orientationChanging (handler:App.CancelEventArgs<Orientation>->unit) = this.props.add "radioGroup.orientationChanging" handler
    member inline this.selectedItemChanged (handler:SelectedItemChangedArgs->unit) = this.props.add "radioGroup.selectedItemChanged" handler

// SaveDialog
// No properties or events SaveDialog

// ScrollBar
type scrollBar() =
    inherit view()

    // Properties
    member inline this.autoShow (value:bool) = this.props.add "scrollBar.autoShow" value
    member inline this.increment (value:Int32) = this.props.add "scrollBar.increment" value
    member inline this.orientation (value:Orientation) = this.props.add "scrollBar.orientation" value
    member inline this.position (value:Int32) = this.props.add "scrollBar.position" value
    member inline this.scrollableContentSize (value:Int32) = this.props.add "scrollBar.scrollableContentSize" value
    member inline this.visibleContentSize (value:Int32) = this.props.add "scrollBar.visibleContentSize" value
    // Events
    member inline this.orientationChanged (handler:Orientation->unit) = this.props.add "scrollBar.orientationChanged" handler
    member inline this.orientationChanging (handler:CancelEventArgs<Orientation>->unit) = this.props.add "scrollBar.orientationChanging" handler
    member inline this.scrollableContentSizeChanged (handler:EventArgs<Int32>->unit) = this.props.add "scrollBar.scrollableContentSizeChanged" handler
    member inline this.sliderPositionChanged (handler:EventArgs<Int32>->unit) = this.props.add "scrollBar.sliderPositionChanged" handler

// ScrollSlider
type scrollSlider() =
    inherit view()

    // Properties
    member inline this.orientation (value:Orientation) = this.props.add "scrollSlider.orientation" value
    member inline this.position (value:Int32) = this.props.add "scrollSlider.position" value
    member inline this.size (value:Int32) = this.props.add "scrollSlider.size" value
    member inline this.sliderPadding (value:Int32) = this.props.add "scrollSlider.sliderPadding" value
    member inline this.visibleContentSize (value:Int32) = this.props.add "scrollSlider.visibleContentSize" value
    // Events
    member inline this.orientationChanged (handler:Orientation->unit) = this.props.add "scrollSlider.orientationChanged" handler
    member inline this.orientationChanging (handler:CancelEventArgs<Orientation>->unit) = this.props.add "scrollSlider.orientationChanging" handler
    member inline this.positionChanged (handler:EventArgs<Int32>->unit) = this.props.add "scrollSlider.positionChanged" handler
    member inline this.positionChanging (handler:CancelEventArgs<Int32>->unit) = this.props.add "scrollSlider.positionChanging" handler
    member inline this.scrolled (handler:EventArgs<Int32>->unit) = this.props.add "scrollSlider.scrolled" handler

// Slider`1
type slider<'a>() =
    inherit view()

    // Properties
    member inline this.allowEmpty (value:bool) = this.props.add "slider`1.allowEmpty" value
    member inline this.focusedOption (value:Int32) = this.props.add "slider`1.focusedOption" value
    member inline this.legendsOrientation (value:Orientation) = this.props.add "slider`1.legendsOrientation" value
    member inline this.minimumInnerSpacing (value:Int32) = this.props.add "slider`1.minimumInnerSpacing" value
    member inline this.options (value:SliderOption<'a> list) = this.props.add "slider`1.options" value
    member inline this.orientation (value:Orientation) = this.props.add "slider`1.orientation" value
    member inline this.rangeAllowSingle (value:bool) = this.props.add "slider`1.rangeAllowSingle" value
    member inline this.showEndSpacing (value:bool) = this.props.add "slider`1.showEndSpacing" value
    member inline this.showLegends (value:bool) = this.props.add "slider`1.showLegends" value
    member inline this.style (value:SliderStyle) = this.props.add "slider`1.style" value
    member inline this.text (value:string) = this.props.add "slider`1.text" value
    member inline this.``type`` (value:SliderType) = this.props.add "slider`1.``type``" value
    member inline this.useMinimumSize (value:bool) = this.props.add "slider`1.useMinimumSize" value
    // Events
    member inline this.optionFocused (handler:SliderEventArgs<'a>->unit) = this.props.add "slider`1.optionFocused" handler
    member inline this.optionsChanged (handler:SliderEventArgs<'a>->unit) = this.props.add "slider`1.optionsChanged" handler
    member inline this.orientationChanged (handler:Orientation->unit) = this.props.add "slider`1.orientationChanged" handler
    member inline this.orientationChanging (handler:App.CancelEventArgs<Orientation>->unit) = this.props.add "slider`1.orientationChanging" handler

// Slider
type slider() =
    inherit slider<obj>()
// No properties or events Slider

// SpinnerView
type spinnerView() =
    inherit view()

    // Properties
    member inline this.autoSpin (value:bool) = this.props.add "spinnerView.autoSpin" value
    member inline this.sequence (value:string list) = this.props.add "spinnerView.sequence" value
    member inline this.spinBounce (value:bool) = this.props.add "spinnerView.spinBounce" value
    member inline this.spinDelay (value:Int32) = this.props.add "spinnerView.spinDelay" value
    member inline this.spinReverse (value:bool) = this.props.add "spinnerView.spinReverse" value
    member inline this.style (value:SpinnerStyle) = this.props.add "spinnerView.style" value

// StatusBar
type statusBar() =
    inherit bar()
// No properties or events StatusBar

// Tab
type tab() =
    inherit view()

    // Properties
    member inline this.displayText (value:string) = this.props.add "tab.displayText" value
    member inline this.view (value:TerminalElement) = this.props.add "tab.view" value

// TabView
type tabView() =
    inherit view()

    // Properties
    member inline this.maxTabTextWidth (value:int) = this.props.add "tabView.maxTabTextWidth" value
    member inline this.selectedTab (value:Tab) = this.props.add "tabView.selectedTab" value
    member inline this.style (value:TabStyle) = this.props.add "tabView.style" value
    member inline this.tabScrollOffset (value:Int32) = this.props.add "tabView.tabScrollOffset" value
    // Events
    member inline this.selectedTabChanged (handler:TabChangedEventArgs->unit) = this.props.add "tabView.selectedTabChanged" handler
    member inline this.tabClicked (handler:TabMouseEventArgs->unit) = this.props.add "tabView.tabClicked" handler

    member inline this.tabs (value:TerminalElement list) = this.props.add "tabView.tabs" value


// TableView
type tableView() =
    inherit view()

    // Properties
    member inline this.cellActivationKey (value:KeyCode) = this.props.add "tableView.cellActivationKey" value
    member inline this.collectionNavigator (value:ICollectionNavigator) = this.props.add "tableView.collectionNavigator" value
    member inline this.columnOffset (value:Int32) = this.props.add "tableView.columnOffset" value
    member inline this.fullRowSelect (value:bool) = this.props.add "tableView.fullRowSelect" value
    member inline this.maxCellWidth (value:Int32) = this.props.add "tableView.maxCellWidth" value
    member inline this.minCellWidth (value:Int32) = this.props.add "tableView.minCellWidth" value
    member inline this.multiSelect (value:bool) = this.props.add "tableView.multiSelect" value
    member inline this.nullSymbol (value:string) = this.props.add "tableView.nullSymbol" value
    member inline this.rowOffset (value:Int32) = this.props.add "tableView.rowOffset" value
    member inline this.selectedColumn (value:Int32) = this.props.add "tableView.selectedColumn" value
    member inline this.selectedRow (value:Int32) = this.props.add "tableView.selectedRow" value
    member inline this.separatorSymbol (value:Char) = this.props.add "tableView.separatorSymbol" value
    member inline this.style (value:TableStyle) = this.props.add "tableView.style" value
    member inline this.table (value:ITableSource) = this.props.add "tableView.table" value
    // Events
    member inline this.cellActivated (handler:CellActivatedEventArgs->unit) = this.props.add "tableView.cellActivated" handler
    member inline this.cellToggled (handler:CellToggledEventArgs->unit) = this.props.add "tableView.cellToggled" handler
    member inline this.selectedCellChanged (handler:SelectedCellChangedEventArgs->unit) = this.props.add "tableView.selectedCellChanged" handler


// TextValidateField
type textValidateField() =
    inherit view()

    // Properties
    member inline this.provider (value:ITextValidateProvider) = this.props.add "textValidateField.provider" value
    member inline this.text (value:string) = this.props.add "textValidateField.text" value

// TextView
type textView() =
    inherit view()

    // Properties
    member inline this.allowsReturn (value:bool) = this.props.add "textView.allowsReturn" value
    member inline this.allowsTab (value:bool) = this.props.add "textView.allowsTab" value
    member inline this.cursorPosition (value:Point) = this.props.add "textView.cursorPosition" value
    member inline this.inheritsPreviousAttribute (value:bool) = this.props.add "textView.inheritsPreviousAttribute" value
    member inline this.isDirty (value:bool) = this.props.add "textView.isDirty" value
    member inline this.isSelecting (value:bool) = this.props.add "textView.isSelecting" value
    member inline this.leftColumn (value:Int32) = this.props.add "textView.leftColumn" value
    member inline this.multiline (value:bool) = this.props.add "textView.multiline" value
    member inline this.readOnly (value:bool) = this.props.add "textView.readOnly" value
    member inline this.selectionStartColumn (value:Int32) = this.props.add "textView.selectionStartColumn" value
    member inline this.selectionStartRow (value:Int32) = this.props.add "textView.selectionStartRow" value
    member inline this.selectWordOnlyOnDoubleClick (value:bool) = this.props.add "textView.selectWordOnlyOnDoubleClick" value
    member inline this.tabWidth (value:Int32) = this.props.add "textView.tabWidth" value
    member inline this.text (value:string) = this.props.add "textView.text" value
    member inline this.topRow (value:Int32) = this.props.add "textView.topRow" value
    member inline this.used (value:bool) = this.props.add "textView.used" value
    member inline this.useSameRuneTypeForWords (value:bool) = this.props.add "textView.useSameRuneTypeForWords" value
    member inline this.wordWrap (value:bool) = this.props.add "textView.wordWrap" value
    // Events
    member inline this.contentsChanged (handler:ContentsChangedEventArgs->unit) = this.props.add "textView.contentsChanged" handler
    member inline this.drawNormalColor (handler:CellEventArgs->unit) = this.props.add "textView.drawNormalColor" handler
    member inline this.drawReadOnlyColor (handler:CellEventArgs->unit) = this.props.add "textView.drawReadOnlyColor" handler
    member inline this.drawSelectionColor (handler:CellEventArgs->unit) = this.props.add "textView.drawSelectionColor" handler
    member inline this.drawUsedColor (handler:CellEventArgs->unit) = this.props.add "textView.drawUsedColor" handler
    member inline this.unwrappedCursorPosition (handler:Point->unit) = this.props.add "textView.unwrappedCursorPosition" handler
    // Additional properties
    member inline this.textChanged (value:string->unit) = this.props.add "textView.textChanged" value

// TileView
type tileView() =
    inherit view()

    // Properties
    member inline this.lineStyle (value:LineStyle) = this.props.add "tileView.lineStyle" value
    member inline this.orientation (value:Orientation) = this.props.add "tileView.orientation" value
    member inline this.toggleResizable (value:KeyCode) = this.props.add "tileView.toggleResizable" value
    // Events
    member inline this.splitterMoved (handler:SplitterEventArgs->unit) = this.props.add "tileView.splitterMoved" handler

// TimeField
type timeField() =
    inherit textField()

    // Properties
    member inline this.cursorPosition (value:Int32) = this.props.add "timeField.cursorPosition" value
    member inline this.isShortFormat (value:bool) = this.props.add "timeField.isShortFormat" value
    member inline this.time (value:TimeSpan) = this.props.add "timeField.time" value
    // Events
    member inline this.timeChanged (handler:DateTimeEventArgs<TimeSpan>->unit) = this.props.add "timeField.timeChanged" handler

// TreeView`1
type treeView<'a when 'a : not struct>() =
    inherit view()

    // Properties
    member inline this.allowLetterBasedNavigation (value:bool) = this.props.add "treeView`1.allowLetterBasedNavigation" value
    member inline this.aspectGetter<'a when 'a : not struct> (value:AspectGetterDelegate<'a>) = this.props.add "treeView`1.aspectGetter" value
    member inline this.colorGetter<'a when 'a : not struct> (value:Func<'a,Scheme>) = this.props.add "treeView`1.colorGetter" value
    member inline this.maxDepth (value:Int32) = this.props.add "treeView`1.maxDepth" value
    member inline this.multiSelect (value:bool) = this.props.add "treeView`1.multiSelect" value
    member inline this.objectActivationButton (value:MouseFlags option) = this.props.add "treeView`1.objectActivationButton" value
    member inline this.objectActivationKey (value:KeyCode) = this.props.add "treeView`1.objectActivationKey" value
    member inline this.scrollOffsetHorizontal (value:Int32) = this.props.add "treeView`1.scrollOffsetHorizontal" value
    member inline this.scrollOffsetVertical (value:Int32) = this.props.add "treeView`1.scrollOffsetVertical" value
    member inline this.selectedObject<'a when 'a : not struct> (value:'a) = this.props.add "treeView`1.selectedObject" value
    member inline this.style (value:TreeStyle) = this.props.add "treeView`1.style" value
    member inline this.treeBuilder<'a when 'a : not struct> (value:ITreeBuilder<'a>) = this.props.add "treeView`1.treeBuilder" value
    // Events
    member inline this.drawLine<'a when 'a : not struct> (handler:DrawTreeViewLineEventArgs<'a>->unit) = this.props.add "treeView`1.drawLine" handler
    member inline this.objectActivated<'a when 'a : not struct> (handler:ObjectActivatedEventArgs<'a>->unit) = this.props.add "treeView`1.objectActivated" handler
    member inline this.selectionChanged<'a when 'a : not struct> (handler:SelectionChangedEventArgs<'a>->unit) = this.props.add "treeView`1.selectionChanged" handler

// TreeView
type treeView() =
    inherit treeView<ITreeNode>()
// No properties or events TreeView

// Window
type window() =
    inherit toplevel()
// No properties or events Window

// Wizard
type wizard() =
    inherit dialog()

    // Properties
    member inline this.currentStep (value:WizardStep) = this.props.add "wizard.currentStep" value
    member inline this.modal (value:bool) = this.props.add "wizard.modal" value
    // Events
    member inline this.cancelled (handler:WizardButtonEventArgs->unit) = this.props.add "wizard.cancelled" handler
    member inline this.finished (handler:WizardButtonEventArgs->unit) = this.props.add "wizard.finished" handler
    member inline this.movingBack (handler:WizardButtonEventArgs->unit) = this.props.add "wizard.movingBack" handler
    member inline this.movingNext (handler:WizardButtonEventArgs->unit) = this.props.add "wizard.movingNext" handler
    member inline this.stepChanged (handler:StepChangeEventArgs->unit) = this.props.add "wizard.stepChanged" handler
    member inline this.stepChanging (handler:StepChangeEventArgs->unit) = this.props.add "wizard.stepChanging" handler

// WizardStep
type wizardStep() =
    inherit view()

    // Properties
    member inline this.backButtonText (value:string) = this.props.add "wizardStep.backButtonText" value
    member inline this.helpText (value:string) = this.props.add "wizardStep.helpText" value
    member inline this.nextButtonText (value:string) = this.props.add "wizardStep.nextButtonText" value
