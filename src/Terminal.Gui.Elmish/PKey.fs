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
        member inline this.children : SimplePropertyKey<System.Collections.Generic.List<IInternalTerminalElement>> = SimplePropertyKey.create "children"

        // Properties
        member inline this.arrangement : SimplePropertyKey<ViewArrangement> = SimplePropertyKey.create "view.arrangement"
        member inline this.borderStyle : SimplePropertyKey<LineStyle> = SimplePropertyKey.create "view.borderStyle"
        member inline this.canFocus : SimplePropertyKey<bool> = SimplePropertyKey.create "view.canFocus"
        member inline this.contentSizeTracksViewport : SimplePropertyKey<bool> = SimplePropertyKey.create "view.contentSizeTracksViewport"
        member inline this.cursorVisibility : SimplePropertyKey<CursorVisibility> = SimplePropertyKey.create "view.cursorVisibility"
        member inline this.data : SimplePropertyKey<Object> = SimplePropertyKey.create "view.data"
        member inline this.enabled : SimplePropertyKey<bool> = SimplePropertyKey.create "view.enabled"
        member inline this.frame : SimplePropertyKey<Rectangle> = SimplePropertyKey.create "view.frame"
        member inline this.hasFocus : SimplePropertyKey<bool> = SimplePropertyKey.create "view.hasFocus"
        member inline this.height : SimplePropertyKey<Dim> = SimplePropertyKey.create "view.height"
        member inline this.highlightStates : SimplePropertyKey<MouseState> = SimplePropertyKey.create "view.highlightStates"
        member inline this.hotKey : SimplePropertyKey<Key> = SimplePropertyKey.create "view.hotKey"
        member inline this.hotKeySpecifier : SimplePropertyKey<Rune> = SimplePropertyKey.create "view.hotKeySpecifier"
        member inline this.id : SimplePropertyKey<string> = SimplePropertyKey.create "view.id"
        member inline this.isInitialized : SimplePropertyKey<bool> = SimplePropertyKey.create "view.isInitialized"
        member inline this.mouseHeldDown : SimplePropertyKey<IMouseHeldDown> = SimplePropertyKey.create "view.mouseHeldDown"
        member inline this.needsDraw : SimplePropertyKey<bool> = SimplePropertyKey.create "view.needsDraw"
        member inline this.preserveTrailingSpaces : SimplePropertyKey<bool> = SimplePropertyKey.create "view.preserveTrailingSpaces"
        member inline this.schemeName : SimplePropertyKey<string> = SimplePropertyKey.create "view.schemeName"
        member inline this.shadowStyle : SimplePropertyKey<ShadowStyle> = SimplePropertyKey.create "view.shadowStyle"
        member inline this.superViewRendersLineCanvas : SimplePropertyKey<bool> = SimplePropertyKey.create "view.superViewRendersLineCanvas"
        member inline this.tabStop : SimplePropertyKey<TabBehavior option> = SimplePropertyKey.create "view.tabStop"
        member inline this.text : SimplePropertyKey<string> = SimplePropertyKey.create "view.text"
        member inline this.textAlignment : SimplePropertyKey<Alignment> = SimplePropertyKey.create "view.textAlignment"
        member inline this.textDirection : SimplePropertyKey<TextDirection> = SimplePropertyKey.create "view.textDirection"
        member inline this.title : SimplePropertyKey<string> = SimplePropertyKey.create "view.title"
        member inline this.validatePosDim : SimplePropertyKey<bool> = SimplePropertyKey.create "view.validatePosDim"
        member inline this.verticalTextAlignment : SimplePropertyKey<Alignment> = SimplePropertyKey.create "view.verticalTextAlignment"
        member inline this.viewport : SimplePropertyKey<Rectangle> = SimplePropertyKey.create "view.viewport"
        member inline this.viewportSettings : SimplePropertyKey<ViewportSettingsFlags> = SimplePropertyKey.create "view.viewportSettings"
        member inline this.visible : SimplePropertyKey<bool> = SimplePropertyKey.create "view.visible"
        member inline this.wantContinuousButtonPressed : SimplePropertyKey<bool> = SimplePropertyKey.create "view.wantContinuousButtonPressed"
        member inline this.wantMousePositionReports : SimplePropertyKey<bool> = SimplePropertyKey.create "view.wantMousePositionReports"
        member inline this.width : SimplePropertyKey<Dim> = SimplePropertyKey.create "view.width"
        member inline this.x : SimplePropertyKey<Pos> = SimplePropertyKey.create "view.x"
        member inline this.y : SimplePropertyKey<Pos> = SimplePropertyKey.create "view.y"
        // Events
        member inline this.accepting : SimplePropertyKey<HandledEventArgs->unit> = SimplePropertyKey.create "view.accepting"
        member inline this.advancingFocus : SimplePropertyKey<AdvanceFocusEventArgs->unit> = SimplePropertyKey.create "view.advancingFocus"
        member inline this.borderStyleChanged : SimplePropertyKey<EventArgs->unit> = SimplePropertyKey.create "view.borderStyleChanged"
        member inline this.canFocusChanged : SimplePropertyKey<unit->unit> = SimplePropertyKey.create "view.canFocusChanged"
        member inline this.clearedViewport : SimplePropertyKey<DrawEventArgs->unit> = SimplePropertyKey.create "view.clearedViewport"
        member inline this.clearingViewport : SimplePropertyKey<DrawEventArgs->unit> = SimplePropertyKey.create "view.clearingViewport"
        member inline this.commandNotBound : SimplePropertyKey<CommandEventArgs->unit> = SimplePropertyKey.create "view.commandNotBound"
        member inline this.contentSizeChanged : SimplePropertyKey<SizeChangedEventArgs->unit> = SimplePropertyKey.create "view.contentSizeChanged"
        member inline this.disposing : SimplePropertyKey<unit->unit> = SimplePropertyKey.create "view.disposing"
        member inline this.drawComplete : SimplePropertyKey<DrawEventArgs->unit> = SimplePropertyKey.create "view.drawComplete"
        member inline this.drawingContent : SimplePropertyKey<DrawEventArgs->unit> = SimplePropertyKey.create "view.drawingContent"
        member inline this.drawingSubViews : SimplePropertyKey<DrawEventArgs->unit> = SimplePropertyKey.create "view.drawingSubViews"
        member inline this.drawingText : SimplePropertyKey<DrawEventArgs->unit> = SimplePropertyKey.create "view.drawingText"
        member inline this.enabledChanged : SimplePropertyKey<unit->unit> = SimplePropertyKey.create "view.enabledChanged"
        member inline this.focusedChanged : SimplePropertyKey<HasFocusEventArgs->unit> = SimplePropertyKey.create "view.focusedChanged"
        member inline this.frameChanged : SimplePropertyKey<EventArgs<Rectangle>->unit> = SimplePropertyKey.create "view.frameChanged"
        member inline this.gettingAttributeForRole : SimplePropertyKey<VisualRoleEventArgs->unit> = SimplePropertyKey.create "view.gettingAttributeForRole"
        member inline this.gettingScheme : SimplePropertyKey<ResultEventArgs<Scheme>->unit> = SimplePropertyKey.create "view.gettingScheme"
        member inline this.handlingHotKey : SimplePropertyKey<CommandEventArgs->unit> = SimplePropertyKey.create "view.handlingHotKey"
        member inline this.hasFocusChanged : SimplePropertyKey<HasFocusEventArgs->unit> = SimplePropertyKey.create "view.hasFocusChanged"
        member inline this.hasFocusChanging : SimplePropertyKey<HasFocusEventArgs->unit> = SimplePropertyKey.create "view.hasFocusChanging"
        member inline this.hotKeyChanged : SimplePropertyKey<KeyChangedEventArgs->unit> = SimplePropertyKey.create "view.hotKeyChanged"
        member inline this.initialized : SimplePropertyKey<unit->unit> = SimplePropertyKey.create "view.initialized"
        member inline this.keyDown : SimplePropertyKey<Key->unit> = SimplePropertyKey.create "view.keyDown"
        member inline this.keyDownNotHandled : SimplePropertyKey<Key->unit> = SimplePropertyKey.create "view.keyDownNotHandled"
        member inline this.keyUp : SimplePropertyKey<Key->unit> = SimplePropertyKey.create "view.keyUp"
        member inline this.mouseClick : SimplePropertyKey<MouseEventArgs->unit> = SimplePropertyKey.create "view.mouseClick"
        member inline this.mouseEnter : SimplePropertyKey<CancelEventArgs->unit> = SimplePropertyKey.create "view.mouseEnter"
        member inline this.mouseEvent : SimplePropertyKey<MouseEventArgs->unit> = SimplePropertyKey.create "view.mouseEvent"
        member inline this.mouseLeave : SimplePropertyKey<EventArgs->unit> = SimplePropertyKey.create "view.mouseLeave"
        member inline this.mouseStateChanged : SimplePropertyKey<EventArgs->unit> = SimplePropertyKey.create "view.mouseStateChanged"
        member inline this.mouseWheel : SimplePropertyKey<MouseEventArgs->unit> = SimplePropertyKey.create "view.mouseWheel"
        member inline this.removed : SimplePropertyKey<SuperViewChangedEventArgs->unit> = SimplePropertyKey.create "view.removed"
        member inline this.schemeChanged : SimplePropertyKey<ValueChangedEventArgs<Scheme>->unit> = SimplePropertyKey.create "view.schemeChanged"
        member inline this.schemeChanging : SimplePropertyKey<ValueChangingEventArgs<Scheme>->unit> = SimplePropertyKey.create "view.schemeChanging"
        member inline this.schemeNameChanged : SimplePropertyKey<ValueChangedEventArgs<string>->unit> = SimplePropertyKey.create "view.schemeNameChanged"
        member inline this.schemeNameChanging : SimplePropertyKey<ValueChangingEventArgs<string>->unit> = SimplePropertyKey.create "view.schemeNameChanging"
        member inline this.selecting : SimplePropertyKey<CommandEventArgs->unit> = SimplePropertyKey.create "view.selecting"
        member inline this.subViewAdded : SimplePropertyKey<SuperViewChangedEventArgs->unit> = SimplePropertyKey.create "view.subViewAdded"
        member inline this.subViewLayout : SimplePropertyKey<LayoutEventArgs->unit> = SimplePropertyKey.create "view.subViewLayout"
        member inline this.subViewRemoved : SimplePropertyKey<SuperViewChangedEventArgs->unit> = SimplePropertyKey.create "view.subViewRemoved"
        member inline this.subViewsLaidOut : SimplePropertyKey<LayoutEventArgs->unit> = SimplePropertyKey.create "view.subViewsLaidOut"
        member inline this.superViewChanged : SimplePropertyKey<SuperViewChangedEventArgs->unit> = SimplePropertyKey.create "view.superViewChanged"
        member inline this.textChanged : SimplePropertyKey<unit->unit> = SimplePropertyKey.create "view.textChanged"
        member inline this.titleChanged : SimplePropertyKey<string->unit> = SimplePropertyKey.create "view.titleChanged"
        member inline this.titleChanging : SimplePropertyKey<App.CancelEventArgs<string>->unit> = SimplePropertyKey.create "view.titleChanging"
        member inline this.viewportChanged : SimplePropertyKey<DrawEventArgs->unit> = SimplePropertyKey.create "view.viewportChanged"
        member inline this.visibleChanged : SimplePropertyKey<unit->unit> = SimplePropertyKey.create "view.visibleChanged"
        member inline this.visibleChanging : SimplePropertyKey<unit->unit> = SimplePropertyKey.create "view.visibleChanging"

    // Adornment
    type adornmentPKeys() =
        inherit viewPKeys()
        // Properties
        member inline this.diagnostics : SimplePropertyKey<ViewDiagnosticFlags> = SimplePropertyKey.create "adornment.diagnostics"
        member inline this.superViewRendersLineCanvas : SimplePropertyKey<bool> = SimplePropertyKey.create "adornment.superViewRendersLineCanvas"
        member inline this.thickness : SimplePropertyKey<Thickness> = SimplePropertyKey.create "adornment.thickness"
        member inline this.viewport : SimplePropertyKey<Rectangle> = SimplePropertyKey.create "adornment.viewport"
        // Events
        member inline this.thicknessChanged : SimplePropertyKey<unit->unit> = SimplePropertyKey.create "adornment.thicknessChanged"

    // Bar
    type barPKeys() =
        inherit viewPKeys()
        // Properties
        member inline this.alignmentModes : SimplePropertyKey<AlignmentModes> = SimplePropertyKey.create "bar.alignmentModes"
        member inline this.orientation : SimplePropertyKey<Orientation> = SimplePropertyKey.create "bar.orientation"
        // Events
        member inline this.orientationChanged : SimplePropertyKey<Orientation->unit> = SimplePropertyKey.create "bar.orientationChanged"
        member inline this.orientationChanging : SimplePropertyKey<App.CancelEventArgs<Orientation>->unit> = SimplePropertyKey.create "bar.orientationChanging"

    // Border
    type borderPKeys() =
        inherit adornmentPKeys()
        // Properties
        member inline this.lineStyle : SimplePropertyKey<LineStyle> = SimplePropertyKey.create "border.lineStyle"
        member inline this.settings : SimplePropertyKey<BorderSettings> = SimplePropertyKey.create "border.settings"

    // Button
    type buttonPKeys() =
        inherit viewPKeys()
        // Properties
        member inline this.hotKeySpecifier : SimplePropertyKey<Rune> = SimplePropertyKey.create "button.hotKeySpecifier"
        member inline this.isDefault : SimplePropertyKey<bool> = SimplePropertyKey.create "button.isDefault"
        member inline this.noDecorations : SimplePropertyKey<bool> = SimplePropertyKey.create "button.noDecorations"
        member inline this.noPadding : SimplePropertyKey<bool> = SimplePropertyKey.create "button.noPadding"
        member inline this.text : SimplePropertyKey<string> = SimplePropertyKey.create "button.text"
        member inline this.wantContinuousButtonPressed : SimplePropertyKey<bool> = SimplePropertyKey.create "button.wantContinuousButtonPressed"

    // CheckBox
    type checkBoxPKeys() =
        inherit viewPKeys()
        // Properties
        member inline this.allowCheckStateNone : SimplePropertyKey<bool> = SimplePropertyKey.create "checkBox.allowCheckStateNone"
        member inline this.checkedState : SimplePropertyKey<CheckState> = SimplePropertyKey.create "checkBox.checkedState"
        member inline this.hotKeySpecifier : SimplePropertyKey<Rune> = SimplePropertyKey.create "checkBox.hotKeySpecifier"
        member inline this.radioStyle : SimplePropertyKey<bool> = SimplePropertyKey.create "checkBox.radioStyle"
        member inline this.text : SimplePropertyKey<string> = SimplePropertyKey.create "checkBox.text"
        // Events
        member inline this.checkedStateChanging : SimplePropertyKey<ResultEventArgs<CheckState>->unit> = SimplePropertyKey.create "checkBox.checkedStateChanging"
        member inline this.checkedStateChanged : SimplePropertyKey<EventArgs<CheckState>->unit> = SimplePropertyKey.create "checkBox.checkedStateChanged"

    // ColorPicker
    type colorPickerPKeys() =
        inherit viewPKeys()
        // Properties
        member inline this.selectedColor : SimplePropertyKey<Color> = SimplePropertyKey.create "colorPicker.selectedColor"
        member inline this.style : SimplePropertyKey<ColorPickerStyle> = SimplePropertyKey.create "colorPicker.style"
        // Events
        member inline this.colorChanged : SimplePropertyKey<ResultEventArgs<Color>->unit> = SimplePropertyKey.create "colorPicker.colorChanged"

    // ColorPicker16
    type colorPicker16PKeys() =
        inherit viewPKeys()
        // Properties
        member inline this.boxHeight : SimplePropertyKey<Int32> = SimplePropertyKey.create "colorPicker16.boxHeight"
        member inline this.boxWidth : SimplePropertyKey<Int32> = SimplePropertyKey.create "colorPicker16.boxWidth"
        member inline this.cursor : SimplePropertyKey<Point> = SimplePropertyKey.create "colorPicker16.cursor"
        member inline this.selectedColor : SimplePropertyKey<ColorName16> = SimplePropertyKey.create "colorPicker16.selectedColor"
        // Events
        member inline this.colorChanged : SimplePropertyKey<ResultEventArgs<Color>->unit> = SimplePropertyKey.create "colorPicker16.colorChanged"

    // ComboBox
    type comboBoxPKeys() =
        inherit viewPKeys()
        // Properties
        member inline this.hideDropdownListOnClick : SimplePropertyKey<bool> = SimplePropertyKey.create "comboBox.hideDropdownListOnClick"
        member inline this.readOnly : SimplePropertyKey<bool> = SimplePropertyKey.create "comboBox.readOnly"
        member inline this.searchText : SimplePropertyKey<string> = SimplePropertyKey.create "comboBox.searchText"
        member inline this.selectedItem : SimplePropertyKey<Int32> = SimplePropertyKey.create "comboBox.selectedItem"
        member inline this.source : SimplePropertyKey<string list> = SimplePropertyKey.create "comboBox.source"
        member inline this.text : SimplePropertyKey<string> = SimplePropertyKey.create "comboBox.text"
        // Events
        member inline this.collapsed : SimplePropertyKey<unit->unit> = SimplePropertyKey.create "comboBox.collapsed"
        member inline this.expanded : SimplePropertyKey<unit->unit> = SimplePropertyKey.create "comboBox.expanded"
        member inline this.openSelectedItem : SimplePropertyKey<ListViewItemEventArgs->unit> = SimplePropertyKey.create "comboBox.openSelectedItem"
        member inline this.selectedItemChanged : SimplePropertyKey<ListViewItemEventArgs->unit> = SimplePropertyKey.create "comboBox.selectedItemChanged"

    // TextField
    type textFieldPKeys() =
        inherit viewPKeys()
        // Properties
        member inline this.autocomplete : SimplePropertyKey<IAutocomplete> = SimplePropertyKey.create "textField.autocomplete"
        member inline this.caption : SimplePropertyKey<string> = SimplePropertyKey.create "textField.caption"
        member inline this.captionColor : SimplePropertyKey<Terminal.Gui.Drawing.Color> = SimplePropertyKey.create "textField.captionColor"
        member inline this.cursorPosition : SimplePropertyKey<Int32> = SimplePropertyKey.create "textField.cursorPosition"
        member inline this.readOnly : SimplePropertyKey<bool> = SimplePropertyKey.create "textField.readOnly"
        member inline this.secret : SimplePropertyKey<bool> = SimplePropertyKey.create "textField.secret"
        member inline this.selectedStart : SimplePropertyKey<Int32> = SimplePropertyKey.create "textField.selectedStart"
        member inline this.selectWordOnlyOnDoubleClick : SimplePropertyKey<bool> = SimplePropertyKey.create "textField.selectWordOnlyOnDoubleClick"
        member inline this.text : SimplePropertyKey<string> = SimplePropertyKey.create "textField.text"
        member inline this.used : SimplePropertyKey<bool> = SimplePropertyKey.create "textField.used"
        member inline this.useSameRuneTypeForWords : SimplePropertyKey<bool> = SimplePropertyKey.create "textField.useSameRuneTypeForWords"
        // Events
        member inline this.textChanging : SimplePropertyKey<ResultEventArgs<string>->unit> = SimplePropertyKey.create "textField.textChanging"

    // DateField
    type dateFieldPKeys() =
        inherit textFieldPKeys()
        // Properties
        member inline this.culture : SimplePropertyKey<CultureInfo> = SimplePropertyKey.create "dateField.culture"
        member inline this.cursorPosition : SimplePropertyKey<Int32> = SimplePropertyKey.create "dateField.cursorPosition"
        member inline this.date : SimplePropertyKey<DateTime> = SimplePropertyKey.create "dateField.date"
        // Events
        member inline this.dateChanged : SimplePropertyKey<DateTimeEventArgs<DateTime>->unit> = SimplePropertyKey.create "dateField.dateChanged"

    // DatePicker
    type datePickerPKeys() =
        inherit viewPKeys()
        // Properties
        member inline this.culture : SimplePropertyKey<CultureInfo> = SimplePropertyKey.create "datePicker.culture"
        member inline this.date : SimplePropertyKey<DateTime> = SimplePropertyKey.create "datePicker.date"

    // Toplevel
    type toplevelPKeys() =
        inherit viewPKeys()
        // Properties
        member inline this.modal : SimplePropertyKey<bool> = SimplePropertyKey.create "toplevel.modal"
        member inline this.running : SimplePropertyKey<bool> = SimplePropertyKey.create "toplevel.running"
        // Events
        member inline this.activate : SimplePropertyKey<ToplevelEventArgs->unit> = SimplePropertyKey.create "toplevel.activate"
        member inline this.closed : SimplePropertyKey<ToplevelEventArgs->unit> = SimplePropertyKey.create "toplevel.closed"
        member inline this.closing : SimplePropertyKey<ToplevelClosingEventArgs->unit> = SimplePropertyKey.create "toplevel.closing"
        member inline this.deactivate : SimplePropertyKey<ToplevelEventArgs->unit> = SimplePropertyKey.create "toplevel.deactivate"
        member inline this.loaded : SimplePropertyKey<unit->unit> = SimplePropertyKey.create "toplevel.loaded"
        member inline this.ready : SimplePropertyKey<unit->unit> = SimplePropertyKey.create "toplevel.ready"
        member inline this.sizeChanging : SimplePropertyKey<SizeChangedEventArgs->unit> = SimplePropertyKey.create "toplevel.sizeChanging"
        member inline this.unloaded : SimplePropertyKey<unit->unit> = SimplePropertyKey.create "toplevel.unloaded"

    // Dialog
    type dialogPKeys() =
        inherit toplevelPKeys()
        // Properties
        member inline this.buttonAlignment : SimplePropertyKey<Alignment> = SimplePropertyKey.create "dialog.buttonAlignment"
        member inline this.buttonAlignmentModes : SimplePropertyKey<AlignmentModes> = SimplePropertyKey.create "dialog.buttonAlignmentModes"
        member inline this.canceled : SimplePropertyKey<bool> = SimplePropertyKey.create "dialog.canceled"

    // FileDialog
    type fileDialogPKeys() =
        inherit dialogPKeys()
        // Properties
        member inline this.allowedTypes : SimplePropertyKey<IAllowedType list> = SimplePropertyKey.create "fileDialog.allowedTypes"
        member inline this.allowsMultipleSelection : SimplePropertyKey<bool> = SimplePropertyKey.create "fileDialog.allowsMultipleSelection"
        member inline this.fileOperationsHandler : SimplePropertyKey<IFileOperations> = SimplePropertyKey.create "fileDialog.fileOperationsHandler"
        member inline this.mustExist : SimplePropertyKey<bool> = SimplePropertyKey.create "fileDialog.mustExist"
        member inline this.openMode : SimplePropertyKey<OpenMode> = SimplePropertyKey.create "fileDialog.openMode"
        member inline this.path : SimplePropertyKey<string> = SimplePropertyKey.create "fileDialog.path"
        member inline this.searchMatcher : SimplePropertyKey<ISearchMatcher> = SimplePropertyKey.create "fileDialog.searchMatcher"
        // Events
        member inline this.filesSelected : SimplePropertyKey<FilesSelectedEventArgs->unit> = SimplePropertyKey.create "fileDialog.filesSelected"

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
        member inline this.axisX : SimplePropertyKey<HorizontalAxis> = SimplePropertyKey.create "graphView.axisX"
        member inline this.axisY : SimplePropertyKey<VerticalAxis> = SimplePropertyKey.create "graphView.axisY"
        member inline this.cellSize : SimplePropertyKey<PointF> = SimplePropertyKey.create "graphView.cellSize"
        member inline this.graphColor : SimplePropertyKey<Attribute option> = SimplePropertyKey.create "graphView.graphColor"
        member inline this.marginBottom : SimplePropertyKey<int> = SimplePropertyKey.create "graphView.marginBottom"
        member inline this.marginLeft : SimplePropertyKey<int> = SimplePropertyKey.create "graphView.marginLeft"
        member inline this.scrollOffset : SimplePropertyKey<PointF> = SimplePropertyKey.create "graphView.scrollOffset"

    // HexView
    type hexViewPKeys() =
        inherit viewPKeys()

        // Properties
        member inline this.address : SimplePropertyKey<Int64> = SimplePropertyKey.create "hexView.address"
        member inline this.addressWidth : SimplePropertyKey<int> = SimplePropertyKey.create "hexView.addressWidth"
        member inline this.allowEdits : SimplePropertyKey<int> = SimplePropertyKey.create "hexView.allowEdits"
        member inline this.readOnly : SimplePropertyKey<bool> = SimplePropertyKey.create "hexView.readOnly"
        member inline this.source : SimplePropertyKey<Stream> = SimplePropertyKey.create "hexView.source"
        // Events
        member inline this.edited : SimplePropertyKey<HexViewEditEventArgs->unit> = SimplePropertyKey.create "hexView.edited"
        member inline this.positionChanged : SimplePropertyKey<HexViewEventArgs->unit> = SimplePropertyKey.create "hexView.positionChanged"

    // Label
    type labelPKeys() =
        inherit viewPKeys()

        // Properties
        member inline this.hotKeySpecifier : SimplePropertyKey<Rune> = SimplePropertyKey.create "label.hotKeySpecifier"
        member inline this.text : SimplePropertyKey<string> = SimplePropertyKey.create "label.text"

    // LegendAnnotation
    type legendAnnotationPKeys() =
        inherit viewPKeys()
    // No properties or events LegendAnnotation

    // Line
    type linePKeys() =
        inherit viewPKeys()

        // Properties
        member inline this.orientation : SimplePropertyKey<Orientation> = SimplePropertyKey.create "line.orientation"
        // Events
        member inline this.orientationChanged : SimplePropertyKey<Orientation->unit> = SimplePropertyKey.create "line.orientationChanged"
        member inline this.orientationChanging : SimplePropertyKey<App.CancelEventArgs<Orientation>->unit> = SimplePropertyKey.create "line.orientationChanging"

    // LineView
    type lineViewPKeys() =
        inherit viewPKeys()

        // Properties
        member inline this.endingAnchor : SimplePropertyKey<Rune option> = SimplePropertyKey.create "lineView.endingAnchor"
        member inline this.lineRune : SimplePropertyKey<Rune> = SimplePropertyKey.create "lineView.lineRune"
        member inline this.orientation : SimplePropertyKey<Orientation> = SimplePropertyKey.create "lineView.orientation"
        member inline this.startingAnchor : SimplePropertyKey<Rune option> = SimplePropertyKey.create "lineView.startingAnchor"

    // ListView
    type listViewPKeys() =
        inherit viewPKeys()

        // Properties
        member inline this.allowsMarking : SimplePropertyKey<bool> = SimplePropertyKey.create "listView.allowsMarking"
        member inline this.allowsMultipleSelection : SimplePropertyKey<bool> = SimplePropertyKey.create "listView.allowsMultipleSelection"
        member inline this.leftItem : SimplePropertyKey<Int32> = SimplePropertyKey.create "listView.leftItem"
        member inline this.selectedItem : SimplePropertyKey<Int32> = SimplePropertyKey.create "listView.selectedItem"
        member inline this.source : SimplePropertyKey<string list> = SimplePropertyKey.create "listView.source"
        member inline this.topItem : SimplePropertyKey<Int32> = SimplePropertyKey.create "listView.topItem"
        // Events
        member inline this.collectionChanged : SimplePropertyKey<NotifyCollectionChangedEventArgs->unit> = SimplePropertyKey.create "listView.collectionChanged"
        member inline this.openSelectedItem : SimplePropertyKey<ListViewItemEventArgs->unit> = SimplePropertyKey.create "listView.openSelectedItem"
        member inline this.rowRender : SimplePropertyKey<ListViewRowEventArgs->unit> = SimplePropertyKey.create "listView.rowRender"
        member inline this.selectedItemChanged : SimplePropertyKey<ListViewItemEventArgs->unit> = SimplePropertyKey.create "listView.selectedItemChanged"

    // Margin
    type marginPKeys() =
        inherit adornmentPKeys()

        // Properties
        member inline this.shadowStyle : SimplePropertyKey<ShadowStyle> = SimplePropertyKey.create "margin.shadowStyle"

    type menuv2PKeys() =
        inherit barPKeys()

        // Properties
        member inline this.selectedMenuItem : SimplePropertyKey<MenuItemv2> = SimplePropertyKey.create "menuv2.selectedMenuItem"
        member inline this.superMenuItem : SimplePropertyKey<MenuItemv2> = SimplePropertyKey.create "menuv2.superMenuItem"
        // Events
        member inline this.accepted : SimplePropertyKey< CommandEventArgs->unit> = SimplePropertyKey.create "menuv2.accepted"
        member inline this.selectedMenuItemChanged : SimplePropertyKey< MenuItemv2->unit> = SimplePropertyKey.create "menuv2.selectedMenuItemChanged"

    // MenuBarV2
    type menuBarv2PKeys() =
        inherit menuv2PKeys()

        // Properties
        member inline this.key : SimplePropertyKey<Key> = SimplePropertyKey.create "menuBarv2.key"
        member inline this.menus : SimplePropertyKey<MenuBarItemv2 array> = SimplePropertyKey.create "view.menus"
        // Events
        member inline this.keyChanged : SimplePropertyKey<KeyChangedEventArgs->unit> = SimplePropertyKey.create "menuBarv2.keyChanged"

    type shortcutPKeys() =
         inherit viewPKeys()

         // Properties
         member inline this.action : SimplePropertyKey<Action> = SimplePropertyKey.create "shortcut.action"
         member inline this.alignmentModes : SimplePropertyKey<AlignmentModes> = SimplePropertyKey.create "shortcut.alignmentModes"
         member inline this.commandView : SimplePropertyKey<View> = SimplePropertyKey.create "shortcut.commandView_view"
         member inline this.commandView_element : SingleElementKey<ITerminalElement> = SingleElementKey.create "shortcut.commandView_element"
         member inline this.forceFocusColors : SimplePropertyKey<bool> = SimplePropertyKey.create "shortcut.forceFocusColors"
         member inline this.helpText : SimplePropertyKey<string> = SimplePropertyKey.create "shortcut.helpText"
         member inline this.text : SimplePropertyKey<string> = SimplePropertyKey.create "shortcut.text"
         member inline this.bindKeyToApplication : SimplePropertyKey<bool> = SimplePropertyKey.create "shortcut.bindKeyToApplication"
         member inline this.key : SimplePropertyKey<Key> = SimplePropertyKey.create "shortcut.key"
         member inline this.minimumKeyTextSize : SimplePropertyKey<Int32> = SimplePropertyKey.create "shortcut.minimumKeyTextSize"
         // Events
         member inline this.orientationChanged : SimplePropertyKey<Orientation->unit> = SimplePropertyKey.create "shortcut.orientationChanged"
         member inline this.orientationChanging : SimplePropertyKey<CancelEventArgs<Orientation>->unit> = SimplePropertyKey.create "shortcut.orientationChanging"

    type menuItemv2PKeys() =
        inherit shortcutPKeys()
        member inline this.command : SimplePropertyKey< Command> = SimplePropertyKey.create "menuItemv2.command"
        member inline this.subMenu: SimplePropertyKey<Menuv2> = SimplePropertyKey.create "menuItemv2.subMenu_view"
        member inline this.subMenu_element : SingleElementKey<IMenuv2Element> = SingleElementKey.create "menuItemv2.subMenu_element"
        member inline this.targetView : SimplePropertyKey<View> = SimplePropertyKey.create "menuItemv2.targetView"
        member inline this.accepted: SimplePropertyKey<CommandEventArgs -> unit> = SimplePropertyKey.create "menuItemv2.accepted"

    type menuBarItemv2PKeys() =
        inherit menuItemv2PKeys()

        // Properties
        member inline this.popoverMenu : SimplePropertyKey<PopoverMenu> = SimplePropertyKey.create "menuBarItemv2.popoverMenu_view"
        member inline this.popoverMenu_element : SingleElementKey<IPopoverMenuElement> = SingleElementKey.create "menuBarItemv2.popoverMenu_element"
        member inline this.popoverMenuOpen : SimplePropertyKey<bool> = SimplePropertyKey.create "menuBarItemv2.popoverMenuOpen"
        // Events
        member inline this.popoverMenuOpenChanged : SimplePropertyKey<bool->unit> = SimplePropertyKey.create "menuBarItemv2.popoverMenuOpenChanged"

    type popoverMenuPKeys() =
        inherit viewPKeys()

        // Properties
        member inline this.key : SimplePropertyKey<Key> = SimplePropertyKey.create "popoverMenu.key"
        member inline this.mouseFlags : SimplePropertyKey<MouseFlags> = SimplePropertyKey.create "popoverMenu.mouseFlags"
        member inline this.root : SimplePropertyKey<Menuv2> = SimplePropertyKey.create "popoverMenu.root_view"
        member inline this.root_element : SingleElementKey<IMenuv2Element> = SingleElementKey.create "popoverMenu.root_element"
        // Events
        member inline this.accepted : SimplePropertyKey<CommandEventArgs->unit> = SimplePropertyKey.create "popoverMenu.accepted"
        member inline this.keyChanged : SimplePropertyKey<KeyChangedEventArgs->unit> = SimplePropertyKey.create "popoverMenu.keyChanged"

    // NumericUpDown`1
    type numericUpDownPKeys<'a>() =
        inherit viewPKeys()

        // Properties
        member inline this.format : SimplePropertyKey<string> = SimplePropertyKey.create "numericUpDown.format"
        member inline this.increment : SimplePropertyKey<'a> = SimplePropertyKey.create "numericUpDown.increment"
        member inline this.value : SimplePropertyKey<'a> = SimplePropertyKey.create "numericUpDown.value"
        // Events
        member inline this.formatChanged : SimplePropertyKey<string->unit> = SimplePropertyKey.create "numericUpDown.formatChanged"
        member inline this.incrementChanged : SimplePropertyKey<'a->unit> = SimplePropertyKey.create "numericUpDown.incrementChanged"
        member inline this.valueChanged : SimplePropertyKey<'a->unit> = SimplePropertyKey.create "numericUpDown.valueChanged"
        member inline this.valueChanging : SimplePropertyKey<App.CancelEventArgs<'a>->unit> = SimplePropertyKey.create "numericUpDown.valueChanging"

    // NumericUpDown
    // type numericUpDownPKeys() =
    //     inherit numericUpDownPKeys<int>()
    // No properties or events NumericUpDown

    // OpenDialog
    type openDialogPKeys() =
        inherit fileDialogPKeys()
        // Properties
        member inline this.openMode : SimplePropertyKey<OpenMode> = SimplePropertyKey.create "openDialog.openMode"

    // OptionSelector
    type optionSelectorPKeys() =
        inherit viewPKeys()
        //Properties
        member inline this.assignHotKeysToCheckBoxes : SimplePropertyKey<bool> = SimplePropertyKey.create "optionSelector.assignHotKeysToCheckBoxes"
        member inline this.orientation : SimplePropertyKey<Orientation> = SimplePropertyKey.create "optionSelector.orientation"
        member inline this.options : SimplePropertyKey<IReadOnlyList<string>> = SimplePropertyKey.create "optionSelector.options"
        member inline this.selectedItem : SimplePropertyKey<Int32> = SimplePropertyKey.create "optionSelector.selectedItem"
        // Events
        member inline this.orientationChanged : SimplePropertyKey<Orientation->unit> = SimplePropertyKey.create "optionSelector.orientationChanged"
        member inline this.orientationChanging : SimplePropertyKey<CancelEventArgs<Orientation>->unit> = SimplePropertyKey.create "optionSelector.orientationChanging"
        member inline this.selectedItemChanged : SimplePropertyKey<SelectedItemChangedArgs->unit> = SimplePropertyKey.create "optionSelector.selectedItemChanged"

    // Padding
    type paddingPKeys() =
        inherit adornmentPKeys()

    // ProgressBar
    type progressBarPKeys() =
        inherit viewPKeys()

        // Properties
        member inline this.bidirectionalMarquee : SimplePropertyKey<bool> = SimplePropertyKey.create "progressBar.bidirectionalMarquee"
        member inline this.fraction : SimplePropertyKey<Single> = SimplePropertyKey.create "progressBar.fraction"
        member inline this.progressBarFormat : SimplePropertyKey<ProgressBarFormat> = SimplePropertyKey.create "progressBar.progressBarFormat"
        member inline this.progressBarStyle : SimplePropertyKey<ProgressBarStyle> = SimplePropertyKey.create "progressBar.progressBarStyle"
        member inline this.segmentCharacter : SimplePropertyKey<Rune> = SimplePropertyKey.create "progressBar.segmentCharacter"
        member inline this.text : SimplePropertyKey<string> = SimplePropertyKey.create "progressBar.text"

    // RadioGroup
    type radioGroupPKeys() =
        inherit viewPKeys()

        // Properties
        member inline this.assignHotKeysToRadioLabels : SimplePropertyKey<bool> = SimplePropertyKey.create "radioGroup.assignHotKeysToRadioLabels"
        member inline this.cursor : SimplePropertyKey<Int32> = SimplePropertyKey.create "radioGroup.cursor"
        member inline this.doubleClickAccepts : SimplePropertyKey<bool> = SimplePropertyKey.create "radioGroup.doubleClickAccepts"
        member inline this.horizontalSpace : SimplePropertyKey<Int32> = SimplePropertyKey.create "radioGroup.horizontalSpace"
        member inline this.orientation : SimplePropertyKey<Orientation> = SimplePropertyKey.create "radioGroup.orientation"
        member inline this.radioLabels : SimplePropertyKey<string list> = SimplePropertyKey.create "radioGroup.radioLabels"
        member inline this.selectedItem : SimplePropertyKey<Int32> = SimplePropertyKey.create "radioGroup.selectedItem"
        // Events
        member inline this.orientationChanged : SimplePropertyKey<Orientation->unit> = SimplePropertyKey.create "radioGroup.orientationChanged"
        member inline this.orientationChanging : SimplePropertyKey<App.CancelEventArgs<Orientation>->unit> = SimplePropertyKey.create "radioGroup.orientationChanging"
        member inline this.selectedItemChanged : SimplePropertyKey<SelectedItemChangedArgs->unit> = SimplePropertyKey.create "radioGroup.selectedItemChanged"

    // SaveDialog
    // No properties or events SaveDialog

    // ScrollBar
    type scrollBarPKeys() =
        inherit viewPKeys()

        // Properties
        member inline this.autoShow : SimplePropertyKey<bool> = SimplePropertyKey.create "scrollBar.autoShow"
        member inline this.increment : SimplePropertyKey<Int32> = SimplePropertyKey.create "scrollBar.increment"
        member inline this.orientation : SimplePropertyKey<Orientation> = SimplePropertyKey.create "scrollBar.orientation"
        member inline this.position : SimplePropertyKey<Int32> = SimplePropertyKey.create "scrollBar.position"
        member inline this.scrollableContentSize : SimplePropertyKey<Int32> = SimplePropertyKey.create "scrollBar.scrollableContentSize"
        member inline this.visibleContentSize : SimplePropertyKey<Int32> = SimplePropertyKey.create "scrollBar.visibleContentSize"
        // Events
        member inline this.orientationChanged : SimplePropertyKey<Orientation->unit> = SimplePropertyKey.create "scrollBar.orientationChanged"
        member inline this.orientationChanging : SimplePropertyKey<CancelEventArgs<Orientation>->unit> = SimplePropertyKey.create "scrollBar.orientationChanging"
        member inline this.scrollableContentSizeChanged : SimplePropertyKey<EventArgs<Int32>->unit> = SimplePropertyKey.create "scrollBar.scrollableContentSizeChanged"
        member inline this.sliderPositionChanged : SimplePropertyKey<EventArgs<Int32>->unit> = SimplePropertyKey.create "scrollBar.sliderPositionChanged"

    // ScrollSlider
    type scrollSliderPKeys() =
        inherit viewPKeys()

        // Properties
        member inline this.orientation : SimplePropertyKey<Orientation> = SimplePropertyKey.create "scrollSlider.orientation"
        member inline this.position : SimplePropertyKey<Int32> = SimplePropertyKey.create "scrollSlider.position"
        member inline this.size : SimplePropertyKey<Int32> = SimplePropertyKey.create "scrollSlider.size"
        member inline this.sliderPadding : SimplePropertyKey<Int32> = SimplePropertyKey.create "scrollSlider.sliderPadding"
        member inline this.visibleContentSize : SimplePropertyKey<Int32> = SimplePropertyKey.create "scrollSlider.visibleContentSize"
        // Events
        member inline this.orientationChanged : SimplePropertyKey<Orientation->unit> = SimplePropertyKey.create "scrollSlider.orientationChanged"
        member inline this.orientationChanging : SimplePropertyKey<CancelEventArgs<Orientation>->unit> = SimplePropertyKey.create "scrollSlider.orientationChanging"
        member inline this.positionChanged : SimplePropertyKey<EventArgs<Int32>->unit> = SimplePropertyKey.create "scrollSlider.positionChanged"
        member inline this.positionChanging : SimplePropertyKey<CancelEventArgs<Int32>->unit> = SimplePropertyKey.create "scrollSlider.positionChanging"
        member inline this.scrolled : SimplePropertyKey<EventArgs<Int32>->unit> = SimplePropertyKey.create "scrollSlider.scrolled"

    // Slider`1
    type sliderPKeys<'a>() =
        inherit viewPKeys()

        // Properties
        member inline this.allowEmpty : SimplePropertyKey<bool> = SimplePropertyKey.create "slider.allowEmpty"
        member inline this.focusedOption : SimplePropertyKey<Int32> = SimplePropertyKey.create "slider.focusedOption"
        member inline this.legendsOrientation : SimplePropertyKey<Orientation> = SimplePropertyKey.create "slider.legendsOrientation"
        member inline this.minimumInnerSpacing : SimplePropertyKey<Int32> = SimplePropertyKey.create "slider.minimumInnerSpacing"
        member inline this.options : SimplePropertyKey<SliderOption<'a> list> = SimplePropertyKey.create "slider.options"
        member inline this.orientation : SimplePropertyKey<Orientation> = SimplePropertyKey.create "slider.orientation"
        member inline this.rangeAllowSingle : SimplePropertyKey<bool> = SimplePropertyKey.create "slider.rangeAllowSingle"
        member inline this.showEndSpacing : SimplePropertyKey<bool> = SimplePropertyKey.create "slider.showEndSpacing"
        member inline this.showLegends : SimplePropertyKey<bool> = SimplePropertyKey.create "slider.showLegends"
        member inline this.style : SimplePropertyKey<SliderStyle> = SimplePropertyKey.create "slider.style"
        member inline this.text : SimplePropertyKey<string> = SimplePropertyKey.create "slider.text"
        member inline this.``type`` : SimplePropertyKey<SliderType> = SimplePropertyKey.create "slider.``type``"
        member inline this.useMinimumSize : SimplePropertyKey<bool> = SimplePropertyKey.create "slider.useMinimumSize"
        // Events
        member inline this.optionFocused : SimplePropertyKey<SliderEventArgs<'a>->unit> = SimplePropertyKey.create "slider.optionFocused"
        member inline this.optionsChanged : SimplePropertyKey<SliderEventArgs<'a>->unit> = SimplePropertyKey.create "slider.optionsChanged"
        member inline this.orientationChanged : SimplePropertyKey<Orientation->unit> = SimplePropertyKey.create "slider.orientationChanged"
        member inline this.orientationChanging : SimplePropertyKey<App.CancelEventArgs<Orientation>->unit> = SimplePropertyKey.create "slider.orientationChanging"

    // Slider
    // type sliderPKeys() =
    //     inherit sliderPKeys<obj>()
    // No properties or events Slider

    // SpinnerView
    type spinnerViewPKeys() =
        inherit viewPKeys()

        // Properties
        member inline this.autoSpin : SimplePropertyKey<bool> = SimplePropertyKey.create "spinnerView.autoSpin"
        member inline this.sequence : SimplePropertyKey<string list> = SimplePropertyKey.create "spinnerView.sequence"
        member inline this.spinBounce : SimplePropertyKey<bool> = SimplePropertyKey.create "spinnerView.spinBounce"
        member inline this.spinDelay : SimplePropertyKey<Int32> = SimplePropertyKey.create "spinnerView.spinDelay"
        member inline this.spinReverse : SimplePropertyKey<bool> = SimplePropertyKey.create "spinnerView.spinReverse"
        member inline this.style : SimplePropertyKey<SpinnerStyle> = SimplePropertyKey.create "spinnerView.style"

    // StatusBar
    type statusBarPKeys() =
        inherit barPKeys()
    // No properties or events StatusBar

    // Tab
    type tabPKeys() =
        inherit viewPKeys()

        // Properties
        member inline this.displayText : SimplePropertyKey<string> = SimplePropertyKey.create "tab.displayText"
        member inline this.view : SimplePropertyKey<View> = SimplePropertyKey.create "tab.view_view"
        member inline this.view_element : SingleElementKey<ITerminalElement> = SingleElementKey.create "tab.view_element"

    // TabView
    type tabViewPKeys() =
        inherit viewPKeys()

        // Properties
        member inline this.maxTabTextWidth : SimplePropertyKey<int> = SimplePropertyKey.create "tabView.maxTabTextWidth"
        member inline this.selectedTab : SimplePropertyKey<Tab> = SimplePropertyKey.create "tabView.selectedTab"
        member inline this.style : SimplePropertyKey<TabStyle> = SimplePropertyKey.create "tabView.style"
        member inline this.tabScrollOffset : SimplePropertyKey<Int32> = SimplePropertyKey.create "tabView.tabScrollOffset"
        // Events
        member inline this.selectedTabChanged : SimplePropertyKey<TabChangedEventArgs->unit> = SimplePropertyKey.create "tabView.selectedTabChanged"
        member inline this.tabClicked : SimplePropertyKey<TabMouseEventArgs->unit> = SimplePropertyKey.create "tabView.tabClicked"
        // Additional properties
        member inline this.tabs : SimplePropertyKey<List<ITerminalElement>> = SimplePropertyKey.create "tabView.tabs_view"
        member inline this.tabs_elements : MultiElementKey<List<ITerminalElement>> = MultiElementKey.create "tabView.tabs_elements"

    // TableView
    type tableViewPKeys() =
        inherit viewPKeys()

        // Properties
        member inline this.cellActivationKey : SimplePropertyKey<KeyCode> = SimplePropertyKey.create "tableView.cellActivationKey"
        member inline this.collectionNavigator : SimplePropertyKey<ICollectionNavigator> = SimplePropertyKey.create "tableView.collectionNavigator"
        member inline this.columnOffset : SimplePropertyKey<Int32> = SimplePropertyKey.create "tableView.columnOffset"
        member inline this.fullRowSelect : SimplePropertyKey<bool> = SimplePropertyKey.create "tableView.fullRowSelect"
        member inline this.maxCellWidth : SimplePropertyKey<Int32> = SimplePropertyKey.create "tableView.maxCellWidth"
        member inline this.minCellWidth : SimplePropertyKey<Int32> = SimplePropertyKey.create "tableView.minCellWidth"
        member inline this.multiSelect : SimplePropertyKey<bool> = SimplePropertyKey.create "tableView.multiSelect"
        member inline this.nullSymbol : SimplePropertyKey<string> = SimplePropertyKey.create "tableView.nullSymbol"
        member inline this.rowOffset : SimplePropertyKey<Int32> = SimplePropertyKey.create "tableView.rowOffset"
        member inline this.selectedColumn : SimplePropertyKey<Int32> = SimplePropertyKey.create "tableView.selectedColumn"
        member inline this.selectedRow : SimplePropertyKey<Int32> = SimplePropertyKey.create "tableView.selectedRow"
        member inline this.separatorSymbol : SimplePropertyKey<Char> = SimplePropertyKey.create "tableView.separatorSymbol"
        member inline this.style : SimplePropertyKey<TableStyle> = SimplePropertyKey.create "tableView.style"
        member inline this.table : SimplePropertyKey<ITableSource> = SimplePropertyKey.create "tableView.table"
        // Events
        member inline this.cellActivated : SimplePropertyKey<CellActivatedEventArgs->unit> = SimplePropertyKey.create "tableView.cellActivated"
        member inline this.cellToggled : SimplePropertyKey<CellToggledEventArgs->unit> = SimplePropertyKey.create "tableView.cellToggled"
        member inline this.selectedCellChanged : SimplePropertyKey<SelectedCellChangedEventArgs->unit> = SimplePropertyKey.create "tableView.selectedCellChanged"

    // TextValidateField
    type textValidateFieldPKeys() =
        inherit viewPKeys()

        // Properties
        member inline this.provider : SimplePropertyKey<ITextValidateProvider> = SimplePropertyKey.create "textValidateField.provider"
        member inline this.text : SimplePropertyKey<string> = SimplePropertyKey.create "textValidateField.text"

    // TextView
    type textViewPKeys() =
        inherit viewPKeys()

        // Properties
        member inline this.allowsReturn : SimplePropertyKey<bool> = SimplePropertyKey.create "textView.allowsReturn"
        member inline this.allowsTab : SimplePropertyKey<bool> = SimplePropertyKey.create "textView.allowsTab"
        member inline this.cursorPosition : SimplePropertyKey<Point> = SimplePropertyKey.create "textView.cursorPosition"
        member inline this.inheritsPreviousAttribute : SimplePropertyKey<bool> = SimplePropertyKey.create "textView.inheritsPreviousAttribute"
        member inline this.isDirty : SimplePropertyKey<bool> = SimplePropertyKey.create "textView.isDirty"
        member inline this.isSelecting : SimplePropertyKey<bool> = SimplePropertyKey.create "textView.isSelecting"
        member inline this.leftColumn : SimplePropertyKey<Int32> = SimplePropertyKey.create "textView.leftColumn"
        member inline this.multiline : SimplePropertyKey<bool> = SimplePropertyKey.create "textView.multiline"
        member inline this.readOnly : SimplePropertyKey<bool> = SimplePropertyKey.create "textView.readOnly"
        member inline this.selectionStartColumn : SimplePropertyKey<Int32> = SimplePropertyKey.create "textView.selectionStartColumn"
        member inline this.selectionStartRow : SimplePropertyKey<Int32> = SimplePropertyKey.create "textView.selectionStartRow"
        member inline this.selectWordOnlyOnDoubleClick : SimplePropertyKey<bool> = SimplePropertyKey.create "textView.selectWordOnlyOnDoubleClick"
        member inline this.tabWidth : SimplePropertyKey<Int32> = SimplePropertyKey.create "textView.tabWidth"
        member inline this.text : SimplePropertyKey<string> = SimplePropertyKey.create "textView.text"
        member inline this.topRow : SimplePropertyKey<Int32> = SimplePropertyKey.create "textView.topRow"
        member inline this.used : SimplePropertyKey<bool> = SimplePropertyKey.create "textView.used"
        member inline this.useSameRuneTypeForWords : SimplePropertyKey<bool> = SimplePropertyKey.create "textView.useSameRuneTypeForWords"
        member inline this.wordWrap : SimplePropertyKey<bool> = SimplePropertyKey.create "textView.wordWrap"
        // Events
        member inline this.contentsChanged : SimplePropertyKey<ContentsChangedEventArgs->unit> = SimplePropertyKey.create "textView.contentsChanged"
        member inline this.drawNormalColor : SimplePropertyKey<CellEventArgs->unit> = SimplePropertyKey.create "textView.drawNormalColor"
        member inline this.drawReadOnlyColor : SimplePropertyKey<CellEventArgs->unit> = SimplePropertyKey.create "textView.drawReadOnlyColor"
        member inline this.drawSelectionColor : SimplePropertyKey<CellEventArgs->unit> = SimplePropertyKey.create "textView.drawSelectionColor"
        member inline this.drawUsedColor : SimplePropertyKey<CellEventArgs->unit> = SimplePropertyKey.create "textView.drawUsedColor"
        member inline this.unwrappedCursorPosition : SimplePropertyKey<Point->unit> = SimplePropertyKey.create "textView.unwrappedCursorPosition"
        // Additional properties
        member inline this.textChanged : SimplePropertyKey<string->unit> = SimplePropertyKey.create "textView.textChanged"

    // TileView
    type tileViewPKeys() =
        inherit viewPKeys()

        // Properties
        member inline this.lineStyle : SimplePropertyKey<LineStyle> = SimplePropertyKey.create "tileView.lineStyle"
        member inline this.orientation : SimplePropertyKey<Orientation> = SimplePropertyKey.create "tileView.orientation"
        member inline this.toggleResizable : SimplePropertyKey<KeyCode> = SimplePropertyKey.create "tileView.toggleResizable"
        // Events
        member inline this.splitterMoved : SimplePropertyKey<SplitterEventArgs->unit> = SimplePropertyKey.create "tileView.splitterMoved"

    // TimeField
    type timeFieldPKeys() =
        inherit textFieldPKeys()

        // Properties
        member inline this.cursorPosition : SimplePropertyKey<Int32> = SimplePropertyKey.create "timeField.cursorPosition"
        member inline this.isShortFormat : SimplePropertyKey<bool> = SimplePropertyKey.create "timeField.isShortFormat"
        member inline this.time : SimplePropertyKey<TimeSpan> = SimplePropertyKey.create "timeField.time"
        // Events
        member inline this.timeChanged : SimplePropertyKey<DateTimeEventArgs<TimeSpan>->unit> = SimplePropertyKey.create "timeField.timeChanged"

    // TreeView`1
    type treeViewPKeys<'a when 'a : not struct>() =
        inherit viewPKeys()

        // Properties
        member inline this.allowLetterBasedNavigation : SimplePropertyKey<bool> = SimplePropertyKey.create "treeView.allowLetterBasedNavigation"
        member inline this.aspectGetter : SimplePropertyKey<AspectGetterDelegate<'a>> = SimplePropertyKey.create "treeView.aspectGetter"
        member inline this.colorGetter : SimplePropertyKey<Func<'a,Scheme>> = SimplePropertyKey.create "treeView.colorGetter"
        member inline this.maxDepth : SimplePropertyKey<Int32> = SimplePropertyKey.create "treeView.maxDepth"
        member inline this.multiSelect : SimplePropertyKey<bool> = SimplePropertyKey.create "treeView.multiSelect"
        member inline this.objectActivationButton : SimplePropertyKey<MouseFlags option> = SimplePropertyKey.create "treeView.objectActivationButton"
        member inline this.objectActivationKey : SimplePropertyKey<KeyCode> = SimplePropertyKey.create "treeView.objectActivationKey"
        member inline this.scrollOffsetHorizontal : SimplePropertyKey<Int32> = SimplePropertyKey.create "treeView.scrollOffsetHorizontal"
        member inline this.scrollOffsetVertical : SimplePropertyKey<Int32> = SimplePropertyKey.create "treeView.scrollOffsetVertical"
        member inline this.selectedObject : SimplePropertyKey<'a> = SimplePropertyKey.create "treeView.selectedObject"
        member inline this.style : SimplePropertyKey<TreeStyle> = SimplePropertyKey.create "treeView.style"
        member inline this.treeBuilder : SimplePropertyKey<ITreeBuilder<'a>> = SimplePropertyKey.create "treeView.treeBuilder"
        // Events
        member inline this.drawLine : SimplePropertyKey<DrawTreeViewLineEventArgs<'a>->unit> = SimplePropertyKey.create "treeView.drawLine"
        member inline this.objectActivated : SimplePropertyKey<ObjectActivatedEventArgs<'a>->unit> = SimplePropertyKey.create "treeView.objectActivated"
        member inline this.selectionChanged : SimplePropertyKey<SelectionChangedEventArgs<'a>->unit> = SimplePropertyKey.create "treeView.selectionChanged"

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
        member inline this.currentStep : SimplePropertyKey<WizardStep> = SimplePropertyKey.create "wizard.currentStep"
        member inline this.modal : SimplePropertyKey<bool> = SimplePropertyKey.create "wizard.modal"
        // Events
        member inline this.cancelled : SimplePropertyKey<WizardButtonEventArgs->unit> = SimplePropertyKey.create "wizard.cancelled"
        member inline this.finished : SimplePropertyKey<WizardButtonEventArgs->unit> = SimplePropertyKey.create "wizard.finished"
        member inline this.movingBack : SimplePropertyKey<WizardButtonEventArgs->unit> = SimplePropertyKey.create "wizard.movingBack"
        member inline this.movingNext : SimplePropertyKey<WizardButtonEventArgs->unit> = SimplePropertyKey.create "wizard.movingNext"
        member inline this.stepChanged : SimplePropertyKey<StepChangeEventArgs->unit> = SimplePropertyKey.create "wizard.stepChanged"
        member inline this.stepChanging : SimplePropertyKey<StepChangeEventArgs->unit> = SimplePropertyKey.create "wizard.stepChanging"

    // WizardStep
    type wizardStepPKeys() =
        inherit viewPKeys()

        // Properties
        member inline this.backButtonText : SimplePropertyKey<string> = SimplePropertyKey.create "wizardStep.backButtonText"
        member inline this.helpText : SimplePropertyKey<string> = SimplePropertyKey.create "wizardStep.helpText"
        member inline this.nextButtonText : SimplePropertyKey<string> = SimplePropertyKey.create "wizardStep.nextButtonText"


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
