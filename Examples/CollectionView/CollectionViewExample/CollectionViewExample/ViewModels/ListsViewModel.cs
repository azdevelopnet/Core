using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using CollectionViewExample.Models;
using Xamarin.Forms;
using Xamarin.Forms.Core;

namespace CollectionViewExample.ViewModels
{


    public class ListsViewModel : CoreViewModel
    {
        public ObservableCollection<RandomUser> Users { get; set; }

        public ListsViewModel()
        {

        }

        public override void OnInit()
        {
            Device.BeginInvokeOnMainThread(async () => {
                LoadingMessageHUD = "Loading...";
                IsLoadingHUD = true;
                var results = await this.SomeLogic.GetRandomUsers();
                IsLoadingHUD = false;
                if (results.ex == null)
                {
                    Users = results.users.ToObservable();
                }
                else
                {
                    DialogPrompt.ShowMessage(new Prompt()
                    {
                        Title = "Error",
                        Message = results.ex.Message
                    });
                }
               
            });
        }

        public override void OnViewMessageReceived(string key, object obj)
        {

        }
    }
}
