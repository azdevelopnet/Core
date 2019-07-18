#if __ANDROID__
using System;
using Android.Text.Method;
using Android.Text.Util;
using Android.Widget;
using Xamarin.Forms.Core;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Util = Android.Util;
using Graphics = Android.Graphics;
using Plugin.CurrentActivity;
using Android.Content;

[assembly: ExportRenderer(typeof(CoreTextArea), typeof(CoreTextAreaRenderer))]
namespace Xamarin.Forms.Core
{
    public class CoreTextAreaRenderer : ViewRenderer<CoreTextArea, TextView>
    {
        private TextView txtView;
        private CoreTextArea parent;

        public CoreTextAreaRenderer(Context ctx) : base(ctx)
        {

        }

        protected override void OnElementChanged(ElementChangedEventArgs<CoreTextArea> e)
        {
            base.OnElementChanged(e);
            if (e.NewElement != null)
                parent = e.NewElement;

            if (txtView == null)
            {
                txtView = new TextView(CrossCurrentActivity.Current.Activity);
                txtView.Text = e.NewElement.Text;

                var textColor = Graphics.Color.Black;
                if (((int)parent.TextColor.R) != -1)
                    textColor = e.NewElement.TextColor.ToAndroid();

                if (parent.LinksEnabled)
                    Linkify.AddLinks(txtView, MatchOptions.All);

                txtView.SetTextSize(Util.ComplexUnitType.Sp, (float)parent.FontSize);
                txtView.SetTextColor(textColor);
                SetNativeControl(txtView);
            }
        }
    }
}
#endif

