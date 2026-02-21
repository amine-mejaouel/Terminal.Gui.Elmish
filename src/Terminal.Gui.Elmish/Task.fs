module Terminal.Gui.Elmish.Task

let inline wait<'a> (task: System.Threading.Tasks.Task<'a>) =
  task.GetAwaiter().GetResult()
