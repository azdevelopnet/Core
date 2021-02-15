using System;
using SkiaSharp.Views.Forms;
using Xamarin.Forms;
using System.Diagnostics;
using SkiaSharp;

namespace Xamarin.Forms.Core
{
    public enum CoreRatingType
    {
        Full,
        Half,
        Tenths,
        Floating
    }

    public static class CoreRatingColors
    {
        public static SKColor Red => SKColor.Parse("F44336");
        public static SKColor Pink => SKColor.Parse("E91E63");
        public static SKColor Purple => SKColor.Parse("9C27B0");
        public static SKColor DeepPurple => SKColor.Parse("673AB7");
        public static SKColor Indigo => SKColor.Parse("3F51B5");
        public static SKColor Blue => SKColor.Parse("2196F3");
        public static SKColor LightBlue => SKColor.Parse("03A9F4");
        public static SKColor Cyan => SKColor.Parse("00BCD4");
        public static SKColor Teal => SKColor.Parse("009688");
        public static SKColor Green => SKColor.Parse("4CAF50");
        public static SKColor LightGreen => SKColor.Parse("8BC34A");
        public static SKColor Lime => SKColor.Parse("CDDC39");
        public static SKColor Yellow => SKColor.Parse("FFEB3B");
        public static SKColor Amber => SKColor.Parse("FFC107");
        public static SKColor Orange => SKColor.Parse("FF9800");
        public static SKColor DeepOrange => SKColor.Parse("FF5722");
        public static SKColor Brown => SKColor.Parse("795548");
        public static SKColor Grey => SKColor.Parse("9E9E9E");
        public static SKColor BlueGrey => SKColor.Parse("607D8B");
        public static SKColor Black => SKColor.Parse("000000");
        public static SKColor White => SKColor.Parse("FFFFFF");

    }

    public static class CoreRatingPaths
    {
        public const string Star = "M9 11.3l3.71 2.7-1.42-4.36L15 7h-4.55L9 2.5 7.55 7H3l3.71 2.64L5.29 14z";
        public const string Heart = "M12 21.35l-1.45-1.32C5.4 15.36 2 12.28 2 8.5 2 5.42 4.42 3 7.5 3c1.74 0 3.41.81 4.5 2.09C13.09 3.81 14.76 3 16.5 3 19.58 3 22 5.42 22 8.5c0 3.78-3.4 6.86-8.55 11.54L12 21.35z";
        public const string Circle = "M12 2C6.48 2 2 6.48 2 12s4.48 10 10 10 10-4.48 10-10S17.52 2 12 2z";
        public const string Bar = "M6 6h36v2H6z";
        public const string BatteryAlert = "M15.67 4H14V2h-4v2H8.33C7.6 4 7 4.6 7 5.33v15.33C7 21.4 7.6 22 8.33 22h7.33c.74 0 1.34-.6 1.34-1.33V5.33C17 4.6 16.4 4 15.67 4zM13 18h-2v-2h2v2zm0-4h-2V9h2v5z";
        public const string BatteryCharging = "M15.67 4H14V2h-4v2H8.33C7.6 4 7 4.6 7 5.33v15.33C7 21.4 7.6 22 8.33 22h7.33c.74 0 1.34-.6 1.34-1.33V5.33C17 4.6 16.4 4 15.67 4zM11 20v-5.5H9L13 7v5.5h2L11 20z";
        public const string Like = "M1 21h4V9H1v12zm22-11c0-1.1-.9-2-2-2h-6.31l.95-4.57.03-.32c0-.41-.17-.79-.44-1.06L14.17 1 7.59 7.59C7.22 7.95 7 8.45 7 9v10c0 1.1.9 2 2 2h9c.83 0 1.54-.5 1.84-1.22l3.02-7.05c.09-.23.14-.47.14-.73v-1.91l-.01-.01L23 10z";
        public const string Dislike = "M15 3H6c-.83 0-1.54.5-1.84 1.22l-3.02 7.05c-.09.23-.14.47-.14.73v1.91l.01.01L1 14c0 1.1.9 2 2 2h6.31l-.95 4.57-.03.32c0 .41.17.79.44 1.06L9.83 23l6.59-6.59c.36-.36.58-.86.58-1.41V5c0-1.1-.9-2-2-2zm4 0v12h4V3h-4z";
        public const string Theaters = "M18 3v2h-2V3H8v2H6V3H4v18h2v-2h2v2h8v-2h2v2h2V3h-2zM8 17H6v-2h2v2zm0-4H6v-2h2v2zm0-4H6V7h2v2zm10 8h-2v-2h2v2zm0-4h-2v-2h2v2zm0-4h-2V7h2v2z";
        public const string Problem = "M1 21h22L12 2 1 21zm12-3h-2v-2h2v2zm0-4h-2v-4h2v4z";
    }

    public partial class CoreRating : SKCanvasView, IDisposable
    {

        #region private
        private PanGestureRecognizer panGestureRecognizer = new PanGestureRecognizer();
        private double touchX;
        private double touchY;
        private float ItemWidth { get; set; }
        private float ItemHeight { get; set; }
        private float CanvasScale { get; set; }
        private SKColor SKColorOn { get; set; } = CoreRatingColors.Amber;
        private SKColor SKOutlineOnColor { get; set; } = SKColors.Transparent;
        private SKColor SKOutlineOffColor { get; set; } = CoreRatingColors.Grey;

        private double CalculateValue(double x)
        {
            if (x < this.ItemWidth)
                return (double)x / this.ItemWidth;
            else if (x < this.ItemWidth + this.Spacing)
                return 1;
            else
                return 1 + CalculateValue(x - (this.ItemWidth + this.Spacing));
        }

        private void Handle_PaintSurface(object sender, SKPaintSurfaceEventArgs e)
        {
            this.Draw(e.Surface.Canvas, e.Info.Width, e.Info.Height);
        }

        protected override void OnTouch(SKTouchEventArgs e)
        {
            this.touchX = e.Location.X;
            this.touchY = e.Location.Y;
            this.SetValue(touchX, touchY);
            this.InvalidateSurface();
        }

        private void PanGestureRecognizer_PanUpdated(object sender, PanUpdatedEventArgs e)
        {
            var point = ConvertToPixel(new Point(e.TotalX, e.TotalY));
            if (e.StatusType != GestureStatus.Completed)
            {
                this.SetValue(touchX + point.X, touchY + e.TotalY);
                this.InvalidateSurface();
            }
        }

        private static void OnPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var view = bindable as CoreRating;
            view.InvalidateSurface();
        }

        private static void OnValueChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var view = bindable as CoreRating;
            view.Value = view.ClampValue((double)newValue);
            OnPropertyChanged(bindable, oldValue, newValue);
        }

        private static void ColorOnChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var view = bindable as CoreRating;
            view.SKColorOn = ((Color)newValue).ToSKColor();
            OnPropertyChanged(bindable, oldValue, newValue);
        }

        private static void OutlineOffColorChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var view = bindable as CoreRating;
            view.SKOutlineOffColor = ((Color)newValue).ToSKColor();
            OnPropertyChanged(bindable, oldValue, newValue);
        }

        private static void OutlineOnColorChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var view = bindable as CoreRating;
            view.SKOutlineOnColor = ((Color)newValue).ToSKColor();
            OnPropertyChanged(bindable, oldValue, newValue);
        }

        private SKPoint ConvertToPixel(Point pt)
        {
            return new SKPoint((float)(this.CanvasSize.Width * pt.X / this.Width),
                               (float)(this.CanvasSize.Height * pt.Y / this.Height));
        }

        #endregion

        #region BindableProperties

        public static readonly BindableProperty ValueProperty = BindableProperty.Create(nameof(Value), typeof(double), typeof(CoreRating), default(double), propertyChanged: OnValueChanged, defaultBindingMode: BindingMode.TwoWay);
        public static readonly BindableProperty PathProperty = BindableProperty.Create(nameof(Path), typeof(string), typeof(CoreRating), CoreRatingPaths.Star, propertyChanged: OnPropertyChanged);
        public static readonly BindableProperty CountProperty = BindableProperty.Create(nameof(Count), typeof(int), typeof(CoreRating), 5, propertyChanged: OnPropertyChanged);
        public static readonly BindableProperty ColorOnProperty = BindableProperty.Create(nameof(ColorOn), typeof(Color), typeof(CoreRating), CoreRatingColors.Amber.ToFormsColor(), propertyChanged: ColorOnChanged);
        public static readonly BindableProperty OutlineOnColorProperty = BindableProperty.Create(nameof(OutlineOnColor), typeof(Color), typeof(CoreRating), SKColors.Transparent.ToFormsColor(), propertyChanged: OutlineOnColorChanged);
        public static readonly BindableProperty OutlineOffColorProperty = BindableProperty.Create(nameof(OutlineOffColor), typeof(Color), typeof(CoreRating), CoreRatingColors.Grey.ToFormsColor(), propertyChanged: OutlineOffColorChanged);
        public static readonly BindableProperty RatingTypeProperty = BindableProperty.Create(nameof(RatingType), typeof(CoreRatingType), typeof(CoreRating), CoreRatingType.Floating, propertyChanged: OnPropertyChanged);

        public double Value
        {
            get { return (double)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, this.ClampValue(value)); }
        }

        public string Path
        {
            get { return (string)GetValue(PathProperty); }
            set { SetValue(PathProperty, value); }
        }

        public int Count
        {
            get { return (int)GetValue(CountProperty); }
            set { SetValue(CountProperty, value); }
        }

        public Color ColorOn
        {
            get { return (Color)GetValue(ColorOnProperty); }
            set { SetValue(ColorOnProperty, value); }
        }

        public Color OutlineOnColor
        {
            get { return (Color)GetValue(OutlineOnColorProperty); }
            set { SetValue(OutlineOnColorProperty, value); }
        }

        public Color OutlineOffColor
        {
            get { return (Color)GetValue(OutlineOffColorProperty); }
            set { SetValue(OutlineOffColorProperty, value); }
        }

        public CoreRatingType RatingType
        {
            get { return (CoreRatingType)GetValue(RatingTypeProperty); }
            set { SetValue(RatingTypeProperty, value); }
        }

        #endregion

        #region properties
        /// <summary>
        /// Gets or sets the spacing between two rating elements
        /// </summary>
        public float Spacing { get; set; } = 8;

        /// <summary>
        /// Gets or sets the color of the canvas background.
        /// </summary>
        /// <value>The color of the canvas background.</value>
        public SKColor CanvasBackgroundColor { get; set; } = SKColors.Transparent;

        /// <summary>
        /// Gets or sets the width of the stroke.
        /// </summary>
        /// <value>The width of the stroke.</value>
		public float StrokeWidth { get; set; } = 0.1f;

        #endregion

        public CoreRating()
        {
            this.BackgroundColor = Color.Transparent;
            this.PaintSurface += Handle_PaintSurface;
            this.EnableTouchEvents = true;
            this.panGestureRecognizer.PanUpdated += PanGestureRecognizer_PanUpdated;
            this.GestureRecognizers.Add(panGestureRecognizer);
        }

        #region public methods

        /// <summary>
        /// Clamps the value between 0 and the number of items.
        /// </summary>
        /// <returns>The value.</returns>
        /// <param name="val">Value.</param>
        public double ClampValue(double val)
        {
            if (val < 0)
                return 0;
            else if (val > this.Count)
                return this.Count;
            else
                return val;
        }

        /// <summary>
        /// Sets the Rating value
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public void SetValue(double x, double y)
        {
            var val = this.CalculateValue(x);
            switch (this.RatingType)
            {
                case CoreRatingType.Full:
                    this.Value = ClampValue((double)Math.Ceiling(val));
                    break;
                case CoreRatingType.Half:
                    this.Value = ClampValue((double)Math.Round(val * 2) / 2);
                    break;
                case CoreRatingType.Floating:
                    this.Value = ClampValue(val);
                    break;
                case CoreRatingType.Tenths:
                    this.Value = ClampValue((double)Math.Round(val, 1));
                    break;

            }
        }

        /// <summary>
        /// Draws the rating view
        /// </summary>
        /// <param name="canvas"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public void Draw(SKCanvas canvas, int width, int height)
        {
            canvas.Clear(this.CanvasBackgroundColor);

            var path = SKPath.ParseSvgPathData(this.Path);

            var itemWidth = ((width - (this.Count - 1) * this.Spacing)) / this.Count;
            var scaleX = (itemWidth / (path.Bounds.Width));
            scaleX = (itemWidth - scaleX * this.StrokeWidth) / path.Bounds.Width;

            this.ItemHeight = height;
            var scaleY = this.ItemHeight / (path.Bounds.Height);
            scaleY = (this.ItemHeight - scaleY * this.StrokeWidth) / (path.Bounds.Height);

            this.CanvasScale = Math.Min(scaleX, scaleY);
            this.ItemWidth = path.Bounds.Width * this.CanvasScale;

            canvas.Scale(this.CanvasScale);
            canvas.Translate(this.StrokeWidth / 2, this.StrokeWidth / 2);
            canvas.Translate(-path.Bounds.Left, 0);
            canvas.Translate(0, -path.Bounds.Top);

            using (var strokePaint = new SKPaint
            {
                Style = SKPaintStyle.Stroke,
                Color = this.SKOutlineOnColor,
                StrokeWidth = this.StrokeWidth,
                StrokeJoin = SKStrokeJoin.Round,
                IsAntialias = true,
            })
            using (var fillPaint = new SKPaint
            {
                Style = SKPaintStyle.Fill,
                Color = this.SKColorOn,
                IsAntialias = true,
            })
            {
                for (int i = 0; i < this.Count; i++)
                {
                    if (i <= this.Value - 1) // Full
                    {
                        canvas.DrawPath(path, fillPaint);
                        canvas.DrawPath(path, strokePaint);
                    }
                    else if (i < this.Value) //Partial
                    {
                        float filledPercentage = (float)(this.Value - Math.Truncate(this.Value));
                        strokePaint.Color = this.SKOutlineOffColor;
                        canvas.DrawPath(path, strokePaint);

                        using (var rectPath = new SKPath())
                        {
                            var rect = SKRect.Create(path.Bounds.Left + path.Bounds.Width * filledPercentage, path.Bounds.Top, path.Bounds.Width * (1 - filledPercentage), this.ItemHeight);
                            rectPath.AddRect(rect);
                            canvas.ClipPath(rectPath, SKClipOperation.Difference);
                            canvas.DrawPath(path, fillPaint);
                        }
                    }
                    else //Empty
                    {
                        strokePaint.Color = this.SKOutlineOffColor;
                        canvas.DrawPath(path, strokePaint);
                    }

                    canvas.Translate((this.ItemWidth + this.Spacing) / this.CanvasScale, 0);
                }
            }

        }

        public void Dispose()
        {
            this.PaintSurface -= Handle_PaintSurface;
            if (this.panGestureRecognizer != null)
            {
                if (this.GestureRecognizers.Contains(this.panGestureRecognizer))
                    this.GestureRecognizers.Remove(this.panGestureRecognizer);
                this.panGestureRecognizer.PanUpdated -= PanGestureRecognizer_PanUpdated;
            }
        }

        #endregion

    }
}