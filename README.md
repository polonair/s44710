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
dotnet publish MauiSlidesCrossPlatformCheck/MauiSlidesCrossPlatformCheck.csproj \
  -f net10.0-maccatalyst \
  -c Release \
  -p:SlidesPackageVersion=26.6.0 \
  -p:RuntimeIdentifier=maccatalyst-x64 \
  -p:CreatePackage=false \
  -p:EnableCodeSigning=false
```

Then launch the produced `.app` and confirm that the app opens and the `Presentation` constructor succeeds.
