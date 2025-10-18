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

// TODO: all concrete Element(s) could be made internal, leaving only the interface as public
// TODO:  ie make all classes internal / expose public interface instead
type ITerminalElement =
    abstract initializeTree: parent: View option -> unit
    abstract canUpdate: prevElement:View -> oldProps: IProps -> bool
    abstract update: prevElement:View -> oldProps: IProps -> unit
    abstract children: List<ITerminalElement> with get
    abstract view: View with get
    abstract props: Props
    abstract name: string

type IMenuv2Element =
    inherit ITerminalElement

type IPopoverMenuElement =
    inherit ITerminalElement

type IMenuBarItemv2Element =
    inherit ITerminalElement

/// Properties key index
module PKey =

    type viewPKeys() =
        member inline this.children : PKey<System.Collections.Generic.List<ITerminalElement>> = PKey "children"

        // Properties
        member inline this.arrangement : PKey<ViewArrangement> = PKey "view.arrangement"
        member inline this.borderStyle : PKey<LineStyle> = PKey "view.borderStyle"
        member inline this.canFocus : PKey<bool> = PKey "view.canFocus"
        member inline this.contentSizeTracksViewport : PKey<bool> = PKey "view.contentSizeTracksViewport"
        member inline this.cursorVisibility : PKey<CursorVisibility> = PKey "view.cursorVisibility"
        member inline this.data : PKey<Object> = PKey "view.data"
        member inline this.enabled : PKey<bool> = PKey "view.enabled"
        member inline this.frame : PKey<Rectangle> = PKey "view.frame"
        member inline this.hasFocus : PKey<bool> = PKey "view.hasFocus"
        member inline this.height : PKey<Dim> = PKey "view.height"
        member inline this.highlightStates : PKey<MouseState> = PKey "view.highlightStates"
        member inline this.hotKey : PKey<Key> = PKey "view.hotKey"
        member inline this.hotKeySpecifier : PKey<Rune> = PKey "view.hotKeySpecifier"
        member inline this.id : PKey<string> = PKey "view.id"
        member inline this.isInitialized : PKey<bool> = PKey "view.isInitialized"
        member inline this.mouseHeldDown : PKey<IMouseHeldDown> = PKey "view.mouseHeldDown"
        member inline this.needsDraw : PKey<bool> = PKey "view.needsDraw"
        member inline this.preserveTrailingSpaces : PKey<bool> = PKey "view.preserveTrailingSpaces"
        member inline this.schemeName : PKey<string> = PKey "view.schemeName"
        member inline this.shadowStyle : PKey<ShadowStyle> = PKey "view.shadowStyle"
        member inline this.superViewRendersLineCanvas : PKey<bool> = PKey "view.superViewRendersLineCanvas"
        member inline this.tabStop : PKey<TabBehavior option> = PKey "view.tabStop"
        member inline this.text : PKey<string> = PKey "view.text"
        member inline this.textAlignment : PKey<Alignment> = PKey "view.textAlignment"
        member inline this.textDirection : PKey<TextDirection> = PKey "view.textDirection"
        member inline this.title : PKey<string> = PKey "view.title"
        member inline this.validatePosDim : PKey<bool> = PKey "view.validatePosDim"
        member inline this.verticalTextAlignment : PKey<Alignment> = PKey "view.verticalTextAlignment"
        member inline this.viewport : PKey<Rectangle> = PKey "view.viewport"
        member inline this.viewportSettings : PKey<ViewportSettingsFlags> = PKey "view.viewportSettings"
        member inline this.visible : PKey<bool> = PKey "view.visible"
        member inline this.wantContinuousButtonPressed : PKey<bool> = PKey "view.wantContinuousButtonPressed"
        member inline this.wantMousePositionReports : PKey<bool> = PKey "view.wantMousePositionReports"
        member inline this.width : PKey<Dim> = PKey "view.width"
        member inline this.x : PKey<Pos> = PKey "view.x"
        member inline this.y : PKey<Pos> = PKey "view.y"
        // Events
        member inline this.accepting : PKey<HandledEventArgs->unit> = PKey "view.accepting"
        member inline this.advancingFocus : PKey<AdvanceFocusEventArgs->unit> = PKey "view.advancingFocus"
        member inline this.borderStyleChanged : PKey<EventArgs->unit> = PKey "view.borderStyleChanged"
        member inline this.canFocusChanged : PKey<unit->unit> = PKey "view.canFocusChanged"
        member inline this.clearedViewport : PKey<DrawEventArgs->unit> = PKey "view.clearedViewport"
        member inline this.clearingViewport : PKey<DrawEventArgs->unit> = PKey "view.clearingViewport"
        member inline this.commandNotBound : PKey<CommandEventArgs->unit> = PKey "view.commandNotBound"
        member inline this.contentSizeChanged : PKey<SizeChangedEventArgs->unit> = PKey "view.contentSizeChanged"
        member inline this.disposing : PKey<unit->unit> = PKey "view.disposing"
        member inline this.drawComplete : PKey<DrawEventArgs->unit> = PKey "view.drawComplete"
        member inline this.drawingContent : PKey<DrawEventArgs->unit> = PKey "view.drawingContent"
        member inline this.drawingSubViews : PKey<DrawEventArgs->unit> = PKey "view.drawingSubViews"
        member inline this.drawingText : PKey<DrawEventArgs->unit> = PKey "view.drawingText"
        member inline this.enabledChanged : PKey<unit->unit> = PKey "view.enabledChanged"
        member inline this.focusedChanged : PKey<HasFocusEventArgs->unit> = PKey "view.focusedChanged"
        member inline this.frameChanged : PKey<EventArgs<Rectangle>->unit> = PKey "view.frameChanged"
        member inline this.gettingAttributeForRole : PKey<VisualRoleEventArgs->unit> = PKey "view.gettingAttributeForRole"
        member inline this.gettingScheme : PKey<ResultEventArgs<Scheme>->unit> = PKey "view.gettingScheme"
        member inline this.handlingHotKey : PKey<CommandEventArgs->unit> = PKey "view.handlingHotKey"
        member inline this.hasFocusChanged : PKey<HasFocusEventArgs->unit> = PKey "view.hasFocusChanged"
        member inline this.hasFocusChanging : PKey<HasFocusEventArgs->unit> = PKey "view.hasFocusChanging"
        member inline this.hotKeyChanged : PKey<KeyChangedEventArgs->unit> = PKey "view.hotKeyChanged"
        member inline this.initialized : PKey<unit->unit> = PKey "view.initialized"
        member inline this.keyDown : PKey<Key->unit> = PKey "view.keyDown"
        member inline this.keyDownNotHandled : PKey<Key->unit> = PKey "view.keyDownNotHandled"
        member inline this.keyUp : PKey<Key->unit> = PKey "view.keyUp"
        member inline this.mouseClick : PKey<MouseEventArgs->unit> = PKey "view.mouseClick"
        member inline this.mouseEnter : PKey<CancelEventArgs->unit> = PKey "view.mouseEnter"
        member inline this.mouseEvent : PKey<MouseEventArgs->unit> = PKey "view.mouseEvent"
        member inline this.mouseLeave : PKey<EventArgs->unit> = PKey "view.mouseLeave"
        member inline this.mouseStateChanged : PKey<EventArgs->unit> = PKey "view.mouseStateChanged"
        member inline this.mouseWheel : PKey<MouseEventArgs->unit> = PKey "view.mouseWheel"
        member inline this.removed : PKey<SuperViewChangedEventArgs->unit> = PKey "view.removed"
        member inline this.schemeChanged : PKey<ValueChangedEventArgs<Scheme>->unit> = PKey "view.schemeChanged"
        member inline this.schemeChanging : PKey<ValueChangingEventArgs<Scheme>->unit> = PKey "view.schemeChanging"
        member inline this.schemeNameChanged : PKey<ValueChangedEventArgs<string>->unit> = PKey "view.schemeNameChanged"
        member inline this.schemeNameChanging : PKey<ValueChangingEventArgs<string>->unit> = PKey "view.schemeNameChanging"
        member inline this.selecting : PKey<CommandEventArgs->unit> = PKey "view.selecting"
        member inline this.subViewAdded : PKey<SuperViewChangedEventArgs->unit> = PKey "view.subViewAdded"
        member inline this.subViewLayout : PKey<LayoutEventArgs->unit> = PKey "view.subViewLayout"
        member inline this.subViewRemoved : PKey<SuperViewChangedEventArgs->unit> = PKey "view.subViewRemoved"
        member inline this.subViewsLaidOut : PKey<LayoutEventArgs->unit> = PKey "view.subViewsLaidOut"
        member inline this.superViewChanged : PKey<SuperViewChangedEventArgs->unit> = PKey "view.superViewChanged"
        member inline this.textChanged : PKey<unit->unit> = PKey "view.textChanged"
        member inline this.titleChanged : PKey<string->unit> = PKey "view.titleChanged"
        member inline this.titleChanging : PKey<App.CancelEventArgs<string>->unit> = PKey "view.titleChanging"
        member inline this.viewportChanged : PKey<DrawEventArgs->unit> = PKey "view.viewportChanged"
        member inline this.visibleChanged : PKey<unit->unit> = PKey "view.visibleChanged"
        member inline this.visibleChanging : PKey<unit->unit> = PKey "view.visibleChanging"

    // Adornment
    type adornmentPKeys() =
        inherit viewPKeys()
        // Properties
        member inline this.diagnostics : PKey<ViewDiagnosticFlags> = PKey "adornment.diagnostics"
        member inline this.superViewRendersLineCanvas : PKey<bool> = PKey "adornment.superViewRendersLineCanvas"
        member inline this.thickness : PKey<Thickness> = PKey "adornment.thickness"
        member inline this.viewport : PKey<Rectangle> = PKey "adornment.viewport"
        // Events
        member inline this.thicknessChanged : PKey<unit->unit> = PKey "adornment.thicknessChanged"

    // Bar
    type barPKeys() =
        inherit viewPKeys()
        // Properties
        member inline this.alignmentModes : PKey<AlignmentModes> = PKey "bar.alignmentModes"
        member inline this.orientation : PKey<Orientation> = PKey "bar.orientation"
        // Events
        member inline this.orientationChanged : PKey<Orientation->unit> = PKey "bar.orientationChanged"
        member inline this.orientationChanging : PKey<App.CancelEventArgs<Orientation>->unit> = PKey "bar.orientationChanging"

    // Border
    type borderPKeys() =
        inherit adornmentPKeys()
        // Properties
        member inline this.lineStyle : PKey<LineStyle> = PKey "border.lineStyle"
        member inline this.settings : PKey<BorderSettings> = PKey "border.settings"

    // Button
    type buttonPKeys() =
        inherit viewPKeys()
        // Properties
        member inline this.hotKeySpecifier : PKey<Rune> = PKey "button.hotKeySpecifier"
        member inline this.isDefault : PKey<bool> = PKey "button.isDefault"
        member inline this.noDecorations : PKey<bool> = PKey "button.noDecorations"
        member inline this.noPadding : PKey<bool> = PKey "button.noPadding"
        member inline this.text : PKey<string> = PKey "button.text"
        member inline this.wantContinuousButtonPressed : PKey<bool> = PKey "button.wantContinuousButtonPressed"

    // CheckBox
    type checkBoxPKeys() =
        inherit viewPKeys()
        // Properties
        member inline this.allowCheckStateNone : PKey<bool> = PKey "checkBox.allowCheckStateNone"
        member inline this.checkedState : PKey<CheckState> = PKey "checkBox.checkedState"
        member inline this.hotKeySpecifier : PKey<Rune> = PKey "checkBox.hotKeySpecifier"
        member inline this.radioStyle : PKey<bool> = PKey "checkBox.radioStyle"
        member inline this.text : PKey<string> = PKey "checkBox.text"
        // Events
        member inline this.checkedStateChanging : PKey<ResultEventArgs<CheckState>->unit> = PKey "checkBox.checkedStateChanging"
        member inline this.checkedStateChanged : PKey<EventArgs<CheckState>->unit> = PKey "checkBox.checkedStateChanged"

    // ColorPicker
    type colorPickerPKeys() =
        inherit viewPKeys()
        // Properties
        member inline this.selectedColor : PKey<Color> = PKey "colorPicker.selectedColor"
        member inline this.style : PKey<ColorPickerStyle> = PKey "colorPicker.style"
        // Events
        member inline this.colorChanged : PKey<ResultEventArgs<Color>->unit> = PKey "colorPicker.colorChanged"

    // ColorPicker16
    type colorPicker16PKeys() =
        inherit viewPKeys()
        // Properties
        member inline this.boxHeight : PKey<Int32> = PKey "colorPicker16.boxHeight"
        member inline this.boxWidth : PKey<Int32> = PKey "colorPicker16.boxWidth"
        member inline this.cursor : PKey<Point> = PKey "colorPicker16.cursor"
        member inline this.selectedColor : PKey<ColorName16> = PKey "colorPicker16.selectedColor"
        // Events
        member inline this.colorChanged : PKey<ResultEventArgs<Color>->unit> = PKey "colorPicker16.colorChanged"

    // ComboBox
    type comboBoxPKeys() =
        inherit viewPKeys()
        // Properties
        member inline this.hideDropdownListOnClick : PKey<bool> = PKey "comboBox.hideDropdownListOnClick"
        member inline this.readOnly : PKey<bool> = PKey "comboBox.readOnly"
        member inline this.searchText : PKey<string> = PKey "comboBox.searchText"
        member inline this.selectedItem : PKey<Int32> = PKey "comboBox.selectedItem"
        member inline this.source : PKey<string list> = PKey "comboBox.source"
        member inline this.text : PKey<string> = PKey "comboBox.text"
        // Events
        member inline this.collapsed : PKey<unit->unit> = PKey "comboBox.collapsed"
        member inline this.expanded : PKey<unit->unit> = PKey "comboBox.expanded"
        member inline this.openSelectedItem : PKey<ListViewItemEventArgs->unit> = PKey "comboBox.openSelectedItem"
        member inline this.selectedItemChanged : PKey<ListViewItemEventArgs->unit> = PKey "comboBox.selectedItemChanged"

    // TextField
    type textFieldPKeys() =
        inherit viewPKeys()
        // Properties
        member inline this.autocomplete : PKey<IAutocomplete> = PKey "textField.autocomplete"
        member inline this.caption : PKey<string> = PKey "textField.caption"
        member inline this.captionColor : PKey<Terminal.Gui.Drawing.Color> = PKey "textField.captionColor"
        member inline this.cursorPosition : PKey<Int32> = PKey "textField.cursorPosition"
        member inline this.readOnly : PKey<bool> = PKey "textField.readOnly"
        member inline this.secret : PKey<bool> = PKey "textField.secret"
        member inline this.selectedStart : PKey<Int32> = PKey "textField.selectedStart"
        member inline this.selectWordOnlyOnDoubleClick : PKey<bool> = PKey "textField.selectWordOnlyOnDoubleClick"
        member inline this.text : PKey<string> = PKey "textField.text"
        member inline this.used : PKey<bool> = PKey "textField.used"
        member inline this.useSameRuneTypeForWords : PKey<bool> = PKey "textField.useSameRuneTypeForWords"
        // Events
        member inline this.textChanging : PKey<ResultEventArgs<string>->unit> = PKey "textField.textChanging"

    // DateField
    type dateFieldPKeys() =
        inherit textFieldPKeys()
        // Properties
        member inline this.culture : PKey<CultureInfo> = PKey "dateField.culture"
        member inline this.cursorPosition : PKey<Int32> = PKey "dateField.cursorPosition"
        member inline this.date : PKey<DateTime> = PKey "dateField.date"
        // Events
        member inline this.dateChanged : PKey<DateTimeEventArgs<DateTime>->unit> = PKey "dateField.dateChanged"

    // DatePicker
    type datePickerPKeys() =
        inherit viewPKeys()
        // Properties
        member inline this.culture : PKey<CultureInfo> = PKey "datePicker.culture"
        member inline this.date : PKey<DateTime> = PKey "datePicker.date"

    // Toplevel
    type toplevelPKeys() =
        inherit viewPKeys()
        // Properties
        member inline this.modal : PKey<bool> = PKey "toplevel.modal"
        member inline this.running : PKey<bool> = PKey "toplevel.running"
        // Events
        member inline this.activate : PKey<ToplevelEventArgs->unit> = PKey "toplevel.activate"
        member inline this.closed : PKey<ToplevelEventArgs->unit> = PKey "toplevel.closed"
        member inline this.closing : PKey<ToplevelClosingEventArgs->unit> = PKey "toplevel.closing"
        member inline this.deactivate : PKey<ToplevelEventArgs->unit> = PKey "toplevel.deactivate"
        member inline this.loaded : PKey<unit->unit> = PKey "toplevel.loaded"
        member inline this.ready : PKey<unit->unit> = PKey "toplevel.ready"
        member inline this.sizeChanging : PKey<SizeChangedEventArgs->unit> = PKey "toplevel.sizeChanging"
        member inline this.unloaded : PKey<unit->unit> = PKey "toplevel.unloaded"

    // Dialog
    type dialogPKeys() =
        inherit toplevelPKeys()
        // Properties
        member inline this.buttonAlignment : PKey<Alignment> = PKey "dialog.buttonAlignment"
        member inline this.buttonAlignmentModes : PKey<AlignmentModes> = PKey "dialog.buttonAlignmentModes"
        member inline this.canceled : PKey<bool> = PKey "dialog.canceled"

    // FileDialog
    type fileDialogPKeys() =
        inherit dialogPKeys()
        // Properties
        member inline this.allowedTypes : PKey<IAllowedType list> = PKey "fileDialog.allowedTypes"
        member inline this.allowsMultipleSelection : PKey<bool> = PKey "fileDialog.allowsMultipleSelection"
        member inline this.fileOperationsHandler : PKey<IFileOperations> = PKey "fileDialog.fileOperationsHandler"
        member inline this.mustExist : PKey<bool> = PKey "fileDialog.mustExist"
        member inline this.openMode : PKey<OpenMode> = PKey "fileDialog.openMode"
        member inline this.path : PKey<string> = PKey "fileDialog.path"
        member inline this.searchMatcher : PKey<ISearchMatcher> = PKey "fileDialog.searchMatcher"
        // Events
        member inline this.filesSelected : PKey<FilesSelectedEventArgs->unit> = PKey "fileDialog.filesSelected"

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
        member inline this.axisX : PKey<HorizontalAxis> = PKey "graphView.axisX"
        member inline this.axisY : PKey<VerticalAxis> = PKey "graphView.axisY"
        member inline this.cellSize : PKey<PointF> = PKey "graphView.cellSize"
        member inline this.graphColor : PKey<Attribute option> = PKey "graphView.graphColor"
        member inline this.marginBottom : PKey<int> = PKey "graphView.marginBottom"
        member inline this.marginLeft : PKey<int> = PKey "graphView.marginLeft"
        member inline this.scrollOffset : PKey<PointF> = PKey "graphView.scrollOffset"

    // HexView
    type hexViewPKeys() =
        inherit viewPKeys()

        // Properties
        member inline this.address : PKey<Int64> = PKey "hexView.address"
        member inline this.addressWidth : PKey<int> = PKey "hexView.addressWidth"
        member inline this.allowEdits : PKey<int> = PKey "hexView.allowEdits"
        member inline this.readOnly : PKey<bool> = PKey "hexView.readOnly"
        member inline this.source : PKey<Stream> = PKey "hexView.source"
        // Events
        member inline this.edited : PKey<HexViewEditEventArgs->unit> = PKey "hexView.edited"
        member inline this.positionChanged : PKey<HexViewEventArgs->unit> = PKey "hexView.positionChanged"

    // Label
    type labelPKeys() =
        inherit viewPKeys()

        // Properties
        member inline this.hotKeySpecifier : PKey<Rune> = PKey "label.hotKeySpecifier"
        member inline this.text : PKey<string> = PKey "label.text"

    // LegendAnnotation
    type legendAnnotationPKeys() =
        inherit viewPKeys()
    // No properties or events LegendAnnotation

    // Line
    type linePKeys() =
        inherit viewPKeys()

        // Properties
        member inline this.orientation : PKey<Orientation> = PKey "line.orientation"
        // Events
        member inline this.orientationChanged : PKey<Orientation->unit> = PKey "line.orientationChanged"
        member inline this.orientationChanging : PKey<App.CancelEventArgs<Orientation>->unit> = PKey "line.orientationChanging"

    // LineView
    type lineViewPKeys() =
        inherit viewPKeys()

        // Properties
        member inline this.endingAnchor : PKey<Rune option> = PKey "lineView.endingAnchor"
        member inline this.lineRune : PKey<Rune> = PKey "lineView.lineRune"
        member inline this.orientation : PKey<Orientation> = PKey "lineView.orientation"
        member inline this.startingAnchor : PKey<Rune option> = PKey "lineView.startingAnchor"

    // ListView
    type listViewPKeys() =
        inherit viewPKeys()

        // Properties
        member inline this.allowsMarking : PKey<bool> = PKey "listView.allowsMarking"
        member inline this.allowsMultipleSelection : PKey<bool> = PKey "listView.allowsMultipleSelection"
        member inline this.leftItem : PKey<Int32> = PKey "listView.leftItem"
        member inline this.selectedItem : PKey<Int32> = PKey "listView.selectedItem"
        member inline this.source : PKey<string list> = PKey "listView.source"
        member inline this.topItem : PKey<Int32> = PKey "listView.topItem"
        // Events
        member inline this.collectionChanged : PKey<NotifyCollectionChangedEventArgs->unit> = PKey "listView.collectionChanged"
        member inline this.openSelectedItem : PKey<ListViewItemEventArgs->unit> = PKey "listView.openSelectedItem"
        member inline this.rowRender : PKey<ListViewRowEventArgs->unit> = PKey "listView.rowRender"
        member inline this.selectedItemChanged : PKey<ListViewItemEventArgs->unit> = PKey "listView.selectedItemChanged"

    // Margin
    type marginPKeys() =
        inherit adornmentPKeys()

        // Properties
        member inline this.shadowStyle : PKey<ShadowStyle> = PKey "margin.shadowStyle"

    type menuv2PKeys() =
        inherit barPKeys()

        // Properties
        member inline this.selectedMenuItem : PKey<MenuItemv2> = PKey "menuv2.selectedMenuItem"
        member inline this.superMenuItem : PKey<MenuItemv2> = PKey "menuv2.superMenuItem"
        // Events
        member inline this.accepted : PKey< CommandEventArgs->unit> = PKey "menuv2.accepted"
        member inline this.selectedMenuItemChanged : PKey< MenuItemv2->unit> = PKey "menuv2.selectedMenuItemChanged"

    // MenuBarV2
    type menuBarv2PKeys() =
        inherit menuv2PKeys()

        // Properties
        member inline this.key : PKey<Key> = PKey "menuBarv2.key"
        member inline this.menus : PKey<MenuBarItemv2 array> = PKey "view.menus"
        // Events
        member inline this.keyChanged : PKey<KeyChangedEventArgs->unit> = PKey "menuBarv2.keyChanged"

    type shortcutPKeys() =
         inherit viewPKeys()

         // Properties
         member inline this.action : PKey<Action> = PKey "shortcut.action"
         member inline this.alignmentModes : PKey<AlignmentModes> = PKey "shortcut.alignmentModes"
         member inline this.commandView : PKey<View> = PKey "shortcut.commandView"
         member inline this.commandView_element : PKey<ITerminalElement> = PKey "shortcut.commandView_element"
         member inline this.forceFocusColors : PKey<bool> = PKey "shortcut.forceFocusColors"
         member inline this.helpText : PKey<string> = PKey "shortcut.helpText"
         member inline this.text : PKey<string> = PKey "shortcut.text"
         member inline this.bindKeyToApplication : PKey<bool> = PKey "shortcut.bindKeyToApplication"
         member inline this.key : PKey<Key> = PKey "shortcut.key"
         member inline this.minimumKeyTextSize : PKey<Int32> = PKey "shortcut.minimumKeyTextSize"
         // Events
         member inline this.orientationChanged : PKey<Orientation->unit> = PKey "shortcut.orientationChanged"
         member inline this.orientationChanging : PKey<CancelEventArgs<Orientation>->unit> = PKey "shortcut.orientationChanging"

    type menuItemv2PKeys() =
        inherit shortcutPKeys()
        member inline this.command : PKey< Command> = PKey "menuItemv2.command"
        member inline this.subMenu: PKey<Menuv2> = PKey "menuItemv2.subMenu"
        member inline this.subMenu_element : PKey<IMenuv2Element> = PKey "menuItemv2.subMenu_element"
        member inline this.targetView : PKey<View> = PKey "menuItemv2.targetView"
        member inline this.accepted: PKey<CommandEventArgs -> unit> = PKey "menuItemv2.accepted"

    type menuBarItemv2PKeys() =
        inherit menuItemv2PKeys()

        // Properties
        member inline this.popoverMenu : PKey<PopoverMenu> = PKey "menuBarItemv2.popoverMenu"
        member inline this.popoverMenu_element : PKey<IPopoverMenuElement> = PKey "menuBarItemv2.popoverMenu_element"
        member inline this.popoverMenuOpen : PKey<bool> = PKey "menuBarItemv2.popoverMenuOpen"
        // Events
        member inline this.popoverMenuOpenChanged : PKey<bool->unit> = PKey "menuBarItemv2.popoverMenuOpenChanged"

    type popoverMenuPKeys() =
        inherit viewPKeys()

        // Properties
        member inline this.key : PKey<Key> = PKey "popoverMenu.key"
        member inline this.mouseFlags : PKey<MouseFlags> = PKey "popoverMenu.mouseFlags"
        member inline this.root : PKey<Menuv2> = PKey "popoverMenu.root"
        member inline this.root_element : PKey<IMenuv2Element> = PKey "popoverMenu.root_element"
        // Events
        member inline this.accepted : PKey<CommandEventArgs->unit> = PKey "popoverMenu.accepted"
        member inline this.keyChanged : PKey<KeyChangedEventArgs->unit> = PKey "popoverMenu.keyChanged"

    // NumericUpDown`1
    type numericUpDownPKeys<'a>() =
        inherit viewPKeys()

        // Properties
        member inline this.format : PKey<string> = PKey "numericUpDown.format"
        member inline this.increment : PKey<'a> = PKey "numericUpDown.increment"
        member inline this.value : PKey<'a> = PKey "numericUpDown.value"
        // Events
        member inline this.formatChanged : PKey<string->unit> = PKey "numericUpDown.formatChanged"
        member inline this.incrementChanged : PKey<'a->unit> = PKey "numericUpDown.incrementChanged"
        member inline this.valueChanged : PKey<'a->unit> = PKey "numericUpDown.valueChanged"
        member inline this.valueChanging : PKey<App.CancelEventArgs<'a>->unit> = PKey "numericUpDown.valueChanging"

    // NumericUpDown
    // type numericUpDownPKeys() =
    //     inherit numericUpDownPKeys<int>()
    // No properties or events NumericUpDown

    // OpenDialog
    type openDialogPKeys() =
        inherit fileDialogPKeys()
        // Properties
        member inline this.openMode : PKey<OpenMode> = PKey "openDialog.openMode"

    // OptionSelector
    type optionSelectorPKeys() =
        inherit viewPKeys()
        //Properties
        member inline this.assignHotKeysToCheckBoxes : PKey<bool> = PKey "optionSelector.assignHotKeysToCheckBoxes"
        member inline this.orientation : PKey<Orientation> = PKey "optionSelector.orientation"
        member inline this.options : PKey<IReadOnlyList<string>> = PKey "optionSelector.options"
        member inline this.selectedItem : PKey<Int32> = PKey "optionSelector.selectedItem"
        // Events
        member inline this.orientationChanged : PKey<Orientation->unit> = PKey "optionSelector.orientationChanged"
        member inline this.orientationChanging : PKey<CancelEventArgs<Orientation>->unit> = PKey "optionSelector.orientationChanging"
        member inline this.selectedItemChanged : PKey<SelectedItemChangedArgs->unit> = PKey "optionSelector.selectedItemChanged"

    // Padding
    type paddingPKeys() =
        inherit adornmentPKeys()

    // ProgressBar
    type progressBarPKeys() =
        inherit viewPKeys()

        // Properties
        member inline this.bidirectionalMarquee : PKey<bool> = PKey "progressBar.bidirectionalMarquee"
        member inline this.fraction : PKey<Single> = PKey "progressBar.fraction"
        member inline this.progressBarFormat : PKey<ProgressBarFormat> = PKey "progressBar.progressBarFormat"
        member inline this.progressBarStyle : PKey<ProgressBarStyle> = PKey "progressBar.progressBarStyle"
        member inline this.segmentCharacter : PKey<Rune> = PKey "progressBar.segmentCharacter"
        member inline this.text : PKey<string> = PKey "progressBar.text"

    // RadioGroup
    type radioGroupPKeys() =
        inherit viewPKeys()

        // Properties
        member inline this.assignHotKeysToRadioLabels : PKey<bool> = PKey "radioGroup.assignHotKeysToRadioLabels"
        member inline this.cursor : PKey<Int32> = PKey "radioGroup.cursor"
        member inline this.doubleClickAccepts : PKey<bool> = PKey "radioGroup.doubleClickAccepts"
        member inline this.horizontalSpace : PKey<Int32> = PKey "radioGroup.horizontalSpace"
        member inline this.orientation : PKey<Orientation> = PKey "radioGroup.orientation"
        member inline this.radioLabels : PKey<string list> = PKey "radioGroup.radioLabels"
        member inline this.selectedItem : PKey<Int32> = PKey "radioGroup.selectedItem"
        // Events
        member inline this.orientationChanged : PKey<Orientation->unit> = PKey "radioGroup.orientationChanged"
        member inline this.orientationChanging : PKey<App.CancelEventArgs<Orientation>->unit> = PKey "radioGroup.orientationChanging"
        member inline this.selectedItemChanged : PKey<SelectedItemChangedArgs->unit> = PKey "radioGroup.selectedItemChanged"

    // SaveDialog
    // No properties or events SaveDialog

    // ScrollBar
    type scrollBarPKeys() =
        inherit viewPKeys()

        // Properties
        member inline this.autoShow : PKey<bool> = PKey "scrollBar.autoShow"
        member inline this.increment : PKey<Int32> = PKey "scrollBar.increment"
        member inline this.orientation : PKey<Orientation> = PKey "scrollBar.orientation"
        member inline this.position : PKey<Int32> = PKey "scrollBar.position"
        member inline this.scrollableContentSize : PKey<Int32> = PKey "scrollBar.scrollableContentSize"
        member inline this.visibleContentSize : PKey<Int32> = PKey "scrollBar.visibleContentSize"
        // Events
        member inline this.orientationChanged : PKey<Orientation->unit> = PKey "scrollBar.orientationChanged"
        member inline this.orientationChanging : PKey<CancelEventArgs<Orientation>->unit> = PKey "scrollBar.orientationChanging"
        member inline this.scrollableContentSizeChanged : PKey<EventArgs<Int32>->unit> = PKey "scrollBar.scrollableContentSizeChanged"
        member inline this.sliderPositionChanged : PKey<EventArgs<Int32>->unit> = PKey "scrollBar.sliderPositionChanged"

    // ScrollSlider
    type scrollSliderPKeys() =
        inherit viewPKeys()

        // Properties
        member inline this.orientation : PKey<Orientation> = PKey "scrollSlider.orientation"
        member inline this.position : PKey<Int32> = PKey "scrollSlider.position"
        member inline this.size : PKey<Int32> = PKey "scrollSlider.size"
        member inline this.sliderPadding : PKey<Int32> = PKey "scrollSlider.sliderPadding"
        member inline this.visibleContentSize : PKey<Int32> = PKey "scrollSlider.visibleContentSize"
        // Events
        member inline this.orientationChanged : PKey<Orientation->unit> = PKey "scrollSlider.orientationChanged"
        member inline this.orientationChanging : PKey<CancelEventArgs<Orientation>->unit> = PKey "scrollSlider.orientationChanging"
        member inline this.positionChanged : PKey<EventArgs<Int32>->unit> = PKey "scrollSlider.positionChanged"
        member inline this.positionChanging : PKey<CancelEventArgs<Int32>->unit> = PKey "scrollSlider.positionChanging"
        member inline this.scrolled : PKey<EventArgs<Int32>->unit> = PKey "scrollSlider.scrolled"

    // Slider`1
    type sliderPKeys<'a>() =
        inherit viewPKeys()

        // Properties
        member inline this.allowEmpty : PKey<bool> = PKey "slider.allowEmpty"
        member inline this.focusedOption : PKey<Int32> = PKey "slider.focusedOption"
        member inline this.legendsOrientation : PKey<Orientation> = PKey "slider.legendsOrientation"
        member inline this.minimumInnerSpacing : PKey<Int32> = PKey "slider.minimumInnerSpacing"
        member inline this.options : PKey<SliderOption<'a> list> = PKey "slider.options"
        member inline this.orientation : PKey<Orientation> = PKey "slider.orientation"
        member inline this.rangeAllowSingle : PKey<bool> = PKey "slider.rangeAllowSingle"
        member inline this.showEndSpacing : PKey<bool> = PKey "slider.showEndSpacing"
        member inline this.showLegends : PKey<bool> = PKey "slider.showLegends"
        member inline this.style : PKey<SliderStyle> = PKey "slider.style"
        member inline this.text : PKey<string> = PKey "slider.text"
        member inline this.``type`` : PKey<SliderType> = PKey "slider.``type``"
        member inline this.useMinimumSize : PKey<bool> = PKey "slider.useMinimumSize"
        // Events
        member inline this.optionFocused : PKey<SliderEventArgs<'a>->unit> = PKey "slider.optionFocused"
        member inline this.optionsChanged : PKey<SliderEventArgs<'a>->unit> = PKey "slider.optionsChanged"
        member inline this.orientationChanged : PKey<Orientation->unit> = PKey "slider.orientationChanged"
        member inline this.orientationChanging : PKey<App.CancelEventArgs<Orientation>->unit> = PKey "slider.orientationChanging"

    // Slider
    // type sliderPKeys() =
    //     inherit sliderPKeys<obj>()
    // No properties or events Slider

    // SpinnerView
    type spinnerViewPKeys() =
        inherit viewPKeys()

        // Properties
        member inline this.autoSpin : PKey<bool> = PKey "spinnerView.autoSpin"
        member inline this.sequence : PKey<string list> = PKey "spinnerView.sequence"
        member inline this.spinBounce : PKey<bool> = PKey "spinnerView.spinBounce"
        member inline this.spinDelay : PKey<Int32> = PKey "spinnerView.spinDelay"
        member inline this.spinReverse : PKey<bool> = PKey "spinnerView.spinReverse"
        member inline this.style : PKey<SpinnerStyle> = PKey "spinnerView.style"

    // StatusBar
    type statusBarPKeys() =
        inherit barPKeys()
    // No properties or events StatusBar

    // Tab
    type tabPKeys() =
        inherit viewPKeys()

        // Properties
        member inline this.displayText : PKey<string> = PKey "tab.displayText"
        member inline this.view : PKey<View> = PKey "tab.view"
        member inline this.view_element : PKey<ITerminalElement> = PKey "tab.view_element"

    // TabView
    type tabViewPKeys() =
        inherit viewPKeys()

        // Properties
        member inline this.maxTabTextWidth : PKey<int> = PKey "tabView.maxTabTextWidth"
        member inline this.selectedTab : PKey<Tab> = PKey "tabView.selectedTab"
        member inline this.style : PKey<TabStyle> = PKey "tabView.style"
        member inline this.tabScrollOffset : PKey<Int32> = PKey "tabView.tabScrollOffset"
        // Events
        member inline this.selectedTabChanged : PKey<TabChangedEventArgs->unit> = PKey "tabView.selectedTabChanged"
        member inline this.tabClicked : PKey<TabMouseEventArgs->unit> = PKey "tabView.tabClicked"
        // Additional properties
        member inline this.tabs : PKey<List<ITerminalElement>> = PKey "tabView.tabs"
        member inline this.tabs_elements : PKey<List<ITerminalElement>> = PKey "tabView.tabs_elements"

    // TableView
    type tableViewPKeys() =
        inherit viewPKeys()

        // Properties
        member inline this.cellActivationKey : PKey<KeyCode> = PKey "tableView.cellActivationKey"
        member inline this.collectionNavigator : PKey<ICollectionNavigator> = PKey "tableView.collectionNavigator"
        member inline this.columnOffset : PKey<Int32> = PKey "tableView.columnOffset"
        member inline this.fullRowSelect : PKey<bool> = PKey "tableView.fullRowSelect"
        member inline this.maxCellWidth : PKey<Int32> = PKey "tableView.maxCellWidth"
        member inline this.minCellWidth : PKey<Int32> = PKey "tableView.minCellWidth"
        member inline this.multiSelect : PKey<bool> = PKey "tableView.multiSelect"
        member inline this.nullSymbol : PKey<string> = PKey "tableView.nullSymbol"
        member inline this.rowOffset : PKey<Int32> = PKey "tableView.rowOffset"
        member inline this.selectedColumn : PKey<Int32> = PKey "tableView.selectedColumn"
        member inline this.selectedRow : PKey<Int32> = PKey "tableView.selectedRow"
        member inline this.separatorSymbol : PKey<Char> = PKey "tableView.separatorSymbol"
        member inline this.style : PKey<TableStyle> = PKey "tableView.style"
        member inline this.table : PKey<ITableSource> = PKey "tableView.table"
        // Events
        member inline this.cellActivated : PKey<CellActivatedEventArgs->unit> = PKey "tableView.cellActivated"
        member inline this.cellToggled : PKey<CellToggledEventArgs->unit> = PKey "tableView.cellToggled"
        member inline this.selectedCellChanged : PKey<SelectedCellChangedEventArgs->unit> = PKey "tableView.selectedCellChanged"

    // TextValidateField
    type textValidateFieldPKeys() =
        inherit viewPKeys()

        // Properties
        member inline this.provider : PKey<ITextValidateProvider> = PKey "textValidateField.provider"
        member inline this.text : PKey<string> = PKey "textValidateField.text"

    // TextView
    type textViewPKeys() =
        inherit viewPKeys()

        // Properties
        member inline this.allowsReturn : PKey<bool> = PKey "textView.allowsReturn"
        member inline this.allowsTab : PKey<bool> = PKey "textView.allowsTab"
        member inline this.cursorPosition : PKey<Point> = PKey "textView.cursorPosition"
        member inline this.inheritsPreviousAttribute : PKey<bool> = PKey "textView.inheritsPreviousAttribute"
        member inline this.isDirty : PKey<bool> = PKey "textView.isDirty"
        member inline this.isSelecting : PKey<bool> = PKey "textView.isSelecting"
        member inline this.leftColumn : PKey<Int32> = PKey "textView.leftColumn"
        member inline this.multiline : PKey<bool> = PKey "textView.multiline"
        member inline this.readOnly : PKey<bool> = PKey "textView.readOnly"
        member inline this.selectionStartColumn : PKey<Int32> = PKey "textView.selectionStartColumn"
        member inline this.selectionStartRow : PKey<Int32> = PKey "textView.selectionStartRow"
        member inline this.selectWordOnlyOnDoubleClick : PKey<bool> = PKey "textView.selectWordOnlyOnDoubleClick"
        member inline this.tabWidth : PKey<Int32> = PKey "textView.tabWidth"
        member inline this.text : PKey<string> = PKey "textView.text"
        member inline this.topRow : PKey<Int32> = PKey "textView.topRow"
        member inline this.used : PKey<bool> = PKey "textView.used"
        member inline this.useSameRuneTypeForWords : PKey<bool> = PKey "textView.useSameRuneTypeForWords"
        member inline this.wordWrap : PKey<bool> = PKey "textView.wordWrap"
        // Events
        member inline this.contentsChanged : PKey<ContentsChangedEventArgs->unit> = PKey "textView.contentsChanged"
        member inline this.drawNormalColor : PKey<CellEventArgs->unit> = PKey "textView.drawNormalColor"
        member inline this.drawReadOnlyColor : PKey<CellEventArgs->unit> = PKey "textView.drawReadOnlyColor"
        member inline this.drawSelectionColor : PKey<CellEventArgs->unit> = PKey "textView.drawSelectionColor"
        member inline this.drawUsedColor : PKey<CellEventArgs->unit> = PKey "textView.drawUsedColor"
        member inline this.unwrappedCursorPosition : PKey<Point->unit> = PKey "textView.unwrappedCursorPosition"
        // Additional properties
        member inline this.textChanged : PKey<string->unit> = PKey "textView.textChanged"

    // TileView
    type tileViewPKeys() =
        inherit viewPKeys()

        // Properties
        member inline this.lineStyle : PKey<LineStyle> = PKey "tileView.lineStyle"
        member inline this.orientation : PKey<Orientation> = PKey "tileView.orientation"
        member inline this.toggleResizable : PKey<KeyCode> = PKey "tileView.toggleResizable"
        // Events
        member inline this.splitterMoved : PKey<SplitterEventArgs->unit> = PKey "tileView.splitterMoved"

    // TimeField
    type timeFieldPKeys() =
        inherit textFieldPKeys()

        // Properties
        member inline this.cursorPosition : PKey<Int32> = PKey "timeField.cursorPosition"
        member inline this.isShortFormat : PKey<bool> = PKey "timeField.isShortFormat"
        member inline this.time : PKey<TimeSpan> = PKey "timeField.time"
        // Events
        member inline this.timeChanged : PKey<DateTimeEventArgs<TimeSpan>->unit> = PKey "timeField.timeChanged"

    // TreeView`1
    type treeViewPKeys<'a when 'a : not struct>() =
        inherit viewPKeys()

        // Properties
        member inline this.allowLetterBasedNavigation : PKey<bool> = PKey "treeView.allowLetterBasedNavigation"
        member inline this.aspectGetter : PKey<AspectGetterDelegate<'a>> = PKey "treeView.aspectGetter"
        member inline this.colorGetter : PKey<Func<'a,Scheme>> = PKey "treeView.colorGetter"
        member inline this.maxDepth : PKey<Int32> = PKey "treeView.maxDepth"
        member inline this.multiSelect : PKey<bool> = PKey "treeView.multiSelect"
        member inline this.objectActivationButton : PKey<MouseFlags option> = PKey "treeView.objectActivationButton"
        member inline this.objectActivationKey : PKey<KeyCode> = PKey "treeView.objectActivationKey"
        member inline this.scrollOffsetHorizontal : PKey<Int32> = PKey "treeView.scrollOffsetHorizontal"
        member inline this.scrollOffsetVertical : PKey<Int32> = PKey "treeView.scrollOffsetVertical"
        member inline this.selectedObject : PKey<'a> = PKey "treeView.selectedObject"
        member inline this.style : PKey<TreeStyle> = PKey "treeView.style"
        member inline this.treeBuilder : PKey<ITreeBuilder<'a>> = PKey "treeView.treeBuilder"
        // Events
        member inline this.drawLine : PKey<DrawTreeViewLineEventArgs<'a>->unit> = PKey "treeView.drawLine"
        member inline this.objectActivated : PKey<ObjectActivatedEventArgs<'a>->unit> = PKey "treeView.objectActivated"
        member inline this.selectionChanged : PKey<SelectionChangedEventArgs<'a>->unit> = PKey "treeView.selectionChanged"

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
        member inline this.currentStep : PKey<WizardStep> = PKey "wizard.currentStep"
        member inline this.modal : PKey<bool> = PKey "wizard.modal"
        // Events
        member inline this.cancelled : PKey<WizardButtonEventArgs->unit> = PKey "wizard.cancelled"
        member inline this.finished : PKey<WizardButtonEventArgs->unit> = PKey "wizard.finished"
        member inline this.movingBack : PKey<WizardButtonEventArgs->unit> = PKey "wizard.movingBack"
        member inline this.movingNext : PKey<WizardButtonEventArgs->unit> = PKey "wizard.movingNext"
        member inline this.stepChanged : PKey<StepChangeEventArgs->unit> = PKey "wizard.stepChanged"
        member inline this.stepChanging : PKey<StepChangeEventArgs->unit> = PKey "wizard.stepChanging"

    // WizardStep
    type wizardStepPKeys() =
        inherit viewPKeys()

        // Properties
        member inline this.backButtonText : PKey<string> = PKey "wizardStep.backButtonText"
        member inline this.helpText : PKey<string> = PKey "wizardStep.helpText"
        member inline this.nextButtonText : PKey<string> = PKey "wizardStep.nextButtonText"


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
