using System;
using SQLite;


namespace Xamarin.Forms.Core
{
    public interface ICoreSqlModel
    {
        Guid CorrelationID { get; set; }
        long UTCTickStamp { get; set; } //can be set by DateTime.UtcNow.Ticks;
        bool MarkedForDelete { get; set; }
    }
    public class CoreSqlModel : CoreModel, ICoreSqlModel
    {
        [PrimaryKey]
        public Guid CorrelationID { get; set; } = Guid.NewGuid();
        public long UTCTickStamp { get; set; } = DateTime.UtcNow.Ticks;
        public bool MarkedForDelete { get; set; }

        [Ignore]
        public DateTime LocalTimeStamp
        {
            get
            {
                var utcDateTime = new DateTime(UTCTickStamp, DateTimeKind.Utc);
                return utcDateTime.ToLocalTime();
            }
        }

    }
}
