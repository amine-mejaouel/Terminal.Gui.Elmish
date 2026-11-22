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

  /// Stores the handlers for each property key.
  let trackedHandlers = Dictionary<PropKey, Delegate>()

  /// Stores functions to be invoked to remove previously added handlers.
  /// Which will call IEvent.RemoveHandler on the event associated with the property key.
  let handlerRemovalActions = Dictionary<PropKey, unit -> unit>()

  member private this.TryFindHandler<'THandler when 'THandler :> Delegate>(pkey: PropKey) =
    match trackedHandlers.TryGetValue(pkey) with
    | true, existingHandler -> Some(existingHandler :?> 'THandler)
    | false, _ -> None

  member private this.TryGetHandlerRemovalAction(pkey: PropKey) =
    match handlerRemovalActions.TryGetValue(pkey) with
    | true, existingRemover -> Some existingRemover
    | false, _ -> None

  /// Registers a function to be invoked to remove previously added handlers associated with the specified property key.
  member private this.RegisterHandlerRemoval<'THandler when 'THandler :> Delegate>
    (pkey: PropKey, handler: 'THandler, removeHandler: 'THandler -> unit)
    =
    handlerRemovalActions[pkey] <-
      fun () ->
        // This will only remove the handler from the event, not from the repositories
        removeHandler handler
  // Note: The actual removal from the repositories is done in `removeHandler` method

  /// Removes the handler associated with the specified property key, if it exists.
  member this.RemoveHandler(pkey: PropKey) =
    match this.TryGetHandlerRemovalAction pkey with
    | Some removeHandler ->
      removeHandler ()
      handlerRemovalActions.Remove pkey |> ignore
      trackedHandlers.Remove pkey |> ignore
    | None -> ()

  member private this.SetHandler<'THandler when 'THandler :> Delegate>
    (pkey: PropKey, handler: 'THandler, removeHandler: 'THandler -> unit, addHandler: 'THandler -> unit)
    =
    match this.TryFindHandler<'THandler> pkey with
    | Some previouslySetHandler -> removeHandler previouslySetHandler
    | None -> ()

    trackedHandlers[pkey] <- handler
    addHandler handler

  member this.SetEventHandler
    (
      pkey: PropKey<'TEventArgs -> unit>,
      event: IEvent<EventHandler<'TEventArgs>, 'TEventArgs>,
      action: 'TEventArgs -> unit
    ) =
    let handler: EventHandler<'TEventArgs> =
      EventHandler<'TEventArgs>(fun sender args -> action args)

    this.SetHandler(pkey.Untyped, handler, event.RemoveHandler, event.AddHandler)
    this.RegisterHandlerRemoval(pkey.Untyped, handler, event.RemoveHandler)

  member this.SetEventHandler
    (pkey: PropKey<'TEventArgs -> unit>, event: IEvent<EventHandler, EventArgs>, action: unit -> unit)
    =
    let handler: EventHandler = EventHandler(fun sender args -> action ())
    this.SetHandler(pkey.Untyped, handler, event.RemoveHandler, event.AddHandler)
    this.RegisterHandlerRemoval(pkey.Untyped, handler, event.RemoveHandler)

  member this.SetEventHandler
    (pkey: PropKey<'TEventArgs -> unit>, event: IEvent<EventHandler, EventArgs>, action: EventArgs -> unit)
    =
    let handler: EventHandler = EventHandler(fun sender args -> action args)
    this.SetHandler(pkey.Untyped, handler, event.RemoveHandler, event.AddHandler)
    this.RegisterHandlerRemoval(pkey.Untyped, handler, event.RemoveHandler)

  member this.SetEventHandler
    (
      pkey: PropKey<NotifyCollectionChangedEventArgs -> unit>,
      event: IEvent<NotifyCollectionChangedEventHandler, NotifyCollectionChangedEventArgs>,
      action: NotifyCollectionChangedEventArgs -> unit
    ) =
    let handler: NotifyCollectionChangedEventHandler =
      NotifyCollectionChangedEventHandler(fun sender args -> action args)

    this.SetHandler(pkey.Untyped, handler, event.RemoveHandler, event.AddHandler)
    this.RegisterHandlerRemoval(pkey.Untyped, handler, event.RemoveHandler)

/// Standalone event handler helpers that work with any (EventHandlerRegistrar, Props) pair.
/// Used by both ViewBackedTerminalElement (existing path) and DomNode applicators (new path).
type internal EventHandlerHelpers =

  static member TrySetEventHandler<'TEventArgs>
    (
      registrar: EventHandlerRegistrar,
      props: Props,
      k: PropKey<'TEventArgs -> unit>,
      event: IEvent<EventHandler<'TEventArgs>, 'TEventArgs>
    ) =
    registrar.RemoveHandler k.Untyped

    props
    |> Props.tryFind k
    |> Option.iter (fun action -> registrar.SetEventHandler(k, event, action))

  static member TrySetEventHandler
    (
      registrar: EventHandlerRegistrar,
      props: Props,
      k: PropKey<EventArgs -> unit>,
      event: IEvent<EventHandler, EventArgs>
    ) =
    registrar.RemoveHandler k.Untyped

    props
    |> Props.tryFind k
    |> Option.iter (fun action -> registrar.SetEventHandler(k, event, action))

  static member TrySetEventHandler
    (
      registrar: EventHandlerRegistrar,
      props: Props,
      k: PropKey<NotifyCollectionChangedEventArgs -> unit>,
      event: IEvent<NotifyCollectionChangedEventHandler, NotifyCollectionChangedEventArgs>
    ) =
    registrar.RemoveHandler k.Untyped

    props
    |> Props.tryFind k
    |> Option.iter (fun action -> registrar.SetEventHandler(k, event, action))

  static member TryRemoveEventHandler(registrar: EventHandlerRegistrar, k: PropKey<_>) =
    registrar.RemoveHandler k.Untyped

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

  member val EventRegistrar: EventHandlerRegistrar = EventHandlerRegistrar() with get, set

  member val Props: Props = props with get, set

  member this.Children: List<TerminalElement> = props.Children

  abstract SubElements_PropKeys: RawPropKey list
  default _.SubElements_PropKeys = []

  abstract NewView: unit -> View

  abstract SetAsChildOfParentView: bool
  default _.SetAsChildOfParentView = true

  abstract Name: string

  member this.TrySetEventHandler<'TEventArgs>
    (k: PropKey<'TEventArgs -> unit>, event: IEvent<EventHandler<'TEventArgs>, 'TEventArgs>)
    =
    EventHandlerHelpers.TrySetEventHandler(this.EventRegistrar, this.Props, k, event)

  member this.TrySetEventHandler(k: PropKey<EventArgs -> unit>, event: IEvent<EventHandler, EventArgs>) =
    EventHandlerHelpers.TrySetEventHandler(this.EventRegistrar, this.Props, k, event)

  member this.TrySetEventHandler
    (
      k: PropKey<NotifyCollectionChangedEventArgs -> unit>,
      event: IEvent<NotifyCollectionChangedEventHandler, NotifyCollectionChangedEventArgs>
    ) =
    EventHandlerHelpers.TrySetEventHandler(this.EventRegistrar, this.Props, k, event)

  member this.TryRemoveEventHandler(k: PropKey<_>) =
    EventHandlerHelpers.TryRemoveEventHandler(this.EventRegistrar, k)

  abstract SetProps: terminalElement: ViewBackedTerminalElement * props: Props -> unit

  default this.SetProps(terminalElement: ViewBackedTerminalElement, props: Props) = ()

  abstract RemoveProps: terminalElement: ViewBackedTerminalElement * props: Props -> unit

  default this.RemoveProps(terminalElement: ViewBackedTerminalElement, props: Props) = ()

  member this.equivalentTo(other: ViewBackedTerminalElement) =
    let mutable isEquivalent = true

    this.Props
    |> Props.iter (fun kv ->
      if isEquivalent then
        if kv.Key.Key = "children" then // TODO: for now children comparison is not yet implemented
          ()
        elif kv.Key.Kind = PropKeyKind.View then
          ()
        elif kv.Key.Kind = PropKeyKind.SubElement then
          let curElement = kv.Value :?> ViewBackedTerminalElement

          let otherElement =
            other.Props
            |> Props.tryFind kv.Key
            |> Option.map (fun (x: obj) -> x :?> ViewBackedTerminalElement)

          match curElement, otherElement with
          | curValue, Some otherValue when (curValue.equivalentTo otherValue) -> ()
          | _, _ -> isEquivalent <- false
        else
          let curElement = kv.Value

          match other.Props |> Props.tryFind kv.Key with
          | Some otherElement -> isEquivalent <- curElement = otherElement
          | None -> isEquivalent <- false)

    isEquivalent

  static member compare
    (prevProps: Props)
    (curProps: Props)
    : {| changedProps: Props
         unchangedProps: Props
         removedProps: Props |}
    =

    let remainingOldProps, removedProps =
      prevProps |> Props.partition (fun kv -> curProps |> Props.rawKeyExists kv.Key)

    let unchangedProps, changedProps =
      curProps
      |> Props.partition (fun kv ->
        match remainingOldProps |> Props.tryFind kv.Key with
        | _ when kv.Key.Key = "children" -> // Here we always consider the 'children' unchanged
          true
        | Some(v: obj) when kv.Key.Kind = PropKeyKind.SubElement ->
          let curElement = kv.Value :?> ViewBackedTerminalElement

          let oldElement = v :?> ViewBackedTerminalElement
          curElement.equivalentTo oldElement
        // TODO: comparison is not good here, it can fail for many C# types
        // TODO: Properties values should be comparable
        // TODO: should also be able to compare _element props
        | Some v' when kv.Value = v' -> true
        | _ -> false)

    {| changedProps = changedProps
       unchangedProps = unchangedProps
       removedProps = removedProps |}


  /// A ViewBackedTerminalElement is now a pure description; it owns no View, so disposal is a no-op.
  /// (DomNode owns the actual View; see plan-staticDom.md, decision A for the rest of the cleanup.)
  member this.Dispose() = ()

  interface IViewTE with
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

    member this.Dispose() = ()
