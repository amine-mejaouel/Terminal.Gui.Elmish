namespace Terminal.Gui.Elmish

open System
open Terminal.Gui.App
open Terminal.Gui.ViewBase
open Terminal.Gui.Views


type internal ViewTerminalElement(props: Props) =
  inherit TerminalElement(props)

  override _.Name = "View"

  override _.NewView() = new View()

  override _.SetAsChildOfParentView = true

  override _.SetProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.SetProps(terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View

    // Properties
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
    |> Props.tryFind PKey.View.ContentSizeTracksViewport
    |> Option.iter (fun v -> view.ContentSizeTracksViewport <- v)

    props
    |> Props.tryFind PKey.View.Cursor
    |> Option.iter (fun v -> view.Cursor <- v)

    props
    |> Props.tryFind PKey.View.Data
    |> Option.iter (fun v -> view.Data <- v)

    props
    |> Props.tryFind PKey.View.Enabled
    |> Option.iter (fun v -> view.Enabled <- v)

    props
    |> Props.tryFind PKey.View.Frame
    |> Option.iter (fun v -> view.Frame <- v)

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

    props
    |> Props.tryFind PKey.View.Id
    |> Option.iter (fun v -> view.Id <- v)

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

    props
    |> Props.tryFind PKey.View.Text
    |> Option.iter (fun v -> view.Text <- v)

    props
    |> Props.tryFind PKey.View.TextAlignment
    |> Option.iter (fun v -> view.TextAlignment <- v)

    props
    |> Props.tryFind PKey.View.TextDirection
    |> Option.iter (fun v -> view.TextDirection <- v)

    props
    |> Props.tryFind PKey.View.Title
    |> Option.iter (fun v -> view.Title <- v)

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

    props
    |> Props.tryFind PKey.View.Width
    |> Option.iter (fun v -> view.Width <- v)

    props
    |> Props.tryFind PKey.View.X
    |> Option.iter (fun v -> view.X <- v)

    props
    |> Props.tryFind PKey.View.Y
    |> Option.iter (fun v -> view.Y <- v)

    // Events
    terminalElement.TrySetEventHandler(PKey.View.Accepted, view.Accepted)

    terminalElement.TrySetEventHandler(PKey.View.Accepting, view.Accepting)

    terminalElement.TrySetEventHandler(PKey.View.Activating, view.Activating)

    terminalElement.TrySetEventHandler(PKey.View.AdvancingFocus, view.AdvancingFocus)

    terminalElement.TrySetEventHandler(PKey.View.BorderStyleChanged, view.BorderStyleChanged)

    terminalElement.TrySetEventHandler(PKey.View.CanFocusChanged, view.CanFocusChanged)

    terminalElement.TrySetEventHandler(PKey.View.ClearedViewport, view.ClearedViewport)

    terminalElement.TrySetEventHandler(PKey.View.ClearingViewport, view.ClearingViewport)

    terminalElement.TrySetEventHandler(PKey.View.CommandNotBound, view.CommandNotBound)

    terminalElement.TrySetEventHandler(PKey.View.ContentSizeChanged, view.ContentSizeChanged)

    terminalElement.TrySetEventHandler(PKey.View.ContentSizeChanging, view.ContentSizeChanging)

    terminalElement.TrySetEventHandler(PKey.View.Disposing, view.Disposing)

    terminalElement.TrySetEventHandler(PKey.View.DrawComplete, view.DrawComplete)

    terminalElement.TrySetEventHandler(PKey.View.DrawingContent, view.DrawingContent)

    terminalElement.TrySetEventHandler(PKey.View.DrawingSubViews, view.DrawingSubViews)

    terminalElement.TrySetEventHandler(PKey.View.DrawingText, view.DrawingText)

    terminalElement.TrySetEventHandler(PKey.View.DrewText, view.DrewText)

    terminalElement.TrySetEventHandler(PKey.View.EnabledChanged, view.EnabledChanged)

    terminalElement.TrySetEventHandler(PKey.View.FocusedChanged, view.FocusedChanged)

    terminalElement.TrySetEventHandler(PKey.View.FrameChanged, view.FrameChanged)

    terminalElement.TrySetEventHandler(PKey.View.GettingAttributeForRole, view.GettingAttributeForRole)

    terminalElement.TrySetEventHandler(PKey.View.GettingScheme, view.GettingScheme)

    terminalElement.TrySetEventHandler(PKey.View.HandlingHotKey, view.HandlingHotKey)

    terminalElement.TrySetEventHandler(PKey.View.HasFocusChanged, view.HasFocusChanged)

    terminalElement.TrySetEventHandler(PKey.View.HasFocusChanging, view.HasFocusChanging)

    terminalElement.TrySetEventHandler(PKey.View.HeightChanged, view.HeightChanged)

    terminalElement.TrySetEventHandler(PKey.View.HeightChanging, view.HeightChanging)

    terminalElement.TrySetEventHandler(PKey.View.HotKeyChanged, view.HotKeyChanged)

    terminalElement.TrySetEventHandler(PKey.View.Initialized, view.Initialized)

    terminalElement.TrySetEventHandler(PKey.View.KeyDown, view.KeyDown)

    terminalElement.TrySetEventHandler(PKey.View.KeyDownNotHandled, view.KeyDownNotHandled)

    terminalElement.TrySetEventHandler(PKey.View.MouseEnter, view.MouseEnter)

    terminalElement.TrySetEventHandler(PKey.View.MouseEvent, view.MouseEvent)

    terminalElement.TrySetEventHandler(PKey.View.MouseHoldRepeatChanged, view.MouseHoldRepeatChanged)

    terminalElement.TrySetEventHandler(PKey.View.MouseHoldRepeatChanging, view.MouseHoldRepeatChanging)

    terminalElement.TrySetEventHandler(PKey.View.MouseLeave, view.MouseLeave)

    terminalElement.TrySetEventHandler(PKey.View.MouseStateChanged, view.MouseStateChanged)

    terminalElement.TrySetEventHandler(PKey.View.Removed, view.Removed)

    terminalElement.TrySetEventHandler(PKey.View.SchemeChanged, view.SchemeChanged)

    terminalElement.TrySetEventHandler(PKey.View.SchemeChanging, view.SchemeChanging)

    terminalElement.TrySetEventHandler(PKey.View.SchemeNameChanged, view.SchemeNameChanged)

    terminalElement.TrySetEventHandler(PKey.View.SchemeNameChanging, view.SchemeNameChanging)

    terminalElement.TrySetEventHandler(PKey.View.SubViewAdded, view.SubViewAdded)

    terminalElement.TrySetEventHandler(PKey.View.SubViewLayout, view.SubViewLayout)

    terminalElement.TrySetEventHandler(PKey.View.SubViewRemoved, view.SubViewRemoved)

    terminalElement.TrySetEventHandler(PKey.View.SubViewsLaidOut, view.SubViewsLaidOut)

    terminalElement.TrySetEventHandler(PKey.View.SuperViewChanged, view.SuperViewChanged)

    terminalElement.TrySetEventHandler(PKey.View.SuperViewChanging, view.SuperViewChanging)

    terminalElement.TrySetEventHandler(PKey.View.TextChanged, view.TextChanged)

    terminalElement.TrySetEventHandler(PKey.View.TitleChanged, view.TitleChanged)

    terminalElement.TrySetEventHandler(PKey.View.TitleChanging, view.TitleChanging)

    terminalElement.TrySetEventHandler(PKey.View.ViewportChanged, view.ViewportChanged)

    terminalElement.TrySetEventHandler(PKey.View.VisibleChanged, view.VisibleChanged)

    terminalElement.TrySetEventHandler(PKey.View.VisibleChanging, view.VisibleChanging)

    terminalElement.TrySetEventHandler(PKey.View.WidthChanged, view.WidthChanged)

    terminalElement.TrySetEventHandler(PKey.View.WidthChanging, view.WidthChanging)

  override _.RemoveProps(terminalElement: IInternalTerminalElement, props: Props) =

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View

    // Properties
    props
    |> Props.tryFind PKey.View.Arrangement
    |> Option.iter (fun _ ->
        view.Arrangement <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.View.AssignHotKeys
    |> Option.iter (fun _ ->
        view.AssignHotKeys <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.View.BorderStyle
    |> Option.iter (fun _ ->
        view.BorderStyle <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.View.CanFocus
    |> Option.iter (fun _ ->
        view.CanFocus <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.View.ContentSizeTracksViewport
    |> Option.iter (fun _ ->
        view.ContentSizeTracksViewport <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.View.Cursor
    |> Option.iter (fun _ ->
        view.Cursor <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.View.Data
    |> Option.iter (fun _ ->
        view.Data <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.View.Enabled
    |> Option.iter (fun _ ->
        view.Enabled <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.View.Frame
    |> Option.iter (fun _ ->
        view.Frame <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.View.HasFocus
    |> Option.iter (fun _ ->
        view.HasFocus <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.View.Height
    |> Option.iter (fun _ ->
        view.Height <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.View.HotKey
    |> Option.iter (fun _ ->
        view.HotKey <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.View.HotKeySpecifier
    |> Option.iter (fun _ ->
        view.HotKeySpecifier <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.View.Id
    |> Option.iter (fun _ ->
        view.Id <- "")

    props
    |> Props.tryFind PKey.View.IsInitialized
    |> Option.iter (fun _ ->
        view.IsInitialized <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.View.MouseHighlightStates
    |> Option.iter (fun _ ->
        view.MouseHighlightStates <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.View.MouseHoldRepeat
    |> Option.iter (fun _ ->
        view.MouseHoldRepeat <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.View.MousePositionTracking
    |> Option.iter (fun _ ->
        view.MousePositionTracking <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.View.PreserveTrailingSpaces
    |> Option.iter (fun _ ->
        view.PreserveTrailingSpaces <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.View.SchemeName
    |> Option.iter (fun _ ->
        view.SchemeName <- "")

    props
    |> Props.tryFind PKey.View.ShadowStyle
    |> Option.iter (fun _ ->
        view.ShadowStyle <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.View.SuperViewRendersLineCanvas
    |> Option.iter (fun _ ->
        view.SuperViewRendersLineCanvas <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.View.TabStop
    |> Option.iter (fun _ ->
        view.TabStop <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.View.Text
    |> Option.iter (fun _ ->
        view.Text <- "")

    props
    |> Props.tryFind PKey.View.TextAlignment
    |> Option.iter (fun _ ->
        view.TextAlignment <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.View.TextDirection
    |> Option.iter (fun _ ->
        view.TextDirection <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.View.Title
    |> Option.iter (fun _ ->
        view.Title <- "")

    props
    |> Props.tryFind PKey.View.UsedHotKeys
    |> Option.iter (fun _ ->
        view.UsedHotKeys <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.View.ValidatePosDim
    |> Option.iter (fun _ ->
        view.ValidatePosDim <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.View.VerticalTextAlignment
    |> Option.iter (fun _ ->
        view.VerticalTextAlignment <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.View.Viewport
    |> Option.iter (fun _ ->
        view.Viewport <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.View.ViewportSettings
    |> Option.iter (fun _ ->
        view.ViewportSettings <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.View.Visible
    |> Option.iter (fun _ ->
        view.Visible <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.View.Width
    |> Option.iter (fun _ ->
        view.Width <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.View.X
    |> Option.iter (fun _ ->
        view.X <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.View.Y
    |> Option.iter (fun _ ->
        view.Y <- Unchecked.defaultof<_>)

    // Events
    terminalElement.TryRemoveEventHandler PKey.View.Accepted
    terminalElement.TryRemoveEventHandler PKey.View.Accepting
    terminalElement.TryRemoveEventHandler PKey.View.Activating
    terminalElement.TryRemoveEventHandler PKey.View.AdvancingFocus
    terminalElement.TryRemoveEventHandler PKey.View.BorderStyleChanged
    terminalElement.TryRemoveEventHandler PKey.View.CanFocusChanged
    terminalElement.TryRemoveEventHandler PKey.View.ClearedViewport
    terminalElement.TryRemoveEventHandler PKey.View.ClearingViewport
    terminalElement.TryRemoveEventHandler PKey.View.CommandNotBound
    terminalElement.TryRemoveEventHandler PKey.View.ContentSizeChanged
    terminalElement.TryRemoveEventHandler PKey.View.ContentSizeChanging
    terminalElement.TryRemoveEventHandler PKey.View.Disposing
    terminalElement.TryRemoveEventHandler PKey.View.DrawComplete
    terminalElement.TryRemoveEventHandler PKey.View.DrawingContent
    terminalElement.TryRemoveEventHandler PKey.View.DrawingSubViews
    terminalElement.TryRemoveEventHandler PKey.View.DrawingText
    terminalElement.TryRemoveEventHandler PKey.View.DrewText
    terminalElement.TryRemoveEventHandler PKey.View.EnabledChanged
    terminalElement.TryRemoveEventHandler PKey.View.FocusedChanged
    terminalElement.TryRemoveEventHandler PKey.View.FrameChanged
    terminalElement.TryRemoveEventHandler PKey.View.GettingAttributeForRole
    terminalElement.TryRemoveEventHandler PKey.View.GettingScheme
    terminalElement.TryRemoveEventHandler PKey.View.HandlingHotKey
    terminalElement.TryRemoveEventHandler PKey.View.HasFocusChanged
    terminalElement.TryRemoveEventHandler PKey.View.HasFocusChanging
    terminalElement.TryRemoveEventHandler PKey.View.HeightChanged
    terminalElement.TryRemoveEventHandler PKey.View.HeightChanging
    terminalElement.TryRemoveEventHandler PKey.View.HotKeyChanged
    terminalElement.TryRemoveEventHandler PKey.View.Initialized
    terminalElement.TryRemoveEventHandler PKey.View.KeyDown
    terminalElement.TryRemoveEventHandler PKey.View.KeyDownNotHandled
    terminalElement.TryRemoveEventHandler PKey.View.MouseEnter
    terminalElement.TryRemoveEventHandler PKey.View.MouseEvent
    terminalElement.TryRemoveEventHandler PKey.View.MouseHoldRepeatChanged
    terminalElement.TryRemoveEventHandler PKey.View.MouseHoldRepeatChanging
    terminalElement.TryRemoveEventHandler PKey.View.MouseLeave
    terminalElement.TryRemoveEventHandler PKey.View.MouseStateChanged
    terminalElement.TryRemoveEventHandler PKey.View.Removed
    terminalElement.TryRemoveEventHandler PKey.View.SchemeChanged
    terminalElement.TryRemoveEventHandler PKey.View.SchemeChanging
    terminalElement.TryRemoveEventHandler PKey.View.SchemeNameChanged
    terminalElement.TryRemoveEventHandler PKey.View.SchemeNameChanging
    terminalElement.TryRemoveEventHandler PKey.View.SubViewAdded
    terminalElement.TryRemoveEventHandler PKey.View.SubViewLayout
    terminalElement.TryRemoveEventHandler PKey.View.SubViewRemoved
    terminalElement.TryRemoveEventHandler PKey.View.SubViewsLaidOut
    terminalElement.TryRemoveEventHandler PKey.View.SuperViewChanged
    terminalElement.TryRemoveEventHandler PKey.View.SuperViewChanging
    terminalElement.TryRemoveEventHandler PKey.View.TextChanged
    terminalElement.TryRemoveEventHandler PKey.View.TitleChanged
    terminalElement.TryRemoveEventHandler PKey.View.TitleChanging
    terminalElement.TryRemoveEventHandler PKey.View.ViewportChanged
    terminalElement.TryRemoveEventHandler PKey.View.VisibleChanged
    terminalElement.TryRemoveEventHandler PKey.View.VisibleChanging
    terminalElement.TryRemoveEventHandler PKey.View.WidthChanged
    terminalElement.TryRemoveEventHandler PKey.View.WidthChanging

  interface ITerminalElement

type internal AdornmentTerminalElement(props: Props) =
  inherit ViewTerminalElement(props)

  override _.Name = "Adornment"

  override _.NewView() = new Adornment()

  override _.SetAsChildOfParentView = true

  override this.SubElements_PropKeys =
    [
      SubElementPropKey.from PKey.Adornment.Parent_element
    ]
    |> List.append base.SubElements_PropKeys

  override _.SetProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.SetProps(terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> Adornment

    // Properties
    props
    |> Props.tryFind PKey.Adornment.Diagnostics
    |> Option.iter (fun v -> view.Diagnostics <- v)

    props
    |> Props.tryFind PKey.Adornment.Parent
    |> Option.iter (fun v -> view.Parent <- v)

    props
    |> Props.tryFind PKey.Adornment.SuperViewRendersLineCanvas
    |> Option.iter (fun v -> view.SuperViewRendersLineCanvas <- v)

    props
    |> Props.tryFind PKey.Adornment.Thickness
    |> Option.iter (fun v -> view.Thickness <- v)

    props
    |> Props.tryFind PKey.Adornment.Viewport
    |> Option.iter (fun v -> view.Viewport <- v)

    // Events
    terminalElement.TrySetEventHandler(PKey.Adornment.ThicknessChanged, view.ThicknessChanged)

  override _.RemoveProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.RemoveProps(terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> Adornment

    // Properties
    props
    |> Props.tryFind PKey.Adornment.Diagnostics
    |> Option.iter (fun _ ->
        view.Diagnostics <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.Adornment.Parent
    |> Option.iter (fun _ ->
        view.Parent <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.Adornment.SuperViewRendersLineCanvas
    |> Option.iter (fun _ ->
        view.SuperViewRendersLineCanvas <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.Adornment.Thickness
    |> Option.iter (fun _ ->
        view.Thickness <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.Adornment.Viewport
    |> Option.iter (fun _ ->
        view.Viewport <- Unchecked.defaultof<_>)

    // Events
    terminalElement.TryRemoveEventHandler PKey.Adornment.ThicknessChanged

  interface ITerminalElement

type internal AttributePickerTerminalElement(props: Props) =
  inherit ViewTerminalElement(props)

  override _.Name = "AttributePicker"

  override _.NewView() = new AttributePicker()

  override _.SetAsChildOfParentView = true

  override _.SetProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.SetProps(terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> AttributePicker

    // Properties
    props
    |> Props.tryFind PKey.AttributePicker.SampleText
    |> Option.iter (fun v -> view.SampleText <- v)

    props
    |> Props.tryFind PKey.AttributePicker.Value
    |> Option.iter (fun v -> view.Value <- v)

    // Events
    terminalElement.TrySetEventHandler(PKey.AttributePicker.ValueChanged, view.ValueChanged)

    terminalElement.TrySetEventHandler(PKey.AttributePicker.ValueChanging, view.ValueChanging)

  override _.RemoveProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.RemoveProps(terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> AttributePicker

    // Properties
    props
    |> Props.tryFind PKey.AttributePicker.SampleText
    |> Option.iter (fun _ ->
        view.SampleText <- "")

    props
    |> Props.tryFind PKey.AttributePicker.Value
    |> Option.iter (fun _ ->
        view.Value <- Unchecked.defaultof<_>)

    // Events
    terminalElement.TryRemoveEventHandler PKey.AttributePicker.ValueChanged
    terminalElement.TryRemoveEventHandler PKey.AttributePicker.ValueChanging

  interface ITerminalElement

type internal BarTerminalElement(props: Props) =
  inherit ViewTerminalElement(props)

  override _.Name = "Bar"

  override _.NewView() = new Bar()

  override _.SetAsChildOfParentView = true

  override _.SetProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.SetProps(terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> Bar

    // Properties
    props
    |> Props.tryFind PKey.Bar.AlignmentModes
    |> Option.iter (fun v -> view.AlignmentModes <- v)

    props
    |> Props.tryFind PKey.Bar.Orientation
    |> Option.iter (fun v -> view.Orientation <- v)

    // Events
    terminalElement.TrySetEventHandler(PKey.Bar.OrientationChanged, view.OrientationChanged)

    terminalElement.TrySetEventHandler(PKey.Bar.OrientationChanging, view.OrientationChanging)

  override _.RemoveProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.RemoveProps(terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> Bar

    // Properties
    props
    |> Props.tryFind PKey.Bar.AlignmentModes
    |> Option.iter (fun _ ->
        view.AlignmentModes <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.Bar.Orientation
    |> Option.iter (fun _ ->
        view.Orientation <- Unchecked.defaultof<_>)

    // Events
    terminalElement.TryRemoveEventHandler PKey.Bar.OrientationChanged
    terminalElement.TryRemoveEventHandler PKey.Bar.OrientationChanging

  interface ITerminalElement

type internal BorderTerminalElement(props: Props) =
  inherit AdornmentTerminalElement(props)

  override _.Name = "Border"

  override _.NewView() = new Border()

  override _.SetAsChildOfParentView = true

  override _.SetProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.SetProps(terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> Border

    // Properties
    props
    |> Props.tryFind PKey.Border.LineStyle
    |> Option.iter (fun v -> view.LineStyle <- v)

    props
    |> Props.tryFind PKey.Border.Settings
    |> Option.iter (fun v -> view.Settings <- v)

  override _.RemoveProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.RemoveProps(terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> Border

    // Properties
    props
    |> Props.tryFind PKey.Border.LineStyle
    |> Option.iter (fun _ ->
        view.LineStyle <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.Border.Settings
    |> Option.iter (fun _ ->
        view.Settings <- Unchecked.defaultof<_>)


  interface ITerminalElement

type internal ButtonTerminalElement(props: Props) =
  inherit ViewTerminalElement(props)

  override _.Name = "Button"

  override _.NewView() = new Button()

  override _.SetAsChildOfParentView = true

  override _.SetProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.SetProps(terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
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

    props
    |> Props.tryFind PKey.Button.Text
    |> Option.iter (fun v -> view.Text <- v)

  override _.RemoveProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.RemoveProps(terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> Button

    // Properties
    props
    |> Props.tryFind PKey.Button.HotKeySpecifier
    |> Option.iter (fun _ ->
        view.HotKeySpecifier <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.Button.IsDefault
    |> Option.iter (fun _ ->
        view.IsDefault <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.Button.NoDecorations
    |> Option.iter (fun _ ->
        view.NoDecorations <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.Button.NoPadding
    |> Option.iter (fun _ ->
        view.NoPadding <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.Button.Text
    |> Option.iter (fun _ ->
        view.Text <- "")


  interface ITerminalElement

type internal CharMapTerminalElement(props: Props) =
  inherit ViewTerminalElement(props)

  override _.Name = "CharMap"

  override _.NewView() = new CharMap()

  override _.SetAsChildOfParentView = true

  override _.SetProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.SetProps(terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
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
    terminalElement.TrySetEventHandler(PKey.CharMap.ValueChanged, view.ValueChanged)

    terminalElement.TrySetEventHandler(PKey.CharMap.ValueChanging, view.ValueChanging)

  override _.RemoveProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.RemoveProps(terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> CharMap

    // Properties
    props
    |> Props.tryFind PKey.CharMap.SelectedCodePoint
    |> Option.iter (fun _ ->
        view.SelectedCodePoint <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.CharMap.ShowGlyphWidths
    |> Option.iter (fun _ ->
        view.ShowGlyphWidths <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.CharMap.ShowUnicodeCategory
    |> Option.iter (fun _ ->
        view.ShowUnicodeCategory <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.CharMap.StartCodePoint
    |> Option.iter (fun _ ->
        view.StartCodePoint <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.CharMap.Value
    |> Option.iter (fun _ ->
        view.Value <- Unchecked.defaultof<_>)

    // Events
    terminalElement.TryRemoveEventHandler PKey.CharMap.ValueChanged
    terminalElement.TryRemoveEventHandler PKey.CharMap.ValueChanging

  interface ITerminalElement

type internal CheckBoxTerminalElement(props: Props) =
  inherit ViewTerminalElement(props)

  override _.Name = "CheckBox"

  override _.NewView() = new CheckBox()

  override _.SetAsChildOfParentView = true

  override _.SetProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.SetProps(terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
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
    terminalElement.TrySetEventHandler(PKey.CheckBox.ValueChanged, view.ValueChanged)

    terminalElement.TrySetEventHandler(PKey.CheckBox.ValueChanging, view.ValueChanging)

  override _.RemoveProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.RemoveProps(terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> CheckBox

    // Properties
    props
    |> Props.tryFind PKey.CheckBox.AllowCheckStateNone
    |> Option.iter (fun _ ->
        view.AllowCheckStateNone <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.CheckBox.HotKeySpecifier
    |> Option.iter (fun _ ->
        view.HotKeySpecifier <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.CheckBox.RadioStyle
    |> Option.iter (fun _ ->
        view.RadioStyle <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.CheckBox.Text
    |> Option.iter (fun _ ->
        view.Text <- "")

    props
    |> Props.tryFind PKey.CheckBox.Value
    |> Option.iter (fun _ ->
        view.Value <- Unchecked.defaultof<_>)

    // Events
    terminalElement.TryRemoveEventHandler PKey.CheckBox.ValueChanged
    terminalElement.TryRemoveEventHandler PKey.CheckBox.ValueChanging

  interface ITerminalElement

type internal ColorPickerTerminalElement(props: Props) =
  inherit ViewTerminalElement(props)

  override _.Name = "ColorPicker"

  override _.NewView() = new ColorPicker()

  override _.SetAsChildOfParentView = true

  override _.SetProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.SetProps(terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
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
    terminalElement.TrySetEventHandler(PKey.ColorPicker.ValueChanged, view.ValueChanged)

    terminalElement.TrySetEventHandler(PKey.ColorPicker.ValueChanging, view.ValueChanging)

  override _.RemoveProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.RemoveProps(terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> ColorPicker

    // Properties
    props
    |> Props.tryFind PKey.ColorPicker.SelectedColor
    |> Option.iter (fun _ ->
        view.SelectedColor <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.ColorPicker.Style
    |> Option.iter (fun _ ->
        view.Style <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.ColorPicker.Text
    |> Option.iter (fun _ ->
        view.Text <- "")

    props
    |> Props.tryFind PKey.ColorPicker.Value
    |> Option.iter (fun _ ->
        view.Value <- Unchecked.defaultof<_>)

    // Events
    terminalElement.TryRemoveEventHandler PKey.ColorPicker.ValueChanged
    terminalElement.TryRemoveEventHandler PKey.ColorPicker.ValueChanging

  interface ITerminalElement

type internal ColorPicker16TerminalElement(props: Props) =
  inherit ViewTerminalElement(props)

  override _.Name = "ColorPicker16"

  override _.NewView() = new ColorPicker16()

  override _.SetAsChildOfParentView = true

  override _.SetProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.SetProps(terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
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
    terminalElement.TrySetEventHandler(PKey.ColorPicker16.ValueChanged, view.ValueChanged)

    terminalElement.TrySetEventHandler(PKey.ColorPicker16.ValueChanging, view.ValueChanging)

  override _.RemoveProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.RemoveProps(terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> ColorPicker16

    // Properties
    props
    |> Props.tryFind PKey.ColorPicker16.BoxHeight
    |> Option.iter (fun _ ->
        view.BoxHeight <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.ColorPicker16.BoxWidth
    |> Option.iter (fun _ ->
        view.BoxWidth <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.ColorPicker16.Caret
    |> Option.iter (fun _ ->
        view.Caret <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.ColorPicker16.SelectedColor
    |> Option.iter (fun _ ->
        view.SelectedColor <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.ColorPicker16.Value
    |> Option.iter (fun _ ->
        view.Value <- Unchecked.defaultof<_>)

    // Events
    terminalElement.TryRemoveEventHandler PKey.ColorPicker16.ValueChanged
    terminalElement.TryRemoveEventHandler PKey.ColorPicker16.ValueChanging

  interface ITerminalElement

type internal ComboBoxTerminalElement(props: Props) =
  inherit ViewTerminalElement(props)

  override _.Name = "ComboBox"

  override _.NewView() = new ComboBox()

  override _.SetAsChildOfParentView = true

  override _.SetProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.SetProps(terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> ComboBox

    // Properties
    props
    |> Props.tryFind PKey.ComboBox.HideDropdownListOnClick
    |> Option.iter (fun v -> view.HideDropdownListOnClick <- v)

    props
    |> Props.tryFind PKey.ComboBox.ReadOnly
    |> Option.iter (fun v -> view.ReadOnly <- v)

    props
    |> Props.tryFind PKey.ComboBox.SearchText
    |> Option.iter (fun v -> view.SearchText <- v)

    props
    |> Props.tryFind PKey.ComboBox.SelectedItem
    |> Option.iter (fun v -> view.SelectedItem <- v)

    props
    |> Props.tryFind PKey.ComboBox.Source
    |> Option.iter (fun v -> view.Source <- v)

    props
    |> Props.tryFind PKey.ComboBox.Text
    |> Option.iter (fun v -> view.Text <- v)

    // Events
    terminalElement.TrySetEventHandler(PKey.ComboBox.Collapsed, view.Collapsed)

    terminalElement.TrySetEventHandler(PKey.ComboBox.Expanded, view.Expanded)

    terminalElement.TrySetEventHandler(PKey.ComboBox.OpenSelectedItem, view.OpenSelectedItem)

    terminalElement.TrySetEventHandler(PKey.ComboBox.SelectedItemChanged, view.SelectedItemChanged)

  override _.RemoveProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.RemoveProps(terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> ComboBox

    // Properties
    props
    |> Props.tryFind PKey.ComboBox.HideDropdownListOnClick
    |> Option.iter (fun _ ->
        view.HideDropdownListOnClick <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.ComboBox.ReadOnly
    |> Option.iter (fun _ ->
        view.ReadOnly <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.ComboBox.SearchText
    |> Option.iter (fun _ ->
        view.SearchText <- "")

    props
    |> Props.tryFind PKey.ComboBox.SelectedItem
    |> Option.iter (fun _ ->
        view.SelectedItem <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.ComboBox.Source
    |> Option.iter (fun _ ->
        view.Source <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.ComboBox.Text
    |> Option.iter (fun _ ->
        view.Text <- "")

    // Events
    terminalElement.TryRemoveEventHandler PKey.ComboBox.Collapsed
    terminalElement.TryRemoveEventHandler PKey.ComboBox.Expanded
    terminalElement.TryRemoveEventHandler PKey.ComboBox.OpenSelectedItem
    terminalElement.TryRemoveEventHandler PKey.ComboBox.SelectedItemChanged

  interface ITerminalElement

type internal DatePickerTerminalElement(props: Props) =
  inherit ViewTerminalElement(props)

  override _.Name = "DatePicker"

  override _.NewView() = new DatePicker()

  override _.SetAsChildOfParentView = true

  override _.SetProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.SetProps(terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
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
    terminalElement.TrySetEventHandler(PKey.DatePicker.ValueChanged, view.ValueChanged)

    terminalElement.TrySetEventHandler(PKey.DatePicker.ValueChanging, view.ValueChanging)

  override _.RemoveProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.RemoveProps(terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> DatePicker

    // Properties
    props
    |> Props.tryFind PKey.DatePicker.Culture
    |> Option.iter (fun _ ->
        view.Culture <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.DatePicker.Text
    |> Option.iter (fun _ ->
        view.Text <- "")

    props
    |> Props.tryFind PKey.DatePicker.Value
    |> Option.iter (fun _ ->
        view.Value <- Unchecked.defaultof<_>)

    // Events
    terminalElement.TryRemoveEventHandler PKey.DatePicker.ValueChanged
    terminalElement.TryRemoveEventHandler PKey.DatePicker.ValueChanging

  interface ITerminalElement

type internal FrameViewTerminalElement(props: Props) =
  inherit ViewTerminalElement(props)

  override _.Name = "FrameView"

  override _.NewView() = new FrameView()

  override _.SetAsChildOfParentView = true


  interface ITerminalElement

type internal GraphViewTerminalElement(props: Props) =
  inherit ViewTerminalElement(props)

  override _.Name = "GraphView"

  override _.NewView() = new GraphView()

  override _.SetAsChildOfParentView = true

  override _.SetProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.SetProps(terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
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

  override _.RemoveProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.RemoveProps(terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> GraphView

    // Properties
    props
    |> Props.tryFind PKey.GraphView.AxisX
    |> Option.iter (fun _ ->
        view.AxisX <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.GraphView.AxisY
    |> Option.iter (fun _ ->
        view.AxisY <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.GraphView.CellSize
    |> Option.iter (fun _ ->
        view.CellSize <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.GraphView.GraphColor
    |> Option.iter (fun _ ->
        view.GraphColor <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.GraphView.MarginBottom
    |> Option.iter (fun _ ->
        view.MarginBottom <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.GraphView.MarginLeft
    |> Option.iter (fun _ ->
        view.MarginLeft <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.GraphView.ScrollOffset
    |> Option.iter (fun _ ->
        view.ScrollOffset <- Unchecked.defaultof<_>)


  interface ITerminalElement

type internal HexViewTerminalElement(props: Props) =
  inherit ViewTerminalElement(props)

  override _.Name = "HexView"

  override _.NewView() = new HexView()

  override _.SetAsChildOfParentView = true

  override _.SetProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.SetProps(terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
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
    terminalElement.TrySetEventHandler(PKey.HexView.Edited, view.Edited)

    terminalElement.TrySetEventHandler(PKey.HexView.PositionChanged, view.PositionChanged)

  override _.RemoveProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.RemoveProps(terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> HexView

    // Properties
    props
    |> Props.tryFind PKey.HexView.Address
    |> Option.iter (fun _ ->
        view.Address <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.HexView.AddressWidth
    |> Option.iter (fun _ ->
        view.AddressWidth <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.HexView.BytesPerLine
    |> Option.iter (fun _ ->
        view.BytesPerLine <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.HexView.ReadOnly
    |> Option.iter (fun _ ->
        view.ReadOnly <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.HexView.Source
    |> Option.iter (fun _ ->
        view.Source <- Unchecked.defaultof<_>)

    // Events
    terminalElement.TryRemoveEventHandler PKey.HexView.Edited
    terminalElement.TryRemoveEventHandler PKey.HexView.PositionChanged

  interface ITerminalElement

type internal LabelTerminalElement(props: Props) =
  inherit ViewTerminalElement(props)

  override _.Name = "Label"

  override _.NewView() = new Label()

  override _.SetAsChildOfParentView = true

  override _.SetProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.SetProps(terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> Label

    // Properties
    props
    |> Props.tryFind PKey.Label.HotKeySpecifier
    |> Option.iter (fun v -> view.HotKeySpecifier <- v)

    props
    |> Props.tryFind PKey.Label.Text
    |> Option.iter (fun v -> view.Text <- v)

  override _.RemoveProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.RemoveProps(terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> Label

    // Properties
    props
    |> Props.tryFind PKey.Label.HotKeySpecifier
    |> Option.iter (fun _ ->
        view.HotKeySpecifier <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.Label.Text
    |> Option.iter (fun _ ->
        view.Text <- "")


  interface ITerminalElement

type internal LegendAnnotationTerminalElement(props: Props) =
  inherit ViewTerminalElement(props)

  override _.Name = "LegendAnnotation"

  override _.NewView() = new LegendAnnotation()

  override _.SetAsChildOfParentView = true


  interface ITerminalElement

type internal LineTerminalElement(props: Props) =
  inherit ViewTerminalElement(props)

  override _.Name = "Line"

  override _.NewView() = new Line()

  override _.SetAsChildOfParentView = true

  override _.SetProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.SetProps(terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> Line

    // Properties
    props
    |> Props.tryFind PKey.Line.Length
    |> Option.iter (fun v -> view.Length <- v)

    props
    |> Props.tryFind PKey.Line.Orientation
    |> Option.iter (fun v -> view.Orientation <- v)

    props
    |> Props.tryFind PKey.Line.Style
    |> Option.iter (fun v -> view.Style <- v)

    // Events
    terminalElement.TrySetEventHandler(PKey.Line.OrientationChanged, view.OrientationChanged)

    terminalElement.TrySetEventHandler(PKey.Line.OrientationChanging, view.OrientationChanging)

  override _.RemoveProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.RemoveProps(terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> Line

    // Properties
    props
    |> Props.tryFind PKey.Line.Length
    |> Option.iter (fun _ ->
        view.Length <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.Line.Orientation
    |> Option.iter (fun _ ->
        view.Orientation <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.Line.Style
    |> Option.iter (fun _ ->
        view.Style <- Unchecked.defaultof<_>)

    // Events
    terminalElement.TryRemoveEventHandler PKey.Line.OrientationChanged
    terminalElement.TryRemoveEventHandler PKey.Line.OrientationChanging

  interface ITerminalElement

type internal LinearRangeTerminalElement<'T>(props: Props) =
  inherit ViewTerminalElement(props)

  override _.Name = "LinearRange`1"

  override _.NewView() = new LinearRange<'T>()

  override _.SetAsChildOfParentView = true

  override _.SetProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.SetProps(terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> LinearRange<'T>

    // Properties
    props
    |> Props.tryFind PKey.LinearRange<'T>.AllowEmpty
    |> Option.iter (fun v -> view.AllowEmpty <- v)

    props
    |> Props.tryFind PKey.LinearRange<'T>.FocusedOption
    |> Option.iter (fun v -> view.FocusedOption <- v)

    props
    |> Props.tryFind PKey.LinearRange<'T>.LegendsOrientation
    |> Option.iter (fun v -> view.LegendsOrientation <- v)

    props
    |> Props.tryFind PKey.LinearRange<'T>.MinimumInnerSpacing
    |> Option.iter (fun v -> view.MinimumInnerSpacing <- v)

    props
    |> Props.tryFind PKey.LinearRange<'T>.Options
    |> Option.iter (fun v -> view.Options <- v)

    props
    |> Props.tryFind PKey.LinearRange<'T>.Orientation
    |> Option.iter (fun v -> view.Orientation <- v)

    props
    |> Props.tryFind PKey.LinearRange<'T>.RangeAllowSingle
    |> Option.iter (fun v -> view.RangeAllowSingle <- v)

    props
    |> Props.tryFind PKey.LinearRange<'T>.ShowEndSpacing
    |> Option.iter (fun v -> view.ShowEndSpacing <- v)

    props
    |> Props.tryFind PKey.LinearRange<'T>.ShowLegends
    |> Option.iter (fun v -> view.ShowLegends <- v)

    props
    |> Props.tryFind PKey.LinearRange<'T>.Style
    |> Option.iter (fun v -> view.Style <- v)

    props
    |> Props.tryFind PKey.LinearRange<'T>.Text
    |> Option.iter (fun v -> view.Text <- v)

    props
    |> Props.tryFind PKey.LinearRange<'T>.Type
    |> Option.iter (fun v -> view.Type <- v)

    props
    |> Props.tryFind PKey.LinearRange<'T>.UseMinimumSize
    |> Option.iter (fun v -> view.UseMinimumSize <- v)

    // Events
    terminalElement.TrySetEventHandler(PKey.LinearRange<'T>.LegendsOrientationChanged, view.LegendsOrientationChanged)

    terminalElement.TrySetEventHandler(PKey.LinearRange<'T>.LegendsOrientationChanging, view.LegendsOrientationChanging)

    terminalElement.TrySetEventHandler(PKey.LinearRange<'T>.MinimumInnerSpacingChanged, view.MinimumInnerSpacingChanged)

    terminalElement.TrySetEventHandler(PKey.LinearRange<'T>.MinimumInnerSpacingChanging, view.MinimumInnerSpacingChanging)

    terminalElement.TrySetEventHandler(PKey.LinearRange<'T>.OptionFocused, view.OptionFocused)

    terminalElement.TrySetEventHandler(PKey.LinearRange<'T>.OptionsChanged, view.OptionsChanged)

    terminalElement.TrySetEventHandler(PKey.LinearRange<'T>.OrientationChanged, view.OrientationChanged)

    terminalElement.TrySetEventHandler(PKey.LinearRange<'T>.OrientationChanging, view.OrientationChanging)

    terminalElement.TrySetEventHandler(PKey.LinearRange<'T>.ShowEndSpacingChanged, view.ShowEndSpacingChanged)

    terminalElement.TrySetEventHandler(PKey.LinearRange<'T>.ShowEndSpacingChanging, view.ShowEndSpacingChanging)

    terminalElement.TrySetEventHandler(PKey.LinearRange<'T>.ShowLegendsChanged, view.ShowLegendsChanged)

    terminalElement.TrySetEventHandler(PKey.LinearRange<'T>.ShowLegendsChanging, view.ShowLegendsChanging)

    terminalElement.TrySetEventHandler(PKey.LinearRange<'T>.TypeChanged, view.TypeChanged)

    terminalElement.TrySetEventHandler(PKey.LinearRange<'T>.TypeChanging, view.TypeChanging)

    terminalElement.TrySetEventHandler(PKey.LinearRange<'T>.UseMinimumSizeChanged, view.UseMinimumSizeChanged)

    terminalElement.TrySetEventHandler(PKey.LinearRange<'T>.UseMinimumSizeChanging, view.UseMinimumSizeChanging)

  override _.RemoveProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.RemoveProps(terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> LinearRange<'T>

    // Properties
    props
    |> Props.tryFind PKey.LinearRange<'T>.AllowEmpty
    |> Option.iter (fun _ ->
        view.AllowEmpty <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.LinearRange<'T>.FocusedOption
    |> Option.iter (fun _ ->
        view.FocusedOption <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.LinearRange<'T>.LegendsOrientation
    |> Option.iter (fun _ ->
        view.LegendsOrientation <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.LinearRange<'T>.MinimumInnerSpacing
    |> Option.iter (fun _ ->
        view.MinimumInnerSpacing <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.LinearRange<'T>.Options
    |> Option.iter (fun _ ->
        view.Options <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.LinearRange<'T>.Orientation
    |> Option.iter (fun _ ->
        view.Orientation <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.LinearRange<'T>.RangeAllowSingle
    |> Option.iter (fun _ ->
        view.RangeAllowSingle <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.LinearRange<'T>.ShowEndSpacing
    |> Option.iter (fun _ ->
        view.ShowEndSpacing <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.LinearRange<'T>.ShowLegends
    |> Option.iter (fun _ ->
        view.ShowLegends <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.LinearRange<'T>.Style
    |> Option.iter (fun _ ->
        view.Style <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.LinearRange<'T>.Text
    |> Option.iter (fun _ ->
        view.Text <- "")

    props
    |> Props.tryFind PKey.LinearRange<'T>.Type
    |> Option.iter (fun _ ->
        view.Type <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.LinearRange<'T>.UseMinimumSize
    |> Option.iter (fun _ ->
        view.UseMinimumSize <- Unchecked.defaultof<_>)

    // Events
    terminalElement.TryRemoveEventHandler PKey.LinearRange<'T>.LegendsOrientationChanged
    terminalElement.TryRemoveEventHandler PKey.LinearRange<'T>.LegendsOrientationChanging
    terminalElement.TryRemoveEventHandler PKey.LinearRange<'T>.MinimumInnerSpacingChanged
    terminalElement.TryRemoveEventHandler PKey.LinearRange<'T>.MinimumInnerSpacingChanging
    terminalElement.TryRemoveEventHandler PKey.LinearRange<'T>.OptionFocused
    terminalElement.TryRemoveEventHandler PKey.LinearRange<'T>.OptionsChanged
    terminalElement.TryRemoveEventHandler PKey.LinearRange<'T>.OrientationChanged
    terminalElement.TryRemoveEventHandler PKey.LinearRange<'T>.OrientationChanging
    terminalElement.TryRemoveEventHandler PKey.LinearRange<'T>.ShowEndSpacingChanged
    terminalElement.TryRemoveEventHandler PKey.LinearRange<'T>.ShowEndSpacingChanging
    terminalElement.TryRemoveEventHandler PKey.LinearRange<'T>.ShowLegendsChanged
    terminalElement.TryRemoveEventHandler PKey.LinearRange<'T>.ShowLegendsChanging
    terminalElement.TryRemoveEventHandler PKey.LinearRange<'T>.TypeChanged
    terminalElement.TryRemoveEventHandler PKey.LinearRange<'T>.TypeChanging
    terminalElement.TryRemoveEventHandler PKey.LinearRange<'T>.UseMinimumSizeChanged
    terminalElement.TryRemoveEventHandler PKey.LinearRange<'T>.UseMinimumSizeChanging

  interface ITerminalElement

type internal LinearRangeTerminalElement(props: Props) =
  inherit LinearRangeTerminalElement<System.Object>(props)

  override _.Name = "LinearRange"

  override _.NewView() = new LinearRange()

  override _.SetAsChildOfParentView = true


  interface ITerminalElement

type internal ListViewTerminalElement(props: Props) =
  inherit ViewTerminalElement(props)

  override _.Name = "ListView"

  override _.NewView() = new ListView()

  override _.SetAsChildOfParentView = true

  override _.SetProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.SetProps(terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> ListView

    // Properties
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
    |> Props.tryFind PKey.ListView.TopItem
    |> Option.iter (fun v -> view.TopItem <- v)

    props
    |> Props.tryFind PKey.ListView.Value
    |> Option.iter (fun v -> view.Value <- v)

    // Events
    terminalElement.TrySetEventHandler(PKey.ListView.CollectionChanged, view.CollectionChanged)

    terminalElement.TrySetEventHandler(PKey.ListView.RowRender, view.RowRender)

    terminalElement.TrySetEventHandler(PKey.ListView.SourceChanged, view.SourceChanged)

    terminalElement.TrySetEventHandler(PKey.ListView.ValueChanged, view.ValueChanged)

    terminalElement.TrySetEventHandler(PKey.ListView.ValueChanging, view.ValueChanging)

  override _.RemoveProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.RemoveProps(terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> ListView

    // Properties
    props
    |> Props.tryFind PKey.ListView.MarkMultiple
    |> Option.iter (fun _ ->
        view.MarkMultiple <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.ListView.SelectedItem
    |> Option.iter (fun _ ->
        view.SelectedItem <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.ListView.ShowMarks
    |> Option.iter (fun _ ->
        view.ShowMarks <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.ListView.Source
    |> Option.iter (fun _ ->
        view.Source <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.ListView.TopItem
    |> Option.iter (fun _ ->
        view.TopItem <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.ListView.Value
    |> Option.iter (fun _ ->
        view.Value <- Unchecked.defaultof<_>)

    // Events
    terminalElement.TryRemoveEventHandler PKey.ListView.CollectionChanged
    terminalElement.TryRemoveEventHandler PKey.ListView.RowRender
    terminalElement.TryRemoveEventHandler PKey.ListView.SourceChanged
    terminalElement.TryRemoveEventHandler PKey.ListView.ValueChanged
    terminalElement.TryRemoveEventHandler PKey.ListView.ValueChanging

  interface ITerminalElement

type internal MarginTerminalElement(props: Props) =
  inherit AdornmentTerminalElement(props)

  override _.Name = "Margin"

  override _.NewView() = new Margin()

  override _.SetAsChildOfParentView = true

  override _.SetProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.SetProps(terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> Margin

    // Properties
    props
    |> Props.tryFind PKey.Margin.ShadowSize
    |> Option.iter (fun v -> view.ShadowSize <- v)

    props
    |> Props.tryFind PKey.Margin.ShadowStyle
    |> Option.iter (fun v -> view.ShadowStyle <- v)

  override _.RemoveProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.RemoveProps(terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> Margin

    // Properties
    props
    |> Props.tryFind PKey.Margin.ShadowSize
    |> Option.iter (fun _ ->
        view.ShadowSize <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.Margin.ShadowStyle
    |> Option.iter (fun _ ->
        view.ShadowStyle <- Unchecked.defaultof<_>)


  interface ITerminalElement

type internal MenuTerminalElement(props: Props) =
  inherit BarTerminalElement(props)

  override _.Name = "Menu"

  override _.NewView() = new Menu()

  override _.SetAsChildOfParentView = false

  override this.SubElements_PropKeys =
    [
      SubElementPropKey.from PKey.Menu.SelectedMenuItem_element
      SubElementPropKey.from PKey.Menu.SuperMenuItem_element
    ]
    |> List.append base.SubElements_PropKeys

  override _.SetProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.SetProps(terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> Menu

    // Properties
    props
    |> Props.tryFind PKey.Menu.SelectedMenuItem
    |> Option.iter (fun v -> view.SelectedMenuItem <- v)

    props
    |> Props.tryFind PKey.Menu.SuperMenuItem
    |> Option.iter (fun v -> view.SuperMenuItem <- v)

    // Events
    terminalElement.TrySetEventHandler(PKey.Menu.SelectedMenuItemChanged, view.SelectedMenuItemChanged)

  override _.RemoveProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.RemoveProps(terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> Menu

    // Properties
    props
    |> Props.tryFind PKey.Menu.SelectedMenuItem
    |> Option.iter (fun _ ->
        view.SelectedMenuItem <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.Menu.SuperMenuItem
    |> Option.iter (fun _ ->
        view.SuperMenuItem <- Unchecked.defaultof<_>)

    // Events
    terminalElement.TryRemoveEventHandler PKey.Menu.SelectedMenuItemChanged

  interface IMenuTerminalElement

  interface ITerminalElement

type internal MenuBarTerminalElement(props: Props) =
  inherit MenuTerminalElement(props)

  override _.Name = "MenuBar"

  override _.NewView() = new MenuBar()

  override _.SetAsChildOfParentView = true

  override _.SetProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.SetProps(terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> MenuBar

    // Properties
    props
    |> Props.tryFind PKey.MenuBar.Key
    |> Option.iter (fun v -> view.Key <- v)

    // Events
    terminalElement.TrySetEventHandler(PKey.MenuBar.KeyChanged, view.KeyChanged)

  override _.RemoveProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.RemoveProps(terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> MenuBar

    // Properties
    props
    |> Props.tryFind PKey.MenuBar.Key
    |> Option.iter (fun _ ->
        view.Key <- Unchecked.defaultof<_>)

    // Events
    terminalElement.TryRemoveEventHandler PKey.MenuBar.KeyChanged

  interface IMenuTerminalElement

  interface ITerminalElement

type internal NumericUpDownTerminalElement<'T>(props: Props) =
  inherit ViewTerminalElement(props)

  override _.Name = "NumericUpDown`1"

  override _.NewView() = new NumericUpDown<'T>()

  override _.SetAsChildOfParentView = true

  override _.SetProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.SetProps(terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
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
    terminalElement.TrySetEventHandler(PKey.NumericUpDown<'T>.FormatChanged, view.FormatChanged)

    terminalElement.TrySetEventHandler(PKey.NumericUpDown<'T>.IncrementChanged, view.IncrementChanged)

    terminalElement.TrySetEventHandler(PKey.NumericUpDown<'T>.ValueChanged, view.ValueChanged)

    terminalElement.TrySetEventHandler(PKey.NumericUpDown<'T>.ValueChanging, view.ValueChanging)

  override _.RemoveProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.RemoveProps(terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> NumericUpDown<'T>

    // Properties
    props
    |> Props.tryFind PKey.NumericUpDown<'T>.Format
    |> Option.iter (fun _ ->
        view.Format <- "")

    props
    |> Props.tryFind PKey.NumericUpDown<'T>.Increment
    |> Option.iter (fun _ ->
        view.Increment <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.NumericUpDown<'T>.Value
    |> Option.iter (fun _ ->
        view.Value <- Unchecked.defaultof<_>)

    // Events
    terminalElement.TryRemoveEventHandler PKey.NumericUpDown<'T>.FormatChanged
    terminalElement.TryRemoveEventHandler PKey.NumericUpDown<'T>.IncrementChanged
    terminalElement.TryRemoveEventHandler PKey.NumericUpDown<'T>.ValueChanged
    terminalElement.TryRemoveEventHandler PKey.NumericUpDown<'T>.ValueChanging

  interface ITerminalElement

type internal NumericUpDownTerminalElement(props: Props) =
  inherit NumericUpDownTerminalElement<int>(props)

  override _.Name = "NumericUpDown"

  override _.NewView() = new NumericUpDown()

  override _.SetAsChildOfParentView = true


  interface ITerminalElement

type internal PaddingTerminalElement(props: Props) =
  inherit AdornmentTerminalElement(props)

  override _.Name = "Padding"

  override _.NewView() = new Padding()

  override _.SetAsChildOfParentView = true


  interface ITerminalElement

[<AbstractClass>]
type internal PopoverBaseImplTerminalElement(props: Props) =
  inherit ViewTerminalElement(props)

  override _.Name = "PopoverBaseImpl"

  override _.NewView() = failwith "Cannot instantiate abstract view type PopoverBaseImpl"

  override _.SetAsChildOfParentView = true

  override _.SetProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.SetProps(terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> PopoverBaseImpl

    // Properties
    props
    |> Props.tryFind PKey.PopoverBaseImpl.Current
    |> Option.iter (fun v -> view.Current <- v)

  override _.RemoveProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.RemoveProps(terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> PopoverBaseImpl

    // Properties
    props
    |> Props.tryFind PKey.PopoverBaseImpl.Current
    |> Option.iter (fun _ ->
        view.Current <- Unchecked.defaultof<_>)


  interface ITerminalElement

type internal PopoverMenuTerminalElement(props: Props) =
  inherit PopoverBaseImplTerminalElement(props)

  override _.Name = "PopoverMenu"

  override _.NewView() = new PopoverMenu()

  override _.SetAsChildOfParentView = true

  override this.SubElements_PropKeys =
    [
      SubElementPropKey.from PKey.PopoverMenu.Root_element
    ]
    |> List.append base.SubElements_PropKeys

  override _.SetProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.SetProps(terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
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
    terminalElement.TrySetEventHandler(PKey.PopoverMenu.KeyChanged, view.KeyChanged)

  override _.RemoveProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.RemoveProps(terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> PopoverMenu

    // Properties
    props
    |> Props.tryFind PKey.PopoverMenu.Key
    |> Option.iter (fun _ ->
        view.Key <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.PopoverMenu.MouseFlags
    |> Option.iter (fun _ ->
        view.MouseFlags <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.PopoverMenu.Root
    |> Option.iter (fun _ ->
        view.Root <- Unchecked.defaultof<_>)

    // Events
    terminalElement.TryRemoveEventHandler PKey.PopoverMenu.KeyChanged

  interface IPopoverMenuTerminalElement

  interface ITerminalElement

type internal ProgressBarTerminalElement(props: Props) =
  inherit ViewTerminalElement(props)

  override _.Name = "ProgressBar"

  override _.NewView() = new ProgressBar()

  override _.SetAsChildOfParentView = true

  override _.SetProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.SetProps(terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
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
    |> Props.tryFind PKey.ProgressBar.Text
    |> Option.iter (fun v -> view.Text <- v)

  override _.RemoveProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.RemoveProps(terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> ProgressBar

    // Properties
    props
    |> Props.tryFind PKey.ProgressBar.BidirectionalMarquee
    |> Option.iter (fun _ ->
        view.BidirectionalMarquee <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.ProgressBar.Fraction
    |> Option.iter (fun _ ->
        view.Fraction <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.ProgressBar.ProgressBarFormat
    |> Option.iter (fun _ ->
        view.ProgressBarFormat <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.ProgressBar.ProgressBarStyle
    |> Option.iter (fun _ ->
        view.ProgressBarStyle <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.ProgressBar.SegmentCharacter
    |> Option.iter (fun _ ->
        view.SegmentCharacter <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.ProgressBar.Text
    |> Option.iter (fun _ ->
        view.Text <- "")


  interface ITerminalElement

type internal RunnableTerminalElement(props: Props) =
  inherit ViewTerminalElement(props)

  override _.Name = "Runnable"

  override _.NewView() = new Runnable()

  override _.SetAsChildOfParentView = true

  override _.SetProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.SetProps(terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> Runnable

    // Properties
    props
    |> Props.tryFind PKey.Runnable.Result
    |> Option.iter (fun v -> view.Result <- v)

    props
    |> Props.tryFind PKey.Runnable.StopRequested
    |> Option.iter (fun v -> view.StopRequested <- v)

    // Events
    terminalElement.TrySetEventHandler(PKey.Runnable.IsModalChanged, view.IsModalChanged)

    terminalElement.TrySetEventHandler(PKey.Runnable.IsRunningChanged, view.IsRunningChanged)

    terminalElement.TrySetEventHandler(PKey.Runnable.IsRunningChanging, view.IsRunningChanging)

  override _.RemoveProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.RemoveProps(terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> Runnable

    // Properties
    props
    |> Props.tryFind PKey.Runnable.Result
    |> Option.iter (fun _ ->
        view.Result <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.Runnable.StopRequested
    |> Option.iter (fun _ ->
        view.StopRequested <- Unchecked.defaultof<_>)

    // Events
    terminalElement.TryRemoveEventHandler PKey.Runnable.IsModalChanged
    terminalElement.TryRemoveEventHandler PKey.Runnable.IsRunningChanged
    terminalElement.TryRemoveEventHandler PKey.Runnable.IsRunningChanging

  interface ITerminalElement

type internal RunnableTerminalElement<'TResult>(props: Props) =
  inherit RunnableTerminalElement(props)

  override _.Name = "Runnable`1"

  override _.NewView() = new Runnable<'TResult>()

  override _.SetAsChildOfParentView = true

  override _.SetProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.SetProps(terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> Runnable<'TResult>

    // Properties
    props
    |> Props.tryFind PKey.Runnable'<'TResult>.Result
    |> Option.iter (fun v -> view.Result <- v)

  override _.RemoveProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.RemoveProps(terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> Runnable<'TResult>

    // Properties
    props
    |> Props.tryFind PKey.Runnable'<'TResult>.Result
    |> Option.iter (fun _ ->
        view.Result <- Unchecked.defaultof<_>)


  interface ITerminalElement

type internal DialogTerminalElement<'TResult>(props: Props) =
  inherit RunnableTerminalElement<'TResult>(props)

  override _.Name = "Dialog`1"

  override _.NewView() = new Dialog<'TResult>()

  override _.SetAsChildOfParentView = true

  override _.SetProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.SetProps(terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
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

  override _.RemoveProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.RemoveProps(terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> Dialog<'TResult>

    // Properties
    props
    |> Props.tryFind PKey.Dialog<'TResult>.ButtonAlignment
    |> Option.iter (fun _ ->
        view.ButtonAlignment <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.Dialog<'TResult>.ButtonAlignmentModes
    |> Option.iter (fun _ ->
        view.ButtonAlignmentModes <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.Dialog<'TResult>.Buttons
    |> Option.iter (fun _ ->
        view.Buttons <- Unchecked.defaultof<_>)


  interface ITerminalElement

type internal DialogTerminalElement(props: Props) =
  inherit DialogTerminalElement<int>(props)

  override _.Name = "Dialog"

  override _.NewView() = new Dialog()

  override _.SetAsChildOfParentView = true

  override _.SetProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.SetProps(terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> Dialog

    // Properties
    props
    |> Props.tryFind PKey.Dialog'.Result
    |> Option.iter (fun v -> view.Result <- v)

  override _.RemoveProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.RemoveProps(terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> Dialog

    // Properties
    props
    |> Props.tryFind PKey.Dialog'.Result
    |> Option.iter (fun _ ->
        view.Result <- Unchecked.defaultof<_>)


  interface ITerminalElement

type internal PromptTerminalElement<'TView, 'TResult when 'TView: (new: unit -> 'TView) and 'TView:> Terminal.Gui.ViewBase.View>(props: Props) =
  inherit DialogTerminalElement<'TResult>(props)

  override _.Name = "Prompt`2"

  override _.NewView() = new Prompt<'TView, 'TResult>()

  override _.SetAsChildOfParentView = true

  override _.SetProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.SetProps(terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> Prompt<'TView, 'TResult>

    // Properties
    props
    |> Props.tryFind PKey.Prompt<'TView, 'TResult>.ResultExtractor
    |> Option.iter (fun v -> view.ResultExtractor <- v)

  override _.RemoveProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.RemoveProps(terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> Prompt<'TView, 'TResult>

    // Properties
    props
    |> Props.tryFind PKey.Prompt<'TView, 'TResult>.ResultExtractor
    |> Option.iter (fun _ ->
        view.ResultExtractor <- Unchecked.defaultof<_>)


  interface ITerminalElement

type internal FileDialogTerminalElement(props: Props) =
  inherit DialogTerminalElement(props)

  override _.Name = "FileDialog"

  override _.NewView() = new FileDialog()

  override _.SetAsChildOfParentView = true

  override _.SetProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.SetProps(terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
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
    terminalElement.TrySetEventHandler(PKey.FileDialog.FilesSelected, view.FilesSelected)

  override _.RemoveProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.RemoveProps(terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> FileDialog

    // Properties
    props
    |> Props.tryFind PKey.FileDialog.AllowedTypes
    |> Option.iter (fun _ ->
        view.AllowedTypes <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.FileDialog.AllowsMultipleSelection
    |> Option.iter (fun _ ->
        view.AllowsMultipleSelection <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.FileDialog.FileOperationsHandler
    |> Option.iter (fun _ ->
        view.FileOperationsHandler <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.FileDialog.MustExist
    |> Option.iter (fun _ ->
        view.MustExist <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.FileDialog.OpenMode
    |> Option.iter (fun _ ->
        view.OpenMode <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.FileDialog.Path
    |> Option.iter (fun _ ->
        view.Path <- "")

    props
    |> Props.tryFind PKey.FileDialog.SearchMatcher
    |> Option.iter (fun _ ->
        view.SearchMatcher <- Unchecked.defaultof<_>)

    // Events
    terminalElement.TryRemoveEventHandler PKey.FileDialog.FilesSelected

  interface ITerminalElement

type internal OpenDialogTerminalElement(props: Props) =
  inherit FileDialogTerminalElement(props)

  override _.Name = "OpenDialog"

  override _.NewView() = new OpenDialog()

  override _.SetAsChildOfParentView = true

  override _.SetProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.SetProps(terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> OpenDialog

    // Properties
    props
    |> Props.tryFind PKey.OpenDialog.OpenMode
    |> Option.iter (fun v -> view.OpenMode <- v)

  override _.RemoveProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.RemoveProps(terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> OpenDialog

    // Properties
    props
    |> Props.tryFind PKey.OpenDialog.OpenMode
    |> Option.iter (fun _ ->
        view.OpenMode <- Unchecked.defaultof<_>)


  interface ITerminalElement

type internal SaveDialogTerminalElement(props: Props) =
  inherit FileDialogTerminalElement(props)

  override _.Name = "SaveDialog"

  override _.NewView() = new SaveDialog()

  override _.SetAsChildOfParentView = true


  interface ITerminalElement

type internal ScrollBarTerminalElement(props: Props) =
  inherit ViewTerminalElement(props)

  override _.Name = "ScrollBar"

  override _.NewView() = new ScrollBar()

  override _.SetAsChildOfParentView = true

  override _.SetProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.SetProps(terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> ScrollBar

    // Properties
    props
    |> Props.tryFind PKey.ScrollBar.AutoShow
    |> Option.iter (fun v -> view.AutoShow <- v)

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
    |> Props.tryFind PKey.ScrollBar.VisibleContentSize
    |> Option.iter (fun v -> view.VisibleContentSize <- v)

    // Events
    terminalElement.TrySetEventHandler(PKey.ScrollBar.OrientationChanged, view.OrientationChanged)

    terminalElement.TrySetEventHandler(PKey.ScrollBar.OrientationChanging, view.OrientationChanging)

    terminalElement.TrySetEventHandler(PKey.ScrollBar.ScrollableContentSizeChanged, view.ScrollableContentSizeChanged)

    terminalElement.TrySetEventHandler(PKey.ScrollBar.Scrolled, view.Scrolled)

    terminalElement.TrySetEventHandler(PKey.ScrollBar.SliderPositionChanged, view.SliderPositionChanged)

    terminalElement.TrySetEventHandler(PKey.ScrollBar.ValueChanged, view.ValueChanged)

    terminalElement.TrySetEventHandler(PKey.ScrollBar.ValueChanging, view.ValueChanging)

  override _.RemoveProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.RemoveProps(terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> ScrollBar

    // Properties
    props
    |> Props.tryFind PKey.ScrollBar.AutoShow
    |> Option.iter (fun _ ->
        view.AutoShow <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.ScrollBar.Increment
    |> Option.iter (fun _ ->
        view.Increment <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.ScrollBar.Orientation
    |> Option.iter (fun _ ->
        view.Orientation <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.ScrollBar.ScrollableContentSize
    |> Option.iter (fun _ ->
        view.ScrollableContentSize <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.ScrollBar.Value
    |> Option.iter (fun _ ->
        view.Value <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.ScrollBar.VisibleContentSize
    |> Option.iter (fun _ ->
        view.VisibleContentSize <- Unchecked.defaultof<_>)

    // Events
    terminalElement.TryRemoveEventHandler PKey.ScrollBar.OrientationChanged
    terminalElement.TryRemoveEventHandler PKey.ScrollBar.OrientationChanging
    terminalElement.TryRemoveEventHandler PKey.ScrollBar.ScrollableContentSizeChanged
    terminalElement.TryRemoveEventHandler PKey.ScrollBar.Scrolled
    terminalElement.TryRemoveEventHandler PKey.ScrollBar.SliderPositionChanged
    terminalElement.TryRemoveEventHandler PKey.ScrollBar.ValueChanged
    terminalElement.TryRemoveEventHandler PKey.ScrollBar.ValueChanging

  interface ITerminalElement

type internal ScrollSliderTerminalElement(props: Props) =
  inherit ViewTerminalElement(props)

  override _.Name = "ScrollSlider"

  override _.NewView() = new ScrollSlider()

  override _.SetAsChildOfParentView = true

  override _.SetProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.SetProps(terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
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
    |> Props.tryFind PKey.ScrollSlider.VisibleContentSize
    |> Option.iter (fun v -> view.VisibleContentSize <- v)

    // Events
    terminalElement.TrySetEventHandler(PKey.ScrollSlider.OrientationChanged, view.OrientationChanged)

    terminalElement.TrySetEventHandler(PKey.ScrollSlider.OrientationChanging, view.OrientationChanging)

    terminalElement.TrySetEventHandler(PKey.ScrollSlider.PositionChanged, view.PositionChanged)

    terminalElement.TrySetEventHandler(PKey.ScrollSlider.PositionChanging, view.PositionChanging)

    terminalElement.TrySetEventHandler(PKey.ScrollSlider.Scrolled, view.Scrolled)

  override _.RemoveProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.RemoveProps(terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> ScrollSlider

    // Properties
    props
    |> Props.tryFind PKey.ScrollSlider.Orientation
    |> Option.iter (fun _ ->
        view.Orientation <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.ScrollSlider.Position
    |> Option.iter (fun _ ->
        view.Position <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.ScrollSlider.Size
    |> Option.iter (fun _ ->
        view.Size <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.ScrollSlider.SliderPadding
    |> Option.iter (fun _ ->
        view.SliderPadding <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.ScrollSlider.VisibleContentSize
    |> Option.iter (fun _ ->
        view.VisibleContentSize <- Unchecked.defaultof<_>)

    // Events
    terminalElement.TryRemoveEventHandler PKey.ScrollSlider.OrientationChanged
    terminalElement.TryRemoveEventHandler PKey.ScrollSlider.OrientationChanging
    terminalElement.TryRemoveEventHandler PKey.ScrollSlider.PositionChanged
    terminalElement.TryRemoveEventHandler PKey.ScrollSlider.PositionChanging
    terminalElement.TryRemoveEventHandler PKey.ScrollSlider.Scrolled

  interface ITerminalElement

[<AbstractClass>]
type internal SelectorBaseTerminalElement(props: Props) =
  inherit ViewTerminalElement(props)

  override _.Name = "SelectorBase"

  override _.NewView() = failwith "Cannot instantiate abstract view type SelectorBase"

  override _.SetAsChildOfParentView = true

  override _.SetProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.SetProps(terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
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
    |> Props.tryFind PKey.SelectorBase.Value
    |> Option.iter (fun v -> view.Value <- v)

    props
    |> Props.tryFind PKey.SelectorBase.Values
    |> Option.iter (fun v -> view.Values <- v)

    // Events
    terminalElement.TrySetEventHandler(PKey.SelectorBase.OrientationChanged, view.OrientationChanged)

    terminalElement.TrySetEventHandler(PKey.SelectorBase.OrientationChanging, view.OrientationChanging)

    terminalElement.TrySetEventHandler(PKey.SelectorBase.ValueChanged, view.ValueChanged)

    terminalElement.TrySetEventHandler(PKey.SelectorBase.ValueChanging, view.ValueChanging)

  override _.RemoveProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.RemoveProps(terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> SelectorBase

    // Properties
    props
    |> Props.tryFind PKey.SelectorBase.DoubleClickAccepts
    |> Option.iter (fun _ ->
        view.DoubleClickAccepts <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.SelectorBase.HorizontalSpace
    |> Option.iter (fun _ ->
        view.HorizontalSpace <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.SelectorBase.Labels
    |> Option.iter (fun _ ->
        view.Labels <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.SelectorBase.Orientation
    |> Option.iter (fun _ ->
        view.Orientation <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.SelectorBase.Styles
    |> Option.iter (fun _ ->
        view.Styles <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.SelectorBase.Value
    |> Option.iter (fun _ ->
        view.Value <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.SelectorBase.Values
    |> Option.iter (fun _ ->
        view.Values <- Unchecked.defaultof<_>)

    // Events
    terminalElement.TryRemoveEventHandler PKey.SelectorBase.OrientationChanged
    terminalElement.TryRemoveEventHandler PKey.SelectorBase.OrientationChanging
    terminalElement.TryRemoveEventHandler PKey.SelectorBase.ValueChanged
    terminalElement.TryRemoveEventHandler PKey.SelectorBase.ValueChanging

  interface ITerminalElement

type internal FlagSelectorTerminalElement(props: Props) =
  inherit SelectorBaseTerminalElement(props)

  override _.Name = "FlagSelector"

  override _.NewView() = new FlagSelector()

  override _.SetAsChildOfParentView = true

  override _.SetProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.SetProps(terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> FlagSelector

    // Properties
    props
    |> Props.tryFind PKey.FlagSelector.Value
    |> Option.iter (fun v -> view.Value <- v)

  override _.RemoveProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.RemoveProps(terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> FlagSelector

    // Properties
    props
    |> Props.tryFind PKey.FlagSelector.Value
    |> Option.iter (fun _ ->
        view.Value <- Unchecked.defaultof<_>)


  interface ITerminalElement

type internal OptionSelectorTerminalElement(props: Props) =
  inherit SelectorBaseTerminalElement(props)

  override _.Name = "OptionSelector"

  override _.NewView() = new OptionSelector()

  override _.SetAsChildOfParentView = true

  override _.SetProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.SetProps(terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> OptionSelector

    // Properties
    props
    |> Props.tryFind PKey.OptionSelector.Cursor
    |> Option.iter (fun v -> view.Cursor <- v)

  override _.RemoveProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.RemoveProps(terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> OptionSelector

    // Properties
    props
    |> Props.tryFind PKey.OptionSelector.Cursor
    |> Option.iter (fun _ ->
        view.Cursor <- Unchecked.defaultof<_>)


  interface ITerminalElement

type internal FlagSelectorTerminalElement<'TFlagsEnum when 'TFlagsEnum: struct and 'TFlagsEnum: (new: unit -> 'TFlagsEnum) and 'TFlagsEnum:> System.Enum and 'TFlagsEnum:> System.ValueType>(props: Props) =
  inherit FlagSelectorTerminalElement(props)

  override _.Name = "FlagSelector`1"

  override _.NewView() = new FlagSelector<'TFlagsEnum>()

  override _.SetAsChildOfParentView = true

  override _.SetProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.SetProps(terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> FlagSelector<'TFlagsEnum>

    // Properties
    props
    |> Props.tryFind PKey.FlagSelector'<'TFlagsEnum>.Value
    |> Option.iter (fun v -> view.Value <- v)

    // Events
    terminalElement.TrySetEventHandler(PKey.FlagSelector'<'TFlagsEnum>.ValueChanged, view.ValueChanged)

  override _.RemoveProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.RemoveProps(terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> FlagSelector<'TFlagsEnum>

    // Properties
    props
    |> Props.tryFind PKey.FlagSelector'<'TFlagsEnum>.Value
    |> Option.iter (fun _ ->
        view.Value <- Unchecked.defaultof<_>)

    // Events
    terminalElement.TryRemoveEventHandler PKey.FlagSelector'<'TFlagsEnum>.ValueChanged

  interface ITerminalElement

type internal OptionSelectorTerminalElement<'TEnum when 'TEnum: struct and 'TEnum: (new: unit -> 'TEnum) and 'TEnum:> System.Enum and 'TEnum:> System.ValueType>(props: Props) =
  inherit OptionSelectorTerminalElement(props)

  override _.Name = "OptionSelector`1"

  override _.NewView() = new OptionSelector<'TEnum>()

  override _.SetAsChildOfParentView = true

  override _.SetProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.SetProps(terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> OptionSelector<'TEnum>

    // Properties
    props
    |> Props.tryFind PKey.OptionSelector'<'TEnum>.Value
    |> Option.iter (fun v -> view.Value <- v)

    props
    |> Props.tryFind PKey.OptionSelector'<'TEnum>.Values
    |> Option.iter (fun v -> view.Values <- v)

    // Events
    terminalElement.TrySetEventHandler(PKey.OptionSelector'<'TEnum>.ValueChanged, view.ValueChanged)

  override _.RemoveProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.RemoveProps(terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> OptionSelector<'TEnum>

    // Properties
    props
    |> Props.tryFind PKey.OptionSelector'<'TEnum>.Value
    |> Option.iter (fun _ ->
        view.Value <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.OptionSelector'<'TEnum>.Values
    |> Option.iter (fun _ ->
        view.Values <- Unchecked.defaultof<_>)

    // Events
    terminalElement.TryRemoveEventHandler PKey.OptionSelector'<'TEnum>.ValueChanged

  interface ITerminalElement

type internal ShortcutTerminalElement(props: Props) =
  inherit ViewTerminalElement(props)

  override _.Name = "Shortcut"

  override _.NewView() = new Shortcut()

  override _.SetAsChildOfParentView = true

  override this.SubElements_PropKeys =
    [
      SubElementPropKey.from PKey.Shortcut.CommandView_element
    ]
    |> List.append base.SubElements_PropKeys

  override _.SetProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.SetProps(terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
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
    |> Props.tryFind PKey.Shortcut.CommandView
    |> Option.iter (fun v -> view.CommandView <- v)

    props
    |> Props.tryFind PKey.Shortcut.ForceFocusColors
    |> Option.iter (fun v -> view.ForceFocusColors <- v)

    props
    |> Props.tryFind PKey.Shortcut.HelpText
    |> Option.iter (fun v -> view.HelpText <- v)

    props
    |> Props.tryFind PKey.Shortcut.Key
    |> Option.iter (fun v -> view.Key <- v)

    props
    |> Props.tryFind PKey.Shortcut.MinimumKeyTextSize
    |> Option.iter (fun v -> view.MinimumKeyTextSize <- v)

    props
    |> Props.tryFind PKey.Shortcut.Orientation
    |> Option.iter (fun v -> view.Orientation <- v)

    props
    |> Props.tryFind PKey.Shortcut.Text
    |> Option.iter (fun v -> view.Text <- v)

    // Events
    terminalElement.TrySetEventHandler(PKey.Shortcut.OrientationChanged, view.OrientationChanged)

    terminalElement.TrySetEventHandler(PKey.Shortcut.OrientationChanging, view.OrientationChanging)

  override _.RemoveProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.RemoveProps(terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> Shortcut

    // Properties
    props
    |> Props.tryFind PKey.Shortcut.Action
    |> Option.iter (fun _ ->
        view.Action <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.Shortcut.AlignmentModes
    |> Option.iter (fun _ ->
        view.AlignmentModes <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.Shortcut.BindKeyToApplication
    |> Option.iter (fun _ ->
        view.BindKeyToApplication <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.Shortcut.CommandView
    |> Option.iter (fun _ ->
        view.CommandView <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.Shortcut.ForceFocusColors
    |> Option.iter (fun _ ->
        view.ForceFocusColors <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.Shortcut.HelpText
    |> Option.iter (fun _ ->
        view.HelpText <- "")

    props
    |> Props.tryFind PKey.Shortcut.Key
    |> Option.iter (fun _ ->
        view.Key <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.Shortcut.MinimumKeyTextSize
    |> Option.iter (fun _ ->
        view.MinimumKeyTextSize <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.Shortcut.Orientation
    |> Option.iter (fun _ ->
        view.Orientation <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.Shortcut.Text
    |> Option.iter (fun _ ->
        view.Text <- "")

    // Events
    terminalElement.TryRemoveEventHandler PKey.Shortcut.OrientationChanged
    terminalElement.TryRemoveEventHandler PKey.Shortcut.OrientationChanging

  interface ITerminalElement

type internal MenuItemTerminalElement(props: Props) =
  inherit ShortcutTerminalElement(props)

  override _.Name = "MenuItem"

  override _.NewView() = new MenuItem()

  override _.SetAsChildOfParentView = true

  override this.SubElements_PropKeys =
    [
      SubElementPropKey.from PKey.MenuItem.SubMenu_element
      SubElementPropKey.from PKey.MenuItem.TargetView_element
    ]
    |> List.append base.SubElements_PropKeys

  override _.SetProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.SetProps(terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> MenuItem

    // Properties
    props
    |> Props.tryFind PKey.MenuItem.Command
    |> Option.iter (fun v -> view.Command <- v)

    props
    |> Props.tryFind PKey.MenuItem.SubMenu
    |> Option.iter (fun v -> view.SubMenu <- v)

    props
    |> Props.tryFind PKey.MenuItem.TargetView
    |> Option.iter (fun v -> view.TargetView <- v)

  override _.RemoveProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.RemoveProps(terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> MenuItem

    // Properties
    props
    |> Props.tryFind PKey.MenuItem.Command
    |> Option.iter (fun _ ->
        view.Command <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.MenuItem.SubMenu
    |> Option.iter (fun _ ->
        view.SubMenu <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.MenuItem.TargetView
    |> Option.iter (fun _ ->
        view.TargetView <- Unchecked.defaultof<_>)


  interface IMenuItemTerminalElement

  interface ITerminalElement

type internal MenuBarItemTerminalElement(props: Props) =
  inherit MenuItemTerminalElement(props)

  override _.Name = "MenuBarItem"

  override _.NewView() = new MenuBarItem()

  override _.SetAsChildOfParentView = true

  override this.SubElements_PropKeys =
    [
      SubElementPropKey.from PKey.MenuBarItem.PopoverMenu_element
      SubElementPropKey.from PKey.MenuBarItem.SubMenu_element
    ]
    |> List.append base.SubElements_PropKeys

  override _.SetProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.SetProps(terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> MenuBarItem

    // Properties
    props
    |> Props.tryFind PKey.MenuBarItem.PopoverMenu
    |> Option.iter (fun v -> view.PopoverMenu <- v)

    props
    |> Props.tryFind PKey.MenuBarItem.PopoverMenuOpen
    |> Option.iter (fun v -> view.PopoverMenuOpen <- v)

    props
    |> Props.tryFind PKey.MenuBarItem.SubMenu
    |> Option.iter (fun v -> view.SubMenu <- v)

    // Events
    terminalElement.TrySetEventHandler(PKey.MenuBarItem.PopoverMenuOpenChanged, view.PopoverMenuOpenChanged)

  override _.RemoveProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.RemoveProps(terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> MenuBarItem

    // Properties
    props
    |> Props.tryFind PKey.MenuBarItem.PopoverMenu
    |> Option.iter (fun _ ->
        view.PopoverMenu <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.MenuBarItem.PopoverMenuOpen
    |> Option.iter (fun _ ->
        view.PopoverMenuOpen <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.MenuBarItem.SubMenu
    |> Option.iter (fun _ ->
        view.SubMenu <- Unchecked.defaultof<_>)

    // Events
    terminalElement.TryRemoveEventHandler PKey.MenuBarItem.PopoverMenuOpenChanged

  interface IMenuItemTerminalElement

  interface ITerminalElement

type internal SpinnerViewTerminalElement(props: Props) =
  inherit ViewTerminalElement(props)

  override _.Name = "SpinnerView"

  override _.NewView() = new SpinnerView()

  override _.SetAsChildOfParentView = true

  override _.SetProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.SetProps(terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
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

  override _.RemoveProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.RemoveProps(terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> SpinnerView

    // Properties
    props
    |> Props.tryFind PKey.SpinnerView.AutoSpin
    |> Option.iter (fun _ ->
        view.AutoSpin <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.SpinnerView.Sequence
    |> Option.iter (fun _ ->
        view.Sequence <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.SpinnerView.SpinBounce
    |> Option.iter (fun _ ->
        view.SpinBounce <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.SpinnerView.SpinDelay
    |> Option.iter (fun _ ->
        view.SpinDelay <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.SpinnerView.SpinReverse
    |> Option.iter (fun _ ->
        view.SpinReverse <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.SpinnerView.Style
    |> Option.iter (fun _ ->
        view.Style <- Unchecked.defaultof<_>)


  interface ITerminalElement

type internal StatusBarTerminalElement(props: Props) =
  inherit BarTerminalElement(props)

  override _.Name = "StatusBar"

  override _.NewView() = new StatusBar()

  override _.SetAsChildOfParentView = true


  interface ITerminalElement

type internal TabTerminalElement(props: Props) =
  inherit ViewTerminalElement(props)

  override _.Name = "Tab"

  override _.NewView() = new Tab()

  override _.SetAsChildOfParentView = true

  override this.SubElements_PropKeys =
    [
      SubElementPropKey.from PKey.Tab.View_element
    ]
    |> List.append base.SubElements_PropKeys

  override _.SetProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.SetProps(terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> Tab

    // Properties
    props
    |> Props.tryFind PKey.Tab.DisplayText
    |> Option.iter (fun v -> view.DisplayText <- v)

    props
    |> Props.tryFind PKey.Tab.View
    |> Option.iter (fun v -> view.View <- v)

  override _.RemoveProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.RemoveProps(terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> Tab

    // Properties
    props
    |> Props.tryFind PKey.Tab.DisplayText
    |> Option.iter (fun _ ->
        view.DisplayText <- "")

    props
    |> Props.tryFind PKey.Tab.View
    |> Option.iter (fun _ ->
        view.View <- Unchecked.defaultof<_>)


  interface ITabTerminalElement

  interface ITerminalElement

type internal TabViewTerminalElement(props: Props) =
  inherit ViewTerminalElement(props)

  override _.Name = "TabView"

  override _.NewView() = new TabView()

  override _.SetAsChildOfParentView = true

  override this.SubElements_PropKeys =
    [
      SubElementPropKey.from PKey.TabView.SelectedTab_element
    ]
    |> List.append base.SubElements_PropKeys

  override _.SetProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.SetProps(terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> TabView

    // Properties
    props
    |> Props.tryFind PKey.TabView.MaxTabTextWidth
    |> Option.iter (fun v -> view.MaxTabTextWidth <- v)

    props
    |> Props.tryFind PKey.TabView.SelectedTab
    |> Option.iter (fun v -> view.SelectedTab <- v)

    props
    |> Props.tryFind PKey.TabView.Style
    |> Option.iter (fun v -> view.Style <- v)

    props
    |> Props.tryFind PKey.TabView.TabScrollOffset
    |> Option.iter (fun v -> view.TabScrollOffset <- v)

    // Events
    terminalElement.TrySetEventHandler(PKey.TabView.SelectedTabChanged, view.SelectedTabChanged)

    terminalElement.TrySetEventHandler(PKey.TabView.TabClicked, view.TabClicked)

  override _.RemoveProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.RemoveProps(terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> TabView

    // Properties
    props
    |> Props.tryFind PKey.TabView.MaxTabTextWidth
    |> Option.iter (fun _ ->
        view.MaxTabTextWidth <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.TabView.SelectedTab
    |> Option.iter (fun _ ->
        view.SelectedTab <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.TabView.Style
    |> Option.iter (fun _ ->
        view.Style <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.TabView.TabScrollOffset
    |> Option.iter (fun _ ->
        view.TabScrollOffset <- Unchecked.defaultof<_>)

    // Events
    terminalElement.TryRemoveEventHandler PKey.TabView.SelectedTabChanged
    terminalElement.TryRemoveEventHandler PKey.TabView.TabClicked

  interface ITerminalElement

type internal TableViewTerminalElement(props: Props) =
  inherit ViewTerminalElement(props)

  override _.Name = "TableView"

  override _.NewView() = new TableView()

  override _.SetAsChildOfParentView = true

  override _.SetProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.SetProps(terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> TableView

    // Properties
    props
    |> Props.tryFind PKey.TableView.CellActivationKey
    |> Option.iter (fun v -> view.CellActivationKey <- v)

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
    |> Props.tryFind PKey.TableView.SelectedColumn
    |> Option.iter (fun v -> view.SelectedColumn <- v)

    props
    |> Props.tryFind PKey.TableView.SelectedRow
    |> Option.iter (fun v -> view.SelectedRow <- v)

    props
    |> Props.tryFind PKey.TableView.SeparatorSymbol
    |> Option.iter (fun v -> view.SeparatorSymbol <- v)

    props
    |> Props.tryFind PKey.TableView.Style
    |> Option.iter (fun v -> view.Style <- v)

    props
    |> Props.tryFind PKey.TableView.Table
    |> Option.iter (fun v -> view.Table <- v)

    // Events
    terminalElement.TrySetEventHandler(PKey.TableView.CellActivated, view.CellActivated)

    terminalElement.TrySetEventHandler(PKey.TableView.CellToggled, view.CellToggled)

    terminalElement.TrySetEventHandler(PKey.TableView.SelectedCellChanged, view.SelectedCellChanged)

  override _.RemoveProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.RemoveProps(terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> TableView

    // Properties
    props
    |> Props.tryFind PKey.TableView.CellActivationKey
    |> Option.iter (fun _ ->
        view.CellActivationKey <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.TableView.CollectionNavigator
    |> Option.iter (fun _ ->
        view.CollectionNavigator <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.TableView.ColumnOffset
    |> Option.iter (fun _ ->
        view.ColumnOffset <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.TableView.FullRowSelect
    |> Option.iter (fun _ ->
        view.FullRowSelect <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.TableView.MaxCellWidth
    |> Option.iter (fun _ ->
        view.MaxCellWidth <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.TableView.MinCellWidth
    |> Option.iter (fun _ ->
        view.MinCellWidth <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.TableView.MultiSelect
    |> Option.iter (fun _ ->
        view.MultiSelect <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.TableView.NullSymbol
    |> Option.iter (fun _ ->
        view.NullSymbol <- "")

    props
    |> Props.tryFind PKey.TableView.RowOffset
    |> Option.iter (fun _ ->
        view.RowOffset <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.TableView.SelectedColumn
    |> Option.iter (fun _ ->
        view.SelectedColumn <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.TableView.SelectedRow
    |> Option.iter (fun _ ->
        view.SelectedRow <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.TableView.SeparatorSymbol
    |> Option.iter (fun _ ->
        view.SeparatorSymbol <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.TableView.Style
    |> Option.iter (fun _ ->
        view.Style <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.TableView.Table
    |> Option.iter (fun _ ->
        view.Table <- Unchecked.defaultof<_>)

    // Events
    terminalElement.TryRemoveEventHandler PKey.TableView.CellActivated
    terminalElement.TryRemoveEventHandler PKey.TableView.CellToggled
    terminalElement.TryRemoveEventHandler PKey.TableView.SelectedCellChanged

  interface ITerminalElement

type internal TextFieldTerminalElement(props: Props) =
  inherit ViewTerminalElement(props)

  override _.Name = "TextField"

  override _.NewView() = new TextField()

  override _.SetAsChildOfParentView = true

  override _.SetProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.SetProps(terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
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
    terminalElement.TrySetEventHandler(PKey.TextField.TextChanging, view.TextChanging)

    terminalElement.TrySetEventHandler(PKey.TextField.ValueChanged, view.ValueChanged)

    terminalElement.TrySetEventHandler(PKey.TextField.ValueChanging, view.ValueChanging)

  override _.RemoveProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.RemoveProps(terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> TextField

    // Properties
    props
    |> Props.tryFind PKey.TextField.Autocomplete
    |> Option.iter (fun _ ->
        view.Autocomplete <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.TextField.InsertionPoint
    |> Option.iter (fun _ ->
        view.InsertionPoint <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.TextField.ReadOnly
    |> Option.iter (fun _ ->
        view.ReadOnly <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.TextField.Secret
    |> Option.iter (fun _ ->
        view.Secret <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.TextField.SelectWordOnlyOnDoubleClick
    |> Option.iter (fun _ ->
        view.SelectWordOnlyOnDoubleClick <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.TextField.SelectedStart
    |> Option.iter (fun _ ->
        view.SelectedStart <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.TextField.Text
    |> Option.iter (fun _ ->
        view.Text <- "")

    props
    |> Props.tryFind PKey.TextField.UseSameRuneTypeForWords
    |> Option.iter (fun _ ->
        view.UseSameRuneTypeForWords <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.TextField.Used
    |> Option.iter (fun _ ->
        view.Used <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.TextField.Value
    |> Option.iter (fun _ ->
        view.Value <- "")

    // Events
    terminalElement.TryRemoveEventHandler PKey.TextField.TextChanging
    terminalElement.TryRemoveEventHandler PKey.TextField.ValueChanged
    terminalElement.TryRemoveEventHandler PKey.TextField.ValueChanging

  interface ITerminalElement

type internal DateFieldTerminalElement(props: Props) =
  inherit TextFieldTerminalElement(props)

  override _.Name = "DateField"

  override _.NewView() = new DateField()

  override _.SetAsChildOfParentView = true

  override _.SetProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.SetProps(terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> DateField

    // Properties
    props
    |> Props.tryFind PKey.DateField.Culture
    |> Option.iter (fun v -> view.Culture <- v)

    props
    |> Props.tryFind PKey.DateField.InsertionPoint
    |> Option.iter (fun v -> view.InsertionPoint <- v)

    props
    |> Props.tryFind PKey.DateField.Value
    |> Option.iter (fun v -> view.Value <- v)

    // Events
    terminalElement.TrySetEventHandler(PKey.DateField.ValueChanged, view.ValueChanged)

    terminalElement.TrySetEventHandler(PKey.DateField.ValueChanging, view.ValueChanging)

  override _.RemoveProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.RemoveProps(terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> DateField

    // Properties
    props
    |> Props.tryFind PKey.DateField.Culture
    |> Option.iter (fun _ ->
        view.Culture <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.DateField.InsertionPoint
    |> Option.iter (fun _ ->
        view.InsertionPoint <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.DateField.Value
    |> Option.iter (fun _ ->
        view.Value <- Unchecked.defaultof<_>)

    // Events
    terminalElement.TryRemoveEventHandler PKey.DateField.ValueChanged
    terminalElement.TryRemoveEventHandler PKey.DateField.ValueChanging

  interface ITerminalElement

type internal TextValidateFieldTerminalElement(props: Props) =
  inherit ViewTerminalElement(props)

  override _.Name = "TextValidateField"

  override _.NewView() = new TextValidateField()

  override _.SetAsChildOfParentView = true

  override _.SetProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.SetProps(terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> TextValidateField

    // Properties
    props
    |> Props.tryFind PKey.TextValidateField.Provider
    |> Option.iter (fun v -> view.Provider <- v)

    props
    |> Props.tryFind PKey.TextValidateField.Text
    |> Option.iter (fun v -> view.Text <- v)

  override _.RemoveProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.RemoveProps(terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> TextValidateField

    // Properties
    props
    |> Props.tryFind PKey.TextValidateField.Provider
    |> Option.iter (fun _ ->
        view.Provider <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.TextValidateField.Text
    |> Option.iter (fun _ ->
        view.Text <- "")


  interface ITerminalElement

type internal TextViewTerminalElement(props: Props) =
  inherit ViewTerminalElement(props)

  override _.Name = "TextView"

  override _.NewView() = new TextView()

  override _.SetAsChildOfParentView = true

  override _.SetProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.SetProps(terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
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
    |> Props.tryFind PKey.TextView.IsDirty
    |> Option.iter (fun v -> view.IsDirty <- v)

    props
    |> Props.tryFind PKey.TextView.IsSelecting
    |> Option.iter (fun v -> view.IsSelecting <- v)

    props
    |> Props.tryFind PKey.TextView.LeftColumn
    |> Option.iter (fun v -> view.LeftColumn <- v)

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
    |> Props.tryFind PKey.TextView.TopRow
    |> Option.iter (fun v -> view.TopRow <- v)

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
    terminalElement.TrySetEventHandler(PKey.TextView.ContentsChanged, view.ContentsChanged)

    terminalElement.TrySetEventHandler(PKey.TextView.DrawNormalColor, view.DrawNormalColor)

    terminalElement.TrySetEventHandler(PKey.TextView.DrawReadOnlyColor, view.DrawReadOnlyColor)

    terminalElement.TrySetEventHandler(PKey.TextView.DrawSelectionColor, view.DrawSelectionColor)

    terminalElement.TrySetEventHandler(PKey.TextView.DrawUsedColor, view.DrawUsedColor)

    terminalElement.TrySetEventHandler(PKey.TextView.UnwrappedCursorPosition, view.UnwrappedCursorPosition)

  override _.RemoveProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.RemoveProps(terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> TextView

    // Properties
    props
    |> Props.tryFind PKey.TextView.EnterKeyAddsLine
    |> Option.iter (fun _ ->
        view.EnterKeyAddsLine <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.TextView.InheritsPreviousAttribute
    |> Option.iter (fun _ ->
        view.InheritsPreviousAttribute <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.TextView.InsertionPoint
    |> Option.iter (fun _ ->
        view.InsertionPoint <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.TextView.IsDirty
    |> Option.iter (fun _ ->
        view.IsDirty <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.TextView.IsSelecting
    |> Option.iter (fun _ ->
        view.IsSelecting <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.TextView.LeftColumn
    |> Option.iter (fun _ ->
        view.LeftColumn <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.TextView.Multiline
    |> Option.iter (fun _ ->
        view.Multiline <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.TextView.ReadOnly
    |> Option.iter (fun _ ->
        view.ReadOnly <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.TextView.ScrollBars
    |> Option.iter (fun _ ->
        view.ScrollBars <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.TextView.SelectWordOnlyOnDoubleClick
    |> Option.iter (fun _ ->
        view.SelectWordOnlyOnDoubleClick <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.TextView.SelectionStartColumn
    |> Option.iter (fun _ ->
        view.SelectionStartColumn <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.TextView.SelectionStartRow
    |> Option.iter (fun _ ->
        view.SelectionStartRow <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.TextView.TabKeyAddsTab
    |> Option.iter (fun _ ->
        view.TabKeyAddsTab <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.TextView.TabWidth
    |> Option.iter (fun _ ->
        view.TabWidth <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.TextView.Text
    |> Option.iter (fun _ ->
        view.Text <- "")

    props
    |> Props.tryFind PKey.TextView.TopRow
    |> Option.iter (fun _ ->
        view.TopRow <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.TextView.UseSameRuneTypeForWords
    |> Option.iter (fun _ ->
        view.UseSameRuneTypeForWords <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.TextView.Used
    |> Option.iter (fun _ ->
        view.Used <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.TextView.WordWrap
    |> Option.iter (fun _ ->
        view.WordWrap <- Unchecked.defaultof<_>)

    // Events
    terminalElement.TryRemoveEventHandler PKey.TextView.ContentsChanged
    terminalElement.TryRemoveEventHandler PKey.TextView.DrawNormalColor
    terminalElement.TryRemoveEventHandler PKey.TextView.DrawReadOnlyColor
    terminalElement.TryRemoveEventHandler PKey.TextView.DrawSelectionColor
    terminalElement.TryRemoveEventHandler PKey.TextView.DrawUsedColor
    terminalElement.TryRemoveEventHandler PKey.TextView.UnwrappedCursorPosition

  interface ITerminalElement

type internal TimeFieldTerminalElement(props: Props) =
  inherit TextFieldTerminalElement(props)

  override _.Name = "TimeField"

  override _.NewView() = new TimeField()

  override _.SetAsChildOfParentView = true

  override _.SetProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.SetProps(terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> TimeField

    // Properties
    props
    |> Props.tryFind PKey.TimeField.InsertionPoint
    |> Option.iter (fun v -> view.InsertionPoint <- v)

    props
    |> Props.tryFind PKey.TimeField.IsShortFormat
    |> Option.iter (fun v -> view.IsShortFormat <- v)

    props
    |> Props.tryFind PKey.TimeField.Value
    |> Option.iter (fun v -> view.Value <- v)

    // Events
    terminalElement.TrySetEventHandler(PKey.TimeField.ValueChanged, view.ValueChanged)

    terminalElement.TrySetEventHandler(PKey.TimeField.ValueChanging, view.ValueChanging)

  override _.RemoveProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.RemoveProps(terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> TimeField

    // Properties
    props
    |> Props.tryFind PKey.TimeField.InsertionPoint
    |> Option.iter (fun _ ->
        view.InsertionPoint <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.TimeField.IsShortFormat
    |> Option.iter (fun _ ->
        view.IsShortFormat <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.TimeField.Value
    |> Option.iter (fun _ ->
        view.Value <- Unchecked.defaultof<_>)

    // Events
    terminalElement.TryRemoveEventHandler PKey.TimeField.ValueChanged
    terminalElement.TryRemoveEventHandler PKey.TimeField.ValueChanging

  interface ITerminalElement

type internal TreeViewTerminalElement<'T when 'T: not struct>(props: Props) =
  inherit ViewTerminalElement(props)

  override _.Name = "TreeView`1"

  override _.NewView() = new TreeView<'T>()

  override _.SetAsChildOfParentView = true

  override _.SetProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.SetProps(terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> TreeView<'T>

    // Properties
    props
    |> Props.tryFind PKey.TreeView<'T>.AllowLetterBasedNavigation
    |> Option.iter (fun v -> view.AllowLetterBasedNavigation <- v)

    props
    |> Props.tryFind PKey.TreeView<'T>.AspectGetter
    |> Option.iter (fun v -> view.AspectGetter <- v)

    props
    |> Props.tryFind PKey.TreeView<'T>.ColorGetter
    |> Option.iter (fun v -> view.ColorGetter <- v)

    props
    |> Props.tryFind PKey.TreeView<'T>.MaxDepth
    |> Option.iter (fun v -> view.MaxDepth <- v)

    props
    |> Props.tryFind PKey.TreeView<'T>.MultiSelect
    |> Option.iter (fun v -> view.MultiSelect <- v)

    props
    |> Props.tryFind PKey.TreeView<'T>.ObjectActivationButton
    |> Option.iter (fun v -> view.ObjectActivationButton <- v)

    props
    |> Props.tryFind PKey.TreeView<'T>.ObjectActivationKey
    |> Option.iter (fun v -> view.ObjectActivationKey <- v)

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
    terminalElement.TrySetEventHandler(PKey.TreeView<'T>.DrawLine, view.DrawLine)

    terminalElement.TrySetEventHandler(PKey.TreeView<'T>.ObjectActivated, view.ObjectActivated)

    terminalElement.TrySetEventHandler(PKey.TreeView<'T>.SelectionChanged, view.SelectionChanged)

  override _.RemoveProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.RemoveProps(terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> TreeView<'T>

    // Properties
    props
    |> Props.tryFind PKey.TreeView<'T>.AllowLetterBasedNavigation
    |> Option.iter (fun _ ->
        view.AllowLetterBasedNavigation <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.TreeView<'T>.AspectGetter
    |> Option.iter (fun _ ->
        view.AspectGetter <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.TreeView<'T>.ColorGetter
    |> Option.iter (fun _ ->
        view.ColorGetter <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.TreeView<'T>.MaxDepth
    |> Option.iter (fun _ ->
        view.MaxDepth <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.TreeView<'T>.MultiSelect
    |> Option.iter (fun _ ->
        view.MultiSelect <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.TreeView<'T>.ObjectActivationButton
    |> Option.iter (fun _ ->
        view.ObjectActivationButton <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.TreeView<'T>.ObjectActivationKey
    |> Option.iter (fun _ ->
        view.ObjectActivationKey <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.TreeView<'T>.ScrollOffsetHorizontal
    |> Option.iter (fun _ ->
        view.ScrollOffsetHorizontal <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.TreeView<'T>.ScrollOffsetVertical
    |> Option.iter (fun _ ->
        view.ScrollOffsetVertical <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.TreeView<'T>.SelectedObject
    |> Option.iter (fun _ ->
        view.SelectedObject <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.TreeView<'T>.Style
    |> Option.iter (fun _ ->
        view.Style <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.TreeView<'T>.TreeBuilder
    |> Option.iter (fun _ ->
        view.TreeBuilder <- Unchecked.defaultof<_>)

    // Events
    terminalElement.TryRemoveEventHandler PKey.TreeView<'T>.DrawLine
    terminalElement.TryRemoveEventHandler PKey.TreeView<'T>.ObjectActivated
    terminalElement.TryRemoveEventHandler PKey.TreeView<'T>.SelectionChanged

  interface ITerminalElement

type internal TreeViewTerminalElement(props: Props) =
  inherit TreeViewTerminalElement<Terminal.Gui.Views.ITreeNode>(props)

  override _.Name = "TreeView"

  override _.NewView() = new TreeView()

  override _.SetAsChildOfParentView = true


  interface ITerminalElement

type internal WindowTerminalElement(props: Props) =
  inherit RunnableTerminalElement(props)

  override _.Name = "Window"

  override _.NewView() = new Window()

  override _.SetAsChildOfParentView = true


  interface ITerminalElement

type internal WizardTerminalElement(props: Props) =
  inherit DialogTerminalElement(props)

  override _.Name = "Wizard"

  override _.NewView() = new Wizard()

  override _.SetAsChildOfParentView = true

  override this.SubElements_PropKeys =
    [
      SubElementPropKey.from PKey.Wizard.CurrentStep_element
    ]
    |> List.append base.SubElements_PropKeys

  override _.SetProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.SetProps(terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> Wizard

    // Properties
    props
    |> Props.tryFind PKey.Wizard.CurrentStep
    |> Option.iter (fun v -> view.CurrentStep <- v)

    // Events
    terminalElement.TrySetEventHandler(PKey.Wizard.MovingBack, view.MovingBack)

    terminalElement.TrySetEventHandler(PKey.Wizard.MovingNext, view.MovingNext)

    terminalElement.TrySetEventHandler(PKey.Wizard.StepChanged, view.StepChanged)

    terminalElement.TrySetEventHandler(PKey.Wizard.StepChanging, view.StepChanging)

  override _.RemoveProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.RemoveProps(terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> Wizard

    // Properties
    props
    |> Props.tryFind PKey.Wizard.CurrentStep
    |> Option.iter (fun _ ->
        view.CurrentStep <- Unchecked.defaultof<_>)

    // Events
    terminalElement.TryRemoveEventHandler PKey.Wizard.MovingBack
    terminalElement.TryRemoveEventHandler PKey.Wizard.MovingNext
    terminalElement.TryRemoveEventHandler PKey.Wizard.StepChanged
    terminalElement.TryRemoveEventHandler PKey.Wizard.StepChanging

  interface ITerminalElement

type internal WizardStepTerminalElement(props: Props) =
  inherit ViewTerminalElement(props)

  override _.Name = "WizardStep"

  override _.NewView() = new WizardStep()

  override _.SetAsChildOfParentView = true

  override _.SetProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.SetProps(terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
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

  override _.RemoveProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.RemoveProps(terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> WizardStep

    // Properties
    props
    |> Props.tryFind PKey.WizardStep.BackButtonText
    |> Option.iter (fun _ ->
        view.BackButtonText <- "")

    props
    |> Props.tryFind PKey.WizardStep.HelpText
    |> Option.iter (fun _ ->
        view.HelpText <- "")

    props
    |> Props.tryFind PKey.WizardStep.NextButtonText
    |> Option.iter (fun _ ->
        view.NextButtonText <- "")


  interface IWizardStepTerminalElement

  interface ITerminalElement
