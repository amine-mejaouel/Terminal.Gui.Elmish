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

[<Test>]
let ``ElmishComponent follows its identity across a reorder`` () =
  task {
    // Distinct explicit ids (decision B2) so each instance is tracked across the reorder.
    let mkComponent id title =
      ElmishTerminal.mkSimpleComponentWithId
        id
        "TestComponent"
        (fun () -> ())
        (fun (_: unit) model -> model)
        (fun _ _ -> View.Label(fun p -> p.Text title))

    let compA = mkComponent "A" "A"
    let compB = mkComponent "B" "B"

    let init _ = false
    let update (reordered: bool) (_: bool) = reordered

    let view reordered (_: Dispatch<TerminalMsg<bool>>) =
      let children = if reordered then [ compB; compA ] else [ compA; compB ]

      View.Runnable(fun (p: RunnableProps) -> p.Children children)

    use program = ElmishTester.renderStateful init update view

    let componentAt i =
      match program.ViewDom.Children[i] with
      | ElmishComponentDomChild ce -> ce
      | ViewDomChild _ -> failwith "expected a component child"

    let firstBefore = componentAt 0
    let secondBefore = componentAt 1

    // Act: reorder the children.
    let! _ = program.ProcessMsg(true |> TerminalMsg.ofMsg)

    // Assert: the same instances are reused, only their positions swapped.
    let firstAfter = componentAt 0
    let secondAfter = componentAt 1

    Assert.Multiple(fun () ->
      Assert.That(
        obj.ReferenceEquals(firstAfter, secondBefore),
        Is.True,
        "B should move to slot 0 as the same instance."
      )

      Assert.That(
        obj.ReferenceEquals(secondAfter, firstBefore),
        Is.True,
        "A should move to slot 1 as the same instance."
      ))
  }

[<Test>]
let ``mkSimpleComponentWithId decouples ComponentId from Name`` () =
  let component =
    ElmishTerminal.mkSimpleComponentWithId "component-42" "MyComponent" (fun () -> ()) (fun _ m -> m) (fun _ _ ->
      View.Button(fun _ -> ()))

  let ce = component :?> IElmishComponentTE
  Assert.That(ce.ComponentId, Is.EqualTo "component-42")
  Assert.That(ce.Name, Is.EqualTo "MyComponent")
