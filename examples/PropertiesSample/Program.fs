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
  View.Runnable [
    View.MenuBar (fun p _ ->
      p.Children [
        View.MenuBarItem (fun p _ ->
          p.Title "_File"

          p.PopoverMenu (
            View.PopoverMenu (fun p ->
              p.Root (
                View.Menu (fun p ->
                  p.Children [
                    View.MenuItem (fun p ->
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
        View.MenuBarItem (fun p _ ->
          p.Title "_Themes"

          p.PopoverMenu (
            View.PopoverMenu (fun p ->
              p.Root (
                View.Menu (fun p ->
                  p.Children [
                    View.MenuItem (fun p ->
                      p.TargetView (
                        View.CheckBox (fun p ->
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
                    View.MenuItem (fun p -> p.TargetView (View.Line []))
                    if ConfigurationManager.IsEnabled then
                      View.MenuItem (fun p ->
                        // p.HelpText "Cycle Through Themes"
                        // p.Key Key.T.WithCtrl

                        p.TargetView (
                          View.OptionSelector (fun p ->
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
