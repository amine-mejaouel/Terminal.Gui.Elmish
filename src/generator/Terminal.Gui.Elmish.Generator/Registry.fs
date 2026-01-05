namespace Terminal.Gui.Elmish.Generator

open System

type Registry =
  static member val private TypesNames = System.Collections.Generic.Dictionary<string, string>()
  static member val NeededIElementInterfaces = System.Collections.Generic.HashSet<string>()
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

  static member SetNeededIElementInterface(interfaceName) =
    Registry.NeededIElementInterfaces.Add(interfaceName) |> ignore

  static member GetNeededIElementInterfaces() =
    Registry.NeededIElementInterfaces |> Seq.toList |> List.sort
