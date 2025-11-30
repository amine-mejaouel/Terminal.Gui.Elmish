module Terminal.Gui.Elmish.Tests

open System.Linq
open NUnit.Framework
open Terminal.Gui.Elmish.Elements
open Terminal.Gui.Views
open Elmish

[<SetUp>]
let Setup () =
  ElmishTerminal.unitTestMode <- true
  ()

let render view =
  let init _ = (), Cmd.none
  let update _ _ = (), Cmd.none

  let view _ _ = view

  let terminalElement =
    ElmishTerminal.mkSimple init update view
    |> ElmishTerminal.runComponent
    :?> IInternalTerminalElement

  terminalElement.view

[<Test>]
let ``Using properties syntax: Menu should be correctly set`` () =
  let view =
    View.topLevel [
      View.menuBarv2 (fun p m ->
        p.menus [
          View.menuBarItemv2 (fun p ->
            p.title "MenuBarItem"

            p.popoverMenu (
              View.popoverMenu (fun p ->
                p.root (
                  View.menuv2 (fun p ->
                    p.children [
                      View.menuItemv2 (fun p -> p.title "MenuItem 0")
                      View.menuItemv2 (fun p -> p.title "MenuItem 1")
                    ]
                  )
                )
              )
            )
          )
        ]
      )
    ]
    :?> IInternalTerminalElement

  let menuBarv2Element =
    view.children.Single() :?> MenuBarv2Element

  let menuBarItemv2Element =
    (menuBarv2Element.props
     |> Props.find PKey.menuBarv2.children)
      .Single()
    :?> MenuBarItemv2Element

  let popoverMenu =
    menuBarItemv2Element.props
    |> Props.find PKey.menuBarItemv2.popoverMenu_element
    :?> PopoverMenuElement

  let popoverMenuRoot =
    popoverMenu.props
    |> Props.find PKey.popoverMenu.root_element

  let view = view |> render

  let menuBar =
    (view.SubViews |> Seq.head) :?> MenuBarv2

  let menuBarItem =
    (menuBar.SubViews |> Seq.head) :?> MenuBarItemv2

  let popoverMenuRoot =
    menuBarItem.PopoverMenu.Root

  let menuItems =
    popoverMenuRoot.SubViews
    |> Seq.map unbox<MenuItemv2>
    |> Seq.toArray

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
            View.menuItemv2 (fun p -> p.title "MenuItem 0")
            View.menuItemv2 (fun p -> p.title "MenuItem 1")
          ]
        )
      )
    ]
    :?> IInternalTerminalElement

  let menuBarv2Element =
    view.children.Single() :?> MenuBarv2Element

  let menuBarItemv2Element =
    (menuBarv2Element.props
     |> Props.find PKey.menuBarv2.children)
      .Single()
    :?> MenuBarItemv2Element

  let popoverMenu =
    menuBarItemv2Element.props
    |> Props.find PKey.menuBarItemv2.popoverMenu_element
    :?> PopoverMenuElement

  let popoverMenuRoot =
    popoverMenu.props
    |> Props.find PKey.popoverMenu.root_element


  let view = view |> render

  let menuBar =
    (view.SubViews |> Seq.head) :?> MenuBarv2

  let menuBarItem =
    (menuBar.SubViews |> Seq.head) :?> MenuBarItemv2

  let popoverMenuRoot =
    menuBarItem.PopoverMenu.Root

  let menuItems =
    popoverMenuRoot.SubViews
    |> Seq.map unbox<MenuItemv2>
    |> Seq.toArray

  Assert.That(menuItems[0].Title, Is.EqualTo("MenuItem 0"))
  Assert.That(menuItems[1].Title, Is.EqualTo("MenuItem 1"))
