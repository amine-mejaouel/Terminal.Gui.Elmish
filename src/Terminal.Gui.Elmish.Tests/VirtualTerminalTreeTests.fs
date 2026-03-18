module Terminal.Gui.Elmish.Tests.VirtualTerminalTreeTests

open System
open System.Collections.Generic
open NUnit.Framework
open Terminal.Gui.Elmish
open Terminal.Gui.ViewBase
open Terminal.Gui.Views

let private asViewNode (node: VttNode) =
  match node with
  | VttNode.ViewNode vn -> vn
  | VttNode.ElmishComponentNode _ -> failwith "Expected ViewNode, got ElmishComponentNode"

/// Minimal IViewTE stub for use in Address values.
type private StubViewTE(initialAddress: Address) =
  let mutable addr = initialAddress

  interface ITerminalElement

  interface IDisposable with
    member _.Dispose() = ()

  interface ITerminalElementBase with
    member _.Address
      with get () = addr
      and set v = addr <- v

    member _.Name = "Stub"
    member _.View = Unchecked.defaultof<_>
    member _.OnViewSet = Event<View>().Publish
    member _.GetPath() = ""

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

  ivtt.AddView(view, Address.Root)

  Assert.That(vtt.Root.IsSome, Is.True, "Root should be set")

  let rootNode = vtt.Root.Value |> asViewNode

  Assert.Multiple(fun () ->
    Assert.That(rootNode.View, Is.SameAs(view), "Root view should match")
    Assert.That(rootNode.Children.Count, Is.EqualTo(0), "Root should have no children")
    Assert.That(rootNode.SubElements.Count, Is.EqualTo(0), "Root should have no sub-elements")
    Assert.That(rootNode.Address, Is.EqualTo(Address.Root), "Root address should be Address.Root"))

[<Test>]
let ``AddView with Child address adds a child node to the root`` () =
  let vtt = VirtualTerminalTree()
  let ivtt = vtt :> IVirtualTerminalTree

  let rootView = new Button()
  let childView = new Label()

  let parentTE = StubViewTE(Address.Root) :> IViewTE

  ivtt.AddView(rootView, Address.Root)
  ivtt.AddView(childView, Address.Child(parentTE, 0))

  let rootNode = vtt.Root.Value |> asViewNode

  Assert.That(rootNode.Children.Count, Is.EqualTo(1), "Root should have 1 child")

  let childNode = rootNode.Children.[0] |> asViewNode

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

  let parentTE = StubViewTE(Address.Root) :> IViewTE

  ivtt.AddView(rootView, Address.Root)
  ivtt.AddView(child0, Address.Child(parentTE, 0))
  ivtt.AddView(child1, Address.Child(parentTE, 1))
  ivtt.AddView(child2, Address.Child(parentTE, 2))

  let rootNode = vtt.Root.Value |> asViewNode

  Assert.That(rootNode.Children.Count, Is.EqualTo(3), "Root should have 3 children")

  Assert.Multiple(fun () ->
    Assert.That((rootNode.Children.[0] |> asViewNode).View, Is.SameAs(child0), "First child")
    Assert.That((rootNode.Children.[1] |> asViewNode).View, Is.SameAs(child1), "Second child")
    Assert.That((rootNode.Children.[2] |> asViewNode).View, Is.SameAs(child2), "Third child"))

[<Test>]
let ``AddView with nested children creates multi-level tree`` () =
  let vtt = VirtualTerminalTree()
  let ivtt = vtt :> IVirtualTerminalTree

  let rootView = new Button()
  let middleView = new FrameView()
  let leafLabel = new Label()
  let leafButton = new Button()

  let rootTE = StubViewTE(Address.Root) :> IViewTE
  let middleTE = StubViewTE(Address.Child(rootTE, 0)) :> IViewTE

  // Build tree: Root -> middle -> [leafLabel, leafButton]
  ivtt.AddView(rootView, Address.Root)
  ivtt.AddView(middleView, Address.Child(rootTE, 0))
  ivtt.AddView(leafLabel, Address.Child(middleTE, 0))
  ivtt.AddView(leafButton, Address.Child(middleTE, 1))

  let rootNode = vtt.Root.Value |> asViewNode

  Assert.That(rootNode.Children.Count, Is.EqualTo(1), "Root should have 1 child (the middle node)")

  let middleNode = rootNode.Children.[0] |> asViewNode

  Assert.That(middleNode.Children.Count, Is.EqualTo(2), "Middle node should have 2 children")

  Assert.Multiple(fun () ->
    Assert.That((middleNode.Children.[0] |> asViewNode).View, Is.SameAs(leafLabel), "First leaf should be label")
    Assert.That((middleNode.Children.[1] |> asViewNode).View, Is.SameAs(leafButton), "Second leaf should be button"))

[<Test>]
let ``AddView with SubElement address stores node in SubElements dictionary`` () =
  let vtt = VirtualTerminalTree()
  let ivtt = vtt :> IVirtualTerminalTree

  let rootView = new Button()
  let subView = new Label()

  let rootTE = StubViewTE(Address.Root) :> IViewTE

  ivtt.AddView(rootView, Address.Root)
  ivtt.AddView(subView, Address.SubElement(rootTE, None, "title_element"))

  let rootNode = vtt.Root.Value |> asViewNode

  Assert.Multiple(fun () ->
    Assert.That(rootNode.Children.Count, Is.EqualTo(0), "Sub-elements should not appear as children")
    Assert.That(rootNode.SubElements.Count, Is.EqualTo(1), "Should have 1 sub-element")
    Assert.That(rootNode.SubElements.ContainsKey("title_element", None), Is.True, "Sub-element key should match"))

  let subNode = rootNode.SubElements.[("title_element", None)] |> asViewNode

  Assert.That(subNode.View, Is.SameAs(subView), "Sub-element view should match")

[<Test>]
let ``AddView with indexed SubElement stores with correct index`` () =
  let vtt = VirtualTerminalTree()
  let ivtt = vtt :> IVirtualTerminalTree

  let rootView = new Button()
  let sub0 = new Label()
  let sub1 = new Label()

  let rootTE = StubViewTE(Address.Root) :> IViewTE

  ivtt.AddView(rootView, Address.Root)
  ivtt.AddView(sub0, Address.SubElement(rootTE, Some 0, "items_element"))
  ivtt.AddView(sub1, Address.SubElement(rootTE, Some 1, "items_element"))

  let rootNode = vtt.Root.Value |> asViewNode

  Assert.Multiple(fun () ->
    Assert.That(rootNode.SubElements.Count, Is.EqualTo(2), "Should have 2 indexed sub-elements")
    Assert.That(rootNode.SubElements.ContainsKey("items_element", Some 0), Is.True, "Index 0 present")
    Assert.That(rootNode.SubElements.ContainsKey("items_element", Some 1), Is.True, "Index 1 present")

    Assert.That(
      (rootNode.SubElements.[("items_element", Some 0)] |> asViewNode).View,
      Is.SameAs(sub0),
      "Index 0 view should match"
    )

    Assert.That(
      (rootNode.SubElements.[("items_element", Some 1)] |> asViewNode).View,
      Is.SameAs(sub1),
      "Index 1 view should match"
    ))

[<Test>]
let ``AddView stores the correct Address on each node`` () =
  let vtt = VirtualTerminalTree()
  let ivtt = vtt :> IVirtualTerminalTree

  let rootView = new Button()
  let childView = new Label()

  let rootTE = StubViewTE(Address.Root) :> IViewTE
  let childAddress = Address.Child(rootTE, 0)

  ivtt.AddView(rootView, Address.Root)
  ivtt.AddView(childView, childAddress)

  let rootNode = vtt.Root.Value |> asViewNode
  let childNode = rootNode.Children.[0] |> asViewNode

  Assert.Multiple(fun () ->
    Assert.That(rootNode.Address, Is.EqualTo(Address.Root), "Root address should be Root")
    Assert.That(childNode.Address, Is.EqualTo(childAddress), "Child address should match"))
