#if __ANDROID__
using System;
using Android.Graphics.Drawables;
using Xamarin.Forms.Core;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Graphics = Android.Graphics;
using Android.Views;
using OS = Android.OS;
using Android.Graphics;
//using Android.Support.V7.Widget;
using Android.Runtime;
using Android.Widget;

[assembly: ExportEffect(typeof(ViewShadow), "ViewShadow")]
[assembly: ExportEffect(typeof(HideTableSeparator), "HideTableSeparator")]
[assembly: ExportEffect(typeof(ListRemoveEmptyRows), "ListRemoveEmptyRows")]
namespace Xamarin.Forms.Core
{

	public class ListRemoveEmptyRows : PlatformEffect
	{
		protected override void OnAttached()
		{

		}

		protected override void OnDetached()
		{

		}
	}

	public class HideTableSeparator : PlatformEffect
	{
		protected override void OnAttached()
		{
			if (Control != null)
			{
				var listView = Control as global::Android.Widget.ListView;
				//listView.Divider = null;
				listView.Divider = new ColorDrawable(Graphics.Color.Transparent);
				listView.DividerHeight = 0;
			}
		}

		protected override void OnDetached()
		{

		}
	}

	public class ViewShadow : PlatformEffect
	{


		protected override void OnAttached()
		{
			//Control.Elevation = 10;
			//Control.TranslationZ = 20;
			//Control.SetBackgroundColor(Color.Red.ToAndroid());

			//var v = this.Element as View;
			//var renderer = RendererFactory.GetRenderer(v);
			//renderer.ViewGroup.SetClipToPadding(false);

		}

		protected override void OnDetached()
		{

		}
	}

    public class UnderlineColor : PlatformEffect
    {
        public Xamarin.Forms.Color LineColor { get; set; }
        public int ColorResourceId { get; set; } = 0;

        protected override void OnAttached()
        {
            IntPtr IntPtrtextViewClass = JNIEnv.FindClass(typeof(TextView));
            IntPtr mCursorDrawableResProperty = JNIEnv.GetFieldID(IntPtrtextViewClass, "mCursorDrawableRes", "I");
            JNIEnv.SetField(Control.Handle, mCursorDrawableResProperty, ColorResourceId);

            Control.Background.Mutate().SetColorFilter(LineColor.ToAndroid(), Graphics.PorterDuff.Mode.SrcAtop);
        }

        protected override void OnDetached()
        {

        }
    }



}
#endif

