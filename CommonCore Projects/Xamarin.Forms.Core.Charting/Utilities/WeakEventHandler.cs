using System;

namespace Xamarin.Forms.Core.Charting.Utilities
{
    public class WeakEventHandler<TTarget> : IDisposable where TTarget : class
    {
        private readonly WeakReference<Chart> SourceReference;
        private readonly WeakReference<TTarget> TargetReference;
        private readonly Action<TTarget> TargetMethod;
        private bool isSubscribed;

        public WeakEventHandler(Chart source, TTarget target, Action<TTarget> targetMethod)
        {
            SourceReference = new WeakReference<Chart>(source);
            TargetReference = new WeakReference<TTarget>(target);
            TargetMethod = targetMethod;
        }
        public bool IsAlive => SourceReference.TryGetTarget(out Chart s) && TargetReference.TryGetTarget(out TTarget t);
        public void Subscribe()
        {
            if (!isSubscribed && SourceReference.TryGetTarget(out Chart source))
            {
                source.Invalidated += OnEvent;
                isSubscribed = true;
            }
        }
        public void Unsubscribe()
        {
            if (isSubscribed)
            {
                if (SourceReference.TryGetTarget(out Chart source))
                {
                    source.Invalidated -= OnEvent;
                }

                isSubscribed = false;
            }
        }
        public void Dispose() => Unsubscribe();

        private void OnEvent(object sender, EventArgs args)
        {
            if (TargetReference.TryGetTarget(out TTarget t))
            {
                TargetMethod(t);
            }
            else
            {
                Unsubscribe();
            }
        }
    }
}
