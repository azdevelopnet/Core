using System;
using Xamarin.Forms;
using Xamarin.Forms.Core;

namespace CoreReferenceExample
{
    public class UIPage: CorePage<AuthViewModel>
    {
        public UIPage()
        {
            this.BackgroundColor = Color.White;
            NavigationPage.SetHasBackButton(this, false);
            NavigationPage.SetTitleView(this, new PageBanner("UI Page"));

            Content = new StackLayout()
            {
                Children=
                {
                 
                }
            }.IsHeadless();
        }
    }
}
