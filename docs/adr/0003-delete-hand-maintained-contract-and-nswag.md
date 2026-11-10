# 0003 — Delete the hand-maintained contract and all NSwag artifacts

- Status: Accepted
- Date: 2026-11-10
- Workplan subject: `remove-nswag` — remove NSwag, convert the workshop API to Minimal API

## Context

The repository carried a second, hand-written description of the API: `workshop/api-definition/clock-or-die.yaml`,
`workshop/nswag.json`, and the generated `Controllers/CodeGen/CodeGenClockOrDieController.cs`.
Keeping that contract in sync with the code was the main source of complexity this change removes.
The generated types also shadowed hand-written DTOs, which made an incremental migration impossible
(see the workplan's revision 1).

## Decision

Delete `api-definition/`, `nswag.json`, every NSwag-generated file, and the NSwag/Swashbuckle
packages. Do not keep a checked-in OpenAPI document; the runtime document from ADR 0001 replaces it.

## Consequences

- One contract only: the code.
- The migration had to be atomic — DTOs, endpoints, `Program.cs`, deletions and package cleanup
  landed in a single verified change.
- No offline/static API document exists in the repository; use `/openapi/v1.json` from a running app.
