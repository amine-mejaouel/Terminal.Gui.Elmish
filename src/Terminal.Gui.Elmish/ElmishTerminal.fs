module Terminal.Gui.Elmish.ElmishTerminal

open System
open System.Threading.Tasks
open Elmish
open Terminal.Gui.App
open Terminal.Gui.Views

type internal OuterModel<'model> = {
  mutable CurrentTreeState: IInternalTerminalElement option
  TopLevel: TaskCompletionSource<Toplevel>
  Termination: TaskCompletionSource
  InnerModel: 'model
}

module OuterModel =
  let internal wrapInit (init : 'arg -> 'model * Cmd<'msg>) =
    fun (arg: 'arg) ->
      let innerModel, cmd = init arg
      let internalModel = {
        CurrentTreeState = None
        TopLevel = TaskCompletionSource<Toplevel>()
        Termination = TaskCompletionSource()
        InnerModel = innerModel
      }
      internalModel, cmd

  let internal wrapUpdate (update: 'msg -> 'model -> 'model * Cmd<'msg>) : 'msg -> OuterModel<'model> -> OuterModel<'model> * Cmd<'msg> =
    fun (msg: 'msg) (model: OuterModel<'model>) ->
      let innerModel, cmd = update msg model.InnerModel
      { model with InnerModel = innerModel }, cmd

  let internal wrapView (view: 'model -> Dispatch<'msg> -> ITerminalElement) : OuterModel<'model> -> Dispatch<'msg> -> ITerminalElement =
    fun (model: OuterModel<'model>) (dispatch: Dispatch<'msg>) ->
      view model.InnerModel dispatch

  let internal wrapSimpleInit (init : 'arg -> 'model) =
    fun (arg: 'arg) ->
      let innerModel = init arg
      let internalModel = {
        CurrentTreeState = None
        TopLevel = TaskCompletionSource<Toplevel>()
        Termination = TaskCompletionSource()
        InnerModel = innerModel
      }
      internalModel

  let internal wrapSimpleUpdate (update: 'msg -> 'model -> 'model) : 'msg -> OuterModel<'model> -> OuterModel<'model> =
    fun (msg: 'msg) (model: OuterModel<'model>) ->
      let innerModel = update msg model.InnerModel
      { model with InnerModel = innerModel }

  let internal wrapSubscribe (subscribe: 'model -> _) : OuterModel<'model> -> _ =
    fun outerModel ->
      subscribe outerModel.InnerModel

type ElmishTerminalProgram<'arg, 'model, 'msg, 'view> = private ElmishTerminalProgram of Program<'arg, OuterModel<'model>, 'msg, 'view>

let mutable unitTestMode = false

let private setState (view: OuterModel<'model> -> Dispatch<'cmd> -> ITerminalElement) (model: OuterModel<'model>) dispatch =

  let nextTreeState =
    match model.CurrentTreeState with
    | None ->

      let startState =
        view model dispatch :?> IInternalTerminalElement

      startState.initializeTree None

      match startState.view with
      | null -> failwith ("error state not initialized")
      | topElement ->
        match topElement with
        | :? Toplevel as tl ->
            model.TopLevel.SetResult(tl)
        | _ -> failwith ("first element must be a toplevel!")

      startState

    | Some currentState ->
      let nextTreeState =
        view model dispatch :?> IInternalTerminalElement

      Differ.update currentState nextTreeState
      nextTreeState

  model.CurrentTreeState <- Some nextTreeState
  nextTreeState.layout ()

  ()

let private terminate model =
  if not unitTestMode then
    model.TopLevel.Task.Result.Dispose()
    ApplicationImpl.Instance.Shutdown()
    model.Termination.SetResult()

let mkProgram (init: 'arg -> 'model * Cmd<'msg>) (update: 'msg -> 'model -> 'model * Cmd<'msg>) (view: 'model -> Dispatch<'msg> -> ITerminalElement) =
  Program.mkProgram (OuterModel.wrapInit init) (OuterModel.wrapUpdate update) (OuterModel.wrapView view)
  |> Program.withSetState (setState (OuterModel.wrapView view))
  |> ElmishTerminalProgram

let mkSimple (init: 'arg -> 'model) (update: 'cmd -> 'model -> 'model) (view: 'model -> Dispatch<'cmd> -> ITerminalElement) =
  Program.mkSimple (OuterModel.wrapSimpleInit init) (OuterModel.wrapSimpleUpdate update) (OuterModel.wrapView view)
  |> Program.withSetState (setState (OuterModel.wrapView view))
  |> ElmishTerminalProgram

let withSubscription (subscribe: 'model -> Sub<_>) (ElmishTerminalProgram program) =
  program
  |> Program.withSubscription (OuterModel.wrapSubscribe subscribe)
  |> ElmishTerminalProgram

let withTermination predicate (ElmishTerminalProgram program) =
  program
  |> Program.withTermination predicate terminate
  |> ElmishTerminalProgram

let run (ElmishTerminalProgram program) =

    let waitForStart = TaskCompletionSource()
    let mutable waitForTermination = null

    let run (model: OuterModel<_>) =
      let start dispatch =
          task {
            let! toplevel = model.TopLevel.Task
            ApplicationImpl.Instance.Run(toplevel)
            waitForStart.SetResult()
            waitForTermination <- model.Termination
          } |> ignore
          { new IDisposable with
            // TODO: implement disposal
              member _.Dispose() = () }
      start

    let subscribe model =
        [ ["run"], run model ]

    if not unitTestMode then
        ApplicationImpl.Instance.Init()

    program
    |> Program.withSubscription subscribe
    |> Program.run

    waitForStart.Task.GetAwaiter().GetResult()
    waitForTermination.Task.GetAwaiter().GetResult()

