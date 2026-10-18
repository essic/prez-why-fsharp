---
name: commit-code
description: >
  Use when the user asks to commit code, save changes, or make a commit. Stages the
  relevant files, shows git status, drafts a Conventional Commits message, and
  commits only after the user explicitly agrees to it.
---

# Commit code

Turns a working tree with changes in it into one commit, without ever committing
something the user has not actually seen and approved.

## Steps

1. **Look at what changed.**
   Run `git status` and `git diff` (plus `git diff --staged` if anything is already
   staged). Read the actual changes — do not stage from the file list alone.

2. **Stage the relevant files.**
   Use `git add <path> <path> ...` naming the files that belong to this change.
   Never `git add -A` or `git add .`. Leave out anything unrelated, any scratch or
   temp output, and anything that looks like it could hold a secret — if a file's
   contents are unclear, read it before deciding.

3. **Show the result.**
   Run `git status` again and show it in full, so the user sees exactly what is
   about to be committed before a message is even drafted.

4. **Draft the commit message.**
   Follow [Conventional Commits](https://www.conventionalcommits.org/en/v1.0.0/).
   Pick `type` from what the diff actually does — `feat`, `fix`, `docs`, `style`,
   `refactor`, `perf`, `test`, `build`, `ci`, `chore`, `revert` — never default to
   `chore` because it's easiest.

   ```
   <type>: <short message>

   <explanation, 5 lines max>

   Co-authored using <runtime> with <leader model>
   ```

   - `<short message>`: imperative mood, no trailing period.
   - `<explanation>`: up to 5 lines of plain prose — what changed and why, not a
     restatement of the diff.
   - `<runtime>`: the tool this skill is running under right now — Claude Code,
     opencode, or Docker Agent (cagent).
   - `<leader model>`: the model identity currently answering, as it knows itself
     (e.g. "Claude Sonnet 5", "GLM-4.7-Flash").

5. **Ask for agreement.**
   Show the exact message as it will be committed and ask directly: does this look
   right to commit? If the user disagrees, asks for a change, or does not confirm —
   **stop here**. Revise and ask again only once new direction is given. Never
   commit on an assumed yes.

6. **Commit.**
   Only after explicit agreement. Use a heredoc so multi-line messages keep their
   formatting:

   ```sh
   git commit -m "$(cat <<'EOF'
   <type>: <short message>

   <explanation>

   Co-authored using <runtime> with <leader model>
   EOF
   )"
   ```

   Never add `--no-verify`, `--amend`, or a force flag unless the user explicitly
   asked for it.

## Never

- Stage a file you have not read.
- Show a commit message and commit it in the same breath — the agreement step is
  never optional, however small the change.
- Invent a `type` that does not fit Conventional Commits.
