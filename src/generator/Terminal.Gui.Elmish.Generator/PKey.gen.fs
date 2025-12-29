module Terminal.Gui.Elmish.Generator.PKey_gen

open System
open System.IO

let lowerCamelCase (s: String) =
  if String.IsNullOrEmpty s then s
  else s.[0..0].ToLower() + s.[1..]

let escapeReservedKeywords (name: string) =
  match name with
  | "type" | "module" | "open" | "let" | "in" | "do" | "if" | "then" | "else" 
  | "match" | "with" | "function" | "val" | "mutable" | "when" | "rec" 
  | "begin" | "end" | "for" | "to" | "done" | "while" | "and" | "or"
  | "not" | "true" | "false" | "namespace" | "as" | "assert" | "inherit" -> $"``{name}``"
  | _ -> name

let properties (viewType: Type) =
  viewType.GetProperties(
    System.Reflection.BindingFlags.Public |||
    System.Reflection.BindingFlags.Instance |||
    System.Reflection.BindingFlags.DeclaredOnly)
  |> Array.filter (fun p -> p.CanRead && p.CanWrite)

let events (viewType: Type) =
  viewType.GetEvents(
    System.Reflection.BindingFlags.Public |||
    System.Reflection.BindingFlags.Instance |||
    System.Reflection.BindingFlags.DeclaredOnly)
  |> Array.filter (fun e -> e.AddMethod.IsPublic && e.RemoveMethod.IsPublic)

let rec formatTypeName (t: Type) =
  let name = t.Name
  
  // If this is a generic parameter, just return the name without any prefix
  if t.IsGenericParameter then
    name
  else
    // Handle .NET to F# type mappings
    let baseName = 
      match name with
      | "Boolean" -> "bool"
      | "Int32" -> "int"
      | "Int64" -> "int64"
      | "String" -> "string"
      | "Single" -> "float32"
      | "Double" -> "float"
      | "Char" -> "char"
      | _ -> name
    
    // Handle generic types
    if t.IsGenericType then
      let genericArgs = t.GetGenericArguments()
      let baseName = if baseName.Contains("`") then baseName.Substring(0, baseName.IndexOf("`")) else baseName
      let formattedArgs = genericArgs |> Array.map formatTypeName |> String.concat ", "
      $"{baseName}<{formattedArgs}>"
    elif baseName.Contains("`") then
      // Escape backtick for non-closed generic parameters
      $"``{baseName}``"
    else
      baseName

let propertyKeyType (prop: System.Reflection.PropertyInfo) =
  $"ISimplePropKey<{formatTypeName prop.PropertyType}>"

let eventKeyType (event: System.Reflection.EventInfo) =
  let handlerType = event.EventHandlerType
  if handlerType.IsGenericType && handlerType.Name = "EventHandler`1" then
    let genericArgs = handlerType.GetGenericArguments()
    if genericArgs.Length > 0 then
      $"IEventPropKey<{formatTypeName genericArgs.[0]} -> unit>"
    else
      "IEventPropKey<EventArgs -> unit>"
  else
    "IEventPropKey<EventArgs -> unit>"

let generatePKeyClass (viewType: Type) =
  seq {
    let typeName = viewType.Name
    // Remove generic suffix from type name for class name
    let cleanTypeName = if typeName.Contains("`") then typeName.Substring(0, typeName.IndexOf("`")) else typeName
    let className = lowerCamelCase cleanTypeName
    let parentType = ViewTypes.parentViewType viewType
    
    yield $"  // {cleanTypeName}"
    
    // Handle generic types properly
    if viewType.IsGenericType then
      let genericParams = viewType.GetGenericArguments() |> Array.map (fun t -> $"'{t.Name}") |> String.concat ", "
      yield $"  type {className}PKeys<{genericParams}>() ="
    else
      yield $"  type {className}PKeys() ="
    
    // Handle inheritance
    let isViewBaseType = viewType.Name = "View" || viewType.FullName.Contains("Terminal.Gui.ViewBase.View")
    
    if not isViewBaseType then
      match parentType with
      | Some parent when parent.Name <> "View" && not (parent.FullName.Contains("Terminal.Gui.ViewBase.View")) ->
          let parentName = if parent.Name.Contains("`") then parent.Name.Substring(0, parent.Name.IndexOf("`")) else parent.Name
          let parentClassName = lowerCamelCase parentName
          if parent.IsGenericType then
            let genericParams = parent.GetGenericArguments() |> Array.map (fun t -> $"'{t.Name}") |> String.concat ", "
            yield $"    inherit {parentClassName}PKeys<{genericParams}>()"
          else
            yield $"    inherit {parentClassName}PKeys()"
      | _ ->
          yield $"    inherit viewPKeys()"
    
    // For View base type, add children property
    if isViewBaseType then
      yield $"    member val children: ISimplePropKey<System.Collections.Generic.List<IInternalTerminalElement>> = PropKey.Create.simple \"children\""
      yield ""
    
    let props = properties viewType
    let evts = events viewType
    
    if props.Length > 0 then
      yield "    // Properties"
      for prop in props do
        let propName = escapeReservedKeywords (lowerCamelCase prop.Name)
        let keyName = $"{className}.{lowerCamelCase prop.Name}"
        
        // Check if this is a delayed pos property
        if prop.Name = "X" || prop.Name = "Y" then
          yield $"    member val {propName}: ISimplePropKey<Pos> = PropKey.Create.simple \"{keyName}\""
          yield $"    member val {propName}_delayedPos: IDelayedPosKey = PropKey.Create.delayedPos \"{keyName}_delayedPos\""
        else
          let propType = formatTypeName prop.PropertyType
          yield $"    member val {propName}: ISimplePropKey<{propType}> = PropKey.Create.simple \"{keyName}\""
    
    if evts.Length > 0 then
      if props.Length > 0 then yield ""
      yield "    // Events"
      for event in evts do
        let eventName = escapeReservedKeywords (lowerCamelCase event.Name)
        let keyName = $"{className}.{lowerCamelCase event.Name}_event"
        let eventType = eventKeyType event
        yield $"    member val {eventName}: {eventType} = PropKey.Create.event \"{keyName}\""
    
    yield ""
  }

let generateModuleInstances () =
  seq {
    for viewType in ViewTypes.orderedByInheritance do
      let typeName = viewType.Name
      let cleanTypeName = if typeName.Contains("`") then typeName.Substring(0, typeName.IndexOf("`")) else typeName
      let className = lowerCamelCase cleanTypeName
      
      // Check if it's a generic type
      if viewType.IsGenericType then
        let genericParams = viewType.GetGenericArguments() |> Array.map (fun t -> $"'{t.Name}") |> String.concat ", "
        yield $"  let {className}<{genericParams}> = {className}PKeys<{genericParams}> ()"
      else
        yield $"  let {className} = {className}PKeys ()"
  }

let generateOrientationInterface () =
  seq {
    yield "  // IOrientation"
    yield "  module internal orientationInterface ="
    yield "    // Properties"
    yield "    let orientation : ISimplePropKey<Orientation> = PropKey.Create.simple \"orientation.orientation\""
    yield "    // Events"
    yield "    let orientationChanged: IEventPropKey<EventArgs<Orientation> -> unit> = PropKey.Create.event \"orientation.orientationChanged_event\""
    yield "    let orientationChanging: IEventPropKey<CancelEventArgs<Orientation> -> unit> = PropKey.Create.event \"orientation.orientationChanging_event\""
  }

let gen () =
  let outputPath = Path.Combine (Environment.CurrentDirectory, "src", "Terminal.Gui.Elmish", "PKey.fs")
  
  seq {
    yield "namespace Terminal.Gui.Elmish"
    yield ""
    yield "open System"
    yield "open System.Collections.Generic"
    yield "open System.Text"
    yield "open System.Drawing"
    yield "open System.ComponentModel"
    yield "open System.IO"
    yield "open System.Collections.Specialized"
    yield "open System.Globalization"
    yield "open Terminal.Gui.App"
    yield "open Terminal.Gui.Drawing"
    yield "open Terminal.Gui.Drivers"
    yield "open Terminal.Gui"
    yield ""
    yield "open Terminal.Gui.FileServices"
    yield "open Terminal.Gui.Input"
    yield ""
    yield "open Terminal.Gui.Text"
    yield ""
    yield "open Terminal.Gui.ViewBase"
    yield ""
    yield "open Terminal.Gui.Views"
    yield ""
    yield "/// Properties key index"
    yield "[<RequireQualifiedAccess>]"
    yield "module internal PKey ="
    yield ""
    
    // Generate View base type first
    yield! generatePKeyClass typeof<Terminal.Gui.ViewBase.View>
    
    for viewType in ViewTypes.orderedByInheritance do
      // Skip View since we already generated it
      if viewType.Name <> "View" && not (viewType.FullName.Contains("Terminal.Gui.ViewBase.View")) then
        yield! generatePKeyClass viewType
    
    yield! generateOrientationInterface ()
    yield ""
    
    // Generate module instances
    yield $"  let view = viewPKeys ()"
    for viewType in ViewTypes.orderedByInheritance do
      if viewType.Name <> "View" && not (viewType.FullName.Contains("Terminal.Gui.ViewBase.View")) then
        let typeName = viewType.Name
        let cleanTypeName = if typeName.Contains("`") then typeName.Substring(0, typeName.IndexOf("`")) else typeName
        let className = lowerCamelCase cleanTypeName
        
        // Check if it's a generic type
        if viewType.IsGenericType then
          let genericParams = viewType.GetGenericArguments() |> Array.map (fun t -> $"'{t.Name}") |> String.concat ", "
          yield $"  let {className}<{genericParams}> = {className}PKeys<{genericParams}> ()"
        else
          yield $"  let {className} = {className}PKeys ()"
  }
  |> String.concat Environment.NewLine
  |> File.writeAllText outputPath
