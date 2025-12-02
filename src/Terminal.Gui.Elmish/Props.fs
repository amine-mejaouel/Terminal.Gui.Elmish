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

open Terminal.Gui.FileServices
open Terminal.Gui.Input

open Terminal.Gui.Text

open Terminal.Gui.ViewBase

open Terminal.Gui.Views

// View
type viewProps() =
  member val internal props = Props()

  member this.children(children: ITerminalElement list) =
    this.props.add (
      PKey.view.children,
      List<_>(
        children
        |> List.map (fun x -> x :?> IInternalTerminalElement)
      )
    )

  // Properties
  member this.arrangement(value: ViewArrangement) =
    this.props.add (PKey.view.arrangement, value)

  member this.borderStyle(value: LineStyle) =
    this.props.add (PKey.view.borderStyle, value)

  member this.canFocus(value: bool) =
    this.props.add (PKey.view.canFocus, value)

  member this.contentSizeTracksViewport(value: bool) =
    this.props.add (PKey.view.contentSizeTracksViewport, value)

  member this.cursorVisibility(value: CursorVisibility) =
    this.props.add (PKey.view.cursorVisibility, value)

  member this.data(value: Object) = this.props.add (PKey.view.data, value)

  member this.enabled(value: bool) =
    this.props.add (PKey.view.enabled, value)

  member this.frame(value: Rectangle) = this.props.add (PKey.view.frame, value)

  member this.hasFocus(value: bool) =
    this.props.add (PKey.view.hasFocus, value)

  member this.height(value: Dim) =
    this.props.add (PKey.view.height, value)

  member this.highlightStates(value: MouseState) =
    this.props.add (PKey.view.highlightStates, value)

  member this.hotKey(value: Key) =
    this.props.add (PKey.view.hotKey, value)

  member this.hotKeySpecifier(value: Rune) =
    this.props.add (PKey.view.hotKeySpecifier, value)

  member this.id(value: string) = this.props.add (PKey.view.id, value)

  member this.isInitialized(value: bool) =
    this.props.add (PKey.view.isInitialized, value)

  member this.mouseHeldDown(value: IMouseHeldDown) =
    this.props.add (PKey.view.mouseHeldDown, value)

  member this.needsDraw(value: bool) =
    this.props.add (PKey.view.needsDraw, value)

  member this.preserveTrailingSpaces(value: bool) =
    this.props.add (PKey.view.preserveTrailingSpaces, value)

  member this.schemeName(value: string) =
    this.props.add (PKey.view.schemeName, value)

  member this.shadowStyle(value: ShadowStyle) =
    this.props.add (PKey.view.shadowStyle, value)

  member this.superViewRendersLineCanvas(value: bool) =
    this.props.add (PKey.view.superViewRendersLineCanvas, value)

  member this.tabStop(value: TabBehavior option) =
    this.props.add (PKey.view.tabStop, value)

  member this.text(value: string) = this.props.add (PKey.view.text, value)

  member this.textAlignment(value: Alignment) =
    this.props.add (PKey.view.textAlignment, value)

  member this.textDirection(value: TextDirection) =
    this.props.add (PKey.view.textDirection, value)

  member this.title(value: string) = this.props.add (PKey.view.title, value)

  member this.validatePosDim(value: bool) =
    this.props.add (PKey.view.validatePosDim, value)

  member this.verticalTextAlignment(value: Alignment) =
    this.props.add (PKey.view.verticalTextAlignment, value)

  member this.viewport(value: Rectangle) =
    this.props.add (PKey.view.viewport, value)

  member this.viewportSettings(value: ViewportSettingsFlags) =
    this.props.add (PKey.view.viewportSettings, value)

  member this.visible(value: bool) =
    this.props.add (PKey.view.visible, value)

  member this.wantContinuousButtonPressed(value: bool) =
    this.props.add (PKey.view.wantContinuousButtonPressed, value)

  member this.wantMousePositionReports(value: bool) =
    this.props.add (PKey.view.wantMousePositionReports, value)

  member this.width(value: Dim) = this.props.add (PKey.view.width, value)
  member this.x(value: Pos) = this.props.add (PKey.view.x, value)

  member this.x(value: TPos) =
    this.props.add (PKey.view.x_delayedPos, value)

  member this.y(value: Pos) = this.props.add (PKey.view.y, value)

  member this.y(value: TPos) =
    this.props.add (PKey.view.y_delayedPos, value)

  // Events
  member this.accepting(handler: HandledEventArgs -> unit) =
    this.props.add (PKey.view.accepting, handler)

  member this.advancingFocus(handler: AdvanceFocusEventArgs -> unit) =
    this.props.add (PKey.view.advancingFocus, handler)

  member this.borderStyleChanged(handler: EventArgs -> unit) =
    this.props.add (PKey.view.borderStyleChanged, handler)

  member this.canFocusChanged(handler: unit -> unit) =
    this.props.add (PKey.view.canFocusChanged, handler)

  member this.clearedViewport(handler: DrawEventArgs -> unit) =
    this.props.add (PKey.view.clearedViewport, handler)

  member this.clearingViewport(handler: DrawEventArgs -> unit) =
    this.props.add (PKey.view.clearingViewport, handler)

  member this.commandNotBound(handler: CommandEventArgs -> unit) =
    this.props.add (PKey.view.commandNotBound, handler)

  member this.contentSizeChanged(handler: SizeChangedEventArgs -> unit) =
    this.props.add (PKey.view.contentSizeChanged, handler)

  member this.disposing(handler: unit -> unit) =
    this.props.add (PKey.view.disposing, handler)

  member this.drawComplete(handler: DrawEventArgs -> unit) =
    this.props.add (PKey.view.drawComplete, handler)

  member this.drawingContent(handler: DrawEventArgs -> unit) =
    this.props.add (PKey.view.drawingContent, handler)

  member this.drawingSubViews(handler: DrawEventArgs -> unit) =
    this.props.add (PKey.view.drawingSubViews, handler)

  member this.drawingText(handler: DrawEventArgs -> unit) =
    this.props.add (PKey.view.drawingText, handler)

  member this.enabledChanged(handler: unit -> unit) =
    this.props.add (PKey.view.enabledChanged, handler)

  member this.focusedChanged(handler: HasFocusEventArgs -> unit) =
    this.props.add (PKey.view.focusedChanged, handler)

  member this.frameChanged(handler: EventArgs<Rectangle> -> unit) =
    this.props.add (PKey.view.frameChanged, handler)

  member this.gettingAttributeForRole(handler: VisualRoleEventArgs -> unit) =
    this.props.add (PKey.view.gettingAttributeForRole, handler)

  member this.gettingScheme(handler: ResultEventArgs<Scheme> -> unit) =
    this.props.add (PKey.view.gettingScheme, handler)

  member this.handlingHotKey(handler: CommandEventArgs -> unit) =
    this.props.add (PKey.view.handlingHotKey, handler)

  member this.hasFocusChanged(handler: HasFocusEventArgs -> unit) =
    this.props.add (PKey.view.hasFocusChanged, handler)

  member this.hasFocusChanging(handler: HasFocusEventArgs -> unit) =
    this.props.add (PKey.view.hasFocusChanging, handler)

  member this.hotKeyChanged(handler: KeyChangedEventArgs -> unit) =
    this.props.add (PKey.view.hotKeyChanged, handler)

  member this.initialized(handler: unit -> unit) =
    this.props.add (PKey.view.initialized, handler)

  member this.keyDown(handler: Key -> unit) =
    this.props.add (PKey.view.keyDown, handler)

  member this.keyDownNotHandled(handler: Key -> unit) =
    this.props.add (PKey.view.keyDownNotHandled, handler)

  member this.keyUp(handler: Key -> unit) =
    this.props.add (PKey.view.keyUp, handler)

  member this.mouseClick(handler: MouseEventArgs -> unit) =
    this.props.add (PKey.view.mouseClick, handler)

  member this.mouseEnter(handler: CancelEventArgs -> unit) =
    this.props.add (PKey.view.mouseEnter, handler)

  member this.mouseEvent(handler: MouseEventArgs -> unit) =
    this.props.add (PKey.view.mouseEvent, handler)

  member this.mouseLeave(handler: EventArgs -> unit) =
    this.props.add (PKey.view.mouseLeave, handler)

  member this.mouseStateChanged(handler: EventArgs -> unit) =
    this.props.add (PKey.view.mouseStateChanged, handler)

  member this.mouseWheel(handler: MouseEventArgs -> unit) =
    this.props.add (PKey.view.mouseWheel, handler)

  member this.removed(handler: SuperViewChangedEventArgs -> unit) =
    this.props.add (PKey.view.removed, handler)

  member this.schemeChanged(handler: ValueChangedEventArgs<Scheme> -> unit) =
    this.props.add (PKey.view.schemeChanged, handler)

  member this.schemeChanging(handler: ValueChangingEventArgs<Scheme> -> unit) =
    this.props.add (PKey.view.schemeChanging, handler)

  member this.schemeNameChanged(handler: ValueChangedEventArgs<string> -> unit) =
    this.props.add (PKey.view.schemeNameChanged, handler)

  member this.schemeNameChanging(handler: ValueChangingEventArgs<string> -> unit) =
    this.props.add (PKey.view.schemeNameChanging, handler)

  member this.selecting(handler: CommandEventArgs -> unit) =
    this.props.add (PKey.view.selecting, handler)

  member this.subViewAdded(handler: SuperViewChangedEventArgs -> unit) =
    this.props.add (PKey.view.subViewAdded, handler)

  member this.subViewLayout(handler: LayoutEventArgs -> unit) =
    this.props.add (PKey.view.subViewLayout, handler)

  member this.subViewRemoved(handler: SuperViewChangedEventArgs -> unit) =
    this.props.add (PKey.view.subViewRemoved, handler)

  member this.subViewsLaidOut(handler: LayoutEventArgs -> unit) =
    this.props.add (PKey.view.subViewsLaidOut, handler)

  member this.superViewChanged(handler: SuperViewChangedEventArgs -> unit) =
    this.props.add (PKey.view.superViewChanged, handler)

  member this.textChanged(handler: unit -> unit) =
    this.props.add (PKey.view.textChanged, handler)

  member this.titleChanged(handler: string -> unit) =
    this.props.add (PKey.view.titleChanged, handler)

  member this.titleChanging(handler: App.CancelEventArgs<string> -> unit) =
    this.props.add (PKey.view.titleChanging, handler)

  member this.viewportChanged(handler: DrawEventArgs -> unit) =
    this.props.add (PKey.view.viewportChanged, handler)

  member this.visibleChanged(handler: unit -> unit) =
    this.props.add (PKey.view.visibleChanged, handler)

  member this.visibleChanging(handler: unit -> unit) =
    this.props.add (PKey.view.visibleChanging, handler)

// module prop =
//     module position =
//         type x =
//             static member absolute (position:int)                                        = Interop.mkprop PKey.view.x (Pos.Absolute(position))
//             static member align (alignment:Alignment, ?modes:AlignmentModes, ?groupId:int)
//                 =
//                     let modes = defaultArg modes AlignmentModes.StartToEnd ||| AlignmentModes.AddSpaceBetweenItems
//                     let groupId = defaultArg groupId 0
//                     Interop.mkprop PKey.view.x (Pos.Align(alignment, modes, groupId))
//             static member anchorEnd                                                      = Interop.mkprop PKey.view.x (Pos.AnchorEnd())
//             static member anchorEndWithOffset (offset:int)                               = Interop.mkprop PKey.view.x (Pos.AnchorEnd(offset))
//             static member center                                                         = Interop.mkprop PKey.view.x (Pos.Center())
//             static member func (f:View -> int)                                           = Interop.mkprop PKey.view.x (Pos.Func(f))
//             static member percent (percent:int)                                          = Interop.mkprop PKey.view.x (Pos.Percent(percent))
//
//         type y =
//             static member absolute (position:int)                                        = Interop.mkprop PKey.view.y (Pos.Absolute(position))
//             static member align (alignment:Alignment, ?modes:AlignmentModes, ?groupId:int)
//                 =
//                     let modes = defaultArg modes AlignmentModes.StartToEnd ||| AlignmentModes.AddSpaceBetweenItems
//                     let groupId = defaultArg groupId 0
//                     Interop.mkprop PKey.view.y (Pos.Align(alignment, modes, groupId))
//             static member anchorEnd                                                      = Interop.mkprop PKey.view.y (Pos.AnchorEnd())
//             static member anchorEndWithOffset (offset:int)                               = Interop.mkprop PKey.view.y (Pos.AnchorEnd(offset))
//             static member center                                                         = Interop.mkprop PKey.view.y (Pos.Center())
//             static member func (f:View -> int)                                           = Interop.mkprop PKey.view.y (Pos.Func(f))
//             static member percent (percent:int)                                          = Interop.mkprop PKey.view.y (Pos.Percent(percent))
//
//     type width =
//         static member absolute (size:int)                                                                    = Interop.mkprop PKey.view.width (Dim.Absolute(size))
//         static member auto (?style:DimAutoStyle, ?minimumContentDim:Dim, ?maximumContentDim:Dim)
//             =
//                 let style = defaultArg style DimAutoStyle.Auto
//                 let minimumContentDim = defaultArg minimumContentDim null
//                 let maximumContentDim = defaultArg maximumContentDim null
//                 Interop.mkprop PKey.view.width (Dim.Auto(style, minimumContentDim, maximumContentDim))
//         static member fill (margin:int)                                                                      = Interop.mkprop PKey.view.width (Dim.Fill(margin))
//         static member func (f:View->int)                                                                     = Interop.mkprop PKey.view.width (Dim.Func(f))
//         static member percent (percent:int, mode:DimPercentMode)                                             = Interop.mkprop PKey.view.width (Dim.Percent(percent, mode))
//         static member percent (percent:int)                                                                  = Interop.mkprop PKey.view.width (Dim.Percent(percent, DimPercentMode.ContentSize))
//
//     type height =
//         static member absolute (size:int)                                                                    = Interop.mkprop PKey.view.height (Dim.Absolute(size))
//         static member auto (?style:DimAutoStyle, ?minimumContentDim:Dim, ?maximumContentDim:Dim)
//             =
//                 let style = defaultArg style DimAutoStyle.Auto
//                 let minimumContentDim = defaultArg minimumContentDim null
//                 let maximumContentDim = defaultArg maximumContentDim null
//                 Interop.mkprop PKey.view.height (Dim.Auto(style, minimumContentDim, maximumContentDim))
//         static member fill (margin:int)                                                                      = Interop.mkprop PKey.view.height (Dim.Fill(margin))
//         static member func (f:View->int)                                                                     = Interop.mkprop PKey.view.height (Dim.Func(f))
//         static member percent (percent:int, mode:DimPercentMode)                                             = Interop.mkprop PKey.view.height (Dim.Percent(percent, mode))
//         static member percent (percent:int)                                                                  = Interop.mkprop PKey.view.height (Dim.Percent(percent, DimPercentMode.ContentSize))
//
//
//     type alignment =
//         static member center     =   Interop.mkprop PKey.view.alignment Alignment.Center
//         static member ``end``    =   Interop.mkprop PKey.view.alignment Alignment.End
//         static member start      =   Interop.mkprop PKey.view.alignment Alignment.Start
//         static member fill       =   Interop.mkprop PKey.view.alignment Alignment.Fill
//
//     type textDirection =
//         static member bottomTop_leftRight = Interop.mkprop PKey.view.textDirection TextDirection.BottomTop_LeftRight
//         static member bottomTop_rightLeft = Interop.mkprop PKey.view.textDirection TextDirection.BottomTop_RightLeft
//         static member leftRight_bottomTop = Interop.mkprop PKey.view.textDirection TextDirection.LeftRight_BottomTop
//         static member leftRight_topBottom = Interop.mkprop PKey.view.textDirection TextDirection.LeftRight_TopBottom
//         static member rightLeft_bottomTop = Interop.mkprop PKey.view.textDirection TextDirection.RightLeft_BottomTop
//         static member rightLeft_topBottom = Interop.mkprop PKey.view.textDirection TextDirection.RightLeft_TopBottom
//         static member topBottom_leftRight = Interop.mkprop PKey.view.textDirection TextDirection.TopBottom_LeftRight
//
//     type borderStyle =
//         static member double = Interop.mkprop    PKey.view.borderStyle LineStyle.Double
//         static member none = Interop.mkprop      PKey.view.borderStyle LineStyle.None
//         static member rounded = Interop.mkprop   PKey.view.borderStyle LineStyle.Rounded
//         static member single = Interop.mkprop    PKey.view.borderStyle LineStyle.Single
//
//     type shadowStyle =
//         static member none = Interop.mkprop          PKey.view.shadowStyle ShadowStyle.None
//         static member opaque = Interop.mkprop        PKey.view.shadowStyle ShadowStyle.Opaque
//         static member transparent = Interop.mkprop   PKey.view.shadowStyle ShadowStyle.Transparent

// Adornment
type adornmentProps() =
  inherit viewProps()
  // Properties
  member this.diagnostics(value: ViewDiagnosticFlags) =
    this.props.add (PKey.adornment.diagnostics, value)

  member this.superViewRendersLineCanvas(value: bool) =
    this.props.add (PKey.adornment.superViewRendersLineCanvas, value)

  member this.thickness(value: Thickness) =
    this.props.add (PKey.adornment.thickness, value)

  member this.viewport(value: Rectangle) =
    this.props.add (PKey.adornment.viewport, value)
  // Events
  member this.thicknessChanged(handler: unit -> unit) =
    this.props.add (PKey.adornment.thicknessChanged, handler)

// Bar
type barProps() =
  inherit viewProps()
  // Properties
  member this.alignmentModes(value: AlignmentModes) =
    this.props.add (PKey.bar.alignmentModes, value)

  member this.orientation(value: Orientation) =
    this.props.add (PKey.bar.orientation, value)
  // Events
  member this.orientationChanged(handler: Orientation -> unit) =
    this.props.add (PKey.bar.orientationChanged, handler)

  member this.orientationChanging(handler: App.CancelEventArgs<Orientation> -> unit) =
    this.props.add (PKey.bar.orientationChanging, handler)

// Border
type borderProps() =
  inherit adornmentProps()
  // Properties
  member this.lineStyle(value: LineStyle) =
    this.props.add (PKey.border.lineStyle, value)

  member this.settings(value: BorderSettings) =
    this.props.add (PKey.border.settings, value)

// Button
type buttonProps() =
  inherit viewProps()
  // Properties
  member this.hotKeySpecifier(value: Rune) =
    this.props.add (PKey.button.hotKeySpecifier, value)

  member this.isDefault(value: bool) =
    this.props.add (PKey.button.isDefault, value)

  member this.noDecorations(value: bool) =
    this.props.add (PKey.button.noDecorations, value)

  member this.noPadding(value: bool) =
    this.props.add (PKey.button.noPadding, value)

  member this.text(value: string) =
    this.props.add (PKey.button.text, value)

  member this.wantContinuousButtonPressed(value: bool) =
    this.props.add (PKey.button.wantContinuousButtonPressed, value)

// CheckBox
type checkBoxProps() =
  inherit viewProps()
  // Properties
  member this.allowCheckStateNone(value: bool) =
    this.props.add (PKey.checkBox.allowCheckStateNone, value)

  member this.checkedState(value: CheckState) =
    this.props.add (PKey.checkBox.checkedState, value)

  member this.hotKeySpecifier(value: Rune) =
    this.props.add (PKey.checkBox.hotKeySpecifier, value)

  member this.radioStyle(value: bool) =
    this.props.add (PKey.checkBox.radioStyle, value)

  member this.text(value: string) =
    this.props.add (PKey.checkBox.text, value)
  // Events
  member this.checkedStateChanging(handler: ResultEventArgs<CheckState> -> unit) =
    this.props.add (PKey.checkBox.checkedStateChanging, handler)

  member this.checkedStateChanged(handler: EventArgs<CheckState> -> unit) =
    this.props.add (PKey.checkBox.checkedStateChanged, handler)

// ColorPicker
type colorPickerProps() =
  inherit viewProps()
  // Properties
  member this.selectedColor(value: Color) =
    this.props.add (PKey.colorPicker.selectedColor, value)

  member this.style(value: ColorPickerStyle) =
    this.props.add (PKey.colorPicker.style, value)
  // Events
  member this.colorChanged(handler: ResultEventArgs<Color> -> unit) =
    this.props.add (PKey.colorPicker.colorChanged, handler)

// ColorPicker16
type colorPicker16Props() =
  inherit viewProps()
  // Properties
  member this.boxHeight(value: Int32) =
    this.props.add (PKey.colorPicker16.boxHeight, value)

  member this.boxWidth(value: Int32) =
    this.props.add (PKey.colorPicker16.boxWidth, value)

  member this.cursor(value: Point) =
    this.props.add (PKey.colorPicker16.cursor, value)

  member this.selectedColor(value: ColorName16) =
    this.props.add (PKey.colorPicker16.selectedColor, value)
  // Events
  member this.colorChanged(handler: ResultEventArgs<Color> -> unit) =
    this.props.add (PKey.colorPicker16.colorChanged, handler)

// ComboBox
type comboBoxProps() =
  inherit viewProps()
  // Properties
  member this.hideDropdownListOnClick(value: bool) =
    this.props.add (PKey.comboBox.hideDropdownListOnClick, value)

  member this.readOnly(value: bool) =
    this.props.add (PKey.comboBox.readOnly, value)

  member this.searchText(value: string) =
    this.props.add (PKey.comboBox.searchText, value)

  member this.selectedItem(value: Int32) =
    this.props.add (PKey.comboBox.selectedItem, value)

  member this.source(value: string list) =
    this.props.add (PKey.comboBox.source, value)

  member this.text(value: string) =
    this.props.add (PKey.comboBox.text, value)
  // Events
  member this.collapsed(handler: unit -> unit) =
    this.props.add (PKey.comboBox.collapsed, handler)

  member this.expanded(handler: unit -> unit) =
    this.props.add (PKey.comboBox.expanded, handler)

  member this.openSelectedItem(handler: ListViewItemEventArgs -> unit) =
    this.props.add (PKey.comboBox.openSelectedItem, handler)

  member this.selectedItemChanged(handler: ListViewItemEventArgs -> unit) =
    this.props.add (PKey.comboBox.selectedItemChanged, handler)

// TextField
type textFieldProps() =
  inherit viewProps()
  // Properties
  member this.autocomplete(value: IAutocomplete) =
    this.props.add (PKey.textField.autocomplete, value)

  member this.cursorPosition(value: Int32) =
    this.props.add (PKey.textField.cursorPosition, value)

  member this.readOnly(value: bool) =
    this.props.add (PKey.textField.readOnly, value)

  member this.secret(value: bool) =
    this.props.add (PKey.textField.secret, value)

  member this.selectedStart(value: Int32) =
    this.props.add (PKey.textField.selectedStart, value)

  member this.selectWordOnlyOnDoubleClick(value: bool) =
    this.props.add (PKey.textField.selectWordOnlyOnDoubleClick, value)

  member this.text(value: string) =
    this.props.add (PKey.textField.text, value)

  member this.used(value: bool) =
    this.props.add (PKey.textField.used, value)

  member this.useSameRuneTypeForWords(value: bool) =
    this.props.add (PKey.textField.useSameRuneTypeForWords, value)
  // Events
  member this.textChanging(handler: ResultEventArgs<string> -> unit) =
    this.props.add (PKey.textField.textChanging, handler)

// DateField
type dateFieldProps() =
  inherit textFieldProps()
  // Properties
  member this.culture(value: CultureInfo) =
    this.props.add (PKey.dateField.culture, value)

  member this.cursorPosition(value: Int32) =
    this.props.add (PKey.dateField.cursorPosition, value)

  member this.date(value: DateTime) =
    this.props.add (PKey.dateField.date, value)
  // Events
  member this.dateChanged(handler: EventArgs<DateTime> -> unit) =
    this.props.add (PKey.dateField.dateChanged, handler)

// DatePicker
type datePickerProps() =
  inherit viewProps()
  // Properties
  member this.culture(value: CultureInfo) =
    this.props.add (PKey.datePicker.culture, value)

  member this.date(value: DateTime) =
    this.props.add (PKey.datePicker.date, value)

// Runnable
type runnableProps() =
  inherit viewProps()
  // Properties
  member this.isModal(value: bool) =
    this.props.add (PKey.runnable.isModal, value)

  member this.isRunning(value: bool) =
    this.props.add (PKey.runnable.isRunning, value)
  // Events
  member this.isRunningChanging(handler: CancelEventArgs<bool> -> unit) =
    this.props.add (PKey.runnable.isRunningChanging, handler)

  member this.isRunningChanged(handler: EventArgs<bool> -> unit) =
    this.props.add (PKey.runnable.isRunningChanged, handler)

  member this.isModalChanged(handler: EventArgs<bool> -> unit) =
    this.props.add (PKey.runnable.isModalChanged, handler)

// Dialog
type dialogProps() =
  inherit runnableProps()
  // Properties
  member this.buttonAlignment(value: Alignment) =
    this.props.add (PKey.dialog.buttonAlignment, value)

  member this.buttonAlignmentModes(value: AlignmentModes) =
    this.props.add (PKey.dialog.buttonAlignmentModes, value)

  member this.canceled(value: bool) =
    this.props.add (PKey.dialog.canceled, value)

// FileDialog
type fileDialogProps() =
  inherit dialogProps()
  // Properties
  member this.allowedTypes(value: IAllowedType list) =
    this.props.add (PKey.fileDialog.allowedTypes, value)

  member this.allowsMultipleSelection(value: bool) =
    this.props.add (PKey.fileDialog.allowsMultipleSelection, value)

  member this.fileOperationsHandler(value: IFileOperations) =
    this.props.add (PKey.fileDialog.fileOperationsHandler, value)

  member this.mustExist(value: bool) =
    this.props.add (PKey.fileDialog.mustExist, value)

  member this.openMode(value: OpenMode) =
    this.props.add (PKey.fileDialog.openMode, value)

  member this.path(value: string) =
    this.props.add (PKey.fileDialog.path, value)

  member this.searchMatcher(value: ISearchMatcher) =
    this.props.add (PKey.fileDialog.searchMatcher, value)
  // Events
  member this.filesSelected(handler: FilesSelectedEventArgs -> unit) =
    this.props.add (PKey.fileDialog.filesSelected, handler)

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
  member this.axisX(value: HorizontalAxis) =
    this.props.add (PKey.graphView.axisX, value)

  member this.axisY(value: VerticalAxis) =
    this.props.add (PKey.graphView.axisY, value)

  member this.cellSize(value: PointF) =
    this.props.add (PKey.graphView.cellSize, value)

  member this.graphColor(value: Attribute option) =
    this.props.add (PKey.graphView.graphColor, value)

  member this.marginBottom(value: int) =
    this.props.add (PKey.graphView.marginBottom, value)

  member this.marginLeft(value: int) =
    this.props.add (PKey.graphView.marginLeft, value)

  member this.scrollOffset(value: PointF) =
    this.props.add (PKey.graphView.scrollOffset, value)

// HexView
type hexViewProps() =
  inherit viewProps()

  // Properties
  member this.address(value: Int64) =
    this.props.add (PKey.hexView.address, value)

  member this.addressWidth(value: int) =
    this.props.add (PKey.hexView.addressWidth, value)

  member this.allowEdits(value: int) =
    this.props.add (PKey.hexView.allowEdits, value)

  member this.readOnly(value: bool) =
    this.props.add (PKey.hexView.readOnly, value)

  member this.source(value: Stream) =
    this.props.add (PKey.hexView.source, value)
  // Events
  member this.edited(handler: HexViewEditEventArgs -> unit) =
    this.props.add (PKey.hexView.edited, handler)

  member this.positionChanged(handler: HexViewEventArgs -> unit) =
    this.props.add (PKey.hexView.positionChanged, handler)

// Label
type labelProps() =
  inherit viewProps()

  // Properties
  member this.hotKeySpecifier(value: Rune) =
    this.props.add (PKey.label.hotKeySpecifier, value)

  member this.text(value: string) = this.props.add (PKey.label.text, value)

// LegendAnnotation
type legendAnnotationProps() =
  inherit viewProps()
// No properties or events LegendAnnotation

// Line
type lineProps() =
  inherit viewProps()

  // Properties
  member this.orientation(value: Orientation) =
    this.props.add (PKey.line.orientation, value)
  // Events
  member this.orientationChanged(handler: Orientation -> unit) =
    this.props.add (PKey.line.orientationChanged, handler)

  member this.orientationChanging(handler: App.CancelEventArgs<Orientation> -> unit) =
    this.props.add (PKey.line.orientationChanging, handler)

// ListView
type listViewProps() =
  inherit viewProps()

  // Properties
  member this.allowsMarking(value: bool) =
    this.props.add (PKey.listView.allowsMarking, value)

  member this.allowsMultipleSelection(value: bool) =
    this.props.add (PKey.listView.allowsMultipleSelection, value)

  member this.leftItem(value: Int32) =
    this.props.add (PKey.listView.leftItem, value)

  member this.selectedItem(value: Int32) =
    this.props.add (PKey.listView.selectedItem, value)

  member this.source(value: string list) =
    this.props.add (PKey.listView.source, value)

  member this.topItem(value: Int32) =
    this.props.add (PKey.listView.topItem, value)
  // Events
  member this.collectionChanged(handler: NotifyCollectionChangedEventArgs -> unit) =
    this.props.add (PKey.listView.collectionChanged, handler)

  member this.openSelectedItem(handler: ListViewItemEventArgs -> unit) =
    this.props.add (PKey.listView.openSelectedItem, handler)

  member this.rowRender(handler: ListViewRowEventArgs -> unit) =
    this.props.add (PKey.listView.rowRender, handler)

  member this.selectedItemChanged(handler: ListViewItemEventArgs -> unit) =
    this.props.add (PKey.listView.selectedItemChanged, handler)

// Margin
type marginProps() =
  inherit adornmentProps()

  // Properties
  member this.shadowStyle(value: ShadowStyle) =
    this.props.add (PKey.margin.shadowStyle, value)

type menuProps() =
  inherit barProps()

  // Properties
  member this.selectedMenuItem(value: MenuItem) =
    this.props.add (PKey.menu.selectedMenuItem, value)

  member this.superMenuItem(value: MenuItem) =
    this.props.add (PKey.menu.superMenuItem, value)
  // Events
  member this.accepted(value: CommandEventArgs -> unit) =
    this.props.add (PKey.menu.accepted, value)

  member this.selectedMenuItemChanged(value: MenuItem -> unit) =
    this.props.add (PKey.menu.selectedMenuItemChanged, value)

// MenuBar
type menuBarProps() =
  inherit menuProps()

  // Properties
  member this.key(value: Key) =
    this.props.add (PKey.menuBar.key, value)

  member this.menus(value: IMenuBarItemElement list) =
    this.props.add (
      PKey.menuBar.children,
      List<_>(
        value
        |> Seq.map (fun v -> v :?> IInternalTerminalElement)
      )
    )
  // Events
  member this.keyChanged(handler: KeyChangedEventArgs -> unit) =
    this.props.add (PKey.menuBar.keyChanged, handler)

type shortcutProps() =
  inherit viewProps()

  // Properties
  member this.action(value: Action) =
    this.props.add (PKey.shortcut.action, value)

  member this.alignmentModes(value: AlignmentModes) =
    this.props.add (PKey.shortcut.alignmentModes, value)

  member this.commandView(value: ITerminalElement) =
    this.props.add (PKey.shortcut.commandView_element, value)

  member this.forceFocusColors(value: bool) =
    this.props.add (PKey.shortcut.forceFocusColors, value)

  member this.helpText(value: string) =
    this.props.add (PKey.shortcut.helpText, value)

  member this.text(value: string) =
    this.props.add (PKey.shortcut.text, value)

  member this.bindKeyToApplication(value: bool) =
    this.props.add (PKey.shortcut.bindKeyToApplication, value)

  member this.key(value: Key) =
    this.props.add (PKey.shortcut.key, value)

  member this.minimumKeyTextSize(value: Int32) =
    this.props.add (PKey.shortcut.minimumKeyTextSize, value)
  // Events
  member this.orientationChanged(handler: Orientation -> unit) =
    this.props.add (PKey.shortcut.orientationChanged, handler)

  member this.orientationChanging(handler: CancelEventArgs<Orientation> -> unit) =
    this.props.add (PKey.shortcut.orientationChanging, handler)

type menuItemProps() =
  inherit shortcutProps()

  member this.command(value: Command) =
    this.props.add (PKey.menuItem.command, value)

  member this.submenu(value: IMenuElement) =
    this.props.add (PKey.menuItem.subMenu_element, value)

  member this.accepted(value: CommandEventArgs -> unit) =
    this.props.add (PKey.menuItem.accepted, value)

type menuBarItemProps() =
  inherit menuItemProps()

  member this.popoverMenu(value: IPopoverMenuElement) =
    this.props.add (PKey.menuBarItem.popoverMenu_element, value)

  member this.popoverMenuOpen(value: bool) =
    this.props.add (PKey.menuBarItem.popoverMenuOpen, value)

type popoverMenuProps() =
  inherit viewProps()

  member this.key(value: Key) =
    this.props.add (PKey.popoverMenu.key, value)

  member this.root(value: IMenuElement) =
    this.props.add (PKey.popoverMenu.root_element, value)

// NumericUpDown`1
type numericUpDownProps<'a>() =
  inherit viewProps()

  // Properties
  member this.format(value: string) =
    this.props.add (PKey.numericUpDown.format, value)

  member this.increment(value: 'a) =
    this.props.add (PKey.numericUpDown.increment, value)

  member this.value(value: 'a) =
    this.props.add (PKey.numericUpDown.value, value)
  // Events
  member this.formatChanged(handler: string -> unit) =
    this.props.add (PKey.numericUpDown.formatChanged, handler)

  member this.incrementChanged(handler: 'a -> unit) =
    this.props.add (PKey.numericUpDown.incrementChanged, handler)

  member this.valueChanged(handler: 'a -> unit) =
    this.props.add (PKey.numericUpDown.valueChanged, handler)

  member this.valueChanging(handler: App.CancelEventArgs<'a> -> unit) =
    this.props.add (PKey.numericUpDown.valueChanging, handler)

// NumericUpDown
type numericUpDownProps() =
  inherit numericUpDownProps<int>()
// No properties or events NumericUpDown

// OpenDialog
type openDialogProps() =
  inherit fileDialogProps()
  // Properties
  member this.openMode(value: OpenMode) =
    this.props.add (PKey.openDialog.openMode, value)

// SelectorBase
type [<AbstractClass>] selectorBaseProps() =
  inherit viewProps()
  // IOrientation
  member this.orientation(value: Orientation) =
    this.props.add (PKey.orientationInterface.orientation, value)

  member this.orientationChanged(value: Orientation -> unit) =
    this.props.add (PKey.orientationInterface.orientationChanged, value)

  member this.orientationChanging(value: CancelEventArgs<Orientation> -> unit) =
    this.props.add (PKey.orientationInterface.orientationChanging, value)

  // Properties
  member this.assignHotKeys(value: bool) =
    this.props.add (PKey.optionSelector.assignHotKeys, value)

  member this.doubleClickAccepts(value: bool) =
    this.props.add (PKey.optionSelector.doubleClickAccepts, value)

  member this.horizontalSpace(value: int) =
    this.props.add (PKey.optionSelector.horizontalSpace, value)

  member this.labels(value: IReadOnlyList<string>) =
    this.props.add (PKey.optionSelector.labels, value)

  member this.styles(value: SelectorStyles) =
    this.props.add (PKey.optionSelector.styles, value)

  member this.usedHotKeys(value: HashSet<Key>) =
    this.props.add (PKey.optionSelector.usedHotKeys, value)

  member this.value(value: int option) =
    this.props.add (PKey.optionSelector.value, value |> Option.toNullable)

  member this.values(value: IReadOnlyList<int>) =
    this.props.add (PKey.optionSelector.values, value)

  // Events
  member this.valueChanged(value: EventArgs<Nullable<int>> -> unit) =
    this.props.add (PKey.optionSelector.valueChanged, value)

// OptionSelector
type optionSelectorProps() =
  inherit selectorBaseProps()
  // Properties
  member this.cursor(value: int) =
    this.props.add (PKey.optionSelector.cursor, value)

// FlagSelector
type flagSelectorProps() =
  inherit selectorBaseProps()
  // Properties
  member this.value(value: int) =
    this.props.add (PKey.flagSelector.value, value)

// Padding
type paddingProps() =
  inherit adornmentProps()

// ProgressBar
type progressBarProps() =
  inherit viewProps()

  // Properties
  member this.bidirectionalMarquee(value: bool) =
    this.props.add (PKey.progressBar.bidirectionalMarquee, value)

  member this.fraction(value: Single) =
    this.props.add (PKey.progressBar.fraction, value)

  member this.progressBarFormat(value: ProgressBarFormat) =
    this.props.add (PKey.progressBar.progressBarFormat, value)

  member this.progressBarStyle(value: ProgressBarStyle) =
    this.props.add (PKey.progressBar.progressBarStyle, value)

  member this.segmentCharacter(value: Rune) =
    this.props.add (PKey.progressBar.segmentCharacter, value)

  member this.text(value: string) =
    this.props.add (PKey.progressBar.text, value)


// SaveDialog
// No properties or events SaveDialog

// ScrollBar
type scrollBarProps() =
  inherit viewProps()

  // Properties
  member this.autoShow(value: bool) =
    this.props.add (PKey.scrollBar.autoShow, value)

  member this.increment(value: Int32) =
    this.props.add (PKey.scrollBar.increment, value)

  member this.orientation(value: Orientation) =
    this.props.add (PKey.scrollBar.orientation, value)

  member this.position(value: Int32) =
    this.props.add (PKey.scrollBar.position, value)

  member this.scrollableContentSize(value: Int32) =
    this.props.add (PKey.scrollBar.scrollableContentSize, value)

  member this.visibleContentSize(value: Int32) =
    this.props.add (PKey.scrollBar.visibleContentSize, value)
  // Events
  member this.orientationChanged(handler: Orientation -> unit) =
    this.props.add (PKey.scrollBar.orientationChanged, handler)

  member this.orientationChanging(handler: CancelEventArgs<Orientation> -> unit) =
    this.props.add (PKey.scrollBar.orientationChanging, handler)

  member this.scrollableContentSizeChanged(handler: EventArgs<Int32> -> unit) =
    this.props.add (PKey.scrollBar.scrollableContentSizeChanged, handler)

  member this.sliderPositionChanged(handler: EventArgs<Int32> -> unit) =
    this.props.add (PKey.scrollBar.sliderPositionChanged, handler)

// ScrollSlider
type scrollSliderProps() =
  inherit viewProps()

  // Properties
  member this.orientation(value: Orientation) =
    this.props.add (PKey.scrollSlider.orientation, value)

  member this.position(value: Int32) =
    this.props.add (PKey.scrollSlider.position, value)

  member this.size(value: Int32) =
    this.props.add (PKey.scrollSlider.size, value)

  member this.sliderPadding(value: Int32) =
    this.props.add (PKey.scrollSlider.sliderPadding, value)

  member this.visibleContentSize(value: Int32) =
    this.props.add (PKey.scrollSlider.visibleContentSize, value)
  // Events
  member this.orientationChanged(handler: Orientation -> unit) =
    this.props.add (PKey.scrollSlider.orientationChanged, handler)

  member this.orientationChanging(handler: CancelEventArgs<Orientation> -> unit) =
    this.props.add (PKey.scrollSlider.orientationChanging, handler)

  member this.positionChanged(handler: EventArgs<Int32> -> unit) =
    this.props.add (PKey.scrollSlider.positionChanged, handler)

  member this.positionChanging(handler: CancelEventArgs<Int32> -> unit) =
    this.props.add (PKey.scrollSlider.positionChanging, handler)

  member this.scrolled(handler: EventArgs<Int32> -> unit) =
    this.props.add (PKey.scrollSlider.scrolled, handler)

// Slider`1
type sliderProps<'a>() =
  inherit viewProps()

  // Properties
  member this.allowEmpty(value: bool) =
    this.props.add (PKey.slider.allowEmpty, value)

  member this.focusedOption(value: Int32) =
    this.props.add (PKey.slider.focusedOption, value)

  member this.legendsOrientation(value: Orientation) =
    this.props.add (PKey.slider.legendsOrientation, value)

  member this.minimumInnerSpacing(value: Int32) =
    this.props.add (PKey.slider.minimumInnerSpacing, value)

  member this.options(value: SliderOption<'a> list) =
    this.props.add (PKey.slider.options, value)

  member this.orientation(value: Orientation) =
    this.props.add (PKey.slider.orientation, value)

  member this.rangeAllowSingle(value: bool) =
    this.props.add (PKey.slider.rangeAllowSingle, value)

  member this.showEndSpacing(value: bool) =
    this.props.add (PKey.slider.showEndSpacing, value)

  member this.showLegends(value: bool) =
    this.props.add (PKey.slider.showLegends, value)

  member this.style(value: SliderStyle) =
    this.props.add (PKey.slider.style, value)

  member this.text(value: string) =
    this.props.add (PKey.slider.text, value)

  member this.``type``(value: SliderType) =
    this.props.add (PKey.slider.``type``, value)

  member this.useMinimumSize(value: bool) =
    this.props.add (PKey.slider.useMinimumSize, value)
  // Events
  member this.optionFocused(handler: SliderEventArgs<'a> -> unit) =
    this.props.add (PKey.slider.optionFocused, handler)

  member this.optionsChanged(handler: SliderEventArgs<'a> -> unit) =
    this.props.add (PKey.slider.optionsChanged, handler)

  member this.orientationChanged(handler: Orientation -> unit) =
    this.props.add (PKey.slider.orientationChanged, handler)

  member this.orientationChanging(handler: App.CancelEventArgs<Orientation> -> unit) =
    this.props.add (PKey.slider.orientationChanging, handler)

// Slider
type sliderProps() =
  inherit sliderProps<obj>()
// No properties or events Slider

// SpinnerView
type spinnerViewProps() =
  inherit viewProps()

  // Properties
  member this.autoSpin(value: bool) =
    this.props.add (PKey.spinnerView.autoSpin, value)

  member this.sequence(value: string list) =
    this.props.add (PKey.spinnerView.sequence, value)

  member this.spinBounce(value: bool) =
    this.props.add (PKey.spinnerView.spinBounce, value)

  member this.spinDelay(value: Int32) =
    this.props.add (PKey.spinnerView.spinDelay, value)

  member this.spinReverse(value: bool) =
    this.props.add (PKey.spinnerView.spinReverse, value)

  member this.style(value: SpinnerStyle) =
    this.props.add (PKey.spinnerView.style, value)

// StatusBar
type statusBarProps() =
  inherit barProps()
// No properties or events StatusBar

// Tab
type tabProps() =
  inherit viewProps()

  // Properties
  member this.displayText(value: string) =
    this.props.add (PKey.tab.displayText, value)

  member this.view(value: ITerminalElement) =
    this.props.add (PKey.tab.view_element, value)

// TabView
type tabViewProps() =
  inherit viewProps()

  // Properties
  member this.maxTabTextWidth(value: int) =
    this.props.add (PKey.tabView.maxTabTextWidth, value)

  member this.selectedTab(value: Tab) =
    this.props.add (PKey.tabView.selectedTab, value)

  member this.style(value: TabStyle) =
    this.props.add (PKey.tabView.style, value)

  member this.tabScrollOffset(value: Int32) =
    this.props.add (PKey.tabView.tabScrollOffset, value)
  // Events
  member this.selectedTabChanged(handler: TabChangedEventArgs -> unit) =
    this.props.add (PKey.tabView.selectedTabChanged, handler)

  member this.tabClicked(handler: TabMouseEventArgs -> unit) =
    this.props.add (PKey.tabView.tabClicked, handler)

  member this.tabs(value: ITerminalElement list) =
    this.props.add (PKey.tabView.tabs, System.Collections.Generic.List<_>(value))

// TableView
type tableViewProps() =
  inherit viewProps()

  // Properties
  member this.cellActivationKey(value: KeyCode) =
    this.props.add (PKey.tableView.cellActivationKey, value)

  member this.collectionNavigator(value: ICollectionNavigator) =
    this.props.add (PKey.tableView.collectionNavigator, value)

  member this.columnOffset(value: Int32) =
    this.props.add (PKey.tableView.columnOffset, value)

  member this.fullRowSelect(value: bool) =
    this.props.add (PKey.tableView.fullRowSelect, value)

  member this.maxCellWidth(value: Int32) =
    this.props.add (PKey.tableView.maxCellWidth, value)

  member this.minCellWidth(value: Int32) =
    this.props.add (PKey.tableView.minCellWidth, value)

  member this.multiSelect(value: bool) =
    this.props.add (PKey.tableView.multiSelect, value)

  member this.nullSymbol(value: string) =
    this.props.add (PKey.tableView.nullSymbol, value)

  member this.rowOffset(value: Int32) =
    this.props.add (PKey.tableView.rowOffset, value)

  member this.selectedColumn(value: Int32) =
    this.props.add (PKey.tableView.selectedColumn, value)

  member this.selectedRow(value: Int32) =
    this.props.add (PKey.tableView.selectedRow, value)

  member this.separatorSymbol(value: Char) =
    this.props.add (PKey.tableView.separatorSymbol, value)

  member this.style(value: TableStyle) =
    this.props.add (PKey.tableView.style, value)

  member this.table(value: ITableSource) =
    this.props.add (PKey.tableView.table, value)
  // Events
  member this.cellActivated(handler: CellActivatedEventArgs -> unit) =
    this.props.add (PKey.tableView.cellActivated, handler)

  member this.cellToggled(handler: CellToggledEventArgs -> unit) =
    this.props.add (PKey.tableView.cellToggled, handler)

  member this.selectedCellChanged(handler: SelectedCellChangedEventArgs -> unit) =
    this.props.add (PKey.tableView.selectedCellChanged, handler)

// TextValidateField
type textValidateFieldProps() =
  inherit viewProps()

  // Properties
  member this.provider(value: ITextValidateProvider) =
    this.props.add (PKey.textValidateField.provider, value)

  member this.text(value: string) =
    this.props.add (PKey.textValidateField.text, value)

// TextView
type textViewProps() =
  inherit viewProps()

  // Properties
  member this.allowsReturn(value: bool) =
    this.props.add (PKey.textView.allowsReturn, value)

  member this.allowsTab(value: bool) =
    this.props.add (PKey.textView.allowsTab, value)

  member this.cursorPosition(value: Point) =
    this.props.add (PKey.textView.cursorPosition, value)

  member this.inheritsPreviousAttribute(value: bool) =
    this.props.add (PKey.textView.inheritsPreviousAttribute, value)

  member this.isDirty(value: bool) =
    this.props.add (PKey.textView.isDirty, value)

  member this.isSelecting(value: bool) =
    this.props.add (PKey.textView.isSelecting, value)

  member this.leftColumn(value: Int32) =
    this.props.add (PKey.textView.leftColumn, value)

  member this.multiline(value: bool) =
    this.props.add (PKey.textView.multiline, value)

  member this.readOnly(value: bool) =
    this.props.add (PKey.textView.readOnly, value)

  member this.selectionStartColumn(value: Int32) =
    this.props.add (PKey.textView.selectionStartColumn, value)

  member this.selectionStartRow(value: Int32) =
    this.props.add (PKey.textView.selectionStartRow, value)

  member this.selectWordOnlyOnDoubleClick(value: bool) =
    this.props.add (PKey.textView.selectWordOnlyOnDoubleClick, value)

  member this.tabWidth(value: Int32) =
    this.props.add (PKey.textView.tabWidth, value)

  member this.text(value: string) =
    this.props.add (PKey.textView.text, value)

  member this.topRow(value: Int32) =
    this.props.add (PKey.textView.topRow, value)

  member this.used(value: bool) =
    this.props.add (PKey.textView.used, value)

  member this.useSameRuneTypeForWords(value: bool) =
    this.props.add (PKey.textView.useSameRuneTypeForWords, value)

  member this.wordWrap(value: bool) =
    this.props.add (PKey.textView.wordWrap, value)
  // Events
  member this.contentsChanged(handler: ContentsChangedEventArgs -> unit) =
    this.props.add (PKey.textView.contentsChanged, handler)

  member this.drawNormalColor(handler: CellEventArgs -> unit) =
    this.props.add (PKey.textView.drawNormalColor, handler)

  member this.drawReadOnlyColor(handler: CellEventArgs -> unit) =
    this.props.add (PKey.textView.drawReadOnlyColor, handler)

  member this.drawSelectionColor(handler: CellEventArgs -> unit) =
    this.props.add (PKey.textView.drawSelectionColor, handler)

  member this.drawUsedColor(handler: CellEventArgs -> unit) =
    this.props.add (PKey.textView.drawUsedColor, handler)

  member this.unwrappedCursorPosition(handler: Point -> unit) =
    this.props.add (PKey.textView.unwrappedCursorPosition, handler)
  // Additional properties
  member this.textChanged(value: string -> unit) =
    this.props.add (PKey.textView.textChanged, value)


// TimeField
type timeFieldProps() =
  inherit textFieldProps()

  // Properties
  member this.cursorPosition(value: Int32) =
    this.props.add (PKey.timeField.cursorPosition, value)

  member this.isShortFormat(value: bool) =
    this.props.add (PKey.timeField.isShortFormat, value)

  member this.time(value: TimeSpan) =
    this.props.add (PKey.timeField.time, value)
  // Events
  member this.timeChanged(handler: EventArgs<TimeSpan> -> unit) =
    this.props.add (PKey.timeField.timeChanged, handler)

// TreeView`1
type treeViewProps<'a when 'a: not struct>() =
  inherit viewProps()

  // Properties
  member this.allowLetterBasedNavigation(value: bool) =
    this.props.add (PKey.treeView.allowLetterBasedNavigation, value)

  member this.aspectGetter<'a when 'a: not struct>(value: AspectGetterDelegate<'a>) =
    this.props.add (PKey.treeView.aspectGetter, value)

  member this.colorGetter<'a when 'a: not struct>(value: Func<'a, Scheme>) =
    this.props.add (PKey.treeView.colorGetter, value)

  member this.maxDepth(value: Int32) =
    this.props.add (PKey.treeView.maxDepth, value)

  member this.multiSelect(value: bool) =
    this.props.add (PKey.treeView.multiSelect, value)

  member this.objectActivationButton(value: MouseFlags option) =
    this.props.add (PKey.treeView.objectActivationButton, value)

  member this.objectActivationKey(value: KeyCode) =
    this.props.add (PKey.treeView.objectActivationKey, value)

  member this.scrollOffsetHorizontal(value: Int32) =
    this.props.add (PKey.treeView.scrollOffsetHorizontal, value)

  member this.scrollOffsetVertical(value: Int32) =
    this.props.add (PKey.treeView.scrollOffsetVertical, value)

  member this.selectedObject<'a when 'a: not struct>(value: 'a) =
    this.props.add (PKey.treeView.selectedObject, value)

  member this.style(value: TreeStyle) =
    this.props.add (PKey.treeView.style, value)

  member this.treeBuilder<'a when 'a: not struct>(value: ITreeBuilder<'a>) =
    this.props.add (PKey.treeView.treeBuilder, value)
  // Events
  member this.drawLine<'a when 'a: not struct>(handler: DrawTreeViewLineEventArgs<'a> -> unit) =
    this.props.add (PKey.treeView.drawLine, handler)

  member this.objectActivated<'a when 'a: not struct>(handler: ObjectActivatedEventArgs<'a> -> unit) =
    this.props.add (PKey.treeView.objectActivated, handler)

  member this.selectionChanged<'a when 'a: not struct>(handler: SelectionChangedEventArgs<'a> -> unit) =
    this.props.add (PKey.treeView.selectionChanged, handler)

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
  member this.currentStep(value: WizardStep) =
    this.props.add (PKey.wizard.currentStep, value)

  member this.modal(value: bool) =
    this.props.add (PKey.wizard.modal, value)
  // Events
  member this.cancelled(handler: WizardButtonEventArgs -> unit) =
    this.props.add (PKey.wizard.cancelled, handler)

  member this.finished(handler: WizardButtonEventArgs -> unit) =
    this.props.add (PKey.wizard.finished, handler)

  member this.movingBack(handler: WizardButtonEventArgs -> unit) =
    this.props.add (PKey.wizard.movingBack, handler)

  member this.movingNext(handler: WizardButtonEventArgs -> unit) =
    this.props.add (PKey.wizard.movingNext, handler)

  member this.stepChanged(handler: StepChangeEventArgs -> unit) =
    this.props.add (PKey.wizard.stepChanged, handler)

  member this.stepChanging(handler: StepChangeEventArgs -> unit) =
    this.props.add (PKey.wizard.stepChanging, handler)

// WizardStep
type wizardStepProps() =
  inherit viewProps()

  // Properties
  member this.backButtonText(value: string) =
    this.props.add (PKey.wizardStep.backButtonText, value)

  member this.helpText(value: string) =
    this.props.add (PKey.wizardStep.helpText, value)

  member this.nextButtonText(value: string) =
    this.props.add (PKey.wizardStep.nextButtonText, value)
