namespace Terminal.Gui.Elmish.Generator

open System

module ViewType =
  let private viewTypes =
    typeof<Terminal.Gui.ViewBase.View>.Assembly.GetTypes()
    |> Seq.filter (fun t -> t.IsAssignableTo(typeof<Terminal.Gui.ViewBase.View>) && t.IsPublic)
    |> Seq.sortBy _.Name
    |> Seq.toList

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
    } |> Seq.toList

  let parentView (viewType: Type) =
    let baseType = viewType.BaseType
    if baseType.IsAssignableTo typeof<Terminal.Gui.ViewBase.View> then
      baseType
    else
      failwith $"Type {viewType.FullName} does not have Terminal.Gui.ViewBase.View as parent type."
