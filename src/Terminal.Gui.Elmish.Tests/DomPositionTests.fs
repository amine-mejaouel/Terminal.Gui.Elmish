module Terminal.Gui.Elmish.Tests.DomPositionTests

open NUnit.Framework
open Terminal.Gui.Elmish
open Terminal.Gui.ViewBase

// Phase 5 / decision A2: positions are applied by a deferred pass over the DomNode tree,
// resolving delayed TPos references through the per-render TE -> DomNode mapping.

let private asDomNode (child: DomChild) : DomNode =
  match child with
  | ViewDomChild node -> node
  | ElmishComponentDomChild _ -> failwith "Expected a ViewDomChild."

[<Test>]
let ``initializeTree applies an absolute position via the deferred pass`` () =
  let te =
    View.Label(fun p ->
      p.Text "x"
      p.X(Pos.Absolute 7))
    :?> IViewTE

  let rootChild, _ = DomNode.initializeTree DomRoot (ViewTE te)
  let root = asDomNode rootChild

  Assert.That(root.View.X, Is.EqualTo(Pos.Absolute 7))
  DomNode.dispose root

[<Test>]
let ``initializeTree resolves delayed TPos and registers a cleanup that dispose removes`` () =
  let first = View.Label(fun p -> p.Text "first")

  let second =
    View.Label(fun p ->
      p.Text "second"
      p.Y(TPos.Bottom first))

  let te = View.Runnable([ first; second ]) :?> IViewTE

  let before = DomPositionService.Current.Cleanups.Count
  let rootChild, _ = DomNode.initializeTree DomRoot (ViewTE te)
  let root = asDomNode rootChild

  Assert.That(
    DomPositionService.Current.Cleanups.Count,
    Is.GreaterThan before,
    "A relative position should register a cleanup pair."
  )

  DomNode.dispose root
  Assert.That(DomPositionService.Current.Cleanups.Count, Is.EqualTo before)

[<Test>]
let ``updateTree re-applies positions from the new props`` () =
  let mk x =
    View.Label(fun p ->
      p.Text "x"
      p.X(Pos.Absolute x))

  let te1 = View.Runnable([ mk 1 ]) :?> IViewTE
  let rootChild, _ = DomNode.initializeTree DomRoot (ViewTE te1)
  let root = asDomNode rootChild
  let child = asDomNode root.Children[0]

  Assert.That(child.View.X, Is.EqualTo(Pos.Absolute 1))

  let te2 = View.Runnable([ mk 9 ]) :?> IViewTE
  DomDiffer.updateTree root te2

  Assert.That(child.View.X, Is.EqualTo(Pos.Absolute 9))
  DomNode.dispose root
