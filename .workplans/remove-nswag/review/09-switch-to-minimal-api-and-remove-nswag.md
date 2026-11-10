# Review — 09-switch-to-minimal-api-and-remove-nswag

Reviewed commits: e76065c (item 09 work) + 0ddcf12 (item 02 failure record), 4e38387
(plan revision 1), 925faf2 (status stamps). Item 01 re-reviewed at new HEAD.

## Verification rerun

- Item 09 Verify (`dotnet build --nologo` + negative grep) — PASS: build green
  (0 warnings, 0 errors), no `nswag|swashbuckle|api-definition|clock-or-die.yaml|CodeGenClockOrDie`
  left in `src`, `paket.dependencies`, `paket.lock`.
- Item 01 Verify — PASS at new HEAD: build green; `Microsoft.AspNetCore.OpenApi (10.0.14)`
  and `Scalar.AspNetCore (2.19)` still in `paket.lock` (lines 142, 145).
- All four "with exactly" files match the spec byte-for-byte (`Dtos.cs`, `ApiResults.cs`,
  `Program.cs`; `Endpoints.cs` differs from the markdown only by the block's 3-space indent).
- Deletions complete (`Controllers/` ×3, `Globals.cs`, `AppErrorExt.cs`, `nswag.json`,
  `api-definition/` ×2, `TODO/` gone); `launchSettings.json`: both `swagger` → `scalar`;
  `paket.references`/`paket.dependencies` WebApi groups exact; lock correctly drops Swashbuckle/
  Newtonsoft/transitives while keeping `Microsoft.OpenApi` as the OpenApi package's dependency.
- Atomicity holds: every commit boundary on the branch builds (0ddcf12 and 4e38387 touch only
  `.workplans` files; e76065c is the atomic switch and equals the current green tree).
- Failure protocol followed properly: item 02 stopped after 1 attempt, `problems/02` records
  the exact CS0535/CS0738 errors, hypothesis, wrong plan assumption, and the user-chosen
  recovery; revision 1 supersedes 02–04 coherently.
- Diligence: no dangling `Newtonsoft.Json` usages; terms item 06 will grep for survive only in
  `workshop/README.md` and root `README.md` — exactly the lines item 06's steps rewrite.

## Findings

- **should fix** — item 09 — `workshop/src/ClockOrDie.WebApi/Endpoints.cs:53` —
  `GetActivities` lost the old controller's `try/catch` (old
  `ClockOrDieController.GetActivitiesForUserAsync` returned `500` with the exception message).
  An exception from `db.GetAllActivities()` is now unhandled (developer exception page in
  Development, empty 500 in Production). This contradicts the plan decision "keep all existing
  endpoint semantics" and is asymmetric with `CreateActivity` (`Endpoints.cs:75`), which kept
  its catch. Latent only — current `FakeDb.GetAllActivities` cannot throw — but the executor
  copied the prescribed code exactly, so the flaw is in the plan's spec, not its execution.
  Fix: wrap the `GetActivities` body in the same `try/catch` as `CreateActivity`, or amend the
  plan decision to state that transport-level exception mapping is dropped.

- **nit** — item 09 — `workshop/src/ClockOrDie.WebApi/Endpoints.cs:27` — the new `:int` route
  constraints on `/timesheets/week/{week_number:int}/{week_year:int}` change edge-case
  behaviour: non-integer input now gets 404 (route mismatch) where the old unconstrained route
  + `CustomValidationFilter` returned 400. URLs and happy-path semantics are unchanged.
  Fix: none needed if accepted; otherwise drop `:int` to restore 400s.

- **nit** — item 09 — `workshop/src/ClockOrDie.WebApi/Endpoints.cs:84` (and metadata lines
  like `:24`) — deleting `CustomValidationFilter` means malformed JSON now returns the
  framework's ProblemDetails 400, while `.Produces<ApiError[]>(400)` advertises `ApiError[]`;
  the declared and actual 400 body differ for validation failures (business errors still match).
  Fix: have item 06 mention validation-error shape, or accept as the conventional
  framework-problem behavior (415 for non-JSON is preserved automatically).

- **nit** — item 09 — `workshop/src/ClockOrDie.WebApi/Endpoints.cs:19,29,38` and
  `ApiResults.cs:38` — 501 responses now have empty bodies (old stubs replied
  "Not implemented yet !" / "Unknown error occured"). Status-code semantics kept; only body
  parity changed. No action needed unless body parity matters.

- **nit** — item 09 — commit `e76065c` message — says "items 02-04" but per revision 1 the
  work is item 09; also sweeps in `.workplans/.../review/01-...md`, which is not in the item's
  Files list. Traceability/housekeeping only; the stamps in `plan.md` are correct.

Runtime serving of `/openapi/v1.json` and `/scalar` is intentionally not yet exercised — that
is item 05 (todo). Do not merge before item 05 runs.
