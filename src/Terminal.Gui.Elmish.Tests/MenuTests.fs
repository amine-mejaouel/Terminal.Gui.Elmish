module Terminal.Gui.Elmish.Tests.Tests

open System.Linq
open NUnit.Framework
open Terminal.Gui.Elmish
open Terminal.Gui.Views

[<Test>]
let ``Using properties syntax: Menu should be correctly set`` () =
  let viewTE =
    View.Runnable
      [ View.MenuBar(fun p m ->
          p.Children
            [ View.MenuBarItem(fun p m ->
                p.Title "MenuBarItem"

                p.PopoverMenu(
                  View.PopoverMenu(fun p ->
                    p.Root(
                      View.Menu(fun p ->
                        p.Children
                          [ View.MenuItem(fun p -> p.Title "MenuItem 0")
                            View.MenuItem(fun p -> p.Title "MenuItem 1") ])
                    ))
                )) ])
        :> ITerminalElement ]
    :?> IViewTE

  let menuBarElement =
    viewTE.Children.Single().GetViewBackedTE() :?> MenuBarTerminalElement

  let menuBarItemElement =
    menuBarElement.Props.Children.Single().GetViewBackedTE() :?> MenuBarItemTerminalElement

  let popoverMenu =
    menuBarItemElement.Props |> Props.find PKey.MenuBarItem.PopoverMenu_element :?> PopoverMenuTerminalElement

  let popoverMenuRoot = popoverMenu.Props |> Props.find PKey.PopoverMenu.Root_element

  let view = (ElmishTester.render viewTE).View

  let menuBar = (view.SubViews |> Seq.head) :?> MenuBar

  let menuBarItem = (menuBar.SubViews |> Seq.head) :?> MenuBarItem

  let popoverMenuRoot = menuBarItem.PopoverMenu.Root

  let menuItems = popoverMenuRoot.SubViews |> Seq.map unbox<MenuItem> |> Seq.toArray

  Assert.That(menuItems[0].Title, Is.EqualTo("MenuItem 0"))
  Assert.That(menuItems[1].Title, Is.EqualTo("MenuItem 1"))

[<Test>]
let ``Using macros syntax: Menu should be correctly set`` () =
  let viewTE =
    View.Runnable
      [ View.MenuBar(fun p m ->
          m.MenuBarItem(fun p m ->
            p.Title "MenuBarItem"

            m.MenuItems
              [ View.MenuItem(fun p -> p.Title "MenuItem 0")
                View.MenuItem(fun p -> p.Title "MenuItem 1") ]))
        :> ITerminalElement ]
    :?> IViewTE

  let menuBarElement =
    viewTE.Children.Single().GetViewBackedTE() :?> MenuBarTerminalElement

  let menuBarItemElement =
    menuBarElement.Props.Children.Single().GetViewBackedTE() :?> MenuBarItemTerminalElement

  let popoverMenu =
    menuBarItemElement.Props |> Props.find PKey.MenuBarItem.PopoverMenu_element :?> PopoverMenuTerminalElement

  let popoverMenuRoot = popoverMenu.Props |> Props.find PKey.PopoverMenu.Root_element


  let view = (ElmishTester.render viewTE).View

  let menuBar = (view.SubViews |> Seq.head) :?> MenuBar

  let menuBarItem = (menuBar.SubViews |> Seq.head) :?> MenuBarItem

  let popoverMenuRoot = menuBarItem.PopoverMenu.Root

  let menuItems = popoverMenuRoot.SubViews |> Seq.map unbox<MenuItem> |> Seq.toArray

  Assert.That(menuItems[0].Title, Is.EqualTo("MenuItem 0"))
  Assert.That(menuItems[1].Title, Is.EqualTo("MenuItem 1"))


[<Test>]
let ``Sub-elements are not added to parent SubViews by Elmish traverse`` () =
  // Shortcut.TargetView is a sub-element whose setter is a plain auto-property (no View.Add).
  // If Elmish's traverse incorrectly calls View.Add for sub-elements, the TargetView
  // will appear in the Shortcut's SubViews. With the fix, only children get View.Add'd
  // by the traverse; sub-elements are wired through their property setters only.
  let viewTE =
    View.Runnable [ View.Shortcut(fun p -> p.TargetView(View.Label(fun p -> p.Text "target"))) :> ITerminalElement ]
    :?> IViewTE

  let view = (ElmishTester.render viewTE).View
  let shortcut = view.SubViews |> Seq.head :?> Shortcut

  // The TargetView property should be set
  Assert.That(shortcut.TargetView, Is.Not.Null)
  Assert.That(shortcut.TargetView, Is.InstanceOf<Label>())

  // But TargetView must NOT appear in the Shortcut's SubViews —
  // it's a sub-element, not a child. Only children get View.Add'd by the traverse.
  Assert.That(
    shortcut.SubViews
    |> Seq.exists (fun sv -> obj.ReferenceEquals(sv, shortcut.TargetView)),
    Is.False,
    "TargetView (a sub-element) should not be added to Shortcut.SubViews"
  )
