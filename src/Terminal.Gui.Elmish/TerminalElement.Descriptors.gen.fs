namespace Terminal.Gui.Elmish

type Adornment(props: AdornmentProps) =
  interface ITerminalElementDescriptor with
    member _.CreateViewTE() =
      new AdornmentTerminalElement(props.props) :> IViewTE

    member _.Props = props.props

type AttributePicker(props: AttributePickerProps) =
  interface ITerminalElementDescriptor with
    member _.CreateViewTE() =
      new AttributePickerTerminalElement(props.props) :> IViewTE

    member _.Props = props.props

type Bar(props: BarProps) =
  interface ITerminalElementDescriptor with
    member _.CreateViewTE() =
      new BarTerminalElement(props.props) :> IViewTE

    member _.Props = props.props

type Border(props: BorderProps) =
  interface ITerminalElementDescriptor with
    member _.CreateViewTE() =
      new BorderTerminalElement(props.props) :> IViewTE

    member _.Props = props.props

type Button(props: ButtonProps) =
  interface ITerminalElementDescriptor with
    member _.CreateViewTE() =
      new ButtonTerminalElement(props.props) :> IViewTE

    member _.Props = props.props

type CharMap(props: CharMapProps) =
  interface ITerminalElementDescriptor with
    member _.CreateViewTE() =
      new CharMapTerminalElement(props.props) :> IViewTE

    member _.Props = props.props

type CheckBox(props: CheckBoxProps) =
  interface ITerminalElementDescriptor with
    member _.CreateViewTE() =
      new CheckBoxTerminalElement(props.props) :> IViewTE

    member _.Props = props.props

type ColorPicker(props: ColorPickerProps) =
  interface ITerminalElementDescriptor with
    member _.CreateViewTE() =
      new ColorPickerTerminalElement(props.props) :> IViewTE

    member _.Props = props.props

type ColorPicker16(props: ColorPicker16Props) =
  interface ITerminalElementDescriptor with
    member _.CreateViewTE() =
      new ColorPicker16TerminalElement(props.props) :> IViewTE

    member _.Props = props.props

type DatePicker(props: DatePickerProps) =
  interface ITerminalElementDescriptor with
    member _.CreateViewTE() =
      new DatePickerTerminalElement(props.props) :> IViewTE

    member _.Props = props.props

type FrameView(props: FrameViewProps) =
  interface ITerminalElementDescriptor with
    member _.CreateViewTE() =
      new FrameViewTerminalElement(props.props) :> IViewTE

    member _.Props = props.props

type GraphView(props: GraphViewProps) =
  interface ITerminalElementDescriptor with
    member _.CreateViewTE() =
      new GraphViewTerminalElement(props.props) :> IViewTE

    member _.Props = props.props

type HexView(props: HexViewProps) =
  interface ITerminalElementDescriptor with
    member _.CreateViewTE() =
      new HexViewTerminalElement(props.props) :> IViewTE

    member _.Props = props.props

type Label(props: LabelProps) =
  interface ITerminalElementDescriptor with
    member _.CreateViewTE() =
      new LabelTerminalElement(props.props) :> IViewTE

    member _.Props = props.props

type LegendAnnotation(props: LegendAnnotationProps) =
  interface ITerminalElementDescriptor with
    member _.CreateViewTE() =
      new LegendAnnotationTerminalElement(props.props) :> IViewTE

    member _.Props = props.props

type Line(props: LineProps) =
  interface ITerminalElementDescriptor with
    member _.CreateViewTE() =
      new LineTerminalElement(props.props) :> IViewTE

    member _.Props = props.props

type LinearRange<'T>(props: LinearRangeProps<'T>) =
  interface ITerminalElementDescriptor with
    member _.CreateViewTE() =
      new LinearRangeTerminalElement<'T>(props.props) :> IViewTE

    member _.Props = props.props

type LinearRange(props: LinearRangeProps) =
  interface ITerminalElementDescriptor with
    member _.CreateViewTE() =
      new LinearRangeTerminalElement(props.props) :> IViewTE

    member _.Props = props.props

type Link(props: LinkProps) =
  interface ITerminalElementDescriptor with
    member _.CreateViewTE() =
      new LinkTerminalElement(props.props) :> IViewTE

    member _.Props = props.props

type ListView(props: ListViewProps) =
  interface ITerminalElementDescriptor with
    member _.CreateViewTE() =
      new ListViewTerminalElement(props.props) :> IViewTE

    member _.Props = props.props

type Margin(props: MarginProps) =
  interface ITerminalElementDescriptor with
    member _.CreateViewTE() =
      new MarginTerminalElement(props.props) :> IViewTE

    member _.Props = props.props

type Menu(props: MenuProps) =
  interface IMenuTerminalElement

  interface ITerminalElementDescriptor with
    member _.CreateViewTE() =
      new MenuTerminalElement(props.props) :> IViewTE

    member _.Props = props.props

type MenuBar(props: MenuBarProps) =
  interface IMenuTerminalElement

  interface ITerminalElementDescriptor with
    member _.CreateViewTE() =
      new MenuBarTerminalElement(props.props) :> IViewTE

    member _.Props = props.props

type NumericUpDown<'T>(props: NumericUpDownProps<'T>) =
  interface ITerminalElementDescriptor with
    member _.CreateViewTE() =
      new NumericUpDownTerminalElement<'T>(props.props) :> IViewTE

    member _.Props = props.props

type NumericUpDown(props: NumericUpDownProps) =
  interface ITerminalElementDescriptor with
    member _.CreateViewTE() =
      new NumericUpDownTerminalElement(props.props) :> IViewTE

    member _.Props = props.props

type Padding(props: PaddingProps) =
  interface ITerminalElementDescriptor with
    member _.CreateViewTE() =
      new PaddingTerminalElement(props.props) :> IViewTE

    member _.Props = props.props

type Popover<'TView, 'TResult when 'TView: (new: unit -> 'TView) and 'TView :> Terminal.Gui.ViewBase.View>
  (props: PopoverProps<'TView, 'TResult>) =
  interface ITerminalElementDescriptor with
    member _.CreateViewTE() =
      new PopoverTerminalElement<'TView, 'TResult>(props.props) :> IViewTE

    member _.Props = props.props

type PopoverMenu(props: PopoverMenuProps) =
  interface IPopoverMenuTerminalElement

  interface ITerminalElementDescriptor with
    member _.CreateViewTE() =
      new PopoverMenuTerminalElement(props.props) :> IViewTE

    member _.Props = props.props

type ProgressBar(props: ProgressBarProps) =
  interface ITerminalElementDescriptor with
    member _.CreateViewTE() =
      new ProgressBarTerminalElement(props.props) :> IViewTE

    member _.Props = props.props

type Runnable(props: RunnableProps) =
  interface ITerminalElementDescriptor with
    member _.CreateViewTE() =
      new RunnableTerminalElement(props.props) :> IViewTE

    member _.Props = props.props

type Runnable<'TResult>(props: RunnableProps<'TResult>) =
  interface ITerminalElementDescriptor with
    member _.CreateViewTE() =
      new RunnableTerminalElement<'TResult>(props.props) :> IViewTE

    member _.Props = props.props

type Dialog<'TResult>(props: DialogProps<'TResult>) =
  interface ITerminalElementDescriptor with
    member _.CreateViewTE() =
      new DialogTerminalElement<'TResult>(props.props) :> IViewTE

    member _.Props = props.props

type Dialog(props: DialogProps) =
  interface ITerminalElementDescriptor with
    member _.CreateViewTE() =
      new DialogTerminalElement(props.props) :> IViewTE

    member _.Props = props.props

type Prompt<'TView, 'TResult when 'TView: (new: unit -> 'TView) and 'TView :> Terminal.Gui.ViewBase.View>
  (props: PromptProps<'TView, 'TResult>) =
  interface ITerminalElementDescriptor with
    member _.CreateViewTE() =
      new PromptTerminalElement<'TView, 'TResult>(props.props) :> IViewTE

    member _.Props = props.props

type FileDialog(props: FileDialogProps) =
  interface ITerminalElementDescriptor with
    member _.CreateViewTE() =
      new FileDialogTerminalElement(props.props) :> IViewTE

    member _.Props = props.props

type OpenDialog(props: OpenDialogProps) =
  interface ITerminalElementDescriptor with
    member _.CreateViewTE() =
      new OpenDialogTerminalElement(props.props) :> IViewTE

    member _.Props = props.props

type SaveDialog(props: SaveDialogProps) =
  interface ITerminalElementDescriptor with
    member _.CreateViewTE() =
      new SaveDialogTerminalElement(props.props) :> IViewTE

    member _.Props = props.props

type ScrollBar(props: ScrollBarProps) =
  interface ITerminalElementDescriptor with
    member _.CreateViewTE() =
      new ScrollBarTerminalElement(props.props) :> IViewTE

    member _.Props = props.props

type ScrollSlider(props: ScrollSliderProps) =
  interface ITerminalElementDescriptor with
    member _.CreateViewTE() =
      new ScrollSliderTerminalElement(props.props) :> IViewTE

    member _.Props = props.props

type FlagSelector(props: FlagSelectorProps) =
  interface ITerminalElementDescriptor with
    member _.CreateViewTE() =
      new FlagSelectorTerminalElement(props.props) :> IViewTE

    member _.Props = props.props

type OptionSelector(props: OptionSelectorProps) =
  interface ITerminalElementDescriptor with
    member _.CreateViewTE() =
      new OptionSelectorTerminalElement(props.props) :> IViewTE

    member _.Props = props.props

type FlagSelector<'TFlagsEnum
  when 'TFlagsEnum: struct
  and 'TFlagsEnum: (new: unit -> 'TFlagsEnum)
  and 'TFlagsEnum :> System.Enum
  and 'TFlagsEnum :> System.ValueType>(props: FlagSelectorProps<'TFlagsEnum>) =
  interface ITerminalElementDescriptor with
    member _.CreateViewTE() =
      new FlagSelectorTerminalElement<'TFlagsEnum>(props.props) :> IViewTE

    member _.Props = props.props

type OptionSelector<'TEnum
  when 'TEnum: struct and 'TEnum: (new: unit -> 'TEnum) and 'TEnum :> System.Enum and 'TEnum :> System.ValueType>
  (props: OptionSelectorProps<'TEnum>) =
  interface ITerminalElementDescriptor with
    member _.CreateViewTE() =
      new OptionSelectorTerminalElement<'TEnum>(props.props) :> IViewTE

    member _.Props = props.props

type Shortcut(props: ShortcutProps) =
  interface ITerminalElementDescriptor with
    member _.CreateViewTE() =
      new ShortcutTerminalElement(props.props) :> IViewTE

    member _.Props = props.props

type MenuItem(props: MenuItemProps) =
  interface IMenuItemTerminalElement

  interface ITerminalElementDescriptor with
    member _.CreateViewTE() =
      new MenuItemTerminalElement(props.props) :> IViewTE

    member _.Props = props.props

type MenuBarItem(props: MenuBarItemProps) =
  interface IMenuItemTerminalElement

  interface ITerminalElementDescriptor with
    member _.CreateViewTE() =
      new MenuBarItemTerminalElement(props.props) :> IViewTE

    member _.Props = props.props

type SpinnerView(props: SpinnerViewProps) =
  interface ITerminalElementDescriptor with
    member _.CreateViewTE() =
      new SpinnerViewTerminalElement(props.props) :> IViewTE

    member _.Props = props.props

type StatusBar(props: StatusBarProps) =
  interface ITerminalElementDescriptor with
    member _.CreateViewTE() =
      new StatusBarTerminalElement(props.props) :> IViewTE

    member _.Props = props.props

type Tab(props: TabProps) =
  interface ITabTerminalElement

  interface ITerminalElementDescriptor with
    member _.CreateViewTE() =
      new TabTerminalElement(props.props) :> IViewTE

    member _.Props = props.props

type TabView(props: TabViewProps) =
  interface ITerminalElementDescriptor with
    member _.CreateViewTE() =
      new TabViewTerminalElement(props.props) :> IViewTE

    member _.Props = props.props

type TableView(props: TableViewProps) =
  interface ITerminalElementDescriptor with
    member _.CreateViewTE() =
      new TableViewTerminalElement(props.props) :> IViewTE

    member _.Props = props.props

type TextField(props: TextFieldProps) =
  interface ITerminalElementDescriptor with
    member _.CreateViewTE() =
      new TextFieldTerminalElement(props.props) :> IViewTE

    member _.Props = props.props

type DropDownList(props: DropDownListProps) =
  interface ITerminalElementDescriptor with
    member _.CreateViewTE() =
      new DropDownListTerminalElement(props.props) :> IViewTE

    member _.Props = props.props

type TextValidateField(props: TextValidateFieldProps) =
  interface ITerminalElementDescriptor with
    member _.CreateViewTE() =
      new TextValidateFieldTerminalElement(props.props) :> IViewTE

    member _.Props = props.props

type DateEditor(props: DateEditorProps) =
  interface ITerminalElementDescriptor with
    member _.CreateViewTE() =
      new DateEditorTerminalElement(props.props) :> IViewTE

    member _.Props = props.props

type TextView(props: TextViewProps) =
  interface ITerminalElementDescriptor with
    member _.CreateViewTE() =
      new TextViewTerminalElement(props.props) :> IViewTE

    member _.Props = props.props

type TimeEditor(props: TimeEditorProps) =
  interface ITerminalElementDescriptor with
    member _.CreateViewTE() =
      new TimeEditorTerminalElement(props.props) :> IViewTE

    member _.Props = props.props

type TreeView<'T when 'T: not struct>(props: TreeViewProps<'T>) =
  interface ITerminalElementDescriptor with
    member _.CreateViewTE() =
      new TreeViewTerminalElement<'T>(props.props) :> IViewTE

    member _.Props = props.props

type TreeView(props: TreeViewProps) =
  interface ITerminalElementDescriptor with
    member _.CreateViewTE() =
      new TreeViewTerminalElement(props.props) :> IViewTE

    member _.Props = props.props

type Window(props: WindowProps) =
  interface ITerminalElementDescriptor with
    member _.CreateViewTE() =
      new WindowTerminalElement(props.props) :> IViewTE

    member _.Props = props.props

type Wizard(props: WizardProps) =
  interface ITerminalElementDescriptor with
    member _.CreateViewTE() =
      new WizardTerminalElement(props.props) :> IViewTE

    member _.Props = props.props

type WizardStep(props: WizardStepProps) =
  interface IWizardStepTerminalElement

  interface ITerminalElementDescriptor with
    member _.CreateViewTE() =
      new WizardStepTerminalElement(props.props) :> IViewTE

    member _.Props = props.props
