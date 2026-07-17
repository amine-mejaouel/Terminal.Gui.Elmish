namespace Terminal.Gui.Elmish

[<RequireQualifiedAccess>]
module private ItemsMacro =
  [<Literal>]
  let ListViewKey = "Terminal.Gui.Elmish.ListView.Items"

  [<Literal>]
  let DropDownListKey = "Terminal.Gui.Elmish.DropDownList.Items"

  let add (reconciledKey: string) (props: Props) (items: seq<'T>) (key: 'T -> 'Key) (text: 'T -> string) =
    let snapshot =
      items
      |> Seq.map (fun item ->
        { Key = key item :> obj
          Text = text item })
      |> List.ofSeq

    props |> Props.addReconciled (ReconciledListItems.create reconciledKey snapshot)

  let addStrings reconciledKey props (items: seq<string>) = add reconciledKey props items id id

type ListViewMacros internal (props: ListViewProps) =
  /// Declares string items whose text is also their stable identity. Duplicate strings require the keyed overload.
  member _.Items(items: seq<string>) =
    ItemsMacro.addStrings ItemsMacro.ListViewKey props.props items

  /// Declares immutable items and the stable key/text projections used by the retained native collection adapter.
  member _.Items(items: seq<'T>, key: 'T -> 'Key, text: 'T -> string) =
    ItemsMacro.add ItemsMacro.ListViewKey props.props items key text

type DropDownListMacros internal (props: DropDownListProps) =
  /// Declares string items whose text is also their stable identity. Duplicate strings require the keyed overload.
  member _.Items(items: seq<string>) =
    ItemsMacro.addStrings ItemsMacro.DropDownListKey props.props items

  /// Declares immutable items and the stable key/text projections used by the retained native collection adapter.
  member _.Items(items: seq<'T>, key: 'T -> 'Key, text: 'T -> string) =
    ItemsMacro.add ItemsMacro.DropDownListKey props.props items key text

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
