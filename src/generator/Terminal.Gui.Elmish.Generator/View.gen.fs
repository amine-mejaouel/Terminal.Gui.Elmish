module Terminal.Gui.Elmish.Generator.View

open System
open System.IO

let camelCase (str: string) =
  if String.IsNullOrEmpty(str) then str
  else Char.ToLowerInvariant(str.[0]).ToString() + str.Substring(1)

let generateMethods (viewType: Type) =
  let typeName = ViewType.cleanTypeName viewType
  let elementName = typeName + "TerminalElement"
  let propsName = typeName + "Props"
  let viewName = camelCase typeName
  let genericBlock = ViewType.genericTypeParamsWithConstraintsBlock viewType
  let genericParamsBlock = ViewType.genericTypeParamsBlock viewType

  // Check if this is a special case with macros
  let hasMacros =
    [ typeof<Terminal.Gui.Views.MenuBar>
      typeof<Terminal.Gui.Views.MenuBarItem> ]
    |> List.contains viewType

  let returnInterface =
      match Registry.TryGetNeededIElementInterface viewType with
      | Some interfaceName -> $" :> {interfaceName}"
      | None -> " :> ITerminalElement"

  seq {
    if hasMacros then
      let macrosName = camelCase typeName + "Macros"
      yield $"  static member {viewName}(set: {propsName} -> {macrosName} -> unit) ="
      yield $"    let props = {propsName} ()"
      yield $"    let macros = {macrosName} props"
      yield $"    set props macros"
      yield $"    new {elementName}(props.props){returnInterface}"
    else if viewType.IsGenericType then
      yield $"  static member {viewName}{genericBlock}(set: {propsName}{genericParamsBlock} -> unit) ="
      yield $"    let viewProps = {propsName}{genericParamsBlock} ()"
      yield ""
      yield $"    set viewProps"
      yield $"    new {elementName}{genericParamsBlock}(viewProps.props)"
      yield $"    {returnInterface}"
    else
      yield $"  static member {viewName}(set: {propsName} -> unit) ="
      yield $"    let viewProps = {propsName} ()"
      yield $"    set viewProps"
      yield $"    new {elementName}(viewProps.props){returnInterface}"
    yield ""

    if viewType.IsGenericType then
      yield $"  static member {viewName}{genericBlock}(children: ITerminalElement list) ="
      yield $"    let viewProps = {propsName}{genericParamsBlock} ()"
      yield ""
      yield $"    viewProps.children children"
      yield $"    new {elementName}{genericParamsBlock}(viewProps.props)"
      yield $"    {returnInterface}"
    else
      yield $"  static member {viewName}(children: ITerminalElement list) ="
      yield $"    let viewProps = {propsName} ()"
      yield $"    viewProps.children children"
      yield $"    new {elementName}(viewProps.props){returnInterface}"
    yield ""

    if viewType.IsGenericType then
      yield $"  static member {viewName}{genericBlock}(x: int, y: int, title: string) ="
      yield $"    let setProps ="
      yield $"      fun (p: {propsName}{genericParamsBlock}) ->"
      yield $"        p.X (Pos.Absolute(x))"
      yield $"        p.Y (Pos.Absolute(y))"
      yield $"        p.Title title"
      yield ""
      yield $"    let viewProps = {propsName}{genericParamsBlock} ()"
      yield ""
      yield $"    setProps viewProps"
      yield $"    new {elementName}{genericParamsBlock}(viewProps.props)"
      yield $"    {returnInterface}"
    else
      yield $"  static member {viewName}(x: int, y: int, title: string) ="
      yield $"    let setProps ="
      yield $"      fun (p: {propsName}) ->"
      yield $"        p.X (Pos.Absolute(x))"
      yield $"        p.Y (Pos.Absolute(y))"
      yield $"        p.Title title"
      yield ""
      yield $"    let viewProps = {propsName} ()"
      yield $"    setProps viewProps"
      yield $"    new {elementName}(viewProps.props){returnInterface}"
    yield ""
  }

let gen () =
  let viewTypesToGenerate =
    ViewType.viewTypesOrderedByInheritance
    |> List.filter (fun t -> t <> typeof<Terminal.Gui.ViewBase.View> && not t.IsAbstract)

  seq {
    yield "namespace Terminal.Gui.Elmish"
    yield ""
    yield "open System"
    yield "open Terminal.Gui.Elmish"
    yield "open Terminal.Gui.ViewBase"
    yield "open Terminal.Gui.Views"
    yield ""
    yield "type View ="
    yield ""

    for viewType in viewTypesToGenerate do
      yield! generateMethods viewType
  }
  |> String.concat Environment.NewLine
  |> File.writeAllText (Path.Combine (Environment.CurrentDirectory, "View.gen.fs"))
