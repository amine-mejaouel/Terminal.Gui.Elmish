namespace Terminal.Gui.Elmish.Benchmarks

open System
open System.Collections.Generic
open System.Threading
open System.Threading.Tasks
open Terminal.Gui.Elmish
open Terminal.Gui.ViewBase

[<RequireQualifiedAccess>]
module internal BenchmarkTrees =

  let asSimpleSpec (view: IView) = view :?> ISimpleViewSpec

  let private label keyed index text : IView =
    View.Label(fun p ->
      if keyed then
        p.Key $"item-{index}"

      p.Text text)
    :> IView

  let private button keyed index text : IView =
    View.Button(fun p ->
      if keyed then
        p.Key $"item-{index}"

      p.Text text)
    :> IView

  let root (children: IView seq) =
    View.Runnable(fun (p: RunnableProps) -> p.Children(List.ofSeq children)) :> IView

  let flatRoot keyed (order: int array) (textFor: int -> string) (isButton: int -> bool) =
    order
    |> Seq.map (fun index ->
      if isButton index then
        button keyed index (textFor index)
      else
        label keyed index (textFor index))
    |> root

  let labels keyed count revision =
    flatRoot keyed [| 0 .. count - 1 |] (fun index -> $"item-{index}-v{revision}") (fun _ -> false)

  let buttons keyed count revision =
    flatRoot keyed [| 0 .. count - 1 |] (fun index -> $"item-{index}-v{revision}") (fun _ -> true)

  let order count = [| 0 .. count - 1 |]

  let rotatedOrder count =
    if count <= 1 then
      order count
    else
      Array.append [| 1 .. count - 1 |] [| 0 |]

  let reversedOrder count = order count |> Array.rev

  let shuffledOrder count =
    let values = order count
    let random = Random(7319 + count)

    for index = values.Length - 1 downto 1 do
      let swapIndex = random.Next(index + 1)
      let value = values[index]
      values[index] <- values[swapIndex]
      values[swapIndex] <- value

    values

  let reorderedLabels order =
    flatRoot true order (fun index -> string index) (fun _ -> false)

  let private balancedChild key path depth revision : IView =
    let rec build childKey currentPath remainingDepth =
      if remainingDepth = 0 then
        View.Label(fun p ->
          p.Key childKey
          p.Text $"{currentPath}-v{revision}")
        :> IView
      else
        let children =
          [ for index = 0 to 3 do
              let nextPath = $"{currentPath}.{index}"
              yield build (string index) nextPath (remainingDepth - 1) ]

        View.FrameView(fun p ->
          p.Key childKey
          p.Title currentPath
          p.Children children)
        :> IView

    build key path depth

  let balancedRoot depth revision =
    root [ balancedChild "tree" "tree" depth revision ]

  let slotRoot revision =
    root
      [ View.Shortcut(fun p ->
          p.Key "shortcut"

          p.TargetView(
            View.Label(fun p ->
              p.Key "target"
              p.Text $"target-v{revision}")
          ))
        :> IView ]

  let render (renderer: VirtualTree.Renderer) (view: IView) =
    renderer.Render(asSimpleSpec view, Origin.Root)

  let renderIgnore renderer view = render renderer view |> ignore

  let assertRootOrder (expected: int array) (root: IViewTE) =
    let actual =
      root.View.SubViews
      |> Seq.map (fun view -> Int32.Parse(view.Text.ToString()))
      |> Seq.toArray

    if actual <> expected then
      invalidOp $"Benchmark fixture produced order {actual}, expected {expected}."

type internal ManualRenderDispatcher() =
  let gate = obj ()
  let scheduled = new SemaphoreSlim(0)
  let mutable pending: (Action * TaskCompletionSource) option = None
  let mutable disposed = 0

  member _.WaitUntilScheduledAsync(cancellationToken: CancellationToken) = scheduled.WaitAsync(cancellationToken)

  member _.ExecuteOne() =
    let work =
      lock gate (fun () ->
        match pending with
        | Some value ->
          pending <- None
          value
        | None -> invalidOp "No render dispatch is pending.")

    let action, completion = work

    try
      action.Invoke()
      completion.TrySetResult() |> ignore
    with ex ->
      completion.TrySetException(ex) |> ignore
      raise ex

  member _.Dispose() =
    if Interlocked.Exchange(&disposed, 1) = 0 then
      let completion =
        lock gate (fun () ->
          let value = pending |> Option.map snd
          pending <- None
          value)

      completion
      |> Option.iter (fun value ->
        value.TrySetException(ObjectDisposedException(nameof ManualRenderDispatcher))
        |> ignore)

      scheduled.Dispose()

  interface IRenderDispatcher with
    member _.Activate() = ()

    member _.DispatchAsync(action, cancellationToken) =
      if Volatile.Read(&disposed) <> 0 then
        Task.FromException(ObjectDisposedException(nameof ManualRenderDispatcher))
      else
        let completion =
          TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously)

        lock gate (fun () ->
          if pending.IsSome then
            invalidOp "Only one render dispatch may be pending in the benchmark harness."

          pending <- Some(action, completion))

        scheduled.Release() |> ignore

        task {
          use _registration =
            cancellationToken.Register(fun () -> completion.TrySetCanceled(cancellationToken) |> ignore)

          do! completion.Task
        }

    member this.Dispose() = this.Dispose()
