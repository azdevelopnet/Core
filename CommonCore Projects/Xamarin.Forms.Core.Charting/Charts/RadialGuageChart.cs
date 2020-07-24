using SkiaSharp;
using System;
using System.Linq;
using System.Collections.Generic;
using Xamarin.Forms.Core.Charting.Extensions;

namespace Xamarin.Forms.Core.Charting
{
    public class RadialGuageChart : Chart
    {
        public float LineSize { get; set; } = -1;
        public byte LineAreaAlpha { get; set; } = 52;
        public float StartAngle { get; set; } = -90;
        private float AbsoluteMinimum => Inputs?.Select(x => x.Value).Concat(new[] { MaxValue, MinValue, InternalMinValue ?? 0 }).Min(x => Math.Abs(x)) ?? 0;
        private float AbsoluteMaximum => Inputs?.Select(x => x.Value).Concat(new[] { MaxValue, MinValue, InternalMinValue ?? 0 }).Max(x => Math.Abs(x)) ?? 0;
        private float ValueRange => AbsoluteMaximum - AbsoluteMinimum;

        public void DrawGaugeArea(SKCanvas canvas, ChartInput input, float radius, int cx, int cy, float strokeWidth)
        {
            using (var paint = new SKPaint
            {
                Style = SKPaintStyle.Stroke,
                StrokeWidth = strokeWidth,
                Color = input.Color.WithAlpha(LineAreaAlpha),
                IsAntialias = true,
            })
            {
                canvas.DrawCircle(cx, cy, radius, paint);
            }
        }

        public void DrawGauge(SKCanvas canvas, ChartInput input, float radius, int cx, int cy, float strokeWidth)
        {
            using (var paint = new SKPaint
            {
                Style = SKPaintStyle.Stroke,
                StrokeWidth = strokeWidth,
                StrokeCap = SKStrokeCap.Round,
                Color = input.Color,
                IsAntialias = true,
            })
            {
                using (SKPath path = new SKPath())
                {
                    var sweepAngle = AnimationProgress * 360 * (Math.Abs(input.Value) - AbsoluteMinimum) / ValueRange;
                    path.AddArc(SKRect.Create(cx - radius, cy - radius, 2 * radius, 2 * radius), StartAngle, sweepAngle);
                    canvas.DrawPath(path, paint);
                }
            }
        }

        public override void DrawContent(SKCanvas canvas, int width, int height)
        {
            if (Inputs != null)
            {
                DrawCaption(canvas, width, height);

                var sumValue = Inputs.Sum(x => Math.Abs(x.Value));
                var radius = (Math.Min(width, height) - (2 * Margin)) / 2;
                var cx = width / 2;
                var cy = height / 2;
                var lineWidth = (LineSize < 0) ? (radius / ((Inputs.Count() + 1) * 2)) : LineSize;
                var radiusSpace = lineWidth * 2;

                for (int i = 0; i < Inputs.Count(); i++)
                {
                    var input = Inputs.ElementAt(i);
                    var inputRadius = (i + 1) * radiusSpace;
                    DrawGaugeArea(canvas, input, inputRadius, cx, cy, lineWidth);
                    DrawGauge(canvas, input, inputRadius, cx, cy, lineWidth);
                }
            }
        }

        private void DrawCaption(SKCanvas canvas, int width, int height)
        {
            var rightValues = Inputs.Take(Inputs.Count() / 2).ToList<ChartInput>();
            var leftValues = Inputs.Skip(rightValues.Count()).ToList<ChartInput>();

            leftValues.Reverse();

            DrawCaptionElements(canvas, width, height, rightValues, false);
            DrawCaptionElements(canvas, width, height, leftValues, true);
        }
    }
}
