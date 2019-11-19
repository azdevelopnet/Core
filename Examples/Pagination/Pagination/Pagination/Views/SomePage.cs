using System;
using Pagination.Views;
using Xamarin.Forms;
using Xamarin.Forms.Core;

namespace Pagination
{
    public class SomePage : CorePage<SomeViewModel>
    {
        private CoreListView _lstView;
        public SomePage()
        {
            this.Title = "Paginated";

            Content = new StackLayout()
            {
                Children = {
                    new CoreListView(ListViewCachingStrategy.RecycleElement)
                    {
                        HasUnevenRows = true,
                        ItemTemplate = new DataTemplate(typeof(DatatumCell)),
                        AutomationId = "lstView",
                        IsPullToRefreshEnabled = true,

                    }.Assign(out _lstView)
                        .Bind(CoreListView.IsRefreshingProperty, "IsRefreshing")
                        .Bind(CoreListView.RefreshCommandProperty, "RereshCommand")
                        .Bind(CoreListView.ItemsSourceProperty, "PaginatedData")
                        .Bind(CoreListView.LoadMoreCommandProperty, "LoadMorePaginatedData")
                        .Bind(CoreListView.SelectedItemProperty, "SelectedPagingatedUser"),
                    new Grid(){
                        ColumnDefinitions =
                        {
                            new ColumnDefinition(){ Width = new GridLength(2,GridUnitType.Star)},
                            new ColumnDefinition(){ Width = new GridLength(1,GridUnitType.Star)}
                        },
                        Children =
                        {

                            new Label()
                            {
                                Text="Left"
                            }.Row(0).Col(0),
                            new Label()
                            {
                                Text="Right"
                            }.Row(0).Col(1)
                        }

                    }

                }
            };
        }
    }
}