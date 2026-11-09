Status: todo
Goal: Run the documentor: stop and ask the user to start the documentor agent.
Files: `.workplans/remove-nswag/review/`
Steps:
1. Stop work. Do not edit any repository file.
2. Ask the user to start the documentor agent for `.workplans/remove-nswag`.
3. Wait for the user's answer.
Verify: test -d .workplans/remove-nswag/review
Done when: the user confirms the documentor agent has finished.
If verify fails: record the blocker in `.workplans/remove-nswag/problems/08-run-documentor.md` and stop.
