# Virtual Terminal Tree Reconciler Implementation Plan

Status: proposed  
Target baseline: Terminal.Gui 2.4.17, .NET 10, Elmish 5.0.2

## Objective

Replace the current terminal-element transfer/diff mechanism with a retained virtual terminal tree reconciler that:

- preserves `Terminal.Gui.View` instances, component state, focus, selection, and scroll state when identity is stable;
- updates only properties and hierarchy entries that changed;
- reconciles keyed children in linear time, with move minimization for reordered children;
- treats normal children, view-valued property slots, and Elmish components correctly;
- coalesces model changes into at most one Terminal.Gui commit per application iteration;
- avoids unnecessary layout, drawing, initialization, event subscription, and allocation work.

The reconciler will determine **what changed**. Generated property patchers will determine **how each change is applied** to a strongly typed Terminal.Gui view.

## Existing implementation and motivation

The active `KeyedDiffer` currently:

- identifies children by their view name/type rather than an explicit key;
- sorts children before matching them, losing sibling-order semantics;
- pairs repeated controls of the same type by occurrence;
- repeatedly creates lists, sorts them, and filters them by type;
- transfers a `View` from a newly created terminal element and disposes the previous wrapper;
- does not fully reconcile Elmish components or view-valued sub-elements.

`SimpleDiffer` is currently a stub and is the intended entry point for the replacement.

Property application is duplicated between `PropsHandler.gen.fs` and `TerminalElement.Elements.gen.fs`. The new architecture will keep a redesigned generated property-patching layer and remove property ownership from terminal-element wrapper classes.

## Scope

### In scope

- A retained mounted tree containing the live Terminal.Gui objects.
- Explicit sibling keys and exact node-type compatibility.
- Positional reconciliation for unkeyed children.
- Generated, typed property set/unset/equality operations.
- Stable event subscriptions with mutable dispatch targets.
- Reconciliation of normal children and view-valued property slots.
- Preservation and update of Elmish components.
- Render scheduling and update coalescing.
- Correct disposal and cleanup.
- Correctness tests, performance tests, and migration from `KeyedDiffer`.

### Out of scope

- Diffing terminal screen cells; Terminal.Gui already owns drawing and buffering.
- Representing every row of large lists, tables, or trees as a virtual view. Those controls should continue to use Terminal.Gui data sources and virtualization.
- Parallel mutation of Terminal.Gui views. All commits must occur on the application/UI thread.
- Hand-editing generated files. Generator sources remain the source of truth.

## Architectural overview

```text
Elmish model
    -> view function
        -> immutable ViewSpec tree
            -> reconcile against MountedNode tree
                -> generated property patchers
                -> Terminal.Gui hierarchy operations
                -> retained MountedNode tree
```

The `ViewSpec` tree describes desired state. A `MountedNode` owns the live state that survives renders.

## Core data model

The exact representation can evolve after benchmarking, but it should expose the following semantics:

```fsharp
[<Struct; CustomEquality; NoComparison>]
type ElementKey =
  private
  | ElementKey of string

[<Struct>]
type NodeKind =
  | View of typeId: RuntimeTypeHandle
  | Component of componentTypeId: RuntimeTypeHandle

type MountedNode =
  { Kind: NodeKind
    Key: ElementKey voption
    View: Terminal.Gui.ViewBase.View
    mutable Props: PropSnapshot
    mutable Children: ResizeArray<MountedNode>
    mutable Slots: MountedSlot array
    EventSlots: EventSlot array
    Component: IComponentView voption
    mutable Parent: MountedNode voption
    mutable Index: int }
```

Implementation notes:

- `ElementKey` is renderer identity and must be distinct from `Terminal.Gui.View.Id`.
- `NodeKind` must distinguish exact closed generic types.
- Use `ValueOption`, arrays, `ResizeArray`, integer IDs, and iterative loops in hot paths.
- Do not use display names or raw property-name strings for runtime matching.
- Add an optional memo/revision token to `ViewSpec` after the base reconciler is correct. A stable token can provide an O(1) subtree bailout.

## Identity rules

1. A keyed node is reusable only when its key and exact node kind both match.
2. An unkeyed node is reusable only at the same logical position and when its exact node kind matches.
3. A matching key with a different view/component type replaces the node.
4. Keys must be unique among siblings. Duplicate keys fail before committing that sibling list.
5. Keyed and unkeyed children should not be mixed in one list. Initially reject mixed lists with a clear error; support segmented mixed reconciliation later only if a real use case requires it.
6. The root view type must remain stable during a running Terminal.Gui session. A root replacement requires an explicit application-session replacement path.

These rules prevent state from moving between logically unrelated controls.

## Node reconciliation

For each old mounted node and new specification:

1. If kind or key differs, mount a replacement subtree, replace the hierarchy entry, then unmount the old subtree.
2. If a memo/revision token is unchanged, reuse the entire mounted subtree without inspecting it.
3. Patch ordinary properties.
4. Reconcile view-valued property slots.
5. Reconcile normal children.
6. Update the stored property snapshot and virtual metadata.

Use an explicit work stack where recursion depth may be controlled by user input.

## Child-list reconciliation

### Fast paths

Run these before allocating a lookup table:

1. Skip the common prefix while identities match.
2. Skip the common suffix while identities match.
3. If only new nodes remain, mount and append/insert them.
4. If only old nodes remain, remove and unmount them.
5. For a fully unkeyed list, reconcile positionally.

### Keyed middle range

For the unmatched keyed range:

1. Build or reuse a key-to-mounted-node index for old children.
2. Scan new children once, rejecting duplicate keys.
3. Reconcile matching nodes and mount missing nodes while detached.
4. Record the old index of every reused node in new order.
5. Remove and unmount old nodes that were not matched.
6. Compute the longest increasing subsequence of the old-index sequence.
7. Process the new range from right to left:
   - attach new nodes at their final positions;
   - leave nodes in the increasing subsequence in place;
   - move only reused nodes outside the subsequence.

Expected complexity:

- unchanged/append/truncate paths: O(changed range);
- keyed matching: O(n);
- move minimization: O(n log n);
- scratch memory: O(largest changed sibling range).

### Terminal.Gui reorder limitation

Terminal.Gui 2.4.17 exposes `AddAt`, `Remove`, move-to-start/end, and one-position movement, but it does not expose an arbitrary-index move or batched reorder operation.

Initial implementation:

- use movement APIs for small pure reorderings because they preserve the superview and avoid layout/focus churn;
- use `Remove` followed by `AddAt` for a large non-LIS move, preserving the `View` instance but accepting hierarchy invalidation;
- select the crossover using benchmarks, not a hard-coded assumption.

Preferred upstream optimization:

```csharp
void ApplySubViewOrder(IReadOnlyList<View> order)
```

or:

```csharp
void MoveSubViewTo(View view, int index)
```

A batched API should preserve focus and superview ownership, update the internal list once, and request one redraw. The virtual diff can be optimal without it, but arbitrary physical reordering can still be quadratic with the current public APIs.

## Generated property patching

`PropsHandler.gen.fs` remains necessary, but its generated contract changes. It becomes the only layer that knows how to apply a property to a concrete Terminal.Gui type.

Generate a descriptor or equivalent direct dispatch for each property:

```fsharp
type PropKind =
  | Value
  | Event
  | ViewSlot
  | ViewCollectionSlot
  | InitOnly
  | ReplaceOnChange

type PropDescriptor =
  { Id: int
    Kind: PropKind
    Equals: obj -> obj -> bool
    Set: Terminal.Gui.ViewBase.View -> obj -> unit
    Unset: Terminal.Gui.ViewBase.View -> unit }
```

### Required behavior

- Assign stable integer property IDs at generation time.
- Apply only added or changed properties.
- Reset removed properties through a generated `Unset` operation.
- Use typed equality where safe.
- Permit property-specific comparison for values whose normal object equality is unsuitable.
- Mark init-only or unsafe-to-update properties as node-replacement triggers.
- Delegate view-valued properties to slot reconciliation instead of ordinary assignment.
- Do not manually request layout/draw when the Terminal.Gui setter already performs invalidation.

### Property storage

Replace nested dictionaries keyed by raw strings in the hot path with one of:

1. sorted compact `(propertyId, value)` arrays merged in O(old + new); or
2. generated dense slots plus a presence bitset for types with a bounded property set.

Start with sorted compact arrays because they are simpler and naturally handle inheritance. Benchmark dense slots before adopting them.

### Event properties

Subscribe once per mounted event property using a stable trampoline:

```text
Terminal.Gui event -> stable delegate -> mutable current Elmish handler
```

On render:

- added event: create slot and subscribe once;
- changed handler: replace the slot's current callback only;
- removed event: unsubscribe and release the slot;
- unmount: unsubscribe all remaining slots.

This prevents newly allocated F# closures from causing remove/add subscription churn every render.

## View-valued property slots

Properties such as `Shortcut.TargetView`, `Tabs.Value`, menu sub-elements, and other view-valued properties are not normal `SubViews` children.

Represent each as a named slot:

```fsharp
type MountedSlot =
  { PropertyId: int
    mutable Node: MountedNode voption }
```

For every slot:

1. Reconcile its old node against the new slot specification.
2. Assign the resulting live `View` through the generated setter.
3. Never call `View.Add` for a property slot.
4. Unset the property before disposing a removed slot if the Terminal.Gui control retains the reference.

Collection-valued view properties require generated or hand-written collection adapters capable of keyed item deltas. They must not be treated as ordinary child lists unless Terminal.Gui documents them as `SubViews`.

## Elmish component handling

Component identity is `(component type, key)`.

- Matching identity: preserve the existing component loop and call `IComponentView.Update` with new component props.
- Different identity: terminate/unmount the old component and initialize the replacement.
- A component's rendered root participates in its own retained subtree.
- Parent reconciliation treats the component as one mounted node; it must not pair components merely because their rendered root view types match.
- Component termination must be idempotent and must release subscriptions and child trees.

This completes the currently unfinished component reuse path.

## Mount, commit, and unmount ordering

### Mount

1. Create the Terminal.Gui view while detached.
2. Apply all initial non-slot properties.
3. Install event slots.
4. Mount view-valued property slots.
5. Mount normal child subtrees into the still-uninitialized parent.
6. Attach the completed subtree to its initialized live parent with `AddAt`.

Building detached minimizes invalidation propagation and lets Terminal.Gui initialize the subtree once when it enters an initialized hierarchy.

### Commit

1. Verify all keys and construct the sibling reconciliation plan before mutating that sibling list.
2. Patch retained views only through changed-property operations.
3. Mount replacement/new subtrees detached.
4. Remove unmatched hierarchy entries.
5. Attach new entries and apply required moves.
6. Resolve position/layout references that depend on newly mounted sibling views.
7. Dispose removed nodes after they are detached.
8. Publish the new mounted tree as current only after the commit succeeds.

Terminal.Gui hierarchy events can cancel additions/removals. Renderer-owned hierarchies should document cancellation as unsupported. If a mutation returns failure, stop the commit and raise a diagnostic containing the parent path, key, type, and operation; do not silently let the mounted tree diverge from Terminal.Gui.

### Unmount

1. Mark the node as unmounting to make cleanup idempotent.
2. Clear component subscriptions and stop component loops.
3. Remove event handlers and external service registrations.
4. Clear view-valued properties that retain child references.
5. Detach the root view from its parent once.
6. Dispose the root `View`, allowing Terminal.Gui to dispose its actual `SubViews` once.
7. Clear mounted references.

Avoid independently disposing every child and then disposing a parent that already owns those children.

## Render scheduling

Add a renderer owned by `TerminalModel`:

```fsharp
type Renderer =
  { mutable Current: MountedNode voption
    mutable Pending: ViewSpec voption
    mutable CommitScheduled: bool }
```

On Elmish state change:

1. Build or store the latest desired `ViewSpec`.
2. Replace any older pending spec that has not been committed.
3. Schedule one application-thread callback if none is scheduled.
4. Reconcile the latest pending tree in that callback.
5. If another update arrives during reconciliation, schedule one additional callback.

Never call `Layout` or `Draw` directly from the renderer. Terminal.Gui setters and hierarchy operations already establish `NeedsLayout`, `NeedsDraw`, and dirty regions for the next application iteration.

## File-level implementation plan

### New handwritten files

- `VirtualTree.Types.fs`
  - keys, node kinds, prop snapshots, mounted nodes, slots, event slots, and errors.
- `VirtualTree.Mount.fs`
  - mount/unmount and lifecycle ownership.
- `VirtualTree.Children.fs`
  - prefix/suffix matching, keyed index, duplicate validation, LIS, and move planning.
- `VirtualTree.Reconciler.fs`
  - node reconciliation and commit orchestration.
- `VirtualTree.Renderer.fs`
  - current/pending state and application-thread scheduling.

Names can be shortened after the architecture settles, but responsibilities should remain separated.

### Existing handwritten files

- `Types.fs`
  - expose an explicit renderer key on view/component specifications;
  - phase out old `TerminalElement`, `Origin`, and reuse interfaces after migration.
- `SimpleDiffer.fs`
  - initially become a thin entry point into `VirtualTree.Reconciler`;
  - remove once callers use the reconciler directly, or retain as the public internal facade.
- `ElmishTerminal.fs`
  - own a `Renderer` per terminal program/component;
  - replace `CreateViewTE` plus `KeyedDiffer.update` with scheduled reconciliation;
  - preserve component renderers across parent renders.
- `Config.fs`
  - add a temporary feature flag for migration and A/B benchmarking;
  - remove the flag after the new reconciler becomes the only implementation.
- `PositionService.fs`
  - consume mounted identities/live views rather than wrapper origins;
  - support post-hierarchy reference fixups.

### Generator sources

- `ViewMetadata.fs`
  - assign property IDs and update policies;
  - identify init-only, view-slot, collection-slot, event, and replacement properties;
  - expose equality/default metadata.
- `generators/PropsHandler.gen.fs`
  - generate the new typed patch descriptors or direct integer-ID dispatch;
  - generate set, unset, event attach/detach, and slot assignment operations.
- `generators/Props.gen.fs`
  - write compact property entries instead of nested string dictionaries.
- `generators/PKey.gen.fs`
  - generate stable integer IDs while preserving typed builder APIs.
- `generators/SimpleViewSpec.gen.fs`
  - expose exact node-kind/factory metadata without creating old terminal-element wrappers.
- `generators/TerminalElement.Elements.gen.fs`
  - stop generating property application;
  - remove this output entirely after the old differ is deleted.

### Project file

Update `Terminal.Gui.Elmish.fsproj` in dependency order. The expected high-level order is:

1. generated view/property identity types;
2. base virtual-tree types;
3. generated prop patchers/factories;
4. mount and child reconciliation;
5. reconciler and renderer;
6. Elmish program integration;
7. public DSL/macros.

## Delivery phases

### Phase 0: Baseline and invariants

- [ ] Add tests that capture current expected property, event, child, sub-element, focus, and disposal behavior.
- [ ] Add counters/test doubles for view creation, initialization, addition, removal, event subscription, and disposal.
- [ ] Add benchmarks for unchanged, property-only, append, prepend, delete, replace, reverse, and random shuffle updates.
- [ ] Record allocation and elapsed-time baselines for `KeyedDiffer`.

Acceptance criteria:

- Baseline tests reproduce the known component and repeated-sibling weaknesses.
- Benchmarks run deterministically without a real terminal.

### Phase 1: Retained tree and basic unkeyed reconciliation

- [ ] Introduce renderer keys and exact node kinds.
- [ ] Implement mount/unmount ownership.
- [ ] Implement same-type positional reuse and type replacement.
- [ ] Integrate behind a feature flag for root programs without components or view slots.

Acceptance criteria:

- An unchanged render creates and disposes zero Terminal.Gui views.
- A property-only render preserves all view references.
- Replacing a type disposes exactly the replaced subtree.

### Phase 2: Generated property patchers

- [ ] Add integer property IDs and compact snapshots.
- [ ] Redesign `PropsHandler.gen.fs` around set/unset/equality/update policy.
- [ ] Remove new-path dependence on `ViewBackedTerminalElement.SetProps` and `RemoveProps`.
- [ ] Implement stable event trampolines.

Acceptance criteria:

- Unchanged props invoke zero setters.
- One changed prop invokes one generated setter unless Terminal.Gui itself performs related work.
- Removed props restore their documented defaults.
- A changing closure does not resubscribe the Terminal.Gui event.

### Phase 3: Keyed child reconciliation

- [ ] Implement key validation and keyed lookup.
- [ ] Implement prefix/suffix fast paths.
- [ ] Implement LIS calculation and move planning.
- [ ] Implement the benchmark-driven Terminal.Gui move strategy.
- [ ] Preserve origins/paths through mounted parent and index metadata.

Acceptance criteria:

- Prepending a keyed child does not recreate existing siblings.
- Reordering preserves each child's live view and local state.
- Duplicate and mixed-key errors occur before hierarchy mutation.
- Final `SubViews` order exactly matches the desired tree.

### Phase 4: View slots and components

- [ ] Reconcile single view-valued properties by property ID.
- [ ] Add adapters for supported collection-valued view properties.
- [ ] Implement component update without restarting its loop.
- [ ] Implement component replacement and termination.
- [ ] Migrate position references to mounted live views.

Acceptance criteria:

- Slot views never appear in `SubViews` unless Terminal.Gui itself owns them there.
- Component state survives parent rerenders and keyed moves.
- Removing a component terminates and disposes it exactly once.

### Phase 5: Scheduling and invalidation discipline

- [ ] Coalesce pending view trees.
- [ ] Commit only on the application thread.
- [ ] Remove direct renderer calls to layout/draw.
- [ ] Verify add/remove/move ordering with focusable and overlapping views.

Acceptance criteria:

- Multiple state updates before an application iteration produce one commit of the latest tree.
- Property-only updates do not trigger hierarchy operations.
- No update is performed concurrently with Terminal.Gui drawing/layout.

### Phase 6: Optimization and old-path removal

- [ ] Profile before introducing pooling or dense property storage.
- [ ] Reuse keyed indexes and scratch buffers where measurements justify it.
- [ ] Remove `KeyedDiffer`, old terminal-element reuse, duplicate generated prop code, and the feature flag.
- [ ] Update documentation and examples with explicit key guidance.
- [ ] Consider proposing the batched reorder API upstream.

Acceptance criteria:

- The full solution builds and all tests pass.
- Generated files are reproducible from an unchanged working tree.
- No old terminal-element wrapper is created during a render.
- Performance gates below are satisfied.

## Correctness test matrix

Test each scenario with reference-identity assertions on live Terminal.Gui views:

| Scenario | Expected result |
|---|---|
| Identical tree | No setters, hierarchy calls, initialization, or disposal |
| One value prop changed | Same view; only that property patched |
| Prop removed | Same view; generated default restored |
| Event callback changed | Same subscription; current callback updated |
| Append keyed child | Existing children retained; one mount/add |
| Prepend keyed child | Existing children retained and reordered correctly |
| Remove middle keyed child | Only removed subtree disposed |
| Reverse keyed list | All views retained; order exactly reversed |
| Duplicate key | Error before mutation |
| Same key, different type | Old node replaced and disposed |
| Unkeyed insertion | Positional semantics documented and observed |
| View-valued property added/replaced/removed | Correct setter used; no accidental `Add` |
| Component parent rerender | Component loop/state retained |
| Component keyed move | State follows key |
| Focused child moved | Focus remains on the same live view where Terminal.Gui permits |
| Focused child removed | Terminal.Gui selects valid focus; removed view disposed |
| Overlapping children reordered | Drawing/Z-order follows the virtual order |
| Layout reference to a moved sibling | Reference resolves to retained live view |
| Parent unmount | Each view and external registration cleaned once |
| Cancelled hierarchy event | Commit fails loudly without publishing divergent state |

Add randomized model-based tests comparing the final mounted/Terminal.Gui hierarchy against a simple reference implementation after sequences of insert, remove, replace, and move operations.

## Performance test matrix

Benchmark at multiple tree widths and depths:

- identical render;
- single leaf property change;
- all leaf properties changed;
- append/prepend one child;
- remove first/middle/last child;
- replace one subtree;
- rotate, reverse, and random shuffle keyed siblings;
- component parent rerender;
- view-slot replacement;
- burst of model updates coalesced to one render.

Record:

- elapsed time;
- managed allocations and Gen0 collections;
- number of view constructions/disposals;
- property setter calls;
- event add/remove operations;
- `AddAt`, `Remove`, and move calls;
- initialization calls;
- Terminal.Gui layout passes where observable;
- driver writes or dirty-cell counts where observable.

Performance gates:

- Identical renders perform zero live-view mutations and allocate no data proportional to total tree size when a memo/revision token permits bailout.
- Property-only updates perform zero hierarchy operations and zero view construction/disposal.
- Keyed insertion/removal performs work proportional to the affected sibling range, without recreating retained siblings.
- Reordering performs the minimum logical moves predicted by LIS, subject to the current Terminal.Gui physical-move API.
- The new reconciler outperforms `KeyedDiffer` and allocates less in every representative non-trivial update scenario. Any regression requires a documented correctness or maintainability justification.

## Rollout and compatibility

1. Keep the existing DSL source-compatible where possible.
2. Add explicit `Key` support without changing the behavior of `View.Id`.
3. Run old and new renderer test suites during migration.
4. Use a temporary internal configuration switch for A/B tests; do not expose two permanent public differ modes.
5. Emit development-time warnings for dynamic sibling lists without keys.
6. Document that keys are local to a parent and must be derived from stable domain identity, not indexes.
7. Remove obsolete terminal-element APIs only after examples, tests, and components use the retained renderer.

## Open decisions

Resolve these with targeted prototypes and benchmarks:

1. **Key representation:** start with a dedicated string key for API clarity; consider a generic/string-or-int key only if boxing or conversion is measurable.
2. **Property snapshot:** start with sorted compact arrays; compare against dense slots after Phase 2.
3. **Reorder strategy:** measure adjacent moves versus remove/add-at by list size and permutation distance.
4. **Hierarchy cancellation:** recommended policy is fail-fast because general rollback cannot undo arbitrary user event side effects.
5. **Collection view slots:** implement only for properties with verified Terminal.Gui ownership/lifecycle semantics.
6. **Memoization API:** add after correctness; require a revision/input equality contract rather than recomputing whole-subtree structural hashes.
7. **Root replacement:** either reject it during a session or implement it as a controlled stop/start operation.

## Definition of done

The implementation is complete when:

- the retained mounted tree is the only update path;
- keys and exact types determine state-preserving identity;
- normal children, view slots, and components are all reconciled correctly;
- `PropsHandler.gen.fs` is the sole generated property application layer;
- event handlers use stable subscriptions;
- unchanged views are never recreated;
- Terminal.Gui performs layout/drawing through its normal invalidation lifecycle;
- old terminal-element reuse and duplicate generated property code are removed;
- correctness, randomized hierarchy, lifecycle, and performance tests pass;
- benchmarks demonstrate lower mutation counts and allocation than the old differ.
