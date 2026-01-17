(*
#######################################
#            Views.gen.fs             #
#######################################
*)


namespace Terminal.Gui.Elmish

open System
open Terminal.Gui.Elmish
open Terminal.Gui.ViewBase
open Terminal.Gui.Views

type View with

  /// <seealso cref="Terminal.Gui.Adornment"/>
  static member adornment(set: adornmentProps -> unit) =
    let viewProps = adornmentProps ()
    set viewProps
    new AdornmentElement(viewProps.props) :> ITerminalElement

  static member adornment(children: ITerminalElement list) =
    let viewProps = adornmentProps ()
    viewProps.children children
    new AdornmentElement(viewProps.props) :> ITerminalElement

  static member adornment(x: int, y: int, title: string) =
    let setProps =
      fun (p: adornmentProps) ->
        p.x (Pos.Absolute(x))
        p.y (Pos.Absolute(y))
        p.title title

    let viewProps = adornmentProps ()
    setProps viewProps
    new AdornmentElement(viewProps.props) :> ITerminalElement

  /// <seealso cref="Terminal.Gui.Bar"/>
  static member bar(set: barProps -> unit) =
    let viewProps = barProps ()
    set viewProps
    new BarElement(viewProps.props) :> ITerminalElement

  static member bar(children: ITerminalElement list) =
    let viewProps = barProps ()
    viewProps.children children
    new BarElement(viewProps.props) :> ITerminalElement

  static member bar(x: int, y: int, title: string) =
    let setProps =
      fun (p: barProps) ->
        p.x (Pos.Absolute(x))
        p.y (Pos.Absolute(y))
        p.title title

    let viewProps = barProps ()
    setProps viewProps
    new BarElement(viewProps.props) :> ITerminalElement

  /// <seealso cref="Terminal.Gui.Border"/>
  static member border(set: borderProps -> unit) =
    let viewProps = borderProps ()
    set viewProps
    new BorderElement(viewProps.props) :> ITerminalElement

  static member border(children: ITerminalElement list) =
    let viewProps = borderProps ()
    viewProps.children children
    new BorderElement(viewProps.props) :> ITerminalElement

  static member border(x: int, y: int, title: string) =
    let setProps =
      fun (p: borderProps) ->
        p.x (Pos.Absolute(x))
        p.y (Pos.Absolute(y))
        p.title title

    let viewProps = borderProps ()
    setProps viewProps
    new BorderElement(viewProps.props) :> ITerminalElement

  /// <seealso cref="Terminal.Gui.Button"/>
  static member button(set: buttonProps -> unit) =
    let viewProps = buttonProps ()
    set viewProps
    new ButtonElement(viewProps.props) :> ITerminalElement

  static member button(children: ITerminalElement list) =
    let viewProps = buttonProps ()
    viewProps.children children
    new ButtonElement(viewProps.props) :> ITerminalElement

  static member button(x: int, y: int, title: string) =
    let setProps =
      fun (p: buttonProps) ->
        p.x (Pos.Absolute(x))
        p.y (Pos.Absolute(y))
        p.title title

    let viewProps = buttonProps ()
    setProps viewProps
    new ButtonElement(viewProps.props) :> ITerminalElement

  /// <seealso cref="Terminal.Gui.CharMap"/>
  static member charMap(set: charMapProps -> unit) =
    let viewProps = charMapProps ()
    set viewProps
    new CharMapElement(viewProps.props) :> ITerminalElement

  static member charMap(children: ITerminalElement list) =
    let viewProps = charMapProps ()
    viewProps.children children
    new CharMapElement(viewProps.props) :> ITerminalElement

  static member charMap(x: int, y: int, title: string) =
    let setProps =
      fun (p: charMapProps) ->
        p.x (Pos.Absolute(x))
        p.y (Pos.Absolute(y))
        p.title title

    let viewProps = charMapProps ()
    setProps viewProps
    new CharMapElement(viewProps.props) :> ITerminalElement

  /// <seealso cref="Terminal.Gui.CheckBox"/>
  static member checkBox(set: checkBoxProps -> unit) =
    let viewProps = checkBoxProps ()
    set viewProps
    new CheckBoxElement(viewProps.props) :> ITerminalElement

  static member checkBox(children: ITerminalElement list) =
    let viewProps = checkBoxProps ()
    viewProps.children children
    new CheckBoxElement(viewProps.props) :> ITerminalElement

  static member checkBox(x: int, y: int, title: string) =
    let setProps =
      fun (p: checkBoxProps) ->
        p.x (Pos.Absolute(x))
        p.y (Pos.Absolute(y))
        p.title title

    let viewProps = checkBoxProps ()
    setProps viewProps
    new CheckBoxElement(viewProps.props) :> ITerminalElement

  /// <seealso cref="Terminal.Gui.ColorPicker"/>
  static member colorPicker(set: colorPickerProps -> unit) =
    let viewProps = colorPickerProps ()
    set viewProps
    new ColorPickerElement(viewProps.props) :> ITerminalElement

  static member colorPicker(children: ITerminalElement list) =
    let viewProps = colorPickerProps ()
    viewProps.children children
    new ColorPickerElement(viewProps.props) :> ITerminalElement

  static member colorPicker(x: int, y: int, title: string) =
    let setProps =
      fun (p: colorPickerProps) ->
        p.x (Pos.Absolute(x))
        p.y (Pos.Absolute(y))
        p.title title

    let viewProps = colorPickerProps ()
    setProps viewProps
    new ColorPickerElement(viewProps.props) :> ITerminalElement

  /// <seealso cref="Terminal.Gui.ColorPicker16"/>
  static member colorPicker16(set: colorPicker16Props -> unit) =
    let viewProps = colorPicker16Props ()
    set viewProps
    new ColorPicker16Element(viewProps.props) :> ITerminalElement

  static member colorPicker16(children: ITerminalElement list) =
    let viewProps = colorPicker16Props ()
    viewProps.children children
    new ColorPicker16Element(viewProps.props) :> ITerminalElement

  static member colorPicker16(x: int, y: int, title: string) =
    let setProps =
      fun (p: colorPicker16Props) ->
        p.x (Pos.Absolute(x))
        p.y (Pos.Absolute(y))
        p.title title

    let viewProps = colorPicker16Props ()
    setProps viewProps
    new ColorPicker16Element(viewProps.props) :> ITerminalElement

  /// <seealso cref="Terminal.Gui.ComboBox"/>
  static member comboBox(set: comboBoxProps -> unit) =
    let viewProps = comboBoxProps ()
    set viewProps
    new ComboBoxElement(viewProps.props) :> ITerminalElement

  static member comboBox(children: ITerminalElement list) =
    let viewProps = comboBoxProps ()
    viewProps.children children
    new ComboBoxElement(viewProps.props) :> ITerminalElement

  static member comboBox(x: int, y: int, title: string) =
    let setProps =
      fun (p: comboBoxProps) ->
        p.x (Pos.Absolute(x))
        p.y (Pos.Absolute(y))
        p.title title

    let viewProps = comboBoxProps ()
    setProps viewProps
    new ComboBoxElement(viewProps.props) :> ITerminalElement

  /// <seealso cref="Terminal.Gui.DatePicker"/>
  static member datePicker(set: datePickerProps -> unit) =
    let viewProps = datePickerProps ()
    set viewProps
    new DatePickerElement(viewProps.props) :> ITerminalElement

  static member datePicker(children: ITerminalElement list) =
    let viewProps = datePickerProps ()
    viewProps.children children
    new DatePickerElement(viewProps.props) :> ITerminalElement

  static member datePicker(x: int, y: int, title: string) =
    let setProps =
      fun (p: datePickerProps) ->
        p.x (Pos.Absolute(x))
        p.y (Pos.Absolute(y))
        p.title title

    let viewProps = datePickerProps ()
    setProps viewProps
    new DatePickerElement(viewProps.props) :> ITerminalElement

  /// <seealso cref="Terminal.Gui.FrameView"/>
  static member frameView(set: frameViewProps -> unit) =
    let viewProps = frameViewProps ()
    set viewProps
    new FrameViewElement(viewProps.props) :> ITerminalElement

  static member frameView(children: ITerminalElement list) =
    let viewProps = frameViewProps ()
    viewProps.children children
    new FrameViewElement(viewProps.props) :> ITerminalElement

  static member frameView(x: int, y: int, title: string) =
    let setProps =
      fun (p: frameViewProps) ->
        p.x (Pos.Absolute(x))
        p.y (Pos.Absolute(y))
        p.title title

    let viewProps = frameViewProps ()
    setProps viewProps
    new FrameViewElement(viewProps.props) :> ITerminalElement

  /// <seealso cref="Terminal.Gui.GraphView"/>
  static member graphView(set: graphViewProps -> unit) =
    let viewProps = graphViewProps ()
    set viewProps
    new GraphViewElement(viewProps.props) :> ITerminalElement

  static member graphView(children: ITerminalElement list) =
    let viewProps = graphViewProps ()
    viewProps.children children
    new GraphViewElement(viewProps.props) :> ITerminalElement

  static member graphView(x: int, y: int, title: string) =
    let setProps =
      fun (p: graphViewProps) ->
        p.x (Pos.Absolute(x))
        p.y (Pos.Absolute(y))
        p.title title

    let viewProps = graphViewProps ()
    setProps viewProps
    new GraphViewElement(viewProps.props) :> ITerminalElement

  /// <seealso cref="Terminal.Gui.HexView"/>
  static member hexView(set: hexViewProps -> unit) =
    let viewProps = hexViewProps ()
    set viewProps
    new HexViewElement(viewProps.props) :> ITerminalElement

  static member hexView(children: ITerminalElement list) =
    let viewProps = hexViewProps ()
    viewProps.children children
    new HexViewElement(viewProps.props) :> ITerminalElement

  static member hexView(x: int, y: int, title: string) =
    let setProps =
      fun (p: hexViewProps) ->
        p.x (Pos.Absolute(x))
        p.y (Pos.Absolute(y))
        p.title title

    let viewProps = hexViewProps ()
    setProps viewProps
    new HexViewElement(viewProps.props) :> ITerminalElement

  /// <seealso cref="Terminal.Gui.Label"/>
  static member label(set: labelProps -> unit) =
    let viewProps = labelProps ()
    set viewProps
    new LabelElement(viewProps.props) :> ITerminalElement

  static member label(children: ITerminalElement list) =
    let viewProps = labelProps ()
    viewProps.children children
    new LabelElement(viewProps.props) :> ITerminalElement

  static member label(x: int, y: int, title: string) =
    let setProps =
      fun (p: labelProps) ->
        p.x (Pos.Absolute(x))
        p.y (Pos.Absolute(y))
        p.title title

    let viewProps = labelProps ()
    setProps viewProps
    new LabelElement(viewProps.props) :> ITerminalElement

  /// <seealso cref="Terminal.Gui.LegendAnnotation"/>
  static member legendAnnotation(set: legendAnnotationProps -> unit) =
    let viewProps = legendAnnotationProps ()
    set viewProps
    new LegendAnnotationElement(viewProps.props) :> ITerminalElement

  static member legendAnnotation(children: ITerminalElement list) =
    let viewProps = legendAnnotationProps ()
    viewProps.children children
    new LegendAnnotationElement(viewProps.props) :> ITerminalElement

  static member legendAnnotation(x: int, y: int, title: string) =
    let setProps =
      fun (p: legendAnnotationProps) ->
        p.x (Pos.Absolute(x))
        p.y (Pos.Absolute(y))
        p.title title

    let viewProps = legendAnnotationProps ()
    setProps viewProps
    new LegendAnnotationElement(viewProps.props) :> ITerminalElement

  /// <seealso cref="Terminal.Gui.Line"/>
  static member line(set: lineProps -> unit) =
    let viewProps = lineProps ()
    set viewProps
    new LineElement(viewProps.props) :> ITerminalElement

  static member line(children: ITerminalElement list) =
    let viewProps = lineProps ()
    viewProps.children children
    new LineElement(viewProps.props) :> ITerminalElement

  static member line(x: int, y: int, title: string) =
    let setProps =
      fun (p: lineProps) ->
        p.x (Pos.Absolute(x))
        p.y (Pos.Absolute(y))
        p.title title

    let viewProps = lineProps ()
    setProps viewProps
    new LineElement(viewProps.props) :> ITerminalElement

  /// <seealso cref="Terminal.Gui.ListView"/>
  static member listView(set: listViewProps -> unit) =
    let viewProps = listViewProps ()
    set viewProps
    new ListViewElement(viewProps.props) :> ITerminalElement

  static member listView(children: ITerminalElement list) =
    let viewProps = listViewProps ()
    viewProps.children children
    new ListViewElement(viewProps.props) :> ITerminalElement

  static member listView(x: int, y: int, title: string) =
    let setProps =
      fun (p: listViewProps) ->
        p.x (Pos.Absolute(x))
        p.y (Pos.Absolute(y))
        p.title title

    let viewProps = listViewProps ()
    setProps viewProps
    new ListViewElement(viewProps.props) :> ITerminalElement

  /// <seealso cref="Terminal.Gui.Margin"/>
  static member margin(set: marginProps -> unit) =
    let viewProps = marginProps ()
    set viewProps
    new MarginElement(viewProps.props) :> ITerminalElement

  static member margin(children: ITerminalElement list) =
    let viewProps = marginProps ()
    viewProps.children children
    new MarginElement(viewProps.props) :> ITerminalElement

  static member margin(x: int, y: int, title: string) =
    let setProps =
      fun (p: marginProps) ->
        p.x (Pos.Absolute(x))
        p.y (Pos.Absolute(y))
        p.title title

    let viewProps = marginProps ()
    setProps viewProps
    new MarginElement(viewProps.props) :> ITerminalElement

  /// <seealso cref="Terminal.Gui.MenuBar"/>
  static member menuBar(set: menuBarProps -> menuBarMacros -> unit) =
    let props = menuBarProps ()
    let macros = menuBarMacros props
    set props macros
    new MenuBarElement(props.props) :> ITerminalElement

  static member menuBar(children: ITerminalElement list) =
    let viewProps = menuBarProps ()
    viewProps.children children
    new MenuBarElement(viewProps.props) :> ITerminalElement

  static member menuBar(x: int, y: int, title: string) =
    let setProps =
      fun (p: menuBarProps) ->
        p.x (Pos.Absolute(x))
        p.y (Pos.Absolute(y))
        p.title title

    let viewProps = menuBarProps ()
    setProps viewProps
    new MenuBarElement(viewProps.props) :> ITerminalElement

  /// <seealso cref="Terminal.Gui.NumericUpDown"/>
  static member numericUpDown<'T>(set: numericUpDownProps<'T> -> unit) =
    let viewProps = numericUpDownProps<'T> ()

    set viewProps
    new NumericUpDownElement<'T>(viewProps.props)
    : INumericUpDownElement

  static member numericUpDown<'T>(children: ITerminalElement list) =
    let viewProps = numericUpDownProps<'T> ()

    viewProps.children children
    new NumericUpDownElement<'T>(viewProps.props)
    : INumericUpDownElement

  static member numericUpDown<'T>(x: int, y: int, title: string) =
    let setProps =
      fun (p: numericUpDownProps<'T>) ->
        p.x (Pos.Absolute(x))
        p.y (Pos.Absolute(y))
        p.title title

    let viewProps = numericUpDownProps<'T> ()

    setProps viewProps
    new NumericUpDownElement<'T>(viewProps.props)
    : INumericUpDownElement

  /// <seealso cref="Terminal.Gui.Padding"/>
  static member padding(set: paddingProps -> unit) =
    let viewProps = paddingProps ()
    set viewProps
    new PaddingElement(viewProps.props) :> ITerminalElement

  static member padding(children: ITerminalElement list) =
    let viewProps = paddingProps ()
    viewProps.children children
    new PaddingElement(viewProps.props) :> ITerminalElement

  static member padding(x: int, y: int, title: string) =
    let setProps =
      fun (p: paddingProps) ->
        p.x (Pos.Absolute(x))
        p.y (Pos.Absolute(y))
        p.title title

    let viewProps = paddingProps ()
    setProps viewProps
    new PaddingElement(viewProps.props) :> ITerminalElement

  /// <seealso cref="Terminal.Gui.PopoverBaseImpl"/>
  static member popoverBaseImpl(set: popoverBaseImplProps -> unit) =
    let viewProps = popoverBaseImplProps ()
    set viewProps
    new PopoverBaseImplElement(viewProps.props) :> ITerminalElement

  static member popoverBaseImpl(children: ITerminalElement list) =
    let viewProps = popoverBaseImplProps ()
    viewProps.children children
    new PopoverBaseImplElement(viewProps.props) :> ITerminalElement

  static member popoverBaseImpl(x: int, y: int, title: string) =
    let setProps =
      fun (p: popoverBaseImplProps) ->
        p.x (Pos.Absolute(x))
        p.y (Pos.Absolute(y))
        p.title title

    let viewProps = popoverBaseImplProps ()
    setProps viewProps
    new PopoverBaseImplElement(viewProps.props) :> ITerminalElement

  /// <seealso cref="Terminal.Gui.ProgressBar"/>
  static member progressBar(set: progressBarProps -> unit) =
    let viewProps = progressBarProps ()
    set viewProps
    new ProgressBarElement(viewProps.props) :> ITerminalElement

  static member progressBar(children: ITerminalElement list) =
    let viewProps = progressBarProps ()
    viewProps.children children
    new ProgressBarElement(viewProps.props) :> ITerminalElement

  static member progressBar(x: int, y: int, title: string) =
    let setProps =
      fun (p: progressBarProps) ->
        p.x (Pos.Absolute(x))
        p.y (Pos.Absolute(y))
        p.title title

    let viewProps = progressBarProps ()
    setProps viewProps
    new ProgressBarElement(viewProps.props) :> ITerminalElement

  /// <seealso cref="Terminal.Gui.Runnable"/>
  static member runnable(set: runnableProps -> unit) =
    let viewProps = runnableProps ()
    set viewProps
    new RunnableElement(viewProps.props) :> ITerminalElement

  static member runnable(children: ITerminalElement list) =
    let viewProps = runnableProps ()
    viewProps.children children
    new RunnableElement(viewProps.props) :> ITerminalElement

  static member runnable(x: int, y: int, title: string) =
    let setProps =
      fun (p: runnableProps) ->
        p.x (Pos.Absolute(x))
        p.y (Pos.Absolute(y))
        p.title title

    let viewProps = runnableProps ()
    setProps viewProps
    new RunnableElement(viewProps.props) :> ITerminalElement

  /// <seealso cref="Terminal.Gui.Runnable"/>
  static member runnable<'TResult>(set: runnableProps<'TResult> -> unit) =
    let viewProps = runnableProps<'TResult> ()

    set viewProps
    new RunnableElement<'TResult>(viewProps.props)
     :> ITerminalElement

  static member runnable<'TResult>(children: ITerminalElement list) =
    let viewProps = runnableProps<'TResult> ()

    viewProps.children children
    new RunnableElement<'TResult>(viewProps.props)
     :> ITerminalElement

  static member runnable<'TResult>(x: int, y: int, title: string) =
    let setProps =
      fun (p: runnableProps<'TResult>) ->
        p.x (Pos.Absolute(x))
        p.y (Pos.Absolute(y))
        p.title title

    let viewProps = runnableProps<'TResult> ()

    setProps viewProps
    new RunnableElement<'TResult>(viewProps.props)
     :> ITerminalElement

  /// <seealso cref="Terminal.Gui.ScrollBar"/>
  static member scrollBar(set: scrollBarProps -> unit) =
    let viewProps = scrollBarProps ()
    set viewProps
    new ScrollBarElement(viewProps.props) :> ITerminalElement

  static member scrollBar(children: ITerminalElement list) =
    let viewProps = scrollBarProps ()
    viewProps.children children
    new ScrollBarElement(viewProps.props) :> ITerminalElement

  static member scrollBar(x: int, y: int, title: string) =
    let setProps =
      fun (p: scrollBarProps) ->
        p.x (Pos.Absolute(x))
        p.y (Pos.Absolute(y))
        p.title title

    let viewProps = scrollBarProps ()
    setProps viewProps
    new ScrollBarElement(viewProps.props) :> ITerminalElement

  /// <seealso cref="Terminal.Gui.ScrollSlider"/>
  static member scrollSlider(set: scrollSliderProps -> unit) =
    let viewProps = scrollSliderProps ()
    set viewProps
    new ScrollSliderElement(viewProps.props) :> ITerminalElement

  static member scrollSlider(children: ITerminalElement list) =
    let viewProps = scrollSliderProps ()
    viewProps.children children
    new ScrollSliderElement(viewProps.props) :> ITerminalElement

  static member scrollSlider(x: int, y: int, title: string) =
    let setProps =
      fun (p: scrollSliderProps) ->
        p.x (Pos.Absolute(x))
        p.y (Pos.Absolute(y))
        p.title title

    let viewProps = scrollSliderProps ()
    setProps viewProps
    new ScrollSliderElement(viewProps.props) :> ITerminalElement

  /// <seealso cref="Terminal.Gui.SelectorBase"/>
  static member selectorBase(set: selectorBaseProps -> unit) =
    let viewProps = selectorBaseProps ()
    set viewProps
    new SelectorBaseElement(viewProps.props) :> ITerminalElement

  static member selectorBase(children: ITerminalElement list) =
    let viewProps = selectorBaseProps ()
    viewProps.children children
    new SelectorBaseElement(viewProps.props) :> ITerminalElement

  static member selectorBase(x: int, y: int, title: string) =
    let setProps =
      fun (p: selectorBaseProps) ->
        p.x (Pos.Absolute(x))
        p.y (Pos.Absolute(y))
        p.title title

    let viewProps = selectorBaseProps ()
    setProps viewProps
    new SelectorBaseElement(viewProps.props) :> ITerminalElement

  /// <seealso cref="Terminal.Gui.Shortcut"/>
  static member shortcut(set: shortcutProps -> unit) =
    let viewProps = shortcutProps ()
    set viewProps
    new ShortcutElement(viewProps.props) :> ITerminalElement

  static member shortcut(children: ITerminalElement list) =
    let viewProps = shortcutProps ()
    viewProps.children children
    new ShortcutElement(viewProps.props) :> ITerminalElement

  static member shortcut(x: int, y: int, title: string) =
    let setProps =
      fun (p: shortcutProps) ->
        p.x (Pos.Absolute(x))
        p.y (Pos.Absolute(y))
        p.title title

    let viewProps = shortcutProps ()
    setProps viewProps
    new ShortcutElement(viewProps.props) :> ITerminalElement

  /// <seealso cref="Terminal.Gui.Slider"/>
  static member slider<'T>(set: sliderProps<'T> -> unit) =
    let viewProps = sliderProps<'T> ()

    set viewProps
    new SliderElement<'T>(viewProps.props)
    : ISliderElement

  static member slider<'T>(children: ITerminalElement list) =
    let viewProps = sliderProps<'T> ()

    viewProps.children children
    new SliderElement<'T>(viewProps.props)
    : ISliderElement

  static member slider<'T>(x: int, y: int, title: string) =
    let setProps =
      fun (p: sliderProps<'T>) ->
        p.x (Pos.Absolute(x))
        p.y (Pos.Absolute(y))
        p.title title

    let viewProps = sliderProps<'T> ()

    setProps viewProps
    new SliderElement<'T>(viewProps.props)
    : ISliderElement

  /// <seealso cref="Terminal.Gui.SpinnerView"/>
  static member spinnerView(set: spinnerViewProps -> unit) =
    let viewProps = spinnerViewProps ()
    set viewProps
    new SpinnerViewElement(viewProps.props) :> ITerminalElement

  static member spinnerView(children: ITerminalElement list) =
    let viewProps = spinnerViewProps ()
    viewProps.children children
    new SpinnerViewElement(viewProps.props) :> ITerminalElement

  static member spinnerView(x: int, y: int, title: string) =
    let setProps =
      fun (p: spinnerViewProps) ->
        p.x (Pos.Absolute(x))
        p.y (Pos.Absolute(y))
        p.title title

    let viewProps = spinnerViewProps ()
    setProps viewProps
    new SpinnerViewElement(viewProps.props) :> ITerminalElement

  /// <seealso cref="Terminal.Gui.StatusBar"/>
  static member statusBar(set: statusBarProps -> unit) =
    let viewProps = statusBarProps ()
    set viewProps
    new StatusBarElement(viewProps.props) :> ITerminalElement

  static member statusBar(children: ITerminalElement list) =
    let viewProps = statusBarProps ()
    viewProps.children children
    new StatusBarElement(viewProps.props) :> ITerminalElement

  static member statusBar(x: int, y: int, title: string) =
    let setProps =
      fun (p: statusBarProps) ->
        p.x (Pos.Absolute(x))
        p.y (Pos.Absolute(y))
        p.title title

    let viewProps = statusBarProps ()
    setProps viewProps
    new StatusBarElement(viewProps.props) :> ITerminalElement

  /// <seealso cref="Terminal.Gui.Tab"/>
  static member tab(set: tabProps -> unit) =
    let viewProps = tabProps ()
    set viewProps
    new TabElement(viewProps.props) :> ITabTerminalElement

  static member tab(children: ITerminalElement list) =
    let viewProps = tabProps ()
    viewProps.children children
    new TabElement(viewProps.props) :> ITabTerminalElement

  static member tab(x: int, y: int, title: string) =
    let setProps =
      fun (p: tabProps) ->
        p.x (Pos.Absolute(x))
        p.y (Pos.Absolute(y))
        p.title title

    let viewProps = tabProps ()
    setProps viewProps
    new TabElement(viewProps.props) :> ITabTerminalElement

  /// <seealso cref="Terminal.Gui.TabView"/>
  static member tabView(set: tabViewProps -> unit) =
    let viewProps = tabViewProps ()
    set viewProps
    new TabViewElement(viewProps.props) :> ITerminalElement

  static member tabView(children: ITerminalElement list) =
    let viewProps = tabViewProps ()
    viewProps.children children
    new TabViewElement(viewProps.props) :> ITerminalElement

  static member tabView(x: int, y: int, title: string) =
    let setProps =
      fun (p: tabViewProps) ->
        p.x (Pos.Absolute(x))
        p.y (Pos.Absolute(y))
        p.title title

    let viewProps = tabViewProps ()
    setProps viewProps
    new TabViewElement(viewProps.props) :> ITerminalElement

  /// <seealso cref="Terminal.Gui.TableView"/>
  static member tableView(set: tableViewProps -> unit) =
    let viewProps = tableViewProps ()
    set viewProps
    new TableViewElement(viewProps.props) :> ITerminalElement

  static member tableView(children: ITerminalElement list) =
    let viewProps = tableViewProps ()
    viewProps.children children
    new TableViewElement(viewProps.props) :> ITerminalElement

  static member tableView(x: int, y: int, title: string) =
    let setProps =
      fun (p: tableViewProps) ->
        p.x (Pos.Absolute(x))
        p.y (Pos.Absolute(y))
        p.title title

    let viewProps = tableViewProps ()
    setProps viewProps
    new TableViewElement(viewProps.props) :> ITerminalElement

  /// <seealso cref="Terminal.Gui.TextField"/>
  static member textField(set: textFieldProps -> unit) =
    let viewProps = textFieldProps ()
    set viewProps
    new TextFieldElement(viewProps.props) :> ITerminalElement

  static member textField(children: ITerminalElement list) =
    let viewProps = textFieldProps ()
    viewProps.children children
    new TextFieldElement(viewProps.props) :> ITerminalElement

  static member textField(x: int, y: int, title: string) =
    let setProps =
      fun (p: textFieldProps) ->
        p.x (Pos.Absolute(x))
        p.y (Pos.Absolute(y))
        p.title title

    let viewProps = textFieldProps ()
    setProps viewProps
    new TextFieldElement(viewProps.props) :> ITerminalElement

  /// <seealso cref="Terminal.Gui.DateField"/>
  static member dateField(set: dateFieldProps -> unit) =
    let viewProps = dateFieldProps ()
    set viewProps
    new DateFieldElement(viewProps.props) :> ITerminalElement

  static member dateField(children: ITerminalElement list) =
    let viewProps = dateFieldProps ()
    viewProps.children children
    new DateFieldElement(viewProps.props) :> ITerminalElement

  static member dateField(x: int, y: int, title: string) =
    let setProps =
      fun (p: dateFieldProps) ->
        p.x (Pos.Absolute(x))
        p.y (Pos.Absolute(y))
        p.title title

    let viewProps = dateFieldProps ()
    setProps viewProps
    new DateFieldElement(viewProps.props) :> ITerminalElement

  /// <seealso cref="Terminal.Gui.TextValidateField"/>
  static member textValidateField(set: textValidateFieldProps -> unit) =
    let viewProps = textValidateFieldProps ()
    set viewProps
    new TextValidateFieldElement(viewProps.props) :> ITerminalElement

  static member textValidateField(children: ITerminalElement list) =
    let viewProps = textValidateFieldProps ()
    viewProps.children children
    new TextValidateFieldElement(viewProps.props) :> ITerminalElement

  static member textValidateField(x: int, y: int, title: string) =
    let setProps =
      fun (p: textValidateFieldProps) ->
        p.x (Pos.Absolute(x))
        p.y (Pos.Absolute(y))
        p.title title

    let viewProps = textValidateFieldProps ()
    setProps viewProps
    new TextValidateFieldElement(viewProps.props) :> ITerminalElement

  /// <seealso cref="Terminal.Gui.TextView"/>
  static member textView(set: textViewProps -> unit) =
    let viewProps = textViewProps ()
    set viewProps
    new TextViewElement(viewProps.props) :> ITerminalElement

  static member textView(children: ITerminalElement list) =
    let viewProps = textViewProps ()
    viewProps.children children
    new TextViewElement(viewProps.props) :> ITerminalElement

  static member textView(x: int, y: int, title: string) =
    let setProps =
      fun (p: textViewProps) ->
        p.x (Pos.Absolute(x))
        p.y (Pos.Absolute(y))
        p.title title

    let viewProps = textViewProps ()
    setProps viewProps
    new TextViewElement(viewProps.props) :> ITerminalElement

  /// <seealso cref="Terminal.Gui.TimeField"/>
  static member timeField(set: timeFieldProps -> unit) =
    let viewProps = timeFieldProps ()
    set viewProps
    new TimeFieldElement(viewProps.props) :> ITerminalElement

  static member timeField(children: ITerminalElement list) =
    let viewProps = timeFieldProps ()
    viewProps.children children
    new TimeFieldElement(viewProps.props) :> ITerminalElement

  static member timeField(x: int, y: int, title: string) =
    let setProps =
      fun (p: timeFieldProps) ->
        p.x (Pos.Absolute(x))
        p.y (Pos.Absolute(y))
        p.title title

    let viewProps = timeFieldProps ()
    setProps viewProps
    new TimeFieldElement(viewProps.props) :> ITerminalElement

  /// <seealso cref="Terminal.Gui.TreeView"/>
  static member treeView<'T when 'T: not struct>(set: treeViewProps<'T> -> unit) =
    let viewProps = treeViewProps<'T> ()

    set viewProps
    new TreeViewElement<'T>(viewProps.props)
    : ITreeViewElement

  static member treeView<'T when 'T: not struct>(children: ITerminalElement list) =
    let viewProps = treeViewProps<'T> ()

    viewProps.children children
    new TreeViewElement<'T>(viewProps.props)
    : ITreeViewElement

  static member treeView<'T when 'T: not struct>(x: int, y: int, title: string) =
    let setProps =
      fun (p: treeViewProps<'T>) ->
        p.x (Pos.Absolute(x))
        p.y (Pos.Absolute(y))
        p.title title

    let viewProps = treeViewProps<'T> ()

    setProps viewProps
    new TreeViewElement<'T>(viewProps.props)
    : ITreeViewElement

  /// <seealso cref="Terminal.Gui.Window"/>
  static member window(set: windowProps -> unit) =
    let viewProps = windowProps ()
    set viewProps
    new WindowElement(viewProps.props) :> ITerminalElement

  static member window(children: ITerminalElement list) =
    let viewProps = windowProps ()
    viewProps.children children
    new WindowElement(viewProps.props) :> ITerminalElement

  static member window(x: int, y: int, title: string) =
    let setProps =
      fun (p: windowProps) ->
        p.x (Pos.Absolute(x))
        p.y (Pos.Absolute(y))
        p.title title

    let viewProps = windowProps ()
    setProps viewProps
    new WindowElement(viewProps.props) :> ITerminalElement

  /// <seealso cref="Terminal.Gui.Dialog"/>
  static member dialog(set: dialogProps -> unit) =
    let viewProps = dialogProps ()
    set viewProps
    new DialogElement(viewProps.props) :> ITerminalElement

  static member dialog(children: ITerminalElement list) =
    let viewProps = dialogProps ()
    viewProps.children children
    new DialogElement(viewProps.props) :> ITerminalElement

  static member dialog(x: int, y: int, title: string) =
    let setProps =
      fun (p: dialogProps) ->
        p.x (Pos.Absolute(x))
        p.y (Pos.Absolute(y))
        p.title title

    let viewProps = dialogProps ()
    setProps viewProps
    new DialogElement(viewProps.props) :> ITerminalElement

  /// <seealso cref="Terminal.Gui.FileDialog"/>
  static member fileDialog(set: fileDialogProps -> unit) =
    let viewProps = fileDialogProps ()
    set viewProps
    new FileDialogElement(viewProps.props) :> ITerminalElement

  static member fileDialog(children: ITerminalElement list) =
    let viewProps = fileDialogProps ()
    viewProps.children children
    new FileDialogElement(viewProps.props) :> ITerminalElement

  static member fileDialog(x: int, y: int, title: string) =
    let setProps =
      fun (p: fileDialogProps) ->
        p.x (Pos.Absolute(x))
        p.y (Pos.Absolute(y))
        p.title title

    let viewProps = fileDialogProps ()
    setProps viewProps
    new FileDialogElement(viewProps.props) :> ITerminalElement

  /// <seealso cref="Terminal.Gui.OpenDialog"/>
  static member openDialog(set: openDialogProps -> unit) =
    let viewProps = openDialogProps ()
    set viewProps
    new OpenDialogElement(viewProps.props) :> ITerminalElement

  static member openDialog(children: ITerminalElement list) =
    let viewProps = openDialogProps ()
    viewProps.children children
    new OpenDialogElement(viewProps.props) :> ITerminalElement

  static member openDialog(x: int, y: int, title: string) =
    let setProps =
      fun (p: openDialogProps) ->
        p.x (Pos.Absolute(x))
        p.y (Pos.Absolute(y))
        p.title title

    let viewProps = openDialogProps ()
    setProps viewProps
    new OpenDialogElement(viewProps.props) :> ITerminalElement

  /// <seealso cref="Terminal.Gui.SaveDialog"/>
  static member saveDialog(set: saveDialogProps -> unit) =
    let viewProps = saveDialogProps ()
    set viewProps
    new SaveDialogElement(viewProps.props) :> ITerminalElement

  static member saveDialog(children: ITerminalElement list) =
    let viewProps = saveDialogProps ()
    viewProps.children children
    new SaveDialogElement(viewProps.props) :> ITerminalElement

  static member saveDialog(x: int, y: int, title: string) =
    let setProps =
      fun (p: saveDialogProps) ->
        p.x (Pos.Absolute(x))
        p.y (Pos.Absolute(y))
        p.title title

    let viewProps = saveDialogProps ()
    setProps viewProps
    new SaveDialogElement(viewProps.props) :> ITerminalElement

  /// <seealso cref="Terminal.Gui.Wizard"/>
  static member wizard(set: wizardProps -> unit) =
    let viewProps = wizardProps ()
    set viewProps
    new WizardElement(viewProps.props) :> ITerminalElement

  static member wizard(children: ITerminalElement list) =
    let viewProps = wizardProps ()
    viewProps.children children
    new WizardElement(viewProps.props) :> ITerminalElement

  static member wizard(x: int, y: int, title: string) =
    let setProps =
      fun (p: wizardProps) ->
        p.x (Pos.Absolute(x))
        p.y (Pos.Absolute(y))
        p.title title

    let viewProps = wizardProps ()
    setProps viewProps
    new WizardElement(viewProps.props) :> ITerminalElement

  /// <seealso cref="Terminal.Gui.WizardStep"/>
  static member wizardStep(set: wizardStepProps -> unit) =
    let viewProps = wizardStepProps ()
    set viewProps
    new WizardStepElement(viewProps.props) :> IWizardStepTerminalElement

  static member wizardStep(children: ITerminalElement list) =
    let viewProps = wizardStepProps ()
    viewProps.children children
    new WizardStepElement(viewProps.props) :> IWizardStepTerminalElement

  static member wizardStep(x: int, y: int, title: string) =
    let setProps =
      fun (p: wizardStepProps) ->
        p.x (Pos.Absolute(x))
        p.y (Pos.Absolute(y))
        p.title title

    let viewProps = wizardStepProps ()
    setProps viewProps
    new WizardStepElement(viewProps.props) :> IWizardStepTerminalElement
