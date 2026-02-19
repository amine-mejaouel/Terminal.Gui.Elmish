module Terminal.Gui.Elmish.Tests.ElmishLoopTests

open System.Linq
open NUnit.Framework
open Terminal.Gui.Elmish

type DisplayedView =
  | Label
  | Button

type Model = {
  DisplayedView: DisplayedView
}

type Msg =
  | ChangeView of DisplayedView

[<Test>]
let ``Unused Button instance should be collected after update`` () =
  task {
    let init _ = { DisplayedView = Button }

    let update (msg: TerminalMsg<Msg>) model =
      match msg with
      | Msg msg ->
        match msg with
        | ChangeView view ->
          { model with DisplayedView = view }
      | _ -> model

    let view model dispatch : ITerminalElement =
      View.Runnable (fun (p: RunnableProps) ->
        p.Children [
          let first =
            if model.DisplayedView = Button then
              View.Button (fun p ->
                p.Text "Click to test changing the Terminal Element type!"
                p.Activating (fun _ -> dispatch (ChangeView Label |> TerminalMsg.ofMsg))
              )
            else
              View.Label (fun p ->
                p.Text "Click to test changing the Terminal Element type!"
                p.Activating (fun _ -> dispatch (ChangeView Button |> TerminalMsg.ofMsg))
              )

          let second =
            View.Label (fun p ->
              p.Text "I am a static label below the first element."
              p.Y (TPos.Bottom first)
            )

          first
          second
        ])

    let program =
      ElmishTerminal.mkSimple init update view
      |> ElmishTester.run

    let buttonRef =
      System.WeakReference(program.ViewTE.Children.First().View)

    // For Memory snapshot testing:
    // let success = System.GC.TryStartNoGCRegion(1024L * 1024L)
    // Assert.That(success, Is.True, "Failed to start no GC region. Test cannot proceed.")

    do! program.ProcessMsg (ChangeView Label |> TerminalMsg.ofMsg)

    System.GC.Collect()
    System.GC.WaitForPendingFinalizers()

    Assert.That(buttonRef.IsAlive, Is.False, "Button instance should have been collected after the update.")
  }

