module Terminal.Gui.Elmish.Generator.ViewType

let gen () =
  seq {
    yield "namespace Terminal.Gui.Elmish"
    yield ""

    yield "type ViewType ="

    for i in Registry.ViewTypes.orderedByInheritance do
      yield $"  | {(getDuCaseTypeName i)}"

  }
  |> CodeWriter.write "ViewType.gen.fs"
