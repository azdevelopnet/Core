#if __IOS__
using System;
using Foundation;
using UIKit;
using Xamarin.Forms.Core;

[assembly: Xamarin.Forms.Dependency(typeof(MapNavigate))]
namespace Xamarin.Forms.Core
{
    public class MapNavigate : IMapNavigate
    {
        public void NavigateWithAddress(string address)
        {
            address = System.Net.WebUtility.UrlEncode(address);
            NSUrl mapUrl = NSUrl.FromString(string.Format("http://maps.apple.com/?daddr={0}", address));
            UIApplication.SharedApplication.OpenUrl(mapUrl);
        }

        public void NavigateLatLong(double latitude, double longtitude)
        {

        }
    }
}
#endif
