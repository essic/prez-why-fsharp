# 0006 — `dotnet build` is the gate; failing tests are expected

- Status: Accepted
- Date: 2026-11-10
- Workplan subject: `remove-nswag` — remove NSwag, convert the workshop API to Minimal API

## Context

The workshop is shipped in an unsolved state: the F# unit tests fail on purpose, because making
them pass is the exercise. Any change to the web layer therefore cannot use `dotnet test` as a
success gate without either solving the exercise or weakening the tests.

## Decision

Use `dotnet build` as the build gate for changes to the workshop. Do not run `dotnet test` as a
gate. The reference solution is validated separately by `workshop/solutions/verify.sh`, which
copies the solution over `src/`, runs the tests, and restores the exercise state.

## Consequences

- Web-layer changes are verified by a green build plus a runtime smoke test of the API.
- A green `dotnet test` on the checked-out tree means the solution was copied over `src/`;
  restore with `git checkout -- src/`.
- CI still runs `verify.sh` and then rebuilds the unsolved state.
