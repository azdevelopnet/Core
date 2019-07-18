using System;
using Xamarin.Forms;
using System.Linq;
using System.Collections.Generic;
using System.Windows.Input;

namespace Xamarin.Forms.Core
{
    public class CoreSegmentItem
    {
        public string Text { get; set; }
        public ImageSource Image { get; set; }
    }

    public class CoreSegmentControl : ContentView, IDisposable
    {
        private Grid contentGrid;
        private List<TapGestureRecognizer> tapGestures = new List<TapGestureRecognizer>();

        public static readonly BindableProperty CommandProperty =
            BindableProperty.Create("Command",
                            typeof(ICommand),
                            typeof(CoreSegmentControl),
                            null);

        public static readonly BindableProperty ItemsSourceProperty =
            BindableProperty.Create(propertyName: "ItemsSource",
                    returnType: typeof(List<CoreSegmentItem>),
                    declaringType: typeof(CoreSegmentControl),
                    defaultValue: new List<CoreSegmentItem>(),
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


        public static readonly BindableProperty FontFamilyProperty =
            BindableProperty.Create("FontFamily",
                                    typeof(string),
                                    typeof(CoreSegmentControl),
                                    "");


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

        public int SelectedIndex
        {
            get { return (int)this.GetValue(SelectedIndexProperty); }
            set { this.SetValue(SelectedIndexProperty, value); }
        }

        public List<CoreSegmentItem> ItemsSource
        {
            get { return (List<CoreSegmentItem>)this.GetValue(ItemsSourceProperty); }
            set
            {
                this.SetValue(ItemsSourceProperty, value);
                RenderControl();
            }
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


        private void RenderControl()
        {
            if (ItemsSource != null)
            {
                if (contentGrid == null)
                {
                    contentGrid = new Grid()
                    {
                        ColumnSpacing = BorderThickness,
                        Padding = BorderThickness,
                        BackgroundColor = BorderColor
                    };
                }

                else
                {
                    foreach (var gesture in tapGestures)
                    {
                        gesture.Tapped -= SegmentClickEvent;
                    }
                    tapGestures.Clear();
                    contentGrid.Children.Clear();
                    contentGrid.ColumnDefinitions.Clear();
                }

                for (int x = 0; x < ItemsSource.Count; x++)
                {
                    var segmentItem = ItemsSource[x];
                    var tapGesture = new TapGestureRecognizer();
                    tapGesture.Tapped += SegmentClickEvent;
                    tapGestures.Add(tapGesture);

                    var pnl = new StackLayout()
                    {
                        BackgroundColor = SelectedIndex == x ? SelectedBackground : UnselectedBackground,
                        Orientation = StackOrientation.Horizontal
                    };
                    pnl.GestureRecognizers.Add(tapGesture);
                    pnl.Children.Add(new StackLayout() { HorizontalOptions = LayoutOptions.StartAndExpand });

                    contentGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = GridLength.Star });

                
                    if (segmentItem.Image != null)
                    {
                        var imgItem = new Image() {
                            Source = segmentItem.Image,
                            BackgroundColor = SelectedIndex == x ? SelectedBackground : UnselectedBackground,
                            HeightRequest = this.FontSize,
                            WidthRequest = this.FontSize
                        };
                        pnl.Children.Add(imgItem);
                    }


                    var segmentLabel = new Label()
                    {
                        FontSize = this.FontSize,
                        FontFamily = this.FontFamily,
                        Text = segmentItem.Text,
                        VerticalTextAlignment = TextAlignment.Center,
                        TextColor = SelectedIndex == x ? SelectedTextColor : UnselectedTextColor,
                        BackgroundColor = SelectedIndex == x ? SelectedBackground : UnselectedBackground,
                        HeightRequest = this.HeightRequest
                    };
                    pnl.Children.Add(segmentLabel);

                    pnl.Children.Add(new StackLayout() { HorizontalOptions = LayoutOptions.EndAndExpand });

                    contentGrid.AddChild(pnl, 0, x);
                }

                this.Content = contentGrid;
            }
        }

        private void SegmentClickEvent(object sender, EventArgs args)
        {
            for (int x = 0; x < contentGrid.Children.Count; x++)
            {
                var pnl = (StackLayout)contentGrid.Children[x];
                var item = pnl.Children.FirstOrDefault(p => p is Label);
                var img = pnl.Children.FirstOrDefault(p => p is Image);
                if (item is Label)
                {
                    var lbl = (Label)item;
                    if (pnl == sender)
                    {
                        pnl.BackgroundColor = SelectedBackground;
                        lbl.TextColor = SelectedTextColor;
                        lbl.BackgroundColor = SelectedBackground;
                        if (img != null)
                            img.BackgroundColor = SelectedBackground;

                        SelectedIndex = x;
                        Command?.Execute(x);
                    }
                    else
                    {
                        pnl.BackgroundColor = UnselectedBackground;
                        lbl.BackgroundColor = UnselectedBackground;
                        lbl.TextColor = UnselectedTextColor;
                        if (img != null)
                            img.BackgroundColor = UnselectedBackground;
                    }
                }
            }

        }

        public void Dispose()
        {
            if (contentGrid != null)
            {
                foreach (var gesture in tapGestures)
                {
                    gesture.Tapped -= SegmentClickEvent;
                }
                tapGestures.Clear();
                contentGrid.Children.Clear();
                contentGrid.ColumnDefinitions.Clear();
            }
        }

        ~CoreSegmentControl()
        {
            this.Dispose();
        }
    }
}

