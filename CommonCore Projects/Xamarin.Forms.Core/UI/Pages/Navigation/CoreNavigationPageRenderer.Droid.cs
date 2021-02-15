#if __ANDROID__
using System;
using Xamarin.Forms;
using Xamarin.Forms.Core;
using Xamarin.Forms.Platform.Android.AppCompat;
using Views = Android.Views;
using AToolbar = AndroidX.AppCompat.Widget.Toolbar;
using System.Reflection;
using Android.Content.Res;
using Android.Widget;
using Android.Content;

[assembly: ExportRenderer(typeof(NavigationPage), typeof(CoreNavigationPageRenderer))]
namespace Xamarin.Forms.Core
{
    /// <summary>
    /// Core navigation page renderer. Provides override implementation for listener
    /// on the toolbar events.
    /// </summary>
    public class CoreNavigationPageRenderer : NavigationPageRenderer, Views.View.IOnClickListener
    {

        private FieldInfo ToolbarFieldInfo;

        private bool _disposed;
        private AToolbar _toolbar;

        public CoreNavigationPageRenderer(Context ctx):base(ctx)
        {
            // get _toolbar private field info
            ToolbarFieldInfo = typeof(NavigationPageRenderer).GetField("_toolbar",
                    BindingFlags.NonPublic | BindingFlags.Instance)
                ;
        }


        private bool IsMasterDetail()
        {
            var n = Application.Current.MainPage.GetType().BaseType.Name;
            var array = n.Split('`');
            if (array[0].Equals("CoreMasterDetailPage"))
                return true;
            else
                return false;
        }

        public INavigation Navigation
        {
            get
            {
                if (Application.Current.MainPage is NavigationPage)
                {
                    return ((NavigationPage)Application.Current.MainPage).Navigation;
                }
                if (Application.Current.MainPage is TabbedPage)
                {
                    var tab = (TabbedPage)Application.Current.MainPage;
                    if (tab.CurrentPage is INavigation)
                        return ((NavigationPage)tab.CurrentPage).Navigation;
                    else
                        return null;
                }
                if (Application.Current.MainPage is FlyoutPage)
                {
                    var md = (FlyoutPage)Application.Current.MainPage;
                    if (md.Detail is NavigationPage)
                        return ((NavigationPage)md.Detail).Navigation;
                    else
                        return null;
                }

                return null;
            }
        }

        public new void OnClick(Views.View v)
        {

            if (IsMasterDetail() && Navigation.NavigationStack.Count <= 1)
            {
                CoreDependencyService.InvokeMasterDetailEvent();
                return;
            }

            // Call the NavigationPage which will trigger the default behavior
            // The default behavior is to navigate back if the Page derived classes return true from OnBackButtonPressed override            
            var curPage = Element.CurrentPage as BasePages;
            if (curPage == null)
            {
                Element.PopAsync();
            }
            else
            {
                //if (curPage.NeedOverrideSoftBackButton)
                //    curPage.OnSoftBackButtonPressed();
                //else
                    Element.PopAsync();
            }
        }

        protected override void OnLayout(bool changed, int l, int t, int r, int b)
        {

                base.OnLayout(changed, l, t, r, b);

                UpdateToolbarInstance();
        }

        protected override void OnConfigurationChanged(Configuration newConfig)
        {
            base.OnConfigurationChanged(newConfig);

            UpdateToolbarInstance();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && !_disposed)
            {
                _disposed = true;

                RemoveToolbarInstance();
            }

            base.Dispose(disposing);
        }

        private void UpdateToolbarInstance()
        {
            RemoveToolbarInstance();
            GetToolbarInstance();
        }

        private void GetToolbarInstance()
        {
            try
            {
                _toolbar = (AToolbar)ToolbarFieldInfo.GetValue(this);
                _toolbar.SetNavigationOnClickListener(this);
            }
            catch (Exception exception)
            {
                System.Diagnostics.Debug.WriteLine($"Can't get toolbar with error: {exception.Message}");
            }
        }

        private void RemoveToolbarInstance()
        {
            if (_toolbar == null) return;
            _toolbar.SetNavigationOnClickListener(null);
            _toolbar = null;
        }

    }
}
#endif
