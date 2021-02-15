using System;
using Xamarin.Forms;

namespace Xamarin.Forms.Core
{
    public interface IOverlayService
    {
        void ShowOverlay(ContentPage page);

        void HideOverlay();
    }
}
