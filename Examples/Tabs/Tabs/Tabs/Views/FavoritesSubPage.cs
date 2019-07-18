using System;
using Xamarin.Forms;
using Xamarin.Forms.Core;

namespace Tabs.Views
{
    public class FavoritesSubPage : CorePage<SomeViewModel>
    {
        public FavoritesSubPage()
        {
            this.Title = "Favorite Kitties";

            Content = new StackLayout()
            {
                Padding = 20,
                Children = { new Label() { Text = "Favorite Kitties" } }
            };
        }
    }
}
