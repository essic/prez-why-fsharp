# Clock or Die — solution

The finished state of the workshop: these files make **all 10 tests green**.

| File here                                            | Overwrites                                        |
|------------------------------------------------------|---------------------------------------------------|
| `Domain.fs`                                          | `src/ClockOrDie.Core/Domain.fs`                   |
| `Library.fs`                                         | `src/ClockOrDie.Core/Library.fs`                  |
| `ActivityDomainTests.fs`                             | `src/ClockOrDie.Core.Tests/ActivityDomainTests.fs` (only exercise `00` — `removeOddNumbers`) |

## Use

During the session, to reveal the answer (or after a team finishes, to check):

```bash
cd workshop/solutions
./verify.sh
```

`verify.sh` copies the solution over `src/`, runs `dotnet test`, and then
**restores `src/` to the unsolved exercise state** — safe to run mid-session.

To finish and keep the solution (e.g. as a reference clone):

```bash
cp Domain.fs      ../src/ClockOrDie.Core/Domain.fs
cp Library.fs     ../src/ClockOrDie.Core/Library.fs
cp ActivityDomainTests.fs ../src/ClockOrDie.Core.Tests/ActivityDomainTests.fs
git add -A && git commit -m "workshop: complete the domain"
```

## Rules

Keep the solution and the tests in the **same commit**: whenever a test in
`src/ClockOrDie.Core.Tests/ActivityDomainTests.fs` changes, update the matching
file here and run `./verify.sh` to prove the pair still lines up.