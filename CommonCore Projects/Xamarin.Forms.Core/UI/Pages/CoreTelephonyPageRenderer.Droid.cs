#if __ANDROID__
using Android.Content;
using Xamarin.Forms;
using Xamarin.Forms.Core;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(CoreTelephonyPage), typeof(CoreTelephonyPageRenderer))]
namespace Xamarin.Forms.Core
{
    public class CoreTelephonyPageRenderer : PageRenderer
    {

        public CoreTelephonyPageRenderer(Context ctx):base(ctx)
        {

        }
    }
}
#endif
