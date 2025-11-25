module Terminal.Gui.Elmish.ElmishComponent

open System
open System.Threading.Tasks
open Elmish

type internal OuterModel<'model> = {
  mutable CurrentTreeState: IInternalTerminalElement option
  TopView: TaskCompletionSource<Terminal.Gui.ViewBase.View>
  InnerModel: 'model
}

module OuterModel =
  let internal wrapInit (init: 'arg -> 'model * Cmd<'msg>) =
    fun (arg: 'arg) ->
      let innerModel, cmd = init arg

      let internalModel = {
        CurrentTreeState = None
        TopView = TaskCompletionSource<Terminal.Gui.ViewBase.View>()
        InnerModel = innerModel
      }

      internalModel, cmd

  let internal wrapUpdate (update: 'msg -> 'model -> 'model * Cmd<'msg>) : 'msg -> OuterModel<'model> -> OuterModel<'model> * Cmd<'msg> =
    fun (msg: 'msg) (model: OuterModel<'model>) ->
      let innerModel, cmd =
        update msg model.InnerModel

      { model with InnerModel = innerModel }, cmd

  let internal wrapView (view: 'model -> Dispatch<'msg> -> ITerminalElement) : OuterModel<'model> -> Dispatch<'msg> -> ITerminalElement =
    fun (model: OuterModel<'model>) (dispatch: Dispatch<'msg>) -> view model.InnerModel dispatch

  let internal wrapSimpleInit (init: 'arg -> 'model) =
    fun (arg: 'arg) ->
      let innerModel = init arg

      let internalModel = {
        CurrentTreeState = None
        TopView = TaskCompletionSource<Terminal.Gui.ViewBase.View>()
        InnerModel = innerModel
      }

      internalModel

  let internal wrapSimpleUpdate (update: 'msg -> 'model -> 'model) : 'msg -> OuterModel<'model> -> OuterModel<'model> =
    fun (msg: 'msg) (model: OuterModel<'model>) ->
      let innerModel = update msg model.InnerModel
      { model with InnerModel = innerModel }

  let internal wrapSubscribe (subscribe: 'model -> _) : OuterModel<'model> -> _ =
    fun outerModel -> subscribe outerModel.InnerModel

type ElmishComponent<'arg, 'model, 'msg, 'view> = private ElmishComponent of Program<'arg, OuterModel<'model>, 'msg, 'view>

let private setState (view: OuterModel<'model> -> Dispatch<'cmd> -> ITerminalElement) (model: OuterModel<'model>) dispatch =

  let nextTreeState =
    match model.CurrentTreeState with
    | None ->

      let startState =
        view model dispatch :?> IInternalTerminalElement

      startState.initializeTree None

      match startState.view with
      | null -> failwith ("error state not initialized")
      | topElement ->
        model.TopView.SetResult(topElement)

      startState

    | Some currentState ->
      let nextTreeState =
        view model dispatch :?> IInternalTerminalElement

      Differ.update currentState nextTreeState
      nextTreeState

  model.CurrentTreeState <- Some nextTreeState
  nextTreeState.layout ()

  ()

let mkProgram (init: 'arg -> 'model * Cmd<'msg>) (update: 'msg -> 'model -> 'model * Cmd<'msg>) (view: 'model -> Dispatch<'msg> -> ITerminalElement) =
  Program.mkProgram (OuterModel.wrapInit init) (OuterModel.wrapUpdate update) (OuterModel.wrapView view)
  |> Program.withSetState (setState (OuterModel.wrapView view))
  |> ElmishComponent

let mkSimple (init: 'arg -> 'model) (update: 'cmd -> 'model -> 'model) (view: 'model -> Dispatch<'cmd> -> ITerminalElement) =
  Program.mkSimple (OuterModel.wrapSimpleInit init) (OuterModel.wrapSimpleUpdate update) (OuterModel.wrapView view)
  |> Program.withSetState (setState (OuterModel.wrapView view))
  |> ElmishComponent

let withSubscription (subscribe: 'model -> Sub<_>) (ElmishComponent program) =
  program
  |> Program.withSubscription (OuterModel.wrapSubscribe subscribe)
  |> ElmishComponent

let run (ElmishComponent program) : ITerminalElement =

  let waitForView = TaskCompletionSource()
  let mutable view: ITerminalElement = Unchecked.defaultof<_>

  let run (model: OuterModel<_>) =
    let start dispatch =
      task {
        let! topView = model.TopView.Task
        view <- model.CurrentTreeState.Value :> ITerminalElement
        waitForView.SetResult()
      }
      |> ignore

      { new IDisposable with
          // TODO: implement disposal
          member _.Dispose() = ()
      }

    start

  let subscribe model = [ [ "run" ], run model ]

  program
  |> Program.withSubscription subscribe
  |> Program.run

  waitForView.Task.GetAwaiter().GetResult()
  view



