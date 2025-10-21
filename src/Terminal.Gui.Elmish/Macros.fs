namespace Terminal.Gui.Elmish

open Terminal.Gui.Elmish.Elements

type menuBarItemv2Macros internal (props: menuBarItemv2Props) =
    member _.menuItems(value: ITerminalElement list) =
        let popoverMenu = props.props.getOrInit PKey.menuBarItemv2.popoverMenu_element (fun () -> PopoverMenuElement(Props())) :?> PopoverMenuElement
        let menuv2 = popoverMenu.props.getOrInit PKey.popoverMenu.root_element (fun () -> Menuv2Element(Props())) :?> Menuv2Element

        menuv2.props.add (PKey.menuv2.children, System.Collections.Generic.List<_>(value |> List.map (fun x -> x :?> IInternalTerminalElement)))

type menuBarv2Macros internal (props: menuBarv2Props) =
    member _.menuBarItemv2(set: menuBarItemv2Props -> menuBarItemv2Macros -> unit) =
        let menus =
            props.props.getOrInit PKey.menuBarv2.children (fun () -> System.Collections.Generic.List<IInternalTerminalElement>())

        let props = menuBarItemv2Props()
        let macros = menuBarItemv2Macros(props)
        set props macros

        menus.Add (MenuBarItemv2Element(props.props))
