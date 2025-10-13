
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

    static member inline topLevel (children:TerminalElement list) =
        let view = toplevelProps()
        view.children children
        ToplevelElement(view.props) :> TerminalElement
    static member inline topLevel (set: toplevelProps -> unit) =
        let view = toplevelProps()
        set view
        ToplevelElement(view.props) :> TerminalElement

    /// <seealso cref="Terminal.Gui.Adornment"/>
    static member inline adornment (set: adornmentProps -> unit) =
        let view = adornmentProps()
        set view
        AdornmentElement(view.props) :> TerminalElement
    static member inline adornment (children:TerminalElement list) =
        let view = adornmentProps()
        view.children children
        AdornmentElement(view.props) :> TerminalElement
    static member inline adornment (x:int, y:int, title: string) =
        let setProps = fun (p: adornmentProps) ->
            p.x (Pos.Absolute(x))
            p.y (Pos.Absolute(y))
            p.title title

        let view = adornmentProps()
        setProps view
        AdornmentElement(view.props) :> TerminalElement

    /// <seealso cref="Terminal.Gui.Bar"/>
    static member inline bar (set: barProps -> unit) =
        let view = barProps()
        set view
        BarElement(view.props) :> TerminalElement
    static member inline bar (children:TerminalElement list) =
        let view = barProps()
        view.children children
        BarElement(view.props) :> TerminalElement
    static member inline bar (x:int, y:int, title: string) =
        let setProps = fun (p: barProps) ->
            p.x (Pos.Absolute(x))
            p.y (Pos.Absolute(y))
            p.title title

        let view = barProps()
        setProps view
        BarElement(view.props) :> TerminalElement

    /// <seealso cref="Terminal.Gui.Border"/>
    static member inline border (set: borderProps -> unit) =
        let view = borderProps()
        set view
        BorderElement(view.props) :> TerminalElement
    static member inline border (children:TerminalElement list) =
        let view = borderProps()
        view.children children
        BorderElement(view.props) :> TerminalElement
    static member inline border (x:int, y:int, title: string) =
        let setProps = fun (p: borderProps) ->
            p.x (Pos.Absolute(x))
            p.y (Pos.Absolute(y))
            p.title title

        let view = borderProps()
        setProps view
        BorderElement(view.props) :> TerminalElement

    /// <seealso cref="Terminal.Gui.Button"/>
    static member inline button (set: buttonProps -> unit) =
        let view = buttonProps()
        set view
        ButtonElement(view.props) :> TerminalElement
    static member inline button (children:TerminalElement list) =
        let view = buttonProps()
        view.children children
        ButtonElement(view.props) :> TerminalElement
    static member inline button (x:int, y:int, title: string) =
        let setProps = fun (p: buttonProps) ->
            p.x (Pos.Absolute(x))
            p.y (Pos.Absolute(y))
            p.title title

        let view = buttonProps()
        setProps view
        // TODO: could get better perf when choosing another props type
        ButtonElement(view.props) :> TerminalElement

    /// <seealso cref="Terminal.Gui.CheckBox"/>
    static member inline checkBox (set: checkBoxProps -> unit) =
        let view = checkBoxProps()
        set view
        CheckBoxElement(view.props) :> TerminalElement
    static member inline checkBox (children:TerminalElement list) =
        let view = checkBoxProps()
        view.children children
        CheckBoxElement(view.props) :> TerminalElement
    static member inline checkBox (x:int, y:int, title: string) =
        let setProps = fun (p: checkBoxProps) ->
            p.x (Pos.Absolute(x))
            p.y (Pos.Absolute(y))
            p.title title

        let view = checkBoxProps()
        setProps view
        CheckBoxElement(view.props) :> TerminalElement

    /// <seealso cref="Terminal.Gui.ColorPicker"/>
    static member inline colorPicker (set: colorPickerProps -> unit) =
        let view = colorPickerProps()
        set view
        ColorPickerElement(view.props) :> TerminalElement
    static member inline colorPicker (children:TerminalElement list) =
        let view = colorPickerProps()
        view.children children
        ColorPickerElement(view.props) :> TerminalElement
    static member inline colorPicker (x:int, y:int, title: string) =
        let setProps = fun (p: colorPickerProps) ->
            p.x (Pos.Absolute(x))
            p.y (Pos.Absolute(y))
            p.title title

        let view = colorPickerProps()
        setProps view
        ColorPickerElement(view.props) :> TerminalElement

    /// <seealso cref="Terminal.Gui.ColorPicker16"/>
    static member inline colorPicker16 (set: colorPicker16Props -> unit) =
        let view = colorPicker16Props()
        set view
        ColorPicker16Element(view.props) :> TerminalElement
    static member inline colorPicker16 (children:TerminalElement list) =
        let view = colorPicker16Props()
        view.children children
        ColorPicker16Element(view.props) :> TerminalElement
    static member inline colorPicker16 (x:int, y:int, title: string) =
        let setProps = fun (p: colorPicker16Props) ->
            p.x (Pos.Absolute(x))
            p.y (Pos.Absolute(y))
            p.title title

        let view = colorPicker16Props()
        setProps view
        ColorPicker16Element(view.props) :> TerminalElement

    /// <seealso cref="Terminal.Gui.ComboBox"/>
    static member inline comboBox (set: comboBoxProps -> unit) =
        let view = comboBoxProps()
        set view
        ComboBoxElement(view.props) :> TerminalElement
    static member inline comboBox (children:TerminalElement list) =
        let view = comboBoxProps()
        view.children children
        ComboBoxElement(view.props) :> TerminalElement
    static member inline comboBox (x:int, y:int, title: string) =
        let setProps = fun (p: comboBoxProps) ->
            p.x (Pos.Absolute(x))
            p.y (Pos.Absolute(y))
            p.title title

        let view = comboBoxProps()
        setProps view
        ComboBoxElement(view.props) :> TerminalElement

    /// <seealso cref="Terminal.Gui.DateField"/>
    static member inline dateField (set: dateFieldProps -> unit) =
        let view = dateFieldProps()
        set view
        DateFieldElement(view.props) :> TerminalElement
    static member inline dateField (children:TerminalElement list) =
        let view = dateFieldProps()
        view.children children
        DateFieldElement(view.props) :> TerminalElement
    static member inline dateField (x:int, y:int, title: string) =
        let setProps = fun (p: dateFieldProps) ->
            p.x (Pos.Absolute(x))
            p.y (Pos.Absolute(y))
            p.title title

        let view = dateFieldProps()
        setProps view
        DateFieldElement(view.props) :> TerminalElement

    /// <seealso cref="Terminal.Gui.DatePicker"/>
    static member inline datePicker (set: datePickerProps -> unit) =
        let view = datePickerProps()
        set view
        DatePickerElement(view.props) :> TerminalElement
    static member inline datePicker (children:TerminalElement list) =
        let view = datePickerProps()
        view.children children
        DatePickerElement(view.props) :> TerminalElement
    static member inline datePicker (x:int, y:int, title: string) =
        let setProps = fun (p: datePickerProps) ->
            p.x (Pos.Absolute(x))
            p.y (Pos.Absolute(y))
            p.title title

        let view = datePickerProps()
        setProps view
        DatePickerElement(view.props) :> TerminalElement

    /// <seealso cref="Terminal.Gui.Dialog"/>
    static member inline dialog (set: dialogProps -> unit) =
        let view = dialogProps()
        set view
        DialogElement(view.props) :> TerminalElement
    static member inline dialog (children:TerminalElement list) =
        let view = dialogProps()
        view.children children
        DialogElement(view.props) :> TerminalElement
    static member inline dialog (x:int, y:int, title: string) =
        let setProps = fun (p: dialogProps) ->
            p.x (Pos.Absolute(x))
            p.y (Pos.Absolute(y))
            p.title title

        let view = dialogProps()
        setProps view
        DialogElement(view.props) :> TerminalElement

    /// <seealso cref="Terminal.Gui.FileDialog"/>
    static member inline fileDialog (set: fileDialogProps -> unit) =
        let view = fileDialogProps()
        set view
        FileDialogElement(view.props) :> TerminalElement
    static member inline fileDialog (children:TerminalElement list) =
        let view = fileDialogProps()
        view.children children
        FileDialogElement(view.props) :> TerminalElement
    static member inline fileDialog (x:int, y:int, title: string) =
        let setProps = fun (p: fileDialogProps) ->
            p.x (Pos.Absolute(x))
            p.y (Pos.Absolute(y))
            p.title title

        let view = fileDialogProps()
        setProps view
        FileDialogElement(view.props) :> TerminalElement

    /// <seealso cref="Terminal.Gui.FrameView"/>
    static member inline frameView (set: frameViewProps -> unit) =
        let view = frameViewProps()
        set view
        FrameViewElement(view.props) :> TerminalElement
    static member inline frameView (children:TerminalElement list) =
        let view = frameViewProps()
        view.children children
        FrameViewElement(view.props) :> TerminalElement
    static member inline frameView (x:int, y:int, title: string) =
        let setProps = fun (p: frameViewProps) ->
            p.x (Pos.Absolute(x))
            p.y (Pos.Absolute(y))
            p.title title

        let view = frameViewProps()
        setProps view
        FrameViewElement(view.props) :> TerminalElement

    /// <seealso cref="Terminal.Gui.GraphView"/>
    static member inline graphView (set: graphViewProps -> unit) =
        let view = graphViewProps()
        set view
        GraphViewElement(view.props) :> TerminalElement
    static member inline graphView (children:TerminalElement list) =
        let view = graphViewProps()
        view.children children
        GraphViewElement(view.props) :> TerminalElement
    static member inline graphView (x:int, y:int, title: string) =
        let setProps = fun (p: graphViewProps) ->
            p.x (Pos.Absolute(x))
            p.y (Pos.Absolute(y))
            p.title title

        let view = graphViewProps()
        setProps view
        GraphViewElement(view.props) :> TerminalElement

    /// <seealso cref="Terminal.Gui.HexView"/>
    static member inline hexView (set: hexViewProps -> unit) =
        let view = hexViewProps()
        set view
        HexViewElement(view.props) :> TerminalElement
    static member inline hexView (children:TerminalElement list) =
        let view = hexViewProps()
        view.children children
        HexViewElement(view.props) :> TerminalElement
    static member inline hexView (x:int, y:int, title: string) =
        let setProps = fun (p: hexViewProps) ->
            p.x (Pos.Absolute(x))
            p.y (Pos.Absolute(y))
            p.title title

        let view = hexViewProps()
        setProps view
        HexViewElement(view.props) :> TerminalElement

    /// <seealso cref="Terminal.Gui.Label"/>
    static member inline label (set: labelProps -> unit) =
        let view = labelProps()
        set view
        LabelElement(view.props) :> TerminalElement
    static member inline label (children:TerminalElement list) =
        let view = labelProps()
        view.children children
        LabelElement(view.props) :> TerminalElement
    static member inline label (x:int, y:int, title: string) =
        let setProps = fun (p: labelProps) ->
            p.x (Pos.Absolute(x))
            p.y (Pos.Absolute(y))
            p.title title

        let view = labelProps()
        setProps view
        LabelElement(view.props) :> TerminalElement

    /// <seealso cref="Terminal.Gui.LegendAnnotation"/>
    static member inline legendAnnotation (set: legendAnnotationProps -> unit) =
        let view = legendAnnotationProps()
        set view
        LegendAnnotationElement(view.props) :> TerminalElement
    static member inline legendAnnotation (children:TerminalElement list) =
        let view = legendAnnotationProps()
        view.children children
        LegendAnnotationElement(view.props) :> TerminalElement
    static member inline legendAnnotation (x:int, y:int, title: string) =
        let setProps = fun (p: legendAnnotationProps) ->
            p.x (Pos.Absolute(x))
            p.y (Pos.Absolute(y))
            p.title title

        let view = legendAnnotationProps()
        setProps view
        LegendAnnotationElement(view.props) :> TerminalElement

    /// <seealso cref="Terminal.Gui.Line"/>
    static member inline line (set: lineProps -> unit) =
        let view = lineProps()
        set view
        LineElement(view.props) :> TerminalElement
    static member inline line (children:TerminalElement list) =
        let view = lineProps()
        view.children children
        LineElement(view.props) :> TerminalElement
    static member inline line (x:int, y:int, title: string) =
        let setProps = fun (p: lineProps) ->
            p.x (Pos.Absolute(x))
            p.y (Pos.Absolute(y))
            p.title title

        let view = lineProps()
        setProps view
        LineElement(view.props) :> TerminalElement

    /// <seealso cref="Terminal.Gui.LineView"/>
    static member inline lineView (set: lineViewProps -> unit) =
        let view = lineViewProps()
        set view
        LineViewElement(view.props) :> TerminalElement
    static member inline lineView (children:TerminalElement list) =
        let view = lineViewProps()
        view.children children
        LineViewElement(view.props) :> TerminalElement
    static member inline lineView (x:int, y:int, title: string) =
        let setProps = fun (p: lineViewProps) ->
            p.x (Pos.Absolute(x))
            p.y (Pos.Absolute(y))
            p.title title

        let view = lineViewProps()
        setProps view
        LineViewElement(view.props) :> TerminalElement

    /// <seealso cref="Terminal.Gui.ListView"/>
    static member inline listView (set: listViewProps -> unit) =
        let view = listViewProps()
        set view
        ListViewElement(view.props) :> TerminalElement
    static member inline listView (children:TerminalElement list) =
        let view = listViewProps()
        view.children children
        ListViewElement(view.props) :> TerminalElement
    static member inline listView (x:int, y:int, title: string) =
        let setProps = fun (p: listViewProps) ->
            p.x (Pos.Absolute(x))
            p.y (Pos.Absolute(y))
            p.title title

        let view = listViewProps()
        setProps view
        ListViewElement(view.props) :> TerminalElement

    /// <seealso cref="Terminal.Gui.Margin"/>
    static member inline margin (set: marginProps -> unit) =
        let view = marginProps()
        set view
        MarginElement(view.props) :> TerminalElement
    static member inline margin (children:TerminalElement list) =
        let view = marginProps()
        view.children children
        MarginElement(view.props) :> TerminalElement
    static member inline margin (x:int, y:int, title: string) =
        let setProps = fun (p: marginProps) ->
            p.x (Pos.Absolute(x))
            p.y (Pos.Absolute(y))
            p.title title

        let view = marginProps()
        setProps view
        MarginElement(view.props) :> TerminalElement

    /// <seealso cref="Terminal.Gui.MenuBarv2"/>
    static member inline menuBarv2 (set: menuBarv2Props -> menuBarv2Macros -> unit) =
        let props = menuBarv2Props()
        let macros = menuBarv2Macros(props)
        set props macros
        MenuBarv2Element(props.props) :> TerminalElement
    static member inline menuBarv2 (children:TerminalElement list) =
        let view = menuBarv2Props()
        view.children children
        MenuBarv2Element(view.props) :> TerminalElement
    static member inline menuBarv2 (x:int, y:int, title: string) =
        let setProps = fun (p: menuBarv2Props) ->
            p.x (Pos.Absolute(x))
            p.y (Pos.Absolute(y))
            p.title title

        let view = menuBarv2Props()
        setProps view
        MenuBarv2Element(view.props) :> TerminalElement

    /// <seealso cref="Terminal.Gui.Menuv2"/>
    static member inline menuv2 (set: menuv2Props -> unit) =
        let view = menuv2Props()
        set(view)
        Menuv2Element(view.props) // Not casted to TerminalElement as for now its usage is targeted for menuItemv2.submenu
    static member inline menuv2 (children:TerminalElement list) =
        let view = menuv2Props()
        view.children children
        Menuv2Element(view.props) :> TerminalElement
    static member inline menuv2 (x:int, y:int, title: string) =
        let setProps = fun (p: menuv2Props) ->
            p.x (Pos.Absolute(x))
            p.y (Pos.Absolute(y))
            p.title title

        let view = menuv2Props()
        setProps view
        Menuv2Element(view.props) :> TerminalElement

    static member inline menuBarItemv2 (set: menuBarItemv2Props -> unit) =
        let view = menuBarItemv2Props()
        set(view)
        MenuBarItemv2Element(view.props) // Not casted to TerminalElement as for now its usage is targeted for menuBarv2.menus

    static member inline popoverMenu (set: popoverMenuProps -> unit) =
        let view = popoverMenuProps()
        set(view)
        PopoverMenuElement(view.props) // Not casted to TerminalElement as for now its usage is targeted for menuBarItemv2.popoverMenu

    static member inline menuItemv2 (set: menuItemv2Props -> unit) =
        let view = menuItemv2Props()
        set(view)
        MenuItemv2Element(view.props) :> TerminalElement

    /// <seealso cref="Terminal.Gui.NumericUpDown"/>
    static member inline numericUpDown (set: numericUpDownProps -> unit) =
        let view = numericUpDownProps()
        set(view)
        NumericUpDownElement(view.props) :> TerminalElement
    static member inline numericUpDown (children:TerminalElement list) =
        let view = numericUpDownProps()
        view.children children
        NumericUpDownElement(view.props) :> TerminalElement
    static member inline numericUpDown (x:int, y:int, title: string) =
        let setProps = fun (p: numericUpDownProps) ->
            p.x (Pos.Absolute(x))
            p.y (Pos.Absolute(y))
            p.title title

        let view = numericUpDownProps()
        setProps view
        NumericUpDownElement(view.props) :> TerminalElement

    /// <seealso cref="Terminal.Gui.NumericUpDown"/>
    static member inline numericUpDown<'a> (set: numericUpDownProps<'a> -> unit) =
        let view = numericUpDownProps<'a>()
        set(view)
        NumericUpDownElement<'a>(view.props)
    static member inline numericUpDown<'a> (children:TerminalElement list) =
        let view = numericUpDownProps<'a>()
        view.children children
        NumericUpDownElement<'a>(view.props)
    static member inline numericUpDown<'a> (x:int, y:int, title: string) =
        let setProps = fun (p: numericUpDownProps<'a>) ->
            p.x (Pos.Absolute(x))
            p.y (Pos.Absolute(y))
            p.title title

        let view = numericUpDownProps<'a>()
        setProps view
        NumericUpDownElement<'a>(view.props)

    /// <seealso cref="Terminal.Gui.OpenDialog"/>
    static member inline openDialog (set: openDialogProps -> unit) =
        let view = openDialogProps()
        set(view)
        OpenDialogElement(view.props) :> TerminalElement
    static member inline openDialog (children:TerminalElement list) =
        let view = openDialogProps()
        view.children children
        OpenDialogElement(view.props) :> TerminalElement
    static member inline openDialog (x:int, y:int, title: string) =
        let setProps = fun (p: openDialogProps) ->
            p.x (Pos.Absolute(x))
            p.y (Pos.Absolute(y))
            p.title title

        let view = openDialogProps()
        setProps view
        OpenDialogElement(view.props) :> TerminalElement

    static member inline optionSelector(set: optionSelectorProps -> unit) =
        let view = optionSelectorProps()
        set(view)
        OptionSelectorElement(view.props) :> TerminalElement

    /// <seealso cref="Terminal.Gui.Padding"/>
    static member inline padding (set: paddingProps -> unit) =
        let view = paddingProps()
        set(view)
        PaddingElement(view.props) :> TerminalElement
    static member inline padding (children:TerminalElement list) =
        let view = paddingProps()
        view.children children
        PaddingElement(view.props) :> TerminalElement
    static member inline padding (x:int, y:int, title: string) =
        let setProps = fun (p: paddingProps) ->
            p.x (Pos.Absolute(x))
            p.y (Pos.Absolute(y))
            p.title title

        let view = paddingProps()
        setProps view
        PaddingElement(view.props) :> TerminalElement

    /// <seealso cref="Terminal.Gui.ProgressBar"/>
    static member inline progressBar (set: progressBarProps -> unit) =
        let view = progressBarProps()
        set(view)
        ProgressBarElement(view.props) :> TerminalElement
    static member inline progressBar (children:TerminalElement list) =
        let view = progressBarProps()
        view.children children
        ProgressBarElement(view.props) :> TerminalElement
    static member inline progressBar (x:int, y:int, title: string) =
        let setProps = fun (p: progressBarProps) ->
            p.x (Pos.Absolute(x))
            p.y (Pos.Absolute(y))
            p.title title

        let view = progressBarProps()
        setProps view
        ProgressBarElement(view.props) :> TerminalElement

    /// <seealso cref="Terminal.Gui.RadioGroup"/>
    static member inline radioGroup (set: radioGroupProps -> unit) =
        let view = radioGroupProps()
        set(view)
        RadioGroupElement(view.props) :> TerminalElement
    static member inline radioGroup (children:TerminalElement list) =
        let view = radioGroupProps()
        view.children children
        RadioGroupElement(view.props) :> TerminalElement
    static member inline radioGroup (x:int, y:int, title: string) =
        let setProps = fun (p: radioGroupProps) ->
            p.x (Pos.Absolute(x))
            p.y (Pos.Absolute(y))
            p.title title

        let view = radioGroupProps()
        setProps view
        RadioGroupElement(view.props) :> TerminalElement

    /// <seealso cref="Terminal.Gui.SaveDialog"/>
    static member inline saveDialog (set: saveDialogProps -> unit) =
        let view = saveDialogProps()
        set(view)
        SaveDialogElement(view.props) :> TerminalElement
    static member inline saveDialog (children:TerminalElement list) =
        let view = saveDialogProps()
        view.children children
        SaveDialogElement(view.props) :> TerminalElement
    static member inline saveDialog (x:int, y:int, title: string) =
        let setProps = fun (p: saveDialogProps) ->
            p.x (Pos.Absolute(x))
            p.y (Pos.Absolute(y))
            p.title title

        let view = saveDialogProps()
        setProps view
        SaveDialogElement(view.props) :> TerminalElement

    /// <seealso cref="Terminal.Gui.ScrollBar"/>
    static member inline scrollBar (set: scrollBarProps -> unit) =
        let view = scrollBarProps()
        set(view)
        ScrollBarElement(view.props) :> TerminalElement
    static member inline scrollBar (children:TerminalElement list) =
        let view = scrollBarProps()
        view.children children
        ScrollBarElement(view.props) :> TerminalElement
    static member inline scrollBar (x:int, y:int, title: string) =
        let setProps = fun (p: scrollBarProps) ->
            p.x (Pos.Absolute(x))
            p.y (Pos.Absolute(y))
            p.title title

        let view = scrollBarProps()
        setProps view
        ScrollBarElement(view.props) :> TerminalElement

    /// <seealso cref="Terminal.Gui.ScrollSlider"/>
    static member inline scrollSlider (set: scrollSliderProps -> unit) =
        let view = scrollSliderProps()
        set(view)
        ScrollSliderElement(view.props) :> TerminalElement
    static member inline scrollSlider (children:TerminalElement list) =
        let view = scrollSliderProps()
        view.children children
        ScrollSliderElement(view.props) :> TerminalElement
    static member inline scrollSlider (x:int, y:int, title: string) =
        let setProps = fun (p: scrollSliderProps) ->
            p.x (Pos.Absolute(x))
            p.y (Pos.Absolute(y))
            p.title title

        let view = scrollSliderProps()
        setProps view
        ScrollSliderElement(view.props) :> TerminalElement

    /// <seealso cref="Terminal.Gui.Shortcut"/>
    static member inline shortcut (set: shortcutProps -> unit) =
        let view = shortcutProps()
        set(view)
        ShortcutElement(view.props) :> TerminalElement
    static member inline shortcut (children:TerminalElement list) =
        let view = shortcutProps()
        view.children children
        ShortcutElement(view.props) :> TerminalElement
    static member inline shortcut (x:int, y:int, title: string) =
        let setProps = fun (p: shortcutProps) ->
            p.x (Pos.Absolute(x))
            p.y (Pos.Absolute(y))
            p.title title

        let view = shortcutProps()
        setProps view
        ShortcutElement(view.props) :> TerminalElement

    /// <seealso cref="Terminal.Gui.Slider"/>
    static member inline slider (set: sliderProps -> unit) =
        let view = sliderProps()
        set(view)
        SliderElement(view.props) :> TerminalElement
    static member inline slider (children:TerminalElement list) =
        let view = sliderProps()
        view.children children
        SliderElement(view.props) :> TerminalElement
    static member inline slider (x:int, y:int, title: string) =
        let setProps = fun (p: sliderProps) ->
            p.x (Pos.Absolute(x))
            p.y (Pos.Absolute(y))
            p.title title

        let view = sliderProps()
        setProps view
        SliderElement(view.props) :> TerminalElement

    /// <seealso cref="Terminal.Gui.Slider"/>
    static member inline slider<'a> (set: sliderProps<'a> -> unit) =
        let view = sliderProps<'a>()
        set(view)
        SliderElement<'a>(view.props)
    static member inline slider<'a> (children:TerminalElement list) =
        let view = sliderProps<'a>()
        view.children children
        SliderElement<'a>(view.props)
    static member inline slider<'a> (x:int, y:int, title: string) =
        let setProps = fun (p: sliderProps<'a>) ->
            p.x (Pos.Absolute(x))
            p.y (Pos.Absolute(y))
            p.title title

        let view = sliderProps<'a>()
        setProps view
        SliderElement<'a>(view.props)

    /// <seealso cref="Terminal.Gui.SpinnerView"/>
    static member inline spinnerView (set: spinnerViewProps -> unit) =
        let view = spinnerViewProps()
        set(view)
        SpinnerViewElement(view.props) :> TerminalElement
    static member inline spinnerView (children:TerminalElement list) =
        let view = spinnerViewProps()
        view.children children
        SpinnerViewElement(view.props) :> TerminalElement
    static member inline spinnerView (x:int, y:int, title: string) =
        let setProps = fun (p: spinnerViewProps) ->
            p.x (Pos.Absolute(x))
            p.y (Pos.Absolute(y))
            p.title title

        let view = spinnerViewProps()
        setProps view
        SpinnerViewElement(view.props) :> TerminalElement

    /// <seealso cref="Terminal.Gui.StatusBar"/>
    static member inline statusBar (set: statusBarProps -> unit) =
        let view = statusBarProps()
        set(view)
        StatusBarElement(view.props) :> TerminalElement
    static member inline statusBar (children:TerminalElement list) =
        let view = statusBarProps()
        view.children children
        StatusBarElement(view.props) :> TerminalElement
    static member inline statusBar (x:int, y:int, title: string) =
        let setProps = fun (p: statusBarProps) ->
            p.x (Pos.Absolute(x))
            p.y (Pos.Absolute(y))
            p.title title

        let view = statusBarProps()
        setProps view
        StatusBarElement(view.props) :> TerminalElement

    /// <seealso cref="Terminal.Gui.Tab"/>
    static member inline tab (set: tabProps -> unit) =
        let view = tabProps()
        set(view)
        TabElement(view.props) :> TerminalElement
    static member inline tab (children:TerminalElement list) =
        let view = tabProps()
        view.children children
        TabElement(view.props) :> TerminalElement
    static member inline tab (x:int, y:int, title: string) =
        let setProps = fun (p: tabProps) ->
            p.x (Pos.Absolute(x))
            p.y (Pos.Absolute(y))
            p.title title

        let view = tabProps()
        setProps view
        TabElement(view.props) :> TerminalElement

    /// <seealso cref="Terminal.Gui.TabView"/>
    static member inline tabView (set: tabViewProps -> unit) =
        let view = tabViewProps()
        set(view)
        TabViewElement(view.props) :> TerminalElement
    static member inline tabView (children:TerminalElement list) =
        let view = tabViewProps()
        view.children children
        TabViewElement(view.props) :> TerminalElement
    static member inline tabView (x:int, y:int, title: string) =
        let setProps = fun (p: tabViewProps) ->
            p.x (Pos.Absolute(x))
            p.y (Pos.Absolute(y))
            p.title title

        let view = tabViewProps()
        setProps view
        TabViewElement(view.props) :> TerminalElement

    /// <seealso cref="Terminal.Gui.TableView"/>
    static member inline tableView (set: tableViewProps -> unit) =
        let view = tableViewProps()
        set(view)
        TableViewElement(view.props) :> TerminalElement
    static member inline tableView (children:TerminalElement list) =
        let view = tableViewProps()
        view.children children
        TableViewElement(view.props) :> TerminalElement
    static member inline tableView (x:int, y:int, title: string) =
        let setProps = fun (p: tableViewProps) ->
            p.x (Pos.Absolute(x))
            p.y (Pos.Absolute(y))
            p.title title

        let view = tableViewProps()
        setProps view
        TableViewElement(view.props) :> TerminalElement

    /// <seealso cref="Terminal.Gui.TextField"/>
    static member inline textField (set: textFieldProps -> unit) =
        let view = textFieldProps()
        set(view)
        TextFieldElement(view.props) :> TerminalElement
    static member inline textField (children:TerminalElement list) =
        let view = textFieldProps()
        view.children children
        TextFieldElement(view.props) :> TerminalElement
    static member inline textField (x:int, y:int, title: string) =
        let setProps = fun (p: textFieldProps) ->
            p.x (Pos.Absolute(x))
            p.y (Pos.Absolute(y))
            p.title title

        let view = textFieldProps()
        setProps view
        TextFieldElement(view.props) :> TerminalElement

    /// <seealso cref="Terminal.Gui.TextValidateField"/>
    static member inline textValidateField (set: textValidateFieldProps -> unit) =
        let view = textValidateFieldProps()
        set(view)
        TextValidateFieldElement(view.props) :> TerminalElement
    static member inline textValidateField (children:TerminalElement list) =
        let view = textValidateFieldProps()
        view.children children
        TextValidateFieldElement(view.props) :> TerminalElement
    static member inline textValidateField (x:int, y:int, title: string) =
        let setProps = fun (p: textValidateFieldProps) ->
            p.x (Pos.Absolute(x))
            p.y (Pos.Absolute(y))
            p.title title

        let view = textValidateFieldProps()
        setProps view
        TextValidateFieldElement(view.props) :> TerminalElement

    /// <seealso cref="Terminal.Gui.TextView"/>
    static member inline textView (set: textViewProps -> unit) =
        let view = textViewProps()
        set(view)
        TextViewElement(view.props) :> TerminalElement
    static member inline textView (children:TerminalElement list) =
        let view = textViewProps()
        view.children children
        TextViewElement(view.props) :> TerminalElement
    static member inline textView (x:int, y:int, title: string) =
        let setProps = fun (p: textViewProps) ->
            p.x (Pos.Absolute(x))
            p.y (Pos.Absolute(y))
            p.title title

        let view = textViewProps()
        setProps view
        TextViewElement(view.props) :> TerminalElement

    /// <seealso cref="Terminal.Gui.TileView"/>
    static member inline tileView (set: tileViewProps -> unit) =
        let view = tileViewProps()
        set(view)
        TileViewElement(view.props) :> TerminalElement
    static member inline tileView (children:TerminalElement list) =
        let view = tileViewProps()
        view.children children
        TileViewElement(view.props) :> TerminalElement
    static member inline tileView (x:int, y:int, title: string) =
        let setProps = fun (p: tileViewProps) ->
            p.x (Pos.Absolute(x))
            p.y (Pos.Absolute(y))
            p.title title

        let view = tileViewProps()
        setProps view
        TileViewElement(view.props) :> TerminalElement

    /// <seealso cref="Terminal.Gui.TimeField"/>
    static member inline timeField (set: timeFieldProps -> unit) =
        let view = timeFieldProps()
        set(view)
        TimeFieldElement(view.props) :> TerminalElement
    static member inline timeField (children:TerminalElement list) =
        let view = timeFieldProps()
        view.children children
        TimeFieldElement(view.props) :> TerminalElement
    static member inline timeField (x:int, y:int, title: string) =
        let setProps = fun (p: timeFieldProps) ->
            p.x (Pos.Absolute(x))
            p.y (Pos.Absolute(y))
            p.title title

        let view = timeFieldProps()
        setProps view
        TimeFieldElement(view.props) :> TerminalElement

    /// <seealso cref="Terminal.Gui.Toplevel"/>
    static member inline toplevel (set: toplevelProps -> unit) =
        let view = toplevelProps()
        set(view)
        ToplevelElement(view.props) :> TerminalElement
    static member inline toplevel (children:TerminalElement list) =
        let view = toplevelProps()
        view.children children
        ToplevelElement(view.props) :> TerminalElement
    static member inline toplevel (x:int, y:int, title: string) =
        let setProps = fun (p: toplevelProps) ->
            p.x (Pos.Absolute(x))
            p.y (Pos.Absolute(y))
            p.title title

        let view = toplevelProps()
        setProps view
        ToplevelElement(view.props) :> TerminalElement

    /// <seealso cref="Terminal.Gui.TreeView"/>
    static member inline treeView (set: treeViewProps -> unit) =
        let view = treeViewProps()
        set(view)
        TreeViewElement(view.props) :> TerminalElement
    static member inline treeView (children:TerminalElement list) =
        let view = treeViewProps()
        view.children children
        TreeViewElement(view.props) :> TerminalElement
    static member inline treeView (x:int, y:int, title: string) =
        let setProps = fun (p: treeViewProps<_>) ->
            p.x (Pos.Absolute(x))
            p.y (Pos.Absolute(y))
            p.title title

        let view = treeViewProps()
        setProps view
        TreeViewElement(view.props) :> TerminalElement

    /// <seealso cref="Terminal.Gui.TreeView"/>
    static member inline treeView<'a when 'a : not struct> (set: treeViewProps<'a> -> unit) =
        let view = treeViewProps<'a>()
        set(view)
        TreeViewElement<'a>(view.props)
    static member inline treeView<'a when 'a : not struct> (children:TerminalElement list) =
        let view = treeViewProps<'a>()
        view.children children
        TreeViewElement<'a>(view.props)
    static member inline treeView<'a when 'a : not struct> (x:int, y:int, title: string) =
        let setProps = fun (p: treeViewProps<'a>) ->
            p.x (Pos.Absolute(x))
            p.y (Pos.Absolute(y))
            p.title title

        let view = treeViewProps<'a>()
        setProps view
        TreeViewElement<'a>(view.props)

    /// <seealso cref="Terminal.Gui.Window"/>
    static member inline window (set: windowProps -> unit) =
        let view = windowProps()
        set(view)
        WindowElement(view.props) :> TerminalElement
    static member inline window (children:TerminalElement list) =
        let view = windowProps()
        view.children children
        WindowElement(view.props) :> TerminalElement
    static member inline window (x:int, y:int, title: string) =
        let setProps = fun (p: windowProps) ->
            p.x (Pos.Absolute(x))
            p.y (Pos.Absolute(y))
            p.title title

        let view = windowProps()
        setProps view
        WindowElement(view.props) :> TerminalElement

    /// <seealso cref="Terminal.Gui.Wizard"/>
    static member inline wizard (set: wizardProps -> unit) =
        let view = wizardProps()
        set(view)
        WizardElement(view.props) :> TerminalElement
    static member inline wizard (children:TerminalElement list) =
        let view = wizardProps()
        view.children children
        WizardElement(view.props) :> TerminalElement
    static member inline wizard (x:int, y:int, title: string) =
        let setProps = fun (p: wizardProps) ->
            p.x (Pos.Absolute(x))
            p.y (Pos.Absolute(y))
            p.title title

        let view = wizardProps()
        setProps view
        WizardElement(view.props) :> TerminalElement

    /// <seealso cref="Terminal.Gui.WizardStep"/>
    static member inline wizardStep (set: wizardStepProps -> unit) =
        let view = wizardStepProps()
        set(view)
        WizardStepElement(view.props) :> TerminalElement
    static member inline wizardStep (children:TerminalElement list) =
        let view = wizardStepProps()
        view.children children
        WizardStepElement(view.props) :> TerminalElement
    static member inline wizardStep (x:int, y:int, title: string) =
        let setProps = fun (p: wizardStepProps) ->
            p.x (Pos.Absolute(x))
            p.y (Pos.Absolute(y))
            p.title title

        let view = wizardStepProps()
        setProps view
        WizardStepElement(view.props) :> TerminalElement



module Dialogs =
    let showWizard (wizard:Wizard) =
        Application.Run(wizard)
        wizard.Dispose()
        ()


    let openFileDialog title =
        use dia = new OpenDialog(Title=title)
        Application.Run(dia)
        dia.Dispose()
        //Application.Top.Remove(dia) |> ignore
        if dia.Canceled then
            None
        else
            let file =
                dia.FilePaths
                |> Seq.tryHead
                |> Option.bind (fun s ->
                    if String.IsNullOrEmpty(s) then None
                    else Some s
                )
            file


    let messageBox (width:int) height title text (buttons:string list) =
        let result = MessageBox.Query(width,height,title,text,buttons |> List.toArray)
        match buttons with
        | [] -> ""
        | _ when result < 0 || result > buttons.Length - 1  -> ""
        | _ -> buttons.[result]


    let errorBox (width:int) height title text (buttons:string list) =
        let result = MessageBox.ErrorQuery(width,height,title,text,buttons |> List.toArray)
        match buttons with
        | [] -> ""
        | _ when result < 0 || result > buttons.Length - 1  -> ""
        | _ -> buttons.[result]
