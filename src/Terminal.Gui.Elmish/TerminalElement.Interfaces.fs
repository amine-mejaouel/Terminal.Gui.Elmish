namespace Terminal.Gui.Elmish

open Terminal.Gui.ViewBase

// OrientationInterface - used by elements that implement Terminal.Gui.ViewBase.IOrientation
type internal OrientationInterface =
  static member removeProps (terminalElement: TerminalElement) (view: IOrientation) (props: Props) =
    // Properties
    props
    |> Props.tryFind PKey.orientationInterface.orientation
    |> Option.iter (fun _ -> view.Orientation <- Unchecked.defaultof<_>)

    // Events
    terminalElement.tryRemoveEventHandler PKey.orientationInterface.orientationChanged

    terminalElement.tryRemoveEventHandler PKey.orientationInterface.orientationChanging

  static member setProps (terminalElement: TerminalElement) (view: IOrientation) (props: Props) =
    // Properties
    props
    |> Props.tryFind PKey.orientationInterface.orientation
    |> Option.iter (fun v -> view.Orientation <- v)

    // Events
    terminalElement.trySetEventHandler(PKey.orientationInterface.orientationChanged, view.OrientationChanged)

    terminalElement.trySetEventHandler(PKey.orientationInterface.orientationChanging, view.OrientationChanging)

