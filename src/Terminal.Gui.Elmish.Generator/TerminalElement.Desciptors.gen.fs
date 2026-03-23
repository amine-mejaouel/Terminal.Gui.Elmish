module Terminal.Gui.Elmish.Generator.TerminalElement_Descriptors

let gen () =
  let viewTypesToGen =
    Registry.ViewTypes.orderedByInheritance
    |> List.filter (fun t -> t <> typeof<Terminal.Gui.ViewBase.View> && not t.IsAbstract)

  seq {
    yield "namespace Terminal.Gui.Elmish"
    yield ""

    for viewType in viewTypesToGen do
      let typeName = getTypeNameWithoutArity viewType
      let propsName = typeName + "Props"
      let elementName = typeName + "TerminalElement"
      let genericBlock = genericTypeParamsWithConstraintsBlock viewType
      let genericParamsBlock = genericTypeParamsBlock viewType
      let returnInterface = Registry.TEInterfaces.GetAssignableInterface viewType

      yield $"type {typeName}{genericBlock}(props: {propsName}{genericParamsBlock}) ="
      yield $"  interface ViewBase with"
      yield $"    member _.CreateViewTE() = new {elementName}{genericParamsBlock}(props.props)"
      yield $"    member _.Props = props.props"

      if returnInterface <> "ITerminalElement" then
        yield $"  interface {returnInterface}"

      yield ""
  }
  |> CodeWriter.write "TerminalElement.Descriptors.gen.fs"
