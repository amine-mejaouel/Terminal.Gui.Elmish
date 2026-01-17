module Terminal.Gui.Elmish.Generator.Views

open System
open System.IO

let camelCase (str: string) =
  if String.IsNullOrEmpty(str) then str
  else Char.ToLowerInvariant(str.[0]).ToString() + str.Substring(1)

let generateViewMethod (viewType: Type) =
  let typeName = ViewType.cleanTypeName viewType
  let elementName = typeName + "Element"
  let propsName = camelCase typeName + "Props"
  let viewName = camelCase typeName
  let genericBlock = ViewType.genericTypeParamsWithConstraintsBlock viewType
  let genericParamsBlock = ViewType.genericTypeParamsBlock viewType
  
  // Check if this is a special case with macros
  let hasMacros = typeName = "MenuBar"
  
  // Check what interface this element implements
  // Special cases for generic types that return specific interfaces
  let returnInterface = 
    if viewType.IsGenericType then
      match typeName with
      | "Slider" -> ": ISliderElement"
      | "NumericUpDown" -> ": INumericUpDownElement"
      | "TreeView" -> ": ITreeViewElement"
      | _ -> 
          match Registry.TryGetNeededIElementInterface viewType with
          | Some interfaceName -> $" :> {interfaceName}"
          | None -> " :> ITerminalElement"
    else
      match Registry.TryGetNeededIElementInterface viewType with
      | Some interfaceName -> $" :> {interfaceName}"
      | None -> " :> ITerminalElement"
  
  seq {
    // XML doc comment
    yield $"  /// <seealso cref=\"Terminal.Gui.{typeName}\"/>"
    
    // First overload: set function
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
    
    // Second overload: children list
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
    
    // Third overload: x, y, title convenience
    if viewType.IsGenericType then
      yield $"  static member {viewName}{genericBlock}(x: int, y: int, title: string) ="
      yield $"    let setProps ="
      yield $"      fun (p: {propsName}{genericParamsBlock}) ->"
      yield $"        p.x (Pos.Absolute(x))"
      yield $"        p.y (Pos.Absolute(y))"
      yield $"        p.title title"
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
      yield $"        p.x (Pos.Absolute(x))"
      yield $"        p.y (Pos.Absolute(y))"
      yield $"        p.title title"
      yield ""
      yield $"    let viewProps = {propsName} ()"
      yield $"    setProps viewProps"
      yield $"    new {elementName}(viewProps.props){returnInterface}"
    yield ""
  }

let gen () =
  // Get all view types except View itself
  let viewTypesToGenerate =
    ViewType.viewTypesOrderedByInheritance
    |> List.filter (fun t -> t <> typeof<Terminal.Gui.ViewBase.View>)
    // Filter out special menu types that don't have standard Props
    |> List.filter (fun t -> 
      let typeName = ViewType.cleanTypeName t
      typeName <> "Menu" && 
      typeName <> "MenuBarItem" && 
      typeName <> "PopoverMenu" && 
      typeName <> "MenuItem" &&
      typeName <> "OptionSelector" &&
      typeName <> "FlagSelector"
    )
  
  seq {
    yield "(*"
    yield "#######################################"
    yield "#            Views.gen.fs             #"
    yield "#######################################"
    yield "*)"
    yield ""
    yield ""
    yield "namespace Terminal.Gui.Elmish"
    yield ""
    yield "open System"
    yield "open Terminal.Gui.Elmish"
    yield "open Terminal.Gui.ViewBase"
    yield "open Terminal.Gui.Views"
    yield ""
    yield "type View with"
    yield ""
    
    // Generate standard view methods
    for viewType in viewTypesToGenerate do
      yield! generateViewMethod viewType
  }
  |> String.concat Environment.NewLine
  |> File.writeAllText (Path.Combine (Environment.CurrentDirectory, "Views.gen.fs"))
