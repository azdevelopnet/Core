using System;
using Xamarin.Forms;

namespace Xamarin.Forms.Core
{
    public interface IWatermark
    {
        byte[] CreateWatermark(byte[] byteArray, string[] content);
    }
}
