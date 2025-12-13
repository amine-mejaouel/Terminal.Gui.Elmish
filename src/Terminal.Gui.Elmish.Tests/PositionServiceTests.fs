module Terminal.Gui.Elmish.Tests.PositionServiceTests

open System
open System.Collections.Generic
open System.Reflection
open NUnit.Framework
open Terminal.Gui
open Terminal.Gui.Views
open Terminal.Gui.Elmish
open Terminal.Gui.Elmish.Elements

// Type aliases for complex generic types
type private RemoveHandlerRepo = Dictionary<IElementData * IElementData, List<unit -> unit>>
type private IndexedRemoveHandlerRepo = Dictionary<IElementData, HashSet<IElementData * IElementData>>

/// Helper to get private/internal properties using reflection
let private getPrivateProperty<'T> (target: obj) (propertyName: string) : 'T =
    let targetType = target.GetType()
    let prop = targetType.GetProperty(propertyName, BindingFlags.NonPublic ||| BindingFlags.Instance ||| BindingFlags.Public)
    Assert.That(prop, Is.Not.Null, $"Property '{propertyName}' not found on {targetType.FullName}")
    prop.GetValue(target) :?> 'T

/// Helper to create a minimal IInternalTerminalElement for testing
let private createTestTerminalElement (elementData: ElementData) (view: ViewBase.View) =
    let mutable parentValue = None
    {
        new IInternalTerminalElement with
            member _.elementData = elementData :> IElementData
            member _.view = view
            member _.name = "test"
            member _.isElmishComponent = false
            member _.reuse(_) = ()
            member _.detachElementData() = elementData :> IElementData
            member _.Dispose() = ()
            member _.initialize() = ()
            member _.initializeTree(_) = ()
            member _.setAsChildOfParentView = false
            member _.parent 
                with get() = parentValue
                and set(value) = parentValue <- value
    }

[<SetUp>]
let Setup() =
    // Clear the PositionService singleton state before each test
    let positionService = PositionService.Current
    let removeHandlerRepo = getPrivateProperty<RemoveHandlerRepo> positionService "RemoveHandlerRepository"
    let indexedRemoveHandler = getPrivateProperty<IndexedRemoveHandlerRepo> positionService "IndexedRemoveHandler"
    
    removeHandlerRepo.Clear()
    indexedRemoveHandler.Clear()

[<Test>]
let ``SignalReuse removes handlers from both internal dictionaries`` () =
    // Arrange
    let positionService = PositionService.Current
    
    // Create test element data
    let props1 = Props()
    let elementData1 = ElementData.create props1
    
    let props2 = Props()
    let elementData2 = ElementData.create props2
    
    // We need to create views for the element data
    let view1 = Label()
    let view2 = Label()
    elementData1.View <- view1
    elementData2.View <- view2
    
    // Use reflection to manually populate the dictionaries to simulate ApplyPos behavior
    let removeHandlerRepo = getPrivateProperty<RemoveHandlerRepo> positionService "RemoveHandlerRepository"
    let indexedRemoveHandler = getPrivateProperty<IndexedRemoveHandlerRepo> positionService "IndexedRemoveHandler"
    
    // Add a dummy handler to RemoveHandlerRepository
    let key = (elementData1 :> IElementData, elementData2 :> IElementData)
    let handlers = List<unit -> unit>()
    handlers.Add(fun () -> ())
    removeHandlerRepo.[key] <- handlers
    
    // Add entries to IndexedRemoveHandler for both element data objects
    let set1 = HashSet<IElementData * IElementData>()
    set1.Add(key) |> ignore
    indexedRemoveHandler.[elementData1 :> IElementData] <- set1
    
    let set2 = HashSet<IElementData * IElementData>()
    set2.Add(key) |> ignore
    indexedRemoveHandler.[elementData2 :> IElementData] <- set2
    
    // Verify the dictionaries are populated
    Assert.That(removeHandlerRepo.Count, Is.EqualTo(1), "RemoveHandlerRepository should have 1 entry before SignalReuse")
    Assert.That(indexedRemoveHandler.Count, Is.EqualTo(2), "IndexedRemoveHandler should have 2 entries before SignalReuse")
    
    // Act: Call SignalReuse on elementData1
    positionService.SignalReuse(elementData1 :> IElementData)
    
    // Assert: Verify the handlers associated with elementData1 are removed
    Assert.That(removeHandlerRepo.Count, Is.EqualTo(0), "RemoveHandlerRepository should be empty after SignalReuse")
    Assert.That(indexedRemoveHandler.ContainsKey(elementData1 :> IElementData), Is.False, "IndexedRemoveHandler should not contain elementData1")
    
    // Note: elementData2 should still be in IndexedRemoveHandler if it has other references,
    // but since the key was removed from RemoveHandlerRepository, it's effectively cleaned up.
    // The current implementation removes elementData1's entry and the shared key from RemoveHandlerRepository.

[<Test>]
let ``SignalReuse with ApplyPos - verify handler cleanup after relative positioning`` () =
    // Arrange
    let positionService = PositionService.Current
    
    // Create a simple view structure for testing
    let props1 = Props()
    let elementData1 = ElementData.create props1
    let view1 = Label()
    elementData1.View <- view1
    
    let props2 = Props()  
    let elementData2 = ElementData.create props2
    let view2 = Label()
    elementData2.View <- view2
    
    // Create a terminal element to use in TPos (minimal implementation for testing)
    let terminalElement = createTestTerminalElement elementData2 view2
    
    // Apply a relative position (this should register handlers)
    let targetPos = TPos.X terminalElement
    positionService.ApplyPos(elementData1 :> IElementData, targetPos, fun v p -> v.X <- p)
    
    // Verify handlers were registered
    let removeHandlerRepo = getPrivateProperty<RemoveHandlerRepo> positionService "RemoveHandlerRepository"
    let indexedRemoveHandler = getPrivateProperty<IndexedRemoveHandlerRepo> positionService "IndexedRemoveHandler"
    
    let initialRemoveHandlerCount = removeHandlerRepo.Count
    let initialIndexedHandlerCount = indexedRemoveHandler.Count
    
    Assert.That(initialRemoveHandlerCount, Is.GreaterThan(0), "RemoveHandlerRepository should have entries after ApplyPos")
    Assert.That(initialIndexedHandlerCount, Is.GreaterThan(0), "IndexedRemoveHandler should have entries after ApplyPos")
    
    // Act: Signal reuse on elementData1
    positionService.SignalReuse(elementData1 :> IElementData)
    
    // Assert: Verify handlers are cleaned up
    let finalRemoveHandlerCount = removeHandlerRepo.Count
    let finalIndexedHandlerCount = indexedRemoveHandler.Count
    
    Assert.That(finalRemoveHandlerCount, Is.LessThan(initialRemoveHandlerCount), "RemoveHandlerRepository should have fewer entries after SignalReuse")
    Assert.That(indexedRemoveHandler.ContainsKey(elementData1 :> IElementData), Is.False, "IndexedRemoveHandler should not contain elementData1 after SignalReuse")

[<Test>]
let ``Multiple SignalReuse calls handle empty dictionaries gracefully`` () =
    // Arrange
    let positionService = PositionService.Current
    let props = Props()
    let elementData = ElementData.create props
    let view = Label()
    elementData.View <- view
    
    // Act & Assert: Multiple calls should not throw
    Assert.DoesNotThrow(fun () -> 
        positionService.SignalReuse(elementData :> IElementData)
        positionService.SignalReuse(elementData :> IElementData)
        positionService.SignalReuse(elementData :> IElementData)
    )
    
    // Verify dictionaries remain empty
    let removeHandlerRepo = getPrivateProperty<RemoveHandlerRepo> positionService "RemoveHandlerRepository"
    let indexedRemoveHandler = getPrivateProperty<IndexedRemoveHandlerRepo> positionService "IndexedRemoveHandler"
    
    Assert.That(removeHandlerRepo.Count, Is.EqualTo(0))
    Assert.That(indexedRemoveHandler.Count, Is.EqualTo(0))
