#if __ANDROID__
using System.IO;
using Android.Content;
using Xamarin.Forms;
using App = Android.App;
using Xamarin.Forms.Core;

[assembly: Dependency(typeof(BaseUrl))]
namespace Xamarin.Forms.Core
{
    public class BaseUrl : IBaseUrl
    {
        public string Get()
        {
            return "file:///android_asset/";
        }
        public string ReadContent(string fileName)
        {
            
            Context context = App.Application.Context;
        
            var assetManager = context.Assets;
            using (var streamReader = new StreamReader(assetManager.Open(fileName)))
            {
                var html = streamReader.ReadToEnd();
                return html;
            }
        }
    }
}
#endif
