#if __ANDROID__
using System;
using Android.App;
using Android.Content;
using Android.Widget;
using Xamarin.Forms.Core;
using Xamarin.Forms;
using Net = Android.Net;
using Plugin.CurrentActivity;

[assembly: Xamarin.Forms.Dependency(typeof(MapNavigate))]
namespace Xamarin.Forms.Core
{
    public class MapNavigate : IMapNavigate
    {
        public Context Ctx
        {
            get => CrossCurrentActivity.Current.Activity;
        }

        public void NavigateWithAddress(string address)
        {
            try
            {
                var activity = (Activity)Ctx;
                address = System.Net.WebUtility.UrlEncode(address);
                var gmmIntentUri = Net.Uri.Parse("google.navigation:q=" + address);
                var mapIntent = new Intent(Intent.ActionView, gmmIntentUri);
                mapIntent.SetPackage("com.google.android.apps.maps");
                activity.StartActivity(mapIntent);
            }
            catch
            {
                Toast toast = Toast.MakeText(Ctx, "This activity is not supported", ToastLength.Long);
                toast.Show();
            }

        }

        public void NavigateLatLong(double latitude, double longtitude)
        {

        }
    }
}
#endif
