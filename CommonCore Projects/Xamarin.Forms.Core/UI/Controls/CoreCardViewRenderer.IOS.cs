#if __IOS__
using System;
using CoreAnimation;
using CoreGraphics;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Core;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(CoreCardView), typeof(CoreCardViewRenderer))]
namespace Xamarin.Forms.Core
{
    public class CoreCardViewRenderer : AppleCardView, IVisualElementRenderer
    {
        public event EventHandler<VisualElementChangedEventArgs> ElementChanged;

        public CoreCardView TheView { get { return this.Element == null ? null : (CoreCardView)Element; } }

        public VisualElementTracker Tracker { get; private set; }

        public VisualElementPackager Packager { get; private set; }

        public VisualElement Element { get; private set; }

        public UIView NativeView { get { return this as UIView; } }

        public UIViewController ViewController { get { return null; } }

        public void SetElement(VisualElement element)
        {
            var oldElement = this.Element;

            if (oldElement != null)
            {
                oldElement.PropertyChanged -= this.HandlePropertyChanged;
            }

            this.Element = element;

            if (this.Element != null)
            {
                this.Element.PropertyChanged += this.HandlePropertyChanged;
            }

            this.RemoveAllSubviews();
            this.Tracker = new VisualElementTracker(this);

            this.Packager = new VisualElementPackager(this);
            this.Packager.Load();

            this.SetContentPadding((int)TheView.Padding.Left, (int)TheView.Padding.Top, (int)TheView.Padding.Right, (int)TheView.Padding.Bottom);

            this.SetCardBackgroundColor(this.TheView.BackgroundColor.ToUIColor());

            if (ElementChanged != null)
            {
                this.ElementChanged(this, new VisualElementChangedEventArgs(oldElement, this.Element));
            }
        }

        public SizeRequest GetDesiredSize(double widthConstraint, double heightConstraint)
        {
            var size = NativeView.GetSizeRequest(widthConstraint, heightConstraint, 44.0, 44.0);
            return size;
        }

        public void SetElementSize(Size size)
        {
            this.Element.Layout(new Rectangle(this.Element.X, this.Element.Y, size.Width, size.Height));
        }

        protected override void Dispose(bool disposing) { }

        private void HandlePropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Content")
            {
                //Tracker.UpdateLayout ();
            }
            else if (
                e.PropertyName == CoreCardView.WidthProperty.PropertyName ||
                e.PropertyName == CoreCardView.HeightProperty.PropertyName ||
                e.PropertyName == CoreCardView.XProperty.PropertyName ||
                e.PropertyName == CoreCardView.YProperty.PropertyName ||
                e.PropertyName == CoreCardView.CornerRadiusProperty.PropertyName)
            {
                this.Element.Layout(this.Element.Bounds);

                var radius = (this.Element as CoreCardView).CornerRadius;
                var bound = this.Element.Bounds;
                this.DrawBorder(new CoreGraphics.CGRect(bound.X, bound.Y, bound.Width, bound.Height), (nfloat)radius);

            
               
            }
            else if (e.PropertyName == CoreCardView.PaddingProperty.PropertyName)
            {
                SetContentPadding((int)TheView.Padding.Left, (int)TheView.Padding.Top, (int)TheView.Padding.Right, (int)TheView.Padding.Bottom);
            }
            else if (e.PropertyName == CoreCardView.BackgroundColorProperty.PropertyName)
            {
				SetCardBackgroundColor(TheView.BackgroundColor.ToUIColor()); 
            }

  
        }

        public override void Draw(CGRect rect)
        {

            this.Layer.ShadowColor = UIColor.DarkGray.CGColor;
            this.Layer.ShadowOpacity = 0.6f;
            this.Layer.ShadowRadius = 3.0f;
            this.Layer.ShadowOffset = new System.Drawing.SizeF(3f, 3f);
            this.Layer.ShouldRasterize = true;
            this.Layer.MasksToBounds = false;
            base.Draw(rect);

        }

        private void SetCardBackgroundColor(UIColor color)
        {
            this.BackgroundColor = color;
        }

        private void SetContentPadding(int left, int top, int right, int bottom) { }
    }

    internal static class Extensions
    {
        internal static void RemoveAllSubviews(this UIView super)
        {
            if (super == null)
            {
                return;
            }
            for (int i = 0; i < super.Subviews.Length; i++)
            {
                var subview = super.Subviews[i];
                subview.RemoveFromSuperview();
            }
        }
    }
}
#endif
