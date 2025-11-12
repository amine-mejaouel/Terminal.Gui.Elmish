module TextFields

open System
open Terminal.Gui
open Terminal.Gui.Elmish

type Model = {
  Text: string
  SecretText: string
  Number: int
  CurrentTime: TimeSpan
  CurrentDate: DateTime
}

type Msg =
  | ChangeText of string
  | ChangeNumber of int
  | ChangeSecretText of string
  | ChangeTime of TimeSpan
  | ChangeDate of DateTime


let init () : Model * Cmd<Msg> =
  let model = {
    Text = "some Text!"
    SecretText = "Secret"
    CurrentTime = TimeSpan(9, 1, 35)
    CurrentDate = DateTime.Today
    Number = 0
  }

  model, Cmd.none


let update (msg: Msg) (model: Model) =
  match msg with
  | ChangeText txt -> { model with Text = txt }, Cmd.none
  | ChangeNumber num -> { model with Number = num }, Cmd.none
  | ChangeSecretText txt -> { model with SecretText = txt }, Cmd.none
  | ChangeTime time -> { model with CurrentTime = time }, Cmd.none
  | ChangeDate date -> { model with CurrentDate = date }, Cmd.none


let view (model: Model) (dispatch: Msg -> unit) = [
  View.label [
    prop.position.x.center
    prop.position.y.absolute 1
    prop.width.fill 1
    prop.alignment.center
    prop.color (Color.BrightYellow, Color.Green)
    labelProps.text "Some Text Fields..."
  ]

  View.label [
    prop.position.x.absolute 1
    prop.position.y.absolute 3
    prop.width.fill 14
    labelProps.text "Text:"
  ]

  View.textField [
    prop.position.x.absolute 14
    prop.position.y.absolute 3
    prop.width.fill 0
    textFieldProps.text model.Text
    textFieldProps.textChanging (fun ev -> dispatch (ChangeText ev.NewValue))
  ]

  View.label [
    prop.position.x.absolute 1
    prop.position.y.absolute 5
    prop.width.fill 14
    labelProps.text "Secret Text:"
  ]

  View.textField [
    prop.position.x.absolute 14
    prop.position.y.absolute 5
    prop.width.fill 0
    textFieldProps.text model.SecretText
    textFieldProps.textChanging (fun ev -> dispatch (ChangeSecretText ev.NewValue))
    textFieldProps.secret true
  ]


  View.numericUpDown<int> [
    prop.position.x.absolute 1
    prop.position.y.absolute 7
    prop.width.fill 14
    numericUpDownProps<_>.value model.Number
    numericUpDownProps<_>.valueChanged (fun num -> dispatch <| ChangeNumber num)
  ]


  View.numericUpDown [
    prop.position.x.absolute 10
    prop.position.y.absolute 7
    prop.width.fill 14
    numericUpDownProps<_>.value model.Number
    numericUpDownProps<_>.valueChanged (fun num -> dispatch <| ChangeNumber num)
  ]

  View.label [
    prop.position.x.absolute 1
    prop.position.y.absolute 9
    labelProps.text $"The Text says: {model.Text}"
  ]

  View.label [
    prop.position.x.absolute 1
    prop.position.y.absolute 11
    labelProps.text $"The Secret Text says: {model.SecretText}"
  ]


  View.label [
    prop.position.x.absolute 1
    prop.position.y.absolute 13
    prop.width.fill 14
    labelProps.text "Time Field:"
  ]

  View.timeField [
    prop.position.x.absolute 16
    prop.position.y.absolute 13
    timeFieldProps.time model.CurrentTime
    timeFieldProps.timeChanged (fun ev -> dispatch <| ChangeTime ev.NewValue)
  ]

  View.timeField [
    prop.position.x.absolute 30
    prop.position.y.absolute 13
    timeFieldProps.isShortFormat true
    timeFieldProps.time model.CurrentTime
    timeFieldProps.timeChanged (fun ev -> dispatch <| ChangeTime ev.NewValue)
  ]

  View.label [
    prop.position.x.absolute 1
    prop.position.y.absolute 15
    prop.width.fill 14
    labelProps.text "Date Field:"
  ]

  View.dateField [
    prop.position.x.absolute 16
    prop.position.y.absolute 15
    dateFieldProps.date model.CurrentDate
    dateFieldProps.dateChanged (fun ev -> dispatch <| ChangeDate ev.NewValue)
  ]

  View.dateField [
    prop.position.x.absolute 30
    prop.position.y.absolute 15
    dateFieldProps.date model.CurrentDate
    dateFieldProps.dateChanged (fun ev -> dispatch <| ChangeDate ev.NewValue)
  ]


  View.label [
    prop.position.x.absolute 1
    prop.position.y.absolute 17
    labelProps.text $"The DateTime (time and date Field) says: {model.CurrentDate.Add(model.CurrentTime):``ddd, yyyy-MM-dd HH:mm:ss``}"
  ]

  View.label (1, 19, $"The Number is: {model.Number}")

]
