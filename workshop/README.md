# F# Workshop

## Project structure

1. __src__ folder contains the code related to the project
1. __solutions__ folder contains the finished state of the exercise (one file per exercise file), and __verify.sh__ which proves it still makes everything green against the current tests. Do not copy these files into src/ yourself: use __verify.sh__ for live runs — it copies, tests, and restores.

### Pre-requisites

This project uses :
- [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0)
- _F# 10_, which is included in the SDK, however, you can check this [website](https://learn.microsoft.com/en-us/dotnet/fsharp/whats-new/fsharp-10) for more information.
- _C# 14_, also included in the SDK.

### For those who wish to setup the workshop

From the current folder, run the following:
1. `dotnet tool restore` to restore [paket](https://fsprojects.github.io/Paket/), the tool used to handle project dependencies.
1. `dotnet paket install` to resolve and install all project dependencies.
1. `dotnet build` to build the solution.

To run the tests:
```bash
dotnet test
```

Tests use [NUnit](https://nunit.org/) as the test framework and [FsUnit](https://fsprojects.github.io/FsUnit/) for assertions. The [NUnit3TestAdapter](https://github.com/nunit/nunit3-vs-adapter) is the adapter that lets the VSTest runner, supplied by `Microsoft.NET.Test.Sdk`, discover and run them.

__NOTE.__ As checked out, the tests __fail on purpose__ — implementing them is the exercise (see below). A green run here is a sign that the solution was already copied over `src/`: restore with `git checkout -- src/` (or re-clone).

To run the API (Scalar UI at `https://localhost:7036/scalar`, OpenAPI document at `https://localhost:7036/openapi/v1.json`):

```bash
dotnet run --project src/ClockOrDie.WebApi
```

### Continuous integration

[__../.github/workflows/solution.yml__](../.github/workflows/solution.yml) runs on pushes
to `main` and on pull requests. It runs __solutions/verify.sh__ (solution must turn every
test green) and then rebuilds the unsolved exercise state, where failing tests are expected.

### API documentation

The API is a Minimal API. Its OpenAPI document is generated from the endpoints and DTOs at
runtime by [Microsoft.AspNetCore.OpenApi](https://learn.microsoft.com/aspnet/core/fundamentals/openapi/overview),
so the code and the documentation cannot drift. In development the document is served at
`/openapi/v1.json` and the interactive [Scalar](https://scalar.com/) UI at `/scalar`.

To change the API, edit the endpoints in `src/ClockOrDie.WebApi/Endpoints.cs` and the DTOs in
`src/ClockOrDie.WebApi/Dtos.cs`; the documentation follows automatically.

## Let's code in F# !

In this workshop, we will code a very simple __Time Tracker__ API, in order to demonstrate F# in a more realistic/practical context.

In order to focus on the F# language and demonstrate its compatibility with C#, we will also use C#.

This project implements the __[Functional Core/Imperative Shell architecture](https://www.kennethlange.com/functional-core-imperative-shell/)__ .

In short it allows to separate _business constraints_ from _technical ones_ by getting rid of _side effects_ within the _business code_ 

The project structure is as follow:
1. __src/ClockOrDie.WebApi__ hosts the web api code, entirely in C#
   - __Program.cs__ wires the DI container and OpenAPI/Scalar.
   - __Endpoints.cs__ contains the Minimal API endpoints, which call the _F# adapters_.
   - __Dtos.cs__ contains the request/response records shown in the OpenAPI document.
   - __ApiResults.cs__ maps F# `Result` values to HTTP responses.
   - __FakeDb.cs__ is the in-memory `IHandleDatabaseOperations` adapter used by the workshop.
2. __src/ClockOrDie.Core__ hosts the business code, entirely in F#
   - __Domain.fs__ contains the code which implements the business logic. As required in the architecture we chose, this code is 100% synchronous and free of side effects. Therefore it is completely free from concerns of the outside world.
   - __Effects.fs__ infrastructure code needed to communicate with the outside world with other technical concerns.
   - __Shell.fs__ code that will be used by the outside world to realise our use cases. More on this during the workshop.
   - __Library.fs__ contains the `Say.hello` warm-up and the `notYetImplementedFailure` helper used by the stubs.
3. __src/ClockOrDie.Core.Tests__ hosts a test project, entirely in F#, using NUnit (with the NUnit3TestAdapter) & FsUnit (for assertions)
   - __ActivityDomainTests.fs__ is a set of 10 failing unit tests, numbered `00` to `09`. The workshop consists of making those tests pass.

### API surface

The contract is generated from the code by `Microsoft.AspNetCore.OpenApi`; browse it at `/scalar` or read it at `/openapi/v1.json`.

| Method | Route | Operation |
|--------|-------|-----------|
| `POST` | `/activity-tracking/save` | Save a day's time-sheet cells |
| `GET` | `/timesheets/week/current` | Current week's time sheet |
| `GET` | `/timesheets/week/{week_number}/{week_year}` | Time sheet for a given week |
| `GET` | `/timesheets/activities` | Activities of the connected user |
| `POST` | `/admin/activities/{activity_name}` | Create or update an activity |

At the start of the workshop, `GET /timesheets/activities` and `POST /admin/activities/{activity_name}`
are already wired to `FakeDb` and the F# `Shell.saveActivity`; the other endpoints return
`501 Not Implemented`. The exercises themselves target the F# core, not the web layer.

### Solutions

The finished state lives in __solutions/__ (one file per exercise file). During the session, run:

```bash
cd workshop/solutions
./verify.sh
```

It copies the solution over `src/`, runs the full test suite, then restores `src/` to the exercise state — safe to run at any point, and the guarantee that the solution and the tests still agree. The finished files double as the facilitator's reference: to keep the completed code, copy them over and commit, as described in __solutions/README.md__.
