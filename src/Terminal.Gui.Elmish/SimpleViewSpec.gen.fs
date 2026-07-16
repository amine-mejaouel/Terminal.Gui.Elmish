namespace Terminal.Gui.Elmish

type AdornmentView(props: AdornmentViewProps) =
  let mutable viewTe: IViewTE voption = ValueNone

  let getOrCreateViewTE () =
    match viewTe with
    | ValueSome value -> value
    | ValueNone ->
      let value = new AdornmentViewTerminalElement(props.props) :> IViewTE
      viewTe <- ValueSome value
      value

  interface ISimpleViewSpec with
    member _.CreateViewTE() = getOrCreateViewTE ()
    member _.BindViewTE(value) = viewTe <- ValueSome value
    member _.Props = props.props

    member _.SetProps(target, changedProps) =
      AdornmentViewPropHandler.setProps (target :?> ViewBackedTerminalElement, changedProps)

    member _.ClearProp(target, propertyId) =
      AdornmentViewPropHandler.clearProp (target :?> ViewBackedTerminalElement, propertyId)

    member _.ViewType = ViewType.``ViewBaseAdornmentView``

type AttributePicker(props: AttributePickerProps) =
  let mutable viewTe: IViewTE voption = ValueNone

  let getOrCreateViewTE () =
    match viewTe with
    | ValueSome value -> value
    | ValueNone ->
      let value = new AttributePickerTerminalElement(props.props) :> IViewTE
      viewTe <- ValueSome value
      value

  interface ISimpleViewSpec with
    member _.CreateViewTE() = getOrCreateViewTE ()
    member _.BindViewTE(value) = viewTe <- ValueSome value
    member _.Props = props.props

    member _.SetProps(target, changedProps) =
      AttributePickerPropHandler.setProps (target :?> ViewBackedTerminalElement, changedProps)

    member _.ClearProp(target, propertyId) =
      AttributePickerPropHandler.clearProp (target :?> ViewBackedTerminalElement, propertyId)

    member _.ViewType = ViewType.``ViewsAttributePicker``

type Bar(props: BarProps) =
  let mutable viewTe: IViewTE voption = ValueNone

  let getOrCreateViewTE () =
    match viewTe with
    | ValueSome value -> value
    | ValueNone ->
      let value = new BarTerminalElement(props.props) :> IViewTE
      viewTe <- ValueSome value
      value

  interface ISimpleViewSpec with
    member _.CreateViewTE() = getOrCreateViewTE ()
    member _.BindViewTE(value) = viewTe <- ValueSome value
    member _.Props = props.props

    member _.SetProps(target, changedProps) =
      BarPropHandler.setProps (target :?> ViewBackedTerminalElement, changedProps)

    member _.ClearProp(target, propertyId) =
      BarPropHandler.clearProp (target :?> ViewBackedTerminalElement, propertyId)

    member _.ViewType = ViewType.``ViewsBar``

type BorderView(props: BorderViewProps) =
  let mutable viewTe: IViewTE voption = ValueNone

  let getOrCreateViewTE () =
    match viewTe with
    | ValueSome value -> value
    | ValueNone ->
      let value = new BorderViewTerminalElement(props.props) :> IViewTE
      viewTe <- ValueSome value
      value

  interface ISimpleViewSpec with
    member _.CreateViewTE() = getOrCreateViewTE ()
    member _.BindViewTE(value) = viewTe <- ValueSome value
    member _.Props = props.props

    member _.SetProps(target, changedProps) =
      BorderViewPropHandler.setProps (target :?> ViewBackedTerminalElement, changedProps)

    member _.ClearProp(target, propertyId) =
      BorderViewPropHandler.clearProp (target :?> ViewBackedTerminalElement, propertyId)

    member _.ViewType = ViewType.``ViewBaseBorderView``

type Button(props: ButtonProps) =
  let mutable viewTe: IViewTE voption = ValueNone

  let getOrCreateViewTE () =
    match viewTe with
    | ValueSome value -> value
    | ValueNone ->
      let value = new ButtonTerminalElement(props.props) :> IViewTE
      viewTe <- ValueSome value
      value

  interface ISimpleViewSpec with
    member _.CreateViewTE() = getOrCreateViewTE ()
    member _.BindViewTE(value) = viewTe <- ValueSome value
    member _.Props = props.props

    member _.SetProps(target, changedProps) =
      ButtonPropHandler.setProps (target :?> ViewBackedTerminalElement, changedProps)

    member _.ClearProp(target, propertyId) =
      ButtonPropHandler.clearProp (target :?> ViewBackedTerminalElement, propertyId)

    member _.ViewType = ViewType.``ViewsButton``

type CharMap(props: CharMapProps) =
  let mutable viewTe: IViewTE voption = ValueNone

  let getOrCreateViewTE () =
    match viewTe with
    | ValueSome value -> value
    | ValueNone ->
      let value = new CharMapTerminalElement(props.props) :> IViewTE
      viewTe <- ValueSome value
      value

  interface ISimpleViewSpec with
    member _.CreateViewTE() = getOrCreateViewTE ()
    member _.BindViewTE(value) = viewTe <- ValueSome value
    member _.Props = props.props

    member _.SetProps(target, changedProps) =
      CharMapPropHandler.setProps (target :?> ViewBackedTerminalElement, changedProps)

    member _.ClearProp(target, propertyId) =
      CharMapPropHandler.clearProp (target :?> ViewBackedTerminalElement, propertyId)

    member _.ViewType = ViewType.``ViewsCharMap``

type CheckBox(props: CheckBoxProps) =
  let mutable viewTe: IViewTE voption = ValueNone

  let getOrCreateViewTE () =
    match viewTe with
    | ValueSome value -> value
    | ValueNone ->
      let value = new CheckBoxTerminalElement(props.props) :> IViewTE
      viewTe <- ValueSome value
      value

  interface ISimpleViewSpec with
    member _.CreateViewTE() = getOrCreateViewTE ()
    member _.BindViewTE(value) = viewTe <- ValueSome value
    member _.Props = props.props

    member _.SetProps(target, changedProps) =
      CheckBoxPropHandler.setProps (target :?> ViewBackedTerminalElement, changedProps)

    member _.ClearProp(target, propertyId) =
      CheckBoxPropHandler.clearProp (target :?> ViewBackedTerminalElement, propertyId)

    member _.ViewType = ViewType.``ViewsCheckBox``

type Code(props: CodeProps) =
  let mutable viewTe: IViewTE voption = ValueNone

  let getOrCreateViewTE () =
    match viewTe with
    | ValueSome value -> value
    | ValueNone ->
      let value = new CodeTerminalElement(props.props) :> IViewTE
      viewTe <- ValueSome value
      value

  interface ISimpleViewSpec with
    member _.CreateViewTE() = getOrCreateViewTE ()
    member _.BindViewTE(value) = viewTe <- ValueSome value
    member _.Props = props.props

    member _.SetProps(target, changedProps) =
      CodePropHandler.setProps (target :?> ViewBackedTerminalElement, changedProps)

    member _.ClearProp(target, propertyId) =
      CodePropHandler.clearProp (target :?> ViewBackedTerminalElement, propertyId)

    member _.ViewType = ViewType.``ViewsCode``

type ColorPicker(props: ColorPickerProps) =
  let mutable viewTe: IViewTE voption = ValueNone

  let getOrCreateViewTE () =
    match viewTe with
    | ValueSome value -> value
    | ValueNone ->
      let value = new ColorPickerTerminalElement(props.props) :> IViewTE
      viewTe <- ValueSome value
      value

  interface ISimpleViewSpec with
    member _.CreateViewTE() = getOrCreateViewTE ()
    member _.BindViewTE(value) = viewTe <- ValueSome value
    member _.Props = props.props

    member _.SetProps(target, changedProps) =
      ColorPickerPropHandler.setProps (target :?> ViewBackedTerminalElement, changedProps)

    member _.ClearProp(target, propertyId) =
      ColorPickerPropHandler.clearProp (target :?> ViewBackedTerminalElement, propertyId)

    member _.ViewType = ViewType.``ViewsColorPicker``

type ColorPicker16(props: ColorPicker16Props) =
  let mutable viewTe: IViewTE voption = ValueNone

  let getOrCreateViewTE () =
    match viewTe with
    | ValueSome value -> value
    | ValueNone ->
      let value = new ColorPicker16TerminalElement(props.props) :> IViewTE
      viewTe <- ValueSome value
      value

  interface ISimpleViewSpec with
    member _.CreateViewTE() = getOrCreateViewTE ()
    member _.BindViewTE(value) = viewTe <- ValueSome value
    member _.Props = props.props

    member _.SetProps(target, changedProps) =
      ColorPicker16PropHandler.setProps (target :?> ViewBackedTerminalElement, changedProps)

    member _.ClearProp(target, propertyId) =
      ColorPicker16PropHandler.clearProp (target :?> ViewBackedTerminalElement, propertyId)

    member _.ViewType = ViewType.``ViewsColorPicker16``

type DatePicker(props: DatePickerProps) =
  let mutable viewTe: IViewTE voption = ValueNone

  let getOrCreateViewTE () =
    match viewTe with
    | ValueSome value -> value
    | ValueNone ->
      let value = new DatePickerTerminalElement(props.props) :> IViewTE
      viewTe <- ValueSome value
      value

  interface ISimpleViewSpec with
    member _.CreateViewTE() = getOrCreateViewTE ()
    member _.BindViewTE(value) = viewTe <- ValueSome value
    member _.Props = props.props

    member _.SetProps(target, changedProps) =
      DatePickerPropHandler.setProps (target :?> ViewBackedTerminalElement, changedProps)

    member _.ClearProp(target, propertyId) =
      DatePickerPropHandler.clearProp (target :?> ViewBackedTerminalElement, propertyId)

    member _.ViewType = ViewType.``ViewsDatePicker``

type FrameView(props: FrameViewProps) =
  let mutable viewTe: IViewTE voption = ValueNone

  let getOrCreateViewTE () =
    match viewTe with
    | ValueSome value -> value
    | ValueNone ->
      let value = new FrameViewTerminalElement(props.props) :> IViewTE
      viewTe <- ValueSome value
      value

  interface ISimpleViewSpec with
    member _.CreateViewTE() = getOrCreateViewTE ()
    member _.BindViewTE(value) = viewTe <- ValueSome value
    member _.Props = props.props

    member _.SetProps(target, changedProps) =
      FrameViewPropHandler.setProps (target :?> ViewBackedTerminalElement, changedProps)

    member _.ClearProp(target, propertyId) =
      FrameViewPropHandler.clearProp (target :?> ViewBackedTerminalElement, propertyId)

    member _.ViewType = ViewType.``ViewsFrameView``

type GraphView(props: GraphViewProps) =
  let mutable viewTe: IViewTE voption = ValueNone

  let getOrCreateViewTE () =
    match viewTe with
    | ValueSome value -> value
    | ValueNone ->
      let value = new GraphViewTerminalElement(props.props) :> IViewTE
      viewTe <- ValueSome value
      value

  interface ISimpleViewSpec with
    member _.CreateViewTE() = getOrCreateViewTE ()
    member _.BindViewTE(value) = viewTe <- ValueSome value
    member _.Props = props.props

    member _.SetProps(target, changedProps) =
      GraphViewPropHandler.setProps (target :?> ViewBackedTerminalElement, changedProps)

    member _.ClearProp(target, propertyId) =
      GraphViewPropHandler.clearProp (target :?> ViewBackedTerminalElement, propertyId)

    member _.ViewType = ViewType.``ViewsGraphView``

type HexView(props: HexViewProps) =
  let mutable viewTe: IViewTE voption = ValueNone

  let getOrCreateViewTE () =
    match viewTe with
    | ValueSome value -> value
    | ValueNone ->
      let value = new HexViewTerminalElement(props.props) :> IViewTE
      viewTe <- ValueSome value
      value

  interface ISimpleViewSpec with
    member _.CreateViewTE() = getOrCreateViewTE ()
    member _.BindViewTE(value) = viewTe <- ValueSome value
    member _.Props = props.props

    member _.SetProps(target, changedProps) =
      HexViewPropHandler.setProps (target :?> ViewBackedTerminalElement, changedProps)

    member _.ClearProp(target, propertyId) =
      HexViewPropHandler.clearProp (target :?> ViewBackedTerminalElement, propertyId)

    member _.ViewType = ViewType.``ViewsHexView``

type ImageView(props: ImageViewProps) =
  let mutable viewTe: IViewTE voption = ValueNone

  let getOrCreateViewTE () =
    match viewTe with
    | ValueSome value -> value
    | ValueNone ->
      let value = new ImageViewTerminalElement(props.props) :> IViewTE
      viewTe <- ValueSome value
      value

  interface ISimpleViewSpec with
    member _.CreateViewTE() = getOrCreateViewTE ()
    member _.BindViewTE(value) = viewTe <- ValueSome value
    member _.Props = props.props

    member _.SetProps(target, changedProps) =
      ImageViewPropHandler.setProps (target :?> ViewBackedTerminalElement, changedProps)

    member _.ClearProp(target, propertyId) =
      ImageViewPropHandler.clearProp (target :?> ViewBackedTerminalElement, propertyId)

    member _.ViewType = ViewType.``ViewsImageView``

type Label(props: LabelProps) =
  let mutable viewTe: IViewTE voption = ValueNone

  let getOrCreateViewTE () =
    match viewTe with
    | ValueSome value -> value
    | ValueNone ->
      let value = new LabelTerminalElement(props.props) :> IViewTE
      viewTe <- ValueSome value
      value

  interface ISimpleViewSpec with
    member _.CreateViewTE() = getOrCreateViewTE ()
    member _.BindViewTE(value) = viewTe <- ValueSome value
    member _.Props = props.props

    member _.SetProps(target, changedProps) =
      LabelPropHandler.setProps (target :?> ViewBackedTerminalElement, changedProps)

    member _.ClearProp(target, propertyId) =
      LabelPropHandler.clearProp (target :?> ViewBackedTerminalElement, propertyId)

    member _.ViewType = ViewType.``ViewsLabel``

type LegendAnnotation(props: LegendAnnotationProps) =
  let mutable viewTe: IViewTE voption = ValueNone

  let getOrCreateViewTE () =
    match viewTe with
    | ValueSome value -> value
    | ValueNone ->
      let value = new LegendAnnotationTerminalElement(props.props) :> IViewTE
      viewTe <- ValueSome value
      value

  interface ISimpleViewSpec with
    member _.CreateViewTE() = getOrCreateViewTE ()
    member _.BindViewTE(value) = viewTe <- ValueSome value
    member _.Props = props.props

    member _.SetProps(target, changedProps) =
      LegendAnnotationPropHandler.setProps (target :?> ViewBackedTerminalElement, changedProps)

    member _.ClearProp(target, propertyId) =
      LegendAnnotationPropHandler.clearProp (target :?> ViewBackedTerminalElement, propertyId)

    member _.ViewType = ViewType.``ViewsLegendAnnotation``

type Line(props: LineProps) =
  let mutable viewTe: IViewTE voption = ValueNone

  let getOrCreateViewTE () =
    match viewTe with
    | ValueSome value -> value
    | ValueNone ->
      let value = new LineTerminalElement(props.props) :> IViewTE
      viewTe <- ValueSome value
      value

  interface ISimpleViewSpec with
    member _.CreateViewTE() = getOrCreateViewTE ()
    member _.BindViewTE(value) = viewTe <- ValueSome value
    member _.Props = props.props

    member _.SetProps(target, changedProps) =
      LinePropHandler.setProps (target :?> ViewBackedTerminalElement, changedProps)

    member _.ClearProp(target, propertyId) =
      LinePropHandler.clearProp (target :?> ViewBackedTerminalElement, propertyId)

    member _.ViewType = ViewType.``ViewsLine``

type LinearMultiSelector<'T>(props: LinearMultiSelectorProps<'T>) =
  let mutable viewTe: IViewTE voption = ValueNone

  let getOrCreateViewTE () =
    match viewTe with
    | ValueSome value -> value
    | ValueNone ->
      let value = new LinearMultiSelectorTerminalElement<'T>(props.props) :> IViewTE
      viewTe <- ValueSome value
      value

  interface ISimpleViewSpec with
    member _.CreateViewTE() = getOrCreateViewTE ()
    member _.BindViewTE(value) = viewTe <- ValueSome value
    member _.Props = props.props

    member _.SetProps(target, changedProps) =
      LinearMultiSelectorPropHandler<'T>.setProps (target :?> ViewBackedTerminalElement, changedProps)

    member _.ClearProp(target, propertyId) =
      LinearMultiSelectorPropHandler<'T>.clearProp (target :?> ViewBackedTerminalElement, propertyId)

    member _.ViewType = ViewType.``LinearMultiSelector<'T>``

type LinearMultiSelector(props: LinearMultiSelectorProps) =
  let mutable viewTe: IViewTE voption = ValueNone

  let getOrCreateViewTE () =
    match viewTe with
    | ValueSome value -> value
    | ValueNone ->
      let value = new LinearMultiSelectorTerminalElement(props.props) :> IViewTE
      viewTe <- ValueSome value
      value

  interface ISimpleViewSpec with
    member _.CreateViewTE() = getOrCreateViewTE ()
    member _.BindViewTE(value) = viewTe <- ValueSome value
    member _.Props = props.props

    member _.SetProps(target, changedProps) =
      LinearMultiSelectorPropHandler.setProps (target :?> ViewBackedTerminalElement, changedProps)

    member _.ClearProp(target, propertyId) =
      LinearMultiSelectorPropHandler.clearProp (target :?> ViewBackedTerminalElement, propertyId)

    member _.ViewType = ViewType.``ViewsLinearMultiSelector``

type LinearRange<'T>(props: LinearRangeProps<'T>) =
  let mutable viewTe: IViewTE voption = ValueNone

  let getOrCreateViewTE () =
    match viewTe with
    | ValueSome value -> value
    | ValueNone ->
      let value = new LinearRangeTerminalElement<'T>(props.props) :> IViewTE
      viewTe <- ValueSome value
      value

  interface ISimpleViewSpec with
    member _.CreateViewTE() = getOrCreateViewTE ()
    member _.BindViewTE(value) = viewTe <- ValueSome value
    member _.Props = props.props

    member _.SetProps(target, changedProps) =
      LinearRangePropHandler<'T>.setProps (target :?> ViewBackedTerminalElement, changedProps)

    member _.ClearProp(target, propertyId) =
      LinearRangePropHandler<'T>.clearProp (target :?> ViewBackedTerminalElement, propertyId)

    member _.ViewType = ViewType.``LinearRange<'T>``

type LinearRange(props: LinearRangeProps) =
  let mutable viewTe: IViewTE voption = ValueNone

  let getOrCreateViewTE () =
    match viewTe with
    | ValueSome value -> value
    | ValueNone ->
      let value = new LinearRangeTerminalElement(props.props) :> IViewTE
      viewTe <- ValueSome value
      value

  interface ISimpleViewSpec with
    member _.CreateViewTE() = getOrCreateViewTE ()
    member _.BindViewTE(value) = viewTe <- ValueSome value
    member _.Props = props.props

    member _.SetProps(target, changedProps) =
      LinearRangePropHandler.setProps (target :?> ViewBackedTerminalElement, changedProps)

    member _.ClearProp(target, propertyId) =
      LinearRangePropHandler.clearProp (target :?> ViewBackedTerminalElement, propertyId)

    member _.ViewType = ViewType.``ViewsLinearRange``

type LinearSelector<'T>(props: LinearSelectorProps<'T>) =
  let mutable viewTe: IViewTE voption = ValueNone

  let getOrCreateViewTE () =
    match viewTe with
    | ValueSome value -> value
    | ValueNone ->
      let value = new LinearSelectorTerminalElement<'T>(props.props) :> IViewTE
      viewTe <- ValueSome value
      value

  interface ISimpleViewSpec with
    member _.CreateViewTE() = getOrCreateViewTE ()
    member _.BindViewTE(value) = viewTe <- ValueSome value
    member _.Props = props.props

    member _.SetProps(target, changedProps) =
      LinearSelectorPropHandler<'T>.setProps (target :?> ViewBackedTerminalElement, changedProps)

    member _.ClearProp(target, propertyId) =
      LinearSelectorPropHandler<'T>.clearProp (target :?> ViewBackedTerminalElement, propertyId)

    member _.ViewType = ViewType.``LinearSelector<'T>``

type LinearSelector(props: LinearSelectorProps) =
  let mutable viewTe: IViewTE voption = ValueNone

  let getOrCreateViewTE () =
    match viewTe with
    | ValueSome value -> value
    | ValueNone ->
      let value = new LinearSelectorTerminalElement(props.props) :> IViewTE
      viewTe <- ValueSome value
      value

  interface ISimpleViewSpec with
    member _.CreateViewTE() = getOrCreateViewTE ()
    member _.BindViewTE(value) = viewTe <- ValueSome value
    member _.Props = props.props

    member _.SetProps(target, changedProps) =
      LinearSelectorPropHandler.setProps (target :?> ViewBackedTerminalElement, changedProps)

    member _.ClearProp(target, propertyId) =
      LinearSelectorPropHandler.clearProp (target :?> ViewBackedTerminalElement, propertyId)

    member _.ViewType = ViewType.``ViewsLinearSelector``

type Link(props: LinkProps) =
  let mutable viewTe: IViewTE voption = ValueNone

  let getOrCreateViewTE () =
    match viewTe with
    | ValueSome value -> value
    | ValueNone ->
      let value = new LinkTerminalElement(props.props) :> IViewTE
      viewTe <- ValueSome value
      value

  interface ISimpleViewSpec with
    member _.CreateViewTE() = getOrCreateViewTE ()
    member _.BindViewTE(value) = viewTe <- ValueSome value
    member _.Props = props.props

    member _.SetProps(target, changedProps) =
      LinkPropHandler.setProps (target :?> ViewBackedTerminalElement, changedProps)

    member _.ClearProp(target, propertyId) =
      LinkPropHandler.clearProp (target :?> ViewBackedTerminalElement, propertyId)

    member _.ViewType = ViewType.``ViewsLink``

type ListView(props: ListViewProps) =
  let mutable viewTe: IViewTE voption = ValueNone

  let getOrCreateViewTE () =
    match viewTe with
    | ValueSome value -> value
    | ValueNone ->
      let value = new ListViewTerminalElement(props.props) :> IViewTE
      viewTe <- ValueSome value
      value

  interface ISimpleViewSpec with
    member _.CreateViewTE() = getOrCreateViewTE ()
    member _.BindViewTE(value) = viewTe <- ValueSome value
    member _.Props = props.props

    member _.SetProps(target, changedProps) =
      ListViewPropHandler.setProps (target :?> ViewBackedTerminalElement, changedProps)

    member _.ClearProp(target, propertyId) =
      ListViewPropHandler.clearProp (target :?> ViewBackedTerminalElement, propertyId)

    member _.ViewType = ViewType.``ViewsListView``

type ListView<'T>(props: ListViewProps<'T>) =
  let mutable viewTe: IViewTE voption = ValueNone

  let getOrCreateViewTE () =
    match viewTe with
    | ValueSome value -> value
    | ValueNone ->
      let value = new ListViewTerminalElement<'T>(props.props) :> IViewTE
      viewTe <- ValueSome value
      value

  interface ISimpleViewSpec with
    member _.CreateViewTE() = getOrCreateViewTE ()
    member _.BindViewTE(value) = viewTe <- ValueSome value
    member _.Props = props.props

    member _.SetProps(target, changedProps) =
      ListViewPropHandler<'T>.setProps (target :?> ViewBackedTerminalElement, changedProps)

    member _.ClearProp(target, propertyId) =
      ListViewPropHandler<'T>.clearProp (target :?> ViewBackedTerminalElement, propertyId)

    member _.ViewType = ViewType.``ListView<'T>``

type MarginView(props: MarginViewProps) =
  let mutable viewTe: IViewTE voption = ValueNone

  let getOrCreateViewTE () =
    match viewTe with
    | ValueSome value -> value
    | ValueNone ->
      let value = new MarginViewTerminalElement(props.props) :> IViewTE
      viewTe <- ValueSome value
      value

  interface ISimpleViewSpec with
    member _.CreateViewTE() = getOrCreateViewTE ()
    member _.BindViewTE(value) = viewTe <- ValueSome value
    member _.Props = props.props

    member _.SetProps(target, changedProps) =
      MarginViewPropHandler.setProps (target :?> ViewBackedTerminalElement, changedProps)

    member _.ClearProp(target, propertyId) =
      MarginViewPropHandler.clearProp (target :?> ViewBackedTerminalElement, propertyId)

    member _.ViewType = ViewType.``ViewBaseMarginView``

type Markdown(props: MarkdownProps) =
  let mutable viewTe: IViewTE voption = ValueNone

  let getOrCreateViewTE () =
    match viewTe with
    | ValueSome value -> value
    | ValueNone ->
      let value = new MarkdownTerminalElement(props.props) :> IViewTE
      viewTe <- ValueSome value
      value

  interface ISimpleViewSpec with
    member _.CreateViewTE() = getOrCreateViewTE ()
    member _.BindViewTE(value) = viewTe <- ValueSome value
    member _.Props = props.props

    member _.SetProps(target, changedProps) =
      MarkdownPropHandler.setProps (target :?> ViewBackedTerminalElement, changedProps)

    member _.ClearProp(target, propertyId) =
      MarkdownPropHandler.clearProp (target :?> ViewBackedTerminalElement, propertyId)

    member _.ViewType = ViewType.``ViewsMarkdown``

type MarkdownCodeBlock(props: MarkdownCodeBlockProps) =
  let mutable viewTe: IViewTE voption = ValueNone

  let getOrCreateViewTE () =
    match viewTe with
    | ValueSome value -> value
    | ValueNone ->
      let value = new MarkdownCodeBlockTerminalElement(props.props) :> IViewTE
      viewTe <- ValueSome value
      value

  interface ISimpleViewSpec with
    member _.CreateViewTE() = getOrCreateViewTE ()
    member _.BindViewTE(value) = viewTe <- ValueSome value
    member _.Props = props.props

    member _.SetProps(target, changedProps) =
      MarkdownCodeBlockPropHandler.setProps (target :?> ViewBackedTerminalElement, changedProps)

    member _.ClearProp(target, propertyId) =
      MarkdownCodeBlockPropHandler.clearProp (target :?> ViewBackedTerminalElement, propertyId)

    member _.ViewType = ViewType.``ViewsMarkdownCodeBlock``

type MarkdownTable(props: MarkdownTableProps) =
  let mutable viewTe: IViewTE voption = ValueNone

  let getOrCreateViewTE () =
    match viewTe with
    | ValueSome value -> value
    | ValueNone ->
      let value = new MarkdownTableTerminalElement(props.props) :> IViewTE
      viewTe <- ValueSome value
      value

  interface ISimpleViewSpec with
    member _.CreateViewTE() = getOrCreateViewTE ()
    member _.BindViewTE(value) = viewTe <- ValueSome value
    member _.Props = props.props

    member _.SetProps(target, changedProps) =
      MarkdownTablePropHandler.setProps (target :?> ViewBackedTerminalElement, changedProps)

    member _.ClearProp(target, propertyId) =
      MarkdownTablePropHandler.clearProp (target :?> ViewBackedTerminalElement, propertyId)

    member _.ViewType = ViewType.``ViewsMarkdownTable``

type Menu(props: MenuProps) =
  let mutable viewTe: IViewTE voption = ValueNone

  let getOrCreateViewTE () =
    match viewTe with
    | ValueSome value -> value
    | ValueNone ->
      let value = new MenuTerminalElement(props.props) :> IViewTE
      viewTe <- ValueSome value
      value

  interface ISimpleViewSpec with
    member _.CreateViewTE() = getOrCreateViewTE ()
    member _.BindViewTE(value) = viewTe <- ValueSome value
    member _.Props = props.props

    member _.SetProps(target, changedProps) =
      MenuPropHandler.setProps (target :?> ViewBackedTerminalElement, changedProps)

    member _.ClearProp(target, propertyId) =
      MenuPropHandler.clearProp (target :?> ViewBackedTerminalElement, propertyId)

    member _.ViewType = ViewType.``ViewsMenu``

  interface IMenuView

type MenuBar(props: MenuBarProps) =
  let mutable viewTe: IViewTE voption = ValueNone

  let getOrCreateViewTE () =
    match viewTe with
    | ValueSome value -> value
    | ValueNone ->
      let value = new MenuBarTerminalElement(props.props) :> IViewTE
      viewTe <- ValueSome value
      value

  interface ISimpleViewSpec with
    member _.CreateViewTE() = getOrCreateViewTE ()
    member _.BindViewTE(value) = viewTe <- ValueSome value
    member _.Props = props.props

    member _.SetProps(target, changedProps) =
      MenuBarPropHandler.setProps (target :?> ViewBackedTerminalElement, changedProps)

    member _.ClearProp(target, propertyId) =
      MenuBarPropHandler.clearProp (target :?> ViewBackedTerminalElement, propertyId)

    member _.ViewType = ViewType.``ViewsMenuBar``

  interface IMenuView

type NumericUpDown<'T>(props: NumericUpDownProps<'T>) =
  let mutable viewTe: IViewTE voption = ValueNone

  let getOrCreateViewTE () =
    match viewTe with
    | ValueSome value -> value
    | ValueNone ->
      let value = new NumericUpDownTerminalElement<'T>(props.props) :> IViewTE
      viewTe <- ValueSome value
      value

  interface ISimpleViewSpec with
    member _.CreateViewTE() = getOrCreateViewTE ()
    member _.BindViewTE(value) = viewTe <- ValueSome value
    member _.Props = props.props

    member _.SetProps(target, changedProps) =
      NumericUpDownPropHandler<'T>.setProps (target :?> ViewBackedTerminalElement, changedProps)

    member _.ClearProp(target, propertyId) =
      NumericUpDownPropHandler<'T>.clearProp (target :?> ViewBackedTerminalElement, propertyId)

    member _.ViewType = ViewType.``NumericUpDown<'T>``

type NumericUpDown(props: NumericUpDownProps) =
  let mutable viewTe: IViewTE voption = ValueNone

  let getOrCreateViewTE () =
    match viewTe with
    | ValueSome value -> value
    | ValueNone ->
      let value = new NumericUpDownTerminalElement(props.props) :> IViewTE
      viewTe <- ValueSome value
      value

  interface ISimpleViewSpec with
    member _.CreateViewTE() = getOrCreateViewTE ()
    member _.BindViewTE(value) = viewTe <- ValueSome value
    member _.Props = props.props

    member _.SetProps(target, changedProps) =
      NumericUpDownPropHandler.setProps (target :?> ViewBackedTerminalElement, changedProps)

    member _.ClearProp(target, propertyId) =
      NumericUpDownPropHandler.clearProp (target :?> ViewBackedTerminalElement, propertyId)

    member _.ViewType = ViewType.``ViewsNumericUpDown``

type PaddingView(props: PaddingViewProps) =
  let mutable viewTe: IViewTE voption = ValueNone

  let getOrCreateViewTE () =
    match viewTe with
    | ValueSome value -> value
    | ValueNone ->
      let value = new PaddingViewTerminalElement(props.props) :> IViewTE
      viewTe <- ValueSome value
      value

  interface ISimpleViewSpec with
    member _.CreateViewTE() = getOrCreateViewTE ()
    member _.BindViewTE(value) = viewTe <- ValueSome value
    member _.Props = props.props

    member _.SetProps(target, changedProps) =
      PaddingViewPropHandler.setProps (target :?> ViewBackedTerminalElement, changedProps)

    member _.ClearProp(target, propertyId) =
      PaddingViewPropHandler.clearProp (target :?> ViewBackedTerminalElement, propertyId)

    member _.ViewType = ViewType.``ViewBasePaddingView``

type Popover<'TView, 'TResult when 'TView: (new: unit -> 'TView) and 'TView :> Terminal.Gui.ViewBase.View>
  (props: PopoverProps<'TView, 'TResult>) =
  let mutable viewTe: IViewTE voption = ValueNone

  let getOrCreateViewTE () =
    match viewTe with
    | ValueSome value -> value
    | ValueNone ->
      let value = new PopoverTerminalElement<'TView, 'TResult>(props.props) :> IViewTE
      viewTe <- ValueSome value
      value

  interface ISimpleViewSpec with
    member _.CreateViewTE() = getOrCreateViewTE ()
    member _.BindViewTE(value) = viewTe <- ValueSome value
    member _.Props = props.props

    member _.SetProps(target, changedProps) =
      PopoverPropHandler<'TView, 'TResult>.setProps (target :?> ViewBackedTerminalElement, changedProps)

    member _.ClearProp(target, propertyId) =
      PopoverPropHandler<'TView, 'TResult>.clearProp (target :?> ViewBackedTerminalElement, propertyId)

    member _.ViewType = ViewType.``Popover<'TView, 'TResult>``

type PopoverMenu(props: PopoverMenuProps) =
  let mutable viewTe: IViewTE voption = ValueNone

  let getOrCreateViewTE () =
    match viewTe with
    | ValueSome value -> value
    | ValueNone ->
      let value = new PopoverMenuTerminalElement(props.props) :> IViewTE
      viewTe <- ValueSome value
      value

  interface ISimpleViewSpec with
    member _.CreateViewTE() = getOrCreateViewTE ()
    member _.BindViewTE(value) = viewTe <- ValueSome value
    member _.Props = props.props

    member _.SetProps(target, changedProps) =
      PopoverMenuPropHandler.setProps (target :?> ViewBackedTerminalElement, changedProps)

    member _.ClearProp(target, propertyId) =
      PopoverMenuPropHandler.clearProp (target :?> ViewBackedTerminalElement, propertyId)

    member _.ViewType = ViewType.``ViewsPopoverMenu``

  interface IPopoverMenuView

type ProgressBar(props: ProgressBarProps) =
  let mutable viewTe: IViewTE voption = ValueNone

  let getOrCreateViewTE () =
    match viewTe with
    | ValueSome value -> value
    | ValueNone ->
      let value = new ProgressBarTerminalElement(props.props) :> IViewTE
      viewTe <- ValueSome value
      value

  interface ISimpleViewSpec with
    member _.CreateViewTE() = getOrCreateViewTE ()
    member _.BindViewTE(value) = viewTe <- ValueSome value
    member _.Props = props.props

    member _.SetProps(target, changedProps) =
      ProgressBarPropHandler.setProps (target :?> ViewBackedTerminalElement, changedProps)

    member _.ClearProp(target, propertyId) =
      ProgressBarPropHandler.clearProp (target :?> ViewBackedTerminalElement, propertyId)

    member _.ViewType = ViewType.``ViewsProgressBar``

type Runnable(props: RunnableProps) =
  let mutable viewTe: IViewTE voption = ValueNone

  let getOrCreateViewTE () =
    match viewTe with
    | ValueSome value -> value
    | ValueNone ->
      let value = new RunnableTerminalElement(props.props) :> IViewTE
      viewTe <- ValueSome value
      value

  interface ISimpleViewSpec with
    member _.CreateViewTE() = getOrCreateViewTE ()
    member _.BindViewTE(value) = viewTe <- ValueSome value
    member _.Props = props.props

    member _.SetProps(target, changedProps) =
      RunnablePropHandler.setProps (target :?> ViewBackedTerminalElement, changedProps)

    member _.ClearProp(target, propertyId) =
      RunnablePropHandler.clearProp (target :?> ViewBackedTerminalElement, propertyId)

    member _.ViewType = ViewType.``ViewsRunnable``

type Runnable<'TResult>(props: RunnableProps<'TResult>) =
  let mutable viewTe: IViewTE voption = ValueNone

  let getOrCreateViewTE () =
    match viewTe with
    | ValueSome value -> value
    | ValueNone ->
      let value = new RunnableTerminalElement<'TResult>(props.props) :> IViewTE
      viewTe <- ValueSome value
      value

  interface ISimpleViewSpec with
    member _.CreateViewTE() = getOrCreateViewTE ()
    member _.BindViewTE(value) = viewTe <- ValueSome value
    member _.Props = props.props

    member _.SetProps(target, changedProps) =
      RunnablePropHandler<'TResult>.setProps (target :?> ViewBackedTerminalElement, changedProps)

    member _.ClearProp(target, propertyId) =
      RunnablePropHandler<'TResult>.clearProp (target :?> ViewBackedTerminalElement, propertyId)

    member _.ViewType = ViewType.``Runnable<'TResult>``

type Dialog<'TResult>(props: DialogProps<'TResult>) =
  let mutable viewTe: IViewTE voption = ValueNone

  let getOrCreateViewTE () =
    match viewTe with
    | ValueSome value -> value
    | ValueNone ->
      let value = new DialogTerminalElement<'TResult>(props.props) :> IViewTE
      viewTe <- ValueSome value
      value

  interface ISimpleViewSpec with
    member _.CreateViewTE() = getOrCreateViewTE ()
    member _.BindViewTE(value) = viewTe <- ValueSome value
    member _.Props = props.props

    member _.SetProps(target, changedProps) =
      DialogPropHandler<'TResult>.setProps (target :?> ViewBackedTerminalElement, changedProps)

    member _.ClearProp(target, propertyId) =
      DialogPropHandler<'TResult>.clearProp (target :?> ViewBackedTerminalElement, propertyId)

    member _.ViewType = ViewType.``Dialog<'TResult>``

type RunnableWrapper<'TView, 'TResult when 'TView: (new: unit -> 'TView) and 'TView :> Terminal.Gui.ViewBase.View>
  (props: RunnableWrapperProps<'TView, 'TResult>) =
  let mutable viewTe: IViewTE voption = ValueNone

  let getOrCreateViewTE () =
    match viewTe with
    | ValueSome value -> value
    | ValueNone ->
      let value =
        new RunnableWrapperTerminalElement<'TView, 'TResult>(props.props) :> IViewTE

      viewTe <- ValueSome value
      value

  interface ISimpleViewSpec with
    member _.CreateViewTE() = getOrCreateViewTE ()
    member _.BindViewTE(value) = viewTe <- ValueSome value
    member _.Props = props.props

    member _.SetProps(target, changedProps) =
      RunnableWrapperPropHandler<'TView, 'TResult>.setProps (target :?> ViewBackedTerminalElement, changedProps)

    member _.ClearProp(target, propertyId) =
      RunnableWrapperPropHandler<'TView, 'TResult>.clearProp (target :?> ViewBackedTerminalElement, propertyId)

    member _.ViewType = ViewType.``RunnableWrapper<'TView, 'TResult>``

type Dialog(props: DialogProps) =
  let mutable viewTe: IViewTE voption = ValueNone

  let getOrCreateViewTE () =
    match viewTe with
    | ValueSome value -> value
    | ValueNone ->
      let value = new DialogTerminalElement(props.props) :> IViewTE
      viewTe <- ValueSome value
      value

  interface ISimpleViewSpec with
    member _.CreateViewTE() = getOrCreateViewTE ()
    member _.BindViewTE(value) = viewTe <- ValueSome value
    member _.Props = props.props

    member _.SetProps(target, changedProps) =
      DialogPropHandler.setProps (target :?> ViewBackedTerminalElement, changedProps)

    member _.ClearProp(target, propertyId) =
      DialogPropHandler.clearProp (target :?> ViewBackedTerminalElement, propertyId)

    member _.ViewType = ViewType.``ViewsDialog``

type FileDialog(props: FileDialogProps) =
  let mutable viewTe: IViewTE voption = ValueNone

  let getOrCreateViewTE () =
    match viewTe with
    | ValueSome value -> value
    | ValueNone ->
      let value = new FileDialogTerminalElement(props.props) :> IViewTE
      viewTe <- ValueSome value
      value

  interface ISimpleViewSpec with
    member _.CreateViewTE() = getOrCreateViewTE ()
    member _.BindViewTE(value) = viewTe <- ValueSome value
    member _.Props = props.props

    member _.SetProps(target, changedProps) =
      FileDialogPropHandler.setProps (target :?> ViewBackedTerminalElement, changedProps)

    member _.ClearProp(target, propertyId) =
      FileDialogPropHandler.clearProp (target :?> ViewBackedTerminalElement, propertyId)

    member _.ViewType = ViewType.``ViewsFileDialog``

type Prompt<'TView, 'TResult when 'TView: (new: unit -> 'TView) and 'TView :> Terminal.Gui.ViewBase.View>
  (props: PromptProps<'TView, 'TResult>) =
  let mutable viewTe: IViewTE voption = ValueNone

  let getOrCreateViewTE () =
    match viewTe with
    | ValueSome value -> value
    | ValueNone ->
      let value = new PromptTerminalElement<'TView, 'TResult>(props.props) :> IViewTE
      viewTe <- ValueSome value
      value

  interface ISimpleViewSpec with
    member _.CreateViewTE() = getOrCreateViewTE ()
    member _.BindViewTE(value) = viewTe <- ValueSome value
    member _.Props = props.props

    member _.SetProps(target, changedProps) =
      PromptPropHandler<'TView, 'TResult>.setProps (target :?> ViewBackedTerminalElement, changedProps)

    member _.ClearProp(target, propertyId) =
      PromptPropHandler<'TView, 'TResult>.clearProp (target :?> ViewBackedTerminalElement, propertyId)

    member _.ViewType = ViewType.``Prompt<'TView, 'TResult>``

type OpenDialog(props: OpenDialogProps) =
  let mutable viewTe: IViewTE voption = ValueNone

  let getOrCreateViewTE () =
    match viewTe with
    | ValueSome value -> value
    | ValueNone ->
      let value = new OpenDialogTerminalElement(props.props) :> IViewTE
      viewTe <- ValueSome value
      value

  interface ISimpleViewSpec with
    member _.CreateViewTE() = getOrCreateViewTE ()
    member _.BindViewTE(value) = viewTe <- ValueSome value
    member _.Props = props.props

    member _.SetProps(target, changedProps) =
      OpenDialogPropHandler.setProps (target :?> ViewBackedTerminalElement, changedProps)

    member _.ClearProp(target, propertyId) =
      OpenDialogPropHandler.clearProp (target :?> ViewBackedTerminalElement, propertyId)

    member _.ViewType = ViewType.``ViewsOpenDialog``

type SaveDialog(props: SaveDialogProps) =
  let mutable viewTe: IViewTE voption = ValueNone

  let getOrCreateViewTE () =
    match viewTe with
    | ValueSome value -> value
    | ValueNone ->
      let value = new SaveDialogTerminalElement(props.props) :> IViewTE
      viewTe <- ValueSome value
      value

  interface ISimpleViewSpec with
    member _.CreateViewTE() = getOrCreateViewTE ()
    member _.BindViewTE(value) = viewTe <- ValueSome value
    member _.Props = props.props

    member _.SetProps(target, changedProps) =
      SaveDialogPropHandler.setProps (target :?> ViewBackedTerminalElement, changedProps)

    member _.ClearProp(target, propertyId) =
      SaveDialogPropHandler.clearProp (target :?> ViewBackedTerminalElement, propertyId)

    member _.ViewType = ViewType.``ViewsSaveDialog``

type ScrollBar(props: ScrollBarProps) =
  let mutable viewTe: IViewTE voption = ValueNone

  let getOrCreateViewTE () =
    match viewTe with
    | ValueSome value -> value
    | ValueNone ->
      let value = new ScrollBarTerminalElement(props.props) :> IViewTE
      viewTe <- ValueSome value
      value

  interface ISimpleViewSpec with
    member _.CreateViewTE() = getOrCreateViewTE ()
    member _.BindViewTE(value) = viewTe <- ValueSome value
    member _.Props = props.props

    member _.SetProps(target, changedProps) =
      ScrollBarPropHandler.setProps (target :?> ViewBackedTerminalElement, changedProps)

    member _.ClearProp(target, propertyId) =
      ScrollBarPropHandler.clearProp (target :?> ViewBackedTerminalElement, propertyId)

    member _.ViewType = ViewType.``ViewsScrollBar``

type ScrollButton(props: ScrollButtonProps) =
  let mutable viewTe: IViewTE voption = ValueNone

  let getOrCreateViewTE () =
    match viewTe with
    | ValueSome value -> value
    | ValueNone ->
      let value = new ScrollButtonTerminalElement(props.props) :> IViewTE
      viewTe <- ValueSome value
      value

  interface ISimpleViewSpec with
    member _.CreateViewTE() = getOrCreateViewTE ()
    member _.BindViewTE(value) = viewTe <- ValueSome value
    member _.Props = props.props

    member _.SetProps(target, changedProps) =
      ScrollButtonPropHandler.setProps (target :?> ViewBackedTerminalElement, changedProps)

    member _.ClearProp(target, propertyId) =
      ScrollButtonPropHandler.clearProp (target :?> ViewBackedTerminalElement, propertyId)

    member _.ViewType = ViewType.``ViewsScrollButton``

type ScrollSlider(props: ScrollSliderProps) =
  let mutable viewTe: IViewTE voption = ValueNone

  let getOrCreateViewTE () =
    match viewTe with
    | ValueSome value -> value
    | ValueNone ->
      let value = new ScrollSliderTerminalElement(props.props) :> IViewTE
      viewTe <- ValueSome value
      value

  interface ISimpleViewSpec with
    member _.CreateViewTE() = getOrCreateViewTE ()
    member _.BindViewTE(value) = viewTe <- ValueSome value
    member _.Props = props.props

    member _.SetProps(target, changedProps) =
      ScrollSliderPropHandler.setProps (target :?> ViewBackedTerminalElement, changedProps)

    member _.ClearProp(target, propertyId) =
      ScrollSliderPropHandler.clearProp (target :?> ViewBackedTerminalElement, propertyId)

    member _.ViewType = ViewType.``ViewsScrollSlider``

type FlagSelector(props: FlagSelectorProps) =
  let mutable viewTe: IViewTE voption = ValueNone

  let getOrCreateViewTE () =
    match viewTe with
    | ValueSome value -> value
    | ValueNone ->
      let value = new FlagSelectorTerminalElement(props.props) :> IViewTE
      viewTe <- ValueSome value
      value

  interface ISimpleViewSpec with
    member _.CreateViewTE() = getOrCreateViewTE ()
    member _.BindViewTE(value) = viewTe <- ValueSome value
    member _.Props = props.props

    member _.SetProps(target, changedProps) =
      FlagSelectorPropHandler.setProps (target :?> ViewBackedTerminalElement, changedProps)

    member _.ClearProp(target, propertyId) =
      FlagSelectorPropHandler.clearProp (target :?> ViewBackedTerminalElement, propertyId)

    member _.ViewType = ViewType.``ViewsFlagSelector``

type OptionSelector(props: OptionSelectorProps) =
  let mutable viewTe: IViewTE voption = ValueNone

  let getOrCreateViewTE () =
    match viewTe with
    | ValueSome value -> value
    | ValueNone ->
      let value = new OptionSelectorTerminalElement(props.props) :> IViewTE
      viewTe <- ValueSome value
      value

  interface ISimpleViewSpec with
    member _.CreateViewTE() = getOrCreateViewTE ()
    member _.BindViewTE(value) = viewTe <- ValueSome value
    member _.Props = props.props

    member _.SetProps(target, changedProps) =
      OptionSelectorPropHandler.setProps (target :?> ViewBackedTerminalElement, changedProps)

    member _.ClearProp(target, propertyId) =
      OptionSelectorPropHandler.clearProp (target :?> ViewBackedTerminalElement, propertyId)

    member _.ViewType = ViewType.``ViewsOptionSelector``

type FlagSelector<'TFlagsEnum
  when 'TFlagsEnum: struct
  and 'TFlagsEnum: (new: unit -> 'TFlagsEnum)
  and 'TFlagsEnum :> System.Enum
  and 'TFlagsEnum :> System.ValueType>(props: FlagSelectorProps<'TFlagsEnum>) =
  let mutable viewTe: IViewTE voption = ValueNone

  let getOrCreateViewTE () =
    match viewTe with
    | ValueSome value -> value
    | ValueNone ->
      let value = new FlagSelectorTerminalElement<'TFlagsEnum>(props.props) :> IViewTE
      viewTe <- ValueSome value
      value

  interface ISimpleViewSpec with
    member _.CreateViewTE() = getOrCreateViewTE ()
    member _.BindViewTE(value) = viewTe <- ValueSome value
    member _.Props = props.props

    member _.SetProps(target, changedProps) =
      FlagSelectorPropHandler<'TFlagsEnum>.setProps (target :?> ViewBackedTerminalElement, changedProps)

    member _.ClearProp(target, propertyId) =
      FlagSelectorPropHandler<'TFlagsEnum>.clearProp (target :?> ViewBackedTerminalElement, propertyId)

    member _.ViewType = ViewType.``FlagSelector<'TFlagsEnum>``

type OptionSelector<'TEnum
  when 'TEnum: struct and 'TEnum: (new: unit -> 'TEnum) and 'TEnum :> System.Enum and 'TEnum :> System.ValueType>
  (props: OptionSelectorProps<'TEnum>) =
  let mutable viewTe: IViewTE voption = ValueNone

  let getOrCreateViewTE () =
    match viewTe with
    | ValueSome value -> value
    | ValueNone ->
      let value = new OptionSelectorTerminalElement<'TEnum>(props.props) :> IViewTE
      viewTe <- ValueSome value
      value

  interface ISimpleViewSpec with
    member _.CreateViewTE() = getOrCreateViewTE ()
    member _.BindViewTE(value) = viewTe <- ValueSome value
    member _.Props = props.props

    member _.SetProps(target, changedProps) =
      OptionSelectorPropHandler<'TEnum>.setProps (target :?> ViewBackedTerminalElement, changedProps)

    member _.ClearProp(target, propertyId) =
      OptionSelectorPropHandler<'TEnum>.clearProp (target :?> ViewBackedTerminalElement, propertyId)

    member _.ViewType = ViewType.``OptionSelector<'TEnum>``

type Shortcut(props: ShortcutProps) =
  let mutable viewTe: IViewTE voption = ValueNone

  let getOrCreateViewTE () =
    match viewTe with
    | ValueSome value -> value
    | ValueNone ->
      let value = new ShortcutTerminalElement(props.props) :> IViewTE
      viewTe <- ValueSome value
      value

  interface ISimpleViewSpec with
    member _.CreateViewTE() = getOrCreateViewTE ()
    member _.BindViewTE(value) = viewTe <- ValueSome value
    member _.Props = props.props

    member _.SetProps(target, changedProps) =
      ShortcutPropHandler.setProps (target :?> ViewBackedTerminalElement, changedProps)

    member _.ClearProp(target, propertyId) =
      ShortcutPropHandler.clearProp (target :?> ViewBackedTerminalElement, propertyId)

    member _.ViewType = ViewType.``ViewsShortcut``

type MenuItem(props: MenuItemProps) =
  let mutable viewTe: IViewTE voption = ValueNone

  let getOrCreateViewTE () =
    match viewTe with
    | ValueSome value -> value
    | ValueNone ->
      let value = new MenuItemTerminalElement(props.props) :> IViewTE
      viewTe <- ValueSome value
      value

  interface ISimpleViewSpec with
    member _.CreateViewTE() = getOrCreateViewTE ()
    member _.BindViewTE(value) = viewTe <- ValueSome value
    member _.Props = props.props

    member _.SetProps(target, changedProps) =
      MenuItemPropHandler.setProps (target :?> ViewBackedTerminalElement, changedProps)

    member _.ClearProp(target, propertyId) =
      MenuItemPropHandler.clearProp (target :?> ViewBackedTerminalElement, propertyId)

    member _.ViewType = ViewType.``ViewsMenuItem``

  interface IMenuItemView

type MenuBarItem(props: MenuBarItemProps) =
  let mutable viewTe: IViewTE voption = ValueNone

  let getOrCreateViewTE () =
    match viewTe with
    | ValueSome value -> value
    | ValueNone ->
      let value = new MenuBarItemTerminalElement(props.props) :> IViewTE
      viewTe <- ValueSome value
      value

  interface ISimpleViewSpec with
    member _.CreateViewTE() = getOrCreateViewTE ()
    member _.BindViewTE(value) = viewTe <- ValueSome value
    member _.Props = props.props

    member _.SetProps(target, changedProps) =
      MenuBarItemPropHandler.setProps (target :?> ViewBackedTerminalElement, changedProps)

    member _.ClearProp(target, propertyId) =
      MenuBarItemPropHandler.clearProp (target :?> ViewBackedTerminalElement, propertyId)

    member _.ViewType = ViewType.``ViewsMenuBarItem``

  interface IMenuItemView

type SpinnerView(props: SpinnerViewProps) =
  let mutable viewTe: IViewTE voption = ValueNone

  let getOrCreateViewTE () =
    match viewTe with
    | ValueSome value -> value
    | ValueNone ->
      let value = new SpinnerViewTerminalElement(props.props) :> IViewTE
      viewTe <- ValueSome value
      value

  interface ISimpleViewSpec with
    member _.CreateViewTE() = getOrCreateViewTE ()
    member _.BindViewTE(value) = viewTe <- ValueSome value
    member _.Props = props.props

    member _.SetProps(target, changedProps) =
      SpinnerViewPropHandler.setProps (target :?> ViewBackedTerminalElement, changedProps)

    member _.ClearProp(target, propertyId) =
      SpinnerViewPropHandler.clearProp (target :?> ViewBackedTerminalElement, propertyId)

    member _.ViewType = ViewType.``ViewsSpinnerView``

type StatusBar(props: StatusBarProps) =
  let mutable viewTe: IViewTE voption = ValueNone

  let getOrCreateViewTE () =
    match viewTe with
    | ValueSome value -> value
    | ValueNone ->
      let value = new StatusBarTerminalElement(props.props) :> IViewTE
      viewTe <- ValueSome value
      value

  interface ISimpleViewSpec with
    member _.CreateViewTE() = getOrCreateViewTE ()
    member _.BindViewTE(value) = viewTe <- ValueSome value
    member _.Props = props.props

    member _.SetProps(target, changedProps) =
      StatusBarPropHandler.setProps (target :?> ViewBackedTerminalElement, changedProps)

    member _.ClearProp(target, propertyId) =
      StatusBarPropHandler.clearProp (target :?> ViewBackedTerminalElement, propertyId)

    member _.ViewType = ViewType.``ViewsStatusBar``

type TableView(props: TableViewProps) =
  let mutable viewTe: IViewTE voption = ValueNone

  let getOrCreateViewTE () =
    match viewTe with
    | ValueSome value -> value
    | ValueNone ->
      let value = new TableViewTerminalElement(props.props) :> IViewTE
      viewTe <- ValueSome value
      value

  interface ISimpleViewSpec with
    member _.CreateViewTE() = getOrCreateViewTE ()
    member _.BindViewTE(value) = viewTe <- ValueSome value
    member _.Props = props.props

    member _.SetProps(target, changedProps) =
      TableViewPropHandler.setProps (target :?> ViewBackedTerminalElement, changedProps)

    member _.ClearProp(target, propertyId) =
      TableViewPropHandler.clearProp (target :?> ViewBackedTerminalElement, propertyId)

    member _.ViewType = ViewType.``ViewsTableView``

type Tabs(props: TabsProps) =
  let mutable viewTe: IViewTE voption = ValueNone

  let getOrCreateViewTE () =
    match viewTe with
    | ValueSome value -> value
    | ValueNone ->
      let value = new TabsTerminalElement(props.props) :> IViewTE
      viewTe <- ValueSome value
      value

  interface ISimpleViewSpec with
    member _.CreateViewTE() = getOrCreateViewTE ()
    member _.BindViewTE(value) = viewTe <- ValueSome value
    member _.Props = props.props

    member _.SetProps(target, changedProps) =
      TabsPropHandler.setProps (target :?> ViewBackedTerminalElement, changedProps)

    member _.ClearProp(target, propertyId) =
      TabsPropHandler.clearProp (target :?> ViewBackedTerminalElement, propertyId)

    member _.ViewType = ViewType.``ViewsTabs``

type TextField(props: TextFieldProps) =
  let mutable viewTe: IViewTE voption = ValueNone

  let getOrCreateViewTE () =
    match viewTe with
    | ValueSome value -> value
    | ValueNone ->
      let value = new TextFieldTerminalElement(props.props) :> IViewTE
      viewTe <- ValueSome value
      value

  interface ISimpleViewSpec with
    member _.CreateViewTE() = getOrCreateViewTE ()
    member _.BindViewTE(value) = viewTe <- ValueSome value
    member _.Props = props.props

    member _.SetProps(target, changedProps) =
      TextFieldPropHandler.setProps (target :?> ViewBackedTerminalElement, changedProps)

    member _.ClearProp(target, propertyId) =
      TextFieldPropHandler.clearProp (target :?> ViewBackedTerminalElement, propertyId)

    member _.ViewType = ViewType.``ViewsTextField``

type DropDownList(props: DropDownListProps) =
  let mutable viewTe: IViewTE voption = ValueNone

  let getOrCreateViewTE () =
    match viewTe with
    | ValueSome value -> value
    | ValueNone ->
      let value = new DropDownListTerminalElement(props.props) :> IViewTE
      viewTe <- ValueSome value
      value

  interface ISimpleViewSpec with
    member _.CreateViewTE() = getOrCreateViewTE ()
    member _.BindViewTE(value) = viewTe <- ValueSome value
    member _.Props = props.props

    member _.SetProps(target, changedProps) =
      DropDownListPropHandler.setProps (target :?> ViewBackedTerminalElement, changedProps)

    member _.ClearProp(target, propertyId) =
      DropDownListPropHandler.clearProp (target :?> ViewBackedTerminalElement, propertyId)

    member _.ViewType = ViewType.``ViewsDropDownList``

type DropDownList<'TEnum
  when 'TEnum: struct and 'TEnum: (new: unit -> 'TEnum) and 'TEnum :> System.Enum and 'TEnum :> System.ValueType>
  (props: DropDownListProps<'TEnum>) =
  let mutable viewTe: IViewTE voption = ValueNone

  let getOrCreateViewTE () =
    match viewTe with
    | ValueSome value -> value
    | ValueNone ->
      let value = new DropDownListTerminalElement<'TEnum>(props.props) :> IViewTE
      viewTe <- ValueSome value
      value

  interface ISimpleViewSpec with
    member _.CreateViewTE() = getOrCreateViewTE ()
    member _.BindViewTE(value) = viewTe <- ValueSome value
    member _.Props = props.props

    member _.SetProps(target, changedProps) =
      DropDownListPropHandler<'TEnum>.setProps (target :?> ViewBackedTerminalElement, changedProps)

    member _.ClearProp(target, propertyId) =
      DropDownListPropHandler<'TEnum>.clearProp (target :?> ViewBackedTerminalElement, propertyId)

    member _.ViewType = ViewType.``DropDownList<'TEnum>``

type TextValidateField(props: TextValidateFieldProps) =
  let mutable viewTe: IViewTE voption = ValueNone

  let getOrCreateViewTE () =
    match viewTe with
    | ValueSome value -> value
    | ValueNone ->
      let value = new TextValidateFieldTerminalElement(props.props) :> IViewTE
      viewTe <- ValueSome value
      value

  interface ISimpleViewSpec with
    member _.CreateViewTE() = getOrCreateViewTE ()
    member _.BindViewTE(value) = viewTe <- ValueSome value
    member _.Props = props.props

    member _.SetProps(target, changedProps) =
      TextValidateFieldPropHandler.setProps (target :?> ViewBackedTerminalElement, changedProps)

    member _.ClearProp(target, propertyId) =
      TextValidateFieldPropHandler.clearProp (target :?> ViewBackedTerminalElement, propertyId)

    member _.ViewType = ViewType.``ViewsTextValidateField``

type DateEditor(props: DateEditorProps) =
  let mutable viewTe: IViewTE voption = ValueNone

  let getOrCreateViewTE () =
    match viewTe with
    | ValueSome value -> value
    | ValueNone ->
      let value = new DateEditorTerminalElement(props.props) :> IViewTE
      viewTe <- ValueSome value
      value

  interface ISimpleViewSpec with
    member _.CreateViewTE() = getOrCreateViewTE ()
    member _.BindViewTE(value) = viewTe <- ValueSome value
    member _.Props = props.props

    member _.SetProps(target, changedProps) =
      DateEditorPropHandler.setProps (target :?> ViewBackedTerminalElement, changedProps)

    member _.ClearProp(target, propertyId) =
      DateEditorPropHandler.clearProp (target :?> ViewBackedTerminalElement, propertyId)

    member _.ViewType = ViewType.``ViewsDateEditor``

type TextView(props: TextViewProps) =
  let mutable viewTe: IViewTE voption = ValueNone

  let getOrCreateViewTE () =
    match viewTe with
    | ValueSome value -> value
    | ValueNone ->
      let value = new TextViewTerminalElement(props.props) :> IViewTE
      viewTe <- ValueSome value
      value

  interface ISimpleViewSpec with
    member _.CreateViewTE() = getOrCreateViewTE ()
    member _.BindViewTE(value) = viewTe <- ValueSome value
    member _.Props = props.props

    member _.SetProps(target, changedProps) =
      TextViewPropHandler.setProps (target :?> ViewBackedTerminalElement, changedProps)

    member _.ClearProp(target, propertyId) =
      TextViewPropHandler.clearProp (target :?> ViewBackedTerminalElement, propertyId)

    member _.ViewType = ViewType.``ViewsTextView``

type TimeEditor(props: TimeEditorProps) =
  let mutable viewTe: IViewTE voption = ValueNone

  let getOrCreateViewTE () =
    match viewTe with
    | ValueSome value -> value
    | ValueNone ->
      let value = new TimeEditorTerminalElement(props.props) :> IViewTE
      viewTe <- ValueSome value
      value

  interface ISimpleViewSpec with
    member _.CreateViewTE() = getOrCreateViewTE ()
    member _.BindViewTE(value) = viewTe <- ValueSome value
    member _.Props = props.props

    member _.SetProps(target, changedProps) =
      TimeEditorPropHandler.setProps (target :?> ViewBackedTerminalElement, changedProps)

    member _.ClearProp(target, propertyId) =
      TimeEditorPropHandler.clearProp (target :?> ViewBackedTerminalElement, propertyId)

    member _.ViewType = ViewType.``ViewsTimeEditor``

type TitleView(props: TitleViewProps) =
  let mutable viewTe: IViewTE voption = ValueNone

  let getOrCreateViewTE () =
    match viewTe with
    | ValueSome value -> value
    | ValueNone ->
      let value = new TitleViewTerminalElement(props.props) :> IViewTE
      viewTe <- ValueSome value
      value

  interface ISimpleViewSpec with
    member _.CreateViewTE() = getOrCreateViewTE ()
    member _.BindViewTE(value) = viewTe <- ValueSome value
    member _.Props = props.props

    member _.SetProps(target, changedProps) =
      TitleViewPropHandler.setProps (target :?> ViewBackedTerminalElement, changedProps)

    member _.ClearProp(target, propertyId) =
      TitleViewPropHandler.clearProp (target :?> ViewBackedTerminalElement, propertyId)

    member _.ViewType = ViewType.``ViewBaseTitleView``

type ToolTipHost<'TView when 'TView: (new: unit -> 'TView) and 'TView :> Terminal.Gui.ViewBase.View>
  (props: ToolTipHostProps<'TView>) =
  let mutable viewTe: IViewTE voption = ValueNone

  let getOrCreateViewTE () =
    match viewTe with
    | ValueSome value -> value
    | ValueNone ->
      let value = new ToolTipHostTerminalElement<'TView>(props.props) :> IViewTE
      viewTe <- ValueSome value
      value

  interface ISimpleViewSpec with
    member _.CreateViewTE() = getOrCreateViewTE ()
    member _.BindViewTE(value) = viewTe <- ValueSome value
    member _.Props = props.props

    member _.SetProps(target, changedProps) =
      ToolTipHostPropHandler<'TView>.setProps (target :?> ViewBackedTerminalElement, changedProps)

    member _.ClearProp(target, propertyId) =
      ToolTipHostPropHandler<'TView>.clearProp (target :?> ViewBackedTerminalElement, propertyId)

    member _.ViewType = ViewType.``ToolTipHost<'TView>``

type TreeView<'T when 'T: not struct>(props: TreeViewProps<'T>) =
  let mutable viewTe: IViewTE voption = ValueNone

  let getOrCreateViewTE () =
    match viewTe with
    | ValueSome value -> value
    | ValueNone ->
      let value = new TreeViewTerminalElement<'T>(props.props) :> IViewTE
      viewTe <- ValueSome value
      value

  interface ISimpleViewSpec with
    member _.CreateViewTE() = getOrCreateViewTE ()
    member _.BindViewTE(value) = viewTe <- ValueSome value
    member _.Props = props.props

    member _.SetProps(target, changedProps) =
      TreeViewPropHandler<'T>.setProps (target :?> ViewBackedTerminalElement, changedProps)

    member _.ClearProp(target, propertyId) =
      TreeViewPropHandler<'T>.clearProp (target :?> ViewBackedTerminalElement, propertyId)

    member _.ViewType = ViewType.``TreeView<'T>``

type TreeView(props: TreeViewProps) =
  let mutable viewTe: IViewTE voption = ValueNone

  let getOrCreateViewTE () =
    match viewTe with
    | ValueSome value -> value
    | ValueNone ->
      let value = new TreeViewTerminalElement(props.props) :> IViewTE
      viewTe <- ValueSome value
      value

  interface ISimpleViewSpec with
    member _.CreateViewTE() = getOrCreateViewTE ()
    member _.BindViewTE(value) = viewTe <- ValueSome value
    member _.Props = props.props

    member _.SetProps(target, changedProps) =
      TreeViewPropHandler.setProps (target :?> ViewBackedTerminalElement, changedProps)

    member _.ClearProp(target, propertyId) =
      TreeViewPropHandler.clearProp (target :?> ViewBackedTerminalElement, propertyId)

    member _.ViewType = ViewType.``ViewsTreeView``

type Window(props: WindowProps) =
  let mutable viewTe: IViewTE voption = ValueNone

  let getOrCreateViewTE () =
    match viewTe with
    | ValueSome value -> value
    | ValueNone ->
      let value = new WindowTerminalElement(props.props) :> IViewTE
      viewTe <- ValueSome value
      value

  interface ISimpleViewSpec with
    member _.CreateViewTE() = getOrCreateViewTE ()
    member _.BindViewTE(value) = viewTe <- ValueSome value
    member _.Props = props.props

    member _.SetProps(target, changedProps) =
      WindowPropHandler.setProps (target :?> ViewBackedTerminalElement, changedProps)

    member _.ClearProp(target, propertyId) =
      WindowPropHandler.clearProp (target :?> ViewBackedTerminalElement, propertyId)

    member _.ViewType = ViewType.``ViewsWindow``

type Wizard(props: WizardProps) =
  let mutable viewTe: IViewTE voption = ValueNone

  let getOrCreateViewTE () =
    match viewTe with
    | ValueSome value -> value
    | ValueNone ->
      let value = new WizardTerminalElement(props.props) :> IViewTE
      viewTe <- ValueSome value
      value

  interface ISimpleViewSpec with
    member _.CreateViewTE() = getOrCreateViewTE ()
    member _.BindViewTE(value) = viewTe <- ValueSome value
    member _.Props = props.props

    member _.SetProps(target, changedProps) =
      WizardPropHandler.setProps (target :?> ViewBackedTerminalElement, changedProps)

    member _.ClearProp(target, propertyId) =
      WizardPropHandler.clearProp (target :?> ViewBackedTerminalElement, propertyId)

    member _.ViewType = ViewType.``ViewsWizard``

type WizardStep(props: WizardStepProps) =
  let mutable viewTe: IViewTE voption = ValueNone

  let getOrCreateViewTE () =
    match viewTe with
    | ValueSome value -> value
    | ValueNone ->
      let value = new WizardStepTerminalElement(props.props) :> IViewTE
      viewTe <- ValueSome value
      value

  interface ISimpleViewSpec with
    member _.CreateViewTE() = getOrCreateViewTE ()
    member _.BindViewTE(value) = viewTe <- ValueSome value
    member _.Props = props.props

    member _.SetProps(target, changedProps) =
      WizardStepPropHandler.setProps (target :?> ViewBackedTerminalElement, changedProps)

    member _.ClearProp(target, propertyId) =
      WizardStepPropHandler.clearProp (target :?> ViewBackedTerminalElement, propertyId)

    member _.ViewType = ViewType.``ViewsWizardStep``

  interface IWizardStepView
