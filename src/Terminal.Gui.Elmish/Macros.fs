namespace Terminal.Gui.Elmish

open Terminal.Gui.Elmish.Elements

type menuBarItemMacros internal (props: menuBarItemProps) =
  member _.menuItems(value: ITerminalElement list) =
    let popoverMenu =
      props.props.getOrInit PKey.menuBarItem.popoverMenu_element (fun () -> new PopoverMenuElement(Props())) :?> PopoverMenuElement

    let menu =
      popoverMenu.elementData.Props.getOrInit PKey.popoverMenu.root_element (fun () -> new MenuElement(Props())) :?> MenuElement

    menu.elementData.Props.add (
      PKey.menu.children,
      System.Collections.Generic.List<_>(
        value
        |> List.map (fun x -> x :?> IInternalTerminalElement)
      )
    )

type menuBarMacros internal (props: menuBarProps) =
  member _.menuBarItem(set: menuBarItemProps -> menuBarItemMacros -> unit) =
    let menus =
      props.props.getOrInit PKey.menuBar.children (fun () -> System.Collections.Generic.List<IInternalTerminalElement>())

    let props = menuBarItemProps ()
    let macros = menuBarItemMacros (props)
    set props macros

    menus.Add(new MenuBarItemElement(props.props))
