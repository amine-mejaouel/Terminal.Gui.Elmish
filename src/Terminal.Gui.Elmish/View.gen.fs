namespace Terminal.Gui.Elmish

open System
open Terminal.Gui.Elmish

type View =

  static member Adornment(set: AdornmentProps -> unit) =
    let viewProps = AdornmentProps ()
    set viewProps
    new AdornmentTerminalElement(viewProps.props)
    :> ITerminalElement

  static member Adornment(children: ITerminalElement list) =
    let viewProps = AdornmentProps ()
    viewProps.Children children
    new AdornmentTerminalElement(viewProps.props)
    :> ITerminalElement

  static member AttributePicker(set: AttributePickerProps -> unit) =
    let viewProps = AttributePickerProps ()
    set viewProps
    new AttributePickerTerminalElement(viewProps.props)
    :> ITerminalElement

  static member AttributePicker(children: ITerminalElement list) =
    let viewProps = AttributePickerProps ()
    viewProps.Children children
    new AttributePickerTerminalElement(viewProps.props)
    :> ITerminalElement

  static member Bar(set: BarProps -> unit) =
    let viewProps = BarProps ()
    set viewProps
    new BarTerminalElement(viewProps.props)
    :> ITerminalElement

  static member Bar(children: ITerminalElement list) =
    let viewProps = BarProps ()
    viewProps.Children children
    new BarTerminalElement(viewProps.props)
    :> ITerminalElement

  static member Border(set: BorderProps -> unit) =
    let viewProps = BorderProps ()
    set viewProps
    new BorderTerminalElement(viewProps.props)
    :> ITerminalElement

  static member Border(children: ITerminalElement list) =
    let viewProps = BorderProps ()
    viewProps.Children children
    new BorderTerminalElement(viewProps.props)
    :> ITerminalElement

  static member Button(set: ButtonProps -> unit) =
    let viewProps = ButtonProps ()
    set viewProps
    new ButtonTerminalElement(viewProps.props)
    :> ITerminalElement

  static member Button(children: ITerminalElement list) =
    let viewProps = ButtonProps ()
    viewProps.Children children
    new ButtonTerminalElement(viewProps.props)
    :> ITerminalElement

  static member CharMap(set: CharMapProps -> unit) =
    let viewProps = CharMapProps ()
    set viewProps
    new CharMapTerminalElement(viewProps.props)
    :> ITerminalElement

  static member CharMap(children: ITerminalElement list) =
    let viewProps = CharMapProps ()
    viewProps.Children children
    new CharMapTerminalElement(viewProps.props)
    :> ITerminalElement

  static member CheckBox(set: CheckBoxProps -> unit) =
    let viewProps = CheckBoxProps ()
    set viewProps
    new CheckBoxTerminalElement(viewProps.props)
    :> ITerminalElement

  static member CheckBox(children: ITerminalElement list) =
    let viewProps = CheckBoxProps ()
    viewProps.Children children
    new CheckBoxTerminalElement(viewProps.props)
    :> ITerminalElement

  static member ColorPicker(set: ColorPickerProps -> unit) =
    let viewProps = ColorPickerProps ()
    set viewProps
    new ColorPickerTerminalElement(viewProps.props)
    :> ITerminalElement

  static member ColorPicker(children: ITerminalElement list) =
    let viewProps = ColorPickerProps ()
    viewProps.Children children
    new ColorPickerTerminalElement(viewProps.props)
    :> ITerminalElement

  static member ColorPicker16(set: ColorPicker16Props -> unit) =
    let viewProps = ColorPicker16Props ()
    set viewProps
    new ColorPicker16TerminalElement(viewProps.props)
    :> ITerminalElement

  static member ColorPicker16(children: ITerminalElement list) =
    let viewProps = ColorPicker16Props ()
    viewProps.Children children
    new ColorPicker16TerminalElement(viewProps.props)
    :> ITerminalElement

  static member ComboBox(set: ComboBoxProps -> unit) =
    let viewProps = ComboBoxProps ()
    set viewProps
    new ComboBoxTerminalElement(viewProps.props)
    :> ITerminalElement

  static member ComboBox(children: ITerminalElement list) =
    let viewProps = ComboBoxProps ()
    viewProps.Children children
    new ComboBoxTerminalElement(viewProps.props)
    :> ITerminalElement

  static member DatePicker(set: DatePickerProps -> unit) =
    let viewProps = DatePickerProps ()
    set viewProps
    new DatePickerTerminalElement(viewProps.props)
    :> ITerminalElement

  static member DatePicker(children: ITerminalElement list) =
    let viewProps = DatePickerProps ()
    viewProps.Children children
    new DatePickerTerminalElement(viewProps.props)
    :> ITerminalElement

  static member FrameView(set: FrameViewProps -> unit) =
    let viewProps = FrameViewProps ()
    set viewProps
    new FrameViewTerminalElement(viewProps.props)
    :> ITerminalElement

  static member FrameView(children: ITerminalElement list) =
    let viewProps = FrameViewProps ()
    viewProps.Children children
    new FrameViewTerminalElement(viewProps.props)
    :> ITerminalElement

  static member GraphView(set: GraphViewProps -> unit) =
    let viewProps = GraphViewProps ()
    set viewProps
    new GraphViewTerminalElement(viewProps.props)
    :> ITerminalElement

  static member GraphView(children: ITerminalElement list) =
    let viewProps = GraphViewProps ()
    viewProps.Children children
    new GraphViewTerminalElement(viewProps.props)
    :> ITerminalElement

  static member HexView(set: HexViewProps -> unit) =
    let viewProps = HexViewProps ()
    set viewProps
    new HexViewTerminalElement(viewProps.props)
    :> ITerminalElement

  static member HexView(children: ITerminalElement list) =
    let viewProps = HexViewProps ()
    viewProps.Children children
    new HexViewTerminalElement(viewProps.props)
    :> ITerminalElement

  static member Label(set: LabelProps -> unit) =
    let viewProps = LabelProps ()
    set viewProps
    new LabelTerminalElement(viewProps.props)
    :> ITerminalElement

  static member Label(children: ITerminalElement list) =
    let viewProps = LabelProps ()
    viewProps.Children children
    new LabelTerminalElement(viewProps.props)
    :> ITerminalElement

  static member LegendAnnotation(set: LegendAnnotationProps -> unit) =
    let viewProps = LegendAnnotationProps ()
    set viewProps
    new LegendAnnotationTerminalElement(viewProps.props)
    :> ITerminalElement

  static member LegendAnnotation(children: ITerminalElement list) =
    let viewProps = LegendAnnotationProps ()
    viewProps.Children children
    new LegendAnnotationTerminalElement(viewProps.props)
    :> ITerminalElement

  static member Line(set: LineProps -> unit) =
    let viewProps = LineProps ()
    set viewProps
    new LineTerminalElement(viewProps.props)
    :> ITerminalElement

  static member Line(children: ITerminalElement list) =
    let viewProps = LineProps ()
    viewProps.Children children
    new LineTerminalElement(viewProps.props)
    :> ITerminalElement

  static member LinearRange<'T>(set: LinearRangeProps<'T> -> unit) =
    let viewProps = LinearRangeProps<'T> ()
    set viewProps
    new LinearRangeTerminalElement<'T>(viewProps.props)
    :> ITerminalElement

  static member LinearRange<'T>(children: ITerminalElement list) =
    let viewProps = LinearRangeProps<'T> ()
    viewProps.Children children
    new LinearRangeTerminalElement<'T>(viewProps.props)
    :> ITerminalElement

  static member LinearRange(set: LinearRangeProps -> unit) =
    let viewProps = LinearRangeProps ()
    set viewProps
    new LinearRangeTerminalElement(viewProps.props)
    :> ITerminalElement

  static member LinearRange(children: ITerminalElement list) =
    let viewProps = LinearRangeProps ()
    viewProps.Children children
    new LinearRangeTerminalElement(viewProps.props)
    :> ITerminalElement

  static member ListView(set: ListViewProps -> unit) =
    let viewProps = ListViewProps ()
    set viewProps
    new ListViewTerminalElement(viewProps.props)
    :> ITerminalElement

  static member ListView(children: ITerminalElement list) =
    let viewProps = ListViewProps ()
    viewProps.Children children
    new ListViewTerminalElement(viewProps.props)
    :> ITerminalElement

  static member Margin(set: MarginProps -> unit) =
    let viewProps = MarginProps ()
    set viewProps
    new MarginTerminalElement(viewProps.props)
    :> ITerminalElement

  static member Margin(children: ITerminalElement list) =
    let viewProps = MarginProps ()
    viewProps.Children children
    new MarginTerminalElement(viewProps.props)
    :> ITerminalElement

  static member Menu(set: MenuProps -> unit) =
    let viewProps = MenuProps ()
    set viewProps
    new MenuTerminalElement(viewProps.props)
    :> IMenuTerminalElement

  static member Menu(children: ITerminalElement list) =
    let viewProps = MenuProps ()
    viewProps.Children children
    new MenuTerminalElement(viewProps.props)
    :> IMenuTerminalElement

  static member MenuBar(set: MenuBarProps -> MenuBarMacros -> unit) =
    let props = MenuBarProps ()
    let macros = MenuBarMacros props
    set props macros
    new MenuBarTerminalElement(props.props)
    :> IMenuTerminalElement

  static member MenuBar(set: MenuBarProps -> unit) =
    let viewProps = MenuBarProps ()
    set viewProps
    new MenuBarTerminalElement(viewProps.props)
    :> IMenuTerminalElement

  static member MenuBar(children: ITerminalElement list) =
    let viewProps = MenuBarProps ()
    viewProps.Children children
    new MenuBarTerminalElement(viewProps.props)
    :> IMenuTerminalElement

  static member NumericUpDown<'T>(set: NumericUpDownProps<'T> -> unit) =
    let viewProps = NumericUpDownProps<'T> ()
    set viewProps
    new NumericUpDownTerminalElement<'T>(viewProps.props)
    :> ITerminalElement

  static member NumericUpDown<'T>(children: ITerminalElement list) =
    let viewProps = NumericUpDownProps<'T> ()
    viewProps.Children children
    new NumericUpDownTerminalElement<'T>(viewProps.props)
    :> ITerminalElement

  static member NumericUpDown(set: NumericUpDownProps -> unit) =
    let viewProps = NumericUpDownProps ()
    set viewProps
    new NumericUpDownTerminalElement(viewProps.props)
    :> ITerminalElement

  static member NumericUpDown(children: ITerminalElement list) =
    let viewProps = NumericUpDownProps ()
    viewProps.Children children
    new NumericUpDownTerminalElement(viewProps.props)
    :> ITerminalElement

  static member Padding(set: PaddingProps -> unit) =
    let viewProps = PaddingProps ()
    set viewProps
    new PaddingTerminalElement(viewProps.props)
    :> ITerminalElement

  static member Padding(children: ITerminalElement list) =
    let viewProps = PaddingProps ()
    viewProps.Children children
    new PaddingTerminalElement(viewProps.props)
    :> ITerminalElement

  static member PopoverMenu(set: PopoverMenuProps -> unit) =
    let viewProps = PopoverMenuProps ()
    set viewProps
    new PopoverMenuTerminalElement(viewProps.props)
    :> IPopoverMenuTerminalElement

  static member PopoverMenu(children: ITerminalElement list) =
    let viewProps = PopoverMenuProps ()
    viewProps.Children children
    new PopoverMenuTerminalElement(viewProps.props)
    :> IPopoverMenuTerminalElement

  static member ProgressBar(set: ProgressBarProps -> unit) =
    let viewProps = ProgressBarProps ()
    set viewProps
    new ProgressBarTerminalElement(viewProps.props)
    :> ITerminalElement

  static member ProgressBar(children: ITerminalElement list) =
    let viewProps = ProgressBarProps ()
    viewProps.Children children
    new ProgressBarTerminalElement(viewProps.props)
    :> ITerminalElement

  static member Runnable(set: RunnableProps -> unit) =
    let viewProps = RunnableProps ()
    set viewProps
    new RunnableTerminalElement(viewProps.props)
    :> ITerminalElement

  static member Runnable(children: ITerminalElement list) =
    let viewProps = RunnableProps ()
    viewProps.Children children
    new RunnableTerminalElement(viewProps.props)
    :> ITerminalElement

  static member Runnable<'TResult>(set: RunnableProps<'TResult> -> unit) =
    let viewProps = RunnableProps<'TResult> ()
    set viewProps
    new RunnableTerminalElement<'TResult>(viewProps.props)
    :> ITerminalElement

  static member Runnable<'TResult>(children: ITerminalElement list) =
    let viewProps = RunnableProps<'TResult> ()
    viewProps.Children children
    new RunnableTerminalElement<'TResult>(viewProps.props)
    :> ITerminalElement

  static member Dialog<'TResult>(set: DialogProps<'TResult> -> unit) =
    let viewProps = DialogProps<'TResult> ()
    set viewProps
    new DialogTerminalElement<'TResult>(viewProps.props)
    :> ITerminalElement

  static member Dialog<'TResult>(children: ITerminalElement list) =
    let viewProps = DialogProps<'TResult> ()
    viewProps.Children children
    new DialogTerminalElement<'TResult>(viewProps.props)
    :> ITerminalElement

  static member Dialog(set: DialogProps -> unit) =
    let viewProps = DialogProps ()
    set viewProps
    new DialogTerminalElement(viewProps.props)
    :> ITerminalElement

  static member Dialog(children: ITerminalElement list) =
    let viewProps = DialogProps ()
    viewProps.Children children
    new DialogTerminalElement(viewProps.props)
    :> ITerminalElement

  static member Prompt<'TView, 'TResult when 'TView: (new: unit -> 'TView) and 'TView:> Terminal.Gui.ViewBase.View>(set: PromptProps<'TView, 'TResult> -> unit) =
    let viewProps = PromptProps<'TView, 'TResult> ()
    set viewProps
    new PromptTerminalElement<'TView, 'TResult>(viewProps.props)
    :> ITerminalElement

  static member Prompt<'TView, 'TResult when 'TView: (new: unit -> 'TView) and 'TView:> Terminal.Gui.ViewBase.View>(children: ITerminalElement list) =
    let viewProps = PromptProps<'TView, 'TResult> ()
    viewProps.Children children
    new PromptTerminalElement<'TView, 'TResult>(viewProps.props)
    :> ITerminalElement

  static member FileDialog(set: FileDialogProps -> unit) =
    let viewProps = FileDialogProps ()
    set viewProps
    new FileDialogTerminalElement(viewProps.props)
    :> ITerminalElement

  static member FileDialog(children: ITerminalElement list) =
    let viewProps = FileDialogProps ()
    viewProps.Children children
    new FileDialogTerminalElement(viewProps.props)
    :> ITerminalElement

  static member OpenDialog(set: OpenDialogProps -> unit) =
    let viewProps = OpenDialogProps ()
    set viewProps
    new OpenDialogTerminalElement(viewProps.props)
    :> ITerminalElement

  static member OpenDialog(children: ITerminalElement list) =
    let viewProps = OpenDialogProps ()
    viewProps.Children children
    new OpenDialogTerminalElement(viewProps.props)
    :> ITerminalElement

  static member SaveDialog(set: SaveDialogProps -> unit) =
    let viewProps = SaveDialogProps ()
    set viewProps
    new SaveDialogTerminalElement(viewProps.props)
    :> ITerminalElement

  static member SaveDialog(children: ITerminalElement list) =
    let viewProps = SaveDialogProps ()
    viewProps.Children children
    new SaveDialogTerminalElement(viewProps.props)
    :> ITerminalElement

  static member ScrollBar(set: ScrollBarProps -> unit) =
    let viewProps = ScrollBarProps ()
    set viewProps
    new ScrollBarTerminalElement(viewProps.props)
    :> ITerminalElement

  static member ScrollBar(children: ITerminalElement list) =
    let viewProps = ScrollBarProps ()
    viewProps.Children children
    new ScrollBarTerminalElement(viewProps.props)
    :> ITerminalElement

  static member ScrollSlider(set: ScrollSliderProps -> unit) =
    let viewProps = ScrollSliderProps ()
    set viewProps
    new ScrollSliderTerminalElement(viewProps.props)
    :> ITerminalElement

  static member ScrollSlider(children: ITerminalElement list) =
    let viewProps = ScrollSliderProps ()
    viewProps.Children children
    new ScrollSliderTerminalElement(viewProps.props)
    :> ITerminalElement

  static member FlagSelector(set: FlagSelectorProps -> unit) =
    let viewProps = FlagSelectorProps ()
    set viewProps
    new FlagSelectorTerminalElement(viewProps.props)
    :> ITerminalElement

  static member FlagSelector(children: ITerminalElement list) =
    let viewProps = FlagSelectorProps ()
    viewProps.Children children
    new FlagSelectorTerminalElement(viewProps.props)
    :> ITerminalElement

  static member OptionSelector(set: OptionSelectorProps -> unit) =
    let viewProps = OptionSelectorProps ()
    set viewProps
    new OptionSelectorTerminalElement(viewProps.props)
    :> ITerminalElement

  static member OptionSelector(children: ITerminalElement list) =
    let viewProps = OptionSelectorProps ()
    viewProps.Children children
    new OptionSelectorTerminalElement(viewProps.props)
    :> ITerminalElement

  static member FlagSelector<'TFlagsEnum when 'TFlagsEnum: struct and 'TFlagsEnum: (new: unit -> 'TFlagsEnum) and 'TFlagsEnum:> System.Enum and 'TFlagsEnum:> System.ValueType>(set: FlagSelectorProps<'TFlagsEnum> -> unit) =
    let viewProps = FlagSelectorProps<'TFlagsEnum> ()
    set viewProps
    new FlagSelectorTerminalElement<'TFlagsEnum>(viewProps.props)
    :> ITerminalElement

  static member FlagSelector<'TFlagsEnum when 'TFlagsEnum: struct and 'TFlagsEnum: (new: unit -> 'TFlagsEnum) and 'TFlagsEnum:> System.Enum and 'TFlagsEnum:> System.ValueType>(children: ITerminalElement list) =
    let viewProps = FlagSelectorProps<'TFlagsEnum> ()
    viewProps.Children children
    new FlagSelectorTerminalElement<'TFlagsEnum>(viewProps.props)
    :> ITerminalElement

  static member OptionSelector<'TEnum when 'TEnum: struct and 'TEnum: (new: unit -> 'TEnum) and 'TEnum:> System.Enum and 'TEnum:> System.ValueType>(set: OptionSelectorProps<'TEnum> -> unit) =
    let viewProps = OptionSelectorProps<'TEnum> ()
    set viewProps
    new OptionSelectorTerminalElement<'TEnum>(viewProps.props)
    :> ITerminalElement

  static member OptionSelector<'TEnum when 'TEnum: struct and 'TEnum: (new: unit -> 'TEnum) and 'TEnum:> System.Enum and 'TEnum:> System.ValueType>(children: ITerminalElement list) =
    let viewProps = OptionSelectorProps<'TEnum> ()
    viewProps.Children children
    new OptionSelectorTerminalElement<'TEnum>(viewProps.props)
    :> ITerminalElement

  static member Shortcut(set: ShortcutProps -> unit) =
    let viewProps = ShortcutProps ()
    set viewProps
    new ShortcutTerminalElement(viewProps.props)
    :> ITerminalElement

  static member Shortcut(children: ITerminalElement list) =
    let viewProps = ShortcutProps ()
    viewProps.Children children
    new ShortcutTerminalElement(viewProps.props)
    :> ITerminalElement

  static member MenuItem(set: MenuItemProps -> unit) =
    let viewProps = MenuItemProps ()
    set viewProps
    new MenuItemTerminalElement(viewProps.props)
    :> IMenuItemTerminalElement

  static member MenuItem(children: ITerminalElement list) =
    let viewProps = MenuItemProps ()
    viewProps.Children children
    new MenuItemTerminalElement(viewProps.props)
    :> IMenuItemTerminalElement

  static member MenuBarItem(set: MenuBarItemProps -> MenuBarItemMacros -> unit) =
    let props = MenuBarItemProps ()
    let macros = MenuBarItemMacros props
    set props macros
    new MenuBarItemTerminalElement(props.props)
    :> IMenuItemTerminalElement

  static member MenuBarItem(set: MenuBarItemProps -> unit) =
    let viewProps = MenuBarItemProps ()
    set viewProps
    new MenuBarItemTerminalElement(viewProps.props)
    :> IMenuItemTerminalElement

  static member MenuBarItem(children: ITerminalElement list) =
    let viewProps = MenuBarItemProps ()
    viewProps.Children children
    new MenuBarItemTerminalElement(viewProps.props)
    :> IMenuItemTerminalElement

  static member SpinnerView(set: SpinnerViewProps -> unit) =
    let viewProps = SpinnerViewProps ()
    set viewProps
    new SpinnerViewTerminalElement(viewProps.props)
    :> ITerminalElement

  static member SpinnerView(children: ITerminalElement list) =
    let viewProps = SpinnerViewProps ()
    viewProps.Children children
    new SpinnerViewTerminalElement(viewProps.props)
    :> ITerminalElement

  static member StatusBar(set: StatusBarProps -> unit) =
    let viewProps = StatusBarProps ()
    set viewProps
    new StatusBarTerminalElement(viewProps.props)
    :> ITerminalElement

  static member StatusBar(children: ITerminalElement list) =
    let viewProps = StatusBarProps ()
    viewProps.Children children
    new StatusBarTerminalElement(viewProps.props)
    :> ITerminalElement

  static member Tab(set: TabProps -> unit) =
    let viewProps = TabProps ()
    set viewProps
    new TabTerminalElement(viewProps.props)
    :> ITabTerminalElement

  static member Tab(children: ITerminalElement list) =
    let viewProps = TabProps ()
    viewProps.Children children
    new TabTerminalElement(viewProps.props)
    :> ITabTerminalElement

  static member TabView(set: TabViewProps -> unit) =
    let viewProps = TabViewProps ()
    set viewProps
    new TabViewTerminalElement(viewProps.props)
    :> ITerminalElement

  static member TabView(children: ITerminalElement list) =
    let viewProps = TabViewProps ()
    viewProps.Children children
    new TabViewTerminalElement(viewProps.props)
    :> ITerminalElement

  static member TableView(set: TableViewProps -> unit) =
    let viewProps = TableViewProps ()
    set viewProps
    new TableViewTerminalElement(viewProps.props)
    :> ITerminalElement

  static member TableView(children: ITerminalElement list) =
    let viewProps = TableViewProps ()
    viewProps.Children children
    new TableViewTerminalElement(viewProps.props)
    :> ITerminalElement

  static member TextField(set: TextFieldProps -> unit) =
    let viewProps = TextFieldProps ()
    set viewProps
    new TextFieldTerminalElement(viewProps.props)
    :> ITerminalElement

  static member TextField(children: ITerminalElement list) =
    let viewProps = TextFieldProps ()
    viewProps.Children children
    new TextFieldTerminalElement(viewProps.props)
    :> ITerminalElement

  static member DateField(set: DateFieldProps -> unit) =
    let viewProps = DateFieldProps ()
    set viewProps
    new DateFieldTerminalElement(viewProps.props)
    :> ITerminalElement

  static member DateField(children: ITerminalElement list) =
    let viewProps = DateFieldProps ()
    viewProps.Children children
    new DateFieldTerminalElement(viewProps.props)
    :> ITerminalElement

  static member TextValidateField(set: TextValidateFieldProps -> unit) =
    let viewProps = TextValidateFieldProps ()
    set viewProps
    new TextValidateFieldTerminalElement(viewProps.props)
    :> ITerminalElement

  static member TextValidateField(children: ITerminalElement list) =
    let viewProps = TextValidateFieldProps ()
    viewProps.Children children
    new TextValidateFieldTerminalElement(viewProps.props)
    :> ITerminalElement

  static member TextView(set: TextViewProps -> unit) =
    let viewProps = TextViewProps ()
    set viewProps
    new TextViewTerminalElement(viewProps.props)
    :> ITerminalElement

  static member TextView(children: ITerminalElement list) =
    let viewProps = TextViewProps ()
    viewProps.Children children
    new TextViewTerminalElement(viewProps.props)
    :> ITerminalElement

  static member TimeField(set: TimeFieldProps -> unit) =
    let viewProps = TimeFieldProps ()
    set viewProps
    new TimeFieldTerminalElement(viewProps.props)
    :> ITerminalElement

  static member TimeField(children: ITerminalElement list) =
    let viewProps = TimeFieldProps ()
    viewProps.Children children
    new TimeFieldTerminalElement(viewProps.props)
    :> ITerminalElement

  static member TreeView<'T when 'T: not struct>(set: TreeViewProps<'T> -> unit) =
    let viewProps = TreeViewProps<'T> ()
    set viewProps
    new TreeViewTerminalElement<'T>(viewProps.props)
    :> ITerminalElement

  static member TreeView<'T when 'T: not struct>(children: ITerminalElement list) =
    let viewProps = TreeViewProps<'T> ()
    viewProps.Children children
    new TreeViewTerminalElement<'T>(viewProps.props)
    :> ITerminalElement

  static member TreeView(set: TreeViewProps -> unit) =
    let viewProps = TreeViewProps ()
    set viewProps
    new TreeViewTerminalElement(viewProps.props)
    :> ITerminalElement

  static member TreeView(children: ITerminalElement list) =
    let viewProps = TreeViewProps ()
    viewProps.Children children
    new TreeViewTerminalElement(viewProps.props)
    :> ITerminalElement

  static member Window(set: WindowProps -> unit) =
    let viewProps = WindowProps ()
    set viewProps
    new WindowTerminalElement(viewProps.props)
    :> ITerminalElement

  static member Window(children: ITerminalElement list) =
    let viewProps = WindowProps ()
    viewProps.Children children
    new WindowTerminalElement(viewProps.props)
    :> ITerminalElement

  static member Wizard(set: WizardProps -> unit) =
    let viewProps = WizardProps ()
    set viewProps
    new WizardTerminalElement(viewProps.props)
    :> ITerminalElement

  static member Wizard(children: ITerminalElement list) =
    let viewProps = WizardProps ()
    viewProps.Children children
    new WizardTerminalElement(viewProps.props)
    :> ITerminalElement

  static member WizardStep(set: WizardStepProps -> unit) =
    let viewProps = WizardStepProps ()
    set viewProps
    new WizardStepTerminalElement(viewProps.props)
    :> IWizardStepTerminalElement

  static member WizardStep(children: ITerminalElement list) =
    let viewProps = WizardStepProps ()
    viewProps.Children children
    new WizardStepTerminalElement(viewProps.props)
    :> IWizardStepTerminalElement
