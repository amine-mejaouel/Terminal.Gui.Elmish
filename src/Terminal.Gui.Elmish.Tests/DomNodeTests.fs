module Terminal.Gui.Elmish.Tests.DomNodeTests

open System.Collections.Generic
open NUnit.Framework
open Terminal.Gui.Elmish
open Terminal.Gui.Views

// Phase 2.5 safety net (see review-staticDom.md #4).
//
// Phases 3-7 of plan-staticDom.md must land together; there is no partial
// migration path. These tests exercise the *new* TE -> DomNode construction and
// disposal in isolation, while the production render path still runs the old
// TE/Reuse pipeline. That way the innermost building block (turning a TE tree
// into a persistent DomNode tree) is validated before the big-bang switch.

/// Bridges the generated `IDomSpecProvider` implementations into the spec
/// resolver that `DomNode.initializeTree` / `createFromTE` expect.
let private resolveSpec (te: IViewTE) : DomNodeSpec =
  match te with
  | :? IDomSpecProvider as provider -> provider.DomSpec
  | _ -> failwith $"Terminal element '{te.Name}' does not implement IDomSpecProvider."

let private asDomNode (child: DomChild) : DomNode =
  match child with
  | ViewDomChild node -> node
  | ElmishComponentDomChild _ -> failwith "Expected a ViewDomChild, got an ElmishComponentDomChild."

/// Build a DomNode tree from a TE tree, exactly the way (future) Phase 4 will.
let private initialize (te: IViewTE) : DomNode * Dictionary<ITerminalElement, DomNode> =
  let rootChild, mapping = DomNode.initializeTree resolveSpec DomRoot (ViewTE te)
  asDomNode rootChild, mapping

[<Test>]
let ``initializeTree mirrors the TE tree structure and view types`` () =
  let te =
    View.Runnable([ View.Label(fun p -> p.Text "label"); View.Button(fun p -> p.Text "button") ]) :?> IViewTE

  let root, _ = initialize te

  Assert.That(root.Name, Is.EqualTo("Runnable"))
  Assert.That(root.View, Is.TypeOf<Runnable>())
  Assert.That(root.Children.Count, Is.EqualTo(2))

  let label = asDomNode root.Children[0]
  let button = asDomNode root.Children[1]

  Assert.That(label.Name, Is.EqualTo("Label"))
  Assert.That(label.View, Is.TypeOf<Label>())
  Assert.That(button.Name, Is.EqualTo("Button"))
  Assert.That(button.View, Is.TypeOf<Button>())

[<Test>]
let ``initializeTree applies props to the view like the old path`` () =
  let newTe = View.Button(fun p -> p.Text "Hello") :?> IViewTE
  let root, _ = initialize newTe
  let newView = root.View :?> Button

  // Same TE description rendered through the current (old) pipeline.
  let oldTe = View.Button(fun p -> p.Text "Hello") :?> IViewTE
  oldTe.InitializeTree Origin.Root
  let oldView = oldTe.View :?> Button

  Assert.That(newView.Text, Is.EqualTo("Hello"))
  Assert.That(newView.Text, Is.EqualTo(oldView.Text))

[<Test>]
let ``initializeTree adds child views to the parent SubViews in order`` () =
  let te =
    View.Runnable([ View.Label(fun p -> p.Text "a"); View.Button(fun p -> p.Text "b") ]) :?> IViewTE

  let root, _ = initialize te
  let subViews = root.View.SubViews |> Seq.toArray

  Assert.That(subViews.Length, Is.EqualTo(2))
  Assert.That(subViews[0], Is.TypeOf<Label>())
  Assert.That(subViews[1], Is.TypeOf<Button>())

[<Test>]
let ``initializeTree creates sub-element DomNodes and injects their views`` () =
  let te =
    View.Button(fun p ->
      p.Text "OK"
      p.DefaultAcceptView(View.Label(fun p -> p.Text "accept")))
    :?> IViewTE

  let root, _ = initialize te

  Assert.That(root.SubElementDomNodes.Count, Is.EqualTo(1))

  let sub = root.SubElementDomNodes[0]

  Assert.That(sub.Name, Is.EqualTo("Label"))
  Assert.That((root.View :?> Button).DefaultAcceptView, Is.SameAs(sub.View))

[<Test>]
let ``initializeTree returns a mapping from TEs to DomNodes`` () =
  let te = View.Runnable([ View.Label(fun p -> p.Text "a") ]) :?> IViewTE
  let root, mapping = initialize te

  Assert.That(mapping.ContainsKey te, Is.True)
  Assert.That(mapping[te], Is.SameAs(root))

[<Test>]
let ``dispose removes child views from the parent and detaches them`` () =
  let te = View.Runnable([ View.Label(fun p -> p.Text "a") ]) :?> IViewTE
  let root, _ = initialize te
  let child = asDomNode root.Children[0]

  Assert.That(root.View.SubViews |> Seq.length, Is.EqualTo(1))

  DomNode.dispose root

  Assert.That(root.View.SubViews |> Seq.length, Is.EqualTo(0))
  Assert.That(child.View.SuperView, Is.Null)
