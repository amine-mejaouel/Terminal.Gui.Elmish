# Todo app

A polished, keyboard-first Todo application built with the properties and macros DSL. It demonstrates controlled text input, immutable Elmish updates, retained native list navigation, filtering, stable todo IDs, and theme-aware Terminal.Gui styling.

Run it from the repository root:

```bash
dotnet run --project examples/TodoApp/TodoApp.fsproj
```

## Controls

| Action | Keyboard | Mouse |
| --- | --- | --- |
| Add a task | Type in the composer and press `Enter` | Select **Add** |
| Navigate | Arrow keys | Select a row |
| Complete or reopen | `Enter` or `Space` on a selected task | Double-click a row |
| Start a fresh task | `F4` | Select the composer |
| Edit the selected task | `F2`, then `Enter` to save | Select **Edit** in the status bar |
| Cancel editing | `Esc` | Select **Cancel** in the status bar |
| Delete the selected task | `F8` globally, or `Delete` from the task list | Select **Delete** in the status bar |
| Filter | `Alt+A` All, `Alt+V` Active, `Alt+O` Done | Select a filter |
| Quit | `F12` | Select **Quit** in the status bar |

The app deliberately keeps its data in memory and starts with a few seeded tasks. Its visual treatment uses the built-in `Base`, `Accent`, and `Menu` schemes, so it follows the active Terminal.Gui theme instead of forcing a custom palette.

## Declarative task list

The task list passes immutable model values directly to the collection macro:

```fsharp
View.ListView(fun (p: ListViewProps) (m: ListViewMacros) ->
  p.Value(selectedIndex |> Option.toNullable)
  m.Items(visibleTodos, (fun todo -> todo.Id), todoLabel)

  p.ValueChanged(fun args ->
    if args.NewValue.HasValue then
      visibleTodos[args.NewValue.Value].Id |> Select |> dispatch))
```

`Todo.Id` is the row identity, while `todoLabel` can change whenever a task is completed or edited. The view does not create or cache an `ObservableCollection`, `ListWrapper`, or `IListDataSource`. Terminal.Gui.Elmish retains that mutable adapter with the mounted `ListView`, updates it by key, and suppresses only the synthetic selection events caused by synchronization. Arrow keys and mouse selection continue to dispatch normal `ValueChanged` messages.

See the [declarative collection controls guide](../../docs/public/collections.md) for the API, ownership rules, raw `p.Source` compatibility, and `DropDownList` usage.
