#if __IOS__
using System;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Core;

[assembly: Xamarin.Forms.Dependency(typeof(KeyboardService))]
namespace Xamarin.Forms.Core
{
    public class KeyboardService : IKeyboardService
    {
        public void Hide()
        {
            UIApplication.SharedApplication.KeyWindow.EndEditing(true);
        }
    }
}
#endif
