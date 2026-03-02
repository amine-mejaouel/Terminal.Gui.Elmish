module Terminal.Gui.Elmish.Generator.Program

[<EntryPoint>]
let main _argv =
  PKey.gen()
  Props.gen()
  Types.gen()
  TerminalElement_Elements.gen()
  View.gen()

  0

