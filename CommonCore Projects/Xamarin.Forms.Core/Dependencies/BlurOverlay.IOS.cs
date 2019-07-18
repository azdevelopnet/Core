#if __IOS__
using System;
using Xamarin.Forms.Core;
using UIKit;

[assembly: Xamarin.Forms.Dependency(typeof(BlurOverlay))]
namespace Xamarin.Forms.Core
{
    //https://blog.xamarin.com/adding-view-effects-in-ios-8/
	public class BlurOverlay : IBlurOverlay
	{
		public static UIVisualEffectView visualEffectView;

		public void Show()
		{
			var controller = GetUIController();
			UIVisualEffect blurEffect = UIBlurEffect.FromStyle(UIBlurEffectStyle.Regular);
			visualEffectView = new UIVisualEffectView(blurEffect);
			visualEffectView.Frame = controller.View.Bounds;
			controller.View.AddSubview(visualEffectView);
		}

		public void Hide()
		{
			visualEffectView?.RemoveFromSuperview();
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
