namespace Terminal.Gui.Elmish

open System
open System.Collections.Generic
open System.Collections.ObjectModel
open Terminal.Gui.Views

type private ReconciledListRow(key: obj, text: string) =
  member _.Key = key
  member _.Text = text
  override _.ToString() = text

type internal ReconciledListItem = { Key: obj; Text: string }

[<RequireQualifiedAccess>]
module private ReconciledListValidation =
  let sourceOwnership (tryGet: PropKey -> obj option) =
    let rejectRawSource key controlName =
      if (tryGet key).IsSome then
        invalidOp
          $"{controlName} cannot use both p.Source and m.Items. Remove p.Source when the reconciled items macro owns the data source."

    rejectRawSource PKey.ListView.Source.Untyped "ListView"
    rejectRawSource PKey.DropDownList.Source.Untyped "DropDownList"

type private ReconciledListState() =
  let rows = ObservableCollection<ReconciledListRow>()
  let source = new ListWrapper<ReconciledListRow>(rows)
  let mutable detachSource: (unit -> unit) option = None
  let mutable disposed = false

  let findRow startIndex key =
    seq { startIndex .. rows.Count - 1 }
    |> Seq.tryFind (fun index -> Object.Equals(rows[index].Key, key))

  let sync (items: ReconciledListItem list) =
    items
    |> List.iteri (fun index item ->
      if index < rows.Count && Object.Equals(rows[index].Key, item.Key) then
        if rows[index].Text <> item.Text then
          rows[index] <- ReconciledListRow(item.Key, item.Text)
      else
        match findRow index item.Key with
        | Some existingIndex ->
          rows.Move(existingIndex, index)

          if rows[index].Text <> item.Text then
            rows[index] <- ReconciledListRow(item.Key, item.Text)
        | None -> rows.Insert(index, ReconciledListRow(item.Key, item.Text)))

    while rows.Count > items.Length do
      rows.RemoveAt(rows.Count - 1)

  let nullableIndex value =
    if value >= 0 && value < rows.Count then
      Nullable value
    else
      Nullable()

  let reconcileSource (eventKeys: PropKey seq) (suppress: PropKey seq -> IDisposable) getSource setSource reconcile =
    let suppressEvents () = suppress eventKeys

    detachSource <-
      Some(fun () ->
        if obj.ReferenceEquals(getSource (), source) then
          use _scope = suppressEvents ()
          setSource null)

    use _scope = suppressEvents ()

    if not (obj.ReferenceEquals(getSource (), source)) then
      setSource source

    reconcile ()

  let applyListView items (context: ReconciledPropApplyContext) (listView: ListView) =
    let controlledIndex =
      context.TryGetProperty PKey.ListView.Value.Untyped
      |> Option.orElseWith (fun () -> context.TryGetProperty PKey.ListView.SelectedItem.Untyped)
      |> Option.map unbox<Nullable<int>>

    let selectedKey =
      if listView.Value.HasValue && listView.Value.Value < rows.Count then
        Some rows[listView.Value.Value].Key
      else
        None

    reconcileSource
      PKey.ReconciledEventKeys.ListViewItems
      context.SuppressEvents
      (fun () -> listView.Source)
      (fun value -> listView.Source <- value)
      (fun () ->
        sync items

        match controlledIndex with
        | Some index ->
          listView.Value <-
            if index.HasValue then
              nullableIndex index.Value
            else
              Nullable()
        | None ->
          let retainedIndex =
            selectedKey
            |> Option.bind (fun key -> rows |> Seq.tryFindIndex (fun row -> Object.Equals(row.Key, key)))

          listView.Value <- retainedIndex |> Option.map nullableIndex |> Option.defaultValue (Nullable()))

  let applyDropDownList items (context: ReconciledPropApplyContext) (dropDown: DropDownList) =
    let currentValue = dropDown.Value

    reconcileSource
      PKey.ReconciledEventKeys.DropDownListItems
      context.SuppressEvents
      (fun () -> dropDown.Source)
      (fun value -> dropDown.Source <- value)
      (fun () ->
        sync items

        let desiredValue =
          context.TryGetProperty PKey.TextField.Value.Untyped
          |> Option.map unbox<string>
          |> Option.defaultValue currentValue

        dropDown.Value <- desiredValue)

  member _.Apply(items: ReconciledListItem list, context: ReconciledPropApplyContext) =
    if disposed then
      ObjectDisposedException(nameof ReconciledListState) |> raise

    ReconciledListValidation.sourceOwnership context.TryGetProperty

    match context.NativeView with
    | :? ListView as listView -> applyListView items context listView
    | :? DropDownList as dropDown -> applyDropDownList items context dropDown
    | view -> invalidOp $"The reconciled items adapter does not support '{view.GetType().FullName}'."

  interface IReconciledState

  interface IDisposable with
    member _.Dispose() =
      if not disposed then
        disposed <- true

        detachSource |> Option.iter (fun detach -> detach ())

        detachSource <- None
        source.Dispose()
        rows.Clear()

[<RequireQualifiedAccess>]
module internal ReconciledListItems =
  let create (reconciledKey: string) (items: ReconciledListItem list) =
    let keys = HashSet<obj>()

    for item in items do
      if isNull item.Key then
        invalidArg (nameof items) "Reconciled item keys cannot be null."

      if not (keys.Add item.Key) then
        invalidArg (nameof items) $"Reconciled items contain the duplicate key '{item.Key}'."

    ReconciledPropSpec.create
      reconciledKey
      { Phase = ReconciledPropPhase.BeforeNative
        Value = items
        Validate = ReconciledListValidation.sourceOwnership
        CreateState = fun () -> new ReconciledListState()
        Apply = fun state context -> state.Apply(items, context) }
