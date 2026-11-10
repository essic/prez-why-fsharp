# 0004 — Minimal API endpoints, record DTOs, structured error bodies

- Status: Accepted
- Date: 2026-11-10
- Workplan subject: `remove-nswag` — remove NSwag, convert the workshop API to Minimal API

## Context

The web layer was MVC controllers plus an NSwag-generated controller interface. The workshop's
teaching focus is the F# core, so the web layer should be as small and direct as possible while
keeping the existing URLs and behaviour.

## Decision

- Replace controllers with Minimal API endpoints in `Endpoints.cs` (`MapClockOrDieEndpoints`).
- Use plain C# records for request/response DTOs in `Dtos.cs`.
- Map F# `Result<_, ApplicationError>` to HTTP in `ApiResults.cs`.
- Keep the project name `ClockOrDie.WebApi`, all existing URLs, and endpoint semantics, including
  the `501` stubs and the `try/catch` → `500` behaviour.
- Error responses are a `{ code, message }` array (`ApiError[]`), with codes `Business_Error`
  and `Technical_Error`.

## Consequences

- The web layer is a thin adapter over the F# shell; no generated code to maintain.
- Business errors return `400` with `ApiError[]`; malformed JSON returns the framework's
  `ProblemDetails` `400`, so the declared and actual body differ for validation failures.
- `501` stubs have empty bodies (status codes unchanged from the old stubs).
- Route parameters are bound with `[FromRoute]`; non-integer week values return `400`.
