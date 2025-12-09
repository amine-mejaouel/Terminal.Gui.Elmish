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

  type IDelayedPosKey =
    inherit IPropKey<TPos>

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

  /// Mainly used for positions that are relative to other views.
  /// These positions take in a `ITerminalElement` as parameter.
  /// Evaluating such positions will be done on the `View.OnDrawComplete` event.
  [<CustomEquality; NoComparison>]
  type private DelayedPosKey =
    private
    | Key of string

    static member create(key: string) : IDelayedPosKey =
      if key.EndsWith "_delayedPos" then
        Key key
      else
        failwith $"Invalid key: {key}"

    member private this.key =
      let (Key key) = this
      key

    override this.GetHashCode() = this.key.GetHashCode()

    override this.Equals(obj) =
      match obj with
      | :? IPropKey as x -> this.key.Equals(x.key)
      | _ -> false

    interface IDelayedPosKey with
      member this.key = this.key
      member this.isViewKey = false
      member this.isSingleElementKey = false

  [<RequireQualifiedAccess>]
  module PropKey =
    type Create =
      static member singleElement key = SingleElementPropKey.create key
      static member multiElement key = MultiElementPropKey.create key
      static member simple key = SimplePropKey.create key
      static member view key = ViewPropKey.create key
      static member delayedPos key = DelayedPosKey.create key


/// Props object that is still under construction
type internal Props(?initialProps) =

  member val dict = defaultArg initialProps (Dictionary<IPropKey, _>()) with get

  member this.add<'a>(k: IPropKey<'a>, v: 'a) = this.dict.Add(k, v :> obj)
  member this.addNonTyped<'a>(k: IPropKey, v: 'a) = this.dict.Add(k, v :> obj)

  member this.getOrInit<'a> (k: IPropKey<'a>) (init: unit -> 'a) : 'a =
    match this.dict.TryGetValue k with
    | true, value -> value |> unbox<'a>
    | false, _ ->
      let value = init ()
      this.dict[k] <- value :> obj
      value

module internal Props =
  let merge (props': Props) (props'': Props) =
    let result = Dictionary()

    let addToResult (source: Props) =
      source.dict
      |> Seq.iter (fun kv -> result.Add(kv.Key, kv.Value))

    addToResult props'
    addToResult props''

    Props(result)

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

  type internal IInternalTerminalElement =
    inherit ITerminalElement
    inherit IDisposable
    abstract initialize: unit -> unit
    abstract initializeTree: parent: View option -> unit
    abstract canReuseView: prevView: View -> prevProps: Props -> bool
    abstract reuse: prevView: View -> prevProps: Props -> unit
    abstract onDrawComplete: IEvent<View>
    abstract children: List<IInternalTerminalElement> with get
    abstract view: View with get
    abstract props: Props
    abstract name: string
    abstract setAsChildOfParentView: bool
    abstract parent: View option with get, set
    abstract detachView: unit -> Result<View, string>

  type IMenuElement =
    inherit ITerminalElement

  type IPopoverMenuElement =
    inherit ITerminalElement

  type IMenuBarItemElement =
    inherit ITerminalElement

  type INumericUpDownElement =
    inherit ITerminalElement

  type ISliderElement =
    inherit ITerminalElement

  type ITreeViewElement =
    inherit ITerminalElement

  /// <summary>
  /// <para>Wrapper that elmish components should use to expose themselves as IInternalTerminalElement.</para>
  /// <para>As the Elmish component handles its own initialization and children management in his separate Elmish loop,
  /// this wrapper will hide these aspects to the outside world. Thus preventing double initialization or double children management.</para>
  /// </summary>
  type internal ElmishComponent_TerminalElement_Wrapper(element: IInternalTerminalElement) =
    interface IInternalTerminalElement with
      member this.initialize() = () // Do nothing, initialization is handled by the Elmish component
      member this.initializeTree(parent) = () // Do nothing, initialization is handled by the Elmish component
      member this.canReuseView prevView prevProps = element.canReuseView prevView prevProps
      member this.reuse prevView prevProps = element.reuse prevView prevProps
      member this.view = element.view
      member this.props = element.props
      member this.name = element.name
      // Children are managed by the Elmish component itself. Hence they are hidden to the outside.
      member this.children = new System.Collections.Generic.List<IInternalTerminalElement>()
      member this.setAsChildOfParentView = element.setAsChildOfParentView
      member this.onDrawComplete = element.onDrawComplete

      member this.parent = element.parent
      member this.parent with set value = element.parent <- value

      member this.Dispose() = element.Dispose()

      member this.detachView() = failwith "Operation not supported. View handling is managed by the Elmish component itself."
