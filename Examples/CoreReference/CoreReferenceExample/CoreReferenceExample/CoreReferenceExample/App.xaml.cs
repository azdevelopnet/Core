using System;
using Xamarin.Forms;
using Xamarin.Forms.Core;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace CoreReferenceExample
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            XF.Material.Forms.Material.Init(this);
            /*
             In order for the status bar to reflect color change in iOS add the following to info.plist
             -  Status bar style  - White
             -  View controller-based status bar appearance - No
             */
            MainPage = new NavigationPage(new Dashboard())
            {
                BarBackgroundColor = Color.FromHex(CoreStyles.BackgroundColor),
                BarTextColor = Color.White
            };
        }

    }
}
