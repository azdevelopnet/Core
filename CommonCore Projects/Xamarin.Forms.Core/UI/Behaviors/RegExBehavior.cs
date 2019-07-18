using System;
using System.Text.RegularExpressions;
using Xamarin.Forms;

namespace Xamarin.Forms.Core
{
	public class RegExBehavior : Behavior<Entry>
	{
		public static readonly BindableProperty RegexExpProperty =
			BindableProperty.Create("RegexExp",
									typeof(string),
									typeof(CoreUnderlineEntry),
									null);
		public string RegexExp
		{
			get { return (string)this.GetValue(RegexExpProperty); }
			set { this.SetValue(RegexExpProperty, value); }
		}

		public static readonly BindableProperty ErrorMessageProperty =
			BindableProperty.Create("ErrorMessage",
									typeof(string),
									typeof(CoreUnderlineEntry),
									null);
		public string ErrorMessage
		{
			get { return (string)this.GetValue(ErrorMessageProperty); }
			set { this.SetValue(ErrorMessageProperty, value); }
		}

		public static readonly BindableProperty HasErrorProperty =
			BindableProperty.Create("HasError",
									typeof(bool),
									typeof(RegExBehavior),
									false);

		public bool HasError
		{
			get { return (bool)base.GetValue(HasErrorProperty); }
			private set { base.SetValue(HasErrorProperty, value); }
		}

		void HandleTextChanged(object sender, TextChangedEventArgs e)
		{
			if (e.NewTextValue != null)
			{
				var isValid = (Regex.IsMatch(e.NewTextValue, RegexExp, RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250)));
				HasError = !isValid;
			}
		}

		protected override void OnAttachedTo(Entry bindable)
		{
			bindable.TextChanged += HandleTextChanged;
		}

		protected override void OnDetachingFrom(Entry bindable)
		{
			bindable.TextChanged -= HandleTextChanged;

		}
	}
}

