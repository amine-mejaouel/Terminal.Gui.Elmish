# Terminal.Gui.Elmish

Terminal.Gui.Elmish is an F# Elmish wrapper around [Terminal.Gui](https://github.com/gui-cs/Terminal.Gui) with a typed, Feliz-style view DSL and retained native-view reconciliation.

```fsharp
View.Button(fun p ->
  p.Text $"Count: {model.Count}"
  p.Accepting(fun _ -> dispatch (Increment |> TerminalMsg.ofMsg)))
```

Application code describes the desired interface from immutable model state. Compatible native Terminal.Gui controls are updated in place so focus, selection, scrolling, event subscriptions, and component-local state can survive Elmish renders.

## Documentation

Start with the [public application-author handbook](docs/public/index.md):

- [Build your first application](docs/public/getting-started.md)
- [Elmish programs and lifecycle](docs/public/programs.md)
- [Views, layout, and styling](docs/public/views-and-layout.md)
- [State and events](docs/public/state-and-events.md)
- [Reconciliation and keys](docs/public/reconciliation.md)
- [Declarative collection controls](docs/public/collections.md)
- [Elmish components](docs/public/components.md)
- [Troubleshooting](docs/public/troubleshooting.md)

Internal renderer and reconciliation notes live under [`docs/internal`](docs/internal/).

## Version status

The current repository source targets **.NET 10** and **Terminal.Gui 2.4.17**.

> The latest `Terminal.Gui.Elmish` package currently published on NuGet targets an older .NET/Terminal.Gui development stack and does not contain every API documented for this branch. Use a project reference to the current source until a newly versioned matching package is published.

## Build the current source

Prerequisites: .NET SDK 10 or newer.

```bash
dotnet tool restore
dotnet paket restore
dotnet build Terminal.Gui.Elmish.sln
dotnet test --project src/Terminal.Gui.Elmish.Tests/Terminal.Gui.Elmish.Tests.fsproj --no-build
```

See the [quickstart](docs/public/getting-started.md) for creating an application against this checkout.

## Examples

- [`TodoApp`](examples/TodoApp/) — polished forms, filtering, controlled text, retained list navigation, shortcuts, and theme-aware styling.
- [`DynamicListSample`](examples/DynamicListSample/) — keyed child insertion, removal, updates, and reordering.
- [`MacrosSample`](examples/MacrosSample/) — menu/collection macros and nested Elmish components.
- [`PropertiesSample`](examples/PropertiesSample/) — direct generated-property syntax.

Run an example from the repository root:

```bash
dotnet run --project examples/TodoApp/TodoApp.fsproj
```

## Documentation development

The public site uses MkDocs Material:

```bash
pip install 'mkdocs-material==9.*'
cd docs
mkdocs serve
```

Public pages live in `docs/public`; internal design notes are not included in the generated site.

## License

Terminal.Gui.Elmish is distributed under the MIT License.
