# Build your first application

This guide creates a small counter with keyboard-accessible buttons and a clean quit action. It uses `mkSimple`, the best starting point when updates are synchronous and do not yet need Elmish commands.

## Prerequisites

- .NET SDK 10 or newer
- Git
- a terminal supported by Terminal.Gui

The current public NuGet package predates this source API. For now, build against a checkout of the repository.

## Prepare the source and application

From a working directory:

```bash
git clone https://github.com/Hardt-Coded/Terminal.Gui.Elmish.git
cd Terminal.Gui.Elmish
dotnet tool restore
dotnet paket restore
dotnet build src/Terminal.Gui.Elmish/Terminal.Gui.Elmish.fsproj

dotnet new console --language F# --framework net10.0 --output MyTui
dotnet add MyTui/MyTui.fsproj reference src/Terminal.Gui.Elmish/Terminal.Gui.Elmish.fsproj
```

The project reference deliberately uses the current source. When a matching package is published, it can be replaced by a normal package reference.

## Write the application

Replace `MyTui/Program.fs` with:

```fsharp
open Terminal.Gui.Elmish

type Model = { Count: int }

type Msg =
  | Increment
  | Decrement

let init () = { Count = 0 }

let update msg model =
  match msg with
  | Increment -> { model with Count = model.Count + 1 }
  | Decrement -> { model with Count = model.Count - 1 }

let private send dispatch msg =
  dispatch (TerminalMsg.ofMsg msg)

let view model dispatch =
  View.Runnable(fun (p: RunnableProps) ->
    p.Children
      [ View.Label(fun p ->
          p.Text $"Count: {model.Count}"
          p.X(TPos.Absolute 2)
          p.Y(TPos.Absolute 1))
        :> IView

        View.Button(fun p ->
          p.Text "_Increment"
          p.X(TPos.Absolute 2)
          p.Y(TPos.Absolute 3)
          p.Accepting(fun _ -> send dispatch Increment))
        :> IView

        View.Button(fun p ->
          p.Text "_Decrement"
          p.X(TPos.Absolute 18)
          p.Y(TPos.Absolute 3)
          p.Accepting(fun _ -> send dispatch Decrement))
        :> IView

        View.Button(fun p ->
          p.Text "_Quit"
          p.X(TPos.Absolute 2)
          p.Y(TPos.Absolute 6)
          p.Accepting(fun _ -> dispatch TerminalMsg.Terminate))
        :> IView ])
  :> IView

[<EntryPoint>]
let main _ =
  ElmishTerminal.mkSimple init update view
  |> ElmishTerminal.runTerminal

  0
```

Run it:

```bash
dotnet run --project MyTui/MyTui.fsproj
```

Select buttons with `Tab`, activate them with `Enter` or `Space`, or use the underlined access keys (`Alt+I`, `Alt+D`, and `Alt+Q`, depending on terminal behavior).

## Follow one click

When **Increment** is accepted:

1. the event handler wraps `Increment` with `TerminalMsg.ofMsg` and dispatches it;
2. `update` returns a new model with a larger count;
3. Elmish calls `view` with that model;
4. `view` describes a label containing the new text;
5. Terminal.Gui.Elmish patches the existing native label instead of replacing the complete application.

The `view` values are descriptions, not native controls. Recreate them freely from the model on every call.

## The three signatures to recognize

```fsharp
init   : unit -> Model
update : Msg -> Model -> Model
view   : Model -> (TerminalMsg<Msg> -> unit) -> IView
```

`TerminalMsg` adds the library-level `Terminate` message around your application messages. Use `TerminalMsg.ofMsg` for normal messages and dispatch `TerminalMsg.Terminate` to stop the application.

## Where to go next

- Add asynchronous work with [full Elmish programs](programs.md).
- Replace absolute coordinates with [responsive layout](views-and-layout.md).
- Add text input using [controlled state and events](state-and-events.md).
- Study the complete [Todo application](examples.md#todoapp).
