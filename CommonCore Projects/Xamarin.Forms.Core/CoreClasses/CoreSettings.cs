using System;
using System.Collections.Generic;
using System.Net;
using Newtonsoft.Json;
using System.Reflection;
using Xamarin.Essentials;
using System.IO;
using System.Linq;

#if __ANDROID__
using Android.Widget;
using App = Android.App;
using Plugin.CurrentActivity;
#endif
#if __IOS__
using Foundation;
using UIKit;
#endif

namespace Xamarin.Forms.Core
{

    public enum DeviceOS
    {
        IOS,
        ANDROID
    }
    public partial class CoreSettings
    {
        private static CoreConfiguration _config;

        public const string NetworkConnectivityChanged = "NetworkConnectivityChanged";

        public const string RemoteNotificationReceived = "RemoteNotificationReceived";

        public const string TokenReceived = "TokenReceived";

        public static string CurrentBuild { get; set; } = "dev";

        //public static INavigation AppNav { get; set; }

        public static string JsonEncryptionKey { get; set; }

        public static string Version
        {
            get
            {
                var num = typeof(CoreSettings).Assembly.GetName().Version;
                var buildVersion = "0.0.0";
                if (num.Build > 1000)
                {
                    var date = new DateTime(2000, 1, 1).AddDays(num.Build).AddSeconds(num.Revision * 2);
                    buildVersion = $"{date.Month}.{date.Day}.{date.ToString("HHmm")}";
                }
                return $"{CoreSettings.CurrentBuild} (v.{VersionTracking.CurrentBuild}.{buildVersion})";
            }
        }

        public static Size ScreenSize
        {
            get
            {

#if __ANDROID__

                var ctx = App.Application.Context;
                var height = (int) (ctx.Resources.DisplayMetrics.HeightPixels / ctx.Resources.DisplayMetrics.Density);
                var width = (int) (ctx.Resources.DisplayMetrics.WidthPixels / ctx.Resources.DisplayMetrics.Density);
                return new Size(width, height);
#endif
#if __IOS__
                var height = (int)UIScreen.MainScreen.Bounds.Height;
                var width = (int)UIScreen.MainScreen.Bounds.Width;
                return new Size(width, height);
#endif

            }
        }

        public static List<string> NotificationTags { get; set; } = new List<string>();

        public static T OnIdiom<T>(params T[] parameters)
        {
            if (Device.Idiom == TargetIdiom.Phone)
                return parameters[0];
            if (Device.Idiom == TargetIdiom.Tablet)
                return parameters[1];

            return parameters[0];
        }

        public static T On<T>(params T[] parameters)
        {
            T obj = default(T);

            switch (Device.RuntimePlatform.ToUpper())
            {
                case "IOS":
                    if (parameters.Length > 0)
                        obj = parameters[0];
                    break;
                case "ANDROID":
                    if (parameters.Length > 1)
                        obj = parameters[1];
                    break;
                default:
                    if (parameters.Length > 2)
                        obj = parameters[2];
                    break;
            }

            return obj;
        }

        public static DeviceOS OS
        {
            get
            {
#if __ANDROID__
                return DeviceOS.ANDROID;
#else
                return DeviceOS.IOS;
#endif
            }
        }

        public static CoreConfiguration Config
        {
            get { return _config; }
            set { _config = value; }
        }

        public static string InstallationId
        {
            get
            {
                
                var id = Preferences.Get("InstallationId", null);

                if (string.IsNullOrEmpty(id))
                {
                
                    id = Guid.NewGuid().ToString();
                     Preferences.Set("InstallationId", id);
                }

                return id;
            }
            set { Preferences.Set("InstallationId", value); }
        }

        public static long SyncTimeStamp
        {
            get
            {
                var id = Preferences.Get("SyncTimeStamp", 0L);

                if (id == default(long))
                {
                    id = DateTime.UtcNow.AddDays(-30).Ticks;
                    Preferences.Set("SyncTimeStamp", id);
                }

                return id;
            }
            set { Preferences.Set("SyncTimeStamp", value); }
        }

        public static bool HasDeviceToken
        {
            get
            {
#if __ANDROID__
                return !string.IsNullOrEmpty(DeviceToken);
#else
                return DeviceToken!=null;
#endif
            }
        }

#if __ANDROID__

        public static string DeviceToken
        {
            get
            {
                return Preferences.Get("DeviceToken", null);
            }
            set
            {
                Preferences.Set("DeviceToken", value);
            }
        }

        public static int AppIcon { get; set; }

        public static int SearchView { get; set; }
        
#endif

#if __IOS__
        public static NSData DeviceToken
        {
            get
            {
                var tokenString = Preferences.Get("DeviceToken", null);
                if(!string.IsNullOrEmpty(tokenString))
                    return new NSData(tokenString, NSDataBase64DecodingOptions.None);

                return null;
            }
            set
            {
                var tokenStringBase64 = value.GetBase64EncodedString(NSDataBase64EncodingOptions.None);
                Preferences.Set("DeviceToken", tokenStringBase64);
            }
        }

#endif

        public static void GlobalInit()
        {
            Load();
        }

        public static void GlobalInit(CoreConfiguration config)
        {
            Config = config;
        }

        public static void GlobalRefresh()
        {
            CoreDependencyService.DisposeAllServices();
            CoreDependencyService.ReleaseViewModelResources();
            Load();
            CoreDependencyService.InitViewModelResources();
        }

        public static void GlobalRefresh(CoreConfiguration config)
        {
            CoreDependencyService.DisposeAllServices();
            CoreDependencyService.ReleaseViewModelResources();
            Config = config;
            CoreDependencyService.InitViewModelResources();
        }

        private static void Load()
        {
            string fileName = null;
            fileName = $"config.{CoreSettings.CurrentBuild}.json";

            var response = ResourceLoader.GetEmbeddedResourceString(Assembly.GetAssembly(typeof(ResourceLoader)), fileName);
            if (response.Error == null)
            {
                try
                {
                    if (!string.IsNullOrEmpty(response.Response))
                    {
                        if (string.IsNullOrEmpty(JsonEncryptionKey))
                        {
                            Config = JsonConvert.DeserializeObject<CoreConfiguration>(response.Response);
                        }
                        else
                        {
                            Config = JsonConvert.DeserializeObject<CoreConfiguration>(response.Response, new JsonSerializerSettings()
                            {
                                ContractResolver = new EncryptedStringPropertyResolver(JsonEncryptionKey)
                            });
                        }
                    }
                }
                catch (Exception ex)
                {
                    ex.ConsoleWrite();
                }

            }
            else
            {
                response.Error?.ConsoleWrite();
            }

            if(Config==null)
                Config = new CoreConfiguration();
        }
    }

}
