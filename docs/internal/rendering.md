# Rendering mental model

Terminal.Gui.Elmish does not render terminal cells itself. Its job is to turn the latest Elmish model into a description of the desired view tree, reconcile that description with the views already mounted, and mutate the retained `Terminal.Gui.View` objects. Terminal.Gui then performs layout and drawing during its application loop.

This distinction is the most useful starting point for reading the rendering code:

```text
Terminal.Gui event
       |
       v
    dispatch -> Elmish update -> model -> view function
                                           |
                                           v
                                  desired ViewSpec tree
                                           |
                                           v
                               TerminalRenderCoordinator
                                  /                 \
                     first mount /                   \ later requests
                       (sync)   v                     v
                              |          Channel<RenderRequest> (capacity 1)
                              |                     |
                              |            shared UI-thread dispatcher
                              |                     |
                              +----------+----------+
                                         v
                                  VirtualTree.Renderer
                                    |              |
                               first mount     reconciliation
                                    |              |
                                    +-------+------+
                                            v
                                retained Terminal.Gui.View tree
                                            |
                                            v
                               Terminal.Gui layout and drawing
```

## The representations to keep separate

Most rendering bugs become easier to understand once the declarative and retained objects are not treated as the same tree.

| Representation | Important types | Lifetime and responsibility |
| --- | --- | --- |
| Desired tree | `IView`, `ViewSpec`, `ISimpleViewSpec`, `IComponentViewSpec`, `Props` | Normally rebuilt by the application's `view` function. It describes what should exist now; it does not own the displayed native hierarchy. |
| Retained renderer state | `MountedNode`, `TerminalElement`, `IViewTE`, `IElmishComponentTE` | Survives compatible renders. It remembers the previous specification, node identity, mounted children and slots, native event subscriptions, reconciled-property resources, origins, and cleanup state. |
| Native UI | `Terminal.Gui.ViewBase.View` and its subclasses | The mutable objects used by Terminal.Gui. Their identity carries focus, selection, scrolling, and other control-local state across renders. Terminal.Gui owns their layout and drawing behavior. |

For example, `View.Label(fun p -> p.Text "Ready")` creates a label specification and its property snapshot. It does **not** immediately create a native `Terminal.Gui.Views.Label`. The native label is created only if the renderer needs to mount that specification. On a compatible later render, the new specification is bound to the existing terminal element and native label.

`MountedNode` and `TerminalElement` have related but different jobs. `MountedNode` is the reconciler's structural record: it stores the last specification plus ordered children and named slots. The terminal element is the bridge to the live object: a view terminal element owns the native `View`, current `Props`, event registrar, origin, and view-specific set/clear operations.

## From Elmish state to a render commit

`ElmishTerminal.mkProgram` and `ElmishTerminal.mkSimple` install a custom Elmish `setState` function. Whenever Elmish publishes state, that function:

1. calls the application `view` function with the current model and dispatch function;
2. casts the returned root to `ISimpleViewSpec`;
3. chooses an `Origin` for either the root program or a nested Elmish component;
4. sends the desired root to `TerminalRenderCoordinator.RequestRender`.

## One coordinator per Elmish loop

Every Elmish loop owns its own `TerminalRenderCoordinator`. The root program has one, and every mounted Elmish component has another. Each coordinator owns the state that must be isolated per loop: its capacity-one render channel, render pump, `VirtualTree.Renderer`, current rendered root, lifecycle, and waiters.

The `TerminalRenderContext` is different: it belongs to the Terminal.Gui application and is shared by all those coordinators. It contains the single `IApplication` and single `IRenderDispatcher`. When a parent renderer mounts an Elmish component, it passes this same context to `StartElmishLoop`, so the component creates a new coordinator without creating another dispatcher.

```text
 Root Elmish loop          Component Elmish loop A       Nested component loop B
         |                           |                              |
         v                           v                              v
 TerminalRenderCoordinator  TerminalRenderCoordinator    TerminalRenderCoordinator
 +-----------------------+  +-------------------------+   +-------------------------+
 | Channel (capacity 1)  |  | Channel (capacity 1)    |   | Channel (capacity 1)    |
 | render pump           |  | render pump             |   | render pump             |
 | VirtualTree.Renderer  |  | VirtualTree.Renderer    |   | VirtualTree.Renderer    |
 +-----------+-----------+  +------------+------------+   +------------+------------+
             \                         |                            /
              \________ subsequent commits call DispatchAsync ____/
                                        |
                                        v
                        TerminalRenderContext (one per application)
                        +-------------------------------------------+
                        | IRenderDispatcher (one, shared)           |
                        | IApplication                              |
                        +-------------------+-----------------------+
                                            |
                                            v
                             Terminal.Gui application/UI thread
```

The coordinators do not share desired trees or mounted-tree state. They share only the application-level context needed to commit mutations safely. The shared dispatcher posts every later commit to the same Terminal.Gui UI thread, which serializes native view mutation across the root and every component. The root application owns dispatcher activation and disposal; component disposal stops only that component's coordinator.

### The first render

The first desired tree is committed synchronously while the `IApplication` is not initialized. This is necessary because `runTerminal` must obtain a native root `Runnable` before it can call `IApplication.Run`.

Once that root exists, `runTerminal` calls `Init`, activates render dispatching, and passes the retained root view to `Run`. The root view type and key must remain stable for the lifetime of that application session. The renderer rejects a root replacement instead of swapping the runnable underneath Terminal.Gui.

### Later renders

After the initial mount, each `TerminalRenderCoordinator` has one long-lived render pump. `RequestRender` writes desired trees to a bounded `Channel<RenderRequest>` with capacity one and `DropOldest` behavior. The channel is the pending slot: if several Elmish states arrive faster than Terminal.Gui can apply them, only the newest uncommitted tree remains.

Elmish serializes its own `update` and `setState` calls, but that does not make the whole rendering pipeline synchronous. The Terminal.Gui loop is a separate consumer: an Elmish update can request another tree while an earlier request is waiting to enter the UI thread. The channel models that boundary explicitly and guarantees a single render consumer without relying on mutable `pending` and `scheduled` flags.

The pump is the channel's single reader. It waits for a request, drains any newer request already available, and asks `IRenderDispatcher` to run one commit on the Terminal.Gui application thread. Immediately before reconciliation, the callback drains the channel once more so an update that arrived while dispatching replaces the older tree. An update that arrives during reconciliation remains in the channel and causes the next pump iteration.

The production dispatcher is inactive until `runTerminal` has called `IApplication.Init`. Once activated, it posts commits with a zero-duration application timeout. Tests and standalone renderer use can inject an immediate dispatcher without changing the channel or renderer rules. A commit is guarded against concurrent disposal, and a render failure moves the scheduler to a terminal faulted state.

This means an Elmish `view` function may run more often than the native hierarchy is committed. Code must not rely on every intermediate desired tree becoming visible.

## First mount

`VirtualTree.Renderer.Render` first converts the root into a `ViewSpec` and validates sibling keys throughout the desired tree. With no current `MountedNode`, it mounts the root:

1. A simple view specification creates its generated `IViewTE`; a component specification resolves its `IElmishComponentTE`.
2. `InitializeTree` assigns each terminal element's `Origin` and creates native views through the generated `NewView` implementations.
3. View-valued property slots are initialized and converted from declarative specifications to native property values.
4. Positions are applied, followed by reconciled properties in their `BeforeNative` phase, generated native properties, and reconciled properties in their `AfterNative` phase. Event properties install their stable native subscriptions.
5. Normal child views are inserted into their parent's `SubViews` at their declared index when that view kind participates in the normal hierarchy.
6. Nested components start their own Elmish loops and expose the native root produced by those loops.
7. `captureMountedTree` records the initialized terminal elements as a retained `MountedNode` tree and binds each specification to the element it represents.

The renderer never asks Terminal.Gui to draw at this point. Native setters, hierarchy operations, and the Terminal.Gui application lifecycle establish the layout and drawing work that Terminal.Gui will perform.

## Reconciliation

On every later commit, the renderer validates the new desired tree before changing the mounted hierarchy. It then compares the desired root with the current root and recursively reconciles compatible nodes.

For a retained simple view, reconciliation occurs in this order:

1. reconcile view-valued property slots;
2. reconcile normal children;
3. clean up and reapply relative positions if `X` or `Y` changed;
4. compute native and reconciled-property removals and changes;
5. clear removed reconciled resources and removed native properties;
6. apply changed `BeforeNative` reconciled properties, changed native properties, and changed `AfterNative` reconciled properties;
7. store the new specification.

The new specification is bound to the retained terminal element before its descendants and properties are processed. For a sibling list, all matched specifications are bound before any sibling is reconciled. This is important for `TPos` values that refer to another specification created by the current `view` call: resolving that specification must lead to the already-retained native view.

### Identity decides whether state survives

Node identity has two modes:

| Child list | A node is retained when... |
| --- | --- |
| Keyed | The key and exact view or component type both match. Sibling position may change. |
| Unkeyed | The old and new nodes occupy the same sibling position and have the same exact type. |

Keys are local to one parent. Every child in a sibling list must either have a key or be unkeyed; keyed and unkeyed specifications cannot be mixed. Duplicate keys are also rejected. Validation happens before hierarchy reconciliation so an invalid desired tree does not partially reorder the mounted hierarchy.

A matching key with a different exact type is a replacement, not reuse. For example, changing a keyed `Button` into a keyed `Label` with the same key disposes the button and mounts a new label. For an unkeyed list, inserting at the start shifts positional identities and may replace or recreate every later node. See [Reconciliation and keys](../public/reconciliation.md) for application-facing key guidance.

### Child matching and ordering

For keyed children, the reconciler indexes the old siblings by key, matches compatible types, and preserves the corresponding `MountedNode` and native `View`. Unkeyed children are matched positionally.

After compatible children have been recursively updated:

- unmatched old children are unmounted;
- unmatched desired children are mounted;
- retained and new native children are reordered to the requested order;
- every child's `Origin.Child` index is updated.

The reorder step computes a longest increasing subsequence of old indexes. Views already in a useful relative order remain in place, while the other views are moved using Terminal.Gui's adjacent-move or remove-and-`AddAt` operations. Reference identity is preserved even when a retained native view moves.

Some Terminal.Gui types, such as menus and popovers, are logically present in the desired child tree but are not attached as ordinary `SubViews`. Generated terminal-element metadata controls this through `SetAsChildOfParentView`; ownership-specific properties or Terminal.Gui services attach those views instead.

## Four ownership domains

The renderer deliberately does not treat every nested view as an ordinary child.

### Normal children

Normal children come from `Props.Children`. They are stored in `MountedNode.Children`, have sibling identity, and normally appear in the same order in the parent's native `SubViews` collection.

Use keys for dynamic lists where items can be inserted, removed, or reordered. Use unkeyed children for small, fixed-shape layouts where positional identity is intentional.

### View-valued property slots

A property such as `Shortcut.TargetView` owns a view through a named native property rather than through `SubViews`. Generated property keys give such a property two identities:

- a declarative `SubViewSpec`, containing the desired `IView`;
- a native `SubView`, containing the mounted `Terminal.Gui.View` assigned to the owner.

`MountedNode.Slots` reconciles these nodes by property ID. Slot reconciliation mounts or retains the slot subtree, then uses the generated property handler to assign its native root. It never inserts the slot into `SubViews`. When replacing or removing a slot, the owner property is cleared before the old slot is disposed so the owner cannot retain a disposed native reference.

Ordinary `Props.diff` excludes both halves of a slot. Slot reconciliation is the single owner of their lifecycle and assignment.

### Reconciled properties

Some declarative values require a mutable native resource whose lifetime is longer than one desired `Props` snapshot but shorter than, or equal to, the mounted terminal element. These values live in `Props.ReconciledProps`, separately from generated Terminal.Gui property IDs.

A `ReconciledPropSpec` provides:

- a stable, namespaced string key owned by the macro that declares the resource;
- an immutable value used for structural diffing;
- pre-mutation validation;
- a `BeforeNative` or `AfterNative` application phase;
- a retained-state factory;
- an apply operation that receives the retained state and a `ReconciledPropApplyContext` containing the native view, complete native-property lookup, and scoped event suppression service.

`ViewBackedTerminalElement` owns the resulting `IReconciledState`. `ReconciledPropSpec.create` infers the state type from its generic configuration, ensuring that the factory and apply callback agree without making the key part of that relationship. The macro-owned string is the resource contract: retaining it reuses existing state, while an incompatible implementation must use a new, versioned string. Removing the property, changing its string key, or unmounting the element disposes its state exactly once. Because a changed string appears as one removed property and one added property, removal runs first and application creates the replacement only after disposing the previous state.

The keyed collection adapter is the first consumer. `ListViewMacros.Items` and `DropDownListMacros.Items` convert immutable values into keyed text snapshots. The retained state owns one `ObservableCollection` and `ListWrapper`, synchronizes rows with minimal insert/move/replace/remove operations, and assigns the source once. This preserves Terminal.Gui selection and navigation without placing a stable source in the Elmish model or at application module scope.

Collection mutations can cause Terminal.Gui to raise selection or source events. `EventHandlerRegistrar.Suppress` uses scoped per-property counters so the adapter suppresses only callbacks induced by reconciliation. Native keyboard and mouse changes outside that scope continue through the stable event proxies normally.

The suppression sets are generated from each registered control's public events, including events declared by behavioral base classes down to, but not including, the general `View` base class. This makes a Terminal.Gui upgrade surface new control-specific events in generated keys and reflection-backed tests instead of relying on handwritten lists. The registry supports validated exclusions for callback-style hooks that must remain active: `ListView.RowRender`, for example, is deliberately excluded because it customizes rendering rather than reporting a state transition. Generation fails if an exclusion no longer names a real upstream event.

Raw `p.Source` remains an alternative native-property path. A specification cannot combine it with `m.Items`; reconciled-property validation rejects competing ownership before any mounted-tree mutation.

### Elmish components

An Elmish component is one node in its parent's mounted tree, identified by its component type and key. Internally it owns another Elmish model, loop, `TerminalRenderCoordinator`, renderer, render channel, and rendered root view. It receives the parent's shared `TerminalRenderContext`, so its later commits use the same `IRenderDispatcher` and `IApplication` as the root loop.

When component identity matches, the parent updates `ComponentProps` on the retained component instead of restarting it. The component's own loop continues to reconcile its child tree, so component-local model state follows the component through a keyed parent reorder. Replacing or removing the component terminates its loop and detaches its rendered root.

This boundary explains why the parent reconciler does not recursively inspect a component as ordinary `MountedNode.Children`: the component's renderer owns that subtree.

## Property and event patching

`Props` is the property snapshot attached to a simple view specification. Its native dictionary contains generated `PropKey` values with collision-checked integer property IDs and classifies entries as ordinary values, events, native view properties, or declarative view specifications. Its separate reconciled-property dictionary contains declarative values backed by terminal-element-owned resources.

For a retained simple view, `Props.diff` returns:

- property keys that existed previously but are now absent;
- a lazily allocated `Props` snapshot containing only added or changed ordinary properties and events.

`Props.diffAll` additionally returns removed and changed reconciled properties. Their string key, immutable `Value`, and phase determine whether lifecycle work is required. Changing a string key appears as an ordinary removal plus addition, expressing replacement through the remove-then-apply lifecycle. Keeping this key space separate means handwritten macros do not consume or collide with generated Terminal.Gui property IDs.

Removed event properties are unsubscribed by `EventHandlerRegistrar`. Other removed properties go through the generated integer-ID `ClearProp` dispatch, which restores the generated default for the correct concrete or base Terminal.Gui type. Added and changed values go through the specification's generated `ApplyNativeProps` dispatch.

Events need special treatment because an F# callback often closes over the model and therefore becomes a new delegate on every render. Each mounted event property installs one stable proxy delegate on the native event. The registrar separately stores the latest Elmish callback. Updating the property replaces that callback; it does not remove and re-add the native subscription. Removing the event property or disposing the element removes the proxy. Scoped suppression temporarily prevents selected proxies from invoking their current callbacks during renderer-owned native mutations without removing subscriptions.

As a concrete example, changing a retained label from `p.Text "before"` to `p.Text "after"` follows this path:

```text
new Label specification
  -> same child identity
  -> bind to retained LabelTerminalElement
  -> Props.diff finds Text changed
  -> generated Label/View property handler assigns existingLabel.Text
  -> Terminal.Gui redraws the same native Label
```

If `Text` is omitted in the next specification, `Props.diff` reports it as removed and generated clear dispatch resets the existing native label's text to its default value.

## Positioning, layout, and drawing

`TPos` is part of the declarative snapshot but is handled separately from generated ordinary properties. Absolute, centered, anchored, percentage, and alignment positions can be assigned directly. Relative positions such as `TPos.Bottom otherView` must resolve another desired specification to its bound retained terminal element.

`PositionService` applies these relationships and tracks cleanup actions for both participating terminal elements. Depending on whether the referenced native view exists yet, it waits for that view to be set or registers against its `DrawComplete` event. When a relevant position changes or either element is disposed, the old handlers are removed before new relationships are installed.

The renderer itself does not call `Layout`, `Draw`, or manipulate terminal cells. It changes native properties and hierarchy relationships on the application thread. Terminal.Gui observes those mutations, performs layout, marks dirty regions, and draws through its normal application iteration.

## Unmounting and disposal

`VirtualTree.unmount` first marks the complete mounted subtree as disposed so cleanup is idempotent, then delegates to the root terminal element's disposal behavior.

A view terminal element first disposes its retained reconciled-property resources, then clears applied native properties and event subscriptions, detaches itself when it is an attached child, disposes owned slot elements and children, removes position registrations, and finally disposes its native `View`. Component disposal requests termination of the component loop and waits for that loop to release its renderer and rendered tree.

At program shutdown, `TerminalRenderCoordinator.Dispose` prevents new renders, discards pending work, and disposes that loop's renderer under the commit guard. The root program then disposes the shared dispatcher and `IApplication`, allowing Terminal.Gui to restore terminal state and release its driver.

When adding a new rendering-owned resource, decide which terminal element owns it and add cleanup to that owner's idempotent disposal path. A retained node must not accumulate handlers or references from previous specifications.

## Generated and handwritten boundaries

The public DSL is mostly generated, but reconciliation policy is handwritten:

| Area | Responsibility |
| --- | --- |
| `View.gen.fs` and `Props.gen.fs` | Build typed desired view specifications and property snapshots. |
| `PKey.gen.fs` | Provide typed property keys, generated property IDs, and registered reconciliation event sets. |
| `SimpleViewSpec.gen.fs` | Cache or bind terminal elements and route typed set/clear operations. |
| `TerminalElement.Elements.gen.fs` | Construct the correct native Terminal.Gui subclass and expose view-specific metadata. |
| `PropsHandler.gen.fs` | Apply changed native values and clear removed values with inheritance-aware dispatch. |
| `Macros.fs` and `ReconciledCollections.fs` | Build higher-level declarative operations and their retained native-resource adapters. |
| `RenderContext.fs`, `Types.fs`, `TerminalElement.Base.fs`, `VirtualTree.fs`, `ElmishTerminal.fs` | Define the shared application context, per-loop coordination, identity, retained resources, event suppression, ownership, reconciliation, scheduling, and program/component integration. |

Generated files are evidence of the render-context bridge, not editing points. Change the corresponding source under `src/Terminal.Gui.Elmish.Generator/generators/` or its metadata/registry, then rebuild to regenerate and format the outputs.

## How to reason about a rendering change

Before modifying the renderer, answer these questions in order:

1. **What is the desired snapshot?** Identify the `ViewSpec`, `Props`, child list, or slot produced by the DSL.
2. **What establishes identity?** Decide whether the node should be retained by key or position and exact type, or deliberately replaced.
3. **Who owns the native object?** Distinguish a normal child, property slot, component root, or Terminal.Gui-managed special view.
4. **Is this mount-only or patchable?** A retained value needs generated equality, set, and clear behavior; an ownership change may require replacement.
5. **Which thread performs the mutation?** After initialization, native mutations belong inside the scheduled render commit.
6. **What must be cleaned up?** Account for subscriptions, relative-position handlers, native owner properties, component loops, hierarchy attachment, and disposal.
7. **Which invariant proves correctness?** Prefer assertions about native reference identity, final `SubViews` order, exact property values, and released resources.

The renderer assumes exclusive ownership of the hierarchy entries and property slots it mounts. External code that independently removes or reorders those native views can make `MountedNode.Children` diverge from `View.SubViews`; reconciliation detects several forms of this divergence and fails rather than silently publishing a false mounted state.

## Debugging map

| Symptom | Start here | Question to ask |
| --- | --- | --- |
| Focus, selection, or scroll state unexpectedly resets | `VirtualTree.sameIdentity` and child matching | Did the key, exact type, or unkeyed position change? |
| List navigation resets after an unrelated render | `ReconciledListItems` and `ViewBackedTerminalElement.ApplyReconciledProps` | Is the control using `m.Items` with stable domain keys, or is application code replacing `p.Source`? |
| Collection synchronization dispatches model messages | `EventHandlerRegistrar.Suppress` and the adapter's event-key scope | Is every native event induced by the resource mutation included in the scoped suppression set? |
| Dynamic children display in the wrong order | `reconcileChildren` and `reorderChildren` | Do keys match domain identity, and does final `SubViews` order match the desired list? |
| A nested property view appears as a normal child | `reconcileSlots` and generated subview keys | Was a view-valued property incorrectly put in `Children` or ordinary property diffing? |
| A removed native property keeps its old value | `Props.diff` and generated `clearProp` | Does the property have the correct generated ID and default clear case across inheritance? |
| An event fires multiple times or invokes stale model state | `EventHandlerRegistrar` | Is the callback being updated behind one proxy, and is removal routed as an event key? |
| A relative layout follows an old or disposed view | Specification binding and `PositionService` | Was the new spec bound to the retained element, and were pair cleanups executed? |
| Component-local state resets after a parent update | Component identity and `UpdateProps` | Was the same component type and key retained, or was a new loop mounted? |
| An update is not immediately visible | `TerminalRenderCoordinator.RequestRender` and `runRenderPump` | Was it replaced in that loop's capacity-one channel or waiting for the shared application-thread dispatcher? |
| A removed control remains referenced | `unmount`, terminal-element `Dispose`, and `Origin` | Which owner should detach it and remove its event/position registrations? |

## Suggested code-reading order

For a top-down trace through the current implementation:

1. Start with `ElmishTerminal.fs`: `setState`, `TerminalRenderCoordinator`, and `runTerminal` show when a desired tree is produced and committed.
2. Read `RenderContext.fs` to see the application-level `IApplication` and `IRenderDispatcher` shared by the root and component coordinators.
3. Read the core types in `Types.fs`: `Props`, `ReconciledPropSpec`, `ViewSpec`, terminal-element interfaces, `Origin`, and property diffing define the vocabulary used by the renderer.
4. Read `VirtualTree.fs`: mount, identity, child/slot reconciliation, reordering, validation, and renderer disposal are kept together.
5. Read `TerminalElement.Base.fs`: native view initialization, reconciled-resource state, scoped event suppression, stable event subscriptions, initial tree traversal, and disposal live here.
6. Read `Services/PositionService.fs` for relative-layout lifetime handling.
7. Inspect the generator sources for property/view-specific mechanics, using the corresponding `*.gen.fs` outputs to see the emitted code.
8. Use `RenderSchedulingTests.fs`, `VirtualTreeTests.fs`, `PositionServiceTests.fs`, and the component/Elmish-loop tests as executable statements of scheduling, reference retention, and cleanup behavior.

The [reconciler implementation plan](virtual-terminal-tree-reconciler-plan.md) records the design motivation and performance goals. Treat the current source and tests as authoritative where that historical plan still describes unimplemented alternatives or older intended structure.

## Invariants worth preserving

- A desired specification is an ephemeral description; compatible native view identity is retained state.
- Exact type plus key, or exact type plus unkeyed position, is the only reuse rule.
- The root runnable identity remains stable during an application session.
- Invalid sibling keys are rejected before hierarchy reconciliation.
- Normal children, view-valued property slots, and component-owned subtrees have distinct owners.
- Slots are assigned through native properties and never inserted into `SubViews` by slot reconciliation.
- Ordinary property diffing does not own slots; generated handlers are the only typed native set/clear layer.
- A retained event property has one native subscription and a replaceable current callback.
- Reconciled-property state belongs to the mounted terminal element, is diffed by immutable declarative value, and is disposed on removal or unmount.
- Renderer-owned native mutations suppress only the event proxies they can induce; user input remains observable.
- A native property and a reconciled property cannot compete to own the same resource.
- Commits after application initialization run through the Terminal.Gui application thread.
- Reconciliation mutates views; Terminal.Gui performs layout and drawing.
- Unmount and program shutdown release hierarchy links, callbacks, position handlers, component loops, native views, and application resources exactly once.
