namespace Terminal.Gui.Elmish

open System
open System.Collections.Generic
open System.Collections.Specialized
open System.Threading
open Terminal.Gui.Elmish
open Terminal.Gui.ViewBase


/// <p>
///   Repository for event handlers associated with property keys.
///   It allows setting and removing handlers for events.
/// </p>
/// <p>
///   Its main purpose is to ensure the proper removal of events, avoiding memory leaks or unintended behavior due to lingering event handlers.
/// </p>
type internal EventHandlerRegistrar() =

  /// Stores the stable proxy handlers subscribed to Terminal.Gui events.
  let trackedHandlers = Dictionary<PropKey, Delegate>()

  /// Stores the latest Elmish callback. Proxy handlers read this dictionary when invoked,
  /// so a render can replace a closure without removing and re-adding the event subscription.
  let currentActions = Dictionary<PropKey, obj>()

  /// Stores functions to be invoked to remove previously added handlers.
  /// Which will call IEvent.RemoveHandler on the event associated with the property key.
  let handlerRemovalActions = Dictionary<PropKey, unit -> unit>()

  member private this.TryGetHandlerRemovalAction(pkey: PropKey) =
    match handlerRemovalActions.TryGetValue(pkey) with
    | true, existingRemover -> Some existingRemover
    | false, _ -> None

  /// Registers a function to be invoked to remove previously added handlers associated with the specified property key.
  member private this.RegisterHandler<'THandler when 'THandler :> Delegate>
    (pkey: PropKey, handler: 'THandler, removeHandler: 'THandler -> unit)
    =
    trackedHandlers[pkey] <- handler
    handlerRemovalActions[pkey] <- fun () -> removeHandler handler

  member private _.TryGetAction<'TAction>(pkey: PropKey) =
    match currentActions.TryGetValue pkey with
    | true, action -> Some(unbox<'TAction> action)
    | false, _ -> None

  /// Removes the handler associated with the specified property key, if it exists.
  member this.RemoveHandler(pkey: PropKey) =
    match this.TryGetHandlerRemovalAction pkey with
    | Some removeHandler ->
      removeHandler ()
      handlerRemovalActions.Remove pkey |> ignore
      trackedHandlers.Remove pkey |> ignore
      currentActions.Remove pkey |> ignore
    | None -> ()

  member this.SetEventHandler
    (
      pkey: PropKey<'TEventArgs -> unit>,
      event: IEvent<EventHandler<'TEventArgs>, 'TEventArgs>,
      action: 'TEventArgs -> unit
    ) =
    currentActions[pkey.Untyped] <- action

    if not (trackedHandlers.ContainsKey pkey.Untyped) then
      let handler =
        EventHandler<'TEventArgs>(fun _ args ->
          this.TryGetAction<'TEventArgs -> unit>(pkey.Untyped)
          |> Option.iter (fun current -> current args))

      event.AddHandler handler
      this.RegisterHandler(pkey.Untyped, handler, event.RemoveHandler)

  member this.SetEventHandler
    (pkey: PropKey<'TEventArgs -> unit>, event: IEvent<EventHandler, EventArgs>, action: unit -> unit)
    =
    currentActions[pkey.Untyped] <- action

    if not (trackedHandlers.ContainsKey pkey.Untyped) then
      let handler =
        EventHandler(fun _ _ ->
          this.TryGetAction<unit -> unit>(pkey.Untyped)
          |> Option.iter (fun current -> current ()))

      event.AddHandler handler
      this.RegisterHandler(pkey.Untyped, handler, event.RemoveHandler)

  member this.SetEventHandler
    (pkey: PropKey<'TEventArgs -> unit>, event: IEvent<EventHandler, EventArgs>, action: EventArgs -> unit)
    =
    currentActions[pkey.Untyped] <- action

    if not (trackedHandlers.ContainsKey pkey.Untyped) then
      let handler =
        EventHandler(fun _ args ->
          this.TryGetAction<EventArgs -> unit>(pkey.Untyped)
          |> Option.iter (fun current -> current args))

      event.AddHandler handler
      this.RegisterHandler(pkey.Untyped, handler, event.RemoveHandler)

  member this.SetEventHandler
    (
      pkey: PropKey<NotifyCollectionChangedEventArgs -> unit>,
      event: IEvent<NotifyCollectionChangedEventHandler, NotifyCollectionChangedEventArgs>,
      action: NotifyCollectionChangedEventArgs -> unit
    ) =
    currentActions[pkey.Untyped] <- action

    if not (trackedHandlers.ContainsKey pkey.Untyped) then
      let handler =
        NotifyCollectionChangedEventHandler(fun _ args ->
          this.TryGetAction<NotifyCollectionChangedEventArgs -> unit>(pkey.Untyped)
          |> Option.iter (fun current -> current args))

      event.AddHandler handler
      this.RegisterHandler(pkey.Untyped, handler, event.RemoveHandler)

type internal TreeNode =
  { TerminalElement: TerminalElement
    Origin: Origin }

[<AbstractClass>]
type internal ViewBackedTerminalElement(props: Props) =

  /// Go through the tree of TerminalElements and their `Children`, and call the given function on each node.
  let rec traverseTree (nodes: TreeNode list) (traverse: TreeNode -> unit) =

    match nodes with
    | [] -> ()
    | cur :: remainingNodes ->
      let curNode =
        { TerminalElement = cur.TerminalElement
          Origin = cur.Origin }

      traverse curNode

      let childNodes =
        match curNode.TerminalElement with
        | ElmishComponentTE _ -> []
        | ViewTE te ->
          te.Children
          |> Seq.mapi (fun i e ->
            { TerminalElement = e
              Origin = Origin.Child(te, i) })
          |> List.ofSeq

      traverseTree (childNodes @ remainingNodes) traverse

  let mutable view = null

  let mutable disposing = false

  let viewSetEvent = Event<View>()

  member this.View
    with get () = view
    and set value =
      if (view <> null) then
        failwith $"View has already been set."

      view <- value
      viewSetEvent.Trigger value

  member val Origin: Origin = Origin.Root with get, set

  member val ViewSet = viewSetEvent.Publish

  member val EventRegistrar: EventHandlerRegistrar = EventHandlerRegistrar()

  member val Props: Props = props with get, set

  member this.Children: List<TerminalElement> =
    List(this.Props.Children |> Seq.map TerminalElement.from)

  abstract SubElements_PropKeys: PropKey list
  default _.SubElements_PropKeys = []

  abstract NewView: unit -> View

  abstract SetAsChildOfParentView: bool
  default _.SetAsChildOfParentView = true

  member this.InitializeView(application: Terminal.Gui.App.IApplication) =
#if DEBUG
    Diagnostics.Trace.WriteLine $"{this.Name} created!"
#endif
    this.View <- this.NewView()

    this.InitializeSubElements(application)
    |> Seq.iter (fun (k, v) -> this.Props |> Props.add (k, v))

    PositionService.Current.ApplyPos this
    this.SetProps(this, this.Props)

  abstract Name: string

  member this.InitializeTree(origin: Origin, application: Terminal.Gui.App.IApplication) : unit =
    this.Origin <- origin

    let traverse (node: TreeNode) =

      match node.TerminalElement with
      | ViewTE te ->
        te.Origin <- node.Origin
        (te :?> ViewBackedTerminalElement).InitializeView(application)
      | ElmishComponentTE ce ->
        ce.Origin <- node.Origin
        ce.StartElmishLoop(application)

#if DEBUG
      Diagnostics.Trace.WriteLine $"ID: {node.TerminalElement.GetPath()}"
#endif

      // Here, the "children" views are added to their parent.
      match node.TerminalElement with
      | ViewTE te when te.Origin.IsChild ->
        if te.SetAsChildOfParentView then
          match te.Origin with
          | Origin.Child(_, index) ->
            te.Origin
            |> Origin.parentView
            |> Option.iter (fun parent -> parent.AddAt(min index parent.SubViews.Count, te.View) |> ignore)
          | _ -> ()
      | ElmishComponentTE ce when ce.Origin.IsChild ->
        match ce.Origin with
        | Origin.Child(_, index) ->
          ce.Origin
          |> Origin.parentView
          |> Option.iter (fun parent -> parent.AddAt(min index parent.SubViews.Count, ce.View) |> ignore)
        | _ -> ()
      | _ -> ()

    traverseTree
      [ { TerminalElement = TerminalElement.from this
          Origin = origin } ]
      traverse

  /// For each '*.element' prop, initialize the Tree of the element and then return the sub element: (proPKey * View)
  member this.InitializeSubElements(application: Terminal.Gui.App.IApplication) : (PropKey * obj) seq =
    seq {
      for x in this.SubElements_PropKeys do
        match this.Props |> Props.tryFind x with

        | None -> ()

        | Some value ->
          match TerminalElement.from (value :?> IView) with
          | ViewTE viewTe ->
            viewTe.InitializeTree(Origin.SubElement(this, None, x.Key), application)

            let viewKey = PropKey.viewKeyOfSubElement x

            yield viewKey, viewTe.View
          | _ -> failwith "Out of range subElement type"
    }

  member this.TrySetEventHandler<'TEventArgs>
    (k: PropKey<'TEventArgs -> unit>, event: IEvent<EventHandler<'TEventArgs>, 'TEventArgs>)
    =
    this.Props
    |> Props.tryFind k
    |> Option.iter (fun action -> this.EventRegistrar.SetEventHandler(k, event, action))

  member this.TrySetEventHandler(k: PropKey<EventArgs -> unit>, event: IEvent<EventHandler, EventArgs>) =
    this.Props
    |> Props.tryFind k
    |> Option.iter (fun action -> this.EventRegistrar.SetEventHandler(k, event, action))

  member this.TrySetEventHandler
    (
      k: PropKey<NotifyCollectionChangedEventArgs -> unit>,
      event: IEvent<NotifyCollectionChangedEventHandler, NotifyCollectionChangedEventArgs>
    ) =
    this.Props
    |> Props.tryFind k
    |> Option.iter (fun action -> this.EventRegistrar.SetEventHandler(k, event, action))

  abstract SetProps: terminalElement: ViewBackedTerminalElement * props: Props -> unit

  default this.SetProps(terminalElement: ViewBackedTerminalElement, props: Props) = ()

  abstract ClearProp: propertyId: PropertyId -> unit

  default this.ClearProp(propertyId: PropertyId) = ()

  /// Clears a removed declarative property without requiring its previous value. Event keys bypass
  /// generated property dispatch because the registrar already owns their native subscriptions.
  member this.ClearProp(propertyKey: PropKey) =
    if propertyKey.IsEvent then
      this.EventRegistrar.RemoveHandler propertyKey
    else
      this.ClearProp propertyKey.Id

  member this.Dispose() =
    if Interlocked.Exchange(&disposing, true) then
      ()
    else

      // Clear applied properties and remove any event subscriptions. Declarative view-slot
      // specifications are not native properties and are disposed separately below.
      for entry in Props.toEntries this.Props do
        if not entry.Key.IsSubViewSpec then
          this.ClearProp entry.Key

      match this.Origin with
      | Origin.Root
      | Origin.SubElement _ -> ()
      | _ ->
        this.Origin
        |> Origin.parentView
        |> Option.iter (fun v -> v.Remove this.View |> ignore)

      // Dispose SubElements (Represented as `View` typed properties of the View, that are not children)
      for key in this.SubElements_PropKeys do
        this.Props
        |> Props.tryFind key
        |> Option.iter (fun v -> ((v :?> ISimpleViewSpec).CreateViewTE() :> IDisposable).Dispose())

      for child in this.Children do
        child.Dispose()

      PositionService.Current.ExecuteCleanups(this)
      // Finally, dispose the View itself
      this.View.Dispose()

  interface IViewTE with
    member this.InitializeTree(origin, application) =
      this.InitializeTree(origin, application)

    member this.GetPath() =
      this.Origin |> Origin.getPath (this.Name)

    member this.Origin
      with get () = this.Origin
      and set v = this.Origin <- v

    member this.View = this.View
    member this.Name = this.Name

    member this.SetAsChildOfParentView = this.SetAsChildOfParentView

    member this.Children = this.Children

    member this.Props = this.Props

    member this.OnViewSet = this.ViewSet

    member this.Dispose() = this.Dispose()
