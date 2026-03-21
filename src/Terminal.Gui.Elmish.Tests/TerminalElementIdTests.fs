module Terminal.Gui.Elmish.Tests.TerminalElementIdTests

open NUnit.Framework
open Terminal.Gui.Elmish

[<Test>]
let ``Simple ID test`` () =
  // Arrange
  let label = View.Label(fun p -> p.Text "Label")
  let view = View.Runnable [ label ]

  // Act
  use _ = ElmishTester.render view

  let label = label :?> IViewTE
  let view = view :?> IViewTE

  Assert.Multiple(fun () ->
    Assert.That(view.Address |> Address.getPath, Is.EqualTo("root"))
    Assert.That(label.Address |> Address.getPath, Is.EqualTo("root:child[0]")))

[<Test>]
let ``Component ID test`` () =
  // Arrange
  let testComp = TestComponent.create (fun p -> p.text "Test Component")
  let view = View.Runnable [ testComp :> ITerminalElement ]

  // Act
  use _ = ElmishTester.render view

  let testComp = testComp :> IElmishComponentTE
  let view = view :?> IViewTE

  // Get the children from the component
  let children = testComp.Child.Children
  let label = children.[0]
  let button = children.[1]

  Assert.Multiple(fun () ->
    Assert.That(view.Address |> Address.getPath, Is.EqualTo("root"))
    Assert.That(testComp.Address |> Address.getPath, Is.EqualTo("root:child[0](component)"))
    Assert.That(label.Address |> Address.getPath, Is.EqualTo("root:child[0](component):child[0]"))
    Assert.That(button.Address |> Address.getPath, Is.EqualTo("root:child[0](component):child[1]")))
