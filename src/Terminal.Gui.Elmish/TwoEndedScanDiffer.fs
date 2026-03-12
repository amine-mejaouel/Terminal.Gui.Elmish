module Terminal.Gui.Elmish.TwoEndedScanDiffer

open System.Collections

type IDomApi = interface end

let internal patchVNode (prevNode: TerminalElement) (newNode: TerminalElement) =
  match prevNode, newNode with
  | ViewTE prevView, ViewTE newView -> newView.Reuse prevView
  | ElmishComponentTE _, ElmishComponentTE _ -> () // TODO: should handle elmish component case.
  | ViewTE _, ElmishComponentTE _
  | ElmishComponentTE _, ViewTE _ -> failwith "Impossible case"

let internal initializeVNode (newNode: TerminalElement) (parent: TerminalElement) index =
  let origin =
    match parent with
    | ViewTE viewTe -> Origin.Child(viewTe, index)
    | ElmishComponentTE elmishComponentTe -> Origin.ElmishComponent elmishComponentTe

  match newNode with
  | ViewTE newView -> newView.InitializeTree origin
  | ElmishComponentTE _ -> () // TODO: should handle elmish component case.


let internal sameVNode (prevNode: TerminalElement) (newNode: TerminalElement) =
  match prevNode, newNode with
  | ViewTE prevView, ViewTE newView -> prevView.Name = newView.Name // TODO: This is incomplete check, can do better.
  | ElmishComponentTE prevComp, ElmishComponentTE newComp -> false // TODO: should handle elmish component case.
  | ViewTE _, ElmishComponentTE _
  | ElmishComponentTE _, ViewTE _ -> false

let rec internal updateViewTe (prevView: IViewTE) (newView: IViewTE) =
  let prevChildren = prevView.Children.ToArray()
  let newChildren = newView.Children.ToArray()
  let mutable prevStartIdx = 0
  let mutable prevEndIdx = prevChildren.Length - 1
  let mutable newStartIdx = 0
  let mutable newEndIdx = newChildren.Length - 1
  let mutable oldKeyToIdx: Generic.IDictionary<string, int> = dict []

  while (prevStartIdx <= prevEndIdx && newStartIdx <= newEndIdx) do
    if prevChildren[prevStartIdx] = Unchecked.defaultof<_> then
      prevStartIdx <- prevStartIdx + 1
    elif prevChildren[prevEndIdx] = Unchecked.defaultof<_> then
      prevEndIdx <- prevEndIdx - 1
    elif newChildren[prevStartIdx] = Unchecked.defaultof<_> then
      newStartIdx <- newStartIdx + 1
    elif newChildren[prevEndIdx] = Unchecked.defaultof<_> then
      newEndIdx <- newEndIdx - 1
    elif sameVNode prevChildren[prevStartIdx] newChildren[newStartIdx] then
      patchVNode prevChildren[prevStartIdx] newChildren[newStartIdx]
      update prevChildren[prevStartIdx] newChildren[newStartIdx]
      prevStartIdx <- prevStartIdx + 1
      newStartIdx <- newStartIdx + 1
    elif sameVNode prevChildren[prevEndIdx] newChildren[newEndIdx] then
      patchVNode prevChildren[prevEndIdx] newChildren[newEndIdx]
      update prevChildren[prevEndIdx] newChildren[newEndIdx]
      prevEndIdx <- prevEndIdx - 1
      newEndIdx <- newEndIdx - 1
    elif sameVNode prevChildren[prevStartIdx] newChildren[newEndIdx] then
      patchVNode prevChildren[prevStartIdx] newChildren[newEndIdx]
      update prevChildren[prevStartIdx] newChildren[newEndIdx]
      prevStartIdx <- prevStartIdx + 1
      newEndIdx <- newEndIdx - 1
    elif sameVNode prevChildren[prevEndIdx] newChildren[newStartIdx] then
      patchVNode prevChildren[prevEndIdx] newChildren[newStartIdx]
      update prevChildren[prevEndIdx] newChildren[newStartIdx]
      prevEndIdx <- prevEndIdx - 1
      newStartIdx <- newStartIdx + 1
    else
      if oldKeyToIdx.Count = 0 then
        oldKeyToIdx <-
          Seq.init (prevEndIdx - prevStartIdx + 1) (fun i -> prevChildren[prevStartIdx + i])
          |> Seq.mapi (fun i child -> (child, prevStartIdx + i))
          |> Seq.filter (fun (child, idx) -> child <> Unchecked.defaultof<_>)
          |> Seq.map (fun (child, idx) -> (child.Name, idx))
          |> dict

      let newStartKeyIdxInOLd = oldKeyToIdx.TryGetValue newChildren[newStartIdx].Name

      match newStartKeyIdxInOLd with
      | false, _ ->
        // TODO: will not work because underneath the new child will be added at the end of the children list, need to insert at correct index.
        initializeVNode newChildren[newStartIdx] (ViewTE newView) newStartIdx
        newStartIdx <- newStartIdx + 1
      | _ -> ()

      let newEndKeyIdxInOld = oldKeyToIdx.TryGetValue newChildren[newEndIdx].Name

      match newEndKeyIdxInOld with
      | false, _ ->
        initializeVNode newChildren[newEndIdx] (ViewTE newView) newEndIdx
        newEndIdx <- newEndIdx - 1
      | _ -> ()

      match newStartKeyIdxInOLd, newEndKeyIdxInOld with
      | (true, startIdxInOld), _ ->
        let nodeToMove = prevChildren[startIdxInOld]
        patchVNode nodeToMove newChildren[newStartIdx]
        update nodeToMove newChildren[newStartIdx]
        prevChildren[startIdxInOld] <- Unchecked.defaultof<_>
      | _, (true, endIdxInOld) ->
        let nodeToMove = prevChildren[endIdxInOld]
        patchVNode nodeToMove newChildren[newEndIdx]
        update nodeToMove newChildren[newEndIdx]
        prevChildren[endIdxInOld] <- Unchecked.defaultof<_>
      | _ -> failwith "Impossible case"

and internal update (prevTree: TerminalElement) (newTree: TerminalElement) =
  match prevTree, newTree with
  | ViewTE prevView, ViewTE newView -> updateViewTe prevView newView
  | ElmishComponentTE _, ElmishComponentTE _ -> () // TODO: should handle
  | ViewTE _, ElmishComponentTE _
  | ElmishComponentTE _, ViewTE _ -> failwith "Invalid case"


let internal init (domApi: IDomApi) : TerminalElement -> TerminalElement -> unit = update
