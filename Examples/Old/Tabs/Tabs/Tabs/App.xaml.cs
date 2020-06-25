using System;
using Tabs.Views;
using Xamarin.Forms;
using Xamarin.Forms.Core;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace Tabs
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new MainTabPage();
        }
    }
}
