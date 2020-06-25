#if __IOS__
using System;
using System.IO;
using CoreGraphics;
using Foundation;
using SkiaSharp;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Core;

[assembly: Dependency(typeof(Watermark))]
namespace Xamarin.Forms.Core
{
    public class Watermark: IWatermark
    {
        public byte[] CreateWatermark(byte[] byteArray, string[] content)
        {
            var _skImage = SKBitmap.Decode(byteArray);
            var w = _skImage.Width;
            var h = _skImage.Height;

            using (SKCanvas canvas = new SKCanvas(_skImage))
            {
                using (SKPaint textPaint = new SKPaint { TextSize = 48 })
                {
                    SKFontMetrics fontMetrics;
                    textPaint.GetFontMetrics(out fontMetrics);
                    var lineHeight = fontMetrics.Bottom - fontMetrics.Top;

                    textPaint.Style = SKPaintStyle.Stroke;
                    textPaint.Color = new SKColor(128, 0, 0);
                    textPaint.StrokeWidth = 2;

                    h -= 10;
                    w -= 10;
                    for (var x = content.Length - 1; x > -1; x--)
                    {
                        var txt = content[x];
                        SKRect bounds = new SKRect();
                        textPaint.MeasureText(txt, ref bounds);
                        canvas.DrawText(txt, (w - bounds.Width), h, textPaint);
                        h -= ((int)lineHeight + 5);
                    }
                }
            }
            return _skImage.Bytes;
        }

    }
}
#endif
