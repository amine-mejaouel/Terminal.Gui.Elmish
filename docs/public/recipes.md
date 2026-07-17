# Practical application patterns

These patterns are small enough to copy but preserve the ownership and identity rules described elsewhere in the handbook.

The snippets use these common imports:

```fsharp
open Terminal.Gui.Elmish
open Terminal.Gui.Input
open Terminal.Gui.ViewBase
```

## Split a large view into functions

View helpers can return `IView` and receive only the model slice and dispatch function they need:

```fsharp
let private send dispatch msg =
  dispatch (TerminalMsg.ofMsg msg)

let toolbar model dispatch : IView =
  View.FrameView(fun p ->
    p.Height 3
    p.Width(Dim.Fill())
    p.Children
      [ View.Button(fun p ->
          p.Text "_Save"
          p.Enabled model.CanSave
          p.Accepting(fun _ -> send dispatch Save))
        :> IView ])
  :> IView

let view model dispatch =
  View.Runnable [ toolbar model dispatch ] :> IView
```

This does not create a component or another Elmish loop. Use ordinary functions for organization and [components](components.md) for independent local state.

## Derive view data instead of storing it twice

```fsharp
let visibleTodos model =
  model.Todos
  |> List.filter (fun todo ->
    match model.Filter with
    | All -> true
    | Active -> not todo.IsCompleted
    | Completed -> todo.IsCompleted)
```

Calculate filtered rows, counts, enabled states, and selection indexes from the model. Storing both source data and derived UI data creates synchronization problems.

## Render an empty state without losing the surrounding layout

Keep the outer region stable and change its children:

```fsharp
View.FrameView(fun p ->
  p.Key "results"
  p.Title "Results"
  p.Width(Dim.Fill())
  p.Height(Dim.Fill())

  p.Children
    [ if model.Results.IsEmpty then
        View.Label(fun p ->
          p.Text "No results"
          p.X TPos.Center
          p.Y TPos.Center)
        :> IView
      else
        resultsList model dispatch ])
```

The frame retains its identity while the content changes. If alternative children share a dynamic sibling list, follow the [key rules](reconciliation.md).

## Put global actions in a status bar

```fsharp
open Terminal.Gui.Input

let shortcut key title enabled action =
  View.Shortcut(fun p ->
    p.Key key
    p.Title title
    p.Enabled enabled
    p.BindKeyToApplication true
    p.Action action)
  :> IView

View.StatusBar(fun p ->
  p.Y(TPos.AnchorEnd(Some 1))
  p.Width(Dim.Fill())
  p.Children
    [ shortcut Key.F2 "Edit" model.SelectedId.IsSome (fun () -> send dispatch BeginEdit)
      shortcut Key.F12 "Quit" true (fun () -> dispatch TerminalMsg.Terminate) ])
```

Keep contextual keys on the focused control and global actions in application-bound shortcuts.

## Represent loading as model state

```fsharp
type LoadState<'a> =
  | NotStarted
  | Loading
  | Loaded of 'a
  | Failed of string
```

An Elmish command moves the model from `Loading` to `Loaded` or `Failed`. The view renders the corresponding spinner, content, or error message. The command dispatches results; it never reaches into a native label or list.

See [full programs](programs.md#a-full-program-with-a-command) for the command shape.

## Preserve selection while filtering

Store a selected domain ID, derive the current visible index, and normalize the ID in `update` when the selected item becomes hidden or is deleted. Pass immutable visible rows through `m.Items`.

This keeps business identity out of Terminal.Gui indexes and makes filtering deterministic. The complete implementation is in TodoApp's `Domain.fs` and `Program.fs`; see the [examples tour](examples.md#todoapp).

## Reset a native editing session deliberately

Most fields should keep one stable key. If a new editing session must start with a fresh caret and native state, include a model revision in that field's key:

```fsharp
p.Key $"editor-{model.EditorRevision}"
```

Increment the revision only when beginning or cancelling a session. This intentionally replaces the native field; it should not happen for every keystroke.
