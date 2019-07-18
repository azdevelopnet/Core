using System;
using Pagination.Views;
using Xamarin.Forms;
using Xamarin.Forms.Core;

namespace Pagination
{
    public class SomePage : CorePage<SomeViewModel>
    {
        public SomePage()
        {
            this.Title = "Paginated";

            var lstView = new CoreListView(ListViewCachingStrategy.RecycleElement)
            {
                HasUnevenRows = true,
                ItemTemplate = new DataTemplate(typeof(DatatumCell)),
                AutomationId = "lstView",
                IsPullToRefreshEnabled = true,
               
            };
            lstView.SetBinding(CoreListView.IsRefreshingProperty, "IsRefreshing");
            lstView.SetBinding(CoreListView.RefreshCommandProperty, "RereshCommand");
            lstView.SetBinding(CoreListView.ItemsSourceProperty, "PaginatedData");
            lstView.SetBinding(CoreListView.LoadMoreCommandProperty, "LoadMorePaginatedData");
            lstView.SetBinding(CoreListView.SelectedItemProperty, "SelectedPagingatedUser");

            Content = new StackLayout()
            {
                Children = { lstView }
            };
        }
    }
}