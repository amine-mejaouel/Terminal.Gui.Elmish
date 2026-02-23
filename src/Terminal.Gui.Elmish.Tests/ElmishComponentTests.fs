module Terminal.Gui.Elmish.Tests.ElmishComponentTests

open System.Linq
open NUnit.Framework
open Terminal.Gui.Elmish
open Elmish

[<Test>]
let ``ElmishComponent.Parent is set`` () =

  let init _ = (), Cmd.none
  let update _ _ = (), Cmd.none

  let view _ _ = View.Button(fun _ -> ())

  let elmishComponent =
    ElmishTerminal.mkSimpleComponent "ElmishComponent" init update view

  let parent =
    View.Runnable [
      elmishComponent
    ]

  // Act
  use _ = ElmishTester.render parent

  let elmishComponent = elmishComponent :?> ITerminalElementBase
  let parent = parent :?> ITerminalElementBase

  Assert.That(elmishComponent.Origin.ParentTerminalElement, Is.Not.Null)
  Assert.That(elmishComponent.Origin.ParentTerminalElement.Value.GetPath(), Is.EqualTo(parent.GetPath()))

[<Test>]
let ``ElmishComponent.Origin should keep correct value between elmish loops`` () =
  task {

    // Arrange
    let testComponentTE = TestComponent.create (fun p -> p.text "Test")

    let view =
      View.Runnable (fun (p: RunnableProps) ->
        p.Children [
          testComponentTE
        ]
      )

    use program = ElmishTester.render view

    // Get initial state
    let initialPath = testComponentTE.GetPath()

    // Act - trigger a re-render by dispatching a message
    let! _ = testComponentTE.ProcessMsg (TestComponent.Increment |> TerminalMsg.ofMsg)

    // Assert - Origin should be preserved
    let afterUpdateComponent = program.ViewTE.Children.First()
    let afterUpdateOrigin = afterUpdateComponent.Origin
    let afterUpdatePath = afterUpdateComponent.GetPath()

    Assert.Multiple(fun () ->
      // Path should remain consistent
      Assert.That(afterUpdatePath, Is.EqualTo(initialPath),
        $"Component path should be consistent: expected %s{initialPath}, got %s{afterUpdatePath}")

      // Origin should point to the correct parent
      Assert.That(afterUpdateOrigin.ParentTerminalElement.Value.GetPath(), Is.EqualTo(program.ViewTE.GetPath()),
        "Origin.ParentTerminalElement should point to root")
    )
  }

