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
  let trackedHandlers = Dictionary<IPropKey, Delegate>()

  /// Stores functions to be invoked to remove previously added handlers.
  /// Which will call IEvent.RemoveHandler on the event associated with the property key.
  let handlerRemovalActions = Dictionary<IPropKey, unit -> unit>()

  member private this.TryFindHandler<'THandler when 'THandler :> Delegate> (pkey: IPropKey) =
    match trackedHandlers.TryGetValue(pkey) with
    | true, existingHandler ->
      Some (existingHandler :?> 'THandler)
    | false, _ ->
      None

  member private this.TryGetHandlerRemovalAction (pkey: IPropKey) =
    match handlerRemovalActions.TryGetValue(pkey) with
    | true, existingRemover ->
      Some existingRemover
    | false, _ ->
      None

  /// Registers a function to be invoked to remove previously added handlers associated with the specified property key.
  member private this.RegisterHandlerRemoval<'THandler when 'THandler :> Delegate> (pkey: IPropKey, handler: 'THandler, removeHandler: 'THandler -> unit) =
    handlerRemovalActions[pkey] <-
      fun () ->
        // This will only remove the handler from the event, not from the repositories
        removeHandler handler
        // Note: The actual removal from the repositories is done in `removeHandler` method

  /// Removes the handler associated with the specified property key, if it exists.
  member this.RemoveHandler (pkey: IPropKey) =
    match this.TryGetHandlerRemovalAction pkey with
    | Some removeHandler ->
      removeHandler ()
      handlerRemovalActions.Remove pkey |> ignore
      trackedHandlers.Remove pkey |> ignore
    | None ->
      ()

  member private this.SetHandler<'THandler when 'THandler :> Delegate> (pkey: IPropKey, handler: 'THandler, removeHandler: 'THandler -> unit, addHandler: 'THandler -> unit) =
    match this.TryFindHandler<'THandler> pkey with
    | Some previouslySetHandler ->
      removeHandler previouslySetHandler
    | None ->
      ()

    trackedHandlers[pkey] <- handler
    addHandler handler

  member this.SetEventHandler (pkey: IEventPropKey<'TEventArgs -> unit>, event: IEvent<EventHandler<'TEventArgs>,'TEventArgs>, action: 'TEventArgs -> unit) =
    let handler: EventHandler<'TEventArgs> = EventHandler<'TEventArgs>(fun sender args -> action args)
    this.SetHandler(pkey, handler, event.RemoveHandler, event.AddHandler)
    this.RegisterHandlerRemoval(pkey, handler, event.RemoveHandler)

  member this.SetEventHandler (pkey: IEventPropKey<'TEventArgs -> unit>, event: IEvent<EventHandler,EventArgs>, action: unit -> unit) =
    let handler: EventHandler = EventHandler(fun sender args -> action ())
    this.SetHandler(pkey, handler, event.RemoveHandler, event.AddHandler)
    this.RegisterHandlerRemoval(pkey, handler, event.RemoveHandler)

  member this.SetEventHandler (pkey: IEventPropKey<'TEventArgs -> unit>, event: IEvent<EventHandler,EventArgs>, action: EventArgs -> unit) =
    let handler: EventHandler = EventHandler(fun sender args -> action args)
    this.SetHandler(pkey, handler, event.RemoveHandler, event.AddHandler)
    this.RegisterHandlerRemoval(pkey, handler, event.RemoveHandler)

  member this.SetEventHandler (pkey: IEventPropKey<NotifyCollectionChangedEventArgs -> unit>, event: IEvent<NotifyCollectionChangedEventHandler,NotifyCollectionChangedEventArgs>, action: NotifyCollectionChangedEventArgs -> unit) =
    let handler: NotifyCollectionChangedEventHandler = NotifyCollectionChangedEventHandler(fun sender args -> action args)
    this.SetHandler(pkey, handler, event.RemoveHandler, event.AddHandler)
    this.RegisterHandlerRemoval(pkey, handler, event.RemoveHandler)

type internal TreeNode = {
  TerminalElement: TerminalElement
  Origin: Origin
}

[<AbstractClass>]
type internal ViewBackedTerminalElement(props: Props) =

  /// Go through the tree of TerminalElements and their `Children`, and call the given function on each node.
  let rec traverseTree (nodes: TreeNode list) (traverse: TreeNode -> unit) =

    match nodes with
    | [] -> ()
    | cur :: remainingNodes ->
      let curNode = {
        TerminalElement = cur.TerminalElement
        Origin = cur.Origin
      }

      traverse curNode

      let childNodes =
        match curNode.TerminalElement with
        | ElmishComponentTE _ -> []
        | ViewBackedTE te ->
          te.Children
          |> Seq.mapi (fun i e -> {
            TerminalElement = e
            Origin = Origin.Child (te, i)
          })
          |> List.ofSeq

      traverseTree (childNodes @ remainingNodes) traverse

  let mutable view = null

  let mutable disposing = false

  let viewSetEvent = Event<View>()

  member val ViewReusedByAnotherTE = false with get, set

  member this.View
    with get() = view
    and set value =
      if (view <> null) then
        failwith $"View has already been set."
      view <- value
      viewSetEvent.Trigger value

  member val Origin: Origin = Origin.Root with get, set

  member val ViewSet = viewSetEvent.Publish

  member val EventRegistrar: EventHandlerRegistrar = EventHandlerRegistrar() with get, set

  member val Props: Props = props with get, set

  member this.Children
    with get() : List<TerminalElement> =
      props
      |> Props.tryFind PKey.View.children
      |> Option.defaultValue (List<TerminalElement>())
    and set value =
      match props.tryFind PKey.View.children with
      | Some _ -> failwith "Children property has already been set."
      | None -> props.add(PKey.View.children, value)

  abstract SubElements_PropKeys: SubElementPropKey<IViewTE> list
  default _.SubElements_PropKeys = []

  abstract NewView: unit -> View

  abstract SetAsChildOfParentView: bool
  default _.SetAsChildOfParentView = true

  member this.InitializeView() =
#if DEBUG
    Diagnostics.Trace.WriteLine $"{this.Name} created!"
#endif
    this.View <- this.NewView ()

    this.InitializeSubElements()
    |> Seq.iter this.Props.addNonTyped

    this.SetProps (this, this.Props)

  abstract Reuse: prev: IViewTE -> unit

  abstract Name: string

  member this.InitializeTree(origin: Origin) : unit =
    this.Origin <- origin

    let traverse (node: TreeNode) =

      match node.TerminalElement with
      | ViewBackedTE te ->
        te.Origin <- node.Origin
        (te :?> ViewBackedTerminalElement).InitializeView ()
      | ElmishComponentTE ce ->
        ce.Origin <- node.Origin
        ce.StartElmishLoop ()

      #if DEBUG
      Diagnostics.Trace.WriteLine $"ID: {node.TerminalElement.GetPath()}"
      #endif

      // Here, the "children" views are added to their parent
      match node.TerminalElement with
      | ElmishComponentTE _ -> ()
      | ViewBackedTE te ->
        if te.SetAsChildOfParentView then
          te.Origin.ParentView
          |> Option.iter (fun v -> v.Add te.View |> ignore)

    traverseTree
      [
        {
          TerminalElement = TerminalElement.from this
          Origin = origin
        }
      ]
      traverse

  /// For each '*.element' prop, initialize the Tree of the element and then return the sub element: (proPKey * View)
  member this.InitializeSubElements () : (IPropKey * obj) seq =
    seq {
      for x in this.SubElements_PropKeys do
        match this.Props |> Props.tryFindByRawKey<obj> x with

        | None -> ()

        | Some value ->
          match value with
          | :? ViewBackedTerminalElement as subElement ->
            subElement.InitializeTree (Origin.SubElement (this, None, x))

            let viewKey = x.viewKey

            yield viewKey, subElement.View
          | :? List<IViewTE> as elements ->
            elements
            |> Seq.iteri (fun i e -> e.InitializeTree (Origin.SubElement (this, Some i, x)))

            let viewKey = x.viewKey

            let views =
              elements |> Seq.map _.View |> Seq.toList

            yield viewKey, views
          | _ -> failwith "Out of range subElement type"
    }

  member this.TrySetEventHandler<'TEventArgs> (k: IEventPropKey<'TEventArgs -> unit>, event: IEvent<EventHandler<'TEventArgs>,'TEventArgs>) =

    this.TryRemoveEventHandler k

    this.Props.tryFind k
    |> Option.iter (fun action -> this.EventRegistrar.SetEventHandler(k, event, action))

  member this.TrySetEventHandler (k: IEventPropKey<EventArgs -> unit>, event: IEvent<EventHandler,EventArgs>) =

    this.TryRemoveEventHandler k

    this.Props.tryFind k
    |> Option.iter (fun action -> this.EventRegistrar.SetEventHandler(k, event, action))

  member this.TrySetEventHandler (k: IEventPropKey<NotifyCollectionChangedEventArgs -> unit>, event: IEvent<NotifyCollectionChangedEventHandler,NotifyCollectionChangedEventArgs>) =

    this.TryRemoveEventHandler k

    this.Props.tryFind k
    |> Option.iter (fun action -> this.EventRegistrar.SetEventHandler(k, event, action))

  member this.TryRemoveEventHandler (k: IPropKey) =
    this.EventRegistrar.RemoveHandler k

  abstract SetProps: terminalElement: ViewBackedTerminalElement * props: Props -> unit

  default this.SetProps (terminalElement: ViewBackedTerminalElement, props: Props) =
    // Custom Props
    props
    |> Props.tryFind PKey.View.X_delayedPos
    |> Option.iter (fun tPos -> PositionService.Current.ApplyPos(terminalElement, X, tPos))

    props
    |> Props.tryFind PKey.View.Y_delayedPos
    |> Option.iter (fun tPos -> PositionService.Current.ApplyPos(terminalElement, Y, tPos))

  // TODO: Is the view needed as param ? is the props needed as param ?
  abstract RemoveProps: terminalElement: ViewBackedTerminalElement * props: Props -> unit

  default this.RemoveProps (terminalElement: ViewBackedTerminalElement, props: Props) =
    ()

  /// Reuses:
  /// // TODO: outdated documentation
  /// - Previous `View`, while updating its properties to match the current TerminalElement properties.
  /// - But also other Views that are sub elements of the previous `ITerminalElement` and made available in the `prevProps`.
  override this.Reuse prev =

    let prev = prev :?> ViewBackedTerminalElement

    prev.ViewReusedByAnotherTE <- true
    PositionService.Current.ExecuteCleanups prev

    this.View <- prev.View
    this.EventRegistrar <- prev.EventRegistrar

    PositionService.Current.ApplyPos this

    // TODO: it seems that comparing x_delayedPos/y_delayedPos is working well
    // TODO: this should be tested and documented to make sure that it continues to work well in the future.

    // TODO: Should refactor props to be clear that X and Y are treated separately
    let c = ViewBackedTerminalElement.compare prev.Props this.Props

    // 0 - foreach unchanged _element property, we identify the _view to reinject to `this` TerminalElement
    let view_PropKeys_ToReinject =
      c.unchangedProps
      |> Props.filterSingleElementKeys
      |> Seq.map _.viewKey
      |> Seq.toArray

    // 1 - then we get these Views missing in `this` TerminalElement.
    let view_Props_ToReinject, removedProps =
      c.removedProps
      |> Props.partition (fun kv -> view_PropKeys_ToReinject |> Array.contains kv.Key)

    // 2 - And we add them.
    view_Props_ToReinject
    |> Props.iter (fun kv -> this.Props.addNonTyped (kv.Key, kv.Value))

    this.RemoveProps (this, removedProps)
    this.SetProps (this, c.changedProps)

  member this.equivalentTo(other: ViewBackedTerminalElement) =
    let mutable isEquivalent = true

    let mutable enumerator =
      this.Props.dict.GetEnumerator()

    while isEquivalent && enumerator.MoveNext() do
      let kv = enumerator.Current

      if kv.Key.key = "children" then // TODO: for now children comparison is not yet implemented
        ()
      elif kv.Key.isViewKey then
        ()
      elif kv.Key.isSingleElementKey then
        let curElement =
          kv.Value :?> ViewBackedTerminalElement

        let otherElement =
          other.Props
          |> Props.tryFindByRawKey kv.Key
          |> Option.map (fun (x: obj) -> x :?> ViewBackedTerminalElement)

        match curElement, otherElement with
        | curValue, Some otherValue when (curValue.equivalentTo otherValue) -> ()
        | _, _ -> isEquivalent <- false
      else
        let curElement = kv.Value

        let otherElement =
          other.Props |> Props.tryFindByRawKey kv.Key

        isEquivalent <- curElement = otherElement

    isEquivalent

  static member compare
    (prevProps: Props)
    (curProps: Props)
    : {|
        changedProps: Props
        unchangedProps: Props
        removedProps: Props
      |}
    =

    let remainingOldProps, removedProps =
      prevProps
      |> Props.partition (fun kv -> curProps |> Props.rawKeyExists kv.Key)

    let unchangedProps, changedProps =
      curProps
      |> Props.partition (fun kv ->
        match remainingOldProps |> Props.tryFindByRawKey kv.Key with
        | _ when kv.Key.key = "children" -> // Here we always consider the 'children' unchanged
          true
        | Some(v: obj) when kv.Key.isSingleElementKey ->
          let curElement =
            kv.Value :?> ViewBackedTerminalElement

          let oldElement = v :?> ViewBackedTerminalElement
          curElement.equivalentTo oldElement
        // TODO: comparison is not good here, it can fail for many C# types
        // TODO: Properties values should be comparable
        // TODO: should also be able to compare _element props
        | Some v' when kv.Value = v' -> true
        | _ -> false
      )

    {|
      changedProps = changedProps
      unchangedProps = unchangedProps
      removedProps = removedProps
    |}


  member this.Dispose() =
    if Interlocked.Exchange(&disposing, true) then
      ()
    elif (not this.ViewReusedByAnotherTE) then

      // Remove any event subscriptions
      this.RemoveProps (this, this.Props)

      this.Origin.ParentView |> Option.iter (fun v -> v.Remove this.View |> ignore)

      // Dispose SubElements (Represented as `View` typed properties of the View, that are not children)
      for key in this.SubElements_PropKeys do
        this.Props
        |> Props.tryFind key
        |> Option.iter _.Dispose()

      for child in this.Children do
        child.Dispose()

      PositionService.Current.ExecuteCleanups(this)
      // Finally, dispose the View itself
      this.View.Dispose()

  interface IViewTE with
    member this.InitializeTree origin  = this.InitializeTree origin
    member this.Reuse prevElementData = this.Reuse prevElementData
    member this.GetPath() = this.Origin.GetPath(this.Name)
    member this.Origin with get () = this.Origin and set v = this.Origin <- v
    member this.View = this.View
    member this.Name = this.Name

    member this.SetAsChildOfParentView =
      this.SetAsChildOfParentView

    member this.Children = this.Children

    member this.Props = this.Props

    member this.OnViewSet = this.ViewSet

    member this.Dispose() = this.Dispose()

