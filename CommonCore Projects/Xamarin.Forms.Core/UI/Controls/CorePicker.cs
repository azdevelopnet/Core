using System;
using System.Collections;
using System.Collections.Specialized;
using Xamarin.Forms;
using System.Linq;
using System.Collections.Generic;
using System.Reflection;

namespace Xamarin.Forms.Core
{
    //public class CorePickerItem
    //{
    //    public int ID { get; set; }
    //    public string OptionText { get; set; }
    //}
    public class CorePicker : Picker
    {

        public static readonly BindableProperty IsEntryUnderlineProperty =
            BindableProperty.Create("IsEntryUnderline",
                                    typeof(bool),
                                    typeof(CorePicker),
                                    true);


        public bool IsEntryUnderline
        {
            get { return (bool)this.GetValue(IsEntryUnderlineProperty); }
            set { this.SetValue(IsEntryUnderlineProperty, value); }
        }


        public static readonly BindableProperty EmptyDataMessageProperty =
            BindableProperty.Create("EmptyDataMessage",
                                    typeof(string),
                                    typeof(CorePicker),
                                    null);


        public string EmptyDataMessage
        {
            get { return (string)this.GetValue(EmptyDataMessageProperty); }
            set { this.SetValue(EmptyDataMessageProperty, value); }
        }

        public static readonly BindableProperty BindingPathProperty =
            BindableProperty.Create("BindingPath",
                                    typeof(string),
                                    typeof(CorePicker),
                                    string.Empty);
        public string BindingPath
        {
            get { return (string)this.GetValue(BindingPathProperty); }
            set { this.SetValue(BindingPathProperty, value); }
        }

        public static readonly BindableProperty EntryColorProperty =
            BindableProperty.Create("EntryColor",
                                    typeof(Color),
                                    typeof(CorePicker),
                                    Color.Black);
        public Color EntryColor
        {
            get { return (Color)this.GetValue(EntryColorProperty); }
            set { this.SetValue(EntryColorProperty, value); }
        }


    }
}

