namespace Terminal.Gui.Elmish

open System
open Terminal.Gui.Elmish
open Terminal.Gui.ViewBase
open Terminal.Gui.Views

type View =

  static member adornment(set: AdornmentProps -> unit) =
    let viewProps = AdornmentProps ()
    set viewProps
    new AdornmentTerminalElement(viewProps.props) :> ITerminalElement

  static member adornment(children: ITerminalElement list) =
    let viewProps = AdornmentProps ()
    viewProps.children children
    new AdornmentTerminalElement(viewProps.props) :> ITerminalElement

  static member adornment(x: int, y: int, title: string) =
    let setProps =
      fun (p: AdornmentProps) ->
        p.X (Pos.Absolute(x))
        p.Y (Pos.Absolute(y))
        p.Title title

    let viewProps = AdornmentProps ()
    setProps viewProps
    new AdornmentTerminalElement(viewProps.props) :> ITerminalElement

  static member bar(set: BarProps -> unit) =
    let viewProps = BarProps ()
    set viewProps
    new BarTerminalElement(viewProps.props) :> ITerminalElement

  static member bar(children: ITerminalElement list) =
    let viewProps = BarProps ()
    viewProps.children children
    new BarTerminalElement(viewProps.props) :> ITerminalElement

  static member bar(x: int, y: int, title: string) =
    let setProps =
      fun (p: BarProps) ->
        p.X (Pos.Absolute(x))
        p.Y (Pos.Absolute(y))
        p.Title title

    let viewProps = BarProps ()
    setProps viewProps
    new BarTerminalElement(viewProps.props) :> ITerminalElement

  static member border(set: BorderProps -> unit) =
    let viewProps = BorderProps ()
    set viewProps
    new BorderTerminalElement(viewProps.props) :> ITerminalElement

  static member border(children: ITerminalElement list) =
    let viewProps = BorderProps ()
    viewProps.children children
    new BorderTerminalElement(viewProps.props) :> ITerminalElement

  static member border(x: int, y: int, title: string) =
    let setProps =
      fun (p: BorderProps) ->
        p.X (Pos.Absolute(x))
        p.Y (Pos.Absolute(y))
        p.Title title

    let viewProps = BorderProps ()
    setProps viewProps
    new BorderTerminalElement(viewProps.props) :> ITerminalElement

  static member button(set: ButtonProps -> unit) =
    let viewProps = ButtonProps ()
    set viewProps
    new ButtonTerminalElement(viewProps.props) :> ITerminalElement

  static member button(children: ITerminalElement list) =
    let viewProps = ButtonProps ()
    viewProps.children children
    new ButtonTerminalElement(viewProps.props) :> ITerminalElement

  static member button(x: int, y: int, title: string) =
    let setProps =
      fun (p: ButtonProps) ->
        p.X (Pos.Absolute(x))
        p.Y (Pos.Absolute(y))
        p.Title title

    let viewProps = ButtonProps ()
    setProps viewProps
    new ButtonTerminalElement(viewProps.props) :> ITerminalElement

  static member charMap(set: CharMapProps -> unit) =
    let viewProps = CharMapProps ()
    set viewProps
    new CharMapTerminalElement(viewProps.props) :> ITerminalElement

  static member charMap(children: ITerminalElement list) =
    let viewProps = CharMapProps ()
    viewProps.children children
    new CharMapTerminalElement(viewProps.props) :> ITerminalElement

  static member charMap(x: int, y: int, title: string) =
    let setProps =
      fun (p: CharMapProps) ->
        p.X (Pos.Absolute(x))
        p.Y (Pos.Absolute(y))
        p.Title title

    let viewProps = CharMapProps ()
    setProps viewProps
    new CharMapTerminalElement(viewProps.props) :> ITerminalElement

  static member checkBox(set: CheckBoxProps -> unit) =
    let viewProps = CheckBoxProps ()
    set viewProps
    new CheckBoxTerminalElement(viewProps.props) :> ITerminalElement

  static member checkBox(children: ITerminalElement list) =
    let viewProps = CheckBoxProps ()
    viewProps.children children
    new CheckBoxTerminalElement(viewProps.props) :> ITerminalElement

  static member checkBox(x: int, y: int, title: string) =
    let setProps =
      fun (p: CheckBoxProps) ->
        p.X (Pos.Absolute(x))
        p.Y (Pos.Absolute(y))
        p.Title title

    let viewProps = CheckBoxProps ()
    setProps viewProps
    new CheckBoxTerminalElement(viewProps.props) :> ITerminalElement

  static member colorPicker(set: ColorPickerProps -> unit) =
    let viewProps = ColorPickerProps ()
    set viewProps
    new ColorPickerTerminalElement(viewProps.props) :> ITerminalElement

  static member colorPicker(children: ITerminalElement list) =
    let viewProps = ColorPickerProps ()
    viewProps.children children
    new ColorPickerTerminalElement(viewProps.props) :> ITerminalElement

  static member colorPicker(x: int, y: int, title: string) =
    let setProps =
      fun (p: ColorPickerProps) ->
        p.X (Pos.Absolute(x))
        p.Y (Pos.Absolute(y))
        p.Title title

    let viewProps = ColorPickerProps ()
    setProps viewProps
    new ColorPickerTerminalElement(viewProps.props) :> ITerminalElement

  static member colorPicker16(set: ColorPicker16Props -> unit) =
    let viewProps = ColorPicker16Props ()
    set viewProps
    new ColorPicker16TerminalElement(viewProps.props) :> ITerminalElement

  static member colorPicker16(children: ITerminalElement list) =
    let viewProps = ColorPicker16Props ()
    viewProps.children children
    new ColorPicker16TerminalElement(viewProps.props) :> ITerminalElement

  static member colorPicker16(x: int, y: int, title: string) =
    let setProps =
      fun (p: ColorPicker16Props) ->
        p.X (Pos.Absolute(x))
        p.Y (Pos.Absolute(y))
        p.Title title

    let viewProps = ColorPicker16Props ()
    setProps viewProps
    new ColorPicker16TerminalElement(viewProps.props) :> ITerminalElement

  static member comboBox(set: ComboBoxProps -> unit) =
    let viewProps = ComboBoxProps ()
    set viewProps
    new ComboBoxTerminalElement(viewProps.props) :> ITerminalElement

  static member comboBox(children: ITerminalElement list) =
    let viewProps = ComboBoxProps ()
    viewProps.children children
    new ComboBoxTerminalElement(viewProps.props) :> ITerminalElement

  static member comboBox(x: int, y: int, title: string) =
    let setProps =
      fun (p: ComboBoxProps) ->
        p.X (Pos.Absolute(x))
        p.Y (Pos.Absolute(y))
        p.Title title

    let viewProps = ComboBoxProps ()
    setProps viewProps
    new ComboBoxTerminalElement(viewProps.props) :> ITerminalElement

  static member datePicker(set: DatePickerProps -> unit) =
    let viewProps = DatePickerProps ()
    set viewProps
    new DatePickerTerminalElement(viewProps.props) :> ITerminalElement

  static member datePicker(children: ITerminalElement list) =
    let viewProps = DatePickerProps ()
    viewProps.children children
    new DatePickerTerminalElement(viewProps.props) :> ITerminalElement

  static member datePicker(x: int, y: int, title: string) =
    let setProps =
      fun (p: DatePickerProps) ->
        p.X (Pos.Absolute(x))
        p.Y (Pos.Absolute(y))
        p.Title title

    let viewProps = DatePickerProps ()
    setProps viewProps
    new DatePickerTerminalElement(viewProps.props) :> ITerminalElement

  static member frameView(set: FrameViewProps -> unit) =
    let viewProps = FrameViewProps ()
    set viewProps
    new FrameViewTerminalElement(viewProps.props) :> ITerminalElement

  static member frameView(children: ITerminalElement list) =
    let viewProps = FrameViewProps ()
    viewProps.children children
    new FrameViewTerminalElement(viewProps.props) :> ITerminalElement

  static member frameView(x: int, y: int, title: string) =
    let setProps =
      fun (p: FrameViewProps) ->
        p.X (Pos.Absolute(x))
        p.Y (Pos.Absolute(y))
        p.Title title

    let viewProps = FrameViewProps ()
    setProps viewProps
    new FrameViewTerminalElement(viewProps.props) :> ITerminalElement

  static member graphView(set: GraphViewProps -> unit) =
    let viewProps = GraphViewProps ()
    set viewProps
    new GraphViewTerminalElement(viewProps.props) :> ITerminalElement

  static member graphView(children: ITerminalElement list) =
    let viewProps = GraphViewProps ()
    viewProps.children children
    new GraphViewTerminalElement(viewProps.props) :> ITerminalElement

  static member graphView(x: int, y: int, title: string) =
    let setProps =
      fun (p: GraphViewProps) ->
        p.X (Pos.Absolute(x))
        p.Y (Pos.Absolute(y))
        p.Title title

    let viewProps = GraphViewProps ()
    setProps viewProps
    new GraphViewTerminalElement(viewProps.props) :> ITerminalElement

  static member hexView(set: HexViewProps -> unit) =
    let viewProps = HexViewProps ()
    set viewProps
    new HexViewTerminalElement(viewProps.props) :> ITerminalElement

  static member hexView(children: ITerminalElement list) =
    let viewProps = HexViewProps ()
    viewProps.children children
    new HexViewTerminalElement(viewProps.props) :> ITerminalElement

  static member hexView(x: int, y: int, title: string) =
    let setProps =
      fun (p: HexViewProps) ->
        p.X (Pos.Absolute(x))
        p.Y (Pos.Absolute(y))
        p.Title title

    let viewProps = HexViewProps ()
    setProps viewProps
    new HexViewTerminalElement(viewProps.props) :> ITerminalElement

  static member label(set: LabelProps -> unit) =
    let viewProps = LabelProps ()
    set viewProps
    new LabelTerminalElement(viewProps.props) :> ITerminalElement

  static member label(children: ITerminalElement list) =
    let viewProps = LabelProps ()
    viewProps.children children
    new LabelTerminalElement(viewProps.props) :> ITerminalElement

  static member label(x: int, y: int, title: string) =
    let setProps =
      fun (p: LabelProps) ->
        p.X (Pos.Absolute(x))
        p.Y (Pos.Absolute(y))
        p.Title title

    let viewProps = LabelProps ()
    setProps viewProps
    new LabelTerminalElement(viewProps.props) :> ITerminalElement

  static member legendAnnotation(set: LegendAnnotationProps -> unit) =
    let viewProps = LegendAnnotationProps ()
    set viewProps
    new LegendAnnotationTerminalElement(viewProps.props) :> ITerminalElement

  static member legendAnnotation(children: ITerminalElement list) =
    let viewProps = LegendAnnotationProps ()
    viewProps.children children
    new LegendAnnotationTerminalElement(viewProps.props) :> ITerminalElement

  static member legendAnnotation(x: int, y: int, title: string) =
    let setProps =
      fun (p: LegendAnnotationProps) ->
        p.X (Pos.Absolute(x))
        p.Y (Pos.Absolute(y))
        p.Title title

    let viewProps = LegendAnnotationProps ()
    setProps viewProps
    new LegendAnnotationTerminalElement(viewProps.props) :> ITerminalElement

  static member line(set: LineProps -> unit) =
    let viewProps = LineProps ()
    set viewProps
    new LineTerminalElement(viewProps.props) :> ITerminalElement

  static member line(children: ITerminalElement list) =
    let viewProps = LineProps ()
    viewProps.children children
    new LineTerminalElement(viewProps.props) :> ITerminalElement

  static member line(x: int, y: int, title: string) =
    let setProps =
      fun (p: LineProps) ->
        p.X (Pos.Absolute(x))
        p.Y (Pos.Absolute(y))
        p.Title title

    let viewProps = LineProps ()
    setProps viewProps
    new LineTerminalElement(viewProps.props) :> ITerminalElement

  static member listView(set: ListViewProps -> unit) =
    let viewProps = ListViewProps ()
    set viewProps
    new ListViewTerminalElement(viewProps.props) :> ITerminalElement

  static member listView(children: ITerminalElement list) =
    let viewProps = ListViewProps ()
    viewProps.children children
    new ListViewTerminalElement(viewProps.props) :> ITerminalElement

  static member listView(x: int, y: int, title: string) =
    let setProps =
      fun (p: ListViewProps) ->
        p.X (Pos.Absolute(x))
        p.Y (Pos.Absolute(y))
        p.Title title

    let viewProps = ListViewProps ()
    setProps viewProps
    new ListViewTerminalElement(viewProps.props) :> ITerminalElement

  static member margin(set: MarginProps -> unit) =
    let viewProps = MarginProps ()
    set viewProps
    new MarginTerminalElement(viewProps.props) :> ITerminalElement

  static member margin(children: ITerminalElement list) =
    let viewProps = MarginProps ()
    viewProps.children children
    new MarginTerminalElement(viewProps.props) :> ITerminalElement

  static member margin(x: int, y: int, title: string) =
    let setProps =
      fun (p: MarginProps) ->
        p.X (Pos.Absolute(x))
        p.Y (Pos.Absolute(y))
        p.Title title

    let viewProps = MarginProps ()
    setProps viewProps
    new MarginTerminalElement(viewProps.props) :> ITerminalElement

  static member menu(set: MenuProps -> unit) =
    let viewProps = MenuProps ()
    set viewProps
    new MenuTerminalElement(viewProps.props) :> IMenuTerminalElement

  static member menu(children: ITerminalElement list) =
    let viewProps = MenuProps ()
    viewProps.children children
    new MenuTerminalElement(viewProps.props) :> IMenuTerminalElement

  static member menu(x: int, y: int, title: string) =
    let setProps =
      fun (p: MenuProps) ->
        p.X (Pos.Absolute(x))
        p.Y (Pos.Absolute(y))
        p.Title title

    let viewProps = MenuProps ()
    setProps viewProps
    new MenuTerminalElement(viewProps.props) :> IMenuTerminalElement

  static member menuBar(set: MenuBarProps -> menuBarMacros -> unit) =
    let props = MenuBarProps ()
    let macros = menuBarMacros props
    set props macros
    new MenuBarTerminalElement(props.props) :> ITerminalElement

  static member menuBar(children: ITerminalElement list) =
    let viewProps = MenuBarProps ()
    viewProps.children children
    new MenuBarTerminalElement(viewProps.props) :> ITerminalElement

  static member menuBar(x: int, y: int, title: string) =
    let setProps =
      fun (p: MenuBarProps) ->
        p.X (Pos.Absolute(x))
        p.Y (Pos.Absolute(y))
        p.Title title

    let viewProps = MenuBarProps ()
    setProps viewProps
    new MenuBarTerminalElement(viewProps.props) :> ITerminalElement

  static member numericUpDown<'T>(set: NumericUpDownProps<'T> -> unit) =
    let viewProps = NumericUpDownProps<'T> ()

    set viewProps
    new NumericUpDownTerminalElement<'T>(viewProps.props)
     :> ITerminalElement

  static member numericUpDown<'T>(children: ITerminalElement list) =
    let viewProps = NumericUpDownProps<'T> ()

    viewProps.children children
    new NumericUpDownTerminalElement<'T>(viewProps.props)
     :> ITerminalElement

  static member numericUpDown<'T>(x: int, y: int, title: string) =
    let setProps =
      fun (p: NumericUpDownProps<'T>) ->
        p.X (Pos.Absolute(x))
        p.Y (Pos.Absolute(y))
        p.Title title

    let viewProps = NumericUpDownProps<'T> ()

    setProps viewProps
    new NumericUpDownTerminalElement<'T>(viewProps.props)
     :> ITerminalElement

  static member padding(set: PaddingProps -> unit) =
    let viewProps = PaddingProps ()
    set viewProps
    new PaddingTerminalElement(viewProps.props) :> ITerminalElement

  static member padding(children: ITerminalElement list) =
    let viewProps = PaddingProps ()
    viewProps.children children
    new PaddingTerminalElement(viewProps.props) :> ITerminalElement

  static member padding(x: int, y: int, title: string) =
    let setProps =
      fun (p: PaddingProps) ->
        p.X (Pos.Absolute(x))
        p.Y (Pos.Absolute(y))
        p.Title title

    let viewProps = PaddingProps ()
    setProps viewProps
    new PaddingTerminalElement(viewProps.props) :> ITerminalElement

  static member popoverMenu(set: PopoverMenuProps -> unit) =
    let viewProps = PopoverMenuProps ()
    set viewProps
    new PopoverMenuTerminalElement(viewProps.props) :> IPopoverMenuTerminalElement

  static member popoverMenu(children: ITerminalElement list) =
    let viewProps = PopoverMenuProps ()
    viewProps.children children
    new PopoverMenuTerminalElement(viewProps.props) :> IPopoverMenuTerminalElement

  static member popoverMenu(x: int, y: int, title: string) =
    let setProps =
      fun (p: PopoverMenuProps) ->
        p.X (Pos.Absolute(x))
        p.Y (Pos.Absolute(y))
        p.Title title

    let viewProps = PopoverMenuProps ()
    setProps viewProps
    new PopoverMenuTerminalElement(viewProps.props) :> IPopoverMenuTerminalElement

  static member progressBar(set: ProgressBarProps -> unit) =
    let viewProps = ProgressBarProps ()
    set viewProps
    new ProgressBarTerminalElement(viewProps.props) :> ITerminalElement

  static member progressBar(children: ITerminalElement list) =
    let viewProps = ProgressBarProps ()
    viewProps.children children
    new ProgressBarTerminalElement(viewProps.props) :> ITerminalElement

  static member progressBar(x: int, y: int, title: string) =
    let setProps =
      fun (p: ProgressBarProps) ->
        p.X (Pos.Absolute(x))
        p.Y (Pos.Absolute(y))
        p.Title title

    let viewProps = ProgressBarProps ()
    setProps viewProps
    new ProgressBarTerminalElement(viewProps.props) :> ITerminalElement

  static member runnable(set: RunnableProps -> unit) =
    let viewProps = RunnableProps ()
    set viewProps
    new RunnableTerminalElement(viewProps.props) :> ITerminalElement

  static member runnable(children: ITerminalElement list) =
    let viewProps = RunnableProps ()
    viewProps.children children
    new RunnableTerminalElement(viewProps.props) :> ITerminalElement

  static member runnable(x: int, y: int, title: string) =
    let setProps =
      fun (p: RunnableProps) ->
        p.X (Pos.Absolute(x))
        p.Y (Pos.Absolute(y))
        p.Title title

    let viewProps = RunnableProps ()
    setProps viewProps
    new RunnableTerminalElement(viewProps.props) :> ITerminalElement

  static member runnable<'TResult>(set: RunnableProps<'TResult> -> unit) =
    let viewProps = RunnableProps<'TResult> ()

    set viewProps
    new RunnableTerminalElement<'TResult>(viewProps.props)
     :> ITerminalElement

  static member runnable<'TResult>(children: ITerminalElement list) =
    let viewProps = RunnableProps<'TResult> ()

    viewProps.children children
    new RunnableTerminalElement<'TResult>(viewProps.props)
     :> ITerminalElement

  static member runnable<'TResult>(x: int, y: int, title: string) =
    let setProps =
      fun (p: RunnableProps<'TResult>) ->
        p.X (Pos.Absolute(x))
        p.Y (Pos.Absolute(y))
        p.Title title

    let viewProps = RunnableProps<'TResult> ()

    setProps viewProps
    new RunnableTerminalElement<'TResult>(viewProps.props)
     :> ITerminalElement

  static member scrollBar(set: ScrollBarProps -> unit) =
    let viewProps = ScrollBarProps ()
    set viewProps
    new ScrollBarTerminalElement(viewProps.props) :> ITerminalElement

  static member scrollBar(children: ITerminalElement list) =
    let viewProps = ScrollBarProps ()
    viewProps.children children
    new ScrollBarTerminalElement(viewProps.props) :> ITerminalElement

  static member scrollBar(x: int, y: int, title: string) =
    let setProps =
      fun (p: ScrollBarProps) ->
        p.X (Pos.Absolute(x))
        p.Y (Pos.Absolute(y))
        p.Title title

    let viewProps = ScrollBarProps ()
    setProps viewProps
    new ScrollBarTerminalElement(viewProps.props) :> ITerminalElement

  static member scrollSlider(set: ScrollSliderProps -> unit) =
    let viewProps = ScrollSliderProps ()
    set viewProps
    new ScrollSliderTerminalElement(viewProps.props) :> ITerminalElement

  static member scrollSlider(children: ITerminalElement list) =
    let viewProps = ScrollSliderProps ()
    viewProps.children children
    new ScrollSliderTerminalElement(viewProps.props) :> ITerminalElement

  static member scrollSlider(x: int, y: int, title: string) =
    let setProps =
      fun (p: ScrollSliderProps) ->
        p.X (Pos.Absolute(x))
        p.Y (Pos.Absolute(y))
        p.Title title

    let viewProps = ScrollSliderProps ()
    setProps viewProps
    new ScrollSliderTerminalElement(viewProps.props) :> ITerminalElement

  static member flagSelector(set: FlagSelectorProps -> unit) =
    let viewProps = FlagSelectorProps ()
    set viewProps
    new FlagSelectorTerminalElement(viewProps.props) :> ITerminalElement

  static member flagSelector(children: ITerminalElement list) =
    let viewProps = FlagSelectorProps ()
    viewProps.children children
    new FlagSelectorTerminalElement(viewProps.props) :> ITerminalElement

  static member flagSelector(x: int, y: int, title: string) =
    let setProps =
      fun (p: FlagSelectorProps) ->
        p.X (Pos.Absolute(x))
        p.Y (Pos.Absolute(y))
        p.Title title

    let viewProps = FlagSelectorProps ()
    setProps viewProps
    new FlagSelectorTerminalElement(viewProps.props) :> ITerminalElement

  static member optionSelector(set: OptionSelectorProps -> unit) =
    let viewProps = OptionSelectorProps ()
    set viewProps
    new OptionSelectorTerminalElement(viewProps.props) :> ITerminalElement

  static member optionSelector(children: ITerminalElement list) =
    let viewProps = OptionSelectorProps ()
    viewProps.children children
    new OptionSelectorTerminalElement(viewProps.props) :> ITerminalElement

  static member optionSelector(x: int, y: int, title: string) =
    let setProps =
      fun (p: OptionSelectorProps) ->
        p.X (Pos.Absolute(x))
        p.Y (Pos.Absolute(y))
        p.Title title

    let viewProps = OptionSelectorProps ()
    setProps viewProps
    new OptionSelectorTerminalElement(viewProps.props) :> ITerminalElement

  static member flagSelector<'TFlagsEnum when 'TFlagsEnum: struct and 'TFlagsEnum: (new: unit -> 'TFlagsEnum) and 'TFlagsEnum:> Enum and 'TFlagsEnum:> ValueType>(set: FlagSelectorProps<'TFlagsEnum> -> unit) =
    let viewProps = FlagSelectorProps<'TFlagsEnum> ()

    set viewProps
    new FlagSelectorTerminalElement<'TFlagsEnum>(viewProps.props)
     :> ITerminalElement

  static member flagSelector<'TFlagsEnum when 'TFlagsEnum: struct and 'TFlagsEnum: (new: unit -> 'TFlagsEnum) and 'TFlagsEnum:> Enum and 'TFlagsEnum:> ValueType>(children: ITerminalElement list) =
    let viewProps = FlagSelectorProps<'TFlagsEnum> ()

    viewProps.children children
    new FlagSelectorTerminalElement<'TFlagsEnum>(viewProps.props)
     :> ITerminalElement

  static member flagSelector<'TFlagsEnum when 'TFlagsEnum: struct and 'TFlagsEnum: (new: unit -> 'TFlagsEnum) and 'TFlagsEnum:> Enum and 'TFlagsEnum:> ValueType>(x: int, y: int, title: string) =
    let setProps =
      fun (p: FlagSelectorProps<'TFlagsEnum>) ->
        p.X (Pos.Absolute(x))
        p.Y (Pos.Absolute(y))
        p.Title title

    let viewProps = FlagSelectorProps<'TFlagsEnum> ()

    setProps viewProps
    new FlagSelectorTerminalElement<'TFlagsEnum>(viewProps.props)
     :> ITerminalElement

  static member optionSelector<'TEnum when 'TEnum: struct and 'TEnum: (new: unit -> 'TEnum) and 'TEnum:> Enum and 'TEnum:> ValueType>(set: OptionSelectorProps<'TEnum> -> unit) =
    let viewProps = OptionSelectorProps<'TEnum> ()

    set viewProps
    new OptionSelectorTerminalElement<'TEnum>(viewProps.props)
     :> ITerminalElement

  static member optionSelector<'TEnum when 'TEnum: struct and 'TEnum: (new: unit -> 'TEnum) and 'TEnum:> Enum and 'TEnum:> ValueType>(children: ITerminalElement list) =
    let viewProps = OptionSelectorProps<'TEnum> ()

    viewProps.children children
    new OptionSelectorTerminalElement<'TEnum>(viewProps.props)
     :> ITerminalElement

  static member optionSelector<'TEnum when 'TEnum: struct and 'TEnum: (new: unit -> 'TEnum) and 'TEnum:> Enum and 'TEnum:> ValueType>(x: int, y: int, title: string) =
    let setProps =
      fun (p: OptionSelectorProps<'TEnum>) ->
        p.X (Pos.Absolute(x))
        p.Y (Pos.Absolute(y))
        p.Title title

    let viewProps = OptionSelectorProps<'TEnum> ()

    setProps viewProps
    new OptionSelectorTerminalElement<'TEnum>(viewProps.props)
     :> ITerminalElement

  static member shortcut(set: ShortcutProps -> unit) =
    let viewProps = ShortcutProps ()
    set viewProps
    new ShortcutTerminalElement(viewProps.props) :> ITerminalElement

  static member shortcut(children: ITerminalElement list) =
    let viewProps = ShortcutProps ()
    viewProps.children children
    new ShortcutTerminalElement(viewProps.props) :> ITerminalElement

  static member shortcut(x: int, y: int, title: string) =
    let setProps =
      fun (p: ShortcutProps) ->
        p.X (Pos.Absolute(x))
        p.Y (Pos.Absolute(y))
        p.Title title

    let viewProps = ShortcutProps ()
    setProps viewProps
    new ShortcutTerminalElement(viewProps.props) :> ITerminalElement

  static member menuItem(set: MenuItemProps -> unit) =
    let viewProps = MenuItemProps ()
    set viewProps
    new MenuItemTerminalElement(viewProps.props) :> IMenuItemTerminalElement

  static member menuItem(children: ITerminalElement list) =
    let viewProps = MenuItemProps ()
    viewProps.children children
    new MenuItemTerminalElement(viewProps.props) :> IMenuItemTerminalElement

  static member menuItem(x: int, y: int, title: string) =
    let setProps =
      fun (p: MenuItemProps) ->
        p.X (Pos.Absolute(x))
        p.Y (Pos.Absolute(y))
        p.Title title

    let viewProps = MenuItemProps ()
    setProps viewProps
    new MenuItemTerminalElement(viewProps.props) :> IMenuItemTerminalElement

  static member menuBarItem(set: MenuBarItemProps -> menuBarItemMacros -> unit) =
    let props = MenuBarItemProps ()
    let macros = menuBarItemMacros props
    set props macros
    new MenuBarItemTerminalElement(props.props) :> ITerminalElement

  static member menuBarItem(children: ITerminalElement list) =
    let viewProps = MenuBarItemProps ()
    viewProps.children children
    new MenuBarItemTerminalElement(viewProps.props) :> ITerminalElement

  static member menuBarItem(x: int, y: int, title: string) =
    let setProps =
      fun (p: MenuBarItemProps) ->
        p.X (Pos.Absolute(x))
        p.Y (Pos.Absolute(y))
        p.Title title

    let viewProps = MenuBarItemProps ()
    setProps viewProps
    new MenuBarItemTerminalElement(viewProps.props) :> ITerminalElement

  static member slider<'T>(set: SliderProps<'T> -> unit) =
    let viewProps = SliderProps<'T> ()

    set viewProps
    new SliderTerminalElement<'T>(viewProps.props)
     :> ITerminalElement

  static member slider<'T>(children: ITerminalElement list) =
    let viewProps = SliderProps<'T> ()

    viewProps.children children
    new SliderTerminalElement<'T>(viewProps.props)
     :> ITerminalElement

  static member slider<'T>(x: int, y: int, title: string) =
    let setProps =
      fun (p: SliderProps<'T>) ->
        p.X (Pos.Absolute(x))
        p.Y (Pos.Absolute(y))
        p.Title title

    let viewProps = SliderProps<'T> ()

    setProps viewProps
    new SliderTerminalElement<'T>(viewProps.props)
     :> ITerminalElement

  static member spinnerView(set: SpinnerViewProps -> unit) =
    let viewProps = SpinnerViewProps ()
    set viewProps
    new SpinnerViewTerminalElement(viewProps.props) :> ITerminalElement

  static member spinnerView(children: ITerminalElement list) =
    let viewProps = SpinnerViewProps ()
    viewProps.children children
    new SpinnerViewTerminalElement(viewProps.props) :> ITerminalElement

  static member spinnerView(x: int, y: int, title: string) =
    let setProps =
      fun (p: SpinnerViewProps) ->
        p.X (Pos.Absolute(x))
        p.Y (Pos.Absolute(y))
        p.Title title

    let viewProps = SpinnerViewProps ()
    setProps viewProps
    new SpinnerViewTerminalElement(viewProps.props) :> ITerminalElement

  static member statusBar(set: StatusBarProps -> unit) =
    let viewProps = StatusBarProps ()
    set viewProps
    new StatusBarTerminalElement(viewProps.props) :> ITerminalElement

  static member statusBar(children: ITerminalElement list) =
    let viewProps = StatusBarProps ()
    viewProps.children children
    new StatusBarTerminalElement(viewProps.props) :> ITerminalElement

  static member statusBar(x: int, y: int, title: string) =
    let setProps =
      fun (p: StatusBarProps) ->
        p.X (Pos.Absolute(x))
        p.Y (Pos.Absolute(y))
        p.Title title

    let viewProps = StatusBarProps ()
    setProps viewProps
    new StatusBarTerminalElement(viewProps.props) :> ITerminalElement

  static member tab(set: TabProps -> unit) =
    let viewProps = TabProps ()
    set viewProps
    new TabTerminalElement(viewProps.props) :> ITabTerminalElement

  static member tab(children: ITerminalElement list) =
    let viewProps = TabProps ()
    viewProps.children children
    new TabTerminalElement(viewProps.props) :> ITabTerminalElement

  static member tab(x: int, y: int, title: string) =
    let setProps =
      fun (p: TabProps) ->
        p.X (Pos.Absolute(x))
        p.Y (Pos.Absolute(y))
        p.Title title

    let viewProps = TabProps ()
    setProps viewProps
    new TabTerminalElement(viewProps.props) :> ITabTerminalElement

  static member tabView(set: TabViewProps -> unit) =
    let viewProps = TabViewProps ()
    set viewProps
    new TabViewTerminalElement(viewProps.props) :> ITerminalElement

  static member tabView(children: ITerminalElement list) =
    let viewProps = TabViewProps ()
    viewProps.children children
    new TabViewTerminalElement(viewProps.props) :> ITerminalElement

  static member tabView(x: int, y: int, title: string) =
    let setProps =
      fun (p: TabViewProps) ->
        p.X (Pos.Absolute(x))
        p.Y (Pos.Absolute(y))
        p.Title title

    let viewProps = TabViewProps ()
    setProps viewProps
    new TabViewTerminalElement(viewProps.props) :> ITerminalElement

  static member tableView(set: TableViewProps -> unit) =
    let viewProps = TableViewProps ()
    set viewProps
    new TableViewTerminalElement(viewProps.props) :> ITerminalElement

  static member tableView(children: ITerminalElement list) =
    let viewProps = TableViewProps ()
    viewProps.children children
    new TableViewTerminalElement(viewProps.props) :> ITerminalElement

  static member tableView(x: int, y: int, title: string) =
    let setProps =
      fun (p: TableViewProps) ->
        p.X (Pos.Absolute(x))
        p.Y (Pos.Absolute(y))
        p.Title title

    let viewProps = TableViewProps ()
    setProps viewProps
    new TableViewTerminalElement(viewProps.props) :> ITerminalElement

  static member textField(set: TextFieldProps -> unit) =
    let viewProps = TextFieldProps ()
    set viewProps
    new TextFieldTerminalElement(viewProps.props) :> ITerminalElement

  static member textField(children: ITerminalElement list) =
    let viewProps = TextFieldProps ()
    viewProps.children children
    new TextFieldTerminalElement(viewProps.props) :> ITerminalElement

  static member textField(x: int, y: int, title: string) =
    let setProps =
      fun (p: TextFieldProps) ->
        p.X (Pos.Absolute(x))
        p.Y (Pos.Absolute(y))
        p.Title title

    let viewProps = TextFieldProps ()
    setProps viewProps
    new TextFieldTerminalElement(viewProps.props) :> ITerminalElement

  static member dateField(set: DateFieldProps -> unit) =
    let viewProps = DateFieldProps ()
    set viewProps
    new DateFieldTerminalElement(viewProps.props) :> ITerminalElement

  static member dateField(children: ITerminalElement list) =
    let viewProps = DateFieldProps ()
    viewProps.children children
    new DateFieldTerminalElement(viewProps.props) :> ITerminalElement

  static member dateField(x: int, y: int, title: string) =
    let setProps =
      fun (p: DateFieldProps) ->
        p.X (Pos.Absolute(x))
        p.Y (Pos.Absolute(y))
        p.Title title

    let viewProps = DateFieldProps ()
    setProps viewProps
    new DateFieldTerminalElement(viewProps.props) :> ITerminalElement

  static member textValidateField(set: TextValidateFieldProps -> unit) =
    let viewProps = TextValidateFieldProps ()
    set viewProps
    new TextValidateFieldTerminalElement(viewProps.props) :> ITerminalElement

  static member textValidateField(children: ITerminalElement list) =
    let viewProps = TextValidateFieldProps ()
    viewProps.children children
    new TextValidateFieldTerminalElement(viewProps.props) :> ITerminalElement

  static member textValidateField(x: int, y: int, title: string) =
    let setProps =
      fun (p: TextValidateFieldProps) ->
        p.X (Pos.Absolute(x))
        p.Y (Pos.Absolute(y))
        p.Title title

    let viewProps = TextValidateFieldProps ()
    setProps viewProps
    new TextValidateFieldTerminalElement(viewProps.props) :> ITerminalElement

  static member textView(set: TextViewProps -> unit) =
    let viewProps = TextViewProps ()
    set viewProps
    new TextViewTerminalElement(viewProps.props) :> ITerminalElement

  static member textView(children: ITerminalElement list) =
    let viewProps = TextViewProps ()
    viewProps.children children
    new TextViewTerminalElement(viewProps.props) :> ITerminalElement

  static member textView(x: int, y: int, title: string) =
    let setProps =
      fun (p: TextViewProps) ->
        p.X (Pos.Absolute(x))
        p.Y (Pos.Absolute(y))
        p.Title title

    let viewProps = TextViewProps ()
    setProps viewProps
    new TextViewTerminalElement(viewProps.props) :> ITerminalElement

  static member timeField(set: TimeFieldProps -> unit) =
    let viewProps = TimeFieldProps ()
    set viewProps
    new TimeFieldTerminalElement(viewProps.props) :> ITerminalElement

  static member timeField(children: ITerminalElement list) =
    let viewProps = TimeFieldProps ()
    viewProps.children children
    new TimeFieldTerminalElement(viewProps.props) :> ITerminalElement

  static member timeField(x: int, y: int, title: string) =
    let setProps =
      fun (p: TimeFieldProps) ->
        p.X (Pos.Absolute(x))
        p.Y (Pos.Absolute(y))
        p.Title title

    let viewProps = TimeFieldProps ()
    setProps viewProps
    new TimeFieldTerminalElement(viewProps.props) :> ITerminalElement

  static member treeView<'T when 'T: not struct>(set: TreeViewProps<'T> -> unit) =
    let viewProps = TreeViewProps<'T> ()

    set viewProps
    new TreeViewTerminalElement<'T>(viewProps.props)
     :> ITerminalElement

  static member treeView<'T when 'T: not struct>(children: ITerminalElement list) =
    let viewProps = TreeViewProps<'T> ()

    viewProps.children children
    new TreeViewTerminalElement<'T>(viewProps.props)
     :> ITerminalElement

  static member treeView<'T when 'T: not struct>(x: int, y: int, title: string) =
    let setProps =
      fun (p: TreeViewProps<'T>) ->
        p.X (Pos.Absolute(x))
        p.Y (Pos.Absolute(y))
        p.Title title

    let viewProps = TreeViewProps<'T> ()

    setProps viewProps
    new TreeViewTerminalElement<'T>(viewProps.props)
     :> ITerminalElement

  static member window(set: WindowProps -> unit) =
    let viewProps = WindowProps ()
    set viewProps
    new WindowTerminalElement(viewProps.props) :> ITerminalElement

  static member window(children: ITerminalElement list) =
    let viewProps = WindowProps ()
    viewProps.children children
    new WindowTerminalElement(viewProps.props) :> ITerminalElement

  static member window(x: int, y: int, title: string) =
    let setProps =
      fun (p: WindowProps) ->
        p.X (Pos.Absolute(x))
        p.Y (Pos.Absolute(y))
        p.Title title

    let viewProps = WindowProps ()
    setProps viewProps
    new WindowTerminalElement(viewProps.props) :> ITerminalElement

  static member dialog(set: DialogProps -> unit) =
    let viewProps = DialogProps ()
    set viewProps
    new DialogTerminalElement(viewProps.props) :> ITerminalElement

  static member dialog(children: ITerminalElement list) =
    let viewProps = DialogProps ()
    viewProps.children children
    new DialogTerminalElement(viewProps.props) :> ITerminalElement

  static member dialog(x: int, y: int, title: string) =
    let setProps =
      fun (p: DialogProps) ->
        p.X (Pos.Absolute(x))
        p.Y (Pos.Absolute(y))
        p.Title title

    let viewProps = DialogProps ()
    setProps viewProps
    new DialogTerminalElement(viewProps.props) :> ITerminalElement

  static member fileDialog(set: FileDialogProps -> unit) =
    let viewProps = FileDialogProps ()
    set viewProps
    new FileDialogTerminalElement(viewProps.props) :> ITerminalElement

  static member fileDialog(children: ITerminalElement list) =
    let viewProps = FileDialogProps ()
    viewProps.children children
    new FileDialogTerminalElement(viewProps.props) :> ITerminalElement

  static member fileDialog(x: int, y: int, title: string) =
    let setProps =
      fun (p: FileDialogProps) ->
        p.X (Pos.Absolute(x))
        p.Y (Pos.Absolute(y))
        p.Title title

    let viewProps = FileDialogProps ()
    setProps viewProps
    new FileDialogTerminalElement(viewProps.props) :> ITerminalElement

  static member openDialog(set: OpenDialogProps -> unit) =
    let viewProps = OpenDialogProps ()
    set viewProps
    new OpenDialogTerminalElement(viewProps.props) :> ITerminalElement

  static member openDialog(children: ITerminalElement list) =
    let viewProps = OpenDialogProps ()
    viewProps.children children
    new OpenDialogTerminalElement(viewProps.props) :> ITerminalElement

  static member openDialog(x: int, y: int, title: string) =
    let setProps =
      fun (p: OpenDialogProps) ->
        p.X (Pos.Absolute(x))
        p.Y (Pos.Absolute(y))
        p.Title title

    let viewProps = OpenDialogProps ()
    setProps viewProps
    new OpenDialogTerminalElement(viewProps.props) :> ITerminalElement

  static member saveDialog(set: SaveDialogProps -> unit) =
    let viewProps = SaveDialogProps ()
    set viewProps
    new SaveDialogTerminalElement(viewProps.props) :> ITerminalElement

  static member saveDialog(children: ITerminalElement list) =
    let viewProps = SaveDialogProps ()
    viewProps.children children
    new SaveDialogTerminalElement(viewProps.props) :> ITerminalElement

  static member saveDialog(x: int, y: int, title: string) =
    let setProps =
      fun (p: SaveDialogProps) ->
        p.X (Pos.Absolute(x))
        p.Y (Pos.Absolute(y))
        p.Title title

    let viewProps = SaveDialogProps ()
    setProps viewProps
    new SaveDialogTerminalElement(viewProps.props) :> ITerminalElement

  static member wizard(set: WizardProps -> unit) =
    let viewProps = WizardProps ()
    set viewProps
    new WizardTerminalElement(viewProps.props) :> ITerminalElement

  static member wizard(children: ITerminalElement list) =
    let viewProps = WizardProps ()
    viewProps.children children
    new WizardTerminalElement(viewProps.props) :> ITerminalElement

  static member wizard(x: int, y: int, title: string) =
    let setProps =
      fun (p: WizardProps) ->
        p.X (Pos.Absolute(x))
        p.Y (Pos.Absolute(y))
        p.Title title

    let viewProps = WizardProps ()
    setProps viewProps
    new WizardTerminalElement(viewProps.props) :> ITerminalElement

  static member wizardStep(set: WizardStepProps -> unit) =
    let viewProps = WizardStepProps ()
    set viewProps
    new WizardStepTerminalElement(viewProps.props) :> IWizardStepTerminalElement

  static member wizardStep(children: ITerminalElement list) =
    let viewProps = WizardStepProps ()
    viewProps.children children
    new WizardStepTerminalElement(viewProps.props) :> IWizardStepTerminalElement

  static member wizardStep(x: int, y: int, title: string) =
    let setProps =
      fun (p: WizardStepProps) ->
        p.X (Pos.Absolute(x))
        p.Y (Pos.Absolute(y))
        p.Title title

    let viewProps = WizardStepProps ()
    setProps viewProps
    new WizardStepTerminalElement(viewProps.props) :> IWizardStepTerminalElement
