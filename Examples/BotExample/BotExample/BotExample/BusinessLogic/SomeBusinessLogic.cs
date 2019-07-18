using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms.Core;

namespace BotExample
{
    public class DirectLineResponse
    {
        public string conversationId { get; set; }
        public string token { get; set; }
        public int expires_in { get; set; }
    }

    public class SomeBusinessLogic : CoreBusiness
    {
        public async Task<DirectLineResponse> GetBotFrameworkToken()
        {
            using (var client = new HttpService())
            {
                client.AddTokenHeader(CoreSettings.Config.CustomSettings["ChatBotSecret"]);
                var rawResult = await client.Post<DirectLineResponse>(CoreSettings.Config.WebApi["directLine"], null);
                if (rawResult.Success)
                {
                    return rawResult.Response;
                }
                else
                {
                    return null;
                }
            }

        }
    }
}