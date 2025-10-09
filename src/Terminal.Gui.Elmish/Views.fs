
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
        let view = toplevel()
        view.children children
        ToplevelElement(view.props) :> TerminalElement
    static member inline topLevel (set:toplevel -> unit) =
        let view = toplevel()
        set view
        ToplevelElement(view.props) :> TerminalElement

    /// <seealso cref="Terminal.Gui.Adornment"/>
    static member inline adornment (set:adornment -> unit) =
        let view = adornment()
        set view
        AdornmentElement(view.props) :> TerminalElement
    static member inline adornment (children:TerminalElement list) =
        let view = adornment()
        view.children children
        AdornmentElement(view.props) :> TerminalElement
    static member inline adornment (x:int, y:int, title: string) =
        let setProps = fun (p: adornment) ->
            p.x (Pos.Absolute(x))
            p.y (Pos.Absolute(y))
            p.title title

        let view = adornment()
        setProps view
        AdornmentElement(view.props) :> TerminalElement

    /// <seealso cref="Terminal.Gui.Bar"/>
    static member inline bar (set:bar -> unit) =
        let view = bar()
        set view
        BarElement(view.props) :> TerminalElement
    static member inline bar (children:TerminalElement list) =
        let view = bar()
        view.children children
        BarElement(view.props) :> TerminalElement
    static member inline bar (x:int, y:int, title: string) =
        let setProps = fun (p: bar) ->
            p.x (Pos.Absolute(x))
            p.y (Pos.Absolute(y))
            p.title title

        let view = bar()
        setProps view
        BarElement(view.props) :> TerminalElement

    /// <seealso cref="Terminal.Gui.Border"/>
    static member inline border (set:border -> unit) =
        let view = border()
        set view
        BorderElement(view.props) :> TerminalElement
    static member inline border (children:TerminalElement list) =
        let view = border()
        view.children children
        BorderElement(view.props) :> TerminalElement
    static member inline border (x:int, y:int, title: string) =
        let setProps = fun (p: border) ->
            p.x (Pos.Absolute(x))
            p.y (Pos.Absolute(y))
            p.title title

        let view = border()
        setProps view
        BorderElement(view.props) :> TerminalElement

    /// <seealso cref="Terminal.Gui.Button"/>
    static member inline button (set:button -> unit) =
        let view = button()
        set view
        ButtonElement(view.props) :> TerminalElement
    static member inline button (children:TerminalElement list) =
        let view = button()
        view.children children
        ButtonElement(view.props) :> TerminalElement
    static member inline button (x:int, y:int, title: string) =
        let setProps = fun (p: button) ->
            p.x (Pos.Absolute(x))
            p.y (Pos.Absolute(y))
            p.title title

        let view = button()
        setProps view
        // TODO: could get better perf when choosing another props type
        ButtonElement(view.props) :> TerminalElement

    /// <seealso cref="Terminal.Gui.CheckBox"/>
    static member inline checkBox (set:checkBox -> unit) =
        let view = checkBox()
        set view
        CheckBoxElement(view.props) :> TerminalElement
    static member inline checkBox (children:TerminalElement list) =
        let view = checkBox()
        view.children children
        CheckBoxElement(view.props) :> TerminalElement
    static member inline checkBox (x:int, y:int, title: string) =
        let setProps = fun (p: checkBox) ->
            p.x (Pos.Absolute(x))
            p.y (Pos.Absolute(y))
            p.title title

        let view = checkBox()
        setProps view
        CheckBoxElement(view.props) :> TerminalElement

    /// <seealso cref="Terminal.Gui.ColorPicker"/>
    static member inline colorPicker (set:colorPicker -> unit) =
        let view = colorPicker()
        set view
        ColorPickerElement(view.props) :> TerminalElement
    static member inline colorPicker (children:TerminalElement list) =
        let view = colorPicker()
        view.children children
        ColorPickerElement(view.props) :> TerminalElement
    static member inline colorPicker (x:int, y:int, title: string) =
        let setProps = fun (p: colorPicker) ->
            p.x (Pos.Absolute(x))
            p.y (Pos.Absolute(y))
            p.title title

        let view = colorPicker()
        setProps view
        ColorPickerElement(view.props) :> TerminalElement

    /// <seealso cref="Terminal.Gui.ColorPicker16"/>
    static member inline colorPicker16 (set:colorPicker16 -> unit) =
        let view = colorPicker16()
        set view
        ColorPicker16Element(view.props) :> TerminalElement
    static member inline colorPicker16 (children:TerminalElement list) =
        let view = colorPicker16()
        view.children children
        ColorPicker16Element(view.props) :> TerminalElement
    static member inline colorPicker16 (x:int, y:int, title: string) =
        let setProps = fun (p: colorPicker16) ->
            p.x (Pos.Absolute(x))
            p.y (Pos.Absolute(y))
            p.title title

        let view = colorPicker16()
        setProps view
        ColorPicker16Element(view.props) :> TerminalElement

    /// <seealso cref="Terminal.Gui.ComboBox"/>
    static member inline comboBox (set:comboBox -> unit) =
        let view = comboBox()
        set view
        ComboBoxElement(view.props) :> TerminalElement
    static member inline comboBox (children:TerminalElement list) =
        let view = comboBox()
        view.children children
        ComboBoxElement(view.props) :> TerminalElement
    static member inline comboBox (x:int, y:int, title: string) =
        let setProps = fun (p: comboBox) ->
            p.x (Pos.Absolute(x))
            p.y (Pos.Absolute(y))
            p.title title

        let view = comboBox()
        setProps view
        ComboBoxElement(view.props) :> TerminalElement

    /// <seealso cref="Terminal.Gui.DateField"/>
    static member inline dateField (set:dateField -> unit) =
        let view = dateField()
        set view
        DateFieldElement(view.props) :> TerminalElement
    static member inline dateField (children:TerminalElement list) =
        let view = dateField()
        view.children children
        DateFieldElement(view.props) :> TerminalElement
    static member inline dateField (x:int, y:int, title: string) =
        let setProps = fun (p: dateField) ->
            p.x (Pos.Absolute(x))
            p.y (Pos.Absolute(y))
            p.title title

        let view = dateField()
        setProps view
        DateFieldElement(view.props) :> TerminalElement

    /// <seealso cref="Terminal.Gui.DatePicker"/>
    static member inline datePicker (set:datePicker -> unit) =
        let view = datePicker()
        set view
        DatePickerElement(view.props) :> TerminalElement
    static member inline datePicker (children:TerminalElement list) =
        let view = datePicker()
        view.children children
        DatePickerElement(view.props) :> TerminalElement
    static member inline datePicker (x:int, y:int, title: string) =
        let setProps = fun (p: datePicker) ->
            p.x (Pos.Absolute(x))
            p.y (Pos.Absolute(y))
            p.title title

        let view = datePicker()
        setProps view
        DatePickerElement(view.props) :> TerminalElement

    /// <seealso cref="Terminal.Gui.Dialog"/>
    static member inline dialog (set:dialog -> unit) =
        let view = dialog()
        set view
        DialogElement(view.props) :> TerminalElement
    static member inline dialog (children:TerminalElement list) =
        let view = dialog()
        view.children children
        DialogElement(view.props) :> TerminalElement
    static member inline dialog (x:int, y:int, title: string) =
        let setProps = fun (p: dialog) ->
            p.x (Pos.Absolute(x))
            p.y (Pos.Absolute(y))
            p.title title

        let view = dialog()
        setProps view
        DialogElement(view.props) :> TerminalElement

    /// <seealso cref="Terminal.Gui.FileDialog"/>
    static member inline fileDialog (set:fileDialog -> unit) =
        let view = fileDialog()
        set view
        FileDialogElement(view.props) :> TerminalElement
    static member inline fileDialog (children:TerminalElement list) =
        let view = fileDialog()
        view.children children
        FileDialogElement(view.props) :> TerminalElement
    static member inline fileDialog (x:int, y:int, title: string) =
        let setProps = fun (p: fileDialog) ->
            p.x (Pos.Absolute(x))
            p.y (Pos.Absolute(y))
            p.title title

        let view = fileDialog()
        setProps view
        FileDialogElement(view.props) :> TerminalElement

    /// <seealso cref="Terminal.Gui.FrameView"/>
    static member inline frameView (set:frameView -> unit) =
        let view = frameView()
        set view
        FrameViewElement(view.props) :> TerminalElement
    static member inline frameView (children:TerminalElement list) =
        let view = frameView()
        view.children children
        FrameViewElement(view.props) :> TerminalElement
    static member inline frameView (x:int, y:int, title: string) =
        let setProps = fun (p: frameView) ->
            p.x (Pos.Absolute(x))
            p.y (Pos.Absolute(y))
            p.title title

        let view = frameView()
        setProps view
        FrameViewElement(view.props) :> TerminalElement

    /// <seealso cref="Terminal.Gui.GraphView"/>
    static member inline graphView (set:graphView -> unit) =
        let view = graphView()
        set view
        GraphViewElement(view.props) :> TerminalElement
    static member inline graphView (children:TerminalElement list) =
        let view = graphView()
        view.children children
        GraphViewElement(view.props) :> TerminalElement
    static member inline graphView (x:int, y:int, title: string) =
        let setProps = fun (p: graphView) ->
            p.x (Pos.Absolute(x))
            p.y (Pos.Absolute(y))
            p.title title

        let view = graphView()
        setProps view
        GraphViewElement(view.props) :> TerminalElement

    /// <seealso cref="Terminal.Gui.HexView"/>
    static member inline hexView (set:hexView -> unit) =
        let view = hexView()
        set view
        HexViewElement(view.props) :> TerminalElement
    static member inline hexView (children:TerminalElement list) =
        let view = hexView()
        view.children children
        HexViewElement(view.props) :> TerminalElement
    static member inline hexView (x:int, y:int, title: string) =
        let setProps = fun (p: hexView) ->
            p.x (Pos.Absolute(x))
            p.y (Pos.Absolute(y))
            p.title title

        let view = hexView()
        setProps view
        HexViewElement(view.props) :> TerminalElement

    /// <seealso cref="Terminal.Gui.Label"/>
    static member inline label (set:label -> unit) =
        let view = label()
        set view
        LabelElement(view.props) :> TerminalElement
    static member inline label (children:TerminalElement list) =
        let view = label()
        view.children children
        LabelElement(view.props) :> TerminalElement
    static member inline label (x:int, y:int, title: string) =
        let setProps = fun (p: label) ->
            p.x (Pos.Absolute(x))
            p.y (Pos.Absolute(y))
            p.title title

        let view = label()
        setProps view
        LabelElement(view.props) :> TerminalElement

    /// <seealso cref="Terminal.Gui.LegendAnnotation"/>
    static member inline legendAnnotation (set:legendAnnotation -> unit) =
        let view = legendAnnotation()
        set view
        LegendAnnotationElement(view.props) :> TerminalElement
    static member inline legendAnnotation (children:TerminalElement list) =
        let view = legendAnnotation()
        view.children children
        LegendAnnotationElement(view.props) :> TerminalElement
    static member inline legendAnnotation (x:int, y:int, title: string) =
        let setProps = fun (p: legendAnnotation) ->
            p.x (Pos.Absolute(x))
            p.y (Pos.Absolute(y))
            p.title title

        let view = legendAnnotation()
        setProps view
        LegendAnnotationElement(view.props) :> TerminalElement

    /// <seealso cref="Terminal.Gui.Line"/>
    static member inline line (set:line -> unit) =
        let view = line()
        set view
        LineElement(view.props) :> TerminalElement
    static member inline line (children:TerminalElement list) =
        let view = line()
        view.children children
        LineElement(view.props) :> TerminalElement
    static member inline line (x:int, y:int, title: string) =
        let setProps = fun (p: line) ->
            p.x (Pos.Absolute(x))
            p.y (Pos.Absolute(y))
            p.title title

        let view = line()
        setProps view
        LineElement(view.props) :> TerminalElement

    /// <seealso cref="Terminal.Gui.LineView"/>
    static member inline lineView (set:lineView -> unit) =
        let view = lineView()
        set view
        LineViewElement(view.props) :> TerminalElement
    static member inline lineView (children:TerminalElement list) =
        let view = lineView()
        view.children children
        LineViewElement(view.props) :> TerminalElement
    static member inline lineView (x:int, y:int, title: string) =
        let setProps = fun (p: lineView) ->
            p.x (Pos.Absolute(x))
            p.y (Pos.Absolute(y))
            p.title title

        let view = lineView()
        setProps view
        LineViewElement(view.props) :> TerminalElement

    /// <seealso cref="Terminal.Gui.ListView"/>
    static member inline listView (set:listView -> unit) =
        let view = listView()
        set view
        ListViewElement(view.props) :> TerminalElement
    static member inline listView (children:TerminalElement list) =
        let view = listView()
        view.children children
        ListViewElement(view.props) :> TerminalElement
    static member inline listView (x:int, y:int, title: string) =
        let setProps = fun (p: listView) ->
            p.x (Pos.Absolute(x))
            p.y (Pos.Absolute(y))
            p.title title

        let view = listView()
        setProps view
        ListViewElement(view.props) :> TerminalElement

    /// <seealso cref="Terminal.Gui.Margin"/>
    static member inline margin (set:margin -> unit) =
        let view = margin()
        set view
        MarginElement(view.props) :> TerminalElement
    static member inline margin (children:TerminalElement list) =
        let view = margin()
        view.children children
        MarginElement(view.props) :> TerminalElement
    static member inline margin (x:int, y:int, title: string) =
        let setProps = fun (p: margin) ->
            p.x (Pos.Absolute(x))
            p.y (Pos.Absolute(y))
            p.title title

        let view = margin()
        setProps view
        MarginElement(view.props) :> TerminalElement

    /// <seealso cref="Terminal.Gui.MenuBarv2"/>
    static member inline menuBarv2 (set:menuBarv2 -> unit) =
        let view = menuBarv2()
        set view
        MenuBarv2Element(view.props) :> TerminalElement
    static member inline menuBarv2 (children:TerminalElement list) =
        let view = menuBarv2()
        view.children children
        MenuBarv2Element(view.props) :> TerminalElement
    static member inline menuBarv2 (x:int, y:int, title: string) =
        let setProps = fun (p: menuBarv2) ->
            p.x (Pos.Absolute(x))
            p.y (Pos.Absolute(y))
            p.title title

        let view = menuBarv2()
        setProps view
        MenuBarv2Element(view.props) :> TerminalElement

    /// <seealso cref="Terminal.Gui.Menuv2"/>
    static member inline menuv2 (set: menuv2 -> unit) =
        let view = menuv2()
        set(view)
        Menuv2Element(view.props) // Not casted to TerminalElement as for now its usage is targeted for menuItemv2.submenu
    static member inline menuv2 (children:TerminalElement list) =
        let view = menuv2()
        view.children children
        Menuv2Element(view.props) :> TerminalElement
    static member inline menuv2 (x:int, y:int, title: string) =
        let setProps = fun (p: menuv2) ->
            p.x (Pos.Absolute(x))
            p.y (Pos.Absolute(y))
            p.title title

        let view = menuv2()
        setProps view
        Menuv2Element(view.props) :> TerminalElement

    static member inline menuBarItemv2 (set: menuBarItemv2 -> unit) =
        let view = menuBarItemv2()
        set(view)
        MenuBarItemv2Element(view.props) // Not casted to TerminalElement as for now its usage is targeted for menuBarv2.menus

    static member inline popoverMenu (set: popoverMenu -> unit) =
        let view = popoverMenu()
        set(view)
        PopoverMenuElement(view.props) // Not casted to TerminalElement as for now its usage is targeted for menuBarItemv2.popoverMenu

    static member inline menuItemv2 (set: menuItemv2 -> unit) =
        let view = menuItemv2()
        set(view)
        MenuItemv2Element(view.props) :> TerminalElement

    /// <seealso cref="Terminal.Gui.NumericUpDown"/>
    static member inline numericUpDown (set: numericUpDown -> unit) =
        let view = numericUpDown()
        set(view)
        NumericUpDownElement(view.props) :> TerminalElement
    static member inline numericUpDown (children:TerminalElement list) =
        let view = numericUpDown()
        view.children children
        NumericUpDownElement(view.props) :> TerminalElement
    static member inline numericUpDown (x:int, y:int, title: string) =
        let setProps = fun (p: numericUpDown) ->
            p.x (Pos.Absolute(x))
            p.y (Pos.Absolute(y))
            p.title title

        let view = numericUpDown()
        setProps view
        NumericUpDownElement(view.props) :> TerminalElement

    /// <seealso cref="Terminal.Gui.NumericUpDown"/>
    static member inline numericUpDown<'a> (set: numericUpDown<'a> -> unit) =
        let view = numericUpDown<'a>()
        set(view)
        NumericUpDownElement<'a>(view.props)
    static member inline numericUpDown<'a> (children:TerminalElement list) =
        let view = numericUpDown<'a>()
        view.children children
        NumericUpDownElement<'a>(view.props)
    static member inline numericUpDown<'a> (x:int, y:int, title: string) =
        let setProps = fun (p: numericUpDown<'a>) ->
            p.x (Pos.Absolute(x))
            p.y (Pos.Absolute(y))
            p.title title

        let view = numericUpDown<'a>()
        setProps view
        NumericUpDownElement<'a>(view.props)

    /// <seealso cref="Terminal.Gui.OpenDialog"/>
    static member inline openDialog (set: openDialog -> unit) =
        let view = openDialog()
        set(view)
        OpenDialogElement(view.props) :> TerminalElement
    static member inline openDialog (children:TerminalElement list) =
        let view = openDialog()
        view.children children
        OpenDialogElement(view.props) :> TerminalElement
    static member inline openDialog (x:int, y:int, title: string) =
        let setProps = fun (p: openDialog) ->
            p.x (Pos.Absolute(x))
            p.y (Pos.Absolute(y))
            p.title title

        let view = openDialog()
        setProps view
        OpenDialogElement(view.props) :> TerminalElement

    /// <seealso cref="Terminal.Gui.Padding"/>
    static member inline padding (set: padding -> unit) =
        let view = padding()
        set(view)
        PaddingElement(view.props) :> TerminalElement
    static member inline padding (children:TerminalElement list) =
        let view = padding()
        view.children children
        PaddingElement(view.props) :> TerminalElement
    static member inline padding (x:int, y:int, title: string) =
        let setProps = fun (p: padding) ->
            p.x (Pos.Absolute(x))
            p.y (Pos.Absolute(y))
            p.title title

        let view = padding()
        setProps view
        PaddingElement(view.props) :> TerminalElement

    /// <seealso cref="Terminal.Gui.ProgressBar"/>
    static member inline progressBar (set: progressBar -> unit) =
        let view = progressBar()
        set(view)
        ProgressBarElement(view.props) :> TerminalElement
    static member inline progressBar (children:TerminalElement list) =
        let view = progressBar()
        view.children children
        ProgressBarElement(view.props) :> TerminalElement
    static member inline progressBar (x:int, y:int, title: string) =
        let setProps = fun (p: progressBar) ->
            p.x (Pos.Absolute(x))
            p.y (Pos.Absolute(y))
            p.title title

        let view = progressBar()
        setProps view
        ProgressBarElement(view.props) :> TerminalElement

    /// <seealso cref="Terminal.Gui.RadioGroup"/>
    static member inline radioGroup (set: radioGroup -> unit) =
        let view = radioGroup()
        set(view)
        RadioGroupElement(view.props) :> TerminalElement
    static member inline radioGroup (children:TerminalElement list) =
        let view = radioGroup()
        view.children children
        RadioGroupElement(view.props) :> TerminalElement
    static member inline radioGroup (x:int, y:int, title: string) =
        let setProps = fun (p: radioGroup) ->
            p.x (Pos.Absolute(x))
            p.y (Pos.Absolute(y))
            p.title title

        let view = radioGroup()
        setProps view
        RadioGroupElement(view.props) :> TerminalElement

    /// <seealso cref="Terminal.Gui.SaveDialog"/>
    static member inline saveDialog (set: saveDialog -> unit) =
        let view = saveDialog()
        set(view)
        SaveDialogElement(view.props) :> TerminalElement
    static member inline saveDialog (children:TerminalElement list) =
        let view = saveDialog()
        view.children children
        SaveDialogElement(view.props) :> TerminalElement
    static member inline saveDialog (x:int, y:int, title: string) =
        let setProps = fun (p: saveDialog) ->
            p.x (Pos.Absolute(x))
            p.y (Pos.Absolute(y))
            p.title title

        let view = saveDialog()
        setProps view
        SaveDialogElement(view.props) :> TerminalElement

    /// <seealso cref="Terminal.Gui.ScrollBar"/>
    static member inline scrollBar (set: scrollBar -> unit) =
        let view = scrollBar()
        set(view)
        ScrollBarElement(view.props) :> TerminalElement
    static member inline scrollBar (children:TerminalElement list) =
        let view = scrollBar()
        view.children children
        ScrollBarElement(view.props) :> TerminalElement
    static member inline scrollBar (x:int, y:int, title: string) =
        let setProps = fun (p: scrollBar) ->
            p.x (Pos.Absolute(x))
            p.y (Pos.Absolute(y))
            p.title title

        let view = scrollBar()
        setProps view
        ScrollBarElement(view.props) :> TerminalElement

    /// <seealso cref="Terminal.Gui.ScrollSlider"/>
    static member inline scrollSlider (set: scrollSlider -> unit) =
        let view = scrollSlider()
        set(view)
        ScrollSliderElement(view.props) :> TerminalElement
    static member inline scrollSlider (children:TerminalElement list) =
        let view = scrollSlider()
        view.children children
        ScrollSliderElement(view.props) :> TerminalElement
    static member inline scrollSlider (x:int, y:int, title: string) =
        let setProps = fun (p: scrollSlider) ->
            p.x (Pos.Absolute(x))
            p.y (Pos.Absolute(y))
            p.title title

        let view = scrollSlider()
        setProps view
        ScrollSliderElement(view.props) :> TerminalElement

    /// <seealso cref="Terminal.Gui.Shortcut"/>
    static member inline shortcut (set: shortcut -> unit) =
        let view = shortcut()
        set(view)
        ShortcutElement(view.props) :> TerminalElement
    static member inline shortcut (children:TerminalElement list) =
        let view = shortcut()
        view.children children
        ShortcutElement(view.props) :> TerminalElement
    static member inline shortcut (x:int, y:int, title: string) =
        let setProps = fun (p: shortcut) ->
            p.x (Pos.Absolute(x))
            p.y (Pos.Absolute(y))
            p.title title

        let view = shortcut()
        setProps view
        ShortcutElement(view.props) :> TerminalElement

    /// <seealso cref="Terminal.Gui.Slider"/>
    static member inline slider (set: slider -> unit) =
        let view = slider()
        set(view)
        SliderElement(view.props) :> TerminalElement
    static member inline slider (children:TerminalElement list) =
        let view = slider()
        view.children children
        SliderElement(view.props) :> TerminalElement
    static member inline slider (x:int, y:int, title: string) =
        let setProps = fun (p: slider) ->
            p.x (Pos.Absolute(x))
            p.y (Pos.Absolute(y))
            p.title title

        let view = slider()
        setProps view
        SliderElement(view.props) :> TerminalElement

    /// <seealso cref="Terminal.Gui.Slider"/>
    static member inline slider<'a> (set: slider<'a> -> unit) =
        let view = slider<'a>()
        set(view)
        SliderElement<'a>(view.props)
    static member inline slider<'a> (children:TerminalElement list) =
        let view = slider<'a>()
        view.children children
        SliderElement<'a>(view.props)
    static member inline slider<'a> (x:int, y:int, title: string) =
        let setProps = fun (p: slider<'a>) ->
            p.x (Pos.Absolute(x))
            p.y (Pos.Absolute(y))
            p.title title

        let view = slider<'a>()
        setProps view
        SliderElement<'a>(view.props)

    /// <seealso cref="Terminal.Gui.SpinnerView"/>
    static member inline spinnerView (set: spinnerView -> unit) =
        let view = spinnerView()
        set(view)
        SpinnerViewElement(view.props) :> TerminalElement
    static member inline spinnerView (children:TerminalElement list) =
        let view = spinnerView()
        view.children children
        SpinnerViewElement(view.props) :> TerminalElement
    static member inline spinnerView (x:int, y:int, title: string) =
        let setProps = fun (p: spinnerView) ->
            p.x (Pos.Absolute(x))
            p.y (Pos.Absolute(y))
            p.title title

        let view = spinnerView()
        setProps view
        SpinnerViewElement(view.props) :> TerminalElement

    /// <seealso cref="Terminal.Gui.StatusBar"/>
    static member inline statusBar (set: statusBar -> unit) =
        let view = statusBar()
        set(view)
        StatusBarElement(view.props) :> TerminalElement
    static member inline statusBar (children:TerminalElement list) =
        let view = statusBar()
        view.children children
        StatusBarElement(view.props) :> TerminalElement
    static member inline statusBar (x:int, y:int, title: string) =
        let setProps = fun (p: statusBar) ->
            p.x (Pos.Absolute(x))
            p.y (Pos.Absolute(y))
            p.title title

        let view = statusBar()
        setProps view
        StatusBarElement(view.props) :> TerminalElement

    /// <seealso cref="Terminal.Gui.Tab"/>
    static member inline tab (set: tab -> unit) =
        let view = tab()
        set(view)
        TabElement(view.props) :> TerminalElement
    static member inline tab (children:TerminalElement list) =
        let view = tab()
        view.children children
        TabElement(view.props) :> TerminalElement
    static member inline tab (x:int, y:int, title: string) =
        let setProps = fun (p: tab) ->
            p.x (Pos.Absolute(x))
            p.y (Pos.Absolute(y))
            p.title title

        let view = tab()
        setProps view
        TabElement(view.props) :> TerminalElement

    /// <seealso cref="Terminal.Gui.TabView"/>
    static member inline tabView (set: tabView -> unit) =
        let view = tabView()
        set(view)
        TabViewElement(view.props) :> TerminalElement
    static member inline tabView (children:TerminalElement list) =
        let view = tabView()
        view.children children
        TabViewElement(view.props) :> TerminalElement
    static member inline tabView (x:int, y:int, title: string) =
        let setProps = fun (p: tabView) ->
            p.x (Pos.Absolute(x))
            p.y (Pos.Absolute(y))
            p.title title

        let view = tabView()
        setProps view
        TabViewElement(view.props) :> TerminalElement

    /// <seealso cref="Terminal.Gui.TableView"/>
    static member inline tableView (set: tableView -> unit) =
        let view = tableView()
        set(view)
        TableViewElement(view.props) :> TerminalElement
    static member inline tableView (children:TerminalElement list) =
        let view = tableView()
        view.children children
        TableViewElement(view.props) :> TerminalElement
    static member inline tableView (x:int, y:int, title: string) =
        let setProps = fun (p: tableView) ->
            p.x (Pos.Absolute(x))
            p.y (Pos.Absolute(y))
            p.title title

        let view = tableView()
        setProps view
        TableViewElement(view.props) :> TerminalElement

    /// <seealso cref="Terminal.Gui.TextField"/>
    static member inline textField (set: textField -> unit) =
        let view = textField()
        set(view)
        TextFieldElement(view.props) :> TerminalElement
    static member inline textField (children:TerminalElement list) =
        let view = textField()
        view.children children
        TextFieldElement(view.props) :> TerminalElement
    static member inline textField (x:int, y:int, title: string) =
        let setProps = fun (p: textField) ->
            p.x (Pos.Absolute(x))
            p.y (Pos.Absolute(y))
            p.title title

        let view = textField()
        setProps view
        TextFieldElement(view.props) :> TerminalElement

    /// <seealso cref="Terminal.Gui.TextValidateField"/>
    static member inline textValidateField (set: textValidateField -> unit) =
        let view = textValidateField()
        set(view)
        TextValidateFieldElement(view.props) :> TerminalElement
    static member inline textValidateField (children:TerminalElement list) =
        let view = textValidateField()
        view.children children
        TextValidateFieldElement(view.props) :> TerminalElement
    static member inline textValidateField (x:int, y:int, title: string) =
        let setProps = fun (p: textValidateField) ->
            p.x (Pos.Absolute(x))
            p.y (Pos.Absolute(y))
            p.title title

        let view = textValidateField()
        setProps view
        TextValidateFieldElement(view.props) :> TerminalElement

    /// <seealso cref="Terminal.Gui.TextView"/>
    static member inline textView (set: textView -> unit) =
        let view = textView()
        set(view)
        TextViewElement(view.props) :> TerminalElement
    static member inline textView (children:TerminalElement list) =
        let view = textView()
        view.children children
        TextViewElement(view.props) :> TerminalElement
    static member inline textView (x:int, y:int, title: string) =
        let setProps = fun (p: textView) ->
            p.x (Pos.Absolute(x))
            p.y (Pos.Absolute(y))
            p.title title

        let view = textView()
        setProps view
        TextViewElement(view.props) :> TerminalElement

    /// <seealso cref="Terminal.Gui.TileView"/>
    static member inline tileView (set: tileView -> unit) =
        let view = tileView()
        set(view)
        TileViewElement(view.props) :> TerminalElement
    static member inline tileView (children:TerminalElement list) =
        let view = tileView()
        view.children children
        TileViewElement(view.props) :> TerminalElement
    static member inline tileView (x:int, y:int, title: string) =
        let setProps = fun (p: tileView) ->
            p.x (Pos.Absolute(x))
            p.y (Pos.Absolute(y))
            p.title title

        let view = tileView()
        setProps view
        TileViewElement(view.props) :> TerminalElement

    /// <seealso cref="Terminal.Gui.TimeField"/>
    static member inline timeField (set: timeField -> unit) =
        let view = timeField()
        set(view)
        TimeFieldElement(view.props) :> TerminalElement
    static member inline timeField (children:TerminalElement list) =
        let view = timeField()
        view.children children
        TimeFieldElement(view.props) :> TerminalElement
    static member inline timeField (x:int, y:int, title: string) =
        let setProps = fun (p: timeField) ->
            p.x (Pos.Absolute(x))
            p.y (Pos.Absolute(y))
            p.title title

        let view = timeField()
        setProps view
        TimeFieldElement(view.props) :> TerminalElement

    /// <seealso cref="Terminal.Gui.Toplevel"/>
    static member inline toplevel (set: toplevel -> unit) =
        let view = toplevel()
        set(view)
        ToplevelElement(view.props) :> TerminalElement
    static member inline toplevel (children:TerminalElement list) =
        let view = toplevel()
        view.children children
        ToplevelElement(view.props) :> TerminalElement
    static member inline toplevel (x:int, y:int, title: string) =
        let setProps = fun (p: toplevel) ->
            p.x (Pos.Absolute(x))
            p.y (Pos.Absolute(y))
            p.title title

        let view = toplevel()
        setProps view
        ToplevelElement(view.props) :> TerminalElement

    /// <seealso cref="Terminal.Gui.TreeView"/>
    static member inline treeView (set: treeView -> unit) =
        let view = treeView()
        set(view)
        TreeViewElement(view.props) :> TerminalElement
    static member inline treeView (children:TerminalElement list) =
        let view = treeView()
        view.children children
        TreeViewElement(view.props) :> TerminalElement
    static member inline treeView (x:int, y:int, title: string) =
        let setProps = fun (p: treeView<_>) ->
            p.x (Pos.Absolute(x))
            p.y (Pos.Absolute(y))
            p.title title

        let view = treeView()
        setProps view
        TreeViewElement(view.props) :> TerminalElement

    /// <seealso cref="Terminal.Gui.TreeView"/>
    static member inline treeView<'a when 'a : not struct> (set: treeView<'a> -> unit) =
        let view = treeView<'a>()
        set(view)
        TreeViewElement<'a>(view.props)
    static member inline treeView<'a when 'a : not struct> (children:TerminalElement list) =
        let view = treeView<'a>()
        view.children children
        TreeViewElement<'a>(view.props)
    static member inline treeView<'a when 'a : not struct> (x:int, y:int, title: string) =
        let setProps = fun (p: treeView<'a>) ->
            p.x (Pos.Absolute(x))
            p.y (Pos.Absolute(y))
            p.title title

        let view = treeView<'a>()
        setProps view
        TreeViewElement<'a>(view.props)

    /// <seealso cref="Terminal.Gui.Window"/>
    static member inline window (set: window -> unit) =
        let view = window()
        set(view)
        WindowElement(view.props) :> TerminalElement
    static member inline window (children:TerminalElement list) =
        let view = window()
        view.children children
        WindowElement(view.props) :> TerminalElement
    static member inline window (x:int, y:int, title: string) =
        let setProps = fun (p: window) ->
            p.x (Pos.Absolute(x))
            p.y (Pos.Absolute(y))
            p.title title

        let view = window()
        setProps view
        WindowElement(view.props) :> TerminalElement

    /// <seealso cref="Terminal.Gui.Wizard"/>
    static member inline wizard (set: wizard -> unit) =
        let view = wizard()
        set(view)
        WizardElement(view.props) :> TerminalElement
    static member inline wizard (children:TerminalElement list) =
        let view = wizard()
        view.children children
        WizardElement(view.props) :> TerminalElement
    static member inline wizard (x:int, y:int, title: string) =
        let setProps = fun (p: wizard) ->
            p.x (Pos.Absolute(x))
            p.y (Pos.Absolute(y))
            p.title title

        let view = wizard()
        setProps view
        WizardElement(view.props) :> TerminalElement

    /// <seealso cref="Terminal.Gui.WizardStep"/>
    static member inline wizardStep (set: wizardStep -> unit) =
        let view = wizardStep()
        set(view)
        WizardStepElement(view.props) :> TerminalElement
    static member inline wizardStep (children:TerminalElement list) =
        let view = wizardStep()
        view.children children
        WizardStepElement(view.props) :> TerminalElement
    static member inline wizardStep (x:int, y:int, title: string) =
        let setProps = fun (p: wizardStep) ->
            p.x (Pos.Absolute(x))
            p.y (Pos.Absolute(y))
            p.title title

        let view = wizardStep()
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
