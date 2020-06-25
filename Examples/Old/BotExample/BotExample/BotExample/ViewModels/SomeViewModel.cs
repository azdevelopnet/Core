using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms.Core;

namespace BotExample
{
    public class SomeViewModel : CoreViewModel
    {
        public string DirectLineToken { get; set; }
        public string UserName { get; set; } = "tony@starkindustries.com";
        public SomeViewModel()
        {
        }

        public async Task DirectLineInitialized()
        {
            if (string.IsNullOrEmpty(DirectLineToken))
            {
                var response = await this.SomeLogic.GetBotFrameworkToken();
                DirectLineToken = response.token;
            }

        }


        public override void OnViewMessageReceived(string key, object obj)
        {

        }
    }
}
