using System;
using MasterDetail.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Core;
namespace MasterDetail.Views
{
    public class PageThree : CorePage<PageViewModel>
    {
        public PageThree()
        {
            this.Title = "Page Thre";
            Content = new StackLayout()
            {
                Children = { new Label(){
                        Text = "Page Number 3",
                        Margin = 20
                    }}
            };
        }
    }
}
