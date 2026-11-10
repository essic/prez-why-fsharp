# Review — 10-restore-endpoint-parity

Reviewed commits: 22d7222 (item 10 work), d4a6f8c (plan revision 2), ee8e153 (stamps).
This item answers the item 09 review findings; item 09 itself is not re-reviewed.

## Verification rerun

- Item 10 Verify — PASS: `dotnet build --nologo` green (0 warnings, 0 errors);
  exactly two `catch (Exception e)` blocks in `Endpoints.cs`; no `week_number:int` left.
- Diff matches the spec exactly: `GetActivities` now mirrors `CreateActivity`'s try/catch
  (500 `ApiError[]{Technical_Error}` on exception — same shape as the plan's error contract),
  and the week route lost its `:int` constraints while keeping `[FromRoute] int` parameters,
  restoring 400-on-bad-input binding.
- Revision 2 extended item 05's smoke test to assert the 400 at runtime — the parity fix is
  proven by the passing item 05 rerun, not just by the build.
- Stamp `done 22d7222` consistent with plan.md.

## Findings

no findings
