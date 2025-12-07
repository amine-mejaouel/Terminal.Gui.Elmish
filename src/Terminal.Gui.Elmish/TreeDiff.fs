namespace Terminal.Gui.Elmish

module internal Differ =

  open Terminal.Gui.ViewBase
  open Terminal.Gui.Elmish.Elements
  open System.Linq

  //traverse Tree and get the Name of ever child
  let rec disposeTree (tree: View) =
    match tree.SubViews.ToArray() with
    | [||] ->
      tree.Dispose()
      System.Diagnostics.Trace.WriteLine($"{tree.GetType().Name} disposed!")
      ()
    | _ ->
      tree.SubViews |> Seq.iter (fun e -> disposeTree e)
      ()

  /// Helper function to remove view from parent and dispose it
  let private removeAndDisposeView (prevTree: IInternalTerminalElement) =
    let parent = prevTree.view |> Interop.getParent
    parent |> Option.iter (fun p -> p.Remove prevTree.view |> ignore)
    prevTree.view.Dispose()
#if DEBUG
    System.Diagnostics.Trace.WriteLine($"{prevTree.name} removed and disposed!")
#endif

  /// Get sorted children list (sort once, reuse multiple times)
  let private getSortedChildren (children: System.Collections.Generic.List<IInternalTerminalElement>) =
    children |> Seq.sortBy (fun v -> v.name) |> Seq.toList

  let (|OnlyPropsChanged|_|) (ve1: IInternalTerminalElement, ve2: IInternalTerminalElement) =
    let cve1 =
      ve1.children
      |> Seq.map (fun e -> e.name)
      |> Seq.toList
      |> List.sort

    let cve2 =
      ve2.children
      |> Seq.map (fun e -> e.name)
      |> Seq.toList
      |> List.sort
    //let cve1 = getChildrenNames(ve1)
    //let cve2 = getChildrenNames(ve2)

    if cve1 = cve2 then Some() else None

  let (|ChildsDifferent|_|) (ve1: IInternalTerminalElement, ve2: IInternalTerminalElement) =
    //let cve1 = getChildrenNames(ve1)
    //let cve2 = getChildrenNames(ve2)

    let cve1 =
      ve1.children
      |> Seq.map (fun e -> e.name)
      |> Seq.toList
      |> List.sort

    let cve2 =
      ve2.children
      |> Seq.map (fun e -> e.name)
      |> Seq.toList
      |> List.sort

    if cve1 <> cve2 then Some() else None

  // TODO: Could be nice if this can be turned into a TailCall
  let rec update (prevTree: IInternalTerminalElement) (newTree: IInternalTerminalElement) =
    match prevTree, newTree with
    | rt, nt when rt.name <> nt.name ->
      let parent = prevTree.view |> Interop.getParent
      removeAndDisposeView prevTree
      newTree.initializeTree parent
      
    | OnlyPropsChanged ->
      if newTree.canReuseView prevTree.view prevTree.props then
        newTree.reuse prevTree.view prevTree.props
      else
        let parent = prevTree.view |> Interop.getParent
        removeAndDisposeView prevTree
        newTree.initializeTree parent

      let sortedRootChildren = getSortedChildren prevTree.children
      let sortedNewChildren = getSortedChildren newTree.children

      (sortedRootChildren, sortedNewChildren)
      ||> List.iter2 (fun rt nt -> update rt nt)
      
    | ChildsDifferent ->
      if newTree.canReuseView prevTree.view prevTree.props then
        newTree.reuse prevTree.view prevTree.props
      else
        let parent = prevTree.view |> Interop.getParent
        removeAndDisposeView prevTree
        newTree.initializeTree parent

      let sortedRootChildren = getSortedChildren prevTree.children
      let sortedNewChildren = getSortedChildren newTree.children

      // Group elements by type for efficient lookup
      let rootElementsByType = 
        sortedRootChildren 
        |> List.groupBy (fun e -> e.name)
        |> Map.ofList

      let newElementsByType = 
        sortedNewChildren 
        |> List.groupBy (fun e -> e.name)
        |> Map.ofList

      // Get all unique element types from both trees
      let allTypes =
        Set.union
          (rootElementsByType.Keys |> Set.ofSeq)
          (newElementsByType.Keys |> Set.ofSeq)

      allTypes
      |> Set.iter (fun elementType ->
        let rootElements = 
          rootElementsByType 
          |> Map.tryFind elementType 
          |> Option.defaultValue []

        let newElements = 
          newElementsByType 
          |> Map.tryFind elementType 
          |> Option.defaultValue []

        if newElements.Length > rootElements.Length then
          newElements
          |> List.iteri (fun idx ne ->
            if idx < rootElements.Length then
              update rootElements.[idx] ne
            else
              // Ensure parent can focus when adding first child
              if prevTree.view.SubViews.Count = 0 then
                prevTree.view.CanFocus <- true

              ne.initializeTree (Some prevTree.view) |> ignore
#if DEBUG
              System.Diagnostics.Trace.WriteLine($"child {ne.name} created!")
#endif
          )
        else
          rootElements
          |> List.iteri (fun idx re ->
            if idx < newElements.Length then
              update re newElements.[idx]
            else
              // Remove excess elements
              re.view |> prevTree.view.Remove |> ignore
              re.view.Dispose()
#if DEBUG
              System.Diagnostics.Trace.WriteLine($"child {re.name} removed and disposed!")
#endif
          )
      )
      
    | _ ->
      // This case should not occur if active patterns cover all scenarios
      failwithf "Unexpected case in update: prevTree.name=%s, newTree.name=%s" prevTree.name newTree.name
