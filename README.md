# SLIDESNET-44710 MAUI Mac Catalyst check

This folder contains a minimal MAUI Mac Catalyst repro for investigating whether `Aspose.Slides.NET6.CrossPlatform` is packaged correctly for MAUI.

The repro targets .NET 10 MAUI because current GitHub-hosted macOS runners reject older Mac Catalyst TFMs under the current workload policy.

The GitHub Actions workflow `.github/workflows/slidesnet-44710-maui-maccatalyst.yml` restores the same MAUI app twice and inspects the restored Aspose.Slides package:

- `Aspose.Slides.NET6.CrossPlatform` `24.7.0`
- `Aspose.Slides.NET6.CrossPlatform` `26.6.0`

The workflow checks whether the package contains `libaspose.slides.drawing.capi_appleclang*.dylib` and whether the package targets gate native library copy by exact `TargetFramework` values.

Expected investigation result:

- `24.7.0` should report a warning because its `buildTransitive` targets only include native libraries for exact `net6.0`, `net7.0`, and `net8.0` TFMs. `net10.0-maccatalyst` does not match that condition.
- `26.6.0` should include the native libraries because the exact-TFM condition has been removed from the package targets. The workflow fails if the native library is still missing for this version.

For the final runtime check on a rented Mac, open this project and run:

```bash
dotnet workload install maui
chmod +x scripts/run-maccatalyst-runtime-check.sh
scripts/run-maccatalyst-runtime-check.sh 24.7.0 maccatalyst-x64
scripts/run-maccatalyst-runtime-check.sh 26.6.0 maccatalyst-x64
```

The app writes `slidesnet-44710-result.txt` and `slidesnet-44710-output.pdf`. The result is exact:

- `RESULT: PASS` means the Mac Catalyst app loaded Aspose.Slides and exported a PDF successfully.
- `RESULT: FAIL` includes the full exception and stack trace.
