using System;
using Xamarin.Forms;
using Xamarin.Forms.Core;

namespace Tabs.Views
{
    public class MainTabPage : CoreTabbedPage
    {
        public MainTabPage()
        {

            this.UnSelectedForegroundColor = Color.FromHex("#000000");
            this.SelectedForegroundColor = Color.FromHex("#d3d3d3");
            this.TabBackgroundColor = Color.FromHex("#2196f3");
           // this.BarTextColor = Color.Green;
            //this.IsToolbarBottom = true; 

            Children.Add( new NavigationPage(new HomePage() { Title = "Home"  }) { Icon = "home.png", Title="Home"});
            Children.Add(new NavigationPage(new FavoritesPage() { Title = "Favorites" }) { Icon = "star.png", Title= "Favorites" });
            Children.Add(new NavigationPage(new AboutPage() { Title = "About" }) { Icon = "info.png", Title= "About" });

        }

        protected override void OnCurrentPageChanged()
        {
            base.OnCurrentPageChanged();
            this.Title = this.CurrentPage.Title;

        }
    }
}
