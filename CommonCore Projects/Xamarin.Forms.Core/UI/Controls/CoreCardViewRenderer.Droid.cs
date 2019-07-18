#if __ANDROID__
using System;
using System.ComponentModel;
using Android.Content;
using Android.Support.V7.Widget;
using Android.Views;
using Xamarin.Forms;
using Xamarin.Forms.Core;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(CoreCardView), typeof(CoreCardViewRenderer))]
namespace Xamarin.Forms.Core
{
    public class CoreCardViewRenderer : CardView, IVisualElementRenderer
    {
        int? _defaultLabelFor;

        public event EventHandler<VisualElementChangedEventArgs> ElementChanged;
        public event EventHandler<PropertyChangedEventArgs> ElementPropertyChanged;

        public VisualElementTracker Tracker { get; private set; }

        public VisualElementPackager Packager { get; private set; }

        public ViewGroup ViewGroup { get { return this; } }

        public VisualElement Element { get; private set; }

        private ViewGroup packed;

        public CoreCardViewRenderer(Context ctx) : base(ctx){}

        public void SetElement(VisualElement element)
        {
            var oldElement = this.Element;

            if (oldElement != null)
                oldElement.PropertyChanged -= HandlePropertyChanged;

            this.Element = element;
            if (this.Element != null)
            {

                this.Element.PropertyChanged += HandlePropertyChanged;
            }

            ViewGroup.RemoveAllViews();
            Tracker = new VisualElementTracker(this);

            Packager = new VisualElementPackager(this);
            Packager.Load();

            UseCompatPadding = true;

            SetContentPadding((int)TheView.Padding.Left, (int)TheView.Padding.Top,
                   (int)TheView.Padding.Right, (int)TheView.Padding.Bottom);

            Radius = TheView.CornerRadius;
            SetCardBackgroundColor(TheView.BackgroundColor.ToAndroid());

            if (ElementChanged != null)
                ElementChanged(this, new VisualElementChangedEventArgs(oldElement, this.Element));
        }

        public CoreCardView TheView
        {
            get { return this.Element == null ? null : (CoreCardView)Element; }
        }

        public global::Android.Views.View View => this;

        void HandlePropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Content")
            {
                Tracker.UpdateLayout();
            }
            else if (e.PropertyName == CoreCardView.PaddingProperty.PropertyName)
            {
                SetContentPadding((int)TheView.Padding.Left, (int)TheView.Padding.Top,
                    (int)TheView.Padding.Right, (int)TheView.Padding.Bottom);
            }
            else if (e.PropertyName == CoreCardView.CornerRadiusProperty.PropertyName)
            {
                this.Radius = TheView.CornerRadius;
            }
            else if (e.PropertyName == CoreCardView.BackgroundColorProperty.PropertyName)
            {
                if (TheView.BackgroundColor != null)
                    SetCardBackgroundColor(TheView.BackgroundColor.ToAndroid());
            }
        }

        public SizeRequest GetDesiredSize(int widthConstraint, int heightConstraint)
        {
            packed.Measure(widthConstraint, heightConstraint);
            return new SizeRequest(new Size(packed.MeasuredWidth, packed.MeasuredHeight));
        }

        public void UpdateLayout()
        {
            if (Tracker == null)
                return;

            Tracker.UpdateLayout();
        }

        public void SetLabelFor(int? id)
        {
            if (_defaultLabelFor == null)
                _defaultLabelFor = LabelFor;

            LabelFor = (int)(id ?? _defaultLabelFor);
        }
    }
}
#endif
