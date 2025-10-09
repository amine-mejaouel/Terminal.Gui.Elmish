
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

    static member inline topLevel (props:IProperty list) = ToplevelElement(props) :> TerminalElement
    static member inline topLevel (children:TerminalElement list) =
        let view = toplevel()
        view.children children
        ToplevelElement(view.props |> Seq.toList) :> TerminalElement


    /// <seealso cref="Terminal.Gui.Adornment"/>
    static member inline adornment (props:IProperty list) = AdornmentElement(props) :> TerminalElement
    static member inline adornment (children:TerminalElement list) =
        let view = adornment()
        view.children children
        AdornmentElement(view.props |> Seq.toList) :> TerminalElement
    static member inline adornment (x:int, y:int, title: string) =
        let setProps = fun (p: adornment) ->
            p.x (Pos.Absolute(x))
            p.y (Pos.Absolute(y))
            p.title title

        let view = adornment()
        setProps view
        AdornmentElement(view.props |> Seq.toList) :> TerminalElement

    /// <seealso cref="Terminal.Gui.Bar"/>
    static member inline bar (props:IProperty list) = BarElement(props) :> TerminalElement
    static member inline bar (children:TerminalElement list) =
        let view = bar()
        view.children children
        BarElement(view.props |> Seq.toList) :> TerminalElement
    static member inline bar (x:int, y:int, title: string) =
        let setProps = fun (p: bar) ->
            p.x (Pos.Absolute(x))
            p.y (Pos.Absolute(y))
            p.title title

        let view = bar()
        setProps view
        BarElement(view.props |> Seq.toList) :> TerminalElement

    /// <seealso cref="Terminal.Gui.Border"/>
    static member inline border (props:IProperty list) = BorderElement(props) :> TerminalElement
    static member inline border (children:TerminalElement list) =
        let view = border()
        view.children children
        BorderElement(view.props |> Seq.toList) :> TerminalElement
    static member inline border (x:int, y:int, title: string) =
        let setProps = fun (p: border) ->
            p.x (Pos.Absolute(x))
            p.y (Pos.Absolute(y))
            p.title title

        let view = border()
        setProps view
        BorderElement(view.props |> Seq.toList) :> TerminalElement

    /// <seealso cref="Terminal.Gui.Button"/>
    static member inline button (props:IProperty list) = ButtonElement(props) :> TerminalElement
    static member inline button (children:TerminalElement list) =
        let view = button()
        view.children children
        ButtonElement(view.props |> Seq.toList) :> TerminalElement
    static member inline button (x:int, y:int, title: string) =
        let setProps = fun (p: button) ->
            p.x (Pos.Absolute(x))
            p.y (Pos.Absolute(y))
            p.title title

        let view = button()
        setProps view
        // TODO: could get better perf when choosing another props type
        ButtonElement(view.props |> Seq.toList) :> TerminalElement

    /// <seealso cref="Terminal.Gui.CheckBox"/>
    static member inline checkBox (props:IProperty list) = CheckBoxElement(props) :> TerminalElement
    static member inline checkBox (children:TerminalElement list) =
        let view = checkBox()
        view.children children
        CheckBoxElement(view.props |> Seq.toList) :> TerminalElement
    static member inline checkBox (x:int, y:int, title: string) =
        let setProps = fun (p: checkBox) ->
            p.x (Pos.Absolute(x))
            p.y (Pos.Absolute(y))
            p.title title

        let view = checkBox()
        setProps view
        CheckBoxElement(view.props |> Seq.toList) :> TerminalElement

    /// <seealso cref="Terminal.Gui.ColorPicker"/>
    static member inline colorPicker (props:IProperty list) = ColorPickerElement(props) :> TerminalElement
    static member inline colorPicker (children:TerminalElement list) =
        let view = colorPicker()
        view.children children
        ColorPickerElement(view.props |> Seq.toList) :> TerminalElement
    static member inline colorPicker (x:int, y:int, title: string) =
        let setProps = fun (p: colorPicker) ->
            p.x (Pos.Absolute(x))
            p.y (Pos.Absolute(y))
            p.title title

        let view = colorPicker()
        setProps view
        ColorPickerElement(view.props |> Seq.toList) :> TerminalElement

    /// <seealso cref="Terminal.Gui.ColorPicker16"/>
    static member inline colorPicker16 (props:IProperty list) = ColorPicker16Element(props) :> TerminalElement
    static member inline colorPicker16 (children:TerminalElement list) =
        let view = colorPicker16()
        view.children children
        ColorPicker16Element(view.props |> Seq.toList) :> TerminalElement
    static member inline colorPicker16 (x:int, y:int, title: string) =
        let setProps = fun (p: colorPicker16) ->
            p.x (Pos.Absolute(x))
            p.y (Pos.Absolute(y))
            p.title title

        let view = colorPicker16()
        setProps view
        ColorPicker16Element(view.props |> Seq.toList) :> TerminalElement

    /// <seealso cref="Terminal.Gui.ComboBox"/>
    static member inline comboBox (props:IProperty list) = ComboBoxElement(props) :> TerminalElement
    static member inline comboBox (children:TerminalElement list) =
        let view = comboBox()
        view.children children
        ComboBoxElement(view.props |> Seq.toList) :> TerminalElement
    static member inline comboBox (x:int, y:int, title: string) =
        let setProps = fun (p: comboBox) ->
            p.x (Pos.Absolute(x))
            p.y (Pos.Absolute(y))
            p.title title

        let view = comboBox()
        setProps view
        ComboBoxElement(view.props |> Seq.toList) :> TerminalElement

    /// <seealso cref="Terminal.Gui.DateField"/>
    static member inline dateField (props:IProperty list) = DateFieldElement(props) :> TerminalElement
    static member inline dateField (children:TerminalElement list) =
        let view = dateField()
        view.children children
        DateFieldElement(view.props |> Seq.toList) :> TerminalElement
    static member inline dateField (x:int, y:int, title: string) =
        let setProps = fun (p: dateField) ->
            p.x (Pos.Absolute(x))
            p.y (Pos.Absolute(y))
            p.title title

        let view = dateField()
        setProps view
        DateFieldElement(view.props |> Seq.toList) :> TerminalElement

    /// <seealso cref="Terminal.Gui.DatePicker"/>
    static member inline datePicker (props:IProperty list) = DatePickerElement(props) :> TerminalElement
    static member inline datePicker (children:TerminalElement list) =
        let view = datePicker()
        view.children children
        DatePickerElement(view.props |> Seq.toList) :> TerminalElement
    static member inline datePicker (x:int, y:int, title: string) =
        let setProps = fun (p: datePicker) ->
            p.x (Pos.Absolute(x))
            p.y (Pos.Absolute(y))
            p.title title

        let view = datePicker()
        setProps view
        DatePickerElement(view.props |> Seq.toList) :> TerminalElement

    /// <seealso cref="Terminal.Gui.Dialog"/>
    static member inline dialog (props:IProperty list) = DialogElement(props) :> TerminalElement
    static member inline dialog (children:TerminalElement list) =
        let view = dialog()
        view.children children
        DialogElement(view.props |> Seq.toList) :> TerminalElement
    static member inline dialog (x:int, y:int, title: string) =
        let setProps = fun (p: dialog) ->
            p.x (Pos.Absolute(x))
            p.y (Pos.Absolute(y))
            p.title title

        let view = dialog()
        setProps view
        DialogElement(view.props |> Seq.toList) :> TerminalElement

    /// <seealso cref="Terminal.Gui.FileDialog"/>
    static member inline fileDialog (props:IProperty list) = FileDialogElement(props) :> TerminalElement
    static member inline fileDialog (children:TerminalElement list) =
        let view = fileDialog()
        view.children children
        FileDialogElement(view.props |> Seq.toList) :> TerminalElement
    static member inline fileDialog (x:int, y:int, title: string) =
        let setProps = fun (p: fileDialog) ->
            p.x (Pos.Absolute(x))
            p.y (Pos.Absolute(y))
            p.title title

        let view = fileDialog()
        setProps view
        FileDialogElement(view.props |> Seq.toList) :> TerminalElement

    /// <seealso cref="Terminal.Gui.FrameView"/>
    static member inline frameView (props:IProperty list) = FrameViewElement(props) :> TerminalElement
    static member inline frameView (children:TerminalElement list) =
        let view = frameView()
        view.children children
        FrameViewElement(view.props |> Seq.toList) :> TerminalElement
    static member inline frameView (x:int, y:int, title: string) =
        let setProps = fun (p: frameView) ->
            p.x (Pos.Absolute(x))
            p.y (Pos.Absolute(y))
            p.title title

        let view = frameView()
        setProps view
        FrameViewElement(view.props |> Seq.toList) :> TerminalElement

    /// <seealso cref="Terminal.Gui.GraphView"/>
    static member inline graphView (props:IProperty list) = GraphViewElement(props) :> TerminalElement
    static member inline graphView (children:TerminalElement list) =
        let view = graphView()
        view.children children
        GraphViewElement(view.props |> Seq.toList) :> TerminalElement
    static member inline graphView (x:int, y:int, title: string) =
        let setProps = fun (p: graphView) ->
            p.x (Pos.Absolute(x))
            p.y (Pos.Absolute(y))
            p.title title

        let view = graphView()
        setProps view
        GraphViewElement(view.props |> Seq.toList) :> TerminalElement

    /// <seealso cref="Terminal.Gui.HexView"/>
    static member inline hexView (props:IProperty list) = HexViewElement(props) :> TerminalElement
    static member inline hexView (children:TerminalElement list) =
        let view = hexView()
        view.children children
        HexViewElement(view.props |> Seq.toList) :> TerminalElement
    static member inline hexView (x:int, y:int, title: string) =
        let setProps = fun (p: hexView) ->
            p.x (Pos.Absolute(x))
            p.y (Pos.Absolute(y))
            p.title title

        let view = hexView()
        setProps view
        HexViewElement(view.props |> Seq.toList) :> TerminalElement

    /// <seealso cref="Terminal.Gui.Label"/>
    static member inline label (props:IProperty list) = LabelElement(props) :> TerminalElement
    static member inline label (children:TerminalElement list) =
        let view = label()
        view.children children
        LabelElement(view.props |> Seq.toList) :> TerminalElement
    static member inline label (x:int, y:int, title: string) =
        let setProps = fun (p: label) ->
            p.x (Pos.Absolute(x))
            p.y (Pos.Absolute(y))
            p.title title

        let view = label()
        setProps view
        LabelElement(view.props |> Seq.toList) :> TerminalElement

    /// <seealso cref="Terminal.Gui.LegendAnnotation"/>
    static member inline legendAnnotation (props:IProperty list) = LegendAnnotationElement(props) :> TerminalElement
    static member inline legendAnnotation (children:TerminalElement list) =
        let view = legendAnnotation()
        view.children children
        LegendAnnotationElement(view.props |> Seq.toList) :> TerminalElement
    static member inline legendAnnotation (x:int, y:int, title: string) =
        let setProps = fun (p: legendAnnotation) ->
            p.x (Pos.Absolute(x))
            p.y (Pos.Absolute(y))
            p.title title

        let view = legendAnnotation()
        setProps view
        LegendAnnotationElement(view.props |> Seq.toList) :> TerminalElement

    /// <seealso cref="Terminal.Gui.Line"/>
    static member inline line (props:IProperty list) = LineElement(props) :> TerminalElement
    static member inline line (children:TerminalElement list) =
        let view = line()
        view.children children
        LineElement(view.props |> Seq.toList) :> TerminalElement
    static member inline line (x:int, y:int, title: string) =
        let setProps = fun (p: line) ->
            p.x (Pos.Absolute(x))
            p.y (Pos.Absolute(y))
            p.title title

        let view = line()
        setProps view
        LineElement(view.props |> Seq.toList) :> TerminalElement

    /// <seealso cref="Terminal.Gui.LineView"/>
    static member inline lineView (props:IProperty list) = LineViewElement(props) :> TerminalElement
    static member inline lineView (children:TerminalElement list) =
        let view = lineView()
        view.children children
        LineViewElement(view.props |> Seq.toList) :> TerminalElement
    static member inline lineView (x:int, y:int, title: string) =
        let setProps = fun (p: lineView) ->
            p.x (Pos.Absolute(x))
            p.y (Pos.Absolute(y))
            p.title title

        let view = lineView()
        setProps view
        LineViewElement(view.props |> Seq.toList) :> TerminalElement

    /// <seealso cref="Terminal.Gui.ListView"/>
    static member inline listView (props:IProperty list) = ListViewElement(props) :> TerminalElement
    static member inline listView (children:TerminalElement list) =
        let view = listView()
        view.children children
        ListViewElement(view.props |> Seq.toList) :> TerminalElement
    static member inline listView (x:int, y:int, title: string) =
        let setProps = fun (p: listView) ->
            p.x (Pos.Absolute(x))
            p.y (Pos.Absolute(y))
            p.title title

        let view = listView()
        setProps view
        ListViewElement(view.props |> Seq.toList) :> TerminalElement

    /// <seealso cref="Terminal.Gui.Margin"/>
    static member inline margin (props:IProperty list) = MarginElement(props) :> TerminalElement
    static member inline margin (children:TerminalElement list) =
        let view = margin()
        view.children children
        MarginElement(view.props |> Seq.toList) :> TerminalElement
    static member inline margin (x:int, y:int, title: string) =
        let setProps = fun (p: margin) ->
            p.x (Pos.Absolute(x))
            p.y (Pos.Absolute(y))
            p.title title

        let view = margin()
        setProps view
        MarginElement(view.props |> Seq.toList) :> TerminalElement

    /// <seealso cref="Terminal.Gui.MenuBarv2"/>
    static member inline menuBarv2 (props:IProperty list) = MenuBarv2Element(props) :> TerminalElement
    static member inline menuBarv2 (set: menuBarv2 -> unit) =
        let props = menuBarv2()
        set props
        MenuBarv2Element(props.props |> Seq.toList) :> TerminalElement
    static member inline menuBarv2 (children:TerminalElement list) =
        let view = menuBarv2()
        view.children children
        MenuBarv2Element(view.props |> Seq.toList) :> TerminalElement
    static member inline menuBarv2 (x:int, y:int, title: string) =
        let setProps = fun (p: menuBarv2) ->
            p.x (Pos.Absolute(x))
            p.y (Pos.Absolute(y))
            p.title title

        let view = menuBarv2()
        setProps view
        MenuBarv2Element(view.props |> Seq.toList) :> TerminalElement

    /// <seealso cref="Terminal.Gui.Menuv2"/>
    static member inline menuv2 (props:IProperty list) = Menuv2Element(props)
    static member inline menuv2 (set: menuv2 -> unit) =
        let props = menuv2()
        set(props)
        Menuv2Element(props.props |> Seq.toList)
    static member inline menuv2 (children:TerminalElement list) =
        let view = menuv2()
        view.children children
        Menuv2Element(view.props |> Seq.toList) :> TerminalElement
    static member inline menuv2 (x:int, y:int, title: string) =
        let setProps = fun (p: menuv2) ->
            p.x (Pos.Absolute(x))
            p.y (Pos.Absolute(y))
            p.title title

        let view = menuv2()
        setProps view
        Menuv2Element(view.props |> Seq.toList) :> TerminalElement

    static member inline menuBarItemv2 (set: menuBarItemv2 -> unit) =
        let props = menuBarItemv2()
        set(props)
        MenuBarItemv2Element(props.props |> Seq.toList)

    static member inline popoverMenu (set: popoverMenu -> unit) =
        let props = popoverMenu()
        set(props)
        PopoverMenuElement(props.props |> Seq.toList)

    static member inline menuItemv2 (props:IProperty list) =
        MenuItemv2Element(props)

    static member inline menuItemv2 (set: menuItemv2 -> unit) =
        let props = menuItemv2()
        set(props)
        MenuItemv2Element(props.props |> Seq.toList)

    /// <seealso cref="Terminal.Gui.NumericUpDown"/>
    static member inline numericUpDown (props:IProperty list) = NumericUpDownElement(props) :> TerminalElement
    static member inline numericUpDown (children:TerminalElement list) =
        let view = numericUpDown()
        view.children children
        NumericUpDownElement(view.props |> Seq.toList) :> TerminalElement
    static member inline numericUpDown (x:int, y:int, title: string) =
        let setProps = fun (p: numericUpDown) ->
            p.x (Pos.Absolute(x))
            p.y (Pos.Absolute(y))
            p.title title

        let view = numericUpDown()
        setProps view
        NumericUpDownElement(view.props |> Seq.toList) :> TerminalElement

    /// <seealso cref="Terminal.Gui.NumericUpDown"/>
    static member inline numericUpDown<'a> (props:IProperty list) = NumericUpDownElement<'a>(props) :> TerminalElement
    static member inline numericUpDown<'a> (children:TerminalElement list) =
        let view = numericUpDown()
        view.children children
        NumericUpDownElement<'a>(view.props |> Seq.toList) :> TerminalElement
    static member inline numericUpDown<'a> (x:int, y:int, title: string) =
        let setProps = fun (p: numericUpDown) ->
            p.x (Pos.Absolute(x))
            p.y (Pos.Absolute(y))
            p.title title

        let props = numericUpDown()
        setProps props
        NumericUpDownElement<'a>(props.props |> Seq.toList) :> TerminalElement

    /// <seealso cref="Terminal.Gui.OpenDialog"/>
    static member inline openDialog (props:IProperty list) = OpenDialogElement(props) :> TerminalElement
    static member inline openDialog (children:TerminalElement list) =
        let view = openDialog()
        view.children children
        OpenDialogElement(view.props |> Seq.toList) :> TerminalElement
    static member inline openDialog (x:int, y:int, title: string) =
        let setProps = fun (p: openDialog) ->
            p.x (Pos.Absolute(x))
            p.y (Pos.Absolute(y))
            p.title title

        let view = openDialog()
        setProps view
        OpenDialogElement(view.props |> Seq.toList) :> TerminalElement

    /// <seealso cref="Terminal.Gui.Padding"/>
    static member inline padding (props:IProperty list) = PaddingElement(props) :> TerminalElement
    static member inline padding (children:TerminalElement list) =
        let view = padding()
        view.children children
        PaddingElement(view.props |> Seq.toList) :> TerminalElement
    static member inline padding (x:int, y:int, title: string) =
        let setProps = fun (p: padding) ->
            p.x (Pos.Absolute(x))
            p.y (Pos.Absolute(y))
            p.title title

        let view = padding()
        setProps view
        PaddingElement(view.props |> Seq.toList) :> TerminalElement

    /// <seealso cref="Terminal.Gui.ProgressBar"/>
    static member inline progressBar (props:IProperty list) = ProgressBarElement(props) :> TerminalElement
    static member inline progressBar (children:TerminalElement list) =
        let view = progressBar()
        view.children children
        ProgressBarElement(view.props |> Seq.toList) :> TerminalElement
    static member inline progressBar (x:int, y:int, title: string) =
        let setProps = fun (p: progressBar) ->
            p.x (Pos.Absolute(x))
            p.y (Pos.Absolute(y))
            p.title title

        let view = progressBar()
        setProps view
        ProgressBarElement(view.props |> Seq.toList) :> TerminalElement

    /// <seealso cref="Terminal.Gui.RadioGroup"/>
    static member inline radioGroup (props:IProperty list) = RadioGroupElement(props) :> TerminalElement
    static member inline radioGroup (children:TerminalElement list) =
        let view = radioGroup()
        view.children children
        RadioGroupElement(view.props |> Seq.toList) :> TerminalElement
    static member inline radioGroup (x:int, y:int, title: string) =
        let setProps = fun (p: radioGroup) ->
            p.x (Pos.Absolute(x))
            p.y (Pos.Absolute(y))
            p.title title

        let view = radioGroup()
        setProps view
        RadioGroupElement(view.props |> Seq.toList) :> TerminalElement

    /// <seealso cref="Terminal.Gui.SaveDialog"/>
    static member inline saveDialog (props:IProperty list) = SaveDialogElement(props) :> TerminalElement
    static member inline saveDialog (children:TerminalElement list) =
        let view = saveDialog()
        view.children children
        SaveDialogElement(view.props |> Seq.toList) :> TerminalElement
    static member inline saveDialog (x:int, y:int, title: string) =
        let setProps = fun (p: saveDialog) ->
            p.x (Pos.Absolute(x))
            p.y (Pos.Absolute(y))
            p.title title

        let view = saveDialog()
        setProps view
        SaveDialogElement(view.props |> Seq.toList) :> TerminalElement

    /// <seealso cref="Terminal.Gui.ScrollBar"/>
    static member inline scrollBar (props:IProperty list) = ScrollBarElement(props) :> TerminalElement
    static member inline scrollBar (children:TerminalElement list) =
        let view = scrollBar()
        view.children children
        ScrollBarElement(view.props |> Seq.toList) :> TerminalElement
    static member inline scrollBar (x:int, y:int, title: string) =
        let setProps = fun (p: scrollBar) ->
            p.x (Pos.Absolute(x))
            p.y (Pos.Absolute(y))
            p.title title

        let view = scrollBar()
        setProps view
        ScrollBarElement(view.props |> Seq.toList) :> TerminalElement

    /// <seealso cref="Terminal.Gui.ScrollSlider"/>
    static member inline scrollSlider (props:IProperty list) = ScrollSliderElement(props) :> TerminalElement
    static member inline scrollSlider (children:TerminalElement list) =
        let view = scrollSlider()
        view.children children
        ScrollSliderElement(view.props |> Seq.toList) :> TerminalElement
    static member inline scrollSlider (x:int, y:int, title: string) =
        let setProps = fun (p: scrollSlider) ->
            p.x (Pos.Absolute(x))
            p.y (Pos.Absolute(y))
            p.title title

        let view = scrollSlider()
        setProps view
        ScrollSliderElement(view.props |> Seq.toList) :> TerminalElement

    /// <seealso cref="Terminal.Gui.Shortcut"/>
    static member inline shortcut (props:IProperty list) = ShortcutElement(props) :> TerminalElement
    static member inline shortcut (children:TerminalElement list) =
        let view = shortcut()
        view.children children
        ShortcutElement(view.props |> Seq.toList) :> TerminalElement
    static member inline shortcut (x:int, y:int, title: string) =
        let setProps = fun (p: shortcut) ->
            p.x (Pos.Absolute(x))
            p.y (Pos.Absolute(y))
            p.title title

        let view = shortcut()
        setProps view
        ShortcutElement(view.props |> Seq.toList) :> TerminalElement

    /// <seealso cref="Terminal.Gui.Slider"/>
    static member inline slider (props:IProperty list) = SliderElement(props) :> TerminalElement
    static member inline slider (children:TerminalElement list) =
        let view = slider()
        view.children children
        SliderElement(view.props |> Seq.toList) :> TerminalElement
    static member inline slider (x:int, y:int, title: string) =
        let setProps = fun (p: slider) ->
            p.x (Pos.Absolute(x))
            p.y (Pos.Absolute(y))
            p.title title

        let view = slider()
        setProps view
        SliderElement(view.props |> Seq.toList) :> TerminalElement

    /// <seealso cref="Terminal.Gui.Slider"/>
    static member inline slider<'a> (props:IProperty list) = SliderElement<'a>(props) :> TerminalElement
    static member inline slider<'a> (children:TerminalElement list) =
        let view = slider()
        view.children children
        SliderElement<'a>(view.props |> Seq.toList) :> TerminalElement
    static member inline slider<'a> (x:int, y:int, title: string) =
        let setProps = fun (p: slider) ->
            p.x (Pos.Absolute(x))
            p.y (Pos.Absolute(y))
            p.title title

        let props = slider()
        setProps props
        SliderElement<'a>(props.props |> Seq.toList) :> TerminalElement

    /// <seealso cref="Terminal.Gui.SpinnerView"/>
    static member inline spinnerView (props:IProperty list) = SpinnerViewElement(props) :> TerminalElement
    static member inline spinnerView (children:TerminalElement list) =
        let view = spinnerView()
        view.children children
        SpinnerViewElement(view.props |> Seq.toList) :> TerminalElement
    static member inline spinnerView (x:int, y:int, title: string) =
        let setProps = fun (p: spinnerView) ->
            p.x (Pos.Absolute(x))
            p.y (Pos.Absolute(y))
            p.title title

        let view = spinnerView()
        setProps view
        SpinnerViewElement(view.props |> Seq.toList) :> TerminalElement

    /// <seealso cref="Terminal.Gui.StatusBar"/>
    static member inline statusBar (props:IProperty list) = StatusBarElement(props) :> TerminalElement
    static member inline statusBar (children:TerminalElement list) =
        let view = statusBar()
        view.children children
        StatusBarElement(view.props |> Seq.toList) :> TerminalElement
    static member inline statusBar (x:int, y:int, title: string) =
        let setProps = fun (p: statusBar) ->
            p.x (Pos.Absolute(x))
            p.y (Pos.Absolute(y))
            p.title title

        let view = statusBar()
        setProps view
        StatusBarElement(view.props |> Seq.toList) :> TerminalElement

    /// <seealso cref="Terminal.Gui.Tab"/>
    static member inline tab (props:IProperty list) = TabElement(props) :> TerminalElement
    static member inline tab (children:TerminalElement list) =
        let view = tab()
        view.children children
        TabElement(view.props |> Seq.toList) :> TerminalElement
    static member inline tab (x:int, y:int, title: string) =
        let setProps = fun (p: tab) ->
            p.x (Pos.Absolute(x))
            p.y (Pos.Absolute(y))
            p.title title

        let view = tab()
        setProps view
        TabElement(view.props |> Seq.toList) :> TerminalElement

    /// <seealso cref="Terminal.Gui.TabView"/>
    static member inline tabView (props:IProperty list) = TabViewElement(props) :> TerminalElement
    static member inline tabView (children:TerminalElement list) =
        let view = tabView()
        view.children children
        TabViewElement(view.props |> Seq.toList) :> TerminalElement
    static member inline tabView (x:int, y:int, title: string) =
        let setProps = fun (p: tabView) ->
            p.x (Pos.Absolute(x))
            p.y (Pos.Absolute(y))
            p.title title

        let view = tabView()
        setProps view
        TabViewElement(view.props |> Seq.toList) :> TerminalElement

    /// <seealso cref="Terminal.Gui.TableView"/>
    static member inline tableView (props:IProperty list) = TableViewElement(props) :> TerminalElement
    static member inline tableView (children:TerminalElement list) =
        let view = tableView()
        view.children children
        TableViewElement(view.props |> Seq.toList) :> TerminalElement
    static member inline tableView (x:int, y:int, title: string) =
        let setProps = fun (p: tableView) ->
            p.x (Pos.Absolute(x))
            p.y (Pos.Absolute(y))
            p.title title

        let view = tableView()
        setProps view
        TableViewElement(view.props |> Seq.toList) :> TerminalElement

    /// <seealso cref="Terminal.Gui.TextField"/>
    static member inline textField (props:IProperty list) = TextFieldElement(props) :> TerminalElement
    static member inline textField (children:TerminalElement list) =
        let view = textField()
        view.children children
        TextFieldElement(view.props |> Seq.toList) :> TerminalElement
    static member inline textField (x:int, y:int, title: string) =
        let setProps = fun (p: textField) ->
            p.x (Pos.Absolute(x))
            p.y (Pos.Absolute(y))
            p.title title

        let view = textField()
        setProps view
        TextFieldElement(view.props |> Seq.toList) :> TerminalElement

    /// <seealso cref="Terminal.Gui.TextValidateField"/>
    static member inline textValidateField (props:IProperty list) = TextValidateFieldElement(props) :> TerminalElement
    static member inline textValidateField (children:TerminalElement list) =
        let view = textValidateField()
        view.children children
        TextValidateFieldElement(view.props |> Seq.toList) :> TerminalElement
    static member inline textValidateField (x:int, y:int, title: string) =
        let setProps = fun (p: textValidateField) ->
            p.x (Pos.Absolute(x))
            p.y (Pos.Absolute(y))
            p.title title

        let view = textValidateField()
        setProps view
        TextValidateFieldElement(view.props |> Seq.toList) :> TerminalElement

    /// <seealso cref="Terminal.Gui.TextView"/>
    static member inline textView (props:IProperty list) = TextViewElement(props) :> TerminalElement
    static member inline textView (children:TerminalElement list) =
        let view = textView()
        view.children children
        TextViewElement(view.props |> Seq.toList) :> TerminalElement
    static member inline textView (x:int, y:int, title: string) =
        let setProps = fun (p: textView) ->
            p.x (Pos.Absolute(x))
            p.y (Pos.Absolute(y))
            p.title title

        let view = textView()
        setProps view
        TextViewElement(view.props |> Seq.toList) :> TerminalElement

    /// <seealso cref="Terminal.Gui.TileView"/>
    static member inline tileView (props:IProperty list) = TileViewElement(props) :> TerminalElement
    static member inline tileView (children:TerminalElement list) =
        let view = tileView()
        view.children children
        TileViewElement(view.props |> Seq.toList) :> TerminalElement
    static member inline tileView (x:int, y:int, title: string) =
        let setProps = fun (p: tileView) ->
            p.x (Pos.Absolute(x))
            p.y (Pos.Absolute(y))
            p.title title

        let view = tileView()
        setProps view
        TileViewElement(view.props |> Seq.toList) :> TerminalElement

    /// <seealso cref="Terminal.Gui.TimeField"/>
    static member inline timeField (props:IProperty list) = TimeFieldElement(props) :> TerminalElement
    static member inline timeField (children:TerminalElement list) =
        let view = timeField()
        view.children children
        TimeFieldElement(view.props |> Seq.toList) :> TerminalElement
    static member inline timeField (x:int, y:int, title: string) =
        let setProps = fun (p: timeField) ->
            p.x (Pos.Absolute(x))
            p.y (Pos.Absolute(y))
            p.title title

        let view = timeField()
        setProps view
        TimeFieldElement(view.props |> Seq.toList) :> TerminalElement

    /// <seealso cref="Terminal.Gui.Toplevel"/>
    static member inline toplevel (props:IProperty list) = ToplevelElement(props) :> TerminalElement
    static member inline toplevel (children:TerminalElement list) =
        let view = toplevel()
        view.children children
        ToplevelElement(view.props |> Seq.toList) :> TerminalElement
    static member inline toplevel (x:int, y:int, title: string) =
        let setProps = fun (p: toplevel) ->
            p.x (Pos.Absolute(x))
            p.y (Pos.Absolute(y))
            p.title title

        let view = toplevel()
        setProps view
        ToplevelElement(view.props |> Seq.toList) :> TerminalElement

    /// <seealso cref="Terminal.Gui.TreeView"/>
    static member inline treeView (props:IProperty list) = TreeViewElement(props) :> TerminalElement
    static member inline treeView (children:TerminalElement list) =
        let view = treeView()
        view.children children
        TreeViewElement(view.props |> Seq.toList) :> TerminalElement
    static member inline treeView (x:int, y:int, title: string) =
        let setProps = fun (p: treeView<_>) ->
            p.x (Pos.Absolute(x))
            p.y (Pos.Absolute(y))
            p.title title

        let view = treeView()
        setProps view
        TreeViewElement(view.props |> Seq.toList) :> TerminalElement

    /// <seealso cref="Terminal.Gui.TreeView"/>
    static member inline treeView<'a when 'a : not struct> (props:IProperty list) = TreeViewElement<'a>(props) :> TerminalElement
    static member inline treeView<'a when 'a : not struct> (children:TerminalElement list) =
        let view = treeView()
        view.children children
        TreeViewElement<'a>(view.props |> Seq.toList) :> TerminalElement
    static member inline treeView<'a when 'a : not struct> (x:int, y:int, title: string) =
        let setProps = fun (p: treeView<_>) ->
            p.x (Pos.Absolute(x))
            p.y (Pos.Absolute(y))
            p.title title

        let props = treeView()
        setProps props
        TreeViewElement<'a>(props.props |> Seq.toList) :> TerminalElement

    /// <seealso cref="Terminal.Gui.Window"/>
    static member inline window (props:IProperty list) = WindowElement(props) :> TerminalElement
    static member inline window (children:TerminalElement list) =
        let view = window()
        view.children children
        WindowElement(view.props |> Seq.toList) :> TerminalElement
    static member inline window (x:int, y:int, title: string) =
        let setProps = fun (p: window) ->
            p.x (Pos.Absolute(x))
            p.y (Pos.Absolute(y))
            p.title title

        let view = window()
        setProps view
        WindowElement(view.props |> Seq.toList) :> TerminalElement

    /// <seealso cref="Terminal.Gui.Wizard"/>
    static member inline wizard (props:IProperty list) = WizardElement(props) :> TerminalElement
    static member inline wizard (children:TerminalElement list) =
        let view = wizard()
        view.children children
        WizardElement(view.props |> Seq.toList) :> TerminalElement
    static member inline wizard (x:int, y:int, title: string) =
        let setProps = fun (p: wizard) ->
            p.x (Pos.Absolute(x))
            p.y (Pos.Absolute(y))
            p.title title

        let view = wizard()
        setProps view
        WizardElement(view.props |> Seq.toList) :> TerminalElement

    /// <seealso cref="Terminal.Gui.WizardStep"/>
    static member inline wizardStep (props:IProperty list) = WizardStepElement(props) :> TerminalElement
    static member inline wizardStep (children:TerminalElement list) =
        let view = wizardStep()
        view.children children
        WizardStepElement(view.props |> Seq.toList) :> TerminalElement
    static member inline wizardStep (x:int, y:int, title: string) =
        let setProps = fun (p: wizardStep) ->
            p.x (Pos.Absolute(x))
            p.y (Pos.Absolute(y))
            p.title title

        let view = wizardStep()
        setProps view
        WizardStepElement(view.props |> Seq.toList) :> TerminalElement



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



