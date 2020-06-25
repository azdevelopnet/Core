﻿using System;
using System.Diagnostics;
using System.Globalization;
using System.Reflection;
using System.Windows.Input;

namespace Xamarin.Forms.Core
{
	/// <summary>
	/// This behavior allows a ViewModel to turn any event exposed by a control into
	/// an ICommand which can be forwarded to the ViewModel.
	/// </summary>
	/// <example>
	/// <!--[CDATA[
	/// <Label Text="{Binding Text}"
	///        VerticalOptions="FillAndExpand" HorizontalOptions="FillAndExpand">
	///    <Label.Behaviors>
	///       <inf:EventToCommandBehavior
	///           EventName = "SizeChanged" Command="{Binding MyCommand}"
	///           EventArgsConverter="{StaticResource converter}"/>
	///    </Label.Behaviors>
	/// </Label>
	/// ]]>-->
	/// </example>
	public class EventToCommandBehavior : BindingContextBehavior<VisualElement>
	{
		EventInfo locatedEventInfo;
		Delegate eventHandler;

		/// <summary>
		/// Bindable property for the event name to hook into.
		/// </summary>
		public static readonly BindableProperty EventNameProperty = BindableProperty.Create(
			"EventName", typeof(string), typeof(EventToCommandBehavior),
			propertyChanged: OnEventNameChanged);

		/// <summary>
		/// Event name to hook
		/// </summary>
		/// <value>The name of the event.</value>
		public string EventName
		{
			get { return (string)GetValue(EventNameProperty); }
			set { SetValue(EventNameProperty, value); }
		}

		/// <summary>
		/// Bindable property for the ICommand to forward the event to.
		/// </summary>
		public static readonly BindableProperty CommandProperty = BindableProperty.Create(
			"Command", typeof(ICommand), typeof(EventToCommandBehavior));

		/// <summary>
		/// The ICommand implementation to call when the event is raised.
		/// </summary>
		/// <value>The command.</value>
		public ICommand Command
		{
			get { return (ICommand)GetValue(CommandProperty); }
			set { SetValue(CommandProperty, value); }
		}

		/// <summary>
		/// Bindable property for an optional parameter to send to the Command.
		/// </summary>
		public static readonly BindableProperty CommandParameterProperty = BindableProperty.Create(
			"CommandParameter", typeof(object), typeof(EventToCommandBehavior));

		/// <summary>
		/// Provides an optional piece of data for the command.
		/// This is only used if the EventArgsConverter is _not_ supplied.
		/// </summary>
		/// <value>The command parameter.</value>
		public object CommandParameter
		{
			get { return GetValue(CommandParameterProperty); }
			set { SetValue(CommandParameterProperty, value); }
		}

		/// <summary>
		/// Bindable property for an optional Sender+EventArgs > CommandParameter converter.
		/// </summary>
		public static readonly BindableProperty EventArgsConverterProperty = BindableProperty.Create(
			"EventArgsConverter", typeof(IValueConverter), typeof(EventToCommandBehavior));

		/// <summary>
		/// Converter which is passed the sender/EventArgs for the event; 
		/// returns the parameter value passed to the Command.
		/// </summary>
		/// <value>The command parameter.</value>
		public IValueConverter EventArgsConverter
		{
			get { return (IValueConverter)GetValue(EventArgsConverterProperty); }
			set { SetValue(EventArgsConverterProperty, value); }
		}

		/// <summary>
		/// Called when the behavior is attached to an element.
		/// </summary>
		/// <param name="bindable">Object we are attached to</param>
		protected override void OnAttachedTo(VisualElement bindable)
		{
			base.OnAttachedTo(bindable);
			Subscribe(AssociatedObject, EventName);
		}

		/// <summary>
		/// This is called when the behavior is being removed from
		/// a visual element.
		/// </summary>
		/// <param name="bindable">Bindable.</param> 
		protected override void OnDetachingFrom(VisualElement bindable)
		{
			Unsubscribe();
			base.OnDetachingFrom(bindable);
		}

		/// <summary>
		/// This is used to connect to the EventName on the passed target object.
		/// We wire up to a method in this instance named OnEventRaised.
		/// </summary>
		/// <param name="target">Target object</param>
		/// <param name="eventName">Name of the event to subscribe to</param>
		void Subscribe(object target, string eventName)
		{
			if (target == null || string.IsNullOrEmpty(eventName))
				return;

			// Lookup the named event on the associated object.
			locatedEventInfo = target.GetType().GetRuntimeEvent(eventName);
			if (locatedEventInfo == null)
			{
				throw new Exception($"Event {eventName} not found on {target}.");
			}

			// Wire up the event with reflection.
			MethodInfo methodInfo = typeof(EventToCommandBehavior).GetTypeInfo().GetDeclaredMethod("OnEventRaised");
			Debug.Assert(methodInfo != null);
			eventHandler = methodInfo.CreateDelegate(locatedEventInfo.EventHandlerType, this);
			locatedEventInfo.AddEventHandler(target, eventHandler);
		}

		/// <summary>
		/// Method to unsubscribe from the event on the target object.
		/// </summary>
		void Unsubscribe()
		{
			if (eventHandler == null)
				return;

			locatedEventInfo.RemoveEventHandler(AssociatedObject, eventHandler);
			eventHandler = null;
			locatedEventInfo = null;
		}

		/// <summary>
		/// This event handler is raised in response to the EventName.
		/// </summary>
		/// <param name="sender">View raising event</param>
		/// <param name="e">EventArgs</param>
		void OnEventRaised(object sender, EventArgs e)
		{
			if (Command != null)
			{
				object parameter;

				if (CommandParameter != null)
				{
					parameter = CommandParameter;
				}
				else
				{
					parameter = e;

				}

				if (Command.CanExecute(parameter))
					Command.Execute(parameter);
			}
#if DEBUG
			else
			{
				Debug.WriteLine($"EventToCommandBehavior: missing Command on event handler, {EventName}: Sender={sender}, EventArgs={e}");
			}
#endif
		}

		/// <summary>
		/// This is called when the EventName property is changed.
		/// </summary>
		/// <param name="bindable">EventToCommandBehavior</param>
		/// <param name="oldValue">Old event value.</param>
		/// <param name="newValue">New event value.</param>
		static void OnEventNameChanged(BindableObject bindable, object oldValue, object newValue)
		{
			((EventToCommandBehavior)bindable).OnEventNameChangedImpl((string)oldValue, (string)newValue);
		}

		/// <summary>
		/// This is an instance method clled when the EventName property is changed.
		/// </summary>
		/// <param name="oldValue">Old value.</param>
		/// <param name="newValue">New value.</param>
		void OnEventNameChangedImpl(string oldValue, string newValue)
		{
			Unsubscribe();
			Subscribe(AssociatedObject, newValue);
		}
	}
}
