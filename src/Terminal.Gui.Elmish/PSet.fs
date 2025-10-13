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
open Terminal.Gui
open Terminal.Gui.Elmish.Elements

open Terminal.Gui.FileServices
open Terminal.Gui.Input

open Terminal.Gui.Text

open Terminal.Gui.ViewBase

open Terminal.Gui.Views


module PSet =

    type viewPSetter() =
        member inline this.children (children:System.Collections.Generic.List<TerminalElement>) (props:IncrementalProps) = props.add PName.view.children children
        member inline this.ref (reference:View->unit) (props:IncrementalProps) = props.add PName.view.ref reference

        // Properties
        member inline this.arrangement (value:ViewArrangement) (props:IncrementalProps) = props.add PName.view.arrangement value
        member inline this.borderStyle (value:LineStyle) (props:IncrementalProps) = props.add PName.view.borderStyle value
        member inline this.canFocus (value:bool) (props:IncrementalProps) = props.add PName.view.canFocus value
        member inline this.contentSizeTracksViewport (value:bool) (props:IncrementalProps) = props.add PName.view.contentSizeTracksViewport value
        member inline this.cursorVisibility (value:CursorVisibility) (props:IncrementalProps) = props.add PName.view.cursorVisibility value
        member inline this.data (value:Object) (props:IncrementalProps) = props.add PName.view.data value
        member inline this.enabled (value:bool) (props:IncrementalProps) = props.add PName.view.enabled value
        member inline this.frame (value:Rectangle) (props:IncrementalProps) = props.add PName.view.frame value
        member inline this.hasFocus (value:bool) (props:IncrementalProps) = props.add PName.view.hasFocus value
        member inline this.height (value:Dim) (props:IncrementalProps) = props.add PName.view.height value
        member inline this.highlightStates (value:MouseState) (props:IncrementalProps) = props.add PName.view.highlightStates value
        member inline this.hotKey (value:Key) (props:IncrementalProps) = props.add PName.view.hotKey value
        member inline this.hotKeySpecifier (value:Rune) (props:IncrementalProps) = props.add PName.view.hotKeySpecifier value
        member inline this.id (value:string) (props:IncrementalProps) = props.add PName.view.id value
        member inline this.isInitialized (value:bool) (props:IncrementalProps) = props.add PName.view.isInitialized value
        member inline this.mouseHeldDown (value:IMouseHeldDown) (props:IncrementalProps) = props.add PName.view.mouseHeldDown value
        member inline this.needsDraw (value:bool) (props:IncrementalProps) = props.add PName.view.needsDraw value
        member inline this.preserveTrailingSpaces (value:bool) (props:IncrementalProps) = props.add PName.view.preserveTrailingSpaces value
        member inline this.schemeName (value:string) (props:IncrementalProps) = props.add PName.view.schemeName value
        member inline this.shadowStyle (value:ShadowStyle) (props:IncrementalProps) = props.add PName.view.shadowStyle value
        member inline this.superViewRendersLineCanvas (value:bool) (props:IncrementalProps) = props.add PName.view.superViewRendersLineCanvas value
        member inline this.tabStop (value:TabBehavior option) (props:IncrementalProps) = props.add PName.view.tabStop value
        member inline this.text (value:string) (props:IncrementalProps) = props.add PName.view.text value
        member inline this.textAlignment (value:Alignment) (props:IncrementalProps) = props.add PName.view.textAlignment value
        member inline this.textDirection (value:TextDirection) (props:IncrementalProps) = props.add PName.view.textDirection value
        member inline this.title (value:string) (props:IncrementalProps) = props.add PName.view.title value
        member inline this.validatePosDim (value:bool) (props:IncrementalProps) = props.add PName.view.validatePosDim value
        member inline this.verticalTextAlignment (value:Alignment) (props:IncrementalProps) = props.add PName.view.verticalTextAlignment value
        member inline this.viewport (value:Rectangle) (props:IncrementalProps) = props.add PName.view.viewport value
        member inline this.viewportSettings (value:ViewportSettingsFlags) (props:IncrementalProps) = props.add PName.view.viewportSettings value
        member inline this.visible (value:bool) (props:IncrementalProps) = props.add PName.view.visible value
        member inline this.wantContinuousButtonPressed (value:bool) (props:IncrementalProps) = props.add PName.view.wantContinuousButtonPressed value
        member inline this.wantMousePositionReports (value:bool) (props:IncrementalProps) = props.add PName.view.wantMousePositionReports value
        member inline this.width (value:Dim) (props:IncrementalProps) = props.add PName.view.width value
        member inline this.x (value:Pos) (props:IncrementalProps) = props.add PName.view.x value
        member inline this.y (value:Pos) (props:IncrementalProps) = props.add PName.view.y value
        // Events
        member inline this.accepting (handler:HandledEventArgs->unit) (props:IncrementalProps) = props.add PName.view.accepting handler
        member inline this.advancingFocus (handler:AdvanceFocusEventArgs->unit) (props:IncrementalProps) = props.add PName.view.advancingFocus handler
        member inline this.borderStyleChanged (handler:EventArgs->unit) (props:IncrementalProps) = props.add PName.view.borderStyleChanged handler
        member inline this.canFocusChanged (handler:unit->unit) (props:IncrementalProps) = props.add PName.view.canFocusChanged handler
        member inline this.clearedViewport (handler:DrawEventArgs->unit) (props:IncrementalProps) = props.add PName.view.clearedViewport handler
        member inline this.clearingViewport (handler:DrawEventArgs->unit) (props:IncrementalProps) = props.add PName.view.clearingViewport handler
        member inline this.commandNotBound (handler:CommandEventArgs->unit) (props:IncrementalProps) = props.add PName.view.commandNotBound handler
        member inline this.contentSizeChanged (handler:SizeChangedEventArgs->unit) (props:IncrementalProps) = props.add PName.view.contentSizeChanged handler
        member inline this.disposing (handler:unit->unit) (props:IncrementalProps) = props.add PName.view.disposing handler
        member inline this.drawComplete (handler:DrawEventArgs->unit) (props:IncrementalProps) = props.add PName.view.drawComplete handler
        member inline this.drawingContent (handler:DrawEventArgs->unit) (props:IncrementalProps) = props.add PName.view.drawingContent handler
        member inline this.drawingSubViews (handler:DrawEventArgs->unit) (props:IncrementalProps) = props.add PName.view.drawingSubViews handler
        member inline this.drawingText (handler:DrawEventArgs->unit) (props:IncrementalProps) = props.add PName.view.drawingText handler
        member inline this.enabledChanged (handler:unit->unit) (props:IncrementalProps) = props.add PName.view.enabledChanged handler
        member inline this.focusedChanged (handler:HasFocusEventArgs->unit) (props:IncrementalProps) = props.add PName.view.focusedChanged handler
        member inline this.frameChanged (handler:EventArgs<Rectangle>->unit) (props:IncrementalProps) = props.add PName.view.frameChanged handler
        member inline this.gettingAttributeForRole (handler:VisualRoleEventArgs->unit) (props:IncrementalProps) = props.add PName.view.gettingAttributeForRole handler
        member inline this.gettingScheme (handler:ResultEventArgs<Scheme>->unit) (props:IncrementalProps) = props.add PName.view.gettingScheme handler
        member inline this.handlingHotKey (handler:CommandEventArgs->unit) (props:IncrementalProps) = props.add PName.view.handlingHotKey handler
        member inline this.hasFocusChanged (handler:HasFocusEventArgs->unit) (props:IncrementalProps) = props.add PName.view.hasFocusChanged handler
        member inline this.hasFocusChanging (handler:HasFocusEventArgs->unit) (props:IncrementalProps) = props.add PName.view.hasFocusChanging handler
        member inline this.hotKeyChanged (handler:KeyChangedEventArgs->unit) (props:IncrementalProps) = props.add PName.view.hotKeyChanged handler
        member inline this.initialized (handler:unit->unit) (props:IncrementalProps) = props.add PName.view.initialized handler
        member inline this.keyDown (handler:Key->unit) (props:IncrementalProps) = props.add PName.view.keyDown handler
        member inline this.keyDownNotHandled (handler:Key->unit) (props:IncrementalProps) = props.add PName.view.keyDownNotHandled handler
        member inline this.keyUp (handler:Key->unit) (props:IncrementalProps) = props.add PName.view.keyUp handler
        member inline this.mouseClick (handler:MouseEventArgs->unit) (props:IncrementalProps) = props.add PName.view.mouseClick handler
        member inline this.mouseEnter (handler:CancelEventArgs->unit) (props:IncrementalProps) = props.add PName.view.mouseEnter handler
        member inline this.mouseEvent (handler:MouseEventArgs->unit) (props:IncrementalProps) = props.add PName.view.mouseEvent handler
        member inline this.mouseLeave (handler:EventArgs->unit) (props:IncrementalProps) = props.add PName.view.mouseLeave handler
        member inline this.mouseStateChanged (handler:EventArgs->unit) (props:IncrementalProps) = props.add PName.view.mouseStateChanged handler
        member inline this.mouseWheel (handler:MouseEventArgs->unit) (props:IncrementalProps) = props.add PName.view.mouseWheel handler
        member inline this.removed (handler:SuperViewChangedEventArgs->unit) (props:IncrementalProps) = props.add PName.view.removed handler
        member inline this.schemeChanged (handler:ValueChangedEventArgs<Scheme>->unit) (props:IncrementalProps) = props.add PName.view.schemeChanged handler
        member inline this.schemeChanging (handler:ValueChangingEventArgs<Scheme>->unit) (props:IncrementalProps) = props.add PName.view.schemeChanging handler
        member inline this.schemeNameChanged (handler:ValueChangedEventArgs<string>->unit) (props:IncrementalProps) = props.add PName.view.schemeNameChanged handler
        member inline this.schemeNameChanging (handler:ValueChangingEventArgs<string>->unit) (props:IncrementalProps) = props.add PName.view.schemeNameChanging handler
        member inline this.selecting (handler:CommandEventArgs->unit) (props:IncrementalProps) = props.add PName.view.selecting handler
        member inline this.subViewAdded (handler:SuperViewChangedEventArgs->unit) (props:IncrementalProps) = props.add PName.view.subViewAdded handler
        member inline this.subViewLayout (handler:LayoutEventArgs->unit) (props:IncrementalProps) = props.add PName.view.subViewLayout handler
        member inline this.subViewRemoved (handler:SuperViewChangedEventArgs->unit) (props:IncrementalProps) = props.add PName.view.subViewRemoved handler
        member inline this.subViewsLaidOut (handler:LayoutEventArgs->unit) (props:IncrementalProps) = props.add PName.view.subViewsLaidOut handler
        member inline this.superViewChanged (handler:SuperViewChangedEventArgs->unit) (props:IncrementalProps) = props.add PName.view.superViewChanged handler
        member inline this.textChanged (handler:unit->unit) (props:IncrementalProps) = props.add PName.view.textChanged handler
        member inline this.titleChanged (handler:string->unit) (props:IncrementalProps) = props.add PName.view.titleChanged handler
        member inline this.titleChanging (handler:App.CancelEventArgs<string>->unit) (props:IncrementalProps) = props.add PName.view.titleChanging handler
        member inline this.viewportChanged (handler:DrawEventArgs->unit) (props:IncrementalProps) = props.add PName.view.viewportChanged handler
        member inline this.visibleChanged (handler:unit->unit) (props:IncrementalProps) = props.add PName.view.visibleChanged handler
        member inline this.visibleChanging (handler:unit->unit) (props:IncrementalProps) = props.add PName.view.visibleChanging handler

    // Adornment
    type adornmentPSetter() =
        inherit viewPSetter()
        // Properties
        member inline this.diagnostics (value:ViewDiagnosticFlags) (props:IncrementalProps) = props.add PName.adornment.diagnostics value
        member inline this.superViewRendersLineCanvas (value:bool) (props:IncrementalProps) = props.add PName.adornment.superViewRendersLineCanvas value
        member inline this.thickness (value:Thickness) (props:IncrementalProps) = props.add PName.adornment.thickness value
        member inline this.viewport (value:Rectangle) (props:IncrementalProps) = props.add PName.adornment.viewport value
        // Events
        member inline this.thicknessChanged (handler:unit->unit) (props:IncrementalProps) = props.add PName.adornment.thicknessChanged handler

    // Bar
    type barPSetter() =
        inherit viewPSetter()
        // Properties
        member inline this.alignmentModes (value:AlignmentModes) (props:IncrementalProps) = props.add PName.bar.alignmentModes value
        member inline this.orientation (value:Orientation) (props:IncrementalProps) = props.add PName.bar.orientation value
        // Events
        member inline this.orientationChanged (handler:Orientation->unit) (props:IncrementalProps) = props.add PName.bar.orientationChanged handler
        member inline this.orientationChanging (handler:App.CancelEventArgs<Orientation>->unit) (props:IncrementalProps) = props.add PName.bar.orientationChanging handler

    // Border
    type borderPSetter() =
        inherit adornmentPSetter()
        // Properties
        member inline this.lineStyle (value:LineStyle) (props:IncrementalProps) = props.add PName.border.lineStyle value
        member inline this.settings (value:BorderSettings) (props:IncrementalProps) = props.add PName.border.settings value

    // Button
    type buttonPSetter() =
        inherit viewPSetter()
        // Properties
        member inline this.hotKeySpecifier (value:Rune) (props:IncrementalProps) = props.add PName.button.hotKeySpecifier value
        member inline this.isDefault (value:bool) (props:IncrementalProps) = props.add PName.button.isDefault value
        member inline this.noDecorations (value:bool) (props:IncrementalProps) = props.add PName.button.noDecorations value
        member inline this.noPadding (value:bool) (props:IncrementalProps) = props.add PName.button.noPadding value
        member inline this.text (value:string) (props:IncrementalProps) = props.add PName.button.text value
        member inline this.wantContinuousButtonPressed (value:bool) (props:IncrementalProps) = props.add PName.button.wantContinuousButtonPressed value

    // CheckBox
    type checkBoxPSetter() =
        inherit viewPSetter()
        // Properties
        member inline this.allowCheckStateNone (value:bool) (props:IncrementalProps) = props.add PName.checkBox.allowCheckStateNone value
        member inline this.checkedState (value:CheckState) (props:IncrementalProps) = props.add PName.checkBox.checkedState value
        member inline this.hotKeySpecifier (value:Rune) (props:IncrementalProps) = props.add PName.checkBox.hotKeySpecifier value
        member inline this.radioStyle (value:bool) (props:IncrementalProps) = props.add PName.checkBox.radioStyle value
        member inline this.text (value:string) (props:IncrementalProps) = props.add PName.checkBox.text value
        // Events
        member inline this.checkedStateChanging (handler:ResultEventArgs<CheckState>->unit) (props:IncrementalProps) = props.add PName.checkBox.checkedStateChanging handler
        member inline this.checkedStateChanged (handler:EventArgs<CheckState>->unit) (props:IncrementalProps) = props.add PName.checkBox.checkedStateChanged handler

    // ColorPicker
    type colorPickerPSetter() =
        inherit viewPSetter()
        // Properties
        member inline this.selectedColor (value:Color) (props:IncrementalProps) = props.add PName.colorPicker.selectedColor value
        member inline this.style (value:ColorPickerStyle) (props:IncrementalProps) = props.add PName.colorPicker.style value
        // Events
        member inline this.colorChanged (handler:ResultEventArgs<Color>->unit) (props:IncrementalProps) = props.add PName.colorPicker.colorChanged handler

    // ColorPicker16
    type colorPicker16PSetter() =
        inherit viewPSetter()
        // Properties
        member inline this.boxHeight (value:Int32) (props:IncrementalProps) = props.add PName.colorPicker16.boxHeight value
        member inline this.boxWidth (value:Int32) (props:IncrementalProps) = props.add PName.colorPicker16.boxWidth value
        member inline this.cursor (value:Point) (props:IncrementalProps) = props.add PName.colorPicker16.cursor value
        member inline this.selectedColor (value:ColorName16) (props:IncrementalProps) = props.add PName.colorPicker16.selectedColor value
        // Events
        member inline this.colorChanged (handler:ResultEventArgs<Color>->unit) (props:IncrementalProps) = props.add PName.colorPicker16.colorChanged handler

    // ComboBox
    type comboBoxPSetter() =
        inherit viewPSetter()
        // Properties
        member inline this.hideDropdownListOnClick (value:bool) (props:IncrementalProps) = props.add PName.comboBox.hideDropdownListOnClick value
        member inline this.readOnly (value:bool) (props:IncrementalProps) = props.add PName.comboBox.readOnly value
        member inline this.searchText (value:string) (props:IncrementalProps) = props.add PName.comboBox.searchText value
        member inline this.selectedItem (value:Int32) (props:IncrementalProps) = props.add PName.comboBox.selectedItem value
        member inline this.source (value:string list) (props:IncrementalProps) = props.add PName.comboBox.source value
        member inline this.text (value:string) (props:IncrementalProps) = props.add PName.comboBox.text value
        // Events
        member inline this.collapsed (handler:unit->unit) (props:IncrementalProps) = props.add PName.comboBox.collapsed handler
        member inline this.expanded (handler:unit->unit) (props:IncrementalProps) = props.add PName.comboBox.expanded handler
        member inline this.openSelectedItem (handler:ListViewItemEventArgs->unit) (props:IncrementalProps) = props.add PName.comboBox.openSelectedItem handler
        member inline this.selectedItemChanged (handler:ListViewItemEventArgs->unit) (props:IncrementalProps) = props.add PName.comboBox.selectedItemChanged handler

    // TextField
    type textFieldPSetter() =
        inherit viewPSetter()
        // Properties
        member inline this.autocomplete (value:IAutocomplete) (props:IncrementalProps) = props.add PName.textField.autocomplete value
        member inline this.caption (value:string) (props:IncrementalProps) = props.add PName.textField.caption value
        member inline this.captionColor (value:Terminal.Gui.Drawing.Color) (props:IncrementalProps) = props.add PName.textField.captionColor value
        member inline this.cursorPosition (value:Int32) (props:IncrementalProps) = props.add PName.textField.cursorPosition value
        member inline this.readOnly (value:bool) (props:IncrementalProps) = props.add PName.textField.readOnly value
        member inline this.secret (value:bool) (props:IncrementalProps) = props.add PName.textField.secret value
        member inline this.selectedStart (value:Int32) (props:IncrementalProps) = props.add PName.textField.selectedStart value
        member inline this.selectWordOnlyOnDoubleClick (value:bool) (props:IncrementalProps) = props.add PName.textField.selectWordOnlyOnDoubleClick value
        member inline this.text (value:string) (props:IncrementalProps) = props.add PName.textField.text value
        member inline this.used (value:bool) (props:IncrementalProps) = props.add PName.textField.used value
        member inline this.useSameRuneTypeForWords (value:bool) (props:IncrementalProps) = props.add PName.textField.useSameRuneTypeForWords value
        // Events
        member inline this.textChanging (handler:ResultEventArgs<string>->unit) (props:IncrementalProps) = props.add PName.textField.textChanging handler

    // DateField
    type dateFieldPSetter() =
        inherit textFieldPSetter()
        // Properties
        member inline this.culture (value:CultureInfo) (props:IncrementalProps) = props.add PName.dateField.culture value
        member inline this.cursorPosition (value:Int32) (props:IncrementalProps) = props.add PName.dateField.cursorPosition value
        member inline this.date (value:DateTime) (props:IncrementalProps) = props.add PName.dateField.date value
        // Events
        member inline this.dateChanged (handler:DateTimeEventArgs<DateTime>->unit) (props:IncrementalProps) = props.add PName.dateField.dateChanged handler

    // DatePicker
    type datePickerPSetter() =
        inherit viewPSetter()
        // Properties
        member inline this.culture (value:CultureInfo) (props:IncrementalProps) = props.add PName.datePicker.culture value
        member inline this.date (value:DateTime) (props:IncrementalProps) = props.add PName.datePicker.date value

    // Toplevel
    type toplevelPSetter() =
        inherit viewPSetter()
        // Properties
        member inline this.modal (value:bool) (props:IncrementalProps) = props.add PName.toplevel.modal value
        member inline this.running (value:bool) (props:IncrementalProps) = props.add PName.toplevel.running value
        // Events
        member inline this.activate (handler:ToplevelEventArgs->unit) (props:IncrementalProps) = props.add PName.toplevel.activate handler
        member inline this.closed (handler:ToplevelEventArgs->unit) (props:IncrementalProps) = props.add PName.toplevel.closed handler
        member inline this.closing (handler:ToplevelClosingEventArgs->unit) (props:IncrementalProps) = props.add PName.toplevel.closing handler
        member inline this.deactivate (handler:ToplevelEventArgs->unit) (props:IncrementalProps) = props.add PName.toplevel.deactivate handler
        member inline this.loaded (handler:unit->unit) (props:IncrementalProps) = props.add PName.toplevel.loaded handler
        member inline this.ready (handler:unit->unit) (props:IncrementalProps) = props.add PName.toplevel.ready handler
        member inline this.sizeChanging (handler:SizeChangedEventArgs->unit) (props:IncrementalProps) = props.add PName.toplevel.sizeChanging handler
        member inline this.unloaded (handler:unit->unit) (props:IncrementalProps) = props.add PName.toplevel.unloaded handler

    // Dialog
    type dialogPSetter() =
        inherit toplevelPSetter()
        // Properties
        member inline this.buttonAlignment (value:Alignment) (props:IncrementalProps) = props.add PName.dialog.buttonAlignment value
        member inline this.buttonAlignmentModes (value:AlignmentModes) (props:IncrementalProps) = props.add PName.dialog.buttonAlignmentModes value
        member inline this.canceled (value:bool) (props:IncrementalProps) = props.add PName.dialog.canceled value

    // FileDialog
    type fileDialogPSetter() =
        inherit dialogPSetter()
        // Properties
        member inline this.allowedTypes (value:IAllowedType list) (props:IncrementalProps) = props.add PName.fileDialog.allowedTypes value
        member inline this.allowsMultipleSelection (value:bool) (props:IncrementalProps) = props.add PName.fileDialog.allowsMultipleSelection value
        member inline this.fileOperationsHandler (value:IFileOperations) (props:IncrementalProps) = props.add PName.fileDialog.fileOperationsHandler value
        member inline this.mustExist (value:bool) (props:IncrementalProps) = props.add PName.fileDialog.mustExist value
        member inline this.openMode (value:OpenMode) (props:IncrementalProps) = props.add PName.fileDialog.openMode value
        member inline this.path (value:string) (props:IncrementalProps) = props.add PName.fileDialog.path value
        member inline this.searchMatcher (value:ISearchMatcher) (props:IncrementalProps) = props.add PName.fileDialog.searchMatcher value
        // Events
        member inline this.filesSelected (handler:FilesSelectedEventArgs->unit) (props:IncrementalProps) = props.add PName.fileDialog.filesSelected handler

    /// SaveDialog
    type saveDialogPSetter() =
        inherit fileDialogPSetter()

    // FrameView
    type frameViewPSetter() =
        inherit viewPSetter()
    // No properties or events FrameView

    // GraphView
    type graphViewPSetter() =
        inherit viewPSetter()

        // Properties
        member inline this.axisX (value:HorizontalAxis) (props:IncrementalProps) = props.add PName.graphView.axisX value
        member inline this.axisY (value:VerticalAxis) (props:IncrementalProps) = props.add PName.graphView.axisY value
        member inline this.cellSize (value:PointF) (props:IncrementalProps) = props.add PName.graphView.cellSize value
        member inline this.graphColor (value:Attribute option) (props:IncrementalProps) = props.add PName.graphView.graphColor value
        member inline this.marginBottom (value:int) (props:IncrementalProps) = props.add PName.graphView.marginBottom value
        member inline this.marginLeft (value:int) (props:IncrementalProps) = props.add PName.graphView.marginLeft value
        member inline this.scrollOffset (value:PointF) (props:IncrementalProps) = props.add PName.graphView.scrollOffset value

    // HexView
    type hexViewPSetter() =
        inherit viewPSetter()

        // Properties
        member inline this.address (value:Int64) (props:IncrementalProps) = props.add PName.hexView.address value
        member inline this.addressWidth (value:int) (props:IncrementalProps) = props.add PName.hexView.addressWidth value
        member inline this.allowEdits (value:int) (props:IncrementalProps) = props.add PName.hexView.allowEdits value
        member inline this.readOnly (value:bool) (props:IncrementalProps) = props.add PName.hexView.readOnly value
        member inline this.source (value:Stream) (props:IncrementalProps) = props.add PName.hexView.source value
        // Events
        member inline this.edited (handler:HexViewEditEventArgs->unit) (props:IncrementalProps) = props.add PName.hexView.edited handler
        member inline this.positionChanged (handler:HexViewEventArgs->unit) (props:IncrementalProps) = props.add PName.hexView.positionChanged handler

    // Label
    type labelPSetter() =
        inherit viewPSetter()

        // Properties
        member inline this.hotKeySpecifier (value:Rune) (props:IncrementalProps) = props.add PName.label.hotKeySpecifier value
        member inline this.text (value:string) (props:IncrementalProps) = props.add PName.label.text value

    // LegendAnnotation
    type legendAnnotationPSetter() =
        inherit viewPSetter()
    // No properties or events LegendAnnotation

    // Line
    type linePSetter() =
        inherit viewPSetter()

        // Properties
        member inline this.orientation (value:Orientation) (props:IncrementalProps) = props.add PName.line.orientation value
        // Events
        member inline this.orientationChanged (handler:Orientation->unit) (props:IncrementalProps) = props.add PName.line.orientationChanged handler
        member inline this.orientationChanging (handler:App.CancelEventArgs<Orientation>->unit) (props:IncrementalProps) = props.add PName.line.orientationChanging handler

    // LineView
    type lineViewPSetter() =
        inherit viewPSetter()

        // Properties
        member inline this.endingAnchor (value:Rune option) (props:IncrementalProps) = props.add PName.lineView.endingAnchor value
        member inline this.lineRune (value:Rune) (props:IncrementalProps) = props.add PName.lineView.lineRune value
        member inline this.orientation (value:Orientation) (props:IncrementalProps) = props.add PName.lineView.orientation value
        member inline this.startingAnchor (value:Rune option) (props:IncrementalProps) = props.add PName.lineView.startingAnchor value

    // ListView
    type listViewPSetter() =
        inherit viewPSetter()

        // Properties
        member inline this.allowsMarking (value:bool) (props:IncrementalProps) = props.add PName.listView.allowsMarking value
        member inline this.allowsMultipleSelection (value:bool) (props:IncrementalProps) = props.add PName.listView.allowsMultipleSelection value
        member inline this.leftItem (value:Int32) (props:IncrementalProps) = props.add PName.listView.leftItem value
        member inline this.selectedItem (value:Int32) (props:IncrementalProps) = props.add PName.listView.selectedItem value
        member inline this.source (value:string list) (props:IncrementalProps) = props.add PName.listView.source value
        member inline this.topItem (value:Int32) (props:IncrementalProps) = props.add PName.listView.topItem value
        // Events
        member inline this.collectionChanged (handler:NotifyCollectionChangedEventArgs->unit) (props:IncrementalProps) = props.add PName.listView.collectionChanged handler
        member inline this.openSelectedItem (handler:ListViewItemEventArgs->unit) (props:IncrementalProps) = props.add PName.listView.openSelectedItem handler
        member inline this.rowRender (handler:ListViewRowEventArgs->unit) (props:IncrementalProps) = props.add PName.listView.rowRender handler
        member inline this.selectedItemChanged (handler:ListViewItemEventArgs->unit) (props:IncrementalProps) = props.add PName.listView.selectedItemChanged handler

    // Margin
    type marginPSetter() =
        inherit adornmentPSetter()

        // Properties
        member inline this.shadowStyle (value:ShadowStyle) (props:IncrementalProps) = props.add PName.margin.shadowStyle value

    type menuv2PSetter() =
        inherit barPSetter()

        // Properties
        member inline this.selectedMenuItem (value: MenuItemv2 list) (props:IncrementalProps) = props.add PName.menuv2.selectedMenuItem value
        member inline this.superMenuItem (value: MenuItemv2 list) (props:IncrementalProps) = props.add PName.menuv2.superMenuItem value
        // Events
        member inline this.accepted (value: CommandEventArgs->unit) (props:IncrementalProps) = props.add PName.menuv2.accepted value
        member inline this.selectedMenuItemChanged (value: MenuItemv2->unit) (props:IncrementalProps) = props.add PName.menuv2.selectedMenuItemChanged value

    // MenuBarV2
    type menuBarv2PSetter() =
        inherit menuv2PSetter()

        // Properties
        member inline this.key (value:Key) (props:IncrementalProps) = props.add PName.menuBarv2.key value
        member inline this.menus (value:MenuBarItemv2Element list) (props:IncrementalProps) = props.add PName.view.children (List<_>(value |> Seq.map (fun v -> v :> TerminalElement)))
        // Events
        member inline this.keyChanged (handler:KeyChangedEventArgs->unit) (props:IncrementalProps) = props.add PName.menuBarv2.keyChanged handler

    type shortcutPSetter() =
         inherit viewPSetter()

         // Properties
         member inline this.action (value:Action) (props:IncrementalProps) = props.add PName.shortcut.action value
         member inline this.alignmentModes (value:AlignmentModes) (props:IncrementalProps) = props.add PName.shortcut.alignmentModes value
         member inline this.commandView (value:TerminalElement) (props:IncrementalProps) = props.add PName.shortcut.commandView_element value
         member inline this.forceFocusColors (value:bool) (props:IncrementalProps) = props.add PName.shortcut.forceFocusColors value
         member inline this.helpText (value:string) (props:IncrementalProps) = props.add PName.shortcut.helpText value
         member inline this.text (value:string) (props:IncrementalProps) = props.add PName.shortcut.text value
         member inline this.bindKeyToApplication (value:bool) (props:IncrementalProps) = props.add PName.shortcut.bindKeyToApplication value
         member inline this.key (value:Key) (props:IncrementalProps) = props.add PName.shortcut.key value
         member inline this.minimumKeyTextSize (value:Int32) (props:IncrementalProps) = props.add PName.shortcut.minimumKeyTextSize value
         // Events
         member inline this.orientationChanged (handler:Orientation->unit) (props:IncrementalProps) = props.add PName.shortcut.orientationChanged handler
         member inline this.orientationChanging (handler:CancelEventArgs<Orientation>->unit) (props:IncrementalProps) = props.add PName.shortcut.orientationChanging handler

    type menuItemv2PSetter() =
        inherit shortcutPSetter()
        member inline this.command (value: Command) (props:IncrementalProps) = props.add PName.menuItemv2.command value
        member inline this.submenu(value: Menuv2Element) (props:IncrementalProps) = props.add PName.menuItemv2.subMenu_element value
        member inline this.accepted(value: CommandEventArgs -> unit) (props:IncrementalProps) = props.add PName.menuItemv2.accepted value

    type menuBarItemv2PSetter() =
        inherit menuItemv2PSetter()
        member inline this.popoverMenu (value:PopoverMenuElement) (props:IncrementalProps) = props.add PName.menuBarItemv2.popoverMenu_element value
        member inline this.popoverMenuOpen (value:bool) (props:IncrementalProps) = props.add PName.menuBarItemv2.popoverMenuOpen value

    type popoverMenuPSetter() =
        inherit viewPSetter()

        member inline this.key (value:Key) (props:IncrementalProps) = props.add PName.popoverMenu.key value
        member inline this.root (value: Menuv2Element) (props:IncrementalProps) = props.add PName.popoverMenu.root_element value

    // NumericUpDown`1
    type numericUpDownPSetter<'a>() =
        inherit viewPSetter()

        // Properties
        member inline this.format (value:string) (props:IncrementalProps) = props.add PName.numericUpDown.format value
        member inline this.increment (value:'a) (props:IncrementalProps) = props.add PName.numericUpDown.increment value
        member inline this.value (value:'a) (props:IncrementalProps) = props.add PName.numericUpDown.value value
        // Events
        member inline this.formatChanged (handler:string->unit) (props:IncrementalProps) = props.add PName.numericUpDown.formatChanged handler
        member inline this.incrementChanged (handler:'a->unit) (props:IncrementalProps) = props.add PName.numericUpDown.incrementChanged handler
        member inline this.valueChanged (handler:'a->unit) (props:IncrementalProps) = props.add PName.numericUpDown.valueChanged handler
        member inline this.valueChanging (handler:App.CancelEventArgs<'a>->unit) (props:IncrementalProps) = props.add PName.numericUpDown.valueChanging handler

    // NumericUpDown
    type numericUpDownPSetter() =
        inherit numericUpDownPSetter<int>()
    // No properties or events NumericUpDown

    // OpenDialog
    type openDialogPSetter() =
        inherit fileDialogPSetter()
        // Properties
        member inline this.openMode (value:OpenMode) (props:IncrementalProps) = props.add PName.openDialog.openMode value

    // OptionSelector
    type optionSelectorPSetter() =
        inherit viewPSetter()
        //Properties
        member inline this.assignHotKeysToCheckBoxes (value:bool) (props:IncrementalProps) = props.add PName.optionSelector.assignHotKeysToCheckBoxes value
        member inline this.orientation (value:Orientation) (props:IncrementalProps) = props.add PName.optionSelector.orientation value
        member inline this.options (value:IReadOnlyList<string>) (props:IncrementalProps) = props.add PName.optionSelector.options value
        member inline this.selectedItem (value:Int32) (props:IncrementalProps) = props.add PName.optionSelector.selectedItem value
        // Events
        member inline this.orientationChanged (value:Orientation->unit) (props:IncrementalProps) = props.add PName.optionSelector.orientationChanged value
        member inline this.orientationChanging (value:CancelEventArgs<Orientation>->unit) (props:IncrementalProps) = props.add PName.optionSelector.orientationChanging value
        member inline this.selectedItemChanged (value:SelectedItemChangedArgs->unit) (props:IncrementalProps) = props.add PName.optionSelector.selectedItemChanged value

    // Padding
    type paddingPSetter() =
        inherit adornmentPSetter()

    // ProgressBar
    type progressBarPSetter() =
        inherit viewPSetter()

        // Properties
        member inline this.bidirectionalMarquee (value:bool) (props:IncrementalProps) = props.add PName.progressBar.bidirectionalMarquee value
        member inline this.fraction (value:Single) (props:IncrementalProps) = props.add PName.progressBar.fraction value
        member inline this.progressBarFormat (value:ProgressBarFormat) (props:IncrementalProps) = props.add PName.progressBar.progressBarFormat value
        member inline this.progressBarStyle (value:ProgressBarStyle) (props:IncrementalProps) = props.add PName.progressBar.progressBarStyle value
        member inline this.segmentCharacter (value:Rune) (props:IncrementalProps) = props.add PName.progressBar.segmentCharacter value
        member inline this.text (value:string) (props:IncrementalProps) = props.add PName.progressBar.text value

    // RadioGroup
    type radioGroupPSetter() =
        inherit viewPSetter()

        // Properties
        member inline this.assignHotKeysToRadioLabels (value:bool) (props:IncrementalProps) = props.add PName.radioGroup.assignHotKeysToRadioLabels value
        member inline this.cursor (value:Int32) (props:IncrementalProps) = props.add PName.radioGroup.cursor value
        member inline this.doubleClickAccepts (value:bool) (props:IncrementalProps) = props.add PName.radioGroup.doubleClickAccepts value
        member inline this.horizontalSpace (value:Int32) (props:IncrementalProps) = props.add PName.radioGroup.horizontalSpace value
        member inline this.orientation (value:Orientation) (props:IncrementalProps) = props.add PName.radioGroup.orientation value
        member inline this.radioLabels (value:string list) (props:IncrementalProps) = props.add PName.radioGroup.radioLabels value
        member inline this.selectedItem (value:Int32) (props:IncrementalProps) = props.add PName.radioGroup.selectedItem value
        // Events
        member inline this.orientationChanged (handler:Orientation->unit) (props:IncrementalProps) = props.add PName.radioGroup.orientationChanged handler
        member inline this.orientationChanging (handler:App.CancelEventArgs<Orientation>->unit) (props:IncrementalProps) = props.add PName.radioGroup.orientationChanging handler
        member inline this.selectedItemChanged (handler:SelectedItemChangedArgs->unit) (props:IncrementalProps) = props.add PName.radioGroup.selectedItemChanged handler

    // SaveDialog
    // No properties or events SaveDialog

    // ScrollBar
    type scrollBarPSetter() =
        inherit viewPSetter()

        // Properties
        member inline this.autoShow (value:bool) (props:IncrementalProps) = props.add PName.scrollBar.autoShow value
        member inline this.increment (value:Int32) (props:IncrementalProps) = props.add PName.scrollBar.increment value
        member inline this.orientation (value:Orientation) (props:IncrementalProps) = props.add PName.scrollBar.orientation value
        member inline this.position (value:Int32) (props:IncrementalProps) = props.add PName.scrollBar.position value
        member inline this.scrollableContentSize (value:Int32) (props:IncrementalProps) = props.add PName.scrollBar.scrollableContentSize value
        member inline this.visibleContentSize (value:Int32) (props:IncrementalProps) = props.add PName.scrollBar.visibleContentSize value
        // Events
        member inline this.orientationChanged (handler:Orientation->unit) (props:IncrementalProps) = props.add PName.scrollBar.orientationChanged handler
        member inline this.orientationChanging (handler:CancelEventArgs<Orientation>->unit) (props:IncrementalProps) = props.add PName.scrollBar.orientationChanging handler
        member inline this.scrollableContentSizeChanged (handler:EventArgs<Int32>->unit) (props:IncrementalProps) = props.add PName.scrollBar.scrollableContentSizeChanged handler
        member inline this.sliderPositionChanged (handler:EventArgs<Int32>->unit) (props:IncrementalProps) = props.add PName.scrollBar.sliderPositionChanged handler

    // ScrollSlider
    type scrollSliderPSetter() =
        inherit viewPSetter()

        // Properties
        member inline this.orientation (value:Orientation) (props:IncrementalProps) = props.add PName.scrollSlider.orientation value
        member inline this.position (value:Int32) (props:IncrementalProps) = props.add PName.scrollSlider.position value
        member inline this.size (value:Int32) (props:IncrementalProps) = props.add PName.scrollSlider.size value
        member inline this.sliderPadding (value:Int32) (props:IncrementalProps) = props.add PName.scrollSlider.sliderPadding value
        member inline this.visibleContentSize (value:Int32) (props:IncrementalProps) = props.add PName.scrollSlider.visibleContentSize value
        // Events
        member inline this.orientationChanged (handler:Orientation->unit) (props:IncrementalProps) = props.add PName.scrollSlider.orientationChanged handler
        member inline this.orientationChanging (handler:CancelEventArgs<Orientation>->unit) (props:IncrementalProps) = props.add PName.scrollSlider.orientationChanging handler
        member inline this.positionChanged (handler:EventArgs<Int32>->unit) (props:IncrementalProps) = props.add PName.scrollSlider.positionChanged handler
        member inline this.positionChanging (handler:CancelEventArgs<Int32>->unit) (props:IncrementalProps) = props.add PName.scrollSlider.positionChanging handler
        member inline this.scrolled (handler:EventArgs<Int32>->unit) (props:IncrementalProps) = props.add PName.scrollSlider.scrolled handler

    // Slider`1
    type sliderPSetter<'a>() =
        inherit viewPSetter()

        // Properties
        member inline this.allowEmpty (value:bool) (props:IncrementalProps) = props.add PName.slider.allowEmpty value
        member inline this.focusedOption (value:Int32) (props:IncrementalProps) = props.add PName.slider.focusedOption value
        member inline this.legendsOrientation (value:Orientation) (props:IncrementalProps) = props.add PName.slider.legendsOrientation value
        member inline this.minimumInnerSpacing (value:Int32) (props:IncrementalProps) = props.add PName.slider.minimumInnerSpacing value
        member inline this.options (value:SliderOption<'a> list) (props:IncrementalProps) = props.add PName.slider.options value
        member inline this.orientation (value:Orientation) (props:IncrementalProps) = props.add PName.slider.orientation value
        member inline this.rangeAllowSingle (value:bool) (props:IncrementalProps) = props.add PName.slider.rangeAllowSingle value
        member inline this.showEndSpacing (value:bool) (props:IncrementalProps) = props.add PName.slider.showEndSpacing value
        member inline this.showLegends (value:bool) (props:IncrementalProps) = props.add PName.slider.showLegends value
        member inline this.style (value:SliderStyle) (props:IncrementalProps) = props.add PName.slider.style value
        member inline this.text (value:string) (props:IncrementalProps) = props.add PName.slider.text value
        member inline this.``type`` (value:SliderType) (props:IncrementalProps) = props.add PName.slider.``type`` value
        member inline this.useMinimumSize (value:bool) (props:IncrementalProps) = props.add PName.slider.useMinimumSize value
        // Events
        member inline this.optionFocused (handler:SliderEventArgs<'a>->unit) (props:IncrementalProps) = props.add PName.slider.optionFocused handler
        member inline this.optionsChanged (handler:SliderEventArgs<'a>->unit) (props:IncrementalProps) = props.add PName.slider.optionsChanged handler
        member inline this.orientationChanged (handler:Orientation->unit) (props:IncrementalProps) = props.add PName.slider.orientationChanged handler
        member inline this.orientationChanging (handler:App.CancelEventArgs<Orientation>->unit) (props:IncrementalProps) = props.add PName.slider.orientationChanging handler

    // Slider
    type sliderPSetter() =
        inherit sliderPSetter<obj>()
    // No properties or events Slider

    // SpinnerView
    type spinnerViewPSetter() =
        inherit viewPSetter()

        // Properties
        member inline this.autoSpin (value:bool) (props:IncrementalProps) = props.add PName.spinnerView.autoSpin value
        member inline this.sequence (value:string list) (props:IncrementalProps) = props.add PName.spinnerView.sequence value
        member inline this.spinBounce (value:bool) (props:IncrementalProps) = props.add PName.spinnerView.spinBounce value
        member inline this.spinDelay (value:Int32) (props:IncrementalProps) = props.add PName.spinnerView.spinDelay value
        member inline this.spinReverse (value:bool) (props:IncrementalProps) = props.add PName.spinnerView.spinReverse value
        member inline this.style (value:SpinnerStyle) (props:IncrementalProps) = props.add PName.spinnerView.style value

    // StatusBar
    type statusBarPSetter() =
        inherit barPSetter()
    // No properties or events StatusBar

    // Tab
    type tabPSetter() =
        inherit viewPSetter()

        // Properties
        member inline this.displayText (value:string) (props:IncrementalProps) = props.add PName.tab.displayText value
        member inline this.view (value:TerminalElement) (props:IncrementalProps) = props.add PName.tab.view value

    // TabView
    type tabViewPSetter() =
        inherit viewPSetter()

        // Properties
        member inline this.maxTabTextWidth (value:int) (props:IncrementalProps) = props.add PName.tabView.maxTabTextWidth value
        member inline this.selectedTab (value:Tab) (props:IncrementalProps) = props.add PName.tabView.selectedTab value
        member inline this.style (value:TabStyle) (props:IncrementalProps) = props.add PName.tabView.style value
        member inline this.tabScrollOffset (value:Int32) (props:IncrementalProps) = props.add PName.tabView.tabScrollOffset value
        // Events
        member inline this.selectedTabChanged (handler:TabChangedEventArgs->unit) (props:IncrementalProps) = props.add PName.tabView.selectedTabChanged handler
        member inline this.tabClicked (handler:TabMouseEventArgs->unit) (props:IncrementalProps) = props.add PName.tabView.tabClicked handler

        member inline this.tabs (value:TerminalElement list) (props:IncrementalProps) = props.add PName.tabView.tabs value

    // TableView
    type tableViewPSetter() =
        inherit viewPSetter()

        // Properties
        member inline this.cellActivationKey (value:KeyCode) (props:IncrementalProps) = props.add PName.tableView.cellActivationKey value
        member inline this.collectionNavigator (value:ICollectionNavigator) (props:IncrementalProps) = props.add PName.tableView.collectionNavigator value
        member inline this.columnOffset (value:Int32) (props:IncrementalProps) = props.add PName.tableView.columnOffset value
        member inline this.fullRowSelect (value:bool) (props:IncrementalProps) = props.add PName.tableView.fullRowSelect value
        member inline this.maxCellWidth (value:Int32) (props:IncrementalProps) = props.add PName.tableView.maxCellWidth value
        member inline this.minCellWidth (value:Int32) (props:IncrementalProps) = props.add PName.tableView.minCellWidth value
        member inline this.multiSelect (value:bool) (props:IncrementalProps) = props.add PName.tableView.multiSelect value
        member inline this.nullSymbol (value:string) (props:IncrementalProps) = props.add PName.tableView.nullSymbol value
        member inline this.rowOffset (value:Int32) (props:IncrementalProps) = props.add PName.tableView.rowOffset value
        member inline this.selectedColumn (value:Int32) (props:IncrementalProps) = props.add PName.tableView.selectedColumn value
        member inline this.selectedRow (value:Int32) (props:IncrementalProps) = props.add PName.tableView.selectedRow value
        member inline this.separatorSymbol (value:Char) (props:IncrementalProps) = props.add PName.tableView.separatorSymbol value
        member inline this.style (value:TableStyle) (props:IncrementalProps) = props.add PName.tableView.style value
        member inline this.table (value:ITableSource) (props:IncrementalProps) = props.add PName.tableView.table value
        // Events
        member inline this.cellActivated (handler:CellActivatedEventArgs->unit) (props:IncrementalProps) = props.add PName.tableView.cellActivated handler
        member inline this.cellToggled (handler:CellToggledEventArgs->unit) (props:IncrementalProps) = props.add PName.tableView.cellToggled handler
        member inline this.selectedCellChanged (handler:SelectedCellChangedEventArgs->unit) (props:IncrementalProps) = props.add PName.tableView.selectedCellChanged handler

    // TextValidateField
    type textValidateFieldPSetter() =
        inherit viewPSetter()

        // Properties
        member inline this.provider (value:ITextValidateProvider) (props:IncrementalProps) = props.add PName.textValidateField.provider value
        member inline this.text (value:string) (props:IncrementalProps) = props.add PName.textValidateField.text value

    // TextView
    type textViewPSetter() =
        inherit viewPSetter()

        // Properties
        member inline this.allowsReturn (value:bool) (props:IncrementalProps) = props.add PName.textView.allowsReturn value
        member inline this.allowsTab (value:bool) (props:IncrementalProps) = props.add PName.textView.allowsTab value
        member inline this.cursorPosition (value:Point) (props:IncrementalProps) = props.add PName.textView.cursorPosition value
        member inline this.inheritsPreviousAttribute (value:bool) (props:IncrementalProps) = props.add PName.textView.inheritsPreviousAttribute value
        member inline this.isDirty (value:bool) (props:IncrementalProps) = props.add PName.textView.isDirty value
        member inline this.isSelecting (value:bool) (props:IncrementalProps) = props.add PName.textView.isSelecting value
        member inline this.leftColumn (value:Int32) (props:IncrementalProps) = props.add PName.textView.leftColumn value
        member inline this.multiline (value:bool) (props:IncrementalProps) = props.add PName.textView.multiline value
        member inline this.readOnly (value:bool) (props:IncrementalProps) = props.add PName.textView.readOnly value
        member inline this.selectionStartColumn (value:Int32) (props:IncrementalProps) = props.add PName.textView.selectionStartColumn value
        member inline this.selectionStartRow (value:Int32) (props:IncrementalProps) = props.add PName.textView.selectionStartRow value
        member inline this.selectWordOnlyOnDoubleClick (value:bool) (props:IncrementalProps) = props.add PName.textView.selectWordOnlyOnDoubleClick value
        member inline this.tabWidth (value:Int32) (props:IncrementalProps) = props.add PName.textView.tabWidth value
        member inline this.text (value:string) (props:IncrementalProps) = props.add PName.textView.text value
        member inline this.topRow (value:Int32) (props:IncrementalProps) = props.add PName.textView.topRow value
        member inline this.used (value:bool) (props:IncrementalProps) = props.add PName.textView.used value
        member inline this.useSameRuneTypeForWords (value:bool) (props:IncrementalProps) = props.add PName.textView.useSameRuneTypeForWords value
        member inline this.wordWrap (value:bool) (props:IncrementalProps) = props.add PName.textView.wordWrap value
        // Events
        member inline this.contentsChanged (handler:ContentsChangedEventArgs->unit) (props:IncrementalProps) = props.add PName.textView.contentsChanged handler
        member inline this.drawNormalColor (handler:CellEventArgs->unit) (props:IncrementalProps) = props.add PName.textView.drawNormalColor handler
        member inline this.drawReadOnlyColor (handler:CellEventArgs->unit) (props:IncrementalProps) = props.add PName.textView.drawReadOnlyColor handler
        member inline this.drawSelectionColor (handler:CellEventArgs->unit) (props:IncrementalProps) = props.add PName.textView.drawSelectionColor handler
        member inline this.drawUsedColor (handler:CellEventArgs->unit) (props:IncrementalProps) = props.add PName.textView.drawUsedColor handler
        member inline this.unwrappedCursorPosition (handler:Point->unit) (props:IncrementalProps) = props.add PName.textView.unwrappedCursorPosition handler
        // Additional properties
        member inline this.textChanged (value:string->unit) (props:IncrementalProps) = props.add PName.textView.textChanged value

    // TileView
    type tileViewPSetter() =
        inherit viewPSetter()

        // Properties
        member inline this.lineStyle (value:LineStyle) (props:IncrementalProps) = props.add PName.tileView.lineStyle value
        member inline this.orientation (value:Orientation) (props:IncrementalProps) = props.add PName.tileView.orientation value
        member inline this.toggleResizable (value:KeyCode) (props:IncrementalProps) = props.add PName.tileView.toggleResizable value
        // Events
        member inline this.splitterMoved (handler:SplitterEventArgs->unit) (props:IncrementalProps) = props.add PName.tileView.splitterMoved handler

    // TimeField
    type timeFieldPSetter() =
        inherit textFieldPSetter()

        // Properties
        member inline this.cursorPosition (value:Int32) (props:IncrementalProps) = props.add PName.timeField.cursorPosition value
        member inline this.isShortFormat (value:bool) (props:IncrementalProps) = props.add PName.timeField.isShortFormat value
        member inline this.time (value:TimeSpan) (props:IncrementalProps) = props.add PName.timeField.time value
        // Events
        member inline this.timeChanged (handler:DateTimeEventArgs<TimeSpan>->unit) (props:IncrementalProps) = props.add PName.timeField.timeChanged handler

    // TreeView`1
    type treeViewPSetter<'a when 'a : not struct>() =
        inherit viewPSetter()

        // Properties
        member inline this.allowLetterBasedNavigation (value:bool) (props:IncrementalProps) = props.add PName.treeView.allowLetterBasedNavigation value
        member inline this.aspectGetter<'a when 'a : not struct> (value:AspectGetterDelegate<'a>) (props:IncrementalProps) = props.add PName.treeView.aspectGetter value
        member inline this.colorGetter<'a when 'a : not struct> (value:Func<'a,Scheme>) (props:IncrementalProps) = props.add PName.treeView.colorGetter value
        member inline this.maxDepth (value:Int32) (props:IncrementalProps) = props.add PName.treeView.maxDepth value
        member inline this.multiSelect (value:bool) (props:IncrementalProps) = props.add PName.treeView.multiSelect value
        member inline this.objectActivationButton (value:MouseFlags option) (props:IncrementalProps) = props.add PName.treeView.objectActivationButton value
        member inline this.objectActivationKey (value:KeyCode) (props:IncrementalProps) = props.add PName.treeView.objectActivationKey value
        member inline this.scrollOffsetHorizontal (value:Int32) (props:IncrementalProps) = props.add PName.treeView.scrollOffsetHorizontal value
        member inline this.scrollOffsetVertical (value:Int32) (props:IncrementalProps) = props.add PName.treeView.scrollOffsetVertical value
        member inline this.selectedObject<'a when 'a : not struct> (value:'a) (props:IncrementalProps) = props.add PName.treeView.selectedObject value
        member inline this.style (value:TreeStyle) (props:IncrementalProps) = props.add PName.treeView.style value
        member inline this.treeBuilder<'a when 'a : not struct> (value:ITreeBuilder<'a>) (props:IncrementalProps) = props.add PName.treeView.treeBuilder value
        // Events
        member inline this.drawLine<'a when 'a : not struct> (handler:DrawTreeViewLineEventArgs<'a>->unit) (props:IncrementalProps) = props.add PName.treeView.drawLine handler
        member inline this.objectActivated<'a when 'a : not struct> (handler:ObjectActivatedEventArgs<'a>->unit) (props:IncrementalProps) = props.add PName.treeView.objectActivated handler
        member inline this.selectionChanged<'a when 'a : not struct> (handler:SelectionChangedEventArgs<'a>->unit) (props:IncrementalProps) = props.add PName.treeView.selectionChanged handler

    // TreeView
    type treeViewPSetter() =
        inherit treeViewPSetter<ITreeNode>()
    // No properties or events TreeView

    // Window
    type windowPSetter() =
        inherit toplevelPSetter()
    // No properties or events Window

    // Wizard
    type wizardPSetter() =
        inherit dialogPSetter()

        // Properties
        member inline this.currentStep (value:WizardStep) (props:IncrementalProps) = props.add PName.wizard.currentStep value
        member inline this.modal (value:bool) (props:IncrementalProps) = props.add PName.wizard.modal value
        // Events
        member inline this.cancelled (handler:WizardButtonEventArgs->unit) (props:IncrementalProps) = props.add PName.wizard.cancelled handler
        member inline this.finished (handler:WizardButtonEventArgs->unit) (props:IncrementalProps) = props.add PName.wizard.finished handler
        member inline this.movingBack (handler:WizardButtonEventArgs->unit) (props:IncrementalProps) = props.add PName.wizard.movingBack handler
        member inline this.movingNext (handler:WizardButtonEventArgs->unit) (props:IncrementalProps) = props.add PName.wizard.movingNext handler
        member inline this.stepChanged (handler:StepChangeEventArgs->unit) (props:IncrementalProps) = props.add PName.wizard.stepChanged handler
        member inline this.stepChanging (handler:StepChangeEventArgs->unit) (props:IncrementalProps) = props.add PName.wizard.stepChanging handler

    // WizardStep
    type wizardStepPSetter() =
        inherit viewPSetter()

        // Properties
        member inline this.backButtonText (value:string) (props:IncrementalProps) = props.add PName.wizardStep.backButtonText value
        member inline this.helpText (value:string) (props:IncrementalProps) = props.add PName.wizardStep.helpText value
        member inline this.nextButtonText (value:string) (props:IncrementalProps) = props.add PName.wizardStep.nextButtonText value


    let view = viewPSetter()
    let adornment = adornmentPSetter()
    let bar = barPSetter()
    let border = borderPSetter()
    let button = buttonPSetter()
    let checkBox = checkBoxPSetter()
    let colorPicker = colorPickerPSetter()
    let colorPicker16 = colorPicker16PSetter()
    let comboBox = comboBoxPSetter()
    let textField = textFieldPSetter()
    let dateField = dateFieldPSetter()
    let datePicker = datePickerPSetter()
    let toplevel = toplevelPSetter()
    let dialog = dialogPSetter()
    let fileDialog = fileDialogPSetter()
    let saveDialog = saveDialogPSetter()
    let frameView = frameViewPSetter()
    let graphView = graphViewPSetter()
    let hexView = hexViewPSetter()
    let label = labelPSetter()
    let legendAnnotation = legendAnnotationPSetter()
    let line = linePSetter()
    let lineView = lineViewPSetter()
    let listView = listViewPSetter()
    let margin = marginPSetter()
    let menuv2 = menuv2PSetter()
    let menuBarv2 = menuBarv2PSetter()
    let shortcut = shortcutPSetter()
    let menuItemv2 = menuItemv2PSetter()
    let menuBarItemv2 = menuBarItemv2PSetter()
    let popoverMenu = popoverMenuPSetter()
    let numericUpDown = numericUpDownPSetter()
    let openDialog = openDialogPSetter()
    let optionSelector = optionSelectorPSetter()
    let padding = paddingPSetter()
    let progressBar = progressBarPSetter()
    let radioGroup = radioGroupPSetter()
    let scrollBar = scrollBarPSetter()
    let scrollSlider = scrollSliderPSetter()
    let slider= sliderPSetter()
    let spinnerView = spinnerViewPSetter()
    let statusBar = statusBarPSetter()
    let tab = tabPSetter()
    let tabView = tabViewPSetter()
    let tableView = tableViewPSetter()
    let textValidateField = textValidateFieldPSetter()
    let textView = textViewPSetter()
    let tileView = tileViewPSetter()
    let timeField = timeFieldPSetter()
    let treeView = treeViewPSetter()
    let window = windowPSetter()
    let wizard = wizardPSetter()
    let wizardStep = wizardStepPSetter()
