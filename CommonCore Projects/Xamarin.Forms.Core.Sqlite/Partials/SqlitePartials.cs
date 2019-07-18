using System;
using Newtonsoft.Json;

namespace Xamarin.Forms.Core
{
    public partial class SqliteSettings
    {
        public string SQLiteDatabase { get; set; } = "app.db3";
    }

    public partial class CoreConfiguration
    {
        public SqliteSettings SqliteSettings { get; set; }
    }

    public partial class CoreBusiness
    {
        /// <summary>
        /// Embedded local database with tables defined by the application configuration file
        /// </summary>
        /// <value>The sqlite db.</value>
        [JsonIgnore]
        protected ISqliteDb SqliteDb
        {
            get
            {
                return (ISqliteDb)CoreDependencyService.GetService<ISqliteDb, SqliteDb>(true);
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
        protected ISqliteDb SqliteDb
        {
            get
            {
                return (ISqliteDb)CoreDependencyService.GetService<ISqliteDb, SqliteDb>(true);
            }
        }
    }
}
