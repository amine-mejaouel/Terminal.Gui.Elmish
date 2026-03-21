module Terminal.Gui.Elmish.Tests.VirtualTerminalTreeTests

open System
open System.Collections.Generic
open NUnit.Framework
open Terminal.Gui.Elmish
open Terminal.Gui.ViewBase
open Terminal.Gui.Views

/// Minimal IViewTE stub for use in Address values.
type private StubViewTE(initialAddress: Address) =
  let mutable addr = initialAddress
  let mutable parentView: View option = None

  interface ITerminalElement

  interface IDisposable with
    member _.Dispose() = ()

  interface ITerminalElementBase with
    member _.Address
      with get () = addr
      and set v = addr <- v

    member _.ParentView
      with get () = parentView
      and set v = parentView <- v

    member _.Name = "Stub"
    member _.View = Unchecked.defaultof<_>
    member _.OnViewSet = Event<View>().Publish

  interface IViewTE with
    member _.Props = Props()
    member _.SetAsChildOfParentView = true
    member _.Children = List<TerminalElement>()
    member _.InitializeTree _ _ = ()
    member _.Reuse _ = ()

[<Test>]
let ``AddView with Root address sets the root node`` () =
  let vtt = VirtualTerminalTree()
  let ivtt = vtt :> IVirtualTerminalTree
  let view = new Button()

  ivtt.AddView(view, [ Root ], true)

  Assert.That(vtt.Root.IsSome, Is.True, "Root should be set")

  let rootNode = vtt.Root.Value

  Assert.Multiple(fun () ->
    Assert.That(rootNode.View, Is.SameAs(view), "Root view should match")
    Assert.That(rootNode.Children.Count, Is.EqualTo(0), "Root should have no children")
    Assert.That(rootNode.SubElements.Count, Is.EqualTo(0), "Root should have no sub-elements")
    Assert.That(rootNode.Address, Is.EqualTo<Address>([ Root ]), "Root address should be [Root]"))

[<Test>]
let ``AddView with Child address adds a child node to the root`` () =
  let vtt = VirtualTerminalTree()
  let ivtt = vtt :> IVirtualTerminalTree

  let rootView = new Button()
  let childView = new Label()

  ivtt.AddView(rootView, [ Root ], true)
  ivtt.AddView(childView, [ Root; Child(0, false) ], true)

  let rootNode = vtt.Root.Value

  Assert.That(rootNode.Children.Count, Is.EqualTo(1), "Root should have 1 child")

  let childNode = rootNode.Children[0]

  Assert.Multiple(fun () ->
    Assert.That(childNode.View, Is.SameAs(childView), "Child view should match")
    Assert.That(childNode.Children.Count, Is.EqualTo(0), "Child should have no children"))

[<Test>]
let ``AddView with multiple children preserves insertion order`` () =
  let vtt = VirtualTerminalTree()
  let ivtt = vtt :> IVirtualTerminalTree

  let rootView = new Button()
  let child0 = new Label()
  let child1 = new Button()
  let child2 = new Label()

  ivtt.AddView(rootView, [ Root ])
  ivtt.AddView(child0, [ Root; Child(0, false) ])
  ivtt.AddView(child1, [ Root; Child(1, false) ])
  ivtt.AddView(child2, [ Root; Child(2, false) ])

  let rootNode = vtt.Root.Value

  Assert.That(rootNode.Children.Count, Is.EqualTo(3), "Root should have 3 children")

  Assert.Multiple(fun () ->
    Assert.That(rootNode.Children.[0].View, Is.SameAs(child0), "First child")
    Assert.That(rootNode.Children.[1].View, Is.SameAs(child1), "Second child")
    Assert.That(rootNode.Children.[2].View, Is.SameAs(child2), "Third child"))

[<Test>]
let ``AddView with nested children creates multi-level tree`` () =
  let vtt = VirtualTerminalTree()
  let ivtt = vtt :> IVirtualTerminalTree

  let rootView = new Button()
  let middleView = new FrameView()
  let leafLabel = new Label()
  let leafButton = new Button()

  let rootTE = new StubViewTE([ Root ]) :> IViewTE
  let middleTE = new StubViewTE([ Root; Child(0, false) ]) :> IViewTE

  // Build tree: Root -> middle -> [leafLabel, leafButton]
  ivtt.AddView(rootView, [ Root ])
  ivtt.AddView(middleView, [ Root; Child(0, false) ])
  ivtt.AddView(leafLabel, [ Root; Child(0, false); Child(0, false) ])
  ivtt.AddView(leafButton, [ Root; Child(0, false); Child(1, false) ])

  let rootNode = vtt.Root.Value

  Assert.That(rootNode.Children.Count, Is.EqualTo(1), "Root should have 1 child (the middle node)")

  let middleNode = rootNode.Children.[0]

  Assert.That(middleNode.Children.Count, Is.EqualTo(2), "Middle node should have 2 children")

  Assert.Multiple(fun () ->
    Assert.That((middleNode.Children.[0]).View, Is.SameAs(leafLabel), "First leaf should be label")
    Assert.That((middleNode.Children.[1]).View, Is.SameAs(leafButton), "Second leaf should be button"))

[<Test>]
let ``AddView with SubElement address stores node in SubElements dictionary`` () =
  let vtt = VirtualTerminalTree()
  let ivtt = vtt :> IVirtualTerminalTree

  let rootView = new Button()
  let subView = new Label()

  let rootTE = StubViewTE([ Root ]) :> IViewTE

  ivtt.AddView(rootView, [ Root ])
  ivtt.AddView(subView, [ Root; SubElement(None, "title_element", false) ])

  let rootNode = vtt.Root.Value

  Assert.Multiple(fun () ->
    Assert.That(rootNode.Children.Count, Is.EqualTo(0), "Sub-elements should not appear as children")
    Assert.That(rootNode.SubElements.Count, Is.EqualTo(1), "Should have 1 sub-element")
    Assert.That(rootNode.SubElements.ContainsKey("title_element", None), Is.True, "Sub-element key should match"))

  let subNode = rootNode.SubElements.[("title_element", None)]

  Assert.That(subNode.View, Is.SameAs(subView), "Sub-element view should match")

[<Test>]
let ``AddView with indexed SubElement stores with correct index`` () =
  let vtt = VirtualTerminalTree()
  let ivtt = vtt :> IVirtualTerminalTree

  let rootView = new Button()
  let sub0 = new Label()
  let sub1 = new Label()

  let rootTE = StubViewTE([ Root ]) :> IViewTE

  ivtt.AddView(rootView, [ Root ])
  ivtt.AddView(sub0, [ Root; SubElement(Some 0, "items_element", false) ])
  ivtt.AddView(sub1, [ Root; SubElement(Some 1, "items_element", false) ])

  let rootNode = vtt.Root.Value

  Assert.Multiple(fun () ->
    Assert.That(rootNode.SubElements.Count, Is.EqualTo(2), "Should have 2 indexed sub-elements")
    Assert.That(rootNode.SubElements.ContainsKey("items_element", Some 0), Is.True, "Index 0 present")
    Assert.That(rootNode.SubElements.ContainsKey("items_element", Some 1), Is.True, "Index 1 present")

    Assert.That((rootNode.SubElements.[("items_element", Some 0)]).View, Is.SameAs(sub0), "Index 0 view should match")

    Assert.That((rootNode.SubElements.[("items_element", Some 1)]).View, Is.SameAs(sub1), "Index 1 view should match"))

[<Test>]
let ``AddView stores the correct Address on each node`` () =
  let vtt = VirtualTerminalTree()
  let ivtt = vtt :> IVirtualTerminalTree

  let rootView = new Button()
  let childView = new Label()

  let rootTE = StubViewTE([ Root ]) :> IViewTE
  let childAddress: Address = [ Root; Child(0, false) ]

  ivtt.AddView(rootView, [ Root ])
  ivtt.AddView(childView, childAddress)

  let rootNode = vtt.Root.Value
  let childNode = rootNode.Children[0]

  Assert.Multiple(fun () ->
    Assert.That(rootNode.Address, Is.EqualTo<Address>([ Root ]), "Root address should be Root")
    Assert.That(childNode.Address, Is.EqualTo<Address>(childAddress), "Child address should match"))
