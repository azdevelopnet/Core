using System;
using System.Collections.Generic;
using CoreReferenceExample.Models;
using Xamarin.Essentials;
using Xamarin.Forms.Core;

namespace CoreReferenceExample
{
    public class AuthViewModel: CoreViewModel
    {
        public AuthViewModel()
        {
        }


        public override void OnInit()
        {
            //MainThread.BeginInvokeOnMainThread(async() => {
            //    this.HttpService.AddHeader("ApiKey", "f07eb39e-71f9-484f-8438-4b22eb4b4d95");
            //    var result = await this.HttpService.Post<OAuthResponse>(CoreSettings.AuthenticateUrl, new OAuthParams()
            //    {
            //        email = "jSparrow@gmail.com",
            //        password = "e10adc3949ba59abbe56e057f20f883e",
            //        grant_type = "password"
            //    });
            //    if (result.Error == null)
            //    {
            //        this.HttpService.AddTokenHeader(result.Response.access_token);
            //        var queryResult = await this.HttpService.Get<List<User>>(CoreSettings.GetAllAccountUrl);
            //        if (queryResult.Error == null)
            //        {
            //            var users = queryResult.Response;
            //        }
            //    }
            //});

        }

        public override void OnViewMessageReceived(string key, object obj)
        {
           
        }

    }
}
