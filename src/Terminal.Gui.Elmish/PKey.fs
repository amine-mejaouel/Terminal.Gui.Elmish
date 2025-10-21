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

open Terminal.Gui.FileServices
open Terminal.Gui.Input

open Terminal.Gui.Text

open Terminal.Gui.ViewBase

open Terminal.Gui.Views

/// Properties key index
module internal PKey =

    type viewPKeys() =
        member inline this.children : SimplePropKey<System.Collections.Generic.List<IInternalTerminalElement>> = SimplePropKey.create "children"

        // Properties
        member inline this.arrangement : SimplePropKey<ViewArrangement> = SimplePropKey.create "view.arrangement"
        member inline this.borderStyle : SimplePropKey<LineStyle> = SimplePropKey.create "view.borderStyle"
        member inline this.canFocus : SimplePropKey<bool> = SimplePropKey.create "view.canFocus"
        member inline this.contentSizeTracksViewport : SimplePropKey<bool> = SimplePropKey.create "view.contentSizeTracksViewport"
        member inline this.cursorVisibility : SimplePropKey<CursorVisibility> = SimplePropKey.create "view.cursorVisibility"
        member inline this.data : SimplePropKey<Object> = SimplePropKey.create "view.data"
        member inline this.enabled : SimplePropKey<bool> = SimplePropKey.create "view.enabled"
        member inline this.frame : SimplePropKey<Rectangle> = SimplePropKey.create "view.frame"
        member inline this.hasFocus : SimplePropKey<bool> = SimplePropKey.create "view.hasFocus"
        member inline this.height : SimplePropKey<Dim> = SimplePropKey.create "view.height"
        member inline this.highlightStates : SimplePropKey<MouseState> = SimplePropKey.create "view.highlightStates"
        member inline this.hotKey : SimplePropKey<Key> = SimplePropKey.create "view.hotKey"
        member inline this.hotKeySpecifier : SimplePropKey<Rune> = SimplePropKey.create "view.hotKeySpecifier"
        member inline this.id : SimplePropKey<string> = SimplePropKey.create "view.id"
        member inline this.isInitialized : SimplePropKey<bool> = SimplePropKey.create "view.isInitialized"
        member inline this.mouseHeldDown : SimplePropKey<IMouseHeldDown> = SimplePropKey.create "view.mouseHeldDown"
        member inline this.needsDraw : SimplePropKey<bool> = SimplePropKey.create "view.needsDraw"
        member inline this.preserveTrailingSpaces : SimplePropKey<bool> = SimplePropKey.create "view.preserveTrailingSpaces"
        member inline this.schemeName : SimplePropKey<string> = SimplePropKey.create "view.schemeName"
        member inline this.shadowStyle : SimplePropKey<ShadowStyle> = SimplePropKey.create "view.shadowStyle"
        member inline this.superViewRendersLineCanvas : SimplePropKey<bool> = SimplePropKey.create "view.superViewRendersLineCanvas"
        member inline this.tabStop : SimplePropKey<TabBehavior option> = SimplePropKey.create "view.tabStop"
        member inline this.text : SimplePropKey<string> = SimplePropKey.create "view.text"
        member inline this.textAlignment : SimplePropKey<Alignment> = SimplePropKey.create "view.textAlignment"
        member inline this.textDirection : SimplePropKey<TextDirection> = SimplePropKey.create "view.textDirection"
        member inline this.title : SimplePropKey<string> = SimplePropKey.create "view.title"
        member inline this.validatePosDim : SimplePropKey<bool> = SimplePropKey.create "view.validatePosDim"
        member inline this.verticalTextAlignment : SimplePropKey<Alignment> = SimplePropKey.create "view.verticalTextAlignment"
        member inline this.viewport : SimplePropKey<Rectangle> = SimplePropKey.create "view.viewport"
        member inline this.viewportSettings : SimplePropKey<ViewportSettingsFlags> = SimplePropKey.create "view.viewportSettings"
        member inline this.visible : SimplePropKey<bool> = SimplePropKey.create "view.visible"
        member inline this.wantContinuousButtonPressed : SimplePropKey<bool> = SimplePropKey.create "view.wantContinuousButtonPressed"
        member inline this.wantMousePositionReports : SimplePropKey<bool> = SimplePropKey.create "view.wantMousePositionReports"
        member inline this.width : SimplePropKey<Dim> = SimplePropKey.create "view.width"
        member inline this.x : SimplePropKey<Pos> = SimplePropKey.create "view.x"
        member inline this.y : SimplePropKey<Pos> = SimplePropKey.create "view.y"
        // Events
        member inline this.accepting : SimplePropKey<HandledEventArgs->unit> = SimplePropKey.create "view.accepting"
        member inline this.advancingFocus : SimplePropKey<AdvanceFocusEventArgs->unit> = SimplePropKey.create "view.advancingFocus"
        member inline this.borderStyleChanged : SimplePropKey<EventArgs->unit> = SimplePropKey.create "view.borderStyleChanged"
        member inline this.canFocusChanged : SimplePropKey<unit->unit> = SimplePropKey.create "view.canFocusChanged"
        member inline this.clearedViewport : SimplePropKey<DrawEventArgs->unit> = SimplePropKey.create "view.clearedViewport"
        member inline this.clearingViewport : SimplePropKey<DrawEventArgs->unit> = SimplePropKey.create "view.clearingViewport"
        member inline this.commandNotBound : SimplePropKey<CommandEventArgs->unit> = SimplePropKey.create "view.commandNotBound"
        member inline this.contentSizeChanged : SimplePropKey<SizeChangedEventArgs->unit> = SimplePropKey.create "view.contentSizeChanged"
        member inline this.disposing : SimplePropKey<unit->unit> = SimplePropKey.create "view.disposing"
        member inline this.drawComplete : SimplePropKey<DrawEventArgs->unit> = SimplePropKey.create "view.drawComplete"
        member inline this.drawingContent : SimplePropKey<DrawEventArgs->unit> = SimplePropKey.create "view.drawingContent"
        member inline this.drawingSubViews : SimplePropKey<DrawEventArgs->unit> = SimplePropKey.create "view.drawingSubViews"
        member inline this.drawingText : SimplePropKey<DrawEventArgs->unit> = SimplePropKey.create "view.drawingText"
        member inline this.enabledChanged : SimplePropKey<unit->unit> = SimplePropKey.create "view.enabledChanged"
        member inline this.focusedChanged : SimplePropKey<HasFocusEventArgs->unit> = SimplePropKey.create "view.focusedChanged"
        member inline this.frameChanged : SimplePropKey<EventArgs<Rectangle>->unit> = SimplePropKey.create "view.frameChanged"
        member inline this.gettingAttributeForRole : SimplePropKey<VisualRoleEventArgs->unit> = SimplePropKey.create "view.gettingAttributeForRole"
        member inline this.gettingScheme : SimplePropKey<ResultEventArgs<Scheme>->unit> = SimplePropKey.create "view.gettingScheme"
        member inline this.handlingHotKey : SimplePropKey<CommandEventArgs->unit> = SimplePropKey.create "view.handlingHotKey"
        member inline this.hasFocusChanged : SimplePropKey<HasFocusEventArgs->unit> = SimplePropKey.create "view.hasFocusChanged"
        member inline this.hasFocusChanging : SimplePropKey<HasFocusEventArgs->unit> = SimplePropKey.create "view.hasFocusChanging"
        member inline this.hotKeyChanged : SimplePropKey<KeyChangedEventArgs->unit> = SimplePropKey.create "view.hotKeyChanged"
        member inline this.initialized : SimplePropKey<unit->unit> = SimplePropKey.create "view.initialized"
        member inline this.keyDown : SimplePropKey<Key->unit> = SimplePropKey.create "view.keyDown"
        member inline this.keyDownNotHandled : SimplePropKey<Key->unit> = SimplePropKey.create "view.keyDownNotHandled"
        member inline this.keyUp : SimplePropKey<Key->unit> = SimplePropKey.create "view.keyUp"
        member inline this.mouseClick : SimplePropKey<MouseEventArgs->unit> = SimplePropKey.create "view.mouseClick"
        member inline this.mouseEnter : SimplePropKey<CancelEventArgs->unit> = SimplePropKey.create "view.mouseEnter"
        member inline this.mouseEvent : SimplePropKey<MouseEventArgs->unit> = SimplePropKey.create "view.mouseEvent"
        member inline this.mouseLeave : SimplePropKey<EventArgs->unit> = SimplePropKey.create "view.mouseLeave"
        member inline this.mouseStateChanged : SimplePropKey<EventArgs->unit> = SimplePropKey.create "view.mouseStateChanged"
        member inline this.mouseWheel : SimplePropKey<MouseEventArgs->unit> = SimplePropKey.create "view.mouseWheel"
        member inline this.removed : SimplePropKey<SuperViewChangedEventArgs->unit> = SimplePropKey.create "view.removed"
        member inline this.schemeChanged : SimplePropKey<ValueChangedEventArgs<Scheme>->unit> = SimplePropKey.create "view.schemeChanged"
        member inline this.schemeChanging : SimplePropKey<ValueChangingEventArgs<Scheme>->unit> = SimplePropKey.create "view.schemeChanging"
        member inline this.schemeNameChanged : SimplePropKey<ValueChangedEventArgs<string>->unit> = SimplePropKey.create "view.schemeNameChanged"
        member inline this.schemeNameChanging : SimplePropKey<ValueChangingEventArgs<string>->unit> = SimplePropKey.create "view.schemeNameChanging"
        member inline this.selecting : SimplePropKey<CommandEventArgs->unit> = SimplePropKey.create "view.selecting"
        member inline this.subViewAdded : SimplePropKey<SuperViewChangedEventArgs->unit> = SimplePropKey.create "view.subViewAdded"
        member inline this.subViewLayout : SimplePropKey<LayoutEventArgs->unit> = SimplePropKey.create "view.subViewLayout"
        member inline this.subViewRemoved : SimplePropKey<SuperViewChangedEventArgs->unit> = SimplePropKey.create "view.subViewRemoved"
        member inline this.subViewsLaidOut : SimplePropKey<LayoutEventArgs->unit> = SimplePropKey.create "view.subViewsLaidOut"
        member inline this.superViewChanged : SimplePropKey<SuperViewChangedEventArgs->unit> = SimplePropKey.create "view.superViewChanged"
        member inline this.textChanged : SimplePropKey<unit->unit> = SimplePropKey.create "view.textChanged"
        member inline this.titleChanged : SimplePropKey<string->unit> = SimplePropKey.create "view.titleChanged"
        member inline this.titleChanging : SimplePropKey<App.CancelEventArgs<string>->unit> = SimplePropKey.create "view.titleChanging"
        member inline this.viewportChanged : SimplePropKey<DrawEventArgs->unit> = SimplePropKey.create "view.viewportChanged"
        member inline this.visibleChanged : SimplePropKey<unit->unit> = SimplePropKey.create "view.visibleChanged"
        member inline this.visibleChanging : SimplePropKey<unit->unit> = SimplePropKey.create "view.visibleChanging"

    // Adornment
    type adornmentPKeys() =
        inherit viewPKeys()
        // Properties
        member inline this.diagnostics : SimplePropKey<ViewDiagnosticFlags> = SimplePropKey.create "adornment.diagnostics"
        member inline this.superViewRendersLineCanvas : SimplePropKey<bool> = SimplePropKey.create "adornment.superViewRendersLineCanvas"
        member inline this.thickness : SimplePropKey<Thickness> = SimplePropKey.create "adornment.thickness"
        member inline this.viewport : SimplePropKey<Rectangle> = SimplePropKey.create "adornment.viewport"
        // Events
        member inline this.thicknessChanged : SimplePropKey<unit->unit> = SimplePropKey.create "adornment.thicknessChanged"

    // Bar
    type barPKeys() =
        inherit viewPKeys()
        // Properties
        member inline this.alignmentModes : SimplePropKey<AlignmentModes> = SimplePropKey.create "bar.alignmentModes"
        member inline this.orientation : SimplePropKey<Orientation> = SimplePropKey.create "bar.orientation"
        // Events
        member inline this.orientationChanged : SimplePropKey<Orientation->unit> = SimplePropKey.create "bar.orientationChanged"
        member inline this.orientationChanging : SimplePropKey<App.CancelEventArgs<Orientation>->unit> = SimplePropKey.create "bar.orientationChanging"

    // Border
    type borderPKeys() =
        inherit adornmentPKeys()
        // Properties
        member inline this.lineStyle : SimplePropKey<LineStyle> = SimplePropKey.create "border.lineStyle"
        member inline this.settings : SimplePropKey<BorderSettings> = SimplePropKey.create "border.settings"

    // Button
    type buttonPKeys() =
        inherit viewPKeys()
        // Properties
        member inline this.hotKeySpecifier : SimplePropKey<Rune> = SimplePropKey.create "button.hotKeySpecifier"
        member inline this.isDefault : SimplePropKey<bool> = SimplePropKey.create "button.isDefault"
        member inline this.noDecorations : SimplePropKey<bool> = SimplePropKey.create "button.noDecorations"
        member inline this.noPadding : SimplePropKey<bool> = SimplePropKey.create "button.noPadding"
        member inline this.text : SimplePropKey<string> = SimplePropKey.create "button.text"
        member inline this.wantContinuousButtonPressed : SimplePropKey<bool> = SimplePropKey.create "button.wantContinuousButtonPressed"

    // CheckBox
    type checkBoxPKeys() =
        inherit viewPKeys()
        // Properties
        member inline this.allowCheckStateNone : SimplePropKey<bool> = SimplePropKey.create "checkBox.allowCheckStateNone"
        member inline this.checkedState : SimplePropKey<CheckState> = SimplePropKey.create "checkBox.checkedState"
        member inline this.hotKeySpecifier : SimplePropKey<Rune> = SimplePropKey.create "checkBox.hotKeySpecifier"
        member inline this.radioStyle : SimplePropKey<bool> = SimplePropKey.create "checkBox.radioStyle"
        member inline this.text : SimplePropKey<string> = SimplePropKey.create "checkBox.text"
        // Events
        member inline this.checkedStateChanging : SimplePropKey<ResultEventArgs<CheckState>->unit> = SimplePropKey.create "checkBox.checkedStateChanging"
        member inline this.checkedStateChanged : SimplePropKey<EventArgs<CheckState>->unit> = SimplePropKey.create "checkBox.checkedStateChanged"

    // ColorPicker
    type colorPickerPKeys() =
        inherit viewPKeys()
        // Properties
        member inline this.selectedColor : SimplePropKey<Color> = SimplePropKey.create "colorPicker.selectedColor"
        member inline this.style : SimplePropKey<ColorPickerStyle> = SimplePropKey.create "colorPicker.style"
        // Events
        member inline this.colorChanged : SimplePropKey<ResultEventArgs<Color>->unit> = SimplePropKey.create "colorPicker.colorChanged"

    // ColorPicker16
    type colorPicker16PKeys() =
        inherit viewPKeys()
        // Properties
        member inline this.boxHeight : SimplePropKey<Int32> = SimplePropKey.create "colorPicker16.boxHeight"
        member inline this.boxWidth : SimplePropKey<Int32> = SimplePropKey.create "colorPicker16.boxWidth"
        member inline this.cursor : SimplePropKey<Point> = SimplePropKey.create "colorPicker16.cursor"
        member inline this.selectedColor : SimplePropKey<ColorName16> = SimplePropKey.create "colorPicker16.selectedColor"
        // Events
        member inline this.colorChanged : SimplePropKey<ResultEventArgs<Color>->unit> = SimplePropKey.create "colorPicker16.colorChanged"

    // ComboBox
    type comboBoxPKeys() =
        inherit viewPKeys()
        // Properties
        member inline this.hideDropdownListOnClick : SimplePropKey<bool> = SimplePropKey.create "comboBox.hideDropdownListOnClick"
        member inline this.readOnly : SimplePropKey<bool> = SimplePropKey.create "comboBox.readOnly"
        member inline this.searchText : SimplePropKey<string> = SimplePropKey.create "comboBox.searchText"
        member inline this.selectedItem : SimplePropKey<Int32> = SimplePropKey.create "comboBox.selectedItem"
        member inline this.source : SimplePropKey<string list> = SimplePropKey.create "comboBox.source"
        member inline this.text : SimplePropKey<string> = SimplePropKey.create "comboBox.text"
        // Events
        member inline this.collapsed : SimplePropKey<unit->unit> = SimplePropKey.create "comboBox.collapsed"
        member inline this.expanded : SimplePropKey<unit->unit> = SimplePropKey.create "comboBox.expanded"
        member inline this.openSelectedItem : SimplePropKey<ListViewItemEventArgs->unit> = SimplePropKey.create "comboBox.openSelectedItem"
        member inline this.selectedItemChanged : SimplePropKey<ListViewItemEventArgs->unit> = SimplePropKey.create "comboBox.selectedItemChanged"

    // TextField
    type textFieldPKeys() =
        inherit viewPKeys()
        // Properties
        member inline this.autocomplete : SimplePropKey<IAutocomplete> = SimplePropKey.create "textField.autocomplete"
        member inline this.caption : SimplePropKey<string> = SimplePropKey.create "textField.caption"
        member inline this.captionColor : SimplePropKey<Terminal.Gui.Drawing.Color> = SimplePropKey.create "textField.captionColor"
        member inline this.cursorPosition : SimplePropKey<Int32> = SimplePropKey.create "textField.cursorPosition"
        member inline this.readOnly : SimplePropKey<bool> = SimplePropKey.create "textField.readOnly"
        member inline this.secret : SimplePropKey<bool> = SimplePropKey.create "textField.secret"
        member inline this.selectedStart : SimplePropKey<Int32> = SimplePropKey.create "textField.selectedStart"
        member inline this.selectWordOnlyOnDoubleClick : SimplePropKey<bool> = SimplePropKey.create "textField.selectWordOnlyOnDoubleClick"
        member inline this.text : SimplePropKey<string> = SimplePropKey.create "textField.text"
        member inline this.used : SimplePropKey<bool> = SimplePropKey.create "textField.used"
        member inline this.useSameRuneTypeForWords : SimplePropKey<bool> = SimplePropKey.create "textField.useSameRuneTypeForWords"
        // Events
        member inline this.textChanging : SimplePropKey<ResultEventArgs<string>->unit> = SimplePropKey.create "textField.textChanging"

    // DateField
    type dateFieldPKeys() =
        inherit textFieldPKeys()
        // Properties
        member inline this.culture : SimplePropKey<CultureInfo> = SimplePropKey.create "dateField.culture"
        member inline this.cursorPosition : SimplePropKey<Int32> = SimplePropKey.create "dateField.cursorPosition"
        member inline this.date : SimplePropKey<DateTime> = SimplePropKey.create "dateField.date"
        // Events
        member inline this.dateChanged : SimplePropKey<DateTimeEventArgs<DateTime>->unit> = SimplePropKey.create "dateField.dateChanged"

    // DatePicker
    type datePickerPKeys() =
        inherit viewPKeys()
        // Properties
        member inline this.culture : SimplePropKey<CultureInfo> = SimplePropKey.create "datePicker.culture"
        member inline this.date : SimplePropKey<DateTime> = SimplePropKey.create "datePicker.date"

    // Toplevel
    type toplevelPKeys() =
        inherit viewPKeys()
        // Properties
        member inline this.modal : SimplePropKey<bool> = SimplePropKey.create "toplevel.modal"
        member inline this.running : SimplePropKey<bool> = SimplePropKey.create "toplevel.running"
        // Events
        member inline this.activate : SimplePropKey<ToplevelEventArgs->unit> = SimplePropKey.create "toplevel.activate"
        member inline this.closed : SimplePropKey<ToplevelEventArgs->unit> = SimplePropKey.create "toplevel.closed"
        member inline this.closing : SimplePropKey<ToplevelClosingEventArgs->unit> = SimplePropKey.create "toplevel.closing"
        member inline this.deactivate : SimplePropKey<ToplevelEventArgs->unit> = SimplePropKey.create "toplevel.deactivate"
        member inline this.loaded : SimplePropKey<unit->unit> = SimplePropKey.create "toplevel.loaded"
        member inline this.ready : SimplePropKey<unit->unit> = SimplePropKey.create "toplevel.ready"
        member inline this.sizeChanging : SimplePropKey<SizeChangedEventArgs->unit> = SimplePropKey.create "toplevel.sizeChanging"
        member inline this.unloaded : SimplePropKey<unit->unit> = SimplePropKey.create "toplevel.unloaded"

    // Dialog
    type dialogPKeys() =
        inherit toplevelPKeys()
        // Properties
        member inline this.buttonAlignment : SimplePropKey<Alignment> = SimplePropKey.create "dialog.buttonAlignment"
        member inline this.buttonAlignmentModes : SimplePropKey<AlignmentModes> = SimplePropKey.create "dialog.buttonAlignmentModes"
        member inline this.canceled : SimplePropKey<bool> = SimplePropKey.create "dialog.canceled"

    // FileDialog
    type fileDialogPKeys() =
        inherit dialogPKeys()
        // Properties
        member inline this.allowedTypes : SimplePropKey<IAllowedType list> = SimplePropKey.create "fileDialog.allowedTypes"
        member inline this.allowsMultipleSelection : SimplePropKey<bool> = SimplePropKey.create "fileDialog.allowsMultipleSelection"
        member inline this.fileOperationsHandler : SimplePropKey<IFileOperations> = SimplePropKey.create "fileDialog.fileOperationsHandler"
        member inline this.mustExist : SimplePropKey<bool> = SimplePropKey.create "fileDialog.mustExist"
        member inline this.openMode : SimplePropKey<OpenMode> = SimplePropKey.create "fileDialog.openMode"
        member inline this.path : SimplePropKey<string> = SimplePropKey.create "fileDialog.path"
        member inline this.searchMatcher : SimplePropKey<ISearchMatcher> = SimplePropKey.create "fileDialog.searchMatcher"
        // Events
        member inline this.filesSelected : SimplePropKey<FilesSelectedEventArgs->unit> = SimplePropKey.create "fileDialog.filesSelected"

    /// SaveDialog
    type saveDialogPKeys() =
        inherit fileDialogPKeys()

    // FrameView
    type frameViewPKeys() =
        inherit viewPKeys()
    // No properties or events FrameView

    // GraphView
    type graphViewPKeys() =
        inherit viewPKeys()

        // Properties
        member inline this.axisX : SimplePropKey<HorizontalAxis> = SimplePropKey.create "graphView.axisX"
        member inline this.axisY : SimplePropKey<VerticalAxis> = SimplePropKey.create "graphView.axisY"
        member inline this.cellSize : SimplePropKey<PointF> = SimplePropKey.create "graphView.cellSize"
        member inline this.graphColor : SimplePropKey<Attribute option> = SimplePropKey.create "graphView.graphColor"
        member inline this.marginBottom : SimplePropKey<int> = SimplePropKey.create "graphView.marginBottom"
        member inline this.marginLeft : SimplePropKey<int> = SimplePropKey.create "graphView.marginLeft"
        member inline this.scrollOffset : SimplePropKey<PointF> = SimplePropKey.create "graphView.scrollOffset"

    // HexView
    type hexViewPKeys() =
        inherit viewPKeys()

        // Properties
        member inline this.address : SimplePropKey<Int64> = SimplePropKey.create "hexView.address"
        member inline this.addressWidth : SimplePropKey<int> = SimplePropKey.create "hexView.addressWidth"
        member inline this.allowEdits : SimplePropKey<int> = SimplePropKey.create "hexView.allowEdits"
        member inline this.readOnly : SimplePropKey<bool> = SimplePropKey.create "hexView.readOnly"
        member inline this.source : SimplePropKey<Stream> = SimplePropKey.create "hexView.source"
        // Events
        member inline this.edited : SimplePropKey<HexViewEditEventArgs->unit> = SimplePropKey.create "hexView.edited"
        member inline this.positionChanged : SimplePropKey<HexViewEventArgs->unit> = SimplePropKey.create "hexView.positionChanged"

    // Label
    type labelPKeys() =
        inherit viewPKeys()

        // Properties
        member inline this.hotKeySpecifier : SimplePropKey<Rune> = SimplePropKey.create "label.hotKeySpecifier"
        member inline this.text : SimplePropKey<string> = SimplePropKey.create "label.text"

    // LegendAnnotation
    type legendAnnotationPKeys() =
        inherit viewPKeys()
    // No properties or events LegendAnnotation

    // Line
    type linePKeys() =
        inherit viewPKeys()

        // Properties
        member inline this.orientation : SimplePropKey<Orientation> = SimplePropKey.create "line.orientation"
        // Events
        member inline this.orientationChanged : SimplePropKey<Orientation->unit> = SimplePropKey.create "line.orientationChanged"
        member inline this.orientationChanging : SimplePropKey<App.CancelEventArgs<Orientation>->unit> = SimplePropKey.create "line.orientationChanging"

    // LineView
    type lineViewPKeys() =
        inherit viewPKeys()

        // Properties
        member inline this.endingAnchor : SimplePropKey<Rune option> = SimplePropKey.create "lineView.endingAnchor"
        member inline this.lineRune : SimplePropKey<Rune> = SimplePropKey.create "lineView.lineRune"
        member inline this.orientation : SimplePropKey<Orientation> = SimplePropKey.create "lineView.orientation"
        member inline this.startingAnchor : SimplePropKey<Rune option> = SimplePropKey.create "lineView.startingAnchor"

    // ListView
    type listViewPKeys() =
        inherit viewPKeys()

        // Properties
        member inline this.allowsMarking : SimplePropKey<bool> = SimplePropKey.create "listView.allowsMarking"
        member inline this.allowsMultipleSelection : SimplePropKey<bool> = SimplePropKey.create "listView.allowsMultipleSelection"
        member inline this.leftItem : SimplePropKey<Int32> = SimplePropKey.create "listView.leftItem"
        member inline this.selectedItem : SimplePropKey<Int32> = SimplePropKey.create "listView.selectedItem"
        member inline this.source : SimplePropKey<string list> = SimplePropKey.create "listView.source"
        member inline this.topItem : SimplePropKey<Int32> = SimplePropKey.create "listView.topItem"
        // Events
        member inline this.collectionChanged : SimplePropKey<NotifyCollectionChangedEventArgs->unit> = SimplePropKey.create "listView.collectionChanged"
        member inline this.openSelectedItem : SimplePropKey<ListViewItemEventArgs->unit> = SimplePropKey.create "listView.openSelectedItem"
        member inline this.rowRender : SimplePropKey<ListViewRowEventArgs->unit> = SimplePropKey.create "listView.rowRender"
        member inline this.selectedItemChanged : SimplePropKey<ListViewItemEventArgs->unit> = SimplePropKey.create "listView.selectedItemChanged"

    // Margin
    type marginPKeys() =
        inherit adornmentPKeys()

        // Properties
        member inline this.shadowStyle : SimplePropKey<ShadowStyle> = SimplePropKey.create "margin.shadowStyle"

    type menuv2PKeys() =
        inherit barPKeys()

        // Properties
        member inline this.selectedMenuItem : SimplePropKey<MenuItemv2> = SimplePropKey.create "menuv2.selectedMenuItem"
        member inline this.superMenuItem : SimplePropKey<MenuItemv2> = SimplePropKey.create "menuv2.superMenuItem"
        // Events
        member inline this.accepted : SimplePropKey< CommandEventArgs->unit> = SimplePropKey.create "menuv2.accepted"
        member inline this.selectedMenuItemChanged : SimplePropKey< MenuItemv2->unit> = SimplePropKey.create "menuv2.selectedMenuItemChanged"

    // MenuBarV2
    type menuBarv2PKeys() =
        inherit menuv2PKeys()

        // Properties
        member inline this.key : SimplePropKey<Key> = SimplePropKey.create "menuBarv2.key"
        member inline this.menus : SimplePropKey<MenuBarItemv2 array> = SimplePropKey.create "view.menus"
        // Events
        member inline this.keyChanged : SimplePropKey<KeyChangedEventArgs->unit> = SimplePropKey.create "menuBarv2.keyChanged"

    type shortcutPKeys() =
         inherit viewPKeys()

         // Properties
         member inline this.action : SimplePropKey<Action> = SimplePropKey.create "shortcut.action"
         member inline this.alignmentModes : SimplePropKey<AlignmentModes> = SimplePropKey.create "shortcut.alignmentModes"
         member inline this.commandView : SimplePropKey<View> = SimplePropKey.create "shortcut.commandView_view"
         member inline this.commandView_element : SingleElementPropKey<ITerminalElement> = SingleElementPropKey.create "shortcut.commandView_element"
         member inline this.forceFocusColors : SimplePropKey<bool> = SimplePropKey.create "shortcut.forceFocusColors"
         member inline this.helpText : SimplePropKey<string> = SimplePropKey.create "shortcut.helpText"
         member inline this.text : SimplePropKey<string> = SimplePropKey.create "shortcut.text"
         member inline this.bindKeyToApplication : SimplePropKey<bool> = SimplePropKey.create "shortcut.bindKeyToApplication"
         member inline this.key : SimplePropKey<Key> = SimplePropKey.create "shortcut.key"
         member inline this.minimumKeyTextSize : SimplePropKey<Int32> = SimplePropKey.create "shortcut.minimumKeyTextSize"
         // Events
         member inline this.orientationChanged : SimplePropKey<Orientation->unit> = SimplePropKey.create "shortcut.orientationChanged"
         member inline this.orientationChanging : SimplePropKey<CancelEventArgs<Orientation>->unit> = SimplePropKey.create "shortcut.orientationChanging"

    type menuItemv2PKeys() =
        inherit shortcutPKeys()
        member inline this.command : SimplePropKey< Command> = SimplePropKey.create "menuItemv2.command"
        member inline this.subMenu: SimplePropKey<Menuv2> = SimplePropKey.create "menuItemv2.subMenu_view"
        member inline this.subMenu_element : SingleElementPropKey<IMenuv2Element> = SingleElementPropKey.create "menuItemv2.subMenu_element"
        member inline this.targetView : SimplePropKey<View> = SimplePropKey.create "menuItemv2.targetView"
        member inline this.accepted: SimplePropKey<CommandEventArgs -> unit> = SimplePropKey.create "menuItemv2.accepted"

    type menuBarItemv2PKeys() =
        inherit menuItemv2PKeys()

        // Properties
        member inline this.popoverMenu : SimplePropKey<PopoverMenu> = SimplePropKey.create "menuBarItemv2.popoverMenu_view"
        member inline this.popoverMenu_element : SingleElementPropKey<IPopoverMenuElement> = SingleElementPropKey.create "menuBarItemv2.popoverMenu_element"
        member inline this.popoverMenuOpen : SimplePropKey<bool> = SimplePropKey.create "menuBarItemv2.popoverMenuOpen"
        // Events
        member inline this.popoverMenuOpenChanged : SimplePropKey<bool->unit> = SimplePropKey.create "menuBarItemv2.popoverMenuOpenChanged"

    type popoverMenuPKeys() =
        inherit viewPKeys()

        // Properties
        member inline this.key : SimplePropKey<Key> = SimplePropKey.create "popoverMenu.key"
        member inline this.mouseFlags : SimplePropKey<MouseFlags> = SimplePropKey.create "popoverMenu.mouseFlags"
        member inline this.root : SimplePropKey<Menuv2> = SimplePropKey.create "popoverMenu.root_view"
        member inline this.root_element : SingleElementPropKey<IMenuv2Element> = SingleElementPropKey.create "popoverMenu.root_element"
        // Events
        member inline this.accepted : SimplePropKey<CommandEventArgs->unit> = SimplePropKey.create "popoverMenu.accepted"
        member inline this.keyChanged : SimplePropKey<KeyChangedEventArgs->unit> = SimplePropKey.create "popoverMenu.keyChanged"

    // NumericUpDown`1
    type numericUpDownPKeys<'a>() =
        inherit viewPKeys()

        // Properties
        member inline this.format : SimplePropKey<string> = SimplePropKey.create "numericUpDown.format"
        member inline this.increment : SimplePropKey<'a> = SimplePropKey.create "numericUpDown.increment"
        member inline this.value : SimplePropKey<'a> = SimplePropKey.create "numericUpDown.value"
        // Events
        member inline this.formatChanged : SimplePropKey<string->unit> = SimplePropKey.create "numericUpDown.formatChanged"
        member inline this.incrementChanged : SimplePropKey<'a->unit> = SimplePropKey.create "numericUpDown.incrementChanged"
        member inline this.valueChanged : SimplePropKey<'a->unit> = SimplePropKey.create "numericUpDown.valueChanged"
        member inline this.valueChanging : SimplePropKey<App.CancelEventArgs<'a>->unit> = SimplePropKey.create "numericUpDown.valueChanging"

    // NumericUpDown
    // type numericUpDownPKeys() =
    //     inherit numericUpDownPKeys<int>()
    // No properties or events NumericUpDown

    // OpenDialog
    type openDialogPKeys() =
        inherit fileDialogPKeys()
        // Properties
        member inline this.openMode : SimplePropKey<OpenMode> = SimplePropKey.create "openDialog.openMode"

    // OptionSelector
    type optionSelectorPKeys() =
        inherit viewPKeys()
        //Properties
        member inline this.assignHotKeysToCheckBoxes : SimplePropKey<bool> = SimplePropKey.create "optionSelector.assignHotKeysToCheckBoxes"
        member inline this.orientation : SimplePropKey<Orientation> = SimplePropKey.create "optionSelector.orientation"
        member inline this.options : SimplePropKey<IReadOnlyList<string>> = SimplePropKey.create "optionSelector.options"
        member inline this.selectedItem : SimplePropKey<Int32> = SimplePropKey.create "optionSelector.selectedItem"
        // Events
        member inline this.orientationChanged : SimplePropKey<Orientation->unit> = SimplePropKey.create "optionSelector.orientationChanged"
        member inline this.orientationChanging : SimplePropKey<CancelEventArgs<Orientation>->unit> = SimplePropKey.create "optionSelector.orientationChanging"
        member inline this.selectedItemChanged : SimplePropKey<SelectedItemChangedArgs->unit> = SimplePropKey.create "optionSelector.selectedItemChanged"

    // Padding
    type paddingPKeys() =
        inherit adornmentPKeys()

    // ProgressBar
    type progressBarPKeys() =
        inherit viewPKeys()

        // Properties
        member inline this.bidirectionalMarquee : SimplePropKey<bool> = SimplePropKey.create "progressBar.bidirectionalMarquee"
        member inline this.fraction : SimplePropKey<Single> = SimplePropKey.create "progressBar.fraction"
        member inline this.progressBarFormat : SimplePropKey<ProgressBarFormat> = SimplePropKey.create "progressBar.progressBarFormat"
        member inline this.progressBarStyle : SimplePropKey<ProgressBarStyle> = SimplePropKey.create "progressBar.progressBarStyle"
        member inline this.segmentCharacter : SimplePropKey<Rune> = SimplePropKey.create "progressBar.segmentCharacter"
        member inline this.text : SimplePropKey<string> = SimplePropKey.create "progressBar.text"

    // RadioGroup
    type radioGroupPKeys() =
        inherit viewPKeys()

        // Properties
        member inline this.assignHotKeysToRadioLabels : SimplePropKey<bool> = SimplePropKey.create "radioGroup.assignHotKeysToRadioLabels"
        member inline this.cursor : SimplePropKey<Int32> = SimplePropKey.create "radioGroup.cursor"
        member inline this.doubleClickAccepts : SimplePropKey<bool> = SimplePropKey.create "radioGroup.doubleClickAccepts"
        member inline this.horizontalSpace : SimplePropKey<Int32> = SimplePropKey.create "radioGroup.horizontalSpace"
        member inline this.orientation : SimplePropKey<Orientation> = SimplePropKey.create "radioGroup.orientation"
        member inline this.radioLabels : SimplePropKey<string list> = SimplePropKey.create "radioGroup.radioLabels"
        member inline this.selectedItem : SimplePropKey<Int32> = SimplePropKey.create "radioGroup.selectedItem"
        // Events
        member inline this.orientationChanged : SimplePropKey<Orientation->unit> = SimplePropKey.create "radioGroup.orientationChanged"
        member inline this.orientationChanging : SimplePropKey<App.CancelEventArgs<Orientation>->unit> = SimplePropKey.create "radioGroup.orientationChanging"
        member inline this.selectedItemChanged : SimplePropKey<SelectedItemChangedArgs->unit> = SimplePropKey.create "radioGroup.selectedItemChanged"

    // SaveDialog
    // No properties or events SaveDialog

    // ScrollBar
    type scrollBarPKeys() =
        inherit viewPKeys()

        // Properties
        member inline this.autoShow : SimplePropKey<bool> = SimplePropKey.create "scrollBar.autoShow"
        member inline this.increment : SimplePropKey<Int32> = SimplePropKey.create "scrollBar.increment"
        member inline this.orientation : SimplePropKey<Orientation> = SimplePropKey.create "scrollBar.orientation"
        member inline this.position : SimplePropKey<Int32> = SimplePropKey.create "scrollBar.position"
        member inline this.scrollableContentSize : SimplePropKey<Int32> = SimplePropKey.create "scrollBar.scrollableContentSize"
        member inline this.visibleContentSize : SimplePropKey<Int32> = SimplePropKey.create "scrollBar.visibleContentSize"
        // Events
        member inline this.orientationChanged : SimplePropKey<Orientation->unit> = SimplePropKey.create "scrollBar.orientationChanged"
        member inline this.orientationChanging : SimplePropKey<CancelEventArgs<Orientation>->unit> = SimplePropKey.create "scrollBar.orientationChanging"
        member inline this.scrollableContentSizeChanged : SimplePropKey<EventArgs<Int32>->unit> = SimplePropKey.create "scrollBar.scrollableContentSizeChanged"
        member inline this.sliderPositionChanged : SimplePropKey<EventArgs<Int32>->unit> = SimplePropKey.create "scrollBar.sliderPositionChanged"

    // ScrollSlider
    type scrollSliderPKeys() =
        inherit viewPKeys()

        // Properties
        member inline this.orientation : SimplePropKey<Orientation> = SimplePropKey.create "scrollSlider.orientation"
        member inline this.position : SimplePropKey<Int32> = SimplePropKey.create "scrollSlider.position"
        member inline this.size : SimplePropKey<Int32> = SimplePropKey.create "scrollSlider.size"
        member inline this.sliderPadding : SimplePropKey<Int32> = SimplePropKey.create "scrollSlider.sliderPadding"
        member inline this.visibleContentSize : SimplePropKey<Int32> = SimplePropKey.create "scrollSlider.visibleContentSize"
        // Events
        member inline this.orientationChanged : SimplePropKey<Orientation->unit> = SimplePropKey.create "scrollSlider.orientationChanged"
        member inline this.orientationChanging : SimplePropKey<CancelEventArgs<Orientation>->unit> = SimplePropKey.create "scrollSlider.orientationChanging"
        member inline this.positionChanged : SimplePropKey<EventArgs<Int32>->unit> = SimplePropKey.create "scrollSlider.positionChanged"
        member inline this.positionChanging : SimplePropKey<CancelEventArgs<Int32>->unit> = SimplePropKey.create "scrollSlider.positionChanging"
        member inline this.scrolled : SimplePropKey<EventArgs<Int32>->unit> = SimplePropKey.create "scrollSlider.scrolled"

    // Slider`1
    type sliderPKeys<'a>() =
        inherit viewPKeys()

        // Properties
        member inline this.allowEmpty : SimplePropKey<bool> = SimplePropKey.create "slider.allowEmpty"
        member inline this.focusedOption : SimplePropKey<Int32> = SimplePropKey.create "slider.focusedOption"
        member inline this.legendsOrientation : SimplePropKey<Orientation> = SimplePropKey.create "slider.legendsOrientation"
        member inline this.minimumInnerSpacing : SimplePropKey<Int32> = SimplePropKey.create "slider.minimumInnerSpacing"
        member inline this.options : SimplePropKey<SliderOption<'a> list> = SimplePropKey.create "slider.options"
        member inline this.orientation : SimplePropKey<Orientation> = SimplePropKey.create "slider.orientation"
        member inline this.rangeAllowSingle : SimplePropKey<bool> = SimplePropKey.create "slider.rangeAllowSingle"
        member inline this.showEndSpacing : SimplePropKey<bool> = SimplePropKey.create "slider.showEndSpacing"
        member inline this.showLegends : SimplePropKey<bool> = SimplePropKey.create "slider.showLegends"
        member inline this.style : SimplePropKey<SliderStyle> = SimplePropKey.create "slider.style"
        member inline this.text : SimplePropKey<string> = SimplePropKey.create "slider.text"
        member inline this.``type`` : SimplePropKey<SliderType> = SimplePropKey.create "slider.``type``"
        member inline this.useMinimumSize : SimplePropKey<bool> = SimplePropKey.create "slider.useMinimumSize"
        // Events
        member inline this.optionFocused : SimplePropKey<SliderEventArgs<'a>->unit> = SimplePropKey.create "slider.optionFocused"
        member inline this.optionsChanged : SimplePropKey<SliderEventArgs<'a>->unit> = SimplePropKey.create "slider.optionsChanged"
        member inline this.orientationChanged : SimplePropKey<Orientation->unit> = SimplePropKey.create "slider.orientationChanged"
        member inline this.orientationChanging : SimplePropKey<App.CancelEventArgs<Orientation>->unit> = SimplePropKey.create "slider.orientationChanging"

    // Slider
    // type sliderPKeys() =
    //     inherit sliderPKeys<obj>()
    // No properties or events Slider

    // SpinnerView
    type spinnerViewPKeys() =
        inherit viewPKeys()

        // Properties
        member inline this.autoSpin : SimplePropKey<bool> = SimplePropKey.create "spinnerView.autoSpin"
        member inline this.sequence : SimplePropKey<string list> = SimplePropKey.create "spinnerView.sequence"
        member inline this.spinBounce : SimplePropKey<bool> = SimplePropKey.create "spinnerView.spinBounce"
        member inline this.spinDelay : SimplePropKey<Int32> = SimplePropKey.create "spinnerView.spinDelay"
        member inline this.spinReverse : SimplePropKey<bool> = SimplePropKey.create "spinnerView.spinReverse"
        member inline this.style : SimplePropKey<SpinnerStyle> = SimplePropKey.create "spinnerView.style"

    // StatusBar
    type statusBarPKeys() =
        inherit barPKeys()
    // No properties or events StatusBar

    // Tab
    type tabPKeys() =
        inherit viewPKeys()

        // Properties
        member inline this.displayText : SimplePropKey<string> = SimplePropKey.create "tab.displayText"
        member inline this.view : SimplePropKey<View> = SimplePropKey.create "tab.view_view"
        member inline this.view_element : SingleElementPropKey<ITerminalElement> = SingleElementPropKey.create "tab.view_element"

    // TabView
    type tabViewPKeys() =
        inherit viewPKeys()

        // Properties
        member inline this.maxTabTextWidth : SimplePropKey<int> = SimplePropKey.create "tabView.maxTabTextWidth"
        member inline this.selectedTab : SimplePropKey<Tab> = SimplePropKey.create "tabView.selectedTab"
        member inline this.style : SimplePropKey<TabStyle> = SimplePropKey.create "tabView.style"
        member inline this.tabScrollOffset : SimplePropKey<Int32> = SimplePropKey.create "tabView.tabScrollOffset"
        // Events
        member inline this.selectedTabChanged : SimplePropKey<TabChangedEventArgs->unit> = SimplePropKey.create "tabView.selectedTabChanged"
        member inline this.tabClicked : SimplePropKey<TabMouseEventArgs->unit> = SimplePropKey.create "tabView.tabClicked"
        // Additional properties
        member inline this.tabs : SimplePropKey<List<ITerminalElement>> = SimplePropKey.create "tabView.tabs_view"
        member inline this.tabs_elements : MultiElementPropKey<List<ITerminalElement>> = MultiElementPropKey.create "tabView.tabs_elements"

    // TableView
    type tableViewPKeys() =
        inherit viewPKeys()

        // Properties
        member inline this.cellActivationKey : SimplePropKey<KeyCode> = SimplePropKey.create "tableView.cellActivationKey"
        member inline this.collectionNavigator : SimplePropKey<ICollectionNavigator> = SimplePropKey.create "tableView.collectionNavigator"
        member inline this.columnOffset : SimplePropKey<Int32> = SimplePropKey.create "tableView.columnOffset"
        member inline this.fullRowSelect : SimplePropKey<bool> = SimplePropKey.create "tableView.fullRowSelect"
        member inline this.maxCellWidth : SimplePropKey<Int32> = SimplePropKey.create "tableView.maxCellWidth"
        member inline this.minCellWidth : SimplePropKey<Int32> = SimplePropKey.create "tableView.minCellWidth"
        member inline this.multiSelect : SimplePropKey<bool> = SimplePropKey.create "tableView.multiSelect"
        member inline this.nullSymbol : SimplePropKey<string> = SimplePropKey.create "tableView.nullSymbol"
        member inline this.rowOffset : SimplePropKey<Int32> = SimplePropKey.create "tableView.rowOffset"
        member inline this.selectedColumn : SimplePropKey<Int32> = SimplePropKey.create "tableView.selectedColumn"
        member inline this.selectedRow : SimplePropKey<Int32> = SimplePropKey.create "tableView.selectedRow"
        member inline this.separatorSymbol : SimplePropKey<Char> = SimplePropKey.create "tableView.separatorSymbol"
        member inline this.style : SimplePropKey<TableStyle> = SimplePropKey.create "tableView.style"
        member inline this.table : SimplePropKey<ITableSource> = SimplePropKey.create "tableView.table"
        // Events
        member inline this.cellActivated : SimplePropKey<CellActivatedEventArgs->unit> = SimplePropKey.create "tableView.cellActivated"
        member inline this.cellToggled : SimplePropKey<CellToggledEventArgs->unit> = SimplePropKey.create "tableView.cellToggled"
        member inline this.selectedCellChanged : SimplePropKey<SelectedCellChangedEventArgs->unit> = SimplePropKey.create "tableView.selectedCellChanged"

    // TextValidateField
    type textValidateFieldPKeys() =
        inherit viewPKeys()

        // Properties
        member inline this.provider : SimplePropKey<ITextValidateProvider> = SimplePropKey.create "textValidateField.provider"
        member inline this.text : SimplePropKey<string> = SimplePropKey.create "textValidateField.text"

    // TextView
    type textViewPKeys() =
        inherit viewPKeys()

        // Properties
        member inline this.allowsReturn : SimplePropKey<bool> = SimplePropKey.create "textView.allowsReturn"
        member inline this.allowsTab : SimplePropKey<bool> = SimplePropKey.create "textView.allowsTab"
        member inline this.cursorPosition : SimplePropKey<Point> = SimplePropKey.create "textView.cursorPosition"
        member inline this.inheritsPreviousAttribute : SimplePropKey<bool> = SimplePropKey.create "textView.inheritsPreviousAttribute"
        member inline this.isDirty : SimplePropKey<bool> = SimplePropKey.create "textView.isDirty"
        member inline this.isSelecting : SimplePropKey<bool> = SimplePropKey.create "textView.isSelecting"
        member inline this.leftColumn : SimplePropKey<Int32> = SimplePropKey.create "textView.leftColumn"
        member inline this.multiline : SimplePropKey<bool> = SimplePropKey.create "textView.multiline"
        member inline this.readOnly : SimplePropKey<bool> = SimplePropKey.create "textView.readOnly"
        member inline this.selectionStartColumn : SimplePropKey<Int32> = SimplePropKey.create "textView.selectionStartColumn"
        member inline this.selectionStartRow : SimplePropKey<Int32> = SimplePropKey.create "textView.selectionStartRow"
        member inline this.selectWordOnlyOnDoubleClick : SimplePropKey<bool> = SimplePropKey.create "textView.selectWordOnlyOnDoubleClick"
        member inline this.tabWidth : SimplePropKey<Int32> = SimplePropKey.create "textView.tabWidth"
        member inline this.text : SimplePropKey<string> = SimplePropKey.create "textView.text"
        member inline this.topRow : SimplePropKey<Int32> = SimplePropKey.create "textView.topRow"
        member inline this.used : SimplePropKey<bool> = SimplePropKey.create "textView.used"
        member inline this.useSameRuneTypeForWords : SimplePropKey<bool> = SimplePropKey.create "textView.useSameRuneTypeForWords"
        member inline this.wordWrap : SimplePropKey<bool> = SimplePropKey.create "textView.wordWrap"
        // Events
        member inline this.contentsChanged : SimplePropKey<ContentsChangedEventArgs->unit> = SimplePropKey.create "textView.contentsChanged"
        member inline this.drawNormalColor : SimplePropKey<CellEventArgs->unit> = SimplePropKey.create "textView.drawNormalColor"
        member inline this.drawReadOnlyColor : SimplePropKey<CellEventArgs->unit> = SimplePropKey.create "textView.drawReadOnlyColor"
        member inline this.drawSelectionColor : SimplePropKey<CellEventArgs->unit> = SimplePropKey.create "textView.drawSelectionColor"
        member inline this.drawUsedColor : SimplePropKey<CellEventArgs->unit> = SimplePropKey.create "textView.drawUsedColor"
        member inline this.unwrappedCursorPosition : SimplePropKey<Point->unit> = SimplePropKey.create "textView.unwrappedCursorPosition"
        // Additional properties
        member inline this.textChanged : SimplePropKey<string->unit> = SimplePropKey.create "textView.textChanged"

    // TileView
    type tileViewPKeys() =
        inherit viewPKeys()

        // Properties
        member inline this.lineStyle : SimplePropKey<LineStyle> = SimplePropKey.create "tileView.lineStyle"
        member inline this.orientation : SimplePropKey<Orientation> = SimplePropKey.create "tileView.orientation"
        member inline this.toggleResizable : SimplePropKey<KeyCode> = SimplePropKey.create "tileView.toggleResizable"
        // Events
        member inline this.splitterMoved : SimplePropKey<SplitterEventArgs->unit> = SimplePropKey.create "tileView.splitterMoved"

    // TimeField
    type timeFieldPKeys() =
        inherit textFieldPKeys()

        // Properties
        member inline this.cursorPosition : SimplePropKey<Int32> = SimplePropKey.create "timeField.cursorPosition"
        member inline this.isShortFormat : SimplePropKey<bool> = SimplePropKey.create "timeField.isShortFormat"
        member inline this.time : SimplePropKey<TimeSpan> = SimplePropKey.create "timeField.time"
        // Events
        member inline this.timeChanged : SimplePropKey<DateTimeEventArgs<TimeSpan>->unit> = SimplePropKey.create "timeField.timeChanged"

    // TreeView`1
    type treeViewPKeys<'a when 'a : not struct>() =
        inherit viewPKeys()

        // Properties
        member inline this.allowLetterBasedNavigation : SimplePropKey<bool> = SimplePropKey.create "treeView.allowLetterBasedNavigation"
        member inline this.aspectGetter : SimplePropKey<AspectGetterDelegate<'a>> = SimplePropKey.create "treeView.aspectGetter"
        member inline this.colorGetter : SimplePropKey<Func<'a,Scheme>> = SimplePropKey.create "treeView.colorGetter"
        member inline this.maxDepth : SimplePropKey<Int32> = SimplePropKey.create "treeView.maxDepth"
        member inline this.multiSelect : SimplePropKey<bool> = SimplePropKey.create "treeView.multiSelect"
        member inline this.objectActivationButton : SimplePropKey<MouseFlags option> = SimplePropKey.create "treeView.objectActivationButton"
        member inline this.objectActivationKey : SimplePropKey<KeyCode> = SimplePropKey.create "treeView.objectActivationKey"
        member inline this.scrollOffsetHorizontal : SimplePropKey<Int32> = SimplePropKey.create "treeView.scrollOffsetHorizontal"
        member inline this.scrollOffsetVertical : SimplePropKey<Int32> = SimplePropKey.create "treeView.scrollOffsetVertical"
        member inline this.selectedObject : SimplePropKey<'a> = SimplePropKey.create "treeView.selectedObject"
        member inline this.style : SimplePropKey<TreeStyle> = SimplePropKey.create "treeView.style"
        member inline this.treeBuilder : SimplePropKey<ITreeBuilder<'a>> = SimplePropKey.create "treeView.treeBuilder"
        // Events
        member inline this.drawLine : SimplePropKey<DrawTreeViewLineEventArgs<'a>->unit> = SimplePropKey.create "treeView.drawLine"
        member inline this.objectActivated : SimplePropKey<ObjectActivatedEventArgs<'a>->unit> = SimplePropKey.create "treeView.objectActivated"
        member inline this.selectionChanged : SimplePropKey<SelectionChangedEventArgs<'a>->unit> = SimplePropKey.create "treeView.selectionChanged"

    // TreeView
    // type treeViewPKeys() =
    //     inherit treeViewPKeys<ITreeNode>()
    // No properties or events TreeView

    // Window
    type windowPKeys() =
        inherit toplevelPKeys()
    // No properties or events Window

    // Wizard
    type wizardPKeys() =
        inherit dialogPKeys()

        // Properties
        member inline this.currentStep : SimplePropKey<WizardStep> = SimplePropKey.create "wizard.currentStep"
        member inline this.modal : SimplePropKey<bool> = SimplePropKey.create "wizard.modal"
        // Events
        member inline this.cancelled : SimplePropKey<WizardButtonEventArgs->unit> = SimplePropKey.create "wizard.cancelled"
        member inline this.finished : SimplePropKey<WizardButtonEventArgs->unit> = SimplePropKey.create "wizard.finished"
        member inline this.movingBack : SimplePropKey<WizardButtonEventArgs->unit> = SimplePropKey.create "wizard.movingBack"
        member inline this.movingNext : SimplePropKey<WizardButtonEventArgs->unit> = SimplePropKey.create "wizard.movingNext"
        member inline this.stepChanged : SimplePropKey<StepChangeEventArgs->unit> = SimplePropKey.create "wizard.stepChanged"
        member inline this.stepChanging : SimplePropKey<StepChangeEventArgs->unit> = SimplePropKey.create "wizard.stepChanging"

    // WizardStep
    type wizardStepPKeys() =
        inherit viewPKeys()

        // Properties
        member inline this.backButtonText : SimplePropKey<string> = SimplePropKey.create "wizardStep.backButtonText"
        member inline this.helpText : SimplePropKey<string> = SimplePropKey.create "wizardStep.helpText"
        member inline this.nextButtonText : SimplePropKey<string> = SimplePropKey.create "wizardStep.nextButtonText"


    let view = viewPKeys()
    let adornment = adornmentPKeys()
    let bar = barPKeys()
    let border = borderPKeys()
    let button = buttonPKeys()
    let checkBox = checkBoxPKeys()
    let colorPicker = colorPickerPKeys()
    let colorPicker16 = colorPicker16PKeys()
    let comboBox = comboBoxPKeys()
    let textField = textFieldPKeys()
    let dateField = dateFieldPKeys()
    let datePicker = datePickerPKeys()
    let toplevel = toplevelPKeys()
    let dialog = dialogPKeys()
    let fileDialog = fileDialogPKeys()
    let saveDialog = saveDialogPKeys()
    let frameView = frameViewPKeys()
    let graphView = graphViewPKeys()
    let hexView = hexViewPKeys()
    let label = labelPKeys()
    let legendAnnotation = legendAnnotationPKeys()
    let line = linePKeys()
    let lineView = lineViewPKeys()
    let listView = listViewPKeys()
    let margin = marginPKeys()
    let menuv2 = menuv2PKeys()
    let menuBarv2 = menuBarv2PKeys()
    let shortcut = shortcutPKeys()
    let menuItemv2 = menuItemv2PKeys()
    let menuBarItemv2 = menuBarItemv2PKeys()
    let popoverMenu = popoverMenuPKeys()
    let numericUpDown<'a> = numericUpDownPKeys<'a>()
    let openDialog = openDialogPKeys()
    let optionSelector = optionSelectorPKeys()
    let padding = paddingPKeys()
    let progressBar = progressBarPKeys()
    let radioGroup = radioGroupPKeys()
    let scrollBar = scrollBarPKeys()
    let scrollSlider = scrollSliderPKeys()
    let slider<'a> = sliderPKeys<'a>()
    let spinnerView = spinnerViewPKeys()
    let statusBar = statusBarPKeys()
    let tab = tabPKeys()
    let tabView = tabViewPKeys()
    let tableView = tableViewPKeys()
    let textValidateField = textValidateFieldPKeys()
    let textView = textViewPKeys()
    let tileView = tileViewPKeys()
    let timeField = timeFieldPKeys()
    let treeView<'a when 'a : not struct> = treeViewPKeys<'a>()
    let window = windowPKeys()
    let wizard = wizardPKeys()
    let wizardStep = wizardStepPKeys()
