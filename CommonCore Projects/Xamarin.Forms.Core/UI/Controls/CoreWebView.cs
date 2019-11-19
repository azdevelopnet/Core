using System;
using System.ComponentModel;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Xamarin.Forms.Core
{
    public delegate void ScriptInvocation(string script);
    [DesignTimeVisible(true)]
    public class CoreWebView : WebView
    {

        public Action<string> Callback { get; set; }
        Action<string> javascriptAction;

        public static readonly BindableProperty UriProperty = BindableProperty.Create(
            propertyName: "Uri",
            returnType: typeof(string),
            declaringType: typeof(CoreWebView),
            defaultValue: default(string));

        public string Uri
        {
            get { return (string)GetValue(UriProperty); }
            set { SetValue(UriProperty, value); }
        }

        public static readonly BindableProperty IsLocalProperty = BindableProperty.Create(
            propertyName: "IsLocal",
            returnType: typeof(bool),
            declaringType: typeof(CoreWebView),
            defaultValue: false);

        public bool IsLocal
        {
            get { return (bool)GetValue(IsLocalProperty); }
            set { SetValue(IsLocalProperty, value); }
        }

        public void RegisterAction(Action<string> callback)
        {
            Callback = callback;
        }
        public void RegisterJavscriptAction(Action<string> callback)
        {
            javascriptAction = callback;
        }

        public void Cleanup()
        {
            Callback = null;
        }

        public void InvokeAction(string data)
        {
            if (Callback == null || data == null)
            {
                return;
            }
            Callback.Invoke(data);
        }

        public async Task InvokeJavascriptAction(string data)
        {
            if (Device.RuntimePlatform.ToUpper() == "IOS")
            {
                if (javascriptAction == null || data == null)
                {
                    return;
                }
                javascriptAction.Invoke(data);
            }
            else
            {
                Device.BeginInvokeOnMainThread(async() => {
                    await this.EvaluateJavaScriptAsync(data);
                });
                
            }
        }
    }
}
