namespace Terminal.Gui.Elmish

module internal Differ =

  let sortedChildNames (ve: IInternalTerminalElement) =
    ve.Children
    |> Seq.map (fun e -> e.name)
    |> Seq.toList
    |> List.sort

  let (|OnlyPropsChanged|_|) (ve1: IInternalTerminalElement, ve2: IInternalTerminalElement) =

    let cve1 = sortedChildNames ve1
    let cve2 = sortedChildNames ve2

    if cve1 = cve2 then Some() else None

  let (|DifferentChildren|_|) (ve1: IInternalTerminalElement, ve2: IInternalTerminalElement) =

    let cve1 = sortedChildNames ve1
    let cve2 = sortedChildNames ve2

    if cve1 <> cve2 then Some() else None

  let update (prevTree: IInternalTerminalElement) (newTree: IInternalTerminalElement) =

    let workStack = System.Collections.Generic.Stack<_>()
    workStack.Push((prevTree, newTree))

    while workStack.Count > 0 do
      let prevTree, newTree = workStack.Pop()
      match prevTree, newTree with
      | rt, nt when rt.name <> nt.name ->

        let parent =
          prevTree.View |> Interop.getParent

        prevTree.Dispose()

        newTree.initializeTree parent

      | OnlyPropsChanged ->

        newTree.reuse prevTree

        let sortedRootChildren =
          prevTree.Children
          |> Seq.toList
          |> List.sortBy (fun v -> v.name)

        let sortedNewChildren =
          newTree.Children
          |> Seq.toList
          |> List.sortBy (fun v -> v.name)

        (sortedRootChildren, sortedNewChildren)
        ||> List.iter2 (fun rt nt -> workStack.Push(rt, nt))

        prevTree.Dispose()

      // TODO: should also consider the SubElements in the pattern matching
      | DifferentChildren ->

        newTree.reuse prevTree

        let allTypes =
          seq {
            yield! prevTree.Children
            yield! newTree.Children
          }
          |> Seq.map (fun v -> v.name)
          |> Seq.distinct
          |> Seq.toList

        let prevParent =
          prevTree.View

        allTypes
        |> List.iter (fun et ->
          let rootElements =
            prevTree.Children
            |> Seq.filter (fun e -> e.name = et)
            |> Seq.toList

          let newElements =
            newTree.Children
            |> Seq.filter (fun e -> e.name = et)
            |> Seq.toList

          if (newElements.Length > rootElements.Length) then
            newElements
            |> List.iteri (fun idx ne ->
              if (idx < rootElements.Length) then
                workStack.Push(rootElements[idx], ne)
              else
                // somehow when the window is empty and you add new elements to it, it complains about that the can focus is not set.
                // don't know
                // TODO: check if this is still needed
                if prevTree.View.SubViews.Count = 0 then
                  prevTree.View.CanFocus <- true

                let newElem =
                  ne.initializeTree (Some prevParent)

                newElem
            )
          else
            rootElements
            |> List.iteri (fun idx re ->
              if (idx < newElements.Length) then
                workStack.Push(re, newElements[idx])
              else
                re.Dispose()
            )
        )

        prevTree.Dispose()
      | _ ->
        printfn "other"
        ()
