namespace Terminal.Gui.Elmish

type TerminalMsg<'a> =
  | Terminate
  | Msg of 'a

[<RequireQualifiedAccess>]
module TerminalMsg =
  let ofMsg msg = TerminalMsg.Msg msg

[<RequireQualifiedAccess>]
module ElmishTerminal =

  open System
  open System.Threading.Tasks
  open Elmish
  open Terminal.Gui.App
  open Terminal.Gui.ViewBase

  [<RequireQualifiedAccess>]
  type internal RootView =
    | Runnable of Runnable
    | View of View

  type internal InternalModel<'model> = {
    mutable CurrentTreeState: IInternalTerminalElement option
    mutable Application: IApplication
    RootView: TaskCompletionSource<RootView>
    Termination: TaskCompletionSource
    /// Elmish model provided to the Program by the library caller.
    ClientModel: 'model
  }

  module OuterModel =
    let internal wrapInit (init: 'arg -> 'model * Cmd<'msg>) =
      fun (arg: 'arg) ->
        let innerModel, cmd = init arg

        let internalModel = {
          Application = Application.Create()
          CurrentTreeState = None
          RootView = TaskCompletionSource<RootView>()
          Termination = TaskCompletionSource()
          ClientModel = innerModel
        }

        internalModel, cmd

    let internal wrapUpdate (update: 'msg -> 'model -> 'model * Cmd<'msg>) : 'msg -> InternalModel<'model> -> InternalModel<'model> * Cmd<'msg> =
      fun (msg: 'msg) (model: InternalModel<'model>) ->
        let innerModel, cmd =
          update msg model.ClientModel

        { model with ClientModel = innerModel }, cmd

    let internal wrapView (view: 'model -> Dispatch<'msg> -> ITerminalElement) : InternalModel<'model> -> Dispatch<'msg> -> ITerminalElement =
      fun (model: InternalModel<'model>) (dispatch: Dispatch<'msg>) -> view model.ClientModel dispatch

    let internal wrapSimpleInit (init: 'arg -> 'model) =
      fun (arg: 'arg) ->
        let innerModel = init arg

        let internalModel = {
          Application = Application.Create()
          CurrentTreeState = None
          RootView = TaskCompletionSource<RootView>()
          Termination = TaskCompletionSource()
          ClientModel = innerModel
        }

        internalModel

    let internal wrapSimpleUpdate (update: 'msg -> 'model -> 'model) : 'msg -> InternalModel<'model> -> InternalModel<'model> =
      fun (msg: 'msg) (model: InternalModel<'model>) ->
        let innerModel = update msg model.ClientModel
        { model with ClientModel = innerModel }

    let internal wrapSubscribe (subscribe: 'model -> _) : InternalModel<'model> -> _ =
      fun outerModel -> subscribe outerModel.ClientModel

  type ElmishTerminalProgram<'arg, 'model, 'msg, 'view> = private ElmishTerminalProgram of Program<'arg, InternalModel<'model>, 'msg, 'view>

  let mutable unitTestMode = false

  let private setState (view: InternalModel<'model> -> Dispatch<'cmd> -> ITerminalElement) (model: InternalModel<'model>) dispatch =

    let nextTreeState =
      match model.CurrentTreeState with
      | None ->

        let startState =
          view model dispatch :?> IInternalTerminalElement

        startState.initializeTree None

        match startState.view with
        | null -> failwith "error state not initialized"
        | :? Runnable as r -> model.RootView.SetResult(RootView.Runnable r)
        | view -> model.RootView.SetResult(RootView.View view)

        startState

      | Some currentState ->
        let nextTreeState =
          view model dispatch :?> IInternalTerminalElement

        Differ.update currentState nextTreeState
        currentState.Dispose()
        nextTreeState

    model.CurrentTreeState <- Some nextTreeState

    ()

  let private terminate model =
    if not unitTestMode then
      match model.RootView.Task.Result with
      | RootView.Runnable r ->
        r.Dispose()
        model.Application.RequestStop()
      | RootView.View _ ->
        ()

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
    |> ElmishTerminalProgram

  let runTerminal (ElmishTerminalProgram program) =

    let waitForStart = TaskCompletionSource()
    let running = TaskCompletionSource()
    let mutable waitForTermination = null

    let application = Application.Create()

    let runTerminal (model: InternalModel<_>) =
      let start dispatch =
        let rootView = model.RootView.Task.GetAwaiter().GetResult()
        match rootView with
        | RootView.Runnable runnable ->
          if not unitTestMode then
            Task.Run(fun () ->
              (
                try
                  model.Application <- application
                  model.Application.Init() |> ignore
                  model.Application.Run(runnable) |> ignore
                  running.SetResult()
                with ex -> running.SetException ex
              )
              , TaskCreationOptions.LongRunning
            ) |> ignore

          waitForStart.SetResult()
          waitForTermination <- model.Termination
        | _ ->
          failwith (
            "`run` is meant to be used for Terminal Elmish loop. " +
            "For Terminal components with separate Elmish loop, use `runComponent`.")

        { new IDisposable with member _.Dispose() = () }

      start

    let subscribe model = [ [ "runTerminal" ], runTerminal model ]

    program
    |> Program.withSubscription subscribe
    |> Program.withTermination (fun msg -> msg = Terminate) terminate
    |> Program.runWith application

    waitForStart.Task.GetAwaiter().GetResult()
    Task.WhenAll(running.Task, waitForTermination.Task).GetAwaiter().GetResult()

  let runComponent (ElmishTerminalProgram program) : ITerminalElement =

    let waitForView = TaskCompletionSource()
    let mutable view: ITerminalElement = Unchecked.defaultof<_>

    let runComponent (model: InternalModel<_>) =
      let start dispatch =
        (task {
          // On program startup, Wait for the Elmish loop to take care of creating the root view.
          let! _ = model.RootView.Task
          view <- model.CurrentTreeState.Value :> ITerminalElement
          waitForView.SetResult()
        }).GetAwaiter().GetResult()

        { new IDisposable with member _.Dispose() = () }

      start

    let subscribe model = [ [ "runComponent" ], runComponent model ]

    program
    |> Program.withSubscription subscribe
    |> Program.run

    waitForView.Task.GetAwaiter().GetResult()
    view
