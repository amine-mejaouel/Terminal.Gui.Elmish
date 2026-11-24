# Releasing (prerelease)

`Terminal.Gui.Elmish` ships as a **prerelease** NuGet package: the version carries a
`-suffix` (currently `3.0.0-v2-develop.2329-v1-aibox`), and NuGet treats any version with a
hyphen suffix as a prerelease. Consumers install it with `--prerelease`.

Releases are produced by [`.github/workflows/release.yml`](../.github/workflows/release.yml).
The workflow packages the library, pushes it to **GitHub Packages**, and creates the
**GitHub release** (plus the `v<version>` tag when it does not exist).

## What "pre-release" means here

There are two independent knobs:

| Level | Controlled by | Effect |
|---|---|---|
| NuGet package | the version **suffix** (`-aibox`, `-preview`, …) | NuGet treats it as a prerelease; installing requires `dotnet add package … --prerelease` |
| GitHub release | the workflow's `prerelease` input, or inferred from the suffix | shows the **Pre-release** badge and is not offered as "Latest" |

The workflow keeps them consistent: a version with a `-suffix` is published as a NuGet
prerelease **and** gets the GitHub pre-release badge. On manual dispatch you can override the
GitHub badge with the `prerelease` checkbox. For a *stable* release, use a hyphen-free version
(e.g. `3.0.0`): NuGet publishes it as stable and the workflow omits the GitHub pre-release flag.

## Option 1 — trigger from GitHub (no local tagging)

1. **One-time:** make sure `release.yml` is present on the repository's **default branch**
   (merge it there). GitHub only lists a `workflow_dispatch` workflow once it exists on the
   default branch. The run itself can use any branch you pick in the dropdown.
2. Make sure the branch you want to release is pushed and CI is green
   (`build-and-test` and `native-aot`).
3. Open **Actions → Release → Run workflow**, choose the branch, and enter the version
   (e.g. `3.0.0-v2-develop.2329-v1-aibox`). Run it.
4. The workflow creates the `v<version>` tag at the selected commit, publishes the package to
   GitHub Packages, and opens the GitHub pre-release with the `.nupkg` attached.

No local `git tag` needed. (You can also start it from a terminal with
`gh workflow run release.yml -f version=…` if you prefer, but the tag is still created by
GitHub, not locally.)

## Option 2 — push a tag

If a tag is created by other means (external tooling, API), pushing a `v*` tag triggers the
same workflow:

```bash
git tag v3.0.0-v2-develop.2329-v1-aibox
git push origin v3.0.0-v2-develop.2329-v1-aibox
```

## Manual fallback (offline)

```bash
rm -rf artifacts
dotnet pack src/Terminal.Gui.Elmish/Terminal.Gui.Elmish.fsproj -c Release -o artifacts

gh release create v3.0.0-v2-develop.2329-v1-aibox artifacts/*.nupkg \
  --prerelease --title "3.0.0-v2-develop.2329-v1-aibox" --notes-file RELEASE_NOTES.md
```

## Consuming the GitHub Packages feed

The feed is private; consumers need a token with `read:packages`:

```bash
dotnet nuget add source "https://nuget.pkg.github.com/amine-mejaouel/index.json" \
  --name amine-mejaouel --username amine-mejaouel --password "$GH_PAT" \
  --store-password-in-clear-text

dotnet add package Terminal.Gui.Elmish --version 3.0.0-v2-develop.2329-v1-aibox
```

`nuget.org` must remain a configured source for the `Terminal.Gui` / `Elmish` dependencies.

## Repository settings

- **Actions → General → Workflow permissions** must allow the token used by the workflow to
  create releases and publish packages. The workflow already requests the needed scopes via its
  top-level `permissions:` block (`contents: write`, `packages: write`).
- To also publish to nuget.org, add a repository secret **`NUGET_API_KEY`**.

## Pre-release checklist

- [ ] `dotnet build Terminal.Gui.Elmish.sln` is green and the tests pass.
- [ ] Native AOT smoke passes (`scripts/aot-smoke.sh`).
- [ ] `release.yml` is on the default branch (so the "Run workflow" button is available).
- [ ] `PackageProjectUrl` / `RepositoryUrl` in
      `src/Terminal.Gui.Elmish/Terminal.Gui.Elmish.fsproj` point at
      `https://github.com/amine-mejaouel/Terminal.Gui.Elmish`.
- [ ] Version is a valid prerelease and does not collide with an existing tag.
- [ ] `RELEASE_NOTES.md` updated for the release.
