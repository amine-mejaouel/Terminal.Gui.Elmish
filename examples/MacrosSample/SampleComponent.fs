module SampleComponent

open System
open Terminal.Gui.Elmish

type Msg =
  | ChangeText

type ComponentModel = { Text: string }

let _component =
  let init () = { Text = "Hello" }
  let update cmd model =
    match cmd with
    | ChangeText ->
       { model with Text = Guid.NewGuid().ToString().Substring(0, 8) }
  let view model dispatch =
    View.label (fun p ->
      p.text model.Text
      p.mouseClick (fun _ -> dispatch ChangeText)
    )

  ElmishComponent.mkSimple init update view
  |> ElmishComponent.run

