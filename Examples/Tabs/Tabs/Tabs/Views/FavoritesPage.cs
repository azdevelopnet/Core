using System;
using Xamarin.Forms;
using Xamarin.Forms.Core;
namespace Tabs.Views
{
    public class FavoritesPage : CorePage<SomeViewModel>
    {
        public FavoritesPage()
        {
            this.Title = "Favorites";

            var btn = new CoreButton()
            {
                Margin = 20,
                Text = "Favorite Kitties",
                Style = CoreStyles.LightOrange,
                Command = new Command(async () =>
                {
                    await CoreSettings.AppNav.PushAsync(new FavoritesSubPage());
                })
            };

            Content = new StackLayout()
            {
                Children =
                {
                    new Label(){Text="Favorites Page", Margin=20},
                    btn
                }
            };
        }
    }
}
