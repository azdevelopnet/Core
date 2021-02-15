using System;
using SkiaSharp;

namespace Xamarin.Forms.Core
{
    public interface IWatermark
    {
        byte[] CreateWatermark(byte[] byteArray, string[] content, SKColor color, int fontSize);
    }
}
