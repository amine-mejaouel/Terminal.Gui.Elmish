namespace Terminal.Gui.Elmish

open System
open System.Collections.Generic
open Terminal.Gui.App
open Terminal.Gui.Views

[<RequireQualifiedAccess>]
module internal PKey =

  type ViewPKeys() =

    // Properties
    member val App: PropKey<Terminal.Gui.App.IApplication> = PropKey.Create.simple (0, "View.App")

    member val Arrangement: PropKey<Terminal.Gui.ViewBase.ViewArrangement> =
      PropKey.Create.simple (1, "View.Arrangement")

    member val AssignHotKeys: PropKey<bool> = PropKey.Create.simple (2, "View.AssignHotKeys")

    member val BorderStyle: PropKey<Nullable<Terminal.Gui.Drawing.LineStyle>> =
      PropKey.Create.simple (3, "View.BorderStyle")

    member val CanFocus: PropKey<bool> = PropKey.Create.simple (4, "View.CanFocus")

    member val CommandsToBubbleUp: PropKey<IReadOnlyList<Terminal.Gui.Input.Command>> =
      PropKey.Create.simple (5, "View.CommandsToBubbleUp")

    member val ContentSizeTracksViewport: PropKey<bool> = PropKey.Create.simple (6, "View.ContentSizeTracksViewport")
    member val Cursor: PropKey<Terminal.Gui.Drivers.Cursor> = PropKey.Create.simple (7, "View.Cursor")
    member val Data: PropKey<System.Object> = PropKey.Create.simple (8, "View.Data")

    member val DefaultAcceptView: PropKey<Terminal.Gui.ViewBase.View> =
      PropKey.Create.view (9, 10, "View.DefaultAcceptView_view")

    member val DefaultAcceptView_viewSpec: PropKey<IView> =
      PropKey.Create.subElement (10, 9, "View.DefaultAcceptView_viewSpec")

    member val Enabled: PropKey<bool> = PropKey.Create.simple (11, "View.Enabled")
    member val Frame: PropKey<System.Drawing.Rectangle> = PropKey.Create.simple (12, "View.Frame")
    member val HasFocus: PropKey<bool> = PropKey.Create.simple (13, "View.HasFocus")
    member val Height: PropKey<Terminal.Gui.ViewBase.Dim> = PropKey.Create.simple (14, "View.Height")
    member val HotKey: PropKey<Terminal.Gui.Input.Key> = PropKey.Create.simple (15, "View.HotKey")
    member val HotKeySpecifier: PropKey<System.Text.Rune> = PropKey.Create.simple (16, "View.HotKeySpecifier")
    member val Id: PropKey<string> = PropKey.Create.simple (17, "View.Id")
    member val IsInitialized: PropKey<bool> = PropKey.Create.simple (18, "View.IsInitialized")

    member val MouseHighlightStates: PropKey<Terminal.Gui.ViewBase.MouseState> =
      PropKey.Create.simple (19, "View.MouseHighlightStates")

    member val MouseHoldRepeat: PropKey<Nullable<Terminal.Gui.Input.MouseFlags>> =
      PropKey.Create.simple (20, "View.MouseHoldRepeat")

    member val MousePositionTracking: PropKey<bool> = PropKey.Create.simple (21, "View.MousePositionTracking")
    member val PreserveTrailingSpaces: PropKey<bool> = PropKey.Create.simple (22, "View.PreserveTrailingSpaces")
    member val SchemeName: PropKey<string> = PropKey.Create.simple (23, "View.SchemeName")

    member val ShadowStyle: PropKey<Nullable<Terminal.Gui.ViewBase.ShadowStyles>> =
      PropKey.Create.simple (24, "View.ShadowStyle")

    member val SuperViewRendersLineCanvas: PropKey<bool> = PropKey.Create.simple (25, "View.SuperViewRendersLineCanvas")

    member val TabStop: PropKey<Nullable<Terminal.Gui.ViewBase.TabBehavior>> =
      PropKey.Create.simple (26, "View.TabStop")

    member val Text: PropKey<string> = PropKey.Create.simple (27, "View.Text")

    member val TextAlignment: PropKey<Terminal.Gui.ViewBase.Alignment> =
      PropKey.Create.simple (28, "View.TextAlignment")

    member val TextDirection: PropKey<Terminal.Gui.Text.TextDirection> =
      PropKey.Create.simple (29, "View.TextDirection")

    member val Title: PropKey<string> = PropKey.Create.simple (30, "View.Title")
    member val UsedHotKeys: PropKey<HashSet<Terminal.Gui.Input.Key>> = PropKey.Create.simple (31, "View.UsedHotKeys")
    member val ValidatePosDim: PropKey<bool> = PropKey.Create.simple (32, "View.ValidatePosDim")

    member val VerticalTextAlignment: PropKey<Terminal.Gui.ViewBase.Alignment> =
      PropKey.Create.simple (33, "View.VerticalTextAlignment")

    member val Viewport: PropKey<System.Drawing.Rectangle> = PropKey.Create.simple (34, "View.Viewport")

    member val ViewportSettings: PropKey<Terminal.Gui.ViewBase.ViewportSettingsFlags> =
      PropKey.Create.simple (35, "View.ViewportSettings")

    member val Visible: PropKey<bool> = PropKey.Create.simple (36, "View.Visible")
    member val Width: PropKey<Terminal.Gui.ViewBase.Dim> = PropKey.Create.simple (37, "View.Width")

    // Events
    member val Accepted: PropKey<Terminal.Gui.Input.CommandEventArgs -> unit> =
      PropKey.Create.event (38, "View.Accepted_event")

    member val Accepting: PropKey<Terminal.Gui.Input.CommandEventArgs -> unit> =
      PropKey.Create.event (39, "View.Accepting_event")

    member val Activated: PropKey<EventArgs<Terminal.Gui.Input.ICommandContext> -> unit> =
      PropKey.Create.event (40, "View.Activated_event")

    member val Activating: PropKey<Terminal.Gui.Input.CommandEventArgs -> unit> =
      PropKey.Create.event (41, "View.Activating_event")

    member val AdvancingFocus: PropKey<Terminal.Gui.ViewBase.AdvanceFocusEventArgs -> unit> =
      PropKey.Create.event (42, "View.AdvancingFocus_event")

    member val BorderStyleChanged: PropKey<System.EventArgs -> unit> =
      PropKey.Create.event (43, "View.BorderStyleChanged_event")

    member val CanFocusChanged: PropKey<System.EventArgs -> unit> =
      PropKey.Create.event (44, "View.CanFocusChanged_event")

    member val ClearedViewport: PropKey<Terminal.Gui.ViewBase.DrawEventArgs -> unit> =
      PropKey.Create.event (45, "View.ClearedViewport_event")

    member val ClearingViewport: PropKey<Terminal.Gui.ViewBase.DrawEventArgs -> unit> =
      PropKey.Create.event (46, "View.ClearingViewport_event")

    member val CommandNotBound: PropKey<Terminal.Gui.Input.CommandEventArgs -> unit> =
      PropKey.Create.event (47, "View.CommandNotBound_event")

    member val ContentSizeChanged: PropKey<ValueChangedEventArgs<Nullable<System.Drawing.Size>> -> unit> =
      PropKey.Create.event (48, "View.ContentSizeChanged_event")

    member val ContentSizeChanging: PropKey<ValueChangingEventArgs<Nullable<System.Drawing.Size>> -> unit> =
      PropKey.Create.event (49, "View.ContentSizeChanging_event")

    member val Disposing: PropKey<System.EventArgs -> unit> = PropKey.Create.event (50, "View.Disposing_event")

    member val DrawComplete: PropKey<Terminal.Gui.ViewBase.DrawEventArgs -> unit> =
      PropKey.Create.event (51, "View.DrawComplete_event")

    member val DrawingContent: PropKey<Terminal.Gui.ViewBase.DrawEventArgs -> unit> =
      PropKey.Create.event (52, "View.DrawingContent_event")

    member val DrawingSubViews: PropKey<Terminal.Gui.ViewBase.DrawEventArgs -> unit> =
      PropKey.Create.event (53, "View.DrawingSubViews_event")

    member val DrawingText: PropKey<Terminal.Gui.ViewBase.DrawEventArgs -> unit> =
      PropKey.Create.event (54, "View.DrawingText_event")

    member val DrewText: PropKey<System.EventArgs -> unit> = PropKey.Create.event (55, "View.DrewText_event")

    member val EnabledChanged: PropKey<System.EventArgs -> unit> =
      PropKey.Create.event (56, "View.EnabledChanged_event")

    member val FocusedChanged: PropKey<Terminal.Gui.ViewBase.HasFocusEventArgs -> unit> =
      PropKey.Create.event (57, "View.FocusedChanged_event")

    member val FrameChanged: PropKey<EventArgs<System.Drawing.Rectangle> -> unit> =
      PropKey.Create.event (58, "View.FrameChanged_event")

    member val GettingAttributeForRole: PropKey<Terminal.Gui.Drawing.VisualRoleEventArgs -> unit> =
      PropKey.Create.event (59, "View.GettingAttributeForRole_event")

    member val GettingScheme: PropKey<ResultEventArgs<Terminal.Gui.Drawing.Scheme> -> unit> =
      PropKey.Create.event (60, "View.GettingScheme_event")

    member val HandlingHotKey: PropKey<Terminal.Gui.Input.CommandEventArgs -> unit> =
      PropKey.Create.event (61, "View.HandlingHotKey_event")

    member val HasFocusChanged: PropKey<Terminal.Gui.ViewBase.HasFocusEventArgs -> unit> =
      PropKey.Create.event (62, "View.HasFocusChanged_event")

    member val HasFocusChanging: PropKey<Terminal.Gui.ViewBase.HasFocusEventArgs -> unit> =
      PropKey.Create.event (63, "View.HasFocusChanging_event")

    member val HeightChanged: PropKey<ValueChangedEventArgs<Terminal.Gui.ViewBase.Dim> -> unit> =
      PropKey.Create.event (64, "View.HeightChanged_event")

    member val HeightChanging: PropKey<ValueChangingEventArgs<Terminal.Gui.ViewBase.Dim> -> unit> =
      PropKey.Create.event (65, "View.HeightChanging_event")

    member val HotKeyChanged: PropKey<Terminal.Gui.Input.KeyChangedEventArgs -> unit> =
      PropKey.Create.event (66, "View.HotKeyChanged_event")

    member val HotKeyCommand: PropKey<EventArgs<Terminal.Gui.Input.ICommandContext> -> unit> =
      PropKey.Create.event (67, "View.HotKeyCommand_event")

    member val Initialized: PropKey<System.EventArgs -> unit> = PropKey.Create.event (68, "View.Initialized_event")
    member val KeyDown: PropKey<Terminal.Gui.Input.Key -> unit> = PropKey.Create.event (69, "View.KeyDown_event")

    member val KeyDownNotHandled: PropKey<Terminal.Gui.Input.Key -> unit> =
      PropKey.Create.event (70, "View.KeyDownNotHandled_event")

    member val KeyUp: PropKey<Terminal.Gui.Input.Key -> unit> = PropKey.Create.event (71, "View.KeyUp_event")

    member val MouseEnter: PropKey<System.ComponentModel.CancelEventArgs -> unit> =
      PropKey.Create.event (72, "View.MouseEnter_event")

    member val MouseEvent: PropKey<Terminal.Gui.Input.Mouse -> unit> =
      PropKey.Create.event (73, "View.MouseEvent_event")

    member val MouseHoldRepeatChanged: PropKey<ValueChangedEventArgs<Nullable<Terminal.Gui.Input.MouseFlags>> -> unit> =
      PropKey.Create.event (74, "View.MouseHoldRepeatChanged_event")

    member val MouseHoldRepeatChanging: PropKey<ValueChangingEventArgs<Nullable<Terminal.Gui.Input.MouseFlags>> -> unit> =
      PropKey.Create.event (75, "View.MouseHoldRepeatChanging_event")

    member val MouseLeave: PropKey<System.EventArgs -> unit> = PropKey.Create.event (76, "View.MouseLeave_event")

    member val MouseStateChanged: PropKey<EventArgs<Terminal.Gui.ViewBase.MouseState> -> unit> =
      PropKey.Create.event (77, "View.MouseStateChanged_event")

    member val Pasted: PropKey<Terminal.Gui.Input.PastedEventArgs -> unit> =
      PropKey.Create.event (78, "View.Pasted_event")

    member val Pasting: PropKey<Terminal.Gui.Input.PastingEventArgs -> unit> =
      PropKey.Create.event (79, "View.Pasting_event")

    member val Removed: PropKey<Terminal.Gui.ViewBase.SuperViewChangedEventArgs -> unit> =
      PropKey.Create.event (80, "View.Removed_event")

    member val SchemeChanged: PropKey<ValueChangedEventArgs<Terminal.Gui.Drawing.Scheme> -> unit> =
      PropKey.Create.event (81, "View.SchemeChanged_event")

    member val SchemeChanging: PropKey<ValueChangingEventArgs<Terminal.Gui.Drawing.Scheme> -> unit> =
      PropKey.Create.event (82, "View.SchemeChanging_event")

    member val SchemeNameChanged: PropKey<ValueChangedEventArgs<string> -> unit> =
      PropKey.Create.event (83, "View.SchemeNameChanged_event")

    member val SchemeNameChanging: PropKey<ValueChangingEventArgs<string> -> unit> =
      PropKey.Create.event (84, "View.SchemeNameChanging_event")

    member val ShadowStyleChanged: PropKey<System.EventArgs -> unit> =
      PropKey.Create.event (85, "View.ShadowStyleChanged_event")

    member val SubViewAdded: PropKey<Terminal.Gui.ViewBase.SuperViewChangedEventArgs -> unit> =
      PropKey.Create.event (86, "View.SubViewAdded_event")

    member val SubViewAdding: PropKey<EventArgs<Terminal.Gui.ViewBase.View> -> unit> =
      PropKey.Create.event (87, "View.SubViewAdding_event")

    member val SubViewLayout: PropKey<Terminal.Gui.ViewBase.LayoutEventArgs -> unit> =
      PropKey.Create.event (88, "View.SubViewLayout_event")

    member val SubViewRemoved: PropKey<Terminal.Gui.ViewBase.SuperViewChangedEventArgs -> unit> =
      PropKey.Create.event (89, "View.SubViewRemoved_event")

    member val SubViewsLaidOut: PropKey<Terminal.Gui.ViewBase.LayoutEventArgs -> unit> =
      PropKey.Create.event (90, "View.SubViewsLaidOut_event")

    member val SuperViewChanged: PropKey<ValueChangedEventArgs<Terminal.Gui.ViewBase.View> -> unit> =
      PropKey.Create.event (91, "View.SuperViewChanged_event")

    member val SuperViewChanging: PropKey<ValueChangingEventArgs<Terminal.Gui.ViewBase.View> -> unit> =
      PropKey.Create.event (92, "View.SuperViewChanging_event")

    member val TextChanged: PropKey<System.EventArgs -> unit> = PropKey.Create.event (93, "View.TextChanged_event")
    member val TitleChanged: PropKey<EventArgs<string> -> unit> = PropKey.Create.event (94, "View.TitleChanged_event")

    member val TitleChanging: PropKey<CancelEventArgs<string> -> unit> =
      PropKey.Create.event (95, "View.TitleChanging_event")

    member val ViewportChanged: PropKey<Terminal.Gui.ViewBase.DrawEventArgs -> unit> =
      PropKey.Create.event (96, "View.ViewportChanged_event")

    member val VisibleChanged: PropKey<System.EventArgs -> unit> =
      PropKey.Create.event (97, "View.VisibleChanged_event")

    member val VisibleChanging: PropKey<CancelEventArgs<bool> -> unit> =
      PropKey.Create.event (98, "View.VisibleChanging_event")

    member val WidthChanged: PropKey<ValueChangedEventArgs<Terminal.Gui.ViewBase.Dim> -> unit> =
      PropKey.Create.event (99, "View.WidthChanged_event")

    member val WidthChanging: PropKey<ValueChangingEventArgs<Terminal.Gui.ViewBase.Dim> -> unit> =
      PropKey.Create.event (100, "View.WidthChanging_event")

  type AdornmentViewPKeys() =
    inherit ViewPKeys()

    // Properties
    member val Adornment: PropKey<Terminal.Gui.ViewBase.IAdornment> =
      PropKey.Create.simple (101, "AdornmentView.Adornment")

    member val Diagnostics: PropKey<Terminal.Gui.ViewBase.ViewDiagnosticFlags> =
      PropKey.Create.simple (102, "AdornmentView.Diagnostics")

    member val SuperViewRendersLineCanvas: PropKey<bool> =
      PropKey.Create.simple (103, "AdornmentView.SuperViewRendersLineCanvas")

    member val Viewport: PropKey<System.Drawing.Rectangle> = PropKey.Create.simple (104, "AdornmentView.Viewport")

  type AttributePickerPKeys() =
    inherit ViewPKeys()

    // Properties
    member val SampleText: PropKey<string> = PropKey.Create.simple (105, "AttributePicker.SampleText")

    member val Value: PropKey<Nullable<Terminal.Gui.Drawing.Attribute>> =
      PropKey.Create.simple (106, "AttributePicker.Value")

    // Events
    member val ValueChanged: PropKey<ValueChangedEventArgs<Nullable<Terminal.Gui.Drawing.Attribute>> -> unit> =
      PropKey.Create.event (107, "AttributePicker.ValueChanged_event")

    member val ValueChangedUntyped: PropKey<ValueChangedEventArgs<System.Object> -> unit> =
      PropKey.Create.event (108, "AttributePicker.ValueChangedUntyped_event")

    member val ValueChanging: PropKey<ValueChangingEventArgs<Nullable<Terminal.Gui.Drawing.Attribute>> -> unit> =
      PropKey.Create.event (109, "AttributePicker.ValueChanging_event")

  type BarPKeys() =
    inherit ViewPKeys()

    // Properties
    member val AlignmentModes: PropKey<Terminal.Gui.ViewBase.AlignmentModes> =
      PropKey.Create.simple (110, "Bar.AlignmentModes")

    member val Orientation: PropKey<Terminal.Gui.ViewBase.Orientation> = PropKey.Create.simple (111, "Bar.Orientation")

    // Events
    member val OrientationChanged: PropKey<EventArgs<Terminal.Gui.ViewBase.Orientation> -> unit> =
      PropKey.Create.event (112, "Bar.OrientationChanged_event")

    member val OrientationChanging: PropKey<CancelEventArgs<Terminal.Gui.ViewBase.Orientation> -> unit> =
      PropKey.Create.event (113, "Bar.OrientationChanging_event")

  type BorderViewPKeys() =
    inherit AdornmentViewPKeys()

    // Properties
    member val TabLength: PropKey<Nullable<int>> = PropKey.Create.simple (114, "BorderView.TabLength")
    member val TabOffset: PropKey<int> = PropKey.Create.simple (115, "BorderView.TabOffset")
    member val TabSide: PropKey<Terminal.Gui.ViewBase.Side> = PropKey.Create.simple (116, "BorderView.TabSide")

  type ButtonPKeys() =
    inherit ViewPKeys()

    // Properties
    member val HotKeySpecifier: PropKey<System.Text.Rune> = PropKey.Create.simple (117, "Button.HotKeySpecifier")
    member val IsDefault: PropKey<bool> = PropKey.Create.simple (118, "Button.IsDefault")
    member val NoDecorations: PropKey<bool> = PropKey.Create.simple (119, "Button.NoDecorations")
    member val NoPadding: PropKey<bool> = PropKey.Create.simple (120, "Button.NoPadding")
    member val Text: PropKey<string> = PropKey.Create.simple (121, "Button.Text")

    // Events
    member val InitializingShadowStyle: PropKey<
      ValueChangingEventArgs<Nullable<Terminal.Gui.ViewBase.ShadowStyles>> -> unit
     > = PropKey.Create.event (122, "Button.InitializingShadowStyle_event")

  type CharMapPKeys() =
    inherit ViewPKeys()

    // Properties
    member val SelectedCodePoint: PropKey<int> = PropKey.Create.simple (123, "CharMap.SelectedCodePoint")
    member val ShowGlyphWidths: PropKey<bool> = PropKey.Create.simple (124, "CharMap.ShowGlyphWidths")

    member val ShowUnicodeCategory: PropKey<Nullable<System.Globalization.UnicodeCategory>> =
      PropKey.Create.simple (125, "CharMap.ShowUnicodeCategory")

    member val StartCodePoint: PropKey<int> = PropKey.Create.simple (126, "CharMap.StartCodePoint")
    member val Value: PropKey<System.Text.Rune> = PropKey.Create.simple (127, "CharMap.Value")

    // Events
    member val ValueChanged: PropKey<ValueChangedEventArgs<System.Text.Rune> -> unit> =
      PropKey.Create.event (128, "CharMap.ValueChanged_event")

    member val ValueChangedUntyped: PropKey<ValueChangedEventArgs<System.Object> -> unit> =
      PropKey.Create.event (129, "CharMap.ValueChangedUntyped_event")

    member val ValueChanging: PropKey<ValueChangingEventArgs<System.Text.Rune> -> unit> =
      PropKey.Create.event (130, "CharMap.ValueChanging_event")

  type CheckBoxPKeys() =
    inherit ViewPKeys()

    // Properties
    member val AllowCheckStateNone: PropKey<bool> = PropKey.Create.simple (131, "CheckBox.AllowCheckStateNone")
    member val HotKeySpecifier: PropKey<System.Text.Rune> = PropKey.Create.simple (132, "CheckBox.HotKeySpecifier")
    member val RadioStyle: PropKey<bool> = PropKey.Create.simple (133, "CheckBox.RadioStyle")
    member val Text: PropKey<string> = PropKey.Create.simple (134, "CheckBox.Text")
    member val Value: PropKey<Terminal.Gui.Views.CheckState> = PropKey.Create.simple (135, "CheckBox.Value")

    // Events
    member val ValueChanged: PropKey<ValueChangedEventArgs<Terminal.Gui.Views.CheckState> -> unit> =
      PropKey.Create.event (136, "CheckBox.ValueChanged_event")

    member val ValueChangedUntyped: PropKey<ValueChangedEventArgs<System.Object> -> unit> =
      PropKey.Create.event (137, "CheckBox.ValueChangedUntyped_event")

    member val ValueChanging: PropKey<ValueChangingEventArgs<Terminal.Gui.Views.CheckState> -> unit> =
      PropKey.Create.event (138, "CheckBox.ValueChanging_event")

  type CodePKeys() =
    inherit ViewPKeys()

    // Properties
    member val Language: PropKey<string> = PropKey.Create.simple (139, "Code.Language")

    member val SyntaxHighlighter: PropKey<Terminal.Gui.Drawing.ISyntaxHighlighter> =
      PropKey.Create.simple (140, "Code.SyntaxHighlighter")

    member val Text: PropKey<string> = PropKey.Create.simple (141, "Code.Text")

  type ColorPickerPKeys() =
    inherit ViewPKeys()

    // Properties
    member val SelectedColor: PropKey<Terminal.Gui.Drawing.Color> =
      PropKey.Create.simple (142, "ColorPicker.SelectedColor")

    member val Style: PropKey<Terminal.Gui.Views.ColorPickerStyle> = PropKey.Create.simple (143, "ColorPicker.Style")
    member val Text: PropKey<string> = PropKey.Create.simple (144, "ColorPicker.Text")
    member val Value: PropKey<Nullable<Terminal.Gui.Drawing.Color>> = PropKey.Create.simple (145, "ColorPicker.Value")

    // Events
    member val ValueChanged: PropKey<ValueChangedEventArgs<Nullable<Terminal.Gui.Drawing.Color>> -> unit> =
      PropKey.Create.event (146, "ColorPicker.ValueChanged_event")

    member val ValueChangedUntyped: PropKey<ValueChangedEventArgs<System.Object> -> unit> =
      PropKey.Create.event (147, "ColorPicker.ValueChangedUntyped_event")

    member val ValueChanging: PropKey<ValueChangingEventArgs<Nullable<Terminal.Gui.Drawing.Color>> -> unit> =
      PropKey.Create.event (148, "ColorPicker.ValueChanging_event")

  type ColorPicker16PKeys() =
    inherit ViewPKeys()

    // Properties
    member val BoxHeight: PropKey<int> = PropKey.Create.simple (149, "ColorPicker16.BoxHeight")
    member val BoxWidth: PropKey<int> = PropKey.Create.simple (150, "ColorPicker16.BoxWidth")
    member val Caret: PropKey<System.Drawing.Point> = PropKey.Create.simple (151, "ColorPicker16.Caret")

    member val SelectedColor: PropKey<Terminal.Gui.Drawing.ColorName16> =
      PropKey.Create.simple (152, "ColorPicker16.SelectedColor")

    member val Value: PropKey<Terminal.Gui.Drawing.ColorName16> = PropKey.Create.simple (153, "ColorPicker16.Value")

    // Events
    member val ValueChanged: PropKey<ValueChangedEventArgs<Terminal.Gui.Drawing.ColorName16> -> unit> =
      PropKey.Create.event (154, "ColorPicker16.ValueChanged_event")

    member val ValueChangedUntyped: PropKey<ValueChangedEventArgs<System.Object> -> unit> =
      PropKey.Create.event (155, "ColorPicker16.ValueChangedUntyped_event")

    member val ValueChanging: PropKey<ValueChangingEventArgs<Terminal.Gui.Drawing.ColorName16> -> unit> =
      PropKey.Create.event (156, "ColorPicker16.ValueChanging_event")

  type DatePickerPKeys() =
    inherit ViewPKeys()

    // Properties
    member val Culture: PropKey<System.Globalization.CultureInfo> = PropKey.Create.simple (157, "DatePicker.Culture")
    member val Text: PropKey<string> = PropKey.Create.simple (158, "DatePicker.Text")
    member val Value: PropKey<System.DateTime> = PropKey.Create.simple (159, "DatePicker.Value")

    // Events
    member val ValueChanged: PropKey<ValueChangedEventArgs<System.DateTime> -> unit> =
      PropKey.Create.event (160, "DatePicker.ValueChanged_event")

    member val ValueChangedUntyped: PropKey<ValueChangedEventArgs<System.Object> -> unit> =
      PropKey.Create.event (161, "DatePicker.ValueChangedUntyped_event")

    member val ValueChanging: PropKey<ValueChangingEventArgs<System.DateTime> -> unit> =
      PropKey.Create.event (162, "DatePicker.ValueChanging_event")

  type FrameViewPKeys() =
    inherit ViewPKeys()


  type GraphViewPKeys() =
    inherit ViewPKeys()

    // Properties
    member val AxisX: PropKey<Terminal.Gui.Views.HorizontalAxis> = PropKey.Create.simple (163, "GraphView.AxisX")
    member val AxisY: PropKey<Terminal.Gui.Views.VerticalAxis> = PropKey.Create.simple (164, "GraphView.AxisY")
    member val CellSize: PropKey<System.Drawing.PointF> = PropKey.Create.simple (165, "GraphView.CellSize")

    member val GraphColor: PropKey<Nullable<Terminal.Gui.Drawing.Attribute>> =
      PropKey.Create.simple (166, "GraphView.GraphColor")

    member val MarginBottom: PropKey<System.UInt32> = PropKey.Create.simple (167, "GraphView.MarginBottom")
    member val MarginLeft: PropKey<System.UInt32> = PropKey.Create.simple (168, "GraphView.MarginLeft")
    member val ScrollOffset: PropKey<System.Drawing.PointF> = PropKey.Create.simple (169, "GraphView.ScrollOffset")

  type HexViewPKeys() =
    inherit ViewPKeys()

    // Properties
    member val Address: PropKey<System.Int64> = PropKey.Create.simple (170, "HexView.Address")
    member val AddressWidth: PropKey<int> = PropKey.Create.simple (171, "HexView.AddressWidth")
    member val BytesPerLine: PropKey<int> = PropKey.Create.simple (172, "HexView.BytesPerLine")
    member val ReadOnly: PropKey<bool> = PropKey.Create.simple (173, "HexView.ReadOnly")
    member val Source: PropKey<System.IO.Stream> = PropKey.Create.simple (174, "HexView.Source")

    // Events
    member val Edited: PropKey<Terminal.Gui.Views.HexViewEditEventArgs -> unit> =
      PropKey.Create.event (175, "HexView.Edited_event")

    member val PositionChanged: PropKey<Terminal.Gui.Views.HexViewEventArgs -> unit> =
      PropKey.Create.event (176, "HexView.PositionChanged_event")

  type ImageViewPKeys() =
    inherit ViewPKeys()

    // Properties
    member val AllowSixelUpscaling: PropKey<bool> = PropKey.Create.simple (177, "ImageView.AllowSixelUpscaling")
    member val Image: PropKey<Terminal.Gui.Drawing.Color[,]> = PropKey.Create.simple (178, "ImageView.Image")
    member val MaxSixelPaletteColors: PropKey<int> = PropKey.Create.simple (179, "ImageView.MaxSixelPaletteColors")

    member val SixelEncoder: PropKey<Terminal.Gui.Drawing.SixelEncoder> =
      PropKey.Create.simple (180, "ImageView.SixelEncoder")

    member val UseBackgroundRendering: PropKey<bool> = PropKey.Create.simple (181, "ImageView.UseBackgroundRendering")
    member val UseRasterGraphics: PropKey<bool> = PropKey.Create.simple (182, "ImageView.UseRasterGraphics")
    member val UseSixel: PropKey<bool> = PropKey.Create.simple (183, "ImageView.UseSixel")
    member val ZoomLevel: PropKey<System.Double> = PropKey.Create.simple (184, "ImageView.ZoomLevel")

    // Events
    member val ZoomLevelChanged: PropKey<System.EventArgs -> unit> =
      PropKey.Create.event (185, "ImageView.ZoomLevelChanged_event")

  type LabelPKeys() =
    inherit ViewPKeys()

    // Properties
    member val HotKeySpecifier: PropKey<System.Text.Rune> = PropKey.Create.simple (186, "Label.HotKeySpecifier")
    member val Text: PropKey<string> = PropKey.Create.simple (187, "Label.Text")

  type LegendAnnotationPKeys() =
    inherit ViewPKeys()


  type LinePKeys() =
    inherit ViewPKeys()

    // Properties
    member val Length: PropKey<Terminal.Gui.ViewBase.Dim> = PropKey.Create.simple (188, "Line.Length")

    member val LineAttribute: PropKey<Nullable<Terminal.Gui.Drawing.Attribute>> =
      PropKey.Create.simple (189, "Line.LineAttribute")

    member val Orientation: PropKey<Terminal.Gui.ViewBase.Orientation> = PropKey.Create.simple (190, "Line.Orientation")
    member val Style: PropKey<Terminal.Gui.Drawing.LineStyle> = PropKey.Create.simple (191, "Line.Style")

    // Events
    member val OrientationChanged: PropKey<EventArgs<Terminal.Gui.ViewBase.Orientation> -> unit> =
      PropKey.Create.event (192, "Line.OrientationChanged_event")

    member val OrientationChanging: PropKey<CancelEventArgs<Terminal.Gui.ViewBase.Orientation> -> unit> =
      PropKey.Create.event (193, "Line.OrientationChanging_event")

  type LinearRangeViewBasePKeys<'TOption, 'TValue>() =
    inherit ViewPKeys()

    // Properties
    member val AllowEmpty: PropKey<bool> = PropKey.Create.simple (194, "LinearRangeViewBase.AllowEmpty")
    member val FocusedOption: PropKey<int> = PropKey.Create.simple (195, "LinearRangeViewBase.FocusedOption")

    member val LegendsOrientation: PropKey<Terminal.Gui.ViewBase.Orientation> =
      PropKey.Create.simple (196, "LinearRangeViewBase.LegendsOrientation")

    member val MinimumInnerSpacing: PropKey<int> =
      PropKey.Create.simple (197, "LinearRangeViewBase.MinimumInnerSpacing")

    member val Options: PropKey<List<LinearRangeOption<'TOption>>> =
      PropKey.Create.simple (198, "LinearRangeViewBase.Options")

    member val Orientation: PropKey<Terminal.Gui.ViewBase.Orientation> =
      PropKey.Create.simple (199, "LinearRangeViewBase.Orientation")

    member val ShowEndSpacing: PropKey<bool> = PropKey.Create.simple (200, "LinearRangeViewBase.ShowEndSpacing")
    member val ShowLegends: PropKey<bool> = PropKey.Create.simple (201, "LinearRangeViewBase.ShowLegends")

    member val Style: PropKey<Terminal.Gui.Views.LinearRangeStyle> =
      PropKey.Create.simple (202, "LinearRangeViewBase.Style")

    member val Text: PropKey<string> = PropKey.Create.simple (203, "LinearRangeViewBase.Text")
    member val UseMinimumSize: PropKey<bool> = PropKey.Create.simple (204, "LinearRangeViewBase.UseMinimumSize")
    member val Value: PropKey<'TValue> = PropKey.Create.simple (205, "LinearRangeViewBase.Value")

    // Events
    member val LegendsOrientationChanged: PropKey<ValueChangedEventArgs<Terminal.Gui.ViewBase.Orientation> -> unit> =
      PropKey.Create.event (206, "LinearRangeViewBase.LegendsOrientationChanged_event")

    member val LegendsOrientationChanging: PropKey<ValueChangingEventArgs<Terminal.Gui.ViewBase.Orientation> -> unit> =
      PropKey.Create.event (207, "LinearRangeViewBase.LegendsOrientationChanging_event")

    member val MinimumInnerSpacingChanged: PropKey<ValueChangedEventArgs<int> -> unit> =
      PropKey.Create.event (208, "LinearRangeViewBase.MinimumInnerSpacingChanged_event")

    member val MinimumInnerSpacingChanging: PropKey<ValueChangingEventArgs<int> -> unit> =
      PropKey.Create.event (209, "LinearRangeViewBase.MinimumInnerSpacingChanging_event")

    member val OptionFocused: PropKey<LinearRangeEventArgs<'TOption> -> unit> =
      PropKey.Create.event (210, "LinearRangeViewBase.OptionFocused_event")

    member val OrientationChanged: PropKey<EventArgs<Terminal.Gui.ViewBase.Orientation> -> unit> =
      PropKey.Create.event (211, "LinearRangeViewBase.OrientationChanged_event")

    member val OrientationChanging: PropKey<CancelEventArgs<Terminal.Gui.ViewBase.Orientation> -> unit> =
      PropKey.Create.event (212, "LinearRangeViewBase.OrientationChanging_event")

    member val ShowEndSpacingChanged: PropKey<ValueChangedEventArgs<bool> -> unit> =
      PropKey.Create.event (213, "LinearRangeViewBase.ShowEndSpacingChanged_event")

    member val ShowEndSpacingChanging: PropKey<ValueChangingEventArgs<bool> -> unit> =
      PropKey.Create.event (214, "LinearRangeViewBase.ShowEndSpacingChanging_event")

    member val ShowLegendsChanged: PropKey<ValueChangedEventArgs<bool> -> unit> =
      PropKey.Create.event (215, "LinearRangeViewBase.ShowLegendsChanged_event")

    member val ShowLegendsChanging: PropKey<ValueChangingEventArgs<bool> -> unit> =
      PropKey.Create.event (216, "LinearRangeViewBase.ShowLegendsChanging_event")

    member val UseMinimumSizeChanged: PropKey<ValueChangedEventArgs<bool> -> unit> =
      PropKey.Create.event (217, "LinearRangeViewBase.UseMinimumSizeChanged_event")

    member val UseMinimumSizeChanging: PropKey<ValueChangingEventArgs<bool> -> unit> =
      PropKey.Create.event (218, "LinearRangeViewBase.UseMinimumSizeChanging_event")

    member val ValueChanged: PropKey<ValueChangedEventArgs<'TValue> -> unit> =
      PropKey.Create.event (219, "LinearRangeViewBase.ValueChanged_event")

    member val ValueChangedUntyped: PropKey<ValueChangedEventArgs<System.Object> -> unit> =
      PropKey.Create.event (220, "LinearRangeViewBase.ValueChangedUntyped_event")

    member val ValueChanging: PropKey<ValueChangingEventArgs<'TValue> -> unit> =
      PropKey.Create.event (221, "LinearRangeViewBase.ValueChanging_event")

  type LinearMultiSelectorPKeys<'T>() =
    inherit LinearRangeViewBasePKeys<'T, IReadOnlyList<'T>>()

    // Properties
    member val Value: PropKey<IReadOnlyList<'T>> = PropKey.Create.simple (222, "LinearMultiSelector.Value")

  type LinearMultiSelectorPKeys() =
    inherit LinearMultiSelectorPKeys<string>()


  type LinearRangePKeys<'T>() =
    inherit LinearRangeViewBasePKeys<'T, LinearRangeSpan<'T>>()

    // Properties
    member val RangeAllowSingle: PropKey<bool> = PropKey.Create.simple (223, "LinearRange.RangeAllowSingle")

    member val RangeKind: PropKey<Terminal.Gui.Views.LinearRangeSpanKind> =
      PropKey.Create.simple (224, "LinearRange.RangeKind")

    member val Value: PropKey<LinearRangeSpan<'T>> = PropKey.Create.simple (225, "LinearRange.Value")

  type LinearRangePKeys() =
    inherit LinearRangePKeys<string>()


  type LinearSelectorPKeys<'T>() =
    inherit LinearRangeViewBasePKeys<'T, 'T>()

    // Properties
    member val SelectedIndex: PropKey<Nullable<int>> = PropKey.Create.simple (226, "LinearSelector.SelectedIndex")
    member val Value: PropKey<'T> = PropKey.Create.simple (227, "LinearSelector.Value")

  type LinearSelectorPKeys() =
    inherit LinearSelectorPKeys<string>()


  type LinkPKeys() =
    inherit ViewPKeys()

    // Properties
    member val Url: PropKey<string> = PropKey.Create.simple (228, "Link.Url")

    // Events
    member val UrlChanged: PropKey<ValueChangedEventArgs<string> -> unit> =
      PropKey.Create.event (229, "Link.UrlChanged_event")

    member val UrlChanging: PropKey<ValueChangingEventArgs<string> -> unit> =
      PropKey.Create.event (230, "Link.UrlChanging_event")

  type ListViewPKeys() =
    inherit ViewPKeys()

    // Properties
    member val KeystrokeNavigator: PropKey<Terminal.Gui.Views.IListCollectionNavigator> =
      PropKey.Create.simple (231, "ListView.KeystrokeNavigator")

    member val MarkMultiple: PropKey<bool> = PropKey.Create.simple (232, "ListView.MarkMultiple")
    member val SelectedItem: PropKey<Nullable<int>> = PropKey.Create.simple (233, "ListView.SelectedItem")
    member val ShowMarks: PropKey<bool> = PropKey.Create.simple (234, "ListView.ShowMarks")
    member val Source: PropKey<Terminal.Gui.Views.IListDataSource> = PropKey.Create.simple (235, "ListView.Source")
    member val Value: PropKey<Nullable<int>> = PropKey.Create.simple (236, "ListView.Value")

    // Events
    member val CollectionChanged: PropKey<System.Collections.Specialized.NotifyCollectionChangedEventArgs -> unit> =
      PropKey.Create.event (237, "ListView.CollectionChanged_event")

    member val RowRender: PropKey<Terminal.Gui.Views.ListViewRowEventArgs -> unit> =
      PropKey.Create.event (238, "ListView.RowRender_event")

    member val SourceChanged: PropKey<System.EventArgs -> unit> =
      PropKey.Create.event (239, "ListView.SourceChanged_event")

    member val ValueChanged: PropKey<ValueChangedEventArgs<Nullable<int>> -> unit> =
      PropKey.Create.event (240, "ListView.ValueChanged_event")

    member val ValueChangedUntyped: PropKey<ValueChangedEventArgs<System.Object> -> unit> =
      PropKey.Create.event (241, "ListView.ValueChangedUntyped_event")

    member val ValueChanging: PropKey<ValueChangingEventArgs<Nullable<int>> -> unit> =
      PropKey.Create.event (242, "ListView.ValueChanging_event")

  type ListViewPKeys<'T>() =
    inherit ListViewPKeys()

    // Properties
    member val Index: PropKey<Nullable<int>> = PropKey.Create.simple (243, "ListView.Index")
    member val SelectedItem: PropKey<'T> = PropKey.Create.simple (244, "ListView.SelectedItem")
    member val Value: PropKey<'T> = PropKey.Create.simple (245, "ListView.Value")

    // Events
    member val ValueChanged: PropKey<ValueChangedEventArgs<'T> -> unit> =
      PropKey.Create.event (246, "ListView.ValueChanged_event")

    member val ValueChangedUntyped: PropKey<ValueChangedEventArgs<System.Object> -> unit> =
      PropKey.Create.event (247, "ListView.ValueChangedUntyped_event")

    member val ValueChanging: PropKey<ValueChangingEventArgs<'T> -> unit> =
      PropKey.Create.event (248, "ListView.ValueChanging_event")

  type MarginViewPKeys() =
    inherit AdornmentViewPKeys()

    // Properties
    member val ShadowSize: PropKey<System.Drawing.Size> = PropKey.Create.simple (249, "MarginView.ShadowSize")

    member val ShadowStyle: PropKey<Nullable<Terminal.Gui.ViewBase.ShadowStyles>> =
      PropKey.Create.simple (250, "MarginView.ShadowStyle")

  type MarkdownPKeys() =
    inherit ViewPKeys()

    // Properties
    member val EnableSixelImages: PropKey<bool> = PropKey.Create.simple (251, "Markdown.EnableSixelImages")
    member val HotKeySpecifier: PropKey<System.Text.Rune> = PropKey.Create.simple (252, "Markdown.HotKeySpecifier")
    member val ImageLoader: PropKey<Func<string, System.Byte[]>> = PropKey.Create.simple (253, "Markdown.ImageLoader")

    member val MarkdownPipeline: PropKey<Markdig.MarkdownPipeline> =
      PropKey.Create.simple (254, "Markdown.MarkdownPipeline")

    member val ShowCopyButtons: PropKey<bool> = PropKey.Create.simple (255, "Markdown.ShowCopyButtons")
    member val ShowHeadingPrefix: PropKey<bool> = PropKey.Create.simple (256, "Markdown.ShowHeadingPrefix")

    member val SyntaxHighlighter: PropKey<Terminal.Gui.Drawing.ISyntaxHighlighter> =
      PropKey.Create.simple (257, "Markdown.SyntaxHighlighter")

    member val Text: PropKey<string> = PropKey.Create.simple (258, "Markdown.Text")
    member val UseThemeBackground: PropKey<bool> = PropKey.Create.simple (259, "Markdown.UseThemeBackground")

    // Events
    member val LinkClicked: PropKey<Terminal.Gui.Views.MarkdownLinkEventArgs -> unit> =
      PropKey.Create.event (260, "Markdown.LinkClicked_event")

    member val MarkdownChanged: PropKey<System.EventArgs -> unit> =
      PropKey.Create.event (261, "Markdown.MarkdownChanged_event")

  type MarkdownCodeBlockPKeys() =
    inherit ViewPKeys()

    // Properties
    member val CodeLines: PropKey<IReadOnlyList<string>> = PropKey.Create.simple (262, "MarkdownCodeBlock.CodeLines")
    member val Language: PropKey<string> = PropKey.Create.simple (263, "MarkdownCodeBlock.Language")
    member val ShowCopyButton: PropKey<bool> = PropKey.Create.simple (264, "MarkdownCodeBlock.ShowCopyButton")

    member val SyntaxHighlighter: PropKey<Terminal.Gui.Drawing.ISyntaxHighlighter> =
      PropKey.Create.simple (265, "MarkdownCodeBlock.SyntaxHighlighter")

    member val Text: PropKey<string> = PropKey.Create.simple (266, "MarkdownCodeBlock.Text")

    member val ThemeBackground: PropKey<Nullable<Terminal.Gui.Drawing.Color>> =
      PropKey.Create.simple (267, "MarkdownCodeBlock.ThemeBackground")

  type MarkdownTablePKeys() =
    inherit ViewPKeys()

    // Properties
    member val SyntaxHighlighter: PropKey<Terminal.Gui.Drawing.ISyntaxHighlighter> =
      PropKey.Create.simple (268, "MarkdownTable.SyntaxHighlighter")

    member val TableData: PropKey<Terminal.Gui.Views.TableData> = PropKey.Create.simple (269, "MarkdownTable.TableData")
    member val Text: PropKey<string> = PropKey.Create.simple (270, "MarkdownTable.Text")
    member val UseThemeBackground: PropKey<bool> = PropKey.Create.simple (271, "MarkdownTable.UseThemeBackground")

    // Events
    member val LinkClicked: PropKey<Terminal.Gui.Views.MarkdownLinkEventArgs -> unit> =
      PropKey.Create.event (272, "MarkdownTable.LinkClicked_event")

  type MenuPKeys() =
    inherit BarPKeys()

    // Properties
    member val SuperMenuItem: PropKey<Terminal.Gui.Views.MenuItem> =
      PropKey.Create.view (273, 274, "Menu.SuperMenuItem_view")

    member val SuperMenuItem_viewSpec: PropKey<IMenuItemView> =
      PropKey.Create.subElement (274, 273, "Menu.SuperMenuItem_viewSpec")

    member val Value: PropKey<Terminal.Gui.Views.MenuItem> = PropKey.Create.view (275, 276, "Menu.Value_view")
    member val Value_viewSpec: PropKey<IMenuItemView> = PropKey.Create.subElement (276, 275, "Menu.Value_viewSpec")

    // Events
    member val SelectedMenuItemChanged: PropKey<Terminal.Gui.Views.MenuItem -> unit> =
      PropKey.Create.event (277, "Menu.SelectedMenuItemChanged_event")

    member val ValueChanged: PropKey<ValueChangedEventArgs<Terminal.Gui.Views.MenuItem> -> unit> =
      PropKey.Create.event (278, "Menu.ValueChanged_event")

    member val ValueChangedUntyped: PropKey<ValueChangedEventArgs<System.Object> -> unit> =
      PropKey.Create.event (279, "Menu.ValueChangedUntyped_event")

    member val ValueChanging: PropKey<ValueChangingEventArgs<Terminal.Gui.Views.MenuItem> -> unit> =
      PropKey.Create.event (280, "Menu.ValueChanging_event")

  type MenuBarPKeys() =
    inherit MenuPKeys()

    // Properties
    member val Key: PropKey<Terminal.Gui.Input.Key> = PropKey.Create.simple (281, "MenuBar.Key")

    // Events
    member val KeyChanged: PropKey<Terminal.Gui.Input.KeyChangedEventArgs -> unit> =
      PropKey.Create.event (282, "MenuBar.KeyChanged_event")

  type NumericUpDownPKeys<'T>() =
    inherit ViewPKeys()

    // Properties
    member val Format: PropKey<string> = PropKey.Create.simple (283, "NumericUpDown.Format")
    member val Increment: PropKey<'T> = PropKey.Create.simple (284, "NumericUpDown.Increment")
    member val Value: PropKey<'T> = PropKey.Create.simple (285, "NumericUpDown.Value")

    // Events
    member val FormatChanged: PropKey<EventArgs<string> -> unit> =
      PropKey.Create.event (286, "NumericUpDown.FormatChanged_event")

    member val IncrementChanged: PropKey<EventArgs<'T> -> unit> =
      PropKey.Create.event (287, "NumericUpDown.IncrementChanged_event")

    member val ValueChanged: PropKey<ValueChangedEventArgs<'T> -> unit> =
      PropKey.Create.event (288, "NumericUpDown.ValueChanged_event")

    member val ValueChangedUntyped: PropKey<ValueChangedEventArgs<System.Object> -> unit> =
      PropKey.Create.event (289, "NumericUpDown.ValueChangedUntyped_event")

    member val ValueChanging: PropKey<ValueChangingEventArgs<'T> -> unit> =
      PropKey.Create.event (290, "NumericUpDown.ValueChanging_event")

  type NumericUpDownPKeys() =
    inherit NumericUpDownPKeys<int>()


  type PaddingViewPKeys() =
    inherit AdornmentViewPKeys()


  type PopoverImplPKeys() =
    inherit ViewPKeys()

    // Properties
    member val Anchor: PropKey<Func<Nullable<System.Drawing.Rectangle>>> =
      PropKey.Create.simple (291, "PopoverImpl.Anchor")

    member val Owner: PropKey<Terminal.Gui.App.IRunnable> = PropKey.Create.simple (292, "PopoverImpl.Owner")

    member val Target: PropKey<WeakReference<Terminal.Gui.ViewBase.View>> =
      PropKey.Create.simple (293, "PopoverImpl.Target")

  type PopoverPKeys<'TView, 'TResult when 'TView: (new: unit -> 'TView) and 'TView :> Terminal.Gui.ViewBase.View>() =
    inherit PopoverImplPKeys()

    // Properties
    member val ContentView: PropKey<'TView> = PropKey.Create.view (294, 295, "Popover.ContentView_view")

    member val ContentView_viewSpec: PropKey<ITViewView> =
      PropKey.Create.subElement (295, 294, "Popover.ContentView_viewSpec")

    member val ResultExtractor: PropKey<Func<'TView, 'TResult>> = PropKey.Create.simple (296, "Popover.ResultExtractor")

    // Events
    member val ResultChanged: PropKey<ValueChangedEventArgs<'TResult> -> unit> =
      PropKey.Create.event (297, "Popover.ResultChanged_event")

  type PopoverMenuPKeys() =
    inherit PopoverPKeys<Terminal.Gui.Views.Menu, Terminal.Gui.Views.MenuItem>()

    // Properties
    member val Key: PropKey<Terminal.Gui.Input.Key> = PropKey.Create.simple (298, "PopoverMenu.Key")

    member val MouseFlags: PropKey<Terminal.Gui.Input.MouseFlags> =
      PropKey.Create.simple (299, "PopoverMenu.MouseFlags")

    member val Root: PropKey<Terminal.Gui.Views.Menu> = PropKey.Create.view (300, 301, "PopoverMenu.Root_view")
    member val Root_viewSpec: PropKey<IMenuView> = PropKey.Create.subElement (301, 300, "PopoverMenu.Root_viewSpec")

    // Events
    member val KeyChanged: PropKey<Terminal.Gui.Input.KeyChangedEventArgs -> unit> =
      PropKey.Create.event (302, "PopoverMenu.KeyChanged_event")

  type ProgressBarPKeys() =
    inherit ViewPKeys()

    // Properties
    member val BidirectionalMarquee: PropKey<bool> = PropKey.Create.simple (303, "ProgressBar.BidirectionalMarquee")
    member val Fraction: PropKey<System.Single> = PropKey.Create.simple (304, "ProgressBar.Fraction")

    member val ProgressBarFormat: PropKey<Terminal.Gui.Views.ProgressBarFormat> =
      PropKey.Create.simple (305, "ProgressBar.ProgressBarFormat")

    member val ProgressBarStyle: PropKey<Terminal.Gui.Views.ProgressBarStyle> =
      PropKey.Create.simple (306, "ProgressBar.ProgressBarStyle")

    member val SegmentCharacter: PropKey<System.Text.Rune> = PropKey.Create.simple (307, "ProgressBar.SegmentCharacter")
    member val SyncWithTerminal: PropKey<bool> = PropKey.Create.simple (308, "ProgressBar.SyncWithTerminal")
    member val Text: PropKey<string> = PropKey.Create.simple (309, "ProgressBar.Text")

  type RunnablePKeys() =
    inherit ViewPKeys()

    // Properties
    member val Result: PropKey<System.Object> = PropKey.Create.simple (310, "Runnable.Result")
    member val StopRequested: PropKey<bool> = PropKey.Create.simple (311, "Runnable.StopRequested")

    // Events
    member val IsModalChanged: PropKey<EventArgs<bool> -> unit> =
      PropKey.Create.event (312, "Runnable.IsModalChanged_event")

    member val IsRunningChanged: PropKey<EventArgs<bool> -> unit> =
      PropKey.Create.event (313, "Runnable.IsRunningChanged_event")

    member val IsRunningChanging: PropKey<CancelEventArgs<bool> -> unit> =
      PropKey.Create.event (314, "Runnable.IsRunningChanging_event")

  type RunnablePKeys<'TResult>() =
    inherit RunnablePKeys()

    // Properties
    member val Result: PropKey<'TResult> = PropKey.Create.simple (315, "Runnable.Result")

  type DialogPKeys<'TResult>() =
    inherit RunnablePKeys<'TResult>()

    // Properties
    member val ButtonAlignment: PropKey<Terminal.Gui.ViewBase.Alignment> =
      PropKey.Create.simple (316, "Dialog.ButtonAlignment")

    member val ButtonAlignmentModes: PropKey<Terminal.Gui.ViewBase.AlignmentModes> =
      PropKey.Create.simple (317, "Dialog.ButtonAlignmentModes")

    member val Buttons: PropKey<Terminal.Gui.Views.Button[]> = PropKey.Create.simple (318, "Dialog.Buttons")

  type RunnableWrapperPKeys<'TView, 'TResult when 'TView: (new: unit -> 'TView) and 'TView :> Terminal.Gui.ViewBase.View>
    () =
    inherit RunnablePKeys<'TResult>()

    // Properties
    member val ResultExtractor: PropKey<Func<'TView, 'TResult>> =
      PropKey.Create.simple (319, "RunnableWrapper.ResultExtractor")

  type DialogPKeys() =
    inherit DialogPKeys<int>()

    // Properties
    member val Result: PropKey<Nullable<int>> = PropKey.Create.simple (320, "Dialog.Result")

  type FileDialogPKeys() =
    inherit DialogPKeys<IReadOnlyList<string>>()

    // Properties
    member val AllowedTypes: PropKey<List<Terminal.Gui.Views.IAllowedType>> =
      PropKey.Create.simple (321, "FileDialog.AllowedTypes")

    member val AllowsMultipleSelection: PropKey<bool> =
      PropKey.Create.simple (322, "FileDialog.AllowsMultipleSelection")

    member val FileOperationsHandler: PropKey<Terminal.Gui.FileServices.IFileOperations> =
      PropKey.Create.simple (323, "FileDialog.FileOperationsHandler")

    member val MustExist: PropKey<bool> = PropKey.Create.simple (324, "FileDialog.MustExist")
    member val OpenMode: PropKey<Terminal.Gui.Views.OpenMode> = PropKey.Create.simple (325, "FileDialog.OpenMode")
    member val Path: PropKey<string> = PropKey.Create.simple (326, "FileDialog.Path")

    member val SearchMatcher: PropKey<Terminal.Gui.FileServices.ISearchMatcher> =
      PropKey.Create.simple (327, "FileDialog.SearchMatcher")

    // Events
    member val FilesSelected: PropKey<Terminal.Gui.Views.FilesSelectedEventArgs -> unit> =
      PropKey.Create.event (328, "FileDialog.FilesSelected_event")

  type PromptPKeys<'TView, 'TResult when 'TView: (new: unit -> 'TView) and 'TView :> Terminal.Gui.ViewBase.View>() =
    inherit DialogPKeys<'TResult>()

    // Properties
    member val ResultExtractor: PropKey<Func<'TView, 'TResult>> = PropKey.Create.simple (329, "Prompt.ResultExtractor")

  type OpenDialogPKeys() =
    inherit FileDialogPKeys()

    // Properties
    member val OpenMode: PropKey<Terminal.Gui.Views.OpenMode> = PropKey.Create.simple (330, "OpenDialog.OpenMode")

  type SaveDialogPKeys() =
    inherit FileDialogPKeys()


  type ScrollBarPKeys() =
    inherit ViewPKeys()

    // Properties
    member val Increment: PropKey<int> = PropKey.Create.simple (331, "ScrollBar.Increment")

    member val Orientation: PropKey<Terminal.Gui.ViewBase.Orientation> =
      PropKey.Create.simple (332, "ScrollBar.Orientation")

    member val ScrollableContentSize: PropKey<int> = PropKey.Create.simple (333, "ScrollBar.ScrollableContentSize")
    member val Value: PropKey<int> = PropKey.Create.simple (334, "ScrollBar.Value")

    member val VisibilityMode: PropKey<Terminal.Gui.Views.ScrollBarVisibilityMode> =
      PropKey.Create.simple (335, "ScrollBar.VisibilityMode")

    member val VisibleContentSize: PropKey<int> = PropKey.Create.simple (336, "ScrollBar.VisibleContentSize")

    // Events
    member val OrientationChanged: PropKey<EventArgs<Terminal.Gui.ViewBase.Orientation> -> unit> =
      PropKey.Create.event (337, "ScrollBar.OrientationChanged_event")

    member val OrientationChanging: PropKey<CancelEventArgs<Terminal.Gui.ViewBase.Orientation> -> unit> =
      PropKey.Create.event (338, "ScrollBar.OrientationChanging_event")

    member val ScrollableContentSizeChanged: PropKey<EventArgs<int> -> unit> =
      PropKey.Create.event (339, "ScrollBar.ScrollableContentSizeChanged_event")

    member val Scrolled: PropKey<EventArgs<int> -> unit> = PropKey.Create.event (340, "ScrollBar.Scrolled_event")

    member val SliderPositionChanged: PropKey<EventArgs<int> -> unit> =
      PropKey.Create.event (341, "ScrollBar.SliderPositionChanged_event")

    member val ValueChanged: PropKey<ValueChangedEventArgs<int> -> unit> =
      PropKey.Create.event (342, "ScrollBar.ValueChanged_event")

    member val ValueChangedUntyped: PropKey<ValueChangedEventArgs<System.Object> -> unit> =
      PropKey.Create.event (343, "ScrollBar.ValueChangedUntyped_event")

    member val ValueChanging: PropKey<ValueChangingEventArgs<int> -> unit> =
      PropKey.Create.event (344, "ScrollBar.ValueChanging_event")

  type ScrollButtonPKeys() =
    inherit ButtonPKeys()

    // Properties
    member val Direction: PropKey<Terminal.Gui.ViewBase.NavigationDirection> =
      PropKey.Create.simple (345, "ScrollButton.Direction")

    member val Orientation: PropKey<Terminal.Gui.ViewBase.Orientation> =
      PropKey.Create.simple (346, "ScrollButton.Orientation")

    // Events
    member val OrientationChanged: PropKey<EventArgs<Terminal.Gui.ViewBase.Orientation> -> unit> =
      PropKey.Create.event (347, "ScrollButton.OrientationChanged_event")

    member val OrientationChanging: PropKey<CancelEventArgs<Terminal.Gui.ViewBase.Orientation> -> unit> =
      PropKey.Create.event (348, "ScrollButton.OrientationChanging_event")

  type ScrollSliderPKeys() =
    inherit ViewPKeys()

    // Properties
    member val Orientation: PropKey<Terminal.Gui.ViewBase.Orientation> =
      PropKey.Create.simple (349, "ScrollSlider.Orientation")

    member val Position: PropKey<int> = PropKey.Create.simple (350, "ScrollSlider.Position")
    member val Size: PropKey<int> = PropKey.Create.simple (351, "ScrollSlider.Size")
    member val SliderPadding: PropKey<int> = PropKey.Create.simple (352, "ScrollSlider.SliderPadding")
    member val Value: PropKey<int> = PropKey.Create.simple (353, "ScrollSlider.Value")
    member val VisibleContentSize: PropKey<int> = PropKey.Create.simple (354, "ScrollSlider.VisibleContentSize")

    // Events
    member val OrientationChanged: PropKey<EventArgs<Terminal.Gui.ViewBase.Orientation> -> unit> =
      PropKey.Create.event (355, "ScrollSlider.OrientationChanged_event")

    member val OrientationChanging: PropKey<CancelEventArgs<Terminal.Gui.ViewBase.Orientation> -> unit> =
      PropKey.Create.event (356, "ScrollSlider.OrientationChanging_event")

    member val PositionChanged: PropKey<EventArgs<int> -> unit> =
      PropKey.Create.event (357, "ScrollSlider.PositionChanged_event")

    member val PositionChanging: PropKey<CancelEventArgs<int> -> unit> =
      PropKey.Create.event (358, "ScrollSlider.PositionChanging_event")

    member val Scrolled: PropKey<EventArgs<int> -> unit> = PropKey.Create.event (359, "ScrollSlider.Scrolled_event")

    member val ValueChanged: PropKey<ValueChangedEventArgs<int> -> unit> =
      PropKey.Create.event (360, "ScrollSlider.ValueChanged_event")

    member val ValueChangedUntyped: PropKey<ValueChangedEventArgs<System.Object> -> unit> =
      PropKey.Create.event (361, "ScrollSlider.ValueChangedUntyped_event")

    member val ValueChanging: PropKey<ValueChangingEventArgs<int> -> unit> =
      PropKey.Create.event (362, "ScrollSlider.ValueChanging_event")

  type SelectorBasePKeys() =
    inherit ViewPKeys()

    // Properties
    member val DoubleClickAccepts: PropKey<bool> = PropKey.Create.simple (363, "SelectorBase.DoubleClickAccepts")
    member val HorizontalSpace: PropKey<int> = PropKey.Create.simple (364, "SelectorBase.HorizontalSpace")
    member val Labels: PropKey<IReadOnlyList<string>> = PropKey.Create.simple (365, "SelectorBase.Labels")

    member val Orientation: PropKey<Terminal.Gui.ViewBase.Orientation> =
      PropKey.Create.simple (366, "SelectorBase.Orientation")

    member val Styles: PropKey<Terminal.Gui.Views.SelectorStyles> = PropKey.Create.simple (367, "SelectorBase.Styles")

    member val TabBehavior: PropKey<Nullable<Terminal.Gui.ViewBase.TabBehavior>> =
      PropKey.Create.simple (368, "SelectorBase.TabBehavior")

    member val Value: PropKey<Nullable<int>> = PropKey.Create.simple (369, "SelectorBase.Value")
    member val Values: PropKey<IReadOnlyList<int>> = PropKey.Create.simple (370, "SelectorBase.Values")

    // Events
    member val OrientationChanged: PropKey<EventArgs<Terminal.Gui.ViewBase.Orientation> -> unit> =
      PropKey.Create.event (371, "SelectorBase.OrientationChanged_event")

    member val OrientationChanging: PropKey<CancelEventArgs<Terminal.Gui.ViewBase.Orientation> -> unit> =
      PropKey.Create.event (372, "SelectorBase.OrientationChanging_event")

    member val ValueChanged: PropKey<ValueChangedEventArgs<Nullable<int>> -> unit> =
      PropKey.Create.event (373, "SelectorBase.ValueChanged_event")

    member val ValueChangedUntyped: PropKey<ValueChangedEventArgs<System.Object> -> unit> =
      PropKey.Create.event (374, "SelectorBase.ValueChangedUntyped_event")

    member val ValueChanging: PropKey<ValueChangingEventArgs<Nullable<int>> -> unit> =
      PropKey.Create.event (375, "SelectorBase.ValueChanging_event")

  type FlagSelectorPKeys() =
    inherit SelectorBasePKeys()

    // Properties
    member val Value: PropKey<Nullable<int>> = PropKey.Create.simple (376, "FlagSelector.Value")

  type OptionSelectorPKeys() =
    inherit SelectorBasePKeys()

    // Properties
    member val FocusedItem: PropKey<int> = PropKey.Create.simple (377, "OptionSelector.FocusedItem")

  type FlagSelectorPKeys<'TFlagsEnum
    when 'TFlagsEnum: struct
    and 'TFlagsEnum: (new: unit -> 'TFlagsEnum)
    and 'TFlagsEnum :> System.Enum
    and 'TFlagsEnum :> System.ValueType>() =
    inherit FlagSelectorPKeys()

    // Properties
    member val Value: PropKey<Nullable<'TFlagsEnum>> = PropKey.Create.simple (378, "FlagSelector.Value")

    // Events
    member val ValueChanged: PropKey<EventArgs<Nullable<'TFlagsEnum>> -> unit> =
      PropKey.Create.event (379, "FlagSelector.ValueChanged_event")

  type OptionSelectorPKeys<'TEnum
    when 'TEnum: struct and 'TEnum: (new: unit -> 'TEnum) and 'TEnum :> System.Enum and 'TEnum :> System.ValueType>() =
    inherit OptionSelectorPKeys()

    // Properties
    member val Value: PropKey<Nullable<'TEnum>> = PropKey.Create.simple (380, "OptionSelector.Value")
    member val Values: PropKey<IReadOnlyList<int>> = PropKey.Create.simple (381, "OptionSelector.Values")

    // Events
    member val ValueChanged: PropKey<EventArgs<Nullable<'TEnum>> -> unit> =
      PropKey.Create.event (382, "OptionSelector.ValueChanged_event")

  type ShortcutPKeys() =
    inherit ViewPKeys()

    // Properties
    member val Action: PropKey<System.Action> = PropKey.Create.simple (383, "Shortcut.Action")

    member val AlignmentModes: PropKey<Terminal.Gui.ViewBase.AlignmentModes> =
      PropKey.Create.simple (384, "Shortcut.AlignmentModes")

    member val BindKeyToApplication: PropKey<bool> = PropKey.Create.simple (385, "Shortcut.BindKeyToApplication")
    member val Command: PropKey<Terminal.Gui.Input.Command> = PropKey.Create.simple (386, "Shortcut.Command")

    member val CommandView: PropKey<Terminal.Gui.ViewBase.View> =
      PropKey.Create.view (387, 388, "Shortcut.CommandView_view")

    member val CommandView_viewSpec: PropKey<IView> =
      PropKey.Create.subElement (388, 387, "Shortcut.CommandView_viewSpec")

    member val HelpText: PropKey<string> = PropKey.Create.simple (389, "Shortcut.HelpText")
    member val Key: PropKey<Terminal.Gui.Input.Key> = PropKey.Create.simple (390, "Shortcut.Key")
    member val MinimumKeyTextSize: PropKey<int> = PropKey.Create.simple (391, "Shortcut.MinimumKeyTextSize")

    member val Orientation: PropKey<Terminal.Gui.ViewBase.Orientation> =
      PropKey.Create.simple (392, "Shortcut.Orientation")

    member val TargetView: PropKey<Terminal.Gui.ViewBase.View> =
      PropKey.Create.view (393, 394, "Shortcut.TargetView_view")

    member val TargetView_viewSpec: PropKey<IView> =
      PropKey.Create.subElement (394, 393, "Shortcut.TargetView_viewSpec")

    member val Text: PropKey<string> = PropKey.Create.simple (395, "Shortcut.Text")

    // Events
    member val OrientationChanged: PropKey<EventArgs<Terminal.Gui.ViewBase.Orientation> -> unit> =
      PropKey.Create.event (396, "Shortcut.OrientationChanged_event")

    member val OrientationChanging: PropKey<CancelEventArgs<Terminal.Gui.ViewBase.Orientation> -> unit> =
      PropKey.Create.event (397, "Shortcut.OrientationChanging_event")

  type MenuItemPKeys() =
    inherit ShortcutPKeys()

    // Properties
    member val SubMenu: PropKey<Terminal.Gui.Views.Menu> = PropKey.Create.view (398, 399, "MenuItem.SubMenu_view")
    member val SubMenu_viewSpec: PropKey<IMenuView> = PropKey.Create.subElement (399, 398, "MenuItem.SubMenu_viewSpec")

  type MenuBarItemPKeys() =
    inherit MenuItemPKeys()

    // Properties
    member val PopoverMenu: PropKey<Terminal.Gui.Views.PopoverMenu> =
      PropKey.Create.view (400, 401, "MenuBarItem.PopoverMenu_view")

    member val PopoverMenu_viewSpec: PropKey<IPopoverMenuView> =
      PropKey.Create.subElement (401, 400, "MenuBarItem.PopoverMenu_viewSpec")

    member val PopoverMenuOpen: PropKey<bool> = PropKey.Create.simple (402, "MenuBarItem.PopoverMenuOpen")

    // Events
    member val MenuOpenChanged: PropKey<ValueChangedEventArgs<bool> -> unit> =
      PropKey.Create.event (403, "MenuBarItem.MenuOpenChanged_event")

    member val PopoverMenuOpenChanged: PropKey<ValueChangedEventArgs<bool> -> unit> =
      PropKey.Create.event (404, "MenuBarItem.PopoverMenuOpenChanged_event")

  type SpinnerViewPKeys() =
    inherit ViewPKeys()

    // Properties
    member val AutoSpin: PropKey<bool> = PropKey.Create.simple (405, "SpinnerView.AutoSpin")
    member val Sequence: PropKey<System.String[]> = PropKey.Create.simple (406, "SpinnerView.Sequence")
    member val SpinBounce: PropKey<bool> = PropKey.Create.simple (407, "SpinnerView.SpinBounce")
    member val SpinDelay: PropKey<int> = PropKey.Create.simple (408, "SpinnerView.SpinDelay")
    member val SpinReverse: PropKey<bool> = PropKey.Create.simple (409, "SpinnerView.SpinReverse")
    member val Style: PropKey<Terminal.Gui.Views.SpinnerStyle> = PropKey.Create.simple (410, "SpinnerView.Style")
    member val SyncWithTerminal: PropKey<bool> = PropKey.Create.simple (411, "SpinnerView.SyncWithTerminal")

  type StatusBarPKeys() =
    inherit BarPKeys()


  type TableViewPKeys() =
    inherit ViewPKeys()

    // Properties
    member val CollectionNavigator: PropKey<Terminal.Gui.Views.ICollectionNavigator> =
      PropKey.Create.simple (412, "TableView.CollectionNavigator")

    member val ColumnOffset: PropKey<int> = PropKey.Create.simple (413, "TableView.ColumnOffset")
    member val FullRowSelect: PropKey<bool> = PropKey.Create.simple (414, "TableView.FullRowSelect")
    member val MaxCellWidth: PropKey<int> = PropKey.Create.simple (415, "TableView.MaxCellWidth")
    member val MinCellWidth: PropKey<int> = PropKey.Create.simple (416, "TableView.MinCellWidth")
    member val MultiSelect: PropKey<bool> = PropKey.Create.simple (417, "TableView.MultiSelect")
    member val NullSymbol: PropKey<string> = PropKey.Create.simple (418, "TableView.NullSymbol")
    member val RowOffset: PropKey<int> = PropKey.Create.simple (419, "TableView.RowOffset")
    member val SeparatorSymbol: PropKey<System.Char> = PropKey.Create.simple (420, "TableView.SeparatorSymbol")
    member val Style: PropKey<Terminal.Gui.Views.TableStyle> = PropKey.Create.simple (421, "TableView.Style")
    member val Table: PropKey<Terminal.Gui.Views.ITableSource> = PropKey.Create.simple (422, "TableView.Table")

    member val UseAllRowsForContentCalculation: PropKey<bool> =
      PropKey.Create.simple (423, "TableView.UseAllRowsForContentCalculation")

    member val Value: PropKey<Terminal.Gui.Views.TableSelection> = PropKey.Create.simple (424, "TableView.Value")

    // Events
    member val ValueChanged: PropKey<ValueChangedEventArgs<Terminal.Gui.Views.TableSelection> -> unit> =
      PropKey.Create.event (425, "TableView.ValueChanged_event")

    member val ValueChangedUntyped: PropKey<ValueChangedEventArgs<System.Object> -> unit> =
      PropKey.Create.event (426, "TableView.ValueChangedUntyped_event")

    member val ValueChanging: PropKey<ValueChangingEventArgs<Terminal.Gui.Views.TableSelection> -> unit> =
      PropKey.Create.event (427, "TableView.ValueChanging_event")

  type TabsPKeys() =
    inherit ViewPKeys()

    // Properties
    member val ScrollOffset: PropKey<int> = PropKey.Create.simple (428, "Tabs.ScrollOffset")
    member val TabDepth: PropKey<int> = PropKey.Create.simple (429, "Tabs.TabDepth")
    member val TabLineStyle: PropKey<Terminal.Gui.Drawing.LineStyle> = PropKey.Create.simple (430, "Tabs.TabLineStyle")
    member val TabSide: PropKey<Terminal.Gui.ViewBase.Side> = PropKey.Create.simple (431, "Tabs.TabSide")
    member val TabSpacing: PropKey<int> = PropKey.Create.simple (432, "Tabs.TabSpacing")
    member val Value: PropKey<Terminal.Gui.ViewBase.View> = PropKey.Create.view (433, 434, "Tabs.Value_view")
    member val Value_viewSpec: PropKey<IView> = PropKey.Create.subElement (434, 433, "Tabs.Value_viewSpec")

    // Events
    member val ValueChanged: PropKey<ValueChangedEventArgs<Terminal.Gui.ViewBase.View> -> unit> =
      PropKey.Create.event (435, "Tabs.ValueChanged_event")

    member val ValueChangedUntyped: PropKey<ValueChangedEventArgs<System.Object> -> unit> =
      PropKey.Create.event (436, "Tabs.ValueChangedUntyped_event")

    member val ValueChanging: PropKey<ValueChangingEventArgs<Terminal.Gui.ViewBase.View> -> unit> =
      PropKey.Create.event (437, "Tabs.ValueChanging_event")

  type TextFieldPKeys() =
    inherit ViewPKeys()

    // Properties
    member val Autocomplete: PropKey<Terminal.Gui.Views.IAutocomplete> =
      PropKey.Create.simple (438, "TextField.Autocomplete")

    member val InsertionPoint: PropKey<int> = PropKey.Create.simple (439, "TextField.InsertionPoint")
    member val ReadOnly: PropKey<bool> = PropKey.Create.simple (440, "TextField.ReadOnly")
    member val Secret: PropKey<bool> = PropKey.Create.simple (441, "TextField.Secret")

    member val SelectWordOnlyOnDoubleClick: PropKey<bool> =
      PropKey.Create.simple (442, "TextField.SelectWordOnlyOnDoubleClick")

    member val SelectedStart: PropKey<int> = PropKey.Create.simple (443, "TextField.SelectedStart")
    member val Text: PropKey<string> = PropKey.Create.simple (444, "TextField.Text")
    member val UseSameRuneTypeForWords: PropKey<bool> = PropKey.Create.simple (445, "TextField.UseSameRuneTypeForWords")
    member val Used: PropKey<bool> = PropKey.Create.simple (446, "TextField.Used")
    member val Value: PropKey<string> = PropKey.Create.simple (447, "TextField.Value")

    // Events
    member val TextChanging: PropKey<ResultEventArgs<string> -> unit> =
      PropKey.Create.event (448, "TextField.TextChanging_event")

    member val ValueChanged: PropKey<ValueChangedEventArgs<string> -> unit> =
      PropKey.Create.event (449, "TextField.ValueChanged_event")

    member val ValueChangedUntyped: PropKey<ValueChangedEventArgs<System.Object> -> unit> =
      PropKey.Create.event (450, "TextField.ValueChangedUntyped_event")

    member val ValueChanging: PropKey<ValueChangingEventArgs<string> -> unit> =
      PropKey.Create.event (451, "TextField.ValueChanging_event")

  type DropDownListPKeys() =
    inherit TextFieldPKeys()

    // Properties
    member val Source: PropKey<Terminal.Gui.Views.IListDataSource> = PropKey.Create.simple (452, "DropDownList.Source")

  type DropDownListPKeys<'TEnum
    when 'TEnum: struct and 'TEnum: (new: unit -> 'TEnum) and 'TEnum :> System.Enum and 'TEnum :> System.ValueType>() =
    inherit DropDownListPKeys()

    // Properties
    member val Value: PropKey<Nullable<'TEnum>> = PropKey.Create.simple (453, "DropDownList.Value")

    // Events
    member val ValueChanged: PropKey<EventArgs<Nullable<'TEnum>> -> unit> =
      PropKey.Create.event (454, "DropDownList.ValueChanged_event")

  type TextValidateFieldPKeys() =
    inherit ViewPKeys()

    // Properties
    member val Provider: PropKey<Terminal.Gui.Views.ITextValidateProvider> =
      PropKey.Create.simple (455, "TextValidateField.Provider")

    member val Text: PropKey<string> = PropKey.Create.simple (456, "TextValidateField.Text")
    member val Value: PropKey<string> = PropKey.Create.simple (457, "TextValidateField.Value")

    // Events
    member val ValueChanged: PropKey<ValueChangedEventArgs<string> -> unit> =
      PropKey.Create.event (458, "TextValidateField.ValueChanged_event")

    member val ValueChangedUntyped: PropKey<ValueChangedEventArgs<System.Object> -> unit> =
      PropKey.Create.event (459, "TextValidateField.ValueChangedUntyped_event")

    member val ValueChanging: PropKey<ValueChangingEventArgs<string> -> unit> =
      PropKey.Create.event (460, "TextValidateField.ValueChanging_event")

  type DateEditorPKeys() =
    inherit TextValidateFieldPKeys()

    // Properties
    member val Format: PropKey<System.Globalization.DateTimeFormatInfo> =
      PropKey.Create.simple (461, "DateEditor.Format")

    member val Value: PropKey<System.DateTime> = PropKey.Create.simple (462, "DateEditor.Value")

    // Events
    member val ValueChanged: PropKey<ValueChangedEventArgs<System.DateTime> -> unit> =
      PropKey.Create.event (463, "DateEditor.ValueChanged_event")

    member val ValueChangedUntyped: PropKey<ValueChangedEventArgs<System.Object> -> unit> =
      PropKey.Create.event (464, "DateEditor.ValueChangedUntyped_event")

    member val ValueChanging: PropKey<ValueChangingEventArgs<System.DateTime> -> unit> =
      PropKey.Create.event (465, "DateEditor.ValueChanging_event")

  type TextViewPKeys() =
    inherit ViewPKeys()

    // Properties
    member val EnterKeyAddsLine: PropKey<bool> = PropKey.Create.simple (466, "TextView.EnterKeyAddsLine")

    member val InheritsPreviousAttribute: PropKey<bool> =
      PropKey.Create.simple (467, "TextView.InheritsPreviousAttribute")

    member val InsertionPoint: PropKey<System.Drawing.Point> = PropKey.Create.simple (468, "TextView.InsertionPoint")
    member val IsSelecting: PropKey<bool> = PropKey.Create.simple (469, "TextView.IsSelecting")
    member val Multiline: PropKey<bool> = PropKey.Create.simple (470, "TextView.Multiline")
    member val ReadOnly: PropKey<bool> = PropKey.Create.simple (471, "TextView.ReadOnly")
    member val ScrollBars: PropKey<bool> = PropKey.Create.simple (472, "TextView.ScrollBars")

    member val SelectWordOnlyOnDoubleClick: PropKey<bool> =
      PropKey.Create.simple (473, "TextView.SelectWordOnlyOnDoubleClick")

    member val SelectionStartColumn: PropKey<int> = PropKey.Create.simple (474, "TextView.SelectionStartColumn")
    member val SelectionStartRow: PropKey<int> = PropKey.Create.simple (475, "TextView.SelectionStartRow")
    member val TabKeyAddsTab: PropKey<bool> = PropKey.Create.simple (476, "TextView.TabKeyAddsTab")
    member val TabWidth: PropKey<int> = PropKey.Create.simple (477, "TextView.TabWidth")
    member val Text: PropKey<string> = PropKey.Create.simple (478, "TextView.Text")
    member val UseSameRuneTypeForWords: PropKey<bool> = PropKey.Create.simple (479, "TextView.UseSameRuneTypeForWords")
    member val Used: PropKey<bool> = PropKey.Create.simple (480, "TextView.Used")
    member val WordWrap: PropKey<bool> = PropKey.Create.simple (481, "TextView.WordWrap")

    // Events
    member val ContentsChanged: PropKey<Terminal.Gui.Views.ContentsChangedEventArgs -> unit> =
      PropKey.Create.event (482, "TextView.ContentsChanged_event")

    member val DrawNormalColor: PropKey<Terminal.Gui.Drawing.CellEventArgs -> unit> =
      PropKey.Create.event (483, "TextView.DrawNormalColor_event")

    member val DrawReadOnlyColor: PropKey<Terminal.Gui.Drawing.CellEventArgs -> unit> =
      PropKey.Create.event (484, "TextView.DrawReadOnlyColor_event")

    member val DrawSelectionColor: PropKey<Terminal.Gui.Drawing.CellEventArgs -> unit> =
      PropKey.Create.event (485, "TextView.DrawSelectionColor_event")

    member val DrawUsedColor: PropKey<Terminal.Gui.Drawing.CellEventArgs -> unit> =
      PropKey.Create.event (486, "TextView.DrawUsedColor_event")

    member val UnwrappedCursorPositionChanged: PropKey<System.Drawing.Point -> unit> =
      PropKey.Create.event (487, "TextView.UnwrappedCursorPositionChanged_event")

  type TimeEditorPKeys() =
    inherit TextValidateFieldPKeys()

    // Properties
    member val Format: PropKey<System.Globalization.DateTimeFormatInfo> =
      PropKey.Create.simple (488, "TimeEditor.Format")

    member val Value: PropKey<System.TimeSpan> = PropKey.Create.simple (489, "TimeEditor.Value")

    // Events
    member val ValueChanged: PropKey<ValueChangedEventArgs<System.TimeSpan> -> unit> =
      PropKey.Create.event (490, "TimeEditor.ValueChanged_event")

    member val ValueChangedUntyped: PropKey<ValueChangedEventArgs<System.Object> -> unit> =
      PropKey.Create.event (491, "TimeEditor.ValueChangedUntyped_event")

    member val ValueChanging: PropKey<ValueChangingEventArgs<System.TimeSpan> -> unit> =
      PropKey.Create.event (492, "TimeEditor.ValueChanging_event")

  type TitleViewPKeys() =
    inherit ViewPKeys()

    // Properties
    member val Direction: PropKey<Terminal.Gui.ViewBase.NavigationDirection> =
      PropKey.Create.simple (493, "TitleView.Direction")

    member val MeasuredTabLength: PropKey<int> = PropKey.Create.simple (494, "TitleView.MeasuredTabLength")

    member val Orientation: PropKey<Terminal.Gui.ViewBase.Orientation> =
      PropKey.Create.simple (495, "TitleView.Orientation")

    member val TabDepth: PropKey<int> = PropKey.Create.simple (496, "TitleView.TabDepth")
    member val TabSide: PropKey<Terminal.Gui.ViewBase.Side> = PropKey.Create.simple (497, "TitleView.TabSide")
    member val Text: PropKey<string> = PropKey.Create.simple (498, "TitleView.Text")

    // Events
    member val OrientationChanged: PropKey<EventArgs<Terminal.Gui.ViewBase.Orientation> -> unit> =
      PropKey.Create.event (499, "TitleView.OrientationChanged_event")

    member val OrientationChanging: PropKey<CancelEventArgs<Terminal.Gui.ViewBase.Orientation> -> unit> =
      PropKey.Create.event (500, "TitleView.OrientationChanging_event")

  type ToolTipHostPKeys<'TView when 'TView: (new: unit -> 'TView) and 'TView :> Terminal.Gui.ViewBase.View>() =
    inherit PopoverImplPKeys()

    // Properties
    member val ContentView: PropKey<'TView> = PropKey.Create.view (501, 502, "ToolTipHost.ContentView_view")

    member val ContentView_viewSpec: PropKey<ITViewView> =
      PropKey.Create.subElement (502, 501, "ToolTipHost.ContentView_viewSpec")

  type TreeViewPKeys<'T when 'T: not struct>() =
    inherit ViewPKeys()

    // Properties
    member val AllowLetterBasedNavigation: PropKey<bool> =
      PropKey.Create.simple (503, "TreeView.AllowLetterBasedNavigation")

    member val AspectGetter: PropKey<AspectGetterDelegate<'T>> = PropKey.Create.simple (504, "TreeView.AspectGetter")
    member val CheckboxMode: PropKey<bool> = PropKey.Create.simple (505, "TreeView.CheckboxMode")

    member val ColorGetter: PropKey<Func<'T, Terminal.Gui.Drawing.Scheme>> =
      PropKey.Create.simple (506, "TreeView.ColorGetter")

    member val Filter: PropKey<ITreeViewFilter<'T>> = PropKey.Create.simple (507, "TreeView.Filter")
    member val MaxDepth: PropKey<int> = PropKey.Create.simple (508, "TreeView.MaxDepth")
    member val MultiSelect: PropKey<bool> = PropKey.Create.simple (509, "TreeView.MultiSelect")
    member val ScrollOffsetHorizontal: PropKey<int> = PropKey.Create.simple (510, "TreeView.ScrollOffsetHorizontal")
    member val ScrollOffsetVertical: PropKey<int> = PropKey.Create.simple (511, "TreeView.ScrollOffsetVertical")
    member val SelectedObject: PropKey<'T> = PropKey.Create.simple (512, "TreeView.SelectedObject")
    member val Style: PropKey<Terminal.Gui.Views.TreeStyle> = PropKey.Create.simple (513, "TreeView.Style")
    member val TreeBuilder: PropKey<ITreeBuilder<'T>> = PropKey.Create.simple (514, "TreeView.TreeBuilder")

    // Events
    member val CheckedChanged: PropKey<CheckedChangedEventArgs<'T> -> unit> =
      PropKey.Create.event (515, "TreeView.CheckedChanged_event")

    member val DrawLine: PropKey<DrawTreeViewLineEventArgs<'T> -> unit> =
      PropKey.Create.event (516, "TreeView.DrawLine_event")

    member val SelectionChanged: PropKey<SelectionChangedEventArgs<'T> -> unit> =
      PropKey.Create.event (517, "TreeView.SelectionChanged_event")

  type TreeViewPKeys() =
    inherit TreeViewPKeys<Terminal.Gui.Views.ITreeNode>()


  type WindowPKeys() =
    inherit RunnablePKeys()


  type WizardPKeys() =
    inherit DialogPKeys()

    // Properties
    member val CurrentStep: PropKey<Terminal.Gui.Views.WizardStep> =
      PropKey.Create.view (518, 519, "Wizard.CurrentStep_view")

    member val CurrentStep_viewSpec: PropKey<IWizardStepView> =
      PropKey.Create.subElement (519, 518, "Wizard.CurrentStep_viewSpec")

    // Events
    member val MovingBack: PropKey<System.ComponentModel.CancelEventArgs -> unit> =
      PropKey.Create.event (520, "Wizard.MovingBack_event")

    member val MovingNext: PropKey<System.ComponentModel.CancelEventArgs -> unit> =
      PropKey.Create.event (521, "Wizard.MovingNext_event")

    member val StepChanged: PropKey<ValueChangedEventArgs<Terminal.Gui.Views.WizardStep> -> unit> =
      PropKey.Create.event (522, "Wizard.StepChanged_event")

    member val StepChanging: PropKey<ValueChangingEventArgs<Terminal.Gui.Views.WizardStep> -> unit> =
      PropKey.Create.event (523, "Wizard.StepChanging_event")

  type WizardStepPKeys() =
    inherit ViewPKeys()

    // Properties
    member val BackButtonText: PropKey<string> = PropKey.Create.simple (524, "WizardStep.BackButtonText")
    member val HelpText: PropKey<string> = PropKey.Create.simple (525, "WizardStep.HelpText")
    member val NextButtonText: PropKey<string> = PropKey.Create.simple (526, "WizardStep.NextButtonText")

  module internal IAdornmentInterface =
    // Properties
    let Parent: PropKey<Terminal.Gui.ViewBase.View> =
      PropKey.Create.simple (527, "IAdornmentInterface.Parent")

    let Thickness: PropKey<Terminal.Gui.Drawing.Thickness> =
      PropKey.Create.simple (528, "IAdornmentInterface.Thickness")

    // Events
    let ThicknessChanged: PropKey<System.EventArgs -> unit> =
      PropKey.Create.event (529, "IAdornmentInterface.ThicknessChanged_event")

  module internal IAdornmentViewInterface =
    // Properties
    let Adornment: PropKey<Terminal.Gui.ViewBase.IAdornment> =
      PropKey.Create.simple (530, "IAdornmentViewInterface.Adornment")

  module internal IMouseHoldRepeaterInterface =
    // Properties
    let Timeout: PropKey<Terminal.Gui.App.Timeout> =
      PropKey.Create.simple (531, "IMouseHoldRepeaterInterface.Timeout")

    // Events
    let MouseIsHeldDownTick: PropKey<CancelEventArgs<Terminal.Gui.Input.Mouse> -> unit> =
      PropKey.Create.event (532, "IMouseHoldRepeaterInterface.MouseIsHeldDownTick_event")

  module internal IOrientationInterface =
    // Properties
    let Orientation: PropKey<Terminal.Gui.ViewBase.Orientation> =
      PropKey.Create.simple (533, "IOrientationInterface.Orientation")

    // Events
    let OrientationChanged: PropKey<EventArgs<Terminal.Gui.ViewBase.Orientation> -> unit> =
      PropKey.Create.event (534, "IOrientationInterface.OrientationChanged_event")

    let OrientationChanging: PropKey<CancelEventArgs<Terminal.Gui.ViewBase.Orientation> -> unit> =
      PropKey.Create.event (535, "IOrientationInterface.OrientationChanging_event")

  module internal ITitleViewInterface =
    // Properties
    let MeasuredTabLength: PropKey<int> =
      PropKey.Create.simple (536, "ITitleViewInterface.MeasuredTabLength")

    let TabDepth: PropKey<int> =
      PropKey.Create.simple (537, "ITitleViewInterface.TabDepth")

    let TabSide: PropKey<Terminal.Gui.ViewBase.Side> =
      PropKey.Create.simple (538, "ITitleViewInterface.TabSide")

  module internal IValueInterface =
    // Properties
    let Value<'TValue> : PropKey<'TValue> =
      PropKey.Create.simple (539, "IValueInterface.Value")

    // Events
    let ValueChangedUntyped: PropKey<ValueChangedEventArgs<System.Object> -> unit> =
      PropKey.Create.event (540, "IValueInterface.ValueChangedUntyped_event")

    let ValueChanged<'TValue> : PropKey<ValueChangedEventArgs<'TValue> -> unit> =
      PropKey.Create.event (541, "IValueInterface.ValueChanged_event")

    let ValueChanging<'TValue> : PropKey<ValueChangingEventArgs<'TValue> -> unit> =
      PropKey.Create.event (542, "IValueInterface.ValueChanging_event")


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
