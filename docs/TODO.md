# Fix Elmish components

## 0. Elmish components Root properties is not kept between loops
### Work in progress
- Differ.updateState is not propagating the Root property when a component is reused.
    - Simply fixing that could not be the proper solution:
      - Elmish components are meant to be created once, keeping their state alive.
      - The current `Reuse` mechanism imply recreating the component on every render.
      - `Reuse` should be evolved to keep the component alive.
- But first, I need to create a test case to reproduce the issue protecting the current behavior and to validate the fix when implemented.
- But here again, Current Test Elmish components do not support dispatching messages from the unit tests, so I need to implement that first.
- For that I need to use the `MsgDispatcherSubscription` in the `ElmishComponentTE`.
- Problem is I don't want to overload the RELEASE version of the `ElmishComponentTE` with testing code, so I need to create a `TestableElmishComponentTE` that inherits from `ElmishComponentTE` and adds the testing capabilities.

## 1. Fix state
### Issue
State is not kept between View renderings
### Solution
- Have a centralized DOM service that keeps track of all TerminalElements, including their state
### Steps
- Every TerminalElement should have a unique id -> Calculated from its position in the DOM
- TreeDiff.update should rely on the id

## 2. Fix rendering
### Issue:
- Components View objects are being created on every render
- IInternalTerminalElement.Reuse is not working, since when we try to reuse a view object another do already exists amd this causes an exception