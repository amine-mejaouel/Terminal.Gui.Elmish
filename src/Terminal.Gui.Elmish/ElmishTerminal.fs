module Terminal.Gui.Elmish.ElmishTerminal

open Elmish
open Terminal.Gui.App
open Terminal.Gui.Views

let mutable unitTestMode = false

let mutable private currentTreeState:IInternalTerminalElement option = None
let mutable toplevel: Toplevel = null
let mutable private runState: RunState = null

let private setState (view: 'model -> Dispatch<'cmd> -> ITerminalElement) (model: 'model) dispatch =

    let nextTreeState =
        match currentTreeState with
        | None ->
            if not unitTestMode then
                Application.Init()

            let startState = view model dispatch :?> IInternalTerminalElement

            startState.initializeTree None

            match startState.view with
            | null ->
                failwith ("error state not initialized")
            | topElement ->
                match topElement with
                | :? Toplevel as tl ->
                    if not unitTestMode then
                        toplevel <- tl
                        runState <- Application.Begin(tl)
                        Application.RunIteration(&runState, true) |> ignore
                    else
                        toplevel <- tl
                | _ ->
                    failwith("first element must be a toplevel!")

            startState

        | Some currentState ->
            let nextTreeState = view model dispatch :?> IInternalTerminalElement
            Differ.update currentState nextTreeState
            nextTreeState

    currentTreeState <- Some nextTreeState
    nextTreeState.layout()

    ()

let private terminate model =
    if not unitTestMode then
        Application.End(runState)
        toplevel.Dispose()
        Application.Shutdown()

let mkProgram (init: 'arg -> 'model * Cmd<'msg>) update (view: 'model -> Dispatch<'msg> -> ITerminalElement) =
    Program.mkProgram init update view
    |> Program.withSetState (setState view)

let mkSimple (init: 'arg -> 'model) (update: 'cmd -> 'model -> 'model) (view: 'model -> Dispatch<'cmd> -> ITerminalElement) =
    Program.mkSimple init update view
    |> Program.withSetState (setState view)

let withTermination predicate =
    Program.withTermination predicate terminate

let run program =
    if not unitTestMode then
        Application.Init()

    Program.run program

    if not unitTestMode then
        Application.RunLoop(runState)
