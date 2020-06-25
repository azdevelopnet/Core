using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Core.Reference.Views;
using Xamarin.Forms;
using Xamarin.Forms.Core;

namespace Core.Reference
{
    public class SomeViewModel : CoreViewModel
    {
        public string SomeText { get; set; }
        public int TotalItems { get; set; }
        public ICommand SomeAction { get; set; }
        public ICommand SeeFontAction { get; set; }

        public SomeViewModel()
        {

            SomeAction = new CoreCommand(async (obj) =>
            {
                this.ShowLoadingDialog("Some action...");
                await Task.Delay(new TimeSpan(0, 0, 4));
                this.CloseLoadingDialog();
            });
            SeeFontAction = new Command(async() => {
                await Navigation.PushAsync(new FontDemo());
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
