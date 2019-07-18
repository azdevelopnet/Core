#if __IOS__
using System;
using UIKit;

[assembly: Xamarin.Forms.Dependency(typeof(Xamarin.Forms.Core.ViewStack))]
namespace Xamarin.Forms.Core
{
    public class ViewStack : IViewStack
    {
        public void DismissTopView()
        {
            var controller = GetUIController();
            controller.DismissViewController(true, null);
        }

        private UIViewController GetUIController()
        {
            var win = UIApplication.SharedApplication.KeyWindow;
            var vc = win.RootViewController;
            while (vc.PresentedViewController != null)
                vc = vc.PresentedViewController;
            return vc;
        }
    }
}
#endif
