namespace Terminal.Gui.Elmish

open System
open System.Threading
open System.Threading.Channels
open System.Threading.Tasks
open Elmish
open Terminal.Gui.Elmish
open Terminal.Gui.ViewBase
open Terminal.Gui.Views

type internal ProgramKind =
  /// Main elmish program.
  | Root
  /// Elmish component with its own elmish loop, nested inside a Root program or another Elmish component.
  | ElmishComponent of IElmishComponentTE

type TerminalMsg<'a> =
  | Terminate
  | Msg of 'a

[<RequireQualifiedAccess>]
module TerminalMsg =
  let ofMsg msg = TerminalMsg.Msg msg

[<RequireQualifiedAccess>]
module ElmishTerminal =

  type private RenderRequest =
    { ViewSpec: ISimpleViewSpec
      Origin: Origin }

  [<RequireQualifiedAccess>]
  type private RenderLifecycle =
    | AwaitingInitialRender
    | MountingInitialRender
    | Running
    | Faulted of exn
    | Disposed

  type internal TerminalRenderCoordinator(sharedContext: TerminalRenderContext) =

    let mutable currentRoot: IViewTE option = None
    let publishedRootGate = obj ()

    let newRenderedRootTcs () =
      TaskCompletionSource<IViewTE>(TaskCreationOptions.RunContinuationsAsynchronously)

    let mutable nextRenderedRootTcs = newRenderedRootTcs ()

    let initialRootViewTcs =
      TaskCompletionSource<View>(TaskCreationOptions.RunContinuationsAsynchronously)

    let treeRenderer = new VirtualTree.Renderer(sharedContext)
    let lifecycleGate = obj ()
    let commitGate = obj ()
    let shutdown = new CancellationTokenSource()

    let renderRequests =
      Channel.CreateBounded<RenderRequest>(
        BoundedChannelOptions(
          1,
          SingleReader = true,
          SingleWriter = false,
          AllowSynchronousContinuations = false,
          FullMode = BoundedChannelFullMode.DropOldest
        )
      )

    let mutable lifecycle = RenderLifecycle.AwaitingInitialRender
    let mutable renderPumpTask: Task = Task.CompletedTask

    let failRenderWaiters (ex: exn) =
      let waitingForNext = lock publishedRootGate (fun () -> nextRenderedRootTcs)

      initialRootViewTcs.TrySetException(ex) |> ignore
      waitingForNext.TrySetException(ex) |> ignore

    let enterFaulted ex =
      let transitioned =
        lock lifecycleGate (fun () ->
          match lifecycle with
          | RenderLifecycle.Disposed
          | RenderLifecycle.Faulted _ -> false
          | _ ->
            lifecycle <- RenderLifecycle.Faulted ex
            true)

      if transitioned then
        renderRequests.Writer.TryComplete(ex) |> ignore
        failRenderWaiters ex

    let commitRender request =

      let canCommit () =
        lock lifecycleGate (fun () ->
          match lifecycle with
          | RenderLifecycle.MountingInitialRender
          | RenderLifecycle.Running -> true
          | _ -> false)

      let publishRoot root =
        let renderedRootTcs =
          lock publishedRootGate (fun () ->
            currentRoot <- Some root
            initialRootViewTcs.TrySetResult(root.View) |> ignore

            let waiting = nextRenderedRootTcs
            nextRenderedRootTcs <- newRenderedRootTcs ()
            waiting)

        renderedRootTcs.TrySetResult(root) |> ignore

      lock commitGate (fun () ->
        if canCommit () then
          treeRenderer.Render(request.ViewSpec, request.Origin) |> publishRoot)

    let runRenderPump () : Task =

      let readLatestRequest initial =
        let mutable latest = initial
        let mutable candidate = Unchecked.defaultof<RenderRequest>

        while renderRequests.Reader.TryRead(&candidate) do
          latest <- candidate

        latest

      task {
        try
          let mutable keepRunning = true

          while keepRunning do
            let! canRead = renderRequests.Reader.WaitToReadAsync(shutdown.Token).AsTask()

            if not canRead then
              keepRunning <- false
            else
              let mutable request = Unchecked.defaultof<RenderRequest>

              if renderRequests.Reader.TryRead(&request) then
                let mutable latest = readLatestRequest request

                do!
                  sharedContext.RenderDispatcher.DispatchAsync(
                    Action(fun () ->
                      latest <- readLatestRequest latest
                      commitRender latest),
                    shutdown.Token
                  )
        with
        | :? OperationCanceledException when shutdown.IsCancellationRequested -> ()
        | ex ->
          enterFaulted ex
          return raise ex
      }

    member _.WaitForInitialRootViewAsync() = initialRootViewTcs.Task

    member _.WaitForNextRenderedRootAsync() =
      lock publishedRootGate (fun () -> nextRenderedRootTcs.Task)

    member _.GetCurrentRootAsync() : Task<IViewTE> =
      task {
        let root, waitForRenderedRoot =
          lock publishedRootGate (fun () -> currentRoot, nextRenderedRootTcs.Task)

        if root.IsSome then
          return root.Value
        else
          return! waitForRenderedRoot
      }

    member _.RequestRender(viewSpec: ISimpleViewSpec, origin: Origin) =
      let request = { ViewSpec = viewSpec; Origin = origin }

      let mountInitial =
        lock lifecycleGate (fun () ->
          match lifecycle with
          | RenderLifecycle.AwaitingInitialRender ->
            lifecycle <- RenderLifecycle.MountingInitialRender
            true
          | RenderLifecycle.MountingInitialRender
          | RenderLifecycle.Running -> false
          | RenderLifecycle.Faulted ex -> raise (InvalidOperationException("The terminal renderer is faulted.", ex))
          | RenderLifecycle.Disposed -> raise (ObjectDisposedException(nameof TerminalRenderCoordinator)))

      if mountInitial then
        try
          commitRender request

          let shouldStartPump =
            lock lifecycleGate (fun () ->
              match lifecycle with
              | RenderLifecycle.MountingInitialRender ->
                lifecycle <- RenderLifecycle.Running
                true
              | RenderLifecycle.Disposed
              | RenderLifecycle.Faulted _ -> false
              | _ -> invalidOp "The initial render completed from an invalid lifecycle state.")

          if shouldStartPump then
            renderPumpTask <- runRenderPump ()
        with ex ->
          enterFaulted ex
          raise ex
      elif not (renderRequests.Writer.TryWrite request) then
        lock lifecycleGate (fun () ->
          match lifecycle with
          | RenderLifecycle.Faulted ex -> raise (InvalidOperationException("The terminal renderer is faulted.", ex))
          | _ -> raise (ObjectDisposedException(nameof TerminalRenderCoordinator)))

    member _.Dispose() =
      let shouldDispose =
        lock lifecycleGate (fun () ->
          match lifecycle with
          | RenderLifecycle.Disposed -> false
          | _ ->
            lifecycle <- RenderLifecycle.Disposed
            true)

      if shouldDispose then
        renderRequests.Writer.TryComplete() |> ignore
        shutdown.Cancel()

        try
          renderPumpTask.GetAwaiter().GetResult()
        with
        | :? OperationCanceledException -> ()
        | _ -> ()

        failRenderWaiters (ObjectDisposedException(nameof TerminalRenderCoordinator))
        lock commitGate (fun () -> treeRenderer.Dispose())
        shutdown.Dispose()

  /// <summary>
  /// <p>Internal model of the Elmish loop. This model is not exposed to the library caller.</p>
  /// <p>It is used internally to manage the state of the terminal elements and the application.</p>
  /// <param name="ClientModel">Elmish model provided to the Program by the library caller.</param>
  /// </summary>
  type internal TerminalModel<'model>(sharedContext: TerminalRenderContext, kind: ProgramKind, clientModel: 'model) =
    let renderCoordinator = TerminalRenderCoordinator(sharedContext)

    member val ClientModel = clientModel with get, set
    member _.SharedRenderContext = sharedContext
    member _.Application = sharedContext.Application
    member this.Kind = kind
    member this.RenderCoordinator = renderCoordinator

    member this.Dispose() = renderCoordinator.Dispose()

    interface IDisposable with
      member this.Dispose() = this.Dispose()


  module internal OuterModel =
    let internal wrapInit
      renderContextFactory
      origin
      (init: 'arg -> 'model * Cmd<TerminalMsg<'msg>>)
      : 'arg -> TerminalModel<'model> * Cmd<TerminalMsg<'msg>> =
      fun (arg: 'arg) ->
        let innerModel, cmd = init arg

        let terminalModel =
          new TerminalModel<_>(renderContextFactory (), origin, innerModel)

        terminalModel, cmd

    let internal wrapUpdate
      (update: 'msg -> 'model -> 'model * Cmd<TerminalMsg<'msg>>)
      : TerminalMsg<'msg> -> TerminalModel<'model> -> TerminalModel<'model> * Cmd<TerminalMsg<'msg>> =
      fun (msg: TerminalMsg<'msg>) (model: TerminalModel<'model>) ->
        match msg with
        | Terminate -> model, Cmd.none
        | Msg msg ->
          let innerModel, cmd = update msg model.ClientModel

          model.ClientModel <- innerModel
          model, cmd

    let internal wrapView<'model, 'msg, 'view when 'view :> IView>
      (view: 'model -> Dispatch<TerminalMsg<'msg>> -> 'view)
      : TerminalModel<'model> -> Dispatch<TerminalMsg<'msg>> -> IView =
      fun (model: TerminalModel<'model>) (dispatch: Dispatch<TerminalMsg<'msg>>) -> view model.ClientModel dispatch

    let internal wrapSimpleInit renderContextFactory programKind (init: 'arg -> 'model) =
      fun (arg: 'arg) ->
        let innerModel = init arg

        let terminalModel =
          new TerminalModel<_>(renderContextFactory (), programKind, innerModel)

        terminalModel

    let internal wrapSimpleUpdate
      (update: 'msg -> 'model -> 'model)
      : TerminalMsg<'msg> -> TerminalModel<'model> -> TerminalModel<'model> =
      fun (msg: TerminalMsg<'msg>) (model: TerminalModel<'model>) ->
        match msg with
        | Terminate -> model
        | Msg msg ->
          let innerModel = update msg model.ClientModel

          model.ClientModel <- innerModel
          model

    let internal wrapSubscribe (subscribe: 'model -> Sub<'msg>) : TerminalModel<'model> -> _ =
      fun outerModel -> subscribe outerModel.ClientModel |> Sub.map "WrapSubscribe" TerminalMsg.ofMsg

  type ElmishTerminalProgram<'arg, 'model, 'msg, 'view> =
    internal | ElmishTerminalProgram of Program<'arg, TerminalModel<'model>, TerminalMsg<'msg>, 'view>

  let private setState
    (view: TerminalModel<'model> -> Dispatch<TerminalMsg<'cmd>> -> IView)
    (model: TerminalModel<'model>)
    dispatch
    =
    task {
      let origin =
        match model.Kind with
        | ProgramKind.Root -> Origin.Root
        | ProgramKind.ElmishComponent te -> Origin.ElmishComponent te

      let nextSpec = view model dispatch :?> ISimpleViewSpec
      model.RenderCoordinator.RequestRender(nextSpec, origin)

      ()
    }
    |> Task.wait

  let internal terminate (model: TerminalModel<_>) =
    match model.Kind with
    | ProgramKind.Root ->
      // For the main elmish loop, signal stop and let runTerminal handle cleanup after Run() returns
      model.Application.RequestStop()
    | ProgramKind.ElmishComponent _ -> model.Dispose()

  type internal Subscription<'model, 'msg> =
    { SubId: SubId
      SubscriptionFunc: TerminalModel<'model> -> Subscribe<TerminalMsg<'msg>> }

  /// <summary>
  /// <para>Wrapper that elmish components should use to expose themselves as IInternalTerminalElement.</para>
  /// <para>As the Elmish component handles its own initialization and children management in his separate Elmish loop,
  /// this wrapper will hide these aspects to the outside world. Thus preventing double initialization or double children management.</para>
  ///
  /// <remarks>
  /// <b>The root TE of the component should remain the same across renders, so it's advisable to have a top Runnable TE as the root of the component.</b>
  /// </remarks>
  /// </summary>
  type internal ElmishComponentTE<'model, 'msg, 'view when 'view :> IView>
    (
      props: ComponentProps,
      init: unit -> 'model,
      update: 'msg -> 'model -> 'model,
      view: 'model -> Dispatch<TerminalMsg<'msg>> -> 'view
    ) =

    let initialTeTcs: TaskCompletionSource<IViewTE> = TaskCompletionSource<_>()

    let componentTerminatedTcs =
      TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously)

    let mutable resolvedTerminalElement: IElmishComponentTE option = None
    let mutable terminateComponent: (unit -> unit) option = None
    let mutable started = 0
    let mutable disposing = 0

    let viewSetEvent = Event<View>()

    static member mkSimpleComponent<'arg, 'model, 'msg, 'view when 'view :> IView>
      (terminalElement: IElmishComponentTE)
      (sharedContext: TerminalRenderContext)
      (init: 'arg -> 'model)
      (update: 'cmd -> 'model -> 'model)
      (view: 'model -> Dispatch<TerminalMsg<'cmd>> -> 'view)
      =
      let wrapComponentInit arg =
        let innerModel = init arg

        new TerminalModel<_>(sharedContext, ProgramKind.ElmishComponent terminalElement, innerModel)

      Program.mkSimple wrapComponentInit (OuterModel.wrapSimpleUpdate update) (OuterModel.wrapView view)
      |> Program.withSetState (setState (OuterModel.wrapView view))
      |> ElmishTerminalProgram

    abstract Subscriptions: Subscription<'model, 'msg> list

    default this.Subscriptions =
      // TODO: could be refactored into a ElmishTerminal.runTerminal
      let runComponent (model: TerminalModel<_>) =
        let start dispatch =
          task {
            let! rootView = model.RenderCoordinator.WaitForInitialRootViewAsync()

            viewSetEvent.Trigger rootView

            let! currentRoot = model.RenderCoordinator.GetCurrentRootAsync()
            initialTeTcs.SetResult(currentRoot)
          }
          |> Task.wait

          { new IDisposable with
              member _.Dispose() = () }

        start

      [ { SubId = [ "runComponent" ]
          SubscriptionFunc = runComponent } ]

    member private this.RunComponent(componentProgram: ElmishTerminalProgram<unit, 'model, 'msg, IView>) =
      let (ElmishTerminalProgram program) = componentProgram

      let captureTerminationDispatch (_: TerminalModel<_>) =
        let start dispatch =
          terminateComponent <- Some(fun () -> dispatch Terminate)

          { new IDisposable with
              member _.Dispose() = terminateComponent <- None }

        start

      let subscribe model : Sub<TerminalMsg<'msg>> =
        [ yield [ "componentTermination" ], captureTerminationDispatch model

          yield!
            this.Subscriptions
            |> List.map (fun sub -> sub.SubId, sub.SubscriptionFunc model) ]

      let onTermination model =
        try
          terminate model
        finally
          componentTerminatedTcs.TrySetResult() |> ignore

      program
      |> Program.withSubscription subscribe
      |> Program.withTermination
        (function
        | Terminate -> true
        | Msg _ -> false)
        onTermination
      |> Program.run

      initialTeTcs.Task |> Task.wait |> ignore

      ()

    member this.GetViewAsync() =
      task {
        let! te = initialTeTcs.Task
        return te.View
      }

    member this.View =
      if initialTeTcs.Task.IsCompleted then
        initialTeTcs.Task.Result.View
      else
        failwith "Elmish loop has not been started yet. Call StartElmishLoop before accessing the View property."

    member this.Child =
      if initialTeTcs.Task.IsCompleted then
        initialTeTcs.Task.Result
      else
        failwith "Elmish loop has not been started yet. Call StartElmishLoop before accessing the Child property."

    member val Origin = Unchecked.defaultof<_> with get, set

    [<CLIEvent>]
    member this.OnViewSet = viewSetEvent.Publish

    member this.Dispose() =
      if Interlocked.Exchange(&disposing, 1) = 0 then
        match terminateComponent with
        | Some terminate ->
          terminate ()
          componentTerminatedTcs.Task.GetAwaiter().GetResult()
        | None when initialTeTcs.Task.IsCompletedSuccessfully -> initialTeTcs.Task.Result.Dispose()
        | None -> ()

    interface IElmishComponentTE with
      member this.StartElmishLoop(sharedContext) =
        if Interlocked.Exchange(&started, 1) = 0 then
          ElmishComponentTE<'model, 'msg, 'view>.mkSimpleComponent this sharedContext init update view
          |> this.RunComponent

      member this.Child = this.Child

      member _.UpdateProps(newProps) = props.UpdateFrom newProps

    interface IView

    interface IComponentViewSpec with
      member _.ComponentProps = props

      member this.ComponentType = this.GetType()

      member this.ResolveTerminalElement() =
        resolvedTerminalElement |> Option.defaultValue (this :> IElmishComponentTE)

      member _.BindTerminalElement(value) = resolvedTerminalElement <- Some value

    interface ITerminalElementBase with
      member this.View = this.View
      member this.Name = props.ComponentName
      member this.OnViewSet = this.OnViewSet

      member this.Origin
        with get () = this.Origin
        and set v = this.Origin <- v

      member this.GetPath() =
        this.Origin |> Origin.getPath props.ComponentName

      member this.Dispose() = this.Dispose()

  let mkSimpleComponent<'model, 'msg, 'view when 'view :> IView>
    props
    (init: unit -> 'model)
    (update: 'msg -> 'model -> 'model)
    (view: 'model -> Dispatch<TerminalMsg<'msg>> -> 'view)
    =
    new ElmishComponentTE<'model, 'msg, 'view>(props, init, update, view) :> IView

  let internal mkProgramWithRenderContext<'arg, 'model, 'msg, 'view when 'view :> IView>
    renderContextFactory
    (init: 'arg -> 'model * Cmd<TerminalMsg<'msg>>)
    (update: 'msg -> 'model -> 'model * Cmd<TerminalMsg<'msg>>)
    (view: 'model -> Dispatch<TerminalMsg<'msg>> -> 'view)
    =
    Program.mkProgram
      (OuterModel.wrapInit renderContextFactory ProgramKind.Root init)
      (OuterModel.wrapUpdate update)
      (OuterModel.wrapView view)
    |> Program.withSetState (setState (OuterModel.wrapView view))
    |> ElmishTerminalProgram

  let mkProgram<'arg, 'model, 'msg, 'view when 'view :> IView>
    (init: 'arg -> 'model * Cmd<TerminalMsg<'msg>>)
    (update: 'msg -> 'model -> 'model * Cmd<TerminalMsg<'msg>>)
    (view: 'model -> Dispatch<TerminalMsg<'msg>> -> 'view)
    =
    mkProgramWithRenderContext TerminalRenderContext.createProduction init update view

  let internal mkSimpleWithRenderContext
    renderContextFactory
    (init: 'arg -> 'model)
    (update: 'cmd -> 'model -> 'model)
    (view: 'model -> Dispatch<TerminalMsg<'cmd>> -> IView)
    =
    Program.mkSimple
      (OuterModel.wrapSimpleInit renderContextFactory ProgramKind.Root init)
      (OuterModel.wrapSimpleUpdate update)
      (OuterModel.wrapView view)
    |> Program.withSetState (setState (OuterModel.wrapView view))
    |> ElmishTerminalProgram

  let mkSimple
    (init: 'arg -> 'model)
    (update: 'cmd -> 'model -> 'model)
    (view: 'model -> Dispatch<TerminalMsg<'cmd>> -> IView)
    =
    mkSimpleWithRenderContext TerminalRenderContext.createProduction init update view

  let withSubscription (subscribe: 'model -> Sub<'msg>) (ElmishTerminalProgram program) =
    program
    |> Program.withSubscription (OuterModel.wrapSubscribe subscribe)
    |> ElmishTerminalProgram

  let withTermination predicate (ElmishTerminalProgram program) = program |> ElmishTerminalProgram

  let runTerminal (ElmishTerminalProgram program) =

    let applicationStopped = TaskCompletionSource() // Rethrow any exception from the application thread.

    let runTerminal (model: TerminalModel<_>) =
      let start dispatch =
        task {
          let! rootView = model.RenderCoordinator.WaitForInitialRootViewAsync()

          if model.Kind.IsElmishComponent then
            failwith (
              "`run` is meant to be used for Terminal Elmish loop. "
              + "For Terminal components with separate Elmish loop, use `runComponent`."
            )
          else
            Task.Run(fun () ->
              (try
                try
                  model.Application.Init() |> ignore
                  model.SharedRenderContext.RenderDispatcher.Activate()
                  // Run return after Application.RequestStop is called in terminate.
                  model.Application.Run(rootView :?> Runnable) |> ignore
                finally
                  model.Dispose()
                  model.SharedRenderContext.RenderDispatcher.Dispose()
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
