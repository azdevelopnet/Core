
#if __IOS__
using System;
using UIKit;
using Xamarin.Forms.Core;

[assembly: Xamarin.Forms.Dependency(typeof(OpenSettings))]
namespace Xamarin.Forms.Core
{
    public class OpenSettings : IOpenSettings
    {
        public void Open()
        {
            UIApplication.SharedApplication.OpenUrl(new Foundation.NSUrl(UIApplication.OpenSettingsUrlString));
        }
    }
}
#endif
