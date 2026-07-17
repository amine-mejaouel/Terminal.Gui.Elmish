# Terminal.Gui.Elmish benchmarks

This BenchmarkDotNet suite measures the framework layers that Terminal.Gui.Elmish owns. It deliberately separates declarative specification construction, retained-tree reconciliation, structural changes, lifecycle work, and render-request coalescing.

The suite does not initialize a real terminal driver. Terminal.Gui layout, drawing, dirty-cell processing, and terminal I/O require a deterministic in-memory driver harness and remain separate follow-up work.

## Coverage

| Benchmark group | Measures |
| --- | --- |
| `FlatSpecificationConstructionBenchmarks` | Keyed label, unkeyed label, and keyed button specification construction at widths 10, 100, and 1,000 |
| `BalancedSpecificationConstructionBenchmarks` | Specification construction for balanced trees of approximately 21, 341, and 1,365 nodes |
| `SteadyTreeReconciliationBenchmarks` | Equivalent keyed trees, one changed leaf, and all changed leaves |
| `UnkeyedSteadyTreeReconciliationBenchmarks` | Equivalent positional child reconciliation |
| `BalancedTreeReconciliationBenchmarks` | Complete-tree traversal without a wide sibling-list bias |
| `KeyedReorderBenchmarks` | Rotate, reverse, and deterministic shuffle cycles |
| `KeyedStructuralChangeBenchmarks` | Append/remove, prepend/remove, middle removal/reinsertion, and type replacement cycles |
| `SlotReconciliationBenchmarks` | Retained view-valued slot updates |
| `PropertyRemovalBenchmarks` and `ClearDispatchBenchmarks` | Generated base/derived/event property dispatch microbenchmarks |
| `DisposalBenchmarks` | Disposal of mounted trees at several widths |
| `RenderSchedulingBenchmarks` | Capacity-one latest-tree coalescing for bursts of 1, 10, and 100 requests |

Pure reorder and steady-tree benchmarks prepare alternating specifications outside the timed operation so they isolate reconciliation. Structural insert/remove/replace cycles construct fresh specifications inside the operation because an unmounted specification caches a disposed terminal element and cannot be reused safely. Construction-only benchmarks make that additional cost visible independently.

All structural cycles return the renderer to their baseline and use `OperationsPerInvoke = 2`, so BenchmarkDotNet reports normalized per-render results without state drifting between invocations. Deterministic shuffle seeds make comparisons reproducible.

## Running

Run commands from the repository root after restoring tools and Paket dependencies.

List all discovered benchmarks without executing them:

```bash
dotnet run -c Release --project benchmarks/Terminal.Gui.Elmish.Benchmarks -- --list flat
```

Run a short development job over the principal steady-tree scenarios:

```bash
dotnet run -c Release --project benchmarks/Terminal.Gui.Elmish.Benchmarks -- \
  --job short --filter '*SteadyTreeReconciliationBenchmarks*'
```

Run one benchmark group with the default publication-quality job:

```bash
dotnet run -c Release --project benchmarks/Terminal.Gui.Elmish.Benchmarks -- \
  --filter '*KeyedReorderBenchmarks*'
```

Run the complete suite:

```bash
dotnet run -c Release --project benchmarks/Terminal.Gui.Elmish.Benchmarks -- --filter '*'
```

The full parameterized suite is intentionally not run in CI. CI compiles the benchmark project to catch API and F# compile-order regressions.

## Recording comparisons

`BenchmarkDotNet.Artifacts` remains ignored. For an optimization comparison, run the same filtered default job before and after the change on the same machine, then record a concise table here containing:

- commit or patch identity;
- .NET SDK and runtime version;
- OS, architecture, and CPU;
- exact filter and job;
- mean, ratio, and allocated bytes for the affected scenarios.

Do not compare results from different machines or mix short-job numbers with default-job numbers. Short jobs are suitable for directional development feedback, not publication claims.

## Historical property-dispatch comparison

ShortRun results on .NET 10.0.9 (x64 RyuJIT) for the event-bypass refactor:

| Scenario | Before | After | Change | Allocated before | Allocated after |
| --- | ---: | ---: | ---: | ---: | ---: |
| Base value remove/reapply | 6.707 us | 6.259 us | -6.7% | 13.71 KB | 13.70 KB |
| Derived value remove/reapply | 3.601 us | 3.448 us | -4.2% | 8.02 KB | 8.01 KB |
| Event remove/reapply | 4.625 us | 3.785 us | -18.2% | 8.27 KB | 8.27 KB |

These short runs are regression indicators rather than publication-grade measurements. The Debug/MinOpts JIT disassembly for `ViewPropHandler.clearProp` shrank from 6,810 to 1,507 bytes (-77.9%) while retaining direct jump tables for the dense value-property ranges.

