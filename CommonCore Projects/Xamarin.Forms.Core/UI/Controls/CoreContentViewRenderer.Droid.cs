#if __ANDROID__
using System;
using Android.Graphics;
using Android.Util;
using Xamarin.Forms.Core;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Plugin.CurrentActivity;
using Android.Content;

[assembly: ExportRenderer(typeof(CoreContentView), typeof(CoreContentViewRenderer))]
namespace Xamarin.Forms.Core
{
    public class CoreContentViewRenderer : VisualElementRenderer<ContentView>
    {
        private float _cornerRadius;
        private RectF _bounds;
        private Path _path;

        public CoreContentViewRenderer(Context ctx) : base(ctx)
        {

        }
        protected override void OnElementChanged(ElementChangedEventArgs<ContentView> e)
        {
            base.OnElementChanged(e);

            if (e.OldElement != null)
            {
                return;
            }

            var element = (CoreContentView)Element;

            _cornerRadius = TypedValue.ApplyDimension(ComplexUnitType.Dip, (float)element.CornerRadius,
                Context.Resources.DisplayMetrics);
        }

        protected override void OnSizeChanged(int w, int h, int oldw, int oldh)
        {
            base.OnSizeChanged(w, h, oldw, oldh);
            if (w != oldw && h != oldh)
            {
                _bounds = new RectF(0, 0, w, h);
            }

            _path = new Path();
            _path.Reset();
            _path.AddRoundRect(_bounds, _cornerRadius, _cornerRadius, Path.Direction.Cw);
            _path.Close();
        }

        public override void Draw(Canvas canvas)
        {
            canvas.Save();
            canvas.ClipPath(_path);
            base.Draw(canvas);
            canvas.Restore();
        }
    }
}
#endif

