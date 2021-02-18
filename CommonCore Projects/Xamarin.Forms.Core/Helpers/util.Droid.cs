//#if __ANDROID__
//using System;
//using Java.Util;

//namespace Xamarin.Forms.Core.Utilities
//{
//    public class util
//    {
//        /// <summary>
//        /// Converts a UTC datestamp to the local timezone
//        /// </summary>
//        /// <returns>The UTC to local time zone.</returns>
//        /// <param name="dateTimeUtc">Date time UTC.</param>
//        public DateTime ConvertUTCToLocalTimeZone(DateTime dateTimeUtc)
//        {

//            // get the UTC/GMT Time Zone
//            Java.Util.TimeZone utcGmtTimeZone = Java.Util.TimeZone.GetTimeZone("UTC");

//            // get the local Time Zone
//            Java.Util.TimeZone localTimeZone = Java.Util.TimeZone.Default;

//            // convert the DateTime to Java type
//            Date javaDate = DateTimeToNativeDate(dateTimeUtc);

//            // convert to new time zone
//            Date timeZoneDate = ConvertTimeZone(javaDate, utcGmtTimeZone, localTimeZone);

//            // convert to systwem.datetime
//            DateTime timeZoneDateTime = NativeDateToDateTime(timeZoneDate);

//            return timeZoneDateTime;
//        }

//        /// <summary>
//        /// Converts a System.DateTime to a Java DateTime
//        /// </summary>
//        /// <returns>The time to native date.</returns>
//        /// <param name="date">Date.</param>
//        public static Java.Util.Date DateTimeToNativeDate(DateTime date)
//        {
//            long dateTimeUtcAsMilliseconds = (long)date.ToUniversalTime().Subtract(
//                new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)
//            ).TotalMilliseconds;
//            return new Date(dateTimeUtcAsMilliseconds);
//        }

//        /// <summary>
//        /// Converts a java datetime to system.datetime
//        /// </summary>
//        /// <returns>The date to date time.</returns>
//        /// <param name="date">Date.</param>
//        public static DateTime NativeDateToDateTime(Java.Util.Date date)
//        {
//            long javaDateAsMilliseconds = date.Time;
//            var dateTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).Add(TimeSpan.FromMilliseconds(javaDateAsMilliseconds));
//            return dateTime;
//        }

//        /// <summary>
//        /// Converts a date between time zones
//        /// </summary>
//        /// <returns>The date in the converted timezone.</returns>
//        /// <param name="date">Date to convert</param>
//        /// <param name="fromTZ">from Time Zone</param>
//        /// <param name="toTZ">To Time Zone</param>
//        public static Java.Util.Date ConvertTimeZone(Java.Util.Date date, Java.Util.TimeZone fromTZ, Java.Util.TimeZone toTZ)
//        {
//            long fromTZDst = 0;

//            if (fromTZ.InDaylightTime(date))
//            {
//                fromTZDst = fromTZ.DSTSavings;
//            }

//            long fromTZOffset = fromTZ.RawOffset + fromTZDst;

//            long toTZDst = 0;
//            if (toTZ.InDaylightTime(date))
//            {
//                toTZDst = toTZ.DSTSavings;
//            }

//            long toTZOffset = toTZ.RawOffset + toTZDst;

//            return new Java.Util.Date(date.Time + (toTZOffset - fromTZOffset));
//        }

//    }
//}
//#endif
