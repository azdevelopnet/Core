using System;
using System.ComponentModel;
using Xamarin.Forms;

namespace Xamarin.Forms.Core
{
	public enum ReturnKeyTypes : int
	{
		Default,
		Go,
		Google,
		Join,
		Next,
		Route,
		Search,
		Send,
		Yahoo,
		Done,
		EmergencyCall,
		Continue
	}

    [DesignTimeVisible(true)]
    public class CoreUnderlineEntry : CoreMaskedEntry
	{
		public Action NextFocus { get; set; }

		public static readonly BindableProperty ReturnKeyTypeProperty =
				BindableProperty.Create(
				   propertyName: "ReturnKeyType",
				   returnType: typeof(ReturnKeyTypes),
				   declaringType: typeof(CoreUnderlineEntry),
				   defaultValue: ReturnKeyTypes.Done);

		public ReturnKeyTypes ReturnKeyType
		{
			get { return (ReturnKeyTypes)GetValue(ReturnKeyTypeProperty); }
			set { SetValue(ReturnKeyTypeProperty, value); }
		}

		public static readonly BindableProperty HasErrorProperty =
			BindableProperty.Create("HasError",
									typeof(bool),
									typeof(CoreUnderlineEntry),
									false);
		public bool HasError
		{
			get { return (bool)this.GetValue(HasErrorProperty); }
			set { this.SetValue(HasErrorProperty, value); }
		}

        public static readonly BindableProperty ClearEntryEnabledProperty =
            BindableProperty.Create("ClearEntryEnabled",
                            typeof(bool),
                            typeof(CoreUnderlineEntry),
                            false);
        
        public bool ClearEntryEnabled
        {
            get { return (bool)this.GetValue(ClearEntryEnabledProperty); }
            set { this.SetValue(ClearEntryEnabledProperty, value); }
        }

        /// <summary>
        /// This property sets the clear icon on the right of the entry field (Android only)
        /// </summary>
        public static readonly BindableProperty ClearEntryIconProperty =
            BindableProperty.Create("ClearEntryIcon",
                            typeof(string),
                            typeof(CoreUnderlineEntry),
                            null);

        /// <summary>
        /// This property sets the clear icon on the right of the entry field (Android only)
        /// </summary>
        /// <value>The clear entry icon.</value>
        public string ClearEntryIcon
        {
            get { return (string)this.GetValue(ClearEntryIconProperty); }
            set { this.SetValue(ClearEntryIconProperty, value); }
        }


		public static readonly BindableProperty IconProperty =
			BindableProperty.Create("Icon",
									typeof(string),
									typeof(CoreUnderlineEntry),
									null);
		public string Icon
		{
			get { return (string)this.GetValue(IconProperty); }
			set { this.SetValue(IconProperty, value); }
		}


		public static readonly BindableProperty EntryColorProperty =
			BindableProperty.Create("EntryColor",
									typeof(Color),
									typeof(CoreUnderlineEntry),
									Color.Black);
		public Color EntryColor
		{
			get { return (Color)this.GetValue(EntryColorProperty); }
			set { this.SetValue(EntryColorProperty, value); }
		}


		public static readonly BindableProperty PasswordRevealEnabledProperty =
			BindableProperty.Create("PasswordRevealEnabled",
									typeof(bool),
									typeof(CoreUnderlineEntry),
									false);

		public bool PasswordRevealEnabled
		{
			get { return (bool)this.GetValue(PasswordRevealEnabledProperty); }
			set { this.SetValue(PasswordRevealEnabledProperty, value); }
		}

        public static readonly BindableProperty PasswordRevealIconProperty =
            BindableProperty.Create("PasswordRevealIcon",
                            typeof(string),
                            typeof(CoreUnderlineEntry),
                            null);

        public string PasswordRevealIcon
        {
            get { return (string)this.GetValue(PasswordRevealIconProperty); }
            set { this.SetValue(PasswordRevealIconProperty, value); }
        }

        public static readonly BindableProperty PasswordHideIconProperty =
            BindableProperty.Create("PasswordHideIcon",
                    typeof(string),
                    typeof(CoreUnderlineEntry),
                    null);

        public string PasswordHideIcon
        {
            get { return (string)this.GetValue(PasswordHideIconProperty); }
            set { this.SetValue(PasswordHideIconProperty, value); }
        }


		public CoreUnderlineEntry()
		{
			this.Completed += ReturnKeyHandler;
		}
		~CoreUnderlineEntry()
		{
			this.Completed -= ReturnKeyHandler;
		}

		private void ReturnKeyHandler(object sender, EventArgs args)
		{
			NextFocus?.Invoke();
		}

	}
}

