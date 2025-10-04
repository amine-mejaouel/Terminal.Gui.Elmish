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
let ``Menu should be correctly set`` () =
    let view =
        View.topLevel [
            View.menuBarv2 [
                menuBarv2.menus [
                    View.menuBarItemv2 [
                        prop.title "MenuBarItem"
                        menuBarItemv2.popoverMenu (View.popoverMenu [
                            popoverMenu.root (View.menuv2 [
                                prop.children [
                                    View.menuItemv2 [
                                        prop.title "MenuItem 0"
                                    ]
                                    View.menuItemv2 [
                                        prop.title "MenuItem 1"
                                    ]
                                ]
                            ])
                        ])
                    ]
                ]
            ]
        ]

    let view = view |> render
    let menuBar = (view.SubViews |> Seq.head) :?> MenuBarv2
    let menuBarItem = (menuBar.SubViews |> Seq.head) :?> MenuBarItemv2
    let popoverMenuRoot = menuBarItem.PopoverMenu.Root
    let menuItems = popoverMenuRoot.SubViews |> Seq.map unbox<MenuItemv2> |> Seq.toArray

    Assert.That(menuItems[0].Title, Is.EqualTo("MenuItem 0"))
    Assert.That(menuItems[1].Title, Is.EqualTo("MenuItem 1"))
