using System;
using Core.Reference;
using Xamarin.Forms.Core;

namespace Xamarin.Forms.Core
{
    public class BuildEnv
    {
        public const string Dev = "dev";
        public const string QA = "qa";
        public const string UAT = "uat";
        public const string PROD = "prod";
    }

    public partial class CoreSettings
    {
 
        public static SomeValueConverter UpperText
        {
            get
            {
                return CoreDependencyService.GetConverter<SomeValueConverter>();
            }
        }

    }
}
