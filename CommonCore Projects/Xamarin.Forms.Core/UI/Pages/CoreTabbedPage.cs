using System;
using Plugin.Badge.Abstractions;
using System.Linq;
using Xamarin.CommunityToolkit.Markup;
using Xamarin.Forms.Core;
using System.Collections.ObjectModel;
using PropertyChanged;

#if __ANDROID__
using Xamarin.Forms.PlatformConfiguration.AndroidSpecific;
#else
using Xamarin.Forms.Platform.iOS;
#endif

namespace Xamarin.Forms.Core
{
    #region CoreTabbedPage Implemetation

    public class LazyTabView<T> : ContentView where T : class, new()
    {
        public LazyTabView()
        {
            this.BindingContext = new T();
        }
    }

    [AddINotifyPropertyChangedInterface]
    public class LazyTabbedModel 
    {
        public bool IsRootPage { get; set; }
        public string AutomationId { get; set; }
        public int? BadgeCount { get; set; }
        public Color BadgeColor { get; set; }
        public Font BadgeFont { get; set; }
        public string Title { get; set; }
        public ImageSource IconSource { get; set; }
        public ContentPage TabPage { get; set; }
    }

    public class CoreTabbedPage : Xamarin.Forms.TabbedPage
    {
        private TabbarEffect _effect;
        public NavigationPage RootPage { get; set; }

        public static readonly BindableProperty IsHiddenProperty =
            BindableProperty.Create("IsHidden", typeof(bool), typeof(CoreTabbedPage), false);

        public static readonly BindableProperty IsToolbarBottomProperty =
            BindableProperty.Create(
                propertyName: nameof(IsToolbarBottom),
                returnType: typeof(bool),
                declaringType: typeof(CoreTabbedPage),
                defaultValue: false,
                propertyChanged: OnToolbarBottomPropertyChanged);

        public bool IsHidden
        {
            get { return (bool)GetValue(IsHiddenProperty); }
            set { SetValue(IsHiddenProperty, value); }
        }

        public bool IsToolbarBottom
        {
            get { return (bool)this.GetValue(IsToolbarBottomProperty); }
            set { this.SetValue(IsToolbarBottomProperty, value); }
        }

        public static readonly BindableProperty TabCollectionProperty = BindableProperty.Create(
            nameof(TabCollection),
            typeof(ObservableCollection<LazyTabbedModel>),
            typeof(CoreTabbedPage),
            null,
            propertyChanged: TabCollectionChangedEvent);

        public ObservableCollection<LazyTabbedModel> TabCollection
        {
            get => (ObservableCollection<LazyTabbedModel>)GetValue(TabCollectionProperty);
            set => SetValue(TabCollectionProperty, value);
        }

        public CoreTabbedPage()
        {
            _effect = new TabbarEffect();
            this.Effects.Add(_effect);
        }

        public static void TabCollectionChangedEvent(BindableObject bindable, object oldValue, object newvalue)
        {
            if (newvalue != null)
            {
                var obj = (CoreTabbedPage)bindable;
                obj.Children.Clear();
                if (newvalue != null)
                {
                    var lst = (ObservableCollection<LazyTabbedModel>)newvalue;

                    if (lst.Any(x => x.IsRootPage))
                    {
                        obj.PopulateRootPage();
                    }
                    else
                    {
                        obj.PopulateNonRootPages();
                    }
                }
            }
        }

        protected override void OnCurrentPageChanged()
        {
            LazyLoadPageCurrentPage();
        }

        public void NavigateByAutomationId(string automationId)
        {
            var child = Children.FirstOrDefault(x => x.AutomationId == automationId);
            if (child != null)
            {
                this.CurrentPage = child;
            }
        }

        public void NavigateRootPage()
        {
            if(RootPage!=null)
                PopulateRootPage();
        }

        public void NavigateTabbedPages()
        {
            if (RootPage != null)
                PopulateNonRootPages();
        }

        private void LazyLoadPageCurrentPage()
        {
            if (CurrentPage is NavigationPage)
            {
                var nav = (NavigationPage)CurrentPage;
                if (nav.CurrentPage is IAbstactLazyView)
                {
                    var lazy = (IAbstactLazyView)nav.CurrentPage;
                    if (!lazy.IsLoaded)
                        lazy.LoadView();
                }
            }
        }

        private void PopulateRootPage()
        {
            Children.Clear();
            foreach (var tab in TabCollection.Where(x => x.IsRootPage))
            {
                tab.TabPage.Title = tab.Title;
                tab.TabPage.BindingContext = tab;
                var nav = new NavigationPage(tab.TabPage) { IconImageSource = tab.IconSource, AutomationId = tab.AutomationId, Title = tab.Title };
                RootPage = nav;
                Children.Add(nav);
            }

            this.IsHidden = true;
            _effect.UpdateVisiblity();

#if __ANDROID__
            this.On<Xamarin.Forms.PlatformConfiguration.Android>().SetIsSwipePagingEnabled(false);
#endif

        }
        private void PopulateNonRootPages()
        {
            Children.Clear();
            foreach (var tab in TabCollection.Where(x => !x.IsRootPage))
            {
                tab.TabPage.Title = tab.Title;
                tab.TabPage.BindingContext = tab;
                var nav = new NavigationPage(tab.TabPage) { IconImageSource = tab.IconSource, AutomationId = tab.AutomationId, Title = tab.Title };
                Children.Add(nav);
            }

            this.IsHidden = false;
            _effect.UpdateVisiblity();

#if __ANDROID__
            this.On<Xamarin.Forms.PlatformConfiguration.Android>().SetIsSwipePagingEnabled(true);
#endif

        }

        private static void OnToolbarBottomPropertyChanged(BindableObject bindable, object value, object newValue)
        {

#if __ANDROID__
            var coreTabPage = ((CoreTabbedPage)bindable);
            if (coreTabPage.IsToolbarBottom)
            {
                coreTabPage.On<Xamarin.Forms.PlatformConfiguration.Android>().SetToolbarPlacement(ToolbarPlacement.Bottom);
            }
            else
            {
                coreTabPage.On<Xamarin.Forms.PlatformConfiguration.Android>().SetToolbarPlacement(ToolbarPlacement.Default);
            }
#endif

        }

    }

    #endregion

    #region Lazy Loading Internal Classes
    internal interface IAbstactLazyView
    {
        View Content { get; set; }

        bool IsLoaded { get; }

        void LoadView();
    }

    internal class LazyTabPage<TView> : ContentPage, IAbstactLazyView, IDisposable
    where TView : View, new()
    {
        public bool IsLoaded { get; protected set; }

        public LazyTabPage()
        {
            this.Bind(TabBadge.BadgeTextProperty, nameof(LazyTabbedModel.BadgeCount));
            this.Bind(TabBadge.BadgeColorProperty, nameof(LazyTabbedModel.BadgeColor));
            this.Bind(TabBadge.BadgeFontProperty, nameof(LazyTabbedModel.BadgeFont));
        }
        public void LoadView()
        {
            IsLoaded = true;
            View view = new TView();
            Content = view;
        }

        public void Dispose()
        {
            if (Content is IDisposable disposable)
            {
                disposable.Dispose();
            }
        }
       
    }
    #endregion
}

#region Code Example

/*  IMPLEMENTATION EXAMPLE

    public class TabViewThree : LazyTabView<VM>
    {
        public TabViewThree()
        {
            Content = new StackLayout()
            {
                Children =
                {
                    new Label()
                    {
                        Margin = 40,
                        Text="THIS IS PAGE Three"
                    }
                }
            };
        }
    }

   public class TabVM : INotifyPropertyChanged
    {
        public List<LazyTabbedModel> Pages { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;

        public TabVM()
        {
            var lst = new List<LazyTabbedModel>();
            lst.Add(new LazyTabbedModel()
            {
                AutomationId="PageOne",
                Title = "PageOne",
                BadgeColor = Color.Blue,
                BadgeCount = 6,
                IconSource = ImageSource.FromFile("book_binded.png"),
                TabPage = new LazyTabPage<TabViewOne>()
            });
            lst.Add(new LazyTabbedModel()
            {
                AutomationId="PageTwo",
                Title = "PageTwo",
                BadgeColor = Color.Blue,
                IconSource = ImageSource.FromFile("book_binded.png"),
                TabPage = new LazyTabPage<TabViewTwo>()
            });
            lst.Add(new LazyTabbedModel()
            {
                AutomationId="PageThree",
                Title = "PageThree",
                BadgeColor = Color.Red,
                BadgeCount = 10,
                IconSource = ImageSource.FromFile("book_binded.png"),
                TabPage = new LazyTabPage<TabViewThree>()
            });
            Pages = lst;
        }
    }

    public partial class App : Xamarin.Forms.Application
    {
        public App()
        {
            var vm = new TabVM();
            MainPage = new CoreTabbedPage() {
                SelectedTabColor = Color.Blue,
                UnselectedTabColor = Color.DarkGray,
                BarBackgroundColor = Color.Yellow,
                TabCollection = vm.Pages
            };
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
 
 */
#endregion