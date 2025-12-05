namespace Terminal.Gui.Elmish

open System.Linq.Expressions
open System
open Terminal.Gui.ViewBase

[<RequireQualifiedAccess>]
module Interop =

  let rec getParent (view: View) =
    view.SuperView
    |> Option.ofObj
    |> Option.bind (fun p ->
      if p.GetType().Name.Contains("Content") then
        getParent p
      else
        Some p
    )
