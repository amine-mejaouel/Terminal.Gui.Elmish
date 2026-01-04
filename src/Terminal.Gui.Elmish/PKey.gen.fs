namespace Terminal.Gui.Elmish

open System
open System.Collections.Generic
open System.Collections.Specialized
open System.Text
open System.Drawing
open System.ComponentModel
open System.IO
open System.Globalization
open Terminal.Gui.App
open Terminal.Gui.Drawing
open Terminal.Gui.Drivers

open Terminal.Gui.FileServices
open Terminal.Gui.Input

open Terminal.Gui.Text

open Terminal.Gui.ViewBase

open Terminal.Gui.Views

/// Properties key index
[<RequireQualifiedAccess>]
module internal PKey =

  // View
  type viewPKeys() =
    member val children: ISimplePropKey<System.Collections.Generic.List<IInternalTerminalElement>> = PropKey.Create.simple "children"

    // Properties
    member val arrangement: ISimplePropKey<ViewArrangement> = PropKey.Create.simple "view.arrangement"
    member val borderStyle: ISimplePropKey<LineStyle> = PropKey.Create.simple "view.borderStyle"
    member val canFocus: ISimplePropKey<bool> = PropKey.Create.simple "view.canFocus"
    member val contentSizeTracksViewport: ISimplePropKey<bool> = PropKey.Create.simple "view.contentSizeTracksViewport"
    member val cursorVisibility: ISimplePropKey<CursorVisibility> = PropKey.Create.simple "view.cursorVisibility"
    member val data: ISimplePropKey<Object> = PropKey.Create.simple "view.data"
    member val enabled: ISimplePropKey<bool> = PropKey.Create.simple "view.enabled"
    member val frame: ISimplePropKey<Rectangle> = PropKey.Create.simple "view.frame"
    member val hasFocus: ISimplePropKey<bool> = PropKey.Create.simple "view.hasFocus"
    member val height: ISimplePropKey<Dim> = PropKey.Create.simple "view.height"
    member val highlightStates: ISimplePropKey<MouseState> = PropKey.Create.simple "view.highlightStates"
    member val hotKey: ISimplePropKey<Key> = PropKey.Create.simple "view.hotKey"
    member val hotKeySpecifier: ISimplePropKey<Rune> = PropKey.Create.simple "view.hotKeySpecifier"
    member val id: ISimplePropKey<string> = PropKey.Create.simple "view.id"
    member val isInitialized: ISimplePropKey<bool> = PropKey.Create.simple "view.isInitialized"
    member val mouseHeldDown: ISimplePropKey<IMouseHeldDown> = PropKey.Create.simple "view.mouseHeldDown"
    member val preserveTrailingSpaces: ISimplePropKey<bool> = PropKey.Create.simple "view.preserveTrailingSpaces"
    member val schemeName: ISimplePropKey<string> = PropKey.Create.simple "view.schemeName"
    member val shadowStyle: ISimplePropKey<ShadowStyle> = PropKey.Create.simple "view.shadowStyle"
    member val superViewRendersLineCanvas: ISimplePropKey<bool> = PropKey.Create.simple "view.superViewRendersLineCanvas"
    member val tabStop: ISimplePropKey<Nullable<TabBehavior>> = PropKey.Create.simple "view.tabStop"
    member val text: ISimplePropKey<string> = PropKey.Create.simple "view.text"
    member val textAlignment: ISimplePropKey<Alignment> = PropKey.Create.simple "view.textAlignment"
    member val textDirection: ISimplePropKey<TextDirection> = PropKey.Create.simple "view.textDirection"
    member val title: ISimplePropKey<string> = PropKey.Create.simple "view.title"
    member val validatePosDim: ISimplePropKey<bool> = PropKey.Create.simple "view.validatePosDim"
    member val verticalTextAlignment: ISimplePropKey<Alignment> = PropKey.Create.simple "view.verticalTextAlignment"
    member val viewport: ISimplePropKey<Rectangle> = PropKey.Create.simple "view.viewport"
    member val viewportSettings: ISimplePropKey<ViewportSettingsFlags> = PropKey.Create.simple "view.viewportSettings"
    member val visible: ISimplePropKey<bool> = PropKey.Create.simple "view.visible"
    member val wantContinuousButtonPressed: ISimplePropKey<bool> = PropKey.Create.simple "view.wantContinuousButtonPressed"
    member val wantMousePositionReports: ISimplePropKey<bool> = PropKey.Create.simple "view.wantMousePositionReports"
    member val width: ISimplePropKey<Dim> = PropKey.Create.simple "view.width"
    member val x: ISimplePropKey<Pos> = PropKey.Create.simple "view.x"
    member val x_delayedPos: IDelayedPosKey = PropKey.Create.delayedPos "view.x_delayedPos"
    member val y: ISimplePropKey<Pos> = PropKey.Create.simple "view.y"
    member val y_delayedPos: IDelayedPosKey = PropKey.Create.delayedPos "view.y_delayedPos"

    // Events
    member val accepted: IEventPropKey<CommandEventArgs -> unit> = PropKey.Create.event "view.accepted_event"
    member val accepting: IEventPropKey<CommandEventArgs -> unit> = PropKey.Create.event "view.accepting_event"
    member val activating: IEventPropKey<CommandEventArgs -> unit> = PropKey.Create.event "view.activating_event"
    member val advancingFocus: IEventPropKey<AdvanceFocusEventArgs -> unit> = PropKey.Create.event "view.advancingFocus_event"
    member val borderStyleChanged: IEventPropKey<EventArgs -> unit> = PropKey.Create.event "view.borderStyleChanged_event"
    member val canFocusChanged: IEventPropKey<EventArgs -> unit> = PropKey.Create.event "view.canFocusChanged_event"
    member val clearedViewport: IEventPropKey<DrawEventArgs -> unit> = PropKey.Create.event "view.clearedViewport_event"
    member val clearingViewport: IEventPropKey<DrawEventArgs -> unit> = PropKey.Create.event "view.clearingViewport_event"
    member val commandNotBound: IEventPropKey<CommandEventArgs -> unit> = PropKey.Create.event "view.commandNotBound_event"
    member val contentSizeChanged: IEventPropKey<SizeChangedEventArgs -> unit> = PropKey.Create.event "view.contentSizeChanged_event"
    member val disposing: IEventPropKey<EventArgs -> unit> = PropKey.Create.event "view.disposing_event"
    member val drawComplete: IEventPropKey<DrawEventArgs -> unit> = PropKey.Create.event "view.drawComplete_event"
    member val drawingContent: IEventPropKey<DrawEventArgs -> unit> = PropKey.Create.event "view.drawingContent_event"
    member val drawingSubViews: IEventPropKey<DrawEventArgs -> unit> = PropKey.Create.event "view.drawingSubViews_event"
    member val drawingText: IEventPropKey<DrawEventArgs -> unit> = PropKey.Create.event "view.drawingText_event"
    member val drewText: IEventPropKey<EventArgs -> unit> = PropKey.Create.event "view.drewText_event"
    member val enabledChanged: IEventPropKey<EventArgs -> unit> = PropKey.Create.event "view.enabledChanged_event"
    member val focusedChanged: IEventPropKey<HasFocusEventArgs -> unit> = PropKey.Create.event "view.focusedChanged_event"
    member val frameChanged: IEventPropKey<EventArgs<Rectangle> -> unit> = PropKey.Create.event "view.frameChanged_event"
    member val gettingAttributeForRole: IEventPropKey<VisualRoleEventArgs -> unit> = PropKey.Create.event "view.gettingAttributeForRole_event"
    member val gettingScheme: IEventPropKey<ResultEventArgs<Scheme> -> unit> = PropKey.Create.event "view.gettingScheme_event"
    member val handlingHotKey: IEventPropKey<CommandEventArgs -> unit> = PropKey.Create.event "view.handlingHotKey_event"
    member val hasFocusChanged: IEventPropKey<HasFocusEventArgs -> unit> = PropKey.Create.event "view.hasFocusChanged_event"
    member val hasFocusChanging: IEventPropKey<HasFocusEventArgs -> unit> = PropKey.Create.event "view.hasFocusChanging_event"
    member val heightChanged: IEventPropKey<ValueChangedEventArgs<Dim> -> unit> = PropKey.Create.event "view.heightChanged_event"
    member val heightChanging: IEventPropKey<ValueChangingEventArgs<Dim> -> unit> = PropKey.Create.event "view.heightChanging_event"
    member val hotKeyChanged: IEventPropKey<KeyChangedEventArgs -> unit> = PropKey.Create.event "view.hotKeyChanged_event"
    member val initialized: IEventPropKey<EventArgs -> unit> = PropKey.Create.event "view.initialized_event"
    member val keyDown: IEventPropKey<Key -> unit> = PropKey.Create.event "view.keyDown_event"
    member val keyDownNotHandled: IEventPropKey<Key -> unit> = PropKey.Create.event "view.keyDownNotHandled_event"
    member val keyUp: IEventPropKey<Key -> unit> = PropKey.Create.event "view.keyUp_event"
    member val mouseEnter: IEventPropKey<CancelEventArgs -> unit> = PropKey.Create.event "view.mouseEnter_event"
    member val mouseEvent: IEventPropKey<MouseEventArgs -> unit> = PropKey.Create.event "view.mouseEvent_event"
    member val mouseLeave: IEventPropKey<EventArgs -> unit> = PropKey.Create.event "view.mouseLeave_event"
    member val mouseStateChanged: IEventPropKey<EventArgs<MouseState> -> unit> = PropKey.Create.event "view.mouseStateChanged_event"
    member val mouseWheel: IEventPropKey<MouseEventArgs -> unit> = PropKey.Create.event "view.mouseWheel_event"
    member val removed: IEventPropKey<SuperViewChangedEventArgs -> unit> = PropKey.Create.event "view.removed_event"
    member val schemeChanged: IEventPropKey<ValueChangedEventArgs<Scheme> -> unit> = PropKey.Create.event "view.schemeChanged_event"
    member val schemeChanging: IEventPropKey<ValueChangingEventArgs<Scheme> -> unit> = PropKey.Create.event "view.schemeChanging_event"
    member val schemeNameChanged: IEventPropKey<ValueChangedEventArgs<string> -> unit> = PropKey.Create.event "view.schemeNameChanged_event"
    member val schemeNameChanging: IEventPropKey<ValueChangingEventArgs<string> -> unit> = PropKey.Create.event "view.schemeNameChanging_event"
    member val subViewAdded: IEventPropKey<SuperViewChangedEventArgs -> unit> = PropKey.Create.event "view.subViewAdded_event"
    member val subViewLayout: IEventPropKey<LayoutEventArgs -> unit> = PropKey.Create.event "view.subViewLayout_event"
    member val subViewRemoved: IEventPropKey<SuperViewChangedEventArgs -> unit> = PropKey.Create.event "view.subViewRemoved_event"
    member val subViewsLaidOut: IEventPropKey<LayoutEventArgs -> unit> = PropKey.Create.event "view.subViewsLaidOut_event"
    member val superViewChanged: IEventPropKey<SuperViewChangedEventArgs -> unit> = PropKey.Create.event "view.superViewChanged_event"
    member val textChanged: IEventPropKey<EventArgs -> unit> = PropKey.Create.event "view.textChanged_event"
    member val titleChanged: IEventPropKey<EventArgs<string> -> unit> = PropKey.Create.event "view.titleChanged_event"
    member val titleChanging: IEventPropKey<CancelEventArgs<string> -> unit> = PropKey.Create.event "view.titleChanging_event"
    member val viewportChanged: IEventPropKey<DrawEventArgs -> unit> = PropKey.Create.event "view.viewportChanged_event"
    member val visibleChanged: IEventPropKey<EventArgs -> unit> = PropKey.Create.event "view.visibleChanged_event"
    member val visibleChanging: IEventPropKey<CancelEventArgs<bool> -> unit> = PropKey.Create.event "view.visibleChanging_event"
    member val widthChanged: IEventPropKey<ValueChangedEventArgs<Dim> -> unit> = PropKey.Create.event "view.widthChanged_event"
    member val widthChanging: IEventPropKey<ValueChangingEventArgs<Dim> -> unit> = PropKey.Create.event "view.widthChanging_event"

  // Adornment
  type adornmentPKeys() =
    inherit viewPKeys()
    // Properties
    member val diagnostics: ISimplePropKey<ViewDiagnosticFlags> = PropKey.Create.simple "adornment.diagnostics"
    member val parent: ISimplePropKey<View> = PropKey.Create.simple "adornment.parent"
    member val superViewRendersLineCanvas: ISimplePropKey<bool> = PropKey.Create.simple "adornment.superViewRendersLineCanvas"
    member val thickness: ISimplePropKey<Thickness> = PropKey.Create.simple "adornment.thickness"
    member val viewport: ISimplePropKey<Rectangle> = PropKey.Create.simple "adornment.viewport"

    // Events
    member val thicknessChanged: IEventPropKey<EventArgs -> unit> = PropKey.Create.event "adornment.thicknessChanged_event"

  // Bar
  type barPKeys() =
    inherit viewPKeys()
    // Properties
    member val alignmentModes: ISimplePropKey<AlignmentModes> = PropKey.Create.simple "bar.alignmentModes"
    member val orientation: ISimplePropKey<Orientation> = PropKey.Create.simple "bar.orientation"

    // Events
    member val orientationChanged: IEventPropKey<EventArgs<Orientation> -> unit> = PropKey.Create.event "bar.orientationChanged_event"
    member val orientationChanging: IEventPropKey<CancelEventArgs<Orientation> -> unit> = PropKey.Create.event "bar.orientationChanging_event"

  // Border
  type borderPKeys() =
    inherit adornmentPKeys()
    // Properties
    member val lineStyle: ISimplePropKey<LineStyle> = PropKey.Create.simple "border.lineStyle"
    member val settings: ISimplePropKey<BorderSettings> = PropKey.Create.simple "border.settings"

  // Button
  type buttonPKeys() =
    inherit viewPKeys()
    // Properties
    member val hotKeySpecifier: ISimplePropKey<Rune> = PropKey.Create.simple "button.hotKeySpecifier"
    member val isDefault: ISimplePropKey<bool> = PropKey.Create.simple "button.isDefault"
    member val noDecorations: ISimplePropKey<bool> = PropKey.Create.simple "button.noDecorations"
    member val noPadding: ISimplePropKey<bool> = PropKey.Create.simple "button.noPadding"
    member val text: ISimplePropKey<string> = PropKey.Create.simple "button.text"

  // CharMap
  type charMapPKeys() =
    inherit viewPKeys()
    // Properties
    member val selectedCodePoint: ISimplePropKey<int> = PropKey.Create.simple "charMap.selectedCodePoint"
    member val showGlyphWidths: ISimplePropKey<bool> = PropKey.Create.simple "charMap.showGlyphWidths"
    member val showUnicodeCategory: ISimplePropKey<Nullable<UnicodeCategory>> = PropKey.Create.simple "charMap.showUnicodeCategory"
    member val startCodePoint: ISimplePropKey<int> = PropKey.Create.simple "charMap.startCodePoint"

    // Events
    member val selectedCodePointChanged: IEventPropKey<EventArgs<int> -> unit> = PropKey.Create.event "charMap.selectedCodePointChanged_event"

  // CheckBox
  type checkBoxPKeys() =
    inherit viewPKeys()
    // Properties
    member val allowCheckStateNone: ISimplePropKey<bool> = PropKey.Create.simple "checkBox.allowCheckStateNone"
    member val checkedState: ISimplePropKey<CheckState> = PropKey.Create.simple "checkBox.checkedState"
    member val hotKeySpecifier: ISimplePropKey<Rune> = PropKey.Create.simple "checkBox.hotKeySpecifier"
    member val radioStyle: ISimplePropKey<bool> = PropKey.Create.simple "checkBox.radioStyle"
    member val text: ISimplePropKey<string> = PropKey.Create.simple "checkBox.text"

    // Events
    member val checkedStateChanged: IEventPropKey<EventArgs<CheckState> -> unit> = PropKey.Create.event "checkBox.checkedStateChanged_event"
    member val checkedStateChanging: IEventPropKey<ResultEventArgs<CheckState> -> unit> = PropKey.Create.event "checkBox.checkedStateChanging_event"

  // ColorPicker
  type colorPickerPKeys() =
    inherit viewPKeys()
    // Properties
    member val selectedColor: ISimplePropKey<Color> = PropKey.Create.simple "colorPicker.selectedColor"
    member val style: ISimplePropKey<ColorPickerStyle> = PropKey.Create.simple "colorPicker.style"

    // Events
    member val colorChanged: IEventPropKey<ResultEventArgs<Color> -> unit> = PropKey.Create.event "colorPicker.colorChanged_event"

  // ColorPicker16
  type colorPicker16PKeys() =
    inherit viewPKeys()
    // Properties
    member val boxHeight: ISimplePropKey<int> = PropKey.Create.simple "colorPicker16.boxHeight"
    member val boxWidth: ISimplePropKey<int> = PropKey.Create.simple "colorPicker16.boxWidth"
    member val cursor: ISimplePropKey<Point> = PropKey.Create.simple "colorPicker16.cursor"
    member val selectedColor: ISimplePropKey<ColorName16> = PropKey.Create.simple "colorPicker16.selectedColor"

    // Events
    member val colorChanged: IEventPropKey<ResultEventArgs<Color> -> unit> = PropKey.Create.event "colorPicker16.colorChanged_event"

  // ComboBox
  type comboBoxPKeys() =
    inherit viewPKeys()
    // Properties
    member val hideDropdownListOnClick: ISimplePropKey<bool> = PropKey.Create.simple "comboBox.hideDropdownListOnClick"
    member val readOnly: ISimplePropKey<bool> = PropKey.Create.simple "comboBox.readOnly"
    member val searchText: ISimplePropKey<string> = PropKey.Create.simple "comboBox.searchText"
    member val selectedItem: ISimplePropKey<int> = PropKey.Create.simple "comboBox.selectedItem"
    member val source: ISimplePropKey<IListDataSource> = PropKey.Create.simple "comboBox.source"
    member val text: ISimplePropKey<string> = PropKey.Create.simple "comboBox.text"

    // Events
    member val collapsed: IEventPropKey<EventArgs -> unit> = PropKey.Create.event "comboBox.collapsed_event"
    member val expanded: IEventPropKey<EventArgs -> unit> = PropKey.Create.event "comboBox.expanded_event"
    member val openSelectedItem: IEventPropKey<ListViewItemEventArgs -> unit> = PropKey.Create.event "comboBox.openSelectedItem_event"
    member val selectedItemChanged: IEventPropKey<ListViewItemEventArgs -> unit> = PropKey.Create.event "comboBox.selectedItemChanged_event"

  // DatePicker
  type datePickerPKeys() =
    inherit viewPKeys()
    // Properties
    member val culture: ISimplePropKey<CultureInfo> = PropKey.Create.simple "datePicker.culture"
    member val date: ISimplePropKey<DateTime> = PropKey.Create.simple "datePicker.date"

  // FrameView
  type frameViewPKeys() =
    inherit viewPKeys()

  // GraphView
  type graphViewPKeys() =
    inherit viewPKeys()
    // Properties
    member val axisX: ISimplePropKey<HorizontalAxis> = PropKey.Create.simple "graphView.axisX"
    member val axisY: ISimplePropKey<VerticalAxis> = PropKey.Create.simple "graphView.axisY"
    member val cellSize: ISimplePropKey<PointF> = PropKey.Create.simple "graphView.cellSize"
    member val graphColor: ISimplePropKey<Nullable<Attribute>> = PropKey.Create.simple "graphView.graphColor"
    member val marginBottom: ISimplePropKey<UInt32> = PropKey.Create.simple "graphView.marginBottom"
    member val marginLeft: ISimplePropKey<UInt32> = PropKey.Create.simple "graphView.marginLeft"
    member val scrollOffset: ISimplePropKey<PointF> = PropKey.Create.simple "graphView.scrollOffset"

  // HexView
  type hexViewPKeys() =
    inherit viewPKeys()
    // Properties
    member val address: ISimplePropKey<Int64> = PropKey.Create.simple "hexView.address"
    member val addressWidth: ISimplePropKey<int> = PropKey.Create.simple "hexView.addressWidth"
    member val bytesPerLine: ISimplePropKey<int> = PropKey.Create.simple "hexView.bytesPerLine"
    member val readOnly: ISimplePropKey<bool> = PropKey.Create.simple "hexView.readOnly"
    member val source: ISimplePropKey<Stream> = PropKey.Create.simple "hexView.source"

    // Events
    member val edited: IEventPropKey<HexViewEditEventArgs -> unit> = PropKey.Create.event "hexView.edited_event"
    member val positionChanged: IEventPropKey<HexViewEventArgs -> unit> = PropKey.Create.event "hexView.positionChanged_event"

  // Label
  type labelPKeys() =
    inherit viewPKeys()
    // Properties
    member val hotKeySpecifier: ISimplePropKey<Rune> = PropKey.Create.simple "label.hotKeySpecifier"
    member val text: ISimplePropKey<string> = PropKey.Create.simple "label.text"

  // LegendAnnotation
  type legendAnnotationPKeys() =
    inherit viewPKeys()

  // Line
  type linePKeys() =
    inherit viewPKeys()
    // Properties
    member val length: ISimplePropKey<Dim> = PropKey.Create.simple "line.length"
    member val orientation: ISimplePropKey<Orientation> = PropKey.Create.simple "line.orientation"
    member val style: ISimplePropKey<LineStyle> = PropKey.Create.simple "line.style"

    // Events
    member val orientationChanged: IEventPropKey<EventArgs<Orientation> -> unit> = PropKey.Create.event "line.orientationChanged_event"
    member val orientationChanging: IEventPropKey<CancelEventArgs<Orientation> -> unit> = PropKey.Create.event "line.orientationChanging_event"

  // ListView
  type listViewPKeys() =
    inherit viewPKeys()
    // Properties
    member val allowsMarking: ISimplePropKey<bool> = PropKey.Create.simple "listView.allowsMarking"
    member val allowsMultipleSelection: ISimplePropKey<bool> = PropKey.Create.simple "listView.allowsMultipleSelection"
    member val leftItem: ISimplePropKey<int> = PropKey.Create.simple "listView.leftItem"
    member val selectedItem: ISimplePropKey<Nullable<int>> = PropKey.Create.simple "listView.selectedItem"
    member val source: ISimplePropKey<IListDataSource> = PropKey.Create.simple "listView.source"
    member val topItem: ISimplePropKey<int> = PropKey.Create.simple "listView.topItem"

    // Events
    member val collectionChanged: IEventPropKey<NotifyCollectionChangedEventArgs -> unit> = PropKey.Create.event "listView.collectionChanged_event"
    member val openSelectedItem: IEventPropKey<ListViewItemEventArgs -> unit> = PropKey.Create.event "listView.openSelectedItem_event"
    member val rowRender: IEventPropKey<ListViewRowEventArgs -> unit> = PropKey.Create.event "listView.rowRender_event"
    member val selectedItemChanged: IEventPropKey<ListViewItemEventArgs -> unit> = PropKey.Create.event "listView.selectedItemChanged_event"

  // Margin
  type marginPKeys() =
    inherit adornmentPKeys()
    // Properties
    member val shadowStyle: ISimplePropKey<ShadowStyle> = PropKey.Create.simple "margin.shadowStyle"

  // Menu
  type menuPKeys() =
    inherit barPKeys()
    // Properties
    member val selectedMenuItem: ISimplePropKey<MenuItem> = PropKey.Create.simple "menu.selectedMenuItem"
    member val superMenuItem: ISimplePropKey<MenuItem> = PropKey.Create.simple "menu.superMenuItem"

    // Events
    member val selectedMenuItemChanged: IEventPropKey<MenuItem -> unit> = PropKey.Create.event "menu.selectedMenuItemChanged_event"

  // MenuBar
  type menuBarPKeys() =
    inherit menuPKeys()
    // Properties
    member val key: ISimplePropKey<Key> = PropKey.Create.simple "menuBar.key"

    // Events
    member val keyChanged: IEventPropKey<KeyChangedEventArgs -> unit> = PropKey.Create.event "menuBar.keyChanged_event"

  // NumericUpDown
  type numericUpDownPKeys<'T>() =
    inherit viewPKeys()
    // Properties
    member val format: ISimplePropKey<string> = PropKey.Create.simple "numericUpDown.format"
    member val increment: ISimplePropKey<'T> = PropKey.Create.simple "numericUpDown.increment"
    member val value: ISimplePropKey<'T> = PropKey.Create.simple "numericUpDown.value"

    // Events
    member val formatChanged: IEventPropKey<EventArgs<string> -> unit> = PropKey.Create.event "numericUpDown.formatChanged_event"
    member val incrementChanged: IEventPropKey<EventArgs<'T> -> unit> = PropKey.Create.event "numericUpDown.incrementChanged_event"
    member val valueChanged: IEventPropKey<EventArgs<'T> -> unit> = PropKey.Create.event "numericUpDown.valueChanged_event"
    member val valueChanging: IEventPropKey<CancelEventArgs<'T> -> unit> = PropKey.Create.event "numericUpDown.valueChanging_event"

  // Padding
  type paddingPKeys() =
    inherit adornmentPKeys()

  // PopoverBaseImpl
  type popoverBaseImplPKeys() =
    inherit viewPKeys()
    // Properties
    member val current: ISimplePropKey<IRunnable> = PropKey.Create.simple "popoverBaseImpl.current"

  // PopoverMenu
  type popoverMenuPKeys() =
    inherit popoverBaseImplPKeys()
    // Properties
    member val key: ISimplePropKey<Key> = PropKey.Create.simple "popoverMenu.key"
    member val mouseFlags: ISimplePropKey<MouseFlags> = PropKey.Create.simple "popoverMenu.mouseFlags"
    member val root: ISimplePropKey<Menu> = PropKey.Create.simple "popoverMenu.root"

    // Events
    member val keyChanged: IEventPropKey<KeyChangedEventArgs -> unit> = PropKey.Create.event "popoverMenu.keyChanged_event"

  // ProgressBar
  type progressBarPKeys() =
    inherit viewPKeys()
    // Properties
    member val bidirectionalMarquee: ISimplePropKey<bool> = PropKey.Create.simple "progressBar.bidirectionalMarquee"
    member val fraction: ISimplePropKey<Single> = PropKey.Create.simple "progressBar.fraction"
    member val progressBarFormat: ISimplePropKey<ProgressBarFormat> = PropKey.Create.simple "progressBar.progressBarFormat"
    member val progressBarStyle: ISimplePropKey<ProgressBarStyle> = PropKey.Create.simple "progressBar.progressBarStyle"
    member val segmentCharacter: ISimplePropKey<Rune> = PropKey.Create.simple "progressBar.segmentCharacter"
    member val text: ISimplePropKey<string> = PropKey.Create.simple "progressBar.text"

  // Runnable
  type runnablePKeys() =
    inherit viewPKeys()
    // Properties
    member val result: ISimplePropKey<Object> = PropKey.Create.simple "runnable.result"
    member val stopRequested: ISimplePropKey<bool> = PropKey.Create.simple "runnable.stopRequested"

    // Events
    member val isModalChanged: IEventPropKey<EventArgs<bool> -> unit> = PropKey.Create.event "runnable.isModalChanged_event"
    member val isRunningChanged: IEventPropKey<EventArgs<bool> -> unit> = PropKey.Create.event "runnable.isRunningChanged_event"
    member val isRunningChanging: IEventPropKey<CancelEventArgs<bool> -> unit> = PropKey.Create.event "runnable.isRunningChanging_event"

  // Runnable
  type runnablePKeys<'TResult>() =
    inherit runnablePKeys()
    // Properties
    member val result: ISimplePropKey<'TResult> = PropKey.Create.simple "runnable.result"

  // ScrollBar
  type scrollBarPKeys() =
    inherit viewPKeys()
    // Properties
    member val autoShow: ISimplePropKey<bool> = PropKey.Create.simple "scrollBar.autoShow"
    member val increment: ISimplePropKey<int> = PropKey.Create.simple "scrollBar.increment"
    member val orientation: ISimplePropKey<Orientation> = PropKey.Create.simple "scrollBar.orientation"
    member val position: ISimplePropKey<int> = PropKey.Create.simple "scrollBar.position"
    member val scrollableContentSize: ISimplePropKey<int> = PropKey.Create.simple "scrollBar.scrollableContentSize"
    member val visibleContentSize: ISimplePropKey<int> = PropKey.Create.simple "scrollBar.visibleContentSize"

    // Events
    member val orientationChanged: IEventPropKey<EventArgs<Orientation> -> unit> = PropKey.Create.event "scrollBar.orientationChanged_event"
    member val orientationChanging: IEventPropKey<CancelEventArgs<Orientation> -> unit> = PropKey.Create.event "scrollBar.orientationChanging_event"
    member val positionChanged: IEventPropKey<EventArgs<int> -> unit> = PropKey.Create.event "scrollBar.positionChanged_event"
    member val positionChanging: IEventPropKey<CancelEventArgs<int> -> unit> = PropKey.Create.event "scrollBar.positionChanging_event"
    member val scrollableContentSizeChanged: IEventPropKey<EventArgs<int> -> unit> = PropKey.Create.event "scrollBar.scrollableContentSizeChanged_event"
    member val scrolled: IEventPropKey<EventArgs<int> -> unit> = PropKey.Create.event "scrollBar.scrolled_event"
    member val sliderPositionChanged: IEventPropKey<EventArgs<int> -> unit> = PropKey.Create.event "scrollBar.sliderPositionChanged_event"

  // ScrollSlider
  type scrollSliderPKeys() =
    inherit viewPKeys()
    // Properties
    member val orientation: ISimplePropKey<Orientation> = PropKey.Create.simple "scrollSlider.orientation"
    member val position: ISimplePropKey<int> = PropKey.Create.simple "scrollSlider.position"
    member val size: ISimplePropKey<int> = PropKey.Create.simple "scrollSlider.size"
    member val sliderPadding: ISimplePropKey<int> = PropKey.Create.simple "scrollSlider.sliderPadding"
    member val visibleContentSize: ISimplePropKey<int> = PropKey.Create.simple "scrollSlider.visibleContentSize"

    // Events
    member val orientationChanged: IEventPropKey<EventArgs<Orientation> -> unit> = PropKey.Create.event "scrollSlider.orientationChanged_event"
    member val orientationChanging: IEventPropKey<CancelEventArgs<Orientation> -> unit> = PropKey.Create.event "scrollSlider.orientationChanging_event"
    member val positionChanged: IEventPropKey<EventArgs<int> -> unit> = PropKey.Create.event "scrollSlider.positionChanged_event"
    member val positionChanging: IEventPropKey<CancelEventArgs<int> -> unit> = PropKey.Create.event "scrollSlider.positionChanging_event"
    member val scrolled: IEventPropKey<EventArgs<int> -> unit> = PropKey.Create.event "scrollSlider.scrolled_event"

  // SelectorBase
  type selectorBasePKeys() =
    inherit viewPKeys()
    // Properties
    member val assignHotKeys: ISimplePropKey<bool> = PropKey.Create.simple "selectorBase.assignHotKeys"
    member val doubleClickAccepts: ISimplePropKey<bool> = PropKey.Create.simple "selectorBase.doubleClickAccepts"
    member val horizontalSpace: ISimplePropKey<int> = PropKey.Create.simple "selectorBase.horizontalSpace"
    member val labels: ISimplePropKey<IReadOnlyList<string>> = PropKey.Create.simple "selectorBase.labels"
    member val orientation: ISimplePropKey<Orientation> = PropKey.Create.simple "selectorBase.orientation"
    member val styles: ISimplePropKey<SelectorStyles> = PropKey.Create.simple "selectorBase.styles"
    member val usedHotKeys: ISimplePropKey<HashSet<Key>> = PropKey.Create.simple "selectorBase.usedHotKeys"
    member val value: ISimplePropKey<Nullable<int>> = PropKey.Create.simple "selectorBase.value"
    member val values: ISimplePropKey<IReadOnlyList<int>> = PropKey.Create.simple "selectorBase.values"

    // Events
    member val orientationChanged: IEventPropKey<EventArgs<Orientation> -> unit> = PropKey.Create.event "selectorBase.orientationChanged_event"
    member val orientationChanging: IEventPropKey<CancelEventArgs<Orientation> -> unit> = PropKey.Create.event "selectorBase.orientationChanging_event"
    member val valueChanged: IEventPropKey<EventArgs<Nullable<int>> -> unit> = PropKey.Create.event "selectorBase.valueChanged_event"

  // FlagSelector
  type flagSelectorPKeys() =
    inherit selectorBasePKeys()
    // Properties
    member val value: ISimplePropKey<Nullable<int>> = PropKey.Create.simple "flagSelector.value"

  // OptionSelector
  type optionSelectorPKeys() =
    inherit selectorBasePKeys()
    // Properties
    member val cursor: ISimplePropKey<int> = PropKey.Create.simple "optionSelector.cursor"

  // FlagSelector
  type flagSelectorPKeys<'TFlagsEnum when 'TFlagsEnum: struct and 'TFlagsEnum: (new: unit -> 'TFlagsEnum) and 'TFlagsEnum:> Enum and 'TFlagsEnum:> ValueType>() =
    inherit flagSelectorPKeys()
    // Properties
    member val value: ISimplePropKey<Nullable<'TFlagsEnum>> = PropKey.Create.simple "flagSelector.value"

    // Events
    member val valueChanged: IEventPropKey<EventArgs<Nullable<'TFlagsEnum>> -> unit> = PropKey.Create.event "flagSelector.valueChanged_event"

  // OptionSelector
  type optionSelectorPKeys<'TEnum when 'TEnum: struct and 'TEnum: (new: unit -> 'TEnum) and 'TEnum:> Enum and 'TEnum:> ValueType>() =
    inherit optionSelectorPKeys()
    // Properties
    member val value: ISimplePropKey<Nullable<'TEnum>> = PropKey.Create.simple "optionSelector.value"
    member val values: ISimplePropKey<IReadOnlyList<int>> = PropKey.Create.simple "optionSelector.values"

    // Events
    member val valueChanged: IEventPropKey<EventArgs<Nullable<'TEnum>> -> unit> = PropKey.Create.event "optionSelector.valueChanged_event"

  // Shortcut
  type shortcutPKeys() =
    inherit viewPKeys()
    // Properties
    member val action: ISimplePropKey<Action> = PropKey.Create.simple "shortcut.action"
    member val alignmentModes: ISimplePropKey<AlignmentModes> = PropKey.Create.simple "shortcut.alignmentModes"
    member val bindKeyToApplication: ISimplePropKey<bool> = PropKey.Create.simple "shortcut.bindKeyToApplication"
    member val commandView: ISimplePropKey<View> = PropKey.Create.simple "shortcut.commandView"
    member val forceFocusColors: ISimplePropKey<bool> = PropKey.Create.simple "shortcut.forceFocusColors"
    member val helpText: ISimplePropKey<string> = PropKey.Create.simple "shortcut.helpText"
    member val key: ISimplePropKey<Key> = PropKey.Create.simple "shortcut.key"
    member val minimumKeyTextSize: ISimplePropKey<int> = PropKey.Create.simple "shortcut.minimumKeyTextSize"
    member val orientation: ISimplePropKey<Orientation> = PropKey.Create.simple "shortcut.orientation"
    member val text: ISimplePropKey<string> = PropKey.Create.simple "shortcut.text"

    // Events
    member val orientationChanged: IEventPropKey<EventArgs<Orientation> -> unit> = PropKey.Create.event "shortcut.orientationChanged_event"
    member val orientationChanging: IEventPropKey<CancelEventArgs<Orientation> -> unit> = PropKey.Create.event "shortcut.orientationChanging_event"

  // MenuItem
  type menuItemPKeys() =
    inherit shortcutPKeys()
    // Properties
    member val command: ISimplePropKey<Command> = PropKey.Create.simple "menuItem.command"
    member val subMenu: ISimplePropKey<Menu> = PropKey.Create.simple "menuItem.subMenu"
    member val targetView: ISimplePropKey<View> = PropKey.Create.simple "menuItem.targetView"

  // MenuBarItem
  type menuBarItemPKeys() =
    inherit menuItemPKeys()
    // Properties
    member val popoverMenu: ISimplePropKey<PopoverMenu> = PropKey.Create.simple "menuBarItem.popoverMenu"
    member val popoverMenuOpen: ISimplePropKey<bool> = PropKey.Create.simple "menuBarItem.popoverMenuOpen"
    member val subMenu: ISimplePropKey<Menu> = PropKey.Create.simple "menuBarItem.subMenu"

    // Events
    member val popoverMenuOpenChanged: IEventPropKey<EventArgs<bool> -> unit> = PropKey.Create.event "menuBarItem.popoverMenuOpenChanged_event"

  // Slider
  type sliderPKeys<'T>() =
    inherit viewPKeys()
    // Properties
    member val allowEmpty: ISimplePropKey<bool> = PropKey.Create.simple "slider.allowEmpty"
    member val focusedOption: ISimplePropKey<int> = PropKey.Create.simple "slider.focusedOption"
    member val legendsOrientation: ISimplePropKey<Orientation> = PropKey.Create.simple "slider.legendsOrientation"
    member val minimumInnerSpacing: ISimplePropKey<int> = PropKey.Create.simple "slider.minimumInnerSpacing"
    member val options: ISimplePropKey<List<SliderOption<'T>>> = PropKey.Create.simple "slider.options"
    member val orientation: ISimplePropKey<Orientation> = PropKey.Create.simple "slider.orientation"
    member val rangeAllowSingle: ISimplePropKey<bool> = PropKey.Create.simple "slider.rangeAllowSingle"
    member val showEndSpacing: ISimplePropKey<bool> = PropKey.Create.simple "slider.showEndSpacing"
    member val showLegends: ISimplePropKey<bool> = PropKey.Create.simple "slider.showLegends"
    member val style: ISimplePropKey<SliderStyle> = PropKey.Create.simple "slider.style"
    member val text: ISimplePropKey<string> = PropKey.Create.simple "slider.text"
    member val ``type``: ISimplePropKey<SliderType> = PropKey.Create.simple "slider.type"
    member val useMinimumSize: ISimplePropKey<bool> = PropKey.Create.simple "slider.useMinimumSize"

    // Events
    member val optionFocused: IEventPropKey<SliderEventArgs<'T> -> unit> = PropKey.Create.event "slider.optionFocused_event"
    member val optionsChanged: IEventPropKey<SliderEventArgs<'T> -> unit> = PropKey.Create.event "slider.optionsChanged_event"
    member val orientationChanged: IEventPropKey<EventArgs<Orientation> -> unit> = PropKey.Create.event "slider.orientationChanged_event"
    member val orientationChanging: IEventPropKey<CancelEventArgs<Orientation> -> unit> = PropKey.Create.event "slider.orientationChanging_event"

  // SpinnerView
  type spinnerViewPKeys() =
    inherit viewPKeys()
    // Properties
    member val autoSpin: ISimplePropKey<bool> = PropKey.Create.simple "spinnerView.autoSpin"
    member val sequence: ISimplePropKey<String[]> = PropKey.Create.simple "spinnerView.sequence"
    member val spinBounce: ISimplePropKey<bool> = PropKey.Create.simple "spinnerView.spinBounce"
    member val spinDelay: ISimplePropKey<int> = PropKey.Create.simple "spinnerView.spinDelay"
    member val spinReverse: ISimplePropKey<bool> = PropKey.Create.simple "spinnerView.spinReverse"
    member val style: ISimplePropKey<SpinnerStyle> = PropKey.Create.simple "spinnerView.style"

  // StatusBar
  type statusBarPKeys() =
    inherit barPKeys()

  // Tab
  type tabPKeys() =
    inherit viewPKeys()
    // Properties
    member val displayText: ISimplePropKey<string> = PropKey.Create.simple "tab.displayText"
    member val view: ISimplePropKey<View> = PropKey.Create.simple "tab.view"

  // TabView
  type tabViewPKeys() =
    inherit viewPKeys()
    // Properties
    member val maxTabTextWidth: ISimplePropKey<UInt32> = PropKey.Create.simple "tabView.maxTabTextWidth"
    member val selectedTab: ISimplePropKey<Tab> = PropKey.Create.simple "tabView.selectedTab"
    member val style: ISimplePropKey<TabStyle> = PropKey.Create.simple "tabView.style"
    member val tabScrollOffset: ISimplePropKey<int> = PropKey.Create.simple "tabView.tabScrollOffset"

    // Events
    member val selectedTabChanged: IEventPropKey<TabChangedEventArgs -> unit> = PropKey.Create.event "tabView.selectedTabChanged_event"
    member val tabClicked: IEventPropKey<TabMouseEventArgs -> unit> = PropKey.Create.event "tabView.tabClicked_event"

  // TableView
  type tableViewPKeys() =
    inherit viewPKeys()
    // Properties
    member val cellActivationKey: ISimplePropKey<KeyCode> = PropKey.Create.simple "tableView.cellActivationKey"
    member val collectionNavigator: ISimplePropKey<ICollectionNavigator> = PropKey.Create.simple "tableView.collectionNavigator"
    member val columnOffset: ISimplePropKey<int> = PropKey.Create.simple "tableView.columnOffset"
    member val fullRowSelect: ISimplePropKey<bool> = PropKey.Create.simple "tableView.fullRowSelect"
    member val maxCellWidth: ISimplePropKey<int> = PropKey.Create.simple "tableView.maxCellWidth"
    member val minCellWidth: ISimplePropKey<int> = PropKey.Create.simple "tableView.minCellWidth"
    member val multiSelect: ISimplePropKey<bool> = PropKey.Create.simple "tableView.multiSelect"
    member val nullSymbol: ISimplePropKey<string> = PropKey.Create.simple "tableView.nullSymbol"
    member val rowOffset: ISimplePropKey<int> = PropKey.Create.simple "tableView.rowOffset"
    member val selectedColumn: ISimplePropKey<int> = PropKey.Create.simple "tableView.selectedColumn"
    member val selectedRow: ISimplePropKey<int> = PropKey.Create.simple "tableView.selectedRow"
    member val separatorSymbol: ISimplePropKey<Char> = PropKey.Create.simple "tableView.separatorSymbol"
    member val style: ISimplePropKey<TableStyle> = PropKey.Create.simple "tableView.style"
    member val table: ISimplePropKey<ITableSource> = PropKey.Create.simple "tableView.table"

    // Events
    member val cellActivated: IEventPropKey<CellActivatedEventArgs -> unit> = PropKey.Create.event "tableView.cellActivated_event"
    member val cellToggled: IEventPropKey<CellToggledEventArgs -> unit> = PropKey.Create.event "tableView.cellToggled_event"
    member val selectedCellChanged: IEventPropKey<SelectedCellChangedEventArgs -> unit> = PropKey.Create.event "tableView.selectedCellChanged_event"

  // TextField
  type textFieldPKeys() =
    inherit viewPKeys()
    // Properties
    member val autocomplete: ISimplePropKey<IAutocomplete> = PropKey.Create.simple "textField.autocomplete"
    member val cursorPosition: ISimplePropKey<int> = PropKey.Create.simple "textField.cursorPosition"
    member val readOnly: ISimplePropKey<bool> = PropKey.Create.simple "textField.readOnly"
    member val secret: ISimplePropKey<bool> = PropKey.Create.simple "textField.secret"
    member val selectWordOnlyOnDoubleClick: ISimplePropKey<bool> = PropKey.Create.simple "textField.selectWordOnlyOnDoubleClick"
    member val selectedStart: ISimplePropKey<int> = PropKey.Create.simple "textField.selectedStart"
    member val text: ISimplePropKey<string> = PropKey.Create.simple "textField.text"
    member val useSameRuneTypeForWords: ISimplePropKey<bool> = PropKey.Create.simple "textField.useSameRuneTypeForWords"
    member val used: ISimplePropKey<bool> = PropKey.Create.simple "textField.used"

    // Events
    member val textChanging: IEventPropKey<ResultEventArgs<string> -> unit> = PropKey.Create.event "textField.textChanging_event"

  // DateField
  type dateFieldPKeys() =
    inherit textFieldPKeys()
    // Properties
    member val culture: ISimplePropKey<CultureInfo> = PropKey.Create.simple "dateField.culture"
    member val cursorPosition: ISimplePropKey<int> = PropKey.Create.simple "dateField.cursorPosition"
    member val date: ISimplePropKey<Nullable<DateTime>> = PropKey.Create.simple "dateField.date"

    // Events
    member val dateChanged: IEventPropKey<EventArgs<DateTime> -> unit> = PropKey.Create.event "dateField.dateChanged_event"

  // TextValidateField
  type textValidateFieldPKeys() =
    inherit viewPKeys()
    // Properties
    member val provider: ISimplePropKey<ITextValidateProvider> = PropKey.Create.simple "textValidateField.provider"
    member val text: ISimplePropKey<string> = PropKey.Create.simple "textValidateField.text"

  // TextView
  type textViewPKeys() =
    inherit viewPKeys()
    // Properties
    member val allowsReturn: ISimplePropKey<bool> = PropKey.Create.simple "textView.allowsReturn"
    member val allowsTab: ISimplePropKey<bool> = PropKey.Create.simple "textView.allowsTab"
    member val cursorPosition: ISimplePropKey<Point> = PropKey.Create.simple "textView.cursorPosition"
    member val inheritsPreviousAttribute: ISimplePropKey<bool> = PropKey.Create.simple "textView.inheritsPreviousAttribute"
    member val isDirty: ISimplePropKey<bool> = PropKey.Create.simple "textView.isDirty"
    member val isSelecting: ISimplePropKey<bool> = PropKey.Create.simple "textView.isSelecting"
    member val leftColumn: ISimplePropKey<int> = PropKey.Create.simple "textView.leftColumn"
    member val multiline: ISimplePropKey<bool> = PropKey.Create.simple "textView.multiline"
    member val readOnly: ISimplePropKey<bool> = PropKey.Create.simple "textView.readOnly"
    member val selectWordOnlyOnDoubleClick: ISimplePropKey<bool> = PropKey.Create.simple "textView.selectWordOnlyOnDoubleClick"
    member val selectionStartColumn: ISimplePropKey<int> = PropKey.Create.simple "textView.selectionStartColumn"
    member val selectionStartRow: ISimplePropKey<int> = PropKey.Create.simple "textView.selectionStartRow"
    member val tabWidth: ISimplePropKey<int> = PropKey.Create.simple "textView.tabWidth"
    member val text: ISimplePropKey<string> = PropKey.Create.simple "textView.text"
    member val topRow: ISimplePropKey<int> = PropKey.Create.simple "textView.topRow"
    member val useSameRuneTypeForWords: ISimplePropKey<bool> = PropKey.Create.simple "textView.useSameRuneTypeForWords"
    member val used: ISimplePropKey<bool> = PropKey.Create.simple "textView.used"
    member val wordWrap: ISimplePropKey<bool> = PropKey.Create.simple "textView.wordWrap"

    // Events
    member val contentsChanged: IEventPropKey<ContentsChangedEventArgs -> unit> = PropKey.Create.event "textView.contentsChanged_event"
    member val drawNormalColor: IEventPropKey<CellEventArgs -> unit> = PropKey.Create.event "textView.drawNormalColor_event"
    member val drawReadOnlyColor: IEventPropKey<CellEventArgs -> unit> = PropKey.Create.event "textView.drawReadOnlyColor_event"
    member val drawSelectionColor: IEventPropKey<CellEventArgs -> unit> = PropKey.Create.event "textView.drawSelectionColor_event"
    member val drawUsedColor: IEventPropKey<CellEventArgs -> unit> = PropKey.Create.event "textView.drawUsedColor_event"
    member val unwrappedCursorPosition: IEventPropKey<Point -> unit> = PropKey.Create.event "textView.unwrappedCursorPosition_event"

  // TimeField
  type timeFieldPKeys() =
    inherit textFieldPKeys()
    // Properties
    member val cursorPosition: ISimplePropKey<int> = PropKey.Create.simple "timeField.cursorPosition"
    member val isShortFormat: ISimplePropKey<bool> = PropKey.Create.simple "timeField.isShortFormat"
    member val time: ISimplePropKey<TimeSpan> = PropKey.Create.simple "timeField.time"

    // Events
    member val timeChanged: IEventPropKey<EventArgs<TimeSpan> -> unit> = PropKey.Create.event "timeField.timeChanged_event"

  // TreeView
  type treeViewPKeys<'T when 'T: not struct>() =
    inherit viewPKeys()
    // Properties
    member val allowLetterBasedNavigation: ISimplePropKey<bool> = PropKey.Create.simple "treeView.allowLetterBasedNavigation"
    member val aspectGetter: ISimplePropKey<AspectGetterDelegate<'T>> = PropKey.Create.simple "treeView.aspectGetter"
    member val colorGetter: ISimplePropKey<Func<'T, Scheme>> = PropKey.Create.simple "treeView.colorGetter"
    member val maxDepth: ISimplePropKey<int> = PropKey.Create.simple "treeView.maxDepth"
    member val multiSelect: ISimplePropKey<bool> = PropKey.Create.simple "treeView.multiSelect"
    member val objectActivationButton: ISimplePropKey<Nullable<MouseFlags>> = PropKey.Create.simple "treeView.objectActivationButton"
    member val objectActivationKey: ISimplePropKey<KeyCode> = PropKey.Create.simple "treeView.objectActivationKey"
    member val scrollOffsetHorizontal: ISimplePropKey<int> = PropKey.Create.simple "treeView.scrollOffsetHorizontal"
    member val scrollOffsetVertical: ISimplePropKey<int> = PropKey.Create.simple "treeView.scrollOffsetVertical"
    member val selectedObject: ISimplePropKey<'T> = PropKey.Create.simple "treeView.selectedObject"
    member val style: ISimplePropKey<TreeStyle> = PropKey.Create.simple "treeView.style"
    member val treeBuilder: ISimplePropKey<ITreeBuilder<'T>> = PropKey.Create.simple "treeView.treeBuilder"

    // Events
    member val drawLine: IEventPropKey<DrawTreeViewLineEventArgs<'T> -> unit> = PropKey.Create.event "treeView.drawLine_event"
    member val objectActivated: IEventPropKey<ObjectActivatedEventArgs<'T> -> unit> = PropKey.Create.event "treeView.objectActivated_event"
    member val selectionChanged: IEventPropKey<SelectionChangedEventArgs<'T> -> unit> = PropKey.Create.event "treeView.selectionChanged_event"

  // Window
  type windowPKeys() =
    inherit runnablePKeys()

  // Dialog
  type dialogPKeys() =
    inherit windowPKeys()
    // Properties
    member val buttonAlignment: ISimplePropKey<Alignment> = PropKey.Create.simple "dialog.buttonAlignment"
    member val buttonAlignmentModes: ISimplePropKey<AlignmentModes> = PropKey.Create.simple "dialog.buttonAlignmentModes"
    member val canceled: ISimplePropKey<bool> = PropKey.Create.simple "dialog.canceled"

  // FileDialog
  type fileDialogPKeys() =
    inherit dialogPKeys()
    // Properties
    member val allowedTypes: ISimplePropKey<List<IAllowedType>> = PropKey.Create.simple "fileDialog.allowedTypes"
    member val allowsMultipleSelection: ISimplePropKey<bool> = PropKey.Create.simple "fileDialog.allowsMultipleSelection"
    member val fileOperationsHandler: ISimplePropKey<IFileOperations> = PropKey.Create.simple "fileDialog.fileOperationsHandler"
    member val mustExist: ISimplePropKey<bool> = PropKey.Create.simple "fileDialog.mustExist"
    member val openMode: ISimplePropKey<OpenMode> = PropKey.Create.simple "fileDialog.openMode"
    member val path: ISimplePropKey<string> = PropKey.Create.simple "fileDialog.path"
    member val searchMatcher: ISimplePropKey<ISearchMatcher> = PropKey.Create.simple "fileDialog.searchMatcher"

    // Events
    member val filesSelected: IEventPropKey<FilesSelectedEventArgs -> unit> = PropKey.Create.event "fileDialog.filesSelected_event"

  // OpenDialog
  type openDialogPKeys() =
    inherit fileDialogPKeys()
    // Properties
    member val openMode: ISimplePropKey<OpenMode> = PropKey.Create.simple "openDialog.openMode"

  // SaveDialog
  type saveDialogPKeys() =
    inherit fileDialogPKeys()

  // Wizard
  type wizardPKeys() =
    inherit dialogPKeys()
    // Properties
    member val currentStep: ISimplePropKey<WizardStep> = PropKey.Create.simple "wizard.currentStep"

    // Events
    member val cancelled: IEventPropKey<WizardButtonEventArgs -> unit> = PropKey.Create.event "wizard.cancelled_event"
    member val finished: IEventPropKey<WizardButtonEventArgs -> unit> = PropKey.Create.event "wizard.finished_event"
    member val movingBack: IEventPropKey<WizardButtonEventArgs -> unit> = PropKey.Create.event "wizard.movingBack_event"
    member val movingNext: IEventPropKey<WizardButtonEventArgs -> unit> = PropKey.Create.event "wizard.movingNext_event"
    member val stepChanged: IEventPropKey<StepChangeEventArgs -> unit> = PropKey.Create.event "wizard.stepChanged_event"
    member val stepChanging: IEventPropKey<StepChangeEventArgs -> unit> = PropKey.Create.event "wizard.stepChanging_event"

  // WizardStep
  type wizardStepPKeys() =
    inherit viewPKeys()
    // Properties
    member val backButtonText: ISimplePropKey<string> = PropKey.Create.simple "wizardStep.backButtonText"
    member val helpText: ISimplePropKey<string> = PropKey.Create.simple "wizardStep.helpText"
    member val nextButtonText: ISimplePropKey<string> = PropKey.Create.simple "wizardStep.nextButtonText"

  // IMouseHeldDown
  module internal mouseHeldDownInterface =
    // Events
    let mouseIsHeldDownTick: IEventPropKey<CancelEventArgs -> unit> = PropKey.Create.event "mouseHeldDownInterface.mouseIsHeldDownTick_event"
  // IOrientation
  module internal orientationInterface =
    // Properties
    let orientation: ISimplePropKey<Terminal.Gui.ViewBase.Orientation> = PropKey.Create.simple "orientationInterface.orientation"
    // Events
    let orientationChanged: IEventPropKey<EventArgs<Orientation> -> unit> = PropKey.Create.event "orientationInterface.orientationChanged_event"
    let orientationChanging: IEventPropKey<CancelEventArgs<Orientation> -> unit> = PropKey.Create.event "orientationInterface.orientationChanging_event"

  let view = viewPKeys ()
  let adornment = adornmentPKeys ()
  let bar = barPKeys ()
  let border = borderPKeys ()
  let button = buttonPKeys ()
  let charMap = charMapPKeys ()
  let checkBox = checkBoxPKeys ()
  let colorPicker = colorPickerPKeys ()
  let colorPicker16 = colorPicker16PKeys ()
  let comboBox = comboBoxPKeys ()
  let datePicker = datePickerPKeys ()
  let frameView = frameViewPKeys ()
  let graphView = graphViewPKeys ()
  let hexView = hexViewPKeys ()
  let label = labelPKeys ()
  let legendAnnotation = legendAnnotationPKeys ()
  let line = linePKeys ()
  let listView = listViewPKeys ()
  let margin = marginPKeys ()
  let menu = menuPKeys ()
  let menuBar = menuBarPKeys ()
  let numericUpDown<'T> = numericUpDownPKeys<'T>()
  let padding = paddingPKeys ()
  let popoverBaseImpl = popoverBaseImplPKeys ()
  let popoverMenu = popoverMenuPKeys ()
  let progressBar = progressBarPKeys ()
  let runnable = runnablePKeys ()
  let runnable'<'TResult> = runnablePKeys<'TResult>()
  let scrollBar = scrollBarPKeys ()
  let scrollSlider = scrollSliderPKeys ()
  let selectorBase = selectorBasePKeys ()
  let flagSelector = flagSelectorPKeys ()
  let optionSelector = optionSelectorPKeys ()
  let flagSelector'<'TFlagsEnum when 'TFlagsEnum: struct and 'TFlagsEnum: (new: unit -> 'TFlagsEnum) and 'TFlagsEnum:> Enum and 'TFlagsEnum:> ValueType> = flagSelectorPKeys<'TFlagsEnum>()
  let optionSelector'<'TEnum when 'TEnum: struct and 'TEnum: (new: unit -> 'TEnum) and 'TEnum:> Enum and 'TEnum:> ValueType> = optionSelectorPKeys<'TEnum>()
  let shortcut = shortcutPKeys ()
  let menuItem = menuItemPKeys ()
  let menuBarItem = menuBarItemPKeys ()
  let slider<'T> = sliderPKeys<'T>()
  let spinnerView = spinnerViewPKeys ()
  let statusBar = statusBarPKeys ()
  let tab = tabPKeys ()
  let tabView = tabViewPKeys ()
  let tableView = tableViewPKeys ()
  let textField = textFieldPKeys ()
  let dateField = dateFieldPKeys ()
  let textValidateField = textValidateFieldPKeys ()
  let textView = textViewPKeys ()
  let timeField = timeFieldPKeys ()
  let treeView<'T when 'T: not struct> = treeViewPKeys<'T>()
  let window = windowPKeys ()
  let dialog = dialogPKeys ()
  let fileDialog = fileDialogPKeys ()
  let openDialog = openDialogPKeys ()
  let saveDialog = saveDialogPKeys ()
  let wizard = wizardPKeys ()
  let wizardStep = wizardStepPKeys ()