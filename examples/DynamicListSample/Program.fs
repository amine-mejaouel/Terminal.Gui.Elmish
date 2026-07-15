open Elmish
open Terminal.Gui.Elmish
open Terminal.Gui.ViewBase

type WorkItem =
  { Id: int
    Title: string
    Revision: int }

type Model =
  { Items: WorkItem list
    SelectedId: int option
    NextId: int }

type Msg =
  | Add
  | RemoveSelected
  | UpdateSelected
  | MoveSelected of offset: int
  | Reverse
  | Select of id: int

let private initialItems =
  [ { Id = 1
      Title = "Read input"
      Revision = 0 }
    { Id = 2
      Title = "Update model"
      Revision = 0 }
    { Id = 3
      Title = "Reconcile virtual tree"
      Revision = 0 }
    { Id = 4
      Title = "Render changed views"
      Revision = 0 } ]

let init () =
  { Items = initialItems
    SelectedId = Some 2
    NextId = initialItems.Length + 1 }

let private moveItem offset selectedId items =
  let items = List.toArray items

  match items |> Array.tryFindIndex (fun item -> item.Id = selectedId) with
  | None -> Array.toList items
  | Some index ->
    let destination = max 0 (min (items.Length - 1) (index + offset))

    if destination <> index then
      let displaced = items[destination]
      items[destination] <- items[index]
      items[index] <- displaced

    Array.toList items

let update msg model =
  match msg with
  | Add ->
    let item =
      { Id = model.NextId
        Title = $"Dynamic item {model.NextId}"
        Revision = 0 }

    { model with
        Items = item :: model.Items
        SelectedId = Some item.Id
        NextId = model.NextId + 1 }
  | RemoveSelected ->
    match model.SelectedId with
    | None -> model
    | Some selectedId ->
      let remaining = model.Items |> List.filter (fun item -> item.Id <> selectedId)

      { model with
          Items = remaining
          SelectedId = remaining |> List.tryHead |> Option.map _.Id }
  | UpdateSelected ->
    match model.SelectedId with
    | None -> model
    | Some selectedId ->
      let updateItem item =
        if item.Id = selectedId then
          { item with
              Revision = item.Revision + 1 }
        else
          item

      { model with
          Items = model.Items |> List.map updateItem }
  | MoveSelected offset ->
    match model.SelectedId with
    | None -> model
    | Some selectedId ->
      { model with
          Items = moveItem offset selectedId model.Items }
  | Reverse ->
    { model with
        Items = List.rev model.Items }
  | Select id -> { model with SelectedId = Some id }

let private canMove offset model =
  match model.SelectedId with
  | None -> false
  | Some selectedId ->
    match model.Items |> List.tryFindIndex (fun item -> item.Id = selectedId) with
    | None -> false
    | Some index ->
      let destination = index + offset
      destination >= 0 && destination < model.Items.Length

let private toolbar model dispatch =
  let dispatchMsg msg = dispatch (TerminalMsg.ofMsg msg)
  let hasSelection = model.SelectedId.IsSome
  let at = TPos.Absolute

  View.FrameView(fun p ->
    p.Title "Operations"
    p.Height 6
    p.Width(Dim.Fill())

    p.Children
      [ View.Button(fun p ->
          p.Text "_Add"
          p.X(at 0)
          p.Y(at 0)
          p.Accepting(fun _ -> dispatchMsg Add))
        View.Button(fun p ->
          p.Text "_Remove"
          p.X(at 10)
          p.Y(at 0)
          p.Enabled hasSelection
          p.Accepting(fun _ -> dispatchMsg RemoveSelected))
        View.Button(fun p ->
          p.Text "_Update"
          p.X(at 23)
          p.Y(at 0)
          p.Enabled hasSelection
          p.Accepting(fun _ -> dispatchMsg UpdateSelected))
        View.Button(fun p ->
          p.Text "Re_verse"
          p.X(at 36)
          p.Y(at 0)
          p.Enabled(model.Items.Length > 1)
          p.Accepting(fun _ -> dispatchMsg Reverse))
        View.Button(fun p ->
          p.Text "Move _up"
          p.X(at 0)
          p.Y(at 1)
          p.Enabled(canMove -1 model)
          p.Accepting(fun _ -> dispatchMsg (MoveSelected -1)))
        View.Button(fun p ->
          p.Text "Move _down"
          p.X(at 13)
          p.Y(at 1)
          p.Enabled(canMove 1 model)
          p.Accepting(fun _ -> dispatchMsg (MoveSelected 1)))
        View.Button(fun p ->
          p.Text "_Quit"
          p.X(at 29)
          p.Y(at 1)
          p.Accepting(fun _ -> dispatch TerminalMsg.Terminate))
        View.Label(fun p ->
          let selected = model.SelectedId |> Option.map string |> Option.defaultValue "none"
          p.Text $"Rows: {model.Items.Length}  Selected key: {selected}"
          p.X(at 42)
          p.Y(at 1)
          p.Width(Dim.Fill())) ])

let private itemRow selectedId dispatch row item =
  let selected = selectedId = Some item.Id
  let marker = if selected then ">" else " "

  View.Button(fun p ->
    // This key, rather than the row index, gives the retained view stable identity.
    p.Key $"work-item-{item.Id}"
    p.Text $"{marker} [{item.Id}] {item.Title}  (revision {item.Revision})"
    p.X(TPos.Absolute 0)
    p.Y(TPos.Absolute row)
    p.Width(Dim.Fill())
    p.Accepting(fun _ -> dispatch (Select item.Id |> TerminalMsg.ofMsg)))
  :> IView

let view model dispatch =
  let list =
    View.FrameView(fun p ->
      p.Title "Keyed virtual children — select a row, then mutate the tree"
      p.Y(TPos.Absolute 6)
      p.Height(Dim.Fill())
      p.Width(Dim.Fill())

      model.Items |> List.mapi (itemRow model.SelectedId dispatch) |> p.Children)

  View.Runnable [ toolbar model dispatch :> IView; list ] :> IView

[<EntryPoint>]
let main _ =
  ElmishTerminal.mkSimple init update view |> ElmishTerminal.runTerminal
  0
