using System;
using MasterDetail.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Core;

namespace MasterDetail.Views.Nav
{
    public class MainNav: CoreMasterDetailPage<MasterDetailViewModel>
    {
        public static Page CurrentDetail { get; set; }
        public MainNav()
        {
            try
            {
                Master = new SlidingPage();
                Detail = new Xamarin.Forms.NavigationPage(new SomePage())
                {
                    BarBackgroundColor = Color.FromHex("#b85921"),
                    BarTextColor = Color.White
                };
                CoreSettings.AppNav = Detail.Navigation;
                MainNav.CurrentDetail = Detail;

            }
            catch (Exception ex)
            {
                var x = ex.Message;
            }

            this.SetBinding(MasterDetailPage.IsPresentedProperty, new Binding("IsPresented", BindingMode.TwoWay));
        }
    }
}
