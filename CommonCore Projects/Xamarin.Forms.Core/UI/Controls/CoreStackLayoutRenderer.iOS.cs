#if __IOS__
using System;
using CoreAnimation;
using CoreGraphics;
using Xamarin.Forms;
using Xamarin.Forms.Core;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(CoreStackLayout), typeof(CoreStackLayoutRenderer))]
namespace Xamarin.Forms.Core
{
    public class CoreStackLayoutRenderer : VisualElementRenderer<StackLayout>
    {
        public override void Draw(CGRect rect)
        {

            base.Draw(rect);
            var stack = (CoreStackLayout)this.Element;
            CGColor startColor = stack.StartColor.ToCGColor();

            CGColor endColor = stack.EndColor.ToCGColor();

            var gradientLayer = new CAGradientLayer();

            gradientLayer.Frame = rect;
            gradientLayer.Colors = new CGColor[] { startColor, endColor};

            NativeView.Layer.InsertSublayer(gradientLayer, 0);
        }
    }
}
#endif
