namespace Terminal.Gui.Elmish.Generator

open System

module Registry =

  type Views =
    static member val private TypesNames = System.Collections.Generic.Dictionary<string, string>()

    /// Especially useful to generate unique names for generic types that may also exist with same name but different generic parameters
    static member GetUniqueTypeName(viewType: Type) =
      match Views.TypesNames.TryGetValue(viewType.FullName) with
      | true, pkey -> pkey
      | _ ->
          let uniquePKey =
            let pkeyCandidate =
              viewType
              |> getTypeNameWithoutArity

            let rec findUniquePKey candidate =
              if Views.TypesNames.ContainsValue candidate then
                findUniquePKey (candidate + "'")
              else
                candidate
            findUniquePKey pkeyCandidate

          Views.TypesNames.Add(viewType.FullName, uniquePKey)
          uniquePKey

  type TEInterfaces =
    static let getTEInterfaceName propertyType =
      if propertyType = typeof<Terminal.Gui.ViewBase.View> then
        "ITerminalElement"
      else
        $"I{propertyType.Name}TerminalElement"

    static member val TEInterfaces = System.Collections.Generic.HashSet<Type>()


    static member CreateTEInterface(propertyType: Type) =
      TEInterfaces.TEInterfaces.Add(propertyType) |> ignore
      getTEInterfaceName propertyType

    static member GetAll() =
      TEInterfaces.TEInterfaces
      |> Seq.map getTEInterfaceName
      |> Seq.toList
      |> List.sort

    static member Get(propertyType: Type) =
      let mutable propertyType = propertyType
      let mutable result = None
      while result.IsNone && propertyType.IsAssignableTo typeof<Terminal.Gui.ViewBase.View> do
        if TEInterfaces.TEInterfaces.Contains(propertyType) then
          result <- Some (getTEInterfaceName propertyType)
        else
          propertyType <- propertyType.BaseType

      match result with
      | Some interfaceName -> interfaceName
      | None -> "ITerminalElement"

    static member GetAll(propertyType: Type) =
      seq {
        let mutable propertyType = propertyType
        while propertyType.IsAssignableTo typeof<Terminal.Gui.ViewBase.View> do
          if TEInterfaces.TEInterfaces.Contains(propertyType) then
            yield getTEInterfaceName propertyType
          propertyType <- propertyType.BaseType
      }
