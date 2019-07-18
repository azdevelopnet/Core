#if __IOS__
using System;
using System.ComponentModel;
using System.Linq;
using Xamarin.Forms.Core;
using CoreAnimation;
using CoreGraphics;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(CoreButton), typeof(CoreButtonRenderer))]
namespace Xamarin.Forms.Core
{
    public class CoreButtonRenderer : ButtonRenderer
    {
        CoreButton caller;
        CAGradientLayer gradient;

        public override void LayoutSubviews()
        {
            if (Control != null)
            {
                foreach (var layer in Control.Layer.Sublayers.Where(layer => layer is CAGradientLayer))
                {
                    layer.Frame = GetBounds();
                }
            }
            base.LayoutSubviews();
        }

        private CGRect GetBounds()
        {
            return new CGRect(0, 0, caller.Bounds.Width, caller.Bounds.Height);
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Button> e)
        {
            base.OnElementChanged(e);

            if (e.OldElement == null)
            {
                caller = e.NewElement as CoreButton;

                gradient = new CAGradientLayer();
                gradient.Frame = Control.Bounds;
                gradient.CornerRadius = Control.Layer.CornerRadius = caller.CornerRadius;
                
                gradient.Colors = new CGColor[]
                {
                    caller.StartColor.ToCGColor(),
                    caller.EndColor.ToCGColor(),
                };

				if (caller.ShadowColor.NotNull())
                {
                    Control.Layer.ShadowRadius = (nfloat)caller.ShadowRadius;
                    Control.Layer.ShadowColor = caller.ShadowColor.ToCGColor();
                    Control.Layer.ShadowOffset = new CGSize(0.0f, caller.ShadowOffset);
                    Control.Layer.ShadowOpacity = caller.ShadowOpacity;
                    Control.Layer.MasksToBounds = false;

                }
                Control.SetTitleColor(caller.DisabledTextColor.ToUIColor(), UIControlState.Disabled);

                Control?.Layer.InsertSublayer(gradient, 0);

            }
        }

		private void SetDropShadow()
		{
			Control.Layer.ShadowRadius = (nfloat)caller.ShadowRadius;
			Control.Layer.ShadowColor = caller.ShadowColor.ToCGColor();
			Control.Layer.ShadowOffset = new CGSize(0.0f, caller.ShadowOffset);
			Control.Layer.ShadowOpacity = caller.ShadowOpacity;
			Control.Layer.MasksToBounds = false;
		}
        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == CoreButton.IsEnabledProperty.PropertyName)
            {
                Control.Layer.ShadowColor = caller.ShadowColor.ToCGColor();
                gradient.Colors = new CGColor[]
                 {
                    caller.StartColor.ToCGColor(),
                    caller.EndColor.ToCGColor(),
                 };
				Control.SetNeedsDisplay();

            }
			if (e.PropertyName == CoreButton.ShadowOpacityProperty.PropertyName ||
			   e.PropertyName == CoreButton.ShadowColorProperty.PropertyName ||
			   e.PropertyName == CoreButton.ShadowOffsetProperty.PropertyName ||
			   e.PropertyName == CoreButton.ShadowRadiusProperty.PropertyName)
			{
				SetDropShadow();
				Control.SetNeedsDisplay();
			}

            if (e.PropertyName == "Width" || e.PropertyName == "Height")
            {
                gradient.Frame = caller.Bounds.ToRectangleF();
            }

            base.OnElementPropertyChanged(sender, e);
        }

    }
}
#endif

