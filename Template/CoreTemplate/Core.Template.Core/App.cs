using System;

using Xamarin.Forms;
using Xamarin.Forms.Core;

namespace Core.Template.Core
{
    public partial class App : Application
    {
        public App()
        {
            CoreSettings.Start();
            MainPage = new NavigationPage(new SomePage());

        }

    }
}
