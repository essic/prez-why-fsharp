# 0005 — Keep Paket; only the WebApi group changes

- Status: Accepted
- Date: 2026-11-10
- Workplan subject: `remove-nswag` — remove NSwag, convert the workshop API to Minimal API

## Context

The workshop manages dependencies with Paket (`paket.dependencies`, `paket.lock`,
`paket.references`). Removing NSwag could have been an occasion to switch to NuGet's native
package management, but that is unrelated to the goal and would churn every project.

## Decision

Keep Paket. Only the `WebApi` group changes: drop NSwag/Swashbuckle and add
`Microsoft.AspNetCore.OpenApi` and `Scalar.AspNetCore`.

## Consequences

- The change stays focused; the F# and test projects are untouched.
- Contributors still need `dotnet tool restore` and `dotnet paket install`.
- `paket.lock` is regenerated and must be committed with the dependency change.
