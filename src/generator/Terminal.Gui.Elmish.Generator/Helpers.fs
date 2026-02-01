namespace Terminal.Gui.Elmish.Generator

open System
open System.IO

module String =
  let escapeReservedKeywords (name: string) =
    match name with
    | "type" | "module" | "open" | "let" | "in" | "do" | "if" | "then" | "else"
    | "match" | "with" | "function" | "val" | "mutable" | "when" | "rec"
    | "begin" | "end" | "for" | "to" | "done" | "while" | "and" | "or"
    | "not" | "true" | "false" | "namespace" | "as" | "assert" | "inherit" -> $"``{name}``"
    | _ -> name

module CodeWriter =
  let write (fileName: string) (text: string seq) =
    let outputPath = Path.Combine (Environment.CurrentDirectory, fileName)
    let text = String.concat Environment.NewLine text
    File.WriteAllText (outputPath, text)

[<AutoOpen>]
module TypeExtensions =
  type Type with
    member this.ParentViewType =
        let baseType = this.BaseType
        if baseType.IsAssignableTo typeof<Terminal.Gui.ViewBase.View> then
          baseType
        else
          failwith $"Type {this.FullName} does not have Terminal.Gui.ViewBase.View as parent type."


