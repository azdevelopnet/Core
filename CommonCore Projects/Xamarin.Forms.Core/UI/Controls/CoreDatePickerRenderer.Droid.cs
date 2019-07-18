#if __ANDROID__
using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using Android.App;
using Android.Text;
using Android.Widget;
using Xamarin.Forms;
using Xamarin.Forms.Core;
using Xamarin.Forms.Platform.Android;
using AView = Android.Views.View;
using Views = Android.Views;
using Graphics = Android.Graphics;
using Object = Java.Lang.Object;
using Plugin.CurrentActivity;
using Android.Content;

[assembly: ExportRenderer(typeof(CoreDatePicker), typeof(CoreDatePickerRenderer))]
namespace Xamarin.Forms.Core
{
    public class CoreDatePickerRenderer : ViewRenderer<CoreDatePicker, EditText>
    {
        DatePickerDialog _dialog;
        bool _disposed;

        public CoreDatePickerRenderer(Context ctx) :base(ctx)
        {

        }
        protected override void Dispose(bool disposing)
        {
            if (disposing && !_disposed)
            {
                _disposed = true;
                if (_dialog != null)
                {
                    _dialog.CancelEvent -= OnCancelButtonClicked;
                    _dialog.Hide();
                    _dialog.Dispose();
                    _dialog = null;
                }
            }
            base.Dispose(disposing);
        }

        protected override EditText CreateNativeControl()
        {
            return new EditText(Context) { Focusable = false, Clickable = true, Tag = this };
        }

        protected override void OnElementChanged(ElementChangedEventArgs<CoreDatePicker> e)
        {
            base.OnElementChanged(e);

            if (e.OldElement == null)
            {
                var textField = CreateNativeControl();
                textField.SetSingleLine(true);

                if (e.NewElement != null)
                {
                    var ctrl = (CoreDatePicker)e.NewElement;
                    if (ctrl != null && ctrl.EntryColor != null && textField != null)
                    {
                        textField.Background.Mutate().SetColorFilter(ctrl.EntryColor.ToAndroid(), Graphics.PorterDuff.Mode.SrcAtop);
                    }
                }

                textField.SetOnClickListener(TextFieldClickHandler.Instance);
                SetNativeControl(textField);
            }

            SetDate(Element.Date);

            UpdateMinimumDate();
            UpdateMaximumDate();
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            if (e.PropertyName == CoreDatePicker.DateProperty.PropertyName || e.PropertyName == CoreDatePicker.FormatProperty.PropertyName)
                SetDate(Element.Date);
            else if (e.PropertyName == CoreDatePicker.MinimumDateProperty.PropertyName)
                UpdateMinimumDate();
            else if (e.PropertyName == CoreDatePicker.MaximumDateProperty.PropertyName)
                UpdateMaximumDate();

        }

        protected override void OnFocusChanged(bool gainFocus, Views.FocusSearchDirection direction, Graphics.Rect previouslyFocusedRect)
        {
            base.OnFocusChanged(gainFocus, direction, previouslyFocusedRect);

            if (gainFocus)
            {
                OnTextFieldClicked();
            }
            else
            {
                _dialog.Hide();
                ((IElementController)Element).SetValueFromRenderer(VisualElement.IsFocusedProperty, false);
                Control.ClearFocus();
                _dialog.CancelEvent -= OnCancelButtonClicked;
                _dialog = null;
            }
        }


        void CreateDatePickerDialog(int year, int month, int day)
        {
            CoreDatePicker view = Element;
            _dialog = new DatePickerDialog(Context, (o, e) =>
            {
                view.Date = e.Date;
                ((IElementController)view).SetValueFromRenderer(VisualElement.IsFocusedProperty, false);
                Control.ClearFocus();


                _dialog.CancelEvent -= OnCancelButtonClicked;

                _dialog = null;
            }, year, month, day);
        }

        void DeviceInfoPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "CurrentOrientation")
            {
                DatePickerDialog currentDialog = _dialog;
                if (currentDialog != null && currentDialog.IsShowing)
                {
                    currentDialog.Dismiss();
                    CreateDatePickerDialog(currentDialog.DatePicker.Year, currentDialog.DatePicker.Month, currentDialog.DatePicker.DayOfMonth);
                    _dialog.Show();
                }
            }
        }

        void OnTextFieldClicked()
        {
            CoreDatePicker view = Element;
            ((IElementController)view).SetValueFromRenderer(VisualElement.IsFocusedProperty, true);

            CreateDatePickerDialog(view.Date.Year, view.Date.Month - 1, view.Date.Day);

            UpdateMinimumDate();
            UpdateMaximumDate();

            _dialog.CancelEvent += OnCancelButtonClicked;

            _dialog.Show();
        }

        void OnCancelButtonClicked(object sender, EventArgs e)
        {
            Element.Unfocus();
        }

        void SetDate(DateTime date)
        {
            Control.Text = date.ToString(Element.Format);
        }

        void UpdateMaximumDate()
        {
            if (_dialog != null)
            {
                _dialog.DatePicker.MaxDate = (long)Element.MaximumDate.ToUniversalTime().Subtract(DateTime.MinValue.AddYears(1969)).TotalMilliseconds;
            }
        }

        void UpdateMinimumDate()
        {
            if (_dialog != null)
            {
                _dialog.DatePicker.MinDate = (long)Element.MinimumDate.ToUniversalTime().Subtract(DateTime.MinValue.AddYears(1969)).TotalMilliseconds;
            }
        }

        class TextFieldClickHandler : Object, IOnClickListener
        {
            public static readonly TextFieldClickHandler Instance = new TextFieldClickHandler();

            public void OnClick(AView v)
            {
                ((CoreDatePickerRenderer)v.Tag).OnTextFieldClicked();
            }
        }
    }

}
#endif
