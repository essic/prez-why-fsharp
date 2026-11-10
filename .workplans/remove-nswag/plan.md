# Remove NSwag and use built-in OpenAPI + Scalar

Subject: Remove NSwag, convert the workshop API to Minimal API, generate docs from code
Branch: feat/remove-nswag
The executor may commit after each verified item on this branch.

## Decisions

- Use the .NET 10 built-in OpenAPI generator (`Microsoft.AspNetCore.OpenApi`,
  `AddOpenApi()` / `MapOpenApi()`). It reads endpoints and DTOs from the code and emits
  OpenAPI 3.1 at `/openapi/v1.json`. This is the current .NET standard: code is the source
  of truth, so documentation cannot drift.
- Use `Scalar.AspNetCore` (`MapScalarApiReference()`) as the interactive documentation UI at
  `/scalar`. Microsoft's own .NET 10 docs list Scalar as the recommended UI; Swashbuckle is no
  longer the template default.
- Delete the hand-maintained `api-definition/clock-or-die.yaml`, `nswag.json`, and every
  NSwag-generated file. Keeping a second contract is the source of the drift/complexity this
  change removes.
- Convert controllers to Minimal API endpoints in a new `Endpoints.cs`, with plain C# records
  for DTOs. Keep project name `ClockOrDie.WebApi` and all existing URLs and endpoint semantics
  (including the `501` stubs) to avoid unrelated churn. Error responses become a consistent
  `{ code, message }` array, which is easier to document than the old raw string array.
- Keep Paket as the dependency manager; only its `WebApi` group changes.
- `dotnet build` is the build gate. Do not run `dotnet test` as a gate: the exercise state is
  red on purpose.

## Items

1. `01-add-openapi-scalar-packages.md` — [done 51e111f] Add Microsoft.AspNetCore.OpenApi and Scalar.AspNetCore to Paket
2. `02-add-dtos-and-http-mapping.md` — [failed problems/02-add-dtos-and-http-mapping.md] Add API DTO records and F# Result-to-HTTP mapping
3. `03-write-minimal-api-endpoints.md` — [todo] Replace controllers with Minimal API endpoints
4. `04-remove-nswag-artifacts-and-packages.md` — [todo] Delete NSwag/spec artifacts and unused packages
5. `05-smoke-test-openapi-scalar.md` — [todo] Smoke-test `/openapi/v1.json` and `/scalar`
6. `06-update-documentation.md` — [todo] Update READMEs to describe generated OpenAPI + Scalar
7. `07-run-reviewer.md` — [todo] Run the reviewer: stop and ask the user to start the reviewer agent
8. `08-run-documentor.md` — [todo] Run the documentor: stop and ask the user to start the documentor agent
