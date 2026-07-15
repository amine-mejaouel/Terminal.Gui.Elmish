# Dynamic keyed-list sample

This sample exercises the retained virtual-tree reconciler with visible operations:

- **Add** inserts a new keyed child.
- **Remove** deletes only the selected child.
- **Update** changes a property while retaining the same native view.
- **Move up/down** and **Reverse** reorder existing keyed native views.
- Selecting a row changes its text while preserving its stable identity.

Run it from the repository root:

```bash
dotnet run --project examples/DynamicListSample/DynamicListSample.fsproj
```

Each row uses `p.Key $"work-item-{item.Id}"`. The key is stable across list-index changes, so reordering does not recreate the corresponding `Terminal.Gui.Button`.

The regular props DSL already expresses keys, dynamic children, layout, and event dispatch directly, so this sample does not require a new macro.
