module Terminal.Gui.Elmish.Generator.Props

let gen () =
  seq {
    yield "namespace Terminal.Gui.Elmish"
    yield ""
    yield "open System"
    yield "open System.Collections.Generic"
    yield "open System.Collections.ObjectModel"
    yield "open System.Text"
    yield "open System.Drawing"
    yield "open System.ComponentModel"
    yield "open System.IO"
    yield "open System.Collections.Specialized"
    yield "open System.Globalization"
    yield "open Terminal.Gui.App"
    yield "open Terminal.Gui.Drawing"
    yield "open Terminal.Gui.Drivers"
    yield "open Terminal.Gui.Elmish"
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
    for viewType in ViewType.viewTypesOrderedByInheritance do
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
        yield $"    this.props.add (PKey.{Registry.GetUniqueTypeName viewType}{genericBlock}.{prop.PKey}, value)"
        yield ""
        if prop.IsViewProperty then
          let valueType = Registry.SetNeededIElementInterface prop.PropertyInfo.PropertyType
          yield $"  member this.{prop.PKey}(value: {valueType}) ="
          yield $"    this.props.add (PKey.{Registry.GetUniqueTypeName viewType}{genericBlock}.{prop.PKey}_element, value)"
        yield ""
      if view.Events.Length > 0 then
        yield "  // Events"
      for event in view.Events do
        yield $"  member this.{event.PKey} (handler: {eventHandlerType event.EventInfo}) ="
        yield $"    this.props.add (PKey.{Registry.GetUniqueTypeName viewType}{genericBlock}.{event.PKey}, handler)"
        yield ""
  }
  |> CodeWriter.write "Props.gen.fs"
