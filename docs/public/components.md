# Elmish components

An Elmish component is a reusable child with its own model, update function, view function, and render coordinator. Use one when a section has genuinely independent state or update traffic—not merely to shorten a large view function.

## Create a component

Components receive `ComponentProps`, which also carry their identity among siblings:

```fsharp
module CounterPanel

open Terminal.Gui.Elmish

type private Model = { Count: int }
type private Msg = Increment

type private Props(key: string) =
  inherit ComponentProps("CounterPanel")
  do base.Key <- Some key

let create key =
  let props = Props(key)

  let init () = { Count = 0 }

  let update msg model =
    match msg with
    | Increment -> { model with Count = model.Count + 1 }

  let view model dispatch =
    View.Runnable(fun (p: RunnableProps) ->
      p.Children
        [ View.Button(fun p ->
            p.Text $"Local count: {model.Count}"
            p.Accepting(fun _ ->
              dispatch (Increment |> TerminalMsg.ofMsg)))
          :> IView ])
    :> IView

  ElmishTerminal.mkSimpleComponent props init update view
```

Mount it as a normal child:

```fsharp
View.FrameView(fun p ->
  p.Children [ CounterPanel.create "sidebar" ])
```

The component's model survives compatible parent renders and keyed moves. Removing or replacing the component terminates its loop and disposes its rendered subtree.

## Keep the component root stable

The component owns a nested renderer with the same root rule as the application. Return a stable root type—normally `View.Runnable`—and place conditional content beneath it.

Do not switch the component root between a label, button, and frame as local state changes.

## Add typed component properties

`ComponentProps` contains an integer-keyed property bag. Wrap it with a small writer/reader interface so callers get typed configuration:

```fsharp
type private PropKey =
  | Title = 0

type IProps =
  abstract Title: string -> unit

type private IPropsReader =
  abstract Title: string option

type private Props(key: string) =
  inherit ComponentProps("TitledPanel")

  do base.Key <- Some key

  interface IProps with
    member this.Title value = this[int PropKey.Title] <- value

  interface IPropsReader with
    member this.Title = this.TryGetPropValue(int PropKey.Title)
```

The factory creates `Props`, lets the parent configure `IProps`, and captures `IPropsReader` for the component view. When the parent produces a compatible component specification, the retained component's property bag is updated without restarting its loop.

Use the same pattern for data and callbacks from the parent. Keep property IDs unique within the component type.

## Component identity

Component keys follow the same sibling rules as view keys:

- use a stable domain key when components are added, removed, or reordered;
- do not mix keyed and unkeyed component siblings;
- a different component runtime type is a replacement even if the key matches.

## Boundaries and limitations

- A component is a child of the application or another component; it cannot be the top-level application root passed to `runTerminal`.
- Components are not supported inside view-valued property slots.
- Parent and component loops share the same Terminal.Gui application thread for native commits.
- Component messages still use `TerminalMsg.ofMsg` internally; component termination is managed when its owner removes it.

For a working custom-props implementation, read `SampleComponent.fs` in [MacrosSample](examples.md#macrossample).
