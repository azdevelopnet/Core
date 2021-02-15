#if __IOS__
using System;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Core;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(CoreSearchBar), typeof(CoreSearchBarRenderer))]
namespace Xamarin.Forms.Core
{
    public class CoreSearchBarRenderer : SearchBarRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<SearchBar> e)
        {
            base.OnElementChanged(e);

            if (Control != null)
            {
                if (UIDevice.CurrentDevice.CheckSystemVersion(13, 0))
                    this.Control.SearchTextField.BackgroundColor = Xamarin.Forms.Color.White.ToUIColor();
            }
        }
    }
}
#endif
