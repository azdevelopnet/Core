using System;
using System.ComponentModel;

namespace Xamarin.Forms.Core
{
    [DesignTimeVisible(true)]
    public class CoreCardView : ContentView
    {
        public static readonly BindableProperty CornerRadiusProperty =
            BindableProperty.Create("CornerRadius",
                                    typeof(float),
                                    typeof(CoreCardView),
                                    0.0f);
        public float CornerRadius
        {
            get { return (float)GetValue(CornerRadiusProperty); }
            set { SetValue(CornerRadiusProperty, value); }
        }

        public CoreCardView()
        {
            this.BackgroundColor = Color.White;
        }

    }
}
