# Reconciler follow-up

The retained virtual terminal tree now preserves Elmish component loops and state across compatible parent renders and keyed moves.

Remaining follow-up work:

- add a BenchmarkDotNet project and record allocation/mutation baselines;
- tune the adjacent-move versus `Remove`/`AddAt` crossover from those measurements;
- add Terminal.Gui-specific adapters only for collection-valued view properties with verified ownership semantics;
- consider an upstream Terminal.Gui batch reorder API to reduce hierarchy invalidation for large shuffles.
