module Terminal.Gui.Elmish.Tests.ElmishTester

open System
open System.Threading
open System.Threading.Channels
open System.Threading.Tasks
open Elmish
open Terminal.Gui.App
open Terminal.Gui.Elmish


[<Interface>]
type internal TestableElmishProgram<'msg> =
  abstract member ProcessMsg: TerminalMsg<'msg> -> Task
  abstract member ViewTE: IViewTE
  abstract member View: Terminal.Gui.ViewBase.View
  inherit IDisposable

let internal run (ElmishTerminal.ElmishTerminalProgram program: ElmishTerminal.ElmishTerminalProgram<IApplication,'model,TerminalMsg<'msg>,'view>) =

  let waitForStart = TaskCompletionSource()
  let mutable curTE = Unchecked.defaultof<_>
  let triggerTerminationTcs = TaskCompletionSource()

  let application = Application.Create()

  let startProgram (model: ElmishTerminal.InternalModel<_>) =
    let start dispatch =
      let rootView = model.RootView.Task.GetAwaiter().GetResult()
      match rootView with
      | ElmishTerminal.RootView.AppRootView _ ->
        curTE <- model.CurrentTreeState.Value
        waitForStart.SetResult()
      | _ ->
        failwith "`run` is meant to be used for with Runnable as root view."

      { new IDisposable with member _.Dispose() = () }

    start

  let triggerTermination model =
    let start dispatch =
      let cancellationToken = new CancellationTokenSource()
      Task.Factory.StartNew((fun () ->
        task {
          do! triggerTerminationTcs.Task.WaitAsync(cancellationToken.Token)
          dispatch Terminate
        }), TaskCreationOptions.LongRunning) |> ignore

      { new IDisposable with member _.Dispose() = cancellationToken.Cancel() }

    start

  let msgQueue = Channel.CreateUnbounded<TerminalMsg<_> * TaskCompletionSource<IViewTE>>()

  let msgDispatcher (model: ElmishTerminal.InternalModel<_>) =
    let start dispatch =
      let cancellationToken = new CancellationTokenSource()
      Task.Factory.StartNew((fun () ->
        task {
          while not cancellationToken.Token.IsCancellationRequested do
            let! msg, tcs = msgQueue.Reader.ReadAsync()
            let nextTreeStateTask = model.NextTreeStateTcs.Task
            dispatch msg
            let! nextTreeState = nextTreeStateTask
            tcs.SetResult(nextTreeState)
        }), TaskCreationOptions.LongRunning) |> ignore

      { new IDisposable with member _.Dispose() = cancellationToken.Cancel() }

    start

  let subscribe model = [
    [ "startProgram" ], startProgram model
    [ "triggerTermination" ], triggerTermination model
    [ "msgDispatcher" ], msgDispatcher model
  ]

  program
  |> Program.withSubscription subscribe
  |> Program.withTermination (fun msg -> msg = Terminate) ElmishTerminal.terminate
  |> Program.runWith application

  waitForStart.Task.GetAwaiter().GetResult()

  let processMsg (msg: TerminalMsg<'msg>) =
    task {
      let msgProcessedTcs = TaskCompletionSource<_>()
      let item = (msg, msgProcessedTcs)
      do! msgQueue.Writer.WriteAsync item
      // msgProcessedTcs is signaled by the msgDispatcher subscription.
      let! newTreeState = msgProcessedTcs.Task
      curTE <- newTreeState
    }

  {
    new TestableElmishProgram<'msg> with
      member _.ProcessMsg msg = processMsg msg
      member _.ViewTE = curTE
      member _.View = curTE.View
      member this.Dispose () =
        triggerTerminationTcs.SetResult()
        curTE.Dispose()
  }

let internal render view =
  let init _ = (), Cmd.none
  let update _ _ = (), Cmd.none
  let view _ _ = view

  ElmishTerminal.mkSimple init update view
  |> run
