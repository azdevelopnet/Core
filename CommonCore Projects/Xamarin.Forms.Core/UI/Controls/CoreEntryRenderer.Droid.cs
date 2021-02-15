#if __ANDROID__
using System;
using System.ComponentModel;
using System.Threading.Tasks;
using Android.Content;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.Text.Method;
using Android.Widget;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Core;
using Xamarin.Forms.Platform.Android;
using Views = Android.Views;

[assembly: ExportRenderer(typeof(CoreEntry), typeof(CoreEntryRenderer))]
namespace Xamarin.Forms.Core
{
    public class CoreEntryRenderer : EntryRenderer
    {
        public CoreEntryRenderer(Context context) : base(context)
        {

        }

        public CoreEntry ElementV2 => Element as CoreEntry;

        protected override FormsEditText CreateNativeControl()
        {
            var control = base.CreateNativeControl();
            UpdateBackground(control);
            return control;
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.Entry> e)
        {
            base.OnElementChanged(e);

            LoadControlImage();

            LoadImageClickEvent();

            LoadKeyBoardActionProperties();
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == CoreEntry.CornerRadiusProperty.PropertyName)
            {
                UpdateBackground();
            }
            if (e.PropertyName == CoreEntry.BorderThicknessProperty.PropertyName)
            {
                UpdateBackground();
            }
            if (e.PropertyName == CoreEntry.BorderColorProperty.PropertyName)
            {
                UpdateBackground();
            }
            if (e.PropertyName == CoreEntry.ImageProperty.PropertyName)
            {
                LoadControlImage();
            }
            if (e.PropertyName == CoreEntry.IsPasswordProperty.PropertyName)
            {
                var editText = (EditText)Control;
                if(ElementV2.IsPassword)
                    editText.TransformationMethod = PasswordTransformationMethod.Instance;
                else
                    editText.TransformationMethod = HideReturnsTransformationMethod.Instance;


            }

            base.OnElementPropertyChanged(sender, e);
        }

        protected override void UpdateBackgroundColor()
        {
            UpdateBackground();
        }
        protected void UpdateBackground(FormsEditText control)
        {
            if (control == null) return;

            var gd = new GradientDrawable();
            gd.SetColor(Element.BackgroundColor.ToAndroid());
            gd.SetCornerRadius(Context.ToPixels(ElementV2.CornerRadius));
            gd.SetStroke((int)Context.ToPixels(ElementV2.BorderThickness), ElementV2.BorderColor.ToAndroid());
            control.SetBackground(gd);

            var padTop = (int)Context.ToPixels(ElementV2.Padding.Top);
            var padBottom = (int)Context.ToPixels(ElementV2.Padding.Bottom);
            var padLeft = (int)Context.ToPixels(ElementV2.Padding.Left);
            var padRight = (int)Context.ToPixels(ElementV2.Padding.Right);

            control.SetPadding(padLeft, padTop, padRight, padBottom);
        }
        protected override void UpdateBackground()
        {
            UpdateBackground(Control);
        }

        private void LoadKeyBoardActionProperties()
        {
            Control.EditorAction += (obj, act) =>
            {
                ElementV2?.NextFocus?.Invoke();
                ElementV2?.ReturnCommand?.Execute(obj);
            };

            if ((Control != null) & (ElementV2 != null))
            {
                Control.ImeOptions = ElementV2.ReturnKeyType.ToImeAction();
                Control.SetImeActionLabel(ElementV2.ReturnKeyType.ToString(), Control.ImeOptions);
            }
        }

        private void LoadImageClickEvent()
        {
            if (Control != null && Element != null && ElementV2.ImageAlignment == ImageAlignment.Right)
            {
                var editText = (EditText)Control;
                if (ElementV2.ImageClicked != null)
                {
                    editText.Touch += (a, aa) =>
                    {
                        aa.Handled = false;
                        var w = editText.Width;
                        var wr = w - editText.CompoundPaddingRight;
                        var x = aa.Event.GetX();
                        if (wr < x && aa.Event.Action == Views.MotionEventActions.Down)
                        {
                            ElementV2.ImageClicked?.Execute(ElementV2);
                        }

                    };
                }
            }
        }

        private void LoadControlImage()
        {
            if (Control != null && Element != null)
            {
                if (ElementV2.Image != null)
                {
                    MainThread.BeginInvokeOnMainThread(async () =>
                    {
                        var bitMapDrawable = await GetDrawable(ElementV2.Image);
                        Control.CompoundDrawablePadding = 10;
                        switch (ElementV2.ImageAlignment)
                        {
                            case ImageAlignment.Left:
                                Control.SetCompoundDrawablesWithIntrinsicBounds(bitMapDrawable, null, null, null);
                                break;
                            case ImageAlignment.Right:
                                Control.SetCompoundDrawablesWithIntrinsicBounds(null, null, bitMapDrawable, null);
                                break;
                        }
                    });

                }
            }
        }

        private async Task<BitmapDrawable> GetDrawable(ImageSource source)
        {
            var element = (CoreEntry)this.Element;
            var bitMap = await source.ToBitmap();
            var bitMapDrawable = new BitmapDrawable(Resources, Bitmap.CreateScaledBitmap(bitMap, (element.ImageWidth * 2 ), element.ImageHeight * 2, true));
            return bitMapDrawable;
        }
    }
}
#endif
