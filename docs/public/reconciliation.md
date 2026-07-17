# Reconciliation and keys

Terminal.Gui.Elmish creates a fresh desired tree whenever Elmish publishes a model, then reconciles that description with the tree already mounted in Terminal.Gui.

Understanding identity is the key to preserving focus, selection, scroll position, event subscriptions, and component-local state.

## Desired views are ephemeral

```fsharp
View.Label(fun p -> p.Text model.Status)
```

This returns a specification. It is normal to create a new specification on every call to `view`. The renderer decides whether it describes an existing native label or a replacement.

Do not cache desired views or native views in the Elmish model.

## Unkeyed children use position

For a small, fixed layout, unkeyed children are matched by sibling position and exact type:

```fsharp
p.Children
  [ View.Label(fun p -> p.Text "Name") :> IView
    View.TextField(fun p -> p.Text model.Name) :> IView ]
```

This is appropriate while the shape remains fixed. Inserting another unkeyed child at the beginning shifts the identity of every later position.

## Keyed children use domain identity

Dynamic sibling lists should key every item:

```fsharp
p.Children
  [ for item in model.Items do
      View.Button(fun p ->
        p.Key $"item-{item.Id}"
        p.Text item.Name
        p.Accepting(fun _ ->
          dispatch (Select item.Id |> TerminalMsg.ofMsg)))
      :> IView ]
```

A keyed child is retained when its key and exact view or component type match. It can move to another sibling index without losing its native identity.

Keys are local to one parent. They must be:

- unique among those siblings;
- stable for the lifetime of the domain item;
- derived from domain identity, not the current index;
- present on every sibling when that sibling list is keyed.

The renderer rejects duplicate keys and lists that mix keyed and unkeyed children before changing the mounted hierarchy.

## Same key, different type means replacement

```fsharp
if model.IsEditing then
  View.TextField(fun p -> p.Key "primary")
else
  View.Label(fun p -> p.Key "primary")
```

The key matches, but the exact native type does not. The label is disposed and a new text field is mounted. Use this deliberately, not as a way to hide unrelated alternatives behind one identity.

## Changing a key resets native state

Changing `p.Key` requests a replacement even when the type is unchanged. The new control starts with fresh focus, caret, selection, and scroll state. This is useful for explicit session resets but harmful when keys are generated from a list index or random value.

## The root remains stable

Terminal.Gui needs one native `Runnable` for the application session. The root exact type and key cannot change after the first render.

Keep the stable root and vary its children:

```fsharp
View.Runnable(fun (p: RunnableProps) ->
  p.Children
    [ if model.IsAuthenticated then
        authenticatedView model dispatch
      else
        loginView model dispatch ])
```

If the conditional children use the same sibling position but different types, key the alternatives when their identities need to be explicit.

## View-valued properties have their own identity

A property such as `Shortcut.TargetView` owns a named view slot. It is reconciled independently from ordinary children and is never inserted into `SubViews` by child reconciliation.

Do not supply the same desired view simultaneously as a property value and a normal child.

## Events follow the retained control

A compatible render retains the native event subscription and replaces only its current Elmish callback. A replacement disposes the old control and its handlers. This is why stable keys preserve interaction without requiring stable delegate instances.

## Collection rows are not child views

Rows owned by `ListView` and `DropDownList` use a native `IListDataSource`, not `p.Children`. Use `m.Items` to retain one source and reconcile immutable values by key. Use keyed children when each item is a separate view with its own layout and native identity.

See [declarative collections](collections.md) for the complete distinction.

## Identity checklist

When state unexpectedly resets, ask:

1. Did the view move in an unkeyed sibling list?
2. Did its key change or become index-based?
3. Did the exact `View.*` type change?
4. Did a conditional remove the view completely?
5. Did the application replace the root?
6. Is the state actually controlled by a property that the model resets?

For executable reorder examples, see [DynamicListSample](examples.md#dynamiclistsample).
