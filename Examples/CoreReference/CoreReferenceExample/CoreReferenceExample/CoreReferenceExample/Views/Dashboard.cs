using System;
using Xamarin.Forms;
using Xamarin.Forms.Core;
using RowCreate = Xamarin.Forms.Core.GridRowsAndColumns.Rows;
using ColCreate = Xamarin.Forms.Core.GridRowsAndColumns.Columns;
using CoreReferenceExample.ViewModels;

namespace CoreReferenceExample
{
    public class Dashboard : CorePage<DashboardViewModel>
    {
        public Dashboard()
        {
            this.Title = "Dashboard";
            this.BackgroundImageSource = ImageSource.FromFile("background.png");
            NavigationPage.SetHasNavigationBar(this, false);

            Content = new Grid()
            {
                RowDefinitions = RowCreate.Define("*","*"),
                Children =
                {
                    new Grid()
                    {
                        Margin = new Thickness(5,0,5,0),
                        HorizontalOptions = LayoutOptions.Center,
                        RowDefinitions = RowCreate.Define("120","120"),
                        Children =
                        {
                            new DashboardButton(Fontawesome.Table, "Data", () =>
                            {
                                VM.ShowLoadingDialog("We are currently loading data from the server...");

                            }).Row(0).Col(0),
                            new DashboardButton(Fontawesome.Pencil, "UI", async() =>
                            {
                                await Navigation.PushAsync(new UIPage());
                            }).Row(0).Col(1),
                            new DashboardButton(Fontawesome.Comment, "Push", () =>
                            {

                            }).Row(0).Col(2),
                            new DashboardButton(Fontawesome.User, "Auth", () =>
                            {

                            }).Row(1).Col(0)
                        }
                    }.Row(1).IsHeadless(),
                }
            };
        }
    }
}
