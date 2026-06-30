#!/usr/bin/env bash
set -euo pipefail

SLIDES_VERSION="${1:-26.6.0}"
RID="${2:-maccatalyst-x64}"
CONFIGURATION="${3:-Release}"

ROOT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")/.." && pwd)"
PROJECT="${ROOT_DIR}/MauiSlidesCrossPlatformCheck/MauiSlidesCrossPlatformCheck.csproj"
RESULTS_DIR="${ROOT_DIR}/artifacts/runtime-${SLIDES_VERSION}-${RID}"

mkdir -p "${RESULTS_DIR}"

echo "== Environment =="
sw_vers | tee "${RESULTS_DIR}/environment.txt"
xcodebuild -version | tee -a "${RESULTS_DIR}/environment.txt"
dotnet --info | tee -a "${RESULTS_DIR}/environment.txt"

echo "== Clean =="
rm -rf "${ROOT_DIR}/MauiSlidesCrossPlatformCheck/bin" "${ROOT_DIR}/MauiSlidesCrossPlatformCheck/obj"

echo "== Restore =="
dotnet restore "${PROJECT}" \
  -p:SlidesPackageVersion="${SLIDES_VERSION}" \
  -p:RuntimeIdentifier="${RID}" \
  2>&1 | tee "${RESULTS_DIR}/restore.log"

echo "== Publish =="
dotnet publish "${PROJECT}" \
  -f net10.0-maccatalyst \
  -c "${CONFIGURATION}" \
  -p:SlidesPackageVersion="${SLIDES_VERSION}" \
  -p:RuntimeIdentifier="${RID}" \
  -p:CreatePackage=false \
  -p:EnableCodeSigning=false \
  --no-restore \
  2>&1 | tee "${RESULTS_DIR}/publish.log"

APP_PATH="$(find "${ROOT_DIR}/MauiSlidesCrossPlatformCheck/bin/${CONFIGURATION}/net10.0-maccatalyst" -name '*.app' -type d | head -n 1)"
if [[ -z "${APP_PATH}" ]]; then
  echo "No .app bundle was produced." | tee "${RESULTS_DIR}/failure.txt"
  exit 1
fi

echo "${APP_PATH}" | tee "${RESULTS_DIR}/app-path.txt"

echo "== Native Aspose libraries in app bundle =="
find "${APP_PATH}" -name '*aspose.slides.drawing.capi*' -print | sort | tee "${RESULTS_DIR}/native-libraries.txt"

echo "== Launch =="
open -W "${APP_PATH}" &
OPEN_PID=$!

echo "The app is open. Wait until the window shows PASS or FAIL, then close the app."
wait "${OPEN_PID}" || true

echo "== Collect result files =="
find "${HOME}/Library/Containers" -path '*com.aspose.slidesnet44710.mauicheck*slidesnet-44710-result.txt' -print -exec cp {} "${RESULTS_DIR}/" \; 2>/dev/null || true
find "${HOME}/Library/Containers" -path '*com.aspose.slidesnet44710.mauicheck*slidesnet-44710-output.pdf' -print -exec cp {} "${RESULTS_DIR}/" \; 2>/dev/null || true

if [[ -f "${RESULTS_DIR}/slidesnet-44710-result.txt" ]]; then
  cat "${RESULTS_DIR}/slidesnet-44710-result.txt"
else
  echo "Result file was not found automatically. Use the Report path shown in the app window." | tee "${RESULTS_DIR}/result-file-not-found.txt"
fi
