module Terminal.Gui.Elmish.Generator.View

open System

let genMethods (viewType: Type) =
  let typeName = getTypeNameWithoutArity viewType
  let propsName = typeName + "Props"
  let genericBlock = genericTypeParamsWithConstraintsBlock viewType
  let genericParamsBlock = genericTypeParamsBlock viewType

  // Check if this is a special case with macros
  let hasMacros =
    [ typeof<Terminal.Gui.Views.MenuBar>; typeof<Terminal.Gui.Views.MenuBarItem> ]
    |> List.contains viewType

  seq {
    if hasMacros then
      let macrosName = typeName + "Macros"
      yield $"  static member {typeName}(set: {propsName} -> {macrosName} -> unit) ="
      yield $"    let props = {propsName} ()"
      yield $"    let macros = {macrosName} props"
      yield $"    set props macros"
      yield $"    {typeName}(props)"
      yield ""

    yield $"  static member {typeName}{genericBlock}(set: {propsName}{genericParamsBlock} -> unit) ="
    yield $"    let viewProps = {propsName}{genericParamsBlock} ()"
    yield $"    set viewProps"
    yield $"    {typeName}{genericParamsBlock}(viewProps)"

    yield ""

    yield $"  static member {typeName}{genericBlock}(children: ITerminalElement list) ="
    yield $"    let viewProps = {propsName}{genericParamsBlock} ()"
    yield $"    viewProps.Children children"
    yield $"    {typeName}{genericParamsBlock}(viewProps)"

    yield ""
  }

let opens = []

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
