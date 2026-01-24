// Learn more about F# at http://fsharp.org

open System.Collections.Immutable
open System.Collections.ObjectModel
open Elmish
open Terminal.Gui.App
open Terminal.Gui.Configuration
open Terminal.Gui.Drawing
open Terminal.Gui.Elmish
open Terminal.Gui.Input
open Terminal.Gui.ViewBase
open Terminal.Gui.Views

type Model = {
  Application: IApplication
  AvailableThemes: IImmutableList<string>
  SelectedThemeIndex: int
}

type Msg = ThemeIndexChanged of index: int

let init application : Model * Cmd<TerminalMsg<Msg>> =
  let themes = ThemeManager.GetThemeNames()

  let model = {
    Application = application
    AvailableThemes = themes
    SelectedThemeIndex = themes.IndexOf(ThemeManager.GetCurrentThemeName())
  }

  model, Cmd.none

let update (msg: TerminalMsg<Msg>) (model: Model) : Model * Cmd<TerminalMsg<Msg>> =
  match msg with
  | Msg msg ->
    match msg with
    | ThemeIndexChanged index ->
        ThemeManager.Theme <- model.AvailableThemes[index]
  | _ -> ()

  model, Cmd.none

let view (state: Model) (dispatch: TerminalMsg<Msg> -> unit) =
  let menuBar =
    View.menuBar (fun p m ->
      m.menuBarItem (fun p m ->
        p.Title "_File"

        m.menuItems [
           View.menuItem (fun p ->
             p.Title "Quit"
             // p.HelpText "Quit UI Catalog"
             // p.Key (Key('Q'))
             // p.Action (fun _ -> dispatch TerminalMsg.Terminate)
           )
        ]
      )

      m.menuBarItem (fun p m ->
        p.Title "_Themes"

        m.menuItems [
          View.menuItem (fun p -> p.TargetView (View.line []))
          if ConfigurationManager.IsEnabled then
            View.menuItem (fun p ->
              p.Text "Cycle Through Themes"
              // p.Key Key.T.WithCtrl

              // p.CommandView (
              //   View.optionSelector (fun p ->
              //     p.highlightStates MouseState.None
              //     p.labels state.AvailableThemes
              //
              //     p.valueChanged (fun args ->
              //       if args.Value.HasValue then
              //         dispatch (ThemeIndexChanged args.Value.Value |> TerminalMsg.ofMsg)
              //     )
              //
              //     p.value (Some state.SelectedThemeIndex)
              //   )
              // )
            )
        ]
      )
    )

  let categoriesListView =
    View.listView (fun p ->
      p.BorderStyle LineStyle.Rounded
      p.X 0
      p.Y (TPos.Bottom menuBar)
      p.Height (Dim.Fill())
      p.Width (Dim.Auto())
      p.Title "_Categories"
      p.Source (new ListWrapper<_>(ObservableCollection [ "Hey"; "heylow" ]))
    )

  let scenariosListView =
    View.frameView (fun p ->
      p.BorderStyle LineStyle.Rounded
      p.X (TPos.Right categoriesListView)
      p.Y (TPos.Bottom menuBar)
      p.Height (Dim.Fill())
      p.Width (Dim.Fill())
      p.Title "_Scenarios"
      p.Children [
        let label = View.label (fun p -> p.Title "TextView:")
        let textView =
          View.textView (fun p ->
            p.Text "This is a TextView"
            p.Y (TPos.Bottom label)
            p.Height 2
            p.Width (Dim.Percent 100)
          )

        let sampleComponent =
          SampleComponent._component (fun p ->
            p.y (TPos.Bottom textView)
          )

        let treeDiffUpdateTesterComponent =
          TreeDiffUpdateTesterComponent._component(fun p ->
            p.y (TPos.Bottom sampleComponent)
          )

        label
        textView
        sampleComponent
        treeDiffUpdateTesterComponent
      ]
    )

  View.runnable [
    // TODO: fix type inference if possible so that the cast is not needed
    menuBar :> ITerminalElement
    categoriesListView
    // scenariosListView
  ]


[<EntryPoint>]
let main argv =
  ConfigurationManager.Enable(ConfigLocations.All)

  ElmishTerminal.mkProgram init update view
  |> ElmishTerminal.runTerminal

  0 // return an integer exit code
