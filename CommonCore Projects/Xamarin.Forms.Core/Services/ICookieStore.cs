using System;
using System.Collections.Generic;
using System.Net;

namespace Xamarin.Forms.Core.Services
{
    public interface ICookieStore
    {
        /// <summary>
        /// List of cookies pulled from the cookie storage manager
        /// on the device/platform you're on. Can be quite an expensive call.
        /// </summary>
        IEnumerable<Cookie> CurrentCookies { get; }

        /// <summary>
        /// Debug method, just lists all cookies in the <see cref="CurrentCookies"/> list
        /// </summary>
        //void DumpAllCookiesToLog();

        /// <summary>
        /// Clear cookies for site/url (otherwise auth tokens for your provider
        /// will hang around after a logout, which causes problems if you want
        /// to log in as someone else)
        /// </summary>
        void DeleteAllCookiesForSite(string url);
    }
}
