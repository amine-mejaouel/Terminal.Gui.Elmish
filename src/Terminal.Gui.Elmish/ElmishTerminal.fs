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

  /// <summary>
  /// <p>Internal model of the Elmish loop. This model is not exposed to the library caller.</p>
  /// <p>It is used internally to manage the state of the terminal elements and the application.</p>
  /// <param name="ClientModel">Elmish model provided to the Program by the library caller.</param>
  /// </summary>
  type internal InternalModel<'model> =
    {
      mutable CurrentTe: IViewTE option
      mutable NextTeTcs: TaskCompletionSource<IViewTE>
      InitialTeSet: TaskCompletionSource<IViewTE>
      mutable Application: IApplication
      Origin: Origin
      // TODO: Termination is not used in Elmish Components
      Termination: TaskCompletionSource
      /// Elmish model provided to the Program by the library caller.
      ClientModel: 'model
    }
      member this.WaitForTerminalElementInitialization() =
        this.InitialTeSet.Task

      member this.WaitForNextTerminalElement() =
        this.NextTeTcs.Task

      member this.SetNextTerminalElement(te: IViewTE) =
        this.NextTeTcs.SetResult te
        this.NextTeTcs <- TaskCompletionSource<_>()

  module internal OuterModel =
    let internal wrapInit origin (init: 'arg -> 'model * Cmd<'msg>) =
      fun (arg: 'arg) ->
        let innerModel, cmd = init arg

        let internalModel = {
          Application = Application.Create()
          CurrentTe = None
          NextTeTcs = TaskCompletionSource<_>()
          InitialTeSet = TaskCompletionSource<_>()
          Origin = origin
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
          CurrentTe = None
          NextTeTcs = TaskCompletionSource<_>()
          InitialTeSet = TaskCompletionSource<_>()
          Origin = origin
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

  let private setState (view: InternalModel<'model> -> Dispatch<'cmd> -> ITerminalElement) (model: InternalModel<'model>) dispatch =

    let nextTe =
      match model.CurrentTe with
      | None ->

        let initialTe =
          view model dispatch :?> IViewTE

        initialTe.InitializeTree model.Origin

        model.InitialTeSet.SetResult(initialTe)

        initialTe

      | Some currentTe ->
        let nextTe =
          view model dispatch :?> IViewTE

        Differ.update
          (TerminalElement.ViewBackedTE currentTe)
          (TerminalElement.ViewBackedTE nextTe)

        currentTe.Dispose()
        nextTe

    model.CurrentTe <- Some nextTe
    model.SetNextTerminalElement nextTe

    ()

  let internal terminate model =

    model.CurrentTe |> Option.iter _.Dispose()

    if not model.Origin.IsElmishComponent then
      model.Application.RequestStop()

    model.Termination.SetResult()

  /// <summary>
  /// <para>Wrapper that elmish components should use to expose themselves as IInternalTerminalElement.</para>
  /// <para>As the Elmish component handles its own initialization and children management in his separate Elmish loop,
  /// this wrapper will hide these aspects to the outside world. Thus preventing double initialization or double children management.</para>
  /// </summary>
  type internal ElmishComponentTE<'model, 'msg, 'view>(name, init: unit -> 'model, update: 'msg -> 'model -> 'model, view: 'model -> Dispatch<'msg> -> ITerminalElement) =

    let viewTeTcs: TaskCompletionSource<IViewTE> = TaskCompletionSource<_>()
    let viewSetEvent = Event<View>()

    let runComponent (ElmishTerminalProgram program) =

      // TODO: could be refactored into a ElmishTerminal.runTerminal
      let runComponent (model: InternalModel<_>) =
        let start dispatch =
          task {
            let! te = model.WaitForTerminalElementInitialization()

            viewSetEvent.Trigger te.View
            viewTeTcs.SetResult(te)
          } |> Task.wait

          { new IDisposable with member _.Dispose() = () }

        start

      let subscribe model = [ [ "runComponent" ], runComponent model ]

      program
      |> Program.withSubscription subscribe
      |> Program.run

      viewTeTcs.Task |> Task.wait |> ignore

      ()

    let mkSimpleComponent terminalElement (init: 'arg -> 'model) (update: 'cmd -> 'model -> 'model) (view: 'model -> Dispatch<'cmd> -> ITerminalElement) =
      Program.mkSimple
        (OuterModel.wrapSimpleInit (Origin.ElmishComponent terminalElement) init)
        (OuterModel.wrapSimpleUpdate update)
        (OuterModel.wrapView view)
      |> Program.withSetState (setState (OuterModel.wrapView view))
      |> ElmishTerminalProgram

    member this.GetViewAsync() =
      task {
        let! viewTe = viewTeTcs.Task
        return viewTe.View
      }

    member this.View =
      if viewTeTcs.Task.IsCompleted then
        viewTeTcs.Task.Result.View
      else
        failwith "Elmish loop has not been started yet. Call StartElmishLoop before accessing the View property."

    member this.Child =
      if viewTeTcs.Task.IsCompleted then
        viewTeTcs.Task.Result
      else
        failwith "Elmish loop has not been started yet. Call StartElmishLoop before accessing the Child property."

    member val Origin = Unchecked.defaultof<_> with get, set

    [<CLIEvent>]
    member this.OnViewSet = viewSetEvent.Publish

    member this.Dispose() =
      task {
        let! viewTe = viewTeTcs.Task
        viewTe.Dispose()
      } |> Task.wait

    interface IElmishComponentTE with
      member this.StartElmishLoop() =
        mkSimpleComponent this init update view
        |> runComponent
      member this.Child = this.Child

    interface ITerminalElementBase with
      member this.View = this.View
      member this.Name = name
      member this.OnViewSet = this.OnViewSet
      member this.Origin with get() = this.Origin and set v = this.Origin <- v
      member this.GetPath() = this.Origin.GetPath(name)
      member this.Dispose() = this.Dispose()

  let mkSimpleComponent name (init: unit -> 'model) (update: 'msg -> 'model -> 'model) (view: 'model -> Dispatch<'msg> -> ITerminalElement) =
    new ElmishComponentTE<_,_,_>(name, init, update, view) :> ITerminalElement

  let mkProgram (init: 'arg -> 'model * Cmd<'msg>) (update: 'msg -> 'model -> 'model * Cmd<'msg>) (view: 'model -> Dispatch<'msg> -> ITerminalElement) =
    Program.mkProgram (OuterModel.wrapInit Origin.Root init) (OuterModel.wrapUpdate update) (OuterModel.wrapView view)
    |> Program.withSetState (setState (OuterModel.wrapView view))
    |> ElmishTerminalProgram

  let mkSimple (init: 'arg -> 'model) (update: TerminalMsg<'cmd> -> 'model -> 'model) (view: 'model -> Dispatch<TerminalMsg<'cmd>> -> ITerminalElement) =
    Program.mkSimple (OuterModel.wrapSimpleInit Origin.Root init) (OuterModel.wrapSimpleUpdate update) (OuterModel.wrapView view)
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

    let runTerminal (model: InternalModel<_>) =
      let start dispatch =
        task {
          let! te = model.WaitForTerminalElementInitialization()

          if te.Origin.IsElmishComponent then
            failwith (
              "`run` is meant to be used for Terminal Elmish loop. " +
              "For Terminal components with separate Elmish loop, use `runComponent`.")
          else
            Task.Run(fun () ->
              (
                try
                  model.Application.Init() |> ignore
                  model.Application.Run(model.CurrentTe.Value.View :?> Runnable) |> ignore
                  running.SetResult()
                with ex -> running.SetException ex
              )
              , TaskCreationOptions.LongRunning
            ) |> ignore

            waitForStart.SetResult()
            waitForTermination <- model.Termination

        } |> Task.wait

        { new IDisposable with member _.Dispose() = () }

      start

    let subscribe model = [ [ "runTerminal" ], runTerminal model ]

    program
    |> Program.withSubscription subscribe
    |> Program.withTermination (fun msg -> msg = Terminate) terminate
    |> Program.run

    waitForStart.Task.GetAwaiter().GetResult()
    Task.WhenAll(running.Task, waitForTermination.Task).GetAwaiter().GetResult()
