using System;
using Xamarin.Forms;
using Xamarin.Forms.Core;
namespace Tabs.Views
{
    public class HomePage : CorePage<SomeViewModel>
    {
        public HomePage()
        {
            this.Title = "Home Page";

            var btn = new CoreButton()
            {
                Margin = 20,
                Text = "Hello Kitty",
                Style = CoreStyles.LightOrange,
                Command = new Command(async () =>
                {
                    await CoreSettings.AppNav.PushAsync(new SubHomePage());
                })
            };

            Content = new StackLayout()
            {
                Children =
                {
                    new Label(){Text="Home Page", Margin=20},
                    btn

                }
            };
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            if (Device.RuntimePlatform != "iOS")
                GC.Collect();
        }

    }
}
