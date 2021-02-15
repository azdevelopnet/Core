#if __ANDROID__
using System;
using Android.Content;
using Android.Graphics;
using Android.Text;
using Android.Widget;
using Xamarin.Forms;
using Xamarin.Forms.Core;
using DroidView = Android.Views;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(CoreSearchBar), typeof(CoreSearchBarRenderer))]
namespace Xamarin.Forms.Core
{
    public class CoreSearchBarRenderer : SearchBarRenderer
    {
        public CoreSearchBarRenderer(Context ctx) : base(ctx)
        {

        }

        protected override void OnElementChanged(ElementChangedEventArgs<SearchBar> e)
        {
            base.OnElementChanged(e);
            // Get native control (background set in shared code, but can use SetBackgroundColor here)
            SearchView searchView = (base.Control as SearchView);
            searchView.SetInputType(InputTypes.ClassText | InputTypes.TextVariationNormal);

            // Access search textview within control
            int textViewId = searchView.Context.Resources.GetIdentifier("android:id/search_src_text", null, null);
            EditText textView = (searchView.FindViewById(textViewId) as EditText);

            // Set custom colors
            //textView.SetBackgroundColor(Color.White.ToAndroid());
            textView.SetTextColor(Xamarin.Forms.Color.White.ToAndroid());
            textView.SetHintTextColor(Xamarin.Forms.Color.White.ToAndroid());

            // Customize frame color
            int frameId = searchView.Context.Resources.GetIdentifier("android:id/search_plate", null, null);
            DroidView.View frameView = (searchView.FindViewById(frameId) as DroidView.View);
            //frameView.SetBackgroundColor(Color.White.ToAndroid());

            var searchIconId = searchView.Resources.GetIdentifier("android:id/search_mag_icon", null, null);
            if (searchIconId > 0)
            {
                var searchPlateIcon = searchView.FindViewById(searchIconId);
                (searchPlateIcon as ImageView).SetColorFilter(Xamarin.Forms.Color.White.ToAndroid(), PorterDuff.Mode.SrcIn);
            }
            var searchCloseId = searchView.Resources.GetIdentifier("android:id/search_close_btn", null, null);
            if (searchCloseId > 0)
            {
                var searchCloseIcon = searchView.FindViewById(searchCloseId);
                (searchCloseIcon as ImageView).SetColorFilter(Xamarin.Forms.Color.White.ToAndroid(), PorterDuff.Mode.SrcIn);
            }
        }
    }
}
#endif
