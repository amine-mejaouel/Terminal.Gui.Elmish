namespace Terminal.Gui.Elmish

open System
open System.Collections.Generic
open Terminal.Gui.App
open Terminal.Gui.ViewBase

type ITerminalElement = interface end
type IView = interface end

type ComponentProps(componentName) =
  member _.ComponentName: string = componentName

  /// Stable identity used by the virtual terminal tree reconciler among sibling components.
  member val Key: string option = None with get, set

  member val Id: string = "" with get, set

  member val Props: Dictionary<int, obj> = Dictionary<int, obj>() with get, private set

  member this.Item
    with set (key: int) value = this.Props[key] <- value

  member this.TryGetPropValue<'t>(key: int) =
    match this.Props.TryGetValue key with
    | true, value -> Some value |> Option.map unbox<'t>
    | _ -> None

  member internal this.UpdateFrom(other: ComponentProps) =
    this.Key <- other.Key
    this.Id <- other.Id
    this.Props.Clear()

    for KeyValue(key, value) in other.Props do
      this.Props[key] <- value

[<AutoOpen>]
module internal PropKey =

  type RawPropKey = string

  /// Strongly typed identifier used to index generated properties.
  [<Struct>]
  type PropertyId =
    private
    | PropertyId of int

    static member internal Create(value: int) = PropertyId value

    member this.Value =
      let (PropertyId value) = this
      value

    override this.ToString() = string this.Value

  /// Identifiers shared by the native and declarative representations of a view-valued property.
  type SubViewPropertyIds =
    {
      /// Identifies the concrete <c>Terminal.Gui.View</c> property value.
      View: PropertyId
      /// Identifies the declarative <c>IView</c> specification for the same property.
      ViewSpec: PropertyId
    }

  /// Describes how a generated property participates in property application and virtual-tree reconciliation.
  [<RequireQualifiedAccess>]
  type PropKeyIdentity =
    /// Identifies an ordinary property value that is applied directly to a Terminal.Gui object.
    | Simple of id: PropertyId
    /// Identifies an event-handler property managed by the event registrar.
    | Event of id: PropertyId
    /// Identifies the concrete <c>Terminal.Gui.View</c> assigned to a view-valued property. This is a native property
    /// value, not an ordered child in the parent's <c>SubViews</c> collection.
    | SubView of ids: SubViewPropertyIds
    /// Identifies the declarative <c>IView</c> supplied for a view-valued property. The virtual-tree reconciler mounts
    /// this specification as a slot and stores the resulting native view under the paired <c>SubView</c> identity.
    | SubViewSpec of ids: SubViewPropertyIds

  type internal IRawPropKey =
    abstract Identity: PropKeyIdentity
    abstract RawKey: RawPropKey

  let private equalsByIdentityAndRawKey identity (rawKey: RawPropKey) (obj: obj) =
    match obj with
    | :? IRawPropKey as other -> identity = other.Identity && rawKey = other.RawKey
    | _ -> false

  [<CustomEquality; NoComparison>]
  type PropKey =
    { Identity: PropKeyIdentity
      Key: RawPropKey }

    member this.Id =
      match this.Identity with
      | PropKeyIdentity.Simple id
      | PropKeyIdentity.Event id -> id
      | PropKeyIdentity.SubView ids -> ids.View
      | PropKeyIdentity.SubViewSpec ids -> ids.ViewSpec

    member this.IsSubViewSpec =
      match this.Identity with
      | PropKeyIdentity.SubViewSpec _ -> true
      | _ -> false

    member this.viewKey =
      match this.Identity with
      | PropKeyIdentity.SubViewSpec ids ->
        { Identity = PropKeyIdentity.SubView ids
          Key = this.Key.Replace("_viewSpec", "_view") }
      | _ -> failwith $"viewKey is only valid for SubView PropKeys, got: {this}"

    override this.Equals(obj) =
      equalsByIdentityAndRawKey this.Identity this.Key obj

    override this.GetHashCode() =
      HashCode.Combine(this.Identity, this.Key)

    interface IRawPropKey with
      member this.Identity = this.Identity
      member this.RawKey = this.Key

  [<CustomEquality; NoComparison>]
  type PropKey<'a> =
    private
    | PropKey of PropKey

    member this.Untyped = let (PropKey k) = this in k
    member this.id = this.Untyped.Id
    member this.key = this.Untyped.Key

    override this.Equals(obj) =
      equalsByIdentityAndRawKey this.Untyped.Identity this.Untyped.Key obj

    override this.GetHashCode() = this.Untyped.GetHashCode()

    interface IRawPropKey with
      member this.Identity = this.Untyped.Identity
      member this.RawKey = this.Untyped.Key

  [<RequireQualifiedAccess>]
  module PropKey =

    let viewKeyOfSubElement (key: PropKey) : PropKey = key.viewKey

    type Create =
      static member subElement<'a>(viewId: PropertyId, viewSpecId: PropertyId, key: string) : PropKey<'a> =
        if key.EndsWith "_viewSpec" then
          PropKey
            { Identity = PropKeyIdentity.SubViewSpec { View = viewId; ViewSpec = viewSpecId }
              Key = key }
        else
          failwith $"Invalid key: {key}"

      static member simple<'a>(id: PropertyId, key: string) : PropKey<'a> =
        if key.EndsWith "_viewSpec" || key.EndsWith "_view" then
          failwith $"Invalid key: {key}"
        else
          PropKey
            { Identity = PropKeyIdentity.Simple id
              Key = key }

      static member event<'a>(id: PropertyId, key: string) : PropKey<'a> =
        if not (key.EndsWith "_event") then
          failwith $"Invalid key: {key}"
        else
          PropKey
            { Identity = PropKeyIdentity.Event id
              Key = key }

      static member view<'a>(viewId: PropertyId, viewSpecId: PropertyId, key: string) : PropKey<'a> =
        if not (key.EndsWith "_view") then
          failwith $"Invalid key: {key}"
        else
          PropKey
            { Identity = PropKeyIdentity.SubView { View = viewId; ViewSpec = viewSpecId }
              Key = key }

type internal Props() =

  /// Stable identity used by the virtual terminal tree reconciler among sibling views.
  member val Key: string option = None with get, set
  member val X: TPos = TPos.Default with get, set
  member val Y: TPos = TPos.Default with get, set

  /// Flat generated-property snapshot keyed by a collision-free generated property ID.
  member val Props = Dictionary<PropertyId, KeyValuePair<PropKey, obj>>() with get
  member val SubViewSpecCount = 0 with get, set
  member val Children: List<ViewSpec> = List<_>() with get

// TODO: TPos is no implement all features of Terminal.Gui Pos,
// TODO: should add a unit test ensuring that all features of TPos are implemented.
and [<RequireQualifiedAccess>] TPos =
  | Default
  | X of IView
  | Y of IView
  | Top of IView
  | Bottom of IView
  | Left of IView
  | Right of IView
  | Absolute of position: int
  | AnchorEnd of offset: int option
  | Center
  | Percent of percent: int
  | Func of func: (View -> int) * view: IView
  | Align of alignment: Alignment * modes: AlignmentModes * groupId: int option

and internal ITerminalElementBase =
  inherit ITerminalElement
  inherit IDisposable
  abstract Origin: Origin with get, set
  abstract Name: string
  abstract View: View with get
  abstract OnViewSet: IEvent<View>
  abstract GetPath: unit -> string

and [<Interface>] internal IComponentViewSpec =
  abstract ComponentProps: ComponentProps
  abstract ComponentType: Type
  abstract ResolveTerminalElement: unit -> IElmishComponentTE
  abstract BindTerminalElement: IElmishComponentTE -> unit

and internal ViewSpec =
  | SimpleViewSpec of ISimpleViewSpec
  | ComponentViewSpec of IComponentViewSpec

  interface IView

and [<Interface>] internal ISimpleViewSpec =
  inherit IView
  abstract Props: Props
  abstract CreateViewTE: unit -> IViewTE
  abstract BindViewTE: IViewTE -> unit
  abstract SetProps: target: IViewTE * props: Props -> unit
  abstract RemoveProps: target: IViewTE * props: Props -> unit
  abstract ViewType: ViewType

and internal IViewTE =
  inherit ITerminalElementBase

  abstract Props: Props with get
  abstract SetAsChildOfParentView: bool
  abstract Children: List<TerminalElement>

  abstract InitializeTree: origin: Origin * application: IApplication -> unit

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
  inherit IComponentViewSpec
  abstract Child: IViewTE with get
  abstract StartElmishLoop: application: IApplication -> unit
  abstract UpdateProps: ComponentProps -> unit

and internal TerminalElement =
  | ViewTE of IViewTE
  | ElmishComponentTE of IElmishComponentTE

  static member from(view: IView) =
    match view with
    | :? ISimpleViewSpec as viewBase -> viewBase.CreateViewTE() |> TerminalElement.from
    | :? IComponentViewSpec as componentSpec ->
      componentSpec.ResolveTerminalElement() |> TerminalElement.ElmishComponentTE
    | :? ITerminalElement as terminalElement -> TerminalElement.from terminalElement
    | :? ViewSpec as spec ->
      match spec with
      | SimpleViewSpec viewBase -> viewBase.CreateViewTE() |> TerminalElement.from
      | ComponentViewSpec componentSpec -> componentSpec.ResolveTerminalElement() |> TerminalElement.ElmishComponentTE
    | _ -> failwith "Invalid view type"

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
  member this.Origin = this.TerminalElementBase.Origin

  member this.Origin
    with set value = this.TerminalElementBase.Origin <- value

  member this.ViewSet = this.TerminalElementBase.OnViewSet
  member this.View = this.TerminalElementBase.View
  member this.GetPath() = this.TerminalElementBase.GetPath()
  member this.Dispose() = this.TerminalElementBase.Dispose()

  interface ITerminalElementBase with
    member this.View = this.View
    member this.OnViewSet = this.ViewSet
    member this.Name = this.Name
    member this.Origin = this.Origin

    member this.Origin
      with set value = this.Origin <- value

    member this.GetPath() = this.GetPath()
    member this.Dispose() = this.Dispose()

// Origin describes how a TerminalElement is related to the root of the tree.

and internal Origin =
  /// Root element of the Elmish program.
  | Root
  /// Root element of an Elmish component.
  | ElmishComponent of Parent: IElmishComponentTE
  /// Child element of a view.
  | Child of Parent: IViewTE * Index: int
  /// SubElement of a view, such as a property that is itself a view, or a collection of views.
  | SubElement of Parent: IViewTE * Index: int option * Property: RawPropKey

type PosAxis =
  | X
  | Y

module internal ViewSpec =
  let from<'view when 'view :> IView> (view: 'view) =
    match box view with
    | :? ISimpleViewSpec as viewBase -> SimpleViewSpec viewBase
    | :? IComponentViewSpec as componentSpec -> ComponentViewSpec componentSpec
    | _ -> failwith "Invalid view type"

  let key =
    function
    | SimpleViewSpec view -> view.Props.Key
    | ComponentViewSpec componentSpec -> componentSpec.ComponentProps.Key

  let bind viewSpec terminalElement =
    match viewSpec, terminalElement with
    | SimpleViewSpec view, TerminalElement.ViewTE viewTe -> view.BindViewTE viewTe
    | ComponentViewSpec componentSpec, TerminalElement.ElmishComponentTE componentTe ->
      componentSpec.BindTerminalElement componentTe
    | _ -> invalidArg (nameof terminalElement) "The view specification and terminal element kinds do not match."

type Props with
  static member internal toEntries(props: Props) = seq { yield! props.Props.Values }

  static member internal add(k: PropKey, v: obj) =
    fun (this: Props) ->
      match this.Props.TryGetValue k.Id with
      | true, existing when not (existing.Key.Equals k) ->
        invalidOp $"Generated property ID {k.Id} is shared by '{existing.Key.Key}' and '{k.Key}'."
      | true, _ -> this.Props[k.Id] <- KeyValuePair(k, v)
      | false, _ ->
        this.Props[k.Id] <- KeyValuePair(k, v)

        if k.IsSubViewSpec then
          this.SubViewSpecCount <- this.SubViewSpecCount + 1

  static member internal add<'a>(k: PropKey<'a>, v: 'a) =
    fun (this: Props) -> this |> Props.add (k.Untyped, v :> obj)

  static member internal getOrInit<'a> (k: PropKey<'a>) (init: unit -> 'a) (this: Props) : 'a =
    match Props.tryFind k.Untyped this with
    | Some value -> value |> unbox<'a>
    | None ->
      let value = init ()
      Props.add (k.Untyped, value :> obj) this
      value

  static member internal tryFind(key: PropKey) =
    fun (this: Props) ->
      match this.Props.TryGetValue key.Id with
      | true, entry when entry.Key.Equals key -> Some entry.Value
      | _ -> None

  static member internal tryFind(key: PropKey<'a>) =
    fun (this: Props) ->
      match Props.tryFind key.Untyped this with
      | Some v -> v |> unbox<'a> |> Some
      | None -> None

  static member internal find (key: PropKey<'a>) (props: Props) =
    match Props.tryFind key props with
    | Some v -> v
    | None -> failwith $"Failed to find '{key}'"

  static member internal rawKeyExists (k: PropKey) (p: Props) =
    match p.Props.TryGetValue k.Id with
    | true, entry -> entry.Key.Equals k
    | _ -> false

  static member internal exists (k: PropKey<'a>) (p: Props) = Props.rawKeyExists k.Untyped p

  /// Returns only properties that need to be removed or applied. View specifications are
  /// reconciled separately and their live View properties are compared through SubView entries.
  static member internal diff(prevProps: Props, curProps: Props) =
    let mutable removed: Props option = None
    let mutable changed: Props option = None

    let addEntry current (entry: KeyValuePair<PropKey, obj>) =
      let target = current |> Option.defaultWith Props
      target |> Props.add (entry.Key, entry.Value)
      Some target

    for kv in Props.toEntries prevProps do
      if not kv.Key.IsSubViewSpec && not (curProps |> Props.rawKeyExists kv.Key) then
        removed <- addEntry removed kv

    for kv in Props.toEntries curProps do
      if not kv.Key.IsSubViewSpec then
        match prevProps |> Props.tryFind kv.Key with
        | Some previous when Object.Equals(previous, kv.Value) -> ()
        | _ -> changed <- addEntry changed kv

    removed, changed

[<AutoOpen>]
module Element =

  module internal Origin =
    let parentTerminalElement this : TerminalElement option =
      match this with
      | Root -> None
      | Child(parent, _) -> Some(TerminalElement.ViewTE parent)
      | SubElement(parent, _, _) -> Some(TerminalElement.ViewTE parent)
      | ElmishComponent parent -> Some(TerminalElement.ElmishComponentTE parent)

    let rec parentView (this: Origin) =
      match this |> parentTerminalElement with
      | Some(ElmishComponentTE parent) -> parent.Origin |> parentView
      | Some(ViewTE parent) -> Some parent.View
      | None -> None

    let getPath name (this: Origin) =
      let parentPath =
        match this |> parentTerminalElement with
        | Some parent -> parent.GetPath()
        | None -> "root"

      let propIdStr =
        match this with
        | Origin.Root -> ""
        | Origin.Child _
        | Origin.ElmishComponent _ -> "child"
        | Origin.SubElement(_, _, subElementPropKey) -> $"{subElementPropKey}"

      let indexStr =
        let rec indexStr origin =
          match origin with
          | Origin.Root -> ""
          | Origin.ElmishComponent parent -> indexStr parent.Origin
          | Origin.Child(_, index) -> $"[{index}]"
          | Origin.SubElement(_, index, _) -> index |> Option.map (sprintf "[%i]") |> Option.defaultValue ""

        indexStr this

      match this with
      | Origin.Root -> $"root:{name}"
      | Origin.ElmishComponent parent -> $"{parent.GetPath()}:{name}"
      | _ -> $"{parentPath}|{propIdStr}{indexStr}:{name}"
