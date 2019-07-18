#if __IOS__
using System;
using Xamarin.Forms.Core;
using EventKit;
using Foundation;
using UIKit;
using System.Threading.Tasks;
using System.Collections.Generic;

[assembly: Xamarin.Forms.Dependency(typeof(CalendarEvent))]
namespace Xamarin.Forms.Core
{
	public class CalendarEvent : ICalendarEvent
	{
        private static EKEventStore eventStore;
        public static EKEventStore EventStore
        {
            get
            {
                return eventStore ?? (eventStore = new EKEventStore());
            }
        }

        public async Task<(bool result, CalendarEventModel model)> CreateCalendarEvent(CalendarEventModel calEvent)
        {
            (bool result, CalendarEventModel model) response = (false, calEvent);

            var result = await RequestAccess(EKEntityType.Event);
            if (result.granted)
            {
                var newEvent = EKEvent.FromStore(CalendarEvent.EventStore);
                var selectedCalendar = CalendarEvent.EventStore.DefaultCalendarForNewEvents;
                if (calEvent.DeviceCalendar != null)
                    selectedCalendar = CalendarEvent.EventStore.GetCalendar(calEvent.DeviceCalendar.Id);

                newEvent.StartDate = calEvent.StartTime.ToNSDate();
                newEvent.EndDate = calEvent.EndTime.ToNSDate();
                newEvent.Title = calEvent.Title;
                newEvent.Notes = calEvent.Description;
                newEvent.Calendar = selectedCalendar;

                if (calEvent.HasReminder)
                {
                    var offset = calEvent.StartTime.AddMinutes(-calEvent.ReminderMinutes).ToNSDate();
                    newEvent.AddAlarm(EKAlarm.FromDate(offset));
                }

                NSError e;

                CalendarEvent.EventStore.SaveEvent(newEvent, EKSpan.ThisEvent, true, out e);

                if (e == null)
                {
                    response.result = true;
                    response.model.Id = newEvent.EventIdentifier;
                }

            }

            return response;

        }

        public async Task<List<CalendarAccount>> GetCalendars()
        {
            var result = await RequestAccess(EKEntityType.Event);
            if(result.granted){}
            {
                var cals = CalendarEvent.EventStore.GetCalendars(EKEntityType.Event);
                var lst = new List<CalendarAccount>();
                foreach (var c in cals)
                {
                    if (c.AllowsContentModifications)
                    {
                        lst.Add(new CalendarAccount()
                        {
                            Id = c.CalendarIdentifier,
                            DisplayName = c.Title,
                            AccountName = c.Description
                        });
                    }
                }
                return lst;
            }
        }

        public async Task<(bool result, CalendarEventModel model)> UpdateCalendarEvent(CalendarEventModel calEvent)
        {
            (bool result, CalendarEventModel model) response = (false, calEvent);

            if (calEvent.DeviceCalendar == null)
                return response;

            var result = await RequestAccess(EKEntityType.Event);
            if (result.granted)
            {
                var evt = CalendarEvent.EventStore.EventFromIdentifier(calEvent.Id);
                if (evt != null)
                {

                    evt.StartDate = calEvent.StartTime.ToNSDate();
                    evt.EndDate = calEvent.EndTime.ToNSDate();
                    evt.Title = calEvent.Title;
                    evt.Location = calEvent.Location;
                    evt.Notes = calEvent.Description;
                    evt.Calendar = CalendarEvent.EventStore.GetCalendar(calEvent.DeviceCalendar.Id);

                    if (calEvent.HasReminder)
                    {
                        foreach (var alarm in evt.Alarms)
                            evt.RemoveAlarm(alarm);

                        var offset = calEvent.StartTime.AddMinutes(-calEvent.ReminderMinutes).ToNSDate();
                        evt.AddAlarm(EKAlarm.FromDate(offset));
                    }
                    NSError e=null;

                    var x = CalendarEvent.EventStore.SaveEvent(evt, EKSpan.ThisEvent, true, out e);
           
                    if (e == null)
                    {
                        response.result = true;
                        response.model.Id = evt.EventIdentifier;
                    }
                }
            }
            return response;
        }

        public async Task<CalendarEventModel> GetCalendarEvent(string id)
        {
            var result = await RequestAccess(EKEntityType.Event);
            if(result.granted){
                var evt = CalendarEvent.EventStore.EventFromIdentifier(id);
                if (evt != null)
                {

                    var model = new CalendarEventModel()
                    {
                        Id = evt.EventIdentifier,
                        Description = evt.Notes,
                        HasReminder = evt.HasAlarms,
                        Title = evt.Title,
                        EndTime = evt.EndDate.ToDateTime(),
                        StartTime = evt.StartDate.ToDateTime(),
                        DeviceCalendar = new CalendarAccount()
                        {
                             Id = evt.Calendar.CalendarIdentifier
                        }
                                       
                    };
                    return model;
                }
            }
            return null;

        }

        protected async Task<(bool granted, NSError error)> RequestAccess(EKEntityType type)
		{
            return await Task.Run(() =>
            {
                var t = new TaskCompletionSource<(bool granted, NSError error)>();
                CalendarEvent.EventStore.RequestAccess(type, (granted, error) => t.TrySetResult((granted, error)));
                return t.Task;
            });
		}

	}

}
#endif

