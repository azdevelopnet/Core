#if __IOS__
using System;
using Xamarin.Forms.Core;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(CoreContentView), typeof(CoreContentViewRenderer))]
namespace Xamarin.Forms.Core
{
    public class CoreContentViewRenderer : VisualElementRenderer<ContentView>
    {
        protected override void OnElementChanged(ElementChangedEventArgs<ContentView> e)
        {
            base.OnElementChanged(e);

            if (e.OldElement != null)
            {
                return;
            }

            Layer.CornerRadius = (nfloat)((CoreContentView)Element).CornerRadius;
        }


    }
}
#endif

