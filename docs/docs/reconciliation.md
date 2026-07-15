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

Event properties use a stable subscription. Re-rendering replaces the callback behind that subscription; removing the property unsubscribes it. This avoids duplicate delivery and repeated add/remove work.

## Scheduling

The first tree is mounted synchronously so Terminal.Gui has a root `Runnable`. Once the application is initialized, later renders are queued through `IApplication.Invoke`. Multiple pending renders collapse to the latest requested tree before reconciliation.
