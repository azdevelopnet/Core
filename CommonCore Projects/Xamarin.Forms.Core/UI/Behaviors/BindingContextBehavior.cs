using System;
using System.Diagnostics;

namespace Xamarin.Forms.Core
{
	/// <summary>
	/// Provides a Behavior(Of T) which forwards the associated object's binding
	/// context to the behavior. This is NOT done by default because behaviors can
	/// be shared if they are applied using a Style. In that case, there is not
	/// a 1:1 relationship. This enforces the 1:1 relationship but disallows the 
	/// application via a shared resource.
	/// </summary>
	public class BindingContextBehavior<T> : Behavior<T> where T : BindableObject
	{
		/// <summary>
		/// True if the binding context is being forwarded.
		/// </summary>
		bool bindingContextForwarded;

		/// <summary>
		/// The single object this behavior is bound to.
		/// </summary>
		/// <value>The associated object.</value>
		protected T AssociatedObject { get; set; }

		/// <summary>
		/// Called when the behavior is attached to an object.
		/// </summary>
		/// <param name="bindable">Bindable.</param>
		protected override void OnAttachedTo(T bindable)
		{
			// Disallow sharing of the behavior since we are associating 
			// to a single object and it's binding context.
			if (AssociatedObject != null)
			{
				throw new Exception(GetType() + " behaviors cannot be shared or used in a Style setter.");
			}

			base.OnAttachedTo(bindable);
			AssociatedObject = bindable;

			if (BindingContext == null)
			{
				bindingContextForwarded = true;
				BindingContext = bindable.BindingContext;
				bindable.BindingContextChanged += OnAssociatedBindingContextChanged;
			}
		}

		/// <summary>
		/// Called when this behavior is being detached from a bindable object.
		/// </summary>
		/// <param name="bindable">Bindable.</param>
		protected override void OnDetachingFrom(T bindable)
		{
			Debug.Assert(AssociatedObject == bindable);
			base.OnDetachingFrom(bindable);
			AssociatedObject = null;
			if (bindingContextForwarded)
			{
				bindable.BindingContextChanged -= OnAssociatedBindingContextChanged;
				BindingContext = null;
			}
		}

		/// <summary>
		/// Raised when our associated object's BindingContext changes.
		/// </summary>
		/// <param name="sender">Sender.</param>
		/// <param name="e">E.</param>
		void OnAssociatedBindingContextChanged(object sender, EventArgs e)
		{
			Debug.Assert(AssociatedObject == sender);
			Debug.Assert(bindingContextForwarded == true);
			this.BindingContext = ((BindableObject)sender).BindingContext;
		}
	}
}
