#if __ANDROID__
using System;
using Android.App;
using Android.Content;
using Plugin.CurrentActivity;
using Xamarin.Forms;
using Xamarin.Forms.Core;

[assembly: Dependency(typeof(CloseApplication))]
namespace Xamarin.Forms.Core
{
    public class CloseApplication: ICloseApplication
    {
        public Context Ctx
        {
            get => CrossCurrentActivity.Current.Activity;
        }

        public void Close()
        {
            var activity = (Activity)Ctx;
            activity.FinishAffinity();
        }
    }
}
#endif
