using SkiaSharp;

namespace Xamarin.Forms.Core.Charting
{
    public class ChartInput
    {
        public ChartInput(float value)
        {
            Value = value;
        }
        public float Value { get; }
        public string Label { get; set; }
        public string DisplayValue { get; set; }
        public SKColor Color { get; set; } = SKColors.Black;
        public SKColor TextColor { get; set; } = SKColors.Gray;
    }
}
