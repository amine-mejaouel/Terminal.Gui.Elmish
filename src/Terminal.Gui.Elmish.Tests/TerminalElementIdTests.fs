module Terminal.Gui.Elmish.Tests.TerminalElementIdTests

open NUnit.Framework
open Terminal.Gui.Elmish

[<Test>]
let ``Simple ID test`` () =
  // Arrange
  let root = View.Runnable [ View.Label(fun p -> p.Text "Label") :> IView ]

  // Act
  use program = ElmishTester.render root

  let viewTE = program.ViewTE
  let labelTE = viewTE.Children.[0].GetViewBackedTE()

  Assert.Multiple(fun () ->
    Assert.That(viewTE.GetPath(), Is.EqualTo("root:Runnable"))
    Assert.That(labelTE.GetPath(), Is.EqualTo("root:Runnable|child[0]:Label")))

[<Test>]
let ``Component ID test`` () =
  // Arrange
  let testComp = TestComponent.create (fun p -> p.text "Test Component")
  let root = View.Runnable [ testComp :> IView ]

  // Act
  use program = ElmishTester.render root

  let testComp = testComp :> IElmishComponentTE
  let viewTE = program.ViewTE

  // Get the children from the component
  let children = testComp.Child.Children
  let label = children.[0]
  let button = children.[1]

  Assert.Multiple(fun () ->
    Assert.That(viewTE.GetPath(), Is.EqualTo("root:Runnable"))
    Assert.That(testComp.GetPath(), Is.EqualTo("root:Runnable|child[0]:TestComponent"))
    Assert.That(label.GetPath(), Is.EqualTo("root:Runnable|child[0]:TestComponent:Window|child[0]:Label"))
    Assert.That(button.GetPath(), Is.EqualTo("root:Runnable|child[0]:TestComponent:Window|child[1]:Button")))
