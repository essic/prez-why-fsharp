# Problem — 02-add-dtos-and-http-mapping

Item: `items/02-add-dtos-and-http-mapping.md` (Add API DTO records and F# Result-to-HTTP mapping)
Attempts: 1 (Verify attempt 1 of 5; further attempts pointless — failure is structural)
Outcome: reset to `38238e5`, untracked `Dtos.cs`/`ApiResults.cs` removed, batch stopped.

## Exact error tail (verify: `cd workshop && dotnet build --nologo`)

```
Controllers/ClockOrDieController.cs(9,37): error CS0535: 'ClockOrDieController' does not implement interface member 'ICodeGenClockOrDieController.SaveActivitiesTrackingAsync(IEnumerable<TimeSheetCell>)'
Controllers/ClockOrDieController.cs(9,37): error CS0738: ... 'GetTrackingForCurrentWeekAsync()' cannot implement 'ICodeGenClockOrDieController.GetTrackingForCurrentWeekAsync()' because it does not have the matching return type of 'Task<ActionResult<TimeSheet>>'
Controllers/ClockOrDieController.cs(9,37): error CS0738: ... 'GetTrackingForWeekAsync(int, int)' ... 'Task<ActionResult<TimeSheet>>'
Controllers/ClockOrDieController.cs(9,37): error CS0738: ... 'GetActivitiesForUserAsync()' ... 'Task<ActionResult<ICollection<Activity>>>'
Controllers/ClockOrDieController.cs(9,37): error CS0535: ... 'CreateActivityAsync(string, ActivityDescription)'
Build FAILED. 0 Warning(s) 5 Error(s)
```

## Hypothesis

Name-shadowing collision between the new DTO records and the NSwag-generated DTO classes:

- NSwag generated `partial class TimeSheet/TimeSheetCell/Activity/ActivityDescription` in
  `namespace ClockOrDie.WebApi.Controllers.CodeGen` (`Controllers/CodeGen/CodeGenClockOrDieController.cs`),
  and `ClockOrDieController` (`namespace ClockOrDie.WebApi.Controllers`) implements
  `ICodeGenClockOrDieController`.
- The new records must live in `namespace ClockOrDie.WebApi` — item 03's literal
  `Endpoints.cs` uses the plain names from that namespace (no `using` for a sub-namespace).
- C# simple-name resolution checks enclosing-namespace members before `using` directives, so
  inside `ClockOrDie.WebApi.Controllers` the controller's method signatures now bind to the
  NEW records, while the generated interface binds to its own classes → the signatures no
  longer match (CS0535/CS0738).

## Which plan assumption was wrong

The plan assumes each item builds green independently while "existing code is untouched":
that DTO records (item 02) can coexist with the NSwag artifacts until item 04 deletes them.
False — same-named types in a parent namespace shadow the generated ones for every type in
the `ClockOrDie.WebApi.*` hierarchy, and the fix requires touching the controller (item 03)
and deleting the generated code (item 04). Items 02–04 only compile together at the point
item 04 removes the NSwag artifacts.

## Recovery options (for the plan owner)

1. Merge items 02, 03, 04 into one item ("switch to Minimal API and delete NSwag") verified
   once green — matches reality, keeps one commit.
2. Or keep item 02 but add the generated-file deletion (current item 04 scope) into it, and
   slim item 04 down to package/reference cleanup only.

Fix option chosen by the user: protocol stop; plan restructure pending.

## Resolution

Plan revision 1 adds `items/09-switch-to-minimal-api-and-remove-nswag.md`, which performs the
work of items 02–04 atomically. Item 02 stays `failed` as a record; do not retry it. Items 03
and 04 are marked superseded in `plan.md`.

## Next action

Read `plan.md` (including `## Revision 1`) and execute
`items/09-switch-to-minimal-api-and-remove-nswag.md`. Do not retry items 02–04. After item 09,
continue with items 05, 06, 07 and 08.
