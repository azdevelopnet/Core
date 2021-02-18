using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Linq;
using System.Collections.ObjectModel;
using System.Collections;
using System.Threading;
using System.ComponentModel;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Globalization;
using System.Linq.Expressions;
using System.Windows.Input;
using System.Net.Http;
using System.Net;
using System.Collections.Specialized;
using Xamarin.Forms.Core;
using Xamarin.Forms;
using System.IO;
using Xamarin.Essentials;


#if __ANDROID__
using Android.OS;
using Xamarin.Forms.Platform.Android;
using Android.Graphics;
#else
using Xamarin.Forms.Platform.iOS;
using System.Drawing;
using UIKit;
using CoreGraphics;
#endif

namespace Xamarin.Forms.Core
{

    public static partial class CoreExtensions
    {

        private static IEncryptionService Encryption
        {
            get
            {
                return CoreDependencyService.GetService<IEncryptionService, EncryptionService>(true);
            }
        }

        private static IHttpService HttpService
        {
            get
            {
                return (IHttpService)CoreDependencyService.GetService<IHttpService, HttpService>(true);
            }
        }

        public static async Task<bool> HeightTo(this View view, double height, uint duration = 250, Easing easing = null)
        {
            var tcs = new TaskCompletionSource<bool>();

            var heightAnimation = new Animation(x => view.HeightRequest = x, view.Height, height);
            heightAnimation.Commit(view, "HeightAnimation", 10, duration, easing, (finalValue, finished) => { tcs.SetResult(finished); });

            return await tcs.Task;
        }

        public static async Task<bool> WidthTo(this View view, double width, uint duration = 250, Easing easing = null)
        {
            var tcs = new TaskCompletionSource<bool>();

            var heightAnimation = new Animation(x => view.WidthRequest = x, view.Height, width);
            heightAnimation.Commit(view, "WidthAnimation", 10, duration, easing, (finalValue, finished) => { tcs.SetResult(finished); });

            return await tcs.Task;
        }

        public static void FocusNext(this StackLayout layout, int index)
        {
            var view = layout.Children.FirstOrDefault(x => x.TabIndex == index);
            view?.Focus();
        }

        public static void FindViewByStyleId(this ILayoutController container, string styleId, ref object obj)
        {
            if (container is Layout)
            {
                var layout = (Layout)container;
                if (layout.StyleId == styleId)
                {
                    obj = layout;
                    return;
                }
            }

            var view = container.Children.FirstOrDefault(x => x.StyleId == styleId);
            if (view != null)
            {
                obj = view;
                return;
            }
            else
            {
                foreach (var item in container.Children)
                {
                    if (item is ILayoutController)
                    {
                        ((ILayoutController)item).FindViewByAutomationId(styleId, ref obj);
                    }
                }
            }


        }

        public static void FindViewByAutomationId(this ILayoutController container, string automationId, ref object obj)
        {
            if (container is Layout)
            {
                var layout = (Layout)container;
                if (layout.AutomationId == automationId)
                {
                    obj = layout;
                    return;
                }
            }

            var view = container.Children.FirstOrDefault(x => x.AutomationId == automationId);
            if (view != null)
            {
                obj = view;
                return;
            }
            else
            {
                foreach (var item in container.Children)
                {
                    if (item is ILayoutController)
                    {
                        ((ILayoutController)item).FindViewByAutomationId(automationId, ref obj);
                    }
                }
            }


        }

        public static T OnIdiom<T>(this object call, params T[] parameters)
        {
            if (Device.Idiom == TargetIdiom.Phone)
                return parameters[0];
            if (Device.Idiom == TargetIdiom.Tablet)
                return parameters[1];

            return parameters[0];
        }

        public static T On<T>(this object caller, params T[] parameters)
        {
            T obj = default(T);

            switch (Device.RuntimePlatform.ToUpper())
            {
                case "IOS":
                    if (parameters.Length > 0)
                        obj = parameters[0];
                    break;
                case "ANDROID":
                    if (parameters.Length > 1)
                        obj = parameters[1];
                    break;
                default:
                    if (parameters.Length > 2)
                        obj = parameters[2];
                    break;
            }

            return obj;
        }

        public static void ClearInstanceResources(this object obj)
        {
            foreach (var prop in obj.GetType().GetProperties().Where(x => x.CanWrite))
            {
                try
                {
                    if (prop.PropertyType.IsClass)
                        prop.SetValue(obj, null);
                }
                catch { }
            }
            foreach (var field in obj.GetType().GetFields(BindingFlags.NonPublic | BindingFlags.Instance))
            {
                try
                {
                    if (field.FieldType.IsClass)
                        field.SetValue(obj, null);
                }
                catch { }
            }
        }

        public static string[] SplitAndTrim(this string text, string separator)
        {
            var lst = text.Split(separator);

            for (var x = 0; x < lst.Length; x++)
                lst[x] = lst[x].ToLower().Trim();

            return lst.ToArray();
        }

        public static string[] Trim(this string[] array)
        {
            var lst = new string[array.Length];
            for (var x = 0; x < array.Length; x++)
                lst[x] = array[x].Trim();
            return lst;
        }

        public static string BuildQueryString(this NameValueCollection collection)
        {
            var array = (from key in collection.AllKeys
                         from value in collection.GetValues(key)
                         select string.Format("{0}={1}", WebUtility.UrlEncode(key), WebUtility.UrlEncode(value)))
                        .ToArray();
            return "?" + string.Join("&", array);
        }

        public static void SaveState<T>(this T vm) where T : CoreViewModel, new()
        {
            Task.Run(async () =>
            {
                await CoreDependencyService.GetService<IFileStore, FileStore>(true)?.SaveAsync<T>(typeof(T).FullName, vm);
            });
        }

        public static void LoadState<T>(this T vm) where T : CoreViewModel, new()
        {
            Task.Run(async () =>
            {
                var result = await CoreDependencyService.GetService<IFileStore, FileStore>(true)?.GetAsync<T>(typeof(T).FullName);
                if (result.Error == null)
                {
                    foreach (var prop in typeof(T).GetProperties())
                    {
                        prop.SetValue(vm, prop.GetValue(result.Response));
                    }
                }
            });
        }

        public static DateTime ToLocalTime(this long utcTicks)
        {
            var utcDateTime = new DateTime(utcTicks, DateTimeKind.Utc);
            return utcDateTime.ToLocalTime();
        }

        public static StringContent ToStringContent(this object obj)
        {
            var data = JsonConvert.SerializeObject(obj);
            return new StringContent(data, Encoding.UTF8, "application/json");
        }

        public static T ToEnum<T>(this string str) where T : struct
        {
            T returnEnum;
            Enum.TryParse(str, out returnEnum);
            return returnEnum;
        }

        public static PropertyInfo GetProperty<T, P>(this Expression<Func<T, P>> exp)
        {
            var expression = (MemberExpression)exp.Body;
            string name = expression.Member.Name;
            return typeof(T).GetProperty(name);
        }

        public static P GetPropertyValue<T, P>(this Expression<Func<T, P>> exp, T obj)
        {
            var expression = (MemberExpression)exp.Body;
            string name = expression.Member.Name;
            var prop = obj.GetType().GetProperty(name);
            return (P)prop.GetValue(obj, null);
        }

        public static bool IsEqual(this double variable1, double variable2)
        {
            var var1 = (long)variable1 * 10000;
            var var2 = (long)variable2 * 10000;
            return var1 == var2;
        }

        public static T ToObject<T>(this WeakReference<T> reference) where T : class
        {
            T obj = null;
            reference.TryGetTarget(out obj);
            return obj;
        }

        public static T Cast<T>(this WeakReference obj) where T : class
        {
            if (obj.IsAlive)
                return (T)obj.Target;
            else
                return null;
        }

        public static string ToTitleCase(this string sentence)
        {
            return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(sentence.ToLower());
        }

        public static T ConvertTo<T>(this string str) where T : struct
        {
            object result = null;
            var code = Type.GetTypeCode(typeof(T));
            switch (code)
            {
                case TypeCode.Int32:
                    result = JsonConvert.DeserializeObject<int>(str);
                    break;
                case TypeCode.Int16:
                    result = JsonConvert.DeserializeObject<short>(str);
                    break;
                case TypeCode.Int64:
                    result = JsonConvert.DeserializeObject<long>(str);
                    break;
                case TypeCode.String:
                    result = JsonConvert.DeserializeObject<string>(str);
                    break;
                case TypeCode.Boolean:
                    result = JsonConvert.DeserializeObject<bool>(str);
                    break;
                case TypeCode.Double:
                    result = JsonConvert.DeserializeObject<double>(str);
                    break;
                case TypeCode.Decimal:
                    result = JsonConvert.DeserializeObject<decimal>(str);
                    break;
                case TypeCode.Byte:
                    result = JsonConvert.DeserializeObject<Byte>(str);
                    break;
                case TypeCode.DateTime:
                    result = JsonConvert.DeserializeObject<DateTime>(str);
                    break;
                case TypeCode.Single:
                    result = JsonConvert.DeserializeObject<Single>(str);
                    break;
            }
            return (T)result;
        }

        public static string GetString(this PropertyInfo prop, object obj)
        {
            return (string)prop.GetValue(obj, null);
        }

        public static void EncryptedModelProperties<T>(IEnumerable<T> list) where T : BindableObject
        {
            var props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance)
                     .Where(p => p.GetCustomAttributes(typeof(EncryptedPropertyAttribute)).Count() > 0).ToArray();

            if (props.Count() > 0)
            {
                foreach (var prop in props)
                {
                    foreach (var obj in list)
                    {
                        prop.SetValue(obj, Encryption.AesEncrypt(prop.GetString(obj), CoreSettings.Config.AESEncryptionKey), null);
                    }
                }
            }
        }

        public static void UnEncryptedModelProperties<T>(IEnumerable<T> list) where T : BindableObject
        {

            var props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance)
                     .Where(p => p.GetCustomAttributes(typeof(EncryptedPropertyAttribute)).Count() > 0).ToArray();

            if (props.Count() > 0)
            {
                foreach (var prop in props)
                {
                    foreach (var obj in list)
                    {
                        prop.SetValue(obj, Encryption.AesDecrypt(prop.GetString(obj), CoreSettings.Config.AESEncryptionKey), null);
                    }
                }
            }
        }

        public static void EncryptedDataModelProperties<T>(this Dictionary<Type, PropertyInfo[]> dict, T obj) where T : new()
        {
            var props = dict[typeof(T)];
            if (props.Count() > 0)
            {
                foreach (var prop in props)
                {
                    prop.SetValue(obj, Encryption.AesEncrypt(prop.GetString(obj), CoreSettings.Config.AESEncryptionKey), null);
                }
            }
        }

        public static void EncryptedDataModelProperties<T>(this Dictionary<Type, PropertyInfo[]> dict, IEnumerable<T> list) where T : new()
        {
            var props = dict[typeof(T)];
            if (props.Count() > 0)
            {
                foreach (var prop in props)
                {
                    foreach (var obj in list)
                    {
                        prop.SetValue(obj, Encryption.AesEncrypt(prop.GetString(obj), CoreSettings.Config.AESEncryptionKey), null);
                    }
                }
            }
        }

        public static void UnEncryptedDataModelProperties<T>(this Dictionary<Type, PropertyInfo[]> dict, T obj) where T : new()
        {
            var props = dict[typeof(T)];
            if (props.Count() > 0)
            {
                foreach (var prop in props)
                {
                    prop.SetValue(obj, Encryption.AesDecrypt(prop.GetString(obj), CoreSettings.Config.AESEncryptionKey), null);
                }
            }
        }

        public static void UnEncryptedDataModelProperties<T>(this Dictionary<Type, PropertyInfo[]> dict, IEnumerable<T> list) where T : new()
        {
            var props = dict[typeof(T)];
            if (props.Count() > 0)
            {
                foreach (var prop in props)
                {
                    foreach (var obj in list)
                    {
                        prop.SetValue(obj, Encryption.AesDecrypt(prop.GetString(obj), CoreSettings.Config.AESEncryptionKey), null);
                    }
                }
            }
        }

        public static void SetAutomationIds(this ContentPage page)
        {
            var bindingFlags = BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public;
            var fields = page.GetType().GetFields(bindingFlags);
            foreach (var field in fields)
            {
                try
                {
                    var fObj = field.GetValue(page);
                    if (fObj != null && fObj is View)
                    {
                        var ctrl = (View)fObj;
                        if (string.IsNullOrEmpty(ctrl.AutomationId))
                            ctrl.AutomationId = field.Name;
                    }
                }
                catch { }//suppress error

            }
            var props = page.GetType().GetProperties(bindingFlags);
            foreach (var prop in props)
            {
                try
                {
                    var pObj = prop.GetValue(page);
                    if (pObj != null && pObj is View)
                    {
                        var ctrl = (View)pObj;
                        if (string.IsNullOrEmpty(ctrl.AutomationId))
                            ctrl.AutomationId = prop.Name;

                    }
                }
                catch { }//suppress error

            }
        }

        public static void ConsoleWrite(this Exception ex, bool includeImageMarker = false)
        {
#if DEBUG
            Console.WriteLine();
            Console.WriteLine();
            if (includeImageMarker)
                DrawMonkey();
            Console.WriteLine("*-*-*-*-*-*-*-*-*-*-*-*- " + ex.GetType().Name + " DEBUG EXCEPTION *-*-*-*-*-*-*-*-*-*-*-*-*-");
            Console.WriteLine(ex.Message);
            Console.WriteLine(ex.InnerException?.InnerException);
            Console.WriteLine(ex.StackTrace);
            Console.WriteLine("*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-");
            Console.WriteLine();
            Console.WriteLine();
#endif
        }

        public static void ConsoleWrite(this string str, string title, bool includeImageMarker = false)
        {
#if DEBUG
            if (includeImageMarker)
                DrawMonkey();
            Console.WriteLine($"*-*-*-*-*-*-*-*-*-*-*-*- {title} *-*-*-*-*-*-*-*-*-*-*-*-*-");
            Console.WriteLine(str);
            Console.WriteLine("*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-");
#endif
        }

        private static void DrawMonkey()
        {
            Console.WriteLine("         .-\"-.");
            Console.WriteLine("       _/.-.-.\\_");
            Console.WriteLine("      ( ( o o ) )");
            Console.WriteLine("       |/  \"  \\|");
            Console.WriteLine("        \\ .-. /");
            Console.WriteLine("        /`\"\"\"'\\");
            Console.WriteLine("       /       \\");
        }

        public async static Task<T> WithTimeout<T>(this Task<T> task, int duration)
        {
            var retTask = await Task.WhenAny(task, Task.Delay(duration))
                .ConfigureAwait(false);

            if (retTask is Task<T>)
                return task.Result;

            return default(T);
        }

        public static void ContinueOn(this Task task)
        {
            task.ContinueWith((t) => { });
        }

        public static void Execute(this SynchronizationContext ctx, Action action)
        {
            ctx.Post((x) =>
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    action?.Invoke();
                });

            }, null);
        }

        public static async Task<T> FirstOrDefault<T>(this Task<List<T>> taskCollection)
        {
            var result = await taskCollection;
            return result.FirstOrDefault();
        }

        public static async Task<T> LastOrDefault<T>(this Task<List<T>> taskCollection)
        {
            var result = await taskCollection;
            return result.LastOrDefault();
        }

        public static async Task<T> FirstOrDefault<T>(this Task<(List<T> Response, Exception Error)> taskCollection)
        {
            var result = await taskCollection;
            if (result.Error == null)
                return result.Response.FirstOrDefault();
            else
                return default(T);
        }

        public static T DeepCopy<T>(this T obj) where T : new()
        {
            var newObj = new T();
            foreach (var prop in typeof(T).GetProperties())
            {
                if (prop.CanRead && prop.CanWrite)
                {
                    prop.SetValue(newObj, prop.GetValue(obj));
                }
            }
            return newObj;
        }

        public static void ShallowCopy<T>(this T item, T copyfrom) where T : new()
        {
            foreach (var prop in typeof(T).GetProperties())
            {
                if (prop.CanRead && prop.CanWrite)
                {
                    prop.SetValue(item, prop.GetValue(copyfrom));
                }
            }
        }

        public static T GetBoundItem<T>(this object view) where T : class, new()
        {
            return (T)((View)view).BindingContext;
        }

        public static void RemoveAnimations(this ContentPage page)
        {
            if (page.Content is Layout<View>)
            {
                var layout = (Layout<View>)page.Content;
                RemoveAnimations(layout);
            }
        }

		public static void RemoveAnimations(this Layout<View> layout)
        {
            foreach (var element in layout.Children)
            {
                if (element is Layout<View>)
                {
                    RemoveAnimations((Layout<View>)element);
                }
                else
                {
                    try
                    {
                        ViewExtensions.CancelAnimations(element);
                    }
                    catch { }
                }
            }
        }

        public static void DisableChildren(this Layout<View> layout)
        {
            foreach (var element in layout.Children)
            {
                if (element is Layout<View>)
                {
                    DisableChildren((Layout<View>)element);
                }
                else
                {
                    element.IsEnabled = false;
                }
            }
        }

        public static void EnableChildren(this Layout<View> layout)
        {
            foreach (var element in layout.Children)
            {
                if (element is Layout<View>)
                {
                    EnableChildren((Layout<View>)element);
                }
                else
                {
                    element.IsEnabled = true;
                }
            }
        }

        public static FormattedString AddTextSpan(this FormattedString formattedString, string text)
        {
            formattedString.Spans.Add(new Span() { Text = text });
            return formattedString;
        }

        public static FormattedString AddTextSpan(this FormattedString formattedString, string text, double lineHeight)
        {
            formattedString.Spans.Add(new Span() { Text = text, LineHeight = lineHeight });
            return formattedString;
        }

        public static FormattedString AddSpan(this FormattedString formattedString, Span span)
        {
            formattedString.Spans.Add(span);
            return formattedString;
        }

        public static List<IDictionary<string, object>> ToDictionary(this byte[] array)
        {
            var json = Encoding.Default.GetString(array);
            return JsonConvert.DeserializeObject<List<IDictionary<string, object>>>(json);
        }

        public static Dictionary<string, object> ToDictionary(this object obj)
        {
            var str = JsonConvert.SerializeObject(obj);
            return JsonConvert.DeserializeObject<Dictionary<string, object>>(str);
        }

        public static string ToNumericString(this string phoneNum)
        {
            return new Regex("[^0-9]").Replace(phoneNum, "");
        }

        public static void ResetNotifier(this ICommand command, INotifyPropertyChanged notifier)
        {
            ((CoreCommand)command).NotifyBinder = notifier;
        }

        public static void PushNonAwaited<T>(this INavigation nav, bool animated = true) where T : ContentPage, new()
        {
            nav.PushAsync(new T(), animated).ConfigureAwait(false);
        }

        public static void PushNonAwaited(this INavigation nav, ContentPage page, bool animated = true)
        {
            nav.PushAsync(page, animated).ConfigureAwait(false);
        }

        public static void PopNonAwaited(this INavigation nav, bool animated = true)
        {
            nav.PopAsync(animated).ConfigureAwait(false);
        }

        public static async Task<Page> PopTo<T>(this INavigation nav, bool animated = true) where T : ContentPage, new()
        {
            var pageName = typeof(T).FullName;

            if (nav.NavigationStack.Any(x => x.GetType().FullName == pageName) && nav.NavigationStack.Count > 1)
            {
                if (nav.NavigationStack.Last().GetType().FullName == pageName)
                    return null;

                for (int x = (nav.NavigationStack.Count - 2); x > -1; x--)
                {
                    var page = nav.NavigationStack[x];
                    var name = page.GetType().FullName;
                    if (name == pageName)
                    {
                        return await nav.PopAsync(animated);
                    }
                    else
                    {
                        nav.RemovePage(page);
                    }
                }

            }
            return null;

        }

        public static void AddChild(this Grid grid, View view, int row, int column, int rowspan = 1, int columnspan = 1)
        {
            if (row < 0)
                throw new ArgumentOutOfRangeException("row");
            if (column < 0)
                throw new ArgumentOutOfRangeException("column");
            if (rowspan <= 0)
                throw new ArgumentOutOfRangeException("rowspan");
            if (columnspan <= 0)
                throw new ArgumentOutOfRangeException("columnspan");
            if (view == null)
                throw new ArgumentNullException("view");

            Grid.SetRow((BindableObject)view, row);
            Grid.SetRowSpan((BindableObject)view, rowspan);
            Grid.SetColumn((BindableObject)view, column);
            Grid.SetColumnSpan((BindableObject)view, columnspan);

            grid.Children.Add(view);
        }

        public static string CalendarTitle(this DateTime date)
        {
            var monthName = date.ToString("MMMM");
            return $"{monthName} {date.Year}";
        }

        public static Style EditStyle(this Style style, BindableProperty prop, object value, bool retainInstance = false)
        {
            Style ns = null;
            if (!retainInstance)
            {
                ns = new Style(style.TargetType);
                foreach (var setter in style.Setters)
                {
                    ns.Setters.Add(new Setter() { Property = setter.Property, Value = setter.Value });
                }
            }
            else
            {
                ns = style;
            }

            var existingProp = ns.Setters.FirstOrDefault(x => x.Property == prop);
            if (existingProp != null)
            {
                existingProp.Value = value;
            }
            else
            {
                ns.Setters.Add(new Setter() { Property = prop, Value = value });
            }

            return ns;
        }

    }

    /// <summary>
    /// Validation Extensions
    /// </summary>
    public static class ValidationExtensions
    {
        public static bool ValidateTextFields(this CoreViewModel model, params string[] fields)
        {
            foreach (var obj in fields) if (String.IsNullOrEmpty(obj)) return false;

            return true;
        }

        public static bool ValidateTextFields(this bool chainedResponse, params string[] fields)
        {
            if (chainedResponse)
            {
                foreach (var obj in fields) if (String.IsNullOrEmpty(obj))
                        return false;
                return true;
            }
            else
            {
                return chainedResponse;
            }
        }

        public static bool ValidateNumberFields(this CoreViewModel model, decimal minValue, decimal maxValue, params decimal[] fields)
        {
            foreach (var obj in fields)
            {
                if (obj < minValue || obj > maxValue)
                {
                    return false;
                }
            }
            return true;
        }

        public static bool ValidateNumberFields(this bool chainedResponse, decimal minValue, decimal maxValue, params decimal[] fields)
        {
            if (chainedResponse)
            {
                foreach (var obj in fields)
                {
                    if (obj < minValue || obj > maxValue)
                    {
                        return false;
                    }
                }
                return true;
            }
            else
            {
                return chainedResponse;
            }
        }

        public static bool ValidateDateFields(this CoreViewModel model, DateTime minValue, DateTime maxValue, params DateTime[] fields)
        {
            foreach (var obj in fields)
            {
                if (obj < minValue || obj > maxValue)
                {
                    return false;
                }
            }
            return true;
        }

        public static bool ValidateDateFields(this bool chainedResponse, DateTime minValue, DateTime maxValue, params DateTime[] fields)
        {
            if (chainedResponse)
            {
                foreach (var obj in fields)
                {
                    if (obj < minValue || obj > maxValue)
                    {
                        return false;
                    }
                }
                return true;
            }
            else
            {
                return chainedResponse;
            }
        }

        public static bool ValidateEmailFields(this CoreViewModel model, params string[] fields)
        {
            var RegexExp = @"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$";
            foreach (var obj in fields)
            {
                try
                {
                    return Regex.IsMatch(obj, RegexExp, RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250));
                }
                catch (Exception)
                {
                    return false;
                }
            }
            return false;
        }

        public static bool ValidateEmailFields(this bool chainedResponse, params string[] fields)
        {
            if (chainedResponse)
            {
                var RegexExp = @"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$";
                foreach (var obj in fields)
                {
                    try
                    {
                        return Regex.IsMatch(obj, RegexExp, RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250));
                    }
                    catch (Exception)
                    {
                        return false;
                    }
                }
                return false;
            }
            else
            {
                return chainedResponse;
            }
        }

        public static bool ValidatePasswordFields(this CoreViewModel model, params string[] fields)
        {
            var hasNumber = new Regex(@"[0-9]+");
            var hasChar = new Regex(@"[a-zA-Z]+");
            //var hasCorrectNumChars = new Regex(@"^.{8,16}$");
            var hasCorrectNumChars = new Regex(@"^.{8,}$");

            foreach (var obj in fields)
            {
                try
                {
                    return hasNumber.IsMatch(obj) && hasChar.IsMatch(obj) && hasCorrectNumChars.IsMatch(obj);
                }
                catch (Exception)
                {
                    return false;
                }
            }
            return false;
        }

        public static bool ValidatePasswordFields(this bool chainedResponse, params string[] fields)
        {
            if (chainedResponse)
            {
                var hasNumber = new Regex(@"[0-9]+");
                var hasChar = new Regex(@"[a-zA-Z]+");
                //var hasCorrectNumChars = new Regex(@"^.{8,16}$");
                var hasCorrectNumChars = new Regex(@"^.{8,}$");

                foreach (var obj in fields)
                {
                    try
                    {
                        return hasNumber.IsMatch(obj) && hasChar.IsMatch(obj) && hasCorrectNumChars.IsMatch(obj);
                    }
                    catch (Exception)
                    {
                        return false;
                    }
                }
                return false;
            }
            else
            {
                return chainedResponse;
            }
        }

        public static bool ValidatePasswordMatch(this CoreViewModel model, params string[] fields)
        {
            for (int i = 0; i < fields.Length; i++)
            {
                try
                {
                    var currentPassword = fields[i];

                    // validate passwords match
                    if (i > 0)
                    {
                        var previousPassword = fields[i - 1];
                        var previousPasswordMatches = currentPassword == previousPassword;
                        if (!previousPasswordMatches)
                        {
                            return false;
                        }
                    }
                }
                catch (Exception)
                {
                    return false;
                }
            }
            return true;
        }

        public static bool ValidatePasswordMatch(this bool chainedResponse, params string[] fields)
        {
            if (chainedResponse)
            {
                for (int i = 0; i < fields.Length; i++)
                {
                    try
                    {
                        var currentPassword = fields[i];

                        // validate passwords match
                        if (i > 0)
                        {
                            var previousPassword = fields[i - 1];
                            var previousPasswordMatches = currentPassword == previousPassword;
                            if (!previousPasswordMatches)
                            {
                                return false;
                            }
                        }
                    }
                    catch (Exception)
                    {
                        return false;
                    }
                }
                return true;
            }
            else
            {
                return chainedResponse;
            }
        }

        public static bool ValidateDefaultFields<T>(this CoreViewModel model, params T[] fields) where T : struct
        {
            var state = true;
            var type = typeof(T).Name;
            switch (type)
            {
                case "Boolean":
                    foreach (var obj in fields)
                    {
                        if (((bool)(object)obj) == default(bool))
                            state = false;
                    }
                    break;
                case "Int32":
                    foreach (var obj in fields)
                    {
                        if (((int)(object)obj).Equals(default(int)))
                            state = false;
                    }
                    break;
                case "Int64":
                    foreach (var obj in fields)
                    {
                        if (((long)(object)obj).Equals(default(long)))
                            state = false;
                    }
                    break;
                case "Double":
                    foreach (var obj in fields)
                    {
                        if (((double)(object)obj).Equals(default(double)))
                            state = false;
                    }
                    break;
                case "Single":
                    foreach (var obj in fields)
                    {
                        if (((float)(object)obj).Equals(default(float)))
                            state = false;
                    }
                    break;
            }

            return state;
        }

        public static bool ValidateDefaultFields<T>(this bool chainedResponse, params T[] fields) where T : struct
        {
            if (chainedResponse)
            {
                var state = true;
                var type = typeof(T).Name;
                switch (type)
                {
                    case "Boolean":
                        foreach (var obj in fields)
                        {
                            if (((bool)(object)obj) == default(bool))
                                state = false;
                        }
                        break;
                    case "Int32":
                        foreach (var obj in fields)
                        {
                            if (((int)(object)obj).Equals(default(int)))
                                state = false;
                        }
                        break;
                    case "Int64":
                        foreach (var obj in fields)
                        {
                            if (((long)(object)obj).Equals(default(long)))
                                state = false;
                        }
                        break;
                    case "Double":
                        foreach (var obj in fields)
                        {
                            if (((double)(object)obj).Equals(default(double)))
                                state = false;
                        }
                        break;
                    case "Single":
                        foreach (var obj in fields)
                        {
                            if (((float)(object)obj).Equals(default(float)))
                                state = false;
                        }
                        break;
                }

                return state;
            }
            else
            {
                return chainedResponse;
            }
        }
    }

    /// <summary>
    /// Enumerable Extensions
    /// </summary>
    public static class EnumerablExtensions
    {
        public static int IndexOf(this object[] array, object obj)
        {
            var idx = -1;
            for (int x = 0; x < array.Length; x++)
            {
                if (array[x] == obj)
                {
                    idx = x;
                    break;
                }
            }
            return idx;
        }

        public static void AddRange<T>(this ICollection<T> collection, IEnumerable<T> items)
        {
            if (collection == null)
                throw new ArgumentNullException("collection");
            if (items == null)
                throw new ArgumentNullException("items");

            foreach (var item in items)
                collection.Add(item);
        }


        public static void RemoveRange<T>(this ICollection<T> collection, IEnumerable<T> items)
        {
            if (collection == null)
                throw new ArgumentNullException("collection");
            if (items == null)
                throw new ArgumentNullException("items");

            foreach (var item in items)
                collection.Remove(item);
        }

        public static async Task<ObservableCollection<T>> ToObservable<T>(this Task<List<T>> taskList, bool deepCopy = false) where T : new()
        {
            var result = await taskList;
            return result.ToObservable(deepCopy);
        }

        public static ObservableCollection<T> ToObservable<T>(this IList list, bool deepCopy = false) where T : new()
        {
            var collection = new ObservableCollection<T>();
            for (var x = 0; x < list.Count; x++)
            {
                var obj = (T)list[x];

                if (deepCopy)
                {
                    collection.Add(obj.DeepCopy());
                }
                else
                {
                    collection.Add(obj);
                }
            }
            return collection;
        }

        public static ObservableCollection<T> ToObservable<T>(this IEnumerable<T> list, bool deepCopy = false) where T : new()
        {
            var collection = new ObservableCollection<T>();
            foreach (var obj in list)
            {
                if (deepCopy)
                {
                    collection.Add(obj.DeepCopy());
                }
                else
                {
                    collection.Add(obj);
                }
            }
            return collection;
        }

        public static ObservableCollection<T> ToObservable<T>(this List<T> list, bool deepCopy = false) where T : new()
        {
            var collection = new ObservableCollection<T>();
            foreach (var obj in list)
            {
                if (deepCopy)
                {
                    collection.Add(obj.DeepCopy());
                }
                else
                {
                    collection.Add(obj);
                }
            }
            return collection;
        }

        public static ObservableCollection<T> ToObservable<T>(this IQueryable<T> query, bool deepCopy = false) where T : new()
        {
            var collection = new ObservableCollection<T>();
            foreach (var obj in query.AsEnumerable<T>())
            {
                if (deepCopy)
                {
                    collection.Add(obj.DeepCopy());
                }
                else
                {
                    collection.Add(obj);
                }
            }
            return collection;
        }

        public static ObservableCollection<T> ToObservable<T>(this T[] array, bool deepCopy = false) where T : new()
        {

            var collection = new ObservableCollection<T>();
            for (int x = 0; x < array.Length; x++)
            {
                if (deepCopy)
                {
                    collection.Add(array[x].DeepCopy());
                }
                else
                {
                    collection.Add(array[x]);
                }

            }

            return collection;
        }

        public static T[] AddItem<T>(this T[] array, T obj)
        {
            var lst = new List<T>();
            foreach (var item in array)
                lst.Add(item);
            lst.Add(obj);
            return lst.ToArray();
        }

        public static IList ToList(this IEnumerable enumerable)
        {
            return (IList)enumerable;
        }

        public static void RemoveWhere<T>(this ICollection<T> collection, Func<T, bool> predicate)
        {
            int i = collection.Count;

            while (--i > 0)
            {
                T element = collection.ElementAt(i);

                if (predicate(element))
                {
                    collection.Remove(element);
                }
            }
        }

        public static object ObjectAt(this IEnumerable enumerable, int index)
        {
            if (index < 0)
                return null;

            var list = (IList)enumerable;
            if (list.Count > index && index < (list.Count + 1))
            {
                return list[index];
            }
            else
            {
                return null;
            }
        }

        public static int IndexOf(this IEnumerable self, object obj)
        {
            int index = -1;

            var enumerator = self.GetEnumerator();
            enumerator.Reset();
            int i = 0;
            while (enumerator.MoveNext())
            {
                if (enumerator.Current == obj)
                {
                    index = i;
                    break;
                }

                i++;
            }

            return index;
        }
    }


    /// <summary>
    /// Image and File Extensions
    /// </summary>
    public static class ImageExtensions
    {
        public static byte[] ReadAllBytes(this Stream instream)
        {
            if (instream is MemoryStream)
                return ((MemoryStream)instream).ToArray();

            using (var memoryStream = new MemoryStream())
            {
                instream.CopyTo(memoryStream);
                return memoryStream.ToArray();
            }
        }

        public static async Task<string> ToBase64(this Stream stream)
        {
            var bytes = new byte[stream.Length];
            await stream.ReadAsync(bytes, 0, (int)stream.Length);
            return Convert.ToBase64String(bytes);
        }

        public static async Task<byte[]> ToByteArray(this Stream stream)
        {
            var bytes = new byte[stream.Length];
            await stream.ReadAsync(bytes, 0, (int)stream.Length);
            return bytes;
        }

        public static IImageSourceHandler GetHandler(this ImageSource source)
        {
            IImageSourceHandler returnValue = null;
            if (source is UriImageSource)
            {
                returnValue = new ImageLoaderSourceHandler();
            }
            else if (source is FileImageSource)
            {
                returnValue = new FileImageSourceHandler();
            }
            else if (source is StreamImageSource)
            {
                returnValue = new StreamImagesourceHandler();
            }
            else if (source is FontImageSource)
            {
                returnValue = new FontImageSourceHandler();
            }
            return returnValue;
        }

        public static async Task<byte[]> ResizedImageArray(this FileResult photo, double width, int compressionRate = 50)
        {
            var stream = await photo.OpenReadAsync();
            var array = await stream.ToByteArray();
            var size = CoreDependencyService.GetDependency<IImageManager>().GetSize(array);
            var newHeight = (float)size.Height;

            if (size.Width > width)
            {
                var percentChange = (float)(width / size.Width);
                newHeight = (float)(percentChange * size.Height);
            }

            return array.ResizeImage((float)width, newHeight, compressionRate);
        }

        public static byte[] ResizeImage(this byte[] imageData, float width, float height, int compressRatePercent = 30)
        {

#if __IOS__
            return ResizeImageIOS(imageData, width, height, compressRatePercent);
#endif
#if __ANDROID__
			return ResizeImageAndroid ( imageData, width, height, compressRatePercent );
#endif

        }


#if __IOS__
        private static byte[] ResizeImageIOS(byte[] imageData, float width, float height, int compressRatePercent)
        {
            UIImage originalImage = ImageFromByteArray(imageData);
            UIImageOrientation orientation = originalImage.Orientation;

            //create a 24bit RGB image
            using (CGBitmapContext context = new CGBitmapContext(IntPtr.Zero,
                                                 (int)width, (int)height, 8,
                                                 4 * (int)width, CGColorSpace.CreateDeviceRGB(),
                                                 CGImageAlphaInfo.PremultipliedFirst))
            {

                RectangleF imageRect = new RectangleF(0, 0, width, height);

                // draw the image
                context.DrawImage(imageRect, originalImage.CGImage);

                UIKit.UIImage resizedImage = UIKit.UIImage.FromImage(context.ToImage(), 0, orientation);

                // save the image as a jpeg
                var percent = (float)(compressRatePercent / 100);
                return resizedImage.AsJPEG(percent).ToArray();
            }
        }

        private static UIKit.UIImage ImageFromByteArray(byte[] data)
        {
            if (data == null)
            {
                return null;
            }

            UIKit.UIImage image;
            try
            {
                image = new UIKit.UIImage(Foundation.NSData.FromArray(data));
            }
            catch (Exception e)
            {
                Console.WriteLine("Image load failed: " + e.Message);
                return null;
            }
            return image;
        }
#endif

#if __ANDROID__
		
		public static byte[] ResizeImageAndroid (byte[] imageData, float width, float height, int compressRatePercent)
		{
			// Load the bitmap
			Bitmap originalImage = BitmapFactory.DecodeByteArray (imageData, 0, imageData.Length);
			Bitmap resizedImage = Bitmap.CreateScaledBitmap(originalImage, (int)width, (int)height, false);

			using (MemoryStream ms = new MemoryStream())
			{
				resizedImage.Compress (Bitmap.CompressFormat.Jpeg, compressRatePercent, ms);
				return ms.ToArray ();
			}
		}

#endif
    }
}




