# TODO — Hand-written C# Minimal API for Clock-or-Die

**Context:** Replace NSwag code generation with a hand-written minimal API.
Spec-first stays as design discipline only (`api-definition/clock-or-die.yaml` is the design artifact).
The C# shell is built **from scratch** (`ClockOrDie.App` is empty today).

---

## 1. Project setup

- [ ] Create `ClockOrDie.App` C# project (`.csproj`, .NET 10) and add it to `clock-or-die.sln`
- [ ] Reference `ClockOrDie.Core` (F#) from `ClockOrDie.App`
- [ ] Register minimal API host (`WebApplication.Create`, `app.MapPost`, `app.Run`)

## 2. DTOs — C# records matching the OpenAPI spec

Spec: `POST /activity-tracking/save`, body = `TimeSheetCell[]`

- [ ] `TimeSheetCell` record: `Date` (string), `Activities` (`ActivityCell[]`)
- [ ] `ActivityCell` record: `ActivityId` (`int?`), `ActivityLabel` (string), `TimeTracked` (double)
- [ ] Teaching points: `record` (value equality, immutability, primary constructors),
      nullable reference types, `int?` ↔ F# `int option` mapping

## 3. Route handler — minimal API basics

- [ ] `app.MapPost("/activity-tracking/save", HandleSaveActivities)`
- [ ] Bind body with `[FromBody] TimeSheetCell[] cells`
- [ ] Inject `IHandleDatabaseOperations` (from F# `Effects`) as a method parameter (DI from signature)
- [ ] Return `Task<IResult>` (async handler)
- [ ] Teaching points: model binding, why `[FromBody]` matters for arrays,
      DI via method parameters, no controller boilerplate

## 4. Error handling — mapping F# results to HTTP

- [ ] Map `Result<_, ApplicationError>` from the core to `Results.Ok` / `Results.BadRequest`
- [ ] Use `Results<T>` typed results
- [ ] Optional: C# pattern matching (`switch` expressions) over the F# DU-shaped results
- [ ] Teaching point: imperative shell translates domain outcomes into HTTP semantics

## 5. Date parsing — shell-only concern

- [ ] Parse `Date` string (`dd/MM/yyyy`) with `DateTime.ParseExact` before calling the core
- [ ] Decide behavior on parse failure (400 with clear message)
- [ ] Teaching point: parsing/validation lives in the imperative shell;
      the functional core stays clean

## 6. Integration test

- [ ] Full round-trip test: POST payload → minimal API → F# core → mocked DB effects
- [ ] Cover: valid payload, empty `activities` array, bad date format, missing `activityId`

---

## Teaching sequence (10 exercises)

| # | Exercise | Feature taught |
|---|----------|----------------|
| 1–2 | Write record DTOs from the spec | `record`, immutability, `int?` |
| 3 | Route handler with `[FromBody]` | Minimal API syntax, model binding |
| 4 | Wire DI | Services resolved from method signature |
| 5–6 | Error mapping to HTTP | `IResult`, `Results<T>`, pattern matching |
| 7–8 | Date parsing | Shell handles parsing/validation |
| 9–10 | Integration test | End-to-end: HTTP → core → effects |

---

## Design notes

- Spec stays hand-maintained as the design artifact; when code changes,
  regenerate the spec for a diff/review to catch drift (AI-assisted, not NSwag in the build).
- Generated skeleton code remains gitignored and disposable — no code tax in the workshop flow.
- `date` is a **string** in the spec (`dd/MM/yyyy`); conversion to `DateTime` happens in the shell.
