using System;
using Core.Reference;
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

    }
}
