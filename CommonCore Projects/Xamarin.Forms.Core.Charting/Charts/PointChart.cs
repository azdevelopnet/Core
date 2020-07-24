using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms.Core.Charting.Enums;
using Xamarin.Forms.Core.Charting.Extensions;

namespace Xamarin.Forms.Core.Charting
{
    public class PointChart : Chart
    {
        public PointChart()
        {
            LabelOrientation = Orientation.Default;
            ValueLabelOrientation = Orientation.Default;
        }
        
        private Orientation labelOrientation, valueLabelOrientation;
        public float PointSize { get; set; } = 14;
        public PointMode PointMode { get; set; } = PointMode.Circle;
        public byte PointAreaAlpha { get; set; } = 100;
        public Orientation LabelOrientation
        {
            get => labelOrientation;
            set => labelOrientation = (value == Orientation.Default) ? Orientation.Vertical : value;
        }
        public Orientation ValueLabelOrientation
        {
            get => valueLabelOrientation;
            set => valueLabelOrientation = (value == Orientation.Default) ? Orientation.Vertical : value;
        }

        private float ValueRange => MaxValue - MinValue;

        public override void DrawContent(SKCanvas canvas, int width, int height)
        {
            if (Inputs != null)
            {
                var labels = Inputs.Select(x => x.Label).ToArray();
                var labelSizes = MeasureLabels(labels);
                var footerHeight = CalculateFooterHeaderHeight(labelSizes, LabelOrientation, labels);

                var valueLabels = Inputs.Select(x => x.DisplayValue).ToArray();
                var valueLabelSizes = MeasureLabels(valueLabels);
                var headerHeight = CalculateFooterHeaderHeight(valueLabelSizes, ValueLabelOrientation, valueLabels);

                var itemSize = CalculateItemSize(width, height, footerHeight, headerHeight);
                var origin = CalculateYOrigin(itemSize.Height, headerHeight);
                var points = CalculatePoints(itemSize, origin, headerHeight);

                DrawPointAreas(canvas, points, origin);
                DrawPoints(canvas, points);
                DrawHeader(canvas, valueLabels, valueLabelSizes, points, itemSize, height, headerHeight);
                DrawFooter(canvas, labels, labelSizes, points, itemSize, height, footerHeight);
            }
        }

        protected float CalculateYOrigin(float itemHeight, float headerHeight)
        {
            if (MaxValue <= 0)
            {
                return headerHeight;
            }

            if (MinValue > 0)
            {
                return headerHeight + itemHeight;
            }

            return headerHeight + ((MaxValue / ValueRange) * itemHeight);
        }

        protected SKSize CalculateItemSize(int width, int height, float footerHeight, float headerHeight)
        {
            var total = Inputs.Count();
            var w = (width - ((total + 1) * Margin)) / total;
            var h = height - Margin - footerHeight - headerHeight;
            return new SKSize(w, h);
        }

        protected SKPoint[] CalculatePoints(SKSize itemSize, float origin, float headerHeight)
        {
            var result = new List<SKPoint>();

            for (int i = 0; i < Inputs.Count(); i++)
            {
                var input = Inputs.ElementAt(i);
                var value = input.Value;

                var x = Margin + (itemSize.Width / 2) + (i * (itemSize.Width + Margin));
                var y = headerHeight + ((1 - AnimationProgress) * (origin - headerHeight) + (((MaxValue - value) / ValueRange) * itemSize.Height) * AnimationProgress);
                var point = new SKPoint(x, y);
                result.Add(point);
            }

            return result.ToArray();
        }

        protected void DrawHeader(SKCanvas canvas, string[] labels, SKRect[] labelSizes, SKPoint[] points, SKSize itemSize, int height, float headerHeight)
        {
            DrawLabels(canvas,
                            labels,
                            points.Select(p => new SKPoint(p.X, headerHeight - Margin)).ToArray(),
                            labelSizes,
                            Inputs.Select(x => x.Color.WithAlpha((byte)(255 * AnimationProgress))).ToArray(),
                            ValueLabelOrientation,
                            true,
                            itemSize,
                            height);
        }

        protected void DrawFooter(SKCanvas canvas, string[] labels, SKRect[] labelSizes, SKPoint[] points, SKSize itemSize, int height, float footerHeight)
        {
            DrawLabels(canvas,
                            labels,
                            points.Select(p => new SKPoint(p.X, height - footerHeight + Margin)).ToArray(),
                            labelSizes,
                            Inputs.Select(x => LabelColor).ToArray(),
                            LabelOrientation,
                            false,
                            itemSize,
                            height);
        }

        protected void DrawPoints(SKCanvas canvas, SKPoint[] points)
        {
            if (points.Length > 0 && PointMode != PointMode.None)
            {
                for (int i = 0; i < points.Length; i++)
                {
                    var input = Inputs.ElementAt(i);
                    var point = points[i];
                    canvas.DrawPoint(point, input.Color, PointSize, PointMode);
                }
            }
        }

        protected void DrawPointAreas(SKCanvas canvas, SKPoint[] points, float origin)
        {
            if (points.Length > 0 && PointAreaAlpha > 0)
            {
                for (int i = 0; i < points.Length; i++)
                {
                    var input = Inputs.ElementAt(i);
                    var point = points[i];
                    var y = Math.Min(origin, point.Y);

                    using (var shader = SKShader.CreateLinearGradient(new SKPoint(0, origin), new SKPoint(0, point.Y), new[] { input.Color.WithAlpha(PointAreaAlpha), input.Color.WithAlpha((byte)(PointAreaAlpha / 3)) }, null, SKShaderTileMode.Clamp))
                    using (var paint = new SKPaint
                    {
                        Style = SKPaintStyle.Fill,
                        Color = input.Color.WithAlpha(PointAreaAlpha),
                    })
                    {
                        paint.Shader = shader;
                        var height = Math.Max(2, Math.Abs(origin - point.Y));
                        canvas.DrawRect(SKRect.Create(point.X - (PointSize / 2), y, PointSize, height), paint);
                    }
                }
            }
        }

        protected void DrawLabels(SKCanvas canvas, string[] texts, SKPoint[] points, SKRect[] sizes, SKColor[] colors, Orientation orientation, bool isTop, SKSize itemSize, float height)
        {
            if (points.Length > 0)
            {
                var maxWidth = sizes.Max(x => x.Width);
                var avgHeightAdustment = 0d;

                if (isTop == false)
                {
                    avgHeightAdustment = sizes.Average(s => s.Height);
                }

                for (int i = 0; i < points.Length; i++)
                {
                    var input = Inputs.ElementAt(i);
                    var point = points[i];

                    if (!string.IsNullOrEmpty(texts[i]))
                    {
                        using (new SKAutoCanvasRestore(canvas))
                        {
                            using (var paint = new SKPaint())
                            {
                                paint.TextSize = LabelTextSize;
                                paint.IsAntialias = true;
                                paint.Color = colors[i];
                                paint.IsStroke = false;
                                paint.Typeface = base.Typeface;
                                var bounds = sizes[i];
                                var text = texts[i];

                                if (text != null)
                                {
                                    if (orientation == Orientation.Vertical)
                                    {
                                        var y = point.Y;
                                        if (isTop)
                                        {
                                            y -= bounds.Width;
                                        }

                                        canvas.RotateDegrees(90);
                                        canvas.Translate(y, -point.X + (bounds.Height / 2));
                                    }
                                    else
                                    {
                                        if (bounds.Width > itemSize.Width)
                                        {
                                            text = text.Substring(0, Math.Min(3, text.Length));
                                            paint.MeasureText(text, ref bounds);
                                        }

                                        if (bounds.Width > itemSize.Width)
                                        {
                                            text = text.Substring(0, Math.Min(1, text.Length));
                                            paint.MeasureText(text, ref bounds);
                                        }


                                        var y = point.Y;
                                        if (isTop)
                                        {
                                            y -= bounds.Height;
                                        }

                                        canvas.Translate(point.X - (bounds.Width / 2), y);
                                    }

                                    canvas.DrawText(text, 0, 0, paint);
                                }
                            }
                        }
                    }
                }
            }
        }
        
        protected float CalculateFooterHeaderHeight(SKRect[] valueLabelSizes, Orientation orientation, string[] labels)
        {
            var result = Margin;

            if (labels.Any(e => !string.IsNullOrEmpty(e)))
            {
                if (orientation == Orientation.Vertical)
                {
                    var maxValueWidth = valueLabelSizes.Max(x => x.Width);
                    if (maxValueWidth > 0)
                    {
                        result += maxValueWidth + Margin;
                    }
                }
                else
                {
                    result += LabelTextSize + Margin;
                }
            }

            return result;
        }

        protected SKRect[] MeasureLabels(string[] labels)
        {
            using (var paint = new SKPaint())
            {
                paint.TextSize = LabelTextSize;
                return labels.Select(text =>
                {
                    if (string.IsNullOrEmpty(text))
                    {
                        return SKRect.Empty;
                    }

                    var bounds = new SKRect();
                    paint.MeasureText(text, ref bounds);
                    return bounds;
                }).ToArray();
            }
        }
    }
}
