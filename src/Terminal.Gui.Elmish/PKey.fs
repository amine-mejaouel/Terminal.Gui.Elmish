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
        member inline this.children : SimplePropertyKey<System.Collections.Generic.List<ITerminalElement>> = SimplePropertyKey "children"

        // Properties
        member inline this.arrangement : SimplePropertyKey<ViewArrangement> = SimplePropertyKey "view.arrangement"
        member inline this.borderStyle : SimplePropertyKey<LineStyle> = SimplePropertyKey "view.borderStyle"
        member inline this.canFocus : SimplePropertyKey<bool> = SimplePropertyKey "view.canFocus"
        member inline this.contentSizeTracksViewport : SimplePropertyKey<bool> = SimplePropertyKey "view.contentSizeTracksViewport"
        member inline this.cursorVisibility : SimplePropertyKey<CursorVisibility> = SimplePropertyKey "view.cursorVisibility"
        member inline this.data : SimplePropertyKey<Object> = SimplePropertyKey "view.data"
        member inline this.enabled : SimplePropertyKey<bool> = SimplePropertyKey "view.enabled"
        member inline this.frame : SimplePropertyKey<Rectangle> = SimplePropertyKey "view.frame"
        member inline this.hasFocus : SimplePropertyKey<bool> = SimplePropertyKey "view.hasFocus"
        member inline this.height : SimplePropertyKey<Dim> = SimplePropertyKey "view.height"
        member inline this.highlightStates : SimplePropertyKey<MouseState> = SimplePropertyKey "view.highlightStates"
        member inline this.hotKey : SimplePropertyKey<Key> = SimplePropertyKey "view.hotKey"
        member inline this.hotKeySpecifier : SimplePropertyKey<Rune> = SimplePropertyKey "view.hotKeySpecifier"
        member inline this.id : SimplePropertyKey<string> = SimplePropertyKey "view.id"
        member inline this.isInitialized : SimplePropertyKey<bool> = SimplePropertyKey "view.isInitialized"
        member inline this.mouseHeldDown : SimplePropertyKey<IMouseHeldDown> = SimplePropertyKey "view.mouseHeldDown"
        member inline this.needsDraw : SimplePropertyKey<bool> = SimplePropertyKey "view.needsDraw"
        member inline this.preserveTrailingSpaces : SimplePropertyKey<bool> = SimplePropertyKey "view.preserveTrailingSpaces"
        member inline this.schemeName : SimplePropertyKey<string> = SimplePropertyKey "view.schemeName"
        member inline this.shadowStyle : SimplePropertyKey<ShadowStyle> = SimplePropertyKey "view.shadowStyle"
        member inline this.superViewRendersLineCanvas : SimplePropertyKey<bool> = SimplePropertyKey "view.superViewRendersLineCanvas"
        member inline this.tabStop : SimplePropertyKey<TabBehavior option> = SimplePropertyKey "view.tabStop"
        member inline this.text : SimplePropertyKey<string> = SimplePropertyKey "view.text"
        member inline this.textAlignment : SimplePropertyKey<Alignment> = SimplePropertyKey "view.textAlignment"
        member inline this.textDirection : SimplePropertyKey<TextDirection> = SimplePropertyKey "view.textDirection"
        member inline this.title : SimplePropertyKey<string> = SimplePropertyKey "view.title"
        member inline this.validatePosDim : SimplePropertyKey<bool> = SimplePropertyKey "view.validatePosDim"
        member inline this.verticalTextAlignment : SimplePropertyKey<Alignment> = SimplePropertyKey "view.verticalTextAlignment"
        member inline this.viewport : SimplePropertyKey<Rectangle> = SimplePropertyKey "view.viewport"
        member inline this.viewportSettings : SimplePropertyKey<ViewportSettingsFlags> = SimplePropertyKey "view.viewportSettings"
        member inline this.visible : SimplePropertyKey<bool> = SimplePropertyKey "view.visible"
        member inline this.wantContinuousButtonPressed : SimplePropertyKey<bool> = SimplePropertyKey "view.wantContinuousButtonPressed"
        member inline this.wantMousePositionReports : SimplePropertyKey<bool> = SimplePropertyKey "view.wantMousePositionReports"
        member inline this.width : SimplePropertyKey<Dim> = SimplePropertyKey "view.width"
        member inline this.x : SimplePropertyKey<Pos> = SimplePropertyKey "view.x"
        member inline this.y : SimplePropertyKey<Pos> = SimplePropertyKey "view.y"
        // Events
        member inline this.accepting : SimplePropertyKey<HandledEventArgs->unit> = SimplePropertyKey "view.accepting"
        member inline this.advancingFocus : SimplePropertyKey<AdvanceFocusEventArgs->unit> = SimplePropertyKey "view.advancingFocus"
        member inline this.borderStyleChanged : SimplePropertyKey<EventArgs->unit> = SimplePropertyKey "view.borderStyleChanged"
        member inline this.canFocusChanged : SimplePropertyKey<unit->unit> = SimplePropertyKey "view.canFocusChanged"
        member inline this.clearedViewport : SimplePropertyKey<DrawEventArgs->unit> = SimplePropertyKey "view.clearedViewport"
        member inline this.clearingViewport : SimplePropertyKey<DrawEventArgs->unit> = SimplePropertyKey "view.clearingViewport"
        member inline this.commandNotBound : SimplePropertyKey<CommandEventArgs->unit> = SimplePropertyKey "view.commandNotBound"
        member inline this.contentSizeChanged : SimplePropertyKey<SizeChangedEventArgs->unit> = SimplePropertyKey "view.contentSizeChanged"
        member inline this.disposing : SimplePropertyKey<unit->unit> = SimplePropertyKey "view.disposing"
        member inline this.drawComplete : SimplePropertyKey<DrawEventArgs->unit> = SimplePropertyKey "view.drawComplete"
        member inline this.drawingContent : SimplePropertyKey<DrawEventArgs->unit> = SimplePropertyKey "view.drawingContent"
        member inline this.drawingSubViews : SimplePropertyKey<DrawEventArgs->unit> = SimplePropertyKey "view.drawingSubViews"
        member inline this.drawingText : SimplePropertyKey<DrawEventArgs->unit> = SimplePropertyKey "view.drawingText"
        member inline this.enabledChanged : SimplePropertyKey<unit->unit> = SimplePropertyKey "view.enabledChanged"
        member inline this.focusedChanged : SimplePropertyKey<HasFocusEventArgs->unit> = SimplePropertyKey "view.focusedChanged"
        member inline this.frameChanged : SimplePropertyKey<EventArgs<Rectangle>->unit> = SimplePropertyKey "view.frameChanged"
        member inline this.gettingAttributeForRole : SimplePropertyKey<VisualRoleEventArgs->unit> = SimplePropertyKey "view.gettingAttributeForRole"
        member inline this.gettingScheme : SimplePropertyKey<ResultEventArgs<Scheme>->unit> = SimplePropertyKey "view.gettingScheme"
        member inline this.handlingHotKey : SimplePropertyKey<CommandEventArgs->unit> = SimplePropertyKey "view.handlingHotKey"
        member inline this.hasFocusChanged : SimplePropertyKey<HasFocusEventArgs->unit> = SimplePropertyKey "view.hasFocusChanged"
        member inline this.hasFocusChanging : SimplePropertyKey<HasFocusEventArgs->unit> = SimplePropertyKey "view.hasFocusChanging"
        member inline this.hotKeyChanged : SimplePropertyKey<KeyChangedEventArgs->unit> = SimplePropertyKey "view.hotKeyChanged"
        member inline this.initialized : SimplePropertyKey<unit->unit> = SimplePropertyKey "view.initialized"
        member inline this.keyDown : SimplePropertyKey<Key->unit> = SimplePropertyKey "view.keyDown"
        member inline this.keyDownNotHandled : SimplePropertyKey<Key->unit> = SimplePropertyKey "view.keyDownNotHandled"
        member inline this.keyUp : SimplePropertyKey<Key->unit> = SimplePropertyKey "view.keyUp"
        member inline this.mouseClick : SimplePropertyKey<MouseEventArgs->unit> = SimplePropertyKey "view.mouseClick"
        member inline this.mouseEnter : SimplePropertyKey<CancelEventArgs->unit> = SimplePropertyKey "view.mouseEnter"
        member inline this.mouseEvent : SimplePropertyKey<MouseEventArgs->unit> = SimplePropertyKey "view.mouseEvent"
        member inline this.mouseLeave : SimplePropertyKey<EventArgs->unit> = SimplePropertyKey "view.mouseLeave"
        member inline this.mouseStateChanged : SimplePropertyKey<EventArgs->unit> = SimplePropertyKey "view.mouseStateChanged"
        member inline this.mouseWheel : SimplePropertyKey<MouseEventArgs->unit> = SimplePropertyKey "view.mouseWheel"
        member inline this.removed : SimplePropertyKey<SuperViewChangedEventArgs->unit> = SimplePropertyKey "view.removed"
        member inline this.schemeChanged : SimplePropertyKey<ValueChangedEventArgs<Scheme>->unit> = SimplePropertyKey "view.schemeChanged"
        member inline this.schemeChanging : SimplePropertyKey<ValueChangingEventArgs<Scheme>->unit> = SimplePropertyKey "view.schemeChanging"
        member inline this.schemeNameChanged : SimplePropertyKey<ValueChangedEventArgs<string>->unit> = SimplePropertyKey "view.schemeNameChanged"
        member inline this.schemeNameChanging : SimplePropertyKey<ValueChangingEventArgs<string>->unit> = SimplePropertyKey "view.schemeNameChanging"
        member inline this.selecting : SimplePropertyKey<CommandEventArgs->unit> = SimplePropertyKey "view.selecting"
        member inline this.subViewAdded : SimplePropertyKey<SuperViewChangedEventArgs->unit> = SimplePropertyKey "view.subViewAdded"
        member inline this.subViewLayout : SimplePropertyKey<LayoutEventArgs->unit> = SimplePropertyKey "view.subViewLayout"
        member inline this.subViewRemoved : SimplePropertyKey<SuperViewChangedEventArgs->unit> = SimplePropertyKey "view.subViewRemoved"
        member inline this.subViewsLaidOut : SimplePropertyKey<LayoutEventArgs->unit> = SimplePropertyKey "view.subViewsLaidOut"
        member inline this.superViewChanged : SimplePropertyKey<SuperViewChangedEventArgs->unit> = SimplePropertyKey "view.superViewChanged"
        member inline this.textChanged : SimplePropertyKey<unit->unit> = SimplePropertyKey "view.textChanged"
        member inline this.titleChanged : SimplePropertyKey<string->unit> = SimplePropertyKey "view.titleChanged"
        member inline this.titleChanging : SimplePropertyKey<App.CancelEventArgs<string>->unit> = SimplePropertyKey "view.titleChanging"
        member inline this.viewportChanged : SimplePropertyKey<DrawEventArgs->unit> = SimplePropertyKey "view.viewportChanged"
        member inline this.visibleChanged : SimplePropertyKey<unit->unit> = SimplePropertyKey "view.visibleChanged"
        member inline this.visibleChanging : SimplePropertyKey<unit->unit> = SimplePropertyKey "view.visibleChanging"

    // Adornment
    type adornmentPKeys() =
        inherit viewPKeys()
        // Properties
        member inline this.diagnostics : SimplePropertyKey<ViewDiagnosticFlags> = SimplePropertyKey "adornment.diagnostics"
        member inline this.superViewRendersLineCanvas : SimplePropertyKey<bool> = SimplePropertyKey "adornment.superViewRendersLineCanvas"
        member inline this.thickness : SimplePropertyKey<Thickness> = SimplePropertyKey "adornment.thickness"
        member inline this.viewport : SimplePropertyKey<Rectangle> = SimplePropertyKey "adornment.viewport"
        // Events
        member inline this.thicknessChanged : SimplePropertyKey<unit->unit> = SimplePropertyKey "adornment.thicknessChanged"

    // Bar
    type barPKeys() =
        inherit viewPKeys()
        // Properties
        member inline this.alignmentModes : SimplePropertyKey<AlignmentModes> = SimplePropertyKey "bar.alignmentModes"
        member inline this.orientation : SimplePropertyKey<Orientation> = SimplePropertyKey "bar.orientation"
        // Events
        member inline this.orientationChanged : SimplePropertyKey<Orientation->unit> = SimplePropertyKey "bar.orientationChanged"
        member inline this.orientationChanging : SimplePropertyKey<App.CancelEventArgs<Orientation>->unit> = SimplePropertyKey "bar.orientationChanging"

    // Border
    type borderPKeys() =
        inherit adornmentPKeys()
        // Properties
        member inline this.lineStyle : SimplePropertyKey<LineStyle> = SimplePropertyKey "border.lineStyle"
        member inline this.settings : SimplePropertyKey<BorderSettings> = SimplePropertyKey "border.settings"

    // Button
    type buttonPKeys() =
        inherit viewPKeys()
        // Properties
        member inline this.hotKeySpecifier : SimplePropertyKey<Rune> = SimplePropertyKey "button.hotKeySpecifier"
        member inline this.isDefault : SimplePropertyKey<bool> = SimplePropertyKey "button.isDefault"
        member inline this.noDecorations : SimplePropertyKey<bool> = SimplePropertyKey "button.noDecorations"
        member inline this.noPadding : SimplePropertyKey<bool> = SimplePropertyKey "button.noPadding"
        member inline this.text : SimplePropertyKey<string> = SimplePropertyKey "button.text"
        member inline this.wantContinuousButtonPressed : SimplePropertyKey<bool> = SimplePropertyKey "button.wantContinuousButtonPressed"

    // CheckBox
    type checkBoxPKeys() =
        inherit viewPKeys()
        // Properties
        member inline this.allowCheckStateNone : SimplePropertyKey<bool> = SimplePropertyKey "checkBox.allowCheckStateNone"
        member inline this.checkedState : SimplePropertyKey<CheckState> = SimplePropertyKey "checkBox.checkedState"
        member inline this.hotKeySpecifier : SimplePropertyKey<Rune> = SimplePropertyKey "checkBox.hotKeySpecifier"
        member inline this.radioStyle : SimplePropertyKey<bool> = SimplePropertyKey "checkBox.radioStyle"
        member inline this.text : SimplePropertyKey<string> = SimplePropertyKey "checkBox.text"
        // Events
        member inline this.checkedStateChanging : SimplePropertyKey<ResultEventArgs<CheckState>->unit> = SimplePropertyKey "checkBox.checkedStateChanging"
        member inline this.checkedStateChanged : SimplePropertyKey<EventArgs<CheckState>->unit> = SimplePropertyKey "checkBox.checkedStateChanged"

    // ColorPicker
    type colorPickerPKeys() =
        inherit viewPKeys()
        // Properties
        member inline this.selectedColor : SimplePropertyKey<Color> = SimplePropertyKey "colorPicker.selectedColor"
        member inline this.style : SimplePropertyKey<ColorPickerStyle> = SimplePropertyKey "colorPicker.style"
        // Events
        member inline this.colorChanged : SimplePropertyKey<ResultEventArgs<Color>->unit> = SimplePropertyKey "colorPicker.colorChanged"

    // ColorPicker16
    type colorPicker16PKeys() =
        inherit viewPKeys()
        // Properties
        member inline this.boxHeight : SimplePropertyKey<Int32> = SimplePropertyKey "colorPicker16.boxHeight"
        member inline this.boxWidth : SimplePropertyKey<Int32> = SimplePropertyKey "colorPicker16.boxWidth"
        member inline this.cursor : SimplePropertyKey<Point> = SimplePropertyKey "colorPicker16.cursor"
        member inline this.selectedColor : SimplePropertyKey<ColorName16> = SimplePropertyKey "colorPicker16.selectedColor"
        // Events
        member inline this.colorChanged : SimplePropertyKey<ResultEventArgs<Color>->unit> = SimplePropertyKey "colorPicker16.colorChanged"

    // ComboBox
    type comboBoxPKeys() =
        inherit viewPKeys()
        // Properties
        member inline this.hideDropdownListOnClick : SimplePropertyKey<bool> = SimplePropertyKey "comboBox.hideDropdownListOnClick"
        member inline this.readOnly : SimplePropertyKey<bool> = SimplePropertyKey "comboBox.readOnly"
        member inline this.searchText : SimplePropertyKey<string> = SimplePropertyKey "comboBox.searchText"
        member inline this.selectedItem : SimplePropertyKey<Int32> = SimplePropertyKey "comboBox.selectedItem"
        member inline this.source : SimplePropertyKey<string list> = SimplePropertyKey "comboBox.source"
        member inline this.text : SimplePropertyKey<string> = SimplePropertyKey "comboBox.text"
        // Events
        member inline this.collapsed : SimplePropertyKey<unit->unit> = SimplePropertyKey "comboBox.collapsed"
        member inline this.expanded : SimplePropertyKey<unit->unit> = SimplePropertyKey "comboBox.expanded"
        member inline this.openSelectedItem : SimplePropertyKey<ListViewItemEventArgs->unit> = SimplePropertyKey "comboBox.openSelectedItem"
        member inline this.selectedItemChanged : SimplePropertyKey<ListViewItemEventArgs->unit> = SimplePropertyKey "comboBox.selectedItemChanged"

    // TextField
    type textFieldPKeys() =
        inherit viewPKeys()
        // Properties
        member inline this.autocomplete : SimplePropertyKey<IAutocomplete> = SimplePropertyKey "textField.autocomplete"
        member inline this.caption : SimplePropertyKey<string> = SimplePropertyKey "textField.caption"
        member inline this.captionColor : SimplePropertyKey<Terminal.Gui.Drawing.Color> = SimplePropertyKey "textField.captionColor"
        member inline this.cursorPosition : SimplePropertyKey<Int32> = SimplePropertyKey "textField.cursorPosition"
        member inline this.readOnly : SimplePropertyKey<bool> = SimplePropertyKey "textField.readOnly"
        member inline this.secret : SimplePropertyKey<bool> = SimplePropertyKey "textField.secret"
        member inline this.selectedStart : SimplePropertyKey<Int32> = SimplePropertyKey "textField.selectedStart"
        member inline this.selectWordOnlyOnDoubleClick : SimplePropertyKey<bool> = SimplePropertyKey "textField.selectWordOnlyOnDoubleClick"
        member inline this.text : SimplePropertyKey<string> = SimplePropertyKey "textField.text"
        member inline this.used : SimplePropertyKey<bool> = SimplePropertyKey "textField.used"
        member inline this.useSameRuneTypeForWords : SimplePropertyKey<bool> = SimplePropertyKey "textField.useSameRuneTypeForWords"
        // Events
        member inline this.textChanging : SimplePropertyKey<ResultEventArgs<string>->unit> = SimplePropertyKey "textField.textChanging"

    // DateField
    type dateFieldPKeys() =
        inherit textFieldPKeys()
        // Properties
        member inline this.culture : SimplePropertyKey<CultureInfo> = SimplePropertyKey "dateField.culture"
        member inline this.cursorPosition : SimplePropertyKey<Int32> = SimplePropertyKey "dateField.cursorPosition"
        member inline this.date : SimplePropertyKey<DateTime> = SimplePropertyKey "dateField.date"
        // Events
        member inline this.dateChanged : SimplePropertyKey<DateTimeEventArgs<DateTime>->unit> = SimplePropertyKey "dateField.dateChanged"

    // DatePicker
    type datePickerPKeys() =
        inherit viewPKeys()
        // Properties
        member inline this.culture : SimplePropertyKey<CultureInfo> = SimplePropertyKey "datePicker.culture"
        member inline this.date : SimplePropertyKey<DateTime> = SimplePropertyKey "datePicker.date"

    // Toplevel
    type toplevelPKeys() =
        inherit viewPKeys()
        // Properties
        member inline this.modal : SimplePropertyKey<bool> = SimplePropertyKey "toplevel.modal"
        member inline this.running : SimplePropertyKey<bool> = SimplePropertyKey "toplevel.running"
        // Events
        member inline this.activate : SimplePropertyKey<ToplevelEventArgs->unit> = SimplePropertyKey "toplevel.activate"
        member inline this.closed : SimplePropertyKey<ToplevelEventArgs->unit> = SimplePropertyKey "toplevel.closed"
        member inline this.closing : SimplePropertyKey<ToplevelClosingEventArgs->unit> = SimplePropertyKey "toplevel.closing"
        member inline this.deactivate : SimplePropertyKey<ToplevelEventArgs->unit> = SimplePropertyKey "toplevel.deactivate"
        member inline this.loaded : SimplePropertyKey<unit->unit> = SimplePropertyKey "toplevel.loaded"
        member inline this.ready : SimplePropertyKey<unit->unit> = SimplePropertyKey "toplevel.ready"
        member inline this.sizeChanging : SimplePropertyKey<SizeChangedEventArgs->unit> = SimplePropertyKey "toplevel.sizeChanging"
        member inline this.unloaded : SimplePropertyKey<unit->unit> = SimplePropertyKey "toplevel.unloaded"

    // Dialog
    type dialogPKeys() =
        inherit toplevelPKeys()
        // Properties
        member inline this.buttonAlignment : SimplePropertyKey<Alignment> = SimplePropertyKey "dialog.buttonAlignment"
        member inline this.buttonAlignmentModes : SimplePropertyKey<AlignmentModes> = SimplePropertyKey "dialog.buttonAlignmentModes"
        member inline this.canceled : SimplePropertyKey<bool> = SimplePropertyKey "dialog.canceled"

    // FileDialog
    type fileDialogPKeys() =
        inherit dialogPKeys()
        // Properties
        member inline this.allowedTypes : SimplePropertyKey<IAllowedType list> = SimplePropertyKey "fileDialog.allowedTypes"
        member inline this.allowsMultipleSelection : SimplePropertyKey<bool> = SimplePropertyKey "fileDialog.allowsMultipleSelection"
        member inline this.fileOperationsHandler : SimplePropertyKey<IFileOperations> = SimplePropertyKey "fileDialog.fileOperationsHandler"
        member inline this.mustExist : SimplePropertyKey<bool> = SimplePropertyKey "fileDialog.mustExist"
        member inline this.openMode : SimplePropertyKey<OpenMode> = SimplePropertyKey "fileDialog.openMode"
        member inline this.path : SimplePropertyKey<string> = SimplePropertyKey "fileDialog.path"
        member inline this.searchMatcher : SimplePropertyKey<ISearchMatcher> = SimplePropertyKey "fileDialog.searchMatcher"
        // Events
        member inline this.filesSelected : SimplePropertyKey<FilesSelectedEventArgs->unit> = SimplePropertyKey "fileDialog.filesSelected"

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
        member inline this.axisX : SimplePropertyKey<HorizontalAxis> = SimplePropertyKey "graphView.axisX"
        member inline this.axisY : SimplePropertyKey<VerticalAxis> = SimplePropertyKey "graphView.axisY"
        member inline this.cellSize : SimplePropertyKey<PointF> = SimplePropertyKey "graphView.cellSize"
        member inline this.graphColor : SimplePropertyKey<Attribute option> = SimplePropertyKey "graphView.graphColor"
        member inline this.marginBottom : SimplePropertyKey<int> = SimplePropertyKey "graphView.marginBottom"
        member inline this.marginLeft : SimplePropertyKey<int> = SimplePropertyKey "graphView.marginLeft"
        member inline this.scrollOffset : SimplePropertyKey<PointF> = SimplePropertyKey "graphView.scrollOffset"

    // HexView
    type hexViewPKeys() =
        inherit viewPKeys()

        // Properties
        member inline this.address : SimplePropertyKey<Int64> = SimplePropertyKey "hexView.address"
        member inline this.addressWidth : SimplePropertyKey<int> = SimplePropertyKey "hexView.addressWidth"
        member inline this.allowEdits : SimplePropertyKey<int> = SimplePropertyKey "hexView.allowEdits"
        member inline this.readOnly : SimplePropertyKey<bool> = SimplePropertyKey "hexView.readOnly"
        member inline this.source : SimplePropertyKey<Stream> = SimplePropertyKey "hexView.source"
        // Events
        member inline this.edited : SimplePropertyKey<HexViewEditEventArgs->unit> = SimplePropertyKey "hexView.edited"
        member inline this.positionChanged : SimplePropertyKey<HexViewEventArgs->unit> = SimplePropertyKey "hexView.positionChanged"

    // Label
    type labelPKeys() =
        inherit viewPKeys()

        // Properties
        member inline this.hotKeySpecifier : SimplePropertyKey<Rune> = SimplePropertyKey "label.hotKeySpecifier"
        member inline this.text : SimplePropertyKey<string> = SimplePropertyKey "label.text"

    // LegendAnnotation
    type legendAnnotationPKeys() =
        inherit viewPKeys()
    // No properties or events LegendAnnotation

    // Line
    type linePKeys() =
        inherit viewPKeys()

        // Properties
        member inline this.orientation : SimplePropertyKey<Orientation> = SimplePropertyKey "line.orientation"
        // Events
        member inline this.orientationChanged : SimplePropertyKey<Orientation->unit> = SimplePropertyKey "line.orientationChanged"
        member inline this.orientationChanging : SimplePropertyKey<App.CancelEventArgs<Orientation>->unit> = SimplePropertyKey "line.orientationChanging"

    // LineView
    type lineViewPKeys() =
        inherit viewPKeys()

        // Properties
        member inline this.endingAnchor : SimplePropertyKey<Rune option> = SimplePropertyKey "lineView.endingAnchor"
        member inline this.lineRune : SimplePropertyKey<Rune> = SimplePropertyKey "lineView.lineRune"
        member inline this.orientation : SimplePropertyKey<Orientation> = SimplePropertyKey "lineView.orientation"
        member inline this.startingAnchor : SimplePropertyKey<Rune option> = SimplePropertyKey "lineView.startingAnchor"

    // ListView
    type listViewPKeys() =
        inherit viewPKeys()

        // Properties
        member inline this.allowsMarking : SimplePropertyKey<bool> = SimplePropertyKey "listView.allowsMarking"
        member inline this.allowsMultipleSelection : SimplePropertyKey<bool> = SimplePropertyKey "listView.allowsMultipleSelection"
        member inline this.leftItem : SimplePropertyKey<Int32> = SimplePropertyKey "listView.leftItem"
        member inline this.selectedItem : SimplePropertyKey<Int32> = SimplePropertyKey "listView.selectedItem"
        member inline this.source : SimplePropertyKey<string list> = SimplePropertyKey "listView.source"
        member inline this.topItem : SimplePropertyKey<Int32> = SimplePropertyKey "listView.topItem"
        // Events
        member inline this.collectionChanged : SimplePropertyKey<NotifyCollectionChangedEventArgs->unit> = SimplePropertyKey "listView.collectionChanged"
        member inline this.openSelectedItem : SimplePropertyKey<ListViewItemEventArgs->unit> = SimplePropertyKey "listView.openSelectedItem"
        member inline this.rowRender : SimplePropertyKey<ListViewRowEventArgs->unit> = SimplePropertyKey "listView.rowRender"
        member inline this.selectedItemChanged : SimplePropertyKey<ListViewItemEventArgs->unit> = SimplePropertyKey "listView.selectedItemChanged"

    // Margin
    type marginPKeys() =
        inherit adornmentPKeys()

        // Properties
        member inline this.shadowStyle : SimplePropertyKey<ShadowStyle> = SimplePropertyKey "margin.shadowStyle"

    type menuv2PKeys() =
        inherit barPKeys()

        // Properties
        member inline this.selectedMenuItem : SimplePropertyKey<MenuItemv2> = SimplePropertyKey "menuv2.selectedMenuItem"
        member inline this.superMenuItem : SimplePropertyKey<MenuItemv2> = SimplePropertyKey "menuv2.superMenuItem"
        // Events
        member inline this.accepted : SimplePropertyKey< CommandEventArgs->unit> = SimplePropertyKey "menuv2.accepted"
        member inline this.selectedMenuItemChanged : SimplePropertyKey< MenuItemv2->unit> = SimplePropertyKey "menuv2.selectedMenuItemChanged"

    // MenuBarV2
    type menuBarv2PKeys() =
        inherit menuv2PKeys()

        // Properties
        member inline this.key : SimplePropertyKey<Key> = SimplePropertyKey "menuBarv2.key"
        member inline this.menus : SimplePropertyKey<MenuBarItemv2 array> = SimplePropertyKey "view.menus"
        // Events
        member inline this.keyChanged : SimplePropertyKey<KeyChangedEventArgs->unit> = SimplePropertyKey "menuBarv2.keyChanged"

    type shortcutPKeys() =
         inherit viewPKeys()

         // Properties
         member inline this.action : SimplePropertyKey<Action> = SimplePropertyKey "shortcut.action"
         member inline this.alignmentModes : SimplePropertyKey<AlignmentModes> = SimplePropertyKey "shortcut.alignmentModes"
         member inline this.commandView : SimplePropertyKey<View> = SimplePropertyKey "shortcut.commandView_view"
         member inline this.commandView_element : SingleElementKey<ITerminalElement> = SingleElementKey.create "shortcut.commandView_element"
         member inline this.forceFocusColors : SimplePropertyKey<bool> = SimplePropertyKey "shortcut.forceFocusColors"
         member inline this.helpText : SimplePropertyKey<string> = SimplePropertyKey "shortcut.helpText"
         member inline this.text : SimplePropertyKey<string> = SimplePropertyKey "shortcut.text"
         member inline this.bindKeyToApplication : SimplePropertyKey<bool> = SimplePropertyKey "shortcut.bindKeyToApplication"
         member inline this.key : SimplePropertyKey<Key> = SimplePropertyKey "shortcut.key"
         member inline this.minimumKeyTextSize : SimplePropertyKey<Int32> = SimplePropertyKey "shortcut.minimumKeyTextSize"
         // Events
         member inline this.orientationChanged : SimplePropertyKey<Orientation->unit> = SimplePropertyKey "shortcut.orientationChanged"
         member inline this.orientationChanging : SimplePropertyKey<CancelEventArgs<Orientation>->unit> = SimplePropertyKey "shortcut.orientationChanging"

    type menuItemv2PKeys() =
        inherit shortcutPKeys()
        member inline this.command : SimplePropertyKey< Command> = SimplePropertyKey "menuItemv2.command"
        member inline this.subMenu: SimplePropertyKey<Menuv2> = SimplePropertyKey "menuItemv2.subMenu_view"
        member inline this.subMenu_element : SingleElementKey<IMenuv2Element> = SingleElementKey.create "menuItemv2.subMenu_element"
        member inline this.targetView : SimplePropertyKey<View> = SimplePropertyKey "menuItemv2.targetView"
        member inline this.accepted: SimplePropertyKey<CommandEventArgs -> unit> = SimplePropertyKey "menuItemv2.accepted"

    type menuBarItemv2PKeys() =
        inherit menuItemv2PKeys()

        // Properties
        member inline this.popoverMenu : SimplePropertyKey<PopoverMenu> = SimplePropertyKey "menuBarItemv2.popoverMenu_view"
        member inline this.popoverMenu_element : SingleElementKey<IPopoverMenuElement> = SingleElementKey.create "menuBarItemv2.popoverMenu_element"
        member inline this.popoverMenuOpen : SimplePropertyKey<bool> = SimplePropertyKey "menuBarItemv2.popoverMenuOpen"
        // Events
        member inline this.popoverMenuOpenChanged : SimplePropertyKey<bool->unit> = SimplePropertyKey "menuBarItemv2.popoverMenuOpenChanged"

    type popoverMenuPKeys() =
        inherit viewPKeys()

        // Properties
        member inline this.key : SimplePropertyKey<Key> = SimplePropertyKey "popoverMenu.key"
        member inline this.mouseFlags : SimplePropertyKey<MouseFlags> = SimplePropertyKey "popoverMenu.mouseFlags"
        member inline this.root : SimplePropertyKey<Menuv2> = SimplePropertyKey "popoverMenu.root_view"
        member inline this.root_element : SingleElementKey<IMenuv2Element> = SingleElementKey.create "popoverMenu.root_element"
        // Events
        member inline this.accepted : SimplePropertyKey<CommandEventArgs->unit> = SimplePropertyKey "popoverMenu.accepted"
        member inline this.keyChanged : SimplePropertyKey<KeyChangedEventArgs->unit> = SimplePropertyKey "popoverMenu.keyChanged"

    // NumericUpDown`1
    type numericUpDownPKeys<'a>() =
        inherit viewPKeys()

        // Properties
        member inline this.format : SimplePropertyKey<string> = SimplePropertyKey "numericUpDown.format"
        member inline this.increment : SimplePropertyKey<'a> = SimplePropertyKey "numericUpDown.increment"
        member inline this.value : SimplePropertyKey<'a> = SimplePropertyKey "numericUpDown.value"
        // Events
        member inline this.formatChanged : SimplePropertyKey<string->unit> = SimplePropertyKey "numericUpDown.formatChanged"
        member inline this.incrementChanged : SimplePropertyKey<'a->unit> = SimplePropertyKey "numericUpDown.incrementChanged"
        member inline this.valueChanged : SimplePropertyKey<'a->unit> = SimplePropertyKey "numericUpDown.valueChanged"
        member inline this.valueChanging : SimplePropertyKey<App.CancelEventArgs<'a>->unit> = SimplePropertyKey "numericUpDown.valueChanging"

    // NumericUpDown
    // type numericUpDownPKeys() =
    //     inherit numericUpDownPKeys<int>()
    // No properties or events NumericUpDown

    // OpenDialog
    type openDialogPKeys() =
        inherit fileDialogPKeys()
        // Properties
        member inline this.openMode : SimplePropertyKey<OpenMode> = SimplePropertyKey "openDialog.openMode"

    // OptionSelector
    type optionSelectorPKeys() =
        inherit viewPKeys()
        //Properties
        member inline this.assignHotKeysToCheckBoxes : SimplePropertyKey<bool> = SimplePropertyKey "optionSelector.assignHotKeysToCheckBoxes"
        member inline this.orientation : SimplePropertyKey<Orientation> = SimplePropertyKey "optionSelector.orientation"
        member inline this.options : SimplePropertyKey<IReadOnlyList<string>> = SimplePropertyKey "optionSelector.options"
        member inline this.selectedItem : SimplePropertyKey<Int32> = SimplePropertyKey "optionSelector.selectedItem"
        // Events
        member inline this.orientationChanged : SimplePropertyKey<Orientation->unit> = SimplePropertyKey "optionSelector.orientationChanged"
        member inline this.orientationChanging : SimplePropertyKey<CancelEventArgs<Orientation>->unit> = SimplePropertyKey "optionSelector.orientationChanging"
        member inline this.selectedItemChanged : SimplePropertyKey<SelectedItemChangedArgs->unit> = SimplePropertyKey "optionSelector.selectedItemChanged"

    // Padding
    type paddingPKeys() =
        inherit adornmentPKeys()

    // ProgressBar
    type progressBarPKeys() =
        inherit viewPKeys()

        // Properties
        member inline this.bidirectionalMarquee : SimplePropertyKey<bool> = SimplePropertyKey "progressBar.bidirectionalMarquee"
        member inline this.fraction : SimplePropertyKey<Single> = SimplePropertyKey "progressBar.fraction"
        member inline this.progressBarFormat : SimplePropertyKey<ProgressBarFormat> = SimplePropertyKey "progressBar.progressBarFormat"
        member inline this.progressBarStyle : SimplePropertyKey<ProgressBarStyle> = SimplePropertyKey "progressBar.progressBarStyle"
        member inline this.segmentCharacter : SimplePropertyKey<Rune> = SimplePropertyKey "progressBar.segmentCharacter"
        member inline this.text : SimplePropertyKey<string> = SimplePropertyKey "progressBar.text"

    // RadioGroup
    type radioGroupPKeys() =
        inherit viewPKeys()

        // Properties
        member inline this.assignHotKeysToRadioLabels : SimplePropertyKey<bool> = SimplePropertyKey "radioGroup.assignHotKeysToRadioLabels"
        member inline this.cursor : SimplePropertyKey<Int32> = SimplePropertyKey "radioGroup.cursor"
        member inline this.doubleClickAccepts : SimplePropertyKey<bool> = SimplePropertyKey "radioGroup.doubleClickAccepts"
        member inline this.horizontalSpace : SimplePropertyKey<Int32> = SimplePropertyKey "radioGroup.horizontalSpace"
        member inline this.orientation : SimplePropertyKey<Orientation> = SimplePropertyKey "radioGroup.orientation"
        member inline this.radioLabels : SimplePropertyKey<string list> = SimplePropertyKey "radioGroup.radioLabels"
        member inline this.selectedItem : SimplePropertyKey<Int32> = SimplePropertyKey "radioGroup.selectedItem"
        // Events
        member inline this.orientationChanged : SimplePropertyKey<Orientation->unit> = SimplePropertyKey "radioGroup.orientationChanged"
        member inline this.orientationChanging : SimplePropertyKey<App.CancelEventArgs<Orientation>->unit> = SimplePropertyKey "radioGroup.orientationChanging"
        member inline this.selectedItemChanged : SimplePropertyKey<SelectedItemChangedArgs->unit> = SimplePropertyKey "radioGroup.selectedItemChanged"

    // SaveDialog
    // No properties or events SaveDialog

    // ScrollBar
    type scrollBarPKeys() =
        inherit viewPKeys()

        // Properties
        member inline this.autoShow : SimplePropertyKey<bool> = SimplePropertyKey "scrollBar.autoShow"
        member inline this.increment : SimplePropertyKey<Int32> = SimplePropertyKey "scrollBar.increment"
        member inline this.orientation : SimplePropertyKey<Orientation> = SimplePropertyKey "scrollBar.orientation"
        member inline this.position : SimplePropertyKey<Int32> = SimplePropertyKey "scrollBar.position"
        member inline this.scrollableContentSize : SimplePropertyKey<Int32> = SimplePropertyKey "scrollBar.scrollableContentSize"
        member inline this.visibleContentSize : SimplePropertyKey<Int32> = SimplePropertyKey "scrollBar.visibleContentSize"
        // Events
        member inline this.orientationChanged : SimplePropertyKey<Orientation->unit> = SimplePropertyKey "scrollBar.orientationChanged"
        member inline this.orientationChanging : SimplePropertyKey<CancelEventArgs<Orientation>->unit> = SimplePropertyKey "scrollBar.orientationChanging"
        member inline this.scrollableContentSizeChanged : SimplePropertyKey<EventArgs<Int32>->unit> = SimplePropertyKey "scrollBar.scrollableContentSizeChanged"
        member inline this.sliderPositionChanged : SimplePropertyKey<EventArgs<Int32>->unit> = SimplePropertyKey "scrollBar.sliderPositionChanged"

    // ScrollSlider
    type scrollSliderPKeys() =
        inherit viewPKeys()

        // Properties
        member inline this.orientation : SimplePropertyKey<Orientation> = SimplePropertyKey "scrollSlider.orientation"
        member inline this.position : SimplePropertyKey<Int32> = SimplePropertyKey "scrollSlider.position"
        member inline this.size : SimplePropertyKey<Int32> = SimplePropertyKey "scrollSlider.size"
        member inline this.sliderPadding : SimplePropertyKey<Int32> = SimplePropertyKey "scrollSlider.sliderPadding"
        member inline this.visibleContentSize : SimplePropertyKey<Int32> = SimplePropertyKey "scrollSlider.visibleContentSize"
        // Events
        member inline this.orientationChanged : SimplePropertyKey<Orientation->unit> = SimplePropertyKey "scrollSlider.orientationChanged"
        member inline this.orientationChanging : SimplePropertyKey<CancelEventArgs<Orientation>->unit> = SimplePropertyKey "scrollSlider.orientationChanging"
        member inline this.positionChanged : SimplePropertyKey<EventArgs<Int32>->unit> = SimplePropertyKey "scrollSlider.positionChanged"
        member inline this.positionChanging : SimplePropertyKey<CancelEventArgs<Int32>->unit> = SimplePropertyKey "scrollSlider.positionChanging"
        member inline this.scrolled : SimplePropertyKey<EventArgs<Int32>->unit> = SimplePropertyKey "scrollSlider.scrolled"

    // Slider`1
    type sliderPKeys<'a>() =
        inherit viewPKeys()

        // Properties
        member inline this.allowEmpty : SimplePropertyKey<bool> = SimplePropertyKey "slider.allowEmpty"
        member inline this.focusedOption : SimplePropertyKey<Int32> = SimplePropertyKey "slider.focusedOption"
        member inline this.legendsOrientation : SimplePropertyKey<Orientation> = SimplePropertyKey "slider.legendsOrientation"
        member inline this.minimumInnerSpacing : SimplePropertyKey<Int32> = SimplePropertyKey "slider.minimumInnerSpacing"
        member inline this.options : SimplePropertyKey<SliderOption<'a> list> = SimplePropertyKey "slider.options"
        member inline this.orientation : SimplePropertyKey<Orientation> = SimplePropertyKey "slider.orientation"
        member inline this.rangeAllowSingle : SimplePropertyKey<bool> = SimplePropertyKey "slider.rangeAllowSingle"
        member inline this.showEndSpacing : SimplePropertyKey<bool> = SimplePropertyKey "slider.showEndSpacing"
        member inline this.showLegends : SimplePropertyKey<bool> = SimplePropertyKey "slider.showLegends"
        member inline this.style : SimplePropertyKey<SliderStyle> = SimplePropertyKey "slider.style"
        member inline this.text : SimplePropertyKey<string> = SimplePropertyKey "slider.text"
        member inline this.``type`` : SimplePropertyKey<SliderType> = SimplePropertyKey "slider.``type``"
        member inline this.useMinimumSize : SimplePropertyKey<bool> = SimplePropertyKey "slider.useMinimumSize"
        // Events
        member inline this.optionFocused : SimplePropertyKey<SliderEventArgs<'a>->unit> = SimplePropertyKey "slider.optionFocused"
        member inline this.optionsChanged : SimplePropertyKey<SliderEventArgs<'a>->unit> = SimplePropertyKey "slider.optionsChanged"
        member inline this.orientationChanged : SimplePropertyKey<Orientation->unit> = SimplePropertyKey "slider.orientationChanged"
        member inline this.orientationChanging : SimplePropertyKey<App.CancelEventArgs<Orientation>->unit> = SimplePropertyKey "slider.orientationChanging"

    // Slider
    // type sliderPKeys() =
    //     inherit sliderPKeys<obj>()
    // No properties or events Slider

    // SpinnerView
    type spinnerViewPKeys() =
        inherit viewPKeys()

        // Properties
        member inline this.autoSpin : SimplePropertyKey<bool> = SimplePropertyKey "spinnerView.autoSpin"
        member inline this.sequence : SimplePropertyKey<string list> = SimplePropertyKey "spinnerView.sequence"
        member inline this.spinBounce : SimplePropertyKey<bool> = SimplePropertyKey "spinnerView.spinBounce"
        member inline this.spinDelay : SimplePropertyKey<Int32> = SimplePropertyKey "spinnerView.spinDelay"
        member inline this.spinReverse : SimplePropertyKey<bool> = SimplePropertyKey "spinnerView.spinReverse"
        member inline this.style : SimplePropertyKey<SpinnerStyle> = SimplePropertyKey "spinnerView.style"

    // StatusBar
    type statusBarPKeys() =
        inherit barPKeys()
    // No properties or events StatusBar

    // Tab
    type tabPKeys() =
        inherit viewPKeys()

        // Properties
        member inline this.displayText : SimplePropertyKey<string> = SimplePropertyKey "tab.displayText"
        member inline this.view : SimplePropertyKey<View> = SimplePropertyKey "tab.view_view"
        member inline this.view_element : SingleElementKey<ITerminalElement> = SingleElementKey.create "tab.view_element"

    // TabView
    type tabViewPKeys() =
        inherit viewPKeys()

        // Properties
        member inline this.maxTabTextWidth : SimplePropertyKey<int> = SimplePropertyKey "tabView.maxTabTextWidth"
        member inline this.selectedTab : SimplePropertyKey<Tab> = SimplePropertyKey "tabView.selectedTab"
        member inline this.style : SimplePropertyKey<TabStyle> = SimplePropertyKey "tabView.style"
        member inline this.tabScrollOffset : SimplePropertyKey<Int32> = SimplePropertyKey "tabView.tabScrollOffset"
        // Events
        member inline this.selectedTabChanged : SimplePropertyKey<TabChangedEventArgs->unit> = SimplePropertyKey "tabView.selectedTabChanged"
        member inline this.tabClicked : SimplePropertyKey<TabMouseEventArgs->unit> = SimplePropertyKey "tabView.tabClicked"
        // Additional properties
        member inline this.tabs : SimplePropertyKey<List<ITerminalElement>> = SimplePropertyKey "tabView.tabs_view"
        member inline this.tabs_elements : MultiElementKey<List<ITerminalElement>> = MultiElementKey.create "tabView.tabs_elements"

    // TableView
    type tableViewPKeys() =
        inherit viewPKeys()

        // Properties
        member inline this.cellActivationKey : SimplePropertyKey<KeyCode> = SimplePropertyKey "tableView.cellActivationKey"
        member inline this.collectionNavigator : SimplePropertyKey<ICollectionNavigator> = SimplePropertyKey "tableView.collectionNavigator"
        member inline this.columnOffset : SimplePropertyKey<Int32> = SimplePropertyKey "tableView.columnOffset"
        member inline this.fullRowSelect : SimplePropertyKey<bool> = SimplePropertyKey "tableView.fullRowSelect"
        member inline this.maxCellWidth : SimplePropertyKey<Int32> = SimplePropertyKey "tableView.maxCellWidth"
        member inline this.minCellWidth : SimplePropertyKey<Int32> = SimplePropertyKey "tableView.minCellWidth"
        member inline this.multiSelect : SimplePropertyKey<bool> = SimplePropertyKey "tableView.multiSelect"
        member inline this.nullSymbol : SimplePropertyKey<string> = SimplePropertyKey "tableView.nullSymbol"
        member inline this.rowOffset : SimplePropertyKey<Int32> = SimplePropertyKey "tableView.rowOffset"
        member inline this.selectedColumn : SimplePropertyKey<Int32> = SimplePropertyKey "tableView.selectedColumn"
        member inline this.selectedRow : SimplePropertyKey<Int32> = SimplePropertyKey "tableView.selectedRow"
        member inline this.separatorSymbol : SimplePropertyKey<Char> = SimplePropertyKey "tableView.separatorSymbol"
        member inline this.style : SimplePropertyKey<TableStyle> = SimplePropertyKey "tableView.style"
        member inline this.table : SimplePropertyKey<ITableSource> = SimplePropertyKey "tableView.table"
        // Events
        member inline this.cellActivated : SimplePropertyKey<CellActivatedEventArgs->unit> = SimplePropertyKey "tableView.cellActivated"
        member inline this.cellToggled : SimplePropertyKey<CellToggledEventArgs->unit> = SimplePropertyKey "tableView.cellToggled"
        member inline this.selectedCellChanged : SimplePropertyKey<SelectedCellChangedEventArgs->unit> = SimplePropertyKey "tableView.selectedCellChanged"

    // TextValidateField
    type textValidateFieldPKeys() =
        inherit viewPKeys()

        // Properties
        member inline this.provider : SimplePropertyKey<ITextValidateProvider> = SimplePropertyKey "textValidateField.provider"
        member inline this.text : SimplePropertyKey<string> = SimplePropertyKey "textValidateField.text"

    // TextView
    type textViewPKeys() =
        inherit viewPKeys()

        // Properties
        member inline this.allowsReturn : SimplePropertyKey<bool> = SimplePropertyKey "textView.allowsReturn"
        member inline this.allowsTab : SimplePropertyKey<bool> = SimplePropertyKey "textView.allowsTab"
        member inline this.cursorPosition : SimplePropertyKey<Point> = SimplePropertyKey "textView.cursorPosition"
        member inline this.inheritsPreviousAttribute : SimplePropertyKey<bool> = SimplePropertyKey "textView.inheritsPreviousAttribute"
        member inline this.isDirty : SimplePropertyKey<bool> = SimplePropertyKey "textView.isDirty"
        member inline this.isSelecting : SimplePropertyKey<bool> = SimplePropertyKey "textView.isSelecting"
        member inline this.leftColumn : SimplePropertyKey<Int32> = SimplePropertyKey "textView.leftColumn"
        member inline this.multiline : SimplePropertyKey<bool> = SimplePropertyKey "textView.multiline"
        member inline this.readOnly : SimplePropertyKey<bool> = SimplePropertyKey "textView.readOnly"
        member inline this.selectionStartColumn : SimplePropertyKey<Int32> = SimplePropertyKey "textView.selectionStartColumn"
        member inline this.selectionStartRow : SimplePropertyKey<Int32> = SimplePropertyKey "textView.selectionStartRow"
        member inline this.selectWordOnlyOnDoubleClick : SimplePropertyKey<bool> = SimplePropertyKey "textView.selectWordOnlyOnDoubleClick"
        member inline this.tabWidth : SimplePropertyKey<Int32> = SimplePropertyKey "textView.tabWidth"
        member inline this.text : SimplePropertyKey<string> = SimplePropertyKey "textView.text"
        member inline this.topRow : SimplePropertyKey<Int32> = SimplePropertyKey "textView.topRow"
        member inline this.used : SimplePropertyKey<bool> = SimplePropertyKey "textView.used"
        member inline this.useSameRuneTypeForWords : SimplePropertyKey<bool> = SimplePropertyKey "textView.useSameRuneTypeForWords"
        member inline this.wordWrap : SimplePropertyKey<bool> = SimplePropertyKey "textView.wordWrap"
        // Events
        member inline this.contentsChanged : SimplePropertyKey<ContentsChangedEventArgs->unit> = SimplePropertyKey "textView.contentsChanged"
        member inline this.drawNormalColor : SimplePropertyKey<CellEventArgs->unit> = SimplePropertyKey "textView.drawNormalColor"
        member inline this.drawReadOnlyColor : SimplePropertyKey<CellEventArgs->unit> = SimplePropertyKey "textView.drawReadOnlyColor"
        member inline this.drawSelectionColor : SimplePropertyKey<CellEventArgs->unit> = SimplePropertyKey "textView.drawSelectionColor"
        member inline this.drawUsedColor : SimplePropertyKey<CellEventArgs->unit> = SimplePropertyKey "textView.drawUsedColor"
        member inline this.unwrappedCursorPosition : SimplePropertyKey<Point->unit> = SimplePropertyKey "textView.unwrappedCursorPosition"
        // Additional properties
        member inline this.textChanged : SimplePropertyKey<string->unit> = SimplePropertyKey "textView.textChanged"

    // TileView
    type tileViewPKeys() =
        inherit viewPKeys()

        // Properties
        member inline this.lineStyle : SimplePropertyKey<LineStyle> = SimplePropertyKey "tileView.lineStyle"
        member inline this.orientation : SimplePropertyKey<Orientation> = SimplePropertyKey "tileView.orientation"
        member inline this.toggleResizable : SimplePropertyKey<KeyCode> = SimplePropertyKey "tileView.toggleResizable"
        // Events
        member inline this.splitterMoved : SimplePropertyKey<SplitterEventArgs->unit> = SimplePropertyKey "tileView.splitterMoved"

    // TimeField
    type timeFieldPKeys() =
        inherit textFieldPKeys()

        // Properties
        member inline this.cursorPosition : SimplePropertyKey<Int32> = SimplePropertyKey "timeField.cursorPosition"
        member inline this.isShortFormat : SimplePropertyKey<bool> = SimplePropertyKey "timeField.isShortFormat"
        member inline this.time : SimplePropertyKey<TimeSpan> = SimplePropertyKey "timeField.time"
        // Events
        member inline this.timeChanged : SimplePropertyKey<DateTimeEventArgs<TimeSpan>->unit> = SimplePropertyKey "timeField.timeChanged"

    // TreeView`1
    type treeViewPKeys<'a when 'a : not struct>() =
        inherit viewPKeys()

        // Properties
        member inline this.allowLetterBasedNavigation : SimplePropertyKey<bool> = SimplePropertyKey "treeView.allowLetterBasedNavigation"
        member inline this.aspectGetter : SimplePropertyKey<AspectGetterDelegate<'a>> = SimplePropertyKey "treeView.aspectGetter"
        member inline this.colorGetter : SimplePropertyKey<Func<'a,Scheme>> = SimplePropertyKey "treeView.colorGetter"
        member inline this.maxDepth : SimplePropertyKey<Int32> = SimplePropertyKey "treeView.maxDepth"
        member inline this.multiSelect : SimplePropertyKey<bool> = SimplePropertyKey "treeView.multiSelect"
        member inline this.objectActivationButton : SimplePropertyKey<MouseFlags option> = SimplePropertyKey "treeView.objectActivationButton"
        member inline this.objectActivationKey : SimplePropertyKey<KeyCode> = SimplePropertyKey "treeView.objectActivationKey"
        member inline this.scrollOffsetHorizontal : SimplePropertyKey<Int32> = SimplePropertyKey "treeView.scrollOffsetHorizontal"
        member inline this.scrollOffsetVertical : SimplePropertyKey<Int32> = SimplePropertyKey "treeView.scrollOffsetVertical"
        member inline this.selectedObject : SimplePropertyKey<'a> = SimplePropertyKey "treeView.selectedObject"
        member inline this.style : SimplePropertyKey<TreeStyle> = SimplePropertyKey "treeView.style"
        member inline this.treeBuilder : SimplePropertyKey<ITreeBuilder<'a>> = SimplePropertyKey "treeView.treeBuilder"
        // Events
        member inline this.drawLine : SimplePropertyKey<DrawTreeViewLineEventArgs<'a>->unit> = SimplePropertyKey "treeView.drawLine"
        member inline this.objectActivated : SimplePropertyKey<ObjectActivatedEventArgs<'a>->unit> = SimplePropertyKey "treeView.objectActivated"
        member inline this.selectionChanged : SimplePropertyKey<SelectionChangedEventArgs<'a>->unit> = SimplePropertyKey "treeView.selectionChanged"

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
        member inline this.currentStep : SimplePropertyKey<WizardStep> = SimplePropertyKey "wizard.currentStep"
        member inline this.modal : SimplePropertyKey<bool> = SimplePropertyKey "wizard.modal"
        // Events
        member inline this.cancelled : SimplePropertyKey<WizardButtonEventArgs->unit> = SimplePropertyKey "wizard.cancelled"
        member inline this.finished : SimplePropertyKey<WizardButtonEventArgs->unit> = SimplePropertyKey "wizard.finished"
        member inline this.movingBack : SimplePropertyKey<WizardButtonEventArgs->unit> = SimplePropertyKey "wizard.movingBack"
        member inline this.movingNext : SimplePropertyKey<WizardButtonEventArgs->unit> = SimplePropertyKey "wizard.movingNext"
        member inline this.stepChanged : SimplePropertyKey<StepChangeEventArgs->unit> = SimplePropertyKey "wizard.stepChanged"
        member inline this.stepChanging : SimplePropertyKey<StepChangeEventArgs->unit> = SimplePropertyKey "wizard.stepChanging"

    // WizardStep
    type wizardStepPKeys() =
        inherit viewPKeys()

        // Properties
        member inline this.backButtonText : SimplePropertyKey<string> = SimplePropertyKey "wizardStep.backButtonText"
        member inline this.helpText : SimplePropertyKey<string> = SimplePropertyKey "wizardStep.helpText"
        member inline this.nextButtonText : SimplePropertyKey<string> = SimplePropertyKey "wizardStep.nextButtonText"


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
