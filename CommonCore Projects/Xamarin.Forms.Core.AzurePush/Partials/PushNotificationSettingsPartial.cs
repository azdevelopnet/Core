﻿namespace Xamarin.Forms.Core
{
	public class AzurePushSettings
	{
		[JsonEncrypt]
		public string NotificationChannelName { get; set; }
		[JsonEncrypt]
		public string NotificationHubName { get; set; }
		[JsonEncrypt]
		public string ListenConnectionString { get; set; }
		[JsonEncrypt]
		public string DebugTag { get; set; }
		public string[] SubscriptionTags { get; set; }
		public string FCMTemplateBody { get; set; }
		public string APNTemplateBody { get; set; }
	}

	public partial class CoreConfiguration
	{
		public AzurePushSettings AzurePushSettings { get; set; }
	}

}
