using System;
using CoreReferenceExample;
using Xamarin.Forms.Core;

namespace Xamarin.Forms.Core
{
    public partial class CoreSettings
    {

        public static SomeValueConverter UpperText
        {
            get
            {
                return CoreDependencyService.GetConverter<SomeValueConverter>();
            }
        }

        public static string AuthenticateUrl=> $"{CoreSettings.Config.WebApi["baseApi"]}api/Authentication/Authorize";
        public static string GetAllAccountUrl => $"{CoreSettings.Config.WebApi["baseApi"]}api/User/GetUserAccounts";



    }
}
