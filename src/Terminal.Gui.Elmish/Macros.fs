namespace Terminal.Gui.Elmish

type MenuBarItemMacros internal (props: MenuBarItemProps) =
  member _.MenuItems(value: IMenuItemView list) =
    let popoverMenu =
      props.props
      |> Props.getOrInit PKey.MenuBarItem.PopoverMenu_viewSpec (fun () -> new PopoverMenu(PopoverMenuProps()))
      :?> ISimpleViewSpec

    let menu =
      popoverMenu.Props
      |> Props.getOrInit PKey.PopoverMenu.Root_viewSpec (fun () -> new Menu(MenuProps()))
      :?> ISimpleViewSpec

    value |> List.map ViewSpec.from |> menu.Props.Children.AddRange

type MenuBarMacros internal (props: MenuBarProps) =
  member _.MenuBarItem(set: MenuBarItemProps -> MenuBarItemMacros -> unit) =
    let menus = props.props.Children

    let props = MenuBarItemProps()
    let macros = MenuBarItemMacros props
    set props macros

    new MenuBarItem(props) |> ViewSpec.from |> menus.Add
