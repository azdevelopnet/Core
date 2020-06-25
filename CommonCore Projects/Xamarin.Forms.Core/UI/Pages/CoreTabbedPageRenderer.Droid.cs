#if __ANDROID__
using System;
using System.ComponentModel;
using Android.Content.Res;
using Xamarin.Forms;
using Attribute = Android.Resource.Attribute;
using Xamarin.Forms.Core;
using Xamarin.Forms.Platform.Android;
using Xamarin.Forms.Platform.Android.AppCompat;
using Android.Content;
using Google.Android.Material.Tabs;
using DrawableX = AndroidX.Core.Graphics.Drawable;

[assembly: ExportRenderer(typeof(CoreTabbedPage), typeof(CoreTabbedPageRenderer))]
namespace Xamarin.Forms.Core
{
    public class CoreTabbedPageRenderer :TabbedPageRenderer
    {
		private bool setup;
		private TabLayout layout;
        private CoreTabbedPage tabbedPage;

        public CoreTabbedPageRenderer(Context ctx) : base(ctx)
        {

        }
        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			base.OnElementPropertyChanged(sender, e);

			if (setup)
				return;

			if (e.PropertyName == "Renderer")
			{
                tabbedPage = (CoreTabbedPage)Element;
                if (!tabbedPage.IsToolbarBottom)
                {
                    layout = (TabLayout)ViewGroup.GetChildAt(1);
                    setup = true;

                    ColorStateList colors = CreateColorState();

                    layout?.SetTabTextColors(tabbedPage.UnSelectedForegroundColor.ToAndroid(), tabbedPage.SelectedForegroundColor.ToAndroid());

                    if (tabbedPage.TabBackgroundColor != Color.Default)
                        layout?.SetBackgroundColor(tabbedPage.TabBackgroundColor.ToAndroid());

                    if (layout != null)
                    {
                        for (int i = 0; i < layout.TabCount; i++)
                        {
                            var tab = layout.GetTabAt(i);
                            var icon = tab.Icon;
                            if (icon != null)
                            {
                                icon = DrawableX.DrawableCompat.Wrap(icon);
                                DrawableX.DrawableCompat.SetTintList(icon, colors);
                                
                            }
                        }
                    }
                }



			}
		}
		private ColorStateList CreateColorState()
		{
			int[][] states = new int[][] {
				new int[] { Attribute.StateSelected }, // enabled
                new int[] { -Attribute.StateSelected } // disabled
			};
			int[] colors = new int[] {
               
				tabbedPage.SelectedForegroundColor.ToAndroid(),
                tabbedPage.UnSelectedForegroundColor.ToAndroid()
			};
			return new ColorStateList(states, colors);
		}
    }
}
#endif
