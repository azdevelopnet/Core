#if __ANDROID__
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using AndroidX.Work;
using Plugin.CurrentActivity;
using Xamarin.Forms;
using Xamarin.Forms.Core;

[assembly: Dependency(typeof(BackgroundTask))]
namespace Xamarin.Forms.Core
{

    public class BackgroundTask : IBackgroundTask
    {
        private Dictionary<string, TimerBackground> timers { get; set; } = new Dictionary<string, TimerBackground>();
        private Dictionary<string, Java.Util.UUID> periodic { get; set; } = new Dictionary<string, Java.Util.UUID>();

        public void RegisterBackgroundProcess<T>() where T : ICoreJob, new()
        {
            var tag = typeof(T).Name;
            var data = new Data.Builder();
            data.Put("activatorDetails", $"{typeof(T).Assembly.GetName().Name},{typeof(T).FullName}");

            var otwr = OneTimeWorkRequest.Builder.From<BackgroundTaskWorker>()
                .SetInputData(data.Build())
                .AddTag(tag)
                .Build();

            WorkManager.Instance.EnqueueUniqueWork(tag, ExistingWorkPolicy.Keep, otwr);
        }

        public void RegisterPeriodicBackgroundProcess<T>(int repeatMins, BackgroundTaskMetadata metaData) where T : ICoreJob, new()
        {
            repeatMins = repeatMins >= 20 ? repeatMins : 20; //This is the minimum
            var tag = typeof(T).Name;
            var data = new Data.Builder();
            data.Put("activatorDetails", $"{typeof(T).Assembly.GetName().Name},{typeof(T).FullName}");

            var constraints = new Constraints.Builder();
            if (metaData!=null)
            {
                constraints.SetRequiresCharging(metaData.RequiresCharging);
                constraints.SetRequiresBatteryNotLow(metaData.RequiresBatteryNotLow);
                if (metaData.RequiresNetworkType != null)
                {
                    if(metaData.RequiresNetworkType== "Connected")
                        constraints.SetRequiredNetworkType(NetworkType.Connected);
                    if (metaData.RequiresNetworkType == "Metered")
                        constraints.SetRequiredNetworkType(NetworkType.Metered);
                    if (metaData.RequiresNetworkType == "NotRequired")
                        constraints.SetRequiredNetworkType(NetworkType.NotRequired);
                    if (metaData.RequiresNetworkType == "NotRoaming")
                        constraints.SetRequiredNetworkType(NetworkType.NotRoaming);
                    if (metaData.RequiresNetworkType == "Unmetered")
                        constraints.SetRequiredNetworkType(NetworkType.Unmetered);
                }
                constraints.SetRequiresDeviceIdle(metaData.RequiresDeviceIdle);
                constraints.SetRequiresStorageNotLow(metaData.RequiresStorageNotLow);
            }

            var pwr = PeriodicWorkRequest.Builder.From<BackgroundTaskWorker>(TimeSpan.FromMinutes(repeatMins))
                .SetConstraints(constraints.Build())
                .SetInputData(data.Build())
                .AddTag(tag)
                .Build();

            WorkManager.Instance.EnqueueUniquePeriodicWork(tag, ExistingPeriodicWorkPolicy.Keep, pwr);

            periodic.Add(tag, pwr.Id);
        }

        public void StopPeriodicBackgroundProcess<T>() where T : ICoreJob, new()
        {
            var tag = typeof(T).Name;
            if (periodic.ContainsKey(tag))
            {
                WorkManager.Instance.CancelWorkById(periodic[tag]);
            }
        }

        public void RegisterTimerBackgroundProcess<T>(int repeatMins) where T : ICoreJob, new()
        {
            var timerService = new TimerBackground()
            {
                Job = new T(),
                IntervalMinutes = repeatMins
            };
            timerService.Start();
            timers.Add(typeof(T).Name, timerService);
        }

        public void StopTimerBackgroundProcess<T>() where T : ICoreJob, new()
        {
            var key = typeof(T).Name;
            if (timers.ContainsKey(key))
            {
                timers[key].Stop();
            }
        }
    }

    internal class BackgroundTaskWorker : Worker
    {
        private ICoreJob job;
        public BackgroundTaskWorker(Context context, WorkerParameters workerParameters) : base(context, workerParameters)
        {
            var details = workerParameters.InputData.GetString("activatorDetails").Split(',');
            job = (ICoreJob)Activator.CreateInstance(details[0], details[1]).Unwrap();
        }
        public override Result DoWork()
        {
            job.PerformWork();
            return Result.InvokeSuccess();
        }

    }

    internal class TimerBackground
    {
        private TimerBackgroundingServiceConnection timerServiceConnection;
        private TimerBackgroundingReceiver timerReceiver;
        private AlarmManager alarm;
        private PendingIntent pendingServiceIntent;
        public bool IsBound { get; set; } = false;
        public int IntervalMinutes { get; set; } = 1;
        public TimerBackgroundingServiceBinder Binder { get; set; }
        public ICoreJob Job { get; set; }
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
            timerServiceIntent.PutExtra("jobType", $"{Job.GetType().Assembly.GetName().Name},{Job.GetType().FullName}");

            var intentFilter = new IntentFilter(TimerBackgroundService.TimerUpdatedAction) { Priority = (int)IntentFilterPriority.HighPriority };
            Ctx.RegisterReceiver(timerReceiver, intentFilter);

            timerServiceConnection = new TimerBackgroundingServiceConnection();
            Ctx.BindService(timerServiceIntent, timerServiceConnection, Bind.AutoCreate);

            RegisterAlarmManager();
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

        private void RegisterAlarmManager()
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

    [Service]
    [IntentFilter(new String[] { "Xamarin.Forms.Core.TimerBackgroundService" })]
    internal class TimerBackgroundService : IntentService
    {
        private ICoreJob job;
        private IBinder binder;
        public const string TimerUpdatedAction = "TimerUpdatedAction";

        public static bool IsProcessing { get; set; }

        protected override void OnHandleIntent(Intent intent)
        {
            var jobType = intent.GetStringExtra("jobType");
            var details = jobType.Split(',');
            job = (ICoreJob)Activator.CreateInstance(details[0], details[1]).Unwrap();

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
            job.PerformWork();
        }

    }

    internal class TimerBackgroundingServiceBinder : Binder
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

    internal class TimerBackgroundingReceiver : BroadcastReceiver
    {
        public override void OnReceive(Context context, Intent intent)
        {
            TimerBackground.Instance.TimerElapsedEvent();
            InvokeAbortBroadcast();
        }
    }

    internal class TimerBackgroundingServiceConnection : Java.Lang.Object, IServiceConnection
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
