#if __IOS__
using System;
using System.Collections.Generic;
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

                UNUserNotificationCenter.Current.Delegate = new iOSNotificationReceiver();
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

        public static async Task RegisterNotificationHub(string[] tags)
        {

            if (CoreSettings.DeviceToken != null)
            {
                var listenConnection = CoreSettings.Config.AzurePushSettings.ListenConnectionString;
                var notificationName = CoreSettings.Config.AzurePushSettings.NotificationHubName;
                var apnsTemplate = CoreSettings.Config.AzurePushSettings.APNTemplateBody;

                Hub = new SBNotificationHub(listenConnection, notificationName);

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



            }

        }

        public static void RegisteredForRemoteNotifications(UIApplication application, NSData deviceToken)
        {
            CoreSettings.DeviceToken = deviceToken;
            CoreDependencyService.SendViewModelMessage(CoreSettings.TokenReceived, deviceToken);
        }

        public static void ProcessNotification(NSDictionary options, bool fromFinishedLaunching)
        {
            if (options != null && options.ContainsKey(new NSString("aps")))
            {
                var dict = new Dictionary<string, string>();
                var aps = options.ObjectForKey(new NSString("aps")) as NSDictionary;
                var alertDict = aps.ObjectForKey(new NSString("alert")) as NSDictionary;

                var title = new NSString("title");
                var message = new NSString("messageParam");
                var metaData = new NSString("metaData");

                if (alertDict.ContainsKey(title))
                    dict.Add("Title", alertDict[title].ToString());
                if (alertDict.ContainsKey(title))
                    dict.Add("Message", alertDict[message].ToString());
                if (aps.ContainsKey(metaData))
                    dict.Add("MetaData", aps[metaData].ToString());
                
                if(dict.Count!=0)
                    CoreDependencyService.SendViewModelMessage(CoreSettings.RemoteNotificationReceived, dict);

            }
            else
            {
                Debug.WriteLine($"Received request to process notification but there was no payload.");
            }
        }
    }
}
#endif
