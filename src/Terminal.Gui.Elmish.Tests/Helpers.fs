[<AutoOpen>]
module Terminal.Gui.Elmish.Tests.Helpers

open System.Runtime.CompilerServices
open Terminal.Gui.Elmish

[<Extension>]
type internal TerminalElementExt() =

  [<Extension>]
  static member GetViewBackedTE(this : TerminalElement) =
      match this with
        | ViewBackedTE viewTe -> viewTe
        | ElmishComponentTE _ -> failwith "todo"


