module Terminal.Gui.Elmish.Benchmarks.Program

open BenchmarkDotNet.Attributes
open BenchmarkDotNet.Diagnosers
open BenchmarkDotNet.Running
open Terminal.Gui.Elmish

[<MemoryDiagnoser>]
[<DisassemblyDiagnoser(maxDepth = 3)>]
type PropertyRemovalBenchmarks() =
  let baseValueRenderer = new VirtualTree.Renderer()
  let derivedValueRenderer = new VirtualTree.Renderer()
  let eventRenderer = new VirtualTree.Renderer()

  let asSimpleSpec (view: IView) = view :?> ISimpleViewSpec

  let rootWithButton configure =
    View.Runnable(fun (p: RunnableProps) ->
      p.Children
        [ View.Button(fun p ->
            p.Key "button"
            configure p)
          :> IView ])
    |> asSimpleSpec

  let withoutProps = rootWithButton ignore

  let withBaseValue = rootWithButton (fun p -> p.Title "benchmark")

  let withDerivedValue = rootWithButton (fun p -> p.NoPadding true)

  let withEvent = rootWithButton (fun p -> p.Accepting ignore)

  let render (renderer: VirtualTree.Renderer) spec =
    renderer.Render(spec, Origin.Root) |> ignore

  [<GlobalSetup>]
  member _.Setup() =
    render baseValueRenderer withBaseValue
    render derivedValueRenderer withDerivedValue
    render eventRenderer withEvent

  [<GlobalCleanup>]
  member _.Cleanup() =
    baseValueRenderer.Dispose()
    derivedValueRenderer.Dispose()
    eventRenderer.Dispose()

  [<Benchmark(Baseline = true)>]
  member _.BaseValueRemoveAndReapply() =
    render baseValueRenderer withoutProps
    render baseValueRenderer withBaseValue

  [<Benchmark>]
  member _.DerivedValueRemoveAndReapply() =
    render derivedValueRenderer withoutProps
    render derivedValueRenderer withDerivedValue

  [<Benchmark>]
  member _.EventRemoveAndReapply() =
    render eventRenderer withoutProps
    render eventRenderer withEvent

[<MemoryDiagnoser>]
[<DisassemblyDiagnoser(maxDepth = 3)>]
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

    let mounted = renderer.Render(root :?> ISimpleViewSpec, Origin.Root)
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

  [<IterationSetup>]
  member _.Setup() =
    renderer <- new VirtualTree.Renderer()

    let root =
      View.Runnable(fun (p: RunnableProps) ->
        p.Children
          [ View.Button(fun p ->
              p.Key "button"
              p.Title "benchmark"
              p.NoPadding true
              p.Accepting ignore
              p.Disposing ignore)
            :> IView ])
      :> IView

    renderer.Render(root :?> ISimpleViewSpec, Origin.Root) |> ignore

  [<Benchmark; InvocationCount(1)>]
  member _.DisposeTreeWithValuesAndEvents() = renderer.Dispose()

[<EntryPoint>]
let main args =
  BenchmarkSwitcher.FromAssembly(typeof<PropertyRemovalBenchmarks>.Assembly).Run(args)
  |> ignore

  0
