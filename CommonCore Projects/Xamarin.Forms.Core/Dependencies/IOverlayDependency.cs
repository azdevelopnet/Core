using System;
using Xamarin.Forms;

namespace Xamarin.Forms.Core
{
    public interface IOverlayDependency
    {
        void ShowOverlay(string message);
        void ShowOverlay(string message, Color backgroundColor, float backgroundOpacity);
        void HideOverlay();
    }
}
