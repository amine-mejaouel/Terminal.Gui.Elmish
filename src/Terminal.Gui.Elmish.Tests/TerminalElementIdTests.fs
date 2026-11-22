module Terminal.Gui.Elmish.Tests.TerminalElementIdTests

open NUnit.Framework
open Terminal.Gui.Elmish

let private asViewDomNode (child: DomChild) : DomNode =
  match child with
  | ViewDomChild node -> node
  | ElmishComponentDomChild _ -> failwith "expected a view DomNode"

[<Test>]
let ``Simple ID test`` () =
  // Arrange
  let label = View.Label(fun p -> p.Text "Label")
  let view = View.Runnable [ label ]

  // Act
  use program = ElmishTester.render view

  let rootDom = program.ViewDom
  let labelDom = rootDom.Children[0] |> asViewDomNode

  Assert.Multiple(fun () ->
    Assert.That(rootDom.GetPath(), Is.EqualTo("root:Runnable"))
    Assert.That(labelDom.GetPath(), Is.EqualTo("root:Runnable|child[0]:Label")))

[<Test>]
let ``Component ID test`` () =
  // Arrange
  let testComp = TestComponent.create (fun p -> p.text "Test Component")
  let view = View.Runnable [ testComp :> ITerminalElement ]

  // Act
  use program = ElmishTester.render view

  let rootDom = program.ViewDom

  // The component's child subtree is a DomNode tree (Window -> Label, Button).
  // Full DomNode-native component paths are a Phase 7 item (see plan-staticDom.md,
  // decision A), so this asserts structure.
  let windowDom = testComp.Child
  let labelDom = windowDom.Children[0] |> asViewDomNode
  let buttonDom = windowDom.Children[1] |> asViewDomNode

  Assert.Multiple(fun () ->
    Assert.That(rootDom.GetPath(), Is.EqualTo("root:Runnable"))
    Assert.That(windowDom.Name, Is.EqualTo("Window"))
    Assert.That(labelDom.Name, Is.EqualTo("Label"))
    Assert.That(buttonDom.Name, Is.EqualTo("Button")))
