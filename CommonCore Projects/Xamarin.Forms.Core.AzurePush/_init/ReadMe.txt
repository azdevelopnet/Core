
Resource
    https://docs.microsoft.com/en-us/xamarin/xamarin-forms/data-cloud/push-notifications/azure-notification-hub


Required Nuget Installs
Android
    Xamarin.GooglePlayServices.Base
    Xamarin.Firebase.Messaging
    Xamarin.Azure.NotificationHubs.Android
IOS
    Xamarin.Azure.NotificationHubs.iOS

    

Android
  In the MainActivity of the OnCreate method add:
    CoreAzurePush.Init();

  Android Manifest
    Add the following to the <application> tag
        <receiver android:name="com.google.firebase.iid.FirebaseInstanceIdInternalReceiver" android:exported="false" />
        <receiver android:name="com.google.firebase.iid.FirebaseInstanceIdReceiver" android:exported="true" android:permission="com.google.android.c2dm.permission.SEND">
            <intent-filter>
                <action android:name="com.google.android.c2dm.intent.RECEIVE" />
                <action android:name="com.google.android.c2dm.intent.REGISTRATION" />
                <category android:name="${applicationId}" />
            </intent-filter>
        </receiver>



IOS Implement the following in the AppDelegate

        In the FinishedLaunching method add:
        CoreAzurePush.Init();

        public override void RegisteredForRemoteNotifications(UIApplication application, NSData deviceToken)
        {
            CoreAzurePush.RegisteredForRemoteNotifications(application, deviceToken);
        }
        public override void ReceivedRemoteNotification(UIApplication application, NSDictionary userInfo)
        {
            CoreAzurePush.ProcessNotification(userInfo, false);
        }