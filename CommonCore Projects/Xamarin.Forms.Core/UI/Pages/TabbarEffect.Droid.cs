#if __ANDROID__

using Android.Views;
using Google.Android.Material.BottomNavigation;
using Xamarin.Forms;
using Xamarin.Forms.Core;
using Xamarin.Forms.Platform.Android;
using Xamarin.Forms.Platform.Android.AppCompat;
using XPViews = Android.Views.View;

[assembly: ExportEffect(typeof(TabbarEffect), "TabbarEffect")]
namespace Xamarin.Forms.Core
{
    public class TabbarEffect : PlatformEffect
    {
        private CoreTabbedPage _tabbedPage;
        private TabbedPageRenderer _renderer;

        public void UpdateVisiblity()
        {
            if (_renderer != null)
            {
                var ViewGroup = _renderer.ViewGroup;

                for (int i = 0; i <= ViewGroup.ChildCount - 1; i++)
                {
                    var childView = ViewGroup.GetChildAt(i);
                    if (childView is ViewGroup viewGroup)
                    {
                        for (int j = 0; j <= viewGroup.ChildCount - 1; j++)
                        {
                            var childRelativeLayoutView = viewGroup.GetChildAt(j);
                            if (childRelativeLayoutView is BottomNavigationView)
                            {
                                if (_tabbedPage.IsHidden)
                                    ((BottomNavigationView)childRelativeLayoutView).Visibility = ViewStates.Invisible;
                                else
                                    ((BottomNavigationView)childRelativeLayoutView).Visibility = ViewStates.Visible;
                            }
                        }
                    }
                }


            }
        }

        protected override void OnAttached()
        {
            if (Container != null && Element!=null)
            {
                _tabbedPage = (CoreTabbedPage)Element;
                _renderer = (TabbedPageRenderer)this.Container;
                UpdateVisiblity();
            }

        }

        protected override void OnDetached()
        {
            //throw new System.NotImplementedException();
        }
    }
}
#endif
