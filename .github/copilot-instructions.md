# Copilot Coding Agent Instructions for Terminal.Gui.Elmish

## Repository Overview
Terminal.Gui.Elmish is an **F# Elmish wrapper** around Miguel de Icaza's [Terminal.Gui](https://github.com/migueldeicaza/gui.cs) library. It provides a Feliz-style DSL for building terminal UI applications using the Elm architecture pattern.

**Key technologies:**
- Language: F#
- Target Framework: .NET 8.0+ (main library targets `net8.0`, tests target `net9.0`)
- Package Manager: Paket (not NuGet directly)
- Testing: NUnit
- Key Dependencies: Elmish 5.0.2, Terminal.Gui 2.0.0-develop (prerelease)

## Build Instructions

### Prerequisites
- .NET SDK 8.0 or higher (the project uses `net8.0` and `net9.0` target frameworks)
- Paket tool for dependency management

### Step 1: Install Paket
**Always install Paket first before restoring or building:**
```bash
dotnet tool install paket --tool-path .paket
```

### Step 2: Restore Dependencies
```bash
.paket/paket restore
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

## Project Structure

```
Terminal.Gui.Elmish/
├── src/
│   ├── Terminal.Gui.Elmish/          # Main library
│   │   ├── Types.fs                  # Core types (ITerminalElement, Props, etc.)
│   │   ├── Interop.fs                # Terminal.Gui interop
│   │   ├── Elements.fs               # Element definitions
│   │   ├── Props.fs                  # Property builders
│   │   ├── Views.fs                  # View builders (View.topLevel, View.button, etc.)
│   │   ├── TreeDiff.fs               # Diffing mechanism for updates
│   │   ├── ElmishTerminal.fs         # Program runner (mkProgram, runTerminal)
│   │   └── Terminal.Gui.Elmish.fsproj
│   ├── Terminal.Gui.Elmish.Tests/    # NUnit tests
│   └── generator/                    # Code generator (optional)
├── examples/
│   ├── Elmish.Console.Elements/      # Comprehensive example (net8.0)
│   ├── PropertiesSample/             # Properties API example (net9.0)
│   └── MacrosSample/                 # Macros API example (net9.0)
├── docs/                             # MkDocs documentation
├── .paket/                           # Paket configuration
├── paket.dependencies               # Package dependencies
├── paket.lock                       # Locked versions
└── Terminal.Gui.Elmish.sln          # Solution file
```

## Key Configuration Files

| File | Purpose |
|------|---------|
| `paket.dependencies` | Root package dependencies |
| `paket.lock` | Locked package versions |
| `*/paket.references` | Per-project package references |
| `.paket/Paket.Restore.targets` | MSBuild integration for Paket |
| `Terminal.Gui.Elmish.sln` | Visual Studio solution |

## Important F# Conventions

1. **File order matters in F#**: Files must be listed in dependency order in `.fsproj` files
2. **View DSL pattern**: Use `View.topLevel`, `View.button`, `View.label` etc.
3. **Two API styles**: Properties syntax and Macros syntax (see tests for examples)

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
- Travis CI (`.travis.yml`) - Linux builds with .NET 6.0.300
- AppVeyor (`appveyor.yml`) - Windows builds

## Documentation
Documentation uses MkDocs with Material theme:
```bash
pip install mkdocs-material=="9.*"
cd docs && mkdocs serve
```

## Troubleshooting

### "paket: not found" Error
Run `dotnet tool install paket --tool-path .paket` before building.

### Target Framework Mismatch
The main library uses `net8.0`. Tests and some examples use `net9.0`. Ensure compatible .NET SDK.

### Build Warnings
The project uses prerelease Terminal.Gui (`2.0.0-develop.4639`). Warnings about prerelease packages are expected.

## Trust These Instructions
These instructions are validated. Only search the repository if information is incomplete or found to be in error.
