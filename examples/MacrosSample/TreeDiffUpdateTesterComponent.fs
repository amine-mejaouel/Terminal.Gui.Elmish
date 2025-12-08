module TreeDiffUpdateTesterComponent

open Terminal.Gui.Elmish

type DisplayedView =
  | Label
  | Button

type private Msg =
  | ChangeView of DisplayedView

type private ComponentModel = { DisplayedView: DisplayedView; }

type IProps =
  abstract member y: TPos -> unit

type private Props() =
  member val y_value: TPos option = None with get, set
  member this.y (pos: TPos) = this.y_value <- Some pos

  interface IProps with
    member this.y pos = this.y pos

let _component (set: IProps -> unit) =

  let props = Props()
  set props

  let init () = { DisplayedView = Label }
  let update cmd model =
    match cmd with
    | ChangeView view ->
       { model with DisplayedView = view }

  let view model dispatch =
    if model.DisplayedView = Button then
      View.button (fun p ->
        p.text "Click to test changing the Terminal Element type!"
        props.y_value |> Option.iter p.y
        p.mouseClick (fun _ -> dispatch (ChangeView Label))
      )
    else
      View.label (fun p ->
        p.text "Click to test changing the Terminal Element type!"
        props.y_value |> Option.iter p.y
        p.mouseClick (fun _ -> dispatch (ChangeView Button))
      )

  ElmishTerminal.mkSimple init update view
  |> ElmishTerminal.runComponent
