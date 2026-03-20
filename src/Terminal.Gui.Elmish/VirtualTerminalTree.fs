namespace Terminal.Gui.Elmish

type VirtualTerminalTree() =
  let mutable root: VttNode option = None

  let findNode (address: Address) (root: VttNode) =
    List.fold
      (fun (curNode: VttNode) curSeg ->
        match curSeg with
        | Root -> curNode
        | Child(idx, _) -> curNode.Children.[idx]
        | SubElement(idx, prop, _) -> curNode.SubElements.[(prop, idx)])
      root
      address

  /// Returns the parent node and the last segment of the address.
  [<TailCall>]
  let rec followToParent (address: Address) (root: VttNode) =
    match address with
    | [] -> failwith "Address cannot be empty."
    | [ Root ]
    | _ :: Root :: _ -> failwith "Root node does not have a parent."
    | _ :: Child(index, isComponent) :: next :: rest ->
      followToParent (Child(index, isComponent) :: next :: rest) root.Children[index]
    | _ :: SubElement(index, prop, isComponent) :: next :: rest ->
      followToParent (SubElement(index, prop, isComponent) :: next :: rest) root.SubElements.[(prop, index)]
    | [ _; Child(idx, isComponent) ] -> root, Child(idx, isComponent)
    | [ _; SubElement(idx, prop, isComponent) ] -> root, SubElement(idx, prop, isComponent)
    | [ x ] -> root, x

  member internal _.Root = root

  interface IVirtualTerminalTree with
    member _.AddView(view, address) =

      let newNode = VttNode(view, address)

      let addNode (address: Address) =
        match root, address with
        | None, [ Root ] -> root <- Some newNode
        | None, _ -> failwith "Cannot add a node to a non-root address when the tree is empty."
        | Some root, address ->
          let parentNode, lastSegment = followToParent address root

          match lastSegment with
          | Root -> failwith "Root node already exists."
          | Child(idx, isComponent) ->
            if parentNode.Children.Count > idx then
              failwithf $"A child node already exists at index %d{idx}."
            else
              parentNode.Children.Insert(idx, newNode)
          | SubElement(idx, prop, isComponent) ->
            if parentNode.SubElements.ContainsKey((prop, idx)) then
              failwithf $"A sub-element node already exists for property '%s{prop}' at index %A{idx}."
            else
              parentNode.SubElements.Add((prop, idx), newNode)

      addNode address
