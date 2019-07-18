#if __IOS__
using System;
using CoreGraphics;
using Xamarin.Forms.Core;
using UIKit;
using Xamarin.Forms;

[assembly: Xamarin.Forms.Dependency(typeof(OverlayDependency))]
namespace Xamarin.Forms.Core
{
    public class LoadingOverlay : UIView
    {

        UIActivityIndicatorView activitySpinner;
        UILabel loadingLabel;

        public LoadingOverlay(CGRect frame, string loadingMessage, UIColor color, float opacity) : base(frame)
        {
            ShowOverlay(frame, loadingMessage, color, opacity);
        }
        public LoadingOverlay(CGRect frame, string loadingMessage) : base(frame)
        {
            ShowOverlay(frame, loadingMessage, UIColor.Black, 0.75f);
        }

        private void ShowOverlay(CGRect frame, string loadingMessage, UIColor color, float opacity)
        {
            // configurable bits
            BackgroundColor = color;
            Alpha = opacity;
            AutoresizingMask = UIViewAutoresizing.All;

            nfloat labelHeight = 22;
            nfloat labelWidth = Frame.Width - 20;

            // derive the center x and y
            nfloat centerX = Frame.Width / 2;
            nfloat centerY = Frame.Height / 2;

            // create the activity spinner, center it horizontall and put it 5 points above center x
            activitySpinner = new UIActivityIndicatorView(UIActivityIndicatorViewStyle.WhiteLarge);
            activitySpinner.Frame = new CGRect(
                centerX - (activitySpinner.Frame.Width / 2),
                centerY - activitySpinner.Frame.Height - 20,
                activitySpinner.Frame.Width,
                activitySpinner.Frame.Height);
            activitySpinner.AutoresizingMask = UIViewAutoresizing.All;

            AddSubview(activitySpinner);
            activitySpinner.StartAnimating();

            // create and configure the "Loading Data" label
            loadingLabel = new UILabel(new CGRect(
                centerX - (labelWidth / 2),
                centerY + 20,
                labelWidth,
                labelHeight
                ));
            loadingLabel.BackgroundColor = UIColor.Clear;
            loadingLabel.TextColor = UIColor.White;
            loadingLabel.Text = loadingMessage;
            loadingLabel.TextAlignment = UITextAlignment.Center;
            loadingLabel.AutoresizingMask = UIViewAutoresizing.All;

            AddSubview(loadingLabel);
        }

        /// <summary>
        /// Fades out the control and then removes it from the super view
        /// </summary>
        public void Hide()
        {
            UIView.Animate(
                0.5, // duration
                () => { Alpha = 0; },
                () => { RemoveFromSuperview(); }
            );
        }
    }

    public class OverlayDependency : IOverlayDependency
    {
        private LoadingOverlay overlay;

        public void HideOverlay()
        {
            overlay?.Hide();
        }

        public void ShowOverlay(string loadingMessage)
        {
            var controller = GetUIController();
            var rect = controller.View.Frame;
            overlay = new LoadingOverlay(rect, loadingMessage);
            controller.Add(overlay);
        }

        public void ShowOverlay(string loadingMessage, Color backgroundColor, float backgroundOpacity)
        {
            var controller = GetUIController();
            var rect = controller.View.Frame;
            overlay = new LoadingOverlay(rect, loadingMessage, backgroundColor.ToUIColor(), backgroundOpacity);
            controller.Add(overlay);
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

    public static class ColorExtensions
    {
        public static UIColor ToUIColor(this Xamarin.Forms.Color color)
        {
            return UIColor.FromRGBA(Convert.ToByte(color.R * 255), Convert.ToByte(color.G * 255), Convert.ToByte(color.B * 255), Convert.ToByte(color.A * 255));
        }
    }
}
#endif
