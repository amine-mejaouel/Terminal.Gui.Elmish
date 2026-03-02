namespace Terminal.Gui.Elmish

open System
open System.Collections.Generic
open Terminal.Gui.ViewBase

type ITerminalElement =
  interface
  end

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

  [<CustomEquality; NoComparison>]
  type PropKey =
    { Kind: PropKeyKind; Key: RawPropKey }

    member this.viewKey =
      match this.Kind with
      | PropKeyKind.SubElement -> { Kind = PropKeyKind.View; Key = this.Key.Replace("_element", "_view") }
      | _ -> failwith $"viewKey is only valid for SubElement PropKeys, got: {this}"

    override this.Equals(obj) =
      match obj with
      | :? PropKey as other -> this.Key = other.Key
      | _ -> false

    override this.GetHashCode() = this.Key.GetHashCode()

  [<CustomEquality; NoComparison>]
  type PropKey<'a> =
    private
    | PropKey of PropKey
    member this.Untyped = let (PropKey k) = this in k
    member this.key = this.Untyped.Key
    override this.Equals(obj) =
      match obj with
      | :? PropKey<'a> as other -> this.Untyped = other.Untyped
      | _ -> false
    override this.GetHashCode() = this.Untyped.GetHashCode()

  [<RequireQualifiedAccess>]
  module PropKey =

    let viewKeyOfSubElement (key: RawPropKey) : PropKey =
      { Kind = PropKeyKind.View; Key = key.Replace("_element", "_view") }

    type Create =
      static member subElement<'a>(key: string) : PropKey<'a> =
        if key.EndsWith "_element" then PropKey { Kind = PropKeyKind.SubElement; Key = key }
        else failwith $"Invalid key: {key}"
      static member simple<'a>(key: string) : PropKey<'a> =
        if key.EndsWith "_element" || key.EndsWith "_view" then
          failwith $"Invalid key: {key}"
        else PropKey { Kind = PropKeyKind.Simple; Key = key }
      static member event<'a>(key: string) : PropKey<'a> =
        if not (key.EndsWith "_event") then failwith $"Invalid key: {key}"
        else PropKey { Kind = PropKeyKind.Event; Key = key }
      static member view<'a>(key: string) : PropKey<'a> =
        if not (key.EndsWith "_view") then failwith $"Invalid key: {key}"
        else PropKey { Kind = PropKeyKind.View; Key = key }

/// Props object that is still under construction
type internal Props() =
  member val Children: List<TerminalElement> = List<_>() with get
  member val X: Pos option = None with get, set
  member val Y: Pos option = None with get, set
  member val XDelayed: TPos option = None with get, set
  member val YDelayed: TPos option = None with get, set
  /// Include all other properties that are not present as explicit members of Props.
  member val Props = Dictionary<PropKey, obj>() with get

and internal ITerminalElementBase =
  inherit ITerminalElement
  inherit IDisposable
  abstract Origin: Origin with get, set
  abstract Name: string
  abstract View: View with get
  abstract OnViewSet: IEvent<View>
  abstract GetPath: unit -> string

and internal IViewTE =
  inherit ITerminalElementBase

  abstract Props: Props with get
  abstract SetAsChildOfParentView: bool
  abstract Children: List<TerminalElement>

  abstract InitializeTree: origin: Origin -> unit
  abstract Reuse: prev: IViewTE -> unit

and internal IElmishComponentTE =
  inherit ITerminalElementBase

  abstract Child: IViewTE with get

  abstract StartElmishLoop : unit -> unit
  abstract Reuse: prev: IElmishComponentTE -> unit

and internal TerminalElement =
  | ViewTE of IViewTE
  | ElmishComponentTE of IElmishComponentTE

  static member from (te: ITerminalElement) =
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
  member this.Origin with set value = this.TerminalElementBase.Origin <- value
  member this.ViewSet = this.TerminalElementBase.OnViewSet
  member this.View = this.TerminalElementBase.View
  member this.GetPath() = this.TerminalElementBase.GetPath()
  member this.Dispose() = this.TerminalElementBase.Dispose()

  interface ITerminalElementBase with
    member this.View = this.View
    member this.OnViewSet = this.ViewSet
    member this.Name = this.Name
    member this.Origin = this.Origin
    member this.Origin with set value = this.Origin <- value
    member this.GetPath() = this.GetPath()
    member this.Dispose() = this.Dispose()

and internal Origin =
  | Root
  | ElmishComponent of Parent: IElmishComponentTE
  | Child of Parent: IViewTE * Index: int
  | SubElement of Parent: IViewTE * Index: int option * Property: RawPropKey

type PosAxis =
  | X
  | Y

type Props with
  static member addNonTyped (k: PropKey) (v: obj) (this: Props) = this.Props.Add(k, v)

  static member add<'a>(k: PropKey<'a>, v: 'a) (this: Props) = this |> Props.addNonTyped k.Untyped (v :> obj)

  static member getOrInit<'a> (k: PropKey<'a>) (init: unit -> 'a) (this: Props) : 'a =
    match this.Props.TryGetValue k.Untyped with
    | true, value -> value |> unbox<'a>
    | false, _ ->
      let value = init ()
      this.Props[k.Untyped] <- value :> obj
      value

  static member remove (k: PropKey) (this: Props) = this.Props.Remove k |> ignore

  static member tryFind (key: PropKey) =
    fun (this: Props) ->
      match this.Props.TryGetValue key with
      | true, v -> Some v
      | _, _ -> None

  static member tryFind (key: PropKey<'a>) =
    fun (this: Props) ->
      match Props.tryFind key.Untyped this with
      | Some v -> v |> unbox<'a> |> Some
      | None -> None

  static member tryFind (kind: PropKeyKind, key: RawPropKey) =
    fun (this: Props) ->
      let propKey = { Kind = kind; Key = key }
      Props.tryFind propKey this

  static member tryFind<'a> (kind: PropKeyKind, key: string) =
    fun (this: Props) ->
      Props.tryFind (kind, key) this
      |> Option.map (fun v -> v |> unbox<'a>)

  /// <summary>Builds two new Props, the first containing the bindings for which the given predicate returns 'true', and the other the remaining bindings.</summary>
  /// <returns>A pair of Props in which the first contains the elements for which the predicate returned true and the second containing the elements for which the predicated returned false.</returns>
  static member partition predicate (props: Props) =
    let first = Props()
    let second = Props()

    for kv in props.Props do
      if predicate kv then
        first |> Props.addNonTyped kv.Key kv.Value
      else
        second |> Props.addNonTyped kv.Key kv.Value

    first, second

  static member filter predicate (props: Props) =
    let result = Props()

    for kv in props.Props do
      if predicate kv then
        result |> Props.addNonTyped kv.Key kv.Value

    result

  static member find key (props: Props) =
    match Props.tryFind key props with
    | Some v -> v
    | None -> failwith $"Failed to find '{key}'"

  static member rawKeyExists (k: PropKey) (p: Props) = p.Props.ContainsKey k

  static member exists (k: PropKey<'a>) (p: Props) = p.Props.ContainsKey k.Untyped

  static member keys (props: Props) = props.Props.Keys |> Seq.map id

  static member filterSubElementKeys (props: Props) =
    props.Props.Keys
    |> Seq.choose (fun k -> if k.Kind = PropKeyKind.SubElement then Some k else None)

  static member iter iteration (props: Props) = props.Props |> Seq.iter iteration

[<AutoOpen>]
module Element =

  module internal Origin =
    let parentTerminalElement this : TerminalElement option =
      match this with
      | Root -> None
      | Child (parent, _) -> Some (TerminalElement.ViewTE parent)
      | SubElement(parent, _, _) -> Some (TerminalElement.ViewTE parent)
      | ElmishComponent parent -> Some (TerminalElement.ElmishComponentTE parent)

    let rec parentView (this: Origin) =
      match this |> parentTerminalElement with
      | Some (ElmishComponentTE parent) ->
        parent.Origin |> parentView
      | Some (ViewTE parent) -> Some parent.View
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
        | Origin.ElmishComponent _ ->
          "child"
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

