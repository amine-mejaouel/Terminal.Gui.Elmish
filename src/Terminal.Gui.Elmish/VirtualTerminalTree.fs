namespace Terminal.Gui.Elmish

type VirtualTerminalTree() =
  let mutable root: VttNode option = None

  let findNode (address: Address) (root: VttNode) =
    List.fold
      (fun (curNode: VttNode) curSeg ->
        match curSeg with
        | Root -> curNode
        | Child idx ->
          match curNode with
          | VttNode.ViewNode viewNode -> viewNode.Children.[idx]
          | VttNode.ElmishComponentNode _ ->
            failwith "Component nodes cannot have children in the virtual terminal tree."
        | SubElement(idx, prop) ->
          match curNode with
          | VttNode.ViewNode viewNode -> viewNode.SubElements.[(prop, idx)]
          | VttNode.ElmishComponentNode _ ->
            failwith "Component nodes cannot have sub-elements in the virtual terminal tree."
        | ElmishComponentRoot ->
          match curNode with
          | VttNode.ViewNode _ ->
            failwith "View nodes cannot have Elmish components as children in the virtual terminal tree."
          | VttNode.ElmishComponentNode componentNode -> VttNode.ViewNode componentNode.Root)
      root
      address

  member internal _.Root = root

  interface IVirtualTerminalTree with
    member _.AddView(view, address) =
      let lastSeg = Address.lastSegment address

      let newNode =
        match lastSeg with
        | Root
        | Child _
        | SubElement _ -> VttNode.ViewNode(ViewNode(view, address))
        | ElmishComponentRoot ->
          let viewNode = ViewNode(view, address)
          VttNode.ElmishComponentNode(ElmishComponentNode(viewNode, address))

      let rec addNode address newNode =
        match address with
        | [ Root ] -> root <- Some newNode
        | _ ->
          let lastSeg = Address.lastSegment address
          let parentAddress = Address.getParent address

          match lastSeg with
          | Root -> failwith "Root should only appear as [Root]"
          | Child idx ->
            let parentNode = findNode parentAddress root.Value

            match parentNode with
            | VttNode.ViewNode viewNode ->
              if idx <= viewNode.Children.Count then
                viewNode.Children.Insert(idx, newNode)
              else
                viewNode.Children.Add(newNode)
            | VttNode.ElmishComponentNode _ ->
              failwith "Component nodes cannot have children in the virtual terminal tree."
          | SubElement(idx, prop) ->
            let parentNode = findNode parentAddress root.Value

            match parentNode with
            | VttNode.ViewNode viewNode -> viewNode.SubElements.[(prop, idx)] <- newNode
            | VttNode.ElmishComponentNode _ ->
              failwith "Component nodes cannot have sub-elements in the virtual terminal tree."
          | ElmishComponentRoot -> addNode parentAddress newNode

      addNode address newNode
