# Todo app

A polished, keyboard-first Todo application built with the properties DSL. It demonstrates controlled text input, immutable Elmish updates, native list navigation, filtering, stable todo IDs, and theme-aware Terminal.Gui styling.

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
