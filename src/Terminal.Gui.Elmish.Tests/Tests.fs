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
                        // TODO: following two subsequent lines could be merged into one
                        menuBarItemv2.popoverMenu [
                            // TODO: another way to represent things
                            // View.Menuv2 precision should give us the type
                            // TODO: Alternatively, maybe all properties like popoverMenu.root bellow
                            // TODO: can contain a documentation referencing the time that we are waiting for
                            // popoverMenu.root View.Menuv2 [

                            //]
                            popoverMenu.popoverMenuRoot [
                                prop.children [
                                    View.menuItemv2 [
                                        prop.title "MenuItem 0"
                                    ]
                                    View.menuItemv2 [
                                        prop.title "MenuItem 1"
                                    ]
                                ]
                            ]
                        ]
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
