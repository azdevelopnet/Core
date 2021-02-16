#if __IOS__
using System;
using System.Collections.Generic;
using Foundation;
using UserNotifications;
using Xamarin.Forms;

[assembly: Dependency(typeof(Xamarin.Forms.Core.NotificationManager))]
namespace Xamarin.Forms.Core
{
    public class NotificationManager : INotificationManager
    {
        int messageId = 0;
        public event EventHandler NotificationReceived;

        public NotificationManager()
        {
            UNUserNotificationCenter.Current.Delegate = new iOSNotificationReceiver();
        }

        public void SendNotification(string title, string message, DateTime? notifyTime = null)
        {
            UNUserNotificationCenter.Current.RequestAuthorization(UNAuthorizationOptions.Alert, (approved, err) =>
            {
                if (approved)
                {

                    messageId++;

                    var content = new UNMutableNotificationContent()
                    {
                        Title = title,
                        Subtitle = "",
                        Body = message,
                        Badge = 1
                    };

                    UNNotificationTrigger trigger;
                    if (notifyTime != null)
                    {
                        // Create a calendar-based trigger.
                        trigger = UNCalendarNotificationTrigger.CreateTrigger(GetNSDateComponents(notifyTime.Value), false);
                    }
                    else
                    {
                        // Create a time-based trigger, interval is in seconds and must be greater than 0.
                        trigger = UNTimeIntervalNotificationTrigger.CreateTrigger(0.25, false);
                    }

                    var request = UNNotificationRequest.FromIdentifier(messageId.ToString(), content, trigger);
                    UNUserNotificationCenter.Current.AddNotificationRequest(request, (err) =>
                    {
                        if (err != null)
                        {
                            throw new Exception($"Failed to schedule notification: {err}");
                        }
                    });
                }

            });
        }

        public void ReceiveNotification(string title, string message)
        {
            var args = new NotificationEventArgs()
            {
                Title = title,
                Message = message
            };
            NotificationReceived?.Invoke(null, args);
        }

        NSDateComponents GetNSDateComponents(DateTime dateTime)
        {
            return new NSDateComponents
            {
                Month = dateTime.Month,
                Day = dateTime.Day,
                Year = dateTime.Year,
                Hour = dateTime.Hour,
                Minute = dateTime.Minute,
                Second = dateTime.Second
            };
        }
    }

    public class iOSNotificationReceiver : UNUserNotificationCenterDelegate
    {
        public override void WillPresentNotification(UNUserNotificationCenter center, UNNotification notification, Action<UNNotificationPresentationOptions> completionHandler)
        {
            DependencyService.Get<INotificationManager>().ReceiveNotification(notification.Request.Content.Title, notification.Request.Content.Body);

            var dict = new Dictionary<string, string>();
            dict.Add("Title", notification.Request.Content.Title);
            dict.Add("Message", notification.Request.Content.Body);

            foreach(var key in notification.Request.Content.UserInfo.Keys)
            {
                dict.Add(key.ToString(), notification.Request.Content.UserInfo[key].ToString());
            }

            CoreDependencyService.SendViewModelMessage(CoreSettings.RemoteNotificationReceived, dict);

            // alerts are always shown for demonstration but this can be set to "None"
            // to avoid showing alerts if the app is in the foreground
            completionHandler(UNNotificationPresentationOptions.Alert);
        }
    }
}
#endif