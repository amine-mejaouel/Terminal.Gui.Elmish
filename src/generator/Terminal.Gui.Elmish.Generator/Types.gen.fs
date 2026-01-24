module Terminal.Gui.Elmish.Generator.Types

/// Should be called after PKey.gen() as it depends on Registry.NeededIElementInterfaces
let gen () =
  seq {
    yield "namespace Terminal.Gui.Elmish"
    yield ""
    for i in Registry.GetNeededIElementInterfaces() do
      yield $"type {i} ="
      yield "  inherit ITerminalElement"
      yield ""
  }
  |> CodeWriter.write "Types.gen.fs"

