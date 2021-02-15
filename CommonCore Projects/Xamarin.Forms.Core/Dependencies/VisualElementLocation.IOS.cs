#if __IOS__
using System;
using Xamarin.Forms.Core;
using XFPlatform = Xamarin.Forms.Platform.iOS.Platform;

[assembly: Xamarin.Forms.Dependency(typeof(VisualElementLocation))]
namespace Xamarin.Forms.Core
{
    public class VisualElementLocation : IVisualElementLocation
    {
        public System.Drawing.PointF GetCoordinates(global::Xamarin.Forms.VisualElement element)
        {
            var renderer = XFPlatform.GetRenderer(element);
            var nativeView = renderer.NativeView;
            var rect = nativeView.Superview.ConvertPointToView(nativeView.Frame.Location, null);
            var coord =  new System.Drawing.PointF((int)Math.Round(rect.X), (int)Math.Round(rect.Y));

            //Something is wrong with the calculation and must be adjusted by 91.5
            coord.Y = coord.Y - 91.5f;

            return coord;
        }
    }
}
#endif
