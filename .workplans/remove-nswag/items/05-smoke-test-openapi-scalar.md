Status: todo
Goal: Prove that the running app serves the generated OpenAPI document and the Scalar UI, and that the ported endpoints are routed.
Files: none (read-only smoke test); log output goes to `/tmp/clockordie.log` and `/tmp/clockordie-openapi.json`
Steps:
1. Run this exact command from the repository root:
   ```bash
   cd workshop
   ASPNETCORE_ENVIRONMENT=Development dotnet run --project src/ClockOrDie.WebApi --no-launch-profile --no-build --urls http://127.0.0.1:5199 > /tmp/clockordie.log 2>&1 &
   pid=$!
   trap 'kill $pid 2>/dev/null || true' EXIT
   for i in $(seq 1 60); do
     if curl -fsS http://127.0.0.1:5199/openapi/v1.json -o /tmp/clockordie-openapi.json; then break; fi
     sleep 1
   done
   grep -q '/timesheets/activities' /tmp/clockordie-openapi.json
   grep -q '/activity-tracking/save' /tmp/clockordie-openapi.json
   grep -q '/admin/activities/{activity_name}' /tmp/clockordie-openapi.json
   curl -fsS http://127.0.0.1:5199/timesheets/activities | grep -q '^\[\]$'
   curl -fsS -L -o /dev/null -w '%{http_code}' http://127.0.0.1:5199/scalar | grep -q '^200$'
   ```
2. If the command fails, read `/tmp/clockordie.log` and record the relevant error.
Verify: cd workshop && ASPNETCORE_ENVIRONMENT=Development dotnet run --project src/ClockOrDie.WebApi --no-launch-profile --no-build --urls http://127.0.0.1:5199 > /tmp/clockordie.log 2>&1 & pid=$!; trap 'kill $pid 2>/dev/null || true' EXIT; for i in $(seq 1 60); do curl -fsS http://127.0.0.1:5199/openapi/v1.json -o /tmp/clockordie-openapi.json && break; sleep 1; done; grep -q '/timesheets/activities' /tmp/clockordie-openapi.json && grep -q '/activity-tracking/save' /tmp/clockordie-openapi.json && grep -q '/admin/activities/{activity_name}' /tmp/clockordie-openapi.json && curl -fsS http://127.0.0.1:5199/timesheets/activities | grep -q '^\[\]$' && curl -fsS -L -o /dev/null -w '%{http_code}' http://127.0.0.1:5199/scalar | grep -q '^200$'
Done when: the command exits 0, meaning the OpenAPI document lists all five routes, `GET /timesheets/activities` returns `[]`, and `/scalar` returns HTTP 200.
If verify fails: write `.workplans/remove-nswag/problems/05-smoke-test-openapi-scalar.md` with the failing command and the tail of `/tmp/clockordie.log`, set this item's `Status:` to `failed problems/05-smoke-test-openapi-scalar.md`, then stop.
