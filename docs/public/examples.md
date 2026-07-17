# Examples tour

Run examples from the repository root after restoring tools and dependencies:

```bash
dotnet tool restore
dotnet paket restore
```

## TodoApp

```bash
dotnet run --project examples/TodoApp/TodoApp.fsproj
```

Start here after the quickstart. TodoApp demonstrates:

- a model split from view construction;
- controlled text input and validation;
- immutable filtering and selection by stable todo ID;
- `ListViewMacros.Items` with retained arrow-key navigation;
- deliberate focus handshakes for editing sessions;
- application and contextual keyboard shortcuts;
- `Dim`/`TPos` responsive layout;
- built-in schemes, rounded borders, and restrained styling;
- empty states and derived progress/counts.

Read `Domain.fs` first, then `Program.fs`. Keeping update logic separate makes it clear which behavior belongs to the model and which belongs to the desired UI.

## DynamicListSample

```bash
dotnet run --project examples/DynamicListSample/DynamicListSample.fsproj
```

Use this example to understand the retained virtual tree. It adds, removes, edits, reverses, and moves buttons while each row keeps a stable `p.Key` based on its domain ID.

Compare native child views here with `m.Items` in TodoApp: DynamicListSample retains one native button per item, while TodoApp retains one list control and one data-source adapter.

## MacrosSample

```bash
dotnet run --project examples/MacrosSample/MacrosSample.fsproj
```

This broader playground demonstrates:

- `MenuBar` and `MenuBarItem` macro builders;
- declarative list items;
- view-valued menu properties;
- relative layout between desired views;
- custom `ComponentProps` and nested `mkSimpleComponent` loops.

The focused handbook examples are preferable for first use; MacrosSample is useful when exploring the less common menu and component surfaces.

## PropertiesSample

```bash
dotnet run --project examples/PropertiesSample/PropertiesSample.fsproj
```

This is a small generated-properties playground. It shows direct construction of menus and view-valued properties without relying on convenience macros.

## Suggested order

1. Complete [Build your first application](getting-started.md).
2. Run TodoApp and trace one event through `Domain.update` and `Program.view`.
3. Run DynamicListSample while reading [Reconciliation and keys](reconciliation.md).
4. Use MacrosSample when building menus or local-state components.
