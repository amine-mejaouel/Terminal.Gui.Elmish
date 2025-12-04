// Simple console program to demonstrate event logging functionality
open System
open Terminal.Gui.Elmish.Elements
open Terminal.Gui.Elmish
open Terminal.Gui.Views

[<EntryPoint>]
let main argv =
    printfn "=== Terminal.Gui.Elmish Event Logging Demo ==="
    printfn ""
    
    // Clear the event log
    TerminalElement.clearEventLog()
    printfn "Event log cleared"
    printfn "Current log count: %d" TerminalElement.EventLog.Count
    printfn ""
    
    // Create a button with some properties
    printfn "Creating a button element with text and enabled properties..."
    let buttonProps = Props()
    buttonProps.add(PKey.view.text, "Click Me")
    buttonProps.add(PKey.view.enabled, true)
    buttonProps.add(PKey.view.visible, true)
    
    let button = ButtonElement(buttonProps)
    let buttonView = new Button()
    
    // Call setProps
    printfn "Calling setProps..."
    button.setProps(buttonView, buttonProps)
    
    // Check the event log
    printfn ""
    printfn "Event log after setProps:"
    printfn "Log count: %d" TerminalElement.EventLog.Count
    for kvp in TerminalElement.EventLog do
        printfn "  - Property: %s, Event: %s" kvp.Key kvp.Value
    
    printfn ""
    printfn "Clearing event log..."
    TerminalElement.clearEventLog()
    printfn "Log count: %d" TerminalElement.EventLog.Count
    
    // Test removeProps
    printfn ""
    printfn "Calling removeProps..."
    button.removeProps(buttonView, buttonProps)
    
    printfn ""
    printfn "Event log after removeProps:"
    printfn "Log count: %d" TerminalElement.EventLog.Count
    for kvp in TerminalElement.EventLog do
        printfn "  - Property: %s, Event: %s" kvp.Key kvp.Value
    
    printfn ""
    printfn "=== Demo completed successfully! ==="
    0
