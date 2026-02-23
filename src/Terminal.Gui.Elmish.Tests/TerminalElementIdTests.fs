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

  let label = label :?> IViewTE
  let view = view :?> IViewTE

  Assert.Multiple(fun () ->
    Assert.That(view.GetPath(), Is.EqualTo("root:Runnable"))
    Assert.That(label.GetPath(), Is.EqualTo("root:Runnable|child[0]:Label"))
  )

[<Test>]
let ``Component ID test`` () =
  // Arrange
  let testComp = TestComponent.create (fun p -> p.text "Test Component")
  let view =
    View.Runnable [
      testComp :> ITerminalElement
    ]

  // Act
  use _ = ElmishTester.render view

  let testComp = testComp :> IElmishComponentTE
  let view = view :?> IViewTE

  // Get the children from the component
  let children = testComp.Child.Children
  let label = children.[0]
  let button = children.[1]

  Assert.Multiple(fun () ->
    Assert.That(view.GetPath(), Is.EqualTo("root:Runnable"))
    Assert.That(testComp.GetPath(), Is.EqualTo("root:Runnable|child[0]:TestComponent"))
    Assert.That(label.GetPath(), Is.EqualTo("root:Runnable|child[0]:TestComponent:Window|child[0]:Label"))
    Assert.That(button.GetPath(), Is.EqualTo("root:Runnable|child[0]:TestComponent:Window|child[1]:Button"))
  )

