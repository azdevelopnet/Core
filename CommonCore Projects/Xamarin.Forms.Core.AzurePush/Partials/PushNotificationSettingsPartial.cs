namespace Xamarin.Forms.Core
{
	public class AzurePushSettings
	{
		public string NotificationChannelName { get; set; } 
		public string NotificationHubName { get; set; } 
		public string ListenConnectionString { get; set; } 
		public string DebugTag { get; set; } 
		public string[] SubscriptionTags { get; set; } 
		public string FCMTemplateBody { get; set; } 
		public string APNTemplateBody { get; set; }
	}

	public partial class CoreConfiguration
	{
		public AzurePushSettings AzurePushSettings { get; set; }
	}

    public partial class CoreSettings
    {
        public const string RemoteNotificationReceived = "RemoteNotificationReceived";
        public const string TokenReceived = "TokenReceived";
    }
}
