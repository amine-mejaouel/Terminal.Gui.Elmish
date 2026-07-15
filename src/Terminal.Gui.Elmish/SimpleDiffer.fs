module Terminal.Gui.Elmish.SimpleDiffer

type IVirtualTerminalTree = interface end

let internal update (prev: ViewSpec) (next: ViewSpec) (vtt: IVirtualTerminalTree) = ()
// match prev, next with
// | ComponentViewSpec _, SimpleViewSpec _
// | SimpleViewSpec _, ComponentViewSpec _ -> failwith "Incompatible view types, this should never happen."
// | SimpleViewSpec prev, SimpleViewSpec next ->
//   PropsApplier.applyProps prev next
//   vtt.Patch(prev, next)
//
//
// | ComponentViewSpec componentViewSpec, ComponentViewSpec viewSpec -> failwith "todo"
