#if __ANDROID__
using System;
using Android.App;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.Renderscripts;
using Xamarin.Forms.Core;
using Resource = Android.Resource;
using Renderscripts = Android.Renderscripts;
using Android.Content;
using Plugin.CurrentActivity;
using static Android.Views.View;
using DroidView = Android.Views.View;
using Android.Views;

[assembly: Xamarin.Forms.Dependency(typeof(BlurOverlay))]
namespace Xamarin.Forms.Core
{
    public class BlurOverlay : IBlurOverlay
    {
        public static Dialog dialog;

        public Context Ctx
        {
            get => CrossCurrentActivity.Current.Activity;
        }

        private Bitmap ConvertViewToDrawable(DroidView view)
        {
            var spec = MeasureSpec.MakeMeasureSpec(0, MeasureSpecMode.Unspecified);
            view.Measure(spec, spec);
            view.Layout(0, 0, view.MeasuredWidth, view.MeasuredHeight);
            var b = Bitmap.CreateBitmap((int)view.MeasuredWidth, (int)view.MeasuredHeight, Bitmap.Config.Argb8888);
            var c = new Canvas(b);
            c.Translate((-view.ScrollX), (-view.ScrollY));
            view.Draw(c);
            return b;
        }

        public void Show()
        {
            var obj = (Activity)Ctx;
            var root = obj.Window.DecorView.FindViewById(Resource.Id.Content);
            //root.DrawingCacheEnabled = true;
            //var b = Bitmap.CreateBitmap(root.GetDrawingCache(true));
            //root.DrawingCacheEnabled = false;

            var b = ConvertViewToDrawable(root);

            // Create another bitmap that will hold the results of the filter.
            Bitmap blurredBitmap;
            blurredBitmap = Bitmap.CreateBitmap(b);

            // Create the Renderscript instance that will do the work.
            RenderScript rs = RenderScript.Create(obj);

            // Allocate memory for Renderscript to work with
            Allocation input = Allocation.CreateFromBitmap(rs, b, Allocation.MipmapControl.MipmapFull, AllocationUsage.Script);
            Allocation output = Allocation.CreateTyped(rs, input.Type);

            // Load up an instance of the specific script that we want to use.


            ScriptIntrinsicBlur script = ScriptIntrinsicBlur.Create(rs, Renderscripts.Element.U8_4(rs));
            script.SetInput(input);

            // Set the blur radius
            script.SetRadius(25);

            // Start the ScriptIntrinisicBlur
            script.ForEach(output);

            // Copy the output to the blurred bitmap
            output.CopyTo(blurredBitmap);

            dialog = new Dialog(obj, Resource.Style.ThemeNoTitleBar);

            var d = new BitmapDrawable(Ctx.Resources, blurredBitmap);

            dialog.Window.SetBackgroundDrawable(d);
            dialog.Show();
        }
        public void Hide()
        {
            dialog?.Hide();
        }

    }
}
#endif
