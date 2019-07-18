#if __IOS__
using System;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Core;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(NavigationPage), typeof(CoreNavigationPageRenderer))]
namespace Xamarin.Forms.Core
{
    public class CoreNavigationPageRenderer : NavigationRenderer
    {
        protected override void OnElementChanged(VisualElementChangedEventArgs e)
        {
            base.OnElementChanged(e);


            //if (UIDevice.CurrentDevice.CheckSystemVersion(11, 0))
            //NavigationBar.PrefersLargeTitles = CoreSettings.PrefersLargeTitles;


        }
    }
}
#endif
