module Terminal.Gui.Elmish.Generator.Program

if CodeGenVersion.newGenNeeded then
  PKey.gen()
  Props.gen()
  Types.gen()
  TerminalElement_Elements.gen()
  View.gen()

  CodeGenVersion.saveCurrentCodeGenVersion ()

