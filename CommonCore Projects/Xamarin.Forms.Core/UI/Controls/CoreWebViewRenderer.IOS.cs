#if __IOS__
using System;
using System.IO;
using Xamarin.Forms.Core;
using Foundation;
using UIKit;
using WebKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(CoreWebView), typeof(CoreWebViewRenderer))]
namespace Xamarin.Forms.Core
{
    public class CoreWebViewRenderer : ViewRenderer<CoreWebView, WKWebView>, IWKScriptMessageHandler
    {
        const string JavaScriptFunction = "function invokeCSharpAction(data){window.webkit.messageHandlers.invokeAction.postMessage(data);}";
        WKUserContentController userController;
        WKWebView webView;
        CoreWebView WebView => Element as CoreWebView;

        protected override void OnElementChanged(ElementChangedEventArgs<CoreWebView> e)
        {
            base.OnElementChanged(e);

            if (e.OldElement != null)
            {
                userController.RemoveAllUserScripts();
                userController.RemoveScriptMessageHandler("invokeAction");
                var hybridWebView = e.OldElement as CoreWebView;
                hybridWebView.Cleanup();
            }
            if (e.NewElement != null)
            {
                WebView.RegisterJavscriptAction((script) => {
                    Device.BeginInvokeOnMainThread(async() =>
                    {
                        await webView.EvaluateJavaScriptAsync(script);
                    });
                });
                if (Control == null)
                {
                    userController = new WKUserContentController();
                    var script = new WKUserScript(new NSString(JavaScriptFunction), WKUserScriptInjectionTime.AtDocumentEnd, false);
                    userController.AddUserScript(script);
                    userController.AddScriptMessageHandler(this, "invokeAction");

                    var config = new WKWebViewConfiguration { UserContentController = userController };
                    webView = new WKWebView(Frame, config);

                   
                    webView.UIDelegate = new CoreWebViewUIDelegate();
                    SetNativeControl(webView);
                }

                if (Element.Source == null)
                {
                    NSUrl uri = null;
                    if (Element.IsLocal)
                    {
                        string fileName = Path.Combine(NSBundle.MainBundle.BundlePath, Element.Uri);
                        uri = new NSUrl(fileName, false);
                    }
                    else
                    {
                        uri = new NSUrl(Element.Uri);
                    }

                    if (uri != null)
                        Control.LoadRequest(new NSUrlRequest(uri));
                }
                else
                {
                    var source = (HtmlWebViewSource)Element.Source;
                    Control.LoadHtmlString(source.Html, new NSUrl(source.BaseUrl, true));
                }
            }
        }

        public void DidReceiveScriptMessage(WKUserContentController userContentController, WKScriptMessage message)
        {
            Element.InvokeAction(message.Body.ToString());
        }

    }

    class CoreWebViewUIDelegate : WKUIDelegate
    {
        static string LocalOK = NSBundle.FromIdentifier("com.apple.UIKit").GetLocalizedString("OK");
        static string LocalCancel = NSBundle.FromIdentifier("com.apple.UIKit").GetLocalizedString("Cancel");

        public override void RunJavaScriptAlertPanel(WKWebView webView, string message, WKFrameInfo frame, Action completionHandler)
        {
            PresentAlertController(
                webView,
                message,
                okAction: _ => completionHandler()
            );
        }

        public override void RunJavaScriptConfirmPanel(WKWebView webView, string message, WKFrameInfo frame, Action<bool> completionHandler)
        {
            PresentAlertController(
                webView,
                message,
                okAction: _ => completionHandler(true),
                cancelAction: _ => completionHandler(false)
            );
        }

        public override void RunJavaScriptTextInputPanel(
            WKWebView webView, string prompt, string defaultText, WKFrameInfo frame, Action<string> completionHandler)
        {
            PresentAlertController(
                webView,
                prompt,
                defaultText: defaultText,
                okAction: x => completionHandler(x.TextFields[0].Text),
                cancelAction: _ => completionHandler(null)
            );
        }

        static string GetJsAlertTitle(WKWebView webView)
        {
            // Emulate the behavior of UIWebView dialogs.
            // The scheme and host are used unless local html content is what the webview is displaying,
            // in which case the bundle file name is used.

            if (webView.Url != null && webView.Url.AbsoluteString != $"file://{NSBundle.MainBundle.BundlePath}/")
                return $"{webView.Url.Scheme}://{webView.Url.Host}";

            return new NSString(NSBundle.MainBundle.BundlePath).LastPathComponent;
        }

        static UIAlertAction AddOkAction(UIAlertController controller, Action handler)
        {
            var action = UIAlertAction.Create(LocalOK, UIAlertActionStyle.Default, (_) => handler());
            controller.AddAction(action);
            controller.PreferredAction = action;
            return action;
        }

        static UIAlertAction AddCancelAction(UIAlertController controller, Action handler)
        {
            var action = UIAlertAction.Create(LocalCancel, UIAlertActionStyle.Cancel, (_) => handler());
            controller.AddAction(action);
            return action;
        }

        static void PresentAlertController(
            WKWebView webView,
            string message,
            string defaultText = null,
            Action<UIAlertController> okAction = null,
            Action<UIAlertController> cancelAction = null)
        {
            var controller = UIAlertController.Create(GetJsAlertTitle(webView), message, UIAlertControllerStyle.Alert);

            if (defaultText != null)
                controller.AddTextField((textField) => textField.Text = defaultText);

            if (okAction != null)
                AddOkAction(controller, () => okAction(controller));

            if (cancelAction != null)
                AddCancelAction(controller, () => cancelAction(controller));

            GetTopViewController(UIApplication.SharedApplication.KeyWindow.RootViewController)
                .PresentViewController(controller, true, null);
        }

        static UIViewController GetTopViewController(UIViewController viewController)
        {
            if (viewController is UINavigationController navigationController)
                return GetTopViewController(navigationController.VisibleViewController);

            if (viewController is UITabBarController tabBarController)
                return GetTopViewController(tabBarController.SelectedViewController);

            if (viewController.PresentedViewController != null)
                return GetTopViewController(viewController.PresentedViewController);

            return viewController;
        }
    }
}
#endif
