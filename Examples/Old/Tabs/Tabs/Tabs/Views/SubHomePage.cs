using System;
using Xamarin.Forms.Core;
using Xamarin.Forms;

namespace Tabs.Views
{
    public class SubHomePage : CorePage<SomeViewModel>
    {
        public SubHomePage()
        {
            this.Title = "Hello Kitty";

            Content = new StackLayout()
            {
                Padding = 20,
                Children = { new Label() { Text = "Hello Kitty" } }
            };
        }
    }
}
