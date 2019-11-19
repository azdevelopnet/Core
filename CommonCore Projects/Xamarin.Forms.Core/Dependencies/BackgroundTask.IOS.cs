#if __IOS__
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Foundation;
using Newtonsoft.Json;
using UIKit;
using Xamarin.Forms;
using System.Linq;
using Xamarin.Forms.Core;
using Xamarin.Essentials;

[assembly: Dependency(typeof(BackgroundTask))]
namespace Xamarin.Forms.Core
{
    public class BackgroundTask: IBackgroundTask
    {
        private Dictionary<string, BackgroundExecutionService> timers = new Dictionary<string, BackgroundExecutionService>();

        public static double Interval
        {
            get
            {
                return Preferences.Get("BackgroundInterval", UIApplication.BackgroundFetchIntervalMinimum);
            }
            set
            {
                Preferences.Set("BackgroundInterval", value);
            }
        }

        public void RegisterBackgroundProcess<T>() where T : ICoreJob, new()
        {
            using(var service = new BackgroundExecutionService())
            {
                service.Start(new T());
            }
        }

        public void RegisterTimerBackgroundProcess<T>(int repeatMins) where T : ICoreJob, new()
        {
            using (var service = new BackgroundExecutionService())
            {
                service.IntervalMinutes = repeatMins;
                service.Start(new T());
                timers.Add(typeof(T).Name, service);
            }
        }

        public void StopTimerBackgroundProcess<T>() where T : ICoreJob, new()
        {
            var key = typeof(T).Name;
            if (timers.ContainsKey(key))
            {
                timers[key].Stop();
            }
        }

        public void RegisterPeriodicBackgroundProcess<T>(int repeatMins, BackgroundTaskMetadata metaData) where T : ICoreJob, new()
        {
            if (HasBackgroundMode("fetch"))
            {
                BackgroundTask.Interval = repeatMins <= 15 ? UIApplication.BackgroundFetchIntervalMinimum : (repeatMins * 60);
                UIApplication.SharedApplication.SetMinimumBackgroundFetchInterval(BackgroundTask.Interval);

                var json = $"{typeof(T).Assembly.GetName().Name},{typeof(T).FullName}";

                var invoker = (ICoreJob)Activator.CreateInstance(typeof(T).Assembly.GetName().Name, typeof(T).FullName).Unwrap();
                new BackgroundExecutionService().Start(invoker);

                AddSetting(typeof(T).Name, json);
            }
        }

        public void StopPeriodicBackgroundProcess<T>() where T : ICoreJob, new()
        {
            RemoveSetting(typeof(T).Name);
        }


        public static void SetMinimumBackgroundFetchInterval()
        {
            UIApplication.SharedApplication.SetMinimumBackgroundFetchInterval(BackgroundTask.Interval);
        }


        public static void OnBackgroundFetch(Action<UIBackgroundFetchResult> completionHandler)
        {
            var dict = GetAllSettings();
            foreach (var obj in dict)
            {
                var details = obj.Data.Split(',');
                var invoker = (ICoreJob)Activator.CreateInstance(details[0], details[1]).Unwrap();
                using(var service = new BackgroundExecutionService())
                {
                    service.Start(invoker);
                }
              
            }
            completionHandler(UIBackgroundFetchResult.NoData);
        }


        private static bool HasBackgroundMode(string bgMode)
        {
            var info = NSBundle.MainBundle.InfoDictionary;
            var key = new NSString("UIBackgroundModes");
            var mode = new NSString(bgMode);

            if (info.ContainsKey(key))
            {
                var array = info[key] as NSArray;
                for (nuint i = 0; i < array.Count; i++)
                {
                    if (array.GetItem<NSString>(i) == mode)
                        return true;
                }
            }
            return false;
        }

        private static void RemoveSetting(string key)
        {
            List<ExecutionData> dict = GetAllSettings();
            var entry = dict.FirstOrDefault(x => x.Key == key);
            if (entry!=null)
            {
                dict.Remove(entry);
                Preferences.Set("backgroundList", JsonConvert.SerializeObject(dict));
            }
        }

        private static void AddSetting(string key, string data)
        {
            List<ExecutionData> dict = GetAllSettings();
            if (dict.Any(x => x.Key == key))
            {
                dict.First(x => x.Key == key).Data = data;
            }
            else
            {
                dict.Add(new ExecutionData() { Key = key, Data = data });
            }

            Preferences.Set("backgroundList", JsonConvert.SerializeObject(dict));
        }

        private static List<ExecutionData> GetAllSettings()
        {
            var jsonList = Preferences.Get("backgroundList", null);
            List<ExecutionData> dict = jsonList == null ? new List<ExecutionData>() : (List<ExecutionData>)JsonConvert.DeserializeObject<List<ExecutionData>>(jsonList);
            return dict;
        }

        internal class ExecutionData
        {
            public string Key { get; set; }
            public string Data { get; set; }
        }
        internal class BackgroundExecutionService: IDisposable
        {
            public int IntervalMinutes { get; set; } = -1;
            public nint BackgroundId { get; set; }
            private CancellationTokenSource cancellation;
            private CancellationToken token;
            public BackgroundExecutionService()
            {
                this.cancellation = new CancellationTokenSource();
                this.token = cancellation.Token;
            }

            public void Start(ICoreJob job)
            {
                if (default(nint) == BackgroundId)
                {
                    BackgroundId = UIApplication.SharedApplication.BeginBackgroundTask(() =>
                    {
                        Console.WriteLine("Running out of time to complete you background task!");
                        UIApplication.SharedApplication.EndBackgroundTask(BackgroundId);
                    });
                    Task.Run(async() =>
                    {
                        if (IntervalMinutes != -1)
                        {
                            while (!cancellation.IsCancellationRequested)
                            {
                                job.PerformWork();
                                await Task.Delay(new TimeSpan(0, 0, IntervalMinutes, 0, 0));
                            }
                        }
                        else
                        {
                            job.PerformWork();
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

            public void Dispose()
            {
                
            }
        }
    }
}
#endif
