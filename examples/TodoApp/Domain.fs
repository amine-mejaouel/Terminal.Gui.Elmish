module TodoApp.Domain

type Todo =
  { Id: int
    Title: string
    IsCompleted: bool }

[<RequireQualifiedAccess>]
type Filter =
  | All
  | Active
  | Completed

[<RequireQualifiedAccess>]
type EditorMode =
  | Creating
  | Editing of todoId: int

type Model =
  { Todos: Todo list
    Filter: Filter
    SelectedId: int option
    Draft: string
    EditorMode: EditorMode
    NextId: int
    FocusRevision: int
    FocusReady: bool }

type Msg =
  | DraftChanged of string
  | ComposerMounted of revision: int
  | SubmitDraft
  | NewTodo
  | BeginEdit
  | CancelEdit
  | Select of int option
  | ToggleSelected
  | DeleteSelected
  | SetFilter of Filter
  | ClearCompleted

let private initialTodos =
  [ { Id = 1
      Title = "Sketch the polished layout"
      IsCompleted = true }
    { Id = 2
      Title = "Model the Elmish update flow"
      IsCompleted = true }
    { Id = 3
      Title = "Add thoughtful keyboard shortcuts"
      IsCompleted = false }
    { Id = 4
      Title = "Ship a delightful terminal app"
      IsCompleted = false } ]

let init () =
  { Todos = initialTodos
    Filter = Filter.All
    SelectedId = initialTodos |> List.tryHead |> Option.map _.Id
    Draft = ""
    EditorMode = EditorMode.Creating
    NextId = initialTodos.Length + 1
    FocusRevision = 0
    FocusReady = false }

let visibleTodos model =
  model.Todos
  |> List.filter (fun todo ->
    match model.Filter with
    | Filter.All -> true
    | Filter.Active -> not todo.IsCompleted
    | Filter.Completed -> todo.IsCompleted)

let activeCount model =
  model.Todos |> List.filter (not << _.IsCompleted) |> List.length

let completedCount model = model.Todos.Length - activeCount model

let completionRatio model =
  if model.Todos.IsEmpty then
    0.0f
  else
    float32 (completedCount model) / float32 model.Todos.Length

let selectedTodo model =
  model.SelectedId
  |> Option.bind (fun selectedId -> model.Todos |> List.tryFind (fun todo -> todo.Id = selectedId))

let private normalizeSelection preferredIndex model =
  let visible = visibleTodos model

  let selectedIsVisible =
    model.SelectedId
    |> Option.exists (fun selectedId -> visible |> List.exists (fun todo -> todo.Id = selectedId))

  if selectedIsVisible then
    model
  else
    let nextSelection =
      if visible.IsEmpty then
        None
      else
        let index =
          preferredIndex |> Option.defaultValue 0 |> max 0 |> min (visible.Length - 1)

        Some visible[index].Id

    { model with
        SelectedId = nextSelection }

let private cancelEditorIfMissing model =
  match model.EditorMode with
  | EditorMode.Creating -> model
  | EditorMode.Editing todoId ->
    if model.Todos |> List.exists (fun todo -> todo.Id = todoId) then
      model
    else
      { model with
          Draft = ""
          EditorMode = EditorMode.Creating
          FocusRevision = model.FocusRevision + 1
          FocusReady = false }

let private cancelEditorIfHidden model =
  match model.EditorMode with
  | EditorMode.Creating -> model
  | EditorMode.Editing todoId ->
    if visibleTodos model |> List.exists (fun todo -> todo.Id = todoId) then
      model
    else
      { model with
          Draft = ""
          EditorMode = EditorMode.Creating
          FocusRevision = model.FocusRevision + 1
          FocusReady = false }

let private selectedVisibleIndex model =
  model.SelectedId
  |> Option.bind (fun selectedId -> visibleTodos model |> List.tryFindIndex (fun todo -> todo.Id = selectedId))

let private updateTodo todoId updateTodo todos =
  todos
  |> List.map (fun todo -> if todo.Id = todoId then updateTodo todo else todo)

let update msg model =
  match msg with
  | DraftChanged draft -> { model with Draft = draft }
  | ComposerMounted revision ->
    if revision = model.FocusRevision then
      { model with FocusReady = true }
    else
      model
  | NewTodo ->
    { model with
        Draft = ""
        EditorMode = EditorMode.Creating
        FocusRevision = model.FocusRevision + 1
        FocusReady = false }
  | BeginEdit ->
    match selectedTodo model with
    | None -> model
    | Some todo ->
      { model with
          Draft = todo.Title
          EditorMode = EditorMode.Editing todo.Id
          FocusRevision = model.FocusRevision + 1
          FocusReady = false }
  | CancelEdit ->
    { model with
        Draft = ""
        EditorMode = EditorMode.Creating
        FocusRevision = model.FocusRevision + 1
        FocusReady = false }
  | SubmitDraft ->
    let title = model.Draft.Trim()

    if title = "" then
      model
    else
      match model.EditorMode with
      | EditorMode.Creating ->
        let todo =
          { Id = model.NextId
            Title = title
            IsCompleted = false }

        { model with
            Todos = model.Todos @ [ todo ]
            Filter = Filter.All
            Draft = ""
            NextId = model.NextId + 1
            FocusRevision = model.FocusRevision + 1
            FocusReady = false }
      | EditorMode.Editing todoId ->
        { model with
            Todos = model.Todos |> updateTodo todoId (fun todo -> { todo with Title = title })
            SelectedId = Some todoId
            Draft = ""
            EditorMode = EditorMode.Creating
            FocusRevision = model.FocusRevision + 1
            FocusReady = false }
  | Select selectedId -> { model with SelectedId = selectedId }
  | ToggleSelected ->
    match model.SelectedId with
    | None -> model
    | Some selectedId ->
      let oldIndex = selectedVisibleIndex model

      { model with
          Todos =
            model.Todos
            |> updateTodo selectedId (fun todo ->
              { todo with
                  IsCompleted = not todo.IsCompleted }) }
      |> normalizeSelection oldIndex
      |> cancelEditorIfHidden
  | DeleteSelected ->
    match model.SelectedId with
    | None -> model
    | Some selectedId ->
      let oldIndex = selectedVisibleIndex model

      { model with
          Todos = model.Todos |> List.filter (fun todo -> todo.Id <> selectedId)
          SelectedId = None }
      |> cancelEditorIfMissing
      |> normalizeSelection oldIndex
  | SetFilter filter ->
    { model with Filter = filter }
    |> normalizeSelection None
    |> cancelEditorIfHidden
  | ClearCompleted ->
    let oldIndex = selectedVisibleIndex model

    { model with
        Todos = model.Todos |> List.filter (not << _.IsCompleted)
        SelectedId =
          model.SelectedId
          |> Option.filter (fun selectedId ->
            model.Todos
            |> List.exists (fun todo -> todo.Id = selectedId && not todo.IsCompleted)) }
    |> cancelEditorIfMissing
    |> normalizeSelection oldIndex
