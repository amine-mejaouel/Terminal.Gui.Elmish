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
    View.MenuBar (fun p m ->
      m.MenuBarItem (fun p m ->
        p.Title "_File"

        m.MenuItems [
           View.MenuItem (fun p ->
             p.Title "Quit"
             // p.HelpText "Quit UI Catalog"
             p.HotKey Key.Q
             p.MouseEvent (fun e ->
               if e.IsSingleClicked then
                 dispatch TerminalMsg.Terminate
             )
           )
        ]
      )

      m.MenuBarItem (fun p m ->
        p.Title "_Themes"

        m.MenuItems [
          View.MenuItem (fun p -> p.TargetView (View.Line []))
          if ConfigurationManager.IsEnabled then
            View.MenuItem (fun p ->
              p.Text "Cycle Through Themes"
              // p.Key Key.T.WithCtrl

              p.SubMenu (
                View.Menu (fun p ->
                  p.Children [
                    View.MenuItem (fun p ->
                      p.CommandView (
                        View.OptionSelector (fun (p: OptionSelectorProps) ->
                          p.MouseHighlightStates MouseState.None
                          p.Labels state.AvailableThemes

                          p.ValueChanged (fun args ->
                            if args.NewValue.HasValue then
                              dispatch (ThemeIndexChanged args.NewValue.Value |> TerminalMsg.ofMsg)
                          )

                          p.Value (Some state.SelectedThemeIndex |> Option.toNullable)
                        )
                      )
                    )
                  ]
              )
            )
          )
        ]
      )
    )

  let categoriesListView =
    View.ListView (fun p ->
      p.BorderStyle LineStyle.Rounded
      p.X 0
      p.Y (TPos.Bottom menuBar)
      p.Height (Dim.Fill())
      p.Width (Dim.Auto())
      p.Title "_Categories"
      p.Source (new ListWrapper<_>(ObservableCollection [ "Hey"; "heylow" ]))
    )

  let scenariosListView =
    View.FrameView (fun p ->
      p.BorderStyle LineStyle.Rounded
      p.X (TPos.Right categoriesListView)
      p.Y (TPos.Bottom menuBar)
      p.Height (Dim.Fill())
      p.Width (Dim.Fill())
      p.Title "_Scenarios"
      p.Children [
        let label = View.Label (fun p -> p.Title "TextView:")
        let textView =
          View.TextView (fun p ->
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

  View.Runnable [
    menuBar :> ITerminalElement
    categoriesListView
    scenariosListView
  ]


[<EntryPoint>]
let main argv =
  ConfigurationManager.Enable(ConfigLocations.All)

  ElmishTerminal.mkProgram init update view
  |> ElmishTerminal.runTerminal

  0
