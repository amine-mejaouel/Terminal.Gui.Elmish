namespace Terminal.Gui.Elmish

type MenuBarItemMacros internal (props: MenuBarItemProps) =
  member _.MenuItems(value: IMenuItemTerminalElement list) =
    let popoverMenu =
      props.props.getOrInit PKey.MenuBarItem.PopoverMenu_element (fun () -> new PopoverMenuTerminalElement(Props())) :?> PopoverMenuTerminalElement

    let menu =
      popoverMenu.Props.getOrInit PKey.PopoverMenu.Root_element (fun () -> new MenuTerminalElement(Props())) :?> MenuTerminalElement

    menu.Props.add (
      PKey.Menu.children,
      System.Collections.Generic.List<_>(
        value
        |> List.map (fun x -> x :?> IInternalTerminalElement)
      )
    )

type MenuBarMacros internal (props: MenuBarProps) =
  member _.MenuBarItem(set: MenuBarItemProps -> MenuBarItemMacros -> unit) =
    let menus =
      props.props.getOrInit PKey.MenuBar.children (fun () -> System.Collections.Generic.List<IInternalTerminalElement>())

    let props = MenuBarItemProps ()
    let macros = MenuBarItemMacros (props)
    set props macros

    menus.Add(new MenuBarItemTerminalElement(props.props))
