# Design Enhancement: Data vs Shell Separation in Terminal.Gui.Elmish

## Overview

This document describes the design enhancement that separates element **data** (pure state/structure) from the runtime **shell** (behavior and manipulation) in Terminal.Gui.Elmish.

## Motivation

The goal is to achieve a clean separation of concerns:

1. **Data Layer (`ITerminalElementData`)**: Pure data structure representing element hierarchy and properties
   - Serializable and transferable
   - Immutable or controlled mutation
   - No dependencies on runtime Terminal.Gui views
   - Easy to test, diff, and persist

2. **Shell/Runtime Layer (`IInternalTerminalElement`, `IElementData`)**: Runtime wrapper providing behavior
   - Manages Terminal.Gui View instances
   - Implements event handling and lifecycle management
   - Provides element manipulation APIs
   - Coordinates with parent/child views

## Current Implementation

### Public Data Interface

```fsharp
type ITerminalElementData =
    /// Gets the children of this element as data nodes (not runtime wrappers)
    abstract Children: ITerminalElementData list with get
    
    /// Gets the name/type of this element
    abstract Name: string with get
```

**Key features:**
- `Children` property returns `ITerminalElementData list` - a pure data tree
- `Name` property exposes the element type (e.g., "Button", "FrameView", "Label")
- No exposure of internal Props (which are internal-only)
- Can be safely serialized, compared, and persisted

### Internal Runtime Interface

```fsharp
type internal IElementData =
    inherit ITerminalElementData
    abstract props: Props with get
    abstract view: View with get, set
    abstract eventRegistry: PropsEventRegistry with get
    /// Internal runtime children - kept as List of IInternalTerminalElement for runtime operations
    abstract children: List<IInternalTerminalElement> with get
    abstract ViewSet: IEvent<View>
```

**Key features:**
- Inherits from `ITerminalElementData` so it can be cast to the public interface
- `children` (lowercase) returns `List<IInternalTerminalElement>` - runtime wrappers with full capabilities
- `Children` (capitalized, from ITerminalElementData) returns `ITerminalElementData list` - data-only representation
- Maintains backward compatibility with existing internal code

## Usage Examples

### Accessing Data Tree (Public API)

```fsharp
// Create a view hierarchy
let view =
  View.runnable [
    View.frameView (fun p ->
      p.children [
        View.button (fun p -> p.text "Button 1")
        View.label (fun p -> p.text "Label 1")
      ]
    )
  ] :?> IInternalTerminalElement

view.initializeTree None

// Access as pure data (public interface)
let dataInterface = view.elementData :> ITerminalElementData
let dataChildren = dataInterface.Children  // ITerminalElementData list

// Traverse the data tree
for child in dataChildren do
    printfn "Child: %s" child.Name
    for grandchild in child.Children do
        printfn "  Grandchild: %s" grandchild.Name
```

### Accessing Runtime Tree (Internal API)

```fsharp
// Internal code can access runtime elements
let internalChildren = view.elementData.children  // List<IInternalTerminalElement>

// Work with runtime capabilities
for child in internalChildren do
    printfn "Child name: %s" child.name
    printfn "Child view: %A" child.view  // Access Terminal.Gui View
    child.initialize()  // Call runtime methods
```

## Proposed Future Enhancements

### 1. Immutable Data Records

Replace the current mutable `ElementData` with immutable F# records for the data layer:

```fsharp
type TerminalElementDataRecord = {
    Name: string
    Children: TerminalElementDataRecord list
    Properties: Map<string, obj>  // Simplified property bag
}
```

**Benefits:**
- Thread-safe
- Easy to compare and diff
- Natural fit for functional programming
- Can be used with Elmish messages directly

### 2. Serialization Support

Add JSON/Protobuf serialization for `ITerminalElementData`:

```fsharp
module Serialization =
    /// Serialize element data tree to JSON
    let toJson (data: ITerminalElementData) : string = ...
    
    /// Deserialize JSON to element data tree
    let fromJson (json: string) : ITerminalElementData = ...
```

**Use cases:**
- Persist UI state
- Network transfer
- Debugging and inspection
- State snapshots for undo/redo

### 3. Tree Conversion Utilities

Add helper functions for converting between data and runtime representations:

```fsharp
module TreeConversion =
    /// Convert runtime tree to data tree
    let toDataTree (runtime: IInternalTerminalElement) : ITerminalElementData =
        runtime.elementData :> ITerminalElementData
    
    /// Create runtime tree from data tree (requires factory)
    let rec fromDataTree (factory: IElementFactory) (data: ITerminalElementData) : IInternalTerminalElement =
        let element = factory.CreateFromData(data)
        for child in data.Children do
            let childElement = fromDataTree factory child
            element.elementData.children.Add(childElement)
        element
```

### 4. Tree Diff and Patch

Implement efficient tree diffing:

```fsharp
module TreeDiff =
    type TreeChange =
        | Added of path: int list * element: ITerminalElementData
        | Removed of path: int list
        | Modified of path: int list * oldElement: ITerminalElementData * newElement: ITerminalElementData
        | ChildrenReordered of path: int list
    
    /// Compute differences between two data trees
    let diff (oldTree: ITerminalElementData) (newTree: ITerminalElementData) : TreeChange list = ...
    
    /// Apply changes to a runtime tree
    let patch (runtime: IInternalTerminalElement) (changes: TreeChange list) : unit = ...
```

**Benefits:**
- Efficient UI updates
- Undo/redo support
- Collaborative editing

### 5. Factory Pattern for Element Creation

Define a factory interface for creating runtime elements from data:

```fsharp
type IElementFactory =
    abstract CreateFromData: data: ITerminalElementData -> IInternalTerminalElement
    abstract CreateButton: name: string -> IInternalTerminalElement
    abstract CreateLabel: name: string -> IInternalTerminalElement
    // ... other element types
```

**Benefits:**
- Centralized element creation logic
- Easy to mock for testing
- Dependency injection support

### 6. Query and Navigation APIs

Add helper functions for navigating and querying the data tree:

```fsharp
module TreeQuery =
    /// Find element by name
    let findByName (name: string) (root: ITerminalElementData) : ITerminalElementData option = ...
    
    /// Find all elements matching predicate
    let findAll (predicate: ITerminalElementData -> bool) (root: ITerminalElementData) : ITerminalElementData list = ...
    
    /// Get path from root to element
    let getPath (element: ITerminalElementData) (root: ITerminalElementData) : int list option = ...
    
    /// Fold over tree
    let fold (folder: 'State -> ITerminalElementData -> 'State) (state: 'State) (root: ITerminalElementData) : 'State = ...
```

### 7. Validation and Constraints

Add validation for data trees:

```fsharp
module TreeValidation =
    type ValidationError =
        | InvalidHierarchy of parent: string * child: string * reason: string
        | MissingRequiredProperty of element: string * property: string
        | InvalidPropertyValue of element: string * property: string * value: obj
    
    /// Validate element data tree
    let validate (root: ITerminalElementData) : ValidationError list = ...
```

### 8. Snapshot and Transactional Updates

Support atomic updates to element trees:

```fsharp
module TreeTransaction =
    type Transaction = private {
        Changes: TreeDiff.TreeChange list
    }
    
    /// Begin a transaction
    let beginTransaction (root: ITerminalElementData) : Transaction = ...
    
    /// Add a change to the transaction
    let addChange (change: TreeDiff.TreeChange) (transaction: Transaction) : Transaction = ...
    
    /// Commit transaction to runtime tree
    let commit (runtime: IInternalTerminalElement) (transaction: Transaction) : unit = ...
```

## Migration Guide for Existing Code

### For Library Users (External)

1. **No breaking changes** - Existing code continues to work
2. **New capability**: Cast `elementData` to `ITerminalElementData` to access pure data interface:
   ```fsharp
   let dataView = element.elementData :> ITerminalElementData
   let children = dataView.Children  // ITerminalElementData list
   ```

### For Library Maintainers (Internal)

1. **Backward compatible**: `IElementData.children` (lowercase) still returns `List<IInternalTerminalElement>`
2. **New property**: `IElementData.Children` (capitalized) returns `ITerminalElementData list`
3. **TreeDiff and other internal code**: No changes needed, continues to use `elementData.children`

## Testing

New tests verify the separation:

1. **Data tree access**: Verify `ITerminalElementData.Children` returns data-only list
2. **Runtime tree access**: Verify `IElementData.children` returns runtime wrappers
3. **Tree structure equivalence**: Verify data and runtime trees represent the same structure
4. **Element naming**: Verify `ITerminalElementData.Name` returns correct element type

## Benefits

1. **Separation of Concerns**: Clear boundary between data and behavior
2. **Testability**: Data trees can be tested without Terminal.Gui runtime
3. **Serialization**: Data trees can be easily serialized/deserialized
4. **Functional Patterns**: Data trees are immutable-friendly
5. **Performance**: Can optimize data representation separately from runtime
6. **Future-Proof**: Enables advanced features like undo/redo, persistence, collaborative editing

## Backward Compatibility

✅ **Fully backward compatible**:
- All existing code using `elementData.children` continues to work
- Internal APIs unchanged
- Only adds new public interface `ITerminalElementData`
- No breaking changes to existing behavior

## Conclusion

This enhancement establishes a foundation for a more maintainable, testable, and feature-rich architecture by cleanly separating element data from runtime behavior. The design is backward compatible and sets the stage for future improvements in serialization, persistence, tree manipulation, and collaborative editing.
