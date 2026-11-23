# Releasing (prerelease)

`Terminal.Gui.Elmish` ships as a **prerelease** NuGet package: the version carries a
`-suffix` (currently `3.0.0-v2-develop.2329-v1-aibox`), and NuGet treats any version with a
hyphen suffix as a prerelease. Consumers install it with `--prerelease`.

Releases are produced by [`.github/workflows/release.yml`](../.github/workflows/release.yml),
triggered by pushing a `v*` tag. The tag (without the leading `v`) becomes the package version.

## Preferred: release from CI

1. Make sure the tree is clean and on the commit you want to release, and that CI is green
   (`build-and-test` and `native-aot`).

2. Create and push a tag:

   ```bash
   git tag v3.0.0-v2-develop.2329-v1-aibox
   git push origin v3.0.0-v2-develop.2329-v1-aibox
   ```

3. The workflow then:
   - restores tools/packages and packs with `-p:Version=<tag minus v>`;
   - pushes the `.nupkg` to **GitHub Packages**
     (`https://nuget.pkg.github.com/amine-mejaouel/index.json`);
   - creates a **GitHub pre-release** with the `.nupkg` attached;
   - optionally pushes to **nuget.org** when the `NUGET_API_KEY` secret is set.

## Manual fallback (local)

```bash
rm -rf artifacts
dotnet pack src/Terminal.Gui.Elmish/Terminal.Gui.Elmish.fsproj -c Release -o artifacts

git tag v3.0.0-v2-develop.2329-v1-aibox
git push origin v3.0.0-v2-develop.2329-v1-aibox

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
- [ ] `PackageProjectUrl` / `RepositoryUrl` in
      `src/Terminal.Gui.Elmish/Terminal.Gui.Elmish.fsproj` point at
      `https://github.com/amine-mejaouel/Terminal.Gui.Elmish`.
- [ ] Version/tag is a valid prerelease and does not collide with an existing tag.
- [ ] `RELEASE_NOTES.md` updated for the release.
