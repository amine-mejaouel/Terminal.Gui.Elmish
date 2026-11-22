module Terminal.Gui.Elmish.Tests.PositionServiceTests

open NUnit.Framework
open Terminal.Gui.Elmish
open Terminal.Gui.ViewBase

// Phase 6: these tests target the DomNode-keyed DomPositionService (decision A2).
// A fresh service instance is used per test so the singleton is not mutated.

let private mkService () = DomPositionService()

let private asViewDomNode (child: DomChild) : DomNode =
  match child with
  | ViewDomChild node -> node
  | ElmishComponentDomChild _ -> failwith "expected a view DomNode"

let private build (children: ITerminalElement list) =
  let rootTe = View.Runnable children :?> IViewTE
  let rootChild, _ = DomNode.initializeTree DomRoot (ViewTE rootTe)
  let rootDom = asViewDomNode rootChild
  rootDom, rootDom.Children |> Seq.map asViewDomNode |> Seq.toList

let private buildLabel () =
  let rootDom, doms = build [ View.Label(fun p -> p.Text "test") ]
  rootDom, doms[0]

let private resolveTo (te: ITerminalElement) (dom: DomNode) =
  fun (candidate: ITerminalElement) ->
    if obj.ReferenceEquals(candidate, te) then
      Some dom
    else
      None

// ---------------------------------------------------------------------------
// Absolute / self-contained positions
// ---------------------------------------------------------------------------

[<Test>]
let ``ApplyDelayedPos - TPos.Absolute sets X immediately`` () =
  let rootDom, labelDom = buildLabel ()
  let svc = mkService ()
  svc.ApplyDelayedPos(labelDom, PosAxis.X, TPos.Absolute 42, fun _ -> None)
  Assert.That(labelDom.View.X, Is.EqualTo(Pos.Absolute 42))
  DomNode.dispose rootDom

[<Test>]
let ``ApplyDelayedPos - TPos.Center sets X immediately`` () =
  let rootDom, labelDom = buildLabel ()
  let svc = mkService ()
  svc.ApplyDelayedPos(labelDom, PosAxis.X, TPos.Center, fun _ -> None)
  Assert.That(labelDom.View.X, Is.EqualTo(Pos.Center()))
  DomNode.dispose rootDom

[<Test>]
let ``ApplyDelayedPos - TPos.Percent sets X immediately`` () =
  let rootDom, labelDom = buildLabel ()
  let svc = mkService ()
  svc.ApplyDelayedPos(labelDom, PosAxis.X, TPos.Percent 50, fun _ -> None)
  Assert.That(labelDom.View.X, Is.EqualTo(Pos.Percent 50))
  DomNode.dispose rootDom

[<Test>]
let ``ApplyDelayedPos - TPos.AnchorEnd with Some offset sets X immediately`` () =
  let rootDom, labelDom = buildLabel ()
  let svc = mkService ()
  svc.ApplyDelayedPos(labelDom, PosAxis.X, TPos.AnchorEnd(Some 5), fun _ -> None)
  Assert.That(labelDom.View.X, Is.EqualTo(Pos.AnchorEnd 5))
  DomNode.dispose rootDom

[<Test>]
let ``ApplyDelayedPos - TPos.AnchorEnd with None uses offset 0`` () =
  let rootDom, labelDom = buildLabel ()
  let svc = mkService ()
  svc.ApplyDelayedPos(labelDom, PosAxis.X, TPos.AnchorEnd None, fun _ -> None)
  Assert.That(labelDom.View.X, Is.EqualTo(Pos.AnchorEnd 0))
  DomNode.dispose rootDom

[<Test>]
let ``ApplyDelayedPos - absolute positions do not register handlers`` () =
  let rootDom, labelDom = buildLabel ()
  let svc = mkService ()

  svc.ApplyDelayedPos(labelDom, PosAxis.X, TPos.Absolute 10, fun _ -> None)
  svc.ApplyDelayedPos(labelDom, PosAxis.Y, TPos.Center, fun _ -> None)
  svc.ApplyDelayedPos(labelDom, PosAxis.X, TPos.Percent 25, fun _ -> None)

  Assert.That(svc.Cleanups.Count, Is.EqualTo 0)
  Assert.That(svc.NodePairs.Count, Is.EqualTo 0)
  DomNode.dispose rootDom

// ---------------------------------------------------------------------------
// Relative positions register handlers
// ---------------------------------------------------------------------------

[<Test>]
let ``ApplyDelayedPos - relative targets register handlers in the repository`` () =
  let first = View.Label(fun p -> p.Text "first")
  let second = View.Label(fun p -> p.Text "second")
  let rootDom, doms = build [ first; second ]
  let firstDom, secondDom = doms[0], doms[1]

  let cases =
    [ PosAxis.Y, TPos.Bottom first
      PosAxis.X, TPos.X first
      PosAxis.Y, TPos.Y first
      PosAxis.Y, TPos.Top first
      PosAxis.X, TPos.Left first
      PosAxis.X, TPos.Right first
      PosAxis.X, TPos.Func((fun (v: View) -> v.Frame.X + 10), first) ]

  for axis, target in cases do
    let svc = mkService ()
    svc.ApplyDelayedPos(secondDom, axis, target, resolveTo first firstDom)
    Assert.That(svc.Cleanups.Count, Is.GreaterThanOrEqualTo 1, $"Expected a handler for {target}")

  DomNode.dispose rootDom

// ---------------------------------------------------------------------------
// Index tracks both elements of a relative pair
// ---------------------------------------------------------------------------

[<Test>]
let ``ApplyDelayedPos - both elements are indexed after a relative position is registered`` () =
  let first = View.Label(fun p -> p.Text "first")
  let second = View.Label(fun p -> p.Text "second")
  let rootDom, doms = build [ first; second ]
  let firstDom, secondDom = doms[0], doms[1]
  let svc = mkService ()

  svc.ApplyDelayedPos(secondDom, PosAxis.Y, TPos.Bottom first, resolveTo first firstDom)

  Assert.Multiple(fun () ->
    Assert.That(svc.NodePairs.ContainsKey secondDom, Is.True, "second should be indexed")
    Assert.That(svc.NodePairs.ContainsKey firstDom, Is.True, "first should be indexed"))

  DomNode.dispose rootDom

// ---------------------------------------------------------------------------
// ExecuteCleanups clears handlers for the target
// ---------------------------------------------------------------------------

[<Test>]
let ``ExecuteCleanups - removes handler entries for the target`` () =
  let first = View.Label(fun p -> p.Text "first")
  let second = View.Label(fun p -> p.Text "second")
  let rootDom, doms = build [ first; second ]
  let firstDom, secondDom = doms[0], doms[1]
  let svc = mkService ()

  svc.ApplyDelayedPos(secondDom, PosAxis.Y, TPos.Bottom first, resolveTo first firstDom)

  Assert.That(svc.Cleanups.Count, Is.GreaterThan 0, "Pre-condition: handlers should be registered")

  svc.ExecuteCleanups secondDom

  Assert.Multiple(fun () ->
    Assert.That(svc.NodePairs.ContainsKey secondDom, Is.False, "Index entry for second should be removed")

    Assert.That(
      DomPairKey(secondDom, firstDom) |> svc.Cleanups.ContainsKey,
      Is.False,
      "Handler pair should be removed"
    ))

  DomNode.dispose rootDom

[<Test>]
let ``ExecuteCleanups - calling on a node without handlers is a no-op`` () =
  let rootDom, labelDom = buildLabel ()
  let svc = mkService ()
  Assert.DoesNotThrow(fun () -> svc.ExecuteCleanups labelDom)
  DomNode.dispose rootDom

[<Test>]
let ``ExecuteCleanups - no orphan entries remain after cleaning both elements`` () =
  let first = View.Label(fun p -> p.Text "first")
  let second = View.Label(fun p -> p.Text "second")
  let rootDom, doms = build [ first; second ]
  let firstDom, secondDom = doms[0], doms[1]
  let svc = mkService ()

  svc.ApplyDelayedPos(secondDom, PosAxis.Y, TPos.Bottom first, resolveTo first firstDom)
  svc.ExecuteCleanups secondDom
  svc.ExecuteCleanups firstDom

  Assert.Multiple(fun () ->
    Assert.That(svc.Cleanups.Count, Is.EqualTo 0, "Cleanups should be empty")
    Assert.That(svc.NodePairs.Count, Is.EqualTo 0, "NodePairs should be empty"))

  DomNode.dispose rootDom

// ---------------------------------------------------------------------------
// Multiple relative positions on the same element
// ---------------------------------------------------------------------------

[<Test>]
let ``ApplyDelayedPos - multiple relative positions accumulate for the same element`` () =
  let first = View.Label(fun p -> p.Text "first")
  let second = View.Label(fun p -> p.Text "second")
  let rootDom, doms = build [ first; second ]
  let firstDom, secondDom = doms[0], doms[1]
  let svc = mkService ()

  svc.ApplyDelayedPos(secondDom, PosAxis.Y, TPos.Bottom first, resolveTo first firstDom)
  svc.ApplyDelayedPos(secondDom, PosAxis.X, TPos.Right first, resolveTo first firstDom)

  Assert.That(svc.NodePairs.ContainsKey secondDom, Is.True)
  Assert.That(svc.NodePairs[secondDom].Count, Is.EqualTo 1, "One pair key shared by both positions")

  svc.ExecuteCleanups secondDom
  Assert.That(svc.NodePairs.ContainsKey secondDom, Is.False, "All entries for second should be cleared")

  DomNode.dispose rootDom
