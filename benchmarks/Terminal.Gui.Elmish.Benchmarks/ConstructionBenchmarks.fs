namespace Terminal.Gui.Elmish.Benchmarks

open BenchmarkDotNet.Attributes
open BenchmarkDotNet.Engines

[<MemoryDiagnoser>]
type FlatSpecificationConstructionBenchmarks() =
  let consumer = Consumer()

  [<Params(10, 100, 1000)>]
  member val Width = 0 with get, set

  [<Benchmark(Baseline = true)>]
  member this.KeyedLabels() =
    BenchmarkTrees.labels true this.Width 0 |> consumer.Consume

  [<Benchmark>]
  member this.UnkeyedLabels() =
    BenchmarkTrees.labels false this.Width 0 |> consumer.Consume

  [<Benchmark>]
  member this.KeyedButtons() =
    BenchmarkTrees.buttons true this.Width 0 |> consumer.Consume

[<MemoryDiagnoser>]
type BalancedSpecificationConstructionBenchmarks() =
  let consumer = Consumer()

  [<Params(2, 4, 5)>]
  member val Depth = 0 with get, set

  [<Benchmark>]
  member this.BuildBalancedTree() =
    BenchmarkTrees.balancedRoot this.Depth 0 |> consumer.Consume
