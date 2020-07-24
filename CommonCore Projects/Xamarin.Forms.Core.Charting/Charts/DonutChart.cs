using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms.Core.Charting.Helpers;

namespace Xamarin.Forms.Core.Charting
{
    public class DonutChart : Chart
    {
        public float HoleRadius { get; set; } = 0.5f;

        public override void DrawContent(SKCanvas canvas, int width, int height)
        {
            if (Inputs != null)
            {
                DrawCaption(canvas, width, height);
                using (new SKAutoCanvasRestore(canvas))
                {
                    canvas.Translate(width / 2, height / 2);
                    var sumValue = Inputs.Sum(x => Math.Abs(x.Value));
                    var radius = (Math.Min(width, height) - (2 * Margin)) / 2;

                    var start = 0.0f;
                    for (int i = 0; i < Inputs.Count(); i++)
                    {
                        var input = Inputs.ElementAt(i);
                        var end = start + ((Math.Abs(input.Value) / sumValue) * AnimationProgress);

                        // Sector
                        var path = RadialHelpers.CreateSectorPath(start, end, radius, radius * HoleRadius);
                        using (var paint = new SKPaint
                        {
                            Style = SKPaintStyle.Fill,
                            Color = input.Color,
                            IsAntialias = true,
                        })
                        {
                            canvas.DrawPath(path, paint);
                        }

                        start = end;
                    }
                }
            }
        }

        private void DrawCaption(SKCanvas canvas, int width, int height)
        {
            var sumValue = Inputs.Sum(x => Math.Abs(x.Value));
            var rightValues = new List<ChartInput>();
            var leftValues = new List<ChartInput>();

            int i = 0;
            var current = 0.0f;
            while (i < Inputs.Count() && (current < sumValue / 2))
            {
                var input = Inputs.ElementAt(i);
                rightValues.Add(input);
                current += Math.Abs(input.Value);
                i++;
            }

            while (i < Inputs.Count())
            {
                var input = Inputs.ElementAt(i);
                leftValues.Add(input);
                i++;
            }

            leftValues.Reverse();

            DrawCaptionElements(canvas, width, height, rightValues, false);
            DrawCaptionElements(canvas, width, height, leftValues, true);
        }
    }
}
