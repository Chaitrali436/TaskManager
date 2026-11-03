Contributing to TaskManager

This project uses GitHub Flow: short-lived branches, pull requests, and a protected main branch that always works.

Workflow
Update your local main:
   git switch main
   git pull
Create a branch for one piece of work:
   git switch -c feature/short-description
Make small, focused commits. Before pushing, run:
   dotnet build
   dotnet test
Push the branch and open a pull request into main.
Wait for the CI check to turn green. Never merge a red pull request.
Merge with Squash and merge, then delete the branch.
Update your local copy:
   git switch main
   git pull
   git branch -d feature/short-description
Branch names

Use lowercase words separated by hyphens, with a prefix that says what kind of work it is.

Prefix	Use for	Example
feature/	New functionality	feature/user-registration
bugfix/	Fixing a bug found during development	bugfix/due-date-validation
hotfix/	Urgent fix for something already released	hotfix/login-lockout
release/	Preparing or maintaining a release	release/1.0
chore/	Maintenance with no behavior change	chore/update-gitignore
Commit messages
Start with a verb in the imperative mood: Add, Fix, Update, Remove, Rename.
Keep the first line under 72 characters.
Say what the change does, not how you feel about it.

Good: Add unit tests for TaskItem status rules Bad: fixed stuff, changes, asdf

Pull requests
One purpose per pull request, as small as practical.
The title follows the commit message style.
Fill in the pull request template.
Read the Files changed tab yourself before asking for a merge.
Merge strategy

This project uses Squash and merge: all commits on a branch become one commit on main.

Style	What main looks like	Good for	Drawback
Merge commit	Every branch commit plus a merge commit	Keeping full branch history	Busy, hard-to-read history
Squash and merge	One clean commit per pull request	Readable history, easy to revert a whole change	Individual branch commits are lost
Rebase and merge	Branch commits replayed in a straight line	Linear history with detail	Rewrites commit IDs, easier to get wrong

Reason for the choice: each pull request is one logical change, so one commit on main makes the history easy to read and easy to revert.

Rules for main
No direct pushes. All changes arrive through a pull request.
The build-and-test check must pass before merging.
Merged branches are deleted automatically.
Never commit
.vs/, bin/, obj/ or any other generated output
Passwords, tokens, API keys, connection strings with credentials, publish profiles
Personal editor settings

Secrets belong in user-secrets locally and in GitHub Secrets for the pipeline.