namespace Terminal.Gui.Elmish

open System
open System.Collections.Generic
open Terminal.Gui.App
open Terminal.Gui.Elmish
open Terminal.Gui.Views

type ViewProps() =
  member val internal props = Props()

  /// Stable identity among sibling virtual views.
  member this.Key(value: string) = this.props.Key <- Some value

  member this.Children(children: IView list) =
    children |> List.map (fun x -> ViewSpec.from x) |> this.props.Children.AddRange

  // Delayed Positions
  member this.X(value: TPos) = this.props.X <- value

  member this.Y(value: TPos) = this.props.Y <- value

  // Properties
  member this.App(value: Terminal.Gui.App.IApplication) =
    this.props |> Props.add (PKey.View.App, value)


  member this.Arrangement(value: Terminal.Gui.ViewBase.ViewArrangement) =
    this.props |> Props.add (PKey.View.Arrangement, value)


  member this.AssignHotKeys(value: bool) =
    this.props |> Props.add (PKey.View.AssignHotKeys, value)


  member this.BorderStyle(value: Nullable<Terminal.Gui.Drawing.LineStyle>) =
    this.props |> Props.add (PKey.View.BorderStyle, value)


  member this.CanFocus(value: bool) =
    this.props |> Props.add (PKey.View.CanFocus, value)


  member this.CommandsToBubbleUp(value: IReadOnlyList<Terminal.Gui.Input.Command>) =
    this.props |> Props.add (PKey.View.CommandsToBubbleUp, value)


  member this.ContentSizeTracksViewport(value: bool) =
    this.props |> Props.add (PKey.View.ContentSizeTracksViewport, value)


  member this.Cursor(value: Terminal.Gui.Drivers.Cursor) =
    this.props |> Props.add (PKey.View.Cursor, value)


  member this.Data(value: System.Object) =
    this.props |> Props.add (PKey.View.Data, value)


  member this.DefaultAcceptView(value: Terminal.Gui.ViewBase.View) =
    this.props |> Props.add (PKey.View.DefaultAcceptView, value)

  member this.DefaultAcceptView(value: IView) =
    this.props |> Props.add (PKey.View.DefaultAcceptView_viewSpec, value)

  member this.Enabled(value: bool) =
    this.props |> Props.add (PKey.View.Enabled, value)


  member this.Frame(value: System.Drawing.Rectangle) =
    this.props |> Props.add (PKey.View.Frame, value)


  member this.HasFocus(value: bool) =
    this.props |> Props.add (PKey.View.HasFocus, value)


  member this.Height(value: Terminal.Gui.ViewBase.Dim) =
    this.props |> Props.add (PKey.View.Height, value)


  member this.HotKey(value: Terminal.Gui.Input.Key) =
    this.props |> Props.add (PKey.View.HotKey, value)


  member this.HotKeySpecifier(value: System.Text.Rune) =
    this.props |> Props.add (PKey.View.HotKeySpecifier, value)


  member this.Id(value: string) =
    this.props |> Props.add (PKey.View.Id, value)


  member this.IsInitialized(value: bool) =
    this.props |> Props.add (PKey.View.IsInitialized, value)


  member this.MouseHighlightStates(value: Terminal.Gui.ViewBase.MouseState) =
    this.props |> Props.add (PKey.View.MouseHighlightStates, value)


  member this.MouseHoldRepeat(value: Nullable<Terminal.Gui.Input.MouseFlags>) =
    this.props |> Props.add (PKey.View.MouseHoldRepeat, value)


  member this.MousePositionTracking(value: bool) =
    this.props |> Props.add (PKey.View.MousePositionTracking, value)


  member this.PreserveTrailingSpaces(value: bool) =
    this.props |> Props.add (PKey.View.PreserveTrailingSpaces, value)


  member this.SchemeName(value: string) =
    this.props |> Props.add (PKey.View.SchemeName, value)


  member this.ShadowStyle(value: Nullable<Terminal.Gui.ViewBase.ShadowStyles>) =
    this.props |> Props.add (PKey.View.ShadowStyle, value)


  member this.SuperViewRendersLineCanvas(value: bool) =
    this.props |> Props.add (PKey.View.SuperViewRendersLineCanvas, value)


  member this.TabStop(value: Nullable<Terminal.Gui.ViewBase.TabBehavior>) =
    this.props |> Props.add (PKey.View.TabStop, value)


  member this.Text(value: string) =
    this.props |> Props.add (PKey.View.Text, value)


  member this.TextAlignment(value: Terminal.Gui.ViewBase.Alignment) =
    this.props |> Props.add (PKey.View.TextAlignment, value)


  member this.TextDirection(value: Terminal.Gui.Text.TextDirection) =
    this.props |> Props.add (PKey.View.TextDirection, value)


  member this.Title(value: string) =
    this.props |> Props.add (PKey.View.Title, value)


  member this.UsedHotKeys(value: HashSet<Terminal.Gui.Input.Key>) =
    this.props |> Props.add (PKey.View.UsedHotKeys, value)


  member this.ValidatePosDim(value: bool) =
    this.props |> Props.add (PKey.View.ValidatePosDim, value)


  member this.VerticalTextAlignment(value: Terminal.Gui.ViewBase.Alignment) =
    this.props |> Props.add (PKey.View.VerticalTextAlignment, value)


  member this.Viewport(value: System.Drawing.Rectangle) =
    this.props |> Props.add (PKey.View.Viewport, value)


  member this.ViewportSettings(value: Terminal.Gui.ViewBase.ViewportSettingsFlags) =
    this.props |> Props.add (PKey.View.ViewportSettings, value)


  member this.Visible(value: bool) =
    this.props |> Props.add (PKey.View.Visible, value)


  member this.Width(value: Terminal.Gui.ViewBase.Dim) =
    this.props |> Props.add (PKey.View.Width, value)


  // Events
  member this.Accepted(handler: Terminal.Gui.Input.CommandEventArgs -> unit) =
    this.props |> Props.add (PKey.View.Accepted, handler)

  member this.Accepting(handler: Terminal.Gui.Input.CommandEventArgs -> unit) =
    this.props |> Props.add (PKey.View.Accepting, handler)

  member this.Activated(handler: EventArgs<Terminal.Gui.Input.ICommandContext> -> unit) =
    this.props |> Props.add (PKey.View.Activated, handler)

  member this.Activating(handler: Terminal.Gui.Input.CommandEventArgs -> unit) =
    this.props |> Props.add (PKey.View.Activating, handler)

  member this.AdvancingFocus(handler: Terminal.Gui.ViewBase.AdvanceFocusEventArgs -> unit) =
    this.props |> Props.add (PKey.View.AdvancingFocus, handler)

  member this.BorderStyleChanged(handler: System.EventArgs -> unit) =
    this.props |> Props.add (PKey.View.BorderStyleChanged, handler)

  member this.CanFocusChanged(handler: System.EventArgs -> unit) =
    this.props |> Props.add (PKey.View.CanFocusChanged, handler)

  member this.ClearedViewport(handler: Terminal.Gui.ViewBase.DrawEventArgs -> unit) =
    this.props |> Props.add (PKey.View.ClearedViewport, handler)

  member this.ClearingViewport(handler: Terminal.Gui.ViewBase.DrawEventArgs -> unit) =
    this.props |> Props.add (PKey.View.ClearingViewport, handler)

  member this.CommandNotBound(handler: Terminal.Gui.Input.CommandEventArgs -> unit) =
    this.props |> Props.add (PKey.View.CommandNotBound, handler)

  member this.ContentSizeChanged(handler: ValueChangedEventArgs<Nullable<System.Drawing.Size>> -> unit) =
    this.props |> Props.add (PKey.View.ContentSizeChanged, handler)

  member this.ContentSizeChanging(handler: ValueChangingEventArgs<Nullable<System.Drawing.Size>> -> unit) =
    this.props |> Props.add (PKey.View.ContentSizeChanging, handler)

  member this.Disposing(handler: System.EventArgs -> unit) =
    this.props |> Props.add (PKey.View.Disposing, handler)

  member this.DrawComplete(handler: Terminal.Gui.ViewBase.DrawEventArgs -> unit) =
    this.props |> Props.add (PKey.View.DrawComplete, handler)

  member this.DrawingContent(handler: Terminal.Gui.ViewBase.DrawEventArgs -> unit) =
    this.props |> Props.add (PKey.View.DrawingContent, handler)

  member this.DrawingSubViews(handler: Terminal.Gui.ViewBase.DrawEventArgs -> unit) =
    this.props |> Props.add (PKey.View.DrawingSubViews, handler)

  member this.DrawingText(handler: Terminal.Gui.ViewBase.DrawEventArgs -> unit) =
    this.props |> Props.add (PKey.View.DrawingText, handler)

  member this.DrewText(handler: System.EventArgs -> unit) =
    this.props |> Props.add (PKey.View.DrewText, handler)

  member this.EnabledChanged(handler: System.EventArgs -> unit) =
    this.props |> Props.add (PKey.View.EnabledChanged, handler)

  member this.FocusedChanged(handler: Terminal.Gui.ViewBase.HasFocusEventArgs -> unit) =
    this.props |> Props.add (PKey.View.FocusedChanged, handler)

  member this.FrameChanged(handler: EventArgs<System.Drawing.Rectangle> -> unit) =
    this.props |> Props.add (PKey.View.FrameChanged, handler)

  member this.GettingAttributeForRole(handler: Terminal.Gui.Drawing.VisualRoleEventArgs -> unit) =
    this.props |> Props.add (PKey.View.GettingAttributeForRole, handler)

  member this.GettingScheme(handler: ResultEventArgs<Terminal.Gui.Drawing.Scheme> -> unit) =
    this.props |> Props.add (PKey.View.GettingScheme, handler)

  member this.HandlingHotKey(handler: Terminal.Gui.Input.CommandEventArgs -> unit) =
    this.props |> Props.add (PKey.View.HandlingHotKey, handler)

  member this.HasFocusChanged(handler: Terminal.Gui.ViewBase.HasFocusEventArgs -> unit) =
    this.props |> Props.add (PKey.View.HasFocusChanged, handler)

  member this.HasFocusChanging(handler: Terminal.Gui.ViewBase.HasFocusEventArgs -> unit) =
    this.props |> Props.add (PKey.View.HasFocusChanging, handler)

  member this.HeightChanged(handler: ValueChangedEventArgs<Terminal.Gui.ViewBase.Dim> -> unit) =
    this.props |> Props.add (PKey.View.HeightChanged, handler)

  member this.HeightChanging(handler: ValueChangingEventArgs<Terminal.Gui.ViewBase.Dim> -> unit) =
    this.props |> Props.add (PKey.View.HeightChanging, handler)

  member this.HotKeyChanged(handler: Terminal.Gui.Input.KeyChangedEventArgs -> unit) =
    this.props |> Props.add (PKey.View.HotKeyChanged, handler)

  member this.HotKeyCommand(handler: EventArgs<Terminal.Gui.Input.ICommandContext> -> unit) =
    this.props |> Props.add (PKey.View.HotKeyCommand, handler)

  member this.Initialized(handler: System.EventArgs -> unit) =
    this.props |> Props.add (PKey.View.Initialized, handler)

  member this.KeyDown(handler: Terminal.Gui.Input.Key -> unit) =
    this.props |> Props.add (PKey.View.KeyDown, handler)

  member this.KeyDownNotHandled(handler: Terminal.Gui.Input.Key -> unit) =
    this.props |> Props.add (PKey.View.KeyDownNotHandled, handler)

  member this.KeyUp(handler: Terminal.Gui.Input.Key -> unit) =
    this.props |> Props.add (PKey.View.KeyUp, handler)

  member this.MouseEnter(handler: System.ComponentModel.CancelEventArgs -> unit) =
    this.props |> Props.add (PKey.View.MouseEnter, handler)

  member this.MouseEvent(handler: Terminal.Gui.Input.Mouse -> unit) =
    this.props |> Props.add (PKey.View.MouseEvent, handler)

  member this.MouseHoldRepeatChanged(handler: ValueChangedEventArgs<Nullable<Terminal.Gui.Input.MouseFlags>> -> unit) =
    this.props |> Props.add (PKey.View.MouseHoldRepeatChanged, handler)

  member this.MouseHoldRepeatChanging
    (handler: ValueChangingEventArgs<Nullable<Terminal.Gui.Input.MouseFlags>> -> unit)
    =
    this.props |> Props.add (PKey.View.MouseHoldRepeatChanging, handler)

  member this.MouseLeave(handler: System.EventArgs -> unit) =
    this.props |> Props.add (PKey.View.MouseLeave, handler)

  member this.MouseStateChanged(handler: EventArgs<Terminal.Gui.ViewBase.MouseState> -> unit) =
    this.props |> Props.add (PKey.View.MouseStateChanged, handler)

  member this.Pasted(handler: Terminal.Gui.Input.PastedEventArgs -> unit) =
    this.props |> Props.add (PKey.View.Pasted, handler)

  member this.Pasting(handler: Terminal.Gui.Input.PastingEventArgs -> unit) =
    this.props |> Props.add (PKey.View.Pasting, handler)

  member this.Removed(handler: Terminal.Gui.ViewBase.SuperViewChangedEventArgs -> unit) =
    this.props |> Props.add (PKey.View.Removed, handler)

  member this.SchemeChanged(handler: ValueChangedEventArgs<Terminal.Gui.Drawing.Scheme> -> unit) =
    this.props |> Props.add (PKey.View.SchemeChanged, handler)

  member this.SchemeChanging(handler: ValueChangingEventArgs<Terminal.Gui.Drawing.Scheme> -> unit) =
    this.props |> Props.add (PKey.View.SchemeChanging, handler)

  member this.SchemeNameChanged(handler: ValueChangedEventArgs<string> -> unit) =
    this.props |> Props.add (PKey.View.SchemeNameChanged, handler)

  member this.SchemeNameChanging(handler: ValueChangingEventArgs<string> -> unit) =
    this.props |> Props.add (PKey.View.SchemeNameChanging, handler)

  member this.ShadowStyleChanged(handler: System.EventArgs -> unit) =
    this.props |> Props.add (PKey.View.ShadowStyleChanged, handler)

  member this.SubViewAdded(handler: Terminal.Gui.ViewBase.SuperViewChangedEventArgs -> unit) =
    this.props |> Props.add (PKey.View.SubViewAdded, handler)

  member this.SubViewAdding(handler: EventArgs<Terminal.Gui.ViewBase.View> -> unit) =
    this.props |> Props.add (PKey.View.SubViewAdding, handler)

  member this.SubViewLayout(handler: Terminal.Gui.ViewBase.LayoutEventArgs -> unit) =
    this.props |> Props.add (PKey.View.SubViewLayout, handler)

  member this.SubViewRemoved(handler: Terminal.Gui.ViewBase.SuperViewChangedEventArgs -> unit) =
    this.props |> Props.add (PKey.View.SubViewRemoved, handler)

  member this.SubViewsLaidOut(handler: Terminal.Gui.ViewBase.LayoutEventArgs -> unit) =
    this.props |> Props.add (PKey.View.SubViewsLaidOut, handler)

  member this.SuperViewChanged(handler: ValueChangedEventArgs<Terminal.Gui.ViewBase.View> -> unit) =
    this.props |> Props.add (PKey.View.SuperViewChanged, handler)

  member this.SuperViewChanging(handler: ValueChangingEventArgs<Terminal.Gui.ViewBase.View> -> unit) =
    this.props |> Props.add (PKey.View.SuperViewChanging, handler)

  member this.TextChanged(handler: System.EventArgs -> unit) =
    this.props |> Props.add (PKey.View.TextChanged, handler)

  member this.TitleChanged(handler: EventArgs<string> -> unit) =
    this.props |> Props.add (PKey.View.TitleChanged, handler)

  member this.TitleChanging(handler: CancelEventArgs<string> -> unit) =
    this.props |> Props.add (PKey.View.TitleChanging, handler)

  member this.ViewportChanged(handler: Terminal.Gui.ViewBase.DrawEventArgs -> unit) =
    this.props |> Props.add (PKey.View.ViewportChanged, handler)

  member this.VisibleChanged(handler: System.EventArgs -> unit) =
    this.props |> Props.add (PKey.View.VisibleChanged, handler)

  member this.VisibleChanging(handler: CancelEventArgs<bool> -> unit) =
    this.props |> Props.add (PKey.View.VisibleChanging, handler)

  member this.WidthChanged(handler: ValueChangedEventArgs<Terminal.Gui.ViewBase.Dim> -> unit) =
    this.props |> Props.add (PKey.View.WidthChanged, handler)

  member this.WidthChanging(handler: ValueChangingEventArgs<Terminal.Gui.ViewBase.Dim> -> unit) =
    this.props |> Props.add (PKey.View.WidthChanging, handler)

type AdornmentViewProps() =
  inherit ViewProps()
  // Properties
  member this.Adornment(value: Terminal.Gui.ViewBase.IAdornment) =
    this.props |> Props.add (PKey.AdornmentView.Adornment, value)


  member this.Diagnostics(value: Terminal.Gui.ViewBase.ViewDiagnosticFlags) =
    this.props |> Props.add (PKey.AdornmentView.Diagnostics, value)


  member this.SuperViewRendersLineCanvas(value: bool) =
    this.props |> Props.add (PKey.AdornmentView.SuperViewRendersLineCanvas, value)


  member this.Viewport(value: System.Drawing.Rectangle) =
    this.props |> Props.add (PKey.AdornmentView.Viewport, value)


type AttributePickerProps() =
  inherit ViewProps()
  // Properties
  member this.SampleText(value: string) =
    this.props |> Props.add (PKey.AttributePicker.SampleText, value)


  member this.Value(value: Nullable<Terminal.Gui.Drawing.Attribute>) =
    this.props |> Props.add (PKey.AttributePicker.Value, value)


  // Events
  member this.ValueChanged(handler: ValueChangedEventArgs<Nullable<Terminal.Gui.Drawing.Attribute>> -> unit) =
    this.props |> Props.add (PKey.AttributePicker.ValueChanged, handler)

  member this.ValueChangedUntyped(handler: ValueChangedEventArgs<System.Object> -> unit) =
    this.props |> Props.add (PKey.AttributePicker.ValueChangedUntyped, handler)

  member this.ValueChanging(handler: ValueChangingEventArgs<Nullable<Terminal.Gui.Drawing.Attribute>> -> unit) =
    this.props |> Props.add (PKey.AttributePicker.ValueChanging, handler)

type BarProps() =
  inherit ViewProps()
  // Properties
  member this.AlignmentModes(value: Terminal.Gui.ViewBase.AlignmentModes) =
    this.props |> Props.add (PKey.Bar.AlignmentModes, value)


  member this.Orientation(value: Terminal.Gui.ViewBase.Orientation) =
    this.props |> Props.add (PKey.Bar.Orientation, value)


  // Events
  member this.OrientationChanged(handler: EventArgs<Terminal.Gui.ViewBase.Orientation> -> unit) =
    this.props |> Props.add (PKey.Bar.OrientationChanged, handler)

  member this.OrientationChanging(handler: CancelEventArgs<Terminal.Gui.ViewBase.Orientation> -> unit) =
    this.props |> Props.add (PKey.Bar.OrientationChanging, handler)

type BorderViewProps() =
  inherit AdornmentViewProps()
  // Properties
  member this.TabLength(value: Nullable<int>) =
    this.props |> Props.add (PKey.BorderView.TabLength, value)


  member this.TabOffset(value: int) =
    this.props |> Props.add (PKey.BorderView.TabOffset, value)


  member this.TabSide(value: Terminal.Gui.ViewBase.Side) =
    this.props |> Props.add (PKey.BorderView.TabSide, value)


type ButtonProps() =
  inherit ViewProps()
  // Properties
  member this.HotKeySpecifier(value: System.Text.Rune) =
    this.props |> Props.add (PKey.Button.HotKeySpecifier, value)


  member this.IsDefault(value: bool) =
    this.props |> Props.add (PKey.Button.IsDefault, value)


  member this.NoDecorations(value: bool) =
    this.props |> Props.add (PKey.Button.NoDecorations, value)


  member this.NoPadding(value: bool) =
    this.props |> Props.add (PKey.Button.NoPadding, value)


  member this.Text(value: string) =
    this.props |> Props.add (PKey.Button.Text, value)


  // Events
  member this.InitializingShadowStyle
    (handler: ValueChangingEventArgs<Nullable<Terminal.Gui.ViewBase.ShadowStyles>> -> unit)
    =
    this.props |> Props.add (PKey.Button.InitializingShadowStyle, handler)

type CharMapProps() =
  inherit ViewProps()
  // Properties
  member this.SelectedCodePoint(value: int) =
    this.props |> Props.add (PKey.CharMap.SelectedCodePoint, value)


  member this.ShowGlyphWidths(value: bool) =
    this.props |> Props.add (PKey.CharMap.ShowGlyphWidths, value)


  member this.ShowUnicodeCategory(value: Nullable<System.Globalization.UnicodeCategory>) =
    this.props |> Props.add (PKey.CharMap.ShowUnicodeCategory, value)


  member this.StartCodePoint(value: int) =
    this.props |> Props.add (PKey.CharMap.StartCodePoint, value)


  member this.Value(value: System.Text.Rune) =
    this.props |> Props.add (PKey.CharMap.Value, value)


  // Events
  member this.ValueChanged(handler: ValueChangedEventArgs<System.Text.Rune> -> unit) =
    this.props |> Props.add (PKey.CharMap.ValueChanged, handler)

  member this.ValueChangedUntyped(handler: ValueChangedEventArgs<System.Object> -> unit) =
    this.props |> Props.add (PKey.CharMap.ValueChangedUntyped, handler)

  member this.ValueChanging(handler: ValueChangingEventArgs<System.Text.Rune> -> unit) =
    this.props |> Props.add (PKey.CharMap.ValueChanging, handler)

type CheckBoxProps() =
  inherit ViewProps()
  // Properties
  member this.AllowCheckStateNone(value: bool) =
    this.props |> Props.add (PKey.CheckBox.AllowCheckStateNone, value)


  member this.HotKeySpecifier(value: System.Text.Rune) =
    this.props |> Props.add (PKey.CheckBox.HotKeySpecifier, value)


  member this.RadioStyle(value: bool) =
    this.props |> Props.add (PKey.CheckBox.RadioStyle, value)


  member this.Text(value: string) =
    this.props |> Props.add (PKey.CheckBox.Text, value)


  member this.Value(value: Terminal.Gui.Views.CheckState) =
    this.props |> Props.add (PKey.CheckBox.Value, value)


  // Events
  member this.ValueChanged(handler: ValueChangedEventArgs<Terminal.Gui.Views.CheckState> -> unit) =
    this.props |> Props.add (PKey.CheckBox.ValueChanged, handler)

  member this.ValueChangedUntyped(handler: ValueChangedEventArgs<System.Object> -> unit) =
    this.props |> Props.add (PKey.CheckBox.ValueChangedUntyped, handler)

  member this.ValueChanging(handler: ValueChangingEventArgs<Terminal.Gui.Views.CheckState> -> unit) =
    this.props |> Props.add (PKey.CheckBox.ValueChanging, handler)

type CodeProps() =
  inherit ViewProps()
  // Properties
  member this.Language(value: string) =
    this.props |> Props.add (PKey.Code.Language, value)


  member this.SyntaxHighlighter(value: Terminal.Gui.Drawing.ISyntaxHighlighter) =
    this.props |> Props.add (PKey.Code.SyntaxHighlighter, value)


  member this.Text(value: string) =
    this.props |> Props.add (PKey.Code.Text, value)


type ColorPickerProps() =
  inherit ViewProps()
  // Properties
  member this.SelectedColor(value: Terminal.Gui.Drawing.Color) =
    this.props |> Props.add (PKey.ColorPicker.SelectedColor, value)


  member this.Style(value: Terminal.Gui.Views.ColorPickerStyle) =
    this.props |> Props.add (PKey.ColorPicker.Style, value)


  member this.Text(value: string) =
    this.props |> Props.add (PKey.ColorPicker.Text, value)


  member this.Value(value: Nullable<Terminal.Gui.Drawing.Color>) =
    this.props |> Props.add (PKey.ColorPicker.Value, value)


  // Events
  member this.ValueChanged(handler: ValueChangedEventArgs<Nullable<Terminal.Gui.Drawing.Color>> -> unit) =
    this.props |> Props.add (PKey.ColorPicker.ValueChanged, handler)

  member this.ValueChangedUntyped(handler: ValueChangedEventArgs<System.Object> -> unit) =
    this.props |> Props.add (PKey.ColorPicker.ValueChangedUntyped, handler)

  member this.ValueChanging(handler: ValueChangingEventArgs<Nullable<Terminal.Gui.Drawing.Color>> -> unit) =
    this.props |> Props.add (PKey.ColorPicker.ValueChanging, handler)

type ColorPicker16Props() =
  inherit ViewProps()
  // Properties
  member this.BoxHeight(value: int) =
    this.props |> Props.add (PKey.ColorPicker16.BoxHeight, value)


  member this.BoxWidth(value: int) =
    this.props |> Props.add (PKey.ColorPicker16.BoxWidth, value)


  member this.Caret(value: System.Drawing.Point) =
    this.props |> Props.add (PKey.ColorPicker16.Caret, value)


  member this.SelectedColor(value: Terminal.Gui.Drawing.ColorName16) =
    this.props |> Props.add (PKey.ColorPicker16.SelectedColor, value)


  member this.Value(value: Terminal.Gui.Drawing.ColorName16) =
    this.props |> Props.add (PKey.ColorPicker16.Value, value)


  // Events
  member this.ValueChanged(handler: ValueChangedEventArgs<Terminal.Gui.Drawing.ColorName16> -> unit) =
    this.props |> Props.add (PKey.ColorPicker16.ValueChanged, handler)

  member this.ValueChangedUntyped(handler: ValueChangedEventArgs<System.Object> -> unit) =
    this.props |> Props.add (PKey.ColorPicker16.ValueChangedUntyped, handler)

  member this.ValueChanging(handler: ValueChangingEventArgs<Terminal.Gui.Drawing.ColorName16> -> unit) =
    this.props |> Props.add (PKey.ColorPicker16.ValueChanging, handler)

type DatePickerProps() =
  inherit ViewProps()
  // Properties
  member this.Culture(value: System.Globalization.CultureInfo) =
    this.props |> Props.add (PKey.DatePicker.Culture, value)


  member this.Text(value: string) =
    this.props |> Props.add (PKey.DatePicker.Text, value)


  member this.Value(value: System.DateTime) =
    this.props |> Props.add (PKey.DatePicker.Value, value)


  // Events
  member this.ValueChanged(handler: ValueChangedEventArgs<System.DateTime> -> unit) =
    this.props |> Props.add (PKey.DatePicker.ValueChanged, handler)

  member this.ValueChangedUntyped(handler: ValueChangedEventArgs<System.Object> -> unit) =
    this.props |> Props.add (PKey.DatePicker.ValueChangedUntyped, handler)

  member this.ValueChanging(handler: ValueChangingEventArgs<System.DateTime> -> unit) =
    this.props |> Props.add (PKey.DatePicker.ValueChanging, handler)

type FrameViewProps() =
  inherit ViewProps()

type GraphViewProps() =
  inherit ViewProps()
  // Properties
  member this.AxisX(value: Terminal.Gui.Views.HorizontalAxis) =
    this.props |> Props.add (PKey.GraphView.AxisX, value)


  member this.AxisY(value: Terminal.Gui.Views.VerticalAxis) =
    this.props |> Props.add (PKey.GraphView.AxisY, value)


  member this.CellSize(value: System.Drawing.PointF) =
    this.props |> Props.add (PKey.GraphView.CellSize, value)


  member this.GraphColor(value: Nullable<Terminal.Gui.Drawing.Attribute>) =
    this.props |> Props.add (PKey.GraphView.GraphColor, value)


  member this.MarginBottom(value: System.UInt32) =
    this.props |> Props.add (PKey.GraphView.MarginBottom, value)


  member this.MarginLeft(value: System.UInt32) =
    this.props |> Props.add (PKey.GraphView.MarginLeft, value)


  member this.ScrollOffset(value: System.Drawing.PointF) =
    this.props |> Props.add (PKey.GraphView.ScrollOffset, value)


type HexViewProps() =
  inherit ViewProps()
  // Properties
  member this.Address(value: System.Int64) =
    this.props |> Props.add (PKey.HexView.Address, value)


  member this.AddressWidth(value: int) =
    this.props |> Props.add (PKey.HexView.AddressWidth, value)


  member this.BytesPerLine(value: int) =
    this.props |> Props.add (PKey.HexView.BytesPerLine, value)


  member this.ReadOnly(value: bool) =
    this.props |> Props.add (PKey.HexView.ReadOnly, value)


  member this.Source(value: System.IO.Stream) =
    this.props |> Props.add (PKey.HexView.Source, value)


  // Events
  member this.Edited(handler: Terminal.Gui.Views.HexViewEditEventArgs -> unit) =
    this.props |> Props.add (PKey.HexView.Edited, handler)

  member this.PositionChanged(handler: Terminal.Gui.Views.HexViewEventArgs -> unit) =
    this.props |> Props.add (PKey.HexView.PositionChanged, handler)

type ImageViewProps() =
  inherit ViewProps()
  // Properties
  member this.AllowSixelUpscaling(value: bool) =
    this.props |> Props.add (PKey.ImageView.AllowSixelUpscaling, value)


  member this.Image(value: Terminal.Gui.Drawing.Color[,]) =
    this.props |> Props.add (PKey.ImageView.Image, value)


  member this.MaxSixelPaletteColors(value: int) =
    this.props |> Props.add (PKey.ImageView.MaxSixelPaletteColors, value)


  member this.SixelEncoder(value: Terminal.Gui.Drawing.SixelEncoder) =
    this.props |> Props.add (PKey.ImageView.SixelEncoder, value)


  member this.UseBackgroundRendering(value: bool) =
    this.props |> Props.add (PKey.ImageView.UseBackgroundRendering, value)


  member this.UseRasterGraphics(value: bool) =
    this.props |> Props.add (PKey.ImageView.UseRasterGraphics, value)


  member this.UseSixel(value: bool) =
    this.props |> Props.add (PKey.ImageView.UseSixel, value)


  member this.ZoomLevel(value: System.Double) =
    this.props |> Props.add (PKey.ImageView.ZoomLevel, value)


  // Events
  member this.ZoomLevelChanged(handler: System.EventArgs -> unit) =
    this.props |> Props.add (PKey.ImageView.ZoomLevelChanged, handler)

type LabelProps() =
  inherit ViewProps()
  // Properties
  member this.HotKeySpecifier(value: System.Text.Rune) =
    this.props |> Props.add (PKey.Label.HotKeySpecifier, value)


  member this.Text(value: string) =
    this.props |> Props.add (PKey.Label.Text, value)


type LegendAnnotationProps() =
  inherit ViewProps()

type LineProps() =
  inherit ViewProps()
  // Properties
  member this.Length(value: Terminal.Gui.ViewBase.Dim) =
    this.props |> Props.add (PKey.Line.Length, value)


  member this.LineAttribute(value: Nullable<Terminal.Gui.Drawing.Attribute>) =
    this.props |> Props.add (PKey.Line.LineAttribute, value)


  member this.Orientation(value: Terminal.Gui.ViewBase.Orientation) =
    this.props |> Props.add (PKey.Line.Orientation, value)


  member this.Style(value: Terminal.Gui.Drawing.LineStyle) =
    this.props |> Props.add (PKey.Line.Style, value)


  // Events
  member this.OrientationChanged(handler: EventArgs<Terminal.Gui.ViewBase.Orientation> -> unit) =
    this.props |> Props.add (PKey.Line.OrientationChanged, handler)

  member this.OrientationChanging(handler: CancelEventArgs<Terminal.Gui.ViewBase.Orientation> -> unit) =
    this.props |> Props.add (PKey.Line.OrientationChanging, handler)

type LinearRangeViewBaseProps<'TOption, 'TValue>() =
  inherit ViewProps()
  // Properties
  member this.AllowEmpty(value: bool) =
    this.props
    |> Props.add (PKey.LinearRangeViewBase<'TOption, 'TValue>.AllowEmpty, value)


  member this.FocusedOption(value: int) =
    this.props
    |> Props.add (PKey.LinearRangeViewBase<'TOption, 'TValue>.FocusedOption, value)


  member this.LegendsOrientation(value: Terminal.Gui.ViewBase.Orientation) =
    this.props
    |> Props.add (PKey.LinearRangeViewBase<'TOption, 'TValue>.LegendsOrientation, value)


  member this.MinimumInnerSpacing(value: int) =
    this.props
    |> Props.add (PKey.LinearRangeViewBase<'TOption, 'TValue>.MinimumInnerSpacing, value)


  member this.Options(value: List<LinearRangeOption<'TOption>>) =
    this.props
    |> Props.add (PKey.LinearRangeViewBase<'TOption, 'TValue>.Options, value)


  member this.Orientation(value: Terminal.Gui.ViewBase.Orientation) =
    this.props
    |> Props.add (PKey.LinearRangeViewBase<'TOption, 'TValue>.Orientation, value)


  member this.ShowEndSpacing(value: bool) =
    this.props
    |> Props.add (PKey.LinearRangeViewBase<'TOption, 'TValue>.ShowEndSpacing, value)


  member this.ShowLegends(value: bool) =
    this.props
    |> Props.add (PKey.LinearRangeViewBase<'TOption, 'TValue>.ShowLegends, value)


  member this.Style(value: Terminal.Gui.Views.LinearRangeStyle) =
    this.props
    |> Props.add (PKey.LinearRangeViewBase<'TOption, 'TValue>.Style, value)


  member this.Text(value: string) =
    this.props
    |> Props.add (PKey.LinearRangeViewBase<'TOption, 'TValue>.Text, value)


  member this.UseMinimumSize(value: bool) =
    this.props
    |> Props.add (PKey.LinearRangeViewBase<'TOption, 'TValue>.UseMinimumSize, value)


  member this.Value(value: 'TValue) =
    this.props
    |> Props.add (PKey.LinearRangeViewBase<'TOption, 'TValue>.Value, value)


  // Events
  member this.LegendsOrientationChanged(handler: ValueChangedEventArgs<Terminal.Gui.ViewBase.Orientation> -> unit) =
    this.props
    |> Props.add (PKey.LinearRangeViewBase<'TOption, 'TValue>.LegendsOrientationChanged, handler)

  member this.LegendsOrientationChanging(handler: ValueChangingEventArgs<Terminal.Gui.ViewBase.Orientation> -> unit) =
    this.props
    |> Props.add (PKey.LinearRangeViewBase<'TOption, 'TValue>.LegendsOrientationChanging, handler)

  member this.MinimumInnerSpacingChanged(handler: ValueChangedEventArgs<int> -> unit) =
    this.props
    |> Props.add (PKey.LinearRangeViewBase<'TOption, 'TValue>.MinimumInnerSpacingChanged, handler)

  member this.MinimumInnerSpacingChanging(handler: ValueChangingEventArgs<int> -> unit) =
    this.props
    |> Props.add (PKey.LinearRangeViewBase<'TOption, 'TValue>.MinimumInnerSpacingChanging, handler)

  member this.OptionFocused(handler: LinearRangeEventArgs<'TOption> -> unit) =
    this.props
    |> Props.add (PKey.LinearRangeViewBase<'TOption, 'TValue>.OptionFocused, handler)

  member this.OrientationChanged(handler: EventArgs<Terminal.Gui.ViewBase.Orientation> -> unit) =
    this.props
    |> Props.add (PKey.LinearRangeViewBase<'TOption, 'TValue>.OrientationChanged, handler)

  member this.OrientationChanging(handler: CancelEventArgs<Terminal.Gui.ViewBase.Orientation> -> unit) =
    this.props
    |> Props.add (PKey.LinearRangeViewBase<'TOption, 'TValue>.OrientationChanging, handler)

  member this.ShowEndSpacingChanged(handler: ValueChangedEventArgs<bool> -> unit) =
    this.props
    |> Props.add (PKey.LinearRangeViewBase<'TOption, 'TValue>.ShowEndSpacingChanged, handler)

  member this.ShowEndSpacingChanging(handler: ValueChangingEventArgs<bool> -> unit) =
    this.props
    |> Props.add (PKey.LinearRangeViewBase<'TOption, 'TValue>.ShowEndSpacingChanging, handler)

  member this.ShowLegendsChanged(handler: ValueChangedEventArgs<bool> -> unit) =
    this.props
    |> Props.add (PKey.LinearRangeViewBase<'TOption, 'TValue>.ShowLegendsChanged, handler)

  member this.ShowLegendsChanging(handler: ValueChangingEventArgs<bool> -> unit) =
    this.props
    |> Props.add (PKey.LinearRangeViewBase<'TOption, 'TValue>.ShowLegendsChanging, handler)

  member this.UseMinimumSizeChanged(handler: ValueChangedEventArgs<bool> -> unit) =
    this.props
    |> Props.add (PKey.LinearRangeViewBase<'TOption, 'TValue>.UseMinimumSizeChanged, handler)

  member this.UseMinimumSizeChanging(handler: ValueChangingEventArgs<bool> -> unit) =
    this.props
    |> Props.add (PKey.LinearRangeViewBase<'TOption, 'TValue>.UseMinimumSizeChanging, handler)

  member this.ValueChanged(handler: ValueChangedEventArgs<'TValue> -> unit) =
    this.props
    |> Props.add (PKey.LinearRangeViewBase<'TOption, 'TValue>.ValueChanged, handler)

  member this.ValueChangedUntyped(handler: ValueChangedEventArgs<System.Object> -> unit) =
    this.props
    |> Props.add (PKey.LinearRangeViewBase<'TOption, 'TValue>.ValueChangedUntyped, handler)

  member this.ValueChanging(handler: ValueChangingEventArgs<'TValue> -> unit) =
    this.props
    |> Props.add (PKey.LinearRangeViewBase<'TOption, 'TValue>.ValueChanging, handler)

type LinearMultiSelectorProps<'T>() =
  inherit LinearRangeViewBaseProps<'T, IReadOnlyList<'T>>()
  // Properties
  member this.Value(value: IReadOnlyList<'T>) =
    this.props |> Props.add (PKey.LinearMultiSelector<'T>.Value, value)


type LinearMultiSelectorProps() =
  inherit LinearMultiSelectorProps<string>()

type LinearRangeProps<'T>() =
  inherit LinearRangeViewBaseProps<'T, LinearRangeSpan<'T>>()
  // Properties
  member this.RangeAllowSingle(value: bool) =
    this.props |> Props.add (PKey.LinearRange<'T>.RangeAllowSingle, value)


  member this.RangeKind(value: Terminal.Gui.Views.LinearRangeSpanKind) =
    this.props |> Props.add (PKey.LinearRange<'T>.RangeKind, value)


  member this.Value(value: LinearRangeSpan<'T>) =
    this.props |> Props.add (PKey.LinearRange<'T>.Value, value)


type LinearRangeProps() =
  inherit LinearRangeProps<string>()

type LinearSelectorProps<'T>() =
  inherit LinearRangeViewBaseProps<'T, 'T>()
  // Properties
  member this.SelectedIndex(value: Nullable<int>) =
    this.props |> Props.add (PKey.LinearSelector<'T>.SelectedIndex, value)


  member this.Value(value: 'T) =
    this.props |> Props.add (PKey.LinearSelector<'T>.Value, value)


type LinearSelectorProps() =
  inherit LinearSelectorProps<string>()

type LinkProps() =
  inherit ViewProps()
  // Properties
  member this.Url(value: string) =
    this.props |> Props.add (PKey.Link.Url, value)


  // Events
  member this.UrlChanged(handler: ValueChangedEventArgs<string> -> unit) =
    this.props |> Props.add (PKey.Link.UrlChanged, handler)

  member this.UrlChanging(handler: ValueChangingEventArgs<string> -> unit) =
    this.props |> Props.add (PKey.Link.UrlChanging, handler)

type ListViewProps() =
  inherit ViewProps()
  // Properties
  member this.KeystrokeNavigator(value: Terminal.Gui.Views.IListCollectionNavigator) =
    this.props |> Props.add (PKey.ListView.KeystrokeNavigator, value)


  member this.MarkMultiple(value: bool) =
    this.props |> Props.add (PKey.ListView.MarkMultiple, value)


  member this.SelectedItem(value: Nullable<int>) =
    this.props |> Props.add (PKey.ListView.SelectedItem, value)


  member this.ShowMarks(value: bool) =
    this.props |> Props.add (PKey.ListView.ShowMarks, value)


  member this.Source(value: Terminal.Gui.Views.IListDataSource) =
    this.props |> Props.add (PKey.ListView.Source, value)


  member this.Value(value: Nullable<int>) =
    this.props |> Props.add (PKey.ListView.Value, value)


  // Events
  member this.CollectionChanged(handler: System.Collections.Specialized.NotifyCollectionChangedEventArgs -> unit) =
    this.props |> Props.add (PKey.ListView.CollectionChanged, handler)

  member this.RowRender(handler: Terminal.Gui.Views.ListViewRowEventArgs -> unit) =
    this.props |> Props.add (PKey.ListView.RowRender, handler)

  member this.SourceChanged(handler: System.EventArgs -> unit) =
    this.props |> Props.add (PKey.ListView.SourceChanged, handler)

  member this.ValueChanged(handler: ValueChangedEventArgs<Nullable<int>> -> unit) =
    this.props |> Props.add (PKey.ListView.ValueChanged, handler)

  member this.ValueChangedUntyped(handler: ValueChangedEventArgs<System.Object> -> unit) =
    this.props |> Props.add (PKey.ListView.ValueChangedUntyped, handler)

  member this.ValueChanging(handler: ValueChangingEventArgs<Nullable<int>> -> unit) =
    this.props |> Props.add (PKey.ListView.ValueChanging, handler)

type ListViewProps<'T>() =
  inherit ListViewProps()
  // Properties
  member this.Index(value: Nullable<int>) =
    this.props |> Props.add (PKey.ListView'<'T>.Index, value)


  member this.SelectedItem(value: 'T) =
    this.props |> Props.add (PKey.ListView'<'T>.SelectedItem, value)


  member this.Value(value: 'T) =
    this.props |> Props.add (PKey.ListView'<'T>.Value, value)


  // Events
  member this.ValueChanged(handler: ValueChangedEventArgs<'T> -> unit) =
    this.props |> Props.add (PKey.ListView'<'T>.ValueChanged, handler)

  member this.ValueChangedUntyped(handler: ValueChangedEventArgs<System.Object> -> unit) =
    this.props |> Props.add (PKey.ListView'<'T>.ValueChangedUntyped, handler)

  member this.ValueChanging(handler: ValueChangingEventArgs<'T> -> unit) =
    this.props |> Props.add (PKey.ListView'<'T>.ValueChanging, handler)

type MarginViewProps() =
  inherit AdornmentViewProps()
  // Properties
  member this.ShadowSize(value: System.Drawing.Size) =
    this.props |> Props.add (PKey.MarginView.ShadowSize, value)


  member this.ShadowStyle(value: Nullable<Terminal.Gui.ViewBase.ShadowStyles>) =
    this.props |> Props.add (PKey.MarginView.ShadowStyle, value)


type MarkdownProps() =
  inherit ViewProps()
  // Properties
  member this.EnableSixelImages(value: bool) =
    this.props |> Props.add (PKey.Markdown.EnableSixelImages, value)


  member this.HotKeySpecifier(value: System.Text.Rune) =
    this.props |> Props.add (PKey.Markdown.HotKeySpecifier, value)


  member this.ImageLoader(value: Func<string, System.Byte[]>) =
    this.props |> Props.add (PKey.Markdown.ImageLoader, value)


  member this.MarkdownPipeline(value: Markdig.MarkdownPipeline) =
    this.props |> Props.add (PKey.Markdown.MarkdownPipeline, value)


  member this.ShowCopyButtons(value: bool) =
    this.props |> Props.add (PKey.Markdown.ShowCopyButtons, value)


  member this.ShowHeadingPrefix(value: bool) =
    this.props |> Props.add (PKey.Markdown.ShowHeadingPrefix, value)


  member this.SyntaxHighlighter(value: Terminal.Gui.Drawing.ISyntaxHighlighter) =
    this.props |> Props.add (PKey.Markdown.SyntaxHighlighter, value)


  member this.Text(value: string) =
    this.props |> Props.add (PKey.Markdown.Text, value)


  member this.UseThemeBackground(value: bool) =
    this.props |> Props.add (PKey.Markdown.UseThemeBackground, value)


  // Events
  member this.LinkClicked(handler: Terminal.Gui.Views.MarkdownLinkEventArgs -> unit) =
    this.props |> Props.add (PKey.Markdown.LinkClicked, handler)

  member this.MarkdownChanged(handler: System.EventArgs -> unit) =
    this.props |> Props.add (PKey.Markdown.MarkdownChanged, handler)

type MarkdownCodeBlockProps() =
  inherit ViewProps()
  // Properties
  member this.CodeLines(value: IReadOnlyList<string>) =
    this.props |> Props.add (PKey.MarkdownCodeBlock.CodeLines, value)


  member this.Language(value: string) =
    this.props |> Props.add (PKey.MarkdownCodeBlock.Language, value)


  member this.ShowCopyButton(value: bool) =
    this.props |> Props.add (PKey.MarkdownCodeBlock.ShowCopyButton, value)


  member this.SyntaxHighlighter(value: Terminal.Gui.Drawing.ISyntaxHighlighter) =
    this.props |> Props.add (PKey.MarkdownCodeBlock.SyntaxHighlighter, value)


  member this.Text(value: string) =
    this.props |> Props.add (PKey.MarkdownCodeBlock.Text, value)


  member this.ThemeBackground(value: Nullable<Terminal.Gui.Drawing.Color>) =
    this.props |> Props.add (PKey.MarkdownCodeBlock.ThemeBackground, value)


type MarkdownTableProps() =
  inherit ViewProps()
  // Properties
  member this.SyntaxHighlighter(value: Terminal.Gui.Drawing.ISyntaxHighlighter) =
    this.props |> Props.add (PKey.MarkdownTable.SyntaxHighlighter, value)


  member this.TableData(value: Terminal.Gui.Views.TableData) =
    this.props |> Props.add (PKey.MarkdownTable.TableData, value)


  member this.Text(value: string) =
    this.props |> Props.add (PKey.MarkdownTable.Text, value)


  member this.UseThemeBackground(value: bool) =
    this.props |> Props.add (PKey.MarkdownTable.UseThemeBackground, value)


  // Events
  member this.LinkClicked(handler: Terminal.Gui.Views.MarkdownLinkEventArgs -> unit) =
    this.props |> Props.add (PKey.MarkdownTable.LinkClicked, handler)

type MenuProps() =
  inherit BarProps()
  // Properties
  member this.SuperMenuItem(value: Terminal.Gui.Views.MenuItem) =
    this.props |> Props.add (PKey.Menu.SuperMenuItem, value)

  member this.SuperMenuItem(value: IMenuItemView) =
    this.props |> Props.add (PKey.Menu.SuperMenuItem_viewSpec, value)

  member this.Value(value: Terminal.Gui.Views.MenuItem) =
    this.props |> Props.add (PKey.Menu.Value, value)

  member this.Value(value: IMenuItemView) =
    this.props |> Props.add (PKey.Menu.Value_viewSpec, value)

  // Events
  member this.SelectedMenuItemChanged(handler: Terminal.Gui.Views.MenuItem -> unit) =
    this.props |> Props.add (PKey.Menu.SelectedMenuItemChanged, handler)

  member this.ValueChanged(handler: ValueChangedEventArgs<Terminal.Gui.Views.MenuItem> -> unit) =
    this.props |> Props.add (PKey.Menu.ValueChanged, handler)

  member this.ValueChangedUntyped(handler: ValueChangedEventArgs<System.Object> -> unit) =
    this.props |> Props.add (PKey.Menu.ValueChangedUntyped, handler)

  member this.ValueChanging(handler: ValueChangingEventArgs<Terminal.Gui.Views.MenuItem> -> unit) =
    this.props |> Props.add (PKey.Menu.ValueChanging, handler)

type MenuBarProps() =
  inherit MenuProps()
  // Properties
  member this.Key(value: Terminal.Gui.Input.Key) =
    this.props |> Props.add (PKey.MenuBar.Key, value)


  // Events
  member this.KeyChanged(handler: Terminal.Gui.Input.KeyChangedEventArgs -> unit) =
    this.props |> Props.add (PKey.MenuBar.KeyChanged, handler)

type NumericUpDownProps<'T>() =
  inherit ViewProps()
  // Properties
  member this.Format(value: string) =
    this.props |> Props.add (PKey.NumericUpDown<'T>.Format, value)


  member this.Increment(value: 'T) =
    this.props |> Props.add (PKey.NumericUpDown<'T>.Increment, value)


  member this.Value(value: 'T) =
    this.props |> Props.add (PKey.NumericUpDown<'T>.Value, value)


  // Events
  member this.FormatChanged(handler: EventArgs<string> -> unit) =
    this.props |> Props.add (PKey.NumericUpDown<'T>.FormatChanged, handler)

  member this.IncrementChanged(handler: EventArgs<'T> -> unit) =
    this.props |> Props.add (PKey.NumericUpDown<'T>.IncrementChanged, handler)

  member this.ValueChanged(handler: ValueChangedEventArgs<'T> -> unit) =
    this.props |> Props.add (PKey.NumericUpDown<'T>.ValueChanged, handler)

  member this.ValueChangedUntyped(handler: ValueChangedEventArgs<System.Object> -> unit) =
    this.props |> Props.add (PKey.NumericUpDown<'T>.ValueChangedUntyped, handler)

  member this.ValueChanging(handler: ValueChangingEventArgs<'T> -> unit) =
    this.props |> Props.add (PKey.NumericUpDown<'T>.ValueChanging, handler)

type NumericUpDownProps() =
  inherit NumericUpDownProps<int>()

type PaddingViewProps() =
  inherit AdornmentViewProps()

type PopoverImplProps() =
  inherit ViewProps()
  // Properties
  member this.Anchor(value: Func<Nullable<System.Drawing.Rectangle>>) =
    this.props |> Props.add (PKey.PopoverImpl.Anchor, value)


  member this.Owner(value: Terminal.Gui.App.IRunnable) =
    this.props |> Props.add (PKey.PopoverImpl.Owner, value)


  member this.Target(value: WeakReference<Terminal.Gui.ViewBase.View>) =
    this.props |> Props.add (PKey.PopoverImpl.Target, value)


type PopoverProps<'TView, 'TResult when 'TView: (new: unit -> 'TView) and 'TView :> Terminal.Gui.ViewBase.View>() =
  inherit PopoverImplProps()
  // Properties
  member this.ContentView(value: 'TView) =
    this.props |> Props.add (PKey.Popover<'TView, 'TResult>.ContentView, value)

  member this.ContentView(value: ITViewView) =
    this.props
    |> Props.add (PKey.Popover<'TView, 'TResult>.ContentView_viewSpec, value)

  member this.ResultExtractor(value: Func<'TView, 'TResult>) =
    this.props |> Props.add (PKey.Popover<'TView, 'TResult>.ResultExtractor, value)


  // Events
  member this.ResultChanged(handler: ValueChangedEventArgs<'TResult> -> unit) =
    this.props |> Props.add (PKey.Popover<'TView, 'TResult>.ResultChanged, handler)

type PopoverMenuProps() =
  inherit PopoverProps<Terminal.Gui.Views.Menu, Terminal.Gui.Views.MenuItem>()
  // Properties
  member this.Key(value: Terminal.Gui.Input.Key) =
    this.props |> Props.add (PKey.PopoverMenu.Key, value)


  member this.MouseFlags(value: Terminal.Gui.Input.MouseFlags) =
    this.props |> Props.add (PKey.PopoverMenu.MouseFlags, value)


  member this.Root(value: Terminal.Gui.Views.Menu) =
    this.props |> Props.add (PKey.PopoverMenu.Root, value)

  member this.Root(value: IMenuView) =
    this.props |> Props.add (PKey.PopoverMenu.Root_viewSpec, value)

  // Events
  member this.KeyChanged(handler: Terminal.Gui.Input.KeyChangedEventArgs -> unit) =
    this.props |> Props.add (PKey.PopoverMenu.KeyChanged, handler)

type ProgressBarProps() =
  inherit ViewProps()
  // Properties
  member this.BidirectionalMarquee(value: bool) =
    this.props |> Props.add (PKey.ProgressBar.BidirectionalMarquee, value)


  member this.Fraction(value: System.Single) =
    this.props |> Props.add (PKey.ProgressBar.Fraction, value)


  member this.ProgressBarFormat(value: Terminal.Gui.Views.ProgressBarFormat) =
    this.props |> Props.add (PKey.ProgressBar.ProgressBarFormat, value)


  member this.ProgressBarStyle(value: Terminal.Gui.Views.ProgressBarStyle) =
    this.props |> Props.add (PKey.ProgressBar.ProgressBarStyle, value)


  member this.SegmentCharacter(value: System.Text.Rune) =
    this.props |> Props.add (PKey.ProgressBar.SegmentCharacter, value)


  member this.SyncWithTerminal(value: bool) =
    this.props |> Props.add (PKey.ProgressBar.SyncWithTerminal, value)


  member this.Text(value: string) =
    this.props |> Props.add (PKey.ProgressBar.Text, value)


type RunnableProps() =
  inherit ViewProps()
  // Properties
  member this.Result(value: System.Object) =
    this.props |> Props.add (PKey.Runnable.Result, value)


  member this.StopRequested(value: bool) =
    this.props |> Props.add (PKey.Runnable.StopRequested, value)


  // Events
  member this.IsModalChanged(handler: EventArgs<bool> -> unit) =
    this.props |> Props.add (PKey.Runnable.IsModalChanged, handler)

  member this.IsRunningChanged(handler: EventArgs<bool> -> unit) =
    this.props |> Props.add (PKey.Runnable.IsRunningChanged, handler)

  member this.IsRunningChanging(handler: CancelEventArgs<bool> -> unit) =
    this.props |> Props.add (PKey.Runnable.IsRunningChanging, handler)

type RunnableProps<'TResult>() =
  inherit RunnableProps()
  // Properties
  member this.Result(value: 'TResult) =
    this.props |> Props.add (PKey.Runnable'<'TResult>.Result, value)


type DialogProps<'TResult>() =
  inherit RunnableProps<'TResult>()
  // Properties
  member this.ButtonAlignment(value: Terminal.Gui.ViewBase.Alignment) =
    this.props |> Props.add (PKey.Dialog<'TResult>.ButtonAlignment, value)


  member this.ButtonAlignmentModes(value: Terminal.Gui.ViewBase.AlignmentModes) =
    this.props |> Props.add (PKey.Dialog<'TResult>.ButtonAlignmentModes, value)


  member this.Buttons(value: Terminal.Gui.Views.Button[]) =
    this.props |> Props.add (PKey.Dialog<'TResult>.Buttons, value)


type RunnableWrapperProps<'TView, 'TResult when 'TView: (new: unit -> 'TView) and 'TView :> Terminal.Gui.ViewBase.View>
  () =
  inherit RunnableProps<'TResult>()
  // Properties
  member this.ResultExtractor(value: Func<'TView, 'TResult>) =
    this.props
    |> Props.add (PKey.RunnableWrapper<'TView, 'TResult>.ResultExtractor, value)


type DialogProps() =
  inherit DialogProps<int>()
  // Properties
  member this.Result(value: Nullable<int>) =
    this.props |> Props.add (PKey.Dialog'.Result, value)


type FileDialogProps() =
  inherit DialogProps<IReadOnlyList<string>>()
  // Properties
  member this.AllowedTypes(value: List<Terminal.Gui.Views.IAllowedType>) =
    this.props |> Props.add (PKey.FileDialog.AllowedTypes, value)


  member this.AllowsMultipleSelection(value: bool) =
    this.props |> Props.add (PKey.FileDialog.AllowsMultipleSelection, value)


  member this.FileOperationsHandler(value: Terminal.Gui.FileServices.IFileOperations) =
    this.props |> Props.add (PKey.FileDialog.FileOperationsHandler, value)


  member this.MustExist(value: bool) =
    this.props |> Props.add (PKey.FileDialog.MustExist, value)


  member this.OpenMode(value: Terminal.Gui.Views.OpenMode) =
    this.props |> Props.add (PKey.FileDialog.OpenMode, value)


  member this.Path(value: string) =
    this.props |> Props.add (PKey.FileDialog.Path, value)


  member this.SearchMatcher(value: Terminal.Gui.FileServices.ISearchMatcher) =
    this.props |> Props.add (PKey.FileDialog.SearchMatcher, value)


  // Events
  member this.FilesSelected(handler: Terminal.Gui.Views.FilesSelectedEventArgs -> unit) =
    this.props |> Props.add (PKey.FileDialog.FilesSelected, handler)

type PromptProps<'TView, 'TResult when 'TView: (new: unit -> 'TView) and 'TView :> Terminal.Gui.ViewBase.View>() =
  inherit DialogProps<'TResult>()
  // Properties
  member this.ResultExtractor(value: Func<'TView, 'TResult>) =
    this.props |> Props.add (PKey.Prompt<'TView, 'TResult>.ResultExtractor, value)


type OpenDialogProps() =
  inherit FileDialogProps()
  // Properties
  member this.OpenMode(value: Terminal.Gui.Views.OpenMode) =
    this.props |> Props.add (PKey.OpenDialog.OpenMode, value)


type SaveDialogProps() =
  inherit FileDialogProps()

type ScrollBarProps() =
  inherit ViewProps()
  // Properties
  member this.Increment(value: int) =
    this.props |> Props.add (PKey.ScrollBar.Increment, value)


  member this.Orientation(value: Terminal.Gui.ViewBase.Orientation) =
    this.props |> Props.add (PKey.ScrollBar.Orientation, value)


  member this.ScrollableContentSize(value: int) =
    this.props |> Props.add (PKey.ScrollBar.ScrollableContentSize, value)


  member this.Value(value: int) =
    this.props |> Props.add (PKey.ScrollBar.Value, value)


  member this.VisibilityMode(value: Terminal.Gui.Views.ScrollBarVisibilityMode) =
    this.props |> Props.add (PKey.ScrollBar.VisibilityMode, value)


  member this.VisibleContentSize(value: int) =
    this.props |> Props.add (PKey.ScrollBar.VisibleContentSize, value)


  // Events
  member this.OrientationChanged(handler: EventArgs<Terminal.Gui.ViewBase.Orientation> -> unit) =
    this.props |> Props.add (PKey.ScrollBar.OrientationChanged, handler)

  member this.OrientationChanging(handler: CancelEventArgs<Terminal.Gui.ViewBase.Orientation> -> unit) =
    this.props |> Props.add (PKey.ScrollBar.OrientationChanging, handler)

  member this.ScrollableContentSizeChanged(handler: EventArgs<int> -> unit) =
    this.props |> Props.add (PKey.ScrollBar.ScrollableContentSizeChanged, handler)

  member this.Scrolled(handler: EventArgs<int> -> unit) =
    this.props |> Props.add (PKey.ScrollBar.Scrolled, handler)

  member this.SliderPositionChanged(handler: EventArgs<int> -> unit) =
    this.props |> Props.add (PKey.ScrollBar.SliderPositionChanged, handler)

  member this.ValueChanged(handler: ValueChangedEventArgs<int> -> unit) =
    this.props |> Props.add (PKey.ScrollBar.ValueChanged, handler)

  member this.ValueChangedUntyped(handler: ValueChangedEventArgs<System.Object> -> unit) =
    this.props |> Props.add (PKey.ScrollBar.ValueChangedUntyped, handler)

  member this.ValueChanging(handler: ValueChangingEventArgs<int> -> unit) =
    this.props |> Props.add (PKey.ScrollBar.ValueChanging, handler)

type ScrollButtonProps() =
  inherit ButtonProps()
  // Properties
  member this.Direction(value: Terminal.Gui.ViewBase.NavigationDirection) =
    this.props |> Props.add (PKey.ScrollButton.Direction, value)


  member this.Orientation(value: Terminal.Gui.ViewBase.Orientation) =
    this.props |> Props.add (PKey.ScrollButton.Orientation, value)


  // Events
  member this.OrientationChanged(handler: EventArgs<Terminal.Gui.ViewBase.Orientation> -> unit) =
    this.props |> Props.add (PKey.ScrollButton.OrientationChanged, handler)

  member this.OrientationChanging(handler: CancelEventArgs<Terminal.Gui.ViewBase.Orientation> -> unit) =
    this.props |> Props.add (PKey.ScrollButton.OrientationChanging, handler)

type ScrollSliderProps() =
  inherit ViewProps()
  // Properties
  member this.Orientation(value: Terminal.Gui.ViewBase.Orientation) =
    this.props |> Props.add (PKey.ScrollSlider.Orientation, value)


  member this.Position(value: int) =
    this.props |> Props.add (PKey.ScrollSlider.Position, value)


  member this.Size(value: int) =
    this.props |> Props.add (PKey.ScrollSlider.Size, value)


  member this.SliderPadding(value: int) =
    this.props |> Props.add (PKey.ScrollSlider.SliderPadding, value)


  member this.Value(value: int) =
    this.props |> Props.add (PKey.ScrollSlider.Value, value)


  member this.VisibleContentSize(value: int) =
    this.props |> Props.add (PKey.ScrollSlider.VisibleContentSize, value)


  // Events
  member this.OrientationChanged(handler: EventArgs<Terminal.Gui.ViewBase.Orientation> -> unit) =
    this.props |> Props.add (PKey.ScrollSlider.OrientationChanged, handler)

  member this.OrientationChanging(handler: CancelEventArgs<Terminal.Gui.ViewBase.Orientation> -> unit) =
    this.props |> Props.add (PKey.ScrollSlider.OrientationChanging, handler)

  member this.PositionChanged(handler: EventArgs<int> -> unit) =
    this.props |> Props.add (PKey.ScrollSlider.PositionChanged, handler)

  member this.PositionChanging(handler: CancelEventArgs<int> -> unit) =
    this.props |> Props.add (PKey.ScrollSlider.PositionChanging, handler)

  member this.Scrolled(handler: EventArgs<int> -> unit) =
    this.props |> Props.add (PKey.ScrollSlider.Scrolled, handler)

  member this.ValueChanged(handler: ValueChangedEventArgs<int> -> unit) =
    this.props |> Props.add (PKey.ScrollSlider.ValueChanged, handler)

  member this.ValueChangedUntyped(handler: ValueChangedEventArgs<System.Object> -> unit) =
    this.props |> Props.add (PKey.ScrollSlider.ValueChangedUntyped, handler)

  member this.ValueChanging(handler: ValueChangingEventArgs<int> -> unit) =
    this.props |> Props.add (PKey.ScrollSlider.ValueChanging, handler)

type SelectorBaseProps() =
  inherit ViewProps()
  // Properties
  member this.DoubleClickAccepts(value: bool) =
    this.props |> Props.add (PKey.SelectorBase.DoubleClickAccepts, value)


  member this.HorizontalSpace(value: int) =
    this.props |> Props.add (PKey.SelectorBase.HorizontalSpace, value)


  member this.Labels(value: IReadOnlyList<string>) =
    this.props |> Props.add (PKey.SelectorBase.Labels, value)


  member this.Orientation(value: Terminal.Gui.ViewBase.Orientation) =
    this.props |> Props.add (PKey.SelectorBase.Orientation, value)


  member this.Styles(value: Terminal.Gui.Views.SelectorStyles) =
    this.props |> Props.add (PKey.SelectorBase.Styles, value)


  member this.TabBehavior(value: Nullable<Terminal.Gui.ViewBase.TabBehavior>) =
    this.props |> Props.add (PKey.SelectorBase.TabBehavior, value)


  member this.Value(value: Nullable<int>) =
    this.props |> Props.add (PKey.SelectorBase.Value, value)


  member this.Values(value: IReadOnlyList<int>) =
    this.props |> Props.add (PKey.SelectorBase.Values, value)


  // Events
  member this.OrientationChanged(handler: EventArgs<Terminal.Gui.ViewBase.Orientation> -> unit) =
    this.props |> Props.add (PKey.SelectorBase.OrientationChanged, handler)

  member this.OrientationChanging(handler: CancelEventArgs<Terminal.Gui.ViewBase.Orientation> -> unit) =
    this.props |> Props.add (PKey.SelectorBase.OrientationChanging, handler)

  member this.ValueChanged(handler: ValueChangedEventArgs<Nullable<int>> -> unit) =
    this.props |> Props.add (PKey.SelectorBase.ValueChanged, handler)

  member this.ValueChangedUntyped(handler: ValueChangedEventArgs<System.Object> -> unit) =
    this.props |> Props.add (PKey.SelectorBase.ValueChangedUntyped, handler)

  member this.ValueChanging(handler: ValueChangingEventArgs<Nullable<int>> -> unit) =
    this.props |> Props.add (PKey.SelectorBase.ValueChanging, handler)

type FlagSelectorProps() =
  inherit SelectorBaseProps()
  // Properties
  member this.Value(value: Nullable<int>) =
    this.props |> Props.add (PKey.FlagSelector.Value, value)


type OptionSelectorProps() =
  inherit SelectorBaseProps()
  // Properties
  member this.FocusedItem(value: int) =
    this.props |> Props.add (PKey.OptionSelector.FocusedItem, value)


type FlagSelectorProps<'TFlagsEnum
  when 'TFlagsEnum: struct
  and 'TFlagsEnum: (new: unit -> 'TFlagsEnum)
  and 'TFlagsEnum :> System.Enum
  and 'TFlagsEnum :> System.ValueType>() =
  inherit FlagSelectorProps()
  // Properties
  member this.Value(value: Nullable<'TFlagsEnum>) =
    this.props |> Props.add (PKey.FlagSelector'<'TFlagsEnum>.Value, value)


  // Events
  member this.ValueChanged(handler: EventArgs<Nullable<'TFlagsEnum>> -> unit) =
    this.props |> Props.add (PKey.FlagSelector'<'TFlagsEnum>.ValueChanged, handler)

type OptionSelectorProps<'TEnum
  when 'TEnum: struct and 'TEnum: (new: unit -> 'TEnum) and 'TEnum :> System.Enum and 'TEnum :> System.ValueType>() =
  inherit OptionSelectorProps()
  // Properties
  member this.Value(value: Nullable<'TEnum>) =
    this.props |> Props.add (PKey.OptionSelector'<'TEnum>.Value, value)


  member this.Values(value: IReadOnlyList<int>) =
    this.props |> Props.add (PKey.OptionSelector'<'TEnum>.Values, value)


  // Events
  member this.ValueChanged(handler: EventArgs<Nullable<'TEnum>> -> unit) =
    this.props |> Props.add (PKey.OptionSelector'<'TEnum>.ValueChanged, handler)

type ShortcutProps() =
  inherit ViewProps()
  // Properties
  member this.Action(value: System.Action) =
    this.props |> Props.add (PKey.Shortcut.Action, value)


  member this.AlignmentModes(value: Terminal.Gui.ViewBase.AlignmentModes) =
    this.props |> Props.add (PKey.Shortcut.AlignmentModes, value)


  member this.BindKeyToApplication(value: bool) =
    this.props |> Props.add (PKey.Shortcut.BindKeyToApplication, value)


  member this.Command(value: Terminal.Gui.Input.Command) =
    this.props |> Props.add (PKey.Shortcut.Command, value)


  member this.CommandView(value: Terminal.Gui.ViewBase.View) =
    this.props |> Props.add (PKey.Shortcut.CommandView, value)

  member this.CommandView(value: IView) =
    this.props |> Props.add (PKey.Shortcut.CommandView_viewSpec, value)

  member this.HelpText(value: string) =
    this.props |> Props.add (PKey.Shortcut.HelpText, value)


  member this.Key(value: Terminal.Gui.Input.Key) =
    this.props |> Props.add (PKey.Shortcut.Key, value)


  member this.MinimumKeyTextSize(value: int) =
    this.props |> Props.add (PKey.Shortcut.MinimumKeyTextSize, value)


  member this.Orientation(value: Terminal.Gui.ViewBase.Orientation) =
    this.props |> Props.add (PKey.Shortcut.Orientation, value)


  member this.TargetView(value: Terminal.Gui.ViewBase.View) =
    this.props |> Props.add (PKey.Shortcut.TargetView, value)

  member this.TargetView(value: IView) =
    this.props |> Props.add (PKey.Shortcut.TargetView_viewSpec, value)

  member this.Text(value: string) =
    this.props |> Props.add (PKey.Shortcut.Text, value)


  // Events
  member this.OrientationChanged(handler: EventArgs<Terminal.Gui.ViewBase.Orientation> -> unit) =
    this.props |> Props.add (PKey.Shortcut.OrientationChanged, handler)

  member this.OrientationChanging(handler: CancelEventArgs<Terminal.Gui.ViewBase.Orientation> -> unit) =
    this.props |> Props.add (PKey.Shortcut.OrientationChanging, handler)

type MenuItemProps() =
  inherit ShortcutProps()
  // Properties
  member this.SubMenu(value: Terminal.Gui.Views.Menu) =
    this.props |> Props.add (PKey.MenuItem.SubMenu, value)

  member this.SubMenu(value: IMenuView) =
    this.props |> Props.add (PKey.MenuItem.SubMenu_viewSpec, value)

type MenuBarItemProps() =
  inherit MenuItemProps()
  // Properties
  member this.PopoverMenu(value: Terminal.Gui.Views.PopoverMenu) =
    this.props |> Props.add (PKey.MenuBarItem.PopoverMenu, value)

  member this.PopoverMenu(value: IPopoverMenuView) =
    this.props |> Props.add (PKey.MenuBarItem.PopoverMenu_viewSpec, value)

  member this.PopoverMenuOpen(value: bool) =
    this.props |> Props.add (PKey.MenuBarItem.PopoverMenuOpen, value)


  // Events
  member this.MenuOpenChanged(handler: ValueChangedEventArgs<bool> -> unit) =
    this.props |> Props.add (PKey.MenuBarItem.MenuOpenChanged, handler)

  member this.PopoverMenuOpenChanged(handler: ValueChangedEventArgs<bool> -> unit) =
    this.props |> Props.add (PKey.MenuBarItem.PopoverMenuOpenChanged, handler)

type SpinnerViewProps() =
  inherit ViewProps()
  // Properties
  member this.AutoSpin(value: bool) =
    this.props |> Props.add (PKey.SpinnerView.AutoSpin, value)


  member this.Sequence(value: System.String[]) =
    this.props |> Props.add (PKey.SpinnerView.Sequence, value)


  member this.SpinBounce(value: bool) =
    this.props |> Props.add (PKey.SpinnerView.SpinBounce, value)


  member this.SpinDelay(value: int) =
    this.props |> Props.add (PKey.SpinnerView.SpinDelay, value)


  member this.SpinReverse(value: bool) =
    this.props |> Props.add (PKey.SpinnerView.SpinReverse, value)


  member this.Style(value: Terminal.Gui.Views.SpinnerStyle) =
    this.props |> Props.add (PKey.SpinnerView.Style, value)


  member this.SyncWithTerminal(value: bool) =
    this.props |> Props.add (PKey.SpinnerView.SyncWithTerminal, value)


type StatusBarProps() =
  inherit BarProps()

type TableViewProps() =
  inherit ViewProps()
  // Properties
  member this.CollectionNavigator(value: Terminal.Gui.Views.ICollectionNavigator) =
    this.props |> Props.add (PKey.TableView.CollectionNavigator, value)


  member this.ColumnOffset(value: int) =
    this.props |> Props.add (PKey.TableView.ColumnOffset, value)


  member this.FullRowSelect(value: bool) =
    this.props |> Props.add (PKey.TableView.FullRowSelect, value)


  member this.MaxCellWidth(value: int) =
    this.props |> Props.add (PKey.TableView.MaxCellWidth, value)


  member this.MinCellWidth(value: int) =
    this.props |> Props.add (PKey.TableView.MinCellWidth, value)


  member this.MultiSelect(value: bool) =
    this.props |> Props.add (PKey.TableView.MultiSelect, value)


  member this.NullSymbol(value: string) =
    this.props |> Props.add (PKey.TableView.NullSymbol, value)


  member this.RowOffset(value: int) =
    this.props |> Props.add (PKey.TableView.RowOffset, value)


  member this.SeparatorSymbol(value: System.Char) =
    this.props |> Props.add (PKey.TableView.SeparatorSymbol, value)


  member this.Style(value: Terminal.Gui.Views.TableStyle) =
    this.props |> Props.add (PKey.TableView.Style, value)


  member this.Table(value: Terminal.Gui.Views.ITableSource) =
    this.props |> Props.add (PKey.TableView.Table, value)


  member this.UseAllRowsForContentCalculation(value: bool) =
    this.props |> Props.add (PKey.TableView.UseAllRowsForContentCalculation, value)


  member this.Value(value: Terminal.Gui.Views.TableSelection) =
    this.props |> Props.add (PKey.TableView.Value, value)


  // Events
  member this.ValueChanged(handler: ValueChangedEventArgs<Terminal.Gui.Views.TableSelection> -> unit) =
    this.props |> Props.add (PKey.TableView.ValueChanged, handler)

  member this.ValueChangedUntyped(handler: ValueChangedEventArgs<System.Object> -> unit) =
    this.props |> Props.add (PKey.TableView.ValueChangedUntyped, handler)

  member this.ValueChanging(handler: ValueChangingEventArgs<Terminal.Gui.Views.TableSelection> -> unit) =
    this.props |> Props.add (PKey.TableView.ValueChanging, handler)

type TabsProps() =
  inherit ViewProps()
  // Properties
  member this.ScrollOffset(value: int) =
    this.props |> Props.add (PKey.Tabs.ScrollOffset, value)


  member this.TabDepth(value: int) =
    this.props |> Props.add (PKey.Tabs.TabDepth, value)


  member this.TabLineStyle(value: Terminal.Gui.Drawing.LineStyle) =
    this.props |> Props.add (PKey.Tabs.TabLineStyle, value)


  member this.TabSide(value: Terminal.Gui.ViewBase.Side) =
    this.props |> Props.add (PKey.Tabs.TabSide, value)


  member this.TabSpacing(value: int) =
    this.props |> Props.add (PKey.Tabs.TabSpacing, value)


  member this.Value(value: Terminal.Gui.ViewBase.View) =
    this.props |> Props.add (PKey.Tabs.Value, value)

  member this.Value(value: IView) =
    this.props |> Props.add (PKey.Tabs.Value_viewSpec, value)

  // Events
  member this.ValueChanged(handler: ValueChangedEventArgs<Terminal.Gui.ViewBase.View> -> unit) =
    this.props |> Props.add (PKey.Tabs.ValueChanged, handler)

  member this.ValueChangedUntyped(handler: ValueChangedEventArgs<System.Object> -> unit) =
    this.props |> Props.add (PKey.Tabs.ValueChangedUntyped, handler)

  member this.ValueChanging(handler: ValueChangingEventArgs<Terminal.Gui.ViewBase.View> -> unit) =
    this.props |> Props.add (PKey.Tabs.ValueChanging, handler)

type TextFieldProps() =
  inherit ViewProps()
  // Properties
  member this.Autocomplete(value: Terminal.Gui.Views.IAutocomplete) =
    this.props |> Props.add (PKey.TextField.Autocomplete, value)


  member this.InsertionPoint(value: int) =
    this.props |> Props.add (PKey.TextField.InsertionPoint, value)


  member this.ReadOnly(value: bool) =
    this.props |> Props.add (PKey.TextField.ReadOnly, value)


  member this.Secret(value: bool) =
    this.props |> Props.add (PKey.TextField.Secret, value)


  member this.SelectWordOnlyOnDoubleClick(value: bool) =
    this.props |> Props.add (PKey.TextField.SelectWordOnlyOnDoubleClick, value)


  member this.SelectedStart(value: int) =
    this.props |> Props.add (PKey.TextField.SelectedStart, value)


  member this.Text(value: string) =
    this.props |> Props.add (PKey.TextField.Text, value)


  member this.UseSameRuneTypeForWords(value: bool) =
    this.props |> Props.add (PKey.TextField.UseSameRuneTypeForWords, value)


  member this.Used(value: bool) =
    this.props |> Props.add (PKey.TextField.Used, value)


  member this.Value(value: string) =
    this.props |> Props.add (PKey.TextField.Value, value)


  // Events
  member this.TextChanging(handler: ResultEventArgs<string> -> unit) =
    this.props |> Props.add (PKey.TextField.TextChanging, handler)

  member this.ValueChanged(handler: ValueChangedEventArgs<string> -> unit) =
    this.props |> Props.add (PKey.TextField.ValueChanged, handler)

  member this.ValueChangedUntyped(handler: ValueChangedEventArgs<System.Object> -> unit) =
    this.props |> Props.add (PKey.TextField.ValueChangedUntyped, handler)

  member this.ValueChanging(handler: ValueChangingEventArgs<string> -> unit) =
    this.props |> Props.add (PKey.TextField.ValueChanging, handler)

type DropDownListProps() =
  inherit TextFieldProps()
  // Properties
  member this.Source(value: Terminal.Gui.Views.IListDataSource) =
    this.props |> Props.add (PKey.DropDownList.Source, value)


type DropDownListProps<'TEnum
  when 'TEnum: struct and 'TEnum: (new: unit -> 'TEnum) and 'TEnum :> System.Enum and 'TEnum :> System.ValueType>() =
  inherit DropDownListProps()
  // Properties
  member this.Value(value: Nullable<'TEnum>) =
    this.props |> Props.add (PKey.DropDownList'<'TEnum>.Value, value)


  // Events
  member this.ValueChanged(handler: EventArgs<Nullable<'TEnum>> -> unit) =
    this.props |> Props.add (PKey.DropDownList'<'TEnum>.ValueChanged, handler)

type TextValidateFieldProps() =
  inherit ViewProps()
  // Properties
  member this.Provider(value: Terminal.Gui.Views.ITextValidateProvider) =
    this.props |> Props.add (PKey.TextValidateField.Provider, value)


  member this.Text(value: string) =
    this.props |> Props.add (PKey.TextValidateField.Text, value)


  member this.Value(value: string) =
    this.props |> Props.add (PKey.TextValidateField.Value, value)


  // Events
  member this.ValueChanged(handler: ValueChangedEventArgs<string> -> unit) =
    this.props |> Props.add (PKey.TextValidateField.ValueChanged, handler)

  member this.ValueChangedUntyped(handler: ValueChangedEventArgs<System.Object> -> unit) =
    this.props |> Props.add (PKey.TextValidateField.ValueChangedUntyped, handler)

  member this.ValueChanging(handler: ValueChangingEventArgs<string> -> unit) =
    this.props |> Props.add (PKey.TextValidateField.ValueChanging, handler)

type DateEditorProps() =
  inherit TextValidateFieldProps()
  // Properties
  member this.Format(value: System.Globalization.DateTimeFormatInfo) =
    this.props |> Props.add (PKey.DateEditor.Format, value)


  member this.Value(value: System.DateTime) =
    this.props |> Props.add (PKey.DateEditor.Value, value)


  // Events
  member this.ValueChanged(handler: ValueChangedEventArgs<System.DateTime> -> unit) =
    this.props |> Props.add (PKey.DateEditor.ValueChanged, handler)

  member this.ValueChangedUntyped(handler: ValueChangedEventArgs<System.Object> -> unit) =
    this.props |> Props.add (PKey.DateEditor.ValueChangedUntyped, handler)

  member this.ValueChanging(handler: ValueChangingEventArgs<System.DateTime> -> unit) =
    this.props |> Props.add (PKey.DateEditor.ValueChanging, handler)

type TextViewProps() =
  inherit ViewProps()
  // Properties
  member this.EnterKeyAddsLine(value: bool) =
    this.props |> Props.add (PKey.TextView.EnterKeyAddsLine, value)


  member this.InheritsPreviousAttribute(value: bool) =
    this.props |> Props.add (PKey.TextView.InheritsPreviousAttribute, value)


  member this.InsertionPoint(value: System.Drawing.Point) =
    this.props |> Props.add (PKey.TextView.InsertionPoint, value)


  member this.IsSelecting(value: bool) =
    this.props |> Props.add (PKey.TextView.IsSelecting, value)


  member this.Multiline(value: bool) =
    this.props |> Props.add (PKey.TextView.Multiline, value)


  member this.ReadOnly(value: bool) =
    this.props |> Props.add (PKey.TextView.ReadOnly, value)


  member this.ScrollBars(value: bool) =
    this.props |> Props.add (PKey.TextView.ScrollBars, value)


  member this.SelectWordOnlyOnDoubleClick(value: bool) =
    this.props |> Props.add (PKey.TextView.SelectWordOnlyOnDoubleClick, value)


  member this.SelectionStartColumn(value: int) =
    this.props |> Props.add (PKey.TextView.SelectionStartColumn, value)


  member this.SelectionStartRow(value: int) =
    this.props |> Props.add (PKey.TextView.SelectionStartRow, value)


  member this.TabKeyAddsTab(value: bool) =
    this.props |> Props.add (PKey.TextView.TabKeyAddsTab, value)


  member this.TabWidth(value: int) =
    this.props |> Props.add (PKey.TextView.TabWidth, value)


  member this.Text(value: string) =
    this.props |> Props.add (PKey.TextView.Text, value)


  member this.UseSameRuneTypeForWords(value: bool) =
    this.props |> Props.add (PKey.TextView.UseSameRuneTypeForWords, value)


  member this.Used(value: bool) =
    this.props |> Props.add (PKey.TextView.Used, value)


  member this.WordWrap(value: bool) =
    this.props |> Props.add (PKey.TextView.WordWrap, value)


  // Events
  member this.ContentsChanged(handler: Terminal.Gui.Views.ContentsChangedEventArgs -> unit) =
    this.props |> Props.add (PKey.TextView.ContentsChanged, handler)

  member this.DrawNormalColor(handler: Terminal.Gui.Drawing.CellEventArgs -> unit) =
    this.props |> Props.add (PKey.TextView.DrawNormalColor, handler)

  member this.DrawReadOnlyColor(handler: Terminal.Gui.Drawing.CellEventArgs -> unit) =
    this.props |> Props.add (PKey.TextView.DrawReadOnlyColor, handler)

  member this.DrawSelectionColor(handler: Terminal.Gui.Drawing.CellEventArgs -> unit) =
    this.props |> Props.add (PKey.TextView.DrawSelectionColor, handler)

  member this.DrawUsedColor(handler: Terminal.Gui.Drawing.CellEventArgs -> unit) =
    this.props |> Props.add (PKey.TextView.DrawUsedColor, handler)

  member this.UnwrappedCursorPositionChanged(handler: System.Drawing.Point -> unit) =
    this.props |> Props.add (PKey.TextView.UnwrappedCursorPositionChanged, handler)

type TimeEditorProps() =
  inherit TextValidateFieldProps()
  // Properties
  member this.Format(value: System.Globalization.DateTimeFormatInfo) =
    this.props |> Props.add (PKey.TimeEditor.Format, value)


  member this.Value(value: System.TimeSpan) =
    this.props |> Props.add (PKey.TimeEditor.Value, value)


  // Events
  member this.ValueChanged(handler: ValueChangedEventArgs<System.TimeSpan> -> unit) =
    this.props |> Props.add (PKey.TimeEditor.ValueChanged, handler)

  member this.ValueChangedUntyped(handler: ValueChangedEventArgs<System.Object> -> unit) =
    this.props |> Props.add (PKey.TimeEditor.ValueChangedUntyped, handler)

  member this.ValueChanging(handler: ValueChangingEventArgs<System.TimeSpan> -> unit) =
    this.props |> Props.add (PKey.TimeEditor.ValueChanging, handler)

type TitleViewProps() =
  inherit ViewProps()
  // Properties
  member this.Direction(value: Terminal.Gui.ViewBase.NavigationDirection) =
    this.props |> Props.add (PKey.TitleView.Direction, value)


  member this.MeasuredTabLength(value: int) =
    this.props |> Props.add (PKey.TitleView.MeasuredTabLength, value)


  member this.Orientation(value: Terminal.Gui.ViewBase.Orientation) =
    this.props |> Props.add (PKey.TitleView.Orientation, value)


  member this.TabDepth(value: int) =
    this.props |> Props.add (PKey.TitleView.TabDepth, value)


  member this.TabSide(value: Terminal.Gui.ViewBase.Side) =
    this.props |> Props.add (PKey.TitleView.TabSide, value)


  member this.Text(value: string) =
    this.props |> Props.add (PKey.TitleView.Text, value)


  // Events
  member this.OrientationChanged(handler: EventArgs<Terminal.Gui.ViewBase.Orientation> -> unit) =
    this.props |> Props.add (PKey.TitleView.OrientationChanged, handler)

  member this.OrientationChanging(handler: CancelEventArgs<Terminal.Gui.ViewBase.Orientation> -> unit) =
    this.props |> Props.add (PKey.TitleView.OrientationChanging, handler)

type ToolTipHostProps<'TView when 'TView: (new: unit -> 'TView) and 'TView :> Terminal.Gui.ViewBase.View>() =
  inherit PopoverImplProps()
  // Properties
  member this.ContentView(value: 'TView) =
    this.props |> Props.add (PKey.ToolTipHost<'TView>.ContentView, value)

  member this.ContentView(value: ITViewView) =
    this.props |> Props.add (PKey.ToolTipHost<'TView>.ContentView_viewSpec, value)

type TreeViewProps<'T when 'T: not struct>() =
  inherit ViewProps()
  // Properties
  member this.AllowLetterBasedNavigation(value: bool) =
    this.props |> Props.add (PKey.TreeView<'T>.AllowLetterBasedNavigation, value)


  member this.AspectGetter(value: AspectGetterDelegate<'T>) =
    this.props |> Props.add (PKey.TreeView<'T>.AspectGetter, value)


  member this.CheckboxMode(value: bool) =
    this.props |> Props.add (PKey.TreeView<'T>.CheckboxMode, value)


  member this.ColorGetter(value: Func<'T, Terminal.Gui.Drawing.Scheme>) =
    this.props |> Props.add (PKey.TreeView<'T>.ColorGetter, value)


  member this.Filter(value: ITreeViewFilter<'T>) =
    this.props |> Props.add (PKey.TreeView<'T>.Filter, value)


  member this.MaxDepth(value: int) =
    this.props |> Props.add (PKey.TreeView<'T>.MaxDepth, value)


  member this.MultiSelect(value: bool) =
    this.props |> Props.add (PKey.TreeView<'T>.MultiSelect, value)


  member this.ScrollOffsetHorizontal(value: int) =
    this.props |> Props.add (PKey.TreeView<'T>.ScrollOffsetHorizontal, value)


  member this.ScrollOffsetVertical(value: int) =
    this.props |> Props.add (PKey.TreeView<'T>.ScrollOffsetVertical, value)


  member this.SelectedObject(value: 'T) =
    this.props |> Props.add (PKey.TreeView<'T>.SelectedObject, value)


  member this.Style(value: Terminal.Gui.Views.TreeStyle) =
    this.props |> Props.add (PKey.TreeView<'T>.Style, value)


  member this.TreeBuilder(value: ITreeBuilder<'T>) =
    this.props |> Props.add (PKey.TreeView<'T>.TreeBuilder, value)


  // Events
  member this.CheckedChanged(handler: CheckedChangedEventArgs<'T> -> unit) =
    this.props |> Props.add (PKey.TreeView<'T>.CheckedChanged, handler)

  member this.DrawLine(handler: DrawTreeViewLineEventArgs<'T> -> unit) =
    this.props |> Props.add (PKey.TreeView<'T>.DrawLine, handler)

  member this.SelectionChanged(handler: SelectionChangedEventArgs<'T> -> unit) =
    this.props |> Props.add (PKey.TreeView<'T>.SelectionChanged, handler)

type TreeViewProps() =
  inherit TreeViewProps<Terminal.Gui.Views.ITreeNode>()

type WindowProps() =
  inherit RunnableProps()

type WizardProps() =
  inherit DialogProps()
  // Properties
  member this.CurrentStep(value: Terminal.Gui.Views.WizardStep) =
    this.props |> Props.add (PKey.Wizard.CurrentStep, value)

  member this.CurrentStep(value: IWizardStepView) =
    this.props |> Props.add (PKey.Wizard.CurrentStep_viewSpec, value)

  // Events
  member this.MovingBack(handler: System.ComponentModel.CancelEventArgs -> unit) =
    this.props |> Props.add (PKey.Wizard.MovingBack, handler)

  member this.MovingNext(handler: System.ComponentModel.CancelEventArgs -> unit) =
    this.props |> Props.add (PKey.Wizard.MovingNext, handler)

  member this.StepChanged(handler: ValueChangedEventArgs<Terminal.Gui.Views.WizardStep> -> unit) =
    this.props |> Props.add (PKey.Wizard.StepChanged, handler)

  member this.StepChanging(handler: ValueChangingEventArgs<Terminal.Gui.Views.WizardStep> -> unit) =
    this.props |> Props.add (PKey.Wizard.StepChanging, handler)

type WizardStepProps() =
  inherit ViewProps()
  // Properties
  member this.BackButtonText(value: string) =
    this.props |> Props.add (PKey.WizardStep.BackButtonText, value)


  member this.HelpText(value: string) =
    this.props |> Props.add (PKey.WizardStep.HelpText, value)


  member this.NextButtonText(value: string) =
    this.props |> Props.add (PKey.WizardStep.NextButtonText, value)
