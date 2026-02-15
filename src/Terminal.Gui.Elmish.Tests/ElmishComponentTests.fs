module Terminal.Gui.Elmish.Tests.ElmishComponentTests

open NUnit.Framework
open Terminal.Gui.Elmish
open Elmish

[<Test>]
let ``TerminalElement created by Elmish component is flagged as isElmishComponent`` () =

    let init _ = (), Cmd.none
    let update _ _ = (), Cmd.none

    let view _ _ = View.Button(fun _ -> ())

    let terminalElement =
      ElmishTerminal.mkSimpleComponent "ElmishComponent" init update view
      :?> IInternalTerminalElement

    Assert.That(terminalElement.IsElmishComponent, Is.True)

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

  let elmishComponent = elmishComponent :?> IInternalTerminalElement
  let parent = parent :?> IInternalTerminalElement

  Assert.That(elmishComponent.Origin.Parent, Is.Not.Null)
  Assert.That(elmishComponent.Origin.Parent.Value.GetPath(), Is.EqualTo(parent.GetPath()))
