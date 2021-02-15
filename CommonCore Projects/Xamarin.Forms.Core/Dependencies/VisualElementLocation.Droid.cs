#if __ANDROID__
using System;
using Xamarin.Forms.Core;
using XFPlatform = Xamarin.Forms.Platform.Android.Platform;

[assembly: Xamarin.Forms.Dependency(typeof(VisualElementLocation))]
namespace Xamarin.Forms.Core
{
    public class VisualElementLocation : IVisualElementLocation
    {
        public System.Drawing.PointF GetCoordinates(global::Xamarin.Forms.VisualElement element)
        {
            var renderer = XFPlatform.GetRenderer(element);
            var nativeView = renderer.View;
            var location = new int[2];
            var density = nativeView.Context.Resources.DisplayMetrics.Density;

            nativeView.GetLocationOnScreen(location);
            var coord = new System.Drawing.PointF(location[0] / density, location[1] / density);

            //Something is wrong with the calculation and must be adjusted by 91.5
            coord.Y = coord.Y - 80.2f;

            return coord;
        }
    }
}
#endif
