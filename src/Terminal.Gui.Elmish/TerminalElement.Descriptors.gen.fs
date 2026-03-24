namespace Terminal.Gui.Elmish

type Adornment(props: AdornmentProps) =
  let viewTe = lazy (new AdornmentTerminalElement(props.props))

  interface ViewBase with
    member _.CreateViewTE() = viewTe.Value
    member _.Props = props.props

type AttributePicker(props: AttributePickerProps) =
  let viewTe = lazy (new AttributePickerTerminalElement(props.props))

  interface ViewBase with
    member _.CreateViewTE() = viewTe.Value
    member _.Props = props.props

type Bar(props: BarProps) =
  let viewTe = lazy (new BarTerminalElement(props.props))

  interface ViewBase with
    member _.CreateViewTE() = viewTe.Value
    member _.Props = props.props

type Border(props: BorderProps) =
  let viewTe = lazy (new BorderTerminalElement(props.props))

  interface ViewBase with
    member _.CreateViewTE() = viewTe.Value
    member _.Props = props.props

type Button(props: ButtonProps) =
  let viewTe = lazy (new ButtonTerminalElement(props.props))

  interface ViewBase with
    member _.CreateViewTE() = viewTe.Value
    member _.Props = props.props

type CharMap(props: CharMapProps) =
  let viewTe = lazy (new CharMapTerminalElement(props.props))

  interface ViewBase with
    member _.CreateViewTE() = viewTe.Value
    member _.Props = props.props

type CheckBox(props: CheckBoxProps) =
  let viewTe = lazy (new CheckBoxTerminalElement(props.props))

  interface ViewBase with
    member _.CreateViewTE() = viewTe.Value
    member _.Props = props.props

type ColorPicker(props: ColorPickerProps) =
  let viewTe = lazy (new ColorPickerTerminalElement(props.props))

  interface ViewBase with
    member _.CreateViewTE() = viewTe.Value
    member _.Props = props.props

type ColorPicker16(props: ColorPicker16Props) =
  let viewTe = lazy (new ColorPicker16TerminalElement(props.props))

  interface ViewBase with
    member _.CreateViewTE() = viewTe.Value
    member _.Props = props.props

type DatePicker(props: DatePickerProps) =
  let viewTe = lazy (new DatePickerTerminalElement(props.props))

  interface ViewBase with
    member _.CreateViewTE() = viewTe.Value
    member _.Props = props.props

type FrameView(props: FrameViewProps) =
  let viewTe = lazy (new FrameViewTerminalElement(props.props))

  interface ViewBase with
    member _.CreateViewTE() = viewTe.Value
    member _.Props = props.props

type GraphView(props: GraphViewProps) =
  let viewTe = lazy (new GraphViewTerminalElement(props.props))

  interface ViewBase with
    member _.CreateViewTE() = viewTe.Value
    member _.Props = props.props

type HexView(props: HexViewProps) =
  let viewTe = lazy (new HexViewTerminalElement(props.props))

  interface ViewBase with
    member _.CreateViewTE() = viewTe.Value
    member _.Props = props.props

type Label(props: LabelProps) =
  let viewTe = lazy (new LabelTerminalElement(props.props))

  interface ViewBase with
    member _.CreateViewTE() = viewTe.Value
    member _.Props = props.props

type LegendAnnotation(props: LegendAnnotationProps) =
  let viewTe = lazy (new LegendAnnotationTerminalElement(props.props))

  interface ViewBase with
    member _.CreateViewTE() = viewTe.Value
    member _.Props = props.props

type Line(props: LineProps) =
  let viewTe = lazy (new LineTerminalElement(props.props))

  interface ViewBase with
    member _.CreateViewTE() = viewTe.Value
    member _.Props = props.props

type LinearRange<'T>(props: LinearRangeProps<'T>) =
  let viewTe = lazy (new LinearRangeTerminalElement<'T>(props.props))

  interface ViewBase with
    member _.CreateViewTE() = viewTe.Value
    member _.Props = props.props

type LinearRange(props: LinearRangeProps) =
  let viewTe = lazy (new LinearRangeTerminalElement(props.props))

  interface ViewBase with
    member _.CreateViewTE() = viewTe.Value
    member _.Props = props.props

type Link(props: LinkProps) =
  let viewTe = lazy (new LinkTerminalElement(props.props))

  interface ViewBase with
    member _.CreateViewTE() = viewTe.Value
    member _.Props = props.props

type ListView(props: ListViewProps) =
  let viewTe = lazy (new ListViewTerminalElement(props.props))

  interface ViewBase with
    member _.CreateViewTE() = viewTe.Value
    member _.Props = props.props

type Margin(props: MarginProps) =
  let viewTe = lazy (new MarginTerminalElement(props.props))

  interface ViewBase with
    member _.CreateViewTE() = viewTe.Value
    member _.Props = props.props

type Menu(props: MenuProps) =
  let viewTe = lazy (new MenuTerminalElement(props.props))

  interface ViewBase with
    member _.CreateViewTE() = viewTe.Value
    member _.Props = props.props

  interface IMenuView

type MenuBar(props: MenuBarProps) =
  let viewTe = lazy (new MenuBarTerminalElement(props.props))

  interface ViewBase with
    member _.CreateViewTE() = viewTe.Value
    member _.Props = props.props

  interface IMenuView

type NumericUpDown<'T>(props: NumericUpDownProps<'T>) =
  let viewTe = lazy (new NumericUpDownTerminalElement<'T>(props.props))

  interface ViewBase with
    member _.CreateViewTE() = viewTe.Value
    member _.Props = props.props

type NumericUpDown(props: NumericUpDownProps) =
  let viewTe = lazy (new NumericUpDownTerminalElement(props.props))

  interface ViewBase with
    member _.CreateViewTE() = viewTe.Value
    member _.Props = props.props

type Padding(props: PaddingProps) =
  let viewTe = lazy (new PaddingTerminalElement(props.props))

  interface ViewBase with
    member _.CreateViewTE() = viewTe.Value
    member _.Props = props.props

type Popover<'TView, 'TResult when 'TView: (new: unit -> 'TView) and 'TView :> Terminal.Gui.ViewBase.View>
  (props: PopoverProps<'TView, 'TResult>) =
  let viewTe = lazy (new PopoverTerminalElement<'TView, 'TResult>(props.props))

  interface ViewBase with
    member _.CreateViewTE() = viewTe.Value
    member _.Props = props.props

type PopoverMenu(props: PopoverMenuProps) =
  let viewTe = lazy (new PopoverMenuTerminalElement(props.props))

  interface ViewBase with
    member _.CreateViewTE() = viewTe.Value
    member _.Props = props.props

  interface IPopoverMenuView

type ProgressBar(props: ProgressBarProps) =
  let viewTe = lazy (new ProgressBarTerminalElement(props.props))

  interface ViewBase with
    member _.CreateViewTE() = viewTe.Value
    member _.Props = props.props

type Runnable(props: RunnableProps) =
  let viewTe = lazy (new RunnableTerminalElement(props.props))

  interface ViewBase with
    member _.CreateViewTE() = viewTe.Value
    member _.Props = props.props

type Runnable<'TResult>(props: RunnableProps<'TResult>) =
  let viewTe = lazy (new RunnableTerminalElement<'TResult>(props.props))

  interface ViewBase with
    member _.CreateViewTE() = viewTe.Value
    member _.Props = props.props

type Dialog<'TResult>(props: DialogProps<'TResult>) =
  let viewTe = lazy (new DialogTerminalElement<'TResult>(props.props))

  interface ViewBase with
    member _.CreateViewTE() = viewTe.Value
    member _.Props = props.props

type Dialog(props: DialogProps) =
  let viewTe = lazy (new DialogTerminalElement(props.props))

  interface ViewBase with
    member _.CreateViewTE() = viewTe.Value
    member _.Props = props.props

type Prompt<'TView, 'TResult when 'TView: (new: unit -> 'TView) and 'TView :> Terminal.Gui.ViewBase.View>
  (props: PromptProps<'TView, 'TResult>) =
  let viewTe = lazy (new PromptTerminalElement<'TView, 'TResult>(props.props))

  interface ViewBase with
    member _.CreateViewTE() = viewTe.Value
    member _.Props = props.props

type FileDialog(props: FileDialogProps) =
  let viewTe = lazy (new FileDialogTerminalElement(props.props))

  interface ViewBase with
    member _.CreateViewTE() = viewTe.Value
    member _.Props = props.props

type OpenDialog(props: OpenDialogProps) =
  let viewTe = lazy (new OpenDialogTerminalElement(props.props))

  interface ViewBase with
    member _.CreateViewTE() = viewTe.Value
    member _.Props = props.props

type SaveDialog(props: SaveDialogProps) =
  let viewTe = lazy (new SaveDialogTerminalElement(props.props))

  interface ViewBase with
    member _.CreateViewTE() = viewTe.Value
    member _.Props = props.props

type ScrollBar(props: ScrollBarProps) =
  let viewTe = lazy (new ScrollBarTerminalElement(props.props))

  interface ViewBase with
    member _.CreateViewTE() = viewTe.Value
    member _.Props = props.props

type ScrollSlider(props: ScrollSliderProps) =
  let viewTe = lazy (new ScrollSliderTerminalElement(props.props))

  interface ViewBase with
    member _.CreateViewTE() = viewTe.Value
    member _.Props = props.props

type FlagSelector(props: FlagSelectorProps) =
  let viewTe = lazy (new FlagSelectorTerminalElement(props.props))

  interface ViewBase with
    member _.CreateViewTE() = viewTe.Value
    member _.Props = props.props

type OptionSelector(props: OptionSelectorProps) =
  let viewTe = lazy (new OptionSelectorTerminalElement(props.props))

  interface ViewBase with
    member _.CreateViewTE() = viewTe.Value
    member _.Props = props.props

type FlagSelector<'TFlagsEnum
  when 'TFlagsEnum: struct
  and 'TFlagsEnum: (new: unit -> 'TFlagsEnum)
  and 'TFlagsEnum :> System.Enum
  and 'TFlagsEnum :> System.ValueType>(props: FlagSelectorProps<'TFlagsEnum>) =
  let viewTe = lazy (new FlagSelectorTerminalElement<'TFlagsEnum>(props.props))

  interface ViewBase with
    member _.CreateViewTE() = viewTe.Value
    member _.Props = props.props

type OptionSelector<'TEnum
  when 'TEnum: struct and 'TEnum: (new: unit -> 'TEnum) and 'TEnum :> System.Enum and 'TEnum :> System.ValueType>
  (props: OptionSelectorProps<'TEnum>) =
  let viewTe = lazy (new OptionSelectorTerminalElement<'TEnum>(props.props))

  interface ViewBase with
    member _.CreateViewTE() = viewTe.Value
    member _.Props = props.props

type Shortcut(props: ShortcutProps) =
  let viewTe = lazy (new ShortcutTerminalElement(props.props))

  interface ViewBase with
    member _.CreateViewTE() = viewTe.Value
    member _.Props = props.props

type MenuItem(props: MenuItemProps) =
  let viewTe = lazy (new MenuItemTerminalElement(props.props))

  interface ViewBase with
    member _.CreateViewTE() = viewTe.Value
    member _.Props = props.props

  interface IMenuItemView

type MenuBarItem(props: MenuBarItemProps) =
  let viewTe = lazy (new MenuBarItemTerminalElement(props.props))

  interface ViewBase with
    member _.CreateViewTE() = viewTe.Value
    member _.Props = props.props

  interface IMenuItemView

type SpinnerView(props: SpinnerViewProps) =
  let viewTe = lazy (new SpinnerViewTerminalElement(props.props))

  interface ViewBase with
    member _.CreateViewTE() = viewTe.Value
    member _.Props = props.props

type StatusBar(props: StatusBarProps) =
  let viewTe = lazy (new StatusBarTerminalElement(props.props))

  interface ViewBase with
    member _.CreateViewTE() = viewTe.Value
    member _.Props = props.props

type Tab(props: TabProps) =
  let viewTe = lazy (new TabTerminalElement(props.props))

  interface ViewBase with
    member _.CreateViewTE() = viewTe.Value
    member _.Props = props.props

  interface ITabView

type TabView(props: TabViewProps) =
  let viewTe = lazy (new TabViewTerminalElement(props.props))

  interface ViewBase with
    member _.CreateViewTE() = viewTe.Value
    member _.Props = props.props

type TableView(props: TableViewProps) =
  let viewTe = lazy (new TableViewTerminalElement(props.props))

  interface ViewBase with
    member _.CreateViewTE() = viewTe.Value
    member _.Props = props.props

type TextField(props: TextFieldProps) =
  let viewTe = lazy (new TextFieldTerminalElement(props.props))

  interface ViewBase with
    member _.CreateViewTE() = viewTe.Value
    member _.Props = props.props

type DropDownList(props: DropDownListProps) =
  let viewTe = lazy (new DropDownListTerminalElement(props.props))

  interface ViewBase with
    member _.CreateViewTE() = viewTe.Value
    member _.Props = props.props

type TextValidateField(props: TextValidateFieldProps) =
  let viewTe = lazy (new TextValidateFieldTerminalElement(props.props))

  interface ViewBase with
    member _.CreateViewTE() = viewTe.Value
    member _.Props = props.props

type DateEditor(props: DateEditorProps) =
  let viewTe = lazy (new DateEditorTerminalElement(props.props))

  interface ViewBase with
    member _.CreateViewTE() = viewTe.Value
    member _.Props = props.props

type TextView(props: TextViewProps) =
  let viewTe = lazy (new TextViewTerminalElement(props.props))

  interface ViewBase with
    member _.CreateViewTE() = viewTe.Value
    member _.Props = props.props

type TimeEditor(props: TimeEditorProps) =
  let viewTe = lazy (new TimeEditorTerminalElement(props.props))

  interface ViewBase with
    member _.CreateViewTE() = viewTe.Value
    member _.Props = props.props

type TreeView<'T when 'T: not struct>(props: TreeViewProps<'T>) =
  let viewTe = lazy (new TreeViewTerminalElement<'T>(props.props))

  interface ViewBase with
    member _.CreateViewTE() = viewTe.Value
    member _.Props = props.props

type TreeView(props: TreeViewProps) =
  let viewTe = lazy (new TreeViewTerminalElement(props.props))

  interface ViewBase with
    member _.CreateViewTE() = viewTe.Value
    member _.Props = props.props

type Window(props: WindowProps) =
  let viewTe = lazy (new WindowTerminalElement(props.props))

  interface ViewBase with
    member _.CreateViewTE() = viewTe.Value
    member _.Props = props.props

type Wizard(props: WizardProps) =
  let viewTe = lazy (new WizardTerminalElement(props.props))

  interface ViewBase with
    member _.CreateViewTE() = viewTe.Value
    member _.Props = props.props

type WizardStep(props: WizardStepProps) =
  let viewTe = lazy (new WizardStepTerminalElement(props.props))

  interface ViewBase with
    member _.CreateViewTE() = viewTe.Value
    member _.Props = props.props

  interface IWizardStepView
