using Aspose.Slides;
using Aspose.Slides.Export;
using System.Text;

namespace MauiSlidesCrossPlatformCheck;

public static class SlidesSmokeCheck
{
    public static SmokeCheckResult Run()
    {
        var report = new StringBuilder();
        var outputDirectory = Path.Combine(FileSystem.AppDataDirectory, "SLIDESNET-44710");
        Directory.CreateDirectory(outputDirectory);

        var reportPath = Path.Combine(outputDirectory, "slidesnet-44710-result.txt");
        var pdfPath = Path.Combine(outputDirectory, "slidesnet-44710-output.pdf");

        try
        {
            var slidesAssembly = typeof(Presentation).Assembly;
            report.AppendLine("SLIDESNET-44710 Aspose.Slides MAUI Mac Catalyst runtime check");
            report.AppendLine($"Timestamp UTC: {DateTime.UtcNow:O}");
            report.AppendLine($"AppDataDirectory: {FileSystem.AppDataDirectory}");
            report.AppendLine($"Aspose.Slides assembly: {slidesAssembly.FullName}");
            report.AppendLine($"Aspose.Slides location: {slidesAssembly.Location}");
            report.AppendLine($"Process architecture: {System.Runtime.InteropServices.RuntimeInformation.ProcessArchitecture}");
            report.AppendLine($"OS: {System.Runtime.InteropServices.RuntimeInformation.OSDescription}");
            report.AppendLine();

            using var presentation = new Presentation();
            presentation.Slides[0].Shapes.AddAutoShape(ShapeType.Rectangle, 40, 40, 560, 120)
                .TextFrame.Text = "SLIDESNET-44710 Mac Catalyst PDF export";
            presentation.Save(pdfPath, SaveFormat.Pdf);

            var pdfInfo = new FileInfo(pdfPath);
            report.AppendLine("RESULT: PASS");
            report.AppendLine($"Slides count: {presentation.Slides.Count}");
            report.AppendLine($"PDF path: {pdfPath}");
            report.AppendLine($"PDF bytes: {pdfInfo.Length}");
            File.WriteAllText(reportPath, report.ToString());

            return new SmokeCheckResult("PASS", report.ToString() + Environment.NewLine + $"Report path: {reportPath}");
        }
        catch (Exception ex)
        {
            report.AppendLine("RESULT: FAIL");
            report.AppendLine(ex.ToString());
            File.WriteAllText(reportPath, report.ToString());

            return new SmokeCheckResult("FAIL", report.ToString() + Environment.NewLine + $"Report path: {reportPath}");
        }
    }
}

public sealed record SmokeCheckResult(string Status, string Details);
