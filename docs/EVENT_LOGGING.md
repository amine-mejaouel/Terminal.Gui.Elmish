# TerminalElement Event Logging

## Overview

The TerminalElement base class now provides a shared event logging system that tracks all property set and remove operations across all TerminalElement instances.

## Features

- **Shared Dictionary**: A static dictionary shared across all TerminalElement instances
- **Automatic Logging**: Every `setProps` and `removeProps` call is automatically logged
- **Simple API**: Easy access to the log and clearing functionality

## API

### TerminalElement.EventLog

A static property that returns the shared event log dictionary.

```fsharp
let log = TerminalElement.EventLog
for kvp in log do
    printfn "Property: %s, Event: %s" kvp.Key kvp.Value
```

### TerminalElement.clearEventLog()

Clears all entries from the event log.

```fsharp
TerminalElement.clearEventLog()
```

### TerminalElement.logEvent(propertyName, eventType)

Manually log an event (typically used internally).

```fsharp
TerminalElement.logEvent("myProperty", "set")
```

## Usage Examples

### Example 1: Tracking Button Properties

```fsharp
open Terminal.Gui.Elmish.Elements
open Terminal.Gui.Elmish
open Terminal.Gui.Views

// Clear the log
TerminalElement.clearEventLog()

// Create a button with properties
let props = Props()
props.add(PKey.view.text, "Click Me")
props.add(PKey.view.enabled, true)

let button = ButtonElement(props)
let buttonView = new Button()

// Set properties - this will log each property
button.setProps(buttonView, props)

// Check the log
for kvp in TerminalElement.EventLog do
    printfn "Property: %s, Event: %s" kvp.Key kvp.Value

// Output:
// Property: text, Event: set
// Property: enabled, Event: set
```

### Example 2: Tracking Property Removals

```fsharp
// Clear the log
TerminalElement.clearEventLog()

// Remove properties
button.removeProps(buttonView, props)

// Check the log
for kvp in TerminalElement.EventLog do
    printfn "Property: %s, Event: %s" kvp.Key kvp.Value

// Output:
// Property: text, Event: removed
// Property: enabled, Event: removed
```

### Example 3: Debugging Property Changes

```fsharp
// Clear log before operation
TerminalElement.clearEventLog()

// Perform some operations
performComplexUIUpdate()

// Check what properties were changed
printfn "Properties that were set or removed:"
for kvp in TerminalElement.EventLog do
    printfn "  - %s: %s" kvp.Key kvp.Value
```

## Implementation Details

### Base Class (TerminalElement)

The base class logs all common view properties in both `setProps` and `removeProps`:

```fsharp
props
|> Props.tryFind PKey.view.text
|> Option.iter (fun v -> 
  TerminalElement.logEvent("text", "set")
  element.Text <- v)
```

### Child Classes

All child classes (ButtonElement, LabelElement, etc.) also log their specific properties:

```fsharp
props
|> Props.tryFind PKey.button.isDefault
|> Option.iter (fun v -> 
  TerminalElement.logEvent("isDefault", "set")
  element.IsDefault <- v)
```

## Thread Safety

⚠️ **Note**: The current implementation uses a simple `Dictionary<string, string>` which is not thread-safe. If you're using TerminalElement instances concurrently from multiple threads, consider synchronization or using a thread-safe collection.

## Performance Considerations

- Each property set/remove operation adds one dictionary entry
- The dictionary grows unbounded until `clearEventLog()` is called
- For production use, consider periodically clearing the log or implementing a maximum size limit

## Testing

Example test using NUnit:

```fsharp
[<Test>]
let ``EventLog tracks setProps calls`` () =
  // Arrange
  TerminalElement.clearEventLog()
  
  let props = Props()
  props.add(PKey.view.text, "Test")
  let button = ButtonElement(props)
  let view = new Button()
  
  // Act
  button.setProps(view, props)
  
  // Assert
  let log = TerminalElement.EventLog
  Assert.That(log.ContainsKey("text"), Is.True)
  Assert.That(log.["text"], Is.EqualTo("set"))
```

## Troubleshooting

### Issue: EventLog shows unexpected entries

**Solution**: Make sure to call `TerminalElement.clearEventLog()` before your test or operation to start with a clean slate.

### Issue: EventLog is empty after setProps

**Solution**: Ensure the properties you're setting actually exist in the Props dictionary. Only properties that are found will be logged.

### Issue: Duplicate entries

**Solution**: The dictionary stores the latest event for each property name. If you set the same property twice, only the last event type is stored.

## Future Enhancements

Potential improvements could include:

1. **Timestamping**: Add timestamps to log entries
2. **Instance Tracking**: Track which TerminalElement instance triggered each event
3. **Event History**: Store multiple events per property instead of just the latest
4. **Thread Safety**: Use ConcurrentDictionary for thread-safe operations
5. **Log Rotation**: Automatically clear old entries when log reaches a certain size
