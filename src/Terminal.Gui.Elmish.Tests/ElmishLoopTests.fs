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
let ``Button instance, used as relative position, should be collected after it's no longer part of the view`` () =
  task {
    let init _ = { DisplayedView = Button }

    let update (msg: Msg) model =
      match msg with
      | ChangeView view ->
        { model with DisplayedView = view }

    let view model dispatch : ITerminalElement =
      View.Runnable (fun (p: RunnableProps) ->
        p.Children [
          let first =
            if model.DisplayedView = Button then
              View.Button (fun p ->
                p.Text "Click to test changing the Terminal Element type!"
                p.Activating (fun _ -> dispatch (ChangeView Label))
              )
            else
              View.Label (fun p ->
                p.Text "Click to test changing the Terminal Element type!"
                p.Activating (fun _ -> dispatch (ChangeView Button))
              )

          let second =
            View.Label (fun p ->
              p.Text "I am a static label below the first element."
              if model.DisplayedView = Button then
                p.Y (TPos.Bottom first)
              else
                p.X (TPos.Right first)
            )

          first
          second
        ])

    let program =
      ElmishTerminal.mkSimple init update view
      |> ElmishTester.run

    let buttonRef =
      System.WeakReference(program.ViewTE.Children.First().View)

    do! program.ProcessMsg (ChangeView Label |> TerminalMsg.ofMsg)

    System.GC.Collect()
    System.GC.WaitForPendingFinalizers()
    System.GC.Collect()

    Assert.That(buttonRef.IsAlive, Is.False, "Button instance should have been collected after the update.")
  }

