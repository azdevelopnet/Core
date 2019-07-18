using System;
namespace Xamarin.Forms.Core
{
    public interface IIntervalCallback
    {
        void TimeElapsedEvent();
    }
    public interface IBackgroundTimer
    {
        void Start(int minutes, IIntervalCallback formsCallBack);
        void Stop();
    }
    public class BackgroundTimer : IBackgroundTimer
    {
        public void Start(int minutes, IIntervalCallback formsCallBack)
        {
#if __ANDROID__
            if (TimerBackground.Instance == null)
                TimerBackground.Instance = new TimerBackground();

            TimerBackground.Instance.CallBack = formsCallBack;
            TimerBackground.Instance.IntervalMinutes = minutes;
            TimerBackground.Instance.Start();
            TimerBackground.Instance.RegisterAlarmManager();
#endif

#if __IOS__
            if (BackgroundTimerService.Instance == null)
                BackgroundTimerService.Instance = new BackgroundTimerService();

			BackgroundTimerService.Instance.CallBack = formsCallBack;
			BackgroundTimerService.Instance.IntervalMinutes = minutes;
            BackgroundTimerService.Instance.Start();


#endif

		}

		public void Stop()
		{

#if __ANDROID__
			TimerBackground.Instance.Stop();
            TimerBackground.Instance = null;
#endif

#if __IOS__
            BackgroundTimerService.Instance.Stop();
            BackgroundTimerService.Instance = null;
#endif

		}
	}
}
