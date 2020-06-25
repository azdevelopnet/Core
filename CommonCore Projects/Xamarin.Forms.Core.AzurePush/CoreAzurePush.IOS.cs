#if __IOS__
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Foundation;
using UIKit;
using UserNotifications;
using WindowsAzure.Messaging;

namespace Xamarin.Forms.Core.AzurePush
{
    public class CoreAzurePush
    {
        private static SBNotificationHub Hub { get; set; }

        public static void Init()
		{
           
            // register for remote notifications based on system version
            if (UIDevice.CurrentDevice.CheckSystemVersion(10, 0))
            {
                UNUserNotificationCenter.Current.RequestAuthorization(UNAuthorizationOptions.Alert |
                    UNAuthorizationOptions.Sound |
                    UNAuthorizationOptions.Sound,
                    (granted, error) =>
                    {
                        if (granted)
                        {
                            Device.BeginInvokeOnMainThread(() => {
                                UIApplication.SharedApplication.RegisterForRemoteNotifications();
                            });
                        }
                           
                    });
            }
            else if (UIDevice.CurrentDevice.CheckSystemVersion(8, 0))
            {
                var pushSettings = UIUserNotificationSettings.GetSettingsForTypes(
                UIUserNotificationType.Alert | UIUserNotificationType.Badge | UIUserNotificationType.Sound,
                new NSSet());

                UIApplication.SharedApplication.RegisterUserNotificationSettings(pushSettings);
                UIApplication.SharedApplication.RegisterForRemoteNotifications();
            }
            else
            {
                UIRemoteNotificationType notificationTypes = UIRemoteNotificationType.Alert | UIRemoteNotificationType.Badge | UIRemoteNotificationType.Sound;
                UIApplication.SharedApplication.RegisterForRemoteNotificationTypes(notificationTypes);
            }

        }

        public static void RegisterNotificationHub(string[] tags)
        {

                if (CoreSettings.DeviceToken != null)
                {
                    var listenConnection = CoreSettings.Config.AzurePushSettings.ListenConnectionString;
                    var notificationName = CoreSettings.Config.AzurePushSettings.NotificationHubName;
                    var apnsTemplate = CoreSettings.Config.AzurePushSettings.APNTemplateBody;

                    Hub = new SBNotificationHub(listenConnection, notificationName);

                    Device.BeginInvokeOnMainThread(async() => {
                        try
                        {
                            await Hub.UnregisterAllAsync(CoreSettings.DeviceToken);
                            var nsTags = new NSSet(tags);
                            try
                            {
                                await Hub.RegisterNativeAsync(CoreSettings.DeviceToken, nsTags);
                            }
                            catch (Exception ex)
                            {
                                Debug.WriteLine($"RegisterNativeAsync error: {ex.Message}");
                            }
                            

                            var templateExpiration = DateTime.Now.AddDays(120).ToString(System.Globalization.CultureInfo.CreateSpecificCulture("en-US"));
                            try
                            {
                                await Hub.RegisterTemplateAsync(CoreSettings.DeviceToken, "defaultTemplate", apnsTemplate, templateExpiration, nsTags);
                            }
                            catch (Exception ex)
                            {
                                Debug.WriteLine($"RegisterTemplateAsync error: {ex.Message}");
                            }
                        }
                        catch (Exception ex)
                        {
                            Debug.WriteLine($"Unable to call unregister {ex.Message}");
                        }
                   
                    });

                }
         
        }

        public static void RegisteredForRemoteNotifications(UIApplication application, NSData deviceToken)
        {
            CoreSettings.DeviceToken = deviceToken;
            CoreDependencyService.SendViewModelMessage(CoreSettings.TokenReceived, deviceToken);
        }

        public static void ProcessNotification(NSDictionary options, bool fromFinishedLaunching)
        {
            // make sure we have a payload
            if (options != null && options.ContainsKey(new NSString("aps")))
            {
                // get the APS dictionary and extract message payload. Message JSON will be converted
                // into a NSDictionary so more complex payloads may require more processing
                NSDictionary aps = options.ObjectForKey(new NSString("aps")) as NSDictionary;
                string payload = string.Empty;
                NSString payloadKey = new NSString("alert");
                if (aps.ContainsKey(payloadKey))
                {
                    payload = aps[payloadKey].ToString();
                }

                if (!string.IsNullOrWhiteSpace(payload))
                {
                    CoreDependencyService.SendViewModelMessage(CoreSettings.RemoteNotificationReceived, payload);
                }

            }
            else
            {
                Debug.WriteLine($"Received request to process notification but there was no payload.");
            }
        }
    }
}
#endif
