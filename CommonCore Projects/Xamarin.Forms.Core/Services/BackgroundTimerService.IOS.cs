#if __IOS__
using System;
using System.Threading;
using System.Threading.Tasks;
using UIKit;

namespace Xamarin.Forms.Core
{
    public class BackgroundTimerService
    {
        public int IntervalMinutes { get; set; } = 1;
        public IIntervalCallback CallBack { get; set; }
        public nint BackgroundId { get; set; }
        private CancellationTokenSource cancellation;

        public static BackgroundTimerService Instance = new BackgroundTimerService();

        public BackgroundTimerService()
        {
            this.cancellation = new CancellationTokenSource();
        }

        public void Start()
        {
            if (default(nint) == BackgroundId)
            {
                BackgroundId = UIApplication.SharedApplication.BeginBackgroundTask(() =>
                {
                    Console.WriteLine("Running out of time to complete you background task!");
                    UIApplication.SharedApplication.EndBackgroundTask(BackgroundId);
                });
                Task.Run(async () =>
                {
                    while (!cancellation.IsCancellationRequested)
                    {
                        this.CallBack?.TimeElapsedEvent();
                        await Task.Delay(new TimeSpan(0, 0, IntervalMinutes, 0, 0));
                    }

                    UIApplication.SharedApplication.EndBackgroundTask(BackgroundId);
                    BackgroundId = default(nint);
                });
            }
        }

        public void Stop()
        {
            this.cancellation.Cancel();
        }
    }
}

#endif
