// Learn more about F# at http://fsharp.org

open System

open System.Diagnostics
open System.Drawing
open System.Text
open System.Threading.Tasks
open Terminal.Gui.App
open Terminal.Gui.Configuration
open Terminal.Gui.Drawing
open Elmish
open Terminal.Gui.Elmish
open System.IO
open Terminal.Gui
open Terminal.Gui.Input
open Terminal.Gui.ViewBase
open Terminal.Gui.Views


type Model = unit

type Msg = unit

let init _ = (), Cmd.none

let update (msg: TerminalMsg<Msg>) (model: Model) : Model * Cmd<TerminalMsg<Msg>> =
  (), Cmd.none

let view (state: Model) (dispatch: TerminalMsg<Msg> -> unit) : ITerminalElement =
  View.runnable [
    View.menuBar (fun p _ ->
      p.Children [
        View.menuBarItem (fun p _ ->
          p.Title "_File"

          p.PopoverMenu (
            View.popoverMenu (fun p ->
              p.Root (
                View.menu (fun p ->
                  p.Children [
                    View.menuItem (fun p ->
                      p.Title "Quit"
                      // p.HelpText "Quit UI Catalog"
                      // p.Key Application.QuitKey
                      p.Command Command.Quit
                    )
                  ]
                )
              )
            )
          )
        )
        View.menuBarItem (fun p _ ->
          p.Title "_Themes"

          p.PopoverMenu (
            View.popoverMenu (fun p ->
              p.Root (
                View.menu (fun p ->
                  p.Children [
                    View.menuItem (fun p ->
                      p.TargetView (
                        View.checkBox (fun p ->
                          p.Title "Force _16 Colors"

                          // p.CheckedState (
                          //   if Application.Force16Colors then
                          //     CheckState.Checked
                          //   else
                          //     CheckState.UnChecked
                          // )

                          // p.CheckedStateChanging (fun args ->
                          //   if
                          //     (Application.Force16Colors
                          //      && args.Result = CheckState.UnChecked
                          //      && not Application.Driver.SupportsTrueColor)
                          //   then
                          //
                          //     args.Handled <- true
                          // )

                          // p.CheckedStateChanged (fun args -> Application.Force16Colors <- args.Value = CheckState.Checked)
                        )
                      )
                    )
                    View.menuItem (fun p -> p.TargetView (View.line []))
                    if ConfigurationManager.IsEnabled then
                      View.menuItem (fun p ->
                        // p.HelpText "Cycle Through Themes"
                        // p.Key Key.T.WithCtrl

                        p.TargetView (
                          View.optionSelector (fun p ->
                            ()
                            // p.HighlightStates MouseState.None
                            // p.Options (ThemeManager.GetThemeNames())

                            // p.SelectedItemChanged (fun args ->
                              // if args.SelectedItem.HasValue then
                                // ThemeManager.Theme <- ThemeManager.GetThemeNames().[args.SelectedItem.Value]
                            // )

                            // p.SelectedItem (
                              // ThemeManager
                                // .GetThemeNames()
                                // .IndexOf(ThemeManager.GetCurrentThemeName())
                            // )
                          )
                        )
                      )
                  ]
                )
              )
            )
          )
        )
      ]
    ) :> ITerminalElement
  ]


[<EntryPoint>]
let main argv =
  ConfigurationManager.Enable(ConfigLocations.All)

  let x = ElmishTerminal.mkProgram init update view
  x
  |> ElmishTerminal.runTerminal

  0 // return an integer exit code
