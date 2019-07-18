namespace Xamarin.Forms.Core
{
    public partial class CoreStyles
    {
        public static Style RandomUserCollection { get; } = new Style(typeof(ContentView))
        {
            Setters =
            {
                new Setter(){ Property= VisualStateManager.VisualStateGroupsProperty,  Value= new VisualStateGroupList()
                    {
                         new VisualStateGroup()
                         {
                            Name="CommonStates",
                            States = {
                                new VisualState(){ Name="Normal"},
                                new VisualState(){ Name="Selected",
                                    Setters={
                                        new Setter() { Property= ContentView.BackgroundColorProperty, Value= Color.FromHex("#DF8049")} }
                                    }
                             }
                         }
                    }
                }

            }
        };
        public static Style CardTitle { get; } = new Style(typeof(Span))
        {
            Setters =
            {
                new Setter(){Property = Span.FontSizeProperty,Value = 18},
                new Setter(){Property = Span.FontAttributesProperty, Value=FontAttributes.Bold},
                new Setter(){Property=Span.TextColorProperty, Value = Color.Black}
            }
        };
        public static Style LightOrange { get; } = new Style(typeof(CoreButton))
        {
            Setters =
            {
                new Setter(){Property=CoreButton.StartColorProperty ,Value=Color.FromHex("#DF8049")},
                new Setter(){Property=CoreButton.EndColorProperty ,Value=Color.FromHex("#E8A47D")},
                new Setter(){Property=CoreButton.ShadowColorProperty ,Value=Color.Gray},
                new Setter(){Property=CoreButton.TextColorProperty ,Value=Color.White},
                new Setter(){Property=CoreButton.ShadowOffsetProperty ,Value=1},
                new Setter(){Property=CoreButton.ShadowOpacityProperty ,Value=1},
                new Setter(){Property=CoreButton.ShadowRadiusProperty ,Value= CoreSettings.On<float>(6f,10f,6f)},
                new Setter(){Property=CoreButton.CornerRadiusProperty ,Value= CoreSettings.On<float>(6f,10f,6f)},
            }
        };

        public static Style AddressCell { get; } = new Style(typeof(Label))
        {
            Setters =
            {
                new Setter(){Property=Label.TextProperty ,Value=Color.Gray},
                new Setter(){Property=Label.FontSizeProperty ,Value=12},
                new Setter(){Property=Label.MarginProperty ,Value=new Thickness(5,0,2,0)}
            }
        };


        public static Style FontLabel { get; } = new Style(typeof(Label))
        {
            Setters =
            {
                new Setter(){Property=Label.TextProperty ,Value=Color.Gray},
                new Setter(){Property=Label.FontSizeProperty ,Value=24},
                new Setter(){Property=Label.MarginProperty ,Value=10},
                new Setter(){Property=Label.HorizontalOptionsProperty ,Value=LayoutOptions.Center},
                new Setter(){Property=Label.HorizontalTextAlignmentProperty ,Value=TextAlignment.Center},
                new Setter(){Property=Label.FontFamilyProperty ,Value=CoreSettings.On<string>("Boxise","BoxiseFont.otf#Boxise")}
            }
        };

    }
}
