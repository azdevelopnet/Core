#if __IOS__
using System;
using CoreGraphics;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Core;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(MaterialFrame), typeof(MaterialFrameRenderer))]
namespace Xamarin.Forms.Core
{
    public class MaterialFrameRenderer : FrameRenderer
    {
        public override void Draw(CGRect rect)
        {
            base.Draw(rect);

            // Update shadow to match better material design standards of elevation
            Layer.ShadowRadius = 2.0f;
            Layer.ShadowColor = UIColor.Gray.CGColor;
            Layer.ShadowOffset = new CGSize(2, 2);
            Layer.ShadowOpacity = 0.80f;
            Layer.ShadowPath = UIBezierPath.FromRect(Layer.Bounds).CGPath;
            Layer.MasksToBounds = false;
        }
    }
}
#endif
