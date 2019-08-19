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
        }

        public override void ViewWillAppear(bool animated)
        {
            if (TabBar?.Items == null) return;

            tabbedPage = (CoreTabbedPage)Element;
            var selectedColor = tabbedPage.SelectedForegroundColor.ToUIColor();
            var unselectedColor = tabbedPage.UnSelectedForegroundColor.ToUIColor();

            TabBar.BackgroundImage = new UIImage();
            TabBar.BackgroundColor = tabbedPage.TabBackgroundColor.ToUIColor();
            TabBar.SelectedImageTintColor = selectedColor;
            TabBar.UnselectedItemTintColor = unselectedColor;

            for (int i = 0; i < TabBar.Items.Length; i++)
            {
                UpdateItem(TabBar.Items[i], selectedColor, unselectedColor);
            }

            base.ViewWillAppear(animated);
        }

        private void UpdateItem(UITabBarItem item, UIColor selected, UIColor unselected)
        {
            if (item == null) return;

            item.SetTitleTextAttributes(new UITextAttributes
            {
                TextColor = selected
            }, UIControlState.Selected);

            item.SetTitleTextAttributes(new UITextAttributes
            {
                TextColor = unselected
            }, UIControlState.Normal);
        }

    }
}
#endif
