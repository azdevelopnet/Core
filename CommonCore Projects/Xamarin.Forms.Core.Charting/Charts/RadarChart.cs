using SkiaSharp;
using System;
using System.Linq;
using Xamarin.Forms.Core.Charting.Extensions;

namespace Xamarin.Forms.Core.Charting
{
    public class RadarChart : Chart
    {
        private const float Epsilon = 0.01f;
        public float LineSize { get; set; } = 3;
        public SKColor BorderLineColor { get; set; } = SKColors.LightGray.WithAlpha(110);
        public float BorderLineSize { get; set; } = 2;
        public PointMode PointMode { get; set; } = PointMode.Circle;
        public float PointSize { get; set; } = 14;

        private float AbsoluteMinimum => Inputs.Select(x => x.Value).Concat(new[] { MaxValue, MinValue, InternalMinValue ?? 0 }).Min(x => Math.Abs(x));

        private float AbsoluteMaximum => Inputs.Select(x => x.Value).Concat(new[] { MaxValue, MinValue, InternalMinValue ?? 0 }).Max(x => Math.Abs(x));

        private float ValueRange => AbsoluteMaximum - AbsoluteMinimum;

        public override void DrawContent(SKCanvas canvas, int width, int height)
        {
            var total = Inputs?.Count() ?? 0;

            if (total > 0)
            {
                var captionHeight = Inputs.Max(x =>
                {
                    var result = 0.0f;

                    var hasLabel = !string.IsNullOrEmpty(x.Label);
                    var hasValueLabel = !string.IsNullOrEmpty(x.DisplayValue);
                    if (hasLabel || hasValueLabel)
                    {
                        var hasOffset = hasLabel && hasValueLabel;
                        var captionMargin = LabelTextSize * 0.60f;
                        var space = hasOffset ? captionMargin : 0;

                        if (hasLabel)
                        {
                            result += LabelTextSize;
                        }

                        if (hasValueLabel)
                        {
                            result += LabelTextSize;
                        }
                    }

                    return result;
                });

                var center = new SKPoint(width / 2, height / 2);
                var radius = ((Math.Min(width, height) - (2 * Margin)) / 2) - captionHeight;
                var rangeAngle = (float)((Math.PI * 2) / total);
                var startAngle = (float)Math.PI;

                var nextInput = Inputs.First();
                var nextAngle = startAngle;
                var nextPoint = GetPoint(nextInput.Value * AnimationProgress, center, nextAngle, radius);

                DrawBorder(canvas, center, radius);

                using (var clip = new SKPath())
                {
                    clip.AddCircle(center.X, center.Y, radius);

                    for (int i = 0; i < total; i++)
                    {
                        var angle = nextAngle;
                        var input = nextInput;
                        var point = nextPoint;

                        var nextIndex = (i + 1) % total;
                        nextAngle = startAngle + (rangeAngle * nextIndex);
                        nextInput = Inputs.ElementAt(nextIndex);
                        nextPoint = GetPoint(nextInput.Value * AnimationProgress, center, nextAngle, radius);

                        canvas.Save();
                        canvas.ClipPath(clip);

                        // Border center bars
                        using (var paint = new SKPaint()
                        {
                            Style = SKPaintStyle.Stroke,
                            StrokeWidth = BorderLineSize,
                            Color = BorderLineColor,
                            IsAntialias = true,
                        })
                        {
                            var borderPoint = GetPoint(MaxValue, center, angle, radius);
                            canvas.DrawLine(point.X, point.Y, borderPoint.X, borderPoint.Y, paint);
                        }

                        // Values points and lines
                        using (var paint = new SKPaint()
                        {
                            Style = SKPaintStyle.Stroke,
                            StrokeWidth = BorderLineSize,
                            Color = input.Color.WithAlpha((byte)(input.Color.Alpha * 0.75f * AnimationProgress)),
                            PathEffect = SKPathEffect.CreateDash(new[] { BorderLineSize, BorderLineSize * 2 }, 0),
                            IsAntialias = true,
                        })
                        {
                            var amount = Math.Abs(input.Value - AbsoluteMinimum) / ValueRange;
                            canvas.DrawCircle(center.X, center.Y, radius * amount, paint);
                        }

                        canvas.DrawGradientLine(center, input.Color.WithAlpha(0), point, input.Color.WithAlpha((byte)(input.Color.Alpha * 0.75f)), LineSize);
                        canvas.DrawGradientLine(point, input.Color, nextPoint, nextInput.Color, LineSize);
                        canvas.DrawPoint(point, input.Color, PointSize, PointMode);

                        canvas.Restore();

                        // Labels
                        var labelPoint = new SKPoint(0, radius + LabelTextSize + (PointSize / 2));
                        var rotation = SKMatrix.MakeRotation(angle);
                        labelPoint = center + rotation.MapPoint(labelPoint);
                        var alignment = SKTextAlign.Left;

                        if ((Math.Abs(angle - (startAngle + Math.PI)) < Epsilon) || (Math.Abs(angle - Math.PI) < Epsilon))
                        {
                            alignment = SKTextAlign.Center;
                        }
                        else if (angle > (float)(startAngle + Math.PI))
                        {
                            alignment = SKTextAlign.Right;
                        }

                        canvas.DrawCaptionLabels(input.Label, input.TextColor, input.DisplayValue, input.Color.WithAlpha((byte)(255 * AnimationProgress)), LabelTextSize, labelPoint, alignment, base.Typeface);
                    }
                }
            }
        }

        private SKPoint GetPoint(float value, SKPoint center, float angle, float radius)
        {
            var amount = Math.Abs(value - AbsoluteMinimum) / ValueRange;
            var point = new SKPoint(0, radius * amount);
            var rotation = SKMatrix.MakeRotation(angle);
            return center + rotation.MapPoint(point);
        }

        private void DrawBorder(SKCanvas canvas, SKPoint center, float radius)
        {
            using (var paint = new SKPaint()
            {
                Style = SKPaintStyle.Stroke,
                StrokeWidth = BorderLineSize,
                Color = BorderLineColor,
                IsAntialias = true,
            })
            {
                canvas.DrawCircle(center.X, center.Y, radius, paint);
            }
        }
    }
}
