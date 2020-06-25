using System;
using Pagination;
using Xamarin.Forms.Core;

namespace Xamarin.Forms.Core
{
    public partial class CoreSettings
    {
        public const string FastRenderers = "FastRenderers_Experimental";

        public static SomeValueConverter UpperText
        {
            get
            {
                return CoreDependencyService.GetConverter<SomeValueConverter>();
            }
        }

    }
}
