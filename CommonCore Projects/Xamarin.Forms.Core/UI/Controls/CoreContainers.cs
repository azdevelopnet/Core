using System;
/*

Android MainActivity must implement
    Forms.SetFlags("FastRenderers_Experimental");

 - and -

    Nuget version of Xamarin.Forms 2.5 and above

Reduce native views created - Performance
Cut down on backing native views created for Xamarin.Forms, as noted by Miguel in #42948.

Layout Compression - Performance
LayoutCompression allows multiple layers of Xamarin.Forms layouts to be packed into a single native one.
https://blog.xamarin.com/3-big-things-explore-xamarin-forms-2-5-0-pre-release/
*/
namespace Xamarin.Forms.Core
{
    /// <summary>
    /// LayoutCompression allows multiple layers of Xamarin.Forms layouts to be packed into a single native one. 
    /// Not for UI rendered panels.
    /// </summary>
    public class StackContainer : StackLayout
    {
        public StackContainer(bool isCompressed = false)
        {
            if (isCompressed && CoreSettings.OS == DeviceOS.ANDROID)
            {
                this.SetValue(CompressedLayout.IsHeadlessProperty, true);
            }
        }
    }

    /// <summary>
    /// LayoutCompression allows multiple layers of Xamarin.Forms layouts to be packed into a single native one. 
    /// Not for UI rendered panels.
    /// </summary>
    public class AbsoluteContainer : AbsoluteLayout
    {
        public AbsoluteContainer(bool isCompressed = false)
        {
            if (isCompressed && CoreSettings.OS == DeviceOS.ANDROID)
            {
                this.SetValue(CompressedLayout.IsHeadlessProperty, true);
            }
        }
    }
    /// <summary>
    /// LayoutCompression allows multiple layers of Xamarin.Forms layouts to be packed into a single native one. 
    /// Not for UI rendered panels.
    /// </summary>
    public class GridContainer : Grid
    {
        public GridContainer(bool isCompressed = false)
        {
            if (isCompressed && CoreSettings.OS == DeviceOS.ANDROID)
            {
                this.SetValue(CompressedLayout.IsHeadlessProperty, true);
            }
        }
    }
    /// <summary>
    /// LayoutCompression allows multiple layers of Xamarin.Forms layouts to be packed into a single native one. 
    /// Not for UI rendered panels.
    /// </summary>
    public class RelativeContainer : RelativeLayout
    {
        public RelativeContainer(bool isCompressed = false)
        {
            if (isCompressed && CoreSettings.OS == DeviceOS.ANDROID)
            {
                this.SetValue(CompressedLayout.IsHeadlessProperty, true);
            }
        }
    }
}
