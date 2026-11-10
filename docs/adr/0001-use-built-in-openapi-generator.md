# 0001 — Generate the OpenAPI document from the code

- Status: Accepted
- Date: 2026-11-10
- Workplan subject: `remove-nswag` — remove NSwag, convert the workshop API to Minimal API

## Context

The workshop API shipped a hand-maintained OpenAPI file (`workshop/api-definition/clock-or-die.yaml`)
and used NSwag to generate controllers and DTOs from it. The contract and the code could drift,
and changing an endpoint meant editing the YAML, regenerating, and fixing the generated code.

.NET 10 ships `Microsoft.AspNetCore.OpenApi`, which reads endpoints and DTOs from the running
application and emits an OpenAPI 3.1 document.

## Decision

Use the built-in generator: `AddOpenApi()` in `Program.cs` and `MapOpenApi()` in development.
The document is served at `/openapi/v1.json`. The code is the single source of truth.

## Consequences

- Documentation cannot drift from the implementation; there is no generation step to run.
- The document only exists at runtime (development environment), not as a checked-in file.
- Anything that consumed the old YAML file must read `/openapi/v1.json` instead.
- The API surface is described with endpoint metadata (`WithSummary`, `Produces<T>`, `Accepts<T>`)
  rather than a separate schema.
