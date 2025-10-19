namespace Terminal.Gui.Elmish

module Differ =

    open Terminal.Gui.ViewBase
    open Terminal.Gui.Elmish.Elements
    open System.Linq

    //traverse Tree and get the Name of ever child
    let rec disposeTree (tree:View) =
        match tree.SubViews.ToArray() with
        | [||] ->
            tree.Dispose()
            System.Diagnostics.Trace.WriteLine ($"{tree.GetType().Name} disposed!")
            ()
        | _ ->
            tree.SubViews
            |> Seq.iter (fun e -> disposeTree e)
            ()

    let (|OnlyPropsChanged|_|) (ve1:ITerminalElement,ve2:ITerminalElement) =
        let cve1 = ve1.children |> Seq.map (fun e -> e.name) |> Seq.toList |> List.sort
        let cve2 = ve2.children |> Seq.map (fun e -> e.name) |> Seq.toList |> List.sort
        //let cve1 = getChildrenNames(ve1)
        //let cve2 = getChildrenNames(ve2)

        if cve1 = cve2 then Some () else None

    let (|ChildsDifferent|_|) (ve1:ITerminalElement,ve2:ITerminalElement) =
        //let cve1 = getChildrenNames(ve1)
        //let cve2 = getChildrenNames(ve2)

        let cve1 = ve1.children |> Seq.map (fun e -> e.name) |> Seq.toList |> List.sort
        let cve2 = ve2.children |> Seq.map (fun e -> e.name) |> Seq.toList |> List.sort
        if cve1 <> cve2 then Some () else None

    // TODO: Could be nice if this can be turned into a TailCall
    let rec update (rootTree:ITerminalElement) (newTree:ITerminalElement) =
        match rootTree, newTree with
        | rt, nt when rt.name <> nt.name ->
            let parent = rootTree.view |> Interop.getParent
            parent |> Option.iter (fun p -> p.Remove rootTree.view |> ignore)
            rootTree.view.Dispose()
        #if DEBUG
            System.Diagnostics.Trace.WriteLine ($"{rootTree.name} removed and disposed!")
        #endif
            newTree.initializeTree parent
        | OnlyPropsChanged ->
            if newTree.canUpdate rootTree.view rootTree.props then
                newTree.update rootTree.view rootTree.props
            else
                let parent = rootTree.view |> Interop.getParent
                parent |> Option.iter (fun p -> p.Remove rootTree.view |> ignore)
                disposeTree rootTree.view
                rootTree.view.RemoveAll() |> ignore
                rootTree.view.Dispose()
                #if DEBUG
                System.Diagnostics.Trace.WriteLine ($"{rootTree.name} removed and disposed!")
                #endif
                newTree.initializeTree parent

            let sortedRootChildren = rootTree.children |> Seq.toList |> List.sortBy (fun v -> v.name)
            let sortedNewChildren = newTree.children |> Seq.toList |> List.sortBy (fun v -> v.name)
            (sortedRootChildren,sortedNewChildren) ||> List.iter2 (fun rt nt -> update rt nt)
        | ChildsDifferent ->
            if newTree.canUpdate rootTree.view rootTree.props then
                newTree.update rootTree.view rootTree.props
            else
                let parent = rootTree.view |> Interop.getParent
                parent |> Option.iter (fun p -> p.Remove rootTree.view |> ignore)
                rootTree.view.Dispose()
            #if DEBUG
                System.Diagnostics.Trace.WriteLine ($"{rootTree.name} removed and disposed!")
            #endif
                newTree.initializeTree parent

            let sortedRootChildren = rootTree.children |> Seq.toList |> List.sortBy (fun v -> v.name)
            let sortedNewChildren = newTree.children |> Seq.toList |> List.sortBy (fun v -> v.name)
            let groupedRootType = sortedRootChildren |> List.map (fun v -> v.name) |> List.distinct
            let groupedNewType = sortedNewChildren |> List.map (fun v -> v.name) |> List.distinct
            let allTypes = groupedRootType @ groupedNewType |> List.distinct


            allTypes
            |> List.iter (fun et ->
                let rootElements = sortedRootChildren |> List.filter (fun e -> e.name = et)
                let newElements = sortedNewChildren |> List.filter (fun e -> e.name = et)
                if (newElements.Length > rootElements.Length) then
                    newElements
                    |> List.iteri (fun idx ne ->
                        if (idx+1 <= rootElements.Length) then
                            update rootElements.[idx] ne
                        else
                            // somehow when the window is empty and you add new elements to it, it complains about that the can focus is not set.
                            // don't know
                            if rootTree.view.SubViews.Count = 0 then
                                rootTree.view.CanFocus <- true
                            let newElem = ne.initializeTree (Some rootTree.view)
                            newElem
                        #if DEBUG
                            System.Diagnostics.Trace.WriteLine ($"child {ne.name} created ()!")
                        #endif

                    )
                else
                    rootElements
                    |> List.iteri (fun idx re ->
                        if (idx+1 <= newElements.Length) then
                            update re newElements.[idx]
                        else
                            // the rest we remove
                            re.view |> rootTree.view.Remove |> ignore
                            re.view.Dispose()
                        #if DEBUG
                            System.Diagnostics.Trace.WriteLine ($"child {re.name} removed and disposed!")
                        #endif
                            ()

                    )
            )
        | _ ->
            printfn "other"
            ()
