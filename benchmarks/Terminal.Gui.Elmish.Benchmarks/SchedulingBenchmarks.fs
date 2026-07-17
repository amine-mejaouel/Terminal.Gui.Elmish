namespace Terminal.Gui.Elmish.Benchmarks

open System
open System.Threading
open System.Threading.Tasks
open BenchmarkDotNet.Attributes
open Terminal.Gui.App
open Terminal.Gui.Elmish

[<MemoryDiagnoser>]
type RenderSchedulingBenchmarks() =
  let mutable application = Unchecked.defaultof<IApplication>
  let mutable dispatcher = Unchecked.defaultof<ManualRenderDispatcher>

  let mutable coordinator =
    Unchecked.defaultof<ElmishTerminal.TerminalRenderCoordinator>

  let mutable firstGeneration: ISimpleViewSpec array = Array.empty
  let mutable secondGeneration: ISimpleViewSpec array = Array.empty
  let mutable useSecondGeneration = false

  [<Params(1, 10, 100)>]
  member val BurstSize = 0 with get, set

  [<GlobalSetup>]
  member this.Setup() =
    application <- Application.Create()
    dispatcher <- new ManualRenderDispatcher()

    let context =
      { Application = application
        RenderDispatcher = dispatcher }

    coordinator <- ElmishTerminal.TerminalRenderCoordinator(context)

    let build generation index =
      BenchmarkTrees.labels true 1 (generation * 1000 + index)
      |> BenchmarkTrees.asSimpleSpec

    firstGeneration <- Array.init this.BurstSize (build 1)
    secondGeneration <- Array.init this.BurstSize (build 2)
    coordinator.RequestRender(firstGeneration[0], Origin.Root)

  [<GlobalCleanup>]
  member _.Cleanup() =
    coordinator.Dispose()
    dispatcher.Dispose()
    application.Dispose()

  [<Benchmark>]
  member _.CoalescedBurst() : Task =
    task {
      useSecondGeneration <- not useSecondGeneration

      let requests =
        if useSecondGeneration then
          secondGeneration
        else
          firstGeneration

      let rendered = coordinator.WaitForNextRenderedRootAsync()

      for request in requests do
        coordinator.RequestRender(request, Origin.Root)

      use timeout = new CancellationTokenSource(TimeSpan.FromSeconds 5.0)
      do! dispatcher.WaitUntilScheduledAsync(timeout.Token)
      dispatcher.ExecuteOne()
      let! _ = rendered.WaitAsync(timeout.Token)
      return ()
    }
