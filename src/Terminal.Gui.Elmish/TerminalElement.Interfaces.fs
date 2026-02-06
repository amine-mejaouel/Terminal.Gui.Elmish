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
    terminalElement.TryRemoveEventHandler PKey.IOrientationInterface.OrientationChanged

    terminalElement.TryRemoveEventHandler PKey.IOrientationInterface.OrientationChanging

  static member setProps (terminalElement: TerminalElement) (view: IOrientation) (props: Props) =
    // Properties
    props
    |> Props.tryFind PKey.IOrientationInterface.Orientation
    |> Option.iter (fun v -> view.Orientation <- v)

    // Events
    terminalElement.TrySetEventHandler(PKey.IOrientationInterface.OrientationChanged, view.OrientationChanged)

    terminalElement.TrySetEventHandler(PKey.IOrientationInterface.OrientationChanging, view.OrientationChanging)

