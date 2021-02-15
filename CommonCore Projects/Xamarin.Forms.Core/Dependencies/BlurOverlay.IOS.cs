#if __IOS__
using System;
using Xamarin.Forms.Core;
using UIKit;
using System.Threading.Tasks;

[assembly: Xamarin.Forms.Dependency(typeof(BlurOverlay))]
namespace Xamarin.Forms.Core
{
	public class BlurOverlay : IBlurOverlay
	{
        public Task BlurAsync()
        {
            var controller = UIApplication.SharedApplication.KeyWindow.RootViewController;
            _blurredView = CreateBlurEffectView(controller);
            controller.View.AddSubview(_blurredView);

            return Task.FromResult(true);
        }

        public void Unblur()
        {
            if (_blurredView == null)
            {
                return;
            }

            _blurredView.RemoveFromSuperview();
            _blurredView.Dispose();
            _blurredView = null;
        }

        private UIVisualEffectView CreateBlurEffectView(UIViewController controller)
        {
            var blurEffect = UIBlurEffect.FromStyle(UIBlurEffectStyle.Light);
            var blurEffectView = new UIVisualEffectView(blurEffect);
            blurEffectView.Frame = controller.View.Bounds;
            blurEffectView.AutoresizingMask = UIViewAutoresizing.FlexibleWidth | UIViewAutoresizing.FlexibleHeight;

            return blurEffectView;
        }

        private UIVisualEffectView _blurredView;
    }
}
#endif
