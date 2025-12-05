module internal Terminal.Gui.Elmish.Interop

open Terminal.Gui.ViewBase

let getParent (view: View) : View option =
  if isNull view.SuperView then
    None
  else
    Some view.SuperView
