using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Core;

namespace Pagination
{
    public class SomeViewModel : CoreViewModel
    {
        private int pageIndex = 1;

        public OptimizedObservableCollection<Datum> PaginatedData { get; set; } = new OptimizedObservableCollection<Datum>();
        public bool IsRefreshing { get; set; }

        public ICommand LoadMorePaginatedData { get; set; }
        public ICommand RereshCommand { get; set; }

        public Datum SelectedPagingatedUser { get; set; }

        public SomeViewModel()
        {
            LoadMorePaginatedData = new CoreCommand(GetPaginatedData);
            RereshCommand = new CoreCommand(RefeshData);
        }

        public override void OnViewMessageReceived(string key, object obj)
        {

        }

        public override void OnInit()
        {
            GetPaginatedData(null);
        }

        public void RefeshData(object obj)
        {
            IsRefreshing = true;
            pageIndex = 1;
            PaginatedData.Clear();
            GetPaginatedData(obj);
            IsRefreshing = false;
        }

        public void GetPaginatedData(object obj)
        {
            Task.Run(async () =>
            {
                this.LoadingMessageHUD = "Performing download...";
                this.IsLoadingHUD = true;

                var result = await this.SomeLogic.GetPaginatedData(pageIndex);
                pageIndex++;

                this.IsLoadingHUD = false;
                if (result.Error == null)
                {
                    using (var updated = PaginatedData.BeginMassUpdate())
                    {
                        PaginatedData.AddRange(result.Response);
                    }
                }
                else
                {
                    //Device.BeginInvokeOnMainThread(() => {
                    //    DialogPrompt.ShowMessage(new Prompt()
                    //    {
                    //        Title = "Error",
                    //        Message = result.Error.Message
                    //    });
                    //});
                }

            });

        }

    }
}
