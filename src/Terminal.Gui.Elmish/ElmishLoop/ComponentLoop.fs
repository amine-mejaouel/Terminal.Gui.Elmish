namespace Terminal.Gui.Elmish.ElmishLoop

open System
open System.Threading.Tasks
open Elmish
open Terminal.Gui.Elmish
open Terminal.Gui.Elmish.ElmishLoop.Common
open Terminal.Gui.ViewBase

module ComponentLoop =

  /// <summary>
  /// <p>Internal model of the Elmish loop. This model is not exposed to the library caller.</p>
  /// <p>It is used internally to manage the state of the terminal elements and the application.</p>
  /// <param name="ClientModel">Elmish model provided to the Program by the library caller.</param>
  /// </summary>
  type internal ComponentTerminalModel<'model>(vtt: VirtualTerminalTree, address: Address, clientModel: 'model) =
    // TODO: I prefer to keep vtt and address here, should check that last thing, when all errors are fixed.
    // let terminalElementState = TerminalElementState(vtt, address)
    let terminalElementState = TerminalElementState()

    member val ClientModel = clientModel with get, set
    member this.RootViewSet = terminalElementState.RootViewSet
    member this.TerminalElementState = terminalElementState

    member this.Dispose() = terminalElementState.Dispose()

    interface ITerminalModel<'model> with
      member this.RootViewSet = this.RootViewSet
      member this.TerminalElementState: TerminalElementState = this.TerminalElementState

    interface IDisposable with
      member this.Dispose() = this.Dispose()

  module internal OuterModel =

    let internal wrapView
      (view: 'model -> Dispatch<'msg> -> ITerminalElement)
      : ComponentTerminalModel<'model> -> Dispatch<'msg> -> ITerminalElement =
      fun (model: ComponentTerminalModel<'model>) (dispatch: Dispatch<'msg>) -> view model.ClientModel dispatch

    let internal wrapSimpleInit vtt address (init: 'arg -> 'model) =
      fun (arg: 'arg) ->
        let innerModel = init arg

        let terminalModel = new ComponentTerminalModel<_>(vtt, address, innerModel)

        terminalModel

    let internal wrapSimpleUpdate
      (update: 'msg -> 'model -> 'model)
      : 'msg -> ComponentTerminalModel<'model> -> ComponentTerminalModel<'model> =
      fun (msg: 'msg) (model: ComponentTerminalModel<'model>) ->
        let innerModel = update msg model.ClientModel

        model.ClientModel <- innerModel
        model

    let internal wrapSubscribe (subscribe: 'model -> Sub<'msg>) : ComponentTerminalModel<'model> -> _ =
      fun outerModel -> subscribe outerModel.ClientModel

  type ComponentTerminalProgram<'arg, 'model, 'msg, 'view> =
    internal | ComponentTerminalProgram of Program<'arg, ComponentTerminalModel<'model>, 'msg, 'view>

  /// <summary>
  /// <para>Wrapper that elmish components should use to expose themselves as IInternalTerminalElement.</para>
  /// <para>As the Elmish component handles its own initialization and children management in his separate Elmish loop,
  /// this wrapper will hide these aspects to the outside world. Thus preventing double initialization or double children management.</para>
  ///
  /// <remarks>
  /// <b>The root TE of the component should remain the same across renders, so it's advisable to have a top Runnable TE as the root of the component.</b>
  /// </remarks>
  /// </summary>
  type internal ElmishComponentTE<'model, 'msg, 'view>
    (name, init: unit -> 'model, update: 'msg -> 'model -> 'model, view: 'model -> Dispatch<'msg> -> ITerminalElement) =

    let initialTeTcs: TaskCompletionSource<IViewTE> = TaskCompletionSource<_>()

    let viewSetEvent = Event<View>()

    // let setState (view: 'model -> Dispatch<TerminalMsg<'cmd>> -> ITerminalElement) : (ComponentTerminalModel<'model> -> Dispatch<TerminalMsg<'cmd>> -> unit) =
    //   let wrapView (view: ComponentTerminalModel<'model> -> Dispatch<TerminalMsg<'cmd>> -> ITerminalElement) =
    //     fun (model: ITerminalModel<'model>) (dispatch: Dispatch<TerminalMsg<'cmd>>)  ->
    //       let model = model :?> ComponentTerminalModel<'model>
    //       (view model dispatch)
    //
    //   let x = (view |> OuterModel.wrapView)
    //   let y = Common.setState x
    //   y

    let mkSimpleComponent
      (terminalElement: IElmishComponentTE)
      (vtt: VirtualTerminalTree)
      (address: Address)
      (init: 'arg -> 'model)
      (update: 'cmd -> 'model -> 'model)
      (view: 'model -> Dispatch<'cmd> -> ITerminalElement)
      =
      Program.mkSimple
        (OuterModel.wrapSimpleInit vtt address init)
        (OuterModel.wrapSimpleUpdate update)
        (OuterModel.wrapView view)
      |> Program.withSetState (setState (view |> OuterModel.wrapView))
      |> ComponentTerminalProgram

    abstract Subscriptions: Subscription<ComponentTerminalModel<'model>, 'msg> list

    default this.Subscriptions =
      // TODO: could be refactored into a ElmishTerminal.runTerminal
      let runComponent (model: ComponentTerminalModel<_>) : Subscribe<'msg> =
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

    member private this.RunComponent(ComponentTerminalProgram program) =

      let subscribe model : Sub<'msg> =

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

    member val ParentViewField: View option = None with get, set

    member val ParentPathField: string = "root" with get, set

    [<CLIEvent>]
    member this.OnViewSet = viewSetEvent.Publish

    member this.Reuse(prev: IElmishComponentTE) =
      // An elmish component could have subscriptions
      // It is better to reuse the same component instance instead of creating a new one and reusing the view.
      // For that reason, ElmishComponents should have an ID to identify them and reuse the same instance if the ID is the same.
      ()

    member this.Dispose() =
      task {
        let! te = initialTeTcs.Task
        te.Dispose()
      }
      |> Task.wait

    interface IElmishComponentTE with
      member this.StartElmishLoop(vtt, address) =
        mkSimpleComponent this vtt address init update view |> this.RunComponent

      member this.Reuse prev = this.Reuse prev

      member this.Child = this.Child


    interface ITerminalElementBase with
      member this.View = this.View
      member this.Name = name
      member this.OnViewSet = this.OnViewSet

      member this.Address
        with get () = this.Origin
        and set v = this.Origin <- v

      member this.ParentView
        with get () = this.ParentViewField
        and set v = this.ParentViewField <- v

      member this.Dispose() = this.Dispose()

  let mkSimpleComponent
    name
    (init: unit -> 'model)
    (update: 'msg -> 'model -> 'model)
    (view: 'model -> Dispatch<'msg> -> ITerminalElement)
    =
    new ElmishComponentTE<_, _, _>(name, init, update, view) :> ITerminalElement
