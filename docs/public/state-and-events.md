# State, events, focus, and interaction

An Elmish application works best when application state is explicit in the model and native control state is either controlled from that model or deliberately left local to the retained control.

## Controlled text input

```fsharp
type Model = { Draft: string }
type Msg = DraftChanged of string | Submit

View.TextField(fun p ->
  p.Text model.Draft
  p.ValueChanged(fun args ->
    dispatch (DraftChanged args.NewValue |> TerminalMsg.ofMsg))
  p.Accepting(fun _ ->
    dispatch (Submit |> TerminalMsg.ofMsg)))
```

The current model supplies the text, and `ValueChanged` feeds user edits back into `update`. This is a controlled field.

Use the event argument types exposed by IntelliSense. Terminal.Gui v2 commonly reports transitions through `ValueChangedEventArgs<'T>` with `OldValue` and `NewValue`.

## Controlled versus retained native state

Control a value when application behavior, persistence, validation, or another view depends on it. Examples include form text, selected domain IDs, filter choices, and checked settings.

Leave purely presentational widget state undeclared when Terminal.Gui can own it safely. Scroll offsets, caret details, hover state, and an uncontrolled list selection can remain on the retained native view.

Omitting a property means "use the native/default value"; it does not copy that value into the model.

## Event callbacks stay current

Builder callbacks normally close over the current model:

```fsharp
p.Accepting(fun _ ->
  dispatch (Delete model.SelectedId |> TerminalMsg.ofMsg))
```

It is safe for this to be a fresh F# closure on every view call. A retained control keeps one native subscription while Terminal.Gui.Elmish replaces the Elmish callback behind it. Removing the event property removes the native subscription.

Do not cache handlers merely to preserve delegate identity.

## Keyboard handling

```fsharp
open Terminal.Gui.Input

p.KeyDown(fun key ->
  if key = Key.Esc then
    dispatch (Cancel |> TerminalMsg.ofMsg)
  elif key = Key.Delete then
    dispatch (DeleteSelected |> TerminalMsg.ofMsg))
```

Use control events for contextual keys and `View.Shortcut` for application-level actions:

```fsharp
View.Shortcut(fun p ->
  p.Key Key.F12
  p.Title "Quit"
  p.BindKeyToApplication true
  p.Action(fun () -> dispatch TerminalMsg.Terminate))
```

`Accepting` is the normal activation event for buttons and many views. Menu items expose their native `Action` behavior. Consult the props type and upstream Terminal.Gui semantics rather than assuming every control raises the same activation event.

## Focus

Declare focus only when the application needs to direct it:

```fsharp
View.TextField(fun p ->
  p.Text model.Draft

  if model.FocusEditor then
    p.HasFocus true)
```

For focus that must happen after a newly mounted control is initialized, use `Initialized` to dispatch a readiness message, then declare `HasFocus` from the resulting model. Keep that handshake finite so initialization does not produce an update loop.

Changing a view's `p.Key` deliberately replaces the native control. This can be useful when an editing session genuinely needs a fresh native field, but it also discards caret, focus, and other local state. Do not change keys as routine focus management.

## Validation and disabled actions

Derive validation and action availability from the model:

```fsharp
let canSubmit = not (System.String.IsNullOrWhiteSpace model.Draft)

View.Button(fun p ->
  p.Text "_Save"
  p.Enabled canSubmit
  p.IsDefault true
  p.Accepting(fun _ ->
    if canSubmit then
      dispatch (Submit |> TerminalMsg.ofMsg)))
```

Keep the guard in `update` as well when invalid messages could arrive through another shortcut, command, or test.

## Selection should use domain identity

Native list selection is an index, but application selection should usually be a stable domain ID:

```fsharp
let selectedIndex =
  model.SelectedId
  |> Option.bind (fun id -> visibleItems |> List.tryFindIndex (fun item -> item.Id = id))

View.ListView(fun (p: ListViewProps) (m: ListViewMacros) ->
  p.Value(selectedIndex |> Option.toNullable)
  m.Items(visibleItems, (fun item -> item.Id), (fun item -> item.Name))

  p.ValueChanged(fun args ->
    if args.NewValue.HasValue then
      visibleItems
      |> List.tryItem args.NewValue.Value
      |> Option.iter (fun item ->
        dispatch (Select item.Id |> TerminalMsg.ofMsg))))
```

The index is recalculated for the current filter and order; the model does not confuse row position with item identity. See [declarative collections](collections.md) for ownership and synchronization details.

## Side effects belong outside `view`

Do not write files, start tasks, modify global themes, or mutate controls while constructing the desired tree. Dispatch a message and perform external work through `update`, a command, or a subscription. Configuration that must happen once can run before `runTerminal`.

Next: [reconciliation and keys](reconciliation.md).
