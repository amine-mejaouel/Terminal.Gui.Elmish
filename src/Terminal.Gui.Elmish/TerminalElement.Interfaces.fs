namespace Terminal.Gui.Elmish

open Terminal.Gui.ViewBase

// OrientationInterface - used by elements that implement Terminal.Gui.ViewBase.IOrientation
type internal OrientationInterface =
  static member removeProps (terminalElement: TerminalElement) (view: IOrientation) (props: Props) =
    // Properties
    props
    |> Props.tryFind PKey.IOrientationInterface.Orientation
    |> Option.iter (fun _ -> view.Orientation <- Unchecked.defaultof<_>)

    // Events
    terminalElement.tryRemoveEventHandler PKey.IOrientationInterface.OrientationChanged

    terminalElement.tryRemoveEventHandler PKey.IOrientationInterface.OrientationChanging

  static member setProps (terminalElement: TerminalElement) (view: IOrientation) (props: Props) =
    // Properties
    props
    |> Props.tryFind PKey.IOrientationInterface.Orientation
    |> Option.iter (fun v -> view.Orientation <- v)

    // Events
    terminalElement.trySetEventHandler(PKey.IOrientationInterface.OrientationChanged, view.OrientationChanged)

    terminalElement.trySetEventHandler(PKey.IOrientationInterface.OrientationChanging, view.OrientationChanging)

