using System;
namespace Xamarin.Forms.Core
{
    public class CoreStackLayout :StackLayout
    {
		/// <summary>
		/// Start color for the gradient (top) color
		/// </summary>
		public static readonly BindableProperty StartColorProperty =
			BindableProperty.Create("StartColor",
									typeof(Color),
									typeof(CoreButton),
									Color.Black);
		public Color StartColor
		{
			get { return (Color)this.GetValue(StartColorProperty); }
			set { this.SetValue(StartColorProperty, value); }
		}

		/// <summary>
		/// End color for the gradient (bottom) color
		/// </summary>
		public static readonly BindableProperty EndColorProperty =
			BindableProperty.Create("EndColor",
									typeof(Color),
									typeof(CoreButton),
									Color.Black);
		public Color EndColor
		{
			get { return (Color)this.GetValue(EndColorProperty); }
			set { this.SetValue(EndColorProperty, value); }
		}
    }
}
