#if __IOS__
using System;
using CoreTelephony;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Core;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(CoreTelephonyPage), typeof(CoreTelephonyPageRenderer))]
namespace Xamarin.Forms.Core
{
    public class TelephoneManager
    {
        public static string CallBackKey { get; set; }
        public static bool IsListening { get; set; }
    }
    public class CoreTelephonyPageRenderer : PageRenderer
    {
        private CTCallCenter callCenter;

        public void CallEndedEvent(CTCall call)
        {
            if (TelephoneManager.IsListening && call.CallState == "CTCallStateDisconnected")
            {
                TelephoneManager.IsListening = false;
                CoreDependencyService.SendViewModelMessage(TelephoneManager.CallBackKey, true);
            }

        }

        public override void ViewDidDisappear(bool animated)
        {
            base.ViewDidDisappear(animated);
            callCenter.CallEventHandler -= CallEndedEvent;
        }

        protected override void OnElementChanged(VisualElementChangedEventArgs e)
        {
            base.OnElementChanged(e);

            if (callCenter == null)
            {
                callCenter = new CTCallCenter();
                callCenter.CallEventHandler += CallEndedEvent;
            }
        }

    }
}
#endif
