#if __IOS__
using System;
using System.Threading;
using Xamarin.Forms;
using Xamarin.Forms.Core;

[assembly: Dependency(typeof(CloseApplication))]
namespace Xamarin.Forms.Core
{
    public class CloseApplication: ICloseApplication
    {
        public void Close()
        {
            Thread.CurrentThread.Abort();
        }
    }
}
#endif
