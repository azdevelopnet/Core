# if __IOS__
using System.IO;
using Foundation;
using Xamarin.Forms;
using Xamarin.Forms.Core;

[assembly: Dependency(typeof(BaseUrl))]
namespace Xamarin.Forms.Core
{
    public class BaseUrl : IBaseUrl
    {
        public string Get()
        {
            return NSBundle.MainBundle.BundlePath;
        }

        public string ReadContent(string fileName)
        {
            string ext = Path.GetExtension(fileName);
            string filenameNoExt = fileName.Substring(0, fileName.Length - ext.Length);
            var resourcePathname = NSBundle.MainBundle.PathForResource(filenameNoExt, ext.Substring(1, ext.Length - 1));
            using (var streamReader = new StreamReader(resourcePathname))
            {
                var html = streamReader.ReadToEnd();
                return html;
            }
        }
    }
}
#endif
