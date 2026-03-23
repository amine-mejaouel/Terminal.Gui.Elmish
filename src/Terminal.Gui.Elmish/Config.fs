namespace Terminal.Gui.Elmish

[<RequireQualifiedAccess>]
type Differ =
  | Simple
  | Keyed

module Config =
  let curDiffer = Differ.Keyed
