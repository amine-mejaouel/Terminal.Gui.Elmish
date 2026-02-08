module Terminal.Gui.Elmish.Tests.TerminalElementIdTests

open Elmish
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

