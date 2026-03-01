namespace Terminal.Gui.Elmish

open System
open System.Collections.Generic
open Terminal.Gui.App
open Terminal.Gui.Views

[<RequireQualifiedAccess>]
module internal PKey =

  type ViewPKeys() =

    // Properties
    member val Arrangement: TypedPropKey<Terminal.Gui.ViewBase.ViewArrangement> = PropKey.Create.simple "View.Arrangement"
    member val AssignHotKeys: TypedPropKey<bool> = PropKey.Create.simple "View.AssignHotKeys"
    member val BorderStyle: TypedPropKey<Terminal.Gui.Drawing.LineStyle> = PropKey.Create.simple "View.BorderStyle"
    member val CanFocus: TypedPropKey<bool> = PropKey.Create.simple "View.CanFocus"
    member val ContentSizeTracksViewport: TypedPropKey<bool> = PropKey.Create.simple "View.ContentSizeTracksViewport"
    member val Cursor: TypedPropKey<Terminal.Gui.Drivers.Cursor> = PropKey.Create.simple "View.Cursor"
    member val Data: TypedPropKey<System.Object> = PropKey.Create.simple "View.Data"
    member val Enabled: TypedPropKey<bool> = PropKey.Create.simple "View.Enabled"
    member val Frame: TypedPropKey<System.Drawing.Rectangle> = PropKey.Create.simple "View.Frame"
    member val HasFocus: TypedPropKey<bool> = PropKey.Create.simple "View.HasFocus"
    member val Height: TypedPropKey<Terminal.Gui.ViewBase.Dim> = PropKey.Create.simple "View.Height"
    member val HotKey: TypedPropKey<Terminal.Gui.Input.Key> = PropKey.Create.simple "View.HotKey"
    member val HotKeySpecifier: TypedPropKey<System.Text.Rune> = PropKey.Create.simple "View.HotKeySpecifier"
    member val Id: TypedPropKey<string> = PropKey.Create.simple "View.Id"
    member val IsInitialized: TypedPropKey<bool> = PropKey.Create.simple "View.IsInitialized"
    member val MouseHighlightStates: TypedPropKey<Terminal.Gui.ViewBase.MouseState> = PropKey.Create.simple "View.MouseHighlightStates"
    member val MouseHoldRepeat: TypedPropKey<Nullable<Terminal.Gui.Input.MouseFlags>> = PropKey.Create.simple "View.MouseHoldRepeat"
    member val MousePositionTracking: TypedPropKey<bool> = PropKey.Create.simple "View.MousePositionTracking"
    member val PreserveTrailingSpaces: TypedPropKey<bool> = PropKey.Create.simple "View.PreserveTrailingSpaces"
    member val SchemeName: TypedPropKey<string> = PropKey.Create.simple "View.SchemeName"
    member val ShadowStyle: TypedPropKey<Terminal.Gui.ViewBase.ShadowStyle> = PropKey.Create.simple "View.ShadowStyle"
    member val SuperViewRendersLineCanvas: TypedPropKey<bool> = PropKey.Create.simple "View.SuperViewRendersLineCanvas"
    member val TabStop: TypedPropKey<Nullable<Terminal.Gui.ViewBase.TabBehavior>> = PropKey.Create.simple "View.TabStop"
    member val Text: TypedPropKey<string> = PropKey.Create.simple "View.Text"
    member val TextAlignment: TypedPropKey<Terminal.Gui.ViewBase.Alignment> = PropKey.Create.simple "View.TextAlignment"
    member val TextDirection: TypedPropKey<Terminal.Gui.Text.TextDirection> = PropKey.Create.simple "View.TextDirection"
    member val Title: TypedPropKey<string> = PropKey.Create.simple "View.Title"
    member val UsedHotKeys: TypedPropKey<HashSet<Terminal.Gui.Input.Key>> = PropKey.Create.simple "View.UsedHotKeys"
    member val ValidatePosDim: TypedPropKey<bool> = PropKey.Create.simple "View.ValidatePosDim"
    member val VerticalTextAlignment: TypedPropKey<Terminal.Gui.ViewBase.Alignment> = PropKey.Create.simple "View.VerticalTextAlignment"
    member val Viewport: TypedPropKey<System.Drawing.Rectangle> = PropKey.Create.simple "View.Viewport"
    member val ViewportSettings: TypedPropKey<Terminal.Gui.ViewBase.ViewportSettingsFlags> = PropKey.Create.simple "View.ViewportSettings"
    member val Visible: TypedPropKey<bool> = PropKey.Create.simple "View.Visible"
    member val Width: TypedPropKey<Terminal.Gui.ViewBase.Dim> = PropKey.Create.simple "View.Width"

    // Events
    member val Accepted: TypedPropKey<Terminal.Gui.Input.CommandEventArgs -> unit> = PropKey.Create.event "View.Accepted_event"
    member val Accepting: TypedPropKey<Terminal.Gui.Input.CommandEventArgs -> unit> = PropKey.Create.event "View.Accepting_event"
    member val Activating: TypedPropKey<Terminal.Gui.Input.CommandEventArgs -> unit> = PropKey.Create.event "View.Activating_event"
    member val AdvancingFocus: TypedPropKey<Terminal.Gui.ViewBase.AdvanceFocusEventArgs -> unit> = PropKey.Create.event "View.AdvancingFocus_event"
    member val BorderStyleChanged: TypedPropKey<System.EventArgs -> unit> = PropKey.Create.event "View.BorderStyleChanged_event"
    member val CanFocusChanged: TypedPropKey<System.EventArgs -> unit> = PropKey.Create.event "View.CanFocusChanged_event"
    member val ClearedViewport: TypedPropKey<Terminal.Gui.ViewBase.DrawEventArgs -> unit> = PropKey.Create.event "View.ClearedViewport_event"
    member val ClearingViewport: TypedPropKey<Terminal.Gui.ViewBase.DrawEventArgs -> unit> = PropKey.Create.event "View.ClearingViewport_event"
    member val CommandNotBound: TypedPropKey<Terminal.Gui.Input.CommandEventArgs -> unit> = PropKey.Create.event "View.CommandNotBound_event"
    member val ContentSizeChanged: TypedPropKey<ValueChangedEventArgs<Nullable<System.Drawing.Size>> -> unit> = PropKey.Create.event "View.ContentSizeChanged_event"
    member val ContentSizeChanging: TypedPropKey<ValueChangingEventArgs<Nullable<System.Drawing.Size>> -> unit> = PropKey.Create.event "View.ContentSizeChanging_event"
    member val Disposing: TypedPropKey<System.EventArgs -> unit> = PropKey.Create.event "View.Disposing_event"
    member val DrawComplete: TypedPropKey<Terminal.Gui.ViewBase.DrawEventArgs -> unit> = PropKey.Create.event "View.DrawComplete_event"
    member val DrawingContent: TypedPropKey<Terminal.Gui.ViewBase.DrawEventArgs -> unit> = PropKey.Create.event "View.DrawingContent_event"
    member val DrawingSubViews: TypedPropKey<Terminal.Gui.ViewBase.DrawEventArgs -> unit> = PropKey.Create.event "View.DrawingSubViews_event"
    member val DrawingText: TypedPropKey<Terminal.Gui.ViewBase.DrawEventArgs -> unit> = PropKey.Create.event "View.DrawingText_event"
    member val DrewText: TypedPropKey<System.EventArgs -> unit> = PropKey.Create.event "View.DrewText_event"
    member val EnabledChanged: TypedPropKey<System.EventArgs -> unit> = PropKey.Create.event "View.EnabledChanged_event"
    member val FocusedChanged: TypedPropKey<Terminal.Gui.ViewBase.HasFocusEventArgs -> unit> = PropKey.Create.event "View.FocusedChanged_event"
    member val FrameChanged: TypedPropKey<EventArgs<System.Drawing.Rectangle> -> unit> = PropKey.Create.event "View.FrameChanged_event"
    member val GettingAttributeForRole: TypedPropKey<Terminal.Gui.Drawing.VisualRoleEventArgs -> unit> = PropKey.Create.event "View.GettingAttributeForRole_event"
    member val GettingScheme: TypedPropKey<ResultEventArgs<Terminal.Gui.Drawing.Scheme> -> unit> = PropKey.Create.event "View.GettingScheme_event"
    member val HandlingHotKey: TypedPropKey<Terminal.Gui.Input.CommandEventArgs -> unit> = PropKey.Create.event "View.HandlingHotKey_event"
    member val HasFocusChanged: TypedPropKey<Terminal.Gui.ViewBase.HasFocusEventArgs -> unit> = PropKey.Create.event "View.HasFocusChanged_event"
    member val HasFocusChanging: TypedPropKey<Terminal.Gui.ViewBase.HasFocusEventArgs -> unit> = PropKey.Create.event "View.HasFocusChanging_event"
    member val HeightChanged: TypedPropKey<ValueChangedEventArgs<Terminal.Gui.ViewBase.Dim> -> unit> = PropKey.Create.event "View.HeightChanged_event"
    member val HeightChanging: TypedPropKey<ValueChangingEventArgs<Terminal.Gui.ViewBase.Dim> -> unit> = PropKey.Create.event "View.HeightChanging_event"
    member val HotKeyChanged: TypedPropKey<Terminal.Gui.Input.KeyChangedEventArgs -> unit> = PropKey.Create.event "View.HotKeyChanged_event"
    member val Initialized: TypedPropKey<System.EventArgs -> unit> = PropKey.Create.event "View.Initialized_event"
    member val KeyDown: TypedPropKey<Terminal.Gui.Input.Key -> unit> = PropKey.Create.event "View.KeyDown_event"
    member val KeyDownNotHandled: TypedPropKey<Terminal.Gui.Input.Key -> unit> = PropKey.Create.event "View.KeyDownNotHandled_event"
    member val MouseEnter: TypedPropKey<System.ComponentModel.CancelEventArgs -> unit> = PropKey.Create.event "View.MouseEnter_event"
    member val MouseEvent: TypedPropKey<Terminal.Gui.Input.Mouse -> unit> = PropKey.Create.event "View.MouseEvent_event"
    member val MouseHoldRepeatChanged: TypedPropKey<ValueChangedEventArgs<Nullable<Terminal.Gui.Input.MouseFlags>> -> unit> = PropKey.Create.event "View.MouseHoldRepeatChanged_event"
    member val MouseHoldRepeatChanging: TypedPropKey<ValueChangingEventArgs<Nullable<Terminal.Gui.Input.MouseFlags>> -> unit> = PropKey.Create.event "View.MouseHoldRepeatChanging_event"
    member val MouseLeave: TypedPropKey<System.EventArgs -> unit> = PropKey.Create.event "View.MouseLeave_event"
    member val MouseStateChanged: TypedPropKey<EventArgs<Terminal.Gui.ViewBase.MouseState> -> unit> = PropKey.Create.event "View.MouseStateChanged_event"
    member val Removed: TypedPropKey<Terminal.Gui.ViewBase.SuperViewChangedEventArgs -> unit> = PropKey.Create.event "View.Removed_event"
    member val SchemeChanged: TypedPropKey<ValueChangedEventArgs<Terminal.Gui.Drawing.Scheme> -> unit> = PropKey.Create.event "View.SchemeChanged_event"
    member val SchemeChanging: TypedPropKey<ValueChangingEventArgs<Terminal.Gui.Drawing.Scheme> -> unit> = PropKey.Create.event "View.SchemeChanging_event"
    member val SchemeNameChanged: TypedPropKey<ValueChangedEventArgs<string> -> unit> = PropKey.Create.event "View.SchemeNameChanged_event"
    member val SchemeNameChanging: TypedPropKey<ValueChangingEventArgs<string> -> unit> = PropKey.Create.event "View.SchemeNameChanging_event"
    member val SubViewAdded: TypedPropKey<Terminal.Gui.ViewBase.SuperViewChangedEventArgs -> unit> = PropKey.Create.event "View.SubViewAdded_event"
    member val SubViewLayout: TypedPropKey<Terminal.Gui.ViewBase.LayoutEventArgs -> unit> = PropKey.Create.event "View.SubViewLayout_event"
    member val SubViewRemoved: TypedPropKey<Terminal.Gui.ViewBase.SuperViewChangedEventArgs -> unit> = PropKey.Create.event "View.SubViewRemoved_event"
    member val SubViewsLaidOut: TypedPropKey<Terminal.Gui.ViewBase.LayoutEventArgs -> unit> = PropKey.Create.event "View.SubViewsLaidOut_event"
    member val SuperViewChanged: TypedPropKey<ValueChangedEventArgs<Terminal.Gui.ViewBase.View> -> unit> = PropKey.Create.event "View.SuperViewChanged_event"
    member val SuperViewChanging: TypedPropKey<ValueChangingEventArgs<Terminal.Gui.ViewBase.View> -> unit> = PropKey.Create.event "View.SuperViewChanging_event"
    member val TextChanged: TypedPropKey<System.EventArgs -> unit> = PropKey.Create.event "View.TextChanged_event"
    member val TitleChanged: TypedPropKey<EventArgs<string> -> unit> = PropKey.Create.event "View.TitleChanged_event"
    member val TitleChanging: TypedPropKey<CancelEventArgs<string> -> unit> = PropKey.Create.event "View.TitleChanging_event"
    member val ViewportChanged: TypedPropKey<Terminal.Gui.ViewBase.DrawEventArgs -> unit> = PropKey.Create.event "View.ViewportChanged_event"
    member val VisibleChanged: TypedPropKey<System.EventArgs -> unit> = PropKey.Create.event "View.VisibleChanged_event"
    member val VisibleChanging: TypedPropKey<CancelEventArgs<bool> -> unit> = PropKey.Create.event "View.VisibleChanging_event"
    member val WidthChanged: TypedPropKey<ValueChangedEventArgs<Terminal.Gui.ViewBase.Dim> -> unit> = PropKey.Create.event "View.WidthChanged_event"
    member val WidthChanging: TypedPropKey<ValueChangingEventArgs<Terminal.Gui.ViewBase.Dim> -> unit> = PropKey.Create.event "View.WidthChanging_event"

  type AdornmentPKeys() =
    inherit ViewPKeys()

    // Properties
    member val Diagnostics: TypedPropKey<Terminal.Gui.ViewBase.ViewDiagnosticFlags> = PropKey.Create.simple "Adornment.Diagnostics"
    member val Parent: TypedPropKey<Terminal.Gui.ViewBase.View> = PropKey.Create.view "Adornment.Parent_view"
    member val Parent_element: TypedPropKey<ITerminalElement> = PropKey.Create.singleElement "Adornment.Parent_element"
    member val SuperViewRendersLineCanvas: TypedPropKey<bool> = PropKey.Create.simple "Adornment.SuperViewRendersLineCanvas"
    member val Thickness: TypedPropKey<Terminal.Gui.Drawing.Thickness> = PropKey.Create.simple "Adornment.Thickness"
    member val Viewport: TypedPropKey<System.Drawing.Rectangle> = PropKey.Create.simple "Adornment.Viewport"

    // Events
    member val ThicknessChanged: TypedPropKey<System.EventArgs -> unit> = PropKey.Create.event "Adornment.ThicknessChanged_event"

  type AttributePickerPKeys() =
    inherit ViewPKeys()

    // Properties
    member val SampleText: TypedPropKey<string> = PropKey.Create.simple "AttributePicker.SampleText"
    member val Value: TypedPropKey<Nullable<Terminal.Gui.Drawing.Attribute>> = PropKey.Create.simple "AttributePicker.Value"

    // Events
    member val ValueChanged: TypedPropKey<ValueChangedEventArgs<Nullable<Terminal.Gui.Drawing.Attribute>> -> unit> = PropKey.Create.event "AttributePicker.ValueChanged_event"
    member val ValueChanging: TypedPropKey<ValueChangingEventArgs<Nullable<Terminal.Gui.Drawing.Attribute>> -> unit> = PropKey.Create.event "AttributePicker.ValueChanging_event"

  type BarPKeys() =
    inherit ViewPKeys()

    // Properties
    member val AlignmentModes: TypedPropKey<Terminal.Gui.ViewBase.AlignmentModes> = PropKey.Create.simple "Bar.AlignmentModes"
    member val Orientation: TypedPropKey<Terminal.Gui.ViewBase.Orientation> = PropKey.Create.simple "Bar.Orientation"

    // Events
    member val OrientationChanged: TypedPropKey<EventArgs<Terminal.Gui.ViewBase.Orientation> -> unit> = PropKey.Create.event "Bar.OrientationChanged_event"
    member val OrientationChanging: TypedPropKey<CancelEventArgs<Terminal.Gui.ViewBase.Orientation> -> unit> = PropKey.Create.event "Bar.OrientationChanging_event"

  type BorderPKeys() =
    inherit AdornmentPKeys()

    // Properties
    member val LineStyle: TypedPropKey<Terminal.Gui.Drawing.LineStyle> = PropKey.Create.simple "Border.LineStyle"
    member val Settings: TypedPropKey<Terminal.Gui.ViewBase.BorderSettings> = PropKey.Create.simple "Border.Settings"

  type ButtonPKeys() =
    inherit ViewPKeys()

    // Properties
    member val HotKeySpecifier: TypedPropKey<System.Text.Rune> = PropKey.Create.simple "Button.HotKeySpecifier"
    member val IsDefault: TypedPropKey<bool> = PropKey.Create.simple "Button.IsDefault"
    member val NoDecorations: TypedPropKey<bool> = PropKey.Create.simple "Button.NoDecorations"
    member val NoPadding: TypedPropKey<bool> = PropKey.Create.simple "Button.NoPadding"
    member val Text: TypedPropKey<string> = PropKey.Create.simple "Button.Text"

  type CharMapPKeys() =
    inherit ViewPKeys()

    // Properties
    member val SelectedCodePoint: TypedPropKey<int> = PropKey.Create.simple "CharMap.SelectedCodePoint"
    member val ShowGlyphWidths: TypedPropKey<bool> = PropKey.Create.simple "CharMap.ShowGlyphWidths"
    member val ShowUnicodeCategory: TypedPropKey<Nullable<System.Globalization.UnicodeCategory>> = PropKey.Create.simple "CharMap.ShowUnicodeCategory"
    member val StartCodePoint: TypedPropKey<int> = PropKey.Create.simple "CharMap.StartCodePoint"
    member val Value: TypedPropKey<System.Text.Rune> = PropKey.Create.simple "CharMap.Value"

    // Events
    member val ValueChanged: TypedPropKey<ValueChangedEventArgs<System.Text.Rune> -> unit> = PropKey.Create.event "CharMap.ValueChanged_event"
    member val ValueChanging: TypedPropKey<ValueChangingEventArgs<System.Text.Rune> -> unit> = PropKey.Create.event "CharMap.ValueChanging_event"

  type CheckBoxPKeys() =
    inherit ViewPKeys()

    // Properties
    member val AllowCheckStateNone: TypedPropKey<bool> = PropKey.Create.simple "CheckBox.AllowCheckStateNone"
    member val HotKeySpecifier: TypedPropKey<System.Text.Rune> = PropKey.Create.simple "CheckBox.HotKeySpecifier"
    member val RadioStyle: TypedPropKey<bool> = PropKey.Create.simple "CheckBox.RadioStyle"
    member val Text: TypedPropKey<string> = PropKey.Create.simple "CheckBox.Text"
    member val Value: TypedPropKey<Terminal.Gui.Views.CheckState> = PropKey.Create.simple "CheckBox.Value"

    // Events
    member val ValueChanged: TypedPropKey<ValueChangedEventArgs<Terminal.Gui.Views.CheckState> -> unit> = PropKey.Create.event "CheckBox.ValueChanged_event"
    member val ValueChanging: TypedPropKey<ValueChangingEventArgs<Terminal.Gui.Views.CheckState> -> unit> = PropKey.Create.event "CheckBox.ValueChanging_event"

  type ColorPickerPKeys() =
    inherit ViewPKeys()

    // Properties
    member val SelectedColor: TypedPropKey<Terminal.Gui.Drawing.Color> = PropKey.Create.simple "ColorPicker.SelectedColor"
    member val Style: TypedPropKey<Terminal.Gui.Views.ColorPickerStyle> = PropKey.Create.simple "ColorPicker.Style"
    member val Text: TypedPropKey<string> = PropKey.Create.simple "ColorPicker.Text"
    member val Value: TypedPropKey<Nullable<Terminal.Gui.Drawing.Color>> = PropKey.Create.simple "ColorPicker.Value"

    // Events
    member val ValueChanged: TypedPropKey<ValueChangedEventArgs<Nullable<Terminal.Gui.Drawing.Color>> -> unit> = PropKey.Create.event "ColorPicker.ValueChanged_event"
    member val ValueChanging: TypedPropKey<ValueChangingEventArgs<Nullable<Terminal.Gui.Drawing.Color>> -> unit> = PropKey.Create.event "ColorPicker.ValueChanging_event"

  type ColorPicker16PKeys() =
    inherit ViewPKeys()

    // Properties
    member val BoxHeight: TypedPropKey<int> = PropKey.Create.simple "ColorPicker16.BoxHeight"
    member val BoxWidth: TypedPropKey<int> = PropKey.Create.simple "ColorPicker16.BoxWidth"
    member val Caret: TypedPropKey<System.Drawing.Point> = PropKey.Create.simple "ColorPicker16.Caret"
    member val SelectedColor: TypedPropKey<Terminal.Gui.Drawing.ColorName16> = PropKey.Create.simple "ColorPicker16.SelectedColor"
    member val Value: TypedPropKey<Terminal.Gui.Drawing.ColorName16> = PropKey.Create.simple "ColorPicker16.Value"

    // Events
    member val ValueChanged: TypedPropKey<ValueChangedEventArgs<Terminal.Gui.Drawing.ColorName16> -> unit> = PropKey.Create.event "ColorPicker16.ValueChanged_event"
    member val ValueChanging: TypedPropKey<ValueChangingEventArgs<Terminal.Gui.Drawing.ColorName16> -> unit> = PropKey.Create.event "ColorPicker16.ValueChanging_event"

  type ComboBoxPKeys() =
    inherit ViewPKeys()

    // Properties
    member val HideDropdownListOnClick: TypedPropKey<bool> = PropKey.Create.simple "ComboBox.HideDropdownListOnClick"
    member val ReadOnly: TypedPropKey<bool> = PropKey.Create.simple "ComboBox.ReadOnly"
    member val SearchText: TypedPropKey<string> = PropKey.Create.simple "ComboBox.SearchText"
    member val SelectedItem: TypedPropKey<int> = PropKey.Create.simple "ComboBox.SelectedItem"
    member val Source: TypedPropKey<Terminal.Gui.Views.IListDataSource> = PropKey.Create.simple "ComboBox.Source"
    member val Text: TypedPropKey<string> = PropKey.Create.simple "ComboBox.Text"

    // Events
    member val Collapsed: TypedPropKey<System.EventArgs -> unit> = PropKey.Create.event "ComboBox.Collapsed_event"
    member val Expanded: TypedPropKey<System.EventArgs -> unit> = PropKey.Create.event "ComboBox.Expanded_event"
    member val OpenSelectedItem: TypedPropKey<Terminal.Gui.Views.ListViewItemEventArgs -> unit> = PropKey.Create.event "ComboBox.OpenSelectedItem_event"
    member val SelectedItemChanged: TypedPropKey<Terminal.Gui.Views.ListViewItemEventArgs -> unit> = PropKey.Create.event "ComboBox.SelectedItemChanged_event"

  type DatePickerPKeys() =
    inherit ViewPKeys()

    // Properties
    member val Culture: TypedPropKey<System.Globalization.CultureInfo> = PropKey.Create.simple "DatePicker.Culture"
    member val Text: TypedPropKey<string> = PropKey.Create.simple "DatePicker.Text"
    member val Value: TypedPropKey<System.DateTime> = PropKey.Create.simple "DatePicker.Value"

    // Events
    member val ValueChanged: TypedPropKey<ValueChangedEventArgs<System.DateTime> -> unit> = PropKey.Create.event "DatePicker.ValueChanged_event"
    member val ValueChanging: TypedPropKey<ValueChangingEventArgs<System.DateTime> -> unit> = PropKey.Create.event "DatePicker.ValueChanging_event"

  type FrameViewPKeys() =
    inherit ViewPKeys()


  type GraphViewPKeys() =
    inherit ViewPKeys()

    // Properties
    member val AxisX: TypedPropKey<Terminal.Gui.Views.HorizontalAxis> = PropKey.Create.simple "GraphView.AxisX"
    member val AxisY: TypedPropKey<Terminal.Gui.Views.VerticalAxis> = PropKey.Create.simple "GraphView.AxisY"
    member val CellSize: TypedPropKey<System.Drawing.PointF> = PropKey.Create.simple "GraphView.CellSize"
    member val GraphColor: TypedPropKey<Nullable<Terminal.Gui.Drawing.Attribute>> = PropKey.Create.simple "GraphView.GraphColor"
    member val MarginBottom: TypedPropKey<System.UInt32> = PropKey.Create.simple "GraphView.MarginBottom"
    member val MarginLeft: TypedPropKey<System.UInt32> = PropKey.Create.simple "GraphView.MarginLeft"
    member val ScrollOffset: TypedPropKey<System.Drawing.PointF> = PropKey.Create.simple "GraphView.ScrollOffset"

  type HexViewPKeys() =
    inherit ViewPKeys()

    // Properties
    member val Address: TypedPropKey<System.Int64> = PropKey.Create.simple "HexView.Address"
    member val AddressWidth: TypedPropKey<int> = PropKey.Create.simple "HexView.AddressWidth"
    member val BytesPerLine: TypedPropKey<int> = PropKey.Create.simple "HexView.BytesPerLine"
    member val ReadOnly: TypedPropKey<bool> = PropKey.Create.simple "HexView.ReadOnly"
    member val Source: TypedPropKey<System.IO.Stream> = PropKey.Create.simple "HexView.Source"

    // Events
    member val Edited: TypedPropKey<Terminal.Gui.Views.HexViewEditEventArgs -> unit> = PropKey.Create.event "HexView.Edited_event"
    member val PositionChanged: TypedPropKey<Terminal.Gui.Views.HexViewEventArgs -> unit> = PropKey.Create.event "HexView.PositionChanged_event"

  type LabelPKeys() =
    inherit ViewPKeys()

    // Properties
    member val HotKeySpecifier: TypedPropKey<System.Text.Rune> = PropKey.Create.simple "Label.HotKeySpecifier"
    member val Text: TypedPropKey<string> = PropKey.Create.simple "Label.Text"

  type LegendAnnotationPKeys() =
    inherit ViewPKeys()


  type LinePKeys() =
    inherit ViewPKeys()

    // Properties
    member val Length: TypedPropKey<Terminal.Gui.ViewBase.Dim> = PropKey.Create.simple "Line.Length"
    member val Orientation: TypedPropKey<Terminal.Gui.ViewBase.Orientation> = PropKey.Create.simple "Line.Orientation"
    member val Style: TypedPropKey<Terminal.Gui.Drawing.LineStyle> = PropKey.Create.simple "Line.Style"

    // Events
    member val OrientationChanged: TypedPropKey<EventArgs<Terminal.Gui.ViewBase.Orientation> -> unit> = PropKey.Create.event "Line.OrientationChanged_event"
    member val OrientationChanging: TypedPropKey<CancelEventArgs<Terminal.Gui.ViewBase.Orientation> -> unit> = PropKey.Create.event "Line.OrientationChanging_event"

  type LinearRangePKeys<'T>() =
    inherit ViewPKeys()

    // Properties
    member val AllowEmpty: TypedPropKey<bool> = PropKey.Create.simple "LinearRange.AllowEmpty"
    member val FocusedOption: TypedPropKey<int> = PropKey.Create.simple "LinearRange.FocusedOption"
    member val LegendsOrientation: TypedPropKey<Terminal.Gui.ViewBase.Orientation> = PropKey.Create.simple "LinearRange.LegendsOrientation"
    member val MinimumInnerSpacing: TypedPropKey<int> = PropKey.Create.simple "LinearRange.MinimumInnerSpacing"
    member val Options: TypedPropKey<List<LinearRangeOption<'T>>> = PropKey.Create.simple "LinearRange.Options"
    member val Orientation: TypedPropKey<Terminal.Gui.ViewBase.Orientation> = PropKey.Create.simple "LinearRange.Orientation"
    member val RangeAllowSingle: TypedPropKey<bool> = PropKey.Create.simple "LinearRange.RangeAllowSingle"
    member val ShowEndSpacing: TypedPropKey<bool> = PropKey.Create.simple "LinearRange.ShowEndSpacing"
    member val ShowLegends: TypedPropKey<bool> = PropKey.Create.simple "LinearRange.ShowLegends"
    member val Style: TypedPropKey<Terminal.Gui.Views.LinearRangeStyle> = PropKey.Create.simple "LinearRange.Style"
    member val Text: TypedPropKey<string> = PropKey.Create.simple "LinearRange.Text"
    member val Type: TypedPropKey<Terminal.Gui.Views.LinearRangeType> = PropKey.Create.simple "LinearRange.Type"
    member val UseMinimumSize: TypedPropKey<bool> = PropKey.Create.simple "LinearRange.UseMinimumSize"

    // Events
    member val LegendsOrientationChanged: TypedPropKey<ValueChangedEventArgs<Terminal.Gui.ViewBase.Orientation> -> unit> = PropKey.Create.event "LinearRange.LegendsOrientationChanged_event"
    member val LegendsOrientationChanging: TypedPropKey<ValueChangingEventArgs<Terminal.Gui.ViewBase.Orientation> -> unit> = PropKey.Create.event "LinearRange.LegendsOrientationChanging_event"
    member val MinimumInnerSpacingChanged: TypedPropKey<ValueChangedEventArgs<int> -> unit> = PropKey.Create.event "LinearRange.MinimumInnerSpacingChanged_event"
    member val MinimumInnerSpacingChanging: TypedPropKey<ValueChangingEventArgs<int> -> unit> = PropKey.Create.event "LinearRange.MinimumInnerSpacingChanging_event"
    member val OptionFocused: TypedPropKey<LinearRangeEventArgs<'T> -> unit> = PropKey.Create.event "LinearRange.OptionFocused_event"
    member val OptionsChanged: TypedPropKey<LinearRangeEventArgs<'T> -> unit> = PropKey.Create.event "LinearRange.OptionsChanged_event"
    member val OrientationChanged: TypedPropKey<EventArgs<Terminal.Gui.ViewBase.Orientation> -> unit> = PropKey.Create.event "LinearRange.OrientationChanged_event"
    member val OrientationChanging: TypedPropKey<CancelEventArgs<Terminal.Gui.ViewBase.Orientation> -> unit> = PropKey.Create.event "LinearRange.OrientationChanging_event"
    member val ShowEndSpacingChanged: TypedPropKey<ValueChangedEventArgs<bool> -> unit> = PropKey.Create.event "LinearRange.ShowEndSpacingChanged_event"
    member val ShowEndSpacingChanging: TypedPropKey<ValueChangingEventArgs<bool> -> unit> = PropKey.Create.event "LinearRange.ShowEndSpacingChanging_event"
    member val ShowLegendsChanged: TypedPropKey<ValueChangedEventArgs<bool> -> unit> = PropKey.Create.event "LinearRange.ShowLegendsChanged_event"
    member val ShowLegendsChanging: TypedPropKey<ValueChangingEventArgs<bool> -> unit> = PropKey.Create.event "LinearRange.ShowLegendsChanging_event"
    member val TypeChanged: TypedPropKey<ValueChangedEventArgs<Terminal.Gui.Views.LinearRangeType> -> unit> = PropKey.Create.event "LinearRange.TypeChanged_event"
    member val TypeChanging: TypedPropKey<ValueChangingEventArgs<Terminal.Gui.Views.LinearRangeType> -> unit> = PropKey.Create.event "LinearRange.TypeChanging_event"
    member val UseMinimumSizeChanged: TypedPropKey<ValueChangedEventArgs<bool> -> unit> = PropKey.Create.event "LinearRange.UseMinimumSizeChanged_event"
    member val UseMinimumSizeChanging: TypedPropKey<ValueChangingEventArgs<bool> -> unit> = PropKey.Create.event "LinearRange.UseMinimumSizeChanging_event"

  type LinearRangePKeys() =
    inherit LinearRangePKeys<System.Object>()


  type ListViewPKeys() =
    inherit ViewPKeys()

    // Properties
    member val MarkMultiple: TypedPropKey<bool> = PropKey.Create.simple "ListView.MarkMultiple"
    member val SelectedItem: TypedPropKey<Nullable<int>> = PropKey.Create.simple "ListView.SelectedItem"
    member val ShowMarks: TypedPropKey<bool> = PropKey.Create.simple "ListView.ShowMarks"
    member val Source: TypedPropKey<Terminal.Gui.Views.IListDataSource> = PropKey.Create.simple "ListView.Source"
    member val TopItem: TypedPropKey<int> = PropKey.Create.simple "ListView.TopItem"
    member val Value: TypedPropKey<Nullable<int>> = PropKey.Create.simple "ListView.Value"

    // Events
    member val CollectionChanged: TypedPropKey<System.Collections.Specialized.NotifyCollectionChangedEventArgs -> unit> = PropKey.Create.event "ListView.CollectionChanged_event"
    member val RowRender: TypedPropKey<Terminal.Gui.Views.ListViewRowEventArgs -> unit> = PropKey.Create.event "ListView.RowRender_event"
    member val SourceChanged: TypedPropKey<System.EventArgs -> unit> = PropKey.Create.event "ListView.SourceChanged_event"
    member val ValueChanged: TypedPropKey<ValueChangedEventArgs<Nullable<int>> -> unit> = PropKey.Create.event "ListView.ValueChanged_event"
    member val ValueChanging: TypedPropKey<ValueChangingEventArgs<Nullable<int>> -> unit> = PropKey.Create.event "ListView.ValueChanging_event"

  type MarginPKeys() =
    inherit AdornmentPKeys()

    // Properties
    member val ShadowSize: TypedPropKey<System.Drawing.Size> = PropKey.Create.simple "Margin.ShadowSize"
    member val ShadowStyle: TypedPropKey<Terminal.Gui.ViewBase.ShadowStyle> = PropKey.Create.simple "Margin.ShadowStyle"

  type MenuPKeys() =
    inherit BarPKeys()

    // Properties
    member val SelectedMenuItem: TypedPropKey<Terminal.Gui.Views.MenuItem> = PropKey.Create.view "Menu.SelectedMenuItem_view"
    member val SelectedMenuItem_element: TypedPropKey<IMenuItemTerminalElement> = PropKey.Create.singleElement "Menu.SelectedMenuItem_element"
    member val SuperMenuItem: TypedPropKey<Terminal.Gui.Views.MenuItem> = PropKey.Create.view "Menu.SuperMenuItem_view"
    member val SuperMenuItem_element: TypedPropKey<IMenuItemTerminalElement> = PropKey.Create.singleElement "Menu.SuperMenuItem_element"

    // Events
    member val SelectedMenuItemChanged: TypedPropKey<Terminal.Gui.Views.MenuItem -> unit> = PropKey.Create.event "Menu.SelectedMenuItemChanged_event"

  type MenuBarPKeys() =
    inherit MenuPKeys()

    // Properties
    member val Key: TypedPropKey<Terminal.Gui.Input.Key> = PropKey.Create.simple "MenuBar.Key"

    // Events
    member val KeyChanged: TypedPropKey<Terminal.Gui.Input.KeyChangedEventArgs -> unit> = PropKey.Create.event "MenuBar.KeyChanged_event"

  type NumericUpDownPKeys<'T>() =
    inherit ViewPKeys()

    // Properties
    member val Format: TypedPropKey<string> = PropKey.Create.simple "NumericUpDown.Format"
    member val Increment: TypedPropKey<'T> = PropKey.Create.simple "NumericUpDown.Increment"
    member val Value: TypedPropKey<'T> = PropKey.Create.simple "NumericUpDown.Value"

    // Events
    member val FormatChanged: TypedPropKey<EventArgs<string> -> unit> = PropKey.Create.event "NumericUpDown.FormatChanged_event"
    member val IncrementChanged: TypedPropKey<EventArgs<'T> -> unit> = PropKey.Create.event "NumericUpDown.IncrementChanged_event"
    member val ValueChanged: TypedPropKey<ValueChangedEventArgs<'T> -> unit> = PropKey.Create.event "NumericUpDown.ValueChanged_event"
    member val ValueChanging: TypedPropKey<ValueChangingEventArgs<'T> -> unit> = PropKey.Create.event "NumericUpDown.ValueChanging_event"

  type NumericUpDownPKeys() =
    inherit NumericUpDownPKeys<int>()


  type PaddingPKeys() =
    inherit AdornmentPKeys()


  type PopoverBaseImplPKeys() =
    inherit ViewPKeys()

    // Properties
    member val Current: TypedPropKey<Terminal.Gui.App.IRunnable> = PropKey.Create.simple "PopoverBaseImpl.Current"

  type PopoverMenuPKeys() =
    inherit PopoverBaseImplPKeys()

    // Properties
    member val Key: TypedPropKey<Terminal.Gui.Input.Key> = PropKey.Create.simple "PopoverMenu.Key"
    member val MouseFlags: TypedPropKey<Terminal.Gui.Input.MouseFlags> = PropKey.Create.simple "PopoverMenu.MouseFlags"
    member val Root: TypedPropKey<Terminal.Gui.Views.Menu> = PropKey.Create.view "PopoverMenu.Root_view"
    member val Root_element: TypedPropKey<IMenuTerminalElement> = PropKey.Create.singleElement "PopoverMenu.Root_element"

    // Events
    member val KeyChanged: TypedPropKey<Terminal.Gui.Input.KeyChangedEventArgs -> unit> = PropKey.Create.event "PopoverMenu.KeyChanged_event"

  type ProgressBarPKeys() =
    inherit ViewPKeys()

    // Properties
    member val BidirectionalMarquee: TypedPropKey<bool> = PropKey.Create.simple "ProgressBar.BidirectionalMarquee"
    member val Fraction: TypedPropKey<System.Single> = PropKey.Create.simple "ProgressBar.Fraction"
    member val ProgressBarFormat: TypedPropKey<Terminal.Gui.Views.ProgressBarFormat> = PropKey.Create.simple "ProgressBar.ProgressBarFormat"
    member val ProgressBarStyle: TypedPropKey<Terminal.Gui.Views.ProgressBarStyle> = PropKey.Create.simple "ProgressBar.ProgressBarStyle"
    member val SegmentCharacter: TypedPropKey<System.Text.Rune> = PropKey.Create.simple "ProgressBar.SegmentCharacter"
    member val Text: TypedPropKey<string> = PropKey.Create.simple "ProgressBar.Text"

  type RunnablePKeys() =
    inherit ViewPKeys()

    // Properties
    member val Result: TypedPropKey<System.Object> = PropKey.Create.simple "Runnable.Result"
    member val StopRequested: TypedPropKey<bool> = PropKey.Create.simple "Runnable.StopRequested"

    // Events
    member val IsModalChanged: TypedPropKey<EventArgs<bool> -> unit> = PropKey.Create.event "Runnable.IsModalChanged_event"
    member val IsRunningChanged: TypedPropKey<EventArgs<bool> -> unit> = PropKey.Create.event "Runnable.IsRunningChanged_event"
    member val IsRunningChanging: TypedPropKey<CancelEventArgs<bool> -> unit> = PropKey.Create.event "Runnable.IsRunningChanging_event"

  type RunnablePKeys<'TResult>() =
    inherit RunnablePKeys()

    // Properties
    member val Result: TypedPropKey<'TResult> = PropKey.Create.simple "Runnable.Result"

  type DialogPKeys<'TResult>() =
    inherit RunnablePKeys<'TResult>()

    // Properties
    member val ButtonAlignment: TypedPropKey<Terminal.Gui.ViewBase.Alignment> = PropKey.Create.simple "Dialog.ButtonAlignment"
    member val ButtonAlignmentModes: TypedPropKey<Terminal.Gui.ViewBase.AlignmentModes> = PropKey.Create.simple "Dialog.ButtonAlignmentModes"
    member val Buttons: TypedPropKey<Terminal.Gui.Views.Button[]> = PropKey.Create.simple "Dialog.Buttons"

  type DialogPKeys() =
    inherit DialogPKeys<int>()

    // Properties
    member val Result: TypedPropKey<Nullable<int>> = PropKey.Create.simple "Dialog.Result"

  type PromptPKeys<'TView, 'TResult when 'TView: (new: unit -> 'TView) and 'TView:> Terminal.Gui.ViewBase.View>() =
    inherit DialogPKeys<'TResult>()

    // Properties
    member val ResultExtractor: TypedPropKey<Func<'TView, 'TResult>> = PropKey.Create.simple "Prompt.ResultExtractor"

  type FileDialogPKeys() =
    inherit DialogPKeys()

    // Properties
    member val AllowedTypes: TypedPropKey<List<Terminal.Gui.Views.IAllowedType>> = PropKey.Create.simple "FileDialog.AllowedTypes"
    member val AllowsMultipleSelection: TypedPropKey<bool> = PropKey.Create.simple "FileDialog.AllowsMultipleSelection"
    member val FileOperationsHandler: TypedPropKey<Terminal.Gui.FileServices.IFileOperations> = PropKey.Create.simple "FileDialog.FileOperationsHandler"
    member val MustExist: TypedPropKey<bool> = PropKey.Create.simple "FileDialog.MustExist"
    member val OpenMode: TypedPropKey<Terminal.Gui.Views.OpenMode> = PropKey.Create.simple "FileDialog.OpenMode"
    member val Path: TypedPropKey<string> = PropKey.Create.simple "FileDialog.Path"
    member val SearchMatcher: TypedPropKey<Terminal.Gui.FileServices.ISearchMatcher> = PropKey.Create.simple "FileDialog.SearchMatcher"

    // Events
    member val FilesSelected: TypedPropKey<Terminal.Gui.Views.FilesSelectedEventArgs -> unit> = PropKey.Create.event "FileDialog.FilesSelected_event"

  type OpenDialogPKeys() =
    inherit FileDialogPKeys()

    // Properties
    member val OpenMode: TypedPropKey<Terminal.Gui.Views.OpenMode> = PropKey.Create.simple "OpenDialog.OpenMode"

  type SaveDialogPKeys() =
    inherit FileDialogPKeys()


  type ScrollBarPKeys() =
    inherit ViewPKeys()

    // Properties
    member val AutoShow: TypedPropKey<bool> = PropKey.Create.simple "ScrollBar.AutoShow"
    member val Increment: TypedPropKey<int> = PropKey.Create.simple "ScrollBar.Increment"
    member val Orientation: TypedPropKey<Terminal.Gui.ViewBase.Orientation> = PropKey.Create.simple "ScrollBar.Orientation"
    member val ScrollableContentSize: TypedPropKey<int> = PropKey.Create.simple "ScrollBar.ScrollableContentSize"
    member val Value: TypedPropKey<int> = PropKey.Create.simple "ScrollBar.Value"
    member val VisibleContentSize: TypedPropKey<int> = PropKey.Create.simple "ScrollBar.VisibleContentSize"

    // Events
    member val OrientationChanged: TypedPropKey<EventArgs<Terminal.Gui.ViewBase.Orientation> -> unit> = PropKey.Create.event "ScrollBar.OrientationChanged_event"
    member val OrientationChanging: TypedPropKey<CancelEventArgs<Terminal.Gui.ViewBase.Orientation> -> unit> = PropKey.Create.event "ScrollBar.OrientationChanging_event"
    member val ScrollableContentSizeChanged: TypedPropKey<EventArgs<int> -> unit> = PropKey.Create.event "ScrollBar.ScrollableContentSizeChanged_event"
    member val Scrolled: TypedPropKey<EventArgs<int> -> unit> = PropKey.Create.event "ScrollBar.Scrolled_event"
    member val SliderPositionChanged: TypedPropKey<EventArgs<int> -> unit> = PropKey.Create.event "ScrollBar.SliderPositionChanged_event"
    member val ValueChanged: TypedPropKey<ValueChangedEventArgs<int> -> unit> = PropKey.Create.event "ScrollBar.ValueChanged_event"
    member val ValueChanging: TypedPropKey<ValueChangingEventArgs<int> -> unit> = PropKey.Create.event "ScrollBar.ValueChanging_event"

  type ScrollSliderPKeys() =
    inherit ViewPKeys()

    // Properties
    member val Orientation: TypedPropKey<Terminal.Gui.ViewBase.Orientation> = PropKey.Create.simple "ScrollSlider.Orientation"
    member val Position: TypedPropKey<int> = PropKey.Create.simple "ScrollSlider.Position"
    member val Size: TypedPropKey<int> = PropKey.Create.simple "ScrollSlider.Size"
    member val SliderPadding: TypedPropKey<int> = PropKey.Create.simple "ScrollSlider.SliderPadding"
    member val VisibleContentSize: TypedPropKey<int> = PropKey.Create.simple "ScrollSlider.VisibleContentSize"

    // Events
    member val OrientationChanged: TypedPropKey<EventArgs<Terminal.Gui.ViewBase.Orientation> -> unit> = PropKey.Create.event "ScrollSlider.OrientationChanged_event"
    member val OrientationChanging: TypedPropKey<CancelEventArgs<Terminal.Gui.ViewBase.Orientation> -> unit> = PropKey.Create.event "ScrollSlider.OrientationChanging_event"
    member val PositionChanged: TypedPropKey<EventArgs<int> -> unit> = PropKey.Create.event "ScrollSlider.PositionChanged_event"
    member val PositionChanging: TypedPropKey<CancelEventArgs<int> -> unit> = PropKey.Create.event "ScrollSlider.PositionChanging_event"
    member val Scrolled: TypedPropKey<EventArgs<int> -> unit> = PropKey.Create.event "ScrollSlider.Scrolled_event"

  type SelectorBasePKeys() =
    inherit ViewPKeys()

    // Properties
    member val DoubleClickAccepts: TypedPropKey<bool> = PropKey.Create.simple "SelectorBase.DoubleClickAccepts"
    member val HorizontalSpace: TypedPropKey<int> = PropKey.Create.simple "SelectorBase.HorizontalSpace"
    member val Labels: TypedPropKey<IReadOnlyList<string>> = PropKey.Create.simple "SelectorBase.Labels"
    member val Orientation: TypedPropKey<Terminal.Gui.ViewBase.Orientation> = PropKey.Create.simple "SelectorBase.Orientation"
    member val Styles: TypedPropKey<Terminal.Gui.Views.SelectorStyles> = PropKey.Create.simple "SelectorBase.Styles"
    member val Value: TypedPropKey<Nullable<int>> = PropKey.Create.simple "SelectorBase.Value"
    member val Values: TypedPropKey<IReadOnlyList<int>> = PropKey.Create.simple "SelectorBase.Values"

    // Events
    member val OrientationChanged: TypedPropKey<EventArgs<Terminal.Gui.ViewBase.Orientation> -> unit> = PropKey.Create.event "SelectorBase.OrientationChanged_event"
    member val OrientationChanging: TypedPropKey<CancelEventArgs<Terminal.Gui.ViewBase.Orientation> -> unit> = PropKey.Create.event "SelectorBase.OrientationChanging_event"
    member val ValueChanged: TypedPropKey<ValueChangedEventArgs<Nullable<int>> -> unit> = PropKey.Create.event "SelectorBase.ValueChanged_event"
    member val ValueChanging: TypedPropKey<ValueChangingEventArgs<Nullable<int>> -> unit> = PropKey.Create.event "SelectorBase.ValueChanging_event"

  type FlagSelectorPKeys() =
    inherit SelectorBasePKeys()

    // Properties
    member val Value: TypedPropKey<Nullable<int>> = PropKey.Create.simple "FlagSelector.Value"

  type OptionSelectorPKeys() =
    inherit SelectorBasePKeys()

    // Properties
    member val Cursor: TypedPropKey<int> = PropKey.Create.simple "OptionSelector.Cursor"

  type FlagSelectorPKeys<'TFlagsEnum when 'TFlagsEnum: struct and 'TFlagsEnum: (new: unit -> 'TFlagsEnum) and 'TFlagsEnum:> System.Enum and 'TFlagsEnum:> System.ValueType>() =
    inherit FlagSelectorPKeys()

    // Properties
    member val Value: TypedPropKey<Nullable<'TFlagsEnum>> = PropKey.Create.simple "FlagSelector.Value"

    // Events
    member val ValueChanged: TypedPropKey<EventArgs<Nullable<'TFlagsEnum>> -> unit> = PropKey.Create.event "FlagSelector.ValueChanged_event"

  type OptionSelectorPKeys<'TEnum when 'TEnum: struct and 'TEnum: (new: unit -> 'TEnum) and 'TEnum:> System.Enum and 'TEnum:> System.ValueType>() =
    inherit OptionSelectorPKeys()

    // Properties
    member val Value: TypedPropKey<Nullable<'TEnum>> = PropKey.Create.simple "OptionSelector.Value"
    member val Values: TypedPropKey<IReadOnlyList<int>> = PropKey.Create.simple "OptionSelector.Values"

    // Events
    member val ValueChanged: TypedPropKey<EventArgs<Nullable<'TEnum>> -> unit> = PropKey.Create.event "OptionSelector.ValueChanged_event"

  type ShortcutPKeys() =
    inherit ViewPKeys()

    // Properties
    member val Action: TypedPropKey<System.Action> = PropKey.Create.simple "Shortcut.Action"
    member val AlignmentModes: TypedPropKey<Terminal.Gui.ViewBase.AlignmentModes> = PropKey.Create.simple "Shortcut.AlignmentModes"
    member val BindKeyToApplication: TypedPropKey<bool> = PropKey.Create.simple "Shortcut.BindKeyToApplication"
    member val CommandView: TypedPropKey<Terminal.Gui.ViewBase.View> = PropKey.Create.view "Shortcut.CommandView_view"
    member val CommandView_element: TypedPropKey<ITerminalElement> = PropKey.Create.singleElement "Shortcut.CommandView_element"
    member val ForceFocusColors: TypedPropKey<bool> = PropKey.Create.simple "Shortcut.ForceFocusColors"
    member val HelpText: TypedPropKey<string> = PropKey.Create.simple "Shortcut.HelpText"
    member val Key: TypedPropKey<Terminal.Gui.Input.Key> = PropKey.Create.simple "Shortcut.Key"
    member val MinimumKeyTextSize: TypedPropKey<int> = PropKey.Create.simple "Shortcut.MinimumKeyTextSize"
    member val Orientation: TypedPropKey<Terminal.Gui.ViewBase.Orientation> = PropKey.Create.simple "Shortcut.Orientation"
    member val Text: TypedPropKey<string> = PropKey.Create.simple "Shortcut.Text"

    // Events
    member val OrientationChanged: TypedPropKey<EventArgs<Terminal.Gui.ViewBase.Orientation> -> unit> = PropKey.Create.event "Shortcut.OrientationChanged_event"
    member val OrientationChanging: TypedPropKey<CancelEventArgs<Terminal.Gui.ViewBase.Orientation> -> unit> = PropKey.Create.event "Shortcut.OrientationChanging_event"

  type MenuItemPKeys() =
    inherit ShortcutPKeys()

    // Properties
    member val Command: TypedPropKey<Terminal.Gui.Input.Command> = PropKey.Create.simple "MenuItem.Command"
    member val SubMenu: TypedPropKey<Terminal.Gui.Views.Menu> = PropKey.Create.view "MenuItem.SubMenu_view"
    member val SubMenu_element: TypedPropKey<IMenuTerminalElement> = PropKey.Create.singleElement "MenuItem.SubMenu_element"
    member val TargetView: TypedPropKey<Terminal.Gui.ViewBase.View> = PropKey.Create.view "MenuItem.TargetView_view"
    member val TargetView_element: TypedPropKey<ITerminalElement> = PropKey.Create.singleElement "MenuItem.TargetView_element"

  type MenuBarItemPKeys() =
    inherit MenuItemPKeys()

    // Properties
    member val PopoverMenu: TypedPropKey<Terminal.Gui.Views.PopoverMenu> = PropKey.Create.view "MenuBarItem.PopoverMenu_view"
    member val PopoverMenu_element: TypedPropKey<IPopoverMenuTerminalElement> = PropKey.Create.singleElement "MenuBarItem.PopoverMenu_element"
    member val PopoverMenuOpen: TypedPropKey<bool> = PropKey.Create.simple "MenuBarItem.PopoverMenuOpen"
    member val SubMenu: TypedPropKey<Terminal.Gui.Views.Menu> = PropKey.Create.view "MenuBarItem.SubMenu_view"
    member val SubMenu_element: TypedPropKey<IMenuTerminalElement> = PropKey.Create.singleElement "MenuBarItem.SubMenu_element"

    // Events
    member val PopoverMenuOpenChanged: TypedPropKey<EventArgs<bool> -> unit> = PropKey.Create.event "MenuBarItem.PopoverMenuOpenChanged_event"

  type SpinnerViewPKeys() =
    inherit ViewPKeys()

    // Properties
    member val AutoSpin: TypedPropKey<bool> = PropKey.Create.simple "SpinnerView.AutoSpin"
    member val Sequence: TypedPropKey<System.String[]> = PropKey.Create.simple "SpinnerView.Sequence"
    member val SpinBounce: TypedPropKey<bool> = PropKey.Create.simple "SpinnerView.SpinBounce"
    member val SpinDelay: TypedPropKey<int> = PropKey.Create.simple "SpinnerView.SpinDelay"
    member val SpinReverse: TypedPropKey<bool> = PropKey.Create.simple "SpinnerView.SpinReverse"
    member val Style: TypedPropKey<Terminal.Gui.Views.SpinnerStyle> = PropKey.Create.simple "SpinnerView.Style"

  type StatusBarPKeys() =
    inherit BarPKeys()


  type TabPKeys() =
    inherit ViewPKeys()

    // Properties
    member val DisplayText: TypedPropKey<string> = PropKey.Create.simple "Tab.DisplayText"
    member val View: TypedPropKey<Terminal.Gui.ViewBase.View> = PropKey.Create.view "Tab.View_view"
    member val View_element: TypedPropKey<ITerminalElement> = PropKey.Create.singleElement "Tab.View_element"

  type TabViewPKeys() =
    inherit ViewPKeys()

    // Properties
    member val MaxTabTextWidth: TypedPropKey<System.UInt32> = PropKey.Create.simple "TabView.MaxTabTextWidth"
    member val SelectedTab: TypedPropKey<Terminal.Gui.Views.Tab> = PropKey.Create.view "TabView.SelectedTab_view"
    member val SelectedTab_element: TypedPropKey<ITabTerminalElement> = PropKey.Create.singleElement "TabView.SelectedTab_element"
    member val Style: TypedPropKey<Terminal.Gui.Views.TabStyle> = PropKey.Create.simple "TabView.Style"
    member val TabScrollOffset: TypedPropKey<int> = PropKey.Create.simple "TabView.TabScrollOffset"

    // Events
    member val SelectedTabChanged: TypedPropKey<Terminal.Gui.Views.TabChangedEventArgs -> unit> = PropKey.Create.event "TabView.SelectedTabChanged_event"
    member val TabClicked: TypedPropKey<Terminal.Gui.Views.TabMouseEventArgs -> unit> = PropKey.Create.event "TabView.TabClicked_event"

  type TableViewPKeys() =
    inherit ViewPKeys()

    // Properties
    member val CellActivationKey: TypedPropKey<Terminal.Gui.Drivers.KeyCode> = PropKey.Create.simple "TableView.CellActivationKey"
    member val CollectionNavigator: TypedPropKey<Terminal.Gui.Views.ICollectionNavigator> = PropKey.Create.simple "TableView.CollectionNavigator"
    member val ColumnOffset: TypedPropKey<int> = PropKey.Create.simple "TableView.ColumnOffset"
    member val FullRowSelect: TypedPropKey<bool> = PropKey.Create.simple "TableView.FullRowSelect"
    member val MaxCellWidth: TypedPropKey<int> = PropKey.Create.simple "TableView.MaxCellWidth"
    member val MinCellWidth: TypedPropKey<int> = PropKey.Create.simple "TableView.MinCellWidth"
    member val MultiSelect: TypedPropKey<bool> = PropKey.Create.simple "TableView.MultiSelect"
    member val NullSymbol: TypedPropKey<string> = PropKey.Create.simple "TableView.NullSymbol"
    member val RowOffset: TypedPropKey<int> = PropKey.Create.simple "TableView.RowOffset"
    member val SelectedColumn: TypedPropKey<int> = PropKey.Create.simple "TableView.SelectedColumn"
    member val SelectedRow: TypedPropKey<int> = PropKey.Create.simple "TableView.SelectedRow"
    member val SeparatorSymbol: TypedPropKey<System.Char> = PropKey.Create.simple "TableView.SeparatorSymbol"
    member val Style: TypedPropKey<Terminal.Gui.Views.TableStyle> = PropKey.Create.simple "TableView.Style"
    member val Table: TypedPropKey<Terminal.Gui.Views.ITableSource> = PropKey.Create.simple "TableView.Table"

    // Events
    member val CellActivated: TypedPropKey<Terminal.Gui.Views.CellActivatedEventArgs -> unit> = PropKey.Create.event "TableView.CellActivated_event"
    member val CellToggled: TypedPropKey<Terminal.Gui.Views.CellToggledEventArgs -> unit> = PropKey.Create.event "TableView.CellToggled_event"
    member val SelectedCellChanged: TypedPropKey<Terminal.Gui.Views.SelectedCellChangedEventArgs -> unit> = PropKey.Create.event "TableView.SelectedCellChanged_event"

  type TextFieldPKeys() =
    inherit ViewPKeys()

    // Properties
    member val Autocomplete: TypedPropKey<Terminal.Gui.Views.IAutocomplete> = PropKey.Create.simple "TextField.Autocomplete"
    member val InsertionPoint: TypedPropKey<int> = PropKey.Create.simple "TextField.InsertionPoint"
    member val ReadOnly: TypedPropKey<bool> = PropKey.Create.simple "TextField.ReadOnly"
    member val Secret: TypedPropKey<bool> = PropKey.Create.simple "TextField.Secret"
    member val SelectWordOnlyOnDoubleClick: TypedPropKey<bool> = PropKey.Create.simple "TextField.SelectWordOnlyOnDoubleClick"
    member val SelectedStart: TypedPropKey<int> = PropKey.Create.simple "TextField.SelectedStart"
    member val Text: TypedPropKey<string> = PropKey.Create.simple "TextField.Text"
    member val UseSameRuneTypeForWords: TypedPropKey<bool> = PropKey.Create.simple "TextField.UseSameRuneTypeForWords"
    member val Used: TypedPropKey<bool> = PropKey.Create.simple "TextField.Used"
    member val Value: TypedPropKey<string> = PropKey.Create.simple "TextField.Value"

    // Events
    member val TextChanging: TypedPropKey<ResultEventArgs<string> -> unit> = PropKey.Create.event "TextField.TextChanging_event"
    member val ValueChanged: TypedPropKey<ValueChangedEventArgs<string> -> unit> = PropKey.Create.event "TextField.ValueChanged_event"
    member val ValueChanging: TypedPropKey<ValueChangingEventArgs<string> -> unit> = PropKey.Create.event "TextField.ValueChanging_event"

  type DateFieldPKeys() =
    inherit TextFieldPKeys()

    // Properties
    member val Culture: TypedPropKey<System.Globalization.CultureInfo> = PropKey.Create.simple "DateField.Culture"
    member val InsertionPoint: TypedPropKey<int> = PropKey.Create.simple "DateField.InsertionPoint"
    member val Value: TypedPropKey<Nullable<System.DateTime>> = PropKey.Create.simple "DateField.Value"

    // Events
    member val ValueChanged: TypedPropKey<ValueChangedEventArgs<Nullable<System.DateTime>> -> unit> = PropKey.Create.event "DateField.ValueChanged_event"
    member val ValueChanging: TypedPropKey<ValueChangingEventArgs<Nullable<System.DateTime>> -> unit> = PropKey.Create.event "DateField.ValueChanging_event"

  type TextValidateFieldPKeys() =
    inherit ViewPKeys()

    // Properties
    member val Provider: TypedPropKey<Terminal.Gui.Views.ITextValidateProvider> = PropKey.Create.simple "TextValidateField.Provider"
    member val Text: TypedPropKey<string> = PropKey.Create.simple "TextValidateField.Text"

  type TextViewPKeys() =
    inherit ViewPKeys()

    // Properties
    member val EnterKeyAddsLine: TypedPropKey<bool> = PropKey.Create.simple "TextView.EnterKeyAddsLine"
    member val InheritsPreviousAttribute: TypedPropKey<bool> = PropKey.Create.simple "TextView.InheritsPreviousAttribute"
    member val InsertionPoint: TypedPropKey<System.Drawing.Point> = PropKey.Create.simple "TextView.InsertionPoint"
    member val IsDirty: TypedPropKey<bool> = PropKey.Create.simple "TextView.IsDirty"
    member val IsSelecting: TypedPropKey<bool> = PropKey.Create.simple "TextView.IsSelecting"
    member val LeftColumn: TypedPropKey<int> = PropKey.Create.simple "TextView.LeftColumn"
    member val Multiline: TypedPropKey<bool> = PropKey.Create.simple "TextView.Multiline"
    member val ReadOnly: TypedPropKey<bool> = PropKey.Create.simple "TextView.ReadOnly"
    member val ScrollBars: TypedPropKey<bool> = PropKey.Create.simple "TextView.ScrollBars"
    member val SelectWordOnlyOnDoubleClick: TypedPropKey<bool> = PropKey.Create.simple "TextView.SelectWordOnlyOnDoubleClick"
    member val SelectionStartColumn: TypedPropKey<int> = PropKey.Create.simple "TextView.SelectionStartColumn"
    member val SelectionStartRow: TypedPropKey<int> = PropKey.Create.simple "TextView.SelectionStartRow"
    member val TabKeyAddsTab: TypedPropKey<bool> = PropKey.Create.simple "TextView.TabKeyAddsTab"
    member val TabWidth: TypedPropKey<int> = PropKey.Create.simple "TextView.TabWidth"
    member val Text: TypedPropKey<string> = PropKey.Create.simple "TextView.Text"
    member val TopRow: TypedPropKey<int> = PropKey.Create.simple "TextView.TopRow"
    member val UseSameRuneTypeForWords: TypedPropKey<bool> = PropKey.Create.simple "TextView.UseSameRuneTypeForWords"
    member val Used: TypedPropKey<bool> = PropKey.Create.simple "TextView.Used"
    member val WordWrap: TypedPropKey<bool> = PropKey.Create.simple "TextView.WordWrap"

    // Events
    member val ContentsChanged: TypedPropKey<Terminal.Gui.Views.ContentsChangedEventArgs -> unit> = PropKey.Create.event "TextView.ContentsChanged_event"
    member val DrawNormalColor: TypedPropKey<Terminal.Gui.Drawing.CellEventArgs -> unit> = PropKey.Create.event "TextView.DrawNormalColor_event"
    member val DrawReadOnlyColor: TypedPropKey<Terminal.Gui.Drawing.CellEventArgs -> unit> = PropKey.Create.event "TextView.DrawReadOnlyColor_event"
    member val DrawSelectionColor: TypedPropKey<Terminal.Gui.Drawing.CellEventArgs -> unit> = PropKey.Create.event "TextView.DrawSelectionColor_event"
    member val DrawUsedColor: TypedPropKey<Terminal.Gui.Drawing.CellEventArgs -> unit> = PropKey.Create.event "TextView.DrawUsedColor_event"
    member val UnwrappedCursorPosition: TypedPropKey<System.Drawing.Point -> unit> = PropKey.Create.event "TextView.UnwrappedCursorPosition_event"

  type TimeFieldPKeys() =
    inherit TextFieldPKeys()

    // Properties
    member val InsertionPoint: TypedPropKey<int> = PropKey.Create.simple "TimeField.InsertionPoint"
    member val IsShortFormat: TypedPropKey<bool> = PropKey.Create.simple "TimeField.IsShortFormat"
    member val Value: TypedPropKey<System.TimeSpan> = PropKey.Create.simple "TimeField.Value"

    // Events
    member val ValueChanged: TypedPropKey<ValueChangedEventArgs<System.TimeSpan> -> unit> = PropKey.Create.event "TimeField.ValueChanged_event"
    member val ValueChanging: TypedPropKey<ValueChangingEventArgs<System.TimeSpan> -> unit> = PropKey.Create.event "TimeField.ValueChanging_event"

  type TreeViewPKeys<'T when 'T: not struct>() =
    inherit ViewPKeys()

    // Properties
    member val AllowLetterBasedNavigation: TypedPropKey<bool> = PropKey.Create.simple "TreeView.AllowLetterBasedNavigation"
    member val AspectGetter: TypedPropKey<AspectGetterDelegate<'T>> = PropKey.Create.simple "TreeView.AspectGetter"
    member val ColorGetter: TypedPropKey<Func<'T, Terminal.Gui.Drawing.Scheme>> = PropKey.Create.simple "TreeView.ColorGetter"
    member val MaxDepth: TypedPropKey<int> = PropKey.Create.simple "TreeView.MaxDepth"
    member val MultiSelect: TypedPropKey<bool> = PropKey.Create.simple "TreeView.MultiSelect"
    member val ObjectActivationButton: TypedPropKey<Nullable<Terminal.Gui.Input.MouseFlags>> = PropKey.Create.simple "TreeView.ObjectActivationButton"
    member val ObjectActivationKey: TypedPropKey<Terminal.Gui.Drivers.KeyCode> = PropKey.Create.simple "TreeView.ObjectActivationKey"
    member val ScrollOffsetHorizontal: TypedPropKey<int> = PropKey.Create.simple "TreeView.ScrollOffsetHorizontal"
    member val ScrollOffsetVertical: TypedPropKey<int> = PropKey.Create.simple "TreeView.ScrollOffsetVertical"
    member val SelectedObject: TypedPropKey<'T> = PropKey.Create.simple "TreeView.SelectedObject"
    member val Style: TypedPropKey<Terminal.Gui.Views.TreeStyle> = PropKey.Create.simple "TreeView.Style"
    member val TreeBuilder: TypedPropKey<ITreeBuilder<'T>> = PropKey.Create.simple "TreeView.TreeBuilder"

    // Events
    member val DrawLine: TypedPropKey<DrawTreeViewLineEventArgs<'T> -> unit> = PropKey.Create.event "TreeView.DrawLine_event"
    member val ObjectActivated: TypedPropKey<ObjectActivatedEventArgs<'T> -> unit> = PropKey.Create.event "TreeView.ObjectActivated_event"
    member val SelectionChanged: TypedPropKey<SelectionChangedEventArgs<'T> -> unit> = PropKey.Create.event "TreeView.SelectionChanged_event"

  type TreeViewPKeys() =
    inherit TreeViewPKeys<Terminal.Gui.Views.ITreeNode>()


  type WindowPKeys() =
    inherit RunnablePKeys()


  type WizardPKeys() =
    inherit DialogPKeys()

    // Properties
    member val CurrentStep: TypedPropKey<Terminal.Gui.Views.WizardStep> = PropKey.Create.view "Wizard.CurrentStep_view"
    member val CurrentStep_element: TypedPropKey<IWizardStepTerminalElement> = PropKey.Create.singleElement "Wizard.CurrentStep_element"

    // Events
    member val MovingBack: TypedPropKey<System.ComponentModel.CancelEventArgs -> unit> = PropKey.Create.event "Wizard.MovingBack_event"
    member val MovingNext: TypedPropKey<System.ComponentModel.CancelEventArgs -> unit> = PropKey.Create.event "Wizard.MovingNext_event"
    member val StepChanged: TypedPropKey<ValueChangedEventArgs<Terminal.Gui.Views.WizardStep> -> unit> = PropKey.Create.event "Wizard.StepChanged_event"
    member val StepChanging: TypedPropKey<ValueChangingEventArgs<Terminal.Gui.Views.WizardStep> -> unit> = PropKey.Create.event "Wizard.StepChanging_event"

  type WizardStepPKeys() =
    inherit ViewPKeys()

    // Properties
    member val BackButtonText: TypedPropKey<string> = PropKey.Create.simple "WizardStep.BackButtonText"
    member val HelpText: TypedPropKey<string> = PropKey.Create.simple "WizardStep.HelpText"
    member val NextButtonText: TypedPropKey<string> = PropKey.Create.simple "WizardStep.NextButtonText"

  module internal IMouseHoldRepeaterInterface =
    // Properties
    let Timeout: TypedPropKey<Terminal.Gui.App.Timeout> = PropKey.Create.simple "IMouseHoldRepeaterInterface.Timeout"

    // Events
    let MouseIsHeldDownTick: TypedPropKey<CancelEventArgs<Terminal.Gui.Input.Mouse> -> unit> = PropKey.Create.event "IMouseHoldRepeaterInterface.MouseIsHeldDownTick_event"

  module internal IOrientationInterface =
    // Properties
    let Orientation: TypedPropKey<Terminal.Gui.ViewBase.Orientation> = PropKey.Create.simple "IOrientationInterface.Orientation"

    // Events
    let OrientationChanged: TypedPropKey<EventArgs<Terminal.Gui.ViewBase.Orientation> -> unit> = PropKey.Create.event "IOrientationInterface.OrientationChanged_event"

    let OrientationChanging: TypedPropKey<CancelEventArgs<Terminal.Gui.ViewBase.Orientation> -> unit> = PropKey.Create.event "IOrientationInterface.OrientationChanging_event"

  module internal IValueInterface =
    // Properties
    let Value<'TValue>: TypedPropKey<'TValue> = PropKey.Create.simple "IValueInterface.Value"

    // Events
    let ValueChanged<'TValue>: TypedPropKey<ValueChangedEventArgs<'TValue> -> unit> = PropKey.Create.event "IValueInterface.ValueChanged_event"

    let ValueChanging<'TValue>: TypedPropKey<ValueChangingEventArgs<'TValue> -> unit> = PropKey.Create.event "IValueInterface.ValueChanging_event"


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
  let LinearRange' = LinearRangePKeys()
  let ListView = ListViewPKeys()
  let Margin = MarginPKeys()
  let Menu = MenuPKeys()
  let MenuBar = MenuBarPKeys()
  let NumericUpDown<'T> = NumericUpDownPKeys<'T>()
  let NumericUpDown' = NumericUpDownPKeys()
  let Padding = PaddingPKeys()
  let PopoverBaseImpl = PopoverBaseImplPKeys()
  let PopoverMenu = PopoverMenuPKeys()
  let ProgressBar = ProgressBarPKeys()
  let Runnable = RunnablePKeys()
  let Runnable'<'TResult> = RunnablePKeys<'TResult>()
  let Dialog<'TResult> = DialogPKeys<'TResult>()
  let Dialog' = DialogPKeys()
  let Prompt<'TView, 'TResult when 'TView: (new: unit -> 'TView) and 'TView:> Terminal.Gui.ViewBase.View> = PromptPKeys<'TView, 'TResult>()
  let FileDialog = FileDialogPKeys()
  let OpenDialog = OpenDialogPKeys()
  let SaveDialog = SaveDialogPKeys()
  let ScrollBar = ScrollBarPKeys()
  let ScrollSlider = ScrollSliderPKeys()
  let SelectorBase = SelectorBasePKeys()
  let FlagSelector = FlagSelectorPKeys()
  let OptionSelector = OptionSelectorPKeys()
  let FlagSelector'<'TFlagsEnum when 'TFlagsEnum: struct and 'TFlagsEnum: (new: unit -> 'TFlagsEnum) and 'TFlagsEnum:> System.Enum and 'TFlagsEnum:> System.ValueType> = FlagSelectorPKeys<'TFlagsEnum>()
  let OptionSelector'<'TEnum when 'TEnum: struct and 'TEnum: (new: unit -> 'TEnum) and 'TEnum:> System.Enum and 'TEnum:> System.ValueType> = OptionSelectorPKeys<'TEnum>()
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
  let TreeView' = TreeViewPKeys()
  let Window = WindowPKeys()
  let Wizard = WizardPKeys()
  let WizardStep = WizardStepPKeys()