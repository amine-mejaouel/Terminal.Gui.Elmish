(*
#######################################
#            View.fs                  #
#######################################
*)


namespace Terminal.Gui.Elmish

open System
open Terminal.Gui.App
open Terminal.Gui.Elmish
open Terminal.Gui.Elmish.Elements
open Terminal.Gui.ViewBase
open Terminal.Gui.Views

type View =

  /// <seealso cref="Terminal.Gui.Adornment"/>
  static member adornment(set: adornmentProps -> unit) =
    let view = adornmentProps ()
    set view
    AdornmentElement(view.props) :> ITerminalElement

  static member adornment(children: ITerminalElement list) =
    let view = adornmentProps ()
    view.children children
    AdornmentElement(view.props) :> ITerminalElement

  static member adornment(x: int, y: int, title: string) =
    let setProps =
      fun (p: adornmentProps) ->
        p.x (Pos.Absolute(x))
        p.y (Pos.Absolute(y))
        p.title title

    let view = adornmentProps ()
    setProps view
    AdornmentElement(view.props) :> ITerminalElement

  /// <seealso cref="Terminal.Gui.Bar"/>
  static member bar(set: barProps -> unit) =
    let view = barProps ()
    set view
    BarElement(view.props) :> ITerminalElement

  static member bar(children: ITerminalElement list) =
    let view = barProps ()
    view.children children
    BarElement(view.props) :> ITerminalElement

  static member bar(x: int, y: int, title: string) =
    let setProps =
      fun (p: barProps) ->
        p.x (Pos.Absolute(x))
        p.y (Pos.Absolute(y))
        p.title title

    let view = barProps ()
    setProps view
    BarElement(view.props) :> ITerminalElement

  /// <seealso cref="Terminal.Gui.Border"/>
  static member border(set: borderProps -> unit) =
    let view = borderProps ()
    set view
    BorderElement(view.props) :> ITerminalElement

  static member border(children: ITerminalElement list) =
    let view = borderProps ()
    view.children children
    BorderElement(view.props) :> ITerminalElement

  static member border(x: int, y: int, title: string) =
    let setProps =
      fun (p: borderProps) ->
        p.x (Pos.Absolute(x))
        p.y (Pos.Absolute(y))
        p.title title

    let view = borderProps ()
    setProps view
    BorderElement(view.props) :> ITerminalElement

  /// <seealso cref="Terminal.Gui.Button"/>
  static member button(set: buttonProps -> unit) =
    let view = buttonProps ()
    set view
    ButtonElement(view.props) :> ITerminalElement

  static member button(children: ITerminalElement list) =
    let view = buttonProps ()
    view.children children
    ButtonElement(view.props) :> ITerminalElement

  static member button(x: int, y: int, title: string) =
    let setProps =
      fun (p: buttonProps) ->
        p.x (Pos.Absolute(x))
        p.y (Pos.Absolute(y))
        p.title title

    let view = buttonProps ()
    setProps view
    ButtonElement(view.props) :> ITerminalElement

  /// <seealso cref="Terminal.Gui.CheckBox"/>
  static member checkBox(set: checkBoxProps -> unit) =
    let view = checkBoxProps ()
    set view
    CheckBoxElement(view.props) :> ITerminalElement

  static member checkBox(children: ITerminalElement list) =
    let view = checkBoxProps ()
    view.children children
    CheckBoxElement(view.props) :> ITerminalElement

  static member checkBox(x: int, y: int, title: string) =
    let setProps =
      fun (p: checkBoxProps) ->
        p.x (Pos.Absolute(x))
        p.y (Pos.Absolute(y))
        p.title title

    let view = checkBoxProps ()
    setProps view
    CheckBoxElement(view.props) :> ITerminalElement

  /// <seealso cref="Terminal.Gui.ColorPicker"/>
  static member colorPicker(set: colorPickerProps -> unit) =
    let view = colorPickerProps ()
    set view
    ColorPickerElement(view.props) :> ITerminalElement

  static member colorPicker(children: ITerminalElement list) =
    let view = colorPickerProps ()
    view.children children
    ColorPickerElement(view.props) :> ITerminalElement

  static member colorPicker(x: int, y: int, title: string) =
    let setProps =
      fun (p: colorPickerProps) ->
        p.x (Pos.Absolute(x))
        p.y (Pos.Absolute(y))
        p.title title

    let view = colorPickerProps ()
    setProps view
    ColorPickerElement(view.props) :> ITerminalElement

  /// <seealso cref="Terminal.Gui.ColorPicker16"/>
  static member colorPicker16(set: colorPicker16Props -> unit) =
    let view = colorPicker16Props ()
    set view
    ColorPicker16Element(view.props) :> ITerminalElement

  static member colorPicker16(children: ITerminalElement list) =
    let view = colorPicker16Props ()
    view.children children
    ColorPicker16Element(view.props) :> ITerminalElement

  static member colorPicker16(x: int, y: int, title: string) =
    let setProps =
      fun (p: colorPicker16Props) ->
        p.x (Pos.Absolute(x))
        p.y (Pos.Absolute(y))
        p.title title

    let view = colorPicker16Props ()
    setProps view
    ColorPicker16Element(view.props) :> ITerminalElement

  /// <seealso cref="Terminal.Gui.ComboBox"/>
  static member comboBox(set: comboBoxProps -> unit) =
    let view = comboBoxProps ()
    set view
    ComboBoxElement(view.props) :> ITerminalElement

  static member comboBox(children: ITerminalElement list) =
    let view = comboBoxProps ()
    view.children children
    ComboBoxElement(view.props) :> ITerminalElement

  static member comboBox(x: int, y: int, title: string) =
    let setProps =
      fun (p: comboBoxProps) ->
        p.x (Pos.Absolute(x))
        p.y (Pos.Absolute(y))
        p.title title

    let view = comboBoxProps ()
    setProps view
    ComboBoxElement(view.props) :> ITerminalElement

  /// <seealso cref="Terminal.Gui.DateField"/>
  static member dateField(set: dateFieldProps -> unit) =
    let view = dateFieldProps ()
    set view
    DateFieldElement(view.props) :> ITerminalElement

  static member dateField(children: ITerminalElement list) =
    let view = dateFieldProps ()
    view.children children
    DateFieldElement(view.props) :> ITerminalElement

  static member dateField(x: int, y: int, title: string) =
    let setProps =
      fun (p: dateFieldProps) ->
        p.x (Pos.Absolute(x))
        p.y (Pos.Absolute(y))
        p.title title

    let view = dateFieldProps ()
    setProps view
    DateFieldElement(view.props) :> ITerminalElement

  /// <seealso cref="Terminal.Gui.DatePicker"/>
  static member datePicker(set: datePickerProps -> unit) =
    let view = datePickerProps ()
    set view
    DatePickerElement(view.props) :> ITerminalElement

  static member datePicker(children: ITerminalElement list) =
    let view = datePickerProps ()
    view.children children
    DatePickerElement(view.props) :> ITerminalElement

  static member datePicker(x: int, y: int, title: string) =
    let setProps =
      fun (p: datePickerProps) ->
        p.x (Pos.Absolute(x))
        p.y (Pos.Absolute(y))
        p.title title

    let view = datePickerProps ()
    setProps view
    DatePickerElement(view.props) :> ITerminalElement

  /// <seealso cref="Terminal.Gui.Dialog"/>
  static member dialog(set: dialogProps -> unit) =
    let view = dialogProps ()
    set view
    DialogElement(view.props) :> ITerminalElement

  static member dialog(children: ITerminalElement list) =
    let view = dialogProps ()
    view.children children
    DialogElement(view.props) :> ITerminalElement

  static member dialog(x: int, y: int, title: string) =
    let setProps =
      fun (p: dialogProps) ->
        p.x (Pos.Absolute(x))
        p.y (Pos.Absolute(y))
        p.title title

    let view = dialogProps ()
    setProps view
    DialogElement(view.props) :> ITerminalElement

  /// <seealso cref="Terminal.Gui.FileDialog"/>
  static member fileDialog(set: fileDialogProps -> unit) =
    let view = fileDialogProps ()
    set view
    FileDialogElement(view.props) :> ITerminalElement

  static member fileDialog(children: ITerminalElement list) =
    let view = fileDialogProps ()
    view.children children
    FileDialogElement(view.props) :> ITerminalElement

  static member fileDialog(x: int, y: int, title: string) =
    let setProps =
      fun (p: fileDialogProps) ->
        p.x (Pos.Absolute(x))
        p.y (Pos.Absolute(y))
        p.title title

    let view = fileDialogProps ()
    setProps view
    FileDialogElement(view.props) :> ITerminalElement

  /// <seealso cref="Terminal.Gui.FrameView"/>
  static member frameView(set: frameViewProps -> unit) =
    let view = frameViewProps ()
    set view
    FrameViewElement(view.props) :> ITerminalElement

  static member frameView(children: ITerminalElement list) =
    let view = frameViewProps ()
    view.children children
    FrameViewElement(view.props) :> ITerminalElement

  static member frameView(x: int, y: int, title: string) =
    let setProps =
      fun (p: frameViewProps) ->
        p.x (Pos.Absolute(x))
        p.y (Pos.Absolute(y))
        p.title title

    let view = frameViewProps ()
    setProps view
    FrameViewElement(view.props) :> ITerminalElement

  /// <seealso cref="Terminal.Gui.GraphView"/>
  static member graphView(set: graphViewProps -> unit) =
    let view = graphViewProps ()
    set view
    GraphViewElement(view.props) :> ITerminalElement

  static member graphView(children: ITerminalElement list) =
    let view = graphViewProps ()
    view.children children
    GraphViewElement(view.props) :> ITerminalElement

  static member graphView(x: int, y: int, title: string) =
    let setProps =
      fun (p: graphViewProps) ->
        p.x (Pos.Absolute(x))
        p.y (Pos.Absolute(y))
        p.title title

    let view = graphViewProps ()
    setProps view
    GraphViewElement(view.props) :> ITerminalElement

  /// <seealso cref="Terminal.Gui.HexView"/>
  static member hexView(set: hexViewProps -> unit) =
    let view = hexViewProps ()
    set view
    HexViewElement(view.props) :> ITerminalElement

  static member hexView(children: ITerminalElement list) =
    let view = hexViewProps ()
    view.children children
    HexViewElement(view.props) :> ITerminalElement

  static member hexView(x: int, y: int, title: string) =
    let setProps =
      fun (p: hexViewProps) ->
        p.x (Pos.Absolute(x))
        p.y (Pos.Absolute(y))
        p.title title

    let view = hexViewProps ()
    setProps view
    HexViewElement(view.props) :> ITerminalElement

  /// <seealso cref="Terminal.Gui.Label"/>
  static member label(set: labelProps -> unit) =
    let view = labelProps ()
    set view
    LabelElement(view.props) :> ITerminalElement

  static member label(children: ITerminalElement list) =
    let view = labelProps ()
    view.children children
    LabelElement(view.props) :> ITerminalElement

  static member label(x: int, y: int, title: string) =
    let setProps =
      fun (p: labelProps) ->
        p.x (Pos.Absolute(x))
        p.y (Pos.Absolute(y))
        p.title title

    let view = labelProps ()
    setProps view
    LabelElement(view.props) :> ITerminalElement

  /// <seealso cref="Terminal.Gui.LegendAnnotation"/>
  static member legendAnnotation(set: legendAnnotationProps -> unit) =
    let view = legendAnnotationProps ()
    set view
    LegendAnnotationElement(view.props) :> ITerminalElement

  static member legendAnnotation(children: ITerminalElement list) =
    let view = legendAnnotationProps ()
    view.children children
    LegendAnnotationElement(view.props) :> ITerminalElement

  static member legendAnnotation(x: int, y: int, title: string) =
    let setProps =
      fun (p: legendAnnotationProps) ->
        p.x (Pos.Absolute(x))
        p.y (Pos.Absolute(y))
        p.title title

    let view = legendAnnotationProps ()
    setProps view
    LegendAnnotationElement(view.props) :> ITerminalElement

  /// <seealso cref="Terminal.Gui.Line"/>
  static member line(set: lineProps -> unit) =
    let view = lineProps ()
    set view
    LineElement(view.props) :> ITerminalElement

  static member line(children: ITerminalElement list) =
    let view = lineProps ()
    view.children children
    LineElement(view.props) :> ITerminalElement

  static member line(x: int, y: int, title: string) =
    let setProps =
      fun (p: lineProps) ->
        p.x (Pos.Absolute(x))
        p.y (Pos.Absolute(y))
        p.title title

    let view = lineProps ()
    setProps view
    LineElement(view.props) :> ITerminalElement

  /// <seealso cref="Terminal.Gui.ListView"/>
  static member listView(set: listViewProps -> unit) =
    let view = listViewProps ()
    set view
    ListViewElement(view.props) :> ITerminalElement

  static member listView(children: ITerminalElement list) =
    let view = listViewProps ()
    view.children children
    ListViewElement(view.props) :> ITerminalElement

  static member listView(x: int, y: int, title: string) =
    let setProps =
      fun (p: listViewProps) ->
        p.x (Pos.Absolute(x))
        p.y (Pos.Absolute(y))
        p.title title

    let view = listViewProps ()
    setProps view
    ListViewElement(view.props) :> ITerminalElement

  /// <seealso cref="Terminal.Gui.Margin"/>
  static member margin(set: marginProps -> unit) =
    let view = marginProps ()
    set view
    MarginElement(view.props) :> ITerminalElement

  static member margin(children: ITerminalElement list) =
    let view = marginProps ()
    view.children children
    MarginElement(view.props) :> ITerminalElement

  static member margin(x: int, y: int, title: string) =
    let setProps =
      fun (p: marginProps) ->
        p.x (Pos.Absolute(x))
        p.y (Pos.Absolute(y))
        p.title title

    let view = marginProps ()
    setProps view
    MarginElement(view.props) :> ITerminalElement

  /// <seealso cref="Terminal.Gui.MenuBar"/>
  static member menuBar(set: menuBarProps -> menuBarMacros -> unit) =
    let props = menuBarProps ()
    let macros = menuBarMacros props
    set props macros
    MenuBarElement(props.props) :> ITerminalElement

  static member menuBar(children: ITerminalElement list) =
    let view = menuBarProps ()
    view.children children
    MenuBarElement(view.props) :> ITerminalElement

  static member menuBar(x: int, y: int, title: string) =
    let setProps =
      fun (p: menuBarProps) ->
        p.x (Pos.Absolute(x))
        p.y (Pos.Absolute(y))
        p.title title

    let view = menuBarProps ()
    setProps view
    MenuBarElement(view.props) :> ITerminalElement

  /// <seealso cref="Terminal.Gui.Menu"/>
  static member menu(set: menuProps -> unit) =
    let view = menuProps ()
    set view
    MenuElement(view.props) :> IMenuElement

  static member menu(children: ITerminalElement list) =
    let view = menuProps ()
    view.children children
    MenuElement(view.props) :> ITerminalElement

  static member menu(x: int, y: int, title: string) =
    let setProps =
      fun (p: menuProps) ->
        p.x (Pos.Absolute(x))
        p.y (Pos.Absolute(y))
        p.title title

    let view = menuProps ()
    setProps view
    MenuElement(view.props) :> ITerminalElement

  static member menuBarItem(set: menuBarItemProps -> unit) =
    let view = menuBarItemProps ()
    set view
    MenuBarItemElement(view.props) :> IMenuBarItemElement

  static member popoverMenu(set: popoverMenuProps -> unit) =
    let view = popoverMenuProps ()
    set view
    PopoverMenuElement(view.props) :> IPopoverMenuElement

  static member menuItem(set: menuItemProps -> unit) =
    let view = menuItemProps ()
    set view
    MenuItemElement(view.props) :> ITerminalElement

  /// <seealso cref="Terminal.Gui.NumericUpDown"/>
  static member numericUpDown(set: numericUpDownProps -> unit) =
    let view = numericUpDownProps ()
    set view
    NumericUpDownElement(view.props) :> ITerminalElement

  static member numericUpDown(children: ITerminalElement list) =
    let view = numericUpDownProps ()
    view.children children
    NumericUpDownElement(view.props) :> ITerminalElement

  static member numericUpDown(x: int, y: int, title: string) =
    let setProps =
      fun (p: numericUpDownProps) ->
        p.x (Pos.Absolute(x))
        p.y (Pos.Absolute(y))
        p.title title

    let view = numericUpDownProps ()
    setProps view
    NumericUpDownElement(view.props) :> ITerminalElement

  /// <seealso cref="Terminal.Gui.NumericUpDown"/>
  static member numericUpDown<'a>(set: numericUpDownProps<'a> -> unit) =
    let view = numericUpDownProps<'a> ()
    set view
    NumericUpDownElement<'a>(view.props) :> INumericUpDownElement

  static member numericUpDown<'a>(children: ITerminalElement list) =
    let view = numericUpDownProps<'a> ()
    view.children children
    NumericUpDownElement<'a>(view.props) :> INumericUpDownElement

  static member numericUpDown<'a>(x: int, y: int, title: string) =
    let setProps =
      fun (p: numericUpDownProps<'a>) ->
        p.x (Pos.Absolute(x))
        p.y (Pos.Absolute(y))
        p.title title

    let view = numericUpDownProps<'a> ()
    setProps view
    NumericUpDownElement<'a>(view.props) :> INumericUpDownElement

  /// <seealso cref="Terminal.Gui.OpenDialog"/>
  static member openDialog(set: openDialogProps -> unit) =
    let view = openDialogProps ()
    set view
    OpenDialogElement(view.props) :> ITerminalElement

  static member openDialog(children: ITerminalElement list) =
    let view = openDialogProps ()
    view.children children
    OpenDialogElement(view.props) :> ITerminalElement

  static member openDialog(x: int, y: int, title: string) =
    let setProps =
      fun (p: openDialogProps) ->
        p.x (Pos.Absolute(x))
        p.y (Pos.Absolute(y))
        p.title title

    let view = openDialogProps ()
    setProps view
    OpenDialogElement(view.props) :> ITerminalElement

  static member optionSelector(set: optionSelectorProps -> unit) =
    // TODO: rename view -> props everywhere in the file
    let view = optionSelectorProps ()
    set view
    OptionSelectorElement(view.props) :> ITerminalElement

  static member flagSelector(set: flagSelectorProps -> unit) =
    let view = flagSelectorProps ()
    set view
    FlagSelectorElement(view.props) :> ITerminalElement

  /// <seealso cref="Terminal.Gui.Padding"/>
  static member padding(set: paddingProps -> unit) =
    let view = paddingProps ()
    set view
    PaddingElement(view.props) :> ITerminalElement

  static member padding(children: ITerminalElement list) =
    let view = paddingProps ()
    view.children children
    PaddingElement(view.props) :> ITerminalElement

  static member padding(x: int, y: int, title: string) =
    let setProps =
      fun (p: paddingProps) ->
        p.x (Pos.Absolute(x))
        p.y (Pos.Absolute(y))
        p.title title

    let view = paddingProps ()
    setProps view
    PaddingElement(view.props) :> ITerminalElement

  /// <seealso cref="Terminal.Gui.ProgressBar"/>
  static member progressBar(set: progressBarProps -> unit) =
    let view = progressBarProps ()
    set view
    ProgressBarElement(view.props) :> ITerminalElement

  static member progressBar(children: ITerminalElement list) =
    let view = progressBarProps ()
    view.children children
    ProgressBarElement(view.props) :> ITerminalElement

  static member progressBar(x: int, y: int, title: string) =
    let setProps =
      fun (p: progressBarProps) ->
        p.x (Pos.Absolute(x))
        p.y (Pos.Absolute(y))
        p.title title

    let view = progressBarProps ()
    setProps view
    ProgressBarElement(view.props) :> ITerminalElement

  /// <seealso cref="Terminal.Gui.SaveDialog"/>
  static member saveDialog(set: saveDialogProps -> unit) =
    let view = saveDialogProps ()
    set view
    SaveDialogElement(view.props) :> ITerminalElement

  static member saveDialog(children: ITerminalElement list) =
    let view = saveDialogProps ()
    view.children children
    SaveDialogElement(view.props) :> ITerminalElement

  static member saveDialog(x: int, y: int, title: string) =
    let setProps =
      fun (p: saveDialogProps) ->
        p.x (Pos.Absolute(x))
        p.y (Pos.Absolute(y))
        p.title title

    let view = saveDialogProps ()
    setProps view
    SaveDialogElement(view.props) :> ITerminalElement

  /// <seealso cref="Terminal.Gui.ScrollBar"/>
  static member scrollBar(set: scrollBarProps -> unit) =
    let view = scrollBarProps ()
    set view
    ScrollBarElement(view.props) :> ITerminalElement

  static member scrollBar(children: ITerminalElement list) =
    let view = scrollBarProps ()
    view.children children
    ScrollBarElement(view.props) :> ITerminalElement

  static member scrollBar(x: int, y: int, title: string) =
    let setProps =
      fun (p: scrollBarProps) ->
        p.x (Pos.Absolute(x))
        p.y (Pos.Absolute(y))
        p.title title

    let view = scrollBarProps ()
    setProps view
    ScrollBarElement(view.props) :> ITerminalElement

  /// <seealso cref="Terminal.Gui.ScrollSlider"/>
  static member scrollSlider(set: scrollSliderProps -> unit) =
    let view = scrollSliderProps ()
    set view
    ScrollSliderElement(view.props) :> ITerminalElement

  static member scrollSlider(children: ITerminalElement list) =
    let view = scrollSliderProps ()
    view.children children
    ScrollSliderElement(view.props) :> ITerminalElement

  static member scrollSlider(x: int, y: int, title: string) =
    let setProps =
      fun (p: scrollSliderProps) ->
        p.x (Pos.Absolute(x))
        p.y (Pos.Absolute(y))
        p.title title

    let view = scrollSliderProps ()
    setProps view
    ScrollSliderElement(view.props) :> ITerminalElement

  /// <seealso cref="Terminal.Gui.Shortcut"/>
  static member shortcut(set: shortcutProps -> unit) =
    let view = shortcutProps ()
    set view
    ShortcutElement(view.props) :> ITerminalElement

  static member shortcut(children: ITerminalElement list) =
    let view = shortcutProps ()
    view.children children
    ShortcutElement(view.props) :> ITerminalElement

  static member shortcut(x: int, y: int, title: string) =
    let setProps =
      fun (p: shortcutProps) ->
        p.x (Pos.Absolute(x))
        p.y (Pos.Absolute(y))
        p.title title

    let view = shortcutProps ()
    setProps view
    ShortcutElement(view.props) :> ITerminalElement

  /// <seealso cref="Terminal.Gui.Slider"/>
  static member slider(set: sliderProps -> unit) =
    let view = sliderProps ()
    set view
    SliderElement(view.props) :> ITerminalElement

  static member slider(children: ITerminalElement list) =
    let view = sliderProps ()
    view.children children
    SliderElement(view.props) :> ITerminalElement

  static member slider(x: int, y: int, title: string) =
    let setProps =
      fun (p: sliderProps) ->
        p.x (Pos.Absolute(x))
        p.y (Pos.Absolute(y))
        p.title title

    let view = sliderProps ()
    setProps view
    SliderElement(view.props) :> ITerminalElement

  /// <seealso cref="Terminal.Gui.Slider"/>
  static member slider<'a>(set: sliderProps<'a> -> unit) =
    let view = sliderProps<'a> ()

    set view
    SliderElement<'a>(view.props)
    : ISliderElement

  static member slider<'a>(children: ITerminalElement list) =
    let view = sliderProps<'a> ()

    view.children children
    SliderElement<'a>(view.props)
    : ISliderElement

  static member slider<'a>(x: int, y: int, title: string) =
    let setProps =
      fun (p: sliderProps<'a>) ->
        p.x (Pos.Absolute(x))
        p.y (Pos.Absolute(y))
        p.title title

    let view = sliderProps<'a> ()

    setProps view
    SliderElement<'a>(view.props)
    : ISliderElement

  /// <seealso cref="Terminal.Gui.SpinnerView"/>
  static member spinnerView(set: spinnerViewProps -> unit) =
    let view = spinnerViewProps ()
    set view
    SpinnerViewElement(view.props) :> ITerminalElement

  static member spinnerView(children: ITerminalElement list) =
    let view = spinnerViewProps ()
    view.children children
    SpinnerViewElement(view.props) :> ITerminalElement

  static member spinnerView(x: int, y: int, title: string) =
    let setProps =
      fun (p: spinnerViewProps) ->
        p.x (Pos.Absolute(x))
        p.y (Pos.Absolute(y))
        p.title title

    let view = spinnerViewProps ()
    setProps view
    SpinnerViewElement(view.props) :> ITerminalElement

  /// <seealso cref="Terminal.Gui.StatusBar"/>
  static member statusBar(set: statusBarProps -> unit) =
    let view = statusBarProps ()
    set view
    StatusBarElement(view.props) :> ITerminalElement

  static member statusBar(children: ITerminalElement list) =
    let view = statusBarProps ()
    view.children children
    StatusBarElement(view.props) :> ITerminalElement

  static member statusBar(x: int, y: int, title: string) =
    let setProps =
      fun (p: statusBarProps) ->
        p.x (Pos.Absolute(x))
        p.y (Pos.Absolute(y))
        p.title title

    let view = statusBarProps ()
    setProps view
    StatusBarElement(view.props) :> ITerminalElement

  /// <seealso cref="Terminal.Gui.Tab"/>
  static member tab(set: tabProps -> unit) =
    let view = tabProps ()
    set view
    TabElement(view.props) :> ITerminalElement

  static member tab(children: ITerminalElement list) =
    let view = tabProps ()
    view.children children
    TabElement(view.props) :> ITerminalElement

  static member tab(x: int, y: int, title: string) =
    let setProps =
      fun (p: tabProps) ->
        p.x (Pos.Absolute(x))
        p.y (Pos.Absolute(y))
        p.title title

    let view = tabProps ()
    setProps view
    TabElement(view.props) :> ITerminalElement

  /// <seealso cref="Terminal.Gui.TabView"/>
  static member tabView(set: tabViewProps -> unit) =
    let view = tabViewProps ()
    set view
    TabViewElement(view.props) :> ITerminalElement

  static member tabView(children: ITerminalElement list) =
    let view = tabViewProps ()
    view.children children
    TabViewElement(view.props) :> ITerminalElement

  static member tabView(x: int, y: int, title: string) =
    let setProps =
      fun (p: tabViewProps) ->
        p.x (Pos.Absolute(x))
        p.y (Pos.Absolute(y))
        p.title title

    let view = tabViewProps ()
    setProps view
    TabViewElement(view.props) :> ITerminalElement

  /// <seealso cref="Terminal.Gui.TableView"/>
  static member tableView(set: tableViewProps -> unit) =
    let view = tableViewProps ()
    set view
    TableViewElement(view.props) :> ITerminalElement

  static member tableView(children: ITerminalElement list) =
    let view = tableViewProps ()
    view.children children
    TableViewElement(view.props) :> ITerminalElement

  static member tableView(x: int, y: int, title: string) =
    let setProps =
      fun (p: tableViewProps) ->
        p.x (Pos.Absolute(x))
        p.y (Pos.Absolute(y))
        p.title title

    let view = tableViewProps ()
    setProps view
    TableViewElement(view.props) :> ITerminalElement

  /// <seealso cref="Terminal.Gui.TextField"/>
  static member textField(set: textFieldProps -> unit) =
    let view = textFieldProps ()
    set view
    TextFieldElement(view.props) :> ITerminalElement

  static member textField(children: ITerminalElement list) =
    let view = textFieldProps ()
    view.children children
    TextFieldElement(view.props) :> ITerminalElement

  static member textField(x: int, y: int, title: string) =
    let setProps =
      fun (p: textFieldProps) ->
        p.x (Pos.Absolute(x))
        p.y (Pos.Absolute(y))
        p.title title

    let view = textFieldProps ()
    setProps view
    TextFieldElement(view.props) :> ITerminalElement

  /// <seealso cref="Terminal.Gui.TextValidateField"/>
  static member textValidateField(set: textValidateFieldProps -> unit) =
    let view = textValidateFieldProps ()
    set view
    TextValidateFieldElement(view.props) :> ITerminalElement

  static member textValidateField(children: ITerminalElement list) =
    let view = textValidateFieldProps ()
    view.children children
    TextValidateFieldElement(view.props) :> ITerminalElement

  static member textValidateField(x: int, y: int, title: string) =
    let setProps =
      fun (p: textValidateFieldProps) ->
        p.x (Pos.Absolute(x))
        p.y (Pos.Absolute(y))
        p.title title

    let view = textValidateFieldProps ()
    setProps view
    TextValidateFieldElement(view.props) :> ITerminalElement

  /// <seealso cref="Terminal.Gui.TextView"/>
  static member textView(set: textViewProps -> unit) =
    let view = textViewProps ()
    set view
    TextViewElement(view.props) :> ITerminalElement

  static member textView(children: ITerminalElement list) =
    let view = textViewProps ()
    view.children children
    TextViewElement(view.props) :> ITerminalElement

  static member textView(x: int, y: int, title: string) =
    let setProps =
      fun (p: textViewProps) ->
        p.x (Pos.Absolute(x))
        p.y (Pos.Absolute(y))
        p.title title

    let view = textViewProps ()
    setProps view
    TextViewElement(view.props) :> ITerminalElement

  /// <seealso cref="Terminal.Gui.TimeField"/>
  static member timeField(set: timeFieldProps -> unit) =
    let view = timeFieldProps ()
    set view
    TimeFieldElement(view.props) :> ITerminalElement

  static member timeField(children: ITerminalElement list) =
    let view = timeFieldProps ()
    view.children children
    TimeFieldElement(view.props) :> ITerminalElement

  static member timeField(x: int, y: int, title: string) =
    let setProps =
      fun (p: timeFieldProps) ->
        p.x (Pos.Absolute(x))
        p.y (Pos.Absolute(y))
        p.title title

    let view = timeFieldProps ()
    setProps view
    TimeFieldElement(view.props) :> ITerminalElement

  /// <seealso cref="Terminal.Gui.ViewBase.Runnable"/>
  static member runnable(set: runnableProps -> unit) =
    let view = runnableProps ()
    set view
    RunnableElement(view.props) :> ITerminalElement

  static member runnable(children: ITerminalElement list) =
    let view = runnableProps ()
    view.children children
    RunnableElement(view.props) :> ITerminalElement

  static member runnable(x: int, y: int, title: string) =
    let setProps =
      fun (p: runnableProps) ->
        p.x (Pos.Absolute(x))
        p.y (Pos.Absolute(y))
        p.title title

    let view = runnableProps ()
    setProps view
    RunnableElement(view.props) :> ITerminalElement

  /// <seealso cref="Terminal.Gui.TreeView"/>
  static member treeView(set: treeViewProps -> unit) =
    let view = treeViewProps ()
    set view
    TreeViewElement(view.props) :> ITerminalElement

  static member treeView(children: ITerminalElement list) =
    let view = treeViewProps ()
    view.children children
    TreeViewElement(view.props) :> ITerminalElement

  static member treeView(x: int, y: int, title: string) =
    let setProps =
      fun (p: treeViewProps<_>) ->
        p.x (Pos.Absolute(x))
        p.y (Pos.Absolute(y))
        p.title title

    let view = treeViewProps ()
    setProps view
    TreeViewElement(view.props) :> ITerminalElement

  /// <seealso cref="Terminal.Gui.TreeView"/>
  static member treeView<'a when 'a: not struct>(set: treeViewProps<'a> -> unit) =
    let view = treeViewProps<'a> ()
    set view
    TreeViewElement<'a>(view.props) :> ITreeViewElement

  static member treeView<'a when 'a: not struct>(children: ITerminalElement list) =
    let view = treeViewProps<'a> ()
    view.children children
    TreeViewElement<'a>(view.props) :> ITreeViewElement

  static member treeView<'a when 'a: not struct>(x: int, y: int, title: string) =
    let setProps =
      fun (p: treeViewProps<'a>) ->
        p.x (Pos.Absolute(x))
        p.y (Pos.Absolute(y))
        p.title title

    let view = treeViewProps<'a> ()
    setProps view
    TreeViewElement<'a>(view.props) :> ITreeViewElement

  /// <seealso cref="Terminal.Gui.Window"/>
  static member window(set: windowProps -> unit) =
    let view = windowProps ()
    set view
    WindowElement(view.props) :> ITerminalElement

  static member window(children: ITerminalElement list) =
    let view = windowProps ()
    view.children children
    WindowElement(view.props) :> ITerminalElement

  static member window(x: int, y: int, title: string) =
    let setProps =
      fun (p: windowProps) ->
        p.x (Pos.Absolute(x))
        p.y (Pos.Absolute(y))
        p.title title

    let view = windowProps ()
    setProps view
    WindowElement(view.props) :> ITerminalElement

  /// <seealso cref="Terminal.Gui.Wizard"/>
  static member wizard(set: wizardProps -> unit) =
    let view = wizardProps ()
    set view
    WizardElement(view.props) :> ITerminalElement

  static member wizard(children: ITerminalElement list) =
    let view = wizardProps ()
    view.children children
    WizardElement(view.props) :> ITerminalElement

  static member wizard(x: int, y: int, title: string) =
    let setProps =
      fun (p: wizardProps) ->
        p.x (Pos.Absolute(x))
        p.y (Pos.Absolute(y))
        p.title title

    let view = wizardProps ()
    setProps view
    WizardElement(view.props) :> ITerminalElement

  /// <seealso cref="Terminal.Gui.WizardStep"/>
  static member wizardStep(set: wizardStepProps -> unit) =
    let view = wizardStepProps ()
    set view
    WizardStepElement(view.props) :> ITerminalElement

  static member wizardStep(children: ITerminalElement list) =
    let view = wizardStepProps ()
    view.children children
    WizardStepElement(view.props) :> ITerminalElement

  static member wizardStep(x: int, y: int, title: string) =
    let setProps =
      fun (p: wizardStepProps) ->
        p.x (Pos.Absolute(x))
        p.y (Pos.Absolute(y))
        p.title title

    let view = wizardStepProps ()
    setProps view
    WizardStepElement(view.props) :> ITerminalElement

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

