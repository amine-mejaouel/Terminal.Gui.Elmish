namespace Terminal.Gui.Elmish

open System.Collections.Generic

// TODO: could be removed no ?
type IProperty = interface end
type IMenuBarProperty = interface inherit IProperty end
type IMenuProperty = interface inherit IProperty end
type IStyle = interface end

type ITabProperty = interface inherit IProperty end
type ITabItemProperty = interface inherit IProperty end

type IMenuBar = interface end
type IMenuBarItem = interface end
type IMenu = interface end

type KeyValue =
    KeyValue of (string * obj)
        interface IProperty
        interface IMenuBarProperty
        interface IMenuProperty
        interface ITabProperty
        interface ITabItemProperty
        interface IMenuBarItem
        interface IMenuBar
        interface IMenu
        interface IStyle

type IProps =
    interface
        abstract dict : IReadOnlyDictionary<string, obj>
    end

type IIncrementalProps =
    interface
        inherit IProps
        abstract add : key: string -> value: obj -> unit
        abstract getOrInit<'a> : key: string -> init: (unit -> 'a) -> 'a
    end

/// Props object that is still under construction
type IncrementalProps(?initialProps) =

    let dict = defaultArg initialProps (Dictionary<_,_>())

    member _.add k v  = dict.Add(k, v)
    member _.getOrInit<'a> k (init: unit -> 'a) : 'a =
        match dict.TryGetValue k with
        | true, value -> value |> unbox<'a>
        | false, _ ->
            let value = init()
            dict[k] <- value :> obj
            value

    interface IIncrementalProps with
        member this.add k v = this.add k v
        member this.getOrInit<'a> k v = this.getOrInit<'a> k v

    interface IProps with
        member _.dict with get() = dict

/// Props where property list is frozen but each property held value may be changed.
type Props(incProps: IncrementalProps) =
    interface IProps with
        member _.dict = (incProps :> IProps).dict

module Props =
    let merge (props': IProps) (props'': IProps) =
        let result = Dictionary()
        let addToResult (source: IProps) =
            source.dict |> Seq.iter (fun kv -> result.Add(kv.Key, kv.Value))

        addToResult props'
        addToResult props''

        // TODO: refine this
        IncrementalProps(result) :> IProps

    /// <summary>Builds two new Props, one containing the bindings for which the given predicate returns 'true', and the other the remaining bindings.</summary>
    /// <returns>A pair of Props in which the first contains the elements for which the predicate returned true and the second containing the elements for which the predicated returned false.</returns>
    let partition predicate (props: IProps) =
        let first = IncrementalProps() :> IIncrementalProps
        let second = IncrementalProps() :> IIncrementalProps

        for kv in props.dict do
            if predicate kv then
                first.add kv.Key kv.Value
            else
                second.add kv.Key kv.Value

        first, second

    let filter predicate (props: IProps) =
        let result = IncrementalProps() :> IIncrementalProps

        for kv in props.dict do
            if predicate kv then
                result.add kv.Key kv.Value

        result

    let tryFind<'a> key (props: IProps) =
        match props.dict.TryGetValue key with
        | true, v -> v |> unbox<'a> |> Some
        | _, _ -> None

    let tryFindWithDefault<'a> key defaultValue props =
        props |> tryFind<'a> key |> Option.defaultValue defaultValue

    let keyExists k (p: IProps) = p.dict.ContainsKey k
