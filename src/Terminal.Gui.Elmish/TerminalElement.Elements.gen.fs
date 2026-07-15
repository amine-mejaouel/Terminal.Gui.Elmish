namespace Terminal.Gui.Elmish

open System.Collections.Generic
open Terminal.Gui.App
open Terminal.Gui.ViewBase
open Terminal.Gui.Views


type internal ViewTerminalElement(props: Props) =
  inherit ViewBackedTerminalElement(props)

  override _.Name = "View"

  override _.NewView() = new View()

  override _.SetAsChildOfParentView = true

  override this.SubElements_PropKeys =
    [ PKey.View.DefaultAcceptView_viewSpec.Untyped ]
    |> List.append base.SubElements_PropKeys

  override _.SetProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    ViewPropHandler.setProps (terminalElement, props)

  override _.RemoveProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    ViewPropHandler.removeProps (terminalElement, props)

type internal AdornmentViewTerminalElement(props: Props) =
  inherit ViewTerminalElement(props)

  override _.Name = "AdornmentView"

  override _.NewView() = new AdornmentView()

  override _.SetAsChildOfParentView = true

  override _.SetProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    AdornmentViewPropHandler.setProps (terminalElement, props)

  override _.RemoveProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    AdornmentViewPropHandler.removeProps (terminalElement, props)

type internal AttributePickerTerminalElement(props: Props) =
  inherit ViewTerminalElement(props)

  override _.Name = "AttributePicker"

  override _.NewView() = new AttributePicker()

  override _.SetAsChildOfParentView = true

  override _.SetProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    AttributePickerPropHandler.setProps (terminalElement, props)

  override _.RemoveProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    AttributePickerPropHandler.removeProps (terminalElement, props)

type internal BarTerminalElement(props: Props) =
  inherit ViewTerminalElement(props)

  override _.Name = "Bar"

  override _.NewView() = new Bar()

  override _.SetAsChildOfParentView = true

  override _.SetProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    BarPropHandler.setProps (terminalElement, props)

  override _.RemoveProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    BarPropHandler.removeProps (terminalElement, props)

type internal BorderViewTerminalElement(props: Props) =
  inherit AdornmentViewTerminalElement(props)

  override _.Name = "BorderView"

  override _.NewView() = new BorderView()

  override _.SetAsChildOfParentView = true

  override _.SetProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    BorderViewPropHandler.setProps (terminalElement, props)

  override _.RemoveProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    BorderViewPropHandler.removeProps (terminalElement, props)

type internal ButtonTerminalElement(props: Props) =
  inherit ViewTerminalElement(props)

  override _.Name = "Button"

  override _.NewView() = new Button()

  override _.SetAsChildOfParentView = true

  override _.SetProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    ButtonPropHandler.setProps (terminalElement, props)

  override _.RemoveProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    ButtonPropHandler.removeProps (terminalElement, props)

type internal CharMapTerminalElement(props: Props) =
  inherit ViewTerminalElement(props)

  override _.Name = "CharMap"

  override _.NewView() = new CharMap()

  override _.SetAsChildOfParentView = true

  override _.SetProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    CharMapPropHandler.setProps (terminalElement, props)

  override _.RemoveProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    CharMapPropHandler.removeProps (terminalElement, props)

type internal CheckBoxTerminalElement(props: Props) =
  inherit ViewTerminalElement(props)

  override _.Name = "CheckBox"

  override _.NewView() = new CheckBox()

  override _.SetAsChildOfParentView = true

  override _.SetProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    CheckBoxPropHandler.setProps (terminalElement, props)

  override _.RemoveProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    CheckBoxPropHandler.removeProps (terminalElement, props)

type internal CodeTerminalElement(props: Props) =
  inherit ViewTerminalElement(props)

  override _.Name = "Code"

  override _.NewView() = new Code()

  override _.SetAsChildOfParentView = true

  override _.SetProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    CodePropHandler.setProps (terminalElement, props)

  override _.RemoveProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    CodePropHandler.removeProps (terminalElement, props)

type internal ColorPickerTerminalElement(props: Props) =
  inherit ViewTerminalElement(props)

  override _.Name = "ColorPicker"

  override _.NewView() = new ColorPicker()

  override _.SetAsChildOfParentView = true

  override _.SetProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    ColorPickerPropHandler.setProps (terminalElement, props)

  override _.RemoveProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    ColorPickerPropHandler.removeProps (terminalElement, props)

type internal ColorPicker16TerminalElement(props: Props) =
  inherit ViewTerminalElement(props)

  override _.Name = "ColorPicker16"

  override _.NewView() = new ColorPicker16()

  override _.SetAsChildOfParentView = true

  override _.SetProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    ColorPicker16PropHandler.setProps (terminalElement, props)

  override _.RemoveProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    ColorPicker16PropHandler.removeProps (terminalElement, props)

type internal DatePickerTerminalElement(props: Props) =
  inherit ViewTerminalElement(props)

  override _.Name = "DatePicker"

  override _.NewView() = new DatePicker()

  override _.SetAsChildOfParentView = true

  override _.SetProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    DatePickerPropHandler.setProps (terminalElement, props)

  override _.RemoveProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    DatePickerPropHandler.removeProps (terminalElement, props)

type internal FrameViewTerminalElement(props: Props) =
  inherit ViewTerminalElement(props)

  override _.Name = "FrameView"

  override _.NewView() = new FrameView()

  override _.SetAsChildOfParentView = true

  override _.SetProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    FrameViewPropHandler.setProps (terminalElement, props)

  override _.RemoveProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    FrameViewPropHandler.removeProps (terminalElement, props)

type internal GraphViewTerminalElement(props: Props) =
  inherit ViewTerminalElement(props)

  override _.Name = "GraphView"

  override _.NewView() = new GraphView()

  override _.SetAsChildOfParentView = true

  override _.SetProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    GraphViewPropHandler.setProps (terminalElement, props)

  override _.RemoveProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    GraphViewPropHandler.removeProps (terminalElement, props)

type internal HexViewTerminalElement(props: Props) =
  inherit ViewTerminalElement(props)

  override _.Name = "HexView"

  override _.NewView() = new HexView()

  override _.SetAsChildOfParentView = true

  override _.SetProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    HexViewPropHandler.setProps (terminalElement, props)

  override _.RemoveProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    HexViewPropHandler.removeProps (terminalElement, props)

type internal ImageViewTerminalElement(props: Props) =
  inherit ViewTerminalElement(props)

  override _.Name = "ImageView"

  override _.NewView() = new ImageView()

  override _.SetAsChildOfParentView = true

  override _.SetProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    ImageViewPropHandler.setProps (terminalElement, props)

  override _.RemoveProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    ImageViewPropHandler.removeProps (terminalElement, props)

type internal LabelTerminalElement(props: Props) =
  inherit ViewTerminalElement(props)

  override _.Name = "Label"

  override _.NewView() = new Label()

  override _.SetAsChildOfParentView = true

  override _.SetProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    LabelPropHandler.setProps (terminalElement, props)

  override _.RemoveProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    LabelPropHandler.removeProps (terminalElement, props)

type internal LegendAnnotationTerminalElement(props: Props) =
  inherit ViewTerminalElement(props)

  override _.Name = "LegendAnnotation"

  override _.NewView() = new LegendAnnotation()

  override _.SetAsChildOfParentView = true

  override _.SetProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    LegendAnnotationPropHandler.setProps (terminalElement, props)

  override _.RemoveProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    LegendAnnotationPropHandler.removeProps (terminalElement, props)

type internal LineTerminalElement(props: Props) =
  inherit ViewTerminalElement(props)

  override _.Name = "Line"

  override _.NewView() = new Line()

  override _.SetAsChildOfParentView = true

  override _.SetProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    LinePropHandler.setProps (terminalElement, props)

  override _.RemoveProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    LinePropHandler.removeProps (terminalElement, props)

[<AbstractClass>]
type internal LinearRangeViewBaseTerminalElement<'TOption, 'TValue>(props: Props) =
  inherit ViewTerminalElement(props)

  override _.Name = "LinearRangeViewBase`2"

  override _.NewView() =
    failwith "Cannot instantiate abstract view type LinearRangeViewBase"

  override _.SetAsChildOfParentView = true

  override _.SetProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    LinearRangeViewBasePropHandler<'TOption, 'TValue>.setProps (terminalElement, props)

  override _.RemoveProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    LinearRangeViewBasePropHandler<'TOption, 'TValue>.removeProps (terminalElement, props)

type internal LinearMultiSelectorTerminalElement<'T>(props: Props) =
  inherit LinearRangeViewBaseTerminalElement<'T, IReadOnlyList<'T>>(props)

  override _.Name = "LinearMultiSelector`1"

  override _.NewView() = new LinearMultiSelector<'T>()

  override _.SetAsChildOfParentView = true

  override _.SetProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    LinearMultiSelectorPropHandler<'T>.setProps (terminalElement, props)

  override _.RemoveProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    LinearMultiSelectorPropHandler<'T>.removeProps (terminalElement, props)

type internal LinearMultiSelectorTerminalElement(props: Props) =
  inherit LinearMultiSelectorTerminalElement<string>(props)

  override _.Name = "LinearMultiSelector"

  override _.NewView() = new LinearMultiSelector()

  override _.SetAsChildOfParentView = true

  override _.SetProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    LinearMultiSelectorPropHandler.setProps (terminalElement, props)

  override _.RemoveProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    LinearMultiSelectorPropHandler.removeProps (terminalElement, props)

type internal LinearRangeTerminalElement<'T>(props: Props) =
  inherit LinearRangeViewBaseTerminalElement<'T, LinearRangeSpan<'T>>(props)

  override _.Name = "LinearRange`1"

  override _.NewView() = new LinearRange<'T>()

  override _.SetAsChildOfParentView = true

  override _.SetProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    LinearRangePropHandler<'T>.setProps (terminalElement, props)

  override _.RemoveProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    LinearRangePropHandler<'T>.removeProps (terminalElement, props)

type internal LinearRangeTerminalElement(props: Props) =
  inherit LinearRangeTerminalElement<string>(props)

  override _.Name = "LinearRange"

  override _.NewView() = new LinearRange()

  override _.SetAsChildOfParentView = true

  override _.SetProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    LinearRangePropHandler.setProps (terminalElement, props)

  override _.RemoveProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    LinearRangePropHandler.removeProps (terminalElement, props)

type internal LinearSelectorTerminalElement<'T>(props: Props) =
  inherit LinearRangeViewBaseTerminalElement<'T, 'T>(props)

  override _.Name = "LinearSelector`1"

  override _.NewView() = new LinearSelector<'T>()

  override _.SetAsChildOfParentView = true

  override _.SetProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    LinearSelectorPropHandler<'T>.setProps (terminalElement, props)

  override _.RemoveProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    LinearSelectorPropHandler<'T>.removeProps (terminalElement, props)

type internal LinearSelectorTerminalElement(props: Props) =
  inherit LinearSelectorTerminalElement<string>(props)

  override _.Name = "LinearSelector"

  override _.NewView() = new LinearSelector()

  override _.SetAsChildOfParentView = true

  override _.SetProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    LinearSelectorPropHandler.setProps (terminalElement, props)

  override _.RemoveProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    LinearSelectorPropHandler.removeProps (terminalElement, props)

type internal LinkTerminalElement(props: Props) =
  inherit ViewTerminalElement(props)

  override _.Name = "Link"

  override _.NewView() = new Link()

  override _.SetAsChildOfParentView = true

  override _.SetProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    LinkPropHandler.setProps (terminalElement, props)

  override _.RemoveProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    LinkPropHandler.removeProps (terminalElement, props)

type internal ListViewTerminalElement(props: Props) =
  inherit ViewTerminalElement(props)

  override _.Name = "ListView"

  override _.NewView() = new ListView()

  override _.SetAsChildOfParentView = true

  override _.SetProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    ListViewPropHandler.setProps (terminalElement, props)

  override _.RemoveProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    ListViewPropHandler.removeProps (terminalElement, props)

type internal ListViewTerminalElement<'T>(props: Props) =
  inherit ListViewTerminalElement(props)

  override _.Name = "ListView`1"

  override _.NewView() = new ListView<'T>()

  override _.SetAsChildOfParentView = true

  override _.SetProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    ListViewPropHandler<'T>.setProps (terminalElement, props)

  override _.RemoveProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    ListViewPropHandler<'T>.removeProps (terminalElement, props)

type internal MarginViewTerminalElement(props: Props) =
  inherit AdornmentViewTerminalElement(props)

  override _.Name = "MarginView"

  override _.NewView() = new MarginView()

  override _.SetAsChildOfParentView = true

  override _.SetProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    MarginViewPropHandler.setProps (terminalElement, props)

  override _.RemoveProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    MarginViewPropHandler.removeProps (terminalElement, props)

type internal MarkdownTerminalElement(props: Props) =
  inherit ViewTerminalElement(props)

  override _.Name = "Markdown"

  override _.NewView() = new Markdown()

  override _.SetAsChildOfParentView = true

  override _.SetProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    MarkdownPropHandler.setProps (terminalElement, props)

  override _.RemoveProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    MarkdownPropHandler.removeProps (terminalElement, props)

type internal MarkdownCodeBlockTerminalElement(props: Props) =
  inherit ViewTerminalElement(props)

  override _.Name = "MarkdownCodeBlock"

  override _.NewView() = new MarkdownCodeBlock()

  override _.SetAsChildOfParentView = true

  override _.SetProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    MarkdownCodeBlockPropHandler.setProps (terminalElement, props)

  override _.RemoveProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    MarkdownCodeBlockPropHandler.removeProps (terminalElement, props)

type internal MarkdownTableTerminalElement(props: Props) =
  inherit ViewTerminalElement(props)

  override _.Name = "MarkdownTable"

  override _.NewView() = new MarkdownTable()

  override _.SetAsChildOfParentView = true

  override _.SetProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    MarkdownTablePropHandler.setProps (terminalElement, props)

  override _.RemoveProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    MarkdownTablePropHandler.removeProps (terminalElement, props)

type internal MenuTerminalElement(props: Props) =
  inherit BarTerminalElement(props)

  override _.Name = "Menu"

  override _.NewView() = new Menu()

  override _.SetAsChildOfParentView = false

  override this.SubElements_PropKeys =
    [ PKey.Menu.SuperMenuItem_viewSpec.Untyped; PKey.Menu.Value_viewSpec.Untyped ]
    |> List.append base.SubElements_PropKeys

  override _.SetProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    MenuPropHandler.setProps (terminalElement, props)

  override _.RemoveProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    MenuPropHandler.removeProps (terminalElement, props)

type internal MenuBarTerminalElement(props: Props) =
  inherit MenuTerminalElement(props)

  override _.Name = "MenuBar"

  override _.NewView() = new MenuBar()

  override _.SetAsChildOfParentView = true

  override _.SetProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    MenuBarPropHandler.setProps (terminalElement, props)

  override _.RemoveProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    MenuBarPropHandler.removeProps (terminalElement, props)

type internal NumericUpDownTerminalElement<'T>(props: Props) =
  inherit ViewTerminalElement(props)

  override _.Name = "NumericUpDown`1"

  override _.NewView() = new NumericUpDown<'T>()

  override _.SetAsChildOfParentView = true

  override _.SetProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    NumericUpDownPropHandler<'T>.setProps (terminalElement, props)

  override _.RemoveProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    NumericUpDownPropHandler<'T>.removeProps (terminalElement, props)

type internal NumericUpDownTerminalElement(props: Props) =
  inherit NumericUpDownTerminalElement<int>(props)

  override _.Name = "NumericUpDown"

  override _.NewView() = new NumericUpDown()

  override _.SetAsChildOfParentView = true

  override _.SetProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    NumericUpDownPropHandler.setProps (terminalElement, props)

  override _.RemoveProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    NumericUpDownPropHandler.removeProps (terminalElement, props)

type internal PaddingViewTerminalElement(props: Props) =
  inherit AdornmentViewTerminalElement(props)

  override _.Name = "PaddingView"

  override _.NewView() = new PaddingView()

  override _.SetAsChildOfParentView = true

  override _.SetProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    PaddingViewPropHandler.setProps (terminalElement, props)

  override _.RemoveProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    PaddingViewPropHandler.removeProps (terminalElement, props)

[<AbstractClass>]
type internal PopoverImplTerminalElement(props: Props) =
  inherit ViewTerminalElement(props)

  override _.Name = "PopoverImpl"

  override _.NewView() =
    failwith "Cannot instantiate abstract view type PopoverImpl"

  override _.SetAsChildOfParentView = false

  override _.SetProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    PopoverImplPropHandler.setProps (terminalElement, props)

  override _.RemoveProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    PopoverImplPropHandler.removeProps (terminalElement, props)

type internal PopoverTerminalElement<'TView, 'TResult
  when 'TView: (new: unit -> 'TView) and 'TView :> Terminal.Gui.ViewBase.View>(props: Props) =
  inherit PopoverImplTerminalElement(props)

  override _.Name = "Popover`2"

  override _.NewView() = new Popover<'TView, 'TResult>()

  override _.SetAsChildOfParentView = false

  override this.SubElements_PropKeys =
    [ PKey.Popover<'TView, 'TResult>.ContentView_viewSpec.Untyped ]
    |> List.append base.SubElements_PropKeys

  override _.SetProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    PopoverPropHandler<'TView, 'TResult>.setProps (terminalElement, props)

  override _.RemoveProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    PopoverPropHandler<'TView, 'TResult>.removeProps (terminalElement, props)

type internal PopoverMenuTerminalElement(props: Props) =
  inherit PopoverTerminalElement<Terminal.Gui.Views.Menu, Terminal.Gui.Views.MenuItem>(props)

  override _.Name = "PopoverMenu"

  override _.NewView() = new PopoverMenu()

  override _.SetAsChildOfParentView = false

  override this.SubElements_PropKeys =
    [ PKey.PopoverMenu.Root_viewSpec.Untyped ]
    |> List.append base.SubElements_PropKeys

  override _.SetProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    PopoverMenuPropHandler.setProps (terminalElement, props)

  override _.RemoveProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    PopoverMenuPropHandler.removeProps (terminalElement, props)

type internal ProgressBarTerminalElement(props: Props) =
  inherit ViewTerminalElement(props)

  override _.Name = "ProgressBar"

  override _.NewView() = new ProgressBar()

  override _.SetAsChildOfParentView = true

  override _.SetProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    ProgressBarPropHandler.setProps (terminalElement, props)

  override _.RemoveProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    ProgressBarPropHandler.removeProps (terminalElement, props)

type internal RunnableTerminalElement(props: Props) =
  inherit ViewTerminalElement(props)

  override _.Name = "Runnable"

  override _.NewView() = new Runnable()

  override _.SetAsChildOfParentView = true

  override _.SetProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    RunnablePropHandler.setProps (terminalElement, props)

  override _.RemoveProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    RunnablePropHandler.removeProps (terminalElement, props)

type internal RunnableTerminalElement<'TResult>(props: Props) =
  inherit RunnableTerminalElement(props)

  override _.Name = "Runnable`1"

  override _.NewView() = new Runnable<'TResult>()

  override _.SetAsChildOfParentView = true

  override _.SetProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    RunnablePropHandler<'TResult>.setProps (terminalElement, props)

  override _.RemoveProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    RunnablePropHandler<'TResult>.removeProps (terminalElement, props)

type internal DialogTerminalElement<'TResult>(props: Props) =
  inherit RunnableTerminalElement<'TResult>(props)

  override _.Name = "Dialog`1"

  override _.NewView() = new Dialog<'TResult>()

  override _.SetAsChildOfParentView = true

  override _.SetProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    DialogPropHandler<'TResult>.setProps (terminalElement, props)

  override _.RemoveProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    DialogPropHandler<'TResult>.removeProps (terminalElement, props)

type internal RunnableWrapperTerminalElement<'TView, 'TResult
  when 'TView: (new: unit -> 'TView) and 'TView :> Terminal.Gui.ViewBase.View>(props: Props) =
  inherit RunnableTerminalElement<'TResult>(props)

  override _.Name = "RunnableWrapper`2"

  override _.NewView() = new RunnableWrapper<'TView, 'TResult>()

  override _.SetAsChildOfParentView = true

  override _.SetProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    RunnableWrapperPropHandler<'TView, 'TResult>.setProps (terminalElement, props)

  override _.RemoveProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    RunnableWrapperPropHandler<'TView, 'TResult>.removeProps (terminalElement, props)

type internal DialogTerminalElement(props: Props) =
  inherit DialogTerminalElement<int>(props)

  override _.Name = "Dialog"

  override _.NewView() = new Dialog()

  override _.SetAsChildOfParentView = true

  override _.SetProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    DialogPropHandler.setProps (terminalElement, props)

  override _.RemoveProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    DialogPropHandler.removeProps (terminalElement, props)

type internal FileDialogTerminalElement(props: Props) =
  inherit DialogTerminalElement<IReadOnlyList<string>>(props)

  override _.Name = "FileDialog"

  override _.NewView() = new FileDialog()

  override _.SetAsChildOfParentView = true

  override _.SetProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    FileDialogPropHandler.setProps (terminalElement, props)

  override _.RemoveProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    FileDialogPropHandler.removeProps (terminalElement, props)

type internal PromptTerminalElement<'TView, 'TResult
  when 'TView: (new: unit -> 'TView) and 'TView :> Terminal.Gui.ViewBase.View>(props: Props) =
  inherit DialogTerminalElement<'TResult>(props)

  override _.Name = "Prompt`2"

  override _.NewView() = new Prompt<'TView, 'TResult>()

  override _.SetAsChildOfParentView = true

  override _.SetProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    PromptPropHandler<'TView, 'TResult>.setProps (terminalElement, props)

  override _.RemoveProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    PromptPropHandler<'TView, 'TResult>.removeProps (terminalElement, props)

type internal OpenDialogTerminalElement(props: Props) =
  inherit FileDialogTerminalElement(props)

  override _.Name = "OpenDialog"

  override _.NewView() = new OpenDialog()

  override _.SetAsChildOfParentView = true

  override _.SetProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    OpenDialogPropHandler.setProps (terminalElement, props)

  override _.RemoveProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    OpenDialogPropHandler.removeProps (terminalElement, props)

type internal SaveDialogTerminalElement(props: Props) =
  inherit FileDialogTerminalElement(props)

  override _.Name = "SaveDialog"

  override _.NewView() = new SaveDialog()

  override _.SetAsChildOfParentView = true

  override _.SetProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    SaveDialogPropHandler.setProps (terminalElement, props)

  override _.RemoveProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    SaveDialogPropHandler.removeProps (terminalElement, props)

type internal ScrollBarTerminalElement(props: Props) =
  inherit ViewTerminalElement(props)

  override _.Name = "ScrollBar"

  override _.NewView() = new ScrollBar()

  override _.SetAsChildOfParentView = true

  override _.SetProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    ScrollBarPropHandler.setProps (terminalElement, props)

  override _.RemoveProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    ScrollBarPropHandler.removeProps (terminalElement, props)

type internal ScrollButtonTerminalElement(props: Props) =
  inherit ButtonTerminalElement(props)

  override _.Name = "ScrollButton"

  override _.NewView() = new ScrollButton()

  override _.SetAsChildOfParentView = true

  override _.SetProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    ScrollButtonPropHandler.setProps (terminalElement, props)

  override _.RemoveProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    ScrollButtonPropHandler.removeProps (terminalElement, props)

type internal ScrollSliderTerminalElement(props: Props) =
  inherit ViewTerminalElement(props)

  override _.Name = "ScrollSlider"

  override _.NewView() = new ScrollSlider()

  override _.SetAsChildOfParentView = true

  override _.SetProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    ScrollSliderPropHandler.setProps (terminalElement, props)

  override _.RemoveProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    ScrollSliderPropHandler.removeProps (terminalElement, props)

[<AbstractClass>]
type internal SelectorBaseTerminalElement(props: Props) =
  inherit ViewTerminalElement(props)

  override _.Name = "SelectorBase"

  override _.NewView() =
    failwith "Cannot instantiate abstract view type SelectorBase"

  override _.SetAsChildOfParentView = true

  override _.SetProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    SelectorBasePropHandler.setProps (terminalElement, props)

  override _.RemoveProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    SelectorBasePropHandler.removeProps (terminalElement, props)

type internal FlagSelectorTerminalElement(props: Props) =
  inherit SelectorBaseTerminalElement(props)

  override _.Name = "FlagSelector"

  override _.NewView() = new FlagSelector()

  override _.SetAsChildOfParentView = true

  override _.SetProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    FlagSelectorPropHandler.setProps (terminalElement, props)

  override _.RemoveProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    FlagSelectorPropHandler.removeProps (terminalElement, props)

type internal OptionSelectorTerminalElement(props: Props) =
  inherit SelectorBaseTerminalElement(props)

  override _.Name = "OptionSelector"

  override _.NewView() = new OptionSelector()

  override _.SetAsChildOfParentView = true

  override _.SetProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    OptionSelectorPropHandler.setProps (terminalElement, props)

  override _.RemoveProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    OptionSelectorPropHandler.removeProps (terminalElement, props)

type internal FlagSelectorTerminalElement<'TFlagsEnum
  when 'TFlagsEnum: struct
  and 'TFlagsEnum: (new: unit -> 'TFlagsEnum)
  and 'TFlagsEnum :> System.Enum
  and 'TFlagsEnum :> System.ValueType>(props: Props) =
  inherit FlagSelectorTerminalElement(props)

  override _.Name = "FlagSelector`1"

  override _.NewView() = new FlagSelector<'TFlagsEnum>()

  override _.SetAsChildOfParentView = true

  override _.SetProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    FlagSelectorPropHandler<'TFlagsEnum>.setProps (terminalElement, props)

  override _.RemoveProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    FlagSelectorPropHandler<'TFlagsEnum>.removeProps (terminalElement, props)

type internal OptionSelectorTerminalElement<'TEnum
  when 'TEnum: struct and 'TEnum: (new: unit -> 'TEnum) and 'TEnum :> System.Enum and 'TEnum :> System.ValueType>
  (props: Props) =
  inherit OptionSelectorTerminalElement(props)

  override _.Name = "OptionSelector`1"

  override _.NewView() = new OptionSelector<'TEnum>()

  override _.SetAsChildOfParentView = true

  override _.SetProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    OptionSelectorPropHandler<'TEnum>.setProps (terminalElement, props)

  override _.RemoveProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    OptionSelectorPropHandler<'TEnum>.removeProps (terminalElement, props)

type internal ShortcutTerminalElement(props: Props) =
  inherit ViewTerminalElement(props)

  override _.Name = "Shortcut"

  override _.NewView() = new Shortcut()

  override _.SetAsChildOfParentView = true

  override this.SubElements_PropKeys =
    [ PKey.Shortcut.CommandView_viewSpec.Untyped
      PKey.Shortcut.TargetView_viewSpec.Untyped ]
    |> List.append base.SubElements_PropKeys

  override _.SetProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    ShortcutPropHandler.setProps (terminalElement, props)

  override _.RemoveProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    ShortcutPropHandler.removeProps (terminalElement, props)

type internal MenuItemTerminalElement(props: Props) =
  inherit ShortcutTerminalElement(props)

  override _.Name = "MenuItem"

  override _.NewView() = new MenuItem()

  override _.SetAsChildOfParentView = true

  override this.SubElements_PropKeys =
    [ PKey.MenuItem.SubMenu_viewSpec.Untyped ]
    |> List.append base.SubElements_PropKeys

  override _.SetProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    MenuItemPropHandler.setProps (terminalElement, props)

  override _.RemoveProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    MenuItemPropHandler.removeProps (terminalElement, props)

type internal MenuBarItemTerminalElement(props: Props) =
  inherit MenuItemTerminalElement(props)

  override _.Name = "MenuBarItem"

  override _.NewView() = new MenuBarItem()

  override _.SetAsChildOfParentView = true

  override this.SubElements_PropKeys =
    [ PKey.MenuBarItem.PopoverMenu_viewSpec.Untyped ]
    |> List.append base.SubElements_PropKeys

  override _.SetProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    MenuBarItemPropHandler.setProps (terminalElement, props)

  override _.RemoveProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    MenuBarItemPropHandler.removeProps (terminalElement, props)

type internal SpinnerViewTerminalElement(props: Props) =
  inherit ViewTerminalElement(props)

  override _.Name = "SpinnerView"

  override _.NewView() = new SpinnerView()

  override _.SetAsChildOfParentView = true

  override _.SetProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    SpinnerViewPropHandler.setProps (terminalElement, props)

  override _.RemoveProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    SpinnerViewPropHandler.removeProps (terminalElement, props)

type internal StatusBarTerminalElement(props: Props) =
  inherit BarTerminalElement(props)

  override _.Name = "StatusBar"

  override _.NewView() = new StatusBar()

  override _.SetAsChildOfParentView = true

  override _.SetProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    StatusBarPropHandler.setProps (terminalElement, props)

  override _.RemoveProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    StatusBarPropHandler.removeProps (terminalElement, props)

type internal TableViewTerminalElement(props: Props) =
  inherit ViewTerminalElement(props)

  override _.Name = "TableView"

  override _.NewView() = new TableView()

  override _.SetAsChildOfParentView = true

  override _.SetProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    TableViewPropHandler.setProps (terminalElement, props)

  override _.RemoveProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    TableViewPropHandler.removeProps (terminalElement, props)

type internal TabsTerminalElement(props: Props) =
  inherit ViewTerminalElement(props)

  override _.Name = "Tabs"

  override _.NewView() = new Tabs()

  override _.SetAsChildOfParentView = true

  override this.SubElements_PropKeys =
    [ PKey.Tabs.Value_viewSpec.Untyped ] |> List.append base.SubElements_PropKeys

  override _.SetProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    TabsPropHandler.setProps (terminalElement, props)

  override _.RemoveProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    TabsPropHandler.removeProps (terminalElement, props)

type internal TextFieldTerminalElement(props: Props) =
  inherit ViewTerminalElement(props)

  override _.Name = "TextField"

  override _.NewView() = new TextField()

  override _.SetAsChildOfParentView = true

  override _.SetProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    TextFieldPropHandler.setProps (terminalElement, props)

  override _.RemoveProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    TextFieldPropHandler.removeProps (terminalElement, props)

type internal DropDownListTerminalElement(props: Props) =
  inherit TextFieldTerminalElement(props)

  override _.Name = "DropDownList"

  override _.NewView() = new DropDownList()

  override _.SetAsChildOfParentView = true

  override _.SetProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    DropDownListPropHandler.setProps (terminalElement, props)

  override _.RemoveProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    DropDownListPropHandler.removeProps (terminalElement, props)

type internal DropDownListTerminalElement<'TEnum
  when 'TEnum: struct and 'TEnum: (new: unit -> 'TEnum) and 'TEnum :> System.Enum and 'TEnum :> System.ValueType>
  (props: Props) =
  inherit DropDownListTerminalElement(props)

  override _.Name = "DropDownList`1"

  override _.NewView() = new DropDownList<'TEnum>()

  override _.SetAsChildOfParentView = true

  override _.SetProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    DropDownListPropHandler<'TEnum>.setProps (terminalElement, props)

  override _.RemoveProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    DropDownListPropHandler<'TEnum>.removeProps (terminalElement, props)

type internal TextValidateFieldTerminalElement(props: Props) =
  inherit ViewTerminalElement(props)

  override _.Name = "TextValidateField"

  override _.NewView() = new TextValidateField()

  override _.SetAsChildOfParentView = true

  override _.SetProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    TextValidateFieldPropHandler.setProps (terminalElement, props)

  override _.RemoveProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    TextValidateFieldPropHandler.removeProps (terminalElement, props)

type internal DateEditorTerminalElement(props: Props) =
  inherit TextValidateFieldTerminalElement(props)

  override _.Name = "DateEditor"

  override _.NewView() = new DateEditor()

  override _.SetAsChildOfParentView = true

  override _.SetProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    DateEditorPropHandler.setProps (terminalElement, props)

  override _.RemoveProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    DateEditorPropHandler.removeProps (terminalElement, props)

type internal TextViewTerminalElement(props: Props) =
  inherit ViewTerminalElement(props)

  override _.Name = "TextView"

  override _.NewView() = new TextView()

  override _.SetAsChildOfParentView = true

  override _.SetProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    TextViewPropHandler.setProps (terminalElement, props)

  override _.RemoveProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    TextViewPropHandler.removeProps (terminalElement, props)

type internal TimeEditorTerminalElement(props: Props) =
  inherit TextValidateFieldTerminalElement(props)

  override _.Name = "TimeEditor"

  override _.NewView() = new TimeEditor()

  override _.SetAsChildOfParentView = true

  override _.SetProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    TimeEditorPropHandler.setProps (terminalElement, props)

  override _.RemoveProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    TimeEditorPropHandler.removeProps (terminalElement, props)

type internal TitleViewTerminalElement(props: Props) =
  inherit ViewTerminalElement(props)

  override _.Name = "TitleView"

  override _.NewView() = new TitleView()

  override _.SetAsChildOfParentView = true

  override _.SetProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    TitleViewPropHandler.setProps (terminalElement, props)

  override _.RemoveProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    TitleViewPropHandler.removeProps (terminalElement, props)

type internal ToolTipHostTerminalElement<'TView
  when 'TView: (new: unit -> 'TView) and 'TView :> Terminal.Gui.ViewBase.View>(props: Props) =
  inherit PopoverImplTerminalElement(props)

  override _.Name = "ToolTipHost`1"

  override _.NewView() = new ToolTipHost<'TView>()

  override _.SetAsChildOfParentView = false

  override this.SubElements_PropKeys =
    [ PKey.ToolTipHost<'TView>.ContentView_viewSpec.Untyped ]
    |> List.append base.SubElements_PropKeys

  override _.SetProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    ToolTipHostPropHandler<'TView>.setProps (terminalElement, props)

  override _.RemoveProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    ToolTipHostPropHandler<'TView>.removeProps (terminalElement, props)

type internal TreeViewTerminalElement<'T when 'T: not struct>(props: Props) =
  inherit ViewTerminalElement(props)

  override _.Name = "TreeView`1"

  override _.NewView() = new TreeView<'T>()

  override _.SetAsChildOfParentView = true

  override _.SetProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    TreeViewPropHandler<'T>.setProps (terminalElement, props)

  override _.RemoveProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    TreeViewPropHandler<'T>.removeProps (terminalElement, props)

type internal TreeViewTerminalElement(props: Props) =
  inherit TreeViewTerminalElement<Terminal.Gui.Views.ITreeNode>(props)

  override _.Name = "TreeView"

  override _.NewView() = new TreeView()

  override _.SetAsChildOfParentView = true

  override _.SetProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    TreeViewPropHandler.setProps (terminalElement, props)

  override _.RemoveProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    TreeViewPropHandler.removeProps (terminalElement, props)

type internal WindowTerminalElement(props: Props) =
  inherit RunnableTerminalElement(props)

  override _.Name = "Window"

  override _.NewView() = new Window()

  override _.SetAsChildOfParentView = true

  override _.SetProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    WindowPropHandler.setProps (terminalElement, props)

  override _.RemoveProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    WindowPropHandler.removeProps (terminalElement, props)

type internal WizardTerminalElement(props: Props) =
  inherit DialogTerminalElement(props)

  override _.Name = "Wizard"

  override _.NewView() = new Wizard()

  override _.SetAsChildOfParentView = true

  override this.SubElements_PropKeys =
    [ PKey.Wizard.CurrentStep_viewSpec.Untyped ]
    |> List.append base.SubElements_PropKeys

  override _.SetProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    WizardPropHandler.setProps (terminalElement, props)

  override _.RemoveProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    WizardPropHandler.removeProps (terminalElement, props)

type internal WizardStepTerminalElement(props: Props) =
  inherit ViewTerminalElement(props)

  override _.Name = "WizardStep"

  override _.NewView() = new WizardStep()

  override _.SetAsChildOfParentView = true

  override _.SetProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    WizardStepPropHandler.setProps (terminalElement, props)

  override _.RemoveProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    WizardStepPropHandler.removeProps (terminalElement, props)
