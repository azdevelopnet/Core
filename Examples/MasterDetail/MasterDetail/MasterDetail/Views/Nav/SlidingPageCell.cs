using System;
using FFImageLoading.Forms;
using MasterDetail.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Core;

namespace MasterDetail.Views.Nav
{
    public class SlidingPageCell : ViewCell
    {
        private readonly CachedImage img;
        private readonly Label lbl;

        public SlidingPageCell()
        {

            img = new CachedImage()
            {
                Margin = new Thickness(10, 0, 3, 5),
                HeightRequest = 22,
                WidthRequest = 22,
                DownsampleHeight = 22,
                DownsampleWidth = 22,
                Aspect = Aspect.AspectFit,
                CacheDuration = TimeSpan.FromDays(30),
                VerticalOptions = LayoutOptions.Center,
                DownsampleUseDipUnits = true
            };

            lbl = new Label()
            {
                Margin = 5,
                VerticalOptions = LayoutOptions.Center,
            };

            View = new StackContainer(true)
            {
                Orientation = StackOrientation.Horizontal,
                Children = { img, lbl }
            };
        }

        //On a listview that uses RecycleElement binding can be costly
        protected override void OnBindingContextChanged()
        {
            var item = (SlidingPageItem)BindingContext;
            img.Source = item.IconSource;
            lbl.Text = item.Title;

            base.OnBindingContextChanged();
        }
    }
}
