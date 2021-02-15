using System;
using System.ComponentModel;
using Xamarin.Forms;

namespace Xamarin.Forms.Core
{

    [DesignTimeVisible(true)]
    public class CoreButton : Button, IDisposable
    {

        /// <summary>
        /// Start color for the gradient (top) color
        /// </summary>
        public static readonly BindableProperty StartColorProperty =
            BindableProperty.Create("StartColor",
                                    typeof(Color),
                                    typeof(CoreButton),
                                    Color.Black);
        public Color StartColor
        {
            get { return (Color)this.GetValue(StartColorProperty); }
            set { this.SetValue(StartColorProperty, value); }
        }

        /// <summary>
        /// End color for the gradient (bottom) color
        /// </summary>
        public static readonly BindableProperty EndColorProperty =
            BindableProperty.Create("EndColor",
                                    typeof(Color),
                                    typeof(CoreButton),
                                    Color.Black);
        public Color EndColor
        {
            get { return (Color)this.GetValue(EndColorProperty); }
            set { this.SetValue(EndColorProperty, value); }
        }



        public new static readonly BindableProperty CornerRadiusProperty =
            BindableProperty.Create("CornerRadius",
                                    typeof(float),
                                    typeof(CoreButton),
                                    0f);
        public new float CornerRadius
        {
            get { return (float)this.GetValue(CornerRadiusProperty); }
            set { this.SetValue(CornerRadiusProperty, value); }
        }

        /// <summary>
        /// Color that extends off the button as a shadow
        /// </summary>
        public static readonly BindableProperty ShadowColorProperty =
            BindableProperty.Create("ShadowColor",
                                    typeof(Color),
                                    typeof(CoreButton),
                                    Color.Black);
        public Color ShadowColor
        {
            get { return (Color)this.GetValue(ShadowColorProperty); }
            set { this.SetValue(ShadowColorProperty, value); }
        }

        /// <summary>
        /// The distance the shadow extends off of the button
        /// </summary>
        public static readonly BindableProperty ShadowOffsetProperty =
            BindableProperty.Create("ShadowOffset",
                                    typeof(float),
                                    typeof(CoreButton),
                                    0.0f);
        public float ShadowOffset
        {
            get { return (float)this.GetValue(ShadowOffsetProperty); }
            set { this.SetValue(ShadowOffsetProperty, value); }
        }

        /// <summary>
        /// The arc made at the edges of the dropshadow -- suggest making same as button
        /// </summary>
        public static readonly BindableProperty ShadowRadiusProperty =
            BindableProperty.Create("ShadowRadius",
                                    typeof(float),
                                    typeof(CoreButton),
                                    0.0f);
        public float ShadowRadius
        {
            get { return (float)this.GetValue(ShadowRadiusProperty); }
            set { this.SetValue(ShadowRadiusProperty, value); }
        }

        /// <summary>
        /// Opacity of the dropshadow behind the button
        /// </summary>
        public static readonly BindableProperty ShadowOpacityProperty =
            BindableProperty.Create("ShadowOpacity",
                                    typeof(float),
                                    typeof(CoreButton),
                                    0.0f);
        public float ShadowOpacity
        {
            get { return (float)this.GetValue(ShadowOpacityProperty); }
            set { this.SetValue(ShadowOpacityProperty, value); }
        }

        /// <summary>
        /// Color of the TintColor for Disabled button
        /// </summary>
        public static readonly BindableProperty DisabledTextColorProperty =
            BindableProperty.Create("DisabledTextColor",
                                    typeof(Color),
                                    typeof(CoreButton),
                                    Color.Gray);
        public Color DisabledTextColor
        {
            get { return (Color)this.GetValue(DisabledTextColorProperty); }
            set { this.SetValue(DisabledTextColorProperty, value); }
        }

        public CoreButton()
        {
            this.Clicked += ClickEvent;
        }
        private async void ClickEvent(object sender, EventArgs args)
        {
            await this.ScaleTo(0.98, 100, Easing.Linear);
            await this.ScaleTo(1, 100, Easing.Linear);
        }

        public void Dispose()
        {
            this.Clicked -= ClickEvent;
        }
        ~CoreButton()
        {
            this.Clicked -= ClickEvent;
        }
    }
}

