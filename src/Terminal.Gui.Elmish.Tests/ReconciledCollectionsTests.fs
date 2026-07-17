module Terminal.Gui.Elmish.Tests.ReconciledCollectionsTests

open System
open System.Collections.ObjectModel
open System.Reflection
open NUnit.Framework
open Terminal.Gui.Elmish
open Terminal.Gui.Input
open Terminal.Gui.Views

type private Item = { Id: int; Text: string }

let private asSimpleSpec (view: IView) = view :?> ISimpleViewSpec

let private render (renderer: VirtualTree.Renderer) (root: IView) =
  renderer.Render(asSimpleSpec root, Origin.Root)

let private root children =
  View.Runnable(fun (p: RunnableProps) -> p.Children children) :> IView

let private listView items selected changed title =
  View.ListView(fun (p: ListViewProps) (m: ListViewMacros) ->
    p.Key "list"
    p.Title title
    selected |> Option.iter (Nullable >> p.Value)
    changed |> Option.iter p.ValueChanged
    m.Items(items, (fun item -> item.Id), (fun item -> item.Text)))
  :> IView

let private mountedList (root: IViewTE) =
  root.View.SubViews |> Seq.item 0 :?> ListView

let private sourceText (source: IListDataSource) =
  source.ToList() |> Seq.cast<obj> |> Seq.map string |> Seq.toArray

let private behavioralEventKeys (viewType: Type) =
  let rec collectTypes current result =
    if isNull current || current = typeof<Terminal.Gui.ViewBase.View> then
      result
    else
      collectTypes current.BaseType (current :: result)

  collectTypes viewType []
  |> List.collect (fun declaringType ->
    declaringType.GetEvents(BindingFlags.Public ||| BindingFlags.Instance ||| BindingFlags.DeclaredOnly)
    |> Array.filter (fun event -> event.AddMethod.IsPublic && event.RemoveMethod.IsPublic)
    |> Array.sortBy _.Name
    |> Array.map (fun event -> $"{declaringType.Name}.{event.Name}_event")
    |> List.ofArray)
  |> List.toArray

[<Test>]
let ``Generated ListView suppression events track Terminal.Gui except rendering callbacks`` () =
  let expected =
    behavioralEventKeys typeof<ListView>
    |> Array.except [| "ListView.RowRender_event" |]

  let actual = PKey.ReconciledEventKeys.ListViewItems |> Array.map _.Key

  Assert.Multiple(fun () ->
    Assert.That(actual, Is.EqualTo(box expected))
    Assert.That(actual, Does.Not.Contain("ListView.RowRender_event")))

[<Test>]
let ``Generated DropDownList suppression events track Terminal.Gui behavioral inheritance`` () =
  let expected = behavioralEventKeys typeof<DropDownList>

  let actual = PKey.ReconciledEventKeys.DropDownListItems |> Array.map _.Key

  Assert.That(actual, Is.EqualTo(box expected))

[<Test>]
let ``List items mount before controlled selection and source identity survives unrelated renders`` () =
  use renderer = new VirtualTree.Renderer()

  let items =
    [ { Id = 1; Text = "one" }
      { Id = 2; Text = "two" }
      { Id = 3; Text = "three" } ]

  let initial = root [ listView items (Some 1) None "before" ] |> render renderer
  let list = mountedList initial
  let source = list.Source

  Assert.Multiple(fun () ->
    Assert.That(list.Value, Is.EqualTo(Nullable 1))
    Assert.That(sourceText source, Is.EqualTo(box [| "one"; "two"; "three" |])))

  let updated = root [ listView items (Some 1) None "after" ] |> render renderer
  let retained = mountedList updated

  Assert.Multiple(fun () ->
    Assert.That(retained, Is.SameAs(list))
    Assert.That(retained.Source, Is.SameAs(source))
    Assert.That(retained.Value, Is.EqualTo(Nullable 1)))

[<Test>]
let ``Keyed list edits retain the adapter and establish exact order`` () =
  use renderer = new VirtualTree.Renderer()

  let initialItems =
    [ { Id = 1; Text = "one" }
      { Id = 2; Text = "two" }
      { Id = 3; Text = "three" } ]

  let initial = root [ listView initialItems None None "items" ] |> render renderer
  let list = mountedList initial
  let source = list.Source

  let changedItems =
    [ { Id = 3; Text = "THREE" }
      { Id = 4; Text = "four" }
      { Id = 1; Text = "one" } ]

  root [ listView changedItems None None "items" ] |> render renderer |> ignore

  Assert.Multiple(fun () ->
    Assert.That(list.Source, Is.SameAs(source))
    Assert.That(sourceText source, Is.EqualTo(box [| "THREE"; "four"; "one" |])))

[<Test>]
let ``Collection reconciliation suppresses synthetic selection messages but native navigation still dispatches`` () =
  use renderer = new VirtualTree.Renderer()
  let mutable changes = 0

  let onChanged _ = changes <- changes + 1

  let items =
    [ { Id = 1; Text = "one" }
      { Id = 2; Text = "two" }
      { Id = 3; Text = "three" } ]

  let initial =
    root [ listView items (Some 0) (Some onChanged) "items" ] |> render renderer

  let list = mountedList initial

  let reordered =
    [ { Id = 3; Text = "three" }
      { Id = 1; Text = "one" }
      { Id = 2; Text = "two" } ]

  root [ listView reordered (Some 0) (Some onChanged) "items" ]
  |> render renderer
  |> ignore

  Assert.That(changes, Is.Zero)

  list.NewKeyDownEvent Key.CursorDown |> ignore

  Assert.Multiple(fun () ->
    Assert.That(list.Value, Is.EqualTo(Nullable 1))
    Assert.That(changes, Is.EqualTo(1)))

[<Test>]
let ``Each mounted list owns an isolated source and removal releases ownership`` () =
  use renderer = new VirtualTree.Renderer()
  let items = [ { Id = 1; Text = "one" } ]

  let twoLists =
    root
      [ listView items None None "first"
        View.ListView(fun (p: ListViewProps) (m: ListViewMacros) ->
          p.Key "other"
          m.Items(items, (fun item -> item.Id), (fun item -> item.Text)))
        :> IView ]

  let mounted = twoLists |> render renderer
  let first = mounted.View.SubViews |> Seq.item 0 :?> ListView
  let second = mounted.View.SubViews |> Seq.item 1 :?> ListView

  Assert.That(first.Source, Is.Not.SameAs(second.Source))

  let withoutMacro =
    root
      [ View.ListView(fun (p: ListViewProps) -> p.Key "list") :> IView
        View.ListView(fun (p: ListViewProps) -> p.Key "other") :> IView ]

  withoutMacro |> render renderer |> ignore

  Assert.Multiple(fun () ->
    Assert.That(first.Source, Is.Null)
    Assert.That(second.Source, Is.Null))

[<Test>]
let ``Raw sources remain supported and cannot be combined with managed items`` () =
  use renderer = new VirtualTree.Renderer()
  let rows = ObservableCollection<string>([ "raw" ])
  use source = new ListWrapper<string>(rows)

  let raw =
    root
      [ View.ListView(fun (p: ListViewProps) ->
          p.Key "list"
          p.Source source)
        :> IView ]
    |> render renderer

  let list = mountedList raw
  Assert.That(list.Source, Is.SameAs(source))

  root
    [ View.ListView(fun (p: ListViewProps) (m: ListViewMacros) ->
        p.Key "list"
        m.Items [ "managed" ])
      :> IView ]
  |> render renderer
  |> ignore

  let managedSource = list.Source

  Assert.Multiple(fun () ->
    Assert.That(managedSource, Is.Not.SameAs(source))
    Assert.That(sourceText managedSource, Is.EqualTo(box [| "managed" |])))

  let error =
    Assert.Throws<InvalidOperationException>(fun () ->
      root
        [ View.ListView(fun (p: ListViewProps) (m: ListViewMacros) ->
            p.Key "list"
            p.Source source
            m.Items [ "managed" ])
          :> IView ]
      |> render renderer
      |> ignore)

  Assert.Multiple(fun () ->
    Assert.That(error.Message, Does.Contain("cannot use both p.Source and m.Items"))
    Assert.That(list.Source, Is.SameAs(managedSource)))

[<Test>]
let ``Duplicate keys fail with a clear error`` () =
  let error =
    Assert.Throws<ArgumentException>(fun () ->
      View.ListView(fun (_: ListViewProps) (m: ListViewMacros) ->
        m.Items(
          [ { Id = 1; Text = "one" }; { Id = 1; Text = "duplicate" } ],
          (fun item -> item.Id),
          (fun item -> item.Text)
        ))
      |> ignore)

  Assert.That(error.Message, Does.Contain("duplicate key '1'"))

[<Test>]
let ``DropDownList reuses and releases the same retained items adapter`` () =
  use renderer = new VirtualTree.Renderer()

  let dropdown values =
    root
      [ View.DropDownList(fun (p: DropDownListProps) (m: DropDownListMacros) ->
          p.Key "dropdown"
          m.Items values)
        :> IView ]

  let initial = dropdown [ "one"; "two" ] |> render renderer
  let view = initial.View.SubViews |> Seq.item 0 :?> DropDownList
  let source = view.Source

  dropdown [ "two"; "three" ] |> render renderer |> ignore

  Assert.Multiple(fun () ->
    Assert.That(view.Source, Is.SameAs(source))
    Assert.That(sourceText source, Is.EqualTo(box [| "two"; "three" |])))

  root [ View.DropDownList(fun (p: DropDownListProps) -> p.Key "dropdown") :> IView ]
  |> render renderer
  |> ignore

  Assert.That(view.Source, Is.Null)
