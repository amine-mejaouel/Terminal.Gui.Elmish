module Terminal.Gui.Elmish.Generator.Types

open System
open System.IO

let outputPath = Path.Combine (Environment.CurrentDirectory, "Types.gen.fs")

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
  |> String.concat Environment.NewLine
  |> File.writeAllText outputPath

