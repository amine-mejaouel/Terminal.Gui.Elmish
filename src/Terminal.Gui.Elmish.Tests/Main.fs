module Main

open NUnit.Framework
open Terminal.Gui.Elmish

[<SetUpFixture>]
type AssemblySetup() =
  [<OneTimeSetUp>]
  member _.Setup() =
    ElmishTerminal.unitTestMode <- true

[<EntryPoint>]
let main _ = 0
