namespace Terminal.Gui.Elmish

open System
open Terminal.Gui.Elmish
open Terminal.Gui.ViewBase
open Terminal.Gui.Views

type View =

  static member adornment(set: AdornmentProps -> unit) =
    let viewProps = AdornmentProps ()
    set viewProps
    new AdornmentTerminalElement(viewProps.props)
    :> IViewTerminalElement

  static member adornment(children: ITerminalElement list) =
    let viewProps = AdornmentProps ()
    viewProps.Children children
    new AdornmentTerminalElement(viewProps.props)
    :> IViewTerminalElement

  static member bar(set: BarProps -> unit) =
    let viewProps = BarProps ()
    set viewProps
    new BarTerminalElement(viewProps.props)
    :> IViewTerminalElement

  static member bar(children: ITerminalElement list) =
    let viewProps = BarProps ()
    viewProps.Children children
    new BarTerminalElement(viewProps.props)
    :> IViewTerminalElement

  static member border(set: BorderProps -> unit) =
    let viewProps = BorderProps ()
    set viewProps
    new BorderTerminalElement(viewProps.props)
    :> IViewTerminalElement

  static member border(children: ITerminalElement list) =
    let viewProps = BorderProps ()
    viewProps.Children children
    new BorderTerminalElement(viewProps.props)
    :> IViewTerminalElement

  static member button(set: ButtonProps -> unit) =
    let viewProps = ButtonProps ()
    set viewProps
    new ButtonTerminalElement(viewProps.props)
    :> IViewTerminalElement

  static member button(children: ITerminalElement list) =
    let viewProps = ButtonProps ()
    viewProps.Children children
    new ButtonTerminalElement(viewProps.props)
    :> IViewTerminalElement

  static member charMap(set: CharMapProps -> unit) =
    let viewProps = CharMapProps ()
    set viewProps
    new CharMapTerminalElement(viewProps.props)
    :> IViewTerminalElement

  static member charMap(children: ITerminalElement list) =
    let viewProps = CharMapProps ()
    viewProps.Children children
    new CharMapTerminalElement(viewProps.props)
    :> IViewTerminalElement

  static member checkBox(set: CheckBoxProps -> unit) =
    let viewProps = CheckBoxProps ()
    set viewProps
    new CheckBoxTerminalElement(viewProps.props)
    :> IViewTerminalElement

  static member checkBox(children: ITerminalElement list) =
    let viewProps = CheckBoxProps ()
    viewProps.Children children
    new CheckBoxTerminalElement(viewProps.props)
    :> IViewTerminalElement

  static member colorPicker(set: ColorPickerProps -> unit) =
    let viewProps = ColorPickerProps ()
    set viewProps
    new ColorPickerTerminalElement(viewProps.props)
    :> IViewTerminalElement

  static member colorPicker(children: ITerminalElement list) =
    let viewProps = ColorPickerProps ()
    viewProps.Children children
    new ColorPickerTerminalElement(viewProps.props)
    :> IViewTerminalElement

  static member colorPicker16(set: ColorPicker16Props -> unit) =
    let viewProps = ColorPicker16Props ()
    set viewProps
    new ColorPicker16TerminalElement(viewProps.props)
    :> IViewTerminalElement

  static member colorPicker16(children: ITerminalElement list) =
    let viewProps = ColorPicker16Props ()
    viewProps.Children children
    new ColorPicker16TerminalElement(viewProps.props)
    :> IViewTerminalElement

  static member comboBox(set: ComboBoxProps -> unit) =
    let viewProps = ComboBoxProps ()
    set viewProps
    new ComboBoxTerminalElement(viewProps.props)
    :> IViewTerminalElement

  static member comboBox(children: ITerminalElement list) =
    let viewProps = ComboBoxProps ()
    viewProps.Children children
    new ComboBoxTerminalElement(viewProps.props)
    :> IViewTerminalElement

  static member datePicker(set: DatePickerProps -> unit) =
    let viewProps = DatePickerProps ()
    set viewProps
    new DatePickerTerminalElement(viewProps.props)
    :> IViewTerminalElement

  static member datePicker(children: ITerminalElement list) =
    let viewProps = DatePickerProps ()
    viewProps.Children children
    new DatePickerTerminalElement(viewProps.props)
    :> IViewTerminalElement

  static member frameView(set: FrameViewProps -> unit) =
    let viewProps = FrameViewProps ()
    set viewProps
    new FrameViewTerminalElement(viewProps.props)
    :> IViewTerminalElement

  static member frameView(children: ITerminalElement list) =
    let viewProps = FrameViewProps ()
    viewProps.Children children
    new FrameViewTerminalElement(viewProps.props)
    :> IViewTerminalElement

  static member graphView(set: GraphViewProps -> unit) =
    let viewProps = GraphViewProps ()
    set viewProps
    new GraphViewTerminalElement(viewProps.props)
    :> IViewTerminalElement

  static member graphView(children: ITerminalElement list) =
    let viewProps = GraphViewProps ()
    viewProps.Children children
    new GraphViewTerminalElement(viewProps.props)
    :> IViewTerminalElement

  static member hexView(set: HexViewProps -> unit) =
    let viewProps = HexViewProps ()
    set viewProps
    new HexViewTerminalElement(viewProps.props)
    :> IViewTerminalElement

  static member hexView(children: ITerminalElement list) =
    let viewProps = HexViewProps ()
    viewProps.Children children
    new HexViewTerminalElement(viewProps.props)
    :> IViewTerminalElement

  static member label(set: LabelProps -> unit) =
    let viewProps = LabelProps ()
    set viewProps
    new LabelTerminalElement(viewProps.props)
    :> IViewTerminalElement

  static member label(children: ITerminalElement list) =
    let viewProps = LabelProps ()
    viewProps.Children children
    new LabelTerminalElement(viewProps.props)
    :> IViewTerminalElement

  static member legendAnnotation(set: LegendAnnotationProps -> unit) =
    let viewProps = LegendAnnotationProps ()
    set viewProps
    new LegendAnnotationTerminalElement(viewProps.props)
    :> IViewTerminalElement

  static member legendAnnotation(children: ITerminalElement list) =
    let viewProps = LegendAnnotationProps ()
    viewProps.Children children
    new LegendAnnotationTerminalElement(viewProps.props)
    :> IViewTerminalElement

  static member line(set: LineProps -> unit) =
    let viewProps = LineProps ()
    set viewProps
    new LineTerminalElement(viewProps.props)
    :> IViewTerminalElement

  static member line(children: ITerminalElement list) =
    let viewProps = LineProps ()
    viewProps.Children children
    new LineTerminalElement(viewProps.props)
    :> IViewTerminalElement

  static member listView(set: ListViewProps -> unit) =
    let viewProps = ListViewProps ()
    set viewProps
    new ListViewTerminalElement(viewProps.props)
    :> IViewTerminalElement

  static member listView(children: ITerminalElement list) =
    let viewProps = ListViewProps ()
    viewProps.Children children
    new ListViewTerminalElement(viewProps.props)
    :> IViewTerminalElement

  static member margin(set: MarginProps -> unit) =
    let viewProps = MarginProps ()
    set viewProps
    new MarginTerminalElement(viewProps.props)
    :> IViewTerminalElement

  static member margin(children: ITerminalElement list) =
    let viewProps = MarginProps ()
    viewProps.Children children
    new MarginTerminalElement(viewProps.props)
    :> IViewTerminalElement

  static member menu(set: MenuProps -> unit) =
    let viewProps = MenuProps ()
    set viewProps
    new MenuTerminalElement(viewProps.props)
    :> IMenuTerminalElement

  static member menu(children: ITerminalElement list) =
    let viewProps = MenuProps ()
    viewProps.Children children
    new MenuTerminalElement(viewProps.props)
    :> IMenuTerminalElement

  static member menuBar(set: MenuBarProps -> menuBarMacros -> unit) =
    let props = MenuBarProps ()
    let macros = menuBarMacros props
    set props macros
    new MenuBarTerminalElement(props.props)
    :> IMenuTerminalElement

  static member menuBar(children: ITerminalElement list) =
    let viewProps = MenuBarProps ()
    viewProps.Children children
    new MenuBarTerminalElement(viewProps.props)
    :> IMenuTerminalElement

  static member numericUpDown<'T>(set: NumericUpDownProps<'T> -> unit) =
    let viewProps = NumericUpDownProps<'T> ()

    set viewProps
    new NumericUpDownTerminalElement<'T>(viewProps.props)
    :> IViewTerminalElement

  static member numericUpDown<'T>(children: ITerminalElement list) =
    let viewProps = NumericUpDownProps<'T> ()

    viewProps.Children children
    new NumericUpDownTerminalElement<'T>(viewProps.props)
    :> IViewTerminalElement

  static member padding(set: PaddingProps -> unit) =
    let viewProps = PaddingProps ()
    set viewProps
    new PaddingTerminalElement(viewProps.props)
    :> IViewTerminalElement

  static member padding(children: ITerminalElement list) =
    let viewProps = PaddingProps ()
    viewProps.Children children
    new PaddingTerminalElement(viewProps.props)
    :> IViewTerminalElement

  static member popoverMenu(set: PopoverMenuProps -> unit) =
    let viewProps = PopoverMenuProps ()
    set viewProps
    new PopoverMenuTerminalElement(viewProps.props)
    :> IPopoverMenuTerminalElement

  static member popoverMenu(children: ITerminalElement list) =
    let viewProps = PopoverMenuProps ()
    viewProps.Children children
    new PopoverMenuTerminalElement(viewProps.props)
    :> IPopoverMenuTerminalElement

  static member progressBar(set: ProgressBarProps -> unit) =
    let viewProps = ProgressBarProps ()
    set viewProps
    new ProgressBarTerminalElement(viewProps.props)
    :> IViewTerminalElement

  static member progressBar(children: ITerminalElement list) =
    let viewProps = ProgressBarProps ()
    viewProps.Children children
    new ProgressBarTerminalElement(viewProps.props)
    :> IViewTerminalElement

  static member runnable(set: RunnableProps -> unit) =
    let viewProps = RunnableProps ()
    set viewProps
    new RunnableTerminalElement(viewProps.props)
    :> IViewTerminalElement

  static member runnable(children: ITerminalElement list) =
    let viewProps = RunnableProps ()
    viewProps.Children children
    new RunnableTerminalElement(viewProps.props)
    :> IViewTerminalElement

  static member runnable<'TResult>(set: RunnableProps<'TResult> -> unit) =
    let viewProps = RunnableProps<'TResult> ()

    set viewProps
    new RunnableTerminalElement<'TResult>(viewProps.props)
    :> IViewTerminalElement

  static member runnable<'TResult>(children: ITerminalElement list) =
    let viewProps = RunnableProps<'TResult> ()

    viewProps.Children children
    new RunnableTerminalElement<'TResult>(viewProps.props)
    :> IViewTerminalElement

  static member scrollBar(set: ScrollBarProps -> unit) =
    let viewProps = ScrollBarProps ()
    set viewProps
    new ScrollBarTerminalElement(viewProps.props)
    :> IViewTerminalElement

  static member scrollBar(children: ITerminalElement list) =
    let viewProps = ScrollBarProps ()
    viewProps.Children children
    new ScrollBarTerminalElement(viewProps.props)
    :> IViewTerminalElement

  static member scrollSlider(set: ScrollSliderProps -> unit) =
    let viewProps = ScrollSliderProps ()
    set viewProps
    new ScrollSliderTerminalElement(viewProps.props)
    :> IViewTerminalElement

  static member scrollSlider(children: ITerminalElement list) =
    let viewProps = ScrollSliderProps ()
    viewProps.Children children
    new ScrollSliderTerminalElement(viewProps.props)
    :> IViewTerminalElement

  static member flagSelector(set: FlagSelectorProps -> unit) =
    let viewProps = FlagSelectorProps ()
    set viewProps
    new FlagSelectorTerminalElement(viewProps.props)
    :> IViewTerminalElement

  static member flagSelector(children: ITerminalElement list) =
    let viewProps = FlagSelectorProps ()
    viewProps.Children children
    new FlagSelectorTerminalElement(viewProps.props)
    :> IViewTerminalElement

  static member optionSelector(set: OptionSelectorProps -> unit) =
    let viewProps = OptionSelectorProps ()
    set viewProps
    new OptionSelectorTerminalElement(viewProps.props)
    :> IViewTerminalElement

  static member optionSelector(children: ITerminalElement list) =
    let viewProps = OptionSelectorProps ()
    viewProps.Children children
    new OptionSelectorTerminalElement(viewProps.props)
    :> IViewTerminalElement

  static member flagSelector<'TFlagsEnum when 'TFlagsEnum: struct and 'TFlagsEnum: (new: unit -> 'TFlagsEnum) and 'TFlagsEnum:> Enum and 'TFlagsEnum:> ValueType>(set: FlagSelectorProps<'TFlagsEnum> -> unit) =
    let viewProps = FlagSelectorProps<'TFlagsEnum> ()

    set viewProps
    new FlagSelectorTerminalElement<'TFlagsEnum>(viewProps.props)
    :> IViewTerminalElement

  static member flagSelector<'TFlagsEnum when 'TFlagsEnum: struct and 'TFlagsEnum: (new: unit -> 'TFlagsEnum) and 'TFlagsEnum:> Enum and 'TFlagsEnum:> ValueType>(children: ITerminalElement list) =
    let viewProps = FlagSelectorProps<'TFlagsEnum> ()

    viewProps.Children children
    new FlagSelectorTerminalElement<'TFlagsEnum>(viewProps.props)
    :> IViewTerminalElement

  static member optionSelector<'TEnum when 'TEnum: struct and 'TEnum: (new: unit -> 'TEnum) and 'TEnum:> Enum and 'TEnum:> ValueType>(set: OptionSelectorProps<'TEnum> -> unit) =
    let viewProps = OptionSelectorProps<'TEnum> ()

    set viewProps
    new OptionSelectorTerminalElement<'TEnum>(viewProps.props)
    :> IViewTerminalElement

  static member optionSelector<'TEnum when 'TEnum: struct and 'TEnum: (new: unit -> 'TEnum) and 'TEnum:> Enum and 'TEnum:> ValueType>(children: ITerminalElement list) =
    let viewProps = OptionSelectorProps<'TEnum> ()

    viewProps.Children children
    new OptionSelectorTerminalElement<'TEnum>(viewProps.props)
    :> IViewTerminalElement

  static member shortcut(set: ShortcutProps -> unit) =
    let viewProps = ShortcutProps ()
    set viewProps
    new ShortcutTerminalElement(viewProps.props)
    :> IViewTerminalElement

  static member shortcut(children: ITerminalElement list) =
    let viewProps = ShortcutProps ()
    viewProps.Children children
    new ShortcutTerminalElement(viewProps.props)
    :> IViewTerminalElement

  static member menuItem(set: MenuItemProps -> unit) =
    let viewProps = MenuItemProps ()
    set viewProps
    new MenuItemTerminalElement(viewProps.props)
    :> IMenuItemTerminalElement

  static member menuItem(children: ITerminalElement list) =
    let viewProps = MenuItemProps ()
    viewProps.Children children
    new MenuItemTerminalElement(viewProps.props)
    :> IMenuItemTerminalElement

  static member menuBarItem(set: MenuBarItemProps -> menuBarItemMacros -> unit) =
    let props = MenuBarItemProps ()
    let macros = menuBarItemMacros props
    set props macros
    new MenuBarItemTerminalElement(props.props)
    :> IMenuItemTerminalElement

  static member menuBarItem(children: ITerminalElement list) =
    let viewProps = MenuBarItemProps ()
    viewProps.Children children
    new MenuBarItemTerminalElement(viewProps.props)
    :> IMenuItemTerminalElement

  static member slider<'T>(set: SliderProps<'T> -> unit) =
    let viewProps = SliderProps<'T> ()

    set viewProps
    new SliderTerminalElement<'T>(viewProps.props)
    :> IViewTerminalElement

  static member slider<'T>(children: ITerminalElement list) =
    let viewProps = SliderProps<'T> ()

    viewProps.Children children
    new SliderTerminalElement<'T>(viewProps.props)
    :> IViewTerminalElement

  static member spinnerView(set: SpinnerViewProps -> unit) =
    let viewProps = SpinnerViewProps ()
    set viewProps
    new SpinnerViewTerminalElement(viewProps.props)
    :> IViewTerminalElement

  static member spinnerView(children: ITerminalElement list) =
    let viewProps = SpinnerViewProps ()
    viewProps.Children children
    new SpinnerViewTerminalElement(viewProps.props)
    :> IViewTerminalElement

  static member statusBar(set: StatusBarProps -> unit) =
    let viewProps = StatusBarProps ()
    set viewProps
    new StatusBarTerminalElement(viewProps.props)
    :> IViewTerminalElement

  static member statusBar(children: ITerminalElement list) =
    let viewProps = StatusBarProps ()
    viewProps.Children children
    new StatusBarTerminalElement(viewProps.props)
    :> IViewTerminalElement

  static member tab(set: TabProps -> unit) =
    let viewProps = TabProps ()
    set viewProps
    new TabTerminalElement(viewProps.props)
    :> ITabTerminalElement

  static member tab(children: ITerminalElement list) =
    let viewProps = TabProps ()
    viewProps.Children children
    new TabTerminalElement(viewProps.props)
    :> ITabTerminalElement

  static member tabView(set: TabViewProps -> unit) =
    let viewProps = TabViewProps ()
    set viewProps
    new TabViewTerminalElement(viewProps.props)
    :> IViewTerminalElement

  static member tabView(children: ITerminalElement list) =
    let viewProps = TabViewProps ()
    viewProps.Children children
    new TabViewTerminalElement(viewProps.props)
    :> IViewTerminalElement

  static member tableView(set: TableViewProps -> unit) =
    let viewProps = TableViewProps ()
    set viewProps
    new TableViewTerminalElement(viewProps.props)
    :> IViewTerminalElement

  static member tableView(children: ITerminalElement list) =
    let viewProps = TableViewProps ()
    viewProps.Children children
    new TableViewTerminalElement(viewProps.props)
    :> IViewTerminalElement

  static member textField(set: TextFieldProps -> unit) =
    let viewProps = TextFieldProps ()
    set viewProps
    new TextFieldTerminalElement(viewProps.props)
    :> IViewTerminalElement

  static member textField(children: ITerminalElement list) =
    let viewProps = TextFieldProps ()
    viewProps.Children children
    new TextFieldTerminalElement(viewProps.props)
    :> IViewTerminalElement

  static member dateField(set: DateFieldProps -> unit) =
    let viewProps = DateFieldProps ()
    set viewProps
    new DateFieldTerminalElement(viewProps.props)
    :> IViewTerminalElement

  static member dateField(children: ITerminalElement list) =
    let viewProps = DateFieldProps ()
    viewProps.Children children
    new DateFieldTerminalElement(viewProps.props)
    :> IViewTerminalElement

  static member textValidateField(set: TextValidateFieldProps -> unit) =
    let viewProps = TextValidateFieldProps ()
    set viewProps
    new TextValidateFieldTerminalElement(viewProps.props)
    :> IViewTerminalElement

  static member textValidateField(children: ITerminalElement list) =
    let viewProps = TextValidateFieldProps ()
    viewProps.Children children
    new TextValidateFieldTerminalElement(viewProps.props)
    :> IViewTerminalElement

  static member textView(set: TextViewProps -> unit) =
    let viewProps = TextViewProps ()
    set viewProps
    new TextViewTerminalElement(viewProps.props)
    :> IViewTerminalElement

  static member textView(children: ITerminalElement list) =
    let viewProps = TextViewProps ()
    viewProps.Children children
    new TextViewTerminalElement(viewProps.props)
    :> IViewTerminalElement

  static member timeField(set: TimeFieldProps -> unit) =
    let viewProps = TimeFieldProps ()
    set viewProps
    new TimeFieldTerminalElement(viewProps.props)
    :> IViewTerminalElement

  static member timeField(children: ITerminalElement list) =
    let viewProps = TimeFieldProps ()
    viewProps.Children children
    new TimeFieldTerminalElement(viewProps.props)
    :> IViewTerminalElement

  static member treeView<'T when 'T: not struct>(set: TreeViewProps<'T> -> unit) =
    let viewProps = TreeViewProps<'T> ()

    set viewProps
    new TreeViewTerminalElement<'T>(viewProps.props)
    :> IViewTerminalElement

  static member treeView<'T when 'T: not struct>(children: ITerminalElement list) =
    let viewProps = TreeViewProps<'T> ()

    viewProps.Children children
    new TreeViewTerminalElement<'T>(viewProps.props)
    :> IViewTerminalElement

  static member window(set: WindowProps -> unit) =
    let viewProps = WindowProps ()
    set viewProps
    new WindowTerminalElement(viewProps.props)
    :> IViewTerminalElement

  static member window(children: ITerminalElement list) =
    let viewProps = WindowProps ()
    viewProps.Children children
    new WindowTerminalElement(viewProps.props)
    :> IViewTerminalElement

  static member dialog(set: DialogProps -> unit) =
    let viewProps = DialogProps ()
    set viewProps
    new DialogTerminalElement(viewProps.props)
    :> IViewTerminalElement

  static member dialog(children: ITerminalElement list) =
    let viewProps = DialogProps ()
    viewProps.Children children
    new DialogTerminalElement(viewProps.props)
    :> IViewTerminalElement

  static member fileDialog(set: FileDialogProps -> unit) =
    let viewProps = FileDialogProps ()
    set viewProps
    new FileDialogTerminalElement(viewProps.props)
    :> IViewTerminalElement

  static member fileDialog(children: ITerminalElement list) =
    let viewProps = FileDialogProps ()
    viewProps.Children children
    new FileDialogTerminalElement(viewProps.props)
    :> IViewTerminalElement

  static member openDialog(set: OpenDialogProps -> unit) =
    let viewProps = OpenDialogProps ()
    set viewProps
    new OpenDialogTerminalElement(viewProps.props)
    :> IViewTerminalElement

  static member openDialog(children: ITerminalElement list) =
    let viewProps = OpenDialogProps ()
    viewProps.Children children
    new OpenDialogTerminalElement(viewProps.props)
    :> IViewTerminalElement

  static member saveDialog(set: SaveDialogProps -> unit) =
    let viewProps = SaveDialogProps ()
    set viewProps
    new SaveDialogTerminalElement(viewProps.props)
    :> IViewTerminalElement

  static member saveDialog(children: ITerminalElement list) =
    let viewProps = SaveDialogProps ()
    viewProps.Children children
    new SaveDialogTerminalElement(viewProps.props)
    :> IViewTerminalElement

  static member wizard(set: WizardProps -> unit) =
    let viewProps = WizardProps ()
    set viewProps
    new WizardTerminalElement(viewProps.props)
    :> IViewTerminalElement

  static member wizard(children: ITerminalElement list) =
    let viewProps = WizardProps ()
    viewProps.Children children
    new WizardTerminalElement(viewProps.props)
    :> IViewTerminalElement

  static member wizardStep(set: WizardStepProps -> unit) =
    let viewProps = WizardStepProps ()
    set viewProps
    new WizardStepTerminalElement(viewProps.props)
    :> IWizardStepTerminalElement

  static member wizardStep(children: ITerminalElement list) =
    let viewProps = WizardStepProps ()
    viewProps.Children children
    new WizardStepTerminalElement(viewProps.props)
    :> IWizardStepTerminalElement
