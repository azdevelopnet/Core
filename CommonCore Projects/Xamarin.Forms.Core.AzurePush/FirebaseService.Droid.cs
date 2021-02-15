#if __ANDROID__
using System.Collections.Generic;
using System.Linq;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Support.V4.App;
using AndroidX.Core.App;
using AndroidX.Legacy.Content;
using Firebase.Messaging;

namespace Xamarin.Forms.Core.AzurePush
{
    [Service]
    [IntentFilter(new[] { "com.google.firebase.MESSAGING_EVENT" })]
    public class FirebaseService : FirebaseMessagingService
    {

        public override void OnNewToken(string token)
        {
            CoreSettings.DeviceToken = token;
            CoreDependencyService.SendViewModelMessage(CoreSettings.TokenReceived, token);

            base.OnNewToken(token);
        }
        public override void OnMessageReceived(RemoteMessage message)
        {
            var dict = new Dictionary<string, string>();
            base.OnMessageReceived(message);

            if (message.GetNotification() != null)
            {
                dict.Add("Title", message.GetNotification().Title);
                dict.Add("Message", message.GetNotification().Body);
                foreach(var key in message.Data.Keys)
                {
                    dict.Add(key, message.Data[key]);
                }
            }

            // NOTE: test messages sent via the Azure portal will be received here
            else
            {
                foreach (var key in message.Data.Keys)
                {
                    dict.Add(key, message.Data[key]);
                }
            }

            CoreDependencyService.SendViewModelMessage(CoreSettings.RemoteNotificationReceived, dict);

            CoreDependencyService.GetDependency<INotificationManager>().SendNotification(dict["Title"], dict["Message"]);


        }

    }
}
#endif