#if __ANDROID__
using Android.App;
using Android.Content;
using Android.Util;
using Firebase.Iid;
using Plugin.CurrentActivity;
using System;
using WindowsAzure.Messaging;

namespace Xamarin.Forms.Core.AzurePush
{
    [Service]
    [IntentFilter(new [] { "com.google.firebase.INSTANCE_ID_EVENT"})]
    public class FirebaseRegistrationService : FirebaseInstanceIdService
    {
        string listenConnectionString = CoreSettings.Config.AzurePushSettings.ListenConnectionString;
        string NotificationHubName = CoreSettings.Config.AzurePushSettings.NotificationHubName;
        string debugTag = CoreSettings.Config.AzurePushSettings.DebugTag;
        string[] subscriptionTags = CoreSettings.Config.AzurePushSettings.SubscriptionTags;
        string fcmTemplateBody = CoreSettings.Config.AzurePushSettings.FCMTemplateBody;

        public Context Ctx
        {
            get
            {
                return CrossCurrentActivity.Current.Activity;
            }
        }

        public override void OnTokenRefresh()
        {
            string token = FirebaseInstanceId.Instance.Token;

            // NOTE: logging the token is not recommended in production but during
            // development it is useful to test messages directly from Firebase
            Log.Info(debugTag, $"Token received: {token}");

            SendRegistrationToServer(token);
        }

        void SendRegistrationToServer(string token)
        {
            try
            {
                NotificationHub hub = new NotificationHub(NotificationHubName, listenConnectionString, this);

                // register device with Azure Notification Hub using the token from FCM
                Registration reg = hub.Register(token, subscriptionTags);

                // subscribe to the SubscriptionTags list with a simple template.
                string pnsHandle = reg.PNSHandle;
                var cats = string.Join(", ", reg.Tags);
                var temp = hub.RegisterTemplate(pnsHandle, "defaultTemplate", fcmTemplateBody, subscriptionTags);
                CoreDependencyService.SendViewModelMessage(CoreSettings.TokenReceived, token);
            }
            catch(Exception e)
            {
                Log.Error(debugTag, $"Error registering device: {e.Message}");
            }
        }
    }
}
#endif