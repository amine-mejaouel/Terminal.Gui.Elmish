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

  [<RequireQualifiedAccess; CustomEquality; NoComparison>]
  type PropKey =
    | Simple of key: string
    | View of key: string
    | Event of key: string
    | SingleElement of key: string
    | MultiElement of key: string

    member this.key =
      match this with
      | PropKey.Simple k | PropKey.View k | PropKey.Event k
      | PropKey.SingleElement k | PropKey.MultiElement k -> k

    member this.viewKey =
      match this with
      | PropKey.SingleElement k -> PropKey.View (k.Replace("_element", "_view"))
      | PropKey.MultiElement k -> PropKey.View (k.Replace("_elements", "_view"))
      | _ -> failwith $"viewKey is only valid for SingleElement and MultiElement PropKeys, got: {this}"

    override this.Equals(obj) =
      match obj with
      | :? PropKey as other -> this.key = other.key
      | _ -> false

    override this.GetHashCode() = this.key.GetHashCode()

  [<CustomEquality; NoComparison>]
  type TypedPropKey<'a> =
    private
    | TypedKey of PropKey
    member this.Untyped = let (TypedKey k) = this in k
    member this.key = this.Untyped.key
    override this.Equals(obj) =
      match obj with
      | :? TypedPropKey<'a> as other -> this.Untyped = other.Untyped
      | _ -> false
    override this.GetHashCode() = this.Untyped.GetHashCode()

  [<CustomEquality; NoComparison>]
  type internal SubElementPropKey<'a> =
    | SingleElementKey of TypedPropKey<'a>
    | MultiElementKey of TypedPropKey<'a>

    member this.typed =
      match this with
      | SingleElementKey k | MultiElementKey k -> k

    member this.untyped = this.typed.Untyped
    member this.key = this.untyped.key

    static member createSingleElementKey<'a>(key: string) : SubElementPropKey<'a> =
      if key.EndsWith "_element" then
        SingleElementKey (TypedKey (PropKey.SingleElement key))
      else
        failwith $"Invalid single-element key: {key}"

    static member createMultiElementKey<'a>(key: string) : SubElementPropKey<'a> =
      if key.EndsWith "_elements" then
        MultiElementKey (TypedKey (PropKey.MultiElement key))
      else
        failwith $"Invalid multi-element key: {key}"

    static member from(key: TypedPropKey<'a>) : SubElementPropKey<'b> =
      match key.Untyped with
      | PropKey.SingleElement k -> SubElementPropKey<'b>.createSingleElementKey k
      | PropKey.MultiElement k -> SubElementPropKey<'b>.createMultiElementKey k
      | _ -> failwith $"SubElementPropKey.from: expected SingleElement or MultiElement, got {key.Untyped}"

    override this.GetHashCode() = this.key.GetHashCode()

    override this.Equals(obj) =
      match obj with
      | :? SubElementPropKey<'a> as x -> this.key = x.key
      | :? PropKey as x -> this.key = x.key
      | _ -> false

    member this.viewKey = this.untyped.viewKey

  [<RequireQualifiedAccess>]
  module PropKey =
    type Create =
      static member singleElement<'a>(key: string) : TypedPropKey<'a> =
        if key.EndsWith "_element" then TypedKey (PropKey.SingleElement key)
        else failwith $"Invalid key: {key}"
      static member multiElement<'a>(key: string) : TypedPropKey<'a> =
        if key.EndsWith "_elements" then TypedKey (PropKey.MultiElement key)
        else failwith $"Invalid key: {key}"
      static member simple<'a>(key: string) : TypedPropKey<'a> =
        if key.EndsWith "_element" || key.EndsWith "_elements" || key.EndsWith "_view" then
          failwith $"Invalid key: {key}"
        else TypedKey (PropKey.Simple key)
      static member event<'a>(key: string) : TypedPropKey<'a> =
        if not (key.EndsWith "_event") then failwith $"Invalid key: {key}"
        else TypedKey (PropKey.Event key)
      static member view<'a>(key: string) : TypedPropKey<'a> =
        if not (key.EndsWith "_view") then failwith $"Invalid key: {key}"
        else TypedKey (PropKey.View key)

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
  | SubElement of Parent: IViewTE * Index: int option * Property: SubElementPropKey<IViewTE>

type PosAxis =
  | X
  | Y

module internal Props =
  let addNonTyped (k: PropKey) (v: obj) (this: Props) = this.Props.Add(k, v)

  let add<'a>(k: TypedPropKey<'a>, v: 'a) (this: Props) = this |> addNonTyped k.Untyped (v :> obj)

  let getOrInit<'a> (k: TypedPropKey<'a>) (init: unit -> 'a) (this: Props) : 'a =
    match this.Props.TryGetValue k.Untyped with
    | true, value -> value |> unbox<'a>
    | false, _ ->
      let value = init ()
      this.Props[k.Untyped] <- value :> obj
      value

  let remove (k: PropKey) (this: Props) = this.Props.Remove k |> ignore

  let tryFind (key: TypedPropKey<'a>) (this: Props) =
    match this.Props.TryGetValue key.Untyped with
    | true, v -> v |> unbox<'a> |> Some
    | _, _ -> None

  /// <summary>Builds two new Props, the first containing the bindings for which the given predicate returns 'true', and the other the remaining bindings.</summary>
  /// <returns>A pair of Props in which the first contains the elements for which the predicate returned true and the second containing the elements for which the predicated returned false.</returns>
  let partition predicate (props: Props) =
    let first = Props()
    let second = Props()

    for kv in props.Props do
      if predicate kv then
        first |> addNonTyped kv.Key kv.Value
      else
        second |> addNonTyped kv.Key kv.Value

    first, second

  let filter predicate (props: Props) =
    let result = Props()

    for kv in props.Props do
      if predicate kv then
        result |> addNonTyped kv.Key kv.Value

    result

  let tryFindByRawKey<'a> (key: PropKey) (props: Props) =
    match props.Props.TryGetValue key with
    | true, v -> v |> unbox<'a> |> Some
    | _, _ -> None

  let find key (props: Props) =
    match tryFind key props with
    | Some v -> v
    | None -> failwith $"Failed to find '{key}'"

  let tryFindWithDefault (key: TypedPropKey<'a>) defaultValue props =
    props
    |> tryFind key
    |> Option.defaultValue defaultValue

  let rawKeyExists (k: PropKey) (p: Props) = p.Props.ContainsKey k

  let exists (k: TypedPropKey<'a>) (p: Props) = p.Props.ContainsKey k.Untyped

  let keys (props: Props) = props.Props.Keys |> Seq.map id

  let filterSingleElementKeys (props: Props) =
    props.Props.Keys
    |> Seq.choose (function | PropKey.SingleElement _ as k -> Some k | _ -> None)

  let iter iteration (props: Props) = props.Props |> Seq.iter iteration

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
        | Origin.SubElement(_, _, subElementPropKey) -> $"{subElementPropKey.key}"

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



