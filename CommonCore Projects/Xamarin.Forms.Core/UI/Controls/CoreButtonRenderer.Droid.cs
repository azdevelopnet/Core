#if __ANDROID__
using System;
using Android.Content;
using Android.Content.Res;
using Android.Graphics.Drawables;
using Xamarin.Forms;
using Xamarin.Forms.Core;
using Xamarin.Forms.Platform.Android;
using Attribute = Android.Resource.Attribute;

[assembly: ExportRenderer(typeof(CoreButton), typeof(CoreButtonRenderer))]
namespace Xamarin.Forms.Core
{
    public class CoreButtonRenderer : ButtonRenderer
	{
		CoreButton caller;

        public CoreButtonRenderer(Context ctx) : base(ctx)
        {

        }

        protected override void OnElementChanged(ElementChangedEventArgs<Button> e)
		{
			base.OnElementChanged(e);

			if (Control != null)
			{
				caller = e.NewElement as CoreButton;
				SetButtonDisableState();
                SetGradientAndRadius();

			}
		}

		private void SetButtonDisableState()
		{
			int[][] states = new int[][] {
				new int[] { Attribute.StateEnabled }, // enabled
                new int[] {-Attribute.StateEnabled }, // disabled
                new int[] {-Attribute.StateChecked }, // unchecked
                new int[] { Attribute.StatePressed }  // pressed
            };
			int[] colors = new int[] {
				caller.TextColor.ToAndroid(),
				caller.DisabledTextColor.ToAndroid(),
				caller.TextColor.ToAndroid(),
				caller.TextColor.ToAndroid()
			};
			var buttonStates = new ColorStateList(states, colors);
			Control.SetTextColor(buttonStates);

		}

		protected override void OnElementPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			if (e.PropertyName == CoreButton.IsEnabledProperty.PropertyName)
			{
                SetGradientAndRadius();
			}
			base.OnElementPropertyChanged(sender, e);
		}

        private void SetGradientAndRadius()
        {
			var gradient = new GradientDrawable(GradientDrawable.Orientation.TopBottom, new[] {
					caller.StartColor.ToAndroid().ToArgb(),
					caller.EndColor.ToAndroid().ToArgb()
				});

         
			gradient.SetCornerRadius(caller.CornerRadius.ToDevicePixels());
            Control.SetBackground(gradient);

            var num = caller.IsEnabled ? 105f : 100f;

			Control.Elevation = num;
			Control.TranslationZ = num;
        }
	}
}
#endif

