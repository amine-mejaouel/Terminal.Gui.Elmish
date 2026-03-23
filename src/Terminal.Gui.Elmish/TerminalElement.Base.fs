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

type internal CurrentTreeNode = TE
type internal ParentTreeNode = IViewTE

[<AbstractClass>]
type internal ViewBackedTerminalElement(props: Props) =

  /// <p>Depth-first traversal of a TerminalElement tree.</p>
  /// <p>Applies the provided <c>traverse</c> function to <c>ViewTE</c> and <c>ElmishComponentTE</c> nodes.</p>
  /// <p>But does not recurse into the children of <c>ElmishComponentTE</c> nodes, as they are expected to manage their own tree.</p>
  static let rec traverseTEs (head: TE * Address) (traverse: CurrentTreeNode -> Address -> unit) : unit =

    let rec traverseViewTEs (nodes: TE list) (origin: Address) (traverse: CurrentTreeNode -> Address -> unit) =
      match nodes with
      | [] -> ()
      | current :: remainingNodes ->

        traverse current origin

        match current with
        | ElmishComponentTE _ -> ()
        | ViewTE viewTe ->
          viewTe.Children
          |> Seq.mapi (fun i child -> child, origin @ [ Child(i, child.IsElmishComponentTE) ])
          |> Seq.iter (fun (child, childOrigin) -> traverseViewTEs [ child ] childOrigin traverse)

        traverseViewTEs remainingNodes origin traverse

    let headElement, headOrigin = head
    traverseViewTEs [ headElement ] headOrigin traverse

  let mutable view = null

  let mutable disposing = false

  let viewSetEvent = Event<View>()

  member val ViewReusedByAnotherTE = false with get, set

  member this.View
    with get () = view
    and set value =
      if (view <> null) then
        failwith $"View has already been set."

      view <- value
      viewSetEvent.Trigger value

  member val Origin: Address = [ AddressSegment.Root ] with get, set

  member val ViewSet = viewSetEvent.Publish

  member val EventRegistrar: EventHandlerRegistrar = EventHandlerRegistrar() with get, set

  member val Props: Props = props with get, set

  member this.Children: List<TE> = props.Children

  abstract SubElements_PropKeys: RawPropKey list
  default _.SubElements_PropKeys = []

  abstract NewView: unit -> View

  abstract SetAsChildOfParentView: bool
  default _.SetAsChildOfParentView = true

  static member InitializeView(te: ViewBackedTerminalElement, vtt: IVirtualTerminalTree, address: Address) =
    te.View <- te.NewView()

    // Add this view to the VTT before initializing sub-elements,
    // so sub-elements can find their parent node in the tree.
    vtt.AddView(VttNode.fromViewTE (te, address))

    te.InitializeSubElements(vtt)
    |> Seq.iter (fun (k, v) -> te.Props |> Props.add (k, v))

    PositionService.Current.ApplyPos te
    te.SetProps(te, te.Props)

  abstract Reuse: prev: IViewTE -> unit

  abstract Name: string

  static member InitializeTree terminalElement (address: Address) (vtt: IVirtualTerminalTree) : unit =

    let traverse (cur: CurrentTreeNode) (address: Address) =

      // TODO: address / parentView should be passed as parameters instead
      cur.Address <- address

      match cur with
      | ViewTE te -> ViewBackedTerminalElement.InitializeView((te :?> ViewBackedTerminalElement), vtt, address)
      | ElmishComponentTE ce -> ce.StartElmishLoop(vtt, address)

    traverseTEs ((TE.from terminalElement), address) traverse

  // TODO: InitializeSubElements does not support elmish components as sub elements.
  /// For each '*.element' prop, initialize the Tree of the element and then return the sub element: (proPKey * View)
  member this.InitializeSubElements(vtt) : (PropKey * obj) seq =
    seq {
      for x in this.SubElements_PropKeys do
        match this.Props |> Props.tryFind (PropKeyKind.SubElement, x) with

        | None -> ()

        | Some value ->
          match value with
          | :? ViewBackedTerminalElement as subElement ->
            ViewBackedTerminalElement.InitializeTree subElement (this.Origin @ [ SubElement(None, x, false) ]) vtt

            let viewKey = PropKey.viewKeyOfSubElement x

            yield viewKey, subElement.View
          | :? List<IViewTE> as elements ->
            elements
            |> Seq.iteri (fun i e -> e.InitializeTree (this.Origin @ [ SubElement(Some i, x, false) ]) vtt)

            let viewKey = PropKey.viewKeyOfSubElement x

            let views = elements |> Seq.map _.View |> Seq.toList

            yield viewKey, views
          | _ -> failwith "Out of range subElement type"
    }

  member this.TrySetEventHandler<'TEventArgs>
    (k: PropKey<'TEventArgs -> unit>, event: IEvent<EventHandler<'TEventArgs>, 'TEventArgs>)
    =

    this.TryRemoveEventHandler k.Untyped

    this.Props
    |> Props.tryFind k
    |> Option.iter (fun action -> this.EventRegistrar.SetEventHandler(k, event, action))

  member this.TrySetEventHandler(k: PropKey<EventArgs -> unit>, event: IEvent<EventHandler, EventArgs>) =

    this.TryRemoveEventHandler k.Untyped

    this.Props
    |> Props.tryFind k
    |> Option.iter (fun action -> this.EventRegistrar.SetEventHandler(k, event, action))

  member this.TrySetEventHandler
    (
      k: PropKey<NotifyCollectionChangedEventArgs -> unit>,
      event: IEvent<NotifyCollectionChangedEventHandler, NotifyCollectionChangedEventArgs>
    ) =

    this.TryRemoveEventHandler k.Untyped

    this.Props
    |> Props.tryFind k
    |> Option.iter (fun action -> this.EventRegistrar.SetEventHandler(k, event, action))

  member this.TryRemoveEventHandler(k: PropKey<_>) =
    this.EventRegistrar.RemoveHandler k.Untyped

  member private this.TryRemoveEventHandler(k: PropKey) = this.EventRegistrar.RemoveHandler k

  abstract SetProps: terminalElement: ViewBackedTerminalElement * props: Props -> unit

  default this.SetProps(terminalElement: ViewBackedTerminalElement, props: Props) = ()

  abstract RemoveProps: terminalElement: ViewBackedTerminalElement * props: Props -> unit

  default this.RemoveProps(terminalElement: ViewBackedTerminalElement, props: Props) = ()

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
    this.Origin <- prev.Origin

    PositionService.Current.ApplyPos this

    // TODO: it seems that comparing x_delayedPos/y_delayedPos is working well
    // TODO: this should be tested and documented to make sure that it continues to work well in the future.

    // TODO: Should refactor props to be clear that X and Y are treated separately
    let c = ViewBackedTerminalElement.compare prev.Props this.Props

    // 0 - foreach unchanged _element property, we identify the _view to reinject to `this` TerminalElement
    let view_PropKeys_ToReinject =
      c.unchangedProps
      |> Props.filterSubElementKeys
      |> Seq.map _.viewKey
      |> Seq.toArray

    // 1 - then we get these Views missing in `this` TerminalElement.
    let view_Props_ToReinject, removedProps =
      c.removedProps
      |> Props.partition (fun kv -> view_PropKeys_ToReinject |> Array.contains kv.Key)

    // 2 - And we add them.
    view_Props_ToReinject
    |> Props.iter (fun kv -> this.Props |> Props.add (kv.Key, kv.Value))

    this.RemoveProps(this, removedProps)
    this.SetProps(this, c.changedProps)

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

          let otherElement = other.Props |> Props.tryFind kv.Key

          isEquivalent <- curElement = otherElement)

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


  member this.Dispose() =
    if Interlocked.Exchange(&disposing, true) then
      ()
    elif (not this.ViewReusedByAnotherTE) then

      // Remove any event subscriptions
      this.RemoveProps(this, this.Props)

      // TODO: should confirm if SuperView is the parent view of the current view.
      // TODO: because I added this code without testing it.
      this.View.SuperView.Remove this.View |> ignore

      // Dispose SubElements (Represented as `View` typed properties of the View, that are not children)
      for key in this.SubElements_PropKeys do
        this.Props
        |> Props.tryFind<IDisposable> (PropKeyKind.SubElement, key)
        |> Option.iter _.Dispose()

      for child in this.Children do
        child.Dispose()

      PositionService.Current.ExecuteCleanups(this)
      // Finally, dispose the View itself
      this.View.Dispose()

      // Clear references to help GC
      view <- Unchecked.defaultof<_>

  interface IViewTE with
    member this.InitializeTree address vtt =
      ViewBackedTerminalElement.InitializeTree this address vtt

    member this.Reuse prevElementData = this.Reuse prevElementData

    member this.Address
      with get () = this.Origin
      and set v = this.Origin <- v

    member this.View = this.View
    member this.Name = this.Name

    member this.SetAsChildOfParentView = this.SetAsChildOfParentView

    member this.Children = this.Children

    member this.Props = this.Props

    member this.OnViewSet = this.ViewSet

    member this.Dispose() = this.Dispose()
