using System;
using System.ComponentModel;
using Xamarin.Forms;

namespace Xamarin.Forms.Core
{
    [DesignTimeVisible(true)]
    public class CoreContentView : ContentView
    {
        public static readonly BindableProperty CornerRadiusProperty =
            BindableProperty.Create("CornerRadius", typeof(double), typeof(CoreContentView), 0.0);

        public double CornerRadius
        {
            get { return (double)GetValue(CornerRadiusProperty); }
            set { SetValue(CornerRadiusProperty, value); }
        }
    }
}

