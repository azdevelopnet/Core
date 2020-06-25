using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CollectionViewExample.Models;
using Xamarin.Forms.Core;

namespace CollectionViewExample
{
    public class SomeBusinessLogic : CoreBusiness
    {

        public async Task<(List<RandomUser> users, Exception ex)> GetRandomUsers()
        {
            var url = CoreSettings.Config.WebApi["randomuser"];
            var result = await HttpService.Get<RootObject>(url);
            if (result.Success)
            {
                var users = result.Response.ToRandomUsers();
                return (users, null);
            }
            else
            {
                //Do some logging here

                return (null, result.Error);
            }
        }

        public (List<SomeObject> data, Exception error) GetSomeData()
        {
            try
            {
                var lst = new List<SomeObject>();
                lst.Add(new SomeObject() { Id = 1, Description = "One" });
                lst.Add(new SomeObject() { Id = 2, Description = "Two" });
                lst.Add(new SomeObject() { Id = 4, Description = "Four" });
                return (lst, null);
            }
            catch (Exception ex)
            {
                return (null, ex);
            }

        }
    }
}