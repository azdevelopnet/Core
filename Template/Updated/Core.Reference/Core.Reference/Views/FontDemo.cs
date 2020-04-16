using System;
using Xamarin.Forms;
using Xamarin.Forms.Core;

namespace Core.Reference.Views
{
    public class FontDemo: CorePage<SomeViewModel>
    {
        public FontDemo()
        {
            this.Title = "Custom Font Example";

            Content = new StackLayout()
            {
                Children =
                {
                    new Label()
                    {
                        Margin = new Thickness(50,50,0,10),
                        FontSize = 22,
                        FontFamily = CoreFontFamily.EntypoPlus,
                        Text = EntypoPlus.Aircraft
                    },
                    new Label()
                    {
                        Margin = new Thickness(50,10,0,10),
                        FontSize = 22,
                        FontFamily = CoreFontFamily.Fontawesome,
                        Text = Fontawesome.Android
                    },
                    new Label()
                    {
                        Margin = new Thickness(50,10,0,10),
                        FontSize = 22,
                        FontFamily = CoreFontFamily.IonIcons,
                        Text = IonIcons.IosAlert
                    },
                    new Label()
                    {
                        Margin = new Thickness(50,10,0,10),
                        FontSize = 22,
                        FontFamily = CoreFontFamily.Material,
                        Text = Material.AspectRatio
                    },
                    new Label()
                    {
                        Margin = new Thickness(50,10,0,10),
                        FontSize = 22,
                        FontFamily = CoreFontFamily.Meteocons,
                        Text = Meteocons.Five
                    },
                    new Label()
                    {
                        Margin = new Thickness(50,10,0,10),
                        FontSize = 22,
                        FontFamily = CoreFontFamily.SimplelineIcons,
                        Text = SimplelineIcons.ActionRedo
                    },
                    new Label()
                    {
                        Margin = new Thickness(50,10,0,10),
                        FontSize = 22,
                        FontFamily = CoreFontFamily.Typicons,
                        Text = Typicons.AdjustBrightness
                    },
                    new Label()
                    {
                        Margin = new Thickness(50,10,0,10),
                        FontSize = 22,
                        FontFamily = CoreFontFamily.WeatherIcons,
                        Text = WeatherIcons.DayRainWind
                    }
                }
            };
        }
    }
}
