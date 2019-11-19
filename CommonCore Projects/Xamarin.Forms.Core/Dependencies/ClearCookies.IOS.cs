#if __IOS__
using System;
using Foundation;
using Xamarin.Forms;
using Xamarin.Forms.Core;

[assembly: Dependency(typeof(ClearCookies))]
namespace Xamarin.Forms.Core
{
    public class ClearCookies : IClearCookies
    {
        public void ClearAllCookies()
        {
            NSHttpCookieStorage CookieStorage = NSHttpCookieStorage.SharedStorage;
            foreach (var cookie in CookieStorage.Cookies)
                CookieStorage.DeleteCookie(cookie);
        }
    }
}
#endif
