[<RequireQualifiedAccess>]
module Terminal.Gui.Elmish.Generator.ViewType

open System
open System.Collections
open System.Reflection

let cleanTypeName (t: Type) =
  if t.Name.Contains("`") then t.Name.Substring(0, t.Name.IndexOf("`")) else t.Name

let rec genericTypeParam (t: Type) =
  if t.IsGenericType then
    let baseName = t.Name.Substring(0, t.Name.IndexOf('`'))
    $"{baseName}{genericTypeParamsBlock t}"
  else if t.IsGenericParameter then
    $"'{t.Name}"
  else if t.Name = "Boolean" then
    "bool"
  else if t.Name = "Int32" then
    "int"
  else if t.Name = "String" then
    "string"
  else
    t.Name

and genericTypeParams (t: Type) =
  String.concat ", " (t.GetGenericArguments() |> Array.map genericTypeParam)

and genericTypeParamsBlock (t: Type) =
  genericTypeParams t
  |> fun s -> if s = "" then "" else $"<{s}>"

let genericConstraints (t: Type) =
  let constraints =
    t.GetGenericArguments()
    |> Array.choose (fun t ->
        let constraints = ResizeArray<string>()
        let attrs = t.GenericParameterAttributes

        if attrs.HasFlag(System.Reflection.GenericParameterAttributes.ReferenceTypeConstraint) then
          constraints.Add($"'{t.Name}: not struct")
        if attrs.HasFlag(System.Reflection.GenericParameterAttributes.NotNullableValueTypeConstraint) then
          constraints.Add($"'{t.Name}: struct")
        if attrs.HasFlag(System.Reflection.GenericParameterAttributes.DefaultConstructorConstraint) then
          constraints.Add($"'{t.Name}: (new: unit -> '{t.Name})")
        let baseTypes = t.GetGenericParameterConstraints()
        for baseType in baseTypes do
          constraints.Add($"'{t.Name}:> {genericTypeParam baseType}")

        if constraints.Count > 0 then
          constraints
          |> String.concat " and "
          |> Some
        else
          None
    )
  if constraints.Length > 0 then
    " when " + (constraints |> String.concat " and ")
  else
    ""

let rec genericTypeParamsWithConstraintsBlock (t: Type) =
  if not t.IsGenericType then
    ""
  else
    let genericParams = genericTypeParams t
    let constraints = genericConstraints t
    $"<{genericParams}{constraints}>"

let asPKey (name: string) =
  name
  |> String.escapeReservedKeywords

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
  |> Seq.filter (fun t -> t.IsAssignableTo(typeof<Terminal.Gui.ViewBase.View>) && t.IsPublic)
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
  } |> Seq.toList

let parentViewType (viewType: Type) =
  let baseType = viewType.BaseType
  if baseType.IsSubclassOf typeof<Terminal.Gui.ViewBase.View>
    || baseType = typeof<Terminal.Gui.ViewBase.View> then
    baseType
  else
    failwith $"Type {viewType.FullName} does not have Terminal.Gui.ViewBase.View as parent type."

let isInitOnly (property: PropertyInfo) =
  match property.SetMethod with
  | null ->
    false
  | setMethod ->
    setMethod.ReturnParameter.GetRequiredCustomModifiers()
    |> Option.ofObj
    |> Option.defaultValue [||]
    |> Array.contains typeof<System.Runtime.CompilerServices.IsExternalInit>

let private properties (viewType: Type) =
  viewType.GetProperties(
    System.Reflection.BindingFlags.Public |||
    System.Reflection.BindingFlags.Instance |||
    System.Reflection.BindingFlags.DeclaredOnly)
  |> Array.filter (fun p ->
    p.CanRead && p.GetMethod.IsPublic && p.CanWrite && p.SetMethod.IsPublic && (not <| isInitOnly p)
  )
  |> Array.sortBy _.Name

let private events (viewType: Type) =
  viewType.GetEvents(
    System.Reflection.BindingFlags.Public |||
    System.Reflection.BindingFlags.Instance |||
    System.Reflection.BindingFlags.DeclaredOnly)
  |> Array.filter (fun e -> e.AddMethod.IsPublic && e.RemoveMethod.IsPublic)
  |> Array.sortBy _.Name

let eventHandlerType (event: System.Reflection.EventInfo) =
  let handlerType = event.EventHandlerType
  let genericArgs = handlerType.GetGenericArguments()
  if genericArgs.Length = 1 then
    $"{genericTypeParam genericArgs[0]} -> unit"
  else if genericArgs.Length = 0 then
    let eventArgs = handlerType.GetMethod("Invoke").GetParameters().[1].ParameterType
    $"{genericTypeParam eventArgs} -> unit"
  else
    raise (NotImplementedException())

type PropertyMetadata = {
  PKey: string
  PropertyInfo: PropertyInfo
}

type EventMetadata = {
  PKey: string
  EventInfo: EventInfo
}

type ViewMetadata = {
  ViewType: Type
  Properties: PropertyMetadata[]
  View_Typed_Properties: PropertyMetadata[]
  ViewsCollection_Typed_Properties: PropertyMetadata[]
  Events: EventMetadata[]
  HasNoEventsOrProperties: bool
}

let analyzeViewType (viewType: Type) =
  let toPropertyMetadata (p: PropertyInfo) =
    {
      PKey = asPKey p.Name
      PropertyInfo = p
    }
  let toEventMetadata (e: EventInfo) =
    {
      PKey = asPKey e.Name
      EventInfo = e
    }

  let props = properties viewType
  let view_Typed_SubElementsProps =
    props
    |> Array.filter (fun p ->
      p.PropertyType.IsSubclassOf typeof<Terminal.Gui.ViewBase.View>
    )
    |> Array.sortBy _.Name
    |> Array.map toPropertyMetadata
  let viewsCollection_Typed_SubElementsProps =
    props
    |> Array.filter (fun p ->
      p.PropertyType.IsAssignableTo typeof<IEnumerable> &&
      (if p.PropertyType.IsGenericType then
          let genericArg = p.PropertyType.GetGenericArguments().[0]
          genericArg.IsSubclassOf typeof<Terminal.Gui.ViewBase.View>
       else
          false)
    )
    |> Array.sortBy _.Name
    |> Array.map toPropertyMetadata
  let evts = events viewType |> Array.map toEventMetadata

  {
    ViewType = viewType
    Properties = props |> Array.map toPropertyMetadata
    View_Typed_Properties = view_Typed_SubElementsProps
    ViewsCollection_Typed_Properties = viewsCollection_Typed_SubElementsProps
    Events = evts
    HasNoEventsOrProperties = (props.Length = 0 && evts.Length = 0)
  }
