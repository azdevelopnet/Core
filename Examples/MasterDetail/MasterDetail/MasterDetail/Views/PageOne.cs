using System;
using MasterDetail.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Core;

namespace MasterDetail.Views
{
    public class PageOne : CorePage<PageViewModel>
    {
        public PageOne()
        {
            this.Title = "Page One";
            Content = new StackLayout()
            {
                Children = { new Label(){
                        Text = "Page Number 1",
                        Margin = 20
                    }}
            };
        }
    }
}
