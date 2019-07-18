#if __IOS__
using System;
using System.ComponentModel;
using System.Reflection;
using CoreGraphics;
using Foundation;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Core;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(CoreRadioButton), typeof(CoreRadioButtonRenderer))]
namespace Xamarin.Forms.Core
{
    public class CoreRadioButtonRenderer : ViewRenderer<CoreRadioButton, RadioButtonView>
    {
        protected override void OnElementChanged(ElementChangedEventArgs<CoreRadioButton> e)
        {
            base.OnElementChanged(e);

            if (Control == null)
            {
                var checkBox = new RadioButtonView(Bounds);
                checkBox.TouchUpInside += (s, args) => Element.Checked = Control.Checked;

                SetNativeControl(checkBox);
            }

            if (e.NewElement != null)
            {
                var fontSize = e.NewElement.FontSize.Equals(0.0d) ? Control.Font.PointSize : (nfloat)e.NewElement.FontSize;
                var fontFamily = string.IsNullOrEmpty(e.NewElement.FontFamily) ? Control.Font.FamilyName : e.NewElement.FontFamily;

                var font = UIFont.FromName(fontFamily, fontSize);

                Control.Font = font;
                Control.LineBreakMode = UILineBreakMode.CharacterWrap;
                Control.VerticalAlignment = UIControlContentVerticalAlignment.Center;
                Control.Text = e.NewElement.Text;
                Control.Checked = e.NewElement.Checked;
                Control.SetTitleColor(e.NewElement.TextColor.ToUIColor(), UIControlState.Normal);
                Control.SetTitleColor(e.NewElement.TextColor.ToUIColor(), UIControlState.Selected);
                Control.ImageColor = e.NewElement.ImageColor.ToUIColor();
            }
        }


        public override void Draw(CGRect rect)
        {
            base.Draw(rect);
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            switch (e.PropertyName)
            {
                case "Checked":
                    Control.Checked = Element.Checked;
                    break;
                case "Text":
                    Control.Text = Element.Text;
                    break;
                case "TextColor":
                    Control.SetTitleColor(Element.TextColor.ToUIColor(), UIControlState.Normal);
                    Control.SetTitleColor(Element.TextColor.ToUIColor(), UIControlState.Selected);
                    break;

                case "Element":
                    break;
                default:
                    System.Diagnostics.Debug.WriteLine("Property change for {0} has not been implemented.", e.PropertyName);

					if (Element.SelectedImage != null && Element.UnSelectedImage != null
                        && Control.SelectedImage == null && Control.UnSelectedImage == null)
					{
						Control.SelectedImage = Element.SelectedImage;
                        Control.UnSelectedImage = Element.UnSelectedImage;
						Control.ApplyStyle();
					}

                    return;
            }
        }
    }


    [Register("RadioButtonView")]
    public class RadioButtonView : UIButton
    {
        public string UnSelectedImage { get; set; }

        public string SelectedImage { get; set; }

        public UIColor ImageColor { get; set; }

        public RadioButtonView()
        {
            Initialize();
        }

        public RadioButtonView(CGRect bounds)
            : base(bounds)
        {
            Initialize();
        }


        public bool Checked
        {
            set { this.Selected = value; }
            get { return this.Selected; }
        }

        public string Text
        {
            set { this.SetTitle(value, UIControlState.Normal); }

        }

        void Initialize()
        {
            
            this.AdjustEdgeInsets();
            this.ApplyStyle();

            this.TouchUpInside += (sender, args) => this.Selected = !this.Selected;
        }

        void AdjustEdgeInsets()
        {
            const float inset = 8f;
            this.VerticalAlignment = UIControlContentVerticalAlignment.Center;
            this.HorizontalAlignment = UIControlContentHorizontalAlignment.Left;
            this.ImageEdgeInsets = new UIEdgeInsets(0f, inset, 0f, 0f);
            this.TitleEdgeInsets = new UIEdgeInsets(0f, inset * 2, 0f, 0f);
        }

        public void ApplyStyle()
        {
            if(SelectedImage!=null && UnSelectedImage!=null)
            {
                var imgColor = ImageColor == null ? this.TitleLabel.TextColor : ImageColor;
				this.SetImage(UIImage.FromBundle(this.SelectedImage).ChangeImageColor(imgColor), UIControlState.Selected);
				this.SetImage(UIImage.FromBundle(this.UnSelectedImage).ChangeImageColor(imgColor), UIControlState.Normal);
            }
        }
    }
}
#endif