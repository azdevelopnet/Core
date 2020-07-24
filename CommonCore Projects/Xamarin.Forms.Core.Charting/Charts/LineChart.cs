using SkiaSharp;
using System.Linq;

namespace Xamarin.Forms.Core.Charting
{
    public class LineChart : PointChart
    {
        public LineChart()
        {
            PointSize = 10;
        }
        public float LineSize { get; set; } = 3;
        public LineMode LineMode { get; set; } = LineMode.Spline;
        public byte LineAreaAlpha { get; set; } = 32;
        public bool EnableYFadeOutGradient { get; set; } = false;
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

                DrawArea(canvas, points, itemSize, origin);
                DrawLine(canvas, points, itemSize);
                DrawPoints(canvas, points);
                DrawHeader(canvas, valueLabels, valueLabelSizes, points, itemSize, height, headerHeight);
                DrawFooter(canvas, labels, labelSizes, points, itemSize, height, footerHeight);
            }
        }

        protected void DrawLine(SKCanvas canvas, SKPoint[] points, SKSize itemSize)
        {
            if (points.Length > 1 && LineMode != LineMode.None)
            {
                using (var paint = new SKPaint
                {
                    Style = SKPaintStyle.Stroke,
                    Color = SKColors.White,
                    StrokeWidth = LineSize,
                    IsAntialias = true,
                })
                {
                    using (var shader = CreateXGradient(points))
                    {
                        paint.Shader = shader;

                        var path = new SKPath();

                        path.MoveTo(points.First());

                        var last = (LineMode == LineMode.Spline) ? points.Length - 1 : points.Length;
                        for (int i = 0; i < last; i++)
                        {
                            if (LineMode == LineMode.Spline)
                            {
                                var input = Inputs.ElementAt(i);
                                var nextInput = Inputs.ElementAt(i + 1);
                                var (point, control, nextPoint, nextControl) = CalculateCubicInfo(points, i, itemSize);
                                path.CubicTo(control, nextControl, nextPoint);
                            }
                            else if (LineMode == LineMode.Straight)
                            {
                                path.LineTo(points[i]);
                            }
                        }

                        canvas.DrawPath(path, paint);
                    }
                }
            }
        }

        protected void DrawArea(SKCanvas canvas, SKPoint[] points, SKSize itemSize, float origin)
        {
            if (LineAreaAlpha > 0 && points.Length > 1)
            {
                using (var paint = new SKPaint
                {
                    Style = SKPaintStyle.Fill,
                    Color = SKColors.White,
                    IsAntialias = true,
                })
                {
                    using (var shaderX = CreateXGradient(points, (byte)(LineAreaAlpha * AnimationProgress)))
                    using (var shaderY = CreateYGradient(points, (byte)(LineAreaAlpha * AnimationProgress)))
                    {
                        paint.Shader = EnableYFadeOutGradient ? SKShader.CreateCompose(shaderY, shaderX, SKBlendMode.SrcOut) : shaderX;

                        var path = new SKPath();

                        path.MoveTo(points.First().X, origin);
                        path.LineTo(points.First());

                        var last = (LineMode == LineMode.Spline) ? points.Length - 1 : points.Length;
                        for (int i = 0; i < last; i++)
                        {
                            if (LineMode == LineMode.Spline)
                            {
                                var input = Inputs.ElementAt(i);
                                var nextInput = Inputs.ElementAt(i + 1);
                                var (point, control, nextPoint, nextControl) = CalculateCubicInfo(points, i, itemSize);
                                path.CubicTo(control, nextControl, nextPoint);
                            }
                            else if (LineMode == LineMode.Straight)
                            {
                                path.LineTo(points[i]);
                            }
                        }

                        path.LineTo(points.Last().X, origin);

                        path.Close();

                        canvas.DrawPath(path, paint);
                    }
                }
            }
        }

        private (SKPoint point, SKPoint control, SKPoint nextPoint, SKPoint nextControl) CalculateCubicInfo(SKPoint[] points, int i, SKSize itemSize)
        {
            var point = points[i];
            var nextPoint = points[i + 1];
            var controlOffset = new SKPoint(itemSize.Width * 0.8f, 0);
            var currentControl = point + controlOffset;
            var nextControl = nextPoint - controlOffset;
            return (point, currentControl, nextPoint, nextControl);
        }

        private SKShader CreateXGradient(SKPoint[] points, byte alpha = 255)
        {
            var startX = points.First().X;
            var endX = points.Last().X;
            var rangeX = endX - startX;

            return SKShader.CreateLinearGradient(
                new SKPoint(startX, 0),
                new SKPoint(endX, 0),
                Inputs.Select(x => x.Color.WithAlpha(alpha)).ToArray(),
                null,
                SKShaderTileMode.Clamp);
        }

        private SKShader CreateYGradient(SKPoint[] points, byte alpha = 255)
        {
            var startY = points.Max(i => i.Y);
            var endY = 0;

            return SKShader.CreateLinearGradient(
                new SKPoint(0, startY),
                new SKPoint(0, endY),
                new SKColor[] { SKColors.White.WithAlpha(alpha), SKColors.White.WithAlpha(0) },
                null,
                SKShaderTileMode.Clamp);
        }
    }

    public enum LineMode
    {
        None,
        Spline,
        Straight,
    }
}
