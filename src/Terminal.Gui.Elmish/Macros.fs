namespace Terminal.Gui.Elmish

open Terminal.Gui.Elmish.Elements

type menuBarItemv2Macros(props: menuBarItemv2Props) =
    member _.menuItems(value: ITerminalElement list) =
        let popoverMenu = props.props.getOrInit PKey.menuBarItemv2.popoverMenu_element (fun () -> PopoverMenuElement(IncrementalProps()))
        let menuv2 = popoverMenu.props.getOrInit PKey.popoverMenu.root_element (fun () -> Menuv2Element(IncrementalProps()))

        menuv2.props.add (PKey.menuv2.children, System.Collections.Generic.List<_>(value))

type menuBarv2Macros(props: menuBarv2Props) =
    member _.menuBarItemv2(set: menuBarItemv2Props -> menuBarItemv2Macros -> unit) =
        let menus =
            props.props.getOrInit PKey.menuBarv2.children (fun () -> System.Collections.Generic.List<ITerminalElement>())

        let props = menuBarItemv2Props()
        let macros = menuBarItemv2Macros(props)
        set props macros

        menus.Add (MenuBarItemv2Element(props.props))
