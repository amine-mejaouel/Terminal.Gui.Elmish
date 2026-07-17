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

  override _.ApplyNativeProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    ViewPropHandler.applyNativeProps (terminalElement, props)

  override this.ClearProp(propertyId: PropertyId) =
    ViewPropHandler.clearProp (this, propertyId)

type internal AdornmentViewTerminalElement(props: Props) =
  inherit ViewTerminalElement(props)

  override _.Name = "AdornmentView"

  override _.NewView() = new AdornmentView()

  override _.SetAsChildOfParentView = true

  override _.ApplyNativeProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    AdornmentViewPropHandler.applyNativeProps (terminalElement, props)

  override this.ClearProp(propertyId: PropertyId) =
    AdornmentViewPropHandler.clearProp (this, propertyId)

type internal AttributePickerTerminalElement(props: Props) =
  inherit ViewTerminalElement(props)

  override _.Name = "AttributePicker"

  override _.NewView() = new AttributePicker()

  override _.SetAsChildOfParentView = true

  override _.ApplyNativeProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    AttributePickerPropHandler.applyNativeProps (terminalElement, props)

  override this.ClearProp(propertyId: PropertyId) =
    AttributePickerPropHandler.clearProp (this, propertyId)

type internal BarTerminalElement(props: Props) =
  inherit ViewTerminalElement(props)

  override _.Name = "Bar"

  override _.NewView() = new Bar()

  override _.SetAsChildOfParentView = true

  override _.ApplyNativeProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    BarPropHandler.applyNativeProps (terminalElement, props)

  override this.ClearProp(propertyId: PropertyId) =
    BarPropHandler.clearProp (this, propertyId)

type internal BorderViewTerminalElement(props: Props) =
  inherit AdornmentViewTerminalElement(props)

  override _.Name = "BorderView"

  override _.NewView() = new BorderView()

  override _.SetAsChildOfParentView = true

  override _.ApplyNativeProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    BorderViewPropHandler.applyNativeProps (terminalElement, props)

  override this.ClearProp(propertyId: PropertyId) =
    BorderViewPropHandler.clearProp (this, propertyId)

type internal ButtonTerminalElement(props: Props) =
  inherit ViewTerminalElement(props)

  override _.Name = "Button"

  override _.NewView() = new Button()

  override _.SetAsChildOfParentView = true

  override _.ApplyNativeProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    ButtonPropHandler.applyNativeProps (terminalElement, props)

  override this.ClearProp(propertyId: PropertyId) =
    ButtonPropHandler.clearProp (this, propertyId)

type internal CharMapTerminalElement(props: Props) =
  inherit ViewTerminalElement(props)

  override _.Name = "CharMap"

  override _.NewView() = new CharMap()

  override _.SetAsChildOfParentView = true

  override _.ApplyNativeProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    CharMapPropHandler.applyNativeProps (terminalElement, props)

  override this.ClearProp(propertyId: PropertyId) =
    CharMapPropHandler.clearProp (this, propertyId)

type internal CheckBoxTerminalElement(props: Props) =
  inherit ViewTerminalElement(props)

  override _.Name = "CheckBox"

  override _.NewView() = new CheckBox()

  override _.SetAsChildOfParentView = true

  override _.ApplyNativeProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    CheckBoxPropHandler.applyNativeProps (terminalElement, props)

  override this.ClearProp(propertyId: PropertyId) =
    CheckBoxPropHandler.clearProp (this, propertyId)

type internal CodeTerminalElement(props: Props) =
  inherit ViewTerminalElement(props)

  override _.Name = "Code"

  override _.NewView() = new Code()

  override _.SetAsChildOfParentView = true

  override _.ApplyNativeProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    CodePropHandler.applyNativeProps (terminalElement, props)

  override this.ClearProp(propertyId: PropertyId) =
    CodePropHandler.clearProp (this, propertyId)

type internal ColorPickerTerminalElement(props: Props) =
  inherit ViewTerminalElement(props)

  override _.Name = "ColorPicker"

  override _.NewView() = new ColorPicker()

  override _.SetAsChildOfParentView = true

  override _.ApplyNativeProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    ColorPickerPropHandler.applyNativeProps (terminalElement, props)

  override this.ClearProp(propertyId: PropertyId) =
    ColorPickerPropHandler.clearProp (this, propertyId)

type internal ColorPicker16TerminalElement(props: Props) =
  inherit ViewTerminalElement(props)

  override _.Name = "ColorPicker16"

  override _.NewView() = new ColorPicker16()

  override _.SetAsChildOfParentView = true

  override _.ApplyNativeProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    ColorPicker16PropHandler.applyNativeProps (terminalElement, props)

  override this.ClearProp(propertyId: PropertyId) =
    ColorPicker16PropHandler.clearProp (this, propertyId)

type internal DatePickerTerminalElement(props: Props) =
  inherit ViewTerminalElement(props)

  override _.Name = "DatePicker"

  override _.NewView() = new DatePicker()

  override _.SetAsChildOfParentView = true

  override _.ApplyNativeProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    DatePickerPropHandler.applyNativeProps (terminalElement, props)

  override this.ClearProp(propertyId: PropertyId) =
    DatePickerPropHandler.clearProp (this, propertyId)

type internal FrameViewTerminalElement(props: Props) =
  inherit ViewTerminalElement(props)

  override _.Name = "FrameView"

  override _.NewView() = new FrameView()

  override _.SetAsChildOfParentView = true

  override _.ApplyNativeProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    FrameViewPropHandler.applyNativeProps (terminalElement, props)

  override this.ClearProp(propertyId: PropertyId) =
    FrameViewPropHandler.clearProp (this, propertyId)

type internal GraphViewTerminalElement(props: Props) =
  inherit ViewTerminalElement(props)

  override _.Name = "GraphView"

  override _.NewView() = new GraphView()

  override _.SetAsChildOfParentView = true

  override _.ApplyNativeProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    GraphViewPropHandler.applyNativeProps (terminalElement, props)

  override this.ClearProp(propertyId: PropertyId) =
    GraphViewPropHandler.clearProp (this, propertyId)

type internal HexViewTerminalElement(props: Props) =
  inherit ViewTerminalElement(props)

  override _.Name = "HexView"

  override _.NewView() = new HexView()

  override _.SetAsChildOfParentView = true

  override _.ApplyNativeProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    HexViewPropHandler.applyNativeProps (terminalElement, props)

  override this.ClearProp(propertyId: PropertyId) =
    HexViewPropHandler.clearProp (this, propertyId)

type internal ImageViewTerminalElement(props: Props) =
  inherit ViewTerminalElement(props)

  override _.Name = "ImageView"

  override _.NewView() = new ImageView()

  override _.SetAsChildOfParentView = true

  override _.ApplyNativeProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    ImageViewPropHandler.applyNativeProps (terminalElement, props)

  override this.ClearProp(propertyId: PropertyId) =
    ImageViewPropHandler.clearProp (this, propertyId)

type internal LabelTerminalElement(props: Props) =
  inherit ViewTerminalElement(props)

  override _.Name = "Label"

  override _.NewView() = new Label()

  override _.SetAsChildOfParentView = true

  override _.ApplyNativeProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    LabelPropHandler.applyNativeProps (terminalElement, props)

  override this.ClearProp(propertyId: PropertyId) =
    LabelPropHandler.clearProp (this, propertyId)

type internal LegendAnnotationTerminalElement(props: Props) =
  inherit ViewTerminalElement(props)

  override _.Name = "LegendAnnotation"

  override _.NewView() = new LegendAnnotation()

  override _.SetAsChildOfParentView = true

  override _.ApplyNativeProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    LegendAnnotationPropHandler.applyNativeProps (terminalElement, props)

  override this.ClearProp(propertyId: PropertyId) =
    LegendAnnotationPropHandler.clearProp (this, propertyId)

type internal LineTerminalElement(props: Props) =
  inherit ViewTerminalElement(props)

  override _.Name = "Line"

  override _.NewView() = new Line()

  override _.SetAsChildOfParentView = true

  override _.ApplyNativeProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    LinePropHandler.applyNativeProps (terminalElement, props)

  override this.ClearProp(propertyId: PropertyId) =
    LinePropHandler.clearProp (this, propertyId)

[<AbstractClass>]
type internal LinearRangeViewBaseTerminalElement<'TOption, 'TValue>(props: Props) =
  inherit ViewTerminalElement(props)

  override _.Name = "LinearRangeViewBase`2"

  override _.NewView() =
    failwith "Cannot instantiate abstract view type LinearRangeViewBase"

  override _.SetAsChildOfParentView = true

  override _.ApplyNativeProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    LinearRangeViewBasePropHandler<'TOption, 'TValue>.applyNativeProps (terminalElement, props)

  override this.ClearProp(propertyId: PropertyId) =
    LinearRangeViewBasePropHandler<'TOption, 'TValue>.clearProp (this, propertyId)

type internal LinearMultiSelectorTerminalElement<'T>(props: Props) =
  inherit LinearRangeViewBaseTerminalElement<'T, IReadOnlyList<'T>>(props)

  override _.Name = "LinearMultiSelector`1"

  override _.NewView() = new LinearMultiSelector<'T>()

  override _.SetAsChildOfParentView = true

  override _.ApplyNativeProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    LinearMultiSelectorPropHandler<'T>.applyNativeProps (terminalElement, props)

  override this.ClearProp(propertyId: PropertyId) =
    LinearMultiSelectorPropHandler<'T>.clearProp (this, propertyId)

type internal LinearMultiSelectorTerminalElement(props: Props) =
  inherit LinearMultiSelectorTerminalElement<string>(props)

  override _.Name = "LinearMultiSelector"

  override _.NewView() = new LinearMultiSelector()

  override _.SetAsChildOfParentView = true

  override _.ApplyNativeProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    LinearMultiSelectorPropHandler.applyNativeProps (terminalElement, props)

  override this.ClearProp(propertyId: PropertyId) =
    LinearMultiSelectorPropHandler.clearProp (this, propertyId)

type internal LinearRangeTerminalElement<'T>(props: Props) =
  inherit LinearRangeViewBaseTerminalElement<'T, LinearRangeSpan<'T>>(props)

  override _.Name = "LinearRange`1"

  override _.NewView() = new LinearRange<'T>()

  override _.SetAsChildOfParentView = true

  override _.ApplyNativeProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    LinearRangePropHandler<'T>.applyNativeProps (terminalElement, props)

  override this.ClearProp(propertyId: PropertyId) =
    LinearRangePropHandler<'T>.clearProp (this, propertyId)

type internal LinearRangeTerminalElement(props: Props) =
  inherit LinearRangeTerminalElement<string>(props)

  override _.Name = "LinearRange"

  override _.NewView() = new LinearRange()

  override _.SetAsChildOfParentView = true

  override _.ApplyNativeProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    LinearRangePropHandler.applyNativeProps (terminalElement, props)

  override this.ClearProp(propertyId: PropertyId) =
    LinearRangePropHandler.clearProp (this, propertyId)

type internal LinearSelectorTerminalElement<'T>(props: Props) =
  inherit LinearRangeViewBaseTerminalElement<'T, 'T>(props)

  override _.Name = "LinearSelector`1"

  override _.NewView() = new LinearSelector<'T>()

  override _.SetAsChildOfParentView = true

  override _.ApplyNativeProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    LinearSelectorPropHandler<'T>.applyNativeProps (terminalElement, props)

  override this.ClearProp(propertyId: PropertyId) =
    LinearSelectorPropHandler<'T>.clearProp (this, propertyId)

type internal LinearSelectorTerminalElement(props: Props) =
  inherit LinearSelectorTerminalElement<string>(props)

  override _.Name = "LinearSelector"

  override _.NewView() = new LinearSelector()

  override _.SetAsChildOfParentView = true

  override _.ApplyNativeProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    LinearSelectorPropHandler.applyNativeProps (terminalElement, props)

  override this.ClearProp(propertyId: PropertyId) =
    LinearSelectorPropHandler.clearProp (this, propertyId)

type internal LinkTerminalElement(props: Props) =
  inherit ViewTerminalElement(props)

  override _.Name = "Link"

  override _.NewView() = new Link()

  override _.SetAsChildOfParentView = true

  override _.ApplyNativeProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    LinkPropHandler.applyNativeProps (terminalElement, props)

  override this.ClearProp(propertyId: PropertyId) =
    LinkPropHandler.clearProp (this, propertyId)

type internal ListViewTerminalElement(props: Props) =
  inherit ViewTerminalElement(props)

  override _.Name = "ListView"

  override _.NewView() = new ListView()

  override _.SetAsChildOfParentView = true

  override _.ApplyNativeProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    ListViewPropHandler.applyNativeProps (terminalElement, props)

  override this.ClearProp(propertyId: PropertyId) =
    ListViewPropHandler.clearProp (this, propertyId)

type internal ListViewTerminalElement<'T>(props: Props) =
  inherit ListViewTerminalElement(props)

  override _.Name = "ListView`1"

  override _.NewView() = new ListView<'T>()

  override _.SetAsChildOfParentView = true

  override _.ApplyNativeProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    ListViewPropHandler<'T>.applyNativeProps (terminalElement, props)

  override this.ClearProp(propertyId: PropertyId) =
    ListViewPropHandler<'T>.clearProp (this, propertyId)

type internal MarginViewTerminalElement(props: Props) =
  inherit AdornmentViewTerminalElement(props)

  override _.Name = "MarginView"

  override _.NewView() = new MarginView()

  override _.SetAsChildOfParentView = true

  override _.ApplyNativeProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    MarginViewPropHandler.applyNativeProps (terminalElement, props)

  override this.ClearProp(propertyId: PropertyId) =
    MarginViewPropHandler.clearProp (this, propertyId)

type internal MarkdownTerminalElement(props: Props) =
  inherit ViewTerminalElement(props)

  override _.Name = "Markdown"

  override _.NewView() = new Markdown()

  override _.SetAsChildOfParentView = true

  override _.ApplyNativeProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    MarkdownPropHandler.applyNativeProps (terminalElement, props)

  override this.ClearProp(propertyId: PropertyId) =
    MarkdownPropHandler.clearProp (this, propertyId)

type internal MarkdownCodeBlockTerminalElement(props: Props) =
  inherit ViewTerminalElement(props)

  override _.Name = "MarkdownCodeBlock"

  override _.NewView() = new MarkdownCodeBlock()

  override _.SetAsChildOfParentView = true

  override _.ApplyNativeProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    MarkdownCodeBlockPropHandler.applyNativeProps (terminalElement, props)

  override this.ClearProp(propertyId: PropertyId) =
    MarkdownCodeBlockPropHandler.clearProp (this, propertyId)

type internal MarkdownTableTerminalElement(props: Props) =
  inherit ViewTerminalElement(props)

  override _.Name = "MarkdownTable"

  override _.NewView() = new MarkdownTable()

  override _.SetAsChildOfParentView = true

  override _.ApplyNativeProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    MarkdownTablePropHandler.applyNativeProps (terminalElement, props)

  override this.ClearProp(propertyId: PropertyId) =
    MarkdownTablePropHandler.clearProp (this, propertyId)

type internal MenuTerminalElement(props: Props) =
  inherit BarTerminalElement(props)

  override _.Name = "Menu"

  override _.NewView() = new Menu()

  override _.SetAsChildOfParentView = false

  override this.SubElements_PropKeys =
    [ PKey.Menu.SuperMenuItem_viewSpec.Untyped; PKey.Menu.Value_viewSpec.Untyped ]
    |> List.append base.SubElements_PropKeys

  override _.ApplyNativeProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    MenuPropHandler.applyNativeProps (terminalElement, props)

  override this.ClearProp(propertyId: PropertyId) =
    MenuPropHandler.clearProp (this, propertyId)

type internal MenuBarTerminalElement(props: Props) =
  inherit MenuTerminalElement(props)

  override _.Name = "MenuBar"

  override _.NewView() = new MenuBar()

  override _.SetAsChildOfParentView = true

  override _.ApplyNativeProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    MenuBarPropHandler.applyNativeProps (terminalElement, props)

  override this.ClearProp(propertyId: PropertyId) =
    MenuBarPropHandler.clearProp (this, propertyId)

type internal NumericUpDownTerminalElement<'T>(props: Props) =
  inherit ViewTerminalElement(props)

  override _.Name = "NumericUpDown`1"

  override _.NewView() = new NumericUpDown<'T>()

  override _.SetAsChildOfParentView = true

  override _.ApplyNativeProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    NumericUpDownPropHandler<'T>.applyNativeProps (terminalElement, props)

  override this.ClearProp(propertyId: PropertyId) =
    NumericUpDownPropHandler<'T>.clearProp (this, propertyId)

type internal NumericUpDownTerminalElement(props: Props) =
  inherit NumericUpDownTerminalElement<int>(props)

  override _.Name = "NumericUpDown"

  override _.NewView() = new NumericUpDown()

  override _.SetAsChildOfParentView = true

  override _.ApplyNativeProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    NumericUpDownPropHandler.applyNativeProps (terminalElement, props)

  override this.ClearProp(propertyId: PropertyId) =
    NumericUpDownPropHandler.clearProp (this, propertyId)

type internal PaddingViewTerminalElement(props: Props) =
  inherit AdornmentViewTerminalElement(props)

  override _.Name = "PaddingView"

  override _.NewView() = new PaddingView()

  override _.SetAsChildOfParentView = true

  override _.ApplyNativeProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    PaddingViewPropHandler.applyNativeProps (terminalElement, props)

  override this.ClearProp(propertyId: PropertyId) =
    PaddingViewPropHandler.clearProp (this, propertyId)

[<AbstractClass>]
type internal PopoverImplTerminalElement(props: Props) =
  inherit ViewTerminalElement(props)

  override _.Name = "PopoverImpl"

  override _.NewView() =
    failwith "Cannot instantiate abstract view type PopoverImpl"

  override _.SetAsChildOfParentView = false

  override _.ApplyNativeProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    PopoverImplPropHandler.applyNativeProps (terminalElement, props)

  override this.ClearProp(propertyId: PropertyId) =
    PopoverImplPropHandler.clearProp (this, propertyId)

type internal PopoverTerminalElement<'TView, 'TResult
  when 'TView: (new: unit -> 'TView) and 'TView :> Terminal.Gui.ViewBase.View>(props: Props) =
  inherit PopoverImplTerminalElement(props)

  override _.Name = "Popover`2"

  override _.NewView() = new Popover<'TView, 'TResult>()

  override _.SetAsChildOfParentView = false

  override this.SubElements_PropKeys =
    [ PKey.Popover<'TView, 'TResult>.ContentView_viewSpec.Untyped ]
    |> List.append base.SubElements_PropKeys

  override _.ApplyNativeProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    PopoverPropHandler<'TView, 'TResult>.applyNativeProps (terminalElement, props)

  override this.ClearProp(propertyId: PropertyId) =
    PopoverPropHandler<'TView, 'TResult>.clearProp (this, propertyId)

type internal PopoverMenuTerminalElement(props: Props) =
  inherit PopoverTerminalElement<Terminal.Gui.Views.Menu, Terminal.Gui.Views.MenuItem>(props)

  override _.Name = "PopoverMenu"

  override _.NewView() = new PopoverMenu()

  override _.SetAsChildOfParentView = false

  override this.SubElements_PropKeys =
    [ PKey.PopoverMenu.Root_viewSpec.Untyped ]
    |> List.append base.SubElements_PropKeys

  override _.ApplyNativeProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    PopoverMenuPropHandler.applyNativeProps (terminalElement, props)

  override this.ClearProp(propertyId: PropertyId) =
    PopoverMenuPropHandler.clearProp (this, propertyId)

type internal ProgressBarTerminalElement(props: Props) =
  inherit ViewTerminalElement(props)

  override _.Name = "ProgressBar"

  override _.NewView() = new ProgressBar()

  override _.SetAsChildOfParentView = true

  override _.ApplyNativeProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    ProgressBarPropHandler.applyNativeProps (terminalElement, props)

  override this.ClearProp(propertyId: PropertyId) =
    ProgressBarPropHandler.clearProp (this, propertyId)

type internal RunnableTerminalElement(props: Props) =
  inherit ViewTerminalElement(props)

  override _.Name = "Runnable"

  override _.NewView() = new Runnable()

  override _.SetAsChildOfParentView = true

  override _.ApplyNativeProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    RunnablePropHandler.applyNativeProps (terminalElement, props)

  override this.ClearProp(propertyId: PropertyId) =
    RunnablePropHandler.clearProp (this, propertyId)

type internal RunnableTerminalElement<'TResult>(props: Props) =
  inherit RunnableTerminalElement(props)

  override _.Name = "Runnable`1"

  override _.NewView() = new Runnable<'TResult>()

  override _.SetAsChildOfParentView = true

  override _.ApplyNativeProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    RunnablePropHandler<'TResult>.applyNativeProps (terminalElement, props)

  override this.ClearProp(propertyId: PropertyId) =
    RunnablePropHandler<'TResult>.clearProp (this, propertyId)

type internal DialogTerminalElement<'TResult>(props: Props) =
  inherit RunnableTerminalElement<'TResult>(props)

  override _.Name = "Dialog`1"

  override _.NewView() = new Dialog<'TResult>()

  override _.SetAsChildOfParentView = true

  override _.ApplyNativeProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    DialogPropHandler<'TResult>.applyNativeProps (terminalElement, props)

  override this.ClearProp(propertyId: PropertyId) =
    DialogPropHandler<'TResult>.clearProp (this, propertyId)

type internal RunnableWrapperTerminalElement<'TView, 'TResult
  when 'TView: (new: unit -> 'TView) and 'TView :> Terminal.Gui.ViewBase.View>(props: Props) =
  inherit RunnableTerminalElement<'TResult>(props)

  override _.Name = "RunnableWrapper`2"

  override _.NewView() = new RunnableWrapper<'TView, 'TResult>()

  override _.SetAsChildOfParentView = true

  override _.ApplyNativeProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    RunnableWrapperPropHandler<'TView, 'TResult>.applyNativeProps (terminalElement, props)

  override this.ClearProp(propertyId: PropertyId) =
    RunnableWrapperPropHandler<'TView, 'TResult>.clearProp (this, propertyId)

type internal DialogTerminalElement(props: Props) =
  inherit DialogTerminalElement<int>(props)

  override _.Name = "Dialog"

  override _.NewView() = new Dialog()

  override _.SetAsChildOfParentView = true

  override _.ApplyNativeProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    DialogPropHandler.applyNativeProps (terminalElement, props)

  override this.ClearProp(propertyId: PropertyId) =
    DialogPropHandler.clearProp (this, propertyId)

type internal FileDialogTerminalElement(props: Props) =
  inherit DialogTerminalElement<IReadOnlyList<string>>(props)

  override _.Name = "FileDialog"

  override _.NewView() = new FileDialog()

  override _.SetAsChildOfParentView = true

  override _.ApplyNativeProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    FileDialogPropHandler.applyNativeProps (terminalElement, props)

  override this.ClearProp(propertyId: PropertyId) =
    FileDialogPropHandler.clearProp (this, propertyId)

type internal PromptTerminalElement<'TView, 'TResult
  when 'TView: (new: unit -> 'TView) and 'TView :> Terminal.Gui.ViewBase.View>(props: Props) =
  inherit DialogTerminalElement<'TResult>(props)

  override _.Name = "Prompt`2"

  override _.NewView() = new Prompt<'TView, 'TResult>()

  override _.SetAsChildOfParentView = true

  override _.ApplyNativeProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    PromptPropHandler<'TView, 'TResult>.applyNativeProps (terminalElement, props)

  override this.ClearProp(propertyId: PropertyId) =
    PromptPropHandler<'TView, 'TResult>.clearProp (this, propertyId)

type internal OpenDialogTerminalElement(props: Props) =
  inherit FileDialogTerminalElement(props)

  override _.Name = "OpenDialog"

  override _.NewView() = new OpenDialog()

  override _.SetAsChildOfParentView = true

  override _.ApplyNativeProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    OpenDialogPropHandler.applyNativeProps (terminalElement, props)

  override this.ClearProp(propertyId: PropertyId) =
    OpenDialogPropHandler.clearProp (this, propertyId)

type internal SaveDialogTerminalElement(props: Props) =
  inherit FileDialogTerminalElement(props)

  override _.Name = "SaveDialog"

  override _.NewView() = new SaveDialog()

  override _.SetAsChildOfParentView = true

  override _.ApplyNativeProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    SaveDialogPropHandler.applyNativeProps (terminalElement, props)

  override this.ClearProp(propertyId: PropertyId) =
    SaveDialogPropHandler.clearProp (this, propertyId)

type internal ScrollBarTerminalElement(props: Props) =
  inherit ViewTerminalElement(props)

  override _.Name = "ScrollBar"

  override _.NewView() = new ScrollBar()

  override _.SetAsChildOfParentView = true

  override _.ApplyNativeProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    ScrollBarPropHandler.applyNativeProps (terminalElement, props)

  override this.ClearProp(propertyId: PropertyId) =
    ScrollBarPropHandler.clearProp (this, propertyId)

type internal ScrollButtonTerminalElement(props: Props) =
  inherit ButtonTerminalElement(props)

  override _.Name = "ScrollButton"

  override _.NewView() = new ScrollButton()

  override _.SetAsChildOfParentView = true

  override _.ApplyNativeProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    ScrollButtonPropHandler.applyNativeProps (terminalElement, props)

  override this.ClearProp(propertyId: PropertyId) =
    ScrollButtonPropHandler.clearProp (this, propertyId)

type internal ScrollSliderTerminalElement(props: Props) =
  inherit ViewTerminalElement(props)

  override _.Name = "ScrollSlider"

  override _.NewView() = new ScrollSlider()

  override _.SetAsChildOfParentView = true

  override _.ApplyNativeProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    ScrollSliderPropHandler.applyNativeProps (terminalElement, props)

  override this.ClearProp(propertyId: PropertyId) =
    ScrollSliderPropHandler.clearProp (this, propertyId)

[<AbstractClass>]
type internal SelectorBaseTerminalElement(props: Props) =
  inherit ViewTerminalElement(props)

  override _.Name = "SelectorBase"

  override _.NewView() =
    failwith "Cannot instantiate abstract view type SelectorBase"

  override _.SetAsChildOfParentView = true

  override _.ApplyNativeProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    SelectorBasePropHandler.applyNativeProps (terminalElement, props)

  override this.ClearProp(propertyId: PropertyId) =
    SelectorBasePropHandler.clearProp (this, propertyId)

type internal FlagSelectorTerminalElement(props: Props) =
  inherit SelectorBaseTerminalElement(props)

  override _.Name = "FlagSelector"

  override _.NewView() = new FlagSelector()

  override _.SetAsChildOfParentView = true

  override _.ApplyNativeProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    FlagSelectorPropHandler.applyNativeProps (terminalElement, props)

  override this.ClearProp(propertyId: PropertyId) =
    FlagSelectorPropHandler.clearProp (this, propertyId)

type internal OptionSelectorTerminalElement(props: Props) =
  inherit SelectorBaseTerminalElement(props)

  override _.Name = "OptionSelector"

  override _.NewView() = new OptionSelector()

  override _.SetAsChildOfParentView = true

  override _.ApplyNativeProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    OptionSelectorPropHandler.applyNativeProps (terminalElement, props)

  override this.ClearProp(propertyId: PropertyId) =
    OptionSelectorPropHandler.clearProp (this, propertyId)

type internal FlagSelectorTerminalElement<'TFlagsEnum
  when 'TFlagsEnum: struct
  and 'TFlagsEnum: (new: unit -> 'TFlagsEnum)
  and 'TFlagsEnum :> System.Enum
  and 'TFlagsEnum :> System.ValueType>(props: Props) =
  inherit FlagSelectorTerminalElement(props)

  override _.Name = "FlagSelector`1"

  override _.NewView() = new FlagSelector<'TFlagsEnum>()

  override _.SetAsChildOfParentView = true

  override _.ApplyNativeProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    FlagSelectorPropHandler<'TFlagsEnum>.applyNativeProps (terminalElement, props)

  override this.ClearProp(propertyId: PropertyId) =
    FlagSelectorPropHandler<'TFlagsEnum>.clearProp (this, propertyId)

type internal OptionSelectorTerminalElement<'TEnum
  when 'TEnum: struct and 'TEnum: (new: unit -> 'TEnum) and 'TEnum :> System.Enum and 'TEnum :> System.ValueType>
  (props: Props) =
  inherit OptionSelectorTerminalElement(props)

  override _.Name = "OptionSelector`1"

  override _.NewView() = new OptionSelector<'TEnum>()

  override _.SetAsChildOfParentView = true

  override _.ApplyNativeProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    OptionSelectorPropHandler<'TEnum>.applyNativeProps (terminalElement, props)

  override this.ClearProp(propertyId: PropertyId) =
    OptionSelectorPropHandler<'TEnum>.clearProp (this, propertyId)

type internal ShortcutTerminalElement(props: Props) =
  inherit ViewTerminalElement(props)

  override _.Name = "Shortcut"

  override _.NewView() = new Shortcut()

  override _.SetAsChildOfParentView = true

  override this.SubElements_PropKeys =
    [ PKey.Shortcut.CommandView_viewSpec.Untyped
      PKey.Shortcut.TargetView_viewSpec.Untyped ]
    |> List.append base.SubElements_PropKeys

  override _.ApplyNativeProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    ShortcutPropHandler.applyNativeProps (terminalElement, props)

  override this.ClearProp(propertyId: PropertyId) =
    ShortcutPropHandler.clearProp (this, propertyId)

type internal MenuItemTerminalElement(props: Props) =
  inherit ShortcutTerminalElement(props)

  override _.Name = "MenuItem"

  override _.NewView() = new MenuItem()

  override _.SetAsChildOfParentView = true

  override this.SubElements_PropKeys =
    [ PKey.MenuItem.SubMenu_viewSpec.Untyped ]
    |> List.append base.SubElements_PropKeys

  override _.ApplyNativeProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    MenuItemPropHandler.applyNativeProps (terminalElement, props)

  override this.ClearProp(propertyId: PropertyId) =
    MenuItemPropHandler.clearProp (this, propertyId)

type internal MenuBarItemTerminalElement(props: Props) =
  inherit MenuItemTerminalElement(props)

  override _.Name = "MenuBarItem"

  override _.NewView() = new MenuBarItem()

  override _.SetAsChildOfParentView = true

  override this.SubElements_PropKeys =
    [ PKey.MenuBarItem.PopoverMenu_viewSpec.Untyped ]
    |> List.append base.SubElements_PropKeys

  override _.ApplyNativeProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    MenuBarItemPropHandler.applyNativeProps (terminalElement, props)

  override this.ClearProp(propertyId: PropertyId) =
    MenuBarItemPropHandler.clearProp (this, propertyId)

type internal SpinnerViewTerminalElement(props: Props) =
  inherit ViewTerminalElement(props)

  override _.Name = "SpinnerView"

  override _.NewView() = new SpinnerView()

  override _.SetAsChildOfParentView = true

  override _.ApplyNativeProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    SpinnerViewPropHandler.applyNativeProps (terminalElement, props)

  override this.ClearProp(propertyId: PropertyId) =
    SpinnerViewPropHandler.clearProp (this, propertyId)

type internal StatusBarTerminalElement(props: Props) =
  inherit BarTerminalElement(props)

  override _.Name = "StatusBar"

  override _.NewView() = new StatusBar()

  override _.SetAsChildOfParentView = true

  override _.ApplyNativeProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    StatusBarPropHandler.applyNativeProps (terminalElement, props)

  override this.ClearProp(propertyId: PropertyId) =
    StatusBarPropHandler.clearProp (this, propertyId)

type internal TableViewTerminalElement(props: Props) =
  inherit ViewTerminalElement(props)

  override _.Name = "TableView"

  override _.NewView() = new TableView()

  override _.SetAsChildOfParentView = true

  override _.ApplyNativeProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    TableViewPropHandler.applyNativeProps (terminalElement, props)

  override this.ClearProp(propertyId: PropertyId) =
    TableViewPropHandler.clearProp (this, propertyId)

type internal TabsTerminalElement(props: Props) =
  inherit ViewTerminalElement(props)

  override _.Name = "Tabs"

  override _.NewView() = new Tabs()

  override _.SetAsChildOfParentView = true

  override this.SubElements_PropKeys =
    [ PKey.Tabs.Value_viewSpec.Untyped ] |> List.append base.SubElements_PropKeys

  override _.ApplyNativeProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    TabsPropHandler.applyNativeProps (terminalElement, props)

  override this.ClearProp(propertyId: PropertyId) =
    TabsPropHandler.clearProp (this, propertyId)

type internal TextFieldTerminalElement(props: Props) =
  inherit ViewTerminalElement(props)

  override _.Name = "TextField"

  override _.NewView() = new TextField()

  override _.SetAsChildOfParentView = true

  override _.ApplyNativeProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    TextFieldPropHandler.applyNativeProps (terminalElement, props)

  override this.ClearProp(propertyId: PropertyId) =
    TextFieldPropHandler.clearProp (this, propertyId)

type internal DropDownListTerminalElement(props: Props) =
  inherit TextFieldTerminalElement(props)

  override _.Name = "DropDownList"

  override _.NewView() = new DropDownList()

  override _.SetAsChildOfParentView = true

  override _.ApplyNativeProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    DropDownListPropHandler.applyNativeProps (terminalElement, props)

  override this.ClearProp(propertyId: PropertyId) =
    DropDownListPropHandler.clearProp (this, propertyId)

type internal DropDownListTerminalElement<'TEnum
  when 'TEnum: struct and 'TEnum: (new: unit -> 'TEnum) and 'TEnum :> System.Enum and 'TEnum :> System.ValueType>
  (props: Props) =
  inherit DropDownListTerminalElement(props)

  override _.Name = "DropDownList`1"

  override _.NewView() = new DropDownList<'TEnum>()

  override _.SetAsChildOfParentView = true

  override _.ApplyNativeProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    DropDownListPropHandler<'TEnum>.applyNativeProps (terminalElement, props)

  override this.ClearProp(propertyId: PropertyId) =
    DropDownListPropHandler<'TEnum>.clearProp (this, propertyId)

type internal TextValidateFieldTerminalElement(props: Props) =
  inherit ViewTerminalElement(props)

  override _.Name = "TextValidateField"

  override _.NewView() = new TextValidateField()

  override _.SetAsChildOfParentView = true

  override _.ApplyNativeProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    TextValidateFieldPropHandler.applyNativeProps (terminalElement, props)

  override this.ClearProp(propertyId: PropertyId) =
    TextValidateFieldPropHandler.clearProp (this, propertyId)

type internal DateEditorTerminalElement(props: Props) =
  inherit TextValidateFieldTerminalElement(props)

  override _.Name = "DateEditor"

  override _.NewView() = new DateEditor()

  override _.SetAsChildOfParentView = true

  override _.ApplyNativeProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    DateEditorPropHandler.applyNativeProps (terminalElement, props)

  override this.ClearProp(propertyId: PropertyId) =
    DateEditorPropHandler.clearProp (this, propertyId)

type internal TextViewTerminalElement(props: Props) =
  inherit ViewTerminalElement(props)

  override _.Name = "TextView"

  override _.NewView() = new TextView()

  override _.SetAsChildOfParentView = true

  override _.ApplyNativeProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    TextViewPropHandler.applyNativeProps (terminalElement, props)

  override this.ClearProp(propertyId: PropertyId) =
    TextViewPropHandler.clearProp (this, propertyId)

type internal TimeEditorTerminalElement(props: Props) =
  inherit TextValidateFieldTerminalElement(props)

  override _.Name = "TimeEditor"

  override _.NewView() = new TimeEditor()

  override _.SetAsChildOfParentView = true

  override _.ApplyNativeProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    TimeEditorPropHandler.applyNativeProps (terminalElement, props)

  override this.ClearProp(propertyId: PropertyId) =
    TimeEditorPropHandler.clearProp (this, propertyId)

type internal TitleViewTerminalElement(props: Props) =
  inherit ViewTerminalElement(props)

  override _.Name = "TitleView"

  override _.NewView() = new TitleView()

  override _.SetAsChildOfParentView = true

  override _.ApplyNativeProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    TitleViewPropHandler.applyNativeProps (terminalElement, props)

  override this.ClearProp(propertyId: PropertyId) =
    TitleViewPropHandler.clearProp (this, propertyId)

type internal ToolTipHostTerminalElement<'TView
  when 'TView: (new: unit -> 'TView) and 'TView :> Terminal.Gui.ViewBase.View>(props: Props) =
  inherit PopoverImplTerminalElement(props)

  override _.Name = "ToolTipHost`1"

  override _.NewView() = new ToolTipHost<'TView>()

  override _.SetAsChildOfParentView = false

  override this.SubElements_PropKeys =
    [ PKey.ToolTipHost<'TView>.ContentView_viewSpec.Untyped ]
    |> List.append base.SubElements_PropKeys

  override _.ApplyNativeProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    ToolTipHostPropHandler<'TView>.applyNativeProps (terminalElement, props)

  override this.ClearProp(propertyId: PropertyId) =
    ToolTipHostPropHandler<'TView>.clearProp (this, propertyId)

type internal TreeViewTerminalElement<'T when 'T: not struct>(props: Props) =
  inherit ViewTerminalElement(props)

  override _.Name = "TreeView`1"

  override _.NewView() = new TreeView<'T>()

  override _.SetAsChildOfParentView = true

  override _.ApplyNativeProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    TreeViewPropHandler<'T>.applyNativeProps (terminalElement, props)

  override this.ClearProp(propertyId: PropertyId) =
    TreeViewPropHandler<'T>.clearProp (this, propertyId)

type internal TreeViewTerminalElement(props: Props) =
  inherit TreeViewTerminalElement<Terminal.Gui.Views.ITreeNode>(props)

  override _.Name = "TreeView"

  override _.NewView() = new TreeView()

  override _.SetAsChildOfParentView = true

  override _.ApplyNativeProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    TreeViewPropHandler.applyNativeProps (terminalElement, props)

  override this.ClearProp(propertyId: PropertyId) =
    TreeViewPropHandler.clearProp (this, propertyId)

type internal WindowTerminalElement(props: Props) =
  inherit RunnableTerminalElement(props)

  override _.Name = "Window"

  override _.NewView() = new Window()

  override _.SetAsChildOfParentView = true

  override _.ApplyNativeProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    WindowPropHandler.applyNativeProps (terminalElement, props)

  override this.ClearProp(propertyId: PropertyId) =
    WindowPropHandler.clearProp (this, propertyId)

type internal WizardTerminalElement(props: Props) =
  inherit DialogTerminalElement(props)

  override _.Name = "Wizard"

  override _.NewView() = new Wizard()

  override _.SetAsChildOfParentView = true

  override this.SubElements_PropKeys =
    [ PKey.Wizard.CurrentStep_viewSpec.Untyped ]
    |> List.append base.SubElements_PropKeys

  override _.ApplyNativeProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    WizardPropHandler.applyNativeProps (terminalElement, props)

  override this.ClearProp(propertyId: PropertyId) =
    WizardPropHandler.clearProp (this, propertyId)

type internal WizardStepTerminalElement(props: Props) =
  inherit ViewTerminalElement(props)

  override _.Name = "WizardStep"

  override _.NewView() = new WizardStep()

  override _.SetAsChildOfParentView = true

  override _.ApplyNativeProps(terminalElement: ViewBackedTerminalElement, props: Props) =
    WizardStepPropHandler.applyNativeProps (terminalElement, props)

  override this.ClearProp(propertyId: PropertyId) =
    WizardStepPropHandler.clearProp (this, propertyId)
