#if __ANDROID__
using System;
using Android.App;
using Android.Content;
using Android.Gms.Common;
using Android.OS;
using Android.Util;
using Plugin.CurrentActivity;

namespace Xamarin.Forms.Core.AzurePush
{
    public class CoreAzurePush
    {
        private static string notificationChannelName = CoreSettings.Config.AzurePushSettings.NotificationChannelName;
        private static string debugTag = CoreSettings.Config.AzurePushSettings.DebugTag;

        public static Context Ctx
        {
            get
            {
                return CrossCurrentActivity.Current.Activity;
            }
        }

        public static void Init()
        {
            if (IsPlayServiceAvailable() == false)
            {
                throw new Exception("This device does not have Google Play Services and cannot receive push notifications.");
            }
            else
            {
                CreateNotificationChannel();
            }
        }

        private static bool IsPlayServiceAvailable()
        {
            int resultCode = GoogleApiAvailability.Instance.IsGooglePlayServicesAvailable(Ctx);
            if (resultCode != ConnectionResult.Success)
            {
                if (GoogleApiAvailability.Instance.IsUserResolvableError(resultCode))
                    Log.Debug(debugTag, GoogleApiAvailability.Instance.GetErrorString(resultCode));
                else
                {
                    Log.Debug(debugTag, "This device is not supported");
                }
                return false;
            }
            return true;
        }

        private static void CreateNotificationChannel()
        {
            // Notification channels are new as of "Oreo".
            // There is no need to create a notification channel on older versions of Android.
            if (Build.VERSION.SdkInt >= BuildVersionCodes.O)
            {
                var channelName = notificationChannelName;
                var channelDescription = String.Empty;
                var channel = new NotificationChannel(channelName, channelName, NotificationImportance.Default)
                {
                    Description = channelDescription
                };
            
                var notificationManager = (NotificationManager)Ctx.GetSystemService(Context.NotificationService);
                notificationManager.CreateNotificationChannel(channel);
            }
        }
    }
}
#endif
