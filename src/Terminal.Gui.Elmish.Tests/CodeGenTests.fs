module Terminal.Gui.Elmish.Tests.ElementsCompleteness

open System
open NUnit.Framework

[<Test>]
let ``Should include all elements in Elements module`` () =
  let terminalElementName (viewType: Type) =
    let genericArityIndex = viewType.Name.IndexOf('`')

    if genericArityIndex = -1 then
      viewType.Name + "TerminalElement"
    else
      viewType.Name[.. genericArityIndex - 1]
      + "TerminalElement"
      + viewType.Name[genericArityIndex..]

  let viewTypes =
    typeof<Terminal.Gui.ViewBase.View>.Assembly.GetTypes()
    |> Array.filter (fun t -> t.IsAssignableTo(typeof<Terminal.Gui.ViewBase.View>) && t.IsPublic)

  let terminalElementTypes =
    typeof<Terminal.Gui.Elmish.ITerminalElement>.Assembly.GetTypes()
    |> Array.filter (fun t -> t.IsAssignableTo(typeof<Terminal.Gui.Elmish.ITerminalElement>))

  let missingElements =
    viewTypes
    |> Array.filter (fun vt ->
      not (
        terminalElementTypes
        |> Array.exists (fun tet -> tet.Name = terminalElementName vt)
      ))
    |> Array.map (fun t -> t.Name)

  if missingElements.Length > 0 then
    ("Missing Terminal.Gui.Elmish elements for the following Terminal.Gui views:"
     + Environment.NewLine
     + String.concat Environment.NewLine missingElements)
    |> Assert.Fail

[<Test>]
let ``Should emit each generated view interface once`` () =
  let generatedInterfaces =
    typeof<Terminal.Gui.Elmish.ITerminalElement>.Assembly.GetTypes()
    |> Array.filter (fun t -> t.FullName = "Terminal.Gui.Elmish.ITViewView")

  Assert.That(generatedInterfaces, Has.Length.EqualTo(1))

[<TestCase("RunnablePropHandler", 0)>]
[<TestCase("RunnablePropHandler", 1)>]
[<TestCase("DialogPropHandler", 0)>]
[<TestCase("DialogPropHandler", 1)>]
let ``Should emit generic and non-generic property handlers`` (typeName: string, genericArity: int) =
  let generatedName =
    if genericArity = 0 then
      typeName
    else
      $"{typeName}`{genericArity}"

  let handlerExists =
    typeof<Terminal.Gui.Elmish.ITerminalElement>.Assembly.GetTypes()
    |> Array.exists (fun t -> t.Name = generatedName)

  Assert.That(handlerExists, Is.True, $"Expected generated handler type {generatedName}.")
