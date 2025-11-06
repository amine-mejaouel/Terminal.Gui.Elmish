namespace Terminal.Gui.Elmish

open System.Collections.Generic

[<AutoOpen>]
module internal PropKey =
    type IPropKey =
        abstract member key: string
        abstract member isViewKey: bool
        abstract member isSingleElementKey: bool

    type IPropKey<'a> =
        inherit IPropKey

    type ISingleElementKey =
        abstract member viewKey: IPropKey

    /// Represents a property in a Terminal.Gui View
    [<CustomEquality; NoComparison>]
    type SimplePropKey<'a> = private Key of string
    with
        static member create<'a> (key:string) : SimplePropKey<'a> =
            if key.EndsWith "_element" || key.EndsWith "_elements" || key.EndsWith "_view" then
                failwith $"Invalid key: {key}"
            else
                Key key

        static member map (value: SimplePropKey<'a>) : SimplePropKey<'b> =
            Key value.key

        member this.key =
            let (Key key) = this
            key

        override this.GetHashCode() = this.key.GetHashCode()
        override this.Equals(obj) =
            match obj with
            | :? IPropKey as x ->
                this.key.Equals(x.key)
            | _ ->
                false

        interface IPropKey<'a> with
            member this.key = this.key
            member this.isViewKey = false
            member this.isSingleElementKey = false

    [<CustomEquality; NoComparison>]
    type ViewPropKey<'a> = private Key of string
    with
        static member create<'a> (key:string) : ViewPropKey<'a> =
            if not (key.EndsWith "_view") then
                failwith $"Invalid key: {key}"
            else
                Key key

        static member map (value: ViewPropKey<'a>) : ViewPropKey<'b> =
            Key value.key

        member this.key =
            let (Key key) = this
            key

        override this.GetHashCode() = this.key.GetHashCode()
        override this.Equals(obj) =
            match obj with
            | :? IPropKey as x ->
                this.key.Equals(x.key)
            | _ ->
                false

        interface IPropKey<'a> with
            member this.key = this.key
            member this.isViewKey = true
            member this.isSingleElementKey = false

    [<CustomEquality; NoComparison>]
    type SingleElementPropKey<'a> = private Key of string
    with
        static member create<'a> (key:string) : SingleElementPropKey<'a> =
            if key.EndsWith "_element" then
                Key key
            else failwith $"Invalid key: {key}"

        member this.key =
            let (Key key) = this
            key

        member this.viewKey=
            ViewPropKey.create (this.key.Replace("_element", "_view")) :> IPropKey

        override this.GetHashCode() = this.key.GetHashCode()
        override this.Equals(obj) =
            match obj with
            | :? IPropKey as x ->
                this.key.Equals(x.key)
            | _ ->
                false

        interface IPropKey<'a> with
            member this.key = this.key
            member this.isViewKey = false
            member this.isSingleElementKey = true

        interface ISingleElementKey with
            member this.viewKey = this.viewKey

    [<CustomEquality; NoComparison>]
    type MultiElementPropKey<'a> = private Key of string
    with
        static member create<'a> (key:string) : MultiElementPropKey<'a> =
            if key.EndsWith "_elements" then
                Key key
            else failwith $"Invalid key: {key}"

        member this.key =
            let (Key key) = this
            key

        member this.viewKey=
            ViewPropKey.create (this.key.Replace("_elements", "_view")) :> IPropKey

        override this.GetHashCode() = this.key.GetHashCode()
        override this.Equals(obj) =
            match obj with
            | :? IPropKey as x ->
                this.key.Equals(x.key)
            | _ ->
                false

        interface IPropKey<'a> with
            member this.key = this.key
            member this.isViewKey = false
            member this.isSingleElementKey = false

    [<CustomEquality; NoComparison>]
    type ElementPropKey<'a> =
        | SingleElementKey of SingleElementPropKey<'a>
        | MultiElementKey of MultiElementPropKey<'a>

        static member createSingleElementKey<'a> (key: string) : ElementPropKey<'a> =
            SingleElementKey(SingleElementPropKey.create<'a> key)

        static member createMultiElementKey<'a> (key: string) : ElementPropKey<'a> =
            MultiElementKey(MultiElementPropKey.create<'a> key)

        static member from (singleElementKey: SingleElementPropKey<'a>) : ElementPropKey<'b> =
            ElementPropKey<'b>.createSingleElementKey (singleElementKey :> IPropKey<'a>).key

        static member from (multiElementKey: MultiElementPropKey<'a>) : ElementPropKey<'b> =
            ElementPropKey<'b>.createMultiElementKey (multiElementKey :> IPropKey<'a>).key

        member this.key =
            match this with
            | SingleElementKey key -> key.key
            | MultiElementKey key -> key.key

        override this.GetHashCode() = this.key.GetHashCode()
        override this.Equals(obj) =
            match obj with
            | :? IPropKey as x ->
                this.key.Equals(x.key)
            | _ ->
                false

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

    member val dict = defaultArg initialProps (Dictionary<IPropKey,_>()) with get

    member this.add<'a> (k: IPropKey<'a>, v: 'a)  = this.dict.Add(k, v :> obj)
    member this.addNonTyped<'a> (k: IPropKey, v: 'a)  = this.dict.Add(k, v :> obj)

    member this.getOrInit<'a> (k: IPropKey<'a>) (init: unit -> 'a) : 'a =
        match this.dict.TryGetValue k with
        | true, value -> value |> unbox<'a>
        | false, _ ->
            let value = init()
            this.dict[k] <- value :> obj
            value

module internal Props =
    let merge (props': Props) (props'': Props) =
        let result = Dictionary()
        let addToResult (source: Props) =
            source.dict |> Seq.iter (fun kv -> result.Add(kv.Key, kv.Value))

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

    let tryFindWithDefault (key: SimplePropKey<'a>) defaultValue props =
        props |> tryFind key |> Option.defaultValue defaultValue

    let rawKeyExists k (p: Props) = p.dict.ContainsKey k

    let exists (k: IPropKey<'a>) (p: Props) = p.dict.ContainsKey k

    // TODO: remove this and replace usage with TerminalElement.compare where
    let compare (oldProps: Props) (newProps: Props) =

        let remainingOldProps, removedProps =
            oldProps |> partition (fun kv -> newProps |> rawKeyExists kv.Key)

        let changedProps =
            newProps |> filter (fun kv ->
                match remainingOldProps |> tryFindByRawKey kv.Key with
                | _ when kv.Value = "children" ->
                    false
                | Some v' when kv.Value = v' ->
                    false
                | _ ->
                    true
            )

        (changedProps, removedProps)

    let keys (props: Props) = props.dict.Keys |> Seq.map id

    let filterSingleElementKeys (props: Props) =
        props.dict.Keys
        |> Seq.filter _.isSingleElementKey
        |> Seq.map (fun x -> x :?> ISingleElementKey)

    let iter iteration (props: Props) =
        props.dict |> Seq.iter iteration

[<AutoOpen>]
module Element =
    type ITerminalElement = interface end

    open Terminal.Gui.ViewBase

    // TODO: all concrete Element(s) could be made internal, leaving only the interface as public
    // TODO:  ie make all classes internal / expose public interface instead
    type internal IInternalTerminalElement =
        inherit ITerminalElement
        abstract initialize: parent: View option -> unit
        abstract initializeTree: parent: View option -> unit
        // TODO: rename to prevView
        abstract canUpdate: prevElement:View -> oldProps: Props -> bool
        // TODO: rename to prevView
        abstract update: prevElement:View -> oldProps: Props -> unit
        abstract layout: unit -> unit
        abstract children: List<IInternalTerminalElement> with get
        abstract view: View with get
        abstract props: Props
        abstract name: string

    type IMenuv2Element =
        inherit ITerminalElement

    type IPopoverMenuElement =
        inherit ITerminalElement

    type IMenuBarItemv2Element =
        inherit ITerminalElement

    type INumericUpDownElement =
        inherit ITerminalElement

    type ISliderElement =
        inherit ITerminalElement

    type ITreeViewElement =
        inherit ITerminalElement

[<RequireQualifiedAccess>]
type TPos =
    | Top of ITerminalElement
    | Bottom of ITerminalElement

