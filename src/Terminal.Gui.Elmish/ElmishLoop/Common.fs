module internal Terminal.Gui.Elmish.ElmishLoop.Common

open Elmish
open System.Threading.Tasks
open Terminal.Gui.Elmish
open Terminal.Gui.ViewBase

type internal TerminalElementState() =

  let virtualTerminalTree = VirtualTerminalTree()

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

// TODO: is this still used ???
type internal ProgramKind =
  /// Main elmish program.
  | Main

  /// Elmish component with its own elmish loop, nested inside a Root program or another Elmish component.
  | ElmishComponent of IElmishComponentTE

type internal ITerminalModel<'model> =
  abstract RootViewSet: bool
  abstract Kind: ProgramKind
  abstract TerminalElementState: TerminalElementState

// TODO: Only used by the component loop.
type internal Subscription<'model, 'msg> =
  { SubId: SubId
    SubscriptionFunc: 'model -> Subscribe<'msg> }

// let internal setState
//   (view: ITerminalModel<'model> -> Dispatch<'cmd> -> ITerminalElement)
//   (model: ITerminalModel<'model>)
//   dispatch
//   =
//   task {
//     let nextTe =
//       task {
//         if not model.RootViewSet then
//
//           let initialTe = view model dispatch :?> IViewTE
//
//           let origin =
//             match model.Kind with
//             | ProgramKind.Root -> [ AddressSegment.Root ]
//             | ProgramKind.ElmishComponent te ->
//               // Each ElmishComponent has its own VTT; the child is always the root of that VTT.
//               // Set parentPath so the child tree inherits the component's hierarchy path.
//               [ AddressSegment.Root ]
//
//           initialTe.InitializeTree origin model.TerminalElementState.VTT
//
//           return initialTe
//
//         else
//           let! (currentTe: IViewTE) = model.TerminalElementState.GetCurrentTEAsync()
//
//           let nextTe = view model dispatch :?> IViewTE
//
//           Differ.update
//             model.TerminalElementState.VTT
//             (TerminalElement.ViewTE currentTe)
//             (TerminalElement.ViewTE nextTe)
//
//           currentTe.Dispose()
//           return nextTe
//       }
//
//     let! nextTe = nextTe
//     model.TerminalElementState.SetCurrentTE nextTe
//
//     ()
//   }
//   |> Task.wait

let inline internal setState<'model, 'cmd, ^terminalModel when ^terminalModel :> ITerminalModel<'model>>
  (view: ^terminalModel -> Dispatch<'cmd> -> ITerminalElement)
  (model: ^terminalModel)
  dispatch
  =
  task {
    let nextTe =
      task {
        if not model.RootViewSet then

          let initialTe = view model dispatch :?> IViewTE

          let origin =
            match model.Kind with
            | ProgramKind.Main -> [ AddressSegment.Root ]
            | ProgramKind.ElmishComponent te ->
              // Each ElmishComponent has its own VTT; the child is always the root of that VTT.
              // Set parentPath so the child tree inherits the component's hierarchy path.
              [ AddressSegment.Root ]

          initialTe.InitializeTree origin model.TerminalElementState.VTT

          return initialTe

        else
          let! (currentTe: IViewTE) = model.TerminalElementState.GetCurrentTEAsync()

          let nextTe = view model dispatch :?> IViewTE

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
