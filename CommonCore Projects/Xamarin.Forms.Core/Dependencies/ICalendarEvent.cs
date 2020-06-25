using System;
using System.Collections.Generic;
using System.Threading.Tasks;

/// <summary>
/// Might consider using NUGET
/// https://github.com/TheAlmightyBob/Calendars
/// https://www.nuget.org/packages/CClarke.Plugin.Calendars
/// </summary>
namespace Xamarin.Forms.Core
{
    public class CalendarAccount
    {
        public string Id { get; set; }
        public string DisplayName { get; set; }
        public string AccountName { get; set; }
    }

    public class CalendarEventModel
    {
        public string Id { get; set; }
        public string ReminderId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Location { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public bool HasReminder { get; set; }
        public int ReminderMinutes { get; set; } = 60;
        public CalendarAccount DeviceCalendar { get; set; }
        public bool Deleted { get; set; }
    }


    public interface ICalendarEvent
    {
        Task<(bool result, CalendarEventModel model)> CreateCalendarEvent(CalendarEventModel calEvent);
        Task<List<CalendarAccount>> GetCalendars();
        Task<CalendarEventModel> GetCalendarEvent(string id);
        Task<(bool result, CalendarEventModel model)> UpdateCalendarEvent(CalendarEventModel calEvent);
    }
}

