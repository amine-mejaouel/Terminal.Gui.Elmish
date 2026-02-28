namespace rec Terminal.Gui.Elmish

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

type PosAxis =
  | X
  | Y

[<AutoOpen>]
module internal PropKey =

  /// Inheritors should override both of `Equals` and `GetHashCode` efficiently because Props are held in a dictionary and accessed frequently
  type IPropKey =
    abstract member key: string
    abstract member isViewKey: bool
    abstract member isSingleElementKey: bool

  type IPropKey<'a> =
    inherit IPropKey

  type ISimplePropKey<'a> =
    inherit IPropKey<'a>

  type IViewPropKey<'a> =
    inherit IPropKey<'a>

  type ISingleElementPropKey =
    inherit IPropKey
    abstract member viewKey: IPropKey

  type ISingleElementPropKey<'a> =
    inherit ISingleElementPropKey
    inherit IPropKey<'a>

  type IMultiElementPropKey<'a> =
    inherit IPropKey<'a>
    // TODO: same as ISingleElementPropKey, consider unifying
    abstract member viewKey: IPropKey

  type IEventPropKey<'a> =
    inherit IPropKey<'a>

  /// Represents a property in a Terminal.Gui View
  [<CustomEquality; NoComparison>]
  type private SimplePropKey<'a> =
    private
    | Key of string
    static member create<'a>(key: string) : ISimplePropKey<'a> =
      if
        key.EndsWith "_element"
        || key.EndsWith "_elements"
        || key.EndsWith "_view"
      then
        failwith $"Invalid key: {key}"
      else
        Key key

    static member map(value: SimplePropKey<'a>) : SimplePropKey<'b> = Key value.key

    member this.key =
      let (Key key) = this
      key

    override this.GetHashCode() = this.key.GetHashCode()

    override this.Equals(obj) =
      match obj with
      | :? IPropKey as x -> this.key.Equals(x.key)
      | _ -> false

    interface ISimplePropKey<'a> with
      member this.key = this.key
      member this.isViewKey = false
      member this.isSingleElementKey = false

  [<CustomEquality; NoComparison>]
  type private ViewPropKey<'a> =
    private
    | Key of string
    static member create<'a>(key: string) : IViewPropKey<'a> =
      if not (key.EndsWith "_view") then
        failwith $"Invalid key: {key}"
      else
        Key key

    static member map(value: ViewPropKey<'a>) : ViewPropKey<'b> = Key value.key

    member this.key =
      let (Key key) = this
      key

    override this.GetHashCode() = this.key.GetHashCode()

    override this.Equals(obj) =
      match obj with
      | :? IPropKey as x -> this.key.Equals(x.key)
      | _ -> false

    interface IViewPropKey<'a> with
      member this.key = this.key
      member this.isViewKey = true
      member this.isSingleElementKey = false

  [<CustomEquality; NoComparison>]
  type private EventPropKey<'a> =
    private
    | Key of string
    static member create<'a>(key: string) : IEventPropKey<'a> =
      if not (key.EndsWith "_event") then
        failwith $"Invalid key: {key}"
      else
        Key key

    member this.key =
      let (Key key) = this
      key

    override this.GetHashCode() = this.key.GetHashCode()

    override this.Equals(obj) =
      match obj with
      | :? IPropKey as x -> this.key.Equals(x.key)
      | _ -> false

    interface IEventPropKey<'a> with
      member this.key = this.key
      member this.isViewKey = false
      member this.isSingleElementKey = false

  [<CustomEquality; NoComparison>]
  type private SingleElementPropKey<'a> =
    private
    | Key of string
    static member create<'a>(key: string) : ISingleElementPropKey<'a> =
      if key.EndsWith "_element" then
        Key key
      else
        failwith $"Invalid key: {key}"

    member this.key =
      let (Key key) = this
      key

    member this.viewKey =
      ViewPropKey.create (this.key.Replace("_element", "_view")) :> IPropKey

    override this.GetHashCode() = this.key.GetHashCode()

    override this.Equals(obj) =
      match obj with
      | :? IPropKey as x -> this.key.Equals(x.key)
      | _ -> false

    interface ISingleElementPropKey<'a> with
      member this.key = this.key
      member this.isViewKey = false
      member this.isSingleElementKey = true
      member this.viewKey = this.viewKey

  [<CustomEquality; NoComparison>]
  type private MultiElementPropKey<'a> =
    private
    | Key of string
    static member create<'a>(key: string) : IMultiElementPropKey<'a> =
      if key.EndsWith "_elements" then
        Key key
      else
        failwith $"Invalid key: {key}"

    member this.key =
      let (Key key) = this
      key

    member this.viewKey =
      ViewPropKey.create (this.key.Replace("_elements", "_view")) :> IPropKey

    override this.GetHashCode() = this.key.GetHashCode()

    override this.Equals(obj) =
      match obj with
      | :? IPropKey as x -> this.key.Equals(x.key)
      | _ -> false

    interface IMultiElementPropKey<'a> with
      member this.key = this.key
      member this.isViewKey = false
      member this.isSingleElementKey = false
      member this.viewKey = this.viewKey

  [<RequireQualifiedAccess>]
  module PropKey =
    type Create =
      static member singleElement key = SingleElementPropKey.create key
      static member multiElement key = MultiElementPropKey.create key
      static member simple key = SimplePropKey.create key
      static member event key = EventPropKey.create key
      static member view key = ViewPropKey.create key

  [<CustomEquality; NoComparison>]
  type internal SubElementPropKey<'a> =
    | SingleElementKey of ISingleElementPropKey<'a>
    | MultiElementKey of IMultiElementPropKey<'a>

    static member createSingleElementKey<'a>(key: string) : SubElementPropKey<'a> =
      SingleElementKey(PropKey.Create.singleElement<'a> key)

    static member createMultiElementKey<'a>(key: string) : SubElementPropKey<'a> =
      MultiElementKey(PropKey.Create.multiElement<'a> key)

    static member from(singleElementKey: ISingleElementPropKey<'a>) : SubElementPropKey<'b> =
      SubElementPropKey<'b>.createSingleElementKey (singleElementKey :> IPropKey<'a>).key

    static member from(multiElementKey: IMultiElementPropKey<'a>) : SubElementPropKey<'b> =
      SubElementPropKey<'b>.createMultiElementKey (multiElementKey :> IPropKey<'a>).key

    member this.key =
      match this with
      | SingleElementKey key -> key.key
      | MultiElementKey key -> key.key

    override this.GetHashCode() = this.key.GetHashCode()

    override this.Equals(obj) =
      match obj with
      | :? IPropKey as x -> this.key.Equals(x.key)
      | _ -> false

    member this.viewKey =
      match this with
      | SingleElementKey key -> key.viewKey
      | MultiElementKey key -> key.viewKey

    interface IPropKey<'a> with
      member this.key = this.key
      member this.isViewKey = false

      member this.isSingleElementKey =
        match this with
        | SingleElementKey _ -> true
        | MultiElementKey _ -> false


/// Props object that is still under construction
type internal Props() =
  member val Children: List<TerminalElement> = List<_>() with get, set
  member val X: Pos option = None with get, set
  member val Y: Pos option = None with get, set
  member val XDelayed: TPos option = None with get, set
  member val YDelayed: TPos option = None with get, set

  member val dict = Dictionary<IPropKey, _>() with get

  member this.addNonTyped<'a>(k: IPropKey, v: 'a) = this.dict.Add(k, v :> obj)

  member this.add<'a>(k: IPropKey<'a>, v: 'a) = this.addNonTyped(k,v)

  member this.getOrInit<'a> (k: IPropKey<'a>) (init: unit -> 'a) : 'a =
    match this.dict.TryGetValue k with
    | true, value -> value |> unbox<'a>
    | false, _ ->
      let value = init ()
      this.dict[k] <- value :> obj
      value

  member this.remove (k: IPropKey) = this.dict.Remove k |> ignore

  member this.tryFind (key: IPropKey<'a>) =
    match this.dict.TryGetValue key with
    | true, v -> v |> unbox<'a> |> Some
    | _, _ -> None


module internal Props =

  /// <summary>Builds two new Props, the first containing the bindings for which the given predicate returns 'true', and the other the remaining bindings.</summary>
  /// <returns>A pair of Props in which the first contains the elements for which the predicate returned true and the second containing the elements for which the predicated returned false.</returns>
  let partition predicate (props: Props) =
    let first = Props()
    let second = Props()

    for kv in props.dict do
      if predicate kv then
        first.addNonTyped (kv.Key, kv.Value)
      else
        second.addNonTyped (kv.Key, kv.Value)

    first, second

  let filter predicate (props: Props) =
    let result = Props()

    for kv in props.dict do
      if predicate kv then
        result.addNonTyped (kv.Key, kv.Value)

    result

  let tryFind (key: IPropKey<'a>) (props: Props) =
    match props.dict.TryGetValue key with
    | true, v -> v |> unbox<'a> |> Some
    | _, _ -> None

  let tryFindByRawKey<'a> key (props: Props) =
    match props.dict.TryGetValue key with
    | true, v -> v |> unbox<'a> |> Some
    | _, _ -> None

  let find key (props: Props) =
    match tryFind key props with
    | Some v -> v
    | None -> failwith $"Failed to find '{key}'"

  let tryFindWithDefault (key: ISimplePropKey<'a>) defaultValue props =
    props
    |> tryFind key
    |> Option.defaultValue defaultValue

  let rawKeyExists k (p: Props) = p.dict.ContainsKey k

  let exists (k: IPropKey<'a>) (p: Props) = p.dict.ContainsKey k

  let keys (props: Props) = props.dict.Keys |> Seq.map id

  let filterSingleElementKeys (props: Props) =
    props.dict.Keys
    |> Seq.filter _.isSingleElementKey
    |> Seq.map (fun x -> x :?> ISingleElementPropKey)

  let iter iteration (props: Props) = props.dict |> Seq.iter iteration


[<AutoOpen>]
module Element =

  type internal Origin =
    | Root
    | ElmishComponent of Parent: IElmishComponentTE
    | Child of Parent: IViewTE * Index: int
    | SubElement of Parent: IViewTE * Index: int option * Property: SubElementPropKey<IViewTE>

  module internal Origin =
    let parentTerminalElement this : TerminalElement option =
      match this with
      | Root -> None
      | Child (parent, _) -> Some (TerminalElement.ViewBackedTE parent)
      | SubElement(parent, _, _) -> Some (TerminalElement.ViewBackedTE parent)
      | ElmishComponent parent -> Some (TerminalElement.ElmishComponentTE parent)

    let parentView (this: Origin) =
      match this |> parentTerminalElement with
      | Some (ElmishComponentTE parent) ->
        parent.Origin |> Origin.parentView
      | Some (ViewBackedTE parent) -> Some parent.View
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

  type internal ITerminalElementBase =
    inherit ITerminalElement
    inherit IDisposable
    abstract Origin: Origin with get, set
    abstract Name: string
    abstract View: View with get
    abstract OnViewSet: IEvent<View>
    abstract GetPath: unit -> string

  type internal IViewTE =
    inherit ITerminalElementBase

    abstract Props: Props with get
    abstract SetAsChildOfParentView: bool
    abstract Children: List<TerminalElement>

    abstract InitializeTree: origin: Origin -> unit
    abstract Reuse: prev: IViewTE -> unit


  type internal IElmishComponentTE =
    inherit ITerminalElementBase

    abstract Child: IViewTE with get

    abstract StartElmishLoop : unit -> unit
    abstract Reuse: prev: IElmishComponentTE -> unit

  type internal TerminalElement =
    | ViewBackedTE of IViewTE
    | ElmishComponentTE of IElmishComponentTE

    static member from (te: ITerminalElement) =
      match te with
      | :? IViewTE as viewTE -> ViewBackedTE viewTE
      | :? IElmishComponentTE as elmishComponentTE -> ElmishComponentTE elmishComponentTE
      | _ -> failwith "Invalid terminal element"

    member internal this.TerminalElementBase =
      match this with
      | ViewBackedTE viewTE -> viewTE :> ITerminalElementBase
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

