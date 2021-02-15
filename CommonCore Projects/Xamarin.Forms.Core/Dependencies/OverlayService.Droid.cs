#if __ANDROID__
using System;
using Android.App;
using Android.Graphics.Drawables;
using Android.Views;
using Plugin.CurrentActivity;
using Xamarin.Forms.Core;
using Xamarin.Forms.Platform.Android;
using XPView  = Android.Views.View;
using XPGraphics = Android.Graphics;

[assembly: Xamarin.Forms.Dependency(typeof(OverlayService))]
namespace Xamarin.Forms.Core
{
    public class OverlayService : IOverlayService
    {
        private XPView _nativeView;

        private Dialog _dialog;

        public void InitLoadingPage(ContentPage page)
        {
            if (page != null)
            {
                page.Parent = Xamarin.Forms.Application.Current.MainPage;
                page.Layout(new Rectangle(0, 0,
                    Xamarin.Forms.Application.Current.MainPage.Width,
                    Xamarin.Forms.Application.Current.MainPage.Height));

                var renderer = page.GetOrCreateRenderer();
                _nativeView = renderer.View;
                _dialog = new Dialog(CrossCurrentActivity.Current.Activity);
                _dialog.RequestWindowFeature((int)WindowFeatures.NoTitle);
                _dialog.SetCancelable(false);
                _dialog.SetContentView(_nativeView);
                Window window = _dialog.Window;
                window.SetLayout(ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.MatchParent);
                window.ClearFlags(WindowManagerFlags.DimBehind);
                window.SetBackgroundDrawable(new ColorDrawable(XPGraphics.Color.Transparent));

            }
        }

        public void ShowOverlay(ContentPage page)
        {
            InitLoadingPage(page);
            _dialog.Show();
        }

        public void HideOverlay()
        {
            _dialog.Hide();
        }
    }
}
#endif
