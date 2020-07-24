using SkiaSharp;
using SkiaSharp.Views.Forms;
using Xamarin.Forms;
using Xamarin.Forms.Core.Charting.Utilities;

namespace Xamarin.Forms.Core.Charting.Forms
{
    public class ChartView : SKCanvasView
    {
        public static readonly BindableProperty ChartProperty = BindableProperty.Create(nameof(Chart), typeof(Chart), typeof(ChartView), null, propertyChanged: OnChartChanged);
        public Chart Chart
        {
            get { return (Chart)GetValue(ChartProperty); }
            set { SetValue(ChartProperty, value); }
        }

        private WeakEventHandler<ChartView> _Handler;
        private Chart _Chart;

        public ChartView()
        {
            BackgroundColor = Color.Transparent;
            PaintSurface += OnPaintCanvas;
        }

        private static void OnChartChanged(BindableObject d, object oldValue, object value)
        {
            if (d is ChartView _this && value is Chart newChart)
            {
                if (_this._Chart != null)
                {
                    _this._Handler.Dispose();
                    _this._Handler = null;
                }

                _this._Chart = newChart;
                _this.InvalidateSurface();

                if (_this._Chart != null)
                {
                    _this._Handler = _this._Chart.ObserveInvalidate(_this, (v) => v.InvalidateSurface());
                }
            }
        }

        private void OnPaintCanvas(object sender, SKPaintSurfaceEventArgs e)
        {
            if (_Chart != null)
            {
                _Chart.Draw(e.Surface.Canvas, e.Info.Width, e.Info.Height);
            }
            else
            {
                e.Surface.Canvas.Clear(SKColors.Transparent);
            }
        }
    }
}
