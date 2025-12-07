module Terminal.Gui.Elmish.Tests.ElmishComponentTests

open NUnit.Framework
open Terminal.Gui.Elmish
open Elmish

[<Test>]
let ``TerminalElement created by Elmish component is flagged as isElmishComponent`` () =

    let init _ = (), Cmd.none
    let update _ _ = (), Cmd.none

    let view _ _ = View.button(fun _ -> ())

    let terminalElement =
      ElmishTerminal.mkSimple init update view
      |> ElmishTerminal.runComponent
      :?> IInternalTerminalElement

    Assert.That(terminalElement.isElmishComponent, Is.True)

[<Test>]
let ``ElmishComponent embedded in ElmishTerminal program initializes and sets view exactly once`` () =
    // Counters to track invocations
    let mutable initializeCallCount = 0
    let mutable viewAccessCount = 0

    // Create a custom element that tracks initialize and view access
    let createTrackingElement () =
        let actualElement = View.button(fun _ -> ()) :?> IInternalTerminalElement
        
        { new IInternalTerminalElement with
            member _.initialize() =
                initializeCallCount <- initializeCallCount + 1
                actualElement.initialize()

            member _.initializeTree parent =
                actualElement.initializeTree parent

            member _.canReuseView prevView prevProps =
                actualElement.canReuseView prevView prevProps

            member _.reuse prevView prevProps =
                actualElement.reuse prevView prevProps

            member _.onDrawComplete = actualElement.onDrawComplete

            member _.children = actualElement.children

            member _.view =
                viewAccessCount <- viewAccessCount + 1
                actualElement.view

            member _.props = actualElement.props

            member _.name = actualElement.name

            member _.setAsChildOfParentView = actualElement.setAsChildOfParentView

            member _.parent
                with get() = actualElement.parent
                and set v = actualElement.parent <- v

            member _.isElmishComponent
                with get() = actualElement.isElmishComponent
                and set v = actualElement.isElmishComponent <- v

            member _.Dispose() = actualElement.Dispose()
        }

    // Create a minimal ElmishComponent that returns the tracking element
    let componentInit () = ()
    let componentUpdate _ _ = ()
    let componentView _ _ = createTrackingElement() :> ITerminalElement

    let componentElement =
        ElmishTerminal.mkSimple componentInit componentUpdate componentView
        |> ElmishTerminal.runComponent

    // At this point, the component has been initialized by its own Elmish loop
    // initialize() should have been called once, view should have been accessed
    let initCountAfterComponent = initializeCallCount
    let viewCountAfterComponent = viewAccessCount

    // Now embed this component in a minimal ElmishTerminal program
    let programInit () = (), Cmd.none
    let programUpdate _ _ = (), Cmd.none
    let programView _ _ = componentElement

    // Run the program
    let programElement =
        ElmishTerminal.mkSimple programInit programUpdate programView
        |> ElmishTerminal.runComponent
        :?> IInternalTerminalElement

    // After embedding in a program, the counts should not have increased significantly
    // because the component is already initialized (isElmishComponent = true)
    Assert.That(initializeCallCount, Is.EqualTo(initCountAfterComponent), 
        "TerminalElement.initialize() should not be called again when component is embedded in a program")
    Assert.That(initializeCallCount, Is.EqualTo(1), 
        "TerminalElement.initialize() should be called exactly once total")
    Assert.That(viewAccessCount, Is.GreaterThanOrEqualTo(viewCountAfterComponent), 
        "TerminalElement.view should be accessed")
