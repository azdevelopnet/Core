using System;
using SkiaSharp;
using SkiaSharp.Views.Forms;
using Xamarin.Forms;

namespace Xamarin.Forms.Core
{

    public partial class CoreCircularProgressbar : SKCanvasView
    {
        private CoreCircleProgressDrawer _ProgressDrawer;

        public static readonly BindableProperty ProgressProperty = BindableProperty.Create(
            "Progress", typeof(double), typeof(CoreCircularProgressbar), propertyChanged: OnProgressChanged);

        public double Progress
        {
            get { return (double)GetValue(ProgressProperty); }
            set { SetValue(ProgressProperty, value); }
        }
        private static void OnProgressChanged(BindableObject bindable, object oldvalue, object newvalue)
        {
            var context = bindable as SKCanvasView;
            context.InvalidateSurface();
        }

        public CoreCircularProgressbar(float radius, SKColor backgroundColor, SKColor ringColor, float strokeWidth = 5)
        {
            var circle = new CoreCircle(radius, (info) => new SKPoint((float)info.Width / 2, (float)info.Height / 2));
            _ProgressDrawer = new CoreCircleProgressDrawer(this, circle, () => (float)Progress, strokeWidth, ringColor, backgroundColor);

        }
    }

    public partial class CoreRadialProgressbar : SKCanvasView
    {
        private CoreRadialProgressDrawer _ProgressDrawer;

        public static readonly BindableProperty ProgressProperty = BindableProperty.Create(
            "Progress", typeof(double), typeof(CoreCircularProgressbar), propertyChanged: OnProgressChanged);

        public double Progress
        {
            get { return (double)GetValue(ProgressProperty); }
            set { SetValue(ProgressProperty, value); }
        }
        private static void OnProgressChanged(BindableObject bindable, object oldvalue, object newvalue)
        {
            var context = bindable as SKCanvasView;
            context.InvalidateSurface();
        }

        public CoreRadialProgressbar(float radius, SKColor backgroundColor, SKColor ringColor, float strokeWidth = 5)
        {
            var circle = new CoreCircle(radius, (info) => new SKPoint((float)info.Width / 2, (float)info.Height / 2));
            _ProgressDrawer = new CoreRadialProgressDrawer(this, circle, () => (float)(Progress / 2), strokeWidth, ringColor, backgroundColor);

        }
    }

    #region Internal Helper Classes
    internal class CoreCircle
    {
        private readonly Func<SKImageInfo, SKPoint> _centerfunc;

        public CoreCircle(float radius, Func<SKImageInfo, SKPoint> centerfunc)
        {
            _centerfunc = centerfunc;
            Radius = radius;
        }
        public SKPoint Center { get; set; }
        public float Radius { get; set; }
        public SKRect Rect => new SKRect(Center.X - Radius, Center.Y - Radius, Center.X + Radius, Center.Y + Radius);

        public void CalculateCenter(SKImageInfo argsInfo)
        {
            Center = _centerfunc.Invoke(argsInfo);
        }
    }

    internal class CoreCircleProgressDrawer
    {

        public CoreCircleProgressDrawer(SKCanvasView canvas, CoreCircle circle, Func<float> progress, float strokeWidth, SKColor progressColor, SKColor foregroundColor)
        {
            canvas.PaintSurface += (sender, args) =>
            {
                circle.CalculateCenter(args.Info);
                args.Surface.Canvas.Clear();
                DrawCircle(args.Surface.Canvas, circle, strokeWidth, foregroundColor);
                DrawArc(args.Surface.Canvas, circle, progress, strokeWidth, progressColor);

            };
        }

        private void DrawCircle(SKCanvas canvas, CoreCircle circle, float strokewidth, SKColor color)
        {
            canvas.DrawCircle(circle.Center, circle.Radius,
                new SKPaint()
                {
                    StrokeWidth = strokewidth,
                    Color = color,
                    IsStroke = true
                });

        }

        private void DrawArc(SKCanvas canvas, CoreCircle circle, Func<float> progress, float strokewidth, SKColor color)
        {
            var angle = progress.Invoke() * 3.6f;
            canvas.DrawArc(circle.Rect, 270, angle, false,
                new SKPaint() { StrokeWidth = strokewidth, Color = color, IsStroke = true, StrokeCap = SKStrokeCap.Round });
        }

    }

    internal class CoreRadialProgressDrawer
    {

        public CoreRadialProgressDrawer(SKCanvasView canvas, CoreCircle circle, Func<float> progress, float strokeWidth, SKColor progressColor, SKColor foregroundColor)
        {
            canvas.PaintSurface += (sender, args) =>
            {
                circle.CalculateCenter(args.Info);
                args.Surface.Canvas.Clear();
                DrawArc(args.Surface.Canvas, circle, () => (float)50, strokeWidth, foregroundColor);
                DrawArc(args.Surface.Canvas, circle, progress, strokeWidth, progressColor);

            };
        }

        private void DrawCircle(SKCanvas canvas, CoreCircle circle, float strokewidth, SKColor color)
        {
            canvas.DrawCircle(circle.Center, circle.Radius,
                new SKPaint()
                {
                    StrokeWidth = strokewidth,
                    Color = color,
                    IsStroke = true
                });

        }

        private void DrawArc(SKCanvas canvas, CoreCircle circle, Func<float> progress, float strokewidth, SKColor color)
        {
            var angle = progress.Invoke() * 3.6f;
            canvas.DrawArc(circle.Rect, 180, angle, false,
                new SKPaint() { StrokeWidth = strokewidth, Color = color, IsStroke = true, StrokeCap = SKStrokeCap.Round });
        }

    }
    #endregion
}
