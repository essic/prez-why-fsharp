Status: todo
Goal: Delete every NSwag/spec-first artifact and the packages that are no longer used, and point the launch profile at Scalar.
Files: `workshop/nswag.json`, `workshop/api-definition/`, `workshop/TODO/minimal-api-plan.md`, `workshop/src/ClockOrDie.WebApi/Controllers/`, `workshop/src/ClockOrDie.WebApi/Globals.cs`, `workshop/src/ClockOrDie.WebApi/AppErrorExt.cs`, `workshop/paket.dependencies`, `workshop/src/ClockOrDie.WebApi/paket.references`, `workshop/src/ClockOrDie.WebApi/Properties/launchSettings.json`
Steps:
1. Delete the obsolete files and folders:
   ```bash
   cd workshop
   rm -rf src/ClockOrDie.WebApi/Controllers
   rm -f src/ClockOrDie.WebApi/Globals.cs
   rm -f src/ClockOrDie.WebApi/AppErrorExt.cs
   rm -f nswag.json
   rm -rf api-definition
   rm -f TODO/minimal-api-plan.md
   rmdir TODO 2>/dev/null || true
   ```
2. In `workshop/paket.dependencies`, make `group WebApi` look exactly like this (remove `Swashbuckle.AspNetCore`, `Microsoft.OpenApi` and `Newtonsoft.Json`):
   ```
   group WebApi
   source https://api.nuget.org/v3/index.json
   framework: net10.0, netstandard2.1
   nuget Microsoft.AspNetCore.OpenApi ~> 10.0
   nuget Scalar.AspNetCore
   ```
3. In `workshop/src/ClockOrDie.WebApi/paket.references`, make the file exactly:
   ```
   FSharp.Core

   group WebApi
   Microsoft.AspNetCore.OpenApi
   Scalar.AspNetCore
   ```
4. In `workshop/src/ClockOrDie.WebApi/Properties/launchSettings.json`, replace every `"launchUrl": "swagger"` with `"launchUrl": "scalar"` (two occurrences).
5. Run:
   ```bash
   cd workshop
   dotnet paket install
   ```
6. Run `dotnet build --nologo`.
Verify: cd workshop && dotnet build --nologo && ! grep -RniIE 'nswag|swashbuckle|api-definition|clock-or-die\.yaml|CodeGenClockOrDie' --exclude-dir=bin --exclude-dir=obj --exclude-dir=paket-files src paket.dependencies paket.lock
Done when: the build succeeds and the grep finds no match in `src`, `paket.dependencies` or `paket.lock`.
If verify fails: write `.workplans/remove-nswag/problems/04-remove-nswag-artifacts-and-packages.md` with the failing command and full error output, set this item's `Status:` to `failed problems/04-remove-nswag-artifacts-and-packages.md`, then stop.
