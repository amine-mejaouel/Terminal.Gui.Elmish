namespace Terminal.Gui.Elmish

open System
open System.Collections.Generic
open System.Collections.Specialized
open Terminal.Gui.Elmish
open Terminal.Gui.ViewBase

type internal TreeNode = {
  TerminalElement: IInternalTerminalElement
  Parent: IInternalTerminalElement option
}

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


[<AbstractClass>]
type internal TerminalElement(props: Props) =

  /// Go through the tree of TerminalElements and their `Children`, and call the given function on each node.
  let rec traverseTree (nodes: TreeNode list) (traverse: TreeNode -> unit) =

    match nodes with
    | [] -> ()
    | cur :: remainingNodes ->
      let curNode = {
        TerminalElement = cur.TerminalElement
        Parent = cur.Parent
      }

      traverse curNode

      let childNodes =
        match curNode.TerminalElement.IsElmishComponent with
        | true -> []
        | false ->
          curNode.TerminalElement.Children
          |> Seq.map (fun e -> {
            TerminalElement = e
            Parent = Some curNode.TerminalElement
          })
          |> List.ofSeq

      traverseTree (childNodes @ remainingNodes) traverse

  let mutable reused = false

  let mutable view = null
  let viewSetEvent = Event<View>()

  member this.View
    with get() = view
    and set value =
      if (view <> null) then
        failwith $"View has already been set."
      view <- value
      viewSetEvent.Trigger value

  member val ViewSet = viewSetEvent.Publish

  member val EventRegistry: PropsEventRegistry = PropsEventRegistry() with get, set

  member val Props: Props = props with get, set

  member this.children
    with get() : List<IInternalTerminalElement> =
      props
      |> Props.tryFind PKey.View.children
      |> Option.defaultValue (List<IInternalTerminalElement>())
    and set value =
      match props.tryFind PKey.View.children with
      | Some _ -> failwith "Children property has already been set."
      | None -> props.add(PKey.View.children, value)

  abstract SubElements_PropKeys: SubElementPropKey<IInternalTerminalElement> list
  default _.SubElements_PropKeys = []

  abstract newView: unit -> View

  abstract setAsChildOfParentView: bool
  default _.setAsChildOfParentView = true

  member this.SignalReuse() =
    PositionService.Current.SignalReuse this
    reused <- true

  member this.InitializeView() =
#if DEBUG
    Diagnostics.Trace.WriteLine $"{this.name} created!"
#endif
    this.View <- this.newView ()

    this.initializeSubElements()
    |> Seq.iter this.Props.addNonTyped

    this.setProps (this, this.Props)

  abstract reuse: prev: IInternalTerminalElement -> unit

  abstract name: string

  member this.InitializeTree(parent: IInternalTerminalElement option) : unit =
    let traverse (node: TreeNode) =

      node.TerminalElement.InitializeView ()

      // Here, the "children" views are added to their parent
      if node.TerminalElement.SetAsChildOfParentView then
        node.Parent
        |> Option.map _.View
        |> Option.iter (fun v -> v.Add node.TerminalElement.View |> ignore)

    traverseTree
      [
        {
          TerminalElement = this
          Parent = parent
        }
      ]
      traverse

  /// For each '*.element' prop, initialize the Tree of the element and then return the sub element: (proPKey * View)
  member this.initializeSubElements () : (IPropKey * obj) seq =
    seq {
      for x in this.SubElements_PropKeys do
        match this.Props |> Props.tryFindByRawKey<obj> x with

        | None -> ()

        | Some value ->
          match value with
          | :? TerminalElement as subElement ->
            subElement.InitializeTree (Some this)

            let viewKey = x.viewKey

            yield viewKey, subElement.View
          | :? List<IInternalTerminalElement> as elements ->
            elements
            |> Seq.iter (fun e -> e.InitializeTree (Some this))

            let viewKey = x.viewKey

            let views =
              elements |> Seq.map _.View |> Seq.toList

            yield viewKey, views
          | _ -> failwith "Out of range subElement type"
    }

  member this.trySetEventHandler<'TEventArgs> (k: IEventPropKey<'TEventArgs -> unit>, event: IEvent<EventHandler<'TEventArgs>,'TEventArgs>) =

    this.tryRemoveEventHandler k

    this.Props.tryFind k
    |> Option.iter (fun action -> this.EventRegistry.setEventHandler(k, event, action))

  member this.trySetEventHandler (k: IEventPropKey<EventArgs -> unit>, event: IEvent<EventHandler,EventArgs>) =

    this.tryRemoveEventHandler k

    this.Props.tryFind k
    |> Option.iter (fun action -> this.EventRegistry.setEventHandler(k, event, action))

  member this.trySetEventHandler (k: IEventPropKey<NotifyCollectionChangedEventArgs -> unit>, event: IEvent<NotifyCollectionChangedEventHandler,NotifyCollectionChangedEventArgs>) =

    this.tryRemoveEventHandler k

    this.Props.tryFind k
    |> Option.iter (fun action -> this.EventRegistry.setEventHandler(k, event, action))

  member this.tryRemoveEventHandler (k: IPropKey) =
    this.EventRegistry.removeHandler k

  abstract setProps: terminalElement: IInternalTerminalElement * props: Props -> unit

  default this.setProps (terminalElement: IInternalTerminalElement, props: Props) =
    // Custom Props
    props
    |> Props.tryFind PKey.View.X_delayedPos
    // TODO: too confusing here, too difficult to reason about, need to refactor
    |> Option.iter (fun tPos -> PositionService.Current.ApplyPos(terminalElement, tPos, (fun view pos -> view.X <- pos)))

    props
    |> Props.tryFind PKey.View.Y_delayedPos
    |> Option.iter (fun tPos -> PositionService.Current.ApplyPos(terminalElement, tPos, (fun view pos -> view.Y <- pos)))

  // TODO: Is the view needed as param ? is the props needed as param ?
  abstract removeProps: terminalElement: IInternalTerminalElement * props: Props -> unit

  /// Reuses:
  /// // TODO: outdated documentation
  /// - Previous `View`, while updating its properties to match the current TerminalElement properties.
  /// - But also other Views that are sub elements of the previous `ITerminalElement` and made available in the `prevProps`.
  override this.reuse prev =

    let prev = prev :?> TerminalElement

    prev.SignalReuse()

    // TODO: it seems that comparing x_delayedPos/y_delayedPos is working well
    // TODO: this should be tested and documented to make sure that it continues to work well in the future.

    this.View <- prev.View
    this.EventRegistry <- prev.EventRegistry

    let c = TerminalElement.compare prev.Props this.Props

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

    this.removeProps (this, removedProps)
    this.setProps (this, c.changedProps)

  member this.equivalentTo(other: TerminalElement) =
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
          kv.Value :?> TerminalElement

        let otherElement =
          other.Props
          |> Props.tryFindByRawKey kv.Key
          |> Option.map (fun (x: obj) -> x :?> TerminalElement)

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
            kv.Value :?> TerminalElement

          let oldElement = v :?> TerminalElement
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
    if (not reused) then

      // Remove any event subscriptions
      this.removeProps (this, this.Props)

      this.View |> Interop.removeFromParent
      // Dispose SubElements (Represented as `View` typed properties of the View, that are not children)
      for key in this.SubElements_PropKeys do
        this.Props
        |> Props.tryFind key
        |> Option.iter (fun subElement ->
          subElement.Dispose())

      for child in this.children do
        child.Dispose()

      PositionService.Current.SignalDispose(this)
      // Finally, dispose the View itself
      this.View.Dispose()

  interface IInternalTerminalElement with
    member this.InitializeView() = this.InitializeView()
    member this.InitializeTree(parent) = this.InitializeTree parent
    member this.Reuse prevElementData = this.reuse prevElementData
    member this.Parent = None
    member this.View = this.View
    member this.Name = this.name

    member this.SetAsChildOfParentView =
      this.setAsChildOfParentView

    member this.Children = this.children

    member this.IsElmishComponent = false

    member this.Props = this.Props

    member this.ViewSet = this.ViewSet

    member this.Dispose() = this.Dispose()

