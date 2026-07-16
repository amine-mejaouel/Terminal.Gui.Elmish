module Terminal.Gui.Elmish.Tests.PropKeyTests

open NUnit.Framework
open Terminal.Gui.Elmish

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
