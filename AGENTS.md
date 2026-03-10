# Copilot Coding Agent Instructions for Terminal.Gui.Elmish

## Repository Overview
Terminal.Gui.Elmish is an **F# Elmish wrapper** around Miguel de Icaza's [Terminal.Gui](https://github.com/gui-cs/Terminal.Gui) library. It provides a DSL for building terminal UI applications using the Elm architecture pattern.

**Key technologies:**
- Language: F#
- Target Framework: .NET 10.0 (all projects target `net10.0`)
- Package Manager: Paket (not NuGet directly)
- Testing: NUnit (with Microsoft.Testing.Platform runner)
- Key Dependencies: Elmish 5.0.2, Terminal.Gui 2.0.0-alpha (prerelease)

## Build Instructions

### Prerequisites
- .NET SDK 10.0 or higher (all projects target `net10.0`, as configured in `global.json`)
- Paket tool for dependency management

### Step 1: Restore dotnet tools
**This will install Paket if not already installed.**
```bash
dotnet tool restore
```

### Step 2: Restore Dependencies
```bash
dotnet paket restore
```
This restores packages defined in `paket.dependencies` and `paket.lock`.

### Step 3: Build the Solution
```bash
dotnet build Terminal.Gui.Elmish.sln
```

Or build individual projects:
```bash
# Main library only
dotnet build src/Terminal.Gui.Elmish/Terminal.Gui.Elmish.fsproj

# Tests
dotnet build src/Terminal.Gui.Elmish.Tests/Terminal.Gui.Elmish.Tests.fsproj
```

### Step 4: Run Tests
```bash
dotnet test src/Terminal.Gui.Elmish.Tests/Terminal.Gui.Elmish.Tests.fsproj
```

## Paket Considerations
- To find which package version is used, first check `paket.dependencies` at the repo root.
- Identify the group of the package (if any — e.g. `group test`).
- With the group known, check `paket.lock` for the resolved version.
- Each project has its own `paket.references` listing the packages it uses.

## Project Structure

```
Terminal.Gui.Elmish/
├── src/
│   ├── Terminal.Gui.Elmish/          # Main library
│   │   ├── Task.fs                   # Async task helpers
│   │   ├── Types.fs                  # Core types (ITerminalElement, Props, etc.)
│   │   ├── Differ.fs                 # Diffing mechanism for updates
│   │   ├── ElmishTerminal.fs         # Program runner (mkProgram, runTerminal)
│   │   ├── Types.gen.fs              # Generated types
│   │   ├── PKey.gen.fs               # Generated property keys
│   │   ├── Services/
│   │   │   └── PositionService.fs    # Position/layout service
│   │   ├── TerminalElement.Base.fs   # Base terminal element implementation
│   │   ├── TerminalElement.Interfaces.fs # Element interfaces
│   │   ├── TerminalElement.Elements.gen.fs # Generated element definitions
│   │   ├── Props.gen.fs              # Generated property builders
│   │   ├── Macros.fs                 # Macros API helpers
│   │   ├── View.gen.fs               # Generated view builders
│   │   └── Terminal.Gui.Elmish.fsproj
│   ├── Terminal.Gui.Elmish.Tests/    # NUnit tests
│   └── Terminal.Gui.Elmish.Generator/ # Code generator (runs at build time)
├── examples/
│   ├── PropertiesSample/             # Properties API example
│   └── MacrosSample/                 # Macros API example
├── docs/                             # MkDocs documentation
├── global.json                       # .NET SDK version pinning
├── Directory.Build.props             # Shared MSBuild properties
├── paket.dependencies               # Package dependencies
├── paket.lock                       # Locked versions
└── Terminal.Gui.Elmish.sln          # Solution file
```

## Important F# Conventions

1. **File order matters in F#**: Files must be listed in dependency order in `.fsproj` files. A file can only reference types/modules defined in files listed *above* it.
2. **View DSL pattern**: Use `View.Runnable`, `View.Button`, `View.Label`, etc.
3. **Two API styles**: Properties syntax and Macros syntax (see tests and examples for usage).

## Code Generation

The main library uses a **build-time code generation** step. An MSBuild target (`BeforeTargets="CoreCompile"`) in `Terminal.Gui.Elmish.fsproj` runs the generator project:

```bash
dotnet run --project src/Terminal.Gui.Elmish.Generator/Terminal.Gui.Elmish.Generator.fsproj
```

This produces the `*.gen.fs` files (`Types.gen.fs`, `PKey.gen.fs`, `TerminalElement.Elements.gen.fs`, `Props.gen.fs`, `View.gen.fs`). These generated files **should not be hand-edited**; modify the generator instead. The generation runs automatically as part of `dotnet build`.

After generation, `dotnet fantomas` is invoked to format the generated files.

## Test Runner

Tests use the **Microsoft.Testing.Platform** runner (NUnit's native integration), configured via:
- `EnableNUnitRunner` property in `Terminal.Gui.Elmish.Tests.fsproj`
- `"runner": "Microsoft.Testing.Platform"` in `global.json`

The test project references `Microsoft.NET.Test.Sdk`, `NUnit`, `NUnit.Analyzers`, and `NUnit3TestAdapter` via its `paket.references` (in the `test` Paket group).

## Terminal.Gui Upstream Reference

When working with Terminal.Gui views, properties, or events, **check the upstream Terminal.Gui repository for guidance**:

- **Examples**: <https://github.com/gui-cs/Terminal.Gui/tree/v2_develop/Examples> — contains comprehensive examples demonstrating how to use individual views and features of the library.
- **Tests**: <https://github.com/gui-cs/Terminal.Gui/tree/v2_develop/Tests> — contains unit tests that serve as executable documentation for how views behave, what properties they expose, and how events work.

Use these upstream resources to understand:
- Which properties and events are available on a given `Terminal.Gui.View` subclass.
- How to correctly instantiate and configure views.
- Expected behavior for layout, positioning, color themes, keyboard handling, etc.
- Patterns for testing Terminal.Gui-based code without a running terminal.

## Example Application Pattern
```fsharp
open Terminal.Gui.Elmish

let init () = initialModel, Cmd.none
let update msg model = updatedModel, Cmd.none
let view model dispatch = View.topLevel [ ... ]

ElmishTerminal.mkProgram init update view
|> ElmishTerminal.runTerminal
```

## Troubleshooting

### "paket: not found" Error
Run `dotnet tool restore` before building.

### Target Framework Mismatch
All projects target `net10.0`. Ensure .NET SDK 10.0+ is installed (see `global.json`).

### Build Warnings
The project uses prerelease Terminal.Gui. Warnings about prerelease packages are expected.

## Code Formatting

This repository uses [Fantomas](https://fsprojects.github.io/fantomas/) for F# code formatting. After finishing all edits to `.fs` files in a task, run:

```bash
dotnet fantomas .
```

This must be the **last step** before considering the task complete, to ensure all F# files conform to the project's formatting rules.
