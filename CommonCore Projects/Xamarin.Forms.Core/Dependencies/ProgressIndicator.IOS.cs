#if __IOS__
using System;
using Xamarin.Forms.Core;
using UIKit;
using BigTed;

[assembly: Xamarin.Forms.Dependency(typeof(ProgressIndicator))]
namespace Xamarin.Forms.Core
{

    public class ProgressIndicator : IProgressIndicator
    {
        public static UIAlertController alert;
        public void ShowProgress(string message)
        {
            BTProgressHUD.Show(message, -1, ProgressHUD.MaskType.Black);
        }

        public void Dismiss()
        {
            BTProgressHUD.Dismiss();
        }

        public void ShowProgress(string message, double percentage)
        {
           BTProgressHUD.Show(message, (float)percentage,ProgressHUD.MaskType.Black);
        }

    }
}
#endif

