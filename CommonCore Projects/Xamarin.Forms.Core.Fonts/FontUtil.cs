using System;
namespace Xamarin.Forms.Core
{
    public enum FontType
    {
        FontAwesome,
        EntypoPlus,
        Ionicons,
        Material,
        Meteocons,
        SimpleLineIcons,
        Typicons,
        WeatherIcons
    }

    public class FontItem
    {
        public string FriendlyName { get; set; }
        public string Unicode { get; set; }
        public string FontFamily { get; set; }
    }

    public class FontUtil
    {
        public static FontItem GetFont(string friendlyName, FontType fontType)
        {
            var f = new FontItem()
            {
                FriendlyName = friendlyName
            };
            switch(fontType)
            {
                case FontType.EntypoPlus:
                    f.Unicode= EntypoPlus.Icons[friendlyName].ToString();
                    f.FontFamily = EntypoPlus.FontFamily;
                    break;
                case FontType.FontAwesome:
                    f.Unicode = FontAwesome.Icons[friendlyName].ToString();
                    f.FontFamily = FontAwesome.FontFamily;
                    break;
                case FontType.Ionicons:
                    f.Unicode = Ionicons.Icons[friendlyName].ToString();
                    f.FontFamily = Ionicons.FontFamily;
                    break;
                case FontType.Material:
                    f.Unicode = Material.Icons[friendlyName].ToString();
                    f.FontFamily = Material.FontFamily;
                    break;
                case FontType.Meteocons:
                    f.Unicode = Meteocons.Icons[friendlyName].ToString();
                    f.FontFamily = Meteocons.FontFamily;
                    break;
                case FontType.SimpleLineIcons:
                    f.Unicode = SimpleLineIcons.Icons[friendlyName].ToString();
                    f.FontFamily = SimpleLineIcons.FontFamily;
                    break;
                case FontType.Typicons:
                    f.Unicode = Typicons.Icons[friendlyName].ToString();
                    f.FontFamily = Typicons.FontFamily;
                    break;
                case FontType.WeatherIcons:
                    f.Unicode = WeatherIcons.Icons[friendlyName].ToString();
                    f.FontFamily = WeatherIcons.FontFamily;
                    break;
            }
            return f;
        }
    }
}
