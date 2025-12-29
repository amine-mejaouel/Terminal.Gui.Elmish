namespace Terminal.Gui.Elmish

open System
open System.Collections.ObjectModel
open Terminal.Gui.ViewBase
open Terminal.Gui.Views


type internal ViewTerminalElement(props: Props) =

  override _.name = "View"

  override _.newView() = new Terminal.Gui.ViewBase.View()

  override _.setProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.setProps(terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> Terminal.Gui.ViewBase.View

    // Properties
    props
    |> Props.tryFind PKey.view.margin
    |> Option.iter (fun v -> view.Margin <- v)

    props
    |> Props.tryFind PKey.view.shadowStyle
    |> Option.iter (fun v -> view.ShadowStyle <- v)

    props
    |> Props.tryFind PKey.view.border
    |> Option.iter (fun v -> view.Border <- v)

    props
    |> Props.tryFind PKey.view.borderStyle
    |> Option.iter (fun v -> view.BorderStyle <- v)

    props
    |> Props.tryFind PKey.view.padding
    |> Option.iter (fun v -> view.Padding <- v)

    props
    |> Props.tryFind PKey.view.arrangement
    |> Option.iter (fun v -> view.Arrangement <- v)

    props
    |> Props.tryFind PKey.view.contentSizeTracksViewport
    |> Option.iter (fun v -> view.ContentSizeTracksViewport <- v)

    props
    |> Props.tryFind PKey.view.viewportSettings
    |> Option.iter (fun v -> view.ViewportSettings <- v)

    props
    |> Props.tryFind PKey.view.viewport
    |> Option.iter (fun v -> view.Viewport <- v)

    props
    |> Props.tryFind PKey.view.data
    |> Option.iter (fun v -> view.Data <- v)

    props
    |> Props.tryFind PKey.view.id
    |> Option.iter (fun v -> view.Id <- v)

    props
    |> Props.tryFind PKey.view.app
    |> Option.iter (fun v -> view.App <- v)

    props
    |> Props.tryFind PKey.view.isInitialized
    |> Option.iter (fun v -> view.IsInitialized <- v)

    props
    |> Props.tryFind PKey.view.enabled
    |> Option.iter (fun v -> view.Enabled <- v)

    props
    |> Props.tryFind PKey.view.visible
    |> Option.iter (fun v -> view.Visible <- v)

    props
    |> Props.tryFind PKey.view.title
    |> Option.iter (fun v -> view.Title <- v)

    props
    |> Props.tryFind PKey.view.cursorVisibility
    |> Option.iter (fun v -> view.CursorVisibility <- v)

    props
    |> Props.tryFind PKey.view.superViewRendersLineCanvas
    |> Option.iter (fun v -> view.SuperViewRendersLineCanvas <- v)

    props
    |> Props.tryFind PKey.view.schemeName
    |> Option.iter (fun v -> view.SchemeName <- v)

    props
    |> Props.tryFind PKey.view.superView
    |> Option.iter (fun v -> view.SuperView <- v)

    props
    |> Props.tryFind PKey.view.hotKey
    |> Option.iter (fun v -> view.HotKey <- v)

    props
    |> Props.tryFind PKey.view.hotKeySpecifier
    |> Option.iter (fun v -> view.HotKeySpecifier <- v)

    props
    |> Props.tryFind PKey.view.keyBindings
    |> Option.iter (fun v -> view.KeyBindings <- v)

    props
    |> Props.tryFind PKey.view.hotKeyBindings
    |> Option.iter (fun v -> view.HotKeyBindings <- v)

    props
    |> Props.tryFind PKey.view.frame
    |> Option.iter (fun v -> view.Frame <- v)

    props
    |> Props.tryFind PKey.view.x
    |> Option.iter (fun v -> view.X <- v)

    props
    |> Props.tryFind PKey.view.y
    |> Option.iter (fun v -> view.Y <- v)

    props
    |> Props.tryFind PKey.view.height
    |> Option.iter (fun v -> view.Height <- v)

    props
    |> Props.tryFind PKey.view.width
    |> Option.iter (fun v -> view.Width <- v)

    props
    |> Props.tryFind PKey.view.needsLayout
    |> Option.iter (fun v -> view.NeedsLayout <- v)

    props
    |> Props.tryFind PKey.view.validatePosDim
    |> Option.iter (fun v -> view.ValidatePosDim <- v)

    props
    |> Props.tryFind PKey.view.mouseHeldDown
    |> Option.iter (fun v -> view.MouseHeldDown <- v)

    props
    |> Props.tryFind PKey.view.mouseBindings
    |> Option.iter (fun v -> view.MouseBindings <- v)

    props
    |> Props.tryFind PKey.view.wantContinuousButtonPressed
    |> Option.iter (fun v -> view.WantContinuousButtonPressed <- v)

    props
    |> Props.tryFind PKey.view.wantMousePositionReports
    |> Option.iter (fun v -> view.WantMousePositionReports <- v)

    props
    |> Props.tryFind PKey.view.mouseState
    |> Option.iter (fun v -> view.MouseState <- v)

    props
    |> Props.tryFind PKey.view.highlightStates
    |> Option.iter (fun v -> view.HighlightStates <- v)

    props
    |> Props.tryFind PKey.view.canFocus
    |> Option.iter (fun v -> view.CanFocus <- v)

    props
    |> Props.tryFind PKey.view.hasFocus
    |> Option.iter (fun v -> view.HasFocus <- v)

    props
    |> Props.tryFind PKey.view.tabStop
    |> Option.iter (fun v -> view.TabStop <- v)

    props
    |> Props.tryFind PKey.view.preserveTrailingSpaces
    |> Option.iter (fun v -> view.PreserveTrailingSpaces <- v)

    props
    |> Props.tryFind PKey.view.text
    |> Option.iter (fun v -> view.Text <- v)

    props
    |> Props.tryFind PKey.view.textAlignment
    |> Option.iter (fun v -> view.TextAlignment <- v)

    props
    |> Props.tryFind PKey.view.textDirection
    |> Option.iter (fun v -> view.TextDirection <- v)

    props
    |> Props.tryFind PKey.view.textFormatter
    |> Option.iter (fun v -> view.TextFormatter <- v)

    props
    |> Props.tryFind PKey.view.verticalTextAlignment
    |> Option.iter (fun v -> view.VerticalTextAlignment <- v)

    // Events
    terminalElement.trySetEventHandler(PKey.view.borderStyleChanged, view.BorderStyleChanged)

    terminalElement.trySetEventHandler(PKey.view.commandNotBound, view.CommandNotBound)

    terminalElement.trySetEventHandler(PKey.view.accepting, view.Accepting)

    terminalElement.trySetEventHandler(PKey.view.accepted, view.Accepted)

    terminalElement.trySetEventHandler(PKey.view.activating, view.Activating)

    terminalElement.trySetEventHandler(PKey.view.handlingHotKey, view.HandlingHotKey)

    terminalElement.trySetEventHandler(PKey.view.contentSizeChanged, view.ContentSizeChanged)

    terminalElement.trySetEventHandler(PKey.view.viewportChanged, view.ViewportChanged)

    terminalElement.trySetEventHandler(PKey.view.disposing, view.Disposing)

    terminalElement.trySetEventHandler(PKey.view.initialized, view.Initialized)

    terminalElement.trySetEventHandler(PKey.view.enabledChanged, view.EnabledChanged)

    terminalElement.trySetEventHandler(PKey.view.visibleChanging, view.VisibleChanging)

    terminalElement.trySetEventHandler(PKey.view.visibleChanged, view.VisibleChanged)

    terminalElement.trySetEventHandler(PKey.view.titleChanged, view.TitleChanged)

    terminalElement.trySetEventHandler(PKey.view.titleChanging, view.TitleChanging)

    terminalElement.trySetEventHandler(PKey.view.gettingAttributeForRole, view.GettingAttributeForRole)

    terminalElement.trySetEventHandler(PKey.view.clearingViewport, view.ClearingViewport)

    terminalElement.trySetEventHandler(PKey.view.clearedViewport, view.ClearedViewport)

    terminalElement.trySetEventHandler(PKey.view.drawingText, view.DrawingText)

    terminalElement.trySetEventHandler(PKey.view.drewText, view.DrewText)

    terminalElement.trySetEventHandler(PKey.view.drawingContent, view.DrawingContent)

    terminalElement.trySetEventHandler(PKey.view.drawingSubViews, view.DrawingSubViews)

    terminalElement.trySetEventHandler(PKey.view.drawComplete, view.DrawComplete)

    terminalElement.trySetEventHandler(PKey.view.schemeNameChanging, view.SchemeNameChanging)

    terminalElement.trySetEventHandler(PKey.view.schemeNameChanged, view.SchemeNameChanged)

    terminalElement.trySetEventHandler(PKey.view.gettingScheme, view.GettingScheme)

    terminalElement.trySetEventHandler(PKey.view.schemeChanging, view.SchemeChanging)

    terminalElement.trySetEventHandler(PKey.view.schemeChanged, view.SchemeChanged)

    terminalElement.trySetEventHandler(PKey.view.superViewChanged, view.SuperViewChanged)

    terminalElement.trySetEventHandler(PKey.view.subViewAdded, view.SubViewAdded)

    terminalElement.trySetEventHandler(PKey.view.subViewRemoved, view.SubViewRemoved)

    terminalElement.trySetEventHandler(PKey.view.removed, view.Removed)

    terminalElement.trySetEventHandler(PKey.view.hotKeyChanged, view.HotKeyChanged)

    terminalElement.trySetEventHandler(PKey.view.keyDown, view.KeyDown)

    terminalElement.trySetEventHandler(PKey.view.keyDownNotHandled, view.KeyDownNotHandled)

    terminalElement.trySetEventHandler(PKey.view.keyUp, view.KeyUp)

    terminalElement.trySetEventHandler(PKey.view.frameChanged, view.FrameChanged)

    terminalElement.trySetEventHandler(PKey.view.heightChanging, view.HeightChanging)

    terminalElement.trySetEventHandler(PKey.view.heightChanged, view.HeightChanged)

    terminalElement.trySetEventHandler(PKey.view.widthChanging, view.WidthChanging)

    terminalElement.trySetEventHandler(PKey.view.widthChanged, view.WidthChanged)

    terminalElement.trySetEventHandler(PKey.view.subViewLayout, view.SubViewLayout)

    terminalElement.trySetEventHandler(PKey.view.subViewsLaidOut, view.SubViewsLaidOut)

    terminalElement.trySetEventHandler(PKey.view.mouseEnter, view.MouseEnter)

    terminalElement.trySetEventHandler(PKey.view.mouseLeave, view.MouseLeave)

    terminalElement.trySetEventHandler(PKey.view.mouseEvent, view.MouseEvent)

    terminalElement.trySetEventHandler(PKey.view.mouseWheel, view.MouseWheel)

    terminalElement.trySetEventHandler(PKey.view.mouseStateChanged, view.MouseStateChanged)

    terminalElement.trySetEventHandler(PKey.view.advancingFocus, view.AdvancingFocus)

    terminalElement.trySetEventHandler(PKey.view.canFocusChanged, view.CanFocusChanged)

    terminalElement.trySetEventHandler(PKey.view.focusedChanged, view.FocusedChanged)

    terminalElement.trySetEventHandler(PKey.view.hasFocusChanging, view.HasFocusChanging)

    terminalElement.trySetEventHandler(PKey.view.hasFocusChanged, view.HasFocusChanged)

    terminalElement.trySetEventHandler(PKey.view.textChanged, view.TextChanged)


  override _.removeProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.removeProps(terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> Terminal.Gui.ViewBase.View

    // Properties
    props
    |> Props.tryFind PKey.view.margin
    |> Option.iter (fun _ -> 
        view.Margin <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.view.shadowStyle
    |> Option.iter (fun _ -> 
        view.ShadowStyle <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.view.border
    |> Option.iter (fun _ -> 
        view.Border <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.view.borderStyle
    |> Option.iter (fun _ -> 
        view.BorderStyle <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.view.padding
    |> Option.iter (fun _ -> 
        view.Padding <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.view.arrangement
    |> Option.iter (fun _ -> 
        view.Arrangement <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.view.contentSizeTracksViewport
    |> Option.iter (fun _ -> 
        view.ContentSizeTracksViewport <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.view.viewportSettings
    |> Option.iter (fun _ -> 
        view.ViewportSettings <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.view.viewport
    |> Option.iter (fun _ -> 
        view.Viewport <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.view.data
    |> Option.iter (fun _ -> 
        view.Data <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.view.id
    |> Option.iter (fun _ -> 
        view.Id <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.view.app
    |> Option.iter (fun _ -> 
        view.App <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.view.isInitialized
    |> Option.iter (fun _ -> 
        view.IsInitialized <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.view.enabled
    |> Option.iter (fun _ -> 
        view.Enabled <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.view.visible
    |> Option.iter (fun _ -> 
        view.Visible <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.view.title
    |> Option.iter (fun _ -> 
        view.Title <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.view.cursorVisibility
    |> Option.iter (fun _ -> 
        view.CursorVisibility <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.view.superViewRendersLineCanvas
    |> Option.iter (fun _ -> 
        view.SuperViewRendersLineCanvas <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.view.schemeName
    |> Option.iter (fun _ -> 
        view.SchemeName <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.view.superView
    |> Option.iter (fun _ -> 
        view.SuperView <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.view.hotKey
    |> Option.iter (fun _ -> 
        view.HotKey <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.view.hotKeySpecifier
    |> Option.iter (fun _ -> 
        view.HotKeySpecifier <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.view.keyBindings
    |> Option.iter (fun _ -> 
        view.KeyBindings <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.view.hotKeyBindings
    |> Option.iter (fun _ -> 
        view.HotKeyBindings <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.view.frame
    |> Option.iter (fun _ -> 
        view.Frame <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.view.x
    |> Option.iter (fun _ -> 
        view.X <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.view.y
    |> Option.iter (fun _ -> 
        view.Y <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.view.height
    |> Option.iter (fun _ -> 
        view.Height <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.view.width
    |> Option.iter (fun _ -> 
        view.Width <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.view.needsLayout
    |> Option.iter (fun _ -> 
        view.NeedsLayout <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.view.validatePosDim
    |> Option.iter (fun _ -> 
        view.ValidatePosDim <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.view.mouseHeldDown
    |> Option.iter (fun _ -> 
        view.MouseHeldDown <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.view.mouseBindings
    |> Option.iter (fun _ -> 
        view.MouseBindings <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.view.wantContinuousButtonPressed
    |> Option.iter (fun _ -> 
        view.WantContinuousButtonPressed <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.view.wantMousePositionReports
    |> Option.iter (fun _ -> 
        view.WantMousePositionReports <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.view.mouseState
    |> Option.iter (fun _ -> 
        view.MouseState <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.view.highlightStates
    |> Option.iter (fun _ -> 
        view.HighlightStates <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.view.canFocus
    |> Option.iter (fun _ -> 
        view.CanFocus <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.view.hasFocus
    |> Option.iter (fun _ -> 
        view.HasFocus <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.view.tabStop
    |> Option.iter (fun _ -> 
        view.TabStop <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.view.preserveTrailingSpaces
    |> Option.iter (fun _ -> 
        view.PreserveTrailingSpaces <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.view.text
    |> Option.iter (fun _ -> 
        view.Text <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.view.textAlignment
    |> Option.iter (fun _ -> 
        view.TextAlignment <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.view.textDirection
    |> Option.iter (fun _ -> 
        view.TextDirection <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.view.textFormatter
    |> Option.iter (fun _ -> 
        view.TextFormatter <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.view.verticalTextAlignment
    |> Option.iter (fun _ -> 
        view.VerticalTextAlignment <- Unchecked.defaultof<_>)

    // Events
    terminalElement.tryRemoveEventHandler PKey.View.BorderStyleChanged
    terminalElement.tryRemoveEventHandler PKey.View.CommandNotBound
    terminalElement.tryRemoveEventHandler PKey.View.Accepting
    terminalElement.tryRemoveEventHandler PKey.View.Accepted
    terminalElement.tryRemoveEventHandler PKey.View.Activating
    terminalElement.tryRemoveEventHandler PKey.View.HandlingHotKey
    terminalElement.tryRemoveEventHandler PKey.View.ContentSizeChanged
    terminalElement.tryRemoveEventHandler PKey.View.ViewportChanged
    terminalElement.tryRemoveEventHandler PKey.View.Disposing
    terminalElement.tryRemoveEventHandler PKey.View.Initialized
    terminalElement.tryRemoveEventHandler PKey.View.EnabledChanged
    terminalElement.tryRemoveEventHandler PKey.View.VisibleChanging
    terminalElement.tryRemoveEventHandler PKey.View.VisibleChanged
    terminalElement.tryRemoveEventHandler PKey.View.TitleChanged
    terminalElement.tryRemoveEventHandler PKey.View.TitleChanging
    terminalElement.tryRemoveEventHandler PKey.View.GettingAttributeForRole
    terminalElement.tryRemoveEventHandler PKey.View.ClearingViewport
    terminalElement.tryRemoveEventHandler PKey.View.ClearedViewport
    terminalElement.tryRemoveEventHandler PKey.View.DrawingText
    terminalElement.tryRemoveEventHandler PKey.View.DrewText
    terminalElement.tryRemoveEventHandler PKey.View.DrawingContent
    terminalElement.tryRemoveEventHandler PKey.View.DrawingSubViews
    terminalElement.tryRemoveEventHandler PKey.View.DrawComplete
    terminalElement.tryRemoveEventHandler PKey.View.SchemeNameChanging
    terminalElement.tryRemoveEventHandler PKey.View.SchemeNameChanged
    terminalElement.tryRemoveEventHandler PKey.View.GettingScheme
    terminalElement.tryRemoveEventHandler PKey.View.SchemeChanging
    terminalElement.tryRemoveEventHandler PKey.View.SchemeChanged
    terminalElement.tryRemoveEventHandler PKey.View.SuperViewChanged
    terminalElement.tryRemoveEventHandler PKey.View.SubViewAdded
    terminalElement.tryRemoveEventHandler PKey.View.SubViewRemoved
    terminalElement.tryRemoveEventHandler PKey.View.Removed
    terminalElement.tryRemoveEventHandler PKey.View.HotKeyChanged
    terminalElement.tryRemoveEventHandler PKey.View.KeyDown
    terminalElement.tryRemoveEventHandler PKey.View.KeyDownNotHandled
    terminalElement.tryRemoveEventHandler PKey.View.KeyUp
    terminalElement.tryRemoveEventHandler PKey.View.FrameChanged
    terminalElement.tryRemoveEventHandler PKey.View.HeightChanging
    terminalElement.tryRemoveEventHandler PKey.View.HeightChanged
    terminalElement.tryRemoveEventHandler PKey.View.WidthChanging
    terminalElement.tryRemoveEventHandler PKey.View.WidthChanged
    terminalElement.tryRemoveEventHandler PKey.View.SubViewLayout
    terminalElement.tryRemoveEventHandler PKey.View.SubViewsLaidOut
    terminalElement.tryRemoveEventHandler PKey.View.MouseEnter
    terminalElement.tryRemoveEventHandler PKey.View.MouseLeave
    terminalElement.tryRemoveEventHandler PKey.View.MouseEvent
    terminalElement.tryRemoveEventHandler PKey.View.MouseWheel
    terminalElement.tryRemoveEventHandler PKey.View.MouseStateChanged
    terminalElement.tryRemoveEventHandler PKey.View.AdvancingFocus
    terminalElement.tryRemoveEventHandler PKey.View.CanFocusChanged
    terminalElement.tryRemoveEventHandler PKey.View.FocusedChanged
    terminalElement.tryRemoveEventHandler PKey.View.HasFocusChanging
    terminalElement.tryRemoveEventHandler PKey.View.HasFocusChanged
    terminalElement.tryRemoveEventHandler PKey.View.TextChanged

type internal AdornmentTerminalElement(props: Props) =
  inherit ViewTerminalElement(props)

  override _.name = "Adornment"

  override _.newView() = new Terminal.Gui.ViewBase.Adornment()

  override _.setProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.setProps(terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> Terminal.Gui.ViewBase.Adornment

    // Properties
    props
    |> Props.tryFind PKey.adornment.parent
    |> Option.iter (fun v -> view.Parent <- v)

    props
    |> Props.tryFind PKey.adornment.diagnostics
    |> Option.iter (fun v -> view.Diagnostics <- v)

    props
    |> Props.tryFind PKey.adornment.thickness
    |> Option.iter (fun v -> view.Thickness <- v)

    props
    |> Props.tryFind PKey.adornment.viewport
    |> Option.iter (fun v -> view.Viewport <- v)

    props
    |> Props.tryFind PKey.adornment.superViewRendersLineCanvas
    |> Option.iter (fun v -> view.SuperViewRendersLineCanvas <- v)

    // Events
    terminalElement.trySetEventHandler(PKey.adornment.thicknessChanged, view.ThicknessChanged)


  override _.removeProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.removeProps(terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> Terminal.Gui.ViewBase.Adornment

    // Properties
    props
    |> Props.tryFind PKey.adornment.parent
    |> Option.iter (fun _ -> 
        view.Parent <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.adornment.diagnostics
    |> Option.iter (fun _ -> 
        view.Diagnostics <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.adornment.thickness
    |> Option.iter (fun _ -> 
        view.Thickness <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.adornment.viewport
    |> Option.iter (fun _ -> 
        view.Viewport <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.adornment.superViewRendersLineCanvas
    |> Option.iter (fun _ -> 
        view.SuperViewRendersLineCanvas <- Unchecked.defaultof<_>)

    // Events
    terminalElement.tryRemoveEventHandler PKey.Adornment.ThicknessChanged

type internal BBarTerminalElement(props: Props) =
  inherit ColorBarTerminalElement(props)

  override _.name = "BBar"

  override _.newView() = new Terminal.Gui.Views.BBar()

  override _.setProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.setProps(terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> Terminal.Gui.Views.BBar

    // Properties
    props
    |> Props.tryFind PKey.bBar.gBar
    |> Option.iter (fun v -> view.GBar <- v)

    props
    |> Props.tryFind PKey.bBar.rBar
    |> Option.iter (fun v -> view.RBar <- v)

    // Events

  override _.removeProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.removeProps(terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> Terminal.Gui.Views.BBar

    // Properties
    props
    |> Props.tryFind PKey.bBar.gBar
    |> Option.iter (fun _ -> 
        view.GBar <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.bBar.rBar
    |> Option.iter (fun _ -> 
        view.RBar <- Unchecked.defaultof<_>)


type internal BarTerminalElement(props: Props) =
  inherit ViewTerminalElement(props)

  override _.name = "Bar"

  override _.newView() = new Terminal.Gui.Views.Bar()

  override _.setProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.setProps(terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> Terminal.Gui.Views.Bar

    // Properties
    props
    |> Props.tryFind PKey.bar.orientation
    |> Option.iter (fun v -> view.Orientation <- v)

    props
    |> Props.tryFind PKey.bar.alignmentModes
    |> Option.iter (fun v -> view.AlignmentModes <- v)

    // Events
    terminalElement.trySetEventHandler(PKey.bar.orientationChanging, view.OrientationChanging)

    terminalElement.trySetEventHandler(PKey.bar.orientationChanged, view.OrientationChanged)


  override _.removeProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.removeProps(terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> Terminal.Gui.Views.Bar

    // Properties
    props
    |> Props.tryFind PKey.bar.orientation
    |> Option.iter (fun _ -> 
        view.Orientation <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.bar.alignmentModes
    |> Option.iter (fun _ -> 
        view.AlignmentModes <- Unchecked.defaultof<_>)

    // Events
    terminalElement.tryRemoveEventHandler PKey.Bar.OrientationChanging
    terminalElement.tryRemoveEventHandler PKey.Bar.OrientationChanged

type internal BorderTerminalElement(props: Props) =
  inherit AdornmentTerminalElement(props)

  override _.name = "Border"

  override _.newView() = new Terminal.Gui.ViewBase.Border()

  override _.setProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.setProps(terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> Terminal.Gui.ViewBase.Border

    // Properties
    props
    |> Props.tryFind PKey.border.lineStyle
    |> Option.iter (fun v -> view.LineStyle <- v)

    props
    |> Props.tryFind PKey.border.settings
    |> Option.iter (fun v -> view.Settings <- v)

    props
    |> Props.tryFind PKey.border.drawIndicator
    |> Option.iter (fun v -> view.DrawIndicator <- v)

    // Events

  override _.removeProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.removeProps(terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> Terminal.Gui.ViewBase.Border

    // Properties
    props
    |> Props.tryFind PKey.border.lineStyle
    |> Option.iter (fun _ -> 
        view.LineStyle <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.border.settings
    |> Option.iter (fun _ -> 
        view.Settings <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.border.drawIndicator
    |> Option.iter (fun _ -> 
        view.DrawIndicator <- Unchecked.defaultof<_>)


type internal ButtonTerminalElement(props: Props) =
  inherit ViewTerminalElement(props)

  override _.name = "Button"

  override _.newView() = new Terminal.Gui.Views.Button()

  override _.setProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.setProps(terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> Terminal.Gui.Views.Button

    // Properties
    props
    |> Props.tryFind PKey.button.text
    |> Option.iter (fun v -> view.Text <- v)

    props
    |> Props.tryFind PKey.button.hotKeySpecifier
    |> Option.iter (fun v -> view.HotKeySpecifier <- v)

    props
    |> Props.tryFind PKey.button.isDefault
    |> Option.iter (fun v -> view.IsDefault <- v)

    props
    |> Props.tryFind PKey.button.noDecorations
    |> Option.iter (fun v -> view.NoDecorations <- v)

    props
    |> Props.tryFind PKey.button.noPadding
    |> Option.iter (fun v -> view.NoPadding <- v)

    // Events

  override _.removeProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.removeProps(terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> Terminal.Gui.Views.Button

    // Properties
    props
    |> Props.tryFind PKey.button.text
    |> Option.iter (fun _ -> 
        view.Text <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.button.hotKeySpecifier
    |> Option.iter (fun _ -> 
        view.HotKeySpecifier <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.button.isDefault
    |> Option.iter (fun _ -> 
        view.IsDefault <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.button.noDecorations
    |> Option.iter (fun _ -> 
        view.NoDecorations <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.button.noPadding
    |> Option.iter (fun _ -> 
        view.NoPadding <- Unchecked.defaultof<_>)


type internal CharMapTerminalElement(props: Props) =
  inherit ViewTerminalElement(props)

  override _.name = "CharMap"

  override _.newView() = new Terminal.Gui.Views.CharMap()

  override _.setProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.setProps(terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> Terminal.Gui.Views.CharMap

    // Properties
    props
    |> Props.tryFind PKey.charMap.selectedCodePoint
    |> Option.iter (fun v -> view.SelectedCodePoint <- v)

    props
    |> Props.tryFind PKey.charMap.showGlyphWidths
    |> Option.iter (fun v -> view.ShowGlyphWidths <- v)

    props
    |> Props.tryFind PKey.charMap.startCodePoint
    |> Option.iter (fun v -> view.StartCodePoint <- v)

    props
    |> Props.tryFind PKey.charMap.showUnicodeCategory
    |> Option.iter (fun v -> view.ShowUnicodeCategory <- v)

    // Events
    terminalElement.trySetEventHandler(PKey.charMap.selectedCodePointChanged, view.SelectedCodePointChanged)


  override _.removeProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.removeProps(terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> Terminal.Gui.Views.CharMap

    // Properties
    props
    |> Props.tryFind PKey.charMap.selectedCodePoint
    |> Option.iter (fun _ -> 
        view.SelectedCodePoint <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.charMap.showGlyphWidths
    |> Option.iter (fun _ -> 
        view.ShowGlyphWidths <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.charMap.startCodePoint
    |> Option.iter (fun _ -> 
        view.StartCodePoint <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.charMap.showUnicodeCategory
    |> Option.iter (fun _ -> 
        view.ShowUnicodeCategory <- Unchecked.defaultof<_>)

    // Events
    terminalElement.tryRemoveEventHandler PKey.CharMap.SelectedCodePointChanged

type internal CheckBoxTerminalElement(props: Props) =
  inherit ViewTerminalElement(props)

  override _.name = "CheckBox"

  override _.newView() = new Terminal.Gui.Views.CheckBox()

  override _.setProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.setProps(terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> Terminal.Gui.Views.CheckBox

    // Properties
    props
    |> Props.tryFind PKey.checkBox.text
    |> Option.iter (fun v -> view.Text <- v)

    props
    |> Props.tryFind PKey.checkBox.hotKeySpecifier
    |> Option.iter (fun v -> view.HotKeySpecifier <- v)

    props
    |> Props.tryFind PKey.checkBox.allowCheckStateNone
    |> Option.iter (fun v -> view.AllowCheckStateNone <- v)

    props
    |> Props.tryFind PKey.checkBox.checkedState
    |> Option.iter (fun v -> view.CheckedState <- v)

    props
    |> Props.tryFind PKey.checkBox.radioStyle
    |> Option.iter (fun v -> view.RadioStyle <- v)

    // Events
    terminalElement.trySetEventHandler(PKey.checkBox.checkedStateChanging, view.CheckedStateChanging)

    terminalElement.trySetEventHandler(PKey.checkBox.checkedStateChanged, view.CheckedStateChanged)


  override _.removeProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.removeProps(terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> Terminal.Gui.Views.CheckBox

    // Properties
    props
    |> Props.tryFind PKey.checkBox.text
    |> Option.iter (fun _ -> 
        view.Text <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.checkBox.hotKeySpecifier
    |> Option.iter (fun _ -> 
        view.HotKeySpecifier <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.checkBox.allowCheckStateNone
    |> Option.iter (fun _ -> 
        view.AllowCheckStateNone <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.checkBox.checkedState
    |> Option.iter (fun _ -> 
        view.CheckedState <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.checkBox.radioStyle
    |> Option.iter (fun _ -> 
        view.RadioStyle <- Unchecked.defaultof<_>)

    // Events
    terminalElement.tryRemoveEventHandler PKey.CheckBox.CheckedStateChanging
    terminalElement.tryRemoveEventHandler PKey.CheckBox.CheckedStateChanged

type internal ColorBarTerminalElement(props: Props) =
  inherit ViewTerminalElement(props)

  override _.name = "ColorBar"

  override _.newView() = new Terminal.Gui.Views.ColorBar()

  override _.setProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.setProps(terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> Terminal.Gui.Views.ColorBar

    // Properties
    props
    |> Props.tryFind PKey.colorBar.value
    |> Option.iter (fun v -> view.Value <- v)

    // Events
    terminalElement.trySetEventHandler(PKey.colorBar.valueChanged, view.ValueChanged)


  override _.removeProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.removeProps(terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> Terminal.Gui.Views.ColorBar

    // Properties
    props
    |> Props.tryFind PKey.colorBar.value
    |> Option.iter (fun _ -> 
        view.Value <- Unchecked.defaultof<_>)

    // Events
    terminalElement.tryRemoveEventHandler PKey.ColorBar.ValueChanged

type internal ColorPickerTerminalElement(props: Props) =
  inherit ViewTerminalElement(props)

  override _.name = "ColorPicker"

  override _.newView() = new Terminal.Gui.Views.ColorPicker()

  override _.setProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.setProps(terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> Terminal.Gui.Views.ColorPicker

    // Properties
    props
    |> Props.tryFind PKey.colorPicker.selectedColor
    |> Option.iter (fun v -> view.SelectedColor <- v)

    props
    |> Props.tryFind PKey.colorPicker.style
    |> Option.iter (fun v -> view.Style <- v)

    // Events
    terminalElement.trySetEventHandler(PKey.colorPicker.colorChanged, view.ColorChanged)


  override _.removeProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.removeProps(terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> Terminal.Gui.Views.ColorPicker

    // Properties
    props
    |> Props.tryFind PKey.colorPicker.selectedColor
    |> Option.iter (fun _ -> 
        view.SelectedColor <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.colorPicker.style
    |> Option.iter (fun _ -> 
        view.Style <- Unchecked.defaultof<_>)

    // Events
    terminalElement.tryRemoveEventHandler PKey.ColorPicker.ColorChanged

type internal ColorPicker16TerminalElement(props: Props) =
  inherit ViewTerminalElement(props)

  override _.name = "ColorPicker16"

  override _.newView() = new Terminal.Gui.Views.ColorPicker16()

  override _.setProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.setProps(terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> Terminal.Gui.Views.ColorPicker16

    // Properties
    props
    |> Props.tryFind PKey.colorPicker16.boxHeight
    |> Option.iter (fun v -> view.BoxHeight <- v)

    props
    |> Props.tryFind PKey.colorPicker16.boxWidth
    |> Option.iter (fun v -> view.BoxWidth <- v)

    props
    |> Props.tryFind PKey.colorPicker16.cursor
    |> Option.iter (fun v -> view.Cursor <- v)

    props
    |> Props.tryFind PKey.colorPicker16.selectedColor
    |> Option.iter (fun v -> view.SelectedColor <- v)

    // Events
    terminalElement.trySetEventHandler(PKey.colorPicker16.colorChanged, view.ColorChanged)


  override _.removeProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.removeProps(terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> Terminal.Gui.Views.ColorPicker16

    // Properties
    props
    |> Props.tryFind PKey.colorPicker16.boxHeight
    |> Option.iter (fun _ -> 
        view.BoxHeight <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.colorPicker16.boxWidth
    |> Option.iter (fun _ -> 
        view.BoxWidth <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.colorPicker16.cursor
    |> Option.iter (fun _ -> 
        view.Cursor <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.colorPicker16.selectedColor
    |> Option.iter (fun _ -> 
        view.SelectedColor <- Unchecked.defaultof<_>)

    // Events
    terminalElement.tryRemoveEventHandler PKey.ColorPicker16.ColorChanged

type internal ComboBoxTerminalElement(props: Props) =
  inherit ViewTerminalElement(props)

  override _.name = "ComboBox"

  override _.newView() = new Terminal.Gui.Views.ComboBox()

  override _.setProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.setProps(terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> Terminal.Gui.Views.ComboBox

    // Properties
    props
    |> Props.tryFind PKey.comboBox.hideDropdownListOnClick
    |> Option.iter (fun v -> view.HideDropdownListOnClick <- v)

    props
    |> Props.tryFind PKey.comboBox.isShow
    |> Option.iter (fun v -> view.IsShow <- v)

    props
    |> Props.tryFind PKey.comboBox.readOnly
    |> Option.iter (fun v -> view.ReadOnly <- v)

    props
    |> Props.tryFind PKey.comboBox.searchText
    |> Option.iter (fun v -> view.SearchText <- v)

    props
    |> Props.tryFind PKey.comboBox.selectedItem
    |> Option.iter (fun v -> view.SelectedItem <- v)

    props
    |> Props.tryFind PKey.comboBox.source
    |> Option.iter (fun v -> view.Source <- v)

    props
    |> Props.tryFind PKey.comboBox.text
    |> Option.iter (fun v -> view.Text <- v)

    // Events
    terminalElement.trySetEventHandler(PKey.comboBox.collapsed, view.Collapsed)

    terminalElement.trySetEventHandler(PKey.comboBox.expanded, view.Expanded)

    terminalElement.trySetEventHandler(PKey.comboBox.openSelectedItem, view.OpenSelectedItem)

    terminalElement.trySetEventHandler(PKey.comboBox.selectedItemChanged, view.SelectedItemChanged)


  override _.removeProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.removeProps(terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> Terminal.Gui.Views.ComboBox

    // Properties
    props
    |> Props.tryFind PKey.comboBox.hideDropdownListOnClick
    |> Option.iter (fun _ -> 
        view.HideDropdownListOnClick <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.comboBox.isShow
    |> Option.iter (fun _ -> 
        view.IsShow <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.comboBox.readOnly
    |> Option.iter (fun _ -> 
        view.ReadOnly <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.comboBox.searchText
    |> Option.iter (fun _ -> 
        view.SearchText <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.comboBox.selectedItem
    |> Option.iter (fun _ -> 
        view.SelectedItem <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.comboBox.source
    |> Option.iter (fun _ -> 
        view.Source <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.comboBox.text
    |> Option.iter (fun _ -> 
        view.Text <- Unchecked.defaultof<_>)

    // Events
    terminalElement.tryRemoveEventHandler PKey.ComboBox.Collapsed
    terminalElement.tryRemoveEventHandler PKey.ComboBox.Expanded
    terminalElement.tryRemoveEventHandler PKey.ComboBox.OpenSelectedItem
    terminalElement.tryRemoveEventHandler PKey.ComboBox.SelectedItemChanged

type internal ComboListViewTerminalElement(props: Props) =
  inherit ListViewTerminalElement(props)

  override _.name = "ComboListView"

  override _.newView() = new Terminal.Gui.Views.ComboBox+ComboListView()

  override _.setProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.setProps(terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> Terminal.Gui.Views.ComboBox+ComboListView

    // Properties
    props
    |> Props.tryFind PKey.comboListView.hideDropdownListOnClick
    |> Option.iter (fun v -> view.HideDropdownListOnClick <- v)

    // Events

  override _.removeProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.removeProps(terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> Terminal.Gui.Views.ComboBox+ComboListView

    // Properties
    props
    |> Props.tryFind PKey.comboListView.hideDropdownListOnClick
    |> Option.iter (fun _ -> 
        view.HideDropdownListOnClick <- Unchecked.defaultof<_>)


type internal DateFieldTerminalElement(props: Props) =
  inherit TextFieldTerminalElement(props)

  override _.name = "DateField"

  override _.newView() = new Terminal.Gui.Views.DateField()

  override _.setProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.setProps(terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> Terminal.Gui.Views.DateField

    // Properties
    props
    |> Props.tryFind PKey.dateField.culture
    |> Option.iter (fun v -> view.Culture <- v)

    props
    |> Props.tryFind PKey.dateField.cursorPosition
    |> Option.iter (fun v -> view.CursorPosition <- v)

    props
    |> Props.tryFind PKey.dateField.date
    |> Option.iter (fun v -> view.Date <- v)

    // Events
    terminalElement.trySetEventHandler(PKey.dateField.dateChanged, view.DateChanged)


  override _.removeProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.removeProps(terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> Terminal.Gui.Views.DateField

    // Properties
    props
    |> Props.tryFind PKey.dateField.culture
    |> Option.iter (fun _ -> 
        view.Culture <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.dateField.cursorPosition
    |> Option.iter (fun _ -> 
        view.CursorPosition <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.dateField.date
    |> Option.iter (fun _ -> 
        view.Date <- Unchecked.defaultof<_>)

    // Events
    terminalElement.tryRemoveEventHandler PKey.DateField.DateChanged

type internal DatePickerTerminalElement(props: Props) =
  inherit ViewTerminalElement(props)

  override _.name = "DatePicker"

  override _.newView() = new Terminal.Gui.Views.DatePicker()

  override _.setProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.setProps(terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> Terminal.Gui.Views.DatePicker

    // Properties
    props
    |> Props.tryFind PKey.datePicker.culture
    |> Option.iter (fun v -> view.Culture <- v)

    props
    |> Props.tryFind PKey.datePicker.date
    |> Option.iter (fun v -> view.Date <- v)

    // Events

  override _.removeProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.removeProps(terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> Terminal.Gui.Views.DatePicker

    // Properties
    props
    |> Props.tryFind PKey.datePicker.culture
    |> Option.iter (fun _ -> 
        view.Culture <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.datePicker.date
    |> Option.iter (fun _ -> 
        view.Date <- Unchecked.defaultof<_>)


type internal DialogTerminalElement(props: Props) =
  inherit WindowTerminalElement(props)

  override _.name = "Dialog"

  override _.newView() = new Terminal.Gui.Views.Dialog()

  override _.setProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.setProps(terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> Terminal.Gui.Views.Dialog

    // Properties
    props
    |> Props.tryFind PKey.dialog.buttonAlignment
    |> Option.iter (fun v -> view.ButtonAlignment <- v)

    props
    |> Props.tryFind PKey.dialog.buttonAlignmentModes
    |> Option.iter (fun v -> view.ButtonAlignmentModes <- v)

    props
    |> Props.tryFind PKey.dialog.buttons
    |> Option.iter (fun v -> view.Buttons <- v)

    props
    |> Props.tryFind PKey.dialog.canceled
    |> Option.iter (fun v -> view.Canceled <- v)

    // Events

  override _.removeProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.removeProps(terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> Terminal.Gui.Views.Dialog

    // Properties
    props
    |> Props.tryFind PKey.dialog.buttonAlignment
    |> Option.iter (fun _ -> 
        view.ButtonAlignment <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.dialog.buttonAlignmentModes
    |> Option.iter (fun _ -> 
        view.ButtonAlignmentModes <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.dialog.buttons
    |> Option.iter (fun _ -> 
        view.Buttons <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.dialog.canceled
    |> Option.iter (fun _ -> 
        view.Canceled <- Unchecked.defaultof<_>)


type internal FileDialogTerminalElement(props: Props) =
  inherit DialogTerminalElement(props)

  override _.name = "FileDialog"

  override _.newView() = new Terminal.Gui.Views.FileDialog()

  override _.setProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.setProps(terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> Terminal.Gui.Views.FileDialog

    // Properties
    props
    |> Props.tryFind PKey.fileDialog.allowedTypes
    |> Option.iter (fun v -> view.AllowedTypes <- v)

    props
    |> Props.tryFind PKey.fileDialog.allowsMultipleSelection
    |> Option.iter (fun v -> view.AllowsMultipleSelection <- v)

    props
    |> Props.tryFind PKey.fileDialog.currentFilter
    |> Option.iter (fun v -> view.CurrentFilter <- v)

    props
    |> Props.tryFind PKey.fileDialog.fileOperationsHandler
    |> Option.iter (fun v -> view.FileOperationsHandler <- v)

    props
    |> Props.tryFind PKey.fileDialog.multiSelected
    |> Option.iter (fun v -> view.MultiSelected <- v)

    props
    |> Props.tryFind PKey.fileDialog.mustExist
    |> Option.iter (fun v -> view.MustExist <- v)

    props
    |> Props.tryFind PKey.fileDialog.openMode
    |> Option.iter (fun v -> view.OpenMode <- v)

    props
    |> Props.tryFind PKey.fileDialog.path
    |> Option.iter (fun v -> view.Path <- v)

    props
    |> Props.tryFind PKey.fileDialog.searchMatcher
    |> Option.iter (fun v -> view.SearchMatcher <- v)

    // Events
    terminalElement.trySetEventHandler(PKey.fileDialog.filesSelected, view.FilesSelected)


  override _.removeProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.removeProps(terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> Terminal.Gui.Views.FileDialog

    // Properties
    props
    |> Props.tryFind PKey.fileDialog.allowedTypes
    |> Option.iter (fun _ -> 
        view.AllowedTypes <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.fileDialog.allowsMultipleSelection
    |> Option.iter (fun _ -> 
        view.AllowsMultipleSelection <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.fileDialog.currentFilter
    |> Option.iter (fun _ -> 
        view.CurrentFilter <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.fileDialog.fileOperationsHandler
    |> Option.iter (fun _ -> 
        view.FileOperationsHandler <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.fileDialog.multiSelected
    |> Option.iter (fun _ -> 
        view.MultiSelected <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.fileDialog.mustExist
    |> Option.iter (fun _ -> 
        view.MustExist <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.fileDialog.openMode
    |> Option.iter (fun _ -> 
        view.OpenMode <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.fileDialog.path
    |> Option.iter (fun _ -> 
        view.Path <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.fileDialog.searchMatcher
    |> Option.iter (fun _ -> 
        view.SearchMatcher <- Unchecked.defaultof<_>)

    // Events
    terminalElement.tryRemoveEventHandler PKey.FileDialog.FilesSelected

type internal FlagSelectorTerminalElement(props: Props) =
  inherit SelectorBaseTerminalElement(props)

  override _.name = "FlagSelector"

  override _.newView() = new Terminal.Gui.Views.FlagSelector()

  override _.setProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.setProps(terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> Terminal.Gui.Views.FlagSelector

    // Properties
    props
    |> Props.tryFind PKey.flagSelector.value
    |> Option.iter (fun v -> view.Value <- v)

    // Events

  override _.removeProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.removeProps(terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> Terminal.Gui.Views.FlagSelector

    // Properties
    props
    |> Props.tryFind PKey.flagSelector.value
    |> Option.iter (fun _ -> 
        view.Value <- Unchecked.defaultof<_>)


type internal FlagSelector`1TerminalElement(props: Props) =
  inherit FlagSelectorTerminalElement(props)

  override _.name = "FlagSelector`1"

  override _.newView() = new Terminal.Gui.Views.FlagSelector`1()

  override _.setProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.setProps(terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> Terminal.Gui.Views.FlagSelector`1

    // Properties
    props
    |> Props.tryFind PKey.flagSelector`1.value
    |> Option.iter (fun v -> view.Value <- v)

    // Events
    terminalElement.trySetEventHandler(PKey.flagSelector`1.valueChanged, view.ValueChanged)


  override _.removeProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.removeProps(terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> Terminal.Gui.Views.FlagSelector`1

    // Properties
    props
    |> Props.tryFind PKey.flagSelector`1.value
    |> Option.iter (fun _ -> 
        view.Value <- Unchecked.defaultof<_>)

    // Events
    terminalElement.tryRemoveEventHandler PKey.FlagSelector`1.ValueChanged

type internal FrameViewTerminalElement(props: Props) =
  inherit ViewTerminalElement(props)

  override _.name = "FrameView"

  override _.newView() = new Terminal.Gui.Views.FrameView()

  override _.setProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.setProps(terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> Terminal.Gui.Views.FrameView

    // Properties
    // Events

  override _.removeProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.removeProps(terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> Terminal.Gui.Views.FrameView


type internal GBarTerminalElement(props: Props) =
  inherit ColorBarTerminalElement(props)

  override _.name = "GBar"

  override _.newView() = new Terminal.Gui.Views.GBar()

  override _.setProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.setProps(terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> Terminal.Gui.Views.GBar

    // Properties
    props
    |> Props.tryFind PKey.gBar.bBar
    |> Option.iter (fun v -> view.BBar <- v)

    props
    |> Props.tryFind PKey.gBar.rBar
    |> Option.iter (fun v -> view.RBar <- v)

    // Events

  override _.removeProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.removeProps(terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> Terminal.Gui.Views.GBar

    // Properties
    props
    |> Props.tryFind PKey.gBar.bBar
    |> Option.iter (fun _ -> 
        view.BBar <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.gBar.rBar
    |> Option.iter (fun _ -> 
        view.RBar <- Unchecked.defaultof<_>)


type internal GraphViewTerminalElement(props: Props) =
  inherit ViewTerminalElement(props)

  override _.name = "GraphView"

  override _.newView() = new Terminal.Gui.Views.GraphView()

  override _.setProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.setProps(terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> Terminal.Gui.Views.GraphView

    // Properties
    props
    |> Props.tryFind PKey.graphView.axisX
    |> Option.iter (fun v -> view.AxisX <- v)

    props
    |> Props.tryFind PKey.graphView.axisY
    |> Option.iter (fun v -> view.AxisY <- v)

    props
    |> Props.tryFind PKey.graphView.cellSize
    |> Option.iter (fun v -> view.CellSize <- v)

    props
    |> Props.tryFind PKey.graphView.graphColor
    |> Option.iter (fun v -> view.GraphColor <- v)

    props
    |> Props.tryFind PKey.graphView.marginBottom
    |> Option.iter (fun v -> view.MarginBottom <- v)

    props
    |> Props.tryFind PKey.graphView.marginLeft
    |> Option.iter (fun v -> view.MarginLeft <- v)

    props
    |> Props.tryFind PKey.graphView.scrollOffset
    |> Option.iter (fun v -> view.ScrollOffset <- v)

    // Events

  override _.removeProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.removeProps(terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> Terminal.Gui.Views.GraphView

    // Properties
    props
    |> Props.tryFind PKey.graphView.axisX
    |> Option.iter (fun _ -> 
        view.AxisX <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.graphView.axisY
    |> Option.iter (fun _ -> 
        view.AxisY <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.graphView.cellSize
    |> Option.iter (fun _ -> 
        view.CellSize <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.graphView.graphColor
    |> Option.iter (fun _ -> 
        view.GraphColor <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.graphView.marginBottom
    |> Option.iter (fun _ -> 
        view.MarginBottom <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.graphView.marginLeft
    |> Option.iter (fun _ -> 
        view.MarginLeft <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.graphView.scrollOffset
    |> Option.iter (fun _ -> 
        view.ScrollOffset <- Unchecked.defaultof<_>)


type internal HexViewTerminalElement(props: Props) =
  inherit ViewTerminalElement(props)

  override _.name = "HexView"

  override _.newView() = new Terminal.Gui.Views.HexView()

  override _.setProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.setProps(terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> Terminal.Gui.Views.HexView

    // Properties
    props
    |> Props.tryFind PKey.hexView.readOnly
    |> Option.iter (fun v -> view.ReadOnly <- v)

    props
    |> Props.tryFind PKey.hexView.source
    |> Option.iter (fun v -> view.Source <- v)

    props
    |> Props.tryFind PKey.hexView.bytesPerLine
    |> Option.iter (fun v -> view.BytesPerLine <- v)

    props
    |> Props.tryFind PKey.hexView.address
    |> Option.iter (fun v -> view.Address <- v)

    props
    |> Props.tryFind PKey.hexView.addressWidth
    |> Option.iter (fun v -> view.AddressWidth <- v)

    // Events
    terminalElement.trySetEventHandler(PKey.hexView.edited, view.Edited)

    terminalElement.trySetEventHandler(PKey.hexView.positionChanged, view.PositionChanged)


  override _.removeProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.removeProps(terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> Terminal.Gui.Views.HexView

    // Properties
    props
    |> Props.tryFind PKey.hexView.readOnly
    |> Option.iter (fun _ -> 
        view.ReadOnly <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.hexView.source
    |> Option.iter (fun _ -> 
        view.Source <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.hexView.bytesPerLine
    |> Option.iter (fun _ -> 
        view.BytesPerLine <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.hexView.address
    |> Option.iter (fun _ -> 
        view.Address <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.hexView.addressWidth
    |> Option.iter (fun _ -> 
        view.AddressWidth <- Unchecked.defaultof<_>)

    // Events
    terminalElement.tryRemoveEventHandler PKey.HexView.Edited
    terminalElement.tryRemoveEventHandler PKey.HexView.PositionChanged

type internal HueBarTerminalElement(props: Props) =
  inherit ColorBarTerminalElement(props)

  override _.name = "HueBar"

  override _.newView() = new Terminal.Gui.Views.HueBar()

  override _.setProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.setProps(terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> Terminal.Gui.Views.HueBar

    // Properties
    // Events

  override _.removeProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.removeProps(terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> Terminal.Gui.Views.HueBar


type internal LabelTerminalElement(props: Props) =
  inherit ViewTerminalElement(props)

  override _.name = "Label"

  override _.newView() = new Terminal.Gui.Views.Label()

  override _.setProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.setProps(terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> Terminal.Gui.Views.Label

    // Properties
    props
    |> Props.tryFind PKey.label.text
    |> Option.iter (fun v -> view.Text <- v)

    props
    |> Props.tryFind PKey.label.hotKeySpecifier
    |> Option.iter (fun v -> view.HotKeySpecifier <- v)

    // Events

  override _.removeProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.removeProps(terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> Terminal.Gui.Views.Label

    // Properties
    props
    |> Props.tryFind PKey.label.text
    |> Option.iter (fun _ -> 
        view.Text <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.label.hotKeySpecifier
    |> Option.iter (fun _ -> 
        view.HotKeySpecifier <- Unchecked.defaultof<_>)


type internal LegendAnnotationTerminalElement(props: Props) =
  inherit ViewTerminalElement(props)

  override _.name = "LegendAnnotation"

  override _.newView() = new Terminal.Gui.Views.LegendAnnotation()

  override _.setProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.setProps(terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> Terminal.Gui.Views.LegendAnnotation

    // Properties
    // Events

  override _.removeProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.removeProps(terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> Terminal.Gui.Views.LegendAnnotation


type internal LightnessBarTerminalElement(props: Props) =
  inherit ColorBarTerminalElement(props)

  override _.name = "LightnessBar"

  override _.newView() = new Terminal.Gui.Views.LightnessBar()

  override _.setProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.setProps(terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> Terminal.Gui.Views.LightnessBar

    // Properties
    props
    |> Props.tryFind PKey.lightnessBar.hBar
    |> Option.iter (fun v -> view.HBar <- v)

    props
    |> Props.tryFind PKey.lightnessBar.sBar
    |> Option.iter (fun v -> view.SBar <- v)

    // Events

  override _.removeProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.removeProps(terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> Terminal.Gui.Views.LightnessBar

    // Properties
    props
    |> Props.tryFind PKey.lightnessBar.hBar
    |> Option.iter (fun _ -> 
        view.HBar <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.lightnessBar.sBar
    |> Option.iter (fun _ -> 
        view.SBar <- Unchecked.defaultof<_>)


type internal LineTerminalElement(props: Props) =
  inherit ViewTerminalElement(props)

  override _.name = "Line"

  override _.newView() = new Terminal.Gui.Views.Line()

  override _.setProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.setProps(terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> Terminal.Gui.Views.Line

    // Properties
    props
    |> Props.tryFind PKey.line.length
    |> Option.iter (fun v -> view.Length <- v)

    props
    |> Props.tryFind PKey.line.style
    |> Option.iter (fun v -> view.Style <- v)

    props
    |> Props.tryFind PKey.line.orientation
    |> Option.iter (fun v -> view.Orientation <- v)

    // Events
    terminalElement.trySetEventHandler(PKey.line.orientationChanging, view.OrientationChanging)

    terminalElement.trySetEventHandler(PKey.line.orientationChanged, view.OrientationChanged)


  override _.removeProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.removeProps(terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> Terminal.Gui.Views.Line

    // Properties
    props
    |> Props.tryFind PKey.line.length
    |> Option.iter (fun _ -> 
        view.Length <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.line.style
    |> Option.iter (fun _ -> 
        view.Style <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.line.orientation
    |> Option.iter (fun _ -> 
        view.Orientation <- Unchecked.defaultof<_>)

    // Events
    terminalElement.tryRemoveEventHandler PKey.Line.OrientationChanging
    terminalElement.tryRemoveEventHandler PKey.Line.OrientationChanged

type internal ListViewTerminalElement(props: Props) =
  inherit ViewTerminalElement(props)

  override _.name = "ListView"

  override _.newView() = new Terminal.Gui.Views.ListView()

  override _.setProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.setProps(terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> Terminal.Gui.Views.ListView

    // Properties
    props
    |> Props.tryFind PKey.listView.allowsMarking
    |> Option.iter (fun v -> view.AllowsMarking <- v)

    props
    |> Props.tryFind PKey.listView.allowsMultipleSelection
    |> Option.iter (fun v -> view.AllowsMultipleSelection <- v)

    props
    |> Props.tryFind PKey.listView.leftItem
    |> Option.iter (fun v -> view.LeftItem <- v)

    props
    |> Props.tryFind PKey.listView.selectedItem
    |> Option.iter (fun v -> view.SelectedItem <- v)

    props
    |> Props.tryFind PKey.listView.source
    |> Option.iter (fun v -> view.Source <- v)

    props
    |> Props.tryFind PKey.listView.topItem
    |> Option.iter (fun v -> view.TopItem <- v)

    // Events
    terminalElement.trySetEventHandler(PKey.listView.collectionChanged, view.CollectionChanged)

    terminalElement.trySetEventHandler(PKey.listView.openSelectedItem, view.OpenSelectedItem)

    terminalElement.trySetEventHandler(PKey.listView.rowRender, view.RowRender)

    terminalElement.trySetEventHandler(PKey.listView.selectedItemChanged, view.SelectedItemChanged)


  override _.removeProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.removeProps(terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> Terminal.Gui.Views.ListView

    // Properties
    props
    |> Props.tryFind PKey.listView.allowsMarking
    |> Option.iter (fun _ -> 
        view.AllowsMarking <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.listView.allowsMultipleSelection
    |> Option.iter (fun _ -> 
        view.AllowsMultipleSelection <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.listView.leftItem
    |> Option.iter (fun _ -> 
        view.LeftItem <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.listView.selectedItem
    |> Option.iter (fun _ -> 
        view.SelectedItem <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.listView.source
    |> Option.iter (fun _ -> 
        view.Source <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.listView.topItem
    |> Option.iter (fun _ -> 
        view.TopItem <- Unchecked.defaultof<_>)

    // Events
    terminalElement.tryRemoveEventHandler PKey.ListView.CollectionChanged
    terminalElement.tryRemoveEventHandler PKey.ListView.OpenSelectedItem
    terminalElement.tryRemoveEventHandler PKey.ListView.RowRender
    terminalElement.tryRemoveEventHandler PKey.ListView.SelectedItemChanged

type internal MarginTerminalElement(props: Props) =
  inherit AdornmentTerminalElement(props)

  override _.name = "Margin"

  override _.newView() = new Terminal.Gui.ViewBase.Margin()

  override _.setProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.setProps(terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> Terminal.Gui.ViewBase.Margin

    // Properties
    props
    |> Props.tryFind PKey.margin.shadowStyle
    |> Option.iter (fun v -> view.ShadowStyle <- v)

    // Events

  override _.removeProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.removeProps(terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> Terminal.Gui.ViewBase.Margin

    // Properties
    props
    |> Props.tryFind PKey.margin.shadowStyle
    |> Option.iter (fun _ -> 
        view.ShadowStyle <- Unchecked.defaultof<_>)


type internal MenuTerminalElement(props: Props) =
  inherit BarTerminalElement(props)

  override _.name = "Menu"

  override _.newView() = new Terminal.Gui.Views.Menu()

  override _.setProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.setProps(terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> Terminal.Gui.Views.Menu

    // Properties
    props
    |> Props.tryFind PKey.menu.superMenuItem
    |> Option.iter (fun v -> view.SuperMenuItem <- v)

    props
    |> Props.tryFind PKey.menu.selectedMenuItem
    |> Option.iter (fun v -> view.SelectedMenuItem <- v)

    // Events
    terminalElement.trySetEventHandler(PKey.menu.selectedMenuItemChanged, view.SelectedMenuItemChanged)


  override _.removeProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.removeProps(terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> Terminal.Gui.Views.Menu

    // Properties
    props
    |> Props.tryFind PKey.menu.superMenuItem
    |> Option.iter (fun _ -> 
        view.SuperMenuItem <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.menu.selectedMenuItem
    |> Option.iter (fun _ -> 
        view.SelectedMenuItem <- Unchecked.defaultof<_>)

    // Events
    terminalElement.tryRemoveEventHandler PKey.Menu.SelectedMenuItemChanged

type internal MenuBarTerminalElement(props: Props) =
  inherit MenuTerminalElement(props)

  override _.name = "MenuBar"

  override _.newView() = new Terminal.Gui.Views.MenuBar()

  override _.setProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.setProps(terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> Terminal.Gui.Views.MenuBar

    // Properties
    props
    |> Props.tryFind PKey.menuBar.key
    |> Option.iter (fun v -> view.Key <- v)

    props
    |> Props.tryFind PKey.menuBar.active
    |> Option.iter (fun v -> view.Active <- v)

    // Events
    terminalElement.trySetEventHandler(PKey.menuBar.keyChanged, view.KeyChanged)


  override _.removeProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.removeProps(terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> Terminal.Gui.Views.MenuBar

    // Properties
    props
    |> Props.tryFind PKey.menuBar.key
    |> Option.iter (fun _ -> 
        view.Key <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.menuBar.active
    |> Option.iter (fun _ -> 
        view.Active <- Unchecked.defaultof<_>)

    // Events
    terminalElement.tryRemoveEventHandler PKey.MenuBar.KeyChanged

type internal MenuBarItemTerminalElement(props: Props) =
  inherit MenuItemTerminalElement(props)

  override _.name = "MenuBarItem"

  override _.newView() = new Terminal.Gui.Views.MenuBarItem()

  override _.setProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.setProps(terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> Terminal.Gui.Views.MenuBarItem

    // Properties
    props
    |> Props.tryFind PKey.menuBarItem.subMenu
    |> Option.iter (fun v -> view.SubMenu <- v)

    props
    |> Props.tryFind PKey.menuBarItem.popoverMenu
    |> Option.iter (fun v -> view.PopoverMenu <- v)

    props
    |> Props.tryFind PKey.menuBarItem.popoverMenuOpen
    |> Option.iter (fun v -> view.PopoverMenuOpen <- v)

    // Events
    terminalElement.trySetEventHandler(PKey.menuBarItem.popoverMenuOpenChanged, view.PopoverMenuOpenChanged)


  override _.removeProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.removeProps(terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> Terminal.Gui.Views.MenuBarItem

    // Properties
    props
    |> Props.tryFind PKey.menuBarItem.subMenu
    |> Option.iter (fun _ -> 
        view.SubMenu <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.menuBarItem.popoverMenu
    |> Option.iter (fun _ -> 
        view.PopoverMenu <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.menuBarItem.popoverMenuOpen
    |> Option.iter (fun _ -> 
        view.PopoverMenuOpen <- Unchecked.defaultof<_>)

    // Events
    terminalElement.tryRemoveEventHandler PKey.MenuBarItem.PopoverMenuOpenChanged

type internal MenuItemTerminalElement(props: Props) =
  inherit ShortcutTerminalElement(props)

  override _.name = "MenuItem"

  override _.newView() = new Terminal.Gui.Views.MenuItem()

  override _.setProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.setProps(terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> Terminal.Gui.Views.MenuItem

    // Properties
    props
    |> Props.tryFind PKey.menuItem.targetView
    |> Option.iter (fun v -> view.TargetView <- v)

    props
    |> Props.tryFind PKey.menuItem.command
    |> Option.iter (fun v -> view.Command <- v)

    props
    |> Props.tryFind PKey.menuItem.subMenu
    |> Option.iter (fun v -> view.SubMenu <- v)

    // Events

  override _.removeProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.removeProps(terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> Terminal.Gui.Views.MenuItem

    // Properties
    props
    |> Props.tryFind PKey.menuItem.targetView
    |> Option.iter (fun _ -> 
        view.TargetView <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.menuItem.command
    |> Option.iter (fun _ -> 
        view.Command <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.menuItem.subMenu
    |> Option.iter (fun _ -> 
        view.SubMenu <- Unchecked.defaultof<_>)


type internal NumericUpDown`1TerminalElement(props: Props) =
  inherit ViewTerminalElement(props)

  override _.name = "NumericUpDown`1"

  override _.newView() = new Terminal.Gui.Views.NumericUpDown`1()

  override _.setProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.setProps(terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> Terminal.Gui.Views.NumericUpDown`1

    // Properties
    props
    |> Props.tryFind PKey.numericUpDown`1.value
    |> Option.iter (fun v -> view.Value <- v)

    props
    |> Props.tryFind PKey.numericUpDown`1.format
    |> Option.iter (fun v -> view.Format <- v)

    props
    |> Props.tryFind PKey.numericUpDown`1.increment
    |> Option.iter (fun v -> view.Increment <- v)

    // Events
    terminalElement.trySetEventHandler(PKey.numericUpDown`1.valueChanging, view.ValueChanging)

    terminalElement.trySetEventHandler(PKey.numericUpDown`1.valueChanged, view.ValueChanged)

    terminalElement.trySetEventHandler(PKey.numericUpDown`1.formatChanged, view.FormatChanged)

    terminalElement.trySetEventHandler(PKey.numericUpDown`1.incrementChanged, view.IncrementChanged)


  override _.removeProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.removeProps(terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> Terminal.Gui.Views.NumericUpDown`1

    // Properties
    props
    |> Props.tryFind PKey.numericUpDown`1.value
    |> Option.iter (fun _ -> 
        view.Value <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.numericUpDown`1.format
    |> Option.iter (fun _ -> 
        view.Format <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.numericUpDown`1.increment
    |> Option.iter (fun _ -> 
        view.Increment <- Unchecked.defaultof<_>)

    // Events
    terminalElement.tryRemoveEventHandler PKey.NumericUpDown`1.ValueChanging
    terminalElement.tryRemoveEventHandler PKey.NumericUpDown`1.ValueChanged
    terminalElement.tryRemoveEventHandler PKey.NumericUpDown`1.FormatChanged
    terminalElement.tryRemoveEventHandler PKey.NumericUpDown`1.IncrementChanged

type internal OpenDialogTerminalElement(props: Props) =
  inherit FileDialogTerminalElement(props)

  override _.name = "OpenDialog"

  override _.newView() = new Terminal.Gui.Views.OpenDialog()

  override _.setProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.setProps(terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> Terminal.Gui.Views.OpenDialog

    // Properties
    props
    |> Props.tryFind PKey.openDialog.openMode
    |> Option.iter (fun v -> view.OpenMode <- v)

    // Events

  override _.removeProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.removeProps(terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> Terminal.Gui.Views.OpenDialog

    // Properties
    props
    |> Props.tryFind PKey.openDialog.openMode
    |> Option.iter (fun _ -> 
        view.OpenMode <- Unchecked.defaultof<_>)


type internal OptionSelectorTerminalElement(props: Props) =
  inherit SelectorBaseTerminalElement(props)

  override _.name = "OptionSelector"

  override _.newView() = new Terminal.Gui.Views.OptionSelector()

  override _.setProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.setProps(terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> Terminal.Gui.Views.OptionSelector

    // Properties
    props
    |> Props.tryFind PKey.optionSelector.cursor
    |> Option.iter (fun v -> view.Cursor <- v)

    // Events

  override _.removeProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.removeProps(terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> Terminal.Gui.Views.OptionSelector

    // Properties
    props
    |> Props.tryFind PKey.optionSelector.cursor
    |> Option.iter (fun _ -> 
        view.Cursor <- Unchecked.defaultof<_>)


type internal OptionSelector`1TerminalElement(props: Props) =
  inherit OptionSelectorTerminalElement(props)

  override _.name = "OptionSelector`1"

  override _.newView() = new Terminal.Gui.Views.OptionSelector`1()

  override _.setProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.setProps(terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> Terminal.Gui.Views.OptionSelector`1

    // Properties
    props
    |> Props.tryFind PKey.optionSelector`1.value
    |> Option.iter (fun v -> view.Value <- v)

    props
    |> Props.tryFind PKey.optionSelector`1.values
    |> Option.iter (fun v -> view.Values <- v)

    // Events
    terminalElement.trySetEventHandler(PKey.optionSelector`1.valueChanged, view.ValueChanged)


  override _.removeProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.removeProps(terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> Terminal.Gui.Views.OptionSelector`1

    // Properties
    props
    |> Props.tryFind PKey.optionSelector`1.value
    |> Option.iter (fun _ -> 
        view.Value <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.optionSelector`1.values
    |> Option.iter (fun _ -> 
        view.Values <- Unchecked.defaultof<_>)

    // Events
    terminalElement.tryRemoveEventHandler PKey.OptionSelector`1.ValueChanged

type internal PaddingTerminalElement(props: Props) =
  inherit AdornmentTerminalElement(props)

  override _.name = "Padding"

  override _.newView() = new Terminal.Gui.ViewBase.Padding()

  override _.setProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.setProps(terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> Terminal.Gui.ViewBase.Padding

    // Properties
    // Events

  override _.removeProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.removeProps(terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> Terminal.Gui.ViewBase.Padding


type internal PopoverBaseImplTerminalElement(props: Props) =
  inherit ViewTerminalElement(props)

  override _.name = "PopoverBaseImpl"

  override _.newView() = new Terminal.Gui.App.PopoverBaseImpl()

  override _.setProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.setProps(terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> Terminal.Gui.App.PopoverBaseImpl

    // Properties
    props
    |> Props.tryFind PKey.popoverBaseImpl.current
    |> Option.iter (fun v -> view.Current <- v)

    // Events

  override _.removeProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.removeProps(terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> Terminal.Gui.App.PopoverBaseImpl

    // Properties
    props
    |> Props.tryFind PKey.popoverBaseImpl.current
    |> Option.iter (fun _ -> 
        view.Current <- Unchecked.defaultof<_>)


type internal PopoverMenuTerminalElement(props: Props) =
  inherit PopoverBaseImplTerminalElement(props)

  override _.name = "PopoverMenu"

  override _.newView() = new Terminal.Gui.Views.PopoverMenu()

  override _.setProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.setProps(terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> Terminal.Gui.Views.PopoverMenu

    // Properties
    props
    |> Props.tryFind PKey.popoverMenu.key
    |> Option.iter (fun v -> view.Key <- v)

    props
    |> Props.tryFind PKey.popoverMenu.mouseFlags
    |> Option.iter (fun v -> view.MouseFlags <- v)

    props
    |> Props.tryFind PKey.popoverMenu.root
    |> Option.iter (fun v -> view.Root <- v)

    // Events
    terminalElement.trySetEventHandler(PKey.popoverMenu.keyChanged, view.KeyChanged)


  override _.removeProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.removeProps(terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> Terminal.Gui.Views.PopoverMenu

    // Properties
    props
    |> Props.tryFind PKey.popoverMenu.key
    |> Option.iter (fun _ -> 
        view.Key <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.popoverMenu.mouseFlags
    |> Option.iter (fun _ -> 
        view.MouseFlags <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.popoverMenu.root
    |> Option.iter (fun _ -> 
        view.Root <- Unchecked.defaultof<_>)

    // Events
    terminalElement.tryRemoveEventHandler PKey.PopoverMenu.KeyChanged

type internal PopupTerminalElement(props: Props) =
  inherit ViewTerminalElement(props)

  override _.name = "Popup"

  override _.newView() = new Terminal.Gui.Views.PopupAutocomplete+Popup()

  override _.setProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.setProps(terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> Terminal.Gui.Views.PopupAutocomplete+Popup

    // Properties
    // Events

  override _.removeProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.removeProps(terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> Terminal.Gui.Views.PopupAutocomplete+Popup


type internal ProgressBarTerminalElement(props: Props) =
  inherit ViewTerminalElement(props)

  override _.name = "ProgressBar"

  override _.newView() = new Terminal.Gui.Views.ProgressBar()

  override _.setProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.setProps(terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> Terminal.Gui.Views.ProgressBar

    // Properties
    props
    |> Props.tryFind PKey.progressBar.bidirectionalMarquee
    |> Option.iter (fun v -> view.BidirectionalMarquee <- v)

    props
    |> Props.tryFind PKey.progressBar.fraction
    |> Option.iter (fun v -> view.Fraction <- v)

    props
    |> Props.tryFind PKey.progressBar.progressBarFormat
    |> Option.iter (fun v -> view.ProgressBarFormat <- v)

    props
    |> Props.tryFind PKey.progressBar.progressBarStyle
    |> Option.iter (fun v -> view.ProgressBarStyle <- v)

    props
    |> Props.tryFind PKey.progressBar.segmentCharacter
    |> Option.iter (fun v -> view.SegmentCharacter <- v)

    props
    |> Props.tryFind PKey.progressBar.text
    |> Option.iter (fun v -> view.Text <- v)

    // Events

  override _.removeProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.removeProps(terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> Terminal.Gui.Views.ProgressBar

    // Properties
    props
    |> Props.tryFind PKey.progressBar.bidirectionalMarquee
    |> Option.iter (fun _ -> 
        view.BidirectionalMarquee <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.progressBar.fraction
    |> Option.iter (fun _ -> 
        view.Fraction <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.progressBar.progressBarFormat
    |> Option.iter (fun _ -> 
        view.ProgressBarFormat <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.progressBar.progressBarStyle
    |> Option.iter (fun _ -> 
        view.ProgressBarStyle <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.progressBar.segmentCharacter
    |> Option.iter (fun _ -> 
        view.SegmentCharacter <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.progressBar.text
    |> Option.iter (fun _ -> 
        view.Text <- Unchecked.defaultof<_>)


type internal RBarTerminalElement(props: Props) =
  inherit ColorBarTerminalElement(props)

  override _.name = "RBar"

  override _.newView() = new Terminal.Gui.Views.RBar()

  override _.setProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.setProps(terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> Terminal.Gui.Views.RBar

    // Properties
    props
    |> Props.tryFind PKey.rBar.bBar
    |> Option.iter (fun v -> view.BBar <- v)

    props
    |> Props.tryFind PKey.rBar.gBar
    |> Option.iter (fun v -> view.GBar <- v)

    // Events

  override _.removeProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.removeProps(terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> Terminal.Gui.Views.RBar

    // Properties
    props
    |> Props.tryFind PKey.rBar.bBar
    |> Option.iter (fun _ -> 
        view.BBar <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.rBar.gBar
    |> Option.iter (fun _ -> 
        view.GBar <- Unchecked.defaultof<_>)


type internal RunnableTerminalElement(props: Props) =
  inherit ViewTerminalElement(props)

  override _.name = "Runnable"

  override _.newView() = new Terminal.Gui.Views.Runnable()

  override _.setProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.setProps(terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> Terminal.Gui.Views.Runnable

    // Properties
    props
    |> Props.tryFind PKey.runnable.result
    |> Option.iter (fun v -> view.Result <- v)

    props
    |> Props.tryFind PKey.runnable.stopRequested
    |> Option.iter (fun v -> view.StopRequested <- v)

    // Events
    terminalElement.trySetEventHandler(PKey.runnable.isRunningChanging, view.IsRunningChanging)

    terminalElement.trySetEventHandler(PKey.runnable.isRunningChanged, view.IsRunningChanged)

    terminalElement.trySetEventHandler(PKey.runnable.isModalChanged, view.IsModalChanged)


  override _.removeProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.removeProps(terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> Terminal.Gui.Views.Runnable

    // Properties
    props
    |> Props.tryFind PKey.runnable.result
    |> Option.iter (fun _ -> 
        view.Result <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.runnable.stopRequested
    |> Option.iter (fun _ -> 
        view.StopRequested <- Unchecked.defaultof<_>)

    // Events
    terminalElement.tryRemoveEventHandler PKey.Runnable.IsRunningChanging
    terminalElement.tryRemoveEventHandler PKey.Runnable.IsRunningChanged
    terminalElement.tryRemoveEventHandler PKey.Runnable.IsModalChanged

type internal Runnable`1TerminalElement(props: Props) =
  inherit RunnableTerminalElement(props)

  override _.name = "Runnable`1"

  override _.newView() = new Terminal.Gui.Views.Runnable`1()

  override _.setProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.setProps(terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> Terminal.Gui.Views.Runnable`1

    // Properties
    props
    |> Props.tryFind PKey.runnable`1.result
    |> Option.iter (fun v -> view.Result <- v)

    // Events

  override _.removeProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.removeProps(terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> Terminal.Gui.Views.Runnable`1

    // Properties
    props
    |> Props.tryFind PKey.runnable`1.result
    |> Option.iter (fun _ -> 
        view.Result <- Unchecked.defaultof<_>)


type internal SaturationBarTerminalElement(props: Props) =
  inherit ColorBarTerminalElement(props)

  override _.name = "SaturationBar"

  override _.newView() = new Terminal.Gui.Views.SaturationBar()

  override _.setProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.setProps(terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> Terminal.Gui.Views.SaturationBar

    // Properties
    props
    |> Props.tryFind PKey.saturationBar.hBar
    |> Option.iter (fun v -> view.HBar <- v)

    props
    |> Props.tryFind PKey.saturationBar.lBar
    |> Option.iter (fun v -> view.LBar <- v)

    props
    |> Props.tryFind PKey.saturationBar.vBar
    |> Option.iter (fun v -> view.VBar <- v)

    // Events

  override _.removeProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.removeProps(terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> Terminal.Gui.Views.SaturationBar

    // Properties
    props
    |> Props.tryFind PKey.saturationBar.hBar
    |> Option.iter (fun _ -> 
        view.HBar <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.saturationBar.lBar
    |> Option.iter (fun _ -> 
        view.LBar <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.saturationBar.vBar
    |> Option.iter (fun _ -> 
        view.VBar <- Unchecked.defaultof<_>)


type internal SaveDialogTerminalElement(props: Props) =
  inherit FileDialogTerminalElement(props)

  override _.name = "SaveDialog"

  override _.newView() = new Terminal.Gui.Views.SaveDialog()

  override _.setProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.setProps(terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> Terminal.Gui.Views.SaveDialog

    // Properties
    // Events

  override _.removeProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.removeProps(terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> Terminal.Gui.Views.SaveDialog


type internal ScrollBarTerminalElement(props: Props) =
  inherit ViewTerminalElement(props)

  override _.name = "ScrollBar"

  override _.newView() = new Terminal.Gui.Views.ScrollBar()

  override _.setProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.setProps(terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> Terminal.Gui.Views.ScrollBar

    // Properties
    props
    |> Props.tryFind PKey.scrollBar.orientation
    |> Option.iter (fun v -> view.Orientation <- v)

    props
    |> Props.tryFind PKey.scrollBar.increment
    |> Option.iter (fun v -> view.Increment <- v)

    props
    |> Props.tryFind PKey.scrollBar.autoShow
    |> Option.iter (fun v -> view.AutoShow <- v)

    props
    |> Props.tryFind PKey.scrollBar.visibleContentSize
    |> Option.iter (fun v -> view.VisibleContentSize <- v)

    props
    |> Props.tryFind PKey.scrollBar.scrollableContentSize
    |> Option.iter (fun v -> view.ScrollableContentSize <- v)

    props
    |> Props.tryFind PKey.scrollBar.position
    |> Option.iter (fun v -> view.Position <- v)

    // Events
    terminalElement.trySetEventHandler(PKey.scrollBar.orientationChanging, view.OrientationChanging)

    terminalElement.trySetEventHandler(PKey.scrollBar.orientationChanged, view.OrientationChanged)

    terminalElement.trySetEventHandler(PKey.scrollBar.scrollableContentSizeChanged, view.ScrollableContentSizeChanged)

    terminalElement.trySetEventHandler(PKey.scrollBar.positionChanging, view.PositionChanging)

    terminalElement.trySetEventHandler(PKey.scrollBar.positionChanged, view.PositionChanged)

    terminalElement.trySetEventHandler(PKey.scrollBar.scrolled, view.Scrolled)

    terminalElement.trySetEventHandler(PKey.scrollBar.sliderPositionChanged, view.SliderPositionChanged)


  override _.removeProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.removeProps(terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> Terminal.Gui.Views.ScrollBar

    // Properties
    props
    |> Props.tryFind PKey.scrollBar.orientation
    |> Option.iter (fun _ -> 
        view.Orientation <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.scrollBar.increment
    |> Option.iter (fun _ -> 
        view.Increment <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.scrollBar.autoShow
    |> Option.iter (fun _ -> 
        view.AutoShow <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.scrollBar.visibleContentSize
    |> Option.iter (fun _ -> 
        view.VisibleContentSize <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.scrollBar.scrollableContentSize
    |> Option.iter (fun _ -> 
        view.ScrollableContentSize <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.scrollBar.position
    |> Option.iter (fun _ -> 
        view.Position <- Unchecked.defaultof<_>)

    // Events
    terminalElement.tryRemoveEventHandler PKey.ScrollBar.OrientationChanging
    terminalElement.tryRemoveEventHandler PKey.ScrollBar.OrientationChanged
    terminalElement.tryRemoveEventHandler PKey.ScrollBar.ScrollableContentSizeChanged
    terminalElement.tryRemoveEventHandler PKey.ScrollBar.PositionChanging
    terminalElement.tryRemoveEventHandler PKey.ScrollBar.PositionChanged
    terminalElement.tryRemoveEventHandler PKey.ScrollBar.Scrolled
    terminalElement.tryRemoveEventHandler PKey.ScrollBar.SliderPositionChanged

type internal ScrollSliderTerminalElement(props: Props) =
  inherit ViewTerminalElement(props)

  override _.name = "ScrollSlider"

  override _.newView() = new Terminal.Gui.Views.ScrollSlider()

  override _.setProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.setProps(terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> Terminal.Gui.Views.ScrollSlider

    // Properties
    props
    |> Props.tryFind PKey.scrollSlider.orientation
    |> Option.iter (fun v -> view.Orientation <- v)

    props
    |> Props.tryFind PKey.scrollSlider.size
    |> Option.iter (fun v -> view.Size <- v)

    props
    |> Props.tryFind PKey.scrollSlider.visibleContentSize
    |> Option.iter (fun v -> view.VisibleContentSize <- v)

    props
    |> Props.tryFind PKey.scrollSlider.position
    |> Option.iter (fun v -> view.Position <- v)

    props
    |> Props.tryFind PKey.scrollSlider.sliderPadding
    |> Option.iter (fun v -> view.SliderPadding <- v)

    // Events
    terminalElement.trySetEventHandler(PKey.scrollSlider.orientationChanging, view.OrientationChanging)

    terminalElement.trySetEventHandler(PKey.scrollSlider.orientationChanged, view.OrientationChanged)

    terminalElement.trySetEventHandler(PKey.scrollSlider.positionChanging, view.PositionChanging)

    terminalElement.trySetEventHandler(PKey.scrollSlider.positionChanged, view.PositionChanged)

    terminalElement.trySetEventHandler(PKey.scrollSlider.scrolled, view.Scrolled)


  override _.removeProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.removeProps(terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> Terminal.Gui.Views.ScrollSlider

    // Properties
    props
    |> Props.tryFind PKey.scrollSlider.orientation
    |> Option.iter (fun _ -> 
        view.Orientation <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.scrollSlider.size
    |> Option.iter (fun _ -> 
        view.Size <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.scrollSlider.visibleContentSize
    |> Option.iter (fun _ -> 
        view.VisibleContentSize <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.scrollSlider.position
    |> Option.iter (fun _ -> 
        view.Position <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.scrollSlider.sliderPadding
    |> Option.iter (fun _ -> 
        view.SliderPadding <- Unchecked.defaultof<_>)

    // Events
    terminalElement.tryRemoveEventHandler PKey.ScrollSlider.OrientationChanging
    terminalElement.tryRemoveEventHandler PKey.ScrollSlider.OrientationChanged
    terminalElement.tryRemoveEventHandler PKey.ScrollSlider.PositionChanging
    terminalElement.tryRemoveEventHandler PKey.ScrollSlider.PositionChanged
    terminalElement.tryRemoveEventHandler PKey.ScrollSlider.Scrolled

type internal SelectorBaseTerminalElement(props: Props) =
  inherit ViewTerminalElement(props)

  override _.name = "SelectorBase"

  override _.newView() = new Terminal.Gui.Views.SelectorBase()

  override _.setProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.setProps(terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> Terminal.Gui.Views.SelectorBase

    // Properties
    props
    |> Props.tryFind PKey.selectorBase.styles
    |> Option.iter (fun v -> view.Styles <- v)

    props
    |> Props.tryFind PKey.selectorBase.value
    |> Option.iter (fun v -> view.Value <- v)

    props
    |> Props.tryFind PKey.selectorBase.values
    |> Option.iter (fun v -> view.Values <- v)

    props
    |> Props.tryFind PKey.selectorBase.labels
    |> Option.iter (fun v -> view.Labels <- v)

    props
    |> Props.tryFind PKey.selectorBase.assignHotKeys
    |> Option.iter (fun v -> view.AssignHotKeys <- v)

    props
    |> Props.tryFind PKey.selectorBase.usedHotKeys
    |> Option.iter (fun v -> view.UsedHotKeys <- v)

    props
    |> Props.tryFind PKey.selectorBase.horizontalSpace
    |> Option.iter (fun v -> view.HorizontalSpace <- v)

    props
    |> Props.tryFind PKey.selectorBase.doubleClickAccepts
    |> Option.iter (fun v -> view.DoubleClickAccepts <- v)

    props
    |> Props.tryFind PKey.selectorBase.orientation
    |> Option.iter (fun v -> view.Orientation <- v)

    // Events
    terminalElement.trySetEventHandler(PKey.selectorBase.valueChanged, view.ValueChanged)

    terminalElement.trySetEventHandler(PKey.selectorBase.orientationChanging, view.OrientationChanging)

    terminalElement.trySetEventHandler(PKey.selectorBase.orientationChanged, view.OrientationChanged)


  override _.removeProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.removeProps(terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> Terminal.Gui.Views.SelectorBase

    // Properties
    props
    |> Props.tryFind PKey.selectorBase.styles
    |> Option.iter (fun _ -> 
        view.Styles <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.selectorBase.value
    |> Option.iter (fun _ -> 
        view.Value <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.selectorBase.values
    |> Option.iter (fun _ -> 
        view.Values <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.selectorBase.labels
    |> Option.iter (fun _ -> 
        view.Labels <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.selectorBase.assignHotKeys
    |> Option.iter (fun _ -> 
        view.AssignHotKeys <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.selectorBase.usedHotKeys
    |> Option.iter (fun _ -> 
        view.UsedHotKeys <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.selectorBase.horizontalSpace
    |> Option.iter (fun _ -> 
        view.HorizontalSpace <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.selectorBase.doubleClickAccepts
    |> Option.iter (fun _ -> 
        view.DoubleClickAccepts <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.selectorBase.orientation
    |> Option.iter (fun _ -> 
        view.Orientation <- Unchecked.defaultof<_>)

    // Events
    terminalElement.tryRemoveEventHandler PKey.SelectorBase.ValueChanged
    terminalElement.tryRemoveEventHandler PKey.SelectorBase.OrientationChanging
    terminalElement.tryRemoveEventHandler PKey.SelectorBase.OrientationChanged

type internal ShadowViewTerminalElement(props: Props) =
  inherit ViewTerminalElement(props)

  override _.name = "ShadowView"

  override _.newView() = new Terminal.Gui.ViewBase.ShadowView()

  override _.setProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.setProps(terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> Terminal.Gui.ViewBase.ShadowView

    // Properties
    props
    |> Props.tryFind PKey.shadowView.orientation
    |> Option.iter (fun v -> view.Orientation <- v)

    props
    |> Props.tryFind PKey.shadowView.shadowStyle
    |> Option.iter (fun v -> view.ShadowStyle <- v)

    // Events

  override _.removeProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.removeProps(terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> Terminal.Gui.ViewBase.ShadowView

    // Properties
    props
    |> Props.tryFind PKey.shadowView.orientation
    |> Option.iter (fun _ -> 
        view.Orientation <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.shadowView.shadowStyle
    |> Option.iter (fun _ -> 
        view.ShadowStyle <- Unchecked.defaultof<_>)


type internal ShortcutTerminalElement(props: Props) =
  inherit ViewTerminalElement(props)

  override _.name = "Shortcut"

  override _.newView() = new Terminal.Gui.Views.Shortcut()

  override _.setProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.setProps(terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> Terminal.Gui.Views.Shortcut

    // Properties
    props
    |> Props.tryFind PKey.shortcut.alignmentModes
    |> Option.iter (fun v -> view.AlignmentModes <- v)

    props
    |> Props.tryFind PKey.shortcut.action
    |> Option.iter (fun v -> view.Action <- v)

    props
    |> Props.tryFind PKey.shortcut.orientation
    |> Option.iter (fun v -> view.Orientation <- v)

    props
    |> Props.tryFind PKey.shortcut.commandView
    |> Option.iter (fun v -> view.CommandView <- v)

    props
    |> Props.tryFind PKey.shortcut.text
    |> Option.iter (fun v -> view.Text <- v)

    props
    |> Props.tryFind PKey.shortcut.helpText
    |> Option.iter (fun v -> view.HelpText <- v)

    props
    |> Props.tryFind PKey.shortcut.key
    |> Option.iter (fun v -> view.Key <- v)

    props
    |> Props.tryFind PKey.shortcut.bindKeyToApplication
    |> Option.iter (fun v -> view.BindKeyToApplication <- v)

    props
    |> Props.tryFind PKey.shortcut.minimumKeyTextSize
    |> Option.iter (fun v -> view.MinimumKeyTextSize <- v)

    props
    |> Props.tryFind PKey.shortcut.forceFocusColors
    |> Option.iter (fun v -> view.ForceFocusColors <- v)

    // Events
    terminalElement.trySetEventHandler(PKey.shortcut.orientationChanging, view.OrientationChanging)

    terminalElement.trySetEventHandler(PKey.shortcut.orientationChanged, view.OrientationChanged)


  override _.removeProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.removeProps(terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> Terminal.Gui.Views.Shortcut

    // Properties
    props
    |> Props.tryFind PKey.shortcut.alignmentModes
    |> Option.iter (fun _ -> 
        view.AlignmentModes <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.shortcut.action
    |> Option.iter (fun _ -> 
        view.Action <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.shortcut.orientation
    |> Option.iter (fun _ -> 
        view.Orientation <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.shortcut.commandView
    |> Option.iter (fun _ -> 
        view.CommandView <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.shortcut.text
    |> Option.iter (fun _ -> 
        view.Text <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.shortcut.helpText
    |> Option.iter (fun _ -> 
        view.HelpText <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.shortcut.key
    |> Option.iter (fun _ -> 
        view.Key <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.shortcut.bindKeyToApplication
    |> Option.iter (fun _ -> 
        view.BindKeyToApplication <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.shortcut.minimumKeyTextSize
    |> Option.iter (fun _ -> 
        view.MinimumKeyTextSize <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.shortcut.forceFocusColors
    |> Option.iter (fun _ -> 
        view.ForceFocusColors <- Unchecked.defaultof<_>)

    // Events
    terminalElement.tryRemoveEventHandler PKey.Shortcut.OrientationChanging
    terminalElement.tryRemoveEventHandler PKey.Shortcut.OrientationChanged

type internal Slider`1TerminalElement(props: Props) =
  inherit ViewTerminalElement(props)

  override _.name = "Slider`1"

  override _.newView() = new Terminal.Gui.Views.Slider`1()

  override _.setProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.setProps(terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> Terminal.Gui.Views.Slider`1

    // Properties
    props
    |> Props.tryFind PKey.slider`1.text
    |> Option.iter (fun v -> view.Text <- v)

    props
    |> Props.tryFind PKey.slider`1.allowEmpty
    |> Option.iter (fun v -> view.AllowEmpty <- v)

    props
    |> Props.tryFind PKey.slider`1.minimumInnerSpacing
    |> Option.iter (fun v -> view.MinimumInnerSpacing <- v)

    props
    |> Props.tryFind PKey.slider`1.type
    |> Option.iter (fun v -> view.Type <- v)

    props
    |> Props.tryFind PKey.slider`1.orientation
    |> Option.iter (fun v -> view.Orientation <- v)

    props
    |> Props.tryFind PKey.slider`1.legendsOrientation
    |> Option.iter (fun v -> view.LegendsOrientation <- v)

    props
    |> Props.tryFind PKey.slider`1.style
    |> Option.iter (fun v -> view.Style <- v)

    props
    |> Props.tryFind PKey.slider`1.options
    |> Option.iter (fun v -> view.Options <- v)

    props
    |> Props.tryFind PKey.slider`1.rangeAllowSingle
    |> Option.iter (fun v -> view.RangeAllowSingle <- v)

    props
    |> Props.tryFind PKey.slider`1.showEndSpacing
    |> Option.iter (fun v -> view.ShowEndSpacing <- v)

    props
    |> Props.tryFind PKey.slider`1.showLegends
    |> Option.iter (fun v -> view.ShowLegends <- v)

    props
    |> Props.tryFind PKey.slider`1.useMinimumSize
    |> Option.iter (fun v -> view.UseMinimumSize <- v)

    props
    |> Props.tryFind PKey.slider`1.focusedOption
    |> Option.iter (fun v -> view.FocusedOption <- v)

    // Events
    terminalElement.trySetEventHandler(PKey.slider`1.orientationChanging, view.OrientationChanging)

    terminalElement.trySetEventHandler(PKey.slider`1.orientationChanged, view.OrientationChanged)

    terminalElement.trySetEventHandler(PKey.slider`1.optionsChanged, view.OptionsChanged)

    terminalElement.trySetEventHandler(PKey.slider`1.optionFocused, view.OptionFocused)


  override _.removeProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.removeProps(terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> Terminal.Gui.Views.Slider`1

    // Properties
    props
    |> Props.tryFind PKey.slider`1.text
    |> Option.iter (fun _ -> 
        view.Text <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.slider`1.allowEmpty
    |> Option.iter (fun _ -> 
        view.AllowEmpty <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.slider`1.minimumInnerSpacing
    |> Option.iter (fun _ -> 
        view.MinimumInnerSpacing <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.slider`1.type
    |> Option.iter (fun _ -> 
        view.Type <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.slider`1.orientation
    |> Option.iter (fun _ -> 
        view.Orientation <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.slider`1.legendsOrientation
    |> Option.iter (fun _ -> 
        view.LegendsOrientation <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.slider`1.style
    |> Option.iter (fun _ -> 
        view.Style <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.slider`1.options
    |> Option.iter (fun _ -> 
        view.Options <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.slider`1.rangeAllowSingle
    |> Option.iter (fun _ -> 
        view.RangeAllowSingle <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.slider`1.showEndSpacing
    |> Option.iter (fun _ -> 
        view.ShowEndSpacing <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.slider`1.showLegends
    |> Option.iter (fun _ -> 
        view.ShowLegends <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.slider`1.useMinimumSize
    |> Option.iter (fun _ -> 
        view.UseMinimumSize <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.slider`1.focusedOption
    |> Option.iter (fun _ -> 
        view.FocusedOption <- Unchecked.defaultof<_>)

    // Events
    terminalElement.tryRemoveEventHandler PKey.Slider`1.OrientationChanging
    terminalElement.tryRemoveEventHandler PKey.Slider`1.OrientationChanged
    terminalElement.tryRemoveEventHandler PKey.Slider`1.OptionsChanged
    terminalElement.tryRemoveEventHandler PKey.Slider`1.OptionFocused

type internal SpinnerViewTerminalElement(props: Props) =
  inherit ViewTerminalElement(props)

  override _.name = "SpinnerView"

  override _.newView() = new Terminal.Gui.Views.SpinnerView()

  override _.setProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.setProps(terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> Terminal.Gui.Views.SpinnerView

    // Properties
    props
    |> Props.tryFind PKey.spinnerView.autoSpin
    |> Option.iter (fun v -> view.AutoSpin <- v)

    props
    |> Props.tryFind PKey.spinnerView.sequence
    |> Option.iter (fun v -> view.Sequence <- v)

    props
    |> Props.tryFind PKey.spinnerView.spinBounce
    |> Option.iter (fun v -> view.SpinBounce <- v)

    props
    |> Props.tryFind PKey.spinnerView.spinDelay
    |> Option.iter (fun v -> view.SpinDelay <- v)

    props
    |> Props.tryFind PKey.spinnerView.spinReverse
    |> Option.iter (fun v -> view.SpinReverse <- v)

    props
    |> Props.tryFind PKey.spinnerView.style
    |> Option.iter (fun v -> view.Style <- v)

    // Events

  override _.removeProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.removeProps(terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> Terminal.Gui.Views.SpinnerView

    // Properties
    props
    |> Props.tryFind PKey.spinnerView.autoSpin
    |> Option.iter (fun _ -> 
        view.AutoSpin <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.spinnerView.sequence
    |> Option.iter (fun _ -> 
        view.Sequence <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.spinnerView.spinBounce
    |> Option.iter (fun _ -> 
        view.SpinBounce <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.spinnerView.spinDelay
    |> Option.iter (fun _ -> 
        view.SpinDelay <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.spinnerView.spinReverse
    |> Option.iter (fun _ -> 
        view.SpinReverse <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.spinnerView.style
    |> Option.iter (fun _ -> 
        view.Style <- Unchecked.defaultof<_>)


type internal StatusBarTerminalElement(props: Props) =
  inherit BarTerminalElement(props)

  override _.name = "StatusBar"

  override _.newView() = new Terminal.Gui.Views.StatusBar()

  override _.setProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.setProps(terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> Terminal.Gui.Views.StatusBar

    // Properties
    // Events

  override _.removeProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.removeProps(terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> Terminal.Gui.Views.StatusBar


type internal TabTerminalElement(props: Props) =
  inherit ViewTerminalElement(props)

  override _.name = "Tab"

  override _.newView() = new Terminal.Gui.Views.Tab()

  override _.setProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.setProps(terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> Terminal.Gui.Views.Tab

    // Properties
    props
    |> Props.tryFind PKey.tab.displayText
    |> Option.iter (fun v -> view.DisplayText <- v)

    props
    |> Props.tryFind PKey.tab.view
    |> Option.iter (fun v -> view.View <- v)

    // Events

  override _.removeProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.removeProps(terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> Terminal.Gui.Views.Tab

    // Properties
    props
    |> Props.tryFind PKey.tab.displayText
    |> Option.iter (fun _ -> 
        view.DisplayText <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.tab.view
    |> Option.iter (fun _ -> 
        view.View <- Unchecked.defaultof<_>)


type internal TabRowTerminalElement(props: Props) =
  inherit ViewTerminalElement(props)

  override _.name = "TabRow"

  override _.newView() = new Terminal.Gui.Views.TabRow()

  override _.setProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.setProps(terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> Terminal.Gui.Views.TabRow

    // Properties
    // Events

  override _.removeProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.removeProps(terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> Terminal.Gui.Views.TabRow


type internal TabViewTerminalElement(props: Props) =
  inherit ViewTerminalElement(props)

  override _.name = "TabView"

  override _.newView() = new Terminal.Gui.Views.TabView()

  override _.setProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.setProps(terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> Terminal.Gui.Views.TabView

    // Properties
    props
    |> Props.tryFind PKey.tabView.maxTabTextWidth
    |> Option.iter (fun v -> view.MaxTabTextWidth <- v)

    props
    |> Props.tryFind PKey.tabView.selectedTab
    |> Option.iter (fun v -> view.SelectedTab <- v)

    props
    |> Props.tryFind PKey.tabView.style
    |> Option.iter (fun v -> view.Style <- v)

    props
    |> Props.tryFind PKey.tabView.tabScrollOffset
    |> Option.iter (fun v -> view.TabScrollOffset <- v)

    // Events
    terminalElement.trySetEventHandler(PKey.tabView.selectedTabChanged, view.SelectedTabChanged)

    terminalElement.trySetEventHandler(PKey.tabView.tabClicked, view.TabClicked)


  override _.removeProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.removeProps(terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> Terminal.Gui.Views.TabView

    // Properties
    props
    |> Props.tryFind PKey.tabView.maxTabTextWidth
    |> Option.iter (fun _ -> 
        view.MaxTabTextWidth <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.tabView.selectedTab
    |> Option.iter (fun _ -> 
        view.SelectedTab <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.tabView.style
    |> Option.iter (fun _ -> 
        view.Style <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.tabView.tabScrollOffset
    |> Option.iter (fun _ -> 
        view.TabScrollOffset <- Unchecked.defaultof<_>)

    // Events
    terminalElement.tryRemoveEventHandler PKey.TabView.SelectedTabChanged
    terminalElement.tryRemoveEventHandler PKey.TabView.TabClicked

type internal TableViewTerminalElement(props: Props) =
  inherit ViewTerminalElement(props)

  override _.name = "TableView"

  override _.newView() = new Terminal.Gui.Views.TableView()

  override _.setProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.setProps(terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> Terminal.Gui.Views.TableView

    // Properties
    props
    |> Props.tryFind PKey.tableView.cellActivationKey
    |> Option.iter (fun v -> view.CellActivationKey <- v)

    props
    |> Props.tryFind PKey.tableView.collectionNavigator
    |> Option.iter (fun v -> view.CollectionNavigator <- v)

    props
    |> Props.tryFind PKey.tableView.columnOffset
    |> Option.iter (fun v -> view.ColumnOffset <- v)

    props
    |> Props.tryFind PKey.tableView.fullRowSelect
    |> Option.iter (fun v -> view.FullRowSelect <- v)

    props
    |> Props.tryFind PKey.tableView.maxCellWidth
    |> Option.iter (fun v -> view.MaxCellWidth <- v)

    props
    |> Props.tryFind PKey.tableView.minCellWidth
    |> Option.iter (fun v -> view.MinCellWidth <- v)

    props
    |> Props.tryFind PKey.tableView.multiSelect
    |> Option.iter (fun v -> view.MultiSelect <- v)

    props
    |> Props.tryFind PKey.tableView.nullSymbol
    |> Option.iter (fun v -> view.NullSymbol <- v)

    props
    |> Props.tryFind PKey.tableView.rowOffset
    |> Option.iter (fun v -> view.RowOffset <- v)

    props
    |> Props.tryFind PKey.tableView.selectedColumn
    |> Option.iter (fun v -> view.SelectedColumn <- v)

    props
    |> Props.tryFind PKey.tableView.selectedRow
    |> Option.iter (fun v -> view.SelectedRow <- v)

    props
    |> Props.tryFind PKey.tableView.separatorSymbol
    |> Option.iter (fun v -> view.SeparatorSymbol <- v)

    props
    |> Props.tryFind PKey.tableView.style
    |> Option.iter (fun v -> view.Style <- v)

    props
    |> Props.tryFind PKey.tableView.table
    |> Option.iter (fun v -> view.Table <- v)

    // Events
    terminalElement.trySetEventHandler(PKey.tableView.cellActivated, view.CellActivated)

    terminalElement.trySetEventHandler(PKey.tableView.cellToggled, view.CellToggled)

    terminalElement.trySetEventHandler(PKey.tableView.selectedCellChanged, view.SelectedCellChanged)


  override _.removeProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.removeProps(terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> Terminal.Gui.Views.TableView

    // Properties
    props
    |> Props.tryFind PKey.tableView.cellActivationKey
    |> Option.iter (fun _ -> 
        view.CellActivationKey <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.tableView.collectionNavigator
    |> Option.iter (fun _ -> 
        view.CollectionNavigator <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.tableView.columnOffset
    |> Option.iter (fun _ -> 
        view.ColumnOffset <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.tableView.fullRowSelect
    |> Option.iter (fun _ -> 
        view.FullRowSelect <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.tableView.maxCellWidth
    |> Option.iter (fun _ -> 
        view.MaxCellWidth <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.tableView.minCellWidth
    |> Option.iter (fun _ -> 
        view.MinCellWidth <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.tableView.multiSelect
    |> Option.iter (fun _ -> 
        view.MultiSelect <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.tableView.nullSymbol
    |> Option.iter (fun _ -> 
        view.NullSymbol <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.tableView.rowOffset
    |> Option.iter (fun _ -> 
        view.RowOffset <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.tableView.selectedColumn
    |> Option.iter (fun _ -> 
        view.SelectedColumn <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.tableView.selectedRow
    |> Option.iter (fun _ -> 
        view.SelectedRow <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.tableView.separatorSymbol
    |> Option.iter (fun _ -> 
        view.SeparatorSymbol <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.tableView.style
    |> Option.iter (fun _ -> 
        view.Style <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.tableView.table
    |> Option.iter (fun _ -> 
        view.Table <- Unchecked.defaultof<_>)

    // Events
    terminalElement.tryRemoveEventHandler PKey.TableView.CellActivated
    terminalElement.tryRemoveEventHandler PKey.TableView.CellToggled
    terminalElement.tryRemoveEventHandler PKey.TableView.SelectedCellChanged

type internal TextFieldTerminalElement(props: Props) =
  inherit ViewTerminalElement(props)

  override _.name = "TextField"

  override _.newView() = new Terminal.Gui.Views.TextField()

  override _.setProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.setProps(terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> Terminal.Gui.Views.TextField

    // Properties
    props
    |> Props.tryFind PKey.textField.autocomplete
    |> Option.iter (fun v -> view.Autocomplete <- v)

    props
    |> Props.tryFind PKey.textField.contextMenu
    |> Option.iter (fun v -> view.ContextMenu <- v)

    props
    |> Props.tryFind PKey.textField.cursorPosition
    |> Option.iter (fun v -> view.CursorPosition <- v)

    props
    |> Props.tryFind PKey.textField.readOnly
    |> Option.iter (fun v -> view.ReadOnly <- v)

    props
    |> Props.tryFind PKey.textField.scrollOffset
    |> Option.iter (fun v -> view.ScrollOffset <- v)

    props
    |> Props.tryFind PKey.textField.secret
    |> Option.iter (fun v -> view.Secret <- v)

    props
    |> Props.tryFind PKey.textField.selectedLength
    |> Option.iter (fun v -> view.SelectedLength <- v)

    props
    |> Props.tryFind PKey.textField.selectedStart
    |> Option.iter (fun v -> view.SelectedStart <- v)

    props
    |> Props.tryFind PKey.textField.selectedText
    |> Option.iter (fun v -> view.SelectedText <- v)

    props
    |> Props.tryFind PKey.textField.text
    |> Option.iter (fun v -> view.Text <- v)

    props
    |> Props.tryFind PKey.textField.used
    |> Option.iter (fun v -> view.Used <- v)

    props
    |> Props.tryFind PKey.textField.useSameRuneTypeForWords
    |> Option.iter (fun v -> view.UseSameRuneTypeForWords <- v)

    props
    |> Props.tryFind PKey.textField.selectWordOnlyOnDoubleClick
    |> Option.iter (fun v -> view.SelectWordOnlyOnDoubleClick <- v)

    // Events
    terminalElement.trySetEventHandler(PKey.textField.textChanging, view.TextChanging)


  override _.removeProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.removeProps(terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> Terminal.Gui.Views.TextField

    // Properties
    props
    |> Props.tryFind PKey.textField.autocomplete
    |> Option.iter (fun _ -> 
        view.Autocomplete <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.textField.contextMenu
    |> Option.iter (fun _ -> 
        view.ContextMenu <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.textField.cursorPosition
    |> Option.iter (fun _ -> 
        view.CursorPosition <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.textField.readOnly
    |> Option.iter (fun _ -> 
        view.ReadOnly <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.textField.scrollOffset
    |> Option.iter (fun _ -> 
        view.ScrollOffset <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.textField.secret
    |> Option.iter (fun _ -> 
        view.Secret <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.textField.selectedLength
    |> Option.iter (fun _ -> 
        view.SelectedLength <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.textField.selectedStart
    |> Option.iter (fun _ -> 
        view.SelectedStart <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.textField.selectedText
    |> Option.iter (fun _ -> 
        view.SelectedText <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.textField.text
    |> Option.iter (fun _ -> 
        view.Text <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.textField.used
    |> Option.iter (fun _ -> 
        view.Used <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.textField.useSameRuneTypeForWords
    |> Option.iter (fun _ -> 
        view.UseSameRuneTypeForWords <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.textField.selectWordOnlyOnDoubleClick
    |> Option.iter (fun _ -> 
        view.SelectWordOnlyOnDoubleClick <- Unchecked.defaultof<_>)

    // Events
    terminalElement.tryRemoveEventHandler PKey.TextField.TextChanging

type internal TextValidateFieldTerminalElement(props: Props) =
  inherit ViewTerminalElement(props)

  override _.name = "TextValidateField"

  override _.newView() = new Terminal.Gui.Views.TextValidateField()

  override _.setProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.setProps(terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> Terminal.Gui.Views.TextValidateField

    // Properties
    props
    |> Props.tryFind PKey.textValidateField.provider
    |> Option.iter (fun v -> view.Provider <- v)

    props
    |> Props.tryFind PKey.textValidateField.text
    |> Option.iter (fun v -> view.Text <- v)

    // Events

  override _.removeProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.removeProps(terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> Terminal.Gui.Views.TextValidateField

    // Properties
    props
    |> Props.tryFind PKey.textValidateField.provider
    |> Option.iter (fun _ -> 
        view.Provider <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.textValidateField.text
    |> Option.iter (fun _ -> 
        view.Text <- Unchecked.defaultof<_>)


type internal TextViewTerminalElement(props: Props) =
  inherit ViewTerminalElement(props)

  override _.name = "TextView"

  override _.newView() = new Terminal.Gui.Views.TextView()

  override _.setProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.setProps(terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> Terminal.Gui.Views.TextView

    // Properties
    props
    |> Props.tryFind PKey.textView.allowsReturn
    |> Option.iter (fun v -> view.AllowsReturn <- v)

    props
    |> Props.tryFind PKey.textView.allowsTab
    |> Option.iter (fun v -> view.AllowsTab <- v)

    props
    |> Props.tryFind PKey.textView.autocomplete
    |> Option.iter (fun v -> view.Autocomplete <- v)

    props
    |> Props.tryFind PKey.textView.contextMenu
    |> Option.iter (fun v -> view.ContextMenu <- v)

    props
    |> Props.tryFind PKey.textView.currentColumn
    |> Option.iter (fun v -> view.CurrentColumn <- v)

    props
    |> Props.tryFind PKey.textView.currentRow
    |> Option.iter (fun v -> view.CurrentRow <- v)

    props
    |> Props.tryFind PKey.textView.cursorPosition
    |> Option.iter (fun v -> view.CursorPosition <- v)

    props
    |> Props.tryFind PKey.textView.inheritsPreviousAttribute
    |> Option.iter (fun v -> view.InheritsPreviousAttribute <- v)

    props
    |> Props.tryFind PKey.textView.isDirty
    |> Option.iter (fun v -> view.IsDirty <- v)

    props
    |> Props.tryFind PKey.textView.leftColumn
    |> Option.iter (fun v -> view.LeftColumn <- v)

    props
    |> Props.tryFind PKey.textView.multiline
    |> Option.iter (fun v -> view.Multiline <- v)

    props
    |> Props.tryFind PKey.textView.readOnly
    |> Option.iter (fun v -> view.ReadOnly <- v)

    props
    |> Props.tryFind PKey.textView.isSelecting
    |> Option.iter (fun v -> view.IsSelecting <- v)

    props
    |> Props.tryFind PKey.textView.selectionStartColumn
    |> Option.iter (fun v -> view.SelectionStartColumn <- v)

    props
    |> Props.tryFind PKey.textView.selectionStartRow
    |> Option.iter (fun v -> view.SelectionStartRow <- v)

    props
    |> Props.tryFind PKey.textView.tabWidth
    |> Option.iter (fun v -> view.TabWidth <- v)

    props
    |> Props.tryFind PKey.textView.text
    |> Option.iter (fun v -> view.Text <- v)

    props
    |> Props.tryFind PKey.textView.topRow
    |> Option.iter (fun v -> view.TopRow <- v)

    props
    |> Props.tryFind PKey.textView.used
    |> Option.iter (fun v -> view.Used <- v)

    props
    |> Props.tryFind PKey.textView.wordWrap
    |> Option.iter (fun v -> view.WordWrap <- v)

    props
    |> Props.tryFind PKey.textView.useSameRuneTypeForWords
    |> Option.iter (fun v -> view.UseSameRuneTypeForWords <- v)

    props
    |> Props.tryFind PKey.textView.selectWordOnlyOnDoubleClick
    |> Option.iter (fun v -> view.SelectWordOnlyOnDoubleClick <- v)

    // Events
    terminalElement.trySetEventHandler(PKey.textView.contentsChanged, view.ContentsChanged)

    terminalElement.trySetEventHandler(PKey.textView.drawNormalColor, view.DrawNormalColor)

    terminalElement.trySetEventHandler(PKey.textView.drawReadOnlyColor, view.DrawReadOnlyColor)

    terminalElement.trySetEventHandler(PKey.textView.drawSelectionColor, view.DrawSelectionColor)

    terminalElement.trySetEventHandler(PKey.textView.drawUsedColor, view.DrawUsedColor)

    terminalElement.trySetEventHandler(PKey.textView.unwrappedCursorPosition, view.UnwrappedCursorPosition)


  override _.removeProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.removeProps(terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> Terminal.Gui.Views.TextView

    // Properties
    props
    |> Props.tryFind PKey.textView.allowsReturn
    |> Option.iter (fun _ -> 
        view.AllowsReturn <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.textView.allowsTab
    |> Option.iter (fun _ -> 
        view.AllowsTab <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.textView.autocomplete
    |> Option.iter (fun _ -> 
        view.Autocomplete <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.textView.contextMenu
    |> Option.iter (fun _ -> 
        view.ContextMenu <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.textView.currentColumn
    |> Option.iter (fun _ -> 
        view.CurrentColumn <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.textView.currentRow
    |> Option.iter (fun _ -> 
        view.CurrentRow <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.textView.cursorPosition
    |> Option.iter (fun _ -> 
        view.CursorPosition <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.textView.inheritsPreviousAttribute
    |> Option.iter (fun _ -> 
        view.InheritsPreviousAttribute <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.textView.isDirty
    |> Option.iter (fun _ -> 
        view.IsDirty <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.textView.leftColumn
    |> Option.iter (fun _ -> 
        view.LeftColumn <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.textView.multiline
    |> Option.iter (fun _ -> 
        view.Multiline <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.textView.readOnly
    |> Option.iter (fun _ -> 
        view.ReadOnly <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.textView.isSelecting
    |> Option.iter (fun _ -> 
        view.IsSelecting <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.textView.selectionStartColumn
    |> Option.iter (fun _ -> 
        view.SelectionStartColumn <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.textView.selectionStartRow
    |> Option.iter (fun _ -> 
        view.SelectionStartRow <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.textView.tabWidth
    |> Option.iter (fun _ -> 
        view.TabWidth <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.textView.text
    |> Option.iter (fun _ -> 
        view.Text <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.textView.topRow
    |> Option.iter (fun _ -> 
        view.TopRow <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.textView.used
    |> Option.iter (fun _ -> 
        view.Used <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.textView.wordWrap
    |> Option.iter (fun _ -> 
        view.WordWrap <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.textView.useSameRuneTypeForWords
    |> Option.iter (fun _ -> 
        view.UseSameRuneTypeForWords <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.textView.selectWordOnlyOnDoubleClick
    |> Option.iter (fun _ -> 
        view.SelectWordOnlyOnDoubleClick <- Unchecked.defaultof<_>)

    // Events
    terminalElement.tryRemoveEventHandler PKey.TextView.ContentsChanged
    terminalElement.tryRemoveEventHandler PKey.TextView.DrawNormalColor
    terminalElement.tryRemoveEventHandler PKey.TextView.DrawReadOnlyColor
    terminalElement.tryRemoveEventHandler PKey.TextView.DrawSelectionColor
    terminalElement.tryRemoveEventHandler PKey.TextView.DrawUsedColor
    terminalElement.tryRemoveEventHandler PKey.TextView.UnwrappedCursorPosition

type internal TimeFieldTerminalElement(props: Props) =
  inherit TextFieldTerminalElement(props)

  override _.name = "TimeField"

  override _.newView() = new Terminal.Gui.Views.TimeField()

  override _.setProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.setProps(terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> Terminal.Gui.Views.TimeField

    // Properties
    props
    |> Props.tryFind PKey.timeField.cursorPosition
    |> Option.iter (fun v -> view.CursorPosition <- v)

    props
    |> Props.tryFind PKey.timeField.isShortFormat
    |> Option.iter (fun v -> view.IsShortFormat <- v)

    props
    |> Props.tryFind PKey.timeField.time
    |> Option.iter (fun v -> view.Time <- v)

    // Events
    terminalElement.trySetEventHandler(PKey.timeField.timeChanged, view.TimeChanged)


  override _.removeProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.removeProps(terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> Terminal.Gui.Views.TimeField

    // Properties
    props
    |> Props.tryFind PKey.timeField.cursorPosition
    |> Option.iter (fun _ -> 
        view.CursorPosition <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.timeField.isShortFormat
    |> Option.iter (fun _ -> 
        view.IsShortFormat <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.timeField.time
    |> Option.iter (fun _ -> 
        view.Time <- Unchecked.defaultof<_>)

    // Events
    terminalElement.tryRemoveEventHandler PKey.TimeField.TimeChanged

type internal TreeView`1TerminalElement(props: Props) =
  inherit ViewTerminalElement(props)

  override _.name = "TreeView`1"

  override _.newView() = new Terminal.Gui.Views.TreeView`1()

  override _.setProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.setProps(terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> Terminal.Gui.Views.TreeView`1

    // Properties
    props
    |> Props.tryFind PKey.treeView`1.allowLetterBasedNavigation
    |> Option.iter (fun v -> view.AllowLetterBasedNavigation <- v)

    props
    |> Props.tryFind PKey.treeView`1.aspectGetter
    |> Option.iter (fun v -> view.AspectGetter <- v)

    props
    |> Props.tryFind PKey.treeView`1.colorGetter
    |> Option.iter (fun v -> view.ColorGetter <- v)

    props
    |> Props.tryFind PKey.treeView`1.maxDepth
    |> Option.iter (fun v -> view.MaxDepth <- v)

    props
    |> Props.tryFind PKey.treeView`1.multiSelect
    |> Option.iter (fun v -> view.MultiSelect <- v)

    props
    |> Props.tryFind PKey.treeView`1.objectActivationButton
    |> Option.iter (fun v -> view.ObjectActivationButton <- v)

    props
    |> Props.tryFind PKey.treeView`1.objectActivationKey
    |> Option.iter (fun v -> view.ObjectActivationKey <- v)

    props
    |> Props.tryFind PKey.treeView`1.scrollOffsetHorizontal
    |> Option.iter (fun v -> view.ScrollOffsetHorizontal <- v)

    props
    |> Props.tryFind PKey.treeView`1.scrollOffsetVertical
    |> Option.iter (fun v -> view.ScrollOffsetVertical <- v)

    props
    |> Props.tryFind PKey.treeView`1.selectedObject
    |> Option.iter (fun v -> view.SelectedObject <- v)

    props
    |> Props.tryFind PKey.treeView`1.treeBuilder
    |> Option.iter (fun v -> view.TreeBuilder <- v)

    props
    |> Props.tryFind PKey.treeView`1.style
    |> Option.iter (fun v -> view.Style <- v)

    // Events
    terminalElement.trySetEventHandler(PKey.treeView`1.drawLine, view.DrawLine)

    terminalElement.trySetEventHandler(PKey.treeView`1.objectActivated, view.ObjectActivated)

    terminalElement.trySetEventHandler(PKey.treeView`1.selectionChanged, view.SelectionChanged)


  override _.removeProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.removeProps(terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> Terminal.Gui.Views.TreeView`1

    // Properties
    props
    |> Props.tryFind PKey.treeView`1.allowLetterBasedNavigation
    |> Option.iter (fun _ -> 
        view.AllowLetterBasedNavigation <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.treeView`1.aspectGetter
    |> Option.iter (fun _ -> 
        view.AspectGetter <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.treeView`1.colorGetter
    |> Option.iter (fun _ -> 
        view.ColorGetter <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.treeView`1.maxDepth
    |> Option.iter (fun _ -> 
        view.MaxDepth <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.treeView`1.multiSelect
    |> Option.iter (fun _ -> 
        view.MultiSelect <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.treeView`1.objectActivationButton
    |> Option.iter (fun _ -> 
        view.ObjectActivationButton <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.treeView`1.objectActivationKey
    |> Option.iter (fun _ -> 
        view.ObjectActivationKey <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.treeView`1.scrollOffsetHorizontal
    |> Option.iter (fun _ -> 
        view.ScrollOffsetHorizontal <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.treeView`1.scrollOffsetVertical
    |> Option.iter (fun _ -> 
        view.ScrollOffsetVertical <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.treeView`1.selectedObject
    |> Option.iter (fun _ -> 
        view.SelectedObject <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.treeView`1.treeBuilder
    |> Option.iter (fun _ -> 
        view.TreeBuilder <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.treeView`1.style
    |> Option.iter (fun _ -> 
        view.Style <- Unchecked.defaultof<_>)

    // Events
    terminalElement.tryRemoveEventHandler PKey.TreeView`1.DrawLine
    terminalElement.tryRemoveEventHandler PKey.TreeView`1.ObjectActivated
    terminalElement.tryRemoveEventHandler PKey.TreeView`1.SelectionChanged

type internal ValueBarTerminalElement(props: Props) =
  inherit ColorBarTerminalElement(props)

  override _.name = "ValueBar"

  override _.newView() = new Terminal.Gui.Views.ValueBar()

  override _.setProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.setProps(terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> Terminal.Gui.Views.ValueBar

    // Properties
    props
    |> Props.tryFind PKey.valueBar.hBar
    |> Option.iter (fun v -> view.HBar <- v)

    props
    |> Props.tryFind PKey.valueBar.sBar
    |> Option.iter (fun v -> view.SBar <- v)

    // Events

  override _.removeProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.removeProps(terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> Terminal.Gui.Views.ValueBar

    // Properties
    props
    |> Props.tryFind PKey.valueBar.hBar
    |> Option.iter (fun _ -> 
        view.HBar <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.valueBar.sBar
    |> Option.iter (fun _ -> 
        view.SBar <- Unchecked.defaultof<_>)


type internal WindowTerminalElement(props: Props) =
  inherit RunnableTerminalElement(props)

  override _.name = "Window"

  override _.newView() = new Terminal.Gui.Views.Window()

  override _.setProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.setProps(terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> Terminal.Gui.Views.Window

    // Properties
    // Events

  override _.removeProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.removeProps(terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> Terminal.Gui.Views.Window


type internal WizardTerminalElement(props: Props) =
  inherit DialogTerminalElement(props)

  override _.name = "Wizard"

  override _.newView() = new Terminal.Gui.Views.Wizard()

  override _.setProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.setProps(terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> Terminal.Gui.Views.Wizard

    // Properties
    props
    |> Props.tryFind PKey.wizard.currentStep
    |> Option.iter (fun v -> view.CurrentStep <- v)

    // Events
    terminalElement.trySetEventHandler(PKey.wizard.cancelled, view.Cancelled)

    terminalElement.trySetEventHandler(PKey.wizard.finished, view.Finished)

    terminalElement.trySetEventHandler(PKey.wizard.movingBack, view.MovingBack)

    terminalElement.trySetEventHandler(PKey.wizard.movingNext, view.MovingNext)

    terminalElement.trySetEventHandler(PKey.wizard.stepChanged, view.StepChanged)

    terminalElement.trySetEventHandler(PKey.wizard.stepChanging, view.StepChanging)


  override _.removeProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.removeProps(terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> Terminal.Gui.Views.Wizard

    // Properties
    props
    |> Props.tryFind PKey.wizard.currentStep
    |> Option.iter (fun _ -> 
        view.CurrentStep <- Unchecked.defaultof<_>)

    // Events
    terminalElement.tryRemoveEventHandler PKey.Wizard.Cancelled
    terminalElement.tryRemoveEventHandler PKey.Wizard.Finished
    terminalElement.tryRemoveEventHandler PKey.Wizard.MovingBack
    terminalElement.tryRemoveEventHandler PKey.Wizard.MovingNext
    terminalElement.tryRemoveEventHandler PKey.Wizard.StepChanged
    terminalElement.tryRemoveEventHandler PKey.Wizard.StepChanging

type internal WizardStepTerminalElement(props: Props) =
  inherit ViewTerminalElement(props)

  override _.name = "WizardStep"

  override _.newView() = new Terminal.Gui.Views.WizardStep()

  override _.setProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.setProps(terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> Terminal.Gui.Views.WizardStep

    // Properties
    props
    |> Props.tryFind PKey.wizardStep.backButtonText
    |> Option.iter (fun v -> view.BackButtonText <- v)

    props
    |> Props.tryFind PKey.wizardStep.helpText
    |> Option.iter (fun v -> view.HelpText <- v)

    props
    |> Props.tryFind PKey.wizardStep.nextButtonText
    |> Option.iter (fun v -> view.NextButtonText <- v)

    // Events

  override _.removeProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.removeProps(terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> Terminal.Gui.Views.WizardStep

    // Properties
    props
    |> Props.tryFind PKey.wizardStep.backButtonText
    |> Option.iter (fun _ -> 
        view.BackButtonText <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.wizardStep.helpText
    |> Option.iter (fun _ -> 
        view.HelpText <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.wizardStep.nextButtonText
    |> Option.iter (fun _ -> 
        view.NextButtonText <- Unchecked.defaultof<_>)

