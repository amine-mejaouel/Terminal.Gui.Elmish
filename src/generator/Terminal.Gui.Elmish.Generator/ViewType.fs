[<RequireQualifiedAccess>]
module Terminal.Gui.Elmish.Generator.ViewType

open System

let private inheritanceChain (viewType: Type) =
  let rec collectChain (t: Type) (acc: Type list) =
    match t.BaseType with
    | null -> acc
    | baseType when baseType = typeof<Terminal.Gui.ViewBase.View> -> acc
    | baseType -> collectChain baseType (baseType :: acc)

  collectChain viewType []
  |> List.map _.Name

let private  isSubChainOf (subChain: 'T list) (superChain: 'T list) =
  let rec checkChains subChain superChain =
    match subChain, superChain with
    | [], _ -> true
    | _, [] -> false
    | sh :: st, ph :: pt ->
        if sh = ph then
          checkChains st pt
        else
          false

  checkChains subChain superChain

let private viewTypes =
  typeof<Terminal.Gui.ViewBase.View>.Assembly.GetTypes()
  |> Seq.filter _.IsAssignableTo(typeof<Terminal.Gui.ViewBase.View>)
  |> Seq.sortBy _.Name
  |> Seq.toList

let private inheritanceChains =
  viewTypes
  |> Seq.fold (fun inhChains t ->
      let tChain = inheritanceChain t
      let isSubChain = inhChains |> List.exists (isSubChainOf tChain)
      let isSuperChain = inhChains |> List.exists (fun chain -> isSubChainOf chain tChain)
      if isSubChain then
        inhChains
      else if isSuperChain then
        inhChains
        |> List.filter (fun chain -> not (isSubChainOf chain tChain))
        |> List.append [ tChain ]
      else
        inhChains @ [ tChain ]
  ) []

let viewTypesOrderedByInheritance =

  let parentIsReturned (returnedTypes: System.Collections.Generic.List<Type>) (viewType: Type) =
    match viewType.BaseType with
    | null -> true
    | baseType when baseType = typeof<Terminal.Gui.ViewBase.View> -> true
    | baseType ->
        returnedTypes.Contains baseType

  let returnedTypes = System.Collections.Generic.List<Type>()
  let pendingTypes = System.Collections.Generic.List<Type>()
  seq {
    yield typeof<Terminal.Gui.ViewBase.View>

    for viewType in viewTypes do
      if parentIsReturned returnedTypes viewType then
        returnedTypes.Add viewType
        yield viewType
      else
        pendingTypes.Add viewType

      let mutable iterate = true
      while iterate do
        let readyTypes =
          pendingTypes
          |> Seq.filter (parentIsReturned returnedTypes)
          |> Seq.toArray
        if readyTypes.Length = 0 then
          iterate <- false
        else
          for readyType in readyTypes do
            pendingTypes.Remove readyType |> ignore
            returnedTypes.Add readyType
            yield readyType
  }

let parentViewType (viewType: Type) =
  let baseType = viewType.BaseType
  if baseType.IsSubclassOf typeof<Terminal.Gui.ViewBase.View>
    || baseType = typeof<Terminal.Gui.ViewBase.View> then
    Some baseType
  else
    None

let properties (viewType: Type) =
  viewType.GetProperties(
    System.Reflection.BindingFlags.Public |||
    System.Reflection.BindingFlags.Instance |||
    System.Reflection.BindingFlags.DeclaredOnly)
  |> Array.filter (fun p -> p.CanRead && p.CanWrite)
  |> Array.sortBy _.Name

let events (viewType: Type) =
  viewType.GetEvents(
    System.Reflection.BindingFlags.Public |||
    System.Reflection.BindingFlags.Instance |||
    System.Reflection.BindingFlags.DeclaredOnly)
  |> Array.filter (fun e -> e.AddMethod.IsPublic && e.RemoveMethod.IsPublic)
  |> Array.sortBy _.Name
