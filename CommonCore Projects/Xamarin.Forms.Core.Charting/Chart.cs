using SkiaSharp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms.Core.Charting.Utilities;
using Xamarin.Forms.Core.Charting.Extensions;
using Xamarin.Forms.Core.Charting.Helpers;

namespace Xamarin.Forms.Core.Charting
{
    public abstract class Chart : INotifyPropertyChanged
    {
        private IEnumerable<ChartInput> _Inputs;
        private float _AnimationProgress, _Margin = 20, _LabelTextSize = 16;
        private SKColor _BackgroundColor = SKColors.White;
        private SKColor _LabelColor = SKColors.Gray;
        private SKTypeface _Typeface;
        private float? _InternalMinValue, _InternalMaxValue;
        private bool _IsAnimated = true, _IsAnimating = false;
        private TimeSpan _AnimationDuration = TimeSpan.FromSeconds(1.5f);
        private Task _InvalidationPlanification;
        private CancellationTokenSource _AnimationCancellation;

        public Chart() { PropertyChanged += OnPropertyChanged; }

        public event PropertyChangedEventHandler PropertyChanged;
        public event EventHandler Invalidated;
        public bool IsAnimated
        {
            get => _IsAnimated;
            set
            {
                if (SetValue(ref _IsAnimated, value))
                {
                    if (!value)
                    {
                        AnimationProgress = 1;
                    }
                }
            }
        }
        public bool IsAnimating
        {
            get => _IsAnimating;
            private set => SetValue(ref _IsAnimating, value);
        }
        public TimeSpan AnimationDuration
        {
            get => _AnimationDuration;
            set => SetValue(ref _AnimationDuration, value);
        }
        public float Margin
        {
            get => _Margin;
            set => SetValue(ref _Margin, value);
        }
        public float AnimationProgress
        {
            get => _AnimationProgress;
            set
            {
                value = Math.Min(1, Math.Max(value, 0));
                SetValue(ref _AnimationProgress, value);
            }
        }
        public float LabelTextSize
        {
            get => _LabelTextSize;
            set => SetValue(ref _LabelTextSize, value);
        }
        public SKTypeface Typeface
        {
            get => _Typeface;
            set => SetValue(ref _Typeface, value);
        }
        public SKColor BackgroundColor
        {
            get => _BackgroundColor;
            set => SetValue(ref _BackgroundColor, value);
        }
        public SKColor LabelColor
        {
            get => _LabelColor;
            set => SetValue(ref _LabelColor, value);
        }
        public IEnumerable<ChartInput> Inputs
        {
            get => _Inputs;
            set => UpdateInputs(value);
        }
        public float MinValue
        {
            get
            {
                if (!Inputs.Any())
                {
                    return 0;
                }

                if (InternalMinValue == null)
                {
                    return Math.Min(0, Inputs.Min(x => x.Value));
                }

                return Math.Min(InternalMinValue.Value, Inputs.Min(x => x.Value));
            }

            set => InternalMinValue = value;
        }
        public float MaxValue
        {
            get
            {
                if (!Inputs.Any())
                {
                    return 0;
                }

                if (InternalMaxValue == null)
                {
                    return Math.Max(0, Inputs.Max(x => x.Value));
                }

                return Math.Max(InternalMaxValue.Value, Inputs.Max(x => x.Value));
            }

            set => InternalMaxValue = value;
        }
        protected float? InternalMinValue
        {
            get => _InternalMinValue;
            set
            {
                if (SetValue(ref _InternalMinValue, value))
                {
                    RaisePropertyChanged(nameof(MinValue));
                }
            }
        }
        protected float? InternalMaxValue
        {
            get => _InternalMaxValue;
            set
            {
                if (SetValue(ref _InternalMaxValue, value))
                {
                    RaisePropertyChanged(nameof(MaxValue));
                }
            }
        }
        public void Draw(SKCanvas canvas, int width, int height)
        {
            canvas.Clear(BackgroundColor);

            DrawContent(canvas, width, height);
        }
        public abstract void DrawContent(SKCanvas canvas, int width, int height);
        protected void DrawCaptionElements(SKCanvas canvas, int width, int height, List<ChartInput> inputs, bool isLeft)
        {
            var totalMargin = 2 * Margin;
            var availableHeight = height - (2 * totalMargin);
            var ySpace = (availableHeight - LabelTextSize) / ((inputs.Count <= 1) ? 1 : inputs.Count - 1);

            for (int i = 0; i < inputs.Count; i++)
            {
                var input = inputs.ElementAt(i);
                var y = totalMargin + (i * ySpace);
                if (inputs.Count <= 1)
                {
                    y += (availableHeight - LabelTextSize) / 2;
                }

                var hasLabel = !string.IsNullOrEmpty(input.Label);
                var hasValueLabel = !string.IsNullOrEmpty(input.DisplayValue);

                if (hasLabel || hasValueLabel)
                {
                    var captionMargin = LabelTextSize * 0.60f;
                    var captionX = isLeft ? Margin : width - Margin - LabelTextSize;
                    var valueColor = input.Color.WithAlpha((byte)(input.Color.Alpha * AnimationProgress));
                    var labelColor = input.TextColor.WithAlpha((byte)(input.TextColor.Alpha * AnimationProgress));

                    using (var paint = new SKPaint
                    {
                        Style = SKPaintStyle.Fill,
                        Color = valueColor
                    })
                    {
                        var rect = SKRect.Create(captionX, y, LabelTextSize, LabelTextSize);
                        canvas.DrawRect(rect, paint);
                    }

                    if (isLeft)
                    {
                        captionX += LabelTextSize + captionMargin;
                    }
                    else
                    {
                        captionX -= captionMargin;
                    }

                    canvas.DrawCaptionLabels(input.Label, labelColor, input.DisplayValue, valueColor, LabelTextSize, new SKPoint(captionX, y + (LabelTextSize / 2)), isLeft ? SKTextAlign.Left : SKTextAlign.Right, Typeface);
                }
            }
        }
        protected virtual void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(AnimationProgress):
                    Invalidate();
                    break;
                case nameof(LabelTextSize):
                case nameof(MaxValue):
                case nameof(MinValue):
                case nameof(BackgroundColor):
                    PlanifyInvalidate();
                    break;
                default:
                    break;
            }
        }
        protected void Invalidate() => Invalidated?.Invoke(this, EventArgs.Empty);
        protected async void PlanifyInvalidate()
        {
            if (_InvalidationPlanification != null)
            {
                await _InvalidationPlanification;
            }
            else
            {
                _InvalidationPlanification = Task.Delay(200);
                await _InvalidationPlanification;
                Invalidate();
                _InvalidationPlanification = null;
            }
        }

        public WeakEventHandler<TTarget> ObserveInvalidate<TTarget>(TTarget target, Action<TTarget> onInvalidate) where TTarget : class
        {
            var weakHandler = new WeakEventHandler<TTarget>(this, target, onInvalidate);
            weakHandler.Subscribe();
            return weakHandler;
        }

        public async Task AnimateAsync(bool entrance, CancellationToken token = default)
        {
            var watch = new Stopwatch();

            var start = entrance ? 0 : 1;
            var end = entrance ? 1 : 0;
            var range = end - start;

            AnimationProgress = start;
            IsAnimating = true;

            watch.Start();

            var source = new TaskCompletionSource<bool>();
            var timer = new IntervalTimer();

            timer.Start(TimeSpan.FromSeconds(1.0 / 30), () =>
            {
                if (token.IsCancellationRequested)
                {
                    source.SetCanceled();
                    return false;
                }

                var progress = (float)(watch.Elapsed.TotalSeconds / _AnimationDuration.TotalSeconds);
                progress = entrance ? EaseHelper.EaseIn(progress) : EaseHelper.EaseOut(progress);
                AnimationProgress = start + (progress * (end - start));

                var shouldContinue = (entrance && AnimationProgress < 1) || (!entrance && AnimationProgress > 0);

                if (!shouldContinue)
                {
                    source.SetResult(true);
                }

                return shouldContinue;
            });

            await source.Task;

            watch.Stop();
            IsAnimating = false;
        }

        private async void UpdateInputs(IEnumerable<ChartInput> value)
        {
            try
            {
                if (_AnimationCancellation != null)
                {
                    _AnimationCancellation.Cancel();
                }

                var cancellation = new CancellationTokenSource();
                _AnimationCancellation = cancellation;

                if (!cancellation.Token.IsCancellationRequested && _Inputs != null && IsAnimated)
                {
                    await AnimateAsync(false, cancellation.Token);
                }
                else
                {
                    AnimationProgress = 0;
                }

                if (SetValue(ref _Inputs, value))
                {
                    RaisePropertyChanged(nameof(MinValue));
                    RaisePropertyChanged(nameof(MaxValue));
                }

                if (!cancellation.Token.IsCancellationRequested && _Inputs != null && IsAnimated)
                {
                    await AnimateAsync(true, cancellation.Token);
                }
                else
                {
                    AnimationProgress = 1;
                }
            }
            catch
            {
                if (SetValue(ref _Inputs, value))
                {
                    RaisePropertyChanged(nameof(MinValue));
                    RaisePropertyChanged(nameof(MaxValue));
                }

                Invalidate();
            }
            finally
            {
                _AnimationCancellation = null;
            }
        }

        protected void RaisePropertyChanged([CallerMemberName]string property = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }

        protected bool SetValue<T>(ref T field, T value, [CallerMemberName]string property = null)
        {
            if (!Equals(field, property))
            {
                field = value;
                RaisePropertyChanged(property);
                return true;
            }

            return false;
        }
    }

    public enum PointMode
    {
        None,
        Circle,
        Square,
    }
}