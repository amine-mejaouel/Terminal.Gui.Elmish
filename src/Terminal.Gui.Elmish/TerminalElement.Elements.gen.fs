namespace Terminal.Gui.Elmish

open Terminal.Gui.App
open Terminal.Gui.ViewBase
open Terminal.Gui.Views


type internal ViewTerminalElement(props: Props) =
  inherit TerminalElement(props)

  override _.name = "View"

  override _.newView() = new View()

  override _.setProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.setProps(terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View

    // Properties
    props
    |> Props.tryFind PKey.view.arrangement
    |> Option.iter (fun v -> view.Arrangement <- v)

    props
    |> Props.tryFind PKey.view.borderStyle
    |> Option.iter (fun v -> view.BorderStyle <- v)

    props
    |> Props.tryFind PKey.view.canFocus
    |> Option.iter (fun v -> view.CanFocus <- v)

    props
    |> Props.tryFind PKey.view.contentSizeTracksViewport
    |> Option.iter (fun v -> view.ContentSizeTracksViewport <- v)

    props
    |> Props.tryFind PKey.view.cursorVisibility
    |> Option.iter (fun v -> view.CursorVisibility <- v)

    props
    |> Props.tryFind PKey.view.data
    |> Option.iter (fun v -> view.Data <- v)

    props
    |> Props.tryFind PKey.view.enabled
    |> Option.iter (fun v -> view.Enabled <- v)

    props
    |> Props.tryFind PKey.view.frame
    |> Option.iter (fun v -> view.Frame <- v)

    props
    |> Props.tryFind PKey.view.hasFocus
    |> Option.iter (fun v -> view.HasFocus <- v)

    props
    |> Props.tryFind PKey.view.height
    |> Option.iter (fun v -> view.Height <- v)

    props
    |> Props.tryFind PKey.view.highlightStates
    |> Option.iter (fun v -> view.HighlightStates <- v)

    props
    |> Props.tryFind PKey.view.hotKey
    |> Option.iter (fun v -> view.HotKey <- v)

    props
    |> Props.tryFind PKey.view.hotKeySpecifier
    |> Option.iter (fun v -> view.HotKeySpecifier <- v)

    props
    |> Props.tryFind PKey.view.id
    |> Option.iter (fun v -> view.Id <- v)

    props
    |> Props.tryFind PKey.view.isInitialized
    |> Option.iter (fun v -> view.IsInitialized <- v)

    props
    |> Props.tryFind PKey.view.mouseHeldDown
    |> Option.iter (fun v -> view.MouseHeldDown <- v)

    props
    |> Props.tryFind PKey.view.preserveTrailingSpaces
    |> Option.iter (fun v -> view.PreserveTrailingSpaces <- v)

    props
    |> Props.tryFind PKey.view.schemeName
    |> Option.iter (fun v -> view.SchemeName <- v)

    props
    |> Props.tryFind PKey.view.shadowStyle
    |> Option.iter (fun v -> view.ShadowStyle <- v)

    props
    |> Props.tryFind PKey.view.superViewRendersLineCanvas
    |> Option.iter (fun v -> view.SuperViewRendersLineCanvas <- v)

    props
    |> Props.tryFind PKey.view.tabStop
    |> Option.iter (fun v -> view.TabStop <- v |> Option.toNullable)

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
    |> Props.tryFind PKey.view.title
    |> Option.iter (fun v -> view.Title <- v)

    props
    |> Props.tryFind PKey.view.validatePosDim
    |> Option.iter (fun v -> view.ValidatePosDim <- v)

    props
    |> Props.tryFind PKey.view.verticalTextAlignment
    |> Option.iter (fun v -> view.VerticalTextAlignment <- v)

    props
    |> Props.tryFind PKey.view.viewport
    |> Option.iter (fun v -> view.Viewport <- v)

    props
    |> Props.tryFind PKey.view.viewportSettings
    |> Option.iter (fun v -> view.ViewportSettings <- v)

    props
    |> Props.tryFind PKey.view.visible
    |> Option.iter (fun v -> view.Visible <- v)

    props
    |> Props.tryFind PKey.view.wantContinuousButtonPressed
    |> Option.iter (fun v -> view.WantContinuousButtonPressed <- v)

    props
    |> Props.tryFind PKey.view.wantMousePositionReports
    |> Option.iter (fun v -> view.WantMousePositionReports <- v)

    props
    |> Props.tryFind PKey.view.width
    |> Option.iter (fun v -> view.Width <- v)

    props
    |> Props.tryFind PKey.view.x
    |> Option.iter (fun v -> view.X <- v)

    props
    |> Props.tryFind PKey.view.y
    |> Option.iter (fun v -> view.Y <- v)

    // Events
    terminalElement.trySetEventHandler(PKey.view.accepted, view.Accepted)

    terminalElement.trySetEventHandler(PKey.view.accepting, view.Accepting)

    terminalElement.trySetEventHandler(PKey.view.activating, view.Activating)

    terminalElement.trySetEventHandler(PKey.view.advancingFocus, view.AdvancingFocus)

    terminalElement.trySetEventHandler(PKey.view.borderStyleChanged, view.BorderStyleChanged)

    terminalElement.trySetEventHandler(PKey.view.canFocusChanged, view.CanFocusChanged)

    terminalElement.trySetEventHandler(PKey.view.clearedViewport, view.ClearedViewport)

    terminalElement.trySetEventHandler(PKey.view.clearingViewport, view.ClearingViewport)

    terminalElement.trySetEventHandler(PKey.view.commandNotBound, view.CommandNotBound)

    terminalElement.trySetEventHandler(PKey.view.contentSizeChanged, view.ContentSizeChanged)

    terminalElement.trySetEventHandler(PKey.view.disposing, view.Disposing)

    terminalElement.trySetEventHandler(PKey.view.drawComplete, view.DrawComplete)

    terminalElement.trySetEventHandler(PKey.view.drawingContent, view.DrawingContent)

    terminalElement.trySetEventHandler(PKey.view.drawingSubViews, view.DrawingSubViews)

    terminalElement.trySetEventHandler(PKey.view.drawingText, view.DrawingText)

    terminalElement.trySetEventHandler(PKey.view.drewText, view.DrewText)

    terminalElement.trySetEventHandler(PKey.view.enabledChanged, view.EnabledChanged)

    terminalElement.trySetEventHandler(PKey.view.focusedChanged, view.FocusedChanged)

    terminalElement.trySetEventHandler(PKey.view.frameChanged, view.FrameChanged)

    terminalElement.trySetEventHandler(PKey.view.gettingAttributeForRole, view.GettingAttributeForRole)

    terminalElement.trySetEventHandler(PKey.view.gettingScheme, view.GettingScheme)

    terminalElement.trySetEventHandler(PKey.view.handlingHotKey, view.HandlingHotKey)

    terminalElement.trySetEventHandler(PKey.view.hasFocusChanged, view.HasFocusChanged)

    terminalElement.trySetEventHandler(PKey.view.hasFocusChanging, view.HasFocusChanging)

    terminalElement.trySetEventHandler(PKey.view.heightChanged, view.HeightChanged)

    terminalElement.trySetEventHandler(PKey.view.heightChanging, view.HeightChanging)

    terminalElement.trySetEventHandler(PKey.view.hotKeyChanged, view.HotKeyChanged)

    terminalElement.trySetEventHandler(PKey.view.initialized, view.Initialized)

    terminalElement.trySetEventHandler(PKey.view.keyDown, view.KeyDown)

    terminalElement.trySetEventHandler(PKey.view.keyDownNotHandled, view.KeyDownNotHandled)

    terminalElement.trySetEventHandler(PKey.view.keyUp, view.KeyUp)

    terminalElement.trySetEventHandler(PKey.view.mouseEnter, view.MouseEnter)

    terminalElement.trySetEventHandler(PKey.view.mouseEvent, view.MouseEvent)

    terminalElement.trySetEventHandler(PKey.view.mouseLeave, view.MouseLeave)

    terminalElement.trySetEventHandler(PKey.view.mouseStateChanged, view.MouseStateChanged)

    terminalElement.trySetEventHandler(PKey.view.mouseWheel, view.MouseWheel)

    terminalElement.trySetEventHandler(PKey.view.removed, view.Removed)

    terminalElement.trySetEventHandler(PKey.view.schemeChanged, view.SchemeChanged)

    terminalElement.trySetEventHandler(PKey.view.schemeChanging, view.SchemeChanging)

    terminalElement.trySetEventHandler(PKey.view.schemeNameChanged, view.SchemeNameChanged)

    terminalElement.trySetEventHandler(PKey.view.schemeNameChanging, view.SchemeNameChanging)

    terminalElement.trySetEventHandler(PKey.view.subViewAdded, view.SubViewAdded)

    terminalElement.trySetEventHandler(PKey.view.subViewLayout, view.SubViewLayout)

    terminalElement.trySetEventHandler(PKey.view.subViewRemoved, view.SubViewRemoved)

    terminalElement.trySetEventHandler(PKey.view.subViewsLaidOut, view.SubViewsLaidOut)

    terminalElement.trySetEventHandler(PKey.view.superViewChanged, view.SuperViewChanged)

    terminalElement.trySetEventHandler(PKey.view.textChanged, view.TextChanged)

    terminalElement.trySetEventHandler(PKey.view.titleChanged, view.TitleChanged)

    terminalElement.trySetEventHandler(PKey.view.titleChanging, view.TitleChanging)

    terminalElement.trySetEventHandler(PKey.view.viewportChanged, view.ViewportChanged)

    terminalElement.trySetEventHandler(PKey.view.visibleChanged, view.VisibleChanged)

    terminalElement.trySetEventHandler(PKey.view.visibleChanging, view.VisibleChanging)

    terminalElement.trySetEventHandler(PKey.view.widthChanged, view.WidthChanged)

    terminalElement.trySetEventHandler(PKey.view.widthChanging, view.WidthChanging)


  override _.removeProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.removeProps(terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View

    // Properties
    props
    |> Props.tryFind PKey.view.arrangement
    |> Option.iter (fun _ ->
        view.Arrangement <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.view.borderStyle
    |> Option.iter (fun _ ->
        view.BorderStyle <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.view.canFocus
    |> Option.iter (fun _ ->
        view.CanFocus <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.view.contentSizeTracksViewport
    |> Option.iter (fun _ ->
        view.ContentSizeTracksViewport <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.view.cursorVisibility
    |> Option.iter (fun _ ->
        view.CursorVisibility <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.view.data
    |> Option.iter (fun _ ->
        view.Data <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.view.enabled
    |> Option.iter (fun _ ->
        view.Enabled <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.view.frame
    |> Option.iter (fun _ ->
        view.Frame <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.view.hasFocus
    |> Option.iter (fun _ ->
        view.HasFocus <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.view.height
    |> Option.iter (fun _ ->
        view.Height <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.view.highlightStates
    |> Option.iter (fun _ ->
        view.HighlightStates <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.view.hotKey
    |> Option.iter (fun _ ->
        view.HotKey <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.view.hotKeySpecifier
    |> Option.iter (fun _ ->
        view.HotKeySpecifier <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.view.id
    |> Option.iter (fun _ ->
        view.Id <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.view.isInitialized
    |> Option.iter (fun _ ->
        view.IsInitialized <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.view.mouseHeldDown
    |> Option.iter (fun _ ->
        view.MouseHeldDown <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.view.preserveTrailingSpaces
    |> Option.iter (fun _ ->
        view.PreserveTrailingSpaces <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.view.schemeName
    |> Option.iter (fun _ ->
        view.SchemeName <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.view.shadowStyle
    |> Option.iter (fun _ ->
        view.ShadowStyle <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.view.superViewRendersLineCanvas
    |> Option.iter (fun _ ->
        view.SuperViewRendersLineCanvas <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.view.tabStop
    |> Option.iter (fun _ ->
        view.TabStop <- Unchecked.defaultof<_>)

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
    |> Props.tryFind PKey.view.title
    |> Option.iter (fun _ ->
        view.Title <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.view.validatePosDim
    |> Option.iter (fun _ ->
        view.ValidatePosDim <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.view.verticalTextAlignment
    |> Option.iter (fun _ ->
        view.VerticalTextAlignment <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.view.viewport
    |> Option.iter (fun _ ->
        view.Viewport <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.view.viewportSettings
    |> Option.iter (fun _ ->
        view.ViewportSettings <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.view.visible
    |> Option.iter (fun _ ->
        view.Visible <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.view.wantContinuousButtonPressed
    |> Option.iter (fun _ ->
        view.WantContinuousButtonPressed <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.view.wantMousePositionReports
    |> Option.iter (fun _ ->
        view.WantMousePositionReports <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.view.width
    |> Option.iter (fun _ ->
        view.Width <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.view.x
    |> Option.iter (fun _ ->
        view.X <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.view.y
    |> Option.iter (fun _ ->
        view.Y <- Unchecked.defaultof<_>)

    // Events
    terminalElement.tryRemoveEventHandler PKey.view.accepted
    terminalElement.tryRemoveEventHandler PKey.view.accepting
    terminalElement.tryRemoveEventHandler PKey.view.activating
    terminalElement.tryRemoveEventHandler PKey.view.advancingFocus
    terminalElement.tryRemoveEventHandler PKey.view.borderStyleChanged
    terminalElement.tryRemoveEventHandler PKey.view.canFocusChanged
    terminalElement.tryRemoveEventHandler PKey.view.clearedViewport
    terminalElement.tryRemoveEventHandler PKey.view.clearingViewport
    terminalElement.tryRemoveEventHandler PKey.view.commandNotBound
    terminalElement.tryRemoveEventHandler PKey.view.contentSizeChanged
    terminalElement.tryRemoveEventHandler PKey.view.disposing
    terminalElement.tryRemoveEventHandler PKey.view.drawComplete
    terminalElement.tryRemoveEventHandler PKey.view.drawingContent
    terminalElement.tryRemoveEventHandler PKey.view.drawingSubViews
    terminalElement.tryRemoveEventHandler PKey.view.drawingText
    terminalElement.tryRemoveEventHandler PKey.view.drewText
    terminalElement.tryRemoveEventHandler PKey.view.enabledChanged
    terminalElement.tryRemoveEventHandler PKey.view.focusedChanged
    terminalElement.tryRemoveEventHandler PKey.view.frameChanged
    terminalElement.tryRemoveEventHandler PKey.view.gettingAttributeForRole
    terminalElement.tryRemoveEventHandler PKey.view.gettingScheme
    terminalElement.tryRemoveEventHandler PKey.view.handlingHotKey
    terminalElement.tryRemoveEventHandler PKey.view.hasFocusChanged
    terminalElement.tryRemoveEventHandler PKey.view.hasFocusChanging
    terminalElement.tryRemoveEventHandler PKey.view.heightChanged
    terminalElement.tryRemoveEventHandler PKey.view.heightChanging
    terminalElement.tryRemoveEventHandler PKey.view.hotKeyChanged
    terminalElement.tryRemoveEventHandler PKey.view.initialized
    terminalElement.tryRemoveEventHandler PKey.view.keyDown
    terminalElement.tryRemoveEventHandler PKey.view.keyDownNotHandled
    terminalElement.tryRemoveEventHandler PKey.view.keyUp
    terminalElement.tryRemoveEventHandler PKey.view.mouseEnter
    terminalElement.tryRemoveEventHandler PKey.view.mouseEvent
    terminalElement.tryRemoveEventHandler PKey.view.mouseLeave
    terminalElement.tryRemoveEventHandler PKey.view.mouseStateChanged
    terminalElement.tryRemoveEventHandler PKey.view.mouseWheel
    terminalElement.tryRemoveEventHandler PKey.view.removed
    terminalElement.tryRemoveEventHandler PKey.view.schemeChanged
    terminalElement.tryRemoveEventHandler PKey.view.schemeChanging
    terminalElement.tryRemoveEventHandler PKey.view.schemeNameChanged
    terminalElement.tryRemoveEventHandler PKey.view.schemeNameChanging
    terminalElement.tryRemoveEventHandler PKey.view.subViewAdded
    terminalElement.tryRemoveEventHandler PKey.view.subViewLayout
    terminalElement.tryRemoveEventHandler PKey.view.subViewRemoved
    terminalElement.tryRemoveEventHandler PKey.view.subViewsLaidOut
    terminalElement.tryRemoveEventHandler PKey.view.superViewChanged
    terminalElement.tryRemoveEventHandler PKey.view.textChanged
    terminalElement.tryRemoveEventHandler PKey.view.titleChanged
    terminalElement.tryRemoveEventHandler PKey.view.titleChanging
    terminalElement.tryRemoveEventHandler PKey.view.viewportChanged
    terminalElement.tryRemoveEventHandler PKey.view.visibleChanged
    terminalElement.tryRemoveEventHandler PKey.view.visibleChanging
    terminalElement.tryRemoveEventHandler PKey.view.widthChanged
    terminalElement.tryRemoveEventHandler PKey.view.widthChanging

type internal AdornmentTerminalElement(props: Props) =
  inherit ViewTerminalElement(props)

  override _.name = "Adornment"

  override _.newView() = new Adornment()

  override _.setProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.setProps(terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> Adornment

    // Properties
    props
    |> Props.tryFind PKey.adornment.diagnostics
    |> Option.iter (fun v -> view.Diagnostics <- v)

    props
    |> Props.tryFind PKey.adornment.parent
    |> Option.iter (fun v -> view.Parent <- v)

    props
    |> Props.tryFind PKey.adornment.superViewRendersLineCanvas
    |> Option.iter (fun v -> view.SuperViewRendersLineCanvas <- v)

    props
    |> Props.tryFind PKey.adornment.thickness
    |> Option.iter (fun v -> view.Thickness <- v)

    props
    |> Props.tryFind PKey.adornment.viewport
    |> Option.iter (fun v -> view.Viewport <- v)

    // Events
    terminalElement.trySetEventHandler(PKey.adornment.thicknessChanged, view.ThicknessChanged)


  override _.removeProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.removeProps(terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> Adornment

    // Properties
    props
    |> Props.tryFind PKey.adornment.diagnostics
    |> Option.iter (fun _ ->
        view.Diagnostics <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.adornment.parent
    |> Option.iter (fun _ ->
        view.Parent <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.adornment.superViewRendersLineCanvas
    |> Option.iter (fun _ ->
        view.SuperViewRendersLineCanvas <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.adornment.thickness
    |> Option.iter (fun _ ->
        view.Thickness <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.adornment.viewport
    |> Option.iter (fun _ ->
        view.Viewport <- Unchecked.defaultof<_>)

    // Events
    terminalElement.tryRemoveEventHandler PKey.adornment.thicknessChanged

type internal BarTerminalElement(props: Props) =
  inherit ViewTerminalElement(props)

  override _.name = "Bar"

  override _.newView() = new Bar()

  override _.setProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.setProps(terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> Bar

    // Properties
    props
    |> Props.tryFind PKey.bar.alignmentModes
    |> Option.iter (fun v -> view.AlignmentModes <- v)

    props
    |> Props.tryFind PKey.bar.orientation
    |> Option.iter (fun v -> view.Orientation <- v)

    // Events
    terminalElement.trySetEventHandler(PKey.bar.orientationChanged, view.OrientationChanged)

    terminalElement.trySetEventHandler(PKey.bar.orientationChanging, view.OrientationChanging)


  override _.removeProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.removeProps(terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> Bar

    // Properties
    props
    |> Props.tryFind PKey.bar.alignmentModes
    |> Option.iter (fun _ ->
        view.AlignmentModes <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.bar.orientation
    |> Option.iter (fun _ ->
        view.Orientation <- Unchecked.defaultof<_>)

    // Events
    terminalElement.tryRemoveEventHandler PKey.bar.orientationChanged
    terminalElement.tryRemoveEventHandler PKey.bar.orientationChanging

type internal BorderTerminalElement(props: Props) =
  inherit AdornmentTerminalElement(props)

  override _.name = "Border"

  override _.newView() = new Border()

  override _.setProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.setProps(terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> Border

    // Properties
    props
    |> Props.tryFind PKey.border.lineStyle
    |> Option.iter (fun v -> view.LineStyle <- v)

    props
    |> Props.tryFind PKey.border.settings
    |> Option.iter (fun v -> view.Settings <- v)

    // Events

  override _.removeProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.removeProps(terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> Border

    // Properties
    props
    |> Props.tryFind PKey.border.lineStyle
    |> Option.iter (fun _ ->
        view.LineStyle <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.border.settings
    |> Option.iter (fun _ ->
        view.Settings <- Unchecked.defaultof<_>)


type internal ButtonTerminalElement(props: Props) =
  inherit ViewTerminalElement(props)

  override _.name = "Button"

  override _.newView() = new Button()

  override _.setProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.setProps(terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> Button

    // Properties
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

    props
    |> Props.tryFind PKey.button.text
    |> Option.iter (fun v -> view.Text <- v)

    // Events

  override _.removeProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.removeProps(terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> Button

    // Properties
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

    props
    |> Props.tryFind PKey.button.text
    |> Option.iter (fun _ ->
        view.Text <- Unchecked.defaultof<_>)


type internal CharMapTerminalElement(props: Props) =
  inherit ViewTerminalElement(props)

  override _.name = "CharMap"

  override _.newView() = new CharMap()

  override _.setProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.setProps(terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> CharMap

    // Properties
    props
    |> Props.tryFind PKey.charMap.selectedCodePoint
    |> Option.iter (fun v -> view.SelectedCodePoint <- v)

    props
    |> Props.tryFind PKey.charMap.showGlyphWidths
    |> Option.iter (fun v -> view.ShowGlyphWidths <- v)

    props
    |> Props.tryFind PKey.charMap.showUnicodeCategory
    |> Option.iter (fun v -> view.ShowUnicodeCategory <- v |> Option.toNullable)

    props
    |> Props.tryFind PKey.charMap.startCodePoint
    |> Option.iter (fun v -> view.StartCodePoint <- v)

    // Events
    terminalElement.trySetEventHandler(PKey.charMap.selectedCodePointChanged, view.SelectedCodePointChanged)


  override _.removeProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.removeProps(terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> CharMap

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
    |> Props.tryFind PKey.charMap.showUnicodeCategory
    |> Option.iter (fun _ ->
        view.ShowUnicodeCategory <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.charMap.startCodePoint
    |> Option.iter (fun _ ->
        view.StartCodePoint <- Unchecked.defaultof<_>)

    // Events
    terminalElement.tryRemoveEventHandler PKey.charMap.selectedCodePointChanged

type internal CheckBoxTerminalElement(props: Props) =
  inherit ViewTerminalElement(props)

  override _.name = "CheckBox"

  override _.newView() = new CheckBox()

  override _.setProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.setProps(terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> CheckBox

    // Properties
    props
    |> Props.tryFind PKey.checkBox.allowCheckStateNone
    |> Option.iter (fun v -> view.AllowCheckStateNone <- v)

    props
    |> Props.tryFind PKey.checkBox.checkedState
    |> Option.iter (fun v -> view.CheckedState <- v)

    props
    |> Props.tryFind PKey.checkBox.hotKeySpecifier
    |> Option.iter (fun v -> view.HotKeySpecifier <- v)

    props
    |> Props.tryFind PKey.checkBox.radioStyle
    |> Option.iter (fun v -> view.RadioStyle <- v)

    props
    |> Props.tryFind PKey.checkBox.text
    |> Option.iter (fun v -> view.Text <- v)

    // Events
    terminalElement.trySetEventHandler(PKey.checkBox.checkedStateChanged, view.CheckedStateChanged)

    terminalElement.trySetEventHandler(PKey.checkBox.checkedStateChanging, view.CheckedStateChanging)


  override _.removeProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.removeProps(terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> CheckBox

    // Properties
    props
    |> Props.tryFind PKey.checkBox.allowCheckStateNone
    |> Option.iter (fun _ ->
        view.AllowCheckStateNone <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.checkBox.checkedState
    |> Option.iter (fun _ ->
        view.CheckedState <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.checkBox.hotKeySpecifier
    |> Option.iter (fun _ ->
        view.HotKeySpecifier <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.checkBox.radioStyle
    |> Option.iter (fun _ ->
        view.RadioStyle <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.checkBox.text
    |> Option.iter (fun _ ->
        view.Text <- Unchecked.defaultof<_>)

    // Events
    terminalElement.tryRemoveEventHandler PKey.checkBox.checkedStateChanged
    terminalElement.tryRemoveEventHandler PKey.checkBox.checkedStateChanging

type internal ColorPickerTerminalElement(props: Props) =
  inherit ViewTerminalElement(props)

  override _.name = "ColorPicker"

  override _.newView() = new ColorPicker()

  override _.setProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.setProps(terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> ColorPicker

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
    let view = terminalElement.View :?> ColorPicker

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
    terminalElement.tryRemoveEventHandler PKey.colorPicker.colorChanged

type internal ColorPicker16TerminalElement(props: Props) =
  inherit ViewTerminalElement(props)

  override _.name = "ColorPicker16"

  override _.newView() = new ColorPicker16()

  override _.setProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.setProps(terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> ColorPicker16

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
    let view = terminalElement.View :?> ColorPicker16

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
    terminalElement.tryRemoveEventHandler PKey.colorPicker16.colorChanged

type internal ComboBoxTerminalElement(props: Props) =
  inherit ViewTerminalElement(props)

  override _.name = "ComboBox"

  override _.newView() = new ComboBox()

  override _.setProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.setProps(terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> ComboBox

    // Properties
    props
    |> Props.tryFind PKey.comboBox.hideDropdownListOnClick
    |> Option.iter (fun v -> view.HideDropdownListOnClick <- v)

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
    let view = terminalElement.View :?> ComboBox

    // Properties
    props
    |> Props.tryFind PKey.comboBox.hideDropdownListOnClick
    |> Option.iter (fun _ ->
        view.HideDropdownListOnClick <- Unchecked.defaultof<_>)

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
    terminalElement.tryRemoveEventHandler PKey.comboBox.collapsed
    terminalElement.tryRemoveEventHandler PKey.comboBox.expanded
    terminalElement.tryRemoveEventHandler PKey.comboBox.openSelectedItem
    terminalElement.tryRemoveEventHandler PKey.comboBox.selectedItemChanged

type internal DatePickerTerminalElement(props: Props) =
  inherit ViewTerminalElement(props)

  override _.name = "DatePicker"

  override _.newView() = new DatePicker()

  override _.setProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.setProps(terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> DatePicker

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
    let view = terminalElement.View :?> DatePicker

    // Properties
    props
    |> Props.tryFind PKey.datePicker.culture
    |> Option.iter (fun _ ->
        view.Culture <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.datePicker.date
    |> Option.iter (fun _ ->
        view.Date <- Unchecked.defaultof<_>)


type internal FrameViewTerminalElement(props: Props) =
  inherit ViewTerminalElement(props)

  override _.name = "FrameView"

  override _.newView() = new FrameView()



type internal GraphViewTerminalElement(props: Props) =
  inherit ViewTerminalElement(props)

  override _.name = "GraphView"

  override _.newView() = new GraphView()

  override _.setProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.setProps(terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> GraphView

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
    |> Option.iter (fun v -> view.GraphColor <- v |> Option.toNullable)

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
    let view = terminalElement.View :?> GraphView

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

  override _.newView() = new HexView()

  override _.setProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.setProps(terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> HexView

    // Properties
    props
    |> Props.tryFind PKey.hexView.address
    |> Option.iter (fun v -> view.Address <- v)

    props
    |> Props.tryFind PKey.hexView.addressWidth
    |> Option.iter (fun v -> view.AddressWidth <- v)

    props
    |> Props.tryFind PKey.hexView.bytesPerLine
    |> Option.iter (fun v -> view.BytesPerLine <- v)

    props
    |> Props.tryFind PKey.hexView.readOnly
    |> Option.iter (fun v -> view.ReadOnly <- v)

    props
    |> Props.tryFind PKey.hexView.source
    |> Option.iter (fun v -> view.Source <- v)

    // Events
    terminalElement.trySetEventHandler(PKey.hexView.edited, view.Edited)

    terminalElement.trySetEventHandler(PKey.hexView.positionChanged, view.PositionChanged)


  override _.removeProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.removeProps(terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> HexView

    // Properties
    props
    |> Props.tryFind PKey.hexView.address
    |> Option.iter (fun _ ->
        view.Address <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.hexView.addressWidth
    |> Option.iter (fun _ ->
        view.AddressWidth <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.hexView.bytesPerLine
    |> Option.iter (fun _ ->
        view.BytesPerLine <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.hexView.readOnly
    |> Option.iter (fun _ ->
        view.ReadOnly <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.hexView.source
    |> Option.iter (fun _ ->
        view.Source <- Unchecked.defaultof<_>)

    // Events
    terminalElement.tryRemoveEventHandler PKey.hexView.edited
    terminalElement.tryRemoveEventHandler PKey.hexView.positionChanged

type internal LabelTerminalElement(props: Props) =
  inherit ViewTerminalElement(props)

  override _.name = "Label"

  override _.newView() = new Label()

  override _.setProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.setProps(terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> Label

    // Properties
    props
    |> Props.tryFind PKey.label.hotKeySpecifier
    |> Option.iter (fun v -> view.HotKeySpecifier <- v)

    props
    |> Props.tryFind PKey.label.text
    |> Option.iter (fun v -> view.Text <- v)

    // Events

  override _.removeProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.removeProps(terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> Label

    // Properties
    props
    |> Props.tryFind PKey.label.hotKeySpecifier
    |> Option.iter (fun _ ->
        view.HotKeySpecifier <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.label.text
    |> Option.iter (fun _ ->
        view.Text <- Unchecked.defaultof<_>)


type internal LegendAnnotationTerminalElement(props: Props) =
  inherit ViewTerminalElement(props)

  override _.name = "LegendAnnotation"

  override _.newView() = new LegendAnnotation()



type internal LineTerminalElement(props: Props) =
  inherit ViewTerminalElement(props)

  override _.name = "Line"

  override _.newView() = new Line()

  override _.setProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.setProps(terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> Line

    // Properties
    props
    |> Props.tryFind PKey.line.length
    |> Option.iter (fun v -> view.Length <- v)

    props
    |> Props.tryFind PKey.line.orientation
    |> Option.iter (fun v -> view.Orientation <- v)

    props
    |> Props.tryFind PKey.line.style
    |> Option.iter (fun v -> view.Style <- v)

    // Events
    terminalElement.trySetEventHandler(PKey.line.orientationChanged, view.OrientationChanged)

    terminalElement.trySetEventHandler(PKey.line.orientationChanging, view.OrientationChanging)


  override _.removeProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.removeProps(terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> Line

    // Properties
    props
    |> Props.tryFind PKey.line.length
    |> Option.iter (fun _ ->
        view.Length <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.line.orientation
    |> Option.iter (fun _ ->
        view.Orientation <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.line.style
    |> Option.iter (fun _ ->
        view.Style <- Unchecked.defaultof<_>)

    // Events
    terminalElement.tryRemoveEventHandler PKey.line.orientationChanged
    terminalElement.tryRemoveEventHandler PKey.line.orientationChanging

type internal ListViewTerminalElement(props: Props) =
  inherit ViewTerminalElement(props)

  override _.name = "ListView"

  override _.newView() = new ListView()

  override _.setProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.setProps(terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> ListView

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
    |> Option.iter (fun v -> view.SelectedItem <- v |> Option.toNullable)

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
    let view = terminalElement.View :?> ListView

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
    terminalElement.tryRemoveEventHandler PKey.listView.collectionChanged
    terminalElement.tryRemoveEventHandler PKey.listView.openSelectedItem
    terminalElement.tryRemoveEventHandler PKey.listView.rowRender
    terminalElement.tryRemoveEventHandler PKey.listView.selectedItemChanged

type internal MarginTerminalElement(props: Props) =
  inherit AdornmentTerminalElement(props)

  override _.name = "Margin"

  override _.newView() = new Margin()

  override _.setProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.setProps(terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> Margin

    // Properties
    props
    |> Props.tryFind PKey.margin.shadowStyle
    |> Option.iter (fun v -> view.ShadowStyle <- v)

    // Events

  override _.removeProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.removeProps(terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> Margin

    // Properties
    props
    |> Props.tryFind PKey.margin.shadowStyle
    |> Option.iter (fun _ ->
        view.ShadowStyle <- Unchecked.defaultof<_>)


type internal MenuTerminalElement(props: Props) =
  inherit BarTerminalElement(props)

  override _.name = "Menu"

  override _.newView() = new Menu()

  override _.setProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.setProps(terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> Menu

    // Properties
    props
    |> Props.tryFind PKey.menu.selectedMenuItem
    |> Option.iter (fun v -> view.SelectedMenuItem <- v)

    props
    |> Props.tryFind PKey.menu.superMenuItem
    |> Option.iter (fun v -> view.SuperMenuItem <- v)

    // Events
    terminalElement.trySetEventHandler(PKey.menu.selectedMenuItemChanged, view.SelectedMenuItemChanged)


  override _.removeProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.removeProps(terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> Menu

    // Properties
    props
    |> Props.tryFind PKey.menu.selectedMenuItem
    |> Option.iter (fun _ ->
        view.SelectedMenuItem <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.menu.superMenuItem
    |> Option.iter (fun _ ->
        view.SuperMenuItem <- Unchecked.defaultof<_>)

    // Events
    terminalElement.tryRemoveEventHandler PKey.menu.selectedMenuItemChanged

type internal MenuBarTerminalElement(props: Props) =
  inherit MenuTerminalElement(props)

  override _.name = "MenuBar"

  override _.newView() = new MenuBar()

  override _.setProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.setProps(terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> MenuBar

    // Properties
    props
    |> Props.tryFind PKey.menuBar.key
    |> Option.iter (fun v -> view.Key <- v)

    // Events
    terminalElement.trySetEventHandler(PKey.menuBar.keyChanged, view.KeyChanged)


  override _.removeProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.removeProps(terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> MenuBar

    // Properties
    props
    |> Props.tryFind PKey.menuBar.key
    |> Option.iter (fun _ ->
        view.Key <- Unchecked.defaultof<_>)

    // Events
    terminalElement.tryRemoveEventHandler PKey.menuBar.keyChanged

type internal NumericUpDownTerminalElement<'T>(props: Props) =
  inherit ViewTerminalElement(props)

  override _.name = "NumericUpDown`1"

  override _.newView() = new NumericUpDown<'T>()

  override _.setProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.setProps(terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> NumericUpDown<'T>

    // Properties
    props
    |> Props.tryFind PKey.numericUpDown<'T>.format
    |> Option.iter (fun v -> view.Format <- v)

    props
    |> Props.tryFind PKey.numericUpDown<'T>.increment
    |> Option.iter (fun v -> view.Increment <- v)

    props
    |> Props.tryFind PKey.numericUpDown<'T>.value
    |> Option.iter (fun v -> view.Value <- v)

    // Events
    terminalElement.trySetEventHandler(PKey.numericUpDown<'T>.formatChanged, view.FormatChanged)

    terminalElement.trySetEventHandler(PKey.numericUpDown<'T>.incrementChanged, view.IncrementChanged)

    terminalElement.trySetEventHandler(PKey.numericUpDown<'T>.valueChanged, view.ValueChanged)

    terminalElement.trySetEventHandler(PKey.numericUpDown<'T>.valueChanging, view.ValueChanging)


  override _.removeProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.removeProps(terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> NumericUpDown<'T>

    // Properties
    props
    |> Props.tryFind PKey.numericUpDown<'T>.format
    |> Option.iter (fun _ ->
        view.Format <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.numericUpDown<'T>.increment
    |> Option.iter (fun _ ->
        view.Increment <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.numericUpDown<'T>.value
    |> Option.iter (fun _ ->
        view.Value <- Unchecked.defaultof<_>)

    // Events
    terminalElement.tryRemoveEventHandler PKey.numericUpDown<'T>.formatChanged
    terminalElement.tryRemoveEventHandler PKey.numericUpDown<'T>.incrementChanged
    terminalElement.tryRemoveEventHandler PKey.numericUpDown<'T>.valueChanged
    terminalElement.tryRemoveEventHandler PKey.numericUpDown<'T>.valueChanging

type internal PaddingTerminalElement(props: Props) =
  inherit AdornmentTerminalElement(props)

  override _.name = "Padding"

  override _.newView() = new Padding()



[<AbstractClass>]
type internal PopoverBaseImplTerminalElement(props: Props) =
  inherit ViewTerminalElement(props)

  override _.name = "PopoverBaseImpl"

  override _.newView() = failwith "Cannot instantiate abstract view type PopoverBaseImpl"

  override _.setProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.setProps(terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> PopoverBaseImpl

    // Properties
    props
    |> Props.tryFind PKey.popoverBaseImpl.current
    |> Option.iter (fun v -> view.Current <- v)

    // Events

  override _.removeProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.removeProps(terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> PopoverBaseImpl

    // Properties
    props
    |> Props.tryFind PKey.popoverBaseImpl.current
    |> Option.iter (fun _ ->
        view.Current <- Unchecked.defaultof<_>)


type internal PopoverMenuTerminalElement(props: Props) =
  inherit PopoverBaseImplTerminalElement(props)

  override _.name = "PopoverMenu"

  override _.newView() = new PopoverMenu()

  override _.setProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.setProps(terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> PopoverMenu

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
    let view = terminalElement.View :?> PopoverMenu

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
    terminalElement.tryRemoveEventHandler PKey.popoverMenu.keyChanged

type internal ProgressBarTerminalElement(props: Props) =
  inherit ViewTerminalElement(props)

  override _.name = "ProgressBar"

  override _.newView() = new ProgressBar()

  override _.setProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.setProps(terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> ProgressBar

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
    let view = terminalElement.View :?> ProgressBar

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


type internal RunnableTerminalElement(props: Props) =
  inherit ViewTerminalElement(props)

  override _.name = "Runnable"

  override _.newView() = new Runnable()

  override _.setProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.setProps(terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> Runnable

    // Properties
    props
    |> Props.tryFind PKey.runnable.result
    |> Option.iter (fun v -> view.Result <- v)

    props
    |> Props.tryFind PKey.runnable.stopRequested
    |> Option.iter (fun v -> view.StopRequested <- v)

    // Events
    terminalElement.trySetEventHandler(PKey.runnable.isModalChanged, view.IsModalChanged)

    terminalElement.trySetEventHandler(PKey.runnable.isRunningChanged, view.IsRunningChanged)

    terminalElement.trySetEventHandler(PKey.runnable.isRunningChanging, view.IsRunningChanging)


  override _.removeProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.removeProps(terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> Runnable

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
    terminalElement.tryRemoveEventHandler PKey.runnable.isModalChanged
    terminalElement.tryRemoveEventHandler PKey.runnable.isRunningChanged
    terminalElement.tryRemoveEventHandler PKey.runnable.isRunningChanging

type internal RunnableTerminalElement<'TResult>(props: Props) =
  inherit RunnableTerminalElement(props)

  override _.name = "Runnable`1"

  override _.newView() = new Runnable<'TResult>()

  override _.setProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.setProps(terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> Runnable<'TResult>

    // Properties
    props
    |> Props.tryFind PKey.runnable'<'TResult>.result
    |> Option.iter (fun v -> view.Result <- v)

    // Events

  override _.removeProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.removeProps(terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> Runnable<'TResult>

    // Properties
    props
    |> Props.tryFind PKey.runnable'<'TResult>.result
    |> Option.iter (fun _ ->
        view.Result <- Unchecked.defaultof<_>)


type internal ScrollBarTerminalElement(props: Props) =
  inherit ViewTerminalElement(props)

  override _.name = "ScrollBar"

  override _.newView() = new ScrollBar()

  override _.setProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.setProps(terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> ScrollBar

    // Properties
    props
    |> Props.tryFind PKey.scrollBar.autoShow
    |> Option.iter (fun v -> view.AutoShow <- v)

    props
    |> Props.tryFind PKey.scrollBar.increment
    |> Option.iter (fun v -> view.Increment <- v)

    props
    |> Props.tryFind PKey.scrollBar.orientation
    |> Option.iter (fun v -> view.Orientation <- v)

    props
    |> Props.tryFind PKey.scrollBar.position
    |> Option.iter (fun v -> view.Position <- v)

    props
    |> Props.tryFind PKey.scrollBar.scrollableContentSize
    |> Option.iter (fun v -> view.ScrollableContentSize <- v)

    props
    |> Props.tryFind PKey.scrollBar.visibleContentSize
    |> Option.iter (fun v -> view.VisibleContentSize <- v)

    // Events
    terminalElement.trySetEventHandler(PKey.scrollBar.orientationChanged, view.OrientationChanged)

    terminalElement.trySetEventHandler(PKey.scrollBar.orientationChanging, view.OrientationChanging)

    terminalElement.trySetEventHandler(PKey.scrollBar.positionChanged, view.PositionChanged)

    terminalElement.trySetEventHandler(PKey.scrollBar.positionChanging, view.PositionChanging)

    terminalElement.trySetEventHandler(PKey.scrollBar.scrollableContentSizeChanged, view.ScrollableContentSizeChanged)

    terminalElement.trySetEventHandler(PKey.scrollBar.scrolled, view.Scrolled)

    terminalElement.trySetEventHandler(PKey.scrollBar.sliderPositionChanged, view.SliderPositionChanged)


  override _.removeProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.removeProps(terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> ScrollBar

    // Properties
    props
    |> Props.tryFind PKey.scrollBar.autoShow
    |> Option.iter (fun _ ->
        view.AutoShow <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.scrollBar.increment
    |> Option.iter (fun _ ->
        view.Increment <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.scrollBar.orientation
    |> Option.iter (fun _ ->
        view.Orientation <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.scrollBar.position
    |> Option.iter (fun _ ->
        view.Position <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.scrollBar.scrollableContentSize
    |> Option.iter (fun _ ->
        view.ScrollableContentSize <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.scrollBar.visibleContentSize
    |> Option.iter (fun _ ->
        view.VisibleContentSize <- Unchecked.defaultof<_>)

    // Events
    terminalElement.tryRemoveEventHandler PKey.scrollBar.orientationChanged
    terminalElement.tryRemoveEventHandler PKey.scrollBar.orientationChanging
    terminalElement.tryRemoveEventHandler PKey.scrollBar.positionChanged
    terminalElement.tryRemoveEventHandler PKey.scrollBar.positionChanging
    terminalElement.tryRemoveEventHandler PKey.scrollBar.scrollableContentSizeChanged
    terminalElement.tryRemoveEventHandler PKey.scrollBar.scrolled
    terminalElement.tryRemoveEventHandler PKey.scrollBar.sliderPositionChanged

type internal ScrollSliderTerminalElement(props: Props) =
  inherit ViewTerminalElement(props)

  override _.name = "ScrollSlider"

  override _.newView() = new ScrollSlider()

  override _.setProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.setProps(terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> ScrollSlider

    // Properties
    props
    |> Props.tryFind PKey.scrollSlider.orientation
    |> Option.iter (fun v -> view.Orientation <- v)

    props
    |> Props.tryFind PKey.scrollSlider.position
    |> Option.iter (fun v -> view.Position <- v)

    props
    |> Props.tryFind PKey.scrollSlider.size
    |> Option.iter (fun v -> view.Size <- v)

    props
    |> Props.tryFind PKey.scrollSlider.sliderPadding
    |> Option.iter (fun v -> view.SliderPadding <- v)

    props
    |> Props.tryFind PKey.scrollSlider.visibleContentSize
    |> Option.iter (fun v -> view.VisibleContentSize <- v)

    // Events
    terminalElement.trySetEventHandler(PKey.scrollSlider.orientationChanged, view.OrientationChanged)

    terminalElement.trySetEventHandler(PKey.scrollSlider.orientationChanging, view.OrientationChanging)

    terminalElement.trySetEventHandler(PKey.scrollSlider.positionChanged, view.PositionChanged)

    terminalElement.trySetEventHandler(PKey.scrollSlider.positionChanging, view.PositionChanging)

    terminalElement.trySetEventHandler(PKey.scrollSlider.scrolled, view.Scrolled)


  override _.removeProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.removeProps(terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> ScrollSlider

    // Properties
    props
    |> Props.tryFind PKey.scrollSlider.orientation
    |> Option.iter (fun _ ->
        view.Orientation <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.scrollSlider.position
    |> Option.iter (fun _ ->
        view.Position <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.scrollSlider.size
    |> Option.iter (fun _ ->
        view.Size <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.scrollSlider.sliderPadding
    |> Option.iter (fun _ ->
        view.SliderPadding <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.scrollSlider.visibleContentSize
    |> Option.iter (fun _ ->
        view.VisibleContentSize <- Unchecked.defaultof<_>)

    // Events
    terminalElement.tryRemoveEventHandler PKey.scrollSlider.orientationChanged
    terminalElement.tryRemoveEventHandler PKey.scrollSlider.orientationChanging
    terminalElement.tryRemoveEventHandler PKey.scrollSlider.positionChanged
    terminalElement.tryRemoveEventHandler PKey.scrollSlider.positionChanging
    terminalElement.tryRemoveEventHandler PKey.scrollSlider.scrolled

[<AbstractClass>]
type internal SelectorBaseTerminalElement(props: Props) =
  inherit ViewTerminalElement(props)

  override _.name = "SelectorBase"

  override _.newView() = failwith "Cannot instantiate abstract view type SelectorBase"

  override _.setProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.setProps(terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> SelectorBase

    // Properties
    props
    |> Props.tryFind PKey.selectorBase.assignHotKeys
    |> Option.iter (fun v -> view.AssignHotKeys <- v)

    props
    |> Props.tryFind PKey.selectorBase.doubleClickAccepts
    |> Option.iter (fun v -> view.DoubleClickAccepts <- v)

    props
    |> Props.tryFind PKey.selectorBase.horizontalSpace
    |> Option.iter (fun v -> view.HorizontalSpace <- v)

    props
    |> Props.tryFind PKey.selectorBase.labels
    |> Option.iter (fun v -> view.Labels <- v)

    props
    |> Props.tryFind PKey.selectorBase.orientation
    |> Option.iter (fun v -> view.Orientation <- v)

    props
    |> Props.tryFind PKey.selectorBase.styles
    |> Option.iter (fun v -> view.Styles <- v)

    props
    |> Props.tryFind PKey.selectorBase.usedHotKeys
    |> Option.iter (fun v -> view.UsedHotKeys <- v)

    props
    |> Props.tryFind PKey.selectorBase.value
    |> Option.iter (fun v -> view.Value <- v |> Option.toNullable)

    props
    |> Props.tryFind PKey.selectorBase.values
    |> Option.iter (fun v -> view.Values <- v)

    // Events
    terminalElement.trySetEventHandler(PKey.selectorBase.orientationChanged, view.OrientationChanged)

    terminalElement.trySetEventHandler(PKey.selectorBase.orientationChanging, view.OrientationChanging)

    terminalElement.trySetEventHandler(PKey.selectorBase.valueChanged, view.ValueChanged)


  override _.removeProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.removeProps(terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> SelectorBase

    // Properties
    props
    |> Props.tryFind PKey.selectorBase.assignHotKeys
    |> Option.iter (fun _ ->
        view.AssignHotKeys <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.selectorBase.doubleClickAccepts
    |> Option.iter (fun _ ->
        view.DoubleClickAccepts <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.selectorBase.horizontalSpace
    |> Option.iter (fun _ ->
        view.HorizontalSpace <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.selectorBase.labels
    |> Option.iter (fun _ ->
        view.Labels <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.selectorBase.orientation
    |> Option.iter (fun _ ->
        view.Orientation <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.selectorBase.styles
    |> Option.iter (fun _ ->
        view.Styles <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.selectorBase.usedHotKeys
    |> Option.iter (fun _ ->
        view.UsedHotKeys <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.selectorBase.value
    |> Option.iter (fun _ ->
        view.Value <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.selectorBase.values
    |> Option.iter (fun _ ->
        view.Values <- Unchecked.defaultof<_>)

    // Events
    terminalElement.tryRemoveEventHandler PKey.selectorBase.orientationChanged
    terminalElement.tryRemoveEventHandler PKey.selectorBase.orientationChanging
    terminalElement.tryRemoveEventHandler PKey.selectorBase.valueChanged

type internal FlagSelectorTerminalElement(props: Props) =
  inherit SelectorBaseTerminalElement(props)

  override _.name = "FlagSelector"

  override _.newView() = new FlagSelector()

  override _.setProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.setProps(terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> FlagSelector

    // Properties
    props
    |> Props.tryFind PKey.flagSelector.value
    |> Option.iter (fun v -> view.Value <- v |> Option.toNullable)

    // Events

  override _.removeProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.removeProps(terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> FlagSelector

    // Properties
    props
    |> Props.tryFind PKey.flagSelector.value
    |> Option.iter (fun _ ->
        view.Value <- Unchecked.defaultof<_>)


type internal OptionSelectorTerminalElement(props: Props) =
  inherit SelectorBaseTerminalElement(props)

  override _.name = "OptionSelector"

  override _.newView() = new OptionSelector()

  override _.setProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.setProps(terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> OptionSelector

    // Properties
    props
    |> Props.tryFind PKey.optionSelector.cursor
    |> Option.iter (fun v -> view.Cursor <- v)

    // Events

  override _.removeProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.removeProps(terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> OptionSelector

    // Properties
    props
    |> Props.tryFind PKey.optionSelector.cursor
    |> Option.iter (fun _ ->
        view.Cursor <- Unchecked.defaultof<_>)


type internal FlagSelectorTerminalElement<'TFlagsEnum>(props: Props) =
  inherit FlagSelectorTerminalElement(props)

  override _.name = "FlagSelector`1"

  override _.newView() = new FlagSelector<'TFlagsEnum>()

  override _.setProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.setProps(terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> FlagSelector<'TFlagsEnum>

    // Properties
    props
    |> Props.tryFind PKey.flagSelector'<'TFlagsEnum>.value
    |> Option.iter (fun v -> view.Value <- v |> Option.toNullable)

    // Events
    terminalElement.trySetEventHandler(PKey.flagSelector'<'TFlagsEnum>.valueChanged, view.ValueChanged)


  override _.removeProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.removeProps(terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> FlagSelector<'TFlagsEnum>

    // Properties
    props
    |> Props.tryFind PKey.flagSelector'<'TFlagsEnum>.value
    |> Option.iter (fun _ ->
        view.Value <- Unchecked.defaultof<_>)

    // Events
    terminalElement.tryRemoveEventHandler PKey.flagSelector'<'TFlagsEnum>.valueChanged

type internal OptionSelectorTerminalElement<'TEnum>(props: Props) =
  inherit OptionSelectorTerminalElement(props)

  override _.name = "OptionSelector`1"

  override _.newView() = new OptionSelector<'TEnum>()

  override _.setProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.setProps(terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> OptionSelector<'TEnum>

    // Properties
    props
    |> Props.tryFind PKey.optionSelector'<'TEnum>.value
    |> Option.iter (fun v -> view.Value <- v |> Option.toNullable)

    props
    |> Props.tryFind PKey.optionSelector'<'TEnum>.values
    |> Option.iter (fun v -> view.Values <- v)

    // Events
    terminalElement.trySetEventHandler(PKey.optionSelector'<'TEnum>.valueChanged, view.ValueChanged)


  override _.removeProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.removeProps(terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> OptionSelector<'TEnum>

    // Properties
    props
    |> Props.tryFind PKey.optionSelector'<'TEnum>.value
    |> Option.iter (fun _ ->
        view.Value <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.optionSelector'<'TEnum>.values
    |> Option.iter (fun _ ->
        view.Values <- Unchecked.defaultof<_>)

    // Events
    terminalElement.tryRemoveEventHandler PKey.optionSelector'<'TEnum>.valueChanged

type internal ShortcutTerminalElement(props: Props) =
  inherit ViewTerminalElement(props)

  override _.name = "Shortcut"

  override _.newView() = new Shortcut()

  override _.setProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.setProps(terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> Shortcut

    // Properties
    props
    |> Props.tryFind PKey.shortcut.action
    |> Option.iter (fun v -> view.Action <- v)

    props
    |> Props.tryFind PKey.shortcut.alignmentModes
    |> Option.iter (fun v -> view.AlignmentModes <- v)

    props
    |> Props.tryFind PKey.shortcut.bindKeyToApplication
    |> Option.iter (fun v -> view.BindKeyToApplication <- v)

    props
    |> Props.tryFind PKey.shortcut.commandView
    |> Option.iter (fun v -> view.CommandView <- v)

    props
    |> Props.tryFind PKey.shortcut.forceFocusColors
    |> Option.iter (fun v -> view.ForceFocusColors <- v)

    props
    |> Props.tryFind PKey.shortcut.helpText
    |> Option.iter (fun v -> view.HelpText <- v)

    props
    |> Props.tryFind PKey.shortcut.key
    |> Option.iter (fun v -> view.Key <- v)

    props
    |> Props.tryFind PKey.shortcut.minimumKeyTextSize
    |> Option.iter (fun v -> view.MinimumKeyTextSize <- v)

    props
    |> Props.tryFind PKey.shortcut.orientation
    |> Option.iter (fun v -> view.Orientation <- v)

    props
    |> Props.tryFind PKey.shortcut.text
    |> Option.iter (fun v -> view.Text <- v)

    // Events
    terminalElement.trySetEventHandler(PKey.shortcut.orientationChanged, view.OrientationChanged)

    terminalElement.trySetEventHandler(PKey.shortcut.orientationChanging, view.OrientationChanging)


  override _.removeProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.removeProps(terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> Shortcut

    // Properties
    props
    |> Props.tryFind PKey.shortcut.action
    |> Option.iter (fun _ ->
        view.Action <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.shortcut.alignmentModes
    |> Option.iter (fun _ ->
        view.AlignmentModes <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.shortcut.bindKeyToApplication
    |> Option.iter (fun _ ->
        view.BindKeyToApplication <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.shortcut.commandView
    |> Option.iter (fun _ ->
        view.CommandView <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.shortcut.forceFocusColors
    |> Option.iter (fun _ ->
        view.ForceFocusColors <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.shortcut.helpText
    |> Option.iter (fun _ ->
        view.HelpText <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.shortcut.key
    |> Option.iter (fun _ ->
        view.Key <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.shortcut.minimumKeyTextSize
    |> Option.iter (fun _ ->
        view.MinimumKeyTextSize <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.shortcut.orientation
    |> Option.iter (fun _ ->
        view.Orientation <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.shortcut.text
    |> Option.iter (fun _ ->
        view.Text <- Unchecked.defaultof<_>)

    // Events
    terminalElement.tryRemoveEventHandler PKey.shortcut.orientationChanged
    terminalElement.tryRemoveEventHandler PKey.shortcut.orientationChanging

type internal MenuItemTerminalElement(props: Props) =
  inherit ShortcutTerminalElement(props)

  override _.name = "MenuItem"

  override _.newView() = new MenuItem()

  override _.setProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.setProps(terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> MenuItem

    // Properties
    props
    |> Props.tryFind PKey.menuItem.command
    |> Option.iter (fun v -> view.Command <- v)

    props
    |> Props.tryFind PKey.menuItem.subMenu
    |> Option.iter (fun v -> view.SubMenu <- v)

    props
    |> Props.tryFind PKey.menuItem.targetView
    |> Option.iter (fun v -> view.TargetView <- v)

    // Events

  override _.removeProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.removeProps(terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> MenuItem

    // Properties
    props
    |> Props.tryFind PKey.menuItem.command
    |> Option.iter (fun _ ->
        view.Command <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.menuItem.subMenu
    |> Option.iter (fun _ ->
        view.SubMenu <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.menuItem.targetView
    |> Option.iter (fun _ ->
        view.TargetView <- Unchecked.defaultof<_>)


type internal MenuBarItemTerminalElement(props: Props) =
  inherit MenuItemTerminalElement(props)

  override _.name = "MenuBarItem"

  override _.newView() = new MenuBarItem()

  override _.setProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.setProps(terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> MenuBarItem

    // Properties
    props
    |> Props.tryFind PKey.menuBarItem.popoverMenu
    |> Option.iter (fun v -> view.PopoverMenu <- v)

    props
    |> Props.tryFind PKey.menuBarItem.popoverMenuOpen
    |> Option.iter (fun v -> view.PopoverMenuOpen <- v)

    props
    |> Props.tryFind PKey.menuBarItem.subMenu
    |> Option.iter (fun v -> view.SubMenu <- v)

    // Events
    terminalElement.trySetEventHandler(PKey.menuBarItem.popoverMenuOpenChanged, view.PopoverMenuOpenChanged)


  override _.removeProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.removeProps(terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> MenuBarItem

    // Properties
    props
    |> Props.tryFind PKey.menuBarItem.popoverMenu
    |> Option.iter (fun _ ->
        view.PopoverMenu <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.menuBarItem.popoverMenuOpen
    |> Option.iter (fun _ ->
        view.PopoverMenuOpen <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.menuBarItem.subMenu
    |> Option.iter (fun _ ->
        view.SubMenu <- Unchecked.defaultof<_>)

    // Events
    terminalElement.tryRemoveEventHandler PKey.menuBarItem.popoverMenuOpenChanged

type internal SliderTerminalElement<'T>(props: Props) =
  inherit ViewTerminalElement(props)

  override _.name = "Slider`1"

  override _.newView() = new Slider<'T>()

  override _.setProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.setProps(terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> Slider<'T>

    // Properties
    props
    |> Props.tryFind PKey.slider<'T>.allowEmpty
    |> Option.iter (fun v -> view.AllowEmpty <- v)

    props
    |> Props.tryFind PKey.slider<'T>.focusedOption
    |> Option.iter (fun v -> view.FocusedOption <- v)

    props
    |> Props.tryFind PKey.slider<'T>.legendsOrientation
    |> Option.iter (fun v -> view.LegendsOrientation <- v)

    props
    |> Props.tryFind PKey.slider<'T>.minimumInnerSpacing
    |> Option.iter (fun v -> view.MinimumInnerSpacing <- v)

    props
    |> Props.tryFind PKey.slider<'T>.options
    |> Option.iter (fun v -> view.Options <- v)

    props
    |> Props.tryFind PKey.slider<'T>.orientation
    |> Option.iter (fun v -> view.Orientation <- v)

    props
    |> Props.tryFind PKey.slider<'T>.rangeAllowSingle
    |> Option.iter (fun v -> view.RangeAllowSingle <- v)

    props
    |> Props.tryFind PKey.slider<'T>.showEndSpacing
    |> Option.iter (fun v -> view.ShowEndSpacing <- v)

    props
    |> Props.tryFind PKey.slider<'T>.showLegends
    |> Option.iter (fun v -> view.ShowLegends <- v)

    props
    |> Props.tryFind PKey.slider<'T>.style
    |> Option.iter (fun v -> view.Style <- v)

    props
    |> Props.tryFind PKey.slider<'T>.text
    |> Option.iter (fun v -> view.Text <- v)

    props
    |> Props.tryFind PKey.slider<'T>.``type``
    |> Option.iter (fun v -> view.Type <- v)

    props
    |> Props.tryFind PKey.slider<'T>.useMinimumSize
    |> Option.iter (fun v -> view.UseMinimumSize <- v)

    // Events
    terminalElement.trySetEventHandler(PKey.slider<'T>.optionFocused, view.OptionFocused)

    terminalElement.trySetEventHandler(PKey.slider<'T>.optionsChanged, view.OptionsChanged)

    terminalElement.trySetEventHandler(PKey.slider<'T>.orientationChanged, view.OrientationChanged)

    terminalElement.trySetEventHandler(PKey.slider<'T>.orientationChanging, view.OrientationChanging)


  override _.removeProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.removeProps(terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> Slider<'T>

    // Properties
    props
    |> Props.tryFind PKey.slider<'T>.allowEmpty
    |> Option.iter (fun _ ->
        view.AllowEmpty <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.slider<'T>.focusedOption
    |> Option.iter (fun _ ->
        view.FocusedOption <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.slider<'T>.legendsOrientation
    |> Option.iter (fun _ ->
        view.LegendsOrientation <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.slider<'T>.minimumInnerSpacing
    |> Option.iter (fun _ ->
        view.MinimumInnerSpacing <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.slider<'T>.options
    |> Option.iter (fun _ ->
        view.Options <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.slider<'T>.orientation
    |> Option.iter (fun _ ->
        view.Orientation <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.slider<'T>.rangeAllowSingle
    |> Option.iter (fun _ ->
        view.RangeAllowSingle <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.slider<'T>.showEndSpacing
    |> Option.iter (fun _ ->
        view.ShowEndSpacing <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.slider<'T>.showLegends
    |> Option.iter (fun _ ->
        view.ShowLegends <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.slider<'T>.style
    |> Option.iter (fun _ ->
        view.Style <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.slider<'T>.text
    |> Option.iter (fun _ ->
        view.Text <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.slider<'T>.``type``
    |> Option.iter (fun _ ->
        view.Type <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.slider<'T>.useMinimumSize
    |> Option.iter (fun _ ->
        view.UseMinimumSize <- Unchecked.defaultof<_>)

    // Events
    terminalElement.tryRemoveEventHandler PKey.slider<'T>.optionFocused
    terminalElement.tryRemoveEventHandler PKey.slider<'T>.optionsChanged
    terminalElement.tryRemoveEventHandler PKey.slider<'T>.orientationChanged
    terminalElement.tryRemoveEventHandler PKey.slider<'T>.orientationChanging

type internal SpinnerViewTerminalElement(props: Props) =
  inherit ViewTerminalElement(props)

  override _.name = "SpinnerView"

  override _.newView() = new SpinnerView()

  override _.setProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.setProps(terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> SpinnerView

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
    let view = terminalElement.View :?> SpinnerView

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

  override _.newView() = new StatusBar()



type internal TabTerminalElement(props: Props) =
  inherit ViewTerminalElement(props)

  override _.name = "Tab"

  override _.newView() = new Tab()

  override _.setProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.setProps(terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> Tab

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
    let view = terminalElement.View :?> Tab

    // Properties
    props
    |> Props.tryFind PKey.tab.displayText
    |> Option.iter (fun _ ->
        view.DisplayText <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.tab.view
    |> Option.iter (fun _ ->
        view.View <- Unchecked.defaultof<_>)


type internal TabViewTerminalElement(props: Props) =
  inherit ViewTerminalElement(props)

  override _.name = "TabView"

  override _.newView() = new TabView()

  override _.setProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.setProps(terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> TabView

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
    let view = terminalElement.View :?> TabView

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
    terminalElement.tryRemoveEventHandler PKey.tabView.selectedTabChanged
    terminalElement.tryRemoveEventHandler PKey.tabView.tabClicked

type internal TableViewTerminalElement(props: Props) =
  inherit ViewTerminalElement(props)

  override _.name = "TableView"

  override _.newView() = new TableView()

  override _.setProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.setProps(terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> TableView

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
    let view = terminalElement.View :?> TableView

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
    terminalElement.tryRemoveEventHandler PKey.tableView.cellActivated
    terminalElement.tryRemoveEventHandler PKey.tableView.cellToggled
    terminalElement.tryRemoveEventHandler PKey.tableView.selectedCellChanged

type internal TextFieldTerminalElement(props: Props) =
  inherit ViewTerminalElement(props)

  override _.name = "TextField"

  override _.newView() = new TextField()

  override _.setProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.setProps(terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> TextField

    // Properties
    props
    |> Props.tryFind PKey.textField.autocomplete
    |> Option.iter (fun v -> view.Autocomplete <- v)

    props
    |> Props.tryFind PKey.textField.cursorPosition
    |> Option.iter (fun v -> view.CursorPosition <- v)

    props
    |> Props.tryFind PKey.textField.readOnly
    |> Option.iter (fun v -> view.ReadOnly <- v)

    props
    |> Props.tryFind PKey.textField.secret
    |> Option.iter (fun v -> view.Secret <- v)

    props
    |> Props.tryFind PKey.textField.selectWordOnlyOnDoubleClick
    |> Option.iter (fun v -> view.SelectWordOnlyOnDoubleClick <- v)

    props
    |> Props.tryFind PKey.textField.selectedStart
    |> Option.iter (fun v -> view.SelectedStart <- v)

    props
    |> Props.tryFind PKey.textField.text
    |> Option.iter (fun v -> view.Text <- v)

    props
    |> Props.tryFind PKey.textField.useSameRuneTypeForWords
    |> Option.iter (fun v -> view.UseSameRuneTypeForWords <- v)

    props
    |> Props.tryFind PKey.textField.used
    |> Option.iter (fun v -> view.Used <- v)

    // Events
    terminalElement.trySetEventHandler(PKey.textField.textChanging, view.TextChanging)


  override _.removeProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.removeProps(terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> TextField

    // Properties
    props
    |> Props.tryFind PKey.textField.autocomplete
    |> Option.iter (fun _ ->
        view.Autocomplete <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.textField.cursorPosition
    |> Option.iter (fun _ ->
        view.CursorPosition <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.textField.readOnly
    |> Option.iter (fun _ ->
        view.ReadOnly <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.textField.secret
    |> Option.iter (fun _ ->
        view.Secret <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.textField.selectWordOnlyOnDoubleClick
    |> Option.iter (fun _ ->
        view.SelectWordOnlyOnDoubleClick <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.textField.selectedStart
    |> Option.iter (fun _ ->
        view.SelectedStart <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.textField.text
    |> Option.iter (fun _ ->
        view.Text <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.textField.useSameRuneTypeForWords
    |> Option.iter (fun _ ->
        view.UseSameRuneTypeForWords <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.textField.used
    |> Option.iter (fun _ ->
        view.Used <- Unchecked.defaultof<_>)

    // Events
    terminalElement.tryRemoveEventHandler PKey.textField.textChanging

type internal DateFieldTerminalElement(props: Props) =
  inherit TextFieldTerminalElement(props)

  override _.name = "DateField"

  override _.newView() = new DateField()

  override _.setProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.setProps(terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> DateField

    // Properties
    props
    |> Props.tryFind PKey.dateField.culture
    |> Option.iter (fun v -> view.Culture <- v)

    props
    |> Props.tryFind PKey.dateField.cursorPosition
    |> Option.iter (fun v -> view.CursorPosition <- v)

    props
    |> Props.tryFind PKey.dateField.date
    |> Option.iter (fun v -> view.Date <- v |> Option.toNullable)

    // Events
    terminalElement.trySetEventHandler(PKey.dateField.dateChanged, view.DateChanged)


  override _.removeProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.removeProps(terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> DateField

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
    terminalElement.tryRemoveEventHandler PKey.dateField.dateChanged

type internal TextValidateFieldTerminalElement(props: Props) =
  inherit ViewTerminalElement(props)

  override _.name = "TextValidateField"

  override _.newView() = new TextValidateField()

  override _.setProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.setProps(terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> TextValidateField

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
    let view = terminalElement.View :?> TextValidateField

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

  override _.newView() = new TextView()

  override _.setProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.setProps(terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> TextView

    // Properties
    props
    |> Props.tryFind PKey.textView.allowsReturn
    |> Option.iter (fun v -> view.AllowsReturn <- v)

    props
    |> Props.tryFind PKey.textView.allowsTab
    |> Option.iter (fun v -> view.AllowsTab <- v)

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
    |> Props.tryFind PKey.textView.isSelecting
    |> Option.iter (fun v -> view.IsSelecting <- v)

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
    |> Props.tryFind PKey.textView.selectWordOnlyOnDoubleClick
    |> Option.iter (fun v -> view.SelectWordOnlyOnDoubleClick <- v)

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
    |> Props.tryFind PKey.textView.useSameRuneTypeForWords
    |> Option.iter (fun v -> view.UseSameRuneTypeForWords <- v)

    props
    |> Props.tryFind PKey.textView.used
    |> Option.iter (fun v -> view.Used <- v)

    props
    |> Props.tryFind PKey.textView.wordWrap
    |> Option.iter (fun v -> view.WordWrap <- v)

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
    let view = terminalElement.View :?> TextView

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
    |> Props.tryFind PKey.textView.isSelecting
    |> Option.iter (fun _ ->
        view.IsSelecting <- Unchecked.defaultof<_>)

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
    |> Props.tryFind PKey.textView.selectWordOnlyOnDoubleClick
    |> Option.iter (fun _ ->
        view.SelectWordOnlyOnDoubleClick <- Unchecked.defaultof<_>)

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
    |> Props.tryFind PKey.textView.useSameRuneTypeForWords
    |> Option.iter (fun _ ->
        view.UseSameRuneTypeForWords <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.textView.used
    |> Option.iter (fun _ ->
        view.Used <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.textView.wordWrap
    |> Option.iter (fun _ ->
        view.WordWrap <- Unchecked.defaultof<_>)

    // Events
    terminalElement.tryRemoveEventHandler PKey.textView.contentsChanged
    terminalElement.tryRemoveEventHandler PKey.textView.drawNormalColor
    terminalElement.tryRemoveEventHandler PKey.textView.drawReadOnlyColor
    terminalElement.tryRemoveEventHandler PKey.textView.drawSelectionColor
    terminalElement.tryRemoveEventHandler PKey.textView.drawUsedColor
    terminalElement.tryRemoveEventHandler PKey.textView.unwrappedCursorPosition

type internal TimeFieldTerminalElement(props: Props) =
  inherit TextFieldTerminalElement(props)

  override _.name = "TimeField"

  override _.newView() = new TimeField()

  override _.setProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.setProps(terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> TimeField

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
    let view = terminalElement.View :?> TimeField

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
    terminalElement.tryRemoveEventHandler PKey.timeField.timeChanged

type internal TreeViewTerminalElement<'T>(props: Props) =
  inherit ViewTerminalElement(props)

  override _.name = "TreeView`1"

  override _.newView() = new TreeView<'T>()

  override _.setProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.setProps(terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> TreeView<'T>

    // Properties
    props
    |> Props.tryFind PKey.treeView<'T>.allowLetterBasedNavigation
    |> Option.iter (fun v -> view.AllowLetterBasedNavigation <- v)

    props
    |> Props.tryFind PKey.treeView<'T>.aspectGetter
    |> Option.iter (fun v -> view.AspectGetter <- v)

    props
    |> Props.tryFind PKey.treeView<'T>.colorGetter
    |> Option.iter (fun v -> view.ColorGetter <- v)

    props
    |> Props.tryFind PKey.treeView<'T>.maxDepth
    |> Option.iter (fun v -> view.MaxDepth <- v)

    props
    |> Props.tryFind PKey.treeView<'T>.multiSelect
    |> Option.iter (fun v -> view.MultiSelect <- v)

    props
    |> Props.tryFind PKey.treeView<'T>.objectActivationButton
    |> Option.iter (fun v -> view.ObjectActivationButton <- v |> Option.toNullable)

    props
    |> Props.tryFind PKey.treeView<'T>.objectActivationKey
    |> Option.iter (fun v -> view.ObjectActivationKey <- v)

    props
    |> Props.tryFind PKey.treeView<'T>.scrollOffsetHorizontal
    |> Option.iter (fun v -> view.ScrollOffsetHorizontal <- v)

    props
    |> Props.tryFind PKey.treeView<'T>.scrollOffsetVertical
    |> Option.iter (fun v -> view.ScrollOffsetVertical <- v)

    props
    |> Props.tryFind PKey.treeView<'T>.selectedObject
    |> Option.iter (fun v -> view.SelectedObject <- v)

    props
    |> Props.tryFind PKey.treeView<'T>.style
    |> Option.iter (fun v -> view.Style <- v)

    props
    |> Props.tryFind PKey.treeView<'T>.treeBuilder
    |> Option.iter (fun v -> view.TreeBuilder <- v)

    // Events
    terminalElement.trySetEventHandler(PKey.treeView<'T>.drawLine, view.DrawLine)

    terminalElement.trySetEventHandler(PKey.treeView<'T>.objectActivated, view.ObjectActivated)

    terminalElement.trySetEventHandler(PKey.treeView<'T>.selectionChanged, view.SelectionChanged)


  override _.removeProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.removeProps(terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> TreeView<'T>

    // Properties
    props
    |> Props.tryFind PKey.treeView<'T>.allowLetterBasedNavigation
    |> Option.iter (fun _ ->
        view.AllowLetterBasedNavigation <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.treeView<'T>.aspectGetter
    |> Option.iter (fun _ ->
        view.AspectGetter <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.treeView<'T>.colorGetter
    |> Option.iter (fun _ ->
        view.ColorGetter <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.treeView<'T>.maxDepth
    |> Option.iter (fun _ ->
        view.MaxDepth <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.treeView<'T>.multiSelect
    |> Option.iter (fun _ ->
        view.MultiSelect <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.treeView<'T>.objectActivationButton
    |> Option.iter (fun _ ->
        view.ObjectActivationButton <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.treeView<'T>.objectActivationKey
    |> Option.iter (fun _ ->
        view.ObjectActivationKey <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.treeView<'T>.scrollOffsetHorizontal
    |> Option.iter (fun _ ->
        view.ScrollOffsetHorizontal <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.treeView<'T>.scrollOffsetVertical
    |> Option.iter (fun _ ->
        view.ScrollOffsetVertical <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.treeView<'T>.selectedObject
    |> Option.iter (fun _ ->
        view.SelectedObject <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.treeView<'T>.style
    |> Option.iter (fun _ ->
        view.Style <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.treeView<'T>.treeBuilder
    |> Option.iter (fun _ ->
        view.TreeBuilder <- Unchecked.defaultof<_>)

    // Events
    terminalElement.tryRemoveEventHandler PKey.treeView<'T>.drawLine
    terminalElement.tryRemoveEventHandler PKey.treeView<'T>.objectActivated
    terminalElement.tryRemoveEventHandler PKey.treeView<'T>.selectionChanged

type internal WindowTerminalElement(props: Props) =
  inherit RunnableTerminalElement(props)

  override _.name = "Window"

  override _.newView() = new Window()



type internal DialogTerminalElement(props: Props) =
  inherit WindowTerminalElement(props)

  override _.name = "Dialog"

  override _.newView() = new Dialog()

  override _.setProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.setProps(terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> Dialog

    // Properties
    props
    |> Props.tryFind PKey.dialog.buttonAlignment
    |> Option.iter (fun v -> view.ButtonAlignment <- v)

    props
    |> Props.tryFind PKey.dialog.buttonAlignmentModes
    |> Option.iter (fun v -> view.ButtonAlignmentModes <- v)

    props
    |> Props.tryFind PKey.dialog.canceled
    |> Option.iter (fun v -> view.Canceled <- v)

    // Events

  override _.removeProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.removeProps(terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> Dialog

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
    |> Props.tryFind PKey.dialog.canceled
    |> Option.iter (fun _ ->
        view.Canceled <- Unchecked.defaultof<_>)


type internal FileDialogTerminalElement(props: Props) =
  inherit DialogTerminalElement(props)

  override _.name = "FileDialog"

  override _.newView() = new FileDialog()

  override _.setProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.setProps(terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> FileDialog

    // Properties
    props
    |> Props.tryFind PKey.fileDialog.allowedTypes
    |> Option.iter (fun v -> view.AllowedTypes <- v)

    props
    |> Props.tryFind PKey.fileDialog.allowsMultipleSelection
    |> Option.iter (fun v -> view.AllowsMultipleSelection <- v)

    props
    |> Props.tryFind PKey.fileDialog.fileOperationsHandler
    |> Option.iter (fun v -> view.FileOperationsHandler <- v)

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
    let view = terminalElement.View :?> FileDialog

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
    |> Props.tryFind PKey.fileDialog.fileOperationsHandler
    |> Option.iter (fun _ ->
        view.FileOperationsHandler <- Unchecked.defaultof<_>)

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
    terminalElement.tryRemoveEventHandler PKey.fileDialog.filesSelected

type internal OpenDialogTerminalElement(props: Props) =
  inherit FileDialogTerminalElement(props)

  override _.name = "OpenDialog"

  override _.newView() = new OpenDialog()

  override _.setProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.setProps(terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> OpenDialog

    // Properties
    props
    |> Props.tryFind PKey.openDialog.openMode
    |> Option.iter (fun v -> view.OpenMode <- v)

    // Events

  override _.removeProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.removeProps(terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> OpenDialog

    // Properties
    props
    |> Props.tryFind PKey.openDialog.openMode
    |> Option.iter (fun _ ->
        view.OpenMode <- Unchecked.defaultof<_>)


type internal SaveDialogTerminalElement(props: Props) =
  inherit FileDialogTerminalElement(props)

  override _.name = "SaveDialog"

  override _.newView() = new SaveDialog()



type internal WizardTerminalElement(props: Props) =
  inherit DialogTerminalElement(props)

  override _.name = "Wizard"

  override _.newView() = new Wizard()

  override _.setProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.setProps(terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> Wizard

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
    let view = terminalElement.View :?> Wizard

    // Properties
    props
    |> Props.tryFind PKey.wizard.currentStep
    |> Option.iter (fun _ ->
        view.CurrentStep <- Unchecked.defaultof<_>)

    // Events
    terminalElement.tryRemoveEventHandler PKey.wizard.cancelled
    terminalElement.tryRemoveEventHandler PKey.wizard.finished
    terminalElement.tryRemoveEventHandler PKey.wizard.movingBack
    terminalElement.tryRemoveEventHandler PKey.wizard.movingNext
    terminalElement.tryRemoveEventHandler PKey.wizard.stepChanged
    terminalElement.tryRemoveEventHandler PKey.wizard.stepChanging

type internal WizardStepTerminalElement(props: Props) =
  inherit ViewTerminalElement(props)

  override _.name = "WizardStep"

  override _.newView() = new WizardStep()

  override _.setProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.setProps(terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> WizardStep

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
    let view = terminalElement.View :?> WizardStep

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

