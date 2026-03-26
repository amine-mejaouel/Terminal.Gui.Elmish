module SampleComponent

open System
open Terminal.Gui.Elmish

type private Msg = | ChangeText

type private ComponentModel = { Text: string }

type PKey =
  | Y = 0

type IProps =
  abstract member y: TPos -> unit

type IPropsReader =
  abstract member y: TPos with get

type private Props() =
  inherit ComponentProps("SampleComponent")

  interface IProps with
    member this.y pos = this.Props[int PKey.Y] <- pos

  interface IPropsReader with
    member this.y = this.Props.[int PKey.Y] :?> TPos

let _component (set: IProps -> unit) =

  let props = Props()
  set props
  let propsReader = props :> IPropsReader

  let init () = { Text = "Hello" }

  let update cmd model =
    match cmd with
    | ChangeText ->
      { model with
          Text = Guid.NewGuid().ToString().Substring(0, 8) }

  let view model dispatch =
    View.Label(fun p ->
      p.Text model.Text
      p.Y propsReader.y
      p.Accepting(fun _ -> dispatch (TerminalMsg.ofMsg ChangeText)))

  ElmishTerminal.mkSimpleComponent props init update view
