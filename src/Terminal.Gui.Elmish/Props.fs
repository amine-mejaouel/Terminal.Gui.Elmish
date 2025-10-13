
(*
#######################################
#            Props.fs              #
#######################################
*)

namespace Terminal.Gui.Elmish

open System
open System.Collections.Generic
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
type viewProps() =
    member val props = IncrementalProps()

    member inline this.children (children:TerminalElement list) = this.props.add PName.view.children (List<_>(children))
    member inline this.ref (reference:View->unit) = this.props.add PName.view.ref reference

    // Properties
    member inline this.arrangement (value:ViewArrangement) = this.props.add PName.view.arrangement value
    member inline this.borderStyle (value:LineStyle) = this.props.add PName.view.borderStyle value
    member inline this.canFocus (value:bool) = this.props.add PName.view.canFocus value
    member inline this.contentSizeTracksViewport (value:bool) = this.props.add PName.view.contentSizeTracksViewport value
    member inline this.cursorVisibility (value:CursorVisibility) = this.props.add PName.view.cursorVisibility value
    member inline this.data (value:Object) = this.props.add PName.view.data value
    member inline this.enabled (value:bool) = this.props.add PName.view.enabled value
    member inline this.frame (value:Rectangle) = this.props.add PName.view.frame value
    member inline this.hasFocus (value:bool) = this.props.add PName.view.hasFocus value
    member inline this.height (value:Dim) = this.props.add PName.view.height value
    member inline this.highlightStates (value:MouseState) = this.props.add PName.view.highlightStates value
    member inline this.hotKey (value:Key) = this.props.add PName.view.hotKey value
    member inline this.hotKeySpecifier (value:Rune) = this.props.add PName.view.hotKeySpecifier value
    member inline this.id (value:string) = this.props.add PName.view.id value
    member inline this.isInitialized (value:bool) = this.props.add PName.view.isInitialized value
    member inline this.mouseHeldDown (value:IMouseHeldDown) = this.props.add PName.view.mouseHeldDown value
    member inline this.needsDraw (value:bool) = this.props.add PName.view.needsDraw value
    member inline this.preserveTrailingSpaces (value:bool) = this.props.add PName.view.preserveTrailingSpaces value
    member inline this.schemeName (value:string) = this.props.add PName.view.schemeName value
    member inline this.shadowStyle (value:ShadowStyle) = this.props.add PName.view.shadowStyle value
    member inline this.superViewRendersLineCanvas (value:bool) = this.props.add PName.view.superViewRendersLineCanvas value
    member inline this.tabStop (value:TabBehavior option) = this.props.add PName.view.tabStop value
    member inline this.text (value:string) = this.props.add PName.view.text value
    member inline this.textAlignment (value:Alignment) = this.props.add PName.view.textAlignment value
    member inline this.textDirection (value:TextDirection) = this.props.add PName.view.textDirection value
    member inline this.title (value:string) = this.props.add PName.view.title value
    member inline this.validatePosDim (value:bool) = this.props.add PName.view.validatePosDim value
    member inline this.verticalTextAlignment (value:Alignment) = this.props.add PName.view.verticalTextAlignment value
    member inline this.viewport (value:Rectangle) = this.props.add PName.view.viewport value
    member inline this.viewportSettings (value:ViewportSettingsFlags) = this.props.add PName.view.viewportSettings value
    member inline this.visible (value:bool) = this.props.add PName.view.visible value
    member inline this.wantContinuousButtonPressed (value:bool) = this.props.add PName.view.wantContinuousButtonPressed value
    member inline this.wantMousePositionReports (value:bool) = this.props.add PName.view.wantMousePositionReports value
    member inline this.width (value:Dim) = this.props.add PName.view.width value
    member inline this.x (value:Pos) = this.props.add PName.view.x value
    member inline this.y (value:Pos) = this.props.add PName.view.y value
    // Events
    member inline this.accepting (handler:HandledEventArgs->unit) = this.props.add PName.view.accepting handler
    member inline this.advancingFocus (handler:AdvanceFocusEventArgs->unit) = this.props.add PName.view.advancingFocus handler
    member inline this.borderStyleChanged (handler:EventArgs->unit) = this.props.add PName.view.borderStyleChanged handler
    member inline this.canFocusChanged (handler:unit->unit) = this.props.add PName.view.canFocusChanged handler
    member inline this.clearedViewport (handler:DrawEventArgs->unit) = this.props.add PName.view.clearedViewport handler
    member inline this.clearingViewport (handler:DrawEventArgs->unit) = this.props.add PName.view.clearingViewport handler
    member inline this.commandNotBound (handler:CommandEventArgs->unit) = this.props.add PName.view.commandNotBound handler
    member inline this.contentSizeChanged (handler:SizeChangedEventArgs->unit) = this.props.add PName.view.contentSizeChanged handler
    member inline this.disposing (handler:unit->unit) = this.props.add PName.view.disposing handler
    member inline this.drawComplete (handler:DrawEventArgs->unit) = this.props.add PName.view.drawComplete handler
    member inline this.drawingContent (handler:DrawEventArgs->unit) = this.props.add PName.view.drawingContent handler
    member inline this.drawingSubViews (handler:DrawEventArgs->unit) = this.props.add PName.view.drawingSubViews handler
    member inline this.drawingText (handler:DrawEventArgs->unit) = this.props.add PName.view.drawingText handler
    member inline this.enabledChanged (handler:unit->unit) = this.props.add PName.view.enabledChanged handler
    member inline this.focusedChanged (handler:HasFocusEventArgs->unit) = this.props.add PName.view.focusedChanged handler
    member inline this.frameChanged (handler:EventArgs<Rectangle>->unit) = this.props.add PName.view.frameChanged handler
    member inline this.gettingAttributeForRole (handler:VisualRoleEventArgs->unit) = this.props.add PName.view.gettingAttributeForRole handler
    member inline this.gettingScheme (handler:ResultEventArgs<Scheme>->unit) = this.props.add PName.view.gettingScheme handler
    member inline this.handlingHotKey (handler:CommandEventArgs->unit) = this.props.add PName.view.handlingHotKey handler
    member inline this.hasFocusChanged (handler:HasFocusEventArgs->unit) = this.props.add PName.view.hasFocusChanged handler
    member inline this.hasFocusChanging (handler:HasFocusEventArgs->unit) = this.props.add PName.view.hasFocusChanging handler
    member inline this.hotKeyChanged (handler:KeyChangedEventArgs->unit) = this.props.add PName.view.hotKeyChanged handler
    member inline this.initialized (handler:unit->unit) = this.props.add PName.view.initialized handler
    member inline this.keyDown (handler:Key->unit) = this.props.add PName.view.keyDown handler
    member inline this.keyDownNotHandled (handler:Key->unit) = this.props.add PName.view.keyDownNotHandled handler
    member inline this.keyUp (handler:Key->unit) = this.props.add PName.view.keyUp handler
    member inline this.mouseClick (handler:MouseEventArgs->unit) = this.props.add PName.view.mouseClick handler
    member inline this.mouseEnter (handler:CancelEventArgs->unit) = this.props.add PName.view.mouseEnter handler
    member inline this.mouseEvent (handler:MouseEventArgs->unit) = this.props.add PName.view.mouseEvent handler
    member inline this.mouseLeave (handler:EventArgs->unit) = this.props.add PName.view.mouseLeave handler
    member inline this.mouseStateChanged (handler:EventArgs->unit) = this.props.add PName.view.mouseStateChanged handler
    member inline this.mouseWheel (handler:MouseEventArgs->unit) = this.props.add PName.view.mouseWheel handler
    member inline this.removed (handler:SuperViewChangedEventArgs->unit) = this.props.add PName.view.removed handler
    member inline this.schemeChanged (handler:ValueChangedEventArgs<Scheme>->unit) = this.props.add PName.view.schemeChanged handler
    member inline this.schemeChanging (handler:ValueChangingEventArgs<Scheme>->unit) = this.props.add PName.view.schemeChanging handler
    member inline this.schemeNameChanged (handler:ValueChangedEventArgs<string>->unit) = this.props.add PName.view.schemeNameChanged handler
    member inline this.schemeNameChanging (handler:ValueChangingEventArgs<string>->unit) = this.props.add PName.view.schemeNameChanging handler
    member inline this.selecting (handler:CommandEventArgs->unit) = this.props.add PName.view.selecting handler
    member inline this.subViewAdded (handler:SuperViewChangedEventArgs->unit) = this.props.add PName.view.subViewAdded handler
    member inline this.subViewLayout (handler:LayoutEventArgs->unit) = this.props.add PName.view.subViewLayout handler
    member inline this.subViewRemoved (handler:SuperViewChangedEventArgs->unit) = this.props.add PName.view.subViewRemoved handler
    member inline this.subViewsLaidOut (handler:LayoutEventArgs->unit) = this.props.add PName.view.subViewsLaidOut handler
    member inline this.superViewChanged (handler:SuperViewChangedEventArgs->unit) = this.props.add PName.view.superViewChanged handler
    member inline this.textChanged (handler:unit->unit) = this.props.add PName.view.textChanged handler
    member inline this.titleChanged (handler:string->unit) = this.props.add PName.view.titleChanged handler
    member inline this.titleChanging (handler:App.CancelEventArgs<string>->unit) = this.props.add PName.view.titleChanging handler
    member inline this.viewportChanged (handler:DrawEventArgs->unit) = this.props.add PName.view.viewportChanged handler
    member inline this.visibleChanged (handler:unit->unit) = this.props.add PName.view.visibleChanged handler
    member inline this.visibleChanging (handler:unit->unit) = this.props.add PName.view.visibleChanging handler

// module prop =
//     module position =
//         type x =
//             static member inline absolute (position:int)                                        = Interop.mkprop PName.view.x (Pos.Absolute(position))
//             static member inline align (alignment:Alignment, ?modes:AlignmentModes, ?groupId:int)
//                 =
//                     let modes = defaultArg modes AlignmentModes.StartToEnd ||| AlignmentModes.AddSpaceBetweenItems
//                     let groupId = defaultArg groupId 0
//                     Interop.mkprop PName.view.x (Pos.Align(alignment, modes, groupId))
//             static member inline anchorEnd                                                      = Interop.mkprop PName.view.x (Pos.AnchorEnd())
//             static member inline anchorEndWithOffset (offset:int)                               = Interop.mkprop PName.view.x (Pos.AnchorEnd(offset))
//             static member inline center                                                         = Interop.mkprop PName.view.x (Pos.Center())
//             static member inline func (f:View -> int)                                           = Interop.mkprop PName.view.x (Pos.Func(f))
//             static member inline percent (percent:int)                                          = Interop.mkprop PName.view.x (Pos.Percent(percent))
//
//         type y =
//             static member inline absolute (position:int)                                        = Interop.mkprop PName.view.y (Pos.Absolute(position))
//             static member inline align (alignment:Alignment, ?modes:AlignmentModes, ?groupId:int)
//                 =
//                     let modes = defaultArg modes AlignmentModes.StartToEnd ||| AlignmentModes.AddSpaceBetweenItems
//                     let groupId = defaultArg groupId 0
//                     Interop.mkprop PName.view.y (Pos.Align(alignment, modes, groupId))
//             static member inline anchorEnd                                                      = Interop.mkprop PName.view.y (Pos.AnchorEnd())
//             static member inline anchorEndWithOffset (offset:int)                               = Interop.mkprop PName.view.y (Pos.AnchorEnd(offset))
//             static member inline center                                                         = Interop.mkprop PName.view.y (Pos.Center())
//             static member inline func (f:View -> int)                                           = Interop.mkprop PName.view.y (Pos.Func(f))
//             static member inline percent (percent:int)                                          = Interop.mkprop PName.view.y (Pos.Percent(percent))
//
//     type width =
//         static member inline absolute (size:int)                                                                    = Interop.mkprop PName.view.width (Dim.Absolute(size))
//         static member inline auto (?style:DimAutoStyle, ?minimumContentDim:Dim, ?maximumContentDim:Dim)
//             =
//                 let style = defaultArg style DimAutoStyle.Auto
//                 let minimumContentDim = defaultArg minimumContentDim null
//                 let maximumContentDim = defaultArg maximumContentDim null
//                 Interop.mkprop PName.view.width (Dim.Auto(style, minimumContentDim, maximumContentDim))
//         static member inline fill (margin:int)                                                                      = Interop.mkprop PName.view.width (Dim.Fill(margin))
//         static member inline func (f:View->int)                                                                     = Interop.mkprop PName.view.width (Dim.Func(f))
//         static member inline percent (percent:int, mode:DimPercentMode)                                             = Interop.mkprop PName.view.width (Dim.Percent(percent, mode))
//         static member inline percent (percent:int)                                                                  = Interop.mkprop PName.view.width (Dim.Percent(percent, DimPercentMode.ContentSize))
//
//     type height =
//         static member inline absolute (size:int)                                                                    = Interop.mkprop PName.view.height (Dim.Absolute(size))
//         static member inline auto (?style:DimAutoStyle, ?minimumContentDim:Dim, ?maximumContentDim:Dim)
//             =
//                 let style = defaultArg style DimAutoStyle.Auto
//                 let minimumContentDim = defaultArg minimumContentDim null
//                 let maximumContentDim = defaultArg maximumContentDim null
//                 Interop.mkprop PName.view.height (Dim.Auto(style, minimumContentDim, maximumContentDim))
//         static member inline fill (margin:int)                                                                      = Interop.mkprop PName.view.height (Dim.Fill(margin))
//         static member inline func (f:View->int)                                                                     = Interop.mkprop PName.view.height (Dim.Func(f))
//         static member inline percent (percent:int, mode:DimPercentMode)                                             = Interop.mkprop PName.view.height (Dim.Percent(percent, mode))
//         static member inline percent (percent:int)                                                                  = Interop.mkprop PName.view.height (Dim.Percent(percent, DimPercentMode.ContentSize))
//
//
//     type alignment =
//         static member inline center     =   Interop.mkprop PName.view.alignment Alignment.Center
//         static member inline ``end``    =   Interop.mkprop PName.view.alignment Alignment.End
//         static member inline start      =   Interop.mkprop PName.view.alignment Alignment.Start
//         static member inline fill       =   Interop.mkprop PName.view.alignment Alignment.Fill
//
//     type textDirection =
//         static member inline bottomTop_leftRight = Interop.mkprop PName.view.textDirection TextDirection.BottomTop_LeftRight
//         static member inline bottomTop_rightLeft = Interop.mkprop PName.view.textDirection TextDirection.BottomTop_RightLeft
//         static member inline leftRight_bottomTop = Interop.mkprop PName.view.textDirection TextDirection.LeftRight_BottomTop
//         static member inline leftRight_topBottom = Interop.mkprop PName.view.textDirection TextDirection.LeftRight_TopBottom
//         static member inline rightLeft_bottomTop = Interop.mkprop PName.view.textDirection TextDirection.RightLeft_BottomTop
//         static member inline rightLeft_topBottom = Interop.mkprop PName.view.textDirection TextDirection.RightLeft_TopBottom
//         static member inline topBottom_leftRight = Interop.mkprop PName.view.textDirection TextDirection.TopBottom_LeftRight
//
//     type borderStyle =
//         static member inline double = Interop.mkprop    PName.view.borderStyle LineStyle.Double
//         static member inline none = Interop.mkprop      PName.view.borderStyle LineStyle.None
//         static member inline rounded = Interop.mkprop   PName.view.borderStyle LineStyle.Rounded
//         static member inline single = Interop.mkprop    PName.view.borderStyle LineStyle.Single
//
//     type shadowStyle =
//         static member inline none = Interop.mkprop          PName.view.shadowStyle ShadowStyle.None
//         static member inline opaque = Interop.mkprop        PName.view.shadowStyle ShadowStyle.Opaque
//         static member inline transparent = Interop.mkprop   PName.view.shadowStyle ShadowStyle.Transparent

// Adornment
type adornmentProps() =
    inherit viewProps()
    // Properties
    member inline this.diagnostics (value:ViewDiagnosticFlags) = this.props.add PName.adornment.diagnostics value
    member inline this.superViewRendersLineCanvas (value:bool) = this.props.add PName.adornment.superViewRendersLineCanvas value
    member inline this.thickness (value:Thickness) = this.props.add PName.adornment.thickness value
    member inline this.viewport (value:Rectangle) = this.props.add PName.adornment.viewport value
    // Events
    member inline this.thicknessChanged (handler:unit->unit) = this.props.add PName.adornment.thicknessChanged handler

// Bar
type barProps() =
    inherit viewProps()
    // Properties
    member inline this.alignmentModes (value:AlignmentModes) = this.props.add PName.bar.alignmentModes value
    member inline this.orientation (value:Orientation) = this.props.add PName.bar.orientation value
    // Events
    member inline this.orientationChanged (handler:Orientation->unit) = this.props.add PName.bar.orientationChanged handler
    member inline this.orientationChanging (handler:App.CancelEventArgs<Orientation>->unit) = this.props.add PName.bar.orientationChanging handler

// Border
type borderProps() =
    inherit adornmentProps()
    // Properties
    member inline this.lineStyle (value:LineStyle) = this.props.add PName.border.lineStyle value
    member inline this.settings (value:BorderSettings) = this.props.add PName.border.settings value

// Button
type buttonProps() =
    inherit viewProps()
    // Properties
    member inline this.hotKeySpecifier (value:Rune) = this.props.add PName.button.hotKeySpecifier value
    member inline this.isDefault (value:bool) = this.props.add PName.button.isDefault value
    member inline this.noDecorations (value:bool) = this.props.add PName.button.noDecorations value
    member inline this.noPadding (value:bool) = this.props.add PName.button.noPadding value
    member inline this.text (value:string) = this.props.add PName.button.text value
    member inline this.wantContinuousButtonPressed (value:bool) = this.props.add PName.button.wantContinuousButtonPressed value

// CheckBox
type checkBoxProps() =
    inherit viewProps()
    // Properties
    member inline this.allowCheckStateNone (value:bool) = this.props.add PName.checkBox.allowCheckStateNone value
    member inline this.checkedState (value:CheckState) = this.props.add PName.checkBox.checkedState value
    // TODO: move to macros
    member inline this.checkedState (value:bool) = this.props.add PName.checkBox.checkedState (if value then CheckState.Checked else CheckState.UnChecked)
    member inline this.hotKeySpecifier (value:Rune) = this.props.add PName.checkBox.hotKeySpecifier value
    member inline this.radioStyle (value:bool) = this.props.add PName.checkBox.radioStyle value
    member inline this.text (value:string) = this.props.add PName.checkBox.text value
    // Events
    member inline this.checkedStateChanging (handler:ResultEventArgs<CheckState>->unit) = this.props.add PName.checkBox.checkedStateChanging handler
    member inline this.checkedStateChanged (handler:EventArgs<CheckState>->unit) = this.props.add PName.checkBox.checkedStateChanged handler

// ColorPicker
type colorPickerProps() =
    inherit viewProps()
    // Properties
    member inline this.selectedColor (value:Color) = this.props.add PName.colorPicker.selectedColor value
    member inline this.style (value:ColorPickerStyle) = this.props.add PName.colorPicker.style value
    // Events
    member inline this.colorChanged (handler:ResultEventArgs<Color>->unit) = this.props.add PName.colorPicker.colorChanged handler

// ColorPicker16
type colorPicker16Props() =
    inherit viewProps()
    // Properties
    member inline this.boxHeight (value:Int32) = this.props.add PName.colorPicker16.boxHeight value
    member inline this.boxWidth (value:Int32) = this.props.add PName.colorPicker16.boxWidth value
    member inline this.cursor (value:Point) = this.props.add PName.colorPicker16.cursor value
    member inline this.selectedColor (value:ColorName16) = this.props.add PName.colorPicker16.selectedColor value
    // Events
    member inline this.colorChanged (handler:ResultEventArgs<Color>->unit) = this.props.add PName.colorPicker16.colorChanged handler

// ComboBox
type comboBoxProps() =
    inherit viewProps()
    // Properties
    member inline this.hideDropdownListOnClick (value:bool) = this.props.add PName.comboBox.hideDropdownListOnClick value
    member inline this.readOnly (value:bool) = this.props.add PName.comboBox.readOnly value
    member inline this.searchText (value:string) = this.props.add PName.comboBox.searchText value
    member inline this.selectedItem (value:Int32) = this.props.add PName.comboBox.selectedItem value
    member inline this.source (value:string list) = this.props.add PName.comboBox.source value
    member inline this.text (value:string) = this.props.add PName.comboBox.text value
    // Events
    member inline this.collapsed (handler:unit->unit) = this.props.add PName.comboBox.collapsed handler
    member inline this.expanded (handler:unit->unit) = this.props.add PName.comboBox.expanded handler
    member inline this.openSelectedItem (handler:ListViewItemEventArgs->unit) = this.props.add PName.comboBox.openSelectedItem handler
    member inline this.selectedItemChanged (handler:ListViewItemEventArgs->unit) = this.props.add PName.comboBox.selectedItemChanged handler

// TextField
type textFieldProps() =
    inherit viewProps()
    // Properties
    member inline this.autocomplete (value:IAutocomplete) = this.props.add PName.textField.autocomplete value
    member inline this.caption (value:string) = this.props.add PName.textField.caption value
    member inline this.captionColor (value:Terminal.Gui.Drawing.Color) = this.props.add PName.textField.captionColor value
    member inline this.cursorPosition (value:Int32) = this.props.add PName.textField.cursorPosition value
    member inline this.readOnly (value:bool) = this.props.add PName.textField.readOnly value
    member inline this.secret (value:bool) = this.props.add PName.textField.secret value
    member inline this.selectedStart (value:Int32) = this.props.add PName.textField.selectedStart value
    member inline this.selectWordOnlyOnDoubleClick (value:bool) = this.props.add PName.textField.selectWordOnlyOnDoubleClick value
    member inline this.text (value:string) = this.props.add PName.textField.text value
    member inline this.used (value:bool) = this.props.add PName.textField.used value
    member inline this.useSameRuneTypeForWords (value:bool) = this.props.add PName.textField.useSameRuneTypeForWords value
    // Events
    member inline this.textChanging (handler:ResultEventArgs<string>->unit) = this.props.add PName.textField.textChanging handler

// DateField
type dateFieldProps() =
    inherit textFieldProps()
    // Properties
    member inline this.culture (value:CultureInfo) = this.props.add PName.dateField.culture value
    member inline this.cursorPosition (value:Int32) = this.props.add PName.dateField.cursorPosition value
    member inline this.date (value:DateTime) = this.props.add PName.dateField.date value
    // Events
    member inline this.dateChanged (handler:DateTimeEventArgs<DateTime>->unit) = this.props.add PName.dateField.dateChanged handler

// DatePicker
type datePickerProps() =
    inherit viewProps()
    // Properties
    member inline this.culture (value:CultureInfo) = this.props.add PName.datePicker.culture value
    member inline this.date (value:DateTime) = this.props.add PName.datePicker.date value

// Toplevel
type toplevelProps() =
    inherit viewProps()
    // Properties
    member inline this.modal (value:bool) = this.props.add PName.toplevel.modal value
    member inline this.running (value:bool) = this.props.add PName.toplevel.running value
    // Events
    member inline this.activate (handler:ToplevelEventArgs->unit) = this.props.add PName.toplevel.activate handler
    member inline this.closed (handler:ToplevelEventArgs->unit) = this.props.add PName.toplevel.closed handler
    member inline this.closing (handler:ToplevelClosingEventArgs->unit) = this.props.add PName.toplevel.closing handler
    member inline this.deactivate (handler:ToplevelEventArgs->unit) = this.props.add PName.toplevel.deactivate handler
    member inline this.loaded (handler:unit->unit) = this.props.add PName.toplevel.loaded handler
    member inline this.ready (handler:unit->unit) = this.props.add PName.toplevel.ready handler
    member inline this.sizeChanging (handler:SizeChangedEventArgs->unit) = this.props.add PName.toplevel.sizeChanging handler
    member inline this.unloaded (handler:unit->unit) = this.props.add PName.toplevel.unloaded handler

// Dialog
type dialogProps() =
    inherit toplevelProps()
    // Properties
    member inline this.buttonAlignment (value:Alignment) = this.props.add PName.dialog.buttonAlignment value
    member inline this.buttonAlignmentModes (value:AlignmentModes) = this.props.add PName.dialog.buttonAlignmentModes value
    member inline this.canceled (value:bool) = this.props.add PName.dialog.canceled value

// FileDialog
type fileDialogProps() =
    inherit dialogProps()
    // Properties
    member inline this.allowedTypes (value:IAllowedType list) = this.props.add PName.fileDialog.allowedTypes value
    member inline this.allowsMultipleSelection (value:bool) = this.props.add PName.fileDialog.allowsMultipleSelection value
    member inline this.fileOperationsHandler (value:IFileOperations) = this.props.add PName.fileDialog.fileOperationsHandler value
    member inline this.mustExist (value:bool) = this.props.add PName.fileDialog.mustExist value
    member inline this.openMode (value:OpenMode) = this.props.add PName.fileDialog.openMode value
    member inline this.path (value:string) = this.props.add PName.fileDialog.path value
    member inline this.searchMatcher (value:ISearchMatcher) = this.props.add PName.fileDialog.searchMatcher value
    // Events
    member inline this.filesSelected (handler:FilesSelectedEventArgs->unit) = this.props.add PName.fileDialog.filesSelected handler

/// SaveDialog
type saveDialogProps() =
    inherit fileDialogProps()

// FrameView
type frameViewProps() =
    inherit viewProps()
// No properties or events FrameView

// GraphView
type graphViewProps() =
    inherit viewProps()

    // Properties
    member inline this.axisX (value:HorizontalAxis) = this.props.add PName.graphView.axisX value
    member inline this.axisY (value:VerticalAxis) = this.props.add PName.graphView.axisY value
    member inline this.cellSize (value:PointF) = this.props.add PName.graphView.cellSize value
    member inline this.graphColor (value:Attribute option) = this.props.add PName.graphView.graphColor value
    member inline this.marginBottom (value:int) = this.props.add PName.graphView.marginBottom value
    member inline this.marginLeft (value:int) = this.props.add PName.graphView.marginLeft value
    member inline this.scrollOffset (value:PointF) = this.props.add PName.graphView.scrollOffset value

// HexView
type hexViewProps() =
    inherit viewProps()

    // Properties
    member inline this.address (value:Int64) = this.props.add PName.hexView.address value
    member inline this.addressWidth (value:int) = this.props.add PName.hexView.addressWidth value
    member inline this.allowEdits (value:int) = this.props.add PName.hexView.allowEdits value
    member inline this.readOnly (value:bool) = this.props.add PName.hexView.readOnly value
    member inline this.source (value:Stream) = this.props.add PName.hexView.source value
    // Events
    member inline this.edited (handler:HexViewEditEventArgs->unit) = this.props.add PName.hexView.edited handler
    member inline this.positionChanged (handler:HexViewEventArgs->unit) = this.props.add PName.hexView.positionChanged handler

// Label
type labelProps() =
    inherit viewProps()

    // Properties
    member inline this.hotKeySpecifier (value:Rune) = this.props.add PName.label.hotKeySpecifier value
    member inline this.text (value:string) = this.props.add PName.label.text value

// LegendAnnotation
type legendAnnotationProps() =
    inherit viewProps()
// No properties or events LegendAnnotation

// Line
type lineProps() =
    inherit viewProps()

    // Properties
    member inline this.orientation (value:Orientation) = this.props.add PName.line.orientation value
    // Events
    member inline this.orientationChanged (handler:Orientation->unit) = this.props.add PName.line.orientationChanged handler
    member inline this.orientationChanging (handler:App.CancelEventArgs<Orientation>->unit) = this.props.add PName.line.orientationChanging handler

// LineView
type lineViewProps() =
    inherit viewProps()

    // Properties
    member inline this.endingAnchor (value:Rune option) = this.props.add PName.lineView.endingAnchor value
    member inline this.lineRune (value:Rune) = this.props.add PName.lineView.lineRune value
    member inline this.orientation (value:Orientation) = this.props.add PName.lineView.orientation value
    member inline this.startingAnchor (value:Rune option) = this.props.add PName.lineView.startingAnchor value

// ListView
type listViewProps() =
    inherit viewProps()

    // Properties
    member inline this.allowsMarking (value:bool) = this.props.add PName.listView.allowsMarking value
    member inline this.allowsMultipleSelection (value:bool) = this.props.add PName.listView.allowsMultipleSelection value
    member inline this.leftItem (value:Int32) = this.props.add PName.listView.leftItem value
    member inline this.selectedItem (value:Int32) = this.props.add PName.listView.selectedItem value
    member inline this.source (value:string list) = this.props.add PName.listView.source value
    member inline this.topItem (value:Int32) = this.props.add PName.listView.topItem value
    // Events
    member inline this.collectionChanged (handler:NotifyCollectionChangedEventArgs->unit) = this.props.add PName.listView.collectionChanged handler
    member inline this.openSelectedItem (handler:ListViewItemEventArgs->unit) = this.props.add PName.listView.openSelectedItem handler
    member inline this.rowRender (handler:ListViewRowEventArgs->unit) = this.props.add PName.listView.rowRender handler
    member inline this.selectedItemChanged (handler:ListViewItemEventArgs->unit) = this.props.add PName.listView.selectedItemChanged handler

// Margin
type marginProps() =
    inherit adornmentProps()

    // Properties
    member inline this.shadowStyle (value:ShadowStyle) = this.props.add PName.margin.shadowStyle value

type menuv2Props() =
    inherit barProps()

    // Properties
    member inline this.selectedMenuItem (value: MenuItemv2 list) = this.props.add PName.menuv2.selectedMenuItem value
    member inline this.superMenuItem (value: MenuItemv2 list) = this.props.add PName.menuv2.superMenuItem value
    // Events
    member inline this.accepted (value: CommandEventArgs->unit) = this.props.add PName.menuv2.accepted value
    member inline this.selectedMenuItemChanged (value: MenuItemv2->unit) = this.props.add PName.menuv2.selectedMenuItemChanged value

// MenuBarV2
type menuBarv2Props() =
    inherit menuv2Props()

    // Properties
    member inline this.key (value:Key) = this.props.add PName.menuBarv2.key value member inline this.menus (value:MenuBarItemv2Element list) = this.props.add PName.view.children (List<_>(value |> Seq.map (fun v -> v :> TerminalElement)))
    // Events
    member inline this.keyChanged (handler:KeyChangedEventArgs->unit) = this.props.add PName.menuBarv2.keyChanged handler

type shortcutProps() =
     inherit viewProps()

     // Properties
     member inline this.action (value:Action) = this.props.add PName.shortcut.action value
     member inline this.alignmentModes (value:AlignmentModes) = this.props.add PName.shortcut.alignmentModes value
     member inline this.commandView (value:TerminalElement) = this.props.add PName.shortcut.commandView_element value
     member inline this.forceFocusColors (value:bool) = this.props.add PName.shortcut.forceFocusColors value
     member inline this.helpText (value:string) = this.props.add PName.shortcut.helpText value
     member inline this.text (value:string) = this.props.add PName.shortcut.text value
     member inline this.bindKeyToApplication (value:bool) = this.props.add PName.shortcut.bindKeyToApplication value
     member inline this.key (value:Key) = this.props.add PName.shortcut.key value
     member inline this.minimumKeyTextSize (value:Int32) = this.props.add PName.shortcut.minimumKeyTextSize value
     // Events
     member inline this.orientationChanged (handler:Orientation->unit) = this.props.add PName.shortcut.orientationChanged handler
     member inline this.orientationChanging (handler:CancelEventArgs<Orientation>->unit) = this.props.add PName.shortcut.orientationChanging handler

type menuItemv2Props() =
    inherit shortcutProps()
    member inline this.command (value: Command) = this.props.add PName.menuItemv2.command value
    member inline this.submenu(value: Menuv2Element) = this.props.add PName.menuItemv2.subMenu_element value
    member inline this.accepted(value: CommandEventArgs -> unit) = this.props.add PName.menuItemv2.accepted value

type menuBarItemv2Props() =
    inherit menuItemv2Props()
    member inline this.popoverMenu (value:PopoverMenuElement) = this.props.add PName.menuBarItemv2.popoverMenu_element value
    member inline this.popoverMenuOpen (value:bool) = this.props.add PName.menuBarItemv2.popoverMenuOpen value

type popoverMenuProps() =
    inherit viewProps()

    member inline this.key (value:Key) = this.props.add PName.popoverMenu.key value
    member inline this.root (value: Menuv2Element) = this.props.add PName.popoverMenu.root_element value

// NumericUpDown`1
type numericUpDownProps<'a>() =
    inherit viewProps()

    // Properties
    member inline this.format (value:string) = this.props.add PName.numericUpDown.format value
    member inline this.increment (value:'a) = this.props.add PName.numericUpDown.increment value
    member inline this.value (value:'a) = this.props.add PName.numericUpDown.value value
    // Events
    member inline this.formatChanged (handler:string->unit) = this.props.add PName.numericUpDown.formatChanged handler
    member inline this.incrementChanged (handler:'a->unit) = this.props.add PName.numericUpDown.incrementChanged handler
    member inline this.valueChanged (handler:'a->unit) = this.props.add PName.numericUpDown.valueChanged handler
    member inline this.valueChanging (handler:App.CancelEventArgs<'a>->unit) = this.props.add PName.numericUpDown.valueChanging handler

// NumericUpDown
type numericUpDownProps() =
    inherit numericUpDownProps<int>()
// No properties or events NumericUpDown

// OpenDialog
type openDialogProps() =
    inherit fileDialogProps()
    // Properties
    member inline this.openMode (value:OpenMode) = this.props.add PName.openDialog.openMode value

// OptionSelector
type optionSelectorProps() =
    inherit viewProps()
    //Properties
    member inline this.assignHotKeysToCheckBoxes (value:bool) = this.props.add PName.optionSelector.assignHotKeysToCheckBoxes value
    member inline this.orientation (value:Orientation) = this.props.add PName.optionSelector.orientation value
    member inline this.options (value:IReadOnlyList<string>) = this.props.add PName.optionSelector.options value
    member inline this.options (value:string list) = this.props.add PName.optionSelector.options (value :> IReadOnlyList<string>)
    member inline this.selectedItem (value:Int32) = this.props.add PName.optionSelector.selectedItem value
    // Events
    member inline this.orientationChanged (value:Orientation->unit) = this.props.add PName.optionSelector.orientationChanged value
    member inline this.orientationChanging (value:CancelEventArgs<Orientation>->unit) = this.props.add PName.optionSelector.orientationChanging value
    member inline this.selectedItemChanged (value:SelectedItemChangedArgs->unit) = this.props.add PName.optionSelector.selectedItemChanged value

// Padding
type paddingProps() =
    inherit adornmentProps()

// ProgressBar
type progressBarProps() =
    inherit viewProps()

    // Properties
    member inline this.bidirectionalMarquee (value:bool) = this.props.add PName.progressBar.bidirectionalMarquee value
    member inline this.fraction (value:Single) = this.props.add PName.progressBar.fraction value
    member inline this.progressBarFormat (value:ProgressBarFormat) = this.props.add PName.progressBar.progressBarFormat value
    member inline this.progressBarStyle (value:ProgressBarStyle) = this.props.add PName.progressBar.progressBarStyle value
    member inline this.segmentCharacter (value:Rune) = this.props.add PName.progressBar.segmentCharacter value
    member inline this.text (value:string) = this.props.add PName.progressBar.text value

// RadioGroup
type radioGroupProps() =
    inherit viewProps()

    // Properties
    member inline this.assignHotKeysToRadioLabels (value:bool) = this.props.add PName.radioGroup.assignHotKeysToRadioLabels value
    member inline this.cursor (value:Int32) = this.props.add PName.radioGroup.cursor value
    member inline this.doubleClickAccepts (value:bool) = this.props.add PName.radioGroup.doubleClickAccepts value
    member inline this.horizontalSpace (value:Int32) = this.props.add PName.radioGroup.horizontalSpace value
    member inline this.orientation (value:Orientation) = this.props.add PName.radioGroup.orientation value
    member inline this.radioLabels (value:string list) = this.props.add PName.radioGroup.radioLabels value
    member inline this.selectedItem (value:Int32) = this.props.add PName.radioGroup.selectedItem value
    // Events
    member inline this.orientationChanged (handler:Orientation->unit) = this.props.add PName.radioGroup.orientationChanged handler
    member inline this.orientationChanging (handler:App.CancelEventArgs<Orientation>->unit) = this.props.add PName.radioGroup.orientationChanging handler
    member inline this.selectedItemChanged (handler:SelectedItemChangedArgs->unit) = this.props.add PName.radioGroup.selectedItemChanged handler

// SaveDialog
// No properties or events SaveDialog

// ScrollBar
type scrollBarProps() =
    inherit viewProps()

    // Properties
    member inline this.autoShow (value:bool) = this.props.add PName.scrollBar.autoShow value
    member inline this.increment (value:Int32) = this.props.add PName.scrollBar.increment value
    member inline this.orientation (value:Orientation) = this.props.add PName.scrollBar.orientation value
    member inline this.position (value:Int32) = this.props.add PName.scrollBar.position value
    member inline this.scrollableContentSize (value:Int32) = this.props.add PName.scrollBar.scrollableContentSize value
    member inline this.visibleContentSize (value:Int32) = this.props.add PName.scrollBar.visibleContentSize value
    // Events
    member inline this.orientationChanged (handler:Orientation->unit) = this.props.add PName.scrollBar.orientationChanged handler
    member inline this.orientationChanging (handler:CancelEventArgs<Orientation>->unit) = this.props.add PName.scrollBar.orientationChanging handler
    member inline this.scrollableContentSizeChanged (handler:EventArgs<Int32>->unit) = this.props.add PName.scrollBar.scrollableContentSizeChanged handler
    member inline this.sliderPositionChanged (handler:EventArgs<Int32>->unit) = this.props.add PName.scrollBar.sliderPositionChanged handler

// ScrollSlider
type scrollSliderProps() =
    inherit viewProps()

    // Properties
    member inline this.orientation (value:Orientation) = this.props.add PName.scrollSlider.orientation value
    member inline this.position (value:Int32) = this.props.add PName.scrollSlider.position value
    member inline this.size (value:Int32) = this.props.add PName.scrollSlider.size value
    member inline this.sliderPadding (value:Int32) = this.props.add PName.scrollSlider.sliderPadding value
    member inline this.visibleContentSize (value:Int32) = this.props.add PName.scrollSlider.visibleContentSize value
    // Events
    member inline this.orientationChanged (handler:Orientation->unit) = this.props.add PName.scrollSlider.orientationChanged handler
    member inline this.orientationChanging (handler:CancelEventArgs<Orientation>->unit) = this.props.add PName.scrollSlider.orientationChanging handler
    member inline this.positionChanged (handler:EventArgs<Int32>->unit) = this.props.add PName.scrollSlider.positionChanged handler
    member inline this.positionChanging (handler:CancelEventArgs<Int32>->unit) = this.props.add PName.scrollSlider.positionChanging handler
    member inline this.scrolled (handler:EventArgs<Int32>->unit) = this.props.add PName.scrollSlider.scrolled handler

// Slider`1
type sliderProps<'a>() =
    inherit viewProps()

    // Properties
    member inline this.allowEmpty (value:bool) = this.props.add PName.slider.allowEmpty value
    member inline this.focusedOption (value:Int32) = this.props.add PName.slider.focusedOption value
    member inline this.legendsOrientation (value:Orientation) = this.props.add PName.slider.legendsOrientation value
    member inline this.minimumInnerSpacing (value:Int32) = this.props.add PName.slider.minimumInnerSpacing value
    member inline this.options (value:SliderOption<'a> list) = this.props.add PName.slider.options value
    member inline this.orientation (value:Orientation) = this.props.add PName.slider.orientation value
    member inline this.rangeAllowSingle (value:bool) = this.props.add PName.slider.rangeAllowSingle value
    member inline this.showEndSpacing (value:bool) = this.props.add PName.slider.showEndSpacing value
    member inline this.showLegends (value:bool) = this.props.add PName.slider.showLegends value
    member inline this.style (value:SliderStyle) = this.props.add PName.slider.style value
    member inline this.text (value:string) = this.props.add PName.slider.text value
    member inline this.``type`` (value:SliderType) = this.props.add PName.slider.``type`` value
    member inline this.useMinimumSize (value:bool) = this.props.add PName.slider.useMinimumSize value
    // Events
    member inline this.optionFocused (handler:SliderEventArgs<'a>->unit) = this.props.add PName.slider.optionFocused handler
    member inline this.optionsChanged (handler:SliderEventArgs<'a>->unit) = this.props.add PName.slider.optionsChanged handler
    member inline this.orientationChanged (handler:Orientation->unit) = this.props.add PName.slider.orientationChanged handler
    member inline this.orientationChanging (handler:App.CancelEventArgs<Orientation>->unit) = this.props.add PName.slider.orientationChanging handler

// Slider
type sliderProps() =
    inherit sliderProps<obj>()
// No properties or events Slider

// SpinnerView
type spinnerViewProps() =
    inherit viewProps()

    // Properties
    member inline this.autoSpin (value:bool) = this.props.add PName.spinnerView.autoSpin value
    member inline this.sequence (value:string list) = this.props.add PName.spinnerView.sequence value
    member inline this.spinBounce (value:bool) = this.props.add PName.spinnerView.spinBounce value
    member inline this.spinDelay (value:Int32) = this.props.add PName.spinnerView.spinDelay value
    member inline this.spinReverse (value:bool) = this.props.add PName.spinnerView.spinReverse value
    member inline this.style (value:SpinnerStyle) = this.props.add PName.spinnerView.style value

// StatusBar
type statusBarProps() =
    inherit barProps()
// No properties or events StatusBar

// Tab
type tabProps() =
    inherit viewProps()

    // Properties
    member inline this.displayText (value:string) = this.props.add PName.tab.displayText value
    member inline this.view (value:TerminalElement) = this.props.add PName.tab.view value

// TabView
type tabViewProps() =
    inherit viewProps()

    // Properties
    member inline this.maxTabTextWidth (value:int) = this.props.add PName.tabView.maxTabTextWidth value
    member inline this.selectedTab (value:Tab) = this.props.add PName.tabView.selectedTab value
    member inline this.style (value:TabStyle) = this.props.add PName.tabView.style value
    member inline this.tabScrollOffset (value:Int32) = this.props.add PName.tabView.tabScrollOffset value
    // Events
    member inline this.selectedTabChanged (handler:TabChangedEventArgs->unit) = this.props.add PName.tabView.selectedTabChanged handler
    member inline this.tabClicked (handler:TabMouseEventArgs->unit) = this.props.add PName.tabView.tabClicked handler

    member inline this.tabs (value:TerminalElement list) = this.props.add PName.tabView.tabs value

// TableView
type tableViewProps() =
    inherit viewProps()

    // Properties
    member inline this.cellActivationKey (value:KeyCode) = this.props.add PName.tableView.cellActivationKey value
    member inline this.collectionNavigator (value:ICollectionNavigator) = this.props.add PName.tableView.collectionNavigator value
    member inline this.columnOffset (value:Int32) = this.props.add PName.tableView.columnOffset value
    member inline this.fullRowSelect (value:bool) = this.props.add PName.tableView.fullRowSelect value
    member inline this.maxCellWidth (value:Int32) = this.props.add PName.tableView.maxCellWidth value
    member inline this.minCellWidth (value:Int32) = this.props.add PName.tableView.minCellWidth value
    member inline this.multiSelect (value:bool) = this.props.add PName.tableView.multiSelect value
    member inline this.nullSymbol (value:string) = this.props.add PName.tableView.nullSymbol value
    member inline this.rowOffset (value:Int32) = this.props.add PName.tableView.rowOffset value
    member inline this.selectedColumn (value:Int32) = this.props.add PName.tableView.selectedColumn value
    member inline this.selectedRow (value:Int32) = this.props.add PName.tableView.selectedRow value
    member inline this.separatorSymbol (value:Char) = this.props.add PName.tableView.separatorSymbol value
    member inline this.style (value:TableStyle) = this.props.add PName.tableView.style value
    member inline this.table (value:ITableSource) = this.props.add PName.tableView.table value
    // Events
    member inline this.cellActivated (handler:CellActivatedEventArgs->unit) = this.props.add PName.tableView.cellActivated handler
    member inline this.cellToggled (handler:CellToggledEventArgs->unit) = this.props.add PName.tableView.cellToggled handler
    member inline this.selectedCellChanged (handler:SelectedCellChangedEventArgs->unit) = this.props.add PName.tableView.selectedCellChanged handler

// TextValidateField
type textValidateFieldProps() =
    inherit viewProps()

    // Properties
    member inline this.provider (value:ITextValidateProvider) = this.props.add PName.textValidateField.provider value
    member inline this.text (value:string) = this.props.add PName.textValidateField.text value

// TextView
type textViewProps() =
    inherit viewProps()

    // Properties
    member inline this.allowsReturn (value:bool) = this.props.add PName.textView.allowsReturn value
    member inline this.allowsTab (value:bool) = this.props.add PName.textView.allowsTab value
    member inline this.cursorPosition (value:Point) = this.props.add PName.textView.cursorPosition value
    member inline this.inheritsPreviousAttribute (value:bool) = this.props.add PName.textView.inheritsPreviousAttribute value
    member inline this.isDirty (value:bool) = this.props.add PName.textView.isDirty value
    member inline this.isSelecting (value:bool) = this.props.add PName.textView.isSelecting value
    member inline this.leftColumn (value:Int32) = this.props.add PName.textView.leftColumn value
    member inline this.multiline (value:bool) = this.props.add PName.textView.multiline value
    member inline this.readOnly (value:bool) = this.props.add PName.textView.readOnly value
    member inline this.selectionStartColumn (value:Int32) = this.props.add PName.textView.selectionStartColumn value
    member inline this.selectionStartRow (value:Int32) = this.props.add PName.textView.selectionStartRow value
    member inline this.selectWordOnlyOnDoubleClick (value:bool) = this.props.add PName.textView.selectWordOnlyOnDoubleClick value
    member inline this.tabWidth (value:Int32) = this.props.add PName.textView.tabWidth value
    member inline this.text (value:string) = this.props.add PName.textView.text value
    member inline this.topRow (value:Int32) = this.props.add PName.textView.topRow value
    member inline this.used (value:bool) = this.props.add PName.textView.used value
    member inline this.useSameRuneTypeForWords (value:bool) = this.props.add PName.textView.useSameRuneTypeForWords value
    member inline this.wordWrap (value:bool) = this.props.add PName.textView.wordWrap value
    // Events
    member inline this.contentsChanged (handler:ContentsChangedEventArgs->unit) = this.props.add PName.textView.contentsChanged handler
    member inline this.drawNormalColor (handler:CellEventArgs->unit) = this.props.add PName.textView.drawNormalColor handler
    member inline this.drawReadOnlyColor (handler:CellEventArgs->unit) = this.props.add PName.textView.drawReadOnlyColor handler
    member inline this.drawSelectionColor (handler:CellEventArgs->unit) = this.props.add PName.textView.drawSelectionColor handler
    member inline this.drawUsedColor (handler:CellEventArgs->unit) = this.props.add PName.textView.drawUsedColor handler
    member inline this.unwrappedCursorPosition (handler:Point->unit) = this.props.add PName.textView.unwrappedCursorPosition handler
    // Additional properties
    member inline this.textChanged (value:string->unit) = this.props.add PName.textView.textChanged value

// TileView
type tileViewProps() =
    inherit viewProps()

    // Properties
    member inline this.lineStyle (value:LineStyle) = this.props.add PName.tileView.lineStyle value
    member inline this.orientation (value:Orientation) = this.props.add PName.tileView.orientation value
    member inline this.toggleResizable (value:KeyCode) = this.props.add PName.tileView.toggleResizable value
    // Events
    member inline this.splitterMoved (handler:SplitterEventArgs->unit) = this.props.add PName.tileView.splitterMoved handler

// TimeField
type timeFieldProps() =
    inherit textFieldProps()

    // Properties
    member inline this.cursorPosition (value:Int32) = this.props.add PName.timeField.cursorPosition value
    member inline this.isShortFormat (value:bool) = this.props.add PName.timeField.isShortFormat value
    member inline this.time (value:TimeSpan) = this.props.add PName.timeField.time value
    // Events
    member inline this.timeChanged (handler:DateTimeEventArgs<TimeSpan>->unit) = this.props.add PName.timeField.timeChanged handler

// TreeView`1
type treeViewProps<'a when 'a : not struct>() =
    inherit viewProps()

    // Properties
    member inline this.allowLetterBasedNavigation (value:bool) = this.props.add PName.treeView.allowLetterBasedNavigation value
    member inline this.aspectGetter<'a when 'a : not struct> (value:AspectGetterDelegate<'a>) = this.props.add PName.treeView.aspectGetter value
    member inline this.colorGetter<'a when 'a : not struct> (value:Func<'a,Scheme>) = this.props.add PName.treeView.colorGetter value
    member inline this.maxDepth (value:Int32) = this.props.add PName.treeView.maxDepth value
    member inline this.multiSelect (value:bool) = this.props.add PName.treeView.multiSelect value
    member inline this.objectActivationButton (value:MouseFlags option) = this.props.add PName.treeView.objectActivationButton value
    member inline this.objectActivationKey (value:KeyCode) = this.props.add PName.treeView.objectActivationKey value
    member inline this.scrollOffsetHorizontal (value:Int32) = this.props.add PName.treeView.scrollOffsetHorizontal value
    member inline this.scrollOffsetVertical (value:Int32) = this.props.add PName.treeView.scrollOffsetVertical value
    member inline this.selectedObject<'a when 'a : not struct> (value:'a) = this.props.add PName.treeView.selectedObject value
    member inline this.style (value:TreeStyle) = this.props.add PName.treeView.style value
    member inline this.treeBuilder<'a when 'a : not struct> (value:ITreeBuilder<'a>) = this.props.add PName.treeView.treeBuilder value
    // Events
    member inline this.drawLine<'a when 'a : not struct> (handler:DrawTreeViewLineEventArgs<'a>->unit) = this.props.add PName.treeView.drawLine handler
    member inline this.objectActivated<'a when 'a : not struct> (handler:ObjectActivatedEventArgs<'a>->unit) = this.props.add PName.treeView.objectActivated handler
    member inline this.selectionChanged<'a when 'a : not struct> (handler:SelectionChangedEventArgs<'a>->unit) = this.props.add PName.treeView.selectionChanged handler

// TreeView
type treeViewProps() =
    inherit treeViewProps<ITreeNode>()
// No properties or events TreeView

// Window
type windowProps() =
    inherit toplevelProps()
// No properties or events Window

// Wizard
type wizardProps() =
    inherit dialogProps()

    // Properties
    member inline this.currentStep (value:WizardStep) = this.props.add PName.wizard.currentStep value
    member inline this.modal (value:bool) = this.props.add PName.wizard.modal value
    // Events
    member inline this.cancelled (handler:WizardButtonEventArgs->unit) = this.props.add PName.wizard.cancelled handler
    member inline this.finished (handler:WizardButtonEventArgs->unit) = this.props.add PName.wizard.finished handler
    member inline this.movingBack (handler:WizardButtonEventArgs->unit) = this.props.add PName.wizard.movingBack handler
    member inline this.movingNext (handler:WizardButtonEventArgs->unit) = this.props.add PName.wizard.movingNext handler
    member inline this.stepChanged (handler:StepChangeEventArgs->unit) = this.props.add PName.wizard.stepChanged handler
    member inline this.stepChanging (handler:StepChangeEventArgs->unit) = this.props.add PName.wizard.stepChanging handler

// WizardStep
type wizardStepProps() =
    inherit viewProps()

    // Properties
    member inline this.backButtonText (value:string) = this.props.add PName.wizardStep.backButtonText value
    member inline this.helpText (value:string) = this.props.add PName.wizardStep.helpText value
    member inline this.nextButtonText (value:string) = this.props.add PName.wizardStep.nextButtonText value
