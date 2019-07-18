#if __ANDROID__
using System;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Plugin.CurrentActivity;

namespace Xamarin.Forms.Core
{
    public class TimerBackground
	{
		private TimerBackgroundingServiceConnection timerServiceConnection;
		private TimerBackgroundingReceiver timerReceiver;
        private AlarmManager alarm;
        private PendingIntent pendingServiceIntent;
		public bool IsBound { get; set; } = false;
        public int IntervalMinutes { get; set; } = 1;
		public TimerBackgroundingServiceBinder Binder { get; set; }
        public IIntervalCallback CallBack { get; set; }
		public Intent timerServiceIntent;

        public Context Ctx
        {
            get => CrossCurrentActivity.Current.Activity;
        }

		public static TimerBackground Instance = new TimerBackground();

		public TimerBackground()
		{
			timerServiceIntent = new Intent(Ctx, typeof(TimerBackgroundService));
			timerReceiver = new TimerBackgroundingReceiver();
		}
		public void Start()
		{
			var intentFilter = new IntentFilter(TimerBackgroundService.TimerUpdatedAction) { Priority = (int)IntentFilterPriority.HighPriority };
			Ctx.RegisterReceiver(timerReceiver, intentFilter);

			timerServiceConnection = new TimerBackgroundingServiceConnection();
			Ctx.BindService(timerServiceIntent, timerServiceConnection, Bind.AutoCreate);
		}

		public void Stop()
		{
			if (IsBound)
			{
                alarm.Cancel(pendingServiceIntent);
				Ctx.UnbindService(timerServiceConnection);
				IsBound = false;
			}

			Ctx.UnregisterReceiver(timerReceiver);
		}

		public void RegisterAlarmManager()
		{
			if (!IsAlarmSet())
			{
				alarm = (AlarmManager)Ctx.GetSystemService(Context.AlarmService);
				pendingServiceIntent = PendingIntent.GetService(Ctx, 0, timerServiceIntent, PendingIntentFlags.CancelCurrent);
                alarm.SetRepeating(AlarmType.Rtc, 0, (1000 * 60 * IntervalMinutes), pendingServiceIntent);
			}
		}
		bool IsAlarmSet()
		{
			return PendingIntent.GetBroadcast(Ctx, 0, timerServiceIntent, PendingIntentFlags.NoCreate) != null;
		}

		public void TimerElapsedEvent()
		{
			if (IsBound)
			{
				Task.Run(() =>
			   {
				   Binder.GetTimerService();
			   });

			}
		}
	}

	//[BroadcastReceiver]
	//[IntentFilter(new string[] { TimerBackgroundService.TimerUpdatedAction }, Priority = (int)IntentFilterPriority.LowPriority)]
	//public class TimerBackgroudNotificationReceiver : BroadcastReceiver
	//{

	//	public override void OnReceive(Context context, Intent intent)
	//	{
	//		var nMgr = (NotificationManager)context.GetSystemService(Context.NotificationService);
	//		var notification = new Notification(AppData.AppIcon, "Timer has fired");
	//		var pendingIntent = PendingIntent.GetActivity(context, 0, new Intent(context, LocalNotify.MainType), 0);
	//		notification.SetLatestEventInfo(context, "Timer fired", "AlarmManager event has been triggered", pendingIntent);
	//		nMgr.Notify(0, notification);
	//	}
	//}


	[Service]
	[IntentFilter(new String[] { "Xamarin.Forms.Core.TimerBackgroundService" })]
	public class TimerBackgroundService : IntentService
	{
		private IBinder binder;
		public const string TimerUpdatedAction = "TimerUpdatedAction";

        public static bool IsProcessing { get; set; }

		protected override void OnHandleIntent(Intent intent)
		{
			if (!TimerBackgroundService.IsProcessing)
			{
				TimerBackgroundService.IsProcessing = true;
                TimerElapsedEvent();
				var timerIntent = new Intent(TimerUpdatedAction);
				SendOrderedBroadcast(timerIntent, null);
				TimerBackgroundService.IsProcessing = false;
			}

		}

		public override IBinder OnBind(Intent intent)
		{
			binder = new TimerBackgroundingServiceBinder(this);
			return binder;
		}

		public void TimerElapsedEvent()
		{
            TimerBackground.Instance.CallBack?.TimeElapsedEvent();
		}

	}

	public class TimerBackgroundingServiceBinder : Binder
	{
		TimerBackgroundService service;

		public TimerBackgroundingServiceBinder(TimerBackgroundService service)
		{
			this.service = service;
		}

		public TimerBackgroundService GetTimerService()
		{
			return service;
		}
	}

	public class TimerBackgroundingReceiver : BroadcastReceiver
	{
        public override void OnReceive(Context context, Intent intent)
        {
            TimerBackground.Instance.TimerElapsedEvent();
            InvokeAbortBroadcast();
        }
	}

	public class TimerBackgroundingServiceConnection : Java.Lang.Object, IServiceConnection
	{

		public void OnServiceConnected(ComponentName name, IBinder service)
		{
			var timerServiceBinder = service as TimerBackgroundingServiceBinder;
			if (timerServiceBinder != null)
			{
				var binder = (TimerBackgroundingServiceBinder)service;
				TimerBackground.Instance.Binder = binder;
				TimerBackground.Instance.IsBound = true;
			}
		}

		public void OnServiceDisconnected(ComponentName name)
		{
			TimerBackground.Instance.IsBound = false;
		}
	}
}
#endif
