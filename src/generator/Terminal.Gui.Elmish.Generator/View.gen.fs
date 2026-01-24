module Terminal.Gui.Elmish.Generator.View

open System

let camelCase (str: string) =
  if String.IsNullOrEmpty(str) then str
  else Char.ToLowerInvariant(str.[0]).ToString() + str.Substring(1)

let genMethods (viewType: Type) =
  let typeName = getTypeNameWithoutArity viewType
  let elementName = typeName + "TerminalElement"
  let propsName = typeName + "Props"
  let viewName = camelCase typeName
  let genericBlock = genericTypeParamsWithConstraintsBlock viewType
  let genericParamsBlock = genericTypeParamsBlock viewType

  // Check if this is a special case with macros
  let hasMacros =
    [ typeof<Terminal.Gui.Views.MenuBar>
      typeof<Terminal.Gui.Views.MenuBarItem> ]
    |> List.contains viewType

  let returnInterface = Registry.TEInterfaces.Get viewType

  seq {
    if hasMacros then
      let macrosName = camelCase typeName + "Macros"
      yield $"  static member {viewName}(set: {propsName} -> {macrosName} -> unit) ="
      yield $"    let props = {propsName} ()"
      yield $"    let macros = {macrosName} props"
      yield $"    set props macros"
      yield $"    new {elementName}(props.props)"
      yield $"    :> {returnInterface}"
    else if viewType.IsGenericType then
      yield $"  static member {viewName}{genericBlock}(set: {propsName}{genericParamsBlock} -> unit) ="
      yield $"    let viewProps = {propsName}{genericParamsBlock} ()"
      yield ""
      yield $"    set viewProps"
      yield $"    new {elementName}{genericParamsBlock}(viewProps.props)"
      yield $"    :> {returnInterface}"
    else
      yield $"  static member {viewName}(set: {propsName} -> unit) ="
      yield $"    let viewProps = {propsName} ()"
      yield $"    set viewProps"
      yield $"    new {elementName}(viewProps.props)"
      yield $"    :> {returnInterface}"
    yield ""

    if viewType.IsGenericType then
      yield $"  static member {viewName}{genericBlock}(children: ITerminalElement list) ="
      yield $"    let viewProps = {propsName}{genericParamsBlock} ()"
      yield ""
      yield $"    viewProps.Children children"
      yield $"    new {elementName}{genericParamsBlock}(viewProps.props)"
      yield $"    :> {returnInterface}"
    else
      yield $"  static member {viewName}(children: ITerminalElement list) ="
      yield $"    let viewProps = {propsName} ()"
      yield $"    viewProps.Children children"
      yield $"    new {elementName}(viewProps.props)"
      yield $"    :> {returnInterface}"
    yield ""
  }

let opens = [
    "open System"
    "open Terminal.Gui.Elmish"
    "open Terminal.Gui.ViewBase"
    "open Terminal.Gui.Views"
  ]

let gen () =
  let viewTypesToGen =
    Registry.ViewTypes.orderedByInheritance
    |> List.filter (fun t -> t <> typeof<Terminal.Gui.ViewBase.View> && not t.IsAbstract)

  seq {
    yield "namespace Terminal.Gui.Elmish"
    yield ""
    yield! opens
    yield ""
    yield "type View ="
    yield ""

    for viewType in viewTypesToGen do
      yield! genMethods viewType
  }
  |> CodeWriter.write "View.gen.fs"
