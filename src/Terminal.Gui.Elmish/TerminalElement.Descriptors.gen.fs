namespace Terminal.Gui.Elmish

type Adornment(props: AdornmentProps) =
  interface ViewBase with
    member _.CreateViewTE() =
      new AdornmentTerminalElement(props.props)

    member _.Props = props.props

type AttributePicker(props: AttributePickerProps) =
  interface ViewBase with
    member _.CreateViewTE() =
      new AttributePickerTerminalElement(props.props)

    member _.Props = props.props

type Bar(props: BarProps) =
  interface ViewBase with
    member _.CreateViewTE() = new BarTerminalElement(props.props)
    member _.Props = props.props

type Border(props: BorderProps) =
  interface ViewBase with
    member _.CreateViewTE() = new BorderTerminalElement(props.props)
    member _.Props = props.props

type Button(props: ButtonProps) =
  interface ViewBase with
    member _.CreateViewTE() = new ButtonTerminalElement(props.props)
    member _.Props = props.props

type CharMap(props: CharMapProps) =
  interface ViewBase with
    member _.CreateViewTE() = new CharMapTerminalElement(props.props)
    member _.Props = props.props

type CheckBox(props: CheckBoxProps) =
  interface ViewBase with
    member _.CreateViewTE() =
      new CheckBoxTerminalElement(props.props)

    member _.Props = props.props

type ColorPicker(props: ColorPickerProps) =
  interface ViewBase with
    member _.CreateViewTE() =
      new ColorPickerTerminalElement(props.props)

    member _.Props = props.props

type ColorPicker16(props: ColorPicker16Props) =
  interface ViewBase with
    member _.CreateViewTE() =
      new ColorPicker16TerminalElement(props.props)

    member _.Props = props.props

type DatePicker(props: DatePickerProps) =
  interface ViewBase with
    member _.CreateViewTE() =
      new DatePickerTerminalElement(props.props)

    member _.Props = props.props

type FrameView(props: FrameViewProps) =
  interface ViewBase with
    member _.CreateViewTE() =
      new FrameViewTerminalElement(props.props)

    member _.Props = props.props

type GraphView(props: GraphViewProps) =
  interface ViewBase with
    member _.CreateViewTE() =
      new GraphViewTerminalElement(props.props)

    member _.Props = props.props

type HexView(props: HexViewProps) =
  interface ViewBase with
    member _.CreateViewTE() = new HexViewTerminalElement(props.props)
    member _.Props = props.props

type Label(props: LabelProps) =
  interface ViewBase with
    member _.CreateViewTE() = new LabelTerminalElement(props.props)
    member _.Props = props.props

type LegendAnnotation(props: LegendAnnotationProps) =
  interface ViewBase with
    member _.CreateViewTE() =
      new LegendAnnotationTerminalElement(props.props)

    member _.Props = props.props

type Line(props: LineProps) =
  interface ViewBase with
    member _.CreateViewTE() = new LineTerminalElement(props.props)
    member _.Props = props.props

type LinearRange<'T>(props: LinearRangeProps<'T>) =
  interface ViewBase with
    member _.CreateViewTE() =
      new LinearRangeTerminalElement<'T>(props.props)

    member _.Props = props.props

type LinearRange(props: LinearRangeProps) =
  interface ViewBase with
    member _.CreateViewTE() =
      new LinearRangeTerminalElement(props.props)

    member _.Props = props.props

type Link(props: LinkProps) =
  interface ViewBase with
    member _.CreateViewTE() = new LinkTerminalElement(props.props)
    member _.Props = props.props

type ListView(props: ListViewProps) =
  interface ViewBase with
    member _.CreateViewTE() =
      new ListViewTerminalElement(props.props)

    member _.Props = props.props

type Margin(props: MarginProps) =
  interface ViewBase with
    member _.CreateViewTE() = new MarginTerminalElement(props.props)
    member _.Props = props.props

type Menu(props: MenuProps) =
  interface ViewBase with
    member _.CreateViewTE() = new MenuTerminalElement(props.props)
    member _.Props = props.props

  interface IMenuTerminalElement

type MenuBar(props: MenuBarProps) =
  interface ViewBase with
    member _.CreateViewTE() = new MenuBarTerminalElement(props.props)
    member _.Props = props.props

  interface IMenuTerminalElement

type NumericUpDown<'T>(props: NumericUpDownProps<'T>) =
  interface ViewBase with
    member _.CreateViewTE() =
      new NumericUpDownTerminalElement<'T>(props.props)

    member _.Props = props.props

type NumericUpDown(props: NumericUpDownProps) =
  interface ViewBase with
    member _.CreateViewTE() =
      new NumericUpDownTerminalElement(props.props)

    member _.Props = props.props

type Padding(props: PaddingProps) =
  interface ViewBase with
    member _.CreateViewTE() = new PaddingTerminalElement(props.props)
    member _.Props = props.props

type Popover<'TView, 'TResult when 'TView: (new: unit -> 'TView) and 'TView :> Terminal.Gui.ViewBase.View>
  (props: PopoverProps<'TView, 'TResult>) =
  interface ViewBase with
    member _.CreateViewTE() =
      new PopoverTerminalElement<'TView, 'TResult>(props.props)

    member _.Props = props.props

type PopoverMenu(props: PopoverMenuProps) =
  interface ViewBase with
    member _.CreateViewTE() =
      new PopoverMenuTerminalElement(props.props)

    member _.Props = props.props

  interface IPopoverMenuTerminalElement

type ProgressBar(props: ProgressBarProps) =
  interface ViewBase with
    member _.CreateViewTE() =
      new ProgressBarTerminalElement(props.props)

    member _.Props = props.props

type Runnable(props: RunnableProps) =
  interface ViewBase with
    member _.CreateViewTE() =
      new RunnableTerminalElement(props.props)

    member _.Props = props.props

type Runnable<'TResult>(props: RunnableProps<'TResult>) =
  interface ViewBase with
    member _.CreateViewTE() =
      new RunnableTerminalElement<'TResult>(props.props)

    member _.Props = props.props

type Dialog<'TResult>(props: DialogProps<'TResult>) =
  interface ViewBase with
    member _.CreateViewTE() =
      new DialogTerminalElement<'TResult>(props.props)

    member _.Props = props.props

type Dialog(props: DialogProps) =
  interface ViewBase with
    member _.CreateViewTE() = new DialogTerminalElement(props.props)
    member _.Props = props.props

type Prompt<'TView, 'TResult when 'TView: (new: unit -> 'TView) and 'TView :> Terminal.Gui.ViewBase.View>
  (props: PromptProps<'TView, 'TResult>) =
  interface ViewBase with
    member _.CreateViewTE() =
      new PromptTerminalElement<'TView, 'TResult>(props.props)

    member _.Props = props.props

type FileDialog(props: FileDialogProps) =
  interface ViewBase with
    member _.CreateViewTE() =
      new FileDialogTerminalElement(props.props)

    member _.Props = props.props

type OpenDialog(props: OpenDialogProps) =
  interface ViewBase with
    member _.CreateViewTE() =
      new OpenDialogTerminalElement(props.props)

    member _.Props = props.props

type SaveDialog(props: SaveDialogProps) =
  interface ViewBase with
    member _.CreateViewTE() =
      new SaveDialogTerminalElement(props.props)

    member _.Props = props.props

type ScrollBar(props: ScrollBarProps) =
  interface ViewBase with
    member _.CreateViewTE() =
      new ScrollBarTerminalElement(props.props)

    member _.Props = props.props

type ScrollSlider(props: ScrollSliderProps) =
  interface ViewBase with
    member _.CreateViewTE() =
      new ScrollSliderTerminalElement(props.props)

    member _.Props = props.props

type FlagSelector(props: FlagSelectorProps) =
  interface ViewBase with
    member _.CreateViewTE() =
      new FlagSelectorTerminalElement(props.props)

    member _.Props = props.props

type OptionSelector(props: OptionSelectorProps) =
  interface ViewBase with
    member _.CreateViewTE() =
      new OptionSelectorTerminalElement(props.props)

    member _.Props = props.props

type FlagSelector<'TFlagsEnum
  when 'TFlagsEnum: struct
  and 'TFlagsEnum: (new: unit -> 'TFlagsEnum)
  and 'TFlagsEnum :> System.Enum
  and 'TFlagsEnum :> System.ValueType>(props: FlagSelectorProps<'TFlagsEnum>) =
  interface ViewBase with
    member _.CreateViewTE() =
      new FlagSelectorTerminalElement<'TFlagsEnum>(props.props)

    member _.Props = props.props

type OptionSelector<'TEnum
  when 'TEnum: struct and 'TEnum: (new: unit -> 'TEnum) and 'TEnum :> System.Enum and 'TEnum :> System.ValueType>
  (props: OptionSelectorProps<'TEnum>) =
  interface ViewBase with
    member _.CreateViewTE() =
      new OptionSelectorTerminalElement<'TEnum>(props.props)

    member _.Props = props.props

type Shortcut(props: ShortcutProps) =
  interface ViewBase with
    member _.CreateViewTE() =
      new ShortcutTerminalElement(props.props)

    member _.Props = props.props

type MenuItem(props: MenuItemProps) =
  interface ViewBase with
    member _.CreateViewTE() =
      new MenuItemTerminalElement(props.props)

    member _.Props = props.props

  interface IMenuItemTerminalElement

type MenuBarItem(props: MenuBarItemProps) =
  interface ViewBase with
    member _.CreateViewTE() =
      new MenuBarItemTerminalElement(props.props)

    member _.Props = props.props

  interface IMenuItemTerminalElement

type SpinnerView(props: SpinnerViewProps) =
  interface ViewBase with
    member _.CreateViewTE() =
      new SpinnerViewTerminalElement(props.props)

    member _.Props = props.props

type StatusBar(props: StatusBarProps) =
  interface ViewBase with
    member _.CreateViewTE() =
      new StatusBarTerminalElement(props.props)

    member _.Props = props.props

type Tab(props: TabProps) =
  interface ViewBase with
    member _.CreateViewTE() = new TabTerminalElement(props.props)
    member _.Props = props.props

  interface ITabTerminalElement

type TabView(props: TabViewProps) =
  interface ViewBase with
    member _.CreateViewTE() = new TabViewTerminalElement(props.props)
    member _.Props = props.props

type TableView(props: TableViewProps) =
  interface ViewBase with
    member _.CreateViewTE() =
      new TableViewTerminalElement(props.props)

    member _.Props = props.props

type TextField(props: TextFieldProps) =
  interface ViewBase with
    member _.CreateViewTE() =
      new TextFieldTerminalElement(props.props)

    member _.Props = props.props

type DropDownList(props: DropDownListProps) =
  interface ViewBase with
    member _.CreateViewTE() =
      new DropDownListTerminalElement(props.props)

    member _.Props = props.props

type TextValidateField(props: TextValidateFieldProps) =
  interface ViewBase with
    member _.CreateViewTE() =
      new TextValidateFieldTerminalElement(props.props)

    member _.Props = props.props

type DateEditor(props: DateEditorProps) =
  interface ViewBase with
    member _.CreateViewTE() =
      new DateEditorTerminalElement(props.props)

    member _.Props = props.props

type TextView(props: TextViewProps) =
  interface ViewBase with
    member _.CreateViewTE() =
      new TextViewTerminalElement(props.props)

    member _.Props = props.props

type TimeEditor(props: TimeEditorProps) =
  interface ViewBase with
    member _.CreateViewTE() =
      new TimeEditorTerminalElement(props.props)

    member _.Props = props.props

type TreeView<'T when 'T: not struct>(props: TreeViewProps<'T>) =
  interface ViewBase with
    member _.CreateViewTE() =
      new TreeViewTerminalElement<'T>(props.props)

    member _.Props = props.props

type TreeView(props: TreeViewProps) =
  interface ViewBase with
    member _.CreateViewTE() =
      new TreeViewTerminalElement(props.props)

    member _.Props = props.props

type Window(props: WindowProps) =
  interface ViewBase with
    member _.CreateViewTE() = new WindowTerminalElement(props.props)
    member _.Props = props.props

type Wizard(props: WizardProps) =
  interface ViewBase with
    member _.CreateViewTE() = new WizardTerminalElement(props.props)
    member _.Props = props.props

type WizardStep(props: WizardStepProps) =
  interface ViewBase with
    member _.CreateViewTE() =
      new WizardStepTerminalElement(props.props)

    member _.Props = props.props

  interface IWizardStepTerminalElement
