module Terminal.Gui.Elmish.Generator.String

open System

let lowerCamelCase (s: String) =
  if String.IsNullOrEmpty s then s
  else s.[0..0].ToLower() + s.[1..]

