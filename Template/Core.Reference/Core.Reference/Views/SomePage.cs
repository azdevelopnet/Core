using System;
using Xamarin.Forms;
using Xamarin.Forms.Core;

namespace Core.Reference
{
    public class SomePage : CorePage<SomeViewModel>
    {
        public SomePage()
        {
            this.Title = "Some Page";
            //this.Visual = VisualMarker.Material;

            this.BackgroundImageSource = ImageSource.FromFile("Background.png");

            Content = new StackLayout()
            {
                Padding = 20,
                Children = {
                    new Label()
                    {
                        Text="Enter Text:",
                        Margin = new Thickness(5, 5, 5, 0)
                    },
                    new CoreMaskedEntry()
                    {
                        Margin = 5,
                        BackgroundColor = Color.Transparent
                    }.Bind(CoreMaskedEntry.TextProperty, nameof(SomeViewModel.SomeText)),
                    new Label()
                    {
                        Margin = 5
                    }.Bind(Label.TextProperty, nameof(SomeViewModel.SomeText), converter: CoreSettings.UpperText),
                    new Button() // or use CoreButton
                    {
                        Text="Some Action",
                        //Style = CoreStyles.LightOrange,
                        Margin=5,
                    }.Bind(Button.CommandProperty,nameof(SomeViewModel.SomeAction)),
                    new Label()
                    {
                        Margin = 5
                    }.Bind(Label.TextProperty, nameof(SomeViewModel.TotalItems), stringFormat: "Total count is {0}")
                }
            };
        }

    }
}