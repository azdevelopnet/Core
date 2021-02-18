#if __ANDROID__
using Xamarin.Forms.Platform.Android;
using Android.Util;
using Android.Views.InputMethods;
using Views = Android.Views;
using StrictMode = Android.OS.StrictMode;
using Util = Android.Util;
using System;
using System.ComponentModel;
using System.Reflection;
using Android.Content;
using Plugin.CurrentActivity;
using Android.Runtime;
using Android.App;
using App = Android.App;
using DroidBitMap = Android.Graphics.Bitmap;
using System.Threading.Tasks;
using XFPlatform = Xamarin.Forms.Platform.Android.Platform;
using Java.Util;

namespace Xamarin.Forms.Core
{
    public static partial class CoreExtensions
    {
        private static readonly Type _platformType = Type.GetType("Xamarin.Forms.Platform.Android.Platform, Xamarin.Forms.Platform.Android", true);
        private static BindableProperty _rendererProperty;

        public static BindableProperty RendererProperty
        {
            get
            {
                _rendererProperty = (BindableProperty)_platformType.GetField("RendererProperty", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static)
                    .GetValue(null);

                return _rendererProperty;
            }
        }

        public static IVisualElementRenderer GetOrCreateRenderer(this VisualElement bindable)
        {
            var renderer = XFPlatform.GetRenderer(bindable);
            if (renderer == null)
            {
                renderer = XFPlatform.CreateRendererWithContext(bindable, CrossCurrentActivity.Current.Activity);
                XFPlatform.SetRenderer(bindable, renderer);
            }
            return renderer;
        }


        public static Views.View GetNativeView(this BindableObject bindableObject)
        {
            var renderer = (IVisualElementRenderer)bindableObject.GetValue(RendererProperty);
            var viewGroup = renderer.View;
            return viewGroup;
        }

        public static Task<DroidBitMap> ToBitmap(this ImageSource imageSource)
        {
            var handler = imageSource.GetHandler();
            return handler.LoadImageAsync(imageSource, Ctx);
        }

        public static ImeAction GetValueFromDescription(this ReturnKeyTypes value)
        {
            var type = typeof(ImeAction);
            if (!type.IsEnum) throw new InvalidOperationException();
            foreach (var field in type.GetFields())
            {
                var attribute = Attribute.GetCustomAttribute(field,
                    typeof(DescriptionAttribute)) as DescriptionAttribute;
                if (attribute != null)
                {
                    if (attribute.Description == value.ToString())
                        return (ImeAction)field.GetValue(null);
                }
                else
                {
                    if (field.Name == value.ToString())
                        return (ImeAction)field.GetValue(null);
                }
            }
            throw new NotSupportedException($"Not supported on Android: {value}");
        }
        public static Context Ctx
        {
            get => CrossCurrentActivity.Current.Activity;
        }
        public static float ToDevicePixels(this float number)
        {
            var displayMetrics = Ctx.Resources.DisplayMetrics;

            return (float)System.Math.Round(number * (displayMetrics.Xdpi / (float)Util.DisplayMetricsDensity.Default));
        }
        public static float ToDevicePixels(this int number)
        {
            var displayMetrics = Ctx.Resources.DisplayMetrics;

            return (float)System.Math.Round(number * (displayMetrics.Xdpi / (float)Util.DisplayMetricsDensity.Default));
        }
        public static object Call(this object o, string methodName, params object[] args)
        {
            var mi = o.GetType().GetMethod(methodName, System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            if (mi != null)
            {
                return mi.Invoke(o, args);
            }
            return null;
        }

        public static void RegisterUnhandledExceptions(this Application app, Action<Exception> action)
        {
            AppDomain.CurrentDomain.UnhandledException += (s, e) =>
            {
                var ex = e.ExceptionObject as Exception;
                action.Invoke(ex);
            };

            AndroidEnvironment.UnhandledExceptionRaiser += (s, e) =>
            {
                action.Invoke(e.Exception);
                e.Handled = true;
            };

        }

        public static void ExitApplication(this Activity sender)
        {
            System.Environment.Exit(0);
        }

        public static void GoBack(this Activity context)
        {
            context.OnBackPressed();
        }

        public static void EnableStrictMode(this FormsAppCompatActivity activity)
        {
#if DEBUG
            var builder = new StrictMode.VmPolicy.Builder();
            var policy = builder.DetectActivityLeaks().PenaltyLog().Build();
            StrictMode.SetVmPolicy(policy);
#endif
        }

        public static int ConvertDiptoPix(this int dip)
        {
            var ctx = App.Application.Context;
            var scale = ctx.Resources.DisplayMetrics.Density;
            return (int)(dip * scale + 0.5f);
        }
        public static int ConvertPixtoDip(this int pixel)
        {
            var ctx = App.Application.Context;
            var scale = ctx.Resources.DisplayMetrics.Density;
           
            var dp = (int)((pixel) / scale);
            return dp;
        }
    }

    public static class AndroidDateExtensions
    {
        /// <summary>
        /// Converts a UTC datestamp to the local timezone
        /// </summary>
        /// <returns>The UTC to local time zone.</returns>
        /// <param name="dateTimeUtc">Date time UTC.</param>
        public static DateTime ConvertUTCToLocalTimeZone(this DateTime dateTimeUtc)
        {

            // get the UTC/GMT Time Zone
            Java.Util.TimeZone utcGmtTimeZone = Java.Util.TimeZone.GetTimeZone("UTC");

            // get the local Time Zone
            Java.Util.TimeZone localTimeZone = Java.Util.TimeZone.Default;

            // convert the DateTime to Java type
            Date javaDate = DateTimeToNativeDate(dateTimeUtc);

            // convert to new time zone
            Date timeZoneDate = ConvertTimeZone(javaDate, utcGmtTimeZone, localTimeZone);

            // convert to systwem.datetime
            DateTime timeZoneDateTime = NativeDateToDateTime(timeZoneDate);

            return timeZoneDateTime;
        }

        /// <summary>
        /// Converts a System.DateTime to a Java DateTime
        /// </summary>
        /// <returns>The time to native date.</returns>
        /// <param name="date">Date.</param>
        public static Java.Util.Date DateTimeToNativeDate(this DateTime date)
        {
            long dateTimeUtcAsMilliseconds = (long)date.ToUniversalTime().Subtract(
                new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)
            ).TotalMilliseconds;
            return new Date(dateTimeUtcAsMilliseconds);
        }

        /// <summary>
        /// Converts a java datetime to system.datetime
        /// </summary>
        /// <returns>The date to date time.</returns>
        /// <param name="date">Date.</param>
        public static DateTime NativeDateToDateTime(this Java.Util.Date date)
        {
            long javaDateAsMilliseconds = date.Time;
            var dateTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).Add(TimeSpan.FromMilliseconds(javaDateAsMilliseconds));
            return dateTime;
        }

        /// <summary>
        /// Converts a date between time zones
        /// </summary>
        /// <returns>The date in the converted timezone.</returns>
        /// <param name="date">Date to convert</param>
        /// <param name="fromTZ">from Time Zone</param>
        /// <param name="toTZ">To Time Zone</param>
        public static Java.Util.Date ConvertTimeZone(this Java.Util.Date date, Java.Util.TimeZone fromTZ, Java.Util.TimeZone toTZ)
        {
            long fromTZDst = 0;

            if (fromTZ.InDaylightTime(date))
            {
                fromTZDst = fromTZ.DSTSavings;
            }

            long fromTZOffset = fromTZ.RawOffset + fromTZDst;

            long toTZDst = 0;
            if (toTZ.InDaylightTime(date))
            {
                toTZDst = toTZ.DSTSavings;
            }

            long toTZOffset = toTZ.RawOffset + toTZDst;

            return new Java.Util.Date(date.Time + (toTZOffset - fromTZOffset));
        }

    }

}
#endif
