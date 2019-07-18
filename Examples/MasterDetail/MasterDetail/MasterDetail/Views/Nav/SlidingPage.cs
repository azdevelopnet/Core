using System;
using FFImageLoading.Forms;
using MasterDetail.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Core;

namespace MasterDetail.Views.Nav
{
    public class SlidingPage : CorePage<MasterDetailViewModel>
    {

        public SlidingPage()
        {
            BackgroundColor = Color.FromHex("#b85921");

            var monkey = new CachedImage()
            {
                Margin = 5,
                Source = "iconwhite.png"
            };
            var navTitle = new Label()
            {
                Text = "Common Core",
                TextColor = Color.White,
                Margin = 5
            };
            var navSubtitle = new Label()
            {
                Text = "Options Menu",
                TextColor = Color.White,
                Style = CoreStyles.AddressCell
            };

            var topPanel = new StackLayout()
            {
                Padding = new Thickness(10, 0, 10, 10),
                BackgroundColor = Color.FromHex("#b85921"),
                Orientation = StackOrientation.Horizontal,
                Children = { monkey, new StackLayout() { Children = { navTitle, navSubtitle } } }
            };

            var listView = new CoreListView
            {
                BackgroundColor = Color.White,
                ItemTemplate = new DataTemplate(typeof(SlidingPageCell)),
                VerticalOptions = LayoutOptions.FillAndExpand,
                SeparatorVisibility = SeparatorVisibility.None,
            };
            listView.SetBinding(CoreListView.ItemsSourceProperty, "MasterPageItems");
            listView.SetBinding(CoreListView.ItemClickCommandProperty, "NavClicked");

            Padding = new Thickness(0, 40, 0, 0);
            Icon = "hamburger.png";
            Title = "Reference Guide";
            Content = new StackContainer(true)
            {
                VerticalOptions = LayoutOptions.FillAndExpand,
                Children = {
                    topPanel,
                    listView
                }
            };

        }
    }
}
