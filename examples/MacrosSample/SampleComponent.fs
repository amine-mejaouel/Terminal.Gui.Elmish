module SampleComponent

open System
open Terminal.Gui.Elmish

type private Msg =
  | ChangeText

type private ComponentModel = { Text: string }

type Props() =
  // TODO-ELMISH-COMPONENT: raw properties should not be visible.
  member val y_value: TPos option = None with get, set
  member this.y (pos: TPos) = this.y_value <- Some pos

let _component (set: Props -> unit) =

  let props = Props()
  set props

  let init () = { Text = "Hello" }
  let update cmd model =
    match cmd with
    | ChangeText ->
       { model with Text = Guid.NewGuid().ToString().Substring(0, 8) }
  let view model dispatch =
    View.label (fun p ->
      p.text model.Text
      props.y_value |> Option.iter p.y
      p.mouseClick (fun _ -> dispatch ChangeText)
    )

  ElmishTerminal.mkSimple init update view
  |> ElmishTerminal.runComponent

