(*
#######################################
#            View.fs                  #
#######################################
*)


namespace Terminal.Gui.Elmish

open System
open Terminal.Gui.Elmish
open Terminal.Gui.Elmish.Elements
open Terminal.Gui.ViewBase
open Terminal.Gui.Views

type View =

  /// <seealso cref="Terminal.Gui.Adornment"/>
  static member adornment(set: adornmentProps -> unit) =
    let viewProps = adornmentProps ()
    set viewProps
    AdornmentElement(viewProps.props) :> ITerminalElement

  static member adornment(children: ITerminalElement list) =
    let viewProps = adornmentProps ()
    viewProps.children children
    AdornmentElement(viewProps.props) :> ITerminalElement

  static member adornment(x: int, y: int, title: string) =
    let setProps =
      fun (p: adornmentProps) ->
        p.x (Pos.Absolute(x))
        p.y (Pos.Absolute(y))
        p.title title

    let viewProps = adornmentProps ()
    setProps viewProps
    AdornmentElement(viewProps.props) :> ITerminalElement

  /// <seealso cref="Terminal.Gui.Bar"/>
  static member bar(set: barProps -> unit) =
    let viewProps = barProps ()
    set viewProps
    BarElement(viewProps.props) :> ITerminalElement

  static member bar(children: ITerminalElement list) =
    let viewProps = barProps ()
    viewProps.children children
    BarElement(viewProps.props) :> ITerminalElement

  static member bar(x: int, y: int, title: string) =
    let setProps =
      fun (p: barProps) ->
        p.x (Pos.Absolute(x))
        p.y (Pos.Absolute(y))
        p.title title

    let viewProps = barProps ()
    setProps viewProps
    BarElement(viewProps.props) :> ITerminalElement

  /// <seealso cref="Terminal.Gui.Border"/>
  static member border(set: borderProps -> unit) =
    let viewProps = borderProps ()
    set viewProps
    BorderElement(viewProps.props) :> ITerminalElement

  static member border(children: ITerminalElement list) =
    let viewProps = borderProps ()
    viewProps.children children
    BorderElement(viewProps.props) :> ITerminalElement

  static member border(x: int, y: int, title: string) =
    let setProps =
      fun (p: borderProps) ->
        p.x (Pos.Absolute(x))
        p.y (Pos.Absolute(y))
        p.title title

    let viewProps = borderProps ()
    setProps viewProps
    BorderElement(viewProps.props) :> ITerminalElement

  /// <seealso cref="Terminal.Gui.Button"/>
  static member button(set: buttonProps -> unit) =
    let viewProps = buttonProps ()
    set viewProps
    ButtonElement(viewProps.props) :> ITerminalElement

  static member button(children: ITerminalElement list) =
    let viewProps = buttonProps ()
    viewProps.children children
    ButtonElement(viewProps.props) :> ITerminalElement

  static member button(x: int, y: int, title: string) =
    let setProps =
      fun (p: buttonProps) ->
        p.x (Pos.Absolute(x))
        p.y (Pos.Absolute(y))
        p.title title

    let viewProps = buttonProps ()
    setProps viewProps
    ButtonElement(viewProps.props) :> ITerminalElement

  /// <seealso cref="Terminal.Gui.CheckBox"/>
  static member checkBox(set: checkBoxProps -> unit) =
    let viewProps = checkBoxProps ()
    set viewProps
    CheckBoxElement(viewProps.props) :> ITerminalElement

  static member checkBox(children: ITerminalElement list) =
    let viewProps = checkBoxProps ()
    viewProps.children children
    CheckBoxElement(viewProps.props) :> ITerminalElement

  static member checkBox(x: int, y: int, title: string) =
    let setProps =
      fun (p: checkBoxProps) ->
        p.x (Pos.Absolute(x))
        p.y (Pos.Absolute(y))
        p.title title

    let viewProps = checkBoxProps ()
    setProps viewProps
    CheckBoxElement(viewProps.props) :> ITerminalElement

  /// <seealso cref="Terminal.Gui.ColorPicker"/>
  static member colorPicker(set: colorPickerProps -> unit) =
    let viewProps = colorPickerProps ()
    set viewProps
    ColorPickerElement(viewProps.props) :> ITerminalElement

  static member colorPicker(children: ITerminalElement list) =
    let viewProps = colorPickerProps ()
    viewProps.children children
    ColorPickerElement(viewProps.props) :> ITerminalElement

  static member colorPicker(x: int, y: int, title: string) =
    let setProps =
      fun (p: colorPickerProps) ->
        p.x (Pos.Absolute(x))
        p.y (Pos.Absolute(y))
        p.title title

    let viewProps = colorPickerProps ()
    setProps viewProps
    ColorPickerElement(viewProps.props) :> ITerminalElement

  /// <seealso cref="Terminal.Gui.ColorPicker16"/>
  static member colorPicker16(set: colorPicker16Props -> unit) =
    let viewProps = colorPicker16Props ()
    set viewProps
    ColorPicker16Element(viewProps.props) :> ITerminalElement

  static member colorPicker16(children: ITerminalElement list) =
    let viewProps = colorPicker16Props ()
    viewProps.children children
    ColorPicker16Element(viewProps.props) :> ITerminalElement

  static member colorPicker16(x: int, y: int, title: string) =
    let setProps =
      fun (p: colorPicker16Props) ->
        p.x (Pos.Absolute(x))
        p.y (Pos.Absolute(y))
        p.title title

    let viewProps = colorPicker16Props ()
    setProps viewProps
    ColorPicker16Element(viewProps.props) :> ITerminalElement

  /// <seealso cref="Terminal.Gui.ComboBox"/>
  static member comboBox(set: comboBoxProps -> unit) =
    let viewProps = comboBoxProps ()
    set viewProps
    ComboBoxElement(viewProps.props) :> ITerminalElement

  static member comboBox(children: ITerminalElement list) =
    let viewProps = comboBoxProps ()
    viewProps.children children
    ComboBoxElement(viewProps.props) :> ITerminalElement

  static member comboBox(x: int, y: int, title: string) =
    let setProps =
      fun (p: comboBoxProps) ->
        p.x (Pos.Absolute(x))
        p.y (Pos.Absolute(y))
        p.title title

    let viewProps = comboBoxProps ()
    setProps viewProps
    ComboBoxElement(viewProps.props) :> ITerminalElement

  /// <seealso cref="Terminal.Gui.DateField"/>
  static member dateField(set: dateFieldProps -> unit) =
    let viewProps = dateFieldProps ()
    set viewProps
    DateFieldElement(viewProps.props) :> ITerminalElement

  static member dateField(children: ITerminalElement list) =
    let viewProps = dateFieldProps ()
    viewProps.children children
    DateFieldElement(viewProps.props) :> ITerminalElement

  static member dateField(x: int, y: int, title: string) =
    let setProps =
      fun (p: dateFieldProps) ->
        p.x (Pos.Absolute(x))
        p.y (Pos.Absolute(y))
        p.title title

    let viewProps = dateFieldProps ()
    setProps viewProps
    DateFieldElement(viewProps.props) :> ITerminalElement

  /// <seealso cref="Terminal.Gui.DatePicker"/>
  static member datePicker(set: datePickerProps -> unit) =
    let viewProps = datePickerProps ()
    set viewProps
    DatePickerElement(viewProps.props) :> ITerminalElement

  static member datePicker(children: ITerminalElement list) =
    let viewProps = datePickerProps ()
    viewProps.children children
    DatePickerElement(viewProps.props) :> ITerminalElement

  static member datePicker(x: int, y: int, title: string) =
    let setProps =
      fun (p: datePickerProps) ->
        p.x (Pos.Absolute(x))
        p.y (Pos.Absolute(y))
        p.title title

    let viewProps = datePickerProps ()
    setProps viewProps
    DatePickerElement(viewProps.props) :> ITerminalElement

  /// <seealso cref="Terminal.Gui.Dialog"/>
  static member dialog(set: dialogProps -> unit) =
    let viewProps = dialogProps ()
    set viewProps
    DialogElement(viewProps.props) :> ITerminalElement

  static member dialog(children: ITerminalElement list) =
    let viewProps = dialogProps ()
    viewProps.children children
    DialogElement(viewProps.props) :> ITerminalElement

  static member dialog(x: int, y: int, title: string) =
    let setProps =
      fun (p: dialogProps) ->
        p.x (Pos.Absolute(x))
        p.y (Pos.Absolute(y))
        p.title title

    let viewProps = dialogProps ()
    setProps viewProps
    DialogElement(viewProps.props) :> ITerminalElement

  /// <seealso cref="Terminal.Gui.FileDialog"/>
  static member fileDialog(set: fileDialogProps -> unit) =
    let viewProps = fileDialogProps ()
    set viewProps
    FileDialogElement(viewProps.props) :> ITerminalElement

  static member fileDialog(children: ITerminalElement list) =
    let viewProps = fileDialogProps ()
    viewProps.children children
    FileDialogElement(viewProps.props) :> ITerminalElement

  static member fileDialog(x: int, y: int, title: string) =
    let setProps =
      fun (p: fileDialogProps) ->
        p.x (Pos.Absolute(x))
        p.y (Pos.Absolute(y))
        p.title title

    let viewProps = fileDialogProps ()
    setProps viewProps
    FileDialogElement(viewProps.props) :> ITerminalElement

  /// <seealso cref="Terminal.Gui.FrameView"/>
  static member frameView(set: frameViewProps -> unit) =
    let viewProps = frameViewProps ()
    set viewProps
    FrameViewElement(viewProps.props) :> ITerminalElement

  static member frameView(children: ITerminalElement list) =
    let viewProps = frameViewProps ()
    viewProps.children children
    FrameViewElement(viewProps.props) :> ITerminalElement

  static member frameView(x: int, y: int, title: string) =
    let setProps =
      fun (p: frameViewProps) ->
        p.x (Pos.Absolute(x))
        p.y (Pos.Absolute(y))
        p.title title

    let viewProps = frameViewProps ()
    setProps viewProps
    FrameViewElement(viewProps.props) :> ITerminalElement

  /// <seealso cref="Terminal.Gui.GraphView"/>
  static member graphView(set: graphViewProps -> unit) =
    let viewProps = graphViewProps ()
    set viewProps
    GraphViewElement(viewProps.props) :> ITerminalElement

  static member graphView(children: ITerminalElement list) =
    let viewProps = graphViewProps ()
    viewProps.children children
    GraphViewElement(viewProps.props) :> ITerminalElement

  static member graphView(x: int, y: int, title: string) =
    let setProps =
      fun (p: graphViewProps) ->
        p.x (Pos.Absolute(x))
        p.y (Pos.Absolute(y))
        p.title title

    let viewProps = graphViewProps ()
    setProps viewProps
    GraphViewElement(viewProps.props) :> ITerminalElement

  /// <seealso cref="Terminal.Gui.HexView"/>
  static member hexView(set: hexViewProps -> unit) =
    let viewProps = hexViewProps ()
    set viewProps
    HexViewElement(viewProps.props) :> ITerminalElement

  static member hexView(children: ITerminalElement list) =
    let viewProps = hexViewProps ()
    viewProps.children children
    HexViewElement(viewProps.props) :> ITerminalElement

  static member hexView(x: int, y: int, title: string) =
    let setProps =
      fun (p: hexViewProps) ->
        p.x (Pos.Absolute(x))
        p.y (Pos.Absolute(y))
        p.title title

    let viewProps = hexViewProps ()
    setProps viewProps
    HexViewElement(viewProps.props) :> ITerminalElement

  /// <seealso cref="Terminal.Gui.Label"/>
  static member label(set: labelProps -> unit) =
    let viewProps = labelProps ()
    set viewProps
    LabelElement(viewProps.props) :> ITerminalElement

  static member label(children: ITerminalElement list) =
    let viewProps = labelProps ()
    viewProps.children children
    LabelElement(viewProps.props) :> ITerminalElement

  static member label(x: int, y: int, title: string) =
    let setProps =
      fun (p: labelProps) ->
        p.x (Pos.Absolute(x))
        p.y (Pos.Absolute(y))
        p.title title

    let viewProps = labelProps ()
    setProps viewProps
    LabelElement(viewProps.props) :> ITerminalElement

  /// <seealso cref="Terminal.Gui.LegendAnnotation"/>
  static member legendAnnotation(set: legendAnnotationProps -> unit) =
    let viewProps = legendAnnotationProps ()
    set viewProps
    LegendAnnotationElement(viewProps.props) :> ITerminalElement

  static member legendAnnotation(children: ITerminalElement list) =
    let viewProps = legendAnnotationProps ()
    viewProps.children children
    LegendAnnotationElement(viewProps.props) :> ITerminalElement

  static member legendAnnotation(x: int, y: int, title: string) =
    let setProps =
      fun (p: legendAnnotationProps) ->
        p.x (Pos.Absolute(x))
        p.y (Pos.Absolute(y))
        p.title title

    let viewProps = legendAnnotationProps ()
    setProps viewProps
    LegendAnnotationElement(viewProps.props) :> ITerminalElement

  /// <seealso cref="Terminal.Gui.Line"/>
  static member line(set: lineProps -> unit) =
    let viewProps = lineProps ()
    set viewProps
    LineElement(viewProps.props) :> ITerminalElement

  static member line(children: ITerminalElement list) =
    let viewProps = lineProps ()
    viewProps.children children
    LineElement(viewProps.props) :> ITerminalElement

  static member line(x: int, y: int, title: string) =
    let setProps =
      fun (p: lineProps) ->
        p.x (Pos.Absolute(x))
        p.y (Pos.Absolute(y))
        p.title title

    let viewProps = lineProps ()
    setProps viewProps
    LineElement(viewProps.props) :> ITerminalElement

  /// <seealso cref="Terminal.Gui.ListView"/>
  static member listView(set: listViewProps -> unit) =
    let viewProps = listViewProps ()
    set viewProps
    ListViewElement(viewProps.props) :> ITerminalElement

  static member listView(children: ITerminalElement list) =
    let viewProps = listViewProps ()
    viewProps.children children
    ListViewElement(viewProps.props) :> ITerminalElement

  static member listView(x: int, y: int, title: string) =
    let setProps =
      fun (p: listViewProps) ->
        p.x (Pos.Absolute(x))
        p.y (Pos.Absolute(y))
        p.title title

    let viewProps = listViewProps ()
    setProps viewProps
    ListViewElement(viewProps.props) :> ITerminalElement

  /// <seealso cref="Terminal.Gui.Margin"/>
  static member margin(set: marginProps -> unit) =
    let viewProps = marginProps ()
    set viewProps
    MarginElement(viewProps.props) :> ITerminalElement

  static member margin(children: ITerminalElement list) =
    let viewProps = marginProps ()
    viewProps.children children
    MarginElement(viewProps.props) :> ITerminalElement

  static member margin(x: int, y: int, title: string) =
    let setProps =
      fun (p: marginProps) ->
        p.x (Pos.Absolute(x))
        p.y (Pos.Absolute(y))
        p.title title

    let viewProps = marginProps ()
    setProps viewProps
    MarginElement(viewProps.props) :> ITerminalElement

  /// <seealso cref="Terminal.Gui.MenuBar"/>
  static member menuBar(set: menuBarProps -> menuBarMacros -> unit) =
    let props = menuBarProps ()
    let macros = menuBarMacros props
    set props macros
    MenuBarElement(props.props) :> ITerminalElement

  static member menuBar(children: ITerminalElement list) =
    let viewProps = menuBarProps ()
    viewProps.children children
    MenuBarElement(viewProps.props) :> ITerminalElement

  static member menuBar(x: int, y: int, title: string) =
    let setProps =
      fun (p: menuBarProps) ->
        p.x (Pos.Absolute(x))
        p.y (Pos.Absolute(y))
        p.title title

    let viewProps = menuBarProps ()
    setProps viewProps
    MenuBarElement(viewProps.props) :> ITerminalElement

  /// <seealso cref="Terminal.Gui.Menu"/>
  static member menu(set: menuProps -> unit) =
    let viewProps = menuProps ()
    set viewProps
    MenuElement(viewProps.props) :> IMenuElement

  static member menu(children: ITerminalElement list) =
    let viewProps = menuProps ()
    viewProps.children children
    MenuElement(viewProps.props) :> ITerminalElement

  static member menu(x: int, y: int, title: string) =
    let setProps =
      fun (p: menuProps) ->
        p.x (Pos.Absolute(x))
        p.y (Pos.Absolute(y))
        p.title title

    let viewProps = menuProps ()
    setProps viewProps
    MenuElement(viewProps.props) :> ITerminalElement

  static member menuBarItem(set: menuBarItemProps -> unit) =
    let viewProps = menuBarItemProps ()
    set viewProps
    MenuBarItemElement(viewProps.props) :> IMenuBarItemElement

  static member popoverMenu(set: popoverMenuProps -> unit) =
    let viewProps = popoverMenuProps ()
    set viewProps
    PopoverMenuElement(viewProps.props) :> IPopoverMenuElement

  static member menuItem(set: menuItemProps -> unit) =
    let viewProps = menuItemProps ()
    set viewProps
    MenuItemElement(viewProps.props) :> ITerminalElement

  /// <seealso cref="Terminal.Gui.NumericUpDown"/>
  static member numericUpDown(set: numericUpDownProps -> unit) =
    let viewProps = numericUpDownProps ()
    set viewProps
    NumericUpDownElement(viewProps.props) :> ITerminalElement

  static member numericUpDown(children: ITerminalElement list) =
    let viewProps = numericUpDownProps ()
    viewProps.children children
    NumericUpDownElement(viewProps.props) :> ITerminalElement

  static member numericUpDown(x: int, y: int, title: string) =
    let setProps =
      fun (p: numericUpDownProps) ->
        p.x (Pos.Absolute(x))
        p.y (Pos.Absolute(y))
        p.title title

    let viewProps = numericUpDownProps ()
    setProps viewProps
    NumericUpDownElement(viewProps.props) :> ITerminalElement

  /// <seealso cref="Terminal.Gui.NumericUpDown"/>
  static member numericUpDown<'a>(set: numericUpDownProps<'a> -> unit) =
    let viewProps = numericUpDownProps<'a> ()
    set viewProps
    NumericUpDownElement<'a>(viewProps.props) :> INumericUpDownElement

  static member numericUpDown<'a>(children: ITerminalElement list) =
    let viewProps = numericUpDownProps<'a> ()
    viewProps.children children
    NumericUpDownElement<'a>(viewProps.props) :> INumericUpDownElement

  static member numericUpDown<'a>(x: int, y: int, title: string) =
    let setProps =
      fun (p: numericUpDownProps<'a>) ->
        p.x (Pos.Absolute(x))
        p.y (Pos.Absolute(y))
        p.title title

    let viewProps = numericUpDownProps<'a> ()
    setProps viewProps
    NumericUpDownElement<'a>(viewProps.props) :> INumericUpDownElement

  /// <seealso cref="Terminal.Gui.OpenDialog"/>
  static member openDialog(set: openDialogProps -> unit) =
    let viewProps = openDialogProps ()
    set viewProps
    OpenDialogElement(viewProps.props) :> ITerminalElement

  static member openDialog(children: ITerminalElement list) =
    let viewProps = openDialogProps ()
    viewProps.children children
    OpenDialogElement(viewProps.props) :> ITerminalElement

  static member openDialog(x: int, y: int, title: string) =
    let setProps =
      fun (p: openDialogProps) ->
        p.x (Pos.Absolute(x))
        p.y (Pos.Absolute(y))
        p.title title

    let viewProps = openDialogProps ()
    setProps viewProps
    OpenDialogElement(viewProps.props) :> ITerminalElement

  static member optionSelector(set: optionSelectorProps -> unit) =
    let viewProps = optionSelectorProps ()
    set viewProps
    OptionSelectorElement(viewProps.props) :> ITerminalElement

  static member flagSelector(set: flagSelectorProps -> unit) =
    let viewProps = flagSelectorProps ()
    set viewProps
    FlagSelectorElement(viewProps.props) :> ITerminalElement

  /// <seealso cref="Terminal.Gui.Padding"/>
  static member padding(set: paddingProps -> unit) =
    let viewProps = paddingProps ()
    set viewProps
    PaddingElement(viewProps.props) :> ITerminalElement

  static member padding(children: ITerminalElement list) =
    let viewProps = paddingProps ()
    viewProps.children children
    PaddingElement(viewProps.props) :> ITerminalElement

  static member padding(x: int, y: int, title: string) =
    let setProps =
      fun (p: paddingProps) ->
        p.x (Pos.Absolute(x))
        p.y (Pos.Absolute(y))
        p.title title

    let viewProps = paddingProps ()
    setProps viewProps
    PaddingElement(viewProps.props) :> ITerminalElement

  /// <seealso cref="Terminal.Gui.ProgressBar"/>
  static member progressBar(set: progressBarProps -> unit) =
    let viewProps = progressBarProps ()
    set viewProps
    ProgressBarElement(viewProps.props) :> ITerminalElement

  static member progressBar(children: ITerminalElement list) =
    let viewProps = progressBarProps ()
    viewProps.children children
    ProgressBarElement(viewProps.props) :> ITerminalElement

  static member progressBar(x: int, y: int, title: string) =
    let setProps =
      fun (p: progressBarProps) ->
        p.x (Pos.Absolute(x))
        p.y (Pos.Absolute(y))
        p.title title

    let viewProps = progressBarProps ()
    setProps viewProps
    ProgressBarElement(viewProps.props) :> ITerminalElement

  /// <seealso cref="Terminal.Gui.SaveDialog"/>
  static member saveDialog(set: saveDialogProps -> unit) =
    let viewProps = saveDialogProps ()
    set viewProps
    SaveDialogElement(viewProps.props) :> ITerminalElement

  static member saveDialog(children: ITerminalElement list) =
    let viewProps = saveDialogProps ()
    viewProps.children children
    SaveDialogElement(viewProps.props) :> ITerminalElement

  static member saveDialog(x: int, y: int, title: string) =
    let setProps =
      fun (p: saveDialogProps) ->
        p.x (Pos.Absolute(x))
        p.y (Pos.Absolute(y))
        p.title title

    let viewProps = saveDialogProps ()
    setProps viewProps
    SaveDialogElement(viewProps.props) :> ITerminalElement

  /// <seealso cref="Terminal.Gui.ScrollBar"/>
  static member scrollBar(set: scrollBarProps -> unit) =
    let viewProps = scrollBarProps ()
    set viewProps
    ScrollBarElement(viewProps.props) :> ITerminalElement

  static member scrollBar(children: ITerminalElement list) =
    let viewProps = scrollBarProps ()
    viewProps.children children
    ScrollBarElement(viewProps.props) :> ITerminalElement

  static member scrollBar(x: int, y: int, title: string) =
    let setProps =
      fun (p: scrollBarProps) ->
        p.x (Pos.Absolute(x))
        p.y (Pos.Absolute(y))
        p.title title

    let viewProps = scrollBarProps ()
    setProps viewProps
    ScrollBarElement(viewProps.props) :> ITerminalElement

  /// <seealso cref="Terminal.Gui.ScrollSlider"/>
  static member scrollSlider(set: scrollSliderProps -> unit) =
    let viewProps = scrollSliderProps ()
    set viewProps
    ScrollSliderElement(viewProps.props) :> ITerminalElement

  static member scrollSlider(children: ITerminalElement list) =
    let viewProps = scrollSliderProps ()
    viewProps.children children
    ScrollSliderElement(viewProps.props) :> ITerminalElement

  static member scrollSlider(x: int, y: int, title: string) =
    let setProps =
      fun (p: scrollSliderProps) ->
        p.x (Pos.Absolute(x))
        p.y (Pos.Absolute(y))
        p.title title

    let viewProps = scrollSliderProps ()
    setProps viewProps
    ScrollSliderElement(viewProps.props) :> ITerminalElement

  /// <seealso cref="Terminal.Gui.Shortcut"/>
  static member shortcut(set: shortcutProps -> unit) =
    let viewProps = shortcutProps ()
    set viewProps
    ShortcutElement(viewProps.props) :> ITerminalElement

  static member shortcut(children: ITerminalElement list) =
    let viewProps = shortcutProps ()
    viewProps.children children
    ShortcutElement(viewProps.props) :> ITerminalElement

  static member shortcut(x: int, y: int, title: string) =
    let setProps =
      fun (p: shortcutProps) ->
        p.x (Pos.Absolute(x))
        p.y (Pos.Absolute(y))
        p.title title

    let viewProps = shortcutProps ()
    setProps viewProps
    ShortcutElement(viewProps.props) :> ITerminalElement

  /// <seealso cref="Terminal.Gui.Slider"/>
  static member slider(set: sliderProps -> unit) =
    let viewProps = sliderProps ()
    set viewProps
    SliderElement(viewProps.props) :> ITerminalElement

  static member slider(children: ITerminalElement list) =
    let viewProps = sliderProps ()
    viewProps.children children
    SliderElement(viewProps.props) :> ITerminalElement

  static member slider(x: int, y: int, title: string) =
    let setProps =
      fun (p: sliderProps) ->
        p.x (Pos.Absolute(x))
        p.y (Pos.Absolute(y))
        p.title title

    let viewProps = sliderProps ()
    setProps viewProps
    SliderElement(viewProps.props) :> ITerminalElement

  /// <seealso cref="Terminal.Gui.Slider"/>
  static member slider<'a>(set: sliderProps<'a> -> unit) =
    let viewProps = sliderProps<'a> ()

    set viewProps
    SliderElement<'a>(viewProps.props)
    : ISliderElement

  static member slider<'a>(children: ITerminalElement list) =
    let viewProps = sliderProps<'a> ()

    viewProps.children children
    SliderElement<'a>(viewProps.props)
    : ISliderElement

  static member slider<'a>(x: int, y: int, title: string) =
    let setProps =
      fun (p: sliderProps<'a>) ->
        p.x (Pos.Absolute(x))
        p.y (Pos.Absolute(y))
        p.title title

    let viewProps = sliderProps<'a> ()

    setProps viewProps
    SliderElement<'a>(viewProps.props)
    : ISliderElement

  /// <seealso cref="Terminal.Gui.SpinnerView"/>
  static member spinnerView(set: spinnerViewProps -> unit) =
    let viewProps = spinnerViewProps ()
    set viewProps
    SpinnerViewElement(viewProps.props) :> ITerminalElement

  static member spinnerView(children: ITerminalElement list) =
    let viewProps = spinnerViewProps ()
    viewProps.children children
    SpinnerViewElement(viewProps.props) :> ITerminalElement

  static member spinnerView(x: int, y: int, title: string) =
    let setProps =
      fun (p: spinnerViewProps) ->
        p.x (Pos.Absolute(x))
        p.y (Pos.Absolute(y))
        p.title title

    let viewProps = spinnerViewProps ()
    setProps viewProps
    SpinnerViewElement(viewProps.props) :> ITerminalElement

  /// <seealso cref="Terminal.Gui.StatusBar"/>
  static member statusBar(set: statusBarProps -> unit) =
    let viewProps = statusBarProps ()
    set viewProps
    StatusBarElement(viewProps.props) :> ITerminalElement

  static member statusBar(children: ITerminalElement list) =
    let viewProps = statusBarProps ()
    viewProps.children children
    StatusBarElement(viewProps.props) :> ITerminalElement

  static member statusBar(x: int, y: int, title: string) =
    let setProps =
      fun (p: statusBarProps) ->
        p.x (Pos.Absolute(x))
        p.y (Pos.Absolute(y))
        p.title title

    let viewProps = statusBarProps ()
    setProps viewProps
    StatusBarElement(viewProps.props) :> ITerminalElement

  /// <seealso cref="Terminal.Gui.Tab"/>
  static member tab(set: tabProps -> unit) =
    let viewProps = tabProps ()
    set viewProps
    TabElement(viewProps.props) :> ITerminalElement

  static member tab(children: ITerminalElement list) =
    let viewProps = tabProps ()
    viewProps.children children
    TabElement(viewProps.props) :> ITerminalElement

  static member tab(x: int, y: int, title: string) =
    let setProps =
      fun (p: tabProps) ->
        p.x (Pos.Absolute(x))
        p.y (Pos.Absolute(y))
        p.title title

    let viewProps = tabProps ()
    setProps viewProps
    TabElement(viewProps.props) :> ITerminalElement

  /// <seealso cref="Terminal.Gui.TabView"/>
  static member tabView(set: tabViewProps -> unit) =
    let viewProps = tabViewProps ()
    set viewProps
    TabViewElement(viewProps.props) :> ITerminalElement

  static member tabView(children: ITerminalElement list) =
    let viewProps = tabViewProps ()
    viewProps.children children
    TabViewElement(viewProps.props) :> ITerminalElement

  static member tabView(x: int, y: int, title: string) =
    let setProps =
      fun (p: tabViewProps) ->
        p.x (Pos.Absolute(x))
        p.y (Pos.Absolute(y))
        p.title title

    let viewProps = tabViewProps ()
    setProps viewProps
    TabViewElement(viewProps.props) :> ITerminalElement

  /// <seealso cref="Terminal.Gui.TableView"/>
  static member tableView(set: tableViewProps -> unit) =
    let viewProps = tableViewProps ()
    set viewProps
    TableViewElement(viewProps.props) :> ITerminalElement

  static member tableView(children: ITerminalElement list) =
    let viewProps = tableViewProps ()
    viewProps.children children
    TableViewElement(viewProps.props) :> ITerminalElement

  static member tableView(x: int, y: int, title: string) =
    let setProps =
      fun (p: tableViewProps) ->
        p.x (Pos.Absolute(x))
        p.y (Pos.Absolute(y))
        p.title title

    let viewProps = tableViewProps ()
    setProps viewProps
    TableViewElement(viewProps.props) :> ITerminalElement

  /// <seealso cref="Terminal.Gui.TextField"/>
  static member textField(set: textFieldProps -> unit) =
    let viewProps = textFieldProps ()
    set viewProps
    TextFieldElement(viewProps.props) :> ITerminalElement

  static member textField(children: ITerminalElement list) =
    let viewProps = textFieldProps ()
    viewProps.children children
    TextFieldElement(viewProps.props) :> ITerminalElement

  static member textField(x: int, y: int, title: string) =
    let setProps =
      fun (p: textFieldProps) ->
        p.x (Pos.Absolute(x))
        p.y (Pos.Absolute(y))
        p.title title

    let viewProps = textFieldProps ()
    setProps viewProps
    TextFieldElement(viewProps.props) :> ITerminalElement

  /// <seealso cref="Terminal.Gui.TextValidateField"/>
  static member textValidateField(set: textValidateFieldProps -> unit) =
    let viewProps = textValidateFieldProps ()
    set viewProps
    TextValidateFieldElement(viewProps.props) :> ITerminalElement

  static member textValidateField(children: ITerminalElement list) =
    let viewProps = textValidateFieldProps ()
    viewProps.children children
    TextValidateFieldElement(viewProps.props) :> ITerminalElement

  static member textValidateField(x: int, y: int, title: string) =
    let setProps =
      fun (p: textValidateFieldProps) ->
        p.x (Pos.Absolute(x))
        p.y (Pos.Absolute(y))
        p.title title

    let viewProps = textValidateFieldProps ()
    setProps viewProps
    TextValidateFieldElement(viewProps.props) :> ITerminalElement

  /// <seealso cref="Terminal.Gui.TextView"/>
  static member textView(set: textViewProps -> unit) =
    let viewProps = textViewProps ()
    set viewProps
    TextViewElement(viewProps.props) :> ITerminalElement

  static member textView(children: ITerminalElement list) =
    let viewProps = textViewProps ()
    viewProps.children children
    TextViewElement(viewProps.props) :> ITerminalElement

  static member textView(x: int, y: int, title: string) =
    let setProps =
      fun (p: textViewProps) ->
        p.x (Pos.Absolute(x))
        p.y (Pos.Absolute(y))
        p.title title

    let viewProps = textViewProps ()
    setProps viewProps
    TextViewElement(viewProps.props) :> ITerminalElement

  /// <seealso cref="Terminal.Gui.TimeField"/>
  static member timeField(set: timeFieldProps -> unit) =
    let viewProps = timeFieldProps ()
    set viewProps
    TimeFieldElement(viewProps.props) :> ITerminalElement

  static member timeField(children: ITerminalElement list) =
    let viewProps = timeFieldProps ()
    viewProps.children children
    TimeFieldElement(viewProps.props) :> ITerminalElement

  static member timeField(x: int, y: int, title: string) =
    let setProps =
      fun (p: timeFieldProps) ->
        p.x (Pos.Absolute(x))
        p.y (Pos.Absolute(y))
        p.title title

    let viewProps = timeFieldProps ()
    setProps viewProps
    TimeFieldElement(viewProps.props) :> ITerminalElement

  /// <seealso cref="Terminal.Gui.ViewBase.Runnable"/>
  static member runnable(set: runnableProps -> unit) =
    let viewProps = runnableProps ()
    set viewProps
    RunnableElement(viewProps.props) :> ITerminalElement

  static member runnable(children: ITerminalElement list) =
    let viewProps = runnableProps ()
    viewProps.children children
    RunnableElement(viewProps.props) :> ITerminalElement

  static member runnable(x: int, y: int, title: string) =
    let setProps =
      fun (p: runnableProps) ->
        p.x (Pos.Absolute(x))
        p.y (Pos.Absolute(y))
        p.title title

    let viewProps = runnableProps ()
    setProps viewProps
    RunnableElement(viewProps.props) :> ITerminalElement

  /// <seealso cref="Terminal.Gui.TreeView"/>
  static member treeView(set: treeViewProps -> unit) =
    let viewProps = treeViewProps ()
    set viewProps
    TreeViewElement(viewProps.props) :> ITerminalElement

  static member treeView(children: ITerminalElement list) =
    let viewProps = treeViewProps ()
    viewProps.children children
    TreeViewElement(viewProps.props) :> ITerminalElement

  static member treeView(x: int, y: int, title: string) =
    let setProps =
      fun (p: treeViewProps<_>) ->
        p.x (Pos.Absolute(x))
        p.y (Pos.Absolute(y))
        p.title title

    let viewProps = treeViewProps ()
    setProps viewProps
    TreeViewElement(viewProps.props) :> ITerminalElement

  /// <seealso cref="Terminal.Gui.TreeView"/>
  static member treeView<'a when 'a: not struct>(set: treeViewProps<'a> -> unit) =
    let viewProps = treeViewProps<'a> ()
    set viewProps
    TreeViewElement<'a>(viewProps.props) :> ITreeViewElement

  static member treeView<'a when 'a: not struct>(children: ITerminalElement list) =
    let viewProps = treeViewProps<'a> ()
    viewProps.children children
    TreeViewElement<'a>(viewProps.props) :> ITreeViewElement

  static member treeView<'a when 'a: not struct>(x: int, y: int, title: string) =
    let setProps =
      fun (p: treeViewProps<'a>) ->
        p.x (Pos.Absolute(x))
        p.y (Pos.Absolute(y))
        p.title title

    let viewProps = treeViewProps<'a> ()
    setProps viewProps
    TreeViewElement<'a>(viewProps.props) :> ITreeViewElement

  /// <seealso cref="Terminal.Gui.Window"/>
  static member window(set: windowProps -> unit) =
    let viewProps = windowProps ()
    set viewProps
    WindowElement(viewProps.props) :> ITerminalElement

  static member window(children: ITerminalElement list) =
    let viewProps = windowProps ()
    viewProps.children children
    WindowElement(viewProps.props) :> ITerminalElement

  static member window(x: int, y: int, title: string) =
    let setProps =
      fun (p: windowProps) ->
        p.x (Pos.Absolute(x))
        p.y (Pos.Absolute(y))
        p.title title

    let viewProps = windowProps ()
    setProps viewProps
    WindowElement(viewProps.props) :> ITerminalElement

  /// <seealso cref="Terminal.Gui.Wizard"/>
  static member wizard(set: wizardProps -> unit) =
    let viewProps = wizardProps ()
    set viewProps
    WizardElement(viewProps.props) :> ITerminalElement

  static member wizard(children: ITerminalElement list) =
    let viewProps = wizardProps ()
    viewProps.children children
    WizardElement(viewProps.props) :> ITerminalElement

  static member wizard(x: int, y: int, title: string) =
    let setProps =
      fun (p: wizardProps) ->
        p.x (Pos.Absolute(x))
        p.y (Pos.Absolute(y))
        p.title title

    let viewProps = wizardProps ()
    setProps viewProps
    WizardElement(viewProps.props) :> ITerminalElement

  /// <seealso cref="Terminal.Gui.WizardStep"/>
  static member wizardStep(set: wizardStepProps -> unit) =
    let viewProps = wizardStepProps ()
    set viewProps
    WizardStepElement(viewProps.props) :> ITerminalElement

  static member wizardStep(children: ITerminalElement list) =
    let viewProps = wizardStepProps ()
    viewProps.children children
    WizardStepElement(viewProps.props) :> ITerminalElement

  static member wizardStep(x: int, y: int, title: string) =
    let setProps =
      fun (p: wizardStepProps) ->
        p.x (Pos.Absolute(x))
        p.y (Pos.Absolute(y))
        p.title title

    let viewProps = wizardStepProps ()
    setProps viewProps
    WizardStepElement(viewProps.props) :> ITerminalElement

module Dialogs =
  let showWizard (wizard: Wizard) =
    // TODO: this component should be tested
    // TODO: probably here and elmish component could be used
    // ApplicationImpl.Instance.Run(wizard) |> ignore
    wizard.Dispose()
    ()


  let openFileDialog title =
    use dia = new OpenDialog(Title = title)
    // TODO: this component should be tested
    // TODO: probably here and elmish component could be used
    // ApplicationImpl.Instance.Run(dia) |> ignore
    dia.Dispose()
    //Application.Top.Remove(dia) |> ignore
    if dia.Canceled then
      None
    else
      let file =
        dia.FilePaths
        |> Seq.tryHead
        |> Option.bind (fun s ->
          if String.IsNullOrEmpty(s) then
            None
          else
            Some s
        )

      file

