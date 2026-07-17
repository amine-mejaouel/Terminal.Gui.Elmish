# Performance weaknesses and optimization opportunities

Status: analysis and recommendations  
Target baseline: Terminal.Gui 2.4.17, .NET 10, Elmish 5.0.2

## Summary

Terminal.Gui.Elmish should be fast enough for ordinary application-style TUIs, but the current renderer has several identifiable hot paths that will scale poorly for large trees, wide sibling lists, rapid update bursts, and animation-heavy workloads.

The two highest-priority weaknesses are:

1. generated property application probes every property supported by a view type even when only one property changed;
2. unchanged child lists still enter reorder analysis that repeatedly scans `Terminal.Gui.View.SubViews` and can become quadratic for wide containers.

Most improvements can remain internal and preserve the existing Elmish DSL. The more ambitious changes—subtree memoization, deferred view construction, and batched native reordering—need explicit behavioral contracts or Terminal.Gui support.

| Weakness | Current impact | Recommended direction |
| --- | --- | --- |
| Property application scans every supported property | High for frequent updates | Dispatch changed properties directly by generated integer ID |
| Stable child lists still perform reorder analysis | High for wide containers | Add an unchanged-order fast path and linear verification |
| Every commit traverses the complete specification tree | High for large trees | Add reference and revision-based subtree bailouts |
| Render coalescing occurs after view construction | High during update bursts | Coalesce model snapshots or deferred view builders |
| Property and child diffing allocate many temporary objects | Medium to high GC pressure | Use compact snapshots and reusable scratch storage |
| Key validation is repeated | Moderate | Validate each desired sibling list once |
| Terminal.Gui lacks batched arbitrary reordering | High for large shuffles | Add or upstream a move-to-index or batch reorder API |
| `DEBUG` and `TRACE` are enabled in Release | Initial-mount overhead | Restrict those constants to Debug builds |

## Generated property dispatch

`Props.diff` correctly creates a delta containing only added or changed ordinary properties and events. It allocates that delta lazily, so an unchanged node does not create a changed-property snapshot. See [`Types.fs`](../../src/Terminal.Gui.Elmish/Types.fs#L407).

The generated native-property applicator does not consume the delta directly. `ViewPropHandler.applyNativeProps` probes every possible base `View` property and event with `Props.tryFind` or `Props.exists`, after which the derived handler probes its own property set. The current base handler contains approximately 100 such probes. Changing one `Button.Text` value therefore performs all base probes plus the button-specific probes.

The source of this behavior is [`generators/PropsHandler.gen.fs`](../../src/Terminal.Gui.Elmish.Generator/generators/PropsHandler.gen.fs#L27). Generated output such as `PropsHandler.gen.fs` must not be edited directly.

### Recommended change

Generate a single-property setter alongside the existing clear dispatch:

```fsharp
static member setProp(terminalElement, propertyId, value) =
  let view = terminalElement.View :?> Button

  match propertyId.Value with
  | 118 -> view.IsDefault <- unbox value
  | 120 -> view.NoPadding <- unbox value
  | 121 -> view.Text <- unbox value
  | _ -> ViewPropHandler.setProp(terminalElement, propertyId, value)
```

Reconciliation and initial mount can then iterate only present entries:

```fsharp
for entry in changedProps do
  next.SetProp(viewTe, entry.Key.Id, entry.Value)
```

This changes property application from work proportional to every property supported by the view type to work proportional to the number of changed properties.

The generated dispatcher must preserve deterministic application order. Base properties, derived properties, and events currently execute in generated order, which may be significant for Terminal.Gui setters with related initialization or invalidation behavior. A compact snapshot sorted by generated apply order can retain that behavior while enabling direct dispatch.

## Child reconciliation and reorder analysis

Every parent containing children enters `reconcileChildren`. The method materializes old and new arrays, matches identities, recursively reconciles retained children, constructs a result array and `ResizeArray`, and then calls `reorderChildren`. See [`VirtualTree.fs`](../../src/Terminal.Gui.Elmish/VirtualTree.fs#L416).

`reorderChildren` calls `indexOfView` for every attached desired child. Each `indexOfView` call scans `parent.SubViews`. Even when the order is unchanged, a parent with `k` children can therefore perform quadratic work. See [`VirtualTree.fs`](../../src/Terminal.Gui.Elmish/VirtualTree.fs#L231) and the repeated calls in [`reorderChildren`](../../src/Terminal.Gui.Elmish/VirtualTree.fs#L264).

The longest-increasing-subsequence calculation is not the primary problem: it is `O(k log k)` and minimizes logical moves. The repeated native-view searches surrounding it are the expensive common path.

### Recommended change

Add a no-structural-change fast path after child identity matching:

```text
same child count
+ every desired child matched the child at the same index
+ no child was mounted, removed, or replaced
+ attached native order is valid
=> skip LIS, move calculation, origin rewrites, and Children rebuilding
```

Verify native order in one linear pass rather than calling `indexOfView` per child. When reordering is required, build one reference-identity index for the relevant `SubViews` instead of rescanning the collection for every lookup.

Additional fast paths should cover unchanged prefix and suffix ranges, append-only updates, and truncate-only updates. These were anticipated by the retained-reconciler design but are not implemented in the current child path.

## Complete-tree work on every commit

Before mutation, `validateSpecTree` walks the complete desired tree to reject mixed and duplicate sibling keys. Reconciliation then recursively visits every compatible descendant and compares its properties and children, even when a complete subtree is static. See [`VirtualTree.fs`](../../src/Terminal.Gui.Elmish/VirtualTree.fs#L481).

This guarantees correctness but makes committed update cost proportional to total tree size unless Elmish component boundaries isolate the changing section.

### Recommended changes

Start with a reference-identity bailout. When the exact same specification object is supplied for a retained node, the specification cannot have changed and its mounted subtree can be reused without property or child reconciliation.

Then add an explicit memoization facility, for example:

```fsharp
View.memo revision (fun () ->
  expensiveSubtree relevantModel)
```

When node identity and revision are unchanged, the renderer can skip validation and reconciliation for that subtree. The contract must require the revision to change whenever any produced property, child, callback, slot, or relative-position dependency changes.

Independent Elmish components already provide a partial boundary because the parent retains a component node without traversing the component's mounted child tree. Applications can use components to isolate independently changing UI sections while a general memoization API is developed.

Recursive validation and reconciliation should eventually use an explicit work stack where application data can create unusually deep trees. This is primarily a robustness improvement rather than an expected common-case speedup.

## Coalescing after specification construction

The render coordinator uses a bounded capacity-one channel with `DropOldest`, correctly preventing obsolete pending commits from forming an unbounded queue. See [`ElmishTerminal.fs`](../../src/Terminal.Gui.Elmish/ElmishTerminal.fs#L59).

However, `setState` invokes the user's complete `view` function before it submits the resulting specification to the coordinator. See [`ElmishTerminal.fs`](../../src/Terminal.Gui.Elmish/ElmishTerminal.fs#L314). Ten rapid model updates may therefore allocate ten complete specification trees even if the scheduler reconciles only the last one.

### Recommended change

Queue a model snapshot or deferred view-builder request rather than an already-built specification. The render pump should drain obsolete requests first and invoke `view` only for the latest snapshot.

This change requires a deliberate behavioral contract:

- `view` must be pure and safe to invoke after `setState` returns;
- mutable client models must be snapshotted rather than captured by reference;
- view exceptions become asynchronous renderer failures unless explicitly marshalled back;
- specification construction should happen off the Terminal.Gui UI thread, while native reconciliation remains on it.

Because this changes timing and exception behavior, it should follow the lower-risk reconciler improvements and have focused scheduling tests.

## Allocation pressure

A normal declarative view allocates a generated props builder, a `Props` object, dictionary storage, boxed property values, a specification object, and commonly lists and callbacks. Reconciliation adds arrays, dictionaries, hash sets, options, and result collections for many parents.

The existing short-run microbenchmarks report approximately 8–14 KB allocated for two renders of a tree containing only a `Runnable` and one `Button`. These results are regression indicators rather than end-to-end measurements, but they show that allocation is material even for a tiny tree. See [`benchmarks/Terminal.Gui.Elmish.Benchmarks/README.md`](../../benchmarks/Terminal.Gui.Elmish.Benchmarks/README.md#L13).

### Recommended changes

Apply improvements incrementally and measure each one:

1. avoid reorder buffers entirely on the unchanged child-list path;
2. retain `MountedNode.Children` when membership and order are unchanged;
3. index existing `List<ViewSpec>` and `ResizeArray<MountedNode>` collections directly where arrays are unnecessary;
4. reuse or pool large matching and LIS buffers after profiling identifies useful thresholds;
5. replace `Dictionary<PropertyId, ...>` snapshots with a compact small-property representation.

Most view specifications contain only a few explicitly assigned properties. A compact array sorted by generated apply order, or a purpose-built small map with a dictionary fallback, is likely to have better locality and lower overhead than allocating a dictionary for every node. Sorted old and new snapshots can be diffed with a linear merge and passed directly to generated property-ID dispatch.

Dense global property arrays are unlikely to be efficient because generated IDs span many view types. Dense per-type slots or presence bitsets should only be considered after compact snapshots are benchmarked.

## Repeated key validation

`Renderer.Render` globally validates the desired tree before mutation. During `reconcileChildren`, the current implementation also validates both the mounted old sibling specifications and the new sibling specifications again. This repeats array conversion, key counting, hash-set allocation, and string hashing.

The mounted tree is already a renderer invariant, and the complete desired tree was just validated. Reconciliation should consume the result of that validation rather than rechecking both sides.

A larger improvement would normalize each desired sibling list during validation and retain whether it is keyed, along with any lookup metadata needed by reconciliation. The first change should remain simpler: validate the desired tree once and trust the mounted-tree invariant.

## Terminal.Gui physical reorder limitation

Keyed matching is linear and the LIS calculation minimizes the number of logical moves, but Terminal.Gui 2.4.17 exposes adjacent movement and remove/reinsert operations rather than a batched arbitrary-index reorder. A large reverse or random shuffle can still cause substantial hierarchy invalidation even after wrapper-side searches are optimized.

The best long-term solution is an upstream API such as:

```csharp
void MoveSubViewTo(View view, int index);
```

or:

```csharp
void ApplySubViewOrder(IReadOnlyList<View> order);
```

The operation should preserve view identity, focus, and superview ownership; update internal order once; and request layout and drawing once. Without that support, Terminal.Gui.Elmish can optimize unchanged, append, remove, and small-move cases, but worst-case physical shuffle performance remains constrained by the native API.

## Property equality and position callbacks

Ordinary properties use `Object.Equals` to decide whether they changed. This works well for strings and normal value types, but newly allocated collection or reference values are treated as changed even when their contents are equivalent. Applying those setters can trigger unnecessary Terminal.Gui layout or drawing.

Equality should become generated metadata where a property needs domain-specific behavior. Collection-valued view properties require ownership-aware adapters rather than blanket deep equality, because mutation, selection, and disposal semantics differ by Terminal.Gui control.

`TPos.Func` compares callback delegates by reference. A fresh F# closure therefore causes position cleanup and reapplication on every render. Applications should prefer stable position forms where possible; a future memoized or revision-bearing function-position API could make intentional dependencies explicit.

## Release tracing

The main library project currently defines `DEBUG;TRACE` unconditionally. See [`Terminal.Gui.Elmish.fsproj`](../../src/Terminal.Gui.Elmish/Terminal.Gui.Elmish.fsproj#L31). This includes interpolated trace messages and element-path construction during initialization in [`TerminalElement.Base.fs`](../../src/Terminal.Gui.Elmish/TerminalElement.Base.fs#L185).

This does not affect every retained update, but it adds avoidable work during initial mount and makes Release behavior surprising. Remove the unconditional constants and allow the SDK's configuration-specific defaults, or add the tracing constants only in the Debug property group.

## Benchmark coverage and remaining gaps

The [`Terminal.Gui.Elmish.Benchmarks`](../../benchmarks/Terminal.Gui.Elmish.Benchmarks/README.md) BenchmarkDotNet suite now separates declarative specification construction, retained-tree reconciliation, structural mutations, property dispatch, view-valued slots, disposal, and render-request coalescing.

Parameterized coverage includes:

- flat keyed and unkeyed trees at widths 10, 100, and 1,000;
- balanced trees of approximately 21, 341, and 1,365 nodes;
- equivalent renders, one changed leaf, and all changed leaves;
- append, prepend, middle removal, reinsertion, and type replacement;
- rotate, reverse, and deterministic keyed shuffle;
- retained view-slot changes and mounted-tree disposal;
- scheduling bursts of 1, 10, and 100 requests.

Steady-tree and pure-reorder benchmarks prepare specifications outside the timed operation to isolate reconciliation. Structural insert/remove/replace cycles include fresh specification construction because unmounted specifications cache disposed terminal elements and cannot safely be reused. Separate construction benchmarks expose that cost.

The remaining measurement gaps are operation counters for native construction, setters, subscriptions, and hierarchy calls; component-loop scenarios; and a deterministic Terminal.Gui pipeline harness covering layout, drawing, dirty-cell processing, and driver output. Real-terminal I/O must not be mixed into the core reconciler benchmarks.

The suite should establish baselines before property storage, pooling, or reconciler changes. Before/after runs must use the same machine, runtime, filter, and BenchmarkDotNet job.

## Recommended implementation order

1. Record the implemented scaling-suite baseline and add native operation counters where needed.
2. Add the unchanged child-list fast path and replace quadratic native-view searches with linear verification.
3. Replace generated property scanning with direct property-ID dispatch.
4. Remove repeated key validation and obvious transient arrays.
5. Correct Release `DEBUG` and `TRACE` configuration.
6. Introduce compact property snapshots if benchmarks confirm the expected benefit.
7. Add reference-identity and revision-based subtree memoization.
8. Evaluate deferred view construction and propose a Terminal.Gui batch-reorder API.

The first five changes are internal and should preserve the public DSL and rendering semantics. They should be completed and measured before introducing public memoization or changing when and where the user's `view` function executes.
