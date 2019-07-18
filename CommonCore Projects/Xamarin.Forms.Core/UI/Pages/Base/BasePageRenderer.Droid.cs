#if __ANDROID__
using Android.App;
using Android.Content;
using Android.Views;
using Android.Widget;
using Xamarin.Forms;
using Xamarin.Forms.Core;
using Xamarin.Forms.Platform.Android;
using Ctx = Android.Content.Context;

[assembly: ExportRenderer(typeof(BasePages), typeof(BasePageRenderer))]
namespace Xamarin.Forms.Core
{
    /// <summary>
    /// Page renderer that allows search view to be displayed when view model implements the ISearchProvider interface
    /// </summary>
	public class BasePageRenderer : PageRenderer
	{
		private ISearchProvider _searchProvider;
		private SearchView _searchView;
        private readonly Ctx context;

        public BasePageRenderer(Context ctx):base(ctx)
        {
            context = ctx;
        }
        protected override void OnElementChanged(ElementChangedEventArgs<Page> e)
		{
			base.OnElementChanged(e);

            if(e.OldElement!=null)
            {


            }
            if(Element!=null && Element is ContentPage){
                if (_searchView == null)
                {
                    var contentPage = Element as ContentPage;
                    ConnectSearchView();
                    contentPage.Appearing += (s, a) => HandlePageReappearing();
                    contentPage.Disappearing += (s, a) => HandlePageDisappearing();

                }
   
            }

		}

        protected override void OnDetachedFromWindow()
        {
			if (_searchView != null)
			{
				_searchView.QueryTextSubmit -= HandleQueryTextSubmit;
			}
            base.OnDetachedFromWindow();
        }

		private void HandlePageDisappearing()
		{
			if (_searchView != null)
			{
				_searchView.QueryTextSubmit -= HandleQueryTextSubmit;
			}
		}

        private void HandlePageReappearing()
        {
            if(_searchView!=null && Element!=null && Element is ContentPage)
            {
                var contentPage = Element as ContentPage;
				_searchProvider = contentPage.BindingContext as ISearchProvider;

				if (_searchProvider == null)
				{
					_searchView.Visibility = ViewStates.Gone;
					return;
				}
            }

        }

		private void ConnectSearchView()
		{
            var ctx = (Activity)context;

            if(CoreSettings.SearchView!=0)
			    _searchView = ctx?.FindViewById<SearchView>(CoreSettings.SearchView);

			if (_searchView == null) 
                return;

			var contentPage = Element as ContentPage;

			if (contentPage == null)
			{
				_searchView.Visibility = ViewStates.Gone;
				return;
			}

			_searchProvider = contentPage.BindingContext as ISearchProvider;

			if (_searchProvider == null)
			{
				_searchView.Visibility = ViewStates.Gone;
				return;
			}

            if (!string.IsNullOrEmpty(_searchProvider.QueryHint))
            {
                _searchView.SetQueryHint(_searchProvider.QueryHint);
            }

			_searchView.QueryTextSubmit += HandleQueryTextSubmit;

			if (_searchProvider.SearchIsDefaultAction)
			{
				_searchView.OnActionViewExpanded();
			}
			else
			{
				_searchView.OnActionViewCollapsed();
			}

			_searchView.Visibility = ViewStates.Visible;

		}

		private void HandleQueryTextSubmit(object sender, SearchView.QueryTextSubmitEventArgs e)
		{
			_searchProvider?.SearchCommand.Execute(e.Query);
		}
	}
}
#endif
