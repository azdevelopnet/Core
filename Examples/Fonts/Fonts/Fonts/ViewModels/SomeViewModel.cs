using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms.Core;

namespace Fonts
{
    public class FontItemRow
    {
        public int Row { get; set; }
        public FontItem Item1 { get; set; }
        public FontItem Item2 { get; set; }
        public FontItem Item3 { get; set; }
    }

    public class SomeViewModel : CoreViewModel
    {

        public ObservableCollection<FontItemRow> Items { get; set; } = new ObservableCollection<FontItemRow>();
        public FontType FontType { get; set; }

        public SomeViewModel()
        {

        }

        public override void OnViewMessageReceived(string key, object obj)
        {
        }


        public void BuildResourceList()
        {
            switch (FontType)
            {
                case FontType.FontAwesome:
                    Items = SomeLogic.GetFontList(FontAwesome.Icons, FontAwesome.FontFamily).ToObservable<FontItemRow>();
                    break;
                case FontType.EntypoPlus:
                    Items = SomeLogic.GetFontList(EntypoPlus.Icons, EntypoPlus.FontFamily).ToObservable<FontItemRow>();
                    break;
                case FontType.Ionicons:
                    Items = SomeLogic.GetFontList(Ionicons.Icons, Ionicons.FontFamily).ToObservable<FontItemRow>();
                    break;
                case FontType.Material:
                    Items = SomeLogic.GetFontList(Material.Icons, Material.FontFamily).ToObservable<FontItemRow>();
                    break;
                case FontType.Meteocons:
                    Items = SomeLogic.GetFontList(Meteocons.Icons, Meteocons.FontFamily).ToObservable<FontItemRow>();
                    break;
                case FontType.SimpleLineIcons:
                    Items = SomeLogic.GetFontList(SimpleLineIcons.Icons, SimpleLineIcons.FontFamily).ToObservable<FontItemRow>();
                    break;
                case FontType.Typicons:
                    Items = SomeLogic.GetFontList(Typicons.Icons, Typicons.FontFamily).ToObservable<FontItemRow>();
                    break;
                case FontType.WeatherIcons:
                    Items = SomeLogic.GetFontList(WeatherIcons.Icons, WeatherIcons.FontFamily).ToObservable<FontItemRow>();
                    break;
            }

        }
    }
}
