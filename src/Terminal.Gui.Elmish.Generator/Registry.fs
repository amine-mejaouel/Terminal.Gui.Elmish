namespace Terminal.Gui.Elmish.Generator

open System
open System.Collections.Generic

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
          let pkeyCandidate = viewType |> getTypeNameWithoutArity

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
          returnedTypes
          |> Seq.exists (fun rt ->
            if rt.IsGenericType && baseType.IsGenericType then
              rt = baseType.GetGenericTypeDefinition()
            else
              rt = baseType)

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
              pendingTypes |> Seq.filter (parentIsReturned returnedTypes) |> Seq.toArray

            if readyTypes.Length = 0 then
              iterate <- false
            else
              for readyType in readyTypes do
                pendingTypes.Remove readyType |> ignore
                returnedTypes.Add readyType
                yield readyType
      }
      |> Seq.toList

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
      |> Seq.distinct
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
          result <- Some(getTEInterfaceName propertyType)
        else
          propertyType <- propertyType.BaseType

      match result with
      | Some interfaceName -> interfaceName
      | None -> "ITerminalElement"

  type ViewInterfaces =
    static let getViewInterfaceName propertyType =
      if propertyType = typeof<Terminal.Gui.ViewBase.View> then
        "IView"
      else
        $"I{propertyType.Name}View"

    static member CreateInterface(propertyType: Type) =
      TEInterfaces.TEInterfaces.Add(propertyType) |> ignore
      getViewInterfaceName propertyType

  type private PropertyIdKind =
    | Property = 0
    | ViewProperty = 1
    | ViewSpecProperty = 2
    | Event = 3

  /// Deterministic property IDs shared by every generator that emits property metadata or dispatch code.
  type PropertyIds =
    static let ids =
      let ids = Dictionary<struct (Type * string * PropertyIdKind), int>()
      let mutable nextId = 0

      let add viewType pkey kind =
        ids.Add(struct (viewType, pkey, kind), nextId)
        nextId <- nextId + 1

      for viewType in ViewTypes.orderedByInheritance do
        let view = ViewMetadata.create viewType

        for prop in view.Properties do
          if prop.IsViewProperty then
            add viewType prop.PKey PropertyIdKind.ViewProperty
            add viewType prop.PKey PropertyIdKind.ViewSpecProperty
          else
            add viewType prop.PKey PropertyIdKind.Property

        for event in view.Events do
          add viewType event.PKey PropertyIdKind.Event

      // Preserve the existing ID allocation order for generated Terminal.Gui interface keys.
      let interfaces =
        typeof<Terminal.Gui.ViewBase.View>.Assembly.GetTypes()
        |> Array.filter (fun t ->
          t.IsInterface
          && t.Namespace = "Terminal.Gui.ViewBase"
          && t.Name.StartsWith("I")
          && t.Name <> "IApplication"
          && t.Name <> "IDesignTimeProperties")
        |> Array.sortBy _.Name

      for _, group in interfaces |> Array.groupBy getTypeNameWithoutArity |> Array.sortBy fst do
        let properties =
          group
          |> Array.collect (fun interfaceType ->
            (ViewMetadata.create interfaceType).Properties
            |> Array.map (fun prop -> interfaceType, prop))

        let events =
          group
          |> Array.collect (fun interfaceType ->
            (ViewMetadata.create interfaceType).Events
            |> Array.map (fun event -> interfaceType, event))

        for interfaceType, prop in properties do
          add interfaceType prop.PKey PropertyIdKind.Property

        for interfaceType, event in events do
          add interfaceType event.PKey PropertyIdKind.Event

      ids

    static member private Get(viewType, pkey, kind) = ids[struct (viewType, pkey, kind)]

    static member Property(viewType, pkey) =
      PropertyIds.Get(viewType, pkey, PropertyIdKind.Property)

    static member ViewProperty(viewType, pkey) =
      PropertyIds.Get(viewType, pkey, PropertyIdKind.ViewProperty)

    static member ViewSpecProperty(viewType, pkey) =
      PropertyIds.Get(viewType, pkey, PropertyIdKind.ViewSpecProperty)

    static member Event(viewType, pkey) =
      PropertyIds.Get(viewType, pkey, PropertyIdKind.Event)
