using System.ComponentModel;

namespace Xamarin.Forms.Core
{
    [DesignTimeVisible(true)]
    public class CorePicker : Picker
    {

        public static readonly BindableProperty PlaceholderProperty =
            BindableProperty.Create("Placeholder",
                                    typeof(string),
                                    typeof(CorePicker),
                                    string.Empty);

        public string Placeholder
        {
            get { return (string)GetValue(PlaceholderProperty); }
            set { SetValue(PlaceholderProperty, value); }
        }

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

