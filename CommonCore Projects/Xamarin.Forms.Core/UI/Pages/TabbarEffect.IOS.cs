#if __IOS__
using System;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Core;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportEffect(typeof(TabbarEffect), "TabbarEffect")]
namespace Xamarin.Forms.Core
{
    public class TabbarEffect : PlatformEffect
    {
        private CoreTabbedPage _tabbedPage;
        private UITabBar _tabBar;

        public void UpdateVisiblity()
        {
            if (_tabBar != null)
            {
               _tabBar.Hidden = _tabbedPage.IsHidden;
            }
        }

        protected override void OnAttached()
        {
            if (Container != null && Element != null)
            {
                _tabbedPage = (CoreTabbedPage)Element;
                foreach(var view in this.Container.Subviews)
                {
                    if(view is UITabBar)
                    {
                        _tabBar = (UITabBar)view;
                        break;
                    }
                }
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
