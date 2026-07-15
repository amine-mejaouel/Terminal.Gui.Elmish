namespace Terminal.Gui.Elmish

open System
open System.Collections.Generic
open Terminal.Gui.App
open Terminal.Gui.ViewBase
open Terminal.Gui.Views


type internal ViewPropHandler =
  static member setProps(terminalElement: ViewBackedTerminalElement, props: Props) =

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

  static member removeProps(terminalElement: ViewBackedTerminalElement, props: Props) =

    let view = terminalElement.View

    // Properties
    props
    |> Props.tryFind PKey.View.App
    |> Option.iter (fun _ -> view.App <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.View.Arrangement
    |> Option.iter (fun _ -> view.Arrangement <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.View.AssignHotKeys
    |> Option.iter (fun _ -> view.AssignHotKeys <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.View.BorderStyle
    |> Option.iter (fun _ -> view.BorderStyle <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.View.CanFocus
    |> Option.iter (fun _ -> view.CanFocus <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.View.CommandsToBubbleUp
    |> Option.iter (fun _ -> view.CommandsToBubbleUp <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.View.ContentSizeTracksViewport
    |> Option.iter (fun _ -> view.ContentSizeTracksViewport <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.View.Cursor
    |> Option.iter (fun _ -> view.Cursor <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.View.Data
    |> Option.iter (fun _ -> view.Data <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.View.DefaultAcceptView
    |> Option.iter (fun _ -> view.DefaultAcceptView <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.View.Enabled
    |> Option.iter (fun _ -> view.Enabled <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.View.Frame
    |> Option.iter (fun _ -> view.Frame <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.View.HasFocus
    |> Option.iter (fun _ -> view.HasFocus <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.View.Height
    |> Option.iter (fun _ -> view.Height <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.View.HotKey
    |> Option.iter (fun _ -> view.HotKey <- Terminal.Gui.Input.Key.Empty)

    props
    |> Props.tryFind PKey.View.HotKeySpecifier
    |> Option.iter (fun _ -> view.HotKeySpecifier <- Unchecked.defaultof<_>)

    props |> Props.tryFind PKey.View.Id |> Option.iter (fun _ -> view.Id <- "")

    props
    |> Props.tryFind PKey.View.IsInitialized
    |> Option.iter (fun _ -> view.IsInitialized <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.View.MouseHighlightStates
    |> Option.iter (fun _ -> view.MouseHighlightStates <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.View.MouseHoldRepeat
    |> Option.iter (fun _ -> view.MouseHoldRepeat <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.View.MousePositionTracking
    |> Option.iter (fun _ -> view.MousePositionTracking <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.View.PreserveTrailingSpaces
    |> Option.iter (fun _ -> view.PreserveTrailingSpaces <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.View.SchemeName
    |> Option.iter (fun _ -> view.SchemeName <- "")

    props
    |> Props.tryFind PKey.View.ShadowStyle
    |> Option.iter (fun _ -> view.ShadowStyle <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.View.SuperViewRendersLineCanvas
    |> Option.iter (fun _ -> view.SuperViewRendersLineCanvas <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.View.TabStop
    |> Option.iter (fun _ -> view.TabStop <- Unchecked.defaultof<_>)

    props |> Props.tryFind PKey.View.Text |> Option.iter (fun _ -> view.Text <- "")

    props
    |> Props.tryFind PKey.View.TextAlignment
    |> Option.iter (fun _ -> view.TextAlignment <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.View.TextDirection
    |> Option.iter (fun _ -> view.TextDirection <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.View.Title
    |> Option.iter (fun _ -> view.Title <- "")

    props
    |> Props.tryFind PKey.View.UsedHotKeys
    |> Option.iter (fun _ -> view.UsedHotKeys <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.View.ValidatePosDim
    |> Option.iter (fun _ -> view.ValidatePosDim <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.View.VerticalTextAlignment
    |> Option.iter (fun _ -> view.VerticalTextAlignment <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.View.Viewport
    |> Option.iter (fun _ -> view.Viewport <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.View.ViewportSettings
    |> Option.iter (fun _ -> view.ViewportSettings <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.View.Visible
    |> Option.iter (fun _ -> view.Visible <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.View.Width
    |> Option.iter (fun _ -> view.Width <- Unchecked.defaultof<_>)

    // Events
    if props |> Props.exists PKey.View.Accepted then
      terminalElement.TryRemoveEventHandler(PKey.View.Accepted)

    if props |> Props.exists PKey.View.Accepting then
      terminalElement.TryRemoveEventHandler(PKey.View.Accepting)

    if props |> Props.exists PKey.View.Activated then
      terminalElement.TryRemoveEventHandler(PKey.View.Activated)

    if props |> Props.exists PKey.View.Activating then
      terminalElement.TryRemoveEventHandler(PKey.View.Activating)

    if props |> Props.exists PKey.View.AdvancingFocus then
      terminalElement.TryRemoveEventHandler(PKey.View.AdvancingFocus)

    if props |> Props.exists PKey.View.BorderStyleChanged then
      terminalElement.TryRemoveEventHandler(PKey.View.BorderStyleChanged)

    if props |> Props.exists PKey.View.CanFocusChanged then
      terminalElement.TryRemoveEventHandler(PKey.View.CanFocusChanged)

    if props |> Props.exists PKey.View.ClearedViewport then
      terminalElement.TryRemoveEventHandler(PKey.View.ClearedViewport)

    if props |> Props.exists PKey.View.ClearingViewport then
      terminalElement.TryRemoveEventHandler(PKey.View.ClearingViewport)

    if props |> Props.exists PKey.View.CommandNotBound then
      terminalElement.TryRemoveEventHandler(PKey.View.CommandNotBound)

    if props |> Props.exists PKey.View.ContentSizeChanged then
      terminalElement.TryRemoveEventHandler(PKey.View.ContentSizeChanged)

    if props |> Props.exists PKey.View.ContentSizeChanging then
      terminalElement.TryRemoveEventHandler(PKey.View.ContentSizeChanging)

    if props |> Props.exists PKey.View.Disposing then
      terminalElement.TryRemoveEventHandler(PKey.View.Disposing)

    if props |> Props.exists PKey.View.DrawComplete then
      terminalElement.TryRemoveEventHandler(PKey.View.DrawComplete)

    if props |> Props.exists PKey.View.DrawingContent then
      terminalElement.TryRemoveEventHandler(PKey.View.DrawingContent)

    if props |> Props.exists PKey.View.DrawingSubViews then
      terminalElement.TryRemoveEventHandler(PKey.View.DrawingSubViews)

    if props |> Props.exists PKey.View.DrawingText then
      terminalElement.TryRemoveEventHandler(PKey.View.DrawingText)

    if props |> Props.exists PKey.View.DrewText then
      terminalElement.TryRemoveEventHandler(PKey.View.DrewText)

    if props |> Props.exists PKey.View.EnabledChanged then
      terminalElement.TryRemoveEventHandler(PKey.View.EnabledChanged)

    if props |> Props.exists PKey.View.FocusedChanged then
      terminalElement.TryRemoveEventHandler(PKey.View.FocusedChanged)

    if props |> Props.exists PKey.View.FrameChanged then
      terminalElement.TryRemoveEventHandler(PKey.View.FrameChanged)

    if props |> Props.exists PKey.View.GettingAttributeForRole then
      terminalElement.TryRemoveEventHandler(PKey.View.GettingAttributeForRole)

    if props |> Props.exists PKey.View.GettingScheme then
      terminalElement.TryRemoveEventHandler(PKey.View.GettingScheme)

    if props |> Props.exists PKey.View.HandlingHotKey then
      terminalElement.TryRemoveEventHandler(PKey.View.HandlingHotKey)

    if props |> Props.exists PKey.View.HasFocusChanged then
      terminalElement.TryRemoveEventHandler(PKey.View.HasFocusChanged)

    if props |> Props.exists PKey.View.HasFocusChanging then
      terminalElement.TryRemoveEventHandler(PKey.View.HasFocusChanging)

    if props |> Props.exists PKey.View.HeightChanged then
      terminalElement.TryRemoveEventHandler(PKey.View.HeightChanged)

    if props |> Props.exists PKey.View.HeightChanging then
      terminalElement.TryRemoveEventHandler(PKey.View.HeightChanging)

    if props |> Props.exists PKey.View.HotKeyChanged then
      terminalElement.TryRemoveEventHandler(PKey.View.HotKeyChanged)

    if props |> Props.exists PKey.View.HotKeyCommand then
      terminalElement.TryRemoveEventHandler(PKey.View.HotKeyCommand)

    if props |> Props.exists PKey.View.Initialized then
      terminalElement.TryRemoveEventHandler(PKey.View.Initialized)

    if props |> Props.exists PKey.View.KeyDown then
      terminalElement.TryRemoveEventHandler(PKey.View.KeyDown)

    if props |> Props.exists PKey.View.KeyDownNotHandled then
      terminalElement.TryRemoveEventHandler(PKey.View.KeyDownNotHandled)

    if props |> Props.exists PKey.View.KeyUp then
      terminalElement.TryRemoveEventHandler(PKey.View.KeyUp)

    if props |> Props.exists PKey.View.MouseEnter then
      terminalElement.TryRemoveEventHandler(PKey.View.MouseEnter)

    if props |> Props.exists PKey.View.MouseEvent then
      terminalElement.TryRemoveEventHandler(PKey.View.MouseEvent)

    if props |> Props.exists PKey.View.MouseHoldRepeatChanged then
      terminalElement.TryRemoveEventHandler(PKey.View.MouseHoldRepeatChanged)

    if props |> Props.exists PKey.View.MouseHoldRepeatChanging then
      terminalElement.TryRemoveEventHandler(PKey.View.MouseHoldRepeatChanging)

    if props |> Props.exists PKey.View.MouseLeave then
      terminalElement.TryRemoveEventHandler(PKey.View.MouseLeave)

    if props |> Props.exists PKey.View.MouseStateChanged then
      terminalElement.TryRemoveEventHandler(PKey.View.MouseStateChanged)

    if props |> Props.exists PKey.View.Pasted then
      terminalElement.TryRemoveEventHandler(PKey.View.Pasted)

    if props |> Props.exists PKey.View.Pasting then
      terminalElement.TryRemoveEventHandler(PKey.View.Pasting)

    if props |> Props.exists PKey.View.Removed then
      terminalElement.TryRemoveEventHandler(PKey.View.Removed)

    if props |> Props.exists PKey.View.SchemeChanged then
      terminalElement.TryRemoveEventHandler(PKey.View.SchemeChanged)

    if props |> Props.exists PKey.View.SchemeChanging then
      terminalElement.TryRemoveEventHandler(PKey.View.SchemeChanging)

    if props |> Props.exists PKey.View.SchemeNameChanged then
      terminalElement.TryRemoveEventHandler(PKey.View.SchemeNameChanged)

    if props |> Props.exists PKey.View.SchemeNameChanging then
      terminalElement.TryRemoveEventHandler(PKey.View.SchemeNameChanging)

    if props |> Props.exists PKey.View.ShadowStyleChanged then
      terminalElement.TryRemoveEventHandler(PKey.View.ShadowStyleChanged)

    if props |> Props.exists PKey.View.SubViewAdded then
      terminalElement.TryRemoveEventHandler(PKey.View.SubViewAdded)

    if props |> Props.exists PKey.View.SubViewAdding then
      terminalElement.TryRemoveEventHandler(PKey.View.SubViewAdding)

    if props |> Props.exists PKey.View.SubViewLayout then
      terminalElement.TryRemoveEventHandler(PKey.View.SubViewLayout)

    if props |> Props.exists PKey.View.SubViewRemoved then
      terminalElement.TryRemoveEventHandler(PKey.View.SubViewRemoved)

    if props |> Props.exists PKey.View.SubViewsLaidOut then
      terminalElement.TryRemoveEventHandler(PKey.View.SubViewsLaidOut)

    if props |> Props.exists PKey.View.SuperViewChanged then
      terminalElement.TryRemoveEventHandler(PKey.View.SuperViewChanged)

    if props |> Props.exists PKey.View.SuperViewChanging then
      terminalElement.TryRemoveEventHandler(PKey.View.SuperViewChanging)

    if props |> Props.exists PKey.View.TextChanged then
      terminalElement.TryRemoveEventHandler(PKey.View.TextChanged)

    if props |> Props.exists PKey.View.TitleChanged then
      terminalElement.TryRemoveEventHandler(PKey.View.TitleChanged)

    if props |> Props.exists PKey.View.TitleChanging then
      terminalElement.TryRemoveEventHandler(PKey.View.TitleChanging)

    if props |> Props.exists PKey.View.ViewportChanged then
      terminalElement.TryRemoveEventHandler(PKey.View.ViewportChanged)

    if props |> Props.exists PKey.View.VisibleChanged then
      terminalElement.TryRemoveEventHandler(PKey.View.VisibleChanged)

    if props |> Props.exists PKey.View.VisibleChanging then
      terminalElement.TryRemoveEventHandler(PKey.View.VisibleChanging)

    if props |> Props.exists PKey.View.WidthChanged then
      terminalElement.TryRemoveEventHandler(PKey.View.WidthChanged)

    if props |> Props.exists PKey.View.WidthChanging then
      terminalElement.TryRemoveEventHandler(PKey.View.WidthChanging)

type internal AdornmentViewPropHandler =
  static member setProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    ViewPropHandler.setProps (terminalElement, props)

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

  static member removeProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    ViewPropHandler.removeProps (terminalElement, props)

    let view = terminalElement.View :?> AdornmentView

    // Properties
    props
    |> Props.tryFind PKey.AdornmentView.Adornment
    |> Option.iter (fun _ -> view.Adornment <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.AdornmentView.Diagnostics
    |> Option.iter (fun _ -> view.Diagnostics <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.AdornmentView.SuperViewRendersLineCanvas
    |> Option.iter (fun _ -> view.SuperViewRendersLineCanvas <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.AdornmentView.Viewport
    |> Option.iter (fun _ -> view.Viewport <- Unchecked.defaultof<_>)


type internal AttributePickerPropHandler =
  static member setProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    ViewPropHandler.setProps (terminalElement, props)

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

  static member removeProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    ViewPropHandler.removeProps (terminalElement, props)

    let view = terminalElement.View :?> AttributePicker

    // Properties
    props
    |> Props.tryFind PKey.AttributePicker.SampleText
    |> Option.iter (fun _ -> view.SampleText <- "")

    props
    |> Props.tryFind PKey.AttributePicker.Value
    |> Option.iter (fun _ -> view.Value <- Unchecked.defaultof<_>)

    // Events
    if props |> Props.exists PKey.AttributePicker.ValueChanged then
      terminalElement.TryRemoveEventHandler(PKey.AttributePicker.ValueChanged)

    if props |> Props.exists PKey.AttributePicker.ValueChangedUntyped then
      terminalElement.TryRemoveEventHandler(PKey.AttributePicker.ValueChangedUntyped)

    if props |> Props.exists PKey.AttributePicker.ValueChanging then
      terminalElement.TryRemoveEventHandler(PKey.AttributePicker.ValueChanging)

type internal BarPropHandler =
  static member setProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    ViewPropHandler.setProps (terminalElement, props)

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

  static member removeProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    ViewPropHandler.removeProps (terminalElement, props)

    let view = terminalElement.View :?> Bar

    // Properties
    props
    |> Props.tryFind PKey.Bar.AlignmentModes
    |> Option.iter (fun _ -> view.AlignmentModes <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.Bar.Orientation
    |> Option.iter (fun _ -> view.Orientation <- Unchecked.defaultof<_>)

    // Events
    if props |> Props.exists PKey.Bar.OrientationChanged then
      terminalElement.TryRemoveEventHandler(PKey.Bar.OrientationChanged)

    if props |> Props.exists PKey.Bar.OrientationChanging then
      terminalElement.TryRemoveEventHandler(PKey.Bar.OrientationChanging)

type internal BorderViewPropHandler =
  static member setProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    AdornmentViewPropHandler.setProps (terminalElement, props)

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

  static member removeProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    AdornmentViewPropHandler.removeProps (terminalElement, props)

    let view = terminalElement.View :?> BorderView

    // Properties
    props
    |> Props.tryFind PKey.BorderView.TabLength
    |> Option.iter (fun _ -> view.TabLength <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.BorderView.TabOffset
    |> Option.iter (fun _ -> view.TabOffset <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.BorderView.TabSide
    |> Option.iter (fun _ -> view.TabSide <- Unchecked.defaultof<_>)


type internal ButtonPropHandler =
  static member setProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    ViewPropHandler.setProps (terminalElement, props)

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

  static member removeProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    ViewPropHandler.removeProps (terminalElement, props)

    let view = terminalElement.View :?> Button

    // Properties
    props
    |> Props.tryFind PKey.Button.HotKeySpecifier
    |> Option.iter (fun _ -> view.HotKeySpecifier <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.Button.IsDefault
    |> Option.iter (fun _ -> view.IsDefault <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.Button.NoDecorations
    |> Option.iter (fun _ -> view.NoDecorations <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.Button.NoPadding
    |> Option.iter (fun _ -> view.NoPadding <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.Button.Text
    |> Option.iter (fun _ -> view.Text <- "")

    // Events
    if props |> Props.exists PKey.Button.InitializingShadowStyle then
      terminalElement.TryRemoveEventHandler(PKey.Button.InitializingShadowStyle)

type internal CharMapPropHandler =
  static member setProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    ViewPropHandler.setProps (terminalElement, props)

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

  static member removeProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    ViewPropHandler.removeProps (terminalElement, props)

    let view = terminalElement.View :?> CharMap

    // Properties
    props
    |> Props.tryFind PKey.CharMap.SelectedCodePoint
    |> Option.iter (fun _ -> view.SelectedCodePoint <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.CharMap.ShowGlyphWidths
    |> Option.iter (fun _ -> view.ShowGlyphWidths <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.CharMap.ShowUnicodeCategory
    |> Option.iter (fun _ -> view.ShowUnicodeCategory <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.CharMap.StartCodePoint
    |> Option.iter (fun _ -> view.StartCodePoint <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.CharMap.Value
    |> Option.iter (fun _ -> view.Value <- Unchecked.defaultof<_>)

    // Events
    if props |> Props.exists PKey.CharMap.ValueChanged then
      terminalElement.TryRemoveEventHandler(PKey.CharMap.ValueChanged)

    if props |> Props.exists PKey.CharMap.ValueChangedUntyped then
      terminalElement.TryRemoveEventHandler(PKey.CharMap.ValueChangedUntyped)

    if props |> Props.exists PKey.CharMap.ValueChanging then
      terminalElement.TryRemoveEventHandler(PKey.CharMap.ValueChanging)

type internal CheckBoxPropHandler =
  static member setProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    ViewPropHandler.setProps (terminalElement, props)

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

  static member removeProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    ViewPropHandler.removeProps (terminalElement, props)

    let view = terminalElement.View :?> CheckBox

    // Properties
    props
    |> Props.tryFind PKey.CheckBox.AllowCheckStateNone
    |> Option.iter (fun _ -> view.AllowCheckStateNone <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.CheckBox.HotKeySpecifier
    |> Option.iter (fun _ -> view.HotKeySpecifier <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.CheckBox.RadioStyle
    |> Option.iter (fun _ -> view.RadioStyle <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.CheckBox.Text
    |> Option.iter (fun _ -> view.Text <- "")

    props
    |> Props.tryFind PKey.CheckBox.Value
    |> Option.iter (fun _ -> view.Value <- Unchecked.defaultof<_>)

    // Events
    if props |> Props.exists PKey.CheckBox.ValueChanged then
      terminalElement.TryRemoveEventHandler(PKey.CheckBox.ValueChanged)

    if props |> Props.exists PKey.CheckBox.ValueChangedUntyped then
      terminalElement.TryRemoveEventHandler(PKey.CheckBox.ValueChangedUntyped)

    if props |> Props.exists PKey.CheckBox.ValueChanging then
      terminalElement.TryRemoveEventHandler(PKey.CheckBox.ValueChanging)

type internal CodePropHandler =
  static member setProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    ViewPropHandler.setProps (terminalElement, props)

    let view = terminalElement.View :?> Code

    // Properties
    props
    |> Props.tryFind PKey.Code.Language
    |> Option.iter (fun v -> view.Language <- v)

    props
    |> Props.tryFind PKey.Code.SyntaxHighlighter
    |> Option.iter (fun v -> view.SyntaxHighlighter <- v)

    props |> Props.tryFind PKey.Code.Text |> Option.iter (fun v -> view.Text <- v)

  static member removeProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    ViewPropHandler.removeProps (terminalElement, props)

    let view = terminalElement.View :?> Code

    // Properties
    props
    |> Props.tryFind PKey.Code.Language
    |> Option.iter (fun _ -> view.Language <- "")

    props
    |> Props.tryFind PKey.Code.SyntaxHighlighter
    |> Option.iter (fun _ -> view.SyntaxHighlighter <- Unchecked.defaultof<_>)

    props |> Props.tryFind PKey.Code.Text |> Option.iter (fun _ -> view.Text <- "")


type internal ColorPickerPropHandler =
  static member setProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    ViewPropHandler.setProps (terminalElement, props)

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

  static member removeProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    ViewPropHandler.removeProps (terminalElement, props)

    let view = terminalElement.View :?> ColorPicker

    // Properties
    props
    |> Props.tryFind PKey.ColorPicker.SelectedColor
    |> Option.iter (fun _ -> view.SelectedColor <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.ColorPicker.Style
    |> Option.iter (fun _ -> view.Style <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.ColorPicker.Text
    |> Option.iter (fun _ -> view.Text <- "")

    props
    |> Props.tryFind PKey.ColorPicker.Value
    |> Option.iter (fun _ -> view.Value <- Unchecked.defaultof<_>)

    // Events
    if props |> Props.exists PKey.ColorPicker.ValueChanged then
      terminalElement.TryRemoveEventHandler(PKey.ColorPicker.ValueChanged)

    if props |> Props.exists PKey.ColorPicker.ValueChangedUntyped then
      terminalElement.TryRemoveEventHandler(PKey.ColorPicker.ValueChangedUntyped)

    if props |> Props.exists PKey.ColorPicker.ValueChanging then
      terminalElement.TryRemoveEventHandler(PKey.ColorPicker.ValueChanging)

type internal ColorPicker16PropHandler =
  static member setProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    ViewPropHandler.setProps (terminalElement, props)

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

  static member removeProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    ViewPropHandler.removeProps (terminalElement, props)

    let view = terminalElement.View :?> ColorPicker16

    // Properties
    props
    |> Props.tryFind PKey.ColorPicker16.BoxHeight
    |> Option.iter (fun _ -> view.BoxHeight <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.ColorPicker16.BoxWidth
    |> Option.iter (fun _ -> view.BoxWidth <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.ColorPicker16.Caret
    |> Option.iter (fun _ -> view.Caret <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.ColorPicker16.SelectedColor
    |> Option.iter (fun _ -> view.SelectedColor <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.ColorPicker16.Value
    |> Option.iter (fun _ -> view.Value <- Unchecked.defaultof<_>)

    // Events
    if props |> Props.exists PKey.ColorPicker16.ValueChanged then
      terminalElement.TryRemoveEventHandler(PKey.ColorPicker16.ValueChanged)

    if props |> Props.exists PKey.ColorPicker16.ValueChangedUntyped then
      terminalElement.TryRemoveEventHandler(PKey.ColorPicker16.ValueChangedUntyped)

    if props |> Props.exists PKey.ColorPicker16.ValueChanging then
      terminalElement.TryRemoveEventHandler(PKey.ColorPicker16.ValueChanging)

type internal DatePickerPropHandler =
  static member setProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    ViewPropHandler.setProps (terminalElement, props)

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

  static member removeProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    ViewPropHandler.removeProps (terminalElement, props)

    let view = terminalElement.View :?> DatePicker

    // Properties
    props
    |> Props.tryFind PKey.DatePicker.Culture
    |> Option.iter (fun _ -> view.Culture <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.DatePicker.Text
    |> Option.iter (fun _ -> view.Text <- "")

    props
    |> Props.tryFind PKey.DatePicker.Value
    |> Option.iter (fun _ -> view.Value <- Unchecked.defaultof<_>)

    // Events
    if props |> Props.exists PKey.DatePicker.ValueChanged then
      terminalElement.TryRemoveEventHandler(PKey.DatePicker.ValueChanged)

    if props |> Props.exists PKey.DatePicker.ValueChangedUntyped then
      terminalElement.TryRemoveEventHandler(PKey.DatePicker.ValueChangedUntyped)

    if props |> Props.exists PKey.DatePicker.ValueChanging then
      terminalElement.TryRemoveEventHandler(PKey.DatePicker.ValueChanging)

type internal FrameViewPropHandler =
  static member setProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    ViewPropHandler.setProps (terminalElement, props)

  static member removeProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    ViewPropHandler.removeProps (terminalElement, props)


type internal GraphViewPropHandler =
  static member setProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    ViewPropHandler.setProps (terminalElement, props)

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

  static member removeProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    ViewPropHandler.removeProps (terminalElement, props)

    let view = terminalElement.View :?> GraphView

    // Properties
    props
    |> Props.tryFind PKey.GraphView.AxisX
    |> Option.iter (fun _ -> view.AxisX <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.GraphView.AxisY
    |> Option.iter (fun _ -> view.AxisY <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.GraphView.CellSize
    |> Option.iter (fun _ -> view.CellSize <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.GraphView.GraphColor
    |> Option.iter (fun _ -> view.GraphColor <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.GraphView.MarginBottom
    |> Option.iter (fun _ -> view.MarginBottom <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.GraphView.MarginLeft
    |> Option.iter (fun _ -> view.MarginLeft <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.GraphView.ScrollOffset
    |> Option.iter (fun _ -> view.ScrollOffset <- Unchecked.defaultof<_>)


type internal HexViewPropHandler =
  static member setProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    ViewPropHandler.setProps (terminalElement, props)

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

  static member removeProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    ViewPropHandler.removeProps (terminalElement, props)

    let view = terminalElement.View :?> HexView

    // Properties
    props
    |> Props.tryFind PKey.HexView.Address
    |> Option.iter (fun _ -> view.Address <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.HexView.AddressWidth
    |> Option.iter (fun _ -> view.AddressWidth <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.HexView.BytesPerLine
    |> Option.iter (fun _ -> view.BytesPerLine <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.HexView.ReadOnly
    |> Option.iter (fun _ -> view.ReadOnly <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.HexView.Source
    |> Option.iter (fun _ -> view.Source <- Unchecked.defaultof<_>)

    // Events
    if props |> Props.exists PKey.HexView.Edited then
      terminalElement.TryRemoveEventHandler(PKey.HexView.Edited)

    if props |> Props.exists PKey.HexView.PositionChanged then
      terminalElement.TryRemoveEventHandler(PKey.HexView.PositionChanged)

type internal ImageViewPropHandler =
  static member setProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    ViewPropHandler.setProps (terminalElement, props)

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

  static member removeProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    ViewPropHandler.removeProps (terminalElement, props)

    let view = terminalElement.View :?> ImageView

    // Properties
    props
    |> Props.tryFind PKey.ImageView.AllowSixelUpscaling
    |> Option.iter (fun _ -> view.AllowSixelUpscaling <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.ImageView.Image
    |> Option.iter (fun _ -> view.Image <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.ImageView.MaxSixelPaletteColors
    |> Option.iter (fun _ -> view.MaxSixelPaletteColors <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.ImageView.SixelEncoder
    |> Option.iter (fun _ -> view.SixelEncoder <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.ImageView.UseBackgroundRendering
    |> Option.iter (fun _ -> view.UseBackgroundRendering <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.ImageView.UseRasterGraphics
    |> Option.iter (fun _ -> view.UseRasterGraphics <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.ImageView.UseSixel
    |> Option.iter (fun _ -> view.UseSixel <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.ImageView.ZoomLevel
    |> Option.iter (fun _ -> view.ZoomLevel <- Unchecked.defaultof<_>)

    // Events
    if props |> Props.exists PKey.ImageView.ZoomLevelChanged then
      terminalElement.TryRemoveEventHandler(PKey.ImageView.ZoomLevelChanged)

type internal LabelPropHandler =
  static member setProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    ViewPropHandler.setProps (terminalElement, props)

    let view = terminalElement.View :?> Label

    // Properties
    props
    |> Props.tryFind PKey.Label.HotKeySpecifier
    |> Option.iter (fun v -> view.HotKeySpecifier <- v)

    props |> Props.tryFind PKey.Label.Text |> Option.iter (fun v -> view.Text <- v)

  static member removeProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    ViewPropHandler.removeProps (terminalElement, props)

    let view = terminalElement.View :?> Label

    // Properties
    props
    |> Props.tryFind PKey.Label.HotKeySpecifier
    |> Option.iter (fun _ -> view.HotKeySpecifier <- Unchecked.defaultof<_>)

    props |> Props.tryFind PKey.Label.Text |> Option.iter (fun _ -> view.Text <- "")


type internal LegendAnnotationPropHandler =
  static member setProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    ViewPropHandler.setProps (terminalElement, props)

  static member removeProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    ViewPropHandler.removeProps (terminalElement, props)


type internal LinePropHandler =
  static member setProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    ViewPropHandler.setProps (terminalElement, props)

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

  static member removeProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    ViewPropHandler.removeProps (terminalElement, props)

    let view = terminalElement.View :?> Line

    // Properties
    props
    |> Props.tryFind PKey.Line.Length
    |> Option.iter (fun _ -> view.Length <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.Line.LineAttribute
    |> Option.iter (fun _ -> view.LineAttribute <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.Line.Orientation
    |> Option.iter (fun _ -> view.Orientation <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.Line.Style
    |> Option.iter (fun _ -> view.Style <- Unchecked.defaultof<_>)

    // Events
    if props |> Props.exists PKey.Line.OrientationChanged then
      terminalElement.TryRemoveEventHandler(PKey.Line.OrientationChanged)

    if props |> Props.exists PKey.Line.OrientationChanging then
      terminalElement.TryRemoveEventHandler(PKey.Line.OrientationChanging)

type internal LinearRangeViewBasePropHandler<'TOption, 'TValue> =
  static member setProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    ViewPropHandler.setProps (terminalElement, props)

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

  static member removeProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    ViewPropHandler.removeProps (terminalElement, props)

    let view = terminalElement.View :?> LinearRangeViewBase<'TOption, 'TValue>

    // Properties
    props
    |> Props.tryFind PKey.LinearRangeViewBase<'TOption, 'TValue>.AllowEmpty
    |> Option.iter (fun _ -> view.AllowEmpty <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.LinearRangeViewBase<'TOption, 'TValue>.FocusedOption
    |> Option.iter (fun _ -> view.FocusedOption <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.LinearRangeViewBase<'TOption, 'TValue>.LegendsOrientation
    |> Option.iter (fun _ -> view.LegendsOrientation <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.LinearRangeViewBase<'TOption, 'TValue>.MinimumInnerSpacing
    |> Option.iter (fun _ -> view.MinimumInnerSpacing <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.LinearRangeViewBase<'TOption, 'TValue>.Options
    |> Option.iter (fun _ -> view.Options <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.LinearRangeViewBase<'TOption, 'TValue>.Orientation
    |> Option.iter (fun _ -> view.Orientation <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.LinearRangeViewBase<'TOption, 'TValue>.ShowEndSpacing
    |> Option.iter (fun _ -> view.ShowEndSpacing <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.LinearRangeViewBase<'TOption, 'TValue>.ShowLegends
    |> Option.iter (fun _ -> view.ShowLegends <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.LinearRangeViewBase<'TOption, 'TValue>.Style
    |> Option.iter (fun _ -> view.Style <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.LinearRangeViewBase<'TOption, 'TValue>.Text
    |> Option.iter (fun _ -> view.Text <- "")

    props
    |> Props.tryFind PKey.LinearRangeViewBase<'TOption, 'TValue>.UseMinimumSize
    |> Option.iter (fun _ -> view.UseMinimumSize <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.LinearRangeViewBase<'TOption, 'TValue>.Value
    |> Option.iter (fun _ -> view.Value <- Unchecked.defaultof<_>)

    // Events
    if
      props
      |> Props.exists PKey.LinearRangeViewBase<'TOption, 'TValue>.LegendsOrientationChanged
    then
      terminalElement.TryRemoveEventHandler(PKey.LinearRangeViewBase<'TOption, 'TValue>.LegendsOrientationChanged)

    if
      props
      |> Props.exists PKey.LinearRangeViewBase<'TOption, 'TValue>.LegendsOrientationChanging
    then
      terminalElement.TryRemoveEventHandler(PKey.LinearRangeViewBase<'TOption, 'TValue>.LegendsOrientationChanging)

    if
      props
      |> Props.exists PKey.LinearRangeViewBase<'TOption, 'TValue>.MinimumInnerSpacingChanged
    then
      terminalElement.TryRemoveEventHandler(PKey.LinearRangeViewBase<'TOption, 'TValue>.MinimumInnerSpacingChanged)

    if
      props
      |> Props.exists PKey.LinearRangeViewBase<'TOption, 'TValue>.MinimumInnerSpacingChanging
    then
      terminalElement.TryRemoveEventHandler(PKey.LinearRangeViewBase<'TOption, 'TValue>.MinimumInnerSpacingChanging)

    if props |> Props.exists PKey.LinearRangeViewBase<'TOption, 'TValue>.OptionFocused then
      terminalElement.TryRemoveEventHandler(PKey.LinearRangeViewBase<'TOption, 'TValue>.OptionFocused)

    if
      props
      |> Props.exists PKey.LinearRangeViewBase<'TOption, 'TValue>.OrientationChanged
    then
      terminalElement.TryRemoveEventHandler(PKey.LinearRangeViewBase<'TOption, 'TValue>.OrientationChanged)

    if
      props
      |> Props.exists PKey.LinearRangeViewBase<'TOption, 'TValue>.OrientationChanging
    then
      terminalElement.TryRemoveEventHandler(PKey.LinearRangeViewBase<'TOption, 'TValue>.OrientationChanging)

    if
      props
      |> Props.exists PKey.LinearRangeViewBase<'TOption, 'TValue>.ShowEndSpacingChanged
    then
      terminalElement.TryRemoveEventHandler(PKey.LinearRangeViewBase<'TOption, 'TValue>.ShowEndSpacingChanged)

    if
      props
      |> Props.exists PKey.LinearRangeViewBase<'TOption, 'TValue>.ShowEndSpacingChanging
    then
      terminalElement.TryRemoveEventHandler(PKey.LinearRangeViewBase<'TOption, 'TValue>.ShowEndSpacingChanging)

    if
      props
      |> Props.exists PKey.LinearRangeViewBase<'TOption, 'TValue>.ShowLegendsChanged
    then
      terminalElement.TryRemoveEventHandler(PKey.LinearRangeViewBase<'TOption, 'TValue>.ShowLegendsChanged)

    if
      props
      |> Props.exists PKey.LinearRangeViewBase<'TOption, 'TValue>.ShowLegendsChanging
    then
      terminalElement.TryRemoveEventHandler(PKey.LinearRangeViewBase<'TOption, 'TValue>.ShowLegendsChanging)

    if
      props
      |> Props.exists PKey.LinearRangeViewBase<'TOption, 'TValue>.UseMinimumSizeChanged
    then
      terminalElement.TryRemoveEventHandler(PKey.LinearRangeViewBase<'TOption, 'TValue>.UseMinimumSizeChanged)

    if
      props
      |> Props.exists PKey.LinearRangeViewBase<'TOption, 'TValue>.UseMinimumSizeChanging
    then
      terminalElement.TryRemoveEventHandler(PKey.LinearRangeViewBase<'TOption, 'TValue>.UseMinimumSizeChanging)

    if props |> Props.exists PKey.LinearRangeViewBase<'TOption, 'TValue>.ValueChanged then
      terminalElement.TryRemoveEventHandler(PKey.LinearRangeViewBase<'TOption, 'TValue>.ValueChanged)

    if
      props
      |> Props.exists PKey.LinearRangeViewBase<'TOption, 'TValue>.ValueChangedUntyped
    then
      terminalElement.TryRemoveEventHandler(PKey.LinearRangeViewBase<'TOption, 'TValue>.ValueChangedUntyped)

    if props |> Props.exists PKey.LinearRangeViewBase<'TOption, 'TValue>.ValueChanging then
      terminalElement.TryRemoveEventHandler(PKey.LinearRangeViewBase<'TOption, 'TValue>.ValueChanging)

type internal LinearMultiSelectorPropHandler<'T> =
  static member setProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    LinearRangeViewBasePropHandler<'T, IReadOnlyList<'T>>.setProps (terminalElement, props)

    let view = terminalElement.View :?> LinearMultiSelector<'T>

    // Properties
    props
    |> Props.tryFind PKey.LinearMultiSelector<'T>.Value
    |> Option.iter (fun v -> view.Value <- v)

  static member removeProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    LinearRangeViewBasePropHandler<'T, IReadOnlyList<'T>>.removeProps (terminalElement, props)

    let view = terminalElement.View :?> LinearMultiSelector<'T>

    // Properties
    props
    |> Props.tryFind PKey.LinearMultiSelector<'T>.Value
    |> Option.iter (fun _ -> view.Value <- Unchecked.defaultof<_>)


type internal LinearMultiSelectorPropHandler =
  static member setProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    LinearMultiSelectorPropHandler<string>.setProps (terminalElement, props)

  static member removeProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    LinearMultiSelectorPropHandler<string>.removeProps (terminalElement, props)


type internal LinearRangePropHandler<'T> =
  static member setProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    LinearRangeViewBasePropHandler<'T, LinearRangeSpan<'T>>.setProps (terminalElement, props)

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

  static member removeProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    LinearRangeViewBasePropHandler<'T, LinearRangeSpan<'T>>.removeProps (terminalElement, props)

    let view = terminalElement.View :?> LinearRange<'T>

    // Properties
    props
    |> Props.tryFind PKey.LinearRange<'T>.RangeAllowSingle
    |> Option.iter (fun _ -> view.RangeAllowSingle <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.LinearRange<'T>.RangeKind
    |> Option.iter (fun _ -> view.RangeKind <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.LinearRange<'T>.Value
    |> Option.iter (fun _ -> view.Value <- Unchecked.defaultof<_>)


type internal LinearRangePropHandler =
  static member setProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    LinearRangePropHandler<string>.setProps (terminalElement, props)

  static member removeProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    LinearRangePropHandler<string>.removeProps (terminalElement, props)


type internal LinearSelectorPropHandler<'T> =
  static member setProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    LinearRangeViewBasePropHandler<'T, 'T>.setProps (terminalElement, props)

    let view = terminalElement.View :?> LinearSelector<'T>

    // Properties
    props
    |> Props.tryFind PKey.LinearSelector<'T>.SelectedIndex
    |> Option.iter (fun v -> view.SelectedIndex <- v)

    props
    |> Props.tryFind PKey.LinearSelector<'T>.Value
    |> Option.iter (fun v -> view.Value <- v)

  static member removeProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    LinearRangeViewBasePropHandler<'T, 'T>.removeProps (terminalElement, props)

    let view = terminalElement.View :?> LinearSelector<'T>

    // Properties
    props
    |> Props.tryFind PKey.LinearSelector<'T>.SelectedIndex
    |> Option.iter (fun _ -> view.SelectedIndex <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.LinearSelector<'T>.Value
    |> Option.iter (fun _ -> view.Value <- Unchecked.defaultof<_>)


type internal LinearSelectorPropHandler =
  static member setProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    LinearSelectorPropHandler<string>.setProps (terminalElement, props)

  static member removeProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    LinearSelectorPropHandler<string>.removeProps (terminalElement, props)


type internal LinkPropHandler =
  static member setProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    ViewPropHandler.setProps (terminalElement, props)

    let view = terminalElement.View :?> Link

    // Properties
    props |> Props.tryFind PKey.Link.Url |> Option.iter (fun v -> view.Url <- v)

    // Events
    if props |> Props.exists PKey.Link.UrlChanged then
      terminalElement.TrySetEventHandler(PKey.Link.UrlChanged, view.UrlChanged)

    if props |> Props.exists PKey.Link.UrlChanging then
      terminalElement.TrySetEventHandler(PKey.Link.UrlChanging, view.UrlChanging)

  static member removeProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    ViewPropHandler.removeProps (terminalElement, props)

    let view = terminalElement.View :?> Link

    // Properties
    props |> Props.tryFind PKey.Link.Url |> Option.iter (fun _ -> view.Url <- "")

    // Events
    if props |> Props.exists PKey.Link.UrlChanged then
      terminalElement.TryRemoveEventHandler(PKey.Link.UrlChanged)

    if props |> Props.exists PKey.Link.UrlChanging then
      terminalElement.TryRemoveEventHandler(PKey.Link.UrlChanging)

type internal ListViewPropHandler =
  static member setProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    ViewPropHandler.setProps (terminalElement, props)

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

  static member removeProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    ViewPropHandler.removeProps (terminalElement, props)

    let view = terminalElement.View :?> ListView

    // Properties
    props
    |> Props.tryFind PKey.ListView.KeystrokeNavigator
    |> Option.iter (fun _ -> view.KeystrokeNavigator <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.ListView.MarkMultiple
    |> Option.iter (fun _ -> view.MarkMultiple <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.ListView.SelectedItem
    |> Option.iter (fun _ -> view.SelectedItem <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.ListView.ShowMarks
    |> Option.iter (fun _ -> view.ShowMarks <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.ListView.Source
    |> Option.iter (fun _ -> view.Source <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.ListView.Value
    |> Option.iter (fun _ -> view.Value <- Unchecked.defaultof<_>)

    // Events
    if props |> Props.exists PKey.ListView.CollectionChanged then
      terminalElement.TryRemoveEventHandler(PKey.ListView.CollectionChanged)

    if props |> Props.exists PKey.ListView.RowRender then
      terminalElement.TryRemoveEventHandler(PKey.ListView.RowRender)

    if props |> Props.exists PKey.ListView.SourceChanged then
      terminalElement.TryRemoveEventHandler(PKey.ListView.SourceChanged)

    if props |> Props.exists PKey.ListView.ValueChanged then
      terminalElement.TryRemoveEventHandler(PKey.ListView.ValueChanged)

    if props |> Props.exists PKey.ListView.ValueChangedUntyped then
      terminalElement.TryRemoveEventHandler(PKey.ListView.ValueChangedUntyped)

    if props |> Props.exists PKey.ListView.ValueChanging then
      terminalElement.TryRemoveEventHandler(PKey.ListView.ValueChanging)

type internal ListViewPropHandler<'T> =
  static member setProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    ListViewPropHandler.setProps (terminalElement, props)

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

  static member removeProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    ListViewPropHandler.removeProps (terminalElement, props)

    let view = terminalElement.View :?> ListView<'T>

    // Properties
    props
    |> Props.tryFind PKey.ListView'<'T>.Index
    |> Option.iter (fun _ -> view.Index <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.ListView'<'T>.SelectedItem
    |> Option.iter (fun _ -> view.SelectedItem <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.ListView'<'T>.Value
    |> Option.iter (fun _ -> view.Value <- Unchecked.defaultof<_>)

    // Events
    if props |> Props.exists PKey.ListView'<'T>.ValueChanged then
      terminalElement.TryRemoveEventHandler(PKey.ListView'<'T>.ValueChanged)

    if props |> Props.exists PKey.ListView'<'T>.ValueChangedUntyped then
      terminalElement.TryRemoveEventHandler(PKey.ListView'<'T>.ValueChangedUntyped)

    if props |> Props.exists PKey.ListView'<'T>.ValueChanging then
      terminalElement.TryRemoveEventHandler(PKey.ListView'<'T>.ValueChanging)

type internal MarginViewPropHandler =
  static member setProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    AdornmentViewPropHandler.setProps (terminalElement, props)

    let view = terminalElement.View :?> MarginView

    // Properties
    props
    |> Props.tryFind PKey.MarginView.ShadowSize
    |> Option.iter (fun v -> view.ShadowSize <- v)

    props
    |> Props.tryFind PKey.MarginView.ShadowStyle
    |> Option.iter (fun v -> view.ShadowStyle <- v)

  static member removeProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    AdornmentViewPropHandler.removeProps (terminalElement, props)

    let view = terminalElement.View :?> MarginView

    // Properties
    props
    |> Props.tryFind PKey.MarginView.ShadowSize
    |> Option.iter (fun _ -> view.ShadowSize <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.MarginView.ShadowStyle
    |> Option.iter (fun _ -> view.ShadowStyle <- Unchecked.defaultof<_>)


type internal MarkdownPropHandler =
  static member setProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    ViewPropHandler.setProps (terminalElement, props)

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

  static member removeProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    ViewPropHandler.removeProps (terminalElement, props)

    let view = terminalElement.View :?> Markdown

    // Properties
    props
    |> Props.tryFind PKey.Markdown.EnableSixelImages
    |> Option.iter (fun _ -> view.EnableSixelImages <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.Markdown.HotKeySpecifier
    |> Option.iter (fun _ -> view.HotKeySpecifier <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.Markdown.ImageLoader
    |> Option.iter (fun _ -> view.ImageLoader <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.Markdown.MarkdownPipeline
    |> Option.iter (fun _ -> view.MarkdownPipeline <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.Markdown.ShowCopyButtons
    |> Option.iter (fun _ -> view.ShowCopyButtons <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.Markdown.ShowHeadingPrefix
    |> Option.iter (fun _ -> view.ShowHeadingPrefix <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.Markdown.SyntaxHighlighter
    |> Option.iter (fun _ -> view.SyntaxHighlighter <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.Markdown.Text
    |> Option.iter (fun _ -> view.Text <- "")

    props
    |> Props.tryFind PKey.Markdown.UseThemeBackground
    |> Option.iter (fun _ -> view.UseThemeBackground <- Unchecked.defaultof<_>)

    // Events
    if props |> Props.exists PKey.Markdown.LinkClicked then
      terminalElement.TryRemoveEventHandler(PKey.Markdown.LinkClicked)

    if props |> Props.exists PKey.Markdown.MarkdownChanged then
      terminalElement.TryRemoveEventHandler(PKey.Markdown.MarkdownChanged)

type internal MarkdownCodeBlockPropHandler =
  static member setProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    ViewPropHandler.setProps (terminalElement, props)

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

  static member removeProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    ViewPropHandler.removeProps (terminalElement, props)

    let view = terminalElement.View :?> MarkdownCodeBlock

    // Properties
    props
    |> Props.tryFind PKey.MarkdownCodeBlock.CodeLines
    |> Option.iter (fun _ -> view.CodeLines <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.MarkdownCodeBlock.Language
    |> Option.iter (fun _ -> view.Language <- "")

    props
    |> Props.tryFind PKey.MarkdownCodeBlock.ShowCopyButton
    |> Option.iter (fun _ -> view.ShowCopyButton <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.MarkdownCodeBlock.SyntaxHighlighter
    |> Option.iter (fun _ -> view.SyntaxHighlighter <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.MarkdownCodeBlock.Text
    |> Option.iter (fun _ -> view.Text <- "")

    props
    |> Props.tryFind PKey.MarkdownCodeBlock.ThemeBackground
    |> Option.iter (fun _ -> view.ThemeBackground <- Unchecked.defaultof<_>)


type internal MarkdownTablePropHandler =
  static member setProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    ViewPropHandler.setProps (terminalElement, props)

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

  static member removeProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    ViewPropHandler.removeProps (terminalElement, props)

    let view = terminalElement.View :?> MarkdownTable

    // Properties
    props
    |> Props.tryFind PKey.MarkdownTable.SyntaxHighlighter
    |> Option.iter (fun _ -> view.SyntaxHighlighter <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.MarkdownTable.TableData
    |> Option.iter (fun _ -> view.TableData <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.MarkdownTable.Text
    |> Option.iter (fun _ -> view.Text <- "")

    props
    |> Props.tryFind PKey.MarkdownTable.UseThemeBackground
    |> Option.iter (fun _ -> view.UseThemeBackground <- Unchecked.defaultof<_>)

    // Events
    if props |> Props.exists PKey.MarkdownTable.LinkClicked then
      terminalElement.TryRemoveEventHandler(PKey.MarkdownTable.LinkClicked)

type internal MenuPropHandler =
  static member setProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    BarPropHandler.setProps (terminalElement, props)

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

  static member removeProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    BarPropHandler.removeProps (terminalElement, props)

    let view = terminalElement.View :?> Menu

    // Properties
    props
    |> Props.tryFind PKey.Menu.SuperMenuItem
    |> Option.iter (fun _ -> view.SuperMenuItem <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.Menu.Value
    |> Option.iter (fun _ -> view.Value <- Unchecked.defaultof<_>)

    // Events
    if props |> Props.exists PKey.Menu.SelectedMenuItemChanged then
      terminalElement.TryRemoveEventHandler(PKey.Menu.SelectedMenuItemChanged)

    if props |> Props.exists PKey.Menu.ValueChanged then
      terminalElement.TryRemoveEventHandler(PKey.Menu.ValueChanged)

    if props |> Props.exists PKey.Menu.ValueChangedUntyped then
      terminalElement.TryRemoveEventHandler(PKey.Menu.ValueChangedUntyped)

    if props |> Props.exists PKey.Menu.ValueChanging then
      terminalElement.TryRemoveEventHandler(PKey.Menu.ValueChanging)

type internal MenuBarPropHandler =
  static member setProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    MenuPropHandler.setProps (terminalElement, props)

    let view = terminalElement.View :?> MenuBar

    // Properties
    props |> Props.tryFind PKey.MenuBar.Key |> Option.iter (fun v -> view.Key <- v)

    // Events
    if props |> Props.exists PKey.MenuBar.KeyChanged then
      terminalElement.TrySetEventHandler(PKey.MenuBar.KeyChanged, view.KeyChanged)

  static member removeProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    MenuPropHandler.removeProps (terminalElement, props)

    let view = terminalElement.View :?> MenuBar

    // Properties
    props
    |> Props.tryFind PKey.MenuBar.Key
    |> Option.iter (fun _ -> view.Key <- Terminal.Gui.Input.Key.Empty)

    // Events
    if props |> Props.exists PKey.MenuBar.KeyChanged then
      terminalElement.TryRemoveEventHandler(PKey.MenuBar.KeyChanged)

type internal NumericUpDownPropHandler<'T> =
  static member setProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    ViewPropHandler.setProps (terminalElement, props)

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

  static member removeProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    ViewPropHandler.removeProps (terminalElement, props)

    let view = terminalElement.View :?> NumericUpDown<'T>

    // Properties
    props
    |> Props.tryFind PKey.NumericUpDown<'T>.Format
    |> Option.iter (fun _ -> view.Format <- "")

    props
    |> Props.tryFind PKey.NumericUpDown<'T>.Increment
    |> Option.iter (fun _ -> view.Increment <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.NumericUpDown<'T>.Value
    |> Option.iter (fun _ -> view.Value <- Unchecked.defaultof<_>)

    // Events
    if props |> Props.exists PKey.NumericUpDown<'T>.FormatChanged then
      terminalElement.TryRemoveEventHandler(PKey.NumericUpDown<'T>.FormatChanged)

    if props |> Props.exists PKey.NumericUpDown<'T>.IncrementChanged then
      terminalElement.TryRemoveEventHandler(PKey.NumericUpDown<'T>.IncrementChanged)

    if props |> Props.exists PKey.NumericUpDown<'T>.ValueChanged then
      terminalElement.TryRemoveEventHandler(PKey.NumericUpDown<'T>.ValueChanged)

    if props |> Props.exists PKey.NumericUpDown<'T>.ValueChangedUntyped then
      terminalElement.TryRemoveEventHandler(PKey.NumericUpDown<'T>.ValueChangedUntyped)

    if props |> Props.exists PKey.NumericUpDown<'T>.ValueChanging then
      terminalElement.TryRemoveEventHandler(PKey.NumericUpDown<'T>.ValueChanging)

type internal NumericUpDownPropHandler =
  static member setProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    NumericUpDownPropHandler<int>.setProps (terminalElement, props)

  static member removeProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    NumericUpDownPropHandler<int>.removeProps (terminalElement, props)


type internal PaddingViewPropHandler =
  static member setProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    AdornmentViewPropHandler.setProps (terminalElement, props)

  static member removeProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    AdornmentViewPropHandler.removeProps (terminalElement, props)


type internal PopoverImplPropHandler =
  static member setProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    ViewPropHandler.setProps (terminalElement, props)

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

  static member removeProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    ViewPropHandler.removeProps (terminalElement, props)

    let view = terminalElement.View :?> PopoverImpl

    // Properties
    props
    |> Props.tryFind PKey.PopoverImpl.Anchor
    |> Option.iter (fun _ -> view.Anchor <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.PopoverImpl.Owner
    |> Option.iter (fun _ -> view.Owner <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.PopoverImpl.Target
    |> Option.iter (fun _ -> view.Target <- Unchecked.defaultof<_>)


type internal PopoverPropHandler<'TView, 'TResult
  when 'TView: (new: unit -> 'TView) and 'TView :> Terminal.Gui.ViewBase.View> =
  static member setProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    PopoverImplPropHandler.setProps (terminalElement, props)

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

  static member removeProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    PopoverImplPropHandler.removeProps (terminalElement, props)

    let view = terminalElement.View :?> Popover<'TView, 'TResult>

    // Properties
    props
    |> Props.tryFind PKey.Popover<'TView, 'TResult>.ContentView
    |> Option.iter (fun _ -> view.ContentView <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.Popover<'TView, 'TResult>.ResultExtractor
    |> Option.iter (fun _ -> view.ResultExtractor <- Unchecked.defaultof<_>)

    // Events
    if props |> Props.exists PKey.Popover<'TView, 'TResult>.ResultChanged then
      terminalElement.TryRemoveEventHandler(PKey.Popover<'TView, 'TResult>.ResultChanged)

type internal PopoverMenuPropHandler =
  static member setProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    PopoverPropHandler<Terminal.Gui.Views.Menu, Terminal.Gui.Views.MenuItem>.setProps (terminalElement, props)

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

  static member removeProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    PopoverPropHandler<Terminal.Gui.Views.Menu, Terminal.Gui.Views.MenuItem>.removeProps (terminalElement, props)

    let view = terminalElement.View :?> PopoverMenu

    // Properties
    props
    |> Props.tryFind PKey.PopoverMenu.Key
    |> Option.iter (fun _ -> view.Key <- Terminal.Gui.Input.Key.Empty)

    props
    |> Props.tryFind PKey.PopoverMenu.MouseFlags
    |> Option.iter (fun _ -> view.MouseFlags <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.PopoverMenu.Root
    |> Option.iter (fun _ -> view.Root <- Unchecked.defaultof<_>)

    // Events
    if props |> Props.exists PKey.PopoverMenu.KeyChanged then
      terminalElement.TryRemoveEventHandler(PKey.PopoverMenu.KeyChanged)

type internal ProgressBarPropHandler =
  static member setProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    ViewPropHandler.setProps (terminalElement, props)

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

  static member removeProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    ViewPropHandler.removeProps (terminalElement, props)

    let view = terminalElement.View :?> ProgressBar

    // Properties
    props
    |> Props.tryFind PKey.ProgressBar.BidirectionalMarquee
    |> Option.iter (fun _ -> view.BidirectionalMarquee <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.ProgressBar.Fraction
    |> Option.iter (fun _ -> view.Fraction <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.ProgressBar.ProgressBarFormat
    |> Option.iter (fun _ -> view.ProgressBarFormat <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.ProgressBar.ProgressBarStyle
    |> Option.iter (fun _ -> view.ProgressBarStyle <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.ProgressBar.SegmentCharacter
    |> Option.iter (fun _ -> view.SegmentCharacter <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.ProgressBar.SyncWithTerminal
    |> Option.iter (fun _ -> view.SyncWithTerminal <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.ProgressBar.Text
    |> Option.iter (fun _ -> view.Text <- "")


type internal RunnablePropHandler =
  static member setProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    ViewPropHandler.setProps (terminalElement, props)

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

  static member removeProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    ViewPropHandler.removeProps (terminalElement, props)

    let view = terminalElement.View :?> Runnable

    // Properties
    props
    |> Props.tryFind PKey.Runnable.Result
    |> Option.iter (fun _ -> view.Result <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.Runnable.StopRequested
    |> Option.iter (fun _ -> view.StopRequested <- Unchecked.defaultof<_>)

    // Events
    if props |> Props.exists PKey.Runnable.IsModalChanged then
      terminalElement.TryRemoveEventHandler(PKey.Runnable.IsModalChanged)

    if props |> Props.exists PKey.Runnable.IsRunningChanged then
      terminalElement.TryRemoveEventHandler(PKey.Runnable.IsRunningChanged)

    if props |> Props.exists PKey.Runnable.IsRunningChanging then
      terminalElement.TryRemoveEventHandler(PKey.Runnable.IsRunningChanging)

type internal RunnablePropHandler<'TResult> =
  static member setProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    RunnablePropHandler.setProps (terminalElement, props)

    let view = terminalElement.View :?> Runnable<'TResult>

    // Properties
    props
    |> Props.tryFind PKey.Runnable'<'TResult>.Result
    |> Option.iter (fun v -> view.Result <- v)

  static member removeProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    RunnablePropHandler.removeProps (terminalElement, props)

    let view = terminalElement.View :?> Runnable<'TResult>

    // Properties
    props
    |> Props.tryFind PKey.Runnable'<'TResult>.Result
    |> Option.iter (fun _ -> view.Result <- Unchecked.defaultof<_>)


type internal DialogPropHandler<'TResult> =
  static member setProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    RunnablePropHandler<'TResult>.setProps (terminalElement, props)

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

  static member removeProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    RunnablePropHandler<'TResult>.removeProps (terminalElement, props)

    let view = terminalElement.View :?> Dialog<'TResult>

    // Properties
    props
    |> Props.tryFind PKey.Dialog<'TResult>.ButtonAlignment
    |> Option.iter (fun _ -> view.ButtonAlignment <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.Dialog<'TResult>.ButtonAlignmentModes
    |> Option.iter (fun _ -> view.ButtonAlignmentModes <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.Dialog<'TResult>.Buttons
    |> Option.iter (fun _ -> view.Buttons <- Unchecked.defaultof<_>)


type internal RunnableWrapperPropHandler<'TView, 'TResult
  when 'TView: (new: unit -> 'TView) and 'TView :> Terminal.Gui.ViewBase.View> =
  static member setProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    RunnablePropHandler<'TResult>.setProps (terminalElement, props)

    let view = terminalElement.View :?> RunnableWrapper<'TView, 'TResult>

    // Properties
    props
    |> Props.tryFind PKey.RunnableWrapper<'TView, 'TResult>.ResultExtractor
    |> Option.iter (fun v -> view.ResultExtractor <- v)

  static member removeProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    RunnablePropHandler<'TResult>.removeProps (terminalElement, props)

    let view = terminalElement.View :?> RunnableWrapper<'TView, 'TResult>

    // Properties
    props
    |> Props.tryFind PKey.RunnableWrapper<'TView, 'TResult>.ResultExtractor
    |> Option.iter (fun _ -> view.ResultExtractor <- Unchecked.defaultof<_>)


type internal DialogPropHandler =
  static member setProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    DialogPropHandler<int>.setProps (terminalElement, props)

    let view = terminalElement.View :?> Dialog

    // Properties
    props
    |> Props.tryFind PKey.Dialog'.Result
    |> Option.iter (fun v -> view.Result <- v)

  static member removeProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    DialogPropHandler<int>.removeProps (terminalElement, props)

    let view = terminalElement.View :?> Dialog

    // Properties
    props
    |> Props.tryFind PKey.Dialog'.Result
    |> Option.iter (fun _ -> view.Result <- Unchecked.defaultof<_>)


type internal FileDialogPropHandler =
  static member setProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    DialogPropHandler<IReadOnlyList<string>>.setProps (terminalElement, props)

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

  static member removeProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    DialogPropHandler<IReadOnlyList<string>>.removeProps (terminalElement, props)

    let view = terminalElement.View :?> FileDialog

    // Properties
    props
    |> Props.tryFind PKey.FileDialog.AllowedTypes
    |> Option.iter (fun _ -> view.AllowedTypes <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.FileDialog.AllowsMultipleSelection
    |> Option.iter (fun _ -> view.AllowsMultipleSelection <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.FileDialog.FileOperationsHandler
    |> Option.iter (fun _ -> view.FileOperationsHandler <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.FileDialog.MustExist
    |> Option.iter (fun _ -> view.MustExist <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.FileDialog.OpenMode
    |> Option.iter (fun _ -> view.OpenMode <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.FileDialog.Path
    |> Option.iter (fun _ -> view.Path <- "")

    props
    |> Props.tryFind PKey.FileDialog.SearchMatcher
    |> Option.iter (fun _ -> view.SearchMatcher <- Unchecked.defaultof<_>)

    // Events
    if props |> Props.exists PKey.FileDialog.FilesSelected then
      terminalElement.TryRemoveEventHandler(PKey.FileDialog.FilesSelected)

type internal PromptPropHandler<'TView, 'TResult
  when 'TView: (new: unit -> 'TView) and 'TView :> Terminal.Gui.ViewBase.View> =
  static member setProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    DialogPropHandler<'TResult>.setProps (terminalElement, props)

    let view = terminalElement.View :?> Prompt<'TView, 'TResult>

    // Properties
    props
    |> Props.tryFind PKey.Prompt<'TView, 'TResult>.ResultExtractor
    |> Option.iter (fun v -> view.ResultExtractor <- v)

  static member removeProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    DialogPropHandler<'TResult>.removeProps (terminalElement, props)

    let view = terminalElement.View :?> Prompt<'TView, 'TResult>

    // Properties
    props
    |> Props.tryFind PKey.Prompt<'TView, 'TResult>.ResultExtractor
    |> Option.iter (fun _ -> view.ResultExtractor <- Unchecked.defaultof<_>)


type internal OpenDialogPropHandler =
  static member setProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    FileDialogPropHandler.setProps (terminalElement, props)

    let view = terminalElement.View :?> OpenDialog

    // Properties
    props
    |> Props.tryFind PKey.OpenDialog.OpenMode
    |> Option.iter (fun v -> view.OpenMode <- v)

  static member removeProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    FileDialogPropHandler.removeProps (terminalElement, props)

    let view = terminalElement.View :?> OpenDialog

    // Properties
    props
    |> Props.tryFind PKey.OpenDialog.OpenMode
    |> Option.iter (fun _ -> view.OpenMode <- Unchecked.defaultof<_>)


type internal SaveDialogPropHandler =
  static member setProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    FileDialogPropHandler.setProps (terminalElement, props)

  static member removeProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    FileDialogPropHandler.removeProps (terminalElement, props)


type internal ScrollBarPropHandler =
  static member setProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    ViewPropHandler.setProps (terminalElement, props)

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

  static member removeProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    ViewPropHandler.removeProps (terminalElement, props)

    let view = terminalElement.View :?> ScrollBar

    // Properties
    props
    |> Props.tryFind PKey.ScrollBar.Increment
    |> Option.iter (fun _ -> view.Increment <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.ScrollBar.Orientation
    |> Option.iter (fun _ -> view.Orientation <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.ScrollBar.ScrollableContentSize
    |> Option.iter (fun _ -> view.ScrollableContentSize <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.ScrollBar.Value
    |> Option.iter (fun _ -> view.Value <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.ScrollBar.VisibilityMode
    |> Option.iter (fun _ -> view.VisibilityMode <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.ScrollBar.VisibleContentSize
    |> Option.iter (fun _ -> view.VisibleContentSize <- Unchecked.defaultof<_>)

    // Events
    if props |> Props.exists PKey.ScrollBar.OrientationChanged then
      terminalElement.TryRemoveEventHandler(PKey.ScrollBar.OrientationChanged)

    if props |> Props.exists PKey.ScrollBar.OrientationChanging then
      terminalElement.TryRemoveEventHandler(PKey.ScrollBar.OrientationChanging)

    if props |> Props.exists PKey.ScrollBar.ScrollableContentSizeChanged then
      terminalElement.TryRemoveEventHandler(PKey.ScrollBar.ScrollableContentSizeChanged)

    if props |> Props.exists PKey.ScrollBar.Scrolled then
      terminalElement.TryRemoveEventHandler(PKey.ScrollBar.Scrolled)

    if props |> Props.exists PKey.ScrollBar.SliderPositionChanged then
      terminalElement.TryRemoveEventHandler(PKey.ScrollBar.SliderPositionChanged)

    if props |> Props.exists PKey.ScrollBar.ValueChanged then
      terminalElement.TryRemoveEventHandler(PKey.ScrollBar.ValueChanged)

    if props |> Props.exists PKey.ScrollBar.ValueChangedUntyped then
      terminalElement.TryRemoveEventHandler(PKey.ScrollBar.ValueChangedUntyped)

    if props |> Props.exists PKey.ScrollBar.ValueChanging then
      terminalElement.TryRemoveEventHandler(PKey.ScrollBar.ValueChanging)

type internal ScrollButtonPropHandler =
  static member setProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    ButtonPropHandler.setProps (terminalElement, props)

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

  static member removeProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    ButtonPropHandler.removeProps (terminalElement, props)

    let view = terminalElement.View :?> ScrollButton

    // Properties
    props
    |> Props.tryFind PKey.ScrollButton.Direction
    |> Option.iter (fun _ -> view.Direction <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.ScrollButton.Orientation
    |> Option.iter (fun _ -> view.Orientation <- Unchecked.defaultof<_>)

    // Events
    if props |> Props.exists PKey.ScrollButton.OrientationChanged then
      terminalElement.TryRemoveEventHandler(PKey.ScrollButton.OrientationChanged)

    if props |> Props.exists PKey.ScrollButton.OrientationChanging then
      terminalElement.TryRemoveEventHandler(PKey.ScrollButton.OrientationChanging)

type internal ScrollSliderPropHandler =
  static member setProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    ViewPropHandler.setProps (terminalElement, props)

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

  static member removeProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    ViewPropHandler.removeProps (terminalElement, props)

    let view = terminalElement.View :?> ScrollSlider

    // Properties
    props
    |> Props.tryFind PKey.ScrollSlider.Orientation
    |> Option.iter (fun _ -> view.Orientation <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.ScrollSlider.Position
    |> Option.iter (fun _ -> view.Position <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.ScrollSlider.Size
    |> Option.iter (fun _ -> view.Size <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.ScrollSlider.SliderPadding
    |> Option.iter (fun _ -> view.SliderPadding <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.ScrollSlider.Value
    |> Option.iter (fun _ -> view.Value <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.ScrollSlider.VisibleContentSize
    |> Option.iter (fun _ -> view.VisibleContentSize <- Unchecked.defaultof<_>)

    // Events
    if props |> Props.exists PKey.ScrollSlider.OrientationChanged then
      terminalElement.TryRemoveEventHandler(PKey.ScrollSlider.OrientationChanged)

    if props |> Props.exists PKey.ScrollSlider.OrientationChanging then
      terminalElement.TryRemoveEventHandler(PKey.ScrollSlider.OrientationChanging)

    if props |> Props.exists PKey.ScrollSlider.PositionChanged then
      terminalElement.TryRemoveEventHandler(PKey.ScrollSlider.PositionChanged)

    if props |> Props.exists PKey.ScrollSlider.PositionChanging then
      terminalElement.TryRemoveEventHandler(PKey.ScrollSlider.PositionChanging)

    if props |> Props.exists PKey.ScrollSlider.Scrolled then
      terminalElement.TryRemoveEventHandler(PKey.ScrollSlider.Scrolled)

    if props |> Props.exists PKey.ScrollSlider.ValueChanged then
      terminalElement.TryRemoveEventHandler(PKey.ScrollSlider.ValueChanged)

    if props |> Props.exists PKey.ScrollSlider.ValueChangedUntyped then
      terminalElement.TryRemoveEventHandler(PKey.ScrollSlider.ValueChangedUntyped)

    if props |> Props.exists PKey.ScrollSlider.ValueChanging then
      terminalElement.TryRemoveEventHandler(PKey.ScrollSlider.ValueChanging)

type internal SelectorBasePropHandler =
  static member setProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    ViewPropHandler.setProps (terminalElement, props)

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

  static member removeProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    ViewPropHandler.removeProps (terminalElement, props)

    let view = terminalElement.View :?> SelectorBase

    // Properties
    props
    |> Props.tryFind PKey.SelectorBase.DoubleClickAccepts
    |> Option.iter (fun _ -> view.DoubleClickAccepts <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.SelectorBase.HorizontalSpace
    |> Option.iter (fun _ -> view.HorizontalSpace <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.SelectorBase.Labels
    |> Option.iter (fun _ -> view.Labels <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.SelectorBase.Orientation
    |> Option.iter (fun _ -> view.Orientation <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.SelectorBase.Styles
    |> Option.iter (fun _ -> view.Styles <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.SelectorBase.TabBehavior
    |> Option.iter (fun _ -> view.TabBehavior <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.SelectorBase.Value
    |> Option.iter (fun _ -> view.Value <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.SelectorBase.Values
    |> Option.iter (fun _ -> view.Values <- Unchecked.defaultof<_>)

    // Events
    if props |> Props.exists PKey.SelectorBase.OrientationChanged then
      terminalElement.TryRemoveEventHandler(PKey.SelectorBase.OrientationChanged)

    if props |> Props.exists PKey.SelectorBase.OrientationChanging then
      terminalElement.TryRemoveEventHandler(PKey.SelectorBase.OrientationChanging)

    if props |> Props.exists PKey.SelectorBase.ValueChanged then
      terminalElement.TryRemoveEventHandler(PKey.SelectorBase.ValueChanged)

    if props |> Props.exists PKey.SelectorBase.ValueChangedUntyped then
      terminalElement.TryRemoveEventHandler(PKey.SelectorBase.ValueChangedUntyped)

    if props |> Props.exists PKey.SelectorBase.ValueChanging then
      terminalElement.TryRemoveEventHandler(PKey.SelectorBase.ValueChanging)

type internal FlagSelectorPropHandler =
  static member setProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    SelectorBasePropHandler.setProps (terminalElement, props)

    let view = terminalElement.View :?> FlagSelector

    // Properties
    props
    |> Props.tryFind PKey.FlagSelector.Value
    |> Option.iter (fun v -> view.Value <- v)

  static member removeProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    SelectorBasePropHandler.removeProps (terminalElement, props)

    let view = terminalElement.View :?> FlagSelector

    // Properties
    props
    |> Props.tryFind PKey.FlagSelector.Value
    |> Option.iter (fun _ -> view.Value <- Unchecked.defaultof<_>)


type internal OptionSelectorPropHandler =
  static member setProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    SelectorBasePropHandler.setProps (terminalElement, props)

    let view = terminalElement.View :?> OptionSelector

    // Properties
    props
    |> Props.tryFind PKey.OptionSelector.FocusedItem
    |> Option.iter (fun v -> view.FocusedItem <- v)

  static member removeProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    SelectorBasePropHandler.removeProps (terminalElement, props)

    let view = terminalElement.View :?> OptionSelector

    // Properties
    props
    |> Props.tryFind PKey.OptionSelector.FocusedItem
    |> Option.iter (fun _ -> view.FocusedItem <- Unchecked.defaultof<_>)


type internal FlagSelectorPropHandler<'TFlagsEnum
  when 'TFlagsEnum: struct
  and 'TFlagsEnum: (new: unit -> 'TFlagsEnum)
  and 'TFlagsEnum :> System.Enum
  and 'TFlagsEnum :> System.ValueType> =
  static member setProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    FlagSelectorPropHandler.setProps (terminalElement, props)

    let view = terminalElement.View :?> FlagSelector<'TFlagsEnum>

    // Properties
    props
    |> Props.tryFind PKey.FlagSelector'<'TFlagsEnum>.Value
    |> Option.iter (fun v -> view.Value <- v)

    // Events
    if props |> Props.exists PKey.FlagSelector'<'TFlagsEnum>.ValueChanged then
      terminalElement.TrySetEventHandler(PKey.FlagSelector'<'TFlagsEnum>.ValueChanged, view.ValueChanged)

  static member removeProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    FlagSelectorPropHandler.removeProps (terminalElement, props)

    let view = terminalElement.View :?> FlagSelector<'TFlagsEnum>

    // Properties
    props
    |> Props.tryFind PKey.FlagSelector'<'TFlagsEnum>.Value
    |> Option.iter (fun _ -> view.Value <- Unchecked.defaultof<_>)

    // Events
    if props |> Props.exists PKey.FlagSelector'<'TFlagsEnum>.ValueChanged then
      terminalElement.TryRemoveEventHandler(PKey.FlagSelector'<'TFlagsEnum>.ValueChanged)

type internal OptionSelectorPropHandler<'TEnum
  when 'TEnum: struct and 'TEnum: (new: unit -> 'TEnum) and 'TEnum :> System.Enum and 'TEnum :> System.ValueType> =
  static member setProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    OptionSelectorPropHandler.setProps (terminalElement, props)

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

  static member removeProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    OptionSelectorPropHandler.removeProps (terminalElement, props)

    let view = terminalElement.View :?> OptionSelector<'TEnum>

    // Properties
    props
    |> Props.tryFind PKey.OptionSelector'<'TEnum>.Value
    |> Option.iter (fun _ -> view.Value <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.OptionSelector'<'TEnum>.Values
    |> Option.iter (fun _ -> view.Values <- Unchecked.defaultof<_>)

    // Events
    if props |> Props.exists PKey.OptionSelector'<'TEnum>.ValueChanged then
      terminalElement.TryRemoveEventHandler(PKey.OptionSelector'<'TEnum>.ValueChanged)

type internal ShortcutPropHandler =
  static member setProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    ViewPropHandler.setProps (terminalElement, props)

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

  static member removeProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    ViewPropHandler.removeProps (terminalElement, props)

    let view = terminalElement.View :?> Shortcut

    // Properties
    props
    |> Props.tryFind PKey.Shortcut.Action
    |> Option.iter (fun _ -> view.Action <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.Shortcut.AlignmentModes
    |> Option.iter (fun _ -> view.AlignmentModes <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.Shortcut.BindKeyToApplication
    |> Option.iter (fun _ -> view.BindKeyToApplication <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.Shortcut.Command
    |> Option.iter (fun _ -> view.Command <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.Shortcut.CommandView
    |> Option.iter (fun _ -> view.CommandView <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.Shortcut.HelpText
    |> Option.iter (fun _ -> view.HelpText <- "")

    props
    |> Props.tryFind PKey.Shortcut.Key
    |> Option.iter (fun _ -> view.Key <- Terminal.Gui.Input.Key.Empty)

    props
    |> Props.tryFind PKey.Shortcut.MinimumKeyTextSize
    |> Option.iter (fun _ -> view.MinimumKeyTextSize <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.Shortcut.Orientation
    |> Option.iter (fun _ -> view.Orientation <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.Shortcut.TargetView
    |> Option.iter (fun _ -> view.TargetView <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.Shortcut.Text
    |> Option.iter (fun _ -> view.Text <- "")

    // Events
    if props |> Props.exists PKey.Shortcut.OrientationChanged then
      terminalElement.TryRemoveEventHandler(PKey.Shortcut.OrientationChanged)

    if props |> Props.exists PKey.Shortcut.OrientationChanging then
      terminalElement.TryRemoveEventHandler(PKey.Shortcut.OrientationChanging)

type internal MenuItemPropHandler =
  static member setProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    ShortcutPropHandler.setProps (terminalElement, props)

    let view = terminalElement.View :?> MenuItem

    // Properties
    props
    |> Props.tryFind PKey.MenuItem.SubMenu
    |> Option.iter (fun v -> view.SubMenu <- v)

  static member removeProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    ShortcutPropHandler.removeProps (terminalElement, props)

    let view = terminalElement.View :?> MenuItem

    // Properties
    props
    |> Props.tryFind PKey.MenuItem.SubMenu
    |> Option.iter (fun _ -> view.SubMenu <- Unchecked.defaultof<_>)


type internal MenuBarItemPropHandler =
  static member setProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    MenuItemPropHandler.setProps (terminalElement, props)

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

  static member removeProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    MenuItemPropHandler.removeProps (terminalElement, props)

    let view = terminalElement.View :?> MenuBarItem

    // Properties
    props
    |> Props.tryFind PKey.MenuBarItem.PopoverMenu
    |> Option.iter (fun _ -> view.PopoverMenu <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.MenuBarItem.PopoverMenuOpen
    |> Option.iter (fun _ -> view.PopoverMenuOpen <- Unchecked.defaultof<_>)

    // Events
    if props |> Props.exists PKey.MenuBarItem.MenuOpenChanged then
      terminalElement.TryRemoveEventHandler(PKey.MenuBarItem.MenuOpenChanged)

    if props |> Props.exists PKey.MenuBarItem.PopoverMenuOpenChanged then
      terminalElement.TryRemoveEventHandler(PKey.MenuBarItem.PopoverMenuOpenChanged)

type internal SpinnerViewPropHandler =
  static member setProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    ViewPropHandler.setProps (terminalElement, props)

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

  static member removeProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    ViewPropHandler.removeProps (terminalElement, props)

    let view = terminalElement.View :?> SpinnerView

    // Properties
    props
    |> Props.tryFind PKey.SpinnerView.AutoSpin
    |> Option.iter (fun _ -> view.AutoSpin <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.SpinnerView.Sequence
    |> Option.iter (fun _ -> view.Sequence <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.SpinnerView.SpinBounce
    |> Option.iter (fun _ -> view.SpinBounce <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.SpinnerView.SpinDelay
    |> Option.iter (fun _ -> view.SpinDelay <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.SpinnerView.SpinReverse
    |> Option.iter (fun _ -> view.SpinReverse <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.SpinnerView.Style
    |> Option.iter (fun _ -> view.Style <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.SpinnerView.SyncWithTerminal
    |> Option.iter (fun _ -> view.SyncWithTerminal <- Unchecked.defaultof<_>)


type internal StatusBarPropHandler =
  static member setProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    BarPropHandler.setProps (terminalElement, props)

  static member removeProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    BarPropHandler.removeProps (terminalElement, props)


type internal TableViewPropHandler =
  static member setProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    ViewPropHandler.setProps (terminalElement, props)

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

  static member removeProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    ViewPropHandler.removeProps (terminalElement, props)

    let view = terminalElement.View :?> TableView

    // Properties
    props
    |> Props.tryFind PKey.TableView.CollectionNavigator
    |> Option.iter (fun _ -> view.CollectionNavigator <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.TableView.ColumnOffset
    |> Option.iter (fun _ -> view.ColumnOffset <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.TableView.FullRowSelect
    |> Option.iter (fun _ -> view.FullRowSelect <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.TableView.MaxCellWidth
    |> Option.iter (fun _ -> view.MaxCellWidth <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.TableView.MinCellWidth
    |> Option.iter (fun _ -> view.MinCellWidth <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.TableView.MultiSelect
    |> Option.iter (fun _ -> view.MultiSelect <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.TableView.NullSymbol
    |> Option.iter (fun _ -> view.NullSymbol <- "")

    props
    |> Props.tryFind PKey.TableView.RowOffset
    |> Option.iter (fun _ -> view.RowOffset <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.TableView.SeparatorSymbol
    |> Option.iter (fun _ -> view.SeparatorSymbol <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.TableView.Style
    |> Option.iter (fun _ -> view.Style <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.TableView.Table
    |> Option.iter (fun _ -> view.Table <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.TableView.UseAllRowsForContentCalculation
    |> Option.iter (fun _ -> view.UseAllRowsForContentCalculation <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.TableView.Value
    |> Option.iter (fun _ -> view.Value <- Unchecked.defaultof<_>)

    // Events
    if props |> Props.exists PKey.TableView.ValueChanged then
      terminalElement.TryRemoveEventHandler(PKey.TableView.ValueChanged)

    if props |> Props.exists PKey.TableView.ValueChangedUntyped then
      terminalElement.TryRemoveEventHandler(PKey.TableView.ValueChangedUntyped)

    if props |> Props.exists PKey.TableView.ValueChanging then
      terminalElement.TryRemoveEventHandler(PKey.TableView.ValueChanging)

type internal TabsPropHandler =
  static member setProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    ViewPropHandler.setProps (terminalElement, props)

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

  static member removeProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    ViewPropHandler.removeProps (terminalElement, props)

    let view = terminalElement.View :?> Tabs

    // Properties
    props
    |> Props.tryFind PKey.Tabs.ScrollOffset
    |> Option.iter (fun _ -> view.ScrollOffset <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.Tabs.TabDepth
    |> Option.iter (fun _ -> view.TabDepth <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.Tabs.TabLineStyle
    |> Option.iter (fun _ -> view.TabLineStyle <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.Tabs.TabSide
    |> Option.iter (fun _ -> view.TabSide <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.Tabs.TabSpacing
    |> Option.iter (fun _ -> view.TabSpacing <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.Tabs.Value
    |> Option.iter (fun _ -> view.Value <- Unchecked.defaultof<_>)

    // Events
    if props |> Props.exists PKey.Tabs.ValueChanged then
      terminalElement.TryRemoveEventHandler(PKey.Tabs.ValueChanged)

    if props |> Props.exists PKey.Tabs.ValueChangedUntyped then
      terminalElement.TryRemoveEventHandler(PKey.Tabs.ValueChangedUntyped)

    if props |> Props.exists PKey.Tabs.ValueChanging then
      terminalElement.TryRemoveEventHandler(PKey.Tabs.ValueChanging)

type internal TextFieldPropHandler =
  static member setProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    ViewPropHandler.setProps (terminalElement, props)

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

  static member removeProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    ViewPropHandler.removeProps (terminalElement, props)

    let view = terminalElement.View :?> TextField

    // Properties
    props
    |> Props.tryFind PKey.TextField.Autocomplete
    |> Option.iter (fun _ -> view.Autocomplete <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.TextField.InsertionPoint
    |> Option.iter (fun _ -> view.InsertionPoint <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.TextField.ReadOnly
    |> Option.iter (fun _ -> view.ReadOnly <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.TextField.Secret
    |> Option.iter (fun _ -> view.Secret <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.TextField.SelectWordOnlyOnDoubleClick
    |> Option.iter (fun _ -> view.SelectWordOnlyOnDoubleClick <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.TextField.SelectedStart
    |> Option.iter (fun _ -> view.SelectedStart <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.TextField.Text
    |> Option.iter (fun _ -> view.Text <- "")

    props
    |> Props.tryFind PKey.TextField.UseSameRuneTypeForWords
    |> Option.iter (fun _ -> view.UseSameRuneTypeForWords <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.TextField.Used
    |> Option.iter (fun _ -> view.Used <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.TextField.Value
    |> Option.iter (fun _ -> view.Value <- "")

    // Events
    if props |> Props.exists PKey.TextField.TextChanging then
      terminalElement.TryRemoveEventHandler(PKey.TextField.TextChanging)

    if props |> Props.exists PKey.TextField.ValueChanged then
      terminalElement.TryRemoveEventHandler(PKey.TextField.ValueChanged)

    if props |> Props.exists PKey.TextField.ValueChangedUntyped then
      terminalElement.TryRemoveEventHandler(PKey.TextField.ValueChangedUntyped)

    if props |> Props.exists PKey.TextField.ValueChanging then
      terminalElement.TryRemoveEventHandler(PKey.TextField.ValueChanging)

type internal DropDownListPropHandler =
  static member setProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    TextFieldPropHandler.setProps (terminalElement, props)

    let view = terminalElement.View :?> DropDownList

    // Properties
    props
    |> Props.tryFind PKey.DropDownList.Source
    |> Option.iter (fun v -> view.Source <- v)

  static member removeProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    TextFieldPropHandler.removeProps (terminalElement, props)

    let view = terminalElement.View :?> DropDownList

    // Properties
    props
    |> Props.tryFind PKey.DropDownList.Source
    |> Option.iter (fun _ -> view.Source <- Unchecked.defaultof<_>)


type internal DropDownListPropHandler<'TEnum
  when 'TEnum: struct and 'TEnum: (new: unit -> 'TEnum) and 'TEnum :> System.Enum and 'TEnum :> System.ValueType> =
  static member setProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    DropDownListPropHandler.setProps (terminalElement, props)

    let view = terminalElement.View :?> DropDownList<'TEnum>

    // Properties
    props
    |> Props.tryFind PKey.DropDownList'<'TEnum>.Value
    |> Option.iter (fun v -> view.Value <- v)

    // Events
    if props |> Props.exists PKey.DropDownList'<'TEnum>.ValueChanged then
      terminalElement.TrySetEventHandler(PKey.DropDownList'<'TEnum>.ValueChanged, view.ValueChanged)

  static member removeProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    DropDownListPropHandler.removeProps (terminalElement, props)

    let view = terminalElement.View :?> DropDownList<'TEnum>

    // Properties
    props
    |> Props.tryFind PKey.DropDownList'<'TEnum>.Value
    |> Option.iter (fun _ -> view.Value <- Unchecked.defaultof<_>)

    // Events
    if props |> Props.exists PKey.DropDownList'<'TEnum>.ValueChanged then
      terminalElement.TryRemoveEventHandler(PKey.DropDownList'<'TEnum>.ValueChanged)

type internal TextValidateFieldPropHandler =
  static member setProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    ViewPropHandler.setProps (terminalElement, props)

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

  static member removeProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    ViewPropHandler.removeProps (terminalElement, props)

    let view = terminalElement.View :?> TextValidateField

    // Properties
    props
    |> Props.tryFind PKey.TextValidateField.Provider
    |> Option.iter (fun _ -> view.Provider <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.TextValidateField.Text
    |> Option.iter (fun _ -> view.Text <- "")

    props
    |> Props.tryFind PKey.TextValidateField.Value
    |> Option.iter (fun _ -> view.Value <- "")

    // Events
    if props |> Props.exists PKey.TextValidateField.ValueChanged then
      terminalElement.TryRemoveEventHandler(PKey.TextValidateField.ValueChanged)

    if props |> Props.exists PKey.TextValidateField.ValueChangedUntyped then
      terminalElement.TryRemoveEventHandler(PKey.TextValidateField.ValueChangedUntyped)

    if props |> Props.exists PKey.TextValidateField.ValueChanging then
      terminalElement.TryRemoveEventHandler(PKey.TextValidateField.ValueChanging)

type internal DateEditorPropHandler =
  static member setProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    TextValidateFieldPropHandler.setProps (terminalElement, props)

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

  static member removeProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    TextValidateFieldPropHandler.removeProps (terminalElement, props)

    let view = terminalElement.View :?> DateEditor

    // Properties
    props
    |> Props.tryFind PKey.DateEditor.Format
    |> Option.iter (fun _ -> view.Format <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.DateEditor.Value
    |> Option.iter (fun _ -> view.Value <- Unchecked.defaultof<_>)

    // Events
    if props |> Props.exists PKey.DateEditor.ValueChanged then
      terminalElement.TryRemoveEventHandler(PKey.DateEditor.ValueChanged)

    if props |> Props.exists PKey.DateEditor.ValueChangedUntyped then
      terminalElement.TryRemoveEventHandler(PKey.DateEditor.ValueChangedUntyped)

    if props |> Props.exists PKey.DateEditor.ValueChanging then
      terminalElement.TryRemoveEventHandler(PKey.DateEditor.ValueChanging)

type internal TextViewPropHandler =
  static member setProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    ViewPropHandler.setProps (terminalElement, props)

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

  static member removeProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    ViewPropHandler.removeProps (terminalElement, props)

    let view = terminalElement.View :?> TextView

    // Properties
    props
    |> Props.tryFind PKey.TextView.EnterKeyAddsLine
    |> Option.iter (fun _ -> view.EnterKeyAddsLine <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.TextView.InheritsPreviousAttribute
    |> Option.iter (fun _ -> view.InheritsPreviousAttribute <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.TextView.InsertionPoint
    |> Option.iter (fun _ -> view.InsertionPoint <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.TextView.IsSelecting
    |> Option.iter (fun _ -> view.IsSelecting <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.TextView.Multiline
    |> Option.iter (fun _ -> view.Multiline <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.TextView.ReadOnly
    |> Option.iter (fun _ -> view.ReadOnly <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.TextView.ScrollBars
    |> Option.iter (fun _ -> view.ScrollBars <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.TextView.SelectWordOnlyOnDoubleClick
    |> Option.iter (fun _ -> view.SelectWordOnlyOnDoubleClick <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.TextView.SelectionStartColumn
    |> Option.iter (fun _ -> view.SelectionStartColumn <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.TextView.SelectionStartRow
    |> Option.iter (fun _ -> view.SelectionStartRow <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.TextView.TabKeyAddsTab
    |> Option.iter (fun _ -> view.TabKeyAddsTab <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.TextView.TabWidth
    |> Option.iter (fun _ -> view.TabWidth <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.TextView.Text
    |> Option.iter (fun _ -> view.Text <- "")

    props
    |> Props.tryFind PKey.TextView.UseSameRuneTypeForWords
    |> Option.iter (fun _ -> view.UseSameRuneTypeForWords <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.TextView.Used
    |> Option.iter (fun _ -> view.Used <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.TextView.WordWrap
    |> Option.iter (fun _ -> view.WordWrap <- Unchecked.defaultof<_>)

    // Events
    if props |> Props.exists PKey.TextView.ContentsChanged then
      terminalElement.TryRemoveEventHandler(PKey.TextView.ContentsChanged)

    if props |> Props.exists PKey.TextView.DrawNormalColor then
      terminalElement.TryRemoveEventHandler(PKey.TextView.DrawNormalColor)

    if props |> Props.exists PKey.TextView.DrawReadOnlyColor then
      terminalElement.TryRemoveEventHandler(PKey.TextView.DrawReadOnlyColor)

    if props |> Props.exists PKey.TextView.DrawSelectionColor then
      terminalElement.TryRemoveEventHandler(PKey.TextView.DrawSelectionColor)

    if props |> Props.exists PKey.TextView.DrawUsedColor then
      terminalElement.TryRemoveEventHandler(PKey.TextView.DrawUsedColor)

    if props |> Props.exists PKey.TextView.UnwrappedCursorPositionChanged then
      terminalElement.TryRemoveEventHandler(PKey.TextView.UnwrappedCursorPositionChanged)

type internal TimeEditorPropHandler =
  static member setProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    TextValidateFieldPropHandler.setProps (terminalElement, props)

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

  static member removeProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    TextValidateFieldPropHandler.removeProps (terminalElement, props)

    let view = terminalElement.View :?> TimeEditor

    // Properties
    props
    |> Props.tryFind PKey.TimeEditor.Format
    |> Option.iter (fun _ -> view.Format <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.TimeEditor.Value
    |> Option.iter (fun _ -> view.Value <- Unchecked.defaultof<_>)

    // Events
    if props |> Props.exists PKey.TimeEditor.ValueChanged then
      terminalElement.TryRemoveEventHandler(PKey.TimeEditor.ValueChanged)

    if props |> Props.exists PKey.TimeEditor.ValueChangedUntyped then
      terminalElement.TryRemoveEventHandler(PKey.TimeEditor.ValueChangedUntyped)

    if props |> Props.exists PKey.TimeEditor.ValueChanging then
      terminalElement.TryRemoveEventHandler(PKey.TimeEditor.ValueChanging)

type internal TitleViewPropHandler =
  static member setProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    ViewPropHandler.setProps (terminalElement, props)

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

  static member removeProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    ViewPropHandler.removeProps (terminalElement, props)

    let view = terminalElement.View :?> TitleView

    // Properties
    props
    |> Props.tryFind PKey.TitleView.Direction
    |> Option.iter (fun _ -> view.Direction <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.TitleView.MeasuredTabLength
    |> Option.iter (fun _ -> view.MeasuredTabLength <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.TitleView.Orientation
    |> Option.iter (fun _ -> view.Orientation <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.TitleView.TabDepth
    |> Option.iter (fun _ -> view.TabDepth <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.TitleView.TabSide
    |> Option.iter (fun _ -> view.TabSide <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.TitleView.Text
    |> Option.iter (fun _ -> view.Text <- "")

    // Events
    if props |> Props.exists PKey.TitleView.OrientationChanged then
      terminalElement.TryRemoveEventHandler(PKey.TitleView.OrientationChanged)

    if props |> Props.exists PKey.TitleView.OrientationChanging then
      terminalElement.TryRemoveEventHandler(PKey.TitleView.OrientationChanging)

type internal ToolTipHostPropHandler<'TView when 'TView: (new: unit -> 'TView) and 'TView :> Terminal.Gui.ViewBase.View>
  =
  static member setProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    PopoverImplPropHandler.setProps (terminalElement, props)

    let view = terminalElement.View :?> ToolTipHost<'TView>

    // Properties
    props
    |> Props.tryFind PKey.ToolTipHost<'TView>.ContentView
    |> Option.iter (fun v -> view.ContentView <- v)

  static member removeProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    PopoverImplPropHandler.removeProps (terminalElement, props)

    let view = terminalElement.View :?> ToolTipHost<'TView>

    // Properties
    props
    |> Props.tryFind PKey.ToolTipHost<'TView>.ContentView
    |> Option.iter (fun _ -> view.ContentView <- Unchecked.defaultof<_>)


type internal TreeViewPropHandler<'T when 'T: not struct> =
  static member setProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    ViewPropHandler.setProps (terminalElement, props)

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

  static member removeProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    ViewPropHandler.removeProps (terminalElement, props)

    let view = terminalElement.View :?> TreeView<'T>

    // Properties
    props
    |> Props.tryFind PKey.TreeView<'T>.AllowLetterBasedNavigation
    |> Option.iter (fun _ -> view.AllowLetterBasedNavigation <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.TreeView<'T>.AspectGetter
    |> Option.iter (fun _ -> view.AspectGetter <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.TreeView<'T>.CheckboxMode
    |> Option.iter (fun _ -> view.CheckboxMode <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.TreeView<'T>.ColorGetter
    |> Option.iter (fun _ -> view.ColorGetter <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.TreeView<'T>.Filter
    |> Option.iter (fun _ -> view.Filter <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.TreeView<'T>.MaxDepth
    |> Option.iter (fun _ -> view.MaxDepth <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.TreeView<'T>.MultiSelect
    |> Option.iter (fun _ -> view.MultiSelect <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.TreeView<'T>.ScrollOffsetHorizontal
    |> Option.iter (fun _ -> view.ScrollOffsetHorizontal <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.TreeView<'T>.ScrollOffsetVertical
    |> Option.iter (fun _ -> view.ScrollOffsetVertical <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.TreeView<'T>.SelectedObject
    |> Option.iter (fun _ -> view.SelectedObject <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.TreeView<'T>.Style
    |> Option.iter (fun _ -> view.Style <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.TreeView<'T>.TreeBuilder
    |> Option.iter (fun _ -> view.TreeBuilder <- Unchecked.defaultof<_>)

    // Events
    if props |> Props.exists PKey.TreeView<'T>.CheckedChanged then
      terminalElement.TryRemoveEventHandler(PKey.TreeView<'T>.CheckedChanged)

    if props |> Props.exists PKey.TreeView<'T>.DrawLine then
      terminalElement.TryRemoveEventHandler(PKey.TreeView<'T>.DrawLine)

    if props |> Props.exists PKey.TreeView<'T>.SelectionChanged then
      terminalElement.TryRemoveEventHandler(PKey.TreeView<'T>.SelectionChanged)

type internal TreeViewPropHandler =
  static member setProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    TreeViewPropHandler<Terminal.Gui.Views.ITreeNode>.setProps (terminalElement, props)

  static member removeProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    TreeViewPropHandler<Terminal.Gui.Views.ITreeNode>.removeProps (terminalElement, props)


type internal WindowPropHandler =
  static member setProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    RunnablePropHandler.setProps (terminalElement, props)

  static member removeProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    RunnablePropHandler.removeProps (terminalElement, props)


type internal WizardPropHandler =
  static member setProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    DialogPropHandler.setProps (terminalElement, props)

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

  static member removeProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    DialogPropHandler.removeProps (terminalElement, props)

    let view = terminalElement.View :?> Wizard

    // Properties
    props
    |> Props.tryFind PKey.Wizard.CurrentStep
    |> Option.iter (fun _ -> view.CurrentStep <- Unchecked.defaultof<_>)

    // Events
    if props |> Props.exists PKey.Wizard.MovingBack then
      terminalElement.TryRemoveEventHandler(PKey.Wizard.MovingBack)

    if props |> Props.exists PKey.Wizard.MovingNext then
      terminalElement.TryRemoveEventHandler(PKey.Wizard.MovingNext)

    if props |> Props.exists PKey.Wizard.StepChanged then
      terminalElement.TryRemoveEventHandler(PKey.Wizard.StepChanged)

    if props |> Props.exists PKey.Wizard.StepChanging then
      terminalElement.TryRemoveEventHandler(PKey.Wizard.StepChanging)

type internal WizardStepPropHandler =
  static member setProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    ViewPropHandler.setProps (terminalElement, props)

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

  static member removeProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    ViewPropHandler.removeProps (terminalElement, props)

    let view = terminalElement.View :?> WizardStep

    // Properties
    props
    |> Props.tryFind PKey.WizardStep.BackButtonText
    |> Option.iter (fun _ -> view.BackButtonText <- "")

    props
    |> Props.tryFind PKey.WizardStep.HelpText
    |> Option.iter (fun _ -> view.HelpText <- "")

    props
    |> Props.tryFind PKey.WizardStep.NextButtonText
    |> Option.iter (fun _ -> view.NextButtonText <- "")
