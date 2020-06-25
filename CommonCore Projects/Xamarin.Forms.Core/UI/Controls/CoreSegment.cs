using System;
using Xamarin.Forms;
using System.Linq;
using System.Collections.Generic;
using System.Windows.Input;
using System.ComponentModel;

namespace Xamarin.Forms.Core
{
    [DesignTimeVisible(true)]
    public class CoreSegmentControl : ContentView
    {
        private StackLayout _layout;

        public static readonly BindableProperty CommandProperty =
            BindableProperty.Create("Command",
                            typeof(ICommand),
                            typeof(CoreSegmentControl),
                            null);

        public static readonly BindableProperty ItemsSourceProperty =
            BindableProperty.Create(propertyName: "ItemsSource",
                    returnType: typeof(List<string>),
                    declaringType: typeof(CoreSegmentControl),
                    defaultValue: new List<string>(),
                    propertyChanged: ItemsSourceChangedEvent);

        public static readonly BindableProperty SelectedIndexProperty =
            BindableProperty.Create("SelectedIndex",
                                    typeof(int),
                                    typeof(CoreSegmentControl),
                                    0);

        public static readonly BindableProperty FontSizeProperty =
            BindableProperty.Create("FontSize",
                                    typeof(double),
                                    typeof(CoreSegmentControl),
                                    Device.GetNamedSize(NamedSize.Default, typeof(Label)));



        public static readonly BindableProperty BorderThicknessProperty =
            BindableProperty.Create("BorderThickness", typeof(int), typeof(CoreSegmentControl), 0);


        public static readonly BindableProperty BorderColorProperty =
            BindableProperty.Create("BorderColor", typeof(Color), typeof(CoreSegmentControl), Color.Black);



        public static readonly BindableProperty SelectedBackgroundProperty =
            BindableProperty.Create("SelectedBackground",
                                    typeof(Color),
                                    typeof(CoreSegmentControl),
                                    Color.Black);
        public static readonly BindableProperty SelectedTextColorProperty =
            BindableProperty.Create("SelectedTextColor",
                                    typeof(Color),
                                    typeof(CoreSegmentControl),
                                    Color.White);


        public static readonly BindableProperty UnselectedBackgroundProperty =
            BindableProperty.Create("UnselectedBackground",
                                    typeof(Color),
                                    typeof(CoreSegmentControl),
                                    Color.Gray);


        public static readonly BindableProperty UnselectedTextColorProperty =
            BindableProperty.Create("UnselectedTextColor",
                                    typeof(Color),
                                    typeof(CoreSegmentControl),
                                    Color.Black);

        public static readonly BindableProperty CornerRadiusProperty =
            BindableProperty.Create("CornerRadius",
                                    typeof(double),
                                    typeof(CoreSegmentControl),
                                    0.0);

        public static readonly BindableProperty FontFamilyProperty =
            BindableProperty.Create("FontFamily",
                                    typeof(string),
                                    typeof(CoreSegmentControl),
                                    Font.Default.FontFamily);

        public int SelectedIndex
        {
            get { return (int)this.GetValue(SelectedIndexProperty); }
            set { this.SetValue(SelectedIndexProperty, value); }
        }

        public List<string> ItemsSource
        {
            get { return (List<string>)this.GetValue(ItemsSourceProperty); }
            set
            {
                this.SetValue(ItemsSourceProperty, value);
                RenderControl();
            }
        }


        public double CornerRadius
        {
            get { return (double)GetValue(CornerRadiusProperty); }
            set { SetValue(CornerRadiusProperty, value); }
        }

        public ICommand Command
        {
            get { return (ICommand)GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }

        public Color SelectedBackground
        {
            get { return (Color)this.GetValue(SelectedBackgroundProperty); }
            set { this.SetValue(SelectedBackgroundProperty, value); }
        }
        public Color SelectedTextColor
        {
            get { return (Color)this.GetValue(SelectedTextColorProperty); }
            set { this.SetValue(SelectedTextColorProperty, value); }
        }
        public Color UnselectedBackground
        {
            get { return (Color)this.GetValue(UnselectedBackgroundProperty); }
            set { this.SetValue(UnselectedBackgroundProperty, value); }
        }
        public Color UnselectedTextColor
        {
            get { return (Color)this.GetValue(UnselectedTextColorProperty); }
            set { this.SetValue(UnselectedTextColorProperty, value); }
        }
        public double FontSize
        {
            get { return (double)this.GetValue(FontSizeProperty); }
            set { this.SetValue(FontSizeProperty, value); }
        }
        public string FontFamily
        {
            get { return (string)this.GetValue(FontFamilyProperty); }
            set { this.SetValue(FontFamilyProperty, value); }
        }

        public int BorderThickness
        {
            get { return (int)GetValue(BorderThicknessProperty); }
            set { SetValue(BorderThicknessProperty, value); }
        }

        public Color BorderColor
        {
            get { return (Color)GetValue(BorderColorProperty); }
            set { SetValue(BorderColorProperty, value); }
        }


        public static void ItemsSourceChangedEvent(BindableObject bindable, object oldValue, object newvalue)
        {
            ((CoreSegmentControl)bindable).RenderControl();
        }


        public void RenderControl()
        {
            if (ItemsSource != null)
            {
                _layout = new StackLayout() { Spacing = -1, Orientation = StackOrientation.Horizontal };
                for (int x = 0; x < ItemsSource.Count; x++)
                {
                    var seg = ItemsSource[x];
                    var cornerRadius = new CornerRadius(0);
                    var backgroundColor = Color.Default;
                    var textColor = Color.Default;

                    if (x == 0)
                    {
                        cornerRadius = new CornerRadius(this.CornerRadius, 0, this.CornerRadius, 0);
                    }
                    if (x == ItemsSource.Count - 1)
                    {
                        cornerRadius = new CornerRadius(0, this.CornerRadius, 0, this.CornerRadius);
                    }
                    if (x == SelectedIndex)
                    {
                        backgroundColor = SelectedBackground;
                        textColor = SelectedTextColor;
                    }
                    else
                    {
                        backgroundColor = UnselectedBackground;
                        textColor = UnselectedTextColor;
                    }

                    if (seg == ItemsSource.First())
                    {
                        cornerRadius = new CornerRadius(this.CornerRadius, 0, this.CornerRadius, 0);

                    }
                    if (seg == ItemsSource.Last())
                    {
                        cornerRadius = new CornerRadius(0, this.CornerRadius, 0, this.CornerRadius);
                    }

                    var view = new Xamarin.Forms.PancakeView.PancakeView()
                    {
                        CornerRadius = cornerRadius,
                        BorderColor = this.SelectedBackground,
                        BorderThickness = 1,
                        BackgroundColor = backgroundColor,
                        HorizontalOptions = LayoutOptions.FillAndExpand,
                        Content = new StackLayout()
                        {
                            Children =
                            {
                                new Label(){
                                    TextColor = textColor,
                                    Text = seg,
                                    FontFamily = this.FontFamily,
                                    FontSize = this.FontSize,
                                    VerticalOptions = LayoutOptions.FillAndExpand,
                                    HorizontalOptions = LayoutOptions.FillAndExpand,
                                    VerticalTextAlignment = TextAlignment.Center,
                                    HorizontalTextAlignment = TextAlignment.Center,

                                }
                            }
                        }
                    };
                    view.GestureRecognizers.Add(new TapGestureRecognizer()
                    {
                        Command = new Command(() => {
                            SegmentClicked(view);
                        })
                    });

                    _layout.Children.Add(view);


                }

                this.Content = _layout;
            }
        }

        private void SegmentClicked(Xamarin.Forms.PancakeView.PancakeView view)
        {
            var stackLayout = (StackLayout)this.Content;
            foreach (var item in stackLayout.Children)
            {
                var pancake = (Xamarin.Forms.PancakeView.PancakeView)item;
                var lbl = (Label)((StackLayout)pancake.Content).Children[0];
                if (pancake == view)
                {
                    SelectedIndex = stackLayout.Children.IndexOf(pancake);
                    pancake.BackgroundColor = this.SelectedBackground;
                    lbl.TextColor = this.SelectedTextColor;
                }
                else
                {
                    pancake.BackgroundColor = this.UnselectedBackground;
                    lbl.TextColor = this.UnselectedTextColor;
                }
            }

            Command?.Execute(SelectedIndex);

        }

    }
}


