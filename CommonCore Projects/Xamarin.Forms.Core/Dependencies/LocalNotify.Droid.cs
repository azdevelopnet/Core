#if __ANDROID__
using System;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Support.V4.App;
using AndroidX.Core.App;
using Plugin.CurrentActivity;
using Xamarin.Forms.Core;
using Content = Android.Content;
using DroidColor = Android.Graphics.Color;
using NotificationCompatX = AndroidX.Core.App.NotificationCompat;

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
 
            Intent intent = new Intent(Intent.ActionView);
            //var AppId = NSBundle.MainBundle.BundleIdentifier;
            // intent.SetData(Android.Net.Uri.Parse("https://play.google.com/store/apps/details?id=" + AppId));
            //intent.SetData(Android.Net.Uri.Parse("market://details?id=" + AppId));
            const int _pendingIntentId = 0;
            PendingIntent oPendingIntent = PendingIntent.GetActivity(Ctx, _pendingIntentId, intent, PendingIntentFlags.OneShot);

            if (Build.VERSION.SdkInt >= BuildVersionCodes.O)
            {
                Notification.Builder oBuilder = new Notification.Builder(Ctx, "local_notification")
                     .SetContentIntent(oPendingIntent)
                     .SetContentTitle(notification.Title)
                     .SetContentText(notification.Message)
                     .SetSmallIcon(GetResourceIdByName(notification.Icon));

                oBuilder.SetColor(DroidColor.ParseColor("#f9cf00"));
                Notification oNotification = oBuilder.Build();
                NotificationManager oNotificationManager = NotificationManager.FromContext(Ctx);
                oNotificationManager.Notify(1000, oNotification);
            }
            else
            {
                NotificationCompatX.Builder _Builder = new NotificationCompatX.Builder(Ctx)
                         .SetContentIntent(oPendingIntent)
                         .SetContentTitle(notification.Title)
                         .SetContentText(notification.Message)
                         .SetSmallIcon(GetResourceIdByName(notification.Icon));
                _Builder.SetColor(DroidColor.ParseColor("#f9cf00"));
                Notification oNotification = _Builder.Build();
                NotificationManager oNotificationManager = NotificationManager.FromContext(Ctx);
                oNotificationManager.Notify(1000, oNotification);
            }
        }


		public void RequestPermission(Action<bool> callBack)
		{
            CreateNotificationChannel();
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

        private void CreateNotificationChannel()
        {
            try
            {
                if (Build.VERSION.SdkInt >= BuildVersionCodes.O)
                {
                    var channelName = "YourChannel";
                    var ChannelDescription = "Channel for notification";
                    var channel = new NotificationChannel("local_notification", channelName, NotificationImportance.Default)
                    {
                        Description = ChannelDescription
                    };
                    
                    var cNotificationManager = (NotificationManager)Ctx.GetSystemService(Context.NotificationService);
                    cNotificationManager.CreateNotificationChannel(channel);
                }
            }
            catch (Exception oExp)
            {

            }
        }
    }
}
#endif
