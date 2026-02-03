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
      ElmishTerminal.mkSimple init update view
      |> ElmishTerminal.runComponent
      :?> IInternalTerminalElement

    Assert.That(terminalElement.IsElmishComponent, Is.True)
