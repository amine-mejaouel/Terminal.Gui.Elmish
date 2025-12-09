namespace Terminal.Gui.Elmish

module internal Differ =

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

  let update (prevTree: IInternalTerminalElement) (newTree: IInternalTerminalElement) =

    let removeAndDisposeView
      (removeSubViews: bool)
      (view: Terminal.Gui.ViewBase.View, name: string, parent: Terminal.Gui.ViewBase.View option) =

      parent
      |> Option.iter (fun p -> p.Remove view |> ignore)

      if not removeSubViews then
        // Prevent disposing children views when disposing the view
        prevTree.view.RemoveAll() |> ignore
      // NOTE: view.Dispose() will also dispose all subviews, if not removed first
      view.Dispose()

      #if DEBUG
      System.Diagnostics.Trace.WriteLine($"{name} removed and disposed!")
      #endif

    let workStack = System.Collections.Generic.Stack<_>()
    workStack.Push((prevTree, newTree))

    while workStack.Count > 0 do
      let prevTree, newTree = workStack.Pop()
      match prevTree, newTree with
      | rt, nt when rt.name <> nt.name ->

        let parent =
          prevTree.view |> Interop.getParent

        (prevTree.view, prevTree.name, parent)
        |> removeAndDisposeView true

        newTree.initializeTree parent

      | OnlyPropsChanged ->
        if newTree.canReuseView prevTree.view prevTree.props then
          match prevTree.detachView() with
          | Ok view ->
            newTree.reuse view prevTree.props
          | Error errMsg ->
            failwith errMsg
        else
          let parent =
            prevTree.view |> Interop.getParent

          (prevTree.view, prevTree.name, parent)
          |> removeAndDisposeView false

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
        ||> List.iter2 (fun rt nt -> workStack.Push(rt, nt))

      // TODO: should also consider the SubElements in the pattern matching
      | ChildsDifferent ->
        // TODO: should review the implementation of canReuse and its usefulness.
        let view =
          if newTree.canReuseView prevTree.view prevTree.props then
            match prevTree.detachView() with
            | Ok view ->
              newTree.reuse view prevTree.props
              view
            | Error errMsg ->
              failwith errMsg
          else
            // TODO: should test the case of noReuse manually and with unit test if needed
            let parent =
              prevTree.view |> Interop.getParent

            (prevTree.view, prevTree.name ,parent)
            |> removeAndDisposeView false

            newTree.initializeTree parent
            newTree.view

        let allTypes =
          seq {
            yield! prevTree.children
            yield! newTree.children
          }
          |> Seq.map (fun v -> v.name)
          |> Seq.distinct
          |> Seq.toList

        allTypes
        |> List.iter (fun et ->
          let rootElements =
            prevTree.children
            |> Seq.filter (fun e -> e.name = et)
            |> Seq.toList

          let newElements =
            newTree.children
            |> Seq.filter (fun e -> e.name = et)
            |> Seq.toList

          if (newElements.Length > rootElements.Length) then
            newElements
            |> List.iteri (fun idx ne ->
              if (idx < rootElements.Length) then
                workStack.Push(rootElements.[idx], ne)
              else
                // somehow when the window is empty and you add new elements to it, it complains about that the can focus is not set.
                // don't know
                // TODO: check if this is still needed
                if view.SubViews.Count = 0 then
                  view.CanFocus <- true

                let newElem =
                  ne.initializeTree (Some view)

                newElem
            )
          else
            rootElements
            |> List.iteri (fun idx re ->
              if (idx < newElements.Length) then
                workStack.Push(re, newElements.[idx])
              else
                let parent =
                  re.view |> Interop.getParent

                (re.view, re.name, parent)
                |> removeAndDisposeView true
            )
        )
      | _ ->
        printfn "other"
        ()
