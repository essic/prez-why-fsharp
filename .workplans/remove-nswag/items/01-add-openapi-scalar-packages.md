Status: done 51e111f
Goal: Make the .NET 10 built-in OpenAPI generator and the Scalar UI available to the WebApi project without removing anything yet.
Files: `workshop/paket.dependencies`, `workshop/src/ClockOrDie.WebApi/paket.references`, `workshop/paket.lock`
Steps:
1. Open `workshop/paket.dependencies`. In `group WebApi`, keep all existing lines and add these two lines after the `framework:` line:
   ```
   nuget Microsoft.AspNetCore.OpenApi ~> 10.0
   nuget Scalar.AspNetCore
   ```
2. Open `workshop/src/ClockOrDie.WebApi/paket.references`. Under `group WebApi`, add:
   ```
   Microsoft.AspNetCore.OpenApi
   Scalar.AspNetCore
   ```
3. Run:
   ```bash
   cd workshop
   dotnet tool restore
   dotnet paket install
   ```
4. Run `dotnet build --nologo` from `workshop`.
Verify: cd workshop && dotnet build --nologo
Done when: the build succeeds, and `grep -n "Microsoft.AspNetCore.OpenApi\|Scalar.AspNetCore" workshop/paket.lock` finds both packages. Existing code is untouched and still compiles.
If verify fails: write `.workplans/remove-nswag/problems/01-add-openapi-scalar-packages.md` with the failing command and full error output, set this item's `Status:` to `failed problems/01-add-openapi-scalar-packages.md`, then stop.
