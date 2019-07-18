#if __ANDROID__
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.Widget;
using Xamarin.Forms.Core;
using Xamarin.Forms.Platform.Android;
using Resource = Android.Resource;
using Widget = Android.Widget;
using Graphics = Android.Graphics;
using Views = Android.Views;
using Plugin.CurrentActivity;

[assembly: Xamarin.Forms.Dependency(typeof(OverlayDependency))]
namespace Xamarin.Forms.Core
{
    public class OverlayDependency : IOverlayDependency
    {
        private Dialog dialog;

        public Context Ctx
        {
            get => CrossCurrentActivity.Current.Activity;
        }

        public void HideOverlay()
        {
            dialog?.Dismiss();
        }

        public void ShowOverlay(string message)
        {
            ShowOverlay(message, Xamarin.Forms.Color.Black, 0.75f);
        }

        public void ShowOverlay(string message, Xamarin.Forms.Color backgroundColor, float backgroundOpacity)
        {
            var alpha = (int)(255 * backgroundOpacity);

            dialog = new Dialog(Ctx, Resource.Style.ThemeNoTitleBar);
            Drawable d = new ColorDrawable(backgroundColor.ToAndroid());
            d.SetAlpha(alpha);
            dialog.Window.SetBackgroundDrawable(d);

            var layout = new LinearLayout(Ctx);
            layout.Orientation = Widget.Orientation.Vertical;
            layout.LayoutParameters = new LinearLayout.LayoutParams(LinearLayout.LayoutParams.MatchParent, LinearLayout.LayoutParams.MatchParent);

            var header = new LinearLayout(Ctx);
            var headerParameter = new LinearLayout.LayoutParams(LinearLayout.LayoutParams.WrapContent, LinearLayout.LayoutParams.WrapContent);
            headerParameter.Weight = 1;
            header.LayoutParameters = headerParameter;

            var prg = new Widget.ProgressBar(Ctx);
            prg.Indeterminate = true;

            prg.IndeterminateDrawable.SetColorFilter(Graphics.Color.White, PorterDuff.Mode.Multiply);
            var parameters = new LinearLayout.LayoutParams(LinearLayout.LayoutParams.WrapContent, LinearLayout.LayoutParams.WrapContent);
            parameters.Gravity = Views.GravityFlags.Center;
            parameters.SetMargins(0, 10, 0, 10);
            prg.LayoutParameters = parameters;

            var txtField = new TextView(Ctx);
            txtField.Text = message;
            txtField.TextSize = 22;
            txtField.SetTextColor(Graphics.Color.White);
            txtField.LayoutParameters = parameters;

            var footer = new LinearLayout(Ctx);
            var footerParameter = new LinearLayout.LayoutParams(LinearLayout.LayoutParams.WrapContent, LinearLayout.LayoutParams.WrapContent);
            footerParameter.Weight = 1;
            footer.LayoutParameters = footerParameter;

            layout.AddView(header);
            layout.AddView(prg);
            layout.AddView(txtField);
            layout.AddView(footer);
            dialog.SetContentView(layout);
            dialog.Show();
        }
    }
}
#endif
