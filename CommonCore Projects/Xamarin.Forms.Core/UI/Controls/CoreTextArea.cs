using System;
using System.ComponentModel;
using Xamarin.Forms;

namespace Xamarin.Forms.Core
{
    [DesignTimeVisible(true)]
    public class CoreTextArea : Label
    {
        public static readonly BindableProperty LinksEnabledProperty =
            BindableProperty.Create("LinksEnabled",
                                    typeof(bool),
                                    typeof(CoreTextArea),
                                    false);
        public bool LinksEnabled
        {
            get { return (bool)this.GetValue(LinksEnabledProperty); }
            set { this.SetValue(LinksEnabledProperty, value); }
        }
    }
}

