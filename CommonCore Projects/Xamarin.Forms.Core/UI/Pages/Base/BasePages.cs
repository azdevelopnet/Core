using System;
using System.Collections.Generic;
using System.Reflection;

namespace Xamarin.Forms.Core
{
    /// <summary>
    /// https://channel9.msdn.com/Events/Xamarin/Xamarin-Developer-Summit-2019/Thinking-outside-of-the-box-with-XamarinForms
    /// https://www.thewissen.io/create-a-kickass-banking-app-using-a-basepage-in-xamarin/
    /// </summary>
    public abstract class BlankPage : ContentPage
    {
        public StackLayout TitleBarLeftContent;
        public StackLayout TitleBarRightContent;
        public StackLayout TitleBarMiddleContent;
        public Grid BaseContentGrid;

        public BlankPage()
        {
            NavigationPage.SetHasNavigationBar(this, false);

            float deviceDetailRowHeight = 0;

#if __IOS__
            deviceDetailRowHeight =  (float)UIKit.UIApplication.SharedApplication.StatusBarFrame.Height;
#endif

            var titleBarColor = Color.Blue;// ((NavigationPage)Application.Current.MainPage).BarBackgroundColor;

            Content = new Grid()
            {
                RowDefinitions =
                {
                    new RowDefinition(){Height = new GridLength(deviceDetailRowHeight,GridUnitType.Absolute)},
                    new RowDefinition(){Height = new GridLength(44,GridUnitType.Absolute)},
                    new RowDefinition(){Height = new GridLength(1,GridUnitType.Star)},
                },
                Children =
                {
                    new Grid()
                    {
                        BackgroundColor = titleBarColor,
                        RowSpacing=0,
                        ColumnSpacing=0,
                        ColumnDefinitions =
                        {
                            new ColumnDefinition(){Width= new GridLength(50,GridUnitType.Absolute)},
                            new ColumnDefinition(){Width= new GridLength(1,GridUnitType.Star)},
                            new ColumnDefinition(){Width= new GridLength(50,GridUnitType.Absolute)},
                        },
                        Children =
                        {
                            new StackLayout(){ Orientation = StackOrientation.Horizontal}.Assign(out TitleBarLeftContent).Col(0),
                            new StackLayout(){ Orientation = StackOrientation.Horizontal}.Assign(out TitleBarMiddleContent).Col(1),
                            new StackLayout(){ Orientation = StackOrientation.Horizontal}.Assign(out TitleBarRightContent).Col(2),
                        }
                    }.Row(1),
                    new Grid()
                    {
                        RowSpacing=0,
                        ColumnSpacing=0,
                        VerticalOptions = LayoutOptions.FillAndExpand,
                        HorizontalOptions = LayoutOptions.FillAndExpand,
                    }.Assign(out BaseContentGrid).Row(2)
                }
            };
        }
    }

    public partial class BasePages : ContentPage
    {
        public Size ScreenSize
        {
            get { return CoreSettings.ScreenSize; }
        }


        protected override bool OnBackButtonPressed()
        {
            var bindingContext = BindingContext as CoreViewModel;
            var result = bindingContext?.OnBackButtonPressed() ?? base.OnBackButtonPressed();
            return result;
        }

        public void OnSoftBackButtonPressed()
        {
            var bindingContext = BindingContext as CoreViewModel;
            bindingContext?.OnSoftBackButtonPressed();
        }

        public static readonly BindableProperty NeedOverrideSoftBackButtonProperty =
                BindableProperty.Create("NeedOverrideSoftBackButton", typeof(bool), typeof(BasePages), false);

        /// <summary>
        /// Enables the ability of the Pages view model to receive soft back button press events
        /// </summary>
        /// <value><c>true</c> if need override soft back button; otherwise, <c>false</c>.</value>
        public bool NeedOverrideSoftBackButton
        {
            get { return (bool)GetValue(NeedOverrideSoftBackButtonProperty); }
            set { SetValue(NeedOverrideSoftBackButtonProperty, value); }
        }


#if __IOS__

        /// <summary>
        /// Override default settings for back button and removes the chevron images leaving just text.
        /// </summary>
        public static readonly BindableProperty OverrideBackButtonProperty =
            BindableProperty.Create("OverrideBackButton", typeof(bool), typeof(BasePages), false);

        /// <summary>
        /// Override default settings for back button and removes the chevron images leaving just text.
        /// </summary>
        /// <value><c>true</c> if override back button; otherwise, <c>false</c>.</value>
        public bool OverrideBackButton
        {
            get { return (bool)GetValue(OverrideBackButtonProperty); }
            set { SetValue(OverrideBackButtonProperty, value); }
        }

        /// <summary>
        /// The override back text property.
        /// </summary>
		public static readonly BindableProperty OverrideBackTextProperty =
            BindableProperty.Create("OverrideBackText", typeof(string), typeof(BasePages), "Back");

        /// <summary>
        /// Gets or sets the override back text.
        /// </summary>
        /// <value>The override back text.</value>
		public string OverrideBackText
        {
            get { return (string)GetValue(OverrideBackTextProperty); }
            set { SetValue(OverrideBackTextProperty, value); }
        }

#endif

        protected override void OnAppearing()
        {
            if (Navigation != null)
                CoreSettings.AppNav = Navigation;
            base.OnAppearing();
        }

    }

}
