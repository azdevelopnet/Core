using System;
using Fonts.Views;
using Xamarin.Forms;
using Xamarin.Forms.Core;

namespace Fonts
{
    public class SomePage : CorePage<SomeViewModel>
    {
        public SomePage()
        {
            this.Title = "Fonts";

            var fontAwesome = new CoreButton()
            {
                Text = "FontAwesome",
                Style = CoreStyles.LightOrange,
                Command = new Command((obj) =>
                {
                    VM.FontType = FontType.FontAwesome;
                    VM.BuildResourceList();
                    Navigation.PushNonAwaited<FontsCollectionView>();
                })
            };

            var entypoPlus = new CoreButton()
            {
                Text = "EntypoPlus",
                Style = CoreStyles.LightOrange,
                Command = new Command((obj) =>
                {
                    VM.FontType = FontType.EntypoPlus;
                    VM.BuildResourceList();
                    Navigation.PushNonAwaited<FontsCollectionView>();
                })
            };

            var ionicons = new CoreButton()
            {
                Text = "Ionicons",
                Style = CoreStyles.LightOrange,
                Command = new Command((obj) =>
                {
                    VM.FontType = FontType.Ionicons;
                    VM.BuildResourceList();
                    Navigation.PushNonAwaited<FontsCollectionView>();
                })
            };

            var material = new CoreButton()
            {
                Text = "Material",
                Style = CoreStyles.LightOrange,
                Command = new Command((obj) =>
                {
                    VM.FontType = FontType.Material;
                    VM.BuildResourceList();
                    Navigation.PushNonAwaited<FontsCollectionView>();
                })
            };

            var meteocons = new CoreButton()
            {
                Text = "Meteocons",
                Style = CoreStyles.LightOrange,
                Command = new Command((obj) =>
                {
                    VM.FontType = FontType.Meteocons;
                    VM.BuildResourceList();
                    Navigation.PushNonAwaited<FontsCollectionView>();
                })
            };

            var simpleLineIcons = new CoreButton()
            {
                Text = "SimpleLineIcons",
                Style = CoreStyles.LightOrange,
                Command = new Command((obj) =>
                {
                    VM.FontType = FontType.SimpleLineIcons;
                    VM.BuildResourceList();
                    Navigation.PushNonAwaited<FontsCollectionView>();
                })
            };

            var typicons = new CoreButton()
            {
                Text = "Typicons",
                Style = CoreStyles.LightOrange,
                Command = new Command((obj) =>
                {
                    VM.FontType = FontType.Typicons;
                    VM.BuildResourceList();
                    Navigation.PushNonAwaited<FontsCollectionView>();
                })
            };

            var weatherIcons = new CoreButton()
            {
                Text = "WeatherIcons",
                Style = CoreStyles.LightOrange,
                Command = new Command((obj) =>
                {
                    VM.FontType = FontType.WeatherIcons;
                    VM.BuildResourceList();
                    Navigation.PushNonAwaited<FontsCollectionView>();
                })
            };

            Content = new StackContainer(true)
            {
                Padding = 20,
                Spacing = 10,
                Children = { fontAwesome, entypoPlus, ionicons, material, meteocons, simpleLineIcons, typicons, weatherIcons }
            };
        }
    }
}