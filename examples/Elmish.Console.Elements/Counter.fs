module Counter

open Terminal.Gui
open Terminal.Gui.Elmish
open System

type Model = {
    Counter:int
    IsSpinning: bool
}

type Msg =
    | Increment
    | Decrement
    | Reset
    | StartSpin
    | StopSpin
    | Spinned

let init () : Model * Cmd<Msg> =
    let model = {
        Counter = 0
        IsSpinning = false
    }
    model, Cmd.none


module Commands =
    let startSpinning isSpinning =
        fun dispatch ->
            async {
                do! Async.Sleep 20
                if isSpinning then
                    dispatch Increment
                    dispatch Spinned
            }
            |> Async.StartImmediate
        |> Cmd.ofSub

let update (msg:Msg) (model:Model) =
    match msg with
    | Increment ->
        {model with Counter = model.Counter + 1}, Cmd.none
    | Decrement ->
        {model with Counter = model.Counter - 1}, Cmd.none
    | Reset ->
        {model with Counter = 0}, Cmd.none
    | StartSpin ->
        {model with IsSpinning = true}, Commands.startSpinning true
    | StopSpin ->
        {model with IsSpinning = false}, Cmd.none
    | Spinned ->
        model, Commands.startSpinning model.IsSpinning
        


let view (model:Model) (dispatch:Msg->unit) =
    [
        View.label [
            prop.position.x.center
            prop.position.y.absolute 1
            prop.alignment.center
            prop.color (Color.BrightYellow, Color.Green)
            labelProps.text "'F#ncy' Counter!"
        ] 

        View.button [
            prop.position.x.center
            prop.position.y.absolute 5
            buttonProps.text "Up"
            prop.accept (fun ev -> dispatch Increment)
        ] 

        View.label [
            let c = (model.Counter |> float) / 100.0
            let x = (16.0 * Math.Cos(c)) |> int 
            let y = (8.0 * Math.Sin(c)) |> int

            prop.position.x.absolute (x + 20)
            prop.position.y.absolute (y + 10)
            prop.alignment.center
            prop.color (Color.Magenta, Color.BrightYellow)
            labelProps.text $"The Count of 'Fancyness' is {model.Counter}"
        ] 


        View.button [
            prop.position.x.center
            prop.position.y.absolute 7
            buttonProps.text "Down"
            prop.accept (fun ev -> dispatch Decrement)
        ] 

        View.button [
            prop.position.x.center
            prop.position.y.absolute 9
            buttonProps.text "Start Spinning"
            prop.accept (fun ev -> dispatch StartSpin)
        ] 

        View.button [
            prop.position.x.center
            prop.position.y.absolute 11
            buttonProps.text "Stop Spinning"
            prop.accept (fun ev -> dispatch StopSpin)
        ] 

        View.button [
            prop.position.x.center
            prop.position.y.absolute 13
            buttonProps.text "Reset"
            prop.accept (fun ev -> dispatch Reset)
        ] 
    ]

