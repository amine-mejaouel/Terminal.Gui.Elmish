module Terminal.Gui.Elmish.Generator.File

open System.IO

let writeAllText path text =
  File.WriteAllText (path, text)
