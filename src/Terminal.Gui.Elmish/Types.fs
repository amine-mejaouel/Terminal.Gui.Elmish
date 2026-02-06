namespace Terminal.Gui.Elmish

open System
open System.Collections.Generic
open System.Threading.Tasks
open Elmish
open Terminal.Gui.App
open Terminal.Gui.ViewBase
open Terminal.Gui.Views


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
      static member event key = EventPropKey.create key
      static member view key = ViewPropKey.create key
      static member delayedPos key = DelayedPosKey.create key

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

  member this.remove (k: IPropKey) = this.dict.Remove k |> ignore

  member this.tryFind (key: IPropKey<'a>) =
    match this.dict.TryGetValue key with
    | true, v -> v |> unbox<'a> |> Some
    | _, _ -> None


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
module rec Element =

  /// Represents the relationship between a TerminalElement and its parent.
  type internal Origin =
    | Root
    // TODO: add index of child element
    | Child of Parent: IInternalTerminalElement * Index: int
    | SubElement of Parent: IInternalTerminalElement * Index: int option * Property: SubElementPropKey<IInternalTerminalElement>
    member this.Parent =
      match this with
      | Root -> None
      | Child (parent, _) -> Some parent
      | SubElement(parent, _, _) -> Some parent

    override this.ToString() =
        let parentId =
          match this.Parent with
          | Some parent -> parent.Id.ToString()
          | None -> "root"

        let propIdStr =
          match this with
          | Root -> ""
          | Child _ -> "child"
          | SubElement(_, _, subElementPropKey) -> $"{subElementPropKey.key}"

        let indexStr =
          match this with
          | Root -> ""
          | Child(_, index) -> $"[{index}]"
          | SubElement(_, index, _) -> index |> Option.map (sprintf "[%i]") |> Option.defaultValue ""

        $"{parentId}|{propIdStr}{indexStr}"

  type internal TerminalElementId =
    {
      ExplicitId: string option
      Origin: Origin
    }

    static member Null = { ExplicitId = None; Origin = Root }

    override this.ToString() =
      match this.ExplicitId with
      | Some id -> id
      | None -> this.Origin.ToString()

  and internal IInternalTerminalElement =
    inherit ITerminalElement
    inherit IDisposable
    abstract InitializeView: unit -> unit
    abstract InitializeTree: origin: Origin -> unit
    abstract Reuse: prev: IInternalTerminalElement -> unit
    abstract Id: TerminalElementId with get, set
    abstract Props: Props with get
    abstract View: View with get
    abstract Name: string
    abstract SetAsChildOfParentView: bool
    abstract Children: List<IInternalTerminalElement>
    abstract IsElmishComponent: bool with get
    abstract ViewSet: IEvent<View>

[<AutoOpen>]
module Elmish =

  [<RequireQualifiedAccess>]
  type internal RootView =
    /// Application root view, there is one single instance of these in the application.
    | AppRootView of Runnable
    /// Elmish component root view, there can be multiple instances of these in the application.
    | ComponentRootView of View

  type internal InternalModel<'model> = {
    mutable CurrentTreeState: IInternalTerminalElement option
    mutable Application: IApplication
    RootView: TaskCompletionSource<RootView>
    Termination: TaskCompletionSource
    /// Elmish model provided to the Program by the library caller.
    ClientModel: 'model
  }

  type ElmishTerminalProgram<'arg, 'model, 'msg, 'view> = internal ElmishTerminalProgram of Program<'arg, InternalModel<'model>, 'msg, 'view>

  type internal IElmishComponent_TerminalElement =
    abstract StartElmishLoop: unit -> unit
    inherit ITerminalElement
    inherit IInternalTerminalElement

  /// <summary>
  /// <para>Wrapper that elmish components should use to expose themselves as IInternalTerminalElement.</para>
  /// <para>As the Elmish component handles its own initialization and children management in his separate Elmish loop,
  /// this wrapper will hide these aspects to the outside world. Thus preventing double initialization or double children management.</para>
  /// </summary>
  type internal ElmishComponent_TerminalElement<'model, 'msg, 'view>(program: Program<unit,InternalModel<'model>, 'msg, 'view>) =

    let mutable terminalElement: IInternalTerminalElement = Unchecked.defaultof<_>

    let runComponent program =

      let waitForView = TaskCompletionSource()

      let runComponent (model: InternalModel<_>) =
        let start dispatch =
          (task {
            // On program startup, Wait for the Elmish loop to take care of creating the root view.
            let! _ = model.RootView.Task
            terminalElement <- model.CurrentTreeState.Value
            waitForView.SetResult()
          }).GetAwaiter().GetResult()

          { new IDisposable with member _.Dispose() = () }

        start

      let subscribe model = [ [ "runComponent" ], runComponent model ]

      program
      |> Program.withSubscription subscribe
      |> Program.run

      waitForView.Task.GetAwaiter().GetResult()

      ()

    member this.TerminalElement =
      if terminalElement = Unchecked.defaultof<_> then
        failwith "Elmish loop has not been started yet. Call StartElmishLoop before accessing the View."
      else
        terminalElement.View

    interface IElmishComponent_TerminalElement with
      member this.StartElmishLoop() = runComponent program

    interface IInternalTerminalElement with
      member this.InitializeView() = () // Do nothing, initialization is handled by the Elmish component
      member this.InitializeTree(origin) = () // Do nothing, initialization is handled by the Elmish component
      member this.Reuse prevElementData = terminalElement.Reuse prevElementData
      member this.Id with get() = terminalElement.Id and set v = terminalElement.Id <- v
      member this.View = terminalElement.View

      member this.Name = terminalElement.Name

      // Children are managed by the Elmish component itself. Hence they are hidden to the outside.
      member this.SetAsChildOfParentView = terminalElement.SetAsChildOfParentView

      member this.IsElmishComponent = true

      member this.Dispose() = terminalElement.Dispose()

      member this.Children = List<IInternalTerminalElement>()
      member this.Props = failwith "ElmishComponent_TerminalElement_Wrapper does not expose Props"
      member this.ViewSet = terminalElement.ViewSet
