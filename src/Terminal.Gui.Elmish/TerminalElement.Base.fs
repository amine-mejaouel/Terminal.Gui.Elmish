namespace Terminal.Gui.Elmish

open System.Collections.Generic
open Terminal.Gui.ViewBase

[<AbstractClass>]
type internal ViewBackedTerminalElement(props: Props) =

  member val Props: Props = props with get, set

  member this.Children: List<TerminalElement> = props.Children

  abstract NewView: unit -> View

  abstract SetAsChildOfParentView: bool
  default _.SetAsChildOfParentView = true

  abstract Name: string

  member this.equivalentTo(other: ViewBackedTerminalElement) =
    let mutable isEquivalent = true

    this.Props
    |> Props.iter (fun kv ->
      if isEquivalent then
        if kv.Key.Key = "children" then // TODO: for now children comparison is not yet implemented
          ()
        elif kv.Key.Kind = PropKeyKind.View then
          ()
        elif kv.Key.Kind = PropKeyKind.SubElement then
          let curElement = kv.Value :?> ViewBackedTerminalElement

          let otherElement =
            other.Props
            |> Props.tryFind kv.Key
            |> Option.map (fun (x: obj) -> x :?> ViewBackedTerminalElement)

          match curElement, otherElement with
          | curValue, Some otherValue when (curValue.equivalentTo otherValue) -> ()
          | _, _ -> isEquivalent <- false
        else
          let curElement = kv.Value

          match other.Props |> Props.tryFind kv.Key with
          | Some otherElement -> isEquivalent <- curElement = otherElement
          | None -> isEquivalent <- false)

    isEquivalent

  static member compare
    (prevProps: Props)
    (curProps: Props)
    : {| changedProps: Props
         unchangedProps: Props
         removedProps: Props |}
    =

    let remainingOldProps, removedProps =
      prevProps |> Props.partition (fun kv -> curProps |> Props.rawKeyExists kv.Key)

    let unchangedProps, changedProps =
      curProps
      |> Props.partition (fun kv ->
        match remainingOldProps |> Props.tryFind kv.Key with
        | _ when kv.Key.Key = "children" -> // Here we always consider the 'children' unchanged
          true
        | Some(v: obj) when kv.Key.Kind = PropKeyKind.SubElement ->
          let curElement = kv.Value :?> ViewBackedTerminalElement

          let oldElement = v :?> ViewBackedTerminalElement
          curElement.equivalentTo oldElement
        // TODO: comparison is not good here, it can fail for many C# types
        // TODO: Properties values should be comparable
        // TODO: should also be able to compare _element props
        | Some v' when kv.Value = v' -> true
        | _ -> false)

    {| changedProps = changedProps
       unchangedProps = unchangedProps
       removedProps = removedProps |}

  /// A ViewBackedTerminalElement is a pure description; it owns no View, so disposal is a no-op.
  member this.Dispose() = ()

  interface IViewTE with
    member this.Name = this.Name

    member this.Props = this.Props

    member this.SetAsChildOfParentView = this.SetAsChildOfParentView

    member this.Children = this.Children

    member this.Dispose() = ()
