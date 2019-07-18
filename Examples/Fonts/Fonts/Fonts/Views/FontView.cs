using System;
using Xamarin.Forms;
using Xamarin.Forms.Core;

namespace Fonts.Views
{
    public class FontView : ContentView
    {
        public FontView()
        {
            var imgLabel = new Label()
            {
                FontSize = 32,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center
            };
            imgLabel.SetBinding(Label.TextProperty, "Unicode");
            imgLabel.SetBinding(Label.FontFamilyProperty, "FontFamily");


            var descript = new Label()
            {
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
                FontSize = 10,
            };
            descript.SetBinding(Label.TextProperty, "FriendlyName");

            Content = new StackContainer(true)
            {
                Spacing = 5,
                Children = { imgLabel, descript }
            };

        }
    }
}
