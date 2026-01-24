namespace Terminal.Gui.Elmish

open System
open System.Collections.Generic
open System.Collections.ObjectModel
open System.Text
open System.Drawing
open System.ComponentModel
open System.IO
open System.Collections.Specialized
open System.Globalization
open Terminal.Gui.App
open Terminal.Gui.Drawing
open Terminal.Gui.Drivers
open Terminal.Gui.Elmish
open Terminal.Gui

open Terminal.Gui.FileServices
open Terminal.Gui.Input

open Terminal.Gui.Text

open Terminal.Gui.ViewBase

open Terminal.Gui.Views

type ViewProps() =
  member val internal props = Props()

  member this.Children(children: ITerminalElement list) =
    this.props.add (
      PKey.View.children,
      List<_>(
        children
        |> List.map (fun x -> x :?> IInternalTerminalElement)
      )
    )
  
  // Delayed Positions
  member this.X (value: TPos) =
    this.props.add (PKey.View.X_delayedPos, value)

  member this.Y (value: TPos) =
    this.props.add (PKey.View.Y_delayedPos, value)

  // Properties
  member this.Arrangement (value: ViewArrangement) =
    this.props.add (PKey.View.Arrangement, value)


  member this.BorderStyle (value: LineStyle) =
    this.props.add (PKey.View.BorderStyle, value)


  member this.CanFocus (value: bool) =
    this.props.add (PKey.View.CanFocus, value)


  member this.ContentSizeTracksViewport (value: bool) =
    this.props.add (PKey.View.ContentSizeTracksViewport, value)


  member this.CursorVisibility (value: CursorVisibility) =
    this.props.add (PKey.View.CursorVisibility, value)


  member this.Data (value: Object) =
    this.props.add (PKey.View.Data, value)


  member this.Enabled (value: bool) =
    this.props.add (PKey.View.Enabled, value)


  member this.Frame (value: Rectangle) =
    this.props.add (PKey.View.Frame, value)


  member this.HasFocus (value: bool) =
    this.props.add (PKey.View.HasFocus, value)


  member this.Height (value: Dim) =
    this.props.add (PKey.View.Height, value)


  member this.HighlightStates (value: MouseState) =
    this.props.add (PKey.View.HighlightStates, value)


  member this.HotKey (value: Key) =
    this.props.add (PKey.View.HotKey, value)


  member this.HotKeySpecifier (value: Rune) =
    this.props.add (PKey.View.HotKeySpecifier, value)


  member this.Id (value: string) =
    this.props.add (PKey.View.Id, value)


  member this.IsInitialized (value: bool) =
    this.props.add (PKey.View.IsInitialized, value)


  member this.MouseHeldDown (value: IMouseHeldDown) =
    this.props.add (PKey.View.MouseHeldDown, value)


  member this.PreserveTrailingSpaces (value: bool) =
    this.props.add (PKey.View.PreserveTrailingSpaces, value)


  member this.SchemeName (value: string) =
    this.props.add (PKey.View.SchemeName, value)


  member this.ShadowStyle (value: ShadowStyle) =
    this.props.add (PKey.View.ShadowStyle, value)


  member this.SuperViewRendersLineCanvas (value: bool) =
    this.props.add (PKey.View.SuperViewRendersLineCanvas, value)


  member this.TabStop (value: Nullable<TabBehavior>) =
    this.props.add (PKey.View.TabStop, value)


  member this.Text (value: string) =
    this.props.add (PKey.View.Text, value)


  member this.TextAlignment (value: Alignment) =
    this.props.add (PKey.View.TextAlignment, value)


  member this.TextDirection (value: TextDirection) =
    this.props.add (PKey.View.TextDirection, value)


  member this.Title (value: string) =
    this.props.add (PKey.View.Title, value)


  member this.ValidatePosDim (value: bool) =
    this.props.add (PKey.View.ValidatePosDim, value)


  member this.VerticalTextAlignment (value: Alignment) =
    this.props.add (PKey.View.VerticalTextAlignment, value)


  member this.Viewport (value: Rectangle) =
    this.props.add (PKey.View.Viewport, value)


  member this.ViewportSettings (value: ViewportSettingsFlags) =
    this.props.add (PKey.View.ViewportSettings, value)


  member this.Visible (value: bool) =
    this.props.add (PKey.View.Visible, value)


  member this.WantContinuousButtonPressed (value: bool) =
    this.props.add (PKey.View.WantContinuousButtonPressed, value)


  member this.WantMousePositionReports (value: bool) =
    this.props.add (PKey.View.WantMousePositionReports, value)


  member this.Width (value: Dim) =
    this.props.add (PKey.View.Width, value)


  member this.X (value: Pos) =
    this.props.add (PKey.View.X, value)


  member this.Y (value: Pos) =
    this.props.add (PKey.View.Y, value)


  // Events
  member this.Accepted (handler: CommandEventArgs -> unit) =
    this.props.add (PKey.View.Accepted, handler)

  member this.Accepting (handler: CommandEventArgs -> unit) =
    this.props.add (PKey.View.Accepting, handler)

  member this.Activating (handler: CommandEventArgs -> unit) =
    this.props.add (PKey.View.Activating, handler)

  member this.AdvancingFocus (handler: AdvanceFocusEventArgs -> unit) =
    this.props.add (PKey.View.AdvancingFocus, handler)

  member this.BorderStyleChanged (handler: EventArgs -> unit) =
    this.props.add (PKey.View.BorderStyleChanged, handler)

  member this.CanFocusChanged (handler: EventArgs -> unit) =
    this.props.add (PKey.View.CanFocusChanged, handler)

  member this.ClearedViewport (handler: DrawEventArgs -> unit) =
    this.props.add (PKey.View.ClearedViewport, handler)

  member this.ClearingViewport (handler: DrawEventArgs -> unit) =
    this.props.add (PKey.View.ClearingViewport, handler)

  member this.CommandNotBound (handler: CommandEventArgs -> unit) =
    this.props.add (PKey.View.CommandNotBound, handler)

  member this.ContentSizeChanged (handler: SizeChangedEventArgs -> unit) =
    this.props.add (PKey.View.ContentSizeChanged, handler)

  member this.Disposing (handler: EventArgs -> unit) =
    this.props.add (PKey.View.Disposing, handler)

  member this.DrawComplete (handler: DrawEventArgs -> unit) =
    this.props.add (PKey.View.DrawComplete, handler)

  member this.DrawingContent (handler: DrawEventArgs -> unit) =
    this.props.add (PKey.View.DrawingContent, handler)

  member this.DrawingSubViews (handler: DrawEventArgs -> unit) =
    this.props.add (PKey.View.DrawingSubViews, handler)

  member this.DrawingText (handler: DrawEventArgs -> unit) =
    this.props.add (PKey.View.DrawingText, handler)

  member this.DrewText (handler: EventArgs -> unit) =
    this.props.add (PKey.View.DrewText, handler)

  member this.EnabledChanged (handler: EventArgs -> unit) =
    this.props.add (PKey.View.EnabledChanged, handler)

  member this.FocusedChanged (handler: HasFocusEventArgs -> unit) =
    this.props.add (PKey.View.FocusedChanged, handler)

  member this.FrameChanged (handler: EventArgs<Rectangle> -> unit) =
    this.props.add (PKey.View.FrameChanged, handler)

  member this.GettingAttributeForRole (handler: VisualRoleEventArgs -> unit) =
    this.props.add (PKey.View.GettingAttributeForRole, handler)

  member this.GettingScheme (handler: ResultEventArgs<Scheme> -> unit) =
    this.props.add (PKey.View.GettingScheme, handler)

  member this.HandlingHotKey (handler: CommandEventArgs -> unit) =
    this.props.add (PKey.View.HandlingHotKey, handler)

  member this.HasFocusChanged (handler: HasFocusEventArgs -> unit) =
    this.props.add (PKey.View.HasFocusChanged, handler)

  member this.HasFocusChanging (handler: HasFocusEventArgs -> unit) =
    this.props.add (PKey.View.HasFocusChanging, handler)

  member this.HeightChanged (handler: ValueChangedEventArgs<Dim> -> unit) =
    this.props.add (PKey.View.HeightChanged, handler)

  member this.HeightChanging (handler: ValueChangingEventArgs<Dim> -> unit) =
    this.props.add (PKey.View.HeightChanging, handler)

  member this.HotKeyChanged (handler: KeyChangedEventArgs -> unit) =
    this.props.add (PKey.View.HotKeyChanged, handler)

  member this.Initialized (handler: EventArgs -> unit) =
    this.props.add (PKey.View.Initialized, handler)

  member this.KeyDown (handler: Key -> unit) =
    this.props.add (PKey.View.KeyDown, handler)

  member this.KeyDownNotHandled (handler: Key -> unit) =
    this.props.add (PKey.View.KeyDownNotHandled, handler)

  member this.KeyUp (handler: Key -> unit) =
    this.props.add (PKey.View.KeyUp, handler)

  member this.MouseEnter (handler: CancelEventArgs -> unit) =
    this.props.add (PKey.View.MouseEnter, handler)

  member this.MouseEvent (handler: MouseEventArgs -> unit) =
    this.props.add (PKey.View.MouseEvent, handler)

  member this.MouseLeave (handler: EventArgs -> unit) =
    this.props.add (PKey.View.MouseLeave, handler)

  member this.MouseStateChanged (handler: EventArgs<MouseState> -> unit) =
    this.props.add (PKey.View.MouseStateChanged, handler)

  member this.MouseWheel (handler: MouseEventArgs -> unit) =
    this.props.add (PKey.View.MouseWheel, handler)

  member this.Removed (handler: SuperViewChangedEventArgs -> unit) =
    this.props.add (PKey.View.Removed, handler)

  member this.SchemeChanged (handler: ValueChangedEventArgs<Scheme> -> unit) =
    this.props.add (PKey.View.SchemeChanged, handler)

  member this.SchemeChanging (handler: ValueChangingEventArgs<Scheme> -> unit) =
    this.props.add (PKey.View.SchemeChanging, handler)

  member this.SchemeNameChanged (handler: ValueChangedEventArgs<string> -> unit) =
    this.props.add (PKey.View.SchemeNameChanged, handler)

  member this.SchemeNameChanging (handler: ValueChangingEventArgs<string> -> unit) =
    this.props.add (PKey.View.SchemeNameChanging, handler)

  member this.SubViewAdded (handler: SuperViewChangedEventArgs -> unit) =
    this.props.add (PKey.View.SubViewAdded, handler)

  member this.SubViewLayout (handler: LayoutEventArgs -> unit) =
    this.props.add (PKey.View.SubViewLayout, handler)

  member this.SubViewRemoved (handler: SuperViewChangedEventArgs -> unit) =
    this.props.add (PKey.View.SubViewRemoved, handler)

  member this.SubViewsLaidOut (handler: LayoutEventArgs -> unit) =
    this.props.add (PKey.View.SubViewsLaidOut, handler)

  member this.SuperViewChanged (handler: SuperViewChangedEventArgs -> unit) =
    this.props.add (PKey.View.SuperViewChanged, handler)

  member this.TextChanged (handler: EventArgs -> unit) =
    this.props.add (PKey.View.TextChanged, handler)

  member this.TitleChanged (handler: EventArgs<string> -> unit) =
    this.props.add (PKey.View.TitleChanged, handler)

  member this.TitleChanging (handler: CancelEventArgs<string> -> unit) =
    this.props.add (PKey.View.TitleChanging, handler)

  member this.ViewportChanged (handler: DrawEventArgs -> unit) =
    this.props.add (PKey.View.ViewportChanged, handler)

  member this.VisibleChanged (handler: EventArgs -> unit) =
    this.props.add (PKey.View.VisibleChanged, handler)

  member this.VisibleChanging (handler: CancelEventArgs<bool> -> unit) =
    this.props.add (PKey.View.VisibleChanging, handler)

  member this.WidthChanged (handler: ValueChangedEventArgs<Dim> -> unit) =
    this.props.add (PKey.View.WidthChanged, handler)

  member this.WidthChanging (handler: ValueChangingEventArgs<Dim> -> unit) =
    this.props.add (PKey.View.WidthChanging, handler)

type AdornmentProps() =
  inherit ViewProps()
  // Properties
  member this.Diagnostics (value: ViewDiagnosticFlags) =
    this.props.add (PKey.Adornment.Diagnostics, value)


  member this.Parent (value: View) =
    this.props.add (PKey.Adornment.Parent, value)

  member this.Parent(value: ITerminalElement) =
    this.props.add (PKey.Adornment.Parent_element, value)

  member this.SuperViewRendersLineCanvas (value: bool) =
    this.props.add (PKey.Adornment.SuperViewRendersLineCanvas, value)


  member this.Thickness (value: Thickness) =
    this.props.add (PKey.Adornment.Thickness, value)


  member this.Viewport (value: Rectangle) =
    this.props.add (PKey.Adornment.Viewport, value)


  // Events
  member this.ThicknessChanged (handler: EventArgs -> unit) =
    this.props.add (PKey.Adornment.ThicknessChanged, handler)

type BarProps() =
  inherit ViewProps()
  // Properties
  member this.AlignmentModes (value: AlignmentModes) =
    this.props.add (PKey.Bar.AlignmentModes, value)


  member this.Orientation (value: Orientation) =
    this.props.add (PKey.Bar.Orientation, value)


  // Events
  member this.OrientationChanged (handler: EventArgs<Orientation> -> unit) =
    this.props.add (PKey.Bar.OrientationChanged, handler)

  member this.OrientationChanging (handler: CancelEventArgs<Orientation> -> unit) =
    this.props.add (PKey.Bar.OrientationChanging, handler)

type BorderProps() =
  inherit ViewProps()
  // Properties
  member this.LineStyle (value: LineStyle) =
    this.props.add (PKey.Border.LineStyle, value)


  member this.Settings (value: BorderSettings) =
    this.props.add (PKey.Border.Settings, value)


type ButtonProps() =
  inherit ViewProps()
  // Properties
  member this.HotKeySpecifier (value: Rune) =
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


  member this.ShowUnicodeCategory (value: Nullable<UnicodeCategory>) =
    this.props.add (PKey.CharMap.ShowUnicodeCategory, value)


  member this.StartCodePoint (value: int) =
    this.props.add (PKey.CharMap.StartCodePoint, value)


  // Events
  member this.SelectedCodePointChanged (handler: EventArgs<int> -> unit) =
    this.props.add (PKey.CharMap.SelectedCodePointChanged, handler)

type CheckBoxProps() =
  inherit ViewProps()
  // Properties
  member this.AllowCheckStateNone (value: bool) =
    this.props.add (PKey.CheckBox.AllowCheckStateNone, value)


  member this.CheckedState (value: CheckState) =
    this.props.add (PKey.CheckBox.CheckedState, value)


  member this.HotKeySpecifier (value: Rune) =
    this.props.add (PKey.CheckBox.HotKeySpecifier, value)


  member this.RadioStyle (value: bool) =
    this.props.add (PKey.CheckBox.RadioStyle, value)


  member this.Text (value: string) =
    this.props.add (PKey.CheckBox.Text, value)


  // Events
  member this.CheckedStateChanged (handler: EventArgs<CheckState> -> unit) =
    this.props.add (PKey.CheckBox.CheckedStateChanged, handler)

  member this.CheckedStateChanging (handler: ResultEventArgs<CheckState> -> unit) =
    this.props.add (PKey.CheckBox.CheckedStateChanging, handler)

type ColorPickerProps() =
  inherit ViewProps()
  // Properties
  member this.SelectedColor (value: Color) =
    this.props.add (PKey.ColorPicker.SelectedColor, value)


  member this.Style (value: ColorPickerStyle) =
    this.props.add (PKey.ColorPicker.Style, value)


  // Events
  member this.ColorChanged (handler: ResultEventArgs<Color> -> unit) =
    this.props.add (PKey.ColorPicker.ColorChanged, handler)

type ColorPicker16Props() =
  inherit ViewProps()
  // Properties
  member this.BoxHeight (value: int) =
    this.props.add (PKey.ColorPicker16.BoxHeight, value)


  member this.BoxWidth (value: int) =
    this.props.add (PKey.ColorPicker16.BoxWidth, value)


  member this.Cursor (value: Point) =
    this.props.add (PKey.ColorPicker16.Cursor, value)


  member this.SelectedColor (value: ColorName16) =
    this.props.add (PKey.ColorPicker16.SelectedColor, value)


  // Events
  member this.ColorChanged (handler: ResultEventArgs<Color> -> unit) =
    this.props.add (PKey.ColorPicker16.ColorChanged, handler)

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


  member this.Source (value: IListDataSource) =
    this.props.add (PKey.ComboBox.Source, value)


  member this.Text (value: string) =
    this.props.add (PKey.ComboBox.Text, value)


  // Events
  member this.Collapsed (handler: EventArgs -> unit) =
    this.props.add (PKey.ComboBox.Collapsed, handler)

  member this.Expanded (handler: EventArgs -> unit) =
    this.props.add (PKey.ComboBox.Expanded, handler)

  member this.OpenSelectedItem (handler: ListViewItemEventArgs -> unit) =
    this.props.add (PKey.ComboBox.OpenSelectedItem, handler)

  member this.SelectedItemChanged (handler: ListViewItemEventArgs -> unit) =
    this.props.add (PKey.ComboBox.SelectedItemChanged, handler)

type DatePickerProps() =
  inherit ViewProps()
  // Properties
  member this.Culture (value: CultureInfo) =
    this.props.add (PKey.DatePicker.Culture, value)


  member this.Date (value: DateTime) =
    this.props.add (PKey.DatePicker.Date, value)


type FrameViewProps() =
  inherit ViewProps()
type GraphViewProps() =
  inherit ViewProps()
  // Properties
  member this.AxisX (value: HorizontalAxis) =
    this.props.add (PKey.GraphView.AxisX, value)


  member this.AxisY (value: VerticalAxis) =
    this.props.add (PKey.GraphView.AxisY, value)


  member this.CellSize (value: PointF) =
    this.props.add (PKey.GraphView.CellSize, value)


  member this.GraphColor (value: Nullable<Attribute>) =
    this.props.add (PKey.GraphView.GraphColor, value)


  member this.MarginBottom (value: UInt32) =
    this.props.add (PKey.GraphView.MarginBottom, value)


  member this.MarginLeft (value: UInt32) =
    this.props.add (PKey.GraphView.MarginLeft, value)


  member this.ScrollOffset (value: PointF) =
    this.props.add (PKey.GraphView.ScrollOffset, value)


type HexViewProps() =
  inherit ViewProps()
  // Properties
  member this.Address (value: Int64) =
    this.props.add (PKey.HexView.Address, value)


  member this.AddressWidth (value: int) =
    this.props.add (PKey.HexView.AddressWidth, value)


  member this.BytesPerLine (value: int) =
    this.props.add (PKey.HexView.BytesPerLine, value)


  member this.ReadOnly (value: bool) =
    this.props.add (PKey.HexView.ReadOnly, value)


  member this.Source (value: Stream) =
    this.props.add (PKey.HexView.Source, value)


  // Events
  member this.Edited (handler: HexViewEditEventArgs -> unit) =
    this.props.add (PKey.HexView.Edited, handler)

  member this.PositionChanged (handler: HexViewEventArgs -> unit) =
    this.props.add (PKey.HexView.PositionChanged, handler)

type LabelProps() =
  inherit ViewProps()
  // Properties
  member this.HotKeySpecifier (value: Rune) =
    this.props.add (PKey.Label.HotKeySpecifier, value)


  member this.Text (value: string) =
    this.props.add (PKey.Label.Text, value)


type LegendAnnotationProps() =
  inherit ViewProps()
type LineProps() =
  inherit ViewProps()
  // Properties
  member this.Length (value: Dim) =
    this.props.add (PKey.Line.Length, value)


  member this.Orientation (value: Orientation) =
    this.props.add (PKey.Line.Orientation, value)


  member this.Style (value: LineStyle) =
    this.props.add (PKey.Line.Style, value)


  // Events
  member this.OrientationChanged (handler: EventArgs<Orientation> -> unit) =
    this.props.add (PKey.Line.OrientationChanged, handler)

  member this.OrientationChanging (handler: CancelEventArgs<Orientation> -> unit) =
    this.props.add (PKey.Line.OrientationChanging, handler)

type ListViewProps() =
  inherit ViewProps()
  // Properties
  member this.AllowsMarking (value: bool) =
    this.props.add (PKey.ListView.AllowsMarking, value)


  member this.AllowsMultipleSelection (value: bool) =
    this.props.add (PKey.ListView.AllowsMultipleSelection, value)


  member this.LeftItem (value: int) =
    this.props.add (PKey.ListView.LeftItem, value)


  member this.SelectedItem (value: Nullable<int>) =
    this.props.add (PKey.ListView.SelectedItem, value)


  member this.Source (value: IListDataSource) =
    this.props.add (PKey.ListView.Source, value)


  member this.TopItem (value: int) =
    this.props.add (PKey.ListView.TopItem, value)


  // Events
  member this.CollectionChanged (handler: NotifyCollectionChangedEventArgs -> unit) =
    this.props.add (PKey.ListView.CollectionChanged, handler)

  member this.OpenSelectedItem (handler: ListViewItemEventArgs -> unit) =
    this.props.add (PKey.ListView.OpenSelectedItem, handler)

  member this.RowRender (handler: ListViewRowEventArgs -> unit) =
    this.props.add (PKey.ListView.RowRender, handler)

  member this.SelectedItemChanged (handler: ListViewItemEventArgs -> unit) =
    this.props.add (PKey.ListView.SelectedItemChanged, handler)

type MarginProps() =
  inherit ViewProps()
  // Properties
  member this.ShadowStyle (value: ShadowStyle) =
    this.props.add (PKey.Margin.ShadowStyle, value)


type MenuProps() =
  inherit ViewProps()
  // Properties
  member this.SelectedMenuItem (value: MenuItem) =
    this.props.add (PKey.Menu.SelectedMenuItem, value)

  member this.SelectedMenuItem(value: IMenuItemTerminalElement) =
    this.props.add (PKey.Menu.SelectedMenuItem_element, value)

  member this.SuperMenuItem (value: MenuItem) =
    this.props.add (PKey.Menu.SuperMenuItem, value)

  member this.SuperMenuItem(value: IMenuItemTerminalElement) =
    this.props.add (PKey.Menu.SuperMenuItem_element, value)

  // Events
  member this.SelectedMenuItemChanged (handler: MenuItem -> unit) =
    this.props.add (PKey.Menu.SelectedMenuItemChanged, handler)

type MenuBarProps() =
  inherit ViewProps()
  // Properties
  member this.Key (value: Key) =
    this.props.add (PKey.MenuBar.Key, value)


  // Events
  member this.KeyChanged (handler: KeyChangedEventArgs -> unit) =
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

  member this.ValueChanged (handler: EventArgs<'T> -> unit) =
    this.props.add (PKey.NumericUpDown<'T>.ValueChanged, handler)

  member this.ValueChanging (handler: CancelEventArgs<'T> -> unit) =
    this.props.add (PKey.NumericUpDown<'T>.ValueChanging, handler)

type PaddingProps() =
  inherit ViewProps()
type PopoverBaseImplProps() =
  inherit ViewProps()
  // Properties
  member this.Current (value: IRunnable) =
    this.props.add (PKey.PopoverBaseImpl.Current, value)


type PopoverMenuProps() =
  inherit ViewProps()
  // Properties
  member this.Key (value: Key) =
    this.props.add (PKey.PopoverMenu.Key, value)


  member this.MouseFlags (value: MouseFlags) =
    this.props.add (PKey.PopoverMenu.MouseFlags, value)


  member this.Root (value: Menu) =
    this.props.add (PKey.PopoverMenu.Root, value)

  member this.Root(value: IMenuTerminalElement) =
    this.props.add (PKey.PopoverMenu.Root_element, value)

  // Events
  member this.KeyChanged (handler: KeyChangedEventArgs -> unit) =
    this.props.add (PKey.PopoverMenu.KeyChanged, handler)

type ProgressBarProps() =
  inherit ViewProps()
  // Properties
  member this.BidirectionalMarquee (value: bool) =
    this.props.add (PKey.ProgressBar.BidirectionalMarquee, value)


  member this.Fraction (value: Single) =
    this.props.add (PKey.ProgressBar.Fraction, value)


  member this.ProgressBarFormat (value: ProgressBarFormat) =
    this.props.add (PKey.ProgressBar.ProgressBarFormat, value)


  member this.ProgressBarStyle (value: ProgressBarStyle) =
    this.props.add (PKey.ProgressBar.ProgressBarStyle, value)


  member this.SegmentCharacter (value: Rune) =
    this.props.add (PKey.ProgressBar.SegmentCharacter, value)


  member this.Text (value: string) =
    this.props.add (PKey.ProgressBar.Text, value)


type RunnableProps() =
  inherit ViewProps()
  // Properties
  member this.Result (value: Object) =
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
  inherit ViewProps()
  // Properties
  member this.Result (value: 'TResult) =
    this.props.add (PKey.Runnable'<'TResult>.Result, value)


type ScrollBarProps() =
  inherit ViewProps()
  // Properties
  member this.AutoShow (value: bool) =
    this.props.add (PKey.ScrollBar.AutoShow, value)


  member this.Increment (value: int) =
    this.props.add (PKey.ScrollBar.Increment, value)


  member this.Orientation (value: Orientation) =
    this.props.add (PKey.ScrollBar.Orientation, value)


  member this.Position (value: int) =
    this.props.add (PKey.ScrollBar.Position, value)


  member this.ScrollableContentSize (value: int) =
    this.props.add (PKey.ScrollBar.ScrollableContentSize, value)


  member this.VisibleContentSize (value: int) =
    this.props.add (PKey.ScrollBar.VisibleContentSize, value)


  // Events
  member this.OrientationChanged (handler: EventArgs<Orientation> -> unit) =
    this.props.add (PKey.ScrollBar.OrientationChanged, handler)

  member this.OrientationChanging (handler: CancelEventArgs<Orientation> -> unit) =
    this.props.add (PKey.ScrollBar.OrientationChanging, handler)

  member this.PositionChanged (handler: EventArgs<int> -> unit) =
    this.props.add (PKey.ScrollBar.PositionChanged, handler)

  member this.PositionChanging (handler: CancelEventArgs<int> -> unit) =
    this.props.add (PKey.ScrollBar.PositionChanging, handler)

  member this.ScrollableContentSizeChanged (handler: EventArgs<int> -> unit) =
    this.props.add (PKey.ScrollBar.ScrollableContentSizeChanged, handler)

  member this.Scrolled (handler: EventArgs<int> -> unit) =
    this.props.add (PKey.ScrollBar.Scrolled, handler)

  member this.SliderPositionChanged (handler: EventArgs<int> -> unit) =
    this.props.add (PKey.ScrollBar.SliderPositionChanged, handler)

type ScrollSliderProps() =
  inherit ViewProps()
  // Properties
  member this.Orientation (value: Orientation) =
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
  member this.OrientationChanged (handler: EventArgs<Orientation> -> unit) =
    this.props.add (PKey.ScrollSlider.OrientationChanged, handler)

  member this.OrientationChanging (handler: CancelEventArgs<Orientation> -> unit) =
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
  member this.AssignHotKeys (value: bool) =
    this.props.add (PKey.SelectorBase.AssignHotKeys, value)


  member this.DoubleClickAccepts (value: bool) =
    this.props.add (PKey.SelectorBase.DoubleClickAccepts, value)


  member this.HorizontalSpace (value: int) =
    this.props.add (PKey.SelectorBase.HorizontalSpace, value)


  member this.Labels (value: IReadOnlyList<string>) =
    this.props.add (PKey.SelectorBase.Labels, value)


  member this.Orientation (value: Orientation) =
    this.props.add (PKey.SelectorBase.Orientation, value)


  member this.Styles (value: SelectorStyles) =
    this.props.add (PKey.SelectorBase.Styles, value)


  member this.UsedHotKeys (value: HashSet<Key>) =
    this.props.add (PKey.SelectorBase.UsedHotKeys, value)


  member this.Value (value: Nullable<int>) =
    this.props.add (PKey.SelectorBase.Value, value)


  member this.Values (value: IReadOnlyList<int>) =
    this.props.add (PKey.SelectorBase.Values, value)


  // Events
  member this.OrientationChanged (handler: EventArgs<Orientation> -> unit) =
    this.props.add (PKey.SelectorBase.OrientationChanged, handler)

  member this.OrientationChanging (handler: CancelEventArgs<Orientation> -> unit) =
    this.props.add (PKey.SelectorBase.OrientationChanging, handler)

  member this.ValueChanged (handler: EventArgs<Nullable<int>> -> unit) =
    this.props.add (PKey.SelectorBase.ValueChanged, handler)

type FlagSelectorProps() =
  inherit ViewProps()
  // Properties
  member this.Value (value: Nullable<int>) =
    this.props.add (PKey.FlagSelector.Value, value)


type OptionSelectorProps() =
  inherit ViewProps()
  // Properties
  member this.Cursor (value: int) =
    this.props.add (PKey.OptionSelector.Cursor, value)


type FlagSelectorProps<'TFlagsEnum when 'TFlagsEnum: struct and 'TFlagsEnum: (new: unit -> 'TFlagsEnum) and 'TFlagsEnum:> Enum and 'TFlagsEnum:> ValueType>() =
  inherit ViewProps()
  // Properties
  member this.Value (value: Nullable<'TFlagsEnum>) =
    this.props.add (PKey.FlagSelector'<'TFlagsEnum>.Value, value)


  // Events
  member this.ValueChanged (handler: EventArgs<Nullable<'TFlagsEnum>> -> unit) =
    this.props.add (PKey.FlagSelector'<'TFlagsEnum>.ValueChanged, handler)

type OptionSelectorProps<'TEnum when 'TEnum: struct and 'TEnum: (new: unit -> 'TEnum) and 'TEnum:> Enum and 'TEnum:> ValueType>() =
  inherit ViewProps()
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
  member this.Action (value: Action) =
    this.props.add (PKey.Shortcut.Action, value)


  member this.AlignmentModes (value: AlignmentModes) =
    this.props.add (PKey.Shortcut.AlignmentModes, value)


  member this.BindKeyToApplication (value: bool) =
    this.props.add (PKey.Shortcut.BindKeyToApplication, value)


  member this.CommandView (value: View) =
    this.props.add (PKey.Shortcut.CommandView, value)

  member this.CommandView(value: ITerminalElement) =
    this.props.add (PKey.Shortcut.CommandView_element, value)

  member this.ForceFocusColors (value: bool) =
    this.props.add (PKey.Shortcut.ForceFocusColors, value)


  member this.HelpText (value: string) =
    this.props.add (PKey.Shortcut.HelpText, value)


  member this.Key (value: Key) =
    this.props.add (PKey.Shortcut.Key, value)


  member this.MinimumKeyTextSize (value: int) =
    this.props.add (PKey.Shortcut.MinimumKeyTextSize, value)


  member this.Orientation (value: Orientation) =
    this.props.add (PKey.Shortcut.Orientation, value)


  member this.Text (value: string) =
    this.props.add (PKey.Shortcut.Text, value)


  // Events
  member this.OrientationChanged (handler: EventArgs<Orientation> -> unit) =
    this.props.add (PKey.Shortcut.OrientationChanged, handler)

  member this.OrientationChanging (handler: CancelEventArgs<Orientation> -> unit) =
    this.props.add (PKey.Shortcut.OrientationChanging, handler)

type MenuItemProps() =
  inherit ViewProps()
  // Properties
  member this.Command (value: Command) =
    this.props.add (PKey.MenuItem.Command, value)


  member this.SubMenu (value: Menu) =
    this.props.add (PKey.MenuItem.SubMenu, value)

  member this.SubMenu(value: IMenuTerminalElement) =
    this.props.add (PKey.MenuItem.SubMenu_element, value)

  member this.TargetView (value: View) =
    this.props.add (PKey.MenuItem.TargetView, value)

  member this.TargetView(value: ITerminalElement) =
    this.props.add (PKey.MenuItem.TargetView_element, value)

type MenuBarItemProps() =
  inherit ViewProps()
  // Properties
  member this.PopoverMenu (value: PopoverMenu) =
    this.props.add (PKey.MenuBarItem.PopoverMenu, value)

  member this.PopoverMenu(value: IPopoverMenuTerminalElement) =
    this.props.add (PKey.MenuBarItem.PopoverMenu_element, value)

  member this.PopoverMenuOpen (value: bool) =
    this.props.add (PKey.MenuBarItem.PopoverMenuOpen, value)


  member this.SubMenu (value: Menu) =
    this.props.add (PKey.MenuBarItem.SubMenu, value)

  member this.SubMenu(value: IMenuTerminalElement) =
    this.props.add (PKey.MenuBarItem.SubMenu_element, value)

  // Events
  member this.PopoverMenuOpenChanged (handler: EventArgs<bool> -> unit) =
    this.props.add (PKey.MenuBarItem.PopoverMenuOpenChanged, handler)

type SliderProps<'T>() =
  inherit ViewProps()
  // Properties
  member this.AllowEmpty (value: bool) =
    this.props.add (PKey.Slider<'T>.AllowEmpty, value)


  member this.FocusedOption (value: int) =
    this.props.add (PKey.Slider<'T>.FocusedOption, value)


  member this.LegendsOrientation (value: Orientation) =
    this.props.add (PKey.Slider<'T>.LegendsOrientation, value)


  member this.MinimumInnerSpacing (value: int) =
    this.props.add (PKey.Slider<'T>.MinimumInnerSpacing, value)


  member this.Options (value: List<SliderOption<'T>>) =
    this.props.add (PKey.Slider<'T>.Options, value)


  member this.Orientation (value: Orientation) =
    this.props.add (PKey.Slider<'T>.Orientation, value)


  member this.RangeAllowSingle (value: bool) =
    this.props.add (PKey.Slider<'T>.RangeAllowSingle, value)


  member this.ShowEndSpacing (value: bool) =
    this.props.add (PKey.Slider<'T>.ShowEndSpacing, value)


  member this.ShowLegends (value: bool) =
    this.props.add (PKey.Slider<'T>.ShowLegends, value)


  member this.Style (value: SliderStyle) =
    this.props.add (PKey.Slider<'T>.Style, value)


  member this.Text (value: string) =
    this.props.add (PKey.Slider<'T>.Text, value)


  member this.Type (value: SliderType) =
    this.props.add (PKey.Slider<'T>.Type, value)


  member this.UseMinimumSize (value: bool) =
    this.props.add (PKey.Slider<'T>.UseMinimumSize, value)


  // Events
  member this.OptionFocused (handler: SliderEventArgs<'T> -> unit) =
    this.props.add (PKey.Slider<'T>.OptionFocused, handler)

  member this.OptionsChanged (handler: SliderEventArgs<'T> -> unit) =
    this.props.add (PKey.Slider<'T>.OptionsChanged, handler)

  member this.OrientationChanged (handler: EventArgs<Orientation> -> unit) =
    this.props.add (PKey.Slider<'T>.OrientationChanged, handler)

  member this.OrientationChanging (handler: CancelEventArgs<Orientation> -> unit) =
    this.props.add (PKey.Slider<'T>.OrientationChanging, handler)

type SpinnerViewProps() =
  inherit ViewProps()
  // Properties
  member this.AutoSpin (value: bool) =
    this.props.add (PKey.SpinnerView.AutoSpin, value)


  member this.Sequence (value: String[]) =
    this.props.add (PKey.SpinnerView.Sequence, value)


  member this.SpinBounce (value: bool) =
    this.props.add (PKey.SpinnerView.SpinBounce, value)


  member this.SpinDelay (value: int) =
    this.props.add (PKey.SpinnerView.SpinDelay, value)


  member this.SpinReverse (value: bool) =
    this.props.add (PKey.SpinnerView.SpinReverse, value)


  member this.Style (value: SpinnerStyle) =
    this.props.add (PKey.SpinnerView.Style, value)


type StatusBarProps() =
  inherit ViewProps()
type TabProps() =
  inherit ViewProps()
  // Properties
  member this.DisplayText (value: string) =
    this.props.add (PKey.Tab.DisplayText, value)


  member this.View (value: View) =
    this.props.add (PKey.Tab.View, value)

  member this.View(value: ITerminalElement) =
    this.props.add (PKey.Tab.View_element, value)

type TabViewProps() =
  inherit ViewProps()
  // Properties
  member this.MaxTabTextWidth (value: UInt32) =
    this.props.add (PKey.TabView.MaxTabTextWidth, value)


  member this.SelectedTab (value: Tab) =
    this.props.add (PKey.TabView.SelectedTab, value)

  member this.SelectedTab(value: ITabTerminalElement) =
    this.props.add (PKey.TabView.SelectedTab_element, value)

  member this.Style (value: TabStyle) =
    this.props.add (PKey.TabView.Style, value)


  member this.TabScrollOffset (value: int) =
    this.props.add (PKey.TabView.TabScrollOffset, value)


  // Events
  member this.SelectedTabChanged (handler: TabChangedEventArgs -> unit) =
    this.props.add (PKey.TabView.SelectedTabChanged, handler)

  member this.TabClicked (handler: TabMouseEventArgs -> unit) =
    this.props.add (PKey.TabView.TabClicked, handler)

type TableViewProps() =
  inherit ViewProps()
  // Properties
  member this.CellActivationKey (value: KeyCode) =
    this.props.add (PKey.TableView.CellActivationKey, value)


  member this.CollectionNavigator (value: ICollectionNavigator) =
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


  member this.SeparatorSymbol (value: Char) =
    this.props.add (PKey.TableView.SeparatorSymbol, value)


  member this.Style (value: TableStyle) =
    this.props.add (PKey.TableView.Style, value)


  member this.Table (value: ITableSource) =
    this.props.add (PKey.TableView.Table, value)


  // Events
  member this.CellActivated (handler: CellActivatedEventArgs -> unit) =
    this.props.add (PKey.TableView.CellActivated, handler)

  member this.CellToggled (handler: CellToggledEventArgs -> unit) =
    this.props.add (PKey.TableView.CellToggled, handler)

  member this.SelectedCellChanged (handler: SelectedCellChangedEventArgs -> unit) =
    this.props.add (PKey.TableView.SelectedCellChanged, handler)

type TextFieldProps() =
  inherit ViewProps()
  // Properties
  member this.Autocomplete (value: IAutocomplete) =
    this.props.add (PKey.TextField.Autocomplete, value)


  member this.CursorPosition (value: int) =
    this.props.add (PKey.TextField.CursorPosition, value)


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


  // Events
  member this.TextChanging (handler: ResultEventArgs<string> -> unit) =
    this.props.add (PKey.TextField.TextChanging, handler)

type DateFieldProps() =
  inherit ViewProps()
  // Properties
  member this.Culture (value: CultureInfo) =
    this.props.add (PKey.DateField.Culture, value)


  member this.CursorPosition (value: int) =
    this.props.add (PKey.DateField.CursorPosition, value)


  member this.Date (value: Nullable<DateTime>) =
    this.props.add (PKey.DateField.Date, value)


  // Events
  member this.DateChanged (handler: EventArgs<DateTime> -> unit) =
    this.props.add (PKey.DateField.DateChanged, handler)

type TextValidateFieldProps() =
  inherit ViewProps()
  // Properties
  member this.Provider (value: ITextValidateProvider) =
    this.props.add (PKey.TextValidateField.Provider, value)


  member this.Text (value: string) =
    this.props.add (PKey.TextValidateField.Text, value)


type TextViewProps() =
  inherit ViewProps()
  // Properties
  member this.AllowsReturn (value: bool) =
    this.props.add (PKey.TextView.AllowsReturn, value)


  member this.AllowsTab (value: bool) =
    this.props.add (PKey.TextView.AllowsTab, value)


  member this.CursorPosition (value: Point) =
    this.props.add (PKey.TextView.CursorPosition, value)


  member this.InheritsPreviousAttribute (value: bool) =
    this.props.add (PKey.TextView.InheritsPreviousAttribute, value)


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


  member this.SelectWordOnlyOnDoubleClick (value: bool) =
    this.props.add (PKey.TextView.SelectWordOnlyOnDoubleClick, value)


  member this.SelectionStartColumn (value: int) =
    this.props.add (PKey.TextView.SelectionStartColumn, value)


  member this.SelectionStartRow (value: int) =
    this.props.add (PKey.TextView.SelectionStartRow, value)


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
  member this.ContentsChanged (handler: ContentsChangedEventArgs -> unit) =
    this.props.add (PKey.TextView.ContentsChanged, handler)

  member this.DrawNormalColor (handler: CellEventArgs -> unit) =
    this.props.add (PKey.TextView.DrawNormalColor, handler)

  member this.DrawReadOnlyColor (handler: CellEventArgs -> unit) =
    this.props.add (PKey.TextView.DrawReadOnlyColor, handler)

  member this.DrawSelectionColor (handler: CellEventArgs -> unit) =
    this.props.add (PKey.TextView.DrawSelectionColor, handler)

  member this.DrawUsedColor (handler: CellEventArgs -> unit) =
    this.props.add (PKey.TextView.DrawUsedColor, handler)

  member this.UnwrappedCursorPosition (handler: Point -> unit) =
    this.props.add (PKey.TextView.UnwrappedCursorPosition, handler)

type TimeFieldProps() =
  inherit ViewProps()
  // Properties
  member this.CursorPosition (value: int) =
    this.props.add (PKey.TimeField.CursorPosition, value)


  member this.IsShortFormat (value: bool) =
    this.props.add (PKey.TimeField.IsShortFormat, value)


  member this.Time (value: TimeSpan) =
    this.props.add (PKey.TimeField.Time, value)


  // Events
  member this.TimeChanged (handler: EventArgs<TimeSpan> -> unit) =
    this.props.add (PKey.TimeField.TimeChanged, handler)

type TreeViewProps<'T when 'T: not struct>() =
  inherit ViewProps()
  // Properties
  member this.AllowLetterBasedNavigation (value: bool) =
    this.props.add (PKey.TreeView<'T>.AllowLetterBasedNavigation, value)


  member this.AspectGetter (value: AspectGetterDelegate<'T>) =
    this.props.add (PKey.TreeView<'T>.AspectGetter, value)


  member this.ColorGetter (value: Func<'T, Scheme>) =
    this.props.add (PKey.TreeView<'T>.ColorGetter, value)


  member this.MaxDepth (value: int) =
    this.props.add (PKey.TreeView<'T>.MaxDepth, value)


  member this.MultiSelect (value: bool) =
    this.props.add (PKey.TreeView<'T>.MultiSelect, value)


  member this.ObjectActivationButton (value: Nullable<MouseFlags>) =
    this.props.add (PKey.TreeView<'T>.ObjectActivationButton, value)


  member this.ObjectActivationKey (value: KeyCode) =
    this.props.add (PKey.TreeView<'T>.ObjectActivationKey, value)


  member this.ScrollOffsetHorizontal (value: int) =
    this.props.add (PKey.TreeView<'T>.ScrollOffsetHorizontal, value)


  member this.ScrollOffsetVertical (value: int) =
    this.props.add (PKey.TreeView<'T>.ScrollOffsetVertical, value)


  member this.SelectedObject (value: 'T) =
    this.props.add (PKey.TreeView<'T>.SelectedObject, value)


  member this.Style (value: TreeStyle) =
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

type WindowProps() =
  inherit ViewProps()
type DialogProps() =
  inherit ViewProps()
  // Properties
  member this.ButtonAlignment (value: Alignment) =
    this.props.add (PKey.Dialog.ButtonAlignment, value)


  member this.ButtonAlignmentModes (value: AlignmentModes) =
    this.props.add (PKey.Dialog.ButtonAlignmentModes, value)


  member this.Canceled (value: bool) =
    this.props.add (PKey.Dialog.Canceled, value)


type FileDialogProps() =
  inherit ViewProps()
  // Properties
  member this.AllowedTypes (value: List<IAllowedType>) =
    this.props.add (PKey.FileDialog.AllowedTypes, value)


  member this.AllowsMultipleSelection (value: bool) =
    this.props.add (PKey.FileDialog.AllowsMultipleSelection, value)


  member this.FileOperationsHandler (value: IFileOperations) =
    this.props.add (PKey.FileDialog.FileOperationsHandler, value)


  member this.MustExist (value: bool) =
    this.props.add (PKey.FileDialog.MustExist, value)


  member this.OpenMode (value: OpenMode) =
    this.props.add (PKey.FileDialog.OpenMode, value)


  member this.Path (value: string) =
    this.props.add (PKey.FileDialog.Path, value)


  member this.SearchMatcher (value: ISearchMatcher) =
    this.props.add (PKey.FileDialog.SearchMatcher, value)


  // Events
  member this.FilesSelected (handler: FilesSelectedEventArgs -> unit) =
    this.props.add (PKey.FileDialog.FilesSelected, handler)

type OpenDialogProps() =
  inherit ViewProps()
  // Properties
  member this.OpenMode (value: OpenMode) =
    this.props.add (PKey.OpenDialog.OpenMode, value)


type SaveDialogProps() =
  inherit ViewProps()
type WizardProps() =
  inherit ViewProps()
  // Properties
  member this.CurrentStep (value: WizardStep) =
    this.props.add (PKey.Wizard.CurrentStep, value)

  member this.CurrentStep(value: IWizardStepTerminalElement) =
    this.props.add (PKey.Wizard.CurrentStep_element, value)

  // Events
  member this.Cancelled (handler: WizardButtonEventArgs -> unit) =
    this.props.add (PKey.Wizard.Cancelled, handler)

  member this.Finished (handler: WizardButtonEventArgs -> unit) =
    this.props.add (PKey.Wizard.Finished, handler)

  member this.MovingBack (handler: WizardButtonEventArgs -> unit) =
    this.props.add (PKey.Wizard.MovingBack, handler)

  member this.MovingNext (handler: WizardButtonEventArgs -> unit) =
    this.props.add (PKey.Wizard.MovingNext, handler)

  member this.StepChanged (handler: StepChangeEventArgs -> unit) =
    this.props.add (PKey.Wizard.StepChanged, handler)

  member this.StepChanging (handler: StepChangeEventArgs -> unit) =
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

