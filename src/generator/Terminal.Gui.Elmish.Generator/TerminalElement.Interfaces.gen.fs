module Terminal.Gui.Elmish.Generator.TerminalElement_Interfaces

open System
open System.IO

let lowerCamelCase (s: String) =
  if String.IsNullOrEmpty s then s
  else s.[0..0].ToLower() + s.[1..]

let properties (interfaceType: Type) =
  interfaceType.GetProperties(
    System.Reflection.BindingFlags.Public |||
    System.Reflection.BindingFlags.Instance)
  |> Array.filter (fun p -> p.CanRead && p.CanWrite)

let events (interfaceType: Type) =
  interfaceType.GetEvents(
    System.Reflection.BindingFlags.Public |||
    System.Reflection.BindingFlags.Instance)
  |> Array.filter (fun e -> e.AddMethod.IsPublic && e.RemoveMethod.IsPublic)

let generateInterfaceHandler (interfaceType: Type) =
  let props = properties interfaceType
  let evts = events interfaceType
  
  // Skip interfaces with no properties or events
  if props.Length = 0 && evts.Length = 0 then
    Seq.empty
  else
    seq {
      let interfaceName = interfaceType.Name
      let cleanName = if interfaceName.StartsWith("I") then interfaceName.Substring(1) else interfaceName
      let moduleName = $"{lowerCamelCase cleanName}Interface"
      
      yield $"// {interfaceName} - used by elements that implement Terminal.Gui.ViewBase.{interfaceName}"
      yield $"type internal {cleanName}Interface ="
      
      // Generate removeProps
      yield $"  static member removeProps (terminalElement: TerminalElement) (view: {interfaceName}) (props: Props) ="
      
      if props.Length > 0 then
        yield "    // Properties"
        for prop in props do
          let propName = lowerCamelCase prop.Name
          yield $"    props"
          yield $"    |> Props.tryFind PKey.{moduleName}.{propName}"
          yield $"    |> Option.iter (fun _ -> view.{prop.Name} <- Unchecked.defaultof<_>)"
          yield ""
      
      if evts.Length > 0 then
        if props.Length > 0 then ()
        yield "    // Events"
        for event in evts do
          let eventName = lowerCamelCase event.Name
          yield $"    terminalElement.tryRemoveEventHandler PKey.{moduleName}.{eventName}"
          yield ""
      
      // Generate setProps
      yield $"  static member setProps (terminalElement: TerminalElement) (view: {interfaceName}) (props: Props) ="
      
      if props.Length > 0 then
        yield "    // Properties"
        for prop in props do
          let propName = lowerCamelCase prop.Name
          yield $"    props"
          yield $"    |> Props.tryFind PKey.{moduleName}.{propName}"
          yield $"    |> Option.iter (fun v -> view.{prop.Name} <- v)"
          yield ""
      
      if evts.Length > 0 then
        if props.Length > 0 then ()
        yield "    // Events"
        for event in evts do
          let eventName = lowerCamelCase event.Name
          yield $"    terminalElement.trySetEventHandler(PKey.{moduleName}.{eventName}, view.{event.Name})"
          yield ""
      
      yield ""
    }

let gen () =
  let outputPath = Path.Combine (Environment.CurrentDirectory, "src", "Terminal.Gui.Elmish", "TerminalElement.Interfaces.fs")
  
  // Get all interfaces from Terminal.Gui that we need to handle
  let interfaces = 
    typeof<Terminal.Gui.ViewBase.View>.Assembly.GetTypes()
    |> Array.filter (fun t -> 
      t.IsInterface && 
      t.Namespace = "Terminal.Gui.ViewBase" &&
      t.Name.StartsWith("I") &&
      t.Name <> "IApplication" &&
      t.Name <> "IDesignTimeProperties")
    |> Array.sortBy (fun t -> t.Name)
  
  seq {
    yield "namespace Terminal.Gui.Elmish"
    yield ""
    yield "open Terminal.Gui.ViewBase"
    yield ""
    
    for interfaceType in interfaces do
      yield! generateInterfaceHandler interfaceType
  }
  |> String.concat Environment.NewLine
  |> File.writeAllText outputPath
