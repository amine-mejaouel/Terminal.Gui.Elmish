// Learn more about F# at http://fsharp.org

open System.Collections.Immutable
open Terminal.Gui.App
open Terminal.Gui.Configuration
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
    let themes = ThemeManager.GetThemeNames();
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
    View.topLevel [
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
                    View.menuItemv2 (fun p -> p.commandView (View.line []))
                    if ConfigurationManager.IsEnabled then
                        View.menuItemv2 (fun p ->
                            p.helpText "Cycle Through Themes"
                            p.key Key.T.WithCtrl

                            p.commandView (
                                View.optionSelector (fun p ->
                                    p.highlightStates MouseState.None
                                    p.options state.AvailableThemes

                                    p.selectedItemChanged (fun args ->
                                        if args.SelectedItem.HasValue then
                                            dispatch (ThemeIndexChanged args.SelectedItem.Value)
                                    )

                                    p.selectedItem state.SelectedThemeIndex
                                )
                            )
                        )
                ]
            )
        )
    ]


[<EntryPoint>]
let main argv =
    ConfigurationManager.Enable(ConfigLocations.All)

    Program.mkProgram init update view
    |> Program.withSubscription (fun state ->
        fun dispatch ->
            async {
                ()
            (*while state.Count < 1_000_000 do
                    do! Async.Sleep 10
                    dispatch Inc*)
            }
            |> Async.StartImmediate
        |> Cmd.ofSub
    )
    |> Program.run

    0 // return an integer exit code
