module Terminal.Gui.Elmish.Tests.ElementsCompleteness

open System
open NUnit.Framework

[<Test>]
let ``Should include all elements in Elements module`` () =
  let viewTypes =
    typeof<Terminal.Gui.ViewBase.View>.Assembly.GetTypes()
    |> Array.filter (fun t -> t.IsAssignableTo(typeof<Terminal.Gui.ViewBase.View>))

  let terminalElementTypes =
    typeof<Terminal.Gui.Elmish.ITerminalElement>.Assembly.GetTypes()
    |> Array.filter (fun t -> t.IsAssignableTo(typeof<Terminal.Gui.Elmish.ITerminalElement>))

  let missingElements =
    viewTypes
    |> Array.filter (fun vt ->
      not (terminalElementTypes |> Array.exists (fun tet -> tet.Name = vt.Name + "Element"))
    )
    |> Array.map (fun t -> t.Name)

  let ignoredElements = [
    "CharMap"
    "BBar"
    "ColorBar"
    "GBar"
    "HueBar"
    "LightnessBar"
    "RBar"
    "SaturationBar"
    "ValueBar"
    "NumericUpDown`1"
    "Runnable`1"
    "RunnableWrapper`2"
    "FlagSelector`1"
    "OptionSelector`1"
    "Slider`1"
    "TabRow"
    "TreeView`1"
    "ShadowView"
    "View"
    "PopoverBaseImpl"
    "Popup"
    "ComboListView"
  ]

  let missingElements =
    missingElements
    |> Array.filter (fun name -> not (ignoredElements |> List.contains name))

  if missingElements.Length > 0 then
    ("Missing Terminal.Gui.Elmish elements for the following Terminal.Gui views:" + Environment.NewLine +
    String.concat Environment.NewLine missingElements)
    |> Assert.Fail
