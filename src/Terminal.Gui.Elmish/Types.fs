namespace Terminal.Gui.Elmish

open System
open System.Collections.Generic
open Terminal.Gui.ViewBase

type ITerminalElement = interface end
type IView = interface end

[<AutoOpen>]
module internal PropKey =

  [<RequireQualifiedAccess>]
  type PropKeyKind =
    | Simple
    | SubView
    | Event
    | SubViewSpec

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
      | PropKeyKind.SubViewSpec ->
        { Kind = PropKeyKind.SubView
          Key = this.Key.Replace("_viewSpec", "_view") }
      | _ -> failwith $"viewKey is only valid for SubView PropKeys, got: {this}"

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
      { Kind = PropKeyKind.SubView
        Key = key.Replace("_viewSpec", "_view") }

    type Create =
      static member subElement<'a>(key: string) : PropKey<'a> =
        if key.EndsWith "_viewSpec" then
          PropKey
            { Kind = PropKeyKind.SubViewSpec
              Key = key }
        else
          failwith $"Invalid key: {key}"

      static member simple<'a>(key: string) : PropKey<'a> =
        if key.EndsWith "_viewSpec" || key.EndsWith "_view" then
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
          PropKey
            { Kind = PropKeyKind.SubView
              Key = key }

type Props() =
  // Internal properties

  /// Includes all others properties that are not present as explicit members of Props.
  member val internal Props = Dictionary<PropKeyKind, Dictionary<RawPropKey, obj>>() with get
  member val internal Children: List<ViewSpec> = List<_>() with get

  // Public properties
  member val Id: string option = None with get, set
  member val X: TPos = TPos.Default with get, set
  member val Y: TPos = TPos.Default with get, set

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

and internal IComponentView =
  abstract Props: Props
  abstract Update: Props -> unit

/// <summary>
/// <p>ComponentViewSpec is the <c>ViewSpec</c> of an Elmish component.</p>
/// <p><c>InitComponentView()</c> is intended to be called only once, when the component is first added to the tree.</p>
/// <p>If the component is already existing in the tree, then <c>InitComponentView()</c> is not meant to be called again, and <c>ClearInitComponentView()</c> can be called to clear the reference to the initialization function, allowing it to be garbage collected.</p>
/// </summary>
and [<Interface>] internal IComponentViewSpec =
  abstract Props: Props
  abstract InitComponentView: unit -> IComponentView
  abstract ClearInitComponentView: unit -> unit

and internal ViewSpec =
  | SimpleViewSpec of IViewBase
  | ComponentViewSpec of IComponentViewSpec

  interface IView

and [<Interface>] internal IViewBase =
  inherit IView
  abstract Props: Props
  abstract CreateViewTE: unit -> IViewTE

and [<Obsolete>] internal IViewTE =
  inherit ITerminalElementBase

  abstract Props: Props with get
  abstract SetAsChildOfParentView: bool
  abstract Children: List<TerminalElement>

  abstract InitializeTree: origin: Origin -> unit
  abstract Reuse: prev: IViewTE -> unit

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
and [<Obsolete>] internal IElmishComponentTE =
  inherit ITerminalElementBase
  inherit IComponentViewSpec
  abstract Child: IViewTE with get
  abstract StartElmishLoop: unit -> unit

and internal TerminalElement =
  | ViewTE of IViewTE
  | ElmishComponentTE of IElmishComponentTE

  [<Obsolete>]
  static member from(view: IView) =
    match view with
    | :? IViewBase as viewBase -> viewBase.CreateViewTE() |> TerminalElement.from
    | :? ITerminalElement as terminalElement -> TerminalElement.from terminalElement
    | :? ViewSpec as spec ->
      match spec with
      | SimpleViewSpec viewBase -> viewBase.CreateViewTE() |> TerminalElement.from
      | ComponentViewSpec cvs -> TerminalElement.ElmishComponentTE(cvs :?> IElmishComponentTE)
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

and [<Obsolete>] internal Origin =
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
    | :? IViewBase as viewBase -> SimpleViewSpec viewBase
    | :? IElmishComponentTE as te -> ComponentViewSpec te
    | _ -> failwith "Invalid view type"

type Props with
  static member private toEntries(props: Props) =
    seq {
      for kindKv in props.Props do
        for keyKv in kindKv.Value do
          KeyValuePair({ Kind = kindKv.Key; Key = keyKv.Key }, keyKv.Value)
    }

  static member internal add(k: PropKey, v: obj) =
    fun (this: Props) ->
      match this.Props.TryGetValue k.Kind with
      | true, byKey -> byKey.Add(k.Key, v)
      | false, _ ->
        let byKey = Dictionary<RawPropKey, obj>()
        byKey.Add(k.Key, v)
        this.Props.Add(k.Kind, byKey)

  static member internal add<'a>(k: PropKey<'a>, v: 'a) =
    fun (this: Props) -> this |> Props.add (k.Untyped, v :> obj)

  static member internal getOrInit<'a> (k: PropKey<'a>) (init: unit -> 'a) (this: Props) : 'a =
    match Props.tryFind k.Untyped this with
    | Some value -> value |> unbox<'a>
    | None ->
      let value = init ()
      Props.add (k.Untyped, value :> obj) this
      value

  static member internal remove (k: PropKey) (this: Props) =
    match this.Props.TryGetValue k.Kind with
    | true, byKey ->
      byKey.Remove k.Key |> ignore

      if byKey.Count = 0 then
        this.Props.Remove k.Kind |> ignore
    | false, _ -> ()

  static member internal tryFind(key: PropKey) =
    fun (this: Props) ->
      match this.Props.TryGetValue key.Kind with
      | true, byKey ->
        match byKey.TryGetValue key.Key with
        | true, v -> Some v
        | _ -> None
      | _ -> None

  static member internal tryFind(key: PropKey<'a>) =
    fun (this: Props) ->
      match Props.tryFind key.Untyped this with
      | Some v -> v |> unbox<'a> |> Some
      | None -> None

  static member internal tryFind(kind: PropKeyKind, key: RawPropKey) =
    fun (this: Props) ->
      let propKey = { Kind = kind; Key = key }
      Props.tryFind propKey this

  static member internal tryFind<'a>(kind: PropKeyKind, key: string) =
    fun (this: Props) -> Props.tryFind (kind, key) this |> Option.map (fun v -> v |> unbox<'a>)

  /// <summary>Builds two new Props, the first containing the bindings for which the given predicate returns 'true', and the other the remaining bindings.</summary>
  /// <returns>A pair of Props in which the first contains the elements for which the predicate returned true and the second containing the elements for which the predicated returned false.</returns>
  static member internal partition predicate (props: Props) =
    let first = Props()
    let second = Props()

    for kv in Props.toEntries props do
      if predicate kv then
        first |> Props.add (kv.Key, kv.Value)
      else
        second |> Props.add (kv.Key, kv.Value)

    first, second

  static member internal filter predicate (props: Props) =
    let result = Props()

    for kv in Props.toEntries props do
      if predicate kv then
        result |> Props.add (kv.Key, kv.Value)

    result

  static member internal find (key: PropKey<'a>) (props: Props) =
    match Props.tryFind key props with
    | Some v -> v
    | None -> failwith $"Failed to find '{key}'"

  static member internal rawKeyExists (k: PropKey) (p: Props) =
    match p.Props.TryGetValue k.Kind with
    | true, byKey -> byKey.ContainsKey k.Key
    | _ -> false

  static member internal exists (k: PropKey<'a>) (p: Props) = Props.rawKeyExists k.Untyped p

  static member internal keys(props: Props) = Props.toEntries props |> Seq.map _.Key

  static member internal filterSubElementKeys(props: Props) =
    match props.Props.TryGetValue PropKeyKind.SubViewSpec with
    | true, byKey ->
      byKey.Keys
      |> Seq.map (fun key ->
        { Kind = PropKeyKind.SubViewSpec
          Key = key })
    | _ -> Seq.empty

  static member internal iter iteration (props: Props) =
    Props.toEntries props |> Seq.iter iteration

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
