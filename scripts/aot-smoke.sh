#!/usr/bin/env bash
#
# Native AOT smoke test for Terminal.Gui.Elmish.
#
# Publishes examples/AotSample with PublishAot for the host runtime identifier and runs the
# resulting native binary. The app drives a real Elmish terminal loop: it builds a view tree,
# patches it several times (including a child reorder) and then exits 0.
#
# The script also fails if the publish produces any trim/AOT (IL2xxx/IL3xxx) warning whose
# source location points at a file inside this repository. Warnings originating from
# FSharp.Core are expected and ignored: F# records/unions emit reflection-based
# ToString()/printing that FSharp.Core itself relies on, and the F# toolchain cannot currently
# make those warnings disappear. What matters is that no warning comes from our own code.
#
# Usage:
#   scripts/aot-smoke.sh
# Env:
#   AOT_RID     override the host runtime identifier (default: `dotnet --info` RID)
#   AOT_CONFIG  build configuration (default: Release)

set -euo pipefail

repo_root="$(cd "$(dirname "${BASH_SOURCE[0]}")/.." && pwd)"
cd "$repo_root"

config="${AOT_CONFIG:-Release}"
project="examples/AotSample/AotSample.fsproj"

# Compute a portable (RID-graph) identifier such as linux-x64 / linux-arm64 / osx-arm64 /
# win-x64. `dotnet --info` on Linux distros reports distro-specific RIDs (e.g. fedora.44-x64)
# for which no Native AOT runtime pack exists, so derive it from the host instead.
portable_rid() {
  local os arch
  case "$(uname -s)" in
    Linux) os=linux ;;
    Darwin) os=osx ;;
    MINGW*|MSYS*|CYGWIN*) os=win ;;
    *) os=linux ;;
  esac
  case "$(uname -m)" in
    x86_64|amd64) arch=x64 ;;
    aarch64|arm64) arch=arm64 ;;
    *) arch=x64 ;;
  esac
  echo "$os-$arch"
}

rid="${AOT_RID:-}"
[ -n "$rid" ] || rid="$(portable_rid)"

log="$(mktemp)"
trap 'rm -f "$log"' EXIT

echo "==> dotnet publish $project -c $config -r $rid (PublishAot)"
dotnet publish "$project" -c "$config" -r "$rid" -v minimal | tee "$log"

echo "==> Checking for trim/AOT warnings that point at source in this repository"
# A warning line looks like:
#   /abs/path/File.fs(12,3): warning IL2070: ... [ /abs/project.fsproj ]
# The project path in brackets always contains the repo root, so strip everything from
# ": warning" onwards and match the remaining source location against the repo root.
offending="$(
  grep -aE ': warning IL[0-9]+:' "$log" 2>/dev/null \
    | sed -E 's/: warning IL.*$//' \
    | grep -F "$repo_root" || true
)"
if [ -n "$offending" ]; then
  echo "FAIL: AOT/trim warnings point at repository source:" >&2
  echo "$offending" >&2
  exit 1
fi

publish_dir="$repo_root/examples/AotSample/bin/$config/net10.0/$rid/publish"
exe="$publish_dir/AotSample"
[ -e "$exe" ] || exe="$publish_dir/AotSample.exe"
[ -e "$exe" ] || {
  echo "FAIL: native binary not found under $publish_dir" >&2
  exit 1
}

# Primary check: headless mode exercises the library's DomNode/DomDiffer pipeline without
# needing a terminal, so it works in any CI environment.
echo "==> Running native AOT binary (headless)"
set +e
if command -v timeout >/dev/null 2>&1; then
  timeout 120 "$exe" --headless
else
  "$exe" --headless
fi
status=$?
set -e

if [ "$status" -ne 0 ]; then
  echo "FAIL: native AOT headless smoke exited with status $status" >&2
  exit "$status"
fi

# Secondary check: the interactive Elmish terminal loop needs a controlling TTY. `script`
# provides a pseudo-terminal; when it is unavailable we skip this part (the headless run
# already covered all of the library's runtime code paths).
if command -v script >/dev/null 2>&1; then
  echo "==> Running native AOT binary (interactive, via pseudo-terminal)"
  set +e
  if command -v timeout >/dev/null 2>&1; then
    timeout 120 script -qec "$exe" /dev/null
  else
    script -qec "$exe" /dev/null
  fi
  tui_status=$?
  set -e

  if [ "$tui_status" -ne 0 ]; then
    echo "FAIL: native AOT interactive smoke exited with status $tui_status" >&2
    exit "$tui_status"
  fi
else
  echo "==> Skipping interactive TUI run ('script' not available)"
fi

echo "==> Native AOT smoke test passed"
