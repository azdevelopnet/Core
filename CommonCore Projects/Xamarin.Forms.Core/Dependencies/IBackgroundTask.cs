
namespace Xamarin.Forms.Core
{
    public interface ICoreJob
    {
        void PerformWork();
    }

    public class BackgroundTaskMetadata
    {
        public bool RequiresCharging { get; set; }
        public bool RequiresBatteryNotLow { get; set; }
        public string RequiresNetworkType { get; set; }
        public bool RequiresDeviceIdle { get; set; }
        public bool RequiresStorageNotLow { get; set; }
    }

    public interface IBackgroundTask
    {
        void RegisterPeriodicBackgroundProcess<T>(int repeatMins, BackgroundTaskMetadata metaData = null) where T : ICoreJob, new();
        void StopPeriodicBackgroundProcess<T>() where T : ICoreJob, new();
        void RegisterBackgroundProcess<T>() where T : ICoreJob, new();
        void RegisterTimerBackgroundProcess<T>(int repeatMins) where T : ICoreJob, new();
        void StopTimerBackgroundProcess<T>() where T : ICoreJob, new();
    }
}
