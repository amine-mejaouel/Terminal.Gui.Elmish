module TreeDiffUpdateTesterComponent

open Terminal.Gui.Drawing
open Terminal.Gui.Elmish
open Terminal.Gui.ViewBase

type DisplayedButton =
  | Button2
  | Button1

type private Msg = ChangeDisplayedButton of DisplayedButton

type private ComponentModel = { DisplayedView: DisplayedButton }

type PKey =
  | Y = 0

type IProps =
  abstract member y: TPos -> unit

type IPropsReader =
  abstract member y: TPos option with get

type private Props() =
  inherit ComponentProps("TreeDiffUpdateTesterComponent")

  interface IProps with
    member this.y pos = this[int PKey.Y] <- pos

  interface IPropsReader with
    member this.y = this.TryGetPropValue(int PKey.Y)

let _component (set: IProps -> unit) =

  let props = Props()
  set props
  let propsReader = props :> IPropsReader

  let init () = { DisplayedView = Button1 }

  let update cmd model =
    match cmd with
    | ChangeDisplayedButton view -> { model with DisplayedView = view }

  let view model dispatch =
    View.Runnable(fun (p: RunnableProps) ->
      propsReader.y |> Option.iter p.Y
      p.BorderStyle LineStyle.Dashed

      p.Children
        [ let first =
            if model.DisplayedView = Button1 then
              View.Button(fun p ->
                p.Text "Button 1: Click to test changing the Terminal Element !"

                p.Accepting(fun e -> dispatch (TerminalMsg.ofMsg (ChangeDisplayedButton Button2))))
            else
              View.Button(fun p ->
                p.Text "Button 2: Click to test changing the Terminal Element !"
                p.ShadowStyle ShadowStyle.Transparent

                p.Accepting(fun e -> dispatch (TerminalMsg.ofMsg (ChangeDisplayedButton Button1))))

          let second =
            View.Label(fun p ->
              p.Text "I am a static label below the first element."
              p.Y(TPos.Bottom first))

          let third =
            View.Label(fun p ->
              p.Text "I am another static label below the second element."
              p.Y(TPos.Bottom second))

          first
          second
          third ])

  ElmishTerminal.mkSimpleComponent props init update view
