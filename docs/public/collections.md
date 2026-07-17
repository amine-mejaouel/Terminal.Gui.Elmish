# Declarative collection controls

`ListView` and `DropDownList` can receive immutable Elmish data through the `Items` macro. The application describes the current items on every render; Terminal.Gui.Elmish retains and updates the mutable native data source for the lifetime of the mounted control.

If selection is application state, first read [State and events](state-and-events.md#selection-should-use-domain-identity). If rows are separate native child views rather than data-source entries, use [keyed reconciliation](reconciliation.md#keyed-children-use-domain-identity) instead.

## ListView

For domain values, provide a stable key and a text projection:

```fsharp
type Todo =
  { Id: int
    Title: string
    IsCompleted: bool }

let todoText todo =
  let marker = if todo.IsCompleted then "✓" else "○"
  $" {marker}  {todo.Title}"

View.ListView(fun (p: ListViewProps) (m: ListViewMacros) ->
  p.Value(selectedIndex |> Option.toNullable)
  m.Items(model.Todos, (fun todo -> todo.Id), todoText)

  p.ValueChanged(fun args ->
    if args.NewValue.HasValue then
      model.Todos[args.NewValue.Value].Id |> SelectTodo |> dispatch))
```

The key represents domain identity. It must be unique within that control and stable when an item moves or its displayed text changes. Do not use the list index as the key for a collection that can be inserted, removed, filtered, or reordered.

For an already unique sequence of strings, use the shorter overload:

```fsharp
View.ListView(fun (p: ListViewProps) (m: ListViewMacros) ->
  p.Title "Environments"
  m.Items [ "Development"; "Staging"; "Production" ])
```

In this overload, each string is both the display text and stable key, so duplicate strings are rejected. Use the keyed overload when duplicate labels are valid domain data.

## DropDownList

`DropDownList` uses the same adapter and overloads:

```fsharp
View.DropDownList(fun (p: DropDownListProps) (m: DropDownListMacros) ->
  p.Value model.SelectedEnvironment
  m.Items [ "Development"; "Staging"; "Production" ]
  p.ValueChanged(fun args -> dispatch (EnvironmentChanged args.NewValue)))
```

## Why Items is a macro

Terminal.Gui collection controls require an `IListDataSource`, commonly a `ListWrapper` around an `ObservableCollection`. Creating that source in an Elmish `view` function gives the control a new native source on every render. Terminal.Gui can consequently reset selection and other control-local state, which breaks keyboard and mouse navigation across rerenders.

`m.Items` separates the two lifetimes:

- the Elmish model and view remain immutable descriptions;
- the mounted terminal element owns one retained native source;
- keyed changes update that source with insert, move, replace, and remove operations;
- collection-induced selection events are suppressed while synchronization is in progress;
- real user navigation still raises `ValueChanged` normally;
- removing or disposing the control releases the retained source.

This is consistent with MVU: mutable widget state is an implementation detail of the renderer, just like the retained native `View`, event subscriptions, focus, and scroll position. It does not belong in the application model or at module scope.

## Controlled and uncontrolled selection

Use `p.Value` or `p.SelectedItem` when selection is part of the Elmish model. Items are synchronized before native selection properties are applied, so an initial nonzero selection and selection after filtering work correctly.

If neither selection property is declared, the adapter preserves the selected item's key where possible while items move or change.

## Raw Source compatibility

The existing `p.Source` API remains available when an application genuinely needs a custom `IListDataSource`:

```fsharp
View.ListView(fun (p: ListViewProps) ->
  p.Source customSource)
```

Do not declare `p.Source` and `m.Items` on the same control. They are alternative owners of the native source, and Terminal.Gui.Elmish rejects the combination before mutating the mounted tree.

## Choosing between keyed children and Items

Use keyed `p.Children` when each domain item is a separate Terminal.Gui view with its own identity and layout. Use `m.Items` when a collection control owns rows through an `IListDataSource`. In both cases the stable key comes from domain identity, but the retained resource differs: keyed children retain native views, while `m.Items` retains one data-source adapter and reconciles its rows.

For a complete filtering, controlled-selection, and keyboard-navigation implementation, see [TodoApp](examples.md#todoapp).
