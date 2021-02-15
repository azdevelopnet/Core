using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Core;

namespace Core.Template.Core
{
    public class SomeViewModel : CoreViewModel
    {
        public string SomeText { get; set; }
        public int TotalItems { get; set; }

        public ICommand SomeAction => new Command(async() =>
        {
            await Task.Delay(new TimeSpan(0, 0, 4));
        });

        public ICommand SeeFontAction => new Command(async() => {
            await Navigation.PushAsync(new FontDemo());
        });

        public SomeViewModel()
        {

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
