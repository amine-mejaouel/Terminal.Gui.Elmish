module Terminal.Gui.Elmish.Tests.VirtualTreeTests

open System
open System.Collections.Generic
open NUnit.Framework
open Terminal.Gui.Elmish
open Terminal.Gui.Input
open Terminal.Gui.ViewBase
open Terminal.Gui.Views

let private asSimpleSpec (view: IView) = view :?> ISimpleViewSpec

let private keyedLabel key text : IView =
  View.Label(fun p ->
    p.Key key
    p.Text text)
  :> IView

let private keyedButton key text : IView =
  View.Button(fun p ->
    p.Key key
    p.Text text)
  :> IView

let private keyedRoot (children: IView list) =
  View.Runnable(fun (p: RunnableProps) -> p.Children children) :> IView

let private render (renderer: VirtualTree.Renderer) (root: IView) =
  renderer.Render(asSimpleSpec root, Origin.Root)

let private subViews (view: View) = view.SubViews |> Seq.toArray

[<Test>]
let ``Property-only updates preserve view identity and unset removed properties`` () =
  use renderer = new VirtualTree.Renderer()

  let firstRoot = keyedRoot [ keyedLabel "label" "before" ] |> render renderer

  let firstLabel = (subViews firstRoot.View)[0] :?> Label

  let secondRoot = keyedRoot [ keyedLabel "label" "after" ] |> render renderer

  let secondLabel = (subViews secondRoot.View)[0] :?> Label

  Assert.Multiple(fun () ->
    Assert.That(secondRoot.View, Is.SameAs(firstRoot.View))
    Assert.That(secondLabel, Is.SameAs(firstLabel))
    Assert.That(secondLabel.Text.ToString(), Is.EqualTo("after")))

  let withoutText =
    keyedRoot [ View.Label(fun p -> p.Key "label") :> IView ] |> render renderer

  let labelWithoutText = (subViews withoutText.View)[0] :?> Label

  Assert.Multiple(fun () ->
    Assert.That(labelWithoutText, Is.SameAs(firstLabel))
    Assert.That(labelWithoutText.Text.ToString(), Is.EqualTo("")))

[<Test>]
let ``Clear dispatch handles properties declared by derived and base view types`` () =
  use renderer = new VirtualTree.Renderer()

  let rootWithProps includeProps =
    keyedRoot
      [ View.Button(fun p ->
          p.Key "button"

          if includeProps then
            p.Title "inherited title"
            p.NoPadding true)
        :> IView ]

  let initial = rootWithProps true |> render renderer
  let button = (subViews initial.View)[0] :?> Button

  let updated = rootWithProps false |> render renderer
  let retainedButton = (subViews updated.View)[0] :?> Button

  Assert.Multiple(fun () ->
    Assert.That(retainedButton, Is.SameAs(button))
    Assert.That(retainedButton.Title.ToString(), Is.EqualTo(""))
    Assert.That(retainedButton.NoPadding, Is.False))

[<Test>]
let ``Keyed prepend and reverse preserve existing views in exact requested order`` () =
  use renderer = new VirtualTree.Renderer()

  let initial =
    keyedRoot [ keyedLabel "a" "a"; keyedLabel "b" "b"; keyedLabel "c" "c" ]
    |> render renderer

  let byKey =
    initial.View.SubViews
    |> Seq.map (fun view -> view.Text.ToString(), view)
    |> dict

  let prepended =
    keyedRoot
      [ keyedLabel "x" "x"
        keyedLabel "a" "a"
        keyedLabel "b" "b"
        keyedLabel "c" "c" ]
    |> render renderer

  let prependedViews = subViews prepended.View

  Assert.Multiple(fun () ->
    Assert.That(prependedViews |> Array.map (_.Text.ToString()), Is.EqualTo(box [| "x"; "a"; "b"; "c" |]))
    Assert.That(prependedViews[1], Is.SameAs(byKey["a"]))
    Assert.That(prependedViews[2], Is.SameAs(byKey["b"]))
    Assert.That(prependedViews[3], Is.SameAs(byKey["c"])))

  let reversed =
    keyedRoot
      [ keyedLabel "c" "c"
        keyedLabel "b" "b"
        keyedLabel "a" "a"
        keyedLabel "x" "x" ]
    |> render renderer

  let reversedViews = subViews reversed.View

  Assert.Multiple(fun () ->
    Assert.That(reversedViews |> Array.map (_.Text.ToString()), Is.EqualTo(box [| "c"; "b"; "a"; "x" |]))
    Assert.That(reversedViews[0], Is.SameAs(byKey["c"]))
    Assert.That(reversedViews[1], Is.SameAs(byKey["b"]))
    Assert.That(reversedViews[2], Is.SameAs(byKey["a"])))

[<Test>]
let ``Duplicate and mixed keys fail before mutating the mounted hierarchy`` () =
  use renderer = new VirtualTree.Renderer()

  let mounted =
    keyedRoot [ keyedLabel "a" "a"; keyedLabel "b" "b" ] |> render renderer

  let before = subViews mounted.View

  let duplicateError =
    Assert.Throws<InvalidOperationException>(fun () ->
      keyedRoot [ keyedLabel "a" "replacement"; keyedLabel "a" "duplicate" ]
      |> render renderer
      |> ignore)

  Assert.That(duplicateError.Message, Does.Contain("duplicate key 'a'"))
  let afterDuplicate = subViews mounted.View
  Assert.That(afterDuplicate, Has.Length.EqualTo(before.Length))
  Array.iter2 (fun (actual: View) (expected: View) -> Assert.That(actual, Is.SameAs(expected))) afterDuplicate before

  let mixedError =
    Assert.Throws<InvalidOperationException>(fun () ->
      keyedRoot [ keyedLabel "a" "replacement"; View.Label(fun p -> p.Text "unkeyed") :> IView ]
      |> render renderer
      |> ignore)

  Assert.That(mixedError.Message, Does.Contain("cannot mix keyed and unkeyed"))
  let afterMixed = subViews mounted.View
  Assert.That(afterMixed, Has.Length.EqualTo(before.Length))
  Array.iter2 (fun (actual: View) (expected: View) -> Assert.That(actual, Is.SameAs(expected))) afterMixed before

[<Test>]
let ``Same key with a different exact view type replaces only that node`` () =
  use renderer = new VirtualTree.Renderer()

  let initial =
    keyedRoot [ keyedButton "changing" "button"; keyedLabel "stable" "stable" ]
    |> render renderer

  let initialViews = subViews initial.View
  let oldChanging = initialViews[0]
  let stable = initialViews[1]

  let updated =
    keyedRoot [ keyedLabel "changing" "label"; keyedLabel "stable" "stable" ]
    |> render renderer

  let updatedViews = subViews updated.View

  Assert.Multiple(fun () ->
    Assert.That(updatedViews[0], Is.TypeOf<Label>())
    Assert.That(updatedViews[0], Is.Not.SameAs(oldChanging))
    Assert.That(updatedViews[1], Is.SameAs(stable)))

[<Test>]
let ``View-valued property slots retain compatible views and never become SubViews`` () =
  use renderer = new VirtualTree.Renderer()

  let rootWithTarget (target: IView) =
    keyedRoot
      [ View.Shortcut(fun p ->
          p.Key "shortcut"
          p.TargetView target)
        :> IView ]

  let initial = rootWithTarget (keyedLabel "target" "before") |> render renderer
  let shortcut = (subViews initial.View)[0] :?> Shortcut
  let initialTarget = shortcut.TargetView

  rootWithTarget (keyedLabel "target" "after") |> render renderer |> ignore

  Assert.Multiple(fun () ->
    Assert.That(shortcut.TargetView, Is.SameAs(initialTarget))
    Assert.That(shortcut.TargetView.Text.ToString(), Is.EqualTo("after"))

    Assert.That(
      shortcut.SubViews
      |> Seq.exists (fun view -> obj.ReferenceEquals(view, shortcut.TargetView)),
      Is.False
    ))

  rootWithTarget (keyedButton "target" "replacement") |> render renderer |> ignore

  Assert.Multiple(fun () ->
    Assert.That(shortcut.TargetView, Is.TypeOf<Button>())
    Assert.That(shortcut.TargetView, Is.Not.SameAs(initialTarget))

    Assert.That(
      shortcut.SubViews
      |> Seq.exists (fun view -> obj.ReferenceEquals(view, shortcut.TargetView)),
      Is.False
    ))

[<Test>]
let ``Removing a view-valued property clears its owner before disposing the slot`` () =
  use renderer = new VirtualTree.Renderer()

  let rootWithTarget (target: IView option) =
    keyedRoot
      [ View.Shortcut(fun p ->
          p.Key "shortcut"
          target |> Option.iter (fun value -> p.TargetView value))
        :> IView ]

  let initial =
    rootWithTarget (Some(keyedLabel "target" "removable")) |> render renderer

  let shortcut = (subViews initial.View)[0] :?> Shortcut
  let target = shortcut.TargetView
  let mutable ownerWasClearedBeforeDispose = false

  target.Disposing.Add(fun _ -> ownerWasClearedBeforeDispose <- isNull shortcut.TargetView)
  rootWithTarget None |> render renderer |> ignore

  Assert.Multiple(fun () ->
    Assert.That(shortcut.TargetView, Is.Null)
    Assert.That(ownerWasClearedBeforeDispose, Is.True))

[<Test>]
let ``Ordinary property diff excludes declarative and native view-slot entries`` () =
  let slotProps = Props()
  use nativeTarget = new Label()

  slotProps
  |> Props.add (PKey.Shortcut.TargetView_viewSpec, keyedLabel "target" "declarative")

  slotProps |> Props.add (PKey.Shortcut.TargetView, nativeTarget)

  let removedOnAdd, changedOnAdd = Props.diff (Props(), slotProps)
  let removedOnDelete, changedOnDelete = Props.diff (slotProps, Props())

  Assert.Multiple(fun () ->
    Assert.That(removedOnAdd, Is.Empty)
    Assert.That(changedOnAdd.IsNone, Is.True)
    Assert.That(removedOnDelete, Is.Empty)
    Assert.That(changedOnDelete.IsNone, Is.True))

[<Test>]
let ``Ordinary property diff returns exact removed value and event keys`` () =
  let previous = Props()
  previous |> Props.add (PKey.View.Title, "removed")
  previous |> Props.add (PKey.View.Accepting, ignore)

  let removed, changed = Props.diff (previous, Props())

  Assert.Multiple(fun () ->
    Assert.That(removed, Has.Length.EqualTo(2))
    Assert.That(removed, Does.Contain(PKey.View.Title.Untyped))
    Assert.That(removed, Does.Contain(PKey.View.Accepting.Untyped))
    Assert.That(changed.IsNone, Is.True))

[<Test>]
let ``Event properties use one stable subscription with the latest callback`` () =
  use renderer = new VirtualTree.Renderer()
  let calls = ResizeArray<int>()

  let rootWithHandler handler =
    keyedRoot
      [ View.Button(fun p ->
          p.Key "button"
          p.Text "invoke"

          handler |> Option.iter (fun value -> p.Accepting(fun _ -> calls.Add value)))
        :> IView ]

  let initial = rootWithHandler (Some 1) |> render renderer
  let button = (subViews initial.View)[0] :?> Button
  button.InvokeCommand(Command.Accept) |> ignore

  let updated = rootWithHandler (Some 2) |> render renderer
  let retainedButton = (subViews updated.View)[0] :?> Button
  retainedButton.InvokeCommand(Command.Accept) |> ignore

  rootWithHandler None |> render renderer |> ignore
  retainedButton.InvokeCommand(Command.Accept) |> ignore

  Assert.Multiple(fun () ->
    Assert.That(retainedButton, Is.SameAs(button))
    Assert.That(calls, Is.EqualTo(box [| 1; 2 |])))

[<Test>]
let ``Randomized keyed edits preserve the reference associated with every surviving key`` () =
  use renderer = new VirtualTree.Renderer()
  let random = Random(7319)
  let expectedViews = Dictionary<string, View>()
  let mutable keys: string list = []
  let mutable nextKey = 0

  for _ = 0 to 149 do
    match random.Next(3), keys with
    | 0, _
    | _, [] ->
      let key = $"k{nextKey}"
      nextKey <- nextKey + 1
      let index = random.Next(keys.Length + 1)
      keys <- keys |> List.insertAt index key
    | 1, _ ->
      let index = random.Next(keys.Length)
      keys <- keys |> List.removeAt index
    | _, _ when keys.Length > 1 ->
      let fromIndex = random.Next(keys.Length)
      let key = keys[fromIndex]
      let without = keys |> List.removeAt fromIndex
      let targetIndex = random.Next(without.Length + 1)
      keys <- without |> List.insertAt targetIndex key
    | _ -> ()

    let mounted =
      keys |> List.map (fun key -> keyedLabel key key) |> keyedRoot |> render renderer

    let actual = subViews mounted.View
    Assert.That(actual |> Array.map (_.Text.ToString()), Is.EqualTo(box (keys |> List.toArray)))

    for index = 0 to keys.Length - 1 do
      let key = keys[index]

      match expectedViews.TryGetValue key with
      | true, previous -> Assert.That(actual[index], Is.SameAs(previous), $"View identity changed for key '{key}'.")
      | false, _ -> expectedViews[key] <- actual[index]

    let live = HashSet<string>(keys, StringComparer.Ordinal)

    for stale in expectedViews.Keys |> Seq.filter (live.Contains >> not) |> Seq.toArray do
      expectedViews.Remove stale |> ignore

[<Test>]
let ``Component state and view identity follow the key across a parent reorder`` () =
  task {
    let mutable incrementA: (unit -> System.Threading.Tasks.Task) option = None

    let createComponent key =
      let value = TestComponent.create (fun p -> p.text key)
      (value :> IComponentViewSpec).ComponentProps.Key <- Some key

      if key = "a" && incrementA.IsNone then
        incrementA <-
          Some(fun () ->
            task {
              let! _ = value.ProcessMsg(TestComponent.Increment |> TerminalMsg.ofMsg)
              return ()
            }
            :> System.Threading.Tasks.Task)

      value :> IView

    let init _ = [ "a"; "b" ]
    let update (next: string list) _ = next

    let view keys _ : IView =
      keys |> List.map createComponent |> keyedRoot

    use program = ElmishTester.runSimple init update view
    do! incrementA.Value()

    let before = program.View.SubViews |> Seq.toArray
    do! program.ProcessMsg([ "b"; "a" ] |> TerminalMsg.ofMsg)
    let after = program.View.SubViews |> Seq.toArray
    let componentAChildren = after[1] |> subViews

    Assert.Multiple(fun () ->
      Assert.That(after[0], Is.SameAs(before[1]))
      Assert.That(after[1], Is.SameAs(before[0]))
      Assert.That(componentAChildren[0].Text.ToString(), Is.EqualTo("Counter: 1")))
  }

[<Test>]
let ``Removing a component terminates its loop and detaches its rendered root`` () =
  use renderer = new VirtualTree.Renderer()
  let component = TestComponent.create (fun p -> p.text "removable")
  (component :> IComponentViewSpec).ComponentProps.Key <- Some "component"

  let mounted = keyedRoot [ component :> IView ] |> render renderer
  let componentView = (subViews mounted.View)[0]

  let updated = keyedRoot [] |> render renderer

  Assert.Multiple(fun () ->
    Assert.That(subViews updated.View, Is.Empty)
    Assert.That(componentView.SuperView, Is.Null))
