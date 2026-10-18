#!/usr/bin/env bash
# Verifies that the solution still makes every domain test green.
# - copies the solution files over src/
# - runs the tests
# - restores src/ to the (unsolved) exercise state, whatever the outcome
#
# Usage: ./verify.sh

set -o pipefail

solutions="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
workshop="$(cd "${solutions}/.." && pwd)"

# Safety: no leftover uncommitted work on the paths we are about to overwrite.
git -C "${workshop}" diff --quiet -- src/ \
  || { echo "ERROR: uncommitted changes under src/ - commit or stash first" >&2; exit 1; }
git -C "${workshop}" checkout -- src/

# Apply the solution.
cp -f "${solutions}/Domain.fs"             "${workshop}/src/ClockOrDie.Core/Domain.fs"
cp -f "${solutions}/Library.fs"           "${workshop}/src/ClockOrDie.Core/Library.fs"
cp -f "${solutions}/ActivityDomainTests.fs" "${workshop}/src/ClockOrDie.Core.Tests/ActivityDomainTests.fs"

# Run.
status=0
(cd "${workshop}" && dotnet test) || status=$?

# Hand back the exercise state.
git -C "${workshop}" checkout -- src/

exit "${status}"