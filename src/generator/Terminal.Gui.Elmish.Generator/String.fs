module Terminal.Gui.Elmish.Generator.String

let escapeReservedKeywords (name: string) =
  match name with
  | "type" | "module" | "open" | "let" | "in" | "do" | "if" | "then" | "else"
  | "match" | "with" | "function" | "val" | "mutable" | "when" | "rec"
  | "begin" | "end" | "for" | "to" | "done" | "while" | "and" | "or"
  | "not" | "true" | "false" | "namespace" | "as" | "assert" | "inherit" -> $"``{name}``"
  | _ -> name


