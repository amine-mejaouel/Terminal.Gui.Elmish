[<AutoOpen>]
module internal Terminal.Gui.Elmish.Common

open Elmish
open System.Threading.Tasks
open Terminal.Gui.Elmish
open Terminal.Gui.ViewBase

type internal TerminalElementState(?vtt: IVirtualTerminalTree) =

  let virtualTerminalTree = defaultArg vtt (VirtualTerminalTree())

  let mutable _currentTe: IViewTE option = None
  let mutable nextTeTcs: TaskCompletionSource<IViewTE> = TaskCompletionSource<_>()
  let rootViewTcs: TaskCompletionSource<View> = TaskCompletionSource<View>()

  // TODO: should be moved to the TerminalModel
  member this.VTT = virtualTerminalTree

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

type internal ITerminalModel<'model> =
  abstract RootViewSet: bool
  abstract TerminalElementState: TerminalElementState
  abstract Address: Address
  abstract ClientModel: 'model

// TODO: Only used by the component loop.
type internal Subscription<'model, 'msg> =
  { SubId: SubId
    SubscriptionFunc: 'model -> Subscribe<'msg> }

let inline internal wrapView
  (view: 'model -> Dispatch<'msg> -> 'terminalElement)
  : ITerminalModel<'model> -> Dispatch<'msg> -> 'terminalElement =
  fun (model: ITerminalModel<'model>) (dispatch: Dispatch<'msg>) -> view model.ClientModel dispatch

let inline internal setState<'model, 'cmd, ^terminalModel, ^terminalElement
  when ^terminalModel :> ITerminalModel<'model> and ^terminalElement :> ITerminalElement>
  ([<InlineIfLambda>] view: 'model -> Dispatch<'cmd> -> ^terminalElement)
  (model: ^terminalModel)
  dispatch
  =
  task {
    let nextTe =
      task {
        let view = wrapView view

        if not model.RootViewSet then

          // TODO: double view evaluation, as view is already called by elmish loop
          // TODO: this should vanish once VTT is done.
          let initialTe =
            (view model dispatch :> ITerminalElement :?> IViewDescriptor).CreateViewTE()

          initialTe.InitializeTree model.Address model.TerminalElementState.VTT

          return initialTe

        else
          let! (currentTe: IViewTE) = model.TerminalElementState.GetCurrentTEAsync()

          // TODO: double view evaluation, as view is already called by elmish loop
          let nextTe = view model dispatch :> ITerminalElement :?> IViewTE

          Differ.update
            model.TerminalElementState.VTT
            (TerminalElement.ViewTE currentTe)
            (TerminalElement.ViewTE nextTe)

          currentTe.Dispose()
          return nextTe
      }

    let! nextTe = nextTe
    model.TerminalElementState.SetCurrentTE nextTe

    ()
  }
  |> Task.wait
