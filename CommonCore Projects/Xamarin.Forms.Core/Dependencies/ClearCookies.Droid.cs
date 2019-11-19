#if __ANDROID__
using System;
using Android.Webkit;
using Xamarin.Forms;
using Xamarin.Forms.Core;

[assembly: Dependency(typeof(ClearCookies))]
namespace Xamarin.Forms.Core
{
    public class ClearCookies : IClearCookies
    {
        public void ClearAllCookies()
        {
            var cookieManager = CookieManager.Instance;
            cookieManager.RemoveAllCookie();
        }
    }
}
#endif
