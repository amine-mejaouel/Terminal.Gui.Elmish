namespace Terminal.Gui.Elmish

open System
open System.Collections.Generic
open Terminal.Gui.App
open Terminal.Gui.ViewBase
open Terminal.Gui.Views


type internal ViewPropHandler =
  static member applyNativeProps(terminalElement: ViewBackedTerminalElement, props: Props) =

    let view = terminalElement.View

    // Properties
    props |> Props.tryFind PKey.View.App |> Option.iter (fun v -> view.App <- v)

    props
    |> Props.tryFind PKey.View.Arrangement
    |> Option.iter (fun v -> view.Arrangement <- v)

    props
    |> Props.tryFind PKey.View.AssignHotKeys
    |> Option.iter (fun v -> view.AssignHotKeys <- v)

    props
    |> Props.tryFind PKey.View.BorderStyle
    |> Option.iter (fun v -> view.BorderStyle <- v)

    props
    |> Props.tryFind PKey.View.CanFocus
    |> Option.iter (fun v -> view.CanFocus <- v)

    props
    |> Props.tryFind PKey.View.CommandsToBubbleUp
    |> Option.iter (fun v -> view.CommandsToBubbleUp <- v)

    props
    |> Props.tryFind PKey.View.ContentSizeTracksViewport
    |> Option.iter (fun v -> view.ContentSizeTracksViewport <- v)

    props
    |> Props.tryFind PKey.View.Cursor
    |> Option.iter (fun v -> view.Cursor <- v)

    props |> Props.tryFind PKey.View.Data |> Option.iter (fun v -> view.Data <- v)

    props
    |> Props.tryFind PKey.View.DefaultAcceptView
    |> Option.iter (fun v -> view.DefaultAcceptView <- v)

    props
    |> Props.tryFind PKey.View.Enabled
    |> Option.iter (fun v -> view.Enabled <- v)

    props |> Props.tryFind PKey.View.Frame |> Option.iter (fun v -> view.Frame <- v)

    props
    |> Props.tryFind PKey.View.HasFocus
    |> Option.iter (fun v -> view.HasFocus <- v)

    props
    |> Props.tryFind PKey.View.Height
    |> Option.iter (fun v -> view.Height <- v)

    props
    |> Props.tryFind PKey.View.HotKey
    |> Option.iter (fun v -> view.HotKey <- v)

    props
    |> Props.tryFind PKey.View.HotKeySpecifier
    |> Option.iter (fun v -> view.HotKeySpecifier <- v)

    props |> Props.tryFind PKey.View.Id |> Option.iter (fun v -> view.Id <- v)

    props
    |> Props.tryFind PKey.View.IsInitialized
    |> Option.iter (fun v -> view.IsInitialized <- v)

    props
    |> Props.tryFind PKey.View.MouseHighlightStates
    |> Option.iter (fun v -> view.MouseHighlightStates <- v)

    props
    |> Props.tryFind PKey.View.MouseHoldRepeat
    |> Option.iter (fun v -> view.MouseHoldRepeat <- v)

    props
    |> Props.tryFind PKey.View.MousePositionTracking
    |> Option.iter (fun v -> view.MousePositionTracking <- v)

    props
    |> Props.tryFind PKey.View.PreserveTrailingSpaces
    |> Option.iter (fun v -> view.PreserveTrailingSpaces <- v)

    props
    |> Props.tryFind PKey.View.SchemeName
    |> Option.iter (fun v -> view.SchemeName <- v)

    props
    |> Props.tryFind PKey.View.ShadowStyle
    |> Option.iter (fun v -> view.ShadowStyle <- v)

    props
    |> Props.tryFind PKey.View.SuperViewRendersLineCanvas
    |> Option.iter (fun v -> view.SuperViewRendersLineCanvas <- v)

    props
    |> Props.tryFind PKey.View.TabStop
    |> Option.iter (fun v -> view.TabStop <- v)

    props |> Props.tryFind PKey.View.Text |> Option.iter (fun v -> view.Text <- v)

    props
    |> Props.tryFind PKey.View.TextAlignment
    |> Option.iter (fun v -> view.TextAlignment <- v)

    props
    |> Props.tryFind PKey.View.TextDirection
    |> Option.iter (fun v -> view.TextDirection <- v)

    props |> Props.tryFind PKey.View.Title |> Option.iter (fun v -> view.Title <- v)

    props
    |> Props.tryFind PKey.View.UsedHotKeys
    |> Option.iter (fun v -> view.UsedHotKeys <- v)

    props
    |> Props.tryFind PKey.View.ValidatePosDim
    |> Option.iter (fun v -> view.ValidatePosDim <- v)

    props
    |> Props.tryFind PKey.View.VerticalTextAlignment
    |> Option.iter (fun v -> view.VerticalTextAlignment <- v)

    props
    |> Props.tryFind PKey.View.Viewport
    |> Option.iter (fun v -> view.Viewport <- v)

    props
    |> Props.tryFind PKey.View.ViewportSettings
    |> Option.iter (fun v -> view.ViewportSettings <- v)

    props
    |> Props.tryFind PKey.View.Visible
    |> Option.iter (fun v -> view.Visible <- v)

    props |> Props.tryFind PKey.View.Width |> Option.iter (fun v -> view.Width <- v)

    // Events
    if props |> Props.exists PKey.View.Accepted then
      terminalElement.TrySetEventHandler(PKey.View.Accepted, view.Accepted)

    if props |> Props.exists PKey.View.Accepting then
      terminalElement.TrySetEventHandler(PKey.View.Accepting, view.Accepting)

    if props |> Props.exists PKey.View.Activated then
      terminalElement.TrySetEventHandler(PKey.View.Activated, view.Activated)

    if props |> Props.exists PKey.View.Activating then
      terminalElement.TrySetEventHandler(PKey.View.Activating, view.Activating)

    if props |> Props.exists PKey.View.AdvancingFocus then
      terminalElement.TrySetEventHandler(PKey.View.AdvancingFocus, view.AdvancingFocus)

    if props |> Props.exists PKey.View.BorderStyleChanged then
      terminalElement.TrySetEventHandler(PKey.View.BorderStyleChanged, view.BorderStyleChanged)

    if props |> Props.exists PKey.View.CanFocusChanged then
      terminalElement.TrySetEventHandler(PKey.View.CanFocusChanged, view.CanFocusChanged)

    if props |> Props.exists PKey.View.ClearedViewport then
      terminalElement.TrySetEventHandler(PKey.View.ClearedViewport, view.ClearedViewport)

    if props |> Props.exists PKey.View.ClearingViewport then
      terminalElement.TrySetEventHandler(PKey.View.ClearingViewport, view.ClearingViewport)

    if props |> Props.exists PKey.View.CommandNotBound then
      terminalElement.TrySetEventHandler(PKey.View.CommandNotBound, view.CommandNotBound)

    if props |> Props.exists PKey.View.ContentSizeChanged then
      terminalElement.TrySetEventHandler(PKey.View.ContentSizeChanged, view.ContentSizeChanged)

    if props |> Props.exists PKey.View.ContentSizeChanging then
      terminalElement.TrySetEventHandler(PKey.View.ContentSizeChanging, view.ContentSizeChanging)

    if props |> Props.exists PKey.View.Disposing then
      terminalElement.TrySetEventHandler(PKey.View.Disposing, view.Disposing)

    if props |> Props.exists PKey.View.DrawComplete then
      terminalElement.TrySetEventHandler(PKey.View.DrawComplete, view.DrawComplete)

    if props |> Props.exists PKey.View.DrawingContent then
      terminalElement.TrySetEventHandler(PKey.View.DrawingContent, view.DrawingContent)

    if props |> Props.exists PKey.View.DrawingSubViews then
      terminalElement.TrySetEventHandler(PKey.View.DrawingSubViews, view.DrawingSubViews)

    if props |> Props.exists PKey.View.DrawingText then
      terminalElement.TrySetEventHandler(PKey.View.DrawingText, view.DrawingText)

    if props |> Props.exists PKey.View.DrewText then
      terminalElement.TrySetEventHandler(PKey.View.DrewText, view.DrewText)

    if props |> Props.exists PKey.View.EnabledChanged then
      terminalElement.TrySetEventHandler(PKey.View.EnabledChanged, view.EnabledChanged)

    if props |> Props.exists PKey.View.FocusedChanged then
      terminalElement.TrySetEventHandler(PKey.View.FocusedChanged, view.FocusedChanged)

    if props |> Props.exists PKey.View.FrameChanged then
      terminalElement.TrySetEventHandler(PKey.View.FrameChanged, view.FrameChanged)

    if props |> Props.exists PKey.View.GettingAttributeForRole then
      terminalElement.TrySetEventHandler(PKey.View.GettingAttributeForRole, view.GettingAttributeForRole)

    if props |> Props.exists PKey.View.GettingScheme then
      terminalElement.TrySetEventHandler(PKey.View.GettingScheme, view.GettingScheme)

    if props |> Props.exists PKey.View.HandlingHotKey then
      terminalElement.TrySetEventHandler(PKey.View.HandlingHotKey, view.HandlingHotKey)

    if props |> Props.exists PKey.View.HasFocusChanged then
      terminalElement.TrySetEventHandler(PKey.View.HasFocusChanged, view.HasFocusChanged)

    if props |> Props.exists PKey.View.HasFocusChanging then
      terminalElement.TrySetEventHandler(PKey.View.HasFocusChanging, view.HasFocusChanging)

    if props |> Props.exists PKey.View.HeightChanged then
      terminalElement.TrySetEventHandler(PKey.View.HeightChanged, view.HeightChanged)

    if props |> Props.exists PKey.View.HeightChanging then
      terminalElement.TrySetEventHandler(PKey.View.HeightChanging, view.HeightChanging)

    if props |> Props.exists PKey.View.HotKeyChanged then
      terminalElement.TrySetEventHandler(PKey.View.HotKeyChanged, view.HotKeyChanged)

    if props |> Props.exists PKey.View.HotKeyCommand then
      terminalElement.TrySetEventHandler(PKey.View.HotKeyCommand, view.HotKeyCommand)

    if props |> Props.exists PKey.View.Initialized then
      terminalElement.TrySetEventHandler(PKey.View.Initialized, view.Initialized)

    if props |> Props.exists PKey.View.KeyDown then
      terminalElement.TrySetEventHandler(PKey.View.KeyDown, view.KeyDown)

    if props |> Props.exists PKey.View.KeyDownNotHandled then
      terminalElement.TrySetEventHandler(PKey.View.KeyDownNotHandled, view.KeyDownNotHandled)

    if props |> Props.exists PKey.View.KeyUp then
      terminalElement.TrySetEventHandler(PKey.View.KeyUp, view.KeyUp)

    if props |> Props.exists PKey.View.MouseEnter then
      terminalElement.TrySetEventHandler(PKey.View.MouseEnter, view.MouseEnter)

    if props |> Props.exists PKey.View.MouseEvent then
      terminalElement.TrySetEventHandler(PKey.View.MouseEvent, view.MouseEvent)

    if props |> Props.exists PKey.View.MouseHoldRepeatChanged then
      terminalElement.TrySetEventHandler(PKey.View.MouseHoldRepeatChanged, view.MouseHoldRepeatChanged)

    if props |> Props.exists PKey.View.MouseHoldRepeatChanging then
      terminalElement.TrySetEventHandler(PKey.View.MouseHoldRepeatChanging, view.MouseHoldRepeatChanging)

    if props |> Props.exists PKey.View.MouseLeave then
      terminalElement.TrySetEventHandler(PKey.View.MouseLeave, view.MouseLeave)

    if props |> Props.exists PKey.View.MouseStateChanged then
      terminalElement.TrySetEventHandler(PKey.View.MouseStateChanged, view.MouseStateChanged)

    if props |> Props.exists PKey.View.Pasted then
      terminalElement.TrySetEventHandler(PKey.View.Pasted, view.Pasted)

    if props |> Props.exists PKey.View.Pasting then
      terminalElement.TrySetEventHandler(PKey.View.Pasting, view.Pasting)

    if props |> Props.exists PKey.View.Removed then
      terminalElement.TrySetEventHandler(PKey.View.Removed, view.Removed)

    if props |> Props.exists PKey.View.SchemeChanged then
      terminalElement.TrySetEventHandler(PKey.View.SchemeChanged, view.SchemeChanged)

    if props |> Props.exists PKey.View.SchemeChanging then
      terminalElement.TrySetEventHandler(PKey.View.SchemeChanging, view.SchemeChanging)

    if props |> Props.exists PKey.View.SchemeNameChanged then
      terminalElement.TrySetEventHandler(PKey.View.SchemeNameChanged, view.SchemeNameChanged)

    if props |> Props.exists PKey.View.SchemeNameChanging then
      terminalElement.TrySetEventHandler(PKey.View.SchemeNameChanging, view.SchemeNameChanging)

    if props |> Props.exists PKey.View.ShadowStyleChanged then
      terminalElement.TrySetEventHandler(PKey.View.ShadowStyleChanged, view.ShadowStyleChanged)

    if props |> Props.exists PKey.View.SubViewAdded then
      terminalElement.TrySetEventHandler(PKey.View.SubViewAdded, view.SubViewAdded)

    if props |> Props.exists PKey.View.SubViewAdding then
      terminalElement.TrySetEventHandler(PKey.View.SubViewAdding, view.SubViewAdding)

    if props |> Props.exists PKey.View.SubViewLayout then
      terminalElement.TrySetEventHandler(PKey.View.SubViewLayout, view.SubViewLayout)

    if props |> Props.exists PKey.View.SubViewRemoved then
      terminalElement.TrySetEventHandler(PKey.View.SubViewRemoved, view.SubViewRemoved)

    if props |> Props.exists PKey.View.SubViewsLaidOut then
      terminalElement.TrySetEventHandler(PKey.View.SubViewsLaidOut, view.SubViewsLaidOut)

    if props |> Props.exists PKey.View.SuperViewChanged then
      terminalElement.TrySetEventHandler(PKey.View.SuperViewChanged, view.SuperViewChanged)

    if props |> Props.exists PKey.View.SuperViewChanging then
      terminalElement.TrySetEventHandler(PKey.View.SuperViewChanging, view.SuperViewChanging)

    if props |> Props.exists PKey.View.TextChanged then
      terminalElement.TrySetEventHandler(PKey.View.TextChanged, view.TextChanged)

    if props |> Props.exists PKey.View.TitleChanged then
      terminalElement.TrySetEventHandler(PKey.View.TitleChanged, view.TitleChanged)

    if props |> Props.exists PKey.View.TitleChanging then
      terminalElement.TrySetEventHandler(PKey.View.TitleChanging, view.TitleChanging)

    if props |> Props.exists PKey.View.ViewportChanged then
      terminalElement.TrySetEventHandler(PKey.View.ViewportChanged, view.ViewportChanged)

    if props |> Props.exists PKey.View.VisibleChanged then
      terminalElement.TrySetEventHandler(PKey.View.VisibleChanged, view.VisibleChanged)

    if props |> Props.exists PKey.View.VisibleChanging then
      terminalElement.TrySetEventHandler(PKey.View.VisibleChanging, view.VisibleChanging)

    if props |> Props.exists PKey.View.WidthChanged then
      terminalElement.TrySetEventHandler(PKey.View.WidthChanged, view.WidthChanged)

    if props |> Props.exists PKey.View.WidthChanging then
      terminalElement.TrySetEventHandler(PKey.View.WidthChanging, view.WidthChanging)

  static member clearProp(terminalElement: ViewBackedTerminalElement, propertyId: PropertyId) =
    let view = terminalElement.View

    match propertyId.Value with
    | 0 -> view.App <- Unchecked.defaultof<_>
    | 1 -> view.Arrangement <- Unchecked.defaultof<_>
    | 2 -> view.AssignHotKeys <- Unchecked.defaultof<_>
    | 3 -> view.BorderStyle <- Unchecked.defaultof<_>
    | 4 -> view.CanFocus <- Unchecked.defaultof<_>
    | 5 -> view.CommandsToBubbleUp <- Unchecked.defaultof<_>
    | 6 -> view.ContentSizeTracksViewport <- Unchecked.defaultof<_>
    | 7 -> view.Cursor <- Unchecked.defaultof<_>
    | 8 -> view.Data <- Unchecked.defaultof<_>
    | 9 -> view.DefaultAcceptView <- Unchecked.defaultof<_>
    | 11 -> view.Enabled <- Unchecked.defaultof<_>
    | 12 -> view.Frame <- Unchecked.defaultof<_>
    | 13 -> view.HasFocus <- Unchecked.defaultof<_>
    | 14 -> view.Height <- Unchecked.defaultof<_>
    | 15 -> view.HotKey <- Terminal.Gui.Input.Key.Empty
    | 16 -> view.HotKeySpecifier <- Unchecked.defaultof<_>
    | 17 -> view.Id <- ""
    | 18 -> view.IsInitialized <- Unchecked.defaultof<_>
    | 19 -> view.MouseHighlightStates <- Unchecked.defaultof<_>
    | 20 -> view.MouseHoldRepeat <- Unchecked.defaultof<_>
    | 21 -> view.MousePositionTracking <- Unchecked.defaultof<_>
    | 22 -> view.PreserveTrailingSpaces <- Unchecked.defaultof<_>
    | 23 -> view.SchemeName <- ""
    | 24 -> view.ShadowStyle <- Unchecked.defaultof<_>
    | 25 -> view.SuperViewRendersLineCanvas <- Unchecked.defaultof<_>
    | 26 -> view.TabStop <- Unchecked.defaultof<_>
    | 27 -> view.Text <- ""
    | 28 -> view.TextAlignment <- Unchecked.defaultof<_>
    | 29 -> view.TextDirection <- Unchecked.defaultof<_>
    | 30 -> view.Title <- ""
    | 31 -> view.UsedHotKeys <- Unchecked.defaultof<_>
    | 32 -> view.ValidatePosDim <- Unchecked.defaultof<_>
    | 33 -> view.VerticalTextAlignment <- Unchecked.defaultof<_>
    | 34 -> view.Viewport <- Unchecked.defaultof<_>
    | 35 -> view.ViewportSettings <- Unchecked.defaultof<_>
    | 36 -> view.Visible <- Unchecked.defaultof<_>
    | 37 -> view.Width <- Unchecked.defaultof<_>
    | _ -> invalidOp $"Property ID {propertyId} cannot be cleared on '{terminalElement.Name}'."

type internal AdornmentViewPropHandler =
  static member applyNativeProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    ViewPropHandler.applyNativeProps (terminalElement, props)

    let view = terminalElement.View :?> AdornmentView

    // Properties
    props
    |> Props.tryFind PKey.AdornmentView.Adornment
    |> Option.iter (fun v -> view.Adornment <- v)

    props
    |> Props.tryFind PKey.AdornmentView.Diagnostics
    |> Option.iter (fun v -> view.Diagnostics <- v)

    props
    |> Props.tryFind PKey.AdornmentView.SuperViewRendersLineCanvas
    |> Option.iter (fun v -> view.SuperViewRendersLineCanvas <- v)

    props
    |> Props.tryFind PKey.AdornmentView.Viewport
    |> Option.iter (fun v -> view.Viewport <- v)

  static member clearProp(terminalElement: ViewBackedTerminalElement, propertyId: PropertyId) =
    let view = terminalElement.View :?> AdornmentView

    match propertyId.Value with
    | 101 -> view.Adornment <- Unchecked.defaultof<_>
    | 102 -> view.Diagnostics <- Unchecked.defaultof<_>
    | 103 -> view.SuperViewRendersLineCanvas <- Unchecked.defaultof<_>
    | 104 -> view.Viewport <- Unchecked.defaultof<_>
    | _ -> ViewPropHandler.clearProp (terminalElement, propertyId)

type internal AttributePickerPropHandler =
  static member applyNativeProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    ViewPropHandler.applyNativeProps (terminalElement, props)

    let view = terminalElement.View :?> AttributePicker

    // Properties
    props
    |> Props.tryFind PKey.AttributePicker.SampleText
    |> Option.iter (fun v -> view.SampleText <- v)

    props
    |> Props.tryFind PKey.AttributePicker.Value
    |> Option.iter (fun v -> view.Value <- v)

    // Events
    if props |> Props.exists PKey.AttributePicker.ValueChanged then
      terminalElement.TrySetEventHandler(PKey.AttributePicker.ValueChanged, view.ValueChanged)

    if props |> Props.exists PKey.AttributePicker.ValueChangedUntyped then
      terminalElement.TrySetEventHandler(PKey.AttributePicker.ValueChangedUntyped, view.ValueChangedUntyped)

    if props |> Props.exists PKey.AttributePicker.ValueChanging then
      terminalElement.TrySetEventHandler(PKey.AttributePicker.ValueChanging, view.ValueChanging)

  static member clearProp(terminalElement: ViewBackedTerminalElement, propertyId: PropertyId) =
    let view = terminalElement.View :?> AttributePicker

    match propertyId.Value with
    | 105 -> view.SampleText <- ""
    | 106 -> view.Value <- Unchecked.defaultof<_>
    | _ -> ViewPropHandler.clearProp (terminalElement, propertyId)

type internal BarPropHandler =
  static member applyNativeProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    ViewPropHandler.applyNativeProps (terminalElement, props)

    let view = terminalElement.View :?> Bar

    // Properties
    props
    |> Props.tryFind PKey.Bar.AlignmentModes
    |> Option.iter (fun v -> view.AlignmentModes <- v)

    props
    |> Props.tryFind PKey.Bar.Orientation
    |> Option.iter (fun v -> view.Orientation <- v)

    // Events
    if props |> Props.exists PKey.Bar.OrientationChanged then
      terminalElement.TrySetEventHandler(PKey.Bar.OrientationChanged, view.OrientationChanged)

    if props |> Props.exists PKey.Bar.OrientationChanging then
      terminalElement.TrySetEventHandler(PKey.Bar.OrientationChanging, view.OrientationChanging)

  static member clearProp(terminalElement: ViewBackedTerminalElement, propertyId: PropertyId) =
    let view = terminalElement.View :?> Bar

    match propertyId.Value with
    | 110 -> view.AlignmentModes <- Unchecked.defaultof<_>
    | 111 -> view.Orientation <- Unchecked.defaultof<_>
    | _ -> ViewPropHandler.clearProp (terminalElement, propertyId)

type internal BorderViewPropHandler =
  static member applyNativeProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    AdornmentViewPropHandler.applyNativeProps (terminalElement, props)

    let view = terminalElement.View :?> BorderView

    // Properties
    props
    |> Props.tryFind PKey.BorderView.TabLength
    |> Option.iter (fun v -> view.TabLength <- v)

    props
    |> Props.tryFind PKey.BorderView.TabOffset
    |> Option.iter (fun v -> view.TabOffset <- v)

    props
    |> Props.tryFind PKey.BorderView.TabSide
    |> Option.iter (fun v -> view.TabSide <- v)

  static member clearProp(terminalElement: ViewBackedTerminalElement, propertyId: PropertyId) =
    let view = terminalElement.View :?> BorderView

    match propertyId.Value with
    | 114 -> view.TabLength <- Unchecked.defaultof<_>
    | 115 -> view.TabOffset <- Unchecked.defaultof<_>
    | 116 -> view.TabSide <- Unchecked.defaultof<_>
    | _ -> AdornmentViewPropHandler.clearProp (terminalElement, propertyId)

type internal ButtonPropHandler =
  static member applyNativeProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    ViewPropHandler.applyNativeProps (terminalElement, props)

    let view = terminalElement.View :?> Button

    // Properties
    props
    |> Props.tryFind PKey.Button.HotKeySpecifier
    |> Option.iter (fun v -> view.HotKeySpecifier <- v)

    props
    |> Props.tryFind PKey.Button.IsDefault
    |> Option.iter (fun v -> view.IsDefault <- v)

    props
    |> Props.tryFind PKey.Button.NoDecorations
    |> Option.iter (fun v -> view.NoDecorations <- v)

    props
    |> Props.tryFind PKey.Button.NoPadding
    |> Option.iter (fun v -> view.NoPadding <- v)

    props |> Props.tryFind PKey.Button.Text |> Option.iter (fun v -> view.Text <- v)

    // Events
    if props |> Props.exists PKey.Button.InitializingShadowStyle then
      terminalElement.TrySetEventHandler(PKey.Button.InitializingShadowStyle, view.InitializingShadowStyle)

  static member clearProp(terminalElement: ViewBackedTerminalElement, propertyId: PropertyId) =
    let view = terminalElement.View :?> Button

    match propertyId.Value with
    | 117 -> view.HotKeySpecifier <- Unchecked.defaultof<_>
    | 118 -> view.IsDefault <- Unchecked.defaultof<_>
    | 119 -> view.NoDecorations <- Unchecked.defaultof<_>
    | 120 -> view.NoPadding <- Unchecked.defaultof<_>
    | 121 -> view.Text <- ""
    | _ -> ViewPropHandler.clearProp (terminalElement, propertyId)

type internal CharMapPropHandler =
  static member applyNativeProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    ViewPropHandler.applyNativeProps (terminalElement, props)

    let view = terminalElement.View :?> CharMap

    // Properties
    props
    |> Props.tryFind PKey.CharMap.SelectedCodePoint
    |> Option.iter (fun v -> view.SelectedCodePoint <- v)

    props
    |> Props.tryFind PKey.CharMap.ShowGlyphWidths
    |> Option.iter (fun v -> view.ShowGlyphWidths <- v)

    props
    |> Props.tryFind PKey.CharMap.ShowUnicodeCategory
    |> Option.iter (fun v -> view.ShowUnicodeCategory <- v)

    props
    |> Props.tryFind PKey.CharMap.StartCodePoint
    |> Option.iter (fun v -> view.StartCodePoint <- v)

    props
    |> Props.tryFind PKey.CharMap.Value
    |> Option.iter (fun v -> view.Value <- v)

    // Events
    if props |> Props.exists PKey.CharMap.ValueChanged then
      terminalElement.TrySetEventHandler(PKey.CharMap.ValueChanged, view.ValueChanged)

    if props |> Props.exists PKey.CharMap.ValueChangedUntyped then
      terminalElement.TrySetEventHandler(PKey.CharMap.ValueChangedUntyped, view.ValueChangedUntyped)

    if props |> Props.exists PKey.CharMap.ValueChanging then
      terminalElement.TrySetEventHandler(PKey.CharMap.ValueChanging, view.ValueChanging)

  static member clearProp(terminalElement: ViewBackedTerminalElement, propertyId: PropertyId) =
    let view = terminalElement.View :?> CharMap

    match propertyId.Value with
    | 123 -> view.SelectedCodePoint <- Unchecked.defaultof<_>
    | 124 -> view.ShowGlyphWidths <- Unchecked.defaultof<_>
    | 125 -> view.ShowUnicodeCategory <- Unchecked.defaultof<_>
    | 126 -> view.StartCodePoint <- Unchecked.defaultof<_>
    | 127 -> view.Value <- Unchecked.defaultof<_>
    | _ -> ViewPropHandler.clearProp (terminalElement, propertyId)

type internal CheckBoxPropHandler =
  static member applyNativeProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    ViewPropHandler.applyNativeProps (terminalElement, props)

    let view = terminalElement.View :?> CheckBox

    // Properties
    props
    |> Props.tryFind PKey.CheckBox.AllowCheckStateNone
    |> Option.iter (fun v -> view.AllowCheckStateNone <- v)

    props
    |> Props.tryFind PKey.CheckBox.HotKeySpecifier
    |> Option.iter (fun v -> view.HotKeySpecifier <- v)

    props
    |> Props.tryFind PKey.CheckBox.RadioStyle
    |> Option.iter (fun v -> view.RadioStyle <- v)

    props
    |> Props.tryFind PKey.CheckBox.Text
    |> Option.iter (fun v -> view.Text <- v)

    props
    |> Props.tryFind PKey.CheckBox.Value
    |> Option.iter (fun v -> view.Value <- v)

    // Events
    if props |> Props.exists PKey.CheckBox.ValueChanged then
      terminalElement.TrySetEventHandler(PKey.CheckBox.ValueChanged, view.ValueChanged)

    if props |> Props.exists PKey.CheckBox.ValueChangedUntyped then
      terminalElement.TrySetEventHandler(PKey.CheckBox.ValueChangedUntyped, view.ValueChangedUntyped)

    if props |> Props.exists PKey.CheckBox.ValueChanging then
      terminalElement.TrySetEventHandler(PKey.CheckBox.ValueChanging, view.ValueChanging)

  static member clearProp(terminalElement: ViewBackedTerminalElement, propertyId: PropertyId) =
    let view = terminalElement.View :?> CheckBox

    match propertyId.Value with
    | 131 -> view.AllowCheckStateNone <- Unchecked.defaultof<_>
    | 132 -> view.HotKeySpecifier <- Unchecked.defaultof<_>
    | 133 -> view.RadioStyle <- Unchecked.defaultof<_>
    | 134 -> view.Text <- ""
    | 135 -> view.Value <- Unchecked.defaultof<_>
    | _ -> ViewPropHandler.clearProp (terminalElement, propertyId)

type internal CodePropHandler =
  static member applyNativeProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    ViewPropHandler.applyNativeProps (terminalElement, props)

    let view = terminalElement.View :?> Code

    // Properties
    props
    |> Props.tryFind PKey.Code.Language
    |> Option.iter (fun v -> view.Language <- v)

    props
    |> Props.tryFind PKey.Code.SyntaxHighlighter
    |> Option.iter (fun v -> view.SyntaxHighlighter <- v)

    props |> Props.tryFind PKey.Code.Text |> Option.iter (fun v -> view.Text <- v)

  static member clearProp(terminalElement: ViewBackedTerminalElement, propertyId: PropertyId) =
    let view = terminalElement.View :?> Code

    match propertyId.Value with
    | 139 -> view.Language <- ""
    | 140 -> view.SyntaxHighlighter <- Unchecked.defaultof<_>
    | 141 -> view.Text <- ""
    | _ -> ViewPropHandler.clearProp (terminalElement, propertyId)

type internal ColorPickerPropHandler =
  static member applyNativeProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    ViewPropHandler.applyNativeProps (terminalElement, props)

    let view = terminalElement.View :?> ColorPicker

    // Properties
    props
    |> Props.tryFind PKey.ColorPicker.SelectedColor
    |> Option.iter (fun v -> view.SelectedColor <- v)

    props
    |> Props.tryFind PKey.ColorPicker.Style
    |> Option.iter (fun v -> view.Style <- v)

    props
    |> Props.tryFind PKey.ColorPicker.Text
    |> Option.iter (fun v -> view.Text <- v)

    props
    |> Props.tryFind PKey.ColorPicker.Value
    |> Option.iter (fun v -> view.Value <- v)

    // Events
    if props |> Props.exists PKey.ColorPicker.ValueChanged then
      terminalElement.TrySetEventHandler(PKey.ColorPicker.ValueChanged, view.ValueChanged)

    if props |> Props.exists PKey.ColorPicker.ValueChangedUntyped then
      terminalElement.TrySetEventHandler(PKey.ColorPicker.ValueChangedUntyped, view.ValueChangedUntyped)

    if props |> Props.exists PKey.ColorPicker.ValueChanging then
      terminalElement.TrySetEventHandler(PKey.ColorPicker.ValueChanging, view.ValueChanging)

  static member clearProp(terminalElement: ViewBackedTerminalElement, propertyId: PropertyId) =
    let view = terminalElement.View :?> ColorPicker

    match propertyId.Value with
    | 142 -> view.SelectedColor <- Unchecked.defaultof<_>
    | 143 -> view.Style <- Unchecked.defaultof<_>
    | 144 -> view.Text <- ""
    | 145 -> view.Value <- Unchecked.defaultof<_>
    | _ -> ViewPropHandler.clearProp (terminalElement, propertyId)

type internal ColorPicker16PropHandler =
  static member applyNativeProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    ViewPropHandler.applyNativeProps (terminalElement, props)

    let view = terminalElement.View :?> ColorPicker16

    // Properties
    props
    |> Props.tryFind PKey.ColorPicker16.BoxHeight
    |> Option.iter (fun v -> view.BoxHeight <- v)

    props
    |> Props.tryFind PKey.ColorPicker16.BoxWidth
    |> Option.iter (fun v -> view.BoxWidth <- v)

    props
    |> Props.tryFind PKey.ColorPicker16.Caret
    |> Option.iter (fun v -> view.Caret <- v)

    props
    |> Props.tryFind PKey.ColorPicker16.SelectedColor
    |> Option.iter (fun v -> view.SelectedColor <- v)

    props
    |> Props.tryFind PKey.ColorPicker16.Value
    |> Option.iter (fun v -> view.Value <- v)

    // Events
    if props |> Props.exists PKey.ColorPicker16.ValueChanged then
      terminalElement.TrySetEventHandler(PKey.ColorPicker16.ValueChanged, view.ValueChanged)

    if props |> Props.exists PKey.ColorPicker16.ValueChangedUntyped then
      terminalElement.TrySetEventHandler(PKey.ColorPicker16.ValueChangedUntyped, view.ValueChangedUntyped)

    if props |> Props.exists PKey.ColorPicker16.ValueChanging then
      terminalElement.TrySetEventHandler(PKey.ColorPicker16.ValueChanging, view.ValueChanging)

  static member clearProp(terminalElement: ViewBackedTerminalElement, propertyId: PropertyId) =
    let view = terminalElement.View :?> ColorPicker16

    match propertyId.Value with
    | 149 -> view.BoxHeight <- Unchecked.defaultof<_>
    | 150 -> view.BoxWidth <- Unchecked.defaultof<_>
    | 151 -> view.Caret <- Unchecked.defaultof<_>
    | 152 -> view.SelectedColor <- Unchecked.defaultof<_>
    | 153 -> view.Value <- Unchecked.defaultof<_>
    | _ -> ViewPropHandler.clearProp (terminalElement, propertyId)

type internal DatePickerPropHandler =
  static member applyNativeProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    ViewPropHandler.applyNativeProps (terminalElement, props)

    let view = terminalElement.View :?> DatePicker

    // Properties
    props
    |> Props.tryFind PKey.DatePicker.Culture
    |> Option.iter (fun v -> view.Culture <- v)

    props
    |> Props.tryFind PKey.DatePicker.Text
    |> Option.iter (fun v -> view.Text <- v)

    props
    |> Props.tryFind PKey.DatePicker.Value
    |> Option.iter (fun v -> view.Value <- v)

    // Events
    if props |> Props.exists PKey.DatePicker.ValueChanged then
      terminalElement.TrySetEventHandler(PKey.DatePicker.ValueChanged, view.ValueChanged)

    if props |> Props.exists PKey.DatePicker.ValueChangedUntyped then
      terminalElement.TrySetEventHandler(PKey.DatePicker.ValueChangedUntyped, view.ValueChangedUntyped)

    if props |> Props.exists PKey.DatePicker.ValueChanging then
      terminalElement.TrySetEventHandler(PKey.DatePicker.ValueChanging, view.ValueChanging)

  static member clearProp(terminalElement: ViewBackedTerminalElement, propertyId: PropertyId) =
    let view = terminalElement.View :?> DatePicker

    match propertyId.Value with
    | 157 -> view.Culture <- Unchecked.defaultof<_>
    | 158 -> view.Text <- ""
    | 159 -> view.Value <- Unchecked.defaultof<_>
    | _ -> ViewPropHandler.clearProp (terminalElement, propertyId)

type internal FrameViewPropHandler =
  static member applyNativeProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    ViewPropHandler.applyNativeProps (terminalElement, props)

  static member clearProp(terminalElement: ViewBackedTerminalElement, propertyId: PropertyId) =
    match propertyId.Value with
    | _ -> ViewPropHandler.clearProp (terminalElement, propertyId)

type internal GraphViewPropHandler =
  static member applyNativeProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    ViewPropHandler.applyNativeProps (terminalElement, props)

    let view = terminalElement.View :?> GraphView

    // Properties
    props
    |> Props.tryFind PKey.GraphView.AxisX
    |> Option.iter (fun v -> view.AxisX <- v)

    props
    |> Props.tryFind PKey.GraphView.AxisY
    |> Option.iter (fun v -> view.AxisY <- v)

    props
    |> Props.tryFind PKey.GraphView.CellSize
    |> Option.iter (fun v -> view.CellSize <- v)

    props
    |> Props.tryFind PKey.GraphView.GraphColor
    |> Option.iter (fun v -> view.GraphColor <- v)

    props
    |> Props.tryFind PKey.GraphView.MarginBottom
    |> Option.iter (fun v -> view.MarginBottom <- v)

    props
    |> Props.tryFind PKey.GraphView.MarginLeft
    |> Option.iter (fun v -> view.MarginLeft <- v)

    props
    |> Props.tryFind PKey.GraphView.ScrollOffset
    |> Option.iter (fun v -> view.ScrollOffset <- v)

  static member clearProp(terminalElement: ViewBackedTerminalElement, propertyId: PropertyId) =
    let view = terminalElement.View :?> GraphView

    match propertyId.Value with
    | 163 -> view.AxisX <- Unchecked.defaultof<_>
    | 164 -> view.AxisY <- Unchecked.defaultof<_>
    | 165 -> view.CellSize <- Unchecked.defaultof<_>
    | 166 -> view.GraphColor <- Unchecked.defaultof<_>
    | 167 -> view.MarginBottom <- Unchecked.defaultof<_>
    | 168 -> view.MarginLeft <- Unchecked.defaultof<_>
    | 169 -> view.ScrollOffset <- Unchecked.defaultof<_>
    | _ -> ViewPropHandler.clearProp (terminalElement, propertyId)

type internal HexViewPropHandler =
  static member applyNativeProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    ViewPropHandler.applyNativeProps (terminalElement, props)

    let view = terminalElement.View :?> HexView

    // Properties
    props
    |> Props.tryFind PKey.HexView.Address
    |> Option.iter (fun v -> view.Address <- v)

    props
    |> Props.tryFind PKey.HexView.AddressWidth
    |> Option.iter (fun v -> view.AddressWidth <- v)

    props
    |> Props.tryFind PKey.HexView.BytesPerLine
    |> Option.iter (fun v -> view.BytesPerLine <- v)

    props
    |> Props.tryFind PKey.HexView.ReadOnly
    |> Option.iter (fun v -> view.ReadOnly <- v)

    props
    |> Props.tryFind PKey.HexView.Source
    |> Option.iter (fun v -> view.Source <- v)

    // Events
    if props |> Props.exists PKey.HexView.Edited then
      terminalElement.TrySetEventHandler(PKey.HexView.Edited, view.Edited)

    if props |> Props.exists PKey.HexView.PositionChanged then
      terminalElement.TrySetEventHandler(PKey.HexView.PositionChanged, view.PositionChanged)

  static member clearProp(terminalElement: ViewBackedTerminalElement, propertyId: PropertyId) =
    let view = terminalElement.View :?> HexView

    match propertyId.Value with
    | 170 -> view.Address <- Unchecked.defaultof<_>
    | 171 -> view.AddressWidth <- Unchecked.defaultof<_>
    | 172 -> view.BytesPerLine <- Unchecked.defaultof<_>
    | 173 -> view.ReadOnly <- Unchecked.defaultof<_>
    | 174 -> view.Source <- Unchecked.defaultof<_>
    | _ -> ViewPropHandler.clearProp (terminalElement, propertyId)

type internal ImageViewPropHandler =
  static member applyNativeProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    ViewPropHandler.applyNativeProps (terminalElement, props)

    let view = terminalElement.View :?> ImageView

    // Properties
    props
    |> Props.tryFind PKey.ImageView.AllowSixelUpscaling
    |> Option.iter (fun v -> view.AllowSixelUpscaling <- v)

    props
    |> Props.tryFind PKey.ImageView.Image
    |> Option.iter (fun v -> view.Image <- v)

    props
    |> Props.tryFind PKey.ImageView.MaxSixelPaletteColors
    |> Option.iter (fun v -> view.MaxSixelPaletteColors <- v)

    props
    |> Props.tryFind PKey.ImageView.SixelEncoder
    |> Option.iter (fun v -> view.SixelEncoder <- v)

    props
    |> Props.tryFind PKey.ImageView.UseBackgroundRendering
    |> Option.iter (fun v -> view.UseBackgroundRendering <- v)

    props
    |> Props.tryFind PKey.ImageView.UseRasterGraphics
    |> Option.iter (fun v -> view.UseRasterGraphics <- v)

    props
    |> Props.tryFind PKey.ImageView.UseSixel
    |> Option.iter (fun v -> view.UseSixel <- v)

    props
    |> Props.tryFind PKey.ImageView.ZoomLevel
    |> Option.iter (fun v -> view.ZoomLevel <- v)

    // Events
    if props |> Props.exists PKey.ImageView.ZoomLevelChanged then
      terminalElement.TrySetEventHandler(PKey.ImageView.ZoomLevelChanged, view.ZoomLevelChanged)

  static member clearProp(terminalElement: ViewBackedTerminalElement, propertyId: PropertyId) =
    let view = terminalElement.View :?> ImageView

    match propertyId.Value with
    | 177 -> view.AllowSixelUpscaling <- Unchecked.defaultof<_>
    | 178 -> view.Image <- Unchecked.defaultof<_>
    | 179 -> view.MaxSixelPaletteColors <- Unchecked.defaultof<_>
    | 180 -> view.SixelEncoder <- Unchecked.defaultof<_>
    | 181 -> view.UseBackgroundRendering <- Unchecked.defaultof<_>
    | 182 -> view.UseRasterGraphics <- Unchecked.defaultof<_>
    | 183 -> view.UseSixel <- Unchecked.defaultof<_>
    | 184 -> view.ZoomLevel <- Unchecked.defaultof<_>
    | _ -> ViewPropHandler.clearProp (terminalElement, propertyId)

type internal LabelPropHandler =
  static member applyNativeProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    ViewPropHandler.applyNativeProps (terminalElement, props)

    let view = terminalElement.View :?> Label

    // Properties
    props
    |> Props.tryFind PKey.Label.HotKeySpecifier
    |> Option.iter (fun v -> view.HotKeySpecifier <- v)

    props |> Props.tryFind PKey.Label.Text |> Option.iter (fun v -> view.Text <- v)

  static member clearProp(terminalElement: ViewBackedTerminalElement, propertyId: PropertyId) =
    let view = terminalElement.View :?> Label

    match propertyId.Value with
    | 186 -> view.HotKeySpecifier <- Unchecked.defaultof<_>
    | 187 -> view.Text <- ""
    | _ -> ViewPropHandler.clearProp (terminalElement, propertyId)

type internal LegendAnnotationPropHandler =
  static member applyNativeProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    ViewPropHandler.applyNativeProps (terminalElement, props)

  static member clearProp(terminalElement: ViewBackedTerminalElement, propertyId: PropertyId) =
    match propertyId.Value with
    | _ -> ViewPropHandler.clearProp (terminalElement, propertyId)

type internal LinePropHandler =
  static member applyNativeProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    ViewPropHandler.applyNativeProps (terminalElement, props)

    let view = terminalElement.View :?> Line

    // Properties
    props
    |> Props.tryFind PKey.Line.Length
    |> Option.iter (fun v -> view.Length <- v)

    props
    |> Props.tryFind PKey.Line.LineAttribute
    |> Option.iter (fun v -> view.LineAttribute <- v)

    props
    |> Props.tryFind PKey.Line.Orientation
    |> Option.iter (fun v -> view.Orientation <- v)

    props |> Props.tryFind PKey.Line.Style |> Option.iter (fun v -> view.Style <- v)

    // Events
    if props |> Props.exists PKey.Line.OrientationChanged then
      terminalElement.TrySetEventHandler(PKey.Line.OrientationChanged, view.OrientationChanged)

    if props |> Props.exists PKey.Line.OrientationChanging then
      terminalElement.TrySetEventHandler(PKey.Line.OrientationChanging, view.OrientationChanging)

  static member clearProp(terminalElement: ViewBackedTerminalElement, propertyId: PropertyId) =
    let view = terminalElement.View :?> Line

    match propertyId.Value with
    | 188 -> view.Length <- Unchecked.defaultof<_>
    | 189 -> view.LineAttribute <- Unchecked.defaultof<_>
    | 190 -> view.Orientation <- Unchecked.defaultof<_>
    | 191 -> view.Style <- Unchecked.defaultof<_>
    | _ -> ViewPropHandler.clearProp (terminalElement, propertyId)

type internal LinearRangeViewBasePropHandler<'TOption, 'TValue> =
  static member applyNativeProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    ViewPropHandler.applyNativeProps (terminalElement, props)

    let view = terminalElement.View :?> LinearRangeViewBase<'TOption, 'TValue>

    // Properties
    props
    |> Props.tryFind PKey.LinearRangeViewBase<'TOption, 'TValue>.AllowEmpty
    |> Option.iter (fun v -> view.AllowEmpty <- v)

    props
    |> Props.tryFind PKey.LinearRangeViewBase<'TOption, 'TValue>.FocusedOption
    |> Option.iter (fun v -> view.FocusedOption <- v)

    props
    |> Props.tryFind PKey.LinearRangeViewBase<'TOption, 'TValue>.LegendsOrientation
    |> Option.iter (fun v -> view.LegendsOrientation <- v)

    props
    |> Props.tryFind PKey.LinearRangeViewBase<'TOption, 'TValue>.MinimumInnerSpacing
    |> Option.iter (fun v -> view.MinimumInnerSpacing <- v)

    props
    |> Props.tryFind PKey.LinearRangeViewBase<'TOption, 'TValue>.Options
    |> Option.iter (fun v -> view.Options <- v)

    props
    |> Props.tryFind PKey.LinearRangeViewBase<'TOption, 'TValue>.Orientation
    |> Option.iter (fun v -> view.Orientation <- v)

    props
    |> Props.tryFind PKey.LinearRangeViewBase<'TOption, 'TValue>.ShowEndSpacing
    |> Option.iter (fun v -> view.ShowEndSpacing <- v)

    props
    |> Props.tryFind PKey.LinearRangeViewBase<'TOption, 'TValue>.ShowLegends
    |> Option.iter (fun v -> view.ShowLegends <- v)

    props
    |> Props.tryFind PKey.LinearRangeViewBase<'TOption, 'TValue>.Style
    |> Option.iter (fun v -> view.Style <- v)

    props
    |> Props.tryFind PKey.LinearRangeViewBase<'TOption, 'TValue>.Text
    |> Option.iter (fun v -> view.Text <- v)

    props
    |> Props.tryFind PKey.LinearRangeViewBase<'TOption, 'TValue>.UseMinimumSize
    |> Option.iter (fun v -> view.UseMinimumSize <- v)

    props
    |> Props.tryFind PKey.LinearRangeViewBase<'TOption, 'TValue>.Value
    |> Option.iter (fun v -> view.Value <- v)

    // Events
    if
      props
      |> Props.exists PKey.LinearRangeViewBase<'TOption, 'TValue>.LegendsOrientationChanged
    then
      terminalElement.TrySetEventHandler(
        PKey.LinearRangeViewBase<'TOption, 'TValue>.LegendsOrientationChanged,
        view.LegendsOrientationChanged
      )

    if
      props
      |> Props.exists PKey.LinearRangeViewBase<'TOption, 'TValue>.LegendsOrientationChanging
    then
      terminalElement.TrySetEventHandler(
        PKey.LinearRangeViewBase<'TOption, 'TValue>.LegendsOrientationChanging,
        view.LegendsOrientationChanging
      )

    if
      props
      |> Props.exists PKey.LinearRangeViewBase<'TOption, 'TValue>.MinimumInnerSpacingChanged
    then
      terminalElement.TrySetEventHandler(
        PKey.LinearRangeViewBase<'TOption, 'TValue>.MinimumInnerSpacingChanged,
        view.MinimumInnerSpacingChanged
      )

    if
      props
      |> Props.exists PKey.LinearRangeViewBase<'TOption, 'TValue>.MinimumInnerSpacingChanging
    then
      terminalElement.TrySetEventHandler(
        PKey.LinearRangeViewBase<'TOption, 'TValue>.MinimumInnerSpacingChanging,
        view.MinimumInnerSpacingChanging
      )

    if props |> Props.exists PKey.LinearRangeViewBase<'TOption, 'TValue>.OptionFocused then
      terminalElement.TrySetEventHandler(PKey.LinearRangeViewBase<'TOption, 'TValue>.OptionFocused, view.OptionFocused)

    if
      props
      |> Props.exists PKey.LinearRangeViewBase<'TOption, 'TValue>.OrientationChanged
    then
      terminalElement.TrySetEventHandler(
        PKey.LinearRangeViewBase<'TOption, 'TValue>.OrientationChanged,
        view.OrientationChanged
      )

    if
      props
      |> Props.exists PKey.LinearRangeViewBase<'TOption, 'TValue>.OrientationChanging
    then
      terminalElement.TrySetEventHandler(
        PKey.LinearRangeViewBase<'TOption, 'TValue>.OrientationChanging,
        view.OrientationChanging
      )

    if
      props
      |> Props.exists PKey.LinearRangeViewBase<'TOption, 'TValue>.ShowEndSpacingChanged
    then
      terminalElement.TrySetEventHandler(
        PKey.LinearRangeViewBase<'TOption, 'TValue>.ShowEndSpacingChanged,
        view.ShowEndSpacingChanged
      )

    if
      props
      |> Props.exists PKey.LinearRangeViewBase<'TOption, 'TValue>.ShowEndSpacingChanging
    then
      terminalElement.TrySetEventHandler(
        PKey.LinearRangeViewBase<'TOption, 'TValue>.ShowEndSpacingChanging,
        view.ShowEndSpacingChanging
      )

    if
      props
      |> Props.exists PKey.LinearRangeViewBase<'TOption, 'TValue>.ShowLegendsChanged
    then
      terminalElement.TrySetEventHandler(
        PKey.LinearRangeViewBase<'TOption, 'TValue>.ShowLegendsChanged,
        view.ShowLegendsChanged
      )

    if
      props
      |> Props.exists PKey.LinearRangeViewBase<'TOption, 'TValue>.ShowLegendsChanging
    then
      terminalElement.TrySetEventHandler(
        PKey.LinearRangeViewBase<'TOption, 'TValue>.ShowLegendsChanging,
        view.ShowLegendsChanging
      )

    if
      props
      |> Props.exists PKey.LinearRangeViewBase<'TOption, 'TValue>.UseMinimumSizeChanged
    then
      terminalElement.TrySetEventHandler(
        PKey.LinearRangeViewBase<'TOption, 'TValue>.UseMinimumSizeChanged,
        view.UseMinimumSizeChanged
      )

    if
      props
      |> Props.exists PKey.LinearRangeViewBase<'TOption, 'TValue>.UseMinimumSizeChanging
    then
      terminalElement.TrySetEventHandler(
        PKey.LinearRangeViewBase<'TOption, 'TValue>.UseMinimumSizeChanging,
        view.UseMinimumSizeChanging
      )

    if props |> Props.exists PKey.LinearRangeViewBase<'TOption, 'TValue>.ValueChanged then
      terminalElement.TrySetEventHandler(PKey.LinearRangeViewBase<'TOption, 'TValue>.ValueChanged, view.ValueChanged)

    if
      props
      |> Props.exists PKey.LinearRangeViewBase<'TOption, 'TValue>.ValueChangedUntyped
    then
      terminalElement.TrySetEventHandler(
        PKey.LinearRangeViewBase<'TOption, 'TValue>.ValueChangedUntyped,
        view.ValueChangedUntyped
      )

    if props |> Props.exists PKey.LinearRangeViewBase<'TOption, 'TValue>.ValueChanging then
      terminalElement.TrySetEventHandler(PKey.LinearRangeViewBase<'TOption, 'TValue>.ValueChanging, view.ValueChanging)

  static member clearProp(terminalElement: ViewBackedTerminalElement, propertyId: PropertyId) =
    let view = terminalElement.View :?> LinearRangeViewBase<'TOption, 'TValue>

    match propertyId.Value with
    | 194 -> view.AllowEmpty <- Unchecked.defaultof<_>
    | 195 -> view.FocusedOption <- Unchecked.defaultof<_>
    | 196 -> view.LegendsOrientation <- Unchecked.defaultof<_>
    | 197 -> view.MinimumInnerSpacing <- Unchecked.defaultof<_>
    | 198 -> view.Options <- Unchecked.defaultof<_>
    | 199 -> view.Orientation <- Unchecked.defaultof<_>
    | 200 -> view.ShowEndSpacing <- Unchecked.defaultof<_>
    | 201 -> view.ShowLegends <- Unchecked.defaultof<_>
    | 202 -> view.Style <- Unchecked.defaultof<_>
    | 203 -> view.Text <- ""
    | 204 -> view.UseMinimumSize <- Unchecked.defaultof<_>
    | 205 -> view.Value <- Unchecked.defaultof<_>
    | _ -> ViewPropHandler.clearProp (terminalElement, propertyId)

type internal LinearMultiSelectorPropHandler<'T> =
  static member applyNativeProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    LinearRangeViewBasePropHandler<'T, IReadOnlyList<'T>>.applyNativeProps (terminalElement, props)

    let view = terminalElement.View :?> LinearMultiSelector<'T>

    // Properties
    props
    |> Props.tryFind PKey.LinearMultiSelector<'T>.Value
    |> Option.iter (fun v -> view.Value <- v)

  static member clearProp(terminalElement: ViewBackedTerminalElement, propertyId: PropertyId) =
    let view = terminalElement.View :?> LinearMultiSelector<'T>

    match propertyId.Value with
    | 222 -> view.Value <- Unchecked.defaultof<_>
    | _ -> LinearRangeViewBasePropHandler<'T, IReadOnlyList<'T>>.clearProp (terminalElement, propertyId)

type internal LinearMultiSelectorPropHandler =
  static member applyNativeProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    LinearMultiSelectorPropHandler<string>.applyNativeProps (terminalElement, props)

  static member clearProp(terminalElement: ViewBackedTerminalElement, propertyId: PropertyId) =
    match propertyId.Value with
    | _ -> LinearMultiSelectorPropHandler<string>.clearProp (terminalElement, propertyId)

type internal LinearRangePropHandler<'T> =
  static member applyNativeProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    LinearRangeViewBasePropHandler<'T, LinearRangeSpan<'T>>.applyNativeProps (terminalElement, props)

    let view = terminalElement.View :?> LinearRange<'T>

    // Properties
    props
    |> Props.tryFind PKey.LinearRange<'T>.RangeAllowSingle
    |> Option.iter (fun v -> view.RangeAllowSingle <- v)

    props
    |> Props.tryFind PKey.LinearRange<'T>.RangeKind
    |> Option.iter (fun v -> view.RangeKind <- v)

    props
    |> Props.tryFind PKey.LinearRange<'T>.Value
    |> Option.iter (fun v -> view.Value <- v)

  static member clearProp(terminalElement: ViewBackedTerminalElement, propertyId: PropertyId) =
    let view = terminalElement.View :?> LinearRange<'T>

    match propertyId.Value with
    | 223 -> view.RangeAllowSingle <- Unchecked.defaultof<_>
    | 224 -> view.RangeKind <- Unchecked.defaultof<_>
    | 225 -> view.Value <- Unchecked.defaultof<_>
    | _ -> LinearRangeViewBasePropHandler<'T, LinearRangeSpan<'T>>.clearProp (terminalElement, propertyId)

type internal LinearRangePropHandler =
  static member applyNativeProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    LinearRangePropHandler<string>.applyNativeProps (terminalElement, props)

  static member clearProp(terminalElement: ViewBackedTerminalElement, propertyId: PropertyId) =
    match propertyId.Value with
    | _ -> LinearRangePropHandler<string>.clearProp (terminalElement, propertyId)

type internal LinearSelectorPropHandler<'T> =
  static member applyNativeProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    LinearRangeViewBasePropHandler<'T, 'T>.applyNativeProps (terminalElement, props)

    let view = terminalElement.View :?> LinearSelector<'T>

    // Properties
    props
    |> Props.tryFind PKey.LinearSelector<'T>.SelectedIndex
    |> Option.iter (fun v -> view.SelectedIndex <- v)

    props
    |> Props.tryFind PKey.LinearSelector<'T>.Value
    |> Option.iter (fun v -> view.Value <- v)

  static member clearProp(terminalElement: ViewBackedTerminalElement, propertyId: PropertyId) =
    let view = terminalElement.View :?> LinearSelector<'T>

    match propertyId.Value with
    | 226 -> view.SelectedIndex <- Unchecked.defaultof<_>
    | 227 -> view.Value <- Unchecked.defaultof<_>
    | _ -> LinearRangeViewBasePropHandler<'T, 'T>.clearProp (terminalElement, propertyId)

type internal LinearSelectorPropHandler =
  static member applyNativeProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    LinearSelectorPropHandler<string>.applyNativeProps (terminalElement, props)

  static member clearProp(terminalElement: ViewBackedTerminalElement, propertyId: PropertyId) =
    match propertyId.Value with
    | _ -> LinearSelectorPropHandler<string>.clearProp (terminalElement, propertyId)

type internal LinkPropHandler =
  static member applyNativeProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    ViewPropHandler.applyNativeProps (terminalElement, props)

    let view = terminalElement.View :?> Link

    // Properties
    props |> Props.tryFind PKey.Link.Url |> Option.iter (fun v -> view.Url <- v)

    // Events
    if props |> Props.exists PKey.Link.UrlChanged then
      terminalElement.TrySetEventHandler(PKey.Link.UrlChanged, view.UrlChanged)

    if props |> Props.exists PKey.Link.UrlChanging then
      terminalElement.TrySetEventHandler(PKey.Link.UrlChanging, view.UrlChanging)

  static member clearProp(terminalElement: ViewBackedTerminalElement, propertyId: PropertyId) =
    let view = terminalElement.View :?> Link

    match propertyId.Value with
    | 228 -> view.Url <- ""
    | _ -> ViewPropHandler.clearProp (terminalElement, propertyId)

type internal ListViewPropHandler =
  static member applyNativeProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    ViewPropHandler.applyNativeProps (terminalElement, props)

    let view = terminalElement.View :?> ListView

    // Properties
    props
    |> Props.tryFind PKey.ListView.KeystrokeNavigator
    |> Option.iter (fun v -> view.KeystrokeNavigator <- v)

    props
    |> Props.tryFind PKey.ListView.MarkMultiple
    |> Option.iter (fun v -> view.MarkMultiple <- v)

    props
    |> Props.tryFind PKey.ListView.SelectedItem
    |> Option.iter (fun v -> view.SelectedItem <- v)

    props
    |> Props.tryFind PKey.ListView.ShowMarks
    |> Option.iter (fun v -> view.ShowMarks <- v)

    props
    |> Props.tryFind PKey.ListView.Source
    |> Option.iter (fun v -> view.Source <- v)

    props
    |> Props.tryFind PKey.ListView.Value
    |> Option.iter (fun v -> view.Value <- v)

    // Events
    if props |> Props.exists PKey.ListView.CollectionChanged then
      terminalElement.TrySetEventHandler(PKey.ListView.CollectionChanged, view.CollectionChanged)

    if props |> Props.exists PKey.ListView.RowRender then
      terminalElement.TrySetEventHandler(PKey.ListView.RowRender, view.RowRender)

    if props |> Props.exists PKey.ListView.SourceChanged then
      terminalElement.TrySetEventHandler(PKey.ListView.SourceChanged, view.SourceChanged)

    if props |> Props.exists PKey.ListView.ValueChanged then
      terminalElement.TrySetEventHandler(PKey.ListView.ValueChanged, view.ValueChanged)

    if props |> Props.exists PKey.ListView.ValueChangedUntyped then
      terminalElement.TrySetEventHandler(PKey.ListView.ValueChangedUntyped, view.ValueChangedUntyped)

    if props |> Props.exists PKey.ListView.ValueChanging then
      terminalElement.TrySetEventHandler(PKey.ListView.ValueChanging, view.ValueChanging)

  static member clearProp(terminalElement: ViewBackedTerminalElement, propertyId: PropertyId) =
    let view = terminalElement.View :?> ListView

    match propertyId.Value with
    | 231 -> view.KeystrokeNavigator <- Unchecked.defaultof<_>
    | 232 -> view.MarkMultiple <- Unchecked.defaultof<_>
    | 233 -> view.SelectedItem <- Unchecked.defaultof<_>
    | 234 -> view.ShowMarks <- Unchecked.defaultof<_>
    | 235 -> view.Source <- Unchecked.defaultof<_>
    | 236 -> view.Value <- Unchecked.defaultof<_>
    | _ -> ViewPropHandler.clearProp (terminalElement, propertyId)

type internal ListViewPropHandler<'T> =
  static member applyNativeProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    ListViewPropHandler.applyNativeProps (terminalElement, props)

    let view = terminalElement.View :?> ListView<'T>

    // Properties
    props
    |> Props.tryFind PKey.ListView'<'T>.Index
    |> Option.iter (fun v -> view.Index <- v)

    props
    |> Props.tryFind PKey.ListView'<'T>.SelectedItem
    |> Option.iter (fun v -> view.SelectedItem <- v)

    props
    |> Props.tryFind PKey.ListView'<'T>.Value
    |> Option.iter (fun v -> view.Value <- v)

    // Events
    if props |> Props.exists PKey.ListView'<'T>.ValueChanged then
      terminalElement.TrySetEventHandler(PKey.ListView'<'T>.ValueChanged, view.ValueChanged)

    if props |> Props.exists PKey.ListView'<'T>.ValueChangedUntyped then
      terminalElement.TrySetEventHandler(PKey.ListView'<'T>.ValueChangedUntyped, view.ValueChangedUntyped)

    if props |> Props.exists PKey.ListView'<'T>.ValueChanging then
      terminalElement.TrySetEventHandler(PKey.ListView'<'T>.ValueChanging, view.ValueChanging)

  static member clearProp(terminalElement: ViewBackedTerminalElement, propertyId: PropertyId) =
    let view = terminalElement.View :?> ListView<'T>

    match propertyId.Value with
    | 243 -> view.Index <- Unchecked.defaultof<_>
    | 244 -> view.SelectedItem <- Unchecked.defaultof<_>
    | 245 -> view.Value <- Unchecked.defaultof<_>
    | _ -> ListViewPropHandler.clearProp (terminalElement, propertyId)

type internal MarginViewPropHandler =
  static member applyNativeProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    AdornmentViewPropHandler.applyNativeProps (terminalElement, props)

    let view = terminalElement.View :?> MarginView

    // Properties
    props
    |> Props.tryFind PKey.MarginView.ShadowSize
    |> Option.iter (fun v -> view.ShadowSize <- v)

    props
    |> Props.tryFind PKey.MarginView.ShadowStyle
    |> Option.iter (fun v -> view.ShadowStyle <- v)

  static member clearProp(terminalElement: ViewBackedTerminalElement, propertyId: PropertyId) =
    let view = terminalElement.View :?> MarginView

    match propertyId.Value with
    | 249 -> view.ShadowSize <- Unchecked.defaultof<_>
    | 250 -> view.ShadowStyle <- Unchecked.defaultof<_>
    | _ -> AdornmentViewPropHandler.clearProp (terminalElement, propertyId)

type internal MarkdownPropHandler =
  static member applyNativeProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    ViewPropHandler.applyNativeProps (terminalElement, props)

    let view = terminalElement.View :?> Markdown

    // Properties
    props
    |> Props.tryFind PKey.Markdown.EnableSixelImages
    |> Option.iter (fun v -> view.EnableSixelImages <- v)

    props
    |> Props.tryFind PKey.Markdown.HotKeySpecifier
    |> Option.iter (fun v -> view.HotKeySpecifier <- v)

    props
    |> Props.tryFind PKey.Markdown.ImageLoader
    |> Option.iter (fun v -> view.ImageLoader <- v)

    props
    |> Props.tryFind PKey.Markdown.MarkdownPipeline
    |> Option.iter (fun v -> view.MarkdownPipeline <- v)

    props
    |> Props.tryFind PKey.Markdown.ShowCopyButtons
    |> Option.iter (fun v -> view.ShowCopyButtons <- v)

    props
    |> Props.tryFind PKey.Markdown.ShowHeadingPrefix
    |> Option.iter (fun v -> view.ShowHeadingPrefix <- v)

    props
    |> Props.tryFind PKey.Markdown.SyntaxHighlighter
    |> Option.iter (fun v -> view.SyntaxHighlighter <- v)

    props
    |> Props.tryFind PKey.Markdown.Text
    |> Option.iter (fun v -> view.Text <- v)

    props
    |> Props.tryFind PKey.Markdown.UseThemeBackground
    |> Option.iter (fun v -> view.UseThemeBackground <- v)

    // Events
    if props |> Props.exists PKey.Markdown.LinkClicked then
      terminalElement.TrySetEventHandler(PKey.Markdown.LinkClicked, view.LinkClicked)

    if props |> Props.exists PKey.Markdown.MarkdownChanged then
      terminalElement.TrySetEventHandler(PKey.Markdown.MarkdownChanged, view.MarkdownChanged)

  static member clearProp(terminalElement: ViewBackedTerminalElement, propertyId: PropertyId) =
    let view = terminalElement.View :?> Markdown

    match propertyId.Value with
    | 251 -> view.EnableSixelImages <- Unchecked.defaultof<_>
    | 252 -> view.HotKeySpecifier <- Unchecked.defaultof<_>
    | 253 -> view.ImageLoader <- Unchecked.defaultof<_>
    | 254 -> view.MarkdownPipeline <- Unchecked.defaultof<_>
    | 255 -> view.ShowCopyButtons <- Unchecked.defaultof<_>
    | 256 -> view.ShowHeadingPrefix <- Unchecked.defaultof<_>
    | 257 -> view.SyntaxHighlighter <- Unchecked.defaultof<_>
    | 258 -> view.Text <- ""
    | 259 -> view.UseThemeBackground <- Unchecked.defaultof<_>
    | _ -> ViewPropHandler.clearProp (terminalElement, propertyId)

type internal MarkdownCodeBlockPropHandler =
  static member applyNativeProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    ViewPropHandler.applyNativeProps (terminalElement, props)

    let view = terminalElement.View :?> MarkdownCodeBlock

    // Properties
    props
    |> Props.tryFind PKey.MarkdownCodeBlock.CodeLines
    |> Option.iter (fun v -> view.CodeLines <- v)

    props
    |> Props.tryFind PKey.MarkdownCodeBlock.Language
    |> Option.iter (fun v -> view.Language <- v)

    props
    |> Props.tryFind PKey.MarkdownCodeBlock.ShowCopyButton
    |> Option.iter (fun v -> view.ShowCopyButton <- v)

    props
    |> Props.tryFind PKey.MarkdownCodeBlock.SyntaxHighlighter
    |> Option.iter (fun v -> view.SyntaxHighlighter <- v)

    props
    |> Props.tryFind PKey.MarkdownCodeBlock.Text
    |> Option.iter (fun v -> view.Text <- v)

    props
    |> Props.tryFind PKey.MarkdownCodeBlock.ThemeBackground
    |> Option.iter (fun v -> view.ThemeBackground <- v)

  static member clearProp(terminalElement: ViewBackedTerminalElement, propertyId: PropertyId) =
    let view = terminalElement.View :?> MarkdownCodeBlock

    match propertyId.Value with
    | 262 -> view.CodeLines <- Unchecked.defaultof<_>
    | 263 -> view.Language <- ""
    | 264 -> view.ShowCopyButton <- Unchecked.defaultof<_>
    | 265 -> view.SyntaxHighlighter <- Unchecked.defaultof<_>
    | 266 -> view.Text <- ""
    | 267 -> view.ThemeBackground <- Unchecked.defaultof<_>
    | _ -> ViewPropHandler.clearProp (terminalElement, propertyId)

type internal MarkdownTablePropHandler =
  static member applyNativeProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    ViewPropHandler.applyNativeProps (terminalElement, props)

    let view = terminalElement.View :?> MarkdownTable

    // Properties
    props
    |> Props.tryFind PKey.MarkdownTable.SyntaxHighlighter
    |> Option.iter (fun v -> view.SyntaxHighlighter <- v)

    props
    |> Props.tryFind PKey.MarkdownTable.TableData
    |> Option.iter (fun v -> view.TableData <- v)

    props
    |> Props.tryFind PKey.MarkdownTable.Text
    |> Option.iter (fun v -> view.Text <- v)

    props
    |> Props.tryFind PKey.MarkdownTable.UseThemeBackground
    |> Option.iter (fun v -> view.UseThemeBackground <- v)

    // Events
    if props |> Props.exists PKey.MarkdownTable.LinkClicked then
      terminalElement.TrySetEventHandler(PKey.MarkdownTable.LinkClicked, view.LinkClicked)

  static member clearProp(terminalElement: ViewBackedTerminalElement, propertyId: PropertyId) =
    let view = terminalElement.View :?> MarkdownTable

    match propertyId.Value with
    | 268 -> view.SyntaxHighlighter <- Unchecked.defaultof<_>
    | 269 -> view.TableData <- Unchecked.defaultof<_>
    | 270 -> view.Text <- ""
    | 271 -> view.UseThemeBackground <- Unchecked.defaultof<_>
    | _ -> ViewPropHandler.clearProp (terminalElement, propertyId)

type internal MenuPropHandler =
  static member applyNativeProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    BarPropHandler.applyNativeProps (terminalElement, props)

    let view = terminalElement.View :?> Menu

    // Properties
    props
    |> Props.tryFind PKey.Menu.SuperMenuItem
    |> Option.iter (fun v -> view.SuperMenuItem <- v)

    props |> Props.tryFind PKey.Menu.Value |> Option.iter (fun v -> view.Value <- v)

    // Events
    if props |> Props.exists PKey.Menu.SelectedMenuItemChanged then
      terminalElement.TrySetEventHandler(PKey.Menu.SelectedMenuItemChanged, view.SelectedMenuItemChanged)

    if props |> Props.exists PKey.Menu.ValueChanged then
      terminalElement.TrySetEventHandler(PKey.Menu.ValueChanged, view.ValueChanged)

    if props |> Props.exists PKey.Menu.ValueChangedUntyped then
      terminalElement.TrySetEventHandler(PKey.Menu.ValueChangedUntyped, view.ValueChangedUntyped)

    if props |> Props.exists PKey.Menu.ValueChanging then
      terminalElement.TrySetEventHandler(PKey.Menu.ValueChanging, view.ValueChanging)

  static member clearProp(terminalElement: ViewBackedTerminalElement, propertyId: PropertyId) =
    let view = terminalElement.View :?> Menu

    match propertyId.Value with
    | 273 -> view.SuperMenuItem <- Unchecked.defaultof<_>
    | 275 -> view.Value <- Unchecked.defaultof<_>
    | _ -> BarPropHandler.clearProp (terminalElement, propertyId)

type internal MenuBarPropHandler =
  static member applyNativeProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    MenuPropHandler.applyNativeProps (terminalElement, props)

    let view = terminalElement.View :?> MenuBar

    // Properties
    props |> Props.tryFind PKey.MenuBar.Key |> Option.iter (fun v -> view.Key <- v)

    // Events
    if props |> Props.exists PKey.MenuBar.KeyChanged then
      terminalElement.TrySetEventHandler(PKey.MenuBar.KeyChanged, view.KeyChanged)

  static member clearProp(terminalElement: ViewBackedTerminalElement, propertyId: PropertyId) =
    let view = terminalElement.View :?> MenuBar

    match propertyId.Value with
    | 281 -> view.Key <- Terminal.Gui.Input.Key.Empty
    | _ -> MenuPropHandler.clearProp (terminalElement, propertyId)

type internal NumericUpDownPropHandler<'T> =
  static member applyNativeProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    ViewPropHandler.applyNativeProps (terminalElement, props)

    let view = terminalElement.View :?> NumericUpDown<'T>

    // Properties
    props
    |> Props.tryFind PKey.NumericUpDown<'T>.Format
    |> Option.iter (fun v -> view.Format <- v)

    props
    |> Props.tryFind PKey.NumericUpDown<'T>.Increment
    |> Option.iter (fun v -> view.Increment <- v)

    props
    |> Props.tryFind PKey.NumericUpDown<'T>.Value
    |> Option.iter (fun v -> view.Value <- v)

    // Events
    if props |> Props.exists PKey.NumericUpDown<'T>.FormatChanged then
      terminalElement.TrySetEventHandler(PKey.NumericUpDown<'T>.FormatChanged, view.FormatChanged)

    if props |> Props.exists PKey.NumericUpDown<'T>.IncrementChanged then
      terminalElement.TrySetEventHandler(PKey.NumericUpDown<'T>.IncrementChanged, view.IncrementChanged)

    if props |> Props.exists PKey.NumericUpDown<'T>.ValueChanged then
      terminalElement.TrySetEventHandler(PKey.NumericUpDown<'T>.ValueChanged, view.ValueChanged)

    if props |> Props.exists PKey.NumericUpDown<'T>.ValueChangedUntyped then
      terminalElement.TrySetEventHandler(PKey.NumericUpDown<'T>.ValueChangedUntyped, view.ValueChangedUntyped)

    if props |> Props.exists PKey.NumericUpDown<'T>.ValueChanging then
      terminalElement.TrySetEventHandler(PKey.NumericUpDown<'T>.ValueChanging, view.ValueChanging)

  static member clearProp(terminalElement: ViewBackedTerminalElement, propertyId: PropertyId) =
    let view = terminalElement.View :?> NumericUpDown<'T>

    match propertyId.Value with
    | 283 -> view.Format <- ""
    | 284 -> view.Increment <- Unchecked.defaultof<_>
    | 285 -> view.Value <- Unchecked.defaultof<_>
    | _ -> ViewPropHandler.clearProp (terminalElement, propertyId)

type internal NumericUpDownPropHandler =
  static member applyNativeProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    NumericUpDownPropHandler<int>.applyNativeProps (terminalElement, props)

  static member clearProp(terminalElement: ViewBackedTerminalElement, propertyId: PropertyId) =
    match propertyId.Value with
    | _ -> NumericUpDownPropHandler<int>.clearProp (terminalElement, propertyId)

type internal PaddingViewPropHandler =
  static member applyNativeProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    AdornmentViewPropHandler.applyNativeProps (terminalElement, props)

  static member clearProp(terminalElement: ViewBackedTerminalElement, propertyId: PropertyId) =
    match propertyId.Value with
    | _ -> AdornmentViewPropHandler.clearProp (terminalElement, propertyId)

type internal PopoverImplPropHandler =
  static member applyNativeProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    ViewPropHandler.applyNativeProps (terminalElement, props)

    let view = terminalElement.View :?> PopoverImpl

    // Properties
    props
    |> Props.tryFind PKey.PopoverImpl.Anchor
    |> Option.iter (fun v -> view.Anchor <- v)

    props
    |> Props.tryFind PKey.PopoverImpl.Owner
    |> Option.iter (fun v -> view.Owner <- v)

    props
    |> Props.tryFind PKey.PopoverImpl.Target
    |> Option.iter (fun v -> view.Target <- v)

  static member clearProp(terminalElement: ViewBackedTerminalElement, propertyId: PropertyId) =
    let view = terminalElement.View :?> PopoverImpl

    match propertyId.Value with
    | 291 -> view.Anchor <- Unchecked.defaultof<_>
    | 292 -> view.Owner <- Unchecked.defaultof<_>
    | 293 -> view.Target <- Unchecked.defaultof<_>
    | _ -> ViewPropHandler.clearProp (terminalElement, propertyId)

type internal PopoverPropHandler<'TView, 'TResult
  when 'TView: (new: unit -> 'TView) and 'TView :> Terminal.Gui.ViewBase.View> =
  static member applyNativeProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    PopoverImplPropHandler.applyNativeProps (terminalElement, props)

    let view = terminalElement.View :?> Popover<'TView, 'TResult>

    // Properties
    props
    |> Props.tryFind PKey.Popover<'TView, 'TResult>.ContentView
    |> Option.iter (fun v -> view.ContentView <- v)

    props
    |> Props.tryFind PKey.Popover<'TView, 'TResult>.ResultExtractor
    |> Option.iter (fun v -> view.ResultExtractor <- v)

    // Events
    if props |> Props.exists PKey.Popover<'TView, 'TResult>.ResultChanged then
      terminalElement.TrySetEventHandler(PKey.Popover<'TView, 'TResult>.ResultChanged, view.ResultChanged)

  static member clearProp(terminalElement: ViewBackedTerminalElement, propertyId: PropertyId) =
    let view = terminalElement.View :?> Popover<'TView, 'TResult>

    match propertyId.Value with
    | 294 -> view.ContentView <- Unchecked.defaultof<_>
    | 296 -> view.ResultExtractor <- Unchecked.defaultof<_>
    | _ -> PopoverImplPropHandler.clearProp (terminalElement, propertyId)

type internal PopoverMenuPropHandler =
  static member applyNativeProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    PopoverPropHandler<Terminal.Gui.Views.Menu, Terminal.Gui.Views.MenuItem>.applyNativeProps (terminalElement, props)

    let view = terminalElement.View :?> PopoverMenu

    // Properties
    props
    |> Props.tryFind PKey.PopoverMenu.Key
    |> Option.iter (fun v -> view.Key <- v)

    props
    |> Props.tryFind PKey.PopoverMenu.MouseFlags
    |> Option.iter (fun v -> view.MouseFlags <- v)

    props
    |> Props.tryFind PKey.PopoverMenu.Root
    |> Option.iter (fun v -> view.Root <- v)

    // Events
    if props |> Props.exists PKey.PopoverMenu.KeyChanged then
      terminalElement.TrySetEventHandler(PKey.PopoverMenu.KeyChanged, view.KeyChanged)

  static member clearProp(terminalElement: ViewBackedTerminalElement, propertyId: PropertyId) =
    let view = terminalElement.View :?> PopoverMenu

    match propertyId.Value with
    | 298 -> view.Key <- Terminal.Gui.Input.Key.Empty
    | 299 -> view.MouseFlags <- Unchecked.defaultof<_>
    | 300 -> view.Root <- Unchecked.defaultof<_>
    | _ ->
      PopoverPropHandler<Terminal.Gui.Views.Menu, Terminal.Gui.Views.MenuItem>.clearProp (terminalElement, propertyId)

type internal ProgressBarPropHandler =
  static member applyNativeProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    ViewPropHandler.applyNativeProps (terminalElement, props)

    let view = terminalElement.View :?> ProgressBar

    // Properties
    props
    |> Props.tryFind PKey.ProgressBar.BidirectionalMarquee
    |> Option.iter (fun v -> view.BidirectionalMarquee <- v)

    props
    |> Props.tryFind PKey.ProgressBar.Fraction
    |> Option.iter (fun v -> view.Fraction <- v)

    props
    |> Props.tryFind PKey.ProgressBar.ProgressBarFormat
    |> Option.iter (fun v -> view.ProgressBarFormat <- v)

    props
    |> Props.tryFind PKey.ProgressBar.ProgressBarStyle
    |> Option.iter (fun v -> view.ProgressBarStyle <- v)

    props
    |> Props.tryFind PKey.ProgressBar.SegmentCharacter
    |> Option.iter (fun v -> view.SegmentCharacter <- v)

    props
    |> Props.tryFind PKey.ProgressBar.SyncWithTerminal
    |> Option.iter (fun v -> view.SyncWithTerminal <- v)

    props
    |> Props.tryFind PKey.ProgressBar.Text
    |> Option.iter (fun v -> view.Text <- v)

  static member clearProp(terminalElement: ViewBackedTerminalElement, propertyId: PropertyId) =
    let view = terminalElement.View :?> ProgressBar

    match propertyId.Value with
    | 303 -> view.BidirectionalMarquee <- Unchecked.defaultof<_>
    | 304 -> view.Fraction <- Unchecked.defaultof<_>
    | 305 -> view.ProgressBarFormat <- Unchecked.defaultof<_>
    | 306 -> view.ProgressBarStyle <- Unchecked.defaultof<_>
    | 307 -> view.SegmentCharacter <- Unchecked.defaultof<_>
    | 308 -> view.SyncWithTerminal <- Unchecked.defaultof<_>
    | 309 -> view.Text <- ""
    | _ -> ViewPropHandler.clearProp (terminalElement, propertyId)

type internal RunnablePropHandler =
  static member applyNativeProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    ViewPropHandler.applyNativeProps (terminalElement, props)

    let view = terminalElement.View :?> Runnable

    // Properties
    props
    |> Props.tryFind PKey.Runnable.Result
    |> Option.iter (fun v -> view.Result <- v)

    props
    |> Props.tryFind PKey.Runnable.StopRequested
    |> Option.iter (fun v -> view.StopRequested <- v)

    // Events
    if props |> Props.exists PKey.Runnable.IsModalChanged then
      terminalElement.TrySetEventHandler(PKey.Runnable.IsModalChanged, view.IsModalChanged)

    if props |> Props.exists PKey.Runnable.IsRunningChanged then
      terminalElement.TrySetEventHandler(PKey.Runnable.IsRunningChanged, view.IsRunningChanged)

    if props |> Props.exists PKey.Runnable.IsRunningChanging then
      terminalElement.TrySetEventHandler(PKey.Runnable.IsRunningChanging, view.IsRunningChanging)

  static member clearProp(terminalElement: ViewBackedTerminalElement, propertyId: PropertyId) =
    let view = terminalElement.View :?> Runnable

    match propertyId.Value with
    | 310 -> view.Result <- Unchecked.defaultof<_>
    | 311 -> view.StopRequested <- Unchecked.defaultof<_>
    | _ -> ViewPropHandler.clearProp (terminalElement, propertyId)

type internal RunnablePropHandler<'TResult> =
  static member applyNativeProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    RunnablePropHandler.applyNativeProps (terminalElement, props)

    let view = terminalElement.View :?> Runnable<'TResult>

    // Properties
    props
    |> Props.tryFind PKey.Runnable'<'TResult>.Result
    |> Option.iter (fun v -> view.Result <- v)

  static member clearProp(terminalElement: ViewBackedTerminalElement, propertyId: PropertyId) =
    let view = terminalElement.View :?> Runnable<'TResult>

    match propertyId.Value with
    | 315 -> view.Result <- Unchecked.defaultof<_>
    | _ -> RunnablePropHandler.clearProp (terminalElement, propertyId)

type internal DialogPropHandler<'TResult> =
  static member applyNativeProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    RunnablePropHandler<'TResult>.applyNativeProps (terminalElement, props)

    let view = terminalElement.View :?> Dialog<'TResult>

    // Properties
    props
    |> Props.tryFind PKey.Dialog<'TResult>.ButtonAlignment
    |> Option.iter (fun v -> view.ButtonAlignment <- v)

    props
    |> Props.tryFind PKey.Dialog<'TResult>.ButtonAlignmentModes
    |> Option.iter (fun v -> view.ButtonAlignmentModes <- v)

    props
    |> Props.tryFind PKey.Dialog<'TResult>.Buttons
    |> Option.iter (fun v -> view.Buttons <- v)

  static member clearProp(terminalElement: ViewBackedTerminalElement, propertyId: PropertyId) =
    let view = terminalElement.View :?> Dialog<'TResult>

    match propertyId.Value with
    | 316 -> view.ButtonAlignment <- Unchecked.defaultof<_>
    | 317 -> view.ButtonAlignmentModes <- Unchecked.defaultof<_>
    | 318 -> view.Buttons <- Unchecked.defaultof<_>
    | _ -> RunnablePropHandler<'TResult>.clearProp (terminalElement, propertyId)

type internal RunnableWrapperPropHandler<'TView, 'TResult
  when 'TView: (new: unit -> 'TView) and 'TView :> Terminal.Gui.ViewBase.View> =
  static member applyNativeProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    RunnablePropHandler<'TResult>.applyNativeProps (terminalElement, props)

    let view = terminalElement.View :?> RunnableWrapper<'TView, 'TResult>

    // Properties
    props
    |> Props.tryFind PKey.RunnableWrapper<'TView, 'TResult>.ResultExtractor
    |> Option.iter (fun v -> view.ResultExtractor <- v)

  static member clearProp(terminalElement: ViewBackedTerminalElement, propertyId: PropertyId) =
    let view = terminalElement.View :?> RunnableWrapper<'TView, 'TResult>

    match propertyId.Value with
    | 319 -> view.ResultExtractor <- Unchecked.defaultof<_>
    | _ -> RunnablePropHandler<'TResult>.clearProp (terminalElement, propertyId)

type internal DialogPropHandler =
  static member applyNativeProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    DialogPropHandler<int>.applyNativeProps (terminalElement, props)

    let view = terminalElement.View :?> Dialog

    // Properties
    props
    |> Props.tryFind PKey.Dialog'.Result
    |> Option.iter (fun v -> view.Result <- v)

  static member clearProp(terminalElement: ViewBackedTerminalElement, propertyId: PropertyId) =
    let view = terminalElement.View :?> Dialog

    match propertyId.Value with
    | 320 -> view.Result <- Unchecked.defaultof<_>
    | _ -> DialogPropHandler<int>.clearProp (terminalElement, propertyId)

type internal FileDialogPropHandler =
  static member applyNativeProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    DialogPropHandler<IReadOnlyList<string>>.applyNativeProps (terminalElement, props)

    let view = terminalElement.View :?> FileDialog

    // Properties
    props
    |> Props.tryFind PKey.FileDialog.AllowedTypes
    |> Option.iter (fun v -> view.AllowedTypes <- v)

    props
    |> Props.tryFind PKey.FileDialog.AllowsMultipleSelection
    |> Option.iter (fun v -> view.AllowsMultipleSelection <- v)

    props
    |> Props.tryFind PKey.FileDialog.FileOperationsHandler
    |> Option.iter (fun v -> view.FileOperationsHandler <- v)

    props
    |> Props.tryFind PKey.FileDialog.MustExist
    |> Option.iter (fun v -> view.MustExist <- v)

    props
    |> Props.tryFind PKey.FileDialog.OpenMode
    |> Option.iter (fun v -> view.OpenMode <- v)

    props
    |> Props.tryFind PKey.FileDialog.Path
    |> Option.iter (fun v -> view.Path <- v)

    props
    |> Props.tryFind PKey.FileDialog.SearchMatcher
    |> Option.iter (fun v -> view.SearchMatcher <- v)

    // Events
    if props |> Props.exists PKey.FileDialog.FilesSelected then
      terminalElement.TrySetEventHandler(PKey.FileDialog.FilesSelected, view.FilesSelected)

  static member clearProp(terminalElement: ViewBackedTerminalElement, propertyId: PropertyId) =
    let view = terminalElement.View :?> FileDialog

    match propertyId.Value with
    | 321 -> view.AllowedTypes <- Unchecked.defaultof<_>
    | 322 -> view.AllowsMultipleSelection <- Unchecked.defaultof<_>
    | 323 -> view.FileOperationsHandler <- Unchecked.defaultof<_>
    | 324 -> view.MustExist <- Unchecked.defaultof<_>
    | 325 -> view.OpenMode <- Unchecked.defaultof<_>
    | 326 -> view.Path <- ""
    | 327 -> view.SearchMatcher <- Unchecked.defaultof<_>
    | _ -> DialogPropHandler<IReadOnlyList<string>>.clearProp (terminalElement, propertyId)

type internal PromptPropHandler<'TView, 'TResult
  when 'TView: (new: unit -> 'TView) and 'TView :> Terminal.Gui.ViewBase.View> =
  static member applyNativeProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    DialogPropHandler<'TResult>.applyNativeProps (terminalElement, props)

    let view = terminalElement.View :?> Prompt<'TView, 'TResult>

    // Properties
    props
    |> Props.tryFind PKey.Prompt<'TView, 'TResult>.ResultExtractor
    |> Option.iter (fun v -> view.ResultExtractor <- v)

  static member clearProp(terminalElement: ViewBackedTerminalElement, propertyId: PropertyId) =
    let view = terminalElement.View :?> Prompt<'TView, 'TResult>

    match propertyId.Value with
    | 329 -> view.ResultExtractor <- Unchecked.defaultof<_>
    | _ -> DialogPropHandler<'TResult>.clearProp (terminalElement, propertyId)

type internal OpenDialogPropHandler =
  static member applyNativeProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    FileDialogPropHandler.applyNativeProps (terminalElement, props)

    let view = terminalElement.View :?> OpenDialog

    // Properties
    props
    |> Props.tryFind PKey.OpenDialog.OpenMode
    |> Option.iter (fun v -> view.OpenMode <- v)

  static member clearProp(terminalElement: ViewBackedTerminalElement, propertyId: PropertyId) =
    let view = terminalElement.View :?> OpenDialog

    match propertyId.Value with
    | 330 -> view.OpenMode <- Unchecked.defaultof<_>
    | _ -> FileDialogPropHandler.clearProp (terminalElement, propertyId)

type internal SaveDialogPropHandler =
  static member applyNativeProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    FileDialogPropHandler.applyNativeProps (terminalElement, props)

  static member clearProp(terminalElement: ViewBackedTerminalElement, propertyId: PropertyId) =
    match propertyId.Value with
    | _ -> FileDialogPropHandler.clearProp (terminalElement, propertyId)

type internal ScrollBarPropHandler =
  static member applyNativeProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    ViewPropHandler.applyNativeProps (terminalElement, props)

    let view = terminalElement.View :?> ScrollBar

    // Properties
    props
    |> Props.tryFind PKey.ScrollBar.Increment
    |> Option.iter (fun v -> view.Increment <- v)

    props
    |> Props.tryFind PKey.ScrollBar.Orientation
    |> Option.iter (fun v -> view.Orientation <- v)

    props
    |> Props.tryFind PKey.ScrollBar.ScrollableContentSize
    |> Option.iter (fun v -> view.ScrollableContentSize <- v)

    props
    |> Props.tryFind PKey.ScrollBar.Value
    |> Option.iter (fun v -> view.Value <- v)

    props
    |> Props.tryFind PKey.ScrollBar.VisibilityMode
    |> Option.iter (fun v -> view.VisibilityMode <- v)

    props
    |> Props.tryFind PKey.ScrollBar.VisibleContentSize
    |> Option.iter (fun v -> view.VisibleContentSize <- v)

    // Events
    if props |> Props.exists PKey.ScrollBar.OrientationChanged then
      terminalElement.TrySetEventHandler(PKey.ScrollBar.OrientationChanged, view.OrientationChanged)

    if props |> Props.exists PKey.ScrollBar.OrientationChanging then
      terminalElement.TrySetEventHandler(PKey.ScrollBar.OrientationChanging, view.OrientationChanging)

    if props |> Props.exists PKey.ScrollBar.ScrollableContentSizeChanged then
      terminalElement.TrySetEventHandler(PKey.ScrollBar.ScrollableContentSizeChanged, view.ScrollableContentSizeChanged)

    if props |> Props.exists PKey.ScrollBar.Scrolled then
      terminalElement.TrySetEventHandler(PKey.ScrollBar.Scrolled, view.Scrolled)

    if props |> Props.exists PKey.ScrollBar.SliderPositionChanged then
      terminalElement.TrySetEventHandler(PKey.ScrollBar.SliderPositionChanged, view.SliderPositionChanged)

    if props |> Props.exists PKey.ScrollBar.ValueChanged then
      terminalElement.TrySetEventHandler(PKey.ScrollBar.ValueChanged, view.ValueChanged)

    if props |> Props.exists PKey.ScrollBar.ValueChangedUntyped then
      terminalElement.TrySetEventHandler(PKey.ScrollBar.ValueChangedUntyped, view.ValueChangedUntyped)

    if props |> Props.exists PKey.ScrollBar.ValueChanging then
      terminalElement.TrySetEventHandler(PKey.ScrollBar.ValueChanging, view.ValueChanging)

  static member clearProp(terminalElement: ViewBackedTerminalElement, propertyId: PropertyId) =
    let view = terminalElement.View :?> ScrollBar

    match propertyId.Value with
    | 331 -> view.Increment <- Unchecked.defaultof<_>
    | 332 -> view.Orientation <- Unchecked.defaultof<_>
    | 333 -> view.ScrollableContentSize <- Unchecked.defaultof<_>
    | 334 -> view.Value <- Unchecked.defaultof<_>
    | 335 -> view.VisibilityMode <- Unchecked.defaultof<_>
    | 336 -> view.VisibleContentSize <- Unchecked.defaultof<_>
    | _ -> ViewPropHandler.clearProp (terminalElement, propertyId)

type internal ScrollButtonPropHandler =
  static member applyNativeProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    ButtonPropHandler.applyNativeProps (terminalElement, props)

    let view = terminalElement.View :?> ScrollButton

    // Properties
    props
    |> Props.tryFind PKey.ScrollButton.Direction
    |> Option.iter (fun v -> view.Direction <- v)

    props
    |> Props.tryFind PKey.ScrollButton.Orientation
    |> Option.iter (fun v -> view.Orientation <- v)

    // Events
    if props |> Props.exists PKey.ScrollButton.OrientationChanged then
      terminalElement.TrySetEventHandler(PKey.ScrollButton.OrientationChanged, view.OrientationChanged)

    if props |> Props.exists PKey.ScrollButton.OrientationChanging then
      terminalElement.TrySetEventHandler(PKey.ScrollButton.OrientationChanging, view.OrientationChanging)

  static member clearProp(terminalElement: ViewBackedTerminalElement, propertyId: PropertyId) =
    let view = terminalElement.View :?> ScrollButton

    match propertyId.Value with
    | 345 -> view.Direction <- Unchecked.defaultof<_>
    | 346 -> view.Orientation <- Unchecked.defaultof<_>
    | _ -> ButtonPropHandler.clearProp (terminalElement, propertyId)

type internal ScrollSliderPropHandler =
  static member applyNativeProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    ViewPropHandler.applyNativeProps (terminalElement, props)

    let view = terminalElement.View :?> ScrollSlider

    // Properties
    props
    |> Props.tryFind PKey.ScrollSlider.Orientation
    |> Option.iter (fun v -> view.Orientation <- v)

    props
    |> Props.tryFind PKey.ScrollSlider.Position
    |> Option.iter (fun v -> view.Position <- v)

    props
    |> Props.tryFind PKey.ScrollSlider.Size
    |> Option.iter (fun v -> view.Size <- v)

    props
    |> Props.tryFind PKey.ScrollSlider.SliderPadding
    |> Option.iter (fun v -> view.SliderPadding <- v)

    props
    |> Props.tryFind PKey.ScrollSlider.Value
    |> Option.iter (fun v -> view.Value <- v)

    props
    |> Props.tryFind PKey.ScrollSlider.VisibleContentSize
    |> Option.iter (fun v -> view.VisibleContentSize <- v)

    // Events
    if props |> Props.exists PKey.ScrollSlider.OrientationChanged then
      terminalElement.TrySetEventHandler(PKey.ScrollSlider.OrientationChanged, view.OrientationChanged)

    if props |> Props.exists PKey.ScrollSlider.OrientationChanging then
      terminalElement.TrySetEventHandler(PKey.ScrollSlider.OrientationChanging, view.OrientationChanging)

    if props |> Props.exists PKey.ScrollSlider.PositionChanged then
      terminalElement.TrySetEventHandler(PKey.ScrollSlider.PositionChanged, view.PositionChanged)

    if props |> Props.exists PKey.ScrollSlider.PositionChanging then
      terminalElement.TrySetEventHandler(PKey.ScrollSlider.PositionChanging, view.PositionChanging)

    if props |> Props.exists PKey.ScrollSlider.Scrolled then
      terminalElement.TrySetEventHandler(PKey.ScrollSlider.Scrolled, view.Scrolled)

    if props |> Props.exists PKey.ScrollSlider.ValueChanged then
      terminalElement.TrySetEventHandler(PKey.ScrollSlider.ValueChanged, view.ValueChanged)

    if props |> Props.exists PKey.ScrollSlider.ValueChangedUntyped then
      terminalElement.TrySetEventHandler(PKey.ScrollSlider.ValueChangedUntyped, view.ValueChangedUntyped)

    if props |> Props.exists PKey.ScrollSlider.ValueChanging then
      terminalElement.TrySetEventHandler(PKey.ScrollSlider.ValueChanging, view.ValueChanging)

  static member clearProp(terminalElement: ViewBackedTerminalElement, propertyId: PropertyId) =
    let view = terminalElement.View :?> ScrollSlider

    match propertyId.Value with
    | 349 -> view.Orientation <- Unchecked.defaultof<_>
    | 350 -> view.Position <- Unchecked.defaultof<_>
    | 351 -> view.Size <- Unchecked.defaultof<_>
    | 352 -> view.SliderPadding <- Unchecked.defaultof<_>
    | 353 -> view.Value <- Unchecked.defaultof<_>
    | 354 -> view.VisibleContentSize <- Unchecked.defaultof<_>
    | _ -> ViewPropHandler.clearProp (terminalElement, propertyId)

type internal SelectorBasePropHandler =
  static member applyNativeProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    ViewPropHandler.applyNativeProps (terminalElement, props)

    let view = terminalElement.View :?> SelectorBase

    // Properties
    props
    |> Props.tryFind PKey.SelectorBase.DoubleClickAccepts
    |> Option.iter (fun v -> view.DoubleClickAccepts <- v)

    props
    |> Props.tryFind PKey.SelectorBase.HorizontalSpace
    |> Option.iter (fun v -> view.HorizontalSpace <- v)

    props
    |> Props.tryFind PKey.SelectorBase.Labels
    |> Option.iter (fun v -> view.Labels <- v)

    props
    |> Props.tryFind PKey.SelectorBase.Orientation
    |> Option.iter (fun v -> view.Orientation <- v)

    props
    |> Props.tryFind PKey.SelectorBase.Styles
    |> Option.iter (fun v -> view.Styles <- v)

    props
    |> Props.tryFind PKey.SelectorBase.TabBehavior
    |> Option.iter (fun v -> view.TabBehavior <- v)

    props
    |> Props.tryFind PKey.SelectorBase.Value
    |> Option.iter (fun v -> view.Value <- v)

    props
    |> Props.tryFind PKey.SelectorBase.Values
    |> Option.iter (fun v -> view.Values <- v)

    // Events
    if props |> Props.exists PKey.SelectorBase.OrientationChanged then
      terminalElement.TrySetEventHandler(PKey.SelectorBase.OrientationChanged, view.OrientationChanged)

    if props |> Props.exists PKey.SelectorBase.OrientationChanging then
      terminalElement.TrySetEventHandler(PKey.SelectorBase.OrientationChanging, view.OrientationChanging)

    if props |> Props.exists PKey.SelectorBase.ValueChanged then
      terminalElement.TrySetEventHandler(PKey.SelectorBase.ValueChanged, view.ValueChanged)

    if props |> Props.exists PKey.SelectorBase.ValueChangedUntyped then
      terminalElement.TrySetEventHandler(PKey.SelectorBase.ValueChangedUntyped, view.ValueChangedUntyped)

    if props |> Props.exists PKey.SelectorBase.ValueChanging then
      terminalElement.TrySetEventHandler(PKey.SelectorBase.ValueChanging, view.ValueChanging)

  static member clearProp(terminalElement: ViewBackedTerminalElement, propertyId: PropertyId) =
    let view = terminalElement.View :?> SelectorBase

    match propertyId.Value with
    | 363 -> view.DoubleClickAccepts <- Unchecked.defaultof<_>
    | 364 -> view.HorizontalSpace <- Unchecked.defaultof<_>
    | 365 -> view.Labels <- Unchecked.defaultof<_>
    | 366 -> view.Orientation <- Unchecked.defaultof<_>
    | 367 -> view.Styles <- Unchecked.defaultof<_>
    | 368 -> view.TabBehavior <- Unchecked.defaultof<_>
    | 369 -> view.Value <- Unchecked.defaultof<_>
    | 370 -> view.Values <- Unchecked.defaultof<_>
    | _ -> ViewPropHandler.clearProp (terminalElement, propertyId)

type internal FlagSelectorPropHandler =
  static member applyNativeProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    SelectorBasePropHandler.applyNativeProps (terminalElement, props)

    let view = terminalElement.View :?> FlagSelector

    // Properties
    props
    |> Props.tryFind PKey.FlagSelector.Value
    |> Option.iter (fun v -> view.Value <- v)

  static member clearProp(terminalElement: ViewBackedTerminalElement, propertyId: PropertyId) =
    let view = terminalElement.View :?> FlagSelector

    match propertyId.Value with
    | 376 -> view.Value <- Unchecked.defaultof<_>
    | _ -> SelectorBasePropHandler.clearProp (terminalElement, propertyId)

type internal OptionSelectorPropHandler =
  static member applyNativeProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    SelectorBasePropHandler.applyNativeProps (terminalElement, props)

    let view = terminalElement.View :?> OptionSelector

    // Properties
    props
    |> Props.tryFind PKey.OptionSelector.FocusedItem
    |> Option.iter (fun v -> view.FocusedItem <- v)

  static member clearProp(terminalElement: ViewBackedTerminalElement, propertyId: PropertyId) =
    let view = terminalElement.View :?> OptionSelector

    match propertyId.Value with
    | 377 -> view.FocusedItem <- Unchecked.defaultof<_>
    | _ -> SelectorBasePropHandler.clearProp (terminalElement, propertyId)

type internal FlagSelectorPropHandler<'TFlagsEnum
  when 'TFlagsEnum: struct
  and 'TFlagsEnum: (new: unit -> 'TFlagsEnum)
  and 'TFlagsEnum :> System.Enum
  and 'TFlagsEnum :> System.ValueType> =
  static member applyNativeProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    FlagSelectorPropHandler.applyNativeProps (terminalElement, props)

    let view = terminalElement.View :?> FlagSelector<'TFlagsEnum>

    // Properties
    props
    |> Props.tryFind PKey.FlagSelector'<'TFlagsEnum>.Value
    |> Option.iter (fun v -> view.Value <- v)

    // Events
    if props |> Props.exists PKey.FlagSelector'<'TFlagsEnum>.ValueChanged then
      terminalElement.TrySetEventHandler(PKey.FlagSelector'<'TFlagsEnum>.ValueChanged, view.ValueChanged)

  static member clearProp(terminalElement: ViewBackedTerminalElement, propertyId: PropertyId) =
    let view = terminalElement.View :?> FlagSelector<'TFlagsEnum>

    match propertyId.Value with
    | 378 -> view.Value <- Unchecked.defaultof<_>
    | _ -> FlagSelectorPropHandler.clearProp (terminalElement, propertyId)

type internal OptionSelectorPropHandler<'TEnum
  when 'TEnum: struct and 'TEnum: (new: unit -> 'TEnum) and 'TEnum :> System.Enum and 'TEnum :> System.ValueType> =
  static member applyNativeProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    OptionSelectorPropHandler.applyNativeProps (terminalElement, props)

    let view = terminalElement.View :?> OptionSelector<'TEnum>

    // Properties
    props
    |> Props.tryFind PKey.OptionSelector'<'TEnum>.Value
    |> Option.iter (fun v -> view.Value <- v)

    props
    |> Props.tryFind PKey.OptionSelector'<'TEnum>.Values
    |> Option.iter (fun v -> view.Values <- v)

    // Events
    if props |> Props.exists PKey.OptionSelector'<'TEnum>.ValueChanged then
      terminalElement.TrySetEventHandler(PKey.OptionSelector'<'TEnum>.ValueChanged, view.ValueChanged)

  static member clearProp(terminalElement: ViewBackedTerminalElement, propertyId: PropertyId) =
    let view = terminalElement.View :?> OptionSelector<'TEnum>

    match propertyId.Value with
    | 380 -> view.Value <- Unchecked.defaultof<_>
    | 381 -> view.Values <- Unchecked.defaultof<_>
    | _ -> OptionSelectorPropHandler.clearProp (terminalElement, propertyId)

type internal ShortcutPropHandler =
  static member applyNativeProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    ViewPropHandler.applyNativeProps (terminalElement, props)

    let view = terminalElement.View :?> Shortcut

    // Properties
    props
    |> Props.tryFind PKey.Shortcut.Action
    |> Option.iter (fun v -> view.Action <- v)

    props
    |> Props.tryFind PKey.Shortcut.AlignmentModes
    |> Option.iter (fun v -> view.AlignmentModes <- v)

    props
    |> Props.tryFind PKey.Shortcut.BindKeyToApplication
    |> Option.iter (fun v -> view.BindKeyToApplication <- v)

    props
    |> Props.tryFind PKey.Shortcut.Command
    |> Option.iter (fun v -> view.Command <- v)

    props
    |> Props.tryFind PKey.Shortcut.CommandView
    |> Option.iter (fun v -> view.CommandView <- v)

    props
    |> Props.tryFind PKey.Shortcut.HelpText
    |> Option.iter (fun v -> view.HelpText <- v)

    props |> Props.tryFind PKey.Shortcut.Key |> Option.iter (fun v -> view.Key <- v)

    props
    |> Props.tryFind PKey.Shortcut.MinimumKeyTextSize
    |> Option.iter (fun v -> view.MinimumKeyTextSize <- v)

    props
    |> Props.tryFind PKey.Shortcut.Orientation
    |> Option.iter (fun v -> view.Orientation <- v)

    props
    |> Props.tryFind PKey.Shortcut.TargetView
    |> Option.iter (fun v -> view.TargetView <- v)

    props
    |> Props.tryFind PKey.Shortcut.Text
    |> Option.iter (fun v -> view.Text <- v)

    // Events
    if props |> Props.exists PKey.Shortcut.OrientationChanged then
      terminalElement.TrySetEventHandler(PKey.Shortcut.OrientationChanged, view.OrientationChanged)

    if props |> Props.exists PKey.Shortcut.OrientationChanging then
      terminalElement.TrySetEventHandler(PKey.Shortcut.OrientationChanging, view.OrientationChanging)

  static member clearProp(terminalElement: ViewBackedTerminalElement, propertyId: PropertyId) =
    let view = terminalElement.View :?> Shortcut

    match propertyId.Value with
    | 383 -> view.Action <- Unchecked.defaultof<_>
    | 384 -> view.AlignmentModes <- Unchecked.defaultof<_>
    | 385 -> view.BindKeyToApplication <- Unchecked.defaultof<_>
    | 386 -> view.Command <- Unchecked.defaultof<_>
    | 387 -> view.CommandView <- Unchecked.defaultof<_>
    | 389 -> view.HelpText <- ""
    | 390 -> view.Key <- Terminal.Gui.Input.Key.Empty
    | 391 -> view.MinimumKeyTextSize <- Unchecked.defaultof<_>
    | 392 -> view.Orientation <- Unchecked.defaultof<_>
    | 393 -> view.TargetView <- Unchecked.defaultof<_>
    | 395 -> view.Text <- ""
    | _ -> ViewPropHandler.clearProp (terminalElement, propertyId)

type internal MenuItemPropHandler =
  static member applyNativeProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    ShortcutPropHandler.applyNativeProps (terminalElement, props)

    let view = terminalElement.View :?> MenuItem

    // Properties
    props
    |> Props.tryFind PKey.MenuItem.SubMenu
    |> Option.iter (fun v -> view.SubMenu <- v)

  static member clearProp(terminalElement: ViewBackedTerminalElement, propertyId: PropertyId) =
    let view = terminalElement.View :?> MenuItem

    match propertyId.Value with
    | 398 -> view.SubMenu <- Unchecked.defaultof<_>
    | _ -> ShortcutPropHandler.clearProp (terminalElement, propertyId)

type internal MenuBarItemPropHandler =
  static member applyNativeProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    MenuItemPropHandler.applyNativeProps (terminalElement, props)

    let view = terminalElement.View :?> MenuBarItem

    // Properties
    props
    |> Props.tryFind PKey.MenuBarItem.PopoverMenu
    |> Option.iter (fun v -> view.PopoverMenu <- v)

    props
    |> Props.tryFind PKey.MenuBarItem.PopoverMenuOpen
    |> Option.iter (fun v -> view.PopoverMenuOpen <- v)

    // Events
    if props |> Props.exists PKey.MenuBarItem.MenuOpenChanged then
      terminalElement.TrySetEventHandler(PKey.MenuBarItem.MenuOpenChanged, view.MenuOpenChanged)

    if props |> Props.exists PKey.MenuBarItem.PopoverMenuOpenChanged then
      terminalElement.TrySetEventHandler(PKey.MenuBarItem.PopoverMenuOpenChanged, view.PopoverMenuOpenChanged)

  static member clearProp(terminalElement: ViewBackedTerminalElement, propertyId: PropertyId) =
    let view = terminalElement.View :?> MenuBarItem

    match propertyId.Value with
    | 400 -> view.PopoverMenu <- Unchecked.defaultof<_>
    | 402 -> view.PopoverMenuOpen <- Unchecked.defaultof<_>
    | _ -> MenuItemPropHandler.clearProp (terminalElement, propertyId)

type internal SpinnerViewPropHandler =
  static member applyNativeProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    ViewPropHandler.applyNativeProps (terminalElement, props)

    let view = terminalElement.View :?> SpinnerView

    // Properties
    props
    |> Props.tryFind PKey.SpinnerView.AutoSpin
    |> Option.iter (fun v -> view.AutoSpin <- v)

    props
    |> Props.tryFind PKey.SpinnerView.Sequence
    |> Option.iter (fun v -> view.Sequence <- v)

    props
    |> Props.tryFind PKey.SpinnerView.SpinBounce
    |> Option.iter (fun v -> view.SpinBounce <- v)

    props
    |> Props.tryFind PKey.SpinnerView.SpinDelay
    |> Option.iter (fun v -> view.SpinDelay <- v)

    props
    |> Props.tryFind PKey.SpinnerView.SpinReverse
    |> Option.iter (fun v -> view.SpinReverse <- v)

    props
    |> Props.tryFind PKey.SpinnerView.Style
    |> Option.iter (fun v -> view.Style <- v)

    props
    |> Props.tryFind PKey.SpinnerView.SyncWithTerminal
    |> Option.iter (fun v -> view.SyncWithTerminal <- v)

  static member clearProp(terminalElement: ViewBackedTerminalElement, propertyId: PropertyId) =
    let view = terminalElement.View :?> SpinnerView

    match propertyId.Value with
    | 405 -> view.AutoSpin <- Unchecked.defaultof<_>
    | 406 -> view.Sequence <- Unchecked.defaultof<_>
    | 407 -> view.SpinBounce <- Unchecked.defaultof<_>
    | 408 -> view.SpinDelay <- Unchecked.defaultof<_>
    | 409 -> view.SpinReverse <- Unchecked.defaultof<_>
    | 410 -> view.Style <- Unchecked.defaultof<_>
    | 411 -> view.SyncWithTerminal <- Unchecked.defaultof<_>
    | _ -> ViewPropHandler.clearProp (terminalElement, propertyId)

type internal StatusBarPropHandler =
  static member applyNativeProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    BarPropHandler.applyNativeProps (terminalElement, props)

  static member clearProp(terminalElement: ViewBackedTerminalElement, propertyId: PropertyId) =
    match propertyId.Value with
    | _ -> BarPropHandler.clearProp (terminalElement, propertyId)

type internal TableViewPropHandler =
  static member applyNativeProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    ViewPropHandler.applyNativeProps (terminalElement, props)

    let view = terminalElement.View :?> TableView

    // Properties
    props
    |> Props.tryFind PKey.TableView.CollectionNavigator
    |> Option.iter (fun v -> view.CollectionNavigator <- v)

    props
    |> Props.tryFind PKey.TableView.ColumnOffset
    |> Option.iter (fun v -> view.ColumnOffset <- v)

    props
    |> Props.tryFind PKey.TableView.FullRowSelect
    |> Option.iter (fun v -> view.FullRowSelect <- v)

    props
    |> Props.tryFind PKey.TableView.MaxCellWidth
    |> Option.iter (fun v -> view.MaxCellWidth <- v)

    props
    |> Props.tryFind PKey.TableView.MinCellWidth
    |> Option.iter (fun v -> view.MinCellWidth <- v)

    props
    |> Props.tryFind PKey.TableView.MultiSelect
    |> Option.iter (fun v -> view.MultiSelect <- v)

    props
    |> Props.tryFind PKey.TableView.NullSymbol
    |> Option.iter (fun v -> view.NullSymbol <- v)

    props
    |> Props.tryFind PKey.TableView.RowOffset
    |> Option.iter (fun v -> view.RowOffset <- v)

    props
    |> Props.tryFind PKey.TableView.SeparatorSymbol
    |> Option.iter (fun v -> view.SeparatorSymbol <- v)

    props
    |> Props.tryFind PKey.TableView.Style
    |> Option.iter (fun v -> view.Style <- v)

    props
    |> Props.tryFind PKey.TableView.Table
    |> Option.iter (fun v -> view.Table <- v)

    props
    |> Props.tryFind PKey.TableView.UseAllRowsForContentCalculation
    |> Option.iter (fun v -> view.UseAllRowsForContentCalculation <- v)

    props
    |> Props.tryFind PKey.TableView.Value
    |> Option.iter (fun v -> view.Value <- v)

    // Events
    if props |> Props.exists PKey.TableView.ValueChanged then
      terminalElement.TrySetEventHandler(PKey.TableView.ValueChanged, view.ValueChanged)

    if props |> Props.exists PKey.TableView.ValueChangedUntyped then
      terminalElement.TrySetEventHandler(PKey.TableView.ValueChangedUntyped, view.ValueChangedUntyped)

    if props |> Props.exists PKey.TableView.ValueChanging then
      terminalElement.TrySetEventHandler(PKey.TableView.ValueChanging, view.ValueChanging)

  static member clearProp(terminalElement: ViewBackedTerminalElement, propertyId: PropertyId) =
    let view = terminalElement.View :?> TableView

    match propertyId.Value with
    | 412 -> view.CollectionNavigator <- Unchecked.defaultof<_>
    | 413 -> view.ColumnOffset <- Unchecked.defaultof<_>
    | 414 -> view.FullRowSelect <- Unchecked.defaultof<_>
    | 415 -> view.MaxCellWidth <- Unchecked.defaultof<_>
    | 416 -> view.MinCellWidth <- Unchecked.defaultof<_>
    | 417 -> view.MultiSelect <- Unchecked.defaultof<_>
    | 418 -> view.NullSymbol <- ""
    | 419 -> view.RowOffset <- Unchecked.defaultof<_>
    | 420 -> view.SeparatorSymbol <- Unchecked.defaultof<_>
    | 421 -> view.Style <- Unchecked.defaultof<_>
    | 422 -> view.Table <- Unchecked.defaultof<_>
    | 423 -> view.UseAllRowsForContentCalculation <- Unchecked.defaultof<_>
    | 424 -> view.Value <- Unchecked.defaultof<_>
    | _ -> ViewPropHandler.clearProp (terminalElement, propertyId)

type internal TabsPropHandler =
  static member applyNativeProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    ViewPropHandler.applyNativeProps (terminalElement, props)

    let view = terminalElement.View :?> Tabs

    // Properties
    props
    |> Props.tryFind PKey.Tabs.ScrollOffset
    |> Option.iter (fun v -> view.ScrollOffset <- v)

    props
    |> Props.tryFind PKey.Tabs.TabDepth
    |> Option.iter (fun v -> view.TabDepth <- v)

    props
    |> Props.tryFind PKey.Tabs.TabLineStyle
    |> Option.iter (fun v -> view.TabLineStyle <- v)

    props
    |> Props.tryFind PKey.Tabs.TabSide
    |> Option.iter (fun v -> view.TabSide <- v)

    props
    |> Props.tryFind PKey.Tabs.TabSpacing
    |> Option.iter (fun v -> view.TabSpacing <- v)

    props |> Props.tryFind PKey.Tabs.Value |> Option.iter (fun v -> view.Value <- v)

    // Events
    if props |> Props.exists PKey.Tabs.ValueChanged then
      terminalElement.TrySetEventHandler(PKey.Tabs.ValueChanged, view.ValueChanged)

    if props |> Props.exists PKey.Tabs.ValueChangedUntyped then
      terminalElement.TrySetEventHandler(PKey.Tabs.ValueChangedUntyped, view.ValueChangedUntyped)

    if props |> Props.exists PKey.Tabs.ValueChanging then
      terminalElement.TrySetEventHandler(PKey.Tabs.ValueChanging, view.ValueChanging)

  static member clearProp(terminalElement: ViewBackedTerminalElement, propertyId: PropertyId) =
    let view = terminalElement.View :?> Tabs

    match propertyId.Value with
    | 428 -> view.ScrollOffset <- Unchecked.defaultof<_>
    | 429 -> view.TabDepth <- Unchecked.defaultof<_>
    | 430 -> view.TabLineStyle <- Unchecked.defaultof<_>
    | 431 -> view.TabSide <- Unchecked.defaultof<_>
    | 432 -> view.TabSpacing <- Unchecked.defaultof<_>
    | 433 -> view.Value <- Unchecked.defaultof<_>
    | _ -> ViewPropHandler.clearProp (terminalElement, propertyId)

type internal TextFieldPropHandler =
  static member applyNativeProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    ViewPropHandler.applyNativeProps (terminalElement, props)

    let view = terminalElement.View :?> TextField

    // Properties
    props
    |> Props.tryFind PKey.TextField.Autocomplete
    |> Option.iter (fun v -> view.Autocomplete <- v)

    props
    |> Props.tryFind PKey.TextField.InsertionPoint
    |> Option.iter (fun v -> view.InsertionPoint <- v)

    props
    |> Props.tryFind PKey.TextField.ReadOnly
    |> Option.iter (fun v -> view.ReadOnly <- v)

    props
    |> Props.tryFind PKey.TextField.Secret
    |> Option.iter (fun v -> view.Secret <- v)

    props
    |> Props.tryFind PKey.TextField.SelectWordOnlyOnDoubleClick
    |> Option.iter (fun v -> view.SelectWordOnlyOnDoubleClick <- v)

    props
    |> Props.tryFind PKey.TextField.SelectedStart
    |> Option.iter (fun v -> view.SelectedStart <- v)

    props
    |> Props.tryFind PKey.TextField.Text
    |> Option.iter (fun v -> view.Text <- v)

    props
    |> Props.tryFind PKey.TextField.UseSameRuneTypeForWords
    |> Option.iter (fun v -> view.UseSameRuneTypeForWords <- v)

    props
    |> Props.tryFind PKey.TextField.Used
    |> Option.iter (fun v -> view.Used <- v)

    props
    |> Props.tryFind PKey.TextField.Value
    |> Option.iter (fun v -> view.Value <- v)

    // Events
    if props |> Props.exists PKey.TextField.TextChanging then
      terminalElement.TrySetEventHandler(PKey.TextField.TextChanging, view.TextChanging)

    if props |> Props.exists PKey.TextField.ValueChanged then
      terminalElement.TrySetEventHandler(PKey.TextField.ValueChanged, view.ValueChanged)

    if props |> Props.exists PKey.TextField.ValueChangedUntyped then
      terminalElement.TrySetEventHandler(PKey.TextField.ValueChangedUntyped, view.ValueChangedUntyped)

    if props |> Props.exists PKey.TextField.ValueChanging then
      terminalElement.TrySetEventHandler(PKey.TextField.ValueChanging, view.ValueChanging)

  static member clearProp(terminalElement: ViewBackedTerminalElement, propertyId: PropertyId) =
    let view = terminalElement.View :?> TextField

    match propertyId.Value with
    | 438 -> view.Autocomplete <- Unchecked.defaultof<_>
    | 439 -> view.InsertionPoint <- Unchecked.defaultof<_>
    | 440 -> view.ReadOnly <- Unchecked.defaultof<_>
    | 441 -> view.Secret <- Unchecked.defaultof<_>
    | 442 -> view.SelectWordOnlyOnDoubleClick <- Unchecked.defaultof<_>
    | 443 -> view.SelectedStart <- Unchecked.defaultof<_>
    | 444 -> view.Text <- ""
    | 445 -> view.UseSameRuneTypeForWords <- Unchecked.defaultof<_>
    | 446 -> view.Used <- Unchecked.defaultof<_>
    | 447 -> view.Value <- ""
    | _ -> ViewPropHandler.clearProp (terminalElement, propertyId)

type internal DropDownListPropHandler =
  static member applyNativeProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    TextFieldPropHandler.applyNativeProps (terminalElement, props)

    let view = terminalElement.View :?> DropDownList

    // Properties
    props
    |> Props.tryFind PKey.DropDownList.Source
    |> Option.iter (fun v -> view.Source <- v)

  static member clearProp(terminalElement: ViewBackedTerminalElement, propertyId: PropertyId) =
    let view = terminalElement.View :?> DropDownList

    match propertyId.Value with
    | 452 -> view.Source <- Unchecked.defaultof<_>
    | _ -> TextFieldPropHandler.clearProp (terminalElement, propertyId)

type internal DropDownListPropHandler<'TEnum
  when 'TEnum: struct and 'TEnum: (new: unit -> 'TEnum) and 'TEnum :> System.Enum and 'TEnum :> System.ValueType> =
  static member applyNativeProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    DropDownListPropHandler.applyNativeProps (terminalElement, props)

    let view = terminalElement.View :?> DropDownList<'TEnum>

    // Properties
    props
    |> Props.tryFind PKey.DropDownList'<'TEnum>.Value
    |> Option.iter (fun v -> view.Value <- v)

    // Events
    if props |> Props.exists PKey.DropDownList'<'TEnum>.ValueChanged then
      terminalElement.TrySetEventHandler(PKey.DropDownList'<'TEnum>.ValueChanged, view.ValueChanged)

  static member clearProp(terminalElement: ViewBackedTerminalElement, propertyId: PropertyId) =
    let view = terminalElement.View :?> DropDownList<'TEnum>

    match propertyId.Value with
    | 453 -> view.Value <- Unchecked.defaultof<_>
    | _ -> DropDownListPropHandler.clearProp (terminalElement, propertyId)

type internal TextValidateFieldPropHandler =
  static member applyNativeProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    ViewPropHandler.applyNativeProps (terminalElement, props)

    let view = terminalElement.View :?> TextValidateField

    // Properties
    props
    |> Props.tryFind PKey.TextValidateField.Provider
    |> Option.iter (fun v -> view.Provider <- v)

    props
    |> Props.tryFind PKey.TextValidateField.Text
    |> Option.iter (fun v -> view.Text <- v)

    props
    |> Props.tryFind PKey.TextValidateField.Value
    |> Option.iter (fun v -> view.Value <- v)

    // Events
    if props |> Props.exists PKey.TextValidateField.ValueChanged then
      terminalElement.TrySetEventHandler(PKey.TextValidateField.ValueChanged, view.ValueChanged)

    if props |> Props.exists PKey.TextValidateField.ValueChangedUntyped then
      terminalElement.TrySetEventHandler(PKey.TextValidateField.ValueChangedUntyped, view.ValueChangedUntyped)

    if props |> Props.exists PKey.TextValidateField.ValueChanging then
      terminalElement.TrySetEventHandler(PKey.TextValidateField.ValueChanging, view.ValueChanging)

  static member clearProp(terminalElement: ViewBackedTerminalElement, propertyId: PropertyId) =
    let view = terminalElement.View :?> TextValidateField

    match propertyId.Value with
    | 455 -> view.Provider <- Unchecked.defaultof<_>
    | 456 -> view.Text <- ""
    | 457 -> view.Value <- ""
    | _ -> ViewPropHandler.clearProp (terminalElement, propertyId)

type internal DateEditorPropHandler =
  static member applyNativeProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    TextValidateFieldPropHandler.applyNativeProps (terminalElement, props)

    let view = terminalElement.View :?> DateEditor

    // Properties
    props
    |> Props.tryFind PKey.DateEditor.Format
    |> Option.iter (fun v -> view.Format <- v)

    props
    |> Props.tryFind PKey.DateEditor.Value
    |> Option.iter (fun v -> view.Value <- v)

    // Events
    if props |> Props.exists PKey.DateEditor.ValueChanged then
      terminalElement.TrySetEventHandler(PKey.DateEditor.ValueChanged, view.ValueChanged)

    if props |> Props.exists PKey.DateEditor.ValueChangedUntyped then
      terminalElement.TrySetEventHandler(PKey.DateEditor.ValueChangedUntyped, view.ValueChangedUntyped)

    if props |> Props.exists PKey.DateEditor.ValueChanging then
      terminalElement.TrySetEventHandler(PKey.DateEditor.ValueChanging, view.ValueChanging)

  static member clearProp(terminalElement: ViewBackedTerminalElement, propertyId: PropertyId) =
    let view = terminalElement.View :?> DateEditor

    match propertyId.Value with
    | 461 -> view.Format <- Unchecked.defaultof<_>
    | 462 -> view.Value <- Unchecked.defaultof<_>
    | _ -> TextValidateFieldPropHandler.clearProp (terminalElement, propertyId)

type internal TextViewPropHandler =
  static member applyNativeProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    ViewPropHandler.applyNativeProps (terminalElement, props)

    let view = terminalElement.View :?> TextView

    // Properties
    props
    |> Props.tryFind PKey.TextView.EnterKeyAddsLine
    |> Option.iter (fun v -> view.EnterKeyAddsLine <- v)

    props
    |> Props.tryFind PKey.TextView.InheritsPreviousAttribute
    |> Option.iter (fun v -> view.InheritsPreviousAttribute <- v)

    props
    |> Props.tryFind PKey.TextView.InsertionPoint
    |> Option.iter (fun v -> view.InsertionPoint <- v)

    props
    |> Props.tryFind PKey.TextView.IsSelecting
    |> Option.iter (fun v -> view.IsSelecting <- v)

    props
    |> Props.tryFind PKey.TextView.Multiline
    |> Option.iter (fun v -> view.Multiline <- v)

    props
    |> Props.tryFind PKey.TextView.ReadOnly
    |> Option.iter (fun v -> view.ReadOnly <- v)

    props
    |> Props.tryFind PKey.TextView.ScrollBars
    |> Option.iter (fun v -> view.ScrollBars <- v)

    props
    |> Props.tryFind PKey.TextView.SelectWordOnlyOnDoubleClick
    |> Option.iter (fun v -> view.SelectWordOnlyOnDoubleClick <- v)

    props
    |> Props.tryFind PKey.TextView.SelectionStartColumn
    |> Option.iter (fun v -> view.SelectionStartColumn <- v)

    props
    |> Props.tryFind PKey.TextView.SelectionStartRow
    |> Option.iter (fun v -> view.SelectionStartRow <- v)

    props
    |> Props.tryFind PKey.TextView.TabKeyAddsTab
    |> Option.iter (fun v -> view.TabKeyAddsTab <- v)

    props
    |> Props.tryFind PKey.TextView.TabWidth
    |> Option.iter (fun v -> view.TabWidth <- v)

    props
    |> Props.tryFind PKey.TextView.Text
    |> Option.iter (fun v -> view.Text <- v)

    props
    |> Props.tryFind PKey.TextView.UseSameRuneTypeForWords
    |> Option.iter (fun v -> view.UseSameRuneTypeForWords <- v)

    props
    |> Props.tryFind PKey.TextView.Used
    |> Option.iter (fun v -> view.Used <- v)

    props
    |> Props.tryFind PKey.TextView.WordWrap
    |> Option.iter (fun v -> view.WordWrap <- v)

    // Events
    if props |> Props.exists PKey.TextView.ContentsChanged then
      terminalElement.TrySetEventHandler(PKey.TextView.ContentsChanged, view.ContentsChanged)

    if props |> Props.exists PKey.TextView.DrawNormalColor then
      terminalElement.TrySetEventHandler(PKey.TextView.DrawNormalColor, view.DrawNormalColor)

    if props |> Props.exists PKey.TextView.DrawReadOnlyColor then
      terminalElement.TrySetEventHandler(PKey.TextView.DrawReadOnlyColor, view.DrawReadOnlyColor)

    if props |> Props.exists PKey.TextView.DrawSelectionColor then
      terminalElement.TrySetEventHandler(PKey.TextView.DrawSelectionColor, view.DrawSelectionColor)

    if props |> Props.exists PKey.TextView.DrawUsedColor then
      terminalElement.TrySetEventHandler(PKey.TextView.DrawUsedColor, view.DrawUsedColor)

    if props |> Props.exists PKey.TextView.UnwrappedCursorPositionChanged then
      terminalElement.TrySetEventHandler(
        PKey.TextView.UnwrappedCursorPositionChanged,
        view.UnwrappedCursorPositionChanged
      )

  static member clearProp(terminalElement: ViewBackedTerminalElement, propertyId: PropertyId) =
    let view = terminalElement.View :?> TextView

    match propertyId.Value with
    | 466 -> view.EnterKeyAddsLine <- Unchecked.defaultof<_>
    | 467 -> view.InheritsPreviousAttribute <- Unchecked.defaultof<_>
    | 468 -> view.InsertionPoint <- Unchecked.defaultof<_>
    | 469 -> view.IsSelecting <- Unchecked.defaultof<_>
    | 470 -> view.Multiline <- Unchecked.defaultof<_>
    | 471 -> view.ReadOnly <- Unchecked.defaultof<_>
    | 472 -> view.ScrollBars <- Unchecked.defaultof<_>
    | 473 -> view.SelectWordOnlyOnDoubleClick <- Unchecked.defaultof<_>
    | 474 -> view.SelectionStartColumn <- Unchecked.defaultof<_>
    | 475 -> view.SelectionStartRow <- Unchecked.defaultof<_>
    | 476 -> view.TabKeyAddsTab <- Unchecked.defaultof<_>
    | 477 -> view.TabWidth <- Unchecked.defaultof<_>
    | 478 -> view.Text <- ""
    | 479 -> view.UseSameRuneTypeForWords <- Unchecked.defaultof<_>
    | 480 -> view.Used <- Unchecked.defaultof<_>
    | 481 -> view.WordWrap <- Unchecked.defaultof<_>
    | _ -> ViewPropHandler.clearProp (terminalElement, propertyId)

type internal TimeEditorPropHandler =
  static member applyNativeProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    TextValidateFieldPropHandler.applyNativeProps (terminalElement, props)

    let view = terminalElement.View :?> TimeEditor

    // Properties
    props
    |> Props.tryFind PKey.TimeEditor.Format
    |> Option.iter (fun v -> view.Format <- v)

    props
    |> Props.tryFind PKey.TimeEditor.Value
    |> Option.iter (fun v -> view.Value <- v)

    // Events
    if props |> Props.exists PKey.TimeEditor.ValueChanged then
      terminalElement.TrySetEventHandler(PKey.TimeEditor.ValueChanged, view.ValueChanged)

    if props |> Props.exists PKey.TimeEditor.ValueChangedUntyped then
      terminalElement.TrySetEventHandler(PKey.TimeEditor.ValueChangedUntyped, view.ValueChangedUntyped)

    if props |> Props.exists PKey.TimeEditor.ValueChanging then
      terminalElement.TrySetEventHandler(PKey.TimeEditor.ValueChanging, view.ValueChanging)

  static member clearProp(terminalElement: ViewBackedTerminalElement, propertyId: PropertyId) =
    let view = terminalElement.View :?> TimeEditor

    match propertyId.Value with
    | 488 -> view.Format <- Unchecked.defaultof<_>
    | 489 -> view.Value <- Unchecked.defaultof<_>
    | _ -> TextValidateFieldPropHandler.clearProp (terminalElement, propertyId)

type internal TitleViewPropHandler =
  static member applyNativeProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    ViewPropHandler.applyNativeProps (terminalElement, props)

    let view = terminalElement.View :?> TitleView

    // Properties
    props
    |> Props.tryFind PKey.TitleView.Direction
    |> Option.iter (fun v -> view.Direction <- v)

    props
    |> Props.tryFind PKey.TitleView.MeasuredTabLength
    |> Option.iter (fun v -> view.MeasuredTabLength <- v)

    props
    |> Props.tryFind PKey.TitleView.Orientation
    |> Option.iter (fun v -> view.Orientation <- v)

    props
    |> Props.tryFind PKey.TitleView.TabDepth
    |> Option.iter (fun v -> view.TabDepth <- v)

    props
    |> Props.tryFind PKey.TitleView.TabSide
    |> Option.iter (fun v -> view.TabSide <- v)

    props
    |> Props.tryFind PKey.TitleView.Text
    |> Option.iter (fun v -> view.Text <- v)

    // Events
    if props |> Props.exists PKey.TitleView.OrientationChanged then
      terminalElement.TrySetEventHandler(PKey.TitleView.OrientationChanged, view.OrientationChanged)

    if props |> Props.exists PKey.TitleView.OrientationChanging then
      terminalElement.TrySetEventHandler(PKey.TitleView.OrientationChanging, view.OrientationChanging)

  static member clearProp(terminalElement: ViewBackedTerminalElement, propertyId: PropertyId) =
    let view = terminalElement.View :?> TitleView

    match propertyId.Value with
    | 493 -> view.Direction <- Unchecked.defaultof<_>
    | 494 -> view.MeasuredTabLength <- Unchecked.defaultof<_>
    | 495 -> view.Orientation <- Unchecked.defaultof<_>
    | 496 -> view.TabDepth <- Unchecked.defaultof<_>
    | 497 -> view.TabSide <- Unchecked.defaultof<_>
    | 498 -> view.Text <- ""
    | _ -> ViewPropHandler.clearProp (terminalElement, propertyId)

type internal ToolTipHostPropHandler<'TView when 'TView: (new: unit -> 'TView) and 'TView :> Terminal.Gui.ViewBase.View>
  =
  static member applyNativeProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    PopoverImplPropHandler.applyNativeProps (terminalElement, props)

    let view = terminalElement.View :?> ToolTipHost<'TView>

    // Properties
    props
    |> Props.tryFind PKey.ToolTipHost<'TView>.ContentView
    |> Option.iter (fun v -> view.ContentView <- v)

  static member clearProp(terminalElement: ViewBackedTerminalElement, propertyId: PropertyId) =
    let view = terminalElement.View :?> ToolTipHost<'TView>

    match propertyId.Value with
    | 501 -> view.ContentView <- Unchecked.defaultof<_>
    | _ -> PopoverImplPropHandler.clearProp (terminalElement, propertyId)

type internal TreeViewPropHandler<'T when 'T: not struct> =
  static member applyNativeProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    ViewPropHandler.applyNativeProps (terminalElement, props)

    let view = terminalElement.View :?> TreeView<'T>

    // Properties
    props
    |> Props.tryFind PKey.TreeView<'T>.AllowLetterBasedNavigation
    |> Option.iter (fun v -> view.AllowLetterBasedNavigation <- v)

    props
    |> Props.tryFind PKey.TreeView<'T>.AspectGetter
    |> Option.iter (fun v -> view.AspectGetter <- v)

    props
    |> Props.tryFind PKey.TreeView<'T>.CheckboxMode
    |> Option.iter (fun v -> view.CheckboxMode <- v)

    props
    |> Props.tryFind PKey.TreeView<'T>.ColorGetter
    |> Option.iter (fun v -> view.ColorGetter <- v)

    props
    |> Props.tryFind PKey.TreeView<'T>.Filter
    |> Option.iter (fun v -> view.Filter <- v)

    props
    |> Props.tryFind PKey.TreeView<'T>.MaxDepth
    |> Option.iter (fun v -> view.MaxDepth <- v)

    props
    |> Props.tryFind PKey.TreeView<'T>.MultiSelect
    |> Option.iter (fun v -> view.MultiSelect <- v)

    props
    |> Props.tryFind PKey.TreeView<'T>.ScrollOffsetHorizontal
    |> Option.iter (fun v -> view.ScrollOffsetHorizontal <- v)

    props
    |> Props.tryFind PKey.TreeView<'T>.ScrollOffsetVertical
    |> Option.iter (fun v -> view.ScrollOffsetVertical <- v)

    props
    |> Props.tryFind PKey.TreeView<'T>.SelectedObject
    |> Option.iter (fun v -> view.SelectedObject <- v)

    props
    |> Props.tryFind PKey.TreeView<'T>.Style
    |> Option.iter (fun v -> view.Style <- v)

    props
    |> Props.tryFind PKey.TreeView<'T>.TreeBuilder
    |> Option.iter (fun v -> view.TreeBuilder <- v)

    // Events
    if props |> Props.exists PKey.TreeView<'T>.CheckedChanged then
      terminalElement.TrySetEventHandler(PKey.TreeView<'T>.CheckedChanged, view.CheckedChanged)

    if props |> Props.exists PKey.TreeView<'T>.DrawLine then
      terminalElement.TrySetEventHandler(PKey.TreeView<'T>.DrawLine, view.DrawLine)

    if props |> Props.exists PKey.TreeView<'T>.SelectionChanged then
      terminalElement.TrySetEventHandler(PKey.TreeView<'T>.SelectionChanged, view.SelectionChanged)

  static member clearProp(terminalElement: ViewBackedTerminalElement, propertyId: PropertyId) =
    let view = terminalElement.View :?> TreeView<'T>

    match propertyId.Value with
    | 503 -> view.AllowLetterBasedNavigation <- Unchecked.defaultof<_>
    | 504 -> view.AspectGetter <- Unchecked.defaultof<_>
    | 505 -> view.CheckboxMode <- Unchecked.defaultof<_>
    | 506 -> view.ColorGetter <- Unchecked.defaultof<_>
    | 507 -> view.Filter <- Unchecked.defaultof<_>
    | 508 -> view.MaxDepth <- Unchecked.defaultof<_>
    | 509 -> view.MultiSelect <- Unchecked.defaultof<_>
    | 510 -> view.ScrollOffsetHorizontal <- Unchecked.defaultof<_>
    | 511 -> view.ScrollOffsetVertical <- Unchecked.defaultof<_>
    | 512 -> view.SelectedObject <- Unchecked.defaultof<_>
    | 513 -> view.Style <- Unchecked.defaultof<_>
    | 514 -> view.TreeBuilder <- Unchecked.defaultof<_>
    | _ -> ViewPropHandler.clearProp (terminalElement, propertyId)

type internal TreeViewPropHandler =
  static member applyNativeProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    TreeViewPropHandler<Terminal.Gui.Views.ITreeNode>.applyNativeProps (terminalElement, props)

  static member clearProp(terminalElement: ViewBackedTerminalElement, propertyId: PropertyId) =
    match propertyId.Value with
    | _ -> TreeViewPropHandler<Terminal.Gui.Views.ITreeNode>.clearProp (terminalElement, propertyId)

type internal WindowPropHandler =
  static member applyNativeProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    RunnablePropHandler.applyNativeProps (terminalElement, props)

  static member clearProp(terminalElement: ViewBackedTerminalElement, propertyId: PropertyId) =
    match propertyId.Value with
    | _ -> RunnablePropHandler.clearProp (terminalElement, propertyId)

type internal WizardPropHandler =
  static member applyNativeProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    DialogPropHandler.applyNativeProps (terminalElement, props)

    let view = terminalElement.View :?> Wizard

    // Properties
    props
    |> Props.tryFind PKey.Wizard.CurrentStep
    |> Option.iter (fun v -> view.CurrentStep <- v)

    // Events
    if props |> Props.exists PKey.Wizard.MovingBack then
      terminalElement.TrySetEventHandler(PKey.Wizard.MovingBack, view.MovingBack)

    if props |> Props.exists PKey.Wizard.MovingNext then
      terminalElement.TrySetEventHandler(PKey.Wizard.MovingNext, view.MovingNext)

    if props |> Props.exists PKey.Wizard.StepChanged then
      terminalElement.TrySetEventHandler(PKey.Wizard.StepChanged, view.StepChanged)

    if props |> Props.exists PKey.Wizard.StepChanging then
      terminalElement.TrySetEventHandler(PKey.Wizard.StepChanging, view.StepChanging)

  static member clearProp(terminalElement: ViewBackedTerminalElement, propertyId: PropertyId) =
    let view = terminalElement.View :?> Wizard

    match propertyId.Value with
    | 518 -> view.CurrentStep <- Unchecked.defaultof<_>
    | _ -> DialogPropHandler.clearProp (terminalElement, propertyId)

type internal WizardStepPropHandler =
  static member applyNativeProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    ViewPropHandler.applyNativeProps (terminalElement, props)

    let view = terminalElement.View :?> WizardStep

    // Properties
    props
    |> Props.tryFind PKey.WizardStep.BackButtonText
    |> Option.iter (fun v -> view.BackButtonText <- v)

    props
    |> Props.tryFind PKey.WizardStep.HelpText
    |> Option.iter (fun v -> view.HelpText <- v)

    props
    |> Props.tryFind PKey.WizardStep.NextButtonText
    |> Option.iter (fun v -> view.NextButtonText <- v)

  static member clearProp(terminalElement: ViewBackedTerminalElement, propertyId: PropertyId) =
    let view = terminalElement.View :?> WizardStep

    match propertyId.Value with
    | 524 -> view.BackButtonText <- ""
    | 525 -> view.HelpText <- ""
    | 526 -> view.NextButtonText <- ""
    | _ -> ViewPropHandler.clearProp (terminalElement, propertyId)
