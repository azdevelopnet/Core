#if __ANDROID__
using Android.Content;
using Android.Content.Res;
using Android.Widget;
using Plugin.CurrentActivity;
using Xamarin.Forms;
using Xamarin.Forms.Core;
using Xamarin.Forms.Platform.Android;
using Attribute = Android.Resource.Attribute;

[assembly: ExportRenderer(typeof(CoreRadioButton), typeof(CoreRadioButtonRenderer))]
namespace Xamarin.Forms.Core
{
    public class CoreRadioButtonRenderer: ViewRenderer<CoreRadioButton, RadioButton>
    {
        public CoreRadioButtonRenderer(Context ctx) : base(ctx)
        {

        }

        protected override void OnElementChanged(ElementChangedEventArgs<CoreRadioButton> e)
        {
            base.OnElementChanged(e);

            if (e.OldElement != null)
            {
                e.OldElement.PropertyChanged += ElementOnPropertyChanged;  
            }

            if(this.Control == null)
            {
                var radButton = new RadioButton(this.Context);
                radButton.CheckedChange += radButton_CheckedChange;
              
                this.SetNativeControl(radButton);
            }

            if (e.NewElement != null)
            {
                Control.ButtonTintList = GetTintColors(e.NewElement.ImageColor);
                Control.SetTextColor(GetTintColors(e.NewElement.TextColor));
                Control.TextSize = (float)e.NewElement.FontSize;
				Control.Text = e.NewElement.Text;
                Control.Checked = e.NewElement.Checked;
            }

            if(Element!=null)
                Element.PropertyChanged += ElementOnPropertyChanged;
        }

        void radButton_CheckedChange(object sender, CompoundButton.CheckedChangeEventArgs e)
        {
            this.Element.Checked = e.IsChecked;
        }

        void ElementOnPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "Checked":
                    if(Control!=null && Element!=null)
                        Control.Checked = Element.Checked;
                    break;
                case "Text":
                    if (Control != null && Element != null)
                        Control.Text = Element.Text;
                    break;

            }
        }

		private ColorStateList GetTintColors(Color color)
		{
			int[][] states = new int[][] {
				new int[] { Attribute.StateEnabled }, // enabled
                new int[] {-Attribute.StateEnabled }, // disabled
                new int[] {-Attribute.StateChecked }, // unchecked
                new int[] { Attribute.StatePressed }  // pressed
            };
			int[] colors = new int[] {
				color.ToAndroid(),
				color.ToAndroid(),
				color.ToAndroid(),
				color.ToAndroid()
			};
			return new ColorStateList(states, colors);
		}
    }
}
#endif