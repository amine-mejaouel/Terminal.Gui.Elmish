namespace Terminal.Gui.Elmish

open System.Collections.Generic

/// Represents a property in a Terminal.Gui View
type PKey<'a> = | PKey of string
with
    member this.value =
        let (PKey value) = this
        value

/// Props object that is still under construction
type Props(?initialProps) =

    member val dict = defaultArg initialProps (Dictionary<_,_>()) with get

    member this.add (k: string, v: obj)  = this.dict.Add(k, v)
    member this.add<'a> (PKey k: PKey<'a>, v: 'a)  = this.dict.Add(k, v :> obj)

    member this.getOrInit<'a> (PKey k: PKey<'a>) (init: unit -> 'a) : 'a =
        match this.dict.TryGetValue k with
        | true, value -> value |> unbox<'a>
        | false, _ ->
            let value = init()
            this.dict[k] <- value :> obj
            value

module Props =
    let merge (props': Props) (props'': Props) =
        let result = Dictionary()
        let addToResult (source: Props) =
            source.dict |> Seq.iter (fun kv -> result.Add(kv.Key, kv.Value))

        addToResult props'
        addToResult props''

        Props(result)

    /// <summary>Builds two new Props, one containing the bindings for which the given predicate returns 'true', and the other the remaining bindings.</summary>
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

    let tryFind (PKey key: PKey<'a>) (props: Props) =
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

    let tryFindWithDefault (key: PKey<'a>) defaultValue props =
        props |> tryFind key |> Option.defaultValue defaultValue

    let rawKeyExists k (p: Props) = p.dict.ContainsKey k

    let exists (PKey k) (p: Props) = p.dict.ContainsKey k

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
