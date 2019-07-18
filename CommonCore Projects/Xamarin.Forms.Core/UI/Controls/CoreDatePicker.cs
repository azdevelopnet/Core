using System;
using Xamarin.Forms;

namespace Xamarin.Forms.Core
{
    public class CoreDatePicker : DatePicker
	{
		public static readonly BindableProperty EntryColorProperty =
			BindableProperty.Create("EntryColor",
									typeof(Color),
                                    typeof(CoreDatePicker),
									Color.Black);
		public Color EntryColor
		{
			get { return (Color)this.GetValue(EntryColorProperty); }
			set { this.SetValue(EntryColorProperty, value); }
		}
	}
}
