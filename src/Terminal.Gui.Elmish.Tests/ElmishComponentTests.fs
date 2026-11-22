module Terminal.Gui.Elmish.Tests.ElmishComponentTests

open System.Linq
open NUnit.Framework
open Terminal.Gui.Elmish
open Elmish

[<Test>]
let ``ElmishComponent is placed as a child DomNode`` () =

  let init _ = (), Cmd.none
  let update _ _ = (), Cmd.none

  let view _ _ = View.Button(fun _ -> ())

  let elmishComponent =
    ElmishTerminal.mkSimpleComponent "ElmishComponent" init update view

  let parent = View.Runnable [ elmishComponent ]

  // Act
  use program = ElmishTester.render parent

  let rootDom = program.ViewDom

  Assert.That(rootDom.Children.Count, Is.EqualTo 1)

  match rootDom.Children[0] with
  | ElmishComponentDomChild ce ->
    Assert.That(ce.Name, Is.EqualTo "ElmishComponent")

    Assert.That(
      rootDom.View.SubViews |> Seq.exists (fun v -> obj.ReferenceEquals(v, ce.View)),
      Is.True,
      "The component's view should be a SubView of its parent."
    )
  | ViewDomChild _ -> Assert.Fail("Expected an Elmish component child.")

[<Test>]
let ``ElmishComponent keeps its instance and view between elmish loops`` () =
  task {

    // Arrange
    let testComponentTE = TestComponent.create (fun p -> p.text "Test")

    let view = View.Runnable(fun (p: RunnableProps) -> p.Children [ testComponentTE ])

    use program = ElmishTester.render view

    let componentBefore =
      match program.ViewDom.Children[0] with
      | ElmishComponentDomChild ce -> ce
      | ViewDomChild _ -> failwith "expected a component child"

    let childWindowView = testComponentTE.Child.View

    // Act - trigger a re-render by dispatching a message
    let! _ = testComponentTE.ProcessMsg(TestComponent.Increment |> TerminalMsg.ofMsg)

    // Assert - decision B1 keeps the same component instance and its child view
    let componentAfter =
      match program.ViewDom.Children[0] with
      | ElmishComponentDomChild ce -> ce
      | ViewDomChild _ -> failwith "expected a component child"

    Assert.Multiple(fun () ->
      Assert.That(
        obj.ReferenceEquals(componentBefore, componentAfter),
        Is.True,
        "Component instance should be preserved (B1)."
      )

      Assert.That(
        obj.ReferenceEquals(childWindowView, testComponentTE.Child.View),
        Is.True,
        "Component child view should be preserved."
      )

      Assert.That(
        program.ViewDom.View.SubViews
        |> Seq.exists (fun v -> obj.ReferenceEquals(v, childWindowView)),
        Is.True,
        "The component's view should remain a SubView of the parent."
      ))
  }
