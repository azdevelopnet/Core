using System;
using Xamarin.Forms.Core;
using Xamarin.Forms;

namespace Xamarin.Forms.Core
{
	public partial class CoreStyles
	{
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
        
	}
}
