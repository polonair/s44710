using Aspose.Slides;

namespace MauiSlidesCrossPlatformCheck;

public static class SlidesSmokeCheck
{
    public static string Describe()
    {
        using var presentation = new Presentation();
        return $"Aspose.Slides loaded. Slides count: {presentation.Slides.Count}.";
    }
}
