#if __ANDROID__
using Android.App;
using AndroidHUD;
using Xamarin.Forms.Core;
using Xamarin.Forms;
using Android.Content;
using Plugin.CurrentActivity;

[assembly: Xamarin.Forms.Dependency(typeof(ProgressIndicator))]
namespace Xamarin.Forms.Core
{
    public class ProgressIndicator : IProgressIndicator
    {
        
        public Context Ctx
        {
            get => CrossCurrentActivity.Current.Activity;
        }

        public void ShowProgress(string message)
        {
            AndHUD.Shared.Show(Ctx, message, (int)MaskType.Clear);
        }

        public void Dismiss()
        {
            AndHUD.Shared.Dismiss(Ctx);
        }

        public void ShowProgress(string message, double percentage)
        {
            AndHUD.Shared.Show(Ctx, message, (int)percentage, MaskType.Clear);
        }
    }
}
#endif

