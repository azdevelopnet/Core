#if __IOS__
using System;
using System.Collections.Generic;
using System.Net;
using Foundation;
using System.Linq;

namespace Xamarin.Forms.Core.Services
{
    public class CookieStore: ICookieStore
    {
        private readonly object _refreshLock = new object();

        public IEnumerable<Cookie> CurrentCookies
        {
            get { return RefreshCookies(); }
        }

        public CookieStore(string url = "")
        {
        }

        private IEnumerable<Cookie> RefreshCookies()
        {
            lock (_refreshLock)
            {
                foreach (var cookie in NSHttpCookieStorage.SharedStorage.Cookies)
                {
                    yield return new Cookie
                    {
                        Comment = cookie.Comment,
                        Domain = cookie.Domain,
                        HttpOnly = cookie.IsHttpOnly,
                        Name = cookie.Name,
                        Path = cookie.Path,
                        Secure = cookie.IsSecure,
                        Value = cookie.Value,
                        /// TODO expires? / expired?
                        Version = Convert.ToInt32(cookie.Version)
                    };
                }
            }
        }

        //public void DumpAllCookiesToLog()
        //{
        //    if (!CurrentCookies.Any())
        //    {
        //        //LogDebug("No cookies in your iOS cookie store. Srsly? No cookies? At all?!?");
        //    }
        //    CurrentCookies.ToList()
        //                  .ForEach(cookie =>
        //                          LogDebug(string.Format("Cookie dump: {0} = {1}",
        //                                                  cookie.Name,
        //                                                  cookie.Value)));
        //}

        public void DeleteAllCookiesForSite(string url)
        {
            var cookieStorage = NSHttpCookieStorage.SharedStorage;
            foreach (NSHttpCookie cookie in cookieStorage.CookiesForUrl(new NSUrl(url)).ToList())
            {
                cookieStorage.DeleteCookie(cookie);
            }
            // you MUST call the .Sync method or those cookies may hang around for a bit
            NSUserDefaults.StandardUserDefaults.Synchronize();
        }
    }
}
#endif
