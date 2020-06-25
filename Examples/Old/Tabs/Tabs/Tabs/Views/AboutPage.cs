using System;
using Xamarin.Forms;
using Xamarin.Forms.Core;

namespace Tabs.Views
{
    public class AboutPage : CorePage<SomeViewModel>
    {
        public AboutPage()
        {
            this.Title = "About";

            Content = new StackLayout()
            {
                Padding = 20,
                Children = { new Label() { Text = "About Page" } }
            };
        }
    }
}
