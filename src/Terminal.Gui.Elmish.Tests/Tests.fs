module Terminal.Gui.Elmish.Tests

open NUnit.Framework
open Terminal.Gui.Views

[<SetUp>]
let Setup () =
    Program.unitTestMode <- true
    ()

let render view =
    let init _ = (), Cmd.none
    let update _ _ = (), Cmd.none

    let view _ _ =
        view

    Program.mkSimple init update view
    |> Program.run

    Program.topView.Value

[<Test>]
let ``Using properties syntax: Menu should be correctly set`` () =
    let view =
        View.topLevel [
            View.menuBarv2 (fun p m ->
                p.menus [
                    View.menuBarItemv2 (fun p ->
                        p.title "MenuBarItem"
                        p.popoverMenu (View.popoverMenu (fun p ->
                            p.root (View.menuv2 (fun p ->
                                p.children [
                                    View.menuItemv2 (fun p ->
                                        p.title "MenuItem 0"
                                    )
                                    View.menuItemv2 (fun p ->
                                        p.title "MenuItem 1"
                                    )
                                ]
                            ))
                        ))
                    )
                ]
            )
        ]

    let view = view |> render
    let menuBar = (view.SubViews |> Seq.head) :?> MenuBarv2
    let menuBarItem = (menuBar.SubViews |> Seq.head) :?> MenuBarItemv2
    let popoverMenuRoot = menuBarItem.PopoverMenu.Root
    let menuItems = popoverMenuRoot.SubViews |> Seq.map unbox<MenuItemv2> |> Seq.toArray

    Assert.That(menuItems[0].Title, Is.EqualTo("MenuItem 0"))
    Assert.That(menuItems[1].Title, Is.EqualTo("MenuItem 1"))

[<Test>]
let ``Using macros syntax: Menu should be correctly set`` () =
    let view =
        View.topLevel [
            View.menuBarv2 (fun p m ->
                m.menuBarItemv2 (fun p m ->
                    p.title "MenuBarItem"
                    m.menuItems [
                        View.menuItemv2 (fun p ->
                            p.title "MenuItem 0"
                        )
                        View.menuItemv2 (fun p ->
                            p.title "MenuItem 1"
                        )
                    ]
                )
            )
        ]

    let view = view |> render
    let menuBar = (view.SubViews |> Seq.head) :?> MenuBarv2
    let menuBarItem = (menuBar.SubViews |> Seq.head) :?> MenuBarItemv2
    let popoverMenuRoot = menuBarItem.PopoverMenu.Root
    let menuItems = popoverMenuRoot.SubViews |> Seq.map unbox<MenuItemv2> |> Seq.toArray

    Assert.That(menuItems[0].Title, Is.EqualTo("MenuItem 0"))
    Assert.That(menuItems[1].Title, Is.EqualTo("MenuItem 1"))
