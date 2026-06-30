namespace MauiSlidesCrossPlatformCheck;

public class App : Application
{
    public App()
    {
        var result = SlidesSmokeCheck.Run();

        MainPage = new ContentPage
        {
            Content = new ScrollView
            {
                Content = new VerticalStackLayout
                {
                    Padding = 24,
                    Spacing = 12,
                    Children =
                    {
                        new Label
                        {
                            Text = "SLIDESNET-44710 MAUI Mac Catalyst check",
                            FontSize = 18,
                            FontAttributes = FontAttributes.Bold
                        },
                        new Label
                        {
                            Text = result.Status,
                            FontSize = 16,
                            FontAttributes = FontAttributes.Bold
                        },
                        new Label
                        {
                            Text = result.Details,
                            FontSize = 13
                        }
                    }
                }
            }
        };
    }
}
