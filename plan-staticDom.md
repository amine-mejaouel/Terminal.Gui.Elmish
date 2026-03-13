# Static DOM: Persistent DomNode Tree

## Context

Currently, `ViewBackedTerminalElement` serves as both the virtual description (created each render by `View.Button(...)` etc.) and the real view owner (holds the actual `Terminal.Gui.View`). On updates, the new TE "steals" the View from the old TE via `Reuse()`. This coupling makes it hard to:

1. **Implement a new differ** — the current differ compares TE-to-TE and mutates TEs in place. A cleaner differ would compare a new TE description against the current persistent state.
2. **Test view logic** — TEs can't be inspected or compared without real Terminal.Gui Views.

The goal is to introduce a **persistent DomNode tree** that owns Views, making TEs pure ephemeral descriptions.

## New Architecture

```
view(model) → TE tree (pure data, no View)
                ↓
Differ compares new TE tree against persistent DomNode tree
                ↓
DomNode tree patched in-place:
  - DomNode owns View, EventRegistrar, applied props
  - DomNode persists across renders
  - TE tree discarded after diff
```

---

## Phase 1: Introduce DomNode type (additive)

**New file**: `src/Terminal.Gui.Elmish/DomNode.fs` (add to fsproj after `TerminalElement.Base.fs`)

```fsharp
type internal DomNode(
    name: string,
    view: View,
    appliedProps: Props,
    applyProps: DomNode * Props -> unit,
    removeProps: DomNode * Props -> unit,
    newView: unit -> View,
    setAsChildOfParentView: bool,
    subElementsPropKeys: RawPropKey list
) =
    member val Name = name
    member val View = view with get, set
    member val Origin: DomOrigin = DomRoot with get, set
    member val AppliedProps = appliedProps with get, set
    member val EventRegistrar = EventHandlerRegistrar() with get, set
    member val Children = ResizeArray<DomChild>() with get
    member val SetAsChildOfParentView = setAsChildOfParentView
    member val SubElementsPropKeys = subElementsPropKeys
    member this.ApplyProps(props) = applyProps(this, props)
    member this.RemoveProps(props) = removeProps(this, props)
    member this.NewView() = newView()

and internal DomChild =
    | ViewDomChild of DomNode
    | ElmishComponentDomChild of IElmishComponentTE

and internal DomOrigin =
    | DomRoot
    | DomElmishComponent of parent: IElmishComponentTE
    | DomChild of parent: DomNode * index: int
    | DomSubElement of parent: DomNode * index: int option * property: RawPropKey
```

**Also add**: `DomNode` module with helper functions:
- `createFromTE: ViewBackedTerminalElement -> DomNode` — creates a DomNode from a TE, calling `NewView()`, `SetProps()`, etc.
- `initializeTree: DomOrigin -> TerminalElement -> DomChild` — recursively creates DomNode tree
- `dispose: DomNode -> unit` — recursively disposes views and cleans up

**Files to modify**:
- `Terminal.Gui.Elmish.fsproj` — add `DomNode.fs` after `TerminalElement.Elements.gen.fs`

## Phase 2: Generate applicator functions

Each generated element type (e.g., `ButtonTerminalElement`) exposes a "spec" so DomNode can call the right SetProps/RemoveProps/NewView without inheriting from `ViewBackedTerminalElement`.

**Modify generator**: `src/Terminal.Gui.Elmish.Generator/TerminalElement.Elements.gen.fs`

For each view type, generate a companion module using **composition chain** — each module calls its parent's applicator, mirroring the current `base.SetProps`/`base.RemoveProps` pattern:

```fsharp
module internal ViewDom =
    let applyProps (domNode: DomNode, props: Props) =
        let view = domNode.View
        props |> Props.tryFind PKey.View.BorderStyle |> Option.iter (fun v -> view.BorderStyle <- v)
        props |> Props.tryFind PKey.View.CanFocus |> Option.iter (fun v -> view.CanFocus <- v)
        // ... all View-level props and events
        EventHandlerHelpers.trySetEventHandler domNode.EventRegistrar props PKey.View.Accepted view.Accepted
        ...

    let removeProps (domNode: DomNode, props: Props) =
        let view = domNode.View
        props |> Props.tryFind PKey.View.BorderStyle |> Option.iter (fun _ -> view.BorderStyle <- Unchecked.defaultof<_>)
        ...

    let newView () = new View() :> View
    let subElementsPropKeys = [ PKey.View.DefaultAcceptView_element.key ]

module internal ButtonDom =
    let applyProps (domNode: DomNode, props: Props) =
        ViewDom.applyProps(domNode, props)  // ← calls parent
        let view = domNode.View :?> Button
        props |> Props.tryFind PKey.Button.Text |> Option.iter (fun v -> view.Text <- v)
        props |> Props.tryFind PKey.Button.IsDefault |> Option.iter (fun v -> view.IsDefault <- v)
        ...

    let removeProps (domNode: DomNode, props: Props) =
        ViewDom.removeProps(domNode, props)  // ← calls parent
        let view = domNode.View :?> Button
        props |> Props.tryFind PKey.Button.Text |> Option.iter (fun _ -> view.Text <- "")
        ...

    let newView () = new Button() :> View
    let subElementsPropKeys = ViewDom.subElementsPropKeys  // inherits parent's list
```

This mirrors the existing inheritance: `ButtonTerminalElement : ViewTerminalElement : ViewBackedTerminalElement`, but as module-to-module calls instead of `base.SetProps`.

**Also add** to each TE class a `DomNodeSpec` member:

```fsharp
member _.ApplyProps = ButtonDom.applyProps
member _.RemoveProps = ButtonDom.removeProps
member _.CreateView = ButtonDom.newView
member _.DomSubElementsPropKeys = ButtonDom.subElementsPropKeys
```

**Extract** `TrySetEventHandler`/`TryRemoveEventHandler` helper functions from `ViewBackedTerminalElement` into a standalone `EventHandlerHelpers` module that accepts `(EventHandlerRegistrar, Props, PropKey, event)`. Used by both old TE code path and new DomNode applicators.

**Files to modify**:
- `src/Terminal.Gui.Elmish.Generator/TerminalElement.Elements.gen.fs` — add module generation per view type
- `src/Terminal.Gui.Elmish/TerminalElement.Base.fs` — extract event handler helpers into standalone module

## Phase 3: DomNode-based differ

**New file or addition to `DomNode.fs`**: `DomDiffer` module

```fsharp
module internal DomDiffer =
    let update (currentDom: DomNode) (newTE: IViewTE) : unit =
        // Compare currentDom.AppliedProps vs newTE.Props
        // Call RemoveProps for removed, ApplyProps for changed
        // Recurse into children: match DomNode.Children vs newTE.Children
        // New children → DomNode.initializeTree
        // Removed children → DomNode.dispose
        // Same children → recurse
```

Key difference from current `Differ.update`: it takes `(DomNode, TerminalElement)` instead of `(TerminalElement, TerminalElement)`. The DomNode is mutated in-place. No more `Reuse()`.

This also enables switching to the `TwoEndedScanDiffer` algorithm more easily, since the persistent DomNode tree gives stable identity for matching.

**Files to modify/create**:
- `src/Terminal.Gui.Elmish/DomNode.fs` — add DomDiffer module

## Phase 4: Wire into ElmishTerminal

**Modify**: `src/Terminal.Gui.Elmish/ElmishTerminal.fs`

- `TerminalElementState` stores `DomNode` instead of `IViewTE`
- `setState()` changes:
  - First render: `view() → TE → DomNode.initializeTree(DomRoot, te) → store DomNode`
  - Subsequent: `view() → TE → DomDiffer.update(currentDomNode, newTE)` — DomNode mutated in place, no disposal needed
- `ElmishComponentTE` stores `DomNode` for its child subtree

**Files to modify**:
- `src/Terminal.Gui.Elmish/ElmishTerminal.fs`

## Phase 5: Update PositionService

**Modify**: `src/Terminal.Gui.Elmish/Services/PositionService.fs`

- `ApplyPos` accepts `DomNode` instead of `ViewBackedTerminalElement`
- `ExecuteCleanups` accepts `DomNode`
- Maintain a mapping from `ITerminalElement` (TPos targets) to `DomNode` during tree init/diff, so relative positioning can resolve TEs to their DomNodes

**Files to modify**:
- `src/Terminal.Gui.Elmish/Services/PositionService.fs`

## Phase 6: Update tests

**Modify**: `src/Terminal.Gui.Elmish.Tests/Helpers/ElmishTester.fs`

- `TestableElmishProgram` exposes `DomNode` instead of `IViewTE`
- `ProcessMsg` returns/updates `DomNode`
- Tests access `domNode.View` instead of `viewTE.View`, `domNode.Children` instead of `viewTE.Children`

**New test capabilities** (once DomNode is in place):
- Assert on DomNode tree structure without querying Terminal.Gui Views
- Unit test the differ with mock DomNodes
- Compare TE trees as pure data

**Files to modify**:
- `src/Terminal.Gui.Elmish.Tests/Helpers/ElmishTester.fs`
- `src/Terminal.Gui.Elmish.Tests/Helpers/TerminalElementsExt.fs`
- All test files that reference `IViewTE`

## Phase 7: Clean up old path

Once all tests pass with DomNode:

- Remove `View`, `EventRegistrar`, `Origin`, `Reuse()`, `InitializeTree()`, `InitializeView()`, `Dispose()` from `ViewBackedTerminalElement`
- Remove old `Differ.update` (replaced by `DomDiffer.update`)
- Remove `IViewTE.Reuse`, `IViewTE.InitializeTree` from interface
- `ViewBackedTerminalElement` becomes a lightweight description: `Name + Props + Children + DomNodeSpec`
- Remove `Origin` type (replaced by `DomOrigin`)

**Files to modify**:
- `src/Terminal.Gui.Elmish/Types.fs`
- `src/Terminal.Gui.Elmish/TerminalElement.Base.fs`
- `src/Terminal.Gui.Elmish/Differ.fs` (delete or gut)

---

## Critical files

| File | Role |
|------|------|
| `src/Terminal.Gui.Elmish/Types.fs` | Core types — Origin, IViewTE, TerminalElement DU |
| `src/Terminal.Gui.Elmish/TerminalElement.Base.fs` | ViewBackedTerminalElement — loses view ownership |
| `src/Terminal.Gui.Elmish/Differ.fs` | Current differ — replaced by DomDiffer |
| `src/Terminal.Gui.Elmish/ElmishTerminal.fs` | Elmish loop — switches to DomNode storage |
| `src/Terminal.Gui.Elmish/Services/PositionService.fs` | Positioning — adapts to DomNode |
| `src/Terminal.Gui.Elmish.Generator/TerminalElement.Elements.gen.fs` | Generator — adds applicator modules |
| `src/Terminal.Gui.Elmish.Tests/Helpers/ElmishTester.fs` | Test harness — exposes DomNode |

## Reusable existing code

- `ViewBackedTerminalElement.compare(prevProps, curProps)` — props diffing logic, reuse in DomDiffer
- `EventHandlerRegistrar` — reuse as-is, just lives on DomNode instead of TE
- `Props` type and all its utilities — unchanged
- `PropKey` system — unchanged
- All generated `PKey.*` definitions — unchanged
- `Props.gen.fs` / `View.gen.fs` — unchanged (still create TEs the same way)

## Verification

1. **Build**: `dotnet build` — generator runs, new code compiles
2. **Unit tests**: `dotnet test` — all existing tests pass (adapted to DomNode)
3. **Manual test**: Run the examples (`src/Terminal.Gui.Elmish.Example/`) — UI works, updates work, positioning works
4. **Memory test**: The existing weak-reference test in `ElmishLoopTests.fs` still passes (disposed views are collected)
5. **New differ test**: Write a test that creates a DomNode, diffs with a new TE, and verifies the DomNode is patched correctly without creating new Views
