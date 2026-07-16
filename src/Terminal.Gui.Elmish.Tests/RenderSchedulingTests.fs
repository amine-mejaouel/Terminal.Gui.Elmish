module Terminal.Gui.Elmish.Tests.RenderSchedulingTests

open System
open System.Threading
open System.Threading.Tasks
open NUnit.Framework
open Terminal.Gui.App
open Terminal.Gui.Elmish

type private ManualInvocation(action: Action, completion: TaskCompletionSource) =
  member _.Run() =
    try
      action.Invoke()
      completion.TrySetResult() |> ignore
    with ex ->
      completion.TrySetException(ex) |> ignore
      raise ex

type private ManualRenderDispatcher() =
  let invocation =
    TaskCompletionSource<ManualInvocation>(TaskCreationOptions.RunContinuationsAsynchronously)

  let mutable disposed = 0

  member _.NextInvocationAsync() = invocation.Task

  member _.Dispose() =
    Interlocked.Exchange(&disposed, 1) |> ignore

  interface IRenderDispatcher with
    member _.Activate() = ()

    member _.InvokeAsync(action, cancellationToken) =
      if Volatile.Read(&disposed) <> 0 then
        Task.FromException(ObjectDisposedException(nameof ManualRenderDispatcher))
      else
        let completion =
          TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously)

        if not (invocation.TrySetResult(ManualInvocation(action, completion))) then
          Task.FromException(InvalidOperationException("Only one render invocation was expected."))
        else
          completion.Task.WaitAsync(cancellationToken)

    member this.Dispose() = this.Dispose()

let private root text =
  View.Runnable(fun (p: RunnableProps) -> p.Children [ View.Label(fun p -> p.Text text) :> IView ]) :> IView
  :?> ISimpleViewSpec

let private renderedText (terminalElement: IViewTE) =
  terminalElement.View.SubViews |> Seq.exactlyOne |> _.Text.ToString()

[<Test>]
let ``The initial tree is mounted synchronously`` () =
  use application = Application.Create()
  use dispatcher = new ManualRenderDispatcher()

  let runtime =
    { Application = application
      RenderDispatcher = dispatcher }

  let state = ElmishTerminal.TerminalRenderCoordinator(runtime)

  try
    state.RequestRender(root "initial", Origin.Root)

    Assert.Multiple(fun () ->
      Assert.That(state.RootViewSet, Is.True)
      Assert.That(state.GetCurrentTEAsync().IsCompletedSuccessfully, Is.True)
      Assert.That(renderedText (state.GetCurrentTEAsync().Result), Is.EqualTo("initial")))
  finally
    state.Dispose()

[<Test>]
let ``Pending trees collapse to the latest tree before the UI commit`` () =
  task {
    use application = Application.Create()
    use dispatcher = new ManualRenderDispatcher()

    let runtime =
      { Application = application
        RenderDispatcher = dispatcher }

    let state = ElmishTerminal.TerminalRenderCoordinator(runtime)

    try
      state.RequestRender(root "initial", Origin.Root)
      let initial = state.GetCurrentTEAsync().Result
      let nextRender = state.GetNextTEAsync()

      state.RequestRender(root "intermediate", Origin.Root)

      let! invocation = dispatcher.NextInvocationAsync().WaitAsync(TimeSpan.FromSeconds 5.0)

      state.RequestRender(root "latest", Origin.Root)
      invocation.Run()

      let! rendered = nextRender.WaitAsync(TimeSpan.FromSeconds 5.0)

      Assert.Multiple(fun () ->
        Assert.That(rendered.View, Is.SameAs(initial.View))
        Assert.That(renderedText rendered, Is.EqualTo("latest")))
    finally
      state.Dispose()
  }

[<Test>]
let ``Disposal cancels a scheduled render and rejects later requests`` () =
  task {
    use application = Application.Create()
    use dispatcher = new ManualRenderDispatcher()

    let runtime =
      { Application = application
        RenderDispatcher = dispatcher }

    let state = ElmishTerminal.TerminalRenderCoordinator(runtime)

    state.RequestRender(root "initial", Origin.Root)
    let waitingForRender = state.GetNextTEAsync()
    state.RequestRender(root "pending", Origin.Root)

    let! _ = dispatcher.NextInvocationAsync().WaitAsync(TimeSpan.FromSeconds 5.0)
    state.Dispose()

    Assert.Multiple(fun () ->
      Assert.That(waitingForRender.IsFaulted, Is.True)
      Assert.That(waitingForRender.Exception.GetBaseException(), Is.TypeOf<ObjectDisposedException>())

      Assert.Throws<ObjectDisposedException>(fun () -> state.RequestRender(root "too late", Origin.Root))
      |> ignore)
  }
