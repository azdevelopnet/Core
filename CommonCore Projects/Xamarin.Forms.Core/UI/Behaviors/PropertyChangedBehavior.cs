using System;
using System.ComponentModel;
using Xamarin.Forms;

namespace Xamarin.Forms.Core
{
	public class PropertyChangedBehavior : Behavior<View>
	{
		private INotifyPropertyChanged notifier;
		private Action<string, View> action;
		private View view;

		public PropertyChangedBehavior(INotifyPropertyChanged obj, Action<string, View> propertyChanged)
		{
			notifier = obj;
			action = propertyChanged;
		}
		protected override void OnAttachedTo(View bindable)
		{
			notifier.PropertyChanged += NotificationFired;
			view = bindable;
		}

		protected override void OnDetachingFrom(View bindable)
		{
			notifier.PropertyChanged -= NotificationFired;
		}

		private void NotificationFired(object sender, PropertyChangedEventArgs args)
		{
			action?.Invoke(args.PropertyName, view);
		}
	}
}

