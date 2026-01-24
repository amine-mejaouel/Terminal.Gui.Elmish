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
    View.runnable (fun (p: RunnableProps) ->
      props.y_value |> Option.iter p.Y
      p.Children [
        let first =
          if model.DisplayedView = Button then
            View.button (fun p ->
              p.Text "Click to test changing the Terminal Element type!"
              p.Activating (fun _ -> dispatch (ChangeView Label))
            )
          else
            View.label (fun p ->
              p.Text "Click to test changing the Terminal Element type!"
              p.Activating (fun _ -> dispatch (ChangeView Button))
            )

        let second =
          View.label (fun p ->
            p.Text "I am a static label below the first element."
            p.Y (TPos.Bottom first))

        let third =
          View.label (fun p ->
            p.Text "I am another static label below the second element."
            p.Y (TPos.Bottom second))

        first
        second
        third
      ])

  ElmishTerminal.mkSimple init update view
  |> ElmishTerminal.runComponent
