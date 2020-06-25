using System;
using MasterDetail.Views.Nav;
using Xamarin.Forms;
using Xamarin.Forms.Core;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace MasterDetail
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new MainNav();
        }


    }
}
