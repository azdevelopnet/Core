#if __ANDROID__
using System;
using Android;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Support.V4.App;
using Plugin.CurrentActivity;
using Xamarin.Forms.Core;
using App = Android.App;
using Content = Android.Content;

[assembly: Xamarin.Forms.Dependency(typeof(LocalNotify))]
namespace Xamarin.Forms.Core
{
    public class LocalNotify : ILocalNotify
    {
        public Context Ctx
        {
            get => CrossCurrentActivity.Current.Activity;
        }

        public Type MainType
        {
            get
            {
                return ((Activity)Ctx).GetType();
            }
        }

        public void Show(LocalNotification notification)
        {
            // When the user clicks the notification, SecondActivity will start up.
            Intent resultIntent = new Intent(Ctx, MainType);

            if (!string.IsNullOrEmpty(notification.MetaData))
            {
                Bundle valuesForActivity = new Bundle();
                valuesForActivity.PutString("metadata", notification.MetaData);
                resultIntent.PutExtras(valuesForActivity);
            }

            // Construct a back stack for cross-task navigation:
            var stackBuilder = App.TaskStackBuilder.Create(Ctx);

            stackBuilder.AddParentStack(Java.Lang.Class.FromType(MainType));
            stackBuilder.AddNextIntent(resultIntent);

            // Create the PendingIntent with the back stack:            
            PendingIntent resultPendingIntent =
                stackBuilder.GetPendingIntent(0, PendingIntentFlags.UpdateCurrent);

            int appIcon = GetResourceIdByName(notification.Icon);
            if (appIcon == 0)
                appIcon = CoreSettings.AppIcon;


            // Build the notification:

            NotificationCompat.Builder builder = new NotificationCompat.Builder(Ctx, "Local Notifications")
                .SetAutoCancel(true)                    // Dismiss from the notif. area when clicked
                .SetContentIntent(resultPendingIntent)  // Start 2nd activity when the intent is clicked.
                .SetContentTitle(notification.Title)      // Set its title
                                                          //.SetNumber(count)                       // Display the count in the Content Info
                .SetSmallIcon(appIcon)  // Display this icon
                .SetContentText(notification.Message); // The message to display.

            // Finally, publish the notification:
            NotificationManager notificationManager =
                (NotificationManager)Ctx.GetSystemService(Context.NotificationService);
            
            notificationManager.Notify(notification.Id, builder.Build());
        }

		public void RequestPermission(Action<bool> callBack)
		{
			callBack?.Invoke(true);
		}

        private int GetResourceIdByName(string name)
        {
            if (!string.IsNullOrEmpty(name))
            {
                name = name.Replace(".png", string.Empty).Replace(".jpg", string.Empty);
                return Ctx.Resources.GetIdentifier(name, "drawable", Ctx.PackageName);
            }
            else
            {
                return Ctx.PackageManager.GetApplicationInfo(Ctx.PackageName, Content.PM.PackageInfoFlags.MetaData).Icon;
            }
        }
    }
}
#endif
