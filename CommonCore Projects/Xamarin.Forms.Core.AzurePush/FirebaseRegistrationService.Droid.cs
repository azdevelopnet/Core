//#if __ANDROID__
//using System;
//using Android.App;
//using Android.Content;
//using Android.Util;
//using Firebase.Iid;
//using Plugin.CurrentActivity;
//using WindowsAzure.Messaging;

//namespace Xamarin.Forms.Core.AzurePush
//{
//    [Service]
//    [IntentFilter(new[] { "com.google.firebase.INSTANCE_ID_EVENT" })]
//    public class FirebaseRegistrationService : FirebaseInstanceIdService
//    {
//        private string debugTag = CoreSettings.Config.AzurePushSettings.DebugTag;

//        public Context Ctx
//        {
//            get
//            {
//                return CrossCurrentActivity.Current.Activity;
//            }
//        }

//        public override void OnTokenRefresh()
//        {
//            string token = FirebaseInstanceId.Instance.Token;

//            // NOTE: logging the token is not recommended in production but during
//            // development it is useful to test messages directly from Firebase
//            Log.Info(debugTag, $"Token received: {token}");

//            CoreSettings.DeviceToken = token;
//            CoreDependencyService.SendViewModelMessage(CoreSettings.TokenReceived, token);

//        }




//    }
//}
//#endif