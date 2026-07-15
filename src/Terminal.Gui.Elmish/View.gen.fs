namespace Terminal.Gui.Elmish

open Terminal.Gui.Elmish

type View =

  static member AdornmentView(set: AdornmentViewProps -> unit) =
    let viewProps = AdornmentViewProps()
    set viewProps
    AdornmentView(viewProps)

  static member AdornmentView(children: IView list) =
    let viewProps = AdornmentViewProps()
    viewProps.Children children
    AdornmentView(viewProps)

  static member AttributePicker(set: AttributePickerProps -> unit) =
    let viewProps = AttributePickerProps()
    set viewProps
    AttributePicker(viewProps)

  static member AttributePicker(children: IView list) =
    let viewProps = AttributePickerProps()
    viewProps.Children children
    AttributePicker(viewProps)

  static member Bar(set: BarProps -> unit) =
    let viewProps = BarProps()
    set viewProps
    Bar(viewProps)

  static member Bar(children: IView list) =
    let viewProps = BarProps()
    viewProps.Children children
    Bar(viewProps)

  static member BorderView(set: BorderViewProps -> unit) =
    let viewProps = BorderViewProps()
    set viewProps
    BorderView(viewProps)

  static member BorderView(children: IView list) =
    let viewProps = BorderViewProps()
    viewProps.Children children
    BorderView(viewProps)

  static member Button(set: ButtonProps -> unit) =
    let viewProps = ButtonProps()
    set viewProps
    Button(viewProps)

  static member Button(children: IView list) =
    let viewProps = ButtonProps()
    viewProps.Children children
    Button(viewProps)

  static member CharMap(set: CharMapProps -> unit) =
    let viewProps = CharMapProps()
    set viewProps
    CharMap(viewProps)

  static member CharMap(children: IView list) =
    let viewProps = CharMapProps()
    viewProps.Children children
    CharMap(viewProps)

  static member CheckBox(set: CheckBoxProps -> unit) =
    let viewProps = CheckBoxProps()
    set viewProps
    CheckBox(viewProps)

  static member CheckBox(children: IView list) =
    let viewProps = CheckBoxProps()
    viewProps.Children children
    CheckBox(viewProps)

  static member Code(set: CodeProps -> unit) =
    let viewProps = CodeProps()
    set viewProps
    Code(viewProps)

  static member Code(children: IView list) =
    let viewProps = CodeProps()
    viewProps.Children children
    Code(viewProps)

  static member ColorPicker(set: ColorPickerProps -> unit) =
    let viewProps = ColorPickerProps()
    set viewProps
    ColorPicker(viewProps)

  static member ColorPicker(children: IView list) =
    let viewProps = ColorPickerProps()
    viewProps.Children children
    ColorPicker(viewProps)

  static member ColorPicker16(set: ColorPicker16Props -> unit) =
    let viewProps = ColorPicker16Props()
    set viewProps
    ColorPicker16(viewProps)

  static member ColorPicker16(children: IView list) =
    let viewProps = ColorPicker16Props()
    viewProps.Children children
    ColorPicker16(viewProps)

  static member DatePicker(set: DatePickerProps -> unit) =
    let viewProps = DatePickerProps()
    set viewProps
    DatePicker(viewProps)

  static member DatePicker(children: IView list) =
    let viewProps = DatePickerProps()
    viewProps.Children children
    DatePicker(viewProps)

  static member FrameView(set: FrameViewProps -> unit) =
    let viewProps = FrameViewProps()
    set viewProps
    FrameView(viewProps)

  static member FrameView(children: IView list) =
    let viewProps = FrameViewProps()
    viewProps.Children children
    FrameView(viewProps)

  static member GraphView(set: GraphViewProps -> unit) =
    let viewProps = GraphViewProps()
    set viewProps
    GraphView(viewProps)

  static member GraphView(children: IView list) =
    let viewProps = GraphViewProps()
    viewProps.Children children
    GraphView(viewProps)

  static member HexView(set: HexViewProps -> unit) =
    let viewProps = HexViewProps()
    set viewProps
    HexView(viewProps)

  static member HexView(children: IView list) =
    let viewProps = HexViewProps()
    viewProps.Children children
    HexView(viewProps)

  static member ImageView(set: ImageViewProps -> unit) =
    let viewProps = ImageViewProps()
    set viewProps
    ImageView(viewProps)

  static member ImageView(children: IView list) =
    let viewProps = ImageViewProps()
    viewProps.Children children
    ImageView(viewProps)

  static member Label(set: LabelProps -> unit) =
    let viewProps = LabelProps()
    set viewProps
    Label(viewProps)

  static member Label(children: IView list) =
    let viewProps = LabelProps()
    viewProps.Children children
    Label(viewProps)

  static member LegendAnnotation(set: LegendAnnotationProps -> unit) =
    let viewProps = LegendAnnotationProps()
    set viewProps
    LegendAnnotation(viewProps)

  static member LegendAnnotation(children: IView list) =
    let viewProps = LegendAnnotationProps()
    viewProps.Children children
    LegendAnnotation(viewProps)

  static member Line(set: LineProps -> unit) =
    let viewProps = LineProps()
    set viewProps
    Line(viewProps)

  static member Line(children: IView list) =
    let viewProps = LineProps()
    viewProps.Children children
    Line(viewProps)

  static member LinearMultiSelector<'T>(set: LinearMultiSelectorProps<'T> -> unit) =
    let viewProps = LinearMultiSelectorProps<'T>()
    set viewProps
    LinearMultiSelector<'T>(viewProps)

  static member LinearMultiSelector<'T>(children: IView list) =
    let viewProps = LinearMultiSelectorProps<'T>()
    viewProps.Children children
    LinearMultiSelector<'T>(viewProps)

  static member LinearMultiSelector(set: LinearMultiSelectorProps -> unit) =
    let viewProps = LinearMultiSelectorProps()
    set viewProps
    LinearMultiSelector(viewProps)

  static member LinearMultiSelector(children: IView list) =
    let viewProps = LinearMultiSelectorProps()
    viewProps.Children children
    LinearMultiSelector(viewProps)

  static member LinearRange<'T>(set: LinearRangeProps<'T> -> unit) =
    let viewProps = LinearRangeProps<'T>()
    set viewProps
    LinearRange<'T>(viewProps)

  static member LinearRange<'T>(children: IView list) =
    let viewProps = LinearRangeProps<'T>()
    viewProps.Children children
    LinearRange<'T>(viewProps)

  static member LinearRange(set: LinearRangeProps -> unit) =
    let viewProps = LinearRangeProps()
    set viewProps
    LinearRange(viewProps)

  static member LinearRange(children: IView list) =
    let viewProps = LinearRangeProps()
    viewProps.Children children
    LinearRange(viewProps)

  static member LinearSelector<'T>(set: LinearSelectorProps<'T> -> unit) =
    let viewProps = LinearSelectorProps<'T>()
    set viewProps
    LinearSelector<'T>(viewProps)

  static member LinearSelector<'T>(children: IView list) =
    let viewProps = LinearSelectorProps<'T>()
    viewProps.Children children
    LinearSelector<'T>(viewProps)

  static member LinearSelector(set: LinearSelectorProps -> unit) =
    let viewProps = LinearSelectorProps()
    set viewProps
    LinearSelector(viewProps)

  static member LinearSelector(children: IView list) =
    let viewProps = LinearSelectorProps()
    viewProps.Children children
    LinearSelector(viewProps)

  static member Link(set: LinkProps -> unit) =
    let viewProps = LinkProps()
    set viewProps
    Link(viewProps)

  static member Link(children: IView list) =
    let viewProps = LinkProps()
    viewProps.Children children
    Link(viewProps)

  static member ListView(set: ListViewProps -> unit) =
    let viewProps = ListViewProps()
    set viewProps
    ListView(viewProps)

  static member ListView(children: IView list) =
    let viewProps = ListViewProps()
    viewProps.Children children
    ListView(viewProps)

  static member ListView<'T>(set: ListViewProps<'T> -> unit) =
    let viewProps = ListViewProps<'T>()
    set viewProps
    ListView<'T>(viewProps)

  static member ListView<'T>(children: IView list) =
    let viewProps = ListViewProps<'T>()
    viewProps.Children children
    ListView<'T>(viewProps)

  static member MarginView(set: MarginViewProps -> unit) =
    let viewProps = MarginViewProps()
    set viewProps
    MarginView(viewProps)

  static member MarginView(children: IView list) =
    let viewProps = MarginViewProps()
    viewProps.Children children
    MarginView(viewProps)

  static member Markdown(set: MarkdownProps -> unit) =
    let viewProps = MarkdownProps()
    set viewProps
    Markdown(viewProps)

  static member Markdown(children: IView list) =
    let viewProps = MarkdownProps()
    viewProps.Children children
    Markdown(viewProps)

  static member MarkdownCodeBlock(set: MarkdownCodeBlockProps -> unit) =
    let viewProps = MarkdownCodeBlockProps()
    set viewProps
    MarkdownCodeBlock(viewProps)

  static member MarkdownCodeBlock(children: IView list) =
    let viewProps = MarkdownCodeBlockProps()
    viewProps.Children children
    MarkdownCodeBlock(viewProps)

  static member MarkdownTable(set: MarkdownTableProps -> unit) =
    let viewProps = MarkdownTableProps()
    set viewProps
    MarkdownTable(viewProps)

  static member MarkdownTable(children: IView list) =
    let viewProps = MarkdownTableProps()
    viewProps.Children children
    MarkdownTable(viewProps)

  static member Menu(set: MenuProps -> unit) =
    let viewProps = MenuProps()
    set viewProps
    Menu(viewProps)

  static member Menu(children: IView list) =
    let viewProps = MenuProps()
    viewProps.Children children
    Menu(viewProps)

  static member MenuBar(set: MenuBarProps -> MenuBarMacros -> unit) =
    let props = MenuBarProps()
    let macros = MenuBarMacros props
    set props macros
    MenuBar(props)

  static member MenuBar(set: MenuBarProps -> unit) =
    let viewProps = MenuBarProps()
    set viewProps
    MenuBar(viewProps)

  static member MenuBar(children: IView list) =
    let viewProps = MenuBarProps()
    viewProps.Children children
    MenuBar(viewProps)

  static member NumericUpDown<'T>(set: NumericUpDownProps<'T> -> unit) =
    let viewProps = NumericUpDownProps<'T>()
    set viewProps
    NumericUpDown<'T>(viewProps)

  static member NumericUpDown<'T>(children: IView list) =
    let viewProps = NumericUpDownProps<'T>()
    viewProps.Children children
    NumericUpDown<'T>(viewProps)

  static member NumericUpDown(set: NumericUpDownProps -> unit) =
    let viewProps = NumericUpDownProps()
    set viewProps
    NumericUpDown(viewProps)

  static member NumericUpDown(children: IView list) =
    let viewProps = NumericUpDownProps()
    viewProps.Children children
    NumericUpDown(viewProps)

  static member PaddingView(set: PaddingViewProps -> unit) =
    let viewProps = PaddingViewProps()
    set viewProps
    PaddingView(viewProps)

  static member PaddingView(children: IView list) =
    let viewProps = PaddingViewProps()
    viewProps.Children children
    PaddingView(viewProps)

  static member Popover<'TView, 'TResult when 'TView: (new: unit -> 'TView) and 'TView :> Terminal.Gui.ViewBase.View>
    (set: PopoverProps<'TView, 'TResult> -> unit)
    =
    let viewProps = PopoverProps<'TView, 'TResult>()
    set viewProps
    Popover<'TView, 'TResult>(viewProps)

  static member Popover<'TView, 'TResult when 'TView: (new: unit -> 'TView) and 'TView :> Terminal.Gui.ViewBase.View>
    (children: IView list)
    =
    let viewProps = PopoverProps<'TView, 'TResult>()
    viewProps.Children children
    Popover<'TView, 'TResult>(viewProps)

  static member PopoverMenu(set: PopoverMenuProps -> unit) =
    let viewProps = PopoverMenuProps()
    set viewProps
    PopoverMenu(viewProps)

  static member PopoverMenu(children: IView list) =
    let viewProps = PopoverMenuProps()
    viewProps.Children children
    PopoverMenu(viewProps)

  static member ProgressBar(set: ProgressBarProps -> unit) =
    let viewProps = ProgressBarProps()
    set viewProps
    ProgressBar(viewProps)

  static member ProgressBar(children: IView list) =
    let viewProps = ProgressBarProps()
    viewProps.Children children
    ProgressBar(viewProps)

  static member Runnable(set: RunnableProps -> unit) =
    let viewProps = RunnableProps()
    set viewProps
    Runnable(viewProps)

  static member Runnable(children: IView list) =
    let viewProps = RunnableProps()
    viewProps.Children children
    Runnable(viewProps)

  static member Runnable<'TResult>(set: RunnableProps<'TResult> -> unit) =
    let viewProps = RunnableProps<'TResult>()
    set viewProps
    Runnable<'TResult>(viewProps)

  static member Runnable<'TResult>(children: IView list) =
    let viewProps = RunnableProps<'TResult>()
    viewProps.Children children
    Runnable<'TResult>(viewProps)

  static member Dialog<'TResult>(set: DialogProps<'TResult> -> unit) =
    let viewProps = DialogProps<'TResult>()
    set viewProps
    Dialog<'TResult>(viewProps)

  static member Dialog<'TResult>(children: IView list) =
    let viewProps = DialogProps<'TResult>()
    viewProps.Children children
    Dialog<'TResult>(viewProps)

  static member RunnableWrapper<'TView, 'TResult
    when 'TView: (new: unit -> 'TView) and 'TView :> Terminal.Gui.ViewBase.View>
    (set: RunnableWrapperProps<'TView, 'TResult> -> unit)
    =
    let viewProps = RunnableWrapperProps<'TView, 'TResult>()
    set viewProps
    RunnableWrapper<'TView, 'TResult>(viewProps)

  static member RunnableWrapper<'TView, 'TResult
    when 'TView: (new: unit -> 'TView) and 'TView :> Terminal.Gui.ViewBase.View>
    (children: IView list)
    =
    let viewProps = RunnableWrapperProps<'TView, 'TResult>()
    viewProps.Children children
    RunnableWrapper<'TView, 'TResult>(viewProps)

  static member Dialog(set: DialogProps -> unit) =
    let viewProps = DialogProps()
    set viewProps
    Dialog(viewProps)

  static member Dialog(children: IView list) =
    let viewProps = DialogProps()
    viewProps.Children children
    Dialog(viewProps)

  static member FileDialog(set: FileDialogProps -> unit) =
    let viewProps = FileDialogProps()
    set viewProps
    FileDialog(viewProps)

  static member FileDialog(children: IView list) =
    let viewProps = FileDialogProps()
    viewProps.Children children
    FileDialog(viewProps)

  static member Prompt<'TView, 'TResult when 'TView: (new: unit -> 'TView) and 'TView :> Terminal.Gui.ViewBase.View>
    (set: PromptProps<'TView, 'TResult> -> unit)
    =
    let viewProps = PromptProps<'TView, 'TResult>()
    set viewProps
    Prompt<'TView, 'TResult>(viewProps)

  static member Prompt<'TView, 'TResult when 'TView: (new: unit -> 'TView) and 'TView :> Terminal.Gui.ViewBase.View>
    (children: IView list)
    =
    let viewProps = PromptProps<'TView, 'TResult>()
    viewProps.Children children
    Prompt<'TView, 'TResult>(viewProps)

  static member OpenDialog(set: OpenDialogProps -> unit) =
    let viewProps = OpenDialogProps()
    set viewProps
    OpenDialog(viewProps)

  static member OpenDialog(children: IView list) =
    let viewProps = OpenDialogProps()
    viewProps.Children children
    OpenDialog(viewProps)

  static member SaveDialog(set: SaveDialogProps -> unit) =
    let viewProps = SaveDialogProps()
    set viewProps
    SaveDialog(viewProps)

  static member SaveDialog(children: IView list) =
    let viewProps = SaveDialogProps()
    viewProps.Children children
    SaveDialog(viewProps)

  static member ScrollBar(set: ScrollBarProps -> unit) =
    let viewProps = ScrollBarProps()
    set viewProps
    ScrollBar(viewProps)

  static member ScrollBar(children: IView list) =
    let viewProps = ScrollBarProps()
    viewProps.Children children
    ScrollBar(viewProps)

  static member ScrollButton(set: ScrollButtonProps -> unit) =
    let viewProps = ScrollButtonProps()
    set viewProps
    ScrollButton(viewProps)

  static member ScrollButton(children: IView list) =
    let viewProps = ScrollButtonProps()
    viewProps.Children children
    ScrollButton(viewProps)

  static member ScrollSlider(set: ScrollSliderProps -> unit) =
    let viewProps = ScrollSliderProps()
    set viewProps
    ScrollSlider(viewProps)

  static member ScrollSlider(children: IView list) =
    let viewProps = ScrollSliderProps()
    viewProps.Children children
    ScrollSlider(viewProps)

  static member FlagSelector(set: FlagSelectorProps -> unit) =
    let viewProps = FlagSelectorProps()
    set viewProps
    FlagSelector(viewProps)

  static member FlagSelector(children: IView list) =
    let viewProps = FlagSelectorProps()
    viewProps.Children children
    FlagSelector(viewProps)

  static member OptionSelector(set: OptionSelectorProps -> unit) =
    let viewProps = OptionSelectorProps()
    set viewProps
    OptionSelector(viewProps)

  static member OptionSelector(children: IView list) =
    let viewProps = OptionSelectorProps()
    viewProps.Children children
    OptionSelector(viewProps)

  static member FlagSelector<'TFlagsEnum
    when 'TFlagsEnum: struct
    and 'TFlagsEnum: (new: unit -> 'TFlagsEnum)
    and 'TFlagsEnum :> System.Enum
    and 'TFlagsEnum :> System.ValueType>
    (set: FlagSelectorProps<'TFlagsEnum> -> unit)
    =
    let viewProps = FlagSelectorProps<'TFlagsEnum>()
    set viewProps
    FlagSelector<'TFlagsEnum>(viewProps)

  static member FlagSelector<'TFlagsEnum
    when 'TFlagsEnum: struct
    and 'TFlagsEnum: (new: unit -> 'TFlagsEnum)
    and 'TFlagsEnum :> System.Enum
    and 'TFlagsEnum :> System.ValueType>
    (children: IView list)
    =
    let viewProps = FlagSelectorProps<'TFlagsEnum>()
    viewProps.Children children
    FlagSelector<'TFlagsEnum>(viewProps)

  static member OptionSelector<'TEnum
    when 'TEnum: struct and 'TEnum: (new: unit -> 'TEnum) and 'TEnum :> System.Enum and 'TEnum :> System.ValueType>
    (set: OptionSelectorProps<'TEnum> -> unit)
    =
    let viewProps = OptionSelectorProps<'TEnum>()
    set viewProps
    OptionSelector<'TEnum>(viewProps)

  static member OptionSelector<'TEnum
    when 'TEnum: struct and 'TEnum: (new: unit -> 'TEnum) and 'TEnum :> System.Enum and 'TEnum :> System.ValueType>
    (children: IView list)
    =
    let viewProps = OptionSelectorProps<'TEnum>()
    viewProps.Children children
    OptionSelector<'TEnum>(viewProps)

  static member Shortcut(set: ShortcutProps -> unit) =
    let viewProps = ShortcutProps()
    set viewProps
    Shortcut(viewProps)

  static member Shortcut(children: IView list) =
    let viewProps = ShortcutProps()
    viewProps.Children children
    Shortcut(viewProps)

  static member MenuItem(set: MenuItemProps -> unit) =
    let viewProps = MenuItemProps()
    set viewProps
    MenuItem(viewProps)

  static member MenuItem(children: IView list) =
    let viewProps = MenuItemProps()
    viewProps.Children children
    MenuItem(viewProps)

  static member MenuBarItem(set: MenuBarItemProps -> MenuBarItemMacros -> unit) =
    let props = MenuBarItemProps()
    let macros = MenuBarItemMacros props
    set props macros
    MenuBarItem(props)

  static member MenuBarItem(set: MenuBarItemProps -> unit) =
    let viewProps = MenuBarItemProps()
    set viewProps
    MenuBarItem(viewProps)

  static member MenuBarItem(children: IView list) =
    let viewProps = MenuBarItemProps()
    viewProps.Children children
    MenuBarItem(viewProps)

  static member SpinnerView(set: SpinnerViewProps -> unit) =
    let viewProps = SpinnerViewProps()
    set viewProps
    SpinnerView(viewProps)

  static member SpinnerView(children: IView list) =
    let viewProps = SpinnerViewProps()
    viewProps.Children children
    SpinnerView(viewProps)

  static member StatusBar(set: StatusBarProps -> unit) =
    let viewProps = StatusBarProps()
    set viewProps
    StatusBar(viewProps)

  static member StatusBar(children: IView list) =
    let viewProps = StatusBarProps()
    viewProps.Children children
    StatusBar(viewProps)

  static member TableView(set: TableViewProps -> unit) =
    let viewProps = TableViewProps()
    set viewProps
    TableView(viewProps)

  static member TableView(children: IView list) =
    let viewProps = TableViewProps()
    viewProps.Children children
    TableView(viewProps)

  static member Tabs(set: TabsProps -> unit) =
    let viewProps = TabsProps()
    set viewProps
    Tabs(viewProps)

  static member Tabs(children: IView list) =
    let viewProps = TabsProps()
    viewProps.Children children
    Tabs(viewProps)

  static member TextField(set: TextFieldProps -> unit) =
    let viewProps = TextFieldProps()
    set viewProps
    TextField(viewProps)

  static member TextField(children: IView list) =
    let viewProps = TextFieldProps()
    viewProps.Children children
    TextField(viewProps)

  static member DropDownList(set: DropDownListProps -> unit) =
    let viewProps = DropDownListProps()
    set viewProps
    DropDownList(viewProps)

  static member DropDownList(children: IView list) =
    let viewProps = DropDownListProps()
    viewProps.Children children
    DropDownList(viewProps)

  static member DropDownList<'TEnum
    when 'TEnum: struct and 'TEnum: (new: unit -> 'TEnum) and 'TEnum :> System.Enum and 'TEnum :> System.ValueType>
    (set: DropDownListProps<'TEnum> -> unit)
    =
    let viewProps = DropDownListProps<'TEnum>()
    set viewProps
    DropDownList<'TEnum>(viewProps)

  static member DropDownList<'TEnum
    when 'TEnum: struct and 'TEnum: (new: unit -> 'TEnum) and 'TEnum :> System.Enum and 'TEnum :> System.ValueType>
    (children: IView list)
    =
    let viewProps = DropDownListProps<'TEnum>()
    viewProps.Children children
    DropDownList<'TEnum>(viewProps)

  static member TextValidateField(set: TextValidateFieldProps -> unit) =
    let viewProps = TextValidateFieldProps()
    set viewProps
    TextValidateField(viewProps)

  static member TextValidateField(children: IView list) =
    let viewProps = TextValidateFieldProps()
    viewProps.Children children
    TextValidateField(viewProps)

  static member DateEditor(set: DateEditorProps -> unit) =
    let viewProps = DateEditorProps()
    set viewProps
    DateEditor(viewProps)

  static member DateEditor(children: IView list) =
    let viewProps = DateEditorProps()
    viewProps.Children children
    DateEditor(viewProps)

  static member TextView(set: TextViewProps -> unit) =
    let viewProps = TextViewProps()
    set viewProps
    TextView(viewProps)

  static member TextView(children: IView list) =
    let viewProps = TextViewProps()
    viewProps.Children children
    TextView(viewProps)

  static member TimeEditor(set: TimeEditorProps -> unit) =
    let viewProps = TimeEditorProps()
    set viewProps
    TimeEditor(viewProps)

  static member TimeEditor(children: IView list) =
    let viewProps = TimeEditorProps()
    viewProps.Children children
    TimeEditor(viewProps)

  static member TitleView(set: TitleViewProps -> unit) =
    let viewProps = TitleViewProps()
    set viewProps
    TitleView(viewProps)

  static member TitleView(children: IView list) =
    let viewProps = TitleViewProps()
    viewProps.Children children
    TitleView(viewProps)

  static member ToolTipHost<'TView when 'TView: (new: unit -> 'TView) and 'TView :> Terminal.Gui.ViewBase.View>
    (set: ToolTipHostProps<'TView> -> unit)
    =
    let viewProps = ToolTipHostProps<'TView>()
    set viewProps
    ToolTipHost<'TView>(viewProps)

  static member ToolTipHost<'TView when 'TView: (new: unit -> 'TView) and 'TView :> Terminal.Gui.ViewBase.View>
    (children: IView list)
    =
    let viewProps = ToolTipHostProps<'TView>()
    viewProps.Children children
    ToolTipHost<'TView>(viewProps)

  static member TreeView<'T when 'T: not struct>(set: TreeViewProps<'T> -> unit) =
    let viewProps = TreeViewProps<'T>()
    set viewProps
    TreeView<'T>(viewProps)

  static member TreeView<'T when 'T: not struct>(children: IView list) =
    let viewProps = TreeViewProps<'T>()
    viewProps.Children children
    TreeView<'T>(viewProps)

  static member TreeView(set: TreeViewProps -> unit) =
    let viewProps = TreeViewProps()
    set viewProps
    TreeView(viewProps)

  static member TreeView(children: IView list) =
    let viewProps = TreeViewProps()
    viewProps.Children children
    TreeView(viewProps)

  static member Window(set: WindowProps -> unit) =
    let viewProps = WindowProps()
    set viewProps
    Window(viewProps)

  static member Window(children: IView list) =
    let viewProps = WindowProps()
    viewProps.Children children
    Window(viewProps)

  static member Wizard(set: WizardProps -> unit) =
    let viewProps = WizardProps()
    set viewProps
    Wizard(viewProps)

  static member Wizard(children: IView list) =
    let viewProps = WizardProps()
    viewProps.Children children
    Wizard(viewProps)

  static member WizardStep(set: WizardStepProps -> unit) =
    let viewProps = WizardStepProps()
    set viewProps
    WizardStep(viewProps)

  static member WizardStep(children: IView list) =
    let viewProps = WizardStepProps()
    viewProps.Children children
    WizardStep(viewProps)
