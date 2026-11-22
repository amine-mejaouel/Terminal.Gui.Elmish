// Native AOT smoke app for Terminal.Gui.Elmish.
//
// Two modes, selected by `--headless`:
//
//   * `AotSample`             — drives a real Elmish terminal loop: builds a view tree, patches
//                               it several times (including a child reorder) and exits 0. Needs
//                               a controlling TTY.
//   * `AotSample --headless`  — exercises the same DomNode/DomDiffer pipeline directly (emit
//                               views, apply props, deferred positions, patch + reorder,
//                               dispose) without starting Terminal.Gui. No TTY required, so it
//                               is the mode used by scripts/aot-smoke.sh and CI.
//
// The point is that the *same* library code works when the whole graph is compiled to native
// code. This project is a friend assembly of Terminal.Gui.Elmish (InternalsVisibleTo) so the
// headless path can reach the internal DomNode/DomDiffer API.

module AotSample.Program

open System
open Elmish
open Terminal.Gui.Elmish
open Terminal.Gui.ViewBase

// ---------------------------------------------------------------------------
// Headless: exercise the DOM pipeline directly (no terminal required).
// ---------------------------------------------------------------------------

let private buildTree (reorder: bool) : IViewTE =
  let label = View.Label(fun p -> p.Text "label")
  let check = View.CheckBox(fun p -> p.Text "check")
  let button = View.Button(fun p -> p.Text "button")

  // Alternate the order so every update patches children, not just props.
  let children =
    if reorder then
      [ button; label; check ]
    else
      [ label; check; button ]

  View.Runnable children :?> IViewTE

let private runHeadless () =
  let rootChild, _ = DomNode.initializeTree DomRoot (ViewTE(buildTree false))

  let root =
    match rootChild with
    | ViewDomChild dom -> dom
    | ElmishComponentDomChild _ -> failwith "The headless AOT smoke root must be a view, not a component."

  for i in 1..6 do
    DomDiffer.updateTree root (buildTree (i % 2 = 1))

  DomNode.dispose root
  Console.WriteLine "headless AOT smoke OK"

// ---------------------------------------------------------------------------
// Interactive: drive a real Elmish terminal loop, then self-terminate.
// ---------------------------------------------------------------------------

type Model = { Count: int }

type Msg = Tick

let private nextCmd count : Cmd<TerminalMsg<Msg>> =
  if count >= 6 then
    // Give the app time to render the final frame, then stop.
    Cmd.OfAsync.perform (fun () -> async { do! Async.Sleep 200 }) () (fun () -> TerminalMsg.Terminate)
  else
    Cmd.OfAsync.perform (fun () -> async { do! Async.Sleep 120 }) () (fun () -> TerminalMsg.Msg Tick)

let init _ : Model * Cmd<TerminalMsg<Msg>> = { Count = 0 }, nextCmd 0

let update (msg: Msg) (model: Model) : Model * Cmd<TerminalMsg<Msg>> =
  match msg with
  | Tick ->
    let count = model.Count + 1
    { model with Count = count }, nextCmd count

let view (model: Model) (_: Dispatch<TerminalMsg<Msg>>) : ITerminalElement =
  let label = View.Label(fun p -> p.Text $"count={model.Count}")
  let check = View.CheckBox(fun p -> p.Text "check")
  let button = View.Button(fun p -> p.Text "button")

  let children =
    if model.Count % 2 = 0 then
      [ label; check; button ]
    else
      [ button; label; check ]

  View.Runnable children

[<EntryPoint>]
let main argv =
  if argv |> Array.contains "--headless" then
    runHeadless ()
  else
    ElmishTerminal.mkProgram init update view |> ElmishTerminal.runTerminal

  0
