using System;
#if __IOS__
using UIKit;
using Foundation;
#else
using Android.App;
using Android.Content;
using Android.OS;
using Android.Telephony;
using Java.Net;
using Java.Util;
using Uri = Android.Net.Uri;
#endif
namespace Xamarin.Essentials
{
    public static partial class PhoneDialerHelper
    {
        public static bool CanOpen()
        {
#if __IOS__
            return UIApplication.SharedApplication.CanOpenUrl(new NSUrl(new Uri($"tel:1111111").AbsoluteUri));
#else
            var telUri = Uri.Parse("tel:1111111");
            var dialIntent = new Intent(Intent.ActionDial, telUri);
            return dialIntent.ResolveActivity(Application.Context.PackageManager) != null;
#endif
        }
    }
}
