namespace Terminal.Gui.Elmish

open System
open System.Threading.Tasks
open Elmish
open Terminal.Gui.App
open Terminal.Gui.Elmish
open Terminal.Gui.Views

type TerminalMsg<'a> =
  | Terminate
  | Msg of 'a

[<RequireQualifiedAccess>]
module TerminalMsg =
  let ofMsg msg = TerminalMsg.Msg msg

[<RequireQualifiedAccess>]
module Program =

  /// <summary>
  /// <p>Internal model of the Elmish loop. This model is not exposed to the library caller.</p>
  /// <p>It is used internally to manage the state of the terminal elements and the application.</p>
  /// <param name="ClientModel">Elmish model provided to the Program by the library caller.</param>
  /// </summary>
  type internal MainTerminalModel<'model>(application: IApplication, clientModel: 'model) =
    let terminalElementState = TerminalElementState()

    member val ClientModel = clientModel with get, set
    member this.Application = application
    member this.RootViewSet = terminalElementState.RootViewSet
    member this.TerminalElementState = terminalElementState

    member this.Dispose() = terminalElementState.Dispose()

    interface ITerminalModel<'model> with
      member this.RootViewSet = this.RootViewSet
      member this.TerminalElementState: TerminalElementState = this.TerminalElementState
      member this.Address = [ AddressSegment.Root ]

    interface IDisposable with
      member this.Dispose() = this.Dispose()

  module internal OuterModel =
    let internal wrapInit
      (init: 'arg -> 'model * Cmd<TerminalMsg<'msg>>)
      : 'arg -> MainTerminalModel<'model> * Cmd<TerminalMsg<'msg>> =
      fun (arg: 'arg) ->
        let innerModel, cmd = init arg

        let terminalModel = new MainTerminalModel<_>(Application.Create(), innerModel)

        terminalModel, cmd

    let internal wrapUpdate
      (update: 'msg -> 'model -> 'model * Cmd<TerminalMsg<'msg>>)
      : TerminalMsg<'msg> -> MainTerminalModel<'model> -> MainTerminalModel<'model> * Cmd<TerminalMsg<'msg>> =
      fun (msg: TerminalMsg<'msg>) (model: MainTerminalModel<'model>) ->
        match msg with
        | Terminate -> model, Cmd.none
        | Msg msg ->
          let innerModel, cmd = update msg model.ClientModel

          model.ClientModel <- innerModel
          model, cmd

    let internal wrapView
      (view: 'model -> Dispatch<TerminalMsg<'msg>> -> ITerminalElement)
      : MainTerminalModel<'model> -> Dispatch<TerminalMsg<'msg>> -> ITerminalElement =
      fun (model: MainTerminalModel<'model>) (dispatch: Dispatch<TerminalMsg<'msg>>) -> view model.ClientModel dispatch

    let internal wrapSimpleInit (init: 'arg -> 'model) =
      fun (arg: 'arg) ->
        let innerModel = init arg

        let terminalModel = new MainTerminalModel<_>(Application.Create(), innerModel)

        terminalModel

    let internal wrapSimpleUpdate
      (update: 'msg -> 'model -> 'model)
      : TerminalMsg<'msg> -> MainTerminalModel<'model> -> MainTerminalModel<'model> =
      fun (msg: TerminalMsg<'msg>) (model: MainTerminalModel<'model>) ->
        match msg with
        | Terminate -> model
        | Msg msg ->
          let innerModel = update msg model.ClientModel

          model.ClientModel <- innerModel
          model

    let internal wrapSubscribe (subscribe: 'model -> Sub<'msg>) : MainTerminalModel<'model> -> _ =
      fun outerModel -> subscribe outerModel.ClientModel |> Sub.map "WrapSubscribe" TerminalMsg.ofMsg

  type MainTerminalProgram<'arg, 'model, 'msg, 'view> =
    internal | MainTerminalProgram of Program<'arg, MainTerminalModel<'model>, TerminalMsg<'msg>, 'view>

  let internal terminate (model: MainTerminalModel<_>) =
    // For the main elmish loop, signal stop and let runTerminal handle cleanup after Run() returns
    model.Application.RequestStop()

  let internal setState view : (ITerminalModel<'model> -> Dispatch<TerminalMsg<'cmd>> -> unit) =
    let wrapView (view: MainTerminalModel<'model> -> Dispatch<TerminalMsg<'cmd>> -> ITerminalElement) =
      fun (model: ITerminalModel<'model>) (dispatch: Dispatch<TerminalMsg<'cmd>>) ->
        let model = model :?> MainTerminalModel<'model>
        (view model dispatch)

    Common.setState (view |> OuterModel.wrapView |> wrapView)

  let mkProgram
    (init: 'arg -> 'model * Cmd<TerminalMsg<'msg>>)
    (update: 'msg -> 'model -> 'model * Cmd<TerminalMsg<'msg>>)
    (view: 'model -> Dispatch<TerminalMsg<'msg>> -> ITerminalElement)
    =

    Program.mkProgram (OuterModel.wrapInit init) (OuterModel.wrapUpdate update) (OuterModel.wrapView view)
    |> Program.withSetState (setState view)
    |> MainTerminalProgram

  let mkSimple
    (init: 'arg -> 'model)
    (update: 'cmd -> 'model -> 'model)
    (view: 'model -> Dispatch<TerminalMsg<'cmd>> -> ITerminalElement)
    =
    Program.mkSimple (OuterModel.wrapSimpleInit init) (OuterModel.wrapSimpleUpdate update) (OuterModel.wrapView view)
    |> Program.withSetState (setState view)
    |> MainTerminalProgram

  let withSubscription (subscribe: 'model -> Sub<'msg>) (MainTerminalProgram program) =
    program
    |> Program.withSubscription (OuterModel.wrapSubscribe subscribe)
    |> MainTerminalProgram

  let withTermination predicate (MainTerminalProgram program) = program |> MainTerminalProgram

  let runTerminal (MainTerminalProgram program) =

    let applicationStopped = TaskCompletionSource() // Rethrow any exception from the application thread.

    let runTerminal (model: MainTerminalModel<_>) =
      let start dispatch =
        task {
          let! rootView = model.TerminalElementState.WaitTillRootViewIsSetAsync()

          Task.Run(fun () ->
            (try
              try
                model.Application.Init() |> ignore
                // Run return after Application.RequestStop is called in terminate.
                model.Application.Run(rootView :?> Runnable) |> ignore
              finally
                model.Dispose()
                // 2. Dispose the IApplication (restores terminal, cleans up driver)
                model.Application.Dispose()

              applicationStopped.SetResult()
             with ex ->
               applicationStopped.SetException ex

            ),
            TaskCreationOptions.LongRunning)
          |> ignore

        }
        |> Task.wait

        { new IDisposable with
            member _.Dispose() = () }

      start

    let subscribe model =
      [ [ "runTerminal" ], runTerminal model ]

    program
    |> Program.withSubscription subscribe
    |> Program.withTermination (fun msg -> msg = Terminate) terminate
    |> Program.run

    Task.WhenAll(applicationStopped.Task).GetAwaiter().GetResult()
