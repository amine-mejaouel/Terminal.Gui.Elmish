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

let init () = (), Cmd.none

let update (msg: Msg) (model: Model) = (), Cmd.none

let view (state: Model) (dispatch: Msg -> unit) =
  View.topLevel [
    View.menuBar (fun p m ->
      p.menus [
        View.menuBarItem (fun p ->
          p.title "_File"

          p.popoverMenu (
            View.popoverMenu (fun p ->
              p.root (
                View.menu (fun p ->
                  p.children [
                    View.menuItem (fun p ->
                      p.title "Quit"
                      p.helpText "Quit UI Catalog"
                      p.key Application.QuitKey
                      p.command Command.Quit
                    )
                  ]
                )
              )
            )
          )
        )
        View.menuBarItem (fun p ->
          p.title "_Themes"

          p.popoverMenu (
            View.popoverMenu (fun p ->
              p.root (
                View.menu (fun p ->
                  p.children [
                    View.menuItem (fun p ->
                      p.commandView (
                        View.checkBox (fun p ->
                          p.title "Force _16 Colors"

                          p.checkedState (
                            if Application.Force16Colors then
                              CheckState.Checked
                            else
                              CheckState.UnChecked
                          )

                          p.checkedStateChanging (fun args ->
                            if
                              (Application.Force16Colors
                               && args.Result = CheckState.UnChecked
                               && not Application.Driver.SupportsTrueColor)
                            then

                              args.Handled <- true
                          )

                          p.checkedStateChanged (fun args -> Application.Force16Colors <- args.Value = CheckState.Checked)
                        )
                      )
                    )
                    View.menuItem (fun p -> p.commandView (View.line []))
                    if ConfigurationManager.IsEnabled then
                      View.menuItem (fun p ->
                        p.helpText "Cycle Through Themes"
                        p.key Key.T.WithCtrl

                        p.commandView (
                          View.optionSelector (fun p ->
                            p.highlightStates MouseState.None
                            p.options (ThemeManager.GetThemeNames())

                            p.selectedItemChanged (fun args ->
                              if args.SelectedItem.HasValue then
                                ThemeManager.Theme <- ThemeManager.GetThemeNames().[args.SelectedItem.Value]
                            )

                            p.selectedItem (
                              ThemeManager
                                .GetThemeNames()
                                .IndexOf(ThemeManager.GetCurrentThemeName())
                            )
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
    )
  ]


[<EntryPoint>]
let main argv =
  ConfigurationManager.Enable(ConfigLocations.All)

  ElmishTerminal.mkProgram init update view
  |> ElmishTerminal.runTerminal

  0 // return an integer exit code
