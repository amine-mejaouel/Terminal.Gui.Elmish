namespace Terminal.Gui.Elmish

open System
open System.Collections.Generic
open Terminal.Gui.App
open Terminal.Gui.Elmish
open Terminal.Gui.ViewBase
open Terminal.Gui.Views

type ViewProps() =
  member val internal props = Props()

  member this.Children(children: ITerminalElement list) =
    children
    |> List.map (fun x -> TerminalElement.from x)
    |> this.props.Children.AddRange
  
  // Positions
  member this.X (value: Pos) =
    this.props.X <- Some value

  member this.Y (value: Pos) =
    this.props.Y <- Some value

  // Delayed Positions
  member this.X (value: TPos) =
    this.props.XDelayed <- Some value

  member this.Y (value: TPos) =
    this.props.YDelayed <- Some value

  // Properties
  member this.Arrangement (value: Terminal.Gui.ViewBase.ViewArrangement) =
    this.props.add (PKey.View.Arrangement, value)


  member this.AssignHotKeys (value: bool) =
    this.props.add (PKey.View.AssignHotKeys, value)


  member this.BorderStyle (value: Terminal.Gui.Drawing.LineStyle) =
    this.props.add (PKey.View.BorderStyle, value)


  member this.CanFocus (value: bool) =
    this.props.add (PKey.View.CanFocus, value)


  member this.ContentSizeTracksViewport (value: bool) =
    this.props.add (PKey.View.ContentSizeTracksViewport, value)


  member this.Cursor (value: Terminal.Gui.Drivers.Cursor) =
    this.props.add (PKey.View.Cursor, value)


  member this.Data (value: System.Object) =
    this.props.add (PKey.View.Data, value)


  member this.Enabled (value: bool) =
    this.props.add (PKey.View.Enabled, value)


  member this.Frame (value: System.Drawing.Rectangle) =
    this.props.add (PKey.View.Frame, value)


  member this.HasFocus (value: bool) =
    this.props.add (PKey.View.HasFocus, value)


  member this.Height (value: Terminal.Gui.ViewBase.Dim) =
    this.props.add (PKey.View.Height, value)


  member this.HotKey (value: Terminal.Gui.Input.Key) =
    this.props.add (PKey.View.HotKey, value)


  member this.HotKeySpecifier (value: System.Text.Rune) =
    this.props.add (PKey.View.HotKeySpecifier, value)


  member this.Id (value: string) =
    this.props.add (PKey.View.Id, value)


  member this.IsInitialized (value: bool) =
    this.props.add (PKey.View.IsInitialized, value)


  member this.MouseHighlightStates (value: Terminal.Gui.ViewBase.MouseState) =
    this.props.add (PKey.View.MouseHighlightStates, value)


  member this.MouseHoldRepeat (value: Nullable<Terminal.Gui.Input.MouseFlags>) =
    this.props.add (PKey.View.MouseHoldRepeat, value)


  member this.MousePositionTracking (value: bool) =
    this.props.add (PKey.View.MousePositionTracking, value)


  member this.PreserveTrailingSpaces (value: bool) =
    this.props.add (PKey.View.PreserveTrailingSpaces, value)


  member this.SchemeName (value: string) =
    this.props.add (PKey.View.SchemeName, value)


  member this.ShadowStyle (value: Terminal.Gui.ViewBase.ShadowStyle) =
    this.props.add (PKey.View.ShadowStyle, value)


  member this.SuperViewRendersLineCanvas (value: bool) =
    this.props.add (PKey.View.SuperViewRendersLineCanvas, value)


  member this.TabStop (value: Nullable<Terminal.Gui.ViewBase.TabBehavior>) =
    this.props.add (PKey.View.TabStop, value)


  member this.Text (value: string) =
    this.props.add (PKey.View.Text, value)


  member this.TextAlignment (value: Terminal.Gui.ViewBase.Alignment) =
    this.props.add (PKey.View.TextAlignment, value)


  member this.TextDirection (value: Terminal.Gui.Text.TextDirection) =
    this.props.add (PKey.View.TextDirection, value)


  member this.Title (value: string) =
    this.props.add (PKey.View.Title, value)


  member this.UsedHotKeys (value: HashSet<Terminal.Gui.Input.Key>) =
    this.props.add (PKey.View.UsedHotKeys, value)


  member this.ValidatePosDim (value: bool) =
    this.props.add (PKey.View.ValidatePosDim, value)


  member this.VerticalTextAlignment (value: Terminal.Gui.ViewBase.Alignment) =
    this.props.add (PKey.View.VerticalTextAlignment, value)


  member this.Viewport (value: System.Drawing.Rectangle) =
    this.props.add (PKey.View.Viewport, value)


  member this.ViewportSettings (value: Terminal.Gui.ViewBase.ViewportSettingsFlags) =
    this.props.add (PKey.View.ViewportSettings, value)


  member this.Visible (value: bool) =
    this.props.add (PKey.View.Visible, value)


  member this.Width (value: Terminal.Gui.ViewBase.Dim) =
    this.props.add (PKey.View.Width, value)


  // Events
  member this.Accepted (handler: Terminal.Gui.Input.CommandEventArgs -> unit) =
    this.props.add (PKey.View.Accepted, handler)

  member this.Accepting (handler: Terminal.Gui.Input.CommandEventArgs -> unit) =
    this.props.add (PKey.View.Accepting, handler)

  member this.Activating (handler: Terminal.Gui.Input.CommandEventArgs -> unit) =
    this.props.add (PKey.View.Activating, handler)

  member this.AdvancingFocus (handler: Terminal.Gui.ViewBase.AdvanceFocusEventArgs -> unit) =
    this.props.add (PKey.View.AdvancingFocus, handler)

  member this.BorderStyleChanged (handler: System.EventArgs -> unit) =
    this.props.add (PKey.View.BorderStyleChanged, handler)

  member this.CanFocusChanged (handler: System.EventArgs -> unit) =
    this.props.add (PKey.View.CanFocusChanged, handler)

  member this.ClearedViewport (handler: Terminal.Gui.ViewBase.DrawEventArgs -> unit) =
    this.props.add (PKey.View.ClearedViewport, handler)

  member this.ClearingViewport (handler: Terminal.Gui.ViewBase.DrawEventArgs -> unit) =
    this.props.add (PKey.View.ClearingViewport, handler)

  member this.CommandNotBound (handler: Terminal.Gui.Input.CommandEventArgs -> unit) =
    this.props.add (PKey.View.CommandNotBound, handler)

  member this.ContentSizeChanged (handler: ValueChangedEventArgs<Nullable<System.Drawing.Size>> -> unit) =
    this.props.add (PKey.View.ContentSizeChanged, handler)

  member this.ContentSizeChanging (handler: ValueChangingEventArgs<Nullable<System.Drawing.Size>> -> unit) =
    this.props.add (PKey.View.ContentSizeChanging, handler)

  member this.Disposing (handler: System.EventArgs -> unit) =
    this.props.add (PKey.View.Disposing, handler)

  member this.DrawComplete (handler: Terminal.Gui.ViewBase.DrawEventArgs -> unit) =
    this.props.add (PKey.View.DrawComplete, handler)

  member this.DrawingContent (handler: Terminal.Gui.ViewBase.DrawEventArgs -> unit) =
    this.props.add (PKey.View.DrawingContent, handler)

  member this.DrawingSubViews (handler: Terminal.Gui.ViewBase.DrawEventArgs -> unit) =
    this.props.add (PKey.View.DrawingSubViews, handler)

  member this.DrawingText (handler: Terminal.Gui.ViewBase.DrawEventArgs -> unit) =
    this.props.add (PKey.View.DrawingText, handler)

  member this.DrewText (handler: System.EventArgs -> unit) =
    this.props.add (PKey.View.DrewText, handler)

  member this.EnabledChanged (handler: System.EventArgs -> unit) =
    this.props.add (PKey.View.EnabledChanged, handler)

  member this.FocusedChanged (handler: Terminal.Gui.ViewBase.HasFocusEventArgs -> unit) =
    this.props.add (PKey.View.FocusedChanged, handler)

  member this.FrameChanged (handler: EventArgs<System.Drawing.Rectangle> -> unit) =
    this.props.add (PKey.View.FrameChanged, handler)

  member this.GettingAttributeForRole (handler: Terminal.Gui.Drawing.VisualRoleEventArgs -> unit) =
    this.props.add (PKey.View.GettingAttributeForRole, handler)

  member this.GettingScheme (handler: ResultEventArgs<Terminal.Gui.Drawing.Scheme> -> unit) =
    this.props.add (PKey.View.GettingScheme, handler)

  member this.HandlingHotKey (handler: Terminal.Gui.Input.CommandEventArgs -> unit) =
    this.props.add (PKey.View.HandlingHotKey, handler)

  member this.HasFocusChanged (handler: Terminal.Gui.ViewBase.HasFocusEventArgs -> unit) =
    this.props.add (PKey.View.HasFocusChanged, handler)

  member this.HasFocusChanging (handler: Terminal.Gui.ViewBase.HasFocusEventArgs -> unit) =
    this.props.add (PKey.View.HasFocusChanging, handler)

  member this.HeightChanged (handler: ValueChangedEventArgs<Terminal.Gui.ViewBase.Dim> -> unit) =
    this.props.add (PKey.View.HeightChanged, handler)

  member this.HeightChanging (handler: ValueChangingEventArgs<Terminal.Gui.ViewBase.Dim> -> unit) =
    this.props.add (PKey.View.HeightChanging, handler)

  member this.HotKeyChanged (handler: Terminal.Gui.Input.KeyChangedEventArgs -> unit) =
    this.props.add (PKey.View.HotKeyChanged, handler)

  member this.Initialized (handler: System.EventArgs -> unit) =
    this.props.add (PKey.View.Initialized, handler)

  member this.KeyDown (handler: Terminal.Gui.Input.Key -> unit) =
    this.props.add (PKey.View.KeyDown, handler)

  member this.KeyDownNotHandled (handler: Terminal.Gui.Input.Key -> unit) =
    this.props.add (PKey.View.KeyDownNotHandled, handler)

  member this.MouseEnter (handler: System.ComponentModel.CancelEventArgs -> unit) =
    this.props.add (PKey.View.MouseEnter, handler)

  member this.MouseEvent (handler: Terminal.Gui.Input.Mouse -> unit) =
    this.props.add (PKey.View.MouseEvent, handler)

  member this.MouseHoldRepeatChanged (handler: ValueChangedEventArgs<Nullable<Terminal.Gui.Input.MouseFlags>> -> unit) =
    this.props.add (PKey.View.MouseHoldRepeatChanged, handler)

  member this.MouseHoldRepeatChanging (handler: ValueChangingEventArgs<Nullable<Terminal.Gui.Input.MouseFlags>> -> unit) =
    this.props.add (PKey.View.MouseHoldRepeatChanging, handler)

  member this.MouseLeave (handler: System.EventArgs -> unit) =
    this.props.add (PKey.View.MouseLeave, handler)

  member this.MouseStateChanged (handler: EventArgs<Terminal.Gui.ViewBase.MouseState> -> unit) =
    this.props.add (PKey.View.MouseStateChanged, handler)

  member this.Removed (handler: Terminal.Gui.ViewBase.SuperViewChangedEventArgs -> unit) =
    this.props.add (PKey.View.Removed, handler)

  member this.SchemeChanged (handler: ValueChangedEventArgs<Terminal.Gui.Drawing.Scheme> -> unit) =
    this.props.add (PKey.View.SchemeChanged, handler)

  member this.SchemeChanging (handler: ValueChangingEventArgs<Terminal.Gui.Drawing.Scheme> -> unit) =
    this.props.add (PKey.View.SchemeChanging, handler)

  member this.SchemeNameChanged (handler: ValueChangedEventArgs<string> -> unit) =
    this.props.add (PKey.View.SchemeNameChanged, handler)

  member this.SchemeNameChanging (handler: ValueChangingEventArgs<string> -> unit) =
    this.props.add (PKey.View.SchemeNameChanging, handler)

  member this.SubViewAdded (handler: Terminal.Gui.ViewBase.SuperViewChangedEventArgs -> unit) =
    this.props.add (PKey.View.SubViewAdded, handler)

  member this.SubViewLayout (handler: Terminal.Gui.ViewBase.LayoutEventArgs -> unit) =
    this.props.add (PKey.View.SubViewLayout, handler)

  member this.SubViewRemoved (handler: Terminal.Gui.ViewBase.SuperViewChangedEventArgs -> unit) =
    this.props.add (PKey.View.SubViewRemoved, handler)

  member this.SubViewsLaidOut (handler: Terminal.Gui.ViewBase.LayoutEventArgs -> unit) =
    this.props.add (PKey.View.SubViewsLaidOut, handler)

  member this.SuperViewChanged (handler: ValueChangedEventArgs<Terminal.Gui.ViewBase.View> -> unit) =
    this.props.add (PKey.View.SuperViewChanged, handler)

  member this.SuperViewChanging (handler: ValueChangingEventArgs<Terminal.Gui.ViewBase.View> -> unit) =
    this.props.add (PKey.View.SuperViewChanging, handler)

  member this.TextChanged (handler: System.EventArgs -> unit) =
    this.props.add (PKey.View.TextChanged, handler)

  member this.TitleChanged (handler: EventArgs<string> -> unit) =
    this.props.add (PKey.View.TitleChanged, handler)

  member this.TitleChanging (handler: CancelEventArgs<string> -> unit) =
    this.props.add (PKey.View.TitleChanging, handler)

  member this.ViewportChanged (handler: Terminal.Gui.ViewBase.DrawEventArgs -> unit) =
    this.props.add (PKey.View.ViewportChanged, handler)

  member this.VisibleChanged (handler: System.EventArgs -> unit) =
    this.props.add (PKey.View.VisibleChanged, handler)

  member this.VisibleChanging (handler: CancelEventArgs<bool> -> unit) =
    this.props.add (PKey.View.VisibleChanging, handler)

  member this.WidthChanged (handler: ValueChangedEventArgs<Terminal.Gui.ViewBase.Dim> -> unit) =
    this.props.add (PKey.View.WidthChanged, handler)

  member this.WidthChanging (handler: ValueChangingEventArgs<Terminal.Gui.ViewBase.Dim> -> unit) =
    this.props.add (PKey.View.WidthChanging, handler)

type AdornmentProps() =
  inherit ViewProps()
  // Properties
  member this.Diagnostics (value: Terminal.Gui.ViewBase.ViewDiagnosticFlags) =
    this.props.add (PKey.Adornment.Diagnostics, value)


  member this.Parent (value: Terminal.Gui.ViewBase.View) =
    this.props.add (PKey.Adornment.Parent, value)

  member this.Parent(value: ITerminalElement) =
    this.props.add (PKey.Adornment.Parent_element, value)

  member this.SuperViewRendersLineCanvas (value: bool) =
    this.props.add (PKey.Adornment.SuperViewRendersLineCanvas, value)


  member this.Thickness (value: Terminal.Gui.Drawing.Thickness) =
    this.props.add (PKey.Adornment.Thickness, value)


  member this.Viewport (value: System.Drawing.Rectangle) =
    this.props.add (PKey.Adornment.Viewport, value)


  // Events
  member this.ThicknessChanged (handler: System.EventArgs -> unit) =
    this.props.add (PKey.Adornment.ThicknessChanged, handler)

type AttributePickerProps() =
  inherit ViewProps()
  // Properties
  member this.SampleText (value: string) =
    this.props.add (PKey.AttributePicker.SampleText, value)


  member this.Value (value: Nullable<Terminal.Gui.Drawing.Attribute>) =
    this.props.add (PKey.AttributePicker.Value, value)


  // Events
  member this.ValueChanged (handler: ValueChangedEventArgs<Nullable<Terminal.Gui.Drawing.Attribute>> -> unit) =
    this.props.add (PKey.AttributePicker.ValueChanged, handler)

  member this.ValueChanging (handler: ValueChangingEventArgs<Nullable<Terminal.Gui.Drawing.Attribute>> -> unit) =
    this.props.add (PKey.AttributePicker.ValueChanging, handler)

type BarProps() =
  inherit ViewProps()
  // Properties
  member this.AlignmentModes (value: Terminal.Gui.ViewBase.AlignmentModes) =
    this.props.add (PKey.Bar.AlignmentModes, value)


  member this.Orientation (value: Terminal.Gui.ViewBase.Orientation) =
    this.props.add (PKey.Bar.Orientation, value)


  // Events
  member this.OrientationChanged (handler: EventArgs<Terminal.Gui.ViewBase.Orientation> -> unit) =
    this.props.add (PKey.Bar.OrientationChanged, handler)

  member this.OrientationChanging (handler: CancelEventArgs<Terminal.Gui.ViewBase.Orientation> -> unit) =
    this.props.add (PKey.Bar.OrientationChanging, handler)

type BorderProps() =
  inherit AdornmentProps()
  // Properties
  member this.LineStyle (value: Terminal.Gui.Drawing.LineStyle) =
    this.props.add (PKey.Border.LineStyle, value)


  member this.Settings (value: Terminal.Gui.ViewBase.BorderSettings) =
    this.props.add (PKey.Border.Settings, value)


type ButtonProps() =
  inherit ViewProps()
  // Properties
  member this.HotKeySpecifier (value: System.Text.Rune) =
    this.props.add (PKey.Button.HotKeySpecifier, value)


  member this.IsDefault (value: bool) =
    this.props.add (PKey.Button.IsDefault, value)


  member this.NoDecorations (value: bool) =
    this.props.add (PKey.Button.NoDecorations, value)


  member this.NoPadding (value: bool) =
    this.props.add (PKey.Button.NoPadding, value)


  member this.Text (value: string) =
    this.props.add (PKey.Button.Text, value)


type CharMapProps() =
  inherit ViewProps()
  // Properties
  member this.SelectedCodePoint (value: int) =
    this.props.add (PKey.CharMap.SelectedCodePoint, value)


  member this.ShowGlyphWidths (value: bool) =
    this.props.add (PKey.CharMap.ShowGlyphWidths, value)


  member this.ShowUnicodeCategory (value: Nullable<System.Globalization.UnicodeCategory>) =
    this.props.add (PKey.CharMap.ShowUnicodeCategory, value)


  member this.StartCodePoint (value: int) =
    this.props.add (PKey.CharMap.StartCodePoint, value)


  member this.Value (value: System.Text.Rune) =
    this.props.add (PKey.CharMap.Value, value)


  // Events
  member this.ValueChanged (handler: ValueChangedEventArgs<System.Text.Rune> -> unit) =
    this.props.add (PKey.CharMap.ValueChanged, handler)

  member this.ValueChanging (handler: ValueChangingEventArgs<System.Text.Rune> -> unit) =
    this.props.add (PKey.CharMap.ValueChanging, handler)

type CheckBoxProps() =
  inherit ViewProps()
  // Properties
  member this.AllowCheckStateNone (value: bool) =
    this.props.add (PKey.CheckBox.AllowCheckStateNone, value)


  member this.HotKeySpecifier (value: System.Text.Rune) =
    this.props.add (PKey.CheckBox.HotKeySpecifier, value)


  member this.RadioStyle (value: bool) =
    this.props.add (PKey.CheckBox.RadioStyle, value)


  member this.Text (value: string) =
    this.props.add (PKey.CheckBox.Text, value)


  member this.Value (value: Terminal.Gui.Views.CheckState) =
    this.props.add (PKey.CheckBox.Value, value)


  // Events
  member this.ValueChanged (handler: ValueChangedEventArgs<Terminal.Gui.Views.CheckState> -> unit) =
    this.props.add (PKey.CheckBox.ValueChanged, handler)

  member this.ValueChanging (handler: ValueChangingEventArgs<Terminal.Gui.Views.CheckState> -> unit) =
    this.props.add (PKey.CheckBox.ValueChanging, handler)

type ColorPickerProps() =
  inherit ViewProps()
  // Properties
  member this.SelectedColor (value: Terminal.Gui.Drawing.Color) =
    this.props.add (PKey.ColorPicker.SelectedColor, value)


  member this.Style (value: Terminal.Gui.Views.ColorPickerStyle) =
    this.props.add (PKey.ColorPicker.Style, value)


  member this.Text (value: string) =
    this.props.add (PKey.ColorPicker.Text, value)


  member this.Value (value: Nullable<Terminal.Gui.Drawing.Color>) =
    this.props.add (PKey.ColorPicker.Value, value)


  // Events
  member this.ValueChanged (handler: ValueChangedEventArgs<Nullable<Terminal.Gui.Drawing.Color>> -> unit) =
    this.props.add (PKey.ColorPicker.ValueChanged, handler)

  member this.ValueChanging (handler: ValueChangingEventArgs<Nullable<Terminal.Gui.Drawing.Color>> -> unit) =
    this.props.add (PKey.ColorPicker.ValueChanging, handler)

type ColorPicker16Props() =
  inherit ViewProps()
  // Properties
  member this.BoxHeight (value: int) =
    this.props.add (PKey.ColorPicker16.BoxHeight, value)


  member this.BoxWidth (value: int) =
    this.props.add (PKey.ColorPicker16.BoxWidth, value)


  member this.Caret (value: System.Drawing.Point) =
    this.props.add (PKey.ColorPicker16.Caret, value)


  member this.SelectedColor (value: Terminal.Gui.Drawing.ColorName16) =
    this.props.add (PKey.ColorPicker16.SelectedColor, value)


  member this.Value (value: Terminal.Gui.Drawing.ColorName16) =
    this.props.add (PKey.ColorPicker16.Value, value)


  // Events
  member this.ValueChanged (handler: ValueChangedEventArgs<Terminal.Gui.Drawing.ColorName16> -> unit) =
    this.props.add (PKey.ColorPicker16.ValueChanged, handler)

  member this.ValueChanging (handler: ValueChangingEventArgs<Terminal.Gui.Drawing.ColorName16> -> unit) =
    this.props.add (PKey.ColorPicker16.ValueChanging, handler)

type ComboBoxProps() =
  inherit ViewProps()
  // Properties
  member this.HideDropdownListOnClick (value: bool) =
    this.props.add (PKey.ComboBox.HideDropdownListOnClick, value)


  member this.ReadOnly (value: bool) =
    this.props.add (PKey.ComboBox.ReadOnly, value)


  member this.SearchText (value: string) =
    this.props.add (PKey.ComboBox.SearchText, value)


  member this.SelectedItem (value: int) =
    this.props.add (PKey.ComboBox.SelectedItem, value)


  member this.Source (value: Terminal.Gui.Views.IListDataSource) =
    this.props.add (PKey.ComboBox.Source, value)


  member this.Text (value: string) =
    this.props.add (PKey.ComboBox.Text, value)


  // Events
  member this.Collapsed (handler: System.EventArgs -> unit) =
    this.props.add (PKey.ComboBox.Collapsed, handler)

  member this.Expanded (handler: System.EventArgs -> unit) =
    this.props.add (PKey.ComboBox.Expanded, handler)

  member this.OpenSelectedItem (handler: Terminal.Gui.Views.ListViewItemEventArgs -> unit) =
    this.props.add (PKey.ComboBox.OpenSelectedItem, handler)

  member this.SelectedItemChanged (handler: Terminal.Gui.Views.ListViewItemEventArgs -> unit) =
    this.props.add (PKey.ComboBox.SelectedItemChanged, handler)

type DatePickerProps() =
  inherit ViewProps()
  // Properties
  member this.Culture (value: System.Globalization.CultureInfo) =
    this.props.add (PKey.DatePicker.Culture, value)


  member this.Text (value: string) =
    this.props.add (PKey.DatePicker.Text, value)


  member this.Value (value: System.DateTime) =
    this.props.add (PKey.DatePicker.Value, value)


  // Events
  member this.ValueChanged (handler: ValueChangedEventArgs<System.DateTime> -> unit) =
    this.props.add (PKey.DatePicker.ValueChanged, handler)

  member this.ValueChanging (handler: ValueChangingEventArgs<System.DateTime> -> unit) =
    this.props.add (PKey.DatePicker.ValueChanging, handler)

type FrameViewProps() =
  inherit ViewProps()
type GraphViewProps() =
  inherit ViewProps()
  // Properties
  member this.AxisX (value: Terminal.Gui.Views.HorizontalAxis) =
    this.props.add (PKey.GraphView.AxisX, value)


  member this.AxisY (value: Terminal.Gui.Views.VerticalAxis) =
    this.props.add (PKey.GraphView.AxisY, value)


  member this.CellSize (value: System.Drawing.PointF) =
    this.props.add (PKey.GraphView.CellSize, value)


  member this.GraphColor (value: Nullable<Terminal.Gui.Drawing.Attribute>) =
    this.props.add (PKey.GraphView.GraphColor, value)


  member this.MarginBottom (value: System.UInt32) =
    this.props.add (PKey.GraphView.MarginBottom, value)


  member this.MarginLeft (value: System.UInt32) =
    this.props.add (PKey.GraphView.MarginLeft, value)


  member this.ScrollOffset (value: System.Drawing.PointF) =
    this.props.add (PKey.GraphView.ScrollOffset, value)


type HexViewProps() =
  inherit ViewProps()
  // Properties
  member this.Address (value: System.Int64) =
    this.props.add (PKey.HexView.Address, value)


  member this.AddressWidth (value: int) =
    this.props.add (PKey.HexView.AddressWidth, value)


  member this.BytesPerLine (value: int) =
    this.props.add (PKey.HexView.BytesPerLine, value)


  member this.ReadOnly (value: bool) =
    this.props.add (PKey.HexView.ReadOnly, value)


  member this.Source (value: System.IO.Stream) =
    this.props.add (PKey.HexView.Source, value)


  // Events
  member this.Edited (handler: Terminal.Gui.Views.HexViewEditEventArgs -> unit) =
    this.props.add (PKey.HexView.Edited, handler)

  member this.PositionChanged (handler: Terminal.Gui.Views.HexViewEventArgs -> unit) =
    this.props.add (PKey.HexView.PositionChanged, handler)

type LabelProps() =
  inherit ViewProps()
  // Properties
  member this.HotKeySpecifier (value: System.Text.Rune) =
    this.props.add (PKey.Label.HotKeySpecifier, value)


  member this.Text (value: string) =
    this.props.add (PKey.Label.Text, value)


type LegendAnnotationProps() =
  inherit ViewProps()
type LineProps() =
  inherit ViewProps()
  // Properties
  member this.Length (value: Terminal.Gui.ViewBase.Dim) =
    this.props.add (PKey.Line.Length, value)


  member this.Orientation (value: Terminal.Gui.ViewBase.Orientation) =
    this.props.add (PKey.Line.Orientation, value)


  member this.Style (value: Terminal.Gui.Drawing.LineStyle) =
    this.props.add (PKey.Line.Style, value)


  // Events
  member this.OrientationChanged (handler: EventArgs<Terminal.Gui.ViewBase.Orientation> -> unit) =
    this.props.add (PKey.Line.OrientationChanged, handler)

  member this.OrientationChanging (handler: CancelEventArgs<Terminal.Gui.ViewBase.Orientation> -> unit) =
    this.props.add (PKey.Line.OrientationChanging, handler)

type LinearRangeProps<'T>() =
  inherit ViewProps()
  // Properties
  member this.AllowEmpty (value: bool) =
    this.props.add (PKey.LinearRange<'T>.AllowEmpty, value)


  member this.FocusedOption (value: int) =
    this.props.add (PKey.LinearRange<'T>.FocusedOption, value)


  member this.LegendsOrientation (value: Terminal.Gui.ViewBase.Orientation) =
    this.props.add (PKey.LinearRange<'T>.LegendsOrientation, value)


  member this.MinimumInnerSpacing (value: int) =
    this.props.add (PKey.LinearRange<'T>.MinimumInnerSpacing, value)


  member this.Options (value: List<LinearRangeOption<'T>>) =
    this.props.add (PKey.LinearRange<'T>.Options, value)


  member this.Orientation (value: Terminal.Gui.ViewBase.Orientation) =
    this.props.add (PKey.LinearRange<'T>.Orientation, value)


  member this.RangeAllowSingle (value: bool) =
    this.props.add (PKey.LinearRange<'T>.RangeAllowSingle, value)


  member this.ShowEndSpacing (value: bool) =
    this.props.add (PKey.LinearRange<'T>.ShowEndSpacing, value)


  member this.ShowLegends (value: bool) =
    this.props.add (PKey.LinearRange<'T>.ShowLegends, value)


  member this.Style (value: Terminal.Gui.Views.LinearRangeStyle) =
    this.props.add (PKey.LinearRange<'T>.Style, value)


  member this.Text (value: string) =
    this.props.add (PKey.LinearRange<'T>.Text, value)


  member this.Type (value: Terminal.Gui.Views.LinearRangeType) =
    this.props.add (PKey.LinearRange<'T>.Type, value)


  member this.UseMinimumSize (value: bool) =
    this.props.add (PKey.LinearRange<'T>.UseMinimumSize, value)


  // Events
  member this.LegendsOrientationChanged (handler: ValueChangedEventArgs<Terminal.Gui.ViewBase.Orientation> -> unit) =
    this.props.add (PKey.LinearRange<'T>.LegendsOrientationChanged, handler)

  member this.LegendsOrientationChanging (handler: ValueChangingEventArgs<Terminal.Gui.ViewBase.Orientation> -> unit) =
    this.props.add (PKey.LinearRange<'T>.LegendsOrientationChanging, handler)

  member this.MinimumInnerSpacingChanged (handler: ValueChangedEventArgs<int> -> unit) =
    this.props.add (PKey.LinearRange<'T>.MinimumInnerSpacingChanged, handler)

  member this.MinimumInnerSpacingChanging (handler: ValueChangingEventArgs<int> -> unit) =
    this.props.add (PKey.LinearRange<'T>.MinimumInnerSpacingChanging, handler)

  member this.OptionFocused (handler: LinearRangeEventArgs<'T> -> unit) =
    this.props.add (PKey.LinearRange<'T>.OptionFocused, handler)

  member this.OptionsChanged (handler: LinearRangeEventArgs<'T> -> unit) =
    this.props.add (PKey.LinearRange<'T>.OptionsChanged, handler)

  member this.OrientationChanged (handler: EventArgs<Terminal.Gui.ViewBase.Orientation> -> unit) =
    this.props.add (PKey.LinearRange<'T>.OrientationChanged, handler)

  member this.OrientationChanging (handler: CancelEventArgs<Terminal.Gui.ViewBase.Orientation> -> unit) =
    this.props.add (PKey.LinearRange<'T>.OrientationChanging, handler)

  member this.ShowEndSpacingChanged (handler: ValueChangedEventArgs<bool> -> unit) =
    this.props.add (PKey.LinearRange<'T>.ShowEndSpacingChanged, handler)

  member this.ShowEndSpacingChanging (handler: ValueChangingEventArgs<bool> -> unit) =
    this.props.add (PKey.LinearRange<'T>.ShowEndSpacingChanging, handler)

  member this.ShowLegendsChanged (handler: ValueChangedEventArgs<bool> -> unit) =
    this.props.add (PKey.LinearRange<'T>.ShowLegendsChanged, handler)

  member this.ShowLegendsChanging (handler: ValueChangingEventArgs<bool> -> unit) =
    this.props.add (PKey.LinearRange<'T>.ShowLegendsChanging, handler)

  member this.TypeChanged (handler: ValueChangedEventArgs<Terminal.Gui.Views.LinearRangeType> -> unit) =
    this.props.add (PKey.LinearRange<'T>.TypeChanged, handler)

  member this.TypeChanging (handler: ValueChangingEventArgs<Terminal.Gui.Views.LinearRangeType> -> unit) =
    this.props.add (PKey.LinearRange<'T>.TypeChanging, handler)

  member this.UseMinimumSizeChanged (handler: ValueChangedEventArgs<bool> -> unit) =
    this.props.add (PKey.LinearRange<'T>.UseMinimumSizeChanged, handler)

  member this.UseMinimumSizeChanging (handler: ValueChangingEventArgs<bool> -> unit) =
    this.props.add (PKey.LinearRange<'T>.UseMinimumSizeChanging, handler)

type LinearRangeProps() =
  inherit LinearRangeProps<System.Object>()
type ListViewProps() =
  inherit ViewProps()
  // Properties
  member this.MarkMultiple (value: bool) =
    this.props.add (PKey.ListView.MarkMultiple, value)


  member this.SelectedItem (value: Nullable<int>) =
    this.props.add (PKey.ListView.SelectedItem, value)


  member this.ShowMarks (value: bool) =
    this.props.add (PKey.ListView.ShowMarks, value)


  member this.Source (value: Terminal.Gui.Views.IListDataSource) =
    this.props.add (PKey.ListView.Source, value)


  member this.TopItem (value: int) =
    this.props.add (PKey.ListView.TopItem, value)


  member this.Value (value: Nullable<int>) =
    this.props.add (PKey.ListView.Value, value)


  // Events
  member this.CollectionChanged (handler: System.Collections.Specialized.NotifyCollectionChangedEventArgs -> unit) =
    this.props.add (PKey.ListView.CollectionChanged, handler)

  member this.RowRender (handler: Terminal.Gui.Views.ListViewRowEventArgs -> unit) =
    this.props.add (PKey.ListView.RowRender, handler)

  member this.SourceChanged (handler: System.EventArgs -> unit) =
    this.props.add (PKey.ListView.SourceChanged, handler)

  member this.ValueChanged (handler: ValueChangedEventArgs<Nullable<int>> -> unit) =
    this.props.add (PKey.ListView.ValueChanged, handler)

  member this.ValueChanging (handler: ValueChangingEventArgs<Nullable<int>> -> unit) =
    this.props.add (PKey.ListView.ValueChanging, handler)

type MarginProps() =
  inherit AdornmentProps()
  // Properties
  member this.ShadowSize (value: System.Drawing.Size) =
    this.props.add (PKey.Margin.ShadowSize, value)


  member this.ShadowStyle (value: Terminal.Gui.ViewBase.ShadowStyle) =
    this.props.add (PKey.Margin.ShadowStyle, value)


type MenuProps() =
  inherit BarProps()
  // Properties
  member this.SelectedMenuItem (value: Terminal.Gui.Views.MenuItem) =
    this.props.add (PKey.Menu.SelectedMenuItem, value)

  member this.SelectedMenuItem(value: IMenuItemTerminalElement) =
    this.props.add (PKey.Menu.SelectedMenuItem_element, value)

  member this.SuperMenuItem (value: Terminal.Gui.Views.MenuItem) =
    this.props.add (PKey.Menu.SuperMenuItem, value)

  member this.SuperMenuItem(value: IMenuItemTerminalElement) =
    this.props.add (PKey.Menu.SuperMenuItem_element, value)

  // Events
  member this.SelectedMenuItemChanged (handler: Terminal.Gui.Views.MenuItem -> unit) =
    this.props.add (PKey.Menu.SelectedMenuItemChanged, handler)

type MenuBarProps() =
  inherit MenuProps()
  // Properties
  member this.Key (value: Terminal.Gui.Input.Key) =
    this.props.add (PKey.MenuBar.Key, value)


  // Events
  member this.KeyChanged (handler: Terminal.Gui.Input.KeyChangedEventArgs -> unit) =
    this.props.add (PKey.MenuBar.KeyChanged, handler)

type NumericUpDownProps<'T>() =
  inherit ViewProps()
  // Properties
  member this.Format (value: string) =
    this.props.add (PKey.NumericUpDown<'T>.Format, value)


  member this.Increment (value: 'T) =
    this.props.add (PKey.NumericUpDown<'T>.Increment, value)


  member this.Value (value: 'T) =
    this.props.add (PKey.NumericUpDown<'T>.Value, value)


  // Events
  member this.FormatChanged (handler: EventArgs<string> -> unit) =
    this.props.add (PKey.NumericUpDown<'T>.FormatChanged, handler)

  member this.IncrementChanged (handler: EventArgs<'T> -> unit) =
    this.props.add (PKey.NumericUpDown<'T>.IncrementChanged, handler)

  member this.ValueChanged (handler: ValueChangedEventArgs<'T> -> unit) =
    this.props.add (PKey.NumericUpDown<'T>.ValueChanged, handler)

  member this.ValueChanging (handler: ValueChangingEventArgs<'T> -> unit) =
    this.props.add (PKey.NumericUpDown<'T>.ValueChanging, handler)

type NumericUpDownProps() =
  inherit NumericUpDownProps<int>()
type PaddingProps() =
  inherit AdornmentProps()
type PopoverBaseImplProps() =
  inherit ViewProps()
  // Properties
  member this.Current (value: Terminal.Gui.App.IRunnable) =
    this.props.add (PKey.PopoverBaseImpl.Current, value)


type PopoverMenuProps() =
  inherit PopoverBaseImplProps()
  // Properties
  member this.Key (value: Terminal.Gui.Input.Key) =
    this.props.add (PKey.PopoverMenu.Key, value)


  member this.MouseFlags (value: Terminal.Gui.Input.MouseFlags) =
    this.props.add (PKey.PopoverMenu.MouseFlags, value)


  member this.Root (value: Terminal.Gui.Views.Menu) =
    this.props.add (PKey.PopoverMenu.Root, value)

  member this.Root(value: IMenuTerminalElement) =
    this.props.add (PKey.PopoverMenu.Root_element, value)

  // Events
  member this.KeyChanged (handler: Terminal.Gui.Input.KeyChangedEventArgs -> unit) =
    this.props.add (PKey.PopoverMenu.KeyChanged, handler)

type ProgressBarProps() =
  inherit ViewProps()
  // Properties
  member this.BidirectionalMarquee (value: bool) =
    this.props.add (PKey.ProgressBar.BidirectionalMarquee, value)


  member this.Fraction (value: System.Single) =
    this.props.add (PKey.ProgressBar.Fraction, value)


  member this.ProgressBarFormat (value: Terminal.Gui.Views.ProgressBarFormat) =
    this.props.add (PKey.ProgressBar.ProgressBarFormat, value)


  member this.ProgressBarStyle (value: Terminal.Gui.Views.ProgressBarStyle) =
    this.props.add (PKey.ProgressBar.ProgressBarStyle, value)


  member this.SegmentCharacter (value: System.Text.Rune) =
    this.props.add (PKey.ProgressBar.SegmentCharacter, value)


  member this.Text (value: string) =
    this.props.add (PKey.ProgressBar.Text, value)


type RunnableProps() =
  inherit ViewProps()
  // Properties
  member this.Result (value: System.Object) =
    this.props.add (PKey.Runnable.Result, value)


  member this.StopRequested (value: bool) =
    this.props.add (PKey.Runnable.StopRequested, value)


  // Events
  member this.IsModalChanged (handler: EventArgs<bool> -> unit) =
    this.props.add (PKey.Runnable.IsModalChanged, handler)

  member this.IsRunningChanged (handler: EventArgs<bool> -> unit) =
    this.props.add (PKey.Runnable.IsRunningChanged, handler)

  member this.IsRunningChanging (handler: CancelEventArgs<bool> -> unit) =
    this.props.add (PKey.Runnable.IsRunningChanging, handler)

type RunnableProps<'TResult>() =
  inherit RunnableProps()
  // Properties
  member this.Result (value: 'TResult) =
    this.props.add (PKey.Runnable'<'TResult>.Result, value)


type DialogProps<'TResult>() =
  inherit RunnableProps<'TResult>()
  // Properties
  member this.ButtonAlignment (value: Terminal.Gui.ViewBase.Alignment) =
    this.props.add (PKey.Dialog<'TResult>.ButtonAlignment, value)


  member this.ButtonAlignmentModes (value: Terminal.Gui.ViewBase.AlignmentModes) =
    this.props.add (PKey.Dialog<'TResult>.ButtonAlignmentModes, value)


  member this.Buttons (value: Terminal.Gui.Views.Button[]) =
    this.props.add (PKey.Dialog<'TResult>.Buttons, value)


type DialogProps() =
  inherit DialogProps<int>()
  // Properties
  member this.Result (value: Nullable<int>) =
    this.props.add (PKey.Dialog'.Result, value)


type PromptProps<'TView, 'TResult when 'TView: (new: unit -> 'TView) and 'TView:> Terminal.Gui.ViewBase.View>() =
  inherit DialogProps<'TResult>()
  // Properties
  member this.ResultExtractor (value: Func<'TView, 'TResult>) =
    this.props.add (PKey.Prompt<'TView, 'TResult>.ResultExtractor, value)


type FileDialogProps() =
  inherit DialogProps()
  // Properties
  member this.AllowedTypes (value: List<Terminal.Gui.Views.IAllowedType>) =
    this.props.add (PKey.FileDialog.AllowedTypes, value)


  member this.AllowsMultipleSelection (value: bool) =
    this.props.add (PKey.FileDialog.AllowsMultipleSelection, value)


  member this.FileOperationsHandler (value: Terminal.Gui.FileServices.IFileOperations) =
    this.props.add (PKey.FileDialog.FileOperationsHandler, value)


  member this.MustExist (value: bool) =
    this.props.add (PKey.FileDialog.MustExist, value)


  member this.OpenMode (value: Terminal.Gui.Views.OpenMode) =
    this.props.add (PKey.FileDialog.OpenMode, value)


  member this.Path (value: string) =
    this.props.add (PKey.FileDialog.Path, value)


  member this.SearchMatcher (value: Terminal.Gui.FileServices.ISearchMatcher) =
    this.props.add (PKey.FileDialog.SearchMatcher, value)


  // Events
  member this.FilesSelected (handler: Terminal.Gui.Views.FilesSelectedEventArgs -> unit) =
    this.props.add (PKey.FileDialog.FilesSelected, handler)

type OpenDialogProps() =
  inherit FileDialogProps()
  // Properties
  member this.OpenMode (value: Terminal.Gui.Views.OpenMode) =
    this.props.add (PKey.OpenDialog.OpenMode, value)


type SaveDialogProps() =
  inherit FileDialogProps()
type ScrollBarProps() =
  inherit ViewProps()
  // Properties
  member this.AutoShow (value: bool) =
    this.props.add (PKey.ScrollBar.AutoShow, value)


  member this.Increment (value: int) =
    this.props.add (PKey.ScrollBar.Increment, value)


  member this.Orientation (value: Terminal.Gui.ViewBase.Orientation) =
    this.props.add (PKey.ScrollBar.Orientation, value)


  member this.ScrollableContentSize (value: int) =
    this.props.add (PKey.ScrollBar.ScrollableContentSize, value)


  member this.Value (value: int) =
    this.props.add (PKey.ScrollBar.Value, value)


  member this.VisibleContentSize (value: int) =
    this.props.add (PKey.ScrollBar.VisibleContentSize, value)


  // Events
  member this.OrientationChanged (handler: EventArgs<Terminal.Gui.ViewBase.Orientation> -> unit) =
    this.props.add (PKey.ScrollBar.OrientationChanged, handler)

  member this.OrientationChanging (handler: CancelEventArgs<Terminal.Gui.ViewBase.Orientation> -> unit) =
    this.props.add (PKey.ScrollBar.OrientationChanging, handler)

  member this.ScrollableContentSizeChanged (handler: EventArgs<int> -> unit) =
    this.props.add (PKey.ScrollBar.ScrollableContentSizeChanged, handler)

  member this.Scrolled (handler: EventArgs<int> -> unit) =
    this.props.add (PKey.ScrollBar.Scrolled, handler)

  member this.SliderPositionChanged (handler: EventArgs<int> -> unit) =
    this.props.add (PKey.ScrollBar.SliderPositionChanged, handler)

  member this.ValueChanged (handler: ValueChangedEventArgs<int> -> unit) =
    this.props.add (PKey.ScrollBar.ValueChanged, handler)

  member this.ValueChanging (handler: ValueChangingEventArgs<int> -> unit) =
    this.props.add (PKey.ScrollBar.ValueChanging, handler)

type ScrollSliderProps() =
  inherit ViewProps()
  // Properties
  member this.Orientation (value: Terminal.Gui.ViewBase.Orientation) =
    this.props.add (PKey.ScrollSlider.Orientation, value)


  member this.Position (value: int) =
    this.props.add (PKey.ScrollSlider.Position, value)


  member this.Size (value: int) =
    this.props.add (PKey.ScrollSlider.Size, value)


  member this.SliderPadding (value: int) =
    this.props.add (PKey.ScrollSlider.SliderPadding, value)


  member this.VisibleContentSize (value: int) =
    this.props.add (PKey.ScrollSlider.VisibleContentSize, value)


  // Events
  member this.OrientationChanged (handler: EventArgs<Terminal.Gui.ViewBase.Orientation> -> unit) =
    this.props.add (PKey.ScrollSlider.OrientationChanged, handler)

  member this.OrientationChanging (handler: CancelEventArgs<Terminal.Gui.ViewBase.Orientation> -> unit) =
    this.props.add (PKey.ScrollSlider.OrientationChanging, handler)

  member this.PositionChanged (handler: EventArgs<int> -> unit) =
    this.props.add (PKey.ScrollSlider.PositionChanged, handler)

  member this.PositionChanging (handler: CancelEventArgs<int> -> unit) =
    this.props.add (PKey.ScrollSlider.PositionChanging, handler)

  member this.Scrolled (handler: EventArgs<int> -> unit) =
    this.props.add (PKey.ScrollSlider.Scrolled, handler)

type SelectorBaseProps() =
  inherit ViewProps()
  // Properties
  member this.DoubleClickAccepts (value: bool) =
    this.props.add (PKey.SelectorBase.DoubleClickAccepts, value)


  member this.HorizontalSpace (value: int) =
    this.props.add (PKey.SelectorBase.HorizontalSpace, value)


  member this.Labels (value: IReadOnlyList<string>) =
    this.props.add (PKey.SelectorBase.Labels, value)


  member this.Orientation (value: Terminal.Gui.ViewBase.Orientation) =
    this.props.add (PKey.SelectorBase.Orientation, value)


  member this.Styles (value: Terminal.Gui.Views.SelectorStyles) =
    this.props.add (PKey.SelectorBase.Styles, value)


  member this.Value (value: Nullable<int>) =
    this.props.add (PKey.SelectorBase.Value, value)


  member this.Values (value: IReadOnlyList<int>) =
    this.props.add (PKey.SelectorBase.Values, value)


  // Events
  member this.OrientationChanged (handler: EventArgs<Terminal.Gui.ViewBase.Orientation> -> unit) =
    this.props.add (PKey.SelectorBase.OrientationChanged, handler)

  member this.OrientationChanging (handler: CancelEventArgs<Terminal.Gui.ViewBase.Orientation> -> unit) =
    this.props.add (PKey.SelectorBase.OrientationChanging, handler)

  member this.ValueChanged (handler: ValueChangedEventArgs<Nullable<int>> -> unit) =
    this.props.add (PKey.SelectorBase.ValueChanged, handler)

  member this.ValueChanging (handler: ValueChangingEventArgs<Nullable<int>> -> unit) =
    this.props.add (PKey.SelectorBase.ValueChanging, handler)

type FlagSelectorProps() =
  inherit SelectorBaseProps()
  // Properties
  member this.Value (value: Nullable<int>) =
    this.props.add (PKey.FlagSelector.Value, value)


type OptionSelectorProps() =
  inherit SelectorBaseProps()
  // Properties
  member this.Cursor (value: int) =
    this.props.add (PKey.OptionSelector.Cursor, value)


type FlagSelectorProps<'TFlagsEnum when 'TFlagsEnum: struct and 'TFlagsEnum: (new: unit -> 'TFlagsEnum) and 'TFlagsEnum:> System.Enum and 'TFlagsEnum:> System.ValueType>() =
  inherit FlagSelectorProps()
  // Properties
  member this.Value (value: Nullable<'TFlagsEnum>) =
    this.props.add (PKey.FlagSelector'<'TFlagsEnum>.Value, value)


  // Events
  member this.ValueChanged (handler: EventArgs<Nullable<'TFlagsEnum>> -> unit) =
    this.props.add (PKey.FlagSelector'<'TFlagsEnum>.ValueChanged, handler)

type OptionSelectorProps<'TEnum when 'TEnum: struct and 'TEnum: (new: unit -> 'TEnum) and 'TEnum:> System.Enum and 'TEnum:> System.ValueType>() =
  inherit OptionSelectorProps()
  // Properties
  member this.Value (value: Nullable<'TEnum>) =
    this.props.add (PKey.OptionSelector'<'TEnum>.Value, value)


  member this.Values (value: IReadOnlyList<int>) =
    this.props.add (PKey.OptionSelector'<'TEnum>.Values, value)


  // Events
  member this.ValueChanged (handler: EventArgs<Nullable<'TEnum>> -> unit) =
    this.props.add (PKey.OptionSelector'<'TEnum>.ValueChanged, handler)

type ShortcutProps() =
  inherit ViewProps()
  // Properties
  member this.Action (value: System.Action) =
    this.props.add (PKey.Shortcut.Action, value)


  member this.AlignmentModes (value: Terminal.Gui.ViewBase.AlignmentModes) =
    this.props.add (PKey.Shortcut.AlignmentModes, value)


  member this.BindKeyToApplication (value: bool) =
    this.props.add (PKey.Shortcut.BindKeyToApplication, value)


  member this.CommandView (value: Terminal.Gui.ViewBase.View) =
    this.props.add (PKey.Shortcut.CommandView, value)

  member this.CommandView(value: ITerminalElement) =
    this.props.add (PKey.Shortcut.CommandView_element, value)

  member this.ForceFocusColors (value: bool) =
    this.props.add (PKey.Shortcut.ForceFocusColors, value)


  member this.HelpText (value: string) =
    this.props.add (PKey.Shortcut.HelpText, value)


  member this.Key (value: Terminal.Gui.Input.Key) =
    this.props.add (PKey.Shortcut.Key, value)


  member this.MinimumKeyTextSize (value: int) =
    this.props.add (PKey.Shortcut.MinimumKeyTextSize, value)


  member this.Orientation (value: Terminal.Gui.ViewBase.Orientation) =
    this.props.add (PKey.Shortcut.Orientation, value)


  member this.Text (value: string) =
    this.props.add (PKey.Shortcut.Text, value)


  // Events
  member this.OrientationChanged (handler: EventArgs<Terminal.Gui.ViewBase.Orientation> -> unit) =
    this.props.add (PKey.Shortcut.OrientationChanged, handler)

  member this.OrientationChanging (handler: CancelEventArgs<Terminal.Gui.ViewBase.Orientation> -> unit) =
    this.props.add (PKey.Shortcut.OrientationChanging, handler)

type MenuItemProps() =
  inherit ShortcutProps()
  // Properties
  member this.Command (value: Terminal.Gui.Input.Command) =
    this.props.add (PKey.MenuItem.Command, value)


  member this.SubMenu (value: Terminal.Gui.Views.Menu) =
    this.props.add (PKey.MenuItem.SubMenu, value)

  member this.SubMenu(value: IMenuTerminalElement) =
    this.props.add (PKey.MenuItem.SubMenu_element, value)

  member this.TargetView (value: Terminal.Gui.ViewBase.View) =
    this.props.add (PKey.MenuItem.TargetView, value)

  member this.TargetView(value: ITerminalElement) =
    this.props.add (PKey.MenuItem.TargetView_element, value)

type MenuBarItemProps() =
  inherit MenuItemProps()
  // Properties
  member this.PopoverMenu (value: Terminal.Gui.Views.PopoverMenu) =
    this.props.add (PKey.MenuBarItem.PopoverMenu, value)

  member this.PopoverMenu(value: IPopoverMenuTerminalElement) =
    this.props.add (PKey.MenuBarItem.PopoverMenu_element, value)

  member this.PopoverMenuOpen (value: bool) =
    this.props.add (PKey.MenuBarItem.PopoverMenuOpen, value)


  member this.SubMenu (value: Terminal.Gui.Views.Menu) =
    this.props.add (PKey.MenuBarItem.SubMenu, value)

  member this.SubMenu(value: IMenuTerminalElement) =
    this.props.add (PKey.MenuBarItem.SubMenu_element, value)

  // Events
  member this.PopoverMenuOpenChanged (handler: EventArgs<bool> -> unit) =
    this.props.add (PKey.MenuBarItem.PopoverMenuOpenChanged, handler)

type SpinnerViewProps() =
  inherit ViewProps()
  // Properties
  member this.AutoSpin (value: bool) =
    this.props.add (PKey.SpinnerView.AutoSpin, value)


  member this.Sequence (value: System.String[]) =
    this.props.add (PKey.SpinnerView.Sequence, value)


  member this.SpinBounce (value: bool) =
    this.props.add (PKey.SpinnerView.SpinBounce, value)


  member this.SpinDelay (value: int) =
    this.props.add (PKey.SpinnerView.SpinDelay, value)


  member this.SpinReverse (value: bool) =
    this.props.add (PKey.SpinnerView.SpinReverse, value)


  member this.Style (value: Terminal.Gui.Views.SpinnerStyle) =
    this.props.add (PKey.SpinnerView.Style, value)


type StatusBarProps() =
  inherit BarProps()
type TabProps() =
  inherit ViewProps()
  // Properties
  member this.DisplayText (value: string) =
    this.props.add (PKey.Tab.DisplayText, value)


  member this.View (value: Terminal.Gui.ViewBase.View) =
    this.props.add (PKey.Tab.View, value)

  member this.View(value: ITerminalElement) =
    this.props.add (PKey.Tab.View_element, value)

type TabViewProps() =
  inherit ViewProps()
  // Properties
  member this.MaxTabTextWidth (value: System.UInt32) =
    this.props.add (PKey.TabView.MaxTabTextWidth, value)


  member this.SelectedTab (value: Terminal.Gui.Views.Tab) =
    this.props.add (PKey.TabView.SelectedTab, value)

  member this.SelectedTab(value: ITabTerminalElement) =
    this.props.add (PKey.TabView.SelectedTab_element, value)

  member this.Style (value: Terminal.Gui.Views.TabStyle) =
    this.props.add (PKey.TabView.Style, value)


  member this.TabScrollOffset (value: int) =
    this.props.add (PKey.TabView.TabScrollOffset, value)


  // Events
  member this.SelectedTabChanged (handler: Terminal.Gui.Views.TabChangedEventArgs -> unit) =
    this.props.add (PKey.TabView.SelectedTabChanged, handler)

  member this.TabClicked (handler: Terminal.Gui.Views.TabMouseEventArgs -> unit) =
    this.props.add (PKey.TabView.TabClicked, handler)

type TableViewProps() =
  inherit ViewProps()
  // Properties
  member this.CellActivationKey (value: Terminal.Gui.Drivers.KeyCode) =
    this.props.add (PKey.TableView.CellActivationKey, value)


  member this.CollectionNavigator (value: Terminal.Gui.Views.ICollectionNavigator) =
    this.props.add (PKey.TableView.CollectionNavigator, value)


  member this.ColumnOffset (value: int) =
    this.props.add (PKey.TableView.ColumnOffset, value)


  member this.FullRowSelect (value: bool) =
    this.props.add (PKey.TableView.FullRowSelect, value)


  member this.MaxCellWidth (value: int) =
    this.props.add (PKey.TableView.MaxCellWidth, value)


  member this.MinCellWidth (value: int) =
    this.props.add (PKey.TableView.MinCellWidth, value)


  member this.MultiSelect (value: bool) =
    this.props.add (PKey.TableView.MultiSelect, value)


  member this.NullSymbol (value: string) =
    this.props.add (PKey.TableView.NullSymbol, value)


  member this.RowOffset (value: int) =
    this.props.add (PKey.TableView.RowOffset, value)


  member this.SelectedColumn (value: int) =
    this.props.add (PKey.TableView.SelectedColumn, value)


  member this.SelectedRow (value: int) =
    this.props.add (PKey.TableView.SelectedRow, value)


  member this.SeparatorSymbol (value: System.Char) =
    this.props.add (PKey.TableView.SeparatorSymbol, value)


  member this.Style (value: Terminal.Gui.Views.TableStyle) =
    this.props.add (PKey.TableView.Style, value)


  member this.Table (value: Terminal.Gui.Views.ITableSource) =
    this.props.add (PKey.TableView.Table, value)


  // Events
  member this.CellActivated (handler: Terminal.Gui.Views.CellActivatedEventArgs -> unit) =
    this.props.add (PKey.TableView.CellActivated, handler)

  member this.CellToggled (handler: Terminal.Gui.Views.CellToggledEventArgs -> unit) =
    this.props.add (PKey.TableView.CellToggled, handler)

  member this.SelectedCellChanged (handler: Terminal.Gui.Views.SelectedCellChangedEventArgs -> unit) =
    this.props.add (PKey.TableView.SelectedCellChanged, handler)

type TextFieldProps() =
  inherit ViewProps()
  // Properties
  member this.Autocomplete (value: Terminal.Gui.Views.IAutocomplete) =
    this.props.add (PKey.TextField.Autocomplete, value)


  member this.InsertionPoint (value: int) =
    this.props.add (PKey.TextField.InsertionPoint, value)


  member this.ReadOnly (value: bool) =
    this.props.add (PKey.TextField.ReadOnly, value)


  member this.Secret (value: bool) =
    this.props.add (PKey.TextField.Secret, value)


  member this.SelectWordOnlyOnDoubleClick (value: bool) =
    this.props.add (PKey.TextField.SelectWordOnlyOnDoubleClick, value)


  member this.SelectedStart (value: int) =
    this.props.add (PKey.TextField.SelectedStart, value)


  member this.Text (value: string) =
    this.props.add (PKey.TextField.Text, value)


  member this.UseSameRuneTypeForWords (value: bool) =
    this.props.add (PKey.TextField.UseSameRuneTypeForWords, value)


  member this.Used (value: bool) =
    this.props.add (PKey.TextField.Used, value)


  member this.Value (value: string) =
    this.props.add (PKey.TextField.Value, value)


  // Events
  member this.TextChanging (handler: ResultEventArgs<string> -> unit) =
    this.props.add (PKey.TextField.TextChanging, handler)

  member this.ValueChanged (handler: ValueChangedEventArgs<string> -> unit) =
    this.props.add (PKey.TextField.ValueChanged, handler)

  member this.ValueChanging (handler: ValueChangingEventArgs<string> -> unit) =
    this.props.add (PKey.TextField.ValueChanging, handler)

type DateFieldProps() =
  inherit TextFieldProps()
  // Properties
  member this.Culture (value: System.Globalization.CultureInfo) =
    this.props.add (PKey.DateField.Culture, value)


  member this.InsertionPoint (value: int) =
    this.props.add (PKey.DateField.InsertionPoint, value)


  member this.Value (value: Nullable<System.DateTime>) =
    this.props.add (PKey.DateField.Value, value)


  // Events
  member this.ValueChanged (handler: ValueChangedEventArgs<Nullable<System.DateTime>> -> unit) =
    this.props.add (PKey.DateField.ValueChanged, handler)

  member this.ValueChanging (handler: ValueChangingEventArgs<Nullable<System.DateTime>> -> unit) =
    this.props.add (PKey.DateField.ValueChanging, handler)

type TextValidateFieldProps() =
  inherit ViewProps()
  // Properties
  member this.Provider (value: Terminal.Gui.Views.ITextValidateProvider) =
    this.props.add (PKey.TextValidateField.Provider, value)


  member this.Text (value: string) =
    this.props.add (PKey.TextValidateField.Text, value)


type TextViewProps() =
  inherit ViewProps()
  // Properties
  member this.EnterKeyAddsLine (value: bool) =
    this.props.add (PKey.TextView.EnterKeyAddsLine, value)


  member this.InheritsPreviousAttribute (value: bool) =
    this.props.add (PKey.TextView.InheritsPreviousAttribute, value)


  member this.InsertionPoint (value: System.Drawing.Point) =
    this.props.add (PKey.TextView.InsertionPoint, value)


  member this.IsDirty (value: bool) =
    this.props.add (PKey.TextView.IsDirty, value)


  member this.IsSelecting (value: bool) =
    this.props.add (PKey.TextView.IsSelecting, value)


  member this.LeftColumn (value: int) =
    this.props.add (PKey.TextView.LeftColumn, value)


  member this.Multiline (value: bool) =
    this.props.add (PKey.TextView.Multiline, value)


  member this.ReadOnly (value: bool) =
    this.props.add (PKey.TextView.ReadOnly, value)


  member this.ScrollBars (value: bool) =
    this.props.add (PKey.TextView.ScrollBars, value)


  member this.SelectWordOnlyOnDoubleClick (value: bool) =
    this.props.add (PKey.TextView.SelectWordOnlyOnDoubleClick, value)


  member this.SelectionStartColumn (value: int) =
    this.props.add (PKey.TextView.SelectionStartColumn, value)


  member this.SelectionStartRow (value: int) =
    this.props.add (PKey.TextView.SelectionStartRow, value)


  member this.TabKeyAddsTab (value: bool) =
    this.props.add (PKey.TextView.TabKeyAddsTab, value)


  member this.TabWidth (value: int) =
    this.props.add (PKey.TextView.TabWidth, value)


  member this.Text (value: string) =
    this.props.add (PKey.TextView.Text, value)


  member this.TopRow (value: int) =
    this.props.add (PKey.TextView.TopRow, value)


  member this.UseSameRuneTypeForWords (value: bool) =
    this.props.add (PKey.TextView.UseSameRuneTypeForWords, value)


  member this.Used (value: bool) =
    this.props.add (PKey.TextView.Used, value)


  member this.WordWrap (value: bool) =
    this.props.add (PKey.TextView.WordWrap, value)


  // Events
  member this.ContentsChanged (handler: Terminal.Gui.Views.ContentsChangedEventArgs -> unit) =
    this.props.add (PKey.TextView.ContentsChanged, handler)

  member this.DrawNormalColor (handler: Terminal.Gui.Drawing.CellEventArgs -> unit) =
    this.props.add (PKey.TextView.DrawNormalColor, handler)

  member this.DrawReadOnlyColor (handler: Terminal.Gui.Drawing.CellEventArgs -> unit) =
    this.props.add (PKey.TextView.DrawReadOnlyColor, handler)

  member this.DrawSelectionColor (handler: Terminal.Gui.Drawing.CellEventArgs -> unit) =
    this.props.add (PKey.TextView.DrawSelectionColor, handler)

  member this.DrawUsedColor (handler: Terminal.Gui.Drawing.CellEventArgs -> unit) =
    this.props.add (PKey.TextView.DrawUsedColor, handler)

  member this.UnwrappedCursorPosition (handler: System.Drawing.Point -> unit) =
    this.props.add (PKey.TextView.UnwrappedCursorPosition, handler)

type TimeFieldProps() =
  inherit TextFieldProps()
  // Properties
  member this.InsertionPoint (value: int) =
    this.props.add (PKey.TimeField.InsertionPoint, value)


  member this.IsShortFormat (value: bool) =
    this.props.add (PKey.TimeField.IsShortFormat, value)


  member this.Value (value: System.TimeSpan) =
    this.props.add (PKey.TimeField.Value, value)


  // Events
  member this.ValueChanged (handler: ValueChangedEventArgs<System.TimeSpan> -> unit) =
    this.props.add (PKey.TimeField.ValueChanged, handler)

  member this.ValueChanging (handler: ValueChangingEventArgs<System.TimeSpan> -> unit) =
    this.props.add (PKey.TimeField.ValueChanging, handler)

type TreeViewProps<'T when 'T: not struct>() =
  inherit ViewProps()
  // Properties
  member this.AllowLetterBasedNavigation (value: bool) =
    this.props.add (PKey.TreeView<'T>.AllowLetterBasedNavigation, value)


  member this.AspectGetter (value: AspectGetterDelegate<'T>) =
    this.props.add (PKey.TreeView<'T>.AspectGetter, value)


  member this.ColorGetter (value: Func<'T, Terminal.Gui.Drawing.Scheme>) =
    this.props.add (PKey.TreeView<'T>.ColorGetter, value)


  member this.MaxDepth (value: int) =
    this.props.add (PKey.TreeView<'T>.MaxDepth, value)


  member this.MultiSelect (value: bool) =
    this.props.add (PKey.TreeView<'T>.MultiSelect, value)


  member this.ObjectActivationButton (value: Nullable<Terminal.Gui.Input.MouseFlags>) =
    this.props.add (PKey.TreeView<'T>.ObjectActivationButton, value)


  member this.ObjectActivationKey (value: Terminal.Gui.Drivers.KeyCode) =
    this.props.add (PKey.TreeView<'T>.ObjectActivationKey, value)


  member this.ScrollOffsetHorizontal (value: int) =
    this.props.add (PKey.TreeView<'T>.ScrollOffsetHorizontal, value)


  member this.ScrollOffsetVertical (value: int) =
    this.props.add (PKey.TreeView<'T>.ScrollOffsetVertical, value)


  member this.SelectedObject (value: 'T) =
    this.props.add (PKey.TreeView<'T>.SelectedObject, value)


  member this.Style (value: Terminal.Gui.Views.TreeStyle) =
    this.props.add (PKey.TreeView<'T>.Style, value)


  member this.TreeBuilder (value: ITreeBuilder<'T>) =
    this.props.add (PKey.TreeView<'T>.TreeBuilder, value)


  // Events
  member this.DrawLine (handler: DrawTreeViewLineEventArgs<'T> -> unit) =
    this.props.add (PKey.TreeView<'T>.DrawLine, handler)

  member this.ObjectActivated (handler: ObjectActivatedEventArgs<'T> -> unit) =
    this.props.add (PKey.TreeView<'T>.ObjectActivated, handler)

  member this.SelectionChanged (handler: SelectionChangedEventArgs<'T> -> unit) =
    this.props.add (PKey.TreeView<'T>.SelectionChanged, handler)

type TreeViewProps() =
  inherit TreeViewProps<Terminal.Gui.Views.ITreeNode>()
type WindowProps() =
  inherit RunnableProps()
type WizardProps() =
  inherit DialogProps()
  // Properties
  member this.CurrentStep (value: Terminal.Gui.Views.WizardStep) =
    this.props.add (PKey.Wizard.CurrentStep, value)

  member this.CurrentStep(value: IWizardStepTerminalElement) =
    this.props.add (PKey.Wizard.CurrentStep_element, value)

  // Events
  member this.MovingBack (handler: System.ComponentModel.CancelEventArgs -> unit) =
    this.props.add (PKey.Wizard.MovingBack, handler)

  member this.MovingNext (handler: System.ComponentModel.CancelEventArgs -> unit) =
    this.props.add (PKey.Wizard.MovingNext, handler)

  member this.StepChanged (handler: ValueChangedEventArgs<Terminal.Gui.Views.WizardStep> -> unit) =
    this.props.add (PKey.Wizard.StepChanged, handler)

  member this.StepChanging (handler: ValueChangingEventArgs<Terminal.Gui.Views.WizardStep> -> unit) =
    this.props.add (PKey.Wizard.StepChanging, handler)

type WizardStepProps() =
  inherit ViewProps()
  // Properties
  member this.BackButtonText (value: string) =
    this.props.add (PKey.WizardStep.BackButtonText, value)


  member this.HelpText (value: string) =
    this.props.add (PKey.WizardStep.HelpText, value)


  member this.NextButtonText (value: string) =
    this.props.add (PKey.WizardStep.NextButtonText, value)

