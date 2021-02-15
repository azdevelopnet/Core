#if __ANDROID__
using System;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Gms.Common;
using Android.OS;
using Android.Util;
using Plugin.CurrentActivity;
using WindowsAzure.Messaging;
using AppNotificationManager = Android.App.NotificationManager;

namespace Xamarin.Forms.Core.AzurePush
{
    public class CoreAzurePush
    {
        static string fcmTemplateBody = CoreSettings.Config.AzurePushSettings.FCMTemplateBody;

        static string notificationChannelName = CoreSettings.Config.AzurePushSettings.NotificationChannelName;


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
                    Log.Debug(CoreSettings.Config.AzurePushSettings.DebugTag, GoogleApiAvailability.Instance.GetErrorString(resultCode));
                else
                {
                    Log.Debug(CoreSettings.Config.AzurePushSettings.DebugTag, "This device is not supported");
                }
                return false;
            }
            return true;
        }

        public static async Task RegisterNotificationHub(string[] tags)
        {
            await Task.Run(() =>
            {
                if (!string.IsNullOrEmpty(CoreSettings.DeviceToken))
                {
                    try
                    {
                        NotificationHub hub = new NotificationHub(CoreSettings.Config.AzurePushSettings.NotificationHubName, CoreSettings.Config.AzurePushSettings.ListenConnectionString, Ctx);

                        // register device with Azure Notification Hub using the token from FCM
                        Registration reg = hub.Register(CoreSettings.DeviceToken, tags);

                        // subscribe to the SubscriptionTags list with a simple template.
                        string pnsHandle = reg.PNSHandle;
                        var temp = hub.RegisterTemplate(pnsHandle, "defaultTemplate", fcmTemplateBody, tags);
                        CreateNotificationChannel();
                    }
                    catch (Exception e)
                    {
                        Log.Error(CoreSettings.Config.AzurePushSettings.DebugTag, $"Error registering device: {e.Message}");

                    }
                }
            });

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
            
                var notificationManager = (AppNotificationManager)Ctx.GetSystemService(Context.NotificationService);
                notificationManager.CreateNotificationChannel(channel);
            }
        }
    }
}
#endif
