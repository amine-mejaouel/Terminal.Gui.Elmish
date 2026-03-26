module internal Terminal.Gui.Elmish.Tests.TestComponent

open Terminal.Gui.Elmish

type Msg = | Increment

type ComponentModel = { Counter: int }

type PKey =
  | Text = 0

type IProps =
  abstract member text: string -> unit

type IPropsReader =
  abstract member text: string option with get

type private Props() =
  inherit ComponentProps("TestComponent")

  interface IProps with
    member this.text value = this[int PKey.Text] <- value

  interface IPropsReader with
    member this.text = this.TryGetPropValue(int PKey.Text)

let create (set: IProps -> unit) =

  let props = Props()
  set props
  let propsReader = props :> IPropsReader

  let init () = { Counter = 0 }

  let update msg model =
    match msg with
    | Increment ->
      { model with
          Counter = model.Counter + 1 }

  let view model dispatch =
    View.Window(fun p ->
      p.Title(propsReader.text |> Option.defaultValue "Default")

      p.Children
        [ View.Label(fun p -> p.Text(sprintf "Counter: %d" model.Counter))
          View.Button(fun p ->
            p.Text "Increment"
            p.Accepting(fun _ -> dispatch (TerminalMsg.ofMsg Increment))) ])

  ElmishTester.mkTestableComponent props init update view
