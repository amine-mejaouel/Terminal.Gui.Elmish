# Review of plan-staticDom.md

## ~~1. TPos holds ephemeral ITerminalElement references — but PositionService needs persistent identity~~

**RESOLVED**: PositionService is now hidden behind the DomNode layer. The `initializeTree` and `DomDiffer.update` functions build a `TE→DomNode` mapping as they traverse the tree. TPos's `ITerminalElement` references are resolved to DomNodes via this mapping before calling PositionService. PositionService only works with DomNode references and never sees `ITerminalElement`.

---

## ~~2. `Reuse()` does far more than the DomDiffer description suggests~~

**RESOLVED**: Phase 3 now details all operations the DomDiffer must perform per node:
- **Sub-element handling**: Unchanged sub-elements keep their DomNode (no transfer needed). Changed/new/removed sub-elements are recursed into, created, or disposed. The `_view` prop re-injection pattern from `Reuse()` is eliminated — DomNode owns sub-element DomNodes directly.
- **EventRegistrar**: Lives on the persistent DomNode, no transfer needed.
- **PositionService**: ExecuteCleanups called before re-applying positions with resolved DomNode references.

Phase 1 now also details `createFromTE` initialization ordering (sub-elements before `applyProps`) and `dispose` cleanup steps.

---

## 3. `TerminalElement.Interfaces.fs` (OrientationInterface) is not mentioned

This file contains `OrientationInterface.setProps`/`removeProps` which are called from generated TE code (e.g., `SliderTerminalElement`). These methods take `ViewBackedTerminalElement` and call `terminalElement.TrySetEventHandler(...)`. The plan needs to generate DomNode equivalents for interface-based property applicators too, not just per-view-type modules.

---

## 4. No gradual migration path — Phases 3–7 are big-bang

Phases 1–2 are additive, but Phase 3 (new differ), Phase 4 (rewire ElmishTerminal), Phase 5 (PositionService), and Phase 6 (tests) must all land together for anything to work. There's no way to run DomNode for *some* elements while keeping the old path for others. If any of these phases has a bug, the entire system is broken.

**Suggestion**: Consider a Phase 2.5 that validates DomNode in isolation — create a DomNode from a TE tree, then verify the DomNode's `appliedProps` match the TE's `Props`, the View tree matches, etc. — *without* replacing the rendering pipeline. This gives a safety net before the big switch.

---

## 5. ElmishComponentTE's internal tree is not addressed

The plan says `DomChild = ElmishComponentDomChild of IElmishComponentTE`, treating components as opaque leaves. But each `ElmishComponentTE` internally runs its own Elmish loop that calls `setState`, which calls `Differ.update` with its own TE trees.

After Phase 4 rewires `setState` to use DomNode, the component's internal `setState` (created in `mkSimpleComponent`) also uses the *same* `setState` function. This means ElmishComponentTE's internal tree **automatically** becomes DomNode-based too. The plan should explicitly acknowledge this — it's not optional, it's forced by the shared `setState`.

Also: `ElmishComponentTE.Child` returns `IViewTE`, but after Phase 4, the stored state is `DomNode`. Tests that access `component.Child.Children` would need to go through DomNode. The plan's Phase 6 should address this.

---

## ~~6. `InitializeSubElements` lifecycle is entangled with Props mutation~~

**RESOLVED**: Phase 1 now details the exact `createFromTE` initialization ordering: (1) create View, (2) initialize sub-element DomNodes and inject `_view` props, (3) apply positions, (4) call `applyProps`. This ensures `_view` props are available when `applyProps` reads them.

---

## 7. `View` setter has a one-time guard

`ViewBackedTerminalElement.View` setter (line 146) throws if set twice: `if (view <> null) then failwith "View has already been set."`. This works because TEs are ephemeral — each gets one View in its lifetime.

The plan's DomNode has `member val View = view with get, set`. If a DomNode needs to change its View (e.g., element type changes from Button to Label at the same tree position), the plan says "dispose and re-create DomNode." But consider: is DomNode disposal + re-creation equivalent to mutating the View in place? The parent DomNode's `Children` list would need updating too. The plan should clarify this replacement protocol.

---

## 8. Props comparison for sub-elements uses `equivalentTo()` on TEs

`ViewBackedTerminalElement.compare()` (line 377) calls `curElement.equivalentTo(oldElement)` when comparing sub-element props. But `oldElement` would come from `DomNode.AppliedProps` — which stores the *previous* TE's sub-element. After the first DomNode render, is the original TE still alive? The plan says "TE tree discarded after diff." If the sub-element TE in AppliedProps is from a discarded tree, comparing it against a new TE should still work (the TE objects hold their Props), but this implicit requirement should be documented.

Alternatively: DomNode.AppliedProps should store a **copy** or the comparison-relevant data, not a reference to a potentially-GC'd TE.

---

## 9. The Differ's child-matching algorithm should be preserved exactly

The current differ matches children by type-name grouping and positional index within each group. It's naive but the entire test suite and all examples depend on it. The plan mentions "enables switching to TwoEndedScanDiffer" but doesn't state whether Phase 3's DomDiffer should use the **exact same** algorithm initially. Changing the diffing algorithm simultaneously with the DomNode architecture would make failures very hard to diagnose.

**Suggestion**: Phase 3 DomDiffer should be a direct port of the current algorithm, with TwoEndedScanDiffer as a separate future phase.

---

## Minor notes

- **Dispose protocol**: Current `Dispose()` calls `RemoveProps(this, this.Props)` to unregister all events. The plan's `DomNode.dispose` must do this too — it's not mentioned.
- **`ViewSet` event**: Used by PositionService for lazy positioning when a relative element's View doesn't exist yet. DomNode creates its View eagerly at construction time, so this event pattern may become unnecessary — but only if DomNodes are always created in parent-first order.
- **Generator scope**: The generator currently produces `SetProps(ViewBackedTerminalElement, Props)` signatures. During the transition (Phases 2–6), both old and new signatures coexist. The generator would need to emit the DomNode modules *in addition to* the existing TE overrides until Phase 7 removes the old ones.
