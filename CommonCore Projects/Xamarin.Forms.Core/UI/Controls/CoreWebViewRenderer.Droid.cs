#if __ANDROID__
using Xamarin.Forms.Platform.Android;
using Android.Content;
using Android.Webkit;
using System;
using Java.Interop;
using Xamarin.Forms;
using Xamarin.Forms.Core;
using DroidView = Android.Webkit.WebView;

[assembly: ExportRenderer(typeof(CoreWebView), typeof(CoreWebViewRenderer))]
namespace Xamarin.Forms.Core
{
    public class CoreWebViewRenderer : WebViewRenderer
    {
        const string JavascriptFunction = "function invokeCSharpAction(data){jsBridge.invokeAction(data);}";


        public CoreWebViewRenderer(Context context) : base(context)
        {

        }

        protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.WebView> e)
        {
            base.OnElementChanged(e);


            if (e.OldElement != null)
            {
                Control.RemoveJavascriptInterface("jsBridge");
                var hybridWebView = e.OldElement as CoreWebView;
                hybridWebView.Cleanup();
            }
            if (e.NewElement != null)
            {
                var hybridWebView = Element as CoreWebView;

                Control.Settings.JavaScriptEnabled = true;
                Control.SetWebViewClient(new CoreJavascriptWebViewClient($"javascript: {JavascriptFunction}"));

                Control.AddJavascriptInterface(new CoreJSBridge(this), "jsBridge");

                if (Element.Source == null)
                {
                    if (hybridWebView.IsLocal)
                    {
                        Control.LoadUrl($"file:///android_asset/{hybridWebView.Uri}");
                    }
                    else
                    {
                        Control.LoadUrl(hybridWebView.Uri);
                    }
                }
                else
                {
                    var source = (HtmlWebViewSource)Element.Source;
                    Control.LoadDataWithBaseURL(source.BaseUrl, source.Html, "text/html; charset=utf-8", "UTF-8", null);
                }
            }

        }
    }

    public class CoreJavascriptWebViewClient : WebViewClient
    {
        string _javascript;

        public CoreJavascriptWebViewClient(string javascript)
        {
            _javascript = javascript;
        }

        public override void OnPageFinished(DroidView view, string url)
        {
            base.OnPageFinished(view, url);
            view.EvaluateJavascript(_javascript, null);
        }
    }


    public class CoreJSBridge : Java.Lang.Object
    {
        readonly WeakReference<CoreWebViewRenderer> hybridWebViewRenderer;

        public CoreJSBridge(CoreWebViewRenderer hybridRenderer)
        {
            hybridWebViewRenderer = new WeakReference<CoreWebViewRenderer>(hybridRenderer);
        }

        [JavascriptInterface]
        [Export("invokeAction")]
        public void InvokeAction(string data)
        {
            CoreWebViewRenderer hybridRenderer;

            if (hybridWebViewRenderer != null && hybridWebViewRenderer.TryGetTarget(out hybridRenderer))
            {
                ((CoreWebView)hybridRenderer.Element).InvokeAction(data);
            }
        }
    }
}
#endif
