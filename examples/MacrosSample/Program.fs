// Learn more about F# at http://fsharp.org

open System.Collections.Immutable
open System.IO
open Elmish
open Terminal.Gui.App
open Terminal.Gui.Configuration
open Terminal.Gui.Drawing
open Terminal.Gui.Elmish
open Terminal.Gui.Input
open Terminal.Gui.ViewBase
open Terminal.Gui.Views

type Model = {
  AvailableThemes: IImmutableList<string>
  SelectedThemeIndex: int
}

type Msg = ThemeIndexChanged of index: int

let init () =
  let themes = ThemeManager.GetThemeNames()

  let model = {
    AvailableThemes = themes
    SelectedThemeIndex = themes.IndexOf(ThemeManager.GetCurrentThemeName())
  }

  model, Cmd.none

let update (msg: Msg) (model: Model) =
  match msg with
  | ThemeIndexChanged index -> ThemeManager.Theme <- model.AvailableThemes[index]

  model, Cmd.none

let view (state: Model) (dispatch: Msg -> unit) =
  let menuBarv2 =
    View.menuBarv2 (fun p m ->
      m.menuBarItemv2 (fun p m ->
        p.title "_File"

        m.menuItems [
          View.menuItemv2 (fun p ->
            p.title "Quit"
            p.helpText "Quit UI Catalog"
            p.key Application.QuitKey
            p.command Command.Quit
          )
        ]
      )

      m.menuBarItemv2 (fun p m ->
        p.title "_Themes"

        m.menuItems [
          View.menuItemv2 (fun p ->
            p.commandView (
              View.checkBox (fun p ->
                p.title "Force _16 Colors"

                p.checkedState (
                  if ApplicationImpl.Instance.Force16Colors then
                    CheckState.Checked
                  else
                    CheckState.UnChecked
                )

                p.checkedStateChanging (fun args ->
                  if
                    (ApplicationImpl.Instance.Force16Colors
                     && args.Result = CheckState.UnChecked
                     && not ApplicationImpl.Instance.Driver.SupportsTrueColor)
                  then
                    args.Handled <- true
                )

                p.checkedStateChanged (fun args -> ApplicationImpl.Instance.Force16Colors <- args.Value = CheckState.Checked)
              )
            )
          )
          View.menuItemv2 (fun p -> p.commandView (View.line []))
          if ConfigurationManager.IsEnabled then
            View.menuItemv2 (fun p ->
              p.helpText "Cycle Through Themes"
              p.key Key.T.WithCtrl

              p.commandView (
                View.optionSelector (fun p ->
                  p.highlightStates MouseState.None
                  p.labels state.AvailableThemes

                  p.valueChanged (fun args ->
                    if args.Value.HasValue then
                      dispatch (ThemeIndexChanged args.Value.Value)
                  )

                  p.value (Some state.SelectedThemeIndex)
                )
              )
            )
        ]
      )
    )

  let categoriesListView =
    View.listView (fun p ->
      p.borderStyle LineStyle.Rounded
      p.x 0
      p.y (TPos.Bottom menuBarv2)
      p.height (Dim.Fill())
      p.width (Dim.Auto())
      p.title "_Categories"
      p.source [ "Hey"; "heylow" ]
    )

  let scenariosListView =
    View.frameView (fun p ->
      p.borderStyle LineStyle.Rounded
      p.x (TPos.Right categoriesListView)
      p.y (TPos.Bottom menuBarv2)
      p.height (Dim.Fill())
      p.width (Dim.Fill())
      p.title "_Scenarios"
      p.children [
        let label = View.label (fun p -> p.title "TextView:")
        let textView =
          View.textView (fun p ->
            p.text "This is a TextView"
            p.y (TPos.Bottom label)
            p.height 2
            p.width (Dim.Percent 100)
          )

        label
        textView
        SampleComponent._component (fun p ->
          // TODO-ELMISH-COMPONENT: Failing case where the ElmishComponent tries to set the Pos before the view is rendered
          p.y (TPos.Bottom textView)
        )
      ]
    )

  View.topLevel [
    menuBarv2
    categoriesListView
    scenariosListView
  ]


[<EntryPoint>]
let main argv =
  ConfigurationManager.Enable(ConfigLocations.All)

  ElmishTerminal.mkProgram init update view
  |> ElmishTerminal.withTermination (fun _ -> false)
  |> ElmishTerminal.run

  0 // return an integer exit code
