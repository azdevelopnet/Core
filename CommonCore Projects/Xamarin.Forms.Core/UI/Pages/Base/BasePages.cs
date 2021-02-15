using System;
using System.Reflection;
using Xamarin.Essentials;

namespace Xamarin.Forms.Core
{
    public partial class BasePages : ContentPage
    {
        public Size ScreenSize
        {
            get { return CoreSettings.ScreenSize; }
        }

        //protected override bool OnBackButtonPressed()
        //{
        //    var bindingContext = BindingContext as CoreViewModel;
        //    var result = bindingContext?.OnBackButtonPressed() ?? base.OnBackButtonPressed();
        //    return result;
        //}

        //public void OnSoftBackButtonPressed()
        //{
        //    var bindingContext = BindingContext as CoreViewModel;
        //    bindingContext?.OnSoftBackButtonPressed();
        //}

        //public static readonly BindableProperty NeedOverrideSoftBackButtonProperty =
        //        BindableProperty.Create("NeedOverrideSoftBackButton", typeof(bool), typeof(BasePages), false);

        /// <summary>
        /// Enables the ability of the Pages view model to receive soft back button press events
        /// </summary>
        /// <value><c>true</c> if need override soft back button; otherwise, <c>false</c>.</value>
        //public bool NeedOverrideSoftBackButton
        //{
        //    get { return (bool)GetValue(NeedOverrideSoftBackButtonProperty); }
        //    set { SetValue(NeedOverrideSoftBackButtonProperty, value); }
        //}


#if __IOS__

        //      /// <summary>
        //      /// Override default settings for back button and removes the chevron images leaving just text.
        //      /// </summary>
        //      public static readonly BindableProperty OverrideBackButtonProperty =
        //          BindableProperty.Create("OverrideBackButton", typeof(bool), typeof(BasePages), false);

        //      /// <summary>
        //      /// Override default settings for back button and removes the chevron images leaving just text.
        //      /// </summary>
        //      /// <value><c>true</c> if override back button; otherwise, <c>false</c>.</value>
        //      public bool OverrideBackButton
        //      {
        //          get { return (bool)GetValue(OverrideBackButtonProperty); }
        //          set { SetValue(OverrideBackButtonProperty, value); }
        //      }

        //      /// <summary>
        //      /// The override back text property.
        //      /// </summary>
        //public static readonly BindableProperty OverrideBackTextProperty =
        //          BindableProperty.Create("OverrideBackText", typeof(string), typeof(BasePages), "Back");

        //      /// <summary>
        //      /// Gets or sets the override back text.
        //      /// </summary>
        //      /// <value>The override back text.</value>
        //public string OverrideBackText
        //      {
        //          get { return (string)GetValue(OverrideBackTextProperty); }
        //          set { SetValue(OverrideBackTextProperty, value); }
        //      }

#endif


        public void ScrollToElement(string scrollAutomationId, string elementAutomationId, ScrollToPosition position)
        {
            object scrollObj = null;
            ((ILayoutController)Content).FindViewByAutomationId(scrollAutomationId, ref scrollObj);

            if (scrollObj != null && scrollObj is ScrollView)
            {
                object elementObj = null;
                ((ILayoutController)Content).FindViewByAutomationId(elementAutomationId, ref elementObj);

                if (elementObj != null && elementObj is Element)
                {
                    var element = (Element)elementObj;
                    var scrollView = (ScrollView)scrollObj;
                    MainThread.BeginInvokeOnMainThread(async() => {
                        await scrollView.ScrollToAsync(element, position, true);
                    });
                    
                }
            }
        }

        public void FocusByAutomationId(string automationId)
        {
            object obj = null;
            ((ILayoutController)Content).FindViewByAutomationId(automationId, ref obj);
            ((VisualElement)obj)?.Focus();
        }

        public void CloseKeyboard()
        {
            CoreDependencyService.GetDependency<IKeyboardService>().Hide();
        }

    }

}
