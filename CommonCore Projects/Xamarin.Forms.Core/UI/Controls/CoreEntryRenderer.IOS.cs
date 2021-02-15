#if __IOS__
using System;
using System.ComponentModel;
using System.Drawing;
using System.Threading.Tasks;
using CoreGraphics;
using Foundation;
using UIKit;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Core;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(CoreEntry), typeof(CoreEntryRenderer))]
namespace Xamarin.Forms.Core
{
    public class CoreEntryRenderer : EntryRenderer
    {
        public CoreEntry ElementV2 => Element as CoreEntry;
        public UITextFieldPadding ControlV2 => Control as UITextFieldPadding;

        protected override UITextField CreateNativeControl()
        {
            var control = new UITextFieldPadding(RectangleF.Empty)
            {
                Padding = ElementV2.Padding,
                BorderStyle = UITextBorderStyle.RoundedRect,
                ClipsToBounds = true
            };

            UpdateBackground(control);

            return control;
        }

        protected void UpdateBackground(UITextField control)
        {
            if (control == null) return;
            control.Layer.CornerRadius = ElementV2.CornerRadius;
            control.Layer.BorderWidth = ElementV2.BorderThickness;
            control.Layer.BorderColor = ElementV2.BorderColor.ToCGColor();
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);

            LoadControlImage();

            LoadReturnType();

            if (Control != null)
            {
                ((UITextField)Control).ShouldReturn = (textField) =>
                {
                    ElementV2?.NextFocus?.Invoke();
                    ElementV2?.ReturnCommand?.Execute(textField);
                    return true;
                };
            }

        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == CoreEntry.PaddingProperty.PropertyName)
            {
                UpdatePadding();
            }
            if (e.PropertyName == CoreEntry.BorderColorProperty.PropertyName)
            {
                UpdateBorderColor();
            }
            if (e.PropertyName == CoreEntry.ImageProperty.PropertyName)
            {
                LoadControlImage();
            }

            base.OnElementPropertyChanged(sender, e);
        }

        protected void UpdateBorderColor()
        {
            if (Control == null)
                return;

            Control.Layer.BorderColor = ElementV2.BorderColor.ToCGColor();
        }
        protected void UpdatePadding()
        {
            if (Control == null)
                return;

            ControlV2.Padding = ElementV2.Padding;
        }

        private void LoadReturnType()
        {
            if ((Control != null) & (ElementV2 != null))
            {
                var returnType = ElementV2.ReturnKeyType;

                if (this.Element.Keyboard == Keyboard.Numeric || this.Element.Keyboard == Keyboard.Telephone)
                {
                    this.AddAccessoryButton(returnType);
                }
                else
                {
                    Control.ReturnKeyType = returnType.ToUIReturnKey();
                  
                }
            }
        }

        private void LoadControlImage()
        {
            if (Control != null && Element != null)
            {
                var element = (CoreEntry)this.Element;
                if (element.Image != null)
                {
                    MainThread.BeginInvokeOnMainThread(async () =>
                    {
                        var ctrl = (UITextFieldPadding)Control;
                        var padding = element.ImageWidth + 10;
                        var view = await GetImageView(element.Image, element.ImageHeight, element.ImageWidth);
                        switch (element.ImageAlignment)
                        {
                            case ImageAlignment.Left:
                                Control.LeftViewMode = UITextFieldViewMode.Always;
                                Control.LeftView = view;
                                ctrl.Padding = new Thickness(padding, 0, 5, 0);
                                break;
                            case ImageAlignment.Right:
                                Control.RightViewMode = UITextFieldViewMode.Always;
                                Control.RightView = view;
                                ctrl.Padding = new Thickness(5, 0, padding, 0);
                                break;
                        }
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

            if (ElementV2.ImageClicked != null && ElementV2.ImageAlignment== ImageAlignment.Right)
            {
                var btn = new UIButton(uiImageView.Frame);
                btn.AddSubview(uiImageView);
                btn.TouchUpInside += (ee, aa) => {
                    ElementV2.ImageClicked?.Execute(ElementV2);
                };
                objLeftView.AddSubview(btn);
            }
            else
            {
                objLeftView.AddSubview(uiImageView);
            }

            uiImageView.Center = objLeftView.Center;
            return objLeftView;
        }

        private void AddAccessoryButton(ReturnKeyTypes returnType)
        {
            if (returnType == ReturnKeyTypes.Done || returnType == ReturnKeyTypes.Next)
            {
                UIToolbar toolbar = new UIToolbar(new CGRect(0.0f, 0.0f, 50.0f, 44.0f));

                UIBarButtonItem accessoryButton = null;

                if (returnType == ReturnKeyTypes.Done)
                {
                    accessoryButton = new UIBarButtonItem(UIBarButtonSystemItem.Done, delegate
                    {
                        this.Control.ResignFirstResponder();
                        ElementV2?.ReturnCommand?.Execute(null);
                    });
                }
                else
                {
                    accessoryButton = new UIBarButtonItem("Next", UIBarButtonItemStyle.Bordered, delegate
                    {
                        this.Control.ResignFirstResponder();
                        ElementV2?.ReturnCommand?.Execute(null);
                    });

                }

                toolbar.Items = new UIBarButtonItem[]
                {
                    new UIBarButtonItem (UIBarButtonSystemItem.FlexibleSpace),
                    accessoryButton
                };
                this.Control.InputAccessoryView = toolbar;
            }
        }
    }

    public class UITextFieldPadding : UITextField
    {
        private Thickness _padding = new Thickness(5);

        public Thickness Padding
        {
            get => _padding;
            set
            {
                if (_padding != value)
                {
                    _padding = value;
                }
            }
        }

        public UITextFieldPadding()
        {
        }
        public UITextFieldPadding(NSCoder coder) : base(coder)
        {
        }

        public UITextFieldPadding(CGRect rect) : base(rect)
        {
        }

        public override CGRect TextRect(CGRect forBounds)
        {
            var insets = new UIEdgeInsets((float)Padding.Top, (float)Padding.Left, (float)Padding.Bottom, (float)Padding.Right);
            return insets.InsetRect(forBounds);
        }

        public override CGRect PlaceholderRect(CGRect forBounds)
        {
            var insets = new UIEdgeInsets((float)Padding.Top, (float)Padding.Left, (float)Padding.Bottom, (float)Padding.Right);
            return insets.InsetRect(forBounds);
        }

        public override CGRect EditingRect(CGRect forBounds)
        {
            var insets = new UIEdgeInsets((float)Padding.Top, (float)Padding.Left, (float)Padding.Bottom, (float)Padding.Right);
            return insets.InsetRect(forBounds);
        }
    }
}
#endif
