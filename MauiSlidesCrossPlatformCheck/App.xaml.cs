namespace MauiSlidesCrossPlatformCheck;

public class App : Application
{
    public App()
    {
        MainPage = new ContentPage
        {
            Content = new VerticalStackLayout
            {
                Padding = 24,
                Children =
                {
                    new Label
                    {
                        Text = "SLIDESNET-44710 MAUI Mac Catalyst check",
                        FontSize = 18
                    },
                    new Label
                    {
                        Text = SlidesSmokeCheck.Describe(),
                        FontSize = 13
                    }
                }
            }
        };
    }
}
