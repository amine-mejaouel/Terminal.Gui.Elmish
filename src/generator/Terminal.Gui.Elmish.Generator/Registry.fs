namespace Terminal.Gui.Elmish.Generator

open System

type Registry =
  static member val private TypesNames = System.Collections.Generic.Dictionary<string, string>()
  static member val NeededIElementInterfaces = System.Collections.Generic.HashSet<Type>()
  /// Especially useful to generate unique names for generic types that may also exist with same name but different generic parameters
  static member GetUniqueTypeName(viewType: Type) =
    match Registry.TypesNames.TryGetValue(viewType.FullName) with
    | true, pkey -> pkey
    | _ ->
        let uniquePKey =
          let pkeyCandidate =
            viewType
            |> ViewType.cleanTypeName
          let rec findUniquePKey candidate =
            if Registry.TypesNames.ContainsValue candidate then
              findUniquePKey (candidate + "'")
            else
              candidate
          findUniquePKey pkeyCandidate

        Registry.TypesNames.Add(viewType.FullName, uniquePKey)
        uniquePKey

  static member SetNeededIElementInterface(propertyType: Type) =
    Registry.NeededIElementInterfaces.Add(propertyType) |> ignore
    $"I{propertyType.Name}TerminalElement"

  static member GetNeededIElementInterfaces() =
    Registry.NeededIElementInterfaces |> Seq.map (fun t -> $"I{t.Name}TerminalElement") |> Seq.toList |> List.sort

  static member TryGetNeededIElementInterface(propertyType: Type) =
    if Registry.NeededIElementInterfaces.Contains(propertyType) then
      Some $"I{propertyType.Name}TerminalElement"
    else
      None
