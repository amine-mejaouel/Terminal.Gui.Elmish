module Terminal.Gui.Elmish.Benchmarks.Program

open BenchmarkDotNet.Running

[<EntryPoint>]
let main args =
  BenchmarkSwitcher.FromAssembly(typeof<Terminal.Gui.Elmish.Benchmarks.PropertyRemovalBenchmarks>.Assembly).Run(args)
  |> ignore

  0
