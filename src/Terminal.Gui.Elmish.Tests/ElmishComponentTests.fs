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

  let parent = View.Runnable [ elmishComponent ]

  // Act
  use _ = ElmishTester.render parent

  let elmishComponent = elmishComponent :?> ITerminalElementBase
  let parent = parent :?> ITerminalElementBase

  // Verify the elmish component has a non-root address (i.e., it is a child)
  Assert.That(
    elmishComponent.Address.Length,
    Is.GreaterThan(1),
    "ElmishComponent should have a parent (non-root address)"
  )
  // Verify the component's parent path matches the parent element's path
  Assert.That(
    elmishComponent.ParentPath,
    Is.EqualTo(parent.GetPath()),
    "ElmishComponent parent path should match parent's path"
  )

[<Test>]
let ``ElmishComponent.Origin should keep correct value between elmish loops`` () =
  task {

    // Arrange
    let testComponentTE = TestComponent.create (fun p -> p.text "Test")

    let view = View.Runnable(fun (p: RunnableProps) -> p.Children [ testComponentTE ])

    use program = ElmishTester.render view

    // Get initial state
    let initialPath = testComponentTE.GetPath()

    // Act - trigger a re-render by dispatching a message
    let! _ = testComponentTE.ProcessMsg(TestComponent.Increment |> TerminalMsg.ofMsg)

    // Assert - Origin should be preserved
    let afterUpdateComponent = program.ViewTE.Children.First()
    let afterUpdateOrigin = afterUpdateComponent.Address
    let afterUpdatePath = afterUpdateComponent.GetPath()

    Assert.Multiple(fun () ->
      // Path should remain consistent
      Assert.That(
        afterUpdatePath,
        Is.EqualTo(initialPath),
        $"Component path should be consistent: expected %s{initialPath}, got %s{afterUpdatePath}"
      )

      // Origin should point to the correct parent
      Assert.That(
        afterUpdateComponent.ParentPath,
        Is.EqualTo(program.ViewTE.GetPath()),
        "ParentPath should point to root"
      ))
  }
