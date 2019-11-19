using System;
using System.ComponentModel;
using Xamarin.Forms;

namespace Xamarin.Forms.Core
{
    [DesignTimeVisible(true)]
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
