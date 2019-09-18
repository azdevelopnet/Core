using System;
using Xamarin.Forms;
using Xamarin.Forms.Core;
using Xamarin.Forms.Core.AzurePush;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace AzurePushExample
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            CoreAzurePush.Init();

            MainPage = new NavigationPage(new SomePage());
        }

    }
}
