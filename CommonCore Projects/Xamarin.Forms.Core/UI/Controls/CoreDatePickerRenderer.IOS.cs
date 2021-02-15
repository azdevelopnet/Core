#if __IOS__
using System;
using System.Drawing;
using System.Threading.Tasks;
using CoreAnimation;
using CoreGraphics;
using UIKit;
using Xamarin.Essentials;
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

        public CoreDatePicker ElementV2 => Element as CoreDatePicker;
        public UITextFieldPadding ControlV2 { get; set; }

        protected override UITextField CreateNativeControl()
        {
            var control = new UITextFieldPadding(RectangleF.Empty);

            if (ElementV2.IsEnterUnderline)
            {
                control.BorderStyle = UITextBorderStyle.None;
            }
            else
            {
                control.BorderStyle = UITextBorderStyle.RoundedRect;
                control.ClipsToBounds = true;
                UpdateBackground(control);
            }

            ControlV2 = control;

            LoadControlImage();

            return ControlV2;
        }

        protected override void OnElementPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (ElementV2.IsEnterUnderline)
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
            }

            if (e.PropertyName == CoreEntry.ImageProperty.PropertyName)
            {
                LoadControlImage();
            }


            base.OnElementPropertyChanged(sender, e);

        }

        protected void UpdateBackground(UITextField control)
        {
            if (control == null) return;
            control.Layer.CornerRadius = ElementV2.CornerRadius;
            control.Layer.BorderWidth = ElementV2.BorderThickness;
            control.Layer.BorderColor = ElementV2.BorderColor.ToCGColor();
        }

        private void CreateUnderline(nfloat height, nfloat width)
        {
            bottomBorder = new CALayer();
            bottomBorder.Frame = new CoreGraphics.CGRect(0, height - 1, width, 1);
            bottomBorder.BackgroundColor = controlColor;
            ControlV2.Layer.AddSublayer(bottomBorder);

        }

        private void LoadControlImage()
        {
            if (ControlV2 != null )
            {
                if (ElementV2.Image != null)
                {
                    MainThread.BeginInvokeOnMainThread(async () =>
                    {
                        var padding = ElementV2.ImageWidth + 10;
                        var view = await GetImageView(ElementV2.Image, ElementV2.ImageHeight, ElementV2.ImageWidth);
                        ControlV2.RightViewMode = UITextFieldViewMode.Always;
                        ControlV2.RightView = view;
                        ControlV2.Padding = new Thickness(5, 0, padding, 0);
                    });
                }
            }
        }

        private async Task<UIView> GetImageView(ImageSource source, int height, int width)
        {
            var uiImage = await source.ToUIImage();
            var uiImageView = new UIImageView(uiImage)
            {
                Frame = new RectangleF(0, 0, width, height)
            };
            var objLeftView = new UIView(new System.Drawing.Rectangle(0, 0, width + 10, height));

            objLeftView.AddSubview(uiImageView);

            uiImageView.Center = objLeftView.Center;
            return objLeftView;
        }
    }
}
#endif
