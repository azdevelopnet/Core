#if __IOS__
using System;
using UIKit;
using Xamarin.Forms.Core;
using Xamarin.Forms.Platform.iOS;

[assembly: Xamarin.Forms.Dependency(typeof(OverlayService))]
namespace Xamarin.Forms.Core
{
    public class OverlayService : IOverlayService
    {
        private UIView _nativeView;

        public void InitLoadingPage(ContentPage page)
        {
            if (page != null)
            {
                page.Parent = Xamarin.Forms.Application.Current.MainPage;
                page.Layout(new Rectangle(0, 0,
                    Xamarin.Forms.Application.Current.MainPage.Width,
                    Xamarin.Forms.Application.Current.MainPage.Height));

                var renderer = page.GetOrCreateRenderer();
                _nativeView = renderer.NativeView;
            }
        }

        public void ShowOverlay(ContentPage page)
        {
            InitLoadingPage(page);
            UIApplication.SharedApplication.KeyWindow.AddSubview(_nativeView);
        }

        public void HideOverlay()
        {
            _nativeView.RemoveFromSuperview();
        }
    }
}
#endif


