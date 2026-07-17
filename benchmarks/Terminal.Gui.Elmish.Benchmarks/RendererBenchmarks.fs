namespace Terminal.Gui.Elmish.Benchmarks

open BenchmarkDotNet.Attributes
open BenchmarkDotNet.Diagnosers
open Terminal.Gui.Elmish

[<MemoryDiagnoser>]
type SteadyTreeReconciliationBenchmarks() =
  let mutable renderer = Unchecked.defaultof<VirtualTree.Renderer>
  let mutable baseline = Unchecked.defaultof<IView>
  let mutable equivalent = Unchecked.defaultof<IView>
  let mutable oneChanged = Unchecked.defaultof<IView>
  let mutable allChanged = Unchecked.defaultof<IView>

  [<Params(10, 100, 1000)>]
  member val Width = 0 with get, set

  [<GlobalSetup>]
  member this.Setup() =
    renderer <- new VirtualTree.Renderer()
    baseline <- BenchmarkTrees.labels true this.Width 0
    equivalent <- BenchmarkTrees.labels true this.Width 0

    oneChanged <-
      BenchmarkTrees.flatRoot
        true
        (BenchmarkTrees.order this.Width)
        (fun index ->
          if index = this.Width / 2 then
            $"item-{index}-changed"
          else
            $"item-{index}-v0")
        (fun _ -> false)

    allChanged <- BenchmarkTrees.labels true this.Width 1
    BenchmarkTrees.renderIgnore renderer baseline

  [<GlobalCleanup>]
  member _.Cleanup() = renderer.Dispose()

  [<Benchmark(Baseline = true, OperationsPerInvoke = 2)>]
  member _.EquivalentTreeCycle() =
    BenchmarkTrees.renderIgnore renderer equivalent
    BenchmarkTrees.renderIgnore renderer baseline

  [<Benchmark(OperationsPerInvoke = 2)>]
  member _.OneLeafChangedCycle() =
    BenchmarkTrees.renderIgnore renderer oneChanged
    BenchmarkTrees.renderIgnore renderer baseline

  [<Benchmark(OperationsPerInvoke = 2)>]
  member _.AllLeavesChangedCycle() =
    BenchmarkTrees.renderIgnore renderer allChanged
    BenchmarkTrees.renderIgnore renderer baseline

[<MemoryDiagnoser>]
type UnkeyedSteadyTreeReconciliationBenchmarks() =
  let mutable renderer = Unchecked.defaultof<VirtualTree.Renderer>
  let mutable baseline = Unchecked.defaultof<IView>
  let mutable equivalent = Unchecked.defaultof<IView>

  [<Params(10, 100, 1000)>]
  member val Width = 0 with get, set

  [<GlobalSetup>]
  member this.Setup() =
    renderer <- new VirtualTree.Renderer()
    baseline <- BenchmarkTrees.labels false this.Width 0
    equivalent <- BenchmarkTrees.labels false this.Width 0
    BenchmarkTrees.renderIgnore renderer baseline

  [<GlobalCleanup>]
  member _.Cleanup() = renderer.Dispose()

  [<Benchmark(OperationsPerInvoke = 2)>]
  member _.EquivalentTreeCycle() =
    BenchmarkTrees.renderIgnore renderer equivalent
    BenchmarkTrees.renderIgnore renderer baseline

[<MemoryDiagnoser>]
type BalancedTreeReconciliationBenchmarks() =
  let mutable renderer = Unchecked.defaultof<VirtualTree.Renderer>
  let mutable baseline = Unchecked.defaultof<IView>
  let mutable equivalent = Unchecked.defaultof<IView>
  let mutable changed = Unchecked.defaultof<IView>

  [<Params(2, 4, 5)>]
  member val Depth = 0 with get, set

  [<GlobalSetup>]
  member this.Setup() =
    renderer <- new VirtualTree.Renderer()
    baseline <- BenchmarkTrees.balancedRoot this.Depth 0
    equivalent <- BenchmarkTrees.balancedRoot this.Depth 0
    changed <- BenchmarkTrees.balancedRoot this.Depth 1
    BenchmarkTrees.renderIgnore renderer baseline

  [<GlobalCleanup>]
  member _.Cleanup() = renderer.Dispose()

  [<Benchmark(Baseline = true, OperationsPerInvoke = 2)>]
  member _.EquivalentTreeCycle() =
    BenchmarkTrees.renderIgnore renderer equivalent
    BenchmarkTrees.renderIgnore renderer baseline

  [<Benchmark(OperationsPerInvoke = 2)>]
  member _.AllPropertiesChangedCycle() =
    BenchmarkTrees.renderIgnore renderer changed
    BenchmarkTrees.renderIgnore renderer baseline

[<MemoryDiagnoser>]
type KeyedReorderBenchmarks() =
  let mutable renderer = Unchecked.defaultof<VirtualTree.Renderer>
  let mutable baseline = Unchecked.defaultof<IView>
  let mutable rotated = Unchecked.defaultof<IView>
  let mutable reversed = Unchecked.defaultof<IView>
  let mutable shuffled = Unchecked.defaultof<IView>

  [<Params(10, 100, 1000)>]
  member val Width = 0 with get, set

  [<GlobalSetup>]
  member this.Setup() =
    renderer <- new VirtualTree.Renderer()
    baseline <- BenchmarkTrees.reorderedLabels (BenchmarkTrees.order this.Width)
    rotated <- BenchmarkTrees.reorderedLabels (BenchmarkTrees.rotatedOrder this.Width)
    reversed <- BenchmarkTrees.reorderedLabels (BenchmarkTrees.reversedOrder this.Width)
    shuffled <- BenchmarkTrees.reorderedLabels (BenchmarkTrees.shuffledOrder this.Width)
    let mounted = BenchmarkTrees.render renderer baseline
    BenchmarkTrees.assertRootOrder (BenchmarkTrees.order this.Width) mounted

  [<GlobalCleanup>]
  member _.Cleanup() = renderer.Dispose()

  [<Benchmark(Baseline = true, OperationsPerInvoke = 2)>]
  member _.RotateCycle() =
    BenchmarkTrees.renderIgnore renderer rotated
    BenchmarkTrees.renderIgnore renderer baseline

  [<Benchmark(OperationsPerInvoke = 2)>]
  member _.ReverseCycle() =
    BenchmarkTrees.renderIgnore renderer reversed
    BenchmarkTrees.renderIgnore renderer baseline

  [<Benchmark(OperationsPerInvoke = 2)>]
  member _.DeterministicShuffleCycle() =
    BenchmarkTrees.renderIgnore renderer shuffled
    BenchmarkTrees.renderIgnore renderer baseline

[<MemoryDiagnoser>]
type KeyedStructuralChangeBenchmarks() =
  let mutable renderer = Unchecked.defaultof<VirtualTree.Renderer>
  let mutable currentBaseline = Unchecked.defaultof<IView>

  [<Params(10, 100, 1000)>]
  member val Width = 0 with get, set

  member private this.NewBaseline() = BenchmarkTrees.labels true this.Width 0

  member private this.RenderAndRefreshBaseline(next: IView) =
    BenchmarkTrees.renderIgnore renderer next
    currentBaseline <- this.NewBaseline()
    BenchmarkTrees.renderIgnore renderer currentBaseline

  [<GlobalSetup>]
  member this.Setup() =
    renderer <- new VirtualTree.Renderer()
    currentBaseline <- this.NewBaseline()
    BenchmarkTrees.renderIgnore renderer currentBaseline

  [<GlobalCleanup>]
  member _.Cleanup() = renderer.Dispose()

  [<Benchmark(Baseline = true, OperationsPerInvoke = 2)>]
  member this.AppendRemoveCycle() =
    let next = BenchmarkTrees.labels true (this.Width + 1) 0
    this.RenderAndRefreshBaseline next

  [<Benchmark(OperationsPerInvoke = 2)>]
  member this.PrependRemoveCycle() =
    let order = Array.append [| this.Width |] (BenchmarkTrees.order this.Width)
    let next = BenchmarkTrees.reorderedLabels order
    this.RenderAndRefreshBaseline next

  [<Benchmark(OperationsPerInvoke = 2)>]
  member this.RemoveReinsertMiddleCycle() =
    let middle = this.Width / 2
    let order = BenchmarkTrees.order this.Width |> Array.filter ((<>) middle)
    let next = BenchmarkTrees.reorderedLabels order
    this.RenderAndRefreshBaseline next

  [<Benchmark(OperationsPerInvoke = 2)>]
  member this.ReplaceMiddleCycle() =
    let middle = this.Width / 2

    let next =
      BenchmarkTrees.flatRoot true (BenchmarkTrees.order this.Width) (fun index -> $"item-{index}-v0") ((=) middle)

    this.RenderAndRefreshBaseline next

[<MemoryDiagnoser>]
type SlotReconciliationBenchmarks() =
  let mutable renderer = Unchecked.defaultof<VirtualTree.Renderer>
  let mutable baseline = Unchecked.defaultof<IView>
  let mutable changed = Unchecked.defaultof<IView>

  [<GlobalSetup>]
  member _.Setup() =
    renderer <- new VirtualTree.Renderer()
    baseline <- BenchmarkTrees.slotRoot 0
    changed <- BenchmarkTrees.slotRoot 1
    BenchmarkTrees.renderIgnore renderer baseline

  [<GlobalCleanup>]
  member _.Cleanup() = renderer.Dispose()

  [<Benchmark(OperationsPerInvoke = 2)>]
  member _.RetainedSlotPropertyChangeCycle() =
    BenchmarkTrees.renderIgnore renderer changed
    BenchmarkTrees.renderIgnore renderer baseline

[<MemoryDiagnoser; DisassemblyDiagnoser(maxDepth = 3)>]
type PropertyRemovalBenchmarks() =
  let baseValueRenderer = new VirtualTree.Renderer()
  let derivedValueRenderer = new VirtualTree.Renderer()
  let eventRenderer = new VirtualTree.Renderer()

  let rootWithButton configure =
    View.Runnable(fun (p: RunnableProps) ->
      p.Children
        [ View.Button(fun p ->
            p.Key "button"
            configure p)
          :> IView ])
    :> IView

  let withoutProps = rootWithButton ignore
  let withBaseValue = rootWithButton (fun p -> p.Title "benchmark")
  let withDerivedValue = rootWithButton (fun p -> p.NoPadding true)
  let withEvent = rootWithButton (fun p -> p.Accepting ignore)

  [<GlobalSetup>]
  member _.Setup() =
    BenchmarkTrees.renderIgnore baseValueRenderer withBaseValue
    BenchmarkTrees.renderIgnore derivedValueRenderer withDerivedValue
    BenchmarkTrees.renderIgnore eventRenderer withEvent

  [<GlobalCleanup>]
  member _.Cleanup() =
    baseValueRenderer.Dispose()
    derivedValueRenderer.Dispose()
    eventRenderer.Dispose()

  [<Benchmark(Baseline = true, OperationsPerInvoke = 2)>]
  member _.BaseValueRemoveAndReapply() =
    BenchmarkTrees.renderIgnore baseValueRenderer withoutProps
    BenchmarkTrees.renderIgnore baseValueRenderer withBaseValue

  [<Benchmark(OperationsPerInvoke = 2)>]
  member _.DerivedValueRemoveAndReapply() =
    BenchmarkTrees.renderIgnore derivedValueRenderer withoutProps
    BenchmarkTrees.renderIgnore derivedValueRenderer withDerivedValue

  [<Benchmark(OperationsPerInvoke = 2)>]
  member _.EventRemoveAndReapply() =
    BenchmarkTrees.renderIgnore eventRenderer withoutProps
    BenchmarkTrees.renderIgnore eventRenderer withEvent

[<MemoryDiagnoser; DisassemblyDiagnoser(maxDepth = 3)>]
type ClearDispatchBenchmarks() =
  let renderer = new VirtualTree.Renderer()
  let mutable button = Unchecked.defaultof<ViewBackedTerminalElement>
  let basePropertyId = PKey.View.Title.id
  let derivedPropertyId = PKey.Button.NoPadding.id

  [<GlobalSetup>]
  member _.Setup() =
    let root =
      View.Runnable(fun (p: RunnableProps) ->
        p.Children
          [ View.Button(fun p ->
              p.Key "button"
              p.Title "benchmark"
              p.NoPadding true)
            :> IView ])
      :> IView

    let mounted = BenchmarkTrees.render renderer root
    let child: TerminalElement = mounted.Children[0]

    match child with
    | TerminalElement.ViewTE viewTe -> button <- viewTe :?> ViewBackedTerminalElement
    | TerminalElement.ElmishComponentTE _ -> invalidOp "Expected a view-backed button."

  [<GlobalCleanup>]
  member _.Cleanup() = renderer.Dispose()

  [<Benchmark(Baseline = true)>]
  member _.BaseValueClear() = button.ClearProp basePropertyId

  [<Benchmark>]
  member _.DerivedValueClear() = button.ClearProp derivedPropertyId

[<MemoryDiagnoser>]
type DisposalBenchmarks() =
  let mutable renderer = Unchecked.defaultof<VirtualTree.Renderer>

  [<Params(10, 100, 1000)>]
  member val Width = 0 with get, set

  [<IterationSetup>]
  member this.Setup() =
    renderer <- new VirtualTree.Renderer()
    BenchmarkTrees.buttons true this.Width 0 |> BenchmarkTrees.renderIgnore renderer

  [<Benchmark; InvocationCount(1)>]
  member _.DisposeMountedTree() = renderer.Dispose()
