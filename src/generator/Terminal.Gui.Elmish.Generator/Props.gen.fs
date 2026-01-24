module Terminal.Gui.Elmish.Generator.Props

let opens = [
    "open System"
    "open System.Collections.Generic"
    "open System.Text"
    "open System.Drawing"
    "open System.ComponentModel"
    "open System.IO"
    "open System.Collections.Specialized"
    "open System.Globalization"
    "open Terminal.Gui.App"
    "open Terminal.Gui.Drawing"
    "open Terminal.Gui.Drivers"
    "open Terminal.Gui.Elmish"
    "open Terminal.Gui.FileServices"
    "open Terminal.Gui.Input"
    "open Terminal.Gui.Text"
    "open Terminal.Gui.ViewBase"
    "open Terminal.Gui.Views"
  ]

let gen () =
  seq {
    yield "namespace Terminal.Gui.Elmish"
    yield ""
    yield! opens
    yield ""
    for viewType in Registry.ViewTypes.orderedByInheritance do
      if viewType.IsGenericType then
        let genericBlock = genericTypeParamsWithConstraintsBlock viewType
        yield $"type {getTypeNameWithoutArity viewType}Props{genericBlock}() ="
      else
        yield $"type {getTypeNameWithoutArity viewType}Props() ="
      if viewType = typeof<Terminal.Gui.ViewBase.View> then
        yield "  member val internal props = Props()"
        yield """
  member this.Children(children: ITerminalElement list) =
    this.props.add (
      PKey.View.children,
      List<_>(
        children
        |> List.map (fun x -> x :?> IInternalTerminalElement)
      )
    )
  """
      else
        yield "  inherit ViewProps()"

      let view = ViewMetadata.create viewType
      let genericBlock = genericTypeParamsBlock viewType

      if view.Type = typeof<Terminal.Gui.ViewBase.View> then
        yield $"  // Delayed Positions"
        yield $"  member this.X (value: TPos) ="
        yield $"    this.props.add (PKey.View.X_delayedPos, value)"
        yield $""
        yield $"  member this.Y (value: TPos) ="
        yield $"    this.props.add (PKey.View.Y_delayedPos, value)"
        yield $""

      if view.Properties.Length > 0 then
        yield "  // Properties"
      for prop in view.Properties do
        yield $"  member this.{prop.PKey} (value: {getFSharpTypeName prop.PropertyInfo.PropertyType}) ="
        yield $"    this.props.add (PKey.{Registry.ViewTypes.GetUniqueTypeName viewType}{genericBlock}.{prop.PKey}, value)"
        yield ""
        if prop.IsViewProperty then
          let valueType = Registry.TEInterfaces.CreateTEInterface prop.PropertyInfo.PropertyType
          yield $"  member this.{prop.PKey}(value: {valueType}) ="
          yield $"    this.props.add (PKey.{Registry.ViewTypes.GetUniqueTypeName viewType}{genericBlock}.{prop.PKey}_element, value)"
        yield ""
      if view.Events.Length > 0 then
        yield "  // Events"
      for event in view.Events do
        yield $"  member this.{event.PKey} (handler: {eventHandlerType event.EventInfo}) ="
        yield $"    this.props.add (PKey.{Registry.ViewTypes.GetUniqueTypeName viewType}{genericBlock}.{event.PKey}, handler)"
        yield ""
  }
  |> CodeWriter.write "Props.gen.fs"
