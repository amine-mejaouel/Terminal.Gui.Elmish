---
applyTo: "**/*.fs"
---

After finishing all edits to `.fs` files in a task, run Fantomas to format them:

```bash
dotnet fantomas .
```

This must be the **last step** before considering the task complete, to ensure all F# files conform to the project's formatting rules.
