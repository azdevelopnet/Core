#if __ANDROID__
using System;
using System.Linq;
using Android.Content;
using Android.Graphics.Drawables;
using Android.Widget;
using Xamarin.Forms.Core;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Views = Android.Views;
using Graphics = Android.Graphics;
using System.ComponentModel;
using Android.Views;
using Plugin.CurrentActivity;
using Ctx = Android.Content.Context;
using Android.Runtime;
using AndroidX.Core.Content;

[assembly: ExportRenderer(typeof(CoreUnderlineEntry), typeof(CoreUnderlineEntryRenderer))]
namespace Xamarin.Forms.Core
{
    public class CoreUnderlineEntryRenderer : EntryRenderer
    {
        private CoreUnderlineEntry formControl;
        private readonly Ctx context;


        public CoreUnderlineEntryRenderer(Ctx ctx) : base(ctx)
        {
            context = ctx;
        }
        protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.Entry> e)
        {
            base.OnElementChanged(e);

            if (e.NewElement != null)
            {
                formControl = (Element as CoreUnderlineEntry);

                var editText = (EditText)Control;

                editText.EditorAction += (obj, act) =>
                  {
                      formControl?.NextFocus?.Invoke();
                  };
                editText.Touch += (a, aa) =>
                {
                    aa.Handled = false;
                    var w = editText.Width;
                    var wl = editText.CompoundPaddingLeft;
                    var wr = w - editText.CompoundPaddingRight;
                    var x = aa.Event.GetX();
                    if (wr < x && aa.Event.Action == Views.MotionEventActions.Down)
                    {
                        if (formControl.IsPassword
                            && formControl.PasswordRevealEnabled
                            && !string.IsNullOrEmpty(formControl.PasswordRevealIcon)
                            && !string.IsNullOrEmpty(formControl.PasswordHideIcon))
                        {
                            formControl.IsPassword = !formControl.IsPassword;
                            var rightDrawable = formControl.IsPassword == true ? GetDrawable(formControl.PasswordRevealIcon) : GetDrawable(formControl.PasswordHideIcon);
                            editText.SetCompoundDrawablesWithIntrinsicBounds(GetDrawable(formControl.Icon)?.Target as Drawable, null, rightDrawable?.Target as Drawable, null);
                            editText.CompoundDrawablePadding = 20;
                        }
                    }

                };

                if (formControl != null
                    && formControl.IsPassword
                    && !string.IsNullOrEmpty(formControl.PasswordRevealIcon)
                    && !string.IsNullOrEmpty(formControl.PasswordHideIcon)
                    && formControl.PasswordRevealEnabled)
                {
                    var size = editText.TextSize;
                    var rightDrawable = formControl.IsPassword == true ? GetDrawable(formControl.PasswordRevealIcon) : null;

                    editText.SetCompoundDrawablesWithIntrinsicBounds(GetDrawable(formControl.Icon)?.Target as Drawable, null, rightDrawable?.Target as Drawable, null);
                    editText.CompoundDrawablePadding = 20;
                    editText.Gravity = Views.GravityFlags.Bottom;

                }
                if (!formControl.PasswordRevealEnabled && !string.IsNullOrEmpty(formControl.ClearEntryIcon) && formControl.ClearEntryEnabled)
                {
                    var iconResource = formControl.ClearEntryIcon.Replace(".png", string.Empty).Replace(".jpg", string.Empty);
                    var clearIcon = GetResourceIdByName(iconResource);
                    editText.SetCompoundDrawablesRelativeWithIntrinsicBounds(0, 0, clearIcon, 0);

                    editText.SetOnTouchListener(new OnDrawableTouchListener());
                }
                if (formControl != null && formControl.EntryColor != null)
                {
                    editText.Background.Mutate().SetColorFilter(formControl.EntryColor.ToAndroid(), Graphics.PorterDuff.Mode.SrcAtop);
                }

                if ((Control != null) & (e.NewElement != null))
                {
                    var entryExt = (e.NewElement as CoreUnderlineEntry);
                    Control.ImeOptions = entryExt.ReturnKeyType.GetValueFromDescription();
                    Control.SetImeActionLabel(entryExt.ReturnKeyType.ToString(), Control.ImeOptions);

                }

                if (!string.IsNullOrEmpty(e.NewElement?.StyleId) && Control != null)
                {
                    var fileName = e.NewElement.StyleId + ".ttf";
                    if (Resources.Assets.List("").Contains(fileName))
                    {
                        var font = Graphics.Typeface.CreateFromAsset(context.ApplicationContext.Assets, fileName);
                        Control.Typeface = font;
                    }
                }
            }
        }


        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            if (Control == null) return;

            if (e.PropertyName == CoreUnderlineEntry.ReturnKeyTypeProperty.PropertyName)
            {
                var entryExt = (sender as CoreUnderlineEntry);
                Control.ImeOptions = entryExt.ReturnKeyType.GetValueFromDescription();
                Control.SetImeActionLabel(entryExt.ReturnKeyType.ToString(), Control.ImeOptions);
            }


            if (formControl.HasError)
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    Control?.Background?.ClearColorFilter();
                    Control?.Background?.SetColorFilter(Graphics.Color.Red, Graphics.PorterDuff.Mode.SrcIn);
                });
            }
            else
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    Control?.Background?.ClearColorFilter();
                    Control?.Background?.SetColorFilter(formControl.EntryColor.ToAndroid(), Graphics.PorterDuff.Mode.SrcIn);
                });
            }

        }

        private WeakReference GetDrawable(string fileName)
        {
            if (String.IsNullOrEmpty(fileName))
                return null;
            var img = fileName.Replace(".png", "");
            var id = GetResourceIdByName(img);
            var drawable = new WeakReference((Drawable)ContextCompat.GetDrawable(context, id));
            ((Drawable)drawable.Target).SetColorFilter(formControl.EntryColor.ToAndroid(), Graphics.PorterDuff.Mode.SrcAtop);
            return drawable;
        }

        private int GetResourceIdByName(string name)
        {
            return Resources.GetIdentifier(name, "drawable", Context.PackageName);
        }

        private class OnDrawableTouchListener : Java.Lang.Object, IOnTouchListener
        {
            public bool OnTouch(Views.View v, Views.MotionEvent e)
            {
                if (v is EditText && e.Action == MotionEventActions.Up)
                {
                    EditText editText = (EditText)v;
                    if (e.RawX >= (editText.Right - editText.GetCompoundDrawables()[2].Bounds.Width()))
                    {
                        editText.Text = string.Empty;
                        return true;
                    }
                }

                return false;
            }
        }

    }

}
#endif

