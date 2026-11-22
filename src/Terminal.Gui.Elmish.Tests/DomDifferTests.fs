module Terminal.Gui.Elmish.Tests.DomDifferTests

open System.Collections.Generic
open Elmish
open NUnit.Framework
open Terminal.Gui.Elmish
open Terminal.Gui.Views

// Phase 3: DomDiffer.update patches a persistent DomNode tree in place.
// These tests assert concrete patch outcomes and, where possible, that an
// incrementally patched tree is structurally identical to a freshly built one.

let private asDomNode (child: DomChild) : DomNode =
  match child with
  | ViewDomChild node -> node
  | ElmishComponentDomChild _ -> failwith "Expected a ViewDomChild, got an ElmishComponentDomChild."

let private initialize (te: IViewTE) : DomNode * Dictionary<ITerminalElement, DomNode> =
  let rootChild, mapping = DomNode.initializeTree DomRoot (ViewTE te)
  asDomNode rootChild, mapping

/// Structural shape: node name + view type + recursively nested view children.
let rec private shape (dom: DomNode) : string =
  let children =
    dom.Children
    |> Seq.choose (function
      | ViewDomChild child -> Some(shape child)
      | ElmishComponentDomChild _ -> None)
    |> String.concat ","

  $"{dom.Name}:{dom.View.GetType().Name}({children})"

[<Test>]
let ``update patches a changed prop on the same view instance`` () =
  let te1 = View.Button(fun p -> p.Text "a") :?> IViewTE
  let root, _ = initialize te1
  let beforeView = root.View

  let te2 = View.Button(fun p -> p.Text "b") :?> IViewTE
  DomDiffer.updateTree root te2

  Assert.That(obj.ReferenceEquals(beforeView, root.View), Is.True, "View instance should be reused.")
  Assert.That((root.View :?> Button).Text, Is.EqualTo("b"))

  let fresh, _ = initialize te2
  Assert.That(shape root, Is.EqualTo(shape fresh))

[<Test>]
let ``update appends a new child`` () =
  let te1 = View.Runnable([ View.Label(fun p -> p.Text "a") ]) :?> IViewTE
  let root, _ = initialize te1

  let te2 =
    View.Runnable([ View.Label(fun p -> p.Text "a"); View.Button(fun p -> p.Text "b") ]) :?> IViewTE

  DomDiffer.updateTree root te2

  Assert.That(root.Children.Count, Is.EqualTo(2))

  let button = asDomNode root.Children[1]
  Assert.That(button.Name, Is.EqualTo("Button"))
  Assert.That(button.View, Is.TypeOf<Button>())
  Assert.That(root.View.SubViews |> Seq.length, Is.EqualTo(2))

  let fresh, _ = initialize te2
  Assert.That(shape root, Is.EqualTo(shape fresh))

[<Test>]
let ``update removes a child and detaches its view`` () =
  let te1 =
    View.Runnable([ View.Label(fun p -> p.Text "a"); View.Button(fun p -> p.Text "b") ]) :?> IViewTE

  let root, _ = initialize te1
  let removedView = (asDomNode root.Children[1]).View

  let te2 = View.Runnable([ View.Label(fun p -> p.Text "a") ]) :?> IViewTE
  DomDiffer.updateTree root te2

  Assert.That(root.Children.Count, Is.EqualTo(1))
  Assert.That(root.View.SubViews |> Seq.length, Is.EqualTo(1))
  Assert.That(removedView.SuperView, Is.Null)

  let fresh, _ = initialize te2
  Assert.That(shape root, Is.EqualTo(shape fresh))

[<Test>]
let ``update replaces a child when its type changes`` () =
  let te1 = View.Runnable([ View.Label(fun p -> p.Text "a") ]) :?> IViewTE
  let root, _ = initialize te1
  let oldView = (asDomNode root.Children[0]).View

  let te2 = View.Runnable([ View.Button(fun p -> p.Text "b") ]) :?> IViewTE
  DomDiffer.updateTree root te2

  Assert.That(root.Children.Count, Is.EqualTo(1))

  let replaced = asDomNode root.Children[0]
  Assert.That(replaced.Name, Is.EqualTo("Button"))
  Assert.That(replaced.View, Is.TypeOf<Button>())
  Assert.That(root.View.SubViews |> Seq.length, Is.EqualTo(1))
  Assert.That(oldView.SuperView, Is.Null)

  let fresh, _ = initialize te2
  Assert.That(shape root, Is.EqualTo(shape fresh))

[<Test>]
let ``update recreates a changed sub-element`` () =
  let mk text =
    View.Button(fun p ->
      p.Text "ok"
      p.DefaultAcceptView(View.Label(fun p -> p.Text text)))
    :?> IViewTE

  let root, _ = initialize (mk "a")
  let subBefore = root.SubElementDomNodes[0]

  DomDiffer.updateTree root (mk "b")

  Assert.That(root.SubElementDomNodes.Count, Is.EqualTo(1))

  let subAfter = root.SubElementDomNodes[0]
  Assert.That(subAfter.Name, Is.EqualTo("Label"))
  Assert.That((subAfter.View :?> Label).Text, Is.EqualTo("b"))
  Assert.That((root.View :?> Button).DefaultAcceptView, Is.SameAs(subAfter.View))
  Assert.That(obj.ReferenceEquals(subBefore.View, subAfter.View), Is.False)

[<Test>]
let ``update keeps an unchanged sub-element`` () =
  let mk () =
    View.Button(fun p ->
      p.Text "ok"
      p.DefaultAcceptView(View.Label(fun p -> p.Text "a")))
    :?> IViewTE

  let root, _ = initialize (mk ())
  let subBefore = root.SubElementDomNodes[0]

  DomDiffer.updateTree root (mk ())

  Assert.That(root.SubElementDomNodes.Count, Is.EqualTo(1))
  let subAfter = root.SubElementDomNodes[0]
  Assert.That(obj.ReferenceEquals(subBefore.View, subAfter.View), Is.True, "Sub-element view should be reused.")

/// The order of a parent's `View.SubViews` — this is Terminal.Gui's draw/z-order and
/// focus/tab order, and must match `DomNode.Children`.
let private subViewTypeNames (dom: DomNode) =
  dom.View.SubViews |> Seq.map (fun v -> v.GetType().Name) |> String.concat ","

let private childTypeNames (dom: DomNode) =
  dom.Children
  |> Seq.choose (function
    | ViewDomChild child -> Some(child.View.GetType().Name)
    | ElmishComponentDomChild _ -> None)
  |> String.concat ","

[<Test>]
let ``incremental update keeps View.SubViews order in sync when inserting in the middle`` () =
  task {
    let view inserted (_: Dispatch<TerminalMsg<bool>>) =
      let children =
        if inserted then
          [ View.Label(fun p -> p.Text "a")
            View.CheckBox(fun p -> p.Text "b")
            View.Button(fun p -> p.Text "c") ]
        else
          [ View.Label(fun p -> p.Text "a"); View.Button(fun p -> p.Text "c") ]

      View.Runnable children

    use program = ElmishTester.renderStateful (fun _ -> false) (fun _ _ -> true) view

    Assert.That(subViewTypeNames program.ViewDom, Is.EqualTo "Label,Button")

    // Act: insert a CheckBox between the Label and the Button.
    let! _ = program.ProcessMsg(true |> TerminalMsg.ofMsg)

    let after = program.ViewDom
    let expected = "Label,CheckBox,Button"

    Assert.Multiple(fun () ->
      Assert.That(childTypeNames after, Is.EqualTo expected, "DomNode.Children order")
      Assert.That(subViewTypeNames after, Is.EqualTo expected, "View.SubViews order"))

    // The incremental patch must equal a fresh render.
    use fresh = ElmishTester.render (view true (fun _ -> ()))
    Assert.That(subViewTypeNames fresh.ViewDom, Is.EqualTo expected)
  }

[<Test>]
let ``incremental update keeps View.SubViews order in sync when reordering children`` () =
  task {
    let view reordered (_: Dispatch<TerminalMsg<bool>>) =
      let children =
        if reordered then
          [ View.Button(fun p -> p.Text "b")
            View.Label(fun p -> p.Text "a")
            View.CheckBox(fun p -> p.Text "c") ]
        else
          [ View.Label(fun p -> p.Text "a")
            View.CheckBox(fun p -> p.Text "c")
            View.Button(fun p -> p.Text "b") ]

      View.Runnable children

    use program = ElmishTester.renderStateful (fun _ -> false) (fun _ _ -> true) view

    Assert.That(subViewTypeNames program.ViewDom, Is.EqualTo "Label,CheckBox,Button")

    // Act: move the Button from the end to the front.
    let! _ = program.ProcessMsg(true |> TerminalMsg.ofMsg)

    let after = program.ViewDom
    let expected = "Button,Label,CheckBox"

    Assert.Multiple(fun () ->
      Assert.That(childTypeNames after, Is.EqualTo expected, "DomNode.Children order")
      Assert.That(subViewTypeNames after, Is.EqualTo expected, "View.SubViews order"))

    use fresh = ElmishTester.render (view true (fun _ -> ()))
    Assert.That(subViewTypeNames fresh.ViewDom, Is.EqualTo expected)
  }
