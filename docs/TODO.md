# Fix Elmish components

## 1. Fix state
### Issue
State is not kept between View renderings
### Solution
- Have a centralized DOM service that keeps track of all TerminalElements, including their state
### Steps
- Every TerminalElement should have a unique id -> Calculated from its position in the DOM

## 2. Fix rendering
### Issue:
- Components View objects are being created on every render
- IInternalTerminalElement.Reuse is not working, since when we try to reuse a view object another do already exists amd this causes an exception
