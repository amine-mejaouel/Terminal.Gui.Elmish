namespace Terminal.Gui.Elmish

open System
open System.Threading.Tasks
open Elmish
open Terminal.Gui.App
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

  type internal TerminalElementState() =
    let mutable _currentTe: IViewTE option = None
    let mutable nextTeTcs: TaskCompletionSource<IViewTE> = TaskCompletionSource<_>()
    let rootViewTcs: TaskCompletionSource<View> = TaskCompletionSource<View>()

    member this.RootViewSet = rootViewTcs.Task.IsCompletedSuccessfully

    member this.WaitTillRootViewIsSetAsync() = rootViewTcs.Task

    member this.WaitForNextTerminalElementAsync() = nextTeTcs.Task

    member this.GetCurrentTEAsync() : Task<IViewTE> =
      task {
        let waitForNextTeTask = this.WaitForNextTerminalElementAsync()

        if _currentTe.IsSome then
          return _currentTe.Value
        else
          return! waitForNextTeTask
      }

    member this.SetCurrentTE(te: IViewTE) =
      _currentTe <- Some te

      if rootViewTcs.Task.IsCompletedSuccessfully |> not then
        rootViewTcs.SetResult te.View

      nextTeTcs.SetResult(te)
      nextTeTcs <- TaskCompletionSource<_>()

    member this.Dispose() = _currentTe |> Option.iter _.Dispose()

  /// <summary>
  /// <p>Internal model of the Elmish loop. This model is not exposed to the library caller.</p>
  /// <p>It is used internally to manage the state of the terminal elements and the application.</p>
  /// <param name="ClientModel">Elmish model provided to the Program by the library caller.</param>
  /// </summary>
  type internal TerminalModel<'model>(application: IApplication, kind: ProgramKind, clientModel: 'model) =
    let terminalElementState = TerminalElementState()

    member val ClientModel = clientModel with get, set
    member this.Application = application
    member this.Kind = kind
    member this.RootViewSet = terminalElementState.RootViewSet
    member this.TerminalElementState = terminalElementState

    member this.Dispose() = terminalElementState.Dispose()

    interface IDisposable with
      member this.Dispose() = this.Dispose()


  module internal OuterModel =
    let internal wrapInit
      origin
      (init: 'arg -> 'model * Cmd<TerminalMsg<'msg>>)
      : 'arg -> TerminalModel<'model> * Cmd<TerminalMsg<'msg>> =
      fun (arg: 'arg) ->
        let innerModel, cmd = init arg

        let terminalModel = new TerminalModel<_>(Application.Create(), origin, innerModel)

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

    let internal wrapSimpleInit programKind (init: 'arg -> 'model) =
      fun (arg: 'arg) ->
        let innerModel = init arg

        let terminalModel =
          new TerminalModel<_>(Application.Create(), programKind, innerModel)

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
      let nextTe =
        task {
          if not model.RootViewSet then

            if Config.curDiffer = Differ.Keyed then
              let initialTe = (view model dispatch :?> ISimpleViewSpec).CreateViewTE()

              let origin =
                match model.Kind with
                | ProgramKind.Root -> Origin.Root
                | ProgramKind.ElmishComponent te -> Origin.ElmishComponent te

              initialTe.InitializeTree origin

              return initialTe

            else
              let initialView = (view model dispatch :?> ISimpleViewSpec)

              return Unchecked.defaultof<_>

          else
            let! (currentTe: IViewTE) = model.TerminalElementState.GetCurrentTEAsync()

            let nextTe = (view model dispatch :?> ISimpleViewSpec).CreateViewTE()

            KeyedDiffer.update (TerminalElement.ViewTE currentTe) (TerminalElement.ViewTE nextTe)

            currentTe.Dispose()
            return nextTe
        }

      let! nextTe = nextTe
      model.TerminalElementState.SetCurrentTE nextTe

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

    let viewSetEvent = Event<View>()

    static member mkSimpleComponent<'arg, 'model, 'msg, 'view when 'view :> IView>
      (terminalElement: IElmishComponentTE)
      (init: 'arg -> 'model)
      (update: 'cmd -> 'model -> 'model)
      (view: 'model -> Dispatch<TerminalMsg<'cmd>> -> 'view)
      =
      Program.mkSimple
        (OuterModel.wrapSimpleInit (ProgramKind.ElmishComponent terminalElement) init)
        (OuterModel.wrapSimpleUpdate update)
        (OuterModel.wrapView view)
      |> Program.withSetState (setState (OuterModel.wrapView view))
      |> ElmishTerminalProgram

    abstract Subscriptions: Subscription<'model, 'msg> list

    default this.Subscriptions =
      // TODO: could be refactored into a ElmishTerminal.runTerminal
      let runComponent (model: TerminalModel<_>) =
        let start dispatch =
          task {
            let! rootView = model.TerminalElementState.WaitTillRootViewIsSetAsync()

            viewSetEvent.Trigger rootView

            let! currentTe = model.TerminalElementState.GetCurrentTEAsync()
            initialTeTcs.SetResult(currentTe)
          }
          |> Task.wait

          { new IDisposable with
              member _.Dispose() = () }

        start

      [ { SubId = [ "runComponent" ]
          SubscriptionFunc = runComponent } ]

    member private this.RunComponent(ElmishTerminalProgram program) =

      let subscribe model : Sub<TerminalMsg<'msg>> =

        this.Subscriptions
        |> List.map (fun sub -> sub.SubId, sub.SubscriptionFunc model)

      program |> Program.withSubscription subscribe |> Program.run

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
      task {
        let! te = initialTeTcs.Task
        te.Dispose()
      }
      |> Task.wait

    interface IElmishComponentTE with
      member this.StartElmishLoop() =
        ElmishComponentTE<'model, 'msg, 'view>.mkSimpleComponent this init update view
        |> this.RunComponent

      member this.Child = this.Child

    interface IView

    interface IComponentViewSpec with
      member this.Props = failwith "Not implemented yet"

      member this.InitComponentView() =
        (this :> IElmishComponentTE).StartElmishLoop()

        { new IComponentView with
            member _.Props = props
            member _.Update(props) = failwith "Not implemented yet" }

      member this.ClearInitComponentView() = failwith "Not implemented yet"

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

  let mkProgram<'arg, 'model, 'msg, 'view when 'view :> IView>
    (init: 'arg -> 'model * Cmd<TerminalMsg<'msg>>)
    (update: 'msg -> 'model -> 'model * Cmd<TerminalMsg<'msg>>)
    (view: 'model -> Dispatch<TerminalMsg<'msg>> -> 'view)
    =
    Program.mkProgram
      (OuterModel.wrapInit ProgramKind.Root init)
      (OuterModel.wrapUpdate update)
      (OuterModel.wrapView view)
    |> Program.withSetState (setState (OuterModel.wrapView view))
    |> ElmishTerminalProgram

  let mkSimple
    (init: 'arg -> 'model)
    (update: 'cmd -> 'model -> 'model)
    (view: 'model -> Dispatch<TerminalMsg<'cmd>> -> IView)
    =
    Program.mkSimple
      (OuterModel.wrapSimpleInit ProgramKind.Root init)
      (OuterModel.wrapSimpleUpdate update)
      (OuterModel.wrapView view)
    |> Program.withSetState (setState (OuterModel.wrapView view))
    |> ElmishTerminalProgram

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
          let! rootView = model.TerminalElementState.WaitTillRootViewIsSetAsync()

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
