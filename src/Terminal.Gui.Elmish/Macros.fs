namespace Terminal.Gui.Elmish

type MenuBarItemMacros internal (props: MenuBarItemProps) =
  member _.MenuItems(value: IMenuItemTerminalElement list) =
    let popoverMenu =
      props.props |> Props.getOrInit PKey.MenuBarItem.PopoverMenu_element (fun () -> new PopoverMenuTerminalElement(Props())) :?> PopoverMenuTerminalElement

    let menu =
      popoverMenu.Props |> Props.getOrInit PKey.PopoverMenu.Root_element (fun () -> new MenuTerminalElement(Props())) :?> MenuTerminalElement

    value
    |> List.map TerminalElement.from
    |> menu.Props.Children.AddRange

type MenuBarMacros internal (props: MenuBarProps) =
  member _.MenuBarItem(set: MenuBarItemProps -> MenuBarItemMacros -> unit) =
    let menus =
      props.props.Children

    let props = MenuBarItemProps ()
    let macros = MenuBarItemMacros props
    set props macros

    new MenuBarItemTerminalElement(props.props)
    |> TerminalElement.from
    |> menus.Add
