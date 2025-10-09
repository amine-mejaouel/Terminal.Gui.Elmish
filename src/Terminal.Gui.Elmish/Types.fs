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

// TODO: document that it is mutable
// Track when the mutation is dangerous, and put boundaries there
type Props = | Props of Dictionary<string, obj> with

    member private this.dict =
        let (Props dict) = this
        dict

    /// Mutates the current Props, adding the key/value.
    member this.add k v  = this.dict.Add(k, v)

module Props =
    let init() = Props (Dictionary())

    let merge props' props'' =
        let result = Dictionary()
        let addToResult (Props source) =
            source |> Seq.iter (fun kv -> result.Add(kv.Key, kv.Value))

        addToResult props'
        addToResult props''

        (Props result)

    /// <summary>Builds two new Props, one containing the bindings for which the given predicate returns 'true', and the other the remaining bindings.</summary>
    /// <returns>A pair of Props in which the first contains the elements for which the predicate returned true and the second containing the elements for which the predicated returned false.</returns>
    let partition predicate (Props props) =
        let first = init()
        let second = init()

        for kv in props do
            if predicate kv then
                first.add kv.Key kv.Value
            else
                second.add kv.Key kv.Value

        first, second

    let filter predicate (Props props) =
        let result = init()

        for kv in props do
            if predicate kv then
                result.add kv.Key kv.Value

        result

    let tryFind<'a> key (Props props) =
        match props.TryGetValue key with
        | true, v -> v |> unbox<'a> |> Some
        | _, _ -> None

    let tryFindWithDefault<'a> key defaultValue props =
        props |> tryFind<'a> key |> Option.defaultValue defaultValue

    let keyExists k (Props p) = p.ContainsKey k
