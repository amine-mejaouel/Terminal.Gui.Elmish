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
  type internal TerminalModel<'model>(application: IApplication, origin: Origin, clientModel: 'model) =
    let mutable _currentTe: IViewTE option =
      None

    let mutable nextTeTcs: TaskCompletionSource<IViewTE> =
      TaskCompletionSource<_>()

    let rootViewTcs: TaskCompletionSource<View> =
      TaskCompletionSource<View>()

    let termination: TaskCompletionSource =
      TaskCompletionSource()

    member val ClientModel = clientModel with get, set

    member this.Application = application
    member this.Origin = origin
    member this.Termination = termination

    member this.RootViewSet = rootViewTcs.Task.IsCompletedSuccessfully

    member this.WaitForRootViewAsync() = rootViewTcs.Task

    member this.WaitForNextTerminalElementAsync() = nextTeTcs.Task

    member this.GetCurrentTerminalElementAsync() : Task<IViewTE> =
      task {
        let waitForNextTeTask =
          this.WaitForNextTerminalElementAsync()

        if _currentTe.IsSome then
          return _currentTe.Value
        else
          return! waitForNextTeTask
      }

    member this.SetCurrentTe(te: IViewTE) =

      _currentTe <- Some te

      if rootViewTcs.Task.IsCompletedSuccessfully |> not then
        rootViewTcs.SetResult te.View

      nextTeTcs.SetResult(te)
      nextTeTcs <- TaskCompletionSource<_>()

    interface IDisposable with
      member this.Dispose() =
        _currentTe |> Option.iter _.Dispose()

        if not origin.IsElmishComponent then
          application.RequestStop()

        termination.SetResult()


  module internal OuterModel =
    let internal wrapInit origin (init: 'arg -> 'model * Cmd<'msg>) : 'arg -> TerminalModel<'model> * Cmd<TerminalMsg<'msg>> =
      fun (arg: 'arg) ->
        let innerModel, cmd = init arg

        let terminalModel =
          new TerminalModel<_>(Application.Create(), origin, innerModel)

        terminalModel, cmd |> Cmd.map TerminalMsg.ofMsg

    let internal wrapUpdate (update: 'msg -> 'model -> 'model * Cmd<'msg>) : TerminalMsg<'msg> -> TerminalModel<'model> -> TerminalModel<'model> * Cmd<TerminalMsg<'msg>> =
      fun (msg: TerminalMsg<'msg>) (model: TerminalModel<'model>) ->
        match msg with
        | Terminate -> model, Cmd.none
        | Msg msg ->
          let innerModel, cmd =
            update msg model.ClientModel

          model.ClientModel <- innerModel
          model, Cmd.map TerminalMsg.ofMsg cmd

    let internal wrapView (view: 'model -> Dispatch<'msg> -> ITerminalElement) : TerminalModel<'model> -> Dispatch<TerminalMsg<'msg>> -> ITerminalElement =
      fun (model: TerminalModel<'model>) (dispatch: Dispatch<TerminalMsg<'msg>>) -> view model.ClientModel (TerminalMsg.ofMsg >> dispatch)

    let internal wrapSimpleInit origin (init: 'arg -> 'model) =
      fun (arg: 'arg) ->
        let innerModel = init arg

        let terminalModel =
          new TerminalModel<_>(Application.Create(), origin, innerModel)

        terminalModel

    let internal wrapSimpleUpdate (update: 'msg -> 'model -> 'model) : TerminalMsg<'msg> -> TerminalModel<'model> -> TerminalModel<'model> =
      fun (msg: TerminalMsg<'msg>) (model: TerminalModel<'model>) ->
        match msg with
        | Terminate -> model
        | Msg msg ->
          let innerModel =
            update msg model.ClientModel

          model.ClientModel <- innerModel
          model

    let internal wrapSubscribe (subscribe: 'model -> Sub<'msg>) : TerminalModel<'model> -> _ =
      fun outerModel ->
        subscribe outerModel.ClientModel
        |> Sub.map "WrapSubscribe" (TerminalMsg.ofMsg >> TerminalMsg.Msg)

  type ElmishTerminalProgram<'arg, 'model, 'msg, 'view> = internal ElmishTerminalProgram of Program<'arg, TerminalModel<'model>, TerminalMsg<'msg>, 'view>

  let private setState (view: TerminalModel<'model> -> Dispatch<TerminalMsg<'cmd>> -> ITerminalElement) (model: TerminalModel<'model>) dispatch =
    task {
      let nextTe =
        task {
          if not model.RootViewSet then

            let initialTe =
              view model dispatch :?> IViewTE

            initialTe.InitializeTree model.Origin

            return initialTe

          else
            let! (currentTe: IViewTE) = model.GetCurrentTerminalElementAsync()

            let nextTe = view model dispatch :?> IViewTE

            Differ.update (TerminalElement.ViewBackedTE currentTe) (TerminalElement.ViewBackedTE nextTe)

            currentTe.Dispose()
            return nextTe
        }

      let! nextTe = nextTe
      model.SetCurrentTe nextTe

      ()
    }
    |> Task.wait

  let internal terminate (model: TerminalModel<_>) = (model :> IDisposable).Dispose()

  /// <summary>
  /// <para>Wrapper that elmish components should use to expose themselves as IInternalTerminalElement.</para>
  /// <para>As the Elmish component handles its own initialization and children management in his separate Elmish loop,
  /// this wrapper will hide these aspects to the outside world. Thus preventing double initialization or double children management.</para>
  /// </summary>
  type internal ElmishComponentTE<'model, 'msg, 'view>(name, init: unit -> 'model, update: 'msg -> 'model -> 'model, view: 'model -> Dispatch<'msg> -> ITerminalElement) =

    let teTcs: TaskCompletionSource<IViewTE> =
      TaskCompletionSource<_>()

    let viewSetEvent = Event<View>()

    let runComponent (ElmishTerminalProgram program) =

      // TODO: could be refactored into a ElmishTerminal.runTerminal
      let runComponent (model: TerminalModel<_>) =
        let start dispatch =
          task {
            let! rootView = model.WaitForRootViewAsync()

            viewSetEvent.Trigger rootView

            let! currentTe = model.GetCurrentTerminalElementAsync()
            teTcs.SetResult(currentTe)
          }
          |> Task.wait

          { new IDisposable with
              member _.Dispose() = ()
          }

        start

      let subscribe model = [
        [ "runComponent" ], runComponent model
      ]

      program
      |> Program.withSubscription subscribe
      |> Program.run

      teTcs.Task |> Task.wait |> ignore

      ()

    let mkSimpleComponent terminalElement (init: 'arg -> 'model) (update: 'cmd -> 'model -> 'model) (view: 'model -> Dispatch<'cmd> -> ITerminalElement) =
      Program.mkSimple (OuterModel.wrapSimpleInit (Origin.ElmishComponent terminalElement) init) (OuterModel.wrapSimpleUpdate update) (OuterModel.wrapView view)
      |> Program.withSetState (setState (OuterModel.wrapView view))
      |> ElmishTerminalProgram

    member this.GetViewAsync() =
      task {
        let! te = teTcs.Task
        return te.View
      }

    member this.View =
      if teTcs.Task.IsCompleted then
        teTcs.Task.Result.View
      else
        failwith "Elmish loop has not been started yet. Call StartElmishLoop before accessing the View property."

    member this.Child =
      if teTcs.Task.IsCompleted then
        teTcs.Task.Result
      else
        failwith "Elmish loop has not been started yet. Call StartElmishLoop before accessing the Child property."

    member val Origin = Unchecked.defaultof<_> with get, set

    [<CLIEvent>]
    member this.OnViewSet = viewSetEvent.Publish

    member this.Dispose() =
      task {
        let! te = teTcs.Task
        te.Dispose()
      }
      |> Task.wait

    interface IElmishComponentTE with
      member this.StartElmishLoop() =
        mkSimpleComponent this init update view
        |> runComponent

      member this.Child = this.Child

    interface ITerminalElementBase with
      member this.View = this.View
      member this.Name = name
      member this.OnViewSet = this.OnViewSet

      member this.Origin
        with get () = this.Origin
        and set v = this.Origin <- v

      member this.GetPath() = this.Origin.GetPath(name)
      member this.Dispose() = this.Dispose()

  let mkSimpleComponent name (init: unit -> 'model) (update: 'msg -> 'model -> 'model) (view: 'model -> Dispatch<'msg> -> ITerminalElement) =
    new ElmishComponentTE<_, _, _>(name, init, update, view) :> ITerminalElement

  let mkProgram (init: 'arg -> 'model * Cmd<'msg>) (update: 'msg -> 'model -> 'model * Cmd<'msg>) (view: 'model -> Dispatch<'msg> -> ITerminalElement) =
    Program.mkProgram (OuterModel.wrapInit Origin.Root init) (OuterModel.wrapUpdate update) (OuterModel.wrapView view)
    |> Program.withSetState (setState (OuterModel.wrapView view))
    |> ElmishTerminalProgram

  let mkSimple (init: 'arg -> 'model) (update: 'cmd -> 'model -> 'model) (view: 'model -> Dispatch<'cmd> -> ITerminalElement) =
    Program.mkSimple (OuterModel.wrapSimpleInit Origin.Root init) (OuterModel.wrapSimpleUpdate update) (OuterModel.wrapView view)
    |> Program.withSetState (setState (OuterModel.wrapView view))
    |> ElmishTerminalProgram

  let withSubscription (subscribe: 'model -> Sub<'msg>) (ElmishTerminalProgram program) =
    program
    |> Program.withSubscription (OuterModel.wrapSubscribe subscribe)
    |> ElmishTerminalProgram

  let withTermination predicate (ElmishTerminalProgram program) = program |> ElmishTerminalProgram

  let runTerminal (ElmishTerminalProgram program) =

    let waitForStart = TaskCompletionSource()
    let running = TaskCompletionSource()
    let mutable waitForTermination = null

    let runTerminal (model: TerminalModel<_>) =
      let start dispatch =
        task {
          let! rootView = model.WaitForRootViewAsync()

          if model.Origin.IsElmishComponent then
            failwith (
              "`run` is meant to be used for Terminal Elmish loop. "
              + "For Terminal components with separate Elmish loop, use `runComponent`."
            )
          else
            Task.Run(fun () ->
              (try
                model.Application.Init() |> ignore

                model.Application.Run(rootView :?> Runnable)
                |> ignore

                running.SetResult()
               with ex ->
                 running.SetException ex),
              TaskCreationOptions.LongRunning
            )
            |> ignore

            waitForStart.SetResult()
            waitForTermination <- model.Termination

        }
        |> Task.wait

        { new IDisposable with
            member _.Dispose() = ()
        }

      start

    let subscribe model = [ [ "runTerminal" ], runTerminal model ]

    program
    |> Program.withSubscription subscribe
    |> Program.withTermination (fun msg -> msg = Terminate) terminate
    |> Program.run

    waitForStart.Task.GetAwaiter().GetResult()
    Task.WhenAll(running.Task, waitForTermination.Task).GetAwaiter().GetResult()
