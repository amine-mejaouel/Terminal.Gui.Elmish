namespace Terminal.Gui.Elmish

open System
open System.Threading.Tasks
open Elmish
open Terminal.Gui.App
open Terminal.Gui.ViewBase
open Terminal.Gui.Views

type TerminalMsg<'a> =
  | Terminate
  | Msg of 'a

[<RequireQualifiedAccess>]
module TerminalMsg =
  let ofMsg msg = TerminalMsg.Msg msg

[<RequireQualifiedAccess>]
module ElmishTerminal =

  [<RequireQualifiedAccess>]
  type internal RootView =
    /// Application root view, there is one single instance of these in the application.
    | AppRootView of Runnable
    /// Elmish component root view, there can be multiple instances of these in the application.
    | ComponentRootView of View

  type internal InternalModel<'model> = {
    mutable CurrentTreeState: IInternalTerminalElement option
    mutable Application: IApplication
    Origin: Origin
    RootView: TaskCompletionSource<RootView>
    Termination: TaskCompletionSource
    /// Elmish model provided to the Program by the library caller.
    ClientModel: 'model
  }

  module internal OuterModel =
    let internal wrapInit origin (init: 'arg -> 'model * Cmd<'msg>) =
      fun (arg: 'arg) ->
        let innerModel, cmd = init arg

        let internalModel = {
          Application = Application.Create()
          CurrentTreeState = None
          Origin = origin
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

    let internal wrapSimpleInit origin (init: 'arg -> 'model) =
      fun (arg: 'arg) ->
        let innerModel = init arg

        let internalModel = {
          Application = Application.Create()
          CurrentTreeState = None
          Origin = origin
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

  type ElmishTerminalProgram<'arg, 'model, 'msg, 'view> = internal ElmishTerminalProgram of Program<'arg, InternalModel<'model>, 'msg, 'view>

  /// Elmish component TerminalElement interface.
  type internal IElmishComponentTE =
    abstract StartElmishLoop: Origin -> unit
    inherit ITerminalElement
    inherit IInternalTerminalElement

  let private setState (view: InternalModel<'model> -> Dispatch<'cmd> -> ITerminalElement) (model: InternalModel<'model>) dispatch =

    let nextTreeState =
      match model.CurrentTreeState with
      | None ->

        let startState =
          view model dispatch :?> IInternalTerminalElement

        startState.InitializeTree model.Origin

        match startState.View with
        | null -> failwith "error state not initialized"
        | :? Runnable as r -> model.RootView.SetResult(RootView.AppRootView r)
        | view -> model.RootView.SetResult(RootView.ComponentRootView view)

        startState

      | Some currentState ->
        let nextTreeState =
          view model dispatch :?> IInternalTerminalElement

        Differ.update currentState nextTreeState
        currentState.Dispose()
        nextTreeState

    model.CurrentTreeState <- Some nextTreeState

    ()

  let internal terminate model =
    match model.RootView.Task.Result with
    | RootView.AppRootView r ->
      r.Dispose()
      model.Application.RequestStop()
    | RootView.ComponentRootView _ ->
      ()

    model.Termination.SetResult()

  /// <summary>
  /// <para>Wrapper that elmish components should use to expose themselves as IInternalTerminalElement.</para>
  /// <para>As the Elmish component handles its own initialization and children management in his separate Elmish loop,
  /// this wrapper will hide these aspects to the outside world. Thus preventing double initialization or double children management.</para>
  /// </summary>
  type internal ElmishComponentTE<'model, 'msg, 'view>(init: unit -> 'model, update: 'msg -> 'model -> 'model, view: 'model -> Dispatch<'msg> -> ITerminalElement) =

    let mutable terminalElement: IInternalTerminalElement = Unchecked.defaultof<_>

    let runComponent (ElmishTerminalProgram program) =

      let waitForView = TaskCompletionSource()

      // TODO: could be refactored into a ElmishTerminal.runTerminal
      let runComponent (model: InternalModel<_>) =
        let start dispatch =
          (task {
            // On program startup, Wait for the Elmish loop to take care of creating the root view.
            let! _ = model.RootView.Task
            terminalElement <- model.CurrentTreeState.Value
            waitForView.SetResult()
          }).GetAwaiter().GetResult()

          { new IDisposable with member _.Dispose() = () }

        start

      let subscribe model = [ [ "runComponent" ], runComponent model ]

      program
      |> Program.withSubscription subscribe
      |> Program.run

      waitForView.Task.GetAwaiter().GetResult()

      ()

    let mkSimpleComponent origin (init: 'arg -> 'model) (update: 'cmd -> 'model -> 'model) (view: 'model -> Dispatch<'cmd> -> ITerminalElement) =
      Program.mkSimple (OuterModel.wrapSimpleInit origin init) (OuterModel.wrapSimpleUpdate update) (OuterModel.wrapView view)
      |> Program.withSetState (setState (OuterModel.wrapView view))
      |> ElmishTerminalProgram

    member this.TerminalElement =
      if terminalElement = Unchecked.defaultof<_> then
        failwith "Elmish loop has not been started yet. Call StartElmishLoop before accessing the View."
      else
        terminalElement.View

    interface IElmishComponentTE with
      member this.StartElmishLoop(origin) =
        mkSimpleComponent origin init update view
        |> runComponent

    interface IInternalTerminalElement with
      member this.InitializeTree(origin) = () // Do nothing, initialization is handled by the Elmish component
      member this.Reuse prevElementData = terminalElement.Reuse prevElementData
      member this.Id with get() = terminalElement.Id and set v = terminalElement.Id <- v
      member this.View = terminalElement.View

      member this.Name = terminalElement.Name

      // Children are managed by the Elmish component itself. Hence they are hidden to the outside.
      member this.SetAsChildOfParentView = terminalElement.SetAsChildOfParentView

      member this.IsElmishComponent = true

      member this.Dispose() = terminalElement.Dispose()

      member this.Children = terminalElement.Children
      member this.Props = failwith "ElmishComponent_TerminalElement_Wrapper does not expose Props"
      member this.ViewSet = terminalElement.ViewSet

  let mkSimpleComponent (init: unit -> 'model) (update: 'msg -> 'model -> 'model) (view: 'model -> Dispatch<'msg> -> ITerminalElement) =
    new ElmishComponentTE<_,_,_>(init, update, view) :> ITerminalElement

  let mkProgram (init: 'arg -> 'model * Cmd<'msg>) (update: 'msg -> 'model -> 'model * Cmd<'msg>) (view: 'model -> Dispatch<'msg> -> ITerminalElement) =
    Program.mkProgram (OuterModel.wrapInit Root init) (OuterModel.wrapUpdate update) (OuterModel.wrapView view)
    |> Program.withSetState (setState (OuterModel.wrapView view))
    |> ElmishTerminalProgram

  let mkSimple (init: 'arg -> 'model) (update: 'cmd -> 'model -> 'model) (view: 'model -> Dispatch<'cmd> -> ITerminalElement) =
    Program.mkSimple (OuterModel.wrapSimpleInit Root init) (OuterModel.wrapSimpleUpdate update) (OuterModel.wrapView view)
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
        | RootView.AppRootView runnable ->
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
