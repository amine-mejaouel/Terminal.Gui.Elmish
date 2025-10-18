namespace Terminal.Gui.Elmish

open System.Linq.Expressions
open System
open Terminal.Gui.ViewBase

module internal EventHelpers =

    open System.Reflection

    let getEventDelegates (eventName:string) (o:obj) =
        let field = o.GetType().GetField(eventName, BindingFlags.Instance ||| BindingFlags.NonPublic ||| BindingFlags.Instance ||| BindingFlags.FlattenHierarchy)
        let baseField = o.GetType().BaseType.GetField(eventName, BindingFlags.Instance ||| BindingFlags.NonPublic ||| BindingFlags.Instance ||| BindingFlags.FlattenHierarchy)
        let eventDelegate =
            if field |> isNull |> not then
                field.GetValue(o) :?> MulticastDelegate
            elif baseField |> isNull |> not then
                baseField.GetValue(o) :?> MulticastDelegate
            else null
        if (eventDelegate |> isNull) then
            []
        else
            eventDelegate.GetInvocationList() |> Array.toList

    let clearEventDelegates (eventName:string) (o:obj) =
        let eventInfo  = o.GetType().GetEvent(eventName, BindingFlags.Public ||| BindingFlags.NonPublic ||| BindingFlags.Instance ||| BindingFlags.FlattenHierarchy)
        if eventInfo |> isNull then
            ()
        else
            let field = o.GetType().GetField(eventName, BindingFlags.Instance ||| BindingFlags.NonPublic)
            let baseField = o.GetType().BaseType.GetField(eventName, BindingFlags.Instance ||| BindingFlags.NonPublic ||| BindingFlags.Instance ||| BindingFlags.FlattenHierarchy)
            let eventDelegate =
                if field |> isNull |> not then
                    field.GetValue(o) :?> MulticastDelegate
                elif baseField |> isNull |> not then
                    baseField.GetValue(o) :?> MulticastDelegate
                else null
            if (eventDelegate |> isNull) then
                ()
            else

                eventDelegate.GetInvocationList() |> Array.iter (fun d -> eventInfo.RemoveEventHandler(o, d))

    let addEventDelegates (eventName:string) (delegates:Delegate list) (o:obj) =
        let eventInfo  = o.GetType().GetEvent(eventName, BindingFlags.Public ||| BindingFlags.NonPublic ||| BindingFlags.Instance ||| BindingFlags.FlattenHierarchy)
        if (eventInfo |> isNull) then
            ()
        else
            delegates |> List.iter (fun d -> eventInfo.AddEventHandler(o, d))

[<RequireQualifiedAccess>]
module Interop =
    // TODO: move maybe to Props ?
    let filterProps (oldProps: Props) (newProps: Props) =

        let remainingOldProps, removedProps =
            oldProps |> Props.partition (fun kv -> newProps |> Props.rawKeyExists kv.Key)

        let changedProps =
            newProps |> Props.filter (fun kv ->
                match remainingOldProps |> Props.tryFindByRawKey kv.Key with
                | _ when kv.Value = "children" ->
                    false
                | Some v' when kv.Value = v' ->
                    false
                | _ ->
                    true
            )

        (changedProps, removedProps)

    let inline csharpList (list:'a list) = System.Linq.Enumerable.ToList list

    let rec getParent (view:View) =
        view.SuperView
        |> Option.ofObj
        |> Option.bind (fun p ->
            if p.GetType().Name.Contains("Content") then
                getParent p
            else
                Some p
        )

    open Microsoft.FSharp.Quotations
    open Microsoft.FSharp.Quotations.Patterns
    open FSharp.Linq.RuntimeHelpers

    /// Get the property name from an event expression
    /// i don'T like it, it's unnecessary complicated to get the name as sting from this particualr expression
    let private getPropNameFromEvent (eventExpr: Expr<IEvent<'a,'b>>) =
        match eventExpr with
        | Call (a, mi, expr) ->
            if mi.Name = "CreateEvent" then
                if expr.Length < 1 then
                    None
                else
                    match expr[0] with
                    | Lambda (a, expr) ->
                        match expr with
                        | Call (a, mi, call) ->
                            Some (mi.Name.Replace("add_", "").Replace("remove_", ""))
                        | _ -> None
                    | _ -> None
            else
                None
        | _ -> None

    /// Set an event handler for an event on an element and remove the previous handler
    let setEventHandler (eventExpr: Expr<IEvent<'a,'b>>) (eventHandler:'b->unit) element  =
        let evName = getPropNameFromEvent eventExpr
        match evName with
        | None -> raise (ArgumentException "can not get property name from event expression")
        | Some evName ->
            let eventDel = EventHelpers.getEventDelegates evName element
            if (eventDel.Length > 0) then
                EventHelpers.clearEventDelegates evName element
            let event = unbox<IEvent<'a,'b>>(LeafExpressionConverter.EvaluateQuotation eventExpr)
            event.Add(eventHandler)

    let removeEventHandler (eventExpr: Expr<IEvent<'a,'b>>) element =
        let evName = getPropNameFromEvent eventExpr
        match evName with
        | None -> raise (ArgumentException "can not get property name from event expression")
        | Some evName ->
            let eventDel = EventHelpers.getEventDelegates evName element
            if (eventDel.Length > 0) then
                EventHelpers.clearEventDelegates evName element

    /// Set an event handler for an event on an element and remove the previous handler
    let setEventHandlerExpr (eventExpr: Expression<Func<_,IEvent<'a,'b>>>) eventHandler element =
        let evName =
            match eventExpr.Body with
            | :? MethodCallExpression as me ->
                let property = (me.Object :?> MemberExpression)
                property.Member.Name
            | _ -> raise (ArgumentException("Invalid event expression"))

        let eventDel = EventHelpers.getEventDelegates evName element
        if (eventDel.Length > 0) then
            EventHelpers.clearEventDelegates evName element

        let event = eventExpr.Compile()
        let e = event.Invoke() // :?> IEvent<'a,'b>
        e.Add(eventHandler)
