# Plan: Add `IBasePosPropKey`, `IPosPropKey<'a>`, and `PosPropKey<'a>` to the Prop Key Hierarchy

Introduce `IBasePosPropKey` (inherits only `IPropKey`) as a marker base for all position-related keys. Create `IPosPropKey<'a>` (inherits `IBasePosPropKey` and `IPropKey<'a>`) for simple X/Y position keys, and update `IDelayedPosKey` to also inherit `IBasePosPropKey`. Add a private `PosPropKey<'a>` implementation and a `PropKey.Create.pos` factory method.

## F# Type Ordering Constraints

All new types must respect F#'s top-down dependency rule within [Types.fs](src/Terminal.Gui.Elmish/Types.fs). The current interface order inside `module internal PropKey` (starting line 33) is:

```
IPropKey → IPropKey<'a> → ISimplePropKey<'a> → IViewPropKey<'a> → ISingleElementPropKey → ISingleElementPropKey<'a> → IMultiElementPropKey<'a> → IDelayedPosKey → IEventPropKey<'a>
```

Since `IDelayedPosKey` and `IPosPropKey<'a>` both inherit `IBasePosPropKey`, it must be defined **before** both of them. The required insertion order is:

```
... → IMultiElementPropKey<'a> → IBasePosPropKey (NEW) → IPosPropKey<'a> (NEW) → IDelayedPosKey (UPDATED) → IEventPropKey<'a>
```

Similarly, the private `PosPropKey<'a>` implementation must appear **after** `IPosPropKey<'a>` is defined and **before** `PropKey.Create` (where the factory method is added).

## Steps

### 1. Define `IBasePosPropKey` in Types.fs

In [Types.fs](src/Terminal.Gui.Elmish/Types.fs), **before** the existing `IDelayedPosKey` definition (line 63), after `IMultiElementPropKey<'a>` (line 61), add:

```fsharp
  type IBasePosPropKey =
    inherit IPropKey
```

`IBasePosPropKey` inherits only `IPropKey`. It serves as a marker interface for all position-related prop keys.

### 2. Define `IPosPropKey<'a>` in Types.fs

Right after `IBasePosPropKey`, still before `IDelayedPosKey`, add:

```fsharp
  type IPosPropKey<'a> =
    inherit IBasePosPropKey
    inherit IPropKey<'a>
```

This is for simple X/Y position keys (those that hold a `Pos` value directly).

### 3. Update `IDelayedPosKey` in Types.fs

Change `IDelayedPosKey` (currently line 63) to inherit both `IBasePosPropKey` and `IPropKey<TPos>`:

```fsharp
  type IDelayedPosKey =
    inherit IBasePosPropKey
    inherit IPropKey<TPos>
```

### 4. Add private `PosPropKey<'a>` implementation in Types.fs

**After** `DelayedPosKey` (ends ~line 244) and **before** `PropKey.Create` module (line 246), add a new private DU type that mirrors `SimplePropKey` but implements `IPosPropKey<'a>`:

```fsharp
  [<CustomEquality; NoComparison>]
  type private PosPropKey<'a> =
    private
    | Key of string

    static member create<'a>(key: string) : IPosPropKey<'a> =
      if key.EndsWith ".X" || key.EndsWith ".Y" then
        Key key
      else
        failwith $"Invalid pos key: {key}"

    member private this.key =
      let (Key key) = this
      key

    override this.GetHashCode() = this.key.GetHashCode()

    override this.Equals(obj) =
      match obj with
      | :? IPropKey as x -> this.key.Equals(x.key)
      | _ -> false

    interface IPosPropKey<'a> with
      member this.key = this.key
      member this.isViewKey = false
      member this.isSingleElementKey = false
```

Validation: key must end with `".X"` or `".Y"`. Only `View.X` and `View.Y` use this type currently.

### 5. Add `PropKey.Create.pos` factory method in Types.fs

In the `PropKey.Create` type (~line 248), add alongside the other factory methods:

```fsharp
      static member pos key = PosPropKey.create key
```

### 6. Update generated `PKey.gen.fs`

In [PKey.gen.fs](src/Terminal.Gui.Elmish/PKey.gen.fs) (lines 61, 63), change:

```fsharp
// Before:
member val X: ISimplePropKey<Pos> = PropKey.Create.simple "View.X"
member val Y: ISimplePropKey<Pos> = PropKey.Create.simple "View.Y"

// After:
member val X: IPosPropKey<Pos> = PropKey.Create.pos "View.X"
member val Y: IPosPropKey<Pos> = PropKey.Create.pos "View.Y"
```

### 7. Update the PKey generator

In [generator/.../PKey.gen.fs](src/generator/Terminal.Gui.Elmish.Generator/PKey.gen.fs) (line 30), change the `if prop.PKey = "X" || prop.PKey = "Y"` branch:

```fsharp
// Before:
yield $"    member val {prop.PKey}: ISimplePropKey<Pos> = PropKey.Create.simple \"{keyName}\""

// After:
yield $"    member val {prop.PKey}: IPosPropKey<Pos> = PropKey.Create.pos \"{keyName}\""
```

## Further Considerations

1. **No downstream breakage expected**: `IPosPropKey<'a>` inherits `IPropKey<'a>`, so all call sites that use `Props.tryFind PKey.View.X` (typed as `IPropKey<'a>`) will continue to compile. `Props.add` also accepts `IPropKey<'a>`.
2. **`Props.gen.fs` (generated output)**: The `this.X(value: Pos)` / `this.Y(value: Pos)` methods call `this.props.add(PKey.View.X, value)`. Since `Props.add` accepts `IPropKey<'a>` and `IPosPropKey<'a>` inherits it, **no code change is needed** in `Props.gen.fs` or its generator.
3. **`PositionService.ApplyPos`**: The `Props.tryFind PKey.View.X` / `PKey.View.Y` calls will continue to work since `IPosPropKey<'a>` inherits `IPropKey<'a>` — **no code change needed**.
4. **`PosPropKey` validation**: Restricting to keys ending with `".X"` or `".Y"` is simple and sufficient — only `View.X` and `View.Y` use this type currently.
