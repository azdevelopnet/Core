#if __ANDROID__
using System;
using System.Collections.Specialized;
using System.Linq;
using System.ComponentModel;
using Android.App;
using Android.Text;
using Android.Widget;
using Xamarin.Forms;
using Xamarin.Forms.Core;
using Xamarin.Forms.Platform.Android;
using Graphics = Android.Graphics;
using Views = Android.Views;
using Object = Java.Lang.Object;
using System.Collections.Generic;
using System.Collections;
using System.Reflection;
using Plugin.CurrentActivity;
using Android.Content;
using Android.Graphics;
using Android.Graphics.Drawables;
using RectShape = Android.Graphics.Drawables.Shapes.RectShape;

[assembly: ExportRenderer(typeof(CorePicker), typeof(CorePickerRenderer))]
namespace Xamarin.Forms.Core
{
    public class CorePickerRenderer : ViewRenderer<CorePicker, EditText>
    {
        private AlertDialog _dialog;
        private bool _disposed;
        private CorePicker element;

        public CorePickerRenderer(Context ctx) : base(ctx)
        {
            AutoPackage = false;
        }

        protected override EditText CreateNativeControl()
        {
            return new EditText(Context);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && !_disposed)
            {
                _disposed = true;

                ((INotifyCollectionChanged)Element.Items).CollectionChanged -= RowsCollectionChanged;
            }

            base.Dispose(disposing);
        }

        protected override void OnElementChanged(ElementChangedEventArgs<CorePicker> e)
        {
            if (e.OldElement != null)
                ((INotifyCollectionChanged)e.OldElement.Items).CollectionChanged -= RowsCollectionChanged;

            if (e.NewElement != null)
            {
                element = (CorePicker)e.NewElement;

                UpdatePickerPlaceholder();
                if (element.SelectedIndex <= -1)
                {
                    UpdatePickerPlaceholder();
                }

                ((INotifyCollectionChanged)e.NewElement.Items).CollectionChanged += RowsCollectionChanged;
                if (Control == null)
                {
                    EditText textField = CreateNativeControl();
                    textField.SetSingleLine(true);
                    textField.Focusable = false;
                    textField.Clickable = true;
                    textField.Tag = this;
                    textField.InputType = InputTypes.Null;
                    

                    textField.TextSize = (float)element.FontSize;
                    if (element.FontFamily!=null && element.FontFamily.IndexOf("#", StringComparison.CurrentCultureIgnoreCase) != -1)
                    {
                        var file = element.FontFamily.Split('#')[0];
                        var fontFace = Typeface.CreateFromAsset(this.Context.Assets, file);
                        textField.SetTypeface(fontFace, TypefaceStyle.Normal);
                    }

                    if (!element.IsEntryUnderline)
                    {
                        var shape = new ShapeDrawable(new RectShape());
                        shape.Paint.Color = Xamarin.Forms.Color.LightGray.ToAndroid();
                        shape.Paint.SetStyle(Paint.Style.Stroke);
                        textField.Background = shape;
                    }


                    if (e.NewElement != null)
                    {
                        if (element != null && element.EntryColor != null && textField != null)
                        {
                            textField.Background.Mutate().SetColorFilter(element.EntryColor.ToAndroid(), Graphics.PorterDuff.Mode.SrcAtop);
                        }
                    }

                    textField.SetOnClickListener(PickerListener.Instance);
                    SetNativeControl(textField);
                }
                else
                {
                    Control.TextSize = (float)element.FontSize;
                    if (element.FontFamily != null && element.FontFamily.IndexOf("#", StringComparison.CurrentCultureIgnoreCase) != -1)
                    {
                        var file = element.FontFamily.Split('#')[0];
                        var fontFace = Typeface.CreateFromAsset(this.Context.Assets, file);
                        Control.SetTypeface(fontFace, TypefaceStyle.Normal);
                    }
                }
                UpdatePicker();

            }

            base.OnElementChanged(e);
        }


        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            if (e.PropertyName == CorePicker.TitleProperty.PropertyName)
            {
                UpdatePicker();
            }
            if (e.PropertyName == CorePicker.SelectedIndexProperty.PropertyName)
            {
                UpdatePicker();

                var item = Element.ItemsSource[Element.SelectedIndex];
                if(Element.SelectedItem!=item)
                    Element.SelectedItem = Element.ItemsSource[Element.SelectedIndex];
            }
            if (e.PropertyName == CorePicker.SelectedItemProperty.PropertyName)
            {
                var index = Element.ItemsSource.IndexOf(Element.SelectedItem);
                if(Element.SelectedIndex!=index)
                    Element.SelectedIndex = Element.ItemsSource.IndexOf(Element.SelectedItem);
                
                UpdatePicker();

            }

            if (element != null)
            {
                if (e.PropertyName.Equals(CorePicker.PlaceholderProperty.PropertyName))
                {
                    UpdatePickerPlaceholder();
                }
            }

        }

        protected override void OnFocusChanged(bool gainFocus, Views.FocusSearchDirection direction, Graphics.Rect previouslyFocusedRect)
        {
            base.OnFocusChanged(gainFocus, direction, previouslyFocusedRect);

            if (gainFocus)
            {
                OnClick();
            }
            else
            {
                _dialog.Hide();
                ((IElementController)Element).SetValueFromRenderer(VisualElement.IsFocusedProperty, false);
                Control.ClearFocus();
                _dialog = null;
            }
        }


        void OnClick()
        {
            if (!string.IsNullOrEmpty(element.EmptyDataMessage))
            {
                var cnt = element.Items.Count();
                if (cnt == 0)
                {
                    _dialog?.Hide();
                    DependencyService.Get<IDialogPrompt>().ShowMessage(new Prompt()
                    {
                        Title="Warning",
                        Message = element.EmptyDataMessage
                    });
                    return;
                }
            }

            Picker model = Element;
            if (_dialog == null)
            {
                using (var builder = new AlertDialog.Builder(Context))
                {
                    builder.SetTitle(model.Title ?? "");
                    string[] items = GetPickerDisplayValues();
                    builder.SetItems(items, (s, e) => ((IElementController)model).SetValueFromRenderer(Picker.SelectedIndexProperty, e.Which));

                    builder.SetNegativeButton(global::Android.Resource.String.Cancel, (o, args) => { });

                    ((IElementController)Element).SetValueFromRenderer(VisualElement.IsFocusedProperty, true);

                    _dialog = builder.Create();
                }
                _dialog.SetCanceledOnTouchOutside(true);
                _dialog.DismissEvent += (sender, args) =>
                {
                    (Element as IElementController)?.SetValueFromRenderer(VisualElement.IsFocusedProperty, false);
                    _dialog.Dispose();
                    _dialog = null;
                };

                _dialog.Show();
            }
        }

        private string[] GetPickerDisplayValues()
        {
            if (string.IsNullOrEmpty(element.BindingPath))
            {
                return Element.Items.ToArray();
            }
            else
            {
                var lst = new List<string>();
                var collection = element.ItemsSource as IEnumerable;
                PropertyInfo prop = null;
                foreach (var item in collection)
                {
                    if (prop == null)
                    {
                        prop = item.GetType().GetProperty(element.BindingPath);
                    }
                    lst.Add(prop.GetValue(item, null).ToString());
                }
                return lst.ToArray();
            }
        }

        void RowsCollectionChanged(object sender, EventArgs e)
        {
            UpdatePicker();
        }

        void UpdatePicker()
        {
            Control.Hint = Element.Title;

            if (Element.SelectedIndex == -1 || Element.Items == null)
                Control.Text = null;
            else
                Control.Text = GetPickerDisplayValues()[Element.SelectedIndex];
        }

        void UpdatePickerPlaceholder()
        {
 
            if (element == null)
                element = Element as CorePicker;
            if (element.Placeholder != null && Control!=null)
                Control.Hint = element.Placeholder;
        }


        class PickerListener : Object, IOnClickListener
        {
#region Statics

            public static readonly PickerListener Instance = new PickerListener();

#endregion

            public void OnClick(global::Android.Views.View v)
            {
                var renderer = v.Tag as CorePickerRenderer;
                renderer?.OnClick();
            }
        }
    }
}
#endif
