module Terminal.Gui.Elmish.EventLogTests

open System.Linq
open NUnit.Framework
open Terminal.Gui.Elmish.Elements
open Terminal.Gui.Elmish
open Terminal.Gui.Views

[<SetUp>]
let Setup () =
  ElmishTerminal.unitTestMode <- true
  TerminalElement.clearEventLog()
  ()

[<Test>]
let ``EventLog tracks setProps calls`` () =
  // Arrange
  TerminalElement.clearEventLog()
  
  // Create a simple button element with some properties
  let props = Props()
  props.add(PKey.view.text, "Test Button")
  props.add(PKey.view.enabled, true)
  
  let button = ButtonElement(props)
  let view = new Button()
  
  // Act
  button.setProps(view, props)
  
  // Assert
  let log = TerminalElement.EventLog
  Assert.That(log.ContainsKey("text"), Is.True, "Should log 'text' property being set")
  Assert.That(log.["text"], Is.EqualTo("set"), "Event type should be 'set'")
  Assert.That(log.ContainsKey("enabled"), Is.True, "Should log 'enabled' property being set")
  Assert.That(log.["enabled"], Is.EqualTo("set"), "Event type should be 'set'")

[<Test>]
let ``EventLog tracks removeProps calls`` () =
  // Arrange
  TerminalElement.clearEventLog()
  
  // Create a simple button element with some properties
  let props = Props()
  props.add(PKey.view.text, "Test Button")
  props.add(PKey.view.enabled, true)
  
  let button = ButtonElement(props)
  let view = new Button()
  
  // Act
  button.removeProps(view, props)
  
  // Assert
  let log = TerminalElement.EventLog
  Assert.That(log.ContainsKey("text"), Is.True, "Should log 'text' property being removed")
  Assert.That(log.["text"], Is.EqualTo("removed"), "Event type should be 'removed'")
  Assert.That(log.ContainsKey("enabled"), Is.True, "Should log 'enabled' property being removed")
  Assert.That(log.["enabled"], Is.EqualTo("removed"), "Event type should be 'removed'")

[<Test>]
let ``EventLog can be cleared`` () =
  // Arrange
  let props = Props()
  props.add(PKey.view.text, "Test")
  let button = ButtonElement(props)
  let view = new Button()
  button.setProps(view, props)
  
  Assert.That(TerminalElement.EventLog.Count, Is.GreaterThan(0), "Log should have entries")
  
  // Act
  TerminalElement.clearEventLog()
  
  // Assert
  Assert.That(TerminalElement.EventLog.Count, Is.EqualTo(0), "Log should be empty after clearing")

[<Test>]
let ``EventLog tracks events from different element types`` () =
  // Arrange
  TerminalElement.clearEventLog()
  
  // Create button element
  let buttonProps = Props()
  buttonProps.add(PKey.button.text, "Button")
  let button = ButtonElement(buttonProps)
  let buttonView = new Button()
  
  // Create label element  
  let labelProps = Props()
  labelProps.add(PKey.view.text, "Label")
  let label = LabelElement(labelProps)
  let labelView = new Label()
  
  // Act
  button.setProps(buttonView, buttonProps)
  label.setProps(labelView, labelProps)
  
  // Assert
  let log = TerminalElement.EventLog
  Assert.That(log.ContainsKey("text"), Is.True, "Should log 'text' from both elements")
  Assert.That(log.["text"], Is.EqualTo("set"), "Last event should be 'set'")
