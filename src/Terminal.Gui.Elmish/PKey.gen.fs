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

[<RequireQualifiedAccess>]
module internal PKey =

  type ViewPKeys() =
    member val children: ISimplePropKey<List<IInternalTerminalElement>> = PropKey.Create.simple "children"

    // Properties
    member val Arrangement: ISimplePropKey<ViewArrangement> = PropKey.Create.simple "View.Arrangement"
    member val AssignHotKeys: ISimplePropKey<bool> = PropKey.Create.simple "View.AssignHotKeys"
    member val BorderStyle: ISimplePropKey<LineStyle> = PropKey.Create.simple "View.BorderStyle"
    member val CanFocus: ISimplePropKey<bool> = PropKey.Create.simple "View.CanFocus"
    member val ContentSizeTracksViewport: ISimplePropKey<bool> = PropKey.Create.simple "View.ContentSizeTracksViewport"
    member val Cursor: ISimplePropKey<Cursor> = PropKey.Create.simple "View.Cursor"
    member val Data: ISimplePropKey<Object> = PropKey.Create.simple "View.Data"
    member val Enabled: ISimplePropKey<bool> = PropKey.Create.simple "View.Enabled"
    member val Frame: ISimplePropKey<Rectangle> = PropKey.Create.simple "View.Frame"
    member val HasFocus: ISimplePropKey<bool> = PropKey.Create.simple "View.HasFocus"
    member val Height: ISimplePropKey<Dim> = PropKey.Create.simple "View.Height"
    member val HotKey: ISimplePropKey<Key> = PropKey.Create.simple "View.HotKey"
    member val HotKeySpecifier: ISimplePropKey<Rune> = PropKey.Create.simple "View.HotKeySpecifier"
    member val Id: ISimplePropKey<string> = PropKey.Create.simple "View.Id"
    member val IsInitialized: ISimplePropKey<bool> = PropKey.Create.simple "View.IsInitialized"
    member val MouseHighlightStates: ISimplePropKey<MouseState> = PropKey.Create.simple "View.MouseHighlightStates"
    member val MouseHoldRepeat: ISimplePropKey<Nullable<MouseFlags>> = PropKey.Create.simple "View.MouseHoldRepeat"
    member val MousePositionTracking: ISimplePropKey<bool> = PropKey.Create.simple "View.MousePositionTracking"
    member val PreserveTrailingSpaces: ISimplePropKey<bool> = PropKey.Create.simple "View.PreserveTrailingSpaces"
    member val SchemeName: ISimplePropKey<string> = PropKey.Create.simple "View.SchemeName"
    member val ShadowStyle: ISimplePropKey<ShadowStyle> = PropKey.Create.simple "View.ShadowStyle"
    member val SuperViewRendersLineCanvas: ISimplePropKey<bool> = PropKey.Create.simple "View.SuperViewRendersLineCanvas"
    member val TabStop: ISimplePropKey<Nullable<TabBehavior>> = PropKey.Create.simple "View.TabStop"
    member val Text: ISimplePropKey<string> = PropKey.Create.simple "View.Text"
    member val TextAlignment: ISimplePropKey<Alignment> = PropKey.Create.simple "View.TextAlignment"
    member val TextDirection: ISimplePropKey<TextDirection> = PropKey.Create.simple "View.TextDirection"
    member val Title: ISimplePropKey<string> = PropKey.Create.simple "View.Title"
    member val UsedHotKeys: ISimplePropKey<HashSet<Key>> = PropKey.Create.simple "View.UsedHotKeys"
    member val ValidatePosDim: ISimplePropKey<bool> = PropKey.Create.simple "View.ValidatePosDim"
    member val VerticalTextAlignment: ISimplePropKey<Alignment> = PropKey.Create.simple "View.VerticalTextAlignment"
    member val Viewport: ISimplePropKey<Rectangle> = PropKey.Create.simple "View.Viewport"
    member val ViewportSettings: ISimplePropKey<ViewportSettingsFlags> = PropKey.Create.simple "View.ViewportSettings"
    member val Visible: ISimplePropKey<bool> = PropKey.Create.simple "View.Visible"
    member val Width: ISimplePropKey<Dim> = PropKey.Create.simple "View.Width"
    member val X: ISimplePropKey<Pos> = PropKey.Create.simple "View.X"
    member val X_delayedPos: IDelayedPosKey = PropKey.Create.delayedPos "View.X_delayedPos"
    member val Y: ISimplePropKey<Pos> = PropKey.Create.simple "View.Y"
    member val Y_delayedPos: IDelayedPosKey = PropKey.Create.delayedPos "View.Y_delayedPos"

    // Events
    member val Accepted: IEventPropKey<CommandEventArgs -> unit> = PropKey.Create.event "View.Accepted_event"
    member val Accepting: IEventPropKey<CommandEventArgs -> unit> = PropKey.Create.event "View.Accepting_event"
    member val Activating: IEventPropKey<CommandEventArgs -> unit> = PropKey.Create.event "View.Activating_event"
    member val AdvancingFocus: IEventPropKey<AdvanceFocusEventArgs -> unit> = PropKey.Create.event "View.AdvancingFocus_event"
    member val BorderStyleChanged: IEventPropKey<EventArgs -> unit> = PropKey.Create.event "View.BorderStyleChanged_event"
    member val CanFocusChanged: IEventPropKey<EventArgs -> unit> = PropKey.Create.event "View.CanFocusChanged_event"
    member val ClearedViewport: IEventPropKey<DrawEventArgs -> unit> = PropKey.Create.event "View.ClearedViewport_event"
    member val ClearingViewport: IEventPropKey<DrawEventArgs -> unit> = PropKey.Create.event "View.ClearingViewport_event"
    member val CommandNotBound: IEventPropKey<CommandEventArgs -> unit> = PropKey.Create.event "View.CommandNotBound_event"
    member val ContentSizeChanged: IEventPropKey<ValueChangedEventArgs<Nullable<Size>> -> unit> = PropKey.Create.event "View.ContentSizeChanged_event"
    member val ContentSizeChanging: IEventPropKey<ValueChangingEventArgs<Nullable<Size>> -> unit> = PropKey.Create.event "View.ContentSizeChanging_event"
    member val Disposing: IEventPropKey<EventArgs -> unit> = PropKey.Create.event "View.Disposing_event"
    member val DrawComplete: IEventPropKey<DrawEventArgs -> unit> = PropKey.Create.event "View.DrawComplete_event"
    member val DrawingContent: IEventPropKey<DrawEventArgs -> unit> = PropKey.Create.event "View.DrawingContent_event"
    member val DrawingSubViews: IEventPropKey<DrawEventArgs -> unit> = PropKey.Create.event "View.DrawingSubViews_event"
    member val DrawingText: IEventPropKey<DrawEventArgs -> unit> = PropKey.Create.event "View.DrawingText_event"
    member val DrewText: IEventPropKey<EventArgs -> unit> = PropKey.Create.event "View.DrewText_event"
    member val EnabledChanged: IEventPropKey<EventArgs -> unit> = PropKey.Create.event "View.EnabledChanged_event"
    member val FocusedChanged: IEventPropKey<HasFocusEventArgs -> unit> = PropKey.Create.event "View.FocusedChanged_event"
    member val FrameChanged: IEventPropKey<EventArgs<Rectangle> -> unit> = PropKey.Create.event "View.FrameChanged_event"
    member val GettingAttributeForRole: IEventPropKey<VisualRoleEventArgs -> unit> = PropKey.Create.event "View.GettingAttributeForRole_event"
    member val GettingScheme: IEventPropKey<ResultEventArgs<Scheme> -> unit> = PropKey.Create.event "View.GettingScheme_event"
    member val HandlingHotKey: IEventPropKey<CommandEventArgs -> unit> = PropKey.Create.event "View.HandlingHotKey_event"
    member val HasFocusChanged: IEventPropKey<HasFocusEventArgs -> unit> = PropKey.Create.event "View.HasFocusChanged_event"
    member val HasFocusChanging: IEventPropKey<HasFocusEventArgs -> unit> = PropKey.Create.event "View.HasFocusChanging_event"
    member val HeightChanged: IEventPropKey<ValueChangedEventArgs<Dim> -> unit> = PropKey.Create.event "View.HeightChanged_event"
    member val HeightChanging: IEventPropKey<ValueChangingEventArgs<Dim> -> unit> = PropKey.Create.event "View.HeightChanging_event"
    member val HotKeyChanged: IEventPropKey<KeyChangedEventArgs -> unit> = PropKey.Create.event "View.HotKeyChanged_event"
    member val Initialized: IEventPropKey<EventArgs -> unit> = PropKey.Create.event "View.Initialized_event"
    member val KeyDown: IEventPropKey<Key -> unit> = PropKey.Create.event "View.KeyDown_event"
    member val KeyDownNotHandled: IEventPropKey<Key -> unit> = PropKey.Create.event "View.KeyDownNotHandled_event"
    member val MouseEnter: IEventPropKey<CancelEventArgs -> unit> = PropKey.Create.event "View.MouseEnter_event"
    member val MouseEvent: IEventPropKey<Mouse -> unit> = PropKey.Create.event "View.MouseEvent_event"
    member val MouseHoldRepeatChanged: IEventPropKey<ValueChangedEventArgs<Nullable<MouseFlags>> -> unit> = PropKey.Create.event "View.MouseHoldRepeatChanged_event"
    member val MouseHoldRepeatChanging: IEventPropKey<ValueChangingEventArgs<Nullable<MouseFlags>> -> unit> = PropKey.Create.event "View.MouseHoldRepeatChanging_event"
    member val MouseLeave: IEventPropKey<EventArgs -> unit> = PropKey.Create.event "View.MouseLeave_event"
    member val MouseStateChanged: IEventPropKey<EventArgs<MouseState> -> unit> = PropKey.Create.event "View.MouseStateChanged_event"
    member val Removed: IEventPropKey<SuperViewChangedEventArgs -> unit> = PropKey.Create.event "View.Removed_event"
    member val SchemeChanged: IEventPropKey<ValueChangedEventArgs<Scheme> -> unit> = PropKey.Create.event "View.SchemeChanged_event"
    member val SchemeChanging: IEventPropKey<ValueChangingEventArgs<Scheme> -> unit> = PropKey.Create.event "View.SchemeChanging_event"
    member val SchemeNameChanged: IEventPropKey<ValueChangedEventArgs<string> -> unit> = PropKey.Create.event "View.SchemeNameChanged_event"
    member val SchemeNameChanging: IEventPropKey<ValueChangingEventArgs<string> -> unit> = PropKey.Create.event "View.SchemeNameChanging_event"
    member val SubViewAdded: IEventPropKey<SuperViewChangedEventArgs -> unit> = PropKey.Create.event "View.SubViewAdded_event"
    member val SubViewLayout: IEventPropKey<LayoutEventArgs -> unit> = PropKey.Create.event "View.SubViewLayout_event"
    member val SubViewRemoved: IEventPropKey<SuperViewChangedEventArgs -> unit> = PropKey.Create.event "View.SubViewRemoved_event"
    member val SubViewsLaidOut: IEventPropKey<LayoutEventArgs -> unit> = PropKey.Create.event "View.SubViewsLaidOut_event"
    member val SuperViewChanged: IEventPropKey<ValueChangedEventArgs<View> -> unit> = PropKey.Create.event "View.SuperViewChanged_event"
    member val SuperViewChanging: IEventPropKey<ValueChangingEventArgs<View> -> unit> = PropKey.Create.event "View.SuperViewChanging_event"
    member val TextChanged: IEventPropKey<EventArgs -> unit> = PropKey.Create.event "View.TextChanged_event"
    member val TitleChanged: IEventPropKey<EventArgs<string> -> unit> = PropKey.Create.event "View.TitleChanged_event"
    member val TitleChanging: IEventPropKey<CancelEventArgs<string> -> unit> = PropKey.Create.event "View.TitleChanging_event"
    member val ViewportChanged: IEventPropKey<DrawEventArgs -> unit> = PropKey.Create.event "View.ViewportChanged_event"
    member val VisibleChanged: IEventPropKey<EventArgs -> unit> = PropKey.Create.event "View.VisibleChanged_event"
    member val VisibleChanging: IEventPropKey<CancelEventArgs<bool> -> unit> = PropKey.Create.event "View.VisibleChanging_event"
    member val WidthChanged: IEventPropKey<ValueChangedEventArgs<Dim> -> unit> = PropKey.Create.event "View.WidthChanged_event"
    member val WidthChanging: IEventPropKey<ValueChangingEventArgs<Dim> -> unit> = PropKey.Create.event "View.WidthChanging_event"

  type AdornmentPKeys() =
    inherit ViewPKeys()

    // Properties
    member val Diagnostics: ISimplePropKey<ViewDiagnosticFlags> = PropKey.Create.simple "Adornment.Diagnostics"
    member val Parent: IViewPropKey<View> = PropKey.Create.view "Adornment.Parent_view"
    member val Parent_element: ISingleElementPropKey<ITerminalElement> = PropKey.Create.singleElement "Adornment.Parent_element"
    member val SuperViewRendersLineCanvas: ISimplePropKey<bool> = PropKey.Create.simple "Adornment.SuperViewRendersLineCanvas"
    member val Thickness: ISimplePropKey<Thickness> = PropKey.Create.simple "Adornment.Thickness"
    member val Viewport: ISimplePropKey<Rectangle> = PropKey.Create.simple "Adornment.Viewport"

    // Events
    member val ThicknessChanged: IEventPropKey<EventArgs -> unit> = PropKey.Create.event "Adornment.ThicknessChanged_event"

  type AttributePickerPKeys() =
    inherit ViewPKeys()

    // Properties
    member val SampleText: ISimplePropKey<string> = PropKey.Create.simple "AttributePicker.SampleText"
    member val Value: ISimplePropKey<Nullable<Attribute>> = PropKey.Create.simple "AttributePicker.Value"

    // Events
    member val ValueChanged: IEventPropKey<ValueChangedEventArgs<Nullable<Attribute>> -> unit> = PropKey.Create.event "AttributePicker.ValueChanged_event"
    member val ValueChanging: IEventPropKey<ValueChangingEventArgs<Nullable<Attribute>> -> unit> = PropKey.Create.event "AttributePicker.ValueChanging_event"

  type BarPKeys() =
    inherit ViewPKeys()

    // Properties
    member val AlignmentModes: ISimplePropKey<AlignmentModes> = PropKey.Create.simple "Bar.AlignmentModes"
    member val Orientation: ISimplePropKey<Orientation> = PropKey.Create.simple "Bar.Orientation"

    // Events
    member val OrientationChanged: IEventPropKey<EventArgs<Orientation> -> unit> = PropKey.Create.event "Bar.OrientationChanged_event"
    member val OrientationChanging: IEventPropKey<CancelEventArgs<Orientation> -> unit> = PropKey.Create.event "Bar.OrientationChanging_event"

  type BorderPKeys() =
    inherit AdornmentPKeys()

    // Properties
    member val LineStyle: ISimplePropKey<LineStyle> = PropKey.Create.simple "Border.LineStyle"
    member val Settings: ISimplePropKey<BorderSettings> = PropKey.Create.simple "Border.Settings"

  type ButtonPKeys() =
    inherit ViewPKeys()

    // Properties
    member val HotKeySpecifier: ISimplePropKey<Rune> = PropKey.Create.simple "Button.HotKeySpecifier"
    member val IsDefault: ISimplePropKey<bool> = PropKey.Create.simple "Button.IsDefault"
    member val NoDecorations: ISimplePropKey<bool> = PropKey.Create.simple "Button.NoDecorations"
    member val NoPadding: ISimplePropKey<bool> = PropKey.Create.simple "Button.NoPadding"
    member val Text: ISimplePropKey<string> = PropKey.Create.simple "Button.Text"

  type CharMapPKeys() =
    inherit ViewPKeys()

    // Properties
    member val SelectedCodePoint: ISimplePropKey<int> = PropKey.Create.simple "CharMap.SelectedCodePoint"
    member val ShowGlyphWidths: ISimplePropKey<bool> = PropKey.Create.simple "CharMap.ShowGlyphWidths"
    member val ShowUnicodeCategory: ISimplePropKey<Nullable<UnicodeCategory>> = PropKey.Create.simple "CharMap.ShowUnicodeCategory"
    member val StartCodePoint: ISimplePropKey<int> = PropKey.Create.simple "CharMap.StartCodePoint"
    member val Value: ISimplePropKey<Rune> = PropKey.Create.simple "CharMap.Value"

    // Events
    member val ValueChanged: IEventPropKey<ValueChangedEventArgs<Rune> -> unit> = PropKey.Create.event "CharMap.ValueChanged_event"
    member val ValueChanging: IEventPropKey<ValueChangingEventArgs<Rune> -> unit> = PropKey.Create.event "CharMap.ValueChanging_event"

  type CheckBoxPKeys() =
    inherit ViewPKeys()

    // Properties
    member val AllowCheckStateNone: ISimplePropKey<bool> = PropKey.Create.simple "CheckBox.AllowCheckStateNone"
    member val HotKeySpecifier: ISimplePropKey<Rune> = PropKey.Create.simple "CheckBox.HotKeySpecifier"
    member val RadioStyle: ISimplePropKey<bool> = PropKey.Create.simple "CheckBox.RadioStyle"
    member val Text: ISimplePropKey<string> = PropKey.Create.simple "CheckBox.Text"
    member val Value: ISimplePropKey<CheckState> = PropKey.Create.simple "CheckBox.Value"

    // Events
    member val ValueChanged: IEventPropKey<ValueChangedEventArgs<CheckState> -> unit> = PropKey.Create.event "CheckBox.ValueChanged_event"
    member val ValueChanging: IEventPropKey<ValueChangingEventArgs<CheckState> -> unit> = PropKey.Create.event "CheckBox.ValueChanging_event"

  type ColorPickerPKeys() =
    inherit ViewPKeys()

    // Properties
    member val SelectedColor: ISimplePropKey<Color> = PropKey.Create.simple "ColorPicker.SelectedColor"
    member val Style: ISimplePropKey<ColorPickerStyle> = PropKey.Create.simple "ColorPicker.Style"
    member val Text: ISimplePropKey<string> = PropKey.Create.simple "ColorPicker.Text"
    member val Value: ISimplePropKey<Nullable<Color>> = PropKey.Create.simple "ColorPicker.Value"

    // Events
    member val ValueChanged: IEventPropKey<ValueChangedEventArgs<Nullable<Color>> -> unit> = PropKey.Create.event "ColorPicker.ValueChanged_event"
    member val ValueChanging: IEventPropKey<ValueChangingEventArgs<Nullable<Color>> -> unit> = PropKey.Create.event "ColorPicker.ValueChanging_event"

  type ColorPicker16PKeys() =
    inherit ViewPKeys()

    // Properties
    member val BoxHeight: ISimplePropKey<int> = PropKey.Create.simple "ColorPicker16.BoxHeight"
    member val BoxWidth: ISimplePropKey<int> = PropKey.Create.simple "ColorPicker16.BoxWidth"
    member val Caret: ISimplePropKey<Point> = PropKey.Create.simple "ColorPicker16.Caret"
    member val SelectedColor: ISimplePropKey<ColorName16> = PropKey.Create.simple "ColorPicker16.SelectedColor"
    member val Value: ISimplePropKey<ColorName16> = PropKey.Create.simple "ColorPicker16.Value"

    // Events
    member val ValueChanged: IEventPropKey<ValueChangedEventArgs<ColorName16> -> unit> = PropKey.Create.event "ColorPicker16.ValueChanged_event"
    member val ValueChanging: IEventPropKey<ValueChangingEventArgs<ColorName16> -> unit> = PropKey.Create.event "ColorPicker16.ValueChanging_event"

  type ComboBoxPKeys() =
    inherit ViewPKeys()

    // Properties
    member val HideDropdownListOnClick: ISimplePropKey<bool> = PropKey.Create.simple "ComboBox.HideDropdownListOnClick"
    member val ReadOnly: ISimplePropKey<bool> = PropKey.Create.simple "ComboBox.ReadOnly"
    member val SearchText: ISimplePropKey<string> = PropKey.Create.simple "ComboBox.SearchText"
    member val SelectedItem: ISimplePropKey<int> = PropKey.Create.simple "ComboBox.SelectedItem"
    member val Source: ISimplePropKey<IListDataSource> = PropKey.Create.simple "ComboBox.Source"
    member val Text: ISimplePropKey<string> = PropKey.Create.simple "ComboBox.Text"

    // Events
    member val Collapsed: IEventPropKey<EventArgs -> unit> = PropKey.Create.event "ComboBox.Collapsed_event"
    member val Expanded: IEventPropKey<EventArgs -> unit> = PropKey.Create.event "ComboBox.Expanded_event"
    member val OpenSelectedItem: IEventPropKey<ListViewItemEventArgs -> unit> = PropKey.Create.event "ComboBox.OpenSelectedItem_event"
    member val SelectedItemChanged: IEventPropKey<ListViewItemEventArgs -> unit> = PropKey.Create.event "ComboBox.SelectedItemChanged_event"

  type DatePickerPKeys() =
    inherit ViewPKeys()

    // Properties
    member val Culture: ISimplePropKey<CultureInfo> = PropKey.Create.simple "DatePicker.Culture"
    member val Text: ISimplePropKey<string> = PropKey.Create.simple "DatePicker.Text"
    member val Value: ISimplePropKey<DateTime> = PropKey.Create.simple "DatePicker.Value"

    // Events
    member val ValueChanged: IEventPropKey<ValueChangedEventArgs<DateTime> -> unit> = PropKey.Create.event "DatePicker.ValueChanged_event"
    member val ValueChanging: IEventPropKey<ValueChangingEventArgs<DateTime> -> unit> = PropKey.Create.event "DatePicker.ValueChanging_event"

  type FrameViewPKeys() =
    inherit ViewPKeys()


  type GraphViewPKeys() =
    inherit ViewPKeys()

    // Properties
    member val AxisX: ISimplePropKey<HorizontalAxis> = PropKey.Create.simple "GraphView.AxisX"
    member val AxisY: ISimplePropKey<VerticalAxis> = PropKey.Create.simple "GraphView.AxisY"
    member val CellSize: ISimplePropKey<PointF> = PropKey.Create.simple "GraphView.CellSize"
    member val GraphColor: ISimplePropKey<Nullable<Attribute>> = PropKey.Create.simple "GraphView.GraphColor"
    member val MarginBottom: ISimplePropKey<UInt32> = PropKey.Create.simple "GraphView.MarginBottom"
    member val MarginLeft: ISimplePropKey<UInt32> = PropKey.Create.simple "GraphView.MarginLeft"
    member val ScrollOffset: ISimplePropKey<PointF> = PropKey.Create.simple "GraphView.ScrollOffset"

  type HexViewPKeys() =
    inherit ViewPKeys()

    // Properties
    member val Address: ISimplePropKey<Int64> = PropKey.Create.simple "HexView.Address"
    member val AddressWidth: ISimplePropKey<int> = PropKey.Create.simple "HexView.AddressWidth"
    member val BytesPerLine: ISimplePropKey<int> = PropKey.Create.simple "HexView.BytesPerLine"
    member val ReadOnly: ISimplePropKey<bool> = PropKey.Create.simple "HexView.ReadOnly"
    member val Source: ISimplePropKey<Stream> = PropKey.Create.simple "HexView.Source"

    // Events
    member val Edited: IEventPropKey<HexViewEditEventArgs -> unit> = PropKey.Create.event "HexView.Edited_event"
    member val PositionChanged: IEventPropKey<HexViewEventArgs -> unit> = PropKey.Create.event "HexView.PositionChanged_event"

  type LabelPKeys() =
    inherit ViewPKeys()

    // Properties
    member val HotKeySpecifier: ISimplePropKey<Rune> = PropKey.Create.simple "Label.HotKeySpecifier"
    member val Text: ISimplePropKey<string> = PropKey.Create.simple "Label.Text"

  type LegendAnnotationPKeys() =
    inherit ViewPKeys()


  type LinePKeys() =
    inherit ViewPKeys()

    // Properties
    member val Length: ISimplePropKey<Dim> = PropKey.Create.simple "Line.Length"
    member val Orientation: ISimplePropKey<Orientation> = PropKey.Create.simple "Line.Orientation"
    member val Style: ISimplePropKey<LineStyle> = PropKey.Create.simple "Line.Style"

    // Events
    member val OrientationChanged: IEventPropKey<EventArgs<Orientation> -> unit> = PropKey.Create.event "Line.OrientationChanged_event"
    member val OrientationChanging: IEventPropKey<CancelEventArgs<Orientation> -> unit> = PropKey.Create.event "Line.OrientationChanging_event"

  type LinearRangePKeys<'T>() =
    inherit ViewPKeys()

    // Properties
    member val AllowEmpty: ISimplePropKey<bool> = PropKey.Create.simple "LinearRange.AllowEmpty"
    member val FocusedOption: ISimplePropKey<int> = PropKey.Create.simple "LinearRange.FocusedOption"
    member val LegendsOrientation: ISimplePropKey<Orientation> = PropKey.Create.simple "LinearRange.LegendsOrientation"
    member val MinimumInnerSpacing: ISimplePropKey<int> = PropKey.Create.simple "LinearRange.MinimumInnerSpacing"
    member val Options: ISimplePropKey<List<LinearRangeOption<'T>>> = PropKey.Create.simple "LinearRange.Options"
    member val Orientation: ISimplePropKey<Orientation> = PropKey.Create.simple "LinearRange.Orientation"
    member val RangeAllowSingle: ISimplePropKey<bool> = PropKey.Create.simple "LinearRange.RangeAllowSingle"
    member val ShowEndSpacing: ISimplePropKey<bool> = PropKey.Create.simple "LinearRange.ShowEndSpacing"
    member val ShowLegends: ISimplePropKey<bool> = PropKey.Create.simple "LinearRange.ShowLegends"
    member val Style: ISimplePropKey<LinearRangeStyle> = PropKey.Create.simple "LinearRange.Style"
    member val Text: ISimplePropKey<string> = PropKey.Create.simple "LinearRange.Text"
    member val Type: ISimplePropKey<LinearRangeType> = PropKey.Create.simple "LinearRange.Type"
    member val UseMinimumSize: ISimplePropKey<bool> = PropKey.Create.simple "LinearRange.UseMinimumSize"

    // Events
    member val LegendsOrientationChanged: IEventPropKey<ValueChangedEventArgs<Orientation> -> unit> = PropKey.Create.event "LinearRange.LegendsOrientationChanged_event"
    member val LegendsOrientationChanging: IEventPropKey<ValueChangingEventArgs<Orientation> -> unit> = PropKey.Create.event "LinearRange.LegendsOrientationChanging_event"
    member val MinimumInnerSpacingChanged: IEventPropKey<ValueChangedEventArgs<int> -> unit> = PropKey.Create.event "LinearRange.MinimumInnerSpacingChanged_event"
    member val MinimumInnerSpacingChanging: IEventPropKey<ValueChangingEventArgs<int> -> unit> = PropKey.Create.event "LinearRange.MinimumInnerSpacingChanging_event"
    member val OptionFocused: IEventPropKey<LinearRangeEventArgs<'T> -> unit> = PropKey.Create.event "LinearRange.OptionFocused_event"
    member val OptionsChanged: IEventPropKey<LinearRangeEventArgs<'T> -> unit> = PropKey.Create.event "LinearRange.OptionsChanged_event"
    member val OrientationChanged: IEventPropKey<EventArgs<Orientation> -> unit> = PropKey.Create.event "LinearRange.OrientationChanged_event"
    member val OrientationChanging: IEventPropKey<CancelEventArgs<Orientation> -> unit> = PropKey.Create.event "LinearRange.OrientationChanging_event"
    member val ShowEndSpacingChanged: IEventPropKey<ValueChangedEventArgs<bool> -> unit> = PropKey.Create.event "LinearRange.ShowEndSpacingChanged_event"
    member val ShowEndSpacingChanging: IEventPropKey<ValueChangingEventArgs<bool> -> unit> = PropKey.Create.event "LinearRange.ShowEndSpacingChanging_event"
    member val ShowLegendsChanged: IEventPropKey<ValueChangedEventArgs<bool> -> unit> = PropKey.Create.event "LinearRange.ShowLegendsChanged_event"
    member val ShowLegendsChanging: IEventPropKey<ValueChangingEventArgs<bool> -> unit> = PropKey.Create.event "LinearRange.ShowLegendsChanging_event"
    member val TypeChanged: IEventPropKey<ValueChangedEventArgs<LinearRangeType> -> unit> = PropKey.Create.event "LinearRange.TypeChanged_event"
    member val TypeChanging: IEventPropKey<ValueChangingEventArgs<LinearRangeType> -> unit> = PropKey.Create.event "LinearRange.TypeChanging_event"
    member val UseMinimumSizeChanged: IEventPropKey<ValueChangedEventArgs<bool> -> unit> = PropKey.Create.event "LinearRange.UseMinimumSizeChanged_event"
    member val UseMinimumSizeChanging: IEventPropKey<ValueChangingEventArgs<bool> -> unit> = PropKey.Create.event "LinearRange.UseMinimumSizeChanging_event"

  type ListViewPKeys() =
    inherit ViewPKeys()

    // Properties
    member val MarkMultiple: ISimplePropKey<bool> = PropKey.Create.simple "ListView.MarkMultiple"
    member val SelectedItem: ISimplePropKey<Nullable<int>> = PropKey.Create.simple "ListView.SelectedItem"
    member val ShowMarks: ISimplePropKey<bool> = PropKey.Create.simple "ListView.ShowMarks"
    member val Source: ISimplePropKey<IListDataSource> = PropKey.Create.simple "ListView.Source"
    member val TopItem: ISimplePropKey<int> = PropKey.Create.simple "ListView.TopItem"
    member val Value: ISimplePropKey<Nullable<int>> = PropKey.Create.simple "ListView.Value"

    // Events
    member val CollectionChanged: IEventPropKey<NotifyCollectionChangedEventArgs -> unit> = PropKey.Create.event "ListView.CollectionChanged_event"
    member val RowRender: IEventPropKey<ListViewRowEventArgs -> unit> = PropKey.Create.event "ListView.RowRender_event"
    member val SourceChanged: IEventPropKey<EventArgs -> unit> = PropKey.Create.event "ListView.SourceChanged_event"
    member val ValueChanged: IEventPropKey<ValueChangedEventArgs<Nullable<int>> -> unit> = PropKey.Create.event "ListView.ValueChanged_event"
    member val ValueChanging: IEventPropKey<ValueChangingEventArgs<Nullable<int>> -> unit> = PropKey.Create.event "ListView.ValueChanging_event"

  type MarginPKeys() =
    inherit AdornmentPKeys()

    // Properties
    member val ShadowSize: ISimplePropKey<Size> = PropKey.Create.simple "Margin.ShadowSize"
    member val ShadowStyle: ISimplePropKey<ShadowStyle> = PropKey.Create.simple "Margin.ShadowStyle"

  type MenuPKeys() =
    inherit BarPKeys()

    // Properties
    member val SelectedMenuItem: IViewPropKey<MenuItem> = PropKey.Create.view "Menu.SelectedMenuItem_view"
    member val SelectedMenuItem_element: ISingleElementPropKey<IMenuItemTerminalElement> = PropKey.Create.singleElement "Menu.SelectedMenuItem_element"
    member val SuperMenuItem: IViewPropKey<MenuItem> = PropKey.Create.view "Menu.SuperMenuItem_view"
    member val SuperMenuItem_element: ISingleElementPropKey<IMenuItemTerminalElement> = PropKey.Create.singleElement "Menu.SuperMenuItem_element"

    // Events
    member val SelectedMenuItemChanged: IEventPropKey<MenuItem -> unit> = PropKey.Create.event "Menu.SelectedMenuItemChanged_event"

  type MenuBarPKeys() =
    inherit MenuPKeys()

    // Properties
    member val Key: ISimplePropKey<Key> = PropKey.Create.simple "MenuBar.Key"

    // Events
    member val KeyChanged: IEventPropKey<KeyChangedEventArgs -> unit> = PropKey.Create.event "MenuBar.KeyChanged_event"

  type NumericUpDownPKeys<'T>() =
    inherit ViewPKeys()

    // Properties
    member val Format: ISimplePropKey<string> = PropKey.Create.simple "NumericUpDown.Format"
    member val Increment: ISimplePropKey<'T> = PropKey.Create.simple "NumericUpDown.Increment"
    member val Value: ISimplePropKey<'T> = PropKey.Create.simple "NumericUpDown.Value"

    // Events
    member val FormatChanged: IEventPropKey<EventArgs<string> -> unit> = PropKey.Create.event "NumericUpDown.FormatChanged_event"
    member val IncrementChanged: IEventPropKey<EventArgs<'T> -> unit> = PropKey.Create.event "NumericUpDown.IncrementChanged_event"
    member val ValueChanged: IEventPropKey<ValueChangedEventArgs<'T> -> unit> = PropKey.Create.event "NumericUpDown.ValueChanged_event"
    member val ValueChanging: IEventPropKey<ValueChangingEventArgs<'T> -> unit> = PropKey.Create.event "NumericUpDown.ValueChanging_event"

  type PaddingPKeys() =
    inherit AdornmentPKeys()


  type PopoverBaseImplPKeys() =
    inherit ViewPKeys()

    // Properties
    member val Current: ISimplePropKey<IRunnable> = PropKey.Create.simple "PopoverBaseImpl.Current"

  type PopoverMenuPKeys() =
    inherit PopoverBaseImplPKeys()

    // Properties
    member val Key: ISimplePropKey<Key> = PropKey.Create.simple "PopoverMenu.Key"
    member val MouseFlags: ISimplePropKey<MouseFlags> = PropKey.Create.simple "PopoverMenu.MouseFlags"
    member val Root: IViewPropKey<Menu> = PropKey.Create.view "PopoverMenu.Root_view"
    member val Root_element: ISingleElementPropKey<IMenuTerminalElement> = PropKey.Create.singleElement "PopoverMenu.Root_element"

    // Events
    member val KeyChanged: IEventPropKey<KeyChangedEventArgs -> unit> = PropKey.Create.event "PopoverMenu.KeyChanged_event"

  type ProgressBarPKeys() =
    inherit ViewPKeys()

    // Properties
    member val BidirectionalMarquee: ISimplePropKey<bool> = PropKey.Create.simple "ProgressBar.BidirectionalMarquee"
    member val Fraction: ISimplePropKey<Single> = PropKey.Create.simple "ProgressBar.Fraction"
    member val ProgressBarFormat: ISimplePropKey<ProgressBarFormat> = PropKey.Create.simple "ProgressBar.ProgressBarFormat"
    member val ProgressBarStyle: ISimplePropKey<ProgressBarStyle> = PropKey.Create.simple "ProgressBar.ProgressBarStyle"
    member val SegmentCharacter: ISimplePropKey<Rune> = PropKey.Create.simple "ProgressBar.SegmentCharacter"
    member val Text: ISimplePropKey<string> = PropKey.Create.simple "ProgressBar.Text"

  type RunnablePKeys() =
    inherit ViewPKeys()

    // Properties
    member val Result: ISimplePropKey<Object> = PropKey.Create.simple "Runnable.Result"
    member val StopRequested: ISimplePropKey<bool> = PropKey.Create.simple "Runnable.StopRequested"

    // Events
    member val IsModalChanged: IEventPropKey<EventArgs<bool> -> unit> = PropKey.Create.event "Runnable.IsModalChanged_event"
    member val IsRunningChanged: IEventPropKey<EventArgs<bool> -> unit> = PropKey.Create.event "Runnable.IsRunningChanged_event"
    member val IsRunningChanging: IEventPropKey<CancelEventArgs<bool> -> unit> = PropKey.Create.event "Runnable.IsRunningChanging_event"

  type RunnablePKeys<'TResult>() =
    inherit RunnablePKeys()

    // Properties
    member val Result: ISimplePropKey<'TResult> = PropKey.Create.simple "Runnable.Result"

  type ScrollBarPKeys() =
    inherit ViewPKeys()

    // Properties
    member val AutoShow: ISimplePropKey<bool> = PropKey.Create.simple "ScrollBar.AutoShow"
    member val Increment: ISimplePropKey<int> = PropKey.Create.simple "ScrollBar.Increment"
    member val Orientation: ISimplePropKey<Orientation> = PropKey.Create.simple "ScrollBar.Orientation"
    member val ScrollableContentSize: ISimplePropKey<int> = PropKey.Create.simple "ScrollBar.ScrollableContentSize"
    member val Value: ISimplePropKey<int> = PropKey.Create.simple "ScrollBar.Value"
    member val VisibleContentSize: ISimplePropKey<int> = PropKey.Create.simple "ScrollBar.VisibleContentSize"

    // Events
    member val OrientationChanged: IEventPropKey<EventArgs<Orientation> -> unit> = PropKey.Create.event "ScrollBar.OrientationChanged_event"
    member val OrientationChanging: IEventPropKey<CancelEventArgs<Orientation> -> unit> = PropKey.Create.event "ScrollBar.OrientationChanging_event"
    member val ScrollableContentSizeChanged: IEventPropKey<EventArgs<int> -> unit> = PropKey.Create.event "ScrollBar.ScrollableContentSizeChanged_event"
    member val Scrolled: IEventPropKey<EventArgs<int> -> unit> = PropKey.Create.event "ScrollBar.Scrolled_event"
    member val SliderPositionChanged: IEventPropKey<EventArgs<int> -> unit> = PropKey.Create.event "ScrollBar.SliderPositionChanged_event"
    member val ValueChanged: IEventPropKey<ValueChangedEventArgs<int> -> unit> = PropKey.Create.event "ScrollBar.ValueChanged_event"
    member val ValueChanging: IEventPropKey<ValueChangingEventArgs<int> -> unit> = PropKey.Create.event "ScrollBar.ValueChanging_event"

  type ScrollSliderPKeys() =
    inherit ViewPKeys()

    // Properties
    member val Orientation: ISimplePropKey<Orientation> = PropKey.Create.simple "ScrollSlider.Orientation"
    member val Position: ISimplePropKey<int> = PropKey.Create.simple "ScrollSlider.Position"
    member val Size: ISimplePropKey<int> = PropKey.Create.simple "ScrollSlider.Size"
    member val SliderPadding: ISimplePropKey<int> = PropKey.Create.simple "ScrollSlider.SliderPadding"
    member val VisibleContentSize: ISimplePropKey<int> = PropKey.Create.simple "ScrollSlider.VisibleContentSize"

    // Events
    member val OrientationChanged: IEventPropKey<EventArgs<Orientation> -> unit> = PropKey.Create.event "ScrollSlider.OrientationChanged_event"
    member val OrientationChanging: IEventPropKey<CancelEventArgs<Orientation> -> unit> = PropKey.Create.event "ScrollSlider.OrientationChanging_event"
    member val PositionChanged: IEventPropKey<EventArgs<int> -> unit> = PropKey.Create.event "ScrollSlider.PositionChanged_event"
    member val PositionChanging: IEventPropKey<CancelEventArgs<int> -> unit> = PropKey.Create.event "ScrollSlider.PositionChanging_event"
    member val Scrolled: IEventPropKey<EventArgs<int> -> unit> = PropKey.Create.event "ScrollSlider.Scrolled_event"

  type SelectorBasePKeys() =
    inherit ViewPKeys()

    // Properties
    member val DoubleClickAccepts: ISimplePropKey<bool> = PropKey.Create.simple "SelectorBase.DoubleClickAccepts"
    member val HorizontalSpace: ISimplePropKey<int> = PropKey.Create.simple "SelectorBase.HorizontalSpace"
    member val Labels: ISimplePropKey<IReadOnlyList<string>> = PropKey.Create.simple "SelectorBase.Labels"
    member val Orientation: ISimplePropKey<Orientation> = PropKey.Create.simple "SelectorBase.Orientation"
    member val Styles: ISimplePropKey<SelectorStyles> = PropKey.Create.simple "SelectorBase.Styles"
    member val Value: ISimplePropKey<Nullable<int>> = PropKey.Create.simple "SelectorBase.Value"
    member val Values: ISimplePropKey<IReadOnlyList<int>> = PropKey.Create.simple "SelectorBase.Values"

    // Events
    member val OrientationChanged: IEventPropKey<EventArgs<Orientation> -> unit> = PropKey.Create.event "SelectorBase.OrientationChanged_event"
    member val OrientationChanging: IEventPropKey<CancelEventArgs<Orientation> -> unit> = PropKey.Create.event "SelectorBase.OrientationChanging_event"
    member val ValueChanged: IEventPropKey<ValueChangedEventArgs<Nullable<int>> -> unit> = PropKey.Create.event "SelectorBase.ValueChanged_event"
    member val ValueChanging: IEventPropKey<ValueChangingEventArgs<Nullable<int>> -> unit> = PropKey.Create.event "SelectorBase.ValueChanging_event"

  type FlagSelectorPKeys() =
    inherit SelectorBasePKeys()

    // Properties
    member val Value: ISimplePropKey<Nullable<int>> = PropKey.Create.simple "FlagSelector.Value"

  type OptionSelectorPKeys() =
    inherit SelectorBasePKeys()

    // Properties
    member val Cursor: ISimplePropKey<int> = PropKey.Create.simple "OptionSelector.Cursor"

  type FlagSelectorPKeys<'TFlagsEnum when 'TFlagsEnum: struct and 'TFlagsEnum: (new: unit -> 'TFlagsEnum) and 'TFlagsEnum:> Enum and 'TFlagsEnum:> ValueType>() =
    inherit FlagSelectorPKeys()

    // Properties
    member val Value: ISimplePropKey<Nullable<'TFlagsEnum>> = PropKey.Create.simple "FlagSelector.Value"

    // Events
    member val ValueChanged: IEventPropKey<EventArgs<Nullable<'TFlagsEnum>> -> unit> = PropKey.Create.event "FlagSelector.ValueChanged_event"

  type OptionSelectorPKeys<'TEnum when 'TEnum: struct and 'TEnum: (new: unit -> 'TEnum) and 'TEnum:> Enum and 'TEnum:> ValueType>() =
    inherit OptionSelectorPKeys()

    // Properties
    member val Value: ISimplePropKey<Nullable<'TEnum>> = PropKey.Create.simple "OptionSelector.Value"
    member val Values: ISimplePropKey<IReadOnlyList<int>> = PropKey.Create.simple "OptionSelector.Values"

    // Events
    member val ValueChanged: IEventPropKey<EventArgs<Nullable<'TEnum>> -> unit> = PropKey.Create.event "OptionSelector.ValueChanged_event"

  type ShortcutPKeys() =
    inherit ViewPKeys()

    // Properties
    member val Action: ISimplePropKey<Action> = PropKey.Create.simple "Shortcut.Action"
    member val AlignmentModes: ISimplePropKey<AlignmentModes> = PropKey.Create.simple "Shortcut.AlignmentModes"
    member val BindKeyToApplication: ISimplePropKey<bool> = PropKey.Create.simple "Shortcut.BindKeyToApplication"
    member val CommandView: IViewPropKey<View> = PropKey.Create.view "Shortcut.CommandView_view"
    member val CommandView_element: ISingleElementPropKey<ITerminalElement> = PropKey.Create.singleElement "Shortcut.CommandView_element"
    member val ForceFocusColors: ISimplePropKey<bool> = PropKey.Create.simple "Shortcut.ForceFocusColors"
    member val HelpText: ISimplePropKey<string> = PropKey.Create.simple "Shortcut.HelpText"
    member val Key: ISimplePropKey<Key> = PropKey.Create.simple "Shortcut.Key"
    member val MinimumKeyTextSize: ISimplePropKey<int> = PropKey.Create.simple "Shortcut.MinimumKeyTextSize"
    member val Orientation: ISimplePropKey<Orientation> = PropKey.Create.simple "Shortcut.Orientation"
    member val Text: ISimplePropKey<string> = PropKey.Create.simple "Shortcut.Text"

    // Events
    member val OrientationChanged: IEventPropKey<EventArgs<Orientation> -> unit> = PropKey.Create.event "Shortcut.OrientationChanged_event"
    member val OrientationChanging: IEventPropKey<CancelEventArgs<Orientation> -> unit> = PropKey.Create.event "Shortcut.OrientationChanging_event"

  type MenuItemPKeys() =
    inherit ShortcutPKeys()

    // Properties
    member val Command: ISimplePropKey<Command> = PropKey.Create.simple "MenuItem.Command"
    member val SubMenu: IViewPropKey<Menu> = PropKey.Create.view "MenuItem.SubMenu_view"
    member val SubMenu_element: ISingleElementPropKey<IMenuTerminalElement> = PropKey.Create.singleElement "MenuItem.SubMenu_element"
    member val TargetView: IViewPropKey<View> = PropKey.Create.view "MenuItem.TargetView_view"
    member val TargetView_element: ISingleElementPropKey<ITerminalElement> = PropKey.Create.singleElement "MenuItem.TargetView_element"

  type MenuBarItemPKeys() =
    inherit MenuItemPKeys()

    // Properties
    member val PopoverMenu: IViewPropKey<PopoverMenu> = PropKey.Create.view "MenuBarItem.PopoverMenu_view"
    member val PopoverMenu_element: ISingleElementPropKey<IPopoverMenuTerminalElement> = PropKey.Create.singleElement "MenuBarItem.PopoverMenu_element"
    member val PopoverMenuOpen: ISimplePropKey<bool> = PropKey.Create.simple "MenuBarItem.PopoverMenuOpen"
    member val SubMenu: IViewPropKey<Menu> = PropKey.Create.view "MenuBarItem.SubMenu_view"
    member val SubMenu_element: ISingleElementPropKey<IMenuTerminalElement> = PropKey.Create.singleElement "MenuBarItem.SubMenu_element"

    // Events
    member val PopoverMenuOpenChanged: IEventPropKey<EventArgs<bool> -> unit> = PropKey.Create.event "MenuBarItem.PopoverMenuOpenChanged_event"

  type SpinnerViewPKeys() =
    inherit ViewPKeys()

    // Properties
    member val AutoSpin: ISimplePropKey<bool> = PropKey.Create.simple "SpinnerView.AutoSpin"
    member val Sequence: ISimplePropKey<String[]> = PropKey.Create.simple "SpinnerView.Sequence"
    member val SpinBounce: ISimplePropKey<bool> = PropKey.Create.simple "SpinnerView.SpinBounce"
    member val SpinDelay: ISimplePropKey<int> = PropKey.Create.simple "SpinnerView.SpinDelay"
    member val SpinReverse: ISimplePropKey<bool> = PropKey.Create.simple "SpinnerView.SpinReverse"
    member val Style: ISimplePropKey<SpinnerStyle> = PropKey.Create.simple "SpinnerView.Style"

  type StatusBarPKeys() =
    inherit BarPKeys()


  type TabPKeys() =
    inherit ViewPKeys()

    // Properties
    member val DisplayText: ISimplePropKey<string> = PropKey.Create.simple "Tab.DisplayText"
    member val View: IViewPropKey<View> = PropKey.Create.view "Tab.View_view"
    member val View_element: ISingleElementPropKey<ITerminalElement> = PropKey.Create.singleElement "Tab.View_element"

  type TabViewPKeys() =
    inherit ViewPKeys()

    // Properties
    member val MaxTabTextWidth: ISimplePropKey<UInt32> = PropKey.Create.simple "TabView.MaxTabTextWidth"
    member val SelectedTab: IViewPropKey<Tab> = PropKey.Create.view "TabView.SelectedTab_view"
    member val SelectedTab_element: ISingleElementPropKey<ITabTerminalElement> = PropKey.Create.singleElement "TabView.SelectedTab_element"
    member val Style: ISimplePropKey<TabStyle> = PropKey.Create.simple "TabView.Style"
    member val TabScrollOffset: ISimplePropKey<int> = PropKey.Create.simple "TabView.TabScrollOffset"

    // Events
    member val SelectedTabChanged: IEventPropKey<TabChangedEventArgs -> unit> = PropKey.Create.event "TabView.SelectedTabChanged_event"
    member val TabClicked: IEventPropKey<TabMouseEventArgs -> unit> = PropKey.Create.event "TabView.TabClicked_event"

  type TableViewPKeys() =
    inherit ViewPKeys()

    // Properties
    member val CellActivationKey: ISimplePropKey<KeyCode> = PropKey.Create.simple "TableView.CellActivationKey"
    member val CollectionNavigator: ISimplePropKey<ICollectionNavigator> = PropKey.Create.simple "TableView.CollectionNavigator"
    member val ColumnOffset: ISimplePropKey<int> = PropKey.Create.simple "TableView.ColumnOffset"
    member val FullRowSelect: ISimplePropKey<bool> = PropKey.Create.simple "TableView.FullRowSelect"
    member val MaxCellWidth: ISimplePropKey<int> = PropKey.Create.simple "TableView.MaxCellWidth"
    member val MinCellWidth: ISimplePropKey<int> = PropKey.Create.simple "TableView.MinCellWidth"
    member val MultiSelect: ISimplePropKey<bool> = PropKey.Create.simple "TableView.MultiSelect"
    member val NullSymbol: ISimplePropKey<string> = PropKey.Create.simple "TableView.NullSymbol"
    member val RowOffset: ISimplePropKey<int> = PropKey.Create.simple "TableView.RowOffset"
    member val SelectedColumn: ISimplePropKey<int> = PropKey.Create.simple "TableView.SelectedColumn"
    member val SelectedRow: ISimplePropKey<int> = PropKey.Create.simple "TableView.SelectedRow"
    member val SeparatorSymbol: ISimplePropKey<Char> = PropKey.Create.simple "TableView.SeparatorSymbol"
    member val Style: ISimplePropKey<TableStyle> = PropKey.Create.simple "TableView.Style"
    member val Table: ISimplePropKey<ITableSource> = PropKey.Create.simple "TableView.Table"

    // Events
    member val CellActivated: IEventPropKey<CellActivatedEventArgs -> unit> = PropKey.Create.event "TableView.CellActivated_event"
    member val CellToggled: IEventPropKey<CellToggledEventArgs -> unit> = PropKey.Create.event "TableView.CellToggled_event"
    member val SelectedCellChanged: IEventPropKey<SelectedCellChangedEventArgs -> unit> = PropKey.Create.event "TableView.SelectedCellChanged_event"

  type TextFieldPKeys() =
    inherit ViewPKeys()

    // Properties
    member val Autocomplete: ISimplePropKey<IAutocomplete> = PropKey.Create.simple "TextField.Autocomplete"
    member val InsertionPoint: ISimplePropKey<int> = PropKey.Create.simple "TextField.InsertionPoint"
    member val ReadOnly: ISimplePropKey<bool> = PropKey.Create.simple "TextField.ReadOnly"
    member val Secret: ISimplePropKey<bool> = PropKey.Create.simple "TextField.Secret"
    member val SelectWordOnlyOnDoubleClick: ISimplePropKey<bool> = PropKey.Create.simple "TextField.SelectWordOnlyOnDoubleClick"
    member val SelectedStart: ISimplePropKey<int> = PropKey.Create.simple "TextField.SelectedStart"
    member val Text: ISimplePropKey<string> = PropKey.Create.simple "TextField.Text"
    member val UseSameRuneTypeForWords: ISimplePropKey<bool> = PropKey.Create.simple "TextField.UseSameRuneTypeForWords"
    member val Used: ISimplePropKey<bool> = PropKey.Create.simple "TextField.Used"
    member val Value: ISimplePropKey<string> = PropKey.Create.simple "TextField.Value"

    // Events
    member val TextChanging: IEventPropKey<ResultEventArgs<string> -> unit> = PropKey.Create.event "TextField.TextChanging_event"
    member val ValueChanged: IEventPropKey<ValueChangedEventArgs<string> -> unit> = PropKey.Create.event "TextField.ValueChanged_event"
    member val ValueChanging: IEventPropKey<ValueChangingEventArgs<string> -> unit> = PropKey.Create.event "TextField.ValueChanging_event"

  type DateFieldPKeys() =
    inherit TextFieldPKeys()

    // Properties
    member val Culture: ISimplePropKey<CultureInfo> = PropKey.Create.simple "DateField.Culture"
    member val InsertionPoint: ISimplePropKey<int> = PropKey.Create.simple "DateField.InsertionPoint"
    member val Value: ISimplePropKey<Nullable<DateTime>> = PropKey.Create.simple "DateField.Value"

    // Events
    member val ValueChanged: IEventPropKey<ValueChangedEventArgs<Nullable<DateTime>> -> unit> = PropKey.Create.event "DateField.ValueChanged_event"
    member val ValueChanging: IEventPropKey<ValueChangingEventArgs<Nullable<DateTime>> -> unit> = PropKey.Create.event "DateField.ValueChanging_event"

  type TextValidateFieldPKeys() =
    inherit ViewPKeys()

    // Properties
    member val Provider: ISimplePropKey<ITextValidateProvider> = PropKey.Create.simple "TextValidateField.Provider"
    member val Text: ISimplePropKey<string> = PropKey.Create.simple "TextValidateField.Text"

  type TextViewPKeys() =
    inherit ViewPKeys()

    // Properties
    member val EnterKeyAddsLine: ISimplePropKey<bool> = PropKey.Create.simple "TextView.EnterKeyAddsLine"
    member val InheritsPreviousAttribute: ISimplePropKey<bool> = PropKey.Create.simple "TextView.InheritsPreviousAttribute"
    member val InsertionPoint: ISimplePropKey<Point> = PropKey.Create.simple "TextView.InsertionPoint"
    member val IsDirty: ISimplePropKey<bool> = PropKey.Create.simple "TextView.IsDirty"
    member val IsSelecting: ISimplePropKey<bool> = PropKey.Create.simple "TextView.IsSelecting"
    member val LeftColumn: ISimplePropKey<int> = PropKey.Create.simple "TextView.LeftColumn"
    member val Multiline: ISimplePropKey<bool> = PropKey.Create.simple "TextView.Multiline"
    member val ReadOnly: ISimplePropKey<bool> = PropKey.Create.simple "TextView.ReadOnly"
    member val ScrollBars: ISimplePropKey<bool> = PropKey.Create.simple "TextView.ScrollBars"
    member val SelectWordOnlyOnDoubleClick: ISimplePropKey<bool> = PropKey.Create.simple "TextView.SelectWordOnlyOnDoubleClick"
    member val SelectionStartColumn: ISimplePropKey<int> = PropKey.Create.simple "TextView.SelectionStartColumn"
    member val SelectionStartRow: ISimplePropKey<int> = PropKey.Create.simple "TextView.SelectionStartRow"
    member val TabKeyAddsTab: ISimplePropKey<bool> = PropKey.Create.simple "TextView.TabKeyAddsTab"
    member val TabWidth: ISimplePropKey<int> = PropKey.Create.simple "TextView.TabWidth"
    member val Text: ISimplePropKey<string> = PropKey.Create.simple "TextView.Text"
    member val TopRow: ISimplePropKey<int> = PropKey.Create.simple "TextView.TopRow"
    member val UseSameRuneTypeForWords: ISimplePropKey<bool> = PropKey.Create.simple "TextView.UseSameRuneTypeForWords"
    member val Used: ISimplePropKey<bool> = PropKey.Create.simple "TextView.Used"
    member val WordWrap: ISimplePropKey<bool> = PropKey.Create.simple "TextView.WordWrap"

    // Events
    member val ContentsChanged: IEventPropKey<ContentsChangedEventArgs -> unit> = PropKey.Create.event "TextView.ContentsChanged_event"
    member val DrawNormalColor: IEventPropKey<CellEventArgs -> unit> = PropKey.Create.event "TextView.DrawNormalColor_event"
    member val DrawReadOnlyColor: IEventPropKey<CellEventArgs -> unit> = PropKey.Create.event "TextView.DrawReadOnlyColor_event"
    member val DrawSelectionColor: IEventPropKey<CellEventArgs -> unit> = PropKey.Create.event "TextView.DrawSelectionColor_event"
    member val DrawUsedColor: IEventPropKey<CellEventArgs -> unit> = PropKey.Create.event "TextView.DrawUsedColor_event"
    member val UnwrappedCursorPosition: IEventPropKey<Point -> unit> = PropKey.Create.event "TextView.UnwrappedCursorPosition_event"

  type TimeFieldPKeys() =
    inherit TextFieldPKeys()

    // Properties
    member val InsertionPoint: ISimplePropKey<int> = PropKey.Create.simple "TimeField.InsertionPoint"
    member val IsShortFormat: ISimplePropKey<bool> = PropKey.Create.simple "TimeField.IsShortFormat"
    member val Value: ISimplePropKey<TimeSpan> = PropKey.Create.simple "TimeField.Value"

    // Events
    member val ValueChanged: IEventPropKey<ValueChangedEventArgs<TimeSpan> -> unit> = PropKey.Create.event "TimeField.ValueChanged_event"
    member val ValueChanging: IEventPropKey<ValueChangingEventArgs<TimeSpan> -> unit> = PropKey.Create.event "TimeField.ValueChanging_event"

  type TreeViewPKeys<'T when 'T: not struct>() =
    inherit ViewPKeys()

    // Properties
    member val AllowLetterBasedNavigation: ISimplePropKey<bool> = PropKey.Create.simple "TreeView.AllowLetterBasedNavigation"
    member val AspectGetter: ISimplePropKey<AspectGetterDelegate<'T>> = PropKey.Create.simple "TreeView.AspectGetter"
    member val ColorGetter: ISimplePropKey<Func<'T, Scheme>> = PropKey.Create.simple "TreeView.ColorGetter"
    member val MaxDepth: ISimplePropKey<int> = PropKey.Create.simple "TreeView.MaxDepth"
    member val MultiSelect: ISimplePropKey<bool> = PropKey.Create.simple "TreeView.MultiSelect"
    member val ObjectActivationButton: ISimplePropKey<Nullable<MouseFlags>> = PropKey.Create.simple "TreeView.ObjectActivationButton"
    member val ObjectActivationKey: ISimplePropKey<KeyCode> = PropKey.Create.simple "TreeView.ObjectActivationKey"
    member val ScrollOffsetHorizontal: ISimplePropKey<int> = PropKey.Create.simple "TreeView.ScrollOffsetHorizontal"
    member val ScrollOffsetVertical: ISimplePropKey<int> = PropKey.Create.simple "TreeView.ScrollOffsetVertical"
    member val SelectedObject: ISimplePropKey<'T> = PropKey.Create.simple "TreeView.SelectedObject"
    member val Style: ISimplePropKey<TreeStyle> = PropKey.Create.simple "TreeView.Style"
    member val TreeBuilder: ISimplePropKey<ITreeBuilder<'T>> = PropKey.Create.simple "TreeView.TreeBuilder"

    // Events
    member val DrawLine: IEventPropKey<DrawTreeViewLineEventArgs<'T> -> unit> = PropKey.Create.event "TreeView.DrawLine_event"
    member val ObjectActivated: IEventPropKey<ObjectActivatedEventArgs<'T> -> unit> = PropKey.Create.event "TreeView.ObjectActivated_event"
    member val SelectionChanged: IEventPropKey<SelectionChangedEventArgs<'T> -> unit> = PropKey.Create.event "TreeView.SelectionChanged_event"

  type WindowPKeys() =
    inherit RunnablePKeys()


  type WizardStepPKeys() =
    inherit ViewPKeys()

    // Properties
    member val BackButtonText: ISimplePropKey<string> = PropKey.Create.simple "WizardStep.BackButtonText"
    member val HelpText: ISimplePropKey<string> = PropKey.Create.simple "WizardStep.HelpText"
    member val NextButtonText: ISimplePropKey<string> = PropKey.Create.simple "WizardStep.NextButtonText"

  module internal IMouseHoldRepeaterInterface =
    // Properties
    let Timeout: ISimplePropKey<Timeout> = PropKey.Create.simple "IMouseHoldRepeaterInterface.Timeout"

    // Events
    let MouseIsHeldDownTick: IEventPropKey<CancelEventArgs<Mouse> -> unit> = PropKey.Create.event "IMouseHoldRepeaterInterface.MouseIsHeldDownTick_event"

  module internal IOrientationInterface =
    // Properties
    let Orientation: ISimplePropKey<Orientation> = PropKey.Create.simple "IOrientationInterface.Orientation"

    // Events
    let OrientationChanged: IEventPropKey<EventArgs<Orientation> -> unit> = PropKey.Create.event "IOrientationInterface.OrientationChanged_event"

    let OrientationChanging: IEventPropKey<CancelEventArgs<Orientation> -> unit> = PropKey.Create.event "IOrientationInterface.OrientationChanging_event"

  module internal IValueInterface =
    // Properties
    let Value<'TValue>: ISimplePropKey<'TValue> = PropKey.Create.simple "IValueInterface.Value"

    // Events
    let ValueChanged<'TValue>: IEventPropKey<ValueChangedEventArgs<'TValue> -> unit> = PropKey.Create.event "IValueInterface.ValueChanged_event"

    let ValueChanging<'TValue>: IEventPropKey<ValueChangingEventArgs<'TValue> -> unit> = PropKey.Create.event "IValueInterface.ValueChanging_event"


  let View = ViewPKeys()
  let Adornment = AdornmentPKeys()
  let AttributePicker = AttributePickerPKeys()
  let Bar = BarPKeys()
  let Border = BorderPKeys()
  let Button = ButtonPKeys()
  let CharMap = CharMapPKeys()
  let CheckBox = CheckBoxPKeys()
  let ColorPicker = ColorPickerPKeys()
  let ColorPicker16 = ColorPicker16PKeys()
  let ComboBox = ComboBoxPKeys()
  let DatePicker = DatePickerPKeys()
  let FrameView = FrameViewPKeys()
  let GraphView = GraphViewPKeys()
  let HexView = HexViewPKeys()
  let Label = LabelPKeys()
  let LegendAnnotation = LegendAnnotationPKeys()
  let Line = LinePKeys()
  let LinearRange<'T> = LinearRangePKeys<'T>()
  let ListView = ListViewPKeys()
  let Margin = MarginPKeys()
  let Menu = MenuPKeys()
  let MenuBar = MenuBarPKeys()
  let NumericUpDown<'T> = NumericUpDownPKeys<'T>()
  let Padding = PaddingPKeys()
  let PopoverBaseImpl = PopoverBaseImplPKeys()
  let PopoverMenu = PopoverMenuPKeys()
  let ProgressBar = ProgressBarPKeys()
  let Runnable = RunnablePKeys()
  let Runnable'<'TResult> = RunnablePKeys<'TResult>()
  let ScrollBar = ScrollBarPKeys()
  let ScrollSlider = ScrollSliderPKeys()
  let SelectorBase = SelectorBasePKeys()
  let FlagSelector = FlagSelectorPKeys()
  let OptionSelector = OptionSelectorPKeys()
  let FlagSelector'<'TFlagsEnum when 'TFlagsEnum: struct and 'TFlagsEnum: (new: unit -> 'TFlagsEnum) and 'TFlagsEnum:> Enum and 'TFlagsEnum:> ValueType> = FlagSelectorPKeys<'TFlagsEnum>()
  let OptionSelector'<'TEnum when 'TEnum: struct and 'TEnum: (new: unit -> 'TEnum) and 'TEnum:> Enum and 'TEnum:> ValueType> = OptionSelectorPKeys<'TEnum>()
  let Shortcut = ShortcutPKeys()
  let MenuItem = MenuItemPKeys()
  let MenuBarItem = MenuBarItemPKeys()
  let SpinnerView = SpinnerViewPKeys()
  let StatusBar = StatusBarPKeys()
  let Tab = TabPKeys()
  let TabView = TabViewPKeys()
  let TableView = TableViewPKeys()
  let TextField = TextFieldPKeys()
  let DateField = DateFieldPKeys()
  let TextValidateField = TextValidateFieldPKeys()
  let TextView = TextViewPKeys()
  let TimeField = TimeFieldPKeys()
  let TreeView<'T when 'T: not struct> = TreeViewPKeys<'T>()
  let Window = WindowPKeys()
  let WizardStep = WizardStepPKeys()