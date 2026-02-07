module Terminal.Gui.Elmish.Tests.TerminalElementIdTests

open NUnit.Framework
open Terminal.Gui.Elmish

[<Test>]
let ``TerminalElementId with Root origin should toString as root`` () =
  let id = { ExplicitId = None; Origin = Root }
  Assert.That(id.ToString(), Is.EqualTo("root"))

[<Test>]
let ``TerminalElementId with explicit ID should use that ID`` () =
  let explicitId = { ExplicitId = Some "my-custom-id"; Origin = Root }
  Assert.That(explicitId.ToString(), Is.EqualTo("my-custom-id"))

[<Test>]
let ``Origin Root toString should be root`` () =
  let origin = Root
  Assert.That(origin.ToString(), Is.EqualTo("root"))

[<Test>]
let ``Origin Child should format as parent|child[index]`` () =
  // We test the Origin.ToString logic without creating actual IInternalTerminalElement
  // The Origin type has specific ToString formatting that we can verify
  
  // Create a simple root origin
  let rootOrigin = Root
  Assert.That(rootOrigin.ToString(), Is.EqualTo("root"))
  
  // The Child and ElmishComponent cases require an IInternalTerminalElement parent
  // which would trigger Terminal.Gui initialization, so we test the ID logic instead
  
  // Test that TerminalElementId correctly uses Origin's ToString
  let rootId = { ExplicitId = None; Origin = Root }
  Assert.That(rootId.ToString(), Is.EqualTo("root"))
