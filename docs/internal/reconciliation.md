# Retained view reconciliation

Terminal.Gui.Elmish retains compatible `Terminal.Gui.View` instances between Elmish renders. Property-only changes are patched in place, so focus, selection, scrolling, component state, and event subscriptions survive when a node keeps the same identity.

## Keys

Dynamic sibling lists should give every sibling a stable key derived from domain identity:

```fsharp
View.Runnable(fun p ->
  p.Children
    [ for item in model.Items do
        View.Label(fun p ->
          p.Key item.Id
          p.Text item.Name) ])
```

Keys are local to one parent. A keyed node is retained only when both its key and exact view/component type match. Reusing a key for a different type replaces that node.

Do not use a list index as a key when items can be inserted, removed, or reordered. Do not mix keyed and unkeyed siblings, and do not repeat a key; the renderer rejects either case before changing the mounted hierarchy.

Unkeyed children use positional identity. This is appropriate for small, fixed-shape layouts, but inserting an unkeyed item changes the identity of all later positions.

## Property slots and events

Properties whose value is another view, such as `Shortcut.TargetView`, are retained as named property slots. They are assigned through the owning property and are never inserted into `SubViews`.
Slot reconciliation exclusively owns those native properties: it clears a removed or replaced slot before disposing the old view, and ordinary property diffing ignores both the declarative slot specification and its mounted native value.

Event properties use a stable subscription. Re-rendering replaces the callback behind that subscription; removing the property unsubscribes it. This avoids duplicate delivery and repeated add/remove work.

## Collection controls

`ListView` and `DropDownList` rows are not normal child views. Their `m.Items` macros create immutable keyed snapshots in a separate reconciled-property key space. The mounted terminal element owns the mutable `ObservableCollection`/`ListWrapper` adapter, retains its identity, and updates rows by stable domain key.

```fsharp
View.ListView(fun (p: ListViewProps) (m: ListViewMacros) ->
  p.Value(Nullable model.SelectedIndex)
  m.Items(model.Items, (fun item -> item.Id), (fun item -> item.Name)))
```

Synchronization runs before changed native selection properties and scopes suppression around collection-induced events. Removing `m.Items` disposes the adapter. A raw `p.Source` remains supported as an alternative, but declaring both owners on one control is rejected during desired-tree validation.

See [Declarative collection controls](../public/collections.md) for application-facing guidance.

## Scheduling

The first tree is mounted synchronously so Terminal.Gui has a root `Runnable`. Each Elmish loop has its own `TerminalRenderCoordinator`, bounded capacity-one `Channel<RenderRequest>`, and render pump, so pending renders collapse independently within that loop. All coordinators share the application's single `IRenderDispatcher`. After `IApplication.Init`, it posts their commits to the same Terminal.Gui UI thread; each callback drains its coordinator's channel again immediately before reconciliation so it applies the freshest available tree.
