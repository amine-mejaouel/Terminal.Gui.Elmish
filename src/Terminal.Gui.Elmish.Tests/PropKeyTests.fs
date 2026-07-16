module Terminal.Gui.Elmish.Tests.PropKeyTests

open System
open NUnit.Framework
open Terminal.Gui.Elmish
open Terminal.Gui.ViewBase

type private TrackingTerminalElement(props: Props) =
  inherit ViewBackedTerminalElement(props)

  let mutable clearedPropertyIds = []

  member _.ClearedPropertyIds = clearedPropertyIds

  override _.Name = "Tracking"
  override _.NewView() = new View()
  override _.SetAsChildOfParentView = true

  override _.ClearProp(propertyId: PropertyId) =
    clearedPropertyIds <- propertyId :: clearedPropertyIds

[<Test>]
let ``PropKey and PropKey<_> are equal when raw key is same`` () =
  let rawKey: RawPropKey = "MyView.Parent_viewSpec"
  let viewId = PropertyId.Create 9999
  let viewSpecId = PropertyId.Create 10000

  let typed = PropKey.Create.subElement<IViewTE> (viewId, viewSpecId, rawKey)
  let untyped: PropKey = typed.Untyped

  Assert.That(untyped.Equals typed, Is.True)
  Assert.That(typed.Equals untyped, Is.True)

[<Test>]
let ``PropKey and PropKey<_> have same hashcode when raw key is same`` () =
  let rawKey: RawPropKey = "MyView.Parent_viewSpec"
  let viewId = PropertyId.Create 9999
  let viewSpecId = PropertyId.Create 10000

  let typed = PropKey.Create.subElement<IViewTE> (viewId, viewSpecId, rawKey)
  let untyped: PropKey = typed.Untyped

  Assert.That(untyped.GetHashCode(), Is.EqualTo(typed.GetHashCode()))

[<Test>]
let ``PropKey equality includes the complete subview identity`` () =
  let rawKey: RawPropKey = "MyView.Parent_viewSpec"
  let viewSpecId = PropertyId.Create 10000

  let left =
    PropKey.Create.subElement<IViewTE> (PropertyId.Create 9998, viewSpecId, rawKey)

  let right =
    PropKey.Create.subElement<IViewTE> (PropertyId.Create 9999, viewSpecId, rawKey)

  Assert.That(left.Equals right, Is.False)

[<Test>]
let ``PropKey identifies event keys without classifying values or slots as events`` () =
  Assert.Multiple(fun () ->
    Assert.That(PKey.View.Accepting.Untyped.IsEvent, Is.True)
    Assert.That(PKey.View.Title.Untyped.IsEvent, Is.False)
    Assert.That(PKey.Shortcut.TargetView.Untyped.IsEvent, Is.False)
    Assert.That(PKey.Shortcut.TargetView_viewSpec.Untyped.IsEvent, Is.False))

[<Test>]
let ``Clearing an event key bypasses generated value-property dispatch`` () =
  let props = Props()
  let event = Event<EventHandler, EventArgs>()
  let mutable calls = 0
  props |> Props.add (PKey.View.Disposing, fun _ -> calls <- calls + 1)

  let element = new TrackingTerminalElement(props)
  element.TrySetEventHandler(PKey.View.Disposing, event.Publish)
  event.Trigger(null, EventArgs.Empty)

  element.ClearProp PKey.View.Disposing.Untyped
  event.Trigger(null, EventArgs.Empty)
  element.ClearProp PKey.View.Title.Untyped

  Assert.Multiple(fun () ->
    Assert.That(calls, Is.EqualTo(1))
    Assert.That(element.ClearedPropertyIds, Is.EqualTo(box [ PKey.View.Title.id ])))
