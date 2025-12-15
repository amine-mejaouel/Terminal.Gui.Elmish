# Usage Example: ITerminalElementData Interface

This document demonstrates how to use the new `ITerminalElementData` interface to access element data in a pure, serializable form.

## Basic Usage

```fsharp
open Terminal.Gui.Elmish

// Create a view hierarchy
let createView () =
    View.runnable [
        View.window (fun p ->
            p.children [
                View.frameView (fun p ->
                    p.title "My Frame"
                    p.children [
                        View.button (fun p -> p.text "Click Me")
                        View.label (fun p -> p.text "Status: Ready")
                    ]
                )
                View.button (fun p -> p.text "Exit")
            ]
        )
    ] :?> IInternalTerminalElement

// Initialize the view
let view = createView()
view.initializeTree None

// Access the data interface (public API)
let dataInterface = view.elementData :> ITerminalElementData

printfn "Root element: %s" dataInterface.Name  // "Runnable"
printfn "Number of children: %d" dataInterface.Children.Length  // 1 (Window)

// Navigate the data tree
let windowData = dataInterface.Children.[0]
printfn "First child: %s" windowData.Name  // "Window"

// Access window's children
for child in windowData.Children do
    printfn "  Window child: %s" child.Name
    // Outputs:
    //   Window child: FrameView
    //   Window child: Button

// Access nested elements
let frameData = windowData.Children.[0]
printfn "Frame has %d children" frameData.Children.Length  // 2

for grandchild in frameData.Children do
    printfn "    Frame child: %s" grandchild.Name
    // Outputs:
    //    Frame child: Button
    //    Frame child: Label
```

## Tree Traversal Example

```fsharp
/// Recursively print the element tree
let rec printTree (indent: int) (element: ITerminalElementData) =
    let prefix = String.replicate indent "  "
    printfn "%s- %s" prefix element.Name
    for child in element.Children do
        printTree (indent + 1) child

// Use it
let view = createView()
view.initializeTree None
let dataTree = view.elementData :> ITerminalElementData

printfn "Element Tree:"
printTree 0 dataTree
// Output:
// Element Tree:
// - Runnable
//   - Window
//     - FrameView
//       - Button
//       - Label
//     - Button
```

## Collecting Element Statistics

```fsharp
/// Count elements by type
let countElementsByType (root: ITerminalElementData) : Map<string, int> =
    let rec count (element: ITerminalElementData) (acc: Map<string, int>) =
        let newAcc = 
            acc 
            |> Map.tryFind element.Name
            |> Option.map (fun count -> Map.add element.Name (count + 1) acc)
            |> Option.defaultWith (fun () -> Map.add element.Name 1 acc)
        
        element.Children
        |> List.fold (fun acc child -> count child acc) newAcc
    
    count root Map.empty

// Use it
let view = createView()
view.initializeTree None
let dataTree = view.elementData :> ITerminalElementData

let stats = countElementsByType dataTree
printfn "Element statistics:"
for kvp in stats do
    printfn "  %s: %d" kvp.Key kvp.Value
// Output:
// Element statistics:
//   Runnable: 1
//   Window: 1
//   FrameView: 1
//   Button: 2
//   Label: 1
```

## Finding Elements

```fsharp
/// Find all elements with a specific name
let findByName (name: string) (root: ITerminalElementData) : ITerminalElementData list =
    let rec find (element: ITerminalElementData) (acc: ITerminalElementData list) =
        let newAcc = 
            if element.Name = name then 
                element :: acc 
            else 
                acc
        
        element.Children
        |> List.fold (fun acc child -> find child acc) newAcc
    
    find root []

// Use it
let view = createView()
view.initializeTree None
let dataTree = view.elementData :> ITerminalElementData

let allButtons = findByName "Button" dataTree
printfn "Found %d buttons in the tree" allButtons.Length  // 2
```

## Comparing Trees (Future Enhancement)

This example shows how the data interface could be used for tree comparison (not yet implemented):

```fsharp
/// Check if two data trees have the same structure (ignoring properties)
let rec structureEquals (tree1: ITerminalElementData) (tree2: ITerminalElementData) : bool =
    if tree1.Name <> tree2.Name then
        false
    elif tree1.Children.Length <> tree2.Children.Length then
        false
    else
        List.forall2 structureEquals tree1.Children tree2.Children

// Use it for testing or validation
let view1 = createView()
let view2 = createView()
view1.initializeTree None
view2.initializeTree None

let data1 = view1.elementData :> ITerminalElementData
let data2 = view2.elementData :> ITerminalElementData

if structureEquals data1 data2 then
    printfn "Both views have the same structure"
else
    printfn "Views have different structures"
```

## Key Points

1. **Pure Data**: `ITerminalElementData` provides a pure data view of the element tree
2. **No Side Effects**: Accessing `Children` and `Name` has no side effects
3. **Serializable**: The data tree can be traversed and serialized without triggering Terminal.Gui operations
4. **Testing**: Easy to test tree structures without initializing full Terminal.Gui views
5. **Type Safety**: `Children` returns a strongly-typed `ITerminalElementData list`

## Difference from Runtime API

### Data Interface (Public - ITerminalElementData)
- `Children`: Returns `ITerminalElementData list` (data-only nodes)
- `Name`: Returns element type as string
- No access to View objects or runtime state
- Safe for serialization and testing

### Runtime Interface (Internal - IElementData)
- `children`: Returns `List<IInternalTerminalElement>` (runtime wrappers)
- Full access to Terminal.Gui View objects
- Can call methods like `initialize()`, `dispose()`
- Used internally by TreeDiff and other runtime components

## Backward Compatibility

All existing code continues to work without changes:

```fsharp
// Existing internal code (no changes needed)
let internalChildren = element.elementData.children  // List<IInternalTerminalElement>

// New public API (opt-in)
let dataChildren = (element.elementData :> ITerminalElementData).Children  // ITerminalElementData list
```
