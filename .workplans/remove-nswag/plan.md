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
2. `02-add-dtos-and-http-mapping.md` — [superseded by 09] Add API DTO records and F# Result-to-HTTP mapping
3. `03-write-minimal-api-endpoints.md` — [superseded by 09] Replace controllers with Minimal API endpoints
4. `04-remove-nswag-artifacts-and-packages.md` — [superseded by 09] Delete NSwag/spec artifacts and unused packages
5. `09-switch-to-minimal-api-and-remove-nswag.md` — [done e76065c] Switch to Minimal API and remove NSwag atomically
6. `10-restore-endpoint-parity.md` — [done 22d7222] Restore GetActivities exception handling and route binding parity
7. `05-smoke-test-openapi-scalar.md` — [done] Smoke-test `/openapi/v1.json` and `/scalar`
8. `06-update-documentation.md` — [done eaecf1c] Update READMEs to describe generated OpenAPI + Scalar
9. `07-run-reviewer.md` — [todo] Run the reviewer: stop and ask the user to start the reviewer agent
10. `08-run-documentor.md` — [todo] Run the documentor: stop and ask the user to start the documentor agent

## Revision 1

Item 02 failed: adding the DTO records in `ClockOrDie.WebApi` shadowed the NSwag-generated
`ClockOrDie.WebApi.Controllers.CodeGen` types, so the old controller stopped implementing its
generated interface (CS0535/CS0738). Items 02–04 cannot each build green while the generated
code still exists; the switch has to be atomic.

Items 02, 03 and 04 are therefore superseded by
`09-switch-to-minimal-api-and-remove-nswag.md`, which adds the DTOs, result mapping, endpoints
and new `Program.cs`, deletes the NSwag artifacts, and cleans the Paket references in one
verified change. Do not execute items 02, 03 or 04. Items 05–08 are unchanged and run after
item 09.

## Revision 2

Review of item 09 found one should-fix: `GetActivities` lost the old controller's `try/catch`,
so an exception from the database adapter would escape instead of returning a 500 `ApiError[]`.
It also noted that the `:int` route constraints turn non-integer week values into 404 instead of
the old 400. `10-restore-endpoint-parity.md` fixes both before the smoke test.

Accepted nits, no action: the 501 stubs now have empty bodies (status codes unchanged);
malformed JSON returns the framework's ProblemDetails 400 while business errors return
`ApiError[]`; commit `e76065c`'s message says "items 02-04" and includes the item 01 review file.


