namespace Terminal.Gui.Elmish

open System.Collections.Generic

type IPropertyKey<'a> =
    abstract member key: string

/// Represents a property in a Terminal.Gui View
type SimplePropertyKey<'a> = | SimplePropertyKey of string
with
    interface IPropertyKey<'a> with
        member this.key =
            let (SimplePropertyKey key) = this
            key

type SingleElementKey<'a> = private Key of string
with
    static member create<'a> (key:string) : SingleElementKey<'a> =
        if key.EndsWith "_element" then
            Key key
        else failwith $"Invalid key: {key}"

    member this.key =
        let (Key key) = this
        key

    interface IPropertyKey<'a> with
        member this.key = this.key

type MultiElementKey<'a> = private Key of string
with
    static member create<'a> (key:string) : MultiElementKey<'a> =
        if key.EndsWith "_elements" then
            Key key
        else failwith $"Invalid key: {key}"

    member this.key =
        let (Key key) = this
        key

    interface IPropertyKey<'a> with
        member this.key = this.key

type ElementKey<'a> =
    | SingleElementKey of SingleElementKey<'a>
    | MultiElementKey of MultiElementKey<'a>

    static member createSingleElementKey<'a> (key: string) : ElementKey<'a> =
        SingleElementKey(SingleElementKey.create<'a> key)

    static member createMultiElementKey<'a> (key: string) : ElementKey<'a> =
        MultiElementKey(MultiElementKey.create<'a> key)

    static member from (singleElementKey: SingleElementKey<'a>) : ElementKey<'b> =
        ElementKey<'b>.createSingleElementKey (singleElementKey :> IPropertyKey<'a>).key

    static member from (multiElementKey: MultiElementKey<'a>) : ElementKey<'b> =
        ElementKey<'b>.createMultiElementKey (multiElementKey :> IPropertyKey<'a>).key

    member this.key =
        match this with
        | SingleElementKey key -> key.key
        | MultiElementKey key -> key.key

    member this.viewKey =
        match this with
        | SingleElementKey key -> key.key.Replace("_element", "_view")
        | MultiElementKey key -> key.key.Replace("_elements", "_view")

    interface IPropertyKey<'a> with
        member this.key = this.key

/// Props object that is still under construction
type Props(?initialProps) =

    member val dict = defaultArg initialProps (Dictionary<_,_>()) with get

    member this.add (k: string, v: obj)  = this.dict.Add(k, v)
    member this.add<'a> (k: IPropertyKey<'a>, v: 'a)  = this.dict.Add(k.key, v :> obj)

    member this.getOrInit<'a> (k: IPropertyKey<'a>) (init: unit -> 'a) : 'a =
        match this.dict.TryGetValue k.key with
        | true, value -> value |> unbox<'a>
        | false, _ ->
            let value = init()
            this.dict[k.key] <- value :> obj
            value

module Props =
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
                first.add (kv.Key, kv.Value)
            else
                second.add (kv.Key, kv.Value)

        first, second

    let filter predicate (props: Props) =
        let result = Props()

        for kv in props.dict do
            if predicate kv then
                result.add (kv.Key, kv.Value)

        result

    let tryFind (SimplePropertyKey key: SimplePropertyKey<'a>) (props: Props) =
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

    let tryFindWithDefault (key: SimplePropertyKey<'a>) defaultValue props =
        props |> tryFind key |> Option.defaultValue defaultValue

    let rawKeyExists k (p: Props) = p.dict.ContainsKey k

    let exists (SimplePropertyKey k) (p: Props) = p.dict.ContainsKey k

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

    let iter iteration (props: Props) =
        props.dict |> Seq.iter iteration
