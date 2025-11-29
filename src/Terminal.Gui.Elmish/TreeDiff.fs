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
      let parent =
        prevTree.view |> Interop.getParent

      parent
      |> Option.iter (fun p -> p.Remove prevTree.view |> ignore)

      prevTree.view.Dispose()
#if DEBUG
      System.Diagnostics.Trace.WriteLine($"{prevTree.name} removed and disposed!")
#endif
      newTree.initializeTree parent
    | OnlyPropsChanged ->
      if newTree.canReuseView prevTree.view prevTree.props then
        newTree.reuseView prevTree.view prevTree.props
      else
        let parent =
          prevTree.view |> Interop.getParent

        parent
        |> Option.iter (fun p -> p.Remove prevTree.view |> ignore)

        disposeTree prevTree.view
        prevTree.view.RemoveAll() |> ignore
        prevTree.view.Dispose()
#if DEBUG
        System.Diagnostics.Trace.WriteLine($"{prevTree.name} removed and disposed!")
#endif
        newTree.initializeTree parent

      let sortedRootChildren =
        prevTree.children
        |> Seq.toList
        |> List.sortBy (fun v -> v.name)

      let sortedNewChildren =
        newTree.children
        |> Seq.toList
        |> List.sortBy (fun v -> v.name)

      (sortedRootChildren, sortedNewChildren)
      ||> List.iter2 (fun rt nt -> update rt nt)
    | ChildsDifferent ->
      if newTree.canReuseView prevTree.view prevTree.props then
        newTree.reuseView prevTree.view prevTree.props
      else
        let parent =
          prevTree.view |> Interop.getParent

        parent
        |> Option.iter (fun p -> p.Remove prevTree.view |> ignore)

        prevTree.view.Dispose()
#if DEBUG
        System.Diagnostics.Trace.WriteLine($"{prevTree.name} removed and disposed!")
#endif
        newTree.initializeTree parent

      let sortedRootChildren =
        prevTree.children
        |> Seq.toList
        |> List.sortBy (fun v -> v.name)

      let sortedNewChildren =
        newTree.children
        |> Seq.toList
        |> List.sortBy (fun v -> v.name)

      let groupedRootType =
        sortedRootChildren
        |> List.map (fun v -> v.name)
        |> List.distinct

      let groupedNewType =
        sortedNewChildren
        |> List.map (fun v -> v.name)
        |> List.distinct

      let allTypes =
        groupedRootType @ groupedNewType |> List.distinct


      allTypes
      |> List.iter (fun et ->
        let rootElements =
          sortedRootChildren
          |> List.filter (fun e -> e.name = et)

        let newElements =
          sortedNewChildren
          |> List.filter (fun e -> e.name = et)

        if (newElements.Length > rootElements.Length) then
          newElements
          |> List.iteri (fun idx ne ->
            if (idx + 1 <= rootElements.Length) then
              update rootElements.[idx] ne
            else
              // somehow when the window is empty and you add new elements to it, it complains about that the can focus is not set.
              // don't know
              if prevTree.view.SubViews.Count = 0 then
                prevTree.view.CanFocus <- true

              let newElem =
                ne.initializeTree (Some prevTree.view)

              newElem
#if DEBUG
              System.Diagnostics.Trace.WriteLine($"child {ne.name} created ()!")
#endif

          )
        else
          rootElements
          |> List.iteri (fun idx re ->
            if (idx + 1 <= newElements.Length) then
              update re newElements.[idx]
            else
              // the rest we remove
              re.view |> prevTree.view.Remove |> ignore
              re.view.Dispose()
#if DEBUG
              System.Diagnostics.Trace.WriteLine($"child {re.name} removed and disposed!")
#endif
              ()

          )
      )
    | _ ->
      printfn "other"
      ()
