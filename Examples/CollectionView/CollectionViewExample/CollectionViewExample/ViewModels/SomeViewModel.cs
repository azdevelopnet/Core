using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using CollectionViewExample.Views;
using Xamarin.Forms.Core;

namespace CollectionViewExample
{
    public class SomeViewModel : CoreViewModel
    {
        public string SomeText { get; set; }
        public int TotalItems { get; set; }

        public ICommand SomeAction { get; set; }

        public SomeViewModel()
        {

            SomeAction = new CoreCommand(async (obj) =>
            {
                //LoadingMessageHUD = "Some action...";
                //IsLoadingHUD = true;
                //await Task.Delay(new TimeSpan(0, 0, 4));
                //IsLoadingHUD = false;

                await Navigation.PushAsync(new ListPage());
            });
        }

        public override void OnViewMessageReceived(string key, object obj)
        {
            //Inter-app communication like MessageCenter without Pub/Sub
        }

        public override void OnInit()
        {
            var items = this.SomeLogic.GetSomeData();
            if (items.error == null)
            {
                TotalItems = items.data.Count;
            }
            else
            {
                this.DialogPrompt.ShowMessage(new Prompt()
                {
                    Title = "Error",
                    Message = items.error.Message
                });
            }
        }

        public override void OnRelease(bool includeEvents)
        {
            //Used to release resources - NOT A IMPLEMENTATION OF IDISPOSE
            //Include events mean to unhook all events as well otherwise leave them connected.
        }
    }
}
