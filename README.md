# Why F# ?

Short presentation on F# benefits followed by a hands-on.
Target audience: mid-level to senior developers.

The aim is not to convert people but to give an overview of what F# offers and/or to get some practice with it in a "real-world" like setting.

This talk should last 20 to 35 minutes max.
The hands-on should be at least 45 minutes, and more if people are interested.

## Repository layout

| Path | Contents |
|------|----------|
| `2021/`, `2024/`, `2026/` | Past versions of the presentation (Pitchme deck in `2021/`, slides in `2024/` and `2026/`) |
| `workshop/` | The hands-on (["Clock or Die"](workshop/README.md)) — a small time-tracker API built as functional core (F#) / imperative shell (C#) |

The presentation material for a given year belongs in a `YYYY/` folder.

## The workshop

**Clock or Die** is a small time-tracker that shows F# and C# in the same solution:
business rules and failing unit tests in F#, HTTP and persistence in C#, with an OpenAPI
document generated from the code.

Prerequisites, setup, run and test commands, the API surface, and the reference-solution
workflow all live in [`workshop/README.md`](workshop/README.md).

## Where to go next

- [`workshop/README.md`](workshop/README.md) — setup, run, test, project structure and API documentation
- [`workshop/solutions/README.md`](workshop/solutions/README.md) — reference solution workflow

## Great follow up resources !

[F# for fun and profit](https://fsharpforfunandprofit.com/)

[F# 101](https://www.youtube.com/playlist?list=PLdo4fOcmZ0oUFghYOp89baYFBTGxUkC7Z)

[Learn some full stack web development with __SAFE__](https://github.com/CompositionalIT/SAFE-Dojo/)

[Functional Core, Imperative Shell](https://www.kennethlange.com/functional-core-imperative-shell/)
