namespace Terminal.Gui.Elmish

open System
open System.Collections.Generic
open Terminal.Gui.App
open Terminal.Gui.Views

[<RequireQualifiedAccess>]
module internal PKey =

  type ViewPKeys() =

    // Properties
    member val App: PropKey<Terminal.Gui.App.IApplication> = PropKey.Create.simple (PropertyId.Create(0), "View.App")

    member val Arrangement: PropKey<Terminal.Gui.ViewBase.ViewArrangement> =
      PropKey.Create.simple (PropertyId.Create(1), "View.Arrangement")

    member val AssignHotKeys: PropKey<bool> = PropKey.Create.simple (PropertyId.Create(2), "View.AssignHotKeys")

    member val BorderStyle: PropKey<Nullable<Terminal.Gui.Drawing.LineStyle>> =
      PropKey.Create.simple (PropertyId.Create(3), "View.BorderStyle")

    member val CanFocus: PropKey<bool> = PropKey.Create.simple (PropertyId.Create(4), "View.CanFocus")

    member val CommandsToBubbleUp: PropKey<IReadOnlyList<Terminal.Gui.Input.Command>> =
      PropKey.Create.simple (PropertyId.Create(5), "View.CommandsToBubbleUp")

    member val ContentSizeTracksViewport: PropKey<bool> =
      PropKey.Create.simple (PropertyId.Create(6), "View.ContentSizeTracksViewport")

    member val Cursor: PropKey<Terminal.Gui.Drivers.Cursor> =
      PropKey.Create.simple (PropertyId.Create(7), "View.Cursor")

    member val Data: PropKey<System.Object> = PropKey.Create.simple (PropertyId.Create(8), "View.Data")

    member val DefaultAcceptView: PropKey<Terminal.Gui.ViewBase.View> =
      PropKey.Create.view (PropertyId.Create(9), PropertyId.Create(10), "View.DefaultAcceptView_view")

    member val DefaultAcceptView_viewSpec: PropKey<IView> =
      PropKey.Create.subElement (PropertyId.Create(9), PropertyId.Create(10), "View.DefaultAcceptView_viewSpec")

    member val Enabled: PropKey<bool> = PropKey.Create.simple (PropertyId.Create(11), "View.Enabled")
    member val Frame: PropKey<System.Drawing.Rectangle> = PropKey.Create.simple (PropertyId.Create(12), "View.Frame")
    member val HasFocus: PropKey<bool> = PropKey.Create.simple (PropertyId.Create(13), "View.HasFocus")
    member val Height: PropKey<Terminal.Gui.ViewBase.Dim> = PropKey.Create.simple (PropertyId.Create(14), "View.Height")
    member val HotKey: PropKey<Terminal.Gui.Input.Key> = PropKey.Create.simple (PropertyId.Create(15), "View.HotKey")

    member val HotKeySpecifier: PropKey<System.Text.Rune> =
      PropKey.Create.simple (PropertyId.Create(16), "View.HotKeySpecifier")

    member val Id: PropKey<string> = PropKey.Create.simple (PropertyId.Create(17), "View.Id")
    member val IsInitialized: PropKey<bool> = PropKey.Create.simple (PropertyId.Create(18), "View.IsInitialized")

    member val MouseHighlightStates: PropKey<Terminal.Gui.ViewBase.MouseState> =
      PropKey.Create.simple (PropertyId.Create(19), "View.MouseHighlightStates")

    member val MouseHoldRepeat: PropKey<Nullable<Terminal.Gui.Input.MouseFlags>> =
      PropKey.Create.simple (PropertyId.Create(20), "View.MouseHoldRepeat")

    member val MousePositionTracking: PropKey<bool> =
      PropKey.Create.simple (PropertyId.Create(21), "View.MousePositionTracking")

    member val PreserveTrailingSpaces: PropKey<bool> =
      PropKey.Create.simple (PropertyId.Create(22), "View.PreserveTrailingSpaces")

    member val SchemeName: PropKey<string> = PropKey.Create.simple (PropertyId.Create(23), "View.SchemeName")

    member val ShadowStyle: PropKey<Nullable<Terminal.Gui.ViewBase.ShadowStyles>> =
      PropKey.Create.simple (PropertyId.Create(24), "View.ShadowStyle")

    member val SuperViewRendersLineCanvas: PropKey<bool> =
      PropKey.Create.simple (PropertyId.Create(25), "View.SuperViewRendersLineCanvas")

    member val TabStop: PropKey<Nullable<Terminal.Gui.ViewBase.TabBehavior>> =
      PropKey.Create.simple (PropertyId.Create(26), "View.TabStop")

    member val Text: PropKey<string> = PropKey.Create.simple (PropertyId.Create(27), "View.Text")

    member val TextAlignment: PropKey<Terminal.Gui.ViewBase.Alignment> =
      PropKey.Create.simple (PropertyId.Create(28), "View.TextAlignment")

    member val TextDirection: PropKey<Terminal.Gui.Text.TextDirection> =
      PropKey.Create.simple (PropertyId.Create(29), "View.TextDirection")

    member val Title: PropKey<string> = PropKey.Create.simple (PropertyId.Create(30), "View.Title")

    member val UsedHotKeys: PropKey<HashSet<Terminal.Gui.Input.Key>> =
      PropKey.Create.simple (PropertyId.Create(31), "View.UsedHotKeys")

    member val ValidatePosDim: PropKey<bool> = PropKey.Create.simple (PropertyId.Create(32), "View.ValidatePosDim")

    member val VerticalTextAlignment: PropKey<Terminal.Gui.ViewBase.Alignment> =
      PropKey.Create.simple (PropertyId.Create(33), "View.VerticalTextAlignment")

    member val Viewport: PropKey<System.Drawing.Rectangle> =
      PropKey.Create.simple (PropertyId.Create(34), "View.Viewport")

    member val ViewportSettings: PropKey<Terminal.Gui.ViewBase.ViewportSettingsFlags> =
      PropKey.Create.simple (PropertyId.Create(35), "View.ViewportSettings")

    member val Visible: PropKey<bool> = PropKey.Create.simple (PropertyId.Create(36), "View.Visible")
    member val Width: PropKey<Terminal.Gui.ViewBase.Dim> = PropKey.Create.simple (PropertyId.Create(37), "View.Width")

    // Events
    member val Accepted: PropKey<Terminal.Gui.Input.CommandEventArgs -> unit> =
      PropKey.Create.event (PropertyId.Create(38), "View.Accepted_event")

    member val Accepting: PropKey<Terminal.Gui.Input.CommandEventArgs -> unit> =
      PropKey.Create.event (PropertyId.Create(39), "View.Accepting_event")

    member val Activated: PropKey<EventArgs<Terminal.Gui.Input.ICommandContext> -> unit> =
      PropKey.Create.event (PropertyId.Create(40), "View.Activated_event")

    member val Activating: PropKey<Terminal.Gui.Input.CommandEventArgs -> unit> =
      PropKey.Create.event (PropertyId.Create(41), "View.Activating_event")

    member val AdvancingFocus: PropKey<Terminal.Gui.ViewBase.AdvanceFocusEventArgs -> unit> =
      PropKey.Create.event (PropertyId.Create(42), "View.AdvancingFocus_event")

    member val BorderStyleChanged: PropKey<System.EventArgs -> unit> =
      PropKey.Create.event (PropertyId.Create(43), "View.BorderStyleChanged_event")

    member val CanFocusChanged: PropKey<System.EventArgs -> unit> =
      PropKey.Create.event (PropertyId.Create(44), "View.CanFocusChanged_event")

    member val ClearedViewport: PropKey<Terminal.Gui.ViewBase.DrawEventArgs -> unit> =
      PropKey.Create.event (PropertyId.Create(45), "View.ClearedViewport_event")

    member val ClearingViewport: PropKey<Terminal.Gui.ViewBase.DrawEventArgs -> unit> =
      PropKey.Create.event (PropertyId.Create(46), "View.ClearingViewport_event")

    member val CommandNotBound: PropKey<Terminal.Gui.Input.CommandEventArgs -> unit> =
      PropKey.Create.event (PropertyId.Create(47), "View.CommandNotBound_event")

    member val ContentSizeChanged: PropKey<ValueChangedEventArgs<Nullable<System.Drawing.Size>> -> unit> =
      PropKey.Create.event (PropertyId.Create(48), "View.ContentSizeChanged_event")

    member val ContentSizeChanging: PropKey<ValueChangingEventArgs<Nullable<System.Drawing.Size>> -> unit> =
      PropKey.Create.event (PropertyId.Create(49), "View.ContentSizeChanging_event")

    member val Disposing: PropKey<System.EventArgs -> unit> =
      PropKey.Create.event (PropertyId.Create(50), "View.Disposing_event")

    member val DrawComplete: PropKey<Terminal.Gui.ViewBase.DrawEventArgs -> unit> =
      PropKey.Create.event (PropertyId.Create(51), "View.DrawComplete_event")

    member val DrawingContent: PropKey<Terminal.Gui.ViewBase.DrawEventArgs -> unit> =
      PropKey.Create.event (PropertyId.Create(52), "View.DrawingContent_event")

    member val DrawingSubViews: PropKey<Terminal.Gui.ViewBase.DrawEventArgs -> unit> =
      PropKey.Create.event (PropertyId.Create(53), "View.DrawingSubViews_event")

    member val DrawingText: PropKey<Terminal.Gui.ViewBase.DrawEventArgs -> unit> =
      PropKey.Create.event (PropertyId.Create(54), "View.DrawingText_event")

    member val DrewText: PropKey<System.EventArgs -> unit> =
      PropKey.Create.event (PropertyId.Create(55), "View.DrewText_event")

    member val EnabledChanged: PropKey<System.EventArgs -> unit> =
      PropKey.Create.event (PropertyId.Create(56), "View.EnabledChanged_event")

    member val FocusedChanged: PropKey<Terminal.Gui.ViewBase.HasFocusEventArgs -> unit> =
      PropKey.Create.event (PropertyId.Create(57), "View.FocusedChanged_event")

    member val FrameChanged: PropKey<EventArgs<System.Drawing.Rectangle> -> unit> =
      PropKey.Create.event (PropertyId.Create(58), "View.FrameChanged_event")

    member val GettingAttributeForRole: PropKey<Terminal.Gui.Drawing.VisualRoleEventArgs -> unit> =
      PropKey.Create.event (PropertyId.Create(59), "View.GettingAttributeForRole_event")

    member val GettingScheme: PropKey<ResultEventArgs<Terminal.Gui.Drawing.Scheme> -> unit> =
      PropKey.Create.event (PropertyId.Create(60), "View.GettingScheme_event")

    member val HandlingHotKey: PropKey<Terminal.Gui.Input.CommandEventArgs -> unit> =
      PropKey.Create.event (PropertyId.Create(61), "View.HandlingHotKey_event")

    member val HasFocusChanged: PropKey<Terminal.Gui.ViewBase.HasFocusEventArgs -> unit> =
      PropKey.Create.event (PropertyId.Create(62), "View.HasFocusChanged_event")

    member val HasFocusChanging: PropKey<Terminal.Gui.ViewBase.HasFocusEventArgs -> unit> =
      PropKey.Create.event (PropertyId.Create(63), "View.HasFocusChanging_event")

    member val HeightChanged: PropKey<ValueChangedEventArgs<Terminal.Gui.ViewBase.Dim> -> unit> =
      PropKey.Create.event (PropertyId.Create(64), "View.HeightChanged_event")

    member val HeightChanging: PropKey<ValueChangingEventArgs<Terminal.Gui.ViewBase.Dim> -> unit> =
      PropKey.Create.event (PropertyId.Create(65), "View.HeightChanging_event")

    member val HotKeyChanged: PropKey<Terminal.Gui.Input.KeyChangedEventArgs -> unit> =
      PropKey.Create.event (PropertyId.Create(66), "View.HotKeyChanged_event")

    member val HotKeyCommand: PropKey<EventArgs<Terminal.Gui.Input.ICommandContext> -> unit> =
      PropKey.Create.event (PropertyId.Create(67), "View.HotKeyCommand_event")

    member val Initialized: PropKey<System.EventArgs -> unit> =
      PropKey.Create.event (PropertyId.Create(68), "View.Initialized_event")

    member val KeyDown: PropKey<Terminal.Gui.Input.Key -> unit> =
      PropKey.Create.event (PropertyId.Create(69), "View.KeyDown_event")

    member val KeyDownNotHandled: PropKey<Terminal.Gui.Input.Key -> unit> =
      PropKey.Create.event (PropertyId.Create(70), "View.KeyDownNotHandled_event")

    member val KeyUp: PropKey<Terminal.Gui.Input.Key -> unit> =
      PropKey.Create.event (PropertyId.Create(71), "View.KeyUp_event")

    member val MouseEnter: PropKey<System.ComponentModel.CancelEventArgs -> unit> =
      PropKey.Create.event (PropertyId.Create(72), "View.MouseEnter_event")

    member val MouseEvent: PropKey<Terminal.Gui.Input.Mouse -> unit> =
      PropKey.Create.event (PropertyId.Create(73), "View.MouseEvent_event")

    member val MouseHoldRepeatChanged: PropKey<ValueChangedEventArgs<Nullable<Terminal.Gui.Input.MouseFlags>> -> unit> =
      PropKey.Create.event (PropertyId.Create(74), "View.MouseHoldRepeatChanged_event")

    member val MouseHoldRepeatChanging: PropKey<ValueChangingEventArgs<Nullable<Terminal.Gui.Input.MouseFlags>> -> unit> =
      PropKey.Create.event (PropertyId.Create(75), "View.MouseHoldRepeatChanging_event")

    member val MouseLeave: PropKey<System.EventArgs -> unit> =
      PropKey.Create.event (PropertyId.Create(76), "View.MouseLeave_event")

    member val MouseStateChanged: PropKey<EventArgs<Terminal.Gui.ViewBase.MouseState> -> unit> =
      PropKey.Create.event (PropertyId.Create(77), "View.MouseStateChanged_event")

    member val Pasted: PropKey<Terminal.Gui.Input.PastedEventArgs -> unit> =
      PropKey.Create.event (PropertyId.Create(78), "View.Pasted_event")

    member val Pasting: PropKey<Terminal.Gui.Input.PastingEventArgs -> unit> =
      PropKey.Create.event (PropertyId.Create(79), "View.Pasting_event")

    member val Removed: PropKey<Terminal.Gui.ViewBase.SuperViewChangedEventArgs -> unit> =
      PropKey.Create.event (PropertyId.Create(80), "View.Removed_event")

    member val SchemeChanged: PropKey<ValueChangedEventArgs<Terminal.Gui.Drawing.Scheme> -> unit> =
      PropKey.Create.event (PropertyId.Create(81), "View.SchemeChanged_event")

    member val SchemeChanging: PropKey<ValueChangingEventArgs<Terminal.Gui.Drawing.Scheme> -> unit> =
      PropKey.Create.event (PropertyId.Create(82), "View.SchemeChanging_event")

    member val SchemeNameChanged: PropKey<ValueChangedEventArgs<string> -> unit> =
      PropKey.Create.event (PropertyId.Create(83), "View.SchemeNameChanged_event")

    member val SchemeNameChanging: PropKey<ValueChangingEventArgs<string> -> unit> =
      PropKey.Create.event (PropertyId.Create(84), "View.SchemeNameChanging_event")

    member val ShadowStyleChanged: PropKey<System.EventArgs -> unit> =
      PropKey.Create.event (PropertyId.Create(85), "View.ShadowStyleChanged_event")

    member val SubViewAdded: PropKey<Terminal.Gui.ViewBase.SuperViewChangedEventArgs -> unit> =
      PropKey.Create.event (PropertyId.Create(86), "View.SubViewAdded_event")

    member val SubViewAdding: PropKey<EventArgs<Terminal.Gui.ViewBase.View> -> unit> =
      PropKey.Create.event (PropertyId.Create(87), "View.SubViewAdding_event")

    member val SubViewLayout: PropKey<Terminal.Gui.ViewBase.LayoutEventArgs -> unit> =
      PropKey.Create.event (PropertyId.Create(88), "View.SubViewLayout_event")

    member val SubViewRemoved: PropKey<Terminal.Gui.ViewBase.SuperViewChangedEventArgs -> unit> =
      PropKey.Create.event (PropertyId.Create(89), "View.SubViewRemoved_event")

    member val SubViewsLaidOut: PropKey<Terminal.Gui.ViewBase.LayoutEventArgs -> unit> =
      PropKey.Create.event (PropertyId.Create(90), "View.SubViewsLaidOut_event")

    member val SuperViewChanged: PropKey<ValueChangedEventArgs<Terminal.Gui.ViewBase.View> -> unit> =
      PropKey.Create.event (PropertyId.Create(91), "View.SuperViewChanged_event")

    member val SuperViewChanging: PropKey<ValueChangingEventArgs<Terminal.Gui.ViewBase.View> -> unit> =
      PropKey.Create.event (PropertyId.Create(92), "View.SuperViewChanging_event")

    member val TextChanged: PropKey<System.EventArgs -> unit> =
      PropKey.Create.event (PropertyId.Create(93), "View.TextChanged_event")

    member val TitleChanged: PropKey<EventArgs<string> -> unit> =
      PropKey.Create.event (PropertyId.Create(94), "View.TitleChanged_event")

    member val TitleChanging: PropKey<CancelEventArgs<string> -> unit> =
      PropKey.Create.event (PropertyId.Create(95), "View.TitleChanging_event")

    member val ViewportChanged: PropKey<Terminal.Gui.ViewBase.DrawEventArgs -> unit> =
      PropKey.Create.event (PropertyId.Create(96), "View.ViewportChanged_event")

    member val VisibleChanged: PropKey<System.EventArgs -> unit> =
      PropKey.Create.event (PropertyId.Create(97), "View.VisibleChanged_event")

    member val VisibleChanging: PropKey<CancelEventArgs<bool> -> unit> =
      PropKey.Create.event (PropertyId.Create(98), "View.VisibleChanging_event")

    member val WidthChanged: PropKey<ValueChangedEventArgs<Terminal.Gui.ViewBase.Dim> -> unit> =
      PropKey.Create.event (PropertyId.Create(99), "View.WidthChanged_event")

    member val WidthChanging: PropKey<ValueChangingEventArgs<Terminal.Gui.ViewBase.Dim> -> unit> =
      PropKey.Create.event (PropertyId.Create(100), "View.WidthChanging_event")

  type AdornmentViewPKeys() =
    inherit ViewPKeys()

    // Properties
    member val Adornment: PropKey<Terminal.Gui.ViewBase.IAdornment> =
      PropKey.Create.simple (PropertyId.Create(101), "AdornmentView.Adornment")

    member val Diagnostics: PropKey<Terminal.Gui.ViewBase.ViewDiagnosticFlags> =
      PropKey.Create.simple (PropertyId.Create(102), "AdornmentView.Diagnostics")

    member val SuperViewRendersLineCanvas: PropKey<bool> =
      PropKey.Create.simple (PropertyId.Create(103), "AdornmentView.SuperViewRendersLineCanvas")

    member val Viewport: PropKey<System.Drawing.Rectangle> =
      PropKey.Create.simple (PropertyId.Create(104), "AdornmentView.Viewport")

  type AttributePickerPKeys() =
    inherit ViewPKeys()

    // Properties
    member val SampleText: PropKey<string> =
      PropKey.Create.simple (PropertyId.Create(105), "AttributePicker.SampleText")

    member val Value: PropKey<Nullable<Terminal.Gui.Drawing.Attribute>> =
      PropKey.Create.simple (PropertyId.Create(106), "AttributePicker.Value")

    // Events
    member val ValueChanged: PropKey<ValueChangedEventArgs<Nullable<Terminal.Gui.Drawing.Attribute>> -> unit> =
      PropKey.Create.event (PropertyId.Create(107), "AttributePicker.ValueChanged_event")

    member val ValueChangedUntyped: PropKey<ValueChangedEventArgs<System.Object> -> unit> =
      PropKey.Create.event (PropertyId.Create(108), "AttributePicker.ValueChangedUntyped_event")

    member val ValueChanging: PropKey<ValueChangingEventArgs<Nullable<Terminal.Gui.Drawing.Attribute>> -> unit> =
      PropKey.Create.event (PropertyId.Create(109), "AttributePicker.ValueChanging_event")

  type BarPKeys() =
    inherit ViewPKeys()

    // Properties
    member val AlignmentModes: PropKey<Terminal.Gui.ViewBase.AlignmentModes> =
      PropKey.Create.simple (PropertyId.Create(110), "Bar.AlignmentModes")

    member val Orientation: PropKey<Terminal.Gui.ViewBase.Orientation> =
      PropKey.Create.simple (PropertyId.Create(111), "Bar.Orientation")

    // Events
    member val OrientationChanged: PropKey<EventArgs<Terminal.Gui.ViewBase.Orientation> -> unit> =
      PropKey.Create.event (PropertyId.Create(112), "Bar.OrientationChanged_event")

    member val OrientationChanging: PropKey<CancelEventArgs<Terminal.Gui.ViewBase.Orientation> -> unit> =
      PropKey.Create.event (PropertyId.Create(113), "Bar.OrientationChanging_event")

  type BorderViewPKeys() =
    inherit AdornmentViewPKeys()

    // Properties
    member val TabLength: PropKey<Nullable<int>> =
      PropKey.Create.simple (PropertyId.Create(114), "BorderView.TabLength")

    member val TabOffset: PropKey<int> = PropKey.Create.simple (PropertyId.Create(115), "BorderView.TabOffset")

    member val TabSide: PropKey<Terminal.Gui.ViewBase.Side> =
      PropKey.Create.simple (PropertyId.Create(116), "BorderView.TabSide")

  type ButtonPKeys() =
    inherit ViewPKeys()

    // Properties
    member val HotKeySpecifier: PropKey<System.Text.Rune> =
      PropKey.Create.simple (PropertyId.Create(117), "Button.HotKeySpecifier")

    member val IsDefault: PropKey<bool> = PropKey.Create.simple (PropertyId.Create(118), "Button.IsDefault")
    member val NoDecorations: PropKey<bool> = PropKey.Create.simple (PropertyId.Create(119), "Button.NoDecorations")
    member val NoPadding: PropKey<bool> = PropKey.Create.simple (PropertyId.Create(120), "Button.NoPadding")
    member val Text: PropKey<string> = PropKey.Create.simple (PropertyId.Create(121), "Button.Text")

    // Events
    member val InitializingShadowStyle: PropKey<
      ValueChangingEventArgs<Nullable<Terminal.Gui.ViewBase.ShadowStyles>> -> unit
     > = PropKey.Create.event (PropertyId.Create(122), "Button.InitializingShadowStyle_event")

  type CharMapPKeys() =
    inherit ViewPKeys()

    // Properties
    member val SelectedCodePoint: PropKey<int> =
      PropKey.Create.simple (PropertyId.Create(123), "CharMap.SelectedCodePoint")

    member val ShowGlyphWidths: PropKey<bool> =
      PropKey.Create.simple (PropertyId.Create(124), "CharMap.ShowGlyphWidths")

    member val ShowUnicodeCategory: PropKey<Nullable<System.Globalization.UnicodeCategory>> =
      PropKey.Create.simple (PropertyId.Create(125), "CharMap.ShowUnicodeCategory")

    member val StartCodePoint: PropKey<int> = PropKey.Create.simple (PropertyId.Create(126), "CharMap.StartCodePoint")
    member val Value: PropKey<System.Text.Rune> = PropKey.Create.simple (PropertyId.Create(127), "CharMap.Value")

    // Events
    member val ValueChanged: PropKey<ValueChangedEventArgs<System.Text.Rune> -> unit> =
      PropKey.Create.event (PropertyId.Create(128), "CharMap.ValueChanged_event")

    member val ValueChangedUntyped: PropKey<ValueChangedEventArgs<System.Object> -> unit> =
      PropKey.Create.event (PropertyId.Create(129), "CharMap.ValueChangedUntyped_event")

    member val ValueChanging: PropKey<ValueChangingEventArgs<System.Text.Rune> -> unit> =
      PropKey.Create.event (PropertyId.Create(130), "CharMap.ValueChanging_event")

  type CheckBoxPKeys() =
    inherit ViewPKeys()

    // Properties
    member val AllowCheckStateNone: PropKey<bool> =
      PropKey.Create.simple (PropertyId.Create(131), "CheckBox.AllowCheckStateNone")

    member val HotKeySpecifier: PropKey<System.Text.Rune> =
      PropKey.Create.simple (PropertyId.Create(132), "CheckBox.HotKeySpecifier")

    member val RadioStyle: PropKey<bool> = PropKey.Create.simple (PropertyId.Create(133), "CheckBox.RadioStyle")
    member val Text: PropKey<string> = PropKey.Create.simple (PropertyId.Create(134), "CheckBox.Text")

    member val Value: PropKey<Terminal.Gui.Views.CheckState> =
      PropKey.Create.simple (PropertyId.Create(135), "CheckBox.Value")

    // Events
    member val ValueChanged: PropKey<ValueChangedEventArgs<Terminal.Gui.Views.CheckState> -> unit> =
      PropKey.Create.event (PropertyId.Create(136), "CheckBox.ValueChanged_event")

    member val ValueChangedUntyped: PropKey<ValueChangedEventArgs<System.Object> -> unit> =
      PropKey.Create.event (PropertyId.Create(137), "CheckBox.ValueChangedUntyped_event")

    member val ValueChanging: PropKey<ValueChangingEventArgs<Terminal.Gui.Views.CheckState> -> unit> =
      PropKey.Create.event (PropertyId.Create(138), "CheckBox.ValueChanging_event")

  type CodePKeys() =
    inherit ViewPKeys()

    // Properties
    member val Language: PropKey<string> = PropKey.Create.simple (PropertyId.Create(139), "Code.Language")

    member val SyntaxHighlighter: PropKey<Terminal.Gui.Drawing.ISyntaxHighlighter> =
      PropKey.Create.simple (PropertyId.Create(140), "Code.SyntaxHighlighter")

    member val Text: PropKey<string> = PropKey.Create.simple (PropertyId.Create(141), "Code.Text")

  type ColorPickerPKeys() =
    inherit ViewPKeys()

    // Properties
    member val SelectedColor: PropKey<Terminal.Gui.Drawing.Color> =
      PropKey.Create.simple (PropertyId.Create(142), "ColorPicker.SelectedColor")

    member val Style: PropKey<Terminal.Gui.Views.ColorPickerStyle> =
      PropKey.Create.simple (PropertyId.Create(143), "ColorPicker.Style")

    member val Text: PropKey<string> = PropKey.Create.simple (PropertyId.Create(144), "ColorPicker.Text")

    member val Value: PropKey<Nullable<Terminal.Gui.Drawing.Color>> =
      PropKey.Create.simple (PropertyId.Create(145), "ColorPicker.Value")

    // Events
    member val ValueChanged: PropKey<ValueChangedEventArgs<Nullable<Terminal.Gui.Drawing.Color>> -> unit> =
      PropKey.Create.event (PropertyId.Create(146), "ColorPicker.ValueChanged_event")

    member val ValueChangedUntyped: PropKey<ValueChangedEventArgs<System.Object> -> unit> =
      PropKey.Create.event (PropertyId.Create(147), "ColorPicker.ValueChangedUntyped_event")

    member val ValueChanging: PropKey<ValueChangingEventArgs<Nullable<Terminal.Gui.Drawing.Color>> -> unit> =
      PropKey.Create.event (PropertyId.Create(148), "ColorPicker.ValueChanging_event")

  type ColorPicker16PKeys() =
    inherit ViewPKeys()

    // Properties
    member val BoxHeight: PropKey<int> = PropKey.Create.simple (PropertyId.Create(149), "ColorPicker16.BoxHeight")
    member val BoxWidth: PropKey<int> = PropKey.Create.simple (PropertyId.Create(150), "ColorPicker16.BoxWidth")

    member val Caret: PropKey<System.Drawing.Point> =
      PropKey.Create.simple (PropertyId.Create(151), "ColorPicker16.Caret")

    member val SelectedColor: PropKey<Terminal.Gui.Drawing.ColorName16> =
      PropKey.Create.simple (PropertyId.Create(152), "ColorPicker16.SelectedColor")

    member val Value: PropKey<Terminal.Gui.Drawing.ColorName16> =
      PropKey.Create.simple (PropertyId.Create(153), "ColorPicker16.Value")

    // Events
    member val ValueChanged: PropKey<ValueChangedEventArgs<Terminal.Gui.Drawing.ColorName16> -> unit> =
      PropKey.Create.event (PropertyId.Create(154), "ColorPicker16.ValueChanged_event")

    member val ValueChangedUntyped: PropKey<ValueChangedEventArgs<System.Object> -> unit> =
      PropKey.Create.event (PropertyId.Create(155), "ColorPicker16.ValueChangedUntyped_event")

    member val ValueChanging: PropKey<ValueChangingEventArgs<Terminal.Gui.Drawing.ColorName16> -> unit> =
      PropKey.Create.event (PropertyId.Create(156), "ColorPicker16.ValueChanging_event")

  type DatePickerPKeys() =
    inherit ViewPKeys()

    // Properties
    member val Culture: PropKey<System.Globalization.CultureInfo> =
      PropKey.Create.simple (PropertyId.Create(157), "DatePicker.Culture")

    member val Text: PropKey<string> = PropKey.Create.simple (PropertyId.Create(158), "DatePicker.Text")
    member val Value: PropKey<System.DateTime> = PropKey.Create.simple (PropertyId.Create(159), "DatePicker.Value")

    // Events
    member val ValueChanged: PropKey<ValueChangedEventArgs<System.DateTime> -> unit> =
      PropKey.Create.event (PropertyId.Create(160), "DatePicker.ValueChanged_event")

    member val ValueChangedUntyped: PropKey<ValueChangedEventArgs<System.Object> -> unit> =
      PropKey.Create.event (PropertyId.Create(161), "DatePicker.ValueChangedUntyped_event")

    member val ValueChanging: PropKey<ValueChangingEventArgs<System.DateTime> -> unit> =
      PropKey.Create.event (PropertyId.Create(162), "DatePicker.ValueChanging_event")

  type FrameViewPKeys() =
    inherit ViewPKeys()


  type GraphViewPKeys() =
    inherit ViewPKeys()

    // Properties
    member val AxisX: PropKey<Terminal.Gui.Views.HorizontalAxis> =
      PropKey.Create.simple (PropertyId.Create(163), "GraphView.AxisX")

    member val AxisY: PropKey<Terminal.Gui.Views.VerticalAxis> =
      PropKey.Create.simple (PropertyId.Create(164), "GraphView.AxisY")

    member val CellSize: PropKey<System.Drawing.PointF> =
      PropKey.Create.simple (PropertyId.Create(165), "GraphView.CellSize")

    member val GraphColor: PropKey<Nullable<Terminal.Gui.Drawing.Attribute>> =
      PropKey.Create.simple (PropertyId.Create(166), "GraphView.GraphColor")

    member val MarginBottom: PropKey<System.UInt32> =
      PropKey.Create.simple (PropertyId.Create(167), "GraphView.MarginBottom")

    member val MarginLeft: PropKey<System.UInt32> =
      PropKey.Create.simple (PropertyId.Create(168), "GraphView.MarginLeft")

    member val ScrollOffset: PropKey<System.Drawing.PointF> =
      PropKey.Create.simple (PropertyId.Create(169), "GraphView.ScrollOffset")

  type HexViewPKeys() =
    inherit ViewPKeys()

    // Properties
    member val Address: PropKey<System.Int64> = PropKey.Create.simple (PropertyId.Create(170), "HexView.Address")
    member val AddressWidth: PropKey<int> = PropKey.Create.simple (PropertyId.Create(171), "HexView.AddressWidth")
    member val BytesPerLine: PropKey<int> = PropKey.Create.simple (PropertyId.Create(172), "HexView.BytesPerLine")
    member val ReadOnly: PropKey<bool> = PropKey.Create.simple (PropertyId.Create(173), "HexView.ReadOnly")
    member val Source: PropKey<System.IO.Stream> = PropKey.Create.simple (PropertyId.Create(174), "HexView.Source")

    // Events
    member val Edited: PropKey<Terminal.Gui.Views.HexViewEditEventArgs -> unit> =
      PropKey.Create.event (PropertyId.Create(175), "HexView.Edited_event")

    member val PositionChanged: PropKey<Terminal.Gui.Views.HexViewEventArgs -> unit> =
      PropKey.Create.event (PropertyId.Create(176), "HexView.PositionChanged_event")

  type ImageViewPKeys() =
    inherit ViewPKeys()

    // Properties
    member val AllowSixelUpscaling: PropKey<bool> =
      PropKey.Create.simple (PropertyId.Create(177), "ImageView.AllowSixelUpscaling")

    member val Image: PropKey<Terminal.Gui.Drawing.Color[,]> =
      PropKey.Create.simple (PropertyId.Create(178), "ImageView.Image")

    member val MaxSixelPaletteColors: PropKey<int> =
      PropKey.Create.simple (PropertyId.Create(179), "ImageView.MaxSixelPaletteColors")

    member val SixelEncoder: PropKey<Terminal.Gui.Drawing.SixelEncoder> =
      PropKey.Create.simple (PropertyId.Create(180), "ImageView.SixelEncoder")

    member val UseBackgroundRendering: PropKey<bool> =
      PropKey.Create.simple (PropertyId.Create(181), "ImageView.UseBackgroundRendering")

    member val UseRasterGraphics: PropKey<bool> =
      PropKey.Create.simple (PropertyId.Create(182), "ImageView.UseRasterGraphics")

    member val UseSixel: PropKey<bool> = PropKey.Create.simple (PropertyId.Create(183), "ImageView.UseSixel")
    member val ZoomLevel: PropKey<System.Double> = PropKey.Create.simple (PropertyId.Create(184), "ImageView.ZoomLevel")

    // Events
    member val ZoomLevelChanged: PropKey<System.EventArgs -> unit> =
      PropKey.Create.event (PropertyId.Create(185), "ImageView.ZoomLevelChanged_event")

  type LabelPKeys() =
    inherit ViewPKeys()

    // Properties
    member val HotKeySpecifier: PropKey<System.Text.Rune> =
      PropKey.Create.simple (PropertyId.Create(186), "Label.HotKeySpecifier")

    member val Text: PropKey<string> = PropKey.Create.simple (PropertyId.Create(187), "Label.Text")

  type LegendAnnotationPKeys() =
    inherit ViewPKeys()


  type LinePKeys() =
    inherit ViewPKeys()

    // Properties
    member val Length: PropKey<Terminal.Gui.ViewBase.Dim> =
      PropKey.Create.simple (PropertyId.Create(188), "Line.Length")

    member val LineAttribute: PropKey<Nullable<Terminal.Gui.Drawing.Attribute>> =
      PropKey.Create.simple (PropertyId.Create(189), "Line.LineAttribute")

    member val Orientation: PropKey<Terminal.Gui.ViewBase.Orientation> =
      PropKey.Create.simple (PropertyId.Create(190), "Line.Orientation")

    member val Style: PropKey<Terminal.Gui.Drawing.LineStyle> =
      PropKey.Create.simple (PropertyId.Create(191), "Line.Style")

    // Events
    member val OrientationChanged: PropKey<EventArgs<Terminal.Gui.ViewBase.Orientation> -> unit> =
      PropKey.Create.event (PropertyId.Create(192), "Line.OrientationChanged_event")

    member val OrientationChanging: PropKey<CancelEventArgs<Terminal.Gui.ViewBase.Orientation> -> unit> =
      PropKey.Create.event (PropertyId.Create(193), "Line.OrientationChanging_event")

  type LinearRangeViewBasePKeys<'TOption, 'TValue>() =
    inherit ViewPKeys()

    // Properties
    member val AllowEmpty: PropKey<bool> =
      PropKey.Create.simple (PropertyId.Create(194), "LinearRangeViewBase.AllowEmpty")

    member val FocusedOption: PropKey<int> =
      PropKey.Create.simple (PropertyId.Create(195), "LinearRangeViewBase.FocusedOption")

    member val LegendsOrientation: PropKey<Terminal.Gui.ViewBase.Orientation> =
      PropKey.Create.simple (PropertyId.Create(196), "LinearRangeViewBase.LegendsOrientation")

    member val MinimumInnerSpacing: PropKey<int> =
      PropKey.Create.simple (PropertyId.Create(197), "LinearRangeViewBase.MinimumInnerSpacing")

    member val Options: PropKey<List<LinearRangeOption<'TOption>>> =
      PropKey.Create.simple (PropertyId.Create(198), "LinearRangeViewBase.Options")

    member val Orientation: PropKey<Terminal.Gui.ViewBase.Orientation> =
      PropKey.Create.simple (PropertyId.Create(199), "LinearRangeViewBase.Orientation")

    member val ShowEndSpacing: PropKey<bool> =
      PropKey.Create.simple (PropertyId.Create(200), "LinearRangeViewBase.ShowEndSpacing")

    member val ShowLegends: PropKey<bool> =
      PropKey.Create.simple (PropertyId.Create(201), "LinearRangeViewBase.ShowLegends")

    member val Style: PropKey<Terminal.Gui.Views.LinearRangeStyle> =
      PropKey.Create.simple (PropertyId.Create(202), "LinearRangeViewBase.Style")

    member val Text: PropKey<string> = PropKey.Create.simple (PropertyId.Create(203), "LinearRangeViewBase.Text")

    member val UseMinimumSize: PropKey<bool> =
      PropKey.Create.simple (PropertyId.Create(204), "LinearRangeViewBase.UseMinimumSize")

    member val Value: PropKey<'TValue> = PropKey.Create.simple (PropertyId.Create(205), "LinearRangeViewBase.Value")

    // Events
    member val LegendsOrientationChanged: PropKey<ValueChangedEventArgs<Terminal.Gui.ViewBase.Orientation> -> unit> =
      PropKey.Create.event (PropertyId.Create(206), "LinearRangeViewBase.LegendsOrientationChanged_event")

    member val LegendsOrientationChanging: PropKey<ValueChangingEventArgs<Terminal.Gui.ViewBase.Orientation> -> unit> =
      PropKey.Create.event (PropertyId.Create(207), "LinearRangeViewBase.LegendsOrientationChanging_event")

    member val MinimumInnerSpacingChanged: PropKey<ValueChangedEventArgs<int> -> unit> =
      PropKey.Create.event (PropertyId.Create(208), "LinearRangeViewBase.MinimumInnerSpacingChanged_event")

    member val MinimumInnerSpacingChanging: PropKey<ValueChangingEventArgs<int> -> unit> =
      PropKey.Create.event (PropertyId.Create(209), "LinearRangeViewBase.MinimumInnerSpacingChanging_event")

    member val OptionFocused: PropKey<LinearRangeEventArgs<'TOption> -> unit> =
      PropKey.Create.event (PropertyId.Create(210), "LinearRangeViewBase.OptionFocused_event")

    member val OrientationChanged: PropKey<EventArgs<Terminal.Gui.ViewBase.Orientation> -> unit> =
      PropKey.Create.event (PropertyId.Create(211), "LinearRangeViewBase.OrientationChanged_event")

    member val OrientationChanging: PropKey<CancelEventArgs<Terminal.Gui.ViewBase.Orientation> -> unit> =
      PropKey.Create.event (PropertyId.Create(212), "LinearRangeViewBase.OrientationChanging_event")

    member val ShowEndSpacingChanged: PropKey<ValueChangedEventArgs<bool> -> unit> =
      PropKey.Create.event (PropertyId.Create(213), "LinearRangeViewBase.ShowEndSpacingChanged_event")

    member val ShowEndSpacingChanging: PropKey<ValueChangingEventArgs<bool> -> unit> =
      PropKey.Create.event (PropertyId.Create(214), "LinearRangeViewBase.ShowEndSpacingChanging_event")

    member val ShowLegendsChanged: PropKey<ValueChangedEventArgs<bool> -> unit> =
      PropKey.Create.event (PropertyId.Create(215), "LinearRangeViewBase.ShowLegendsChanged_event")

    member val ShowLegendsChanging: PropKey<ValueChangingEventArgs<bool> -> unit> =
      PropKey.Create.event (PropertyId.Create(216), "LinearRangeViewBase.ShowLegendsChanging_event")

    member val UseMinimumSizeChanged: PropKey<ValueChangedEventArgs<bool> -> unit> =
      PropKey.Create.event (PropertyId.Create(217), "LinearRangeViewBase.UseMinimumSizeChanged_event")

    member val UseMinimumSizeChanging: PropKey<ValueChangingEventArgs<bool> -> unit> =
      PropKey.Create.event (PropertyId.Create(218), "LinearRangeViewBase.UseMinimumSizeChanging_event")

    member val ValueChanged: PropKey<ValueChangedEventArgs<'TValue> -> unit> =
      PropKey.Create.event (PropertyId.Create(219), "LinearRangeViewBase.ValueChanged_event")

    member val ValueChangedUntyped: PropKey<ValueChangedEventArgs<System.Object> -> unit> =
      PropKey.Create.event (PropertyId.Create(220), "LinearRangeViewBase.ValueChangedUntyped_event")

    member val ValueChanging: PropKey<ValueChangingEventArgs<'TValue> -> unit> =
      PropKey.Create.event (PropertyId.Create(221), "LinearRangeViewBase.ValueChanging_event")

  type LinearMultiSelectorPKeys<'T>() =
    inherit LinearRangeViewBasePKeys<'T, IReadOnlyList<'T>>()

    // Properties
    member val Value: PropKey<IReadOnlyList<'T>> =
      PropKey.Create.simple (PropertyId.Create(222), "LinearMultiSelector.Value")

  type LinearMultiSelectorPKeys() =
    inherit LinearMultiSelectorPKeys<string>()


  type LinearRangePKeys<'T>() =
    inherit LinearRangeViewBasePKeys<'T, LinearRangeSpan<'T>>()

    // Properties
    member val RangeAllowSingle: PropKey<bool> =
      PropKey.Create.simple (PropertyId.Create(223), "LinearRange.RangeAllowSingle")

    member val RangeKind: PropKey<Terminal.Gui.Views.LinearRangeSpanKind> =
      PropKey.Create.simple (PropertyId.Create(224), "LinearRange.RangeKind")

    member val Value: PropKey<LinearRangeSpan<'T>> = PropKey.Create.simple (PropertyId.Create(225), "LinearRange.Value")

  type LinearRangePKeys() =
    inherit LinearRangePKeys<string>()


  type LinearSelectorPKeys<'T>() =
    inherit LinearRangeViewBasePKeys<'T, 'T>()

    // Properties
    member val SelectedIndex: PropKey<Nullable<int>> =
      PropKey.Create.simple (PropertyId.Create(226), "LinearSelector.SelectedIndex")

    member val Value: PropKey<'T> = PropKey.Create.simple (PropertyId.Create(227), "LinearSelector.Value")

  type LinearSelectorPKeys() =
    inherit LinearSelectorPKeys<string>()


  type LinkPKeys() =
    inherit ViewPKeys()

    // Properties
    member val Url: PropKey<string> = PropKey.Create.simple (PropertyId.Create(228), "Link.Url")

    // Events
    member val UrlChanged: PropKey<ValueChangedEventArgs<string> -> unit> =
      PropKey.Create.event (PropertyId.Create(229), "Link.UrlChanged_event")

    member val UrlChanging: PropKey<ValueChangingEventArgs<string> -> unit> =
      PropKey.Create.event (PropertyId.Create(230), "Link.UrlChanging_event")

  type ListViewPKeys() =
    inherit ViewPKeys()

    // Properties
    member val KeystrokeNavigator: PropKey<Terminal.Gui.Views.IListCollectionNavigator> =
      PropKey.Create.simple (PropertyId.Create(231), "ListView.KeystrokeNavigator")

    member val MarkMultiple: PropKey<bool> = PropKey.Create.simple (PropertyId.Create(232), "ListView.MarkMultiple")

    member val SelectedItem: PropKey<Nullable<int>> =
      PropKey.Create.simple (PropertyId.Create(233), "ListView.SelectedItem")

    member val ShowMarks: PropKey<bool> = PropKey.Create.simple (PropertyId.Create(234), "ListView.ShowMarks")

    member val Source: PropKey<Terminal.Gui.Views.IListDataSource> =
      PropKey.Create.simple (PropertyId.Create(235), "ListView.Source")

    member val Value: PropKey<Nullable<int>> = PropKey.Create.simple (PropertyId.Create(236), "ListView.Value")

    // Events
    member val CollectionChanged: PropKey<System.Collections.Specialized.NotifyCollectionChangedEventArgs -> unit> =
      PropKey.Create.event (PropertyId.Create(237), "ListView.CollectionChanged_event")

    member val RowRender: PropKey<Terminal.Gui.Views.ListViewRowEventArgs -> unit> =
      PropKey.Create.event (PropertyId.Create(238), "ListView.RowRender_event")

    member val SourceChanged: PropKey<System.EventArgs -> unit> =
      PropKey.Create.event (PropertyId.Create(239), "ListView.SourceChanged_event")

    member val ValueChanged: PropKey<ValueChangedEventArgs<Nullable<int>> -> unit> =
      PropKey.Create.event (PropertyId.Create(240), "ListView.ValueChanged_event")

    member val ValueChangedUntyped: PropKey<ValueChangedEventArgs<System.Object> -> unit> =
      PropKey.Create.event (PropertyId.Create(241), "ListView.ValueChangedUntyped_event")

    member val ValueChanging: PropKey<ValueChangingEventArgs<Nullable<int>> -> unit> =
      PropKey.Create.event (PropertyId.Create(242), "ListView.ValueChanging_event")

  type ListViewPKeys<'T>() =
    inherit ListViewPKeys()

    // Properties
    member val Index: PropKey<Nullable<int>> = PropKey.Create.simple (PropertyId.Create(243), "ListView.Index")
    member val SelectedItem: PropKey<'T> = PropKey.Create.simple (PropertyId.Create(244), "ListView.SelectedItem")
    member val Value: PropKey<'T> = PropKey.Create.simple (PropertyId.Create(245), "ListView.Value")

    // Events
    member val ValueChanged: PropKey<ValueChangedEventArgs<'T> -> unit> =
      PropKey.Create.event (PropertyId.Create(246), "ListView.ValueChanged_event")

    member val ValueChangedUntyped: PropKey<ValueChangedEventArgs<System.Object> -> unit> =
      PropKey.Create.event (PropertyId.Create(247), "ListView.ValueChangedUntyped_event")

    member val ValueChanging: PropKey<ValueChangingEventArgs<'T> -> unit> =
      PropKey.Create.event (PropertyId.Create(248), "ListView.ValueChanging_event")

  type MarginViewPKeys() =
    inherit AdornmentViewPKeys()

    // Properties
    member val ShadowSize: PropKey<System.Drawing.Size> =
      PropKey.Create.simple (PropertyId.Create(249), "MarginView.ShadowSize")

    member val ShadowStyle: PropKey<Nullable<Terminal.Gui.ViewBase.ShadowStyles>> =
      PropKey.Create.simple (PropertyId.Create(250), "MarginView.ShadowStyle")

  type MarkdownPKeys() =
    inherit ViewPKeys()

    // Properties
    member val EnableSixelImages: PropKey<bool> =
      PropKey.Create.simple (PropertyId.Create(251), "Markdown.EnableSixelImages")

    member val HotKeySpecifier: PropKey<System.Text.Rune> =
      PropKey.Create.simple (PropertyId.Create(252), "Markdown.HotKeySpecifier")

    member val ImageLoader: PropKey<Func<string, System.Byte[]>> =
      PropKey.Create.simple (PropertyId.Create(253), "Markdown.ImageLoader")

    member val MarkdownPipeline: PropKey<Markdig.MarkdownPipeline> =
      PropKey.Create.simple (PropertyId.Create(254), "Markdown.MarkdownPipeline")

    member val ShowCopyButtons: PropKey<bool> =
      PropKey.Create.simple (PropertyId.Create(255), "Markdown.ShowCopyButtons")

    member val ShowHeadingPrefix: PropKey<bool> =
      PropKey.Create.simple (PropertyId.Create(256), "Markdown.ShowHeadingPrefix")

    member val SyntaxHighlighter: PropKey<Terminal.Gui.Drawing.ISyntaxHighlighter> =
      PropKey.Create.simple (PropertyId.Create(257), "Markdown.SyntaxHighlighter")

    member val Text: PropKey<string> = PropKey.Create.simple (PropertyId.Create(258), "Markdown.Text")

    member val UseThemeBackground: PropKey<bool> =
      PropKey.Create.simple (PropertyId.Create(259), "Markdown.UseThemeBackground")

    // Events
    member val LinkClicked: PropKey<Terminal.Gui.Views.MarkdownLinkEventArgs -> unit> =
      PropKey.Create.event (PropertyId.Create(260), "Markdown.LinkClicked_event")

    member val MarkdownChanged: PropKey<System.EventArgs -> unit> =
      PropKey.Create.event (PropertyId.Create(261), "Markdown.MarkdownChanged_event")

  type MarkdownCodeBlockPKeys() =
    inherit ViewPKeys()

    // Properties
    member val CodeLines: PropKey<IReadOnlyList<string>> =
      PropKey.Create.simple (PropertyId.Create(262), "MarkdownCodeBlock.CodeLines")

    member val Language: PropKey<string> = PropKey.Create.simple (PropertyId.Create(263), "MarkdownCodeBlock.Language")

    member val ShowCopyButton: PropKey<bool> =
      PropKey.Create.simple (PropertyId.Create(264), "MarkdownCodeBlock.ShowCopyButton")

    member val SyntaxHighlighter: PropKey<Terminal.Gui.Drawing.ISyntaxHighlighter> =
      PropKey.Create.simple (PropertyId.Create(265), "MarkdownCodeBlock.SyntaxHighlighter")

    member val Text: PropKey<string> = PropKey.Create.simple (PropertyId.Create(266), "MarkdownCodeBlock.Text")

    member val ThemeBackground: PropKey<Nullable<Terminal.Gui.Drawing.Color>> =
      PropKey.Create.simple (PropertyId.Create(267), "MarkdownCodeBlock.ThemeBackground")

  type MarkdownTablePKeys() =
    inherit ViewPKeys()

    // Properties
    member val SyntaxHighlighter: PropKey<Terminal.Gui.Drawing.ISyntaxHighlighter> =
      PropKey.Create.simple (PropertyId.Create(268), "MarkdownTable.SyntaxHighlighter")

    member val TableData: PropKey<Terminal.Gui.Views.TableData> =
      PropKey.Create.simple (PropertyId.Create(269), "MarkdownTable.TableData")

    member val Text: PropKey<string> = PropKey.Create.simple (PropertyId.Create(270), "MarkdownTable.Text")

    member val UseThemeBackground: PropKey<bool> =
      PropKey.Create.simple (PropertyId.Create(271), "MarkdownTable.UseThemeBackground")

    // Events
    member val LinkClicked: PropKey<Terminal.Gui.Views.MarkdownLinkEventArgs -> unit> =
      PropKey.Create.event (PropertyId.Create(272), "MarkdownTable.LinkClicked_event")

  type MenuPKeys() =
    inherit BarPKeys()

    // Properties
    member val SuperMenuItem: PropKey<Terminal.Gui.Views.MenuItem> =
      PropKey.Create.view (PropertyId.Create(273), PropertyId.Create(274), "Menu.SuperMenuItem_view")

    member val SuperMenuItem_viewSpec: PropKey<IMenuItemView> =
      PropKey.Create.subElement (PropertyId.Create(273), PropertyId.Create(274), "Menu.SuperMenuItem_viewSpec")

    member val Value: PropKey<Terminal.Gui.Views.MenuItem> =
      PropKey.Create.view (PropertyId.Create(275), PropertyId.Create(276), "Menu.Value_view")

    member val Value_viewSpec: PropKey<IMenuItemView> =
      PropKey.Create.subElement (PropertyId.Create(275), PropertyId.Create(276), "Menu.Value_viewSpec")

    // Events
    member val SelectedMenuItemChanged: PropKey<Terminal.Gui.Views.MenuItem -> unit> =
      PropKey.Create.event (PropertyId.Create(277), "Menu.SelectedMenuItemChanged_event")

    member val ValueChanged: PropKey<ValueChangedEventArgs<Terminal.Gui.Views.MenuItem> -> unit> =
      PropKey.Create.event (PropertyId.Create(278), "Menu.ValueChanged_event")

    member val ValueChangedUntyped: PropKey<ValueChangedEventArgs<System.Object> -> unit> =
      PropKey.Create.event (PropertyId.Create(279), "Menu.ValueChangedUntyped_event")

    member val ValueChanging: PropKey<ValueChangingEventArgs<Terminal.Gui.Views.MenuItem> -> unit> =
      PropKey.Create.event (PropertyId.Create(280), "Menu.ValueChanging_event")

  type MenuBarPKeys() =
    inherit MenuPKeys()

    // Properties
    member val Key: PropKey<Terminal.Gui.Input.Key> = PropKey.Create.simple (PropertyId.Create(281), "MenuBar.Key")

    // Events
    member val KeyChanged: PropKey<Terminal.Gui.Input.KeyChangedEventArgs -> unit> =
      PropKey.Create.event (PropertyId.Create(282), "MenuBar.KeyChanged_event")

  type NumericUpDownPKeys<'T>() =
    inherit ViewPKeys()

    // Properties
    member val Format: PropKey<string> = PropKey.Create.simple (PropertyId.Create(283), "NumericUpDown.Format")
    member val Increment: PropKey<'T> = PropKey.Create.simple (PropertyId.Create(284), "NumericUpDown.Increment")
    member val Value: PropKey<'T> = PropKey.Create.simple (PropertyId.Create(285), "NumericUpDown.Value")

    // Events
    member val FormatChanged: PropKey<EventArgs<string> -> unit> =
      PropKey.Create.event (PropertyId.Create(286), "NumericUpDown.FormatChanged_event")

    member val IncrementChanged: PropKey<EventArgs<'T> -> unit> =
      PropKey.Create.event (PropertyId.Create(287), "NumericUpDown.IncrementChanged_event")

    member val ValueChanged: PropKey<ValueChangedEventArgs<'T> -> unit> =
      PropKey.Create.event (PropertyId.Create(288), "NumericUpDown.ValueChanged_event")

    member val ValueChangedUntyped: PropKey<ValueChangedEventArgs<System.Object> -> unit> =
      PropKey.Create.event (PropertyId.Create(289), "NumericUpDown.ValueChangedUntyped_event")

    member val ValueChanging: PropKey<ValueChangingEventArgs<'T> -> unit> =
      PropKey.Create.event (PropertyId.Create(290), "NumericUpDown.ValueChanging_event")

  type NumericUpDownPKeys() =
    inherit NumericUpDownPKeys<int>()


  type PaddingViewPKeys() =
    inherit AdornmentViewPKeys()


  type PopoverImplPKeys() =
    inherit ViewPKeys()

    // Properties
    member val Anchor: PropKey<Func<Nullable<System.Drawing.Rectangle>>> =
      PropKey.Create.simple (PropertyId.Create(291), "PopoverImpl.Anchor")

    member val Owner: PropKey<Terminal.Gui.App.IRunnable> =
      PropKey.Create.simple (PropertyId.Create(292), "PopoverImpl.Owner")

    member val Target: PropKey<WeakReference<Terminal.Gui.ViewBase.View>> =
      PropKey.Create.simple (PropertyId.Create(293), "PopoverImpl.Target")

  type PopoverPKeys<'TView, 'TResult when 'TView: (new: unit -> 'TView) and 'TView :> Terminal.Gui.ViewBase.View>() =
    inherit PopoverImplPKeys()

    // Properties
    member val ContentView: PropKey<'TView> =
      PropKey.Create.view (PropertyId.Create(294), PropertyId.Create(295), "Popover.ContentView_view")

    member val ContentView_viewSpec: PropKey<ITViewView> =
      PropKey.Create.subElement (PropertyId.Create(294), PropertyId.Create(295), "Popover.ContentView_viewSpec")

    member val ResultExtractor: PropKey<Func<'TView, 'TResult>> =
      PropKey.Create.simple (PropertyId.Create(296), "Popover.ResultExtractor")

    // Events
    member val ResultChanged: PropKey<ValueChangedEventArgs<'TResult> -> unit> =
      PropKey.Create.event (PropertyId.Create(297), "Popover.ResultChanged_event")

  type PopoverMenuPKeys() =
    inherit PopoverPKeys<Terminal.Gui.Views.Menu, Terminal.Gui.Views.MenuItem>()

    // Properties
    member val Key: PropKey<Terminal.Gui.Input.Key> = PropKey.Create.simple (PropertyId.Create(298), "PopoverMenu.Key")

    member val MouseFlags: PropKey<Terminal.Gui.Input.MouseFlags> =
      PropKey.Create.simple (PropertyId.Create(299), "PopoverMenu.MouseFlags")

    member val Root: PropKey<Terminal.Gui.Views.Menu> =
      PropKey.Create.view (PropertyId.Create(300), PropertyId.Create(301), "PopoverMenu.Root_view")

    member val Root_viewSpec: PropKey<IMenuView> =
      PropKey.Create.subElement (PropertyId.Create(300), PropertyId.Create(301), "PopoverMenu.Root_viewSpec")

    // Events
    member val KeyChanged: PropKey<Terminal.Gui.Input.KeyChangedEventArgs -> unit> =
      PropKey.Create.event (PropertyId.Create(302), "PopoverMenu.KeyChanged_event")

  type ProgressBarPKeys() =
    inherit ViewPKeys()

    // Properties
    member val BidirectionalMarquee: PropKey<bool> =
      PropKey.Create.simple (PropertyId.Create(303), "ProgressBar.BidirectionalMarquee")

    member val Fraction: PropKey<System.Single> = PropKey.Create.simple (PropertyId.Create(304), "ProgressBar.Fraction")

    member val ProgressBarFormat: PropKey<Terminal.Gui.Views.ProgressBarFormat> =
      PropKey.Create.simple (PropertyId.Create(305), "ProgressBar.ProgressBarFormat")

    member val ProgressBarStyle: PropKey<Terminal.Gui.Views.ProgressBarStyle> =
      PropKey.Create.simple (PropertyId.Create(306), "ProgressBar.ProgressBarStyle")

    member val SegmentCharacter: PropKey<System.Text.Rune> =
      PropKey.Create.simple (PropertyId.Create(307), "ProgressBar.SegmentCharacter")

    member val SyncWithTerminal: PropKey<bool> =
      PropKey.Create.simple (PropertyId.Create(308), "ProgressBar.SyncWithTerminal")

    member val Text: PropKey<string> = PropKey.Create.simple (PropertyId.Create(309), "ProgressBar.Text")

  type RunnablePKeys() =
    inherit ViewPKeys()

    // Properties
    member val Result: PropKey<System.Object> = PropKey.Create.simple (PropertyId.Create(310), "Runnable.Result")
    member val StopRequested: PropKey<bool> = PropKey.Create.simple (PropertyId.Create(311), "Runnable.StopRequested")

    // Events
    member val IsModalChanged: PropKey<EventArgs<bool> -> unit> =
      PropKey.Create.event (PropertyId.Create(312), "Runnable.IsModalChanged_event")

    member val IsRunningChanged: PropKey<EventArgs<bool> -> unit> =
      PropKey.Create.event (PropertyId.Create(313), "Runnable.IsRunningChanged_event")

    member val IsRunningChanging: PropKey<CancelEventArgs<bool> -> unit> =
      PropKey.Create.event (PropertyId.Create(314), "Runnable.IsRunningChanging_event")

  type RunnablePKeys<'TResult>() =
    inherit RunnablePKeys()

    // Properties
    member val Result: PropKey<'TResult> = PropKey.Create.simple (PropertyId.Create(315), "Runnable.Result")

  type DialogPKeys<'TResult>() =
    inherit RunnablePKeys<'TResult>()

    // Properties
    member val ButtonAlignment: PropKey<Terminal.Gui.ViewBase.Alignment> =
      PropKey.Create.simple (PropertyId.Create(316), "Dialog.ButtonAlignment")

    member val ButtonAlignmentModes: PropKey<Terminal.Gui.ViewBase.AlignmentModes> =
      PropKey.Create.simple (PropertyId.Create(317), "Dialog.ButtonAlignmentModes")

    member val Buttons: PropKey<Terminal.Gui.Views.Button[]> =
      PropKey.Create.simple (PropertyId.Create(318), "Dialog.Buttons")

  type RunnableWrapperPKeys<'TView, 'TResult when 'TView: (new: unit -> 'TView) and 'TView :> Terminal.Gui.ViewBase.View>
    () =
    inherit RunnablePKeys<'TResult>()

    // Properties
    member val ResultExtractor: PropKey<Func<'TView, 'TResult>> =
      PropKey.Create.simple (PropertyId.Create(319), "RunnableWrapper.ResultExtractor")

  type DialogPKeys() =
    inherit DialogPKeys<int>()

    // Properties
    member val Result: PropKey<Nullable<int>> = PropKey.Create.simple (PropertyId.Create(320), "Dialog.Result")

  type FileDialogPKeys() =
    inherit DialogPKeys<IReadOnlyList<string>>()

    // Properties
    member val AllowedTypes: PropKey<List<Terminal.Gui.Views.IAllowedType>> =
      PropKey.Create.simple (PropertyId.Create(321), "FileDialog.AllowedTypes")

    member val AllowsMultipleSelection: PropKey<bool> =
      PropKey.Create.simple (PropertyId.Create(322), "FileDialog.AllowsMultipleSelection")

    member val FileOperationsHandler: PropKey<Terminal.Gui.FileServices.IFileOperations> =
      PropKey.Create.simple (PropertyId.Create(323), "FileDialog.FileOperationsHandler")

    member val MustExist: PropKey<bool> = PropKey.Create.simple (PropertyId.Create(324), "FileDialog.MustExist")

    member val OpenMode: PropKey<Terminal.Gui.Views.OpenMode> =
      PropKey.Create.simple (PropertyId.Create(325), "FileDialog.OpenMode")

    member val Path: PropKey<string> = PropKey.Create.simple (PropertyId.Create(326), "FileDialog.Path")

    member val SearchMatcher: PropKey<Terminal.Gui.FileServices.ISearchMatcher> =
      PropKey.Create.simple (PropertyId.Create(327), "FileDialog.SearchMatcher")

    // Events
    member val FilesSelected: PropKey<Terminal.Gui.Views.FilesSelectedEventArgs -> unit> =
      PropKey.Create.event (PropertyId.Create(328), "FileDialog.FilesSelected_event")

  type PromptPKeys<'TView, 'TResult when 'TView: (new: unit -> 'TView) and 'TView :> Terminal.Gui.ViewBase.View>() =
    inherit DialogPKeys<'TResult>()

    // Properties
    member val ResultExtractor: PropKey<Func<'TView, 'TResult>> =
      PropKey.Create.simple (PropertyId.Create(329), "Prompt.ResultExtractor")

  type OpenDialogPKeys() =
    inherit FileDialogPKeys()

    // Properties
    member val OpenMode: PropKey<Terminal.Gui.Views.OpenMode> =
      PropKey.Create.simple (PropertyId.Create(330), "OpenDialog.OpenMode")

  type SaveDialogPKeys() =
    inherit FileDialogPKeys()


  type ScrollBarPKeys() =
    inherit ViewPKeys()

    // Properties
    member val Increment: PropKey<int> = PropKey.Create.simple (PropertyId.Create(331), "ScrollBar.Increment")

    member val Orientation: PropKey<Terminal.Gui.ViewBase.Orientation> =
      PropKey.Create.simple (PropertyId.Create(332), "ScrollBar.Orientation")

    member val ScrollableContentSize: PropKey<int> =
      PropKey.Create.simple (PropertyId.Create(333), "ScrollBar.ScrollableContentSize")

    member val Value: PropKey<int> = PropKey.Create.simple (PropertyId.Create(334), "ScrollBar.Value")

    member val VisibilityMode: PropKey<Terminal.Gui.Views.ScrollBarVisibilityMode> =
      PropKey.Create.simple (PropertyId.Create(335), "ScrollBar.VisibilityMode")

    member val VisibleContentSize: PropKey<int> =
      PropKey.Create.simple (PropertyId.Create(336), "ScrollBar.VisibleContentSize")

    // Events
    member val OrientationChanged: PropKey<EventArgs<Terminal.Gui.ViewBase.Orientation> -> unit> =
      PropKey.Create.event (PropertyId.Create(337), "ScrollBar.OrientationChanged_event")

    member val OrientationChanging: PropKey<CancelEventArgs<Terminal.Gui.ViewBase.Orientation> -> unit> =
      PropKey.Create.event (PropertyId.Create(338), "ScrollBar.OrientationChanging_event")

    member val ScrollableContentSizeChanged: PropKey<EventArgs<int> -> unit> =
      PropKey.Create.event (PropertyId.Create(339), "ScrollBar.ScrollableContentSizeChanged_event")

    member val Scrolled: PropKey<EventArgs<int> -> unit> =
      PropKey.Create.event (PropertyId.Create(340), "ScrollBar.Scrolled_event")

    member val SliderPositionChanged: PropKey<EventArgs<int> -> unit> =
      PropKey.Create.event (PropertyId.Create(341), "ScrollBar.SliderPositionChanged_event")

    member val ValueChanged: PropKey<ValueChangedEventArgs<int> -> unit> =
      PropKey.Create.event (PropertyId.Create(342), "ScrollBar.ValueChanged_event")

    member val ValueChangedUntyped: PropKey<ValueChangedEventArgs<System.Object> -> unit> =
      PropKey.Create.event (PropertyId.Create(343), "ScrollBar.ValueChangedUntyped_event")

    member val ValueChanging: PropKey<ValueChangingEventArgs<int> -> unit> =
      PropKey.Create.event (PropertyId.Create(344), "ScrollBar.ValueChanging_event")

  type ScrollButtonPKeys() =
    inherit ButtonPKeys()

    // Properties
    member val Direction: PropKey<Terminal.Gui.ViewBase.NavigationDirection> =
      PropKey.Create.simple (PropertyId.Create(345), "ScrollButton.Direction")

    member val Orientation: PropKey<Terminal.Gui.ViewBase.Orientation> =
      PropKey.Create.simple (PropertyId.Create(346), "ScrollButton.Orientation")

    // Events
    member val OrientationChanged: PropKey<EventArgs<Terminal.Gui.ViewBase.Orientation> -> unit> =
      PropKey.Create.event (PropertyId.Create(347), "ScrollButton.OrientationChanged_event")

    member val OrientationChanging: PropKey<CancelEventArgs<Terminal.Gui.ViewBase.Orientation> -> unit> =
      PropKey.Create.event (PropertyId.Create(348), "ScrollButton.OrientationChanging_event")

  type ScrollSliderPKeys() =
    inherit ViewPKeys()

    // Properties
    member val Orientation: PropKey<Terminal.Gui.ViewBase.Orientation> =
      PropKey.Create.simple (PropertyId.Create(349), "ScrollSlider.Orientation")

    member val Position: PropKey<int> = PropKey.Create.simple (PropertyId.Create(350), "ScrollSlider.Position")
    member val Size: PropKey<int> = PropKey.Create.simple (PropertyId.Create(351), "ScrollSlider.Size")

    member val SliderPadding: PropKey<int> =
      PropKey.Create.simple (PropertyId.Create(352), "ScrollSlider.SliderPadding")

    member val Value: PropKey<int> = PropKey.Create.simple (PropertyId.Create(353), "ScrollSlider.Value")

    member val VisibleContentSize: PropKey<int> =
      PropKey.Create.simple (PropertyId.Create(354), "ScrollSlider.VisibleContentSize")

    // Events
    member val OrientationChanged: PropKey<EventArgs<Terminal.Gui.ViewBase.Orientation> -> unit> =
      PropKey.Create.event (PropertyId.Create(355), "ScrollSlider.OrientationChanged_event")

    member val OrientationChanging: PropKey<CancelEventArgs<Terminal.Gui.ViewBase.Orientation> -> unit> =
      PropKey.Create.event (PropertyId.Create(356), "ScrollSlider.OrientationChanging_event")

    member val PositionChanged: PropKey<EventArgs<int> -> unit> =
      PropKey.Create.event (PropertyId.Create(357), "ScrollSlider.PositionChanged_event")

    member val PositionChanging: PropKey<CancelEventArgs<int> -> unit> =
      PropKey.Create.event (PropertyId.Create(358), "ScrollSlider.PositionChanging_event")

    member val Scrolled: PropKey<EventArgs<int> -> unit> =
      PropKey.Create.event (PropertyId.Create(359), "ScrollSlider.Scrolled_event")

    member val ValueChanged: PropKey<ValueChangedEventArgs<int> -> unit> =
      PropKey.Create.event (PropertyId.Create(360), "ScrollSlider.ValueChanged_event")

    member val ValueChangedUntyped: PropKey<ValueChangedEventArgs<System.Object> -> unit> =
      PropKey.Create.event (PropertyId.Create(361), "ScrollSlider.ValueChangedUntyped_event")

    member val ValueChanging: PropKey<ValueChangingEventArgs<int> -> unit> =
      PropKey.Create.event (PropertyId.Create(362), "ScrollSlider.ValueChanging_event")

  type SelectorBasePKeys() =
    inherit ViewPKeys()

    // Properties
    member val DoubleClickAccepts: PropKey<bool> =
      PropKey.Create.simple (PropertyId.Create(363), "SelectorBase.DoubleClickAccepts")

    member val HorizontalSpace: PropKey<int> =
      PropKey.Create.simple (PropertyId.Create(364), "SelectorBase.HorizontalSpace")

    member val Labels: PropKey<IReadOnlyList<string>> =
      PropKey.Create.simple (PropertyId.Create(365), "SelectorBase.Labels")

    member val Orientation: PropKey<Terminal.Gui.ViewBase.Orientation> =
      PropKey.Create.simple (PropertyId.Create(366), "SelectorBase.Orientation")

    member val Styles: PropKey<Terminal.Gui.Views.SelectorStyles> =
      PropKey.Create.simple (PropertyId.Create(367), "SelectorBase.Styles")

    member val TabBehavior: PropKey<Nullable<Terminal.Gui.ViewBase.TabBehavior>> =
      PropKey.Create.simple (PropertyId.Create(368), "SelectorBase.TabBehavior")

    member val Value: PropKey<Nullable<int>> = PropKey.Create.simple (PropertyId.Create(369), "SelectorBase.Value")

    member val Values: PropKey<IReadOnlyList<int>> =
      PropKey.Create.simple (PropertyId.Create(370), "SelectorBase.Values")

    // Events
    member val OrientationChanged: PropKey<EventArgs<Terminal.Gui.ViewBase.Orientation> -> unit> =
      PropKey.Create.event (PropertyId.Create(371), "SelectorBase.OrientationChanged_event")

    member val OrientationChanging: PropKey<CancelEventArgs<Terminal.Gui.ViewBase.Orientation> -> unit> =
      PropKey.Create.event (PropertyId.Create(372), "SelectorBase.OrientationChanging_event")

    member val ValueChanged: PropKey<ValueChangedEventArgs<Nullable<int>> -> unit> =
      PropKey.Create.event (PropertyId.Create(373), "SelectorBase.ValueChanged_event")

    member val ValueChangedUntyped: PropKey<ValueChangedEventArgs<System.Object> -> unit> =
      PropKey.Create.event (PropertyId.Create(374), "SelectorBase.ValueChangedUntyped_event")

    member val ValueChanging: PropKey<ValueChangingEventArgs<Nullable<int>> -> unit> =
      PropKey.Create.event (PropertyId.Create(375), "SelectorBase.ValueChanging_event")

  type FlagSelectorPKeys() =
    inherit SelectorBasePKeys()

    // Properties
    member val Value: PropKey<Nullable<int>> = PropKey.Create.simple (PropertyId.Create(376), "FlagSelector.Value")

  type OptionSelectorPKeys() =
    inherit SelectorBasePKeys()

    // Properties
    member val FocusedItem: PropKey<int> = PropKey.Create.simple (PropertyId.Create(377), "OptionSelector.FocusedItem")

  type FlagSelectorPKeys<'TFlagsEnum
    when 'TFlagsEnum: struct
    and 'TFlagsEnum: (new: unit -> 'TFlagsEnum)
    and 'TFlagsEnum :> System.Enum
    and 'TFlagsEnum :> System.ValueType>() =
    inherit FlagSelectorPKeys()

    // Properties
    member val Value: PropKey<Nullable<'TFlagsEnum>> =
      PropKey.Create.simple (PropertyId.Create(378), "FlagSelector.Value")

    // Events
    member val ValueChanged: PropKey<EventArgs<Nullable<'TFlagsEnum>> -> unit> =
      PropKey.Create.event (PropertyId.Create(379), "FlagSelector.ValueChanged_event")

  type OptionSelectorPKeys<'TEnum
    when 'TEnum: struct and 'TEnum: (new: unit -> 'TEnum) and 'TEnum :> System.Enum and 'TEnum :> System.ValueType>() =
    inherit OptionSelectorPKeys()

    // Properties
    member val Value: PropKey<Nullable<'TEnum>> = PropKey.Create.simple (PropertyId.Create(380), "OptionSelector.Value")

    member val Values: PropKey<IReadOnlyList<int>> =
      PropKey.Create.simple (PropertyId.Create(381), "OptionSelector.Values")

    // Events
    member val ValueChanged: PropKey<EventArgs<Nullable<'TEnum>> -> unit> =
      PropKey.Create.event (PropertyId.Create(382), "OptionSelector.ValueChanged_event")

  type ShortcutPKeys() =
    inherit ViewPKeys()

    // Properties
    member val Action: PropKey<System.Action> = PropKey.Create.simple (PropertyId.Create(383), "Shortcut.Action")

    member val AlignmentModes: PropKey<Terminal.Gui.ViewBase.AlignmentModes> =
      PropKey.Create.simple (PropertyId.Create(384), "Shortcut.AlignmentModes")

    member val BindKeyToApplication: PropKey<bool> =
      PropKey.Create.simple (PropertyId.Create(385), "Shortcut.BindKeyToApplication")

    member val Command: PropKey<Terminal.Gui.Input.Command> =
      PropKey.Create.simple (PropertyId.Create(386), "Shortcut.Command")

    member val CommandView: PropKey<Terminal.Gui.ViewBase.View> =
      PropKey.Create.view (PropertyId.Create(387), PropertyId.Create(388), "Shortcut.CommandView_view")

    member val CommandView_viewSpec: PropKey<IView> =
      PropKey.Create.subElement (PropertyId.Create(387), PropertyId.Create(388), "Shortcut.CommandView_viewSpec")

    member val HelpText: PropKey<string> = PropKey.Create.simple (PropertyId.Create(389), "Shortcut.HelpText")
    member val Key: PropKey<Terminal.Gui.Input.Key> = PropKey.Create.simple (PropertyId.Create(390), "Shortcut.Key")

    member val MinimumKeyTextSize: PropKey<int> =
      PropKey.Create.simple (PropertyId.Create(391), "Shortcut.MinimumKeyTextSize")

    member val Orientation: PropKey<Terminal.Gui.ViewBase.Orientation> =
      PropKey.Create.simple (PropertyId.Create(392), "Shortcut.Orientation")

    member val TargetView: PropKey<Terminal.Gui.ViewBase.View> =
      PropKey.Create.view (PropertyId.Create(393), PropertyId.Create(394), "Shortcut.TargetView_view")

    member val TargetView_viewSpec: PropKey<IView> =
      PropKey.Create.subElement (PropertyId.Create(393), PropertyId.Create(394), "Shortcut.TargetView_viewSpec")

    member val Text: PropKey<string> = PropKey.Create.simple (PropertyId.Create(395), "Shortcut.Text")

    // Events
    member val OrientationChanged: PropKey<EventArgs<Terminal.Gui.ViewBase.Orientation> -> unit> =
      PropKey.Create.event (PropertyId.Create(396), "Shortcut.OrientationChanged_event")

    member val OrientationChanging: PropKey<CancelEventArgs<Terminal.Gui.ViewBase.Orientation> -> unit> =
      PropKey.Create.event (PropertyId.Create(397), "Shortcut.OrientationChanging_event")

  type MenuItemPKeys() =
    inherit ShortcutPKeys()

    // Properties
    member val SubMenu: PropKey<Terminal.Gui.Views.Menu> =
      PropKey.Create.view (PropertyId.Create(398), PropertyId.Create(399), "MenuItem.SubMenu_view")

    member val SubMenu_viewSpec: PropKey<IMenuView> =
      PropKey.Create.subElement (PropertyId.Create(398), PropertyId.Create(399), "MenuItem.SubMenu_viewSpec")

  type MenuBarItemPKeys() =
    inherit MenuItemPKeys()

    // Properties
    member val PopoverMenu: PropKey<Terminal.Gui.Views.PopoverMenu> =
      PropKey.Create.view (PropertyId.Create(400), PropertyId.Create(401), "MenuBarItem.PopoverMenu_view")

    member val PopoverMenu_viewSpec: PropKey<IPopoverMenuView> =
      PropKey.Create.subElement (PropertyId.Create(400), PropertyId.Create(401), "MenuBarItem.PopoverMenu_viewSpec")

    member val PopoverMenuOpen: PropKey<bool> =
      PropKey.Create.simple (PropertyId.Create(402), "MenuBarItem.PopoverMenuOpen")

    // Events
    member val MenuOpenChanged: PropKey<ValueChangedEventArgs<bool> -> unit> =
      PropKey.Create.event (PropertyId.Create(403), "MenuBarItem.MenuOpenChanged_event")

    member val PopoverMenuOpenChanged: PropKey<ValueChangedEventArgs<bool> -> unit> =
      PropKey.Create.event (PropertyId.Create(404), "MenuBarItem.PopoverMenuOpenChanged_event")

  type SpinnerViewPKeys() =
    inherit ViewPKeys()

    // Properties
    member val AutoSpin: PropKey<bool> = PropKey.Create.simple (PropertyId.Create(405), "SpinnerView.AutoSpin")

    member val Sequence: PropKey<System.String[]> =
      PropKey.Create.simple (PropertyId.Create(406), "SpinnerView.Sequence")

    member val SpinBounce: PropKey<bool> = PropKey.Create.simple (PropertyId.Create(407), "SpinnerView.SpinBounce")
    member val SpinDelay: PropKey<int> = PropKey.Create.simple (PropertyId.Create(408), "SpinnerView.SpinDelay")
    member val SpinReverse: PropKey<bool> = PropKey.Create.simple (PropertyId.Create(409), "SpinnerView.SpinReverse")

    member val Style: PropKey<Terminal.Gui.Views.SpinnerStyle> =
      PropKey.Create.simple (PropertyId.Create(410), "SpinnerView.Style")

    member val SyncWithTerminal: PropKey<bool> =
      PropKey.Create.simple (PropertyId.Create(411), "SpinnerView.SyncWithTerminal")

  type StatusBarPKeys() =
    inherit BarPKeys()


  type TableViewPKeys() =
    inherit ViewPKeys()

    // Properties
    member val CollectionNavigator: PropKey<Terminal.Gui.Views.ICollectionNavigator> =
      PropKey.Create.simple (PropertyId.Create(412), "TableView.CollectionNavigator")

    member val ColumnOffset: PropKey<int> = PropKey.Create.simple (PropertyId.Create(413), "TableView.ColumnOffset")
    member val FullRowSelect: PropKey<bool> = PropKey.Create.simple (PropertyId.Create(414), "TableView.FullRowSelect")
    member val MaxCellWidth: PropKey<int> = PropKey.Create.simple (PropertyId.Create(415), "TableView.MaxCellWidth")
    member val MinCellWidth: PropKey<int> = PropKey.Create.simple (PropertyId.Create(416), "TableView.MinCellWidth")
    member val MultiSelect: PropKey<bool> = PropKey.Create.simple (PropertyId.Create(417), "TableView.MultiSelect")
    member val NullSymbol: PropKey<string> = PropKey.Create.simple (PropertyId.Create(418), "TableView.NullSymbol")
    member val RowOffset: PropKey<int> = PropKey.Create.simple (PropertyId.Create(419), "TableView.RowOffset")

    member val SeparatorSymbol: PropKey<System.Char> =
      PropKey.Create.simple (PropertyId.Create(420), "TableView.SeparatorSymbol")

    member val Style: PropKey<Terminal.Gui.Views.TableStyle> =
      PropKey.Create.simple (PropertyId.Create(421), "TableView.Style")

    member val Table: PropKey<Terminal.Gui.Views.ITableSource> =
      PropKey.Create.simple (PropertyId.Create(422), "TableView.Table")

    member val UseAllRowsForContentCalculation: PropKey<bool> =
      PropKey.Create.simple (PropertyId.Create(423), "TableView.UseAllRowsForContentCalculation")

    member val Value: PropKey<Terminal.Gui.Views.TableSelection> =
      PropKey.Create.simple (PropertyId.Create(424), "TableView.Value")

    // Events
    member val ValueChanged: PropKey<ValueChangedEventArgs<Terminal.Gui.Views.TableSelection> -> unit> =
      PropKey.Create.event (PropertyId.Create(425), "TableView.ValueChanged_event")

    member val ValueChangedUntyped: PropKey<ValueChangedEventArgs<System.Object> -> unit> =
      PropKey.Create.event (PropertyId.Create(426), "TableView.ValueChangedUntyped_event")

    member val ValueChanging: PropKey<ValueChangingEventArgs<Terminal.Gui.Views.TableSelection> -> unit> =
      PropKey.Create.event (PropertyId.Create(427), "TableView.ValueChanging_event")

  type TabsPKeys() =
    inherit ViewPKeys()

    // Properties
    member val ScrollOffset: PropKey<int> = PropKey.Create.simple (PropertyId.Create(428), "Tabs.ScrollOffset")
    member val TabDepth: PropKey<int> = PropKey.Create.simple (PropertyId.Create(429), "Tabs.TabDepth")

    member val TabLineStyle: PropKey<Terminal.Gui.Drawing.LineStyle> =
      PropKey.Create.simple (PropertyId.Create(430), "Tabs.TabLineStyle")

    member val TabSide: PropKey<Terminal.Gui.ViewBase.Side> =
      PropKey.Create.simple (PropertyId.Create(431), "Tabs.TabSide")

    member val TabSpacing: PropKey<int> = PropKey.Create.simple (PropertyId.Create(432), "Tabs.TabSpacing")

    member val Value: PropKey<Terminal.Gui.ViewBase.View> =
      PropKey.Create.view (PropertyId.Create(433), PropertyId.Create(434), "Tabs.Value_view")

    member val Value_viewSpec: PropKey<IView> =
      PropKey.Create.subElement (PropertyId.Create(433), PropertyId.Create(434), "Tabs.Value_viewSpec")

    // Events
    member val ValueChanged: PropKey<ValueChangedEventArgs<Terminal.Gui.ViewBase.View> -> unit> =
      PropKey.Create.event (PropertyId.Create(435), "Tabs.ValueChanged_event")

    member val ValueChangedUntyped: PropKey<ValueChangedEventArgs<System.Object> -> unit> =
      PropKey.Create.event (PropertyId.Create(436), "Tabs.ValueChangedUntyped_event")

    member val ValueChanging: PropKey<ValueChangingEventArgs<Terminal.Gui.ViewBase.View> -> unit> =
      PropKey.Create.event (PropertyId.Create(437), "Tabs.ValueChanging_event")

  type TextFieldPKeys() =
    inherit ViewPKeys()

    // Properties
    member val Autocomplete: PropKey<Terminal.Gui.Views.IAutocomplete> =
      PropKey.Create.simple (PropertyId.Create(438), "TextField.Autocomplete")

    member val InsertionPoint: PropKey<int> = PropKey.Create.simple (PropertyId.Create(439), "TextField.InsertionPoint")
    member val ReadOnly: PropKey<bool> = PropKey.Create.simple (PropertyId.Create(440), "TextField.ReadOnly")
    member val Secret: PropKey<bool> = PropKey.Create.simple (PropertyId.Create(441), "TextField.Secret")

    member val SelectWordOnlyOnDoubleClick: PropKey<bool> =
      PropKey.Create.simple (PropertyId.Create(442), "TextField.SelectWordOnlyOnDoubleClick")

    member val SelectedStart: PropKey<int> = PropKey.Create.simple (PropertyId.Create(443), "TextField.SelectedStart")
    member val Text: PropKey<string> = PropKey.Create.simple (PropertyId.Create(444), "TextField.Text")

    member val UseSameRuneTypeForWords: PropKey<bool> =
      PropKey.Create.simple (PropertyId.Create(445), "TextField.UseSameRuneTypeForWords")

    member val Used: PropKey<bool> = PropKey.Create.simple (PropertyId.Create(446), "TextField.Used")
    member val Value: PropKey<string> = PropKey.Create.simple (PropertyId.Create(447), "TextField.Value")

    // Events
    member val TextChanging: PropKey<ResultEventArgs<string> -> unit> =
      PropKey.Create.event (PropertyId.Create(448), "TextField.TextChanging_event")

    member val ValueChanged: PropKey<ValueChangedEventArgs<string> -> unit> =
      PropKey.Create.event (PropertyId.Create(449), "TextField.ValueChanged_event")

    member val ValueChangedUntyped: PropKey<ValueChangedEventArgs<System.Object> -> unit> =
      PropKey.Create.event (PropertyId.Create(450), "TextField.ValueChangedUntyped_event")

    member val ValueChanging: PropKey<ValueChangingEventArgs<string> -> unit> =
      PropKey.Create.event (PropertyId.Create(451), "TextField.ValueChanging_event")

  type DropDownListPKeys() =
    inherit TextFieldPKeys()

    // Properties
    member val Source: PropKey<Terminal.Gui.Views.IListDataSource> =
      PropKey.Create.simple (PropertyId.Create(452), "DropDownList.Source")

  type DropDownListPKeys<'TEnum
    when 'TEnum: struct and 'TEnum: (new: unit -> 'TEnum) and 'TEnum :> System.Enum and 'TEnum :> System.ValueType>() =
    inherit DropDownListPKeys()

    // Properties
    member val Value: PropKey<Nullable<'TEnum>> = PropKey.Create.simple (PropertyId.Create(453), "DropDownList.Value")

    // Events
    member val ValueChanged: PropKey<EventArgs<Nullable<'TEnum>> -> unit> =
      PropKey.Create.event (PropertyId.Create(454), "DropDownList.ValueChanged_event")

  type TextValidateFieldPKeys() =
    inherit ViewPKeys()

    // Properties
    member val Provider: PropKey<Terminal.Gui.Views.ITextValidateProvider> =
      PropKey.Create.simple (PropertyId.Create(455), "TextValidateField.Provider")

    member val Text: PropKey<string> = PropKey.Create.simple (PropertyId.Create(456), "TextValidateField.Text")
    member val Value: PropKey<string> = PropKey.Create.simple (PropertyId.Create(457), "TextValidateField.Value")

    // Events
    member val ValueChanged: PropKey<ValueChangedEventArgs<string> -> unit> =
      PropKey.Create.event (PropertyId.Create(458), "TextValidateField.ValueChanged_event")

    member val ValueChangedUntyped: PropKey<ValueChangedEventArgs<System.Object> -> unit> =
      PropKey.Create.event (PropertyId.Create(459), "TextValidateField.ValueChangedUntyped_event")

    member val ValueChanging: PropKey<ValueChangingEventArgs<string> -> unit> =
      PropKey.Create.event (PropertyId.Create(460), "TextValidateField.ValueChanging_event")

  type DateEditorPKeys() =
    inherit TextValidateFieldPKeys()

    // Properties
    member val Format: PropKey<System.Globalization.DateTimeFormatInfo> =
      PropKey.Create.simple (PropertyId.Create(461), "DateEditor.Format")

    member val Value: PropKey<System.DateTime> = PropKey.Create.simple (PropertyId.Create(462), "DateEditor.Value")

    // Events
    member val ValueChanged: PropKey<ValueChangedEventArgs<System.DateTime> -> unit> =
      PropKey.Create.event (PropertyId.Create(463), "DateEditor.ValueChanged_event")

    member val ValueChangedUntyped: PropKey<ValueChangedEventArgs<System.Object> -> unit> =
      PropKey.Create.event (PropertyId.Create(464), "DateEditor.ValueChangedUntyped_event")

    member val ValueChanging: PropKey<ValueChangingEventArgs<System.DateTime> -> unit> =
      PropKey.Create.event (PropertyId.Create(465), "DateEditor.ValueChanging_event")

  type TextViewPKeys() =
    inherit ViewPKeys()

    // Properties
    member val EnterKeyAddsLine: PropKey<bool> =
      PropKey.Create.simple (PropertyId.Create(466), "TextView.EnterKeyAddsLine")

    member val InheritsPreviousAttribute: PropKey<bool> =
      PropKey.Create.simple (PropertyId.Create(467), "TextView.InheritsPreviousAttribute")

    member val InsertionPoint: PropKey<System.Drawing.Point> =
      PropKey.Create.simple (PropertyId.Create(468), "TextView.InsertionPoint")

    member val IsSelecting: PropKey<bool> = PropKey.Create.simple (PropertyId.Create(469), "TextView.IsSelecting")
    member val Multiline: PropKey<bool> = PropKey.Create.simple (PropertyId.Create(470), "TextView.Multiline")
    member val ReadOnly: PropKey<bool> = PropKey.Create.simple (PropertyId.Create(471), "TextView.ReadOnly")
    member val ScrollBars: PropKey<bool> = PropKey.Create.simple (PropertyId.Create(472), "TextView.ScrollBars")

    member val SelectWordOnlyOnDoubleClick: PropKey<bool> =
      PropKey.Create.simple (PropertyId.Create(473), "TextView.SelectWordOnlyOnDoubleClick")

    member val SelectionStartColumn: PropKey<int> =
      PropKey.Create.simple (PropertyId.Create(474), "TextView.SelectionStartColumn")

    member val SelectionStartRow: PropKey<int> =
      PropKey.Create.simple (PropertyId.Create(475), "TextView.SelectionStartRow")

    member val TabKeyAddsTab: PropKey<bool> = PropKey.Create.simple (PropertyId.Create(476), "TextView.TabKeyAddsTab")
    member val TabWidth: PropKey<int> = PropKey.Create.simple (PropertyId.Create(477), "TextView.TabWidth")
    member val Text: PropKey<string> = PropKey.Create.simple (PropertyId.Create(478), "TextView.Text")

    member val UseSameRuneTypeForWords: PropKey<bool> =
      PropKey.Create.simple (PropertyId.Create(479), "TextView.UseSameRuneTypeForWords")

    member val Used: PropKey<bool> = PropKey.Create.simple (PropertyId.Create(480), "TextView.Used")
    member val WordWrap: PropKey<bool> = PropKey.Create.simple (PropertyId.Create(481), "TextView.WordWrap")

    // Events
    member val ContentsChanged: PropKey<Terminal.Gui.Views.ContentsChangedEventArgs -> unit> =
      PropKey.Create.event (PropertyId.Create(482), "TextView.ContentsChanged_event")

    member val DrawNormalColor: PropKey<Terminal.Gui.Drawing.CellEventArgs -> unit> =
      PropKey.Create.event (PropertyId.Create(483), "TextView.DrawNormalColor_event")

    member val DrawReadOnlyColor: PropKey<Terminal.Gui.Drawing.CellEventArgs -> unit> =
      PropKey.Create.event (PropertyId.Create(484), "TextView.DrawReadOnlyColor_event")

    member val DrawSelectionColor: PropKey<Terminal.Gui.Drawing.CellEventArgs -> unit> =
      PropKey.Create.event (PropertyId.Create(485), "TextView.DrawSelectionColor_event")

    member val DrawUsedColor: PropKey<Terminal.Gui.Drawing.CellEventArgs -> unit> =
      PropKey.Create.event (PropertyId.Create(486), "TextView.DrawUsedColor_event")

    member val UnwrappedCursorPositionChanged: PropKey<System.Drawing.Point -> unit> =
      PropKey.Create.event (PropertyId.Create(487), "TextView.UnwrappedCursorPositionChanged_event")

  type TimeEditorPKeys() =
    inherit TextValidateFieldPKeys()

    // Properties
    member val Format: PropKey<System.Globalization.DateTimeFormatInfo> =
      PropKey.Create.simple (PropertyId.Create(488), "TimeEditor.Format")

    member val Value: PropKey<System.TimeSpan> = PropKey.Create.simple (PropertyId.Create(489), "TimeEditor.Value")

    // Events
    member val ValueChanged: PropKey<ValueChangedEventArgs<System.TimeSpan> -> unit> =
      PropKey.Create.event (PropertyId.Create(490), "TimeEditor.ValueChanged_event")

    member val ValueChangedUntyped: PropKey<ValueChangedEventArgs<System.Object> -> unit> =
      PropKey.Create.event (PropertyId.Create(491), "TimeEditor.ValueChangedUntyped_event")

    member val ValueChanging: PropKey<ValueChangingEventArgs<System.TimeSpan> -> unit> =
      PropKey.Create.event (PropertyId.Create(492), "TimeEditor.ValueChanging_event")

  type TitleViewPKeys() =
    inherit ViewPKeys()

    // Properties
    member val Direction: PropKey<Terminal.Gui.ViewBase.NavigationDirection> =
      PropKey.Create.simple (PropertyId.Create(493), "TitleView.Direction")

    member val MeasuredTabLength: PropKey<int> =
      PropKey.Create.simple (PropertyId.Create(494), "TitleView.MeasuredTabLength")

    member val Orientation: PropKey<Terminal.Gui.ViewBase.Orientation> =
      PropKey.Create.simple (PropertyId.Create(495), "TitleView.Orientation")

    member val TabDepth: PropKey<int> = PropKey.Create.simple (PropertyId.Create(496), "TitleView.TabDepth")

    member val TabSide: PropKey<Terminal.Gui.ViewBase.Side> =
      PropKey.Create.simple (PropertyId.Create(497), "TitleView.TabSide")

    member val Text: PropKey<string> = PropKey.Create.simple (PropertyId.Create(498), "TitleView.Text")

    // Events
    member val OrientationChanged: PropKey<EventArgs<Terminal.Gui.ViewBase.Orientation> -> unit> =
      PropKey.Create.event (PropertyId.Create(499), "TitleView.OrientationChanged_event")

    member val OrientationChanging: PropKey<CancelEventArgs<Terminal.Gui.ViewBase.Orientation> -> unit> =
      PropKey.Create.event (PropertyId.Create(500), "TitleView.OrientationChanging_event")

  type ToolTipHostPKeys<'TView when 'TView: (new: unit -> 'TView) and 'TView :> Terminal.Gui.ViewBase.View>() =
    inherit PopoverImplPKeys()

    // Properties
    member val ContentView: PropKey<'TView> =
      PropKey.Create.view (PropertyId.Create(501), PropertyId.Create(502), "ToolTipHost.ContentView_view")

    member val ContentView_viewSpec: PropKey<ITViewView> =
      PropKey.Create.subElement (PropertyId.Create(501), PropertyId.Create(502), "ToolTipHost.ContentView_viewSpec")

  type TreeViewPKeys<'T when 'T: not struct>() =
    inherit ViewPKeys()

    // Properties
    member val AllowLetterBasedNavigation: PropKey<bool> =
      PropKey.Create.simple (PropertyId.Create(503), "TreeView.AllowLetterBasedNavigation")

    member val AspectGetter: PropKey<AspectGetterDelegate<'T>> =
      PropKey.Create.simple (PropertyId.Create(504), "TreeView.AspectGetter")

    member val CheckboxMode: PropKey<bool> = PropKey.Create.simple (PropertyId.Create(505), "TreeView.CheckboxMode")

    member val ColorGetter: PropKey<Func<'T, Terminal.Gui.Drawing.Scheme>> =
      PropKey.Create.simple (PropertyId.Create(506), "TreeView.ColorGetter")

    member val Filter: PropKey<ITreeViewFilter<'T>> = PropKey.Create.simple (PropertyId.Create(507), "TreeView.Filter")
    member val MaxDepth: PropKey<int> = PropKey.Create.simple (PropertyId.Create(508), "TreeView.MaxDepth")
    member val MultiSelect: PropKey<bool> = PropKey.Create.simple (PropertyId.Create(509), "TreeView.MultiSelect")

    member val ScrollOffsetHorizontal: PropKey<int> =
      PropKey.Create.simple (PropertyId.Create(510), "TreeView.ScrollOffsetHorizontal")

    member val ScrollOffsetVertical: PropKey<int> =
      PropKey.Create.simple (PropertyId.Create(511), "TreeView.ScrollOffsetVertical")

    member val SelectedObject: PropKey<'T> = PropKey.Create.simple (PropertyId.Create(512), "TreeView.SelectedObject")

    member val Style: PropKey<Terminal.Gui.Views.TreeStyle> =
      PropKey.Create.simple (PropertyId.Create(513), "TreeView.Style")

    member val TreeBuilder: PropKey<ITreeBuilder<'T>> =
      PropKey.Create.simple (PropertyId.Create(514), "TreeView.TreeBuilder")

    // Events
    member val CheckedChanged: PropKey<CheckedChangedEventArgs<'T> -> unit> =
      PropKey.Create.event (PropertyId.Create(515), "TreeView.CheckedChanged_event")

    member val DrawLine: PropKey<DrawTreeViewLineEventArgs<'T> -> unit> =
      PropKey.Create.event (PropertyId.Create(516), "TreeView.DrawLine_event")

    member val SelectionChanged: PropKey<SelectionChangedEventArgs<'T> -> unit> =
      PropKey.Create.event (PropertyId.Create(517), "TreeView.SelectionChanged_event")

  type TreeViewPKeys() =
    inherit TreeViewPKeys<Terminal.Gui.Views.ITreeNode>()


  type WindowPKeys() =
    inherit RunnablePKeys()


  type WizardPKeys() =
    inherit DialogPKeys()

    // Properties
    member val CurrentStep: PropKey<Terminal.Gui.Views.WizardStep> =
      PropKey.Create.view (PropertyId.Create(518), PropertyId.Create(519), "Wizard.CurrentStep_view")

    member val CurrentStep_viewSpec: PropKey<IWizardStepView> =
      PropKey.Create.subElement (PropertyId.Create(518), PropertyId.Create(519), "Wizard.CurrentStep_viewSpec")

    // Events
    member val MovingBack: PropKey<System.ComponentModel.CancelEventArgs -> unit> =
      PropKey.Create.event (PropertyId.Create(520), "Wizard.MovingBack_event")

    member val MovingNext: PropKey<System.ComponentModel.CancelEventArgs -> unit> =
      PropKey.Create.event (PropertyId.Create(521), "Wizard.MovingNext_event")

    member val StepChanged: PropKey<ValueChangedEventArgs<Terminal.Gui.Views.WizardStep> -> unit> =
      PropKey.Create.event (PropertyId.Create(522), "Wizard.StepChanged_event")

    member val StepChanging: PropKey<ValueChangingEventArgs<Terminal.Gui.Views.WizardStep> -> unit> =
      PropKey.Create.event (PropertyId.Create(523), "Wizard.StepChanging_event")

  type WizardStepPKeys() =
    inherit ViewPKeys()

    // Properties
    member val BackButtonText: PropKey<string> =
      PropKey.Create.simple (PropertyId.Create(524), "WizardStep.BackButtonText")

    member val HelpText: PropKey<string> = PropKey.Create.simple (PropertyId.Create(525), "WizardStep.HelpText")

    member val NextButtonText: PropKey<string> =
      PropKey.Create.simple (PropertyId.Create(526), "WizardStep.NextButtonText")

  module internal IAdornmentInterface =
    // Properties
    let Parent: PropKey<Terminal.Gui.ViewBase.View> =
      PropKey.Create.simple (PropertyId.Create(527), "IAdornmentInterface.Parent")

    let Thickness: PropKey<Terminal.Gui.Drawing.Thickness> =
      PropKey.Create.simple (PropertyId.Create(528), "IAdornmentInterface.Thickness")

    // Events
    let ThicknessChanged: PropKey<System.EventArgs -> unit> =
      PropKey.Create.event (PropertyId.Create(529), "IAdornmentInterface.ThicknessChanged_event")

  module internal IAdornmentViewInterface =
    // Properties
    let Adornment: PropKey<Terminal.Gui.ViewBase.IAdornment> =
      PropKey.Create.simple (PropertyId.Create(530), "IAdornmentViewInterface.Adornment")

  module internal IMouseHoldRepeaterInterface =
    // Properties
    let Timeout: PropKey<Terminal.Gui.App.Timeout> =
      PropKey.Create.simple (PropertyId.Create(531), "IMouseHoldRepeaterInterface.Timeout")

    // Events
    let MouseIsHeldDownTick: PropKey<CancelEventArgs<Terminal.Gui.Input.Mouse> -> unit> =
      PropKey.Create.event (PropertyId.Create(532), "IMouseHoldRepeaterInterface.MouseIsHeldDownTick_event")

  module internal IOrientationInterface =
    // Properties
    let Orientation: PropKey<Terminal.Gui.ViewBase.Orientation> =
      PropKey.Create.simple (PropertyId.Create(533), "IOrientationInterface.Orientation")

    // Events
    let OrientationChanged: PropKey<EventArgs<Terminal.Gui.ViewBase.Orientation> -> unit> =
      PropKey.Create.event (PropertyId.Create(534), "IOrientationInterface.OrientationChanged_event")

    let OrientationChanging: PropKey<CancelEventArgs<Terminal.Gui.ViewBase.Orientation> -> unit> =
      PropKey.Create.event (PropertyId.Create(535), "IOrientationInterface.OrientationChanging_event")

  module internal ITitleViewInterface =
    // Properties
    let MeasuredTabLength: PropKey<int> =
      PropKey.Create.simple (PropertyId.Create(536), "ITitleViewInterface.MeasuredTabLength")

    let TabDepth: PropKey<int> =
      PropKey.Create.simple (PropertyId.Create(537), "ITitleViewInterface.TabDepth")

    let TabSide: PropKey<Terminal.Gui.ViewBase.Side> =
      PropKey.Create.simple (PropertyId.Create(538), "ITitleViewInterface.TabSide")

  module internal IValueInterface =
    // Properties
    let Value<'TValue> : PropKey<'TValue> =
      PropKey.Create.simple (PropertyId.Create(539), "IValueInterface.Value")

    // Events
    let ValueChangedUntyped: PropKey<ValueChangedEventArgs<System.Object> -> unit> =
      PropKey.Create.event (PropertyId.Create(540), "IValueInterface.ValueChangedUntyped_event")

    let ValueChanged<'TValue> : PropKey<ValueChangedEventArgs<'TValue> -> unit> =
      PropKey.Create.event (PropertyId.Create(541), "IValueInterface.ValueChanged_event")

    let ValueChanging<'TValue> : PropKey<ValueChangingEventArgs<'TValue> -> unit> =
      PropKey.Create.event (PropertyId.Create(542), "IValueInterface.ValueChanging_event")


  let View = ViewPKeys()
  let AdornmentView = AdornmentViewPKeys()
  let AttributePicker = AttributePickerPKeys()
  let Bar = BarPKeys()
  let BorderView = BorderViewPKeys()
  let Button = ButtonPKeys()
  let CharMap = CharMapPKeys()
  let CheckBox = CheckBoxPKeys()
  let Code = CodePKeys()
  let ColorPicker = ColorPickerPKeys()
  let ColorPicker16 = ColorPicker16PKeys()
  let DatePicker = DatePickerPKeys()
  let FrameView = FrameViewPKeys()
  let GraphView = GraphViewPKeys()
  let HexView = HexViewPKeys()
  let ImageView = ImageViewPKeys()
  let Label = LabelPKeys()
  let LegendAnnotation = LegendAnnotationPKeys()
  let Line = LinePKeys()

  let LinearRangeViewBase<'TOption, 'TValue> =
    LinearRangeViewBasePKeys<'TOption, 'TValue>()

  let LinearMultiSelector<'T> = LinearMultiSelectorPKeys<'T>()
  let LinearMultiSelector' = LinearMultiSelectorPKeys()
  let LinearRange<'T> = LinearRangePKeys<'T>()
  let LinearRange' = LinearRangePKeys()
  let LinearSelector<'T> = LinearSelectorPKeys<'T>()
  let LinearSelector' = LinearSelectorPKeys()
  let Link = LinkPKeys()
  let ListView = ListViewPKeys()
  let ListView'<'T> = ListViewPKeys<'T>()
  let MarginView = MarginViewPKeys()
  let Markdown = MarkdownPKeys()
  let MarkdownCodeBlock = MarkdownCodeBlockPKeys()
  let MarkdownTable = MarkdownTablePKeys()
  let Menu = MenuPKeys()
  let MenuBar = MenuBarPKeys()
  let NumericUpDown<'T> = NumericUpDownPKeys<'T>()
  let NumericUpDown' = NumericUpDownPKeys()
  let PaddingView = PaddingViewPKeys()
  let PopoverImpl = PopoverImplPKeys()

  let Popover<'TView, 'TResult when 'TView: (new: unit -> 'TView) and 'TView :> Terminal.Gui.ViewBase.View> =
    PopoverPKeys<'TView, 'TResult>()

  let PopoverMenu = PopoverMenuPKeys()
  let ProgressBar = ProgressBarPKeys()
  let Runnable = RunnablePKeys()
  let Runnable'<'TResult> = RunnablePKeys<'TResult>()
  let Dialog<'TResult> = DialogPKeys<'TResult>()

  let RunnableWrapper<'TView, 'TResult when 'TView: (new: unit -> 'TView) and 'TView :> Terminal.Gui.ViewBase.View> =
    RunnableWrapperPKeys<'TView, 'TResult>()

  let Dialog' = DialogPKeys()
  let FileDialog = FileDialogPKeys()

  let Prompt<'TView, 'TResult when 'TView: (new: unit -> 'TView) and 'TView :> Terminal.Gui.ViewBase.View> =
    PromptPKeys<'TView, 'TResult>()

  let OpenDialog = OpenDialogPKeys()
  let SaveDialog = SaveDialogPKeys()
  let ScrollBar = ScrollBarPKeys()
  let ScrollButton = ScrollButtonPKeys()
  let ScrollSlider = ScrollSliderPKeys()
  let SelectorBase = SelectorBasePKeys()
  let FlagSelector = FlagSelectorPKeys()
  let OptionSelector = OptionSelectorPKeys()

  let FlagSelector'<'TFlagsEnum
    when 'TFlagsEnum: struct
    and 'TFlagsEnum: (new: unit -> 'TFlagsEnum)
    and 'TFlagsEnum :> System.Enum
    and 'TFlagsEnum :> System.ValueType> =
    FlagSelectorPKeys<'TFlagsEnum>()

  let OptionSelector'<'TEnum
    when 'TEnum: struct and 'TEnum: (new: unit -> 'TEnum) and 'TEnum :> System.Enum and 'TEnum :> System.ValueType> =
    OptionSelectorPKeys<'TEnum>()

  let Shortcut = ShortcutPKeys()
  let MenuItem = MenuItemPKeys()
  let MenuBarItem = MenuBarItemPKeys()
  let SpinnerView = SpinnerViewPKeys()
  let StatusBar = StatusBarPKeys()
  let TableView = TableViewPKeys()
  let Tabs = TabsPKeys()
  let TextField = TextFieldPKeys()
  let DropDownList = DropDownListPKeys()

  let DropDownList'<'TEnum
    when 'TEnum: struct and 'TEnum: (new: unit -> 'TEnum) and 'TEnum :> System.Enum and 'TEnum :> System.ValueType> =
    DropDownListPKeys<'TEnum>()

  let TextValidateField = TextValidateFieldPKeys()
  let DateEditor = DateEditorPKeys()
  let TextView = TextViewPKeys()
  let TimeEditor = TimeEditorPKeys()
  let TitleView = TitleViewPKeys()

  let ToolTipHost<'TView when 'TView: (new: unit -> 'TView) and 'TView :> Terminal.Gui.ViewBase.View> =
    ToolTipHostPKeys<'TView>()

  let TreeView<'T when 'T: not struct> = TreeViewPKeys<'T>()
  let TreeView' = TreeViewPKeys()
  let Window = WindowPKeys()
  let Wizard = WizardPKeys()
  let WizardStep = WizardStepPKeys()

  module ReconciledEventKeys =
    let ListViewItems: PropKey array =
      [| ListView.CollectionChanged.Untyped
         ListView.SourceChanged.Untyped
         ListView.ValueChanged.Untyped
         ListView.ValueChangedUntyped.Untyped
         ListView.ValueChanging.Untyped |]

    let DropDownListItems: PropKey array =
      [| TextField.TextChanging.Untyped
         TextField.ValueChanged.Untyped
         TextField.ValueChangedUntyped.Untyped
         TextField.ValueChanging.Untyped |]
