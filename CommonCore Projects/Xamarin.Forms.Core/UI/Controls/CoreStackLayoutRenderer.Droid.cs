#if __ANDROID__
using System;
using Android.Content;
using Plugin.CurrentActivity;
using Xamarin.Forms;
using Xamarin.Forms.Core;
using Xamarin.Forms.Platform.Android;
using Graphics = Android.Graphics;

[assembly: ExportRenderer(typeof(CoreStackLayout), typeof(CoreStackLayoutRenderer))]
namespace Xamarin.Forms.Core
{
    public class CoreStackLayoutRenderer : VisualElementRenderer<StackLayout>
    {
        private Color StartColor { get; set; }
        private Color EndColor { get; set; }

        public CoreStackLayoutRenderer(Context ctx) : base(ctx)
        {

        }
        protected override void DispatchDraw(global::Android.Graphics.Canvas canvas)
        {
            var gradient = new Graphics.LinearGradient(0, 0, 0, 
                    Height,
                    this.StartColor.ToAndroid(),
                    this.EndColor.ToAndroid(),
                    Graphics.Shader.TileMode.Mirror);

            var paint = new Graphics.Paint()
            {
                Dither = true,
            };
            paint.SetShader(gradient);
            canvas.DrawPaint(paint);
            base.DispatchDraw(canvas);
        }

        protected override void OnElementChanged(ElementChangedEventArgs<StackLayout> e)
        {
            base.OnElementChanged(e);

            if (e.OldElement != null || Element == null)
            {
                return;
            }
            try
            {
                var stack = e.NewElement as CoreStackLayout;
                this.StartColor = stack.StartColor;
                this.EndColor = stack.EndColor;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(@"ERROR:", ex.Message);
            }
        }
    }
}

#endif
