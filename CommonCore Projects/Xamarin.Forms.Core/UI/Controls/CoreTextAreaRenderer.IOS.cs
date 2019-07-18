#if __IOS__
using System;
using Xamarin.Forms.Core;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(CoreTextArea), typeof(CoreTextAreaRenderer))]
namespace Xamarin.Forms.Core
{
    public class CoreTextAreaRenderer : ViewRenderer<CoreTextArea, UITextView>
    {
        private UITextView txtView;
        private CoreTextArea parent;

        protected override void OnElementChanged(ElementChangedEventArgs<CoreTextArea> e)
        {
            base.OnElementChanged(e);
            if (e.NewElement != null)
                parent = e.NewElement;

            if (txtView == null)
            {
                txtView = new UITextView();
                txtView.Text = parent.Text;
                if(!string.IsNullOrEmpty(parent.FontFamily))
                    txtView.Font = UIFont.FromName(parent.FontFamily, (nfloat)parent.FontSize);

                var txtColor = UIColor.Black;
                if ((int)parent.TextColor.R != -1)
                    txtColor = parent.TextColor.ToUIColor();

                txtView.TextColor = txtColor;
                txtView.Editable = false;
                txtView.ScrollEnabled = false;

                if (parent.LinksEnabled)
                    txtView.DataDetectorTypes = UIDataDetectorType.All;

                SetNativeControl(txtView);
            }

        }
    }
}
#endif

