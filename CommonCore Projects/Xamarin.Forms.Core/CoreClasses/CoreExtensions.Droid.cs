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

        public static IVisualElementRenderer GetRenderer(this BindableObject bindableObject)
        {
            var value = bindableObject.GetValue(RendererProperty);
            return (IVisualElementRenderer)bindableObject.GetValue(RendererProperty);
        }

        public static Views.View GetNativeView(this BindableObject bindableObject)
        {
            var renderer = bindableObject.GetRenderer();
            var viewGroup = renderer.View;
            return viewGroup;
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
            return returnValue;
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
}
#endif
