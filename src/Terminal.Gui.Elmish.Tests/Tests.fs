module Terminal.Gui.Elmish.Tests.Tests

open System.Linq
open NUnit.Framework
open Terminal.Gui.Elmish
open Terminal.Gui.Elmish.Elements
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
    view.elementData.children.Single() :?> MenuBarElement

  let menuBarItemElement =
    (menuBarElement.elementData.Props
     |> Props.find PKey.menuBar.children)
      .Single()
    :?> MenuBarItemElement

  let popoverMenu =
    menuBarItemElement.elementData.Props
    |> Props.find PKey.menuBarItem.popoverMenu_element
    :?> PopoverMenuElement

  let popoverMenuRoot =
    popoverMenu.elementData.Props
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
    view.elementData.children.Single() :?> MenuBarElement

  let menuBarItemElement =
    (menuBarElement.elementData.Props
     |> Props.find PKey.menuBar.children)
      .Single()
    :?> MenuBarItemElement

  let popoverMenu =
    menuBarItemElement.elementData.Props
    |> Props.find PKey.menuBarItem.popoverMenu_element
    :?> PopoverMenuElement

  let popoverMenuRoot =
    popoverMenu.elementData.Props
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
let ``ITerminalElementData.Children returns data-only children list`` () =
  // Arrange: Create a simple view hierarchy
  let view =
    View.runnable [
      View.frameView (fun p ->
        p.children [
          View.button (fun p -> p.text "Button 1")
          View.button (fun p -> p.text "Button 2")
          View.label (fun p -> p.text "Label 1")
        ]
      )
    ]
    :?> IInternalTerminalElement

  // Initialize the tree
  view.initializeTree None

  // Act: Access children through the public ITerminalElementData interface
  let dataInterface = view.elementData :> ITerminalElementData
  let dataChildren = dataInterface.Children

  // Assert: Children should be a list of ITerminalElementData
  Assert.That(dataChildren, Is.Not.Null)
  Assert.That(dataChildren, Is.InstanceOf<ITerminalElementData list>())
  Assert.That(dataChildren.Length, Is.EqualTo(1), "Should have one child (FrameView)")
  
  // Verify the child is a FrameView
  let frameViewData = dataChildren.[0]
  Assert.That(frameViewData.Name, Is.EqualTo("FrameView"))
  
  // Verify FrameView has three children (buttons and label)
  let frameChildren = frameViewData.Children
  Assert.That(frameChildren.Length, Is.EqualTo(3), "FrameView should have 3 children")
  
  // Verify the children names
  let childNames = frameChildren |> List.map (fun c -> c.Name)
  Assert.That(childNames, Is.EquivalentTo(["Button"; "Button"; "Label"]))

[<Test>]
let ``IElementData.children returns runtime children (backward compatibility)`` () =
  // Arrange: Create a simple view hierarchy
  let view =
    View.runnable [
      View.frameView (fun p ->
        p.children [
          View.button (fun p -> p.text "Button 1")
          View.label (fun p -> p.text "Label 1")
        ]
      )
    ]
    :?> IInternalTerminalElement

  // Initialize the tree
  view.initializeTree None

  // Act: Access internal children (runtime wrappers)
  let internalChildren = view.elementData.children

  // Assert: children should be List<IInternalTerminalElement>
  Assert.That(internalChildren, Is.Not.Null)
  Assert.That(internalChildren, Is.InstanceOf<System.Collections.Generic.List<IInternalTerminalElement>>())
  Assert.That(internalChildren.Count, Is.EqualTo(1), "Should have one child (FrameView)")
  
  // Verify it's a runtime element with full capabilities
  let frameView = internalChildren.[0]
  Assert.That(frameView.name, Is.EqualTo("FrameView"))
  Assert.That(frameView.view, Is.Not.Null, "Runtime element should have view")
  
  // Verify nested children are also runtime elements
  let frameChildren = frameView.elementData.children
  Assert.That(frameChildren.Count, Is.EqualTo(2))

[<Test>]
let ``Data and runtime children represent the same tree structure`` () =
  // Arrange: Create a view hierarchy
  let view =
    View.runnable [
      View.window (fun p ->
        p.children [
          View.button (fun p -> p.text "Button")
          View.frameView (fun p ->
            p.children [
              View.label (fun p -> p.text "Nested Label")
            ]
          )
        ]
      )
    ]
    :?> IInternalTerminalElement

  // Initialize the tree
  view.initializeTree None

  // Act: Get both data and runtime representations
  let dataInterface = view.elementData :> ITerminalElementData
  let dataChildren = dataInterface.Children
  let runtimeChildren = view.elementData.children

  // Assert: Same number of children
  Assert.That(dataChildren.Length, Is.EqualTo(runtimeChildren.Count))
  
  // Assert: Names match between data and runtime
  let dataNames = dataChildren |> List.map (fun c -> c.Name) |> List.sort
  let runtimeNames = runtimeChildren |> Seq.map (fun c -> c.name) |> Seq.sort |> Seq.toList
  Assert.That(dataNames, Is.EquivalentTo(runtimeNames))

[<Test>]
let ``ITerminalElementData.Name returns element type`` () =
  // Arrange: Create various element types
  let button = View.button (fun p -> p.text "Test") :?> IInternalTerminalElement
  let label = View.label (fun p -> p.text "Test") :?> IInternalTerminalElement
  let frameView = View.frameView (fun _ -> ()) :?> IInternalTerminalElement
  
  // Initialize elements
  button.initializeTree None
  label.initializeTree None
  frameView.initializeTree None
  
  // Act: Access via ITerminalElementData interface
  let buttonData = button.elementData :> ITerminalElementData
  let labelData = label.elementData :> ITerminalElementData
  let frameData = frameView.elementData :> ITerminalElementData
  
  // Assert: Names match element types
  Assert.That(buttonData.Name, Is.EqualTo("Button"))
  Assert.That(labelData.Name, Is.EqualTo("Label"))
  Assert.That(frameData.Name, Is.EqualTo("FrameView"))
