# Copilot Coding Agent Instructions for Terminal.Gui.Elmish

## Repository Overview
Terminal.Gui.Elmish is an **F# Elmish wrapper** around Miguel de Icaza's [Terminal.Gui](https://github.com/gui-cs/Terminal.Gui) library. It provides a DSL for building terminal UI applications using the Elm architecture pattern.

**Key technologies:**
- Language: F#
- Target Framework: .NET 10.0 (all projects target `net10.0`)
- Package Manager: Paket (not NuGet directly)
- Testing: NUnit (with Microsoft.Testing.Platform runner)
- Key Dependencies: Elmish 5.0.2, Terminal.Gui 2.0.0-alpha.4170 (prerelease)

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

## Paket considerations
- To find which package version is used, first check `paket.dependencies` of the given project.
- Identify the group of the package (if any).
- With the group known, check `paket.lock` for the resolved version.

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
│   ├── Terminal.Gui.Elmish.Base/     # Base abstractions (build output only)
│   ├── Terminal.Gui.Elmish.Gen/      # Generated code output (build output only)
│   ├── Terminal.Gui.Elmish.Tests/    # NUnit tests
│   └── generator/
│       └── Terminal.Gui.Elmish.Generator/  # Code generator (runs at build time)
├── examples/
│   ├── Elmish.Console.Elements/      # Comprehensive example
│   ├── PropertiesSample/             # Properties API example
│   └── MacrosSample/                 # Macros API example
├── docs/                             # MkDocs documentation
├── .paket/                           # Paket configuration
├── global.json                       # .NET SDK version pinning
├── Directory.Build.props             # Shared MSBuild properties
├── paket.dependencies               # Package dependencies
├── paket.lock                       # Locked versions
└── Terminal.Gui.Elmish.sln          # Solution file
```

## Key Configuration Files

| File | Purpose |
|------|---------|
| `global.json` | .NET SDK version pinning and test runner configuration |
| `Directory.Build.props` | Shared MSBuild properties across all projects |
| `paket.dependencies` | Root package dependencies |
| `paket.lock` | Locked package versions |
| `*/paket.references` | Per-project package references |
| `.paket/Paket.Restore.targets` | MSBuild integration for Paket |
| `Terminal.Gui.Elmish.sln` | Visual Studio solution |

## Important F# Conventions

1. **File order matters in F#**: Files must be listed in dependency order in `.fsproj` files
2. **View DSL pattern**: Use `View.Runnable`, `View.Button`, `View.Label` etc.
3. **Two API styles**: Properties syntax and Macros syntax (see tests for examples)

## Code Generation

The main library uses a **build-time code generation** step. An MSBuild target (`BeforeTargets="CoreCompile"`) in `Terminal.Gui.Elmish.fsproj` runs the generator project:

```bash
dotnet run --project src/generator/Terminal.Gui.Elmish.Generator/Terminal.Gui.Elmish.Generator.fsproj
```

This produces the `*.gen.fs` files (`Types.gen.fs`, `PKey.gen.fs`, `TerminalElement.Elements.gen.fs`, `Props.gen.fs`, `View.gen.fs`). These generated files **should not be hand-edited**; modify the generator instead. The generation runs automatically as part of `dotnet build`.

## Test Runner

Tests use the **Microsoft.Testing.Platform** runner (NUnit's native integration), configured via:
- `EnableNUnitRunner` property in `Terminal.Gui.Elmish.Tests.fsproj`
- `"runner": "Microsoft.Testing.Platform"` in `global.json`

The test project references `Microsoft.Testing.Platform`, `NUnit`, and `NUnit3TestAdapter` via its `paket.references` (in the `test` Paket group).

## Example Application Pattern
```fsharp
open Terminal.Gui.Elmish

let init () = initialModel, Cmd.none
let update msg model = updatedModel, Cmd.none
let view model dispatch = View.topLevel [ ... ]

ElmishTerminal.mkProgram init update view
|> ElmishTerminal.runTerminal
```

## CI/CD
- Travis CI (`.travis.yml`) - Linux builds (configured with .NET 6.0.300, but current project requires .NET 10.0+)
- AppVeyor (`appveyor.yml`) - Windows builds (configured for Visual Studio 2017, also outdated)

**Note:** Both CI configurations are outdated. The current codebase requires .NET 10.0+ to build properly.

## Documentation
Documentation uses MkDocs with Material theme:
```bash
pip install mkdocs-material=="9.*"
cd docs && mkdocs serve
```

## Troubleshooting

### "paket: not found" Error
Run `dotnet tool restore` before building.

### Target Framework Mismatch
All projects target `net10.0`. Ensure .NET SDK 10.0+ is installed (see `global.json`).

### Build Warnings
The project uses prerelease Terminal.Gui. Warnings about prerelease packages are expected.

## Trust These Instructions
These instructions are validated. Only search the repository if information is incomplete or found to be in error.
