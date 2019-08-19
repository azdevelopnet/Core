﻿using System;
using System.Collections.Generic;

namespace Xamarin.Forms.Core
{
	public partial class HttpSettings
	{
        public bool DisplayRawJson { get; set; } = false;
		public int HttpTimeOut { get; set; } = 0;
		public bool HttpAllowAutoRedirect { get; set; } = false;
		public string IOSHttpHandler { get; set; } = "Managed";
		public string AndroidHttpHandler { get; set; } = "Managed";
        public bool GZipEnabled { get; set; } = false;
    }
    public partial class SocialMedia
    {
        public GoogleSettings GoogleSettings { get; set; }
        public string FaceBookAppId { get; set; }
        public string MicrosoftAppId { get; set; }
    }

	public partial class GoogleSettings
	{
		public string GoogleAppId { get; set; }
	}

	public class MobileAppCenter
	{
		public string IOSAppId { get; set; }
		public string AndroidAppId { get; set; }
		public string UWPAppId { get; set; }
	}

	public partial class CoreConfiguration
	{
        public string AppLinkUrl { get; set; }
        public string AESEncryptionKey { get; set; }
		public HttpSettings HttpSettings { get; set; }
		public SocialMedia SocialMedia { get; set; }
        public MobileAppCenter MobileAppCenter { get; set; }
		public Dictionary<string,string> WebApi { get; set; }
		public Dictionary<string, string> CustomSettings { get; set; }
	}

}
