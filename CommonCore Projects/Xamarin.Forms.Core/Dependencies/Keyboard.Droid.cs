#if __ANDROID__
using System;
using Android.App;
using Android.Content;
using Android.Views.InputMethods;
using Plugin.CurrentActivity;
using Xamarin.Forms;
using Xamarin.Forms.Core;

[assembly: Xamarin.Forms.Dependency(typeof(KeyboardService))]
namespace Xamarin.Forms.Core
{
    public class KeyboardService : IKeyboardService
    {
        public Context Ctx
        {
            get => CrossCurrentActivity.Current.Activity;
        }

        public void Hide()
        {
            var inputMethodManager = Ctx.GetSystemService(Context.InputMethodService) as InputMethodManager;
            if (inputMethodManager != null && Ctx is Activity)
            {
                var activity = Ctx as Activity;
                var token = activity.CurrentFocus?.WindowToken;
                inputMethodManager.HideSoftInputFromWindow(token, HideSoftInputFlags.None);

                activity.Window.DecorView.ClearFocus();
            }
        }
    }
}
#endif
