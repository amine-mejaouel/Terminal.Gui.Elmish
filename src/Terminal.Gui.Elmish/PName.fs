/// Properties names index
[<RequireQualifiedAccess>]
module Terminal.Gui.Elmish.PName

type view() =
    member inline _.children = "children"
    member inline _.ref = "ref"
    // Properties
    member inline _.arrangement = "view.arrangement"
    member inline _.borderStyle = "view.borderStyle"
    member inline _.canFocus = "view.canFocus"
    member inline _.contentSizeTracksViewport = "view.contentSizeTracksViewport"
    member inline _.cursorVisibility = "view.cursorVisibility"
    member inline _.data = "view.data"
    member inline _.enabled = "view.enabled"
    member inline _.frame = "view.frame"
    member inline _.hasFocus = "view.hasFocus"
    member inline _.height = "view.height"
    member inline _.highlightStates = "view.highlightStates"
    member inline _.hotKey = "view.hotKey"
    member inline _.hotKeySpecifier = "view.hotKeySpecifier"
    member inline _.id = "view.id"
    member inline _.isInitialized = "view.isInitialized"
    member inline _.mouseHeldDown = "view.mouseHeldDown"
    member inline _.needsDraw = "view.needsDraw"
    member inline _.preserveTrailingSpaces = "view.preserveTrailingSpaces"
    member inline _.schemeName = "view.schemeName"
    member inline _.shadowStyle = "view.shadowStyle"
    member inline _.superViewRendersLineCanvas = "view.superViewRendersLineCanvas"
    member inline _.tabStop = "view.tabStop"
    member inline _.text = "view.text"
    member inline _.textAlignment = "view.textAlignment"
    member inline _.textDirection = "view.textDirection"
    member inline _.title = "view.title"
    member inline _.validatePosDim = "view.validatePosDim"
    member inline _.verticalTextAlignment = "view.verticalTextAlignment"
    member inline _.viewport = "view.viewport"
    member inline _.viewportSettings = "view.viewportSettings"
    member inline _.visible = "view.visible"
    member inline _.wantContinuousButtonPressed = "view.wantContinuousButtonPressed"
    member inline _.wantMousePositionReports = "view.wantMousePositionReports"
    member inline _.width = "view.width"
    member inline _.x = "view.x"
    member inline _.y = "view.y"

    // Events
    member inline _.accepting = "view.accepting"
    member inline _.advancingFocus = "view.advancingFocus"
    member inline _.borderStyleChanged = "view.borderStyleChanged"
    member inline _.canFocusChanged = "view.canFocusChanged"
    member inline _.clearedViewport = "view.clearedViewport"
    member inline _.clearingViewport = "view.clearingViewport"
    member inline _.commandNotBound = "view.commandNotBound"
    member inline _.contentSizeChanged = "view.contentSizeChanged"
    member inline _.disposing = "view.disposing"
    member inline _.drawComplete = "view.drawComplete"
    member inline _.drawingContent = "view.drawingContent"
    member inline _.drawingSubViews = "view.drawingSubViews"
    member inline _.drawingText = "view.drawingText"
    member inline _.enabledChanged = "view.enabledChanged"
    member inline _.focusedChanged = "view.focusedChanged"
    member inline _.frameChanged = "view.frameChanged"
    member inline _.gettingAttributeForRole = "view.gettingAttributeForRole"
    member inline _.gettingScheme = "view.gettingScheme"
    member inline _.handlingHotKey = "view.handlingHotKey"
    member inline _.hasFocusChanged = "view.hasFocusChanged"
    member inline _.hasFocusChanging = "view.hasFocusChanging"
    member inline _.hotKeyChanged = "view.hotKeyChanged"
    member inline _.initialized = "view.initialized"
    member inline _.keyDown = "view.keyDown"
    member inline _.keyDownNotHandled = "view.keyDownNotHandled"
    member inline _.keyUp = "view.keyUp"
    member inline _.mouseClick = "view.mouseClick"
    member inline _.mouseEnter = "view.mouseEnter"
    member inline _.mouseEvent = "view.mouseEvent"
    member inline _.mouseLeave = "view.mouseLeave"
    member inline _.mouseStateChanged = "view.mouseStateChanged"
    member inline _.mouseWheel = "view.mouseWheel"
    member inline _.removed = "view.removed"
    member inline _.schemeChanged = "view.schemeChanged"
    member inline _.schemeChanging = "view.schemeChanging"
    member inline _.schemeNameChanged = "view.schemeNameChanged"
    member inline _.schemeNameChanging = "view.schemeNameChanging"
    member inline _.selecting = "view.selecting"
    member inline _.subViewAdded = "view.subViewAdded"
    member inline _.subViewLayout = "view.subViewLayout"
    member inline _.subViewRemoved = "view.subViewRemoved"
    member inline _.subViewsLaidOut = "view.subViewsLaidOut"
    member inline _.superViewChanged = "view.superViewChanged"
    member inline _.textChanged = "view.textChanged"
    member inline _.titleChanged = "view.titleChanged"
    member inline _.titleChanging = "view.titleChanging"
    member inline _.viewportChanged = "view.viewportChanged"
    member inline _.visibleChanged = "view.visibleChanged"
    member inline _.visibleChanging = "view.visibleChanging"

type adornment() =
    inherit view()
    // Properties
    member inline _.diagnostics = "adornment.diagnostics"
    member inline _.superViewRendersLineCanvas = "adornment.superViewRendersLineCanvas"
    member inline _.thickness = "adornment.thickness"
    member inline _.viewport = "adornment.viewport"
    // Events
    member inline _.thicknessChanged = "adornment.thicknessChanged"

type bar() =
    inherit view()
    // Properties
    member inline _.alignmentModes = "bar.alignmentModes"
    member inline _.orientation = "bar.orientation"
    // Events
    member inline _.orientationChanged = "bar.orientationChanged"
    member inline _.orientationChanging = "bar.orientationChanging"

type border() =
    inherit adornment()
    // Properties
    member inline _.lineStyle = "border.lineStyle"
    member inline _.settings = "border.settings"

type button() =
    inherit view()
    // Properties
    member inline _.hotKeySpecifier = "button.hotKeySpecifier"
    member inline _.isDefault = "button.isDefault"
    member inline _.noDecorations = "button.noDecorations"
    member inline _.noPadding = "button.noPadding"
    member inline _.text = "button.text"
    member inline _.wantContinuousButtonPressed = "button.wantContinuousButtonPressed"

type checkBox() =
    inherit view()
    // Properties
    member inline _.allowCheckStateNone = "checkBox.allowCheckStateNone"
    member inline _.checkedState = "checkBox.checkedState"
    member inline _.hotKeySpecifier = "checkBox.hotKeySpecifier"
    member inline _.radioStyle = "checkBox.radioStyle"
    member inline _.text = "checkBox.text"
    // Events
    member inline _.checkedStateChanging = "checkBox.checkedStateChanging"
    member inline _.checkedStateChanged = "checkBox.checkedStateChanged"

type colorPicker() =
    inherit view()
    // Properties
    member inline _.selectedColor = "colorPicker.selectedColor"
    member inline _.style = "colorPicker.style"
    // Events
    member inline _.colorChanged = "colorPicker.colorChanged"

type colorPicker16() =
    inherit view()
    // Properties
    member inline _.boxHeight = "colorPicker16.boxHeight"
    member inline _.boxWidth = "colorPicker16.boxWidth"
    member inline _.cursor = "colorPicker16.cursor"
    member inline _.selectedColor = "colorPicker16.selectedColor"
    // Events
    member inline _.colorChanged = "colorPicker16.colorChanged"

type comboBox() =
    inherit view()
    // Properties
    member inline _.hideDropdownListOnClick = "comboBox.hideDropdownListOnClick"
    member inline _.readOnly = "comboBox.readOnly"
    member inline _.searchText = "comboBox.searchText"
    member inline _.selectedItem = "comboBox.selectedItem"
    member inline _.source = "comboBox.source"
    member inline _.text = "comboBox.text"
    // Events
    member inline _.collapsed = "comboBox.collapsed"
    member inline _.expanded = "comboBox.expanded"
    member inline _.openSelectedItem = "comboBox.openSelectedItem"
    member inline _.selectedItemChanged = "comboBox.selectedItemChanged"

type textField() =
    inherit view()
    // Properties
    member inline _.autocomplete = "textField.autocomplete"
    member inline _.caption = "textField.caption"
    member inline _.captionColor = "textField.captionColor"
    member inline _.cursorPosition = "textField.cursorPosition"
    member inline _.readOnly = "textField.readOnly"
    member inline _.secret = "textField.secret"
    member inline _.selectedStart = "textField.selectedStart"
    member inline _.selectWordOnlyOnDoubleClick = "textField.selectWordOnlyOnDoubleClick"
    member inline _.text = "textField.text"
    member inline _.used = "textField.used"
    member inline _.useSameRuneTypeForWords = "textField.useSameRuneTypeForWords"
    // Events
    member inline _.textChanging = "textField.textChanging"

type dateField() =
    inherit textField()
    // Properties
    member inline _.culture = "dateField.culture"
    member inline _.cursorPosition = "dateField.cursorPosition"
    member inline _.date = "dateField.date"
    // Events
    member inline _.dateChanged = "dateField.dateChanged"

type datePicker() =
    inherit view()
    // Properties
    member inline _.culture = "datePicker.culture"
    member inline _.date = "datePicker.date"

type toplevel() =
    inherit view()
    // Properties
    member inline _.modal = "toplevel.modal"
    member inline _.running = "toplevel.running"
    // Events
    member inline _.activate = "toplevel.activate"
    member inline _.closed = "toplevel.closed"
    member inline _.closing = "toplevel.closing"
    member inline _.deactivate = "toplevel.deactivate"
    member inline _.loaded = "toplevel.loaded"
    member inline _.ready = "toplevel.ready"
    member inline _.sizeChanging = "toplevel.sizeChanging"
    member inline _.unloaded = "toplevel.unloaded"

type dialog() =
    inherit toplevel()
    // Properties
    member inline _.buttonAlignment = "dialog.buttonAlignment"
    member inline _.buttonAlignmentModes = "dialog.buttonAlignmentModes"
    member inline _.canceled = "dialog.canceled"

type fileDialog() =
    inherit dialog()
    // Properties
    member inline _.allowedTypes = "fileDialog.allowedTypes"
    member inline _.allowsMultipleSelection = "fileDialog.allowsMultipleSelection"
    member inline _.fileOperationsHandler = "fileDialog.fileOperationsHandler"
    member inline _.mustExist = "fileDialog.mustExist"
    member inline _.openMode = "fileDialog.openMode"
    member inline _.path = "fileDialog.path"
    member inline _.searchMatcher = "fileDialog.searchMatcher"
    // Events
    member inline _.filesSelected = "fileDialog.filesSelected"

type saveDialog() =
    inherit fileDialog()

type frameView() =
    inherit view()

type graphView() =
    inherit view()
    // Properties
    member inline _.axisX = "graphView.axisX"
    member inline _.axisY = "graphView.axisY"
    member inline _.cellSize = "graphView.cellSize"
    member inline _.graphColor = "graphView.graphColor"
    member inline _.marginBottom = "graphView.marginBottom"
    member inline _.marginLeft = "graphView.marginLeft"
    member inline _.scrollOffset = "graphView.scrollOffset"

type hexView() =
    inherit view()
    // Properties
    member inline _.address = "hexView.address"
    member inline _.addressWidth = "hexView.addressWidth"
    member inline _.allowEdits = "hexView.allowEdits"
    member inline _.readOnly = "hexView.readOnly"
    member inline _.source = "hexView.source"
    // Events
    member inline _.edited = "hexView.edited"
    member inline _.positionChanged = "hexView.positionChanged"

type label() =
    inherit view()
    // Properties
    member inline _.hotKeySpecifier = "label.hotKeySpecifier"
    member inline _.text = "label.text"

type legendAnnotation() =
    inherit view()

type line() =
    inherit view()
    // Properties
    member inline _.orientation = "line.orientation"
    // Events
    member inline _.orientationChanged = "line.orientationChanged"
    member inline _.orientationChanging = "line.orientationChanging"

type lineView() =
    inherit view()
    // Properties
    member inline _.endingAnchor = "lineView.endingAnchor"
    member inline _.lineRune = "lineView.lineRune"
    member inline _.orientation = "lineView.orientation"
    member inline _.startingAnchor = "lineView.startingAnchor"

type listView() =
    inherit view()
    // Properties
    member inline _.allowsMarking = "listView.allowsMarking"
    member inline _.allowsMultipleSelection = "listView.allowsMultipleSelection"
    member inline _.leftItem = "listView.leftItem"
    member inline _.selectedItem = "listView.selectedItem"
    member inline _.source = "listView.source"
    member inline _.topItem = "listView.topItem"
    // Events
    member inline _.collectionChanged = "listView.collectionChanged"
    member inline _.openSelectedItem = "listView.openSelectedItem"
    member inline _.rowRender = "listView.rowRender"
    member inline _.selectedItemChanged = "listView.selectedItemChanged"

type margin() =
    inherit adornment()
    // Properties
    member inline _.shadowStyle = "margin.shadowStyle"

type menuv2() =
    inherit bar()
    // Properties
    member inline _.selectedMenuItem = "menuv2.selectedMenuItem"
    member inline _.superMenuItem = "menuv2.superMenuItem"
    // Events
    member inline _.accepted = "menuv2.accepted"
    member inline _.selectedMenuItemChanged = "menuv2.selectedMenuItemChanged"

type menuBarv2() =
    inherit menuv2()
    // Properties
    member inline _.key = "menuBarv2.key"
    // TODO: is this a macro, or is it wrongly named ?
    member inline _.menus = "children"
    // Events
    member inline _.keyChanged = "menuBarv2.keyChanged"

type shortcut() =
     inherit view()
     // Properties
     member inline _.action = "shortcut.action"
     member inline _.alignmentModes = "shortcut.alignmentModes"
     member inline _.commandView = "shortcut.commandView"
     member inline _.commandView_element = "shortcut.commandView.element"
     member inline _.forceFocusColors = "shortcut.forceFocusColors"
     member inline _.helpText = "shortcut.helpText"
     member inline _.text = "shortcut.text"
     member inline _.bindKeyToApplication = "shortcut.bindKeyToApplication"
     member inline _.key = "shortcut.key"
     member inline _.minimumKeyTextSize = "shortcut.minimumKeyTextSize"
     // Events
     member inline _.orientationChanged = "shortcut.orientationChanged"
     member inline _.orientationChanging = "shortcut.orientationChanging"

type menuItemv2() =
    inherit shortcut()
    // Properties
    member inline _.command = "menuItemv2.command"
    member inline _.subMenu= "menuItemv2.subMenu"
    member inline _.subMenu_element= "menuItemv2.subMenu.element"
    member inline _.targetView= "menuItemv2.targetView"
    // Events
    member inline _.accepted= "menuItemv2.accepted"


type menuBarItemv2() =
    inherit menuItemv2()
    // Properties
    member inline _.popoverMenu = "menuBarItemv2.popoverMenu"
    member inline _.popoverMenu_element = "menuBarItemv2.popoverMenu.element"
    member inline _.popoverMenuOpen = "menuBarItemv2.popoverMenuOpen"
    // Events
    member inline _.popoverMenuOpenChanged = "menuBarItemv2.popoverMenuOpenChanged"

type popoverMenu() =
    inherit view()
    // Properties
    member inline _.key = "popoverMenu.key"
    member inline _.mouseFlags = "popoverMenu.mouseFlags"
    member inline _.root = "popoverMenu.root"
    member inline _.root_element = "popoverMenu.root.element"
    // Events
    member inline _.accepted = "popoverMenu.accepted"
    member inline _.keyChanged = "popoverMenu.keyChanged"

type numericUpDown() =
    inherit view()
    // Properties
    member inline _.format = "numericUpDown.format"
    member inline _.increment = "numericUpDown.increment"
    member inline _.value = "numericUpDown.value"
    // Events
    member inline _.formatChanged = "numericUpDown.formatChanged"
    member inline _.incrementChanged = "numericUpDown.incrementChanged"
    member inline _.valueChanged = "numericUpDown.valueChanged"
    member inline _.valueChanging = "numericUpDown.valueChanging"

type openDialog() =
    inherit fileDialog()
    // Properties
    member inline _.openMode = "openDialog.openMode"

type optionSelector() =
    inherit view()
    //Properties
    member inline _.assignHotKeysToCheckBoxes = "optionSelector.assignHotKeysToCheckBoxes"
    member inline _.orientation = "optionSelector.orientation"
    member inline _.options = "optionSelector.options"
    member inline _.selectedItem = "optionSelector.selectedItem"
    // Events
    member inline _.orientationChanged = "optionSelector.orientationChanged"
    member inline _.orientationChanging = "optionSelector.orientationChanging"
    member inline _.selectedItemChanged = "optionSelector.selectedItemChanged"

type padding() =
    inherit adornment()

type progressBar() =
    inherit view()
    // Properties
    member inline _.bidirectionalMarquee = "progressBar.bidirectionalMarquee"
    member inline _.fraction = "progressBar.fraction"
    member inline _.progressBarFormat = "progressBar.progressBarFormat"
    member inline _.progressBarStyle = "progressBar.progressBarStyle"
    member inline _.segmentCharacter = "progressBar.segmentCharacter"
    member inline _.text = "progressBar.text"

type radioGroup() =
    inherit view()
    // Properties
    member inline _.assignHotKeysToRadioLabels = "radioGroup.assignHotKeysToRadioLabels"
    member inline _.cursor = "radioGroup.cursor"
    member inline _.doubleClickAccepts = "radioGroup.doubleClickAccepts"
    member inline _.horizontalSpace = "radioGroup.horizontalSpace"
    member inline _.orientation = "radioGroup.orientation"
    member inline _.radioLabels = "radioGroup.radioLabels"
    member inline _.selectedItem = "radioGroup.selectedItem"
    // Events
    member inline _.orientationChanged = "radioGroup.orientationChanged"
    member inline _.orientationChanging = "radioGroup.orientationChanging"
    member inline _.selectedItemChanged = "radioGroup.selectedItemChanged"

type scrollBar() =
    inherit view()
    // Properties
    member inline _.autoShow = "scrollBar.autoShow"
    member inline _.increment = "scrollBar.increment"
    member inline _.orientation = "scrollBar.orientation"
    member inline _.position = "scrollBar.position"
    member inline _.scrollableContentSize = "scrollBar.scrollableContentSize"
    member inline _.visibleContentSize = "scrollBar.visibleContentSize"
    // Events
    member inline _.orientationChanged = "scrollBar.orientationChanged"
    member inline _.orientationChanging = "scrollBar.orientationChanging"
    member inline _.scrollableContentSizeChanged = "scrollBar.scrollableContentSizeChanged"
    member inline _.sliderPositionChanged = "scrollBar.sliderPositionChanged"

type scrollSlider() =
    inherit view()
    // Properties
    member inline _.orientation = "scrollSlider.orientation"
    member inline _.position = "scrollSlider.position"
    member inline _.size = "scrollSlider.size"
    member inline _.sliderPadding = "scrollSlider.sliderPadding"
    member inline _.visibleContentSize = "scrollSlider.visibleContentSize"
    // Events
    member inline _.orientationChanged = "scrollSlider.orientationChanged"
    member inline _.orientationChanging = "scrollSlider.orientationChanging"
    member inline _.positionChanged = "scrollSlider.positionChanged"
    member inline _.positionChanging = "scrollSlider.positionChanging"
    member inline _.scrolled = "scrollSlider.scrolled"

type slider() =
    inherit view()
    // Properties
    member inline _.allowEmpty = "slider.allowEmpty"
    member inline _.focusedOption = "slider.focusedOption"
    member inline _.legendsOrientation = "slider.legendsOrientation"
    member inline _.minimumInnerSpacing = "slider.minimumInnerSpacing"
    member inline _.options = "slider.options"
    member inline _.orientation = "slider.orientation"
    member inline _.rangeAllowSingle = "slider.rangeAllowSingle"
    member inline _.showEndSpacing = "slider.showEndSpacing"
    member inline _.showLegends = "slider.showLegends"
    member inline _.style = "slider.style"
    member inline _.text = "slider.text"
    member inline _.``type`` = "slider.type"
    member inline _.useMinimumSize = "slider.useMinimumSize"
    // Events
    member inline _.optionFocused = "slider.optionFocused"
    member inline _.optionsChanged = "slider.optionsChanged"
    member inline _.orientationChanged = "slider.orientationChanged"
    member inline _.orientationChanging = "slider.orientationChanging"

type spinnerView() =
    inherit view()
    // Properties
    member inline _.autoSpin = "spinnerView.autoSpin"
    member inline _.sequence = "spinnerView.sequence"
    member inline _.spinBounce = "spinnerView.spinBounce"
    member inline _.spinDelay = "spinnerView.spinDelay"
    member inline _.spinReverse = "spinnerView.spinReverse"
    member inline _.style = "spinnerView.style"

type statusBar() =
    inherit bar()

type tab() =
    inherit view()
    // Properties
    member inline _.displayText = "tab.displayText"
    member inline _.view = "tab.view"

type tabView() =
    inherit view()
    // Properties
    member inline _.maxTabTextWidth = "tabView.maxTabTextWidth"
    member inline _.selectedTab = "tabView.selectedTab"
    member inline _.style = "tabView.style"
    member inline _.tabScrollOffset = "tabView.tabScrollOffset"
    // Events
    member inline _.selectedTabChanged = "tabView.selectedTabChanged"
    member inline _.tabClicked = "tabView.tabClicked"

    member inline _.tabs = "tabView.tabs"

type tableView() =
    inherit view()
    // Properties
    member inline _.cellActivationKey = "tableView.cellActivationKey"
    member inline _.collectionNavigator = "tableView.collectionNavigator"
    member inline _.columnOffset = "tableView.columnOffset"
    member inline _.fullRowSelect = "tableView.fullRowSelect"
    member inline _.maxCellWidth = "tableView.maxCellWidth"
    member inline _.minCellWidth = "tableView.minCellWidth"
    member inline _.multiSelect = "tableView.multiSelect"
    member inline _.nullSymbol = "tableView.nullSymbol"
    member inline _.rowOffset = "tableView.rowOffset"
    member inline _.selectedColumn = "tableView.selectedColumn"
    member inline _.selectedRow = "tableView.selectedRow"
    member inline _.separatorSymbol = "tableView.separatorSymbol"
    member inline _.style = "tableView.style"
    member inline _.table = "tableView.table"
    // Events
    member inline _.cellActivated = "tableView.cellActivated"
    member inline _.cellToggled = "tableView.cellToggled"
    member inline _.selectedCellChanged = "tableView.selectedCellChanged"

type textValidateField() =
    inherit view()
    // Properties
    member inline _.provider = "textValidateField.provider"
    member inline _.text = "textValidateField.text"

type textView() =
    inherit view()
    // Properties
    member inline _.allowsReturn = "textView.allowsReturn"
    member inline _.allowsTab = "textView.allowsTab"
    member inline _.cursorPosition = "textView.cursorPosition"
    member inline _.inheritsPreviousAttribute = "textView.inheritsPreviousAttribute"
    member inline _.isDirty = "textView.isDirty"
    member inline _.isSelecting = "textView.isSelecting"
    member inline _.leftColumn = "textView.leftColumn"
    member inline _.multiline = "textView.multiline"
    member inline _.readOnly = "textView.readOnly"
    member inline _.selectionStartColumn = "textView.selectionStartColumn"
    member inline _.selectionStartRow = "textView.selectionStartRow"
    member inline _.selectWordOnlyOnDoubleClick = "textView.selectWordOnlyOnDoubleClick"
    member inline _.tabWidth = "textView.tabWidth"
    member inline _.text = "textView.text"
    member inline _.topRow = "textView.topRow"
    member inline _.used = "textView.used"
    member inline _.useSameRuneTypeForWords = "textView.useSameRuneTypeForWords"
    member inline _.wordWrap = "textView.wordWrap"
    // Events
    member inline _.contentsChanged = "textView.contentsChanged"
    member inline _.drawNormalColor = "textView.drawNormalColor"
    member inline _.drawReadOnlyColor = "textView.drawReadOnlyColor"
    member inline _.drawSelectionColor = "textView.drawSelectionColor"
    member inline _.drawUsedColor = "textView.drawUsedColor"
    member inline _.unwrappedCursorPosition = "textView.unwrappedCursorPosition"
    // Additional properties
    member inline _.textChanged = "textView.textChanged"

type tileView() =
    inherit view()
    // Properties
    member inline _.lineStyle = "tileView.lineStyle"
    member inline _.orientation = "tileView.orientation"
    member inline _.toggleResizable = "tileView.toggleResizable"
    // Events
    member inline _.splitterMoved = "tileView.splitterMoved"

type timeField() =
    inherit textField()
    // Properties
    member inline _.cursorPosition = "timeField.cursorPosition"
    member inline _.isShortFormat = "timeField.isShortFormat"
    member inline _.time = "timeField.time"
    // Events
    member inline _.timeChanged = "timeField.timeChanged"

type treeView() =
    inherit view()
    // Properties
    member inline _.allowLetterBasedNavigation = "treeView.allowLetterBasedNavigation"
    member inline _.aspectGetter = "treeView.aspectGetter"
    member inline _.colorGetter = "treeView.colorGetter"
    member inline _.maxDepth = "treeView.maxDepth"
    member inline _.multiSelect = "treeView.multiSelect"
    member inline _.objectActivationButton = "treeView.objectActivationButton"
    member inline _.objectActivationKey = "treeView.objectActivationKey"
    member inline _.scrollOffsetHorizontal = "treeView.scrollOffsetHorizontal"
    member inline _.scrollOffsetVertical = "treeView.scrollOffsetVertical"
    member inline _.selectedObject = "treeView.selectedObject"
    member inline _.style = "treeView.style"
    member inline _.treeBuilder = "treeView.treeBuilder"
    // Events
    member inline _.drawLine = "treeView.drawLine"
    member inline _.objectActivated = "treeView.objectActivated"
    member inline _.selectionChanged = "treeView.selectionChanged"

type window() =
    inherit toplevel()

type wizard() =
    inherit dialog()
    // Properties
    member inline _.currentStep = "wizard.currentStep"
    member inline _.modal = "wizard.modal"
    // Events
    member inline _.cancelled = "wizard.cancelled"
    member inline _.finished = "wizard.finished"
    member inline _.movingBack = "wizard.movingBack"
    member inline _.movingNext = "wizard.movingNext"
    member inline _.stepChanged = "wizard.stepChanged"
    member inline _.stepChanging = "wizard.stepChanging"

type wizardStep() =
    inherit view()
    // Properties
    member inline _.backButtonText = "wizardStep.backButtonText"
    member inline _.helpText = "wizardStep.helpText"
    member inline _.nextButtonText = "wizardStep.nextButtonText"

let view = view()
let adornment = adornment()
let bar = bar()
let border = border()
let button = button()
let checkBox = checkBox()
let colorPicker = colorPicker()
let colorPicker16 = colorPicker16()
let comboBox = comboBox()
let textField = textField()
let dateField = dateField()
let datePicker = datePicker()
let toplevel = toplevel()
let dialog = dialog()
let fileDialog = fileDialog()
let saveDialog = saveDialog()
let frameView = frameView()
let graphView = graphView()
let hexView = hexView()
let label = label()
let legendAnnotation = legendAnnotation()
let line = line()
let lineView = lineView()
let listView = listView()
let margin = margin()
let menuv2 = menuv2()
let menuBarv2 = menuBarv2()
let shortcut = shortcut()
let menuItemv2 = menuItemv2()
let menuBarItemv2 = menuBarItemv2()
let popoverMenu = popoverMenu()
let numericUpDown = numericUpDown()
let openDialog = openDialog()
let optionSelector = optionSelector()
let padding = padding()
let progressBar = progressBar()
let radioGroup = radioGroup()
let scrollBar = scrollBar()
let scrollSlider = scrollSlider()
let slider= slider()
let spinnerView = spinnerView()
let statusBar = statusBar()
let tab = tab()
let tabView = tabView()
let tableView = tableView()
let textValidateField = textValidateField()
let textView = textView()
let tileView = tileView()
let timeField = timeField()
let treeView = treeView()
let window = window()
let wizard = wizard()
let wizardStep = wizardStep()
