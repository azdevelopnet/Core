#if __IOS__
using System;
using CoreAnimation;
using CoreGraphics;
using System.Linq;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Core;
using Xamarin.Forms.Platform.iOS;
using System.Collections.Generic;
using System.Collections;
using System.Reflection;
using System.Collections.Specialized;
using System.ComponentModel;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;

[assembly: ExportRenderer(typeof(CorePicker), typeof(CorePickerRenderer))]
namespace Xamarin.Forms.Core
{
    public class CorePickerRenderer : CoreNoCaretPickerRenderer
    {
        private CALayer bottomBorder;
        private CGColor controlColor;
        private CorePicker element;
        private UIPickerView pickerView;

        protected override void OnElementChanged(ElementChangedEventArgs<Picker> e)
        {
            base.OnElementChanged(e);
            if (e.NewElement != null && Control != null)
            {
                element = (CorePicker)e.NewElement;
                element.Focused += FocusChangedEvent;
                controlColor = element.EntryColor.ToCGColor();
                if (element.IsEntryUnderline)
                {
                    Control.BorderStyle = UITextBorderStyle.None;
                }
                pickerView = (UIPickerView)Control.InputView;
                
                var font = UIFont.FromName(element.FontFamily, (nfloat)element.FontSize);
                Control.Font = font;
            }
        }
        private void FocusChangedEvent(object sender, FocusEventArgs args){
            if(args.IsFocused && !string.IsNullOrEmpty(element.EmptyDataMessage))
            {
     
                var cnt = element.Items.Count();
                if (cnt == 0)
                {
                    pickerView.Hidden = true;
                    DependencyService.Get<IDialogPrompt>().ShowMessage(new Prompt()
                    {
                        Title = "Warning",
                        Message = element.EmptyDataMessage
                    });
                }
            }
        }
        protected override void OnElementPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (element.IsEntryUnderline)
            {
                if (e.PropertyName == CorePicker.WidthProperty.PropertyName)
                {
                    var width = ((CorePicker)sender).Width;
                    var height = ((CorePicker)sender).Height;
                    bottomBorder?.RemoveFromSuperLayer();
                    if (width > 0 && height > 0)
                        CreateUnderline((nfloat)height, (nfloat)width);
                }
                if (e.PropertyName == CorePicker.HeightProperty.PropertyName)
                {
                    var width = ((CorePicker)sender).Width;
                    var height = ((CorePicker)sender).Height;
                    bottomBorder?.RemoveFromSuperLayer();
                    if (width > 0 && height > 0)
                        CreateUnderline((nfloat)height, (nfloat)width);
                }

                if (e.PropertyName == CorePicker.SelectedIndexProperty.PropertyName)
                {
                    var item = Element.ItemsSource[Element.SelectedIndex];
                    if (Element.SelectedItem != item)
                        Element.SelectedItem = Element.ItemsSource[Element.SelectedIndex];
                }

                if (e.PropertyName == CorePicker.SelectedItemProperty.PropertyName)
                {
                    var index = Element.ItemsSource.IndexOf(Element.SelectedItem);
                    if (Element.SelectedIndex != index)
                        Element.SelectedIndex = Element.ItemsSource.IndexOf(Element.SelectedItem);
                }

            }

            if (e.PropertyName == CorePicker.FontFamilyProperty.PropertyName)
            {
                Element.FontFamily = ((CorePicker)sender).FontFamily;
            }
            if (e.PropertyName == CorePicker.FontSizeProperty.PropertyName)
            {
                Element.FontSize = ((CorePicker)sender).FontSize;
            }

            base.OnElementPropertyChanged(sender, e);

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

        private void CreateUnderline(nfloat height, nfloat width)
        {

            bottomBorder = new CALayer();
            bottomBorder.Frame = new CoreGraphics.CGRect(0, height - 1, width, 1);
            bottomBorder.BackgroundColor = controlColor;
            Control.Layer.AddSublayer(bottomBorder);

        }

    }
}


#endif
