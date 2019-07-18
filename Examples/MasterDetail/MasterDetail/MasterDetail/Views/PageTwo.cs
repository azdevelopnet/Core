using System;
using MasterDetail.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Core;
namespace MasterDetail.Views
{
    public class PageTwo : CorePage<PageViewModel>
    {
        public PageTwo()
        {
            this.Title = "Page Two";
            Content = new StackLayout()
            {
                Children = { new Label(){
                        Text = "Page Number 2",
                        Margin = 20
                    }}
            };
        }
    }
}
