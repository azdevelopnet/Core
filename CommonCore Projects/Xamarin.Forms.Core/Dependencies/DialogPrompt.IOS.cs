#if __IOS__
using System;
using System.Linq;
using System.Threading.Tasks;
using CoreGraphics;
using Foundation;
using UIKit;
using Xamarin.Forms.Core;

[assembly: Xamarin.Forms.Dependency(typeof(DialogPrompt))]
namespace Xamarin.Forms.Core
{
    public class DialogPrompt : IDialogPrompt
    {
        NSTimer alertDelay;
        UIAlertController alert;

        public void ShowMessage(Prompt prompt)
        {
            if (prompt.ButtonTitles == null || prompt.ButtonTitles.Length == 0)
                return;

            var controller = GetUIController();
            var alert = UIAlertController.Create(prompt.Title, prompt.Message, UIAlertControllerStyle.Alert);
            foreach (var txt in prompt.ButtonTitles)
            {
                alert.AddAction(UIAlertAction.Create(txt, UIAlertActionStyle.Default, action =>
                {
                    prompt.Callback?.Invoke(prompt.ButtonTitles.IndexOf(txt));
                }));
            }

            controller.PresentViewController(alert, true, null);
        }

        public void ShowActionSheet(string title, string subTitle, string[] list, Action<int> callBack, PromptMetaData metaData)
        {
            var controller = GetUIController();
            var alert = UIAlertController.Create(title, subTitle, UIAlertControllerStyle.ActionSheet);

            alert.View.TintColor = UIColor.Black;
            foreach (var obj in list)
            {
                alert.AddAction(UIAlertAction.Create(obj, UIAlertActionStyle.Default, (action) =>
                {
                    var index = list.ToList().IndexOf(action.Title);
                    callBack.Invoke(index);
                }));
            }
            alert.AddAction(UIAlertAction.Create("Cancel", UIAlertActionStyle.Cancel, (action) =>
            {
                callBack.Invoke(-1);
            }));

            var presentationPopover = alert.PopoverPresentationController;

            if (presentationPopover != null)
            {
                if (Device.Idiom == TargetIdiom.Tablet)
                {
                    if (metaData != null)
                    {
                        UIView ctrl = null;
                        if (metaData.Control != null)
                        {
                            ctrl = metaData.Control.ConvertFormsToNative();
                        }
                        else
                        {
                            ctrl = new UIView(new CGRect(
                                metaData.Rect.X,
                                metaData.Rect.Y,
                                metaData.Rect.Width,
                                metaData.Rect.Height));
                            GetTopWindow().Subviews[0].AddSubview(ctrl);
                        }

                        presentationPopover.SourceView = ctrl;
                        presentationPopover.PermittedArrowDirections = UIPopoverArrowDirection.Up;
                    }

                }
                else
                {
                    presentationPopover.SourceView = controller.View;
                    presentationPopover.PermittedArrowDirections = UIPopoverArrowDirection.Up;
                    presentationPopover.SourceRect = controller.View.Frame;
                }
            }
            controller.PresentViewController(alert, true, null);
        }
        public void ShowActionSheet(string title, string subTitle, string[] list, Action<int> callBack)
        {

            var controller = GetUIController();
            var alert = UIAlertController.Create(title, subTitle, UIAlertControllerStyle.ActionSheet);

            alert.View.TintColor = UIColor.Black;
            foreach (var obj in list)
            {
                alert.AddAction(UIAlertAction.Create(obj, UIAlertActionStyle.Default, (action) =>
                {
                    var index = list.ToList().IndexOf(action.Title);
                    callBack.Invoke(index);
                }));
            }
            alert.AddAction(UIAlertAction.Create("Cancel", UIAlertActionStyle.Cancel, (action) =>
            {
                callBack.Invoke(-1);
            }));

            var presentationPopover = alert.PopoverPresentationController;

            if (presentationPopover != null)
            {
                if (Device.Idiom == TargetIdiom.Tablet)
                {
                    presentationPopover.SourceView = GetTopWindow().Subviews.Last();
                    presentationPopover.PermittedArrowDirections = UIPopoverArrowDirection.Up;
                }
                else
                {
                    presentationPopover.SourceView = controller.View;
                    presentationPopover.PermittedArrowDirections = UIPopoverArrowDirection.Up;
                    presentationPopover.SourceRect = controller.View.Frame;
                }
            }
            controller.PresentViewController(alert, true, null);

        }

        private UIViewController GetUIController()
        {
            var win = UIApplication.SharedApplication.KeyWindow;
            var vc = win.RootViewController;
            while (vc.PresentedViewController != null)
                vc = vc.PresentedViewController;
            return vc;
        }
        private UIWindow GetTopWindow()
        {
            return UIApplication
                        .SharedApplication
                            .Windows
                            .Reverse()
                            .FirstOrDefault(x => x.WindowLevel == UIWindowLevel.Normal && !x.Hidden);
        }


        public void ShowToast(string message)
        {
            alertDelay = NSTimer.CreateScheduledTimer(3, (obj) =>
            {
                if (alert != null)
                {
                    alert.DismissViewController(true, null);
                }
                if (alertDelay != null)
                {
                    alertDelay.Dispose();
                }
            });
            alert = UIAlertController.Create(null, message, UIAlertControllerStyle.Alert);
            UIApplication.SharedApplication.KeyWindow.RootViewController.PresentViewController(alert, true, null);
        }
    }
}
#endif

