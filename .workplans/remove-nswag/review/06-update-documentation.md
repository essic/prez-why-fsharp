# Review — 06-update-documentation

Reviewed commits: eaecf1c (item 06 work), d669bbf (stamps). Items 01/09 not re-reviewed.

## Verification rerun

- Item 06 Verify — **FAILS as written**, on spurious grounds: the grep sweeps
  `workshop/packages/` (only `.git`, `bin`, `obj`, `paket-files` are excluded) and matches
  third-party package metadata, e.g. `packages/webapi/Scalar.AspNetCore/README.md:14`
  ("OpenAPI/Swagger" description), `packages/webapi/Microsoft.AspNetCore.OpenApi/lib/net10.0/Microsoft.AspNetCore.OpenApi.xml:655,667`
  (mentions "Swashbuckle.AspNetCore"), `packages/webapi/Microsoft.OpenApi/lib/netstandard2.0/Microsoft.OpenApi.xml:725`
  (swagger.io URL), `packages/test/Azure.Monitor.OpenTelemetry.Exporter/...xml` (swagger.json
  URLs). All 15 matches are under `packages/`; nothing workshop-controlled matches.
- The documentation goal itself IS met: all nine prescribed edits match the spec exactly
  (root README.md reworded + api-definition bullet removed; workshop/README.md structure list,
  prerequisite bullet, Scalar run line, `### API documentation` section, WebApi file list,
  contract line); `api-definition/` is gone; `test ! -d api-definition` passes; my repo-wide
  diligence found no NSwag leftovers outside `packages/`.

## Findings

- **should fix** — item 06 — `.workplans/remove-nswag/items/06-update-documentation.md:79` —
  the Verify command is unpassable on any restored tree: it lacks `--exclude-dir=packages`, so
  vendor metadata (Scalar's own "OpenAPI/Swagger" blurb, Swashbuckle mentions inside
  Microsoft.AspNetCore.OpenApi's XML docs, swagger.io/swagger.json URLs) trips the negative
  grep forever. The matching files' mtimes (Aug 20 – Nov 9) predate the item 06 run
  (Nov 10 14:57), so the executor's verify cannot have exited 0, yet the item is stamped
  `done eaecf1c` with no `problems/06-...md` — the item's own protocol ("If verify fails:
  write the problem file, set Status: failed, stop") was skipped for a failure that was real
  on this machine, even though spurious in substance. Fix (plan owner): amend the Verify to add
  `--exclude-dir=packages` (and note the false positive); fix (executor habit): a spurious
  verify failure still gets a problems/ file and a plan revision — that is exactly how
  revision 1 handled item 02.
