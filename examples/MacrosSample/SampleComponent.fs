module SampleComponent

open System
open Terminal.Gui.Elmish

type private Msg =
  | ChangeText

type private ComponentModel = { Text: string }

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

  let init () = { Text = "Hello" }
  let update cmd model =
    match cmd with
    | ChangeText ->
      { model with Text = Guid.NewGuid().ToString().Substring(0, 8) }
  let view model dispatch =
    View.label (fun p ->
      p.Text model.Text
      props.y_value |> Option.iter p.Y
      p.Activating (fun _ -> dispatch ChangeText)
    )

  ElmishTerminal.mkSimple init update view
  |> ElmishTerminal.runComponent

