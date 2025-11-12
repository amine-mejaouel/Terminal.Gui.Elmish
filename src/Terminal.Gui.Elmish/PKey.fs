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
[<RequireQualifiedAccess>]
module internal PKey =

  type viewPKeys() =
    member val children: SimplePropKey<System.Collections.Generic.List<IInternalTerminalElement>> = SimplePropKey.create "children"

    // Properties
    member val arrangement: SimplePropKey<ViewArrangement> = SimplePropKey.create "view.arrangement"
    member val borderStyle: SimplePropKey<LineStyle> = SimplePropKey.create "view.borderStyle"
    member val canFocus: SimplePropKey<bool> = SimplePropKey.create "view.canFocus"
    member val contentSizeTracksViewport: SimplePropKey<bool> = SimplePropKey.create "view.contentSizeTracksViewport"
    member val cursorVisibility: SimplePropKey<CursorVisibility> = SimplePropKey.create "view.cursorVisibility"
    member val data: SimplePropKey<Object> = SimplePropKey.create "view.data"
    member val enabled: SimplePropKey<bool> = SimplePropKey.create "view.enabled"
    member val frame: SimplePropKey<Rectangle> = SimplePropKey.create "view.frame"
    member val hasFocus: SimplePropKey<bool> = SimplePropKey.create "view.hasFocus"
    member val height: SimplePropKey<Dim> = SimplePropKey.create "view.height"
    member val highlightStates: SimplePropKey<MouseState> = SimplePropKey.create "view.highlightStates"
    member val hotKey: SimplePropKey<Key> = SimplePropKey.create "view.hotKey"
    member val hotKeySpecifier: SimplePropKey<Rune> = SimplePropKey.create "view.hotKeySpecifier"
    member val id: SimplePropKey<string> = SimplePropKey.create "view.id"
    member val isInitialized: SimplePropKey<bool> = SimplePropKey.create "view.isInitialized"
    member val mouseHeldDown: SimplePropKey<IMouseHeldDown> = SimplePropKey.create "view.mouseHeldDown"
    member val needsDraw: SimplePropKey<bool> = SimplePropKey.create "view.needsDraw"
    member val preserveTrailingSpaces: SimplePropKey<bool> = SimplePropKey.create "view.preserveTrailingSpaces"
    member val schemeName: SimplePropKey<string> = SimplePropKey.create "view.schemeName"
    member val shadowStyle: SimplePropKey<ShadowStyle> = SimplePropKey.create "view.shadowStyle"
    member val superViewRendersLineCanvas: SimplePropKey<bool> = SimplePropKey.create "view.superViewRendersLineCanvas"
    member val tabStop: SimplePropKey<TabBehavior option> = SimplePropKey.create "view.tabStop"
    member val text: SimplePropKey<string> = SimplePropKey.create "view.text"
    member val textAlignment: SimplePropKey<Alignment> = SimplePropKey.create "view.textAlignment"
    member val textDirection: SimplePropKey<TextDirection> = SimplePropKey.create "view.textDirection"
    member val title: SimplePropKey<string> = SimplePropKey.create "view.title"
    member val validatePosDim: SimplePropKey<bool> = SimplePropKey.create "view.validatePosDim"
    member val verticalTextAlignment: SimplePropKey<Alignment> = SimplePropKey.create "view.verticalTextAlignment"
    member val viewport: SimplePropKey<Rectangle> = SimplePropKey.create "view.viewport"
    member val viewportSettings: SimplePropKey<ViewportSettingsFlags> = SimplePropKey.create "view.viewportSettings"
    member val visible: SimplePropKey<bool> = SimplePropKey.create "view.visible"
    member val wantContinuousButtonPressed: SimplePropKey<bool> = SimplePropKey.create "view.wantContinuousButtonPressed"
    member val wantMousePositionReports: SimplePropKey<bool> = SimplePropKey.create "view.wantMousePositionReports"
    member val width: SimplePropKey<Dim> = SimplePropKey.create "view.width"
    member val x: SimplePropKey<Pos> = SimplePropKey.create "view.x"
    member val x_delayedPos: DelayedPosKey = DelayedPosKey.create "view.x_delayedPos"
    member val y: SimplePropKey<Pos> = SimplePropKey.create "view.y"
    member val y_delayedPos: DelayedPosKey = DelayedPosKey.create "view.y_delayedPos"
    // Events
    member val accepting: SimplePropKey<HandledEventArgs -> unit> = SimplePropKey.create "view.accepting"
    member val advancingFocus: SimplePropKey<AdvanceFocusEventArgs -> unit> = SimplePropKey.create "view.advancingFocus"
    member val borderStyleChanged: SimplePropKey<EventArgs -> unit> = SimplePropKey.create "view.borderStyleChanged"
    member val canFocusChanged: SimplePropKey<unit -> unit> = SimplePropKey.create "view.canFocusChanged"
    member val clearedViewport: SimplePropKey<DrawEventArgs -> unit> = SimplePropKey.create "view.clearedViewport"
    member val clearingViewport: SimplePropKey<DrawEventArgs -> unit> = SimplePropKey.create "view.clearingViewport"
    member val commandNotBound: SimplePropKey<CommandEventArgs -> unit> = SimplePropKey.create "view.commandNotBound"
    member val contentSizeChanged: SimplePropKey<SizeChangedEventArgs -> unit> = SimplePropKey.create "view.contentSizeChanged"
    member val disposing: SimplePropKey<unit -> unit> = SimplePropKey.create "view.disposing"
    member val drawComplete: SimplePropKey<DrawEventArgs -> unit> = SimplePropKey.create "view.drawComplete"
    member val drawingContent: SimplePropKey<DrawEventArgs -> unit> = SimplePropKey.create "view.drawingContent"
    member val drawingSubViews: SimplePropKey<DrawEventArgs -> unit> = SimplePropKey.create "view.drawingSubViews"
    member val drawingText: SimplePropKey<DrawEventArgs -> unit> = SimplePropKey.create "view.drawingText"
    member val enabledChanged: SimplePropKey<unit -> unit> = SimplePropKey.create "view.enabledChanged"
    member val focusedChanged: SimplePropKey<HasFocusEventArgs -> unit> = SimplePropKey.create "view.focusedChanged"
    member val frameChanged: SimplePropKey<EventArgs<Rectangle> -> unit> = SimplePropKey.create "view.frameChanged"
    member val gettingAttributeForRole: SimplePropKey<VisualRoleEventArgs -> unit> = SimplePropKey.create "view.gettingAttributeForRole"
    member val gettingScheme: SimplePropKey<ResultEventArgs<Scheme> -> unit> = SimplePropKey.create "view.gettingScheme"
    member val handlingHotKey: SimplePropKey<CommandEventArgs -> unit> = SimplePropKey.create "view.handlingHotKey"
    member val hasFocusChanged: SimplePropKey<HasFocusEventArgs -> unit> = SimplePropKey.create "view.hasFocusChanged"
    member val hasFocusChanging: SimplePropKey<HasFocusEventArgs -> unit> = SimplePropKey.create "view.hasFocusChanging"
    member val hotKeyChanged: SimplePropKey<KeyChangedEventArgs -> unit> = SimplePropKey.create "view.hotKeyChanged"
    member val initialized: SimplePropKey<unit -> unit> = SimplePropKey.create "view.initialized"
    member val keyDown: SimplePropKey<Key -> unit> = SimplePropKey.create "view.keyDown"
    member val keyDownNotHandled: SimplePropKey<Key -> unit> = SimplePropKey.create "view.keyDownNotHandled"
    member val keyUp: SimplePropKey<Key -> unit> = SimplePropKey.create "view.keyUp"
    member val mouseClick: SimplePropKey<MouseEventArgs -> unit> = SimplePropKey.create "view.mouseClick"
    member val mouseEnter: SimplePropKey<CancelEventArgs -> unit> = SimplePropKey.create "view.mouseEnter"
    member val mouseEvent: SimplePropKey<MouseEventArgs -> unit> = SimplePropKey.create "view.mouseEvent"
    member val mouseLeave: SimplePropKey<EventArgs -> unit> = SimplePropKey.create "view.mouseLeave"
    member val mouseStateChanged: SimplePropKey<EventArgs -> unit> = SimplePropKey.create "view.mouseStateChanged"
    member val mouseWheel: SimplePropKey<MouseEventArgs -> unit> = SimplePropKey.create "view.mouseWheel"
    member val removed: SimplePropKey<SuperViewChangedEventArgs -> unit> = SimplePropKey.create "view.removed"
    member val schemeChanged: SimplePropKey<ValueChangedEventArgs<Scheme> -> unit> = SimplePropKey.create "view.schemeChanged"
    member val schemeChanging: SimplePropKey<ValueChangingEventArgs<Scheme> -> unit> = SimplePropKey.create "view.schemeChanging"
    member val schemeNameChanged: SimplePropKey<ValueChangedEventArgs<string> -> unit> = SimplePropKey.create "view.schemeNameChanged"
    member val schemeNameChanging: SimplePropKey<ValueChangingEventArgs<string> -> unit> = SimplePropKey.create "view.schemeNameChanging"
    member val selecting: SimplePropKey<CommandEventArgs -> unit> = SimplePropKey.create "view.selecting"
    member val subViewAdded: SimplePropKey<SuperViewChangedEventArgs -> unit> = SimplePropKey.create "view.subViewAdded"
    member val subViewLayout: SimplePropKey<LayoutEventArgs -> unit> = SimplePropKey.create "view.subViewLayout"
    member val subViewRemoved: SimplePropKey<SuperViewChangedEventArgs -> unit> = SimplePropKey.create "view.subViewRemoved"
    member val subViewsLaidOut: SimplePropKey<LayoutEventArgs -> unit> = SimplePropKey.create "view.subViewsLaidOut"
    member val superViewChanged: SimplePropKey<SuperViewChangedEventArgs -> unit> = SimplePropKey.create "view.superViewChanged"
    member val textChanged: SimplePropKey<unit -> unit> = SimplePropKey.create "view.textChanged"
    member val titleChanged: SimplePropKey<string -> unit> = SimplePropKey.create "view.titleChanged"
    member val titleChanging: SimplePropKey<App.CancelEventArgs<string> -> unit> = SimplePropKey.create "view.titleChanging"
    member val viewportChanged: SimplePropKey<DrawEventArgs -> unit> = SimplePropKey.create "view.viewportChanged"
    member val visibleChanged: SimplePropKey<unit -> unit> = SimplePropKey.create "view.visibleChanged"
    member val visibleChanging: SimplePropKey<unit -> unit> = SimplePropKey.create "view.visibleChanging"

  // Adornment
  type adornmentPKeys() =
    inherit viewPKeys()
    // Properties
    member val diagnostics: SimplePropKey<ViewDiagnosticFlags> = SimplePropKey.create "adornment.diagnostics"
    member val superViewRendersLineCanvas: SimplePropKey<bool> = SimplePropKey.create "adornment.superViewRendersLineCanvas"
    member val thickness: SimplePropKey<Thickness> = SimplePropKey.create "adornment.thickness"
    member val viewport: SimplePropKey<Rectangle> = SimplePropKey.create "adornment.viewport"
    // Events
    member val thicknessChanged: SimplePropKey<unit -> unit> = SimplePropKey.create "adornment.thicknessChanged"

  // Bar
  type barPKeys() =
    inherit viewPKeys()
    // Properties
    member val alignmentModes: SimplePropKey<AlignmentModes> = SimplePropKey.create "bar.alignmentModes"
    member val orientation: SimplePropKey<Orientation> = SimplePropKey.create "bar.orientation"
    // Events
    member val orientationChanged: SimplePropKey<Orientation -> unit> = SimplePropKey.create "bar.orientationChanged"
    member val orientationChanging: SimplePropKey<App.CancelEventArgs<Orientation> -> unit> = SimplePropKey.create "bar.orientationChanging"

  // Border
  type borderPKeys() =
    inherit adornmentPKeys()
    // Properties
    member val lineStyle: SimplePropKey<LineStyle> = SimplePropKey.create "border.lineStyle"
    member val settings: SimplePropKey<BorderSettings> = SimplePropKey.create "border.settings"

  // Button
  type buttonPKeys() =
    inherit viewPKeys()
    // Properties
    member val hotKeySpecifier: SimplePropKey<Rune> = SimplePropKey.create "button.hotKeySpecifier"
    member val isDefault: SimplePropKey<bool> = SimplePropKey.create "button.isDefault"
    member val noDecorations: SimplePropKey<bool> = SimplePropKey.create "button.noDecorations"
    member val noPadding: SimplePropKey<bool> = SimplePropKey.create "button.noPadding"
    member val text: SimplePropKey<string> = SimplePropKey.create "button.text"
    member val wantContinuousButtonPressed: SimplePropKey<bool> = SimplePropKey.create "button.wantContinuousButtonPressed"

  // CheckBox
  type checkBoxPKeys() =
    inherit viewPKeys()
    // Properties
    member val allowCheckStateNone: SimplePropKey<bool> = SimplePropKey.create "checkBox.allowCheckStateNone"
    member val checkedState: SimplePropKey<CheckState> = SimplePropKey.create "checkBox.checkedState"
    member val hotKeySpecifier: SimplePropKey<Rune> = SimplePropKey.create "checkBox.hotKeySpecifier"
    member val radioStyle: SimplePropKey<bool> = SimplePropKey.create "checkBox.radioStyle"
    member val text: SimplePropKey<string> = SimplePropKey.create "checkBox.text"
    // Events
    member val checkedStateChanging: SimplePropKey<ResultEventArgs<CheckState> -> unit> = SimplePropKey.create "checkBox.checkedStateChanging"
    member val checkedStateChanged: SimplePropKey<EventArgs<CheckState> -> unit> = SimplePropKey.create "checkBox.checkedStateChanged"

  // ColorPicker
  type colorPickerPKeys() =
    inherit viewPKeys()
    // Properties
    member val selectedColor: SimplePropKey<Color> = SimplePropKey.create "colorPicker.selectedColor"
    member val style: SimplePropKey<ColorPickerStyle> = SimplePropKey.create "colorPicker.style"
    // Events
    member val colorChanged: SimplePropKey<ResultEventArgs<Color> -> unit> = SimplePropKey.create "colorPicker.colorChanged"

  // ColorPicker16
  type colorPicker16PKeys() =
    inherit viewPKeys()
    // Properties
    member val boxHeight: SimplePropKey<Int32> = SimplePropKey.create "colorPicker16.boxHeight"
    member val boxWidth: SimplePropKey<Int32> = SimplePropKey.create "colorPicker16.boxWidth"
    member val cursor: SimplePropKey<Point> = SimplePropKey.create "colorPicker16.cursor"
    member val selectedColor: SimplePropKey<ColorName16> = SimplePropKey.create "colorPicker16.selectedColor"
    // Events
    member val colorChanged: SimplePropKey<ResultEventArgs<Color> -> unit> = SimplePropKey.create "colorPicker16.colorChanged"

  // ComboBox
  type comboBoxPKeys() =
    inherit viewPKeys()
    // Properties
    member val hideDropdownListOnClick: SimplePropKey<bool> = SimplePropKey.create "comboBox.hideDropdownListOnClick"
    member val readOnly: SimplePropKey<bool> = SimplePropKey.create "comboBox.readOnly"
    member val searchText: SimplePropKey<string> = SimplePropKey.create "comboBox.searchText"
    member val selectedItem: SimplePropKey<Int32> = SimplePropKey.create "comboBox.selectedItem"
    member val source: SimplePropKey<string list> = SimplePropKey.create "comboBox.source"
    member val text: SimplePropKey<string> = SimplePropKey.create "comboBox.text"
    // Events
    member val collapsed: SimplePropKey<unit -> unit> = SimplePropKey.create "comboBox.collapsed"
    member val expanded: SimplePropKey<unit -> unit> = SimplePropKey.create "comboBox.expanded"
    member val openSelectedItem: SimplePropKey<ListViewItemEventArgs -> unit> = SimplePropKey.create "comboBox.openSelectedItem"
    member val selectedItemChanged: SimplePropKey<ListViewItemEventArgs -> unit> = SimplePropKey.create "comboBox.selectedItemChanged"

  // TextField
  type textFieldPKeys() =
    inherit viewPKeys()
    // Properties
    member val autocomplete: SimplePropKey<IAutocomplete> = SimplePropKey.create "textField.autocomplete"
    member val cursorPosition: SimplePropKey<Int32> = SimplePropKey.create "textField.cursorPosition"
    member val readOnly: SimplePropKey<bool> = SimplePropKey.create "textField.readOnly"
    member val secret: SimplePropKey<bool> = SimplePropKey.create "textField.secret"
    member val selectedStart: SimplePropKey<Int32> = SimplePropKey.create "textField.selectedStart"
    member val selectWordOnlyOnDoubleClick: SimplePropKey<bool> = SimplePropKey.create "textField.selectWordOnlyOnDoubleClick"
    member val text: SimplePropKey<string> = SimplePropKey.create "textField.text"
    member val used: SimplePropKey<bool> = SimplePropKey.create "textField.used"
    member val useSameRuneTypeForWords: SimplePropKey<bool> = SimplePropKey.create "textField.useSameRuneTypeForWords"
    // Events
    member val textChanging: SimplePropKey<ResultEventArgs<string> -> unit> = SimplePropKey.create "textField.textChanging"

  // DateField
  type dateFieldPKeys() =
    inherit textFieldPKeys()
    // Properties
    member val culture: SimplePropKey<CultureInfo> = SimplePropKey.create "dateField.culture"
    member val cursorPosition: SimplePropKey<Int32> = SimplePropKey.create "dateField.cursorPosition"
    member val date: SimplePropKey<DateTime> = SimplePropKey.create "dateField.date"
    // Events
    member val dateChanged: SimplePropKey<EventArgs<DateTime> -> unit> = SimplePropKey.create "dateField.dateChanged"

  // DatePicker
  type datePickerPKeys() =
    inherit viewPKeys()
    // Properties
    member val culture: SimplePropKey<CultureInfo> = SimplePropKey.create "datePicker.culture"
    member val date: SimplePropKey<DateTime> = SimplePropKey.create "datePicker.date"

  // Toplevel
  type toplevelPKeys() =
    inherit viewPKeys()
    // Properties
    member val modal: SimplePropKey<bool> = SimplePropKey.create "toplevel.modal"
    member val running: SimplePropKey<bool> = SimplePropKey.create "toplevel.running"
    // Events
    member val activate: SimplePropKey<ToplevelEventArgs -> unit> = SimplePropKey.create "toplevel.activate"
    member val closed: SimplePropKey<ToplevelEventArgs -> unit> = SimplePropKey.create "toplevel.closed"
    member val closing: SimplePropKey<ToplevelClosingEventArgs -> unit> = SimplePropKey.create "toplevel.closing"
    member val deactivate: SimplePropKey<ToplevelEventArgs -> unit> = SimplePropKey.create "toplevel.deactivate"
    member val loaded: SimplePropKey<unit -> unit> = SimplePropKey.create "toplevel.loaded"
    member val ready: SimplePropKey<unit -> unit> = SimplePropKey.create "toplevel.ready"
    member val sizeChanging: SimplePropKey<SizeChangedEventArgs -> unit> = SimplePropKey.create "toplevel.sizeChanging"
    member val unloaded: SimplePropKey<unit -> unit> = SimplePropKey.create "toplevel.unloaded"

  // Dialog
  type dialogPKeys() =
    inherit toplevelPKeys()
    // Properties
    member val buttonAlignment: SimplePropKey<Alignment> = SimplePropKey.create "dialog.buttonAlignment"
    member val buttonAlignmentModes: SimplePropKey<AlignmentModes> = SimplePropKey.create "dialog.buttonAlignmentModes"
    member val canceled: SimplePropKey<bool> = SimplePropKey.create "dialog.canceled"

  // FileDialog
  type fileDialogPKeys() =
    inherit dialogPKeys()
    // Properties
    member val allowedTypes: SimplePropKey<IAllowedType list> = SimplePropKey.create "fileDialog.allowedTypes"
    member val allowsMultipleSelection: SimplePropKey<bool> = SimplePropKey.create "fileDialog.allowsMultipleSelection"
    member val fileOperationsHandler: SimplePropKey<IFileOperations> = SimplePropKey.create "fileDialog.fileOperationsHandler"
    member val mustExist: SimplePropKey<bool> = SimplePropKey.create "fileDialog.mustExist"
    member val openMode: SimplePropKey<OpenMode> = SimplePropKey.create "fileDialog.openMode"
    member val path: SimplePropKey<string> = SimplePropKey.create "fileDialog.path"
    member val searchMatcher: SimplePropKey<ISearchMatcher> = SimplePropKey.create "fileDialog.searchMatcher"
    // Events
    member val filesSelected: SimplePropKey<FilesSelectedEventArgs -> unit> = SimplePropKey.create "fileDialog.filesSelected"

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
    member val axisX: SimplePropKey<HorizontalAxis> = SimplePropKey.create "graphView.axisX"
    member val axisY: SimplePropKey<VerticalAxis> = SimplePropKey.create "graphView.axisY"
    member val cellSize: SimplePropKey<PointF> = SimplePropKey.create "graphView.cellSize"
    member val graphColor: SimplePropKey<Attribute option> = SimplePropKey.create "graphView.graphColor"
    member val marginBottom: SimplePropKey<int> = SimplePropKey.create "graphView.marginBottom"
    member val marginLeft: SimplePropKey<int> = SimplePropKey.create "graphView.marginLeft"
    member val scrollOffset: SimplePropKey<PointF> = SimplePropKey.create "graphView.scrollOffset"

  // HexView
  type hexViewPKeys() =
    inherit viewPKeys()

    // Properties
    member val address: SimplePropKey<Int64> = SimplePropKey.create "hexView.address"
    member val addressWidth: SimplePropKey<int> = SimplePropKey.create "hexView.addressWidth"
    member val allowEdits: SimplePropKey<int> = SimplePropKey.create "hexView.allowEdits"
    member val readOnly: SimplePropKey<bool> = SimplePropKey.create "hexView.readOnly"
    member val source: SimplePropKey<Stream> = SimplePropKey.create "hexView.source"
    // Events
    member val edited: SimplePropKey<HexViewEditEventArgs -> unit> = SimplePropKey.create "hexView.edited"
    member val positionChanged: SimplePropKey<HexViewEventArgs -> unit> = SimplePropKey.create "hexView.positionChanged"

  // Label
  type labelPKeys() =
    inherit viewPKeys()

    // Properties
    member val hotKeySpecifier: SimplePropKey<Rune> = SimplePropKey.create "label.hotKeySpecifier"
    member val text: SimplePropKey<string> = SimplePropKey.create "label.text"

  // LegendAnnotation
  type legendAnnotationPKeys() =
    inherit viewPKeys()
  // No properties or events LegendAnnotation

  // Line
  type linePKeys() =
    inherit viewPKeys()

    // Properties
    member val orientation: SimplePropKey<Orientation> = SimplePropKey.create "line.orientation"
    // Events
    member val orientationChanged: SimplePropKey<Orientation -> unit> = SimplePropKey.create "line.orientationChanged"
    member val orientationChanging: SimplePropKey<App.CancelEventArgs<Orientation> -> unit> = SimplePropKey.create "line.orientationChanging"

  // ListView
  type listViewPKeys() =
    inherit viewPKeys()

    // Properties
    member val allowsMarking: SimplePropKey<bool> = SimplePropKey.create "listView.allowsMarking"
    member val allowsMultipleSelection: SimplePropKey<bool> = SimplePropKey.create "listView.allowsMultipleSelection"
    member val leftItem: SimplePropKey<Int32> = SimplePropKey.create "listView.leftItem"
    member val selectedItem: SimplePropKey<Int32> = SimplePropKey.create "listView.selectedItem"
    member val source: SimplePropKey<string list> = SimplePropKey.create "listView.source"
    member val topItem: SimplePropKey<Int32> = SimplePropKey.create "listView.topItem"
    // Events
    member val collectionChanged: SimplePropKey<NotifyCollectionChangedEventArgs -> unit> = SimplePropKey.create "listView.collectionChanged"
    member val openSelectedItem: SimplePropKey<ListViewItemEventArgs -> unit> = SimplePropKey.create "listView.openSelectedItem"
    member val rowRender: SimplePropKey<ListViewRowEventArgs -> unit> = SimplePropKey.create "listView.rowRender"
    member val selectedItemChanged: SimplePropKey<ListViewItemEventArgs -> unit> = SimplePropKey.create "listView.selectedItemChanged"

  // Margin
  type marginPKeys() =
    inherit adornmentPKeys()

    // Properties
    member val shadowStyle: SimplePropKey<ShadowStyle> = SimplePropKey.create "margin.shadowStyle"

  type menuv2PKeys() =
    inherit barPKeys()

    // Properties
    member val selectedMenuItem: SimplePropKey<MenuItemv2> = SimplePropKey.create "menuv2.selectedMenuItem"
    member val superMenuItem: SimplePropKey<MenuItemv2> = SimplePropKey.create "menuv2.superMenuItem"
    // Events
    member val accepted: SimplePropKey<CommandEventArgs -> unit> = SimplePropKey.create "menuv2.accepted"
    member val selectedMenuItemChanged: SimplePropKey<MenuItemv2 -> unit> = SimplePropKey.create "menuv2.selectedMenuItemChanged"

  // MenuBarV2
  type menuBarv2PKeys() =
    inherit menuv2PKeys()

    // Properties
    member val key: SimplePropKey<Key> = SimplePropKey.create "menuBarv2.key"
    member val menus: SimplePropKey<MenuBarItemv2 array> = SimplePropKey.create "view.menus"
    // Events
    member val keyChanged: SimplePropKey<KeyChangedEventArgs -> unit> = SimplePropKey.create "menuBarv2.keyChanged"

  type shortcutPKeys() =
    inherit viewPKeys()

    // Properties
    member val action: SimplePropKey<Action> = SimplePropKey.create "shortcut.action"
    member val alignmentModes: SimplePropKey<AlignmentModes> = SimplePropKey.create "shortcut.alignmentModes"
    member val commandView: ViewPropKey<View> = ViewPropKey.create "shortcut.commandView_view"
    member val commandView_element: SingleElementPropKey<ITerminalElement> = SingleElementPropKey.create "shortcut.commandView_element"
    member val forceFocusColors: SimplePropKey<bool> = SimplePropKey.create "shortcut.forceFocusColors"
    member val helpText: SimplePropKey<string> = SimplePropKey.create "shortcut.helpText"
    member val text: SimplePropKey<string> = SimplePropKey.create "shortcut.text"
    member val bindKeyToApplication: SimplePropKey<bool> = SimplePropKey.create "shortcut.bindKeyToApplication"
    member val key: SimplePropKey<Key> = SimplePropKey.create "shortcut.key"
    member val minimumKeyTextSize: SimplePropKey<Int32> = SimplePropKey.create "shortcut.minimumKeyTextSize"
    // Events
    member val orientationChanged: SimplePropKey<Orientation -> unit> = SimplePropKey.create "shortcut.orientationChanged"
    member val orientationChanging: SimplePropKey<CancelEventArgs<Orientation> -> unit> = SimplePropKey.create "shortcut.orientationChanging"

  type menuItemv2PKeys() =
    inherit shortcutPKeys()
    member val command: SimplePropKey<Command> = SimplePropKey.create "menuItemv2.command"
    member val subMenu: ViewPropKey<Menuv2> = ViewPropKey.create "menuItemv2.subMenu_view"
    member val subMenu_element: SingleElementPropKey<IMenuv2Element> = SingleElementPropKey.create "menuItemv2.subMenu_element"
    member val targetView: SimplePropKey<View> = SimplePropKey.create "menuItemv2.targetView"
    member val accepted: SimplePropKey<CommandEventArgs -> unit> = SimplePropKey.create "menuItemv2.accepted"

  type menuBarItemv2PKeys() =
    inherit menuItemv2PKeys()

    // Properties
    member val popoverMenu: ViewPropKey<PopoverMenu> = ViewPropKey.create "menuBarItemv2.popoverMenu_view"
    member val popoverMenu_element: SingleElementPropKey<IPopoverMenuElement> = SingleElementPropKey.create "menuBarItemv2.popoverMenu_element"
    member val popoverMenuOpen: SimplePropKey<bool> = SimplePropKey.create "menuBarItemv2.popoverMenuOpen"
    // Events
    member val popoverMenuOpenChanged: SimplePropKey<bool -> unit> = SimplePropKey.create "menuBarItemv2.popoverMenuOpenChanged"

  type popoverMenuPKeys() =
    inherit viewPKeys()

    // Properties
    member val key: SimplePropKey<Key> = SimplePropKey.create "popoverMenu.key"
    member val mouseFlags: SimplePropKey<MouseFlags> = SimplePropKey.create "popoverMenu.mouseFlags"
    member val root: ViewPropKey<Menuv2> = ViewPropKey.create "popoverMenu.root_view"
    member val root_element: SingleElementPropKey<IMenuv2Element> = SingleElementPropKey.create "popoverMenu.root_element"
    // Events
    member val accepted: SimplePropKey<CommandEventArgs -> unit> = SimplePropKey.create "popoverMenu.accepted"
    member val keyChanged: SimplePropKey<KeyChangedEventArgs -> unit> = SimplePropKey.create "popoverMenu.keyChanged"

  // NumericUpDown`1
  type numericUpDownPKeys<'a>() =
    inherit viewPKeys()

    // Properties
    member val format: SimplePropKey<string> = SimplePropKey.create "numericUpDown.format"
    member val increment: SimplePropKey<'a> = SimplePropKey.create "numericUpDown.increment"
    member val value: SimplePropKey<'a> = SimplePropKey.create "numericUpDown.value"
    // Events
    member val formatChanged: SimplePropKey<string -> unit> = SimplePropKey.create "numericUpDown.formatChanged"
    member val incrementChanged: SimplePropKey<'a -> unit> = SimplePropKey.create "numericUpDown.incrementChanged"
    member val valueChanged: SimplePropKey<'a -> unit> = SimplePropKey.create "numericUpDown.valueChanged"
    member val valueChanging: SimplePropKey<App.CancelEventArgs<'a> -> unit> = SimplePropKey.create "numericUpDown.valueChanging"

  // NumericUpDown
  // type numericUpDownPKeys() =
  //     inherit numericUpDownPKeys<int>()
  // No properties or events NumericUpDown

  // OpenDialog
  type openDialogPKeys() =
    inherit fileDialogPKeys()
    // Properties
    member val openMode: SimplePropKey<OpenMode> = SimplePropKey.create "openDialog.openMode"

  // SelectorBase
  type selectorBasePKeys() =
    inherit viewPKeys()

    //Properties
    member val assignHotKeys: SimplePropKey<bool> = SimplePropKey.create "selectorBase.assignHotKeys"
    member val doubleClickAccepts: SimplePropKey<bool> = SimplePropKey.create "selectorBase.doubleClickAccepts"
    member val horizontalSpace: SimplePropKey<int> = SimplePropKey.create "selectorBase.horizontalSpace"
    member val labels: SimplePropKey<IReadOnlyList<string>> = SimplePropKey.create "selectorBase.labels"
    member val styles: SimplePropKey<SelectorStyles> = SimplePropKey.create "selectorBase.styles"
    member val usedHotKeys: SimplePropKey<HashSet<Key>> = SimplePropKey.create "selectorBase.usedHotKeys"
    member val value: SimplePropKey<Nullable<int>> = SimplePropKey.create "selectorBase.value"
    member val values: SimplePropKey<IReadOnlyList<int>> = SimplePropKey.create "selectorBase.values"
    // Events
    member val valueChanged: SimplePropKey<EventArgs<Nullable<int>> -> unit> = SimplePropKey.create "selectorBase.valueChanged"

  // OptionSelector
  type optionSelectorPKeys() =
    inherit selectorBasePKeys()
    //Properties
    member val cursor: SimplePropKey<int> = SimplePropKey.create "optionSelector.cursor"

  // FlagSelector
  type flagSelectorPKeys() =
    inherit selectorBasePKeys()
    //Properties
    member val value: SimplePropKey<int> = SimplePropKey.create "flagSelector.value"

  // Padding
  type paddingPKeys() =
    inherit adornmentPKeys()

  // ProgressBar
  type progressBarPKeys() =
    inherit viewPKeys()

    // Properties
    member val bidirectionalMarquee: SimplePropKey<bool> = SimplePropKey.create "progressBar.bidirectionalMarquee"
    member val fraction: SimplePropKey<Single> = SimplePropKey.create "progressBar.fraction"
    member val progressBarFormat: SimplePropKey<ProgressBarFormat> = SimplePropKey.create "progressBar.progressBarFormat"
    member val progressBarStyle: SimplePropKey<ProgressBarStyle> = SimplePropKey.create "progressBar.progressBarStyle"
    member val segmentCharacter: SimplePropKey<Rune> = SimplePropKey.create "progressBar.segmentCharacter"
    member val text: SimplePropKey<string> = SimplePropKey.create "progressBar.text"

  // SaveDialog
  // No properties or events SaveDialog

  // ScrollBar
  type scrollBarPKeys() =
    inherit viewPKeys()

    // Properties
    member val autoShow: SimplePropKey<bool> = SimplePropKey.create "scrollBar.autoShow"
    member val increment: SimplePropKey<Int32> = SimplePropKey.create "scrollBar.increment"
    member val orientation: SimplePropKey<Orientation> = SimplePropKey.create "scrollBar.orientation"
    member val position: SimplePropKey<Int32> = SimplePropKey.create "scrollBar.position"
    member val scrollableContentSize: SimplePropKey<Int32> = SimplePropKey.create "scrollBar.scrollableContentSize"
    member val visibleContentSize: SimplePropKey<Int32> = SimplePropKey.create "scrollBar.visibleContentSize"
    // Events
    member val orientationChanged: SimplePropKey<Orientation -> unit> = SimplePropKey.create "scrollBar.orientationChanged"
    member val orientationChanging: SimplePropKey<CancelEventArgs<Orientation> -> unit> = SimplePropKey.create "scrollBar.orientationChanging"
    member val scrollableContentSizeChanged: SimplePropKey<EventArgs<Int32> -> unit> = SimplePropKey.create "scrollBar.scrollableContentSizeChanged"
    member val sliderPositionChanged: SimplePropKey<EventArgs<Int32> -> unit> = SimplePropKey.create "scrollBar.sliderPositionChanged"

  // ScrollSlider
  type scrollSliderPKeys() =
    inherit viewPKeys()

    // Properties
    member val orientation: SimplePropKey<Orientation> = SimplePropKey.create "scrollSlider.orientation"
    member val position: SimplePropKey<Int32> = SimplePropKey.create "scrollSlider.position"
    member val size: SimplePropKey<Int32> = SimplePropKey.create "scrollSlider.size"
    member val sliderPadding: SimplePropKey<Int32> = SimplePropKey.create "scrollSlider.sliderPadding"
    member val visibleContentSize: SimplePropKey<Int32> = SimplePropKey.create "scrollSlider.visibleContentSize"
    // Events
    member val orientationChanged: SimplePropKey<Orientation -> unit> = SimplePropKey.create "scrollSlider.orientationChanged"
    member val orientationChanging: SimplePropKey<CancelEventArgs<Orientation> -> unit> = SimplePropKey.create "scrollSlider.orientationChanging"
    member val positionChanged: SimplePropKey<EventArgs<Int32> -> unit> = SimplePropKey.create "scrollSlider.positionChanged"
    member val positionChanging: SimplePropKey<CancelEventArgs<Int32> -> unit> = SimplePropKey.create "scrollSlider.positionChanging"
    member val scrolled: SimplePropKey<EventArgs<Int32> -> unit> = SimplePropKey.create "scrollSlider.scrolled"

  // Slider`1
  type sliderPKeys<'a>() =
    inherit viewPKeys()

    // Properties
    member val allowEmpty: SimplePropKey<bool> = SimplePropKey.create "slider.allowEmpty"
    member val focusedOption: SimplePropKey<Int32> = SimplePropKey.create "slider.focusedOption"
    member val legendsOrientation: SimplePropKey<Orientation> = SimplePropKey.create "slider.legendsOrientation"
    member val minimumInnerSpacing: SimplePropKey<Int32> = SimplePropKey.create "slider.minimumInnerSpacing"
    member val options: SimplePropKey<SliderOption<'a> list> = SimplePropKey.create "slider.options"
    member val orientation: SimplePropKey<Orientation> = SimplePropKey.create "slider.orientation"
    member val rangeAllowSingle: SimplePropKey<bool> = SimplePropKey.create "slider.rangeAllowSingle"
    member val showEndSpacing: SimplePropKey<bool> = SimplePropKey.create "slider.showEndSpacing"
    member val showLegends: SimplePropKey<bool> = SimplePropKey.create "slider.showLegends"
    member val style: SimplePropKey<SliderStyle> = SimplePropKey.create "slider.style"
    member val text: SimplePropKey<string> = SimplePropKey.create "slider.text"
    member val ``type``: SimplePropKey<SliderType> = SimplePropKey.create "slider.``type``"
    member val useMinimumSize: SimplePropKey<bool> = SimplePropKey.create "slider.useMinimumSize"
    // Events
    member val optionFocused: SimplePropKey<SliderEventArgs<'a> -> unit> = SimplePropKey.create "slider.optionFocused"
    member val optionsChanged: SimplePropKey<SliderEventArgs<'a> -> unit> = SimplePropKey.create "slider.optionsChanged"
    member val orientationChanged: SimplePropKey<Orientation -> unit> = SimplePropKey.create "slider.orientationChanged"
    member val orientationChanging: SimplePropKey<App.CancelEventArgs<Orientation> -> unit> = SimplePropKey.create "slider.orientationChanging"

  // Slider
  // type sliderPKeys() =
  //     inherit sliderPKeys<obj>()
  // No properties or events Slider

  // SpinnerView
  type spinnerViewPKeys() =
    inherit viewPKeys()

    // Properties
    member val autoSpin: SimplePropKey<bool> = SimplePropKey.create "spinnerView.autoSpin"
    member val sequence: SimplePropKey<string list> = SimplePropKey.create "spinnerView.sequence"
    member val spinBounce: SimplePropKey<bool> = SimplePropKey.create "spinnerView.spinBounce"
    member val spinDelay: SimplePropKey<Int32> = SimplePropKey.create "spinnerView.spinDelay"
    member val spinReverse: SimplePropKey<bool> = SimplePropKey.create "spinnerView.spinReverse"
    member val style: SimplePropKey<SpinnerStyle> = SimplePropKey.create "spinnerView.style"

  // StatusBar
  type statusBarPKeys() =
    inherit barPKeys()
  // No properties or events StatusBar

  // Tab
  type tabPKeys() =
    inherit viewPKeys()

    // Properties
    member val displayText: SimplePropKey<string> = SimplePropKey.create "tab.displayText"
    member val view: ViewPropKey<View> = ViewPropKey.create "tab.view_view"
    member val view_element: SingleElementPropKey<ITerminalElement> = SingleElementPropKey.create "tab.view_element"

  // TabView
  type tabViewPKeys() =
    inherit viewPKeys()

    // Properties
    member val maxTabTextWidth: SimplePropKey<int> = SimplePropKey.create "tabView.maxTabTextWidth"
    member val selectedTab: SimplePropKey<Tab> = SimplePropKey.create "tabView.selectedTab"
    member val style: SimplePropKey<TabStyle> = SimplePropKey.create "tabView.style"
    member val tabScrollOffset: SimplePropKey<Int32> = SimplePropKey.create "tabView.tabScrollOffset"
    // Events
    member val selectedTabChanged: SimplePropKey<TabChangedEventArgs -> unit> = SimplePropKey.create "tabView.selectedTabChanged"
    member val tabClicked: SimplePropKey<TabMouseEventArgs -> unit> = SimplePropKey.create "tabView.tabClicked"
    // Additional properties
    member val tabs: ViewPropKey<List<ITerminalElement>> = ViewPropKey.create "tabView.tabs_view"
    member val tabs_elements: MultiElementPropKey<List<ITerminalElement>> = MultiElementPropKey.create "tabView.tabs_elements"

  // TableView
  type tableViewPKeys() =
    inherit viewPKeys()

    // Properties
    member val cellActivationKey: SimplePropKey<KeyCode> = SimplePropKey.create "tableView.cellActivationKey"
    member val collectionNavigator: SimplePropKey<ICollectionNavigator> = SimplePropKey.create "tableView.collectionNavigator"
    member val columnOffset: SimplePropKey<Int32> = SimplePropKey.create "tableView.columnOffset"
    member val fullRowSelect: SimplePropKey<bool> = SimplePropKey.create "tableView.fullRowSelect"
    member val maxCellWidth: SimplePropKey<Int32> = SimplePropKey.create "tableView.maxCellWidth"
    member val minCellWidth: SimplePropKey<Int32> = SimplePropKey.create "tableView.minCellWidth"
    member val multiSelect: SimplePropKey<bool> = SimplePropKey.create "tableView.multiSelect"
    member val nullSymbol: SimplePropKey<string> = SimplePropKey.create "tableView.nullSymbol"
    member val rowOffset: SimplePropKey<Int32> = SimplePropKey.create "tableView.rowOffset"
    member val selectedColumn: SimplePropKey<Int32> = SimplePropKey.create "tableView.selectedColumn"
    member val selectedRow: SimplePropKey<Int32> = SimplePropKey.create "tableView.selectedRow"
    member val separatorSymbol: SimplePropKey<Char> = SimplePropKey.create "tableView.separatorSymbol"
    member val style: SimplePropKey<TableStyle> = SimplePropKey.create "tableView.style"
    member val table: SimplePropKey<ITableSource> = SimplePropKey.create "tableView.table"
    // Events
    member val cellActivated: SimplePropKey<CellActivatedEventArgs -> unit> = SimplePropKey.create "tableView.cellActivated"
    member val cellToggled: SimplePropKey<CellToggledEventArgs -> unit> = SimplePropKey.create "tableView.cellToggled"
    member val selectedCellChanged: SimplePropKey<SelectedCellChangedEventArgs -> unit> = SimplePropKey.create "tableView.selectedCellChanged"

  // TextValidateField
  type textValidateFieldPKeys() =
    inherit viewPKeys()

    // Properties
    member val provider: SimplePropKey<ITextValidateProvider> = SimplePropKey.create "textValidateField.provider"
    member val text: SimplePropKey<string> = SimplePropKey.create "textValidateField.text"

  // TextView
  type textViewPKeys() =
    inherit viewPKeys()

    // Properties
    member val allowsReturn: SimplePropKey<bool> = SimplePropKey.create "textView.allowsReturn"
    member val allowsTab: SimplePropKey<bool> = SimplePropKey.create "textView.allowsTab"
    member val cursorPosition: SimplePropKey<Point> = SimplePropKey.create "textView.cursorPosition"
    member val inheritsPreviousAttribute: SimplePropKey<bool> = SimplePropKey.create "textView.inheritsPreviousAttribute"
    member val isDirty: SimplePropKey<bool> = SimplePropKey.create "textView.isDirty"
    member val isSelecting: SimplePropKey<bool> = SimplePropKey.create "textView.isSelecting"
    member val leftColumn: SimplePropKey<Int32> = SimplePropKey.create "textView.leftColumn"
    member val multiline: SimplePropKey<bool> = SimplePropKey.create "textView.multiline"
    member val readOnly: SimplePropKey<bool> = SimplePropKey.create "textView.readOnly"
    member val selectionStartColumn: SimplePropKey<Int32> = SimplePropKey.create "textView.selectionStartColumn"
    member val selectionStartRow: SimplePropKey<Int32> = SimplePropKey.create "textView.selectionStartRow"
    member val selectWordOnlyOnDoubleClick: SimplePropKey<bool> = SimplePropKey.create "textView.selectWordOnlyOnDoubleClick"
    member val tabWidth: SimplePropKey<Int32> = SimplePropKey.create "textView.tabWidth"
    member val text: SimplePropKey<string> = SimplePropKey.create "textView.text"
    member val topRow: SimplePropKey<Int32> = SimplePropKey.create "textView.topRow"
    member val used: SimplePropKey<bool> = SimplePropKey.create "textView.used"
    member val useSameRuneTypeForWords: SimplePropKey<bool> = SimplePropKey.create "textView.useSameRuneTypeForWords"
    member val wordWrap: SimplePropKey<bool> = SimplePropKey.create "textView.wordWrap"
    // Events
    member val contentsChanged: SimplePropKey<ContentsChangedEventArgs -> unit> = SimplePropKey.create "textView.contentsChanged"
    member val drawNormalColor: SimplePropKey<CellEventArgs -> unit> = SimplePropKey.create "textView.drawNormalColor"
    member val drawReadOnlyColor: SimplePropKey<CellEventArgs -> unit> = SimplePropKey.create "textView.drawReadOnlyColor"
    member val drawSelectionColor: SimplePropKey<CellEventArgs -> unit> = SimplePropKey.create "textView.drawSelectionColor"
    member val drawUsedColor: SimplePropKey<CellEventArgs -> unit> = SimplePropKey.create "textView.drawUsedColor"
    member val unwrappedCursorPosition: SimplePropKey<Point -> unit> = SimplePropKey.create "textView.unwrappedCursorPosition"
    // Additional properties
    member val textChanged: SimplePropKey<string -> unit> = SimplePropKey.create "textView.textChanged"

  // TimeField
  type timeFieldPKeys() =
    inherit textFieldPKeys()

    // Properties
    member val cursorPosition: SimplePropKey<Int32> = SimplePropKey.create "timeField.cursorPosition"
    member val isShortFormat: SimplePropKey<bool> = SimplePropKey.create "timeField.isShortFormat"
    member val time: SimplePropKey<TimeSpan> = SimplePropKey.create "timeField.time"
    // Events
    member val timeChanged: SimplePropKey<DateTimeEventArgs<TimeSpan> -> unit> = SimplePropKey.create "timeField.timeChanged"

  // TreeView`1
  type treeViewPKeys<'a when 'a: not struct>() =
    inherit viewPKeys()

    // Properties
    member val allowLetterBasedNavigation: SimplePropKey<bool> = SimplePropKey.create "treeView.allowLetterBasedNavigation"
    member val aspectGetter: SimplePropKey<AspectGetterDelegate<'a>> = SimplePropKey.create "treeView.aspectGetter"
    member val colorGetter: SimplePropKey<Func<'a, Scheme>> = SimplePropKey.create "treeView.colorGetter"
    member val maxDepth: SimplePropKey<Int32> = SimplePropKey.create "treeView.maxDepth"
    member val multiSelect: SimplePropKey<bool> = SimplePropKey.create "treeView.multiSelect"
    member val objectActivationButton: SimplePropKey<MouseFlags option> = SimplePropKey.create "treeView.objectActivationButton"
    member val objectActivationKey: SimplePropKey<KeyCode> = SimplePropKey.create "treeView.objectActivationKey"
    member val scrollOffsetHorizontal: SimplePropKey<Int32> = SimplePropKey.create "treeView.scrollOffsetHorizontal"
    member val scrollOffsetVertical: SimplePropKey<Int32> = SimplePropKey.create "treeView.scrollOffsetVertical"
    member val selectedObject: SimplePropKey<'a> = SimplePropKey.create "treeView.selectedObject"
    member val style: SimplePropKey<TreeStyle> = SimplePropKey.create "treeView.style"
    member val treeBuilder: SimplePropKey<ITreeBuilder<'a>> = SimplePropKey.create "treeView.treeBuilder"
    // Events
    member val drawLine: SimplePropKey<DrawTreeViewLineEventArgs<'a> -> unit> = SimplePropKey.create "treeView.drawLine"
    member val objectActivated: SimplePropKey<ObjectActivatedEventArgs<'a> -> unit> = SimplePropKey.create "treeView.objectActivated"
    member val selectionChanged: SimplePropKey<SelectionChangedEventArgs<'a> -> unit> = SimplePropKey.create "treeView.selectionChanged"

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
    member val currentStep: SimplePropKey<WizardStep> = SimplePropKey.create "wizard.currentStep"
    member val modal: SimplePropKey<bool> = SimplePropKey.create "wizard.modal"
    // Events
    member val cancelled: SimplePropKey<WizardButtonEventArgs -> unit> = SimplePropKey.create "wizard.cancelled"
    member val finished: SimplePropKey<WizardButtonEventArgs -> unit> = SimplePropKey.create "wizard.finished"
    member val movingBack: SimplePropKey<WizardButtonEventArgs -> unit> = SimplePropKey.create "wizard.movingBack"
    member val movingNext: SimplePropKey<WizardButtonEventArgs -> unit> = SimplePropKey.create "wizard.movingNext"
    member val stepChanged: SimplePropKey<StepChangeEventArgs -> unit> = SimplePropKey.create "wizard.stepChanged"
    member val stepChanging: SimplePropKey<StepChangeEventArgs -> unit> = SimplePropKey.create "wizard.stepChanging"

  // WizardStep
  type wizardStepPKeys() =
    inherit viewPKeys()

    // Properties
    member val backButtonText: SimplePropKey<string> = SimplePropKey.create "wizardStep.backButtonText"
    member val helpText: SimplePropKey<string> = SimplePropKey.create "wizardStep.helpText"
    member val nextButtonText: SimplePropKey<string> = SimplePropKey.create "wizardStep.nextButtonText"


  let view = viewPKeys ()
  let adornment = adornmentPKeys ()
  let bar = barPKeys ()
  let border = borderPKeys ()
  let button = buttonPKeys ()
  let checkBox = checkBoxPKeys ()
  let colorPicker = colorPickerPKeys ()
  let colorPicker16 = colorPicker16PKeys ()
  let comboBox = comboBoxPKeys ()
  let textField = textFieldPKeys ()
  let dateField = dateFieldPKeys ()
  let datePicker = datePickerPKeys ()
  let toplevel = toplevelPKeys ()
  let dialog = dialogPKeys ()
  let fileDialog = fileDialogPKeys ()
  let saveDialog = saveDialogPKeys ()
  let frameView = frameViewPKeys ()
  let graphView = graphViewPKeys ()
  let hexView = hexViewPKeys ()
  let label = labelPKeys ()

  let legendAnnotation =
    legendAnnotationPKeys ()

  let line = linePKeys ()
  let listView = listViewPKeys ()
  let margin = marginPKeys ()
  let menuv2 = menuv2PKeys ()
  let menuBarv2 = menuBarv2PKeys ()
  let shortcut = shortcutPKeys ()
  let menuItemv2 = menuItemv2PKeys ()
  let menuBarItemv2 = menuBarItemv2PKeys ()
  let popoverMenu = popoverMenuPKeys ()

  let numericUpDown<'a> =
    numericUpDownPKeys<'a> ()

  let openDialog = openDialogPKeys ()
  let selectorBase = selectorBasePKeys ()
  let optionSelector = optionSelectorPKeys ()
  let flagSelector = flagSelectorPKeys ()
  let padding = paddingPKeys ()
  let progressBar = progressBarPKeys ()
  let scrollBar = scrollBarPKeys ()
  let scrollSlider = scrollSliderPKeys ()
  let slider<'a> = sliderPKeys<'a> ()
  let spinnerView = spinnerViewPKeys ()
  let statusBar = statusBarPKeys ()
  let tab = tabPKeys ()
  let tabView = tabViewPKeys ()
  let tableView = tableViewPKeys ()

  let textValidateField =
    textValidateFieldPKeys ()

  let textView = textViewPKeys ()
  let timeField = timeFieldPKeys ()

  let treeView<'a when 'a: not struct> =
    treeViewPKeys<'a> ()

  let window = windowPKeys ()
  let wizard = wizardPKeys ()
  let wizardStep = wizardStepPKeys ()

  // IOrientation
  module internal orientationInterface =
    // Properties
    let orientation : SimplePropKey<Orientation> = SimplePropKey.create "orientation.orientation"
    // Events
    let orientationChanged: SimplePropKey<Orientation -> unit> = SimplePropKey.create "orientation.orientationChanged"
    let orientationChanging: SimplePropKey<CancelEventArgs<Orientation> -> unit> = SimplePropKey.create "orientation.orientationChanging"
