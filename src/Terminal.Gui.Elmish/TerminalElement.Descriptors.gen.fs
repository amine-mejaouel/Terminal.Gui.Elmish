namespace Terminal.Gui.Elmish

type Adornment(props: AdornmentProps) =
  let viewTe = lazy (new AdornmentTerminalElement(props.props))

  interface IViewBase with
    member _.CreateViewTE() = viewTe.Value
    member _.Props = props.props
    member _.ViewType = ViewType.``ViewBaseAdornment``

type AttributePicker(props: AttributePickerProps) =
  let viewTe = lazy (new AttributePickerTerminalElement(props.props))

  interface IViewBase with
    member _.CreateViewTE() = viewTe.Value
    member _.Props = props.props
    member _.ViewType = ViewType.``ViewsAttributePicker``

type Bar(props: BarProps) =
  let viewTe = lazy (new BarTerminalElement(props.props))

  interface IViewBase with
    member _.CreateViewTE() = viewTe.Value
    member _.Props = props.props
    member _.ViewType = ViewType.``ViewsBar``

type Border(props: BorderProps) =
  let viewTe = lazy (new BorderTerminalElement(props.props))

  interface IViewBase with
    member _.CreateViewTE() = viewTe.Value
    member _.Props = props.props
    member _.ViewType = ViewType.``ViewBaseBorder``

type Button(props: ButtonProps) =
  let viewTe = lazy (new ButtonTerminalElement(props.props))

  interface IViewBase with
    member _.CreateViewTE() = viewTe.Value
    member _.Props = props.props
    member _.ViewType = ViewType.``ViewsButton``

type CharMap(props: CharMapProps) =
  let viewTe = lazy (new CharMapTerminalElement(props.props))

  interface IViewBase with
    member _.CreateViewTE() = viewTe.Value
    member _.Props = props.props
    member _.ViewType = ViewType.``ViewsCharMap``

type CheckBox(props: CheckBoxProps) =
  let viewTe = lazy (new CheckBoxTerminalElement(props.props))

  interface IViewBase with
    member _.CreateViewTE() = viewTe.Value
    member _.Props = props.props
    member _.ViewType = ViewType.``ViewsCheckBox``

type ColorPicker(props: ColorPickerProps) =
  let viewTe = lazy (new ColorPickerTerminalElement(props.props))

  interface IViewBase with
    member _.CreateViewTE() = viewTe.Value
    member _.Props = props.props
    member _.ViewType = ViewType.``ViewsColorPicker``

type ColorPicker16(props: ColorPicker16Props) =
  let viewTe = lazy (new ColorPicker16TerminalElement(props.props))

  interface IViewBase with
    member _.CreateViewTE() = viewTe.Value
    member _.Props = props.props
    member _.ViewType = ViewType.``ViewsColorPicker16``

type DatePicker(props: DatePickerProps) =
  let viewTe = lazy (new DatePickerTerminalElement(props.props))

  interface IViewBase with
    member _.CreateViewTE() = viewTe.Value
    member _.Props = props.props
    member _.ViewType = ViewType.``ViewsDatePicker``

type FrameView(props: FrameViewProps) =
  let viewTe = lazy (new FrameViewTerminalElement(props.props))

  interface IViewBase with
    member _.CreateViewTE() = viewTe.Value
    member _.Props = props.props
    member _.ViewType = ViewType.``ViewsFrameView``

type GraphView(props: GraphViewProps) =
  let viewTe = lazy (new GraphViewTerminalElement(props.props))

  interface IViewBase with
    member _.CreateViewTE() = viewTe.Value
    member _.Props = props.props
    member _.ViewType = ViewType.``ViewsGraphView``

type HexView(props: HexViewProps) =
  let viewTe = lazy (new HexViewTerminalElement(props.props))

  interface IViewBase with
    member _.CreateViewTE() = viewTe.Value
    member _.Props = props.props
    member _.ViewType = ViewType.``ViewsHexView``

type Label(props: LabelProps) =
  let viewTe = lazy (new LabelTerminalElement(props.props))

  interface IViewBase with
    member _.CreateViewTE() = viewTe.Value
    member _.Props = props.props
    member _.ViewType = ViewType.``ViewsLabel``

type LegendAnnotation(props: LegendAnnotationProps) =
  let viewTe = lazy (new LegendAnnotationTerminalElement(props.props))

  interface IViewBase with
    member _.CreateViewTE() = viewTe.Value
    member _.Props = props.props
    member _.ViewType = ViewType.``ViewsLegendAnnotation``

type Line(props: LineProps) =
  let viewTe = lazy (new LineTerminalElement(props.props))

  interface IViewBase with
    member _.CreateViewTE() = viewTe.Value
    member _.Props = props.props
    member _.ViewType = ViewType.``ViewsLine``

type LinearRange<'T>(props: LinearRangeProps<'T>) =
  let viewTe = lazy (new LinearRangeTerminalElement<'T>(props.props))

  interface IViewBase with
    member _.CreateViewTE() = viewTe.Value
    member _.Props = props.props
    member _.ViewType = ViewType.``LinearRange<'T>``

type LinearRange(props: LinearRangeProps) =
  let viewTe = lazy (new LinearRangeTerminalElement(props.props))

  interface IViewBase with
    member _.CreateViewTE() = viewTe.Value
    member _.Props = props.props
    member _.ViewType = ViewType.``ViewsLinearRange``

type Link(props: LinkProps) =
  let viewTe = lazy (new LinkTerminalElement(props.props))

  interface IViewBase with
    member _.CreateViewTE() = viewTe.Value
    member _.Props = props.props
    member _.ViewType = ViewType.``ViewsLink``

type ListView(props: ListViewProps) =
  let viewTe = lazy (new ListViewTerminalElement(props.props))

  interface IViewBase with
    member _.CreateViewTE() = viewTe.Value
    member _.Props = props.props
    member _.ViewType = ViewType.``ViewsListView``

type Margin(props: MarginProps) =
  let viewTe = lazy (new MarginTerminalElement(props.props))

  interface IViewBase with
    member _.CreateViewTE() = viewTe.Value
    member _.Props = props.props
    member _.ViewType = ViewType.``ViewBaseMargin``

type Menu(props: MenuProps) =
  let viewTe = lazy (new MenuTerminalElement(props.props))

  interface IViewBase with
    member _.CreateViewTE() = viewTe.Value
    member _.Props = props.props
    member _.ViewType = ViewType.``ViewsMenu``

  interface IMenuView

type MenuBar(props: MenuBarProps) =
  let viewTe = lazy (new MenuBarTerminalElement(props.props))

  interface IViewBase with
    member _.CreateViewTE() = viewTe.Value
    member _.Props = props.props
    member _.ViewType = ViewType.``ViewsMenuBar``

  interface IMenuView

type NumericUpDown<'T>(props: NumericUpDownProps<'T>) =
  let viewTe = lazy (new NumericUpDownTerminalElement<'T>(props.props))

  interface IViewBase with
    member _.CreateViewTE() = viewTe.Value
    member _.Props = props.props
    member _.ViewType = ViewType.``NumericUpDown<'T>``

type NumericUpDown(props: NumericUpDownProps) =
  let viewTe = lazy (new NumericUpDownTerminalElement(props.props))

  interface IViewBase with
    member _.CreateViewTE() = viewTe.Value
    member _.Props = props.props
    member _.ViewType = ViewType.``ViewsNumericUpDown``

type Padding(props: PaddingProps) =
  let viewTe = lazy (new PaddingTerminalElement(props.props))

  interface IViewBase with
    member _.CreateViewTE() = viewTe.Value
    member _.Props = props.props
    member _.ViewType = ViewType.``ViewBasePadding``

type Popover<'TView, 'TResult when 'TView: (new: unit -> 'TView) and 'TView :> Terminal.Gui.ViewBase.View>
  (props: PopoverProps<'TView, 'TResult>) =
  let viewTe = lazy (new PopoverTerminalElement<'TView, 'TResult>(props.props))

  interface IViewBase with
    member _.CreateViewTE() = viewTe.Value
    member _.Props = props.props
    member _.ViewType = ViewType.``Popover<'TView, 'TResult>``

type PopoverMenu(props: PopoverMenuProps) =
  let viewTe = lazy (new PopoverMenuTerminalElement(props.props))

  interface IViewBase with
    member _.CreateViewTE() = viewTe.Value
    member _.Props = props.props
    member _.ViewType = ViewType.``ViewsPopoverMenu``

  interface IPopoverMenuView

type ProgressBar(props: ProgressBarProps) =
  let viewTe = lazy (new ProgressBarTerminalElement(props.props))

  interface IViewBase with
    member _.CreateViewTE() = viewTe.Value
    member _.Props = props.props
    member _.ViewType = ViewType.``ViewsProgressBar``

type Runnable(props: RunnableProps) =
  let viewTe = lazy (new RunnableTerminalElement(props.props))

  interface IViewBase with
    member _.CreateViewTE() = viewTe.Value
    member _.Props = props.props
    member _.ViewType = ViewType.``ViewsRunnable``

type Runnable<'TResult>(props: RunnableProps<'TResult>) =
  let viewTe = lazy (new RunnableTerminalElement<'TResult>(props.props))

  interface IViewBase with
    member _.CreateViewTE() = viewTe.Value
    member _.Props = props.props
    member _.ViewType = ViewType.``Runnable<'TResult>``

type Dialog<'TResult>(props: DialogProps<'TResult>) =
  let viewTe = lazy (new DialogTerminalElement<'TResult>(props.props))

  interface IViewBase with
    member _.CreateViewTE() = viewTe.Value
    member _.Props = props.props
    member _.ViewType = ViewType.``Dialog<'TResult>``

type Dialog(props: DialogProps) =
  let viewTe = lazy (new DialogTerminalElement(props.props))

  interface IViewBase with
    member _.CreateViewTE() = viewTe.Value
    member _.Props = props.props
    member _.ViewType = ViewType.``ViewsDialog``

type Prompt<'TView, 'TResult when 'TView: (new: unit -> 'TView) and 'TView :> Terminal.Gui.ViewBase.View>
  (props: PromptProps<'TView, 'TResult>) =
  let viewTe = lazy (new PromptTerminalElement<'TView, 'TResult>(props.props))

  interface IViewBase with
    member _.CreateViewTE() = viewTe.Value
    member _.Props = props.props
    member _.ViewType = ViewType.``Prompt<'TView, 'TResult>``

type FileDialog(props: FileDialogProps) =
  let viewTe = lazy (new FileDialogTerminalElement(props.props))

  interface IViewBase with
    member _.CreateViewTE() = viewTe.Value
    member _.Props = props.props
    member _.ViewType = ViewType.``ViewsFileDialog``

type OpenDialog(props: OpenDialogProps) =
  let viewTe = lazy (new OpenDialogTerminalElement(props.props))

  interface IViewBase with
    member _.CreateViewTE() = viewTe.Value
    member _.Props = props.props
    member _.ViewType = ViewType.``ViewsOpenDialog``

type SaveDialog(props: SaveDialogProps) =
  let viewTe = lazy (new SaveDialogTerminalElement(props.props))

  interface IViewBase with
    member _.CreateViewTE() = viewTe.Value
    member _.Props = props.props
    member _.ViewType = ViewType.``ViewsSaveDialog``

type ScrollBar(props: ScrollBarProps) =
  let viewTe = lazy (new ScrollBarTerminalElement(props.props))

  interface IViewBase with
    member _.CreateViewTE() = viewTe.Value
    member _.Props = props.props
    member _.ViewType = ViewType.``ViewsScrollBar``

type ScrollSlider(props: ScrollSliderProps) =
  let viewTe = lazy (new ScrollSliderTerminalElement(props.props))

  interface IViewBase with
    member _.CreateViewTE() = viewTe.Value
    member _.Props = props.props
    member _.ViewType = ViewType.``ViewsScrollSlider``

type FlagSelector(props: FlagSelectorProps) =
  let viewTe = lazy (new FlagSelectorTerminalElement(props.props))

  interface IViewBase with
    member _.CreateViewTE() = viewTe.Value
    member _.Props = props.props
    member _.ViewType = ViewType.``ViewsFlagSelector``

type OptionSelector(props: OptionSelectorProps) =
  let viewTe = lazy (new OptionSelectorTerminalElement(props.props))

  interface IViewBase with
    member _.CreateViewTE() = viewTe.Value
    member _.Props = props.props
    member _.ViewType = ViewType.``ViewsOptionSelector``

type FlagSelector<'TFlagsEnum
  when 'TFlagsEnum: struct
  and 'TFlagsEnum: (new: unit -> 'TFlagsEnum)
  and 'TFlagsEnum :> System.Enum
  and 'TFlagsEnum :> System.ValueType>(props: FlagSelectorProps<'TFlagsEnum>) =
  let viewTe = lazy (new FlagSelectorTerminalElement<'TFlagsEnum>(props.props))

  interface IViewBase with
    member _.CreateViewTE() = viewTe.Value
    member _.Props = props.props
    member _.ViewType = ViewType.``FlagSelector<'TFlagsEnum>``

type OptionSelector<'TEnum
  when 'TEnum: struct and 'TEnum: (new: unit -> 'TEnum) and 'TEnum :> System.Enum and 'TEnum :> System.ValueType>
  (props: OptionSelectorProps<'TEnum>) =
  let viewTe = lazy (new OptionSelectorTerminalElement<'TEnum>(props.props))

  interface IViewBase with
    member _.CreateViewTE() = viewTe.Value
    member _.Props = props.props
    member _.ViewType = ViewType.``OptionSelector<'TEnum>``

type Shortcut(props: ShortcutProps) =
  let viewTe = lazy (new ShortcutTerminalElement(props.props))

  interface IViewBase with
    member _.CreateViewTE() = viewTe.Value
    member _.Props = props.props
    member _.ViewType = ViewType.``ViewsShortcut``

type MenuItem(props: MenuItemProps) =
  let viewTe = lazy (new MenuItemTerminalElement(props.props))

  interface IViewBase with
    member _.CreateViewTE() = viewTe.Value
    member _.Props = props.props
    member _.ViewType = ViewType.``ViewsMenuItem``

  interface IMenuItemView

type MenuBarItem(props: MenuBarItemProps) =
  let viewTe = lazy (new MenuBarItemTerminalElement(props.props))

  interface IViewBase with
    member _.CreateViewTE() = viewTe.Value
    member _.Props = props.props
    member _.ViewType = ViewType.``ViewsMenuBarItem``

  interface IMenuItemView

type SpinnerView(props: SpinnerViewProps) =
  let viewTe = lazy (new SpinnerViewTerminalElement(props.props))

  interface IViewBase with
    member _.CreateViewTE() = viewTe.Value
    member _.Props = props.props
    member _.ViewType = ViewType.``ViewsSpinnerView``

type StatusBar(props: StatusBarProps) =
  let viewTe = lazy (new StatusBarTerminalElement(props.props))

  interface IViewBase with
    member _.CreateViewTE() = viewTe.Value
    member _.Props = props.props
    member _.ViewType = ViewType.``ViewsStatusBar``

type Tab(props: TabProps) =
  let viewTe = lazy (new TabTerminalElement(props.props))

  interface IViewBase with
    member _.CreateViewTE() = viewTe.Value
    member _.Props = props.props
    member _.ViewType = ViewType.``ViewsTab``

  interface ITabView

type TabView(props: TabViewProps) =
  let viewTe = lazy (new TabViewTerminalElement(props.props))

  interface IViewBase with
    member _.CreateViewTE() = viewTe.Value
    member _.Props = props.props
    member _.ViewType = ViewType.``ViewsTabView``

type TableView(props: TableViewProps) =
  let viewTe = lazy (new TableViewTerminalElement(props.props))

  interface IViewBase with
    member _.CreateViewTE() = viewTe.Value
    member _.Props = props.props
    member _.ViewType = ViewType.``ViewsTableView``

type TextField(props: TextFieldProps) =
  let viewTe = lazy (new TextFieldTerminalElement(props.props))

  interface IViewBase with
    member _.CreateViewTE() = viewTe.Value
    member _.Props = props.props
    member _.ViewType = ViewType.``ViewsTextField``

type DropDownList(props: DropDownListProps) =
  let viewTe = lazy (new DropDownListTerminalElement(props.props))

  interface IViewBase with
    member _.CreateViewTE() = viewTe.Value
    member _.Props = props.props
    member _.ViewType = ViewType.``ViewsDropDownList``

type TextValidateField(props: TextValidateFieldProps) =
  let viewTe = lazy (new TextValidateFieldTerminalElement(props.props))

  interface IViewBase with
    member _.CreateViewTE() = viewTe.Value
    member _.Props = props.props
    member _.ViewType = ViewType.``ViewsTextValidateField``

type DateEditor(props: DateEditorProps) =
  let viewTe = lazy (new DateEditorTerminalElement(props.props))

  interface IViewBase with
    member _.CreateViewTE() = viewTe.Value
    member _.Props = props.props
    member _.ViewType = ViewType.``ViewsDateEditor``

type TextView(props: TextViewProps) =
  let viewTe = lazy (new TextViewTerminalElement(props.props))

  interface IViewBase with
    member _.CreateViewTE() = viewTe.Value
    member _.Props = props.props
    member _.ViewType = ViewType.``ViewsTextView``

type TimeEditor(props: TimeEditorProps) =
  let viewTe = lazy (new TimeEditorTerminalElement(props.props))

  interface IViewBase with
    member _.CreateViewTE() = viewTe.Value
    member _.Props = props.props
    member _.ViewType = ViewType.``ViewsTimeEditor``

type TreeView<'T when 'T: not struct>(props: TreeViewProps<'T>) =
  let viewTe = lazy (new TreeViewTerminalElement<'T>(props.props))

  interface IViewBase with
    member _.CreateViewTE() = viewTe.Value
    member _.Props = props.props
    member _.ViewType = ViewType.``TreeView<'T>``

type TreeView(props: TreeViewProps) =
  let viewTe = lazy (new TreeViewTerminalElement(props.props))

  interface IViewBase with
    member _.CreateViewTE() = viewTe.Value
    member _.Props = props.props
    member _.ViewType = ViewType.``ViewsTreeView``

type Window(props: WindowProps) =
  let viewTe = lazy (new WindowTerminalElement(props.props))

  interface IViewBase with
    member _.CreateViewTE() = viewTe.Value
    member _.Props = props.props
    member _.ViewType = ViewType.``ViewsWindow``

type Wizard(props: WizardProps) =
  let viewTe = lazy (new WizardTerminalElement(props.props))

  interface IViewBase with
    member _.CreateViewTE() = viewTe.Value
    member _.Props = props.props
    member _.ViewType = ViewType.``ViewsWizard``

type WizardStep(props: WizardStepProps) =
  let viewTe = lazy (new WizardStepTerminalElement(props.props))

  interface IViewBase with
    member _.CreateViewTE() = viewTe.Value
    member _.Props = props.props
    member _.ViewType = ViewType.``ViewsWizardStep``

  interface IWizardStepView
