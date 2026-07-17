module Terminal.Gui.Elmish.Generator.SimpleViewSpec

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

      let propHandlerName =
        $"{getTypeNameWithoutArity viewType}PropHandler{genericTypeParamsBlock viewType}"

      let genericBlock = genericTypeParamsWithConstraintsBlock viewType
      let genericParamsBlock = genericTypeParamsBlock viewType
      let returnInterface = Registry.TEInterfaces.GetAssignableInterface viewType

      yield $"type {typeName}{genericBlock}(props: {propsName}{genericParamsBlock}) ="
      yield $"  let mutable viewTe: IViewTE voption = ValueNone"
      yield $""
      yield $"  let getOrCreateViewTE () ="
      yield $"    match viewTe with"
      yield $"    | ValueSome value -> value"
      yield $"    | ValueNone ->"
      yield $"      let value = new {elementName}{genericParamsBlock}(props.props) :> IViewTE"
      yield $"      viewTe <- ValueSome value"
      yield $"      value"
      yield $""
      yield $"  interface ISimpleViewSpec with"
      yield $"    member _.CreateViewTE() = getOrCreateViewTE ()"
      yield $"    member _.BindViewTE(value) = viewTe <- ValueSome value"
      yield $"    member _.Props = props.props"
      yield $"    member _.ApplyNativeProps(target, changedProps) ="
      yield $"      {propHandlerName}.applyNativeProps(target :?> ViewBackedTerminalElement, changedProps)"
      yield $"    member _.ClearProp(target, propertyId) ="
      yield $"      {propHandlerName}.clearProp(target :?> ViewBackedTerminalElement, propertyId)"
      yield $"    member _.ViewType = ViewType.{getDuCaseTypeName viewType}"

      if returnInterface <> "ITerminalElement" then
        let interfaceName = returnInterface.Replace("TerminalElement", "View")
        yield $"  interface {interfaceName}"

      yield ""
  }
  |> CodeWriter.write __SOURCE_FILE__
