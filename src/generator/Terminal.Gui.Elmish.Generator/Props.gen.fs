module Terminal.Gui.Elmish.Generator.Props

let opens = [
    "open System"
    "open System.Collections.Generic"
    "open Terminal.Gui.App"
    "open Terminal.Gui.Elmish"
    "open Terminal.Gui.ViewBase"
    "open Terminal.Gui.Views"
  ]

let viewSpecificMembers =
  seq {
    yield "  member val internal props = Props()"
    yield """
  member this.Children(children: ITerminalElement list) =
    children
    |> List.map (fun x -> TerminalElement.from x)
    |> this.props.Children.AddRange
  """
    yield "  // Positions"
    yield "  member this.X (value: Pos) ="
    yield "    this.props.X <- Some value"
    yield ""
    yield "  member this.Y (value: Pos) ="
    yield "    this.props.Y <- Some value"
    yield ""
    yield "  // Delayed Positions"
    yield "  member this.X (value: TPos) ="
    yield "    this.props.XDelayed <- Some value"
    yield ""
    yield "  member this.Y (value: TPos) ="
    yield "    this.props.YDelayed <- Some value"
    yield ""
  }

let gen () =
  seq {
    yield "namespace Terminal.Gui.Elmish"
    yield ""
    yield! opens
    yield ""
    for viewType in Registry.ViewTypes.orderedByInheritance do
      let genericBlock = genericTypeParamsWithConstraintsBlock viewType
      yield $"type {getTypeNameWithoutArity viewType}Props{genericBlock}() ="
      if viewType <> typeof<Terminal.Gui.ViewBase.View> then
        let genericBlock = genericTypeParamsBlock viewType.ParentViewType
        yield $"  inherit {getTypeNameWithoutArity viewType.ParentViewType}Props{genericBlock}()"
      else
        yield! viewSpecificMembers

      let view = ViewMetadata.create viewType

      if view.Properties.Length > 0 then
        yield "  // Properties"
      for prop in view.Properties do
        yield $"  member this.{prop.PKey} (value: {getFSharpTypeName prop.PropertyInfo.PropertyType}) ="
        yield $"    this.props.add ({PKey.getAccessor viewType}.{prop.PKey}, value)"
        yield ""
        if prop.IsViewProperty then
          let valueType = Registry.TEInterfaces.CreateInterface prop.PropertyInfo.PropertyType
          yield $"  member this.{prop.PKey}(value: {valueType}) ="
          yield $"    this.props.add ({PKey.getAccessor viewType}.{prop.PKey}_element, value)"
        yield ""
      if view.Events.Length > 0 then
        yield "  // Events"
      for event in view.Events do
        yield $"  member this.{event.PKey} (handler: {eventHandlerType event.EventInfo}) ="
        yield $"    this.props.add ({PKey.getAccessor viewType}.{event.PKey}, handler)"
        yield ""
  }
  |> CodeWriter.write "Props.gen.fs"
