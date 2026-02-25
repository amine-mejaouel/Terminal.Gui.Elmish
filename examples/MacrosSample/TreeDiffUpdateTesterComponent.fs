module TreeDiffUpdateTesterComponent

open Terminal.Gui.Drawing
open Terminal.Gui.Elmish
open Terminal.Gui.Input
open Terminal.Gui.ViewBase

type DisplayedButton =
  | Button2
  | Button1

type private Msg =
  | ChangeDisplayedButton of DisplayedButton

type private ComponentModel = { DisplayedView: DisplayedButton; }

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

  let init () = { DisplayedView = Button1 }
  let update cmd model =
    match cmd with
    | ChangeDisplayedButton view ->
       { model with DisplayedView = view }

  let view model dispatch =
    View.Runnable (fun (p: RunnableProps) ->
      props.y_value |> Option.iter p.Y
      p.BorderStyle LineStyle.Dashed
      p.Children [
        let first =
          if model.DisplayedView = Button1 then
            View.Button (fun p ->
              p.Text "Button 1: Click to test changing the Terminal Element !"
              p.Activating (fun e ->
                match e.Context.Binding with
                | :? MouseBinding as mb when mb.MouseEvent.Flags.HasFlag MouseFlags.LeftButtonClicked ->
                  dispatch (ChangeDisplayedButton Button2)
                  e.Handled <- true
                | _ -> ()
              )
            )
          else
            View.Button (fun p ->
              p.Text "Button 2: Click to test changing the Terminal Element !"
              p.ShadowStyle ShadowStyle.Transparent
              p.Activating (fun e ->
                match e.Context.Binding with
                | :? MouseBinding as mb when mb.MouseEvent.Flags.HasFlag MouseFlags.LeftButtonClicked ->
                  dispatch (ChangeDisplayedButton Button1)
                  e.Handled <- true
                | _ -> ()
              )
            )

        let second =
          View.Label (fun p ->
            p.Text "I am a static label below the first element."
            p.Y (TPos.Bottom first))

        let third =
          View.Label (fun p ->
            p.Text "I am another static label below the second element."
            p.Y (TPos.Bottom second))

        first
        second
        third
      ])

  ElmishTerminal.mkSimpleComponent "TreeDiffUpdateTesterComponent" init update view
