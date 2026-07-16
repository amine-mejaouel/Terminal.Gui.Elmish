# Property removal benchmarks

Run the benchmarks from the repository root:

```bash
dotnet run -c Release --project benchmarks/Terminal.Gui.Elmish.Benchmarks -- --filter '*'
```

`PropertyRemovalBenchmarks` measures complete remove/reapply cycles through virtual-tree reconciliation. `ClearDispatchBenchmarks` isolates the generated value-property dispatch for a base property and a derived property with base fallback. `DisposalBenchmarks` measures disposal of a mounted tree containing both values and registered events.

BenchmarkDotNet result artifacts are intentionally not committed. Record only a concise before/after comparison here when changing property-removal dispatch.

## Event-bypass comparison

ShortRun results on .NET 10.0.9 (x64 RyuJIT) for the event-bypass refactor:

| Scenario | Before | After | Change | Allocated before | Allocated after |
| --- | ---: | ---: | ---: | ---: | ---: |
| Base value remove/reapply | 6.707 us | 6.259 us | -6.7% | 13.71 KB | 13.70 KB |
| Derived value remove/reapply | 3.601 us | 3.448 us | -4.2% | 8.02 KB | 8.01 KB |
| Event remove/reapply | 4.625 us | 3.785 us | -18.2% | 8.27 KB | 8.27 KB |

These short runs are regression indicators rather than publication-grade measurements; rerun the default job for higher-confidence results after further dispatch changes.

The Debug/MinOpts JIT disassembly for `ViewPropHandler.clearProp` shrank from 6,810 to 1,507 bytes (-77.9%) while retaining two direct jump tables for the dense value-property ranges.
