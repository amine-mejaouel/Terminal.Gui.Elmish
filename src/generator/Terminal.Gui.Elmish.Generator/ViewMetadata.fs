namespace Terminal.Gui.Elmish.Generator

open System
open System.Collections
open System.Reflection

type PropertyMetadata =
  { PKey: string
    PropertyInfo: PropertyInfo }

  member x.IsViewProperty =
    x.PropertyInfo.PropertyType.IsAssignableTo typeof<Terminal.Gui.ViewBase.View>

type EventMetadata = {
  PKey: string
  EventInfo: EventInfo
}

type ViewMetadata =
  // TODO: Use this {} style to define the record type everywhere
  // because it allows adding members easily, in contrast to other styles that do not.
  { Type: Type
    Properties: PropertyMetadata[]
    View_Typed_Properties: PropertyMetadata[]
    ViewsCollection_Typed_Properties: PropertyMetadata[]
    Events: EventMetadata[] }

    member x.HasNoEventsOrProperties =
      (x.Properties.Length = 0 && x.Events.Length = 0)

[<RequireQualifiedAccess>]
module ViewMetadata =

  let private hasInitOnlySetter (property: PropertyInfo) =
    match property.SetMethod with
    | null ->
      false
    | setMethod ->
      setMethod.ReturnParameter.GetRequiredCustomModifiers()
      |> Option.ofObj
      |> Option.defaultValue [||]
      |> Array.contains typeof<System.Runtime.CompilerServices.IsExternalInit>

  let private readWriteProperties (viewType: Type) =
    viewType.GetProperties(
      BindingFlags.Public |||
      BindingFlags.Instance |||
      BindingFlags.DeclaredOnly)
    |> Array.filter (fun p ->
      p.CanRead && p.GetMethod.IsPublic && p.CanWrite && p.SetMethod.IsPublic && (not <| hasInitOnlySetter p)
    )
    |> Array.sortBy _.Name

  let private events (viewType: Type) =
    viewType.GetEvents(
      BindingFlags.Public |||
      BindingFlags.Instance |||
      BindingFlags.DeclaredOnly)
    |> Array.filter (fun e -> e.AddMethod.IsPublic && e.RemoveMethod.IsPublic)
    |> Array.sortBy _.Name

  let create (viewType: Type) =
    let toPropertyMetadata (p: PropertyInfo) =
      {
        PKey = String.escapeReservedKeywords p.Name
        PropertyInfo = p
      }
    let toEventMetadata (e: EventInfo) =
      {
        PKey = String.escapeReservedKeywords e.Name
        EventInfo = e
      }

    let props = readWriteProperties viewType
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
      Type = viewType
      Properties = props |> Array.map toPropertyMetadata
      View_Typed_Properties = view_Typed_SubElementsProps
      ViewsCollection_Typed_Properties = viewsCollection_Typed_SubElementsProps
      Events = evts
    }
