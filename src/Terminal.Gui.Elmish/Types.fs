namespace Terminal.Gui.Elmish

open System
open System.Collections.Generic
open System.Collections.Specialized
open Terminal.Gui.ViewBase

type ITerminalElement = interface end

[<RequireQualifiedAccess>]
type TPos =
  | X of ITerminalElement
  | Y of ITerminalElement
  | Top of ITerminalElement
  | Bottom of ITerminalElement
  | Left of ITerminalElement
  | Right of ITerminalElement
  | Absolute of position: int
  | AnchorEnd of offset: int option
  | Center
  | Percent of percent: int
  | Func of func: (View -> int) * view: ITerminalElement
  | Align of alignment: Alignment * modes: AlignmentModes * groupId: int option

[<AutoOpen>]
module internal PropKey =

  [<RequireQualifiedAccess>]
  type PropKeyKind =
    | Simple
    | View
    | Event
    | SubElement

  type RawPropKey = string

  type internal IRawPropKey =
    abstract RawKey: RawPropKey

  let private equalsByRawKey (rawKey: RawPropKey) (obj: obj) =
    match obj with
    | :? IRawPropKey as other -> rawKey = other.RawKey
    | _ -> false

  [<CustomEquality; NoComparison>]
  type PropKey =
    { Kind: PropKeyKind
      Key: RawPropKey }

    member this.viewKey =
      match this.Kind with
      | PropKeyKind.SubElement ->
        { Kind = PropKeyKind.View
          Key = this.Key.Replace("_element", "_view") }
      | _ -> failwith $"viewKey is only valid for SubElement PropKeys, got: {this}"

    override this.Equals(obj) = equalsByRawKey this.Key obj

    override this.GetHashCode() = this.Key.GetHashCode()

    interface IRawPropKey with
      member this.RawKey = this.Key

  [<CustomEquality; NoComparison>]
  type PropKey<'a> =
    private
    | PropKey of PropKey

    member this.Untyped = let (PropKey k) = this in k
    member this.key = this.Untyped.Key
    override this.Equals(obj) = equalsByRawKey this.Untyped.Key obj
    override this.GetHashCode() = this.Untyped.GetHashCode()

    interface IRawPropKey with
      member this.RawKey = this.Untyped.Key

  [<RequireQualifiedAccess>]
  module PropKey =

    let viewKeyOfSubElement (key: RawPropKey) : PropKey =
      { Kind = PropKeyKind.View
        Key = key.Replace("_element", "_view") }

    type Create =
      static member subElement<'a>(key: string) : PropKey<'a> =
        if key.EndsWith "_element" then
          PropKey
            { Kind = PropKeyKind.SubElement
              Key = key }
        else
          failwith $"Invalid key: {key}"

      static member simple<'a>(key: string) : PropKey<'a> =
        if key.EndsWith "_element" || key.EndsWith "_view" then
          failwith $"Invalid key: {key}"
        else
          PropKey { Kind = PropKeyKind.Simple; Key = key }

      static member event<'a>(key: string) : PropKey<'a> =
        if not (key.EndsWith "_event") then
          failwith $"Invalid key: {key}"
        else
          PropKey { Kind = PropKeyKind.Event; Key = key }

      static member view<'a>(key: string) : PropKey<'a> =
        if not (key.EndsWith "_view") then
          failwith $"Invalid key: {key}"
        else
          PropKey { Kind = PropKeyKind.View; Key = key }

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

/// Props object that is still under construction
type internal Props() =
  member val Children: List<TerminalElement> = List<_>() with get
  member val X: Pos option = None with get, set
  member val Y: Pos option = None with get, set
  member val XDelayed: TPos option = None with get, set
  member val YDelayed: TPos option = None with get, set
  /// Include all other properties that are not present as explicit members of Props.
  member val Props = Dictionary<PropKeyKind, Dictionary<RawPropKey, obj>>() with get

and internal ITerminalElementBase =
  inherit ITerminalElement
  inherit IDisposable
  abstract Name: string

and internal IViewTE =
  inherit ITerminalElementBase

  abstract Props: Props with get
  abstract SetAsChildOfParentView: bool
  abstract Children: List<TerminalElement>

/// <summary>
/// An Elmish component is a reusable piece of UI that contains its own Elmish loop.
/// It can be used to create complex, self-contained components that manage their own state and logic.
/// </summary>
/// <remarks>
/// <p>For convenience, the child view of an Elmish component is exposed as a property of the component itself,
/// so that it can be easily accessed and manipulated by parent views.</p>
/// <p>This also allows the component to be used in the same way as a regular view in the tree,
/// without requiring special handling for its child view.</p>
/// </remarks>
and internal IElmishComponentTE =
  inherit ITerminalElementBase

  /// The component's root View (owned by the component's own DomNode subtree).
  abstract View: View with get
  /// The component's placement within the surrounding DomNode tree.
  abstract DomOrigin: DomOrigin with get, set
  abstract StartElmishLoop: unit -> unit

and internal TerminalElement =
  | ViewTE of IViewTE
  | ElmishComponentTE of IElmishComponentTE

  static member from(te: ITerminalElement) =
    match te with
    | :? IViewTE as viewTE -> ViewTE viewTE
    | :? IElmishComponentTE as elmishComponentTE -> ElmishComponentTE elmishComponentTE
    | _ -> failwith "Invalid terminal element"

  member internal this.TerminalElementBase =
    match this with
    | ViewTE viewTE -> viewTE :> ITerminalElementBase
    | ElmishComponentTE elmishComponentTE -> elmishComponentTE :> ITerminalElementBase

  member this.Name = this.TerminalElementBase.Name

  member this.Dispose() = this.TerminalElementBase.Dispose()

  interface ITerminalElementBase with
    member this.Name = this.Name
    member this.Dispose() = this.Dispose()

/// Describes the position of a DomNode within the tree.
and internal DomOrigin =
  | DomRoot
  | DomElmishComponent of parent: IElmishComponentTE
  | DomChild of parent: DomNode * index: int
  | DomSubElement of parent: DomNode * index: int option * property: RawPropKey

/// A child of a DomNode — either a view-based DomNode or an Elmish component.
and internal DomChild =
  | ViewDomChild of DomNode
  | ElmishComponentDomChild of IElmishComponentTE

/// Persistent DOM node that owns a Terminal.Gui View.
/// Created from a TerminalElement description and mutated in place during diffs.
and [<AllowNullLiteral>] internal DomNode
  (
    name: string,
    view: View,
    appliedProps: Props,
    applyProps: DomNode * Props -> unit,
    removeProps: DomNode * Props -> unit,
    newView: unit -> View,
    setAsChildOfParentView: bool,
    subElementsPropKeys: RawPropKey list
  ) =

  let viewSetEvent = Event<View>()

  member _.Name = name
  member val View = view with get, set
  member val Origin: DomOrigin = DomRoot with get, set
  member val AppliedProps: Props = appliedProps with get, set
  member val EventRegistrar: EventHandlerRegistrar = EventHandlerRegistrar() with get, set
  member val Children = ResizeArray<DomChild>() with get
  member val SubElementDomNodes = ResizeArray<DomNode>() with get
  member _.SetAsChildOfParentView = setAsChildOfParentView
  member _.SubElementsPropKeys = subElementsPropKeys

  member this.ApplyProps(props) = applyProps (this, props)
  member this.RemoveProps(props) = removeProps (this, props)
  member _.NewView() = newView ()

  /// Stable path for debugging/IDs, derived from the DomOrigin chain.
  /// Component segments resolve through the component's own DomOrigin, so paths are fully
  /// DomNode-native.
  member this.GetPath() =
    let rec pathOf (name: string) (origin: DomOrigin) : string =
      match origin with
      | DomRoot -> $"root:{name}"
      | DomElmishComponent parent -> $"{pathOf parent.Name parent.DomOrigin}:{name}"
      | DomChild(parent, index) -> $"{parent.GetPath()}|child[{index}]:{name}"
      | DomSubElement(parent, index, key) ->
        let indexStr = index |> Option.map (sprintf "[%i]") |> Option.defaultValue ""
        $"{parent.GetPath()}|{key}{indexStr}:{name}"

    pathOf name this.Origin

  /// Fires the ViewReady event, analogous to ViewSet on the old ViewBackedTerminalElement.
  /// Called by createFromTE after the DomNode is fully initialized.
  member this.TriggerViewReady() = viewSetEvent.Trigger this.View

  [<CLIEvent>]
  member _.OnViewSet = viewSetEvent.Publish

/// Bundles the DomNode-compatible applicator functions for a TE type.
/// Generated per-element by the DomNode.Elements generator.
and internal DomNodeSpec =
  { ApplyProps: DomNode * Props -> unit
    RemoveProps: DomNode * Props -> unit
    NewView: unit -> View
    SetAsChildOfParentView: bool
    SubElementsPropKeys: RawPropKey list }

/// Implemented by each generated TerminalElement class to expose its DomNodeSpec.
and internal IDomSpecProvider =
  abstract DomSpec: DomNodeSpec

type PosAxis =
  | X
  | Y

type Props with
  static member private toEntries(props: Props) =
    seq {
      for kindKv in props.Props do
        for keyKv in kindKv.Value do
          KeyValuePair({ Kind = kindKv.Key; Key = keyKv.Key }, keyKv.Value)
    }

  static member add(k: PropKey, v: obj) =
    fun (this: Props) ->
      match this.Props.TryGetValue k.Kind with
      | true, byKey -> byKey.Add(k.Key, v)
      | false, _ ->
        let byKey = Dictionary<RawPropKey, obj>()
        byKey.Add(k.Key, v)
        this.Props.Add(k.Kind, byKey)

  static member add<'a>(k: PropKey<'a>, v: 'a) =
    fun (this: Props) -> this |> Props.add (k.Untyped, v :> obj)

  static member getOrInit<'a> (k: PropKey<'a>) (init: unit -> 'a) (this: Props) : 'a =
    match Props.tryFind k.Untyped this with
    | Some value -> value |> unbox<'a>
    | None ->
      let value = init ()
      Props.add (k.Untyped, value :> obj) this
      value

  static member remove (k: PropKey) (this: Props) =
    match this.Props.TryGetValue k.Kind with
    | true, byKey ->
      byKey.Remove k.Key |> ignore

      if byKey.Count = 0 then
        this.Props.Remove k.Kind |> ignore
    | false, _ -> ()

  static member tryFind(key: PropKey) =
    fun (this: Props) ->
      match this.Props.TryGetValue key.Kind with
      | true, byKey ->
        match byKey.TryGetValue key.Key with
        | true, v -> Some v
        | _ -> None
      | _ -> None

  static member tryFind(key: PropKey<'a>) =
    fun (this: Props) ->
      match Props.tryFind key.Untyped this with
      | Some v -> v |> unbox<'a> |> Some
      | None -> None

  static member tryFind(kind: PropKeyKind, key: RawPropKey) =
    fun (this: Props) ->
      let propKey = { Kind = kind; Key = key }
      Props.tryFind propKey this

  static member tryFind<'a>(kind: PropKeyKind, key: string) =
    fun (this: Props) -> Props.tryFind (kind, key) this |> Option.map (fun v -> v |> unbox<'a>)

  /// <summary>Builds two new Props, the first containing the bindings for which the given predicate returns 'true', and the other the remaining bindings.</summary>
  /// <returns>A pair of Props in which the first contains the elements for which the predicate returned true and the second containing the elements for which the predicated returned false.</returns>
  static member partition predicate (props: Props) =
    let first = Props()
    let second = Props()

    for kv in Props.toEntries props do
      if predicate kv then
        first |> Props.add (kv.Key, kv.Value)
      else
        second |> Props.add (kv.Key, kv.Value)

    first, second

  static member filter predicate (props: Props) =
    let result = Props()

    for kv in Props.toEntries props do
      if predicate kv then
        result |> Props.add (kv.Key, kv.Value)

    result

  static member find (key: PropKey<'a>) (props: Props) =
    match Props.tryFind key props with
    | Some v -> v
    | None -> failwith $"Failed to find '{key}'"

  static member rawKeyExists (k: PropKey) (p: Props) =
    match p.Props.TryGetValue k.Kind with
    | true, byKey -> byKey.ContainsKey k.Key
    | _ -> false

  static member exists (k: PropKey<'a>) (p: Props) = Props.rawKeyExists k.Untyped p

  static member keys(props: Props) = Props.toEntries props |> Seq.map _.Key

  static member filterSubElementKeys(props: Props) =
    match props.Props.TryGetValue PropKeyKind.SubElement with
    | true, byKey ->
      byKey.Keys
      |> Seq.map (fun key ->
        { Kind = PropKeyKind.SubElement
          Key = key })
    | _ -> Seq.empty

  static member iter iteration (props: Props) =
    Props.toEntries props |> Seq.iter iteration

/// Standalone event handler helpers that work with any (EventHandlerRegistrar, Props) pair.
/// Used by the generated DomNode applicators.
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
    |> Option.iter (fun action -> registrar.SetEventHandler(k, event, (action: EventArgs -> unit)))

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
