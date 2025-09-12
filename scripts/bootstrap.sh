#!/usr/bin/env bash
set -euo pipefail

echo "==> Check Node version"
if command -v nvm >/dev/null 2>&1 && [ -f ".nvmrc" ]; then
  nvm use
fi
node -v
npm -v

if [[ "${SKIP_FRONTEND:-0}" != "1" ]]; then
  echo "==> Install frontend deps"
  pushd client >/dev/null
  if [[ -f package-lock.json ]]; then
    npm ci
  else
    npm install
  fi
  popd >/dev/null
fi

if [[ "${SKIP_BACKEND:-0}" != "1" ]]; then
  echo "==> Restore backend deps"
  pushd server >/dev/null
  dotnet --info | head -n 5
  dotnet restore
  popd >/dev/null
fi

echo "All set ✅"