namespace Terminal.Gui.Elmish

open System
open System.Collections.ObjectModel
open Terminal.Gui.ViewBase
open Terminal.Gui.Views

// Adornment
type internal AdornmentElement(props: Props) =
  inherit ViewTerminalElement(props)

  override _.name = $"Adornment"

  override _.newView() = new Adornment()

  override _.setProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.setProps (terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> Adornment

    // Properties
    props
    |> Props.tryFind PKey.adornment.diagnostics
    |> Option.iter (fun v -> view.Diagnostics <- v)

    props
    |> Props.tryFind PKey.adornment.superViewRendersLineCanvas
    |> Option.iter (fun v -> view.SuperViewRendersLineCanvas <- v)

    props
    |> Props.tryFind PKey.adornment.thickness
    |> Option.iter (fun v -> view.Thickness <- v)

    props
    |> Props.tryFind PKey.adornment.viewport
    |> Option.iter (fun v -> view.Viewport <- v)
    // Events
    terminalElement.trySetEventHandler(PKey.adornment.thicknessChanged, view.ThicknessChanged)

  override _.removeProps(terminalElement: IInternalTerminalElement, props: Props) =

    let terminalElement = terminalElement :?> TerminalElement

    base.removeProps (terminalElement, props)
    let view = terminalElement.View :?> Adornment
    // Properties
    props
    |> Props.tryFind PKey.adornment.diagnostics
    |> Option.iter (fun _ -> view.Diagnostics <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.adornment.superViewRendersLineCanvas
    |> Option.iter (fun _ -> view.SuperViewRendersLineCanvas <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.adornment.thickness
    |> Option.iter (fun _ -> view.Thickness <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.adornment.viewport
    |> Option.iter (fun _ -> view.Viewport <- Unchecked.defaultof<_>)
    // Events
    terminalElement.tryRemoveEventHandler PKey.adornment.thicknessChanged


// Bar
type internal BarElement(props: Props) =
  inherit ViewTerminalElement(props)

  override _.removeProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.removeProps (terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> Bar

    // Interfaces
    OrientationInterface.removeProps terminalElement view props

    // Properties
    props
    |> Props.tryFind PKey.bar.alignmentModes
    |> Option.iter (fun _ -> view.AlignmentModes <- Unchecked.defaultof<_>)

  override _.name = $"Bar"

  override _.setProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.setProps (terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> Bar

    // Interfaces
    OrientationInterface.setProps terminalElement view props

    // Properties
    props
    |> Props.tryFind PKey.bar.alignmentModes
    |> Option.iter (fun v -> view.AlignmentModes <- v)

  override this.newView() = new Bar()


// Border
type internal BorderElement(props: Props) =
  inherit ViewTerminalElement(props)

  override _.removeProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.removeProps (terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> Border
    // Properties
    props
    |> Props.tryFind PKey.border.lineStyle
    |> Option.iter (fun _ -> view.LineStyle <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.border.settings
    |> Option.iter (fun _ -> view.Settings <- Unchecked.defaultof<_>)

  override _.name = $"Border"

  override _.setProps(elementData: IInternalTerminalElement, props: Props) =
    base.setProps (elementData, props)

    let view = elementData.View :?> Border

    // Properties
    props
    |> Props.tryFind PKey.border.lineStyle
    |> Option.iter (fun v -> view.LineStyle <- v)

    props
    |> Props.tryFind PKey.border.settings
    |> Option.iter (fun v -> view.Settings <- v)

  override this.newView() = new Border()

// Button
type internal ButtonElement(props: Props) =
  inherit ViewTerminalElement(props)

  override _.removeProps(elementData: IInternalTerminalElement, props: Props) =
    base.removeProps (elementData, props)
    let view = elementData.View :?> Button
    // Properties
    props
    |> Props.tryFind PKey.button.hotKeySpecifier
    |> Option.iter (fun _ -> elementData.View.HotKeySpecifier <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.button.isDefault
    |> Option.iter (fun _ -> view.IsDefault <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.button.noDecorations
    |> Option.iter (fun _ -> view.NoDecorations <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.button.noPadding
    |> Option.iter (fun _ -> view.NoPadding <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.button.text
    |> Option.iter (fun _ -> view.Text <- Unchecked.defaultof<_>)
    // Events
    props
    |> Props.tryFind PKey.button.wantContinuousButtonPressed
    |> Option.iter (fun _ -> view.WantContinuousButtonPressed <- Unchecked.defaultof<_>)

  override _.name = $"Button"

  override _.setProps(elementData: IInternalTerminalElement, props: Props) =
    base.setProps (elementData, props)

    let view = elementData.View :?> Button

    // Properties
    props
    |> Props.tryFind PKey.button.hotKeySpecifier
    |> Option.iter (fun v -> elementData.View.HotKeySpecifier <- v)

    props
    |> Props.tryFind PKey.button.isDefault
    |> Option.iter (fun v -> view.IsDefault <- v)

    props
    |> Props.tryFind PKey.button.noDecorations
    |> Option.iter (fun v -> view.NoDecorations <- v)

    props
    |> Props.tryFind PKey.button.noPadding
    |> Option.iter (fun v -> view.NoPadding <- v)

    props
    |> Props.tryFind PKey.button.text
    |> Option.iter (fun v -> view.Text <- v)
    // Events
    props
    |> Props.tryFind PKey.button.wantContinuousButtonPressed
    |> Option.iter (fun v -> view.WantContinuousButtonPressed <- v)

  override this.newView() = new Button()


// CheckBox
type internal CheckBoxElement(props: Props) =
  inherit ViewTerminalElement(props)

  override _.removeProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.removeProps (terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> CheckBox

    // Properties
    props
    |> Props.tryFind PKey.checkBox.allowCheckStateNone
    |> Option.iter (fun _ -> view.AllowCheckStateNone <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.checkBox.checkedState
    |> Option.iter (fun _ -> view.CheckedState <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.checkBox.hotKeySpecifier
    |> Option.iter (fun _ -> view.HotKeySpecifier <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.checkBox.radioStyle
    |> Option.iter (fun _ -> view.RadioStyle <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.checkBox.text
    |> Option.iter (fun _ -> view.Text <- Unchecked.defaultof<_>)
    // Events
    terminalElement.tryRemoveEventHandler PKey.checkBox.checkedStateChanging

    terminalElement.tryRemoveEventHandler PKey.checkBox.checkedStateChanged

  override _.name = $"CheckBox"

  override _.setProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.setProps (terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> CheckBox

    // Properties
    props
    |> Props.tryFind PKey.checkBox.allowCheckStateNone
    |> Option.iter (fun v -> view.AllowCheckStateNone <- v)

    props
    |> Props.tryFind PKey.checkBox.checkedState
    |> Option.iter (fun v -> view.CheckedState <- v)

    props
    |> Props.tryFind PKey.checkBox.hotKeySpecifier
    |> Option.iter (fun v -> view.HotKeySpecifier <- v)

    props
    |> Props.tryFind PKey.checkBox.radioStyle
    |> Option.iter (fun v -> view.RadioStyle <- v)

    props
    |> Props.tryFind PKey.checkBox.text
    |> Option.iter (fun v -> view.Text <- v)
    // Events
    terminalElement.trySetEventHandler(PKey.checkBox.checkedStateChanging, view.CheckedStateChanging)

    terminalElement.trySetEventHandler(PKey.checkBox.checkedStateChanged, view.CheckedStateChanged)


  override this.newView() = new CheckBox()


// ColorPicker
type internal ColorPickerElement(props: Props) =
  inherit ViewTerminalElement(props)

  override _.removeProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.removeProps (terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> ColorPicker

    // Properties
    props
    |> Props.tryFind PKey.colorPicker.selectedColor
    |> Option.iter (fun _ -> view.SelectedColor <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.colorPicker.style
    |> Option.iter (fun _ -> view.Style <- Unchecked.defaultof<_>)
    // Events
    terminalElement.tryRemoveEventHandler PKey.colorPicker.colorChanged

  override _.name = $"ColorPicker"

  override _.setProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.setProps (terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> ColorPicker

    // Properties
    props
    |> Props.tryFind PKey.colorPicker.selectedColor
    |> Option.iter (fun v -> view.SelectedColor <- v)

    props
    |> Props.tryFind PKey.colorPicker.style
    |> Option.iter (fun v -> view.Style <- v)
    // Events
    terminalElement.trySetEventHandler(PKey.colorPicker.colorChanged, view.ColorChanged)


  override this.newView() = new ColorPicker()


// ColorPicker16
type internal ColorPicker16Element(props: Props) =
  inherit ViewTerminalElement(props)

  override _.removeProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.removeProps (terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> ColorPicker16

    // Properties
    props
    |> Props.tryFind PKey.colorPicker16.boxHeight
    |> Option.iter (fun _ -> view.BoxHeight <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.colorPicker16.boxWidth
    |> Option.iter (fun _ -> view.BoxWidth <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.colorPicker16.cursor
    |> Option.iter (fun _ -> view.Cursor <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.colorPicker16.selectedColor
    |> Option.iter (fun _ -> view.SelectedColor <- Unchecked.defaultof<_>)
    // Events
    terminalElement.tryRemoveEventHandler PKey.colorPicker16.colorChanged

  override _.name = $"ColorPicker16"

  override _.setProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.setProps (terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> ColorPicker16

    // Properties
    props
    |> Props.tryFind PKey.colorPicker16.boxHeight
    |> Option.iter (fun v -> view.BoxHeight <- v)

    props
    |> Props.tryFind PKey.colorPicker16.boxWidth
    |> Option.iter (fun v -> view.BoxWidth <- v)

    props
    |> Props.tryFind PKey.colorPicker16.cursor
    |> Option.iter (fun v -> view.Cursor <- v)

    props
    |> Props.tryFind PKey.colorPicker16.selectedColor
    |> Option.iter (fun v -> view.SelectedColor <- v)
    // Events
    terminalElement.trySetEventHandler(PKey.colorPicker16.colorChanged, view.ColorChanged)


  override this.newView() = new ColorPicker16()


// ComboBox
type internal ComboBoxElement(props: Props) =
  inherit ViewTerminalElement(props)

  override _.removeProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.removeProps (terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> ComboBox

    // Properties
    props
    |> Props.tryFind PKey.comboBox.hideDropdownListOnClick
    |> Option.iter (fun _ -> view.HideDropdownListOnClick <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.comboBox.readOnly
    |> Option.iter (fun _ -> view.ReadOnly <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.comboBox.searchText
    |> Option.iter (fun _ -> view.SearchText <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.comboBox.selectedItem
    |> Option.iter (fun _ -> view.SelectedItem <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.comboBox.source
    |> Option.iter (fun _ -> view.SetSource Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.comboBox.text
    |> Option.iter (fun _ -> view.Text <- Unchecked.defaultof<_>)
    // Events
    terminalElement.tryRemoveEventHandler PKey.comboBox.collapsed

    terminalElement.tryRemoveEventHandler PKey.comboBox.expanded

    terminalElement.tryRemoveEventHandler PKey.comboBox.openSelectedItem

    terminalElement.tryRemoveEventHandler PKey.comboBox.selectedItemChanged

  override _.name = $"ComboBox"

  override _.setProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.setProps (terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> ComboBox

    // Properties
    props
    |> Props.tryFind PKey.comboBox.hideDropdownListOnClick
    |> Option.iter (fun v -> view.HideDropdownListOnClick <- v)

    props
    |> Props.tryFind PKey.comboBox.readOnly
    |> Option.iter (fun v -> view.ReadOnly <- v)

    props
    |> Props.tryFind PKey.comboBox.searchText
    |> Option.iter (fun v -> view.SearchText <- v)

    props
    |> Props.tryFind PKey.comboBox.selectedItem
    |> Option.iter (fun v -> view.SelectedItem <- v)

    props
    |> Props.tryFind PKey.comboBox.source
    |> Option.iter (fun v -> view.SetSource(ObservableCollection(v)))

    props
    |> Props.tryFind PKey.comboBox.text
    |> Option.iter (fun v -> terminalElement.View.Text <- v)
    // Events
    terminalElement.trySetEventHandler(PKey.comboBox.collapsed, view.Collapsed)

    terminalElement.trySetEventHandler(PKey.comboBox.expanded, view.Expanded)

    terminalElement.trySetEventHandler(PKey.comboBox.openSelectedItem, view.OpenSelectedItem)

    terminalElement.trySetEventHandler(PKey.comboBox.selectedItemChanged, view.SelectedItemChanged)


  override this.newView() = new ComboBox()


// DateField
type internal DateFieldElement(props: Props) =
  inherit ViewTerminalElement(props)

  override _.removeProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.removeProps (terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> DateField

    // Properties
    props
    |> Props.tryFind PKey.dateField.culture
    |> Option.iter (fun _ -> view.Culture <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.dateField.cursorPosition
    |> Option.iter (fun _ -> view.CursorPosition <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.dateField.date
    |> Option.iter (fun _ -> view.Date <- Unchecked.defaultof<_>)
    // Events
    terminalElement.tryRemoveEventHandler PKey.dateField.dateChanged

  override _.name = $"DateField"

  override _.setProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.setProps (terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> DateField

    // Properties
    props
    |> Props.tryFind PKey.dateField.culture
    |> Option.iter (fun v -> view.Culture <- v)

    props
    |> Props.tryFind PKey.dateField.cursorPosition
    |> Option.iter (fun v -> view.CursorPosition <- v)

    props
    |> Props.tryFind PKey.dateField.date
    |> Option.iter (fun v -> view.Date <- v)
    // Events
    terminalElement.trySetEventHandler(PKey.dateField.dateChanged, view.DateChanged)


  override this.newView() = new DateField()


// DatePicker
type internal DatePickerElement(props: Props) =
  inherit ViewTerminalElement(props)

  override _.removeProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.removeProps (terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> DatePicker

    // Properties
    props
    |> Props.tryFind PKey.datePicker.culture
    |> Option.iter (fun _ -> view.Culture <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.datePicker.date
    |> Option.iter (fun _ -> view.Date <- Unchecked.defaultof<_>)

  override _.name = $"DatePicker"

  override _.setProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.setProps (terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> DatePicker

    // Properties
    props
    |> Props.tryFind PKey.datePicker.culture
    |> Option.iter (fun v -> view.Culture <- v)

    props
    |> Props.tryFind PKey.datePicker.date
    |> Option.iter (fun v -> view.Date <- v)


  override this.newView() = new DatePicker()


// Dialog
type internal DialogElement(props: Props) =
  inherit ViewTerminalElement(props)

  override _.removeProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.removeProps (terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> Dialog

    // Properties
    props
    |> Props.tryFind PKey.dialog.buttonAlignment
    |> Option.iter (fun _ -> view.ButtonAlignment <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.dialog.buttonAlignmentModes
    |> Option.iter (fun _ -> view.ButtonAlignmentModes <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.dialog.canceled
    |> Option.iter (fun _ -> view.Canceled <- Unchecked.defaultof<_>)

  override _.name = $"Dialog"

  override _.setProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.setProps (terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> Dialog

    // Properties
    props
    |> Props.tryFind PKey.dialog.buttonAlignment
    |> Option.iter (fun v -> view.ButtonAlignment <- v)

    props
    |> Props.tryFind PKey.dialog.buttonAlignmentModes
    |> Option.iter (fun v -> view.ButtonAlignmentModes <- v)

    props
    |> Props.tryFind PKey.dialog.canceled
    |> Option.iter (fun v -> view.Canceled <- v)


  override this.newView() = new Dialog()


// FileDialog
type internal FileDialogElement(props: Props) =
  inherit ViewTerminalElement(props)

  override _.removeProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.removeProps (terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> FileDialog

    // Properties
    props
    |> Props.tryFind PKey.fileDialog.allowedTypes
    |> Option.iter (fun _ -> view.AllowedTypes <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.fileDialog.allowsMultipleSelection
    |> Option.iter (fun _ -> view.AllowsMultipleSelection <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.fileDialog.fileOperationsHandler
    |> Option.iter (fun _ -> view.FileOperationsHandler <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.fileDialog.mustExist
    |> Option.iter (fun _ -> view.MustExist <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.fileDialog.openMode
    |> Option.iter (fun _ -> view.OpenMode <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.fileDialog.path
    |> Option.iter (fun _ -> view.Path <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.fileDialog.searchMatcher
    |> Option.iter (fun _ -> view.SearchMatcher <- Unchecked.defaultof<_>)
    // Events
    terminalElement.tryRemoveEventHandler PKey.fileDialog.filesSelected

  override _.name = $"FileDialog"

  override _.setProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.setProps (terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> FileDialog

    // Properties
    props
    |> Props.tryFind PKey.fileDialog.allowedTypes
    |> Option.iter (fun v -> view.AllowedTypes <- Collections.Generic.List<_>(v))

    props
    |> Props.tryFind PKey.fileDialog.allowsMultipleSelection
    |> Option.iter (fun v -> view.AllowsMultipleSelection <- v)

    props
    |> Props.tryFind PKey.fileDialog.fileOperationsHandler
    |> Option.iter (fun v -> view.FileOperationsHandler <- v)

    props
    |> Props.tryFind PKey.fileDialog.mustExist
    |> Option.iter (fun v -> view.MustExist <- v)

    props
    |> Props.tryFind PKey.fileDialog.openMode
    |> Option.iter (fun v -> view.OpenMode <- v)

    props
    |> Props.tryFind PKey.fileDialog.path
    |> Option.iter (fun v -> view.Path <- v)

    props
    |> Props.tryFind PKey.fileDialog.searchMatcher
    |> Option.iter (fun v -> view.SearchMatcher <- v)
    // Events
    terminalElement.trySetEventHandler(PKey.fileDialog.filesSelected, view.FilesSelected)


  override this.newView() = new FileDialog()


// FrameView
type internal FrameViewElement(props: Props) =
  inherit ViewTerminalElement(props)

  override _.removeProps(terminalElement: IInternalTerminalElement, props: Props) = base.removeProps (terminalElement, props)
  // No properties or events FrameView

  override _.name = $"FrameView"

  override _.setProps(terminalElement: IInternalTerminalElement, props: Props) = base.setProps (terminalElement, props)
  // No properties or events FrameView


  override this.newView() = new FrameView()


// GraphView
type internal GraphViewElement(props: Props) =
  inherit ViewTerminalElement(props)

  override _.removeProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.removeProps (terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> GraphView

    // Properties
    props
    |> Props.tryFind PKey.graphView.axisX
    |> Option.iter (fun _ -> view.AxisX <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.graphView.axisY
    |> Option.iter (fun _ -> view.AxisY <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.graphView.cellSize
    |> Option.iter (fun _ -> view.CellSize <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.graphView.graphColor
    |> Option.iter (fun _ -> view.GraphColor <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.graphView.marginBottom
    |> Option.iter (fun _ -> view.MarginBottom <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.graphView.marginLeft
    |> Option.iter (fun _ -> view.MarginLeft <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.graphView.scrollOffset
    |> Option.iter (fun _ -> view.ScrollOffset <- Unchecked.defaultof<_>)

  override _.name = $"GraphView"

  override _.setProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.setProps (terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> GraphView

    // Properties
    props
    |> Props.tryFind PKey.graphView.axisX
    |> Option.iter (fun v -> view.AxisX <- v)

    props
    |> Props.tryFind PKey.graphView.axisY
    |> Option.iter (fun v -> view.AxisY <- v)

    props
    |> Props.tryFind PKey.graphView.cellSize
    |> Option.iter (fun v -> view.CellSize <- v)

    props
    |> Props.tryFind PKey.graphView.graphColor
    |> Option.iter (fun v -> view.GraphColor <- v |> Option.toNullable)

    props
    |> Props.tryFind PKey.graphView.marginBottom
    |> Option.iter (fun v -> view.MarginBottom <- (v |> uint32))

    props
    |> Props.tryFind PKey.graphView.marginLeft
    |> Option.iter (fun v -> view.MarginLeft <- (v |> uint32))

    props
    |> Props.tryFind PKey.graphView.scrollOffset
    |> Option.iter (fun v -> view.ScrollOffset <- v)


  override this.newView() = new GraphView()


// HexView
type internal HexViewElement(props: Props) =
  inherit ViewTerminalElement(props)

  override _.removeProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.removeProps (terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> HexView

    // Properties
    props
    |> Props.tryFind PKey.hexView.address
    |> Option.iter (fun _ -> view.Address <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.hexView.addressWidth
    |> Option.iter (fun _ -> view.AddressWidth <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.hexView.allowEdits
    |> Option.iter (fun _ -> view.BytesPerLine <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.hexView.readOnly
    |> Option.iter (fun _ -> view.ReadOnly <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.hexView.source
    |> Option.iter (fun _ -> view.Source <- Unchecked.defaultof<_>)
    // Events
    terminalElement.tryRemoveEventHandler PKey.hexView.edited

    terminalElement.tryRemoveEventHandler PKey.hexView.positionChanged

  override _.name = $"HexView"

  override _.setProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.setProps (terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> HexView

    // Properties
    props
    |> Props.tryFind PKey.hexView.address
    |> Option.iter (fun v -> view.Address <- v)

    props
    |> Props.tryFind PKey.hexView.addressWidth
    |> Option.iter (fun v -> view.AddressWidth <- v)

    props
    |> Props.tryFind PKey.hexView.allowEdits
    |> Option.iter (fun v -> view.BytesPerLine <- v)

    props
    |> Props.tryFind PKey.hexView.readOnly
    |> Option.iter (fun v -> view.ReadOnly <- v)

    props
    |> Props.tryFind PKey.hexView.source
    |> Option.iter (fun v -> view.Source <- v)
    // Events
    terminalElement.trySetEventHandler(PKey.hexView.edited, view.Edited)

    terminalElement.trySetEventHandler(PKey.hexView.positionChanged, view.PositionChanged)


  override this.newView() = new HexView()


// Label
type internal LabelElement(props: Props) =
  inherit ViewTerminalElement(props)

  override _.removeProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.removeProps (terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> Label

    // Properties
    props
    |> Props.tryFind PKey.label.hotKeySpecifier
    |> Option.iter (fun _ -> view.HotKeySpecifier <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.label.text
    |> Option.iter (fun _ -> view.Text <- Unchecked.defaultof<_>)

  override _.name = $"Label"

  override _.setProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.setProps (terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> Label

    // Properties
    props
    |> Props.tryFind PKey.label.hotKeySpecifier
    |> Option.iter (fun v -> view.HotKeySpecifier <- v)

    props
    |> Props.tryFind PKey.label.text
    |> Option.iter (fun v -> view.Text <- v)


  override this.newView() = new Label()


// LegendAnnotation
type internal LegendAnnotationElement(props: Props) =
  inherit ViewTerminalElement(props)

  override _.removeProps(terminalElement: IInternalTerminalElement, props: Props) = base.removeProps (terminalElement, props)
  // No properties or events LegendAnnotation

  override _.name = $"LegendAnnotation"

  override _.setProps(terminalElement: IInternalTerminalElement, props: Props) = base.setProps (terminalElement, props)
  // No properties or events LegendAnnotation


  override this.newView() = new LegendAnnotation()


// Line
type internal LineElement(props: Props) =
  inherit ViewTerminalElement(props)

  override _.removeProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.removeProps (terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> Line

    // Interfaces
    OrientationInterface.removeProps terminalElement view props

  override _.name = $"Line"

  override _.setProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.setProps (terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> Line

    // Interfaces
    OrientationInterface.setProps terminalElement view props


  override this.newView() = new Line()


// ListView
type internal ListViewElement(props: Props) =
  inherit ViewTerminalElement(props)

  override _.removeProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.removeProps (terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> ListView

    // Properties
    props
    |> Props.tryFind PKey.listView.allowsMarking
    |> Option.iter (fun _ -> view.AllowsMarking <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.listView.allowsMultipleSelection
    |> Option.iter (fun _ -> view.AllowsMultipleSelection <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.listView.leftItem
    |> Option.iter (fun _ -> view.LeftItem <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.listView.selectedItem
    |> Option.iter (fun _ -> view.SelectedItem <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.listView.source
    |> Option.iter (fun _ -> view.SetSource Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.listView.topItem
    |> Option.iter (fun _ -> view.TopItem <- Unchecked.defaultof<_>)
    // Events
    terminalElement.tryRemoveEventHandler PKey.listView.collectionChanged

    terminalElement.tryRemoveEventHandler PKey.listView.openSelectedItem

    terminalElement.tryRemoveEventHandler PKey.listView.rowRender

    terminalElement.tryRemoveEventHandler PKey.listView.selectedItemChanged

  override _.name = $"ListView"

  override _.setProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.setProps (terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> ListView

    // Properties
    props
    |> Props.tryFind PKey.listView.allowsMarking
    |> Option.iter (fun v -> view.AllowsMarking <- v)

    props
    |> Props.tryFind PKey.listView.allowsMultipleSelection
    |> Option.iter (fun v -> view.AllowsMultipleSelection <- v)

    props
    |> Props.tryFind PKey.listView.leftItem
    |> Option.iter (fun v -> view.LeftItem <- v)

    props
    |> Props.tryFind PKey.listView.selectedItem
    |> Option.iter (fun v -> view.SelectedItem <- v)

    props
    |> Props.tryFind PKey.listView.source
    |> Option.iter (fun v -> view.SetSource(ObservableCollection(v)))

    props
    |> Props.tryFind PKey.listView.topItem
    |> Option.iter (fun v -> view.TopItem <- v)
    // Events
    terminalElement.trySetEventHandler(PKey.listView.collectionChanged, view.CollectionChanged)

    terminalElement.trySetEventHandler(PKey.listView.openSelectedItem, view.OpenSelectedItem)

    terminalElement.trySetEventHandler(PKey.listView.rowRender, view.RowRender)

    terminalElement.trySetEventHandler(PKey.listView.selectedItemChanged, view.SelectedItemChanged)


  override this.newView() = new ListView()


// Margin
type internal MarginElement(props: Props) =
  inherit ViewTerminalElement(props)

  override _.removeProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.removeProps (terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> Margin

    // Properties
    props
    |> Props.tryFind PKey.margin.shadowStyle
    |> Option.iter (fun _ -> view.ShadowStyle <- Unchecked.defaultof<_>)

  override _.name = $"Margin"

  override _.setProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.setProps (terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> Margin

    // Properties
    props
    |> Props.tryFind PKey.margin.shadowStyle
    |> Option.iter (fun v -> view.ShadowStyle <- v)


  override this.newView() = new Margin()


// Menu
type internal MenuElement(props: Props) =
  inherit ViewTerminalElement(props)

  override _.removeProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.removeProps (terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> Menu

    // Properties
    props
    |> Props.tryFind PKey.menu.selectedMenuItem
    |> Option.iter (fun _ -> view.SelectedMenuItem <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.menu.superMenuItem
    |> Option.iter (fun _ -> view.SuperMenuItem <- Unchecked.defaultof<_>)
    // Events
    terminalElement.trySetEventHandler(PKey.menu.accepted, view.Accepted)

    terminalElement.trySetEventHandler(PKey.menu.selectedMenuItemChanged, view.SelectedMenuItemChanged)

    ()

  override _.name = $"Menu"

  override _.setProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.setProps (terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> Menu

    // Properties
    props
    |> Props.tryFind PKey.menu.selectedMenuItem
    |> Option.iter (fun v -> view.SelectedMenuItem <- v)

    props
    |> Props.tryFind PKey.menu.superMenuItem
    |> Option.iter (fun v -> view.SuperMenuItem <- v)
    // Events
    terminalElement.trySetEventHandler(PKey.menu.accepted, view.Accepted)

    terminalElement.trySetEventHandler(PKey.menu.selectedMenuItemChanged, view.SelectedMenuItemChanged)

  override this.newView() = new Menu()


  override this.setAsChildOfParentView = false

  interface IMenuElement


type internal PopoverMenuElement(props: Props) =
  inherit ViewTerminalElement(props)

  override _.removeProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.removeProps (terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> PopoverMenu

    // Properties
    props
    |> Props.tryFind PKey.popoverMenu.key
    |> Option.iter (fun _ -> view.Key <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.popoverMenu.mouseFlags
    |> Option.iter (fun _ -> view.MouseFlags <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.popoverMenu.root
    |> Option.iter (fun _ -> view.Root <- Unchecked.defaultof<_>)
    // Events
    terminalElement.tryRemoveEventHandler PKey.popoverMenu.accepted

    terminalElement.tryRemoveEventHandler PKey.popoverMenu.keyChanged

  override this.name = "PopoverMenu"

  override _.setProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.setProps (terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> PopoverMenu

    // Properties
    props
    |> Props.tryFind PKey.popoverMenu.key
    |> Option.iter (fun v -> view.Key <- v)

    props
    |> Props.tryFind PKey.popoverMenu.mouseFlags
    |> Option.iter (fun v -> view.MouseFlags <- v)

    props
    |> Props.tryFind PKey.popoverMenu.root
    |> Option.iter (fun v -> view.Root <- v)
    // Events
    terminalElement.trySetEventHandler(PKey.popoverMenu.accepted, view.Accepted)

    terminalElement.trySetEventHandler(PKey.popoverMenu.keyChanged, view.KeyChanged)

  override this.SubElements_PropKeys =
    SubElementPropKey.from PKey.popoverMenu.root_element
    :: base.SubElements_PropKeys

  override this.newView() = new PopoverMenu()


  interface IPopoverMenuElement


// MenuBarItem
type internal MenuBarItemElement(props: Props) =
  inherit ViewTerminalElement(props)

  override _.removeProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.removeProps (terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> MenuBarItem

    // Properties
    props
    |> Props.tryFind PKey.menuBarItem.popoverMenu
    |> Option.iter (fun _ -> view.PopoverMenu <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.menuBarItem.popoverMenuOpen
    |> Option.iter (fun _ -> view.PopoverMenuOpen <- Unchecked.defaultof<_>)
    // Events
    terminalElement.tryRemoveEventHandler PKey.menuBarItem.popoverMenuOpenChanged

  override this.name = "MenuBarItem"

  override _.setProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.setProps (terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> MenuBarItem

    // Properties
    props
    |> Props.tryFind PKey.menuBarItem.popoverMenu
    |> Option.iter (fun v -> view.PopoverMenu <- v)

    props
    |> Props.tryFind PKey.menuBarItem.popoverMenuOpen
    |> Option.iter (fun v -> view.PopoverMenuOpen <- v)
    // Events
    terminalElement.trySetEventHandler(PKey.menuBarItem.popoverMenuOpenChanged, view.PopoverMenuOpenChanged)

  override this.SubElements_PropKeys =
    SubElementPropKey.from PKey.menuBarItem.popoverMenu_element
    :: base.SubElements_PropKeys

  override this.newView() = new MenuBarItem()


  interface IMenuBarItemElement


// MenuBar
type internal MenuBarElement(props: Props) =
  inherit ViewTerminalElement(props)

  override _.removeProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.removeProps (terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> MenuBar

    // Properties
    props
    |> Props.tryFind PKey.menuBar.key
    |> Option.iter (fun _ -> view.Key <- Unchecked.defaultof<_>)

    // NOTE: No need to handle `Menus: MenuBarItemElement list` property here,
    //       as it already registered as "children" property.
    //       And "children" properties are handled by the TreeDiff initializeTree function

    // Events
    terminalElement.tryRemoveEventHandler PKey.menuBar.keyChanged

  override _.name = $"MenuBar"

  override _.setProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.setProps (terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> MenuBar

    // Properties
    props
    |> Props.tryFind PKey.menuBar.key
    |> Option.iter (fun v -> view.Key <- v)

    // NOTE: No need to handle `Menus: MenuBarItemElement list` property here,
    //       as it already registered as "children" property.
    //       And "children" properties are handled by the TreeDiff initializeTree function

    // Events
    terminalElement.trySetEventHandler(PKey.menuBar.keyChanged, view.KeyChanged)

  override this.newView() = new MenuBar()


// Shortcut
type internal ShortcutElement(props: Props) =
  inherit ViewTerminalElement(props)

  override _.removeProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.removeProps (terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> Shortcut

    // Interfaces
    OrientationInterface.removeProps terminalElement view props

    // Properties
    props
    |> Props.tryFind PKey.shortcut.action
    |> Option.iter (fun _ -> view.Action <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.shortcut.alignmentModes
    |> Option.iter (fun _ -> view.AlignmentModes <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.shortcut.commandView
    |> Option.iter (fun _ -> view.CommandView <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.shortcut.forceFocusColors
    |> Option.iter (fun _ -> view.ForceFocusColors <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.shortcut.helpText
    |> Option.iter (fun _ -> view.HelpText <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.shortcut.text
    |> Option.iter (fun _ -> view.Text <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.shortcut.bindKeyToApplication
    |> Option.iter (fun _ -> view.BindKeyToApplication <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.shortcut.key
    |> Option.iter (fun _ -> view.Key <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.shortcut.minimumKeyTextSize
    |> Option.iter (fun _ -> view.MinimumKeyTextSize <- Unchecked.defaultof<_>)

  override _.name = $"Shortcut"

  override _.setProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.setProps (terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> Shortcut

    // Interfaces
    OrientationInterface.setProps terminalElement view props

    // Properties
    props
    |> Props.tryFind PKey.shortcut.action
    |> Option.iter (fun v -> view.Action <- v)

    props
    |> Props.tryFind PKey.shortcut.alignmentModes
    |> Option.iter (fun v -> view.AlignmentModes <- v)

    props
    |> Props.tryFind PKey.shortcut.commandView
    |> Option.iter (fun v -> view.CommandView <- v)

    props
    |> Props.tryFind PKey.shortcut.forceFocusColors
    |> Option.iter (fun v -> view.ForceFocusColors <- v)

    props
    |> Props.tryFind PKey.shortcut.helpText
    |> Option.iter (fun v -> view.HelpText <- v)

    props
    |> Props.tryFind PKey.shortcut.text
    |> Option.iter (fun v -> view.Text <- v)

    props
    |> Props.tryFind PKey.shortcut.bindKeyToApplication
    |> Option.iter (fun v -> view.BindKeyToApplication <- v)

    props
    |> Props.tryFind PKey.shortcut.key
    |> Option.iter (fun v -> view.Key <- v)

    props
    |> Props.tryFind PKey.shortcut.minimumKeyTextSize
    |> Option.iter (fun v -> view.MinimumKeyTextSize <- v)

  override this.SubElements_PropKeys =
    SubElementPropKey.from PKey.shortcut.commandView_element
    :: base.SubElements_PropKeys

  override this.newView() = new Shortcut()


type internal MenuItemElement(props: Props) =
  inherit ShortcutElement(props)

  override _.removeProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.removeProps (terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> MenuItem

    // Properties
    props
    |> Props.tryFind PKey.menuItem.command
    |> Option.iter (fun _ -> Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.menuItem.subMenu
    |> Option.iter (fun _ -> Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.menuItem.targetView
    |> Option.iter (fun _ -> Unchecked.defaultof<_>)
    // Events
    terminalElement.tryRemoveEventHandler PKey.menuItem.accepted

  override _.name = $"MenuItem"

  override _.setProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.setProps (terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> MenuItem

    // Properties
    props
    |> Props.tryFind PKey.menuItem.command
    |> Option.iter (fun v -> view.Command <- v)

    props
    |> Props.tryFind PKey.menuItem.subMenu
    |> Option.iter (fun v -> view.SubMenu <- v)

    props
    |> Props.tryFind PKey.menuItem.targetView
    |> Option.iter (fun v -> view.TargetView <- v)
    // Events
    terminalElement.trySetEventHandler(PKey.menuItem.accepted, view.Accepted)

  override this.SubElements_PropKeys =
    SubElementPropKey.from PKey.menuItem.subMenu_element
    :: base.SubElements_PropKeys


  override this.newView() = new MenuItem()


// NumericUpDown<'a>
type internal NumericUpDownElement<'a>(props: Props) =
  inherit ViewTerminalElement(props)

  override _.removeProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.removeProps (terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> NumericUpDown<'a>

    // Properties
    props
    |> Props.tryFind PKey.numericUpDown<'a>.format
    |> Option.iter (fun _ -> view.Format <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.numericUpDown<'a>.increment
    |> Option.iter (fun _ -> view.Increment <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.numericUpDown<'a>.value
    |> Option.iter (fun _ -> view.Value <- Unchecked.defaultof<_>)
    // Events
    terminalElement.tryRemoveEventHandler PKey.numericUpDown<'a>.formatChanged

    terminalElement.tryRemoveEventHandler PKey.numericUpDown<'a>.incrementChanged

    terminalElement.tryRemoveEventHandler PKey.numericUpDown<'a>.valueChanged

    terminalElement.tryRemoveEventHandler PKey.numericUpDown<'a>.valueChanging

  override _.name = $"NumericUpDown<'a>"

  override _.setProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.setProps (terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> NumericUpDown<'a>

    // Properties
    props
    |> Props.tryFind PKey.numericUpDown<'a>.format
    |> Option.iter (fun v -> view.Format <- v)

    props
    |> Props.tryFind PKey.numericUpDown<'a>.increment
    |> Option.iter (fun v -> view.Increment <- v)

    props
    |> Props.tryFind PKey.numericUpDown<'a>.value
    |> Option.iter (fun v -> view.Value <- v)
    // Events
    props
    |> Props.tryFind PKey.numericUpDown<'a>.formatChanged
    |> Option.iter (fun v -> terminalElement.EventRegistry.setEventHandler(PKey.numericUpDown<'a>.formatChanged, view.FormatChanged, v))

    props
    |> Props.tryFind PKey.numericUpDown<'a>.incrementChanged
    |> Option.iter (fun v -> terminalElement.EventRegistry.setEventHandler(PKey.numericUpDown<'a>.incrementChanged, view.IncrementChanged, v))

    props
    |> Props.tryFind PKey.numericUpDown<'a>.valueChanged
    |> Option.iter (fun v -> terminalElement.EventRegistry.setEventHandler(PKey.numericUpDown<'a>.valueChanged, view.ValueChanged, v))

    props
    |> Props.tryFind PKey.numericUpDown<'a>.valueChanging
    |> Option.iter (fun v -> terminalElement.EventRegistry.setEventHandler(PKey.numericUpDown<'a>.valueChanging, view.ValueChanging, v))

  override this.newView() = new NumericUpDown<'a>()


  interface INumericUpDownElement


// NumericUpDown
type internal NumericUpDownElement(props: Props) =
  inherit NumericUpDownElement<int>(props)


// OpenDialog
type internal OpenDialogElement(props: Props) =
  inherit ViewTerminalElement(props)

  override _.removeProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.removeProps (terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> OpenDialog

    // Properties
    props
    |> Props.tryFind PKey.openDialog.openMode
    |> Option.iter (fun _ -> view.OpenMode <- Unchecked.defaultof<_>)

  override _.name = $"OpenDialog"

  override _.setProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.setProps (terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> OpenDialog

    // Properties
    props
    |> Props.tryFind PKey.openDialog.openMode
    |> Option.iter (fun v -> view.OpenMode <- v)


  override this.newView() = new OpenDialog()


// SelectorBase
type internal SelectorBaseElement(props: Props) =
  inherit ViewTerminalElement(props)

  override _.removeProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.removeProps (terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> SelectorBase

    // Interfaces
    OrientationInterface.removeProps terminalElement view props

    // Properties
    props
    |> Props.tryFind PKey.selectorBase.assignHotKeys
    |> Option.iter (fun _ -> view.AssignHotKeys <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.selectorBase.doubleClickAccepts
    |> Option.iter (fun _ -> view.DoubleClickAccepts <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.selectorBase.horizontalSpace
    |> Option.iter (fun _ -> view.HorizontalSpace <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.selectorBase.labels
    |> Option.iter (fun _ -> view.Labels <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.selectorBase.styles
    |> Option.iter (fun _ -> view.Styles <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.selectorBase.usedHotKeys
    |> Option.iter (fun _ -> view.UsedHotKeys <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.selectorBase.value
    |> Option.iter (fun _ -> view.Value <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.selectorBase.values
    |> Option.iter (fun _ -> view.Values <- Unchecked.defaultof<_>)
    // Events
    terminalElement.tryRemoveEventHandler PKey.selectorBase.valueChanged

  override _.name = "SelectorBase"

  override _.setProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.setProps (terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> SelectorBase

    // Interfaces
    OrientationInterface.setProps terminalElement view props

    // Properties
    props
    |> Props.tryFind PKey.selectorBase.assignHotKeys
    |> Option.iter (fun v -> view.AssignHotKeys <- v)

    props
    |> Props.tryFind PKey.selectorBase.doubleClickAccepts
    |> Option.iter (fun v -> view.DoubleClickAccepts <- v)

    props
    |> Props.tryFind PKey.selectorBase.horizontalSpace
    |> Option.iter (fun v -> view.HorizontalSpace <- v)

    props
    |> Props.tryFind PKey.selectorBase.labels
    |> Option.iter (fun v -> view.Labels <- v)

    props
    |> Props.tryFind PKey.selectorBase.styles
    |> Option.iter (fun v -> view.Styles <- v)

    props
    |> Props.tryFind PKey.selectorBase.usedHotKeys
    |> Option.iter (fun v -> view.UsedHotKeys <- v)

    props
    |> Props.tryFind PKey.selectorBase.value
    |> Option.iter (fun v -> view.Value <- v)

    props
    |> Props.tryFind PKey.selectorBase.values
    |> Option.iter (fun v -> view.Values <- v)
    // Events
    terminalElement.trySetEventHandler(PKey.selectorBase.valueChanged, view.ValueChanged)

  override this.newView() = raise (NotImplementedException())


// OptionSelector
type internal OptionSelectorElement(props: Props) =
  inherit SelectorBaseElement(props)

  override _.removeProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.removeProps (terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> OptionSelector

    // Properties
    props
    |> Props.tryFind PKey.optionSelector.cursor
    |> Option.iter (fun _ -> view.Cursor <- Unchecked.defaultof<_>)

  override _.name = $"OptionSelector"

  override _.setProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.setProps (terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> OptionSelector

    // Properties
    props
    |> Props.tryFind PKey.optionSelector.cursor
    |> Option.iter (fun v -> view.Cursor <- v)

  override this.newView() = new OptionSelector()


// FlagSelector
type internal FlagSelectorElement(props: Props) =
  inherit SelectorBaseElement(props)

  override _.removeProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.removeProps (terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> FlagSelector

    // Properties
    props
    |> Props.tryFind PKey.flagSelector.value
    |> Option.iter (fun _ -> view.Value <- Unchecked.defaultof<_>)

  override _.name = $"FlagSelector"

  override _.setProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.setProps (terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> FlagSelector

    // Properties
    props
    |> Props.tryFind PKey.flagSelector.value
    |> Option.iter (fun v -> view.Value <- v)

  override this.newView() = new FlagSelector()


// Padding
type internal PaddingElement(props: Props) =
  inherit ViewTerminalElement(props)

  override _.removeProps(terminalElement: IInternalTerminalElement, props: Props) = base.removeProps (terminalElement, props)

  override _.name = $"Padding"

  override _.setProps(terminalElement: IInternalTerminalElement, props: Props) = base.setProps (terminalElement, props)


  override this.newView() = new Padding()


// ProgressBar
type internal ProgressBarElement(props: Props) =
  inherit ViewTerminalElement(props)

  override _.removeProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.removeProps (terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> ProgressBar

    // Properties
    props
    |> Props.tryFind PKey.progressBar.bidirectionalMarquee
    |> Option.iter (fun _ -> view.BidirectionalMarquee <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.progressBar.fraction
    |> Option.iter (fun _ -> view.Fraction <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.progressBar.progressBarFormat
    |> Option.iter (fun _ -> view.ProgressBarFormat <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.progressBar.progressBarStyle
    |> Option.iter (fun _ -> view.ProgressBarStyle <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.progressBar.segmentCharacter
    |> Option.iter (fun _ -> view.SegmentCharacter <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.progressBar.text
    |> Option.iter (fun _ -> view.Text <- Unchecked.defaultof<_>)

  override _.name = $"ProgressBar"

  override _.setProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.setProps (terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> ProgressBar

    // Properties
    props
    |> Props.tryFind PKey.progressBar.bidirectionalMarquee
    |> Option.iter (fun v -> view.BidirectionalMarquee <- v)

    props
    |> Props.tryFind PKey.progressBar.fraction
    |> Option.iter (fun v -> view.Fraction <- v)

    props
    |> Props.tryFind PKey.progressBar.progressBarFormat
    |> Option.iter (fun v -> view.ProgressBarFormat <- v)

    props
    |> Props.tryFind PKey.progressBar.progressBarStyle
    |> Option.iter (fun v -> view.ProgressBarStyle <- v)

    props
    |> Props.tryFind PKey.progressBar.segmentCharacter
    |> Option.iter (fun v -> view.SegmentCharacter <- v)

    props
    |> Props.tryFind PKey.progressBar.text
    |> Option.iter (fun v -> view.Text <- v)


  override this.newView() = new ProgressBar()


// SaveDialog
type internal SaveDialogElement(props: Props) =
  inherit ViewTerminalElement(props)

  override _.removeProps(terminalElement: IInternalTerminalElement, props: Props) = base.removeProps (terminalElement, props)
  // No properties or events SaveDialog

  override _.name = $"SaveDialog"

  override _.setProps(terminalElement: IInternalTerminalElement, props: Props) = base.setProps (terminalElement, props)
  // No properties or events SaveDialog


  override this.newView() = new SaveDialog()


// ScrollBar
type internal ScrollBarElement(props: Props) =
  inherit ViewTerminalElement(props)

  override _.removeProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.removeProps (terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> ScrollBar

    // Interfaces
    OrientationInterface.removeProps terminalElement view props

    // Properties
    props
    |> Props.tryFind PKey.scrollBar.autoShow
    |> Option.iter (fun _ -> view.AutoShow <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.scrollBar.increment
    |> Option.iter (fun _ -> view.Increment <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.scrollBar.position
    |> Option.iter (fun _ -> view.Position <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.scrollBar.scrollableContentSize
    |> Option.iter (fun _ -> view.ScrollableContentSize <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.scrollBar.visibleContentSize
    |> Option.iter (fun _ -> view.VisibleContentSize <- Unchecked.defaultof<_>)
    // Events
    terminalElement.tryRemoveEventHandler PKey.scrollBar.scrollableContentSizeChanged

    terminalElement.tryRemoveEventHandler PKey.scrollBar.sliderPositionChanged

  override _.name = $"ScrollBar"

  override _.setProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.setProps (terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> ScrollBar

    // Interfaces
    OrientationInterface.setProps terminalElement view props

    // Properties
    props
    |> Props.tryFind PKey.scrollBar.autoShow
    |> Option.iter (fun v -> view.AutoShow <- v)

    props
    |> Props.tryFind PKey.scrollBar.increment
    |> Option.iter (fun v -> view.Increment <- v)

    props
    |> Props.tryFind PKey.scrollBar.position
    |> Option.iter (fun v -> view.Position <- v)

    props
    |> Props.tryFind PKey.scrollBar.scrollableContentSize
    |> Option.iter (fun v -> view.ScrollableContentSize <- v)

    props
    |> Props.tryFind PKey.scrollBar.visibleContentSize
    |> Option.iter (fun v -> view.VisibleContentSize <- v)
    // Events
    terminalElement.trySetEventHandler(PKey.scrollBar.scrollableContentSizeChanged, view.ScrollableContentSizeChanged)

    terminalElement.trySetEventHandler(PKey.scrollBar.sliderPositionChanged, view.SliderPositionChanged)


  override this.newView() = new ScrollBar()


// ScrollSlider
type internal ScrollSliderElement(props: Props) =
  inherit ViewTerminalElement(props)

  override _.removeProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.removeProps (terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> ScrollSlider

    // Interfaces
    OrientationInterface.removeProps terminalElement view props

    // Properties
    props
    |> Props.tryFind PKey.scrollSlider.position
    |> Option.iter (fun _ -> view.Position <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.scrollSlider.size
    |> Option.iter (fun _ -> view.Size <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.scrollSlider.sliderPadding
    |> Option.iter (fun _ -> view.SliderPadding <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.scrollSlider.visibleContentSize
    |> Option.iter (fun _ -> view.VisibleContentSize <- Unchecked.defaultof<_>)
    // Events
    terminalElement.tryRemoveEventHandler PKey.scrollSlider.positionChanged

    terminalElement.tryRemoveEventHandler PKey.scrollSlider.positionChanging

    terminalElement.tryRemoveEventHandler PKey.scrollSlider.scrolled

  override _.name = $"ScrollSlider"

  override _.setProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.setProps (terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> ScrollSlider

    // Interfaces
    OrientationInterface.setProps terminalElement view props

    // Properties
    props
    |> Props.tryFind PKey.scrollSlider.position
    |> Option.iter (fun v -> view.Position <- v)

    props
    |> Props.tryFind PKey.scrollSlider.size
    |> Option.iter (fun v -> view.Size <- v)

    props
    |> Props.tryFind PKey.scrollSlider.sliderPadding
    |> Option.iter (fun v -> view.SliderPadding <- v)

    props
    |> Props.tryFind PKey.scrollSlider.visibleContentSize
    |> Option.iter (fun v -> view.VisibleContentSize <- v)
    // Events
    terminalElement.trySetEventHandler(PKey.scrollSlider.positionChanged, view.PositionChanged)

    terminalElement.trySetEventHandler(PKey.scrollSlider.positionChanging, view.PositionChanging)

    terminalElement.trySetEventHandler(PKey.scrollSlider.scrolled, view.Scrolled)


  override this.newView() = new ScrollSlider()


// Slider<'a>
type internal SliderElement<'a>(props: Props) =
  inherit ViewTerminalElement(props)

  override _.removeProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.removeProps (terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> Slider<'a>

    // Interfaces
    OrientationInterface.removeProps terminalElement view props

    // Properties
    props
    |> Props.tryFind PKey.slider<'a>.allowEmpty
    |> Option.iter (fun _ -> view.AllowEmpty <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.slider<'a>.focusedOption
    |> Option.iter (fun _ -> view.FocusedOption <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.slider<'a>.legendsOrientation
    |> Option.iter (fun _ -> view.LegendsOrientation <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.slider<'a>.minimumInnerSpacing
    |> Option.iter (fun _ -> view.MinimumInnerSpacing <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.slider<'a>.options
    |> Option.iter (fun _ -> view.Options <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.slider<'a>.rangeAllowSingle
    |> Option.iter (fun _ -> view.RangeAllowSingle <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.slider<'a>.showEndSpacing
    |> Option.iter (fun _ -> view.ShowEndSpacing <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.slider<'a>.showLegends
    |> Option.iter (fun _ -> view.ShowLegends <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.slider<'a>.style
    |> Option.iter (fun _ -> view.Style <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.slider<'a>.text
    |> Option.iter (fun _ -> view.Text <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.slider<'a>.``type``
    |> Option.iter (fun _ -> view.Type <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.slider<'a>.useMinimumSize
    |> Option.iter (fun _ -> view.UseMinimumSize <- Unchecked.defaultof<_>)
    // Events
    terminalElement.tryRemoveEventHandler PKey.slider.optionFocused

    terminalElement.tryRemoveEventHandler PKey.slider.optionsChanged

  override _.name = $"Slider<'a>"

  override _.setProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.setProps (terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> Slider<'a>

    // Interfaces
    OrientationInterface.setProps terminalElement view props

    // Properties
    props
    |> Props.tryFind PKey.slider.allowEmpty
    |> Option.iter (fun v -> view.AllowEmpty <- v)

    props
    |> Props.tryFind PKey.slider.focusedOption
    |> Option.iter (fun v -> view.FocusedOption <- v)

    props
    |> Props.tryFind PKey.slider.legendsOrientation
    |> Option.iter (fun v -> view.LegendsOrientation <- v)

    props
    |> Props.tryFind PKey.slider.minimumInnerSpacing
    |> Option.iter (fun v -> view.MinimumInnerSpacing <- v)

    props
    |> Props.tryFind PKey.slider.options
    |> Option.iter (fun v -> view.Options <- Collections.Generic.List<_>(v))

    props
    |> Props.tryFind PKey.slider.rangeAllowSingle
    |> Option.iter (fun v -> view.RangeAllowSingle <- v)

    props
    |> Props.tryFind PKey.slider.showEndSpacing
    |> Option.iter (fun v -> view.ShowEndSpacing <- v)

    props
    |> Props.tryFind PKey.slider.showLegends
    |> Option.iter (fun v -> view.ShowLegends <- v)

    props
    |> Props.tryFind PKey.slider.style
    |> Option.iter (fun v -> view.Style <- v)

    props
    |> Props.tryFind PKey.slider.text
    |> Option.iter (fun v -> view.Text <- v)

    props
    |> Props.tryFind PKey.slider.``type``
    |> Option.iter (fun v -> view.Type <- v)

    props
    |> Props.tryFind PKey.slider.useMinimumSize
    |> Option.iter (fun v -> view.UseMinimumSize <- v)
    // Events
    terminalElement.trySetEventHandler(PKey.slider.optionFocused, view.OptionFocused)

    terminalElement.trySetEventHandler(PKey.slider.optionsChanged, view.OptionsChanged)


  override this.newView() = new Slider<'a>()


  interface ISliderElement


// Slider
type internal SliderElement(props: Props) =
  inherit ViewTerminalElement(props)

  override _.removeProps(terminalElement: IInternalTerminalElement, props: Props) = base.removeProps (terminalElement, props)
  // No properties or events Slider

  override _.name = $"Slider"

  override _.setProps(terminalElement: IInternalTerminalElement, props: Props) = base.setProps (terminalElement, props)
  // No properties or events Slider

  override this.newView() = new Slider()


// SpinnerView
type internal SpinnerViewElement(props: Props) =
  inherit ViewTerminalElement(props)

  override _.removeProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.removeProps (terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> SpinnerView

    // Properties
    props
    |> Props.tryFind PKey.spinnerView.autoSpin
    |> Option.iter (fun _ -> view.AutoSpin <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.spinnerView.sequence
    |> Option.iter (fun _ -> view.Sequence <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.spinnerView.spinBounce
    |> Option.iter (fun _ -> view.SpinBounce <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.spinnerView.spinDelay
    |> Option.iter (fun _ -> view.SpinDelay <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.spinnerView.spinReverse
    |> Option.iter (fun _ -> view.SpinReverse <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.spinnerView.style
    |> Option.iter (fun _ -> view.Style <- Unchecked.defaultof<_>)

  override _.name = $"SpinnerView"

  override _.setProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.setProps (terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> SpinnerView

    // Properties
    props
    |> Props.tryFind PKey.spinnerView.autoSpin
    |> Option.iter (fun v -> view.AutoSpin <- v)

    props
    |> Props.tryFind PKey.spinnerView.sequence
    |> Option.iter (fun v -> view.Sequence <- v |> List.toArray)

    props
    |> Props.tryFind PKey.spinnerView.spinBounce
    |> Option.iter (fun v -> view.SpinBounce <- v)

    props
    |> Props.tryFind PKey.spinnerView.spinDelay
    |> Option.iter (fun v -> view.SpinDelay <- v)

    props
    |> Props.tryFind PKey.spinnerView.spinReverse
    |> Option.iter (fun v -> view.SpinReverse <- v)

    props
    |> Props.tryFind PKey.spinnerView.style
    |> Option.iter (fun v -> view.Style <- v)


  override this.newView() = new SpinnerView()


// StatusBar
type internal StatusBarElement(props: Props) =
  inherit ViewTerminalElement(props)

  override _.removeProps(terminalElement: IInternalTerminalElement, props: Props) = base.removeProps (terminalElement, props)
  // No properties or events StatusBar

  override _.name = $"StatusBar"

  override _.setProps(terminalElement: IInternalTerminalElement, props: Props) = base.setProps (terminalElement, props)
  // No properties or events StatusBar


  override this.newView() = new StatusBar()


// Tab
type internal TabElement(props: Props) =
  inherit ViewTerminalElement(props)

  override _.removeProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.removeProps (terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> Tab

    // Properties
    props
    |> Props.tryFind PKey.tab.displayText
    |> Option.iter (fun _ -> view.DisplayText <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.tab.view
    |> Option.iter (fun _ -> view.View <- Unchecked.defaultof<_>)

  override _.name = $"Tab"

  override _.setProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.setProps (terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> Tab

    // Properties
    props
    |> Props.tryFind PKey.tab.displayText
    |> Option.iter (fun v -> view.DisplayText <- v)

  override this.SubElements_PropKeys =
    SubElementPropKey.from PKey.tab.view_element
    :: base.SubElements_PropKeys

  override this.newView() = new Tab()


// TabView
type internal TabViewElement(props: Props) =
  inherit ViewTerminalElement(props)

  override _.removeProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.removeProps (terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> TabView

    // Properties
    props
    |> Props.tryFind PKey.tabView.maxTabTextWidth
    |> Option.iter (fun _ -> view.MaxTabTextWidth <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.tabView.selectedTab
    |> Option.iter (fun _ -> view.SelectedTab <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.tabView.style
    |> Option.iter (fun _ -> view.Style <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.tabView.tabScrollOffset
    |> Option.iter (fun _ -> view.TabScrollOffset <- Unchecked.defaultof<_>)
    // Events
    terminalElement.tryRemoveEventHandler PKey.tabView.selectedTabChanged

    terminalElement.tryRemoveEventHandler PKey.tabView.tabClicked

  override _.name = $"TabView"

  override _.setProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.setProps (terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> TabView

    // Properties
    props
    |> Props.tryFind PKey.tabView.maxTabTextWidth
    |> Option.iter (fun v -> view.MaxTabTextWidth <- (v |> uint32))

    props
    |> Props.tryFind PKey.tabView.selectedTab
    |> Option.iter (fun v -> view.SelectedTab <- v)

    props
    |> Props.tryFind PKey.tabView.style
    |> Option.iter (fun v -> view.Style <- v)

    props
    |> Props.tryFind PKey.tabView.tabScrollOffset
    |> Option.iter (fun v -> view.TabScrollOffset <- v)
    // Events
    terminalElement.trySetEventHandler(PKey.tabView.selectedTabChanged, view.SelectedTabChanged)

    terminalElement.trySetEventHandler(PKey.tabView.tabClicked, view.TabClicked)

    // Additional properties
    props
    |> Props.tryFind PKey.tabView.tabs
    |> Option.iter (fun tabItems ->
      tabItems
      |> Seq.iter (fun tabItem -> view.AddTab((tabItem :?> IInternalTerminalElement).View :?> Tab, false))
    )

  override this.SubElements_PropKeys =
    SubElementPropKey.from PKey.tabView.tabs_elements
    :: base.SubElements_PropKeys

  override this.newView() = new TabView()


// TableView
type internal TableViewElement(props: Props) =
  inherit ViewTerminalElement(props)

  override _.removeProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.removeProps (terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> TableView

    // Properties
    props
    |> Props.tryFind PKey.tableView.cellActivationKey
    |> Option.iter (fun _ -> view.CellActivationKey <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.tableView.collectionNavigator
    |> Option.iter (fun _ -> view.CollectionNavigator <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.tableView.columnOffset
    |> Option.iter (fun _ -> view.ColumnOffset <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.tableView.fullRowSelect
    |> Option.iter (fun _ -> view.FullRowSelect <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.tableView.maxCellWidth
    |> Option.iter (fun _ -> view.MaxCellWidth <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.tableView.minCellWidth
    |> Option.iter (fun _ -> view.MinCellWidth <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.tableView.multiSelect
    |> Option.iter (fun _ -> view.MultiSelect <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.tableView.nullSymbol
    |> Option.iter (fun _ -> view.NullSymbol <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.tableView.rowOffset
    |> Option.iter (fun _ -> view.RowOffset <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.tableView.selectedColumn
    |> Option.iter (fun _ -> view.SelectedColumn <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.tableView.selectedRow
    |> Option.iter (fun _ -> view.SelectedRow <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.tableView.separatorSymbol
    |> Option.iter (fun _ -> view.SeparatorSymbol <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.tableView.style
    |> Option.iter (fun _ -> view.Style <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.tableView.table
    |> Option.iter (fun _ -> view.Table <- Unchecked.defaultof<_>)
    // Events
    terminalElement.tryRemoveEventHandler PKey.tableView.cellActivated

    terminalElement.tryRemoveEventHandler PKey.tableView.cellToggled

    terminalElement.tryRemoveEventHandler PKey.tableView.selectedCellChanged

  override _.name = $"TableView"

  override _.setProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.setProps (terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> TableView

    // Properties
    props
    |> Props.tryFind PKey.tableView.cellActivationKey
    |> Option.iter (fun v -> view.CellActivationKey <- v)

    props
    |> Props.tryFind PKey.tableView.collectionNavigator
    |> Option.iter (fun v -> view.CollectionNavigator <- v)

    props
    |> Props.tryFind PKey.tableView.columnOffset
    |> Option.iter (fun v -> view.ColumnOffset <- v)

    props
    |> Props.tryFind PKey.tableView.fullRowSelect
    |> Option.iter (fun v -> view.FullRowSelect <- v)

    props
    |> Props.tryFind PKey.tableView.maxCellWidth
    |> Option.iter (fun v -> view.MaxCellWidth <- v)

    props
    |> Props.tryFind PKey.tableView.minCellWidth
    |> Option.iter (fun v -> view.MinCellWidth <- v)

    props
    |> Props.tryFind PKey.tableView.multiSelect
    |> Option.iter (fun v -> view.MultiSelect <- v)

    props
    |> Props.tryFind PKey.tableView.nullSymbol
    |> Option.iter (fun v -> view.NullSymbol <- v)

    props
    |> Props.tryFind PKey.tableView.rowOffset
    |> Option.iter (fun v -> view.RowOffset <- v)

    props
    |> Props.tryFind PKey.tableView.selectedColumn
    |> Option.iter (fun v -> view.SelectedColumn <- v)

    props
    |> Props.tryFind PKey.tableView.selectedRow
    |> Option.iter (fun v -> view.SelectedRow <- v)

    props
    |> Props.tryFind PKey.tableView.separatorSymbol
    |> Option.iter (fun v -> view.SeparatorSymbol <- v)

    props
    |> Props.tryFind PKey.tableView.style
    |> Option.iter (fun v -> view.Style <- v)

    props
    |> Props.tryFind PKey.tableView.table
    |> Option.iter (fun v -> view.Table <- v)
    // Events
    terminalElement.trySetEventHandler(PKey.tableView.cellActivated, view.CellActivated)

    terminalElement.trySetEventHandler(PKey.tableView.cellToggled, view.CellToggled)

    terminalElement.trySetEventHandler(PKey.tableView.selectedCellChanged, view.SelectedCellChanged)


  override this.newView() = new TableView()


// TextField
type internal TextFieldElement(props: Props) =
  inherit ViewTerminalElement(props)

  override _.removeProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.removeProps (terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> TextField

    // Properties
    props
    |> Props.tryFind PKey.textField.autocomplete
    |> Option.iter (fun _ -> view.Autocomplete <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.textField.cursorPosition
    |> Option.iter (fun _ -> view.CursorPosition <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.textField.readOnly
    |> Option.iter (fun _ -> view.ReadOnly <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.textField.secret
    |> Option.iter (fun _ -> view.Secret <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.textField.selectedStart
    |> Option.iter (fun _ -> view.SelectedStart <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.textField.selectWordOnlyOnDoubleClick
    |> Option.iter (fun _ -> view.SelectWordOnlyOnDoubleClick <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.textField.text
    |> Option.iter (fun _ -> view.Text <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.textField.used
    |> Option.iter (fun _ -> view.Used <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.textField.useSameRuneTypeForWords
    |> Option.iter (fun _ -> view.UseSameRuneTypeForWords <- Unchecked.defaultof<_>)
    // Events
    terminalElement.tryRemoveEventHandler PKey.textField.textChanging

  override _.name = $"TextField"

  override _.setProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.setProps (terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> TextField

    // Properties
    props
    |> Props.tryFind PKey.textField.autocomplete
    |> Option.iter (fun v -> view.Autocomplete <- v)

    props
    |> Props.tryFind PKey.textField.cursorPosition
    |> Option.iter (fun v -> view.CursorPosition <- v)

    props
    |> Props.tryFind PKey.textField.readOnly
    |> Option.iter (fun v -> view.ReadOnly <- v)

    props
    |> Props.tryFind PKey.textField.secret
    |> Option.iter (fun v -> view.Secret <- v)

    props
    |> Props.tryFind PKey.textField.selectedStart
    |> Option.iter (fun v -> view.SelectedStart <- v)

    props
    |> Props.tryFind PKey.textField.selectWordOnlyOnDoubleClick
    |> Option.iter (fun v -> view.SelectWordOnlyOnDoubleClick <- v)

    props
    |> Props.tryFind PKey.textField.text
    |> Option.iter (fun v -> view.Text <- v)

    props
    |> Props.tryFind PKey.textField.used
    |> Option.iter (fun v -> view.Used <- v)

    props
    |> Props.tryFind PKey.textField.useSameRuneTypeForWords
    |> Option.iter (fun v -> view.UseSameRuneTypeForWords <- v)
    // Events
    terminalElement.trySetEventHandler(PKey.textField.textChanging, view.TextChanging)


  override this.newView() = new TextField()


// TextValidateField
type internal TextValidateFieldElement(props: Props) =
  inherit ViewTerminalElement(props)

  override _.removeProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.removeProps (terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> TextValidateField

    // Properties
    props
    |> Props.tryFind PKey.textValidateField.provider
    |> Option.iter (fun _ -> view.Provider <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.textValidateField.text
    |> Option.iter (fun _ -> view.Text <- Unchecked.defaultof<_>)

  override _.name = $"TextValidateField"

  override _.setProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.setProps (terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> TextValidateField

    // Properties
    props
    |> Props.tryFind PKey.textValidateField.provider
    |> Option.iter (fun v -> view.Provider <- v)

    props
    |> Props.tryFind PKey.textValidateField.text
    |> Option.iter (fun v -> view.Text <- v)


  override this.newView() = new TextValidateField()


// TextView
type internal TextViewElement(props: Props) =
  inherit ViewTerminalElement(props)


  override _.removeProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.removeProps (terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> TextView

    // Properties
    props
    |> Props.tryFind PKey.textView.allowsReturn
    |> Option.iter (fun _ -> view.AllowsReturn <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.textView.allowsTab
    |> Option.iter (fun _ -> view.AllowsTab <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.textView.cursorPosition
    |> Option.iter (fun _ -> view.CursorPosition <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.textView.inheritsPreviousAttribute
    |> Option.iter (fun _ -> view.InheritsPreviousAttribute <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.textView.isDirty
    |> Option.iter (fun _ -> view.IsDirty <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.textView.isSelecting
    |> Option.iter (fun _ -> view.IsSelecting <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.textView.leftColumn
    |> Option.iter (fun _ -> view.LeftColumn <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.textView.multiline
    |> Option.iter (fun _ -> view.Multiline <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.textView.readOnly
    |> Option.iter (fun _ -> view.ReadOnly <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.textView.selectionStartColumn
    |> Option.iter (fun _ -> view.SelectionStartColumn <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.textView.selectionStartRow
    |> Option.iter (fun _ -> view.SelectionStartRow <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.textView.selectWordOnlyOnDoubleClick
    |> Option.iter (fun _ -> view.SelectWordOnlyOnDoubleClick <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.textView.tabWidth
    |> Option.iter (fun _ -> view.TabWidth <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.textView.text
    |> Option.iter (fun _ -> view.Text <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.textView.topRow
    |> Option.iter (fun _ -> view.TopRow <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.textView.used
    |> Option.iter (fun _ -> view.Used <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.textView.useSameRuneTypeForWords
    |> Option.iter (fun _ -> view.UseSameRuneTypeForWords <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.textView.wordWrap
    |> Option.iter (fun _ -> view.WordWrap <- Unchecked.defaultof<_>)
    // Events
    terminalElement.tryRemoveEventHandler PKey.textView.contentsChanged

    terminalElement.tryRemoveEventHandler PKey.textView.drawNormalColor

    terminalElement.tryRemoveEventHandler PKey.textView.drawReadOnlyColor

    terminalElement.tryRemoveEventHandler PKey.textView.drawSelectionColor

    terminalElement.tryRemoveEventHandler PKey.textView.drawUsedColor

    terminalElement.tryRemoveEventHandler PKey.textView.unwrappedCursorPosition

    // Additional properties
    terminalElement.tryRemoveEventHandler PKey.textView.textChanged


  override _.name = $"TextView"

  override _.setProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.setProps (terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> TextView

    // Properties
    props
    |> Props.tryFind PKey.textView.allowsReturn
    |> Option.iter (fun v -> view.AllowsReturn <- v)

    props
    |> Props.tryFind PKey.textView.allowsTab
    |> Option.iter (fun v -> view.AllowsTab <- v)

    props
    |> Props.tryFind PKey.textView.cursorPosition
    |> Option.iter (fun v -> view.CursorPosition <- v)

    props
    |> Props.tryFind PKey.textView.inheritsPreviousAttribute
    |> Option.iter (fun v -> view.InheritsPreviousAttribute <- v)

    props
    |> Props.tryFind PKey.textView.isDirty
    |> Option.iter (fun v -> view.IsDirty <- v)

    props
    |> Props.tryFind PKey.textView.isSelecting
    |> Option.iter (fun v -> view.IsSelecting <- v)

    props
    |> Props.tryFind PKey.textView.leftColumn
    |> Option.iter (fun v -> view.LeftColumn <- v)

    props
    |> Props.tryFind PKey.textView.multiline
    |> Option.iter (fun v -> view.Multiline <- v)

    props
    |> Props.tryFind PKey.textView.readOnly
    |> Option.iter (fun v -> view.ReadOnly <- v)

    props
    |> Props.tryFind PKey.textView.selectionStartColumn
    |> Option.iter (fun v -> view.SelectionStartColumn <- v)

    props
    |> Props.tryFind PKey.textView.selectionStartRow
    |> Option.iter (fun v -> view.SelectionStartRow <- v)

    props
    |> Props.tryFind PKey.textView.selectWordOnlyOnDoubleClick
    |> Option.iter (fun v -> view.SelectWordOnlyOnDoubleClick <- v)

    props
    |> Props.tryFind PKey.textView.tabWidth
    |> Option.iter (fun v -> view.TabWidth <- v)

    props
    |> Props.tryFind PKey.textView.text
    |> Option.iter (fun v -> view.Text <- v)

    props
    |> Props.tryFind PKey.textView.topRow
    |> Option.iter (fun v -> view.TopRow <- v)

    props
    |> Props.tryFind PKey.textView.used
    |> Option.iter (fun v -> view.Used <- v)

    props
    |> Props.tryFind PKey.textView.useSameRuneTypeForWords
    |> Option.iter (fun v -> view.UseSameRuneTypeForWords <- v)

    props
    |> Props.tryFind PKey.textView.wordWrap
    |> Option.iter (fun v -> view.WordWrap <- v)
    // Events
    terminalElement.trySetEventHandler(PKey.textView.contentsChanged, view.ContentsChanged)

    terminalElement.trySetEventHandler(PKey.textView.drawNormalColor, view.DrawNormalColor)

    terminalElement.trySetEventHandler(PKey.textView.drawReadOnlyColor, view.DrawReadOnlyColor)

    terminalElement.trySetEventHandler(PKey.textView.drawSelectionColor, view.DrawSelectionColor)

    terminalElement.trySetEventHandler(PKey.textView.drawUsedColor, view.DrawUsedColor)

    terminalElement.trySetEventHandler(PKey.textView.unwrappedCursorPosition, view.UnwrappedCursorPosition)

    // Additional properties
    terminalElement.trySetEventHandler(PKey.textView.textChanged, view.ContentsChanged)


  override this.newView() = new TextView()


// TimeField
type internal TimeFieldElement(props: Props) =
  inherit ViewTerminalElement(props)

  override _.removeProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.removeProps (terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> TimeField

    // Properties
    props
    |> Props.tryFind PKey.timeField.cursorPosition
    |> Option.iter (fun _ -> view.CursorPosition <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.timeField.isShortFormat
    |> Option.iter (fun _ -> view.IsShortFormat <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.timeField.time
    |> Option.iter (fun _ -> view.Time <- Unchecked.defaultof<_>)
    // Events
    terminalElement.tryRemoveEventHandler PKey.timeField.timeChanged

  override _.name = $"TimeField"

  override _.setProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.setProps (terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> TimeField

    // Properties
    props
    |> Props.tryFind PKey.timeField.cursorPosition
    |> Option.iter (fun v -> view.CursorPosition <- v)

    props
    |> Props.tryFind PKey.timeField.isShortFormat
    |> Option.iter (fun v -> view.IsShortFormat <- v)

    props
    |> Props.tryFind PKey.timeField.time
    |> Option.iter (fun v -> view.Time <- v)
    // Events
    terminalElement.trySetEventHandler(PKey.timeField.timeChanged, view.TimeChanged)


  override this.newView() = new TimeField()


// Runnable
type internal RunnableElement(props: Props) =
  inherit ViewTerminalElement(props)

  override _.removeProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.removeProps (terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> Runnable

    // Properties
    props
    |> Props.tryFind PKey.runnable.isModal
    |> Option.iter (fun _ -> view.SetIsModal(Unchecked.defaultof<_>))

    props
    |> Props.tryFind PKey.runnable.isRunning
    |> Option.iter (fun _ -> view.SetIsRunning(Unchecked.defaultof<_>))

    props
    |> Props.tryFind PKey.runnable.stopRequested
    |> Option.iter (fun _ -> view.StopRequested <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.runnable.result
    |> Option.iter (fun _ -> view.Result <- Unchecked.defaultof<_>)

    // Events
    terminalElement.tryRemoveEventHandler PKey.runnable.isRunningChanging

    terminalElement.tryRemoveEventHandler PKey.runnable.isRunningChanged

    terminalElement.tryRemoveEventHandler PKey.runnable.isModalChanged

  override _.name = $"Runnable"

  override _.setProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.setProps (terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> Runnable

    // Properties
    props
    |> Props.tryFind PKey.runnable.isModal
    |> Option.iter (fun v -> view.SetIsModal(v))

    props
    |> Props.tryFind PKey.runnable.isRunning
    |> Option.iter (fun v -> view.SetIsRunning(v))

    props
    |> Props.tryFind PKey.runnable.stopRequested
    |> Option.iter (fun v -> view.StopRequested <- v)

    props
    |> Props.tryFind PKey.runnable.result
    |> Option.iter (fun v -> view.Result <- v)

    // Events
    terminalElement.trySetEventHandler(PKey.runnable.isRunningChanging, view.IsRunningChanging)

    terminalElement.trySetEventHandler(PKey.runnable.isRunningChanged, view.IsRunningChanged)

    terminalElement.trySetEventHandler(PKey.runnable.isModalChanged, view.IsModalChanged)


  override this.newView() = new Runnable()


// TreeView<'a when 'a : not struct>
type internal TreeViewElement<'a when 'a: not struct>(props: Props) =
  inherit ViewTerminalElement(props)

  override _.removeProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.removeProps (terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> TreeView<'a>

    // Properties
    props
    |> Props.tryFind PKey.treeView<'a>.allowLetterBasedNavigation
    |> Option.iter (fun _ -> view.AllowLetterBasedNavigation <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.treeView<'a>.aspectGetter
    |> Option.iter (fun _ -> view.AspectGetter <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.treeView<'a>.colorGetter
    |> Option.iter (fun _ -> view.ColorGetter <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.treeView<'a>.maxDepth
    |> Option.iter (fun _ -> view.MaxDepth <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.treeView<'a>.multiSelect
    |> Option.iter (fun _ -> view.MultiSelect <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.treeView<'a>.objectActivationButton
    |> Option.iter (fun _ -> view.ObjectActivationButton <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.treeView<'a>.objectActivationKey
    |> Option.iter (fun _ -> view.ObjectActivationKey <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.treeView<'a>.scrollOffsetHorizontal
    |> Option.iter (fun _ -> view.ScrollOffsetHorizontal <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.treeView<'a>.scrollOffsetVertical
    |> Option.iter (fun _ -> view.ScrollOffsetVertical <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.treeView<'a>.selectedObject
    |> Option.iter (fun _ -> view.SelectedObject <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.treeView<'a>.style
    |> Option.iter (fun _ -> view.Style <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.treeView<'a>.treeBuilder
    |> Option.iter (fun _ -> view.TreeBuilder <- Unchecked.defaultof<_>)
    // Events
    terminalElement.tryRemoveEventHandler PKey.treeView<'a>.drawLine

    terminalElement.tryRemoveEventHandler PKey.treeView<'a>.objectActivated

    terminalElement.tryRemoveEventHandler PKey.treeView<'a>.selectionChanged

  override _.name = $"TreeView<'a>"

  override _.setProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.setProps (terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> TreeView<'a>

    // Properties
    props
    |> Props.tryFind PKey.treeView<'a>.allowLetterBasedNavigation
    |> Option.iter (fun v -> view.AllowLetterBasedNavigation <- v)

    props
    |> Props.tryFind PKey.treeView<'a>.aspectGetter
    |> Option.iter (fun v -> view.AspectGetter <- v)

    props
    |> Props.tryFind PKey.treeView<'a>.colorGetter
    |> Option.iter (fun v -> view.ColorGetter <- v)

    props
    |> Props.tryFind PKey.treeView<'a>.maxDepth
    |> Option.iter (fun v -> view.MaxDepth <- v)

    props
    |> Props.tryFind PKey.treeView<'a>.multiSelect
    |> Option.iter (fun v -> view.MultiSelect <- v)

    props
    |> Props.tryFind PKey.treeView<'a>.objectActivationButton
    |> Option.iter (fun v -> view.ObjectActivationButton <- v |> Option.toNullable)

    props
    |> Props.tryFind PKey.treeView<'a>.objectActivationKey
    |> Option.iter (fun v -> view.ObjectActivationKey <- v)

    props
    |> Props.tryFind PKey.treeView<'a>.scrollOffsetHorizontal
    |> Option.iter (fun v -> view.ScrollOffsetHorizontal <- v)

    props
    |> Props.tryFind PKey.treeView<'a>.scrollOffsetVertical
    |> Option.iter (fun v -> view.ScrollOffsetVertical <- v)

    props
    |> Props.tryFind PKey.treeView<'a>.selectedObject
    |> Option.iter (fun v -> view.SelectedObject <- v)

    props
    |> Props.tryFind PKey.treeView<'a>.style
    |> Option.iter (fun v -> view.Style <- v)

    props
    |> Props.tryFind PKey.treeView<'a>.treeBuilder
    |> Option.iter (fun v -> view.TreeBuilder <- v)
    // Events
    props
    |> Props.tryFind PKey.treeView<'a>.drawLine
    |> Option.iter (fun v -> terminalElement.EventRegistry.setEventHandler(PKey.treeView<'a>.drawLine, view.DrawLine, v))

    props
    |> Props.tryFind PKey.treeView<'a>.objectActivated
    |> Option.iter (fun v -> terminalElement.EventRegistry.setEventHandler(PKey.treeView<'a>.objectActivated, view.ObjectActivated, v))

    props
    |> Props.tryFind PKey.treeView<'a>.selectionChanged
    |> Option.iter (fun v -> terminalElement.EventRegistry.setEventHandler(PKey.treeView<'a>.selectionChanged, view.SelectionChanged, v))

  override this.newView() = new TreeView<'a>()


  interface ITreeViewElement


// TreeView
type internal TreeViewElement(props: Props) =
  inherit TreeViewElement<ITreeNode>(props)


// Window
type internal WindowElement(props: Props) =
  inherit ViewTerminalElement(props)

  override _.removeProps(terminalElement: IInternalTerminalElement, props: Props) = base.removeProps (terminalElement, props)
  // No properties or events Window

  override _.name = $"Window"

  override _.setProps(terminalElement: IInternalTerminalElement, props: Props) = base.setProps (terminalElement, props)
  // No properties or events Window


  override this.newView() = new Window()


// Wizard
type internal WizardElement(props: Props) =
  inherit ViewTerminalElement(props)

  override _.removeProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.removeProps (terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> Wizard

    // Properties
    props
    |> Props.tryFind PKey.wizard.currentStep
    |> Option.iter (fun _ -> view.CurrentStep <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.wizard.modal
    |> Option.iter (fun _ -> view.SetIsModal(Unchecked.defaultof<_>))
    // Events
    terminalElement.tryRemoveEventHandler PKey.wizard.cancelled

    terminalElement.tryRemoveEventHandler PKey.wizard.finished

    terminalElement.tryRemoveEventHandler PKey.wizard.movingBack

    terminalElement.tryRemoveEventHandler PKey.wizard.movingNext

    terminalElement.tryRemoveEventHandler PKey.wizard.stepChanged

    terminalElement.tryRemoveEventHandler PKey.wizard.stepChanging

  override _.name = $"Wizard"

  override _.setProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.setProps (terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> Wizard

    // Properties
    props
    |> Props.tryFind PKey.wizard.currentStep
    |> Option.iter (fun v -> view.CurrentStep <- v)

    props
    |> Props.tryFind PKey.wizard.modal
    |> Option.iter (fun v -> view.SetIsModal(v))
    // Events
    terminalElement.trySetEventHandler(PKey.wizard.cancelled, view.Cancelled)

    terminalElement.trySetEventHandler(PKey.wizard.finished, view.Finished)

    terminalElement.trySetEventHandler(PKey.wizard.movingBack, view.MovingBack)

    terminalElement.trySetEventHandler(PKey.wizard.movingNext, view.MovingNext)

    terminalElement.trySetEventHandler(PKey.wizard.stepChanged, view.StepChanged)

    terminalElement.trySetEventHandler(PKey.wizard.stepChanging, view.StepChanging)


  override this.newView() = new Wizard()


// WizardStep
type internal WizardStepElement(props: Props) =
  inherit ViewTerminalElement(props)

  override _.removeProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.removeProps (terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> WizardStep

    // Properties
    props
    |> Props.tryFind PKey.wizardStep.backButtonText
    |> Option.iter (fun _ -> view.BackButtonText <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.wizardStep.helpText
    |> Option.iter (fun _ -> view.HelpText <- Unchecked.defaultof<_>)

    props
    |> Props.tryFind PKey.wizardStep.nextButtonText
    |> Option.iter (fun _ -> view.NextButtonText <- Unchecked.defaultof<_>)

  override _.name = $"WizardStep"

  override _.setProps(terminalElement: IInternalTerminalElement, props: Props) =
    base.setProps (terminalElement, props)

    let terminalElement = terminalElement :?> TerminalElement
    let view = terminalElement.View :?> WizardStep

    // Properties
    props
    |> Props.tryFind PKey.wizardStep.backButtonText
    |> Option.iter (fun v -> view.BackButtonText <- v)

    props
    |> Props.tryFind PKey.wizardStep.helpText
    |> Option.iter (fun v -> view.HelpText <- v)

    props
    |> Props.tryFind PKey.wizardStep.nextButtonText
    |> Option.iter (fun v -> view.NextButtonText <- v)


  override this.newView() = new WizardStep()
