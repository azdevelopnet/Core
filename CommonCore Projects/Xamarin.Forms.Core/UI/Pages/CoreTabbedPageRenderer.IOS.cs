#if __IOS__
using System;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Core;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(CoreTabbedPage), typeof(CoreTabbedPageRenderer))]
namespace Xamarin.Forms.Core
{
    public class CoreTabbedPageRenderer: TabbedRenderer
    {
        CoreTabbedPage tabbedPage;
        protected override void OnElementChanged(VisualElementChangedEventArgs e)
        {
            base.OnElementChanged(e);

            tabbedPage = (CoreTabbedPage)Element;
            TabBar.BackgroundImage = new UIImage();
            TabBar.BackgroundColor = tabbedPage.TabBackgroundColor.ToUIColor();
            TabBar.SelectedImageTintColor = tabbedPage.SelectedForegroundColor.ToUIColor();
            TabBar.UnselectedItemTintColor =tabbedPage.UnSelectedForegroundColor.ToUIColor();

        }

    }
}
#endif
