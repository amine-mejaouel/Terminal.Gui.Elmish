# Views, properties, layout, and styling

The DSL mirrors Terminal.Gui with strongly typed F# builders. `View.Button` creates a desired button specification, and `ButtonProps` exposes the properties and events supported by its native Terminal.Gui type and base classes.

## The builder pattern

```fsharp
View.Button(fun p ->
  p.Key "save-button"
  p.Text "_Save"
  p.Enabled model.CanSave
  p.Width 12
  p.Accepting(fun _ -> dispatch (Save |> TerminalMsg.ofMsg)))
```

The builder callback only fills a property snapshot. It does not construct or mutate a native button immediately.

Most controls have two useful overloads:

```fsharp
View.FrameView(fun p ->
  p.Title "Details"
  p.Children children)

View.FrameView children
```

Controls with handwritten macros add a two-argument overload:

```fsharp
View.ListView(fun (p: ListViewProps) (m: ListViewMacros) ->
  p.Title "Items"
  m.Items model.Items)
```

Macros currently cover retained list/dropdown items and convenient menu construction. They complement the generated properties; they are not a separate rendering system.

## Discover controls and properties

Type `View.` and use IDE completion to discover available Terminal.Gui controls. Inside a builder, type `p.` to discover inherited and control-specific properties and events.

Common starting points include:

| Purpose | Views |
| --- | --- |
| Containers | `Runnable`, `FrameView`, `Dialog`, `TabView` |
| Text | `Label`, `TextField`, `TextView` |
| Actions | `Button`, `MenuBar`, `MenuItem`, `Shortcut`, `StatusBar` |
| Choices | `CheckBox`, `OptionSelector`, `ListView`, `DropDownList` |
| Feedback | `ProgressBar`, `SpinnerView` |

The generated DSL intentionally follows upstream Terminal.Gui naming. Use Terminal.Gui documentation and tests to understand a native property or event, then use the same member through the corresponding props builder. Terminal.Gui.Elmish documentation focuses on declarative ownership, identity, and Elmish integration rather than duplicating the complete upstream control reference.

## Children and F# type inference

`p.Children` accepts `IView list`. Explicit upcasts are sometimes needed when a list contains different generated view types:

```fsharp
p.Children
  [ View.Label(fun p -> p.Text "Name") :> IView
    View.TextField(fun p -> p.Text model.Name) :> IView
    View.Button(fun p -> p.Text "_Save") :> IView ]
```

Use normal children for controls that belong in the parent's `SubViews`. Some native properties own a view in a named slot instead:

```fsharp
View.Shortcut(fun p ->
  p.Key Key.F2
  p.Title "Edit"
  p.TargetView(View.Button(fun p -> p.Text "Edit selected")))
```

The renderer owns that target through the property; it does not also insert it as an ordinary child.

## Dimensions

Width and height use Terminal.Gui `Dim` values:

```fsharp
open Terminal.Gui.ViewBase

p.Width 24
p.Height(Dim.Fill 1)
p.Width(Dim.Percent 50)
p.Width(Dim.Auto())
```

- An integer is a fixed cell count.
- `Dim.Fill offset` consumes the remaining parent space, leaving the offset.
- `Dim.Percent value` uses a percentage of the parent.
- `Dim.Auto()` derives a size according to the native control's content and rules.

## Positions

`p.X` and `p.Y` use `TPos`, the declarative position type:

| Form | Purpose |
| --- | --- |
| `TPos.Default` | Coordinate zero; this is the default. |
| `TPos.Absolute n` | Fixed cell coordinate. |
| `TPos.Center` | Center on the relevant axis. |
| `TPos.Percent n` | Percentage position within the parent. |
| `TPos.AnchorEnd offset` | Anchor to the far edge; `None` means zero offset. |
| `TPos.X view`, `TPos.Y view` | Reuse another view's coordinate. |
| `TPos.Left view`, `Right`, `Top`, `Bottom` | Position relative to another desired view. |
| `TPos.Func (f, view)` | Calculate a Terminal.Gui position from another native view. |
| `TPos.Align (...)` | Use Terminal.Gui alignment groups. |

Relative positions refer to desired view specifications, not cached native controls:

```fsharp
let heading =
  View.Label(fun p ->
    p.Text "Account"
    p.X(TPos.Absolute 2)
    p.Y(TPos.Absolute 1))

let editor =
  View.TextField(fun p ->
    p.Text model.Name
    p.X(TPos.X heading)
    p.Y(TPos.Bottom heading)
    p.Width(Dim.Fill 2))

View.FrameView [ heading :> IView; editor :> IView ]
```

The renderer binds specifications from the current view call to retained native views before resolving the relationship. You do not need to keep `heading` at module scope.

Prefer the named `TPos` forms over `TPos.Func`. A freshly allocated function closure is a changed position dependency and can cause the relationship to be reapplied on every render.

## Responsive layout pattern

A practical application usually combines fixed edges with flexible content:

```fsharp
View.FrameView(fun p ->
  p.Title "Results"
  p.X(TPos.Absolute 1)
  p.Y(TPos.Absolute 3)
  p.Width(Dim.Fill 1)
  p.Height(Dim.Fill 2))
```

Avoid calculating terminal width in the Elmish model when Terminal.Gui dimensions and anchors can express the relationship directly.

## Theme-aware styling

Use named schemes and Terminal.Gui style values rather than hard-coded terminal colors where possible:

```fsharp
open Terminal.Gui.Drawing
open Terminal.Gui.ViewBase

let schemeName scheme = scheme.ToString()

View.FrameView(fun p ->
  p.Title "Tasks"
  p.BorderStyle LineStyle.Rounded
  p.SchemeName(schemeName Schemes.Accent)
  p.ShadowStyle ShadowStyles.Transparent)
```

This lets the interface follow the active Terminal.Gui theme and terminal capabilities.

Text containing an underscore declares a hot key when the control supports it:

```fsharp
p.Text "_Save"
```

Always make important actions reachable through focus navigation or explicit shortcuts; color and mouse interaction should not be the only cues.

Next: [state, events, and interaction](state-and-events.md).
