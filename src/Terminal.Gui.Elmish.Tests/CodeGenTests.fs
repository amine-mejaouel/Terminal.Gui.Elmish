module Terminal.Gui.Elmish.Tests.ElementsCompleteness

open System
open NUnit.Framework

[<Test>]
let ``Should include all elements in Elements module`` () =
  let viewTypes =
    typeof<Terminal.Gui.ViewBase.View>.Assembly.GetTypes()
    |> Array.filter (fun t -> t.IsAssignableTo(typeof<Terminal.Gui.ViewBase.View>) && t.IsPublic)

  let terminalElementTypes =
    typeof<Terminal.Gui.Elmish.ITerminalElement>.Assembly.GetTypes()
    |> Array.filter (fun t -> t.IsAssignableTo(typeof<Terminal.Gui.Elmish.ITerminalElement>))

  let missingElements =
    viewTypes
    |> Array.filter (fun vt ->
      not (terminalElementTypes |> Array.exists (fun tet -> tet.Name = vt.Name + "TerminalElement"))
    )
    |> Array.map (fun t -> t.Name)

  // TODO: Should generate these components automatically
  let ignoredElements = [
    "Dialog`1"
    "LinearRange`1"
    "NumericUpDown`1"
    "Prompt`2"
    "Runnable`1"
    "FlagSelector`1"
    "OptionSelector`1"
    "TreeView`1"
  ]

  let missingElements =
    missingElements
    |> Array.filter (fun name -> not (ignoredElements |> List.contains name))

  if missingElements.Length > 0 then
    ("Missing Terminal.Gui.Elmish elements for the following Terminal.Gui views:" + Environment.NewLine +
    String.concat Environment.NewLine missingElements)
    |> Assert.Fail
