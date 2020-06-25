using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms.Core;

namespace Pagination
{
    public class SomeBusinessLogic : CoreBusiness
    {


        public async Task<(List<Datum> Response, Exception Error)> GetPaginatedData(int pageIndex)
        {
            var url = this.WebApis["nih"];
            url = string.Format(url, pageIndex);
            var result = await this.HttpService.Get<RootObject>(url);
            if (result.Error == null)
            {
                var lst = result.Response.data;
                return (lst, null);
            }
            else
            {
                return (null, result.Error);
            }
        }
    }
}