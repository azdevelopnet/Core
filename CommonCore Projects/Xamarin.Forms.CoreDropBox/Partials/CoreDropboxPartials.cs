using System;
using Newtonsoft.Json;

namespace Xamarin.Forms.Core
{
    public partial class CoreDropboxSettings
    {
        public string DropBoxAppKey { get; set; }
        public string DropBoxSecret { get; set; }
        public string RedirectUri { get; set; }
    }

    public partial class CoreConfiguration
    {
        public CoreDropboxSettings Dropbox { get; set; }
    }

    public partial class CoreBusiness
    {

        [JsonIgnore]
        protected ICoreDropBoxService CoreDropBox
        {
            get
            {
                return (ICoreDropBoxService)CoreDependencyService.GetService<ICoreDropBoxService, CoreDropBoxSerivce>(true);
            }
        }
    }

    public partial class CoreViewModel
    {
        [JsonIgnore]
        protected ICoreDropBoxService CoreDropBox
        {
            get
            {
                return (ICoreDropBoxService)CoreDependencyService.GetService<ICoreDropBoxService, CoreDropBoxSerivce>(true);

            }
        }
    }
}
