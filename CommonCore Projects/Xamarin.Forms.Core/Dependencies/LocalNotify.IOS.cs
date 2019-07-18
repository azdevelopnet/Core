#if __IOS__
using System;
using Xamarin.Forms.Core;
using Foundation;
using UserNotifications;

[assembly: Xamarin.Forms.Dependency(typeof(LocalNotify))]
namespace Xamarin.Forms.Core
{
	public class LocalNotify : ILocalNotify
	{
		public void Show(LocalNotification notification)
		{
			var content = new UNMutableNotificationContent();
			content.Title = notification.Title;
			content.Subtitle = notification.SubTitle;
			content.Body = notification.Message;
			if (!string.IsNullOrEmpty(notification.Sound))
			{
				content.Sound = UNNotificationSound.GetSound(notification.Sound);
			}
			if (notification.Badge.HasValue)
			{
				content.Badge = notification.Badge.Value;
			}

			var trigger = UNTimeIntervalNotificationTrigger.CreateTrigger(notification.SecondsOffSet, false);
			var requestID = notification.Id.ToString();
			var request = UNNotificationRequest.FromIdentifier(requestID, content, trigger);
			UNUserNotificationCenter.Current.AddNotificationRequest(request, (err) =>
			{
				if (err != null)
				{
					// Do something with error...
				}
			});
		}

		public void RequestPermission(Action<bool> callBack)
		{

			// Get current notification settings
			UNUserNotificationCenter.Current.GetNotificationSettings((settings) =>
			{
				UNUserNotificationCenter.Current.Delegate = new UserNotificationCenterDelegate();
				var alertsAllowed = (settings.AlertSetting == UNNotificationSetting.Enabled);
				if (!alertsAllowed)
				{
					// Request notification permissions from the user
					UNUserNotificationCenter.Current.RequestAuthorization(UNAuthorizationOptions.Alert, (approved, err) =>
					{
						callBack?.Invoke(approved);
					});
				}
				else
				{
					callBack?.Invoke(alertsAllowed);
				}
			});
		}
	}


	public class UserNotificationCenterDelegate : UNUserNotificationCenterDelegate
	{

		public override void WillPresentNotification(UNUserNotificationCenter center, UNNotification notification, Action<UNNotificationPresentationOptions> completionHandler)
		{

			Console.WriteLine("Active Notification: {0}", notification);

			completionHandler(UNNotificationPresentationOptions.Alert);
		}
	}
}
#endif
