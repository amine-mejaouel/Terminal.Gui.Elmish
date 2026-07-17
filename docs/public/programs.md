# Elmish programs and application lifecycle

Terminal.Gui.Elmish supports the two standard Elmish shapes: a simple program for synchronous state transitions and a full program for commands.

## Choose the program shape

| Use | Initialization | Update result |
| --- | --- | --- |
| `ElmishTerminal.mkSimple` | `arg -> Model` | `Model` |
| `ElmishTerminal.mkProgram` | `arg -> Model * Cmd<TerminalMsg<Msg>>` | `Model * Cmd<TerminalMsg<Msg>>` |

Start with `mkSimple`. Move to `mkProgram` when an update must schedule file, network, timer, process, or other asynchronous work.

Both program shapes expect the view to dispatch `TerminalMsg<Msg>`:

```fsharp
let send dispatch msg =
  dispatch (TerminalMsg.ofMsg msg)
```

`TerminalMsg.Msg` carries an application message. `TerminalMsg.Terminate` belongs to the host and stops the Terminal.Gui application.

## A full program with a command

```fsharp
open Elmish
open Terminal.Gui.Elmish

type Model =
  { IsSaving: bool
    Status: string }

type Msg =
  | Save
  | Saved

let saveCommand : Cmd<TerminalMsg<Msg>> =
  Cmd.ofEffect(fun dispatch ->
    async {
      do! Async.Sleep 500
      dispatch (TerminalMsg.ofMsg Saved)
    }
    |> Async.StartImmediate)

let init () =
  { IsSaving = false
    Status = "Ready" },
  Cmd.none

let update msg model =
  match msg with
  | Save ->
    { model with
        IsSaving = true
        Status = "Saving…" },
    saveCommand
  | Saved ->
    { model with
        IsSaving = false
        Status = "Saved" },
    Cmd.none
```

Commands dispatch wrapped messages because the outer Elmish loop handles both your messages and `Terminate`. Commands should perform application work and dispatch results; they should not mutate Terminal.Gui controls directly.

## Subscriptions

Attach ordinary Elmish subscriptions before running the program:

```fsharp
ElmishTerminal.mkProgram init update view
|> ElmishTerminal.withSubscription subscriptions
|> ElmishTerminal.runTerminal
```

`subscriptions` has the application-level shape `Model -> Sub<Msg>`. Terminal.Gui.Elmish maps emitted messages through `TerminalMsg.ofMsg`, so subscription implementations remain expressed in terms of your `Msg` type.

Give subscriptions stable Elmish subscription IDs and release their resources through the normal subscription disposal contract. Model-dependent subscriptions may be restarted when their identity changes.

## Stop the application

Dispatch termination from any view callback:

```fsharp
p.Accepting(fun _ -> dispatch TerminalMsg.Terminate)
```

`runTerminal` owns the Terminal.Gui application lifecycle. It initializes the application after the first `Runnable` is available, runs the application loop, requests shutdown on `Terminate`, and disposes the renderer, native views, dispatcher, and application.

Do not also call Terminal.Gui `Application.Init`, `Run`, or `Shutdown` around `runTerminal`.

## Configuration and themes

Terminal.Gui configuration can be enabled before starting Elmish:

```fsharp
open Terminal.Gui.Configuration

[<EntryPoint>]
let main _ =
  ConfigurationManager.Enable(ConfigLocations.All)

  ElmishTerminal.mkSimple init update view
  |> ElmishTerminal.runTerminal

  0
```

This is optional. Use it when the application should load Terminal.Gui configuration and theme settings.

## Keep the view pure

Treat `view` as a pure function of the current model and dispatch function:

- create fresh view specifications freely;
- derive every declared property from the model;
- dispatch from event handlers instead of changing the model in place;
- start external work through commands or subscriptions;
- never depend on every intermediate view being committed.

The renderer keeps only the newest pending render when updates arrive faster than Terminal.Gui can apply them. Application correctness must therefore come from model state, not from side effects performed while constructing a view.

## Root requirement

The application root must be a `View.Runnable`, and its identity must remain stable for the complete session:

```fsharp
let view model dispatch =
  View.Runnable(fun (p: RunnableProps) ->
    p.Children [ content model dispatch ])
  :> IView
```

Change children and properties inside the root. Do not switch the root between `Runnable`, `Dialog`, or another exact native type after startup.

Next: [compose views and layouts](views-and-layout.md).
