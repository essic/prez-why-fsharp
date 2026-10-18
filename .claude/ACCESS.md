<!-- agents 2026.09.565504a-dirty -->

# Access on claude-code

Claude Code applies `permissions.deny` to the whole project, not to one agent.
The leader owns every git write, so applying these rules globally would block
the role that needs them. They are therefore **not** applied by the build.

`settings.deny.json.example` holds the union of them. Merge it into the
project's `.claude/settings.json` only if you want no agent, including the
leader, to touch git — and then commit yourself.

Until then these are prompt-level rules. Each role's own "Never" section states
them, and the reviewer checks the diff against them.

What each role was configured to be denied:

- **developer**: `Bash(git commit:*)`, `Bash(git push:*)`, `Bash(git rebase:*)`, `Bash(git reset:*)`, `Bash(git checkout:*)`, `Bash(git merge:*)`, `Bash(git tag:*)`
- **reviewer**: `Bash(git commit:*)`, `Bash(git push:*)`, `Bash(git reset:*)`, `Bash(git checkout:*)`
- **tester**: `Bash(git commit:*)`, `Bash(git push:*)`, `Bash(git reset:*)`, `Bash(git checkout:*)`
