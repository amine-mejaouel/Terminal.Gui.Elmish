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
    View.runnable [
      View.menuBar (fun p m ->
        p.menus [
          View.menuBarItem (fun p ->
            p.title "MenuBarItem"

            p.popoverMenu (
              View.popoverMenu (fun p ->
                p.root (
                  View.menu (fun p ->
                    p.children [
                      View.menuItem (fun p -> p.title "MenuItem 0")
                      View.menuItem (fun p -> p.title "MenuItem 1")
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

  let menuBarElement =
    view.children.Single() :?> MenuBarElement

  let menuBarItemElement =
    (menuBarElement.props
     |> Props.find PKey.menuBar.children)
      .Single()
    :?> MenuBarItemElement

  let popoverMenu =
    menuBarItemElement.props
    |> Props.find PKey.menuBarItem.popoverMenu_element
    :?> PopoverMenuElement

  let popoverMenuRoot =
    popoverMenu.props
    |> Props.find PKey.popoverMenu.root_element

  let view = view |> render

  let menuBar =
    (view.SubViews |> Seq.head) :?> MenuBar

  let menuBarItem =
    (menuBar.SubViews |> Seq.head) :?> MenuBarItem

  let popoverMenuRoot =
    menuBarItem.PopoverMenu.Root

  let menuItems =
    popoverMenuRoot.SubViews
    |> Seq.map unbox<MenuItem>
    |> Seq.toArray

  Assert.That(menuItems[0].Title, Is.EqualTo("MenuItem 0"))
  Assert.That(menuItems[1].Title, Is.EqualTo("MenuItem 1"))

[<Test>]
let ``Using macros syntax: Menu should be correctly set`` () =
  let view =
    View.runnable [
      View.menuBar (fun p m ->
        m.menuBarItem (fun p m ->
          p.title "MenuBarItem"

          m.menuItems [
            View.menuItem (fun p -> p.title "MenuItem 0")
            View.menuItem (fun p -> p.title "MenuItem 1")
          ]
        )
      )
    ]
    :?> IInternalTerminalElement

  let menuBarElement =
    view.children.Single() :?> MenuBarElement

  let menuBarItemElement =
    (menuBarElement.props
     |> Props.find PKey.menuBar.children)
      .Single()
    :?> MenuBarItemElement

  let popoverMenu =
    menuBarItemElement.props
    |> Props.find PKey.menuBarItem.popoverMenu_element
    :?> PopoverMenuElement

  let popoverMenuRoot =
    popoverMenu.props
    |> Props.find PKey.popoverMenu.root_element


  let view = view |> render

  let menuBar =
    (view.SubViews |> Seq.head) :?> MenuBar

  let menuBarItem =
    (menuBar.SubViews |> Seq.head) :?> MenuBarItem

  let popoverMenuRoot =
    menuBarItem.PopoverMenu.Root

  let menuItems =
    popoverMenuRoot.SubViews
    |> Seq.map unbox<MenuItem>
    |> Seq.toArray

  Assert.That(menuItems[0].Title, Is.EqualTo("MenuItem 0"))
  Assert.That(menuItems[1].Title, Is.EqualTo("MenuItem 1"))
