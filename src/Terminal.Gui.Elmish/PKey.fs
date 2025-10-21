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
    // TODO: rename to prevView
    abstract canUpdate: prevElement:View -> oldProps: Props -> bool
    // TODO: rename to prevView
    abstract update: prevElement:View -> oldProps: Props -> unit
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
        member inline this.children : PropertyKey<System.Collections.Generic.List<ITerminalElement>> = PropertyKey "children"

        // Properties
        member inline this.arrangement : PropertyKey<ViewArrangement> = PropertyKey "view.arrangement"
        member inline this.borderStyle : PropertyKey<LineStyle> = PropertyKey "view.borderStyle"
        member inline this.canFocus : PropertyKey<bool> = PropertyKey "view.canFocus"
        member inline this.contentSizeTracksViewport : PropertyKey<bool> = PropertyKey "view.contentSizeTracksViewport"
        member inline this.cursorVisibility : PropertyKey<CursorVisibility> = PropertyKey "view.cursorVisibility"
        member inline this.data : PropertyKey<Object> = PropertyKey "view.data"
        member inline this.enabled : PropertyKey<bool> = PropertyKey "view.enabled"
        member inline this.frame : PropertyKey<Rectangle> = PropertyKey "view.frame"
        member inline this.hasFocus : PropertyKey<bool> = PropertyKey "view.hasFocus"
        member inline this.height : PropertyKey<Dim> = PropertyKey "view.height"
        member inline this.highlightStates : PropertyKey<MouseState> = PropertyKey "view.highlightStates"
        member inline this.hotKey : PropertyKey<Key> = PropertyKey "view.hotKey"
        member inline this.hotKeySpecifier : PropertyKey<Rune> = PropertyKey "view.hotKeySpecifier"
        member inline this.id : PropertyKey<string> = PropertyKey "view.id"
        member inline this.isInitialized : PropertyKey<bool> = PropertyKey "view.isInitialized"
        member inline this.mouseHeldDown : PropertyKey<IMouseHeldDown> = PropertyKey "view.mouseHeldDown"
        member inline this.needsDraw : PropertyKey<bool> = PropertyKey "view.needsDraw"
        member inline this.preserveTrailingSpaces : PropertyKey<bool> = PropertyKey "view.preserveTrailingSpaces"
        member inline this.schemeName : PropertyKey<string> = PropertyKey "view.schemeName"
        member inline this.shadowStyle : PropertyKey<ShadowStyle> = PropertyKey "view.shadowStyle"
        member inline this.superViewRendersLineCanvas : PropertyKey<bool> = PropertyKey "view.superViewRendersLineCanvas"
        member inline this.tabStop : PropertyKey<TabBehavior option> = PropertyKey "view.tabStop"
        member inline this.text : PropertyKey<string> = PropertyKey "view.text"
        member inline this.textAlignment : PropertyKey<Alignment> = PropertyKey "view.textAlignment"
        member inline this.textDirection : PropertyKey<TextDirection> = PropertyKey "view.textDirection"
        member inline this.title : PropertyKey<string> = PropertyKey "view.title"
        member inline this.validatePosDim : PropertyKey<bool> = PropertyKey "view.validatePosDim"
        member inline this.verticalTextAlignment : PropertyKey<Alignment> = PropertyKey "view.verticalTextAlignment"
        member inline this.viewport : PropertyKey<Rectangle> = PropertyKey "view.viewport"
        member inline this.viewportSettings : PropertyKey<ViewportSettingsFlags> = PropertyKey "view.viewportSettings"
        member inline this.visible : PropertyKey<bool> = PropertyKey "view.visible"
        member inline this.wantContinuousButtonPressed : PropertyKey<bool> = PropertyKey "view.wantContinuousButtonPressed"
        member inline this.wantMousePositionReports : PropertyKey<bool> = PropertyKey "view.wantMousePositionReports"
        member inline this.width : PropertyKey<Dim> = PropertyKey "view.width"
        member inline this.x : PropertyKey<Pos> = PropertyKey "view.x"
        member inline this.y : PropertyKey<Pos> = PropertyKey "view.y"
        // Events
        member inline this.accepting : PropertyKey<HandledEventArgs->unit> = PropertyKey "view.accepting"
        member inline this.advancingFocus : PropertyKey<AdvanceFocusEventArgs->unit> = PropertyKey "view.advancingFocus"
        member inline this.borderStyleChanged : PropertyKey<EventArgs->unit> = PropertyKey "view.borderStyleChanged"
        member inline this.canFocusChanged : PropertyKey<unit->unit> = PropertyKey "view.canFocusChanged"
        member inline this.clearedViewport : PropertyKey<DrawEventArgs->unit> = PropertyKey "view.clearedViewport"
        member inline this.clearingViewport : PropertyKey<DrawEventArgs->unit> = PropertyKey "view.clearingViewport"
        member inline this.commandNotBound : PropertyKey<CommandEventArgs->unit> = PropertyKey "view.commandNotBound"
        member inline this.contentSizeChanged : PropertyKey<SizeChangedEventArgs->unit> = PropertyKey "view.contentSizeChanged"
        member inline this.disposing : PropertyKey<unit->unit> = PropertyKey "view.disposing"
        member inline this.drawComplete : PropertyKey<DrawEventArgs->unit> = PropertyKey "view.drawComplete"
        member inline this.drawingContent : PropertyKey<DrawEventArgs->unit> = PropertyKey "view.drawingContent"
        member inline this.drawingSubViews : PropertyKey<DrawEventArgs->unit> = PropertyKey "view.drawingSubViews"
        member inline this.drawingText : PropertyKey<DrawEventArgs->unit> = PropertyKey "view.drawingText"
        member inline this.enabledChanged : PropertyKey<unit->unit> = PropertyKey "view.enabledChanged"
        member inline this.focusedChanged : PropertyKey<HasFocusEventArgs->unit> = PropertyKey "view.focusedChanged"
        member inline this.frameChanged : PropertyKey<EventArgs<Rectangle>->unit> = PropertyKey "view.frameChanged"
        member inline this.gettingAttributeForRole : PropertyKey<VisualRoleEventArgs->unit> = PropertyKey "view.gettingAttributeForRole"
        member inline this.gettingScheme : PropertyKey<ResultEventArgs<Scheme>->unit> = PropertyKey "view.gettingScheme"
        member inline this.handlingHotKey : PropertyKey<CommandEventArgs->unit> = PropertyKey "view.handlingHotKey"
        member inline this.hasFocusChanged : PropertyKey<HasFocusEventArgs->unit> = PropertyKey "view.hasFocusChanged"
        member inline this.hasFocusChanging : PropertyKey<HasFocusEventArgs->unit> = PropertyKey "view.hasFocusChanging"
        member inline this.hotKeyChanged : PropertyKey<KeyChangedEventArgs->unit> = PropertyKey "view.hotKeyChanged"
        member inline this.initialized : PropertyKey<unit->unit> = PropertyKey "view.initialized"
        member inline this.keyDown : PropertyKey<Key->unit> = PropertyKey "view.keyDown"
        member inline this.keyDownNotHandled : PropertyKey<Key->unit> = PropertyKey "view.keyDownNotHandled"
        member inline this.keyUp : PropertyKey<Key->unit> = PropertyKey "view.keyUp"
        member inline this.mouseClick : PropertyKey<MouseEventArgs->unit> = PropertyKey "view.mouseClick"
        member inline this.mouseEnter : PropertyKey<CancelEventArgs->unit> = PropertyKey "view.mouseEnter"
        member inline this.mouseEvent : PropertyKey<MouseEventArgs->unit> = PropertyKey "view.mouseEvent"
        member inline this.mouseLeave : PropertyKey<EventArgs->unit> = PropertyKey "view.mouseLeave"
        member inline this.mouseStateChanged : PropertyKey<EventArgs->unit> = PropertyKey "view.mouseStateChanged"
        member inline this.mouseWheel : PropertyKey<MouseEventArgs->unit> = PropertyKey "view.mouseWheel"
        member inline this.removed : PropertyKey<SuperViewChangedEventArgs->unit> = PropertyKey "view.removed"
        member inline this.schemeChanged : PropertyKey<ValueChangedEventArgs<Scheme>->unit> = PropertyKey "view.schemeChanged"
        member inline this.schemeChanging : PropertyKey<ValueChangingEventArgs<Scheme>->unit> = PropertyKey "view.schemeChanging"
        member inline this.schemeNameChanged : PropertyKey<ValueChangedEventArgs<string>->unit> = PropertyKey "view.schemeNameChanged"
        member inline this.schemeNameChanging : PropertyKey<ValueChangingEventArgs<string>->unit> = PropertyKey "view.schemeNameChanging"
        member inline this.selecting : PropertyKey<CommandEventArgs->unit> = PropertyKey "view.selecting"
        member inline this.subViewAdded : PropertyKey<SuperViewChangedEventArgs->unit> = PropertyKey "view.subViewAdded"
        member inline this.subViewLayout : PropertyKey<LayoutEventArgs->unit> = PropertyKey "view.subViewLayout"
        member inline this.subViewRemoved : PropertyKey<SuperViewChangedEventArgs->unit> = PropertyKey "view.subViewRemoved"
        member inline this.subViewsLaidOut : PropertyKey<LayoutEventArgs->unit> = PropertyKey "view.subViewsLaidOut"
        member inline this.superViewChanged : PropertyKey<SuperViewChangedEventArgs->unit> = PropertyKey "view.superViewChanged"
        member inline this.textChanged : PropertyKey<unit->unit> = PropertyKey "view.textChanged"
        member inline this.titleChanged : PropertyKey<string->unit> = PropertyKey "view.titleChanged"
        member inline this.titleChanging : PropertyKey<App.CancelEventArgs<string>->unit> = PropertyKey "view.titleChanging"
        member inline this.viewportChanged : PropertyKey<DrawEventArgs->unit> = PropertyKey "view.viewportChanged"
        member inline this.visibleChanged : PropertyKey<unit->unit> = PropertyKey "view.visibleChanged"
        member inline this.visibleChanging : PropertyKey<unit->unit> = PropertyKey "view.visibleChanging"

    // Adornment
    type adornmentPKeys() =
        inherit viewPKeys()
        // Properties
        member inline this.diagnostics : PropertyKey<ViewDiagnosticFlags> = PropertyKey "adornment.diagnostics"
        member inline this.superViewRendersLineCanvas : PropertyKey<bool> = PropertyKey "adornment.superViewRendersLineCanvas"
        member inline this.thickness : PropertyKey<Thickness> = PropertyKey "adornment.thickness"
        member inline this.viewport : PropertyKey<Rectangle> = PropertyKey "adornment.viewport"
        // Events
        member inline this.thicknessChanged : PropertyKey<unit->unit> = PropertyKey "adornment.thicknessChanged"

    // Bar
    type barPKeys() =
        inherit viewPKeys()
        // Properties
        member inline this.alignmentModes : PropertyKey<AlignmentModes> = PropertyKey "bar.alignmentModes"
        member inline this.orientation : PropertyKey<Orientation> = PropertyKey "bar.orientation"
        // Events
        member inline this.orientationChanged : PropertyKey<Orientation->unit> = PropertyKey "bar.orientationChanged"
        member inline this.orientationChanging : PropertyKey<App.CancelEventArgs<Orientation>->unit> = PropertyKey "bar.orientationChanging"

    // Border
    type borderPKeys() =
        inherit adornmentPKeys()
        // Properties
        member inline this.lineStyle : PropertyKey<LineStyle> = PropertyKey "border.lineStyle"
        member inline this.settings : PropertyKey<BorderSettings> = PropertyKey "border.settings"

    // Button
    type buttonPKeys() =
        inherit viewPKeys()
        // Properties
        member inline this.hotKeySpecifier : PropertyKey<Rune> = PropertyKey "button.hotKeySpecifier"
        member inline this.isDefault : PropertyKey<bool> = PropertyKey "button.isDefault"
        member inline this.noDecorations : PropertyKey<bool> = PropertyKey "button.noDecorations"
        member inline this.noPadding : PropertyKey<bool> = PropertyKey "button.noPadding"
        member inline this.text : PropertyKey<string> = PropertyKey "button.text"
        member inline this.wantContinuousButtonPressed : PropertyKey<bool> = PropertyKey "button.wantContinuousButtonPressed"

    // CheckBox
    type checkBoxPKeys() =
        inherit viewPKeys()
        // Properties
        member inline this.allowCheckStateNone : PropertyKey<bool> = PropertyKey "checkBox.allowCheckStateNone"
        member inline this.checkedState : PropertyKey<CheckState> = PropertyKey "checkBox.checkedState"
        member inline this.hotKeySpecifier : PropertyKey<Rune> = PropertyKey "checkBox.hotKeySpecifier"
        member inline this.radioStyle : PropertyKey<bool> = PropertyKey "checkBox.radioStyle"
        member inline this.text : PropertyKey<string> = PropertyKey "checkBox.text"
        // Events
        member inline this.checkedStateChanging : PropertyKey<ResultEventArgs<CheckState>->unit> = PropertyKey "checkBox.checkedStateChanging"
        member inline this.checkedStateChanged : PropertyKey<EventArgs<CheckState>->unit> = PropertyKey "checkBox.checkedStateChanged"

    // ColorPicker
    type colorPickerPKeys() =
        inherit viewPKeys()
        // Properties
        member inline this.selectedColor : PropertyKey<Color> = PropertyKey "colorPicker.selectedColor"
        member inline this.style : PropertyKey<ColorPickerStyle> = PropertyKey "colorPicker.style"
        // Events
        member inline this.colorChanged : PropertyKey<ResultEventArgs<Color>->unit> = PropertyKey "colorPicker.colorChanged"

    // ColorPicker16
    type colorPicker16PKeys() =
        inherit viewPKeys()
        // Properties
        member inline this.boxHeight : PropertyKey<Int32> = PropertyKey "colorPicker16.boxHeight"
        member inline this.boxWidth : PropertyKey<Int32> = PropertyKey "colorPicker16.boxWidth"
        member inline this.cursor : PropertyKey<Point> = PropertyKey "colorPicker16.cursor"
        member inline this.selectedColor : PropertyKey<ColorName16> = PropertyKey "colorPicker16.selectedColor"
        // Events
        member inline this.colorChanged : PropertyKey<ResultEventArgs<Color>->unit> = PropertyKey "colorPicker16.colorChanged"

    // ComboBox
    type comboBoxPKeys() =
        inherit viewPKeys()
        // Properties
        member inline this.hideDropdownListOnClick : PropertyKey<bool> = PropertyKey "comboBox.hideDropdownListOnClick"
        member inline this.readOnly : PropertyKey<bool> = PropertyKey "comboBox.readOnly"
        member inline this.searchText : PropertyKey<string> = PropertyKey "comboBox.searchText"
        member inline this.selectedItem : PropertyKey<Int32> = PropertyKey "comboBox.selectedItem"
        member inline this.source : PropertyKey<string list> = PropertyKey "comboBox.source"
        member inline this.text : PropertyKey<string> = PropertyKey "comboBox.text"
        // Events
        member inline this.collapsed : PropertyKey<unit->unit> = PropertyKey "comboBox.collapsed"
        member inline this.expanded : PropertyKey<unit->unit> = PropertyKey "comboBox.expanded"
        member inline this.openSelectedItem : PropertyKey<ListViewItemEventArgs->unit> = PropertyKey "comboBox.openSelectedItem"
        member inline this.selectedItemChanged : PropertyKey<ListViewItemEventArgs->unit> = PropertyKey "comboBox.selectedItemChanged"

    // TextField
    type textFieldPKeys() =
        inherit viewPKeys()
        // Properties
        member inline this.autocomplete : PropertyKey<IAutocomplete> = PropertyKey "textField.autocomplete"
        member inline this.caption : PropertyKey<string> = PropertyKey "textField.caption"
        member inline this.captionColor : PropertyKey<Terminal.Gui.Drawing.Color> = PropertyKey "textField.captionColor"
        member inline this.cursorPosition : PropertyKey<Int32> = PropertyKey "textField.cursorPosition"
        member inline this.readOnly : PropertyKey<bool> = PropertyKey "textField.readOnly"
        member inline this.secret : PropertyKey<bool> = PropertyKey "textField.secret"
        member inline this.selectedStart : PropertyKey<Int32> = PropertyKey "textField.selectedStart"
        member inline this.selectWordOnlyOnDoubleClick : PropertyKey<bool> = PropertyKey "textField.selectWordOnlyOnDoubleClick"
        member inline this.text : PropertyKey<string> = PropertyKey "textField.text"
        member inline this.used : PropertyKey<bool> = PropertyKey "textField.used"
        member inline this.useSameRuneTypeForWords : PropertyKey<bool> = PropertyKey "textField.useSameRuneTypeForWords"
        // Events
        member inline this.textChanging : PropertyKey<ResultEventArgs<string>->unit> = PropertyKey "textField.textChanging"

    // DateField
    type dateFieldPKeys() =
        inherit textFieldPKeys()
        // Properties
        member inline this.culture : PropertyKey<CultureInfo> = PropertyKey "dateField.culture"
        member inline this.cursorPosition : PropertyKey<Int32> = PropertyKey "dateField.cursorPosition"
        member inline this.date : PropertyKey<DateTime> = PropertyKey "dateField.date"
        // Events
        member inline this.dateChanged : PropertyKey<DateTimeEventArgs<DateTime>->unit> = PropertyKey "dateField.dateChanged"

    // DatePicker
    type datePickerPKeys() =
        inherit viewPKeys()
        // Properties
        member inline this.culture : PropertyKey<CultureInfo> = PropertyKey "datePicker.culture"
        member inline this.date : PropertyKey<DateTime> = PropertyKey "datePicker.date"

    // Toplevel
    type toplevelPKeys() =
        inherit viewPKeys()
        // Properties
        member inline this.modal : PropertyKey<bool> = PropertyKey "toplevel.modal"
        member inline this.running : PropertyKey<bool> = PropertyKey "toplevel.running"
        // Events
        member inline this.activate : PropertyKey<ToplevelEventArgs->unit> = PropertyKey "toplevel.activate"
        member inline this.closed : PropertyKey<ToplevelEventArgs->unit> = PropertyKey "toplevel.closed"
        member inline this.closing : PropertyKey<ToplevelClosingEventArgs->unit> = PropertyKey "toplevel.closing"
        member inline this.deactivate : PropertyKey<ToplevelEventArgs->unit> = PropertyKey "toplevel.deactivate"
        member inline this.loaded : PropertyKey<unit->unit> = PropertyKey "toplevel.loaded"
        member inline this.ready : PropertyKey<unit->unit> = PropertyKey "toplevel.ready"
        member inline this.sizeChanging : PropertyKey<SizeChangedEventArgs->unit> = PropertyKey "toplevel.sizeChanging"
        member inline this.unloaded : PropertyKey<unit->unit> = PropertyKey "toplevel.unloaded"

    // Dialog
    type dialogPKeys() =
        inherit toplevelPKeys()
        // Properties
        member inline this.buttonAlignment : PropertyKey<Alignment> = PropertyKey "dialog.buttonAlignment"
        member inline this.buttonAlignmentModes : PropertyKey<AlignmentModes> = PropertyKey "dialog.buttonAlignmentModes"
        member inline this.canceled : PropertyKey<bool> = PropertyKey "dialog.canceled"

    // FileDialog
    type fileDialogPKeys() =
        inherit dialogPKeys()
        // Properties
        member inline this.allowedTypes : PropertyKey<IAllowedType list> = PropertyKey "fileDialog.allowedTypes"
        member inline this.allowsMultipleSelection : PropertyKey<bool> = PropertyKey "fileDialog.allowsMultipleSelection"
        member inline this.fileOperationsHandler : PropertyKey<IFileOperations> = PropertyKey "fileDialog.fileOperationsHandler"
        member inline this.mustExist : PropertyKey<bool> = PropertyKey "fileDialog.mustExist"
        member inline this.openMode : PropertyKey<OpenMode> = PropertyKey "fileDialog.openMode"
        member inline this.path : PropertyKey<string> = PropertyKey "fileDialog.path"
        member inline this.searchMatcher : PropertyKey<ISearchMatcher> = PropertyKey "fileDialog.searchMatcher"
        // Events
        member inline this.filesSelected : PropertyKey<FilesSelectedEventArgs->unit> = PropertyKey "fileDialog.filesSelected"

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
        member inline this.axisX : PropertyKey<HorizontalAxis> = PropertyKey "graphView.axisX"
        member inline this.axisY : PropertyKey<VerticalAxis> = PropertyKey "graphView.axisY"
        member inline this.cellSize : PropertyKey<PointF> = PropertyKey "graphView.cellSize"
        member inline this.graphColor : PropertyKey<Attribute option> = PropertyKey "graphView.graphColor"
        member inline this.marginBottom : PropertyKey<int> = PropertyKey "graphView.marginBottom"
        member inline this.marginLeft : PropertyKey<int> = PropertyKey "graphView.marginLeft"
        member inline this.scrollOffset : PropertyKey<PointF> = PropertyKey "graphView.scrollOffset"

    // HexView
    type hexViewPKeys() =
        inherit viewPKeys()

        // Properties
        member inline this.address : PropertyKey<Int64> = PropertyKey "hexView.address"
        member inline this.addressWidth : PropertyKey<int> = PropertyKey "hexView.addressWidth"
        member inline this.allowEdits : PropertyKey<int> = PropertyKey "hexView.allowEdits"
        member inline this.readOnly : PropertyKey<bool> = PropertyKey "hexView.readOnly"
        member inline this.source : PropertyKey<Stream> = PropertyKey "hexView.source"
        // Events
        member inline this.edited : PropertyKey<HexViewEditEventArgs->unit> = PropertyKey "hexView.edited"
        member inline this.positionChanged : PropertyKey<HexViewEventArgs->unit> = PropertyKey "hexView.positionChanged"

    // Label
    type labelPKeys() =
        inherit viewPKeys()

        // Properties
        member inline this.hotKeySpecifier : PropertyKey<Rune> = PropertyKey "label.hotKeySpecifier"
        member inline this.text : PropertyKey<string> = PropertyKey "label.text"

    // LegendAnnotation
    type legendAnnotationPKeys() =
        inherit viewPKeys()
    // No properties or events LegendAnnotation

    // Line
    type linePKeys() =
        inherit viewPKeys()

        // Properties
        member inline this.orientation : PropertyKey<Orientation> = PropertyKey "line.orientation"
        // Events
        member inline this.orientationChanged : PropertyKey<Orientation->unit> = PropertyKey "line.orientationChanged"
        member inline this.orientationChanging : PropertyKey<App.CancelEventArgs<Orientation>->unit> = PropertyKey "line.orientationChanging"

    // LineView
    type lineViewPKeys() =
        inherit viewPKeys()

        // Properties
        member inline this.endingAnchor : PropertyKey<Rune option> = PropertyKey "lineView.endingAnchor"
        member inline this.lineRune : PropertyKey<Rune> = PropertyKey "lineView.lineRune"
        member inline this.orientation : PropertyKey<Orientation> = PropertyKey "lineView.orientation"
        member inline this.startingAnchor : PropertyKey<Rune option> = PropertyKey "lineView.startingAnchor"

    // ListView
    type listViewPKeys() =
        inherit viewPKeys()

        // Properties
        member inline this.allowsMarking : PropertyKey<bool> = PropertyKey "listView.allowsMarking"
        member inline this.allowsMultipleSelection : PropertyKey<bool> = PropertyKey "listView.allowsMultipleSelection"
        member inline this.leftItem : PropertyKey<Int32> = PropertyKey "listView.leftItem"
        member inline this.selectedItem : PropertyKey<Int32> = PropertyKey "listView.selectedItem"
        member inline this.source : PropertyKey<string list> = PropertyKey "listView.source"
        member inline this.topItem : PropertyKey<Int32> = PropertyKey "listView.topItem"
        // Events
        member inline this.collectionChanged : PropertyKey<NotifyCollectionChangedEventArgs->unit> = PropertyKey "listView.collectionChanged"
        member inline this.openSelectedItem : PropertyKey<ListViewItemEventArgs->unit> = PropertyKey "listView.openSelectedItem"
        member inline this.rowRender : PropertyKey<ListViewRowEventArgs->unit> = PropertyKey "listView.rowRender"
        member inline this.selectedItemChanged : PropertyKey<ListViewItemEventArgs->unit> = PropertyKey "listView.selectedItemChanged"

    // Margin
    type marginPKeys() =
        inherit adornmentPKeys()

        // Properties
        member inline this.shadowStyle : PropertyKey<ShadowStyle> = PropertyKey "margin.shadowStyle"

    type menuv2PKeys() =
        inherit barPKeys()

        // Properties
        member inline this.selectedMenuItem : PropertyKey<MenuItemv2> = PropertyKey "menuv2.selectedMenuItem"
        member inline this.superMenuItem : PropertyKey<MenuItemv2> = PropertyKey "menuv2.superMenuItem"
        // Events
        member inline this.accepted : PropertyKey< CommandEventArgs->unit> = PropertyKey "menuv2.accepted"
        member inline this.selectedMenuItemChanged : PropertyKey< MenuItemv2->unit> = PropertyKey "menuv2.selectedMenuItemChanged"

    // MenuBarV2
    type menuBarv2PKeys() =
        inherit menuv2PKeys()

        // Properties
        member inline this.key : PropertyKey<Key> = PropertyKey "menuBarv2.key"
        member inline this.menus : PropertyKey<MenuBarItemv2 array> = PropertyKey "view.menus"
        // Events
        member inline this.keyChanged : PropertyKey<KeyChangedEventArgs->unit> = PropertyKey "menuBarv2.keyChanged"

    type shortcutPKeys() =
         inherit viewPKeys()

         // Properties
         member inline this.action : PropertyKey<Action> = PropertyKey "shortcut.action"
         member inline this.alignmentModes : PropertyKey<AlignmentModes> = PropertyKey "shortcut.alignmentModes"
         member inline this.commandView : PropertyKey<View> = PropertyKey "shortcut.commandView_view"
         member inline this.commandView_element : SingleElementKey<ITerminalElement> = SingleElementKey.create "shortcut.commandView_element"
         member inline this.forceFocusColors : PropertyKey<bool> = PropertyKey "shortcut.forceFocusColors"
         member inline this.helpText : PropertyKey<string> = PropertyKey "shortcut.helpText"
         member inline this.text : PropertyKey<string> = PropertyKey "shortcut.text"
         member inline this.bindKeyToApplication : PropertyKey<bool> = PropertyKey "shortcut.bindKeyToApplication"
         member inline this.key : PropertyKey<Key> = PropertyKey "shortcut.key"
         member inline this.minimumKeyTextSize : PropertyKey<Int32> = PropertyKey "shortcut.minimumKeyTextSize"
         // Events
         member inline this.orientationChanged : PropertyKey<Orientation->unit> = PropertyKey "shortcut.orientationChanged"
         member inline this.orientationChanging : PropertyKey<CancelEventArgs<Orientation>->unit> = PropertyKey "shortcut.orientationChanging"

    type menuItemv2PKeys() =
        inherit shortcutPKeys()
        member inline this.command : PropertyKey< Command> = PropertyKey "menuItemv2.command"
        member inline this.subMenu: PropertyKey<Menuv2> = PropertyKey "menuItemv2.subMenu_view"
        member inline this.subMenu_element : SingleElementKey<IMenuv2Element> = SingleElementKey.create "menuItemv2.subMenu_element"
        member inline this.targetView : PropertyKey<View> = PropertyKey "menuItemv2.targetView"
        member inline this.accepted: PropertyKey<CommandEventArgs -> unit> = PropertyKey "menuItemv2.accepted"

    type menuBarItemv2PKeys() =
        inherit menuItemv2PKeys()

        // Properties
        member inline this.popoverMenu : PropertyKey<PopoverMenu> = PropertyKey "menuBarItemv2.popoverMenu_view"
        member inline this.popoverMenu_element : SingleElementKey<IPopoverMenuElement> = SingleElementKey.create "menuBarItemv2.popoverMenu_element"
        member inline this.popoverMenuOpen : PropertyKey<bool> = PropertyKey "menuBarItemv2.popoverMenuOpen"
        // Events
        member inline this.popoverMenuOpenChanged : PropertyKey<bool->unit> = PropertyKey "menuBarItemv2.popoverMenuOpenChanged"

    type popoverMenuPKeys() =
        inherit viewPKeys()

        // Properties
        member inline this.key : PropertyKey<Key> = PropertyKey "popoverMenu.key"
        member inline this.mouseFlags : PropertyKey<MouseFlags> = PropertyKey "popoverMenu.mouseFlags"
        member inline this.root : PropertyKey<Menuv2> = PropertyKey "popoverMenu.root_view"
        member inline this.root_element : SingleElementKey<IMenuv2Element> = SingleElementKey.create "popoverMenu.root_element"
        // Events
        member inline this.accepted : PropertyKey<CommandEventArgs->unit> = PropertyKey "popoverMenu.accepted"
        member inline this.keyChanged : PropertyKey<KeyChangedEventArgs->unit> = PropertyKey "popoverMenu.keyChanged"

    // NumericUpDown`1
    type numericUpDownPKeys<'a>() =
        inherit viewPKeys()

        // Properties
        member inline this.format : PropertyKey<string> = PropertyKey "numericUpDown.format"
        member inline this.increment : PropertyKey<'a> = PropertyKey "numericUpDown.increment"
        member inline this.value : PropertyKey<'a> = PropertyKey "numericUpDown.value"
        // Events
        member inline this.formatChanged : PropertyKey<string->unit> = PropertyKey "numericUpDown.formatChanged"
        member inline this.incrementChanged : PropertyKey<'a->unit> = PropertyKey "numericUpDown.incrementChanged"
        member inline this.valueChanged : PropertyKey<'a->unit> = PropertyKey "numericUpDown.valueChanged"
        member inline this.valueChanging : PropertyKey<App.CancelEventArgs<'a>->unit> = PropertyKey "numericUpDown.valueChanging"

    // NumericUpDown
    // type numericUpDownPKeys() =
    //     inherit numericUpDownPKeys<int>()
    // No properties or events NumericUpDown

    // OpenDialog
    type openDialogPKeys() =
        inherit fileDialogPKeys()
        // Properties
        member inline this.openMode : PropertyKey<OpenMode> = PropertyKey "openDialog.openMode"

    // OptionSelector
    type optionSelectorPKeys() =
        inherit viewPKeys()
        //Properties
        member inline this.assignHotKeysToCheckBoxes : PropertyKey<bool> = PropertyKey "optionSelector.assignHotKeysToCheckBoxes"
        member inline this.orientation : PropertyKey<Orientation> = PropertyKey "optionSelector.orientation"
        member inline this.options : PropertyKey<IReadOnlyList<string>> = PropertyKey "optionSelector.options"
        member inline this.selectedItem : PropertyKey<Int32> = PropertyKey "optionSelector.selectedItem"
        // Events
        member inline this.orientationChanged : PropertyKey<Orientation->unit> = PropertyKey "optionSelector.orientationChanged"
        member inline this.orientationChanging : PropertyKey<CancelEventArgs<Orientation>->unit> = PropertyKey "optionSelector.orientationChanging"
        member inline this.selectedItemChanged : PropertyKey<SelectedItemChangedArgs->unit> = PropertyKey "optionSelector.selectedItemChanged"

    // Padding
    type paddingPKeys() =
        inherit adornmentPKeys()

    // ProgressBar
    type progressBarPKeys() =
        inherit viewPKeys()

        // Properties
        member inline this.bidirectionalMarquee : PropertyKey<bool> = PropertyKey "progressBar.bidirectionalMarquee"
        member inline this.fraction : PropertyKey<Single> = PropertyKey "progressBar.fraction"
        member inline this.progressBarFormat : PropertyKey<ProgressBarFormat> = PropertyKey "progressBar.progressBarFormat"
        member inline this.progressBarStyle : PropertyKey<ProgressBarStyle> = PropertyKey "progressBar.progressBarStyle"
        member inline this.segmentCharacter : PropertyKey<Rune> = PropertyKey "progressBar.segmentCharacter"
        member inline this.text : PropertyKey<string> = PropertyKey "progressBar.text"

    // RadioGroup
    type radioGroupPKeys() =
        inherit viewPKeys()

        // Properties
        member inline this.assignHotKeysToRadioLabels : PropertyKey<bool> = PropertyKey "radioGroup.assignHotKeysToRadioLabels"
        member inline this.cursor : PropertyKey<Int32> = PropertyKey "radioGroup.cursor"
        member inline this.doubleClickAccepts : PropertyKey<bool> = PropertyKey "radioGroup.doubleClickAccepts"
        member inline this.horizontalSpace : PropertyKey<Int32> = PropertyKey "radioGroup.horizontalSpace"
        member inline this.orientation : PropertyKey<Orientation> = PropertyKey "radioGroup.orientation"
        member inline this.radioLabels : PropertyKey<string list> = PropertyKey "radioGroup.radioLabels"
        member inline this.selectedItem : PropertyKey<Int32> = PropertyKey "radioGroup.selectedItem"
        // Events
        member inline this.orientationChanged : PropertyKey<Orientation->unit> = PropertyKey "radioGroup.orientationChanged"
        member inline this.orientationChanging : PropertyKey<App.CancelEventArgs<Orientation>->unit> = PropertyKey "radioGroup.orientationChanging"
        member inline this.selectedItemChanged : PropertyKey<SelectedItemChangedArgs->unit> = PropertyKey "radioGroup.selectedItemChanged"

    // SaveDialog
    // No properties or events SaveDialog

    // ScrollBar
    type scrollBarPKeys() =
        inherit viewPKeys()

        // Properties
        member inline this.autoShow : PropertyKey<bool> = PropertyKey "scrollBar.autoShow"
        member inline this.increment : PropertyKey<Int32> = PropertyKey "scrollBar.increment"
        member inline this.orientation : PropertyKey<Orientation> = PropertyKey "scrollBar.orientation"
        member inline this.position : PropertyKey<Int32> = PropertyKey "scrollBar.position"
        member inline this.scrollableContentSize : PropertyKey<Int32> = PropertyKey "scrollBar.scrollableContentSize"
        member inline this.visibleContentSize : PropertyKey<Int32> = PropertyKey "scrollBar.visibleContentSize"
        // Events
        member inline this.orientationChanged : PropertyKey<Orientation->unit> = PropertyKey "scrollBar.orientationChanged"
        member inline this.orientationChanging : PropertyKey<CancelEventArgs<Orientation>->unit> = PropertyKey "scrollBar.orientationChanging"
        member inline this.scrollableContentSizeChanged : PropertyKey<EventArgs<Int32>->unit> = PropertyKey "scrollBar.scrollableContentSizeChanged"
        member inline this.sliderPositionChanged : PropertyKey<EventArgs<Int32>->unit> = PropertyKey "scrollBar.sliderPositionChanged"

    // ScrollSlider
    type scrollSliderPKeys() =
        inherit viewPKeys()

        // Properties
        member inline this.orientation : PropertyKey<Orientation> = PropertyKey "scrollSlider.orientation"
        member inline this.position : PropertyKey<Int32> = PropertyKey "scrollSlider.position"
        member inline this.size : PropertyKey<Int32> = PropertyKey "scrollSlider.size"
        member inline this.sliderPadding : PropertyKey<Int32> = PropertyKey "scrollSlider.sliderPadding"
        member inline this.visibleContentSize : PropertyKey<Int32> = PropertyKey "scrollSlider.visibleContentSize"
        // Events
        member inline this.orientationChanged : PropertyKey<Orientation->unit> = PropertyKey "scrollSlider.orientationChanged"
        member inline this.orientationChanging : PropertyKey<CancelEventArgs<Orientation>->unit> = PropertyKey "scrollSlider.orientationChanging"
        member inline this.positionChanged : PropertyKey<EventArgs<Int32>->unit> = PropertyKey "scrollSlider.positionChanged"
        member inline this.positionChanging : PropertyKey<CancelEventArgs<Int32>->unit> = PropertyKey "scrollSlider.positionChanging"
        member inline this.scrolled : PropertyKey<EventArgs<Int32>->unit> = PropertyKey "scrollSlider.scrolled"

    // Slider`1
    type sliderPKeys<'a>() =
        inherit viewPKeys()

        // Properties
        member inline this.allowEmpty : PropertyKey<bool> = PropertyKey "slider.allowEmpty"
        member inline this.focusedOption : PropertyKey<Int32> = PropertyKey "slider.focusedOption"
        member inline this.legendsOrientation : PropertyKey<Orientation> = PropertyKey "slider.legendsOrientation"
        member inline this.minimumInnerSpacing : PropertyKey<Int32> = PropertyKey "slider.minimumInnerSpacing"
        member inline this.options : PropertyKey<SliderOption<'a> list> = PropertyKey "slider.options"
        member inline this.orientation : PropertyKey<Orientation> = PropertyKey "slider.orientation"
        member inline this.rangeAllowSingle : PropertyKey<bool> = PropertyKey "slider.rangeAllowSingle"
        member inline this.showEndSpacing : PropertyKey<bool> = PropertyKey "slider.showEndSpacing"
        member inline this.showLegends : PropertyKey<bool> = PropertyKey "slider.showLegends"
        member inline this.style : PropertyKey<SliderStyle> = PropertyKey "slider.style"
        member inline this.text : PropertyKey<string> = PropertyKey "slider.text"
        member inline this.``type`` : PropertyKey<SliderType> = PropertyKey "slider.``type``"
        member inline this.useMinimumSize : PropertyKey<bool> = PropertyKey "slider.useMinimumSize"
        // Events
        member inline this.optionFocused : PropertyKey<SliderEventArgs<'a>->unit> = PropertyKey "slider.optionFocused"
        member inline this.optionsChanged : PropertyKey<SliderEventArgs<'a>->unit> = PropertyKey "slider.optionsChanged"
        member inline this.orientationChanged : PropertyKey<Orientation->unit> = PropertyKey "slider.orientationChanged"
        member inline this.orientationChanging : PropertyKey<App.CancelEventArgs<Orientation>->unit> = PropertyKey "slider.orientationChanging"

    // Slider
    // type sliderPKeys() =
    //     inherit sliderPKeys<obj>()
    // No properties or events Slider

    // SpinnerView
    type spinnerViewPKeys() =
        inherit viewPKeys()

        // Properties
        member inline this.autoSpin : PropertyKey<bool> = PropertyKey "spinnerView.autoSpin"
        member inline this.sequence : PropertyKey<string list> = PropertyKey "spinnerView.sequence"
        member inline this.spinBounce : PropertyKey<bool> = PropertyKey "spinnerView.spinBounce"
        member inline this.spinDelay : PropertyKey<Int32> = PropertyKey "spinnerView.spinDelay"
        member inline this.spinReverse : PropertyKey<bool> = PropertyKey "spinnerView.spinReverse"
        member inline this.style : PropertyKey<SpinnerStyle> = PropertyKey "spinnerView.style"

    // StatusBar
    type statusBarPKeys() =
        inherit barPKeys()
    // No properties or events StatusBar

    // Tab
    type tabPKeys() =
        inherit viewPKeys()

        // Properties
        member inline this.displayText : PropertyKey<string> = PropertyKey "tab.displayText"
        member inline this.view : PropertyKey<View> = PropertyKey "tab.view_view"
        member inline this.view_element : SingleElementKey<ITerminalElement> = SingleElementKey.create "tab.view_element"

    // TabView
    type tabViewPKeys() =
        inherit viewPKeys()

        // Properties
        member inline this.maxTabTextWidth : PropertyKey<int> = PropertyKey "tabView.maxTabTextWidth"
        member inline this.selectedTab : PropertyKey<Tab> = PropertyKey "tabView.selectedTab"
        member inline this.style : PropertyKey<TabStyle> = PropertyKey "tabView.style"
        member inline this.tabScrollOffset : PropertyKey<Int32> = PropertyKey "tabView.tabScrollOffset"
        // Events
        member inline this.selectedTabChanged : PropertyKey<TabChangedEventArgs->unit> = PropertyKey "tabView.selectedTabChanged"
        member inline this.tabClicked : PropertyKey<TabMouseEventArgs->unit> = PropertyKey "tabView.tabClicked"
        // Additional properties
        member inline this.tabs : PropertyKey<List<ITerminalElement>> = PropertyKey "tabView.tabs_view"
        member inline this.tabs_elements : MultiElementKey<List<ITerminalElement>> = MultiElementKey.create "tabView.tabs_elements"

    // TableView
    type tableViewPKeys() =
        inherit viewPKeys()

        // Properties
        member inline this.cellActivationKey : PropertyKey<KeyCode> = PropertyKey "tableView.cellActivationKey"
        member inline this.collectionNavigator : PropertyKey<ICollectionNavigator> = PropertyKey "tableView.collectionNavigator"
        member inline this.columnOffset : PropertyKey<Int32> = PropertyKey "tableView.columnOffset"
        member inline this.fullRowSelect : PropertyKey<bool> = PropertyKey "tableView.fullRowSelect"
        member inline this.maxCellWidth : PropertyKey<Int32> = PropertyKey "tableView.maxCellWidth"
        member inline this.minCellWidth : PropertyKey<Int32> = PropertyKey "tableView.minCellWidth"
        member inline this.multiSelect : PropertyKey<bool> = PropertyKey "tableView.multiSelect"
        member inline this.nullSymbol : PropertyKey<string> = PropertyKey "tableView.nullSymbol"
        member inline this.rowOffset : PropertyKey<Int32> = PropertyKey "tableView.rowOffset"
        member inline this.selectedColumn : PropertyKey<Int32> = PropertyKey "tableView.selectedColumn"
        member inline this.selectedRow : PropertyKey<Int32> = PropertyKey "tableView.selectedRow"
        member inline this.separatorSymbol : PropertyKey<Char> = PropertyKey "tableView.separatorSymbol"
        member inline this.style : PropertyKey<TableStyle> = PropertyKey "tableView.style"
        member inline this.table : PropertyKey<ITableSource> = PropertyKey "tableView.table"
        // Events
        member inline this.cellActivated : PropertyKey<CellActivatedEventArgs->unit> = PropertyKey "tableView.cellActivated"
        member inline this.cellToggled : PropertyKey<CellToggledEventArgs->unit> = PropertyKey "tableView.cellToggled"
        member inline this.selectedCellChanged : PropertyKey<SelectedCellChangedEventArgs->unit> = PropertyKey "tableView.selectedCellChanged"

    // TextValidateField
    type textValidateFieldPKeys() =
        inherit viewPKeys()

        // Properties
        member inline this.provider : PropertyKey<ITextValidateProvider> = PropertyKey "textValidateField.provider"
        member inline this.text : PropertyKey<string> = PropertyKey "textValidateField.text"

    // TextView
    type textViewPKeys() =
        inherit viewPKeys()

        // Properties
        member inline this.allowsReturn : PropertyKey<bool> = PropertyKey "textView.allowsReturn"
        member inline this.allowsTab : PropertyKey<bool> = PropertyKey "textView.allowsTab"
        member inline this.cursorPosition : PropertyKey<Point> = PropertyKey "textView.cursorPosition"
        member inline this.inheritsPreviousAttribute : PropertyKey<bool> = PropertyKey "textView.inheritsPreviousAttribute"
        member inline this.isDirty : PropertyKey<bool> = PropertyKey "textView.isDirty"
        member inline this.isSelecting : PropertyKey<bool> = PropertyKey "textView.isSelecting"
        member inline this.leftColumn : PropertyKey<Int32> = PropertyKey "textView.leftColumn"
        member inline this.multiline : PropertyKey<bool> = PropertyKey "textView.multiline"
        member inline this.readOnly : PropertyKey<bool> = PropertyKey "textView.readOnly"
        member inline this.selectionStartColumn : PropertyKey<Int32> = PropertyKey "textView.selectionStartColumn"
        member inline this.selectionStartRow : PropertyKey<Int32> = PropertyKey "textView.selectionStartRow"
        member inline this.selectWordOnlyOnDoubleClick : PropertyKey<bool> = PropertyKey "textView.selectWordOnlyOnDoubleClick"
        member inline this.tabWidth : PropertyKey<Int32> = PropertyKey "textView.tabWidth"
        member inline this.text : PropertyKey<string> = PropertyKey "textView.text"
        member inline this.topRow : PropertyKey<Int32> = PropertyKey "textView.topRow"
        member inline this.used : PropertyKey<bool> = PropertyKey "textView.used"
        member inline this.useSameRuneTypeForWords : PropertyKey<bool> = PropertyKey "textView.useSameRuneTypeForWords"
        member inline this.wordWrap : PropertyKey<bool> = PropertyKey "textView.wordWrap"
        // Events
        member inline this.contentsChanged : PropertyKey<ContentsChangedEventArgs->unit> = PropertyKey "textView.contentsChanged"
        member inline this.drawNormalColor : PropertyKey<CellEventArgs->unit> = PropertyKey "textView.drawNormalColor"
        member inline this.drawReadOnlyColor : PropertyKey<CellEventArgs->unit> = PropertyKey "textView.drawReadOnlyColor"
        member inline this.drawSelectionColor : PropertyKey<CellEventArgs->unit> = PropertyKey "textView.drawSelectionColor"
        member inline this.drawUsedColor : PropertyKey<CellEventArgs->unit> = PropertyKey "textView.drawUsedColor"
        member inline this.unwrappedCursorPosition : PropertyKey<Point->unit> = PropertyKey "textView.unwrappedCursorPosition"
        // Additional properties
        member inline this.textChanged : PropertyKey<string->unit> = PropertyKey "textView.textChanged"

    // TileView
    type tileViewPKeys() =
        inherit viewPKeys()

        // Properties
        member inline this.lineStyle : PropertyKey<LineStyle> = PropertyKey "tileView.lineStyle"
        member inline this.orientation : PropertyKey<Orientation> = PropertyKey "tileView.orientation"
        member inline this.toggleResizable : PropertyKey<KeyCode> = PropertyKey "tileView.toggleResizable"
        // Events
        member inline this.splitterMoved : PropertyKey<SplitterEventArgs->unit> = PropertyKey "tileView.splitterMoved"

    // TimeField
    type timeFieldPKeys() =
        inherit textFieldPKeys()

        // Properties
        member inline this.cursorPosition : PropertyKey<Int32> = PropertyKey "timeField.cursorPosition"
        member inline this.isShortFormat : PropertyKey<bool> = PropertyKey "timeField.isShortFormat"
        member inline this.time : PropertyKey<TimeSpan> = PropertyKey "timeField.time"
        // Events
        member inline this.timeChanged : PropertyKey<DateTimeEventArgs<TimeSpan>->unit> = PropertyKey "timeField.timeChanged"

    // TreeView`1
    type treeViewPKeys<'a when 'a : not struct>() =
        inherit viewPKeys()

        // Properties
        member inline this.allowLetterBasedNavigation : PropertyKey<bool> = PropertyKey "treeView.allowLetterBasedNavigation"
        member inline this.aspectGetter : PropertyKey<AspectGetterDelegate<'a>> = PropertyKey "treeView.aspectGetter"
        member inline this.colorGetter : PropertyKey<Func<'a,Scheme>> = PropertyKey "treeView.colorGetter"
        member inline this.maxDepth : PropertyKey<Int32> = PropertyKey "treeView.maxDepth"
        member inline this.multiSelect : PropertyKey<bool> = PropertyKey "treeView.multiSelect"
        member inline this.objectActivationButton : PropertyKey<MouseFlags option> = PropertyKey "treeView.objectActivationButton"
        member inline this.objectActivationKey : PropertyKey<KeyCode> = PropertyKey "treeView.objectActivationKey"
        member inline this.scrollOffsetHorizontal : PropertyKey<Int32> = PropertyKey "treeView.scrollOffsetHorizontal"
        member inline this.scrollOffsetVertical : PropertyKey<Int32> = PropertyKey "treeView.scrollOffsetVertical"
        member inline this.selectedObject : PropertyKey<'a> = PropertyKey "treeView.selectedObject"
        member inline this.style : PropertyKey<TreeStyle> = PropertyKey "treeView.style"
        member inline this.treeBuilder : PropertyKey<ITreeBuilder<'a>> = PropertyKey "treeView.treeBuilder"
        // Events
        member inline this.drawLine : PropertyKey<DrawTreeViewLineEventArgs<'a>->unit> = PropertyKey "treeView.drawLine"
        member inline this.objectActivated : PropertyKey<ObjectActivatedEventArgs<'a>->unit> = PropertyKey "treeView.objectActivated"
        member inline this.selectionChanged : PropertyKey<SelectionChangedEventArgs<'a>->unit> = PropertyKey "treeView.selectionChanged"

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
        member inline this.currentStep : PropertyKey<WizardStep> = PropertyKey "wizard.currentStep"
        member inline this.modal : PropertyKey<bool> = PropertyKey "wizard.modal"
        // Events
        member inline this.cancelled : PropertyKey<WizardButtonEventArgs->unit> = PropertyKey "wizard.cancelled"
        member inline this.finished : PropertyKey<WizardButtonEventArgs->unit> = PropertyKey "wizard.finished"
        member inline this.movingBack : PropertyKey<WizardButtonEventArgs->unit> = PropertyKey "wizard.movingBack"
        member inline this.movingNext : PropertyKey<WizardButtonEventArgs->unit> = PropertyKey "wizard.movingNext"
        member inline this.stepChanged : PropertyKey<StepChangeEventArgs->unit> = PropertyKey "wizard.stepChanged"
        member inline this.stepChanging : PropertyKey<StepChangeEventArgs->unit> = PropertyKey "wizard.stepChanging"

    // WizardStep
    type wizardStepPKeys() =
        inherit viewPKeys()

        // Properties
        member inline this.backButtonText : PropertyKey<string> = PropertyKey "wizardStep.backButtonText"
        member inline this.helpText : PropertyKey<string> = PropertyKey "wizardStep.helpText"
        member inline this.nextButtonText : PropertyKey<string> = PropertyKey "wizardStep.nextButtonText"


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
