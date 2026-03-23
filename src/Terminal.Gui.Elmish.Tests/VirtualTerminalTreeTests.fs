module Terminal.Gui.Elmish.Tests.VirtualTerminalTreeTests

open System
open System.Collections.Generic
open NUnit.Framework
open Terminal.Gui.Elmish
open Terminal.Gui.ViewBase
open Terminal.Gui.Views

/// Minimal IViewTE stub for use in Address values.
type private StubViewTE(initialAddress: Address, view: View) =
  let mutable addr = initialAddress
  let mutable parentView: View option = None

  member _.View = view
  member this.VttNode = VttNode.fromViewTE (this, initialAddress)

  interface ITerminalElement

  interface IDisposable with
    member _.Dispose() = ()

  interface ITerminalElementBase with
    member _.Address
      with get () = addr
      and set v = addr <- v

    member _.Name = "Stub"
    member _.View = view
    member _.OnViewSet = Event<View>().Publish

  interface IViewTE with
    member _.Props = Props()
    member _.SetAsChildOfParentView = true
    member _.Children = List<TE>()
    member _.InitializeTree _ _ = ()
    member _.Reuse _ = ()

[<Test>]
let ``AddView with Root address sets the root node`` () =
  let vtt = VirtualTerminalTree()
  let ivtt = vtt :> IVirtualTerminalTree
  let te = new StubViewTE([ Root ], new Button())

  ivtt.AddView(te.VttNode)

  Assert.That(vtt.Root.IsSome, Is.True, "Root should be set")

  let rootNode = vtt.Root.Value

  Assert.Multiple(fun () ->
    Assert.That(rootNode.View, Is.SameAs(te.View), "Root view should match")
    Assert.That(rootNode.Children.Count, Is.EqualTo(0), "Root should have no children")
    Assert.That(rootNode.SubElements.Count, Is.EqualTo(0), "Root should have no sub-elements")
    Assert.That(rootNode.Address, Is.EqualTo<Address>([ Root ]), "Root address should be [Root]"))

[<Test>]
let ``AddView with Child address adds a child node to the root`` () =
  let vtt = VirtualTerminalTree()
  let ivtt = vtt :> IVirtualTerminalTree

  let rootTE = new StubViewTE([ Root ], new Button())
  let childTE = new StubViewTE([ Root; Child(0, false) ], new Label())

  ivtt.AddView(rootTE.VttNode)
  ivtt.AddView(childTE.VttNode)

  let rootNode = vtt.Root.Value

  Assert.That(rootNode.Children.Count, Is.EqualTo(1), "Root should have 1 child")

  let childNode = rootNode.Children[0]

  Assert.Multiple(fun () ->
    Assert.That(childNode.View, Is.SameAs(childTE.View), "Child view should match")
    Assert.That(childNode.Children.Count, Is.EqualTo(0), "Child should have no children"))

[<Test>]
let ``AddView with multiple children preserves insertion order`` () =
  let vtt = VirtualTerminalTree()
  let ivtt = vtt :> IVirtualTerminalTree

  let rootTE = new StubViewTE([ Root ], new Button())
  let child0TE = new StubViewTE([ Root; Child(0, false) ], new Label())
  let child1TE = new StubViewTE([ Root; Child(1, false) ], new Button())
  let child2TE = new StubViewTE([ Root; Child(2, false) ], new Label())

  ivtt.AddView(rootTE.VttNode)
  ivtt.AddView(child0TE.VttNode)
  ivtt.AddView(child1TE.VttNode)
  ivtt.AddView(child2TE.VttNode)

  let rootNode = vtt.Root.Value

  Assert.That(rootNode.Children.Count, Is.EqualTo(3), "Root should have 3 children")

  Assert.Multiple(fun () ->
    Assert.That(rootNode.Children.[0].View, Is.SameAs(child0TE.View), "First child")
    Assert.That(rootNode.Children.[1].View, Is.SameAs(child1TE.View), "Second child")
    Assert.That(rootNode.Children.[2].View, Is.SameAs(child2TE.View), "Third child"))

[<Test>]
let ``AddView with nested children creates multi-level tree`` () =
  let vtt = VirtualTerminalTree()
  let ivtt = vtt :> IVirtualTerminalTree

  let rootTE = new StubViewTE([ Root ], new Button())
  let middleTE = new StubViewTE([ Root; Child(0, false) ], new FrameView())

  let leafLabelTE =
    new StubViewTE([ Root; Child(0, false); Child(0, false) ], new Label())

  let leafButtonTE =
    new StubViewTE([ Root; Child(0, false); Child(1, false) ], new Button())

  // Build tree: Root -> middle -> [leafLabel, leafButton]
  ivtt.AddView(rootTE.VttNode)
  ivtt.AddView(middleTE.VttNode)
  ivtt.AddView(leafLabelTE.VttNode)
  ivtt.AddView(leafButtonTE.VttNode)

  let rootNode = vtt.Root.Value

  Assert.That(rootNode.Children.Count, Is.EqualTo(1), "Root should have 1 child (the middle node)")

  let middleNode = rootNode.Children.[0]

  Assert.That(middleNode.Children.Count, Is.EqualTo(2), "Middle node should have 2 children")

  Assert.Multiple(fun () ->
    Assert.That((middleNode.Children.[0]).View, Is.SameAs(leafLabelTE.View), "First leaf should be label")
    Assert.That((middleNode.Children.[1]).View, Is.SameAs(leafButtonTE.View), "Second leaf should be button"))

[<Test>]
let ``AddView with SubElement address stores node in SubElements dictionary`` () =
  let vtt = VirtualTerminalTree()
  let ivtt = vtt :> IVirtualTerminalTree

  let rootTE = new StubViewTE([ Root ], new Button())

  let subTE =
    new StubViewTE([ Root; SubElement(None, "title_element", false) ], new Label())

  ivtt.AddView(rootTE.VttNode)
  ivtt.AddView(subTE.VttNode)

  let rootNode = vtt.Root.Value

  Assert.Multiple(fun () ->
    Assert.That(rootNode.Children.Count, Is.EqualTo(0), "Sub-elements should not appear as children")
    Assert.That(rootNode.SubElements.Count, Is.EqualTo(1), "Should have 1 sub-element")
    Assert.That(rootNode.SubElements.ContainsKey("title_element", None), Is.True, "Sub-element key should match"))

  let subNode = rootNode.SubElements.[("title_element", None)]

  Assert.That(subNode.View, Is.SameAs(subTE.View), "Sub-element view should match")

[<Test>]
let ``AddView with indexed SubElement stores with correct index`` () =
  let vtt = VirtualTerminalTree()
  let ivtt = vtt :> IVirtualTerminalTree

  let rootTE = new StubViewTE([ Root ], new Button())

  let sub0TE =
    new StubViewTE([ Root; SubElement(Some 0, "items_element", false) ], new Label())

  let sub1TE =
    new StubViewTE([ Root; SubElement(Some 1, "items_element", false) ], new Label())

  ivtt.AddView(rootTE.VttNode)
  ivtt.AddView(sub0TE.VttNode)
  ivtt.AddView(sub1TE.VttNode)

  let rootNode = vtt.Root.Value

  Assert.Multiple(fun () ->
    Assert.That(rootNode.SubElements.Count, Is.EqualTo(2), "Should have 2 indexed sub-elements")
    Assert.That(rootNode.SubElements.ContainsKey("items_element", Some 0), Is.True, "Index 0 present")
    Assert.That(rootNode.SubElements.ContainsKey("items_element", Some 1), Is.True, "Index 1 present")

    Assert.That(
      (rootNode.SubElements.[("items_element", Some 0)]).View,
      Is.SameAs(sub0TE.View),
      "Index 0 view should match"
    )

    Assert.That(
      (rootNode.SubElements.[("items_element", Some 1)]).View,
      Is.SameAs(sub1TE.View),
      "Index 1 view should match"
    ))

[<Test>]
let ``AddView stores the correct Address on each node`` () =
  let vtt = VirtualTerminalTree()
  let ivtt = vtt :> IVirtualTerminalTree

  let rootTE = new StubViewTE([ Root ], new Button())
  let childTE = new StubViewTE([ Root; Child(0, false) ], new Label())

  ivtt.AddView(rootTE.VttNode)
  ivtt.AddView(childTE.VttNode)

  let rootNode = vtt.Root.Value
  let childNode = rootNode.Children[0]

  Assert.Multiple(fun () ->
    Assert.That(rootNode.Address, Is.EqualTo<Address>([ Root ]), "Root address should be Root")
    Assert.That(childNode.Address, Is.EqualTo<Address>([ Root; Child(0, false) ]), "Child address should match"))
