module Terminal.Gui.Elmish.Tests.PropKeyTests

open NUnit.Framework
open Terminal.Gui.Elmish

[<Test>]
let ``PropKey and PropKey<_> are equal when raw key is same`` () =
  let rawKey: RawPropKey = "MyView.Parent_viewSpec"

  let typed = PropKey.Create.subElement<IViewTE> (10000, 9999, rawKey)
  let untyped: PropKey = typed.Untyped

  Assert.That(untyped.Equals typed, Is.True)
  Assert.That(typed.Equals untyped, Is.True)

[<Test>]
let ``PropKey and PropKey<_> have same hashcode when raw key is same`` () =
  let rawKey: RawPropKey = "MyView.Parent_viewSpec"

  let typed = PropKey.Create.subElement<IViewTE> (10000, 9999, rawKey)
  let untyped: PropKey = typed.Untyped

  Assert.That(untyped.GetHashCode(), Is.EqualTo(typed.GetHashCode()))
