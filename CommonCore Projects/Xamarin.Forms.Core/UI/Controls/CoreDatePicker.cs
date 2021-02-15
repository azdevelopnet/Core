using System;
using System.ComponentModel;
using Xamarin.Forms;

namespace Xamarin.Forms.Core
{
    [DesignTimeVisible(true)]
    public class CoreDatePicker : DatePicker
	{
		public static readonly BindableProperty ImageProperty =
			BindableProperty.Create(nameof(Image), typeof(ImageSource), typeof(CoreDatePicker), null);

		public static readonly BindableProperty ImageHeightProperty =
			BindableProperty.Create(nameof(ImageHeight), typeof(int), typeof(CoreDatePicker), 22);

		public static readonly BindableProperty ImageWidthProperty =
			BindableProperty.Create(nameof(ImageWidth), typeof(int), typeof(CoreDatePicker), 22);

		public static BindableProperty CornerRadiusProperty =
			BindableProperty.Create(nameof(CornerRadius), typeof(int), typeof(CoreEntry), 0);

		public static BindableProperty BorderThicknessProperty =
			BindableProperty.Create(nameof(CoreDatePicker), typeof(int), typeof(CoreEntry), 0);

		public static BindableProperty PaddingProperty =
			BindableProperty.Create(nameof(CoreDatePicker), typeof(Thickness), typeof(CoreEntry), new Thickness(5));

		public static BindableProperty BorderColorProperty =
			BindableProperty.Create(nameof(CoreDatePicker), typeof(Color), typeof(CoreEntry), Color.Transparent);

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


		public static readonly BindableProperty IsEnterUnderlineProperty =
			BindableProperty.Create("IsEnterUnderline",
									typeof(bool),
									typeof(CoreDatePicker),
									false);

		public bool IsEnterUnderline
		{
			get { return (bool)this.GetValue(IsEnterUnderlineProperty); }
			set { this.SetValue(IsEnterUnderlineProperty, value); }
		}

		public int CornerRadius
		{
			get => (int)GetValue(CornerRadiusProperty);
			set => SetValue(CornerRadiusProperty, value);
		}

		public int BorderThickness
		{
			get => (int)GetValue(BorderThicknessProperty);
			set => SetValue(BorderThicknessProperty, value);
		}
		public Color BorderColor
		{
			get => (Color)GetValue(BorderColorProperty);
			set => SetValue(BorderColorProperty, value);
		}

		public int ImageWidth
		{
			get { return (int)GetValue(ImageWidthProperty); }
			set { SetValue(ImageWidthProperty, value); }
		}

		public int ImageHeight
		{
			get { return (int)GetValue(ImageHeightProperty); }
			set { SetValue(ImageHeightProperty, value); }
		}

		public ImageSource Image
		{
			get { return (ImageSource)GetValue(ImageProperty); }
			set { SetValue(ImageProperty, value); }
		}
	}
}
