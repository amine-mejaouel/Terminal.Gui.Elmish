module Terminal.Gui.Elmish.Tests.Tests

open System.Linq
open NUnit.Framework
open Terminal.Gui.Elmish
open Terminal.Gui.Views
open Elmish

let render view =
  let init _ = (), Cmd.none
  let update _ _ = (), Cmd.none

  let view _ _ = view

  let terminalElement =
    ElmishTerminal.mkSimple init update view
    |> ElmishTerminal.runComponent
    :?> IInternalTerminalElement

  terminalElement.View

[<Test>]
let ``Using properties syntax: Menu should be correctly set`` () =
  let view =
    View.Runnable [
      View.MenuBar (fun p m ->
        p.Children [
          View.MenuBarItem (fun p m ->
            p.Title "MenuBarItem"
            p.PopoverMenu (
              View.PopoverMenu (fun p ->
                p.Root (
                  View.Menu (fun p ->
                    p.Children [
                      View.MenuItem (fun p -> p.Title "MenuItem 0")
                      View.MenuItem (fun p -> p.Title "MenuItem 1")
                    ]
                  )
                )
              )
            )
          )
        ]
      ) :> ITerminalElement
    ]
    :?> IInternalTerminalElement

  let menuBarElement =
    view.Children.Single() :?> MenuBarTerminalElement

  let menuBarItemElement =
    (menuBarElement.Props
     |> Props.find PKey.MenuBar.children)
      .Single()
    :?> MenuBarTerminalElement

  let popoverMenu =
    menuBarItemElement.Props
    |> Props.find PKey.MenuBarItem.PopoverMenu_element
    :?> PopoverMenuTerminalElement

  let popoverMenuRoot =
    popoverMenu.Props
    |> Props.find PKey.PopoverMenu.Root_element

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
    View.Runnable [
      View.MenuBar (fun p m ->
        m.MenuBarItem (fun p m ->
          p.Title "MenuBarItem"

          m.MenuItems [
            View.MenuItem (fun p -> p.Title "MenuItem 0")
            View.MenuItem (fun p -> p.Title "MenuItem 1")
          ]
        )
      ) :> ITerminalElement
    ]
    :?> IInternalTerminalElement

  let menuBarElement =
    view.Children.Single() :?> MenuBarTerminalElement

  let menuBarItemElement =
    (menuBarElement.Props
     |> Props.find PKey.MenuBar.children)
      .Single()
    :?> MenuBarItemTerminalElement

  let popoverMenu =
    menuBarItemElement.Props
    |> Props.find PKey.MenuBarItem.PopoverMenu_element
    :?> PopoverMenuTerminalElement

  let popoverMenuRoot =
    popoverMenu.Props
    |> Props.find PKey.PopoverMenu.Root_element


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
