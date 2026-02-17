module Terminal.Gui.Elmish.Tests.ElmishTester

open System
open System.Threading
open System.Threading.Tasks
open Elmish
open Terminal.Gui.App
open Terminal.Gui.Elmish

let internal run (ElmishTerminal.ElmishTerminalProgram program) =

  let waitForStart = TaskCompletionSource()
  let mutable curTE = Unchecked.defaultof<_>
  let triggerTerminationTcs = TaskCompletionSource()

  let application = Application.Create()

  let runTerminal (model: ElmishTerminal.InternalModel<_>) =
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


  let subscribe model = [
    [ "triggerTermination" ], triggerTermination model
    [ "runTerminal" ], runTerminal model
  ]

  program
  |> Program.withSubscription subscribe
  |> Program.withTermination (fun msg -> msg = Terminate) ElmishTerminal.terminate
  |> Program.runWith application

  waitForStart.Task.GetAwaiter().GetResult()

  { new IViewTE with
      member this.InitializeTree origin = failwith "todo"
      member this.GetPath() = failwith "todo"
      member this.Origin = failwith "todo"
      member this.Origin with set value = failwith "todo"
      member this.Reuse(prev) = failwith "todo"
      member this.Name = failwith "todo"
      member this.View = curTE.View
      member this.Props = failwith "todo"
      member this.SetAsChildOfParentView = failwith "todo"
      member this.OnViewSet = failwith "todo"
      member this.Children = failwith "todo"

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
