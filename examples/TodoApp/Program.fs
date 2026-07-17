module TodoApp.Program

#nowarn "44"

open System.Collections.ObjectModel
open Terminal.Gui.Configuration
open Terminal.Gui.Drawing
open Terminal.Gui.Elmish
open Terminal.Gui.Input
open Terminal.Gui.ViewBase
open Terminal.Gui.Views
open TodoApp.Domain

let private at = TPos.Absolute
let private dispatchMsg dispatch msg = dispatch (TerminalMsg.ofMsg msg)

let private schemeName scheme = scheme.ToString()

let private filterLabel filter count selected =
  let marker = if selected then "●" else "○"

  match filter with
  | Filter.All -> $"Alt+A {marker} _All {count}"
  | Filter.Active -> $"Alt+V {marker} Acti_ve {count}"
  | Filter.Completed -> $"Alt+O {marker} D_one {count}"

let private todoLabel todo =
  let marker = if todo.IsCompleted then "✓" else "○"
  $" {marker}   {todo.Title}"

type private TaskRow(todo: Todo) =
  member _.Todo = todo
  member _.Id = todo.Id
  override _.ToString() = todoLabel todo

type private TaskListState() =
  // Keep the adapter stable: replacing ListView.Source clears its native selection.
  let rows = ObservableCollection<TaskRow>()
  let source = new ListWrapper<TaskRow>(rows)
  let mutable visibleTodos: Todo list = []
  let mutable isSyncing = false

  let tryFindRow startIndex todoId =
    seq { startIndex .. rows.Count - 1 }
    |> Seq.tryFind (fun index -> rows[index].Id = todoId)

  member _.Source = source :> IListDataSource
  member _.IsSyncing = isSyncing

  member _.TryGetTodoId(index: int) =
    visibleTodos |> List.tryItem index |> Option.map _.Id

  member _.Sync(todos: Todo list) =
    if todos <> visibleTodos then
      isSyncing <- true
      visibleTodos <- todos

      try
        todos
        |> List.iteri (fun index todo ->
          if index < rows.Count && rows[index].Id = todo.Id then
            if rows[index].Todo <> todo then
              rows[index] <- TaskRow(todo)
          else
            match tryFindRow index todo.Id with
            | Some existingIndex ->
              rows.Move(existingIndex, index)

              if rows[index].Todo <> todo then
                rows[index] <- TaskRow(todo)
            | None -> rows.Insert(index, TaskRow(todo)))

        while rows.Count > todos.Length do
          rows.RemoveAt(rows.Count - 1)
      finally
        isSyncing <- false

let private taskListState = TaskListState()

let private composer model dispatch =
  let isEditing =
    match model.EditorMode with
    | EditorMode.Creating -> false
    | EditorMode.Editing _ -> true

  let title = if isEditing then " EDIT TASK " else " NEW TASK "
  let actionText = if isEditing then "_Save" else "_Add"

  let hint =
    if isEditing then
      "Enter saves  ·  Esc cancels"
    else
      "Write it down, then press Enter"

  let canSubmit = not (System.String.IsNullOrWhiteSpace model.Draft)

  View.FrameView(fun p ->
    p.Title title
    p.BorderStyle LineStyle.Rounded
    p.SchemeName(schemeName Schemes.Accent)
    p.ShadowStyle ShadowStyles.Transparent
    p.X(at 1)
    p.Y(at 4)
    p.Width(Dim.Fill 2)
    p.Height 5

    p.Children
      [ View.TextField(fun p ->
          p.Key $"todo-composer-{model.FocusRevision}"
          p.X(at 0)
          p.Y(at 0)
          p.Width(Dim.Fill 11)
          p.Text model.Draft
          p.Initialized(fun _ -> dispatchMsg dispatch (ComposerMounted model.FocusRevision))

          if model.FocusReady then
            p.HasFocus true

          p.ValueChanged(fun args -> dispatchMsg dispatch (DraftChanged args.NewValue))
          p.Accepting(fun _ -> dispatchMsg dispatch SubmitDraft)

          p.KeyDown(fun key ->
            if isEditing && key = Key.Esc then
              dispatchMsg dispatch CancelEdit))
        View.Button(fun p ->
          p.Key "todo-composer-action"
          p.Text actionText
          p.X(TPos.AnchorEnd(Some 9))
          p.Y(at 0)
          p.Width 9
          p.Enabled canSubmit
          p.IsDefault true
          p.Accepting(fun _ -> dispatchMsg dispatch SubmitDraft))
        View.Label(fun p ->
          p.Key "todo-composer-hint"
          p.Text hint
          p.X(at 0)
          p.Y(at 1)
          p.Width(Dim.Fill())) ])

let private filterBar model dispatch =
  let allCount = model.Todos.Length
  let openCount = activeCount model
  let doneCount = completedCount model

  let filterButton x width filter count =
    View.Button(fun p ->
      p.Text(filterLabel filter count (model.Filter = filter))
      p.X(at x)
      p.Y(at 0)
      p.Width(Dim.Absolute width)
      p.NoDecorations true
      p.NoPadding true
      p.ShadowStyle ShadowStyles.None

      p.SchemeName(
        schemeName (
          if model.Filter = filter then
            Schemes.Accent
          else
            Schemes.Base
        )
      )

      p.Accepting(fun _ -> dispatchMsg dispatch (SetFilter filter)))
    :> IView

  View.FrameView(fun p ->
    p.BorderStyle LineStyle.None
    p.X(at 1)
    p.Y(at 10)
    p.Width(Dim.Fill 2)
    p.Height 1

    p.Children
      [ filterButton 0 14 Filter.All allCount
        filterButton 16 17 Filter.Active openCount
        filterButton 35 15 Filter.Completed doneCount ])

let private taskArea model dispatch =
  let visible = visibleTodos model
  taskListState.Sync visible

  let title = $" TASKS  {visible.Length} "

  if visible.IsEmpty then
    View.FrameView(fun p ->
      p.Title title
      p.BorderStyle LineStyle.Rounded
      p.SchemeName(schemeName Schemes.Accent)
      p.X(at 1)
      p.Y(at 12)
      p.Width(Dim.Fill 2)
      p.Height(Dim.Fill 3)

      p.Children
        [ View.Label(fun p ->
            p.Text(
              if model.Todos.IsEmpty then
                "Nothing here yet  ·  press F4 to add a task"
              else
                "No tasks match this view"
            )

            p.X TPos.Center
            p.Y TPos.Center) ])
    :> IView
  else
    let selectedIndex =
      model.SelectedId
      |> Option.bind (fun selectedId -> visible |> List.tryFindIndex (fun todo -> todo.Id = selectedId))

    View.ListView(fun (p: ListViewProps) ->
      p.Title title
      p.BorderStyle LineStyle.Rounded
      p.SchemeName(schemeName Schemes.Accent)
      p.X(at 1)
      p.Y(at 12)
      p.Width(Dim.Fill 2)
      p.Height(Dim.Fill 3)
      p.Source taskListState.Source
      p.Value(selectedIndex |> Option.toNullable)

      p.ValueChanged(fun args ->
        if not taskListState.IsSyncing && args.NewValue.HasValue then
          match taskListState.TryGetTodoId(args.NewValue.Value) with
          | Some todoId when model.SelectedId <> Some todoId -> dispatchMsg dispatch (Select(Some todoId))
          | _ -> ())

      p.Accepting(fun _ -> dispatchMsg dispatch ToggleSelected)

      p.KeyDown(fun key ->
        if key = Key.Space then
          dispatchMsg dispatch ToggleSelected
        elif key = Key.Delete then
          dispatchMsg dispatch DeleteSelected))
    :> IView

let private summary model dispatch =
  let openCount = activeCount model
  let doneCount = completedCount model
  let noun = if openCount = 1 then "task" else "tasks"

  [ View.Label(fun p ->
      p.Text $"{openCount} active {noun}  ·  {doneCount} completed"
      p.X(at 2)
      p.Y(TPos.AnchorEnd(Some 2))
      p.Width(Dim.Fill 24))
    :> IView
    View.Button(fun p ->
      p.Text "Clear completed"
      p.X(TPos.AnchorEnd(Some 19))
      p.Y(TPos.AnchorEnd(Some 2))
      p.Width 18
      p.NoDecorations true
      p.ShadowStyle ShadowStyles.None
      p.Enabled(doneCount > 0)
      p.Accepting(fun _ -> dispatchMsg dispatch ClearCompleted))
    :> IView ]

let private shortcut (key: Key) title enabled (action: unit -> unit) =
  View.Shortcut(fun (p: ShortcutProps) ->
    p.Key key
    p.Title title
    p.Enabled enabled
    p.BindKeyToApplication true
    p.Action action)
  :> IView

let private statusBar model dispatch =
  let hasSelection = model.SelectedId.IsSome

  let contextualShortcuts =
    match model.EditorMode with
    | EditorMode.Creating -> []
    | EditorMode.Editing _ -> [ shortcut Key.Esc "Cancel" true (fun () -> dispatchMsg dispatch CancelEdit) ]

  let shortcuts =
    [ shortcut Key.F4 "New" true (fun () -> dispatchMsg dispatch NewTodo)
      shortcut Key.F2 "Edit" hasSelection (fun () -> dispatchMsg dispatch BeginEdit)
      shortcut Key.F8 "Delete" hasSelection (fun () -> dispatchMsg dispatch DeleteSelected) ]
    @ contextualShortcuts
    @ [ shortcut Key.F12 "Quit" true (fun () -> dispatch TerminalMsg.Terminate) ]

  View.StatusBar(fun p ->
    p.X(at 0)
    p.Y(TPos.AnchorEnd(Some 1))
    p.Width(Dim.Fill())
    p.Height 1
    p.Children shortcuts)
  :> IView

let view model dispatch =
  let openCount = activeCount model
  let doneCount = completedCount model

  let header =
    [ View.Label(fun p ->
        p.Text " TASKS "
        p.SchemeName(schemeName Schemes.Accent)
        p.TextAlignment Alignment.Center
        p.X(at 1)
        p.Y(at 1)
        p.Width 9)
      :> IView
      View.Label(fun p ->
        p.Text "A quiet place to get things done."
        p.X(at 12)
        p.Y(at 1)
        p.Width(Dim.Fill 30))
      :> IView
      View.Label(fun p ->
        p.Text $"{openCount} active  ·  {doneCount} done"
        p.TextAlignment Alignment.End
        p.X(TPos.AnchorEnd(Some 27))
        p.Y(at 1)
        p.Width 26)
      :> IView
      View.ProgressBar(fun p ->
        p.X(at 1)
        p.Y(at 2)
        p.Width(Dim.Fill 2)
        p.Height 1
        p.Fraction(completionRatio model)
        p.ProgressBarStyle ProgressBarStyle.Continuous
        p.ProgressBarFormat ProgressBarFormat.SimplePlusPercentage)
      :> IView ]

  View.Runnable(fun (p: RunnableProps) ->
    p.SchemeName(schemeName Schemes.Base)

    p.Children(
      header
      @ [ composer model dispatch :> IView
          filterBar model dispatch :> IView
          taskArea model dispatch ]
      @ summary model dispatch
      @ [ statusBar model dispatch ]
    ))
  :> IView

[<EntryPoint>]
let main _ =
  ConfigurationManager.Enable(ConfigLocations.All)
  ElmishTerminal.mkSimple init update view |> ElmishTerminal.runTerminal
  0
