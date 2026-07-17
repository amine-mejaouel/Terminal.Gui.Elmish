# Testing and shipping

Most application behavior can be tested without starting a terminal because Elmish keeps decisions in pure functions.

## Test update logic first

Given a model and message, assert the next model:

```fsharp
[<Test>]
let ``submitting a draft creates a todo`` () =
  let before =
    { initialModel with
        Draft = "Write documentation" }

  let after = update SubmitDraft before

  Assert.That(after.Todos |> List.exists (fun todo -> todo.Title = "Write documentation"))
```

Test domain identity and normalization rules explicitly:

- selected IDs after deletion or filtering;
- validation and enabled-state predicates;
- command transitions such as loading/success/failure;
- ordering and stable IDs after inserts or moves.

## Keep view helpers deterministic

Extract calculations such as `visibleItems`, `selectedIndex`, labels, and progress fractions into normal functions and test them directly. View builders should remain a thin mapping from those results to props.

The library does not currently expose its internal virtual-tree test renderer as a supported application-testing API. Prefer domain tests plus focused manual UI checks over tests coupled to internal terminal elements.

## Manual smoke checklist

Before a release, exercise the interface at more than one terminal size and verify:

- `Tab` and reverse focus traversal reach every action;
- hot keys and application shortcuts work;
- arrow-key and mouse selection survive unrelated renders;
- forms retain text and focus after validation updates;
- empty, loading, failure, and populated states fit the layout;
- resizing does not clip fixed-width content unexpectedly;
- quitting restores the terminal normally;
- the active theme keeps text and focus indicators readable.

## Build and publish

Build the application normally:

```bash
dotnet build MyTui/MyTui.fsproj --configuration Release
```

Publish a framework-dependent build:

```bash
dotnet publish MyTui/MyTui.fsproj --configuration Release --output publish
```

Choose runtime identifiers, self-contained deployment, trimming, or single-file options according to the target environment and verify them with Terminal.Gui and every library your application uses. Do not assume a packaging option is safe without running the published artifact in the target terminal.

## Lifecycle and failure handling

`ElmishTerminal.runTerminal` owns Terminal.Gui initialization and disposal. Let exceptions leave the program normally so the host can restore terminal state in its cleanup path. Avoid process termination APIs that bypass `finally` blocks.

Commands should convert expected failures into messages and model states. Unexpected exceptions can still fail the process; log them outside the drawing surface or after the terminal application has stopped.

## Current-source deployment

These docs target the development source. A project reference is appropriate while developing against the checkout, but a distributable application should eventually consume a newly versioned package matching this API or pin the exact source revision used to build it.

Do not claim compatibility with the older published package merely because its package ID is the same; its target framework and Terminal.Gui dependency differ from this branch.
