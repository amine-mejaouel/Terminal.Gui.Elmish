# Future Enhancement: Serialization Example

This document shows how the `ITerminalElementData` interface could be used to implement serialization (a future enhancement).

## Concept

Since `ITerminalElementData` provides a pure data view of the element tree, it can be serialized to various formats without depending on Terminal.Gui runtime state.

## JSON Serialization Example (Conceptual)

```fsharp
open System.Text.Json
open System.Text.Json.Serialization

/// Simplified representation for serialization
type SerializableElement = {
    Name: string
    Children: SerializableElement list
}

/// Convert ITerminalElementData to serializable form
let rec toSerializable (data: ITerminalElementData) : SerializableElement =
    {
        Name = data.Name
        Children = data.Children |> List.map toSerializable
    }

/// Serialize to JSON
let toJson (data: ITerminalElementData) : string =
    let serializable = toSerializable data
    JsonSerializer.Serialize(serializable, JsonSerializerOptions(WriteIndented = true))

/// Example usage
let view = 
    View.runnable [
        View.window (fun p ->
            p.children [
                View.button (fun p -> p.text "Click Me")
                View.label (fun p -> p.text "Status")
            ]
        )
    ] :?> IInternalTerminalElement

view.initializeTree None
let dataTree = view.elementData :> ITerminalElementData

let json = toJson dataTree
printfn "Serialized UI:\n%s" json

// Output:
// {
//   "Name": "Runnable",
//   "Children": [
//     {
//       "Name": "Window",
//       "Children": [
//         {
//           "Name": "Button",
//           "Children": []
//         },
//         {
//           "Name": "Label",
//           "Children": []
//         }
//       ]
//     }
//   ]
// }
```

## Snapshot/Restore Example (Conceptual)

```fsharp
/// Save a snapshot of the current UI state
let saveSnapshot (data: ITerminalElementData) (filename: string) =
    let json = toJson data
    System.IO.File.WriteAllText(filename, json)

/// Compare two snapshots
let compareSnapshots (snapshot1: string) (snapshot2: string) =
    let data1 = JsonSerializer.Deserialize<SerializableElement>(snapshot1)
    let data2 = JsonSerializer.Deserialize<SerializableElement>(snapshot2)
    
    // Simple structural comparison
    data1 = data2

/// Example: Save UI state before making changes
let view = createComplexView()
view.initializeTree None

// Save initial state
let initialData = view.elementData :> ITerminalElementData
saveSnapshot initialData "ui_snapshot_v1.json"

// ... user makes changes ...

// Save new state
let newData = view.elementData :> ITerminalElementData
saveSnapshot newData "ui_snapshot_v2.json"

// Compare
let json1 = System.IO.File.ReadAllText("ui_snapshot_v1.json")
let json2 = System.IO.File.ReadAllText("ui_snapshot_v2.json")

if compareSnapshots json1 json2 then
    printfn "No structural changes"
else
    printfn "UI structure has changed"
```

## Tree Diff for Undo/Redo (Conceptual)

```fsharp
type TreeOperation =
    | AddChild of parentPath: int list * child: SerializableElement
    | RemoveChild of parentPath: int list * childIndex: int
    | ReplaceElement of path: int list * newElement: SerializableElement

/// History of changes
type UndoRedoHistory = {
    Operations: TreeOperation list
    CurrentIndex: int
}

/// Apply an operation to a tree
let applyOperation (tree: SerializableElement) (op: TreeOperation) : SerializableElement =
    // Implementation would navigate to the path and apply the change
    // This is a simplified placeholder
    tree

/// Undo last operation
let undo (history: UndoRedoHistory) (currentTree: SerializableElement) : SerializableElement =
    if history.CurrentIndex > 0 then
        let prevOp = history.Operations.[history.CurrentIndex - 1]
        // Apply inverse of the operation
        applyOperation currentTree prevOp
    else
        currentTree

/// Example: Track UI changes for undo/redo
let mutable history = {
    Operations = []
    CurrentIndex = 0
}

// Record adding a button
history <- {
    history with
        Operations = 
            history.Operations @ [
                AddChild(
                    [0; 0], // Path to parent
                    { Name = "Button"; Children = [] }
                )
            ]
        CurrentIndex = history.CurrentIndex + 1
}

// Undo would remove the button
let currentState = // ... get current tree
let previousState = undo history currentState
```

## Remote UI Transfer (Conceptual)

```fsharp
open System.Net.Http
open System.Net.Http.Json

/// Send UI structure to a server
let sendUIToServer (data: ITerminalElementData) (endpoint: string) =
    async {
        use client = new HttpClient()
        let serializable = toSerializable data
        let! response = client.PostAsJsonAsync(endpoint, serializable) |> Async.AwaitTask
        return response.IsSuccessStatusCode
    }

/// Receive UI structure from a server
let receiveUIFromServer (endpoint: string) =
    async {
        use client = new HttpClient()
        let! serializable = client.GetFromJsonAsync<SerializableElement>(endpoint) |> Async.AwaitTask
        return serializable
    }

/// Example: Collaborative UI editing
let view = createComplexView()
view.initializeTree None
let currentData = view.elementData :> ITerminalElementData

// Send to server
async {
    let! success = sendUIToServer currentData "https://api.example.com/ui"
    if success then
        printfn "UI synchronized with server"
}
|> Async.RunSynchronously
```

## Testing with Data Trees

```fsharp
open NUnit.Framework

/// Test helper: Create a data tree from description
let createTestTree name children : SerializableElement =
    {
        Name = name
        Children = children
    }

[<Test>]
let ``View structure matches expected layout`` () =
    // Arrange
    let view = createMyView()
    view.initializeTree None
    let actual = view.elementData :> ITerminalElementData |> toSerializable
    
    let expected = 
        createTestTree "Runnable" [
            createTestTree "Window" [
                createTestTree "Button" []
                createTestTree "Label" []
            ]
        ]
    
    // Assert
    Assert.AreEqual(expected, actual)

[<Test>]
let ``After update, view has correct structure`` () =
    // Arrange
    let view = createMyView()
    view.initializeTree None
    
    // Act: Apply Elmish update
    let newView = updateView view someMessage
    
    // Assert: Check data structure
    let dataAfter = newView.elementData :> ITerminalElementData |> toSerializable
    let expected = 
        createTestTree "Runnable" [
            createTestTree "Window" [
                createTestTree "Button" []
                createTestTree "Label" []
                createTestTree "Button" []  // New button added
            ]
        ]
    
    Assert.AreEqual(expected, dataAfter)
```

## Benefits of Serialization

1. **Persistence**: Save UI state to disk
2. **Network Transfer**: Send UI structure over network
3. **Version Control**: Track UI changes in git
4. **Testing**: Easy to assert on UI structure
5. **Debugging**: Inspect UI state without debugger
6. **Migration**: Convert between UI versions
7. **Collaboration**: Share UI designs between developers

## Implementation Notes

To implement these features in the future:

1. **Add Properties**: Extend `ITerminalElementData` to expose properties in a serializable format
2. **Factory Methods**: Create factory to rebuild runtime tree from serialized data
3. **Validation**: Add validation to ensure deserialized data is valid
4. **Versioning**: Add version field to handle schema evolution
5. **Compression**: Consider binary formats for large trees
6. **Diff Algorithm**: Implement efficient tree diffing
7. **Event Sourcing**: Consider event-sourced approach for undo/redo

## Current State

This is a **conceptual example** showing how `ITerminalElementData` enables these scenarios. The interface has been implemented, but serialization utilities are not yet built.

The foundation is in place - `ITerminalElementData` provides:
- ✅ Pure data representation (no runtime dependencies)
- ✅ Hierarchical structure (Children property)
- ✅ Type information (Name property)
- ⏳ Property access (future enhancement)
- ⏳ Factory for reconstruction (future enhancement)
- ⏳ Serialization helpers (future enhancement)
