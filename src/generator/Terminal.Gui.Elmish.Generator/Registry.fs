namespace Terminal.Gui.Elmish.Generator

open System

module Registry =

  type ViewTypes =
    static member val private TypesNames = System.Collections.Generic.Dictionary<string, string>()

    /// <summary>
    /// <p>Especially useful to generate unique names for generic types that may also exist with same name but different generic parameters</p>
    /// <p>e.g. MyType and MyType&lt;'T&gt; will be mapped to MyType and MyType' respectively.</p>
    /// </summary>
    static member GetUniqueTypeName(viewType: Type) =
      match ViewTypes.TypesNames.TryGetValue(viewType.FullName) with
      | true, pkey -> pkey
      | _ ->
          let uniquePKey =
            let pkeyCandidate =
              viewType
              |> getTypeNameWithoutArity

            let rec findUniquePKey candidate =
              if ViewTypes.TypesNames.ContainsValue candidate then
                findUniquePKey (candidate + "'")
              else
                candidate
            findUniquePKey pkeyCandidate

          ViewTypes.TypesNames.Add(viewType.FullName, uniquePKey)
          uniquePKey

    static member val private viewTypes =
      typeof<Terminal.Gui.ViewBase.View>.Assembly.GetTypes()
      |> Seq.filter (fun t -> t.IsAssignableTo(typeof<Terminal.Gui.ViewBase.View>) && t.IsPublic)
      |> Seq.sortBy _.Name
      |> Seq.toList

    static member val orderedByInheritance =

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

        for viewType in ViewTypes.viewTypes do
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
      } |> Seq.toList

  type TEInterfaces =
    static let getTEInterfaceName propertyType =
      if propertyType = typeof<Terminal.Gui.ViewBase.View> then
        "ITerminalElement"
      else
        $"I{propertyType.Name}TerminalElement"

    static member val TEInterfaces = System.Collections.Generic.HashSet<Type>()


    static member CreateInterface(propertyType: Type) =
      TEInterfaces.TEInterfaces.Add(propertyType) |> ignore
      getTEInterfaceName propertyType

    static member GetAllPreviouslyCreatedInterfaces() =
      TEInterfaces.TEInterfaces
      |> Seq.map getTEInterfaceName
      |> Seq.toList
      |> List.sort

    static member GetAllPreviouslyCreatedInterfaces(propertyType: Type) =
      seq {
        let mutable propertyType = propertyType
        while propertyType.IsAssignableTo typeof<Terminal.Gui.ViewBase.View> do
          if TEInterfaces.TEInterfaces.Contains(propertyType) then
            yield getTEInterfaceName propertyType
          propertyType <- propertyType.BaseType
      }

    static member GetAssignableInterface(propertyType: Type) =
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
