#if __IOS__
using System;
using CoreAnimation;
using CoreGraphics;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Core;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(CoreDatePicker), typeof(CoreDatePickerRenderer))]
namespace Xamarin.Forms.Core
{
    public class CoreDatePickerRenderer : DatePickerRenderer
    {
        private CALayer bottomBorder;
        private CGColor controlColor;
        private CoreDatePicker element;
        protected override void OnElementChanged(ElementChangedEventArgs<DatePicker> e)
        {
            base.OnElementChanged(e);
            if (e.NewElement != null && Control != null)
            {
                element = (CoreDatePicker)e.NewElement;
                controlColor = element.EntryColor.ToCGColor();
                Control.BorderStyle = UITextBorderStyle.None;
            }
        }


        protected override void OnElementPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Width")
            {
                var width = ((CoreDatePicker)sender).Width;
                var height = ((CoreDatePicker)sender).Height;
                bottomBorder?.RemoveFromSuperLayer();
                if (width > 0 && height > 0)
                    CreateUnderline((nfloat)height, (nfloat)width);
            }
            if (e.PropertyName == "Height")
            {
                var width = ((CoreDatePicker)sender).Width;
                var height = ((CoreDatePicker)sender).Height;
                bottomBorder?.RemoveFromSuperLayer();
                if (width > 0 && height > 0)
                    CreateUnderline((nfloat)height, (nfloat)width);
            }

            base.OnElementPropertyChanged(sender, e);

        }
        private void CreateUnderline(nfloat height, nfloat width)
        {
            bottomBorder = new CALayer();
            bottomBorder.Frame = new CoreGraphics.CGRect(0, height - 1, width, 1);
            bottomBorder.BackgroundColor = controlColor;
            Control.Layer.AddSublayer(bottomBorder);

        }
    }
}
#endif
