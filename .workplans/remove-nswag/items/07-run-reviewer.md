Status: done (reviewer finished; findings in review/)
Goal: Run the reviewer: stop and ask the user to start the reviewer agent.
Files: `.workplans/remove-nswag/review/`
Steps:
1. Stop work. Do not edit any repository file.
2. Ask the user to start the reviewer agent for `.workplans/remove-nswag`.
3. Wait for the user's answer.
Verify: test -d .workplans/remove-nswag/review
Done when: the user confirms the reviewer agent has finished and its findings are in `.workplans/remove-nswag/review/`.
If verify fails: record the blocker in `.workplans/remove-nswag/problems/07-run-reviewer.md` and stop.
