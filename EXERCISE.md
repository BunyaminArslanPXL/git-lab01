# Git lab: Mini Shop (± 60 min)

You work in a group of 2 or 3 on **one shared GitHub repository**. The history is not perfect: there is a bug on `main`, a messy feature branch, a branch that is out of date and a bug nobody noticed yet. Your job is to fix all of it, with the techniques from the slides, without losing anyone's work.

You need the .NET 8 SDK or newer. Run the tests with `dotnet run -- test` (from the folder with `MiniShop.csproj`).

## Roles

There are 3 roles: A, B, and C. Each person in your group picks one. If you are only 2, pick A and B. Every task below says who does it.

| Task | Group of 3 | Group of 2 |
|---|---|---|
| 1. Explore | everyone | everyone |
| 2. Revert the bug on `main` | A | A |
| 3. Clean up `feature/search` | B (C helps) | B (A helps) |
| 4. Bring `feature/export` up to date | C | A |
| 5. Hotfix in a worktree | A | B |
| 6. Review the hotfix in a worktree | B | A |
| 7. Reflog rescue + clean up | everyone | everyone |

**Important for B:** from the moment task 3 starts until you are told otherwise, do **not** run `git fetch` or `git pull` (and turn off auto-fetch in your IDE). You'll see why.

---

## 1. Explore

```bash
git clone <repo-url> mini-shop
cd mini-shop
git log --oneline --graph --all
git branch -a
```

Answer together:

1. Which branches exist on the remote?
2. Which commit is tagged `v1.0`?
3. Run the tests on `main`. They fail. Which commit broke them? (Hint: `git log -p src/Cart.cs` or `git show <sha>`.)
4. Has that commit been pushed? What does that mean for how you can undo it?

## 2. Revert the bug on `main` (A)

1. Undo the commit that broke the tests **without rewriting history**.
2. Run the tests: all green? Push.
3. The others: `git pull` on `main` and check that the tests pass for you too.

> Why not `git reset`? Write your answer down.

## 3. Clean up `feature/search` (B, with help from C)

`feature/search` works, but its history is embarrassing: `wip`, `fix typo`, a debug print that was added and removed again.

**C first (A in a group of 2):**

```bash
git switch feature/search
```

In `src/Search.cs`, add an XML doc comment on the line right above `public static class Search`:
`/// <summary>Search helpers for Mini Shop.</summary>`
Commit it and push. Then go on with task 4.

**B (no fetching!):**

1. `git switch feature/search` and look at the history: `git log --oneline main..feature/search`
2. Use an **interactive rebase** on the last six commits (`git rebase -i HEAD~6`) to end up with exactly **two** commits:
   - `Add search function` (the code, with the typo fixed and **no** `DEBUG` line in it)
   - `Add tests for search`

   Tip: you can reorder lines in the rebase editor. Move `remove debug print` up so it sits right after `debug print`.
3. Check: `git log --oneline -p main..HEAD` shows no `DEBUG` anywhere, and the tests pass.
4. Push your rewritten branch with `git push --force-with-lease`.

    What happens? Why?

5. Now you may fetch. Find C's commit (`git log --oneline origin/feature/search`), bring it onto your clean branch with `git cherry-pick <sha>`, test, and push with `--force-with-lease` again.

> What would plain `git push --force` have done in step 4?

## 4. Bring `feature/export` up to date (C in a group of 3, A in a group of 2)

`feature/export` was branched from `v1.0`, long ago.

1. `git switch feature/export`, `git fetch`, then **rebase** it onto `origin/main`.
2. You'll get a conflict in `README.md`. Resolve it so **both** features stay in the list, then `git add README.md` and `git rebase --continue`.
3. Tests green? Push the branch (`--force-with-lease`: you rewrote it).
4. Merge it into `main` as a fast-forward:

   ```bash
   git switch main
   git pull
   git merge --ff-only feature/export
   git push
   ```

   If `--ff-only` refuses: someone pushed to `main` in the meantime. Rebase `feature/export` on `origin/main` again and retry.

## 5. Hotfix in a worktree (A in a group of 3, B in a group of 2)

You start something new: `git switch -c feature/currency` and add a comment `// TODO: support other currencies` at the top of `src/Cart.cs`. **Don't commit it.**

Then a customer complains: a cart of exactly 50 euro still pays shipping, but the README promises *free shipping from 50 euro*. It must be fixed now, and you don't want to touch your unfinished work.

1. Create a worktree with a new branch from the latest `main`:

   ```bash
   git fetch
   git worktree add ../mini-shop-hotfix -b hotfix/free-shipping origin/main
   cd ../mini-shop-hotfix
   ```

2. In `tests/CartTests.cs`, add a `[Test]` that checks `Cart.Shipping(50m)` is `0m`. See it fail, fix the code, see it pass.

   Notice the first `dotnet run -- test` in this folder builds from scratch: `bin/` and `obj/` are not shared between worktrees. Tip: open the worktree folder in its own IDE window.
3. Commit and push: `git push -u origin hotfix/free-shipping`
4. `cd ../mini-shop` and run `git status`. Is your TODO still there? Which branch are you on?
5. Tell your reviewer the branch is ready.

## 6. Review the hotfix in a worktree (B in a group of 3, A in a group of 2)

Review your teammate's branch **without** touching your own working directory.

1. ```bash
   git fetch
   git worktree add ../mini-shop-review hotfix/free-shipping
   cd ../mini-shop-review
   ```

2. Read the change: `git diff origin/main...hotfix/free-shipping`. Run the tests.
3. Happy? Merge it into `main` and push (from your main folder, `cd ../mini-shop`):

   ```bash
   git switch main
   git pull
   git merge hotfix/free-shipping
   git push
   ```

4. Clean up: `git worktree remove ../mini-shop-review`, `git branch -d hotfix/free-shipping`, `git push origin --delete hotfix/free-shipping`.
5. The hotfix author cleans up and tags the release:

   ```bash
   git restore src/Cart.cs               # throw away the TODO
   git switch main && git pull
   git worktree remove ../mini-shop-hotfix
   git branch -d hotfix/free-shipping
   git branch -D feature/currency
   git tag -a v1.1 -m "Free shipping from 50 euro"
   git push origin v1.1
   ```

## 7. Reflog rescue + clean up (everyone, alone)

1. Make a sandbox and "lose" three commits:

   ```bash
   git switch -c sandbox main
   git reset --hard HEAD~3
   git log --oneline -5
   ```

2. Get them back using only `git reflog` and `git reset --hard`.
3. `git switch main` and delete `sandbox`.
4. `git worktree list` should show only one folder. If not, clean up.
5. **C only:** you still have a local `feature/search`. Compare `git log --oneline feature/search` with `git log --oneline origin/feature/search`. Why are they different? Make your local branch match the remote again (`git switch feature/search` + `git reset --hard origin/feature/search`), or simply delete it.
6. **B only:** your reflog still knows the old, messy `feature/search`. Find the `debug print` commit and `git show` it. Nothing is really gone.

## Done?

Everyone runs:

```bash
git switch main && git pull
git fetch --prune
git log --oneline --graph --remotes --tags
```

Compare your graphs: they should be identical. Tests are green on `main`, `v1.1` exists, the hotfix branch is gone.

Discuss as a group:

- Why `revert` on `main` but `rebase` on `feature/search`?
- What protected C's commit in task 3?
- When would you use a worktree instead of `git stash`?
