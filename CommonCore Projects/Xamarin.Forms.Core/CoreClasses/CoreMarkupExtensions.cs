using System;
using System.Collections;
using Xamarin.Forms;
using Xamarin.Forms.Core;
using System.Threading.Tasks;

#if __ANDROID__
using Xamarin.Forms.Platform.Android;
using EventTypes = Android.Views.Accessibility.EventTypes;
using Android.Views.Accessibility;
using XPViews = Android.Views;
using XPView = Android.Views.View;
using Plugin.CurrentActivity;
using XFPlatform = Xamarin.Forms.Platform.Android.Platform;
#else
using XFPlatform = Xamarin.Forms.Platform.iOS.Platform;
using Xamarin.Forms.Platform.iOS;
using Foundation;
using CoreGraphics;
using UIKit;
#endif

namespace Xamarin.CommunityToolkit.Markup
{
    public static class CoreMarkupExtensions
    {

        /// <summary>
        /// Sets AutomationId and StyleId
        /// </summary>
        /// <typeparam name="TView"></typeparam>
        /// <param name="view"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public static TView ElementId<TView>(this TView view, string id) where TView : View
        {
            view.AutomationId = id;
            view.StyleId = id;
            return view;
        }

        public static TView BindEffect<TView>(this TView view, PlatformEffect effect) where TView : View
        {
            view.Effects.Add(effect);
            return view;
        }

        /// <summary>
        /// Execute an accessiblity announcement
        /// </summary>
        /// <typeparam name="TView"></typeparam>
        /// <param name="view"></param>
        /// <param name="text"></param>
        /// <param name="attributtedEnums">Is for iOS UIAccessibilityPostNotification</param>
        public static void AccessibilityAnnounce<TView>(this TView view, string text, AccessbillityNotificationType notificationTypeEnum = AccessbillityNotificationType.Announcement) where TView : View
        {

#if __ANDROID__
            var nView = view.GetOrCreateRenderer();
            if (nView != null)
            {
                nView.View.AnnounceForAccessibility(text);
            }
#else

            var nView = view.GetOrCreateRenderer();
            if (nView != null)
            {
                var notificationType = notificationTypeEnum.ToString();
                var typeEnum = ParseEnum<UIAccessibilityPostNotification>(notificationType);
                UIAccessibility.PostNotification(typeEnum, new NSString(text));
            }
#endif


        }

        public static void AccessibilityEnabled<TView>(this TView view, bool enabled) where TView : View
        {
            AutomationProperties.SetIsInAccessibleTree(view, enabled);
        }


        /// <summary>
        /// Enable accesibility
        /// </summary>
        /// <typeparam name="TView"></typeparam>
        /// <param name="view"></param>
        /// <param name="name"></param>
        /// <param name="helpText"></param>
        /// <param name="attributtedEnums"></param>
        /// <param name="isTabStop"></param>
        /// <param name="tabIndex"></param>
        /// <returns></returns>
        public static TView EnableAccessibility<TView>(this TView view, string name, string helpText, FieldAccessibilityType attributtedEnums = FieldAccessibilityType.None, bool? isTabStop = true, int? tabIndex = null) where TView : View
        {
            AutomationProperties.SetIsInAccessibleTree(view, true);
            AutomationProperties.SetName(view, name);
            AutomationProperties.SetHelpText(view, helpText);
            if (isTabStop.HasValue)
                view.IsTabStop = isTabStop.Value;
            if (tabIndex.HasValue)
                view.TabIndex = tabIndex.Value;

#if __IOS__

            if (attributtedEnums!= FieldAccessibilityType.None)
            {
                var renderer = view.GetOrCreateRenderer();
                if (renderer != null)
                {
                    var nativeView = renderer.NativeView;
                    var aeString = attributtedEnums.ToString();
                    var traitEnum = ParseEnum<UIAccessibilityTrait>(aeString);
                    nativeView.AccessibilityTraits = traitEnum;
                }
            }
#endif
            return view;
        }

        public static TView SetThemeColor<TView>(this TView view, BindableProperty property, Color light, Color dark) where TView : View
        {
            view.SetAppThemeColor(property, light, dark);
            return view;
        }

        public static TView SetOnThemeColor<TView, TObect>(this TView view, BindableProperty property, TObect lightValue, TObect darkValue) where TView : View
            where TObect: class
        {
            view.SetOnAppTheme<TObect>(property, lightValue, darkValue);
            return view;
        }

        public static TView DisableAccessibility<TView>(this TView view) where TView : View
        {
            AutomationProperties.SetIsInAccessibleTree(view, false);
            return view;
        }

        public static TView SetTabIndex<TView>(this TView view, int index) where TView : View
        {
            view.TabIndex = index;
            return view;
        }

        public static SearchBar BindSearchEvent(this SearchBar bar, string bindCommand, string bindText)
        {
            bar.Bind(SearchBar.SearchCommandProperty, bindCommand)
                .Bind(SearchBar.TextProperty, bindText)
                .BindEventCommand(new EventToCommandBehavior()
                {
                    EventName = nameof(SearchBar.TextChanged),
                    Command = new Command((obj) =>
                    {
                        var txt = ((SearchBar)obj).Text;
                        if (string.IsNullOrEmpty(txt))
                        {
                            bar.SearchCommand?.Execute(null);
                        }
                    })
                });
            return bar;
        }

        public static T BindItemTemplate<T>(this T layout, DataTemplate template) where T : Layout
        {
            BindableLayout.SetItemTemplate(layout, template);
            return layout;
        }
        public static T BindItemTemplateSelector<T>(this T layout, DataTemplateSelector selector) where T : Layout
        {
            BindableLayout.SetItemTemplateSelector(layout, selector);
            return layout;
        }

        public static TView BindBehavior<TView>(this TView view, Behavior behavior) where TView : View
        {
            view.Behaviors.Add(behavior);
            return view;
        }

        public static TView BindBehavior<TView>(this TView view, Behavior behavior, out Behavior variable) where TView : View
        {
            variable = behavior;
            view.Behaviors.Add(variable);
            return view;
        }

        public static TView BindTap<TView>(this TView view, System.Action action) where TView : View
        {
            var gesture = new TapGestureRecognizer()
            {
                Command = new Command(() => { action?.Invoke(); })
            };
            view.GestureRecognizers.Add(gesture);
            return view;
        }

        public static TView BindTap<TView>(this TView view, Action<object> action) where TView : View
        {
            var gesture = new TapGestureRecognizer()
            {
                Command = new Command(() => { action?.Invoke(view); })
            };
            view.GestureRecognizers.Add(gesture);
            return view;
        }

        public static TGestureElement BindTap<TGestureElement>(this TGestureElement gestureElement, string commandName) where TGestureElement : GestureElement
        {
            var gesture = new TapGestureRecognizer();
            gesture.SetBinding(TapGestureRecognizer.CommandProperty, commandName);
            gestureElement.GestureRecognizers.Add(gesture);
            return gestureElement;
        }
        public static TGestureElement BindTap<TGestureElement>(this TGestureElement view, System.Action action, string nonsense = null) where TGestureElement : GestureElement
        {
            var gesture = new TapGestureRecognizer()
            {
                Command = new Command(() => { action?.Invoke(); })
            };
            view.GestureRecognizers.Add(gesture);
            return view;
        }
        public static TView BindEventCommand<TView>(this TView view, EventToCommandBehavior behaviorCommand, bool ReturnEventArgs=false) where TView : VisualElement
        {
            if(!ReturnEventArgs)
                behaviorCommand.CommandParameter = view;
            view.Behaviors.Add(behaviorCommand);
            return view;
        }

        public static TView BindCommandParameter<TView>(this TView view, BindableProperty targetProperty, View element) where TView : Element
        {
            view.SetBinding(targetProperty, new Binding() { Source = element });
            return view;
        }

        public static TView BindCommandParameter<TView>(this TView view, BindableProperty targetProperty, View element, string path) where TView : Element
        {
            view.SetBinding(targetProperty, new Binding() { Source = element, Path = path });
            return view;
        }

        /// <summary>
        /// Must be added after all controls involved have been assigned
        /// </summary>
        public static TView TriggerByView<TView>(this TView view, View nestedView, Binding binding, Setter setter) where TView : View
        {
            var trigger = new DataTrigger(typeof(View));
            trigger.Binding = binding;
            trigger.Value = true;
            trigger.Setters.Add(setter);
            nestedView.Triggers.Add(trigger);
            return view;
        }

#region Headless Compression for Android
        public static StackLayout IsHeadless(this StackLayout layout)
        {
            if (CoreSettings.OS == DeviceOS.ANDROID)
            {
                CompressedLayout.SetIsHeadless(layout, true);
            }
            return layout;
        }
        public static AbsoluteLayout IsHeadless(this AbsoluteLayout layout)
        {
            if (CoreSettings.OS == DeviceOS.ANDROID)
            {
                CompressedLayout.SetIsHeadless(layout, true);
            }
            return layout;
        }
        public static Grid IsHeadless(this Grid layout)
        {
            if (CoreSettings.OS == DeviceOS.ANDROID)
            {
                CompressedLayout.SetIsHeadless(layout, true);
            }
            return layout;
        }
        public static RelativeLayout IsHeadless(this RelativeLayout layout)
        {
            if (CoreSettings.OS == DeviceOS.ANDROID)
            {
                CompressedLayout.SetIsHeadless(layout, true);
            }
            return layout;
        }
        #endregion

        private static T ParseEnum<T>(string value)
        {
            return (T)Enum.Parse(typeof(T), value, true);
        }
    }
}
