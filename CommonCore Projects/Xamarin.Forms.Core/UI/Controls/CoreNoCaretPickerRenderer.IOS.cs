#if __IOS__
using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Reflection;
using CoreGraphics;
using UIKit;
using Xamarin.Forms.Platform.iOS;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;
using System.Collections;

namespace Xamarin.Forms.Core
{
    public class NoCaretField : UITextField
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NoCaretField"/> class.
        /// </summary>
        public NoCaretField() : base(default(CGRect))
        {
        }

        /// <summary>
        /// Gets the caret rect for position.
        /// </summary>
        /// <param name="position">The position.</param>
        /// <returns>RectangleF.</returns>
        public override CGRect GetCaretRectForPosition(UITextPosition position)
        {
            return default(CGRect);
        }
    }

    public class CoreNoCaretPickerRenderer : ViewRenderer<Picker, UITextField>
    {
        UIPickerView _picker;
        UIColor _defaultTextColor;
        bool _disposed;
        CorePicker element;

        IElementController ElementController => Element as IElementController;

        protected override void OnElementChanged(ElementChangedEventArgs<Picker> e)
        {
            if (e.OldElement != null)
                ((INotifyCollectionChanged)e.OldElement.Items).CollectionChanged -= RowsCollectionChanged;

            if (e.NewElement != null)
            {
                element = (CorePicker)e.NewElement;

                if (Control == null)
                {
                    var entry = new NoCaretField { BorderStyle = UITextBorderStyle.RoundedRect };

                    entry.EditingDidBegin += OnStarted;
                    entry.EditingDidEnd += OnEnded;
                    entry.EditingChanged += OnEditing;

                    _picker = new UIPickerView();

                    var width = UIScreen.MainScreen.Bounds.Width;
                    var toolbar = new UIToolbar(new CGRect(0, 0, width, 44)) { BarStyle = UIBarStyle.Default, Translucent = true };
                    var spacer = new UIBarButtonItem(UIBarButtonSystemItem.FlexibleSpace);
                    var doneButton = new UIBarButtonItem(UIBarButtonSystemItem.Done, (o, a) =>
                    {
                        var s = (PickerSource1)_picker.Model;
                        if (s.SelectedIndex == -1 && Element.Items != null && Element.Items.Count > 0)
                            UpdatePickerSelectedIndex(0);
                        UpdatePickerFromModel(s);
                        entry.ResignFirstResponder();
                    });

                    toolbar.SetItems(new[] { spacer, doneButton }, false);

                    entry.InputView = _picker;
                    entry.InputAccessoryView = toolbar;

                    _defaultTextColor = entry.TextColor;

                    SetNativeControl(entry);
                }

                _picker.Model = new PickerSource1(this);

                UpdatePicker();
                UpdateTextColor();

                ((INotifyCollectionChanged)e.NewElement.Items).CollectionChanged += RowsCollectionChanged;
            }

            base.OnElementChanged(e);
        }

        public string[] CoreItems
        {
            get
            {

                if (string.IsNullOrEmpty(element.BindingPath))
                {
                    return element.Items.ToArray();
                }
                else
                {
                    if (element.ItemsSource == null)
                        return new string[] { };

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
        }
        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);
            if (e.PropertyName == Picker.TitleProperty.PropertyName)
            {
                UpdatePicker();
            }
            if (e.PropertyName == Picker.SelectedIndexProperty.PropertyName)
            {
                UpdatePicker();
            }
            if (e.PropertyName == Picker.TextColorProperty.PropertyName || e.PropertyName == VisualElement.IsEnabledProperty.PropertyName)
            {
                UpdateTextColor();
            }
        }

        void OnEditing(object sender, EventArgs eventArgs)
        {
            // Reset the TextField's Text so it appears as if typing with a keyboard does not work.
            var selectedIndex = Element.SelectedIndex;
            Control.Text = selectedIndex == -1 || CoreItems == null ? "" : CoreItems[selectedIndex];
        }

        void OnEnded(object sender, EventArgs eventArgs)
        {
            var s = (PickerSource1)_picker.Model;
            if (s.SelectedIndex != _picker.SelectedRowInComponent(0))
            {
                _picker.Select(s.SelectedIndex, 0, false);
            }
            ElementController.SetValueFromRenderer(VisualElement.IsFocusedPropertyKey, false);
        }

        void OnStarted(object sender, EventArgs eventArgs)
        {
            ElementController.SetValueFromRenderer(VisualElement.IsFocusedPropertyKey, true);
        }

        void RowsCollectionChanged(object sender, EventArgs e)
        {
            UpdatePicker();
        }

        void UpdatePicker()
        {
            var selectedIndex = Element.SelectedIndex;
            Control.Placeholder = Element.Title;
            var oldText = Control.Text;
            Control.Text = selectedIndex == -1 || CoreItems == null ? "" : CoreItems[selectedIndex];
            UpdatePickerNativeSize(oldText);
            _picker.ReloadAllComponents();
            if (CoreItems == null || CoreItems.Length == 0)
                return;

            UpdatePickerSelectedIndex(selectedIndex);
        }

        void UpdatePickerFromModel(PickerSource1 s)
        {
            if (Element != null)
            {
                var oldText = Control.Text;
                Control.Text = CoreItems[s.SelectedIndex];
                UpdatePickerNativeSize(oldText);
                ElementController.SetValueFromRenderer(Picker.SelectedIndexProperty, s.SelectedIndex);
            }
        }

        void UpdatePickerNativeSize(string oldText)
        {
            if (oldText != Control.Text)
                ((IVisualElementController)Element).NativeSizeChanged();
        }

        void UpdatePickerSelectedIndex(int formsIndex)
        {
            var source = (PickerSource1)_picker.Model;
            source.SelectedIndex = formsIndex;
            source.SelectedItem = formsIndex >= 0 ? Element.Items[formsIndex] : null;
            _picker.Select(Math.Max(formsIndex, 0), 0, true);
        }

        void UpdateTextColor()
        {
            var textColor = Element.TextColor;

            if (textColor.IsDefault || !Element.IsEnabled)
                Control.TextColor = _defaultTextColor;
            else
                Control.TextColor = textColor.ToUIColor();
        }

        protected override void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            _disposed = true;

            if (disposing)
            {
                _defaultTextColor = null;

                if (_picker != null)
                {
                    if (_picker.Model != null)
                    {
                        _picker.Model.Dispose();
                        _picker.Model = null;
                    }

                    _picker.RemoveFromSuperview();
                    _picker.Dispose();
                    _picker = null;
                }

                if (Control != null)
                {
                    Control.EditingDidBegin -= OnStarted;
                    Control.EditingDidEnd -= OnEnded;
                }

                if (Element != null)
                    ((INotifyCollectionChanged)Element.Items).CollectionChanged -= RowsCollectionChanged;
            }

            base.Dispose(disposing);
        }

        class PickerSource1 : UIPickerViewModel
        {
            CoreNoCaretPickerRenderer _renderer;
            bool _disposed;

            public PickerSource1(CoreNoCaretPickerRenderer renderer)
            {
                _renderer = renderer;
            }

            public int SelectedIndex { get; internal set; }

            public object SelectedItem { get; internal set; }

            public override nint GetComponentCount(UIPickerView picker)
            {
                return 1;
            }

            public override nint GetRowsInComponent(UIPickerView pickerView, nint component)
            {
                return _renderer.Element.Items != null ? _renderer.Element.Items.Count : 0;
            }

            public override string GetTitle(UIPickerView picker, nint row, nint component)
            {
                return _renderer.CoreItems[(int)row];
            }

            public override void Selected(UIPickerView picker, nint row, nint component)
            {
                if (_renderer.Element.Items.Count == 0)
                {
                    SelectedItem = null;
                    SelectedIndex = -1;
                }
                else
                {
                    SelectedItem = _renderer.Element.Items[(int)row];
                    SelectedIndex = (int)row;
                }

                if (_renderer.Element.On<PlatformConfiguration.iOS>().UpdateMode() == UpdateMode.Immediately)
                    _renderer.UpdatePickerFromModel(this);
            }

            protected override void Dispose(bool disposing)
            {
                if (_disposed)
                    return;

                _disposed = true;

                if (disposing)
                    _renderer = null;

                base.Dispose(disposing);
            }
        }
    }
}
#endif



