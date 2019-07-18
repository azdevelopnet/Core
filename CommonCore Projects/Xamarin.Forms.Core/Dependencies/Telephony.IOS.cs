#if __IOS__
using System;
using Foundation;
using UIKit;
using Xamarin.Forms.Core;
using MessageUI;

[assembly: Xamarin.Forms.Dependency(typeof(Telephony))]
namespace Xamarin.Forms.Core
{
    public partial class Telephony : ITelephony
    {
    
        public void PlaceCallWithCallBack(string phoneNumber, string callBackKey)
        {
            TelephoneManager.CallBackKey = callBackKey;
            var currentNumber = CoreExtensions.ToNumericString(phoneNumber);

            if (UIApplication.SharedApplication.CanOpenUrl(new NSUrl("telprompt://" + currentNumber)))
            {
                TelephoneManager.IsListening = true;
                try
                {
                    UIApplication.SharedApplication.OpenUrl(new NSUrl("telprompt://" + currentNumber));
                }
                catch (Exception ex)
                {
                    var m = ex.Message;
                }
            }
            else
            {
                NotSupportedMessage("Phone Not Enabled", "This device does not support phone calls");

            }
        }

        private void NotSupportedMessage(string title, string message)
        {
            var alert = UIAlertController.Create(title, message, UIAlertControllerStyle.Alert);
            alert.AddAction(UIAlertAction.Create("Ok", UIAlertActionStyle.Default, null));
            GetUIController().PresentViewController(alert, true, null);
        }

        private UIViewController GetUIController()
        {
            var win = UIApplication.SharedApplication.KeyWindow;
            var vc = win.RootViewController;
            while (vc.PresentedViewController != null)
                vc = vc.PresentedViewController;
            return vc;
        }
    }

}
#endif
