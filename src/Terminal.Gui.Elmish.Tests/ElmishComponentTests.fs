module Terminal.Gui.Elmish.Tests.ElmishComponentTests

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
