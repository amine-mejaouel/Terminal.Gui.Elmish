# Troubleshooting

Start with the symptom, then check the ownership or identity rule behind it.

## Build and setup

### A documented member is missing

These guides target the current .NET 10 repository source. If the project references the older published package, newer macros and reconciler behavior will be unavailable. Use the [source setup](getting-started.md#prepare-the-source-and-application) and check the resolved assembly reference.

### A heterogeneous children list does not type-check

Upcast entries to `IView`:

```fsharp
p.Children
  [ View.Label(fun p -> p.Text "Name") :> IView
    View.Button(fun p -> p.Text "Save") :> IView ]
```

### A command has `Msg` where `TerminalMsg<Msg>` is expected

Wrap messages emitted by commands and view callbacks:

```fsharp
dispatch (TerminalMsg.ofMsg Saved)
```

Subscriptions passed to `ElmishTerminal.withSubscription` remain `Sub<Msg>` and are mapped by the wrapper.

## Identity and rendering

### Focus or local state resets after an update

Check whether the control moved in an unkeyed list, changed exact type, changed key, or was temporarily removed. Dynamic sibling lists need stable domain keys. See [Reconciliation and keys](reconciliation.md).

### The renderer reports mixed keyed and unkeyed children

Once one sibling has `p.Key`, every sibling in that same `p.Children` list must have a key. Keys are local to that parent.

### The renderer reports a duplicate key

Two desired siblings or collection rows use the same identity. Do not use a non-unique label or a reused index. Use a stable domain ID.

### The root identity cannot change

Always return one stable `View.Runnable` from the root view function. Put authentication, loading, or page alternatives inside its children rather than switching the root type.

### A component loses its local model

Give dynamic components stable `ComponentProps.Key` values and keep the component's internal root exact type stable. A changed key/type or removal intentionally starts a new component loop.

## Inputs and collections

### Typing immediately reverts

The field is controlled by `p.Text` or `p.Value`, but `ValueChanged` is not updating the same model value—or validation is replacing it on every keypress. Trace the event message through `update` and back into the declared property.

### Arrow keys select briefly and then jump back

If selection is controlled, ensure `ValueChanged` updates the selected domain ID and that the current index is derived from that ID. Do not persist an index across filtering or reordering.

For `ListView` or `DropDownList`, use `m.Items` instead of constructing a new `ObservableCollection`, `ListWrapper`, or `IListDataSource` in `view`.

### `p.Source` and `m.Items` fail together

They are alternative owners of the same native source. Use `m.Items` for immutable Elmish values, or use `p.Source` when the application intentionally owns a custom `IListDataSource`, never both.

### A collection rejects duplicate strings

The short string overload uses each string as both display text and key. Use the keyed overload when labels can repeat:

```fsharp
m.Items(items, (fun item -> item.Id), (fun item -> item.Label))
```

## Layout and interaction

### A relative view is positioned incorrectly

Keep the referenced desired view in a local binding from the same `view` call and pass it to `TPos.Bottom`, `Right`, `X`, or another relative form. Do not cache a previous specification or native control.

### A shortcut does not fire globally

Set `p.BindKeyToApplication true`, ensure the shortcut is mounted, and check that another focused control is not intentionally handling the key first.

### An action fires twice or uses stale model state

Declare the event once in the current builder. Do not manually subscribe to the native view in addition to the props event. Terminal.Gui.Elmish keeps one native proxy and updates its current Elmish callback.

## Native interop

### Direct Terminal.Gui mutations disappear or break reconciliation

The renderer assumes ownership of the native properties, child hierarchy, view slots, and reconciled resources declared by the desired tree. Avoid independently reordering/removing mounted `SubViews` or replacing sources behind the renderer.

When an upstream feature has no generated property, first confirm whether the generator should expose it. Treat imperative native interop as an escape hatch with explicit ownership and cleanup, not a routine application pattern.

## Still stuck?

Reduce the issue to:

1. the smallest model and message;
2. one stable `Runnable` root;
3. the affected desired child list and keys;
4. the props declared before and after the update;
5. whether the trigger was user input or renderer-owned reconciliation.

Then compare the relevant control behavior with the upstream Terminal.Gui examples/tests and include both versions when reporting an issue.
