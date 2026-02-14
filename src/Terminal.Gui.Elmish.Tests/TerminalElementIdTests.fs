module Terminal.Gui.Elmish.Tests.TerminalElementIdTests

open NUnit.Framework
open Terminal.Gui.Elmish

[<Test>]
let ``Simple ID test`` () =
  // Arrange
  let label = View.Label (fun p -> p.Text "Label")
  let view =
    View.Runnable [
      label
    ]

  // Act
  use _ = ElmishTester.render view

  let label = label :?> IInternalTerminalElement
  let view = view :?> IInternalTerminalElement

  Assert.Multiple(fun () ->
    Assert.That(view.Id.ToString(), Is.EqualTo("root"))
    Assert.That(label.Id.ToString(), Is.EqualTo("root|child[0]"))
  )

[<Test>]
let ``Component ID test`` () =
  // Arrange
  let testComp = TestComponent._component (fun p -> p.text "Test Component")
  let view =
    View.Runnable [
      testComp
    ]

  // Act
  use _ = ElmishTester.render view

  let testComp = testComp :?> IInternalTerminalElement
  let view = view :?> IInternalTerminalElement

  // Get the children from the component
  let children = testComp.Children
  let label = children.[0]
  let button = children.[1]

  Assert.Multiple(fun () ->
    Assert.That(view.Id.ToString(), Is.EqualTo("root"))
    Assert.That(testComp.Id.ToString(), Is.EqualTo("root|child[0]"))
    Assert.That(label.Id.ToString(), Is.EqualTo("root|child[0]|child[0]"))
    Assert.That(button.Id.ToString(), Is.EqualTo("root|child[0]|child[1]"))
  )

