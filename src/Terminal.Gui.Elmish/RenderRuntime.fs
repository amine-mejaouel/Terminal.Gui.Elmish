namespace Terminal.Gui.Elmish

open System
open System.Threading
open System.Threading.Tasks
open Terminal.Gui.App

[<Interface>]
type internal IRenderDispatcher =
  inherit IDisposable

  abstract Activate: unit -> unit
  abstract InvokeAsync: action: Action * cancellationToken: CancellationToken -> Task

type internal DeferredRenderDispatcher(application: IApplication) =
  let activation =
    TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously)

  let mutable disposed = 0

  member _.Activate() =
    if Volatile.Read(&disposed) <> 0 then
      raise (ObjectDisposedException(nameof DeferredRenderDispatcher))

    activation.TrySetResult() |> ignore

  member _.InvokeAsync(action: Action, cancellationToken: CancellationToken) : Task =
    task {
      if Volatile.Read(&disposed) <> 0 then
        raise (ObjectDisposedException(nameof DeferredRenderDispatcher))

      do! activation.Task.WaitAsync(cancellationToken)

      if Volatile.Read(&disposed) <> 0 then
        raise (ObjectDisposedException(nameof DeferredRenderDispatcher))

      let completion =
        TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously)

      let timeoutToken =
        application.AddTimeout(
          TimeSpan.Zero,
          Func<bool>(fun () ->
            try
              action.Invoke()
              completion.TrySetResult() |> ignore
              false
            with ex ->
              completion.TrySetException(ex) |> ignore
              raise ex)
        )

      use _registration =
        cancellationToken.Register(fun () ->
          if completion.TrySetCanceled(cancellationToken) then
            application.RemoveTimeout timeoutToken |> ignore)

      do! completion.Task
    }

  member _.Dispose() =
    if Interlocked.Exchange(&disposed, 1) = 0 then
      activation.TrySetException(ObjectDisposedException(nameof DeferredRenderDispatcher))
      |> ignore

  interface IRenderDispatcher with
    member this.Activate() = this.Activate()

    member this.InvokeAsync(action, cancellationToken) =
      this.InvokeAsync(action, cancellationToken)

    member this.Dispose() = this.Dispose()

type internal ImmediateRenderDispatcher() =
  let mutable disposed = 0

  member _.Activate() = ()

  member _.InvokeAsync(action: Action, cancellationToken: CancellationToken) : Task =
    task {
      cancellationToken.ThrowIfCancellationRequested()

      if Volatile.Read(&disposed) <> 0 then
        raise (ObjectDisposedException(nameof ImmediateRenderDispatcher))

      action.Invoke()
    }

  member _.Dispose() =
    Interlocked.Exchange(&disposed, 1) |> ignore

  interface IRenderDispatcher with
    member this.Activate() = this.Activate()

    member this.InvokeAsync(action, cancellationToken) =
      this.InvokeAsync(action, cancellationToken)

    member this.Dispose() = this.Dispose()

type internal TerminalRuntime =
  { Application: IApplication
    RenderDispatcher: IRenderDispatcher }

[<RequireQualifiedAccess>]
module internal TerminalRuntime =
  let createProduction () =
    let application = Application.Create()

    { Application = application
      RenderDispatcher = new DeferredRenderDispatcher(application) }

  let createImmediate () =
    { Application = Application.Create()
      RenderDispatcher = new ImmediateRenderDispatcher() }
