#if __ANDROID__
using System;
using Android.App;
using Id = Android.Resource.Id;
using Xamarin.Forms.Platform.Android;
using Android.Widget;
using Android.Content;
using Plugin.CurrentActivity;
using Google.Android.Material.Snackbar;

[assembly: Xamarin.Forms.Dependency(typeof(Xamarin.Forms.Core.SnackBar))]
namespace Xamarin.Forms.Core
{
    public class SnackBar : ISnackBar
    {
        public static Snackbar Bar { get; set; }
        public Context Ctx
        {
            get => CrossCurrentActivity.Current.Activity;
        }
        public void Show(Snack snack)
        {
			if (SnackBar.Bar != null)
			{
				SnackBar.Bar.Dismiss();

			}
          
            var activity = (Activity)Ctx; 
            var view = activity.FindViewById(Id.Content);

            SnackBar.Bar = Snackbar.Make(view, snack.Text, Snackbar.LengthLong);

            if (snack.Duration == -1)
                SnackBar.Bar.SetDuration(Snackbar.LengthIndefinite);
            else
                SnackBar.Bar.SetDuration(snack.Duration);

            var snackbarView = SnackBar.Bar.View;
            snackbarView.SetBackgroundColor(snack.Background.ToAndroid());

            var snackbarId = GetResourceIdByName("snackbar_text");
			var textView = (TextView)snackbarView.FindViewById(snackbarId);
			textView.SetTextColor(snack.TextColor.ToAndroid());
           

			if (!string.IsNullOrEmpty(snack.ActionText))
			{
                SnackBar.Bar.SetAction(snack.ActionText, snack.Action);
                SnackBar.Bar.SetActionTextColor(snack.ActionTextColor.ToAndroid());
			}

			SnackBar.Bar.Show();

        }

        public void Close(){
			if (SnackBar.Bar != null)
			{
				SnackBar.Bar.Dismiss();

			}
        }


		private int GetResourceIdByName(string name)
		{
			return Ctx.Resources.GetIdentifier(name, "id", Ctx.PackageName);
		}
    }
}
#endif
