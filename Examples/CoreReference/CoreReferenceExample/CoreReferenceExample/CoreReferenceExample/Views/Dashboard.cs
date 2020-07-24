using System;
using Xamarin.Forms;
using Xamarin.Forms.Core;
using CoreReferenceExample.ViewModels;

using Xamarin.Forms.Markup;
using static Xamarin.Forms.Markup.GridRowsColumns;

namespace CoreReferenceExample
{
    public class Dashboard : CorePage<DashboardViewModel>
    {
        enum GridRows { Row1, Row2}

        public Dashboard()
        {
            this.Title = "Dashboard";
            this.BackgroundImageSource = ImageSource.FromFile("background.png");
            NavigationPage.SetHasNavigationBar(this, false);

            Content = new Grid()
            {
                //RowDefinitions = RowCreate.Define("*","*"),
                Children =
                {
                    new Grid()
                    {
                        Margin = new Thickness(5,0,5,0),
                        HorizontalOptions = LayoutOptions.Center,
                        RowDefinitions = Rows.Define
                        (
                            (GridRows.Row1,120),
                            (GridRows.Row2,120)
                        ),
                        Children =
                        {
                            new DashboardButton(Fontawesome.Table, "Data", () =>
                            {
                                VM.ShowLoadingDialog("We are currently loading data from the server...");

                            }).Row(0).Column(0),
                            new DashboardButton(Fontawesome.Pencil, "UI", async() =>
                            {
                                await Navigation.PushAsync(new UIPage());
                            }).Row(0).Column(1),
                            new DashboardButton(Fontawesome.Comment, "Push", () =>
                            {

                            }).Row(0).Column(2),
                            new DashboardButton(Fontawesome.User, "Auth", () =>
                            {

                            }).Row(1).Column(0)
                        }
                    }.Row(1).IsHeadless(),
                }
            };
        }
    }
}
