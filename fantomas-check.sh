#!/usr/bin/env bash
set -euo pipefail

ignore_list=(
  "packages/"
)

should_ignore() {
  local dir="$1"
  for ignored in "${ignore_list[@]}"; do
    if [[ "$dir" == "./${ignored}" || "$dir" == "./${ignored}"* ]]; then
      return 0
    fi
  done
  return 1
}

for dir in ./*/; do
  [[ -d "$dir" ]] || continue
  should_ignore "$dir" && continue

  echo "Running fantomas check in ${dir}"
  dotnet fantomas --check "$dir"
done
