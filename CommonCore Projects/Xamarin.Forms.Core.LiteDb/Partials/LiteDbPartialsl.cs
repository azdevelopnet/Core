using System;
using LiteDB;
using Newtonsoft.Json;
using System.Collections.Generic;
using Xamarin.Forms.Core;

namespace Xamarin.Forms.Core
{
    public partial class LiteliteSettings
    {
        public string LiteDatabase { get; set; }
    }

    public partial class CoreConfiguration
    {
        public LiteliteSettings LiteliteSettings { get; set; }
    }

    public partial class CoreBusiness
    {
        /// <summary>
        /// Embedded local database with tables defined by the application configuration file
        /// </summary>
        /// <value>The sqlite db.</value>
        [JsonIgnore]
        protected ILiteNoSql LiteDb
        {
            get
            {
                return (ILiteNoSql)CoreDependencyService.GetService<ILiteNoSql, LiteNoSql>(true);
            }
        }
    }

    public partial class CoreViewModel
    {
        /// <summary>
        /// Embedded local database with tables defined by the application configuration file
        /// </summary>
        /// <value>The sqlite db.</value>
        [JsonIgnore]
        protected ILiteNoSql LiteDb
        {
            get
            {
                return (ILiteNoSql)CoreDependencyService.GetService<ILiteNoSql, LiteNoSql>(true);
            }
        }
    }

}
