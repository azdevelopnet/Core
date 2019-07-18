using System;
#if __ANDROID__
using Xamarin.Forms.PlatformConfiguration.AndroidSpecific;
#endif
namespace Xamarin.Forms.Core
{
    public class CoreTabbedPage : TabbedPage
    {
    
        public static readonly BindableProperty IsToolbarBottomProperty =
                BindableProperty.Create(propertyName: "IsToolbarBottom",
                returnType: typeof(bool),
                declaringType: typeof(CoreTabbedPage),
                defaultValue: false,
                propertyChanged: OnToolbarBottomPropertyChanged);

        public bool IsToolbarBottom
        {
            get { return (bool)this.GetValue(IsToolbarBottomProperty); }
            set { this.SetValue(IsToolbarBottomProperty, value); }
        }

        public static readonly BindableProperty SelectedForegroundColorProperty =
            BindableProperty.Create(propertyName: "SelectedForegroundColor",
                            returnType: typeof(Color),
                            declaringType: typeof(CoreTabbedPage),
                            defaultValue: Color.Black,
                            propertyChanged: OnSelectedForegroundColorChanged);

		public Color SelectedForegroundColor
		{
			get { return (Color)this.GetValue(SelectedForegroundColorProperty); }
			set { this.SetValue(SelectedForegroundColorProperty, value); }
		}

		public static readonly BindableProperty UnSelectedForegroundColorProperty =
			BindableProperty.Create(propertyName: "UnSelectedForegroundColor",
                             returnType: typeof(Color),
                             declaringType: typeof(CoreTabbedPage),
                             defaultValue: Color.Black,
                             propertyChanged: OnUnSelectedForegroundColorChanged);

		public Color UnSelectedForegroundColor
		{
			get { return (Color)this.GetValue(UnSelectedForegroundColorProperty); }
			set { this.SetValue(UnSelectedForegroundColorProperty, value); }
		}

		public static readonly BindableProperty TabBackgroundColorProperty =
			BindableProperty.Create(propertyName: "TabBackgroundColor",
                            returnType: typeof(Color),
                            declaringType: typeof(CoreTabbedPage),
                            defaultValue: Color.Default,
                            propertyChanged: OnTabBackgroundColorChanged);
        
		public Color TabBackgroundColor
		{
			get { return (Color)this.GetValue(TabBackgroundColorProperty); }
			set { this.SetValue(TabBackgroundColorProperty, value); }
		}

        public CoreTabbedPage()
        {

        }

        private static void OnTabBackgroundColorChanged(BindableObject bindable, object value, object newValue)
        {

#if __ANDROID__
            var coreTabePage = ((CoreTabbedPage)bindable);
            if (coreTabePage.IsToolbarBottom)
                coreTabePage.BarBackgroundColor = coreTabePage.TabBackgroundColor;
#endif

        }

        private static void OnSelectedForegroundColorChanged(BindableObject bindable, object value, object newValue)
        {

#if __ANDROID__
            var coreTabePage = ((CoreTabbedPage)bindable);
            if (coreTabePage.IsToolbarBottom)
                coreTabePage.SelectedTabColor = coreTabePage.SelectedForegroundColor;
            //coreTabePage.On<Xamarin.Forms.PlatformConfiguration.Android>().SetBarSelectedItemColor(coreTabePage.SelectedForegroundColor);
#endif

        }

        private static void OnUnSelectedForegroundColorChanged(BindableObject bindable, object value, object newValue)
        {

#if __ANDROID__
            var coreTabePage = ((CoreTabbedPage)bindable);
            
            if (coreTabePage.IsToolbarBottom)
                coreTabePage.UnselectedTabColor = coreTabePage.UnSelectedForegroundColor;
           // coreTabePage.On<Xamarin.Forms.PlatformConfiguration.Android>().SetBarItemColor(coreTabePage.UnSelectedForegroundColor);
#endif

        }

        private static void OnToolbarBottomPropertyChanged(BindableObject bindable, object value, object newValue)
        {

#if __ANDROID__
            var coreTabePage = ((CoreTabbedPage)bindable);

            coreTabePage.BarBackgroundColor = coreTabePage.TabBackgroundColor;

            coreTabePage.SelectedTabColor = coreTabePage.SelectedForegroundColor;
            coreTabePage.UnselectedTabColor = coreTabePage.UnSelectedForegroundColor;

            //coreTabePage.On<Xamarin.Forms.PlatformConfiguration.Android>().SetBarSelectedItemColor(coreTabePage.SelectedForegroundColor);
            //coreTabePage.On<Xamarin.Forms.PlatformConfiguration.Android>().SetBarItemColor(coreTabePage.UnSelectedForegroundColor);

            if (coreTabePage.IsToolbarBottom)
            {
                coreTabePage.On<Xamarin.Forms.PlatformConfiguration.Android>().SetToolbarPlacement(ToolbarPlacement.Bottom);
            }
            else
            {
                coreTabePage.On<Xamarin.Forms.PlatformConfiguration.Android>().SetToolbarPlacement(ToolbarPlacement.Default);
            }
#endif
        }
    }
}
