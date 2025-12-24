open Fabulous.AST
open type Fabulous.AST.Ast

let viewTypes =
  typeof<Terminal.Gui.ViewBase.View>.Assembly.GetTypes()
  |> Array.filter _.IsAssignableTo(typeof<Terminal.Gui.ViewBase.View>)

Oak() {
  Namespace("Terminal.Gui.Elmish") {
    Open("System")
    Open("System.Collections.ObjectModel")
    Open("Terminal.Gui.ViewBase")
    Open("Terminal.Gui.Views")

    for viewType in viewTypes do
      TypeDefn(viewType.Name + "Element", Constructor(ParameterPat("props", "Props"))) {
          InheritParen("ViewTerminalGuiElement", ConstantExpr("props"))
          Member("_.name", ConstantExpr($"\"{viewType.Name}\""))
          Member("_.newView()", ConstantExpr("new " + viewType.FullName + "()"))
          Member("_.setProps(terminalElement: IInternalTerminalElement, props: Props) =
      }
      |> _.toInternal()
  }
}
|> Gen.mkOak // Convert to an Oak AST node
|> Gen.run // Generate the F# code string
|> printfn "%s"
