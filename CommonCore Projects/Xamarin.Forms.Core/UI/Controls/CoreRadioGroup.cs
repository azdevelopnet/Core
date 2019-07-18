using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Input;

namespace Xamarin.Forms.Core
{
    public class CoreRadioGroup: StackLayout
    {
        public List<CoreRadioButton> rads;

		public static readonly BindableProperty ImageColorProperty =
        				BindableProperty.Create(propertyName: "ImageColor",
        				returnType: typeof(Color),
        				declaringType: typeof(CoreRadioGroup),
        				defaultValue: Color.Black,
        				propertyChanged: OnImageColorChanged);

		public static readonly BindableProperty FontFamilyProperty =
        				  BindableProperty.Create(propertyName: "FontFamily",
                          returnType: typeof(string),
                          declaringType: typeof(CoreRadioGroup),
                          defaultValue: Font.Default.FontFamily,
        				  propertyChanged: OnFontFamilyChanged);

        public static readonly BindableProperty FontSizeProperty =
        				BindableProperty.Create(propertyName: "FontSize",
        				returnType: typeof(double),
        				declaringType: typeof(CoreRadioGroup),
        				defaultValue: Device.GetNamedSize(NamedSize.Default, typeof(Label)),
        				propertyChanged: OnFontSizeChanged);

		public static readonly BindableProperty TextColorProperty =
						BindableProperty.Create(propertyName: "TextColor",
						returnType: typeof(Color),
						declaringType: typeof(CoreRadioGroup),
						defaultValue: Color.Black,
                        propertyChanged: OnTextColorChanged);

		public static readonly BindableProperty CheckedCommandProperty =
	                    BindableProperty.Create("CheckedCommand",
						typeof(ICommand),
						typeof(CoreRadioGroup),
						null);
        
		public static readonly BindableProperty UnSelectedImageProperty =
						BindableProperty.Create(propertyName: "UnSelectedImage",
						returnType: typeof(string),
						declaringType: typeof(CoreRadioGroup),
						defaultValue: null,
						propertyChanged: OnUnSelectedImageChanged);

		public static readonly BindableProperty SelectedImageProperty =
						BindableProperty.Create(propertyName: "SelectedImage",
						returnType: typeof(string),
						declaringType: typeof(CoreRadioGroup),
						defaultValue: null,
						propertyChanged: OnSelectedImageChanged);

		public static readonly BindableProperty ItemsSourceProperty =
	                    BindableProperty.Create(propertyName: "ItemsSource",
    				    returnType: typeof(IEnumerable),
    				    declaringType: typeof(CoreRadioGroup),
    				    defaultValue: null,
    				    propertyChanged: OnItemsSourceChanged);

		public static readonly BindableProperty SelectedIndexProperty =
        				BindableProperty.Create(propertyName: "SelectedIndex",
        				returnType: typeof(int),
        				declaringType: typeof(CoreRadioGroup),
        				defaultValue: -1,
                        defaultBindingMode: BindingMode.TwoWay,
        				propertyChanged: OnSelectedIndexChanged);

		public Color ImageColor
		{
			get { return (Color)GetValue(ImageColorProperty); }
			set { SetValue(ImageColorProperty, value); }
		}

        public string FontFamily
		{
			get { return (string)GetValue(FontFamilyProperty); }
			set { SetValue(FontFamilyProperty, value); }
		}

        public double FontSize
		{
			get { return (double)GetValue(FontSizeProperty); }
			set { SetValue(FontSizeProperty, value); }
		}

        public Color TextColor
		{
			get { return (Color)GetValue(TextColorProperty); }
			set { SetValue(TextColorProperty, value); }
		}

        public ICommand CheckedCommand
		{
			get { return (ICommand)this.GetValue(CheckedCommandProperty); }
			set { this.SetValue(CheckedCommandProperty, value); }
		}

		public string UnSelectedImage
		{
			get { return (string)GetValue(UnSelectedImageProperty); }
			set { SetValue(UnSelectedImageProperty, value); }
		}

		public string SelectedImage
		{
			get { return (string)GetValue(SelectedImageProperty); }
			set { SetValue(SelectedImageProperty, value); }
		}

        public IEnumerable ItemsSource
        {
            get { return (IEnumerable)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }

      
        public int SelectedIndex
        {
            get { return (int)GetValue(SelectedIndexProperty); }
            set { SetValue(SelectedIndexProperty, value); }
        }

		public CoreRadioGroup()
		{
			rads = new List<CoreRadioButton>();
		}

		private static void OnImageColorChanged(BindableObject bindable, object oldValue, object newValue)
		{
			var radButtons = bindable as CoreRadioGroup;
			foreach (CoreRadioButton btn in radButtons.Children)
			{
				btn.ImageColor = (Color)newValue;
			}
		}

		private static void OnFontFamilyChanged(BindableObject bindable, object oldValue, object newValue)
		{
			var radButtons = bindable as CoreRadioGroup;
			foreach (CoreRadioButton btn in radButtons.Children)
			{
				btn.FontFamily = (string)newValue;
			}
		}

		private static void OnFontSizeChanged(BindableObject bindable, object oldValue, object newValue)
		{
			var radButtons = bindable as CoreRadioGroup;
			foreach (CoreRadioButton btn in radButtons.Children)
			{
				btn.FontSize = (double)newValue;
			}
		}

		private static void OnTextColorChanged(BindableObject bindable, object oldValue, object newValue)
		{
			var radButtons = bindable as CoreRadioGroup;
			foreach (CoreRadioButton btn in radButtons.Children)
			{
				btn.TextColor = (Color)newValue;
			}
		}

        private static void OnUnSelectedImageChanged(BindableObject bindable, object oldValue, object newValue)
        {
			var radButtons = bindable as CoreRadioGroup;
			foreach (CoreRadioButton btn in radButtons.Children)
			{
				btn.UnSelectedImage = (string)newValue;
			}
        }
		private static void OnSelectedImageChanged(BindableObject bindable, object oldValue, object newValue)
		{
            var radButtons = bindable as CoreRadioGroup;
            foreach(CoreRadioButton btn in radButtons.Children)
            {
                btn.SelectedImage = (string)newValue;
            }
		}

        private static void OnItemsSourceChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var radButtons = bindable as CoreRadioGroup;
           
            radButtons.rads.Clear();
            radButtons.Children.Clear();
            if (newValue != null)
            {
              
                int radIndex = 0;
                foreach (var item in (IEnumerable)newValue)
                {
                    var rad = new CoreRadioButton();
                    rad.SelectedImage = radButtons.SelectedImage;
                    rad.UnSelectedImage = radButtons.UnSelectedImage;;
                    rad.TextColor = radButtons.TextColor;
                    rad.FontSize = radButtons.FontSize;
                    rad.ImageColor = radButtons.ImageColor;
                    rad.Text = item.ToString();
                    rad.RadioButtonId = radIndex;

                    if(radButtons.SelectedIndex!=-1 && radButtons.SelectedIndex==radIndex)
                    {
                        rad.Checked = true;
                    }

                    rad.CheckedChanged += radButtons.OnCheckedChanged;

					radButtons.rads.Add(rad);
                                    
                    radButtons.Children.Add(rad);
                    radIndex++;
                }
            }
        }

        private void OnCheckedChanged(object sender, bool args)
        {
           
           if (!args) return;

            var selectedRad = sender as CoreRadioButton;

            foreach (var rad in rads)
            {
                if(!selectedRad.RadioButtonId.Equals(rad.RadioButtonId))
                {
                    rad.Checked = false;
                }
                else
                {
                    SelectedIndex = rad.RadioButtonId;
                    CheckedCommand?.Execute(rad.RadioButtonId);
                }
                
            }

        }

        private static void OnSelectedIndexChanged(BindableObject bindable, object value, object newvalue)
        {
            if ((int)newvalue == -1) return;

            var bindableRadioGroup = bindable as CoreRadioGroup;


            foreach (var rad in bindableRadioGroup.rads)
            {
                if (rad.RadioButtonId == bindableRadioGroup.SelectedIndex)
                {
                    rad.Checked = true;
                }
               
            }

        }
    
    }
}
