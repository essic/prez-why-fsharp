# Review — 05-smoke-test-openapi-scalar

Reviewed commits: 495825e (stamps only; read-only item by design), d4a6f8c (revision 2 added
the 400-parity assertion to this item's command). Items 01/09 not re-reviewed.

## Verification rerun

- Item 05 Verify — PASS at current HEAD: the app boots, `/openapi/v1.json` serves, all three
  grepped route patterns are present, `GET /timesheets/activities` returns `[]`,
  `GET /timesheets/week/foo/2022` returns 400 (revision 2's parity assertion), and
  `/scalar` returns 200 (redirect followed).
- Read-only honoured: commit 495825e touches only `.workplans` files; no code changed.
- Extra diligence: I additionally confirmed all five routes appear in the generated document
  (`/activity-tracking/save`, `/timesheets/week/current`, `/timesheets/week/{week_number}/{week_year}`,
  `/timesheets/activities`, `/admin/activities/{activity_name}`).

## Findings

- **nit** — item 05 — `.workplans/remove-nswag/items/05-smoke-test-openapi-scalar.md:23` —
  "Done when" claims "the OpenAPI document lists all five routes", but the command greps only
  three patterns; the two week routes are never asserted to be in the document (the 400 check
  does prove the parameterised week route is mapped, but `/timesheets/week/current` is
  untested end-to-end). The claim happens to be true — I verified all five are listed — but the
  gate would pass without them. Fix: add `grep -q '/timesheets/week/current'` and
  `grep -q '/timesheets/week/{week_number}/{week_year}'` to the command (plan-owner edit, low priority).
