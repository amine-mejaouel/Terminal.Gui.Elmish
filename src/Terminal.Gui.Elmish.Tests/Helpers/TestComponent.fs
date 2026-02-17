module Terminal.Gui.Elmish.Tests.TestComponent

open Terminal.Gui.Elmish

type private Msg =
  | Increment

type private ComponentModel = { Counter: int }

type IProps =
  abstract member text: string -> unit

type private Props() =
  member val text_value: string option = None with get, set
  member this.text (value: string) = this.text_value <- Some value

  interface IProps with
    member this.text value = this.text value

let _component (set: IProps -> unit) =

  let props = Props()
  set props

  let init () = { Counter = 0 }
  let update msg model =
    match msg with
    | Increment -> { model with Counter = model.Counter + 1 }

  let view model dispatch =
    View.Window (fun p ->
      p.Title (props.text_value |> Option.defaultValue "Default")
      p.Children [
        View.Label (fun p ->
          p.Text (sprintf "Counter: %d" model.Counter)
        )
        View.Button (fun p ->
          p.Text "Increment"
          p.Activating (fun _ -> dispatch Increment)
        )
      ]
    )

  ElmishTerminal.mkSimpleComponent "TestComponent" init update view
