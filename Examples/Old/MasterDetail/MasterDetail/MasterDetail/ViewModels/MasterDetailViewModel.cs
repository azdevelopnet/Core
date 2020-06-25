using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using MasterDetail.Views;
using Xamarin.Forms;
using Xamarin.Forms.Core;

namespace MasterDetail.ViewModels
{
    public class SlidingPageItem
    {
        public string Title { get; set; }

        public string IconSource { get; set; }

        public Type TargetType { get; set; }
    }

    public class MasterDetailViewModel : CoreViewModel
    {
        private bool _isPresented;

        private Dictionary<string, NavigationPage> navPages { get; set; } = new Dictionary<string, NavigationPage>();

        public bool IsPresented { get; set; }

        public ObservableCollection<SlidingPageItem> MasterPageItems { get; set; }

        public ICommand NavClicked { get; set; }

        public MasterDetailViewModel()
        {
            SetNavigation();
            NavClicked = new CoreCommand((obj) => { NavClickedMethod(obj); });
        }

        private void NavClickedMethod(object obj)
        {
            var item = (SlidingPageItem)obj;
            var page = (MasterDetailPage)Application.Current.MainPage;

            if (!navPages.ContainsKey(item.TargetType.Name))
            {
                var np = new NavigationPage((Page)Activator.CreateInstance(item.TargetType))
                {
                    BarBackgroundColor = Color.FromHex("#b85921"),
                    BarTextColor = Color.White
                };
                CoreSettings.AppNav = np.Navigation;
                navPages.Add(item.TargetType.Name, np);
            }
            page.Detail = navPages[item.TargetType.Name];

            page.IsPresented = false;
        }

        private void SetNavigation()
        {
            var lst = new List<SlidingPageItem>();
            lst.Add(new SlidingPageItem
            {
                Title = "Page One",
                IconSource = "index24.png",
                TargetType = typeof(PageOne)
            });
            lst.Add(new SlidingPageItem
            {
                Title = "Page Two",
                IconSource = "index24.png",
                TargetType = typeof(PageTwo)
            });
            lst.Add(new SlidingPageItem
            {
                Title = "Page Three",
                IconSource = "index24.png",
                TargetType = typeof(PageThree)
            });

            MasterPageItems = lst.ToObservable();
        }


        public override void OnMasterDetailPresented()
        {
            IsPresented = !IsPresented;
        }

        public override void OnViewMessageReceived(string key, object obj)
        {

        }
    }
}
