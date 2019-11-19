#if __IOS__
using System;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Core;

[assembly: Dependency(typeof(KeyboardHelper))]
namespace Xamarin.Forms.Core
{
    public class KeyboardHelper: IKeyboardHelper
    {
        public void HideKeyboard()
        {
            UIApplication.SharedApplication.KeyWindow.EndEditing(true);
        }
    }
}
#endif
