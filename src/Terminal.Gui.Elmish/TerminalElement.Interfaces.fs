namespace Terminal.Gui.Elmish

open Terminal.Gui.ViewBase

// IMouseHeldDown - used by elements that implement Terminal.Gui.ViewBase.IMouseHeldDown
type internal MouseHeldDownInterface =
  static member removeProps (terminalElement: TerminalElement) (view: IMouseHeldDown) (props: Props) =
    // Events
    terminalElement.tryRemoveEventHandler PKey.mouseHeldDownInterface.mouseIsHeldDownTick

  static member setProps (terminalElement: TerminalElement) (view: IMouseHeldDown) (props: Props) =
    // Events
    terminalElement.trySetEventHandler(PKey.mouseHeldDownInterface.mouseIsHeldDownTick, view.MouseIsHeldDownTick)


// IOrientation - used by elements that implement Terminal.Gui.ViewBase.IOrientation
type internal OrientationInterface =
  static member removeProps (terminalElement: TerminalElement) (view: IOrientation) (props: Props) =
    // Properties
    props
    |> Props.tryFind PKey.orientationInterface.orientation
    |> Option.iter (fun _ -> view.Orientation <- Unchecked.defaultof<_>)

    // Events
    terminalElement.tryRemoveEventHandler PKey.orientationInterface.orientationChanging

    terminalElement.tryRemoveEventHandler PKey.orientationInterface.orientationChanged

  static member setProps (terminalElement: TerminalElement) (view: IOrientation) (props: Props) =
    // Properties
    props
    |> Props.tryFind PKey.orientationInterface.orientation
    |> Option.iter (fun v -> view.Orientation <- v)

    // Events
    terminalElement.trySetEventHandler(PKey.orientationInterface.orientationChanging, view.OrientationChanging)

    terminalElement.trySetEventHandler(PKey.orientationInterface.orientationChanged, view.OrientationChanged)

