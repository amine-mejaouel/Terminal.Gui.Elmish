module Terminal.Gui.Elmish.Tests.PositionServiceTests

open NUnit.Framework
open Terminal.Gui.Elmish
open Terminal.Gui.ViewBase

// ---------------------------------------------------------------------------
// Helpers
// ---------------------------------------------------------------------------

/// Creates a fresh, independent PositionService instance for each test so
/// that the singleton `PositionService.Current` is not mutated.
let private mkService () = PositionService()

/// Quickly build a rendered Label IViewTE with the ElmishTester helper.
let private renderLabel () =
    let label = View.Label (fun p -> p.Text "test")
    let root =
        View.Runnable [
            label
        ]
    let tester = ElmishTester.render root
    let labelTE = label :?> IViewTE
    tester, labelTE

/// Render two labels next to each other and return both IViewTE handles.
let private renderTwoLabels () =
    let first = View.Label (fun p -> p.Text "first")
    let second = View.Label (fun p -> p.Text "second")
    let root =
        View.Runnable [
            first
            second
        ]
    let tester = ElmishTester.render root
    first :?> IViewTE, second :?> IViewTE, tester

// ---------------------------------------------------------------------------
// Absolute / self-contained positions
// ---------------------------------------------------------------------------

[<Test>]
let ``ApplyPos - TPos.Absolute sets X immediately`` () =
    let tester, labelTE = renderLabel ()
    use _ = tester
    let svc = mkService ()
    svc.ApplyPos(labelTE, TPos.Absolute 42, fun view pos -> view.X <- pos)
    Assert.That(labelTE.View.X, Is.EqualTo(Pos.Absolute 42))

[<Test>]
let ``ApplyPos - TPos.Center sets X immediately`` () =
    let tester, labelTE = renderLabel ()
    use _ = tester
    let svc = mkService ()
    svc.ApplyPos(labelTE, TPos.Center, fun view pos -> view.X <- pos)
    Assert.That(labelTE.View.X, Is.EqualTo(Pos.Center()))

[<Test>]
let ``ApplyPos - TPos.Percent sets X immediately`` () =
    let tester, labelTE = renderLabel ()
    use _ = tester
    let svc = mkService ()
    svc.ApplyPos(labelTE, TPos.Percent 50, fun view pos -> view.X <- pos)
    Assert.That(labelTE.View.X, Is.EqualTo(Pos.Percent 50))

[<Test>]
let ``ApplyPos - TPos.AnchorEnd with Some offset sets X immediately`` () =
    let tester, labelTE = renderLabel ()
    use _ = tester
    let svc = mkService ()
    svc.ApplyPos(labelTE, TPos.AnchorEnd (Some 5), fun view pos -> view.X <- pos)
    Assert.That(labelTE.View.X, Is.EqualTo(Pos.AnchorEnd 5))

[<Test>]
let ``ApplyPos - TPos.AnchorEnd with None uses offset 0`` () =
    let tester, labelTE = renderLabel ()
    use _ = tester
    let svc = mkService ()
    svc.ApplyPos(labelTE, TPos.AnchorEnd None, fun view pos -> view.X <- pos)
    Assert.That(labelTE.View.X, Is.EqualTo(Pos.AnchorEnd 0))

// ---------------------------------------------------------------------------
// Absolute positions do NOT register any handlers
// ---------------------------------------------------------------------------

[<Test>]
let ``ApplyPos - Absolute positions do not add entries to RemoveHandlerRepository`` () =
    let tester, labelTE = renderLabel ()
    use _ = tester
    let svc = mkService ()

    svc.ApplyPos(labelTE, TPos.Absolute 10, fun view pos -> view.X <- pos)
    svc.ApplyPos(labelTE, TPos.Center,      fun view pos -> view.Y <- pos)
    svc.ApplyPos(labelTE, TPos.Percent 25,  fun view pos -> view.X <- pos)

    Assert.That(svc.Cleanups.Count, Is.EqualTo 0)
    Assert.That(svc.TerminalElementPairs.Count,    Is.EqualTo 0)

// ---------------------------------------------------------------------------
// Relative positions
// ---------------------------------------------------------------------------

[<Test>]
let ``ApplyPos - TPos.Bottom registers handlers in the repository`` () =
    let firstTE, secondTE, tester = renderTwoLabels ()
    use _ = tester
    let svc = mkService ()

    svc.ApplyPos(secondTE, TPos.Bottom firstTE, fun view pos -> view.Y <- pos)

    Assert.That(svc.Cleanups.Count, Is.GreaterThanOrEqualTo 1)

[<Test>]
let ``ApplyPos - TPos.X relative registers handlers in the repository`` () =
    let firstTE, secondTE, tester = renderTwoLabels ()
    use _ = tester
    let svc = mkService ()

    svc.ApplyPos(secondTE, TPos.X firstTE, fun view pos -> view.X <- pos)

    Assert.That(svc.Cleanups.Count, Is.GreaterThanOrEqualTo 1)

[<Test>]
let ``ApplyPos - TPos.Y relative registers handlers in the repository`` () =
    let firstTE, secondTE, tester = renderTwoLabels ()
    use _ = tester
    let svc = mkService ()

    svc.ApplyPos(secondTE, TPos.Y firstTE, fun view pos -> view.Y <- pos)

    Assert.That(svc.Cleanups.Count, Is.GreaterThanOrEqualTo 1)

[<Test>]
let ``ApplyPos - TPos.Top relative registers handlers in the repository`` () =
    let firstTE, secondTE, tester = renderTwoLabels ()
    use _ = tester
    let svc = mkService ()

    svc.ApplyPos(secondTE, TPos.Top firstTE, fun view pos -> view.Y <- pos)

    Assert.That(svc.Cleanups.Count, Is.GreaterThanOrEqualTo 1)

[<Test>]
let ``ApplyPos - TPos.Left relative registers handlers in the repository`` () =
    let firstTE, secondTE, tester = renderTwoLabels ()
    use _ = tester
    let svc = mkService ()

    svc.ApplyPos(secondTE, TPos.Left firstTE, fun view pos -> view.X <- pos)

    Assert.That(svc.Cleanups.Count, Is.GreaterThanOrEqualTo 1)

[<Test>]
let ``ApplyPos - TPos.Right relative registers handlers in the repository`` () =
    let firstTE, secondTE, tester = renderTwoLabels ()
    use _ = tester
    let svc = mkService ()

    svc.ApplyPos(secondTE, TPos.Right firstTE, fun view pos -> view.X <- pos)

    Assert.That(svc.Cleanups.Count, Is.GreaterThanOrEqualTo 1)

[<Test>]
let ``ApplyPos - TPos.Func relative registers handlers in the repository`` () =
    let firstTE, secondTE, tester = renderTwoLabels ()
    use _ = tester
    let svc = mkService ()
    let func = fun (v: View) -> v.Frame.X + 10

    svc.ApplyPos(secondTE, TPos.Func (func, firstTE), fun view pos -> view.X <- pos)

    Assert.That(svc.Cleanups.Count, Is.GreaterThanOrEqualTo 1)

// ---------------------------------------------------------------------------
// Index repository tracks both elements of a relative pair
// ---------------------------------------------------------------------------

[<Test>]
let ``ApplyPos - Both elements are indexed after a relative position is registered`` () =
    let firstTE, secondTE, tester = renderTwoLabels ()
    use _ = tester
    let svc = mkService ()

    svc.ApplyPos(secondTE, TPos.Bottom firstTE, fun view pos -> view.Y <- pos)

    Assert.Multiple(fun () ->
        Assert.That(svc.TerminalElementPairs.ContainsKey(secondTE :> ITerminalElementBase), Is.True,
                                                                                            "secondTE should be indexed")
        Assert.That(svc.TerminalElementPairs.ContainsKey(firstTE  :> ITerminalElementBase), Is.True,
                                                                                            "firstTE should be indexed"))

// ---------------------------------------------------------------------------
// SignalReuse clears handlers for the reused element
// ---------------------------------------------------------------------------

[<Test>]
let ``SignalReuse - removes handler entries for the reused element`` () =
    let firstTE, secondTE, tester = renderTwoLabels ()
    use _ = tester
    let svc = mkService ()

    svc.ApplyPos(secondTE, TPos.Bottom firstTE, fun view pos -> view.Y <- pos)

    let countBefore = svc.Cleanups.Count
    Assert.That(countBefore, Is.GreaterThan 0, "Pre-condition: handlers should be registered")

    svc.SignalReuse secondTE

    Assert.Multiple(fun () ->
        Assert.That(svc.TerminalElementPairs.ContainsKey(secondTE :> ITerminalElementBase), Is.False,
                                                                                            "Index entry for secondTE should be removed after SignalReuse")
        let pairRemoved =
            not (svc.Cleanups.ContainsKey(TePairKey(secondTE, firstTE)))
        Assert.That(pairRemoved, Is.True, "Handler pair should be removed from repository"))

[<Test>]
let ``SignalReuse - calling on element without handlers is a no-op`` () =
    let tester, labelTE = renderLabel ()
    use _ = tester
    let svc = mkService ()

    Assert.DoesNotThrow(fun () -> svc.SignalReuse labelTE)

// ---------------------------------------------------------------------------
// SignalDispose clears handlers for the disposed element
// ---------------------------------------------------------------------------

[<Test>]
let ``SignalDispose - removes handler entries for the disposed element`` () =
    let firstTE, secondTE, tester = renderTwoLabels ()
    use _ = tester
    let svc = mkService ()

    svc.ApplyPos(secondTE, TPos.Bottom firstTE, fun view pos -> view.Y <- pos)

    svc.SignalDispose secondTE

    Assert.Multiple(fun () ->
        Assert.That(svc.TerminalElementPairs.ContainsKey(secondTE :> ITerminalElementBase), Is.False,
                                                                                            "Index entry for secondTE should be removed after SignalDispose")
        let pairRemoved =
            not (svc.Cleanups.ContainsKey(TePairKey(secondTE, firstTE)))
        Assert.That(pairRemoved, Is.True, "Handler pair should be removed from repository")

        Assert.That(svc.TerminalElementPairs.Count, Is.EqualTo 0,
                                                    "No other index entries should remain after SignalDispose")

        Assert.That(svc.Cleanups.Count, Is.EqualTo 0,
                                        "No other handler entries should remain after SignalDispose")
        )

[<Test>]
let ``SignalDispose - calling on element without handlers is a no-op`` () =
    let tester, labelTE = renderLabel ()
    use _ = tester
    let svc = mkService ()

    Assert.DoesNotThrow(fun () -> svc.SignalDispose labelTE)

// ---------------------------------------------------------------------------
// Multiple relative positions on the same element
// ---------------------------------------------------------------------------

[<Test>]
let ``ApplyPos - multiple relative positions accumulate handlers for the same element`` () =
    let firstTE, secondTE, tester = renderTwoLabels ()
    use _ = tester
    let svc = mkService ()

    svc.ApplyPos(secondTE, TPos.Bottom firstTE, fun view pos -> view.Y <- pos)
    svc.ApplyPos(secondTE, TPos.Right  firstTE, fun view pos -> view.X <- pos)

    Assert.That(svc.TerminalElementPairs.ContainsKey(secondTE :> ITerminalElementBase), Is.True)

    let indexSet = svc.TerminalElementPairs[secondTE :> ITerminalElementBase]
    Assert.That(indexSet.Count, Is.EqualTo 1,
                "Index should track both (secondTE,firstTE) pairs for both positions")

[<Test>]
let ``SignalReuse - clears ALL handlers registered for element with multiple relative positions`` () =
    let firstTE, secondTE, tester = renderTwoLabels ()
    use _ = tester
    let svc = mkService ()

    svc.ApplyPos(secondTE, TPos.Bottom firstTE, fun view pos -> view.Y <- pos)
    svc.ApplyPos(secondTE, TPos.Right  firstTE, fun view pos -> view.X <- pos)

    svc.SignalReuse secondTE

    Assert.That(svc.TerminalElementPairs.ContainsKey(secondTE :> ITerminalElementBase), Is.False,
                                                                                        "All index entries for secondTE should be cleared")

// ---------------------------------------------------------------------------
// Repository stays clean after a full render+dispose cycle
// ---------------------------------------------------------------------------

[<Test>]
let ``After SignalDispose on both elements no orphan entries remain`` () =
    let firstTE, secondTE, tester = renderTwoLabels ()
    use _ = tester
    let svc = mkService ()

    svc.ApplyPos(secondTE, TPos.Bottom firstTE, fun view pos -> view.Y <- pos)

    svc.SignalDispose secondTE
    svc.SignalDispose firstTE

    Assert.Multiple(fun () ->
        Assert.That(svc.Cleanups.Count, Is.EqualTo 0,
                                        "RemoveHandlerRepository should be empty")
        Assert.That(svc.TerminalElementPairs.Count, Is.EqualTo 0,
                                                    "IndexedRemoveHandler should be empty"))
