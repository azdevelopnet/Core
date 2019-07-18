#if __ANDROID__
using System;
using System.Collections.Generic;
using System.Net;
using Android.Webkit;
using System.Linq;

namespace Xamarin.Forms.Core.Services
{
    public class CookieStore
    {
        private readonly string _url;
        private readonly object _refreshLock = new object();

        public IEnumerable<Cookie> CurrentCookies
        {
            get { return RefreshCookies(); }
        }

        public CookieStore(string url = "")
        {
            if (string.IsNullOrWhiteSpace(url))
            {
                throw new ArgumentNullException("url", "On Android, 'url' cannot be empty,         please provide a base URL for it to use when loading related cookies");
            }
            _url = url;
        }

        private IEnumerable<Cookie> RefreshCookies()
        {
            lock (_refreshLock)
            {
                // .GetCookie returns ALL cookies related to the URL as a single, long
                // string which we have to split and parse
                var allCookiesForUrl = CookieManager.Instance.GetCookie(_url);

                if (string.IsNullOrWhiteSpace(allCookiesForUrl))
                {
                    //LogDebug(string.Format("No cookies found for '{0}'. Exiting.", _url));
                    yield return new Cookie("none", "none");
                }
                else
                {
                    //LogDebug(string.Format("\r\n===== CookieHeader : '{0}'\r\n", allCookiesForUrl));

                    var cookiePairs = allCookiesForUrl.Split(' ');
                    foreach (var cookiePair in cookiePairs.Where(cp => cp.Contains("=")))
                    {
                        // yeah, I know, but this is a quick-and-dirty, remember? ;)
                        var cookiePieces = cookiePair.Split(new[] { '=' }, StringSplitOptions.RemoveEmptyEntries);
                        if (cookiePieces.Length >= 2)
                        {
                            cookiePieces[0] = cookiePieces[0].Contains(":")
                              ? cookiePieces[0].Substring(0, cookiePieces[0].IndexOf(":"))
                              : cookiePieces[0];

                            // strip off trailing ';' if it's there (some implementations
                            // on droid have it, some do not)
                            cookiePieces[1] = cookiePieces[1].EndsWith(";")
                              ? cookiePieces[1].Substring(0, cookiePieces[1].Length - 1)
                              : cookiePieces[1];

                            yield return new Cookie()
                            {
                                Name = cookiePieces[0],
                                Value = cookiePieces[1],
                                Path = "/",
                                Domain = new Uri(_url).DnsSafeHost,
                            };
                        }
                    }
                }
            }
        }

        //public void DumpAllCookiesToLog()
        //{
        //    // same as for iOS
        //}


        public void DeleteAllCookiesForSite(string url)
        {
            // TODO remove specific cookies by name...?
            // coz this may be a bit scorched-earth...
            CookieManager.Instance.RemoveAllCookie();
        }
    }
}
#endif
