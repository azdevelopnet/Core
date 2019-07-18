using System;
namespace Xamarin.Forms.Core
{
    public class CoreSwitch: Switch
    {
		public static readonly BindableProperty TrueColorProperty =
        	BindableProperty.Create("TrueColor",
        							typeof(Color),
        							typeof(CoreSwitch),
        							Color.LightGreen);
		public Color TrueColor
		{
			get { return (Color)this.GetValue(TrueColorProperty); }
			set { this.SetValue(TrueColorProperty, value); }
		}

		public static readonly BindableProperty FalseColorProperty =
        	BindableProperty.Create("FalseColor",
        							typeof(Color),
        							typeof(CoreSwitch),
        							Color.LightGray);
		public Color FalseColor
		{
			get { return (Color)this.GetValue(FalseColorProperty); }
			set { this.SetValue(FalseColorProperty, value); }
		}

    }
}
