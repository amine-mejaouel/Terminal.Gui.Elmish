module Terminal.Gui.Elmish.Tests.TerminalElementIdTests

open NUnit.Framework
open Terminal.Gui.Elmish
open Terminal.Gui.Elmish.ElmishTerminal
open Elmish

// Helper function to start an elmish component and get its TerminalElement
let private startComponent init update view =
  let elmishComponent = ElmishTerminal.mkSimpleComponent init update view
  let internalElement = elmishComponent :?> IElmishComponent_TerminalElement
  internalElement.StartElmishLoop(Root)
  elmishComponent :?> IInternalTerminalElement

[<Test>]
let ``Simple Elmish program: Root element should have Root origin`` () =
  let init () = ()
  let update _ model = model
  let view _ _ = View.Button(fun _ -> ())
  
  let terminalElement = startComponent init update view
  
  let expectedId = "root"
  Assert.That(terminalElement.Id.ToString(), Is.EqualTo(expectedId))

[<Test>]
let ``Simple Elmish program: Child element should have correct origin`` () =
  let init () = ()
  let update _ model = model
  let view _ _ = 
    View.Window (fun p ->
      p.Children [
        View.Button(fun _ -> ())
      ]
    )
  
  let terminalElement = startComponent init update view
  
  // The root element should be "root"
  Assert.That(terminalElement.Id.ToString(), Is.EqualTo("root"))
  
  // Get the child element
  let children = terminalElement.Children
  Assert.That(children.Count, Is.EqualTo(1))
  
  let childElement = children.[0]
  let childId = childElement.Id.ToString()
  
  // Child should have format "root|child[0]"
  Assert.That(childId, Is.EqualTo("root|child[0]"))

[<Test>]
let ``Elmish program with one component: Component should have ElmishComponent origin`` () =
  // Inner component
  let innerInit () = "inner"
  let innerUpdate _ model = model
  let innerView _ _ = View.Label(fun p -> p.Text "Inner Component")
  
  // Outer component that contains the inner component
  let outerInit () = ()
  let outerUpdate _ model = model
  let outerView _ _ = 
    View.Window (fun p ->
      p.Children [
        ElmishTerminal.mkSimpleComponent innerInit innerUpdate innerView :> ITerminalElement
      ]
    )
  
  let terminalElement = startComponent outerInit outerUpdate outerView
  
  // Root should be "root"
  Assert.That(terminalElement.Id.ToString(), Is.EqualTo("root"))
  
  // Get the child component
  let children = terminalElement.Children
  Assert.That(children.Count, Is.EqualTo(1))
  
  let childComponent = children.[0]
  let childId = childComponent.Id.ToString()
  
  // Child component should have format "root|elmishComponent[0]" or similar
  // Based on the code in TerminalElement.Base.fs line 192, it should be ElmishComponent origin
  Assert.That(childId.Contains("elmishComponent"), Is.True)

[<Test>]
let ``Elmish program with nested components: Nested component should have correct origin chain`` () =
  // Innermost component
  let innermostInit () = "innermost"
  let innermostUpdate _ model = model
  let innermostView _ _ = View.Label(fun p -> p.Text "Innermost Component")
  
  // Middle component that contains innermost
  let middleInit () = "middle"
  let middleUpdate _ model = model
  let middleView _ _ = 
    View.Window (fun p ->
      p.Children [
        ElmishTerminal.mkSimpleComponent innermostInit innermostUpdate innermostView :> ITerminalElement
      ]
    )
  
  // Outer component that contains middle
  let outerInit () = ()
  let outerUpdate _ model = model
  let outerView _ _ = 
    View.Window (fun p ->
      p.Children [
        ElmishTerminal.mkSimpleComponent middleInit middleUpdate middleView :> ITerminalElement
      ]
    )
  
  let terminalElement = startComponent outerInit outerUpdate outerView
  
  // Root should be "root"
  Assert.That(terminalElement.Id.ToString(), Is.EqualTo("root"))
  
  // Get the middle component (first child)
  let children = terminalElement.Children
  Assert.That(children.Count, Is.EqualTo(1))
  
  let middleComponent = children.[0]
  let middleId = middleComponent.Id.ToString()
  
  // Middle component should have elmishComponent in its ID
  Assert.That(middleId.Contains("elmishComponent"), Is.True)
  
  // Get the innermost component (child of middle)
  let middleChildren = middleComponent.Children
  Assert.That(middleChildren.Count, Is.EqualTo(1))
  
  let innermostComponent = middleChildren.[0]
  let innermostId = innermostComponent.Id.ToString()
  
  // Innermost component should also have elmishComponent in its ID
  // and should contain the middle component's ID as a prefix
  Assert.That(innermostId.Contains("elmishComponent"), Is.True)

[<Test>]
let ``Multiple children: Each child should have unique index in origin`` () =
  let init () = ()
  let update _ model = model
  let view _ _ = 
    View.Window (fun p ->
      p.Children [
        View.Button(fun _ -> ())
        View.Label(fun p -> p.Text "Label")
        View.TextField(fun _ -> ())
      ]
    )
  
  let terminalElement = startComponent init update view
  
  // Get all children
  let children = terminalElement.Children
  Assert.That(children.Count, Is.EqualTo(3))
  
  // Check that each child has the correct index
  let child0Id = children.[0].Id.ToString()
  let child1Id = children.[1].Id.ToString()
  let child2Id = children.[2].Id.ToString()
  
  Assert.That(child0Id, Is.EqualTo("root|child[0]"))
  Assert.That(child1Id, Is.EqualTo("root|child[1]"))
  Assert.That(child2Id, Is.EqualTo("root|child[2]"))
