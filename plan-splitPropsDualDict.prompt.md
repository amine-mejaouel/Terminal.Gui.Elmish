# Plan: Split `Props` into Dual Dictionaries for Pos and Non-Pos Keys

Replace the single `dict` in the `Props` type with two separate dictionaries — `posPropsDict` for position-related keys (`IPosBasePropKey<_>`) and `nonPosPropsDict` for everything else. This structurally prevents position props from leaking into the `compare`/diff flow, which already handles them separately via `PositionService`.

**Prerequisite:** The `IPosBasePropKey<'a>` type hierarchy from [plan-basePosPropKey](plan-basePosPropKey.prompt.md) is already implemented. `IPosBasePropKey<'a>` is the marker base inherited by both `IPosPropKey` (for simple X/Y `Pos` keys) and `IDelayedPosKey` (for delayed `TPos` keys). This gives us a type-level way to route keys to the correct dictionary.

## F# Type Ordering Constraints

`Props` (line 347 of [Types.fs](src/Terminal.Gui.Elmish/Types.fs)) is defined after `IPosBasePropKey<'a>` (line 63) in the file, so it can reference `IPosBasePropKey<_>` for the routing check. The `Props` module (line 358) follows `Props` and can also reference `IPosBasePropKey<_>`. No file-order issues.

## Steps

### 1. Replace `dict` with `posPropsDict` and `nonPosPropsDict` in `Props` class

In [Types.fs](src/Terminal.Gui.Elmish/Types.fs) (lines 347–355), replace:

```fsharp
member val dict = Dictionary<IPropKey, _>() with get
```

with:

```fsharp
member val posPropsDict = Dictionary<IPropKey, _>() with get
member val nonPosPropsDict = Dictionary<IPropKey, _>() with get
```

Add a private static routing helper:

```fsharp
static member private isPosKey (k: IPropKey) = k :? IDelayedPosKey || k :? IPosPropKey
```

Update the instance methods to route to the correct dictionary:

- `addNonTyped(k, v)`: if `isPosKey k` → `posPropsDict.Add(k, v)`, else → `nonPosPropsDict.Add(k, v)`.
- `add(k, v)`: delegates to `addNonTyped` (unchanged call, routing happens there).
- `getOrInit(k)(init)`: look up from the dict selected by `isPosKey k`.
- `remove(k)`: remove from the dict selected by `isPosKey k`.
- `tryFind(key)`: look up from the dict selected by `isPosKey key`.

### 2. Update `Props` module functions to operate on `nonPosPropsDict`

In [Types.fs](src/Terminal.Gui.Elmish/Types.fs) (lines 358–414), update all `Props` module functions to use `nonPosPropsDict` instead of `dict`. These functions are only used in the compare/diff/SetProps/RemoveProps flow which should never touch pos keys:

- `partition`: iterate `props.nonPosPropsDict`, add to `first.nonPosPropsDict` / `second.nonPosPropsDict` via `addNonTyped`.
- `filter`: iterate `props.nonPosPropsDict`.
- `tryFind`: look up from `props.nonPosPropsDict`.
- `tryFindByRawKey`: look up from `props.nonPosPropsDict`.
- `find`: delegates to `tryFind` (no direct change needed).
- `tryFindWithDefault`: delegates to `tryFind` (no direct change needed).
- `rawKeyExists`: check `props.nonPosPropsDict`.
- `exists`: check `props.nonPosPropsDict`.
- `keys`: enumerate `props.nonPosPropsDict.Keys`.
- `filterSingleElementKeys`: enumerate `props.nonPosPropsDict.Keys`.
- `iter`: iterate `props.nonPosPropsDict`.

### 3. Update `equivalentTo` in `TerminalElement.Base.fs`

In [TerminalElement.Base.fs](src/Terminal.Gui.Elmish/TerminalElement.Base.fs) (line 323), change:

```fsharp
// Before:
this.Props.dict.GetEnumerator()

// After:
this.Props.nonPosPropsDict.GetEnumerator()
```

Position props are excluded from equivalence checks — they are already handled separately by `PositionService`.

### 4. Remove TODO comments in `Reuse`

In [TerminalElement.Base.fs](src/Terminal.Gui.Elmish/TerminalElement.Base.fs) (lines 294–297), remove the three TODO comments:

```fsharp
// TODO: it seems that comparing x_delayedPos/y_delayedPos is working well
// TODO: this should be tested and documented to make sure that it continues to work well in the future.

// TODO: Should refactor props to be clear that X and Y are treated separately
```

The `compare` call on line 298 passes `Props` objects, but `compare` internally uses `Props` module functions which now operate on `nonPosPropsDict` — so pos keys are structurally excluded. **No code change needed in `compare` itself.**

### 5. Update `PositionService.ApplyPos` to use instance `tryFind`

In [PositionService.fs](src/Terminal.Gui.Elmish/Services/PositionService.fs) (lines 148–151), change from `Props.tryFind` (module function, now non-pos only) to the instance method `viewTe.Props.tryFind` (which routes to the correct dict based on key type):

```fsharp
// Before:
let xPos = viewTe.Props |> Props.tryFind PKey.View.X
let yPos = viewTe.Props |> Props.tryFind PKey.View.Y
let delayedXPos = viewTe.Props |> Props.tryFind PKey.View.X_delayedPos
let delayedYPos = viewTe.Props |> Props.tryFind PKey.View.Y_delayedPos

// After:
let xPos = viewTe.Props.tryFind PKey.View.X
let yPos = viewTe.Props.tryFind PKey.View.Y
let delayedXPos = viewTe.Props.tryFind PKey.View.X_delayedPos
let delayedYPos = viewTe.Props.tryFind PKey.View.Y_delayedPos
```

### 6. Verify no other callers use `Props` module functions with pos keys

Callers to audit (all confirmed non-pos):
- `ViewBackedTerminalElement.Children` getter (line 148 of `TerminalElement.Base.fs`): uses `Props.tryFind PKey.View.children` — non-pos key ✓
- `ViewBackedTerminalElement.InitializeView` (line 175): calls `this.SetProps(this, this.Props)` — generated `SetProps` uses `Props.tryFind` which now only searches `nonPosPropsDict`, matching the fact that generated `SetProps` never accesses pos keys ✓
- `ViewBackedTerminalElement.Dispose` (line 400): calls `this.RemoveProps(this, this.Props)` — generated `RemoveProps` never accesses pos keys ✓
- `TrySetEventHandler` (lines 244–263): uses `this.Props.tryFind` (instance method) — event keys are non-pos, routed correctly ✓
- `Macros.fs`: uses `props.getOrInit` / `props.add` with non-pos keys ✓

## Further Considerations

1. **`InitializeView` passes full `Props` to `SetProps`** (line 175): Generated `SetProps` only does `Props.tryFind` (module function) lookups for non-pos keys, which now search `nonPosPropsDict`. Position keys sit in `posPropsDict` and are never accessed by `SetProps`. No change needed.
2. **`addNonTyped` at line 172**: `InitializeSubElements` produces view-key pairs (not pos keys), so they route to `nonPosPropsDict` correctly.
3. **`addNonTyped` at line 314 (Reuse)**: Re-injects `_view` props from `removedProps` — these are view keys, not pos keys, so they route to `nonPosPropsDict` correctly.
4. **`partition` and `filter` create new `Props()`**: Items added via `addNonTyped` will be routed by key type. Since these functions now only iterate `nonPosPropsDict`, only non-pos keys will be added — routing them back to `nonPosPropsDict` on the new `Props` instances. This is consistent.

