#if __ANDROID__
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Android.Content;
using Android.Database;
using Android.OS;
using Java.Util;
using Plugin.CurrentActivity;
using Xamarin.Essentials;
using Xamarin.Forms.Core;
using DroidUri = Android.Net.Uri;
using Provider = Android.Provider;
using RemindersMethod = Android.Provider.RemindersMethod;

[assembly: Xamarin.Forms.Dependency(typeof(CalendarEvent))]
namespace Xamarin.Forms.Core
{

    public class CalendarEvent : ICalendarEvent
    {
        private static readonly DateTime Jan1st1970 = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        private const string dialogMessage = "The application need access to the devices's calendar";
        public ContentResolver Resolver
        {
            get { return Ctx.ContentResolver; }
        }

        public Context Ctx
        {
            get => CrossCurrentActivity.Current.Activity;
        }

        public async Task<(bool result, CalendarEventModel model)> CreateCalendarEvent(CalendarEventModel calEvent)
        {
            (bool result, CalendarEventModel model) response = (false, calEvent);
            return await Task.Run(async () =>
            {
                if (Looper.MyLooper() == null)
                    Looper.Prepare();

                var status = await Permissions.RequestAsync<Permissions.CalendarWrite>();
                
                if (status == PermissionStatus.Granted)
                {
                    if (calEvent.DeviceCalendar != null)
                    {

                        var eventValues = new ContentValues();
                        eventValues.Put(Provider.CalendarContract.Events.InterfaceConsts.CalendarId, long.Parse(calEvent.DeviceCalendar.Id));
                        eventValues.Put(Provider.CalendarContract.Events.InterfaceConsts.Title, calEvent.Title);
                        eventValues.Put(Provider.CalendarContract.Events.InterfaceConsts.Description, calEvent.Description);
                        eventValues.Put(Provider.CalendarContract.Events.InterfaceConsts.HasAlarm, calEvent.HasReminder);
                        eventValues.Put(Provider.CalendarContract.Events.InterfaceConsts.EventLocation, calEvent.Location);
                        eventValues.Put(Provider.CalendarContract.Events.InterfaceConsts.AllDay, false);
                        eventValues.Put(Provider.CalendarContract.Events.InterfaceConsts.Dtstart, CurrentTimeMillis(calEvent.StartTime));
                        eventValues.Put(Provider.CalendarContract.Events.InterfaceConsts.Dtend, CurrentTimeMillis(calEvent.EndTime));
                        eventValues.Put(Provider.CalendarContract.Events.InterfaceConsts.EventTimezone, "UTC");
                        eventValues.Put(Provider.CalendarContract.Events.InterfaceConsts.EventEndTimezone, "UTC");

                        var uri = Resolver.Insert(Provider.CalendarContract.Events.ContentUri, eventValues);
                        response.model.Id = uri.LastPathSegment;

                        if (calEvent.HasReminder)
                        {
                            ContentValues reminders = new ContentValues();
                            reminders.Put(Provider.CalendarContract.Reminders.InterfaceConsts.EventId, response.model.Id);
                            reminders.Put(Provider.CalendarContract.Reminders.InterfaceConsts.Method, (int)RemindersMethod.Alert);
                            reminders.Put(Provider.CalendarContract.Reminders.InterfaceConsts.Minutes, calEvent.ReminderMinutes);
                            var uri2 = Resolver.Insert(Provider.CalendarContract.Reminders.ContentUri, reminders);
                            response.model.ReminderId = uri2.LastPathSegment;
                        }

                        response.result = true;

                    }

                }
                else{
                    Looper.Loop();
                }

                return response;
            });

        }


        public async Task<(bool result, CalendarEventModel model)> UpdateCalendarEvent(CalendarEventModel calEvent)
        {
            (bool result, CalendarEventModel model) response = (false, calEvent);
            return await Task.Run(async () =>
            {
                if (Looper.MyLooper() == null)
                    Looper.Prepare();

                var status = await Permissions.RequestAsync<Permissions.CalendarWrite>();
                if (status == PermissionStatus.Granted)
                {

                    if (calEvent.DeviceCalendar != null)
                    {

                        var updateUri = DroidUri.Parse($"content://com.android.calendar/events/{calEvent.Id}");
                        var eventValues = new ContentValues();
                        eventValues.Put(Provider.CalendarContract.Events.InterfaceConsts.Id, long.Parse(calEvent.Id));
                        eventValues.Put(Provider.CalendarContract.Events.InterfaceConsts.CalendarId, long.Parse(calEvent.DeviceCalendar.Id));
                        eventValues.Put(Provider.CalendarContract.Events.InterfaceConsts.Title, calEvent.Title);
                        eventValues.Put(Provider.CalendarContract.Events.InterfaceConsts.Description, calEvent.Description);
                        eventValues.Put(Provider.CalendarContract.Events.InterfaceConsts.HasAlarm, calEvent.HasReminder);
                        eventValues.Put(Provider.CalendarContract.Events.InterfaceConsts.EventLocation, calEvent.Location);
                        eventValues.Put(Provider.CalendarContract.Events.InterfaceConsts.AllDay, false);
                        eventValues.Put(Provider.CalendarContract.Events.InterfaceConsts.Dtstart, CurrentTimeMillis(calEvent.StartTime));
                        eventValues.Put(Provider.CalendarContract.Events.InterfaceConsts.Dtend, CurrentTimeMillis(calEvent.EndTime));
                        eventValues.Put(Provider.CalendarContract.Events.InterfaceConsts.EventTimezone, "UTC");
                        eventValues.Put(Provider.CalendarContract.Events.InterfaceConsts.EventEndTimezone, "UTC");

                        var result = Resolver.Update(updateUri, eventValues, null, null);
                        response.result = result == 1 ? true : false;

                        if (response.result && calEvent.HasReminder)
                        {
                            ContentValues reminders = new ContentValues();
                            reminders.Put(Provider.CalendarContract.Reminders.InterfaceConsts.EventId, response.model.Id);
                            reminders.Put(Provider.CalendarContract.Reminders.InterfaceConsts.Method, (int)RemindersMethod.Alert);
                            reminders.Put(Provider.CalendarContract.Reminders.InterfaceConsts.Minutes, calEvent.ReminderMinutes);

                            if (!string.IsNullOrEmpty(calEvent.ReminderId))
                            {
                                var reminderUri = DroidUri.Parse($"content://com.android.calendar/reminders/{calEvent.ReminderId}");
                                Resolver.Update(reminderUri, reminders, null, null);
                            }
                            else
                            {
                                var uri2 = Resolver.Insert(Provider.CalendarContract.Reminders.ContentUri, reminders);
                                response.model.ReminderId = uri2.LastPathSegment;
                            }
                        }

                    }
                }
                else{
                    Looper.Loop();
                }

                return response;
            });
        }

        public async Task<CalendarEventModel> GetCalendarEvent(string id)
        {
            return await Task.Run(async () =>
            {
                if (Looper.MyLooper() == null)
                    Looper.Prepare();

                var status = await Permissions.RequestAsync<Permissions.CalendarRead>();
                if (status == PermissionStatus.Granted)
                {

                    CalendarEventModel model = null;
                    var eventsUri = DroidUri.Parse($"content://com.android.calendar/events/{id}");
                    string[] eventsProjection = {
                    Provider.CalendarContract.Events.InterfaceConsts.Id,
                    Provider.CalendarContract.Events.InterfaceConsts.Title,
                    Provider.CalendarContract.Events.InterfaceConsts.Description,
                    Provider.CalendarContract.Events.InterfaceConsts.Dtstart,
                    Provider.CalendarContract.Events.InterfaceConsts.Dtend,
                    Provider.CalendarContract.Events.InterfaceConsts.HasAlarm,
                    Provider.CalendarContract.Events.InterfaceConsts.CalendarId,
                    Provider.CalendarContract.Events.InterfaceConsts.Deleted
                };
                    var loader = new CursorLoader(Ctx, eventsUri, eventsProjection, null, null, null);
                    var cursor = (ICursor)loader.LoadInBackground();

                    if (cursor.MoveToFirst())
                    {
                        do
                        {
                            var modelId = cursor.GetString(cursor.GetColumnIndex(eventsProjection[0])).ToString();
                            var title = cursor.GetString(cursor.GetColumnIndex(eventsProjection[1]));
                            var description = cursor.GetString(cursor.GetColumnIndex(eventsProjection[2]));
                            var dtstart = cursor.GetString(cursor.GetColumnIndex(eventsProjection[3]));
                            var dtend = cursor.GetString(cursor.GetColumnIndex(eventsProjection[4]));
                            var hasAlarm = cursor.GetString(cursor.GetColumnIndex(eventsProjection[5]));
                            var calendarId = cursor.GetString(cursor.GetColumnIndex(eventsProjection[6]));
                            var deleted = cursor.GetString(cursor.GetColumnIndex(eventsProjection[7]));

                            if (modelId.Equals(id))
                            {
                                model = new CalendarEventModel()
                                {
                                    Id = modelId,
                                    Title = title,
                                    Description = description,
                                    StartTime = CurrentDateTime(long.Parse(dtstart)),
                                    EndTime = CurrentDateTime(long.Parse(dtend)),
                                    HasReminder = int.Parse(hasAlarm) == 0 ? false : true,
                                    Deleted = int.Parse(deleted) == 0 ? false : true
                                };

                                model.DeviceCalendar = await GetCalendar(calendarId);

                                break;
                            }
                        } while (cursor.MoveToNext());
                    }

                    if (model.Deleted)
                        return null;

                    return model;
                }
                else
                {
                    Looper.Loop();
                    return null;
                }
            });

        }
        public async Task<CalendarAccount> GetCalendar(string id)
        {
            return await Task.Run(async () =>
            {
                CalendarAccount calAccount = null;

                if (Looper.MyLooper() == null)
                    Looper.Prepare();

                var status = await Permissions.RequestAsync<Permissions.CalendarRead>();
                if (status == PermissionStatus.Granted)
                {

                    var calendarsUri = DroidUri.Parse($"content://com.android.calendar/calendars/{id}");
                    string[] calendarsProjection = {
                    Provider.CalendarContract.Calendars.InterfaceConsts.Id,
                    Provider.CalendarContract.Calendars.InterfaceConsts.CalendarDisplayName,
                    Provider.CalendarContract.Calendars.InterfaceConsts.AccountName,
                };

                    var loader = new CursorLoader(Ctx, calendarsUri, calendarsProjection, null, null, null);
                    var cursor = (ICursor)loader.LoadInBackground();

                    if (cursor.MoveToFirst())
                    {
                        do
                        {
                            /* 
                             * Most calendar accounts have the same display name as account name.  The expection
                             * is holiday calendars etc.  Therefore, this method will return match account/display calendars.
                            */
                            var calId = cursor.GetLong(cursor.GetColumnIndex(calendarsProjection[0])).ToString();
                            var dn = cursor.GetString(cursor.GetColumnIndex(calendarsProjection[1]));
                            var an = cursor.GetString(cursor.GetColumnIndex(calendarsProjection[2]));
                            if (calId.Equals(id))
                            {
                                calAccount = new CalendarAccount
                                {
                                    Id = calId,
                                    DisplayName = dn,
                                    AccountName = an
                                };
                                break;
                            }
                        } while (cursor.MoveToNext());
                    }

                    return calAccount;
                }
                else
                {
                    Looper.Loop();
                    return null;
                }
            });
        }
        public async Task<List<CalendarAccount>> GetCalendars()
        {
            return await Task.Run(async () =>
            {
                if(Looper.MyLooper()==null)
                    Looper.Prepare();

                var status = await Permissions.RequestAsync<Permissions.CalendarRead>();
                if (status == PermissionStatus.Granted)
                {
                    var calendarsUri = Provider.CalendarContract.Calendars.ContentUri;
                    string[] calendarsProjection = {
                    Provider.CalendarContract.Calendars.InterfaceConsts.Id,
                    Provider.CalendarContract.Calendars.InterfaceConsts.CalendarDisplayName,
                    Provider.CalendarContract.Calendars.InterfaceConsts.AccountName,
                    Provider.CalendarContract.Calendars.InterfaceConsts.CalendarAccessLevel
                };
                    
                    var loader = new CursorLoader(Ctx, calendarsUri, calendarsProjection, null, null, null);
                    var cursor = (ICursor)loader.LoadInBackground();

                    var contactList = new List<CalendarAccount>();
                    if (cursor.MoveToFirst())
                    {
                        do
                        {
                            /* 
                             * Most calendar accounts have the same display name as account name.  The expection
                             * is holiday calendars etc.  Therefore, this method will return match account/display calendars.
                            */
                            var ident = cursor.GetLong(cursor.GetColumnIndex(calendarsProjection[0])).ToString();
                            var dn = cursor.GetString(cursor.GetColumnIndex(calendarsProjection[1]));
                            var an = cursor.GetString(cursor.GetColumnIndex(calendarsProjection[2]));
                            var cal = cursor.GetString(cursor.GetColumnIndex(calendarsProjection[3]));


                            if (dn.Equals(an) && !string.IsNullOrEmpty(cal) && cal == "700")
                            {
                                contactList.Add(new CalendarAccount
                                {
                                    Id = ident,
                                    DisplayName = dn,
                                    AccountName = an
                                });
                            }
                        } while (cursor.MoveToNext());
                    }

                    return contactList;
                }
                else
                {
                    Looper.Loop();
                    return new List<CalendarAccount>();
                }
            });

        }

        private long CurrentTimeMillis(DateTime date)
        {
            return (long)(date.ToUniversalTime() - Jan1st1970).TotalMilliseconds;
        }

        private DateTime CurrentDateTime(long milliseconds)
        {
            var calendar = Calendar.GetInstance(Java.Util.TimeZone.Default);
            calendar.TimeInMillis = milliseconds;
            var mnth = calendar.Get(CalendarField.Month);
            var day = calendar.Get(CalendarField.DayOfMonth);
            var yr = calendar.Get(CalendarField.Year);
            var hr = calendar.Get(CalendarField.HourOfDay);
            var min = calendar.Get(CalendarField.Minute);
            var netDate = new DateTime(yr, mnth, day, hr, min, 0);
            return netDate;
        }
    }
}
#endif

