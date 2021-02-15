using System;
using System.Text.RegularExpressions;
using System.Windows.Input;
using Xamarin.Forms;

namespace Xamarin.Forms.Core
{
	public enum StateChange
	{
		UnFocused,
		TextChanged
	}

	public class RegExBehavior : Behavior<Entry>
	{
		public static readonly BindableProperty CommandProperty = BindableProperty.Create("Command", typeof(ICommand), typeof(RegExBehavior), null);
		public static readonly BindableProperty StateChangeProperty = BindableProperty.Create("StateChange", typeof(StateChange), typeof(RegExBehavior), StateChange.UnFocused);
		public static readonly BindableProperty RegexExpProperty = BindableProperty.Create("RegexExp", typeof(string), typeof(RegExBehavior), null);
		public static readonly BindableProperty ErrorMessageProperty = BindableProperty.Create("ErrorMessage", typeof(string), typeof(RegExBehavior), null);
		public static readonly BindableProperty HasErrorProperty = BindableProperty.Create("HasError", typeof(bool), typeof(RegExBehavior), false);

		public ICommand Command
		{
			get { return (ICommand)this.GetValue(CommandProperty); }
			set { this.SetValue(CommandProperty, value); }
		}

		public StateChange StateChange
		{
			get { return (StateChange)this.GetValue(StateChangeProperty); }
			set { this.SetValue(StateChangeProperty, value); }
		}


		public string RegexExp
		{
			get { return (string)this.GetValue(RegexExpProperty); }
			set { this.SetValue(RegexExpProperty, value); }
		}


		public string ErrorMessage
		{
			get { return (string)this.GetValue(ErrorMessageProperty); }
			set { this.SetValue(ErrorMessageProperty, value); }
		}


		public bool HasError
		{
			get { return (bool)base.GetValue(HasErrorProperty); }
			private set { base.SetValue(HasErrorProperty, value); }
		}

		void HandleTextChanged(object sender, TextChangedEventArgs e)
		{
			if (e.NewTextValue != null)
			{
				Validate(e.NewTextValue);
			}
		}

		private void Bindable_Unfocused(object sender, FocusEventArgs e)
		{
			if (sender is Entry)
			{
				var txt = ((Entry)sender).Text;
				Validate(txt);
			}
		}

		protected override void OnAttachedTo(Entry bindable)
		{
			if (StateChange == StateChange.UnFocused)
			{
				bindable.Unfocused += Bindable_Unfocused;
			}
			else
			{
				bindable.TextChanged += HandleTextChanged;
			}

		}

		protected override void OnDetachingFrom(Entry bindable)
		{
			if (StateChange == StateChange.UnFocused)
			{
				bindable.Unfocused -= Bindable_Unfocused;
			}
			else
			{
				bindable.TextChanged -= HandleTextChanged;
			}

		}

		private void Validate(string text)
		{
			var isValid = (Regex.IsMatch(text, RegexExp, RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250)));
			HasError = !isValid;
			Command?.Execute(isValid);
		}
	}
}

