#if __ANDROID__
using Android.Content.Res;
using Android.Graphics.Drawables;
using AndroidX.Core.View;
using Xamarin.Forms;
using Xamarin.Forms.Core;
using Xamarin.Forms.Platform.Android;
using Graphics = Android.Graphics;

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


    public class UnderlineColor : PlatformEffect
    {
        public Xamarin.Forms.Color LineColor { get; set; }
        public int ColorResourceId { get; set; } = 0;

        protected override void OnAttached()
        {
			var editText = Control as global::Android.Widget.EditText;
			ViewCompat.SetBackgroundTintList(editText, ColorStateList.ValueOf(LineColor.ToAndroid()));
        }

        protected override void OnDetached()
        {

        }
    }



}
#endif

