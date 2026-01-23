namespace Terminal.Gui.Elmish.Generator

open System
open System.Reflection

[<AutoOpen>]
module CodeGen =
  /// <summary>
  /// <p>Returns the name of the type without generic arity suffix.</p>
  /// <p>Example: For List&lt;T&gt;, the type name is "List`1", and this function returns "List".</p>
  /// </summary>
  let getTypeNameWithoutArity (t: Type) =
    if t.Name.Contains("`") then t.Name.Substring(0, t.Name.IndexOf("`")) else t.Name

  /// <summary>
  /// <p>Returns the F# type name written in F# syntax.</p>
  /// </summary>
  let rec getFSharpTypeName (t: Type) =
    if t.IsGenericType then
      let baseName = t.Name.Substring(0, t.Name.IndexOf('`'))
      $"{baseName}{genericTypeParamsBlock t}"
    else if t.IsGenericParameter then
      $"'{t.Name}"
    else if t.Name = "Boolean" then
      "bool"
    else if t.Name = "Int32" then
      "int"
    else if t.Name = "String" then
      "string"
    else
      t.Name

  /// <summary>
  /// <p>Returns the generic type parameters of a generic type as a comma-separated string.</p>
  /// <p>Example: For Dictionary&lt;TKey, TValue&gt;, it returns <c>'TKey, 'TValue</c>.</p>
  /// </summary>
  and genericTypeParams (t: Type) =
    String.concat ", " (t.GetGenericArguments() |> Array.map getFSharpTypeName)

  /// <summary>
  /// <p>Returns the generic type parameters of a generic type enclosed in angle brackets.</p>
  /// <p>Example: For Dictionary&lt;TKey, TValue&gt;, it returns <c>&lt;'TKey, 'TValue&gt;</c>. If the type is not generic, it returns an empty string.</p>
  /// </summary>
  and genericTypeParamsBlock (t: Type) =
    genericTypeParams t
    |> fun s -> if s = "" then "" else $"<{s}>"

  /// <summary>
  /// <p>Returns the generic constraints of a generic type as an F# 'when' clause.</p>
  /// <p>Example: For a generic type with a type parameter T constrained to be a reference type, it returns <c> when 'T: not struct</c>. If there are no constraints, it returns an empty string.</p>
  /// </summary>
  let genericConstraints (t: Type) =
    let constraints =
      t.GetGenericArguments()
      |> Array.choose (fun t ->
          let constraints = ResizeArray<string>()
          let attrs = t.GenericParameterAttributes

          if attrs.HasFlag(GenericParameterAttributes.ReferenceTypeConstraint) then
            constraints.Add($"'{t.Name}: not struct")
          if attrs.HasFlag(GenericParameterAttributes.NotNullableValueTypeConstraint) then
            constraints.Add($"'{t.Name}: struct")
          if attrs.HasFlag(GenericParameterAttributes.DefaultConstructorConstraint) then
            constraints.Add($"'{t.Name}: (new: unit -> '{t.Name})")
          let baseTypes = t.GetGenericParameterConstraints()
          for baseType in baseTypes do
            constraints.Add($"'{t.Name}:> {getFSharpTypeName baseType}")

          if constraints.Count > 0 then
            constraints
            |> String.concat " and "
            |> Some
          else
            None
      )
    if constraints.Length > 0 then
      " when " + (constraints |> String.concat " and ")
    else
      ""

  /// <summary>
  /// <p>Returns the generic type parameters and their constraints of a generic type as an F# angle-bracketed block with a 'when' clause.</p>
  /// <p>Example: For a generic type Dictionary&lt;TKey, TValue&gt; where TKey is constrained to be a reference type, it returns <c>&lt;'TKey, 'TValue when 'TKey: not struct&gt;</c>. If the type is not generic, it returns an empty string.</p>
  /// </summary>
  let rec genericTypeParamsWithConstraintsBlock (t: Type) =
    if not t.IsGenericType then
      ""
    else
      let genericParams = genericTypeParams t
      let constraints = genericConstraints t
      $"<{genericParams}{constraints}>"

  /// <summary>
  /// <p>Returns the F# type name of the event handler type for the given event.</p>
  /// <p>For example, for an IEvent&lt;EventHandler&lt;MyEventArgs&gt;&gt; event, it will return "MyEventArgs -> unit".</p>
  /// </summary>
  /// <param name="event"></param>
  let eventHandlerType (event: EventInfo) =
    let handlerType = event.EventHandlerType
    let genericArgs = handlerType.GetGenericArguments()
    if genericArgs.Length = 1 then
      $"{getFSharpTypeName genericArgs[0]} -> unit"
    else if genericArgs.Length = 0 then
      let eventArgs = handlerType.GetMethod("Invoke").GetParameters().[1].ParameterType
      $"{getFSharpTypeName eventArgs} -> unit"
    else
      raise (NotImplementedException())
